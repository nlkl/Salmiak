module Salmiak.Sandbox.Application

open Salmiak

let init (HttpAction(request, response, context)) =
    let (HttpResponse(status, headers, body)) = response
    let response' = HttpResponse(status, headers, "TEST!")
    HttpAction(request, response', context)