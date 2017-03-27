USE [GestaoPedagogica]
GO

--Iniciar transação
BEGIN TRANSACTION
SET XACT_ABORT ON

	EXEC MS_InsereServico
		@ser_id = 54,
		@ser_nome = 'Processa as divergências entre registros de aulas dadas e aulas previstas.',
		@ser_nomeProcedimento = 'MS_JOB_ProcessamentoDivergenciasAulasPrevistas',
		@ser_ativo = 1
		
-- Fechar transação	
SET XACT_ABORT OFF
COMMIT TRANSACTION