module plugin.hosting

open Grpc.Core
open fsharper.typ
open fsharper.alias
open System.Threading.Tasks
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection

open plugin.ext
open plugin.cfg
open plugin.token
open plugin.credential

type Worker(cfg: Cfg, token_handler: TokenHandler, logger: ILogger) =
    inherit BackgroundService()

    override self.ExecuteAsync _ =
        fun _ ->
            Server()
                .addPort(ServerPort(cfg.host, i32 cfg.port, get_credentials cfg))
                .addService(grpc.api.comment.service.make token_handler logger)
                .addService(grpc.api.post.service.make token_handler logger)
                .addService(grpc.api.token.service.make token_handler logger)
                .Start()

            logger.LogInformation $"gRPC now listening on {cfg.host}:{cfg.port}"
        |> Task.RunAsTask

let runHost cfg token_handler (logger: ILogger) =
    Host
        .CreateDefaultBuilder()
        .ConfigureLogging(fun builder -> builder.ClearProviders() |> ignore)
        .ConfigureServices(fun ctx services ->
            fun _ -> new Worker(cfg, token_handler, logger)
            |> services.AddHostedService
            |> ignore)
        .Build()
        .Run()
