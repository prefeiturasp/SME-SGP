USE [GestaoPedagogica]
GO

BEGIN TRANSACTION 
SET XACT_ABORT ON

	DECLARE @rap_id INT = 2;
	-- Alterar para a url da api do notificações
	DECLARE @urlApi VARCHAR(200) = 'http://10.10.10.37:5019/api/v2/Notification' 

	IF (NOT EXISTS (SELECT TOP 1 1 
					FROM SYS_RecursoAPI WITH(NOLOCK)
					WHERE rap_id = @rap_id
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
			'Web Api do sistema Notificações, para envio dos alertas gerados pelo SGP',
			@urlApi, 
			1,
			@dataAtual,
			@dataAtual
		)

		DECLARE @usuario VARCHAR(100) = 'userBasicAuth';
		DECLARE @senha VARCHAR(256) = 'passwordBasicAuth'

		IF (NOT EXISTS (SELECT TOP 1 1
						FROM SYS_UsuarioAPI WITH(NOLOCK)
						WHERE uap_usuario = @usuario
						AND uap_situacao <> 3))
		BEGIN
			INSERT INTO SYS_UsuarioAPI
			(
				uap_usuario,
				uap_senha,
				uap_situacao,
				uap_dataCriacao,
				uap_dataAlteracao
			)
			VALUES
			(
				@usuario,
				@senha,
				1,
				@dataAtual,
				@dataAtual
			)
		END

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