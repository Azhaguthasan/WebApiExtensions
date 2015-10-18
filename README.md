# WebApiExtensions
ApiDescription Extensions to convert to a simplified structure that can be exposed for client consumption

When targeting Web Api services from a typescript project, one often needs to create types/model object structure in the client. To automate the process of creating the types, this extension helps the Web Api Service to provide additional endpoint that can expose all the API descriptions. This extension applies over the existing .Net System.Web.Http.ApiDescription and simplifies it so that it can be exposed in a separate end point for any automation tool that can create types. 

The library is available in Nuget: https://www.nuget.org/packages/WebApiExplorerExtensions/

In case of an Api that returns IHttpActionResult, an additional attribute is required to specify the return type.


###Sample Api:
```C#
[Route("api/Person/{id}")]
public IEnumerable<Person> GetPerson(int id)  {
  .....
}

[Route("api/Login")]
[ReturnType(typeof(AuthenticationToken)]
public IHttpActionResult Login(string username, string password)
{
     ....
     return OK(token);
}
```


###Additional Api required:

```C# 
[Route("api/WebApiMetadata")]  
public IEnumerable<WebApiDescription> GetWebApi()  
{  
     var apiDescriptions = Configuration.Services.GetApiExplorer().ApiDescriptions;  
     return apiDescriptions.ToExternalDescriptions();  
}
```


This library is used in conjunction with npm 'ts-webapi-ref' module that can be used in a gulp task to generate typescript code. See here for more details: https://www.npmjs.com/package/ts-webapi-ref 
