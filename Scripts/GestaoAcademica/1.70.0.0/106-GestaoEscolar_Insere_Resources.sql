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
		@rcr_chave = 'Sondagem.Busca.lblLegend.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Listagem de sondagens'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Busca.lblTitulo.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Título'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Busca.btnPesquisar.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Pesquisar'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Busca.btnLimparPesquisa.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Limpar pesquisa'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Busca.btnNovo.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Nova sondagem'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Busca.lblLegendResultados.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Resultados'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Busca.dgvSondagem.EmptyDataText' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Nenhuma sondagem encontrada para os filtros informados.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Busca.dgvSondagem.HeaderTitulo' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Título'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Busca.dgvSondagem.HeaderAgendamento' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Agendamento'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Busca.dgvSondagem.btnCadastrarAgendamento.ToolTip' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Cadastrar agendamentos da sondagem.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Busca.dgvSondagem.HeaderExcluir' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Excluir'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Busca.dgvSondagem.btExcluir.ToolTip' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Excluir sondagem.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Busca.ErroCarregarSondagens' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Erro ao tentar carregar sondagens.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Busca.ErroCarregarDados' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Erro ao tentar carregar os dados.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Busca.SondagemExcluidaSucesso' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Sondagem excluída com sucesso.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Busca.ErroExcluirSondagem' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Erro ao tentar excluir a sondagem.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.lblLegend.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Cadastro de sondagem'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.lblTitulo.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Título *'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.rfvTitulo.ErrorMessage' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Título da sondagem é obrigatório'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.lblDescricao.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Descrição'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.bntSalvar.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Salvar'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.btnCancelar.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Cancelar'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.btnVoltar.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Voltar'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.ErroCarregarSondagem' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Erro ao tentar carregar a sondagem.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.SondagemIncluidaSucesso' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Sondagem incluída com sucesso.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.SondagemAlteradaSucesso' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Sondagem alterada com sucesso.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.ErroSalvarSondagem' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Erro ao tentar salvar a sondagem.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.ErroCarregarSistema' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Erro ao tentar carregar o sistema.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.lblLegend.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Agendamento de sondagem'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.ErroCarregarSondagem' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Erro ao tentar carregar o agendamento.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.SelecioneSondagem' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Selecione uma sondagem para efetuar o agendamento.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.ErroCarregarSistema' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Erro ao tentar carregar o sistema.'

-- Fechar transação     
SET XACT_ABORT OFF 
COMMIT TRANSACTION



