﻿namespace Salmiak.Sandbox

open Owin
open Microsoft.Owin.Hosting

type Bootstrapper() = 
    member this.Configuration(app : IAppBuilder) = app.Use(Salmiak.Owin.createMiddlewareFunc id) |> ignore

module Program = 
    [<EntryPoint>]
    let main argv =
        let url = "http://localhost:8888"
        use host = WebApp.Start<Bootstrapper>(url)
        printfn "Application started at URL: %s" url
        System.Console.Read() |> ignore
        0 // return an integer exit code
