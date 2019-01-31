module FreshBooks2Harvest.Domain
open System

type TokenRequest = {
    clientId : string
    clientSecret : string
    code : string
    grantType : string
    redirectUrl : string
}


module TokenRequest =

    let create clientId clientSecret code grantType redirectUrl = { 
        clientId = clientId
        clientSecret = clientSecret
        code = code
        grantType = grantType
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