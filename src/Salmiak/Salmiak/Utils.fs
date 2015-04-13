namespace Salmiak.Utils

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Async =
    open System.Threading.Tasks
    
    let inline singleton value = async { return value }

    let inline map mapping asyncValue =
        async {
            let! value = asyncValue
            return mapping value
        }

    let inline bind mapping asyncValue =
        async {
            let! value = asyncValue
            return! mapping value
        }

    let startAsPlainTask computation = Async.StartAsTask computation :> Task
    let awaitPlainTask (task : Task) = Async.AwaitTask(task.ContinueWith(ignore))

[<AutoOpen>]
module AsyncOperators =
    let inline (>>!) f g x =
        async {
            let! y = f x
            return! g y
        }

    let inline (<<!) f g = g >>! f
    let inline (|>!) x f = Async.bind f x
    let inline (<|!) f x = Async.bind f x

