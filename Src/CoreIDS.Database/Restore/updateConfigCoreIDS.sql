DECLARE
	@clientId INT

SELECT @clientId = id FROM IDS_Clients WHERE ClientId = '$ClientIdMvc$'

-- SGP
IF(NOT EXISTS(SELECT * FROM IDS_ClientCorsOrigins AS icco WHERE icco.ClientId = @clientId AND icco.Origin = '$UrlSGP$'))
BEGIN
	INSERT INTO IDS_ClientCorsOrigins (ClientId, Origin) VALUES (@clientId, '$UrlSGP$')
END

IF(NOT EXISTS(SELECT * FROM IDS_ClientPostLogoutRedirectUris AS icplru WHERE icplru.ClientId = @clientId AND icplru.PostLogoutRedirectUri = '$UrlSGPLogin$'))
BEGIN
	INSERT INTO IDS_ClientPostLogoutRedirectUris (ClientId, PostLogoutRedirectUri) VALUES (@clientId, '$UrlSGPLogin$')
END

-- Boletim Online
IF(NOT EXISTS(SELECT * FROM IDS_ClientRedirectUris AS icru WHERE icru.ClientId = @clientId AND icru.RedirectUri = '$UrlBoletimLogin$'))
BEGIN
	INSERT INTO IDS_ClientRedirectUris (ClientId, RedirectUri) VALUES (@clientId, '$UrlBoletimLogin$')
END

IF(NOT EXISTS(SELECT * FROM IDS_ClientPostLogoutRedirectUris AS icplru WHERE icplru.ClientId = @clientId AND icplru.PostLogoutRedirectUri = '$UrlBoletimLogin$'))
BEGIN
	INSERT INTO IDS_ClientPostLogoutRedirectUris (ClientId, PostLogoutRedirectUri) VALUES (@clientId, '$UrlBoletimLogin$')
END