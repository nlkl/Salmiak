namespace Salmiak

type RouteParameters = seq<string * string>

type RouteResolver<'T> = 'T -> RouteParameters -> RelativeUrl option

type RouteContext<'T> = 
    { parameters : Map<string, string>
      resolvers : RouteResolver<'T> list }

type Route<'S, 'T, 'U> = HttpContext<'T> -> (RouteContext<'S> -> Async<HttpContext<'U>>) option

type Routing<'S, 'T, 'U> =
    { routes : Route<'S, 'T, 'U> list
      resolvers : RouteResolver<'S> list }

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module RouteContext =
    let empty = 
        { parameters = Map.empty
          resolvers = [] }
    
    let getParameters name context = Map.toSeq context.parameters
    let tryGetParameter name context = Map.tryFind name context.parameters
    let withParameters parameters context = { context with parameters = Map.ofSeq parameters }
    let withParameter name value context = { context with parameters = Map.add name value context.parameters }
    let withResolvers resolvers (context : RouteContext<'T>) = { context with resolvers = resolvers }
    // TODO: Map, filter, maybe?

    let tryResolve routeName parameters context = 
        Seq.tryPick (fun resolver -> resolver routeName parameters) context.resolvers

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Routing =
    open System.Text.RegularExpressions
    open Salmiak.Utils

    let init = 
        { routes = []
          resolvers = [] }

    let dispatch routing context =
        let routeContext = RouteContext.empty |> RouteContext.withResolvers routing.resolvers

        let tryFollowRoute route =
            match route context with
            | Some routeApp -> Some (routeApp routeContext)
            | None -> None
        
        match List.tryPick tryFollowRoute routing.routes with
        | Some app -> app
        | None ->
            context
            |> HttpContext.withoutInfo
            |> HttpContext.mapResponse (HttpResponse.withStatus HttpStatus.notFound404)
            |> Async.singleton

    let withResolver resolver (routing : Routing<'S, 'T, 'U>) =
        { routing with resolvers = resolver :: routing.resolvers }

    let withRoute selector application routing =
        let route context =
            match selector context with
            | Some parameters -> 
                let routeApp routeContext = 
                    let routeContext = RouteContext.withParameters parameters routeContext
                    application routeContext context
                Some routeApp
            | None -> None
        { routing with routes = route :: routing.routes }

    let withCatchAllRoute application routing = 
        withRoute (fun _ -> Some Seq.empty) application routing

    let withUrlRoute selector application routing =
        let routeSelector = HttpContext.getRequest >> HttpRequest.getUrl >> selector
        withRoute routeSelector application routing

    let withStaticRoute name path application routing =
        let selector url = if path = Url.getPath url then Some Seq.empty else None
        let resolver name' _ = if name' = name then Some (RelativeUrl.make path) else None
        routing
        |> withUrlRoute selector application
        |> withResolver resolver

    // TODO: Simple reversal
    let withRegexRoute pattern application routing =
        let selector url =
            let path = Url.getPath url
            let regex = Regex(pattern)
            let mtch = regex.Match(path)
            if mtch.Success then 
                let tryGetParameter (groupName : string) =
                    let group = mtch.Groups.[groupName]
                    if group.Success then Some (groupName, group.Value) else None
                regex.GetGroupNames()
                |> Seq.choose tryGetParameter
                |> Some
            else None
        withUrlRoute selector application routing

    // TODO: Consider adding Sinatra like URLs here
    let makeSimpleRoute name verb dynamicPath application = failwith "Not implemented"

