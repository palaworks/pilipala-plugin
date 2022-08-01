namespace pilipala.plugin

open fsharper.op
open fsharper.typ
open pilipala.data.db
open pilipala.pipeline
open pilipala.pipeline.comment

type UserName
    (
        postRenderBuilder: ICommentRenderPipelineBuilder,
        commentRenderBuilder: ICommentRenderPipelineBuilder,
        db: IDbOperationBuilder
    ) =

    //TODO 不知道联表查询会不会更高效（反正有缓存

    do
        //post
        let getUserName post_id =
            db {
                inPost
                getFstVal "user_id" "post_id" post_id
                execute
            }
            >>= fun user_id ->
                    db {
                        inUser
                        getFstVal "user_name" "user_id" user_id
                        execute
                    }

        let data post_id =
            getUserName post_id
            >>= fun user_name -> Some(post_id, user_name)

        postRenderBuilder.["UserName"].collection.Add
        <| Replace(fun fail id -> unwrapOr (data id) (fun _ -> fail id))

    do
        //comment
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

        let data comment_id =
            getUserName comment_id
            >>= fun user_name -> Some(comment_id, user_name)

        commentRenderBuilder.["UserName"].collection.Add
        <| Replace(fun fail id -> unwrapOr (data id) (fun _ -> fail id))
