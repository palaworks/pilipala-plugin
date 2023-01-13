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
type UserSiteUrl
    (
        postRenderBuilder: IPostRenderPipelineBuilder,
        postRenderPipeline: IPostRenderPipeline,
        commentRenderBuilder: ICommentRenderPipelineBuilder,
        commentRenderPipeline: ICommentRenderPipeline,
        cfg: IPluginCfgProvider
    ) =

    //TODO JSON5 support
    let map =
        { json = cfg.config }
            .deserializeTo<Dict<i64, string>>()
            .unwrapOrEval (fun _ -> Dict<i64, string>())

    do
        forPost postRenderBuilder postRenderPipeline map
        forComment commentRenderBuilder commentRenderPipeline map
