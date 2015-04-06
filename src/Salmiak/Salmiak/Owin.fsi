module Salmiak.Owin

open System
open System.Collections.Generic
open System.Threading.Tasks

type SalmiakApplication<'T, 'U> = HttpAction<'T> -> HttpAction<'U> 

type AppFunc = Func<IDictionary<string, obj>, Task>
type MiddlewareFunc = Func<AppFunc, AppFunc>

val createAppFunc : SalmiakApplication<unit, 'T> -> AppFunc
val createMiddlewareFunc : SalmiakApplication<unit, 'T> -> MiddlewareFunc