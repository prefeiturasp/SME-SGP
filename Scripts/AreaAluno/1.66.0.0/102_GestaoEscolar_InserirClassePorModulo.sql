USE [GestaoPedagogica]
GO

--Iniciar transação
BEGIN TRANSACTION
SET XACT_ABORT ON

    DECLARE @nomeSistema VARCHAR(200) = 'Boletim Online'
	DECLARE @sis_id INT = (SELECT TOP 1 S.sis_id FROM Synonym_SYS_Sistema AS S WITH(NOLOCK) WHERE S.sis_nome = @nomeSistema AND S.sis_situacao <> 3)
	DECLARE @dataAtual DATETIME = GETDATE()

	INSERT INTO CFG_ModuloClasse (mod_id, mdc_id, mdc_classe, mdc_situacao, mdc_dataAlteracao, mdc_dataCriacao)
	SELECT 
		M.mod_id, 
		1 AS id, 
		CASE M.mod_nome 
			WHEN ('Calendário escolar') THEN 'roxo'
			WHEN ('Documentos da escola') THEN 'branco'
		END AS classe, 
		1 AS situacao, 
		@dataAtual AS dataAlteracao,
		@dataAtual AS dataCriacao
	FROM 
		Synonym_SYS_Modulo AS M WITH(NOLOCK)
	WHERE
		M.sis_id = @sis_id
		AND M.mod_situacao <> 3
		AND M.mod_nome IN ('Calendário escolar', 'Documentos da escola')
		AND NOT EXISTS (
			SELECT * 
			FROM CFG_ModuloClasse AS Mdc WITH(NOLOCK)
			WHERE
				Mdc.mod_id = M.mod_id
		)
	ORDER BY 
		M.mod_id

--Fechar transação
SET XACT_ABORT OFF
COMMIT TRANSACTION