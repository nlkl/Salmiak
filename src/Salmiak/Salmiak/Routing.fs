namespace Salmiak

type RouteName = string
type RouteParameters = seq<string * string>
type RouteResolver = RouteParameters -> Url option

type RouteContext = 
    { parameters : Map<string, string>
      resolvers : Map<RouteName, RouteResolver> }

type Route<'T, 'U> = 
    { name : RouteName
      predicate : HttpContext<'T> -> RouteParameters option
      resolver : RouteResolver
      application : RouteContext -> Application<'T, 'U> }

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module RouteContext =
    let empty = 
        { parameters = Map.empty
          resolvers = Map.empty }
    
    let getParameters name context = Map.toSeq context.parameters
    let tryGetParameter name context = Map.tryFind name context.parameters
    let withParameters parameters context = { context with parameters = Map.ofSeq parameters }
    let withParameter name value context = { context with parameters = Map.add name value context.parameters }
    // TODO: Map, filter, maybe?

    let tryResolve routeName parameters context = 
        context.resolvers
        |> Map.tryFind routeName
        |> Option.bind (fun resolver -> resolver parameters)


module Routing =
    open System.Text.RegularExpressions
    open Salmiak.Utils

    let dispatch routes context =
        // TODO: Initialize with resolvers
        let routeContext = RouteContext.empty

        let tryFollowRoute route =
            match route.predicate context with
            | Some parameters -> 
                routeContext
                |> RouteContext.withParameters parameters
                |> route.application
                |> Some
            | None -> None
        
        match List.tryPick tryFollowRoute routes with
        | Some app -> app context
        | None ->
            context
            |> HttpContext.withoutInfo
            |> HttpContext.mapResponse (HttpResponse.withStatus HttpStatus.notFound404)
            |> Async.singleton

    let makeRoute name predicate application =
        { name = name
          predicate = predicate
          resolver = fun _ -> None // TODO: Implement resolvers
          application = application }

    let makeCatchAllRoute name application = 
        makeRoute name (fun _ -> Some Seq.empty) application

    let makeUrlRoute name predicate application =
        let routePredicate = HttpContext.getRequest >> HttpRequest.getUrl >> predicate
        makeRoute name routePredicate application 

    let makeStaticRoute name path application =
        let predicate url = if path = Url.getPath url then Some Seq.empty else None
        makeUrlRoute name predicate application 

    let makeRegexRoute name pattern application =
        let predicate url =
            let path = Url.getPath url
            let regex = Regex(pattern)
            let mtch = regex.Match(path)
            if mtch.Success then 
                let tryGetParameter (groupName : string) =
                    let group = mtch.Groups.[groupName]
                    if group.Success then Some (groupName, group.Value)
                    else None
                regex.GetGroupNames()
                |> Seq.choose tryGetParameter
                |> Some
            else None
        makeUrlRoute name predicate application

    // TODO: Consider adding Sinatra like URLs here
    let makeSimpleRoute name verb dynamicPath application = failwith "Not implemented"

