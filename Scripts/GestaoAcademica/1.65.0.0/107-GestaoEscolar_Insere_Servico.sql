USE [GestaoPedagogica]
GO

--Iniciar transação
BEGIN TRANSACTION
SET XACT_ABORT ON

	EXEC MS_InsereServico
		@ser_id = 51,
		@ser_nome = 'Processa os dados para o relatório de divergências de rematrículas.',
		@ser_nomeProcedimento = 'MS_JOB_ProcessamentoDivergenciasRematriculas',
		@ser_ativo = 1
		
-- Fechar transação	
SET XACT_ABORT OFF
COMMIT TRANSACTION