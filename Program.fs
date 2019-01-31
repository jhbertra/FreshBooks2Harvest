open System
open FreshBooks2Harvest
open FreshBooks2Harvest.Common.Result
open FreshBooks2Harvest.Domain


let renderFreshBooksAuthConfigField = function
| ClientId -> "clientId" 
| ClientSecret -> "clientSecret" 
| AuthorizationCode -> "authorizationCode"
| RedirectUrl -> "redirectUrl"


let renderAuthConfigField = function
| FreshBooks -> "freshBooks"
| FreshBooksChild child -> sprintf "freshBooks.%s" (renderFreshBooksAuthConfigField child)


let renderConfigField = function
| DataDir -> "dataDir"
| Auth -> "auth"
| AuthChild child -> sprintf "auth.%s" (renderAuthConfigField child)


let renderExternalSource = function
| ExternalSource.ConfigFile -> "config file"
| ExternalSource.DataDir -> "data directory"


let rec render = function
| Pure e -> [e]
| ConfigInvalid (field, e) -> renderErrorNode "Config file invalid:" (renderConfigField field) e
| IoError (source, e) -> renderErrorNode "Config file invalid:" (renderExternalSource source) e
| JsonInvalid (entity, e) -> renderErrorNode "Json invalid:" (sprintf "%A" entity) e
    
and renderErrorNode msg label e =
    let prefix = sprintf "  - %s: " label
    [
        yield msg
        yield! render e |> List.mapi (fun i -> if i = 0 then sprintf "%s%s" prefix else sprintf "    %s")
    ] 


[<EntryPoint>]
let main argv =
    result {
        let! configFile = readFile ConfigFile "f2h.config.json"
        let! config = Json.configFromJson configFile
        printf "Config: %A" config |> ignore
        return ()
    }
    |> (fun r ->
           match r with
           | Ok _ -> 0
           | Error error ->
               eprintf "Error: %s" (System.String.Join (Environment.NewLine, render error)) |> ignore
               -1
       )