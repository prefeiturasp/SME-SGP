USE [GestaoPedagogica]
GO

BEGIN TRANSACTION 
SET XACT_ABORT ON  

	DECLARE @sis_id INT, @gru_idDocente UNIQUEIDENTIFIER

	SET @sis_id = (SELECT sis_id FROM Synonym_SYS_Sistema WHERE sis_nome = ' SGP' AND sis_situacao = 1)
	SET @gru_idDocente = (SELECT gru_id FROM Synonym_SYS_Grupo WHERE gru_nome = 'Docente' AND gru_situacao <> 3)

	---- Insere as permissões nas telas
	---- Minhas turmas exceto anotações do aluno e observações do fechamento
	DECLARE @mod_idDocentes INT, @modIdMinhasTurmas INT

	SET @mod_idDocentes = (SELECT mod_id FROM Synonym_SYS_Modulo WHERE sis_id = @sis_id AND mod_nome = 'Docentes' AND mod_situacao = 1)
	SET @modIdMinhasTurmas = (SELECT mod_id FROM Synonym_SYS_Modulo WHERE sis_id = @sis_id AND mod_nome = 'Minhas turmas' AND mod_idPai = @mod_idDocentes AND mod_situacao = 1)
	
	IF NOT EXISTS (SELECT mod_id FROM CFG_PermissaoModuloOperacao WHERE gru_id = @gru_idDocente AND sis_id = @sis_id AND mod_id = @modIdMinhasTurmas AND pmo_operacao = 16)
	BEGIN
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
			@gru_idDocente,
			@sis_id,
			@modIdMinhasTurmas,
			16, -- Operação para lançamento de frequência
			1,
			0,
			0,
			0
		)
	END
	ELSE
	BEGIN
		UPDATE CFG_PermissaoModuloOperacao
			SET pmo_permissaoConsulta = 1, 
				pmo_permissaoInclusao = 0, 
				pmo_permissaoEdicao = 0, 
				pmo_permissaoExclusao = 0
		WHERE 
			gru_id = @gru_idDocente AND
			sis_id = @sis_id AND
			mod_id = @modIdMinhasTurmas AND
			pmo_operacao = 16
	END

-- Fechar transação     
SET XACT_ABORT OFF 
COMMIT TRANSACTION