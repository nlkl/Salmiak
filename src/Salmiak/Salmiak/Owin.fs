namespace Salmiak

module Owin =
    open System
    open System.Collections.Generic
    open System.IO
    open System.Text
    open System.Threading.Tasks

    type AppFunc = Func<IDictionary<string, obj>, Task>
    type MiddlewareFunc = Func<AppFunc, AppFunc>

    let private run (env : IDictionary<string, obj>) = 
        async { 
            let responseStream = env.["owin.ResponseBody"] :?> Stream
            let responseWriter = new StreamWriter(responseStream, Encoding.UTF8)
            do! responseWriter.WriteAsync("Test!") |> Async.awaitPlainTask
            do! responseWriter.FlushAsync() |> Async.awaitPlainTask
        } |> Async.startAsPlainTask
    
    let createAppFunc() = AppFunc(run)
    let createMiddlewareFunc() = MiddlewareFunc(ignore >> createAppFunc)


