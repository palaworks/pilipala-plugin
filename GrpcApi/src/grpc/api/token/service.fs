module grpc.api.token.service

open System.Threading.Tasks
open Grpc.Core

open grpc_code_gen.token
open grpc.api.token
open fsharper.op
open fsharper.typ

type Ctx = ServerCallContext

type TokenService() =
    inherit grpc_code_gen.token.TokenService.TokenServiceBase()

    override self.Get(req: get.Req, ctx: Ctx) =
        grpc.api.token.get.handler req ctx |> unwrapOr
        <| fun msg -> grpc_code_gen.token.get.Rsp(Ok = false, Msg = msg)
        |> Task.FromResult
