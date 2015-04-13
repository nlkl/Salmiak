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

let makeResponse context =
    async {
        let request = Context.getRequest context
        let response = Context.getResponse context

        let requestHeaders = 
            request
            |> Req.getHeaders
            |> Seq.map (fun (name, value) -> sprintf "<li>%s: %s</li>" name value)
            |> String.concat ""

        let template = 
            "<html> \
                 <head><title>Salmiak</title></head> \
                 <body> \
                     <h3>Request headers:</h3> \
                     <ul>{0}</ul> \
                 </body> \
             </html>"

        let body = System.String.Format(template, requestHeaders)

        let response' =
            response
            |> Res.withBodyOfString body
            |> Res.withHeader "salmiak" "test"

        return Context.withResponse response' context
    }

let create () = addSalmiakHeader >>! makeResponse
