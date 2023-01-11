module grpc.api.post.create

open grpc_code_gen.post.create
open Grpc.Core
open fsharper.typ
open pilipala.access.user

type Ctx = ServerCallContext

let handler (user: IUser) (req: Req) (ctx: Ctx) = Rsp() |> Ok
