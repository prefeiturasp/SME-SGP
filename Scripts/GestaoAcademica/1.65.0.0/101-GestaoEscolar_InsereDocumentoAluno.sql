USE [GestaoPedagogica]
GO

--Iniciar transação
BEGIN TRANSACTION
SET XACT_ABORT ON

	DECLARE @entId as uniqueidentifier;
	SELECT TOP 1 @entId = sse.ent_id 
	FROM 
		Synonym_SYS_SistemaEntidade AS sse WITH(NOLOCK)
		INNER JOIN  Synonym_SYS_Sistema AS ss WITH(NOLOCK)
			ON sse.sis_id = ss.sis_id
	WHERE 
		ss.sis_nome = ' SGP'
		
	EXEC dbo.[MS_InsereDocumentoAluno]
		@ent_id = @entId
		, @rlt_id = 271
		, @rda_nomeDocumento = 'Histórico escolar'
		
	INSERT INTO CFG_ParametroDocumentoAluno (ent_id, rlt_id, pda_id, pda_chave, pda_valor, pda_situacao, pda_dataAlteracao, pda_dataCriacao)
	VALUES (@entId, 271, 1, 'REPORT_DEVEXPRESS', 'True', 1, GETDATE(), GETDATE())

-- Fechar transação	
SET XACT_ABORT OFF
COMMIT TRANSACTION

GO