module grpc.api.comment.update

open grpc_code_gen.comment.update
open Grpc.Core

type Ctx = ServerCallContext

let handler (req: Req) (ctx: Ctx) = Rsp()
