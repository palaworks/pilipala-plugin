namespace pilipala.plugin

open fsharper.op.Alias
open fsharper.typ.Pipe
open pilipala.pipeline
open pilipala.pipeline.post
open pilipala.util.markdown

type Markdown(render: IPostRenderPipelineBuilder) =

    do
        let after (id: u64, v: string) = (id, v.markdownInHtml)
        render.Body.collection.Add(After(GenericPipe after))
