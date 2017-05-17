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
		,@nomeModuloAvo = 'Administração' -- Nome do módulo avó (Opcional, apenas quando houver) 
		,@nomeModuloPai = 'Cadastros' -- Nome do módulo pai (Opcional, apenas quando houver)
		,@nomeModulo = 'Sondagem' -- Nome do módulo (Obrigatório)
		,@SiteMap1Nome = 'Listagem de sondagens'
		,@SiteMap1Url = '~/Academico/Sondagem/Busca.aspx'
		,@SiteMap2Nome = 'Cadastro de sondagem'
		,@SiteMap2Url = '~/Academico/Sondagem/Cadastro.aspx'
		,@SiteMap3Nome = 'Agendamento de sondagem' 
		,@SiteMap3Url = '~/Academico/Sondagem/Agendamento.aspx'
		,@possuiVisaoAdm = 1 -- Indicar se possui visão de administador
		,@possuiVisaoGestao = 0 -- Indicar se possui visão de Gestão
		,@possuiVisaoUA = 0 -- Indicar se possui visão de UA
		,@possuiVisaoIndividual = 0 -- Indicar se possui visão de individual


	EXEC MS_InserePaginaMenu
		@nomeSistema = @nomeSistema -- Nome do sistema (obrigatório)
		,@nomeModuloAvo = 'Relatórios' -- Nome do módulo avó (Opcional, apenas quando houver) 
		,@nomeModuloPai = 'Gestor' -- Nome do módulo pai (Opcional, apenas quando houver)
		,@nomeModulo = 'Análise de sondagem' -- Nome do módulo (Obrigatório)
		,@SiteMap1Nome = 'Análise de sondagem'
		,@SiteMap1Url = '~/Documentos/AnaliseSondagem/Busca.aspx'
		,@SiteMap2Nome = 'Análise de sondagem'
		,@SiteMap2Url = '~/Relatorios/Relatorio.aspx?dummy=%27QZ3yyOWkxQU%3d%27'
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
		,@nomeModulo = 'Sondagem' -- Nome do módulo (Obrigatório)
		,@SiteMap1Nome = 'Consulta de sondagens'
		,@SiteMap1Url = '~/Classe/LancamentoSondagem/Busca.aspx'
		,@SiteMap2Nome = 'Lançamento de sondagem'
		,@SiteMap2Url = '~/Classe/LancamentoSondagem/Cadastro.aspx'
		,@SiteMap3Nome = NULL 
		,@SiteMap3Url = NULL
		,@possuiVisaoAdm = 1 -- Indicar se possui visão de administador
		,@possuiVisaoGestao = 1 -- Indicar se possui visão de Gestão
		,@possuiVisaoUA = 1 -- Indicar se possui visão de UA
		,@possuiVisaoIndividual = 1 -- Indicar se possui visão de individual
		
	EXEC MS_InserePaginaMenu
		@nomeSistema = @nomeSistema -- Nome do sistema (obrigatório)
		,@nomeModuloAvo = 'Relatórios' -- Nome do módulo avó (Opcional, apenas quando houver) 
		,@nomeModuloPai = 'Gestor' -- Nome do módulo pai (Opcional, apenas quando houver)
		,@nomeModulo = 'Frequência mensal' -- Nome do módulo (Obrigatório)
		,@SiteMap1Nome = 'Frequência mensal'
		,@SiteMap1Url = '~/Relatorios/FrequenciaMensal/Busca.aspx'
		,@SiteMap2Nome = 'Frequência mensal'
		,@SiteMap2Url = '~/Relatorios/Relatorio.aspx?dummy=%27%2fRS0a%2frpFBw%3d%27'
		,@SiteMap3Nome = NULL
		,@SiteMap3Url = NULL
		,@possuiVisaoAdm = 1 -- Indicar se possui visão de administador
		,@possuiVisaoGestao = 1 -- Indicar se possui visão de Gestão
		,@possuiVisaoUA = 1 -- Indicar se possui visão de UA
		,@possuiVisaoIndividual = 0 -- Indicar se possui visão de individual

-- Fechar transação
SET XACT_ABORT OFF
COMMIT TRANSACTION	
