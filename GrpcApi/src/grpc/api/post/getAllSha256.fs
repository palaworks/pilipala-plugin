module grpc.api.post.getAllSha256

open System.Text
open grpc_code_gen.post.get_all_sha256
open Grpc.Core
open fsharper.op
open fsharper.typ
open pilipala.access.user
open pilipala.util.hash.sha256

type Ctx = ServerCallContext

let handler (user: IUser) (req: Req) (ctx: Ctx) =
    let posts = user.GetReadablePost()

    let collection =
        posts.foldl
        <| fun acc comment ->
            IdAndSha256()
                .effect (fun x ->
                    x.Id <- comment.Id

                    x.Sha256 <-
                        StringBuilder().Append(
                            comment.Title.unwrapOrEval (fun _ -> $"Unknown error: can not read post({comment.Id})")
                        )
                            .Append(
                            comment.Body.unwrapOrEval (fun _ -> $"Unknown error: can not read post({comment.Id})")
                        )
                            .ToString()
                            .sha256
                            .sha256)
            :: acc
        <| []

    Rsp(Ok = true, Msg = "")
        .effect (fun rsp -> rsp.Collection.AddRange collection)
    |> Ok
