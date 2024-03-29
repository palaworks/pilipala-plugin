namespace ws.api.post.get_all

open fsharper.op
open fsharper.op.Foldable
open fsharper.typ
open pilipala.access.user
open ws.api.post.get
open ws.helper

type Handler(pl_display_user: IUser) =
    interface IApiHandler<Handler, Req, ws.api.post.get_all.Rsp> with

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
                            Rsp.fromPost post :: pinned, other
                        else
                            pinned, Rsp.fromPost post :: other
                    <| ([], [])

                (pinned @ other).toArray ()

            { Collection = arr } |> Ok
