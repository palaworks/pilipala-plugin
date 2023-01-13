module grpc.api.post.get

open System
open grpc_code_gen.post.get
open Grpc.Core
open fsharper.typ
open pilipala.access.user
open pilipala.util.text.time

type Ctx = ServerCallContext

let handler (user: IUser) (req: Req) (ctx: Ctx) =
    match user.GetPost(req.Id) with
    | Ok post ->
        if post.CanRead then
            let data =
                T(
                    Id = post.Id,
                    Title = post.Title.unwrapOrEval (fun _ -> $"Unknown error: can not read post({post.Id})"),
                    Body = post.Title.unwrapOrEval (fun _ -> $"Unknown error: can not read post({post.Id})"),
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
