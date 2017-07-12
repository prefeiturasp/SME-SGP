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
	

	EXEC MS_InserePaginaMenu
		@nomeSistema = @nomeSistema -- Nome do sistema (obrigatório)
		,@nomeModuloAvo = NULL -- Nome do módulo avó (Opcional, apenas quando houver) 
		,@nomeModuloPai = 'Relatórios' -- Nome do módulo pai (Opcional, apenas quando houver)
		,@nomeModulo = 'Relatórios de atendimento' -- Nome do módulo (Obrigatório)
		,@SiteMap1Nome = 'Relatórios de atendimento'
		,@SiteMap1Url = NULL
		,@SiteMap2Nome = NULL
		,@SiteMap2Url = NULL
		,@SiteMap3Nome = NULL
		,@SiteMap3Url = NULL
		,@possuiVisaoAdm = 1 -- Indicar se possui visão de administador
		,@possuiVisaoGestao = 1 -- Indicar se possui visão de Gestão
		,@possuiVisaoUA = 1 -- Indicar se possui visão de UA
		,@possuiVisaoIndividual = 0 -- Indicar se possui visão de individual
		
	-- Alteração titulo do relatório do resumo das atividades desenvolvidas	no banco DO CORESSO
	update SYS_ModuloSitemap 
		set msm_nome = 'Resumo das atividades desenvolvidas' 
		where msm_url = '~/Documentos/Relatorio.aspx?dummy=%27AU93aqlCeUI%3d%27'
	
	-- Alteração na opção do menudo do relatório de docente
	update cfg_relatoriodocumentodocente 
		set rdd_nomeDocumento = 'Resumo das atividades desenvolvidas' 
		where rlt_id = 248	
		
		
		

-- Fechar transação
SET XACT_ABORT OFF
COMMIT TRANSACTION