namespace pilipala.plugin

open fsharper.typ
open fsharper.typ.Pipe
open pilipala.data.db
open pilipala.pipeline
open pilipala.pipeline.post
open pilipala.container.comment

type PostComments
    (
        mappedCommentProvider: IMappedCommentProvider,
        renderBuilder: IPostRenderPipelineBuilder,
        db: IDbOperationBuilder
    ) =

    let getComments post_id =

        let sql =
            $"SELECT comment_id FROM {db.tables.comment} \
              WHERE comment_is_reply = false AND bind_id = <bind_id>"
            |> db.managed.normalizeSql

        fun (list: obj list) ->
            match list with
            | id :: ids -> Option.Some(mappedCommentProvider.fetch (downcast id), ids)
            | [] -> Option.None
        |> Seq.unfold
        <| db {
            getFstCol sql [ ("bind_id", post_id) ]
            execute
        }

    do
        let data post_id =
            Some(post_id, getComments post_id :> obj)

        renderBuilder.["Comments"]
            .collection.Add (Replace(fun failPipe -> GenericCachePipe(data, failPipe.fill)))
