namespace pilipala.plugin

open System
open System.Collections.Generic
open fsharper.op
open fsharper.typ
open fsharper.alias
open pilipala.plugin
open pilipala.pipeline
open pilipala.util.text
open pilipala.pipeline.post

[<HookOn(AppLifeCycle.BeforeBuild)>]
type Topics(postRenderBuilder: IPostRenderPipelineBuilder, cfg: IPluginCfgProvider) =

    let topics =
        { json = cfg.config }
            .deserializeTo<Dictionary<i64, string []>>()
            .unwrapOr (fun _ -> Dict<i64, string []>())

    do
        let f id : i64 * obj =
            id,
            topics
                .TryGetValue(id)
                .intoOption'()
                .unwrapOr(fun _ -> [||])
                .obj ()

        postRenderBuilder.["Topics"].collection.Add
        <| Replace(always f)
