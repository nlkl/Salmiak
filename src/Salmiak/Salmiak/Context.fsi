﻿namespace Salmiak

type Context<'T>
type Application<'T, 'U> = Context<'T> -> Async<Context<'U>>

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Context =
    val make : request:HttpRequest -> response:HttpResponse -> Context<'T>
    val getRequest : context:Context<'T> -> HttpRequest
    val getResponse : context:Context<'T> -> HttpResponse
    val tryGetInfo : context:Context<'T> -> 'T option
    val withRequest : request:HttpRequest -> context:Context<'T> -> Context<'T>
    val withResponse : response:HttpResponse -> context:Context<'T> -> Context<'T>
    val withInfo : info:'U -> context:Context<'T> -> Context<'U>
    val withoutInfo : context:Context<'T> -> Context<'U>
    val mapRequest : mapping:(HttpRequest -> HttpRequest) -> context:Context<'T> -> Context<'T>
    val mapResponse : mapping:(HttpResponse -> HttpResponse) -> context:Context<'T> -> Context<'T>
    val mapInfo : mapping:('T -> 'U) -> context:Context<'T> -> Context<'U> 
