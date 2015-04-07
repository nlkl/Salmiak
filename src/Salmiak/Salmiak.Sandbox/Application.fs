module Salmiak.Sandbox.Application

open Salmiak

let init (HttpAction asyncData) =
    async {
        let! HttpData(request, response, context) = asyncData
        let (HttpResponse(status, headers, body)) = response
        let response' = HttpResponse(status, headers, HttpBody.ofString "TEST!")
        return HttpData(request, response', context)
    } |> HttpAction