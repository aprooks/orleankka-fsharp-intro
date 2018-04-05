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

open Contracts
open Byke


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
      
      let byke = ActorSystem.actorOf<IByke>(system,"first")
      do! byke <! Reserve (UserId "Alex")

      let! available = byke <? IsAvailable
      printfn "Bike is available: %b" available

      try 
        do! byke <! Reserve (UserId "Andrea")
        printfn "Ohoh, should have failed"
      with
      | BykeIsReserved ->
        printfn "Second reservation failed as expected"
      | _ -> 
        printfn "some unexpected shit happened"
      
      do! byke <! CancelReservation (UserId "Alex")
      do! byke <! StartTrip (UserId "Andrea")
      do! byke <! EndTrip (UserId "Andrea")
    }

    initScript.Wait()

    Console.ReadKey() |> ignore 
    0