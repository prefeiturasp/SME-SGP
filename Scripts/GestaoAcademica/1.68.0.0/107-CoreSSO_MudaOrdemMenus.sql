USE [DES_SPO_CoreSSO]
GO

DECLARE @sis_id INT = 102
DECLARE @visaoDocente INT = 4
DECLARE @visaoDiretor INT = 3

-- DOCENTE

-- ADMINISTRAÇÃO

DECLARE @mod_idPai INT = (SELECT mod_id FROM [SYS_Modulo] WHERE mod_nome = 'Administração' and sis_id = @sis_id and mod_situacao = 1)

	UPDATE VMM
	SET vmm_ordem = 1
	FROM [SYS_VisaoModuloMenu] VMM
	INNER JOIN [SYS_Modulo] m
	ON m.mod_id = VMM.mod_id
	AND m.sis_id = VMM.sis_id
	WHERE VMM.sis_id = @sis_id
	and vis_id = @visaoDocente
	and m.mod_idPai = @mod_idPai
	and m.mod_nome = 'Calendário escolar'

	UPDATE VMM
	SET vmm_ordem = 2
	FROM [SYS_VisaoModuloMenu] VMM
	INNER JOIN [SYS_Modulo] m
	ON m.mod_id = VMM.mod_id
	AND m.sis_id = VMM.sis_id
	WHERE VMM.sis_id = @sis_id
	and vis_id = @visaoDocente
	and m.mod_idPai = @mod_idPai
	and m.mod_nome = 'Alunos'

-- REGISTRO DE CLASSE

SET @mod_idPai = (SELECT mod_id FROM [SYS_Modulo] WHERE mod_nome = 'Registro de classe' and sis_id = @sis_id and mod_situacao = 1)

	UPDATE VMM
	SET vmm_ordem = 1
	FROM [SYS_VisaoModuloMenu] VMM
	INNER JOIN [SYS_Modulo] m
	ON m.mod_id = VMM.mod_id
	AND m.sis_id = VMM.sis_id
	WHERE VMM.sis_id = @sis_id
	and vis_id = @visaoDocente
	and m.mod_idPai = @mod_idPai
	and m.mod_nome = 'Minhas turmas'

	UPDATE VMM
	SET vmm_ordem = 2
	FROM [SYS_VisaoModuloMenu] VMM
	INNER JOIN [SYS_Modulo] m
	ON m.mod_id = VMM.mod_id
	AND m.sis_id = VMM.sis_id
	WHERE VMM.sis_id = @sis_id
	and vis_id = @visaoDocente
	and m.mod_idPai = @mod_idPai
	and m.mod_nome = 'Atribuição de docentes'

	UPDATE VMM
	SET vmm_ordem = 3
	FROM [SYS_VisaoModuloMenu] VMM
	INNER JOIN [SYS_Modulo] m
	ON m.mod_id = VMM.mod_id
	AND m.sis_id = VMM.sis_id
	WHERE VMM.sis_id = @sis_id
	and vis_id = @visaoDocente
	and m.mod_idPai = @mod_idPai
	and m.mod_nome = 'Compensação de ausência'

	UPDATE VMM
	SET vmm_ordem = 4
	FROM [SYS_VisaoModuloMenu] VMM
	INNER JOIN [SYS_Modulo] m
	ON m.mod_id = VMM.mod_id
	AND m.sis_id = VMM.sis_id
	WHERE VMM.sis_id = @sis_id
	and vis_id = @visaoDocente
	and m.mod_idPai = @mod_idPai
	and m.mod_nome = 'Agenda'

	SET @mod_idPai = (SELECT mod_id FROM [SYS_Modulo] WHERE mod_nome = 'Documentos' and sis_id = @sis_id and mod_situacao = 1)

	UPDATE VMM
	SET vmm_ordem = 1
	FROM [SYS_VisaoModuloMenu] VMM
	INNER JOIN [SYS_Modulo] m
	ON m.mod_id = VMM.mod_id
	AND m.sis_id = VMM.sis_id
	WHERE VMM.sis_id = @sis_id
	and vis_id = @visaoDocente
	and m.mod_idPai = @mod_idPai
	and m.mod_nome = 'Documentos do docente'

