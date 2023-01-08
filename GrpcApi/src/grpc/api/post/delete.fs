module grpc.api.post.delete

open grpc_code_gen.post.delete
open Grpc.Core

type Ctx = ServerCallContext

let handler (req: Req) (ctx: Ctx) = Rsp()
