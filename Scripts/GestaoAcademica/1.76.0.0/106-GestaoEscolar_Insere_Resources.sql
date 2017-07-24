USE [GestaoPedagogica]
GO

BEGIN TRANSACTION 
SET XACT_ABORT ON   

	/***************
		Copiar do exemplo abaixo.
	****************

        -- Insere resources. 
        EXEC MS_InsereResource 
            @rcr_chave = 'Relatorios.UCRelatorios.lblMessageLayout.MsgAviso' 
            , @rcr_NomeResource = 'WebControls'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'A visualização do texto na tela abaixo não corresponde, necessariamente, ao formato no qual ele será impresso. Este formato segue as normas estabelecidas pela Secretaria Municipal de Educação.'

	*/

	EXEC MS_InsereResource 
		@rcr_chave = 'LogNotificacoes.Busca.DataInicioInvalida' 
		, @rcr_NomeResource = 'Relatorios'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Data inicial deve estar no formato DD/MM/AAAA.'

	EXEC MS_InsereResource 
		@rcr_chave = 'LogNotificacoes.Busca.DataInicioMaiorHoje' 
		, @rcr_NomeResource = 'Relatorios'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Data inicial do período não pode ser posterior à data atual.'

	EXEC MS_InsereResource 
		@rcr_chave = 'LogNotificacoes.Busca.DataFimInvalida' 
		, @rcr_NomeResource = 'Relatorios'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Data final deve estar no formato DD/MM/AAAA.'

	EXEC MS_InsereResource 
		@rcr_chave = 'LogNotificacoes.Busca.DataFimMaiorHoje' 
		, @rcr_NomeResource = 'Relatorios'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Data final do período não pode ser posterior à data atual.'

	EXEC MS_InsereResource 
		@rcr_chave = 'LogNotificacoes.Busca.DataFimMenorInicio' 
		, @rcr_NomeResource = 'Relatorios'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Data final do período deve ser maior ou igual à data inicial.'

	EXEC MS_InsereResource 
		@rcr_chave = 'LogNotificacoes.Busca.btnGerarRel.Text' 
		, @rcr_NomeResource = 'Relatorios'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Gerar relatório'

	EXEC MS_InsereResource 
		@rcr_chave = 'LogNotificacoes.Busca.btnLimparPesquisa.Text' 
		, @rcr_NomeResource = 'Relatorios'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Limpar pesquisa'

	EXEC MS_InsereResource 
		@rcr_chave = 'ControleTurma.DiarioClasse.MensagemAulaSemPlanoAula' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Se esse ícone estiver ao lado de um plano de aula, significa que esse plano de aula não foi preenchido ou está sem objeto de conhecimento. Para regularizar, é necessário que o plano dessa aula seja preenchido e que tenha algum objeto de conhecimento.'

	EXEC MS_InsereResource 
		@rcr_chave = 'ControleTurma.Listao.MensagemAulaSemPlanoAula' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Se esse ícone estiver ao lado de um plano de aula, significa que esse plano de aula não foi preenchido. Para regularizar, é necessário que o plano dessa aula seja preenchido.'

	EXEC MS_InsereResource 
		@rcr_chave = 'ControleTurma.DiarioClasse.imgSemPlanoAula' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Plano de aula não preenchido ou sem objeto de conhecimento'

	EXEC MS_InsereResource 
		@rcr_chave = 'ControleTurma.Listao.imgSemPlanoAula' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Plano de aula não preenchido'

-- Fechar transação     
SET XACT_ABORT OFF 
COMMIT TRANSACTION



