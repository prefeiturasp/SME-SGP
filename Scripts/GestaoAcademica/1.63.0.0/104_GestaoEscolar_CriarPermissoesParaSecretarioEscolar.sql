USE [GestaoPedagogica]
GO

--Iniciar transação
BEGIN TRANSACTION
SET XACT_ABORT ON

	DECLARE @sis_id INT, @gru_idSecretarioEscolar UNIQUEIDENTIFIER

	SET @sis_id = (SELECT sis_id FROM Synonym_SYS_Sistema WHERE sis_nome = ' SGP' AND sis_situacao = 1)
	SET @gru_idSecretarioEscolar = (SELECT gru_id FROM Synonym_SYS_Grupo WHERE gru_nome = 'Secretário Escolar' AND gru_situacao <> 3)
	------ Desbloqueia grupo 'Secretário Escolar'
	UPDATE Synonym_SYS_Grupo
		SET gru_situacao = 1
	WHERE 
		gru_id = @gru_idSecretarioEscolar

	---- Insere as permissões nas telas
	---- Minhas turmas exceto anotações do aluno e observações do fechamento
	DECLARE @mod_idDocentes INT, @mod_idRegistroClasse INT, @modIdMinhasTurmas INT, @modIdFechamentoGestor INT

	SET @mod_idDocentes = (SELECT mod_id FROM Synonym_SYS_Modulo WHERE sis_id = @sis_id AND mod_nome = 'Docentes' AND mod_situacao = 1)
	SET @mod_idRegistroClasse = (SELECT mod_id FROM Synonym_SYS_Modulo WHERE sis_id = @sis_id AND mod_nome = 'Registro de Classe' AND mod_situacao = 1)
	SET @modIdMinhasTurmas = (SELECT mod_id FROM Synonym_SYS_Modulo WHERE sis_id = @sis_id AND mod_nome = 'Minhas turmas' AND mod_idPai = @mod_idDocentes AND mod_situacao = 1)
	SET @modIdFechamentoGestor = (SELECT mod_id FROM Synonym_SYS_Modulo WHERE sis_id = @sis_id AND mod_nome = 'Fechamento do bimestre' AND mod_idPai = @mod_idRegistroClasse AND mod_situacao = 1)

	IF NOT EXISTS (SELECT mod_id FROM Synonym_SYS_GrupoPermissao WHERE gru_id = @gru_idSecretarioEscolar AND sis_id = @sis_id AND mod_id= @mod_idDocentes)
	BEGIN
		INSERT INTO Synonym_SYS_GrupoPermissao
		(
			gru_id, 
			sis_id, 
			mod_id, 
			grp_consultar, 
			grp_inserir, 
			grp_alterar, 
			grp_excluir
		)
		VALUES
		(
			@gru_idSecretarioEscolar,
			@sis_id,
			@mod_idDocentes,
			1,
			0,
			0,
			0
		)
	END
	ELSE
	BEGIN
		UPDATE Synonym_SYS_GrupoPermissao
			SET grp_consultar = 1, 
				grp_inserir = 0, 
				grp_alterar = 0, 
				grp_excluir = 0
		WHERE 
			gru_id = @gru_idSecretarioEscolar AND
			sis_id = @sis_id AND
			mod_id = @mod_idDocentes
	END

	IF NOT EXISTS (SELECT mod_id FROM Synonym_SYS_GrupoPermissao WHERE gru_id = @gru_idSecretarioEscolar AND sis_id = @sis_id AND mod_id= @modIdMinhasTurmas)
	BEGIN
		INSERT INTO Synonym_SYS_GrupoPermissao
		(
			gru_id, 
			sis_id, 
			mod_id, 
			grp_consultar, 
			grp_inserir, 
			grp_alterar, 
			grp_excluir
		)
		VALUES
		(
			@gru_idSecretarioEscolar,
			@sis_id,
			@modIdMinhasTurmas,
			1,
			0,
			0,
			0
		)
	END
	ELSE
	BEGIN
		UPDATE Synonym_SYS_GrupoPermissao
			SET grp_consultar = 1, 
				grp_inserir = 0, 
				grp_alterar = 0, 
				grp_excluir = 0
		WHERE 
			gru_id = @gru_idSecretarioEscolar AND
			sis_id = @sis_id AND
			mod_id = @modIdMinhasTurmas
	END

	IF NOT EXISTS (SELECT mod_id FROM CFG_PermissaoModuloOperacao WHERE gru_id = @gru_idSecretarioEscolar AND sis_id = @sis_id AND mod_id= @modIdMinhasTurmas AND pmo_operacao = 7)
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
			@gru_idSecretarioEscolar,
			@sis_id,
			@modIdMinhasTurmas,
			7, -- Operação para visualização das anotações do aluno
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
			gru_id = @gru_idSecretarioEscolar AND
			sis_id = @sis_id AND
			mod_id = @modIdMinhasTurmas AND
			pmo_operacao = 7
	END

	IF NOT EXISTS (SELECT mod_id FROM CFG_PermissaoModuloOperacao WHERE gru_id = @gru_idSecretarioEscolar AND sis_id = @sis_id AND mod_id= @modIdMinhasTurmas AND pmo_operacao = 8)
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
			@gru_idSecretarioEscolar,
			@sis_id,
			@modIdMinhasTurmas,
			8, -- Operação para visualização das observações do fechamento
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
			gru_id = @gru_idSecretarioEscolar AND
			sis_id = @sis_id AND
			mod_id = @modIdMinhasTurmas AND
			pmo_operacao = 8
	END

	-- Tem leitura no fechamento do gestor?
	IF NOT EXISTS (SELECT mod_id FROM Synonym_SYS_GrupoPermissao WHERE gru_id = @gru_idSecretarioEscolar AND sis_id = @sis_id AND mod_id= @mod_idRegistroClasse)
	BEGIN
		INSERT INTO Synonym_SYS_GrupoPermissao
		(
			gru_id, 
			sis_id, 
			mod_id, 
			grp_consultar, 
			grp_inserir, 
			grp_alterar, 
			grp_excluir
		)
		VALUES
		(
			@gru_idSecretarioEscolar,
			@sis_id,
			@mod_idRegistroClasse,
			1,
			0,
			0,
			0
		)
	END
	ELSE
	BEGIN
		UPDATE Synonym_SYS_GrupoPermissao
			SET grp_consultar = 1, 
				grp_inserir = 0, 
				grp_alterar = 0, 
				grp_excluir = 0
		WHERE 
			gru_id = @gru_idSecretarioEscolar AND
			sis_id = @sis_id AND
			mod_id = @mod_idRegistroClasse
	END

	IF NOT EXISTS (SELECT mod_id FROM Synonym_SYS_GrupoPermissao WHERE gru_id = @gru_idSecretarioEscolar AND sis_id = @sis_id AND mod_id= @modIdFechamentoGestor)
	BEGIN
		INSERT INTO Synonym_SYS_GrupoPermissao
		(
			gru_id, 
			sis_id, 
			mod_id, 
			grp_consultar, 
			grp_inserir, 
			grp_alterar, 
			grp_excluir
		)
		VALUES
		(
			@gru_idSecretarioEscolar,
			@sis_id,
			@modIdFechamentoGestor,
			1,
			0,
			0,
			0
		)
	END
	ELSE
	BEGIN
		UPDATE Synonym_SYS_GrupoPermissao
			SET grp_consultar = 1, 
				grp_inserir = 0, 
				grp_alterar = 0, 
				grp_excluir = 0
		WHERE 
			gru_id = @gru_idSecretarioEscolar AND
			sis_id = @sis_id AND
			mod_id = @modIdFechamentoGestor
	END

	IF NOT EXISTS (SELECT mod_id FROM CFG_PermissaoModuloOperacao WHERE gru_id = @gru_idSecretarioEscolar AND sis_id = @sis_id AND mod_id= @modIdMinhasTurmas AND pmo_operacao = 9)
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
			@gru_idSecretarioEscolar,
			@sis_id,
			@modIdMinhasTurmas,
			9, -- Operação para exibição da aba 'Parecer conclusivo'
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
			gru_id = @gru_idSecretarioEscolar AND
			sis_id = @sis_id AND
			mod_id = @modIdMinhasTurmas AND
			pmo_operacao = 9
	END

	IF NOT EXISTS (SELECT mod_id FROM CFG_PermissaoModuloOperacao WHERE gru_id = @gru_idSecretarioEscolar AND sis_id = @sis_id AND mod_id= @modIdFechamentoGestor AND pmo_operacao = 9)
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
			@gru_idSecretarioEscolar,
			@sis_id,
			@modIdFechamentoGestor,
			9, -- Operação para exibição da aba 'Parecer conclusivo'
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
			gru_id = @gru_idSecretarioEscolar AND
			sis_id = @sis_id AND
			mod_id = @modIdFechamentoGestor AND
			pmo_operacao = 9
	END

	IF NOT EXISTS (SELECT mod_id FROM CFG_PermissaoModuloOperacao WHERE gru_id = @gru_idSecretarioEscolar AND sis_id = @sis_id AND mod_id= @modIdMinhasTurmas AND pmo_operacao = 10)
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
			@gru_idSecretarioEscolar,
			@sis_id,
			@modIdMinhasTurmas,
			10, -- Operação para exibição da aba 'Justificativa Pos Conselho'
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
			gru_id = @gru_idSecretarioEscolar AND
			sis_id = @sis_id AND
			mod_id = @modIdMinhasTurmas AND
			pmo_operacao = 10
	END

	IF NOT EXISTS (SELECT mod_id FROM CFG_PermissaoModuloOperacao WHERE gru_id = @gru_idSecretarioEscolar AND sis_id = @sis_id AND mod_id= @modIdFechamentoGestor AND pmo_operacao = 10)
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
			@gru_idSecretarioEscolar,
			@sis_id,
			@modIdFechamentoGestor,
			10, -- Operação para exibição da aba 'Justificativa Pos Conselho'
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
			gru_id = @gru_idSecretarioEscolar AND
			sis_id = @sis_id AND
			mod_id = @modIdFechamentoGestor AND
			pmo_operacao = 10
	END

	IF NOT EXISTS (SELECT mod_id FROM CFG_PermissaoModuloOperacao WHERE gru_id = @gru_idSecretarioEscolar AND sis_id = @sis_id AND mod_id= @modIdMinhasTurmas AND pmo_operacao = 11)
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
			@gru_idSecretarioEscolar,
			@sis_id,
			@modIdMinhasTurmas,
			11, -- Operação para exibição da aba 'Desempenho Aprendizagem'
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
			gru_id = @gru_idSecretarioEscolar AND
			sis_id = @sis_id AND
			mod_id = @modIdMinhasTurmas AND
			pmo_operacao = 11
	END

	IF NOT EXISTS (SELECT mod_id FROM CFG_PermissaoModuloOperacao WHERE gru_id = @gru_idSecretarioEscolar AND sis_id = @sis_id AND mod_id= @modIdFechamentoGestor AND pmo_operacao = 11)
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
			@gru_idSecretarioEscolar,
			@sis_id,
			@modIdFechamentoGestor,
			11, -- Operação para exibição da aba 'Desempenho Aprendizagem'
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
			gru_id = @gru_idSecretarioEscolar AND
			sis_id = @sis_id AND
			mod_id = @modIdFechamentoGestor AND
			pmo_operacao = 11
	END

	IF NOT EXISTS (SELECT mod_id FROM CFG_PermissaoModuloOperacao WHERE gru_id = @gru_idSecretarioEscolar AND sis_id = @sis_id AND mod_id= @modIdMinhasTurmas AND pmo_operacao = 12)
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
			@gru_idSecretarioEscolar,
			@sis_id,
			@modIdMinhasTurmas,
			12, -- Operação para exibição da aba 'Recomendacao Aluno'
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
			gru_id = @gru_idSecretarioEscolar AND
			sis_id = @sis_id AND
			mod_id = @modIdMinhasTurmas AND
			pmo_operacao = 12
	END

	IF NOT EXISTS (SELECT mod_id FROM CFG_PermissaoModuloOperacao WHERE gru_id = @gru_idSecretarioEscolar AND sis_id = @sis_id AND mod_id= @modIdFechamentoGestor AND pmo_operacao = 12)
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
			@gru_idSecretarioEscolar,
			@sis_id,
			@modIdFechamentoGestor,
			12, -- Operação para exibição da aba 'Recomendacao Aluno'
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
			gru_id = @gru_idSecretarioEscolar AND
			sis_id = @sis_id AND
			mod_id = @modIdFechamentoGestor AND
			pmo_operacao = 12
	END

	IF NOT EXISTS (SELECT mod_id FROM CFG_PermissaoModuloOperacao WHERE gru_id = @gru_idSecretarioEscolar AND sis_id = @sis_id AND mod_id= @modIdMinhasTurmas AND pmo_operacao = 13)
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
			@gru_idSecretarioEscolar,
			@sis_id,
			@modIdMinhasTurmas,
			13, -- Operação para exibição da aba 'Recomendacao Responsavel'
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
			gru_id = @gru_idSecretarioEscolar AND
			sis_id = @sis_id AND
			mod_id = @modIdMinhasTurmas AND
			pmo_operacao = 13
	END

	IF NOT EXISTS (SELECT mod_id FROM CFG_PermissaoModuloOperacao WHERE gru_id = @gru_idSecretarioEscolar AND sis_id = @sis_id AND mod_id= @modIdFechamentoGestor AND pmo_operacao = 13)
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
			@gru_idSecretarioEscolar,
			@sis_id,
			@modIdFechamentoGestor,
			13, -- Operação para exibição da aba 'Recomendacao Responsavel'
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
			gru_id = @gru_idSecretarioEscolar AND
			sis_id = @sis_id AND
			mod_id = @modIdFechamentoGestor AND
			pmo_operacao = 13
	END

	IF NOT EXISTS (SELECT mod_id FROM CFG_PermissaoModuloOperacao WHERE gru_id = @gru_idSecretarioEscolar AND sis_id = @sis_id AND mod_id= @modIdMinhasTurmas AND pmo_operacao = 14)
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
			@gru_idSecretarioEscolar,
			@sis_id,
			@modIdMinhasTurmas,
			14, -- Operação para exibição da aba 'Anotacoes Aluno'
			0,
			0,
			0,
			0
		)
	END
	ELSE
	BEGIN
		UPDATE CFG_PermissaoModuloOperacao
			SET pmo_permissaoConsulta = 0, 
				pmo_permissaoInclusao = 0, 
				pmo_permissaoEdicao = 0, 
				pmo_permissaoExclusao = 0
		WHERE 
			gru_id = @gru_idSecretarioEscolar AND
			sis_id = @sis_id AND
			mod_id = @modIdMinhasTurmas AND
			pmo_operacao = 14
	END

	IF NOT EXISTS (SELECT mod_id FROM CFG_PermissaoModuloOperacao WHERE gru_id = @gru_idSecretarioEscolar AND sis_id = @sis_id AND mod_id= @modIdFechamentoGestor AND pmo_operacao = 14)
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
			@gru_idSecretarioEscolar,
			@sis_id,
			@modIdFechamentoGestor,
			14, -- Operação para exibição da aba 'Anotacoes Aluno'
			0,
			0,
			0,
			0
		)
	END
	ELSE
	BEGIN
		UPDATE CFG_PermissaoModuloOperacao
			SET pmo_permissaoConsulta = 0, 
				pmo_permissaoInclusao = 0, 
				pmo_permissaoEdicao = 0, 
				pmo_permissaoExclusao = 0
		WHERE 
			gru_id = @gru_idSecretarioEscolar AND
			sis_id = @sis_id AND
			mod_id = @modIdFechamentoGestor AND
			pmo_operacao = 14
	END

	-- Permissão para cadastrar de histórico escolar
	-- Histórico escolar – menu administração
	DECLARE @mod_idHistoricoEscolar INT, @modIdDadosDoAluno INT, @modIdEnsinoFundamental INT, @modIdTransferencia INT, @modIdInformacoesComplementares INT

	SET @mod_idHistoricoEscolar = (SELECT mod_id FROM Synonym_SYS_Modulo WHERE sis_id = @sis_id AND mod_nome = 'Histórico escolar' AND mod_situacao = 1)
	SET @modIdDadosDoAluno = (SELECT mod_id FROM Synonym_SYS_Modulo WHERE sis_id = @sis_id AND mod_nome = 'Dados do aluno' AND mod_idPai = @mod_idHistoricoEscolar AND mod_situacao = 1)
	SET @modIdEnsinoFundamental = (SELECT mod_id FROM Synonym_SYS_Modulo WHERE sis_id = @sis_id AND mod_nome = 'Ensino fundamental' AND mod_idPai = @mod_idHistoricoEscolar AND mod_situacao = 1)
	SET @modIdTransferencia = (SELECT mod_id FROM Synonym_SYS_Modulo WHERE sis_id = @sis_id AND mod_nome = 'Transferência' AND mod_idPai = @mod_idHistoricoEscolar AND mod_situacao = 1)
	SET @modIdInformacoesComplementares = (SELECT mod_id FROM Synonym_SYS_Modulo WHERE sis_id = @sis_id AND mod_nome = 'Informações complementares' AND mod_idPai = @mod_idHistoricoEscolar AND mod_situacao = 1)

	IF NOT EXISTS (SELECT mod_id FROM Synonym_SYS_GrupoPermissao WHERE gru_id = @gru_idSecretarioEscolar AND sis_id = @sis_id AND mod_id= @mod_idHistoricoEscolar)
	BEGIN
		INSERT INTO Synonym_SYS_GrupoPermissao
		(
			gru_id, 
			sis_id, 
			mod_id, 
			grp_consultar, 
			grp_inserir, 
			grp_alterar, 
			grp_excluir
		)
		VALUES
		(
			@gru_idSecretarioEscolar,
			@sis_id,
			@mod_idHistoricoEscolar,
			1,
			1,
			1,
			1
		)
	END
	ELSE
	BEGIN
		UPDATE Synonym_SYS_GrupoPermissao
			SET grp_consultar = 1, 
				grp_inserir = 1, 
				grp_alterar = 1, 
				grp_excluir = 1
		WHERE 
			gru_id = @gru_idSecretarioEscolar AND
			sis_id = @sis_id AND
			mod_id = @mod_idHistoricoEscolar
	END

	IF NOT EXISTS(SELECT mod_id FROM Synonym_SYS_VisaoModulo WHERE vis_id = 3 AND sis_id = @sis_id AND mod_id = @mod_idHistoricoEscolar)
	BEGIN
		INSERT INTO Synonym_SYS_VisaoModulo
		(
			vis_id,
			sis_id,
			mod_id
		)
		VALUES
		(
			3, 
			@sis_id, 
			@mod_idHistoricoEscolar 
		)
	END

	IF NOT EXISTS(SELECT mod_id FROM Synonym_SYS_VisaoModuloMenu WHERE vis_id = 3 AND sis_id = @sis_id AND mod_id = @mod_idHistoricoEscolar)
	BEGIN
		INSERT INTO Synonym_SYS_VisaoModuloMenu
		(
			vis_id,
			sis_id,
			mod_id,
			msm_id,
			vmm_ordem
		)
		SELECT 
			3,
			sis_id,
			mod_id,
			msm_id,
			vmm_ordem
		FROM	
			Synonym_SYS_VisaoModuloMenu
		WHERE 
			vis_id = 1 
			AND sis_id = @sis_id
			AND mod_id = @mod_idHistoricoEscolar
	END

	IF NOT EXISTS (SELECT mod_id FROM Synonym_SYS_GrupoPermissao WHERE gru_id = @gru_idSecretarioEscolar AND sis_id = @sis_id AND mod_id= @modIdDadosDoAluno)
	BEGIN
		INSERT INTO Synonym_SYS_GrupoPermissao
		(
			gru_id, 
			sis_id, 
			mod_id, 
			grp_consultar, 
			grp_inserir, 
			grp_alterar, 
			grp_excluir
		)
		VALUES
		(
			@gru_idSecretarioEscolar,
			@sis_id,
			@modIdDadosDoAluno,
			1,
			1,
			1,
			1
		)
	END
	ELSE
	BEGIN
		UPDATE Synonym_SYS_GrupoPermissao
			SET grp_consultar = 1, 
				grp_inserir = 1, 
				grp_alterar = 1, 
				grp_excluir = 1
		WHERE 
			gru_id = @gru_idSecretarioEscolar AND
			sis_id = @sis_id AND
			mod_id = @modIdDadosDoAluno
	END

	IF NOT EXISTS (SELECT mod_id FROM Synonym_SYS_GrupoPermissao WHERE gru_id = @gru_idSecretarioEscolar AND sis_id = @sis_id AND mod_id= @modIdEnsinoFundamental)
	BEGIN
		INSERT INTO Synonym_SYS_GrupoPermissao
		(
			gru_id, 
			sis_id, 
			mod_id, 
			grp_consultar, 
			grp_inserir, 
			grp_alterar, 
			grp_excluir
		)
		VALUES
		(
			@gru_idSecretarioEscolar,
			@sis_id,
			@modIdEnsinoFundamental,
			1,
			1,
			1,
			1
		)
	END
	ELSE
	BEGIN
		UPDATE Synonym_SYS_GrupoPermissao
			SET grp_consultar = 1, 
				grp_inserir = 1, 
				grp_alterar = 1, 
				grp_excluir = 1
		WHERE 
			gru_id = @gru_idSecretarioEscolar AND
			sis_id = @sis_id AND
			mod_id = @modIdEnsinoFundamental
	END

	IF NOT EXISTS (SELECT mod_id FROM Synonym_SYS_GrupoPermissao WHERE gru_id = @gru_idSecretarioEscolar AND sis_id = @sis_id AND mod_id= @modIdTransferencia)
	BEGIN
		INSERT INTO Synonym_SYS_GrupoPermissao
		(
			gru_id, 
			sis_id, 
			mod_id, 
			grp_consultar, 
			grp_inserir, 
			grp_alterar, 
			grp_excluir
		)
		VALUES
		(
			@gru_idSecretarioEscolar,
			@sis_id,
			@modIdTransferencia,
			1,
			1,
			1,
			1
		)
	END
	ELSE
	BEGIN
		UPDATE Synonym_SYS_GrupoPermissao
			SET grp_consultar = 1, 
				grp_inserir = 1, 
				grp_alterar = 1, 
				grp_excluir = 1
		WHERE 
			gru_id = @gru_idSecretarioEscolar AND
			sis_id = @sis_id AND
			mod_id = @modIdTransferencia
	END

	IF NOT EXISTS (SELECT mod_id FROM Synonym_SYS_GrupoPermissao WHERE gru_id = @gru_idSecretarioEscolar AND sis_id = @sis_id AND mod_id= @modIdTransferencia)
	BEGIN
		INSERT INTO Synonym_SYS_GrupoPermissao
		(
			gru_id, 
			sis_id, 
			mod_id, 
			grp_consultar, 
			grp_inserir, 
			grp_alterar, 
			grp_excluir
		)
		VALUES
		(
			@gru_idSecretarioEscolar,
			@sis_id,
			@modIdInformacoesComplementares,
			1,
			1,
			1,
			1
		)
	END
	ELSE
	BEGIN
		UPDATE Synonym_SYS_GrupoPermissao
			SET grp_consultar = 1, 
				grp_inserir = 1, 
				grp_alterar = 1, 
				grp_excluir = 1
		WHERE 
			gru_id = @gru_idSecretarioEscolar AND
			sis_id = @sis_id AND
			mod_id = @modIdInformacoesComplementares
	END
	
	-- Documentos do Gestor
	--	Alunos com baixa frequência
	--	Ata final de resultados
	--	Ata final de enriquecimento currículos
	--	Alunos com ausência de fechamento
	DECLARE @modIdDocumentos INT, @modIdDocumentosGestor INT, @modIdRelAlunosBaixaFrequencia INT, @modIdRelAtaFinalResultados INT, @modIdRelAtaFinalEnriquecimentoCurr INT, @modIdRelAlunosAusenciaRegFechamento INT

	SET @modIdDocumentos = (SELECT mod_id FROM Synonym_SYS_Modulo WHERE sis_id = @sis_id AND mod_nome = 'Documentos' AND mod_situacao = 1)
	SET @modIdDocumentosGestor = (SELECT mod_id FROM Synonym_SYS_Modulo WHERE sis_id = @sis_id AND mod_nome = 'Documentos do gestor' AND mod_idPai = @modIdDocumentos AND mod_situacao = 1)
	SET @modIdRelAlunosBaixaFrequencia = (SELECT mod_id FROM Synonym_SYS_Modulo WHERE sis_id = @sis_id AND mod_nome = 'Alunos com baixa frequência' AND mod_idPai = @modIdDocumentosGestor AND mod_situacao = 1)
	SET @modIdRelAtaFinalResultados = (SELECT mod_id FROM Synonym_SYS_Modulo WHERE sis_id = @sis_id AND mod_nome = 'Ata final de resultados' AND mod_idPai = @modIdDocumentosGestor AND mod_situacao = 1)
	SET @modIdRelAtaFinalEnriquecimentoCurr = (SELECT mod_id FROM Synonym_SYS_Modulo WHERE sis_id = @sis_id AND mod_nome = 'Ata final de enriquecimento curricular' AND mod_idPai = @modIdDocumentosGestor AND mod_situacao = 1)
	SET @modIdRelAlunosAusenciaRegFechamento = (SELECT mod_id FROM Synonym_SYS_Modulo WHERE sis_id = @sis_id AND mod_nome = 'Alunos com ausência de registro de fechamento' AND mod_idPai = @modIdDocumentosGestor AND mod_situacao = 1)

	IF NOT EXISTS (SELECT mod_id FROM Synonym_SYS_GrupoPermissao WHERE gru_id = @gru_idSecretarioEscolar AND sis_id = @sis_id AND mod_id= @modIdDocumentos)
	BEGIN
		INSERT INTO Synonym_SYS_GrupoPermissao
		(
			gru_id, 
			sis_id, 
			mod_id, 
			grp_consultar, 
			grp_inserir, 
			grp_alterar, 
			grp_excluir
		)
		VALUES
		(
			@gru_idSecretarioEscolar,
			@sis_id,
			@modIdDocumentos,
			1,
			0,
			0,
			0
		)
	END
	ELSE
	BEGIN
		UPDATE Synonym_SYS_GrupoPermissao
			SET grp_consultar = 1, 
				grp_inserir = 0, 
				grp_alterar = 0, 
				grp_excluir = 0
		WHERE 
			gru_id = @gru_idSecretarioEscolar AND
			sis_id = @sis_id AND
			mod_id = @modIdDocumentos
	END

	IF NOT EXISTS (SELECT mod_id FROM Synonym_SYS_GrupoPermissao WHERE gru_id = @gru_idSecretarioEscolar AND sis_id = @sis_id AND mod_id= @modIdDocumentosGestor)
	BEGIN
		INSERT INTO Synonym_SYS_GrupoPermissao
		(
			gru_id, 
			sis_id, 
			mod_id, 
			grp_consultar, 
			grp_inserir, 
			grp_alterar, 
			grp_excluir
		)
		VALUES
		(
			@gru_idSecretarioEscolar,
			@sis_id,
			@modIdDocumentosGestor,
			1,
			0,
			0,
			0
		)
	END
	ELSE
	BEGIN
		UPDATE Synonym_SYS_GrupoPermissao
			SET grp_consultar = 1, 
				grp_inserir = 0, 
				grp_alterar = 0, 
				grp_excluir = 0
		WHERE 
			gru_id = @gru_idSecretarioEscolar AND
			sis_id = @sis_id AND
			mod_id = @modIdDocumentosGestor
	END

	IF NOT EXISTS (SELECT mod_id FROM Synonym_SYS_GrupoPermissao WHERE gru_id = @gru_idSecretarioEscolar AND sis_id = @sis_id AND mod_id= @modIdRelAlunosBaixaFrequencia)
	BEGIN
		INSERT INTO Synonym_SYS_GrupoPermissao
		(
			gru_id, 
			sis_id, 
			mod_id, 
			grp_consultar, 
			grp_inserir, 
			grp_alterar, 
			grp_excluir
		)
		VALUES
		(
			@gru_idSecretarioEscolar,
			@sis_id,
			@modIdRelAlunosBaixaFrequencia,
			1,
			0,
			0,
			0
		)
	END
	ELSE
	BEGIN
		UPDATE Synonym_SYS_GrupoPermissao
			SET grp_consultar = 1, 
				grp_inserir = 0, 
				grp_alterar = 0, 
				grp_excluir = 0
		WHERE 
			gru_id = @gru_idSecretarioEscolar AND
			sis_id = @sis_id AND
			mod_id = @modIdRelAlunosBaixaFrequencia
	END

	IF NOT EXISTS (SELECT mod_id FROM Synonym_SYS_GrupoPermissao WHERE gru_id = @gru_idSecretarioEscolar AND sis_id = @sis_id AND mod_id= @modIdRelAtaFinalResultados)
	BEGIN
		INSERT INTO Synonym_SYS_GrupoPermissao
		(
			gru_id, 
			sis_id, 
			mod_id, 
			grp_consultar, 
			grp_inserir, 
			grp_alterar, 
			grp_excluir
		)
		VALUES
		(
			@gru_idSecretarioEscolar,
			@sis_id,
			@modIdRelAtaFinalResultados,
			1,
			0,
			0,
			0
		)
	END
	ELSE
	BEGIN
		UPDATE Synonym_SYS_GrupoPermissao
			SET grp_consultar = 1, 
				grp_inserir = 0, 
				grp_alterar = 0, 
				grp_excluir = 0
		WHERE 
			gru_id = @gru_idSecretarioEscolar AND
			sis_id = @sis_id AND
			mod_id = @modIdRelAtaFinalResultados
	END

	IF NOT EXISTS (SELECT mod_id FROM Synonym_SYS_GrupoPermissao WHERE gru_id = @gru_idSecretarioEscolar AND sis_id = @sis_id AND mod_id= @modIdRelAtaFinalEnriquecimentoCurr)
	BEGIN
		INSERT INTO Synonym_SYS_GrupoPermissao
		(
			gru_id, 
			sis_id, 
			mod_id, 
			grp_consultar, 
			grp_inserir, 
			grp_alterar, 
			grp_excluir
		)
		VALUES
		(
			@gru_idSecretarioEscolar,
			@sis_id,
			@modIdRelAtaFinalEnriquecimentoCurr,
			1,
			0,
			0,
			0
		)
	END
	ELSE
	BEGIN
		UPDATE Synonym_SYS_GrupoPermissao
			SET grp_consultar = 1, 
				grp_inserir = 0, 
				grp_alterar = 0, 
				grp_excluir = 0
		WHERE 
			gru_id = @gru_idSecretarioEscolar AND
			sis_id = @sis_id AND
			mod_id = @modIdRelAtaFinalEnriquecimentoCurr
	END

	IF NOT EXISTS (SELECT mod_id FROM Synonym_SYS_GrupoPermissao WHERE gru_id = @gru_idSecretarioEscolar AND sis_id = @sis_id AND mod_id= @modIdRelAlunosAusenciaRegFechamento)
	BEGIN
		INSERT INTO Synonym_SYS_GrupoPermissao
		(
			gru_id, 
			sis_id, 
			mod_id, 
			grp_consultar, 
			grp_inserir, 
			grp_alterar, 
			grp_excluir
		)
		VALUES
		(
			@gru_idSecretarioEscolar,
			@sis_id,
			@modIdRelAlunosAusenciaRegFechamento,
			1,
			0,
			0,
			0
		)
	END
	ELSE
	BEGIN
		UPDATE Synonym_SYS_GrupoPermissao
			SET grp_consultar = 1, 
				grp_inserir = 0, 
				grp_alterar = 0, 
				grp_excluir = 0
		WHERE 
			gru_id = @gru_idSecretarioEscolar AND
			sis_id = @sis_id AND
			mod_id = @modIdRelAlunosAusenciaRegFechamento
	END

-- Fechar transação
SET XACT_ABORT OFF
COMMIT TRANSACTION