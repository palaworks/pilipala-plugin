module grpc.api.post.getAll

open System
open Google.Protobuf.Collections
open grpc_code_gen.post.get_all
open Grpc.Core
open fsharper.op
open fsharper.typ
open pilipala.access.user
open pilipala.util.text.time

type Ctx = ServerCallContext

let handler (user: IUser) (req: Req) (ctx: Ctx) =
    let posts = user.GetReadablePost()

    let collection =
        posts.foldl
        <| fun acc post ->
            grpc_code_gen.post.get_one.T(
                Id = post.Id,
                Title = post.Title.unwrapOrEval (fun _ -> $"Unknown error: can not read post({post.Id})"),
                Body = post.Body.unwrapOrEval (fun _ -> $"Unknown error: can not read post({post.Id})"),
                CreateTime =
                    post
                        .CreateTime
                        .unwrapOrEval(fun _ -> DateTime.UnixEpoch)
                        .ToIso8601(),
                ModifyTime =
                    post
                        .ModifyTime
                        .unwrapOrEval(fun _ -> DateTime.UnixEpoch)
                        .ToIso8601()
            )
            :: acc
        <| []

    Rsp(Ok = true, Msg = "").effect
    <| fun rsp -> rsp.Collection.AddRange collection
    |> Ok
