namespace pilipala.plugin

open Grpc.Core
open pilipala
open fsharper.op
open fsharper.typ
open pilipala.plugin
open pilipala.util.text
open plugin.cfg
open ext

[<HookOn(AppLifeCycle.AfterBuild)>]
type GrpcApi(cfg: IPluginCfgProvider, app: IApp) =
    do
        (*
        let cfg =
            { json = cfg.config }
                .deserializeTo<Cfg>()
                .unwrap ()
                *)
        
        let token_handler = token.TokenHandler(app)

        Server()
            .addPort(ServerPort("localhost", 40040, ServerCredentials.Insecure))
            .addService(grpc.api.token.service.make token_handler)
            .addService(grpc.api.post.service.make token_handler)
            .addService(grpc.api.comment.service.make token_handler)
            .Start()
