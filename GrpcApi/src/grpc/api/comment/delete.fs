module grpc.api.comment.delete

open System
open Microsoft.Extensions.Logging
open grpc_code_gen.comment.delete
open Grpc.Core
open fsharper.typ
open pilipala.access.user
open pilipala.util.text.time
open pilipala.util.hash.sha256

type Ctx = ServerCallContext

let handler (user: IUser) (req: Req) (ctx: Ctx) (logger: ILogger) =
    match user.GetComment req.Id with
    | Ok post ->
        match post.Drop() with
        | Ok _ -> Rsp(Ok = true, Msg = "") |> Ok
        | Err msg -> Err msg
    | Err msg -> Err msg
