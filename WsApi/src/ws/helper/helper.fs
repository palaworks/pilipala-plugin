namespace ws.helper

open fsharper.typ
open fsharper.alias

type EmptyReq() =
    class
    end

type ApiRequest<'req> = { Seq: i64; Data: 'req }

type ApiResponse<'rsp> =
    { Seq: i64
      Ok: bool
      Msg: string
      Data: 'rsp }

type IApiHandler<'h, 'req, 'rsp> =
    abstract handle: 'req -> Result'<'rsp, string>
