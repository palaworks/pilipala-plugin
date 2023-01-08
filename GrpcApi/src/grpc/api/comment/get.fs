module grpc.api.comment.get

open grpc_code_gen.comment.get
open Grpc.Core
open fsharper.typ

type Ctx = ServerCallContext

let handler (req: Req) (ctx: Ctx) = Rsp() |> Ok
