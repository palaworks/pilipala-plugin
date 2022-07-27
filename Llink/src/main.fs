namespace pilipala.plugin

open System
open System.IO
open System.Reflection
open System.Collections.Generic
open Newtonsoft.Json.Linq
open fsharper.op.Alias
open fsharper.typ.Pipe
open fsharper.op.Coerce
open fsharper.op.Foldable
open pilipala.pipeline
open pilipala.util.text
open pilipala.pipeline.post

type Llink(render: IPostRenderPipelineBuilder, cfg: IPluginCfgProvider) =

    do
        let map = Dictionary<string, string>()

        for el in JObject.Parse cfg.config do
            map.Add($"<{{{el.Key}}}>", coerce el.Value)

        let after (id: u64, v: string) =
            id, map.foldl (fun (acc: string) x -> acc.Replace(x.Key, x.Value)) v

        render.Body.collection.Add(After(GenericPipe after))
