USE [GestaoPedagogica]
GO

--Iniciar transação
BEGIN TRANSACTION
SET XACT_ABORT ON

	INSERT INTO SYS_ServicosLogExecucao (ser_id, sle_dataInicioExecucao)
	VALUES (52, GETDATE())

	DECLARE @sle_id UNIQUEIDENTIFIER
	SELECT TOP 1 @sle_id = sle_id FROM SYS_ServicosLogExecucao WHERE ser_id = 52 ORDER BY sle_dataInicioExecucao DESC

	EXEC MS_JOB_ProcessamentoSugestaoAulasPrevistas 0

	UPDATE SYS_ServicosLogExecucao 
	SET sle_dataFimExecucao = GETDATE()
	WHERE sle_id = @sle_id
	
-- Fechar transação	
SET XACT_ABORT OFF
COMMIT TRANSACTION