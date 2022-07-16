namespace pilipala.plugin

open System
open System.IO
open System.Reflection
open System.Collections.Generic
open fsharper.op.Alias
open fsharper.typ.Pipe
open fsharper.op.Coerce
open fsharper.op.Foldable
open pilipala.util.io
open pilipala.pipeline
open pilipala.util.json
open pilipala.pipeline.post

type Llink(render: IPostRenderPipelineBuilder) =

    let path =
        let asmDir =
            Assembly
                .GetAssembly(typeof<Llink>)
                .Location.Replace($"{typeof<Llink>.Name}.dll", "")

        $"{asmDir}/config.json"

    let fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite)

    do
        if fs.Length <> 0 then
            let map = Dictionary<string, string>()

            for el in readFile(path).jsonParsed do
                map.Add($"<{{{el.Key}}}>", coerce el.Value)

            let after (id: u64, v: string) =
                id, map.foldl (fun (acc: string) x -> acc.Replace(x.Key, x.Value)) v

            render.Body.collection.Add(After(GenericPipe after))
        else
            let sw = new StreamWriter(fs)
            sw.Write("{}")
            sw.Close()
