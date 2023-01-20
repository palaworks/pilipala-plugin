module grpc.api.token.service

open System.Threading.Tasks
open Grpc.Core

open grpc_code_gen.token
open grpc.api.token
open fsharper.op
open fsharper.typ
open pilipala
open token

type Ctx = ServerCallContext

let make (token_handler: TokenHandler) =
    { new grpc_code_gen.token.TokenService.TokenServiceBase() with

        override self.GetOne(req: get_one.Req, ctx: Ctx) =
            grpc.api.token.get_one.handler token_handler req ctx
            |> unwrapOrEval
            <| fun msg -> grpc_code_gen.token.get_one.Rsp(Ok = false, Msg = msg)
            |> Task.FromResult }

    |> TokenService.BindService
