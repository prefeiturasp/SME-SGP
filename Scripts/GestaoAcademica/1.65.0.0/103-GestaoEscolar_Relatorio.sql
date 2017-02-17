USE [GestaoPedagogica]
GO

--Iniciar transação
BEGIN TRANSACTION
SET XACT_ABORT ON

	EXEC MS_InsereRelatorio
		@rlt_id = 317 -- ID do relatório. (Obrigatório, igual ao enumerador do sistema)
		,@rlt_nome = 'DivergenciasRematriculas' -- Nome do relatorio. (Obrigatório, igual a descricção do enumerador do sistema)

-- Fechar transação
SET XACT_ABORT OFF
COMMIT TRANSACTION
GO