using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;

namespace WebApiDescriptionGenerator
{
    public static class Extensions
    {

        public static IEnumerable<WebApiDescription> ToExternalDescriptions(this IEnumerable<ApiDescription> apiDescriptions)
        {
            var descriptions = apiDescriptions.Where(description => !description.IsApiExplorer())
                .Select(description => description.ToExternalDescription())
                .ToList();


            var result = descriptions.ResolveConflicts();

            return result;
        }

        private static IEnumerable<WebApiDescription> ResolveConflicts(this IEnumerable<WebApiDescription> descriptions)
        {
            var webApiMap = new Dictionary<string, WebApiDescription>();

            foreach (var webApiDescription in descriptions)
            {
                if (!webApiMap.ContainsKey(webApiDescription.ActionName))
                {
                    webApiMap[webApiDescription.ActionName] = webApiDescription;
                }
                else
                {
                    var keyCount = 2;
                    var newKey = webApiMap.GetANewKey(webApiDescription.ActionName, () =>
                    {
                        var count = keyCount;
                        keyCount++;
                        return webApiDescription.ActionName + count;
                    });

                    webApiDescription.ActionName = newKey;
                    webApiMap[newKey] = webApiDescription;
                }
            }

            return webApiMap.Values;
        }

        private static TKey GetANewKey<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TKey> newKeyFromPreviouKeyFunc)
        {
            if (!dictionary.ContainsKey(key))
                return key;

            return dictionary.GetANewKey(newKeyFromPreviouKeyFunc(), newKeyFromPreviouKeyFunc);
        }


        public static WebApiDescription ToExternalDescription(this ApiDescription apiDescription)
        {

            var parameters = apiDescription.ParameterDescriptions.Select(description => new WebApiParameterDescription()
            {
                Name = description.ParameterDescriptor.ParameterName,
                Type = description.ParameterDescriptor.ParameterType.ToTypeInfo()
            });

            var returnType = GetReturnType(apiDescription).ToTypeInfo();

            return new WebApiDescription()
            {
                HttpMethod = apiDescription.HttpMethod.Method,
                ActionName = apiDescription.ActionDescriptor.ActionName,
                ControllerName = apiDescription.ActionDescriptor.ControllerDescriptor.ControllerName,
                ParameterDescriptions = parameters,
                RelativePath = apiDescription.RelativePath,
                ResponseType = returnType
            };

        }

        private static Type GetReturnType(ApiDescription apiDescription)
        {
            if (apiDescription.ActionDescriptor.ReturnType != typeof (IHttpActionResult))
            {
                return apiDescription.ActionDescriptor.ReturnType;
            }

            var attributes = apiDescription.ActionDescriptor.GetCustomAttributes<ReturnTypeAttribute>();
            if (!attributes.Any())
                return null;

            return attributes.First().ReturnType;

        }

        public static bool IsApiExplorer(this ApiDescription apiDescription)
        {
            var returnType = GetReturnType(apiDescription);
            return returnType == typeof(IEnumerable<WebApiDescription>);
        }


        private static TypeInfo ToTypeInfo(this Type type)
        {
            if (type == null)
                return null;

            IEnumerable<TypePropertyInfo> properties = null;

            if (!type.FullName.StartsWith("System"))
            {
                properties = type.IsEnum 
                    ? type.GetFields(BindingFlags.Static | BindingFlags.Public).Select(info => info.ToTypePropertyInfo()) 
                    : type.GetProperties().Select(info => info.ToTypePropertyInfo());
            }
            

            var genericArguments = type.GenericTypeArguments.Select(typeArg => typeArg.ToTypeInfo());

            var isArray = type.IsArray;
            var arrayElementType = isArray ? type.GetElementType().ToTypeInfo() : null;

            return new TypeInfo()
            {
                Name = type.Name,
                FullName = type.FullName,
                BaseType = type.BaseType.ToTypeInfo(),
                TypeArguments = genericArguments,
                IsEnum = type.IsEnum,
                IsArray = isArray,
                ArrayElementType = arrayElementType,
                Namespace = type.Namespace,
                Properties = properties
            };
        }

        private static TypePropertyInfo ToTypePropertyInfo(this PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                return null;

            return new TypePropertyInfo()
            {
                Name = propertyInfo.Name,
                Type = propertyInfo.PropertyType.ToTypeInfo()
            };
        }

        private static TypePropertyInfo ToTypePropertyInfo(this FieldInfo fieldInfo)
        {
            if (fieldInfo == null)
                return null;


            int rawValue;
            try
            {
                rawValue = Convert.ToInt32(fieldInfo.GetRawConstantValue());
            }
            catch
            {
                rawValue = 0;
            }


            return new TypePropertyInfo()
            {
                Name = fieldInfo.Name,
                IsEnumMember = true,         
                EnumValue = rawValue
            };
        }

    }
}