namespace ws.api.comment.create

open pilipala.access.user
open ws.helper

type Handler(pl_comment_user: IUser) =
    interface IApiHandler<Handler, Req, Rsp> with
        override self.handle req =

            if req.IsReply then
                pl_comment_user.GetComment(req.Binding).bind
                <| fun comment ->
                    comment.NewComment(req.Body).fmap
                    <| fun comment -> Rsp.fromComment (comment, req.Binding, true)
            else
                pl_comment_user.GetPost(req.Binding).bind
                <| fun post ->
                    post.NewComment(req.Body).fmap
                    <| fun comment -> Rsp.fromComment (comment, req.Binding, false)
