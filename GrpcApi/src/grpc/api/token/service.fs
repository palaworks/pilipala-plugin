module grpc.api.token.service

open pilipala
open fsharper.op
open plugin.token
open grpc.api.token
open plugin.grpc.alias
open grpc_code_gen.token
open System.Threading.Tasks
open Microsoft.Extensions.Logging

let make (token_handler: TokenHandler) (logger: ILogger) =
    { new grpc_code_gen.token.TokenService.TokenServiceBase() with

        override self.GetOne(req: get_one.Req, ctx: Ctx) =
            getOne.handler token_handler req ctx logger |> unwrapOrEval
            <| fun e -> grpc_code_gen.token.get_one.Rsp(Ok = false, Msg = e.Message)
            |> Task.FromResult }

    |> TokenService.BindService
