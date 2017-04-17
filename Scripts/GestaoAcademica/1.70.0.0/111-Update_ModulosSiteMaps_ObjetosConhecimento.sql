

--Iniciar transação
BEGIN TRANSACTION
SET XACT_ABORT ON

	DECLARE @sis_id INT = 102
	DECLARE @mod_idPai INT

	SET @mod_idPai = (SELECT mod_id FROM [SYS_Modulo] WHERE mod_nome = 'Cadastros' AND sis_id = @sis_id AND mod_situacao = 1)

	IF(@mod_idPai IS NOT NULL)
	BEGIN

		--MODULO
		UPDATE m
		SET m.mod_nome = 'Objetos de conhecimento'
		FROM [SYS_Modulo] m
		WHERE m.sis_id = @sis_id
		AND m.mod_idPai = @mod_idPai
		AND m.mod_nome = 'Objetos de aprendizagem'
		AND m.mod_situacao = 1

		--SITEMAP
		UPDATE msp
		SET msp.msm_nome = 'Consulta de objetos de conhecimento'
		FROM [SYS_ModuloSiteMap] msp
		WHERE msm_nome = 'Consulta de objetos de aprendizagem'
		AND sis_id = @sis_id

		--SITEMAP
		UPDATE msp
		SET msp.msm_nome = 'Cadastro de objetos de conhecimento'
		FROM [SYS_ModuloSiteMap] msp
		WHERE msm_nome = 'Cadastro de objetos de aprendizagem'
		AND sis_id = @sis_id

	END

	SET @mod_idPai = (SELECT mod_id FROM [SYS_Modulo] WHERE mod_nome = 'Gestor' AND sis_id = @sis_id AND mod_situacao = 1)

	IF(@mod_idPai IS NOT NULL)
	BEGIN

		--MODULO
		UPDATE m
		SET m.mod_nome = 'Objetos de conhecimento'
		FROM [SYS_Modulo] m
		WHERE m.sis_id = @sis_id
		AND m.mod_idPai = @mod_idPai
		AND m.mod_nome = 'Objetos de aprendizagem'
		AND m.mod_situacao = 1

		--SITEMAP
		UPDATE msp
		SET msp.msm_nome = 'Objetos de conhecimento'
		FROM [SYS_ModuloSiteMap] msp
		WHERE msm_nome = 'Objetos de aprendizagem'
		AND sis_id = @sis_id

	END

-- Fechar transação
SET XACT_ABORT OFF
COMMIT TRANSACTION	