module grpc.api.token.get_one

open grpc_code_gen.token.get_one
open Grpc.Core
open fsharper.typ
open pilipala
open token

type Ctx = ServerCallContext

let handler (token_handler: TokenHandler) (req: Req) (ctx: Ctx) =
    match token_handler.NewToken req.Uid req.Pwd with
    | Ok token -> Rsp(Ok = true, Msg = "", Value = token) |> Ok
    | Err msg -> Err msg
