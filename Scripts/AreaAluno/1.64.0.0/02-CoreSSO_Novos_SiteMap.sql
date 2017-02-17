USE [CoreSSO]
GO

--Iniciar transação
BEGIN TRANSACTION
SET XACT_ABORT ON

	DECLARE @nomeSistema VARCHAR(100) = 'Boletim Online'

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
		,@nomeModulo = 'Compromisso de Estudo' -- Nome do módulo (Obrigatório)
		,@SiteMapNome = 'Cadastro de compromisso de estudo' -- Nome do SiteMap (Obrigatório)
		,@SiteMapUrl = '~/Cadastro/CompromissoEstudo/Cadastro.aspx' -- Url da SiteMap (Obrigatório)

-- Fechar transação
SET XACT_ABORT OFF
COMMIT TRANSACTION