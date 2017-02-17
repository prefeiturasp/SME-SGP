USE [CoreSSO]
GO

--Iniciar transação
BEGIN TRANSACTION
SET XACT_ABORT ON

	DECLARE @nomeSistema VARCHAR(100) = ' SGP'
	
	EXEC MS_InsereSiteMap
		@nomeSistema = @nomeSistema,
		@nomeModulo = 'Tipo de classificação de escola',
		@nomeModuloPai = 'Dados gerais',
		@nomeModuloAvo = 'Configuração',
		@SiteMapNome = 'Cargos para atribuição esporádica',
		@SiteMapUrl = '~/Configuracao/TipoClassificacaoEscola/Cargos.aspx'	
		
	
	
	EXEC MS_InserePaginaMenu
		@nomeSistema = @nomeSistema -- Nome do sistema (obrigatório)
		,@nomeModuloPai = 'Configuração' -- Nome do módulo pai (Opcional, apenas quando houver)
		,@nomeModulo = 'Segurança' -- Nome do módulo (Obrigatório)
		,@SiteMap1Nome = null
		,@SiteMap1Url = null
		,@SiteMap2Nome = null
		,@SiteMap2Url = null
		,@SiteMap3Nome = NULL 
		,@SiteMap3Url = NULL
		,@possuiVisaoAdm = 1 -- Indicar se possui visão de administador
		,@possuiVisaoGestao = 0 -- Indicar se possui visão de Gestão
		,@possuiVisaoUA = 0 -- Indicar se possui visão de UA
		,@possuiVisaoIndividual = 0 -- Indicar se possui visão de individual

	EXEC MS_InserePaginaMenu
		@nomeSistema = @nomeSistema -- Nome do sistema (obrigatório)
		,@nomeModuloAvo = 'Configuração' -- Nome do módulo avó (Opcional, apenas quando houver) 
		,@nomeModuloPai = 'Segurança' -- Nome do módulo pai (Opcional, apenas quando houver)
		,@nomeModulo = 'Permissões específicas' -- Nome do módulo (Obrigatório)
		,@SiteMap1Nome = 'Permissões específicas'
		,@SiteMap1Url = '~/Configuracao/PermissaoEspecifica/Cadastro.aspx'
		,@SiteMap2Nome = null
		,@SiteMap2Url = null
		,@SiteMap3Nome = NULL 
		,@SiteMap3Url = NULL
		,@possuiVisaoAdm = 1 -- Indicar se possui visão de administador
		,@possuiVisaoGestao = 0 -- Indicar se possui visão de Gestão
		,@possuiVisaoUA = 0 -- Indicar se possui visão de UA
		,@possuiVisaoIndividual = 0 -- Indicar se possui visão de individual

-- Fechar transação
SET XACT_ABORT OFF
COMMIT TRANSACTION	
