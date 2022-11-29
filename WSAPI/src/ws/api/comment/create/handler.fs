namespace ws.api.comment.create

open System
open pilipala.access.user
open ws.helper

type Handler(pl_comment_user: IUser) =
    interface IApiHandler<Handler, Req, Rsp> with
        override self.handle req =

            if req.IsReply then
                pl_comment_user
                    .GetComment(
                        Int64.Parse req.Binding
                    )
                    .bind
                <| fun comment ->
                    comment.NewComment(req.Body).fmap
                    <| fun comment -> Rsp.fromComment (comment, Int64.Parse req.Binding, true)
            else
                pl_comment_user
                    .GetPost(
                        Int64.Parse req.Binding
                    )
                    .bind
                <| fun post ->
                    post.NewComment(req.Body).fmap
                    <| fun comment -> Rsp.fromComment (comment, Int64.Parse req.Binding, false)
