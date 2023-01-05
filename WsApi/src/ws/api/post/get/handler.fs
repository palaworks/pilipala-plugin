namespace ws.api.post.get

open System
open fsharper.op
open fsharper.typ
open pilipala.access.user
open pilipala.container.post
open pilipala.container.comment
open ws.helper
open helper

type Handler(pl_display_user: IUser) =
    interface IApiHandler<Handler, Req, Rsp> with

        override self.handle req =

            pl_display_user
                .GetPost(Int64.Parse req.Id)
                .fmap (fun post -> Rsp.fromPost (post, pl_display_user))
