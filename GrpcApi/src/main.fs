namespace pilipala.plugin

open System
open System.Collections.Generic
open System.IO
open Grpc.Core
open Microsoft.Extensions.Logging
open pilipala
open fsharper.op
open fsharper.typ
open fsharper.alias
open pilipala.plugin
open pilipala.util.text
open plugin.cfg
open plugin.credential
open ext

[<HookOn(AppLifeCycle.AfterBuild)>]
type GrpcApi(cfg: IPluginCfgProvider, app: IApp, logger: ILogger<GrpcApi>) =
    do
        let cfg = { json = cfg.config }.deserializeTo<Cfg>().unwrap ()

        let token_handler = token.TokenHandler app

        logger.LogInformation $"gRPC now listen on {cfg.host}:{cfg.port}"

        Server()
            .addPort(ServerPort(cfg.host, i32 cfg.port, get_credentials cfg))
            .addService(grpc.api.comment.service.make token_handler logger)
            .addService(grpc.api.post.service.make token_handler logger)
            .addService(grpc.api.token.service.make token_handler logger)
            .Start()
