namespace Salmiak

type HttpMethod = Get | Post | Put | Delete
type HttpStatusCode = HttpStatusCode of int

type HttpHeaders = Map<string, string>
type HttpBody = string

type HttpRequest = HttpRequest of Url * HttpMethod * HttpHeaders * HttpBody
type HttpResponse = HttpResponse of HttpStatusCode * HttpHeaders * HttpBody
type HttpAction<'T> = HttpAction of HttpRequest * HttpResponse * 'T

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module HttpRequest = failwith "Not implemented"

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module HttpResponse = failwith "Not implemented"