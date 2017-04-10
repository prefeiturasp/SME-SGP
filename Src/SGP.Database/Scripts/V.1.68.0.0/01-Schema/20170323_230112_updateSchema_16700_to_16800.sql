/*------------------------------------------------------------------------------------
   Project/Ticket#: SGP
   Description: Atualiza o schema do banco de dados GestaoPedagogica da 1.67.0.0 para 1.68.0.0
-------------------------------------------------------------------------------------*/

SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

PRINT N'Altering [dbo].[NEW_ACA_CalendarioAnualRelCurso_SelectBy_EscId]'
GO
-- ========================================================================
-- Author:		Diego Fadanni
-- Create date: 07/02/2014 13:00
-- Description:	Filtra todos os calendários não excluídos logicamente
-- filtrados entidade, escola
--
-- Alteração:	Leonardo Brito 14/03/2017
--				Alterada procedure para filtrar os calendários 
--				ligados à escola ou ao docente
-- ========================================================================
ALTER PROCEDURE [dbo].[NEW_ACA_CalendarioAnualRelCurso_SelectBy_EscId] 
	  @esc_id INT 	
	, @ent_id UNIQUEIDENTIFIER	
	, @doc_id BIGINT
	, @usu_id UNIQUEIDENTIFIER
	, @gru_id UNIQUEIDENTIFIER
AS
BEGIN

	DECLARE @tabelaUas TABLE (uad_id UNIQUEIDENTIFIER NOT NULL)
	DECLARE @cal_ids TABLE (cal_id INT)
	
	IF (ISNULL(@doc_id, 0) > 0)
	BEGIN
		INSERT INTO @cal_ids
		SELECT tur.cal_id FROM TUR_TurmaDocente tdt WITH(NOLOCK)
		INNER JOIN TUR_TurmaRelTurmaDisciplina trt WITH(NOLOCK) ON tdt.tud_id = trt.tud_id
		INNER JOIN TUR_Turma tur WITH(NOLOCK) ON trt.tur_id = tur.tur_id AND tur.tur_situacao <> 3
		WHERE tdt.doc_id = @doc_id AND tdt.tdt_situacao <> 3
		GROUP BY tur.cal_id
	END
	ELSE IF (@usu_id IS NOT NULL AND @gru_id IS NOT NULL)
	BEGIN
		INSERT INTO @tabelaUas 
		SELECT uad_id FROM Synonym_FN_Select_UAs_By_PermissaoUsuario(@usu_id, @gru_id) GROUP BY uad_id

		INSERT INTO @cal_ids
		SELECT cac.cal_id FROM @tabelaUas t
		INNER JOIN ESC_Escola esc WITH(NOLOCK) ON t.uad_id = esc.uad_id AND esc.esc_situacao <> 3
		INNER JOIN ACA_CurriculoEscola ces WITH(NOLOCK) ON esc.esc_id = ces.esc_id AND ces.ces_situacao <> 3
		INNER JOIN ACA_CalendarioCurso cac WITH(NOLOCK) ON ces.cur_id = cac.cur_id
		GROUP BY cac.cal_id
	END

	SELECT
		CAL.cal_id, Convert(VARCHAR,CAL.cal_ano) + ' - ' + CAL.cal_descricao AS cal_ano_desc	
	FROM
		ACA_CalendarioAnual	CAL WITH(NOLOCK)			
	INNER JOIN ACA_CalendarioCurso CALCUR WITH (NOLOCK)
		ON CALCUR.cal_id = CAL.cal_id
	INNER JOIN ACA_CurriculoEscola CES WITH (NOLOCK)	
		ON CALCUR.cur_id = CES.cur_id
		AND CES.esc_id = @esc_id
		AND CES.ces_situacao = 1
	WHERE
		CAL.cal_situacao <> 3
		AND CAL.ent_id = @ent_id
		AND ((ISNULL(@doc_id, 0) = 0 AND @usu_id IS NULL AND @gru_id IS NULL) OR
			 EXISTS(SELECT c.cal_id FROM @cal_ids c WHERE CAL.cal_id = c.cal_id))
	
	GROUP BY CAL.cal_id
			, CAL.cal_ano
			, CAL.cal_descricao			
	
	ORDER BY
		CAL.cal_ano DESC
		, CAL.cal_descricao DESC	

END


GO

PRINT N'Altering [dbo].[ACA_TipoCiclo]'
GO

ALTER TABLE [dbo].[ACA_TipoCiclo] ADD
[tci_objetoAprendizagem] [bit] NULL
GO

PRINT N'Creating [dbo].[NEW_ACA_TipoCiclo_AtualizaObjetoAprendizagem]'
GO
-- ========================================================================
---- Criado: Rafael Matias - 13/03/2017
---- Description: Atualiza a coluna tci_objetoAprendizagem
-- ========================================================================

CREATE PROCEDURE [dbo].[NEW_ACA_TipoCiclo_AtualizaObjetoAprendizagem]
	@tci_objetoAprendizagem BIT = NULL,
	@tci_id INT = NULL
AS
BEGIN
	UPDATE ACA_TipoCiclo 
	SET 
		tci_objetoAprendizagem = @tci_objetoAprendizagem,
		tci_dataAlteracao = GETDATE()
	WHERE 
		tci_id = @tci_id 

		
	DECLARE @ret INT
	SELECT @ret = ISNULL(@@ROWCOUNT,-1)
	RETURN @ret
	
END
GO

PRINT N'Refreshing [dbo].[VW_Avaliacoes_Por_TurmaAluno]'
GO
EXEC sp_refreshview N'[dbo].[VW_Avaliacoes_Por_TurmaAluno]'
GO

PRINT N'Altering [dbo].[NEW_ACA_CalendarioAnual_SelectBy_Entidade]'
GO
-- ==============================================================================
-- Author:		Carla Frascareli
-- Create date: 20/02/2012 17:30
-- Description:	Retorna os calendários da entidade
--
-- Alteração:	Leonardo Brito 14/03/2017
--				Alterada procedure para filtrar os calendários 
--				ligados à escola ou ao docente
-- ==============================================================================
ALTER PROCEDURE [dbo].[NEW_ACA_CalendarioAnual_SelectBy_Entidade]
	@ent_id UNIQUEIDENTIFIER
	, @doc_id BIGINT
	, @usu_id UNIQUEIDENTIFIER
	, @gru_id UNIQUEIDENTIFIER
AS
BEGIN

	DECLARE @tabelaUas TABLE (uad_id UNIQUEIDENTIFIER NOT NULL)
	DECLARE @cal_ids TABLE (cal_id INT)
	
	IF (ISNULL(@doc_id, 0) > 0)
	BEGIN
		INSERT INTO @cal_ids
		SELECT tur.cal_id FROM TUR_TurmaDocente tdt WITH(NOLOCK)
		INNER JOIN TUR_TurmaRelTurmaDisciplina trt WITH(NOLOCK) ON tdt.tud_id = trt.tud_id
		INNER JOIN TUR_Turma tur WITH(NOLOCK) ON trt.tur_id = tur.tur_id AND tur.tur_situacao <> 3
		WHERE tdt.doc_id = @doc_id AND tdt.tdt_situacao <> 3
		GROUP BY tur.cal_id
	END
	ELSE IF (@usu_id IS NOT NULL AND @gru_id IS NOT NULL)
	BEGIN
		INSERT INTO @tabelaUas 
		SELECT uad_id FROM Synonym_FN_Select_UAs_By_PermissaoUsuario(@usu_id, @gru_id) GROUP BY uad_id

		INSERT INTO @cal_ids
		SELECT cac.cal_id FROM @tabelaUas t
		INNER JOIN ESC_Escola esc WITH(NOLOCK) ON t.uad_id = esc.uad_id AND esc.esc_situacao <> 3
		INNER JOIN ACA_CurriculoEscola ces WITH(NOLOCK) ON esc.esc_id = ces.esc_id AND ces.ces_situacao <> 3
		INNER JOIN ACA_CalendarioCurso cac WITH(NOLOCK) ON ces.cur_id = cac.cur_id
		GROUP BY cac.cal_id
	END

	SELECT
		cal.cal_id
		, cal.cal_ano
		, cal.cal_descricao
		, CONVERT(VARCHAR, cal.cal_dataInicio,103) + ' - ' + CONVERT(VARCHAR,cal.cal_dataFim,103) AS cal_periodoLetivo
		, Convert(VARCHAR,cal.cal_ano) + ' - ' + cal.cal_descricao AS cal_ano_desc
	FROM
		ACA_CalendarioAnual cal WITH (NOLOCK)
	WHERE
		cal.cal_situacao <> 3
		AND cal.ent_id = @ent_id
		AND ((ISNULL(@doc_id, 0) = 0 AND @usu_id IS NULL AND @gru_id IS NULL) OR
			 EXISTS(SELECT c.cal_id FROM @cal_ids c WHERE cal.cal_id = c.cal_id))
	ORDER BY
		cal.cal_ano DESC
		, cal.cal_descricao DESC
END

GO

PRINT N'Creating [dbo].[NEW_TUR_TurmaDisciplinaRelacionada_Update]'
GO


CREATE PROCEDURE [dbo].[NEW_TUR_TurmaDisciplinaRelacionada_Update]
	@tdr_id BIGINT
	, @tud_id BIGINT
	, @tud_idRelacionada BIGINT
	, @tdr_vigenciaInicio DATETIME
	, @tdr_vigenciaFim DATETIME
	, @tdr_situacao TINYINT
	, @tdr_dataAlteracao DATETIME

AS
BEGIN
	UPDATE TUR_TurmaDisciplinaRelacionada 
	SET 
		tdr_vigenciaInicio = @tdr_vigenciaInicio 
		, tdr_vigenciaFim = @tdr_vigenciaFim 
		, tdr_situacao = @tdr_situacao  
		, tdr_dataAlteracao = @tdr_dataAlteracao 

	WHERE 
		tdr_id = @tdr_id 
		AND tud_id = @tud_id 
		AND tud_idRelacionada = @tud_idRelacionada 

		
	DECLARE @ret INT
	SELECT @ret = ISNULL(@@ROWCOUNT,-1)
	RETURN @ret
END



GO

PRINT N'Creating [dbo].[ACA_ObjetoAprendizagem]'
GO
CREATE TABLE [dbo].[ACA_ObjetoAprendizagem]
(
[oap_id] [int] NOT NULL IDENTITY(1, 1),
[tds_id] [int] NOT NULL,
[oap_descricao] [nvarchar] (500) NOT NULL,
[cal_ano] [int] NOT NULL CONSTRAINT [DF_ACA_ObjetoAprendizagem_cal_ano] DEFAULT ((0)),
[oap_situacao] [tinyint] NOT NULL,
[oap_dataCriacao] [datetime] NOT NULL,
[oap_dataAlteracao] [datetime] NOT NULL
)
GO

PRINT N'Creating primary key [PK_ACA_ObjetoAprendizagem] on [dbo].[ACA_ObjetoAprendizagem]'
GO
ALTER TABLE [dbo].[ACA_ObjetoAprendizagem] ADD CONSTRAINT [PK_ACA_ObjetoAprendizagem] PRIMARY KEY CLUSTERED  ([oap_id])
GO

PRINT N'Creating [dbo].[STP_ACA_ObjetoAprendizagem_LOAD]'
GO

CREATE PROCEDURE [dbo].[STP_ACA_ObjetoAprendizagem_LOAD]
	@oap_id Int
	
AS
BEGIN
	SELECT	Top 1
		 oap_id  
		, tds_id 
		, oap_descricao 
		, cal_ano 
		, oap_situacao 
		, oap_dataCriacao 
		, oap_dataAlteracao 

 	FROM
 		ACA_ObjetoAprendizagem
	WHERE 
		oap_id = @oap_id
END

GO

PRINT N'Altering [dbo].[ACA_TipoCurriculoPeriodo]'
GO

ALTER TABLE [dbo].[ACA_TipoCurriculoPeriodo] ADD
[tcp_objetoAprendizagem] [bit] NOT NULL CONSTRAINT [DF__ACA_TipoC__tcp_o__3A1ACC2F] DEFAULT ((0))
GO

PRINT N'Creating [dbo].[STP_ACA_ObjetoAprendizagem_INSERT]'
GO

CREATE PROCEDURE [dbo].[STP_ACA_ObjetoAprendizagem_INSERT]
	@tds_id Int
	, @oap_descricao NVarChar (1000)
	, @cal_ano Int
	, @oap_situacao TinyInt
	, @oap_dataCriacao DateTime
	, @oap_dataAlteracao DateTime

AS
BEGIN
	INSERT INTO 
		ACA_ObjetoAprendizagem
		( 
			tds_id 
			, oap_descricao 
			, cal_ano 
			, oap_situacao 
			, oap_dataCriacao 
			, oap_dataAlteracao 
 
		)
	VALUES
		( 
			@tds_id 
			, @oap_descricao 
			, @cal_ano 
			, @oap_situacao 
			, @oap_dataCriacao 
			, @oap_dataAlteracao 
 
		)
		
		SELECT ISNULL(SCOPE_IDENTITY(),-1)

	
	
END

GO

PRINT N'Altering [dbo].[REL_TurmaDisciplinaSituacaoFechamento]'
GO

ALTER TABLE [dbo].[REL_TurmaDisciplinaSituacaoFechamento] ADD
[PendentePlanejamento] [bit] NOT NULL CONSTRAINT [DF_REL_TurmaDisciplinaSituacaoFechamento_PendentePlanejamento] DEFAULT ((0))
GO

PRINT N'Altering [dbo].[NEW_ACA_CalendarioAnual_SelectByCursoComDisciplinaEletiva]'
GO
-- ========================================================================
-- Author:		Renata Tiepo Fonseca
-- Create date: 10/10/2011 12:36
-- Description:	seleciona os calendarios anuais de cursos que possuem 
--				disciplina eletiva filtrando (ou não) por curso e por escola.
--
-- Alteração:	Leonardo Brito 14/03/2017
--				Alterada procedure para filtrar os calendários 
--				ligados à escola ou ao docente
-- ========================================================================
ALTER PROCEDURE [dbo].[NEW_ACA_CalendarioAnual_SelectByCursoComDisciplinaEletiva]	
	@ent_id UNIQUEIDENTIFIER
	, @esc_id INT
	, @uni_id INT
	, @tds_id INT
	, @cur_id INT
	, @doc_id BIGINT
	, @usu_id UNIQUEIDENTIFIER
	, @gru_id UNIQUEIDENTIFIER
AS
BEGIN

	DECLARE @tabelaUas TABLE (uad_id UNIQUEIDENTIFIER NOT NULL)
	DECLARE @cal_ids TABLE (cal_id INT)
	
	IF (ISNULL(@doc_id, 0) > 0)
	BEGIN
		INSERT INTO @cal_ids
		SELECT tur.cal_id FROM TUR_TurmaDocente tdt WITH(NOLOCK)
		INNER JOIN TUR_TurmaRelTurmaDisciplina trt WITH(NOLOCK) ON tdt.tud_id = trt.tud_id
		INNER JOIN TUR_Turma tur WITH(NOLOCK) ON trt.tur_id = tur.tur_id AND tur.tur_situacao <> 3
		WHERE tdt.doc_id = @doc_id AND tdt.tdt_situacao <> 3
		GROUP BY tur.cal_id
	END
	ELSE IF (@usu_id IS NOT NULL AND @gru_id IS NOT NULL)
	BEGIN
		INSERT INTO @tabelaUas 
		SELECT uad_id FROM Synonym_FN_Select_UAs_By_PermissaoUsuario(@usu_id, @gru_id) GROUP BY uad_id

		INSERT INTO @cal_ids
		SELECT cac.cal_id FROM @tabelaUas t
		INNER JOIN ESC_Escola esc WITH(NOLOCK) ON t.uad_id = esc.uad_id AND esc.esc_situacao <> 3
		INNER JOIN ACA_CurriculoEscola ces WITH(NOLOCK) ON esc.esc_id = ces.esc_id AND ces.ces_situacao <> 3
		INNER JOIN ACA_CalendarioCurso cac WITH(NOLOCK) ON ces.cur_id = cac.cur_id
		GROUP BY cac.cal_id
	END

	SELECT
		cal.cal_id
		, Convert(VARCHAR,cal.cal_ano) + ' - ' + cal.cal_descricao AS cal_ano_desc
	FROM
		ACA_CalendarioAnual cal WITH(NOLOCK)
	INNER JOIN ACA_CalendarioCurso cac WITH(NOLOCK)
		ON  cal.cal_id = cac.cal_id
		AND cac.cur_id = ISNULL(@cur_id, cac.cur_id)
	INNER JOIN ACA_Curso cur WITH(NOLOCK)
		ON  cac.cur_id = cur.cur_id
		AND cur.cur_situacao <> 3
	INNER JOIN ACA_Curriculo crr WITH(NOLOCK)
		ON  cur.cur_id = crr.cur_id	
		AND cur.ent_id = @ent_id
		AND crr.crr_situacao <> 3
	INNER JOIN ACA_CurriculoPeriodo crp WITH(NOLOCK)
		ON  cur.cur_id = crp.cur_id
		AND crr.crr_id = crp.crr_id
		AND crp.crp_qtdeEletivasAlunos > 0
		AND crp.crp_situacao <> 3
	INNER JOIN ACA_CurriculoDisciplina crd WITH(NOLOCK)
		ON  crd.cur_id = cur.cur_id
		AND crd.crr_id = crr.crr_id
		AND crd.crd_tipo = 10
	INNER JOIN ACA_Disciplina dis WITH(NOLOCK)
		ON  dis.dis_id = crd.dis_id
		AND dis.tds_id = @tds_id 
		AND dis.dis_situacao <> 3
	LEFT JOIN ACA_CurriculoEscola cre WITH(NOLOCK)
		ON 	cre.cur_id = crr.cur_id
		AND cre.crr_id = crr.crr_id 
		AND cre.ces_situacao <> 3
	WHERE
		cal.cal_situacao <> 3
		AND (@esc_id is null or cre.esc_id = @esc_id)			
		AND (@uni_id is null or cre.uni_id = @uni_id)
		AND ((ISNULL(@doc_id, 0) = 0 AND @usu_id IS NULL AND @gru_id IS NULL) OR
			 EXISTS(SELECT c.cal_id FROM @cal_ids c WHERE cal.cal_id = c.cal_id))		
	GROUP BY 
		cal.cal_id
		, cal.cal_ano
		, cal.cal_descricao
	ORDER BY
		cal.cal_ano DESC
		, cal.cal_descricao DESC
				
	SELECT @@ROWCOUNT	
END
GO

PRINT N'Creating [dbo].[STP_ACA_ObjetoAprendizagem_UPDATE]'
GO

CREATE PROCEDURE [dbo].[STP_ACA_ObjetoAprendizagem_UPDATE]
	@oap_id INT
	, @tds_id INT
	, @oap_descricao NVARCHAR (1000)
	, @cal_ano INT
	, @oap_situacao TINYINT
	, @oap_dataCriacao DATETIME
	, @oap_dataAlteracao DATETIME

AS
BEGIN
	UPDATE ACA_ObjetoAprendizagem 
	SET 
		tds_id = @tds_id 
		, oap_descricao = @oap_descricao 
		, cal_ano = @cal_ano 
		, oap_situacao = @oap_situacao 
		, oap_dataCriacao = @oap_dataCriacao 
		, oap_dataAlteracao = @oap_dataAlteracao 

	WHERE 
		oap_id = @oap_id 

		
	DECLARE @ret INT
	SELECT @ret = ISNULL(@@ROWCOUNT,-1)
	RETURN @ret
	
END

GO

PRINT N'Creating [dbo].[STP_ACA_ObjetoAprendizagem_DELETE]'
GO


CREATE PROCEDURE [dbo].[STP_ACA_ObjetoAprendizagem_DELETE]
	@oap_id INT

AS
BEGIN
	DELETE FROM 
		ACA_ObjetoAprendizagem 
	WHERE 
		oap_id = @oap_id 

		
	DECLARE @ret INT
	SELECT @ret = ISNULL(@@ROWCOUNT,-1)
	RETURN @ret
	
END

GO

PRINT N'Creating [dbo].[STP_ACA_ObjetoAprendizagem_SELECT]'
GO

CREATE PROCEDURE [dbo].[STP_ACA_ObjetoAprendizagem_SELECT]
	
AS
BEGIN
	SELECT 
		oap_id
		,tds_id
		,oap_descricao
		,cal_ano
		,oap_situacao
		,oap_dataCriacao
		,oap_dataAlteracao

	FROM 
		ACA_ObjetoAprendizagem WITH(NOLOCK) 
	
END

GO

PRINT N'Altering [dbo].[NEW_ACA_CalendarioAnual_SelectBy_cur_id_pfi_id]'
GO
-- ==============================================================================
-- Author:		Renata Prado
-- Create date: 09/01/2012 
-- Description:	Retorna os calendários que não foram excluídos 
--				logicamente por curso e por ano inicio processo
--
-- Alteração:	Leonardo Brito 14/03/2017
--				Alterada procedure para filtrar os calendários 
--				ligados à escola ou ao docente
-- ==============================================================================
ALTER PROCEDURE [dbo].[NEW_ACA_CalendarioAnual_SelectBy_cur_id_pfi_id]	
	@cur_id INT	
	, @ent_id UNIQUEIDENTIFIER
	, @pfi_id INT
	, @doc_id BIGINT
	, @usu_id UNIQUEIDENTIFIER
	, @gru_id UNIQUEIDENTIFIER
AS
BEGIN

	DECLARE @tabelaUas TABLE (uad_id UNIQUEIDENTIFIER NOT NULL)
	DECLARE @cal_ids TABLE (cal_id INT)
	
	IF (ISNULL(@doc_id, 0) > 0)
	BEGIN
		INSERT INTO @cal_ids
		SELECT tur.cal_id FROM TUR_TurmaDocente tdt WITH(NOLOCK)
		INNER JOIN TUR_TurmaRelTurmaDisciplina trt WITH(NOLOCK) ON tdt.tud_id = trt.tud_id
		INNER JOIN TUR_Turma tur WITH(NOLOCK) ON trt.tur_id = tur.tur_id AND tur.tur_situacao <> 3
		WHERE tdt.doc_id = @doc_id AND tdt.tdt_situacao <> 3
		GROUP BY tur.cal_id
	END
	ELSE IF (@usu_id IS NOT NULL AND @gru_id IS NOT NULL)
	BEGIN
		INSERT INTO @tabelaUas 
		SELECT uad_id FROM Synonym_FN_Select_UAs_By_PermissaoUsuario(@usu_id, @gru_id) GROUP BY uad_id

		INSERT INTO @cal_ids
		SELECT cac.cal_id FROM @tabelaUas t
		INNER JOIN ESC_Escola esc WITH(NOLOCK) ON t.uad_id = esc.uad_id AND esc.esc_situacao <> 3
		INNER JOIN ACA_CurriculoEscola ces WITH(NOLOCK) ON esc.esc_id = ces.esc_id AND ces.ces_situacao <> 3
		INNER JOIN ACA_CalendarioCurso cac WITH(NOLOCK) ON ces.cur_id = cac.cur_id
		GROUP BY cac.cal_id
	END

	SELECT
		cal.cal_id
		, Convert(VARCHAR,cal.cal_ano) + ' - ' + cal.cal_descricao AS cal_ano_desc
	FROM
		ACA_CalendarioAnual cal WITH (NOLOCK)
	INNER JOIN ACA_CalendarioCurso cac WITH (NOLOCK)
		ON cal.cal_id = cac.cal_id
		AND cac.cur_id = @cur_id	
	INNER JOIN MTR_ProcessoFechamentoInicio	pfi
		ON pfi.pfi_id = @pfi_id
		AND pfi.pfi_anoInicio = cal.cal_ano
	WHERE
		cal.cal_situacao <> 3
		AND cal.ent_id = @ent_id
		AND ((ISNULL(@doc_id, 0) = 0 AND @usu_id IS NULL AND @gru_id IS NULL) OR
			 EXISTS(SELECT c.cal_id FROM @cal_ids c WHERE cal.cal_id = c.cal_id))
	ORDER BY
		cal.cal_ano DESC
		, cal.cal_descricao DESC
		
	SELECT @@ROWCOUNT		
END
GO

PRINT N'Altering [dbo].[NEW_ACA_CalendarioAnual_SelectBy_CursoQtdePeriodos]'
GO
-- ==============================================================================
-- Author:		Jean Michel Marques da Silva
-- Create date: 14/09/2011 10:30
-- Description:	Retorna os calendários que não foram excluídos 
--				logicamente, por curso e quantidade de períodos
--
-- Alteração:	Leonardo Brito 14/03/2017
--				Alterada procedure para filtrar os calendários 
--				ligados à escola ou ao docente
-- ==============================================================================
ALTER PROCEDURE [dbo].[NEW_ACA_CalendarioAnual_SelectBy_CursoQtdePeriodos]	
	@cur_id INT	
	, @qtdePeriodos INT
	, @ent_id UNIQUEIDENTIFIER	
	, @doc_id BIGINT
	, @usu_id UNIQUEIDENTIFIER
	, @gru_id UNIQUEIDENTIFIER
AS
BEGIN

	DECLARE @tabelaUas TABLE (uad_id UNIQUEIDENTIFIER NOT NULL)
	DECLARE @cal_ids TABLE (cal_id INT)
	
	IF (ISNULL(@doc_id, 0) > 0)
	BEGIN
		INSERT INTO @cal_ids
		SELECT tur.cal_id FROM TUR_TurmaDocente tdt WITH(NOLOCK)
		INNER JOIN TUR_TurmaRelTurmaDisciplina trt WITH(NOLOCK) ON tdt.tud_id = trt.tud_id
		INNER JOIN TUR_Turma tur WITH(NOLOCK) ON trt.tur_id = tur.tur_id AND tur.tur_situacao <> 3
		WHERE tdt.doc_id = @doc_id AND tdt.tdt_situacao <> 3
		GROUP BY tur.cal_id
	END
	ELSE IF (@usu_id IS NOT NULL AND @gru_id IS NOT NULL)
	BEGIN
		INSERT INTO @tabelaUas 
		SELECT uad_id FROM Synonym_FN_Select_UAs_By_PermissaoUsuario(@usu_id, @gru_id) GROUP BY uad_id

		INSERT INTO @cal_ids
		SELECT cac.cal_id FROM @tabelaUas t
		INNER JOIN ESC_Escola esc WITH(NOLOCK) ON t.uad_id = esc.uad_id AND esc.esc_situacao <> 3
		INNER JOIN ACA_CurriculoEscola ces WITH(NOLOCK) ON esc.esc_id = ces.esc_id AND ces.ces_situacao <> 3
		INNER JOIN ACA_CalendarioCurso cac WITH(NOLOCK) ON ces.cur_id = cac.cur_id
		GROUP BY cac.cal_id
	END

	SELECT
		cal.cal_id
		, Convert(VARCHAR,cal.cal_ano) + ' - ' + cal.cal_descricao AS cal_ano_desc
	FROM
		ACA_CalendarioAnual cal WITH (NOLOCK)
	INNER JOIN ACA_CalendarioCurso cac WITH (NOLOCK)
		ON cal.cal_id = cac.cal_id
	WHERE
		cal_situacao <> 3				
		AND cac.cur_id = @cur_id		
		AND cal.ent_id = @ent_id
		AND ((ISNULL(@doc_id, 0) = 0 AND @usu_id IS NULL AND @gru_id IS NULL) OR
			 EXISTS(SELECT c.cal_id FROM @cal_ids c WHERE cal.cal_id = c.cal_id))
		AND @qtdePeriodos = (
								SELECT
									COUNT(*)
								FROM
									ACA_CalendarioPeriodo cap WITH (NOLOCK)
								WHERE 
									cap_situacao <> 3 
									AND cap.cal_id = cal.cal_id
							)
	ORDER BY
		cal.cal_ano DESC
		, cal.cal_descricao DESC
		
	SELECT @@ROWCOUNT		
END
GO

PRINT N'Creating [dbo].[NEW_ACA_ObjetoAprendizagem_UPDATE]'
GO


-- ========================================================================
-- Author:		Rafael Matias
-- Create date: 10/10/2014
-- Description:	Altera o objeto de aprendizagem conservando a data da criação
-- ========================================================================
CREATE PROCEDURE [dbo].[NEW_ACA_ObjetoAprendizagem_UPDATE]
	@oap_id INT
	, @oap_descricao VARCHAR (500)
	, @tds_id INT
	, @cal_ano INT
	, @oap_situacao TINYINT
	, @oap_dataAlteracao DATETIME

AS
BEGIN
	UPDATE ACA_ObjetoAprendizagem 
	SET 
		oap_descricao = @oap_descricao 
		, tds_id = @tds_id 
		, cal_ano = @cal_ano
		, oap_situacao = @oap_situacao 
		, oap_dataAlteracao = @oap_dataAlteracao 

	WHERE 
		oap_id = @oap_id 

		
	DECLARE @ret INT
	SELECT @ret = ISNULL(@@ROWCOUNT,-1)
	RETURN @ret
	
END


GO

PRINT N'Altering [dbo].[STP_REL_TurmaDisciplinaSituacaoFechamento_INSERT]'
GO

ALTER PROCEDURE [dbo].[STP_REL_TurmaDisciplinaSituacaoFechamento_INSERT]
	@tud_id BigInt
	, @esc_id Int
	, @cal_id Int
	, @Pendente Bit
	, @PendentePlanejamento Bit
	, @PendenteParecer Bit
	, @DataProcessamento DateTime

AS
BEGIN
	INSERT INTO 
		REL_TurmaDisciplinaSituacaoFechamento
		( 
			tud_id 
			, esc_id 
			, cal_id 
			, Pendente 
			, PendentePlanejamento
			, PendenteParecer
			, DataProcessamento 
 
		)
	VALUES
		( 
			@tud_id 
			, @esc_id 
			, @cal_id 
			, @Pendente 
			, @PendentePlanejamento
			, @PendenteParecer
			, @DataProcessamento 
 
		)
		
		SELECT ISNULL(SCOPE_IDENTITY(),-1)

	
	
END

GO

PRINT N'Creating [dbo].[NEW_ACA_ObjetoAprendizagem_UPDATE_Situacao]'
GO

