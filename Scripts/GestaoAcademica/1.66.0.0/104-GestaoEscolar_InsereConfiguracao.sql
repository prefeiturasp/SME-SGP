USE [GestaoPedagogica]
GO

--Iniciar transação
BEGIN TRANSACTION
SET XACT_ABORT ON

	-- Insere o configuração no sistema.
	EXEC MS_InsereConfiguracao
		@cfg_chave = N'AppURLAreaAlunoInfantil' -- Chave da configuração. (Obrigatório)
		,@cfg_valor = N'infantilonline.sme.prefeitura.sp.gov.br' -- Valor da configuração. (Obrigatório)
		,@cfg_descricao = N'URL da área do aluno para Ensino Infantil.' -- Descrição da configuração. (Obrigatório)
		,@configuracaoInterna = 1 -- Flag que indica se é configuração interna do sistema. Não permite excluir. (Obrigatório)
		
-- Fechar transação	
SET XACT_ABORT OFF
COMMIT TRANSACTION