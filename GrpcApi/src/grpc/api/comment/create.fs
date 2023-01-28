module grpc.api.comment.create

open System
open fsharper.typ
open plugin.grpc.alias
open pilipala.access.user
open pilipala.util.text.time
open Microsoft.Extensions.Logging
open grpc_code_gen.comment.create

let handler (user: IUser) (req: Req) (ctx: Ctx) (logger: ILogger) =
    if req.IsReply then
        Rsp(Ok = true, Msg = "") |> Ok
    else
        match user.GetPost req.BindingId with
        | Ok post ->
            match post.NewComment req.Body with
            | Ok comment ->
                let data =
                    grpc_code_gen.comment.get_one.T(
                        Id = comment.Id,
                        Body =
                            comment.Body.unwrapOrEval (fun _ ->
                                $"Unknown error: can not read {nameof comment.Body} (comment id:{post.Id})"
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
            | Err msg -> Err msg
        | Err msg -> Err msg
