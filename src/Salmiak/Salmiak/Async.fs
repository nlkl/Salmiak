namespace Salmiak

module internal Async = 
    open System.Threading.Tasks
    
    let startAsPlainTask computation = Async.StartAsTask computation :> Task
    let awaitPlainTask (task : Task) = Async.AwaitTask(task.ContinueWith(ignore))
