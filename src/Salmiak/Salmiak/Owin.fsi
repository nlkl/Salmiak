module Salmiak.Owin

open System
open System.Collections.Generic
open System.Threading.Tasks

type SalmiakApplication<'T> = HttpAction<'T> -> HttpAction<'T> 

type AppFunc = Func<IDictionary<string, obj>, Task>
type MiddlewareFunc = Func<AppFunc, AppFunc>

val createAppFunc : SalmiakApplication<'T> -> AppFunc
val createMiddlewareFunc : SalmiakApplication<'T> -> MiddlewareFunc