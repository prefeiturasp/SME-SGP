USE [PUB_DEV_SPO_CoreSSO]
GO

BEGIN TRANSACTION 
SET XACT_ABORT ON   

	DECLARE @sis_id INT = 102
	DECLARE @visaoAdm INT = 1
	DECLARE @visaoGestao INT = 2
	DECLARE @visaoUnidade INT = 3
	DECLARE @nomeSistema VARCHAR(100) = ' SGP'

	EXEC MS_InserePaginaMenu
		@nomeSistema = @nomeSistema -- Nome do sistema (obrigatório)
		,@nomeModuloAvo = NULL -- Nome do módulo avó (Opcional, apenas quando houver) 
		,@nomeModuloPai = 'Relatórios' -- Nome do módulo pai (Opcional, apenas quando houver)
		,@nomeModulo = 'Frequências' -- Nome do módulo (Obrigatório)
		,@SiteMap1Nome = 'Frequências'
		,@SiteMap1Url = NULL
		,@SiteMap2Nome = NULL
		,@SiteMap2Url = NULL
		,@SiteMap3Nome = NULL
		,@SiteMap3Url = NULL
		,@possuiVisaoAdm = 1 -- Indicar se possui visão de administador
		,@possuiVisaoGestao = 1 -- Indicar se possui visão de Gestão
		,@possuiVisaoUA = 1 -- Indicar se possui visão de UA
		,@possuiVisaoIndividual = 0 -- Indicar se possui visão de individual

	DECLARE @mod_idPai INT = (SELECT mod_id FROM SYS_Modulo WHERE mod_nome = 'Frequências' AND sis_id = @sis_id AND mod_situacao = 1)

	UPDATE SYS_Modulo 
	SET mod_idPai = @mod_idPai
	WHERE sis_id = @sis_id AND mod_nome = 'Alunos com baixa frequência'

	UPDATE SYS_Modulo 
	SET mod_idPai = @mod_idPai
	WHERE sis_id = @sis_id AND mod_nome = 'Indicador de frequência'

	UPDATE SYS_Modulo 
	SET mod_idPai = @mod_idPai
	WHERE sis_id = @sis_id AND mod_nome = 'Frequência mensal'

	UPDATE SYS_Modulo 
	SET mod_idPai = @mod_idPai
	WHERE sis_id = @sis_id AND mod_nome = 'Justificativas de falta'
	
-- Fechar transação     
SET XACT_ABORT OFF
COMMIT TRANSACTION