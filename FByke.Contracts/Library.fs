module Byke

open Orleankka
open Orleankka.FSharp

type Message = 
| Reserve of string


exception BykeIsReserved

type IByke = 
  inherit IActorGrain<Message>


module Bugger = 

  /// Need to explicitly reference smth from Orleans
  /// as F# linker is being too agressive and removes reference 
  /// to Orleans.Abstractions which is required for codegen to work
  type IFSharpGotcha =
    inherit Orleans.IGrainWithGuidKey