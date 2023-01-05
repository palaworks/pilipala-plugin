namespace ws.api.post.get_menu

open fsharper.op
open fsharper.typ
open pilipala.access.user
open ws.helper
open ws.api.post.get.helper

type Handler(pl_display_user: IUser) =
    interface IApiHandler<Handler, ws.api.post.get_batch.Req, Rsp> with

        override self.handle req =

            let list =
                let all = pl_display_user.GetReadablePost()

                all.foldr
                <| fun post acc ->
                    let mark =
                        post.["Mark"]
                            .unwrap() //Opt<obj>
                            .fmap(cast) //Opt<string>
                            .unwrapOr (fun _ -> "")

                    match mark with
                    | "menu"
                    | "about_me"
                    | "about_site" ->
                        ws.api.post.get.Rsp.fromPost (post, pl_display_user)
                        :: acc
                    | _ -> acc
                <| []

            { Collection = list.toArray () } |> Ok
