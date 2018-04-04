open System
open System.Net
open System.Reflection

open FSharp.Control.Tasks
open Orleans
open Orleans.Hosting
open Orleans.Configuration
open Orleans.Runtime
open Orleankka
open Orleankka.Cluster
open Orleankka.Client
open Orleankka.FSharp

open Byke
open Grains


[<EntryPoint>]
let main argv =    
   
    printfn "Running demo. Booting cluster might take some time ...\n"

    let sb = new SiloHostBuilder()
    sb.AddAssembly(typeof<Byke.Message>.Assembly)
    sb.AddAssembly(Assembly.GetExecutingAssembly())
    sb.UseOrleankka() |> ignore

    use host = sb.Start().Result
    use client = host.Connect().Result

    let system = client.ActorSystem()
    
    printfn "Cluster booted!"

    let initScript = task {
      
      let byke = ActorSystem.typedActorOf<IByke, Message>(system,"first")
      do! byke <! Reserve "Alex"

      try 
        do! byke <! Reserve "Andrea"
        printfn "Ohoh, should have failed"
      with
      | :? BykeIsReserved ->
        printfn "Second reservation failed as expected"
      | _ -> 
        printfn "some unexpected shit happened"
      
      do! byke <! CancelReservation "Alex"
      do! byke <! StartTrip "Andrea"
      do! byke <! EndTrip "Andres"
    }

    initScript.Wait()

    Console.ReadKey() |> ignore 
    0