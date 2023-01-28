namespace pilipala.plugin

open pilipala
open plugin.cfg
open fsharper.typ
open plugin.token
open plugin.hosting
open pilipala.plugin
open pilipala.util.text
open System.Threading.Tasks
open Microsoft.Extensions.Logging

[<HookOn(AppLifeCycle.AfterBuild)>]
type GrpcApi(cfg: IPluginCfgProvider, app: IApp, logger: ILogger<GrpcApi>) =
    do
        let cfg = { json = cfg.config }.deserializeTo<Cfg>().unwrap ()

        let token_handler = TokenHandler app

        fun _ -> runHost cfg token_handler logger
        |> Task.RunIgnore
