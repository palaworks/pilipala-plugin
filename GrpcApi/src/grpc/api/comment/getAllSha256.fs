module grpc.api.comment.getAllSha256

open grpc_code_gen.comment.get_all_sha256
open Grpc.Core
open fsharper.typ

type Ctx = ServerCallContext

let handler (req: Req) (ctx: Ctx) = Rsp() |> Ok
