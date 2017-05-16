USE [CoreSSO]
GO

BEGIN TRANSACTION 
SET XACT_ABORT ON

	EXEC MS_InsereServico
		@ser_id = 3
		, @ser_nome = 'Expirar senha de usuários (senha não alterada no prazo)'
		, @ser_nomeProcedimento = 'MS_JOB_ExpiraSenha'
		, @ser_ativo = 1

-- Fechar transação     
SET XACT_ABORT OFF 
COMMIT TRANSACTION 