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

	EXEC MS_InsereParametroAcademico
		@pac_chave = 'EXIBIR_IMPORTACAO_FECHAMENTO_CADASTRO_ESCOLA' -- Chave do parâmetro. (Obrigatório)
		,@pac_valor = 'False' -- Valor do parâmetro. (Obrigatório)
		,@pac_descricao = 'Permitir a exibição de Importação do Fechamento no cadastro de escola.' -- Descrição do parâmetro. (Obrigatório)
		,@pac_obrigatorio = 1 -- indica se o parâmetro é obrigatório no sistema. (Obrigatório)
		,@ent_id = @entId

-- Fechar transação	
SET XACT_ABORT OFF
COMMIT TRANSACTION

GO