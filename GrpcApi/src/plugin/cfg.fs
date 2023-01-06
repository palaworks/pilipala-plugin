module plugin.cfg

open fsharper.alias

type Cfg =
    { listen_port: string
      enable_ssl: string
      pem_cert_chain_path: string
      pem_private_key_path: string }
