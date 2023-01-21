module grpc.api.post.create

open System
open Grpc.Core
open fsharper.typ
open pilipala.access.user
open pilipala.util.text.time
open grpc_code_gen.post.create
open Microsoft.Extensions.Logging

type Ctx = ServerCallContext

let handler (user: IUser) (req: Req) (ctx: Ctx) (logger: ILogger) =
    match user.NewPost req.Title req.Body with
    | Ok post ->
        let data =
            grpc_code_gen.post.get_one.T(
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
    | Err msg -> Err msg
