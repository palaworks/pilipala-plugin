module plugin.cfg

open fsharper.alias

type Cfg =
    { pl_display_user: string
      pl_display_pwd: string
      pl_comment_user: string
      pl_comment_pwd: string
      ws_local_port: u16
      ws_public_port: u16
      ws_public_enable_ssl: bool
      ws_cert_pem_path: string
      ws_cert_key_path: string
      enable_api_response_detail_logging: bool }
