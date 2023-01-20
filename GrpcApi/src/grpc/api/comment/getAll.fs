module grpc.api.comment.getAll

open System
open grpc_code_gen.comment.get_all
open Grpc.Core
open fsharper.op
open fsharper.typ
open pilipala.access.user
open pilipala.util.text.time

type Ctx = ServerCallContext

let handler (user: IUser) (req: Req) (ctx: Ctx) =
    let comments = user.GetReadableComment()

    let collection =
        comments.foldl
        <| fun acc comment ->
            grpc_code_gen.comment.get_one.T(
                Id = comment.Id,
                Body = comment.Body.unwrapOrEval (fun _ -> $"Unknown error: can not read comment({comment.Id})"),
                CreateTime =
                    comment
                        .CreateTime
                        .unwrapOrEval(fun _ -> DateTime.UnixEpoch)
                        .ToIso8601()
            )
            :: acc
        <| []

    Rsp(Ok = true, Msg = "").effect
    <| fun rsp -> rsp.Collection.AddRange collection
    |> Ok
