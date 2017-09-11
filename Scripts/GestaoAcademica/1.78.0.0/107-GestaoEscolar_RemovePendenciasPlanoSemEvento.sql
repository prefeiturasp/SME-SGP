USE [GestaoPedagogica]
GO

--Iniciar transação
BEGIN TRANSACTION
SET XACT_ABORT ON

	DECLARE @dataAtual DATE = CAST(GETDATE() AS DATE);
	DECLARE @tev_EfetivacaoNotas INT = 
		(
			SELECT TOP 1 CAST(pac.pac_valor AS INT)
			FROM ACA_ParametroAcademico pac WITH(NOLOCK)
			WHERE pac.pac_situacao <> 3 AND pac.pac_chave = 'TIPO_EVENTO_EFETIVACAO_NOTAS'
		);

	DELETE apn
	FROM CLS_TurmaAulaPendencia apn WITH(NOLOCK)
	INNER JOIN CLS_TurmaAula tau WITH(NOLOCK)
		ON tau.tud_id = apn.tud_id
		AND tau.tau_id = apn.tau_id
		AND tau.tau_situacao <> 3
	INNER JOIN TUR_TurmaRelTurmaDisciplina relTud WITH(NOLOCK)
		ON relTud.tud_id = tau.tud_id
	INNER JOIN TUR_Turma tur WITH(NOLOCK)
		ON tur.tur_id = relTud.tur_id
	WHERE
	NOT EXISTS
	(
		SELECT TOP 1 1 
		FROM ACA_Evento evt WITH(NOLOCK)
		INNER JOIN ACA_CalendarioEvento cae WITH(NOLOCK)
			ON cae.evt_id = evt.evt_id
			AND cae.cal_id = tur.cal_id
		WHERE evt.tev_id = @tev_EfetivacaoNotas
		AND evt.tpc_id = tau.tpc_id
		AND evt.evt_dataInicio <= @dataAtual
		AND 
		(
			evt.evt_padrao = 1
			OR evt.esc_id = tur.esc_id
		)
		AND evt.evt_situacao <> 3
	)

-- Fechar transação	
SET XACT_ABORT OFF
COMMIT TRANSACTION