module grpc.api.comment.getSome

open System
open fsharper.op
open fsharper.typ
open plugin.grpc.alias
open pilipala.access.user
open pilipala.util.text.time
open pilipala.container.comment
open Microsoft.Extensions.Logging
open grpc_code_gen.comment.message
open grpc_code_gen.comment.get_some

let handler (user: IUser) (req: Req) (ctx: Ctx) (logger: ILogger) =
    let posts = user.GetReadableComment() //TODO optimize performance

    let dataList =
        posts.foldl
        <| fun acc comment ->
            if req.IdList.Contains(comment.Id) then

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
                :: acc
            else
                acc
        <| []

    Rsp(Ok = true, Msg = "").effect <| fun rsp -> rsp.DataList.AddRange dataList
    |> Ok