-- DIRETOR

-- ADMINISTRAÇÃO

SET @mod_idPai = (SELECT mod_id FROM [SYS_Modulo] WHERE mod_nome = 'Administração' and sis_id = @sis_id and mod_situacao = 1)

	UPDATE VMM
	SET vmm_ordem = 1
	FROM [SYS_VisaoModuloMenu] VMM
	INNER JOIN [SYS_Modulo] m
	ON m.mod_id = VMM.mod_id
	AND m.sis_id = VMM.sis_id
	WHERE VMM.sis_id = @sis_id
	and vis_id = @visaoDiretor
	and m.mod_idPai = @mod_idPai
	and m.mod_nome = 'Minha escola'

	UPDATE VMM
	SET vmm_ordem = 2
	FROM [SYS_VisaoModuloMenu] VMM
	INNER JOIN [SYS_Modulo] m
	ON m.mod_id = VMM.mod_id
	AND m.sis_id = VMM.sis_id
	WHERE VMM.sis_id = @sis_id
	and vis_id = @visaoDiretor
	and m.mod_idPai = @mod_idPai
	and m.mod_nome = 'Docentes'

		-- SUBMENUS DOCENTES

		DECLARE @mod_idAvo INT = (SELECT mod_id FROM [SYS_Modulo] WHERE mod_nome = 'Docentes' and sis_id = @sis_id and mod_situacao = 1)

		UPDATE VMM
		SET vmm_ordem = 1
		FROM [SYS_VisaoModuloMenu] VMM
		INNER JOIN [SYS_Modulo] m
		ON m.mod_id = VMM.mod_id
		AND m.sis_id = VMM.sis_id
		WHERE VMM.sis_id = @sis_id
		and vis_id = @visaoDiretor
		and m.mod_idPai = @mod_idAvo
		and m.mod_nome = 'Atribuição esporádica'

	UPDATE VMM
	SET vmm_ordem = 3
	FROM [SYS_VisaoModuloMenu] VMM
	INNER JOIN [SYS_Modulo] m
	ON m.mod_id = VMM.mod_id
	AND m.sis_id = VMM.sis_id
	WHERE VMM.sis_id = @sis_id
	and vis_id = @visaoDiretor
	and m.mod_idPai = @mod_idPai
	and m.mod_nome = 'Calendário escolar'

	UPDATE VMM
	SET vmm_ordem = 4
	FROM [SYS_VisaoModuloMenu] VMM
	INNER JOIN [SYS_Modulo] m
	ON m.mod_id = VMM.mod_id
	AND m.sis_id = VMM.sis_id
	WHERE VMM.sis_id = @sis_id
	and vis_id = @visaoDiretor
	and m.mod_idPai = @mod_idPai
	and m.mod_nome = 'Cadastro de eventos do calendário'

	UPDATE VMM
	SET vmm_ordem = 5
	FROM [SYS_VisaoModuloMenu] VMM
	INNER JOIN [SYS_Modulo] m
	ON m.mod_id = VMM.mod_id
	AND m.sis_id = VMM.sis_id
	WHERE VMM.sis_id = @sis_id
	and vis_id = @visaoDiretor
	and m.mod_idPai = @mod_idPai
	and m.mod_nome = 'Areas'

	UPDATE VMM
	SET vmm_ordem = 6
	FROM [SYS_VisaoModuloMenu] VMM
	INNER JOIN [SYS_Modulo] m
	ON m.mod_id = VMM.mod_id
	AND m.sis_id = VMM.sis_id
	WHERE VMM.sis_id = @sis_id
	and vis_id = @visaoDiretor
	and m.mod_idPai = @mod_idPai
	and m.mod_nome = 'Alunos'

