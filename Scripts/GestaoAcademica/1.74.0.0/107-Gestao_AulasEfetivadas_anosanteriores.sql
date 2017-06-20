USE GestaoPedagogica
GO

--Iniciar transação
BEGIN TRANSACTION
SET XACT_ABORT ON
	
	DECLARE @cal_anoMaximo INT = 2016;
	declare @tau_dataAlteracao DATETIME = GETDATE();

	DECLARE @aulas TABLE (esc_id INT, tud_id BIGINT, tau_id INT, tau_numeroAulas INT primary key (tud_id, tau_id));

	; WITH TurmaDisciplina AS 
	(
		SELECT tur.tur_id, tur_codigo, Tur.esc_id, Tud.tud_id, tud.tud_nome
		FROM TUR_Turma Tur WITH(NOLOCK)
		INNER JOIN TUR_TurmaRelTurmaDisciplina RelTur WITH(NOLOCK)
			ON RelTur.tur_id = Tur.tur_id
		INNER JOIN TUR_TurmaDisciplina Tud WITH(NOLOCK)
			ON Tud.tud_id = RelTur.tud_id
		INNER JOIN ACA_CalendarioAnual Cal WITH(NOLOCK)
			ON Cal.cal_id = Tur.cal_id
		WHERE
			Tur.tur_situacao <> 3
			AND Tud.tud_situacao <> 3
			AND Cal.cal_ano <= @cal_anoMaximo
	)
	INSERT INTO @aulas
	(esc_id, tud_Id , tau_id, tau_numeroAulas)
	select esc_id, Tud.tud_Id , tau_id, tau_numeroAulas
	from TurmaDisciplina Tud
	INNER JOIN CLS_TurmaAula A WITH(NOLOCK)
	ON Tud.tud_id = A.tud_id
	AND A.tau_situacao <> 3
	WHERE 
		ISNULL(A.tau_efetivado, 0) = 0
	
	select count(*) Aulas from @aulas 
	
	--4 092 685

	/**************************************/

	-- Update

	BEGIN
		UPDATE CLS_TurmaAula
		set tau_efetivado = 1, tau_dataAlteracao = @tau_dataAlteracao
		FROM CLS_TurmaAula Tau WITH(NOLOCK)
		INNER JOIN @aulas A
		ON Tau.tud_id = A.tud_id
			AND Tau.tau_id = A.tau_id

		SELECT @@ROWCOUNT AS qtAulas
	END

-- Fechar transação
SET XACT_ABORT OFF
COMMIT TRANSACTION
