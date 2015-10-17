using System.Collections.Generic;

namespace WebApiDescriptionGenerator
{
    public class WebApiDescription
    {
        public string HttpMethod { get; set; }
        public string RelativePath { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public IEnumerable<WebApiParameterDescription> ParameterDescriptions { get; set; }
        public TypeInfo ResponseType { get; set; }
    }
}
