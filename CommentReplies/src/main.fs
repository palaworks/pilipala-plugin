namespace pilipala.plugin

open fsharper.typ
open fsharper.typ.Pipe
open pilipala.data.db
open pilipala.pipeline
open pilipala.pipeline.comment
open pilipala.container.comment

type CommentReplies
    (
        commentProvider: ICommentProvider,
        renderBuilder: ICommentRenderPipelineBuilder,
        db: IDbOperationBuilder
    ) =

    let getReplies comment_id =

        let sql =
            $"SELECT comment_id FROM {db.tables.comment} \
              WHERE comment_is_reply = true AND bind_id = <bind_id>"
            |> db.managed.normalizeSql

        fun (list: obj list) ->
            match list with
            | id :: ids -> Option.Some(commentProvider.fetch (downcast id), ids)
            | [] -> Option.None
        |> Seq.unfold
        <| db {
            getFstCol sql [ ("bind_id", comment_id) ]
            execute
        }

    do
        let data comment_id =
            Some(comment_id, getReplies comment_id :> obj)

        renderBuilder.["Replies"]
            .collection.Add (Replace(fun failPipe -> GenericCachePipe(data, failPipe.fill)))
