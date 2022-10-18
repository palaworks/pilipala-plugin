module internal forPost

open fsharper.op
open fsharper.typ
open fsharper.alias
open pilipala.pipeline
open pilipala.pipeline.post

let forPost (renderBuilder: IPostRenderPipelineBuilder) (renderPipeline: IPostRenderPipeline) (map: Dict<i64, string>) =

    let getUserId id = renderPipeline.UserId id |> snd

    let f id =
        id, map.TryGetValue(getUserId id).intoOption'().obj ()

    renderBuilder.["UserSiteUrl"].collection.Add
    <| Replace(fun _ -> f)
