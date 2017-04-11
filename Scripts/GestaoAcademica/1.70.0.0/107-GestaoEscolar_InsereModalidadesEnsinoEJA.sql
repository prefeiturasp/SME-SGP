USE [GestaoPedagogica]
GO

BEGIN TRANSACTION
SET XACT_ABORT ON
	
	DECLARE @tme_idEJA INT = (SELECT TOP 1 tme_id 
							  FROM ACA_TipoModalidadeEnsino WITH(NOLOCK)
							  WHERE tme_nome = 'Educação de Jovens e Adultos - EJA'
							  AND tme_situacao <> 3);
	
	DECLARE @tme_idEJAEspecial INT = (SELECT TOP 1 tme_id 
									  FROM ACA_TipoModalidadeEnsino WITH(NOLOCK)
									  WHERE tme_nome = 'Educação de Jovens e Adultos da Educação Especial'
									  AND tme_situacao <> 3);

	DECLARE @dataAtual DATETIME = GETDATE();

	IF (NOT EXISTS(SELECT TOP 1 1 
				   FROM ACA_TipoModalidadeEnsino WITH(NOLOCK) 
				   WHERE tme_nome = 'EJA Regular'
				   AND tme_situacao <> 3))
	BEGIN
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
			'EJA Regular',
			@tme_idEJA,
			1,
			@dataAtual,
			@dataAtual
		)
	END

	IF (NOT EXISTS(SELECT TOP 1 1 
				   FROM ACA_TipoModalidadeEnsino WITH(NOLOCK) 
				   WHERE tme_nome = 'EJA Modular'
				   AND tme_situacao <> 3))
	BEGIN
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
			'EJA Modular',
			@tme_idEJA,
			1,
			@dataAtual,
			@dataAtual
		)
	END

	IF (NOT EXISTS(SELECT TOP 1 1 
				   FROM ACA_TipoModalidadeEnsino WITH(NOLOCK) 
				   WHERE tme_nome = 'CIEJA'
				   AND tme_situacao <> 3))
	BEGIN
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
			'CIEJA',
			@tme_idEJA,
			1,
			@dataAtual,
			@dataAtual
		)
	END

	IF (ISNULL(@tme_idEJAEspecial, 0) > 0)
	BEGIN 
		UPDATE ACA_TipoModalidadeEnsino
		SET tme_idSuperior = @tme_idEJA,
			tme_nome = 'EJA Especial'
		WHERE tme_id = @tme_idEJAEspecial
	END

SET XACT_ABORT OFF
COMMIT TRANSACTION