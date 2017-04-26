USE [CoreSSO]
GO

--Iniciar transação
BEGIN TRANSACTION
SET XACT_ABORT ON

	DECLARE @nomeSistema VARCHAR(100) = ' SGP'

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

	EXEC MS_InsereSiteMap
		@nomeSistema = @nomeSistema -- Nome do sistema (Obrigatório - Vária de acordo com o cliente)
		,@nomeModulo = 'Objetos de conhecimento' -- Nome do módulo (Obrigatório)
		,@SiteMapNome = 'Consulta de eixos de objetos de conhecimento' -- Nome do SiteMap (Obrigatório)
		,@SiteMapUrl = '~/Academico/ObjetoAprendizagem/BuscaEixo.aspx' -- Url da SiteMap (Obrigatório)
		
	EXEC MS_InsereSiteMap
		@nomeSistema = @nomeSistema -- Nome do sistema (Obrigatório - Vária de acordo com o cliente)
		,@nomeModulo = 'Objetos de conhecimento' -- Nome do módulo (Obrigatório)
		,@SiteMapNome = 'Cadastro de eixos de objetos de conhecimento' -- Nome do SiteMap (Obrigatório)
		,@SiteMapUrl = '~/Academico/ObjetoAprendizagem/CadastroEixo.aspx' -- Url da SiteMap (Obrigatório)
		
-- Fechar transação
SET XACT_ABORT OFF
COMMIT TRANSACTION