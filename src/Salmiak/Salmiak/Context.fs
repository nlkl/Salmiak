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

    let getRequest action = action.request
    let getResponse action = action.response
    let getData action = action.data
    let withRequest request action = { action with request = request }
    let withResponse response action = { action with response = response }
    let withData data action = { action with data = data }
    let withoutData action = { action with data = () }


