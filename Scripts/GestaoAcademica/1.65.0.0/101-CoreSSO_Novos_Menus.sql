USE [CoreSSO]
GO

--Iniciar transação
BEGIN TRANSACTION
SET XACT_ABORT ON

	DECLARE @nomeSistema VARCHAR(100) = ' SGP'

	EXEC MS_InserePaginaMenu
		@nomeSistema = @nomeSistema -- Nome do sistema (obrigatório)
		,@nomeModuloAvo = 'Documentos' -- Nome do módulo avó (Opcional, apenas quando houver) 
		,@nomeModuloPai = 'Documentos do gestor' -- Nome do módulo pai (Opcional, apenas quando houver)
		,@nomeModulo = 'Divergências das rematrículas' -- Nome do módulo (Obrigatório)
		,@SiteMap1Nome = 'Divergências das rematrículas'
		,@SiteMap1Url = '~/Relatorios/DivergenciasRematriculas/Busca.aspx'
		,@SiteMap2Nome = 'Divergências das rematrículas'
		,@SiteMap2Url = '~/Relatorios/Relatorio.aspx?dummy=%27f58oZhLbi2E%3d%27'
		,@SiteMap3Nome = NULL 
		,@SiteMap3Url = NULL
		,@possuiVisaoAdm = 1 -- Indicar se possui visão de administador
		,@possuiVisaoGestao = 1 -- Indicar se possui visão de Gestão
		,@possuiVisaoUA = 1 -- Indicar se possui visão de UA
		,@possuiVisaoIndividual = 0 -- Indicar se possui visão de individual
		
-- Fechar transação
SET XACT_ABORT OFF
COMMIT TRANSACTION	
