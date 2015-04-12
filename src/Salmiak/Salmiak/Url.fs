namespace Salmiak

type UrlScheme = Http | Https

type Url =
    { scheme : UrlScheme
      host : string
      basePath : string
      path : string
      queryParameters : Map<string, string> }

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Url =
    let make scheme host basePath path =
        { scheme = scheme
          host = host 
          basePath = basePath
          path = path
          queryParameters = Map.empty }

    let getScheme url = url.scheme
    let getHost url = url.host
    let getBasePath url = url.basePath
    let getPath url = url.path
    let getFullPath url = url.basePath + url.path
    let getQueryParameters url = url.queryParameters |> Map.toSeq
    let getQueryParameter name url = Map.find name url.queryParameters
    let tryGetQueryParameter name url = Map.tryFind name url.queryParameters
    let containsQueryParameter name url = Map.containsKey name url.queryParameters
    
    let getQueryString url =
        url.queryParameters
        |> Map.toSeq
        |> Seq.map (fun (name, value) -> sprintf "%s=%s" name value)
        |> String.concat "&"

    let getUrlString url = 
        let scheme = 
            match url.scheme with
            | Http -> "http"
            | Https -> "https"
        let queryString = 
            if Map.isEmpty url.queryParameters then ""
            else "?" + getQueryString url
        sprintf "%s://%s%s%s%s" scheme url.host url.basePath url.path queryString

    let withScheme scheme url = { url with scheme = scheme }
    let withHost host url = { url with host = host }
    let withBasePath basePath url = { url with basePath = basePath }
    let withPath path url = { url with path = path }
    let withQueryParameters parameters url = { url with queryParameters = Map.ofSeq parameters }
    let withQueryParameter name value url = { url with queryParameters = Map.add name value url.queryParameters }
    let withoutQueryParameter name url = { url with queryParameters = Map.remove name url.queryParameters }
    let mapQueryParameters mapping url = { url with queryParameters = Map.map mapping url.queryParameters }
    let filterQueryParameters predicate url = { url with queryParameters = Map.filter predicate url.queryParameters }