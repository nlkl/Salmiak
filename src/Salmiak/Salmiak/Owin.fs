module Salmiak.Owin

open System
open System.Collections.Generic
open System.IO
open System.Text
open System.Threading.Tasks

type SalmiakApplication<'T> = HttpAction<'T> -> HttpAction<'T> 

type AppFunc = Func<IDictionary<string, obj>, Task>
type MiddlewareFunc = Func<AppFunc, AppFunc>

let run application (env : IDictionary<string, obj>) = 
    async { 
        let responseStream = env.["owin.ResponseBody"] :?> Stream
        let responseWriter = new StreamWriter(responseStream, Encoding.UTF8)
        do! responseWriter.WriteAsync("Test!") |> Async.awaitPlainTask
        do! responseWriter.FlushAsync() |> Async.awaitPlainTask
    } |> Async.startAsPlainTask
    
let createAppFunc (application : SalmiakApplication<'T>) = AppFunc(run application)
let createMiddlewareFunc application = MiddlewareFunc(fun _ -> createAppFunc application)


