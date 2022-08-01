namespace pilipala.plugin

open System
open System.Threading.Tasks
open fsharper.op
open fsharper.typ
open fsharper.op.Alias
open pilipala.pipeline
open pilipala.util.text
open pilipala.pipeline.post

type ViewCount(renderBuilder: IPostRenderPipelineBuilder, cfg: IPluginCfgProvider) =

    let map =
        { json = cfg.config }
            .deserializeTo<Dict<u64, u32>> ()

    //TODO 应实现每N次同步、关闭前同步和每时间段同步
    let save () =
        Task.Run(fun _ -> cfg.config <- map.serializeToJson().json)

    do

        let f (post_id, _) =
            if map.ContainsKey post_id then
                map.[post_id] <- map.[post_id] + 1u
            else
                map.Add(post_id, 1u)
            |> effect save

        renderBuilder.Body.collection.Add
        <| After(effect f)

    do
        let data post_id =
            map
                .TryGetValue(post_id)
                .intoOption'()
                .orPure(fun _ ->
                    map.Add(post_id, 0u)
                    0u)
                .fmap (fun n -> post_id, obj n)

        renderBuilder.["ViewCount"].collection.Add
        <| Replace(fun fail id -> unwrapOr (data id) (fun _ -> fail id))
