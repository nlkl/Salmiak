namespace Salmiak

type HttpMethod = Get | Post | Put | Delete
type HttpStatusCode = HttpStatusCode of int

type HttpHeaders = Map<string, string>
type HttpBody = HttpBody of byte[]

type HttpRequest = HttpRequest of Url * HttpMethod * HttpHeaders * HttpBody
type HttpResponse = HttpResponse of HttpStatusCode * HttpHeaders * HttpBody
type HttpData<'T> = HttpData of HttpRequest * HttpResponse * 'T
type HttpAction<'T> = HttpAction of Async<'T>

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
    let placeholder () = failwith "Not implemented"

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module HttpResponse =
    let placeholder () = failwith "Not implemented"