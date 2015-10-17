namespace WebApiDescriptionGenerator
{
    public class TypePropertyInfo
    {
        public string Name { get; set; }
        public TypeInfo Type { get; set; }
        public bool IsEnumMember { get; set; }
        public int EnumValue { get; set; }
    }
}