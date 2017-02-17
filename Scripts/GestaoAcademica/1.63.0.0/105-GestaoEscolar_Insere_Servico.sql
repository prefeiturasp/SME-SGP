USE [GestaoPedagogica]
GO

--Iniciar transação
BEGIN TRANSACTION
SET XACT_ABORT ON

	EXEC MS_InsereServico
		@ser_id = 48,
		@ser_nome = 'Faz o pré procesamento das pendências de aulas sem plano',
		@ser_nomeProcedimento = 'MS_JOB_ProcessamentoPendenciaAulas',
		@ser_ativo = 1

	EXEC MS_InsereServico
		@ser_id = 49,
		@ser_nome = 'Processa a remoção das faltas com justificativa de abono.',
		@ser_nomeProcedimento = 'MS_JOB_ProcessamentoAbonoFalta',
		@ser_ativo = 1

	EXEC MS_InsereServico
		@ser_id = 50,
		@ser_nome = 'Processa a abertura/fechamento das turmas dos anos anteriores.',
		@ser_nomeProcedimento = 'MS_JOB_ProcessamentoAberturaTurmaAnosAnteriores',
		@ser_ativo = 1
		
-- Fechar transação	
SET XACT_ABORT OFF
COMMIT TRANSACTION