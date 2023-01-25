module grpc.api.comment.getAll

open System
open Grpc.Core
open fsharper.op
open fsharper.typ
open pilipala.access.user
open pilipala.util.text.time
open Microsoft.Extensions.Logging
open grpc_code_gen.comment.get_all

type Ctx = ServerCallContext

let handler (user: IUser) (req: Req) (ctx: Ctx) (logger: ILogger) =
    let comments = user.GetReadableComment()

    let collection =
        comments.foldl
        <| fun acc comment ->
            grpc_code_gen.comment.get_one.T(
                Id = comment.Id,
                Body =
                    comment.Body.unwrapOrEval (fun _ ->
                        $"Unknown error: can not read {nameof comment.Body}(comment id:{comment.Id})"
                        |> effect logger.LogError),
                CreateTime =
                    comment
                        .CreateTime
                        .unwrapOrEval(fun _ ->
                            DateTime.UnixEpoch.effect
                            <| fun _ ->
                                logger.LogError
                                    $"Unknown error: can not read {nameof comment.CreateTime}(comment id:{comment.Id})")
                        .ToIso8601(),
                ModifyTime =
                    comment
                        .ModifyTime
                        .unwrapOrEval(fun _ ->
                            DateTime.UnixEpoch.effect
                            <| fun _ ->
                                logger.LogError
                                    $"Unknown error: can not read {nameof comment.ModifyTime}(comment id:{comment.Id})")
                        .ToIso8601()
            )
            :: acc
        <| []

    Rsp(Ok = true, Msg = "").effect <| fun rsp -> rsp.Collection.AddRange collection
    |> Ok
