USE [GestaoPedagogica]
GO

--Iniciar transação
BEGIN TRANSACTION
SET XACT_ABORT ON

	DECLARE @data DATETIME = GETDATE();

	MERGE INTO ACA_ConfiguracaoServicoPendencia AS Destino
	USING
	(
		SELECT
			2 AS tur_tipo
	) AS Origem
	ON Destino.tur_tipo = Origem.tur_tipo
	   AND Destino.csp_situacao <> 3
	WHEN MATCHED THEN
		UPDATE SET Destino.csp_semPlanoAula = CAST(1 AS BIT),
				   Destino.csp_dataAlteracao = @data
	WHEN NOT MATCHED THEN
		INSERT
			(
				tur_tipo,
				csp_semNota,
				csp_semParecer,
				csp_disciplinaSemAula,
				csp_semResultadoFinal,
				csp_semPlanejamento,
				csp_semSintese,
				csp_semPlanoAula,
				csp_semRelatorioAtendimento,
				csp_situacao,
				csp_dataCriacao,
				csp_dataAlteracao
			)
			VALUES
			(
				Origem.tur_tipo,
				CAST(0 AS BIT),
				CAST(0 AS BIT),
				CAST(0 AS BIT),
				CAST(0 AS BIT),
				CAST(0 AS BIT),
				CAST(0 AS BIT),
				CAST(1 AS BIT),
				0,
				1,
				@data,
				@data
			);

-- Fechar transação
SET XACT_ABORT OFF
COMMIT TRANSACTION