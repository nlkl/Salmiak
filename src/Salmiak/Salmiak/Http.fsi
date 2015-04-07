namespace Salmiak

type HttpMethod = Get | Post | Put | Delete
type HttpStatusCode = HttpStatusCode of int

type HttpHeaders = Map<string, string>

type HttpBody

[<NoEquality; NoComparison>]
type HttpRequest = HttpRequest of Url * HttpMethod * HttpHeaders * HttpBody

[<NoEquality; NoComparison>]
type HttpResponse = HttpResponse of HttpStatusCode * HttpHeaders * HttpBody

[<NoEquality; NoComparison>]
type HttpData<'T> = HttpData of HttpRequest * HttpResponse * 'T

[<NoEquality; NoComparison>]
type HttpAction<'T> = HttpAction of Async<'T>

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module HttpBody = 
    val empty : HttpBody
    val ofString : body:string -> HttpBody
    val ofBytes : body:byte[] -> HttpBody
    val asString : body:HttpBody -> string
    val asBytes : body:HttpBody -> byte[]
