module grpc.api.post.create

open grpc_code_gen.post.create
open Grpc.Core

type Ctx = ServerCallContext

let handler (req: Req) (ctx: Ctx) = Rsp()
