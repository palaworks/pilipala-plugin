module plugin.hosting

open System.Threading.Tasks
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open fsharper.typ
open WebSocketSharp.Server

type Worker(wsLocalServer: WebSocketServer, wsPublicServer: WebSocketServer, logger: ILogger) =
    inherit BackgroundService()

    override self.ExecuteAsync _ =
        fun _ ->
            wsLocalServer.Start()
            wsPublicServer.Start()

            logger.LogInformation $"Local WebSocket now listening on {wsLocalServer.Address}:{wsLocalServer.Port}"
            logger.LogInformation $"Public WebSocket now listening on {wsPublicServer.Address}:{wsPublicServer.Port}"
        |> Task.RunAsTask

let runHost wsLocalServer wsPublicServer (logger: ILogger) =
    Host
        .CreateDefaultBuilder()
        .ConfigureServices(fun ctx services ->
            fun _ -> new Worker(wsLocalServer, wsPublicServer, logger)
            |> services.AddHostedService
            |> ignore)
        .Build()
        .Run()
