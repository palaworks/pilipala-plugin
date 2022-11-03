module plugin.cfg

open fsharper.alias

type Cfg =
    { pl_display_user: string
      pl_display_pwd: string
      pl_comment_user: string
      pl_comment_pwd: string
      ws_local_port: i32
      ws_public_port: i32
      ws_public_ssl_enable: bool
      ws_cert_pem_path: string
      ws_cert_key_path: string }
