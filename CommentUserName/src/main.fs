namespace pilipala.plugin

open fsharper.op
open fsharper.typ
open fsharper.op.Alias
open fsharper.typ.Pipe
open pilipala.data.db
open pilipala.pipeline
open pilipala.pipeline.comment

type CommentUserName(renderBuilder: ICommentRenderPipelineBuilder, db: IDbOperationBuilder) =

    //TODO 不知道联表查询会不会更高效（反正有缓存
    let getUserName comment_id =
        db {
            inComment
            getFstVal "user_id" "comment_id" comment_id
            execute
        }
        >>= fun user_id ->
                db {
                    inUser
                    getFstVal "user_name" "user_id" user_id
                    execute
                }

    do
        let data comment_id =
            getUserName comment_id
            >>= fun user_name -> Some(comment_id, user_name)

        renderBuilder.["UserName"]
            .collection.Add (Replace(fun failPipe -> GenericCachePipe(data, failPipe.fill)))
