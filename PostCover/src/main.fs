namespace pilipala.plugin

open fsharper.op
open fsharper.typ
open fsharper.alias
open pilipala.pipeline
open pilipala.util.text
open pilipala.pipeline.post

[<HookOn(AppLifeCycle.BeforeBuild)>]
type PostCover
    (
        renderBuilder: IPostRenderPipelineBuilder,
        modifyBuilder: IPostModifyPipelineBuilder,
        cfg: IPluginCfgProvider
    ) =

    let map =
        { json = cfg.config }
            .deserializeTo<Dict<i64, string>>()
            .unwrapOrEval (fun _ -> Dict<i64, string>())

    do
        let f id =
            id, map.TryGetValue(id).intoOption'().obj ()

        renderBuilder.["CoverUrl"].collection.Add
        <| Replace(fun _ -> f)

    do
        let f (id, v: obj) =
            if map.ContainsKey id then
                map.[id] <- v.ToString()
            else
                map.Add(id, v.ToString())

            cfg.config <- map.serializeToJson().json
            id, v

        modifyBuilder.["CoverUrl"].collection.Add
        <| Replace(fun _ -> f)
