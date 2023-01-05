module plugin.hosting

open System.Threading.Tasks
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open fsharper.typ
open WebSocketSharp.Server

type Worker(wsLocalServer: WebSocketServer, wsPublicServer: WebSocketServer) =
    inherit BackgroundService()

    override self.ExecuteAsync _ =
        fun _ ->
            wsLocalServer.Start()
            wsPublicServer.Start()
        |> Task.RunAsTask

let runHost (wsLocalServer, wsPublicServer) =
    Host
        .CreateDefaultBuilder()
        .ConfigureServices(fun ctx services ->
            fun _ -> new Worker(wsLocalServer, wsPublicServer)
            |> services.AddHostedService
            |> ignore)
        .Build()
        .Run()
