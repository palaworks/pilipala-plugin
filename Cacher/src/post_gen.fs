module internal Cacher.post_gen

open System.Collections.Generic
open System.Collections.Concurrent
open fsharper.op
open fsharper.typ
open fsharper.alias
open fsharper.typ.Pipe
open pilipala.pipeline

let post_gen (renderBuilder: BuilderItem<_, _>) (modifyBuilder: BuilderItem<_>) =
    let map =
        ConcurrentDictionary<u64, u64 * _>()

    //从数据库查询失败后清除缓存
    let beforeRenderFail id = map.Remove id |> always id
    let beforeStorageFail (id, v) = map.Remove id |> always (id, v)
    renderBuilder.beforeFail.Add beforeRenderFail
    modifyBuilder.beforeFail.Add beforeStorageFail

    //从数据库查询成功后添加缓存
    let after (id: u64, v) =
        map.AddOrUpdate(id, (fun _ -> (id, v)), (fun _ -> always (id, v)))

    renderBuilder.collection.Add(After after)
    modifyBuilder.collection.Add(After after)

    //渲染缓存层
    let cache id =
        map.TryGetValue id |> Option'.fromOkComma

    renderBuilder.collection.Add
    <| Replace(fun old id -> unwrapOr (cache id) (fun _ -> old id))
