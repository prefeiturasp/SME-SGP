USE [DES_SPO_CoreSSO]
GO

UPDATE MSM 
	SET [msm_nome] = 'Relatórios'
	FROM [SYS_ModuloSiteMap] MSM
	INNER JOIN [SYS_Modulo] AS Modulo
		ON	Modulo.[sis_id] = MSM.[sis_id]
		AND Modulo.[mod_id] = MSM.[mod_id]
	WHERE [msm_nome] = 'Documentos'
		AND MSM.[sis_id] = 102
		AND [mod_situacao] <> 3

GO