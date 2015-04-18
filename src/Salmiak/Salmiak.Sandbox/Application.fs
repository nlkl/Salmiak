module Salmiak.Sandbox.Application

open Salmiak
open Salmiak.Utils

module Req = HttpRequest
module Res = HttpResponse

let addSalmiakHeader context = 
    async {
        return context
        |> Context.mapResponse (Res.withHeader "X-Powered-By" "Salmiak")
    }

let baseTemplate = 
    sprintf "\
        <html> \
            <head><title>Salmiak</title></head> \
            <body> \
                <nav> \
                    <a href=\"/\">Home</a> | \
                    <a href=\"/headers\">Headers</a> \
                </nav> \
                %s \
            </body> \
        </html>"

let home context =
    async {
        let template = "\
                <h3>Salmiak playground</h3> \
                <p>This is just a test.</p>"

        let body = baseTemplate template

        return Context.mapResponse (Res.withBodyOfString body) context
    }

let viewHeaders context =
    async {
        let request = Context.getRequest context
        let response = Context.getResponse context

        let formatHeaders headers = 
            headers
            |> Seq.map (fun (name, value) -> sprintf "<li>%s: %s</li>" name value)
            |> String.concat ""

        let requestHeaders = request |> Req.getHeaders |> formatHeaders
        let responseHeaders = response |> Res.getHeaders |> formatHeaders

        let template = 
            sprintf "\
                <h3>Request headers:</h3> \
                <ul>%s</ul> \
                <h3>Response headers:</h3> \
                <ul>%s</ul>"

        let body = baseTemplate (template requestHeaders responseHeaders)

        return Context.mapResponse (Res.withBodyOfString body) context
    }

let create () = 
    // Main application
    let app =
        Routing.dispatch
            [ Routing.makeStaticRoute "/" home
              Routing.makeStaticRoute "/headers" viewHeaders ]

    // Application pipeline: middleware and whatnot
    addSalmiakHeader >>! app