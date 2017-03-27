
	DECLARE @nomeSistema VARCHAR(100) = '$SystemName$'
		
	EXEC MS_InserePaginaMenu
		@nomeSistema = @nomeSistema -- Nome do sistema (obrigatório)
		,@nomeModuloAvo = 'Administração' -- Nome do módulo avó (Opcional, apenas quando houver) 
		,@nomeModuloPai = 'Cadastros' -- Nome do módulo pai (Opcional, apenas quando houver)
		,@nomeModulo = 'Objetos de aprendizagem' -- Nome do módulo (Obrigatório)
		,@SiteMap1Nome = 'Consulta de componentes curriculares'
		,@SiteMap1Url = '~/Academico/ObjetoAprendizagem/BuscaDisciplina.aspx'
		,@SiteMap2Nome = 'Consulta de objetos de aprendizagem'
		,@SiteMap2Url = '~/Academico/ObjetoAprendizagem/Busca.aspx'
		,@SiteMap3Nome = 'Cadastro de objetos de aprendizagem' 
		,@SiteMap3Url = '~/Academico/ObjetoAprendizagem/Cadastro.aspx'
		,@possuiVisaoAdm = 1 -- Indicar se possui visão de administador
		,@possuiVisaoGestao = 0 -- Indicar se possui visão de Gestão
		,@possuiVisaoUA = 0 -- Indicar se possui visão de UA
		,@possuiVisaoIndividual = 0 -- Indicar se possui visão de individual
