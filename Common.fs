module FreshBooks2Harvest.Common

open FreshBooks2Harvest.Domain
open System
open System.IO

let first f (a, b) = (f a, b)
let second f (a, b) = (a, f b)

module Result =
    
    let inline (>>=) ma f = Result.bind f ma
    
    let inline (>=>) f g = f >> Result.bind g
    
    let inline (<^>) fab ma = Result.map fab ma
    
    let inline (<*>) mfab ma = Result.bind (fun fab -> fab <^> ma) mfab
    
    let notNull tag a =
        match box a with
        | null -> Error (tag, Pure "Required value")
        | _ -> Ok a
    
    let createDirectory source s =
        try
            Ok (Directory.CreateDirectory s)
        with
        | e -> IoError (source, Pure e.Message) |> Error
        
    let readFile source s =
        try
            Ok (File.ReadAllText s)
        with
        | e -> IoError (source, Pure e.Message) |> Error
        
    let tag t = Result.mapError (fun e -> (t, e))
        
    let wrapTag f = first f |> Result.mapError
        
    let ofOption error = function Some s -> Ok s | None -> Error error

    type ResultBuilder() =
        member __.Return(x) = Ok x
    
        member __.ReturnFrom(m: Result<_, _>) = m
    
        member __.Bind(m, f) = Result.bind f m
        member __.Bind((m, error): (Option<'T> * 'E), f) = m |> ofOption error |> Result.bind f
    
        member __.Zero() = None
    
        member __.Combine(m, f) = Result.bind f m
    
        member __.Delay(f: unit -> _) = f
    
        member __.Run(f) = f()
    
        member __.TryWith(m, h) =
            try __.ReturnFrom(m)
            with e -> h e
    
        member __.TryFinally(m, compensation) =
            try __.ReturnFrom(m)
            finally compensation()
    
        member __.Using(res:#IDisposable, body) =
            __.TryFinally(body res, fun () -> match res with null -> () | disp -> disp.Dispose())
    
        member __.While(guard, f) =
            if not (guard()) then Ok () else
            do f() |> ignore
            __.While(guard, f)
    
        member __.For(sequence:seq<_>, body) =
            __.Using(sequence.GetEnumerator(), fun enum -> __.While(enum.MoveNext, __.Delay(fun () -> body enum.Current)))
            
    let result = new ResultBuilder()