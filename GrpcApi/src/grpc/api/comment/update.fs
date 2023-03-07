module grpc.api.comment.update

open System
open fsharper.typ
open plugin.grpc.alias
open pilipala.access.user
open pilipala.util.hash.sha256
open Microsoft.Extensions.Logging
open grpc_code_gen.comment.update

let handler (user: IUser) (req: Req) (ctx: Ctx) (logger: ILogger) =
    match user.GetComment(req.Id) with
    | Ok comment ->
        if comment.CanRead && comment.CanWrite then
            comment.Body.fmap
            <| fun old ->
                if old.sha256.sha256 <> req.Body.sha256.sha256 then
                    comment.UpdateBody req.Body |> ignore //TODO handle result
            |> ignore

            Rsp(Ok = true, Msg = "") |> Ok
        else
            $"Operation failed: Permission denied (comment id:{comment.Id})"
            |> effect logger.LogError
            |> Exception
            |> Err
    | Err e -> Err e
