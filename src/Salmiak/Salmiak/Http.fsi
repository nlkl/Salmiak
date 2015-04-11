namespace Salmiak

type HttpVerb = Get | Post | Put | Delete

type HttpStatus =
     | HttpStatus of int 
     | HttpStatusWithPhrase of int * string

type HttpBody
type HttpRequest
type HttpResponse

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module HttpStatus =
    val ok200 : HttpStatus
    val unauthorized401 : HttpStatus
    val forbidden403 : HttpStatus
    val notFound404 : HttpStatus
    val internalServerError500 : HttpStatus

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module HttpBody = 
    val empty : HttpBody
    val ofString : body:string -> HttpBody
    val ofBytes : body:byte[] -> HttpBody
    val asString : body:HttpBody -> string
    val asBytes : body:HttpBody -> byte[]

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
    val getBody : request:HttpRequest -> HttpBody
    val getBodyAsBytes : request:HttpRequest -> byte[]
    val getBodyAsString : request:HttpRequest -> string
    val withBody: body:HttpBody -> request:HttpRequest -> HttpRequest
    val withBodyOfBytes : body:byte[] -> request:HttpRequest -> HttpRequest
    val withBodyOfString : body:string -> request:HttpRequest -> HttpRequest
    val withoutBody : request:HttpRequest -> HttpRequest
    val mapBody: mapping:(HttpBody -> HttpBody) -> request:HttpRequest -> HttpRequest

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module HttpResponse = 
    val make : verb:HttpStatus -> HttpResponse
    val getStatus : response:HttpResponse -> HttpStatus
    val withStatus : status:HttpStatus -> response:HttpResponse -> HttpResponse
    val getHeaders : response:HttpResponse -> seq<string * string>
    val getHeader : name:string -> response:HttpResponse -> string
    val tryGetHeader : name:string -> response:HttpResponse -> string option
    val containsHeader : name:string -> response:HttpResponse -> bool
    val withHeaders : headers:seq<string * string> -> response:HttpResponse -> HttpResponse
    val withHeader : name:string -> value:string -> response:HttpResponse -> HttpResponse
    val withoutHeader : name:string -> response:HttpResponse -> HttpResponse
    val mapHeaders : mapping:(string -> string -> string) -> response:HttpResponse -> HttpResponse
    val filterHeaders : predicate:(string -> string -> bool) -> response:HttpResponse -> HttpResponse
    val getBody : response:HttpResponse -> HttpBody
    val getBodyAsBytes : response:HttpResponse -> byte[]
    val getBodyAsString : response:HttpResponse -> string
    val withBody : body:HttpBody -> response:HttpResponse -> HttpResponse
    val withBodyOfBytes : body:byte[] -> response:HttpResponse -> HttpResponse
    val withBodyOfString : body:string -> response:HttpResponse -> HttpResponse
    val withoutBody : response:HttpResponse -> HttpResponse
    val mapBody: mapping:(HttpBody -> HttpBody) -> response:HttpResponse -> HttpResponse