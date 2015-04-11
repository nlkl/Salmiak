namespace Salmiak.Sandbox

open Owin
open Microsoft.Owin.Hosting
open Salmiak

type Bootstrapper() = 
    member this.Configuration(app : IAppBuilder) = 
        let salmiakApplication = Application.create ()
        app.Use(Salmiak.Owin.createMiddlewareFunc salmiakApplication) |> ignore

module Program = 
    [<EntryPoint>]
    let main argv =
        let url = "http://localhost:8888"
        use host = WebApp.Start<Bootstrapper>(url)
        printfn "Application started at URL: %s" url
        System.Console.Read() |> ignore
        0 // return an integer exit code
