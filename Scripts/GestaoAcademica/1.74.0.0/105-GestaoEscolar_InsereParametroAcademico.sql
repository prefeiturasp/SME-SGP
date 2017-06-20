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
	
	Declare @tde_id as uniqueidentifier
		SELECT @tde_id = tde_id FROM Synonym_PES_TipoDeficiencia WHERE tde_nome = 'DEFICIENCIA MULTIPLA'
		
		EXEC MS_InsereParametroAcademico
		@pac_chave = 'DEFICIENCIA_MULTIPLA' -- Chave do parâmetro. (Obrigatório)
		,@pac_valor = @tde_id -- Valor do parâmetro. (Obrigatório)
		,@pac_descricao = 'Indica a deficiência do tipo múltipla, que terá deficiências dependentes.' -- Descrição do parâmetro. (Obrigatório)
		,@pac_obrigatorio = 0 -- indica se o parâmetro é obrigatório no sistema. (Obrigatório)
		,@ent_id = @entId
	
-- Fechar transação	
SET XACT_ABORT OFF
COMMIT TRANSACTION

GO