-- Configuração do API
UPDATE CFG_Configuracao
SET cfg_valor = '$UrlApi$'
WHERE
	cfg_chave = 'UrlGestaoAcademicaWebApi'

-- Configuração do serviço
UPDATE CFG_Configuracao
SET cfg_valor = '$HostService$'
WHERE
	cfg_chave = 'AppSchedulerHost'