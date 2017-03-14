USE [GestaoPedagogica]
GO

--Iniciar transação
BEGIN TRANSACTION
SET XACT_ABORT ON

	EXEC MS_InsereServico
		@ser_id = 52,
		@ser_nome = 'Processa os dados para a sugestão das aulas previstas.',
		@ser_nomeProcedimento = 'MS_JOB_ProcessamentoSugestaoAulasPrevistas',
		@ser_ativo = 1
		
-- Fechar transação	
SET XACT_ABORT OFF
COMMIT TRANSACTION