module ws.server

open System.Net
open System.Security.Cryptography.X509Certificates
open fsharper.typ
open Suave
open Suave.Http
open Suave.Operators
open Suave.Filters
open Suave.Successful
open Suave.Files
open Suave.RequestErrors
open Suave.Logging
open Suave.Utils
open fsharper.alias

let wsLocalServer (port: u16) routing =
    let ipAddr = IPAddress.Loopback

    startWebServer
        { defaultConfig with
            bindings = [ HttpBinding.create HTTP ipAddr port ] }
        routing

    (ipAddr, port)

let wsPublicServer (port: u16) pem_path key_path routing =
    let x509Cert = X509Certificate2.CreateFromPemFile(pem_path, key_path)
    let ipAddr = IPAddress.Loopback

    startWebServer
        { defaultConfig with
            bindings = [ HttpBinding.create (HTTPS x509Cert) ipAddr port ]
            tlsProvider = DefaultTlsProvider() }
        routing

    (ipAddr, port)

(*
    match cert_path with
    | Some(pem_path, key_path) ->

        let X509cert = X509Certificate2.CreateFromPemFile(pem_path, key_path)

        WebSocketServer(IPAddress.Any, port, true)
        |> effect (fun x -> x.SslConfiguration.ServerCertificate <- X509cert)

    | None -> WebSocketServer(IPAddress.Any, port, false)
    *)
