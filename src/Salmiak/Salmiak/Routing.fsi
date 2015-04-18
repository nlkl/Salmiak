namespace Salmiak

type Route<'T, 'U>

module Routing =
    val dispatch : routes:(Route<'T, 'U> list) -> Application<'T, 'U>
    val makeRoute : predicate:(Context<'T> -> bool) -> application:Application<'T, 'U> -> Route<'T, 'U>
    val makeCatchAllRoute : application:Application<'T, 'U> -> Route<'T, 'U>
    val makeUrlRoute : predicate:(Url -> bool) -> application:Application<'T, 'U> -> Route<'T, 'U>
    val makeStaticRoute : path:string -> application:Application<'T, 'U> -> Route<'T, 'U>
    val makeRegexRoute : pattern:string -> application:Application<'T, 'U> -> Route<'T, 'U>