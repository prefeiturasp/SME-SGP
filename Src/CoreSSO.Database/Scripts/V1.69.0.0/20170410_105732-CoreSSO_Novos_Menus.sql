
	DECLARE @nomeSistema VARCHAR(100) = '$SystemName$'
	
	EXEC MS_InserePaginaMenu
		@nomeSistema = @nomeSistema -- Nome do sistema (obrigatório)
		,@nomeModuloAvo = 'Relatórios' -- Nome do módulo avó (Opcional, apenas quando houver) 
		,@nomeModuloPai = 'Gestor' -- Nome do módulo pai (Opcional, apenas quando houver)
		,@nomeModulo = 'Objetos de aprendizagem' -- Nome do módulo (Obrigatório)
		,@SiteMap1Nome = 'Objetos de aprendizagem'
		,@SiteMap1Url = '~/Relatorios/ObjetoAprendizagem/Busca.aspx'
		,@SiteMap2Nome = 'Objetos de aprendizagem'
		,@SiteMap2Url = '~/Relatorios/Relatorio.aspx?dummy=%27Qz5y1nPPR5g%3d%27'
		,@SiteMap3Nome = NULL 
		,@SiteMap3Url = NULL
		,@possuiVisaoAdm = 1 -- Indicar se possui visão de administador
		,@possuiVisaoGestao = 1 -- Indicar se possui visão de Gestão
		,@possuiVisaoUA = 0 -- Indicar se possui visão de UA
		,@possuiVisaoIndividual = 0 -- Indicar se possui visão de individual
	
	EXEC MS_InserePaginaMenu
		@nomeSistema = @nomeSistema -- Nome do sistema (obrigatório)
		,@nomeModuloAvo = 'Relatórios' -- Nome do módulo avó (Opcional, apenas quando houver) 
		,@nomeModuloPai = 'Gestor' -- Nome do módulo pai (Opcional, apenas quando houver)
		,@nomeModulo = 'Justificativas de falta' -- Nome do módulo (Obrigatório)
		,@SiteMap1Nome = 'Justificativas de falta'
		,@SiteMap1Url = '~/Relatorios/RelatoriosCP/DadosAlunosJustificativaFalta/Busca.aspx'
		,@SiteMap2Nome = 'Justificativas de falta'
		,@SiteMap2Url = '~/Relatorios/Relatorio.aspx?dummy=%27LtW9Jo%2bWRTc%3d%27'
		,@SiteMap3Nome = NULL 
		,@SiteMap3Url = NULL
		,@possuiVisaoAdm = 1 -- Indicar se possui visão de administador
		,@possuiVisaoGestao = 1 -- Indicar se possui visão de Gestão
		,@possuiVisaoUA = 0 -- Indicar se possui visão de UA
		,@possuiVisaoIndividual = 0 -- Indicar se possui visão de individual

	EXEC MS_InserePaginaMenu
		@nomeSistema = @nomeSistema -- Nome do sistema (obrigatório)
		,@nomeModuloAvo = 'Relatórios' -- Nome do módulo avó (Opcional, apenas quando houver) 
		,@nomeModuloPai = 'Gestor' -- Nome do módulo pai (Opcional, apenas quando houver)
		,@nomeModulo = 'Divergências entre aulas criadas e previstas' -- Nome do módulo (Obrigatório)
		,@SiteMap1Nome = 'Divergências entre aulas criadas e previstas'
		,@SiteMap1Url = '~/Relatorios/DivergenciasAulasPrevistas/Busca.aspx'
		,@SiteMap2Nome = 'Divergências entre aulas criadas e previstas'
		,@SiteMap2Url = '~/Relatorios/Relatorio.aspx?dummy=%27ewQYQI4%2fS98%3d%27'
		,@SiteMap3Nome = NULL 
		,@SiteMap3Url = NULL
		,@possuiVisaoAdm = 1 -- Indicar se possui visão de administador
		,@possuiVisaoGestao = 1 -- Indicar se possui visão de Gestão
		,@possuiVisaoUA = 1 -- Indicar se possui visão de UA
		,@possuiVisaoIndividual = 0 -- Indicar se possui visão de individual

	EXEC MS_InserePaginaMenu
		@nomeSistema = @nomeSistema -- Nome do sistema (obrigatório)
		,@nomeModuloAvo = NULL -- Nome do módulo avó (Opcional, apenas quando houver) 
		,@nomeModuloPai = 'Registro de classe' -- Nome do módulo pai (Opcional, apenas quando houver)
		,@nomeModulo = 'Planejamento semanal' -- Nome do módulo (Obrigatório)
		,@SiteMap1Nome = 'Consulta de planejamento semanal'
		,@SiteMap1Url = '~/Academico/ControleSemanal/Busca.aspx'
		,@SiteMap2Nome = 'Cadastro de planejamento semanal' 
		,@SiteMap2Url = '~/Academico/ControleSemanal/Cadastro.aspx'
		,@SiteMap3Nome = NULL 
		,@SiteMap3Url = NULL
		,@possuiVisaoAdm = 1 -- Indicar se possui visão de administador
		,@possuiVisaoGestao = 1 -- Indicar se possui visão de Gestão
		,@possuiVisaoUA = 1 -- Indicar se possui visão de UA
		,@possuiVisaoIndividual = 1 -- Indicar se possui visão de individual
