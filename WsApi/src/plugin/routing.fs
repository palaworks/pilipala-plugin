module plugin.routing

open Microsoft.Extensions.Logging
open Suave
open Suave.Sockets
open Suave.Sockets.Control
open Suave.WebSocket
open Suave.Filters
open Suave.Operators
open Suave.Successful
open pilipala.access.user
open ws.api
open ws.helper

let configRouting
    (
        pl_display_user: IUser,
        pl_comment_user: IUser,
        enable_api_response_detail_logging: bool,
        logger: ILogger
    ) =
    choose
        [ path "/post/get_one"
          >=> handShake (
              (pl_display_user |> post.get_one.Handler).toWsBehavior
              <| enable_api_response_detail_logging
              <| logger
          )
          path "/post/get_prev"
          >=> handShake (
              (pl_display_user |> post.get_prev.Handler).toWsBehavior
              <| enable_api_response_detail_logging
              <| logger
          )

          path "/post/get_next"
          >=> handShake (
              (pl_display_user |> post.get_next.Handler).toWsBehavior
              <| enable_api_response_detail_logging
              <| logger
          )


          path "/post/get_some"
          >=> handShake (
              (pl_display_user |> post.get_some.Handler).toWsBehavior
              <| enable_api_response_detail_logging
              <| logger
          )

          path "/post/get_all_id"
          >=> handShake (
              (pl_display_user |> post.get_all_id.Handler).toWsBehavior
              <| enable_api_response_detail_logging
              <| logger
          )

          path "/post/get_menu"
          >=> handShake (
              (pl_display_user |> post.get_menu.Handler).toWsBehavior
              <| enable_api_response_detail_logging
              <| logger
          )

          path "/comment/create"
          >=> handShake (
              (pl_comment_user |> comment.create.Handler).toWsBehavior
              <| enable_api_response_detail_logging
              <| logger
          ) ]
