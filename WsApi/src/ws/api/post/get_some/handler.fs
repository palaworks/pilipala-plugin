namespace ws.api.post.get_some

open System
open fsharper.typ
open fsharper.op.Foldable
open pilipala.access.user
open ws.api.post.get_one.helper
open ws.helper

type Handler(pl_display_user: IUser) =
    interface IApiHandler<Handler, Req, Rsp> with

        override self.handle req =

            let arr =
                req.Ids.foldr
                <| fun id acc ->
                    let id = Int64.Parse id

                    match pl_display_user.GetPost(id) with
                    | Ok post ->
                        ws.api.post.get_one.Rsp.fromPost (post, pl_display_user)
                        :: acc
                    | _ -> acc
                <| []
                |> List.toArray

            { Collection = arr } |> Ok
