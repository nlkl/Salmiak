namespace Salmiak

type HttpBody = HttpBody of byte[]

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