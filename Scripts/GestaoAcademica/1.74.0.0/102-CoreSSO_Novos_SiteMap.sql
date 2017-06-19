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
		,@nomeModulo = 'Cadastro de questionários' -- Nome do módulo (Obrigatório)
		,@SiteMapNome = 'Listagem de conteúdos' -- Nome do SiteMap (Obrigatório)
		,@SiteMapUrl = '~/Configuracao/Questionario/BuscaConteudo.aspx' -- Url da SiteMap (Obrigatório)

	EXEC MS_InsereSiteMap
		@nomeSistema = @nomeSistema -- Nome do sistema (Obrigatório - Vária de acordo com o cliente)
		,@nomeModulo = 'Cadastro de questionários' -- Nome do módulo (Obrigatório)
		,@SiteMapNome = 'Cadastro de conteúdos' -- Nome do SiteMap (Obrigatório)
		,@SiteMapUrl = '~/Configuracao/Questionario/CadastroConteudo.aspx' -- Url da SiteMap (Obrigatório)

	EXEC MS_InsereSiteMap
		@nomeSistema = @nomeSistema -- Nome do sistema (Obrigatório - Vária de acordo com o cliente)
		,@nomeModulo = 'Cadastro de questionários' -- Nome do módulo (Obrigatório)
		,@SiteMapNome = 'Listagem de respostas' -- Nome do SiteMap (Obrigatório)
		,@SiteMapUrl = '~/Configuracao/Questionario/BuscaResposta.aspx' -- Url da SiteMap (Obrigatório)

	EXEC MS_InsereSiteMap
		@nomeSistema = @nomeSistema -- Nome do sistema (Obrigatório - Vária de acordo com o cliente)
		,@nomeModulo = 'Cadastro de questionários' -- Nome do módulo (Obrigatório)
		,@SiteMapNome = 'Cadastro de respostas' -- Nome do SiteMap (Obrigatório)
		,@SiteMapUrl = '~/Configuracao/Questionario/CadastroResposta.aspx' -- Url da SiteMap (Obrigatório)

-- Fechar transação
SET XACT_ABORT OFF
COMMIT TRANSACTION