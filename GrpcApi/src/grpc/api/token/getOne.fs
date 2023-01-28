module grpc.api.token.get_one

open pilipala
open fsharper.typ
open plugin.token
open plugin.grpc.alias
open grpc_code_gen.token.get_one
open Microsoft.Extensions.Logging

let handler (token_handler: TokenHandler) (req: Req) (ctx: Ctx) (logger: ILogger) =
    match token_handler.NewToken req.Uid req.Pwd with
    | Ok token -> Rsp(Ok = true, Msg = "", Value = token) |> Ok
    | Err msg -> Err msg
