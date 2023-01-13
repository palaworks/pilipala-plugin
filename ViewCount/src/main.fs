namespace pilipala.plugin

open System
open System.Threading.Tasks
open fsharper.op
open fsharper.typ
open fsharper.alias
open pilipala.pipeline
open pilipala.util.text
open pilipala.pipeline.post

[<HookOn(AppLifeCycle.BeforeBuild)>]
type ViewCount(postRenderBuilder: IPostRenderPipelineBuilder, cfg: IPluginCfgProvider) =

    let map =
        { json = cfg.config }
            .deserializeTo<Dict<i64, u32>>()
            .unwrapOrEval (fun _ -> Dict<i64, u32>())


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

        postRenderBuilder.Body.collection.Add
        <| After(effect f)

    do
        let f post_id =
            map
                .TryGetValue(post_id)
                .intoOption'()
                .orPure(always 0u) //请求访问计数的时候不需要进行持久化
                .fmap(fun n -> post_id, n :> obj)
                .unwrap ()

        postRenderBuilder.["ViewCount"].collection.Add
        <| Replace(always f)
