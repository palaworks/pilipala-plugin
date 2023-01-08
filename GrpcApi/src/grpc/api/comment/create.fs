module grpc.api.comment.create

open grpc_code_gen.comment.create
open Grpc.Core

type Ctx = ServerCallContext

let handler (req: Req) (ctx: Ctx) = Rsp()
