module grpc.api.comment.get_one

open System
open grpc_code_gen.comment.get_one
open Grpc.Core
open fsharper.typ
open pilipala.access.user
open pilipala.util.text.time

type Ctx = ServerCallContext

let handler (user: IUser) (req: Req) (ctx: Ctx) =
    match user.GetComment(req.Id) with
    | Ok comment ->
        if comment.CanRead then
            let data =
                T(
                    Id = comment.Id,
                    Body = comment.Body.unwrapOrEval (fun _ -> $"Unknown error: can not read comment({comment.Id})"),
                    CreateTime =
                        comment
                            .CreateTime
                            .unwrapOrEval(fun _ -> DateTime.UnixEpoch)
                            .ToIso8601()
                )

            Rsp(Ok = true, Msg = "", Data = data) |> Ok
        else
            Err "Operation failed: Permission denied"
    | Err msg -> Err msg
