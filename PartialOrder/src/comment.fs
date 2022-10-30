module internal forComment

open fsharper.typ
open pilipala.data.db
open pilipala.pipeline
open pilipala.pipeline.comment

let forComment (renderBuilder: ICommentRenderPipelineBuilder) (db: IDbOperationBuilder) =

    do
        let f comment_id =
            comment_id,
            db {
                getFstVal
                    $"SELECT comment_id FROM {db.tables.comment} \
                      WHERE comment_id < :comment_id \
                      ORDER BY comment_id DESC \
                      LIMIT 1"
                    [ ("comment_id", comment_id) ]

                execute
            }
            :> obj

        renderBuilder.["PredId"].collection.Add
        <| Replace(fun _ -> f)

    do
        let f comment_id =
            comment_id,
            db {
                getFstVal
                    $"SELECT comment_id FROM {db.tables.comment} \
                      WHERE comment_id > :comment_id \
                      ORDER BY comment_id \
                      LIMIT 1"
                    [ ("comment_id", comment_id) ]

                execute
            }
            :> obj

        renderBuilder.["SuccId"].collection.Add
        <| Replace(fun _ -> f)
