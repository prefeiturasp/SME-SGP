USE [CoreSSO]
GO

--Iniciar transação
BEGIN TRANSACTION
SET XACT_ABORT ON

	DECLARE @nomeSistema VARCHAR(100) = ' SGP'
			
	EXEC MS_InserePaginaMenu
		@nomeSistema = @nomeSistema
		,@nomeModuloAvo = 'Configuração'
		,@nomeModuloPai = 'Dados gerais'
		,@nomeModulo = 'Tipo de justificativa para exclusão de aulas'
		,@SiteMap1Nome = 'Tipos de justificativa para exclusão de aulas'
		,@SiteMap1Url = '~/Configuracao/TipoJustificativaExclusaoAulas/Busca.aspx'
		,@SiteMap2Nome = 'Tipo de justificativa para exclusão de aulas'
		,@SiteMap2Url = '~/Configuracao/TipoJustificativaExclusaoAulas/Cadastro.aspx'		
		,@possuiVisaoAdm = 1
		,@possuiVisaoGestao = 0
		,@possuiVisaoUA = 0
		,@possuiVisaoIndividual = 0		
		
	EXEC MS_InserePaginaMenu
		@nomeSistema = @nomeSistema
		,@nomeModuloAvo = 'Documentos'
		,@nomeModuloPai = 'Documentos do gestor'
		,@nomeModulo = 'Aviso de aulas cadastradas sem plano de aula'
		,@SiteMap1Nome = 'Aviso de aulas cadastradas sem plano de aula'
		,@SiteMap1Url = '~/Relatorios/AulasSemPlanoAula/Busca.aspx'
		,@SiteMap2Nome = 'Aviso de aulas cadastradas sem plano de aula'
		,@SiteMap2Url = '~/Relatorios/Relatorio.aspx?dummy=%27wQrcpRF1gdI%3d%27'		
		,@possuiVisaoAdm = 1
		,@possuiVisaoGestao = 1
		,@possuiVisaoUA = 1
		,@possuiVisaoIndividual = 1
			
	EXEC MS_InserePaginaMenu
		@nomeSistema = @nomeSistema
		,@nomeModuloAvo = NULL
		,@nomeModuloPai = 'Registro de Classe'
		,@nomeModulo = 'Justificativas de faltas'
		,@SiteMap1Nome = 'Justificativas de faltas'
		,@SiteMap1Url = '~/Classe/JustificativaAbonoFalta/Busca.aspx'
		,@SiteMap2Nome = 'Justificativas de faltas'
		,@SiteMap2Url = '~/Classe/JustificativaAbonoFalta/Cadastro.aspx'		
		,@possuiVisaoAdm = 1
		,@possuiVisaoGestao = 1
		,@possuiVisaoUA = 1
		,@possuiVisaoIndividual = 1			
				

	EXEC MS_InserePaginaMenu
			@nomeSistema = @nomeSistema
			,@nomeModuloPai = 'Administração'
			,@nomeModulo = 'Abertura de anos letivos anteriores'
			,@SiteMap1Nome = 'Agendamento de abertura de anos letivos anteriores'
			,@SiteMap1Url = '~/Academico/AberturaTurmasAnosAnteriores/Busca.aspx'
			,@SiteMap2Nome = 'Agendamento de abertura de anos letivos anteriores'
			,@SiteMap2Url = '~/Academico/AberturaTurmasAnosAnteriores/Cadastro.aspx'		
			,@possuiVisaoAdm = 1
			,@possuiVisaoGestao = 0
			,@possuiVisaoUA = 0
			,@possuiVisaoIndividual = 0
				
	
-- Fechar transação
SET XACT_ABORT OFF
COMMIT TRANSACTION