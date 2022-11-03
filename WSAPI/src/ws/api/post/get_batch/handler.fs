namespace ws.api.post.get_batch

open fsharper.typ
open fsharper.op.Foldable
open pilipala.access.user
open ws.helper

type Handler(pl_display_user: IUser) =
    interface IApiHandler<Handler, Req, Rsp> with

        override self.handle req =

            let arr =
                req.Ids.foldr
                <| fun id acc ->
                    match pl_display_user.GetPost(id) with
                    | Ok post -> ws.api.post.get.Rsp.fromPost post :: acc
                    | _ -> acc
                <| []
                |> List.toArray

            { Collection = arr } |> Ok
