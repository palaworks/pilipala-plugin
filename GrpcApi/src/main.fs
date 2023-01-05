namespace pilipala.plugin

open pilipala
open fsharper.op
open fsharper.typ
open pilipala.plugin
open pilipala.util.text
open plugin.cfg

[<HookOn(AppLifeCycle.AfterBuild)>]
type GrpcApi(cfg: IPluginCfgProvider, app: IApp) =
    do
        let cfg =
            { json = cfg.config }
                .deserializeTo<Cfg>()
                .unwrap ()

        ()
