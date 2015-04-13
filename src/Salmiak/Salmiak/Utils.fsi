namespace Salmiak.Utils

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Async =
    open System.Threading.Tasks
    
    val inline singleton : value:'T -> Async<'T>
    val inline map : mapping:('T -> 'U) -> asyncValue:Async<'T> -> Async<'U>
    val inline bind : mapping:('T -> Async<'U>) -> asyncValue:Async<'T> -> Async<'U>
    
    val internal startAsPlainTask : computation:Async<'T> -> Task
    val internal awaitPlainTask : task:Task -> Async<unit>

[<AutoOpen>]
module AsyncOperators =
    val inline (>>!) : f:('S -> Async<'T>) -> g:('T -> Async<'U>) -> ('S -> Async<'U>)
    val inline (<<!) : g:('T -> Async<'U>) -> f:('S -> Async<'T>) -> ('S -> Async<'U>)
    val inline (|>!) : x:Async<'T> -> f:('T -> Async<'U>) -> Async<'U>
    val inline (<|!) : f:('T -> Async<'U>) -> x:Async<'T> -> Async<'U>

