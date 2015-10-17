# WebApiExtensions
ApiDescription Extensions to convert to a simplified structure that can be exposed for client consumption

When target Web Api services from a typescript project, one often needs to create types/model object structure in the client. In order to automate the process, this extension helps in simplifying the ApiDescription object so that it can be exposed for any automation tool that can create types. 


The library is available in Nuget: https://www.nuget.org/packages/WebApiExplorerExtensions/

In case of Api's that return IHttpActionResult, an additional attribute is required.


Sample Apis:

[Route("api/Person/{id}")]
public IEnumerable<Person> GetPerson(int id)
{
  .....
}

[Route("api/Login")]
[ReturnType(typeof(AuthenticationToken)]
public IHttpActionResult Login(string username, string password)
{
     ....
     return OK(token);
}

Additional Api required:

[Route("api/WebApiMetadata")]
public IEnumerable<WebApiDescription> GetWebApi()
{
     var apiDescriptions = Configuration.Services.GetApiExplorer().ApiDescriptions;
     return apiDescriptions.ToExternalDescriptions();
}


This library is used in conjunction with npm 'ts-webapi-ref' module that can be used in a gulp task to generate typescript code. 
