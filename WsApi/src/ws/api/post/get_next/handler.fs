namespace ws.api.post.get_next

open System
open fsharper.op
open fsharper.typ
open fsharper.alias
open pilipala.access.user
open ws.api.post.get_one.helper
open ws.helper

type Handler(pl_display_user: IUser) =

    interface IApiHandler<Handler, Req, Rsp> with

        override self.handle req =
            let current_id = Int64.Parse req.CurrentId

            (get_prev_post pl_display_user current_id)
                .fmap (fun post -> ws.api.post.get_one.Rsp.fromPost (post, pl_display_user))
