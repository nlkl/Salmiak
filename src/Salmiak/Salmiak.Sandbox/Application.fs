module Salmiak.Sandbox.Application

open Salmiak

let init (HttpAction asyncData) =
    async {
        let! HttpData(request, response, context) = asyncData

        let requestHeaders = 
            request
            |> HttpRequest.getHeaders
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
            |> HttpResponse.withBodyOfString body
            |> HttpResponse.withHeader "salmiak" "test"

        return HttpData(request, response', context)
    } |> HttpAction