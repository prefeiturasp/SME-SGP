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
			
	EXEC MS_InsereParametroAcademico
		@pac_chave = 'PREENCHER_LOGIN_SENHA_AUTOMATICAMENTE_COLABORADORES_DOCENTES' 
		,@pac_valor = 'True' 
		,@pac_descricao = 'Preencher login e senha automaticamente no cadastro de colaboradores e docentes'
		,@pac_obrigatorio = 1 
		,@ent_id = @entId

	EXEC MS_InsereParametroAcademico
		@pac_chave = 'EXIBIR_ALERTA_AULA_SEM_PLANO_ENSINO_INFANTIL' 
		,@pac_valor = 'False' 
		,@pac_descricao = 'Exibir alerta de aula criada sem plano, para o ensino infantil'
		,@pac_obrigatorio = 1 
		,@ent_id = @entId

	EXEC MS_InsereParametroAcademico
		@pac_chave = 'EXIBIR_ABA_PLANEJAMENTO_PLANO_CICLO_ENSINO_INFANTIL' 
		,@pac_valor = 'True' 
		,@pac_descricao = 'Exibir no planejamento a aba plano do ciclo/série para o ensino infantil'
		,@pac_obrigatorio = 1 
		,@ent_id = @entId

	EXEC MS_InsereParametroAcademico
		@pac_chave = 'EXIBIR_ABA_PLANEJAMENTO_PLANO_ANUAL_ENSINO_INFANTIL' 
		,@pac_valor = 'True' 
		,@pac_descricao = 'Exibir no planejamento a aba plano anual para o ensino infantil'
		,@pac_obrigatorio = 1 
		,@ent_id = @entId

	EXEC MS_InsereParametroAcademico
		@pac_chave = 'EXIBIR_ABA_PLANEJAMENTO_PLANO_ALUNO_ENSINO_INFANTIL' 
		,@pac_valor = 'True' 
		,@pac_descricao = 'Exibir no planejamento a aba plano para o aluno para o ensino infantil'
		,@pac_obrigatorio = 1 
		,@ent_id = @entId

-- Fechar transação	
SET XACT_ABORT OFF
COMMIT TRANSACTION

GO