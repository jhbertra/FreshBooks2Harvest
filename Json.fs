module FreshBooks2Harvest.Json

open FreshBooks2Harvest.Common.Result
open FreshBooks2Harvest.Dto
open FreshBooks2Harvest.Domain
open Newtonsoft.Json

let serialize = JsonConvert.SerializeObject

let deserialize<'a> entity str =
    try
        let deserialized = JsonConvert.DeserializeObject<'a> str
        if box deserialized = null then
            JsonInvalid (entity, Pure "Failed to read json") |> Error
        else
            Ok deserialized
    with
    | e -> JsonInvalid (entity, Pure e.Message) |> Error
    

let configFromJson = deserialize<ConfigDto> Config >=> ConfigDto.toDomain