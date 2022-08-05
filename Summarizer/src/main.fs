namespace pilipala.plugin

open System.Collections.Generic
open fsharper.op
open fsharper.typ
open fsharper.op.Alias
open DbManaged.PgSql
open pilipala.plugin
open pilipala.data.db
open pilipala.pipeline
open pilipala.util.text
open pilipala.pipeline.post

type Summarizer(renderBuilder: IPostRenderPipelineBuilder, db: IDbOperationBuilder, cfg: IPluginCfgProvider) =

    let summaries =
        { json = cfg.config }
            .deserializeTo<Dictionary<u64, string>> ()

    let getBody id : string =
        db
            .makeCmd()
            .getFstVal (db.tables.post, "post_body", "post_id", id)
        |> db.managed.executeQuery
        |> fmap coerce
        |> unwrap

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
