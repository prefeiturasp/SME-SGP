USE [GestaoPedagogica]
GO

--Iniciar transação
BEGIN TRANSACTION
SET XACT_ABORT ON

	/***************
		Copiar do exemplo abaixo.
	****************

	-- Insere o relatório no GestãoEscolar
	EXEC MS_InsereRelatorio
		@rlt_id = 0 -- ID do relatório. (Obrigatório, igual ao enumerador do sistema)
		,@rlt_nome = '[Preencher]' -- Nome do relatorio. (Obrigatório, igual a descricção do enumerador do sistema)

	*/

	EXEC MS_InsereRelatorio
		@rlt_id = 323 -- ID do relatório. (Obrigatório, igual ao enumerador do sistema)
		,@rlt_nome = 'RelatorioSugestoesCurriculo' -- Nome do relatorio. (Obrigatório, igual a descricção do enumerador do sistema)

-- Fechar transação
SET XACT_ABORT OFF
COMMIT TRANSACTION
GO