namespace Salmiak

type Url =
    { scheme : string
      host : string
      basePath : string
      path : string
      queryString : Map<string, string> }

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Url = failwith "Not implemented"