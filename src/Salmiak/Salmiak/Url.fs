namespace Salmiak

type UrlScheme = Http | Https

type RelativeUrl =
    { path : string
      queryParameters : Map<string, string> }

type Url =
    { scheme : UrlScheme
      host : string
      basePath : string
      path : string
      queryParameters : Map<string, string> }

// TODO: RelativeUrl
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module RelativeUrl =
    let make path : RelativeUrl =
        { path = path
          queryParameters = Map.empty }

    let getPath (url : RelativeUrl) = url.path

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Url =
    let make scheme host basePath path =
        { scheme = scheme
          host = host 
          basePath = basePath
          path = path
          queryParameters = Map.empty }

    let getScheme (url : Url) = url.scheme
    let getHost (url : Url) = url.host
    let getBasePath (url : Url) = url.basePath
    let getPath (url : Url) = url.path
    let getFullPath (url : Url) = url.basePath + url.path
    let getQueryParameters (url : Url) = url.queryParameters |> Map.toSeq
    let tryGetQueryParameter name (url : Url) = Map.tryFind name url.queryParameters
    let containsQueryParameter name (url : Url) = Map.containsKey name url.queryParameters
    
    let getQueryString (url : Url) =
        url.queryParameters
        |> Map.toSeq
        |> Seq.map (fun (name, value) -> sprintf "%s=%s" name value)
        |> String.concat "&"

    let getUrlString (url : Url) = 
        let scheme = 
            match url.scheme with
            | Http -> "http"
            | Https -> "https"
        let queryString = 
            if Map.isEmpty url.queryParameters then ""
            else "?" + getQueryString url
        sprintf "%s://%s%s%s%s" scheme url.host url.basePath url.path queryString

    let withScheme scheme (url : Url) = { url with scheme = scheme }
    let withHost host (url : Url) = { url with host = host }
    let withBasePath basePath (url : Url) = { url with basePath = basePath }
    let withPath path (url : Url) = { url with path = path }
    let withQueryParameters parameters (url : Url) = { url with queryParameters = Map.ofSeq parameters }
    let withQueryParameter name value (url : Url) = { url with queryParameters = Map.add name value url.queryParameters }
    let withoutQueryParameter name (url : Url) = { url with queryParameters = Map.remove name url.queryParameters }
    let mapQueryParameters mapping (url : Url) = { url with queryParameters = Map.map mapping url.queryParameters }
    let filterQueryParameters predicate (url : Url) = { url with queryParameters = Map.filter predicate url.queryParameters }