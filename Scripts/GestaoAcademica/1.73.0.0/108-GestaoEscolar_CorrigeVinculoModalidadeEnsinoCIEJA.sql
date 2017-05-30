USE [GestaoPedagogica]
GO

DECLARE @tme_idEJA INT;
	SELECT
		@tme_idEJA = tme_id
	FROM
		ACA_TipoModalidadeEnsino WITH(NOLOCK)
	WHERE
		tme_nome = 'Educação de Jovens e Adultos - EJA'
		AND tme_idSuperior IS NULL
		AND tme_situacao <> 3

DECLARE @tme_idCIEJA INT;
	SELECT
		@tme_idCIEJA = tme_id
	FROM
		ACA_TipoModalidadeEnsino WITH(NOLOCK)
	WHERE
		tme_nome = 'CIEJA'
		AND tme_idSuperior = @tme_idEJA
		AND tme_situacao <> 3

UPDATE ACA_Curso
	SET tme_id = @tme_idCIEJA
	WHERE tme_id = @tme_idEJA
	AND cur_nome_abreviado = 'CIEJA'
	AND cur_situacao <> 3

GO