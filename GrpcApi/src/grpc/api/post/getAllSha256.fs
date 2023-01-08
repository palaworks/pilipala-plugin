module grpc.api.post.getAllSha256

open grpc_code_gen.post.get_all_sha256
open Grpc.Core
open fsharper.typ

type Ctx = ServerCallContext

let handler (req: Req) (ctx: Ctx) = Rsp() |> Ok
