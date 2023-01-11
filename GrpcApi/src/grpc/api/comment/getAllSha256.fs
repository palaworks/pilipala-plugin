module grpc.api.comment.getAllSha256

open grpc_code_gen.comment.get_all_sha256
open Grpc.Core
open fsharper.typ
open pilipala.access.user

type Ctx = ServerCallContext

let handler (user: IUser) (req: Req) (ctx: Ctx) = Rsp() |> Ok
