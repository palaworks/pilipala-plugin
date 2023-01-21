module grpc.api.comment.get_one

open System
open Microsoft.Extensions.Logging
open grpc_code_gen.comment.get_one
open Grpc.Core
open fsharper.typ
open pilipala.access.user
open pilipala.util.text.time

type Ctx = ServerCallContext

let handler (user: IUser) (req: Req) (ctx: Ctx) (logger: ILogger) =
    match user.GetComment(req.Id) with
    | Ok comment ->
        if comment.CanRead then
            let data =
                T(
                    Id = comment.Id,
                    Body =
                        comment.Body.unwrapOrEval (fun _ ->
                            $"Unknown error: can not read {nameof comment.Body}(comment id:{comment.Id})"
                            |> effect logger.LogError),
                    CreateTime =
                        comment
                            .CreateTime
                            .unwrapOrEval(fun _ ->
                                DateTime.UnixEpoch.effect
                                <| fun _ ->
                                    logger.LogError
                                        $"Unknown error: can not read {nameof comment.CreateTime}(comment id:{comment.Id})")
                            .ToIso8601()
                )

            Rsp(Ok = true, Msg = "", Data = data) |> Ok
        else
            $"Operation failed: Permission denied(comment id:{comment.Id})"
            |> effect logger.LogError
            |> Err
    | Err msg -> Err msg
