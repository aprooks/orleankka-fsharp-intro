module Grains

open Orleankka.FSharp
open Orleankka
open FSharp.Control.Tasks


open Byke

type Byke() = 
     inherit ActorGrain()
     interface IByke 

     override x.Receive(message:obj) = task {
        match message with 
        | :? Message as msg ->
          match msg with
          |Reserve byUserId -> 
            printfn "Reserved by %s" byUserId
            return none()
        | other -> 
            printfn "Recieved unhandled type: %s" (other.GetType().Name)
            return none()
     }