-- ===================================================================
-- Author:		Rafael Matias
-- Create date: 15/03/2017
-- Description:	utilizado para realizar alteração do campo de situação 
--				(1 – Ativo ; 3 – Excluído) referente ao objeto de aprendizagem.
--				Filtrada por: 
--					oap_id
-- ====================================================================
CREATE PROCEDURE [dbo].[NEW_ACA_ObjetoAprendizagem_UPDATE_Situacao]	
		@oap_id INT
		,@oap_situacao TINYINT
		,@oap_dataAlteracao DATETIME
AS
BEGIN
	UPDATE ACA_ObjetoAprendizagem
	SET 
		oap_situacao = @oap_situacao
		,oap_dataAlteracao = @oap_dataAlteracao
	WHERE 
		oap_id = @oap_id
		
	RETURN ISNULL(@@ROWCOUNT,-1)
END


GO

PRINT N'Altering [dbo].[STP_REL_TurmaDisciplinaSituacaoFechamento_UPDATE]'
GO

ALTER PROCEDURE [dbo].[STP_REL_TurmaDisciplinaSituacaoFechamento_UPDATE]
	@tud_id BIGINT
	, @esc_id INT
	, @cal_id INT
	, @Pendente BIT
	, @PendentePlanejamento BIT
	, @PendenteParecer BIT
	, @DataProcessamento DATETIME

AS
BEGIN
	UPDATE REL_TurmaDisciplinaSituacaoFechamento 
	SET 
		esc_id = @esc_id 
		, cal_id = @cal_id 
		, Pendente = @Pendente 
		, PendentePlanejamento = @PendentePlanejamento
		, PendenteParecer = @PendenteParecer
		, DataProcessamento = @DataProcessamento 

	WHERE 
		tud_id = @tud_id 

		
	DECLARE @ret INT
	SELECT @ret = ISNULL(@@ROWCOUNT,-1)
	RETURN @ret
	
END

GO

PRINT N'Altering [dbo].[STP_REL_TurmaDisciplinaSituacaoFechamento_LOAD]'
GO

ALTER PROCEDURE [dbo].[STP_REL_TurmaDisciplinaSituacaoFechamento_LOAD]
	@tud_id BigInt
	
AS
BEGIN
	SELECT	Top 1
		 tud_id  
		, esc_id 
		, cal_id 
		, Pendente 
		, PendentePlanejamento
		, PendenteParecer
		, DataProcessamento 

 	FROM
 		REL_TurmaDisciplinaSituacaoFechamento WITH(NOLOCK)
	WHERE 
		tud_id = @tud_id
END

GO

PRINT N'Creating [dbo].[NEW_ACA_TipoCurriculoPeriodo_UPDATE]'
GO

CREATE PROCEDURE [dbo].[NEW_ACA_TipoCurriculoPeriodo_UPDATE]
	@tcp_id INT
	, @tne_id INT
	, @tme_id INT
	, @tcp_descricao VARCHAR (100)
	, @tcp_ordem TINYINT
	, @tcp_situacao TINYINT
	, @tcp_dataAlteracao DATETIME
	, @tcp_objetoAprendizagem BIT

AS
BEGIN
	UPDATE ACA_TipoCurriculoPeriodo 
	SET 
		tne_id = @tne_id 
		, tme_id = @tme_id 
		, tcp_descricao = @tcp_descricao 
		, tcp_ordem = @tcp_ordem 
		, tcp_situacao = @tcp_situacao 
		, tcp_dataAlteracao = @tcp_dataAlteracao 
		, tcp_objetoAprendizagem = @tcp_objetoAprendizagem
	WHERE 
		tcp_id = @tcp_id 

		
	DECLARE @ret INT
	SELECT @ret = ISNULL(@@ROWCOUNT,-1)
	RETURN @ret
	
END

GO

PRINT N'Creating [dbo].[NEW_TUR_TurmaDisciplinaRelacionada_Update_Situacao]'
GO


-- ===================================================================
-- Author:		Marcia Haga
-- Create date: 02/12/2014
-- Description:	Utilizado para realizar alteração do campo de situação 
--				filtrado por:	tdr_id, tud_id e tud_idRelacionada
-- ====================================================================
CREATE PROCEDURE [dbo].[NEW_TUR_TurmaDisciplinaRelacionada_Update_Situacao]
	@tdr_id BIGINT
	, @tud_id BIGINT
	, @tud_idRelacionada BIGINT
	, @tdr_situacao TINYINT
	, @tdr_dataAlteracao DATETIME

AS
BEGIN
	UPDATE TUR_TurmaDisciplinaRelacionada 
	SET 
		tdr_situacao = @tdr_situacao 		
		, tdr_dataAlteracao = @tdr_dataAlteracao 

	WHERE 
		tdr_id = @tdr_id 
		AND tud_id = @tud_id 
		AND tud_idRelacionada = @tud_idRelacionada
		
	RETURN ISNULL(@@ROWCOUNT,-1)	
END



GO

PRINT N'Altering [dbo].[NEW_ACA_TipoDisciplina_SelectBy_Pesquisa]'
GO
-- ================================================================================
-- Author:		Jean Michel Marques da Silva
-- Create date: 15/06/2010 09:07
-- Description:	Retorna os tipos de disciplina que não foram excluídos logicamente.
-- ================================================================================
ALTER PROCEDURE [dbo].[NEW_ACA_TipoDisciplina_SelectBy_Pesquisa]	
	@tds_id INT
	, @tne_id INT
	, @tds_base TINYINT
	, @tds_idNaoConsiderar INT
	, @controlarOrdem BIT
AS
BEGIN
	SELECT 
		tds_id
		, tds_nome
		, tds_ordem
		, tne_nome
		, tne_nome + ' - ' + tds_nome as tne_tds_nome 		
		, CASE tds_situacao 
			WHEN 1 THEN 'Não'
			WHEN 2 THEN 'Sim'			
		  END AS tds_situacao
		, CASE tds_base 
			WHEN 1 THEN 'Nacional'
			WHEN 2 THEN 'Diversificada'			
		  END AS tds_base_nome
		, tds_base
	FROM
		ACA_TipoDisciplina WITH (NOLOCK)
	INNER JOIN ACA_TipoNivelEnsino WITH (NOLOCK)
		ON ACA_TipoNivelEnsino.tne_id = ACA_TipoDisciplina.tne_id
	WHERE
		tds_situacao <> 3
		AND tne_situacao <> 3		
		AND (@tds_id IS NULL OR ACA_TipoDisciplina.tds_id = @tds_id)
		AND (@tne_id IS NULL OR ACA_TipoDisciplina.tne_id = @tne_id)
		AND (@tds_base IS NULL OR ACA_TipoDisciplina.tds_base = @tds_base)
		AND (@tds_idNaoConsiderar IS NULL OR ACA_TipoDisciplina.tds_id <> @tds_idNaoConsiderar) 
	ORDER BY 		
	 CASE WHEN @controlarOrdem = 1 THEN tds_ordem END
	,CASE WHEN @controlarOrdem = 0 THEN tne_nome + ' - ' + tds_nome END 
			
	SELECT @@ROWCOUNT			
END

GO

PRINT N'Altering [dbo].[NEW_ACA_CalendarioAnual_SelectBy_Esc_id]'
GO
-- ========================================================================
-- Author:		Rodolfo 
-- Create date: 27/07/2011 13:00
-- Description:	Filtra calendario por esc_id
--
-- Alteração:	Leonardo Brito 14/03/2017
--				Alterada procedure para filtrar os calendários 
--				ligados à escola ou ao docente
-- ========================================================================
ALTER PROCEDURE [dbo].[NEW_ACA_CalendarioAnual_SelectBy_Esc_id] 
	  @esc_id INT 	
	, @ent_id UNIQUEIDENTIFIER
	, @doc_id BIGINT
	, @usu_id UNIQUEIDENTIFIER
	, @gru_id UNIQUEIDENTIFIER	
AS
BEGIN

	DECLARE @tabelaUas TABLE (uad_id UNIQUEIDENTIFIER NOT NULL)
	DECLARE @cal_ids TABLE (cal_id INT)
	
	IF (ISNULL(@doc_id, 0) > 0)
	BEGIN
		INSERT INTO @cal_ids
		SELECT tur.cal_id FROM TUR_TurmaDocente tdt WITH(NOLOCK)
		INNER JOIN TUR_TurmaRelTurmaDisciplina trt WITH(NOLOCK) ON tdt.tud_id = trt.tud_id
		INNER JOIN TUR_Turma tur WITH(NOLOCK) ON trt.tur_id = tur.tur_id AND tur.tur_situacao <> 3
		WHERE tdt.doc_id = @doc_id AND tdt.tdt_situacao <> 3
		GROUP BY tur.cal_id
	END
	ELSE IF (@usu_id IS NOT NULL AND @gru_id IS NOT NULL)
	BEGIN
		INSERT INTO @tabelaUas 
		SELECT uad_id FROM Synonym_FN_Select_UAs_By_PermissaoUsuario(@usu_id, @gru_id) GROUP BY uad_id

		INSERT INTO @cal_ids
		SELECT cac.cal_id FROM @tabelaUas t
		INNER JOIN ESC_Escola esc WITH(NOLOCK) ON t.uad_id = esc.uad_id AND esc.esc_situacao <> 3
		INNER JOIN ACA_CurriculoEscola ces WITH(NOLOCK) ON esc.esc_id = ces.esc_id AND ces.ces_situacao <> 3
		INNER JOIN ACA_CalendarioCurso cac WITH(NOLOCK) ON ces.cur_id = cac.cur_id
		GROUP BY cac.cal_id
	END

	SELECT 
		cal.cal_id, Convert(VARCHAR,cal.cal_ano) + ' - ' + cal.cal_descricao AS cal_ano_desc	
	FROM
		ACA_CalendarioAnual	cal WITH(NOLOCK)			
	INNER JOIN TUR_TURMA TUR WITH (NOLOCK)
		ON TUR.cal_id = CAL.cal_id		
		AND TUR.esc_id = @esc_id
		AND TUR.tur_situacao = 1
	WHERE
		cal_situacao <> 3
		AND ent_id = @ent_id
		AND ((ISNULL(@doc_id, 0) = 0 AND @usu_id IS NULL AND @gru_id IS NULL) OR
			 EXISTS(SELECT c.cal_id FROM @cal_ids c WHERE CAL.cal_id = c.cal_id))
	GROUP BY 
		cal.cal_id
		, cal.cal_ano
		, cal.cal_descricao
				
	ORDER BY
		cal_ano DESC
		, cal_descricao DESC	

END

GO

PRINT N'Altering [dbo].[NEW_TUR_TurmaDisciplina_SelectClassesBy_Docente_SemVigencia]'
GO
--==========================================================================
-- Author:		Daniel Jun Suguimoto
-- Create date: 20/08/2013
-- Description: Carrega as turmas e disciplinas que o docente 
--				dá aula, mais as que ele é coordenador de disciplina. 
--				(Sem considerar se o periodo é vigente)
--				Não traz as disciplinas do tipo 13-Docente específico – complementação da regência.

---- Alterado: Marcia Haga - 02/12/2014
---- Description: Adicionado filtro por turmas ativas, dependendo do parametro.

---- Alterado: Marcia Haga - 05/12/2014
---- Description: Alterado para considerar o novo tipo de disciplina para docencia compartilhada.

---- Alterado: Marcia Haga - 08/12/2014
---- Description: Alterado para considerar a disciplina relacionada vigente no momento.

---- Alterado: Marcia Haga - 09/01/2015
---- Description: Alterado para retornar as disciplinas ordenadas pelo tds_ordem,
---- caso o parametro CONTROLAR_ORDEM_DISCIPLINAS seja 1.
---- Alterado para retornar o tud_nome sozinho.

---- Alterado: Marcia Haga - 05/06/2015
---- Description: Alterada validacao para disciplina compartilhada vigente.

---- Alterado: Leonardo Brito 05/08/2015
---- Description:  Removido nome da escola do tur_tud_nome devido ao bug 27834
-- ========================================================================
ALTER PROCEDURE [dbo].[NEW_TUR_TurmaDisciplina_SelectClassesBy_Docente_SemVigencia]
	@doc_id BIGINT
	, @fav_tipoLancamentoFrequencia TINYINT
	, @regencia TINYINT
	, @tipoRegencia INT
	, @filtroTurmasAtivas BIT
AS
BEGIN

	DECLARE @controlarOrdem BIT;
	SELECT @controlarOrdem = isnull(pac_valor,0) FROM dbo.ACA_ParametroAcademico WITH(NOLOCK) WHERE pac_chave = 'CONTROLAR_ORDEM_DISCIPLINAS'
	
	DECLARE @dataAtual DATE
	SET @dataAtual = GETDATE()

	DECLARE @tev_idEfetivacaoNotas INT = 
	(
		SELECT CAST(Pac.pac_valor AS INT)
		FROM ACA_ParametroAcademico Pac WITH(NOLOCK)
		WHERE
			Pac.pac_situacao <> 3
			AND CAST(GETDATE() AS DATE) BETWEEN Pac.pac_vigenciaInicio AND 
				ISNULL(Pac.pac_vigenciaFim, CAST(GETDATE() AS DATE))
			AND Pac.pac_chave = 'TIPO_EVENTO_EFETIVACAO_NOTAS'
	);
	
	WITH DisciplinasCoordenador_cte (tud_id, tur_id,permissao,TemPrincipal) AS (
		SELECT
			RelTud.tud_id
			, Tur.tur_id
			, 0 -- Sem permissão.
			, CASE WHEN 
				EXISTS(
					SELECT TudAux.tud_id 
					FROM TUR_TurmaDisciplina TudAux WITH(NOLOCK)
					INNER JOIN TUR_TurmaRelTurmaDisciplina RelTurAux WITH(NOLOCK)
						ON TudAux.tud_id = RelTurAux.tud_id
							AND RelTurAux.tur_id = Tur.tur_id
					WHERE 
						TudAux.tud_tipo = 5
					) THEN 1 ELSE 0 END AS TemPrincipal
		FROM
			ACA_CoordenadorDisciplina cdd WITH (NOLOCK)
		INNER JOIN ACA_Disciplina dis WITH (NOLOCK)
			ON dis.tds_id = cdd.tds_id
			AND dis.dis_situacao <> 3
		INNER JOIN TUR_TurmaDisciplinaRelDisciplina AS RelDis WITH(NOLOCK)
			ON (RelDis.dis_id = dis.dis_id)
		INNER JOIN TUR_TurmaRelTurmaDisciplina AS RelTud WITH(NOLOCK)
			ON (RelTud.tud_id = RelDis.tud_id)
		INNER JOIN TUR_Turma tur WITH (NOLOCK)
			ON  tur.tur_id = RelTud.tur_id
			AND tur.esc_id = cdd.esc_id
			-- Só ativas, dependendo do parametro
			AND ((@filtroTurmasAtivas = 1 AND tur_situacao = 1) OR (@filtroTurmasAtivas = 0 AND tur_situacao <> 3))
		INNER JOIN ACA_FormatoAvaliacao fav WITH (NOLOCK)
			ON fav.fav_id = tur.fav_id
			AND fav_situacao <> 3
		WHERE
			cdd.doc_id = @doc_id
			AND cdd.cdd_situacao = 1	
			-- só considera coordenador especialista se o planejamento e
			-- o lançamento de notas e frequência não for em conjunto
			AND (tur.tur_docenteEspecialista = 0 OR fav.fav_planejamentoAulasNotasConjunto = 0)		

		UNION
		
		SELECT 
		Tdt.tud_id
		, RelTur.tur_id
		, 1 -- Com permissão.
		, CASE WHEN 
			EXISTS(
				SELECT TudAux.tud_id 
				FROM TUR_TurmaDisciplina TudAux WITH(NOLOCK)
				INNER JOIN TUR_TurmaRelTurmaDisciplina RelTurAux WITH(NOLOCK)
					ON TudAux.tud_id = RelTurAux.tud_id
						AND RelTurAux.tur_id = RelTur.tur_id
				WHERE 
					TudAux.tud_tipo = 5
				) THEN 1 ELSE 0 END AS TemPrincipal
		FROM TUR_TurmaDocente AS Tdt WITH(NOLOCK)
		INNER JOIN TUR_TurmaRelTurmaDisciplina AS RelTur WITH(NOLOCK)
			ON (RelTur.tud_id = Tdt.tud_id)
		WHERE
			Tdt.doc_id = @doc_id
			AND Tdt.tdt_situacao <> 3 -- Não traz somente ativos
			
	)
	, Dados AS (	
		SELECT Tud.tud_id
			, Tud.tud_tipo
			, tdc.tdc_id
			, Tur.tur_docenteEspecialista
			, Fav.fav_planejamentoAulasNotasConjunto
			, Tur.tur_codigo
			, Esc.esc_nome
			, Tur.tur_id
			, Tud.tud_nome
			, Tud.tud_disciplinaEspecial
			, tds.tds_nomeDisciplinaEspecial
			, ISNULL(Tud.tud_naoLancarPlanejamento, 0) tud_naoLancarPlanejamento	
			, tds_ordem		
		FROM TUR_TurmaDocente tdt WITH(NOLOCK)
		INNER JOIN TUR_TurmaDisciplina Tud WITH (NOLOCK)
			ON tdt.tud_id = Tud.tud_id
			AND Tud.tud_situacao <> 3
		INNER JOIN ACA_TipoDocente tdc WITH(NOLOCK)
			ON tdc.tdc_posicao = tdt.tdt_posicao
		INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDis WITH(NOLOCK)
			ON RelDis.tud_id = Tud.tud_id
		INNER JOIN ACA_Disciplina dis WITH(NOLOCK)
			ON RelDis.dis_id = dis.dis_id
			AND dis.dis_situacao <> 3
		INNER JOIN ACA_TipoDisciplina tds WITH(NOLOCK)
			ON tds.tds_id = dis.tds_id
			AND tds.tds_situacao <> 3
		INNER JOIN DisciplinasCoordenador_cte Cd
			ON Cd.tud_id = Tud.tud_id
		INNER JOIN Tur_turma AS Tur WITH(NOLOCK)
			ON Tur.tur_id = Cd.tur_id
		INNER JOIN ESC_Escola AS Esc WITH(NOLOCK)
			ON Esc.esc_id = Tur.esc_id
		INNER JOIN ACA_FormatoAvaliacao AS Fav WITH(NOLOCK)
			ON Fav.fav_id = Tur.fav_id
		INNER JOIN TUR_TurmaCurriculo tcr WITH(NOLOCK)
			ON tcr.tur_id = tur.tur_id
			AND tcr.tcr_situacao <> 3
		INNER JOIN ACA_CurriculoPeriodo crp WITH(NOLOCK)
			ON tcr.cur_id = crp.cur_id
			AND tcr.crr_id = crp.crr_id
			AND tcr.crp_id = crp.crp_id
			AND crp.crp_situacao <> 3
		WHERE
			tdt.doc_id = @doc_id
			AND tdt.tdt_situacao <> 3
			-- Só ativas, dependendo do parametro
			AND ((@filtroTurmasAtivas = 1 AND Tur.tur_situacao = 1) OR (@filtroTurmasAtivas = 0 AND Tur.tur_situacao <> 3)) 
			AND (@regencia = 0 OR 
				(@regencia = 1 AND
					((@tipoRegencia IS NULL AND Tud.tud_tipo <> 12) OR
					(@tipoRegencia = 1 AND Tud.tud_tipo <> 11))))
			-- Se está na tela de lançamento de frequência, só traz disciplinas diferentes do tipo
			-- 13-Docente específico – complementação da regência.
			AND (@fav_tipoLancamentoFrequencia IS NULL OR
					Tud.tud_tipo <> 13)
			AND (@fav_tipoLancamentoFrequencia IS NULL OR 
					-- 1-Aulas planejadas, 2-Período, 4-Aulas planejadas e mensal
					(CASE WHEN Fav.fav_tipoLancamentoFrequencia = 4 THEN 1
					 ELSE Fav.fav_tipoLancamentoFrequencia END)
					= @fav_tipoLancamentoFrequencia)
			AND (
					NOT (Tur.tur_docenteEspecialista = 1 AND Fav.fav_planejamentoAulasNotasConjunto = 1)
					AND Tud.tud_global = 0
					AND EXISTS 
					(
						SELECT tud_id 
						FROM DisciplinasCoordenador_cte AS DisciplinasDocente
						WHERE 
							DisciplinasDocente.tud_id = Tud.tud_id
							AND (@fav_tipoLancamentoFrequencia IS NULL  
							OR -- 1-Aulas planejadas, 2-Período, 4-Aulas planejadas e mensal
								(CASE WHEN fav_tipoLancamentoFrequencia = 4 THEN 1
								 ELSE fav_tipoLancamentoFrequencia END)
							= @fav_tipoLancamentoFrequencia
							OR (DisciplinasDocente.TemPrincipal = 0 
								OR Tud.tud_tipo = 5  
								OR (
									Fav.fav_tipoApuracaoFrequencia = 2
									AND crp.crp_controleTempo = 2
								)
								)
							)
					)
				)			
			AND
			(
			---- Optativa = 3, Eletiva = 4, DocenteTurmaEletiva = 7, DependeDisponibilidadeProfessorEletiva = 9, DisciplinaEletivaAluno = 10
			--tud_tipo NOT IN (3, 4, 7, 9, 10)
			--OR
			EXISTS 
			(
				-- Só traz as disciplinas eletivas, caso possua pelo menos um período do calendário cadastrado.
				SELECT Tdc.tpc_id
				FROM TUR_TurmaDisciplinaCalendario AS Tdc WITH(NOLOCK)
				WHERE
					Tdc.tud_id = Tud.tud_id
					AND Tdc.tpc_id IN 
					(
						SELECT tpc_id FROM dbo.FN_Select_Periodos_Com_EventosVigentes(@tev_idEfetivacaoNotas, Tur.tur_id)
					)
			)
			OR
			EXISTS
			(
				SELECT Cap.cap_id
				FROM ACA_CalendarioPeriodo AS Cap WITH(NOLOCK)				
				WHERE 
					Cap.cal_id = Tur.cal_id
					AND Cap.cap_situacao <> 3
			)
			)	
		GROUP BY
			Tur.tur_docenteEspecialista
			, Fav.fav_planejamentoAulasNotasConjunto
			, Tur.tur_codigo
			, Esc.esc_nome
			, Tud.tud_nome
			, Tur.tur_id
			, Tud.tud_id
			, Tds.tds_ordem
			, Tud.tud_tipo
			, tud.tud_disciplinaEspecial
			, tds.tds_nomeDisciplinaEspecial
			, tdc.tdc_id
			, Tud.tud_naoLancarPlanejamento	
	)
	, DisciplinaRelacionada AS
	(					
		SELECT tudRelacionada.tud_id
			, tudRelacionada.tud_tipo
			, tdc.tdc_id
			, Tur.tur_docenteEspecialista
			, Tur.fav_planejamentoAulasNotasConjunto
			, Tur.tur_codigo
			, Tur.esc_nome
			, Tur.tur_id
			, tudRelacionada.tud_nome		
			, tudRelacionada.tud_disciplinaEspecial
			, '' AS tds_nomeDisciplinaEspecial
			, ISNULL(tudRelacionada.tud_naoLancarPlanejamento, 0) tud_naoLancarPlanejamento
			, Tur.tds_ordem
		FROM Dados Tur
		INNER JOIN TUR_TurmaDisciplinaRelacionada tdr WITH(NOLOCK)
			ON tdr.tud_id = Tur.tud_id
			AND tdr.tdr_situacao = 1
		INNER JOIN TUR_TurmaDisciplina tudRelacionada WITH(NOLOCK)
			ON tudRelacionada.tud_id = tdr.tud_idRelacionada	
			AND tudRelacionada.tud_situacao <> 3
		INNER JOIN ACA_TipoDocente tdc WITH(NOLOCK)
			ON Tur.tdc_id = tdc.tdc_id
			AND (ISNULL(Tur.tud_naoLancarPlanejamento, 0) = 0 AND tdc.tdc_id = 2) -- Compartilhado
			OR (ISNULL(Tur.tud_naoLancarPlanejamento, 0) = 1 AND tdc.tdc_id = 3) -- Projeto	
		WHERE 
			Tur.tdc_id > 0 -- nao eh disciplina global
			AND Tur.tud_tipo = 17 -- docencia compartilhada
	)
	, DadosSemDisciplinaRelacionada AS
	(
		SELECT Tur.*
		FROM Dados Tur
		-- possui uma posicao compartilhada, mas ela nao aparece na docencia relacionada.
		-- serve para manter compatibilidade com as versoes 
		-- sem a nova regra de docencia compartilhada	
		LEFT JOIN DisciplinaRelacionada dr
			ON (Tur.tdc_id > 0 AND Tur.tud_tipo = 17)
			AND dr.tur_id = Tur.tur_id
			AND dr.tud_id = Tur.tud_id		
		WHERE 
		dr.tud_id IS NULL
	)
	, DadosTotal AS
	(
		SELECT *, 0 AS disciplinaRelacionada 
		FROM DadosSemDisciplinaRelacionada 
		UNION
		SELECT *, 1 AS disciplinaRelacionada 
		FROM DisciplinaRelacionada
	)		
	, ResultadoDesordenado AS
	( 
		SELECT 
			-- Se for lançamento em conjunto mostra só nome da turma.
			CASE WHEN (Tur.tur_docenteEspecialista = 1 AND Tur.fav_planejamentoAulasNotasConjunto = 1) THEN
				Tur.tur_codigo --+ ' (' + Tur.esc_nome + ')' Removido nome da escola devido ao bug 27834
			ELSE 
				Tur.tur_codigo + '/' + 
				CASE WHEN Tur.tud_disciplinaEspecial = 1 AND Tur.tdc_id = 5
					THEN ISNULL(Tur.tds_nomeDisciplinaEspecial, Tur.tud_nome)
					ELSE Tur.tud_nome
				END --+ ' (' + Tur.esc_nome + ')'  Removido nome da escola devido ao bug 27834
			END AS tur_tud_nome
			, CAST(Tur.tur_id AS VARCHAR(20)) + ';' + CAST(Tur.tud_id AS VARCHAR(20)) +
				';' + (CASE WHEN Tur.disciplinaRelacionada = 1 THEN '1' ELSE 
					CAST(-- Se ele dá aula na disciplina.
					(SELECT MAX(Aux.permissao)
					FROM DisciplinasCoordenador_cte Aux 
					WHERE 
						Aux.tud_id = Tur.tud_id) AS VARCHAR(1)) END) +
				';' + 
					CAST(Tur.tud_tipo AS VARCHAR(20))
			AS tur_tud_id
			, Tur.tud_nome
			, Tur.tds_ordem
			, Tur.tur_codigo
		FROM 
		DadosTotal Tur
			
		UNION 
		
			SELECT
					-- Se for lançamento em conjunto mostra só nome da turma.
				CASE WHEN (Tur.tur_docenteEspecialista = 1 AND Fav.fav_planejamentoAulasNotasConjunto = 1) THEN
					Tur.tur_codigo --+ ' (' + Esc.esc_nome + ')' Removido nome da escola devido ao bug 27834
				ELSE 
					Tur.tur_codigo + '/' + Tud.tud_nome --+ ' (' + Esc.esc_nome + ')' Removido nome da escola devido ao bug 27834
				END AS tur_tud_nome
				, CAST(Tur.tur_id AS VARCHAR(20)) + ';' + CAST(Tud.tud_id AS VARCHAR(20)) +
					';' + 
						CAST(-- Se ele dá aula na disciplina.
						(SELECT MAX(Aux.permissao)
						FROM DisciplinasCoordenador_cte Aux 
						WHERE 
							Aux.tur_id = Tur.tur_id) AS VARCHAR(1)) +
					';' + 
						CAST(Tud.tud_tipo AS VARCHAR(20))
				AS tur_tud_id
				, Tud.tud_nome
				, Tds.tds_ordem
				, Tur.tur_codigo
			FROM TUR_TurmaDisciplina AS Tud WITH(NOLOCK)
			INNER JOIN TUR_TurmaRelTurmaDisciplina AS RelTud WITH(NOLOCK)
				ON (RelTud.tud_id = Tud.tud_id)
			INNER JOIN Tur_turma AS Tur WITH(NOLOCK)
				ON (Tur.tur_id = RelTud.tur_id)
			INNER JOIN ESC_Escola AS Esc WITH(NOLOCK)
				ON (Esc.esc_id = Tur.esc_id)
			INNER JOIN ACA_FormatoAvaliacao AS Fav WITH(NOLOCK)
				ON (Fav.fav_id = Tur.fav_id)
			INNER JOIN TUR_TurmaCurriculo tcr WITH(NOLOCK)
				ON tcr.tur_id = tur.tur_id
				AND tcr.tcr_situacao <> 3
			INNER JOIN ACA_CurriculoPeriodo crp WITH(NOLOCK)
				ON tcr.cur_id = crp.cur_id
				AND tcr.crr_id = crp.crr_id
				AND tcr.crp_id = crp.crp_id
				AND crp.crp_situacao <> 3
			--
			LEFT JOIN TUR_TurmaDisciplinaRelDisciplina Tdd WITH(NOLOCK)
				ON Tdd.tud_id = Tud.tud_id
			LEFT JOIN ACA_Disciplina Dis WITH(NOLOCK)
				ON Dis.dis_id = Tdd.dis_id
				AND Dis.dis_situacao <> 3
			LEFT JOIN ACA_TipoDisciplina Tds WITH(NOLOCK)
				ON Tds.tds_id = Dis.tds_id
				AND Tds.tds_situacao <> 3
			--		
			WHERE
				Tud.tud_situacao <> 3
				-- Só ativas, dependendo do parametro
				AND ((@filtroTurmasAtivas = 1 AND Tur.tur_situacao = 1) OR (@filtroTurmasAtivas = 0 AND Tur.tur_situacao <> 3))
				AND (@fav_tipoLancamentoFrequencia IS NULL OR 
						-- 1-Aulas planejadas, 2-Período, 4-Aulas planejadas e mensal
						(CASE WHEN Fav.fav_tipoLancamentoFrequencia = 4 THEN 1
						 ELSE Fav.fav_tipoLancamentoFrequencia END)
						= @fav_tipoLancamentoFrequencia)
				AND (
						(Tur.tur_docenteEspecialista = 1 AND Fav.fav_planejamentoAulasNotasConjunto = 1)
						AND (Tud.tud_global = 1) 
						AND EXISTS
						(
							SELECT tud_id FROM DisciplinasCoordenador_cte AS TurmasDocente
							WHERE TurmasDocente.tur_id = Tur.tur_id
							AND (@fav_tipoLancamentoFrequencia IS NULL 
								OR 
								-- 1-Aulas planejadas, 2-Período, 4-Aulas planejadas e mensal
								(CASE WHEN fav_tipoLancamentoFrequencia = 4 THEN 1
								 ELSE fav_tipoLancamentoFrequencia END) = @fav_tipoLancamentoFrequencia
								OR (TurmasDocente.TemPrincipal = 0 
									OR Tud.tud_tipo = 5  
									OR (
										Fav.fav_tipoApuracaoFrequencia = 2
										AND crp.crp_controleTempo = 2
									)
									)		
								)
						)
					)
				AND
				(
					---- Optativa = 3, Eletiva = 4, DocenteTurmaEletiva = 7, DependeDisponibilidadeProfessorEletiva = 9, DisciplinaEletivaAluno = 10
					--tud_tipo NOT IN (3, 4, 7, 9, 10)
					--OR
					EXISTS 
					(
						-- Só traz as disciplinas eletivas, caso possua pelo menos um período do calendário cadastrado.
						SELECT Tdc.tpc_id
						FROM TUR_TurmaDisciplinaCalendario AS Tdc WITH(NOLOCK)
						WHERE
							Tdc.tud_id = Tud.tud_id
							AND Tdc.tpc_id IN 
							(
								SELECT tpc_id FROM dbo.FN_Select_Periodos_Com_EventosVigentes(@tev_idEfetivacaoNotas, Tur.tur_id)
							)
					)
					OR
					EXISTS
					(
						SELECT Cap.cap_id
						FROM ACA_CalendarioPeriodo AS Cap WITH(NOLOCK)				
						WHERE 
							Cap.cal_id = Tur.cal_id
							AND Cap.cap_situacao <> 3
					)	
				)
		GROUP BY
			Tur.tur_docenteEspecialista
			, Fav.fav_planejamentoAulasNotasConjunto
			, Tur.tur_codigo
			, Esc.esc_nome
			, Tud.tud_nome
			, Tur.tur_id
			, Tud.tud_id
			, Tds.tds_ordem
			, Tud.tud_tipo 
			, tud.tud_disciplinaEspecial 
	)
	SELECT
		tur_tud_nome
		, tur_tud_id
		, tud_nome
	FROM ResultadoDesordenado		
	GROUP BY 	
		tur_tud_nome
		, tur_tud_id
		, tud_nome
		, tur_codigo
		, tds_ordem
	ORDER BY
		tur_codigo 		
		, CASE WHEN @controlarOrdem = 1 THEN tds_ordem END
		, CASE WHEN @controlarOrdem = 0 THEN tud_nome END 		
