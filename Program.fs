open FreshBooks2Harvest
open FreshBooks2Harvest.Common.Result

[<EntryPoint>]
let main argv =
    result {
        let! configFile = readFile "f2h.config.json"
        let! config = Json.configFromJson configFile
        printf "Config: %A" config |> ignore
        return ()
    }
    |> (fun r ->
           match r with
           | Ok _ -> 0
           | Error error ->
               printf "Error: %s" error |> ignore
               -1
       )