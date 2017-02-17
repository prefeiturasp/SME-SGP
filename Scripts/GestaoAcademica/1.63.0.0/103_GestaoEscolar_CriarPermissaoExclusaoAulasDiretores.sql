USE [GestaoPedagogica]
GO

--Iniciar transação
BEGIN TRANSACTION
SET XACT_ABORT ON

	DECLARE @sis_id INT, @gru_idDiretores UNIQUEIDENTIFIER, @mod_id INT 

	SET @sis_id = (SELECT sis_id FROM Synonym_SYS_Sistema WHERE sis_nome = ' SGP' and sis_situacao = 1)
	SET @gru_idDiretores = (SELECT gru_id FROM Synonym_SYS_Grupo WHERE gru_nome = 'Diretor Escolar' and sis_id = @sis_id and gru_situacao = 1)
	SET @mod_id = (SELECT mod_id FROM Synonym_SYS_Modulo WHERE mod_nome = 'Minhas turmas' and mod_situacao = 1 and sis_id = @sis_id)

	INSERT INTO CFG_PermissaoModuloOperacao
	(
		gru_id, 
		sis_id, 
		mod_id, 
		pmo_operacao, 
		pmo_permissaoConsulta, 
		pmo_permissaoEdicao, 
		pmo_permissaoInclusao, 
		pmo_permissaoExclusao
	)
	VALUES
	(
		@gru_idDiretores,
		@sis_id,
		@mod_id,
		6, -- Operação para exclusão de aulas no diário de classe.
		0,
		0,
		0,
		1
	)
	
-- Fechar transação
SET XACT_ABORT OFF
COMMIT TRANSACTION