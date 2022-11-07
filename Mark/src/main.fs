namespace pilipala.plugin

open fsharper.typ
open fsharper.alias
open pilipala.plugin
open pilipala.pipeline
open pilipala.util.text
open pilipala.pipeline.post

[<HookOn(AppLifeCycle.BeforeBuild)>]
type Mark(postRenderBuilder: IPostRenderPipelineBuilder, cfg: IPluginCfgProvider) =

    let map =
        { json = cfg.config }.deserializeTo<Dict<i64, string>>()
            .unwrapOr
        <| fun _ -> Dict<_, _>()

    do
        let f id : i64 * obj =
            if map.ContainsKey id then
                id, map.[id]
            else
                id, ""

        postRenderBuilder.["Mark"].collection.Add
        <| Replace(always f)
