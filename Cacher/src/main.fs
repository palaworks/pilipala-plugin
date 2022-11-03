namespace pilipala.plugin

open System.Reflection
open fsharper.op
open pilipala.pipeline.post
open pilipala.util.text
open pilipala.pipeline.comment
open Cacher.gen
open System.Runtime.Caching
open Microsoft.Extensions.Caching.Memory

type Cfg =
    { enable_post: bool
      enable_comment: bool }

[<HookOn(AppLifeCycle.BeforeBuild)>]
type Cacher
    (
        cfg: IPluginCfgProvider,
        postRenderBuilder: IPostRenderPipelineBuilder,
        postModifyBuilder: IPostModifyPipelineBuilder,
        commentRenderBuilder: ICommentRenderPipelineBuilder,
        commentModifyBuilder: ICommentModifyPipelineBuilder
    ) =

    let config =
        { json = cfg.config }
            .deserializeTo<Cfg>()
            .unwrapOr (fun _ ->
                { enable_post = false
                  enable_comment = false })

    let cache = MemoryCache.Default

    do
        if config.enable_post then
            gen cache "post_title" postRenderBuilder.Title postModifyBuilder.Title
            gen cache "post_body" postRenderBuilder.Body postModifyBuilder.Body
            gen cache "post_create_time" postRenderBuilder.CreateTime postModifyBuilder.CreateTime
            gen cache "post_access_time" postRenderBuilder.AccessTime postModifyBuilder.AccessTime
            gen cache "post_modify_time" postRenderBuilder.ModifyTime postModifyBuilder.ModifyTime
            gen cache "post_permission" postRenderBuilder.Permission postModifyBuilder.Permission
            gen cache "post_user_id" postRenderBuilder.UserId postModifyBuilder.UserId

        if config.enable_comment then
            gen cache "comment_body" commentRenderBuilder.Body commentModifyBuilder.Body
            gen cache "comment_create_time" commentRenderBuilder.CreateTime commentModifyBuilder.CreateTime
            gen cache "comment_binding" commentRenderBuilder.Binding commentModifyBuilder.Binding
