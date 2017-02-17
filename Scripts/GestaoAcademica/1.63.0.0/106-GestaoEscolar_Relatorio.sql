USE [GestaoPedagogica]
GO

--Iniciar transação
BEGIN TRANSACTION
SET XACT_ABORT ON

	EXEC MS_InsereRelatorio
		  @rlt_id = 316
		, @rlt_nome = 'AulasSemPlanoAula'

-- Fechar transação
SET XACT_ABORT OFF
COMMIT TRANSACTION
GO