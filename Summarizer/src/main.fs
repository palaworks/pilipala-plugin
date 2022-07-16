namespace pilipala.plugin

open fsharper.op
open fsharper.typ
open fsharper.op.Alias
open fsharper.typ
open fsharper.typ.Pipe
open fsharper.op.Error
open fsharper.op.Foldable
open DbManaged.PgSql
open pilipala.db
open pilipala.pipeline
open pilipala.util.html
open pilipala.pipeline.post

type Summarizer(render: IPostRenderPipelineBuilder, dp: IDbProvider) =

    //TODO 应该有更好的管道间通信方式，而不是手动构建目标管道

    let getBody =
        let beforeFail =
            render.body.beforeFail.foldr (fun p (acc: IGenericPipe<_, _>) -> acc.export p) (GenericPipe<_, _>(id))

        let fail: u64 -> _ =
            beforeFail.fill .> panicwith

        let data id =
            dp
                .mkCmd()
                .getFstVal (dp.tables.record, "body", "recordId", id)
            |> dp.managed.executeQuery
            >>= coerce

        let bodyRenderPipeline =
            render.body.collection.foldl
            <| fun acc x ->
                match x with
                | Before before -> before.export acc
                | Replace f -> f acc
                | After after -> acc.export after
            <| GenericCachePipe<_, _>(data, fail)

        bodyRenderPipeline.fill

    do
        let after (id: u64, v: string) =
            match v with
            | ""
            | null ->
                id,
                (getBody id)
                    .snd()
                    .removeHtmlTags()
                    .Substring(0, 80)
            | _ -> (id, v)

        render.body.collection.Add(After(GenericPipe after))
