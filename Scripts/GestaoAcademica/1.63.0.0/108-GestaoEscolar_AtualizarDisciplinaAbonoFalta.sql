USE [GestaoPedagogica]
GO

--Iniciar transação
BEGIN TRANSACTION
SET XACT_ABORT ON

	UPDATE TUR_TurmaDisciplina 
	SET tud_permitirLancarAbonoFalta = 1
	WHERE tud_nome IN ('Educação Física', 'Ensino Religioso', 'Língua espanhola')    

--Fechar transação
SET XACT_ABORT OFF
COMMIT TRANSACTION
