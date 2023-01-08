module grpc.api.comment.delete

open grpc_code_gen.comment.delete
open Grpc.Core

type Ctx = ServerCallContext

let handler (req: Req) (ctx: Ctx) = Rsp()
