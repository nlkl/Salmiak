namespace Salmiak

type RouteName = string
type RouteParameters = seq<string * string>
type RouteData
type Route<'T, 'U>

module Routing =
    val dispatch : routes:(Route<'T, 'U> list) ->  Application<'T, 'U>
    val makeRoute : name:RouteName -> predicate:(Context<'T> -> RouteParameters option) -> application:(RouteData -> Application<'T, 'U>) -> Route<'T, 'U>
    val makeCatchAllRoute : name:RouteName -> application:(RouteData -> Application<'T, 'U>) -> Route<'T, 'U>
    val makeUrlRoute : name:RouteName -> predicate:(Url -> RouteParameters option) -> application:(RouteData -> Application<'T, 'U>) -> Route<'T, 'U>
    val makeStaticRoute : name:RouteName -> path:string -> application:(RouteData -> Application<'T, 'U>) -> Route<'T, 'U>
    val makeRegexRoute : name:RouteName -> pattern:string -> application:(RouteData -> Application<'T, 'U>) -> Route<'T, 'U>