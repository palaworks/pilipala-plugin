module grpc.api.comment.delete

open System
open grpc_code_gen.comment.delete
open Grpc.Core
open fsharper.typ
open pilipala.access.user
open pilipala.util.text.time
open pilipala.util.hash.sha256

type Ctx = ServerCallContext

let handler (user: IUser) (req: Req) (ctx: Ctx) = Rsp() |> Ok
