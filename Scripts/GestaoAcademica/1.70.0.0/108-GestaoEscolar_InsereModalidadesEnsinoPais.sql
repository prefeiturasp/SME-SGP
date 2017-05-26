USE [GestaoPedagogica]
GO

BEGIN TRANSACTION
SET XACT_ABORT ON

	DECLARE @dataAtual DATETIME = GETDATE();

	DECLARE @Modalidades TABLE (tme_id INT, tme_nome VARCHAR(100))
	INSERT INTO @Modalidades (tme_id, tme_nome)
	SELECT tme.tme_id, tme.tme_nome
	FROM ACA_TipoModalidadeEnsino tme WITH(NOLOCK)
	WHERE tme.tme_idSuperior IS NULL
	AND tme.tme_situacao <> 3
	AND NOT EXISTS
	(
		SELECT TOP 1 1
		FROM ACA_TipoModalidadeEnsino tme2 WITH(NOLOCK)
		WHERE tme2.tme_idSuperior = tme.tme_id
		AND tme2.tme_situacao <> 3
	)

	WHILE (EXISTS (SELECT TOP 1 1 FROM @Modalidades))
	BEGIN
		DECLARE @tme_id INT;
		DECLARE @tme_idSuperior INT;
		DECLARE @tme_nome VARCHAR(100);

		SELECT TOP 1
			@tme_id = tme_id,
			@tme_nome = tme_nome
		FROM
			@Modalidades

		INSERT INTO ACA_TipoModalidadeEnsino
		(
			tme_nome,
			tme_idSuperior,
			tme_situacao,
			tme_dataCriacao,
			tme_dataAlteracao
		)
		VALUES
		(
			@tme_nome,
			NULL,
			1,
			@dataAtual,
			@dataAtual
		)

		SET @tme_idSuperior = ISNULL(SCOPE_IDENTITY(),-1)

		IF (@tme_idSuperior > 0)
		BEGIN
			UPDATE ACA_TipoModalidadeEnsino
			SET tme_idSuperior = @tme_idSuperior
			WHERE tme_id = @tme_id
		END

		DELETE @Modalidades
		WHERE tme_id = @tme_id
	END

SET XACT_ABORT OFF
COMMIT TRANSACTION