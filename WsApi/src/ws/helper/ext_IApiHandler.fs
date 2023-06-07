[<AutoOpen>]
module ws.helper.ext_IApiHandler

open System
open Suave
open Suave.Sockets
open Suave.Sockets.Control
open Suave.WebSocket
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Microsoft.Extensions.Logging
open pilipala.util.text
open fsharper.typ
open pilipala.util.encoding.byte


type IApiHandler<'h, 'req, 'rsp> with

    member self.toWsBehavior
        (enable_api_response_detail_logging: bool)
        (logger: ILogger)
        (webSocket: WebSocket)
        (ctx: HttpContext)
        =
        socket {
            let mutable loop = true

            while loop do
                let! msg = webSocket.read ()

                match msg with
                | Text, data, true ->
                    let msg = UTF8.toString data

                    $"recv {typeof<'h>.FullName} req:\n{msg}" |> logger.LogInformation

                    let opt_api_req = { json = msg }.deserializeTo<ApiRequest<_>> ()

                    let result =
                        match opt_api_req with
                        | Ok api_req -> self.handle api_req.Data
                        | Err e ->
                            logger.LogError msg
                            "Invalid api request" |> Exception |> Err

                    let api_rsp =
                        match result with
                        | Ok x ->
                            { Seq = opt_api_req.unwrap().Seq
                              Ok = true
                              Msg = "Success"
                              Data = x }
                        | Err e ->
                            { Seq =
                                //TODO 不优雅
                                match opt_api_req with
                                | Ok api_req -> api_req.Seq
                                | _ -> -1
                              Ok = false
                              Msg = msg
                              Data = Unchecked.defaultof<'rsp> }

                    let json = api_rsp.serializeToJson().json

                    let byteResponse = json |> utf8ToBytes |> ByteSegment
                    do! webSocket.send Text byteResponse true

                    if enable_api_response_detail_logging then
                        $"sent {typeof<'h>.FullName} rsp:\n{json}" |> logger.LogInformation
                    else
                        $"sent {typeof<'h>.FullName} rsp." |> logger.LogInformation
                | Close, _, _ ->
                    let rsp = [||] |> ByteSegment
                    do! webSocket.send Close rsp true
                    loop <- false

                | _ -> ()
        }
