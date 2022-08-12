module internal Cacher.comment_gen

open System.Collections.Generic
open System.Collections.Concurrent
open fsharper.op
open fsharper.typ
open fsharper.alias
open pilipala.pipeline

let comment_gen (renderBuilder: BuilderItem<_, _>) (modifyBuilder: BuilderItem<_>) =
    
    let map = ConcurrentDictionary<i64, i64 * _>()

    //从数据库查询失败后清除缓存
    let beforeRenderFail id = map.Remove id |> always id
    let beforeStorageFail (id, v) = map.Remove id |> always (id, v)
    renderBuilder.beforeFail.Add beforeRenderFail
    modifyBuilder.beforeFail.Add beforeStorageFail

    //从数据库查询成功后添加缓存
    let after (id: i64, v) =
        map.AddOrUpdate(id, (fun _ -> (id, v)), (fun _ -> always (id, v)))

    renderBuilder.collection.Add <| After after
    modifyBuilder.collection.Add <| After after

    //渲染缓存层
    let cache id =
        map.TryGetValue id |> Option'.fromOkComma

    renderBuilder.collection.Add
    <| Replace(fun old id -> unwrapOr (cache id) (fun _ -> old id))
