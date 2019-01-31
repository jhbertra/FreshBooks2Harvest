module FreshBooks2Harvest.Domain
open System
open System.IO


type FreshBooksAuthorizationConfig = {
    clientId : string
    clientSecret : string
    authorizationCode : string
    redirectUrl : string
}


module FreshBooksAuthorizationConfig =
    
    let create clientId clientSecret authorizationCode redirectUrl = {
        clientId = clientId
        clientSecret = clientSecret
        authorizationCode = authorizationCode
        redirectUrl = redirectUrl
    }
    
    


type AuthorizationConfig = {
    freshBooks : FreshBooksAuthorizationConfig
}


module AuthorizationConfig =
    
    let create freshBooks = {
        freshBooks = freshBooks
    }


type Config = {
    dataDir : DirectoryInfo
    auth : AuthorizationConfig
}


module Config =
    
    let create dataDir auth = {
        dataDir = dataDir
        auth = auth
    }


type TokenRequest = {
    clientId : string
    clientSecret : string
    code : string
    redirectUrl : string
}


module TokenRequest =

    let create clientId clientSecret code redirectUrl = { 
        clientId = clientId
        clientSecret = clientSecret
        code = code
        redirectUrl = redirectUrl
    }



type TokenResponse = {
    accessToken : string
    createdAt : DateTimeOffset
    expiresIn : TimeSpan
    refreshToken : string
    scope : string
    tokenType : string
}


module TokenResponse =

    let create accessToken createdAt expiresIn refreshToken scope tokenType = {
        accessToken = accessToken
        createdAt = createdAt
        expiresIn = expiresIn
        refreshToken = refreshToken
        scope = scope
        tokenType = tokenType 
    }


type Business = {
    accountId : string
    id : string
}


module Business =

    let create accountId id = {
        accountId = accountId
        id = id 
    }


type Client = {
    id : string
    organization : string
}


module Client =

    let create id organization = {
        id = id
        organization = organization 
    }


type Service = {
    id : string
    name : string
}


module Service =

    let create id name = {
        id = id
        name = name 
    }


type Project = {
    client : Client
    id : string
    services : string list
    title : string
}


module Project =

    let create client id services title = {
        client = client
        id = id
        services = services
        title = title 
    }


type TimeEntry = {
    billed : bool
    client : Client
    duration : TimeSpan
    harvestId : string option
    id : string
    isLogged : bool
    note : string
    project : Project
    serviceId : string
    startedAt : DateTimeOffset
}


module TimeEntry =
    
    let create billed client duration harvestId id isLogged note project serviceId startedAt = {
        billed = billed
        client = client
        duration = duration
        harvestId = harvestId
        id = id
        isLogged = isLogged
        note = note
        project = project
        serviceId = serviceId
        startedAt = startedAt 
    }
    
type ExternalSource =
    | ConfigFile
    | DataDir

type JsonEntity =
    | Config
    
type FreshBooksAuthField =
    | ClientId
    | ClientSecret
    | AuthorizationCode
    | RedirectUrl
    
type AuthField =
    | FreshBooks
    | FreshBooksChild of FreshBooksAuthField
    
type ConfigField =
    | DataDir
    | Auth
    | AuthChild of AuthField 

type Error =
    | ConfigInvalid of (ConfigField * Error)
    | IoError of (ExternalSource * Error)
    | JsonInvalid of (JsonEntity * Error)
    | Pure of string
