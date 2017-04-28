
	EXEC MS_InsereRelatorio
		@rlt_id = 318 -- ID do relatório. (Obrigatório, igual ao enumerador do sistema)
		,@rlt_nome = 'RelatorioObjetoAprendizagem' -- Nome do relatorio. (Obrigatório, igual a descricção do enumerador do sistema)

	EXEC MS_InsereRelatorio
		@rlt_id = 319 -- ID do relatório. (Obrigatório, igual ao enumerador do sistema)
		,@rlt_nome = 'AlunosJustificativaFalta' -- Nome do relatorio. (Obrigatório, igual a descricção do enumerador do sistema)

	EXEC MS_InsereRelatorio
		@rlt_id = 320 -- ID do relatório. (Obrigatório, igual ao enumerador do sistema)
		,@rlt_nome = 'DivergenciasAulasPrevistas' -- Nome do relatorio. (Obrigatório, igual a descricção do enumerador do sistema)
