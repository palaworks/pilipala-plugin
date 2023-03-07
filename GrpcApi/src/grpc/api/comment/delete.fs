module grpc.api.comment.delete

open fsharper.typ
open plugin.grpc.alias
open pilipala.access.user
open Microsoft.Extensions.Logging
open grpc_code_gen.comment.delete

let handler (user: IUser) (req: Req) (ctx: Ctx) (logger: ILogger) =
    match user.GetComment req.Id with
    | Ok post ->
        match post.Drop() with
        | Ok _ -> Rsp(Ok = true, Msg = "") |> Ok
        | Err e -> Err e
    | Err e -> Err e
