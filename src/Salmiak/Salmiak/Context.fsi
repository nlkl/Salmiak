namespace Salmiak

type HttpContext<'T>
type Application<'T, 'U> = HttpContext<'T> -> Async<HttpContext<'U>>

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module HttpContext =
    val make : request:HttpRequest -> response:HttpResponse -> HttpContext<'T>
    val getRequest : context:HttpContext<'T> -> HttpRequest
    val getResponse : context:HttpContext<'T> -> HttpResponse
    val tryGetInfo : context:HttpContext<'T> -> 'T option
    val withRequest : request:HttpRequest -> context:HttpContext<'T> -> HttpContext<'T>
    val withResponse : response:HttpResponse -> context:HttpContext<'T> -> HttpContext<'T>
    val withInfo : info:'U -> context:HttpContext<'T> -> HttpContext<'U>
    val withoutInfo : context:HttpContext<'T> -> HttpContext<'U>
    val mapRequest : mapping:(HttpRequest -> HttpRequest) -> context:HttpContext<'T> -> HttpContext<'T>
    val mapResponse : mapping:(HttpResponse -> HttpResponse) -> context:HttpContext<'T> -> HttpContext<'T>
    val mapInfo : mapping:('T -> 'U) -> context:HttpContext<'T> -> HttpContext<'U> 
