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
                        let mark =
                            post.["Mark"]
                                .unwrap() //Opt<obj>
                                .fmap(cast) //Opt<string>
                                .unwrapOr (fun _ -> "")

                        match mark with
                        | "pin" -> post.Id.ToString() :: pinned, other //指定文章，排名靠前
                        | "" -> pinned, post.Id.ToString() :: other //普通文章
                        | _ -> pinned, other //其他文章，不显示
                    <| ([], [])

                (pinned @ other).toArray ()

            { PostIds = arr } |> Ok
