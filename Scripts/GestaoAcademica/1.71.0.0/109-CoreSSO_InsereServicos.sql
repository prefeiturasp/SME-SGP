USE [CoreSSO]
GO

BEGIN TRANSACTION 
SET XACT_ABORT ON

	EXEC MS_InsereParametro
		@par_chave = 'PRAZO_DIAS_EXPIRA_SENHA'
		, @par_valor = ''
		, @par_descricao = 'Prazo em dias para expirar as senhas'
		, @par_obrigatorio = 1

-- Fechar transação     
SET XACT_ABORT OFF 
COMMIT TRANSACTION 