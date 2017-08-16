USE [PUB_DEV_SPO_GestaoPedagogica]
GO

--Iniciar transação
BEGIN TRANSACTION
SET XACT_ABORT ON

	/***************
		Copiar do exemplo abaixo.
	****************
	
	-- Insere o configuração no sistema.
	EXEC MS_InsereConfiguracao
		@cfg_chave = N'AppAlunoFrequenciaLimite' -- Chave da configuração. (Obrigatório)
		,@cfg_valor = N'#FA3440' -- Valor da configuração. (Obrigatório)
		,@cfg_descricao = N'Cor para o aluno com frequência final abaixo do limite.' -- Descrição da configuração. (Obrigatório)
		,@configuracaoInterna = 1 -- Flag que indica se é configuração interna do sistema. Não permite excluir. (Obrigatório)
	*/

-- Fechar transação	
SET XACT_ABORT OFF
COMMIT TRANSACTION