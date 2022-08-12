namespace pilipala.plugin

open pilipala.pipeline.post
open pilipala.util.text
open pilipala.pipeline.comment
open Cacher.post_gen
open Cacher.comment_gen

type Config =
    { enable_post: bool
      enable_comment: bool }

type Cacher
    (
        cfg: IPluginCfgProvider,
        postRenderBuilder: IPostRenderPipelineBuilder,
        postModifyBuilder: IPostModifyPipelineBuilder,
        commentRenderBuilder: ICommentRenderPipelineBuilder,
        commentModifyBuilder: ICommentModifyPipelineBuilder
    ) =
    //TODO 引入缓存容量

    let config =
        { json = cfg.config }.deserializeTo<Config> ()

    do
        if config.enable_post then
            post_gen postRenderBuilder.Title postModifyBuilder.Title
            post_gen postRenderBuilder.Body postModifyBuilder.Body
            post_gen postRenderBuilder.CreateTime postModifyBuilder.CreateTime
            post_gen postRenderBuilder.AccessTime postModifyBuilder.AccessTime
            post_gen postRenderBuilder.ModifyTime postModifyBuilder.ModifyTime

        if config.enable_comment then
            comment_gen commentRenderBuilder.Body commentModifyBuilder.Body
            comment_gen commentRenderBuilder.CreateTime commentModifyBuilder.CreateTime
