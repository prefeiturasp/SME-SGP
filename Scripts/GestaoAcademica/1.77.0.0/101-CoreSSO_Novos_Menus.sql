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
		,@nomeModuloPai = 'Frequências' -- Nome do módulo pai (Opcional, apenas quando houver)
		,@nomeModulo = 'Frequência mensal efetivada' -- Nome do módulo (Obrigatório)
		,@SiteMap1Nome = 'Frequência mensal efetivada'
		,@SiteMap1Url = '~/Relatorios/FrequenciaEfetivada/Busca.aspx'
		,@SiteMap2Nome = 'Frequência mensal efetivada'
		,@SiteMap2Url = '~/Relatorios/Relatorio.aspx?dummy=%27yr6MV3EhsVs%3d%27'
		,@SiteMap3Nome = NULL 
		,@SiteMap3Url = NULL
		,@possuiVisaoAdm = 1 -- Indicar se possui visão de administador
		,@possuiVisaoGestao = 1 -- Indicar se possui visão de Gestão
		,@possuiVisaoUA = 1 -- Indicar se possui visão de UA
		,@possuiVisaoIndividual = 0 -- Indicar se possui visão de individual
	
	EXEC MS_InserePaginaMenu
		@nomeSistema = @nomeSistema -- Nome do sistema (obrigatório)
		,@nomeModuloAvo = 'Relatórios' -- Nome do módulo avó (Opcional, apenas quando houver) 
		,@nomeModuloPai = 'Gerais' -- Nome do módulo pai (Opcional, apenas quando houver)
		,@nomeModulo = 'Análise de sondagem consolidada' -- Nome do módulo (Obrigatório)
		,@SiteMap1Nome = 'Análise de sondagem consolidada'
		,@SiteMap1Url = '~/Documentos/AnaliseSondagemConsolidada/Busca.aspx'
		,@SiteMap2Nome = 'Análise de sondagem consolidada'
		,@SiteMap2Url = '~/Relatorios/Relatorio.aspx?dummy=%27ZwwUapTtKmo%3d%27'
		,@SiteMap3Nome = NULL 
		,@SiteMap3Url = NULL
		,@possuiVisaoAdm = 1 -- Indicar se possui visão de administador
		,@possuiVisaoGestao = 1 -- Indicar se possui visão de Gestão
		,@possuiVisaoUA = 0 -- Indicar se possui visão de UA
		,@possuiVisaoIndividual = 0 -- Indicar se possui visão de individual
	
-- Fechar transação
SET XACT_ABORT OFF
COMMIT TRANSACTION	
