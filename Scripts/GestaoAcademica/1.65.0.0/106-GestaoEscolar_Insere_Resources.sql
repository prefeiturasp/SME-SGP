USE [GestaoPedagogica]
GO

BEGIN TRANSACTION 
SET XACT_ABORT ON   

	EXEC MS_InsereResource 
		@rcr_chave = 'DivergenciasRematriculas.Busca.btnGerarRel.Text' 
		, @rcr_NomeResource = 'Relatorios'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Gerar documento'

	EXEC MS_InsereResource 
		@rcr_chave = 'DivergenciasRematriculas.Busca.lblMessage.ErroPermissao' 
		, @rcr_NomeResource = 'Relatorios'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Você não possui permissão para acessar a página solicitada.'

	EXEC MS_InsereResource 
		@rcr_chave = 'DivergenciasRematriculas.Busca.lblMessage.ErroCarregarSistema' 
		, @rcr_NomeResource = 'Relatorios'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Erro ao tentar carregar o sistema.'

	EXEC MS_InsereResource 
		@rcr_chave = 'DivergenciasRematriculas.Busca.lblMessage.ErroGerarRelatorio' 
		, @rcr_NomeResource = 'Relatorios'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Erro ao tentar gerar o relatório.'

	EXEC MS_InsereResource 
		@rcr_chave = 'DivergenciasRematriculas.Busca.lblMessage.ErroCarregarDados' 
		, @rcr_NomeResource = 'Relatorios'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Erro ao tentar carregar os dados.'

	EXEC MS_InsereResource 
		@rcr_chave = 'DivergenciasRematriculas.Busca.MensagemAviso' 
		, @rcr_NomeResource = 'Relatorios'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Verificar todos os alunos dessa lista, pois devem ter as informações corrigidas.'

	EXEC MS_InsereResource 
		@rcr_chave = 'Aluno.Cadastro.chbPossuiInformacaoSigilosa.Text' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Possui informação sigilosa'

	EXEC MS_InsereResource 
		@rcr_chave = 'ControleTurma.DiarioClasse.imgSemPlanoAula' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Plano de aula não preenchido'

	EXEC MS_InsereResource 
		@rcr_chave = 'ControleTurma.DiarioClasse.imgPlanoAulaSituacao' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Conteúdo programático preenchido'

	EXEC MS_InsereResource 
		@rcr_chave = 'ControleTurma.DiarioClasse.imgPlanoAulaSituacaoIncompleta' 
		, @rcr_NomeResource = 'Academico'
		, @rcr_cultura = 'pt-BR'
		, @rcr_valorPadrao = 'Plano de aula preenchido sem o resumo conteúdo programático'

-- Fechar transação     
SET XACT_ABORT OFF 
COMMIT TRANSACTION



