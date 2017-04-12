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
		@rcr_chave = 'Sondagem.Busca.dgvSondagem.HeaderSituacao' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Situação'

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
		@rcr_chave = 'Sondagem.Busca.dgvSondagem.btnExcluir.ToolTip' 
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

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.lblLegendQuestoes.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Questões'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.lblLegendSubQuestoes.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Sub-Questões'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.lblLegendRespostas.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Respostas/Avaliações'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.btnAdicionarQuestao.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Adicionar questão'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.btnAdicionarSubQuestao.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Adicionar sub-questão'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.btnAdicionarResposta.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Adicionar resposta'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.grvQuestoes.EmptyDataText' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Nenhuma questão cadastrada.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.grvQuestoes.HeaderNome' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Questão'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.grvQuestoes.HeaderOrdem' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Ordem'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.grvSubQuestoes.EmptyDataText' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Nenhuma sub-questão cadastrada.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.grvSubQuestoes.HeaderNome' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Sub-Questão'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.grvSubQuestoes.HeaderOrdem' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Ordem'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.grvRespostas.EmptyDataText' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Nenhuma resposta/avaliação cadastrada.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.grvRespostas.HeaderNome' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Resposta/Avaliação'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.grvRespostas.HeaderOrdem' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Ordem'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.grvRespostas.HeaderExcluir' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Excluir'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.grvRespostas.btnExcluir.ToolTip' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Excluir questão.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.grvSubQuestoes.HeaderExcluir' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Excluir'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.grvSubQuestoes.btnExcluir.ToolTip' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Excluir sub-questão.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.grvRespostas.HeaderExcluir' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Excluir'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.grvRespostas.btnExcluir.ToolTip' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Excluir resposta/avaliação.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.ErroAbrirPopUp' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Erro ao abrir pop-up de cadastro.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.bntAdicionar.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Adicionar'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.bntAlterar.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Alterar'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.ErroAdicionar' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Erro ao tentar adicionar item.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.DescricaoObrigatorio' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = '{0} é obrigatória.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.lblCampo.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Descrição *'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.grvSubQuestoes.HeaderNumero' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Nº'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.grvQuestoes.HeaderNumero' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Nº'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.grvRespostas.HeaderNumero' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Nº'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.grvQuestoes.HeaderExcluir' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Excluir'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.grvSubQuestoes.HeaderExcluir' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Excluir'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.grvRespostas.HeaderExcluir' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Excluir'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.JaExisteItem' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Já existe uma {0} cadastrada com essa descrição.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.lblSigla.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Sigla *'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.JaExisteSigla' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Já existe uma {0} cadastrada com essa sigla.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.SiglaObrigatorio' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Sigla da {0} é obrigatória.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.grvRespostas.HeaderSigla' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Sigla'

	EXEC MS_InsereResource 
		@rcr_chave = 'ACA_SondagemBO.NaoPermiteReordenar' 
		, @rcr_NomeResource = 'BLL'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Não é permitido alterar a ordem dos campos pois a sondagem já está em uso.'

	EXEC MS_InsereResource 
		@rcr_chave = 'ACA_SondagemBO.NaoPermiteExcluirItem' 
		, @rcr_NomeResource = 'BLL'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Não é permitido remover itens pois a sondagem já está em uso.'

	EXEC MS_InsereResource 
		@rcr_chave = 'ACA_SondagemBO.NaoPermiteAdicionarItem' 
		, @rcr_NomeResource = 'BLL'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Não é permitido adicionar itens pois a sondagem já está em uso.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.AgendamentoIncluidoSucesso' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Agendamentos da sondagem incluídos com sucesso.'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.ErroSalvarAgendamento' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Erro ao tentar salvar os agendamentos da sondagem.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.ErroAbrirPopUp' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Erro ao tentar abrir o cadastro de agendamento.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.btnCancelar.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Cancelar'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.btnVoltar.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Voltar'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.DataInicioInvalida' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Data início deve estar no formato DD/MM/AAAA.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.DataFimInvalida' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Data fim deve estar no formato DD/MM/AAAA.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.DataFimMenorInicio' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Data fim do período deve ser maior ou igual à data início.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.PeriodoJaAdicionado' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Período já adicionado.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.ErroAdicionar' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Erro ao tentar adicionar período.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.lblSondagem.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Sondagem'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.lblDescricao.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Descrição'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.lblLegendDatas.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Datas do agendamento'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.btnAdicionarAgendamento.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Adicionar agendamento'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.grvAgendamentos.EmptyDataText' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Nenhum agendamento cadastrado.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.grvAgendamentos.HeaderDataInicio' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Data início'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.grvAgendamentos.HeaderDataFim' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Data fim'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.grvAgendamentos.HeaderPeriodos' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Períodos selecionados'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.grvAgendamentos.HeaderAlterar' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Alterar'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.grvAgendamentos.HeaderExcluir' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Excluir'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.grvAgendamentos.btnAlterar.ToolTip' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Altera o agendamento.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.grvAgendamentos.btnExcluir.ToolTip' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Exclui o agendamento (exclui também os períodos de retificação desse agendamento).'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.bntSalvar.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Salvar'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.lblTituloPopUp.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Adicionar agendamento'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.lblDataInicio.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Data início *'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento._rfvInicio.ErrorMessage' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Data início é obrigatória.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.cvDataInicio.ErrorMessage' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Data início deve estar no formato DD/MM/AAAA.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.lblDataFim.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Data fim *'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento._rfvFim.ErrorMessage' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Data fim é obrigatória.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.cvDataFim.ErrorMessage' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Data fim deve estar no formato DD/MM/AAAA.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.cpvDataFim2.ErrorMessage' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Data fim do período deve ser maior ou igual à data início.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.bntAdicionar.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Adicionar'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.btnCancelar.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Cancelar'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.bntAlterar.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Alterar'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.lblLegendPeriodos.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Períodos do curso'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.SemPeriodosSelecionados' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Selecione pelo menos um período do curso para o agendamento.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Cadastro.RespostaObrigatoria' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Informe pelo menos uma resposta/avaliação para a sondagem.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.grvAgendamentos.HeaderRetificar' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Retificar'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.grvAgendamentos.btnRetificar.ToolTip' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Retificar o agendamento para uma escola.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.grvAgendamentos.lblRetificado.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Retificado'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.grvAgendamentos.HeaderCancelarAgendamento' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Cancelar'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.grvAgendamentos.btnCancelarAgendamento.ToolTip' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Cancelar o período do agendamento (cancela também os períodos de retificação desse agendamento).'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.EscolaObrigatorio' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Escola é obrigatória para um agendamento de retificação.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.lblPeriodoRetificar.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Retificar agendamento: {0} - {1}'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.grvAgendamentos.btnReativar.ToolTip' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Reativa o período do agendamento (reativa também os períodos de retificação desse agendamento).'

	EXEC MS_InsereResource 
		@rcr_chave = 'Sondagem.Agendamento.grvAgendamentos.lblCancelado.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Cancelado'

-- Fechar transação     
SET XACT_ABORT OFF 
COMMIT TRANSACTION



