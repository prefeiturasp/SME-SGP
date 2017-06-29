USE [CoreSSO]
GO

--Iniciar transação
BEGIN TRANSACTION
SET XACT_ABORT ON

	DECLARE @nomeSistema VARCHAR(100) = ' SGP'

	/***************
		Copiar do exemplo abaixo.
	****************

	-- Insere modulo no menu do sistema no CoreSSO
	EXEC MS_InserePaginaMenu
		@nomeSistema = @nomeSistema -- Nome do sistema (obrigatório)
		,@nomeModuloAvo = '[Preencher]' -- Nome do módulo avó (Opcional, apenas quando houver) 
		,@nomeModuloPai = '[Preencher]' -- Nome do módulo pai (Opcional, apenas quando houver)
		,@nomeModulo = '[Preencher]' -- Nome do módulo (Obrigatório)
		,@SiteMap1Nome = 'Listagem de [Preencher]'
		,@SiteMap1Url = '~/[Preencher]/Busca.aspx'
		,@SiteMap2Nome = 'Cadastro de [Preencher]'
		,@SiteMap2Url = '~/[Preencher]/Cadastro.aspx'
		,@SiteMap3Nome = NULL 
		,@SiteMap3Url = NULL
		,@possuiVisaoAdm = 1 -- Indicar se possui visão de administador
		,@possuiVisaoGestao = 0 -- Indicar se possui visão de Gestão
		,@possuiVisaoUA = 0 -- Indicar se possui visão de UA
		,@possuiVisaoIndividual = 0 -- Indicar se possui visão de individual
	*/

	EXEC MS_InserePaginaMenu
		@nomeSistema = @nomeSistema -- Nome do sistema (obrigatório)
		,@nomeModuloAvo = 'Relatórios' -- Nome do módulo avó (Opcional, apenas quando houver) 
		,@nomeModuloPai = 'Relatórios de atendimento' -- Nome do módulo pai (Opcional, apenas quando houver)
		,@nomeModulo = 'Relatório geral' -- Nome do módulo (Obrigatório)
		,@SiteMap1Nome = 'Relatório geral'
		,@SiteMap1Url = '~/Relatorios/RelatorioGeralAtendimento/Busca.aspx'
		,@SiteMap2Nome = 'Relatório geral'
		,@SiteMap2Url = '~/Documentos/Relatorio.aspx?dummy=%27iGnoDuhcFtk%3d%27'
		,@SiteMap3Nome = NULL 
		,@SiteMap3Url = NULL
		,@possuiVisaoAdm = 1 -- Indicar se possui visão de administador
		,@possuiVisaoGestao = 1 -- Indicar se possui visão de Gestão
		,@possuiVisaoUA = 1 -- Indicar se possui visão de UA
		,@possuiVisaoIndividual = 0 -- Indicar se possui visão de individual
		
	EXEC MS_InserePaginaMenu
		@nomeSistema = @nomeSistema -- Nome do sistema (obrigatório)
		,@nomeModuloAvo = 'Relatórios' -- Nome do módulo avó (Opcional, apenas quando houver) 
		,@nomeModuloPai = 'Gestor' -- Nome do módulo pai (Opcional, apenas quando houver)
		,@nomeModulo = 'Ações realizadas' -- Nome do módulo (Obrigatório)
		,@SiteMap1Nome = 'Relatórios de ações realizadas'
		,@SiteMap1Url = '~/Relatorios/AcoesRealizadas/Busca.aspx'
		,@SiteMap2Nome = NULL
		,@SiteMap2Url = NULL
		,@SiteMap3Nome = NULL 
		,@SiteMap3Url = NULL
		,@possuiVisaoAdm = 1 -- Indicar se possui visão de administador
		,@possuiVisaoGestao = 1 -- Indicar se possui visão de Gestão
		,@possuiVisaoUA = 1 -- Indicar se possui visão de UA
		,@possuiVisaoIndividual = 1 -- Indicar se possui visão de individual

	EXEC MS_InserePaginaMenu
		@nomeSistema = @nomeSistema -- Nome do sistema (obrigatório)
		,@nomeModuloAvo = 'Relatórios' -- Nome do módulo avó (Opcional, apenas quando houver) 
		,@nomeModuloPai = 'Gráficos' -- Nome do módulo pai (Opcional, apenas quando houver)
		,@nomeModulo = 'Gráfico de justificativa de falta' -- Nome do módulo (Obrigatório)
		,@SiteMap1Nome = 'Gráfico de justificativa de falta'
		,@SiteMap1Url = '~/Relatorios/GraficoJustificativaFalta/Busca.aspx'
		,@SiteMap2Nome = 'Gráfico de justificativa de falta'
		,@SiteMap2Url = '~/Documentos/RelatorioDev.aspx?dummy=%277RDgZU5k%2bhU%3d%27'
		,@SiteMap3Nome = NULL 
		,@SiteMap3Url = NULL
		,@possuiVisaoAdm = 1 -- Indicar se possui visão de administador
		,@possuiVisaoGestao = 1 -- Indicar se possui visão de Gestão
		,@possuiVisaoUA = 1 -- Indicar se possui visão de UA
		,@possuiVisaoIndividual = 0 -- Indicar se possui visão de individual

	EXEC MS_InserePaginaMenu
		@nomeSistema = @nomeSistema -- Nome do sistema (obrigatório)
		,@nomeModuloAvo = NULL -- Nome do módulo avó (Opcional, apenas quando houver) 
		,@nomeModuloPai = 'Registro de Classe' -- Nome do módulo pai (Opcional, apenas quando houver)
		,@nomeModulo = 'Relatório NAAPA' -- Nome do módulo (Obrigatório)
		,@SiteMap1Nome = 'Consulta de relatórios do NAAPA'
		,@SiteMap1Url = '~/Classe/RelatorioNaapa/Busca.aspx'
		,@SiteMap2Nome = 'Lançamento de relatórios do NAAPA'
		,@SiteMap2Url = '~/Classe/RelatorioNaapa/Cadastro.aspx'
		,@possuiVisaoAdm = 1 -- Indicar se possui visão de administador
		,@possuiVisaoGestao = 1 -- Indicar se possui visão de Gestão
		,@possuiVisaoUA = 1 -- Indicar se possui visão de UA
		,@possuiVisaoIndividual = 1 -- Indicar se possui visão de individual


-- Fechar transação
SET XACT_ABORT OFF
COMMIT TRANSACTION	
