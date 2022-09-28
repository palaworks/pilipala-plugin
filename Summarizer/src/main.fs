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

type Summarizer
    (
        postRenderBuilder: IPostRenderPipelineBuilder,
        postRenderPipeline: IPostRenderPipeline,
        cfg: IPluginCfgProvider
    ) =

    let summaries =
        { json = cfg.config }
            .deserializeTo<Dictionary<i64, string>>()
            .unwrapOr (fun _ -> Dict<i64, string>())

    let getBody id = postRenderPipeline.Body id |> snd

    do
        let f id : i64 * obj =
            match summaries.TryGetValue(id).intoOption' () with
            | None ->
                id,
                { html = getBody id }
                    .removeTags()
                    .mergeBlanks()
                    .prefix 120
            | Some x -> id, x

        postRenderBuilder.["Summary"].collection.Add
        <| Replace(always f)

    do
        //用于提供判断概要是否为生成的的依据
        let f id : i64 * obj =
            match summaries.TryGetValue(id).intoOption' () with
            | None -> id, true
            | Some _ -> id, false

        postRenderBuilder.["IsGeneratedSummary"]
            .collection
            .Add
        <| Replace(always f)
