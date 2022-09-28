namespace pilipala.plugin

open System
open System.IO
open System.Collections.Generic
open Newtonsoft.Json.Linq
open fsharper.op
open fsharper.op.Pattern
open fsharper.op.Foldable
open fsharper.typ
open fsharper.alias
open fsharper.op.Coerce
open fsharper.op.Foldable
open pilipala.pipeline
open pilipala.util.text
open pilipala.pipeline.post

type Llink(render: IPostRenderPipelineBuilder, cfg: IPluginCfgProvider) =

    do
        let map =
            { json = cfg.config }
                .deserializeTo<Dict<string, string>>()
                .unwrapOr (fun _ -> Dict<string, string>())

        let f (id: i64, body: string) =
            id, map.foldl (fun (acc: string) (KV (k, v)) -> acc.Replace(k, v)) body

        render.Body.collection.Add(After f)
