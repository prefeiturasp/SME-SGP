USE [GestaoPedagogica]
GO

--Iniciar transação
BEGIN TRANSACTION
SET XACT_ABORT ON

	EXEC MS_InsereServico
		@ser_id = 57,
		@ser_nome = 'Sondagem - Análise de sondagem consolidada',
		@ser_nomeProcedimento = 'MS_JOB_ProcessamentoAnaliseSondagemConsolidada',
		@ser_ativo = 1,
		@ser_descricao = 'Processa os dados para o relatório de análise de sondagem consolidada.'
		
-- Fechar transação	
SET XACT_ABORT OFF
COMMIT TRANSACTION