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
		@rlt_id = 327 -- ID do relatório. (Obrigatório, igual ao enumerador do sistema)
		,@rlt_nome = 'RelatorioAcoesRealizadas' -- Nome do relatorio. (Obrigatório, igual a descricção do enumerador do sistema)

	-- Insere o relatório no GestãoEscolar
	EXEC MS_InsereRelatorio
		@rlt_id = 326 -- ID do relatório. (Obrigatório, igual ao enumerador do sistema)
		,@rlt_nome = 'GraficoJustificativaFalta' -- Nome do relatorio. (Obrigatório, igual a descricção do enumerador do sistema)

	EXEC MS_InsereRelatorio
		@rlt_id = 325 -- ID do relatório. (Obrigatório, igual ao enumerador do sistema)
		,@rlt_nome = 'RelatorioGeralAtendimento' -- Nome do relatorio. (Obrigatório, igual a descricção do enumerador do sistema)


	-- Alteração na opção do menudo do relatório de docente
	update cfg_relatoriodocumentodocente 
		set rdd_nomeDocumento = 'Resumo das atividades desenvolvidas' 
		where rlt_id = 248	

-- Fechar transação
SET XACT_ABORT OFF
COMMIT TRANSACTION
GO