module plugin.routing

open Microsoft.Extensions.Logging
open WebSocketSharp.Server
open pilipala.access.user
open ws.api
open ws.helper

type WebSocketServer with

    member self.configRouting
        (
            pl_display_user: IUser,
            pl_comment_user: IUser,
            enable_api_response_detail_logging: bool,
            logger: ILogger
        ) =
        self
            .addService(
                "/post/get",
                fun _ ->
                    (pl_display_user |> post.get.Handler).toWsBehavior
                    <| enable_api_response_detail_logging
                    <| logger
            )
            .addService(
                "/post/get_prev",
                fun _ ->
                    (pl_display_user |> post.get_prev.Handler).toWsBehavior
                    <| enable_api_response_detail_logging
                    <| logger
            )
            .addService(
                "/post/get_next",
                fun _ ->
                    (pl_display_user |> post.get_next.Handler).toWsBehavior
                    <| enable_api_response_detail_logging
                    <| logger
            )
            .addService(
                "/post/get_batch",
                fun _ ->
                    (pl_display_user |> post.get_batch.Handler).toWsBehavior
                    <| enable_api_response_detail_logging
                    <| logger
            )
            .addService(
                "/post/get_all_id",
                fun _ ->
                    (pl_display_user |> post.get_all_id.Handler).toWsBehavior
                    <| enable_api_response_detail_logging
                    <| logger
            )
            .addService(
                "/post/get_menu",
                fun _ ->
                    (pl_display_user |> post.get_menu.Handler).toWsBehavior
                    <| enable_api_response_detail_logging
                    <| logger
            )
            .addService (
                "/comment/create",
                fun _ ->
                    (pl_comment_user |> comment.create.Handler).toWsBehavior
                    <| enable_api_response_detail_logging
                    <| logger
            )
