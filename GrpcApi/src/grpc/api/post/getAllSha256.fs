module grpc.api.post.getAllSha256

open grpc_code_gen.post.get_all_sha256
open Grpc.Core
open fsharper.typ
open pilipala.access.user

type Ctx = ServerCallContext

let handler (user: IUser) (req: Req) (ctx: Ctx) = Rsp() |> Ok
