module plugin.routing

open WebSocketSharp.Server
open pilipala.access.user
open ws.api
open ws.helper

type WebSocketServer with

    member self.configRouting(pl_display_user: IUser, pl_comment_user: IUser) =
        self
            .addService("/post/get", (pl_display_user |> post.get.Handler).toWsBehavior)
            .addService(
                "/post/get_all",
                (pl_display_user |> post.get_all.Handler)
                    .toWsBehavior
            )
            .addService(
                "/post/get_prev",
                (pl_display_user |> post.get_prev.Handler)
                    .toWsBehavior
            )
            .addService(
                "/post/get_next",
                (pl_display_user |> post.get_next.Handler)
                    .toWsBehavior
            )
            .addService(
                "/post/get_batch",
                (pl_display_user |> post.get_batch.Handler)
                    .toWsBehavior
            )
            .addService(
                "/post/get_all_id",
                (pl_display_user |> post.get_all_id.Handler)
                    .toWsBehavior
            )
            .addService (
                "/comment/create",
                (pl_comment_user |> comment.create.Handler)
                    .toWsBehavior
            )
