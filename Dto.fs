module FreshBooks2Harvest.Dto
    
open FreshBooks2Harvest.Domain
open FreshBooks2Harvest.Common.Result


type ConfigDto = {
    dataDir : string
    freshbooksClientId : string
    freshbooksClientSecret : string
    freshbooksCode : string
    freshbooksRedirectUrl : string
}


module ConfigDto =
    
    let toDomain (configDto : ConfigDto) : Result<Config, string> =
        Config.create
            <^> (notNull "dataDir" configDto.dataDir >>= createDirectory)
            <*> (notNull "freshbooksClientId" configDto.freshbooksClientId)
            <*> (notNull "freshbooksClientSecret" configDto.freshbooksClientSecret)
            <*> (notNull "freshbooksCode" configDto.freshbooksCode)
            <*> (notNull "freshbooksRedirectUrl" configDto.freshbooksRedirectUrl)