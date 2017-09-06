
-- Relatórios - Servidor
UPDATE 
	CFG_ServidorRelatorio 
SET 
	srr_usuario = ''
	,srr_senha = ''
	,srr_dominio = ''
	,srr_diretorioRelatorios = ''
	,srr_pastaRelatorios = ''

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

-- Deletar serviços do Gestao Escolar
DELETE FROM QTZ_Blob_Triggers
DELETE FROM QTZ_Calendars
DELETE FROM QTZ_Cron_Triggers
DELETE FROM QTZ_Fired_Triggers
DELETE FROM QTZ_Locks
DELETE FROM QTZ_Paused_Trigger_Grps
DELETE FROM QTZ_Scheduler_State
DELETE FROM QTZ_Simple_Triggers
DELETE FROM QTZ_Simprop_Triggers
DELETE FROM QTZ_Triggers
DELETE FROM QTZ_Job_Details


