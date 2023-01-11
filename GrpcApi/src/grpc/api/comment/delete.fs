module grpc.api.comment.delete

open grpc_code_gen.comment.delete
open Grpc.Core
open fsharper.typ
open pilipala.access.user

type Ctx = ServerCallContext

let handler (user: IUser) (req: Req) (ctx: Ctx) = Rsp() |> Ok