END
GO

PRINT N'Creating [dbo].[STP_TUR_TurmaDisciplinaRelacionada_UPDATE]'
GO


CREATE PROCEDURE [dbo].[STP_TUR_TurmaDisciplinaRelacionada_UPDATE]
	@tdr_id BIGINT
	, @tud_id BIGINT
	, @tud_idRelacionada BIGINT
	, @tdr_vigenciaInicio DATETIME
	, @tdr_vigenciaFim DATETIME
	, @tdr_situacao TINYINT
	, @tdr_dataCriacao DATETIME
	, @tdr_dataAlteracao DATETIME

AS
BEGIN
	UPDATE TUR_TurmaDisciplinaRelacionada 
	SET 
		tdr_vigenciaInicio = @tdr_vigenciaInicio 
		, tdr_vigenciaFim = @tdr_vigenciaFim 
		, tdr_situacao = @tdr_situacao 
		, tdr_dataCriacao = @tdr_dataCriacao 
		, tdr_dataAlteracao = @tdr_dataAlteracao 

	WHERE 
		tdr_id = @tdr_id 
		AND tud_id = @tud_id 
		AND tud_idRelacionada = @tud_idRelacionada 

		
	DECLARE @ret INT
	SELECT @ret = ISNULL(@@ROWCOUNT,-1)
	RETURN @ret
	
END


GO

PRINT N'Creating [dbo].[TUR_TurmaDisciplinaAulaSugestao]'
GO
CREATE TABLE [dbo].[TUR_TurmaDisciplinaAulaSugestao]
(
[tud_id] [bigint] NOT NULL,
[tpc_id] [int] NOT NULL,
[tas_aulasSugestao] [int] NULL,
[tas_dataCriacao] [datetime] NOT NULL CONSTRAINT [DF_TUR_TurmaDisciplinaAulaSugestao_tas_dataCriacao] DEFAULT (getdate()),
[tas_dataAlteracao] [datetime] NOT NULL CONSTRAINT [DF_TUR_TurmaDisciplinaAulaSugestao_tas_dataAlteracao] DEFAULT (getdate())
)
GO

PRINT N'Creating primary key [PK_TUR_TurmaDisciplinaAulaSugestao] on [dbo].[TUR_TurmaDisciplinaAulaSugestao]'
GO
ALTER TABLE [dbo].[TUR_TurmaDisciplinaAulaSugestao] ADD CONSTRAINT [PK_TUR_TurmaDisciplinaAulaSugestao] PRIMARY KEY CLUSTERED  ([tud_id], [tpc_id])
GO

PRINT N'Altering [dbo].[NEW_ACA_CalendarioPeriodo_Seleciona_QtdeAulas_TurmaDiscplina]'
GO

-- =============================================
-- Author:		Carla Frascareli
-- Create date: 28/01/2014
-- Description:	Retorna os períodos do calendário, com as quantidades de aulas
--				lançadas na disciplina.
-- =============================================
-- =============================================
-- Author:		Daniel Jun Suguimoto
-- Alter date:  28/01/2015
-- Description:	Correção ao contabiliar as aulas quando for regência e
--				e possuir mais de um docente titular.

---- Alterado: Marcia Haga - 09/04/2015
---- Description: Alterado para retornar apenas os periodos que o docente esteve ativo na turma.

-- Alterado: Jean Michel - 04/08/2016
-- Description: Incluído método para calcular quantidade de aulas da Experiência (Território do Saber)

---- Alterado: Marcia Haga - 14/03/2017
---- Description: Alterado para considerar a sugestão para aulas previstas.
-- =============================================
ALTER PROCEDURE [dbo].[NEW_ACA_CalendarioPeriodo_Seleciona_QtdeAulas_TurmaDiscplina]
	@tud_id BIGINT
	, @tur_id BIGINT
	, @cal_id INT
	, @tdt_posicao TINYINT = NULL
	, @doc_id BIGINT = 0

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @dataAtual DATE = GETDATE()
			, @tud_tipo TINYINT
			, @ttn_tipo TINYINT
			, @fav_fechamentoAutomatico BIT;
	
	SELECT
		@tud_tipo = tud_tipo
	FROM
		TUR_TurmaDisciplina tud WITH(NOLOCK)
	WHERE
		tud.tud_id = @tud_id
		AND tud.tud_situacao <> 3

	SELECT 
		@ttn_tipo = Ttn.ttn_tipo
		, @fav_fechamentoAutomatico = Fav.fav_fechamentoAutomatico
	FROM 
		TUR_Turma AS Tur WITH(NOLOCK)
		INNER JOIN ACA_FormatoAvaliacao AS Fav WITH(NOLOCK)
			ON Fav.fav_id = Tur.fav_id
			AND Fav.fav_situacao <> 3
		INNER JOIN ACA_Turno AS Trn WITH(NOLOCK)
			ON Tur.trn_id = Trn.trn_id 
			AND Trn.trn_situacao <> 3
		INNER JOIN ACA_TipoTurno AS Ttn WITH(NOLOCK)
			ON Trn.ttn_id = Ttn.ttn_id
			AND Ttn.ttn_situacao <> 3
	WHERE
		TUR.tur_id = @tur_id

	IF (ISNULL(@tud_tipo, 0) = 11 AND ISNULL(@ttn_tipo, 0) = 4) --Integral
	BEGIN
		;WITH PermissaoDocenteConsulta AS 
		(
			SELECT 
				tdcPermissao.tdc_id,
				tdcPermissao.tdc_posicao AS tdt_posicaoPermissao
			FROM
				ACA_TipoDocente tdc WITH(NOLOCK)
				INNER JOIN CFG_PermissaoDocente pdc WITH(NOLOCK)
					ON tdc.tdc_id = pdc.tdc_id
					AND pdc.pdc_modulo = 10
					AND pdc.pdc_permissaoConsulta = 1
					AND pdc.pdc_situacao <> 3 
				INNER JOIN ACA_TipoDocente tdcPermissao WITH(NOLOCK)
					ON tdcPermissao.tdc_id = pdc.tdc_idPermissao
					AND tdcPermissao.tdc_situacao <> 3
			WHERE
				tdc.tdc_posicao = @tdt_posicao
				AND tdc.tdc_situacao <> 3
		)		
		, TodasAulas AS 
		(
			SELECT
				tau.tud_id,
				tau.tau_id,
				tau.tau_numeroAulas,
				tau.tpc_id,
				tau.tau_reposicao,
				tau.tau_data
			FROM CLS_TurmaAula tau WITH(NOLOCK)
			INNER JOIN PermissaoDocenteConsulta pdc
				ON tau.tdt_posicao = pdc.tdt_posicaoPermissao
				AND pdc.tdc_id IN (1,4,6)
			WHERE
				tau.tud_id = @tud_id
				AND tau.tau_situacao <> 3
		)
		, Aulas AS (
			SELECT
				tau.tud_id,
				tau.tpc_id,
				tau.tau_reposicao,
				tau.tau_data,
				CASE 
					WHEN (SUM(tau.tau_numeroAulas) = 0) THEN 0
					WHEN (SUM(tau.tau_numeroAulas) = 1 OR SUM(tau.tau_numeroAulas) = 2) THEN 1 
					ELSE 2
				END AS tau_numeroAulas
			FROM 
				TodasAulas AS Tau WITH(NOLOCK)
			GROUP BY
				tau.tud_id,
				tau.tpc_id,
				tau.tau_reposicao,
				tau.tau_data
		)

		SELECT
			Tap.tap_aulasPrevitas AS aulasPrevistas
			, cap.tpc_id
			, ISNULL(
			(
				SELECT SUM(tau.tau_numeroAulas) 
				FROM Aulas tau 
				WHERE tau.tud_id = @tud_id
				AND tau.tpc_id = cap.tpc_id
				AND tau.tau_reposicao = 0
				AND tau.tau_data <= @dataAtual
			), 0) AS aulasDadas
			, ISNULL(
			(
				SELECT SUM(tau.tau_numeroAulas) 
				FROM Aulas tau 
				WHERE tau.tud_id = @tud_id
				AND tau.tpc_id = cap.tpc_id
				AND tau.tau_reposicao = 1
				AND tau.tau_data <= @dataAtual
			), 0) AS aulasRepostas
			, CONVERT(VARCHAR(10), Cap.cap_dataInicio, 103) + ' a ' +
				CONVERT(VARCHAR(10), Cap.cap_dataFim, 103) AS periodo
			, Cap.cap_descricao
			, @tud_id AS tud_id
			, @fav_fechamentoAutomatico  AS fav_fechamentoAutomatico
			, cap.cap_dataInicio
			, cap.cap_dataFim
			, @tud_tipo As tud_tipo
			, ISNULL(Tas.tas_aulasSugestao, 0) AS aulasSugestao
		FROM ACA_CalendarioPeriodo Cap WITH(NOLOCK)	
		INNER JOIN ACA_TipoPeriodoCalendario Tpc WITH(NOLOCK)
			ON Tpc.tpc_id = Cap.tpc_id
		LEFT JOIN TUR_TurmaDisciplinaAulaPrevista Tap WITH(NOLOCK)
			ON Tap.tud_id = @tud_id
			AND Tap.tpc_id = Cap.tpc_id
			AND Tap.tap_situacao <> 3
		LEFT JOIN TUR_TurmaDisciplinaAulaSugestao Tas WITH(NOLOCK)
			ON Tas.tud_id = @tud_id
			AND Tas.tpc_id = Cap.tpc_id
		WHERE
			Cap.cal_id = @cal_id
			AND Cap.cap_situacao <> 3
			
			-- Traz somente os períodos a partir de quando a turma iniciou (períodos que possuem alunos matriculados)
			AND (EXISTS (SELECT TOP 1 alu_id
							FROM MTR_MatriculaTurmaDisciplina WITH(NOLOCK)
							WHERE 
								mtd_situacao <> 3
								AND tud_id = @tud_id
								AND mtd_dataMatricula <= Cap.cap_dataFim
								AND (mtd_dataSaida IS NULL OR mtd_dataSaida >= Cap.cap_dataInicio))
				)
				
			-- Não exibir os bimestre(s) que a turma não esteve ativa
			AND EXISTS (SELECT Tur.tur_id 
							FROM TUR_Turma Tur WITH(NOLOCK)
							WHERE 
								Tur.tur_id = @tur_id
								AND (Tur.tur_dataEncerramento IS NULL OR Tur.tur_dataEncerramento >= Cap.cap_dataFim)
							)

			-- Não exibir os bimestre(s) que o docente nao esteve ativo na turma
			AND (@doc_id <= 0 OR EXISTS (SELECT TOP 1 Tdt.tud_id 
							FROM TUR_TurmaDocente Tdt WITH(NOLOCK)
							WHERE 
								Tdt.tud_id = @tud_id
								AND Tdt.doc_id = @doc_id
								AND 
								(
									Tdt.tdt_situacao = 1
									OR
									(
										Tdt.tdt_situacao <> 3
										AND
										(
											Tdt.tdt_vigenciaFim IS NULL OR Tdt.tdt_vigenciaFim >= Cap.cap_dataFim
										)
									)
								)
							))
			
		ORDER BY Tpc.tpc_ordem
	END
	ELSE IF (ISNULL(@tud_tipo,0) = 18) -- Experiência (Território Saber)
	BEGIN
		;WITH PermissaoDocenteConsulta AS 
		(
			SELECT 
				tdcPermissao.tdc_posicao AS tdt_posicaoPermissao
			FROM
				ACA_TipoDocente tdc WITH(NOLOCK)
				INNER JOIN CFG_PermissaoDocente pdc WITH(NOLOCK)
					ON tdc.tdc_id = pdc.tdc_id
					AND pdc.pdc_modulo = 10
					AND pdc.pdc_permissaoConsulta = 1
					AND pdc.pdc_situacao <> 3 
				INNER JOIN ACA_TipoDocente tdcPermissao WITH(NOLOCK)
					ON tdcPermissao.tdc_id = pdc.tdc_idPermissao
					AND tdcPermissao.tdc_situacao <> 3
			WHERE
				tdc.tdc_posicao = @tdt_posicao
				AND tdc.tdc_situacao <> 3
		)
				
		, Aulas AS 
		(
			SELECT
				tau.tud_id,
				tau.tau_id,
				tau.tau_numeroAulas,
				tau.tpc_id,
				tau.tau_reposicao,
				tau.tau_data
			FROM CLS_TurmaAula tau WITH(NOLOCK)
			INNER JOIN PermissaoDocenteConsulta pdc
				ON tau.tdt_posicao = pdc.tdt_posicaoPermissao
			WHERE
				tau.tud_id = @tud_id
				AND tau.tau_situacao <> 3
		)

		-- Verifica qual o período do território dentro do período de cada período do calendário
		, AulasExperienciaPeriodo AS
		(
			SELECT
				Tau.tud_id
				, Tau.tpc_id
				, Tau.tau_reposicao
				, Tau.tau_data
				
				-- Recupera a maior data entre a data inicial do período do calendário e a data inicial da experiência de cada período do calendário
				, CASE WHEN tte.tte_vigenciaInicio < cap.cap_dataInicio
					THEN cap.cap_dataInicio
					ELSE tte.tte_vigenciaInicio
					END AS VigenciaInicial

				-- Recupera a menor data entre a data final do período do calendário e a data final da experiência de cada período do calendário
				, CASE WHEN tte.tte_vigenciaFim > cap.cap_dataFim
					THEN cap.cap_dataFim
					ELSE tte.tte_vigenciaFim
					END AS VigenciaFinal

				, tau.tau_numeroAulas
			FROM Aulas tau			
			INNER JOIN ACA_CalendarioPeriodo cap WITH (NOLOCK)
				ON cap.cal_id = @cal_id
				AND cap.tpc_id = cap.tpc_id
			INNER JOIN  TUR_TurmaDisciplinaTerritorio tte WITH (NOLOCK)
				ON tau.tud_id = tte.tud_idExperiencia
				AND tte.tte_situacao <> 3
			INNER JOIN CLS_TurmaAulaTerritorio AS tat WITH (NOLOCK)
				ON tat.tud_idExperiencia = tte.tud_idExperiencia
				AND tat.tud_idTerritorio = tte.tud_idTerritorio			
				AND tat.tau_idExperiencia = tau.tau_id
			WHERE			
				-- Apenas experiências ativas em cada período do calendário			
				( 
					tte.tte_vigenciaInicio BETWEEN cap.cap_dataInicio AND cap.cap_dataFim
					OR tte.tte_vigenciaFim BETWEEN cap.cap_dataInicio AND cap.cap_dataFim
					OR cap.cap_dataInicio BETWEEN tte.tte_vigenciaInicio AND tte.tte_vigenciaFim
					OR cap.cap_dataFim BETWEEN tte.tte_vigenciaInicio AND tte.tte_vigenciaFim
				)			
		)	

		, AulasExperiencia AS (
			SELECT
				Tau.tud_id,				
				Tau.tpc_id,
				Tau.tau_reposicao,
				Tau.tau_data,
				COUNT(Tau.tau_numeroAulas) AS tau_numeroAulas
			FROM 
				AulasExperienciaPeriodo AS Tau WITH(NOLOCK)
			WHERE
				tau.tau_data BETWEEN Tau.VigenciaInicial and Tau.VigenciaFinal
			GROUP BY
				tau.tud_id,				
				tau.tpc_id,
				tau.tau_reposicao,
				tau.tau_data
		)
		
		SELECT
			Tap.tap_aulasPrevitas AS aulasPrevistas
			, cap.tpc_id

			, ISNULL(
			(
				SELECT SUM(tau.tau_numeroAulas) 
				FROM AulasExperiencia tau 
				WHERE tau.tud_id = @tud_id
				AND tau.tpc_id = cap.tpc_id
				AND tau.tau_reposicao = 0
				AND tau.tau_data <= @dataAtual
			), 0) AS aulasDadas

			, ISNULL(
			(
				SELECT SUM(tau.tau_numeroAulas) 
				FROM AulasExperiencia tau 
				WHERE tau.tud_id = @tud_id
				AND tau.tpc_id = cap.tpc_id
				AND tau.tau_reposicao = 1
				AND tau.tau_data <= @dataAtual
			), 0) AS aulasRepostas

			, CONVERT(VARCHAR(10), Cap.cap_dataInicio, 103) + ' a ' +
				CONVERT(VARCHAR(10), Cap.cap_dataFim, 103) AS periodo

			, Cap.cap_descricao
			, @tud_id AS tud_id
			, @fav_fechamentoAutomatico AS fav_fechamentoAutomatico
			, cap.cap_dataInicio
			, cap.cap_dataFim
			, @tud_tipo AS tud_tipo
			, ISNULL(Tas.tas_aulasSugestao, 0) AS aulasSugestao
		FROM ACA_CalendarioPeriodo Cap WITH(NOLOCK)	
		INNER JOIN ACA_TipoPeriodoCalendario Tpc WITH(NOLOCK)
			ON Tpc.tpc_id = Cap.tpc_id
		LEFT JOIN TUR_TurmaDisciplinaAulaPrevista Tap WITH(NOLOCK)
			ON Tap.tud_id = @tud_id
			AND Tap.tpc_id = Cap.tpc_id
			AND Tap.tap_situacao <> 3
		LEFT JOIN TUR_TurmaDisciplinaAulaSugestao Tas WITH(NOLOCK)
			ON Tas.tud_id = @tud_id
			AND Tas.tpc_id = Cap.tpc_id
		WHERE
			Cap.cal_id = @cal_id
			AND Cap.cap_situacao <> 3
			
			-- Traz somente os períodos a partir de quando a turma iniciou (períodos que possuem alunos matriculados)
			AND (EXISTS (SELECT TOP 1 alu_id
							FROM MTR_MatriculaTurmaDisciplina WITH(NOLOCK)
							WHERE 
								mtd_situacao <> 3
								AND tud_id = @tud_id
								AND mtd_dataMatricula <= Cap.cap_dataFim
								AND (mtd_dataSaida IS NULL OR mtd_dataSaida >= Cap.cap_dataInicio))
				)
				
			-- Não exibir os bimestre(s) que a turma não esteve ativa
			AND EXISTS (SELECT Tur.tur_id 
							FROM TUR_Turma Tur WITH(NOLOCK)
							WHERE 
								Tur.tur_id = @tur_id
								AND (Tur.tur_dataEncerramento IS NULL OR Tur.tur_dataEncerramento >= Cap.cap_dataFim)
							)

			-- Não exibir os bimestre(s) que o docente nao esteve ativo na turma
			AND (@doc_id <= 0 OR EXISTS (SELECT TOP 1 Tdt.tud_id 
							FROM TUR_TurmaDocente Tdt WITH(NOLOCK)
							WHERE 
								Tdt.tud_id = @tud_id
								AND Tdt.doc_id = @doc_id
								AND 
								(
									Tdt.tdt_situacao = 1
									OR
									(
										Tdt.tdt_situacao <> 3
										AND
										(
											Tdt.tdt_vigenciaFim IS NULL OR Tdt.tdt_vigenciaFim >= Cap.cap_dataFim
										)
									)
								)
							))
			
		ORDER BY Tpc.tpc_ordem
    END
	ELSE 
	BEGIN
		;WITH PermissaoDocenteConsulta AS 
		(
			SELECT 
				tdcPermissao.tdc_posicao AS tdt_posicaoPermissao
			FROM
				ACA_TipoDocente tdc WITH(NOLOCK)
				INNER JOIN CFG_PermissaoDocente pdc WITH(NOLOCK)
					ON tdc.tdc_id = pdc.tdc_id
					AND pdc.pdc_modulo = 10
					AND pdc.pdc_permissaoConsulta = 1
					AND pdc.pdc_situacao <> 3 
				INNER JOIN ACA_TipoDocente tdcPermissao WITH(NOLOCK)
					ON tdcPermissao.tdc_id = pdc.tdc_idPermissao
					AND tdcPermissao.tdc_situacao <> 3
			WHERE
				tdc.tdc_posicao = @tdt_posicao
				AND tdc.tdc_situacao <> 3
		)
		
		, Aulas AS 
		(
			SELECT
				tau.tud_id,
				tau.tau_id,
				tau.tau_numeroAulas,
				tau.tpc_id,
				tau.tau_reposicao,
				tau.tau_data
			FROM CLS_TurmaAula tau WITH(NOLOCK)
			INNER JOIN PermissaoDocenteConsulta pdc
				ON tau.tdt_posicao = pdc.tdt_posicaoPermissao
			WHERE
				tau.tud_id = @tud_id
				AND tau.tau_situacao <> 3
		)
		
		SELECT
			Tap.tap_aulasPrevitas AS aulasPrevistas
			, cap.tpc_id
			, ISNULL(
			(
				SELECT SUM(tau.tau_numeroAulas) 
				FROM Aulas tau 
				WHERE tau.tud_id = @tud_id
				AND tau.tpc_id = cap.tpc_id
				AND tau.tau_reposicao = 0
				AND tau.tau_data <= @dataAtual
			), 0) AS aulasDadas
			, ISNULL(
			(
				SELECT SUM(tau.tau_numeroAulas) 
				FROM Aulas tau 
				WHERE tau.tud_id = @tud_id
				AND tau.tpc_id = cap.tpc_id
				AND tau.tau_reposicao = 1
				AND tau.tau_data <= @dataAtual
			), 0) AS aulasRepostas
			, CONVERT(VARCHAR(10), Cap.cap_dataInicio, 103) + ' a ' +
				CONVERT(VARCHAR(10), Cap.cap_dataFim, 103) AS periodo
			, Cap.cap_descricao
			, @tud_id AS tud_id
			, @fav_fechamentoAutomatico AS fav_fechamentoAutomatico
			, cap.cap_dataInicio
			, cap.cap_dataFim
			, @tud_tipo AS tud_tipo
			, ISNULL(Tas.tas_aulasSugestao, 0) AS aulasSugestao
		FROM ACA_CalendarioPeriodo Cap WITH(NOLOCK)	
		INNER JOIN ACA_TipoPeriodoCalendario Tpc WITH(NOLOCK)
			ON Tpc.tpc_id = Cap.tpc_id
		LEFT JOIN TUR_TurmaDisciplinaAulaPrevista Tap WITH(NOLOCK)
			ON Tap.tud_id = @tud_id
			AND Tap.tpc_id = Cap.tpc_id
			AND Tap.tap_situacao <> 3
		LEFT JOIN TUR_TurmaDisciplinaAulaSugestao Tas WITH(NOLOCK)
			ON Tas.tud_id = @tud_id
			AND Tas.tpc_id = Cap.tpc_id
		WHERE
			Cap.cal_id = @cal_id
			AND Cap.cap_situacao <> 3
			
			-- Traz somente os períodos a partir de quando a turma iniciou (períodos que possuem alunos matriculados)
			AND (EXISTS (SELECT TOP 1 alu_id
							FROM MTR_MatriculaTurmaDisciplina WITH(NOLOCK)
							WHERE 
								mtd_situacao <> 3
								AND tud_id = @tud_id
								AND mtd_dataMatricula <= Cap.cap_dataFim
								AND (mtd_dataSaida IS NULL OR mtd_dataSaida >= Cap.cap_dataInicio))
				)
				
			-- Não exibir os bimestre(s) que a turma não esteve ativa
			AND EXISTS (SELECT Tur.tur_id 
							FROM TUR_Turma Tur WITH(NOLOCK)
							WHERE 
								Tur.tur_id = @tur_id
								AND (Tur.tur_dataEncerramento IS NULL OR Tur.tur_dataEncerramento >= Cap.cap_dataFim)
							)

			-- Não exibir os bimestre(s) que o docente nao esteve ativo na turma
			AND (@doc_id <= 0 OR EXISTS (SELECT TOP 1 Tdt.tud_id 
							FROM TUR_TurmaDocente Tdt WITH(NOLOCK)
							WHERE 
								Tdt.tud_id = @tud_id
								AND Tdt.doc_id = @doc_id
								AND 
								(
									Tdt.tdt_situacao = 1
									OR
									(
										Tdt.tdt_situacao <> 3
										AND
										(
											Tdt.tdt_vigenciaFim IS NULL OR Tdt.tdt_vigenciaFim >= Cap.cap_dataFim
										)
									)
								)
							))
			
		ORDER BY Tpc.tpc_ordem
    END
END

GO

PRINT N'Refreshing [dbo].[VW_DadosAlunos]'
GO
EXEC sp_refreshview N'[dbo].[VW_DadosAlunos]'
GO

PRINT N'Altering [dbo].[STP_REL_TurmaDisciplinaSituacaoFechamento_SELECT]'
GO

ALTER PROCEDURE [dbo].[STP_REL_TurmaDisciplinaSituacaoFechamento_SELECT]
	
AS
BEGIN
	SELECT 
		tud_id
		,esc_id
		,cal_id
		,Pendente
		,PendentePlanejamento
		,PendenteParecer
		,DataProcessamento

	FROM 
		REL_TurmaDisciplinaSituacaoFechamento WITH(NOLOCK) 
	
END

GO

PRINT N'Altering [dbo].[NEW_ACA_CalendarioAnual_SelectBy_cur_id]'
GO
-- ==============================================================================
-- Author:		Jean Michel Marques da Silva
-- Create date: 15/04/2011 13:10
-- Description:	Retorna os calendários que não foram excluídos 
--				logicamente por curso
--
-- Alteração:	Leonardo Brito 14/03/2017
--				Alterada procedure para filtrar os calendários 
--				ligados à escola ou ao docente
-- ==============================================================================
ALTER PROCEDURE [dbo].[NEW_ACA_CalendarioAnual_SelectBy_cur_id]	
	@cur_id INT	
	, @ent_id UNIQUEIDENTIFIER
	, @doc_id BIGINT
	, @usu_id UNIQUEIDENTIFIER
	, @gru_id UNIQUEIDENTIFIER
AS
BEGIN

	DECLARE @tabelaUas TABLE (uad_id UNIQUEIDENTIFIER NOT NULL)
	DECLARE @cal_ids TABLE (cal_id INT)
	
	IF (ISNULL(@doc_id, 0) > 0)
	BEGIN
		INSERT INTO @cal_ids
		SELECT tur.cal_id FROM TUR_TurmaDocente tdt WITH(NOLOCK)
		INNER JOIN TUR_TurmaRelTurmaDisciplina trt WITH(NOLOCK) ON tdt.tud_id = trt.tud_id
		INNER JOIN TUR_Turma tur WITH(NOLOCK) ON trt.tur_id = tur.tur_id AND tur.tur_situacao <> 3
		WHERE tdt.doc_id = @doc_id AND tdt.tdt_situacao <> 3
		GROUP BY tur.cal_id
	END
	ELSE IF (@usu_id IS NOT NULL AND @gru_id IS NOT NULL)
	BEGIN
		INSERT INTO @tabelaUas 
		SELECT uad_id FROM Synonym_FN_Select_UAs_By_PermissaoUsuario(@usu_id, @gru_id) GROUP BY uad_id

		INSERT INTO @cal_ids
		SELECT cac.cal_id FROM @tabelaUas t
		INNER JOIN ESC_Escola esc WITH(NOLOCK) ON t.uad_id = esc.uad_id AND esc.esc_situacao <> 3
		INNER JOIN ACA_CurriculoEscola ces WITH(NOLOCK) ON esc.esc_id = ces.esc_id AND ces.ces_situacao <> 3
		INNER JOIN ACA_CalendarioCurso cac WITH(NOLOCK) ON ces.cur_id = cac.cur_id
		GROUP BY cac.cal_id
	END

	SELECT
		cal.cal_id
		, Convert(VARCHAR,cal.cal_ano) + ' - ' + cal.cal_descricao AS cal_ano_desc
		, cal.cal_ano
	FROM
		ACA_CalendarioAnual cal WITH (NOLOCK)
	INNER JOIN ACA_CalendarioCurso cac WITH (NOLOCK)
		ON cal.cal_id = cac.cal_id
		AND cac.cur_id = @cur_id
	WHERE
		cal.cal_situacao <> 3
		AND cal.ent_id = @ent_id
		AND ((ISNULL(@doc_id, 0) = 0 AND @usu_id IS NULL AND @gru_id IS NULL) OR
			 EXISTS(SELECT c.cal_id FROM @cal_ids c WHERE cal.cal_id = c.cal_id))
	ORDER BY
		cal.cal_ano DESC
		, cal.cal_descricao DESC
		
	SELECT @@ROWCOUNT
