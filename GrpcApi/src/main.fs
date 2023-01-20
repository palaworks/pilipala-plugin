namespace pilipala.plugin

open System
open System.Collections.Generic
open System.IO
open Grpc.Core
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
type GrpcApi(cfg: IPluginCfgProvider, app: IApp) =
    do
        let cfg =
            { json = cfg.config }
                .deserializeTo<Cfg>()
                .unwrap ()

        let token_handler = token.TokenHandler app
        
        Console.WriteLine cfg.host
        Console.WriteLine cfg.port

        Server()
            .addPort(ServerPort(cfg.host, i32 cfg.port, get_credentials cfg))
            .addService(grpc.api.token.service.make token_handler)
            .addService(grpc.api.post.service.make token_handler)
            .addService(grpc.api.comment.service.make token_handler)
            .Start()
