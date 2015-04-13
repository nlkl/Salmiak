namespace Salmiak

type Context<'T> =
    { request : HttpRequest
      response : HttpResponse
      data : 'T }

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Context =
    let make request response = 
        { request = request
          response = response
          data = () }

    let getRequest context = context.request
    let getResponse context = context.response
    let getData context = context.data
    let withRequest request context = { context with request = request }
    let withResponse response context = { context with response = response }

    let withData data context = 
        { request = context.request
          response = context.response
          data = data }

    let withoutData context = withData () context
    let mapRequest mapping context = { context with request = mapping context.request }
    let mapResponse mapping context = { context with response = mapping context.response }
    let mapData mapping context = withData (mapping context.data ) context


