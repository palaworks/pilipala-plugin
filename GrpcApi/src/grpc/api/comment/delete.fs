module grpc.api.comment.delete

open grpc_code_gen.comment.delete
open Grpc.Core
open fsharper.typ

type Ctx = ServerCallContext

let handler (req: Req) (ctx: Ctx) = Rsp() |> Ok
