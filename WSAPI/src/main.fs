namespace pilipala.plugin

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
type WSAPI(cfg: IPluginCfgProvider, app: IApp) =
    do
        let cfg =
            { json = cfg.config }
                .deserializeTo<Cfg>()
                .unwrap ()

        //Display
        let pl_display_user =
            app.userLoginByName cfg.pl_comment_user cfg.pl_comment_pwd
            |> unwrap

        //Anonymous
        let pl_comment_user =
            app.userLoginByName cfg.pl_comment_user cfg.pl_comment_pwd
            |> unwrap

        let wsLocalServer =
            (wsLocalServer cfg.ws_local_port)
                .configRouting (pl_display_user, pl_comment_user)

        let wsPublicServer =
            let cert_path =
                if cfg.ws_public_ssl_enable then
                    Some(cfg.ws_cert_key_path, cfg.ws_cert_pem_path)
                else
                    None

            (wsPublicServer cfg.ws_public_port cert_path)
                .configRouting (pl_display_user, pl_comment_user)

        runHost (wsLocalServer, wsPublicServer)
