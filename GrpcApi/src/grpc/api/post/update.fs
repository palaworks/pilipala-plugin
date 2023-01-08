module grpc.api.post.update

open grpc_code_gen.post.update
open Grpc.Core
open fsharper.typ

type Ctx = ServerCallContext

let handler (req: Req) (ctx: Ctx) = Rsp() |> Ok
