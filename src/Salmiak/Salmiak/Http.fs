namespace Salmiak

type HttpMethod = Get | Post | Put | Delete
type HttpStatusCode = HttpStatusCode of int

type HttpUrl =
    { scheme : string
      host : string
      basePath : string
      path : string
      queryString : Map<string, string> }

type HttpRequest = 
    { url : HttpUrl
      verb : HttpMethod
      headers : Map<string, string>
      body : string }

type HttpResponse = 
    { status : HttpStatusCode
      headers : Map<string, string>
      body : string }

type HttpAction<'Data> =
    { request : HttpRequest
      response : HttpResponse
      customData : 'Data }

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module HttpRequest = failwith "Not implemented"

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module HttpResponse = failwith "Not implemented"