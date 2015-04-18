namespace Salmiak

type UrlScheme = Http | Https
type Url

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Url =
    val make : scheme:UrlScheme -> host:string -> basePath:string -> path:string -> Url
    val getScheme : url:Url -> UrlScheme
    val getHost : url:Url -> string
    val getBasePath : url:Url -> string
    val getPath : url:Url -> string
    val getFullPath : url:Url -> string
    val getQueryParameters : url:Url -> seq<string * string>
    val tryGetQueryParameter : name:string -> url:Url -> string option
    val containsQueryParameter : name:string -> url:Url -> bool
    val getQueryString : url:Url -> string
    val getUrlString : url:Url -> string
    val withScheme : scheme:UrlScheme -> url:Url -> Url
    val withHost : host:string -> url:Url -> Url
    val withBasePath : basePath:string -> url:Url -> Url
    val withPath : path:string -> url:Url -> Url
    val withQueryParameters : parameters:seq<string * string> -> url:Url -> Url
    val withQueryParameter : name:string -> value:string -> url:Url -> Url
    val withoutQueryParameter : name:string -> url:Url -> Url
    val mapQueryParameters : mapping:(string -> string -> string) -> url:Url -> Url
    val filterQueryParameters : predicate:(string -> string -> bool) -> url:Url -> Url
