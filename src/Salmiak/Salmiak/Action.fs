namespace Salmiak

type HttpData<'T> = HttpData of HttpRequest * HttpResponse * 'T
type HttpAction<'T> = HttpAction of Async<'T>