namespace Salmiak

type Application<'T, 'U> = HttpContext<'T> -> Async<HttpContext<'U>>
