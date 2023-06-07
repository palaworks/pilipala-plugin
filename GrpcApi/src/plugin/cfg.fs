module plugin.cfg

open fsharper.alias

type Cfg =
    { host: string
      port: u16
      enable_ssl: bool
      root_cert_path: string
      pem_cert_chain_path: string
      pem_private_key_path: string }
