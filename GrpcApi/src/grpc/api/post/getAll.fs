module grpc.api.post.getAll

open grpc_code_gen.post.get_all
open Grpc.Core
open fsharper.typ
open pilipala.access.user

type Ctx = ServerCallContext

let handler (user: IUser) (req: Req) (ctx: Ctx) = Rsp() |> Ok