END
GO

PRINT N'Refreshing [dbo].[VW_DadosAlunos_MovRetroativa]'
GO
EXEC sp_refreshview N'[dbo].[VW_DadosAlunos_MovRetroativa]'
GO

PRINT N'Creating [dbo].[MS_JOB_ProcessamentoSugestaoAulasPrevistas]'
GO
-- =============================================
-- Author:		Marcia Haga
-- Create date: 15/03/2017
-- Description:	Processa os dados para a sugestão das aulas previstas.
-- =============================================
CREATE PROCEDURE [dbo].[MS_JOB_ProcessamentoSugestaoAulasPrevistas]
	@todaRede BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Verifica se a StoredProcedure será executada
	DECLARE @exec BIT
			, @ser_id INT

	IF (@todaRede = 1)
	BEGIN
		SELECT 
			@exec = ISNULL(ser_ativo, 0)
		FROM SYS_Servicos WITH (NOLOCK) 
		WHERE ser_nomeProcedimento = 'MS_JOB_ProcessamentoSugestaoAulasPrevistas_TodaRede'
	
		SET @ser_id = 53
	END
	ELSE
	BEGIN
		SELECT 
			@exec = ISNULL(ser_ativo, 0)
		FROM SYS_Servicos WITH (NOLOCK) 
		WHERE ser_nomeProcedimento = 'MS_JOB_ProcessamentoSugestaoAulasPrevistas'

		SET @ser_id = 52
	END

	IF (@exec = 1)
	BEGIN
		DECLARE @ultimaExecucao DATETIME
		DECLARE @dataAtual DATETIME = GETDATE();

		SELECT @ultimaExecucao = MAX(sle_dataInicioExecucao) FROM SYS_ServicosLogExecucao WITH(NOLOCK) 
		WHERE ser_id = @ser_id AND sle_dataFimExecucao IS NOT NULL

		DECLARE @Atualizar TABLE
		(
			tud_id BIGINT
			, tpc_id INT
			, cal_id INT
			, cap_dataInicio DATE
			, cap_dataFim DATE
			, esc_id INT
		)

		DECLARE @AtualizarReduzido TABLE
		(
			tud_id BIGINT
			, tpc_id INT
			, cal_id INT
			, cap_dataInicio DATE
			, cap_dataFim DATE
			, esc_id INT
			, totalSugestao INT
			, PRIMARY KEY (tud_id, tpc_id)
		)
		
		DECLARE @EventosEscola TABLE
		(
			cal_id INT
			, esc_id INT
			, evt_dataInicio DATE
			, evt_dataFim DATE
			, tev_id INT
		)

		DECLARE @DiaNaoUtil TABLE
		(
			dnu_data DATE
			, dnu_recorrencia BIT
			, dnu_vigenciaInicio DATE
			, dnu_vigenciaFim DATE
		)
		
		DECLARE @AulasAgenda TABLE
		(
			tud_id BIGINT
			, cal_id INT
			, tpc_id INT
			, diaSemana TINYINT
			, numeroAulas INT
		)

		DECLARE @SugestaoApagar TABLE
		(
			tud_id BIGINT
			, tpc_id INT
		)

		-- Verifica se existe evento de atividade diversificada alterado após a data da última execução do serviço
		DECLARE @idAtividadeDiversificada INT
		SELECT TOP 1 @idAtividadeDiversificada = CAST(ISNULL(pac_valor, '0') AS INT) 
		FROM ACA_ParametroAcademico WITH(NOLOCK)
		WHERE pac_chave = 'TIPO_EVENTO_ATIVIDADE_DIVERSIFICADA'

		IF (@todaRede = 1)
		BEGIN
			IF (@ultimaExecucao IS NOT NULL)
			BEGIN
				-- Processa os registros afetados por eventos cadastrados para toda a rede
				;WITH Eventos AS
				(
					SELECT evt_id, evt_dataInicio, evt_dataFim
					FROM ACA_Evento WITH(NOLOCK)
					WHERE 
					evt_padrao = 1
					AND (tev_id = @idAtividadeDiversificada OR evt_semAtividadeDiscente = 1)
					AND evt_dataAlteracao > @ultimaExecucao
				)
				, PeriodosEventos AS
				(
					SELECT cap.cal_id, cap.tpc_id, cap.cap_dataInicio, cap.cap_dataFim
					FROM Eventos evt
					INNER JOIN ACA_CalendarioEvento cev WITH(NOLOCK)
						ON cev.evt_id = evt.evt_id
					INNER JOIN ACA_CalendarioPeriodo cap WITH(NOLOCK)
						ON cap.cal_id = cev.cal_id
						AND evt.evt_dataInicio <= cap.cap_dataFim
						AND evt.evt_dataFim >= cap.cap_dataInicio
						AND cap.cap_situacao <> 3
				)
				INSERT INTO @Atualizar
				SELECT tag.tud_id, tag.tpc_id, pev.cal_id, pev.cap_dataInicio, pev.cap_dataFim, tur.esc_id
				FROM CLS_TurmaAulaGerada tag WITH(NOLOCK)
				INNER JOIN TUR_TurmaDisciplina tud WITH(NOLOCK)
					ON tud.tud_id = tag.tud_id
					AND tud.tud_tipo NOT IN (17,18,19)
				INNER JOIN TUR_TurmaRelTurmaDisciplina relTud WITH(NOLOCK)
					ON relTud.tud_id = tud.tud_id
				INNER JOIN TUR_Turma tur WITH(NOLOCK)
					ON tur.tur_id = relTud.tur_id
					AND tur.tur_situacao <> 3
				INNER JOIN PeriodosEventos pev
					ON pev.cal_id = tur.cal_id
					AND pev.tpc_id = tag.tpc_id
			END
		END
		ELSE
		BEGIN
			-- Verifica se existe agenda alterada após a data da última execução do serviço
			INSERT INTO @Atualizar
			SELECT tag.tud_id, tag.tpc_id, cap.cal_id, cap.cap_dataInicio, cap.cap_dataFim, tur.esc_id
			FROM CLS_TurmaAulaGerada tag WITH(NOLOCK) 
			INNER JOIN TUR_TurmaDisciplina tud WITH(NOLOCK)
				ON tud.tud_id = tag.tud_id
				AND tud.tud_tipo NOT IN (17,18,19)
			INNER JOIN TUR_TurmaRelTurmaDisciplina relTud WITH(NOLOCK)
				ON relTud.tud_id = tud.tud_id
			INNER JOIN TUR_Turma tur WITH(NOLOCK)
				ON tur.tur_id = relTud.tur_id
				AND tur.tur_situacao <> 3
			INNER JOIN ACA_CalendarioPeriodo cap WITH(NOLOCK)
				ON cap.cal_id = tur.cal_id
				AND cap.tpc_id = tag.tpc_id
				AND cap.cap_situacao <> 3
			INNER JOIN ACA_CalendarioAnual cal WITH(NOLOCK)
				ON cal.cal_id = cap.cal_id
				AND cal.cal_situacao <> 3
			WHERE 
			(@ultimaExecucao IS NULL AND cal.cal_ano >= 2017) 
			OR (@ultimaExecucao IS NOT NULL AND tag.tag_dataAlteracao > @ultimaExecucao)

			IF (@ultimaExecucao IS NOT NULL)
			BEGIN
				-- Processa os registros afetados por eventos cadastrados para uma escola
				;WITH Eventos AS
				(
					SELECT evt_id, esc_id, uni_id, evt_dataInicio, evt_dataFim
					FROM ACA_Evento WITH(NOLOCK)
					WHERE 
					ISNULL(evt_padrao, 0) = 0
					AND (tev_id = @idAtividadeDiversificada OR evt_semAtividadeDiscente = 1)
					AND evt_dataAlteracao > @ultimaExecucao
				)
				, PeriodosEventos AS
				(
					SELECT esc_id, uni_id, cap.cal_id, cap.tpc_id, cap.cap_dataInicio, cap.cap_dataFim
					FROM Eventos evt
					INNER JOIN ACA_CalendarioEvento cev WITH(NOLOCK)
						ON cev.evt_id = evt.evt_id
					INNER JOIN ACA_CalendarioPeriodo cap WITH(NOLOCK)
						ON cap.cal_id = cev.cal_id
						AND evt.evt_dataInicio <= cap.cap_dataFim
						AND evt.evt_dataFim >= cap.cap_dataInicio
						AND cap.cap_situacao <> 3
				)
				INSERT INTO @Atualizar
				SELECT tag.tud_id, tag.tpc_id, pev.cal_id, pev.cap_dataInicio, pev.cap_dataFim, tur.esc_id
				FROM CLS_TurmaAulaGerada tag WITH(NOLOCK)
				INNER JOIN TUR_TurmaDisciplina tud WITH(NOLOCK)
					ON tud.tud_id = tag.tud_id
					AND tud.tud_tipo NOT IN (17,18,19)
				INNER JOIN TUR_TurmaRelTurmaDisciplina relTud WITH(NOLOCK)
					ON relTud.tud_id = tud.tud_id
				INNER JOIN TUR_Turma tur WITH(NOLOCK)
					ON tur.tur_id = relTud.tur_id
					AND tur.tur_situacao <> 3
				INNER JOIN PeriodosEventos pev
					ON pev.cal_id = tur.cal_id
					AND pev.tpc_id = tag.tpc_id
					AND pev.esc_id = tur.esc_id
					AND pev.uni_id = tur.uni_id
			END
		END

		IF (EXISTS (SELECT TOP 1 1 FROM @Atualizar))
		BEGIN

			INSERT INTO @AtualizarReduzido
			SELECT tud_id, tpc_id, cal_id, cap_dataInicio, cap_dataFim, esc_id, 0	
			FROM @Atualizar
			GROUP BY tud_id, tpc_id, cal_id, cap_dataInicio, cap_dataFim, esc_id

			DELETE FROM @Atualizar

			;WITH Calendarios AS
			(
				SELECT cal_id, esc_id
				FROM @AtualizarReduzido
				GROUP BY cal_id, esc_id
			)
			INSERT INTO @EventosEscola
			SELECT cal.cal_id, cal.esc_id, evt.evt_dataInicio, evt.evt_dataFim, evt.tev_id
			FROM
			Calendarios cal 
			INNER JOIN ACA_CalendarioEvento AS ce WITH(NOLOCK)
				ON ce.cal_id = cal.cal_id
			INNER JOIN ACA_Evento AS evt WITH(NOLOCK)
				ON evt.evt_id = ce.evt_id
				AND (evt.evt_padrao = 1 OR (evt.evt_padrao = 0 AND evt.esc_id = cal.esc_id))
				AND (evt.tev_id = @idAtividadeDiversificada OR evt.evt_semAtividadeDiscente = 1)
				AND evt.evt_situacao <> 3

			DECLARE @ent_id UNIQUEIDENTIFIER
			SELECT TOP 1 @ent_id = ent_id
			FROM ESC_Escola WITH(NOLOCK)

			INSERT INTO @DiaNaoUtil
			SELECT 
				Dnu.dnu_data
				, Dnu.dnu_recorrencia
				, Dnu.dnu_vigenciaInicio
				, Dnu.dnu_vigenciaFim
			FROM Synonym_SYS_DiaNaoUtil Dnu WITH(NOLOCK)
			LEFT JOIN Synonym_SYS_EntidadeEndereco Ene WITH(NOLOCK)
				ON Ene.ent_id = @ent_id
				AND Ene.ene_situacao <> 3
			LEFT JOIN Synonym_END_Endereco Ende WITH(NOLOCK)
				ON Ende.end_id = Ene.end_id
				AND Ende.end_situacao <> 3
			LEFT JOIN Synonym_END_Cidade Cid WITH(NOLOCK)
				ON Cid.cid_id = Ende.cid_id
				AND Cid.unf_id = Dnu.unf_id
				AND Cid.cid_situacao <> 3
			WHERE 
			Dnu.dnu_situacao <> 3
			AND (Dnu.unf_id IS NULL 
				OR Cid.cid_id IS NOT NULL)
			AND (ISNULL(Dnu.dnu_recorrencia, 0) = 0 
				OR (dnu_vigenciaInicio <= CAST(@dataAtual AS DATE)
					AND (dnu_vigenciaFim IS NULL OR CAST(@dataAtual AS DATE) <= dnu_vigenciaFim)))

			INSERT INTO @AulasAgenda
			SELECT atu.tud_id, atu.cal_id, atu.tpc_id, tag_diaSemana + 1, tag_numeroAulas
			FROM @AtualizarReduzido atu
			INNER JOIN CLS_TurmaAulaGerada tag WITH(NOLOCK)
				ON tag.tud_id = atu.tud_id
				AND tag.tpc_id = atu.tpc_id
				AND tag.tag_situacao <> 3

			INSERT INTO @SugestaoApagar
			SELECT tud_id, tpc_id
			FROM @AulasAgenda
			GROUP BY tud_id, tpc_id
			HAVING SUM(ISNULL(numeroAulas, 0)) = 0

			DELETE @AtualizarReduzido
			FROM @AtualizarReduzido atu
			INNER JOIN @SugestaoApagar sug
				ON sug.tud_id = atu.tud_id
				AND sug.tpc_id = atu.tpc_id
				
			-- Calcula quantidade de aulas para sugestão
			DECLARE @cap_dataFim DATE
					, @dataInicial DATE
					, @diaSemana TINYINT;

			SELECT @dataInicial = MIN(cap_dataInicio) FROM @AtualizarReduzido
			SELECT @cap_dataFim = MAX(cap_dataFim) FROM @AtualizarReduzido
				
			WHILE (@dataInicial <= @cap_dataFim)
			BEGIN
				SET @diaSemana = DATEPART(WEEKDAY, @dataInicial)

				-- Não é domingo e não é sábado
				IF (@diaSemana <> 1 AND @diaSemana <> 7)
				BEGIN
					-- Disciplinas com aula gerada nesse dia da semana
					;WITH Eventos AS
					(
						SELECT tev_id, esc_id, cal_id
						FROM @EventosEscola
						WHERE 
						evt_dataInicio <= @dataInicial 
						AND @dataInicial <= evt_dataFim
					)
					-- Escolas com evento de atividade diversificada na data
					, AtividadeDiversificadaData AS
					(
						SELECT esc_id, cal_id
						FROM Eventos
						WHERE tev_id = @idAtividadeDiversificada
						GROUP BY esc_id, cal_id
					)
					-- Escolas com evento sem atividade discente na data,
					, EventosData AS
					(
						SELECT esc_id, cal_id
						FROM Eventos
						WHERE 
						tev_id <> @idAtividadeDiversificada
						GROUP BY esc_id, cal_id
					)
					-- Retorna se data está em um dia não útil
					, DiaNaoUtilData AS
					(
						SELECT TOP 1 1 naoUtil
						FROM @DiaNaoUtil
						WHERE 
						DAY(dnu_data) = DAY(@dataInicial)
						AND MONTH(dnu_data) = MONTH(@dataInicial)
						AND (YEAR(dnu_data) = YEAR(@dataInicial) OR dnu_recorrencia = 1)
					)
					UPDATE @AtualizarReduzido
					SET totalSugestao = totalSugestao + dia.numeroAulas
					FROM @AtualizarReduzido sug
					-- Existe aula gerada
					INNER JOIN @AulasAgenda dia
						ON dia.tud_id = sug.tud_id
						AND dia.tpc_id = sug.tpc_id
						AND dia.diaSemana = @diaSemana
						AND ISNULL(numeroAulas, 0) > 0
					-- Evento de atividade diversificada na data
					LEFT JOIN AtividadeDiversificadaData atvDiv
						ON atvDiv.esc_id = sug.esc_id
						AND atvDiv.cal_id = sug.cal_id
					-- Evento sem atividade discente na data
					LEFT JOIN EventosData evt
						ON evt.esc_id = sug.esc_id
						AND evt.cal_id = sug.cal_id
					-- Data está em um dia não útil
					LEFT JOIN DiaNaoUtilData nud
						ON nud.naoUtil = nud.naoUtil
					WHERE
					-- Data está dentro do período
					sug.cap_dataInicio <= @dataInicial 
					AND @dataInicial <= sug.cap_dataFim
					AND
					(
						-- Existe evento de atividade diversificada na data
						atvDiv.esc_id IS NOT NULL
						OR
						(
							-- Não existe evento sem atividade discente na data
							evt.esc_id IS NULL
							-- Não existe dia não útil na data
							AND nud.naoUtil IS NULL
						)
					)
				END
				ELSE
				BEGIN
					-- Escolas com evento de atividade diversificada na data,
					-- sendo a data em um sábado ou domingo
					;WITH AtividadeDiversificadaData AS
					(
						SELECT esc_id, cal_id
						FROM @EventosEscola
						WHERE 
						evt_dataInicio <= @dataInicial 
						AND @dataInicial <= evt_dataFim
						AND tev_id = @idAtividadeDiversificada
						GROUP BY esc_id, cal_id
					)
					UPDATE @AtualizarReduzido
					SET totalSugestao = totalSugestao + 1
					FROM @AtualizarReduzido sug
					-- Existe evento de atividade diversificada na data
					INNER JOIN AtividadeDiversificadaData evt
						ON evt.esc_id = sug.esc_id
						AND evt.cal_id = sug.cal_id
					WHERE
					-- Data está dentro do período
					sug.cap_dataInicio <= @dataInicial 
					AND @dataInicial <= sug.cap_dataFim
				END

				SET @dataInicial = DATEADD(DAY, 1, @dataInicial)
			END
			
			MERGE TUR_TurmaDisciplinaAulaSugestao AS Destino
			USING @AtualizarReduzido AS Origem
			ON (
				Destino.tud_id = Origem.tud_id
				AND Destino.tpc_id = Origem.tpc_id
			)
			WHEN MATCHED THEN
				UPDATE SET
					Destino.tas_aulasSugestao = Origem.totalSugestao
					, Destino.tas_dataAlteracao = @dataAtual
			WHEN NOT MATCHED THEN
				INSERT (tud_id, tpc_id, tas_aulasSugestao, tas_dataCriacao, tas_dataAlteracao)
				VALUES (Origem.tud_id, Origem.tpc_id, Origem.totalSugestao, @dataAtual, @dataAtual);

			DELETE TUR_TurmaDisciplinaAulaSugestao
			FROM TUR_TurmaDisciplinaAulaSugestao atu
			INNER JOIN @SugestaoApagar sug
				ON sug.tud_id = atu.tud_id
				AND sug.tpc_id = atu.tpc_id 

			DELETE FROM @AtualizarReduzido
			DELETE FROM @EventosEscola
			DELETE FROM @DiaNaoUtil
			DELETE FROM @AulasAgenda
			DELETE FROM @SugestaoApagar
		END
	END 

END
GO

PRINT N'Creating [dbo].[NEW_ACA_ObjetoAprendizagem_SELECT_ByTipoDisciplina]'
GO


-- ========================================================================
-- Author:		Rafael Matias
-- Create date: 15/03/2017
-- Description:	Retorna todos os objetos de aprendizagem ativo do tipo de disciplina e ano passados
-- ========================================================================
CREATE PROCEDURE [dbo].[NEW_ACA_ObjetoAprendizagem_SELECT_ByTipoDisciplina]
	@tds_id INT,
	@cal_ano INT
AS
BEGIN
	SELECT
		oap_id,
		oap_descricao,
		CASE
		WHEN oap_situacao = 1 THEN 'Ativo'
		ELSE 'Inativo' END oap_situacao
	FROM ACA_ObjetoAprendizagem WITH(NOLOCK)
	WHERE tds_id = @tds_id AND cal_ano = @cal_ano
	AND oap_situacao <> 3
	ORDER BY
		oap_descricao
END


GO

PRINT N'Creating [dbo].[STP_TUR_TurmaDisciplinaRelacionada_DELETE]'
GO


CREATE PROCEDURE [dbo].[STP_TUR_TurmaDisciplinaRelacionada_DELETE]
	@tdr_id BIGINT
	, @tud_id BIGINT
	, @tud_idRelacionada BIGINT

AS
BEGIN
	DELETE FROM 
		TUR_TurmaDisciplinaRelacionada 
	WHERE 
		tdr_id = @tdr_id 
		AND tud_id = @tud_id 
		AND tud_idRelacionada = @tud_idRelacionada 

		
	DECLARE @ret INT
	SELECT @ret = ISNULL(@@ROWCOUNT,-1)
	RETURN @ret
	
END


GO

PRINT N'Altering [dbo].[NEW_REL_TurmaDisciplinaSituacaoFechamento_SelecionaPendenciasFechamentoDisciplinas]'
GO
-- =============================================
-- Author:		Daniel Jun Suguimoto
-- Create date: 02/10/2015
-- Description:	Seleciona as pendências de fechamento por disciplinas
-- =============================================
ALTER PROCEDURE [dbo].[NEW_REL_TurmaDisciplinaSituacaoFechamento_SelecionaPendenciasFechamentoDisciplinas]
	@tabelaTurmaDisciplina TipoTabela_TurmaDisciplina READONLY,
	@tev_EfetivacaoNotas INT
AS
BEGIN
	DECLARE @DataAtual DATETIME = GETDATE();

	DECLARE @tabelaPendenciaFechamento TABLE
	(
		tud_id BIGINT NOT NULL,
		tud_idRegencia BIGINT NULL,
		tud_tipo TINYINT,
		esc_id INT NOT NULL,
		ent_id UNIQUEIDENTIFIER NOT NULL,
		cal_id INT NOT NULL,
		tpc_id INT NOT NULL,
		Pendente BIT NOT NULL,
		PendentePlanejamento BIT NOT NULL,
		PendenteParecer BIT NOT NULL,
		DataProcessamento DATETIME NULL,
		PRIMARY KEY (tud_id, tpc_id)
	);

	INSERT INTO @tabelaPendenciaFechamento (tud_id, tud_idRegencia, tud_tipo, esc_id, ent_id, cal_id, tpc_id, Pendente, PendentePlanejamento, PendenteParecer, DataProcessamento)
	SELECT
		tud.tud_id,
		NULL AS tud_idRegencia,
		tud.tud_tipo,
		esc.esc_id,
		esc.ent_id,
		tur.cal_id,
		cap.tpc_id,
		CASE WHEN cap.cap_dataInicio > GETDATE() 
			 THEN 0 ELSE tdf.Pendente END AS Pendente,
		CASE WHEN cap.cap_dataInicio > GETDATE() 
			 THEN 0 ELSE tdf.PendentePlanejamento END AS PendentePlanejamento,
		CASE WHEN cap.cap_dataInicio > GETDATE() 
			 THEN 0 ELSE tdf.PendenteParecer END AS PendenteParecer,
		tdf.DataProcessamento
	FROM
		@tabelaTurmaDisciplina ttd
		INNER JOIN TUR_TurmaDisciplina tud WITH(NOLOCK)
			ON ttd.tud_id = tud.tud_id
			AND tud.tud_tipo <> 11
		INNER JOIN TUR_TurmaRelTurmaDisciplina trt WITH(NOLOCK)
			ON trt.tud_id = tud.tud_id
		INNER JOIN TUR_Turma tur WITH(NOLOCK)
			ON tur.tur_id = trt.tur_id
			AND tur.tur_situacao <> 3
		INNER JOIN ESC_Escola esc WITH(NOLOCK)
			ON tur.esc_id = esc.esc_id
			AND esc.esc_situacao <> 3
		INNER JOIN ACA_CalendarioPeriodo cap WITH(NOLOCK)
			ON cap.cal_id = tur.cal_id
			AND cap.cap_situacao <> 3
		INNER JOIN REL_TurmaDisciplinaSituacaoFechamento tdf WITH(NOLOCK)
			ON tdf.tud_id = tud.tud_id
			AND tdf.tpc_id = cap.tpc_id
	GROUP BY
		tud.tud_id,
		tud.tud_tipo,
		esc.esc_id,
		esc.ent_id,
		tur.cal_id,
		cap.tpc_id,
		cap.cap_dataInicio,
		tdf.Pendente,
		tdf.PendentePlanejamento,
		tdf.PendenteParecer,
		tdf.DataProcessamento

	UNION

	SELECT
		tudComponente.tud_id,
		tud.tud_id AS tud_idRegencia,
		tudComponente.tud_tipo,
		esc.esc_id,
		esc.ent_id,
		tur.cal_id,
		cap.tpc_id,
		CASE WHEN cap.cap_dataInicio > GETDATE() 
			 THEN 0 ELSE tdf.Pendente END AS Pendente,
		CASE WHEN cap.cap_dataInicio > GETDATE() 
			 THEN 0 ELSE tdf.PendentePlanejamento END AS PendentePlanejamento,
		CASE WHEN cap.cap_dataInicio > GETDATE() 
			 THEN 0 ELSE tdf.PendenteParecer END AS PendenteParecer,
		tdf.DataProcessamento
	FROM
		@tabelaTurmaDisciplina ttd
		INNER JOIN TUR_TurmaDisciplina tud WITH(NOLOCK)
			ON ttd.tud_id = tud.tud_id
			AND tud.tud_tipo = 11
		INNER JOIN TUR_TurmaRelTurmaDisciplina trt WITH(NOLOCK)
			ON trt.tud_id = tud.tud_id
		INNER JOIN TUR_TurmaRelTurmaDisciplina trtComponente WITH(NOLOCK)
			ON trtComponente.tur_id = trt.tur_id
		INNER JOIN TUR_TurmaDisciplina tudComponente WITH(NOLOCK)
			ON trtComponente.tud_id = tudComponente.tud_id
			AND tudComponente.tud_tipo = 12
			AND tudComponente.tud_situacao <> 3
		INNER JOIN TUR_Turma tur WITH(NOLOCK)
			ON tur.tur_id = trt.tur_id
			AND tur.tur_situacao <> 3
		INNER JOIN ESC_Escola esc WITH(NOLOCK)
			ON tur.esc_id = esc.esc_id
			AND esc.esc_situacao <> 3
		INNER JOIN ACA_CalendarioPeriodo cap WITH(NOLOCK)
			ON cap.cal_id = tur.cal_id
			AND cap.cap_situacao <> 3
		INNER JOIN REL_TurmaDisciplinaSituacaoFechamento tdf WITH(NOLOCK)
			ON tdf.tud_id = tudComponente.tud_id
			AND tdf.tpc_id = cap.tpc_id
	GROUP BY
		tudComponente.tud_id,
		tud.tud_id,
		tudComponente.tud_tipo,
		esc.esc_id,
		esc.ent_id,
		tur.cal_id,
		cap.tpc_id,
		cap.cap_dataInicio,
		tdf.Pendente,
		tdf.PendentePlanejamento,
		tdf.PendenteParecer,
		tdf.DataProcessamento

	DECLARE @tpcs TABLE (esc_id INT, cal_id INT, tpc_id INT, PRIMARY KEY(esc_id, cal_id, tpc_id))
	DECLARE @escolas TABLE (esc_id INT, cal_id INT, ent_id UNIQUEIDENTIFIER, PRIMARY KEY(esc_id, cal_id))

	INSERT INTO @escolas (esc_id, cal_id, ent_id)
	SELECT esc_id, cal_id, ent_id FROM @tabelaPendenciaFechamento GROUP BY esc_id, cal_id, ent_id

	--Adiciona os eventos de fechamento da escola que estão vigentes ou já finalizaram
	INSERT INTO @tpcs (esc_id, cal_id, tpc_id)
	SELECT esc.esc_id, esc.cal_id, evtE.tpc_id
	FROM @escolas esc
	INNER JOIN ACA_Evento evtE WITH(NOLOCK)
		ON Esc.ent_id = evtE.ent_id
		AND Esc.esc_id = evtE.esc_id
		AND evtE.evt_padrao = 0
		AND evtE.tev_id = @tev_EfetivacaoNotas
		AND evtE.evt_situacao <> 3
		AND (evtE.evt_dataFim <= @DataAtual OR evtE.evt_dataInicio <= @DataAtual)
		AND evtE.tpc_id IS NOT NULL
	INNER JOIN ACA_CalendarioEvento cev WITH(NOLOCK)
		ON cev.cal_id = esc.cal_id
		AND evtE.evt_id = cev.evt_id
	
	GROUP BY esc.esc_id, esc.cal_id, evtE.tpc_id

	--Se a escola não possui evento de fechamento então adiciona o evento de fechamento padrão que estão vigentes ou já finalizaram
	INSERT INTO @tpcs (esc_id, cal_id, tpc_id)
	SELECT esc.esc_id, esc.cal_id, evtP.tpc_id
	FROM @escolas esc
	INNER JOIN ACA_Evento evtP WITH(NOLOCK)
		ON Esc.ent_id = evtP.ent_id
		AND evtP.evt_padrao = 1
		AND evtP.tev_id = @tev_EfetivacaoNotas
		AND evtP.evt_situacao <> 3
		AND (evtP.evt_dataFim <= @DataAtual OR evtP.evt_dataInicio <= @DataAtual)
		AND evtP.tpc_id IS NOT NULL
	INNER JOIN ACA_CalendarioEvento cev WITH(NOLOCK)
		ON cev.cal_id = esc.cal_id
		AND evtP.evt_id = cev.evt_id
	WHERE
		NOT EXISTS(SELECT TOP 1 tpc.esc_id FROM @tpcs tpc
				   WHERE tpc.esc_id = esc.esc_id
					 	 AND tpc.tpc_id = evtP.tpc_id)
	GROUP BY esc.esc_id, esc.cal_id, evtP.tpc_id

	UPDATE tpf
	SET tpf.Pendente = 0, tpf.PendentePlanejamento = 0, tpf.PendenteParecer = 0
	FROM @tabelaPendenciaFechamento tpf
	WHERE 
		NOT EXISTS
		(
			SELECT TOP 1 1
			FROM @tpcs tpc
			WHERE tpf.tpc_id = tpc.tpc_id
		)

	SELECT 
		tud_idRegencia,
		tud_id,
		tud_tipo,
		CAST(MAX(CAST(Pendente AS INT)) AS BIT) AS Pendente,
		CAST(MAX(CAST(PendentePlanejamento AS INT)) AS BIT) AS PendentePlanejamento,
		CAST(MAX(CAST(PendenteParecer AS INT)) AS BIT) AS PendenteParecer,
		MAX(DataProcessamento) AS DataProcessamento
	FROM 
		@tabelaPendenciaFechamento
	GROUP BY
		tud_idRegencia,
		tud_id,
		tud_tipo
