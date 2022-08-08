namespace pilipala.plugin

open System.Collections.Generic
open fsharper.op
open fsharper.typ
open fsharper.op.Alias
open DbManaged.PgSql
open pilipala.container.post
open pilipala.plugin
open pilipala.data.db
open pilipala.pipeline
open pilipala.util.text
open pilipala.pipeline.post

type Summarizer
    (
        postRenderBuilder: IPostRenderPipelineBuilder,
        mappedPostProvider: IMappedPostProvider,
        cfg: IPluginCfgProvider
    ) =

    let summaries =
        { json = cfg.config }
            .deserializeTo<Dictionary<u64, string>> ()

    let getBody id =
        mappedPostProvider.fetch

        do
            let f id : u64 * obj =
                match summaries.TryGetValue(id).intoOption' () with
                | None ->
                    id,
                    { html = getBody id }
                        .withoutTags()
                        .Substring(0, 80)
                | Some x -> (id, x)

            renderBuilder.["Summary"].collection.Add
            <| Replace(fun _ -> f)
