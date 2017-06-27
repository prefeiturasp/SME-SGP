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
		,@nomeModuloAvo = NULL -- Nome do módulo avó (Opcional, apenas quando houver) 
		,@nomeModuloPai = 'Configurações' -- Nome do módulo pai (Opcional, apenas quando houver)
		,@nomeModulo = 'Agendamento de sondagem' -- Nome do módulo (Obrigatório)
		,@SiteMap1Nome = 'Listagem de sondagens'
		,@SiteMap1Url = '~/Academico/AgendamentoSondagem/Busca.aspx'
		,@SiteMap2Nome = 'Agendamento de sondagem'
		,@SiteMap2Url = '~/Academico/AgendamentoSondagem/Agendamento.aspx'
		,@SiteMap3Nome = NULL 
		,@SiteMap3Url = NULL
		,@possuiVisaoAdm = 1 -- Indicar se possui visão de administador
		,@possuiVisaoGestao = 0 -- Indicar se possui visão de Gestão
		,@possuiVisaoUA = 0 -- Indicar se possui visão de UA
		,@possuiVisaoIndividual = 0 -- Indicar se possui visão de individual
	
	EXEC MS_InserePaginaMenu
		@nomeSistema = @nomeSistema -- Nome do sistema (obrigatório)
		,@nomeModuloAvo = NULL -- Nome do módulo avó (Opcional, apenas quando houver) 
		,@nomeModuloPai = 'Administração' -- Nome do módulo pai (Opcional, apenas quando houver)
		,@nomeModulo = 'Currículo' -- Nome do módulo (Obrigatório)
		,@SiteMap1Nome = NULL
		,@SiteMap1Url = NULL
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
		,@nomeModuloAvo = 'Administração' -- Nome do módulo avó (Opcional, apenas quando houver) 
		,@nomeModuloPai = 'Currículo' -- Nome do módulo pai (Opcional, apenas quando houver)
		,@nomeModulo = 'Cadastro de currículo' -- Nome do módulo (Obrigatório)
		,@SiteMap1Nome = 'Cadastro de currículo'
		,@SiteMap1Url = '~/Academico/Curriculo/Cadastro.aspx'
		,@SiteMap2Nome = NULL 
		,@SiteMap2Url = NULL 
		,@SiteMap3Nome = NULL 
		,@SiteMap3Url = NULL
		,@possuiVisaoAdm = 1 -- Indicar se possui visão de administador
		,@possuiVisaoGestao = 0 -- Indicar se possui visão de Gestão
		,@possuiVisaoUA = 0 -- Indicar se possui visão de UA
		,@possuiVisaoIndividual = 0 -- Indicar se possui visão de individual

	EXEC MS_InserePaginaMenu
		@nomeSistema = @nomeSistema -- Nome do sistema (obrigatório)
		,@nomeModuloAvo = 'Administração' -- Nome do módulo avó (Opcional, apenas quando houver) 
		,@nomeModuloPai = 'Currículo' -- Nome do módulo pai (Opcional, apenas quando houver)
		,@nomeModulo = 'Registro de sugestões' -- Nome do módulo (Obrigatório)
		,@SiteMap1Nome = 'Registro de sugestões'
		,@SiteMap1Url = '~/Academico/Curriculo/RegistroSugestoes.aspx'
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
		,@nomeModuloPai = 'Gestor' -- Nome do módulo pai (Opcional, apenas quando houver)
		,@nomeModulo = 'Sugestões de currículos' -- Nome do módulo (Obrigatório)
		,@SiteMap1Nome = 'Sugestões de currículos'
		,@SiteMap1Url = '~/Relatorios/RelatorioSugestoesCurriculo/Busca.aspx'
		,@SiteMap2Nome = 'Sugestões de currículos' 
		,@SiteMap2Url = '~/Relatorios/Relatorio.aspx?dummy=%27kLjxLb8B2OE%3d%27' 
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
		,@nomeModulo = 'Quantitativo de sugestões de currículos' -- Nome do módulo (Obrigatório)
		,@SiteMap1Nome = 'Quantitativo de sugestões de currículos'
		,@SiteMap1Url = '~/Relatorios/QuantitativoSugestoes/Busca.aspx'
		,@SiteMap2Nome = 'Quantitativo de sugestões de currículos' 
		,@SiteMap2Url = '~/Relatorios/Relatorio.aspx?dummy=%27ZFOrUN%2f6yDI%3d%27' 
		,@SiteMap3Nome = NULL 
		,@SiteMap3Url = NULL
		,@possuiVisaoAdm = 1 -- Indicar se possui visão de administador
		,@possuiVisaoGestao = 1 -- Indicar se possui visão de Gestão
		,@possuiVisaoUA = 1 -- Indicar se possui visão de UA
		,@possuiVisaoIndividual = 0 -- Indicar se possui visão de individual

-- Fechar transação
SET XACT_ABORT OFF
COMMIT TRANSACTION	
