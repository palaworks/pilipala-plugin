module grpc.api.token.get

open grpc_code_gen.token.get
open Grpc.Core
open fsharper.typ
open pilipala
open token

type Ctx = ServerCallContext

let handler (token_handler: TokenHandler) (req: Req) (ctx: Ctx) =

    Rsp() |> Ok
