module internal forPost

open fsharper.typ
open pilipala.data.db
open pilipala.pipeline
open pilipala.pipeline.post

let forPost (renderBuilder: IPostRenderPipelineBuilder) (db: IDbOperationBuilder) =

    do
        let f post_id =
            post_id,
            db {
                getFstVal//TODO sql normalization
                    $"SELECT post_id FROM {db.tables.post} \
                      WHERE post_id < :post_id \
                      ORDER BY post_id DESC \
                      LIMIT 1"
                    [ ("post_id", post_id) ]

                execute
            }
            :> obj

        renderBuilder.["PredId"].collection.Add
        <| Replace(fun _ -> f)

    do
        let f post_id =
            post_id,
            db {
                getFstVal
                    $"SELECT post_id FROM {db.tables.post} \
                      WHERE post_id > :post_id \
                      ORDER BY post_id \
                      LIMIT 1"
                    [ ("post_id", post_id) ]

                execute
            }
            :> obj

        renderBuilder.["SuccId"].collection.Add
        <| Replace(fun _ -> f)
