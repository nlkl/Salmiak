namespace Salmiak

type RoutePredicate<'T> = Context<'T> -> bool
type RouteMapping<'T, 'U> = RoutePredicate<'T> * Application<'T, 'U>
type RouteMap<'T, 'U> = RouteMapping<'T, 'U> list

module Routing =
    let route mappings context =
        mappings
        |> List.tryPick (fun (pred, app) -> if pred context then Some app else None)
        |> Option.map (fun app -> app context)
        |> Option.get // TODO: Handle not found routes

