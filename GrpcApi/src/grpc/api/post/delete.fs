module grpc.api.post.delete

open fsharper.typ
open plugin.grpc.alias
open pilipala.access.user
open grpc_code_gen.post.delete
open Microsoft.Extensions.Logging

let handler (user: IUser) (req: Req) (ctx: Ctx) (logger: ILogger) =
    match user.GetPost req.Id with
    | Ok post ->
        match post.Drop() with
        | Ok _ -> Rsp(Ok = true, Msg = "") |> Ok
        | Err e -> Err e
    | Err e -> Err e