END
GO

PRINT N'Altering [dbo].[STP_ACA_TipoCurriculoPeriodo_LOAD]'
GO

ALTER PROCEDURE [dbo].[STP_ACA_TipoCurriculoPeriodo_LOAD]
	@tcp_id Int
	
AS
BEGIN
	SELECT	Top 1
		 tcp_id  
		, tne_id 
		, tme_id 
		, tcp_descricao 
		, tcp_ordem 
		, tcp_situacao 
		, tcp_dataCriacao 
		, tcp_dataAlteracao
		, tcp_objetoAprendizagem

 	FROM
 		ACA_TipoCurriculoPeriodo
	WHERE 
		tcp_id = @tcp_id
END

GO

PRINT N'Creating [dbo].[ACA_ObjetoAprendizagemTipoCiclo]'
GO
CREATE TABLE [dbo].[ACA_ObjetoAprendizagemTipoCiclo]
(
[oap_id] [int] NOT NULL,
[tci_id] [int] NOT NULL
)
GO

PRINT N'Creating primary key [PK_ACA_ObjetoAprendizagemCicloPeriodo] on [dbo].[ACA_ObjetoAprendizagemTipoCiclo]'
GO
ALTER TABLE [dbo].[ACA_ObjetoAprendizagemTipoCiclo] ADD CONSTRAINT [PK_ACA_ObjetoAprendizagemCicloPeriodo] PRIMARY KEY CLUSTERED  ([oap_id], [tci_id])
GO

PRINT N'Creating [dbo].[STP_ACA_ObjetoAprendizagemTipoCiclo_INSERT]'
GO

CREATE PROCEDURE [dbo].[STP_ACA_ObjetoAprendizagemTipoCiclo_INSERT]
	@oap_id Int
	, @tci_id Int

AS
BEGIN
	INSERT INTO 
		ACA_ObjetoAprendizagemTipoCiclo
		( 
			oap_id 
			, tci_id 
 
		)
	VALUES
		( 
			@oap_id 
			, @tci_id 
 
		)
		
		SELECT ISNULL(SCOPE_IDENTITY(),-1)

	
	
END

GO

PRINT N'Creating [dbo].[STP_ACA_ObjetoAprendizagemTipoCiclo_DELETE]'
GO


CREATE PROCEDURE [dbo].[STP_ACA_ObjetoAprendizagemTipoCiclo_DELETE]
	@oap_id INT
	, @tci_id INT

AS
BEGIN
	DELETE FROM 
		ACA_ObjetoAprendizagemTipoCiclo 
	WHERE 
		oap_id = @oap_id 
		AND tci_id = @tci_id 

		
	DECLARE @ret INT
	SELECT @ret = ISNULL(@@ROWCOUNT,-1)
	RETURN @ret
	
END

GO

PRINT N'Refreshing [dbo].[VW_Disciplinas_Por_TurmaAluno]'
GO
EXEC sp_refreshview N'[dbo].[VW_Disciplinas_Por_TurmaAluno]'
GO

PRINT N'Creating [dbo].[STP_ACA_ObjetoAprendizagemTipoCiclo_SELECT]'
GO

CREATE PROCEDURE [dbo].[STP_ACA_ObjetoAprendizagemTipoCiclo_SELECT]
	
AS
BEGIN
	SELECT 
		oap_id
		,tci_id

	FROM 
		ACA_ObjetoAprendizagemTipoCiclo WITH(NOLOCK) 
	
END

GO

PRINT N'Creating [dbo].[NEW_ACA_ObjetoAprendizagemTipoCiclo_ByTipoDisciplina]'
GO
-- ========================================================================
-- Author:		Leonardo Brito
-- Create date: 17/03/2017
-- Description:	Retorna os objetos de aprendizagem pela disciplina e ano
-- ========================================================================
CREATE PROCEDURE [dbo].[NEW_ACA_ObjetoAprendizagemTipoCiclo_ByTipoDisciplina]
	@tds_id Int
	, @cal_ano Int
AS
BEGIN
	SELECT 
		OAP.oap_id
		,tds_id
		,oap_descricao
		, STUFF((SELECT ', ' + tci.tci_nome FROM ACA_ObjetoAprendizagemTipoCiclo OAP_TC WITH(NOLOCK) 
				 INNER JOIN ACA_TipoCiclo tci WITH(NOLOCK) ON OAP_TC.tci_id = tci.tci_id AND tci.tci_situacao <> 3
				 WHERE OAP.oap_id = OAP_TC.oap_id ORDER BY tci.tci_nome FOR XML PATH ('')),1,2,'') AS ciclos
		,CASE
			WHEN oap_situacao = 1 THEN 'Ativo'
			ELSE 'Inativo' 
		 END oap_situacao
	FROM ACA_ObjetoAprendizagem OAP WITH(NOLOCK)
	WHERE 
		tds_id = @tds_id
		AND cal_ano = @cal_ano
		AND OAP.oap_situacao <> 3
	GROUP BY 
		tds_id, oap_descricao, OAP.oap_id, oap_situacao
	ORDER BY
		oap_descricao, ciclos
	
END


GO

PRINT N'Creating [dbo].[CLS_ObjetoAprendizagemTurmaDisciplina]'
GO
CREATE TABLE [dbo].[CLS_ObjetoAprendizagemTurmaDisciplina]
(
[tud_id] [bigint] NOT NULL,
[tpc_id] [int] NOT NULL,
[oap_id] [int] NOT NULL
)
GO

PRINT N'Creating primary key [PK_CLS_ObjetoAprendizagemTurmaDisciplina] on [dbo].[CLS_ObjetoAprendizagemTurmaDisciplina]'
GO
ALTER TABLE [dbo].[CLS_ObjetoAprendizagemTurmaDisciplina] ADD CONSTRAINT [PK_CLS_ObjetoAprendizagemTurmaDisciplina] PRIMARY KEY CLUSTERED  ([tud_id], [oap_id], [tpc_id])
GO

PRINT N'Creating [dbo].[NEW_ACA_ObjetoAprendizagemTipoCiclo_By_Oap_Id]'
GO



CREATE PROCEDURE [dbo].[NEW_ACA_ObjetoAprendizagemTipoCiclo_By_Oap_Id]
	@oap_id Int

AS
BEGIN
	SELECT 
		atc.tci_id,
		CASE WHEN crp.tci_id IS NULL THEN 0 ELSE 1 END AS CicloEmUso
	FROM 
		ACA_ObjetoAprendizagemTipoCiclo atc WITH(NOLOCK)
	LEFT JOIN CLS_ObjetoAprendizagemTurmaDisciplina oat WITH(NOLOCK)
		ON oat.oap_id = atc.oap_id
	LEFT JOIN TUR_TurmaDisciplina tud WITH(NOLOCK)
		ON oat.tud_id = tud.tud_id
		AND tud.tud_situacao <> 3
	LEFT JOIN TUR_TurmaRelTurmaDisciplina trt WITH(NOLOCK)
		ON tud.tud_id = trt.tud_id
	LEFT JOIN TUR_Turma tur WITH(NOLOCK)
		ON trt.tur_id = tur.tur_id
		AND tur.tur_situacao <> 3
	LEFT JOIN TUR_TurmaCurriculo tcr WITH(NOLOCK)
		ON tur.tur_id = tcr.tur_id
		AND tcr.tcr_situacao <> 3
	LEFT JOIN ACA_CurriculoPeriodo crp WITH(NOLOCK)
		ON tcr.cur_id = crp.cur_id
		AND tcr.crr_id = crp.crr_id
		AND tcr.crp_id = crp.crp_id
		AND atc.tci_id = crp.tci_id
		AND crp.crp_situacao <> 3
	WHERE 
		atc.oap_id = @oap_id
	GROUP BY
		atc.tci_id,
		crp.tci_id	
END



GO

PRINT N'Creating [dbo].[NEW_ACA_ObjetoAprendizagemTipoCiclo_DELETE]'
GO


CREATE PROCEDURE [dbo].[NEW_ACA_ObjetoAprendizagemTipoCiclo_DELETE]
	@oap_id INT

AS
BEGIN
	DELETE FROM 
		ACA_ObjetoAprendizagemTipoCiclo 
	WHERE 
		oap_id = @oap_id 

		
	DECLARE @ret INT
	SELECT @ret = ISNULL(@@ROWCOUNT,-1)
	RETURN @ret
	
END

GO

PRINT N'Creating [dbo].[CLS_ObjetoAprendizagemTurmaAula]'
GO
CREATE TABLE [dbo].[CLS_ObjetoAprendizagemTurmaAula]
(
[tud_id] [bigint] NOT NULL,
[tau_id] [int] NOT NULL,
[oap_id] [int] NOT NULL
)
GO

PRINT N'Creating primary key [PK_CLS_ObjetoAprendizagemTurmaAula] on [dbo].[CLS_ObjetoAprendizagemTurmaAula]'
GO
ALTER TABLE [dbo].[CLS_ObjetoAprendizagemTurmaAula] ADD CONSTRAINT [PK_CLS_ObjetoAprendizagemTurmaAula] PRIMARY KEY CLUSTERED  ([tud_id], [tau_id], [oap_id])
GO

PRINT N'Creating [dbo].[STP_CLS_ObjetoAprendizagemTurmaAula_INSERT]'
GO

CREATE PROCEDURE [dbo].[STP_CLS_ObjetoAprendizagemTurmaAula_INSERT]
	@tud_id BigInt
	, @tau_id Int
	, @oap_id Int

AS
BEGIN
	INSERT INTO 
		CLS_ObjetoAprendizagemTurmaAula
		( 
			tud_id 
			, tau_id 
			, oap_id 
 
		)
	VALUES
		( 
			@tud_id 
			, @tau_id 
			, @oap_id 
 
		)
		
		SELECT ISNULL(SCOPE_IDENTITY(),-1)

	
	
END

GO

PRINT N'Refreshing [dbo].[VW_DESESC_SERIE]'
GO
EXEC sp_refreshview N'[dbo].[VW_DESESC_SERIE]'
GO

PRINT N'Creating [dbo].[STP_CLS_ObjetoAprendizagemTurmaDisciplina_INSERT]'
GO

CREATE PROCEDURE [dbo].[STP_CLS_ObjetoAprendizagemTurmaDisciplina_INSERT]
	@tud_id BigInt
	, @oap_id Int
	, @tpc_id Int

AS
BEGIN
	INSERT INTO 
		CLS_ObjetoAprendizagemTurmaDisciplina
		( 
			tud_id 
			, oap_id 
			, tpc_id 
 
		)
	VALUES
		( 
			@tud_id 
			, @oap_id 
			, @tpc_id 
 
		)
		
		SELECT ISNULL(SCOPE_IDENTITY(),-1)

	
	
END

GO

PRINT N'Creating [dbo].[STP_CLS_ObjetoAprendizagemTurmaAula_DELETE]'
GO


CREATE PROCEDURE [dbo].[STP_CLS_ObjetoAprendizagemTurmaAula_DELETE]
	@tud_id BIGINT
	, @tau_id INT
	, @oap_id INT

AS
BEGIN
	DELETE FROM 
		CLS_ObjetoAprendizagemTurmaAula 
	WHERE 
		tud_id = @tud_id 
		AND tau_id = @tau_id 
		AND oap_id = @oap_id 

		
	DECLARE @ret INT
	SELECT @ret = ISNULL(@@ROWCOUNT,-1)
	RETURN @ret
	
END

GO

PRINT N'Altering [dbo].[NEW_ACA_CalendarioAnual_BimestresAberto_SelectBy_EntidadeEscola]'
GO
-- ==============================================================================
-- Author:		Katiusca Murari
-- Create date: 10/04/2015
-- Description:	Retorna os calendários da entidade que tem bimestres abertos 
--				por escola
--
-- Alteração:	Leonardo Brito 14/03/2017
--				Alterada procedure para filtrar os calendários 
--				ligados à escola ou ao docente
-- ==============================================================================
ALTER PROCEDURE [dbo].[NEW_ACA_CalendarioAnual_BimestresAberto_SelectBy_EntidadeEscola]
	@ent_id UNIQUEIDENTIFIER
	, @esc_id INT
	, @tev_idEfetivacao INT
	, @VerificaEscolaCalendarioPeriodo BIT
	, @doc_id BIGINT
	, @usu_id UNIQUEIDENTIFIER
	, @gru_id UNIQUEIDENTIFIER
AS
BEGIN

	DECLARE @tabelaUas TABLE (uad_id UNIQUEIDENTIFIER NOT NULL)
	DECLARE @cal_ids TABLE (cal_id INT)
	
	IF (ISNULL(@doc_id, 0) > 0)
	BEGIN
		INSERT INTO @cal_ids
		SELECT tur.cal_id FROM TUR_TurmaDocente tdt WITH(NOLOCK)
		INNER JOIN TUR_TurmaRelTurmaDisciplina trt WITH(NOLOCK) ON tdt.tud_id = trt.tud_id
		INNER JOIN TUR_Turma tur WITH(NOLOCK) ON trt.tur_id = tur.tur_id AND tur.tur_situacao <> 3
		WHERE tdt.doc_id = @doc_id AND tdt.tdt_situacao <> 3
		GROUP BY tur.cal_id
	END
	ELSE IF (@usu_id IS NOT NULL AND @gru_id IS NOT NULL)
	BEGIN
		INSERT INTO @tabelaUas 
		SELECT uad_id FROM Synonym_FN_Select_UAs_By_PermissaoUsuario(@usu_id, @gru_id) GROUP BY uad_id

		INSERT INTO @cal_ids
		SELECT cac.cal_id FROM @tabelaUas t
		INNER JOIN ESC_Escola esc WITH(NOLOCK) ON t.uad_id = esc.uad_id AND esc.esc_situacao <> 3
		INNER JOIN ACA_CurriculoEscola ces WITH(NOLOCK) ON esc.esc_id = ces.esc_id AND ces.ces_situacao <> 3
		INNER JOIN ACA_CalendarioCurso cac WITH(NOLOCK) ON ces.cur_id = cac.cur_id
		GROUP BY cac.cal_id
	END

	DECLARE @tbCalendarios TABLE( 
		cal_id INT
		, cal_ano INT
		, cal_descricao VARCHAR(200)
		, cal_periodoLetivo VARCHAR(50)
		, cal_ano_desc VARCHAR(300)
	)
	
	DECLARE @tbEscolaCalendarioPeriodo TABLE (
		tpc_id INT
	)
	
	DECLARE @tbPeriodosEventosEfetivacaoVigente TABLE ( 
		tpc_id INT, cal_id INT
	)
	
	DECLARE @tbTurmasDisciplinas TABLE ( 
		tud_id BIGINT
		, verificar INT
		, tud_tipo INT
		, tpc_id INT
	)
	
	-- Seleciona os calendarios disponiveis para aquela escola e entidade
	INSERT INTO @tbCalendarios ( cal_id, cal_ano, cal_descricao, cal_periodoLetivo, cal_ano_desc)
	SELECT
		CAL.cal_id
		, CAL.cal_ano
		, CAL.cal_descricao
		, CONVERT(VARCHAR, CAL.cal_dataInicio,103) + ' - ' + CONVERT(VARCHAR,CAL.cal_dataFim,103) AS cal_periodoLetivo
		, Convert(VARCHAR,CAL.cal_ano) + ' - ' + CAL.cal_descricao AS cal_ano_desc
	FROM
		TUR_Turma TUR WITH(NOLOCK)
		INNER JOIN ACA_CalendarioAnual CAL WITH (NOLOCK)
			ON	CAL.cal_id = TUR.cal_id
			AND CAL.ent_id = @ent_id
			AND	CAL.cal_situacao <> 3
	WHERE
		TUR.esc_id = @esc_id
		AND TUR.tur_situacao <> 3
		AND ((ISNULL(@doc_id, 0) = 0 AND @usu_id IS NULL AND @gru_id IS NULL) OR
			 EXISTS(SELECT c.cal_id FROM @cal_ids c WHERE CAL.cal_id = c.cal_id))
	GROUP BY
		CAL.cal_id
		, CAL.cal_ano
		, CAL.cal_descricao
		, CAL.cal_dataInicio
		, CAL.cal_dataFim
	
	
	IF @VerificaEscolaCalendarioPeriodo = 1
	BEGIN
		INSERT INTO @tbEscolaCalendarioPeriodo ( tpc_id )
		SELECT 
			ecp.tpc_id 
		FROM 
			@tbCalendarios CAL
			INNER JOIN dbo.ESC_EscolaCalendarioPeriodo ecp WITH(NOLOCK)
				ON ecp.esc_id = @esc_id
				AND ecp.cal_id = CAL.cal_id
	END
	
	-- Traz as disciplina é controlada por período do calendário
	INSERT INTO @tbTurmasDisciplinas( tud_id, verificar, tud_tipo, tpc_id)
	SELECT
		TUD.tud_id
		-- Optativa = 3, Eletiva = 4, DocenteTurmaEletiva = 7, DependeDisponibilidadeProfessorEletiva = 9, DisciplinaEletivaAluno = 10
		, (CASE WHEN TUD.tud_tipo IN (3, 4, 7, 9, 10) THEN 1 ELSE 0 END)
		, TUD.tud_tipo
		, Tdc.tpc_id
	FROM 
		TUR_Turma TUR WITH(NOLOCK)
		INNER JOIN TUR_TurmaRelTurmaDisciplina TUR_TUD WITH(NOLOCK)
			ON TUR_TUD.tur_id = TUR.tur_id
		INNER JOIN TUR_TurmaDisciplina TUD WITH(NOLOCK)
			ON	TUD.tud_id = TUR_TUD.tud_id
			AND TUD.tud_situacao <> 3
		LEFT JOIN TUR_TurmaDisciplinaCalendario AS Tdc WITH(NOLOCK)
			ON Tdc.tud_id = Tud.tud_id
	WHERE
		TUR.esc_id = @esc_id
		AND TUR.tur_situacao <> 3
	
	-- Seleciona os bimestres que tem eventos ativos	
	INSERT INTO @tbPeriodosEventosEfetivacaoVigente ( tpc_id, cal_id )
	SELECT 
		Evt.tpc_id
		, Cal.cal_id
	FROM @tbCalendarios CAL
		INNER JOIN ACA_Evento Evt WITH(NOLOCK)
			ON Evt.tev_id = @tev_idEfetivacao
			AND Evt.tpc_id IS NOT NULL
			-- Eventos vigentes.
			AND CAST(GETDATE() AS DATE) BETWEEN Evt.evt_dataInicio AND Evt.evt_dataFim
			AND 
			(
				-- Ou evento padrão ou evento é da escola da turma.
				Evt.evt_padrao = 1
				OR Evt.esc_id = @esc_id
			)
			AND Evt.evt_situacao <> 3
		INNER JOIN ACA_CalendarioEvento C WITH(NOLOCK)
			ON  C.evt_id = Evt.evt_id
			AND C.cal_id = CAL.cal_id
	
	SELECT
		C.cal_id
		, C.cal_ano
		, C.cal_descricao
		, C.cal_periodoLetivo
		, C.cal_ano_desc
	FROM 
		@tbCalendarios C
		INNER JOIN ACA_CalendarioPeriodo cal WITH (NOLOCK)
			ON cal.cal_id = C.cal_id
			AND cal.cap_situacao <> 3
		INNER JOIN ACA_TipoPeriodoCalendario tpc WITH (NOLOCK)
			ON tpc.tpc_id = cal.tpc_id
		LEFT JOIN @tbEscolaCalendarioPeriodo ecp
			ON cal.tpc_id = ecp.tpc_id
	WHERE
		(
			-- Período vigente.
			CAST(GETDATE() AS DATE) BETWEEN CAST(cal.cap_dataInicio AS DATE) AND CAST(cal.cap_dataFim AS DATE)
			OR
			EXISTS
			(
				-- Ou se o tpc_id está ligado a um evento de efetivação vigente.
				SELECT 1
				FROM @tbPeriodosEventosEfetivacaoVigente Evt
				WHERE cal.cal_id = Evt.cal_id
				AND cal.tpc_id = Evt.tpc_id
			)
		)
		-- Ve se a disciplina tem que verificar
		-- Se não tem que verificar traz o resultado
		-- Se tem que verificar, só traz o resultado se ela possui o tpc que esta trazendo
		AND EXISTS (
			SELECT TOP 1 P.tpc_id
			FROM  @tbTurmasDisciplinas P
			WHERE
				P.verificar = 0
				OR ( P.verificar = 1 AND P.tpc_id = tpc.tpc_id)
		)
		AND ecp.tpc_id IS NULL
	GROUP BY
		C.cal_id
		, C.cal_ano
		, C.cal_descricao
		, C.cal_periodoLetivo
		, C.cal_ano_desc
	ORDER BY
		cal_ano DESC
		, cal_descricao DESC
	
END
GO

PRINT N'Creating [dbo].[STP_CLS_ObjetoAprendizagemTurmaDisciplina_DELETE]'
GO


CREATE PROCEDURE [dbo].[STP_CLS_ObjetoAprendizagemTurmaDisciplina_DELETE]
	@tud_id BIGINT
	, @oap_id INT
	, @tpc_id INT

AS
BEGIN
	DELETE FROM 
		CLS_ObjetoAprendizagemTurmaDisciplina 
	WHERE 
		tud_id = @tud_id 
		AND oap_id = @oap_id 
		AND tpc_id = @tpc_id 

		
	DECLARE @ret INT
	SELECT @ret = ISNULL(@@ROWCOUNT,-1)
	RETURN @ret
	
END

GO

PRINT N'Creating [dbo].[STP_CLS_ObjetoAprendizagemTurmaAula_SELECT]'
GO

CREATE PROCEDURE [dbo].[STP_CLS_ObjetoAprendizagemTurmaAula_SELECT]
	
AS
BEGIN
	SELECT 
		tud_id
		,tau_id
		,oap_id

	FROM 
		CLS_ObjetoAprendizagemTurmaAula WITH(NOLOCK) 
	
END

GO

PRINT N'Refreshing [dbo].[VW_MatriculasBoletimTodas]'
GO
EXEC sp_refreshview N'[dbo].[VW_MatriculasBoletimTodas]'
GO

PRINT N'Creating [dbo].[STP_CLS_ObjetoAprendizagemTurmaDisciplina_SELECT]'
GO

CREATE PROCEDURE [dbo].[STP_CLS_ObjetoAprendizagemTurmaDisciplina_SELECT]
	
AS
BEGIN
	SELECT 
		tud_id
		,oap_id
		,tpc_id

	FROM 
		CLS_ObjetoAprendizagemTurmaDisciplina WITH(NOLOCK) 
	
END

GO

PRINT N'Creating [dbo].[NEW_CLS_ObjetoAprendizagemTurmaDisciplina_SelecionaObjTudTpc]'
GO
-- ========================================================================
-- Author:		Leonardo Brito
-- Create date: 17/03/2017
-- Description:	Seleciona os objetos de aprendizagem ligados à disciplina e período do calendário
-- ========================================================================
CREATE PROCEDURE [dbo].[NEW_CLS_ObjetoAprendizagemTurmaDisciplina_SelecionaObjTudTpc]
	@tud_id BIGINT
	, @tpc_id INT
AS
BEGIN
	SELECT
		oap.oap_id,
		oap.oap_descricao
	FROM CLS_ObjetoAprendizagemTurmaDisciplina oat WITH(NOLOCK)
	INNER JOIN ACA_ObjetoAprendizagem oap WITH(NOLOCK)
		ON oat.oap_id = oap.oap_id
		AND oap.oap_situacao <> 3
	WHERE 
		oat.tud_id = @tud_id
		AND oat.tpc_id = @tpc_id
	ORDER BY
		oap.oap_descricao
END


GO

PRINT N'Creating [dbo].[NEW_CLS_ObjetoAprendizagemTurmaAula_SelecionaObjTudTau]'
GO
-- ========================================================================
-- Author:		Leonardo Brito
-- Create date: 17/03/2017
-- Description:	Seleciona os objetos de aprendizagem ligados à aula da disciplina
-- ========================================================================
CREATE PROCEDURE [dbo].[NEW_CLS_ObjetoAprendizagemTurmaAula_SelecionaObjTudTau]
	@tud_id BIGINT
	, @tau_id INT
AS
BEGIN
	SELECT
		oap.oap_id,
		oap.oap_descricao
	FROM CLS_ObjetoAprendizagemTurmaAula oaa WITH(NOLOCK)
	INNER JOIN CLS_TurmaAula tau WITH(NOLOCK)
		ON oaa.tud_id = tau.tud_id
		AND oaa.tau_id = tau.tau_id
		AND tau.tau_situacao <> 3
	INNER JOIN CLS_ObjetoAprendizagemTurmaDisciplina oat WITH(NOLOCK)
		ON oaa.tud_id = oat.tud_id
		AND tau.tpc_id = oat.tpc_id
		AND oaa.oap_id = oat.oap_id
	INNER JOIN ACA_ObjetoAprendizagem oap WITH(NOLOCK)
		ON oaa.oap_id = oap.oap_id
		AND oap.oap_situacao <> 3
	WHERE 
		oaa.tud_id = @tud_id
		AND oaa.tau_id = @tau_id
	ORDER BY
		oap.oap_descricao
END


GO

PRINT N'Refreshing [dbo].[VW_Alunos_Acesso_Boletim]'
GO
EXEC sp_refreshview N'[dbo].[VW_Alunos_Acesso_Boletim]'
GO

PRINT N'Refreshing [dbo].[VW_DESESC_TURMA_SERIE_ESCOLA]'
GO
EXEC sp_refreshview N'[dbo].[VW_DESESC_TURMA_SERIE_ESCOLA]'
GO

PRINT N'Refreshing [dbo].[VW_DadosAlunosRede]'
GO
EXEC sp_refreshview N'[dbo].[VW_DadosAlunosRede]'
GO

PRINT N'Creating [dbo].[NEW_CLS_ObjetoAprendizagemTurmaAula_DELETETudTau]'
GO
-- ========================================================================
-- Author:		Leonardo Brito
-- Create date: 17/03/2017
-- Description:	Deleta todos os relacionamentos da turma aula com objetos de aprendizagem
-- ========================================================================
CREATE PROCEDURE [dbo].[NEW_CLS_ObjetoAprendizagemTurmaAula_DELETETudTau]
	@tud_id BIGINT
	, @tau_id INT
AS
BEGIN
    SET NOCOUNT ON ;

	DELETE FROM CLS_ObjetoAprendizagemTurmaAula
	WHERE tud_id = @tud_id
		AND tau_id = @tau_id
END


GO

PRINT N'Creating [dbo].[STP_TUR_TurmaDisciplinaRelacionada_INSERT]'
GO


CREATE PROCEDURE [dbo].[STP_TUR_TurmaDisciplinaRelacionada_INSERT]
	@tdr_id BigInt
	, @tud_id BigInt
	, @tud_idRelacionada BigInt
	, @tdr_vigenciaInicio DateTime
	, @tdr_vigenciaFim DateTime
	, @tdr_situacao TinyInt
	, @tdr_dataCriacao DateTime
	, @tdr_dataAlteracao DateTime

AS
BEGIN
	INSERT INTO 
		TUR_TurmaDisciplinaRelacionada
		( 
			tdr_id 
			, tud_id 
			, tud_idRelacionada 
			, tdr_vigenciaInicio 
			, tdr_vigenciaFim 
			, tdr_situacao 
			, tdr_dataCriacao 
			, tdr_dataAlteracao 
 
		)
	VALUES
		( 
			@tdr_id 
			, @tud_id 
			, @tud_idRelacionada 
			, @tdr_vigenciaInicio 
			, @tdr_vigenciaFim 
			, @tdr_situacao 
			, @tdr_dataCriacao 
			, @tdr_dataAlteracao 
 
		)
		
		SELECT ISNULL(SCOPE_IDENTITY(),-1)

	
	
END


GO

PRINT N'Creating [dbo].[STP_CLS_ObjetoAprendizagemTurmaDisciplina_LOAD]'
GO


CREATE PROCEDURE [dbo].[STP_CLS_ObjetoAprendizagemTurmaDisciplina_LOAD]
	@tud_id BigInt
	, @tpc_id Int
	, @oap_id Int
	
AS
BEGIN
	SELECT	Top 1
		 tud_id  
		, tpc_id 
		, oap_id 

 	FROM
 		CLS_ObjetoAprendizagemTurmaDisciplina
	WHERE 
		tud_id = @tud_id
		AND tpc_id = @tpc_id
		AND oap_id = @oap_id
END


GO

PRINT N'Altering [dbo].[MS_JOB_ProcessamentoRelatorioDisciplinasAlunosPendencias]'
GO
-- =============================================
-- Author:		Pedro Silva
-- Create date: 14/07/2015
-- Description:	Chama a STP MS_JOB_RelatorioDisciplinasAlunosPendencias de acordo com regra pré-definida,
--			    para gerar os dados de pendência de fechamento por Dre/Escola/Calendário/Bimestre.
--				Roda pelo Quartz todos os dias, sendo que no Domingo carrega todos os dados desde 2014 e nos outros dias, apenas 2015
-- Author:		Juliano
-- Create date: 28/09/2015
-- Description:	Alterados os filtros de chamada do MS_JOB_RelatorioDisciplinasAlunosPendencias, para buscar da 
--				fila de fechamento (assim que salva os dados, gera a fila de fechamento).
-- Author:		Carla Frascareli
-- Create date: 10/10/2015
-- Description:	Melhorias de performance - removi a chamada da procedure que processava por bimestre, e fiz o processamento
--				para vários tud e tpcs. Ao invés de deletar e inserir na tabela do relatório, foi dado um merge.
-- Author:		Pedro Silva
-- Create date: 10/10/2015
-- Description:	Adicionado o parâmetro @tud_idFiltrar, para atender a um "fura-fila" de pendências também.

-- Alterado: Jean Michel - 26/07/2016
-- Description: Alterado para considerar as experiências do território
-- =============================================
ALTER PROCEDURE [dbo].[MS_JOB_ProcessamentoRelatorioDisciplinasAlunosPendencias]
	@tud_idFiltrar BIGINT = NULL
