module grpc.api.token.service

open pilipala
open Grpc.Core
open fsharper.op
open fsharper.typ
open grpc.api.token
open grpc_code_gen.token
open System.Threading.Tasks
open Microsoft.Extensions.Logging

open plugin.token

type Ctx = ServerCallContext

let make (token_handler: TokenHandler) (logger: ILogger) =
    { new grpc_code_gen.token.TokenService.TokenServiceBase() with

        override self.GetOne(req: get_one.Req, ctx: Ctx) =
            grpc.api.token.get_one.handler token_handler req ctx logger |> unwrapOrEval
            <| fun msg -> grpc_code_gen.token.get_one.Rsp(Ok = false, Msg = msg)
            |> Task.FromResult }

    |> TokenService.BindService
