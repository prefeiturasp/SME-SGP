USE [GestaoPedagogica]
GO

--Iniciar transação
BEGIN TRANSACTION
SET XACT_ABORT ON

	IF (NOT EXISTS (SELECT TOP 1 1 FROM SYS_ServicosLogExecucao WITH(NOLOCK) WHERE ser_id = 54))
	BEGIN

		DECLARE @dataInicio DATETIME = GETDATE();

		-- Executa o serviço para todos os registros
		EXEC MS_JOB_ProcessamentoDivergenciasAulasPrevistas

		-- Insere registros na tabela SYS_ServicosLogExecucao
		-- para referência de data na próxima execução do serviço.
		INSERT INTO SYS_ServicosLogExecucao (ser_id, sle_dataInicioExecucao, sle_dataFimExecucao)
		VALUES (54, @dataInicio, GETDATE())

	END
	
-- Fechar transação	
SET XACT_ABORT OFF
COMMIT TRANSACTION