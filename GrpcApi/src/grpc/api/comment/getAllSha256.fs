module grpc.api.comment.getAllSha256

open fsharper.op
open fsharper.typ
open plugin.grpc.alias
open pilipala.access.user
open pilipala.util.hash.sha256
open Microsoft.Extensions.Logging
open grpc_code_gen.comment.get_all_sha256

let handler (user: IUser) (req: Req) (ctx: Ctx) (logger: ILogger) =
    let comments = user.GetReadableComment()

    let collection =
        comments.foldl
        <| fun acc comment ->
            IdAndSha256()
                .effect (fun x ->
                    x.Id <- comment.Id

                    //TODO code formatter will make this broken
                    x.Sha256 <-
                        comment.Body.unwrapOrEval(fun _ ->
                            $"Unknown error: can not read {nameof comment.Body} (comment id:{comment.Id})"
                            |> effect logger.LogError)
                            .sha256.sha256)
            :: acc
        <| []

    Rsp(Ok = true, Msg = "").effect (fun rsp -> rsp.Collection.AddRange collection)
    |> Ok
