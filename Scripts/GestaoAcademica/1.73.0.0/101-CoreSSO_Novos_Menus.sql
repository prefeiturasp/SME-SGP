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
	
-- Fechar transação
SET XACT_ABORT OFF
COMMIT TRANSACTION	
