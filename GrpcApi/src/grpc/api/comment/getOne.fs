module grpc.api.comment.get_one

open System
open fsharper.typ
open plugin.grpc.alias
open pilipala.access.user
open pilipala.container.comment
open pilipala.util.text.time
open Microsoft.Extensions.Logging
open grpc_code_gen.comment.get_one

let handler (user: IUser) (req: Req) (ctx: Ctx) (logger: ILogger) =
    match user.GetComment(req.Id) with
    | Ok comment ->
        if comment.CanRead then
            let data =

                let binding_id, is_reply =
                    comment
                        .Binding
                        .fmap(fun x ->
                            match x with
                            | BindPost id -> id, false
                            | BindComment id -> id, true)
                        .unwrapOrEval (fun _ ->
                            $"Unknown error: can not read {nameof comment.Binding} (comment id:{comment.Id})"
                            |> logger.LogError

                            0, false)

                T(
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
                            .ToIso8601(),
                    BindingId = binding_id,
                    IsReply = is_reply
                )

            Rsp(Ok = true, Msg = "", Data = data) |> Ok
        else
            $"Operation failed: Permission denied (comment id:{comment.Id})"
            |> effect logger.LogError
            |> Err
    | Err msg -> Err msg
