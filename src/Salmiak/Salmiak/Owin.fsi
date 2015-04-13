module Salmiak.Owin

open System
open System.Collections.Generic
open System.Threading.Tasks

type AppFunc = Func<IDictionary<string, obj>, Task>
type MiddlewareFunc = Func<AppFunc, AppFunc>

val createAppFunc : Application<unit, 'T> -> AppFunc
val createMiddlewareFunc : Application<unit, 'T> -> MiddlewareFunc