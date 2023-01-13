module grpc.api.comment.update

open System
open grpc_code_gen.comment.update
open Grpc.Core
open fsharper.typ
open pilipala.access.user
open pilipala.util.text.time
open pilipala.util.hash.sha256

type Ctx = ServerCallContext

let handler (user: IUser) (req: Req) (ctx: Ctx) =
    match user.GetComment(req.Id) with
    | Ok comment ->
        if comment.CanRead && comment.CanWrite then
            
            comment.Body.fmap
            <| fun old ->
                if old.sha256.sha256 <> req.Body.sha256.sha256 then
                    comment.UpdateBody req.Body |> ignore //TODO handle result
            |> ignore

            let data =
                grpc_code_gen.comment.get.T(
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
