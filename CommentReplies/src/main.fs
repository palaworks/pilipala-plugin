namespace pilipala.plugin

open fsharper.op
open fsharper.typ
open fsharper.alias
open pilipala.data.db
open pilipala.pipeline
open pilipala.pipeline.comment
open pilipala.container.comment

type CommentReplies
    (
        mappedCommentProvider: IMappedCommentProvider,
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
            | id :: ids -> Option.Some(mappedCommentProvider.fetch (downcast id), ids)
            | [] -> Option.None
        |> Seq.unfold
        <| db {
            getFstCol sql [ ("bind_id", comment_id) ]
            execute
        }

    do
        let data (comment_id: u64) =
            Some(comment_id, getReplies comment_id :> obj)

        renderBuilder.["Replies"].collection.Add
        <| Replace(fun fail id -> unwrapOr (data id) (fun _ -> fail id))
