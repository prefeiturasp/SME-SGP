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
        @rcr_chave = 'RelatorioAtendimento.Cadastro.chkAcoesRealizadas.Text' 
        , @rcr_NomeResource = 'Configuracao'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Permite ações realizadas'

	EXEC MS_InsereResource 
		@rcr_chave = 'pnlBusca.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.Classe.RelatorioNaapa.Busca'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Consulta de relatórios do NAAPA'

	EXEC MS_InsereResource 
		@rcr_chave = 'ctrl_61.ToolTip' 
		, @rcr_NomeResource = 'GestaoEscolar.Classe.RelatorioNaapa.Busca'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Lançar relatório do NAAPA'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'pnlBusca.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.Relatorios.AcoesRealizadas.Busca, pnlBusca.Text'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Consulta de ações realizadas'

	EXEC MS_InsereResource 
		@rcr_chave = 'Padrao.Calendario.Text' 
		, @rcr_NomeResource = 'Padrao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Calendário'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioNaapa.Cadastro.pnlFiltros.GroupingText' 
        , @rcr_NomeResource = 'Classe'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Lançamento de relatórios do NAAPA'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioNaapa.Cadastro.btnNovo.Text' 
        , @rcr_NomeResource = 'Classe'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Novo lançamento'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioNaapa.Cadastro.grvLancamentos.EmptyDataText' 
        , @rcr_NomeResource = 'Classe'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'A pesquisa não encontrou resultados.'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioNaapa.Cadastro.grvLancamentos.ColunaDescricao' 
        , @rcr_NomeResource = 'Classe'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Data do lançamento'

	EXEC MS_InsereResource 
		@rcr_chave = 'Padrao.Alterar.Text' 
		, @rcr_NomeResource = 'Padrao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Alterar'

	EXEC MS_InsereResource 
		@rcr_chave = 'Padrao.Detalhar.Text' 
		, @rcr_NomeResource = 'Padrao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Detalhar'

	EXEC MS_InsereResource 
		@rcr_chave = 'Padrao.Excluir.Text' 
		, @rcr_NomeResource = 'Padrao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Excluir'

	EXEC MS_InsereResource 
		@rcr_chave = 'GraficoJustificativaFalta.Busca.btnGerarRel.Text' 
		, @rcr_NomeResource = 'Relatorios'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Gerar relatório'

	EXEC MS_InsereResource 
		@rcr_chave = 'GraficoJustificativaFalta.Busca.lblMessage.ErroPermissao' 
		, @rcr_NomeResource = 'Relatorios'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Você não possui permissão para acessar a página solicitada.'

	EXEC MS_InsereResource 
		@rcr_chave = 'GraficoJustificativaFalta.Busca.lblMessage.ErroCarregarSistema' 
		, @rcr_NomeResource = 'Relatorios'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Erro ao tentar carregar o sistema.'

	EXEC MS_InsereResource 
		@rcr_chave = 'GraficoJustificativaFalta.Busca.lblMessage.ErroGerarRelatorio' 
		, @rcr_NomeResource = 'Relatorios'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Erro ao tentar carregar o relatório.'

	EXEC MS_InsereResource 
		@rcr_chave = 'GraficoJustificativaFalta.Busca.lblMessage.ErroCarregarDados' 
		, @rcr_NomeResource = 'Relatorios'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Erro ao tentar carregar os dados.'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioNaapa.Cadastro.MensagemSucessoSalvar' 
        , @rcr_NomeResource = 'Classe'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Lançamento do relatório salvo com sucesso.'

	EXEC MS_InsereResource 
        @rcr_chave = 'RelatorioNaapa.Cadastro.MensagemSucessoExcluir' 
        , @rcr_NomeResource = 'Classe'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Lançamento do relatório excluído com sucesso.'

	EXEC MS_InsereResource 
		@rcr_chave = 'litAcoesRealizadas.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.WebControls.LancamentoRelatorioAtendimento.UCLancamentoRelatorioAtendimento'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Ações realizadas'

	EXEC MS_InsereResource 
		@rcr_chave = 'btnNovaAcao.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.WebControls.LancamentoRelatorioAtendimento.UCLancamentoRelatorioAtendimento'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Nova ação'

	EXEC MS_InsereResource 
		@rcr_chave = 'grvAcoes.EmptyDataText' 
		, @rcr_NomeResource = 'GestaoEscolar.WebControls.LancamentoRelatorioAtendimento.UCLancamentoRelatorioAtendimento'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Não existem ações realizadas cadastradas nesse lançamento.'

	EXEC MS_InsereResource 
		@rcr_chave = 'ckbImpressao.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.WebControls.LancamentoRelatorioAtendimento.UCLancamentoRelatorioAtendimento'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Exibir na impressão'

	EXEC MS_InsereResource 
		@rcr_chave = 'lblData.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.WebControls.LancamentoRelatorioAtendimento.UCLancamentoRelatorioAtendimento'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Data *'

	EXEC MS_InsereResource 
		@rcr_chave = 'rfvData.ErrorMessage' 
		, @rcr_NomeResource = 'GestaoEscolar.WebControls.LancamentoRelatorioAtendimento.UCLancamentoRelatorioAtendimento'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Data da ação realizada é obrigatório.'

	EXEC MS_InsereResource 
		@rcr_chave = 'lblAcao.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.WebControls.LancamentoRelatorioAtendimento.UCLancamentoRelatorioAtendimento'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Ação realizada *'

	EXEC MS_InsereResource 
		@rcr_chave = 'rfvAcao.ErrorMessage' 
		, @rcr_NomeResource = 'GestaoEscolar.WebControls.LancamentoRelatorioAtendimento.UCLancamentoRelatorioAtendimento'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Ação realizada é obrigatório.'

	EXEC MS_InsereResource 
		@rcr_chave = 'ctvDataFormato.ErrorMessage' 
		, @rcr_NomeResource = 'GestaoEscolar.WebControls.LancamentoRelatorioAtendimento.UCLancamentoRelatorioAtendimento'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Data da ação realizada não está no formato dd/mm/aaaa ou é inexistente.'

	EXEC MS_InsereResource 
		@rcr_chave = 'litTitulo.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.Configuracao.Alertas.Busca'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Listagem de alertas'

	EXEC MS_InsereResource 
		@rcr_chave = 'grvAlertas.ColunaNome' 
		, @rcr_NomeResource = 'GestaoEscolar.Configuracao.Alertas.Busca'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Nome do alerta'

	EXEC MS_InsereResource 
		@rcr_chave = 'litTitulo.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.Configuracao.Alertas.Cadastro'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Cadastro de alerta'

	EXEC MS_InsereResource 
		@rcr_chave = 'lblNome.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.Configuracao.Alertas.Cadastro'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Nome *'

	EXEC MS_InsereResource 
		@rcr_chave = 'lblAssunto.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.Configuracao.Alertas.Cadastro'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Assunto'

	EXEC MS_InsereResource 
		@rcr_chave = 'lblPeriodoAnalise.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.Configuracao.Alertas.Cadastro'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Quantidade de dias para análise de dados'

	EXEC MS_InsereResource 
		@rcr_chave = 'lblPeriodoValidade.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.Configuracao.Alertas.Cadastro'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Período de validade da notificação (em horas)'

	EXEC MS_InsereResource 
		@rcr_chave = 'litGrupos.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.Configuracao.Alertas.Cadastro'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Grupos de envio da notificação'

	EXEC MS_InsereResource 
		@rcr_chave = 'grvGrupos.EmptyDataText' 
		, @rcr_NomeResource = 'GestaoEscolar.Configuracao.Alertas.Cadastro'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Nenhum grupo foi encontrado.'

	EXEC MS_InsereResource 
		@rcr_chave = 'grvGrupos.ColunaNome' 
		, @rcr_NomeResource = 'GestaoEscolar.Configuracao.Alertas.Cadastro'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Nome'

	EXEC MS_InsereResource 
		@rcr_chave = 'grvGrupos.ColunaEnvioNotificacao' 
		, @rcr_NomeResource = 'GestaoEscolar.Configuracao.Alertas.Cadastro'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Enviar notificação'

	EXEC MS_InsereResource 
        @rcr_chave = 'mensagemSucessoSalvar' 
        , @rcr_NomeResource = 'GestaoEscolar.Configuracao.Alertas.Cadastro'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Alerta salvo com sucesso.'

	EXEC MS_InsereResource 
        @rcr_chave = 'chkDesativar.Text' 
        , @rcr_NomeResource = 'GestaoEscolar.Configuracao.Alertas.Cadastro'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Desativar alerta'

	EXEC MS_InsereResource 
        @rcr_chave = 'rfvNome.ErrorMessage' 
        , @rcr_NomeResource = 'GestaoEscolar.Configuracao.Alertas.Cadastro'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Nome é obrigatório.'

	EXEC MS_InsereResource 
        @rcr_chave = 'litAgendamento.Text' 
        , @rcr_NomeResource = 'GestaoEscolar.Configuracao.Alertas.Cadastro'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Agendamento'

-- Fechar transação     
SET XACT_ABORT OFF 
COMMIT TRANSACTION



