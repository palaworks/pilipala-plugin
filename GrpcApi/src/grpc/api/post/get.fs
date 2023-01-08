module grpc.api.post.get

open grpc_code_gen.post.get
open Grpc.Core

type Ctx = ServerCallContext

let handler (req: Req) (ctx: Ctx) = Rsp()
