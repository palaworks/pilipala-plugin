[<AutoOpen>]
module ws.helper.ext_IApiHandler

open System
open pilipala.util.text
open WebSocketSharp.Server
open fsharper.typ

type IApiHandler<'h, 'req, 'rsp> with
    member self.toWsBehavior(enable_api_response_detail_logging: bool) =
        { new WebSocketBehavior() with
            override b.OnMessage e =
                $"recv {typeof<'h>.FullName} req:\n{e.Data}"
                |> Console.WriteLine

                let opt_api_req =
                    { json = e.Data }.deserializeTo<ApiRequest<_>> ()

                let result =
                    match opt_api_req with
                    | Ok api_req -> self.handle api_req.Data
                    | Err e ->
                        Console.WriteLine e
                        Err "Invalid api request"

                let api_rsp =
                    match result with
                    | Ok x ->
                        { Seq = opt_api_req.unwrap().Seq
                          Ok = true
                          Msg = "Success"
                          Data = x }
                    | Err msg ->
                        { Seq =
                            //TODO 不优雅
                            match opt_api_req with
                            | Ok api_req -> api_req.Seq
                            | _ -> -1
                          Ok = false
                          Msg = msg
                          Data = Unchecked.defaultof<'rsp> }

                let json = api_rsp.serializeToJson().json

                b.Send(json)

                if enable_api_response_detail_logging then
                    $"sent {typeof<'h>.FullName} rsp:\n{json}"
                    |> Console.WriteLine
                else
                    $"sent {typeof<'h>.FullName} rsp."
                    |> Console.WriteLine }
