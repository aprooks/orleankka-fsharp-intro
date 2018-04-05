module Grains

open Orleankka.FSharp
open Orleankka
open FSharp.Control.Tasks

open System

open Contracts
open Byke

type Byke() = 
     inherit ActorGrain()
     interface IByke 

     override x.Receive(message:obj) = task {
        match message with 
        | :? Message as msg ->
          match msg with
          | Reserve byUserId -> 
            
            do! x.Reminders.Register("expired",
                                      TimeSpan.FromMinutes(10.),
                                      TimeSpan.FromMinutes(10.))
            
            return none()

          | CancelReservation userId ->
            printfn "Cancelled"
            return none()

          | StartTrip userId ->
            return none()

          | EndTrip userId ->
            return none()

          | IsAvailable ->
            return some(true)

        | :? Reminder as r ->
          match r.Name with 
          | "expired" -> 
            printfn "ticked"
            do! x.Reminders.Unregister("expired")
          | _ ->
            failwith "don't do this!"
          return none()

        | other -> 
            printfn "Recieved unhandled type: %s" (other.GetType().Name)
            return none()
     }