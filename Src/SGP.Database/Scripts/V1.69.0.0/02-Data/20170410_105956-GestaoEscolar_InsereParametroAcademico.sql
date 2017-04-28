
	DECLARE @entId as uniqueidentifier;
	SELECT TOP 1 @entId = sse.ent_id 
	FROM 
		Synonym_SYS_SistemaEntidade AS sse WITH(NOLOCK)
		INNER JOIN  Synonym_SYS_Sistema AS ss WITH(NOLOCK)
			ON sse.sis_id = ss.sis_id
	WHERE 
		ss.sis_nome = '$SystemName$'

	DECLARE @tne_id VARCHAR(5) = (SELECT TOP 1 CAST(tne_id AS VARCHAR(5)) FROM ACA_TipoNivelEnsino WITH(NOLOCK) WHERE tne_nome = 'Ensino Médio' AND tne_situacao <> 3)

	EXEC MS_InsereParametroAcademico
		@pac_chave = 'TIPO_NIVEL_ENSINO_MEDIO' -- Chave do parâmetro. (Obrigatório)
		,@pac_valor = @tne_id -- Valor do parâmetro. (Obrigatório)
		,@pac_descricao = 'Nível de ensino que se refere ao curso do ensino médio' -- Descrição do parâmetro. (Obrigatório)
		,@pac_obrigatorio = 1 -- indica se o parâmetro é obrigatório no sistema. (Obrigatório)
		,@ent_id = @entId
