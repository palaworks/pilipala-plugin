namespace pilipala.plugin

open System
open System.Net
open System.Threading.Tasks
open System.Net.Mail
open fsharper.typ
open fsharper.alias
open pilipala.access.user
open pilipala.container.post
open pilipala.pipeline
open pilipala.util.text
open pilipala.pipeline.comment
open pilipala.container.comment
open helper

type Cfg =
    { smtp_host: string
      smtp_port: i32
      sender_email: string
      sender_email_pwd: string
      receiver_email: string }

[<HookOn(AppLifeCycle.BeforeBuild)>]
type EmailNotifier
    (
        commentInitBuilder: ICommentInitPipelineBuilder,
        mappedCommentProvider: IMappedCommentProvider,
        mappedPostProvider: IMappedPostProvider,
        mappedUserProvider: IMappedUserProvider,
        cfg_provider: IPluginCfgProvider
    ) =

    let cfg =
        { json = cfg_provider.config }
            .deserializeTo<Cfg> ()

    do
        match cfg with
        | Ok cfg ->
            let send comment_id =

                fun _ ->
                    let mapped_comment =
                        mappedCommentProvider.fetch comment_id

                    use smtp =
                        let nc =
                            NetworkCredential(cfg.sender_email, cfg.sender_email_pwd)

                        new SmtpClient(
                            Host = cfg.smtp_host,
                            Port = cfg.smtp_port,
                            UseDefaultCredentials = false,
                            Credentials = nc
                        )

                    let mail_body =
                        body_builder mappedPostProvider mappedCommentProvider mappedUserProvider mapped_comment

                    smtp.Send(cfg.sender_email, cfg.receiver_email, "噼哩啪啦事务局", mail_body)
                |> Task.RunIgnore

            commentInitBuilder.Batch.collection.Add
            <| After(effect send)
        | _ -> ()
