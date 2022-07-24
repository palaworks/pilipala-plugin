namespace pilipala.plugin

open fsharper.op.Alias
open fsharper.typ.Pipe
open pilipala.pipeline
open pilipala.pipeline.post
open pilipala.util.text

type Markdown(render: IPostRenderPipelineBuilder) =

    do
        let after (id: u64, v: string) = (id, { markdown = v }.intoHtml.html)
        render.Body.collection.Add(After(GenericPipe after))
