module grpc.api.post.service

open System.Threading.Tasks
open Grpc.Core

open grpc_code_gen.post
open grpc.api.post
open fsharper.op
open fsharper.typ
open pilipala
open token

type Ctx = ServerCallContext
type M = grpc_code_gen.post.create.Req

let make (token_handler: TokenHandler) =
    { new grpc_code_gen.post.PostService.PostServiceBase() with

        override self.Get(req: get.Req, ctx: Ctx) =
            match token_handler.GetUser req.Token with
            | Some user ->
                grpc.api.post.get.handler user req ctx |> unwrapOr
                <| fun msg -> grpc_code_gen.post.get.Rsp(Ok = false, Msg = msg)
            | None -> grpc_code_gen.post.get.Rsp(Ok = false, Msg = "")
            |> Task.FromResult

        override self.GetAll(req: get_all.Req, ctx: Ctx) =
            match token_handler.GetUser req.Token with
            | Some user ->
                getAll.handler user req ctx |> unwrapOr
                <| fun msg -> get_all.Rsp()
            | None -> get_all.Rsp() //TODO
            |> Task.FromResult

        override self.GetAllSha256(req: get_all_sha256.Req, ctx: Ctx) =
            match token_handler.GetUser req.Token with
            | Some user ->
                getAllSha256.handler user req ctx |> unwrapOr
                <| fun msg -> get_all_sha256.Rsp()
            | None -> get_all_sha256.Rsp() //TODO
            |> Task.FromResult

        override self.Create(req: create.Req, ctx: Ctx) =
            match token_handler.GetUser req.Token with
            | Some user ->
                create.handler user req ctx |> unwrapOr
                <| fun msg -> create.Rsp(Ok = false, Msg = msg)
            | None -> create.Rsp() //TODO
            |> Task.FromResult

        override self.Update(req: update.Req, ctx: Ctx) =
            match token_handler.GetUser req.Token with
            | Some user ->
                update.handler user req ctx |> unwrapOr
                <| fun msg -> update.Rsp(Ok = false, Msg = msg)
            | None -> update.Rsp() //TODO
            |> Task.FromResult

        override self.Delete(req: delete.Req, ctx: Ctx) =
            match token_handler.GetUser req.Token with
            | Some user ->
                delete.handler user req ctx |> unwrapOr
                <| fun msg -> delete.Rsp(Ok = false, Msg = msg)
            | None -> delete.Rsp() //TODO
            |> Task.FromResult }

    |> PostService.BindService
