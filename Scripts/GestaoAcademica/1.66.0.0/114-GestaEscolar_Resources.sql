USE [GestaoPedagogica]
GO

BEGIN TRANSACTION 
SET XACT_ABORT ON   

	EXEC MS_InsereResource 
		@rcr_chave = 'Padrao.ParametrosBusca.Text' 
		, @rcr_NomeResource = 'Padrao'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Parâmetros de busca'
	
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
		@rcr_chave = 'btnGerar.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.Configuracao.FilaFechamento.Fila'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Gerar'
	
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
		@rcr_chave = 'ctrl_52.HeaderText' 
		, @rcr_NomeResource = 'GestaoEscolar.Configuracao.FilaFechamento.Fila'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Nome disciplina'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'chkGerarFilaNota.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.Configuracao.FilaFechamento.Fila'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Gerar fila nota'
	
	EXEC MS_InsereResource 
		@rcr_chave = 'chkGerarFilaFrequencia.Text' 
		, @rcr_NomeResource = 'GestaoEscolar.Configuracao.FilaFechamento.Fila'
		, @rcr_cultura = 'pt-BR'
		, @rcr_codigo = 0 
		, @rcr_valorPadrao = 'Gerar fila frequência'
	


-- Fechar transação     
SET XACT_ABORT OFF 
COMMIT TRANSACTION