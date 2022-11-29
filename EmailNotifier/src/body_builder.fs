module helper

open System
open pilipala.access.user
open pilipala.container.post
open pilipala.container.comment

let body_builder
    (mappedPostProvider: IMappedPostProvider)
    (mappedCommentProvider: IMappedCommentProvider)
    (mappedUserProvider: IMappedUserProvider)
    (comment: IMappedComment)
    =

    let when_bind_post post_id =

        let binding_post =
            mappedPostProvider.fetch post_id

        let comment_user =
            mappedUserProvider.fetch comment.UserId

        $"""来自 {comment_user.Name}({comment.UserId}) 的评论({comment.Id})：
{comment.Body}

发表在 {if binding_post.Title = "" then
         binding_post.Body
     else
         binding_post.Title}({post_id})
于 {DateTime.Now}
"""

    let when_bind_comment comment_id =

        let binding_comment =
            mappedCommentProvider.fetch comment_id

        let binding_comment_user =
            mappedUserProvider.fetch binding_comment.UserId

        let comment_user =
            mappedUserProvider.fetch comment.UserId

        $"""来自 {comment_user.Name}({comment.UserId}) 的回复({comment.Id})：
{comment.Body}

发表在:
{binding_comment.Body}
({binding_comment_user.Name}({binding_comment.UserId}) 于 {binding_comment.CreateTime})
于 {DateTime.Now}
"""

    match comment.Binding with
    | BindPost post_id -> when_bind_post post_id
    | BindComment comment_id -> when_bind_comment comment_id
