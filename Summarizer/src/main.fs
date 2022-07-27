namespace pilipala.plugin

open System.Collections.Generic
open fsharper.op
open fsharper.typ
open fsharper.op.Alias
open fsharper.typ.Pipe
open DbManaged.PgSql
open pilipala.plugin
open pilipala.data.db
open pilipala.util.io
open pilipala.pipeline
open pilipala.util.text
open pilipala.pipeline.post

type Summarizer(renderBuilder: IPostRenderPipelineBuilder, db: IDbOperationBuilder,cfg:IPluginCfgProvider) =

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

        renderBuilder.["Summary"]
            .collection.Add (Replace(fun _ -> GenericPipe f))

//TODO 应该有更好的管道间通信方式，而不是手动构建目标管道
//下为手动管道构建模式
(*
    let getBody =
        let beforeFail =
            renderBuilder.Body.beforeFail.foldr
                (fun p (acc: IGenericPipe<_, _>) -> acc.export p)
                (GenericPipe<_, _>(id))

        let fail: u64 -> _ =
            beforeFail.fill .> panicwith

        let data id =
            db
                .makeCmd()
                .getFstVal (db.tables.post, "post_body", "post_id", id)
            |> db.managed.executeQuery
            >>= coerce

        let bodyRenderPipeline =
            renderBuilder.Body.collection.foldl
            <| fun acc x ->
                match x with
                | Before before -> before.export acc
                | Replace f -> f acc
                | After after -> acc.export after
            <| GenericCachePipe<_, _>(data, fail)

        bodyRenderPipeline.fill
*)
