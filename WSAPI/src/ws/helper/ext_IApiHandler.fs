[<AutoOpen>]
module ws.helper.ext_IApiHandler

open System
open pilipala.util.text
open WebSocketSharp.Server
open fsharper.typ

type IApiHandler<'h, 'req, 'rsp> with
    member self.toWsBehavior() =
        { new WebSocketBehavior() with
            override b.OnMessage e =
                $"recv {typeof<'h>.FullName} req:\n{e.Data}"
                |> Console.WriteLine

                let opt_api_req =
                    { json = e.Data }.deserializeTo<ApiRequest<_>> ()
                //Some(JsonConvert.DeserializeObject<ApiRequest<_>> e.Data)

                let result =
                    match opt_api_req with
                    | Ok api_req -> self.handle api_req.Data
                    | _ -> Err "Invalid api request"

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

                (*
                $"send {typeof<'h>.FullName} rsp:\n{api_rsp.serializeToJson().json}"
                |> Console.WriteLine*)

                $"sent {typeof<'h>.FullName} rsp"
                |> Console.WriteLine

                b.Send(api_rsp.serializeToJson().json) }
