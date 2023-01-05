[<AutoOpen>]
module ws.helper.ext_WebSocketServer

open WebSocketSharp.Server

type WebSocketServer with
    member self.addService(path, f: unit -> WebSocketBehavior) =
        self.AddWebSocketService(path, f)
        self
