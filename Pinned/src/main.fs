namespace pilipala.plugin

open System
open System.Collections.Generic
open fsharper.op
open fsharper.typ
open fsharper.alias
open pilipala.plugin
open pilipala.pipeline
open pilipala.util.text
open pilipala.pipeline.post

type Config = { pinned: HashSet<i64> }

type Pinned(postRenderBuilder: IPostRenderPipelineBuilder, cfg: IPluginCfgProvider) =

    let map =
        { json = cfg.config }
            .deserializeTo<Config>()
            .unwrapOr (fun _ -> { pinned = HashSet<i64>() })

    do
        let f id : i64 * obj = id, map.pinned.Contains(id)

        postRenderBuilder.["IsPinned"].collection.Add
        <| Replace(always f)
