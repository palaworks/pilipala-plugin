module grpc.api.post.delete

open Grpc.Core
open fsharper.typ
open pilipala.access.user
open grpc_code_gen.post.delete
open Microsoft.Extensions.Logging

type Ctx = ServerCallContext

let handler (user: IUser) (req: Req) (ctx: Ctx) (logger: ILogger) =
    match user.GetPost req.Id with
    | Ok post ->
        match post.Drop() with
        | Ok _ -> Rsp(Ok = true, Msg = "") |> Ok
        | Err msg -> Err msg
    | Err msg -> Err msg
