module grpc.api.comment.create

open System
open grpc_code_gen.comment.create
open Grpc.Core
open fsharper.typ
open pilipala.access.user
open pilipala.util.text.time


type Ctx = ServerCallContext

let handler (user: IUser) (req: Req) (ctx: Ctx) =
    if req.IsReply then
        Rsp(Ok = true, Msg = "") |> Ok
    else
        match user.GetPost req.BindingId with
        | Ok post ->
            match post.NewComment req.Body with
            | Ok comment ->
                let data =
                    grpc_code_gen.comment.get.T(
                        Id = comment.Id,
                        Body = comment.Body.unwrapOrEval (fun _ -> $"Unknown error: can not read post({post.Id})"),
                        CreateTime =
                            comment
                                .CreateTime
                                .unwrapOrEval(fun _ -> DateTime.UnixEpoch)
                                .ToIso8601()
                    )

                Rsp(Ok = true, Msg = "", Data = data) |> Ok
            | Err msg -> Err msg
        | Err msg -> Err msg
