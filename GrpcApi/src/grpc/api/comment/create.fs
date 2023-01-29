module grpc.api.comment.create

open System
open fsharper.op
open fsharper.typ
open plugin.grpc.alias
open pilipala.access.user
open pilipala.util.text.time
open pilipala.container.comment
open Microsoft.Extensions.Logging
open grpc_code_gen.comment.create
open grpc_code_gen.comment.message

let handler (user: IUser) (req: Req) (ctx: Ctx) (logger: ILogger) =
    let result =
        if req.IsReply then
            match user.GetComment req.BindingId with
            | Ok comment -> comment.NewComment req.Body
            | Err msg -> Err msg
        else
            match user.GetPost req.BindingId with
            | Ok post -> post.NewComment req.Body
            | Err msg -> Err msg

    match result with
    | Ok comment ->

        let bindingId, isReply =
            comment.Binding.fmap
            <| fun x ->
                match x with
                | BindPost id -> id, false
                | BindComment id -> id, true
            |> unwrapOrEval
            <| fun _ ->
                $"Unknown error: can not read {nameof comment.Binding} (comment id:{comment.Id})"
                |> logger.LogError

                0, false

        let data =
            Comment(
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
                BindingId = bindingId,
                IsReply = isReply
            )

        Rsp(Ok = true, Msg = "", Data = data) |> Ok
    | Err msg -> Err msg
