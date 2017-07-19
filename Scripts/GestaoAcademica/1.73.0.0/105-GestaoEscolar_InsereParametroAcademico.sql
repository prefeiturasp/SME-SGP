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
		@pac_chave = 'TIPO_MODALIDADE_EJA' -- Chave do parâmetro. (Obrigatório)
		,@pac_valor = '6' -- Valor do parâmetro. (Obrigatório)
		,@pac_descricao = 'Modalidade de ensino que se refere a modalidade EJA.' -- Descrição do parâmetro. (Obrigatório)
		,@pac_obrigatorio = 0 -- indica se o parâmetro é obrigatório no sistema. (Obrigatório)
		,@ent_id = @entId

	EXEC MS_InsereParametroAcademico
		@pac_chave = 'TIPO_MODALIDADE_CIEJA' -- Chave do parâmetro. (Obrigatório)
		,@pac_valor = '8' -- Valor do parâmetro. (Obrigatório)
		,@pac_descricao = 'Modalidade de ensino que se refere a modalidade CIEJA.' -- Descrição do parâmetro. (Obrigatório)
		,@pac_obrigatorio = 0 -- indica se o parâmetro é obrigatório no sistema. (Obrigatório)
		,@ent_id = @entId
		
	INSERT INTO ACA_TipoEvento (tev_nome, tev_periodoCalendario, tev_situacao, tev_dataCriacao, tev_dataAlteracao)
	VALUES ('Abertura de sugestões de currículos', 0, 1, GETDATE(), GETDATE())
	
	DECLARE @tev_id INT = (SELECT SCOPE_IDENTITY());
	
	EXEC MS_InsereParametroAcademico
		@pac_chave = 'TIPO_EVENTO_ABERTURA_SUGESTOES' -- Chave do parâmetro. (Obrigatório)
		,@pac_valor = @tev_id -- Valor do parâmetro. (Obrigatório)
		,@pac_descricao = 'Tipo de evento de abertura de período para cadastro de sugestões no currículo' -- Descrição do parâmetro. (Obrigatório)
		,@pac_obrigatorio = 0 -- indica se o parâmetro é obrigatório no sistema. (Obrigatório)
		,@ent_id = @entId
	
-- Fechar transação	
SET XACT_ABORT OFF
COMMIT TRANSACTION

GO