AS
BEGIN

	DECLARE @tbPendencias TABLE
	(tud_id BIGINT, tpc_id INT, processado tinyint
	PRIMARY	KEY	(tud_id, tpc_id))

	--adicionado este if para tratar o "fura-fila" de pendências... 
	--ao invés de pegar da CLS_AlunoFechamentoPendencia, pega dp parametro
	IF (@tud_idFiltrar is null)
	BEGIN
		-- Busca os registros que serão afetados.
		INSERT INTO @tbPendencias
		(tud_id, tpc_id, processado)
		SELECT TOP 500
			tud_id, tpc_id, 3
		FROM CLS_AlunoFechamentoPendencia WITH(NOLOCK)
		WHERE afp_processado = 2

		; WITH Regencias AS
		(
			SELECT P.tud_id, P.tpc_id, ttrtd.tur_id
			FROM @tbPendencias P
			INNER JOIN dbo.TUR_TurmaDisciplina ttd WITH (NOLOCK)
			ON ttd.tud_id = P.tud_id
			AND ttd.tud_tipo=11--regencia
			INNER JOIN dbo.TUR_TurmaRelTurmaDisciplina ttrtd	 WITH (NOLOCK)
			ON ttrtd.tud_id = ttd.tud_id
		)
		-- Adicionar os componentes das regências que serão processadas.
		INSERT INTO @tbPendencias
		(tud_id, tpc_id, processado)
		SELECT TudComp.tud_id, R.tpc_id, 3
		FROM Regencias R
		INNER JOIN TUR_TurmaRelTurmaDisciplina RelTudComp WITH(NOLOCK)
			ON RelTudComp.tur_id = R.tur_id
		INNER JOIN dbo.TUR_TurmaDisciplina TudComp WITH (NOLOCK)
			ON TudComp.tud_id = RelTudComp.tud_id
			AND TudComp.tud_tipo = 12 --componente da regencia
		EXCEPT
		(
			SELECT tud_id, tpc_id, processado
			FROM @tbPendencias
		)


		-- Altera os registros afetados para "em processamento".
		UPDATE CLS_AlunoFechamentoPendencia SET afp_processado = 3
		  FROM @tbPendencias tp 
			   INNER JOIN CLS_AlunoFechamentoPendencia P WITH(NOLOCK)
					   ON P.tud_id	 = tp.tud_id
					  AND p.tpc_id = tp.tpc_id
	END
	ELSE
	BEGIN
		INSERT INTO @tbPendencias
		(tud_id, tpc_id, processado)
		
		(SELECT trel.tud_id, cap.tpc_id, 3
		   FROM TUR_TurmaRelTurmaDisciplina trel WITH(NOLOCK)
				INNER JOIN TUR_Turma tur WITH (NOLOCK) on tur.tur_id = trel.tur_id
				INNER JOIN ACA_CalendarioPeriodo cap WITH (NOLOCK) on cap.cal_id = tur.cal_id and cap.cap_situacao <> 3
		  WHERE trel.tud_id = @tud_idFiltrar
		 
			UNION
		 
		 SELECT ttd2.tud_id, cap.tpc_id, 3
		   FROM TUR_TurmaDisciplina ttd WITH (NOLOCK)
				INNER JOIN TUR_TurmaRelTurmaDisciplina ttrtd	 WITH (NOLOCK)
						ON ttrtd.tud_id = ttd.tud_id
				INNER JOIN TUR_TurmaRelTurmaDisciplina ttrtd2	 WITH (NOLOCK)
						ON ttrtd2.tur_id = ttrtd.tur_id
				INNER JOIN TUR_TurmaDisciplina ttd2	   WITH (NOLOCK)
						ON ttd2.tud_id = ttrtd2.tud_id
					   AND ttd2.tud_tipo = 12 --componente da regencia
				INNER JOIN TUR_Turma tur WITH (NOLOCK) on tur.tur_id = ttrtd.tur_id
				INNER JOIN ACA_CalendarioPeriodo cap WITH (NOLOCK) on cap.cal_id = tur.cal_id and cap.cap_situacao <> 3
		  WHERE ttd.tud_id = @tud_idFiltrar
		    AND ttd.tud_tipo = 11--regencia
		 )

		--Altera os registros afetados para "em processamento".
		UPDATE CLS_AlunoFechamentoPendencia SET afp_processado = 3
		  FROM @tbPendencias tp 
			   INNER JOIN CLS_AlunoFechamentoPendencia P WITH(NOLOCK)
					   ON P.tud_id	 = tp.tud_id
					  AND p.tpc_id = tp.tpc_id
	END

	BEGIN TRY

		DECLARE @filtroTurmaDisciplina TABLE 
		(tur_id BIGINT, tud_id BIGINT, tds_id INT, tpc_id INT, esc_id INT NULL, uni_id INT NULL, cal_id INT NULL,
		PRIMARY KEY (tur_id, tud_id, tpc_id));

		INSERT INTO @filtroTurmaDisciplina
		(tur_id, tud_id, tds_id, tpc_id, esc_id, uni_id, cal_id)
		-- Select de retorno para o serviço - para limpar o cache.
		OUTPUT inserted.tud_id, inserted.tds_id, inserted.tpc_id, inserted.esc_id, inserted.uni_id, inserted.cal_id, inserted.tur_id
		SELECT 
			Tur.tur_id, Afp.tud_id, dis.tds_id, Afp.tpc_id, tur.esc_id, tur.uni_id, tur.cal_id
		FROM @tbPendencias AS AFP
		INNER JOIN dbo.TUR_TurmaRelTurmaDisciplina RelTud WITH(NOLOCK)
			ON RelTud.tud_id = Afp.tud_id
		INNER JOIN TUR_Turma tur WITH(NOLOCK)
			ON RelTud.tur_id = tur.tur_id
		INNER JOIN ACA_CalendarioAnual cal WITH(NOLOCK)
			ON tur.cal_id = cal.cal_id
		INNER JOIN TUR_TurmaDisciplinaRelDisciplina trd WITH(NOLOCK)
			ON RelTud.tud_id = trd.tud_id
		INNER JOIN ACA_Disciplina dis WITH(NOLOCK)
			ON trd.dis_id = dis.dis_id
		WHERE 
			processado = 3

		BEGIN -- Processamento das pendências 

			DECLARE @EscolasTurmasDisciplinasPeriodos AS TABLE 
			(
				esc_id BIGINT NOT NULL,
				tur_id BIGINT NOT NULL,
				tud_id BIGINT NOT NULL,
				tcp_id INT NULL,
				tds_id INT NULL,
				tci_id INT NULL,
				tur_codigo VARCHAR(30),
				cal_id INT NOT NULL,
				fav_id INT NOT NULL,
				ava_id INT NOT NULL,
				tpc_id INT NOT NULL,
				tur_tipo TINYINT NOT NULL,
				cap_dataFim DATE,
				cal_ano INT,
				tpc_nome VARCHAR(200)
				PRIMARY KEY (tur_id, tud_id, tpc_id, fav_id, ava_id)
			);


			INSERT INTO @EscolasTurmasDisciplinasPeriodos (esc_id, tur_id, tud_id, tcp_id, tds_id, tci_id, tur_codigo, cal_id, fav_id, ava_id, tpc_id, tur_tipo, cap_dataFim
			, cal_ano, tpc_nome)
			SELECT
				Tur.esc_id, Tur.tur_id, T.tud_id, crp.tcp_id, T.tds_id, crp.tci_id, tur_codigo, Tur.cal_id, Tur.fav_id, ava_id, T.tpc_id, Tur.tur_tipo, Cap.cap_dataFim
				, Cal.cal_ano, Tpc.tpc_nome
			FROM @filtroTurmaDisciplina T
			INNER JOIN TUR_Turma AS Tur WITH ( NOLOCK )
				ON T.tur_id = Tur.tur_id
			INNER JOIN ACA_CalendarioAnual Cal WITH(NOLOCK)
				ON Cal.cal_id = Tur.cal_id
			INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
				ON Cap.cal_id = Tur.cal_id
				AND Cap.tpc_id = T.tpc_id
				AND Cap.cap_situacao <> 3
			INNER JOIN ACA_TipoPeriodoCalendario Tpc WITH(NOLOCK)
				ON Tpc.tpc_id = Cap.tpc_id
			INNER JOIN ACA_Avaliacao Ava WITH(NOLOCK)
				ON Ava.fav_id = Tur.fav_id
				AND Ava.ava_tipo IN (1,5)
				AND Ava.tpc_id = T.tpc_id
				AND Ava.ava_situacao <> 3
			INNER JOIN TUR_TurmaCurriculo tcr WITH(NOLOCK)
				ON Tur.tur_id = tcr.tur_id
				AND tcr.tcr_situacao <> 3
			INNER JOIN ACA_CurriculoPeriodo crp WITH(NOLOCK)
				ON tcr.cur_id = crp.cur_id
				AND tcr.crr_id = crp.crr_id
				AND tcr.crp_id = crp.crp_id
				AND crp.crp_situacao <> 3

			--SELECT * FROM @EscolasTurmasDisciplinasPeriodos

			DECLARE @MatriculaTurma AS TABLE
			(alu_id BIGINT NOT NULL, mtu_id INT NOT NULL, tur_id BIGINT NOT NULL
			PRIMARY KEY (alu_id, mtu_id, tur_id))

			INSERT INTO @MatriculaTurma (alu_id, mtu_id, tur_id)
			SELECT  
				Mtu.alu_id, MAX(Mtu.mtu_id) AS mtu_id, Tur.tur_id
			FROM @EscolasTurmasDisciplinasPeriodos Tur
			INNER JOIN MTR_MatriculaTurma Mtu WITH(NOLOCK)
				ON Mtu.tur_id = Tur.tur_id
				AND Mtu.mtu_situacao <> 3
			INNER JOIN ACA_Aluno alu WITH(NOLOCK)
				ON Mtu.alu_id = alu.alu_id
				AND alu.alu_situacao <> 3
			WHERE Tur.tur_tipo = 1 --Apenas turmas normais.
			GROUP BY Mtu.alu_id, Tur.tur_id
	
			DECLARE @AlunosDisciplinasPendencias AS TABLE
			(
				alu_id BIGINT,
				mtu_id INT,
				mtd_id INT,
				tur_id BIGINT,
				tud_id BIGINT,
				tud_naoLancarNota BIT NOT NULL,
				tpc_id INT,
				tud_nome VARCHAR(200),
				tud_tipo TINYINT,
				tur_tipo TINYINT,
				SemNota BIT DEFAULT(1),
				SemSintese BIT DEFAULT(0),
				SemResultadoFinal BIT DEFAULT(0),
				SemParecer BIT DEFAULT(0),
				SemPlanejamento BIT DEFAULT(0),
				DisciplinaSemAula BIT DEFAULT(0),
				PRIMARY KEY (tur_id, tud_id, alu_id, mtu_id, mtd_id, tpc_id)
			);

			INSERT INTO @AlunosDisciplinasPendencias
			(alu_id, mtu_id, mtd_id, tur_id, tud_id, tud_naoLancarNota, tpc_id, tud_nome, tud_tipo, tur_tipo)
			SELECT
				Mtr.alu_id, Mtr.mtu_id, Mtd.mtd_id, Mtr.tur_id, Mtd.tud_id, ISNULL(tud_naoLancarNota, 0), Tur.tpc_id, tud_nome, tud_tipo, Tur.tur_tipo
			FROM @MatriculaTurma Mtu
			INNER JOIN @EscolasTurmasDisciplinasPeriodos Tur
				ON Tur.tur_id = Mtu.tur_id
			INNER JOIN MTR_MatriculasBoletim Mtr WITH(NOLOCK)
				ON Mtr.alu_id = Mtu.alu_id
				AND Mtr.mtu_origemDados = Mtu.mtu_id
				AND Mtr.tur_id = Mtu.tur_id
				AND Mtr.tpc_id = Tur.tpc_id
				AND Mtr.PossuiSaidaPeriodo = 0
				AND Mtr.registroExterno = 0
			-- Pegar tud_id e mtd_id pelo mtu_id, para buscar as EFs.
			INNER JOIN MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
				ON Mtd.alu_id = Mtr.alu_id
				AND Mtd.mtu_id = Mtr.mtu_id
				AND Mtd.tud_id = Tur.tud_id
				AND Mtd.mtd_situacao <> 3
			INNER JOIN TUR_TurmaDisciplina Tud WITH(NOLOCK)
				ON Tud.tud_id = Mtd.tud_id
				AND Tud.tud_situacao <> 3
				-- Não trazer 10-Eletiva.
				-- Não trazer 11-Regência - só verifica nota nos seus componentes.
				-- Não trazer 14-Ed. Física Multiseriada
				-- Não trazer 17-Compartilhada.				
				-- Não trazer 19-Territorio - só verifica disciplina sem aula na experiência
				AND tud.tud_tipo NOT IN (10, 11, 14, 17, 19)

			INSERT INTO @AlunosDisciplinasPendencias
			(alu_id, mtu_id, mtd_id, tur_id, tud_id, tud_naoLancarNota, tpc_id, tud_nome, tud_tipo, tur_tipo)
			SELECT
				Mtd.alu_id, Mtd.mtu_id, Mtd.mtd_id, Tur.tur_id, Mtd.tud_id, ISNULL(tud_naoLancarNota, 0), tpc_id, tud_nome, tud_tipo, Tur.tur_tipo
			FROM @EscolasTurmasDisciplinasPeriodos Tur
			INNER JOIN TUR_TurmaDisciplina Tud WITH(NOLOCK)
				ON Tur.tud_id = Tud.tud_id
				AND Tud.tud_situacao <> 3
				-- Não trazer 11-Regência - só verifica nota nos seus componentes.
				-- Não trazer 17-Compartilhada.				
				-- Não trazer 19-Territorio - só verifica disciplina sem aula na experiência
				AND Tud.tud_tipo NOT IN (11, 17, 19)
			INNER JOIN MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
				ON Mtd.tud_id = Tud.tud_id
				AND Mtd.mtd_situacao <> 3
				AND Mtd.mtd_dataMatricula <= Tur.cap_dataFim
				AND (Mtd.mtd_dataSaida IS NULL OR Mtd.mtd_dataSaida > Tur.cap_dataFim)
			WHERE Tur.tur_tipo in (2,3) --Apenas turmas eletivas (recuperação).

			--select * from @AlunosDisciplinasPendencias
			--order by alu_Id
			
		----	select * from @AlunosDisciplinasPendencias where tud_id = 248490
			
			; WITH TurmaDisciplinaComObjetoAprendizagem AS 
			(
				SELECT Tur.tud_id, Tur.tpc_id, MAX(oat.oap_id) AS oap_id
				FROM @EscolasTurmasDisciplinasPeriodos Tur
				INNER JOIN CLS_ObjetoAprendizagemTurmaDisciplina oat WITH(NOLOCK)
					ON Tur.tud_id = oat.tud_id
					AND Tur.tpc_id = oat.tpc_id
				GROUP BY Tur.tud_id, Tur.tpc_id
			)
			, ObjetosAprendizagem AS 
			(
				SELECT Tur.tci_id, Tur.tds_id, MAX(oap.oap_id) AS oap_id
				FROM @EscolasTurmasDisciplinasPeriodos Tur
				INNER JOIN ACA_TipoCiclo tci WITH(NOLOCK)
					ON Tur.tci_id = tci.tci_id
					AND ISNULL(tci.tci_objetoAprendizagem,0) = 1
				INNER JOIN ACA_TipoCurriculoPeriodo tcrp WITH(NOLOCK)
					ON Tur.tcp_id = tcrp.tcp_id
					AND ISNULL(tcrp.tcp_objetoAprendizagem,0) = 1
				INNER JOIN ACA_ObjetoAprendizagemTipoCiclo AS ocp WITH(NOLOCK)
					ON Tur.tci_id = ocp.tci_id
				INNER JOIN ACA_ObjetoAprendizagem oap WITH(NOLOCK)
					ON ocp.oap_id = oap.oap_id 
					AND Tur.tds_id = oap.tds_id 
					AND oap.oap_situacao <> 3
				GROUP BY Tur.tci_id, Tur.tds_id
			)
			, TurmaDisciplinaObjetoAprendizagem AS
			(
				SELECT Mtr.tud_id, Mtr.tur_id, Mtr.alu_id, Mtr.mtu_id, Mtr.mtd_id, Mtr.tpc_id, 
								 --Se existe pelo menos um objeto de aprendizagem para prencher
					   CASE WHEN EXISTS(SELECT TOP 1 oap.oap_id FROM ObjetosAprendizagem oap
										WHERE Tur.tci_id = oap.tci_id AND Tur.tds_id = oap.tds_id) AND
								 --E não foi preenchido nenhum objeto no período para a turma disciplina
								 NOT EXISTS(SELECT TOP 1 oat.oap_id FROM TurmaDisciplinaComObjetoAprendizagem oat
											WHERE Tur.tud_id = oat.tud_id AND Tur.tpc_id = oat.tpc_id) 
						    THEN 1 ELSE 0 END AS pendente
				FROM @AlunosDisciplinasPendencias Mtr
				INNER JOIN @EscolasTurmasDisciplinasPeriodos Tur
					ON Tur.tur_id = Mtr.tur_id
					AND Tur.tud_id = Mtr.tud_id
					AND tur.tpc_id = Mtr.tpc_id
				INNER JOIN ACA_TipoCiclo tci WITH(NOLOCK)
					ON Tur.tci_id = tci.tci_id
					AND ISNULL(tci.tci_objetoAprendizagem,0) = 1
				INNER JOIN ACA_TipoCurriculoPeriodo tcrp WITH(NOLOCK)
					ON Tur.tcp_id = tcrp.tcp_id
					AND ISNULL(tcrp.tcp_objetoAprendizagem,0) = 1
				WHERE
				   Mtr.tud_tipo NOT IN (19, 11) --Não verifica pendência para Territórios e Regência
			)
			UPDATE @AlunosDisciplinasPendencias
			SET SemPlanejamento = 1
			FROM TurmaDisciplinaObjetoAprendizagem Tud
			INNER JOIN @AlunosDisciplinasPendencias P
				ON P.tur_id = Tud.tur_id
				AND P.tud_id = Tud.tud_id
				AND P.alu_id = Tud.alu_id
				AND P.mtu_id = Tud.mtu_id
				AND P.mtd_id = Tud.mtd_id
				AND P.tpc_id = Tud.tpc_id
			WHERE Tud.pendente = 1

			; WITH AlunosComNota AS
			(
				-- Alunos com lançamento de nota ok.
				SELECT Mtr.tud_id, Mtr.tur_id, Mtr.alu_id, Mtr.mtu_id, Mtr.mtd_id, Mtr.tpc_id
				FROM @AlunosDisciplinasPendencias Mtr
				INNER JOIN @EscolasTurmasDisciplinasPeriodos Tur
					ON Tur.tur_id = Mtr.tur_id
					AND Tur.tud_id = Mtr.tud_id
					AND tur.tpc_id = Mtr.tpc_id
				INNER JOIN CLS_AlunoAvaliacaoTurmaDisciplina AS Atd WITH (NOLOCK)
					ON Atd.tud_id = Mtr.tud_id
					AND Atd.alu_id = Mtr.alu_id
					AND Atd.mtu_id = Mtr.mtu_id
					AND Atd.mtd_id = Mtr.mtd_id
					AND Atd.fav_id = Tur.fav_id
					AND Atd.ava_id = Tur.ava_id
					AND Atd.atd_situacao <> 3
				WHERE
					(
						(
							-- Caso tenha que lançar nota, traz registros que possuam nota.
							tud_naoLancarNota = 0 AND 
							NOT (COALESCE(atd_avaliacaoPosConselho, atd_avaliacao, '') = '') 
						)
					OR 
						-- Caso não seja de lançar nota, traz o registro somente.
						(tud_naoLancarNota = 1)
					)
			)
			UPDATE @AlunosDisciplinasPendencias
			SET SemNota = 0
			FROM AlunosComNota Alu
			INNER JOIN @AlunosDisciplinasPendencias P
				ON P.tur_id = Alu.tur_id
				AND P.tud_id = Alu.tud_id
				AND P.alu_id = Alu.alu_id
				AND P.mtu_id = Alu.mtu_id
				AND P.mtd_id = Alu.mtd_id
				AND P.tpc_id = Alu.tpc_id

			--IF (@tpc_id = 4)
			BEGIN -- Pegar alunos sem parecer conclusivo se for o último bimestre.
				UPDATE @AlunosDisciplinasPendencias
				SET SemParecer = 1
				FROM @AlunosDisciplinasPendencias P
				INNER JOIN MTR_MatriculaTurma Mtu WITH(NOLOCK)
					ON P.alu_id = Mtu.alu_id
					AND P.mtu_id = Mtu.mtu_id
					AND P.tpc_id = 4
					AND Mtu.mtu_resultado IS NULL
					AND P.tur_tipo = 1

				-- Para os casos em que não é "Experiência" (Território do Saber), mantem a verificação atual
				UPDATE @AlunosDisciplinasPendencias
				SET SemResultadoFinal = 1
				FROM @AlunosDisciplinasPendencias P
				INNER JOIN MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
					ON P.alu_id = Mtd.alu_id
					AND P.mtu_id = Mtd.mtu_id
					AND P.tud_id = Mtd.tud_id
					AND P.tpc_id = 4
					AND Mtd.mtd_situacao <> 3
					AND Mtd.mtd_resultado IS NULL
				WHERE
					-- Só verifica se nao tem resultado na disciplina(Sintese final) se for "nao lançar nota" ou eletiva de aluno.
				   (P.tud_naoLancarNota = 1 OR P.tud_tipo = 10)
					-- Regra padrão, exceto para "Experiência" (Território do Saber)
				   AND P.tud_tipo != 18

				-- Para os casos em que é "Experiência" (Território do Saber)
				-- Só marca pendencia de resultado final, quando a experiência é oferecida no último período do calendário
				UPDATE @AlunosDisciplinasPendencias
				SET SemResultadoFinal = 1
				FROM @AlunosDisciplinasPendencias P
				INNER JOIN MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
					ON P.alu_id = Mtd.alu_id
					AND P.mtu_id = Mtd.mtu_id
					AND P.tud_id = Mtd.tud_id
					AND P.tpc_id = 4
					AND Mtd.mtd_situacao <> 3
					AND Mtd.mtd_resultado IS NULL
				INNER JOIN TUR_Turma tur WITH (NOLOCK)
					ON tur.tur_id = P.tur_id
				INNER JOIN ACA_CalendarioPeriodo cap WITH (NOLOCK)
					ON cap.cal_id = tur.cal_id
					AND cap.tpc_id = 4
				INNER JOIN TUR_TurmaDisciplinaTerritorio tte WITH (NOLOCK)
					ON tte.tud_idExperiencia = P.tud_id
					-- Apenas experiências ativas no último período do calendário		
					AND cap.cap_dataFim >= tte.tte_vigenciaInicio
					AND cap.cap_dataInicio <= ISNULL(tte.tte_vigenciaFim, cap.cap_dataInicio)
					AND tte.tte_situacao <> 3
				WHERE
					-- Só verifica se nao tem resultado na disciplina(Sintese final) se for "nao lançar nota" ou eletiva de aluno.
				   (P.tud_naoLancarNota = 1 OR P.tud_tipo = 10)
					-- Regra específica para "Experiência" (Território do Saber)
				   AND P.tud_tipo = 18
		
				-- Pedro Silva 20/08
				-- neste caso do campo SemSintese, estou usando a lógica contrária: 
				-- Seto todos para 1, e coloco 0 apenas nos q tiverem registros. 
				-- Fiz isto para evitar o left join e ter melhor performance
				UPDATE @AlunosDisciplinasPendencias
				SET SemSintese = 1
				FROM @AlunosDisciplinasPendencias P
				WHERE P.tpc_id = 4 
				  AND P.tud_naoLancarNota = 0 AND P.tud_tipo <> 10
		
				UPDATE @AlunosDisciplinasPendencias
				SET SemSintese = 0
				FROM @AlunosDisciplinasPendencias P
				INNER JOIN CLS_AlunoAvaliacaoTurmaDisciplina Atd WITH(NOLOCK)
						ON P.tud_id = Atd.tud_id
					   AND P.alu_id = Atd.alu_id
					   AND P.mtu_id = Atd.mtu_id
					   AND P.mtd_id = Atd.mtd_id
					   AND Atd.atd_situacao <> 3
					   AND Atd.atd_avaliacao IS NOT NULL
				INNER JOIN ACA_Avaliacao Ava  WITH(NOLOCK)
						ON Ava.fav_id = Atd.fav_id
					   AND Ava.ava_id = Atd.ava_id
					   AND Ava.ava_tipo = 3 --FINAL
				WHERE P.tpc_id = 4 
					-- Só verifica se nao tem avaliação na disciplina(Sintese final) se for "nao lançar nota" = 1 e não for eletiva de aluno.
				  AND P.tud_naoLancarNota = 0 AND P.tud_tipo <> 10

				UPDATE @AlunosDisciplinasPendencias
				SET SemSintese = 0
				FROM @AlunosDisciplinasPendencias P
				WHERE P.tpc_id = 4 AND P.tud_naoLancarNota = 1
		  
			END

			-- Pegar disciplinas que não lançam nota e ver se elas tem aula (é obrigatório ter uma aula para não ter pendência).
			-- Verificação padrão. Vale para todas as disciplinas exceto para Experiência (Território do Saber)
			; WITH DisciplinasNaoLancarNota AS
			(
				SELECT
					Tud.tud_id
					, Tud.tpc_id
				FROM 
					@AlunosDisciplinasPendencias Tud
				WHERE 
					Tud.tud_naoLancarNota = 1
					-- Tudo que não lança nota exceto Experiência (Território do Saber)
					AND tud.tud_tipo <> 18
				GROUP BY
					Tud.tud_id
					, Tud.tpc_id
			)
			, DisciplinasSemAula AS
			(
				SELECT 
					Tud.tud_id
					, Tud.tpc_id
				FROM
					DisciplinasNaoLancarNota Tud
				WHERE
					NOT EXISTS
						(
							SELECT TOP 1 1
							FROM CLS_TurmaAula Tau WITH(NOLOCK)
							WHERE
								Tau.tud_id = Tud.tud_id
								AND Tau.tpc_id = Tud.tpc_id
								AND Tau.tau_situacao <> 3
						)
			)
			UPDATE @AlunosDisciplinasPendencias
			SET DisciplinaSemAula = 1
			FROM DisciplinasSemAula Tud
			INNER JOIN @AlunosDisciplinasPendencias P
				ON P.tud_id = Tud.tud_id
				AND P.tpc_id = Tud.tpc_id

			-- Pegar disciplinas que não lançam nota e ver se elas tem aula (é obrigatório ter uma aula para não ter pendência).
			-- Verificação específica para Experiência (Território do Saber)
			; WITH DisciplinasNaoLancarNotaExp AS
			(
				SELECT
					tud.tud_id
					, Tud.tpc_id

					  -- Recupera a maior data entre a data inicial do período do calendário e a data inicial da experiência
					, CASE WHEN tte.tte_vigenciaInicio < cap.cap_dataInicio 
						THEN cap.cap_dataInicio
						ELSE tte.tte_vigenciaInicio
					  END AS VigenciaInicial

					  -- Recupera a menor data entre a data final do período do calendário e a data final da experiência
					, CASE WHEN tte.tte_vigenciaFim > cap.cap_dataFim
						THEN cap.cap_dataFim
						ELSE tte.tte_vigenciaFim
					  END AS VigenciaFinal
				FROM
					@AlunosDisciplinasPendencias tud
				INNER JOIN TUR_Turma tur WITH (NOLOCK)
					ON tur.tur_id = tud.tur_id
				INNER JOIN ACA_CalendarioPeriodo cap WITH (NOLOCK)
					ON cap.cal_id = tur.cal_id
					AND cap.tpc_id = tud.tpc_id
				INNER JOIN TUR_TurmaDisciplinaTerritorio tte WITH (NOLOCK)
					ON tte.tud_idExperiencia = tud.tud_id
					-- Apenas experiências ativas em cada período do calendário			
					AND cap.cap_dataFim >= tte.tte_vigenciaInicio
					AND cap.cap_dataInicio <= ISNULL(tte.tte_vigenciaFim, cap.cap_dataInicio)
					AND tte.tte_situacao <> 3
				WHERE 
					tud.tud_naoLancarNota = 1
					-- Apenas para Experiência (Território do Saber)
					AND tud.tud_tipo = 18
				GROUP BY
					Tud.tud_id
					, Tud.tpc_id
					, tte.tte_vigenciaInicio
					, tte.tte_vigenciaFim
					, cap.cap_dataInicio
					, cap.cap_dataFim
			)
			, DisciplinaSemAulaVigenciaExp AS
			(		
				SELECT 
					Tud.tud_id
					, Tud.tpc_id
						-- Verifica se existe aula criada para cada período de vigência de cada experiência dentro de cada período do calendário
					, 	ISNULL((
							SELECT TOP 1 1
							FROM CLS_TurmaAula Tau WITH(NOLOCK)
							WHERE
								Tau.tud_id = Tud.tud_id
								AND Tau.tpc_id = Tud.tpc_id
								AND Tau.tau_data BETWEEN Tud.VigenciaInicial and Tud.VigenciaFinal
								AND Tau.tau_situacao <> 3
						),0) AS AulaCriada
				FROM 
					DisciplinasNaoLancarNotaExp Tud
			)
			, DisciplinasSemAulaExp AS
			(
				-- Retorna apenas as disciplinas que não possuem aula criada em nenhum dos períodos de vigência da experiência dentro de cada período do calendário
				SELECT 
					Tud.tud_id
					, Tud.tpc_id
				FROM
					DisciplinaSemAulaVigenciaExp Tud
				GROUP BY
					Tud.tud_id
					, Tud.tpc_id
				HAVING 
					SUM(Tud.AulaCriada) = 0
			)
			UPDATE @AlunosDisciplinasPendencias
			SET DisciplinaSemAula = 1
			FROM DisciplinasSemAulaExp Tud
			INNER JOIN @AlunosDisciplinasPendencias P
				ON P.tud_id = Tud.tud_id
				AND P.tpc_id = Tud.tpc_id

			--select * from @AlunosDisciplinasPendencias

			; WITH JustificativasRecPar AS
			(
				SELECT 
					Tud.tud_id, Tud.tpc_id
				FROM @AlunosDisciplinasPendencias Tud
				INNER JOIN CLS_FechamentoJustificativaPendencia Fjp WITH(NOLOCK)
					ON Fjp.tud_id = Tud.tud_id
					AND Fjp.tpc_id = Tud.tpc_id
					AND Fjp.fjp_situacao <> 3
				WHERE
					Tud.tur_tipo = 2
			)

			UPDATE P
			SET P.DisciplinaSemAula = 0,
				P.SemNota = 0
			FROM JustificativasRecPar J
			INNER JOIN @AlunosDisciplinasPendencias P
				ON P.tud_id = J.tud_id
				AND P.tpc_id = J.tpc_id

			DECLARE @data DATETIME = GETDATE();

			UPDATE tsf
			SET Pendente = CAST(0 AS BIT),
				PendentePlanejamento = CAST(0 AS BIT),
				PendenteParecer = CAST(0 AS BIT),
				DataProcessamento = @data
			FROM
				REL_TurmaDisciplinaSituacaoFechamento tsf WITH(NOLOCK)
				INNER JOIN @filtroTurmaDisciplina T
					ON T.tud_id = tsf.tud_id
					AND T.tpc_id = tsf.tpc_id
	
			DECLARE @EscolasDados TABLE
			(uad_nome VARCHAR(200), esc_nome VARCHAR(200), tur_codigo VARCHAR(200), cal_ano INT, tpc_nome VARCHAR(200)
			, cal_id INT, uad_idSuperior UNIQUEIDENTIFIER, tur_id BIGINT, tud_id BIGINT, tpc_id INT, esc_id INT
			PRIMARY KEY (tur_id, tud_id, tpc_id))
			INSERT INTO @EscolasDados
			(uad_nome, esc_nome, tur_codigo, cal_ano, tpc_nome, cal_id, uad_idSuperior
			, tur_id, tud_id, tpc_id, esc_id)
			SELECT
				UadSuperior.uad_nome
				, Esc.esc_nome
				, EscTur.tur_codigo
				, EscTur.cal_ano
				, EscTur.tpc_nome
				, EscTur.cal_id
				, UadSuperior.uad_id
				, EscTur.tur_id
				, EscTur.tud_id
				, EscTur.tpc_id
				, EscTur.esc_id
			FROM @EscolasTurmasDisciplinasPeriodos EscTur
			INNER JOIN ESC_Escola Esc WITH(NOLOCK)
				ON Esc.esc_id = EscTur.esc_id
			INNER JOIN Synonym_SYS_UnidadeAdministrativa uad WITH(NOLOCK)
				ON esc.uad_id = uad.uad_id
			LEFT JOIN Synonym_SYS_UnidadeAdministrativa UadSuperior  WITH(NOLOCK)
				ON UadSuperior.ent_id = Esc.ent_id
				AND UadSuperior.uad_id = ISNULL(Esc.uad_idSuperiorGestao, uad.uad_idSuperior)

			--select * from @EscolasDados

			DECLARE @AlunoSituacaoFechamentoOrigem TABLE
			(
				DRE varchar(200) NULL,
				Escola varchar(200) NOT NULL,
				Ciclo varchar(200) NULL,
				Serie varchar(200) NOT NULL,
				Turma varchar(50) NOT NULL,
				AnoLetivo int NOT NULL,
				Bimestre varchar(200) NOT NULL,
				Aluno varchar(400) NOT NULL,
				Disciplina varchar(200) NOT NULL,
				uad_idSuperior uniqueidentifier NULL,
				esc_id int NOT NULL,
				tci_id int NOT NULL,
				cur_id int NOT NULL,
				crr_id int NOT NULL,
				crp_id int NOT NULL,
				tur_id bigint NOT NULL,
				cal_id int NOT NULL,
				tpc_id int NOT NULL,
				alu_id bigint NOT NULL,
				mtu_id int NOT NULL,
				mtd_id int NOT NULL,
				tud_id bigint NOT NULL,
				tud_tipo tinyint NOT NULL,
				SemNota bit NOT NULL,
				SemParecer bit NOT NULL,
				DisciplinaSemAula bit NOT NULL,
				DataRegistro datetime NOT NULL,
				SemSintese bit NOT NULL,
				SemResultadoFinal bit NOT NULL,
				PRIMARY KEY
				(
					esc_id,
					tur_id,
					cal_id,
					tpc_id,
					alu_id,
					mtu_id,
					mtd_id,
					tud_id
				),
				UNIQUE CLUSTERED
				(
					esc_id,
					tur_id,
					cal_id,
					tpc_id,
					alu_id,
					mtu_id,
					mtd_id,
					tud_id
				)
			)

			INSERT INTO @AlunoSituacaoFechamentoOrigem
			(
				DRE,
				Escola,
				Ciclo,
				Serie,
				Turma,
				AnoLetivo,
				Bimestre,
				Aluno,
				Disciplina,
				uad_idSuperior,
				esc_id,
				tci_id,
				cur_id,
				crr_id,
				crp_id,
				tur_id,
				cal_id,
				tpc_id,
				alu_id,
				mtu_id,
				mtd_id,
				tud_id,
				tud_tipo,
				SemNota,
				SemParecer,
				DisciplinaSemAula,
				DataRegistro,
				SemSintese,
				SemResultadoFinal
			)
			select
				EscTur.uad_nome AS DRE
				, EscTur.esc_nome AS Escola
				, Tci.tci_nome AS Ciclo
				, Crp.crp_descricao AS Serie
				, EscTur.tur_codigo AS Turma
				, EscTur.cal_ano AS AnoLetivo
				, EscTur.tpc_nome AS Bimestre
				, Pes.pes_nome AS Aluno
				, Tabela.tud_nome AS Disciplina
				, EscTur.uad_idSuperior
				, EscTur.esc_id
				, Tci.tci_id
				, Crp.cur_id
				, Crp.crr_id
				, Crp.crp_id
				, EscTur.tur_id
				, EscTur.cal_id AS cal_id
				, Tabela.tpc_id AS tpc_id
				, Alu.alu_id
				, Mtu.mtu_id
				, Tabela.mtd_id
				, Tabela.tud_id
				, Tabela.tud_tipo
				, Tabela.SemNota
				, Tabela.SemParecer
				, Tabela.DisciplinaSemAula
				, @data ASDataRegistro
				, Tabela.SemSintese
				, Tabela.SemResultadoFinal
			from @AlunosDisciplinasPendencias Tabela
			INNER JOIN @EscolasDados EscTur
				ON EscTur.tur_id = Tabela.tur_id
				AND EscTur.tud_id = Tabela.tud_id
				AND EscTur.tpc_id = Tabela.tpc_id
	
			INNER JOIN MTR_MatriculaTurma Mtu WITH(NOLOCK)
				ON Mtu.alu_id = Tabela.alu_id
				AND Mtu.mtu_id = Tabela.mtu_id
			INNER JOIN ACA_CurriculoPeriodo Crp WITH(NOLOCK)
				ON Crp.cur_id = Mtu.cur_id
				AND Crp.crr_id = Mtu.crr_id
				AND Crp.crp_id = Mtu.crp_id
			LEFT JOIN ACA_TipoCiclo Tci WITH(NOLOCK)
				ON Tci.tci_id = Crp.tci_id
			INNER JOIN ACA_Aluno Alu WITH(NOLOCK)
				ON Alu.alu_id = Mtu.alu_id
			INNER JOIN VW_DadosAlunoPessoa Pes
				ON Pes.alu_id = Alu.alu_id

			DELETE asf
			FROM REL_AlunosSituacaoFechamento asf WITH(NOLOCK)
			INNER JOIN @tbPendencias pen
				ON asf.tud_id = pen.tud_id
				AND asf.tpc_id = pen.tpc_id
			WHERE
				NOT EXISTS
				(
					SELECT TOP 1 1
					FROM @AlunoSituacaoFechamentoOrigem Origem
					WHERE asf.esc_id = Origem.esc_id 
					AND asf.tur_id = Origem.tur_id
					AND asf.cal_id = Origem.cal_id
					AND asf.tpc_id = Origem.tpc_id
					AND asf.alu_id = Origem.alu_id
					AND asf.mtu_id = Origem.mtu_id
					AND asf.mtd_id = Origem.mtd_id
					AND asf.tud_id = Origem.tud_id
				)

			MERGE REL_AlunosSituacaoFechamento WITH (HOLDLOCK) AS Destino
			USING @AlunoSituacaoFechamentoOrigem AS Origem
			ON
			(
				Destino.esc_id = Origem.esc_id 
				AND Destino.tur_id = Origem.tur_id
				AND Destino.cal_id = Origem.cal_id
				AND Destino.tpc_id = Origem.tpc_id
				AND Destino.alu_id = Origem.alu_id
				AND Destino.mtu_id = Origem.mtu_id
				AND Destino.mtd_id = Origem.mtd_id
				AND Destino.tud_id = Origem.tud_id
			)
			WHEN NOT MATCHED THEN
				INSERT
					([uad_nomeSuperior]
				   ,[esc_nome]
				   ,[tci_nome]
				   ,[crp_descricao]
				   ,[tur_codigo]
				   ,[cal_ano]
				   ,[tpc_nome]
				   ,[pes_nome]
				   ,[tud_nome]
				   ,[uad_idSuperior]
				   ,[esc_id]
				   ,[tci_id]
				   ,[cur_id]
				   ,[crr_id]
				   ,[crp_id]
				   ,[tur_id]
				   ,[cal_id]
				   ,[tpc_id]
				   ,[alu_id]
				   ,[mtu_id]
				   ,[mtd_id]
				   ,[tud_id]
				   ,[tud_tipo]
				   ,[SemNota]
				   ,[SemSintese]
				   ,[SemResultadoFinal]
				   ,[SemParecer]
				   ,[DisciplinaSemAula]
				   ,[DataRegistro])
				VALUES
				(Origem.DRE
				   ,Origem.Escola
				   ,Origem.Ciclo
				   ,Origem.Serie
				   ,Origem.Turma
				   ,Origem.AnoLetivo
				   ,Origem.Bimestre
				   ,Origem.Aluno
				   ,Origem.Disciplina
				   ,Origem.uad_idSuperior
				   ,Origem.esc_id
				   ,Origem.tci_id
				   ,Origem.cur_id
				   ,Origem.crr_id
				   ,Origem.crp_id
				   ,Origem.tur_id
				   ,Origem.cal_id
				   ,Origem.tpc_id
				   ,Origem.alu_id
				   ,Origem.mtu_id
				   ,Origem.mtd_id
				   ,Origem.tud_id
				   ,Origem.tud_tipo
				   ,Origem.SemNota
				   ,Origem.SemSintese
				   ,Origem.SemResultadoFinal
				   ,Origem.SemParecer
				   ,Origem.DisciplinaSemAula
				   ,Origem.DataRegistro)
			WHEN MATCHED THEN
				UPDATE SET 
					SemNota = Origem.SemNota,
					SemSintese = Origem.SemSintese,
					SemResultadoFinal = Origem.SemResultadoFinal,
					SemParecer = Origem.SemParecer,
					DisciplinaSemAula = Origem.DisciplinaSemAula,
					DataRegistro = Origem.DataRegistro;

			DECLARE @PendenciaTurmaDisciplina TABLE
			(
				tud_id BIGINT,
				tpc_id INT,
				esc_id INT,
				cal_id INT,
				Pendente BIT,
				PendentePlanejamento BIT,
				PendenteParecer BIT
				PRIMARY KEY (tud_id, tpc_id)
			);

			INSERT INTO @PendenciaTurmaDisciplina
			(
				tud_id,
				tpc_id,
				esc_id,
				cal_id,
				Pendente,
				PendentePlanejamento,
				PendenteParecer
			)
			SELECT
				pend.tud_id,
				pend.tpc_id,
				est.esc_id,
				est.cal_id,
				CAST(CASE WHEN 
						(SUM(CAST(pend.SemNota AS INT)) + 
						 SUM(CAST(pend.SemSintese AS INT)) +
						 SUM(CAST(pend.SemResultadoFinal AS INT)) +
						 --SUM(CAST(pend.SemParecer AS INT)) +
						 SUM(CAST(pend.DisciplinaSemAula AS INT))) > 0
							THEN 1
							ELSE 0 
					 END AS BIT) AS Pendente,
				CAST(CASE WHEN 
						SUM(CAST(pend.SemPlanejamento AS INT)) > 0
							THEN 1
							ELSE 0
					END AS BIT)AS PendentePlanejamento,
				CAST(CASE WHEN
						SUM(CAST(pend.SemParecer AS INT)) > 0
							THEN 1
							ELSE 0
					END AS BIT) AS PendenteParecer
			FROM 
				@AlunosDisciplinasPendencias pend
				INNER JOIN @EscolasTurmasDisciplinasPeriodos est
					ON est.tur_id = pend.tur_id
					AND est.tud_id = pend.tud_id
					AND est.tpc_id = pend.tpc_id
			GROUP BY 
				pend.tud_id,
				pend.tpc_id,
				est.esc_id,
				est.cal_id

			MERGE REL_TurmaDisciplinaSituacaoFechamento AS Destino
			USING @PendenciaTurmaDisciplina AS Origem
			ON Destino.tud_id = Origem.tud_id
			   AND Destino.tpc_id = Origem.tpc_id
			WHEN MATCHED THEN
				UPDATE SET Pendente = Origem.Pendente,
						   PendentePlanejamento = Origem.PendentePlanejamento,
						   PendenteParecer = Origem.PendenteParecer,
						   DataProcessamento = @data
			WHEN NOT MATCHED THEN
				INSERT (tud_id, tpc_id, esc_id, cal_id, Pendente, PendentePlanejamento, PendenteParecer, DataProcessamento)
				VALUES (Origem.tud_id, Origem.tpc_id, Origem.esc_id, Origem.cal_id, Origem.Pendente, Origem.PendentePlanejamento, Origem.PendenteParecer, @data);

			DELETE AFP 
			FROM 
				CLS_AlunoFechamentoPendencia as AFP WITH(NOLOCK)
				INNER JOIN @filtroTurmaDisciplina AS ftd
				ON AFP.tud_id = ftd.tud_id
				AND AFP.tpc_id = ftd.tpc_id
			WHERE
				AFP.afp_processado = 3;

		END
	END TRY
	BEGIN CATCH
		 -- Altera os registros afetados para "aguardando processamento".
		UPDATE CLS_AlunoFechamentoPendencia SET afp_processado = 2
		  FROM @tbPendencias tp 
			   INNER JOIN CLS_AlunoFechamentoPendencia P WITH(NOLOCK)
					   ON P.tud_id	 = tp.tud_id
					  AND p.tpc_id = tp.tpc_id
	END CATCH;
