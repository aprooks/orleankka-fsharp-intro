
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

[<EntryPoint>]
let main argv =    
   
    printfn "Running demo. Booting cluster might take some time ...\n"

    let sb = new SiloHostBuilder()
    sb.AddAssembly(Assembly.GetExecutingAssembly())
    sb.UseOrleankka() |> ignore

    use host = sb.Start().Result
    use client = host.Connect().Result

    let system = client.ActorSystem()
    
    printfn "Cluster booted!"

    Console.ReadKey() |> ignore 
    0