USE [CoreSSO]
GO

DECLARE @sis_id INT = 102

	DECLARE @mod_idPai INT

	SET @mod_idPai = (SELECT mod_id FROM [SYS_Modulo] WHERE mod_nome = 'Documentos' and sis_id = @sis_id and mod_situacao = 1)

	IF(@mod_idPai <> NULL)
	BEGIN
		--MODULO PAI
		UPDATE m
		SET m.mod_nome = 'Relatórios'
		FROM [SYS_Modulo] m
		WHERE m.sis_id = @sis_id
		--and m.mod_idPai = @mod_idPai
		and m.mod_id = @mod_idPai
		and m.mod_situacao = 1

		--MODULO
		UPDATE m
		SET m.mod_nome = 'Alunos'
		FROM [SYS_Modulo] m
		WHERE m.sis_id = @sis_id
		and m.mod_idPai = @mod_idPai
		and m.mod_nome = 'Documentos do aluno'
		and m.mod_situacao = 1

		--SITEMAP
		UPDATE msp
		SET msp.msm_nome = 'Alunos'
		FROM [SYS_ModuloSiteMap] msp
		WHERE msm_nome = 'Documentos do aluno'
		and sis_id = @sis_id

		--MODULO
		UPDATE m
		SET m.mod_nome = 'Docente'
		FROM [SYS_Modulo] m
		WHERE m.sis_id = @sis_id
		and m.mod_idPai = @mod_idPai
		and m.mod_nome = 'Documentos do docente'
		and m.mod_situacao = 1

		--SITEMAP
		UPDATE msp
		SET msp.msm_nome = 'Docente'
		FROM [SYS_ModuloSiteMap] msp
		WHERE msm_nome = 'Documentos do docente'
		and sis_id = @sis_id

		--MODULO
		UPDATE m
		SET m.mod_nome = 'Gestor'
		FROM [SYS_Modulo] m
		WHERE m.sis_id = @sis_id
		and m.mod_idPai = @mod_idPai
		and m.mod_nome = 'Documentos do gestor'
		and m.mod_situacao = 1

		--SITEMAP
		UPDATE msp
		SET msp.msm_nome = 'Gestor'
		FROM [SYS_ModuloSiteMap] msp
		WHERE msm_nome = 'Documentos do gestor'
		and sis_id = @sis_id

		SET @mod_idPai = (SELECT mod_id FROM [SYS_Modulo] WHERE mod_nome = 'Cadastros' and sis_id = @sis_id and mod_situacao = 1)

		IF(@mod_idPai <> NULL)
		BEGIN
			--MODULO
			UPDATE m
			SET m.mod_nome = 'Documentos'
			FROM [SYS_Modulo] m
			WHERE m.sis_id = @sis_id
			and m.mod_idPai = @mod_idPai
			and m.mod_nome = 'Áreas'
			and m.mod_situacao = 1

			UPDATE msp
			SET msp.msm_nome = 'Busca de documentos'
			FROM [SYS_ModuloSiteMap] msp
			WHERE msm_nome = 'Busca de áreas'
			and sis_id = @sis_id
		END
	END