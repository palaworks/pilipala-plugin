namespace pilipala.plugin

open pilipala
open pilipala.plugin
open pilipala.util.text
open Microsoft.Extensions.Logging

open plugin.cfg
open plugin.token
open plugin.hosting

[<HookOn(AppLifeCycle.AfterBuild)>]
type GrpcApi(cfg: IPluginCfgProvider, app: IApp, logger: ILogger<GrpcApi>) =
    do
        let cfg = { json = cfg.config }.deserializeTo<Cfg>().unwrap ()

        let token_handler = TokenHandler app

        runHost cfg token_handler logger
