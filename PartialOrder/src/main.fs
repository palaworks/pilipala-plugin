namespace pilipala.plugin

open pilipala.data.db
open pilipala.pipeline.comment
open pilipala.pipeline.post
open forPost
open forComment

[<HookOn(AppLifeCycle.BeforeBuild)>]
type PartialOrder
    (
        postRenderBuilder: IPostRenderPipelineBuilder,
        commentRenderBuilder: ICommentRenderPipelineBuilder,
        db: IDbOperationBuilder
    ) =

    do forPost postRenderBuilder db
    do forComment commentRenderBuilder db
