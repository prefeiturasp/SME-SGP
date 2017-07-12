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
			
	-- Alteração titulo do relatório do resumo das atividades desenvolvidas	
	update SYS_ModuloSitemap 
		set msm_nome = 'Resumo das atividades desenvolvidas' 
		where msm_url = '~/Documentos/Relatorio.aspx?dummy=%27AU93aqlCeUI%3d%27'
		
		
		
		

-- Fechar transação
SET XACT_ABORT OFF
COMMIT TRANSACTION