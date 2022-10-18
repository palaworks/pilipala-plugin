module internal forComment

open fsharper.op
open fsharper.typ
open fsharper.alias
open pilipala.pipeline
open pilipala.pipeline.comment

let forComment
    (renderBuilder: ICommentRenderPipelineBuilder)
    (renderPipeline: ICommentRenderPipeline)
    (map: Dict<i64, string>)
    =

    let getUserId id = renderPipeline.UserId id |> snd

    let f id =
        id, map.TryGetValue(getUserId id).intoOption'().obj ()

    renderBuilder.["UserAvatarUrl"].collection.Add
    <| Replace(fun _ -> f)
