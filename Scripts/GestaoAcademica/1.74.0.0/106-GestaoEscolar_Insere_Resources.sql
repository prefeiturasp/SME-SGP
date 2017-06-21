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
        @rcr_chave = 'UCDadosBoletim.lblAEETitulo.Text' 
        , @rcr_NomeResource = 'UserControl'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Atendimento educacional especializado'

	EXEC MS_InsereResource 
        @rcr_chave = 'UCAlunoEfetivacaoObservacaoGeral.lblParecerFinalAEE.Text' 
        , @rcr_NomeResource = 'UserControl'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Parecer Final'

	EXEC MS_InsereResource 
        @rcr_chave = 'UCAlunoEfetivacaoObservacaoGeral.lblPorcentagemFreqAEE.Text' 
        , @rcr_NomeResource = 'UserControl'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = '% Freq.'

	EXEC MS_InsereResource 
        @rcr_chave = 'ControleTurma.Alunos.btnRelatorioRP.ToolTip' 
        , @rcr_NomeResource = 'Academico'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Anotações da recuperação paralela'
		
	EXEC MS_InsereResource 
        @rcr_chave = 'CLS_RelatorioAtendimentoBO.CLS_RelatorioAtendimentoTipo.AEE' 
        , @rcr_NomeResource = 'Enumerador'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'AEE'

	EXEC MS_InsereResource 
			@rcr_chave = 'CLS_RelatorioAtendimentoBO.CLS_RelatorioAtendimentoTipo.NAAPA' 
			, @rcr_NomeResource = 'Enumerador'
			, @rcr_cultura = 'pt-BR'
			, @rcr_codigo = 0 
			, @rcr_valorPadrao = 'NAAPA'

	EXEC MS_InsereResource 
			@rcr_chave = 'CLS_RelatorioAtendimentoBO.CLS_RelatorioAtendimentoTipo.RP' 
			, @rcr_NomeResource = 'Enumerador'
			, @rcr_cultura = 'pt-BR'
			, @rcr_codigo = 0 
			, @rcr_valorPadrao = 'Recuperação paralela'

	EXEC MS_InsereResource 
			@rcr_chave = 'CLS_RelatorioPreenchimentoAlunoTurmaDisciplinaBO.RelatorioPreenchimentoAlunoSituacao.Rascunho' 
			, @rcr_NomeResource = 'Enumerador'
			, @rcr_cultura = 'pt-BR'
			, @rcr_codigo = 0 
			, @rcr_valorPadrao = 'Rascunho'

	EXEC MS_InsereResource 
			@rcr_chave = 'CLS_RelatorioPreenchimentoAlunoTurmaDisciplinaBO.RelatorioPreenchimentoAlunoSituacao.Finalizado' 
			, @rcr_NomeResource = 'Enumerador'
			, @rcr_cultura = 'pt-BR'
			, @rcr_codigo = 0 
			, @rcr_valorPadrao = 'Finalizado'

	EXEC MS_InsereResource 
			@rcr_chave = 'CLS_RelatorioPreenchimentoAlunoTurmaDisciplinaBO.RelatorioPreenchimentoAlunoSituacao.Aprovado' 
			, @rcr_NomeResource = 'Enumerador'
			, @rcr_cultura = 'pt-BR'
			, @rcr_codigo = 0 
			, @rcr_valorPadrao = 'Aprovado'

	EXEC MS_InsereResource 
        @rcr_chave = 'ControleTurma.Alunos.btnRelatorioAEE.ToolTip' 
        , @rcr_NomeResource = 'Academico'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Relatórios do AEE'

-- Fechar transação     
SET XACT_ABORT OFF 
COMMIT TRANSACTION



