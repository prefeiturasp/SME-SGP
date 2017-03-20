USE [CoreSSO]
GO

--Iniciar transação
BEGIN TRANSACTION
SET XACT_ABORT ON

	DECLARE @nomeSistema VARCHAR(100) = ' SGP'
	DECLARE @sis_id INT = (SELECT TOP 1 sis_id FROM SYS_Sistema WHERE sis_nome = @nomeSistema)

	/***************
		Copiar do exemplo abaixo.
	****************
		
	-- Insere o SiteMap da pagina no CoreSSO
	EXEC MS_InsereSiteMap
		@nomeSistema = @nomeSistema -- Nome do sistema (Obrigatório - Vária de acordo com o cliente)
		,@nomeModulo = 'Tipo de evento' -- Nome do módulo (Obrigatório)
		,@SiteMapNome = 'Listagem de tipos de evento' -- Nome do SiteMap (Obrigatório)
		,@SiteMapUrl = '~/Configuracao/TipoEvento/Busca.aspx' -- Url da SiteMap (Obrigatório)
	*/
	
	DECLARE @mod_id INT = (SELECT TOP 1 mod_id FROM SYS_Modulo WITH(NOLOCK) WHERE mod_nome = 'Áreas' AND sis_id = @sis_id AND mod_situacao <> 3)
	UPDATE SYS_ModuloSitemap
	SET msm_nome = 'Cadastro de links/documentos'
	WHERE mod_id = @mod_id AND sis_id = @sis_id AND msm_nome = 'Cadastro de documentos'
	UPDATE SYS_ModuloSitemap
	SET msm_nome = 'Cadastro de documentos'
	WHERE mod_id = @mod_id AND sis_id = @sis_id AND msm_nome = 'Cadastro de áreas para links e documentos.'

-- Fechar transação
SET XACT_ABORT OFF
COMMIT TRANSACTION