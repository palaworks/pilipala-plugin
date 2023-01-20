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
    let token_check_failed token =
        $"Operation failed: Token({token}) not exist or expired"

    { new grpc_code_gen.post.PostService.PostServiceBase() with

        override self.GetOne(req: get_one.Req, ctx: Ctx) =
            match token_handler.GetUser req.Token with
            | Some user ->
                grpc.api.post.get_one.handler user req ctx
                |> unwrapOrEval
                <| fun msg -> grpc_code_gen.post.get_one.Rsp(Ok = false, Msg = msg)
            | None -> grpc_code_gen.post.get_one.Rsp(Ok = false, Msg = token_check_failed req.Token)
            |> Task.FromResult

        override self.GetAll(req: get_all.Req, ctx: Ctx) =
            match token_handler.GetUser req.Token with
            | Some user ->
                getAll.handler user req ctx |> unwrapOrEval
                <| fun msg -> get_all.Rsp(Ok = false, Msg = msg)
            | None -> get_all.Rsp(Ok = false, Msg = token_check_failed req.Token)
            |> Task.FromResult

        override self.GetAllSha256(req: get_all_sha256.Req, ctx: Ctx) =
            match token_handler.GetUser req.Token with
            | Some user ->
                getAllSha256.handler user req ctx |> unwrapOrEval
                <| fun msg -> get_all_sha256.Rsp(Ok = false, Msg = msg)
            | None -> get_all_sha256.Rsp(Ok = false, Msg = token_check_failed req.Token)
            |> Task.FromResult

        override self.Create(req: create.Req, ctx: Ctx) =
            match token_handler.GetUser req.Token with
            | Some user ->
                create.handler user req ctx |> unwrapOrEval
                <| fun msg -> create.Rsp(Ok = false, Msg = msg)
            | None -> create.Rsp(Ok = false, Msg = token_check_failed req.Token)
            |> Task.FromResult

        override self.Update(req: update.Req, ctx: Ctx) =
            match token_handler.GetUser req.Token with
            | Some user ->
                update.handler user req ctx |> unwrapOrEval
                <| fun msg -> update.Rsp(Ok = false, Msg = msg)
            | None -> update.Rsp(Ok = false, Msg = token_check_failed req.Token)
            |> Task.FromResult

        override self.Delete(req: delete.Req, ctx: Ctx) =
            match token_handler.GetUser req.Token with
            | Some user ->
                delete.handler user req ctx |> unwrapOrEval
                <| fun msg -> delete.Rsp(Ok = false, Msg = msg)
            | None -> delete.Rsp(Ok = false, Msg = token_check_failed req.Token)
            |> Task.FromResult }

    |> PostService.BindService
