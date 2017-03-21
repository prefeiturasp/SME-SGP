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
        @rcr_chave = 'UCPlanejamentoProjetos.litObjetoAprendizagem.Text' 
        , @rcr_NomeResource = 'UserControl'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Objetos de Aprendizagem'

	EXEC MS_InsereResource 
        @rcr_chave = 'ControleTurma.DiarioClasse.lblLgdObjAprendizagem.Text' 
        , @rcr_NomeResource = 'Academico'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Objetos de Aprendizagem'

	EXEC MS_InsereResource 
        @rcr_chave = 'PlanejamentoDiario.Cadastro.MensagemAulasPrevistas' 
        , @rcr_NomeResource = 'Classe'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Não é possível gerar aulas pois as disciplinas estão sem aulas previstas:'

	EXEC MS_InsereResource 
        @rcr_chave = 'Areas.Busca.btnCadastroArea.Text' 
        , @rcr_NomeResource = 'Academico'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Incluir novo documento'

	EXEC MS_InsereResource 
        @rcr_chave = 'Areas.Busca.btnCadastroArea.ToolTip' 
        , @rcr_NomeResource = 'Academico'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Incluir novo documento'

	EXEC MS_InsereResource 
        @rcr_chave = 'Areas.Busca.btnNovaArea.Text' 
        , @rcr_NomeResource = 'Academico'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Incluir novo documento'

	EXEC MS_InsereResource 
        @rcr_chave = 'Areas.Busca.grvAreas.btnAlterar.ToolTip' 
        , @rcr_NomeResource = 'Academico'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Alterar nome do documento'

	EXEC MS_InsereResource 
        @rcr_chave = 'Areas.Busca.grvAreas.HeaderTextNome' 
        , @rcr_NomeResource = 'Academico'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Nome'

	EXEC MS_InsereResource 
        @rcr_chave = 'Areas.Busca.lblAreaLegend.Text' 
        , @rcr_NomeResource = 'Academico'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Busca de documentos'

	EXEC MS_InsereResource 
        @rcr_chave = 'Areas.Busca.pnlBusca.GroupingText' 
        , @rcr_NomeResource = 'Academico'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Busca de documentos'

	EXEC MS_InsereResource 
        @rcr_chave = 'Areas.Cadastro.ErroAoTentarSalvar' 
        , @rcr_NomeResource = 'Academico'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Erro ao tentar salvar cadastro de documento.'

	EXEC MS_InsereResource 
        @rcr_chave = 'Areas.Cadastro.ErroAoTentarCarregar' 
        , @rcr_NomeResource = 'Academico'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Erro ao tentar carregar cadastro de documento.'

	EXEC MS_InsereResource 
        @rcr_chave = 'Areas.Busca.ErroExcluirTipoArea' 
        , @rcr_NomeResource = 'Academico'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Erro ao excluir documento.'

	EXEC MS_InsereResource 
        @rcr_chave = 'Areas.Busca.SucessoExclusaoPopup' 
        , @rcr_NomeResource = 'Academico'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Documento excluído com sucesso.'

	EXEC MS_InsereResource 
        @rcr_chave = 'Areas.Cadastro.btnCancelarPopUp.ToolTip' 
        , @rcr_NomeResource = 'Academico'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Cancelar cadastro de documento'

	EXEC MS_InsereResource 
        @rcr_chave = 'Areas.Cadastro.btnSalvarPopUp.ToolTip' 
        , @rcr_NomeResource = 'Academico'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Salvar novo documento'

	EXEC MS_InsereResource 
        @rcr_chave = 'Areas.Cadastro.lblAreaLegendCadastro.Text' 
        , @rcr_NomeResource = 'Academico'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Cadastro de documentos'

	EXEC MS_InsereResource 
        @rcr_chave = 'Areas.Cadastro.lblMsgInfo.Text' 
        , @rcr_NomeResource = 'Academico'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Apenas o nome do documento pode ser alterado, pois este já possui documento(s)/link(s) cadastrado(s).'

	EXEC MS_InsereResource 
        @rcr_chave = 'Areas.Cadastro.lblNome.Text' 
        , @rcr_NomeResource = 'Academico'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Nome do documento: *'

	EXEC MS_InsereResource 
        @rcr_chave = 'Areas.Cadastro.lblPopUpArea.Text' 
        , @rcr_NomeResource = 'Academico'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Nome da documento: *'

	EXEC MS_InsereResource 
        @rcr_chave = 'Areas.Cadastro.pnlCadastro.GroupingText' 
        , @rcr_NomeResource = 'Academico'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Cadastro de documentos'

	EXEC MS_InsereResource 
        @rcr_chave = 'Areas.Cadastro.rfvArea.ErrorMessage' 
        , @rcr_NomeResource = 'Academico'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Documento é obrigatório.'

	EXEC MS_InsereResource 
        @rcr_chave = 'Areas.Cadastro.rfvNome.ErrorMessage' 
        , @rcr_NomeResource = 'Academico'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Documento é obrigatório.'

	EXEC MS_InsereResource 
        @rcr_chave = 'DocumentosDocente.Busca.btnGerar.Text' 
        , @rcr_NomeResource = 'Documentos'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Gerar relatório'

	EXEC MS_InsereResource 
        @rcr_chave = 'DocumentosDocente.Busca.btnGerarRelatorioCima.Text' 
        , @rcr_NomeResource = 'Documentos'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Gerar relatório'

	EXEC MS_InsereResource 
        @rcr_chave = 'DocumentosDocente.Busca._btnGerarRelatorio.Text' 
        , @rcr_NomeResource = 'Documentos'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Gerar relatório'

	EXEC MS_InsereResource 
        @rcr_chave = 'UCPlanejamentoProjetos.lblSemAreas.Text' 
        , @rcr_NomeResource = 'UserControl'
        , @rcr_cultura = 'pt-BR'
        , @rcr_codigo = 0 
        , @rcr_valorPadrao = 'Não há documentos cadastrados.'

-- Fechar transação     
SET XACT_ABORT OFF 
COMMIT TRANSACTION



