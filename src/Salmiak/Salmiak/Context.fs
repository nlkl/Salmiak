namespace Salmiak

type Context<'T> =
    { request : HttpRequest
      response : HttpResponse
      state : 'T option }

type Application<'T, 'U> = Context<'T> -> Async<Context<'U>>

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Context =
    let make request response = 
        { request = request
          response = response
          state = None }

    let getRequest context = context.request
    let getResponse context = context.response
    let tryGetState context = context.state
    let withRequest request context = { context with request = request }
    let withResponse response context = { context with response = response }

    let withState state context = 
        { request = context.request
          response = context.response
          state = Some state }

    let withoutState context = 
        { request = context.request
          response = context.response
          state = None }

    let mapRequest mapping context = { context with request = mapping context.request }
    let mapResponse mapping context = { context with response = mapping context.response }

    let mapState mapping context = 
        { request = context.request
          response = context.response
          state = Option.map mapping context.state }


