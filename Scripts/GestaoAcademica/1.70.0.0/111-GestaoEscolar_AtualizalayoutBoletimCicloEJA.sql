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

	;WITH TipoCicloEJA AS
	(
		SELECT
			tci.tci_id
		FROM
			ACA_Curso cur WITH(NOLOCK)
			INNER JOIN ACA_CurriculoPeriodo crp WITH(NOLOCK)
				ON crp.cur_id = cur.cur_id
				AND crp.crp_situacao <> 3
			INNER JOIN ACA_TipoCiclo tci WITH(NOLOCK)
				ON tci.tci_id = crp.tci_id
				AND tci.tci_situacao <> 3
		WHERE
			cur.tme_id = @tme_idEJAReg
			AND cur.cur_situacao <> 3
		GROUP BY
			tci.tci_id
	)
	
	UPDATE tci
	SET tci.tci_layout = 'cicloEja'
	FROM TipoCicloEJA tca
	INNER JOIN ACA_TipoCiclo tci 
		ON tci.tci_id = tca.tci_id

SET XACT_ABORT OFF
COMMIT TRANSACTION