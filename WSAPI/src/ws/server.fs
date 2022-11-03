module ws.server

open System.Net
open System.Security.Cryptography.X509Certificates
open WebSocketSharp.Server
open fsharper.typ

let wsLocalServer port =
    WebSocketServer(IPAddress.Loopback, port, false)

let wsPublicServer port cert_path =

    match cert_path with
    | Some (pem_path, key_path) ->

        let X509cert =
            X509Certificate2.CreateFromPemFile(pem_path, key_path)

        WebSocketServer(IPAddress.Any, port, true)
        |> effect (fun x -> x.SslConfiguration.ServerCertificate <- X509cert)

    | None -> WebSocketServer(IPAddress.Any, port, false)
