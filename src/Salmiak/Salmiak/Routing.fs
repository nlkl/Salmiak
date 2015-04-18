namespace Salmiak

type RoutePredicate<'T> = Context<'T> -> bool
type Route<'T, 'U> = Route of RoutePredicate<'T> * Application<'T, 'U>

module Routing =
    open System.Text.RegularExpressions
    open Salmiak.Utils

    let dispatch routes context =
        let tryFollowRoute (Route (predicate, app)) =
            if predicate context then Some app else None

        match List.tryPick tryFollowRoute routes with
        | Some app -> app context
        | None ->
            context
            |> Context.withoutInfo
            |> Context.mapResponse (HttpResponse.withStatus HttpStatus.notFound404)
            |> Async.singleton

    let makeRoute predicate application = Route (predicate, application)
    let makeCatchAllRoute application = Route ((fun _ -> true), application)

    let makeUrlRoute predicate application =
        let routePredicate = Context.getRequest >> HttpRequest.getUrl >> predicate
        Route (routePredicate, application)

    let makeStaticRoute path application =
        let predicate url = path = Url.getPath url
        makeUrlRoute predicate application

    let makeRegexRoute pattern application =
        let predicate url =
            let path = Url.getPath url
            Regex.IsMatch(path, pattern)
        makeUrlRoute predicate application

    // TODO: Consider adding Sinatra like URLs here
    let makeSimpleRoute verb dynamicPath application = failwith "Not implemented"

