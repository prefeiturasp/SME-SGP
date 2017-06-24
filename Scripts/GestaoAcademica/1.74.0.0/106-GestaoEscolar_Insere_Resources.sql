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

	EXEC MS_InsereResource 
        @rcr_chave = 'TUR_TurmaHorarioBO.SalvarTurmaHorario.ValidacaoTemposAula' 
        , @rcr_NomeResource = 'BLL'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'O componente curricular {0} ultrapassou a carga horária ({1}) na turma {2} em {3} aulas.'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.lblLegend.Text' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Relatório de atendimento'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.lblTitulo.Text' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Título *'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.rfvTitulo.ErrorMessage' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Título do relatório é obrigatório.'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.lblPeriodicidade.Text' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Periodicidade *'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.ddlPeriodicidade.msgSelecione' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = '-- Selecione uma peridiocidade --'

	EXEC MS_InsereResource 
        @rcr_chave = 'CLS_RelatorioAtendimentoBO.CLS_RelatorioAtendimentoPeriodicidade.Periodico' 
        , @rcr_NomeResource = 'Enumerador'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Periódico'

	EXEC MS_InsereResource 
        @rcr_chave = 'CLS_RelatorioAtendimentoBO.CLS_RelatorioAtendimentoPeriodicidade.Encerramento' 
        , @rcr_NomeResource = 'Enumerador'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Encerramento'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.cpvPeriodicidade.ErrorMessag' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Peridiocidade do relatório é obrigatória.'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.lblTipo.Text' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Tipo de relatório *'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.ddlTipo.msgSelecione' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = '-- Selecione um tipo --'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.cpvTipo.ErrorMessage' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Tipo de relatório é obrigatório.'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.chkExibeRacaCor.Text' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Permite editar raça/cor'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.chkExibeHipotese.Text' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Permite editar hipótese diagnóstica'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.lblTituloAnexo.Text' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Título do anexo'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.lblAnexo.Text' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Anexo'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.fupAnexo.ToolTip' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Arquivo anexo do relatório.'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.lblLegendQuestionario.Text' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Questionário'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.btnAdicionarQuestionario.Text' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Adicionar'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.btnCacelarQuestionario.Text' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Cancelar'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.gvQuestionario.EmptyDataText' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Nenhum questionário ligado ao relatório de atendimento.'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.gvQuestionario.HeaderTitulo' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Título'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.gvQuestionario.HeaderOrdem' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Ordem'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.grvQuestoes.HeaderExcluir' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Excluir'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.gvQuestionario.btnExcluir.ToolTip' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Excluir questionário do relatório.'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.lblLegendGrupo.Text' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Grupo de acesso'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.gvGrupo.EmptyDataText' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Nenhum grupo de acesso ligado ao relatório.'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.gvGrupo.HeaderNome' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Nome'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.gvGrupo.HeaderPermissaoConsulta' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Permissão de consulta'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.gvGrupo.HeaderPermissaoEdicao' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Permissão de edição'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.gvGrupo.HeaderPermissaoExclusao' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Permissão de exclusão'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.gvGrupo.HeaderPermissaoAprovacao' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Permissão de aprovação'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.lblLegendCargo.Text' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Cargo'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.gvCargo.EmptyDataText' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Nenhum cargo relacionado ao relatório.'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.gvCargo.HeaderDescricao' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Descrição'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.gvCargo.HeaderPermissaoConsulta' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Permissão de consulta'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.gvCargo.HeaderPermissaoEdicao' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Permissão de edição'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.gvCargo.HeaderPermissaoExclusao' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Permissão de exclusão'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.gvCargo.HeaderPermissaoAprovacao' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Permissão de aprovação'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.ckbBloqueado.Text' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Bloqueado'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.bntSalvar.Text' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Salvar'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.btnCancelar.Text' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Cancelar'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.ErroCarregarRelatorio' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Erro ao carregar relatório de atendimento.'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.TituloAnexoSemArquivo' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Título do anexo é obrigatório se adicionar um arquivo.'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.RelatorioIncluidoSucesso' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Relatório de atendimento incuído com sucesso.'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.RelatorioAlteradoSucesso' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Relatório de atendimento alterado com sucesso.'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.ErroSalvarRelatorio' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Erro ao tentar salvar o relatório de atendimento.'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.btnVoltar.Text' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Voltar'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.ErroCarregarSistema' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Erro ao tentar carregar o sistema.'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.QuestionarioObrigatorio' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Questionário é obrigatório.'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.ErroAdicionarQuestionario' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Erro ao tentar adicionar o questionário.'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.btnNovoQuestionario.Text' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Adicionar questionário'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.lblLegendAnexo.Text' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Anexo'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.NenhumQuestionarioAdicionado' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Adicione pelo menos um questionário.'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.NenhumaPermissao' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Selecione pelo menos uma permissão para grupo ou cargo.'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.QuestionarioJaAdicionado' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Questionário já adicionado no relatório.'

	EXEC MS_InsereResource 
		@rcr_chave = 'pnlFiltros.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.Classe.RelatorioAtendimento.Cadastro'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Lançamento de relatórios de AEE'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'Padrao.Salvar.Text' 
		, @rcr_NomeResource = 'Padrao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Salvar'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'btnAprovar.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.Classe.RelatorioAtendimento.Cadastro'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Aprovar'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'Padrao.Editar.Text' 
		, @rcr_NomeResource = 'Padrao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Editar'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'Padrao.Cancelar.Text' 
		, @rcr_NomeResource = 'Padrao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Cancelar'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'Padrao.Salvar.Text' 
		, @rcr_NomeResource = 'Padrao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Salvar'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'btnAprovarBaixo.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.Classe.RelatorioAtendimento.Cadastro'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Aprovar'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'Padrao.Editar.Text' 
		, @rcr_NomeResource = 'Padrao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Editar'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'Padrao.Cancelar.Text' 
		, @rcr_NomeResource = 'Padrao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Cancelar'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'pnlBusca.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.Classe.RelatorioAtendimento.Busca'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Consulta de relatórios de AEE'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'Padrao.Pesquisar.Text' 
		, @rcr_NomeResource = 'Padrao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Pesquisar'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'Padrao.LimparPesquisa.Text' 
		, @rcr_NomeResource = 'Padrao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Limpar pesquisa'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'Padrao.Resultados.Text' 
		, @rcr_NomeResource = 'Padrao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Resultados'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'Padrao.SemResultado.Text' 
		, @rcr_NomeResource = 'Padrao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'A pesquisa não encontrou resultados.'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'Padrao.Nome.Text' 
		, @rcr_NomeResource = 'Padrao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Nome'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'Padrao.Idade.Text' 
		, @rcr_NomeResource = 'Padrao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Idade'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'Padrao.Escola.Text' 
		, @rcr_NomeResource = 'Padrao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Escola'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'Padrao.Curso.Text' 
		, @rcr_NomeResource = 'Padrao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Curso'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'Padrao.Turma.Text' 
		, @rcr_NomeResource = 'Padrao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Turma'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'Padrao.LancarRelatorio.Text' 
		, @rcr_NomeResource = 'Padrao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Lançar relatório'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'ctrl_61.ToolTip' 
		, @rcr_NomeResource = 'GestaoEscolar.Classe.RelatorioAtendimento.Busca'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Lnaçar relatório de atendimento AEE'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'lblDownloadAnexo.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.WebControls.LancamentoRelatorioAtendimento.UCLancamentoRelatorioAtendimento'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Instruções de preenchimento '
	
	EXEC MS_InsereResource 
		@rcr_chave = 'hplDownloadAnexo.ToolTip' 
		, @rcr_NomeResource = 'GestaoEscolar.WebControls.LancamentoRelatorioAtendimento.UCLancamentoRelatorioAtendimento'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Realizar o download de instruções de preenchimento'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'lit_22.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.WebControls.LancamentoRelatorioAtendimento.UCLancamentoRelatorioAtendimento'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Hipótese diagnóstica'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'lblTitulo.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.WebControls.Combos.UCComboRelatorioAtendimento'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Relatório'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'cpvCombo.ErrorMessage' 
		, @rcr_NomeResource = 'GestaoEscolar.WebControls.Combos.UCComboRelatorioAtendimento'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Relatório é obrigatório.'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'lblTitulo.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.WebControls.Combos.UCComboTipoRelatorioAtendimento'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Tipo de relatório'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'cpvCombo.ErrorMessage' 
		, @rcr_NomeResource = 'GestaoEscolar.WebControls.Combos.UCComboTipoRelatorioAtendimento'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Tipo de relatório é obrigatório.'
		
	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioRecuperacaoParalela.Cadastro.pnlFiltros.GroupingText' 
        , @rcr_NomeResource = 'Classe'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Anotações da recuperação paralela'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioRecuperacaoParalela.Cadastro.btnNovo.Text' 
        , @rcr_NomeResource = 'Classe'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Nova anotação'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioRecuperacaoParalela.Cadastro.btnSalvar.Text' 
        , @rcr_NomeResource = 'Classe'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Salvar'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioRecuperacaoParalela.Cadastro.btnCancelar.Text' 
        , @rcr_NomeResource = 'Classe'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Cancelar'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioRecuperacaoParalela.Cadastro.grvLancamentos.EmptyDataText' 
        , @rcr_NomeResource = 'Classe'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Não existem anotações de recuperação paralela para o aluno.'
		
	EXEC MS_InsereResource 
        @rcr_chave = 'Padrao.LimparPesquisa.Text' 
        , @rcr_NomeResource = 'Padrao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Limpar pesquisa'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.btnAddAnexo.Text' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Adicionar anexo'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.ErroAdicionarArquivo' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Erro ao tentar adicionar anexo.'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.SelecioneArquivo' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Selecione um aqruivo para adicionar anexo.'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.btnExcluirAnexo.Text' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Excluir anexo'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.ErroExcluirArquivo' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Erro ao tentar excluir anexo.'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.TituloAnexoObrigatorio' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Título do anexo é obrigatório para adicionar um arquivo.'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioAtendimento.Cadastro.cpvPeriodicidade.ErrorMessage' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Periodicidade do relatório é obrigatória.'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioRecuperacaoParalela.Cadastro.litLancamento.Text' 
        , @rcr_NomeResource = 'Classe'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Lançamento de anotação'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioRecuperacaoParalela.Cadastro.btnVoltar.Text' 
        , @rcr_NomeResource = 'Classe'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Voltar'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioRecuperacaoParalela.Cadastro.MensagemSucessoSalvar' 
        , @rcr_NomeResource = 'Classe'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Lançamento de anotação salvo com sucesso.'

-- Fechar transação     
SET XACT_ABORT OFF 
COMMIT TRANSACTION



