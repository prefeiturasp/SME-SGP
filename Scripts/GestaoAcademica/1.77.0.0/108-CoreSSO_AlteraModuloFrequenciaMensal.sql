USE [CoreSSO]
GO

BEGIN TRANSACTION
SET XACT_ABORT ON

DECLARE @mod_id INT

SELECT @mod_id = [mod_id]
	FROM [dbo].[SYS_Modulo]
	WHERE mod_nome = 'Frequência mensal'
	AND sis_id = 102

DECLARE @msm_ids TABLE (msm_id INT)

UPDATE [dbo].[SYS_Modulo]
	SET mod_nome = 'Relatório mensal'
	WHERE mod_id = @mod_id
	AND sis_id = 102

INSERT INTO @msm_ids 
	SELECT [msm_id]
	FROM [dbo].[SYS_ModuloSiteMap]
	WHERE mod_id = @mod_id
	AND sis_id = 102

UPDATE [dbo].[SYS_ModuloSiteMap]
	SET msm_nome = 'Relatório mensal'
	FROM [dbo].[SYS_ModuloSiteMap] MSM 
	INNER JOIN @msm_ids m
	ON msm.msm_id = m.msm_id
	AND mod_id = @mod_id
	AND sis_id = 102

SET XACT_ABORT OFF
COMMIT TRANSACTION	
GO


