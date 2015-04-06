module internal Salmiak.Async
 
open System.Threading.Tasks
    
let startAsPlainTask computation = Async.StartAsTask computation :> Task
let awaitPlainTask (task : Task) = Async.AwaitTask(task.ContinueWith(ignore))
