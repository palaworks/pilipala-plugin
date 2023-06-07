module plugin.credential

open Grpc.Core
open System.IO
open plugin.cfg
open fsharper.typ
open System.Collections.Generic

let get_credentials (cfg: Cfg) : ServerCredentials =
    
    if cfg.enable_ssl then
        
        let root_cert = File.ReadAllText cfg.root_cert_path
        let pem_cert_chain = File.ReadAllText cfg.pem_cert_chain_path
        let pem_private_key = File.ReadAllText cfg.pem_private_key_path

        let key_pairs =
            List<KeyCertificatePair>()
                .effect (fun list -> KeyCertificatePair(pem_cert_chain, pem_private_key) |> list.Add)

        SslServerCredentials(key_pairs, root_cert, false)
    else
        ServerCredentials.Insecure
