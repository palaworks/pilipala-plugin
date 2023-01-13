namespace ws.api.post.get

open System
open ws
open fsharper.alias
open fsharper.op
open fsharper.typ
open fsharper.op.Foldable
open pilipala.container.post
open pilipala.access.user
open pilipala.container.comment
open pilipala.util.text
open ws.api.comment.create

type Rsp =
    { Id: string //raw i64
      Title: string
      Body: string
      CreateTime: string
      ModifyTime: string
      CoverUrl: string
      Summary: string
      IsGeneratedSummary: bool
      ViewCount: u32
      Comments: api.comment.create.Rsp array
      CanComment: bool
      IsArchived: bool
      IsScheduled: bool
      Topics: string array
      (*
      PrevId: i64
      NextId: i64*)
      Mark: string }

module helper =

    let rec get_next_post (user: IUser) current_id =
        user.GetPost(current_id).bind
        <| fun post ->
            post.["SuccId"].bind
            <| fun opt_opt_id ->
                (opt_opt_id.fmap
                 <| fun x ->
                     x.cast<Option'<obj>>().fmap
                     <| fun x -> x.cast<i64> ())
                    .bind id
                |> fun optId ->
                    match optId with
                    | None -> Err "No post available"
                    | Some id ->
                        match user.GetPost id with
                        | Ok post when post.CanRead -> Ok post
                        //TODO? 通常是因为权限不够，转而获取下一个
                        //TODO 此实现存在问题，因为自定义属性也受权限系统控制
                        | _ -> get_next_post user id

    let rec get_prev_post (user: IUser) current_id =
        user.GetPost(current_id).bind
        <| fun post ->
            post.["PredId"].bind
            <| fun opt_opt_id ->
                (opt_opt_id.fmap
                 <| fun x ->
                     x.cast<Option'<obj>>().fmap
                     <| fun x -> x.cast<i64> ())
                    .bind id
                |> fun optId ->
                    match optId with
                    | None -> Err "No post available"
                    | Some id ->
                        match user.GetPost id with
                        | Ok post when post.CanRead -> Ok post
                        //TODO? 通常是因为权限不够，转而获取下一个
                        //TODO 此实现存在问题，因为自定义属性也受权限系统控制
                        | _ -> get_prev_post user id

    type Rsp with
        static member fromPost(post: IPost, user: IUser) =
            let rec getRecursiveComments (comment: IComment) =
                match comment.Comments.unwrap().toList () with
                | [] -> []
                | cs ->
                    cs.foldl
                    <| fun acc c ->
                        acc
                        @ (comment.Id, true, c) :: getRecursiveComments c
                    <| []

            let comments =
                (post.Comments.unwrap().foldl
                 <| fun acc c ->
                     acc
                     @ (post.Id, false, c) :: getRecursiveComments c
                 <| []
                 |> List.sortBy (fun (_, _, c) -> c.CreateTime |> unwrap))
                    .foldr
                <| fun (replyTo, isReply, comment: IComment) acc -> Rsp.fromComment (comment, replyTo, isReply) :: acc
                <| []

            let coverUrl =
                post.["CoverUrl"]
                    .unwrap() //Opt<Opt<obj>>
                    .fmap(cast) //Opt<Opt<str>>
                    .bind(id) //Opt<str>
                    .unwrapOrEval (fun _ -> null)

            let summary =
                post.["Summary"]
                    .unwrap() //Opt<obj>
                    .fmap(cast) //Opt<str>
                    .unwrapOrEval (fun _ -> null)

            let isGeneratedSummary =
                post.["IsGeneratedSummary"]
                    .unwrap() //Opt<obj>
                    .fmap(cast) //Opt<bool>
                    .unwrapOrEval (fun _ -> false)

            let viewCount =
                post.["ViewCount"]
                    .unwrap() //Opt<obj>
                    .fmap(cast) //Opt<u32>
                    .unwrapOrEval (fun _ -> 0u)

            let isArchived =
                post.["IsArchived"]
                    .unwrap() //Opt<obj>
                    .fmap(cast) //Opt<bool>
                    .unwrapOrEval (fun _ -> false)

            let isScheduled =
                post.["IsScheduled"]
                    .unwrap() //Opt<obj>
                    .fmap(cast) //Opt<bool>
                    .unwrapOrEval (fun _ -> false)

            let topics =
                post.["Topics"]
                    .unwrap() //Opt<obj>
                    .fmap(cast) //Opt<[str]>
                    .unwrapOrEval (fun _ -> [||])

            (*
            let prevId =
                (get_prev_post user post.Id)
                    .fmap(fun post -> post.Id)
                    .unwrapOr (fun _ -> -1)

            let nextId =
                (get_next_post user post.Id)
                    .fmap(fun post -> post.Id)
                    .unwrapOr (fun _ -> -1)*)

            let mark =
                post.["Mark"]
                    .unwrap() //Opt<obj>
                    .fmap(cast) //Opt<u32>
                    .unwrapOrEval (fun _ -> "")

            { Id = post.Id.ToString()
              Title = post.Title.unwrap ()
              Body = post.Body.unwrap ()
              CreateTime = post.CreateTime.unwrap().ToIso8601()
              ModifyTime = post.ModifyTime.unwrap().ToIso8601()
              CoverUrl = coverUrl
              Summary = summary
              IsGeneratedSummary = isGeneratedSummary
              ViewCount = viewCount
              Comments = comments.toArray ()
              CanComment = post.CanComment
              IsArchived = isArchived
              IsScheduled = isScheduled
              Topics = topics
              (*
              PrevId = prevId
              NextId = nextId*)
              Mark = mark }
