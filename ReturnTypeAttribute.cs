using System;

namespace WebApiDescriptionGenerator
{
    public class ReturnTypeAttribute : Attribute
    {
        public Type ReturnType { get; private set; }

        public ReturnTypeAttribute(Type returnType)
        {
            ReturnType = returnType;
        }
    }
}