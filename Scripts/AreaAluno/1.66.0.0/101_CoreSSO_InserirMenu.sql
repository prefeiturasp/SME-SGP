USE [CoreSSO]
GO

BEGIN TRANSACTION
SET XACT_ABORT ON

	-- Nome do sistema.
	DECLARE @nomeSistema VARCHAR(200)= 'Boletim Online'

	EXEC MS_InserePaginaMenu
		@nomeSistema = @nomeSistema,
		@nomeModuloPai = NULL,
		@nomeModulo = 'Documentos da escola',
		@SiteMap1Nome = 'Documentos da escola',
		@SiteMap1Url = '~/Consulta/Documentos/Busca.aspx',
		@SiteMap2Nome = NULL,
		@SiteMap2Url = NULL,
		@SiteMap3Nome = NULL,
		@SiteMap3Url = NULL,
		@possuiVisaoAdm = 0,
		@possuiVisaoGestao = 0,
		@possuiVisaoUA = 0,
		@possuiVisaoIndividual = 1
		
	EXEC MS_InserePaginaMenu
		@nomeSistema = @nomeSistema,
		@nomeModuloPai = 'Documentos da escola',
		@nomeModulo = 'Documentos da escola',
		@SiteMap1Nome = NULL,
		@SiteMap1Url = NULL,
		@SiteMap2Nome = NULL,
		@SiteMap2Url = NULL,
		@SiteMap3Nome = NULL,
		@SiteMap3Url = NULL,
		@possuiVisaoAdm = 0,
		@possuiVisaoGestao = 0,
		@possuiVisaoUA = 0,
		@possuiVisaoIndividual = 1	

	DECLARE @sis_id INT, @mod_id INT
	SET @sis_id = (SELECT sis_id FROM SYS_Sistema WHERE sis_nome = @nomeSistema AND sis_situacao = 1)

	SET @mod_id = (SELECT mod_id FROM SYS_Modulo WHERE sis_id = @sis_id AND mod_nome = 'Documentos da escola' AND mod_idPai IS NOT NULL AND mod_situacao = 1)

	DELETE FROM SYS_ModuloSiteMap WHERE mod_id = @mod_id AND sis_id = @sis_id

SET XACT_ABORT OFF
COMMIT TRANSACTION	
