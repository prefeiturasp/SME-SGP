

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
	and m.mod_situacao = 1

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
	and m.mod_situacao = 1

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
	and m.mod_situacao = 1

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
	and m.mod_situacao = 1

	UPDATE VMM
	SET vmm_ordem = 3
	FROM [SYS_VisaoModuloMenu] VMM
	INNER JOIN [SYS_Modulo] m
	ON m.mod_id = VMM.mod_id
	AND m.sis_id = VMM.sis_id
	WHERE VMM.sis_id = @sis_id
	and vis_id = @visaoDocente
	and m.mod_idPai = @mod_idPai
	and m.mod_nome = 'Compensação de ausências'
	and m.mod_situacao = 1

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
	and m.mod_situacao = 1

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
	and m.mod_situacao = 1

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
	and m.mod_situacao = 1

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
	and m.mod_situacao = 1

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
		and m.mod_situacao = 1

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
	and m.mod_situacao = 1

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
	and m.mod_situacao = 1

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
	and m.mod_situacao = 1

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
	and m.mod_situacao = 1

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
	and m.mod_situacao = 1

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
	and m.mod_situacao = 1

	UPDATE VMM
	SET vmm_ordem = 3
	FROM [SYS_VisaoModuloMenu] VMM
	INNER JOIN [SYS_Modulo] m
	ON m.mod_id = VMM.mod_id
	AND m.sis_id = VMM.sis_id
	WHERE VMM.sis_id = @sis_id
	and vis_id = @visaoDiretor
	and m.mod_idPai = @mod_idPai
	and m.mod_nome = 'Registro de justificativa de falta'
	and m.mod_situacao = 1

	UPDATE VMM
	SET vmm_ordem = 4
	FROM [SYS_VisaoModuloMenu] VMM
	INNER JOIN [SYS_Modulo] m
	ON m.mod_id = VMM.mod_id
	AND m.sis_id = VMM.sis_id
	WHERE VMM.sis_id = @sis_id
	and vis_id = @visaoDiretor
	and m.mod_idPai = @mod_idPai
	and m.mod_nome = 'Registro de justificativa de falta EF'
	and m.mod_situacao = 1

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
	and m.mod_situacao = 1

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
	and m.mod_situacao = 1

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
	and m.mod_situacao = 1