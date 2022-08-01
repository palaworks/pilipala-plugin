namespace pilipala.plugin

open fsharper.op
open fsharper.typ
open fsharper.op.Alias
open pilipala.pipeline
open pilipala.util.text
open pilipala.pipeline.post

type PostCover
    (
        renderBuilder: IPostRenderPipelineBuilder,
        modifyBuilder: IPostModifyPipelineBuilder,
        cfg: IPluginCfgProvider
    ) =

    let map =
        { json = cfg.config }
            .deserializeTo<Dict<u64, string>> ()

    do
        let data post_id =
            map
                .TryGetValue(post_id)
                .intoOption'()
                .orPure(fun _ -> "")
                .fmap (fun x -> post_id, obj x)

        renderBuilder.["Cover"].collection.Add
        <| Replace(fun fail id -> unwrapOr (data id) (fun _ -> fail id))

    do
        let data (post_id, v) =
            if map.ContainsKey post_id then
                map.[post_id] <- v
            else
                map.Add(post_id, v)

            cfg.config <- map.serializeToJson().json
            Some(post_id, obj v)

        modifyBuilder.["Cover"].collection.Add
        <| Replace(fun fail x -> unwrapOr (x |> coerce |> data) (fun _ -> fail x))
