namespace Salmiak

type RouteName = string
type RouteParameters = seq<string * string>
type RouteResolver = RouteParameters -> Url option

type RouteData = 
    { parameters : Map<string, string>
      resolvers : Map<RouteName, RouteResolver> }

type Route<'T, 'U> = 
    { name : RouteName
      predicate : Context<'T> -> RouteParameters option
      resolver : RouteResolver
      application : RouteData -> Application<'T, 'U> }

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module RouteData =
    let empty = 
        { parameters = Map.empty
          resolvers = Map.empty }
    
    let getParameters name data = Map.toSeq data.parameters
    let tryGetParameter name data = Map.tryFind name data.parameters
    let withParameters parameters data = { data with parameters = Map.ofSeq parameters }
    let withParameter name value data = { data with parameters = Map.add name value data.parameters }
    // TODO: Map, filter, maybe?

    let tryResolve routeName parameters data = 
        data.resolvers
        |> Map.tryFind routeName
        |> Option.bind (fun resolver -> resolver parameters)


module Routing =
    open System.Text.RegularExpressions
    open Salmiak.Utils

    let dispatch routes context =
        // TODO: Initialize with resolvers
        let routeData = RouteData.empty

        let tryFollowRoute route =
            match route.predicate context with
            | Some parameters -> 
                routeData
                |> RouteData.withParameters parameters
                |> route.application
                |> Some
            | None -> None
        
        match List.tryPick tryFollowRoute routes with
        | Some app -> app context
        | None ->
            context
            |> Context.withoutInfo
            |> Context.mapResponse (HttpResponse.withStatus HttpStatus.notFound404)
            |> Async.singleton

    let makeRoute name predicate application =
        { name = name
          predicate = predicate
          resolver = fun _ -> None // TODO: Implement resolvers
          application = application }

    let makeCatchAllRoute name application = 
        makeRoute name (fun _ -> Some Seq.empty) application

    let makeUrlRoute name predicate application =
        let routePredicate = Context.getRequest >> HttpRequest.getUrl >> predicate
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
               regex.GetGroupNames()
               |> Array.map (fun name -> (name, mtch.Groups.[name]))
               |> Array.filter (fun (_, grp) -> grp.Success)
               |> Array.map (fun (name, grp) -> (name, grp.Value))
               |> Array.toSeq
               |> Some
            else None
        makeUrlRoute name predicate application

    // TODO: Consider adding Sinatra like URLs here
    let makeSimpleRoute verb dynamicPath application = failwith "Not implemented"

