module Salmiak.Owin

open System
open System.Collections.Generic
open System.IO
open System.Text
open System.Threading.Tasks

type SalmiakApplication<'T, 'U> = HttpAction<'T> -> HttpAction<'U> 

type AppFunc = Func<IDictionary<string, obj>, Task>
type MiddlewareFunc = Func<AppFunc, AppFunc>

let initializeRequest (env : IDictionary<string, obj>) = 
    let owinHeaders = env.["owin.RequestHeaders"] :?> IDictionary<string, string[]>
    let owinMethod = env.["owin.RequestMethod"] :?> string
    let owinBasePath = env.["owin.RequestPathBase"] :?> string
    let owinPath = env.["owin.RequestPath"] :?> string
    let owinProtocol = env.["owin.RequestProtocol"] :?> string
    let owinScheme = env.["owin.RequestScheme"] :?> string
    let owinQueryString = env.["owin.RequestQueryString"] :?> string
    let owinBodyStream = env.["owin.RequestBody"] :?> Stream

    let headers =
        owinHeaders :> seq<KeyValuePair<string, string[]>>
        |> Seq.map (fun (KeyValue(key, values)) -> (key, String.concat ", " values))
        |> Map.ofSeq

    let requestMethod =
        match owinMethod.ToLowerInvariant() with
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
    
    // TODO: Make other kinds of bodies possible
    // TODO: Make body async
    let bodyReader = new StreamReader(owinBodyStream, Encoding.UTF8)
    let body = bodyReader.ReadToEnd()

    HttpRequest (url, requestMethod, headers, body)

let initializeResponse (env : IDictionary<string, obj>) = 
    let owinHeaders = env.["owin.ResponseHeaders"] :?> IDictionary<string, string[]>

    let headers =
        owinHeaders :> seq<KeyValuePair<string, string[]>>
        |> Seq.map (fun (KeyValue(key, values)) -> (key, String.concat ", " values))
        |> Map.ofSeq

    // TODO: Include status code smarter
    // TODO: Make body more generic and async
    HttpResponse (HttpStatusCode 200, headers, String.Empty)

let owinToAction env = 
    let request = initializeRequest env
    let response = initializeResponse env
    HttpAction (request, response, ())

let actionToOwin (env : IDictionary<string, obj>) (HttpAction(_, HttpResponse(HttpStatusCode code, headers, body), _)) =
    env.["owin.ResponseStatusCode"] <- code :> obj
    
    let owinHeaders = env.["owin.ResponseHeaders"] :?> IDictionary<string, string[]>
    headers |> Map.iter (fun key value -> owinHeaders.[key] <- [| value |])
    
    // TODO: Fix body in same way as for request described above
    let owinBodyStream = env.["owin.ResponseBody"] :?> Stream
    let responseWriter = new StreamWriter(owinBodyStream, Encoding.UTF8)
    responseWriter.Write(body)
    responseWriter.Flush()

    ()

let run application env = 
    let initialAction = owinToAction env
    let result = application initialAction
    actionToOwin env result
    Task.FromResult () :> Task
    
let createAppFunc (application : SalmiakApplication<unit, 'T>) = AppFunc(run application)
let createMiddlewareFunc application = MiddlewareFunc(fun _ -> createAppFunc application)


