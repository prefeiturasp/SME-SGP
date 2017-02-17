USE [GestaoPedagogica]
GO

--Iniciar transação
BEGIN TRANSACTION
SET XACT_ABORT ON

	DECLARE @entId AS UNIQUEIDENTIFIER;
	SELECT TOP 1 @entId = sse.ent_id 
	FROM 
		Synonym_SYS_SistemaEntidade AS sse WITH(NOLOCK)
		INNER JOIN  Synonym_SYS_Sistema AS ss WITH(NOLOCK)
			ON sse.sis_id = ss.sis_id
	WHERE 
		ss.sis_nome = ' SGP'

	DECLARE @areaConhecimento VARCHAR(MAX) = 'Fora da rede'

	IF (NOT EXISTS (SELECT aco_id FROM ACA_AreaConhecimento WITH(NOLOCK) WHERE aco_nome = @areaConhecimento AND aco_situacao <> 3))
	BEGIN
		DECLARE @ordem INT = (SELECT MAX(Aco.aco_ordem) + 1
								FROM ACA_AreaConhecimento AS Aco WITH(NOLOCK) 
								WHERE Aco.aco_tipoBaseGeral = 1
									AND Aco.aco_tipoBase = 1
									AND Aco.aco_situacao <> 3)

		--Atualiza a ordem das áreas de outras bases.
		UPDATE ACA_AreaConhecimento
		SET aco_ordem = (aco_ordem + 1)
		WHERE
			aco_situacao <> 3
			AND aco_ordem >= @ordem

		--Insere o registro da área de fora da rede
		INSERT INTO ACA_AreaConhecimento(aco_nome, aco_tipoBaseGeral, aco_tipoBase, aco_ordem, aco_situacao, aco_dataCriacao, aco_dataAlteracao)
		VALUES (@areaConhecimento, 1, 1, @ordem, 1, GETDATE(), GETDATE())
	END

	-- Insere o parametro
	DECLARE @aco_id INT = (SELECT TOP 1 aco_id FROM ACA_AreaConhecimento WITH(NOLOCK) WHERE aco_nome = @areaConhecimento AND aco_situacao <> 3)
	DECLARE @valor VARCHAR(MAX) = CAST(@aco_id AS VARCHAR(MAX))

	EXEC MS_InsereParametroAcademico
		@pac_chave = 'EXIBE_DISCIPLINA_FORA_GRADE_HISTORICO',
		@pac_valor = @valor,
		@pac_descricao = 'Área de conhecimento para disciplinas de fora da rede',
		@pac_obrigatorio = 1,
		@ent_id = @entId
    

--Fechar transação
SET XACT_ABORT OFF
COMMIT TRANSACTION