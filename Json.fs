module FreshBooks2Harvest.Json

open FreshBooks2Harvest.Common.Result
open FreshBooks2Harvest.Dto
open Newtonsoft.Json

let serialize = JsonConvert.SerializeObject

let deserialize<'a> str =
    try
        let deserialized = JsonConvert.DeserializeObject<'a> str
        if box deserialized = null then
            Error "Failed to read config"
        else
            Ok deserialized
    with
    | e -> Error e.Message
    

let configFromJson = deserialize<ConfigDto> >=> ConfigDto.toDomain