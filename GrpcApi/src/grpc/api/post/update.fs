module grpc.api.post.update

open System
open fsharper.typ
open plugin.grpc.alias
open pilipala.access.user
open pilipala.util.hash.sha256
open grpc_code_gen.post.update
open Microsoft.Extensions.Logging

let handler (user: IUser) (req: Req) (ctx: Ctx) (logger: ILogger) =
    match user.GetPost(req.Id) with
    | Ok post ->
        if post.CanRead && post.CanWrite then
            post.Title.fmap
            <| fun old ->
                if old.sha256.sha256 <> req.Title.sha256.sha256 then
                    post.UpdateTitle req.Title |> ignore //TODO handle result
            |> ignore

            post.Body.fmap
            <| fun old ->
                if old.sha256.sha256 <> req.Body.sha256.sha256 then
                    post.UpdateBody req.Body |> ignore //TODO handle result
            |> ignore

            Rsp(Ok = true, Msg = "") |> Ok
        else
            $"Operation failed: Permission denied (post id:{post.Id})"
            |> effect logger.LogError
            |> Exception
            |> Err
    | Err e -> Err e
