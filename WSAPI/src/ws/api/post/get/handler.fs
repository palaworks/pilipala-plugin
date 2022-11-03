namespace ws.api.post.get

open fsharper.op
open fsharper.typ
open pilipala.access.user
open pilipala.container.post
open pilipala.container.comment
open ws.helper

type Handler(pl_display_user: IUser) =
    interface IApiHandler<Handler, Req, Rsp> with

        override self.handle req =

            pl_display_user.GetPost(req.Id).fmap Rsp.fromPost
