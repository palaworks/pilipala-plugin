module grpc.api.post.create

open System
open grpc_code_gen.post.create
open Grpc.Core
open fsharper.typ
open pilipala.access.user
open pilipala.util.text.time

type Ctx = ServerCallContext

let handler (user: IUser) (req: Req) (ctx: Ctx) =
    match user.NewPost req.Title req.Body with
    | Ok post ->
        let data =
            grpc_code_gen.post.get.T(
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
    | Err msg -> Err msg
