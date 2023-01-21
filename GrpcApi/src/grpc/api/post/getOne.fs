module grpc.api.post.get_one

open System
open Microsoft.Extensions.Logging
open grpc_code_gen.post.get_one
open Grpc.Core
open fsharper.typ
open pilipala.access.user
open pilipala.util.text.time

type Ctx = ServerCallContext

let handler (user: IUser) (req: Req) (ctx: Ctx) (logger: ILogger) =
    match user.GetPost(req.Id) with
    | Ok post ->
        if post.CanRead then
            let data =
                T(
                    Id = post.Id,
                    Title =
                        post.Title.unwrapOrEval (fun _ ->
                            $"Unknown error: can not read {nameof post.Title}(post id:{post.Id})"
                            |> effect logger.LogError),
                    Body =
                        post.Body.unwrapOrEval (fun _ ->
                            $"Unknown error: can not read {nameof post.Body}(post id:{post.Id})"
                            |> effect logger.LogError),
                    CreateTime =
                        post
                            .CreateTime
                            .unwrapOrEval(fun _ ->
                                DateTime.UnixEpoch.effect
                                <| fun _ ->
                                    logger.LogError
                                        $"Unknown error: can not read {nameof post.CreateTime}(post id:{post.Id})")
                            .ToIso8601(),
                    ModifyTime =
                        post
                            .ModifyTime
                            .unwrapOrEval(fun _ ->
                                DateTime.UnixEpoch.effect
                                <| fun _ ->
                                    logger.LogError
                                        $"Unknown error: can not read {nameof post.ModifyTime}(post id:{post.Id})")
                            .ToIso8601()
                )

            Rsp(Ok = true, Msg = "", Data = data) |> Ok
        else
            $"Operation failed: Permission denied(post id:{post.Id})"
            |> effect logger.LogError
            |> Err
    | Err msg -> Err msg
