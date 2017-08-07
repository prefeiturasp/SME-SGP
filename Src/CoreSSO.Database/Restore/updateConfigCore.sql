
-- SGP
UPDATE SYS_Sistema
SET sis_caminho = '$UrlSGPLogin$', sis_caminhoLogout = '$UrlSGPLogout$'
WHERE sis_id = 102

-- Boletim Online
UPDATE SYS_Sistema
SET sis_caminho = '$UrlBoletimLogin$', sis_caminhoLogout = '$UrlBoletimLogout$'
WHERE sis_id = 174