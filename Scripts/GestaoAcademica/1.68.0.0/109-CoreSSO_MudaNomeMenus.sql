USE [CoreSSO]
GO

BEGIN TRANSACTION 
SET XACT_ABORT ON   

	DECLARE @sis_id INT = 102

	DECLARE @mod_idPai INT

	SET @mod_idPai = (SELECT mod_id FROM [SYS_Modulo] WHERE mod_nome = 'Documentos' AND sis_id = @sis_id AND mod_situacao = 1)

	IF(@mod_idPai IS NOT NULL)
	BEGIN
		--MODULO PAI
		UPDATE m
		SET m.mod_nome = 'Relatórios'
		FROM [SYS_Modulo] m
		WHERE m.sis_id = @sis_id
		--AND m.mod_idPai = @mod_idPai
		AND m.mod_id = @mod_idPai
		AND m.mod_situacao = 1

		--MODULO
		UPDATE m
		SET m.mod_nome = 'Alunos'
		FROM [SYS_Modulo] m
		WHERE m.sis_id = @sis_id
		AND m.mod_idPai = @mod_idPai
		AND m.mod_nome = 'Documentos do aluno'
		AND m.mod_situacao = 1

		--SITEMAP
		UPDATE msp
		SET msp.msm_nome = 'Alunos'
		FROM [SYS_ModuloSiteMap] msp
		WHERE msm_nome = 'Documentos do aluno'
		AND sis_id = @sis_id

		--MODULO
		UPDATE m
		SET m.mod_nome = 'Docente'
		FROM [SYS_Modulo] m
		WHERE m.sis_id = @sis_id
		AND m.mod_idPai = @mod_idPai
		AND m.mod_nome = 'Documentos do docente'
		AND m.mod_situacao = 1

		--SITEMAP
		UPDATE msp
		SET msp.msm_nome = 'Docente'
		FROM [SYS_ModuloSiteMap] msp
		WHERE msm_nome = 'Documentos do docente'
		AND sis_id = @sis_id

		--MODULO
		UPDATE m
		SET m.mod_nome = 'Gestor'
		FROM [SYS_Modulo] m
		WHERE m.sis_id = @sis_id
		AND m.mod_idPai = @mod_idPai
		AND m.mod_nome = 'Documentos do gestor'
		AND m.mod_situacao = 1

		--SITEMAP
		UPDATE msp
		SET msp.msm_nome = 'Gestor'
		FROM [SYS_ModuloSiteMap] msp
		WHERE msm_nome = 'Documentos do gestor'
		AND sis_id = @sis_id

	END
	
	SET @mod_idPai = (SELECT mod_id FROM [SYS_Modulo] WHERE mod_nome = 'Administração' AND sis_id = @sis_id AND mod_situacao = 1)

	IF(@mod_idPai IS NOT NULL)
	BEGIN
		
		DECLARE @mod_id INT = (SELECT TOP 1 mod_id FROM SYS_Modulo WITH(NOLOCK) WHERE mod_nome = 'Áreas' AND sis_id = @sis_id AND mod_situacao <> 3)
		
		IF(@mod_id IS NOT NULL)
		BEGIN
			UPDATE msp
			SET msp.msm_nome = 'Cadastro de links/documentos'
			FROM [SYS_ModuloSitemap] msp
			WHERE msp.msm_nome = 'Cadastro de documentos'
			AND msp.sis_id = @sis_id 
			AND msp.mod_id = @mod_id 
			
			UPDATE msp
			SET msp.msm_nome = 'Cadastro de documentos'
			FROM [SYS_ModuloSitemap] msp
			WHERE msp.msm_nome = 'Cadastro de áreas para links e documentos.'
			AND msp.sis_id = @sis_id 
			AND msp.mod_id = @mod_id 

			UPDATE msp
			SET msp.msm_nome = 'Busca de documentos'
			FROM [SYS_ModuloSiteMap] msp
			WHERE msm_nome = 'Busca de áreas'
			AND sis_id = @sis_id
			AND mod_id = @mod_id
		END
		
		--MODULO
		UPDATE m
		SET m.mod_nome = 'Documentos'
		FROM [SYS_Modulo] m
		WHERE m.sis_id = @sis_id
		AND m.mod_idPai = @mod_idPai
		AND m.mod_id = @mod_id
		AND m.mod_situacao = 1
	END
	
-- Fechar transação     
SET XACT_ABORT OFF 
COMMIT TRANSACTION