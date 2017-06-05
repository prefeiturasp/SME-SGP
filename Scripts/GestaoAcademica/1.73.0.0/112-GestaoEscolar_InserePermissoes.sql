USE [GestaoPedagogica]
GO

BEGIN TRANSACTION
SET XACT_ABORT ON

	INSERT INTO CFG_PermissaoDocente
	(
		tdc_id,
		tdc_idPermissao,
		pdc_modulo,
		pdc_permissaoConsulta,
		pdc_permissaoEdicao,
		pdc_situacao,
		pdc_dataCriacao,
		pdc_dataAlteracao
	)
	SELECT
		tdc_id,
		tdc_idPermissao,
		12,
		pdc_permissaoConsulta,
		pdc_permissaoEdicao,
		1,
		GETDATE(),
		GETDATE()
	FROM
		CFG_PermissaoDocente
	WHERE
		pdc_modulo = 6
		AND pdc_situacao = 1

SET XACT_ABORT OFF
COMMIT TRANSACTION	