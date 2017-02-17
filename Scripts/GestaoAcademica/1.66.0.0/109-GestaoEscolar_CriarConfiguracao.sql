USE [GestaoPedagogica]
GO

--Iniciar transação
BEGIN TRANSACTION
SET XACT_ABORT ON

	DECLARE @nomeSistema VARCHAR(100) = ' SGP'
	DECLARE @sis_id INT = (SELECT TOP 1 sis_id FROM Synonym_SYS_Sistema AS Sis WITH(NOLOCK) WHERE Sis.sis_nome = @nomeSistema AND Sis.sis_situacao <> 3)
    DECLARE @ent_id UNIQUEIDENTIFIER = (SELECT TOP 1 ent_id FROM Synonym_SYS_SistemaEntidade WITH(NOLOCK) WHERE sis_id = @sis_id)
	DECLARE @dataAtual DATETIME = GETDATE()
	DECLARE @tne_id INT = (SELECT TOP 1 Tne.tne_id FROM ACA_TipoNivelEnsino AS Tne WITH(NOLOCK) WHERE Tne.tne_nome = 'Educação Infantil' AND Tne.tne_situacao <> 3)
	DECLARE @nomeDisciplina VARCHAR(100)
	
	DECLARE @tvi_idTerceirizado INT 

	DECLARE @tbCargos AS TABLE (
		codigo VARCHAR(20) NOT NULL DEFAULT(''),
		nomeCargo VARCHAR(100) NOT NULL,
		descricao TEXT NOT NULL DEFAULT(''),
		grupoPadrao VARCHAR(100) NULL,
		cargoDocente BIT NOT NULL,
		tipo TINYINT NOT NULL,
		controleIntegracao BIT NOT NULL DEFAULT(0),
		disciplina VARCHAR(100) NULL
	)

	--Atualiza os parâmetros acadêmicos:
	--Controlar colaboradores somente por integração - Não
	UPDATE ACA_ParametroAcademico 
	SET pac_valor = 'False' 
	WHERE pac_chave = 'CONTROLAR_COLABORADOR_APENAS_INTEGRACAO'

	--Controlar colaboradores e vínculos integrados e virtuais - Sim
	UPDATE ACA_ParametroAcademico 
	SET pac_valor = 'True' 
	WHERE pac_chave = 'CONTROLAR_COLABORADOR_VINCULO_INTEGRADO_VIRTUAL'

	--Insere o ciclo
	IF NOT EXISTS(SELECT TOP 1 Tci.tci_id FROM ACA_TipoCiclo AS Tci WITH(NOLOCK) WHERE Tci.tci_nome = 'Infantil' AND Tci.tci_situacao <> 3)
	BEGIN
		INSERT INTO ACA_TipoCiclo (tci_nome, tci_situacao, tci_dataCriacao, tci_dataAlteracao, tci_exibirBoletim, tci_ordem, tci_layout)
		VALUES ('Infantil', 1, @dataAtual, @dataAtual, 1, 0, 'cicloInfantil')
	END	
	
	--Insere o tipo de vinculo
	IF NOT EXISTS(SELECT TOP 1 Tvi.tvi_id FROM RHU_TipoVinculo AS Tvi WITH(NOLOCK) WHERE Tvi.tvi_nome = 'TERCEIRIZADO' AND Tvi.tvi_situacao <> 3)
	BEGIN
		INSERT INTO RHU_TipoVinculo (tvi_nome, tvi_descricao, tvi_horasSemanais, tvi_minutosAlmoco, tvi_horarioMinEntrada, tvi_horarioMaxSaida, tvi_codIntegracao, tvi_situacao, tvi_dataCriacao, tvi_dataAlteracao, ent_id)
		VALUES ('TERCEIRIZADO', 'TERCEIRIZADO', NULL, NULL, NULL, NULL, NULL, 1, GETDATE(), GETDATE(), @ent_id) 
	END
	SET @tvi_idTerceirizado = (SELECT TOP 1 Tvi.tvi_id FROM RHU_TipoVinculo AS Tvi WITH(NOLOCK) WHERE Tvi.tvi_nome = 'TERCEIRIZADO' AND Tvi.tvi_situacao <> 3)

	--Insere os tipos de disciplina
	SET @nomeDisciplina = 'Conceito Global (Berçario/Minigrupo)'
	IF NOT EXISTS(SELECT TOP 1 Tds.tds_id FROM ACA_TipoDisciplina AS Tds WITH(NOLOCK) WHERE Tds.tds_nome = @nomeDisciplina AND Tds.tds_situacao <> 3)
	BEGIN
		INSERT INTO ACA_TipoDisciplina (tne_id, mds_id, tds_nome, tds_base, tds_situacao, tds_dataCriacao, tds_dataAlteracao, tds_ordem, tds_nomeDisciplinaEspecial, aco_id, tds_tipo, tds_qtdeDisciplinaRelacionada)
		VALUES (@tne_id, NULL, @nomeDisciplina, 1, 1, @dataAtual, @dataAtual, 1, NULL, NULL, 1, 0)
	END

	SET @nomeDisciplina = 'Conceito Global (Infantil I e II)'
	IF NOT EXISTS(SELECT TOP 1 Tds.tds_id FROM ACA_TipoDisciplina AS Tds WITH(NOLOCK) WHERE Tds.tds_nome = @nomeDisciplina AND Tds.tds_situacao <> 3)
	BEGIN
		INSERT INTO ACA_TipoDisciplina (tne_id, mds_id, tds_nome, tds_base, tds_situacao, tds_dataCriacao, tds_dataAlteracao, tds_ordem, tds_nomeDisciplinaEspecial, aco_id, tds_tipo, tds_qtdeDisciplinaRelacionada)
		VALUES (@tne_id, NULL, @nomeDisciplina, 1, 1, @dataAtual, @dataAtual, 2, NULL, NULL, 1, 0)
	END
	
	--Insere os cargos
	INSERT INTO @tbCargos (nomeCargo, grupoPadrao, cargoDocente, tipo, disciplina)
	VALUES 
		('Professor Ensino Infantil Conveniado', 'padrao_docente_terceirizado', 1, 1, 'Conceito Global (Berçario/Minigrupo)'),
		('Diretor Escolar Conveniado', 'padrao_diretor_terceirizado', 0, 1, NULL),
		('Secretário Escolar Conveniado', 'padrao_secretario_terceirizado', 0, 1, NULL)

	INSERT INTO RHU_Cargo (crg_codigo, crg_nome, crg_descricao, tvi_id, crg_cargoDocente, crg_maxAulaSemana, crg_maxAulaDia, crg_codIntegracao, crg_especialista, crg_situacao, crg_dataCriacao, crg_dataAlteracao, ent_id, pgs_chave, crg_tipo, crg_controleIntegracao)
	SELECT C.codigo, C.nomeCargo, C.descricao, @tvi_idTerceirizado, C.cargoDocente, NULL, NULL, C.codigo, NULL, 1 AS crg_situacao, @dataAtual, @dataAtual, @ent_id, C.grupoPadrao, C.tipo, C.controleIntegracao
	FROM 
		@tbCargos C
	WHERE
		NOT EXISTS (
			SELECT TOP 1 Crg.crg_id 
			FROM RHU_Cargo AS Crg WITH(NOLOCK)
			WHERE 
				LTRIM(RTRIM(Crg.crg_nome)) = LTRIM(RTRIM(C.nomeCargo))
				AND Crg.crg_situacao <> 3
		)

	-- Insere a disciplina dos cargos
	INSERT INTO RHU_CargoDisciplina (crg_id, tds_id)
	SELECT Crg.crg_id, Tds.tds_id
	FROM 
		@tbCargos C
		INNER JOIN RHU_Cargo AS Crg WITH(NOLOCK)
			ON LTRIM(RTRIM(Crg.crg_nome)) = LTRIM(RTRIM(C.nomeCargo))
			AND Crg.crg_situacao <> 3
		INNER JOIN ACA_TipoDisciplina AS Tds WITH(NOLOCK)
			ON LTRIM(RTRIM(Tds.tds_nome)) = LTRIM(RTRIM(C.disciplina))
			AND Tds.tds_situacao <> 3
	WHERE
		C.disciplina IS NOT NULL
		AND NOT EXISTS (
			SELECT * 
			FROM RHU_CargoDisciplina Cd WITH(NOLOCK)
			WHERE
				Cd.crg_id = Crg.crg_id
				AND Cd.tds_id = Tds.tds_id
		)

	--Insere a carga horária para os cargos
	INSERT INTO RHU_CargaHoraria (ent_id, chr_descricao, chr_padrao, chr_especialista, crg_id, chr_cargaHorariaSemanal, chr_temposAula, chr_horasAula, chr_horasComplementares, chr_situacao, chr_dataCriacao, chr_dataAlteracao)
	SELECT @ent_id, NULL, 0, NULL, Crg.crg_id, 0, NULL, NULL, NULL, 1 AS chr_situacao, @dataAtual, @dataAtual
	FROM 
		@tbCargos C
		INNER JOIN RHU_Cargo AS Crg WITH(NOLOCK)
			ON LTRIM(RTRIM(Crg.crg_nome)) = LTRIM(RTRIM(C.nomeCargo))
			AND Crg.crg_situacao <> 3
	WHERE
		NOT EXISTS (
			SELECT TOP 1 Crg.crg_id 
			FROM RHU_CargaHoraria AS Chr WITH(NOLOCK)
			WHERE 
				Crg.crg_id = LTRIM(RTRIM(Chr.crg_id))
				AND Crg.crg_situacao <> 3
		)

--Fechar transação
SET XACT_ABORT OFF
COMMIT TRANSACTION