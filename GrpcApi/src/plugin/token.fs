namespace plugin.token

open System
open pilipala
open fsharper.op
open fsharper.typ
open pilipala.util.id
open pilipala.access.user
open System.Runtime.Caching

type TokenHandler(app: IApp) =

    let cache = MemoryCache.Default

    let uuid_generator = uuid.Generator(uuid.Format.N)

    let gen_policy () =
        CacheItemPolicy()
        //TODO configurable token expire time
        |> effect (fun x -> x.AbsoluteExpiration <- DateTimeOffset.Now.AddMinutes 10)

    member self.NewToken uid pwd =
        let token = uuid_generator.next ()

        match app.userLoginById uid pwd with
        | Ok user ->
            //TODO 不清除之前的缓存带来了允许多次登录而不冲突的潜在特性，该特性可以应用于多设备登录
            cache.Set(CacheItem(token, user), gen_policy ())
            Ok token
        | Err e -> Err e

    member self.GetUser token : Option'<IUser> =
        cache.Get token
        |> Option'.fromNullable
        |> fmap (fun user ->
            //extend expire time
            cache.Set(CacheItem(token, user), gen_policy ())
            user.cast ())
