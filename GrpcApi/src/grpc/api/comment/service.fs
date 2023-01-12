module grpc.api.comment.service

open System.Threading.Tasks
open Grpc.Core

open grpc_code_gen.comment
open grpc.api.comment
open fsharper.op
open fsharper.typ
open pilipala
open token

type Ctx = ServerCallContext

let make (token_handler: TokenHandler) =
    let token_check_failed token =
        $"Operation failed: Token({token}) not exist or expired"

    { new grpc_code_gen.comment.CommentService.CommentServiceBase() with

        override self.Get(req: get.Req, ctx: Ctx) =
            match token_handler.GetUser req.Token with
            | Some user ->
                grpc.api.comment.get.handler user req ctx
                |> unwrapOr
                <| fun msg -> grpc_code_gen.comment.get.Rsp(Ok = false, Msg = msg)
            | None -> grpc_code_gen.comment.get.Rsp(Ok = false, Msg = token_check_failed req.Token)
            |> Task.FromResult

        override self.GetAll(req: get_all.Req, ctx: Ctx) =
            match token_handler.GetUser req.Token with
            | Some user ->
                getAll.handler user req ctx |> unwrapOr
                <| fun msg -> get_all.Rsp()
            | None -> get_all.Rsp(Ok = false, Msg = token_check_failed req.Token)
            |> Task.FromResult

        override self.GetAllSha256(req: get_all_sha256.Req, ctx: Ctx) =
            match token_handler.GetUser req.Token with
            | Some user ->
                getAllSha256.handler user req ctx |> unwrapOr
                <| fun msg -> get_all_sha256.Rsp()
            | None -> get_all_sha256.Rsp(Ok = false, Msg = token_check_failed req.Token)
            |> Task.FromResult

        override self.Create(req: create.Req, ctx: Ctx) =
            match token_handler.GetUser req.Token with
            | Some user ->
                create.handler user req ctx |> unwrapOr
                <| fun msg -> create.Rsp(Ok = false, Msg = msg)
            | None -> create.Rsp(Ok = false, Msg = token_check_failed req.Token)
            |> Task.FromResult

        override self.Update(req: update.Req, ctx: Ctx) =
            match token_handler.GetUser req.Token with
            | Some user ->
                update.handler user req ctx |> unwrapOr
                <| fun msg -> update.Rsp(Ok = false, Msg = msg)
            | None -> update.Rsp(Ok = false, Msg = token_check_failed req.Token)
            |> Task.FromResult

        override self.Delete(req: delete.Req, ctx: Ctx) =
            match token_handler.GetUser req.Token with
            | Some user ->
                delete.handler user req ctx |> unwrapOr
                <| fun msg -> delete.Rsp(Ok = false, Msg = msg)
            | None -> delete.Rsp(Ok = false, Msg = token_check_failed req.Token)
            |> Task.FromResult }

    |> CommentService.BindService
