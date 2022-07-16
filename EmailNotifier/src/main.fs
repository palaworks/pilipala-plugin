namespace pilipala.plugin

open System.Net.Mail
open System.Reflection
open pilipala.util.io
open Newtonsoft.Json
open fsharper.op.Alias
open fsharper.typ.Pipe
open pilipala.pipeline
open pilipala.pipeline.comment
open pilipala.container.comment

type internal Config =
    { host: string
      port: i32
      usr: string
      pwd: string
      receiver: string }

type EmailNotifier(initBuilder: ICommentInitPipelineBuilder) =

    let path =
        let asmDir =
            Assembly
                .GetAssembly(typeof<EmailNotifier>)
                .Location.Replace($"{typeof<EmailNotifier>.Name}.dll", "")

        $"{asmDir}/config.json"

    let config =
        JsonConvert.DeserializeObject<Config>(readFile (path))

    let send (id: u64, comment: IComment) =
        let smtp = new SmtpClient()

        let client =
            new SmtpClient(
                Host = config.host,
                Port = config.port,
                UseDefaultCredentials = false,
                Credentials = System.Net.NetworkCredential(config.usr, config.pwd)
            )

        client.Send(usr, config.receiver, "新的评论", comment.Body)
        id, comment

    do initBuilder.Batch.collection.Add(After(Pipe(send)))
