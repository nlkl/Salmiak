namespace Salmiak

type HttpVerb = Get | Post | Put | Delete
type HttpStatusCode = HttpStatusCode of int

type HttpHeaders = Map<string, string>

type HttpRequest

[<NoEquality; NoComparison>]
type HttpResponse = HttpResponse of HttpStatusCode * HttpHeaders * HttpBody

[<NoEquality; NoComparison>]
type HttpData<'T> = HttpData of HttpRequest * HttpResponse * 'T

[<NoEquality; NoComparison>]
type HttpAction<'T> = HttpAction of Async<'T>

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module HttpRequest = 
    val make : verb:HttpVerb -> url:Url -> HttpRequest
    val getVerb : request:HttpRequest -> HttpVerb
    val withVerb : verb:HttpVerb -> request:HttpRequest -> HttpRequest
    val getUrl : request:HttpRequest -> Url
    val withUrl : url:Url -> request:HttpRequest -> HttpRequest
    val getHeaders : request:HttpRequest -> seq<string * string>
    val getHeader : name:string -> request:HttpRequest -> string
    val tryGetHeader : name:string -> request:HttpRequest -> string option
    val containsHeader : name:string -> request:HttpRequest -> bool
    val withHeaders : headers:seq<string * string> -> request:HttpRequest -> HttpRequest
    val withHeader : name:string -> value:string -> request:HttpRequest -> HttpRequest
    val withoutHeader : name:string -> request:HttpRequest -> HttpRequest
    val mapHeaders : mapping:(string -> string -> string) -> request:HttpRequest -> HttpRequest
    val filterHeaders : predicate:(string -> string -> bool) -> request:HttpRequest -> HttpRequest
    val getBody : request:HttpRequest -> byte []
    val getBodyAsString : request:HttpRequest -> string
    val withBody : body:byte [] -> request:HttpRequest -> HttpRequest
    val withBodyOfString : body:string -> request:HttpRequest -> HttpRequest
    val withoutBody : request:HttpRequest -> HttpRequest
