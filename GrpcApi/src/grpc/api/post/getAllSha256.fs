module grpc.api.post.getAllSha256

open System.Text
open fsharper.op
open fsharper.typ
open plugin.grpc.alias
open pilipala.access.user
open pilipala.util.hash.sha256
open grpc_code_gen.post.message
open Microsoft.Extensions.Logging
open grpc_code_gen.post.get_all_sha256

let handler (user: IUser) (req: Req) (ctx: Ctx) (logger: ILogger) =
    let posts = user.GetReadablePost()

    let dataList =
        posts.foldl
        <| fun acc post ->
            IdWithSha256()
                .effect (fun x ->
                    x.Id <- post.Id

                    x.Sha256 <-
                        StringBuilder().Append(
                            fun _ ->
                                $"Unknown error: can not read {nameof post.Title} (post id:{post.Id})"
                                |> effect logger.LogError
                            |> post.Title.unwrapOrEval
                        )
                            .Append(
                            fun _ ->
                                $"Unknown error: can not read {nameof post.Body} (post id:{post.Id})"
                                |> effect logger.LogError
                            |> post.Body.unwrapOrEval
                        )
                            .ToString()
                            .sha256
                            .sha256)
            :: acc
        <| []

    Rsp(Ok = true, Msg = "").effect (fun rsp -> rsp.DataList.AddRange dataList)
    |> Ok
