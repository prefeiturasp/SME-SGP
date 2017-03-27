
	EXEC MS_InsereServico
		@ser_id = 52,
		@ser_nome = 'Processa os dados para a sugestão das aulas previstas.',
		@ser_nomeProcedimento = 'MS_JOB_ProcessamentoSugestaoAulasPrevistas',
		@ser_ativo = 1
	
	EXEC MS_InsereServico
		@ser_id = 53,
		@ser_nome = 'Processa os dados para a sugestão das aulas previstas, para evento cadastrado para toda a rede.',
		@ser_nomeProcedimento = 'MS_JOB_ProcessamentoSugestaoAulasPrevistas_TodaRede',
		@ser_ativo = 1
