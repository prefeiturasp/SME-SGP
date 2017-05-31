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

-- Fechar transação     
SET XACT_ABORT OFF 
COMMIT TRANSACTION



