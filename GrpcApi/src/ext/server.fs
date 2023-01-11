namespace ext

open Grpc.Core
open fsharper.op
open fsharper.typ

[<AutoOpen>]
module ext_Server =
    type Server with

        member self.addPort port = self.Ports.Add port |> always self

        member self.addService service =
            self.Services.Add service
            self
