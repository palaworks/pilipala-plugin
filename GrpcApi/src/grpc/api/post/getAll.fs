module grpc.api.post.getAll

open grpc_code_gen.post.get_all
open Grpc.Core

type Ctx = ServerCallContext

let handler (req: Req) (ctx: Ctx) = Rsp()
