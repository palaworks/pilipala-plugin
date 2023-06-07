module plugin.hosting

open System.Net
open System.Threading.Tasks
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open fsharper.typ
open fsharper.alias

type Worker(startWsLocalServer: unit -> IPAddress * u16, startWsPublicServer: unit -> IPAddress * u16, logger: ILogger)
    =
    inherit BackgroundService()

    override self.ExecuteAsync _ =
        let wsLocalServer =
            fun _ ->
                let localAddr, localPort = startWsLocalServer ()

                logger.LogInformation $"Local WebSocket now listening on {localAddr}:{localPort}"
            |> Task.RunAsTask

        let wsPublicServer =
            fun _ ->
                let publicAddr, publicPort = startWsPublicServer ()

                logger.LogInformation $"Public WebSocket now listening on {publicAddr}:{publicPort}"
            |> Task.RunAsTask

        Task.WhenAll([| wsLocalServer; wsPublicServer |])


let runHost startWsLocalServer startWsPublicServer (logger: ILogger) =
    Host
        .CreateDefaultBuilder()
        .ConfigureLogging(fun builder -> builder.ClearProviders() |> ignore)
        .ConfigureServices(fun ctx services ->
            fun _ -> new Worker(startWsLocalServer, startWsPublicServer, logger)
            |> services.AddHostedService
            |> ignore)
        .Build()
        .Run()
