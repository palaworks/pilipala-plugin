namespace ws.api.post.get_all_id

open fsharper.op
open fsharper.typ
open pilipala.access.user
open ws.api.post.get
open ws.helper

type Handler(pl_display_user: IUser) =
    interface IApiHandler<Handler, Req, ws.api.post.get_all_id.Rsp> with

        override self.handle req =

            let arr =
                let all = pl_display_user.GetReadablePost()

                let pinned, other =
                    all.foldr
                    <| fun post (pinned, other) ->
                        let isPinned =
                            post.["IsPinned"]
                                .unwrap() //Opt<obj>
                                .fmap(cast) //Opt<bool>
                                .unwrapOr (fun _ -> false)

                        if isPinned then
                            post.Id :: pinned, other
                        else
                            pinned, post.Id :: other
                    <| ([], [])

                (pinned @ other).toArray ()

            { PostIds = arr } |> Ok
