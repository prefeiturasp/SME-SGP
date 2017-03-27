/*------------------------------------------------------------------------------------
   Project/Ticket#: SGP
   Description: Ajusta a ordem dos itens do menu
-------------------------------------------------------------------------------------*/

	DECLARE @entId as uniqueidentifier;
	DECLARE @sis_id INT;
	DECLARE @visaoDocente INT = 4;
	DECLARE @visaoDiretor INT = 3;

	SELECT TOP 1 
		@entId = sse.ent_id,
		@sis_id =  ss.sis_id
	FROM 
		Synonym_SYS_SistemaEntidade AS sse WITH(NOLOCK)
		INNER JOIN  Synonym_SYS_Sistema AS ss WITH(NOLOCK)
			ON sse.sis_id = ss.sis_id
	WHERE 
		ss.sis_nome = '$SystemName$'

	-- DOCENTE

	-- ADMINISTRAÇÃO

	DECLARE @mod_idPai INT = (SELECT mod_id FROM SYS_Modulo WHERE mod_nome = 'Administração' AND sis_id = @sis_id AND mod_situacao = 1)

	UPDATE vmm
	SET vmm_ordem = 1
	FROM SYS_VisaoModuloMenu vmm
	INNER JOIN SYS_Modulo m
	ON m.mod_id = vmm.mod_id
	AND m.sis_id = vmm.sis_id
	WHERE vmm.sis_id = @sis_id
	AND vis_id = @visaoDocente
	AND m.mod_idPai = @mod_idPai
	AND m.mod_nome = 'Calendário escolar'
	AND m.mod_situacao = 1

	UPDATE vmm
	SET vmm_ordem = 2
	FROM SYS_VisaoModuloMenu vmm
	INNER JOIN SYS_Modulo m
	ON m.mod_id = vmm.mod_id
	AND m.sis_id = vmm.sis_id
	WHERE vmm.sis_id = @sis_id
	AND vis_id = @visaoDocente
	AND m.mod_idPai = @mod_idPai
	AND m.mod_nome = 'Alunos'
	AND m.mod_situacao = 1

	-- REGISTRO DE CLASSE

	SET @mod_idPai = (SELECT mod_id FROM SYS_Modulo WHERE mod_nome = 'Registro de classe' AND sis_id = @sis_id AND mod_situacao = 1)

	UPDATE SYS_Modulo 
	SET mod_idPai = @mod_idPai
	WHERE sis_id = @sis_id AND mod_nome = 'Minhas turmas'

	UPDATE SYS_Modulo 
	SET mod_idPai = @mod_idPai
	WHERE sis_id = @sis_id AND mod_nome = 'Atribuição de docentes'

	DECLARE @mod_id INT = (SELECT mod_id FROM [SYS_Modulo] WHERE mod_nome = 'Docentes' AND sis_id = @sis_id AND mod_situacao = 1)

	DELETE FROM SYS_GrupoPermissao
	WHERE sis_id = @sis_id 
	AND mod_id =  @mod_id
	AND gru_id IN (SELECT gru_id FROM SYS_Grupo gru
				   WHERE vis_id = @visaoDocente)

	UPDATE vmm
	SET vmm_ordem = 1
	FROM SYS_VisaoModuloMenu vmm
	INNER JOIN SYS_Modulo m
	ON m.mod_id = vmm.mod_id
	AND m.sis_id = vmm.sis_id
	WHERE vmm.sis_id = @sis_id
	AND vis_id = @visaoDocente
	AND m.mod_idPai = @mod_idPai
	AND m.mod_nome = 'Minhas turmas'
	AND m.mod_situacao = 1

	UPDATE vmm
	SET vmm_ordem = 2
	FROM SYS_VisaoModuloMenu vmm
	INNER JOIN SYS_Modulo m
	ON m.mod_id = vmm.mod_id
	AND m.sis_id = vmm.sis_id
	WHERE vmm.sis_id = @sis_id
	AND vis_id = @visaoDocente
	AND m.mod_idPai = @mod_idPai
	AND m.mod_nome = 'Atribuição de docentes'
	AND m.mod_situacao = 1

	UPDATE vmm
	SET vmm_ordem = 3
	FROM SYS_VisaoModuloMenu vmm
	INNER JOIN SYS_Modulo m
	ON m.mod_id = vmm.mod_id
	AND m.sis_id = vmm.sis_id
	WHERE vmm.sis_id = @sis_id
	AND vis_id = @visaoDocente
	AND m.mod_idPai = @mod_idPai
	AND m.mod_nome = 'Compensação de ausências'
	AND m.mod_situacao = 1

	UPDATE vmm
	SET vmm_ordem = 4
	FROM SYS_VisaoModuloMenu vmm
	INNER JOIN SYS_Modulo m
	ON m.mod_id = vmm.mod_id
	AND m.sis_id = vmm.sis_id
	WHERE vmm.sis_id = @sis_id
	AND vis_id = @visaoDocente
	AND m.mod_idPai = @mod_idPai
	AND m.mod_nome = 'Agenda'
	AND m.mod_situacao = 1

	SET @mod_idPai = (SELECT mod_id FROM SYS_Modulo WHERE mod_nome = 'Documentos' AND sis_id = @sis_id AND mod_situacao = 1)

	UPDATE vmm
	SET vmm_ordem = 1
	FROM SYS_VisaoModuloMenu vmm
	INNER JOIN SYS_Modulo m
	ON m.mod_id = vmm.mod_id
	AND m.sis_id = vmm.sis_id
	WHERE vmm.sis_id = @sis_id
	AND vis_id = @visaoDocente
	AND m.mod_idPai = @mod_idPai
	AND m.mod_nome = 'Documentos do docente'
	AND m.mod_situacao = 1

	-- DIRETOR

	-- ADMINISTRAÇÃO

	SET @mod_idPai = (SELECT mod_id FROM SYS_Modulo WHERE mod_nome = 'Administração' AND sis_id = @sis_id AND mod_situacao = 1)

	UPDATE SYS_Modulo 
	SET mod_idPai = @mod_idPai
	WHERE sis_id = @sis_id AND mod_nome = 'Minha escola'

	UPDATE SYS_Modulo 
	SET mod_idPai = @mod_idPai
	WHERE sis_id = @sis_id AND mod_nome = 'Eventos do calendário escolar'

	UPDATE SYS_Modulo 
	SET mod_idPai = @mod_idPai
	WHERE sis_id = @sis_id AND mod_nome = 'Áreas'

	SET @mod_id = (SELECT mod_id FROM [SYS_Modulo] WHERE mod_nome = 'Gestores' AND sis_id = @sis_id AND mod_situacao = 1)

	DELETE FROM SYS_GrupoPermissao
	WHERE sis_id = @sis_id 
	AND mod_id =  @mod_id
	AND gru_id IN (SELECT gru_id FROM SYS_Grupo gru
				   WHERE vis_id = @visaoDiretor)

	UPDATE SYS_Modulo
	SET mod_situacao = 3
	WHERE sis_id = @sis_id 
	AND mod_id =  @mod_id

	SET @mod_id = (SELECT mod_id FROM [SYS_Modulo] WHERE mod_nome = 'Cadastros' AND sis_id = @sis_id AND mod_situacao = 1)

	DELETE FROM SYS_GrupoPermissao
	WHERE sis_id = @sis_id 
	AND mod_id =  @mod_id
	AND gru_id IN (SELECT gru_id FROM SYS_Grupo gru
				   WHERE vis_id = @visaoDiretor)

	UPDATE vmm
	SET vmm_ordem = 1
	FROM SYS_VisaoModuloMenu vmm
	INNER JOIN SYS_Modulo m
	ON m.mod_id = vmm.mod_id
	AND m.sis_id = vmm.sis_id
	WHERE vmm.sis_id = @sis_id
	AND vis_id = @visaoDiretor
	AND m.mod_idPai = @mod_idPai
	AND m.mod_nome = 'Minha escola'
	AND m.mod_situacao = 1

	UPDATE vmm
	SET vmm_ordem = 2
	FROM SYS_VisaoModuloMenu vmm
	INNER JOIN SYS_Modulo m
	ON m.mod_id = vmm.mod_id
	AND m.sis_id = vmm.sis_id
	WHERE vmm.sis_id = @sis_id
	AND vis_id = @visaoDiretor
	AND m.mod_idPai = @mod_idPai
	AND m.mod_nome = 'Docentes'
	AND m.mod_situacao = 1

		-- SUBMENUS DOCENTES

		DECLARE @mod_idAvo INT = (SELECT mod_id FROM SYS_Modulo WHERE mod_nome = 'Docentes' AND sis_id = @sis_id AND mod_situacao = 1)

		UPDATE vmm
		SET vmm_ordem = 1
		FROM SYS_VisaoModuloMenu vmm
		INNER JOIN SYS_Modulo m
		ON m.mod_id = vmm.mod_id
		AND m.sis_id = vmm.sis_id
		WHERE vmm.sis_id = @sis_id
		AND vis_id = @visaoDiretor
		AND m.mod_idPai = @mod_idAvo
		AND m.mod_nome = 'Atribuição esporádica'
		AND m.mod_situacao = 1

		SET @mod_id = (SELECT mod_id FROM [SYS_Modulo] WHERE mod_nome = 'Atribuição esporádica' AND sis_id = @sis_id AND mod_idPai = @mod_idAvo AND mod_situacao = 1)

		DELETE FROM SYS_GrupoPermissao
		WHERE sis_id = @sis_id 
		AND mod_id =  @mod_idAvo
		AND gru_id IN (SELECT gru_id FROM SYS_Grupo gru
					   WHERE vis_id = @visaoDiretor 
					   AND not exists(SELECT TOP 1 gpe.gru_id FROM SYS_GrupoPermissao gpe
									  WHERE gru.gru_id = gpe.gru_id AND sis_id = @sis_id AND mod_id = @mod_id
									  AND (grp_consultar = 1 or grp_inserir = 1 or grp_alterar = 1 or grp_excluir = 1)))

	UPDATE vmm
	SET vmm_ordem = 3
	FROM SYS_VisaoModuloMenu vmm
	INNER JOIN SYS_Modulo m
	ON m.mod_id = vmm.mod_id
	AND m.sis_id = vmm.sis_id
	WHERE vmm.sis_id = @sis_id
	AND vis_id = @visaoDiretor
	AND m.mod_idPai = @mod_idPai
	AND m.mod_nome = 'Calendário escolar'
	AND m.mod_situacao = 1

	UPDATE vmm
	SET vmm_ordem = 4
	FROM SYS_VisaoModuloMenu vmm
	INNER JOIN SYS_Modulo m
	ON m.mod_id = vmm.mod_id
	AND m.sis_id = vmm.sis_id
	WHERE vmm.sis_id = @sis_id
	AND vis_id = @visaoDiretor
	AND m.mod_idPai = @mod_idPai
	AND m.mod_nome = 'Eventos do calendário escolar'
	AND m.mod_situacao = 1

	UPDATE vmm
	SET vmm_ordem = 5
	FROM SYS_VisaoModuloMenu vmm
	INNER JOIN SYS_Modulo m
	ON m.mod_id = vmm.mod_id
	AND m.sis_id = vmm.sis_id
	WHERE vmm.sis_id = @sis_id
	AND vis_id = @visaoDiretor
	AND m.mod_idPai = @mod_idPai
	AND m.mod_nome = 'Áreas'
	AND m.mod_situacao = 1

	UPDATE vmm
	SET vmm_ordem = 6
	FROM SYS_VisaoModuloMenu vmm
	INNER JOIN SYS_Modulo m
	ON m.mod_id = vmm.mod_id
	AND m.sis_id = vmm.sis_id
	WHERE vmm.sis_id = @sis_id
	AND vis_id = @visaoDiretor
	AND m.mod_idPai = @mod_idPai
	AND m.mod_nome = 'Alunos'
	AND m.mod_situacao = 1

	-- REGISTRO DE CLASSE

	SET @mod_idPai = (SELECT mod_id FROM SYS_Modulo WHERE mod_nome = 'Registro de classe' AND sis_id = @sis_id AND mod_situacao = 1)

	UPDATE SYS_Modulo 
	SET mod_idPai = @mod_idPai
	WHERE sis_id = @sis_id AND mod_nome = 'Registros de justificativas de pendências'

	UPDATE vmm
	SET vmm_ordem = 1
	FROM SYS_VisaoModuloMenu vmm
	INNER JOIN SYS_Modulo m
	ON m.mod_id = vmm.mod_id
	AND m.sis_id = vmm.sis_id
	WHERE vmm.sis_id = @sis_id
	AND vis_id = @visaoDiretor
	AND m.mod_idPai = @mod_idPai
	AND m.mod_nome = 'Minhas turmas'
	AND m.mod_situacao = 1

	UPDATE vmm
	SET vmm_ordem = 2
	FROM SYS_VisaoModuloMenu vmm
	INNER JOIN SYS_Modulo m
	ON m.mod_id = vmm.mod_id
	AND m.sis_id = vmm.sis_id
	WHERE vmm.sis_id = @sis_id
	AND vis_id = @visaoDiretor
	AND m.mod_idPai = @mod_idPai
	AND m.mod_nome = 'Fechamento do bimestre'
	AND m.mod_situacao = 1

	UPDATE vmm
	SET vmm_ordem = 3
	FROM SYS_VisaoModuloMenu vmm
	INNER JOIN SYS_Modulo m
	ON m.mod_id = vmm.mod_id
	AND m.sis_id = vmm.sis_id
	WHERE vmm.sis_id = @sis_id
	AND vis_id = @visaoDiretor
	AND m.mod_idPai = @mod_idPai
	AND m.mod_nome = 'Registros de justificativas de pendências'
	AND m.mod_situacao = 1

	--UPDATE vmm
	--SET vmm_ordem = 4
	--FROM SYS_VisaoModuloMenu vmm
	--INNER JOIN SYS_Modulo m
	--ON m.mod_id = vmm.mod_id
	--AND m.sis_id = vmm.sis_id
	--WHERE vmm.sis_id = @sis_id
	--AND vis_id = @visaoDiretor
	--AND m.mod_idPai = @mod_idPai
	--AND m.mod_nome = 'Registro de justificativa de falta'
	--AND m.mod_situacao = 1
    --
	--UPDATE vmm
	--SET vmm_ordem = 5
	--FROM SYS_VisaoModuloMenu vmm
	--INNER JOIN SYS_Modulo m
	--ON m.mod_id = vmm.mod_id
	--AND m.sis_id = vmm.sis_id
	--WHERE vmm.sis_id = @sis_id
	--AND vis_id = @visaoDiretor
	--AND m.mod_idPai = @mod_idPai
	--AND m.mod_nome = 'Registro de justificativa de falta EF'
	--AND m.mod_situacao = 1

	UPDATE vmm
	SET vmm_ordem = 6
	FROM SYS_VisaoModuloMenu vmm
	INNER JOIN SYS_Modulo m
	ON m.mod_id = vmm.mod_id
	AND m.sis_id = vmm.sis_id
	WHERE vmm.sis_id = @sis_id
	AND vis_id = @visaoDiretor
	AND m.mod_idPai = @mod_idPai
	AND m.mod_nome = 'Justificativas de faltas'
	AND m.mod_situacao = 1

	-- DOCUMENTOS

	SET @mod_idPai = (SELECT mod_id FROM SYS_Modulo WHERE mod_nome = 'Documentos' AND sis_id = @sis_id AND mod_situacao = 1)

	UPDATE vmm
	SET vmm_ordem = 1
	FROM SYS_VisaoModuloMenu vmm
	INNER JOIN SYS_Modulo m
	ON m.mod_id = vmm.mod_id
	AND m.sis_id = vmm.sis_id
	WHERE vmm.sis_id = @sis_id
	AND vis_id = @visaoDiretor
	AND m.mod_idPai = @mod_idPai
	AND m.mod_nome = 'Documentos do aluno'
	AND m.mod_situacao = 1

	UPDATE vmm
	SET vmm_ordem = 2
	FROM SYS_VisaoModuloMenu vmm
	INNER JOIN SYS_Modulo m
	ON m.mod_id = vmm.mod_id
	AND m.sis_id = vmm.sis_id
	WHERE vmm.sis_id = @sis_id
	AND vis_id = @visaoDiretor
	AND m.mod_idPai = @mod_idPai
	AND m.mod_nome = 'Documentos do docente'
	AND m.mod_situacao = 1

	UPDATE vmm
	SET vmm_ordem = 3
	FROM SYS_VisaoModuloMenu vmm
	INNER JOIN SYS_Modulo m
	ON m.mod_id = vmm.mod_id
	AND m.sis_id = vmm.sis_id
	WHERE vmm.sis_id = @sis_id
	AND vis_id = @visaoDiretor
	AND m.mod_idPai = @mod_idPai
	AND m.mod_nome = 'Documentos do gestor'
	AND m.mod_situacao = 1