END

GO

PRINT N'Refreshing [dbo].[VW_DESESC_DISCIPLINA]'
GO
EXEC sp_refreshview N'[dbo].[VW_DESESC_DISCIPLINA]'
GO

PRINT N'Creating [dbo].[NEW_ACA_ObjetoAprendizagemCollection_SELECT_By_tud_id_cal_id]'
GO
-- ========================================================================
-- Author:		Myller Batista
-- Create date: 17/03/2017
-- Description:	Seleciona os objetos de aprendizagem ligados à disciplina e período do calendário
-- ========================================================================
CREATE PROCEDURE [dbo].[NEW_ACA_ObjetoAprendizagemCollection_SELECT_By_tud_id_cal_id]
	 @cal_id INT
	, @tud_id BIGINT
AS
BEGIN

	DECLARE @tpc_idRecesso INT = (SELECT TOP 1 CAST(pac_valor AS INT) FROM ACA_ParametroAcademico WITH(NOLOCK) 
								  WHERE pac_chave = 'TIPO_PERIODO_CALENDARIO_RECESSO' AND pac_situacao <> 3)

	DECLARE @tud_tipo TINYINT = (SELECT TOP 1 tud_tipo FROM TUR_TurmaDisciplina WITH(NOLOCK) WHERE tud_id = @tud_id)

	DECLARE @tud_ids TABLE (tud_id BIGINT)
	
	IF (@tud_tipo = 11)--Regencia
		INSERT INTO @tud_ids
		SELECT tudComp.tud_id
		FROM TUR_TurmaDisciplina tudReg WITH(NOLOCK)
		INNER JOIN TUR_TurmaRelTurmaDisciplina RelTud WITH(NOLOCK)
			ON RelTud.tud_id = tudReg.tud_id
		INNER JOIN TUR_TurmaRelTurmaDisciplina RelTudComp WITH(NOLOCK)
			ON RelTudComp.tur_id = RelTud.tur_id
		INNER JOIN dbo.TUR_TurmaDisciplina TudComp WITH (NOLOCK)
			ON TudComp.tud_id = RelTudComp.tud_id
			AND TudComp.tud_tipo = 12 --componente da regencia
		WHERE tudReg.tud_id = @tud_id
	ELSE
		INSERT INTO @tud_ids
		VALUES (@tud_id)

	SELECT 
		t.tud_id,
		oap.oap_id,
		CAST((CASE WHEN oat.oap_id IS NULL THEN 0
			ELSE 1 END) AS BIT) AS selecionado
		, oap.oap_descricao
		, oap.cal_ano
		, oap.oap_situacao
		, tpc.tpc_id
		, tpc.tpc_nome
		, tpc.tpc_ordem
	FROM
		ACA_CalendarioAnual cal WITH(NOLOCK)
	INNER JOIN ACA_CalendarioPeriodo cap WITH(NOLOCK)
		ON cal.cal_id = cap.cal_id
		AND cap.cap_situacao <> 3
	INNER JOIN ACA_TipoPeriodoCalendario tpc WITH(NOLOCK)
		ON cap.tpc_id = tpc.tpc_id
		AND tpc.tpc_id <> @tpc_idRecesso
		AND tpc.tpc_situacao <> 3	
	CROSS JOIN ACA_ObjetoAprendizagem oap WITH(NOLOCK)
	INNER JOIN ACA_Disciplina dis WITH(NOLOCK)
		ON oap.tds_id = dis.tds_id
		AND oap.cal_ano = cal.cal_ano
		AND dis.dis_situacao <> 3
	INNER JOIN TUR_TurmaDisciplinaRelDisciplina trd WITH(NOLOCK)
		ON dis.dis_id = trd.dis_id
	INNER JOIN @tud_ids t
		ON trd.tud_id = t.tud_id
	INNER JOIN TUR_TurmaRelTurmaDisciplina trt WITH(NOLOCK)
		ON trd.tud_id = trt.tud_id
	INNER JOIN TUR_TurmaCurriculo tcr WITH(NOLOCK)
		ON trt.tur_id = tcr.tur_id
		AND tcr.tcr_situacao <> 3
	INNER JOIN ACA_CurriculoPeriodo crp WITH(NOLOCK)
		ON tcr.cur_id = crp.cur_id
		AND tcr.crr_id = crp.crr_id
		AND tcr.crp_id = crp.crp_id
		AND crp.crp_situacao <> 3
	INNER JOIN ACA_ObjetoAprendizagemTipoCiclo otc WITH(NOLOCK)
		ON oap.oap_id = otc.oap_id
		AND crp.tci_id = otc.tci_id
	LEFT JOIN CLS_ObjetoAprendizagemTurmaDisciplina oat WITH(NOLOCK)
		ON oat.tpc_id = tpc.tpc_id
		AND oat.oap_id = oap.oap_id
		AND oat.tud_id = t.tud_id
	WHERE
		cal.cal_id = @cal_id
		AND oap.oap_situacao <> 3
	ORDER BY 
		t.tud_id
		, oap.oap_descricao
		, tpc.tpc_ordem
END
GO

PRINT N'Refreshing [dbo].[VW_DESESC_SERIE_DISCIPLINA]'
GO
EXEC sp_refreshview N'[dbo].[VW_DESESC_SERIE_DISCIPLINA]'
GO

PRINT N'Creating [dbo].[NEW_SYS_ServicosLogExecucao_SELECT_AtivoBy_servico]'
GO
-- =============================================
-- Author:		Marcia Haga
-- Create date: 17/03/2017
-- Description:	Verifica se existe um serviço rodando.
-- =============================================
CREATE PROCEDURE [dbo].[NEW_SYS_ServicosLogExecucao_SELECT_AtivoBy_servico]
	@ser_id SMALLINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT TOP 1 sle_id
	FROM SYS_ServicosLogExecucao WITH(NOLOCK)
	WHERE ser_id = @ser_id
	AND sle_dataFimExecucao IS NULL
END
GO

PRINT N'Refreshing [dbo].[VW_DESESC_ALUNO]'
GO
EXEC sp_refreshview N'[dbo].[VW_DESESC_ALUNO]'
GO

PRINT N'Creating [dbo].[NEW_CLS_AlunoPlanejamentoRelacionada_LimparRelacionadas_porLista]'
GO
-- =============================================
-- Author:		Cesar Henrique Marcusso
-- Create date: 24/09/2015
-- Description:	Remove todas turmadisciplinas relacionadas ao planejamento do aluno
--				que estão na lista
-- =============================================
CREATE PROCEDURE [dbo].[NEW_CLS_AlunoPlanejamentoRelacionada_LimparRelacionadas_porLista] 
	@alu_ids VARCHAR(MAX),
	@tud_ids VARCHAR(MAX),
	@apl_ids VARCHAR(MAX)
AS
BEGIN
		
    BEGIN TRANSACTION
    SET XACT_ABORT ON		

	;WITH tbAlunosPlanejamento AS (
			SELECT A.alu_id, T.tud_id, P.apl_id
			FROM (
				SELECT valor AS alu_id, ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS I
				FROM dbo.FN_StringToArrayInt64(@alu_ids,',')
			) AS A
			INNER JOIN (
				SELECT valor AS tud_id, ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS I
				FROM dbo.FN_StringToArrayInt64(@tud_ids,',')
			) AS T
			ON T.I = A.I
			INNER JOIN (
				SELECT valor AS apl_id, ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS I
				FROM dbo.FN_StringToArrayInt32(@apl_ids,',')
			) AS P
			ON T.I = P.I)

	DELETE FROM CLS_AlunoPlanejamentoRelacionada 
		   WHERE EXISTS(SELECT * FROM tbAlunosPlanejamento AS Tba
							     WHERE Tba.alu_id = CLS_AlunoPlanejamentoRelacionada.alu_id AND											
							     Tba.tud_id = CLS_AlunoPlanejamentoRelacionada.tud_id AND	
		   					     (Tba.apl_id <= 0 OR Tba.apl_id = CLS_AlunoPlanejamentoRelacionada.apl_id));

	-- Fechar transação
	SET XACT_ABORT OFF
	COMMIT TRANSACTION	

END
GO

PRINT N'Creating [dbo].[NEW_CLS_ObjetoAprendizagemTurmaDisciplina_DELETETudDis]'
GO
-- ========================================================================
-- Author:		Myller Batista
-- Create date: 20/03/2017
-- Description:	Deleta todos os relacionamentos da turma disciplina com objetos de aprendizagem
-- ========================================================================
CREATE PROCEDURE [dbo].[NEW_CLS_ObjetoAprendizagemTurmaDisciplina_DELETETudDis]
	@tud_id BIGINT
AS
BEGIN
    SET NOCOUNT ON ;

	DELETE FROM CLS_ObjetoAprendizagemTurmaDisciplina
	WHERE tud_id = @tud_id

END


GO

PRINT N'Altering [dbo].[NEW_ACA_TipoCiclo_SelecionarAtivos]'
GO

ALTER PROCEDURE [dbo].[NEW_ACA_TipoCiclo_SelecionarAtivos]
AS

-- ========================================================================
-- Author:		Webber V. dos Santos
-- Create date: 03/02/2014
-- Description:	Retorna os tipos de ciclo de aprendizagem ativos
-- ========================================================================

BEGIN

	SELECT	  
		 tci_id  
		, tci_nome
		, tci_situacao 
		, tci_dataCriacao 
		, tci_dataAlteracao 		
  	    , CASE tci_exibirBoletim
			WHEN 1 THEN
				'Sim'
			ELSE	
			    'Não'
		END AS tci_exibirBoletim
		, tci_ordem
		, tci_objetoAprendizagem
 	FROM
 		dbo.ACA_TipoCiclo WITH(NOLOCK) 		
	WHERE 
		tci_situacao = 1
	--ORDER BY tci_nome
		ORDER BY tci_ordem

END


GO

PRINT N'Creating [dbo].[NEW_ACA_TipoDisciplina_SelectBy_Pesquisa_SemRegencia]'
GO
-- ================================================================================
-- Author:		Myller Batista
-- Create date: 21/03/2017
-- Description:	Retorna os tipos de disciplina sem a Regência que não foram excluídos logicamente.
-- ================================================================================
CREATE PROCEDURE [dbo].[NEW_ACA_TipoDisciplina_SelectBy_Pesquisa_SemRegencia]	
	@tds_id INT
	, @tne_id INT
	, @tds_base TINYINT
	, @tds_idNaoConsiderar INT
	, @controlarOrdem BIT
AS
BEGIN
	SELECT 
		tds_id
		, tds_nome
		, tds_ordem
		, tne_nome
		, tne_nome + ' - ' + tds_nome as tne_tds_nome 		
		, CASE tds_situacao 
			WHEN 1 THEN 'Não'
			WHEN 2 THEN 'Sim'			
		  END AS tds_situacao
		, CASE tds_base 
			WHEN 1 THEN 'Nacional'
			WHEN 2 THEN 'Diversificada'			
		  END AS tds_base_nome
		, tds_base
	FROM
		ACA_TipoDisciplina WITH (NOLOCK)
	INNER JOIN ACA_TipoNivelEnsino WITH (NOLOCK)
		ON ACA_TipoNivelEnsino.tne_id = ACA_TipoDisciplina.tne_id
	WHERE
		tds_situacao <> 3
		AND tds_tipo <> 3
		AND tne_situacao <> 3
		AND (@tds_id IS NULL OR ACA_TipoDisciplina.tds_id = @tds_id)
		AND (@tne_id IS NULL OR ACA_TipoDisciplina.tne_id = @tne_id)
		AND (@tds_base IS NULL OR ACA_TipoDisciplina.tds_base = @tds_base)
		AND (@tds_idNaoConsiderar IS NULL OR ACA_TipoDisciplina.tds_id <> @tds_idNaoConsiderar) 
	ORDER BY 		
	 CASE WHEN @controlarOrdem = 1 THEN tds_ordem END
	,CASE WHEN @controlarOrdem = 0 THEN tne_nome + ' - ' + tds_nome END 
			
	SELECT @@ROWCOUNT			
END

GO

PRINT N'Creating [dbo].[NEW_ACA_ObjetoAprendizagem_SELECTEmUsoBy_oap_id]'
GO
-- ========================================================================
-- Author:		Leonardo Brito
-- Create date: 22/03/2017
-- Description:	Verifica se o objeto de aprendizagem está em uso
-- ========================================================================
CREATE PROCEDURE [dbo].[NEW_ACA_ObjetoAprendizagem_SELECTEmUsoBy_oap_id]
	@oap_id Int
AS
BEGIN

	SELECT 
		oat.tud_id
		, oat.tpc_id
	FROM CLS_ObjetoAprendizagemTurmaDisciplina oat WITH(NOLOCK)
	INNER JOIN TUR_TurmaDisciplina tud WITH(NOLOCK)
		ON oat.tud_id = tud.tud_id
		AND tud.tud_situacao <> 3
	INNER JOIN TUR_TurmaRelTurmaDisciplina trt WITH(NOLOCK)
		ON tud.tud_id = trt.tud_id
	INNER JOIN TUR_Turma tur WITH(NOLOCK)
		ON trt.tur_id = tur.tur_id
		AND tur.tur_situacao <> 3
	WHERE 
		oap_id = @oap_id
	
END


GO

PRINT N'Refreshing [dbo].[VW_Alunos_Ativos]'
GO
EXEC sp_refreshview N'[dbo].[VW_Alunos_Ativos]'
GO

PRINT N'Creating [dbo].[NEW_ACA_ObjetoAprendizagemTipoCiclo_SelectEmUsoBy_Oap_Id]'
GO
-- ========================================================================
-- Author:		Leonardo Brito
-- Create date: 23/03/2017
-- Description:	Verifica se os ciclos do objeto de aprendizagem estão em uso
-- ========================================================================
CREATE PROCEDURE [dbo].[NEW_ACA_ObjetoAprendizagemTipoCiclo_SelectEmUsoBy_Oap_Id]
	@oap_id Int
AS
BEGIN

	SELECT 
		crp.tci_id,
		tci.tci_nome
	FROM CLS_ObjetoAprendizagemTurmaDisciplina oat WITH(NOLOCK)
	INNER JOIN TUR_TurmaDisciplina tud WITH(NOLOCK)
		ON oat.tud_id = tud.tud_id
		AND tud.tud_situacao <> 3
	INNER JOIN TUR_TurmaRelTurmaDisciplina trt WITH(NOLOCK)
		ON tud.tud_id = trt.tud_id
	INNER JOIN TUR_Turma tur WITH(NOLOCK)
		ON trt.tur_id = tur.tur_id
		AND tur.tur_situacao <> 3
	INNER JOIN TUR_TurmaCurriculo tcr WITH(NOLOCK)
		ON tur.tur_id = tcr.tur_id
		AND tcr.tcr_situacao <> 3
	INNER JOIN ACA_CurriculoPeriodo crp WITH(NOLOCK)
		ON tcr.cur_id = crp.cur_id
		AND tcr.crr_id = crp.crr_id
		AND tcr.crp_id = crp.crp_id
		AND crp.crp_situacao <> 3
	INNER JOIN ACA_TipoCiclo tci WITH(NOLOCK)
		ON crp.tci_id = tci.tci_id
		AND tci.tci_situacao <> 3
	WHERE 
		oap_id = @oap_id
	GROUP BY
		crp.tci_id,
		tci.tci_nome
	
END


GO

PRINT N'Altering [dbo].[STP_ACA_TipoCiclo_LOAD]'
GO

ALTER PROCEDURE [dbo].[STP_ACA_TipoCiclo_LOAD]
	@tci_id Int
	
AS
BEGIN
	SELECT	Top 1
		 tci_id  
		, tci_nome 
		, tci_situacao 
		, tci_dataCriacao 
		, tci_dataAlteracao 
		, tci_exibirBoletim 
		, tci_ordem 
		, tci_layout 
		, tci_objetoAprendizagem
 	FROM
 		ACA_TipoCiclo
	WHERE 
		tci_id = @tci_id
END

GO

