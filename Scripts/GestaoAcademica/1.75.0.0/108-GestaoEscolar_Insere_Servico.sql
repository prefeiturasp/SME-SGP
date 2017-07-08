USE [GestaoPedagogica]
GO

--Iniciar transação
BEGIN TRANSACTION
SET XACT_ABORT ON

	EXEC MS_InsereServico
		@ser_id = 55,
		@ser_nome = 'Fechamento - Preenchimento frequência',
		@ser_nomeProcedimento = 'MS_JOB_ProcessamentoPreenchimentoFrequencia',
		@ser_ativo = 1,
		@ser_descricao = 'Processa o preenchimento de frequência, conta as aulas sem a flag efetivado. Utiliza a fila do fechamento e faz o pré-processamento para geração de alerta.'

	EXEC MS_InsereServico
		@ser_id = 56,
		@ser_nome = 'Fechamento - Alunos com baixa frequência e faltas consecutivas',
		@ser_nomeProcedimento = 'MS_JOB_ProcessamentoAlunosFrequencia',
		@ser_ativo = 1,
		@ser_descricao = 'Processa os alunos com baixa frequência e com faltas consecutivas. Utiliza a fila do fechamento e faz o pré-processamento para geração de alerta.'
		
-- Fechar transação	
SET XACT_ABORT OFF
COMMIT TRANSACTION