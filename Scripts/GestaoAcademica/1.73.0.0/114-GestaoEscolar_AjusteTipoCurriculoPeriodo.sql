USE [GestaoPedagogica]
GO

BEGIN TRANSACTION
SET XACT_ABORT ON
	
	UPDATE ACA_CurriculoPeriodo
	SET tcp_id = 75, crp_dataAlteracao = GETDATE()
	WHERE tcp_id = 45

	UPDATE ACA_CurriculoPeriodo
	SET tcp_id = 83, crp_dataAlteracao = GETDATE()
	WHERE tcp_id = 46

	UPDATE ACA_CurriculoPeriodo
	SET tcp_id = 84, crp_dataAlteracao = GETDATE()
	WHERE tcp_id = 47

	UPDATE ACA_CurriculoPeriodo
	SET tcp_id = 76, crp_dataAlteracao = GETDATE()
	WHERE tcp_id = 48

	UPDATE ACA_CurriculoPeriodo
	SET tcp_id = 77, crp_dataAlteracao = GETDATE()
	WHERE tcp_id = 49

	UPDATE ACA_CurriculoPeriodo
	SET tcp_id = 85, crp_dataAlteracao = GETDATE()
	WHERE tcp_id = 50

	UPDATE ACA_CurriculoPeriodo
	SET tcp_id = 78, crp_dataAlteracao = GETDATE()
	WHERE tcp_id = 52

	UPDATE ACA_CurriculoPeriodo
	SET tcp_id = 79, crp_dataAlteracao = GETDATE()
	WHERE tcp_id = 53

	UPDATE ACA_CurriculoPeriodo
	SET tcp_id = 87, crp_dataAlteracao = GETDATE()
	WHERE tcp_id = 54

	UPDATE ACA_CurriculoPeriodo
	SET tcp_id = 88, crp_dataAlteracao = GETDATE()
	WHERE tcp_id = 55

	UPDATE ACA_CurriculoPeriodo
	SET tcp_id = 80, crp_dataAlteracao = GETDATE()
	WHERE tcp_id = 56

	UPDATE ACA_CurriculoPeriodo
	SET tcp_id = 81, crp_dataAlteracao = GETDATE()
	WHERE tcp_id = 57

	UPDATE ACA_CurriculoPeriodo
	SET tcp_id = 89, crp_dataAlteracao = GETDATE()
	WHERE tcp_id = 58

	UPDATE ACA_CurriculoPeriodo
	SET tcp_id = 90, crp_dataAlteracao = GETDATE()
	WHERE tcp_id = 59

	UPDATE ACA_CurriculoPeriodo
	SET tcp_id = 82, crp_dataAlteracao = GETDATE()
	WHERE tcp_id = 60

	UPDATE ACA_TipoCurriculoPeriodo
	SET tcp_situacao = 3, tcp_dataAlteracao = GETDATE()
	WHERE tcp_id IN (45,46,47,48,49,50,52,53,54,55,56,57,58,59,60)
	
SET XACT_ABORT OFF
COMMIT TRANSACTION
