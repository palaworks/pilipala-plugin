namespace pilipala.plugin

open System.Collections.Generic
open fsharper.op
open fsharper.typ
open fsharper.alias
open pilipala.plugin
open pilipala.pipeline
open pilipala.util.text
open pilipala.pipeline.post

type Summarizer
    (
        postRenderBuilder: IPostRenderPipelineBuilder,
        postRenderPipeline: IPostRenderPipeline,
        cfg: IPluginCfgProvider
    ) =

    let summaries =
        { json = cfg.config }
            .deserializeTo<Dictionary<u64, string>> ()

    let getBody id = postRenderPipeline.Body id |> snd

    do
        let f id : u64 * obj =
            match summaries.TryGetValue(id).intoOption' () with
            | None ->
                id,
                { html = getBody id }
                    .withoutTags()
                    .Substring(0, 80)
            | Some x -> (id, x)

        postRenderBuilder.["Summary"].collection.Add
        <| Replace(fun _ -> f)
