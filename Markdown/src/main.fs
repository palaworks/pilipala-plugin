namespace pilipala.plugin

open System
open fsharper.op
open fsharper.alias
open pilipala.pipeline
open pilipala.util.text
open pilipala.pipeline.post

type Markdown(render: IPostRenderPipelineBuilder) =

    do
        let f (id: i64, v: string) = id, { markdown = v }.intoHtml().html
        render.Body.collection.Add <| After f
