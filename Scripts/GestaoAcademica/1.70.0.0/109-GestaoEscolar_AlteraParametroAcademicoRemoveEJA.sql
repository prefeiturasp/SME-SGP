USE [GestaoPedagogica]
GO

--Iniciar transação
BEGIN TRANSACTION
SET XACT_ABORT ON

	DECLARE @entId as uniqueidentifier;
	SELECT TOP 1 @entId = sse.ent_id 
	FROM 
		Synonym_SYS_SistemaEntidade AS sse WITH(NOLOCK)
		INNER JOIN  Synonym_SYS_Sistema AS ss WITH(NOLOCK)
			ON sse.sis_id = ss.sis_id
	WHERE 
		ss.sis_nome = ' SGP'

	/***************
		Copiar do exemplo abaixo.
	****************
	
	-- Insere o parâmetro academico no sistema.
	EXEC MS_InsereParametroAcademico
		@pac_chave = 'EXIBIR_NOTAFINAL_LANCAMENTO_AVALIACOES' -- Chave do parâmetro. (Obrigatório)
		,@pac_valor = 'False' -- Valor do parâmetro. (Obrigatório)
		,@pac_descricao = 'Permitir cálculo/lançamento da Média das Atividades Avaliativas.' -- Descrição do parâmetro. (Obrigatório)
		,@pac_obrigatorio = 0 -- indica se o parâmetro é obrigatório no sistema. (Obrigatório)
		,@ent_id = @entId

	*/
	
	DECLARE @tme_ids VARCHAR(100) = STUFF
									(
										(SELECT
											',' + CAST(tme.tme_id AS VARCHAR)
										FROM ACA_TipoModalidadeEnsino tme WITH(NOLOCK)
										WHERE
											tme.tme_nome IN ('EJA Especial', 'EJA Modular', 'CIEJA')
											AND tme.tme_idSuperior IS NOT NULL
											AND tme.tme_situacao <> 3
										FOR XML PATH(''))
									, 1, 1,'')

	DECLARE @pac_id INT;
	SELECT TOP 1 @pac_id = pac_id
	FROM ACA_ParametroAcademico WITH(NOLOCK)
	WHERE pac_chave = 'TIPO_MODALIDADES_EJA_REMOVER_RELATORIO'
	AND ISNULL(ent_id, @entId) = @entId
	AND pac_situacao <> 3

	IF (ISNULL(@pac_id, 0) > 0)
	BEGIN
		UPDATE ACA_ParametroAcademico
		SET pac_valor = @tme_ids
		WHERE pac_id = @pac_id
	END
	ELSE
	BEGIN
		EXEC MS_InsereParametroAcademico
			@pac_chave = 'TIPO_MODALIDADES_EJA_REMOVER_RELATORIO' -- Chave do parâmetro. (Obrigatório)
			,@pac_valor = @tme_ids -- Valor do parâmetro. (Obrigatório)
			,@pac_descricao = 'Modalidades de ensino que se referem à modalidade EJA para não aparecer nos relatórios.' -- Descrição do parâmetro. (Obrigatório)
			,@pac_obrigatorio = 0 -- indica se o parâmetro é obrigatório no sistema. (Obrigatório)
			,@ent_id = @entId
	END

-- Fechar transação	
SET XACT_ABORT OFF
COMMIT TRANSACTION