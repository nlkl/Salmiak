namespace Salmiak

type HttpVerb = Get | Post | Put | Delete
type HttpStatusCode = HttpStatusCode of int

type HttpHeaders = Map<string, string>

type HttpRequest = 
    { verb : HttpVerb
      url : Url  
      headers : HttpHeaders
      body : HttpBody }

type HttpResponse = HttpResponse of HttpStatusCode * HttpHeaders * HttpBody
type HttpData<'T> = HttpData of HttpRequest * HttpResponse * 'T
type HttpAction<'T> = HttpAction of Async<'T>

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module HttpRequest =
    let make verb url =
        { verb = verb
          url = url
          headers = Map.empty
          body = HttpBody.empty }

    let getVerb request = request.verb
    let withVerb verb request = { request with verb = verb }

    let getUrl request = request.url
    let withUrl url request = { request with url = url }

    let getHeaders request = Map.toSeq request.headers
    let getHeader name request = Map.find name request.headers
    let tryGetHeader name request = Map.tryFind name request.headers
    let containsHeader name request = Map.containsKey name request.headers

    let withHeaders headers request = { request with headers = Seq.fold (fun hs (name, value) -> Map.add name value hs) request.headers headers }
    let withHeader name value request = { request with headers = Map.add name value request.headers } 
    let withoutHeader name request = { request with headers = Map.remove name request.headers }
    let mapHeaders mapping request = { request with headers = Map.map mapping request.headers }
    let filterHeaders predicate request = { request with headers = Map.filter predicate request.headers }

    let getBody request = HttpBody.asBytes request.body
    let getBodyAsString request = HttpBody.asString request.body
    let withBody body request = { request with body = HttpBody.ofBytes body }
    let withBodyOfString body request = { request with body = HttpBody.ofString body }
    let withoutBody request = { request with body = HttpBody.empty }
    // TODO: Implement async versions
    // TODO: Implement other body types

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module HttpResponse =
    let placeholder () = failwith "Not implemented"