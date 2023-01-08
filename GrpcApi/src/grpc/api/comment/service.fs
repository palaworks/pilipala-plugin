module grpc.api.comment.service

open System.Threading.Tasks
open Grpc.Core

open grpc_code_gen.comment
open grpc.api.comment

type Ctx = ServerCallContext

type CommentService() =
    inherit grpc_code_gen.comment.CommentService.CommentServiceBase()

    override self.Get(req: get.Req, ctx: Ctx) = get.handler req ctx |> Task.FromResult

    override self.GetAll(req: get_all.Req, ctx: Ctx) =
        getAll.handler req ctx |> Task.FromResult

    override self.GetAllSha256(req: get_all_sha256.Req, ctx: Ctx) =
        getAllSha256.handler req ctx |> Task.FromResult

    override self.Create(req: create.Req, ctx: Ctx) =
        create.handler req ctx |> Task.FromResult

    override self.Update(req: update.Req, ctx: Ctx) =
        update.handler req ctx |> Task.FromResult

    override self.Delete(req: delete.Req, ctx: Ctx) =
        delete.handler req ctx |> Task.FromResult
