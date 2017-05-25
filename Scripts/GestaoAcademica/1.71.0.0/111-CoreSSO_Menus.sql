USE [CoreSSO]
GO

BEGIN TRANSACTION 
SET XACT_ABORT ON   

	DECLARE @sis_id INT = 102
	DECLARE @visaoAdm INT = 1
	DECLARE @visaoGestao INT = 2
	DECLARE @visaoUnidade INT = 3
	DECLARE @nomeSistema VARCHAR(100) = ' SGP'

	UPDATE SYS_Modulo
	SET mod_nome = 'Sistemas'
	WHERE mod_nome = 'Configuração' AND sis_id = 102 AND mod_situacao <> 3
	
	UPDATE SYS_Modulo
	SET mod_nome = 'Calendário'
	WHERE mod_nome = 'Calendário escolar' AND sis_id = 102 AND mod_situacao <> 3
	
	UPDATE SYS_Modulo
	SET mod_nome = 'Turmas normais'
	WHERE mod_nome = 'Manutenção de turmas' AND sis_id = 102 AND mod_situacao <> 3
	
	UPDATE SYS_Modulo
	SET mod_nome = 'Turmas eletivas'
	WHERE mod_nome = 'Manutenção de turmas eletivas' AND sis_id = 102 AND mod_situacao <> 3
	
	UPDATE SYS_Modulo
	SET mod_nome = 'Turmas multisseriadas'
	WHERE mod_nome = 'Manutenção de turmas multisseriadas' AND sis_id = 102 AND mod_situacao <> 3
	
	EXEC MS_InserePaginaMenu
		@nomeSistema = @nomeSistema -- Nome do sistema (obrigatório)
		,@nomeModuloAvo = NULL -- Nome do módulo avó (Opcional, apenas quando houver) 
		,@nomeModuloPai = NULL -- Nome do módulo pai (Opcional, apenas quando houver)
		,@nomeModulo = 'Configurações' -- Nome do módulo (Obrigatório)
		,@SiteMap1Nome = 'Configurações'
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
		,@nomeModuloAvo = NULL -- Nome do módulo avó (Opcional, apenas quando houver) 
		,@nomeModuloPai = 'Administração' -- Nome do módulo pai (Opcional, apenas quando houver)
		,@nomeModulo = 'Calendário escolar' -- Nome do módulo (Obrigatório)
		,@SiteMap1Nome = 'Calendário escolar'
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
		,@nomeModuloAvo = NULL -- Nome do módulo avó (Opcional, apenas quando houver) 
		,@nomeModuloPai = 'Administração' -- Nome do módulo pai (Opcional, apenas quando houver)
		,@nomeModulo = 'Manutenção de turmas' -- Nome do módulo (Obrigatório)
		,@SiteMap1Nome = 'Manutenção de turmas'
		,@SiteMap1Url = NULL
		,@SiteMap2Nome = NULL
		,@SiteMap2Url = NULL
		,@SiteMap3Nome = NULL
		,@SiteMap3Url = NULL
		,@possuiVisaoAdm = 1 -- Indicar se possui visão de administador
		,@possuiVisaoGestao = 1 -- Indicar se possui visão de Gestão
		,@possuiVisaoUA = 1 -- Indicar se possui visão de UA
		,@possuiVisaoIndividual = 1 -- Indicar se possui visão de individual

	
	-- Mudando módulos de Configurações para o mod_idPai novo

	DECLARE @mod_idPai INT = (SELECT mod_id FROM SYS_Modulo WHERE mod_nome = 'Configurações' AND sis_id = @sis_id AND mod_situacao = 1)
	DECLARE @mod_idCadastros INT = (SELECT mod_id FROM SYS_Modulo WHERE mod_nome = 'Cadastros' AND sis_id = @sis_id AND mod_situacao = 1)

	UPDATE SYS_Modulo 
	SET mod_idPai = @mod_idPai
	WHERE sis_id = @sis_id AND mod_nome = 'Formato de avaliação'

	UPDATE SYS_Modulo 
	SET mod_idPai = @mod_idPai
	WHERE sis_id = @sis_id AND mod_nome = 'Escala de avaliação'

	UPDATE SYS_Modulo 
	SET mod_idPai = @mod_idPai
	WHERE sis_id = @sis_id AND mod_nome = 'Turnos'
	
	UPDATE SYS_Modulo 
	SET mod_idPai = @mod_idPai
	WHERE sis_id = @sis_id AND mod_nome = 'Cursos'
	
	UPDATE SYS_Modulo 
	SET mod_idPai = @mod_idPai
	WHERE sis_id = @sis_id AND mod_nome = 'Orientações curriculares'
	
	UPDATE SYS_Modulo 
	SET mod_idPai = @mod_idPai
	WHERE sis_id = @sis_id AND mod_nome = 'Reuniões de responsáveis'
	
	UPDATE SYS_Modulo 
	SET mod_idPai = @mod_idPai
	WHERE sis_id = @sis_id AND mod_nome = 'Nível de aprendizado'
	
	UPDATE SYS_Modulo 
	SET mod_idPai = @mod_idPai
	WHERE sis_id = @sis_id AND mod_nome = 'Matriz de habilidades'
	
	UPDATE SYS_Modulo 
	SET mod_idPai = @mod_idPai
	WHERE sis_id = @sis_id AND mod_nome = 'Objetos de conhecimento'
	
	UPDATE SYS_Modulo 
	SET mod_idPai = @mod_idPai
	WHERE sis_id = @sis_id AND mod_nome = 'Sondagem' AND mod_idPai = @mod_idCadastros
	
	UPDATE SYS_Modulo 
	SET mod_idPai = @mod_idPai
	WHERE sis_id = @sis_id AND mod_nome = 'Configuração do serviço de pendência'
	
	UPDATE SYS_Modulo
	SET mod_situacao = 3
	WHERE sis_id = @sis_id AND mod_nome = 'Cadastros'
	
	INSERT INTO SYS_GrupoPermissao 
	SELECT gru_id, mdl.sis_id, @mod_idPai, 1, 0, 0, 0
	FROM SYS_Modulo mdl WITH(NOLOCK)
	INNER JOIN SYS_GrupoPermissao gpm WITH(NOLOCK)
		ON mdl.sis_id = gpm.sis_id
		AND mdl.mod_id = gpm.mod_id
	WHERE
		mdl.mod_idPai = @mod_idPai
		AND mdl.sis_id = @sis_id
		AND grp_consultar = 1
		AND NOT EXISTS(SELECT TOP 1 g.gru_id FROM SYS_GrupoPermissao g WITH(NOLOCK)
					   WHERE g.gru_id = gpm.gru_id AND g.sis_id = mdl.sis_id AND g.mod_id = @mod_idPai)
		AND mdl.mod_situacao <> 3
	GROUP BY
		gru_id, mdl.sis_id
	
	UPDATE gpm 
	SET grp_consultar = 1
	FROM SYS_GrupoPermissao gpm WITH(NOLOCK)
	INNER JOIN SYS_Modulo mdl WITH(NOLOCK)
		ON mdl.sis_id = gpm.sis_id
		AND mdl.mod_idPai = gpm.mod_id
		AND mdl.mod_situacao <> 3
	INNER JOIN SYS_GrupoPermissao gpmF WITH(NOLOCK)
		ON mdl.sis_id = gpmF.sis_id
		AND mdl.mod_id = gpmF.mod_id
		AND gpm.gru_id = gpmF.gru_id
	WHERE
		gpm.mod_id = @mod_idPai AND gpm.sis_id = @sis_id AND
		(gpmF.grp_consultar = 1 OR gpmF.grp_alterar = 1 OR gpmF.grp_excluir = 1 OR gpmF.grp_inserir = 1)
		
	-- Mudando módulos de Calendário para o mod_idPai novo

	SET @mod_idPai = (SELECT mod_id FROM SYS_Modulo WHERE mod_nome = 'Calendário escolar' AND sis_id = @sis_id AND mod_situacao = 1)

	UPDATE SYS_Modulo 
	SET mod_idPai = @mod_idPai
	WHERE sis_id = @sis_id AND mod_nome = 'Calendário'

	UPDATE SYS_Modulo 
	SET mod_idPai = @mod_idPai
	WHERE sis_id = @sis_id AND mod_nome = 'Eventos do calendário escolar'

	UPDATE SYS_Modulo 
	SET mod_idPai = @mod_idPai
	WHERE sis_id = @sis_id AND mod_nome = 'Abertura de anos letivos anteriores'
	
	-- Organizando a ordem dos módulos 
	
	UPDATE SYS_VisaoModuloMenu
	SET vmm_ordem = 2
	WHERE sis_id = @sis_id
	AND vis_id = @visaoAdm
	AND mod_id = @mod_idPai

	UPDATE SYS_VisaoModuloMenu
	SET vmm_ordem = 2
	WHERE sis_id = @sis_id
	AND vis_id = @visaoGestao
	AND mod_id = @mod_idPai

	UPDATE SYS_VisaoModuloMenu
	SET vmm_ordem = 4
	WHERE sis_id = @sis_id
	AND vis_id = @visaoUnidade
	AND mod_id = @mod_idPai
	
	INSERT INTO SYS_GrupoPermissao 
	SELECT gru_id, mdl.sis_id, @mod_idPai, 1, 0, 0, 0
	FROM SYS_Modulo mdl WITH(NOLOCK)
	INNER JOIN SYS_GrupoPermissao gpm WITH(NOLOCK)
		ON mdl.sis_id = gpm.sis_id
		AND mdl.mod_id = gpm.mod_id
	WHERE
		mdl.mod_idPai = @mod_idPai
		AND mdl.sis_id = @sis_id
		AND grp_consultar = 1
		AND NOT EXISTS(SELECT TOP 1 g.gru_id FROM SYS_GrupoPermissao g WITH(NOLOCK)
					   WHERE g.gru_id = gpm.gru_id AND g.sis_id = mdl.sis_id AND g.mod_id = @mod_idPai)
		AND mdl.mod_situacao <> 3
	GROUP BY
		gru_id, mdl.sis_id
	
	UPDATE gpm 
	SET grp_consultar = 1
	FROM SYS_GrupoPermissao gpm WITH(NOLOCK)
	INNER JOIN SYS_Modulo mdl WITH(NOLOCK)
		ON mdl.sis_id = gpm.sis_id
		AND mdl.mod_idPai = gpm.mod_id
		AND mdl.mod_situacao <> 3
	INNER JOIN SYS_GrupoPermissao gpmF WITH(NOLOCK)
		ON mdl.sis_id = gpmF.sis_id
		AND mdl.mod_id = gpmF.mod_id
		AND gpm.gru_id = gpmF.gru_id
	WHERE
		gpm.mod_id = @mod_idPai AND gpm.sis_id = @sis_id AND
		(gpmF.grp_consultar = 1 OR gpmF.grp_alterar = 1 OR gpmF.grp_excluir = 1 OR gpmF.grp_inserir = 1)
		
	-- Mudando módulos de Manutenção de turmas para o mod_idPai novo

	SET @mod_idPai = (SELECT mod_id FROM SYS_Modulo WHERE mod_nome = 'Manutenção de turmas' AND sis_id = @sis_id AND mod_situacao = 1)

	UPDATE SYS_Modulo 
	SET mod_idPai = @mod_idPai
	WHERE sis_id = @sis_id AND mod_nome = 'Turmas normais'

	UPDATE SYS_Modulo 
	SET mod_idPai = @mod_idPai
	WHERE sis_id = @sis_id AND mod_nome = 'Turmas eletivas'

	UPDATE SYS_Modulo 
	SET mod_idPai = @mod_idPai
	WHERE sis_id = @sis_id AND mod_nome = 'Turmas multisseriadas'
	
	-- Organizando a ordem dos módulos 

	DECLARE @vmm_ordem INT
	
	SELECT @vmm_ordem = MAX(vmm_ordem)
	FROM SYS_Modulo mdl WITH(NOLOCK)
	INNER JOIN SYS_Modulo mdf WITH(NOLOCK)
		ON mdl.sis_id = mdf.sis_id
		AND mdl.mod_id = mdf.mod_idPai
		AND mdf.mod_situacao <> 3
	INNER JOIN SYS_VisaoModuloMenu vmm WITH(NOLOCK)
		ON mdf.sis_id = vmm.sis_id
		AND mdf.mod_id = vmm.mod_id
		AND vis_id = @visaoAdm
	WHERE mdl.sis_id = @sis_id AND mdl.mod_nome = 'Administração' AND mdl.mod_situacao <> 3
	
	UPDATE SYS_VisaoModuloMenu
	SET vmm_ordem = ISNULL(@vmm_ordem + 1, 1)
	WHERE sis_id = @sis_id
	AND vis_id = @visaoAdm
	AND mod_id = @mod_idPai

	SELECT @vmm_ordem = MAX(vmm_ordem)
	FROM SYS_Modulo mdl WITH(NOLOCK)
	INNER JOIN SYS_Modulo mdf WITH(NOLOCK)
		ON mdl.sis_id = mdf.sis_id
		AND mdl.mod_id = mdf.mod_idPai
		AND mdf.mod_situacao <> 3
	INNER JOIN SYS_VisaoModuloMenu vmm WITH(NOLOCK)
		ON mdf.sis_id = vmm.sis_id
		AND mdf.mod_id = vmm.mod_id
		AND vis_id = @visaoGestao
	WHERE mdl.sis_id = @sis_id AND mdl.mod_nome = 'Administração' AND mdl.mod_situacao <> 3
	
	UPDATE SYS_VisaoModuloMenu
	SET vmm_ordem = ISNULL(@vmm_ordem + 1, 1)
	WHERE sis_id = @sis_id
	AND vis_id = @visaoGestao
	AND mod_id = @mod_idPai

	SELECT @vmm_ordem = MAX(vmm_ordem)
	FROM SYS_Modulo mdl WITH(NOLOCK)
	INNER JOIN SYS_Modulo mdf WITH(NOLOCK)
		ON mdl.sis_id = mdf.sis_id
		AND mdl.mod_id = mdf.mod_idPai
		AND mdf.mod_situacao <> 3
	INNER JOIN SYS_VisaoModuloMenu vmm WITH(NOLOCK)
		ON mdf.sis_id = vmm.sis_id
		AND mdf.mod_id = vmm.mod_id
		AND vis_id = @visaoUnidade
	WHERE mdl.sis_id = @sis_id AND mdl.mod_nome = 'Administração' AND mdl.mod_situacao <> 3
	
	UPDATE SYS_VisaoModuloMenu
	SET vmm_ordem = ISNULL(@vmm_ordem + 1, 1)
	WHERE sis_id = @sis_id
	AND vis_id = @visaoUnidade
	AND mod_id = @mod_idPai
	
	INSERT INTO SYS_GrupoPermissao 
	SELECT gru_id, mdl.sis_id, @mod_idPai, 1, 0, 0, 0
	FROM SYS_Modulo mdl WITH(NOLOCK)
	INNER JOIN SYS_GrupoPermissao gpm WITH(NOLOCK)
		ON mdl.sis_id = gpm.sis_id
		AND mdl.mod_id = gpm.mod_id
	WHERE
		mdl.mod_idPai = @mod_idPai
		AND mdl.sis_id = @sis_id
		AND grp_consultar = 1
		AND NOT EXISTS(SELECT TOP 1 g.gru_id FROM SYS_GrupoPermissao g WITH(NOLOCK)
					   WHERE g.gru_id = gpm.gru_id AND g.sis_id = mdl.sis_id AND g.mod_id = @mod_idPai)
		AND mdl.mod_situacao <> 3
	GROUP BY
		gru_id, mdl.sis_id
	
	UPDATE gpm 
	SET grp_consultar = 1
	FROM SYS_GrupoPermissao gpm WITH(NOLOCK)
	INNER JOIN SYS_Modulo mdl WITH(NOLOCK)
		ON mdl.sis_id = gpm.sis_id
		AND mdl.mod_idPai = gpm.mod_id
		AND mdl.mod_situacao <> 3
	INNER JOIN SYS_GrupoPermissao gpmF WITH(NOLOCK)
		ON mdl.sis_id = gpmF.sis_id
		AND mdl.mod_id = gpmF.mod_id
		AND gpm.gru_id = gpmF.gru_id
	WHERE
		gpm.mod_id = @mod_idPai AND gpm.sis_id = @sis_id AND
		(gpmF.grp_consultar = 1 OR gpmF.grp_alterar = 1 OR gpmF.grp_excluir = 1 OR gpmF.grp_inserir = 1)
		
-- Fechar transação     
SET XACT_ABORT OFF 
COMMIT TRANSACTION