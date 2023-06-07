namespace ws.api.comment.create

open fsharper.alias

type Req =
    { Binding: string
      IsReply: bool
      Body: string }
