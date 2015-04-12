namespace Salmiak

type HttpVerb = Get | Post | Put | Delete

type HttpStatus =
     | HttpStatus of int 
     | HttpStatusWithPhrase of int * string

type HttpBody = HttpBody of byte[]
type HttpHeaders = Map<string, string>

type HttpRequest = 
    { verb : HttpVerb
      url : Url  
      headers : HttpHeaders
      body : HttpBody }

type HttpResponse =
    { status : HttpStatus
      headers : HttpHeaders
      body : HttpBody }

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module HttpStatus =
    let ok200 = HttpStatusWithPhrase(200, "OK")
    let unauthorized401 = HttpStatusWithPhrase(401, "Unauthorized")
    let forbidden403 = HttpStatusWithPhrase(403, "Forbidden")
    let notFound404 = HttpStatusWithPhrase(404, "Not Found")
    let internalServerError500 = HttpStatusWithPhrase(500, "Internal Server Error")

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module HttpBody =
    open System.Text

    let getUtf8Bytes (str : string) = Encoding.UTF8.GetBytes str
    let getUtf8String (bytes : byte[]) = Encoding.UTF8.GetString bytes

    let empty = HttpBody Array.empty

    let ofString (body : string) = HttpBody (getUtf8Bytes body)
    let ofBytes (body : byte[]) = HttpBody body
    let asString (HttpBody bytes) = getUtf8String bytes
    let asBytes (HttpBody bytes) = bytes

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module HttpRequest =
    type Req = HttpRequest

    let make verb url =
        { verb = verb
          url = url
          headers = Map.empty
          body = HttpBody.empty }

    let getVerb request = request.verb
    let withVerb verb request = { request with verb = verb }

    let getUrl request = request.url
    let withUrl url request = { request with url = url }

    let getHeaders (request : Req) = Map.toSeq request.headers
    let getHeader name (request : Req) = Map.find name request.headers
    let tryGetHeader name (request : Req) = Map.tryFind name request.headers
    let containsHeader name (request : Req) = Map.containsKey name request.headers
    let withHeaders headers (request : Req) = { request with headers = Map.ofSeq headers }
    let withHeader name value (request : Req) = { request with headers = Map.add name value request.headers } 
    let withoutHeader name (request : Req) = { request with headers = Map.remove name request.headers }
    let mapHeaders mapping (request : Req) = { request with headers = Map.map mapping request.headers }
    let filterHeaders predicate (request : Req) = { request with headers = Map.filter predicate request.headers }

    let getBody (request : Req) = request.body
    let getBodyAsBytes (request : Req) = HttpBody.asBytes request.body
    let getBodyAsString (request : Req) = HttpBody.asString request.body
    let withBody body (request : Req) = { request with body = body }
    let withBodyOfBytes body (request : Req) = { request with body = HttpBody.ofBytes body }
    let withBodyOfString body (request : Req) = { request with body = HttpBody.ofString body }
    let withoutBody (request : Req) = { request with body = HttpBody.empty }
    let mapBody mapping (request : Req) = { request with body = mapping request.body }

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module HttpResponse =
    type Res = HttpResponse

    let make status =
        { status = status
          headers = Map.empty
          body = HttpBody.empty }

    let getStatus response = response.status
    let withStatus status response = { response with status = status }

    let getHeaders (response : Res) = Map.toSeq response.headers
    let getHeader name (response : Res) = Map.find name response.headers
    let tryGetHeader name (response : Res) = Map.tryFind name response.headers
    let containsHeader name (response : Res) = Map.containsKey name response.headers
    let withHeaders headers (response : Res) = { response with headers = Seq.fold (fun hs (name, value) -> Map.add name value hs) response.headers headers }
    let withHeader name value (response : Res) = { response with headers = Map.add name value response.headers } 
    let withoutHeader name (response : Res) = { response with headers = Map.remove name response.headers }
    let mapHeaders mapping (response : Res) = { response with headers = Map.map mapping response.headers }
    let filterHeaders predicate (response : Res) = { response with headers = Map.filter predicate response.headers }

    let getBody (response : Res) = response.body
    let getBodyAsBytes (response : Res) = HttpBody.asBytes response.body
    let getBodyAsString (response : Res) = HttpBody.asString response.body
    let withBody body (response : Res) = { response with body = body }
    let withBodyOfBytes body (response : Res) = { response with body = HttpBody.ofBytes body }
    let withBodyOfString body (response : Res) = { response with body = HttpBody.ofString body }
    let withoutBody (response : Res) = { response with body = HttpBody.empty }
    let mapBody mapping (response : Res) = { response with body = mapping response.body }