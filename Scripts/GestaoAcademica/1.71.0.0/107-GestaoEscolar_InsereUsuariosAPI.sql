USE [GestaoPedagogica]
GO

BEGIN TRANSACTION 
SET XACT_ABORT ON   

	DECLARE @usuario VARCHAR(100) = 'usuarioGestaoPedagogicaAPI';
	DECLARE @senha VARCHAR(256) = '0lGzDU+IVMg='

	IF (NOT EXISTS (SELECT TOP 1 1
					FROM SYS_UsuarioAPI WITH(NOLOCK)
					WHERE uap_usuario = @usuario
					AND uap_situacao <> 3))
	BEGIN
		DECLARE @dataAtual DATETIME = GETDATE();

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
			4,
			@dataAtual,
			@dataAtual
		)
	END

-- Fechar transação     
SET XACT_ABORT OFF 
COMMIT TRANSACTION



