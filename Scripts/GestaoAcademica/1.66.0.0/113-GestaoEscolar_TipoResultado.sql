USE [GestaoPedagogica]
GO

BEGIN TRANSACTION 
SET XACT_ABORT ON 

	DECLARE @id INT
	DECLARE @tne_idInfantil INT = (SELECT TOP 1 CAST(pac_valor AS INT) FROM ACA_ParametroAcademico WITH(NOLOCK) WHERE pac_chave = 'TIPO_NIVEL_ENSINO_EDUCACAO_INFANTIL' AND pac_situacao <> 3)

	INSERT INTO ACA_TipoResultado (tpr_resultado, tpr_nomenclatura, tpr_situacao, tpr_dataCriacao, tpr_dataAlteracao, tpr_tipoLancamento)
	VALUES (1, 'Aprovado', 1, GETDATE(), GETDATE(), 1)

	SET @id = @@IDENTITY

	INSERT INTO ACA_TipoResultadoCurriculoPeriodo (tpr_id, cur_id, crr_id, crp_id)
	SELECT @id, cur.cur_id, crp.crr_id, crp.crp_id
	FROM ACA_Curso cur WITH(NOLOCK)
	INNER JOIN ACA_CurriculoPeriodo crp WITH(NOLOCK)
		ON cur.cur_id = crp.cur_id
		AND crp.crp_situacao <> 3
	WHERE
		cur.tne_id = @tne_idInfantil
		AND cur.cur_situacao <> 3
	GROUP BY
		cur.cur_id, crp.crr_id, crp.crp_id

	INSERT INTO ACA_TipoResultado (tpr_resultado, tpr_nomenclatura, tpr_situacao, tpr_dataCriacao, tpr_dataAlteracao, tpr_tipoLancamento)
	VALUES (1, 'Aprovado', 1, GETDATE(), GETDATE(), 2)

	SET @id = @@IDENTITY

	INSERT INTO ACA_TipoResultadoCurriculoPeriodo (tpr_id, cur_id, crr_id, crp_id)
	SELECT @id, cur.cur_id, crp.crr_id, crp.crp_id
	FROM ACA_Curso cur WITH(NOLOCK)
	INNER JOIN ACA_CurriculoPeriodo crp WITH(NOLOCK)
		ON cur.cur_id = crp.cur_id
		AND crp.crp_situacao <> 3
	WHERE
		cur.tne_id = @tne_idInfantil
		AND cur.cur_situacao <> 3
	GROUP BY
		cur.cur_id, crp.crr_id, crp.crp_id
		
-- Fechar transação     
SET XACT_ABORT OFF 
COMMIT TRANSACTION