PRINT N'Altering [dbo].[REL_DivergenciaRematriculas]'
GO

ALTER TABLE [dbo].[REL_DivergenciaRematriculas] ADD
[mtu_idFinal] [int] NULL
GO

PRINT N'Altering [dbo].[NEW_Relatorio_DivergenciasRematriculas]'
GO
-- =============================================
-- Author:		Leonardo Brito
-- Create date: 29/11/2016
-- Description:	Seleciona os alunos com divergências nas rematrículas
-- =============================================
ALTER PROCEDURE [dbo].[NEW_Relatorio_DivergenciasRematriculas]	
	@uad_idSuperior UNIQUEIDENTIFIER,
    @uni_id INT,
    @esc_id INT,
    @cal_id INT,    
    @cur_id INT,
    @crr_id INT,
    @crp_id INT,
    @tur_id BIGINT,
	@mostraCodigoEscola BIT,
	@ent_id UNIQUEIDENTIFIER,
	@adm BIT,
	@usu_id UNIQUEIDENTIFIER,
	@gru_id UNIQUEIDENTIFIER
		
AS
BEGIN

	DECLARE	@UAs TABLE (uad_id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY);
	IF (@adm <> 1)
	BEGIN
		INSERT INTO @UAs (uad_id)
		SELECT uad_id from Synonym_FN_Select_UAs_By_PermissaoUsuario(@usu_id, @gru_id)
		GROUP BY uad_id
	END
	
	DECLARE @ser_id INT = 51

	DECLARE @ultimaExecucao DATETIME
	IF (@ser_id IS NOT NULL)
		SELECT @ultimaExecucao = MAX(sle_dataFimExecucao) FROM SYS_ServicosLogExecucao WITH(NOLOCK) 
		WHERE ser_id = @ser_id AND sle_dataFimExecucao IS NOT NULL

	;WITH dados AS (
	SELECT
		alu.alu_id,
		pes.pes_nome AS nomeAluno,
		escAtual.esc_id,
		CASE @mostraCodigoEscola
			WHEN 1 THEN ISNULL(escAtual.esc_codigo + ' - ', '')
			ELSE ''
		END + escAtual.esc_nome AS esc_nome,
		crpAnterior.cur_id AS cur_idAnterior,
		crpAnterior.crr_id AS crr_idAnterior,
		crpAnterior.crp_id AS crp_idAnterior,
		crpAnterior.crp_descricao AS crp_descricaoAnterior,
		turAnterior.tur_id AS tur_idAnterior,
		turAnterior.tur_codigo AS tur_codigoAnterior,
		CASE WHEN mtuAnterior.mtu_resultado IS NULL THEN '-'
			 WHEN mtuAnterior.mtu_resultado = 1 THEN 'Aprovado'
			 WHEN mtuAnterior.mtu_resultado = 2 THEN 'Reprovado'
			 WHEN mtuAnterior.mtu_resultado = 8 THEN 'Reprovado por frequência'
			 WHEN mtuAnterior.mtu_resultado = 9 THEN 'Recuperação final'
			 WHEN mtuAnterior.mtu_resultado = 10 THEN 'Aprovado conselho'
			 ELSE '-' END AS resultadoAnterior,
		crpAtual.cur_id AS cur_idAtual,
		crpAtual.crr_id AS crr_idAtual,
		crpAtual.crp_id AS crp_idAtual,
		crpAtual.crp_descricao AS crp_descricaoAtual,
		turAtual.tur_id AS tur_idAtual,
		turAtual.tur_codigo AS tur_codigoAtual,
		crpFinal.cur_id AS cur_idFinal,
		crpFinal.crr_id AS crr_idFinal,
		crpFinal.crp_id AS crp_idFinal,
		crpFinal.crp_descricao AS crp_descricaoFinal,
		turFinal.tur_id AS tur_idFinal,
		turFinal.tur_codigo AS tur_codigoFinal,
		rdr.DataProcessamento,
		CASE WHEN Inconsistencia = 1 THEN 'Aluno aprovado no SGP e matriculado no mesmo ano no EOL'
			 WHEN Inconsistencia = 2 THEN 'Aluno reprovado no SGP e matriculado no próximo ano no EOL'
			 WHEN Inconsistencia = 3 THEN 'Aluno sem resultado no SGP e matriculado no EOL'
			 ELSE ''
		END AS Inconsistencia
	FROM
		REL_DivergenciaRematriculas rdr WITH(NOLOCK)
	INNER JOIN MTR_MatriculaTurma mtuAtual WITH(NOLOCK)
		ON rdr.alu_id = mtuAtual.alu_id
		AND rdr.mtu_idAtual = mtuAtual.mtu_id
		AND mtuAtual.mtu_situacao <> 3
	INNER JOIN TUR_Turma turAtual WITH(NOLOCK)
		ON mtuAtual.tur_id = turAtual.tur_id
		AND turAtual.tur_id = ISNULL(@tur_id, turAtual.tur_id)
		AND turAtual.cal_id = ISNULL(@cal_id, turAtual.cal_id)
		AND turAtual.esc_id = ISNULL(@esc_id, turAtual.esc_id)
		AND turAtual.uni_id = ISNULL(@uni_id, turAtual.uni_id)
		AND turAtual.tur_situacao <> 3
	INNER JOIN ESC_Escola escAtual WITH(NOLOCK)
		ON turAtual.esc_id = escAtual.esc_id
		AND escAtual.esc_situacao <> 3
	INNER JOIN Synonym_SYS_UnidadeAdministrativa uadAtual WITH(NOLOCK)
		ON escAtual.uad_id = uadAtual.uad_id
		AND (ISNULL(escAtual.uad_idSuperiorGestao, uadAtual.uad_idSuperior) = COALESCE(@uad_idSuperior, escAtual.uad_idSuperiorGestao, uadAtual.uad_idSuperior))
		AND uadAtual.uad_situacao <> 3
	INNER JOIN ACA_AlunoCurriculo alcAtual WITH(NOLOCK)
		ON mtuAtual.alu_id = alcAtual.alu_id
		AND mtuAtual.alc_id = alcAtual.alc_id
		AND alcAtual.cur_id = ISNULL(@cur_id, alcAtual.cur_id)
		AND alcAtual.crr_id = ISNULL(@crr_id, alcAtual.crr_id)
		AND alcAtual.crp_id = ISNULL(@crp_id, alcAtual.crp_id)
		AND alcAtual.alc_situacao <> 3
	INNER JOIN ACA_Curriculoperiodo crpAtual WITH(NOLOCK)
		ON alcAtual.cur_id = crpAtual.cur_id
		AND alcAtual.crr_id = crpAtual.crr_id
		AND alcAtual.crp_id = crpAtual.crp_id
		AND crpAtual.crp_situacao <> 3
	INNER JOIN ACA_Aluno alu WITH(NOLOCK)
		ON rdr.alu_id = alu.alu_id
		AND alu.ent_id = @ent_id
		AND alu.alu_situacao <> 3
	INNER JOIN VW_DadosAlunoPessoa pes
		ON alu.alu_id = pes.alu_id
	INNER JOIN MTR_MatriculaTurma mtuAnterior WITH(NOLOCK)
		ON rdr.alu_id = mtuAnterior.alu_id
		AND rdr.mtu_idAnterior = mtuAnterior.mtu_id
		AND mtuAnterior.mtu_situacao <> 3
	INNER JOIN TUR_Turma turAnterior WITH(NOLOCK)
		ON mtuAnterior.tur_id = turAnterior.tur_id
		AND turAnterior.tur_situacao <> 3
	INNER JOIN ESC_Escola escAnterior WITH(NOLOCK)
		ON turAnterior.esc_id = escAnterior.esc_id
		AND escAnterior.esc_situacao <> 3
	INNER JOIN ACA_AlunoCurriculo alcAnterior WITH(NOLOCK)
		ON mtuAnterior.alu_id = alcAnterior.alu_id
		AND mtuAnterior.alc_id = alcAnterior.alc_id
		AND alcAnterior.alc_situacao <> 3
	INNER JOIN ACA_Curriculoperiodo crpAnterior WITH(NOLOCK)
		ON alcAnterior.cur_id = crpAnterior.cur_id
		AND alcAnterior.crr_id = crpAnterior.crr_id
		AND alcAnterior.crp_id = crpAnterior.crp_id
		AND crpAnterior.crp_situacao <> 3
	INNER JOIN MTR_MatriculaTurma mtuFinal WITH(NOLOCK)
		ON rdr.alu_id = mtuFinal.alu_id
		AND rdr.mtu_idFinal = mtuFinal.mtu_id
		AND mtuFinal.mtu_situacao <> 3
	INNER JOIN TUR_Turma turFinal WITH(NOLOCK)
		ON mtuFinal.tur_id = turFinal.tur_id
		AND turFinal.tur_situacao <> 3
	INNER JOIN ACA_AlunoCurriculo alcFinal WITH(NOLOCK)
		ON mtuFinal.alu_id = alcFinal.alu_id
		AND mtuFinal.alc_id = alcFinal.alc_id
		AND alcFinal.alc_situacao <> 3
	INNER JOIN ACA_Curriculoperiodo crpFinal WITH(NOLOCK)
		ON alcFinal.cur_id = crpFinal.cur_id
		AND alcFinal.crr_id = crpFinal.crr_id
		AND alcFinal.crp_id = crpFinal.crp_id
		AND crpFinal.crp_situacao <> 3
	WHERE
		(@adm = 1 OR EXISTS(SELECT TOP 1 u.uad_id FROM @UAs u WHERE escAnterior.uad_id = u.uad_id) OR 
					 EXISTS(SELECT TOP 1 u.uad_id FROM @UAs u WHERE escAtual.uad_id = u.uad_id))
	)

	SELECT
		alu_id,
		nomeAluno,
		esc_id,
		esc_nome,
		cur_idAnterior,
		crr_idAnterior,
		crp_idAnterior,
		crp_descricaoAnterior,
		tur_idAnterior,
		tur_codigoAnterior,
		resultadoAnterior,
		cur_idAtual,
		crr_idAtual,
		crp_idAtual,
		crp_descricaoAtual,
		tur_idAtual,
		tur_codigoAtual,
		cur_idFinal,
		crr_idFinal,
		crp_idFinal,
		crp_descricaoFinal,
		tur_idFinal,
		tur_codigoFinal,
		CASE WHEN @ultimaExecucao IS NULL THEN '' 
			 ELSE CONVERT(VARCHAR(10), @ultimaExecucao, 103) + ' - ' + 
				  CONVERT(VARCHAR(10), @ultimaExecucao, 108) 
		END AS ultimaDataProcessamento,
		Inconsistencia
	FROM dados
	ORDER BY
		esc_nome,
		nomeAluno
	
END

GO

PRINT N'Refreshing [dbo].[VW_DESESC_ALUNO_TURMA]'
GO
EXEC sp_refreshview N'[dbo].[VW_DESESC_ALUNO_TURMA]'
GO

PRINT N'Altering [dbo].[MS_JOB_ProcessamentoDivergenciasRematriculas]'
GO

-- =============================================
-- Author:		Leonardo Brito
-- Create date: 29/11/2016
-- Description: Processa as divergências nas rematrículas conforme o resultado do ano anterior
-- =============================================
ALTER PROCEDURE [dbo].[MS_JOB_ProcessamentoDivergenciasRematriculas]
AS
BEGIN
	
	DECLARE @ser_id INT = 51

	DECLARE @ultimaExecucao DATETIME
	IF (@ser_id IS NOT NULL)
		SELECT @ultimaExecucao = MAX(sle_dataFimExecucao) FROM SYS_ServicosLogExecucao WITH(NOLOCK) 
		WHERE ser_id = @ser_id AND sle_dataFimExecucao IS NOT NULL

	DECLARE @resultadoMatricula TABLE (alu_id BIGINT, mtu_idAnterior INT, mtu_idAtual INT, mtu_idFinal INT, 
									   crp_ordemAnterior INT, crp_ordemAtual INT, resultado TINYINT)

	DECLARE @tme_idsRemover TABLE (tme_id INT)
	DECLARE @tme_ids VARCHAR(250)
	
	SELECT @tme_ids = pac_valor 
	FROM dbo.ACA_ParametroAcademico WITH(NOLOCK) 
	WHERE pac_chave = 'TIPO_MODALIDADES_EJA_REMOVER_RELATORIO';
	
	INSERT INTO @tme_idsRemover
	SELECT valor FROM FN_StringToArrayInt32(@tme_ids, ',')

	;WITH DadosAlunos AS (
		SELECT
			mtuAnterior.alu_id,
			calAnterior.cal_ano AS cal_anoAnterior,
			tpcAnterior.tpc_ordem AS tpc_ordemAnterior,
			mtuAnterior.mtu_id AS mtu_idAnterior,
			mtuAnterior.mtu_dataMatricula AS mtu_dataMatriculaAnterior,
			(tneAnterior.tne_ordem * 10) + crpAnterior.crp_ordem AS crp_ordemAnterior,
			mtuAnterior.mtu_resultado,
			calAtual.cal_ano AS cal_anoAtual,
			tpcAtual.tpc_ordem AS tpc_ordemAtual,
			mtuAtual.mtu_id AS mtu_idAtual,
			(tneAtual.tne_ordem * 10) + crpAtual.crp_ordem AS crp_ordemAtual,
			movAnterior.tmo_id AS tmo_idAnterior,
			movAtual.tmo_id AS tmo_idAtual
		FROM --Dados da matrícula 
			MTR_MatriculaTurma mtuAnterior WITH(NOLOCK)
		INNER JOIN ACA_AlunoCurriculo alcAnterior WITH(NOLOCK)
			ON mtuAnterior.alu_id = alcAnterior.alu_id
			AND mtuAnterior.alc_id = alcAnterior.alc_id
			AND alcAnterior.alc_situacao <> 3
		INNER JOIN ACA_Curso curAnterior WITH(NOLOCK)
			ON alcAnterior.cur_id = curAnterior.cur_id
			AND NOT EXISTS(SELECT TOP 1 t.tme_id FROM @tme_idsRemover t WHERE curAnterior.tme_id = t.tme_id)
			AND curAnterior.cur_situacao <> 3
		INNER JOIN ACA_TipoNivelEnsino tneAnterior WITH(NOLOCK)
			ON curAnterior.tne_id = tneAnterior.tne_id
			AND tneAnterior.tne_situacao <> 3
		INNER JOIN ACA_CurriculoPeriodo crpAnterior WITH(NOLOCK)
			ON alcAnterior.cur_id = crpAnterior.cur_id
			AND alcAnterior.crr_id = crpAnterior.crr_id
			AND alcAnterior.crp_id = crpAnterior.crp_id
			AND crpAnterior.crp_situacao <> 3
		INNER JOIN TUR_Turma turAnterior WITH(NOLOCK)
			ON mtuAnterior.tur_id = turAnterior.tur_id
			AND turAnterior.tur_situacao <> 3
		INNER JOIN ACA_CalendarioAnual calAnterior WITH(NOLOCK)
			ON turAnterior.cal_id = calAnterior.cal_id
			AND calAnterior.cal_ano > 2014 --não pega dados de antes de 2014
			AND calAnterior.cal_situacao <> 3
		INNER JOIN ACA_CalendarioPeriodo capAnterior WITH(NOLOCK)
			ON calAnterior.cal_id = capAnterior.cal_id
			AND capAnterior.cap_dataInicio <= ISNULL(mtuAnterior.mtu_dataSaida, capAnterior.cap_dataInicio)
			AND capAnterior.cap_dataFim >= mtuAnterior.mtu_dataMatricula
			AND capAnterior.cap_situacao <> 3
		INNER JOIN ACA_TipoPeriodoCalendario tpcAnterior WITH(NOLOCK)
			ON capAnterior.tpc_id = tpcAnterior.tpc_id
			AND tpcAnterior.tpc_situacao <> 3
		INNER JOIN MTR_Movimentacao AS movAnterior
			ON movAnterior.alu_id = mtuAnterior.alu_id
			AND movAnterior.mtu_idAnterior = mtuAnterior.mtu_id
			AND movAnterior.alc_idAnterior = alcAnterior.alc_id

		--Dados do ano seguinte ao da matrícula
		INNER JOIN MTR_MatriculaTurma mtuAtual WITH(NOLOCK)
			ON mtuAnterior.alu_id = mtuAtual.alu_id
			AND mtuAtual.mtu_id <> mtuAnterior.mtu_id
			AND mtuAtual.mtu_situacao <> 3
		INNER JOIN TUR_Turma turAtual WITH(NOLOCK)
			ON mtuAtual.tur_id = turAtual.tur_id
			AND turAtual.tur_situacao <> 3
		INNER JOIN ACA_CalendarioAnual calAtual WITH(NOLOCK)
			ON turAtual.cal_id = calAtual.cal_id
			AND calAnterior.cal_ano = calAtual.cal_ano - 1
			AND calAtual.cal_situacao <> 3
		INNER JOIN ACA_AlunoCurriculo alcAtual WITH(NOLOCK)
			ON mtuAtual.alu_id = alcAtual.alu_id
			AND mtuAtual.alc_id = alcAtual.alc_id
			AND alcAtual.alc_situacao <> 3
		INNER JOIN ACA_Curso curAtual WITH(NOLOCK)
			ON alcAtual.cur_id = curAtual.cur_id
			AND NOT EXISTS(SELECT TOP 1 t.tme_id FROM @tme_idsRemover t WHERE curAtual.tme_id = t.tme_id)
			AND curAtual.cur_situacao <> 3
		INNER JOIN ACA_TipoNivelEnsino tneAtual WITH(NOLOCK)
			ON curAtual.tne_id = tneAtual.tne_id
			AND tneAtual.tne_situacao <> 3
		INNER JOIN ACA_CurriculoPeriodo crpAtual WITH(NOLOCK)
			ON alcAtual.cur_id = crpAtual.cur_id
			AND alcAtual.crr_id = crpAtual.crr_id
			AND alcAtual.crp_id = crpAtual.crp_id
			AND crpAtual.crp_situacao <> 3
		INNER JOIN ACA_CalendarioPeriodo capAtual WITH(NOLOCK)
			ON calAtual.cal_id = capAtual.cal_id
			AND capAtual.cap_dataInicio <= ISNULL(mtuAtual.mtu_dataSaida, capAtual.cap_dataInicio)
			AND capAtual.cap_dataFim >= mtuAtual.mtu_dataMatricula
			AND capAtual.cap_situacao <> 3
		INNER JOIN ACA_TipoPeriodoCalendario tpcAtual WITH(NOLOCK)
			ON capAtual.tpc_id = tpcAtual.tpc_id
			AND tpcAtual.tpc_situacao <> 3
		INNER JOIN MTR_Movimentacao AS movAtual
			ON movAtual.alu_id = mtuAtual.alu_id
			AND movAtual.mtu_idAtual = mtuAtual.mtu_id
			AND movAtual.alc_idAtual = alcAtual.alc_id
			
		WHERE --Seleciona apenas os dados que foram alterados (anterior OU atual) depois da última execução do serviço
			(
			@ultimaExecucao IS NULL OR
			mtuAtual.mtu_dataAlteracao >= @ultimaExecucao OR 
			mtuAnterior.mtu_dataAlteracao >= @ultimaExecucao
			)
			AND mtuAnterior.mtu_situacao <> 3
		GROUP BY
			mtuAnterior.alu_id,
			calAnterior.cal_ano,
			tpcAnterior.tpc_ordem,
			mtuAnterior.mtu_id,
			mtuAnterior.mtu_dataMatricula,
			tneAnterior.tne_ordem,
			crpAnterior.crp_ordem,
			mtuAnterior.mtu_resultado,
			calAtual.cal_ano,
			tpcAtual.tpc_ordem,
			mtuAtual.mtu_id,
			tneAtual.tne_ordem,
			crpAtual.crp_ordem,
			movAnterior.tmo_id,
			movAtual.tmo_id
	)
	--Seleciona o último período da matrícula anterior do aluno no ano letivo
	, ultimoPeriodoAnterior AS (
		SELECT
			d.alu_id,
			d.cal_anoAnterior,
			MAX(d.tpc_ordemAnterior) AS tpc_ordemMaxAnterior,
			MAX(d.mtu_dataMatriculaAnterior) AS mtu_dataMatriculaAnterior
		FROM 
			DadosAlunos d
		GROUP BY
			d.alu_id,
			d.cal_anoAnterior
	)
	--Seleciona o último período da matrícula atual do aluno no ano letivo
	, ultimoPeriodoAtual AS (
		SELECT
			d.alu_id,
			d.cal_anoAtual,
			MAX(d.tpc_ordemAtual) AS tpc_ordemMaxAtual
		FROM 
			DadosAlunos d
		GROUP BY
			d.alu_id,
			d.cal_anoAtual
	)
	--Seleciona o primeiro período da matrícula atual do aluno no ano letivo
	, primeiroPeriodoAtual AS (
		SELECT
			d.alu_id,
			d.cal_anoAtual,
			MIN(d.tpc_ordemAtual) AS tpc_ordemMinAtual
		FROM 
			DadosAlunos d
		GROUP BY
			d.alu_id,
			d.cal_anoAtual
	)

	INSERT INTO @resultadoMatricula (alu_id, mtu_idAnterior, crp_ordemAnterior, resultado, mtu_idAtual, mtu_idFinal, crp_ordemAtual)

	SELECT
		d.alu_id,
		d.mtu_idAnterior,
		d.crp_ordemAnterior,
		d.mtu_resultado,
		d.mtu_idAtual,
		(SELECT TOP 1 d2.mtu_idAtual
		 FROM DadosAlunos d2
		 INNER JOIN ultimoPeriodoAtual upFinal
			ON d2.alu_id = upFinal.alu_id
			AND d2.cal_anoAtual = upFinal.cal_anoAtual
			AND d2.tpc_ordemAtual = upFinal.tpc_ordemMaxAtual
		 WHERE d2.alu_id = d.alu_id AND d2.mtu_idAnterior = d.mtu_idAnterior) AS mtu_idFinal,
		d.crp_ordemAtual
	FROM 
		DadosAlunos d
	INNER JOIN ultimoPeriodoAnterior upAnterior
		ON d.alu_id = upAnterior.alu_id
		AND d.cal_anoAnterior = upAnterior.cal_anoAnterior
		AND d.tpc_ordemAnterior = upAnterior.tpc_ordemMaxAnterior
		AND d.mtu_dataMatriculaAnterior = upAnterior.mtu_dataMatriculaAnterior
		AND d.tmo_idAnterior NOT IN (8,9,10,11)
	INNER JOIN primeiroPeriodoAtual upAtual
		ON d.alu_id = upAtual.alu_id
		AND d.cal_anoAtual = upAtual.cal_anoAtual
		AND d.tpc_ordemAtual = upAtual.tpc_ordemMinAtual
		AND d.tmo_idAtual NOT IN (3,8,9,10,11)
	GROUP BY
		d.alu_id,
		d.mtu_idAnterior,
		d.crp_ordemAnterior,
		d.mtu_resultado,
		d.mtu_idAtual,
		d.crp_ordemAtual

	DECLARE @dataExecucao DATETIME = GETDATE()

	MERGE INTO REL_DivergenciaRematriculas Destino
	USING (SELECT alu_id, mtu_idAnterior, crp_ordemAnterior, resultado, mtu_idAtual, mtu_idFinal, crp_ordemAtual 
		   FROM @resultadoMatricula
		   WHERE resultado IS NOT NULL AND 
				 (
				  (resultado IN (1,10) AND crp_ordemAtual > crp_ordemAnterior) OR
				  (resultado IN (2,8,9) AND crp_ordemAtual = crp_ordemAnterior)
				 )) Origem
	ON Destino.alu_id = Origem.alu_id
	AND Destino.mtu_idAnterior = Origem.mtu_idAnterior
	WHEN MATCHED THEN
		DELETE;
		
	DELETE FROM @resultadoMatricula
	WHERE resultado IS NOT NULL AND (
		  (resultado IN (1,10) AND crp_ordemAtual > crp_ordemAnterior) OR
		  (resultado IN (2,8,9) AND crp_ordemAtual = crp_ordemAnterior))
	
	--SELECT * FROM @resultadoMatricula AS RM

	MERGE INTO REL_DivergenciaRematriculas Destino
	USING (SELECT alu_id, mtu_idAnterior, crp_ordemAnterior, resultado, mtu_idAtual, mtu_idFinal, crp_ordemAtual 
		   FROM @resultadoMatricula) Origem
	ON Destino.alu_id = Origem.alu_id
	AND Destino.mtu_idAnterior = Origem.mtu_idAnterior
	WHEN MATCHED THEN
		UPDATE SET mtu_idAtual = Origem.mtu_idAtual,
				   DataProcessamento = @dataExecucao
	WHEN NOT MATCHED THEN
		INSERT (alu_id, mtu_idAnterior, mtu_idAtual, mtu_idFinal, Inconsistencia, DataProcessamento)
		VALUES (Origem.alu_id, Origem.mtu_idAnterior, Origem.mtu_idAtual, Origem.mtu_idFinal,
				CASE WHEN resultado IN (1,10) AND crp_ordemAtual <= crp_ordemAnterior
					 THEN 1 --Alunos aprovados no SGP e matriculados no mesmo ano no EOL
					 WHEN resultado IN (2,8,9) AND crp_ordemAtual <> crp_ordemAnterior
					 THEN 2 --Alunos reprovados no SGP e matriculados no próximo ano no EOL
					 WHEN resultado IS NULL
					 THEN 3 --Alunos sem resultado no SGP e matriculados no EOL
					 ELSE 3
				END, 
				@dataExecucao);

END

GO

PRINT N'Adding foreign keys to [dbo].[ACA_ObjetoAprendizagemTipoCiclo]'
GO
ALTER TABLE [dbo].[ACA_ObjetoAprendizagemTipoCiclo] ADD CONSTRAINT [FK_ACA_ObjetoAprendizagemCicloPeriodo_ACA_ObjetoAprendizagem] FOREIGN KEY ([oap_id]) REFERENCES [dbo].[ACA_ObjetoAprendizagem] ([oap_id])
GO
ALTER TABLE [dbo].[ACA_ObjetoAprendizagemTipoCiclo] ADD CONSTRAINT [FK_ACA_ObjetoAprendizagemCicloPeriodo_ACA_TipoCiclo] FOREIGN KEY ([tci_id]) REFERENCES [dbo].[ACA_TipoCiclo] ([tci_id])
GO

PRINT N'Adding foreign keys to [dbo].[CLS_ObjetoAprendizagemTurmaAula]'
GO
ALTER TABLE [dbo].[CLS_ObjetoAprendizagemTurmaAula] ADD CONSTRAINT [FK_CLS_ObjetoAprendizagemTurmaAula_ACA_ObjetoAprendizagem] FOREIGN KEY ([oap_id]) REFERENCES [dbo].[ACA_ObjetoAprendizagem] ([oap_id])
GO
ALTER TABLE [dbo].[CLS_ObjetoAprendizagemTurmaAula] ADD CONSTRAINT [FK_CLS_ObjetoAprendizagemTurmaAula_CLS_TurmaAula] FOREIGN KEY ([tud_id], [tau_id]) REFERENCES [dbo].[CLS_TurmaAula] ([tud_id], [tau_id])
GO
ALTER TABLE [dbo].[CLS_ObjetoAprendizagemTurmaAula] ADD CONSTRAINT [FK_CLS_ObjetoAprendizagemTurmaAula_TUR_TurmaDisciplina] FOREIGN KEY ([tud_id]) REFERENCES [dbo].[TUR_TurmaDisciplina] ([tud_id])
GO

PRINT N'Adding foreign keys to [dbo].[CLS_ObjetoAprendizagemTurmaDisciplina]'
GO
ALTER TABLE [dbo].[CLS_ObjetoAprendizagemTurmaDisciplina] ADD CONSTRAINT [FK_CLS_ObjetoAprendizagemTurmaDisciplina_ACA_ObjetoAprendizagem] FOREIGN KEY ([oap_id]) REFERENCES [dbo].[ACA_ObjetoAprendizagem] ([oap_id])
GO
ALTER TABLE [dbo].[CLS_ObjetoAprendizagemTurmaDisciplina] ADD CONSTRAINT [FK_CLS_ObjetoAprendizagemTurmaDisciplina_TUR_TurmaDisciplina] FOREIGN KEY ([tud_id]) REFERENCES [dbo].[TUR_TurmaDisciplina] ([tud_id])
GO
ALTER TABLE [dbo].[CLS_ObjetoAprendizagemTurmaDisciplina] ADD CONSTRAINT [FK_CLS_ObjetoAprendizagemTurmaDisciplina_ACA_TipoPeriodoCalendario] FOREIGN KEY ([tpc_id]) REFERENCES [dbo].[ACA_TipoPeriodoCalendario] ([tpc_id])
GO

PRINT N'Adding foreign keys to [dbo].[ACA_ObjetoAprendizagem]'
GO
ALTER TABLE [dbo].[ACA_ObjetoAprendizagem] ADD CONSTRAINT [FK_ACA_ObjetoAprendizagem_ACA_TipoDisciplina] FOREIGN KEY ([tds_id]) REFERENCES [dbo].[ACA_TipoDisciplina] ([tds_id])
GO

PRINT N'Adding foreign keys to [dbo].[REL_DivergenciaRematriculas]'
GO
ALTER TABLE [dbo].[REL_DivergenciaRematriculas] ADD CONSTRAINT [FK_REL_DivergenciaRematriculas_MTR_MatriculaTurma2] FOREIGN KEY ([alu_id], [mtu_idFinal]) REFERENCES [dbo].[MTR_MatriculaTurma] ([alu_id], [mtu_id])
GO

PRINT N'Adding foreign keys to [dbo].[TUR_TurmaDisciplinaAulaSugestao]'
GO
ALTER TABLE [dbo].[TUR_TurmaDisciplinaAulaSugestao] ADD CONSTRAINT [FK_TUR_TurmaDisciplinaAulaSugestao_TUR_TurmaDisciplina] FOREIGN KEY ([tud_id]) REFERENCES [dbo].[TUR_TurmaDisciplina] ([tud_id])
GO
ALTER TABLE [dbo].[TUR_TurmaDisciplinaAulaSugestao] ADD CONSTRAINT [FK_TUR_TurmaDisciplinaAulaSugestao_ACA_TipoPeriodoCalendario] FOREIGN KEY ([tpc_id]) REFERENCES [dbo].[ACA_TipoPeriodoCalendario] ([tpc_id])
GO