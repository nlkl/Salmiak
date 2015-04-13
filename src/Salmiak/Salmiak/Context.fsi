namespace Salmiak

type Context<'T>

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Context =
    val make : request:HttpRequest -> response:HttpResponse -> Context<unit>
    val getRequest : context:Context<'T> -> HttpRequest
    val getResponse : context:Context<'T> -> HttpResponse
    val getData : context:Context<'T> -> 'T
    val withRequest : request:HttpRequest -> context:Context<'T> -> Context<'T>
    val withResponse : response:HttpResponse -> context:Context<'T> -> Context<'T>
    val withData : data:'U -> context:Context<'T> -> Context<'U>
    val withoutData : context:Context<'T> -> Context<unit>
    val mapRequest : mapping:(HttpRequest -> HttpRequest) -> context:Context<'T> -> Context<'T>
    val mapResponse : mapping:(HttpResponse -> HttpResponse) -> context:Context<'T> -> Context<'T>
    val mapData : mapping:('T -> 'U) -> context:Context<'T> -> Context<'U> 
