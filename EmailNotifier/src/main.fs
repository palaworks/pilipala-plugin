namespace pilipala.plugin

open System.Net.Mail
open fsharper.op
open fsharper.typ
open fsharper.alias
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

type EmailNotifier
    (
        commentInitBuilder: ICommentInitPipelineBuilder,
        mappedCommentProvider: IMappedCommentProvider,
        cfg: IPluginCfgProvider
    ) =

    let config =
        { json = cfg.config }.deserializeTo<Config> ()

    do
        match config with
        | Some config ->
            let send (id: i64) =
                use smtp =
                    new SmtpClient(
                        Host = config.host,
                        Port = config.port,
                        UseDefaultCredentials = false,
                        Credentials = System.Net.NetworkCredential(config.usr, config.pwd)
                    )

                let mapped = mappedCommentProvider.fetch id
                smtp.Send("噼里啪啦事件", config.receiver, "新的评论", mapped.Body)

            commentInitBuilder.Batch.collection.Add
            <| After(effect send)
        | _ -> ()
