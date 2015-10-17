using System;
using System.Collections.Generic;

namespace WebApiDescriptionGenerator
{
    public class TypeInfo
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Namespace { get; set; }
        public TypeInfo BaseType { get; set; }
        public bool IsEnum { get; set; }
        public IEnumerable<TypeInfo> TypeArguments { get; set; }
        public IEnumerable<TypePropertyInfo> Properties { get; set; }
    }
}