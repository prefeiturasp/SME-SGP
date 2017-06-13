USE [GestaoPedagogica]
GO

BEGIN TRANSACTION 
SET XACT_ABORT ON

	DECLARE @rap_id INT = 1;
	-- Alterar para a url da api do EOL
	DECLARE @urlApiEOL VARCHAR(200) = 'http://localhost:1425/v1/parecerConclusivo/' 

	IF (NOT EXISTS (SELECT TOP 1 1 
					FROM SYS_RecursoAPI WITH(NOLOCK)
					WHERE rap_id = 1
					AND rap_situacao <> 3))
	BEGIN
		DECLARE @dataAtual DATETIME = GETDATE();

		INSERT INTO SYS_RecursoAPI
		(
			rap_id,
			rap_descricao,
			rap_url,
			rap_situacao,
			rap_dataCriacao,
			rap_dataAlteracao
		)
		VALUES
		(
			@rap_id,
			'Recurso (Web Api) para consultar parecer conclusivo do aluno no EOL',
			@urlApiEOL, -- Alterar para a 
			1,
			@dataAtual,
			@dataAtual
		)

		DECLARE @uap_id INT = (SELECT ISNULL(SCOPE_IDENTITY(),-1))

		IF (@uap_id > 0)
		BEGIN
			INSERT INTO SYS_RecursoUsuarioAPI
			(
				rap_id,
				uap_id
			)
			VALUES
			(
				@rap_id,
				@uap_id
			)
		END
	END
-- Fechar transação     
SET XACT_ABORT OFF 
COMMIT TRANSACTION 