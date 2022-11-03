namespace pilipala.plugin

open System
open System.Reflection
open fsharper.op
open fsharper.alias
open fsharper.typ
open pilipala.pipeline
open pilipala.pipeline.comment
open pilipala.util.text
open pilipala.pipeline.post

[<HookOn(AppLifeCycle.BeforeBuild)>]
type Markdown(postRenderBuilder: IPostRenderPipelineBuilder, commentRenderBuilder: ICommentRenderPipelineBuilder) =

    do
        let f (id, body) = id, { markdown = body }.intoHtml().html
        postRenderBuilder.Body.collection.Add(After f)

    do
        let f (id, body) = id, { markdown = body }.intoHtml().html
        commentRenderBuilder.Body.collection.Add(After f)
