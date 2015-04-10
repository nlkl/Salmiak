namespace Salmiak

type HttpBody

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module HttpBody = 
    val empty : HttpBody
    val ofString : body:string -> HttpBody
    val ofBytes : body:byte[] -> HttpBody
    val asString : body:HttpBody -> string
    val asBytes : body:HttpBody -> byte[]