SET @mod_idPai = (SELECT mod_id FROM [SYS_Modulo] WHERE mod_nome = 'Registro de classe' and sis_id = @sis_id and mod_situacao = 1)

-- REGISTRO DE CLASSE

	UPDATE VMM
	SET vmm_ordem = 1
	FROM [SYS_VisaoModuloMenu] VMM
	INNER JOIN [SYS_Modulo] m
	ON m.mod_id = VMM.mod_id
	AND m.sis_id = VMM.sis_id
	WHERE VMM.sis_id = @sis_id
	and vis_id = @visaoDiretor
	and m.mod_idPai = @mod_idPai
	and m.mod_nome = 'Fechamento Bimestre'

	UPDATE VMM
	SET vmm_ordem = 2
	FROM [SYS_VisaoModuloMenu] VMM
	INNER JOIN [SYS_Modulo] m
	ON m.mod_id = VMM.mod_id
	AND m.sis_id = VMM.sis_id
	WHERE VMM.sis_id = @sis_id
	and vis_id = @visaoDiretor
	and m.mod_idPai = @mod_idPai
	and m.mod_nome = 'Registro de justificativa de pendencia'

	UPDATE VMM
	SET vmm_ordem = 3
	FROM [SYS_VisaoModuloMenu] VMM
	INNER JOIN [SYS_Modulo] m
	ON m.mod_id = VMM.mod_id
	AND m.sis_id = VMM.sis_id
	WHERE VMM.sis_id = @sis_id
	and vis_id = @visaoDiretor
	and m.mod_idPai = @mod_idPai
	and m.mod_nome = 'Registro de justificativa de falta - Criar os menus caso não existam. Permissão inclusão para Docente, CP e diretor, demais apenas consulta)'

	UPDATE VMM
	SET vmm_ordem = 4
	FROM [SYS_VisaoModuloMenu] VMM
	INNER JOIN [SYS_Modulo] m
	ON m.mod_id = VMM.mod_id
	AND m.sis_id = VMM.sis_id
	WHERE VMM.sis_id = @sis_id
	and vis_id = @visaoDiretor
	and m.mod_idPai = @mod_idPai
	and m.mod_nome = 'Registro de justificativa de falta EF - Criar os menus caso não existam. Permissão inclusão para Docente EF, CP e secretario, demais apenas consulta)'

SET @mod_idPai = (SELECT mod_id FROM [SYS_Modulo] WHERE mod_nome = 'Registro de classe' and sis_id = @sis_id and mod_situacao = 1)

-- DOCUMENTOS

	UPDATE VMM
	SET vmm_ordem = 1
	FROM [SYS_VisaoModuloMenu] VMM
	INNER JOIN [SYS_Modulo] m
	ON m.mod_id = VMM.mod_id
	AND m.sis_id = VMM.sis_id
	WHERE VMM.sis_id = @sis_id
	and vis_id = @visaoDiretor
	and m.mod_idPai = @mod_idPai
	and m.mod_nome = 'Documentos do aluno'

	UPDATE VMM
	SET vmm_ordem = 2
	FROM [SYS_VisaoModuloMenu] VMM
	INNER JOIN [SYS_Modulo] m
	ON m.mod_id = VMM.mod_id
	AND m.sis_id = VMM.sis_id
	WHERE VMM.sis_id = @sis_id
	and vis_id = @visaoDiretor
	and m.mod_idPai = @mod_idPai
	and m.mod_nome = 'Documentos do docente'

	UPDATE VMM
	SET vmm_ordem = 3
	FROM [SYS_VisaoModuloMenu] VMM
	INNER JOIN [SYS_Modulo] m
	ON m.mod_id = VMM.mod_id
	AND m.sis_id = VMM.sis_id
	WHERE VMM.sis_id = @sis_id
	and vis_id = @visaoDiretor
	and m.mod_idPai = @mod_idPai
	and m.mod_nome = 'Documentos do gestor'