module grpc.api.comment.getAll

open grpc_code_gen.comment.get_all
open Grpc.Core

type Ctx = ServerCallContext

let handler (req: Req) (ctx: Ctx) = Rsp()
