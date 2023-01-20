module grpc.api.post.update

open System
open grpc_code_gen.post.update
open Grpc.Core
open fsharper.typ
open pilipala.access.user
open pilipala.util.text.time
open pilipala.util.hash.sha256

type Ctx = ServerCallContext

let handler (user: IUser) (req: Req) (ctx: Ctx) =
    match user.GetPost(req.Id) with
    | Ok post ->

        if post.CanRead && post.CanWrite then

            post.Title.fmap
            <| fun old ->
                if old.sha256.sha256 <> req.Title.sha256.sha256 then
                    post.UpdateTitle req.Title |> ignore //TODO handle result
            |> ignore

            post.Body.fmap
            <| fun old ->
                if old.sha256.sha256 <> req.Body.sha256.sha256 then
                    post.UpdateBody req.Body |> ignore //TODO handle result
            |> ignore

            let data =
                grpc_code_gen.post.get_one.T(
                    Id = post.Id,
                    Title = post.Title.unwrapOrEval (fun _ -> $"Unknown error: can not read post({post.Id})"),
                    Body = post.Body.unwrapOrEval (fun _ -> $"Unknown error: can not read post({post.Id})"),
                    CreateTime =
                        post
                            .CreateTime
                            .unwrapOrEval(fun _ -> DateTime.UnixEpoch)
                            .ToIso8601(),
                    ModifyTime =
                        post
                            .ModifyTime
                            .unwrapOrEval(fun _ -> DateTime.UnixEpoch)
                            .ToIso8601()
                )

            Rsp(Ok = true, Msg = "", Data = data) |> Ok
        else
            Err "Operation failed: Permission denied"
    | Err msg -> Err msg
