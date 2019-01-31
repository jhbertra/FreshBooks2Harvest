module FreshBooks2Harvest.Dto
    
open FreshBooks2Harvest.Domain
open FreshBooks2Harvest.Common
open FreshBooks2Harvest.Common.Result

type FreshBooksAuthorizationConfigDto = {
    clientId : string
    clientSecret : string
    authorizationCode : string
    redirectUrl : string
}

type AuthorizationConfigDto = {
    freshBooks : FreshBooksAuthorizationConfigDto
}

type ConfigDto = {
    dataDir : string
    auth : AuthorizationConfigDto
}


module ConfigDto =
    open System

    let freshBooksAuthConfigToDomain freshBooks =
        FreshBooksAuthorizationConfig.create
            <^> notNull ClientId freshBooks.clientId
            <*> notNull ClientSecret freshBooks.clientSecret
            <*> notNull AuthorizationCode freshBooks.authorizationCode
            <*> notNull RedirectUrl freshBooks.redirectUrl

    let private authConfigToDomain auth =
        AuthorizationConfig.create
            <^> (notNull FreshBooks auth.freshBooks
                >>= (freshBooksAuthConfigToDomain >> Result.wrapTag FreshBooksChild))
    
    let toDomain configDto =
        Config.create
            <^> (notNull DataDir configDto.dataDir
                |> Result.map
                    (fun dir -> dir.Replace("~", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)))
                >>= (createDirectory ExternalSource.DataDir >> Result.tag DataDir))
            <*> (notNull Auth configDto.auth
                >>= (authConfigToDomain >> Result.wrapTag AuthChild))
        |> Result.mapError ConfigInvalid