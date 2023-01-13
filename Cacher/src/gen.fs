module internal Cacher.gen

open System
open System.Runtime.Caching
open fsharper.op
open fsharper.typ
open fsharper.alias
open pilipala.pipeline

let gen
    (cache: MemoryCache)
    (prefix: string)
    (renderBuilderItem: BuilderItem<_, _>)
    (modifyBuilderItem: BuilderItem<_>)
    =

    //从数据库查询失败后清除缓存
    let beforeRenderFail id =
        $"{prefix}{id}" |> cache.Remove |> always id

    let beforeStorageFail (id, v) =
        $"{prefix}{id}" |> cache.Remove |> always (id, v)

    renderBuilderItem.beforeFail.Add beforeRenderFail
    modifyBuilderItem.beforeFail.Add beforeStorageFail

    let gen_policy () =
        CacheItemPolicy()
        |> effect (fun x -> x.AbsoluteExpiration <- DateTimeOffset.Now.AddSeconds 30)

    //从数据库查询成功后添加缓存
    let after (id: i64, v) =
        (CacheItem($"{prefix}{id}", v), gen_policy ())
        |> cache.Set

        id, v

    renderBuilderItem.collection.Add(After after)
    modifyBuilderItem.collection.Add(After after)

    //渲染缓存层
    let find (id: i64) =
        cache.[$"{prefix}{id}"]
        |> Option'.fromNullable
        |> fmap (fun v ->
            cache.Set(CacheItem($"{prefix}{id}", v), gen_policy ())
            id, v.cast ())

    renderBuilderItem.collection.Add
    <| Replace(fun old id -> unwrapOrEval (find id) (fun _ -> old id))
