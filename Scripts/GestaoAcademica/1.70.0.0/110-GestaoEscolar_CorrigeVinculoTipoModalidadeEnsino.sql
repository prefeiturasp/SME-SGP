USE [GestaoPedagogica]
GO

BEGIN TRANSACTION
SET XACT_ABORT ON
	
	DECLARE @tme_idEJA INT;
	SELECT
		@tme_idEJA = tme_id
	FROM
		ACA_TipoModalidadeEnsino WITH(NOLOCK)
	WHERE
		tme_nome = 'Educação de Jovens e Adultos - EJA'
		AND tme_idSuperior IS NULL
		AND tme_situacao <> 3

	DECLARE @tme_idEJAReg INT;
	SELECT
		@tme_idEJAReg = tme_id
	FROM
		ACA_TipoModalidadeEnsino WITH(NOLOCK)
	WHERE
		tme_nome = 'EJA - Regular'
		AND tme_idSuperior = @tme_idEJA
		AND tme_situacao <> 3

	DECLARE @tme_idEJAMod INT;
	SELECT
		@tme_idEJAMod = tme_id
	FROM
		ACA_TipoModalidadeEnsino WITH(NOLOCK)
	WHERE
		tme_nome = 'EJA - Modular'
		AND tme_idSuperior = @tme_idEJA
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
	SET tme_id = @tme_idEJAReg
	WHERE tme_id = @tme_idEJA
	AND cur_nome_abreviado = 'EJA'
	AND cur_situacao <> 3
	
	UPDATE ACA_Curso
	SET tme_id = @tme_idEJAMod
	WHERE tme_id = @tme_idEJA
	AND cur_nome_abreviado = 'EJA MOD'
	AND cur_situacao <> 3

	UPDATE ACA_Curso
	SET tme_id = @tme_idCIEJA
	WHERE tme_id = @tme_idEJA
	AND cur_nome_abreviado = 'CIEJA'
	AND cur_situacao <> 3

	UPDATE tcp
		SET tcp.tme_id = cur.tme_id	
	FROM ACA_Curso cur
	INNER JOIN ACA_CurriculoPeriodo crp
		ON cur.cur_id = crp.cur_id
	INNER JOIN ACA_TipoCurriculoPeriodo tcp
		ON crp.tcp_id = tcp.tcp_id

SET XACT_ABORT OFF
COMMIT TRANSACTION
