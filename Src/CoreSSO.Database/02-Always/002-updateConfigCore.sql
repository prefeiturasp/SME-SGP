
-- SGP
UPDATE SYS_Sistema
SET sis_caminho = '$UrlSGPLogin$', sis_caminhoLogout = '$UrlSGPLogout$', sis_situacao = 1
WHERE sis_id = 102

-- Boletim Online
UPDATE SYS_Sistema
SET sis_caminho = '$UrlBoletimLogin$', sis_caminhoLogout = '$UrlBoletimLogout$', sis_situacao = 1
WHERE sis_id = 174