module Salmiak.Owin

open System
open System.Collections.Generic
open System.IO
open System.Text
open System.Threading.Tasks

type SalmiakApplication<'T, 'U> = Context<'T>  -> Async<Context<'U>> 

type AppFunc = Func<IDictionary<string, obj>, Task>
type MiddlewareFunc = Func<AppFunc, AppFunc>

let readBytesAsync (stream : Stream) =
    async {
        use memoryStream = new MemoryStream()
        do! stream.CopyToAsync(memoryStream) |> Async.awaitPlainTask
        return memoryStream.ToArray()
    }

let writeBytesAsync bytes (stream : Stream) =
    async {
        do! stream.WriteAsync(bytes, 0, Array.length bytes) |> Async.awaitPlainTask
        do! stream.FlushAsync() |> Async.awaitPlainTask
    }

let mapHeader (KeyValue(key, values)) = (key, String.concat ", " values)

let initializeRequest (env : IDictionary<string, obj>) = 
    async {
        let owinHeaders = env.["owin.RequestHeaders"] :?> IDictionary<string, string[]>
        let owinVerb = env.["owin.RequestMethod"] :?> string
        let owinBasePath = env.["owin.RequestPathBase"] :?> string
        let owinPath = env.["owin.RequestPath"] :?> string
        let owinProtocol = env.["owin.RequestProtocol"] :?> string
        let owinScheme = env.["owin.RequestScheme"] :?> string
        let owinQueryString = env.["owin.RequestQueryString"] :?> string
        let owinBodyStream = env.["owin.RequestBody"] :?> Stream

        let headers =
            owinHeaders :> seq<KeyValuePair<string, string[]>>
            |> Seq.map mapHeader

        let verb =
            match owinVerb.ToLowerInvariant() with
            | "get" -> Get
            | "post" -> Post
            | "put" -> Put
            | "delete" -> Delete
            | _ -> failwith "Unsupported request method."
    
        let url =
            { scheme = owinScheme
              host = owinHeaders.["Host"] |> Seq.head
              basePath = owinBasePath
              path = owinPath
              queryString = Map.empty } // TODO: Include querystring
    
        let! body = readBytesAsync owinBodyStream
        return HttpRequest.make verb url
        |> HttpRequest.withHeaders headers
        |> HttpRequest.withBodyOfBytes body
    }

let initializeResponse (env : IDictionary<string, obj>) = 
    let owinHeaders = env.["owin.ResponseHeaders"] :?> IDictionary<string, string[]>

    let headers =
        owinHeaders :> seq<KeyValuePair<string, string[]>>
        |> Seq.map mapHeader

    HttpResponse.make HttpStatus.ok200
    |> HttpResponse.withHeaders headers

let initializeContext env = 
    async {
        let! request = initializeRequest env
        let response = initializeResponse env
        return Context.make request response
    }

let writeContext (env : IDictionary<string, obj>) context =
    async {
        let response = Context.getResponse context

        match HttpResponse.getStatus response with
        | HttpStatus code ->
            env.["owin.ResponseStatusCode"] <- code :> obj
        | HttpStatusWithPhrase (code, phrase) ->
            env.["owin.ResponseStatusCode"] <- code :> obj
            env.["owin.ResponseReasonPhrase"] <- phrase :> obj
    
        let owinHeaders = env.["owin.ResponseHeaders"] :?> IDictionary<string, string[]>
        response
        |> HttpResponse.getHeaders
        |> Seq.iter (fun (key, value) -> owinHeaders.[key] <- [| value |])
        
        let owinBodyStream = env.["owin.ResponseBody"] :?> Stream
        let bytes = HttpResponse.getBodyAsBytes response
        do! writeBytesAsync bytes owinBodyStream
    }

let run application env = 
    initializeContext env
    |> Async.bind application
    |> Async.bind (writeContext env)
    |> Async.startAsPlainTask
    
let createAppFunc (application : SalmiakApplication<unit, 'T>) = AppFunc(run application)
let createMiddlewareFunc application = MiddlewareFunc(fun _ -> createAppFunc application)


