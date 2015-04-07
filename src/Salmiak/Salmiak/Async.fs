module internal Salmiak.Async
 
open System.Threading.Tasks
    
let singleton value = async { return value }

let map mapping asyncValue =
    async {
        let! value = asyncValue
        return mapping value
    }

let startAsPlainTask computation = Async.StartAsTask computation :> Task
let awaitPlainTask (task : Task) = Async.AwaitTask(task.ContinueWith(ignore))
