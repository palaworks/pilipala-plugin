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

        //Display
        let pl_display_user =
            app.userLoginByName cfg.pl_comment_user cfg.pl_comment_pwd |> unwrap

        //Anonymous
        let pl_comment_user =
            app.userLoginByName cfg.pl_comment_user cfg.pl_comment_pwd |> unwrap

        let wsLocalServer =
            (wsLocalServer cfg.ws_local_port).configRouting
            <| (pl_display_user, pl_comment_user, cfg.enable_api_response_detail_logging, logger)

        let wsPublicServer =
            let cert_path =
                if cfg.ws_public_enable_ssl then
                    Some(cfg.ws_cert_pem_path, cfg.ws_cert_key_path)
                else
                    None

            (wsPublicServer cfg.ws_public_port cert_path).configRouting
            <| (pl_display_user, pl_comment_user, cfg.enable_api_response_detail_logging, logger)

        runHost (wsLocalServer, wsPublicServer)
