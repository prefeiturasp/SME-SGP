USE [GestaoPedagogica]
GO

--Iniciar transação
BEGIN TRANSACTION
SET XACT_ABORT ON

	DECLARE @dataInicio DATETIME = GETDATE();

	-- Executa o serviço para as agendas geradas a partir de 2017
	EXEC MS_JOB_ProcessamentoSugestaoAulasPrevistas 0

	-- Insere registros na tabela SYS_ServicosLogExecucao
	-- para referência de data na próxima execução do serviço.
	DECLARE @sle_id1 UNIQUEIDENTIFIER, @sle_id2 UNIQUEIDENTIFIER

	INSERT INTO SYS_ServicosLogExecucao (ser_id, sle_dataInicioExecucao, sle_dataFimExecucao)
	VALUES (52, @dataInicio, GETDATE())

	INSERT INTO SYS_ServicosLogExecucao (ser_id, sle_dataInicioExecucao, sle_dataFimExecucao)
	VALUES (53, @dataInicio, GETDATE())
	
-- Fechar transação	
SET XACT_ABORT OFF
COMMIT TRANSACTION