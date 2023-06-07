module grpc.api.post.getSome

open System
open fsharper.op
open fsharper.typ
open plugin.grpc.alias
open pilipala.access.user
open pilipala.util.text.time
open grpc_code_gen.post.message
open grpc_code_gen.post.get_some
open Microsoft.Extensions.Logging

let handler (user: IUser) (req: Req) (ctx: Ctx) (logger: ILogger) =
    let posts = user.GetReadablePost()//TODO optimize performance

    let dataList =
        posts.foldl
        <| fun acc post ->
            if req.IdList.Contains(post.Id) then
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
                :: acc
            else
                acc
        <| []

    Rsp(Ok = true, Msg = "").effect <| fun rsp -> rsp.DataList.AddRange dataList
    |> Ok
