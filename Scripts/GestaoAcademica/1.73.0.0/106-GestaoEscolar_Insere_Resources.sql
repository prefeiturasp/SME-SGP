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
            @rcr_chave = 'Curriculo.Cadastro.UCCurriculo1.Titulo' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Cadastro de currículo'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.litIntroducao.Text' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Introdução'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.litHabilidades.Text' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Conteúdos e habilidades'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.btnNovoGeral.Text' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Novo tópico'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.btnNovoDisciplina.Text' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Novo tópico'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.grvGeral.ColunaTopico' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Tópico'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.rfvTitulo.ErrorMessage' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Título do tópico é obrigatório.'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.grvGeral.ColunaOrdem' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Ordem'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.btnSubir.ToolTip' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Mover para cima'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.btnDescer.ToolTip' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Mover para baixo'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.grvGeral.ColunaEditar' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Editar'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.btnEditar.ToolTip' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Editar'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.btnSalvar.ToolTip' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Salvar'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.btnCancelarEdicao.ToolTip' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Cancelar'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.grvGeral.ColunaExcluir' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Excluir'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.btnExcluir.ToolTip' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Excluir'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.MensagemSucessoSalvar' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Tópico salvo com sucesso.'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.MensagemSucessoExcluir' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Tópico excluído com sucesso.'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.grvGeral.EmptyDataText' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Não existem tópicos cadastrados.'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.lblTitulo.Text' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Título *'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.lblDescricao.Text' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Descrição'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.grvDisciplina.EmptyDataText' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Não existem tópicos cadastrados.'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.grvDisciplina.ColunaTopico' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Tópico'

	EXEC MS_InsereResource 
            @rcr_chave = 'UCPlanejamentoProjetos.litPlanoCiclo.Text.Etapa' 
            , @rcr_NomeResource = 'UserControl'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Plano da etapa'

	EXEC MS_InsereResource 
            @rcr_chave = 'UCPlanejamentoProjetos.litPlanoAnual.Text.Semestral' 
            , @rcr_NomeResource = 'UserControl'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Plano semestral'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.btnNovoEixo.Text' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Novo eixo'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.grvEixo.EmptyDataText' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Não existem eixos cadastrados.'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.grvEixo.ColunaEixo' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Eixo'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.grvEixo.rfvDescricao.ErrorMessage' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Descrição do eixo é obrigatório.'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.grvEixo.lblDescricao.Text' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Descrição *'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.MensagemSucessoSalvarEixo' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Eixo salvo com sucesso.'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.MensagemSucessoExcluirEixo' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Eixo excluído com sucesso.'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.btnNovoObjetivo.Text' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Novo objetivo'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.grvObjetivo.EmptyDataText' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Não existem objetivos cadastrados.'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.grvObjetivo.ColunaObjetivo' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Objetivo'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.grvObjetivo.rfvDescricao.ErrorMessage' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Descrição do objetivo é obrigatório.'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.btnNovoObjetivoAprendizagem.Text' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Novo objetivo de aprendizagem'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.MensagemSucessoSalvarObjetivo' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Objetivo salvo com sucesso.'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.MensagemSucessoSalvarObjetivoAprendizagem' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Objetivo de aprendizagem salvo com sucesso.'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.MensagemSucessoExcluirObjetivo' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Objetivo excluído com sucesso.'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.MensagemSucessoExcluirObjetivoAprendizagem' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Objetivo de aprendizagem excluído com sucesso.'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.grvObjetivoAprendizagem.EmptyDataText' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Não existem objetivos de aprendizagem cadastrados.'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.grvObjetivoAprendizagem.ColunaObjetivoAprendizagem' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Objetivo de aprendizagem'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.Cadastro.grvObjetivoAprendizagem.rfvDescricao.ErrorMessage' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Descrição do objetivo de aprendizagem é obrigatório.'


	EXEC MS_InsereResource 
            @rcr_chave = 'Sondagem.Cadastro.ckbBloqueado.Text' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Bloqueado'

	EXEC MS_InsereResource 
            @rcr_chave = 'Sondagem.Cadastro.revtxtItemResposta.ErrorMessage' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Resposta/Avaliação é obrigatório.'

	EXEC MS_InsereResource 
            @rcr_chave = 'Sondagem.Cadastro.revtxtSigla.ErrorMessage' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Sigla é obrigatório.'

	EXEC MS_InsereResource 
            @rcr_chave = 'Sondagem.Cadastro.revtxtItemSubquestao.ErrorMessage' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Subquestão é obrigatório'

	EXEC MS_InsereResource 
            @rcr_chave = 'Sondagem.Cadastro.revtxtItemQuestao.ErrorMessage' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Questão é obrigatório.'

	EXEC MS_InsereResource 
            @rcr_chave = 'Sondagem.Cadastro.grvQuestoes.HeaderNome' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Questão *'

	EXEC MS_InsereResource 
            @rcr_chave = 'Sondagem.Cadastro.grvRespostas.HeaderNome' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Resposta/Avaliação *'

	EXEC MS_InsereResource 
            @rcr_chave = 'Sondagem.Cadastro.grvRespostas.HeaderSigla' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Sigla *'

	EXEC MS_InsereResource 
            @rcr_chave = 'Sondagem.Cadastro.grvSubQuestoes.HeaderNome' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Subquestão *'

	EXEC MS_InsereResource 
            @rcr_chave = 'Sondagem.Cadastro.lblCampo.Text' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Descrição'

	EXEC MS_InsereResource 
            @rcr_chave = 'Sondagem.Cadastro.lblSigla.Text' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Sigla *'

	EXEC MS_InsereResource 
            @rcr_chave = 'Sondagem.Cadastro.lblTitulo.Text' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Título *'

	EXEC MS_InsereResource 
            @rcr_chave = 'Sondagem.Cadastro.ddlOpcaoResposta.Multiselecao' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Multiseleção'

	EXEC MS_InsereResource 
            @rcr_chave = 'Sondagem.Cadastro.ddlOpcaoResposta.SelecaoUnica' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Seleção única'

	EXEC MS_InsereResource 
            @rcr_chave = 'Sondagem.Cadastro.lblOpcaoResposta.Text' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Opção de resposta *'

	EXEC MS_InsereResource 
            @rcr_chave = 'Sondagem.Cadastro.rfvOpcaoResposta.ErrorMessage' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Opção de resposta é obrigatório.'

	EXEC MS_InsereResource 
            @rcr_chave = 'Sondagem.Cadastro.ddlOpcaoResposta.Selecione' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Opção de resposta é obrigatório.'

	EXEC MS_InsereResource 
            @rcr_chave = 'Curriculo.RegistroSugestoes.UCCurriculo1.Titulo' 
            , @rcr_NomeResource = 'Academico'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Registro de sugestões'

	EXEC MS_InsereResource 
            @rcr_chave = 'RelatorioSugestoesCurriculo.Busca.btnGerarRel.Text' 
            , @rcr_NomeResource = 'Relatorios'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Gerar relatório'

	EXEC MS_InsereResource 
            @rcr_chave = 'RelatorioSugestoesCurriculo.Busca.btnLimparPesquisa.Text' 
            , @rcr_NomeResource = 'Relatorios'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Limpar pesquisa'

	EXEC MS_InsereResource 
            @rcr_chave = 'RelatorioSugestoesCurriculo.Busca.DataInicioInvalida' 
            , @rcr_NomeResource = 'Relatorios'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Data inicial deve estar no formato DD/MM/AAAA.'

	EXEC MS_InsereResource 
            @rcr_chave = 'RelatorioSugestoesCurriculo.Busca.DataFimInvalida' 
            , @rcr_NomeResource = 'Relatorios'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Data final deve estar no formato DD/MM/AAAA.'

	EXEC MS_InsereResource 
            @rcr_chave = 'RelatorioSugestoesCurriculo.Busca.DataFimMenorInicio' 
            , @rcr_NomeResource = 'Relatorios'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Data final do período deve ser maior ou igual à data inicial.'

	EXEC MS_InsereResource 
            @rcr_chave = 'RelatorioSugestoesCurriculo.Busca.lblMessage.ErroPermissao' 
            , @rcr_NomeResource = 'Relatorios'
            , @rcr_cultura = 'pt-BR'
            , @rcr_codigo = 0 
            , @rcr_valorPadrao = 'Você não possui permissão para acessar a página solicitada.'

-- Fechar transação     
SET XACT_ABORT OFF 
COMMIT TRANSACTION



