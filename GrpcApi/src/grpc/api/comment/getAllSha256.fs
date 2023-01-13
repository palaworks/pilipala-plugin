module grpc.api.comment.getAllSha256

open System
open System.Text
open grpc_code_gen.comment.get_all_sha256
open Grpc.Core
open fsharper.op
open fsharper.typ
open pilipala.access.user
open pilipala.util.text.time
open pilipala.util.hash.sha256

type Ctx = ServerCallContext

let handler (user: IUser) (req: Req) (ctx: Ctx) =
    let comments = user.GetReadableComment()

    let collection =
        comments.foldl
        <| fun acc comment ->
            IdAndSha256()
                .effect (fun x ->
                    x.Id <- comment.Id

                    //TODO code formatter will make this broken
                    x.Sha256 <-
                        comment.Body.unwrapOrEval(fun _ -> $"Unknown error: can not read comment({comment.Id})")
                            .sha256
                            .sha256)
            :: acc
        <| []

    Rsp(Ok = true, Msg = "")
        .effect (fun rsp -> rsp.Collection.AddRange collection)
    |> Ok
