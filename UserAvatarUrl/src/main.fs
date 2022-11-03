namespace pilipala.plugin

open fsharper.op
open fsharper.typ
open fsharper.alias
open pilipala.pipeline
open pilipala.pipeline.comment
open pilipala.util.text
open pilipala.pipeline.post
open forPost
open forComment

[<HookOn(AppLifeCycle.BeforeBuild)>]
type UserAvatarUrl
    (
        postRenderBuilder: IPostRenderPipelineBuilder,
        postRenderPipeline: IPostRenderPipeline,
        commentRenderBuilder: ICommentRenderPipelineBuilder,
        commentRenderPipeline: ICommentRenderPipeline,
        cfg: IPluginCfgProvider
    ) =

    let map =
        { json = cfg.config }
            .deserializeTo<Dict<i64, string>>()
            .unwrapOr (fun _ -> Dict<i64, string>())

    do
        forPost postRenderBuilder postRenderPipeline map
        forComment commentRenderBuilder commentRenderPipeline map
