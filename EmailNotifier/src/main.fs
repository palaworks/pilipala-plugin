namespace pilipala.plugin

open System.Net.Mail
open fsharper.op.Alias
open fsharper.typ.Pipe
open pilipala.pipeline
open pilipala.util.text
open pilipala.pipeline.comment
open pilipala.container.comment

type internal Config =
    { host: string
      port: i32
      usr: string
      pwd: string
      receiver: string }

type EmailNotifier(initBuilder: ICommentInitPipelineBuilder, cfg: IPluginCfgProvider) =

    let config = { json = cfg.config }.deserializeTo<Config> ()

    let send (id: u64, comment: IComment) =
        use smtp =
            new SmtpClient(
                Host = config.host,
                Port = config.port,
                UseDefaultCredentials = false,
                Credentials = System.Net.NetworkCredential(config.usr, config.pwd)
            )

        smtp.Send("噼里啪啦事件", config.receiver, "新的评论", comment.Body)
        id, comment

    do initBuilder.Batch.collection.Add <| After send
