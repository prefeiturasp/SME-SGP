USE [GestaoPedagogica]
GO
--Iniciar transação
BEGIN TRANSACTION
SET XACT_ABORT ON

	DECLARE @tme_idEJA INT;
	SELECT TOP 1
		@tme_idEJA = tme_id
	FROM
		ACA_TipoModalidadeEnsino tme WITH(NOLOCK)
	WHERE
		tme_nome = 'Educação de Jovens e Adultos - EJA'
		AND tme_idSuperior IS NULL
		AND tme_situacao <> 3

	DECLARE @tme_idEJAModular INT;
	SELECT TOP 1
		@tme_idEJAModular = tme_id
	FROM
		ACA_TipoModalidadeEnsino tme WITH(NOLOCK)
	WHERE
		tme_nome like '%Modular%'
		AND tme_idSuperior = @tme_idEJA
		AND tme_situacao <> 3

	UPDATE ACA_ParametroAcademico
	   SET pac_valor = @tme_idEJAModular
		, pac_dataAlteracao = GETDATE()
	 WHERE pac_chave = 'TIPO_MODALIDADES_EJA_REMOVER_RELATORIO'

SET XACT_ABORT OFF
COMMIT TRANSACTION

GO