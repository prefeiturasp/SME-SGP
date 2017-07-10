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
		,@nomeModulo = 'Questionários' -- Nome do módulo (Obrigatório)
		,@SiteMap1Nome = 'Listagem de questionários'
		,@SiteMap1Url = '~/Configuracao/Questionario/BuscaQuestionario.aspx'
		,@SiteMap2Nome = 'Cadastro de questionários'
		,@SiteMap2Url = '~/Configuracao/Questionario/CadastroQuestionario.aspx'
		,@SiteMap3Nome = NULL 
		,@SiteMap3Url = NULL
		,@possuiVisaoAdm = 1 -- Indicar se possui visão de administador
		,@possuiVisaoGestao = 0 -- Indicar se possui visão de Gestão
		,@possuiVisaoUA = 0 -- Indicar se possui visão de UA
		,@possuiVisaoIndividual = 0 -- Indicar se possui visão de individual

	EXEC MS_InserePaginaMenu
		@nomeSistema = @nomeSistema -- Nome do sistema (obrigatório)
		,@nomeModuloAvo = NULL -- Nome do módulo avó (Opcional, apenas quando houver) 
		,@nomeModuloPai = 'Configurações' -- Nome do módulo pai (Opcional, apenas quando houver)
		,@nomeModulo = 'Relatórios de atendimento' -- Nome do módulo (Obrigatório)
		,@SiteMap1Nome = 'Listagem de relatórios de atendimento'
		,@SiteMap1Url = '~/Configuracao/RelatorioAtendimento/Busca.aspx'
		,@SiteMap2Nome = 'Cadastro de relatórios de atendimento'
		,@SiteMap2Url = '~/Configuracao/RelatorioAtendimento/Cadastro.aspx'
		,@SiteMap3Nome = NULL 
		,@SiteMap3Url = NULL
		,@possuiVisaoAdm = 1 -- Indicar se possui visão de administador
		,@possuiVisaoGestao = 0 -- Indicar se possui visão de Gestão
		,@possuiVisaoUA = 0 -- Indicar se possui visão de UA
		,@possuiVisaoIndividual = 0 -- Indicar se possui visão de individual

	EXEC MS_InserePaginaMenu
		@nomeSistema = @nomeSistema -- Nome do sistema (obrigatório)
		,@nomeModuloAvo = NULL -- Nome do módulo avó (Opcional, apenas quando houver) 
		,@nomeModuloPai = 'Configurações' -- Nome do módulo pai (Opcional, apenas quando houver)
		,@nomeModulo = 'Carga horária de atividades extraclasse' -- Nome do módulo (Obrigatório)
		,@SiteMap1Nome = 'Carga horária de atividades extraclasse'
		,@SiteMap1Url = '~/Academico/CargaHorariaExtraclasse/Cadastro.aspx'
		,@possuiVisaoAdm = 1 -- Indicar se possui visão de administador
		,@possuiVisaoGestao = 0 -- Indicar se possui visão de Gestão
		,@possuiVisaoUA = 0 -- Indicar se possui visão de UA
		,@possuiVisaoIndividual = 0 -- Indicar se possui visão de individual

	
	EXEC MS_InserePaginaMenu
		@nomeSistema = @nomeSistema -- Nome do sistema (obrigatório)
		,@nomeModuloAvo = NULL -- Nome do módulo avó (Opcional, apenas quando houver) 
		,@nomeModuloPai = 'Registro de Classe' -- Nome do módulo pai (Opcional, apenas quando houver)
		,@nomeModulo = 'Relatório de Atendimento Educacional Especializado' -- Nome do módulo (Obrigatório)
		,@SiteMap1Nome = 'Consulta de relatórios de AEE'
		,@SiteMap1Url = '~/Classe/RelatorioAtendimento/Busca.aspx'
		,@SiteMap2Nome = 'Lançamento de relatórios de AEE'
		,@SiteMap2Url = '~/Classe/RelatorioAtendimento/Cadastro.aspx'
		,@possuiVisaoAdm = 1 -- Indicar se possui visão de administador
		,@possuiVisaoGestao = 1 -- Indicar se possui visão de Gestão
		,@possuiVisaoUA = 1 -- Indicar se possui visão de UA
		,@possuiVisaoIndividual = 1 -- Indicar se possui visão de individual

	EXEC MS_InserePaginaMenu
		@nomeSistema = @nomeSistema -- Nome do sistema (obrigatório)
		,@nomeModuloAvo = NULL -- Nome do módulo avó (Opcional, apenas quando houver) 
		,@nomeModuloPai = 'Configurações' -- Nome do módulo pai (Opcional, apenas quando houver)
		,@nomeModulo = 'Detalhamento das deficiências' -- Nome do módulo (Obrigatório)
		,@SiteMap1Nome = 'Listagem do detalhamento das deficiências'
		,@SiteMap1Url = '~/Configuracao/DeficienciaDetalhe/Busca.aspx'
		,@SiteMap2Nome = 'Cadastro do detalhamento das deficiências'
		,@SiteMap2Url = '~/Configuracao/DeficienciaDetalhe/Cadastro.aspx'
		,@SiteMap3Nome = NULL 
		,@SiteMap3Url = NULL
		,@possuiVisaoAdm = 1 -- Indicar se possui visão de administador
		,@possuiVisaoGestao = 0 -- Indicar se possui visão de Gestão
		,@possuiVisaoUA = 0 -- Indicar se possui visão de UA
		,@possuiVisaoIndividual = 0 -- Indicar se possui visão de individual
-- Fechar transação
SET XACT_ABORT OFF
COMMIT TRANSACTION	
