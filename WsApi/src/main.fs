namespace pilipala.plugin

open pilipala
open fsharper.op
open fsharper.typ
open pilipala.plugin
open pilipala.util.text
open System.Threading.Tasks
open Microsoft.Extensions.Logging

open ws.server
open plugin.cfg
open plugin.routing
open plugin.hosting

[<HookOn(AppLifeCycle.AfterBuild)>]
type WsApi(cfg: IPluginCfgProvider, app: IApp, logger: ILogger<WsApi>) =
    do
        let cfg = { json = cfg.config }.deserializeTo<Cfg>().unwrap ()

        //Display
        let pl_display_user =
            app.userLoginByName cfg.pl_comment_user cfg.pl_comment_pwd |> unwrap

        //Anonymous
        let pl_comment_user =
            app.userLoginByName cfg.pl_comment_user cfg.pl_comment_pwd |> unwrap

        let startWsLocalServer () =
            let routing =
                configRouting (pl_display_user, pl_comment_user, cfg.enable_api_response_detail_logging, logger)

            wsLocalServer cfg.ws_local_port routing

        let startWsPublicServer () =
            let routing =
                configRouting (pl_display_user, pl_comment_user, cfg.enable_api_response_detail_logging, logger)

            if cfg.ws_public_enable_ssl then
                wsPublicServer cfg.ws_public_port cfg.ws_cert_pem_path cfg.ws_cert_key_path routing
            else
                wsLocalServer cfg.ws_public_port routing


        fun _ -> runHost startWsLocalServer startWsPublicServer logger
        |> Task.RunIgnore
