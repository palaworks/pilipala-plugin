module grpc.api.post.getOne

open System
open fsharper.typ
open plugin.grpc.alias
open pilipala.access.user
open pilipala.util.text.time
open grpc_code_gen.post.message
open grpc_code_gen.post.get_one
open Microsoft.Extensions.Logging

let handler (user: IUser) (req: Req) (ctx: Ctx) (logger: ILogger) =
    match user.GetPost(req.Id) with
    | Ok post ->
        if post.CanRead then
            let data =
                Post(
                    Id = post.Id,
                    Title =
                        post.Title.unwrapOrEval (fun _ ->
                            $"Unknown error: can not read {nameof post.Title} (post id:{post.Id})"
                            |> effect logger.LogError),
                    Body =
                        post.Body.unwrapOrEval (fun _ ->
                            $"Unknown error: can not read {nameof post.Body} (post id:{post.Id})"
                            |> effect logger.LogError),
                    CreateTime =
                        post
                            .CreateTime
                            .unwrapOrEval(fun _ ->
                                DateTime.UnixEpoch.effect
                                <| fun _ ->
                                    logger.LogError
                                        $"Unknown error: can not read {nameof post.CreateTime} (post id:{post.Id})")
                            .ToIso8601(),
                    ModifyTime =
                        post
                            .ModifyTime
                            .unwrapOrEval(fun _ ->
                                DateTime.UnixEpoch.effect
                                <| fun _ ->
                                    logger.LogError
                                        $"Unknown error: can not read {nameof post.ModifyTime} (post id:{post.Id})")
                            .ToIso8601()
                )

            Rsp(Ok = true, Msg = "", Data = data) |> Ok
        else
            $"Operation failed: Permission denied (post id:{post.Id})"
            |> effect logger.LogError
            |> Exception
            |> Err
    | Err e -> Err e
