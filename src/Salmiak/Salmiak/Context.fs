namespace Salmiak

type HttpContext<'T> =
    { request : HttpRequest
      response : HttpResponse
      info : 'T option }

type Application<'T, 'U> = HttpContext<'T> -> Async<HttpContext<'U>>

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module HttpContext =
    let make request response = 
        { request = request
          response = response
          info = None }

    let getRequest context = context.request
    let getResponse context = context.response
    let tryGetInfo context = context.info
    let withRequest request context = { context with request = request }
    let withResponse response context = { context with response = response }

    let withInfo info context = 
        { request = context.request
          response = context.response
          info = Some info }

    let withoutInfo context = 
        { request = context.request
          response = context.response
          info = None }

    let mapRequest mapping context = { context with request = mapping context.request }
    let mapResponse mapping context = { context with response = mapping context.response }

    let mapInfo mapping context = 
        { request = context.request
          response = context.response
          info = Option.map mapping context.info }


