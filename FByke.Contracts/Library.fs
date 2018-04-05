module Contracts

type UserId = UserId of string

module Byke = 

  open Orleankka.FSharp
  open Orleankka

  type Message = 
  | Reserve of UserId
  | CancelReservation of UserId
  | StartTrip of UserId
  | EndTrip of UserId
  | IsAvailable

  exception BykeIsReserved

  type IByke = 
    inherit IActorGrain<Message>


module Bugger = 

  /// Need to explicitly reference smth from Orleans
  /// as F# linker is being too agressive and removes reference 
  /// to Orleans.Abstractions which is required for codegen to work
  type IFSharpGotcha =
    inherit Orleans.IGrainWithGuidKey
    

module UserWallet = 
  
  open Orleankka.FSharp

  type Message = 
  | ``Reserve minimum amount for trip``
  | Topup of decimal
  | Charge of decimal

  exception AmountNotEnough

  type IUserWallet = 
    inherit IActorGrain<Message>
