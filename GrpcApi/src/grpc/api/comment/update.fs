module grpc.api.comment.update

open System
open fsharper.typ
open plugin.grpc.alias
open pilipala.access.user
open pilipala.util.text.time
open pilipala.util.hash.sha256
open Microsoft.Extensions.Logging
open grpc_code_gen.comment.update

let handler (user: IUser) (req: Req) (ctx: Ctx) (logger: ILogger) =
    match user.GetComment(req.Id) with
    | Ok comment ->
        if comment.CanRead && comment.CanWrite then
            comment.Body.fmap
            <| fun old ->
                if old.sha256.sha256 <> req.Body.sha256.sha256 then
                    comment.UpdateBody req.Body |> ignore //TODO handle result
            |> ignore

            let data =
                grpc_code_gen.comment.get_one.T(
                    Id = comment.Id,
                    Body =
                        comment.Body.unwrapOrEval (fun _ ->
                            $"Unknown error: can not read {nameof comment.Body} (comment id:{comment.Id})"
                            |> effect logger.LogError),
                    CreateTime =
                        comment
                            .CreateTime
                            .unwrapOrEval(fun _ ->
                                DateTime.UnixEpoch.effect
                                <| fun _ ->
                                    logger.LogError
                                        $"Unknown error: can not read {nameof comment.CreateTime} (comment id:{comment.Id})")
                            .ToIso8601(),
                    ModifyTime =
                        comment
                            .ModifyTime
                            .unwrapOrEval(fun _ ->
                                DateTime.UnixEpoch.effect
                                <| fun _ ->
                                    logger.LogError
                                        $"Unknown error: can not read {nameof comment.ModifyTime} (comment id:{comment.Id})")
                            .ToIso8601()
                )

            Rsp(Ok = true, Msg = "", Data = data) |> Ok
        else
            $"Operation failed: Permission denied (comment id:{comment.Id})"
            |> effect logger.LogError
            |> Err
    | Err msg -> Err msg
