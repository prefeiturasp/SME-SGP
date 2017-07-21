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
		,@nomeModuloPai = 'Relatórios de atendimento' -- Nome do módulo pai (Opcional, apenas quando houver)
		,@nomeModulo = 'Ações realizadas' -- Nome do módulo (Obrigatório)
		,@SiteMap1Nome = 'Relatórios de ações realizadas'
		,@SiteMap1Url = '~/Relatorios/AcoesRealizadas/Busca.aspx'
		,@SiteMap2Nome = 'Relatórios de ações realizadas'
		,@SiteMap2Url = '~/Relatorios/Relatorio.aspx?dummy=%271VBlnYbVq7k%3d%27'
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
		,@possuiVisaoIndividual = 0 -- Indicar se possui visão de individual

	EXEC MS_InserePaginaMenu
		@nomeSistema = @nomeSistema -- Nome do sistema (obrigatório)
		,@nomeModuloAvo = NULL -- Nome do módulo avó (Opcional, apenas quando houver) 
		,@nomeModuloPai = 'Configurações' -- Nome do módulo pai (Opcional, apenas quando houver)
		,@nomeModulo = 'Alertas' -- Nome do módulo (Obrigatório)
		,@SiteMap1Nome = 'Listagem de alertas'
		,@SiteMap1Url = '~/Configuracao/Alertas/Busca.aspx'
		,@SiteMap2Nome = 'Cadastro de alertas'
		,@SiteMap2Url = '~/Configuracao/Alertas/Cadastro.aspx'
		,@possuiVisaoAdm = 1 -- Indicar se possui visão de administador
		,@possuiVisaoGestao = 1 -- Indicar se possui visão de Gestão
		,@possuiVisaoUA = 1 -- Indicar se possui visão de UA
		,@possuiVisaoIndividual = 0 -- Indicar se possui visão de individual

	EXEC MS_InserePaginaMenu
		@nomeSistema = @nomeSistema -- Nome do sistema (obrigatório)
		,@nomeModuloAvo = 'Relatórios' -- Nome do módulo avó (Opcional, apenas quando houver) 
		,@nomeModuloPai = 'Relatórios de atendimento' -- Nome do módulo pai (Opcional, apenas quando houver)
		,@nomeModulo = 'Gráficos de atendimento' -- Nome do módulo (Obrigatório)
		,@SiteMap1Nome = 'Gráficos de atendimento'
		,@SiteMap1Url = '~/Relatorios/GraficoAtendimento/Busca.aspx'
		,@SiteMap2Nome = 'Gráficos de atendimento'
		,@SiteMap2Url = '~/Relatorios/GraficoAtendimento/Relatorio.aspx'
		,@SiteMap3Nome = NULL 
		,@SiteMap3Url = NULL
		,@possuiVisaoAdm = 1 -- Indicar se possui visão de administador
		,@possuiVisaoGestao = 1 -- Indicar se possui visão de Gestão
		,@possuiVisaoUA = 1 -- Indicar se possui visão de UA
		,@possuiVisaoIndividual = 0 -- Indicar se possui visão de individual

	EXEC MS_InserePaginaMenu
		@nomeSistema = @nomeSistema -- Nome do sistema (obrigatório)
		,@nomeModuloAvo = NULL -- Nome do módulo avó (Opcional, apenas quando houver) 
		,@nomeModuloPai = 'Configurações' -- Nome do módulo pai (Opcional, apenas quando houver)
		,@nomeModulo = 'Gráficos de atendimento' -- Nome do módulo (Obrigatório)
		,@SiteMap1Nome = 'Listagem de gráficos de atendimento'
		,@SiteMap1Url = '~/Configuracao/GraficoAtendimento/Busca.aspx'
		,@SiteMap2Nome = 'Cadastro de gráficos de atendimento'
		,@SiteMap2Url = '~/Configuracao/GraficoAtendimento/Cadastro.aspx'
		,@SiteMap3Nome = NULL 
		,@SiteMap3Url = NULL
		,@possuiVisaoAdm = 1 -- Indicar se possui visão de administador
		,@possuiVisaoGestao = 1 -- Indicar se possui visão de Gestão
		,@possuiVisaoUA = 1 -- Indicar se possui visão de UA
		,@possuiVisaoIndividual = 0 -- Indicar se possui visão de individual
				
	-- ALTERAÇÃO DE MENU E NOME 
	DECLARE @mod_idRegistroClasse int
				set @mod_idRegistroClasse = (select mod_id from SYS_Modulo where mod_nome = 'Registro de classe' and sis_id = 102)
	UPDATE SYS_Modulo
		SET mod_nome = 'Registro de sugestões de currículo' , mod_idPai = @mod_idRegistroClasse , mod_dataAlteracao = GETDATE()
		WHERE mod_nome = 'Registro de sugestões' and sis_id = 102
		
	-- ALTERAÇÃO DE MENU
	DECLARE @mod_idConfiguracoes int
				set @mod_idConfiguracoes = (select mod_id from SYS_Modulo where mod_nome = 'Configurações' and sis_id = 102)
	UPDATE SYS_Modulo
		SET mod_idPai = @mod_idConfiguracoes , mod_dataAlteracao = GETDATE()
		WHERE mod_nome = 'Cadastro de currículo' and sis_id = 102
	
	-- EXCLUSÃO DO MENU "CURRÍCULO"
	EXEC MS_RemovePaginaMenu
		@nomeSistema = @nomeSistema 
		,@NomeModulo = 'Currículo'
		,@nomeModuloPai = 'Administração'
		
	EXEC MS_InserePaginaMenu
		@nomeSistema = @nomeSistema -- Nome do sistema (obrigatório)
		,@nomeModuloAvo = NULL  -- Nome do módulo avó (Opcional, apenas quando houver) 
		,@nomeModuloPai = 'Relatórios' -- Nome do módulo pai (Opcional, apenas quando houver)
		,@nomeModulo = 'Gerais' -- Nome do módulo (Obrigatório)
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
		
	-- ALTERAÇÕES MENU 
	DECLARE @mod_idGerais int
				set @mod_idGerais = (select mod_id from SYS_Modulo where mod_nome = 'Gerais' and sis_id = 102)
	DECLARE @mod_idPaiAntigogestor int
				set @mod_idPaiAntigogestor = (select mod_id from SYS_Modulo where mod_nome = 'Gestor' and sis_id = 102)

	UPDATE SYS_Modulo
		SET mod_idPai = @mod_idGerais , mod_dataAlteracao = GETDATE()
		WHERE mod_nome = 'Consulta da versão do aplicativo SGP tablet' and sis_id = 102 and mod_idPai = @mod_idPaiAntigogestor

	UPDATE SYS_Modulo
		SET mod_idPai = @mod_idGerais , mod_dataAlteracao = GETDATE()
		WHERE mod_nome = 'Divergências das rematrículas' and sis_id = 102 and mod_idPai = @mod_idPaiAntigogestor

	UPDATE SYS_Modulo
		SET mod_idPai = @mod_idGerais , mod_dataAlteracao = GETDATE()
		WHERE mod_nome = 'Objetos de conhecimento' and sis_id = 102 and mod_idPai = @mod_idPaiAntigogestor

	UPDATE SYS_Modulo
		SET mod_idPai = @mod_idGerais , mod_dataAlteracao = GETDATE()
		WHERE mod_nome = 'Análise de sondagem' and sis_id = 102 and mod_idPai = @mod_idPaiAntigogestor

	UPDATE SYS_Modulo
		SET mod_idPai = @mod_idGerais , mod_dataAlteracao = GETDATE()
		WHERE mod_nome = 'Sugestões de currículos' and sis_id = 102 and mod_idPai = @mod_idPaiAntigogestor
	
	UPDATE SYS_Modulo
		SET mod_idPai = @mod_idGerais , mod_dataAlteracao = GETDATE()
		WHERE mod_nome = 'Quantitativo de sugestões de currículos' and sis_id = 102 and mod_idPai = @mod_idPaiAntigogestor

-- Fechar transação
SET XACT_ABORT OFF
COMMIT TRANSACTION	
