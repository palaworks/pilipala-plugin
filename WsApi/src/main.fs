namespace pilipala.plugin

open Microsoft.Extensions.Logging
open pilipala
open fsharper.op
open fsharper.typ
open pilipala.plugin
open pilipala.util.text
open ws.server
open plugin.cfg
open plugin.routing
open plugin.hosting

[<HookOn(AppLifeCycle.AfterBuild)>]
type WsApi(cfg: IPluginCfgProvider, app: IApp, logger: ILogger<WsApi>) =
    do
        let cfg = { json = cfg.config }.deserializeTo<Cfg>().unwrap ()


        runHost wsLocalServer wsPublicServer logger
