/*------------------------------------------------------------------------------------
   Project/Ticket#: SGP
   Description: Atualiza o schema do banco de dados GestaoPedagogica da 1.67.0.0 para 1.68.0.0
-------------------------------------------------------------------------------------*/

SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
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
PRINT N'Altering [dbo].[ACA_TipoCiclo]'
GO
ALTER TABLE [dbo].[ACA_TipoCiclo] ADD
[tci_objetoAprendizagem] [bit] NULL
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
PRINT N'Altering [dbo].[REL_TurmaDisciplinaSituacaoFechamento]'
GO
ALTER TABLE [dbo].[REL_TurmaDisciplinaSituacaoFechamento] ADD
[PendentePlanejamento] [bit] NOT NULL CONSTRAINT [DF_REL_TurmaDisciplinaSituacaoFechamento_PendentePlanejamento] DEFAULT ((0))
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
PRINT N'Altering [dbo].[ACA_TipoCurriculoPeriodo]'
GO
ALTER TABLE [dbo].[ACA_TipoCurriculoPeriodo] ADD
[tcp_objetoAprendizagem] [bit] NOT NULL CONSTRAINT [DF__ACA_TipoC__tcp_o__3A1ACC2F] DEFAULT ((0))
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
PRINT N'Creating [dbo].[ACA_ObjetoAprendizagem]'
GO
CREATE TABLE [dbo].[ACA_ObjetoAprendizagem]
(
[oap_id] [int] NOT NULL IDENTITY(1, 1),
[tds_id] [int] NOT NULL,
[oap_descricao] [nvarchar] (500) COLLATE Latin1_General_CI_AS NOT NULL,
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
PRINT N'Creating [dbo].[ACA_AlunoJustificativaFalta]'
GO
CREATE TABLE [dbo].[ACA_AlunoJustificativaFalta]
(
[alu_id] [bigint] NOT NULL,
[afj_id] [int] NOT NULL,
[tjf_id] [int] NOT NULL,
[afj_dataInicio] [date] NOT NULL,
[afj_dataFim] [date] NULL,
[afj_situacao] [tinyint] NOT NULL CONSTRAINT [DF_ACA_AlunoJustificativaFalta_afj_situacao] DEFAULT ((1)),
[afj_dataCriacao] [datetime] NOT NULL CONSTRAINT [DF_ACA_AlunoJustificativaFalta_afj_dataCriacao] DEFAULT (getdate()),
[afj_dataAlteracao] [datetime] NOT NULL CONSTRAINT [DF_ACA_AlunoJustificativaFalta_afj_dataAlteracao] DEFAULT (getdate()),
[pro_id] [uniqueidentifier] NULL,
[afj_observacao] [varchar] (max) COLLATE Latin1_General_CI_AS NULL
)
GO
PRINT N'Creating primary key [PK_ACA_AlunoJustificativaFalta] on [dbo].[ACA_AlunoJustificativaFalta]'
GO
ALTER TABLE [dbo].[ACA_AlunoJustificativaFalta] ADD CONSTRAINT [PK_ACA_AlunoJustificativaFalta] PRIMARY KEY CLUSTERED  ([alu_id], [afj_id])
GO
PRINT N'Altering [dbo].[NEW_CLS_TurmaAulaAluno_SelectBy_TurmaDisciplina]'
GO

-- ========================================================================
-- Author:		Jean Michel Marques da Silva
-- Create date: 23/05/2011 09:07
-- Description:	utilizado no cadatro de planejamento, retorna os
--				lançamentos de frequência dos alunos
-- ========================================================================
ALTER PROCEDURE [dbo].[NEW_CLS_TurmaAulaAluno_SelectBy_TurmaDisciplina]
	@tud_id BIGINT	
	, @tau_id INT	
	, @ent_id UNIQUEIDENTIFIER	
	, @ordenacao TINYINT

AS
BEGIN	

	--Selecina as movimentações que possuem matrícula anterior
	WITH TabelaMovimentacao AS (
		SELECT
			alu_id,
			mtu_idAnterior,
			tmv_nome    
		FROM
			MTR_Movimentacao MOV WITH (NOLOCK) 
			INNER JOIN ACA_TipoMovimentacao TMV WITH (NOLOCK) 
				ON MOV.tmv_idSaida = TMV.tmv_id
		WHERE
			mov_situacao NOT IN (3,4)
			AND tmv_situacao <> 3
			AND mtu_idAnterior IS NOT NULL
	)
	SELECT	
		mtd.alu_id
		, mtd.mtu_id
		, mtd.mtd_id
		, mtd.tud_id
		, tau.tau_id
		, pes.pes_nome + 
			(
				CASE WHEN mtd_situacao = 5 THEN ISNULL((SELECT TOP 1 ' (' + tmv_nome + ')' FROM TabelaMovimentacao MOV WITH(NOLOCK) WHERE MOV.mtu_idAnterior = mtd.mtu_id AND MOV.alu_id = mtd.alu_id), ' (Inativo)')
					ELSE '' END
			) AS pes_nome
		,  CASE WHEN mtd.mtd_numeroChamada > 0 THEN CAST(Mtd.mtd_numeroChamada AS VARCHAR)
					ELSE '-' END as mtd_numeroChamada
		, taa.taa_frequencia 	
		, tau.tau_numeroAulas	
		, mtd.mtd_dataMatricula	
		, mtd.mtd_dataSaida
		, tau.tau_data
		-- 0 - não possui FJ / 1 - possui FJ que abona / 2 - possui FJ que não abona 
		, CASE WHEN afj.afj_id IS NULL
				THEN '0'					    						    
				ELSE (CASE WHEN tjf.tjf_abonaFalta = 1 THEN '1' ELSE '2' END)
		   END AS falta_justificada
		, NULL AS tca_numeroAvaliacao
		-- Verifica se há dispensa de disciplina para o aluno.
		, 0 AS dispensadisciplina
		, taa.taa_frequenciaBitMap
		, Mtd.mtd_situacao
	FROM 
		MTR_MatriculaTurma mtu WITH(NOLOCK)
		INNER JOIN MTR_MatriculaTurmaDisciplina mtd WITH(NOLOCK)
			ON mtu.mtu_id = mtd.mtu_id
			AND mtu.alu_id = mtd.alu_id
		INNER JOIN ACA_Aluno alu WITH(NOLOCK)
			ON mtd.alu_id = alu.alu_id
		INNER JOIN VW_DadosAlunoPessoa pes 
			ON alu.alu_id = pes.alu_id
		INNER JOIN CLS_TurmaAula tau WITH (NOLOCK)
			ON tau.tud_id = mtd.tud_id	
		INNER JOIN TUR_Turma tur WITH(NOLOCK)
			ON tur.tur_id = mtu.tur_id
			AND tur.tur_situacao <> 3		
		LEFT JOIN CLS_TurmaAulaAluno taa WITH (NOLOCK)		
			ON taa.tud_id = mtd.tud_id
				AND taa.tau_id = tau.tau_id
				AND taa.alu_id = mtd.alu_id
				AND taa.mtu_id = mtd.mtu_id
				AND taa.mtd_id = mtd.mtd_id
				AND taa.taa_situacao <> 3
		LEFT JOIN ACA_AlunoJustificativaFalta afj WITH(NOLOCK)
			ON  afj.alu_id = mtd.alu_id
			AND afj.afj_situacao <> 3
			AND (tau_data >= afj.afj_dataInicio)
			AND ((afj.afj_dataFim  IS NULL) OR (tau_data <= afj.afj_dataFim))
		LEFT JOIN ACA_TipoJustificativaFalta tjf WITH(NOLOCK)
			ON tjf.tjf_id = afj.tjf_id
			AND tjf.tjf_situacao <> 3
		LEFT OUTER JOIN CLS_AlunoAvaliacaoTurma Aat WITH(NOLOCK)
			ON (Aat.tur_id = Mtu.tur_id)
				AND (Aat.alu_id = Mtu.alu_id)
				AND (Aat.mtu_id = Mtu.mtu_id)
				AND (Aat.fav_id = tur.fav_id)
				AND (Aat.aat_situacao <> 3)
				AND Aat.ava_id =
				(
					SELECT  TOP 1 ava.ava_id
					FROM    ACA_Avaliacao ava WITH(NOLOCK)
					WHERE
						 ava.fav_id = tur.fav_id
						 AND ava.tpc_id = tau.tpc_id
						 AND ava.ava_situacao <> 3
				)
	WHERE 
		alu_situacao <> 3
		AND tau.tau_situacao <> 3
		AND Mtd.tud_id = @tud_id
		AND tau.tau_id = @tau_id
		AND alu.ent_id = @ent_id
		AND Mtd.mtd_situacao <> 3
		AND mtu_situacao <> 3
		-- Valida o período de matrícula e saída do aluno (se está dentro do período atual).
		AND (DATEDIFF(DAY, Mtd.mtd_dataMatricula, tau.tau_data) >= 0)
		AND ISNULL(Mtd.mtd_numeroChamada, 0) >= 0
		AND (Mtd.mtd_situacao <> 5 OR ISNULL((DATEDIFF(DAY, Mtd.mtd_dataSaida, tau.tau_data)), 0) <= 0)
	GROUP BY
		mtd.alu_id
		, mtd.mtu_id
		, mtd.mtd_id
		, mtd.tud_id
		, tau.tau_id
		, pes.pes_nome
		, mtd_situacao
		, mtd.mtd_numeroChamada
		, taa.taa_frequencia 	
		, tau.tau_numeroAulas	
		, mtd.mtd_dataMatricula	
		, mtd.mtd_dataSaida
		, tau.tau_data
		, afj.afj_id
		, tjf.tjf_abonaFalta
		, taa.taa_frequenciaBitMap
	ORDER BY 	
		CASE WHEN @ordenacao = 0 THEN 
			CASE WHEN ISNULL(Mtd.mtd_numeroChamada,0) <= 0 THEN 1 ELSE 0 END
		END ASC
		, CASE WHEN @ordenacao = 0 THEN ISNULL(Mtd.mtd_numeroChamada,0) END ASC
		, CASE WHEN @ordenacao = 1 THEN pes.pes_nome END ASC		
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
PRINT N'Creating [dbo].[STP_ACA_AlunoJustificativaFalta_LOAD]'
GO

CREATE PROCEDURE [dbo].[STP_ACA_AlunoJustificativaFalta_LOAD]
	@alu_id BigInt
	, @afj_id Int
	
AS
BEGIN
	SELECT	Top 1
		 alu_id  
		, afj_id 
		, tjf_id 
		, afj_dataInicio 
		, afj_dataFim 
		, afj_situacao 
		, afj_dataCriacao 
		, afj_dataAlteracao 
		, pro_id 
		, afj_observacao

 	FROM
 		ACA_AlunoJustificativaFalta
	WHERE 
		alu_id = @alu_id
		AND afj_id = @afj_id
END

GO
PRINT N'Altering [dbo].[REL_DivergenciaRematriculas]'
GO
ALTER TABLE [dbo].[REL_DivergenciaRematriculas] ADD
[mtu_idFinal] [int] NULL
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
PRINT N'Creating [dbo].[NEW_ACA_AlunoJustificativaFalta_SelecionaPorMesEAno]'
GO
-- =============================================
-- Author:	  Haila Pelloso
-- Create date: 29/11/2012
-- Description:	Seleciona os motivos de baixa frequência dos alunos filtrado por mês e ano
-- =============================================
CREATE PROCEDURE [dbo].[NEW_ACA_AlunoJustificativaFalta_SelecionaPorMesEAno]
	@mes INT
	,@ano INT
	
AS
BEGIN

	SELECT 
		Justificativa.alu_id
		, Justificativa.afj_id
		, Justificativa.tjf_id
		, (DATEDIFF(DAY, Justificativa.afj_dataInicio, Justificativa.afj_dataFim) + 1) as dias
		, CodigoGP
	FROM
		ACA_AlunoJustificativaFalta Justificativa WITH(NOLOCK)
		LEFT JOIN BNF_BolsaFamilia_DeParaJustificativaFalta AS DeParaJF WITH(NOLOCK)
			ON Justificativa.tjf_id = DeParaJF.tjf_id
	WHERE
		(MONTH(Justificativa.afj_dataInicio) = @mes OR MONTH(Justificativa.afj_dataFim) = @mes)
		AND (YEAR(Justificativa.afj_dataInicio) = @ano OR YEAR(Justificativa.afj_dataFim) = @ano)
		AND Justificativa.afj_situacao <> 3
		
END

GO
PRINT N'Altering [dbo].[FN_Select_FaltasAulasBy_Turma]'
GO
-- =============================================
-- Author:		Carla Frascareli
-- Create date: 18/01/2012
-- Description:	Retorna uma tabela com os alunos da turma, com a quantidade de aulas e faltas lançadas
--				pra cada um. Verifica o tipo de lançamento de frequência para buscar as quantidades
--				e somar as frequências.
-- =============================================
ALTER FUNCTION [dbo].[FN_Select_FaltasAulasBy_Turma]
(	
	@tipoLancamento TINYINT
	, @tpc_id INT
	, @tur_id BIGINT
)
-- Quantidade de faltas e aulas para do aluno
RETURNS @TabelaQtdes TABLE 
(
	alu_id BIGINT NOT NULL
	, mtu_id INT NOT NULL
	, qtFaltas INT NULL
	, qtAulas INT NULL
)
AS
BEGIN

	DECLARE @crp_qtdeTemposDia INT
	
	SELECT TOP 1 @crp_qtdeTemposDia = crp.crp_qtdeTemposDia
	FROM TUR_TurmaCurriculo tcr WITH(NOLOCK)
	INNER JOIN ACA_CurriculoPeriodo crp WITH(NOLOCK)
		ON crp.cur_id = tcr.cur_id
		AND crp.crr_id = tcr.crr_id
		AND crp.crp_id = tcr.crp_id
	WHERE 
		tcr.tur_id = @tur_id
		AND tcr.tcr_situacao <> 3
		
	-- Guardar todos os tud_ids da turma
	DECLARE @TabelaTudID TABLE (tud_id BIGINT NOT NULL, tud_tipo TINYINT NOT NULL , qtTitulares INT NOT NULL)
	
	INSERT INTO @TabelaTudID (tud_id, tud_tipo, qtTitulares)
	-- Trazer todos os tud_id
	SELECT 
		Tds.tud_id,
		Tds.tud_tipo,
		(
			SELECT COUNT(tdt.tdt_id)
			FROM TUR_TurmaDocente tdt WITH(NOLOCK)
			INNER JOIN ACA_TipoDocente tdc WITH(NOLOCK)
				ON tdc.tdc_posicao = tdt.tdt_posicao
				AND tdc.tdc_id IN (1,6)
				AND tdc.tdc_situacao <> 3
			WHERE tdt.tud_id = Tds.tud_id
			AND tdt.tdt_situacao = 1
		)
	FROM TUR_TurmaRelTurmaDisciplina RelTur WITH(NOLOCK)
	INNER JOIN TUR_TurmaDisciplina Tds WITH(NOLOCK)
		ON (Tds.tud_id = RelTur.tud_id)
			AND (Tds.tud_situacao <> 3)
	WHERE
		RelTur.tur_id = @tur_id
		
		
	DECLARE @tbFrequencia AS TABLE (alu_id BIGINT NOT NULL, mtu_id INT NOT NULL, frequencia INT)
	IF(@tipoLancamento = 1) -- 1-Aulas planejadas
	BEGIN
		INSERT INTO @tbFrequencia (alu_id, mtu_id, frequencia)
		SELECT 
			Taa.alu_id
			, Taa.mtu_id
			, MIN(ISNULL(taa_frequencia, 0)) frequencia
		 FROM 
			CLS_TurmaAulaAluno Taa WITH(NOLOCK)
		 INNER JOIN @TabelaTudID Tud 
			ON Tud.tud_id = Taa.tud_id
		 INNER JOIN CLS_TurmaAula Tau WITH(NOLOCK)
			ON Tau.tau_id = Taa.tau_id
			AND Tau.tud_id = Taa.tud_id
			AND Tau.tau_situacao <> 3
		 LEFT JOIN ACA_AlunoJustificativaFalta afj WITH(NOLOCK)
			ON Taa.alu_id = afj.alu_id
			AND afj.afj_situacao <> 3
			AND tau_data >= afj.afj_dataInicio
			AND ((afj.afj_dataFim  IS NULL) OR (tau_data <= afj.afj_dataFim))
		 LEFT JOIN ACA_TipoJustificativaFalta tjf WITH(NOLOCK)
			ON tjf.tjf_id = afj.tjf_id
			AND tjf.tjf_situacao <> 3
		 WHERE 
			Taa.taa_situacao <> 3
			AND Tau.tpc_id = @tpc_id
			AND ((tjf.tjf_abonaFalta IS NULL) OR (tjf.tjf_abonaFalta = 0))
		GROUP BY
			Tau.tau_data
			, Taa.alu_id
			, Taa.mtu_id
	END
	IF(@tipoLancamento = 5) -- 5-Aulas Dadas
	BEGIN
		INSERT INTO @tbFrequencia (alu_id, mtu_id, frequencia)
		SELECT 
			Taa.alu_id
			, Taa.mtu_id
			, MIN(ISNULL(taa_frequencia, 0)) frequencia
		 FROM 
			CLS_TurmaAulaAluno Taa WITH(NOLOCK)
		 INNER JOIN @TabelaTudID Tud 
			ON Tud.tud_id = Taa.tud_id
			AND Tud.qtTitulares < 2
		 INNER JOIN CLS_TurmaAula Tau WITH(NOLOCK)
			ON Tau.tau_id = Taa.tau_id
			AND Tau.tud_id = Taa.tud_id
			AND Tau.tau_situacao <> 3
		INNER JOIN dbo.ACA_TipoDocente TDC WITH(NOLOCK)
			ON	TDC.tdc_posicao = Tau.tdt_posicao
			AND	TDC.tdc_id IN ( 1, 4)	
		 LEFT JOIN ACA_AlunoJustificativaFalta afj WITH(NOLOCK)
			ON Taa.alu_id = afj.alu_id
			AND afj.afj_situacao <> 3
			AND tau_data >= afj.afj_dataInicio
			AND ((afj.afj_dataFim  IS NULL) OR (tau_data <= afj.afj_dataFim))
		 LEFT JOIN ACA_TipoJustificativaFalta tjf WITH(NOLOCK)
			ON tjf.tjf_id = afj.tjf_id
			AND tjf.tjf_situacao <> 3
		 WHERE 
			Taa.taa_situacao <> 3
			AND Tau.tpc_id = @tpc_id
			AND ((tjf.tjf_abonaFalta IS NULL) OR (tjf.tjf_abonaFalta = 0))
		GROUP BY
			Tau.tau_data
			, Taa.alu_id
			, Taa.mtu_id
		
		;WITH tbQtdeAulaFalta AS
		(
			SELECT
				mtd.alu_id,
				mtd.mtu_id,
				mtd.mtd_id,
				Tau.tau_data,
				Tau.tdt_posicao,
				SUM(ISNULL(Tau.tau_numeroAulas, 0)) AS qtAulas,
				SUM(ISNULL(Taa.taa_frequencia, 0)) AS qtFaltas
			FROM
				@TabelaTudID Tud 
				INNER JOIN CLS_TurmaAula Tau WITH(NOLOCK)
					ON Tau.tud_id = Tud.tud_id
					AND Tau.tpc_id = @tpc_id
					AND Tau.tau_situacao <> 3
				INNER JOIN ACA_TipoDocente TDC WITH(NOLOCK)
					ON	TDC.tdc_posicao = Tau.tdt_posicao
					AND	TDC.tdc_id IN ( 1, 4, 6)
				INNER JOIN MTR_MatriculaTurmaDisciplina mtd WITH(NOLOCK)
					ON Tud.tud_id = mtd.tud_id
					AND mtd.mtd_situacao <> 3	
				LEFT JOIN CLS_TurmaAulaAluno Taa WITH(NOLOCK)
					ON Tau.tud_id = Taa.tud_id 
					AND Tau.tau_id = Taa.tau_id
					AND Taa.alu_id = mtd.alu_id
					AND Taa.mtu_id = mtd.mtu_id
					AND Taa.mtd_id = mtd.mtd_id
					AND Taa.taa_situacao <> 3
				LEFT JOIN ACA_AlunoJustificativaFalta afj WITH(NOLOCK)
					ON Taa.alu_id = afj.alu_id
					AND afj.afj_situacao <> 3
					AND tau_data >= afj.afj_dataInicio
					AND ((afj.afj_dataFim  IS NULL) OR (tau_data <= afj.afj_dataFim))
				LEFT JOIN ACA_TipoJustificativaFalta tjf WITH(NOLOCK)
					ON tjf.tjf_id = afj.tjf_id
					AND tjf.tjf_situacao <> 3
			WHERE 
				Tud.tud_tipo = 11
				AND Tud.qtTitulares > 1
				AND ((tjf.tjf_abonaFalta IS NULL) OR (tjf.tjf_abonaFalta = 0))
			GROUP BY
				mtd.alu_id,
				mtd.mtu_id,
				mtd.mtd_id,
				Tau.tau_data,
				Tau.tdt_posicao
		) 
		, tbQtdeFalta AS 
		(
			SELECT
				alu_id,
				mtu_id,
				mtd_id,
				CASE WHEN SUM(qtAulas) = SUM(qtFaltas) THEN 1 ELSE 0 END AS qtFaltas
			FROM
				tbQtdeAulaFalta
			GROUP BY
				alu_id,
				mtu_id,
				mtd_id,
				tau_data
		)
		
		INSERT INTO @tbFrequencia (alu_id, mtu_id, frequencia)
		SELECT
			tqf.alu_id,
			tqf.mtu_id,
			SUM(tqf.qtFaltas) AS frequencia
		FROM
			tbQtdeFalta tqf
		GROUP BY
			tqf.alu_id,
			tqf.mtu_id
	END
	
	INSERT INTO @TabelaQtdes(alu_id, mtu_id, qtFaltas, qtAulas)
	SELECT
		Mtu.alu_id
		, Mtu.mtu_id
		,
		-- Qt faltas do aluno.
		(
			SELECT SUM(Freq.frequencia)
			FROM @tbFrequencia Freq
			WHERE 
				Freq.alu_id = Mtu.alu_id
				AND Freq.mtu_id = Mtu.mtu_id
		) AS QtFaltasAluno
		,
		-- Qt aulas do aluno.
		CASE 
			WHEN @tipoLancamento = 1 THEN -- 1-Aulas planejadas
				(SELECT TOP 1
					dbo.FN_CalcularDiasUteis
					(CASE WHEN cap.cap_dataInicio < Mtu.mtu_dataMatricula THEN Mtu.mtu_dataMatricula ELSE cap.cap_dataInicio END
					, 
					CASE WHEN cap.cap_dataFim > Mtu.mtu_dataSaida THEN Mtu.mtu_dataSaida ELSE cap.cap_dataFim END
					, esc.ent_id, tur.cal_id)
					*
					CASE WHEN fav.fav_tipoApuracaoFrequencia = 1 -- Tempo de aula
						THEN @crp_qtdeTemposDia
						ELSE 1
					END
				FROM TUR_Turma tur WITH(NOLOCK)
				INNER JOIN ACA_FormatoAvaliacao fav WITH(NOLOCK)
					ON  fav.fav_id = tur.fav_id
				INNER JOIN ESC_Escola esc WITH(NOLOCK)
					ON  esc.esc_id = tur.esc_id
				LEFT JOIN ACA_CalendarioPeriodo cap WITH(NOLOCK)
					ON  cap.cal_id = tur.cal_id
					AND cap.tpc_id = @tpc_id
					AND cap.cap_situacao <> 3
				WHERE 
					tur.tur_id = @tur_id
				)
			WHEN @tipoLancamento = 5 THEN -- 5-Aulas dadas
				(
					SELECT 
						SUM(Tau.tau_numeroAulas) NumeroAulas
					FROM
						@TabelaTudID Tud 
						INNER JOIN CLS_TurmaAula Tau WITH(NOLOCK)
							ON	Tau.tud_id = Tud.tud_id
							AND	Tau.tau_situacao <> 3
						INNER JOIN dbo.ACA_TipoDocente TDC WITH(NOLOCK)
							ON	TDC.tdc_posicao = Tau.tdt_posicao
							AND	TDC.tdc_id IN ( 1, 4)
					WHERE 
						Tau.tpc_id = @tpc_id
				)
		END AS QtAulasAluno
	FROM 
		MTR_MatriculaTurma Mtu WITH(NOLOCK)
	WHERE 
		Mtu.tur_id = @tur_id
	
	RETURN;
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
		, ISNULL(tci_objetoAprendizagem, 0) AS tci_objetoAprendizagem
 	FROM
 		dbo.ACA_TipoCiclo WITH(NOLOCK) 		
	WHERE 
		tci_situacao = 1
	--ORDER BY tci_nome
		ORDER BY tci_ordem

END


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
PRINT N'Creating [dbo].[NEW_ACA_AlunoJustificativaFalta_Update_Situacao]'
GO
-- ========================================================================
-- Author:		Aline Dornelas
-- Create date: 22/07/2011 12:03
-- Description:	Deleta o registro logicamente utilizando o 
--				campo situação (3 – Excluído)
-- ========================================================================
CREATE PROCEDURE [dbo].[NEW_ACA_AlunoJustificativaFalta_Update_Situacao]
	@alu_id BIGINT
	, @afj_id INT
AS
BEGIN
	UPDATE 
		ACA_AlunoJustificativaFalta
	SET 
		afj_situacao = 3
		, afj_dataAlteracao = GETDATE()
	WHERE 
		alu_id = @alu_id 
		AND afj_id = @afj_id 

		
	DECLARE @ret INT
	SELECT @ret = ISNULL(@@ROWCOUNT,-1)
	RETURN @ret
	
END
GO
PRINT N'Altering [dbo].[NEW_CLS_TurmaNotaAluno_SelectBy_TurmaDisciplinaPeriodoFiltroDeficiencia]'
GO

-- ========================================================================
-- Author:		Daniel Jun Suguimoto
-- Create date: 12/03/2014
-- Description:	Utilizado no cadatro de planejamento, retorna os
--				lançamentos de notas dos alunos, filtrando pelo período do
--				calendário.
--				Filtrando os alunos com ou sem deficiência, dependendo do docente.
-- ========================================================================
ALTER PROCEDURE [dbo].[NEW_CLS_TurmaNotaAluno_SelectBy_TurmaDisciplinaPeriodoFiltroDeficiencia]
	@tud_id BIGINT	
	, @tnt_id INT
	, @tpc_id INT
	, @tdc_id TINYINT
	, @ent_id UNIQUEIDENTIFIER	
	, @ordenacao TINYINT
	, @trazerInativos BIT

AS
BEGIN

    DECLARE @tipoLancamento TINYINT
    
    SELECT 
        @tipoLancamento = fav.fav_tipoLancamentoFrequencia
    FROM 
        TUR_TurmaDisciplina TurDiscip WITH(NOLOCK)
        INNER JOIN TUR_TurmaRelTurmaDisciplina TurRelD WITH(NOLOCK)
            ON (TurDiscip.tud_id = TurRelD.tud_id)
        INNER JOIN TUR_Turma Turma WITH(NOLOCK)
            ON (TurRelD.tur_id = Turma.tur_id)
        INNER JOIN ACA_FormatoAvaliacao fav WITH(NOLOCK)
            ON (Turma.fav_id = fav.fav_id)
    WHERE 
         TurDiscip.tud_id = @tud_id                   

	DECLARE @dataInicioPeriodo DATE
		, @dataFimPeriodo DATE
	
	-- Seta data de início e fim do período atual do calendário ligado a turma
	SELECT 
		@dataInicioPeriodo = Cap.cap_dataInicio
		, @dataFimPeriodo = Cap.cap_dataFim
	FROM 
		TUR_Turma Tur WITH(NOLOCK)
		INNER JOIN TUR_TurmaRelTurmaDisciplina TurRel WITH(NOLOCK)
			ON (TurRel.tud_id = @tud_id)
				AND (TurRel.tur_id = Tur.tur_id)
		INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
			ON (Cap.cal_id = Tur.cal_id)
	WHERE
		Cap.tpc_id = @tpc_id
		AND Cap.cap_situacao <> 3
		AND Tur.tur_situacao <> 3;               
	
	DECLARE @tud_tipo TINYINT;
	SELECT
		@tud_tipo = ISNULL(tud.tud_tipo,0)
	FROM
		TUR_TurmaDisciplina tud WITH(NOLOCK)
	WHERE
		tud.tud_id = @tud_id
		AND tud.tud_situacao <> 3
	
	DECLARE @tbAlunos TABLE (alu_id INT);
	
	IF (@tdc_id = 5)
	BEGIN
		;WITH MatriculaTurmaDisciplina AS
		(
			SELECT
				mtd.alu_id
			FROM
				MTR_MatriculaTurmaDisciplina mtd WITH(NOLOCK)
			WHERE
				mtd.tud_id = @tud_id
				AND mtd.mtd_situacao <> 3
		)
		
		, TipoDeficiencia AS 
		(
			SELECT 
				RelTde.tde_id
			FROM
				TUR_TurmaDisciplinaRelDisciplina DisRel WITH(NOLOCK)
				INNER JOIN ACA_Disciplina dis WITH(NOLOCK)
					ON DisRel.dis_id = dis.dis_id
					AND dis.dis_situacao <> 3
				INNER JOIN ACA_TipoDisciplina tds WITH(NOLOCK)
					ON dis.tds_id = tds.tds_id
					AND tds.tds_situacao <> 3
				INNER JOIN ACA_TipoDisciplinaDeficiencia RelTde WITH(NOLOCK)
					ON tds.tds_id = RelTde.tds_id
			WHERE
				DisRel.tud_id = @tud_id
		)
		
		INSERT INTO @tbAlunos 
		(
			alu_id
		)
		SELECT
			mtd.alu_id
		FROM
			MatriculaTurmaDisciplina mtd 
			INNER JOIN ACA_Aluno alu WITH(NOLOCK)
				ON alu.alu_id = mtd.alu_id
			INNER JOIN Synonym_PES_PessoaDeficiencia pde WITH(NOLOCK)
				ON pde.pes_id = alu.pes_id
			INNER JOIN TipoDeficiencia tde
				ON pde.tde_id = tde.tde_id
	END
	ELSE
	BEGIN
		;WITH MatriculaTurmaDisciplina AS
		(
			SELECT
				mtd.alu_id
			FROM
				MTR_MatriculaTurmaDisciplina mtd WITH(NOLOCK)
			WHERE
				mtd.tud_id = @tud_id
				AND mtd.mtd_situacao <> 3
		)
		
		, TipoDeficiencia AS 
		(
			SELECT 
				RelTde.tde_id
			FROM
				TUR_TurmaDisciplinaRelDisciplina DisRel WITH(NOLOCK)
				INNER JOIN ACA_Disciplina dis WITH(NOLOCK)
					ON DisRel.dis_id = dis.dis_id
					AND dis.dis_situacao <> 3
				INNER JOIN ACA_TipoDisciplina tds WITH(NOLOCK)
					ON dis.tds_id = tds.tds_id
					AND tds.tds_situacao <> 3
				INNER JOIN ACA_TipoDisciplinaDeficiencia RelTde WITH(NOLOCK)
					ON tds.tds_id = RelTde.tds_id
			WHERE
				DisRel.tud_id = @tud_id
		)
		
		INSERT INTO @tbAlunos 
		(
			alu_id
		)
		SELECT
			mtd.alu_id
		FROM
			MatriculaTurmaDisciplina mtd 
			INNER JOIN ACA_Aluno alu WITH(NOLOCK)
				ON alu.alu_id = mtd.alu_id
			LEFT JOIN Synonym_PES_PessoaDeficiencia pde WITH(NOLOCK)
				ON pde.pes_id = alu.pes_id
		WHERE
			(NOT EXISTS (SELECT tde_id FROM TipoDeficiencia tde WHERE tde.tde_id = pde.tde_id ))	
	END
		
	-- Se a disciplina não for um componente da regência, traz os dados normalmente
	IF (@tud_tipo <> 12)
	BEGIN	
		--Selecina as movimentações que possuem matrícula anterior
		WITH TabelaMovimentacao AS (
			SELECT
				alu_id,
				mtu_idAnterior,
				tmv_nome    
			FROM
				MTR_Movimentacao MOV WITH (NOLOCK) 
				INNER JOIN ACA_TipoMovimentacao TMV WITH (NOLOCK) 
					ON MOV.tmv_idSaida = TMV.tmv_id
			WHERE
				mov_situacao NOT IN (3,4)
				AND tmv_situacao <> 3
				AND mtu_idAnterior IS NOT NULL
		)			
		SELECT	
			mtd.alu_id
			, mtd.mtu_id
			, mtd.mtd_id
			, mtd.tud_id
			, tnt.tnt_id
			, tnt.tnt_efetivado
			, pes.pes_nome + 
				(
					CASE WHEN mtd_situacao = 5 THEN ISNULL((SELECT TOP 1 ' (' + tmv_nome + ')' FROM TabelaMovimentacao MOV WITH(NOLOCK) WHERE MOV.mtu_idAnterior = mtd.mtu_id AND MOV.alu_id = mtd.alu_id), ' (Inativo)')
						ELSE '' END
				) AS pes_nome
			,  CASE WHEN mtd.mtd_numeroChamada > 0 THEN CAST(mtd.mtd_numeroChamada AS VARCHAR)
						ELSE '-' END as mtd_numeroChamada
			, tna.tna_avaliacao
			, tna.tna_relatorio
			-- (FALTA JUSTIFICADA) 0 - não possui FJ / 1 - possui FJ que abona / 2 - possui FJ que não abona 
			, CASE WHEN afj.afj_id IS NULL
					THEN '0'					    						    
					ELSE (CASE WHEN tjf.tjf_abonaFalta = 1 THEN '1' ELSE '2' END)
			   END AS falta_justificada
			-- (ALUNO AUSENTE) 1 - aluno ausente que não possui falta justificada 
			, CAST( CASE WHEN @tipoLancamento IN  (1,4,5) AND TAU.tau_id IS NOT NULL THEN
					(
						CASE WHEN fav.fav_tipoApuracaoFrequencia = 2 AND crp.crp_controleTempo = 2 THEN
						(
							CASE WHEN
							((SELECT 
								taa_frequencia 
							 FROM 
								CLS_TurmaAulaAluno WITH (NOLOCK)
							 WHERE 
								tud_id = TAU.tud_id
								AND tau_id = TAU.tau_id
								AND alu_id = mtd.alu_id
								AND mtu_id = mtd.mtu_id
								AND mtd_id = mtd.mtd_id) > 0)
								AND afj.afj_id IS NULL
							THEN 1 ELSE 0 END
						)
						ELSE
						(
							-- Aulas planejadas
							CASE WHEN  
							((SELECT 
								taa_frequencia 
							 FROM 
								CLS_TurmaAulaAluno WITH (NOLOCK)
							 WHERE 
								tud_id = TAU.tud_id
								AND tau_id = TAU.tau_id
								AND alu_id = mtd.alu_id
								AND mtu_id = mtd.mtu_id
								AND mtd_id = mtd.mtd_id) = TAU.tau_numeroAulas)
								AND afj.afj_id IS NULL
							THEN 1 ELSE 0 END
						) END )
					-- Período ( Nâo terá esta funcionalidade )
					ELSE 0				       
			END AS BIT) AS aluno_ausente 
			, NULL AS tca_numeroAvaliacao
			-- Verifica se há dispensa de disciplina para o aluno.
			, 0 AS dispensadisciplina
			, ISNULL(tna.tna_participante, 0) AS tna_participante
		FROM 
			MTR_MatriculaTurmaDisciplina mtd WITH(NOLOCK)
			INNER JOIN MTR_MatriculaTurma mtu WITH(NOLOCK)
				ON mtd.alu_id = mtu.alu_id 
				AND mtd.mtu_id = mtu.mtu_id
				AND mtu.mtu_situacao <> 3
			INNER JOIN ACA_CurriculoPeriodo crp WITH(NOLOCK)
				ON crp.cur_id = mtu.cur_id
				AND crp.crr_id = mtu.crr_id
				AND crp.crp_id = mtu.crp_id
				AND crp.crp_situacao <> 3
			INNER JOIN @tbAlunos
				ON mtu.alu_id = [@tbAlunos].alu_id
			INNER JOIN ACA_Aluno alu WITH(NOLOCK)
				ON mtd.alu_id = alu.alu_id		
			INNER JOIN VW_DadosAlunoPessoa pes
				ON alu.alu_id = pes.alu_id
			INNER JOIN CLS_TurmaNota tnt WITH (NOLOCK)
				ON tnt.tud_id = mtd.tud_id	
			INNER JOIN TUR_Turma tur WITH(NOLOCK)
				ON tur.tur_id = mtu.tur_id
				AND tur.tur_situacao <> 3		
			INNER JOIN ACA_FormatoAvaliacao fav WITH(NOLOCK)
				ON tur.fav_id = fav.fav_id
				AND fav.fav_situacao <> 3					
			LEFT JOIN CLS_TurmaAula TAU WITH(NOLOCK)
				ON tnt.tud_id = TAU.tud_id 
					AND tnt.tau_id = TAU.tau_id
					AND TAU.tud_id = tnt.tud_id
					AND TAU.tpc_id = tnt.tpc_id
					AND TAU.tau_situacao <> 3
			LEFT JOIN CLS_TurmaNotaAluno tna WITH (NOLOCK)		
				ON tna.tud_id = mtd.tud_id
					AND tna.tnt_id = tnt.tnt_id
					AND tna.alu_id = mtd.alu_id
					AND tna.mtu_id = mtd.mtu_id
					AND tna.mtd_id = mtd.mtd_id
					AND tna.tna_situacao <> 3
			LEFT JOIN ACA_AlunoJustificativaFalta afj WITH(NOLOCK)
			   ON  afj.alu_id = mtd.alu_id
				   AND afj.afj_situacao <> 3
				   AND (tau_data >= afj.afj_dataInicio)
				   AND ((afj.afj_dataFim  IS NULL) OR (tau_data <= afj.afj_dataFim))
			LEFT JOIN ACA_TipoJustificativaFalta tjf WITH(NOLOCK)
			   ON tjf.tjf_id = afj.tjf_id
				   AND tjf.tjf_situacao <> 3
			LEFT OUTER JOIN CLS_AlunoAvaliacaoTurma Aat WITH(NOLOCK)
				ON (Aat.tur_id = Mtu.tur_id)
					AND (Aat.alu_id = Mtu.alu_id)
					AND (Aat.mtu_id = Mtu.mtu_id)
					AND (Aat.fav_id = tur.fav_id)
					AND (Aat.aat_situacao <> 3)
					AND Aat.ava_id =
					(
						SELECT  TOP 1 ava.ava_id
						FROM    ACA_Avaliacao ava WITH(NOLOCK)
						WHERE
							 ava.fav_id = tur.fav_id
							 AND ava.tpc_id = tau.tpc_id
							 AND ava.ava_situacao <> 3
					)
		WHERE 
			-- Trazer somente ativos, ou também os inativos, quando a flag for true
			(mtd_situacao = 1 OR (@trazerInativos = 1 AND mtd_situacao = 5 AND ISNULL(mtd_numeroChamada, 0) >= 0))	
			AND alu_situacao <> 3
			AND tnt.tnt_situacao <> 3
			AND mtd.tud_id = @tud_id
			AND tnt.tnt_id = @tnt_id	 	
			AND alu.ent_id = @ent_id		
			-- Valida o período de matrícula e saída do aluno (se está dentro do período atual).
			AND (@tpc_id IS NULL OR mtd.mtd_dataMatricula <= @dataFimPeriodo)
			AND ((@tpc_id IS NULL) OR (mtd_situacao <> 5 OR mtd.mtd_dataSaida >= @dataInicioPeriodo))
			---- Valida o período de matrícula e saída do aluno (se está dentro da data da aula).
			AND (TAU.tau_id IS NULL 
					OR (
						(DATEDIFF(DAY, Mtd.mtd_dataMatricula, tau.tau_data) >= 0)
						AND (mtd_situacao <> 5 OR ISNULL((DATEDIFF(DAY, Mtd.mtd_dataSaida, tau.tau_data)), 0) <= 0)
					)
			)	
		GROUP BY
			mtd.alu_id
			, mtd.mtu_id
			, mtd.mtd_id
			, mtd.tud_id
			, tnt.tnt_id
			, tnt.tnt_efetivado
			, pes.pes_nome
			, mtd_situacao
			, mtd.mtd_numeroChamada 
			, tna.tna_avaliacao
			, tna.tna_participante
			, tna.tna_relatorio
			, afj.afj_id
			, tjf.tjf_abonaFalta
			, TAU.tau_id
			, TAU.tud_id
			, TAU.tau_numeroAulas
			, fav.fav_tipoApuracaoFrequencia
			, crp.crp_controleTempo
		ORDER BY 	
			CASE WHEN @ordenacao = 0 THEN 
				CASE WHEN ISNULL(Mtd.mtd_numeroChamada,0) <= 0 THEN 1 ELSE 0 END
			END ASC
			, CASE WHEN @ordenacao = 0 THEN ISNULL(Mtd.mtd_numeroChamada,0) END ASC
			, CASE WHEN @ordenacao = 1 THEN pes.pes_nome END ASC
	END
	ELSE
	
	-- Se for um componente da regência, traz os dados baseados nas aulas da regência
	BEGIN
		DECLARE @tud_idRegencia BIGINT = NULL;

		-- Caso seja Componente de regência pega o tud_id da regência
		SELECT 
			@tud_idRegencia = TUR_TUD_REG.tud_id
		FROM 
			dbo.TUR_TurmaDisciplina TUD WITH(NOLOCK)
			INNER JOIN dbo.TUR_TurmaDisciplinaRelDisciplina TUD_DIS WITH(NOLOCK)
				ON	TUD_DIS.tud_id = TUD.tud_id
			INNER JOIN dbo.TUR_TurmaRelTurmaDisciplina TUR_TUD WITH(NOLOCK)
				ON	TUR_TUD.tud_id = TUD.tud_id
			INNER JOIN dbo.ACA_CurriculoDisciplina CRD WITH(NOLOCK)
				ON	CRD.dis_id = TUD_DIS.dis_id
			INNER JOIN dbo.ACA_CurriculoDisciplina CRDREG WITH(NOLOCK)
				ON	CRDREG.cur_id = CRD.cur_id
				AND CRDREG.crr_id = CRD.crr_id
				AND CRDREG.crp_id = CRD.crp_id
			INNER JOIN dbo.TUR_TurmaDisciplinaRelDisciplina TUD_DIS_REG WITH(NOLOCK)
				ON	TUD_DIS_REG.dis_id = CRDREG.dis_id
			INNER JOIN dbo.TUR_TurmaRelTurmaDisciplina TUR_TUD_REG WITH(NOLOCK)
				ON	TUR_TUD_REG.tud_id = TUD_DIS_REG.tud_id
				AND TUR_TUD_REG.tur_id = TUR_TUD.tur_id
		WHERE 
			TUD.tud_id = @tud_id
			AND TUD.tud_tipo = 12
			AND CRDREG.crd_tipo = 11
			-- Exclusão Lógica
			AND TUD.tud_situacao <> 3
			AND CRD.crd_situacao <> 3
			AND CRDREG.crd_situacao <> 3
	
		--Selecina as movimentações que possuem matrícula anterior
		;WITH TabelaMovimentacao AS (
			SELECT
				alu_id,
				mtu_idAnterior,
				tmv_nome    
			FROM
				MTR_Movimentacao MOV WITH (NOLOCK) 
				INNER JOIN ACA_TipoMovimentacao TMV WITH (NOLOCK) 
					ON MOV.tmv_idSaida = TMV.tmv_id
			WHERE
				mov_situacao NOT IN (3,4)
				AND tmv_situacao <> 3
				AND mtu_idAnterior IS NOT NULL
		)	
		
		-- Aulas da regência
		, tbAulas AS 
		(
			SELECT 
				tau.tud_id,
				tau.tau_id,
				tau.tau_data,
				tau.tau_numeroAulas,
				tau.tpc_id
			FROM 
				CLS_TurmaAula tau WITH(NOLOCK)
			WHERE
				tau.tud_id = @tud_idRegencia
				AND tau.tpc_id = @tpc_id
				AND tau.tau_situacao <> 3
		)
		
				
		SELECT	
			mtd.alu_id
			, mtd.mtu_id
			, mtd.mtd_id
			, mtd.tud_id
			, tnt.tnt_id
			, tnt.tnt_efetivado
			, pes.pes_nome + 
				(
					CASE WHEN mtd_situacao = 5 THEN ISNULL((SELECT TOP 1 ' (' + tmv_nome + ')' FROM TabelaMovimentacao MOV WITH(NOLOCK) WHERE MOV.mtu_idAnterior = mtd.mtu_id AND MOV.alu_id = mtd.alu_id), ' (Inativo)')
						ELSE '' END
				) AS pes_nome
			,  CASE WHEN mtd.mtd_numeroChamada > 0 THEN CAST(mtd.mtd_numeroChamada AS VARCHAR)
						ELSE '-' END as mtd_numeroChamada
			, tna.tna_avaliacao
			, tna.tna_relatorio
			-- (FALTA JUSTIFICADA) 0 - não possui FJ / 1 - possui FJ que abona / 2 - possui FJ que não abona 
			, CASE WHEN afj.afj_id IS NULL
					THEN '0'					    						    
					ELSE (CASE WHEN tjf.tjf_abonaFalta = 1 THEN '1' ELSE '2' END)
			   END AS falta_justificada
			-- (ALUNO AUSENTE) 1 - aluno ausente que não possui falta justificada 
			, CAST( CASE WHEN @tipoLancamento IN  (1,4) AND TAU.tau_id IS NOT NULL THEN
				-- Aulas planejadas
				  CASE WHEN  
					((SELECT 
						taa_frequencia 
					 FROM 
						CLS_TurmaAulaAluno WITH (NOLOCK)
					 WHERE 
						tud_id = TAU.tud_id
						AND tau_id = TAU.tau_id
						AND alu_id = mtd.alu_id
						AND mtu_id = mtd.mtu_id) = TAU.tau_numeroAulas)
						AND afj.afj_id IS NULL
					THEN 1 ELSE 0 END
					-- Período ( Nâo terá esta funcionalidade )
					ELSE 0				       
			END AS BIT) AS aluno_ausente 
			, NULL AS tca_numeroAvaliacao
			-- Verifica se há dispensa de disciplina para o aluno.
			, 0 AS dispensadisciplina
			, ISNULL(tna.tna_participante, 0) AS tna_participante
		FROM 
			MTR_MatriculaTurmaDisciplina mtd WITH(NOLOCK)
			INNER JOIN MTR_MatriculaTurma mtu WITH(NOLOCK)
				ON mtd.alu_id = mtu.alu_id 
				AND mtd.mtu_id = mtu.mtu_id
				AND mtu.mtu_situacao <> 3
			INNER JOIN @tbAlunos
				ON mtu.alu_id = [@tbAlunos].alu_id
			INNER JOIN ACA_Aluno alu WITH(NOLOCK)
				ON mtd.alu_id = alu.alu_id		
			INNER JOIN VW_DadosAlunoPessoa pes 
				ON alu.alu_id = pes.alu_id
			INNER JOIN CLS_TurmaNota tnt WITH (NOLOCK)
				ON tnt.tud_id = mtd.tud_id	
			INNER JOIN TUR_Turma tur WITH(NOLOCK)
				ON tur.tur_id = mtu.tur_id
				AND tur.tur_situacao <> 3						
			LEFT JOIN tbAulas TAU 
				ON Tau.tau_data = Tnt.tnt_data
			LEFT JOIN CLS_TurmaNotaAluno tna WITH (NOLOCK)		
				ON tna.tud_id = mtd.tud_id
					AND tna.tnt_id = tnt.tnt_id
					AND tna.alu_id = mtd.alu_id
					AND tna.mtu_id = mtd.mtu_id
					AND tna.mtd_id = mtd.mtd_id
					AND tna.tna_situacao <> 3
			LEFT JOIN ACA_AlunoJustificativaFalta afj WITH(NOLOCK)
			   ON  afj.alu_id = mtd.alu_id
				   AND afj.afj_situacao <> 3
				   AND (tau_data >= afj.afj_dataInicio)
				   AND ((afj.afj_dataFim  IS NULL) OR (tau_data <= afj.afj_dataFim))
			LEFT JOIN ACA_TipoJustificativaFalta tjf WITH(NOLOCK)
			   ON tjf.tjf_id = afj.tjf_id
				   AND tjf.tjf_situacao <> 3
			LEFT OUTER JOIN CLS_AlunoAvaliacaoTurma Aat WITH(NOLOCK)
				ON (Aat.tur_id = Mtu.tur_id)
					AND (Aat.alu_id = Mtu.alu_id)
					AND (Aat.mtu_id = Mtu.mtu_id)
					AND (Aat.fav_id = tur.fav_id)
					AND (Aat.aat_situacao <> 3)
					AND Aat.ava_id =
					(
						SELECT  TOP 1 ava.ava_id
						FROM    ACA_Avaliacao ava WITH(NOLOCK)
						WHERE
							 ava.fav_id = tur.fav_id
							 AND ava.tpc_id = tau.tpc_id
							 AND ava.ava_situacao <> 3
					)	
		WHERE 
			-- Trazer somente ativos, ou também os inativos, quando a flag for true
			(mtd_situacao = 1 OR (@trazerInativos = 1 AND mtd_situacao = 5 AND ISNULL(mtd_numeroChamada, 0) >= 0))	
			AND alu_situacao <> 3
			AND tnt.tnt_situacao <> 3
			AND mtd.tud_id = @tud_id
			AND tnt.tnt_id = @tnt_id	 	
			AND alu.ent_id = @ent_id		
			-- Valida o período de matrícula e saída do aluno (se está dentro do período atual).
			AND (@tpc_id IS NULL OR mtd.mtd_dataMatricula <= @dataFimPeriodo)
			AND ((@tpc_id IS NULL) OR (mtd_situacao <> 5 OR mtd.mtd_dataSaida >= @dataInicioPeriodo))
			---- Valida o período de matrícula e saída do aluno (se está dentro da data da aula).
			AND (TAU.tau_id IS NULL 
					OR (
						(DATEDIFF(DAY, Mtd.mtd_dataMatricula, tau.tau_data) >= 0)
						AND (mtd_situacao <> 5 OR ISNULL((DATEDIFF(DAY, Mtd.mtd_dataSaida, tau.tau_data)), 0) <= 0)
					)
			)	
		GROUP BY
			mtd.alu_id
			, mtd.mtu_id
			, mtd.mtd_id
			, mtd.tud_id
			, tnt.tnt_id
			, tnt.tnt_efetivado
			, pes.pes_nome
			, mtd_situacao
			, mtd.mtd_numeroChamada 
			, tna.tna_avaliacao
			, tna.tna_participante
			, tna.tna_relatorio
			, afj.afj_id
			, tjf.tjf_abonaFalta
			, TAU.tau_id
			, TAU.tud_id
			, TAU.tau_numeroAulas
		ORDER BY 	
			CASE WHEN @ordenacao = 0 THEN 
				CASE WHEN ISNULL(Mtd.mtd_numeroChamada,0) <= 0 THEN 1 ELSE 0 END
			END ASC
			, CASE WHEN @ordenacao = 0 THEN ISNULL(Mtd.mtd_numeroChamada,0) END ASC
			, CASE WHEN @ordenacao = 1 THEN pes.pes_nome END ASC
	END
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
PRINT N'Altering [dbo].[FN_Select_FaltasAulasBy_TurmaDisciplina]'
GO
-- =============================================
-- Author:		Carla Frascareli
-- Create date: 18/01/2012
-- Description:	Retorna uma tabela com os alunos da disciplina, com a quantidade de aulas e faltas
--				lançadas pra cada um. Verifica o tipo de lançamento de frequência para buscar as
--				quantidades e somar as frequências.

---- Alterado: Marcia Haga - 26/01/2015
---- Description: Corrigido para retornar apenas os dados referentes
---- as disciplinas do mesmo calendario.

-- Alterado: Pedro Silva - 29/07/2015
-- Description: Adicionados na tabela de retorno os campos AulasNormais e AulasReposicao, para serem usados no fechamento automático
-- A lógica pra preecher estes campos foi feita apenas para o tipoLançamento = 6 (que é o único usado em SP por enquanto)
-- para os outros tipos estes campos serão retornados nulos por enquanto.
-- Além disso, adicionei tratamento ao final da function para tratar o retorno nulo de alguns campos, quando deveriam retornar 0.
-- =============================================
ALTER FUNCTION [dbo].[FN_Select_FaltasAulasBy_TurmaDisciplina]
(	
	@tipoLancamento TINYINT
	, @tpc_id INT 
	, @tud_id BIGINT
	, @fav_calculoQtdeAulasDadas TINYINT
	/*
		tipoDocente
		0 - administrador
		1 - titular
		5 - especial
	*/
	, @tipoDocente TINYINT = 0
)
-- Quantidade de faltas e aulas para do aluno
RETURNS @TabelaQtdes TABLE 
(
	alu_id BIGINT NOT NULL
	, mtu_id INT NOT NULL
	, mtd_id INT NOT NULL
	, qtFaltas INT NULL
	, qtAulas INT NULL
	, qtFaltasReposicao INT NULL
	, qtAulasNormais INT NULL
	, qtAulasReposicao INT NULL
	, qtFaltasReposicaoNaoAcumuladas INT NULL
)
AS
BEGIN

	declare @TabelaQtdesFaltas TABLE 
	(	alu_id BIGINT NOT NULL
		, mtu_id INT NOT NULL
		, mtd_id INT NOT NULL
		, qtFaltas INT NULL
		, qtFaltasReposicao INT NULL)
		
	declare @TabelaQtdesFaltas_SemAcumularReposicao TABLE 
	(	alu_id BIGINT NOT NULL
		, mtu_id INT NOT NULL
		, mtd_id INT NOT NULL
		, qtFaltasReposicaoNaoAcumulada INT NULL)
	
	declare @TabelaQtdesAulas TABLE 
	(   alu_id BIGINT NOT NULL
		, mtu_id INT NOT NULL
		, mtd_id INT NOT NULL
		, qtAulas INT NULL
		, qtAulasNormais INT NULL
		, qtAulasReposicao INT NULL)
		
	declare @tbTurmaAula TABLE
	(	tau_id BIGINT
		,tud_id BIGINT
		,tau_data DATE
		,tau_reposicao INT
		,tpc_id INT
		,tpc_ordem INT)


	DECLARE @cal_id INT;
	SELECT @cal_id = tur.cal_id 
	FROM TUR_TurmaRelTurmaDisciplina ttd WITH(NOLOCK)
	INNER JOIN TUR_Turma tur WITH(NOLOCK)
		ON tur.tur_id = ttd.tur_id	
		AND tur.tur_situacao <> 3
	WHERE ttd.tud_id = @tud_id

	DECLARE @tud_tipo TINYINT;
	SELECT
			@tud_tipo = tud_tipo
		FROM
			TUR_TurmaDisciplina WITH(NOLOCK)
		WHERE
			tud_id = @tud_id
			AND tud_situacao <> 3

	/**
	* O cálculo por aulas dadas considera a quantidade de aulas e faltas do lançamento do professor, com algumas observações:
	* Quando a disciplina é "complemento da regência" (Inglês), a quantidade de aulas vêm do Inglês, e a qt de faltas, da regência.
	* Quando é docente especial (posição 5), precisa trazer as aulas separados da posição 5 e da 1 (titular).
	* Quando não é docente especial, sempre considera apenas aulas de titular e substituto (nunca de projetos ou compartilhado).
	* Para todos os casos, considerar as aulas dos alunos em todas as turmas que ele passou no bimestre, ou seja,
	*	quando houver uma transferência, as quantidades de aulas e faltas devem ser somadas (da turma de origem e destino).
	*/
	DECLARE @tbAlunos TABLE (alu_id BIGINT NOT NULL, mtu_id INT NOT NULL, mtd_id INT NOT NULL, tud_id BIGINT NOT NULL
		, mtu_idOrigem INT NOT NULL, mtd_idOrigem INT NOT NULL -- Origem - matrícula do aluno no @tud_id informado.
		, mtd_dataMatricula DATE, mtd_dataSaida DATE
		, tud_tipo TINYINT, tud_idRegencia BIGINT NULL, tud_idAluno BIGINT NULL);
	
	-- Configurar as posições de aulas possíveis.
	DECLARE @tbPosicaoDocente TABLE (tdc_posicao TINYINT NOT NULL)
	
	DECLARE @qtdeTitulares INT;
	
	IF(@tipoLancamento = 2) -- 2-Período
	BEGIN
		IF (@tud_tipo = 15)
		BEGIN
			;WITH MatriculaTurmaDisciplinaMultisseriada AS
			(
				SELECT
					mtdDocente.alu_id,
					mtdDocente.mtu_id,
					mtdDocente.mtd_id AS mtd_idDocente,
					tudDocente.tud_id AS tud_idDocente,
					mtdAluno.mtd_id AS mtd_idAluno,
					tudAluno.tud_id AS tud_idAluno,
					mtdDocente.mtd_dataMatricula AS mtd_dataMatriculaDocente,
					mtdDocente.mtd_dataSaida AS mtd_dataSaidaDocente,
					mtdAluno.mtd_dataMatricula AS mtd_dataMatriculaAluno,
					mtdAluno.mtd_dataSaida AS mtd_dataSaidaAluno
				FROM
					MTR_MatriculaTurmaDisciplina mtdDocente WITH(NOLOCK)
					INNER JOIN TUR_TurmaDisciplina tudDocente WITH(NOLOCK)
						ON tudDocente.tud_id = mtdDocente.tud_id
						AND tudDocente.tud_tipo = @tud_tipo
						AND tudDocente.tud_situacao <> 3
					INNER JOIN MTR_MatriculaTurmaDisciplina mtdAluno WITH(NOLOCK)
						ON mtdAluno.alu_id = mtdDocente.alu_id
						AND mtdAluno.mtu_id = mtdDocente.mtu_id
						AND mtdAluno.mtd_situacao IN (1,5)
					INNER JOIN TUR_TurmaDisciplina tudAluno WITH(NOLOCK)
						ON tudAluno.tud_id = mtdAluno.tud_id
						AND tudAluno.tud_tipo = 16
						AND tudAluno.tud_situacao <> 3
				WHERE 
					mtdDocente.tud_id = @tud_id
					AND mtdDocente.mtd_situacao IN (1,5)
				GROUP BY
					mtdDocente.alu_id,
					mtdDocente.mtu_id,
					mtdDocente.mtd_id,
					tudDocente.tud_id,
					mtdAluno.mtd_id,
					tudAluno.tud_id,
					mtdDocente.mtd_dataMatricula,
					mtdDocente.mtd_dataSaida,
					mtdAluno.mtd_dataMatricula,
					mtdAluno.mtd_dataSaida
			)

			, tbTurmaDisciplina AS
			(
				SELECT tud_idAluno AS tud_id
				FROM MatriculaTurmaDisciplinaMultisseriada
				GROUP BY tud_idAluno

				UNION 

				SELECT @tud_id AS tud_id
			)

			INSERT INTO @TabelaQtdes(alu_id, mtu_id, mtd_id, qtFaltas, qtAulas)
			SELECT
				Mtd.alu_id
				, Mtd.mtu_id
				, Mtd.mtd_idAluno
				,
				-- Qt faltas do aluno.
				0 AS QtFaltasAluno
				,
				-- Qt aulas do aluno.
				0 AS QtAulasAluno
			FROM MatriculaTurmaDisciplinaMultisseriada Mtd
		END
		ELSE
		BEGIN 
			INSERT INTO @TabelaQtdes(alu_id, mtu_id, mtd_id, qtFaltas, qtAulas)
			SELECT
				Mtd.alu_id
				, Mtd.mtu_id
				, Mtd.mtd_id
				,
				-- Qt faltas do aluno.
				0 AS QtFaltasAluno
				,
				-- Qt aulas do aluno.
				0 AS QtAulasAluno
			FROM MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
			WHERE 
				(Mtd.tud_id = @tud_id)
				AND mtd_situacao IN (1,5)
		END
	END
	ELSE IF(@tipoLancamento IN (1, 4)) -- 1-Aulas planejadas, 4-Aulas planejadas e mensal
	BEGIN
		DECLARE @crp_qtdeTemposDia INT
		DECLARE @tur_id BIGINT
		
		IF (@tud_tipo = 15)
		BEGIN
			SELECT TOP 1 
				@crp_qtdeTemposDia = crp.crp_qtdeTemposDia, 
				@tur_id = turRtud.tur_id
			FROM TUR_TurmaRelTurmaDisciplina turRtud WITH(NOLOCK)
			INNER JOIN TUR_TurmaDisciplina AS tud WITH (NOLOCK)
				ON turRtud.tud_id = tud.tud_id
			INNER JOIN TUR_TurmaDisciplinaRelDisciplina disRtud WITH(NOLOCK)
				ON tud.tud_id = disRtud.tud_id
			INNER JOIN ACA_Disciplina dis WITH(NOLOCK)
				ON dis.dis_id = disRtud.dis_id
				AND dis.dis_situacao <> 3
			INNER JOIN TUR_TurmaCurriculo tcr WITH(NOLOCK)
				ON  tcr.tur_id = turRtud.tur_id
			INNER JOIN ACA_CurriculoPeriodo crp WITH(NOLOCK)
				ON crp.cur_id = tcr.cur_id
				AND crp.crr_id = tcr.crr_id
				AND crp.crp_id = tcr.crp_id
			WHERE 
				turRtud.tud_id = @tud_id
				AND tcr.tcr_situacao <> 3	
		
			;WITH MatriculaTurmaDisciplinaMultisseriada AS
			(
				SELECT
					mtdDocente.alu_id,
					mtdDocente.mtu_id,
					mtdDocente.mtd_id AS mtd_idDocente,
					tudDocente.tud_id AS tud_idDocente,
					mtdAluno.mtd_id AS mtd_idAluno,
					tudAluno.tud_id AS tud_idAluno,
					mtdDocente.mtd_dataMatricula AS mtd_dataMatriculaDocente,
					mtdDocente.mtd_dataSaida AS mtd_dataSaidaDocente,
					mtdAluno.mtd_dataMatricula AS mtd_dataMatriculaAluno,
					mtdAluno.mtd_dataSaida AS mtd_dataSaidaAluno
				FROM
					MTR_MatriculaTurmaDisciplina mtdDocente WITH(NOLOCK)
					INNER JOIN TUR_TurmaDisciplina tudDocente WITH(NOLOCK)
						ON tudDocente.tud_id = mtdDocente.tud_id
						AND tudDocente.tud_tipo = @tud_tipo
						AND tudDocente.tud_situacao <> 3
					INNER JOIN MTR_MatriculaTurmaDisciplina mtdAluno WITH(NOLOCK)
						ON mtdAluno.alu_id = mtdDocente.alu_id
						AND mtdAluno.mtu_id = mtdDocente.mtu_id
						AND mtdAluno.mtd_situacao IN (1,5)
					INNER JOIN TUR_TurmaDisciplina tudAluno WITH(NOLOCK)
						ON tudAluno.tud_id = mtdAluno.tud_id
						AND tudAluno.tud_tipo = 16
						AND tudAluno.tud_situacao <> 3
				WHERE 
					mtdDocente.tud_id = @tud_id
					AND mtdDocente.mtd_situacao IN (1,5)
				GROUP BY
					mtdDocente.alu_id,
					mtdDocente.mtu_id,
					mtdDocente.mtd_id,
					tudDocente.tud_id,
					mtdAluno.mtd_id,
					tudAluno.tud_id,
					mtdDocente.mtd_dataMatricula,
					mtdDocente.mtd_dataSaida,
					mtdAluno.mtd_dataMatricula,
					mtdAluno.mtd_dataSaida
			)

			, tbTurmaDisciplina AS
			(
				SELECT tud_idAluno AS tud_id
				FROM MatriculaTurmaDisciplinaMultisseriada
				GROUP BY tud_idAluno

				UNION 

				SELECT @tud_id AS tud_id
			)

			, tbFrequencia AS (
				SELECT 
					Taa.alu_id
					, Taa.mtu_id
					, MIN(ISNULL(taa_frequencia, 0)) frequencia
				FROM 
					CLS_TurmaAulaAluno Taa WITH(NOLOCK)
				INNER JOIN CLS_TurmaAula Tau WITH(NOLOCK)
					ON Tau.tud_id = Taa.tud_id
					AND Tau.tau_id = Taa.tau_id
					AND Tau.tpc_id = @tpc_id
					AND Tau.tau_situacao <> 3
				INNER JOIN tbTurmaDisciplina tud
					ON Tau.tud_id = tud.tud_id
				LEFT JOIN ACA_AlunoJustificativaFalta afj WITH(NOLOCK)
					ON Taa.alu_id = afj.alu_id
					AND afj.afj_situacao <> 3
					AND tau_data >= afj.afj_dataInicio
					AND ((afj.afj_dataFim  IS NULL) OR (tau_data <= afj.afj_dataFim))
				LEFT JOIN ACA_TipoJustificativaFalta tjf WITH(NOLOCK)
					ON tjf.tjf_id = afj.tjf_id
					AND tjf.tjf_situacao <> 3
				WHERE 
					Taa.taa_situacao <> 3
					AND ((tjf.tjf_abonaFalta IS NULL) OR (tjf.tjf_abonaFalta = 0))
				GROUP BY
					Tau.tau_data
					, Taa.alu_id
					, Taa.mtu_id
					, Taa.mtd_id
			)

			, tbQtdes AS
			(
				SELECT
				Mtd.alu_id
				, Mtd.mtu_id
				, Mtd.mtd_idAluno
				,
				-- Qt faltas do aluno.
				(
					SELECT SUM(Freq.frequencia)
					FROM tbFrequencia Freq
					WHERE 
						Freq.alu_id = Mtd.alu_id
						AND Freq.mtu_id = Mtd.mtu_id
				) AS QtFaltasAluno
				,
				-- Qt aulas do aluno.
				CASE  
					-- 1-Automático, 2-Manual
					WHEN @fav_calculoQtdeAulasDadas = 1 THEN NULL
					WHEN @tipoLancamento IN (1, 4) THEN -- 1-Aulas planejadas, 4-Aulas planejadas e mensal
						(SELECT TOP 1
							(dbo.FN_CalcularDiasUteis
							(CASE WHEN Cap.cap_dataInicio < Mtd.mtd_dataMatriculaAluno THEN Mtd.mtd_dataMatriculaAluno ELSE Cap.cap_dataInicio END
							, 
							CASE WHEN Cap.cap_dataFim > Mtd.mtd_dataSaidaAluno THEN Mtd.mtd_dataSaidaAluno ELSE Cap.cap_dataFim END
							, esc.ent_id, tur.cal_id)
							*
							CASE WHEN fav.fav_tipoApuracaoFrequencia = 1 -- Tempo de aula 
								THEN @crp_qtdeTemposDia 
								ELSE 1
							END)
						FROM TUR_Turma tur WITH(NOLOCK)
						INNER JOIN ACA_FormatoAvaliacao fav WITH(NOLOCK)
							ON  fav.fav_id = tur.fav_id
						INNER JOIN ESC_Escola esc WITH(NOLOCK)
							ON  esc.esc_id = tur.esc_id
						INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
							ON  Cap.cal_id = tur.cal_id
							AND Cap.tpc_id = @tpc_id
							AND Cap.cap_situacao <> 3
						WHERE 
							tur.tur_id = @tur_id
						)
					END AS QtAulasAluno
			FROM MatriculaTurmaDisciplinaMultisseriada Mtd 
			)

			INSERT INTO @TabelaQtdes(alu_id, mtu_id, mtd_id, qtFaltas, qtAulas)
			SELECT
				alu_id,
				mtu_id,
				mtd_idAluno,
				QtFaltasAluno,
				QtAulasAluno
			FROM tbQtdes
			GROUP BY 
				alu_id,
				mtu_id,
				mtd_idAluno,
				QtFaltasAluno,
				QtAulasAluno
		END
		ELSE
		BEGIN
			SELECT TOP 1 
				@crp_qtdeTemposDia = crp.crp_qtdeTemposDia, 
				@tur_id = turRtud.tur_id
			FROM TUR_TurmaRelTurmaDisciplina turRtud WITH(NOLOCK)
			INNER JOIN TUR_TurmaDisciplina AS tud WITH (NOLOCK)
				ON turRtud.tud_id = tud.tud_id
			INNER JOIN TUR_TurmaDisciplinaRelDisciplina disRtud WITH(NOLOCK)
				ON tud.tud_id = disRtud.tud_id
			INNER JOIN ACA_Disciplina dis WITH(NOLOCK)
				ON dis.dis_id = disRtud.dis_id
				AND dis.dis_situacao <> 3
			INNER JOIN TUR_TurmaCurriculo tcr WITH(NOLOCK)
				ON  tcr.tur_id = turRtud.tur_id
			INNER JOIN ACA_CurriculoPeriodo crp WITH(NOLOCK)
				ON crp.cur_id = tcr.cur_id
				AND crp.crr_id = tcr.crr_id
				AND crp.crp_id = tcr.crp_id
			WHERE 
				turRtud.tud_id = @tud_id
				AND tcr.tcr_situacao <> 3	

			; WITH tbFrequencia AS (
				SELECT 
					Taa.alu_id
					, Taa.mtu_id
					, Taa.mtd_id
					, MIN(ISNULL(taa_frequencia, 0)) frequencia
				FROM 
					CLS_TurmaAulaAluno Taa WITH(NOLOCK)
				INNER JOIN CLS_TurmaAula Tau WITH(NOLOCK)
					ON Tau.tud_id = Taa.tud_id
					AND Tau.tau_id = Taa.tau_id
					AND Tau.tau_situacao <> 3
				LEFT JOIN ACA_AlunoJustificativaFalta afj WITH(NOLOCK)
					ON Taa.alu_id = afj.alu_id
					AND afj.afj_situacao <> 3
					AND tau_data >= afj.afj_dataInicio
					AND ((afj.afj_dataFim  IS NULL) OR (tau_data <= afj.afj_dataFim))
				LEFT JOIN ACA_TipoJustificativaFalta tjf WITH(NOLOCK)
					ON tjf.tjf_id = afj.tjf_id
					AND tjf.tjf_situacao <> 3
				WHERE 
					Taa.taa_situacao <> 3
					AND Tau.tpc_id = @tpc_id
					AND Tau.tud_id = @tud_id
					AND ((tjf.tjf_abonaFalta IS NULL) OR (tjf.tjf_abonaFalta = 0))
				GROUP BY
					Tau.tau_data
					, Taa.alu_id
					, Taa.mtu_id
					, Taa.mtd_id
			)
		
			INSERT INTO @TabelaQtdes(alu_id, mtu_id, mtd_id, qtFaltas, qtAulas)
			SELECT
				Mtd.alu_id
				, Mtd.mtu_id
				, Mtd.mtd_id
				,
				-- Qt faltas do aluno.
				(
					SELECT SUM(Freq.frequencia)
					FROM tbFrequencia Freq
					WHERE 
						Freq.alu_id = Mtd.alu_id
						AND Freq.mtu_id = Mtd.mtu_id
						AND Freq.mtd_id = Mtd.mtd_id
				) AS QtFaltasAluno
				,
				-- Qt aulas do aluno.
				CASE  
					-- 1-Automático, 2-Manual
					WHEN @fav_calculoQtdeAulasDadas = 1 THEN NULL
					WHEN @tipoLancamento IN (1, 4) THEN -- 1-Aulas planejadas, 4-Aulas planejadas e mensal
						(SELECT TOP 1
							(dbo.FN_CalcularDiasUteis
							(CASE WHEN Cap.cap_dataInicio < Mtd.mtd_dataMatricula THEN Mtd.mtd_dataMatricula ELSE Cap.cap_dataInicio END
							, 
							CASE WHEN Cap.cap_dataFim > Mtd.mtd_dataSaida THEN Mtd.mtd_dataSaida ELSE Cap.cap_dataFim END
							, esc.ent_id, tur.cal_id)
							*
							CASE WHEN fav.fav_tipoApuracaoFrequencia = 1 -- Tempo de aula 
								THEN @crp_qtdeTemposDia 
								ELSE 1
							END)
						FROM TUR_Turma tur WITH(NOLOCK)
						INNER JOIN ACA_FormatoAvaliacao fav WITH(NOLOCK)
							ON  fav.fav_id = tur.fav_id
						INNER JOIN ESC_Escola esc WITH(NOLOCK)
							ON  esc.esc_id = tur.esc_id
						INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
							ON  Cap.cal_id = tur.cal_id
							AND Cap.tpc_id = @tpc_id
							AND Cap.cap_situacao <> 3
						WHERE 
							tur.tur_id = @tur_id
						)
					END AS QtAulasAluno
			FROM MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
			WHERE 
				(Mtd.tud_id = @tud_id)
				AND mtd_situacao IN (1,5)
		END
	END
	ELSE IF(@tipoLancamento = 5) -- 5-Aulas Dadas
	BEGIN
		IF (@tud_tipo = 15)
		BEGIN
			;WITH MatriculaTurmaDisciplinaMultisseriada AS
			(
				SELECT
					mtdDocente.alu_id,
					mtdDocente.mtu_id,
					mtdDocente.mtd_id AS mtd_idDocente,
					tudDocente.tud_id AS tud_idDocente,
					mtdAluno.mtd_id AS mtd_idAluno,
					tudAluno.tud_id AS tud_idAluno,
					mtdDocente.mtd_dataMatricula,
					mtdDocente.mtd_dataSaida
				FROM
					MTR_MatriculaTurmaDisciplina mtdDocente WITH(NOLOCK)
					INNER JOIN TUR_TurmaDisciplina tudDocente WITH(NOLOCK)
						ON tudDocente.tud_id = mtdDocente.tud_id
						AND tudDocente.tud_tipo = @tud_tipo
						AND tudDocente.tud_situacao <> 3
					INNER JOIN MTR_MatriculaTurmaDisciplina mtdAluno WITH(NOLOCK)
						ON mtdAluno.alu_id = mtdDocente.alu_id
						AND mtdAluno.mtu_id = mtdDocente.mtu_id
						AND mtdAluno.mtd_situacao IN (1,5)
					INNER JOIN TUR_TurmaDisciplina tudAluno WITH(NOLOCK)
						ON tudAluno.tud_id = mtdAluno.tud_id
						AND tudAluno.tud_tipo = 16
						AND tudAluno.tud_situacao <> 3
				WHERE 
					mtdDocente.tud_id = @tud_id
					AND mtdDocente.mtd_situacao IN (1,5)
				GROUP BY
					mtdDocente.alu_id,
					mtdDocente.mtu_id,
					mtdDocente.mtd_id,
					tudDocente.tud_id,
					mtdAluno.mtd_id,
					tudAluno.tud_id,
					mtdDocente.mtd_dataMatricula,
					mtdDocente.mtd_dataSaida
			)

			-- Insere todas as matrículas daquele aluno dentro do bimestre.
			INSERT INTO @tbAlunos
			(alu_id, mtu_id, mtd_id, tud_id, mtd_dataMatricula, mtd_dataSaida, tud_tipo, tud_idAluno, mtu_idOrigem, mtd_idOrigem)
			SELECT
				tdm.alu_id
				, tdm.mtu_id
				, tdm.mtd_idDocente
				, tdm.tud_idDocente
				, tdm.mtd_dataMatricula
				-- Traz a data de fim do bimestre caso não tenha saída (para filtrar as aulas).
				, ISNULL(tdm.mtd_dataSaida, DATEADD(DAY, 1, Cap.cap_dataFim)) AS mtd_dataSaida
				, @tud_tipo
				, tdm.tud_idAluno AS tud_idAluno
				, tdm.mtu_id, tdm.mtd_idAluno -- Matrícula do @tud_id
			FROM
				MatriculaTurmaDisciplinaMultisseriada tdm
				INNER JOIN TUR_TurmaRelTurmaDisciplina RelTud WITH(NOLOCK)
					ON RelTud.tud_id = tdm.tud_idDocente
				INNER JOIN TUR_Turma Tur WITH(NOLOCK)
					ON Tur.tur_id = RelTud.tur_id
				INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
					ON Cap.cal_id = Tur.cal_id
					AND Cap.tpc_id = @tpc_id
					AND Cap.cap_situacao <> 3
				
		END
		ELSE IF (@tud_tipo = 10)
		BEGIN
			-- para disciplinas eletivas desconsidera bimestres anteriores
			-- como a busca por matrículas em bimestres anteriores é feita por tipo de disciplina
			-- e as disciplinas eletivas no mesmo bimestre também possuem o mesmo tipo de disciplina
			-- a lógica para buscar bimestres anteriores quebra a regra para o bimestre corrente.
			
			-- Insere todas as matrículas daquele aluno dentro do bimestre.
			INSERT INTO @tbAlunos
			(alu_id, mtu_id, mtd_id, tud_id, mtd_dataMatricula, mtd_dataSaida, tud_tipo, tud_idRegencia, mtu_idOrigem, mtd_idOrigem)
			SELECT
				Mtd.alu_id
				, Mtd.mtu_id
				, Mtd.mtd_id
				, Mtd.tud_id
				, Mtd.mtd_dataMatricula
				-- Traz a data de fim do bimestre caso não tenha saída (para filtrar as aulas).
				, ISNULL(Mtd.mtd_dataSaida, DATEADD(DAY, 1, Cap.cap_dataFim)) AS mtd_dataSaida
				, tud.tud_tipo
				, 
				NULL AS tud_idRegencia
				, Mtd.mtu_id, Mtd.mtd_id -- Matrícula do @tud_id
			FROM MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
			INNER JOIN TUR_TurmaRelTurmaDisciplina RelTud WITH(NOLOCK)
				ON RelTud.tud_id = Mtd.tud_id
			INNER JOIN TUR_Turma Tur WITH(NOLOCK)
				ON Tur.tur_id = RelTud.tur_id
			INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
				ON Cap.cal_id = Tur.cal_id
				AND Cap.tpc_id = @tpc_id
				AND Cap.cap_situacao <> 3
			INNER JOIN TUR_TurmaDisciplina tud WITH(NOLOCK)
				ON Mtd.tud_id = tud.tud_id
				AND tud.tud_situacao <> 3
			WHERE
				Mtd.tud_id = @tud_id
				AND Mtd.mtd_situacao <> 3
		END 
		ELSE 
		BEGIN
			-- Insere todas as matrículas daquele aluno dentro do bimestre.
			INSERT INTO @tbAlunos
			(alu_id, mtu_id, mtd_id, tud_id, mtd_dataMatricula, mtd_dataSaida, tud_tipo, tud_idRegencia, mtu_idOrigem, mtd_idOrigem)
			SELECT
				MtdRelacionada.alu_id
				, MtdRelacionada.mtu_id
				, MtdRelacionada.mtd_id
				, MtdRelacionada.tud_id
				, MtdRelacionada.mtd_dataMatricula
				-- Traz a data de fim do bimestre caso não tenha saída (para filtrar as aulas).
				, ISNULL(MtdRelacionada.mtd_dataSaida, DATEADD(DAY, 1, Cap.cap_dataFim)) AS mtd_dataSaida
				, TudRelacionada.tud_tipo
				, 
				(
					-- Quando a disciplina for complementação da regência, trazer o tud_id da regência.
					SELECT TudRegencia.tud_id
					FROM TUR_TurmaRelTurmaDisciplina RelTudComplRegencia WITH(NOLOCK)
					INNER JOIN TUR_TurmaRelTurmaDisciplina RelTudRegencia WITH(NOLOCK)
						ON RelTudRegencia.tur_id = RelTudComplRegencia.tur_id
					INNER JOIN TUR_TurmaDisciplina TudRegencia WITH(NOLOCK)
						ON TudRegencia.tud_id = RelTudRegencia.tud_id
						AND TudRegencia.tud_situacao <> 3
						AND TudRegencia.tud_tipo = 11 -- 11-Regência
					WHERE
						RelTudComplRegencia.tud_id = MtdRelacionada.tud_id
						AND TudRelacionada.tud_tipo = 13 -- 13-Complementação de regência (trazer o tud_id da regência)
				) AS tud_idRegencia
				, Mtd.mtu_id, Mtd.mtd_id -- Matrícula do @tud_id
			FROM MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
			INNER JOIN TUR_TurmaRelTurmaDisciplina RelTud WITH(NOLOCK)
				ON RelTud.tud_id = Mtd.tud_id
			INNER JOIN TUR_Turma Tur WITH(NOLOCK)
				ON Tur.tur_id = RelTud.tur_id
			INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
				ON Cap.cal_id = Tur.cal_id
				AND Cap.tpc_id = @tpc_id
				AND Cap.cap_situacao <> 3
			INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDis WITH(NOLOCK)
				ON RelDis.tud_id = Mtd.tud_id
			INNER JOIN ACA_Disciplina Dis WITH(NOLOCK)
				ON Dis.dis_id = RelDis.dis_id
				AND Dis.dis_situacao <> 3
			INNER JOIN ACA_Disciplina DisRelacionada WITH(NOLOCK)
				ON DisRelacionada.tds_id = Dis.tds_id
				AND Dis.dis_situacao <> 3
			INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDisRelacionada WITH(NOLOCK)
				ON RelDisRelacionada.dis_id = DisRelacionada.dis_id
			INNER JOIN TUR_TurmaDisciplina TudRelacionada WITH(NOLOCK)
				ON TudRelacionada.tud_id = RelDisRelacionada.tud_id
				AND TudRelacionada.tud_situacao <> 3
			--
			INNER JOIN TUR_TurmaRelTurmaDisciplina RelTurmaDisciplina WITH(NOLOCK)
				ON RelTurmaDisciplina.tud_id = TudRelacionada.tud_id
			INNER JOIN TUR_Turma TurRelacionada WITH(NOLOCK)
				ON TurRelacionada.tur_id = RelTurmaDisciplina.tur_id
				AND TurRelacionada.cal_id = @cal_id	
				AND TurRelacionada.tur_situacao <> 3	
			--		
			INNER JOIN MTR_MatriculaTurmaDisciplina MtdRelacionada WITH(NOLOCK)
				ON MtdRelacionada.alu_id = Mtd.alu_id
				AND MtdRelacionada.tud_id = TudRelacionada.tud_id
				AND MtdRelacionada.mtd_situacao IN (1,5)
				-- Só matrículas dentro do bimestre.
				AND Mtd.mtd_dataMatricula <= Cap.cap_dataFim
				AND (Mtd.mtd_situacao = 1 OR Mtd.mtd_dataSaida > Cap.cap_dataInicio)
			WHERE
				Mtd.tud_id = @tud_id
				AND Mtd.mtd_situacao <> 3
		END

		IF (@tipoDocente = 5)
		BEGIN
			INSERT INTO @tbPosicaoDocente (tdc_posicao)
			SELECT tdc_posicao 
			FROM ACA_TipoDocente TDC WITH(NOLOCK)
			WHERE
				TDC.tdc_id = 5 
				AND TDC.tdc_situacao <> 3
		END
		ELSE
		BEGIN
			INSERT INTO @tbPosicaoDocente (tdc_posicao)
			SELECT tdc_posicao 
			FROM ACA_TipoDocente TDC WITH(NOLOCK)
			WHERE
				TDC.tdc_id IN (1,4,6)
				AND TDC.tdc_situacao <> 3
		END
		
		SELECT
			@qtdeTitulares = COUNT(tdt.tdt_id)
		FROM
			TUR_TurmaDocente tdt WITH(NOLOCK)
			INNER JOIN ACA_TipoDocente tdc WITH(NOLOCK)
				ON tdt.tdt_posicao = tdc.tdc_posicao
				AND tdc.tdc_id IN (1,6)
		WHERE
			tdt.tud_id = @tud_id
			AND tdt.tdt_situacao = 1
		
		IF (ISNULL(@tud_tipo, 0) = 11 AND @qtdeTitulares = 2)
		BEGIN
			;WITH tbQtdeAulaFalta AS
			(
				SELECT
					ROW_NUMBER() OVER (PARTITION BY Tau.tau_data, Mtr.alu_id, Mtr.mtu_id, Mtr.mtd_id ORDER BY Tau.tdt_posicao) AS numLinha,
					Mtr.alu_id,
					Mtr.mtu_id,
					Mtr.mtd_id,
					Tau.tau_data,
					Tau.tdt_posicao,
					SUM(ISNULL(Tau.tau_numeroAulas, 0)) AS qtAulas,
					SUM(ISNULL(Taa.taa_frequencia, 0)) AS qtFaltas
				FROM
					@tbAlunos Mtr
					LEFT JOIN CLS_TurmaAula Tau WITH(NOLOCK)
						ON Tau.tud_id = Mtr.tud_id
						AND Tau.tpc_id = @tpc_id
						AND Tau.tau_data >= Mtr.mtd_dataMatricula AND Tau.tau_data < Mtr.mtd_dataSaida
						AND Tau.tau_situacao <> 3
						AND Tau.tdt_posicao IN (SELECT tdc_posicao FROM @tbPosicaoDocente Tdc)
					LEFT JOIN CLS_TurmaAulaAluno Taa WITH(NOLOCK)
						ON Taa.tud_id = Tau.tud_id
						AND Taa.tau_id = Tau.tau_id
						AND Taa.alu_id = Mtr.alu_id
						AND Taa.mtu_id = Mtr.mtu_id
						AND Taa.mtd_id = Mtr.mtd_id
						AND Taa.taa_situacao <> 3
				GROUP BY
					Mtr.alu_id,
					Mtr.mtu_id,
					Mtr.mtd_id,
					Tau.tau_data,
					Tau.tdt_posicao
			) 
			
			, tbQtdeAula AS 
			(
				SELECT
					alu_id,
					mtu_id,
					mtd_id,
					SUM(ISNULL(qtAulas,0)) AS qtAulas
				FROM
					tbQtdeAulaFalta
				WHERE
					numLinha = 1
				GROUP BY
					alu_id,
					mtu_id,
					mtd_id
			)
			
			, tbQtdeFalta AS 
			(
				SELECT
					alu_id,
					mtu_id,
					mtd_id,
					CASE WHEN SUM(qtAulas) = SUM(qtFaltas) THEN 1 ELSE 0 END AS qtFaltas
				FROM
					tbQtdeAulaFalta
				GROUP BY
					alu_id,
					mtu_id,
					mtd_id,
					tau_data
			)
			
			INSERT INTO @TabelaQtdes
			(alu_id, mtu_id, mtd_id, qtAulas, qtFaltas)
			SELECT
				tqa.alu_id,
				tqa.mtu_id,
				tqa.mtd_id,
				tqa.qtAulas,
				SUM(tqf.qtFaltas) AS qtFaltas
			FROM
				tbQtdeAula tqa
				INNER JOIN tbQtdeFalta tqf
					ON tqa.alu_id = tqf.alu_id
					AND tqa.mtu_id = tqf.mtu_id
					AND tqa.mtd_id = tqf.mtd_id
			GROUP BY
				tqa.alu_id,
				tqa.mtu_id,
				tqa.mtd_id,
				tqa.qtAulas
		END
		ELSE
		IF (@tud_tipo = 15)
		BEGIN
			;WITH tbTurmaDisciplina AS (
				SELECT Mtr.tud_idAluno AS tud_id
				FROM @tbAlunos MTR
				GROUP BY Mtr.tud_idAluno

				UNION

				SELECT @tud_id AS tud_id
			)

			, tbFrequencia AS
			(
				SELECT
					Mtr.alu_id, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
					, ISNULL(SUM(Tau.tau_numeroAulas), 0) AS QtAulas
					, SUM(ISNULL(Taa.taa_frequencia, 0)) AS QtFaltas
					--, Tau.*
				FROM @tbAlunos Mtr
				INNER JOIN MTR_MatriculaTurmaDisciplina mtd WITH(NOLOCK)
					ON mtr.alu_id = mtd.alu_id
					AND mtr.mtu_id = mtd.mtu_id
					AND mtr.mtd_id = mtd.mtd_id
					AND mtd.mtd_situacao <> 3
				INNER JOIN TUR_TurmaRelTurmaDisciplina RelTud WITH(NOLOCK)
					ON RelTud.tud_id = Mtd.tud_id
				INNER JOIN TUR_Turma Tur WITH(NOLOCK)
					ON Tur.tur_id = RelTud.tur_id
				INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
					ON Cap.cal_id = Tur.cal_id
					AND Cap.tpc_id = @tpc_id
					AND Cap.cap_situacao <> 3
				LEFT JOIN CLS_TurmaAula Tau WITH(NOLOCK)
					ON Tau.tud_id = mtd.tud_id
					AND Tau.tpc_id = @tpc_id
					AND Tau.tau_data >= mtd.mtd_dataMatricula AND Tau.tau_data < ISNULL(mtd.mtd_dataSaida, DATEADD(DAY, 1, Cap.cap_dataFim))
					AND Tau.tau_situacao <> 3
					AND Tau.tdt_posicao IN (SELECT tdc_posicao FROM @tbPosicaoDocente Tdc)
				LEFT JOIN CLS_TurmaAulaAluno Taa WITH(NOLOCK)
					ON Taa.tud_id = Tau.tud_id
					AND Taa.tau_id = Tau.tau_id
					AND Taa.alu_id = mtd.alu_id
					AND Taa.mtu_id = mtd.mtu_id
					AND Taa.mtd_id = mtd.mtd_id
					AND Taa.taa_situacao <> 3
				GROUP BY
					Mtr.alu_id, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem

				UNION

				SELECT
					Mtr.alu_id, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
					, ISNULL(SUM(Tau.tau_numeroAulas), 0) AS QtAulas
					, SUM(ISNULL(Taa.taa_frequencia, 0)) AS QtFaltas
					--, Tau.*
				FROM @tbAlunos Mtr
				INNER JOIN MTR_MatriculaTurmaDisciplina mtd WITH(NOLOCK)
					ON mtr.alu_id = mtd.alu_id
					AND mtr.mtu_id = mtd.mtu_id
					AND mtr.mtd_idOrigem = mtd.mtd_id
					AND mtd.mtd_situacao <> 3
				INNER JOIN TUR_TurmaRelTurmaDisciplina RelTud WITH(NOLOCK)
					ON RelTud.tud_id = Mtd.tud_id
				INNER JOIN TUR_Turma Tur WITH(NOLOCK)
					ON Tur.tur_id = RelTud.tur_id
				INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
					ON Cap.cal_id = Tur.cal_id
					AND Cap.tpc_id = @tpc_id
					AND Cap.cap_situacao <> 3
				LEFT JOIN CLS_TurmaAula Tau WITH(NOLOCK)
					ON Tau.tud_id = mtd.tud_id
					AND Tau.tpc_id = @tpc_id
					AND Tau.tau_data >= mtd.mtd_dataMatricula AND Tau.tau_data < ISNULL(mtd.mtd_dataSaida, DATEADD(DAY, 1, Cap.cap_dataFim))
					AND Tau.tau_situacao <> 3
					AND Tau.tdt_posicao IN (SELECT tdc_posicao FROM @tbPosicaoDocente Tdc)
				LEFT JOIN CLS_TurmaAulaAluno Taa WITH(NOLOCK)
					ON Taa.tud_id = Tau.tud_id
					AND Taa.tau_id = Tau.tau_id
					AND Taa.alu_id = mtd.alu_id
					AND Taa.mtu_id = mtd.mtu_id
					AND Taa.mtd_id = mtd.mtd_id
					AND Taa.taa_situacao <> 3
				GROUP BY
					Mtr.alu_id, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
			)

			INSERT INTO @TabelaQtdes
			(alu_id, mtu_id, mtd_id, qtAulas, qtFaltas)
			SELECT
				Mtr.alu_id, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
				, ISNULL(SUM(Mtr.QtAulas), 0) AS QtAulas
				, SUM(ISNULL(Mtr.QtFaltas, 0)) AS QtFaltas
				--, Tau.*
			FROM tbFrequencia Mtr
			GROUP BY Mtr.alu_id, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
		END
		ELSE IF (@tud_tipo = 18)--Experiencia
		BEGIN
			-- Aulas da disciplina do aluno.
			INSERT INTO @TabelaQtdes
			(alu_id, mtu_id, mtd_id, qtAulas, qtFaltas)
			SELECT
				Mtr.alu_id, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
				, ISNULL(SUM(Tau.tau_numeroAulas), 0) AS QtAulas
				, SUM(ISNULL(Taa.taa_frequencia, 0)) AS QtFaltas
				--, Tau.*
			FROM @tbAlunos Mtr
			INNER JOIN TUR_TurmaDisciplinaTerritorio TRT WITH(NOLOCK)
				ON Mtr.tud_id = TRT.tud_idExperiencia
			LEFT JOIN CLS_TurmaAulaTerritorio TAT WITH(NOLOCK)
				ON Mtr.tud_id = TAT.tud_idExperiencia
			LEFT JOIN CLS_TurmaAula TAU WITH(NOLOCK)
				ON	TAU.tud_id = TRT.tud_idTerritorio
				AND TAU.tau_id = TAT.tau_idTerritorio
				AND TAU.tud_id = TAT.tud_idTerritorio
				AND TAU.tau_data >= TRT.tte_vigenciaInicio
				AND TAU.tau_data <= ISNULL(TRT.tte_vigenciaFim, TAU.tau_data)
				AND Tau.tpc_id = @tpc_id
				AND Tau.tau_data >= Mtr.mtd_dataMatricula AND Tau.tau_data < Mtr.mtd_dataSaida
				AND Tau.tau_situacao <> 3
				AND Tau.tdt_posicao IN (SELECT tdc_posicao FROM @tbPosicaoDocente Tdc)
			LEFT JOIN CLS_TurmaAulaAluno Taa WITH(NOLOCK)
				ON Taa.tud_id = Tau.tud_id
				AND Taa.tau_id = Tau.tau_id
				AND Taa.alu_id = Mtr.alu_id
				AND Taa.mtu_id = Mtr.mtu_id
				--AND Taa.mtd_id = Mtr.mtd_id
				AND Taa.taa_situacao <> 3
			WHERE
				Mtr.tud_tipo = 18 --Experiencia
			GROUP BY
				Mtr.alu_id, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
		END
		ELSE
		BEGIN
			-- Aulas da disciplina do aluno.
			INSERT INTO @TabelaQtdes
			(alu_id, mtu_id, mtd_id, qtAulas, qtFaltas)
			SELECT
				Mtr.alu_id, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
				, ISNULL(SUM(Tau.tau_numeroAulas), 0) AS QtAulas
				, SUM(ISNULL(Taa.taa_frequencia, 0)) AS QtFaltas
				--, Tau.*
			FROM @tbAlunos Mtr
			LEFT JOIN CLS_TurmaAula Tau WITH(NOLOCK)
				ON Tau.tud_id = Mtr.tud_id
				AND Tau.tpc_id = @tpc_id
				AND Tau.tau_data >= Mtr.mtd_dataMatricula AND Tau.tau_data < Mtr.mtd_dataSaida
				AND Tau.tau_situacao <> 3
				AND Tau.tdt_posicao IN (SELECT tdc_posicao FROM @tbPosicaoDocente Tdc)
			LEFT JOIN CLS_TurmaAulaAluno Taa WITH(NOLOCK)
				ON Taa.tud_id = Tau.tud_id
				AND Taa.tau_id = Tau.tau_id
				AND Taa.alu_id = Mtr.alu_id
				AND Taa.mtu_id = Mtr.mtu_id
				AND Taa.mtd_id = Mtr.mtd_id
				AND Taa.taa_situacao <> 3
			WHERE
				Mtr.tud_tipo NOT IN (13, 16) -- 13-Complementação de regência 
											 -- 16-Multisseriada do aluno
			GROUP BY
				Mtr.alu_id, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
			
			; WITH MatriculasIngles AS
			(
				SELECT
					Mtr.alu_id
					, Mtr.mtu_id
					, Mtd.mtd_id -- Traz o mtd da disciplina Complementação de regência dele na turma.
					, Mtr.tud_id
					, Mtr.mtd_dataMatricula
					, Mtr.mtd_dataSaida
					, Mtr.tud_tipo, Mtr.tud_idRegencia, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
				FROM @tbAlunos Mtr
				INNER JOIN MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
					ON Mtd.alu_id = Mtr.alu_id
					AND Mtd.mtu_id = Mtr.mtu_id
					AND Mtd.tud_id = Mtr.tud_idRegencia
					AND Mtd.mtd_situacao <> 3
			)
			-- Aulas da disciplina Complementação de regência do aluno.
			INSERT INTO @TabelaQtdes
			(alu_id, mtu_id, mtd_id, qtAulas, qtFaltas)
			SELECT
				Mtr.alu_id, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
				, ISNULL(SUM(Tau.tau_numeroAulas), 0) AS QtAulas
				-- Quantidade de aulas do Inglês * quantidade de faltas da regência (no mesmo dia).
				, SUM(ISNULL(Tau.tau_numeroAulas, 0) * ISNULL(TaaRegencia.taa_frequencia, 0)) AS QtFaltas
				--, Tau.*
			FROM MatriculasIngles Mtr
			LEFT JOIN CLS_TurmaAula Tau WITH(NOLOCK)
				ON Tau.tud_id = Mtr.tud_id
				AND Tau.tpc_id = @tpc_id
				AND Tau.tau_data >= Mtr.mtd_dataMatricula AND Tau.tau_data < Mtr.mtd_dataSaida
				AND Tau.tau_situacao <> 3
				AND Tau.tdt_posicao IN (SELECT tdc_posicao FROM @tbPosicaoDocente Tdc)
			LEFT JOIN CLS_TurmaAula TauRegencia WITH(NOLOCK)
				ON TauRegencia.tud_id = Mtr.tud_idRegencia
				AND TauRegencia.tau_data = Tau.tau_data
				AND TauRegencia.tau_situacao <> 3
				AND TauRegencia.tdt_posicao IN (SELECT tdc_posicao FROM @tbPosicaoDocente Tdc)
			LEFT JOIN CLS_TurmaAulaAluno TaaRegencia WITH(NOLOCK)
				ON TaaRegencia.tud_id = TauRegencia.tud_id
				AND TaaRegencia.tau_id = TauRegencia.tau_id -- Busca a quantidade de faltas da regência.
				AND TaaRegencia.alu_id = Mtr.alu_id
				AND TaaRegencia.mtu_id = Mtr.mtu_id
				AND TaaRegencia.mtd_id = Mtr.mtd_id
				AND TaaRegencia.taa_situacao <> 3
			WHERE
				Mtr.tud_tipo = 13 -- 13-Complementação de regência
				AND NOT EXISTS (
					SELECT 1 
					FROM @TabelaQtdes Qtd 
					WHERE 
						Mtr.alu_id = Qtd.alu_id
						AND Mtr.mtu_idOrigem = Qtd.mtu_id
						AND Mtr.mtd_idOrigem = Qtd.mtd_id 
				)
			GROUP BY
				Mtr.alu_id, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
		END
	END
	ELSE IF(@tipoLancamento = 6) -- 6-Aulas previstas do docente
	BEGIN
		DECLARE @tpc_ordem INT;
		
		SELECT TOP 1
			@tpc_ordem = tpc.tpc_ordem
		FROM
			ACA_TipoPeriodoCalendario tpc WITH(NOLOCK)
		WHERE
			tpc.tpc_id = @tpc_id
			AND tpc.tpc_situacao <> 3
		
		IF (@tud_tipo = 15) --Gerando @tbAlunos multiseriada do docente
		BEGIN
			;WITH MatriculaTurmaDisciplinaMultisseriada AS
			(
				SELECT
					mtdDocente.alu_id,
					mtdDocente.mtu_id,
					mtdDocente.mtd_id AS mtd_idDocente,
					tudDocente.tud_id AS tud_idDocente,
					mtdAluno.mtd_id AS mtd_idAluno,
					tudAluno.tud_id AS tud_idAluno,
					mtdDocente.mtd_dataMatricula,
					mtdDocente.mtd_dataSaida
				FROM
					MTR_MatriculaTurmaDisciplina mtdDocente WITH(NOLOCK)
					INNER JOIN TUR_TurmaDisciplina tudDocente WITH(NOLOCK)
						ON tudDocente.tud_id = mtdDocente.tud_id
						AND tudDocente.tud_tipo = @tud_tipo
						AND tudDocente.tud_situacao <> 3
					INNER JOIN MTR_MatriculaTurmaDisciplina mtdAluno WITH(NOLOCK)
						ON mtdAluno.alu_id = mtdDocente.alu_id
						AND mtdAluno.mtu_id = mtdDocente.mtu_id
						AND mtdAluno.mtd_situacao IN (1,5)
					INNER JOIN TUR_TurmaDisciplina tudAluno WITH(NOLOCK)
						ON tudAluno.tud_id = mtdAluno.tud_id
						AND tudAluno.tud_tipo = 16
						AND tudAluno.tud_situacao <> 3
				WHERE 
					mtdDocente.tud_id = @tud_id
					AND mtdDocente.mtd_situacao IN (1,5)
			)

			-- Insere todas as matrículas daquele aluno dentro do bimestre.
			INSERT INTO @tbAlunos
			(alu_id, mtu_id, mtd_id, tud_id, mtd_dataMatricula, mtd_dataSaida, tud_tipo, tud_idAluno, mtu_idOrigem, mtd_idOrigem)
			SELECT
				tdm.alu_id
				, tdm.mtu_id
				, tdm.mtd_idDocente
				, tdm.tud_idDocente
				, tdm.mtd_dataMatricula
				-- Traz a data de fim do bimestre caso não tenha saída (para filtrar as aulas).
				, ISNULL(tdm.mtd_dataSaida, DATEADD(DAY, 1, Cap.cap_dataFim)) AS mtd_dataSaida
				, @tud_tipo
				, tdm.tud_idAluno AS tud_idAluno
				, tdm.mtu_id, tdm.mtd_idAluno -- Matrícula do @tud_id
			FROM
				MatriculaTurmaDisciplinaMultisseriada tdm
				INNER JOIN TUR_TurmaRelTurmaDisciplina RelTud WITH(NOLOCK)
					ON RelTud.tud_id = tdm.tud_idDocente
				INNER JOIN TUR_Turma Tur WITH(NOLOCK)
					ON Tur.tur_id = RelTud.tur_id
				INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
					ON Cap.cal_id = Tur.cal_id
					AND Cap.tpc_id = @tpc_id
					AND Cap.cap_situacao <> 3
			GROUP BY
				tdm.alu_id
				, tdm.mtu_id
				, tdm.mtd_idDocente
				, tdm.tud_idDocente
				, tdm.mtd_dataMatricula
				, ISNULL(tdm.mtd_dataSaida, DATEADD(DAY, 1, Cap.cap_dataFim))
				, tdm.tud_idAluno 
				, tdm.mtu_id
				, tdm.mtd_idAluno


		END
		ELSE IF (@tud_tipo = 10) --Gerando @tbAlunos para eletivas
		BEGIN
			-- para disciplinas eletivas desconsidera bimestres anteriores
			-- como a busca por matrículas em bimestres anteriores é feita por tipo de disciplina
			-- e as disciplinas eletivas no mesmo bimestre também possuem o mesmo tipo de disciplina
			-- a lógica para buscar bimestres anteriores quebra a regra para o bimestre corrente.
			
			-- Insere todas as matrículas daquele aluno dentro do bimestre.
			INSERT INTO @tbAlunos
			(alu_id, mtu_id, mtd_id, tud_id, mtd_dataMatricula, mtd_dataSaida, tud_tipo, tud_idRegencia, mtu_idOrigem, mtd_idOrigem)
			SELECT
				Mtd.alu_id
				, Mtd.mtu_id
				, Mtd.mtd_id
				, Mtd.tud_id
				, Mtd.mtd_dataMatricula
				-- Traz a data de fim do bimestre caso não tenha saída (para filtrar as aulas).
				, ISNULL(Mtd.mtd_dataSaida, DATEADD(DAY, 1, Cap.cap_dataFim)) AS mtd_dataSaida
				, tud.tud_tipo
				, 
				NULL AS tud_idRegencia
				, Mtd.mtu_id, Mtd.mtd_id -- Matrícula do @tud_id
			FROM MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
			INNER JOIN TUR_TurmaRelTurmaDisciplina RelTud WITH(NOLOCK)
				ON RelTud.tud_id = Mtd.tud_id
			INNER JOIN TUR_Turma Tur WITH(NOLOCK)
				ON Tur.tur_id = RelTud.tur_id
			INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
				ON Cap.cal_id = Tur.cal_id
				AND Cap.tpc_id = @tpc_id
				AND Cap.cap_situacao <> 3
			INNER JOIN TUR_TurmaDisciplina tud WITH(NOLOCK)
				ON Mtd.tud_id = tud.tud_id
				AND tud.tud_situacao <> 3
			WHERE
				Mtd.tud_id = @tud_id
				AND Mtd.mtd_situacao <> 3
				AND Mtd.mtd_dataMatricula <= Cap.cap_dataFim
				AND (Mtd.mtd_situacao = 1 OR Mtd.mtd_dataSaida > Cap.cap_dataInicio)
		END 
		ELSE IF (@tud_tipo = 13) --Gerando @tbAlunos para complementação de regencia
		BEGIN
			-- Insere todas as matrículas daquele aluno dentro do bimestre.
			INSERT INTO @tbAlunos
			(alu_id, mtu_id, mtd_id, tud_id, mtd_dataMatricula, mtd_dataSaida, tud_tipo, tud_idRegencia, mtu_idOrigem, mtd_idOrigem)
			SELECT
				MtdRelacionada.alu_id
				, MtdRelacionada.mtu_id
				, MtdRelacionada.mtd_id
				, MtdRelacionada.tud_id
				, MtdRelacionada.mtd_dataMatricula
				-- Traz a data de fim do bimestre caso não tenha saída (para filtrar as aulas).
				, ISNULL(MtdRelacionada.mtd_dataSaida, DATEADD(DAY, 1, Cap.cap_dataFim)) AS mtd_dataSaida
				, TudRelacionada.tud_tipo
				, 
				(
					-- Quando a disciplina for complementação da regência, trazer o tud_id da regência.
					SELECT TudRegencia.tud_id
					FROM TUR_TurmaRelTurmaDisciplina RelTudComplRegencia WITH(NOLOCK)
					INNER JOIN TUR_TurmaRelTurmaDisciplina RelTudRegencia WITH(NOLOCK)
						ON RelTudRegencia.tur_id = RelTudComplRegencia.tur_id
					INNER JOIN TUR_TurmaDisciplina TudRegencia WITH(NOLOCK)
						ON TudRegencia.tud_id = RelTudRegencia.tud_id
						AND TudRegencia.tud_situacao <> 3
						AND TudRegencia.tud_tipo = 11 -- 11-Regência
					WHERE
						RelTudComplRegencia.tud_id = MtdRelacionada.tud_id
						AND TudRelacionada.tud_tipo = 13 -- 13-Complementação de regência (trazer o tud_id da regência)
				) AS tud_idRegencia
				, Mtd.mtu_id, Mtd.mtd_id -- Matrícula do @tud_id
			FROM MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
			INNER JOIN TUR_TurmaRelTurmaDisciplina RelTud WITH(NOLOCK)
				ON RelTud.tud_id = Mtd.tud_id
			INNER JOIN TUR_Turma Tur WITH(NOLOCK)
				ON Tur.tur_id = RelTud.tur_id
			INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
				ON Cap.cal_id = Tur.cal_id
				AND Cap.tpc_id = @tpc_id
				AND Cap.cap_situacao <> 3
			INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDis WITH(NOLOCK)
				ON RelDis.tud_id = Mtd.tud_id
			INNER JOIN ACA_Disciplina Dis WITH(NOLOCK)
				ON Dis.dis_id = RelDis.dis_id
				AND Dis.dis_situacao <> 3
			INNER JOIN ACA_Disciplina DisRelacionada WITH(NOLOCK)
				ON DisRelacionada.tds_id = Dis.tds_id
				AND Dis.dis_situacao <> 3
			INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDisRelacionada WITH(NOLOCK)
				ON RelDisRelacionada.dis_id = DisRelacionada.dis_id
			INNER JOIN TUR_TurmaDisciplina TudRelacionada WITH(NOLOCK)
				ON TudRelacionada.tud_id = RelDisRelacionada.tud_id
				AND TudRelacionada.tud_situacao <> 3
			--
			INNER JOIN TUR_TurmaRelTurmaDisciplina RelTurmaDisciplina WITH(NOLOCK)
				ON RelTurmaDisciplina.tud_id = TudRelacionada.tud_id
			INNER JOIN TUR_Turma TurRelacionada WITH(NOLOCK)
				ON TurRelacionada.tur_id = RelTurmaDisciplina.tur_id
				AND TurRelacionada.cal_id = @cal_id	
				AND TurRelacionada.tur_situacao <> 3	
			--		
			INNER JOIN MTR_MatriculaTurmaDisciplina MtdRelacionada WITH(NOLOCK)
				ON MtdRelacionada.alu_id = Mtd.alu_id
				AND MtdRelacionada.tud_id = TudRelacionada.tud_id
				AND MtdRelacionada.mtd_situacao IN (1,5)
				-- Só matrículas dentro do bimestre.
				AND Mtd.mtd_dataMatricula <= Cap.cap_dataFim
				AND (Mtd.mtd_situacao = 1 OR Mtd.mtd_dataSaida > Cap.cap_dataInicio)
			WHERE
				Mtd.tud_id = @tud_id
				AND Mtd.mtd_situacao <> 3
		END 
		ELSE --Gerando @tbAlunos para todos os outros tipos de disciplinas (exceto multiseriada do docente, eletivas)
		BEGIN
			-- Insere todas as matrículas daquele aluno dentro do bimestre.
			INSERT INTO @tbAlunos
			(alu_id, mtu_id, mtd_id, tud_id, mtd_dataMatricula, mtd_dataSaida, tud_tipo, tud_idRegencia, mtu_idOrigem, mtd_idOrigem)
			SELECT
				MtdRelacionada.alu_id
				, MtdRelacionada.mtu_id
				, MtdRelacionada.mtd_id
				, MtdRelacionada.tud_id
				, MtdRelacionada.mtd_dataMatricula
				-- Traz a data de fim do bimestre caso não tenha saída (para filtrar as aulas).
				, ISNULL(MtdRelacionada.mtd_dataSaida, DATEADD(DAY, 1, Cap.cap_dataFim)) AS mtd_dataSaida
				, TudRelacionada.tud_tipo
				, NULL AS tud_idRegencia
				, Mtd.mtu_id, Mtd.mtd_id -- Matrícula do @tud_id
			FROM MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
			INNER JOIN TUR_TurmaRelTurmaDisciplina RelTud WITH(NOLOCK)
				ON RelTud.tud_id = Mtd.tud_id
			INNER JOIN TUR_Turma Tur WITH(NOLOCK)
				ON Tur.tur_id = RelTud.tur_id
			INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
				ON Cap.cal_id = Tur.cal_id
				AND Cap.tpc_id = @tpc_id
				AND Cap.cap_situacao <> 3
			INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDis WITH(NOLOCK)
				ON RelDis.tud_id = Mtd.tud_id
			INNER JOIN ACA_Disciplina Dis WITH(NOLOCK)
				ON Dis.dis_id = RelDis.dis_id
				AND Dis.dis_situacao <> 3
			INNER JOIN ACA_Disciplina DisRelacionada WITH(NOLOCK)
				ON DisRelacionada.tds_id = Dis.tds_id
				AND Dis.dis_situacao <> 3
			INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDisRelacionada WITH(NOLOCK)
				ON RelDisRelacionada.dis_id = DisRelacionada.dis_id
			INNER JOIN TUR_TurmaDisciplina TudRelacionada WITH(NOLOCK)
				ON TudRelacionada.tud_id = RelDisRelacionada.tud_id
				AND TudRelacionada.tud_situacao <> 3
			--
			INNER JOIN TUR_TurmaRelTurmaDisciplina RelTurmaDisciplina WITH(NOLOCK)
				ON RelTurmaDisciplina.tud_id = TudRelacionada.tud_id
			INNER JOIN TUR_Turma TurRelacionada WITH(NOLOCK)
				ON TurRelacionada.tur_id = RelTurmaDisciplina.tur_id
				AND TurRelacionada.cal_id = @cal_id	
				AND TurRelacionada.tur_situacao <> 3	
			--		
			INNER JOIN MTR_MatriculaTurmaDisciplina MtdRelacionada WITH(NOLOCK)
				ON MtdRelacionada.alu_id = Mtd.alu_id
				AND MtdRelacionada.tud_id = TudRelacionada.tud_id
				AND MtdRelacionada.mtd_situacao IN (1,5)
				-- Só matrículas dentro do bimestre.
				AND Mtd.mtd_dataMatricula <= Cap.cap_dataFim
				AND (Mtd.mtd_situacao = 1 OR Mtd.mtd_dataSaida > Cap.cap_dataInicio)
			WHERE
				Mtd.tud_id = @tud_id
				AND Mtd.mtd_situacao <> 3
		
		END --FIM DO IF que preenchia a @tbAlunos
		
		IF (@tipoDocente = 5)
		BEGIN
			INSERT INTO @tbPosicaoDocente (tdc_posicao)
			SELECT tdc_posicao 
			FROM ACA_TipoDocente TDC WITH(NOLOCK)
			WHERE
				TDC.tdc_id = 5 
				AND TDC.tdc_situacao <> 3
		END
		ELSE
		BEGIN
			INSERT INTO @tbPosicaoDocente (tdc_posicao)
			SELECT tdc_posicao 
			FROM ACA_TipoDocente TDC WITH(NOLOCK)
			WHERE
				TDC.tdc_id IN (1,4,6)
				AND TDC.tdc_situacao <> 3
		END
		
		SELECT
			@qtdeTitulares = COUNT(tdt.tdt_id)
		FROM
			TUR_TurmaDocente tdt WITH(NOLOCK)
			INNER JOIN ACA_TipoDocente tdc WITH(NOLOCK)
				ON tdt.tdt_posicao = tdc.tdc_posicao
				AND tdc.tdc_id IN (1,6)
		WHERE
			tdt.tud_id = @tud_id
			AND tdt.tdt_situacao = 1

		DECLARE @qtdeAulas INT;
		
		-- Pedro Silva 29/07/2015
		-- a quantidade de aulas ( @qtdeAulas )para este tipo de lançamento de frequência é calculado pelas aulas previstas
		-- porém, as quantidades de aulas normais e de reposição que serão calculadas mais abaixo baseada na CLS_TurmaAula mesmo
		
		IF (@tud_tipo = 15)
		BEGIN
			;WITH tbTurmaDisciplina AS (
				SELECT Mtr.tud_idAluno AS tud_id
				FROM @tbAlunos MTR
				GROUP BY Mtr.tud_idAluno

				UNION

				SELECT @tud_id AS tud_id
			)

			SELECT TOP 1
				@qtdeAulas = SUM(tap.tap_aulasPrevitas)
			FROM
				TUR_TurmaDisciplinaAulaPrevista tap WITH(NOLOCK)
				INNER JOIN tbTurmaDisciplina tud
					ON tap.tud_id = tud.tud_id
			WHERE
				 tap.tpc_id = @tpc_id
				AND tap.tap_situacao <> 3
		END
		ELSE
		BEGIN
			SELECT TOP 1
				@qtdeAulas = tap.tap_aulasPrevitas
			FROM
				TUR_TurmaDisciplinaAulaPrevista tap WITH(NOLOCK)
			WHERE tap.tud_id = @tud_id
			  AND tap.tpc_id = @tpc_id
			  AND tap.tap_situacao <> 3
		END
		
		IF (ISNULL(@tud_tipo, 0) = 11 AND @qtdeTitulares = 2) -- IF que vai preencher a @TabelaQtdes para regência com dois titulares
		BEGIN
			DECLARE @tbQtdeFalta TABLE 
			(
				alu_id BIGINT,
				mtu_id INT,
				mtd_id INT,
				tau_reposicao BIT,
				qtFaltas INT
			)
			
			DECLARE @tbQtdeFalta_SemAcumularReposicao TABLE 
			(
				alu_id BIGINT,
				mtu_id INT,
				mtd_id INT,
				tau_reposicao BIT,
				qtFaltas INT
			)
			
			DECLARE @tbQtdeAulas TABLE 
			(
				alu_id BIGINT,
				mtu_id INT,
				mtd_id INT,
				tau_reposicao BIT,
				qtAulas INT
			)
		
			DECLARE @tbQtdeAulaFalta TABLE
			(
				alu_id BIGINT,
				mtu_id INT,
				mtd_id INT,
				tau_data DATE,
				tdt_posicao INT,
				tau_reposicao INT,
				tpc_id INT,
				tpc_ordem INT,
				qtAulas INT,
				qtFaltas INT
			)
			
			INSERT INTO @tbQtdeAulaFalta
				SELECT
					Mtr.alu_id,
					Mtr.mtu_id,
					Mtr.mtd_id,
					Tau.tau_data,
					Tau.tdt_posicao,
					Tau.tau_reposicao,
					Tau.tpc_id,
					ISNULL(tpc.tpc_ordem, 0) AS tpc_ordem,
					SUM(ISNULL(Tau.tau_numeroAulas,0)) AS qtAulas,
					SUM(ISNULL(Taa.taa_frequencia, 0)) AS qtFaltas
				FROM
					@tbAlunos Mtr
					LEFT JOIN CLS_TurmaAula Tau WITH(NOLOCK)
						ON Tau.tud_id = Mtr.tud_id
						AND Tau.tau_data >= Mtr.mtd_dataMatricula AND Tau.tau_data < Mtr.mtd_dataSaida
						AND Tau.tau_situacao <> 3
						AND Tau.tdt_posicao IN (SELECT tdc_posicao FROM @tbPosicaoDocente Tdc)
					LEFT JOIN CLS_TurmaAulaAluno Taa WITH(NOLOCK)
						ON Taa.tud_id = Tau.tud_id
						AND Tau.tau_id = Taa.tau_id
						AND Taa.alu_id = Mtr.alu_id
						AND Taa.mtu_id = Mtr.mtu_id
						AND Taa.mtd_id = Mtr.mtd_id
						AND Taa.taa_situacao <> 3
					LEFT JOIN ACA_TipoPeriodoCalendario tpc WITH(NOLOCK)
						ON tpc.tpc_id = Tau.tpc_id
						AND tpc.tpc_situacao <> 3
				GROUP BY
					Mtr.alu_id,
					Mtr.mtu_id,
					Mtr.mtd_id,
					Tau.tau_data,
					Tau.tau_reposicao,
					Tau.tdt_posicao,
					Tau.tpc_id,
					tpc.tpc_ordem
			
			----- INICIO FALTAS REGÊNCIA 2 PROFs ------
			INSERT INTO @tbQtdeFalta 
			(
				alu_id,
				mtu_id,
				mtd_id,
				tau_reposicao,
				qtFaltas
			) 
			SELECT
				alu_id,
				mtu_id,
				mtd_id,
				0 AS tau_reposicao,
				CASE WHEN SUM(qtAulas) = SUM(qtFaltas) THEN 1 ELSE 0 END AS qtFaltas
			FROM
				@tbQtdeAulaFalta 
			WHERE
				tpc_id = @tpc_id
			GROUP BY
				alu_id,
				mtu_id,
				mtd_id,
				tau_data
			HAVING
				SUM(CAST(tau_reposicao AS INT)) <> SUM(qtAulas)
				
			UNION ALL
			
			SELECT
				alu_id,
				mtu_id,
				mtd_id,
				1 AS tau_reposicao,
				CASE WHEN SUM(qtAulas) = SUM(qtFaltas) THEN 1 ELSE 0 END AS qtFaltas
			FROM
				@tbQtdeAulaFalta
			WHERE
				tpc_ordem <= @tpc_ordem 
			GROUP BY
				alu_id,
				mtu_id,
				mtd_id,
				tau_data
			HAVING
				SUM(CAST(tau_reposicao AS INT)) = SUM(qtAulas)
			
			INSERT INTO @tbQtdeFalta_SemAcumularReposicao
			(
				alu_id,
				mtu_id,
				mtd_id,
				tau_reposicao,
				qtFaltas
			) 
			SELECT
				alu_id,
				mtu_id,
				mtd_id,
				1 AS tau_reposicao,
				CASE WHEN SUM(qtAulas) = SUM(qtFaltas) THEN 1 ELSE 0 END AS qtFaltas
			FROM
				@tbQtdeAulaFalta
			WHERE
				tpc_id = @tpc_id
			GROUP BY
				alu_id,
				mtu_id,
				mtd_id,
				tau_data
			HAVING
				SUM(CAST(tau_reposicao AS INT)) = SUM(qtAulas)
			
			INSERT INTO @TabelaQtdes
			(alu_id, mtu_id, mtd_id, qtAulas, qtFaltas)
			SELECT
				tqf.alu_id,
				tqf.mtu_id,
				tqf.mtd_id,
				@qtdeAulas AS qtAulas,
				SUM(tqf.qtFaltas) AS qtFaltas
			FROM
				@tbQtdeFalta tqf
			WHERE
				tau_reposicao = 0
			GROUP BY
				tqf.alu_id,
				tqf.mtu_id,
				tqf.mtd_id
				
			;WITH tbFaltasReposicao AS
			(
				SELECT
					tqf.alu_id,
					tqf.mtu_id,
					tqf.mtd_id,
					SUM(tqf.qtFaltas) AS faltas
				FROM
					@tbQtdeFalta tqf
				WHERE
					tqf.tau_reposicao = 1
				GROUP BY
					tqf.alu_id,
					tqf.mtu_id,
					tqf.mtd_id
			)	
				
			UPDATE tbq
			SET tbq.qtFaltasReposicao = tfr.faltas
			FROM
				@TabelaQtdes tbq
				INNER JOIN tbFaltasReposicao tfr
					ON tbq.alu_id = tfr.alu_id
					AND tbq.mtu_id = tfr.mtu_id
					AND tbq.mtd_id = tfr.mtd_id
					
			--repetido o bloco de cima para as faltas de reposição sem acumular
			;WITH tbFaltasReposicaoSemAcumular AS
			(
				SELECT
					tqf.alu_id,
					tqf.mtu_id,
					tqf.mtd_id,
					SUM(tqf.qtFaltas) AS faltas
				FROM
					@tbQtdeFalta_SemAcumularReposicao tqf
				WHERE
					tqf.tau_reposicao = 1
				GROUP BY
					tqf.alu_id,
					tqf.mtu_id,
					tqf.mtd_id
			)	
				
			UPDATE tbq
			SET tbq.qtFaltasReposicaoNaoAcumuladas = tfr.faltas
			FROM
				@TabelaQtdes tbq
				INNER JOIN tbFaltasReposicaoSemAcumular tfr
					ON tbq.alu_id = tfr.alu_id
					AND tbq.mtu_id = tfr.mtu_id
					AND tbq.mtd_id = tfr.mtd_id
					
			----- FIM FALTAS REGÊNCIA 2 PROFs ------
			----- INICIO AULAS REGÊNCIA 2 PROFs ------
			
			INSERT INTO @tbQtdeAulas 
			(
				alu_id,
				mtu_id,
				mtd_id,
				tau_reposicao,
				qtAulas
			) 
			SELECT
				alu_id,
				mtu_id,
				mtd_id,
				tau_reposicao,
				CASE WHEN SUM(qtAulas) > 2 THEN 2 WHEN SUM(qtAulas) = 0 THEN 0 ELSE 1 END AS qtAulas -- 0->0, 1e2->1, Maior que 2->2
			FROM
				@tbQtdeAulaFalta 
			WHERE tpc_id = @tpc_id
			GROUP BY
				alu_id,
				mtu_id,
				mtd_id,
				tau_reposicao,
				tau_data
				
			UPDATE tbq
			   SET tbq.qtAulasReposicao = qa.qtAulasReposicao
				  ,tbq.qtAulasNormais = qa.qtAulasNormais
			  FROM @TabelaQtdes tbq
				   inner join (	select qa.alu_id, qa.mtu_id, qa.mtd_id
									 , sum(case qa.tau_reposicao when 1 then qa.qtAulas else 0 end) as qtAulasReposicao
									 , sum(case qa.tau_reposicao when 0 then qa.qtAulas else 1 end) as qtAulasNormais
								  from @tbQtdeAulas qa 
								 group by qa.alu_id, qa.mtu_id, qa.mtd_id) qa
						   on qa.alu_id = tbq.alu_id
						  and qa.mtu_id = tbq.mtu_id 
						  and qa.mtd_id = tbq.mtd_id
						  
			----- FIM AULAS REGÊNCIA 2 PROFs------
		END
		ELSE IF (@tud_tipo = 15) -- IF que vai preencher a @TabelaQtdes para multiseriada do docente
		BEGIN
			DECLARE @tbFrequencia TABLE 
			( alu_id bigint
			 , mtu_idOrigem int
			 , mtd_idOrigem int
			 , tpc_id int
			 , tpc_ordem int
			 , taa_frequencia int
			 , tau_reposicao int)
			 
			 DECLARE @tbFrequenciaSemAcumularReposicao TABLE 
			( alu_id bigint
			 , mtu_idOrigem int
			 , mtd_idOrigem int
			 , tpc_id int
			 , tpc_ordem int
			 , taa_frequencia int
			 , tau_reposicao int)
			 
			;WITH tbTurmaDisciplina AS (
				SELECT Mtr.tud_idAluno AS tud_id
				FROM @tbAlunos MTR
				GROUP BY Mtr.tud_idAluno

				UNION

				SELECT @tud_id AS tud_id
			)
			
			insert into @tbTurmaAula
				SELECT
					Tau.tau_id,
					Tau.tud_id,
					Tau.tau_data,
					Tau.tau_reposicao,
					Tau.tpc_id,
					ISNULL(tpc.tpc_ordem, 0) AS tpc_ordem
				FROM
					CLS_TurmaAula Tau WITH(NOLOCK)
					INNER JOIN tbTurmaDisciplina tud
						ON Tau.tud_id = tud.tud_id
					INNER JOIN @tbPosicaoDocente Tdc
						ON Tau.tdt_posicao = Tdc.tdc_posicao
					LEFT JOIN ACA_TipoPeriodoCalendario tpc WITH(NOLOCK)
						ON tpc.tpc_id = Tau.tpc_id
						AND tpc.tpc_situacao <> 3
				WHERE
					Tau.tau_situacao <> 3
				GROUP BY
					Tau.tau_id,
					Tau.tud_id,
					Tau.tau_data,
					Tau.tau_reposicao,
					Tau.tpc_id,
					tpc.tpc_ordem

			insert into @tbFrequencia 
			(alu_id, mtu_idOrigem, mtd_idOrigem, tpc_id, tpc_ordem, taa_frequencia, tau_reposicao)
				SELECT
					mtd.alu_id
					, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
					,tau.tpc_id
					,ISNULL(tpc.tpc_ordem, 0) AS tpc_ordem
					,Taa.taa_frequencia
					,Tau.tau_reposicao
				FROM @tbAlunos Mtr
				INNER JOIN MTR_MatriculaTurmaDisciplina mtd
					ON mtd.alu_id = Mtr.alu_id
					AND mtd.mtu_id = Mtr.mtu_id
					AND mtd.mtd_id = Mtr.mtd_id
					AND mtd.mtd_situacao <> 3
				INNER JOIN TUR_TurmaRelTurmaDisciplina RelTud WITH(NOLOCK)
					ON RelTud.tud_id = Mtd.tud_id
				INNER JOIN TUR_Turma Tur WITH(NOLOCK)
					ON Tur.tur_id = RelTud.tur_id
				INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
					ON Cap.cal_id = Tur.cal_id
					AND Cap.tpc_id = @tpc_id
					AND Cap.cap_situacao <> 3
				LEFT JOIN @tbTurmaAula Tau 
					ON mtd.tud_id = Tau.tud_id
					AND Tau.tau_data >= mtd.mtd_dataMatricula AND Tau.tau_data <ISNULL(mtd.mtd_dataSaida, DATEADD(DAY, 1, Cap.cap_dataFim))
					AND
					(
						(
							Tau.tau_reposicao = 0
							AND tau.tpc_id = @tpc_id
						)
						OR
						(
							Tau.tau_reposicao = 1
							AND Tau.tpc_ordem <= @tpc_ordem
						)
					)
				LEFT JOIN ACA_TipoPeriodoCalendario tpc WITH(NOLOCK)
					ON tpc.tpc_id = tau.tpc_id
					AND tpc.tpc_situacao <> 3
				LEFT JOIN CLS_TurmaAulaAluno Taa WITH(NOLOCK)
					ON Taa.tud_id = Tau.tud_id
					AND Tau.tau_id = Taa.tau_id
					AND Taa.alu_id = mtd.alu_id
					AND Taa.mtu_id = mtd.mtu_id
					AND Taa.mtd_id = mtd.mtd_id
					AND Taa.taa_situacao <> 3

				UNION ALL

				SELECT
					mtd.alu_id
					, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
					,tau.tpc_id
					,ISNULL(tpc.tpc_ordem, 0) AS tpc_ordem
					,Taa.taa_frequencia
					,Tau.tau_reposicao
				FROM @tbAlunos Mtr
				INNER JOIN MTR_MatriculaTurmaDisciplina mtd
					ON mtd.alu_id = Mtr.alu_id
					AND mtd.mtu_id = Mtr.mtu_idOrigem
					AND mtd.mtd_id = Mtr.mtd_idOrigem
					AND mtd.tud_id <> @tud_id
					AND mtd.mtd_situacao <> 3
				INNER JOIN TUR_TurmaRelTurmaDisciplina RelTud WITH(NOLOCK)
					ON RelTud.tud_id = Mtd.tud_id
				INNER JOIN TUR_Turma Tur WITH(NOLOCK)
					ON Tur.tur_id = RelTud.tur_id
				INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
					ON Cap.cal_id = Tur.cal_id
					AND Cap.tpc_id = @tpc_id
					AND Cap.cap_situacao <> 3
				LEFT JOIN @tbTurmaAula Tau 
					ON mtd.tud_id = Tau.tud_id
					AND Tau.tau_data >= mtd.mtd_dataMatricula AND Tau.tau_data < ISNULL(mtd.mtd_dataSaida, DATEADD(DAY, 1, Cap.cap_dataFim))
					AND
					(
						(
							Tau.tau_reposicao = 0
							AND tau.tpc_id = @tpc_id
						)
						OR
						(
							Tau.tau_reposicao = 1
							AND Tau.tpc_ordem <= @tpc_ordem
						)
					)
				LEFT JOIN ACA_TipoPeriodoCalendario tpc WITH(NOLOCK)
					ON tpc.tpc_id = tau.tpc_id
					AND tpc.tpc_situacao <> 3
				LEFT JOIN CLS_TurmaAulaAluno Taa WITH(NOLOCK)
					ON Taa.tud_id = Tau.tud_id
					AND Tau.tau_id = Taa.tau_id
					AND Taa.alu_id = mtd.alu_id
					AND Taa.mtu_id = mtd.mtu_id
					AND Taa.mtd_id = mtd.mtd_id
					AND Taa.taa_situacao <> 3
					
			-- Pedro Silva - 14/08/2015
			-- Criei esta nova tabela para calcular um campo qtFaltasReposicao sem acumularBimestres anteriores
			-- é praticamente uma cópia do select de cima, só alterando o filtro de tpc no left da tbTurmaaula
			insert into @tbFrequenciaSemAcumularReposicao
			(alu_id, mtu_idOrigem, mtd_idOrigem, tpc_id, tpc_ordem, taa_frequencia, tau_reposicao)
				SELECT
					mtd.alu_id
					, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
					,tau.tpc_id
					,ISNULL(tpc.tpc_ordem, 0) AS tpc_ordem
					,Taa.taa_frequencia
					,Tau.tau_reposicao
				FROM @tbAlunos Mtr
				INNER JOIN MTR_MatriculaTurmaDisciplina mtd
					ON mtd.alu_id = Mtr.alu_id
					AND mtd.mtu_id = Mtr.mtu_id
					AND mtd.mtd_id = Mtr.mtd_id
					AND mtd.mtd_situacao <> 3
				INNER JOIN TUR_TurmaRelTurmaDisciplina RelTud WITH(NOLOCK)
					ON RelTud.tud_id = Mtd.tud_id
				INNER JOIN TUR_Turma Tur WITH(NOLOCK)
					ON Tur.tur_id = RelTud.tur_id
				INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
					ON Cap.cal_id = Tur.cal_id
					AND Cap.tpc_id = @tpc_id
					AND Cap.cap_situacao <> 3
				LEFT JOIN @tbTurmaAula Tau 
					ON mtd.tud_id = Tau.tud_id
					AND Tau.tau_data >= mtd.mtd_dataMatricula AND Tau.tau_data <ISNULL(mtd.mtd_dataSaida, DATEADD(DAY, 1, Cap.cap_dataFim))
					AND Tau.tpc_id = @tpc_id
				LEFT JOIN ACA_TipoPeriodoCalendario tpc WITH(NOLOCK)
					ON tpc.tpc_id = tau.tpc_id
					AND tpc.tpc_situacao <> 3
				LEFT JOIN CLS_TurmaAulaAluno Taa WITH(NOLOCK)
					ON Taa.tud_id = Tau.tud_id
					AND Tau.tau_id = Taa.tau_id
					AND Taa.alu_id = mtd.alu_id
					AND Taa.mtu_id = mtd.mtu_id
					AND Taa.mtd_id = mtd.mtd_id
					AND Taa.taa_situacao <> 3

				UNION ALL

				SELECT
					mtd.alu_id
					, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
					,tau.tpc_id
					,ISNULL(tpc.tpc_ordem, 0) AS tpc_ordem
					,Taa.taa_frequencia
					,Tau.tau_reposicao
				FROM @tbAlunos Mtr
				INNER JOIN MTR_MatriculaTurmaDisciplina mtd
					ON mtd.alu_id = Mtr.alu_id
					AND mtd.mtu_id = Mtr.mtu_idOrigem
					AND mtd.mtd_id = Mtr.mtd_idOrigem
					AND mtd.tud_id <> @tud_id
					AND mtd.mtd_situacao <> 3
				INNER JOIN TUR_TurmaRelTurmaDisciplina RelTud WITH(NOLOCK)
					ON RelTud.tud_id = Mtd.tud_id
				INNER JOIN TUR_Turma Tur WITH(NOLOCK)
					ON Tur.tur_id = RelTud.tur_id
				INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
					ON Cap.cal_id = Tur.cal_id
					AND Cap.tpc_id = @tpc_id
					AND Cap.cap_situacao <> 3
				LEFT JOIN @tbTurmaAula Tau 
					ON mtd.tud_id = Tau.tud_id
					AND Tau.tau_data >= mtd.mtd_dataMatricula AND Tau.tau_data < ISNULL(mtd.mtd_dataSaida, DATEADD(DAY, 1, Cap.cap_dataFim))
					AND Tau.tpc_id = @tpc_id
				LEFT JOIN ACA_TipoPeriodoCalendario tpc WITH(NOLOCK)
					ON tpc.tpc_id = tau.tpc_id
					AND tpc.tpc_situacao <> 3
				LEFT JOIN CLS_TurmaAulaAluno Taa WITH(NOLOCK)
					ON Taa.tud_id = Tau.tud_id
					AND Tau.tau_id = Taa.tau_id
					AND Taa.alu_id = mtd.alu_id
					AND Taa.mtu_id = mtd.mtu_id
					AND Taa.mtd_id = mtd.mtd_id
					AND Taa.taa_situacao <> 3

			-- Faltas da disciplina do aluno.
			INSERT INTO @TabelaQtdesFaltas
			(alu_id, mtu_id, mtd_id, qtFaltas, qtFaltasReposicao)
			SELECT
				alu_id, mtu_idOrigem, mtd_idOrigem
				, [0] AS qtFaltas
				, [1] AS qtFaltasReposicao
			FROM
			(
				SELECT alu_id, mtu_idOrigem, mtd_idOrigem
					,tpc_id, tpc_ordem, tau_reposicao
					, taa_frequencia
				FROM @tbFrequencia
			) faltas
			PIVOT
			(
				SUM(taa_frequencia) FOR tau_reposicao IN ([0], [1])
			) AS pvt
			
			-- Faltas da disciplina do aluno.
			INSERT INTO @TabelaQtdesFaltas_SemAcumularReposicao
			(alu_id, mtu_id, mtd_id, qtFaltasReposicaoNaoAcumulada)
			SELECT
				alu_id, mtu_idOrigem, mtd_idOrigem
				, SUM(taa_frequencia) AS qtFaltasReposicaoNaoAcumulada
			FROM
			(
				SELECT alu_id, mtu_idOrigem, mtd_idOrigem
					,tpc_id, tpc_ordem, tau_reposicao
					, taa_frequencia
				  FROM @tbFrequenciaSemAcumularReposicao
				 WHERE tau_reposicao = 1
			) faltas
			group by alu_id, mtu_idOrigem, mtd_idOrigem
			
			
			-- Aulas da disciplina do aluno.
			INSERT INTO @TabelaQtdesAulas
			(alu_id, mtu_id, mtd_id, qtAulas, qtAulasNormais, qtAulasReposicao)
			SELECT
				alu_id, mtu_idOrigem, mtd_idOrigem
				, @qtdeAulas AS QtAulas
				, [0] AS qtAulasNormais
				, [1] AS qtAulasReposicao
			FROM
			(
				SELECT alu_id, mtu_idOrigem, mtd_idOrigem, tpc_id as tpc
					,tpc_id, tpc_ordem, tau_reposicao
					, taa_frequencia
				FROM @tbFrequencia
			) aulas
			PIVOT
			(
				COUNT(tpc) FOR tau_reposicao IN ([0], [1])
			) AS pvt
			
			INSERT INTO @TabelaQtdes 
			      (alu_id, mtu_id, mtd_id, qtFaltas, qtFaltasReposicao, qtAulas, 
			       qtAulasNormais, qtAulasReposicao, qtFaltasReposicaoNaoAcumuladas)
			SELECT alu_id, mtu_id, mtd_id, 
				   SUM(isnull(qtFaltas,0)) as qtFaltas, 
				   SUM(isnull(qtFaltasReposicao,0)) as qtFaltasReposicao,
				   SUM(isnull(qtAulas,0)) as qtAulas, 
				   SUM(isnull(qtAulasNormais,0)) as qtAulasNormais, 
				   SUM(isnull(qtAulasReposicao,0)) as qtAulasReposicao,
				   SUM(isnull(qtFaltasReposicaoNaoAcumulada,0)) as qtFaltasReposicaoNaoAcumuladas 
			  FROM (
					select alu_id, mtu_id, mtd_id, qtFaltas, qtFaltasReposicao, 
						   null as qtAulas, null as qtAulasNormais, null as qtAulasReposicao, null as qtFaltasReposicaoNaoAcumulada
					  from @TabelaQtdesFaltas
					  
					union
					
					select alu_id, mtu_id, mtd_id, null, null, qtAulas, qtAulasNormais, qtAulasReposicao, null
					  from @TabelaQtdesAulas
					  
					union
					  
					select alu_id, mtu_id, mtd_id, null, null, null, null, null, qtFaltasReposicaoNaoAcumulada
					  from @TabelaQtdesFaltas_SemAcumularReposicao
					  
					) as TabAulasFaltas
			 GROUP BY alu_id, mtu_id, mtd_id
		END
		ELSE IF (@tud_tipo = 18)--Experiencia
		BEGIN
			-- Aulas da disciplina do aluno.
			INSERT INTO @TabelaQtdes
			(alu_id, mtu_id, mtd_id, qtAulas, qtFaltas)
			SELECT
				Mtr.alu_id, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
				, ISNULL(SUM(Tau.tau_numeroAulas), 0) AS QtAulas
				, SUM(ISNULL(Taa.taa_frequencia, 0)) AS QtFaltas
				--, Tau.*
			FROM @tbAlunos Mtr
			INNER JOIN TUR_TurmaDisciplinaTerritorio TRT WITH(NOLOCK)
				ON Mtr.tud_id = TRT.tud_idExperiencia
			LEFT JOIN CLS_TurmaAulaTerritorio TAT WITH(NOLOCK)
				ON Mtr.tud_id = TAT.tud_idExperiencia
			LEFT JOIN CLS_TurmaAula TAU WITH(NOLOCK)
				ON	TAU.tud_id = TRT.tud_idTerritorio
				AND TAU.tau_id = TAT.tau_idTerritorio
				AND TAU.tud_id = TAT.tud_idTerritorio
				AND TAU.tau_data >= TRT.tte_vigenciaInicio
				AND TAU.tau_data <= ISNULL(TRT.tte_vigenciaFim, TAU.tau_data)
				AND Tau.tpc_id = @tpc_id
				AND Tau.tau_data >= Mtr.mtd_dataMatricula AND Tau.tau_data < Mtr.mtd_dataSaida
				AND Tau.tau_situacao <> 3
				AND Tau.tdt_posicao IN (SELECT tdc_posicao FROM @tbPosicaoDocente Tdc)
			LEFT JOIN CLS_TurmaAulaAluno Taa WITH(NOLOCK)
				ON Taa.tud_id = Tau.tud_id
				AND Taa.tau_id = Tau.tau_id
				AND Taa.alu_id = Mtr.alu_id
				AND Taa.mtu_id = Mtr.mtu_id
				--AND Taa.mtd_id = Mtr.mtd_id
				AND Taa.taa_situacao <> 3
			WHERE
				Mtr.tud_tipo = 18 --Experiencia
			GROUP BY
				Mtr.alu_id, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
		END
		ELSE -- ELSE que vai preencher a @TabelaQtdes para todos os outros tipos (exceto regência com dois titulares e multiseriada do docente)
		BEGIN
			;WITH tbTurmaDisciplina AS 
			(
				SELECT
					tud_id
				FROM
					@tbAlunos Mtr
				GROUP BY
					tud_id
			)
			
			insert into @tbTurmaAula
				SELECT
					Tau.tau_id,
					Tau.tud_id,
					Tau.tau_data,
					Tau.tau_reposicao,
					Tau.tpc_id,
					ISNULL(tpc.tpc_ordem, 0) AS tpc_ordem
				FROM
					tbTurmaDisciplina Tud
					INNER JOIN CLS_TurmaAula Tau WITH(NOLOCK)
						ON Tud.tud_id = Tau.tud_id
						AND Tau.tau_situacao <> 3
					INNER JOIN @tbPosicaoDocente Tdc
						ON Tau.tdt_posicao = Tdc.tdc_posicao
					LEFT JOIN ACA_TipoPeriodoCalendario tpc WITH(NOLOCK)
						ON tpc.tpc_id = Tau.tpc_id
						AND tpc.tpc_situacao <> 3
				GROUP BY
					Tau.tau_id,
					Tau.tud_id,
					Tau.tau_data,
					Tau.tau_reposicao,
					Tau.tpc_id,
					tpc.tpc_ordem
			
		
			-- Faltas da disciplina do aluno.
			INSERT INTO @TabelaQtdesFaltas
			(alu_id, mtu_id, mtd_id, qtFaltas, qtFaltasReposicao)
			SELECT
				alu_id, mtu_idOrigem, mtd_idOrigem
				, [0] AS qtFaltas
				, [1] AS qtFaltasReposicao
			FROM
			(
				SELECT
					Mtr.alu_id, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
					,Taa.taa_frequencia
					,Tau.tau_reposicao
				FROM @tbAlunos Mtr
				LEFT JOIN @tbTurmaAula Tau 
					ON Tau.tud_id = Mtr.tud_id
					AND 
					(
						(
							Tau.tau_reposicao = 0
							AND Tau.tpc_id = @tpc_id
						)
						OR
						(
							Tau.tau_reposicao = 1
							AND Tau.tpc_ordem <= @tpc_ordem
						)
					)
					AND Tau.tau_data >= Mtr.mtd_dataMatricula AND Tau.tau_data < Mtr.mtd_dataSaida
				LEFT JOIN CLS_TurmaAulaAluno Taa WITH(NOLOCK)
					ON Taa.tud_id = Mtr.tud_id
					AND Tau.tau_id = Taa.tau_id
					AND Taa.alu_id = Mtr.alu_id
					AND Taa.mtu_id = Mtr.mtu_id
					AND Taa.mtd_id = Mtr.mtd_id
					AND Taa.taa_situacao <> 3
				WHERE
					Mtr.tud_tipo NOT IN (13, 16) -- 13-Complementação de regência 
												 -- 16-Multisseriada do aluno
			) faltas
			PIVOT
			(
				SUM(taa_frequencia) FOR tau_reposicao IN ([0], [1])
			) AS pvt
			
			-- Pedro Silva - 14/08/2015
			-- Criei esta nova tabela para calcular um campo qtFaltasReposicao sem acumularBimestres anteriores
			INSERT INTO @TabelaQtdesFaltas_SemAcumularReposicao
			(alu_id, mtu_id, mtd_id, qtFaltasReposicaoNaoAcumulada)
			SELECT
				alu_id, mtu_idOrigem, mtd_idOrigem
				, SUM(taa_frequencia) AS qtFaltasReposicaoNaoAcumulada
			FROM
			(
				SELECT
					Mtr.alu_id, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
					,Taa.taa_frequencia
					,Tau.tau_reposicao
				FROM @tbAlunos Mtr
				LEFT JOIN @tbTurmaAula Tau 
					ON Tau.tud_id = Mtr.tud_id
				    AND Tau.tpc_id = @tpc_id
				    AND Tau.tau_data >= Mtr.mtd_dataMatricula AND Tau.tau_data < Mtr.mtd_dataSaida
				LEFT JOIN CLS_TurmaAulaAluno Taa WITH(NOLOCK)
					ON Taa.tud_id = Mtr.tud_id
					AND Tau.tau_id = Taa.tau_id
					AND Taa.alu_id = Mtr.alu_id
					AND Taa.mtu_id = Mtr.mtu_id
					AND Taa.mtd_id = Mtr.mtd_id
					AND Taa.taa_situacao <> 3
				WHERE
					Mtr.tud_tipo NOT IN (13, 16)
				    AND Tau.tau_reposicao = 1
			) faltas
			group by alu_id, mtu_idOrigem, mtd_idOrigem
			
			-- Aulas da disciplina do aluno.
			INSERT INTO @TabelaQtdesAulas
			(alu_id, mtu_id, mtd_id, qtAulas, qtAulasNormais, qtAulasReposicao)
			SELECT
				alu_id, mtu_idOrigem, mtd_idOrigem
				, @qtdeAulas AS QtAulas
				, [0] AS qtAulasNormais
				, [1] AS qtAulasReposicao
			FROM
			(
				SELECT
					Mtr.alu_id, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
					,Tau.tud_id as tud
					,Tau.tau_reposicao
				FROM @tbAlunos Mtr
				LEFT JOIN @tbTurmaAula Tau 
					ON Tau.tud_id = Mtr.tud_id
					AND Tau.tpc_id = @tpc_id
					AND Tau.tau_data >= Mtr.mtd_dataMatricula AND Tau.tau_data < Mtr.mtd_dataSaida
				WHERE
					Mtr.tud_tipo NOT IN (13, 16) -- 13-Complementação de regência 
												 -- 16-Multisseriada do aluno
			) aulas
			PIVOT
			(
				COUNT(tud) FOR tau_reposicao IN ([0], [1])
			) AS pvt
			
			INSERT INTO @TabelaQtdes 
			      (alu_id, mtu_id, mtd_id, qtFaltas, qtFaltasReposicao, qtAulas, 
				   qtAulasNormais, qtAulasReposicao, qtFaltasReposicaoNaoAcumuladas)
			SELECT alu_id, mtu_id, mtd_id, 
				   SUM(isnull(qtFaltas,0)) as qtFaltas, 
				   SUM(isnull(qtFaltasReposicao,0)) as qtFaltasReposicao,
				   SUM(isnull(qtAulas,0)) as qtAulas, 
				   SUM(isnull(qtAulasNormais,0)) as qtAulasNormais, 
				   SUM(isnull(qtAulasReposicao,0)) as qtAulasReposicao,
				   SUM(isnull(qtFaltasReposicaoNaoAcumulada,0)) as qtFaltasReposicaoNaoAcumuladas 
			  FROM (
					select alu_id, mtu_id, mtd_id, qtFaltas, qtFaltasReposicao, 
						   null as qtAulas, null as qtAulasNormais, null as qtAulasReposicao, null as qtFaltasReposicaoNaoAcumulada
					  from @TabelaQtdesFaltas
					  
					union
					
					select alu_id, mtu_id, mtd_id, null, null, qtAulas, qtAulasNormais, qtAulasReposicao, null 
					  from @TabelaQtdesAulas
					
					union
					  
					select alu_id, mtu_id, mtd_id, null, null, null, null, null, qtFaltasReposicaoNaoAcumulada
					  from @TabelaQtdesFaltas_SemAcumularReposicao
					  
					) as TabAulasFaltas
			 GROUP BY alu_id, mtu_id, mtd_id
			
			; WITH MatriculasIngles AS
			(
				SELECT
					Mtr.alu_id
					, Mtr.mtu_id
					, Mtd.mtd_id -- Traz o mtd da disciplina Complementação de regência dele na turma.
					, Mtr.tud_id
					, Mtr.mtd_dataMatricula
					, Mtr.mtd_dataSaida
					, Mtr.tud_tipo, Mtr.tud_idRegencia, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
				FROM @tbAlunos Mtr
				INNER JOIN MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
					ON Mtd.alu_id = Mtr.alu_id
					AND Mtd.mtu_id = Mtr.mtu_id
					AND Mtd.tud_id = Mtr.tud_idRegencia
					AND Mtd.mtd_situacao <> 3
			)
			
			, tbTurmaDisciplina AS 
			(
				SELECT
					tud_id
				FROM
					@tbAlunos Mtr
				GROUP BY
					tud_id
			)
			
			, tbTurmaAula AS 
			(
				SELECT
					Tau.tau_id,
					Tau.tud_id,
					Tau.tau_data,
					Tau.tau_numeroAulas,
					Tau.tau_reposicao,
					Tau.tpc_id,
					ISNULL(tpc.tpc_ordem, 0) AS tpc_ordem
				FROM
					tbTurmaDisciplina Tud
					INNER JOIN CLS_TurmaAula Tau WITH(NOLOCK)
						ON Tud.tud_id = Tau.tud_id
						AND Tau.tau_situacao <> 3
					INNER JOIN @tbPosicaoDocente Tdc
						ON Tau.tdt_posicao = Tdc.tdc_posicao
					LEFT JOIN ACA_TipoPeriodoCalendario tpc WITH(NOLOCK)
						ON tpc.tpc_id = Tau.tpc_id
						AND tpc.tpc_situacao <> 3
				GROUP BY
					Tau.tau_id,
					Tau.tud_id,
					Tau.tau_data,
					Tau.tau_numeroAulas,
					Tau.tau_reposicao,
					Tau.tpc_id,
					tpc.tpc_ordem
			)
			
			-- Aulas da disciplina Complementação de regência do aluno.
			INSERT INTO @TabelaQtdes
			(alu_id, mtu_id, mtd_id, qtAulas, qtFaltas, qtFaltasReposicao)
			SELECT
				alu_id, mtu_idOrigem, mtd_idOrigem
				, @qtdeAulas AS QtAulas
				--, [0] AS qtFaltas		-- Comentadas por Daniel Jun para resolução de um bug. Tipo usado apenas em 2014, deve ficar 0 mesmo
				--, [1] AS qtFaltasReposicao
				, 0 AS qtFaltas
				, 0 AS qtFaltasReposicao
			FROM
			(
				SELECT
					Mtr.alu_id, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
					--, @qtdeAulas AS QtAulas
					-- Quantidade de aulas do Inglês * quantidade de faltas da regência (no mesmo dia).
					--, SUM(ISNULL(Tau.tau_numeroAulas, 0) * ISNULL(TaaRegencia.taa_frequencia, 0)) AS QtFaltas
					--, Tau.*
					,ISNULL(Tau.tau_numeroAulas, 0) * ISNULL(TaaRegencia.taa_frequencia, 0) AS taa_frequencia
					,Tau.tau_reposicao
				FROM MatriculasIngles Mtr
				LEFT JOIN tbTurmaAula Tau 
					ON Tau.tud_id = Mtr.tud_id
					AND 
					(
						(
							Tau.tau_reposicao = 0
							AND Tau.tpc_id = @tpc_id
						)
						OR
						(
							Tau.tau_reposicao = 1
							AND Tau.tpc_ordem <= @tpc_ordem
						)
					)
					AND Tau.tau_data >= Mtr.mtd_dataMatricula AND Tau.tau_data < Mtr.mtd_dataSaida
				LEFT JOIN CLS_TurmaAula TauRegencia WITH(NOLOCK)
					ON TauRegencia.tud_id = Mtr.tud_idRegencia
					AND TauRegencia.tau_data = Tau.tau_data
					AND TauRegencia.tau_situacao <> 3
					AND TauRegencia.tdt_posicao IN (SELECT tdc_posicao FROM @tbPosicaoDocente Tdc)
				LEFT JOIN CLS_TurmaAulaAluno TaaRegencia WITH(NOLOCK)
					ON TaaRegencia.tud_id = TauRegencia.tud_id
					AND TaaRegencia.tau_id = TauRegencia.tau_id -- Busca a quantidade de faltas da regência.
					AND TaaRegencia.alu_id = Mtr.alu_id
					AND TaaRegencia.mtu_id = Mtr.mtu_id
					AND TaaRegencia.mtd_id = Mtr.mtd_id
					AND TaaRegencia.taa_situacao <> 3
				WHERE
					Mtr.tud_tipo = 13 -- 13-Complementação de regência
					AND NOT EXISTS (
						SELECT 1 
						FROM @TabelaQtdes Qtd 
						WHERE 
							Mtr.alu_id = Qtd.alu_id
							AND Mtr.mtu_idOrigem = Qtd.mtu_id
							AND Mtr.mtd_idOrigem = Qtd.mtd_id 
					)
				--GROUP BY
				--	Mtr.alu_id, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
			) faltas
			PIVOT
			(
				SUM(taa_frequencia) FOR tau_reposicao IN ([0], [1])
			) AS pvt
		END
		
	END
	
	--Pedro Silva - 29/07/2015
	--alguns destes campos estavam retornando null em algumas situações 
	--e tinha algumas procedures que não estavam tratando para receber null, por isso adicionei os updates
	update @TabelaQtdes set qtAulas = 0			  where qtAulas is null
	update @TabelaQtdes set qtAulasNormais = 0	  where qtAulasNormais is null
	update @TabelaQtdes set qtAulasReposicao = 0  where qtAulasReposicao is null
	update @TabelaQtdes set qtFaltas = 0		  where qtFaltas is null
	update @TabelaQtdes set qtFaltasReposicao = 0 where qtFaltasReposicao is null
	update @TabelaQtdes set qtFaltasReposicaoNaoAcumuladas = 0 where qtFaltasReposicaoNaoAcumuladas is null

	RETURN;
END
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
PRINT N'Altering [dbo].[NEW_CLS_TurmaAulaAluno_Frequencia_SelectBy_TurmaDisciplina_Territorio]'
GO
-- ========================================================================
-- Author:		Daniel Jun Suguimoto
-- Create date: 11/07/2016
-- Description:	utilizado no minhas turmas, para retornar a frequencia de territórios do saber
-- ========================================================================
ALTER PROCEDURE [dbo].[NEW_CLS_TurmaAulaAluno_Frequencia_SelectBy_TurmaDisciplina_Territorio]
	@tud_id BIGINT
	, @tau_id INT
AS
BEGIN	
	DECLARE @tau_data DATE;
	DECLARE @tdt_posicao TINYINT;
	SELECT @tau_data = tau_data, @tdt_posicao = tdt_posicao
	FROM CLS_TurmaAula WITH(NOLOCK)
	WHERE tud_id = @tud_id
	AND tau_id = @tau_id
	AND tau_situacao <> 3

	DECLARE @tbLogAlteracaoFrequencia AS TABLE (tud_id BIGINT NOT NULL, tau_id INT NOT NULL, usu_id UNIQUEIDENTIFIER NOT NULL, lta_data DATETIME NOT NULL, PRIMARY KEY(tud_id, tau_id))
	INSERT INTO @tbLogAlteracaoFrequencia (tud_id, tau_id, usu_id, lta_data)
	SELECT 
		Lta.tud_id,
		Lta.tau_id,
		Lta.usu_id, 
		Lta.lta_data 
	FROM 
		dbo.FN_RetornaUltimaAlteracaoTurmaAula(@tud_id, @tau_id, 3) AS Lta --Alteração na frequência

	DECLARE @tbTurmaAulaTerritorio TABLE
	(
		tud_id BIGINT, 
		tau_id INT, 
		tau_data DATE, 
		tau_numeroAulas INT, 
		tau_dataAlteracao DATETIME, 
		tau_efetivado BIT, 
		tdt_posicao TINYINT, 
		usu_idDocenteAlteracao UNIQUEIDENTIFIER,
		PRIMARY KEY(tud_id, tau_id)
	);

	;WITH TurmaAula AS
	(
		SELECT
			tau.tud_id,
			tau.tau_id,
			tau.tau_data,
			tau.tau_numeroAulas,
			tau.tau_dataAlteracao,
			tau.tau_efetivado,
			tau.tdt_posicao,
			tau.usu_idDocenteAlteracao,
			CAST(1 AS BIT) AS Experiencia
		FROM
			CLS_TurmaAula tau WITH(NOLOCK)
		WHERE
			tau.tud_id = @tud_id
			AND tau.tau_id = @tau_id
			AND tau.tau_situacao <> 3

		UNION

		SELECT
			tau.tud_id,
			tau.tau_id,
			tau.tau_data,
			tau.tau_numeroAulas,
			tau.tau_dataAlteracao,
			tau.tau_efetivado,
			tau.tdt_posicao,
			tau.usu_idDocenteAlteracao,
			CAST(0 AS BIT) AS Experiencia
		FROM
			TUR_TurmaDisciplinaTerritorio tte WITH(NOLOCK)
			INNER JOIN CLS_TurmaAulaTerritorio tat WITH(NOLOCK)
				ON tte.tud_idExperiencia = tat.tud_idExperiencia
				AND tte.tud_idTerritorio = tat.tud_idTerritorio
				AND tat.tud_idExperiencia = @tud_id
				AND tat.tau_idExperiencia = @tau_id
			INNER JOIN CLS_TurmaAula tau WITH(NOLOCK)
				ON tau.tud_id = tat.tud_idTerritorio
				AND tau.tau_id = tat.tau_idTerritorio
				AND tau.tau_data >= tte.tte_vigenciaInicio
				AND tau.tau_data <= ISNULL(tte.tte_vigenciaFim, tau.tau_data)
				AND ISNULL(tau.tdt_posicao,0) = ISNULL(@tdt_posicao,0)
				AND tau.tau_situacao <> 3
		WHERE
			tte.tud_idExperiencia = @tud_id
	)

	INSERT INTO @tbTurmaAulaTerritorio 
	(
		tud_id, 
		tau_id, 
		tau_data, 
		tau_numeroAulas, 
		tau_dataAlteracao, 
		tau_efetivado, 
		tdt_posicao,
		usu_idDocenteAlteracao
	)
	SELECT
		tau.tud_id,
		tau.tau_id,
		tau.tau_data,
		tau.tau_numeroAulas,
		tau.tau_dataAlteracao,
		tau.tau_efetivado,
		tau.tdt_posicao,
		tau.usu_idDocenteAlteracao
	FROM
		TurmaAula tau
	GROUP BY
		tau.tud_id,
		tau.tau_id,
		tau.tau_data,
		tau.tau_numeroAulas,
		tau.tau_dataAlteracao,
		tau.tau_efetivado,
		tau.tdt_posicao,
		tau.usu_idDocenteAlteracao

	--Selecina as movimentações que possuem matrícula anterior
	;WITH TabelaMovimentacao AS (
		SELECT
			alu_id,
			mtu_idAnterior,
			tmv_nome    
		FROM
			MTR_Movimentacao Mov WITH (NOLOCK) 
			INNER JOIN ACA_TipoMovimentacao Tmv WITH (NOLOCK) 
				ON Mov.tmv_idSaida = Tmv.tmv_id
				AND Tmv.tmv_situacao <> 3
		WHERE
			Mov.mov_situacao NOT IN (3,4)
			AND Mov.mtu_idAnterior IS NOT NULL
	)
	SELECT	
		mtd.alu_id
		, mtd.mtu_id
		, mtd.mtd_id
		, mtd.tud_id
		, tau.tau_id
		, ISNULL(taa.taa_frequencia, 0) AS taa_frequencia
		, ISNULL(tau.tau_numeroAulas, 0) AS tau_numeroAulas
		, tau.tau_data
		, CAST(ISNULL(tau.tau_efetivado, 0) AS BIT) AS tau_efetivado
		, CAST(ISNULL(tau.tdt_posicao, 0) AS TINYINT) AS tdt_posicao
		-- 0 - não possui FJ / 1 - possui FJ que abona / 2 - possui FJ que não abona 
		, CASE WHEN afj.afj_id IS NULL
				THEN '0'					    						    
				ELSE (CASE WHEN tjf.tjf_abonaFalta = 1 THEN '1' ELSE '2' END)
			END AS falta_justificada
		, CAST(CASE WHEN ajf.alu_id IS NULL THEN 0 ELSE 1 END AS BIT) AS falta_abonada
		-- Verifica se há dispensa de disciplina para o aluno.
		, 0 AS dispensadisciplina
		, ISNULL(taa.taa_frequenciaBitMap, '') AS taa_frequenciaBitMap
		, Mtd.mtd_situacao
			
		, ISNULL(pesAlteracao.pes_nome,usuAlteracao.usu_Login) as nomeUsuAlteracao	-- inserido para poder exibir o usuário que alterou os dados 
		, ISNULL(Lta.lta_data, tau.tau_dataAlteracao) AS tau_dataAlteracao			-- inserido para poder exibir a data que o usuário realizou a alteração
			
	FROM 
		@tbTurmaAulaTerritorio tau 
		INNER JOIN MTR_MatriculaTurmaDisciplina mtd WITH(NOLOCK)
			ON mtd.tud_id = tau.tud_id
			AND ISNULL(Mtd.mtd_numeroChamada, 0) >= 0
			-- Valida o período de matrícula e saída do aluno (se está dentro do período atual).
			AND (DATEDIFF(DAY, Mtd.mtd_dataMatricula, tau.tau_data) >= 0)
			AND (Mtd.mtd_situacao <> 5 OR ISNULL((DATEDIFF(DAY, Mtd.mtd_dataSaida, tau.tau_data)), 0) <= 0)
			AND Mtd.mtd_situacao <> 3	
		LEFT JOIN CLS_TurmaAulaAluno taa WITH (NOLOCK)		
			ON taa.tud_id = tau.tud_id
			AND taa.tau_id = tau.tau_id
			AND taa.alu_id = mtd.alu_id
			AND taa.mtu_id = mtd.mtu_id
			AND taa.mtd_id = mtd.mtd_id
			AND taa.taa_situacao <> 3
		LEFT JOIN ACA_AlunoJustificativaFalta afj WITH(NOLOCK)
			ON  afj.alu_id = mtd.alu_id
			AND afj.afj_situacao <> 3
			AND (tau_data >= afj.afj_dataInicio)
			AND ((afj.afj_dataFim  IS NULL) OR (tau_data <= afj.afj_dataFim))
		LEFT JOIN ACA_TipoJustificativaFalta tjf WITH(NOLOCK)
			ON tjf.tjf_id = afj.tjf_id
			AND tjf.tjf_situacao <> 3
		LEFT JOIN ACA_AlunoJustificativaAbonoFalta ajf WITH(NOLOCK)
			ON  ajf.alu_id = mtd.alu_id
			AND ajf.tud_id = mtd.tud_id
			AND ajf.ajf_situacao <> 3
			AND (Tau.tau_data >= ajf.ajf_dataInicio)
			AND (Tau.tau_data <= ajf.ajf_dataFim)
		---		
		LEFT JOIN @tbLogAlteracaoFrequencia AS Lta 
			ON Tau.tud_id = Lta.tud_id
			AND Tau.tau_id = Lta.tau_id	
		LEFT JOIN Synonym_SYS_Usuario AS usuAlteracao WITH(NOLOCK)
			ON usuAlteracao.usu_id = ISNULL(Lta.usu_id, tau.usu_idDocenteAlteracao)
			AND usuAlteracao.usu_situacao <> 3
		LEFT JOIN Synonym_PES_Pessoa AS pesAlteracao WITH(NOLOCK)
			ON  pesAlteracao.pes_id = usuAlteracao.pes_id
			AND pesAlteracao.pes_situacao <> 3			
	GROUP BY
		mtd.alu_id
		, mtd.mtu_id
		, mtd.mtd_id
		, mtd.tud_id
		, tau.tau_id
		, mtd_situacao
		, mtd.mtd_numeroChamada
		, taa.taa_frequencia 	
		, tau.tau_numeroAulas	
		, mtd.mtd_dataMatricula	
		, mtd.mtd_dataSaida
		, tau.tau_data
		, tau.tau_efetivado
		, tau.tdt_posicao
		, afj.afj_id
		, tjf.tjf_abonaFalta
		, ajf.alu_id
		, taa.taa_frequenciaBitMap
		, ISNULL(pesAlteracao.pes_nome,usuAlteracao.usu_Login)
		, tau.tau_dataAlteracao
		, Lta.lta_data
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
PRINT N'Altering [dbo].[NEW_CLS_TurmaAulaAluno_SelectFreqGlobalByTurmaPeriodoData]'
GO
-- =================================================
-- Author:		Renata Tiepo Fonseca
-- Create date: 10/02/2012
-- Description:	Retorna a frequência global dos 
--				alunos matriculados na turma
--				e períodos selecionados.
-- =================================================
ALTER PROCEDURE [dbo].[NEW_CLS_TurmaAulaAluno_SelectFreqGlobalByTurmaPeriodoData]	
	@tur_id BIGINT
	, @tpc_id INT
	, @dataInicio DATETIME 
	, @dataFim DATETIME
AS
BEGIN	
	-- Guardar todos os tud_ids da turma
	DECLARE @TabelaTudID TABLE (tud_id BIGINT NOT NULL)
	
	INSERT INTO @TabelaTudID (tud_id)
	-- Trazer todos os tud_id
	SELECT 
		Tds.tud_id 
	FROM TUR_TurmaRelTurmaDisciplina RelTur WITH(NOLOCK)
	INNER JOIN TUR_TurmaDisciplina Tds WITH(NOLOCK)
		ON (Tds.tud_id = RelTur.tud_id)
			AND (Tds.tud_situacao <> 3)
	WHERE
		RelTur.tur_id = @tur_id
		
	;WITH tbFrequencia AS
	(
		SELECT 
			 Taa.alu_id
			, Taa.mtu_id
			, MIN(ISNULL(taa_frequencia, 0)) frequencia
		FROM 
			CLS_TurmaAulaAluno Taa WITH(NOLOCK)
		INNER JOIN CLS_TurmaAula Tau WITH(NOLOCK)
			ON Tau.tau_id = Taa.tau_id
			AND Tau.tud_id = Taa.tud_id
			AND Tau.tau_situacao <> 3
			AND (Tau.tau_data >= @dataInicio AND Tau.tau_data <= @dataFim)
		LEFT JOIN ACA_AlunoJustificativaFalta afj WITH(NOLOCK)
			ON Taa.alu_id = afj.alu_id
			AND afj.afj_situacao <> 3
			AND tau_data >= afj.afj_dataInicio
			AND ((afj.afj_dataFim  IS NULL) OR (tau_data <= afj.afj_dataFim))
		LEFT JOIN ACA_TipoJustificativaFalta tjf WITH(NOLOCK)
			ON tjf.tjf_id = afj.tjf_id
			AND tjf.tjf_situacao <> 3
		WHERE 
			Taa.taa_situacao <> 3
			AND Tau.tpc_id = @tpc_id
			AND Tau.tud_id IN (SELECT tud_id FROM @TabelaTudID)
			AND ((tjf.tjf_abonaFalta IS NULL) OR (tjf.tjf_abonaFalta = 0))
		GROUP BY
			Tau.tau_data
			, Taa.alu_id
			, Taa.mtu_id
	)
	
	SELECT
		Mtu.alu_id
		, Mtu.mtu_id
		, NULL AS mtd_id
		, NULL AS tud_id
		, SUM(Freq.frequencia) frequencia
	FROM 
		MTR_MatriculaTurma Mtu WITH(NOLOCK)
		LEFT JOIN tbFrequencia Freq
			ON  Freq.alu_id = Mtu.alu_id
			AND Freq.mtu_id = Mtu.mtu_id
	WHERE 
		Mtu.tur_id = @tur_id
	GROUP BY
		Mtu.alu_id
		, Mtu.mtu_id
END



GO
PRINT N'Altering [dbo].[NEW_CLS_TurmaNotaAluno_SelectBy_TurmaDisciplinaPeriodo]'
GO

-- ========================================================================
-- Author:		Paula Fiorini
-- Create date: 11/07/2011 12:01
-- Description:	Utilizado no cadatro de planejamento, retorna os
--				lançamentos de notas dos alunos, filtrando pelo período do
--				calendário.
-- ========================================================================
ALTER PROCEDURE [dbo].[NEW_CLS_TurmaNotaAluno_SelectBy_TurmaDisciplinaPeriodo]
	@tud_id BIGINT
	, @tnt_id INT
	, @tpc_id INT
	, @ent_id UNIQUEIDENTIFIER
	, @ordenacao TINYINT
	, @trazerInativos BIT

AS
BEGIN

    DECLARE @tipoLancamento TINYINT
    
    SELECT 
        @tipoLancamento = fav.fav_tipoLancamentoFrequencia
    FROM 
        TUR_TurmaDisciplina TurDiscip WITH(NOLOCK)
        INNER JOIN TUR_TurmaRelTurmaDisciplina TurRelD WITH(NOLOCK)
            ON (TurDiscip.tud_id = TurRelD.tud_id)
        INNER JOIN TUR_Turma Turma WITH(NOLOCK)
            ON (TurRelD.tur_id = Turma.tur_id)
        INNER JOIN ACA_FormatoAvaliacao fav WITH(NOLOCK)
            ON (Turma.fav_id = fav.fav_id)
    WHERE 
         TurDiscip.tud_id = @tud_id                   

	DECLARE @dataInicioPeriodo DATE
		, @dataFimPeriodo DATE
	
	-- Seta data de início e fim do período atual do calendário ligado a turma
	SELECT 
		@dataInicioPeriodo = Cap.cap_dataInicio
		, @dataFimPeriodo = Cap.cap_dataFim
	FROM 
		TUR_Turma Tur WITH(NOLOCK)
		INNER JOIN TUR_TurmaRelTurmaDisciplina TurRel WITH(NOLOCK)
			ON (TurRel.tud_id = @tud_id)
				AND (TurRel.tur_id = Tur.tur_id)
		INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
			ON (Cap.cal_id = Tur.cal_id)
	WHERE
		Cap.tpc_id = @tpc_id
		AND Cap.cap_situacao <> 3
		AND Tur.tur_situacao <> 3;               
	
	DECLARE @tud_tipo TINYINT;
	SELECT
		@tud_tipo = ISNULL(tud.tud_tipo,0)
	FROM
		TUR_TurmaDisciplina tud WITH(NOLOCK)
	WHERE
		tud.tud_id = @tud_id
		AND tud.tud_situacao <> 3
		
	-- Se a disciplina não for um componente da regência, traz os dados normalmente
	IF (@tud_tipo <> 12)
	BEGIN	
		--Selecina as movimentações que possuem matrícula anterior
		WITH TabelaMovimentacao AS (
			SELECT
				alu_id,
				mtu_idAnterior,
				tmv_nome    
			FROM
				MTR_Movimentacao MOV WITH (NOLOCK) 
				INNER JOIN ACA_TipoMovimentacao TMV WITH (NOLOCK) 
					ON MOV.tmv_idSaida = TMV.tmv_id
			WHERE
				mov_situacao NOT IN (3,4)
				AND tmv_situacao <> 3
				AND mtu_idAnterior IS NOT NULL
		)			
		SELECT	
			mtd.alu_id
			, mtd.mtu_id
			, mtd.mtd_id
			, mtd.tud_id
			, tnt.tnt_id
			, tnt.tnt_efetivado
			, pes.pes_nome + 
				(
					CASE WHEN mtd_situacao = 5 THEN ISNULL((SELECT TOP 1 ' (' + tmv_nome + ')' FROM TabelaMovimentacao MOV WITH(NOLOCK) WHERE MOV.mtu_idAnterior = mtd.mtu_id AND MOV.alu_id = mtd.alu_id), ' (Inativo)')
						ELSE '' END
				) AS pes_nome
			,  CASE WHEN mtd.mtd_numeroChamada > 0 THEN CAST(mtd.mtd_numeroChamada AS VARCHAR)
						ELSE '-' END as mtd_numeroChamada
			, tna.tna_avaliacao
			, tna.tna_relatorio
			-- (FALTA JUSTIFICADA) 0 - não possui FJ / 1 - possui FJ que abona / 2 - possui FJ que não abona 
			, CASE WHEN afj.afj_id IS NULL
					THEN '0'					    						    
					ELSE (CASE WHEN tjf.tjf_abonaFalta = 1 THEN '1' ELSE '2' END)
			   END AS falta_justificada
			-- (ALUNO AUSENTE) 1 - aluno ausente que não possui falta justificada 
			, CAST( CASE WHEN @tipoLancamento IN  (1,4,5) AND TAU.tau_id IS NOT NULL THEN
					(
						CASE WHEN fav.fav_tipoApuracaoFrequencia = 2 AND crp.crp_controleTempo = 2 THEN
						(
							CASE WHEN
							((SELECT 
								taa_frequencia 
							 FROM 
								CLS_TurmaAulaAluno WITH (NOLOCK)
							 WHERE 
								tud_id = TAU.tud_id
								AND tau_id = TAU.tau_id
								AND alu_id = mtd.alu_id
								AND mtu_id = mtd.mtu_id
								AND mtd_id = mtd.mtd_id) > 0)
								AND afj.afj_id IS NULL
							THEN 1 ELSE 0 END
						)
						ELSE
						(
							-- Aulas planejadas
							CASE WHEN  
							((SELECT 
								taa_frequencia 
							 FROM 
								CLS_TurmaAulaAluno WITH (NOLOCK)
							 WHERE 
								tud_id = TAU.tud_id
								AND tau_id = TAU.tau_id
								AND alu_id = mtd.alu_id
								AND mtu_id = mtd.mtu_id
								AND mtd_id = mtd.mtd_id) = TAU.tau_numeroAulas)
								AND afj.afj_id IS NULL
							THEN 1 ELSE 0 END
						) END )
					-- Período ( Nâo terá esta funcionalidade )
					ELSE 0				       
			END AS BIT) AS aluno_ausente 
			, NULL AS tca_numeroAvaliacao
			-- Verifica se há dispensa de disciplina para o aluno.
			, 0 AS dispensadisciplina
			, ISNULL(tna.tna_participante, 0) AS tna_participante
		FROM 
			MTR_MatriculaTurmaDisciplina mtd WITH(NOLOCK)
			INNER JOIN MTR_MatriculaTurma mtu WITH(NOLOCK)
				ON mtd.alu_id = mtu.alu_id 
				AND mtd.mtu_id = mtu.mtu_id
				AND mtu.mtu_situacao <> 3
			INNER JOIN ACA_CurriculoPeriodo crp WITH(NOLOCK)
				ON crp.cur_id = mtu.cur_id
				AND crp.crr_id = mtu.crr_id
				AND crp.crp_id = mtu.crp_id
				AND crp.crp_situacao <> 3
			INNER JOIN ACA_Aluno alu WITH(NOLOCK)
				ON mtd.alu_id = alu.alu_id		
			INNER JOIN VW_DadosAlunoPessoa pes 
				ON alu.alu_id = pes.alu_id
			INNER JOIN CLS_TurmaNota tnt WITH (NOLOCK)
				ON tnt.tud_id = mtd.tud_id	
			INNER JOIN TUR_Turma tur WITH(NOLOCK)
				ON tur.tur_id = mtu.tur_id
				AND tur.tur_situacao <> 3	
			INNER JOIN ACA_FormatoAvaliacao fav WITH(NOLOCK)
				ON tur.fav_id = fav.fav_id
				AND fav.fav_situacao <> 3					
			LEFT JOIN CLS_TurmaAula TAU WITH(NOLOCK)
				ON tnt.tud_id = TAU.tud_id 
					AND tnt.tau_id = TAU.tau_id
					AND TAU.tud_id = tnt.tud_id
					AND TAU.tpc_id = tnt.tpc_id
					AND TAU.tau_situacao <> 3
			LEFT JOIN CLS_TurmaNotaAluno tna WITH (NOLOCK)		
				ON tna.tud_id = mtd.tud_id
					AND tna.tnt_id = tnt.tnt_id
					AND tna.alu_id = mtd.alu_id
					AND tna.mtu_id = mtd.mtu_id
					AND tna.mtd_id = mtd.mtd_id
					AND tna.tna_situacao <> 3
			LEFT JOIN ACA_AlunoJustificativaFalta afj WITH(NOLOCK)
			   ON  afj.alu_id = mtd.alu_id
				   AND afj.afj_situacao <> 3
				   AND (tau_data >= afj.afj_dataInicio)
				   AND ((afj.afj_dataFim  IS NULL) OR (tau_data <= afj.afj_dataFim))
			LEFT JOIN ACA_TipoJustificativaFalta tjf WITH(NOLOCK)
			   ON tjf.tjf_id = afj.tjf_id
				   AND tjf.tjf_situacao <> 3
			LEFT OUTER JOIN CLS_AlunoAvaliacaoTurma Aat WITH(NOLOCK)
				ON (Aat.tur_id = Mtu.tur_id)
					AND (Aat.alu_id = Mtu.alu_id)
					AND (Aat.mtu_id = Mtu.mtu_id)
					AND (Aat.fav_id = tur.fav_id)
					AND (Aat.aat_situacao <> 3)
					AND Aat.ava_id =
					(
						SELECT  TOP 1 ava.ava_id
						FROM    ACA_Avaliacao ava WITH(NOLOCK)
						WHERE
							 ava.fav_id = tur.fav_id
							 AND ava.tpc_id = tau.tpc_id
							 AND ava.ava_situacao <> 3
					)	
		WHERE 
			-- Trazer somente ativos, ou também os inativos, quando a flag for true
			(mtd_situacao = 1 OR (@trazerInativos = 1 AND mtd_situacao = 5 AND ISNULL(mtd_numeroChamada, 0) >= 0))	
			AND alu_situacao <> 3
			AND tnt.tnt_situacao <> 3
			AND mtd.tud_id = @tud_id
			AND tnt.tnt_id = @tnt_id	 	
			AND alu.ent_id = @ent_id		
			-- Valida o período de matrícula e saída do aluno (se está dentro do período atual).
			AND (@tpc_id IS NULL OR mtd.mtd_dataMatricula <= @dataFimPeriodo)
			AND ((@tpc_id IS NULL) OR (mtd_situacao <> 5 OR mtd.mtd_dataSaida >= @dataInicioPeriodo))
			---- Valida o período de matrícula e saída do aluno (se está dentro da data da aula).
			AND (TAU.tau_id IS NULL 
					OR (
						(DATEDIFF(DAY, Mtd.mtd_dataMatricula, tau.tau_data) >= 0)
						AND (mtd_situacao <> 5 OR ISNULL((DATEDIFF(DAY, Mtd.mtd_dataSaida, tau.tau_data)), 0) <= 0)
					)
			)	
		GROUP BY
			mtd.alu_id
			, mtd.mtu_id
			, mtd.mtd_id
			, mtd.tud_id
			, tnt.tnt_id
			, tnt.tnt_efetivado
			, pes.pes_nome
			, mtd_situacao
			, mtd.mtd_numeroChamada 
			, tna.tna_avaliacao
			, tna.tna_participante
			, tna.tna_relatorio
			, afj.afj_id
			, tjf.tjf_abonaFalta
			, TAU.tau_id
			, TAU.tud_id
			, TAU.tau_numeroAulas
			, fav.fav_tipoApuracaoFrequencia
			, crp.crp_controleTempo
		ORDER BY 	
			CASE WHEN @ordenacao = 0 THEN 
				CASE WHEN ISNULL(Mtd.mtd_numeroChamada,0) <= 0 THEN 1 ELSE 0 END
			END ASC
			, CASE WHEN @ordenacao = 0 THEN ISNULL(Mtd.mtd_numeroChamada,0) END ASC
			, CASE WHEN @ordenacao = 1 THEN pes.pes_nome END ASC
	END
	ELSE
	
	-- Se for um componente da regência, traz os dados baseados nas aulas da regência
	BEGIN
		DECLARE @tud_idRegencia BIGINT = NULL;

		-- Caso seja Componente de regência pega o tud_id da regência
		SELECT 
			@tud_idRegencia = TUR_TUD_REG.tud_id
		FROM 
			dbo.TUR_TurmaDisciplina TUD WITH(NOLOCK)
			INNER JOIN dbo.TUR_TurmaDisciplinaRelDisciplina TUD_DIS WITH(NOLOCK)
				ON	TUD_DIS.tud_id = TUD.tud_id
			INNER JOIN dbo.TUR_TurmaRelTurmaDisciplina TUR_TUD WITH(NOLOCK)
				ON	TUR_TUD.tud_id = TUD.tud_id
			INNER JOIN dbo.ACA_CurriculoDisciplina CRD WITH(NOLOCK)
				ON	CRD.dis_id = TUD_DIS.dis_id
			INNER JOIN dbo.ACA_CurriculoDisciplina CRDREG WITH(NOLOCK)
				ON	CRDREG.cur_id = CRD.cur_id
				AND CRDREG.crr_id = CRD.crr_id
				AND CRDREG.crp_id = CRD.crp_id
			INNER JOIN dbo.TUR_TurmaDisciplinaRelDisciplina TUD_DIS_REG WITH(NOLOCK)
				ON	TUD_DIS_REG.dis_id = CRDREG.dis_id
			INNER JOIN dbo.TUR_TurmaRelTurmaDisciplina TUR_TUD_REG WITH(NOLOCK)
				ON	TUR_TUD_REG.tud_id = TUD_DIS_REG.tud_id
				AND TUR_TUD_REG.tur_id = TUR_TUD.tur_id
		WHERE 
			TUD.tud_id = @tud_id
			AND TUD.tud_tipo = 12
			AND CRDREG.crd_tipo = 11
			-- Exclusão Lógica
			AND TUD.tud_situacao <> 3
			AND CRD.crd_situacao <> 3
			AND CRDREG.crd_situacao <> 3
	
		--Selecina as movimentações que possuem matrícula anterior
		;WITH TabelaMovimentacao AS (
			SELECT
				alu_id,
				mtu_idAnterior,
				tmv_nome    
			FROM
				MTR_Movimentacao MOV WITH (NOLOCK) 
				INNER JOIN ACA_TipoMovimentacao TMV WITH (NOLOCK) 
					ON MOV.tmv_idSaida = TMV.tmv_id
			WHERE
				mov_situacao NOT IN (3,4)
				AND tmv_situacao <> 3
				AND mtu_idAnterior IS NOT NULL
		)	
		
		-- Aulas da regência
		, tbAulas AS 
		(
			SELECT 
				tau.tud_id,
				tau.tau_id,
				tau.tau_data,
				tau.tau_numeroAulas,
				tau.tpc_id
			FROM 
				CLS_TurmaAula tau WITH(NOLOCK)
			WHERE
				tau.tud_id = @tud_idRegencia
				AND tau.tpc_id = @tpc_id
				AND tau.tau_situacao <> 3
		)
		
				
		SELECT	
			mtd.alu_id
			, mtd.mtu_id
			, mtd.mtd_id
			, mtd.tud_id
			, tnt.tnt_id
			, tnt.tnt_efetivado
			, pes.pes_nome + 
				(
					CASE WHEN mtd_situacao = 5 THEN ISNULL((SELECT TOP 1 ' (' + tmv_nome + ')' FROM TabelaMovimentacao MOV WITH(NOLOCK) WHERE MOV.mtu_idAnterior = mtd.mtu_id AND MOV.alu_id = mtd.alu_id), ' (Inativo)')
						ELSE '' END
				) AS pes_nome
			,  CASE WHEN mtd.mtd_numeroChamada > 0 THEN CAST(mtd.mtd_numeroChamada AS VARCHAR)
						ELSE '-' END as mtd_numeroChamada
			, tna.tna_avaliacao
			, tna.tna_relatorio
			-- (FALTA JUSTIFICADA) 0 - não possui FJ / 1 - possui FJ que abona / 2 - possui FJ que não abona 
			, CASE WHEN afj.afj_id IS NULL
					THEN '0'					    						    
					ELSE (CASE WHEN tjf.tjf_abonaFalta = 1 THEN '1' ELSE '2' END)
			   END AS falta_justificada
			-- (ALUNO AUSENTE) 1 - aluno ausente que não possui falta justificada 
			, CAST( CASE WHEN @tipoLancamento IN  (1,4,5) AND TAU.tau_id IS NOT NULL THEN
				-- Aulas planejadas
				  CASE WHEN  
					((SELECT 
						taa_frequencia 
					 FROM 
						CLS_TurmaAulaAluno WITH (NOLOCK)
					 WHERE 
						tud_id = TAU.tud_id
						AND tau_id = TAU.tau_id
						AND alu_id = mtd.alu_id
						AND mtu_id = mtd.mtu_id) = TAU.tau_numeroAulas)
						AND afj.afj_id IS NULL
					THEN 1 ELSE 0 END
					-- Período ( Nâo terá esta funcionalidade )
					ELSE 0				       
			END AS BIT) AS aluno_ausente 
			, NULL AS tca_numeroAvaliacao
			-- Verifica se há dispensa de disciplina para o aluno.
			, 0 AS dispensadisciplina
			, ISNULL(tna.tna_participante, 0) AS tna_participante
		FROM 
			MTR_MatriculaTurmaDisciplina mtd WITH(NOLOCK)
			INNER JOIN MTR_MatriculaTurma mtu WITH(NOLOCK)
				ON mtd.alu_id = mtu.alu_id 
				AND mtd.mtu_id = mtu.mtu_id
				AND mtu.mtu_situacao <> 3
			INNER JOIN ACA_Aluno alu WITH(NOLOCK)
				ON mtd.alu_id = alu.alu_id		
			INNER JOIN VW_DadosAlunoPessoa pes 
				ON alu.alu_id = pes.alu_id
			INNER JOIN CLS_TurmaNota tnt WITH (NOLOCK)
				ON tnt.tud_id = mtd.tud_id	
			INNER JOIN TUR_Turma tur WITH(NOLOCK)
				ON tur.tur_id = mtu.tur_id
				AND tur.tur_situacao <> 3						
			LEFT JOIN tbAulas TAU 
				ON Tau.tau_data = Tnt.tnt_data
			LEFT JOIN CLS_TurmaNotaAluno tna WITH (NOLOCK)		
				ON tna.tud_id = mtd.tud_id
					AND tna.tnt_id = tnt.tnt_id
					AND tna.alu_id = mtd.alu_id
					AND tna.mtu_id = mtd.mtu_id
					AND tna.mtd_id = mtd.mtd_id
					AND tna.tna_situacao <> 3
			LEFT JOIN ACA_AlunoJustificativaFalta afj WITH(NOLOCK)
			   ON  afj.alu_id = mtd.alu_id
				   AND afj.afj_situacao <> 3
				   AND (tau_data >= afj.afj_dataInicio)
				   AND ((afj.afj_dataFim  IS NULL) OR (tau_data <= afj.afj_dataFim))
			LEFT JOIN ACA_TipoJustificativaFalta tjf WITH(NOLOCK)
			   ON tjf.tjf_id = afj.tjf_id
				   AND tjf.tjf_situacao <> 3
			LEFT OUTER JOIN CLS_AlunoAvaliacaoTurma Aat WITH(NOLOCK)
				ON (Aat.tur_id = Mtu.tur_id)
					AND (Aat.alu_id = Mtu.alu_id)
					AND (Aat.mtu_id = Mtu.mtu_id)
					AND (Aat.fav_id = tur.fav_id)
					AND (Aat.aat_situacao <> 3)
					AND Aat.ava_id =
					(
						SELECT  TOP 1 ava.ava_id
						FROM    ACA_Avaliacao ava WITH(NOLOCK)
						WHERE
							 ava.fav_id = tur.fav_id
							 AND ava.tpc_id = tau.tpc_id
							 AND ava.ava_situacao <> 3
					)	
		WHERE 
			-- Trazer somente ativos, ou também os inativos, quando a flag for true
			(mtd_situacao = 1 OR (@trazerInativos = 1 AND mtd_situacao = 5 AND ISNULL(mtd_numeroChamada, 0) >= 0))	
			AND alu_situacao <> 3
			AND tnt.tnt_situacao <> 3
			AND mtd.tud_id = @tud_id
			AND tnt.tnt_id = @tnt_id	 	
			AND alu.ent_id = @ent_id		
			-- Valida o período de matrícula e saída do aluno (se está dentro do período atual).
			AND (@tpc_id IS NULL OR mtd.mtd_dataMatricula <= @dataFimPeriodo)
			AND ((@tpc_id IS NULL) OR (mtd_situacao <> 5 OR mtd.mtd_dataSaida >= @dataInicioPeriodo))
			---- Valida o período de matrícula e saída do aluno (se está dentro da data da aula).
			AND (TAU.tau_id IS NULL 
					OR (
						(DATEDIFF(DAY, Mtd.mtd_dataMatricula, tau.tau_data) >= 0)
						AND (mtd_situacao <> 5 OR ISNULL((DATEDIFF(DAY, Mtd.mtd_dataSaida, tau.tau_data)), 0) <= 0)
					)
			)	
		GROUP BY
			mtd.alu_id
			, mtd.mtu_id
			, mtd.mtd_id
			, mtd.tud_id
			, tnt.tnt_id
			, tnt.tnt_efetivado
			, pes.pes_nome
			, mtd_situacao
			, mtd.mtd_numeroChamada 
			, tna.tna_avaliacao
			, tna.tna_participante
			, tna.tna_relatorio
			, afj.afj_id
			, tjf.tjf_abonaFalta
			, TAU.tau_id
			, TAU.tud_id
			, TAU.tau_numeroAulas
		ORDER BY 	
			CASE WHEN @ordenacao = 0 THEN 
				CASE WHEN ISNULL(Mtd.mtd_numeroChamada,0) <= 0 THEN 1 ELSE 0 END
			END ASC
			, CASE WHEN @ordenacao = 0 THEN ISNULL(Mtd.mtd_numeroChamada,0) END ASC
			, CASE WHEN @ordenacao = 1 THEN pes.pes_nome END ASC
	END
END

GO
PRINT N'Creating [dbo].[NEW_ACA_AlunoJustificativaFalta_VerificaJustificativaIntervalo]'
GO
-- =======================================================
-- Author:		Ivan Roberto Pimentel
-- Create date: 22/07/2011 12:09
-- Description: Retorna todas as justificativa de falta
--				ativas, filtrando pelo: alu_id e pela intervalo da justificativa
-- =======================================================
CREATE PROCEDURE [dbo].[NEW_ACA_AlunoJustificativaFalta_VerificaJustificativaIntervalo]
	@alu_id BIGINT,
	@afj_id INT, 
	@afj_dataInicio DATE,
	@afj_dataFim DATE
AS
BEGIN
	
	SELECT 
		alu_id
		,afj_id
		,tjf.tjf_id
		,tjf.tjf_nome
		,afj_dataInicio
		,afj_dataFim
		,afj_situacao
		,afj_dataCriacao
		,afj_dataAlteracao
	FROM 
		ACA_AlunoJustificativaFalta afj WITH(NOLOCK)
		INNER JOIN ACA_TipoJustificativaFalta tjf WITH(NOLOCK)
		ON afj.tjf_id = tjf.tjf_id
		AND tjf.tjf_situacao <> 3
	WHERE 
		afj_situacao <> 3
		AND alu_id = @alu_id
		AND afj_id <> @afj_id
		AND (	((@afj_dataInicio <= afj_dataInicio and @afj_dataFim >= afj_dataFim) or (@afj_dataInicio <= afj_dataInicio and @afj_dataFim <= afj_dataFim and @afj_dataFim >= afj_dataInicio) or (@afj_dataInicio >= afj_dataInicio and @afj_dataFim >= afj_dataFim and @afj_dataInicio <= afj_dataFim) or (@afj_dataInicio >= afj_dataInicio and @afj_dataFim <= afj_dataFim))
				OR  ((@afj_dataInicio >= afj_dataInicio and @afj_dataFim >= afj_dataInicio and afj_dataFim is null) or (@afj_dataInicio <= afj_dataInicio and @afj_dataFim >= afj_dataInicio and afj_dataFim is null))
				OR  ((@afj_dataInicio <= afj_dataInicio and @afj_dataInicio <= afj_dataFim and @afj_dataFim is null) or (@afj_dataInicio >= afj_dataInicio and @afj_dataInicio <= afj_dataFim and @afj_dataFim is null))
				OR  ((@afj_dataFim is null and afj_dataFim is null))
			)
		
		
	SELECT @@ROWCOUNT	
	
END

GO
PRINT N'Creating [dbo].[STP_ACA_AlunoJustificativaFalta_UPDATE]'
GO

CREATE PROCEDURE [dbo].[STP_ACA_AlunoJustificativaFalta_UPDATE]
	@alu_id BIGINT
	, @afj_id INT
	, @tjf_id INT
	, @afj_dataInicio DATETIME
	, @afj_dataFim DATETIME
	, @afj_situacao TINYINT
	, @afj_dataCriacao DATETIME
	, @afj_dataAlteracao DATETIME
	, @pro_id UNIQUEIDENTIFIER
	, @afj_observacao VARCHAR(MAX)

AS
BEGIN
	UPDATE ACA_AlunoJustificativaFalta 
	SET 
		tjf_id = @tjf_id 
		, afj_dataInicio = @afj_dataInicio 
		, afj_dataFim = @afj_dataFim 
		, afj_situacao = @afj_situacao 
		, afj_dataCriacao = @afj_dataCriacao 
		, afj_dataAlteracao = @afj_dataAlteracao 
		, pro_id = @pro_id 
		, afj_observacao = @afj_observacao

	WHERE 
		alu_id = @alu_id 
		AND afj_id = @afj_id 

		
	DECLARE @ret INT
	SELECT @ret = ISNULL(@@ROWCOUNT,-1)
	RETURN @ret
	
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
PRINT N'Altering [dbo].[NEW_CLS_TurmaNota_SelectBy_Periodo_NotaAlunoTodos]'
GO
-- ========================================================================
-- Author:		Juliano Real
-- Create date: 14/05/2014 14:50
-- Description:	Retorna as todas as Atividades(por disciplina e secretaria) 
-- com as notas do aluno, caso passado 
--				o alu_id, mtu_id e mtd_id.

-- Author:		Katiusca Murari
-- Alter date:  19/12/2014
-- Description:	Adicao de campo tau_id.

-- Alterado: Marcia Haga - 06/04/2015
-- Description: Alterado para retornar nota do tipo relatorio para a avaliacao da secretaria.
-- Alterado para nao filtrar o tipo da disciplina para as avaliacoes da secretaria, caso o
-- tud_tipo seja disciplina principal (global). 

-- Alterado: Marcia Haga - 17/04/2015
-- Description: Alterada validacao da posicao do docente nas aulas da regencia.

-- Alterado: Marcia Haga - 23/04/2015
-- Description: Alterado para nao retornar valor nulo no tau_id.

-- Alterado: Marcia Haga - 14/05/2015
-- Description: Alterado para retornar as avaliacoes automaticas.

-- Alterado: Marcia Haga - 18/05/2015
-- Description: Alterado para nao retornar as avaliacoes automaticas 
-- para as disciplinas componentes da regencia.

-- Alterado: Marcia Haga - 03/06/2015
-- Description: Alterada regra para relacionar a disciplina compartilhada.

-- Alterado: Marcia Haga - 17/06/2015
-- Description: Alterada regra da avaliacao automatica.

-- Alterado: Marcia Haga - 18/06/2015
-- Description: Ajustes para as alteracoes nas tabelas de configuracao de atividade.

-- Alterado: Marcia Haga - 24/06/2015
-- Description: Alterada a ordenacao para considerar as avaliacoes relacionada.
-- Adicionado parametro para trazer ou nao as avaliacoes da secretaria.

-- Alterado: Marcia Haga - 25/06/2015
-- Description: Alterada validacao para considerar aluno ausente por avaliacao.

-- Alterado: Marcia Haga - 26/06/2015
-- Description: Alterado para retornar o tnt_id da avaliacao relacionada pai.

-- Alterado: Haila Pelloso - 07/07/2015
-- Description: Alterada regra para selecionar apenas atividades do próprio docente caso não tenha permissão de visualizar a posição em que está.

-- Alterado: Haila Pelloso - 13/07/2015
-- Description: Adicionado select de log de alteração de média (listão de avaliações)

---- Alterado: Marcia Haga - 14/07/2015
---- Description: Alterado para retornar a avaliacao automatica(Licao de casa) cadastrada, 
---- independente da permissao do usuario.

---- Alterado: Marcia Haga - 21/07/2015 
---- Description: Corrigido para pegar a configuracao da atividade avaliativa 
---- considerando o curriculo da turma.
-- ========================================================================
ALTER PROCEDURE [dbo].[NEW_CLS_TurmaNota_SelectBy_Periodo_NotaAlunoTodos]	
	@tud_id BIGINT 
	, @tpc_id INT 
	, @usu_id UNIQUEIDENTIFIER 
	, @tdt_posicao TINYINT	
	, @tud_idRelacionada BIGINT
	, @usuario_superior BIT	
	, @trazerAvaSecretaria BIT = 1
	, @ausenteTurmaNota BIT = 0
	, @dtTurmas TipoTabela_Turma READONLY
AS
BEGIN
	DECLARE @tud_tipo TINYINT;
	SELECT 
		@tud_tipo = ISNULL(tud.tud_tipo,0)
	FROM
		TUR_TurmaDisciplina tud WITH(NOLOCK)
	WHERE
		tud.tud_id = @tud_id
		AND tud.tud_situacao <> 3

	DECLARE @PermissaoDocenteEdicao TABLE (tdt_posicaoPermissao INT);
	DECLARE @PermissaoDocenteConsulta TABLE (tdt_posicaoPermissao INT, pdc_permissaoConsulta BIT)

	DECLARE @tbLog TABLE 
					(
						tud_id BIGINT,
						tpc_id INT,
						lam_tipo TINYINT,
						usu_id UNIQUEIDENTIFIER,
						lam_data DATETIME
					)
	INSERT INTO @tbLog SELECT TOP 1 tud_id, tpc_id, lam_tipo, usu_id, lam_data 
					   FROM FN_RetornaUltimaAlteracaoAvaliacaoMedia(@tud_id, @tpc_id, 1)
	DECLARE @lam_data DATETIME, @usu_nome VARCHAR(200)

	SELECT TOP 1 
		@lam_data = lam_data,
		@usu_nome = ISNULL(pesAlteracao.pes_nome,usuAlteracao.usu_Login)
	FROM @tbLog lam
	LEFT JOIN Synonym_SYS_Usuario AS usuAlteracao WITH(NOLOCK)
		ON usuAlteracao.usu_id = lam.usu_id
		AND usuAlteracao.usu_situacao <> 3
	LEFT JOIN Synonym_PES_Pessoa AS pesAlteracao WITH(NOLOCK)
		ON pesAlteracao.pes_id = usuAlteracao.pes_id 
		AND pesAlteracao.pes_situacao <> 3 	
	
	;WITH tbDadosPermissaoDocenteConsulta AS	
	(
		SELECT 
			tdcPermissao.tdc_posicao AS tdt_posicaoPermissao
			, pdc.pdc_permissaoConsulta
		FROM
			ACA_TipoDocente tdc WITH(NOLOCK)
			INNER JOIN CFG_PermissaoDocente pdc WITH(NOLOCK)
				ON tdc.tdc_id = pdc.tdc_id
				AND pdc.pdc_modulo = 1
				AND (@usuario_superior = 1 OR pdc.pdc_permissaoConsulta = 1) -- retonar as permissões de consulta
				AND pdc.pdc_situacao <> 3 
			INNER JOIN ACA_TipoDocente tdcPermissao WITH(NOLOCK)
				ON tdcPermissao.tdc_id = pdc.tdc_idPermissao
				AND tdcPermissao.tdc_situacao <> 3
		WHERE
			(tdc.tdc_posicao = ISNULL(@tdt_posicao, tdc.tdc_posicao) OR @usuario_superior = 1)
			AND tdc.tdc_situacao <> 3
			
		UNION 
		
		SELECT @tdt_posicao, 0 AS pdc_permissaoConsulta
	)
	INSERT INTO @PermissaoDocenteConsulta (tdt_posicaoPermissao, pdc_permissaoConsulta)
	SELECT 
		pdc.tdt_posicaoPermissao
		, MAX(pdc.pdc_permissaoConsulta) AS pdc_permissaoConsulta
	FROM
		tbDadosPermissaoDocenteConsulta pdc WITH(NOLOCK)
	GROUP BY
		pdc.tdt_posicaoPermissao

	INSERT INTO @PermissaoDocenteEdicao (tdt_posicaoPermissao)
	SELECT 
		tdcPermissao.tdc_posicao AS tdt_posicaoPermissao
	FROM
		ACA_TipoDocente tdc WITH(NOLOCK)
		INNER JOIN CFG_PermissaoDocente pdc WITH(NOLOCK)
			ON tdc.tdc_id = pdc.tdc_id
			AND pdc.pdc_modulo = 1
			AND pdc.pdc_permissaoEdicao = 1  -- retonar as permissões de edição
			AND pdc.pdc_situacao <> 3 
		INNER JOIN ACA_TipoDocente tdcPermissao WITH(NOLOCK)
			ON tdcPermissao.tdc_id = pdc.tdc_idPermissao
			AND tdcPermissao.tdc_situacao <> 3
	WHERE
		tdc.tdc_posicao = ISNULL(@tdt_posicao, tdc.tdc_posicao)
		AND tdc.tdc_situacao <> 3
		AND tdc.tdc_id IN (1,6) --Titular / Segundo titular

	DECLARE @tbMatricula TABLE (
		alu_id BIGINT
		, tud_id BIGINT
		, mtu_id INT
		, mtd_id INT
		, mtd_situacao TINYINT
		, AlunoDispensado BIT
		, cal_id INT
		, cap_id INT
		, fav_id INT
		, fav_tipoLancamentoFrequencia TINYINT
		, fav_tipoApuracaoFrequencia TINYINT
		, atm_media VARCHAR(20)
		, tds_id INT
		, esa_idDocente INT
	);

	DECLARE @tbAvaliacoes TABLE (
		tud_id BIGINT
		, tnt_id INT
		, nome VARCHAR(100)
		, tdt_posicao TINYINT
		, usu_id UNIQUEIDENTIFIER
		, tnt_data VARCHAR(10)
		, avaliacao VARCHAR(20)
		, relatorio VARCHAR(MAX)
		, ausente BIT
		, falta_justificada VARCHAR(1)
		, tnt_efetivado BIT
		, alu_id BIGINT
		, mtu_id INT
		, mtd_id INT
		, tnt_exclusiva BIT
		, tna_participante BIT
		, AlunoDispensado BIT
		, permissaoAlteracao INT
		, mtd_situacao TINYINT
		, atm_media VARCHAR(20)
		, avs_id INT
		, naoConstaMedia BIT
		, aas_id INT
		, PossuiNota BIT
		, tau_id INT
		, aau_id INT
		, aaa_id INT
		, fav_id INT
		, tav_id INT
		, qat_id INT
		, semData BIT
		, ordem INT
		, tnt_idRelacionadaPai INT 
	);

	DECLARE @tud_idRegencia BIGINT = NULL;

	-- Avaliações automáticas
	DECLARE @qualificadorLicaoDeCasa TINYINT = 5;
	DECLARE @tavNomeLicaoDeCasa VARCHAR(100) = '';
	DECLARE @tavIdLicaoDeCasa INT = -1;
	DECLARE @esaIdDocente INT = -1;
	DECLARE @ordemLicaoDeCasa INT = 0;
	-- Verifico se esta configurado para possuir atividade automatica	
	DECLARE @possuiAtividadeAutomatica BIT = 0;
	-- Nao retorna as avaliacoes automaticas para as disciplinas componentes da regencia
	IF (@tud_tipo <> 12)
	BEGIN			
		;WITH DadosTurma AS
		(
			SELECT 
				tur.esc_id
				, tur.uni_id
				, cal.cal_ano
				, crd.cur_id
				, crd.crr_id
				, crd.crp_id
				, relDis.dis_id
				, fav.esa_idDocente
				, crp.crp_ordem
			FROM TUR_TurmaDisciplina tud WITH(NOLOCK)
			INNER JOIN TUR_TurmaRelTurmaDisciplina relTud WITH(NOLOCK)
				ON relTud.tud_id = tud.tud_id
			INNER JOIN TUR_Turma tur WITH(NOLOCK)
				ON tur.tur_id = relTud.tur_id
				AND tur.tur_situacao <> 3
			INNER JOIN ACA_FormatoAvaliacao fav WITH(NOLOCK)
				ON fav.fav_id = tur.fav_id
				AND fav.fav_situacao <> 3
			INNER JOIN ACA_EscalaAvaliacao esa WITH(NOLOCK)
				ON esa.esa_id = fav.esa_idDocente
				AND esa.esa_tipo = 1 --numerica
				AND esa.esa_situacao <> 3
			INNER JOIN ACA_CalendarioAnual cal WITH(NOLOCK)
				ON cal.cal_id = tur.cal_id
				AND cal.cal_situacao <> 3
			INNER JOIN TUR_TurmaDisciplinaRelDisciplina relDis WITH(NOLOCK)
				ON relDis.tud_id = tud.tud_id
			INNER JOIN ACA_Disciplina dis WITH(NOLOCK)
				ON dis.dis_id = relDis.dis_id
				AND dis.dis_situacao <> 3
			INNER JOIN ACA_CurriculoDisciplina crd WITH(NOLOCK)
				ON crd.dis_id = dis.dis_id
				AND crd.crd_situacao <> 3
			INNER JOIN ACA_CurriculoPeriodo crp WITH(NOLOCK)
				ON crp.cur_id = crd.cur_id
				AND crp.crr_id = crd.crr_id
				AND crp.crp_id = crd.crp_id
				AND crp.crp_situacao <> 3
			INNER JOIN TUR_TurmaCurriculo tcr WITH(NOLOCK)
				ON tcr.tur_id = tur.tur_id
				AND tcr.cur_id = crd.cur_id
				AND tcr.crr_id = crd.crr_id
				AND tcr.crp_id = crd.crp_id
				AND tcr.tcr_situacao <> 3
			WHERE
				tud.tud_id = @tud_id
		)	
		SELECT TOP 1 
			@possuiAtividadeAutomatica = caa_possuiAtividadeAutomatica
			, @tavNomeLicaoDeCasa = tav.tav_nome
			, @tavIdLicaoDeCasa = tav.tav_id
			, @ordemLicaoDeCasa = qat.qat_ordem
			, @esaIdDocente = tur.esa_idDocente
		FROM DadosTurma tur
		INNER JOIN CLS_ConfiguracaoAtividade caa WITH(NOLOCK)
			ON caa.caa_anoLetivo = tur.cal_ano
			AND caa.esc_id = tur.esc_id
			AND caa.uni_id = tur.uni_id
			AND caa.cur_id = tur.cur_id
			AND caa.crr_id = tur.crr_id
			AND caa.crp_id = tur.crp_id
			AND caa.dis_id = tur.dis_id
			AND caa.caa_situacao <> 3
		INNER JOIN CLS_ConfiguracaoAtividadeTipoAtividade relTav WITH(NOLOCK)
			ON relTav.caa_id = caa.caa_id
		INNER JOIN CLS_TipoAtividadeAvaliativa tav WITH(NOLOCK)
			ON tav.tav_id = relTav.tav_id
			AND tav.tav_situacao <> 3		
		INNER JOIN CLS_QualificadorAtividade qat WITH(NOLOCK)
			ON qat.qat_id = tav.qat_id
			AND qat.qat_situacao <> 3
			AND qat.qat_id = @qualificadorLicaoDeCasa
		ORDER BY
			ISNULL(tur.crp_ordem, 0)
	END
	--

	IF (@tud_tipo = 15)
	BEGIN
	
		INSERT INTO @tbMatricula
		SELECT				
			Mtd.alu_id
			, Mtd.tud_id
			, Mtd.mtu_id
			, Mtd.mtd_id
			, mtd.mtd_situacao
			, CAST(0 AS BIT) --AlunoDispensado
			, Turm.cal_id
			, Cap.cap_id
			, Fav.fav_id
			, Fav.fav_tipoLancamentoFrequencia
			, Fav.fav_tipoApuracaoFrequencia
			, Atm.atm_media
			, Dis.tds_id
			, Fav.esa_idDocente
		FROM MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
		INNER JOIN TUR_TurmaDisciplinaRelDisciplina AS TurDis WITH (NOLOCK)
			ON Mtd.tud_id = TurDis.tud_id
		INNER JOIN ACA_Disciplina AS Dis WITH (NOLOCK)
			ON Dis.dis_id = TurDis.dis_id	
		INNER JOIN MTR_MatriculaTurma Mtu WITH(NOLOCK)
			ON Mtu.alu_id = Mtd.alu_id
			AND Mtu.mtu_id = Mtd.mtu_id
			AND Mtu.mtu_situacao <> 3
		INNER JOIN @dtTurmas dtt
			ON Mtu.tur_id = dtt.tur_id
		INNER JOIN TUR_TurmaDisciplina Tud WITH (NOLOCK)	
			ON Mtd.tud_id = Tud.tud_id
		INNER JOIN TUR_TurmaRelTurmaDisciplina RelTud WITH (NOLOCK)
			ON RelTud.tud_id = Tud.tud_id
		INNER JOIN TUR_Turma Turm WITH(NOLOCK)
			ON Turm.tur_id = RelTud.tur_id
		INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
			ON Cap.cal_id = Turm.cal_id
			AND Cap.tpc_id = @tpc_id
			AND (Mtd.mtd_dataMatricula <= Cap.cap_dataFim)
			AND (
					Mtd.mtd_dataSaida IS NULL
					OR Mtd.mtd_dataSaida >= Cap.cap_dataInicio					
				)
			AND Cap.cap_situacao <> 3
		INNER JOIN ACA_FormatoAvaliacao Fav WITH(NOLOCK)
			ON Fav.fav_id = Turm.fav_id
		LEFT JOIN CLS_AlunoAvaliacaoTurmaDisciplinaMedia Atm WITH(NOLOCK)
			ON	Atm.tud_id = @tud_id
			AND Atm.alu_id = Mtd.alu_id
			AND Atm.mtu_id = Mtd.mtu_id
			AND Atm.mtd_id = Mtd.mtd_id
			AND Atm.tpc_id = @tpc_id
			AND Atm.atm_situacao <> 3			
		WHERE	
			Mtd.tud_id = @tud_id
			AND Mtd.mtd_situacao <> 3

	END
	ELSE
	BEGIN
		
		-- Se for um componente da regência, traz os dados baseados nas aulas da regência
		IF (@tud_tipo = 12)
		BEGIN
			SELECT 
				@tud_idRegencia = TUR_TUD_REG.tud_id
			FROM 
				dbo.TUR_TurmaDisciplina TUD WITH(NOLOCK)
				INNER JOIN dbo.TUR_TurmaDisciplinaRelDisciplina TUD_DIS WITH(NOLOCK)
					ON	TUD.tud_id = TUD_DIS.tud_id
				INNER JOIN dbo.TUR_TurmaRelTurmaDisciplina TUR_TUD WITH(NOLOCK)
					ON	TUD.tud_id = TUR_TUD.tud_id
				INNER JOIN dbo.ACA_CurriculoDisciplina CRD WITH(NOLOCK)
					ON	TUD_DIS.dis_id = CRD.dis_id
				INNER JOIN dbo.ACA_CurriculoDisciplina CRDREG WITH(NOLOCK)
					ON	CRD.cur_id = CRDREG.cur_id
					AND CRD.crr_id = CRDREG.crr_id
					AND CRD.crp_id = CRDREG.crp_id
				INNER JOIN dbo.TUR_TurmaDisciplinaRelDisciplina TUD_DIS_REG WITH(NOLOCK)
					ON	CRDREG.dis_id = TUD_DIS_REG.dis_id
				INNER JOIN dbo.TUR_TurmaRelTurmaDisciplina TUR_TUD_REG WITH(NOLOCK)
					ON	TUD_DIS_REG.tud_id = TUR_TUD_REG.tud_id
					AND TUR_TUD.tur_id = TUR_TUD_REG.tur_id
			WHERE 
				TUD.tud_id = @tud_id
				AND TUD.tud_tipo = 12
				AND CRDREG.crd_tipo = 11
				-- Exclusão Lógica
				AND TUD.tud_situacao <> 3
				AND CRD.crd_situacao <> 3
				AND CRDREG.crd_situacao <> 3
		END
	
		INSERT INTO @tbMatricula
		SELECT				
			Mtd.alu_id
			, Mtd.tud_id
			, Mtd.mtu_id
			, Mtd.mtd_id
			, mtd.mtd_situacao
			, CAST(0 AS BIT) --AlunoDispensado
			, Turm.cal_id
			, Cap.cap_id
			, Fav.fav_id
			, Fav.fav_tipoLancamentoFrequencia
			, Fav.fav_tipoApuracaoFrequencia
			, Atm.atm_media
			, Dis.tds_id
			, Fav.esa_idDocente
		FROM MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
		INNER JOIN TUR_TurmaDisciplinaRelDisciplina AS TurDis WITH (NOLOCK)
			ON Mtd.tud_id = TurDis.tud_id
		INNER JOIN ACA_Disciplina AS Dis WITH (NOLOCK)
			ON Dis.dis_id = TurDis.dis_id	
		INNER JOIN MTR_MatriculaTurma Mtu WITH(NOLOCK)
			ON Mtu.alu_id = Mtd.alu_id
			AND Mtu.mtu_id = Mtd.mtu_id
		INNER JOIN TUR_Turma Tur WITH(NOLOCK)
			ON Tur.tur_id = Mtu.tur_id
		INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
			ON Cap.cal_id = Tur.cal_id
			AND Cap.tpc_id = @tpc_id
			AND Cap.cap_situacao <> 3
		INNER JOIN TUR_TurmaDisciplina Tud WITH (NOLOCK)	
			ON Mtd.tud_id = Tud.tud_id
		INNER JOIN TUR_TurmaRelTurmaDisciplina RelTud WITH (NOLOCK)
			ON RelTud.tud_id = Tud.tud_id
		INNER JOIN TUR_Turma Turm WITH(NOLOCK)
			ON Turm.tur_id = RelTud.tur_id
		INNER JOIN ACA_FormatoAvaliacao Fav WITH(NOLOCK)
			ON Fav.fav_id = Turm.fav_id
		LEFT JOIN CLS_AlunoAvaliacaoTurmaDisciplinaMedia Atm WITH(NOLOCK)
			ON	Atm.tud_id = Mtd.tud_id
			AND Atm.alu_id = Mtd.alu_id
			AND Atm.mtu_id = Mtd.mtu_id
			AND Atm.mtd_id = Mtd.mtd_id
			AND Atm.tpc_id = @tpc_id
			AND Atm.atm_situacao <> 3
		WHERE	
			Mtd.mtd_situacao <> 3
			AND Mtd.tud_id = @tud_id
			AND (Mtd.mtd_dataMatricula <= Cap.cap_dataFim)
			AND (
					Mtd.mtd_dataSaida IS NULL
					OR Mtd.mtd_dataSaida >= Cap.cap_dataInicio					
				)
	END
	
	-- Se for um componente da regência, traz os dados baseados nas aulas da regência
	IF (@tud_tipo = 12)
	BEGIN
		
		-- Aulas da regência
		;WITH tbAulas AS 
		(
			SELECT 
				tau.tud_id,
				tau.tau_id,
				tau.tau_data,
				tau.tau_numeroAulas,
				tau.usu_id
			FROM 
				CLS_TurmaAula tau WITH(NOLOCK)
			WHERE
				tau.tud_id = @tud_idRegencia
				AND tau.tpc_id = @tpc_id
				AND tau.tau_situacao <> 3
				-- inserido para corrigir duplicidade em ativ. avaliativa
				AND
				(
					@tdt_posicao IS NULL
					OR Tau.tdt_posicao = @tdt_posicao
					OR Tau.usu_id = @usu_id
				)
		)
		--Avaliações da disciplina
		INSERT INTO @tbAvaliacoes
		SELECT
			Tnt.tud_id
			, Tnt.tnt_id
			, CASE WHEN (tnt_nome IS NULL) or (tnt_nome = '') then tav_nome ELSE tnt_nome END --nome
			, Tnt.tdt_posicao
			, Tnt.usu_id
			, CONVERT(VARCHAR(10), ISNULL(tau_data, tnt_data), 103) --tnt_data
			, Tna.tna_avaliacao --avaliacao
			, Tna.tna_relatorio	--relatorio
			-- (ALUNO AUSENTE) 1 - aluno ausente que não possui falta justificada 
			, CAST( 
				CASE WHEN @ausenteTurmaNota = 0
							AND Mat.fav_tipoLancamentoFrequencia IN (1, 4, 5, 6) 
							AND (Tau.tau_id IS NOT NULL) THEN
					-- Aulas planejadas
					CASE WHEN(
								(SELECT TOP 1 TAA.taa_frequencia 
									FROM CLS_TurmaAula TAUL WITH(NOLOCK)
										INNER JOIN CLS_TurmaAulaAluno TAA WITH(NOLOCK)
											ON TAUL.tud_id = TAA.tud_id
										AND TAUL.tau_id = TAA.tau_id
										AND Mat.alu_id = TAA.alu_id
										AND Mat.mtu_id = TAA.mtu_id
									WHERE  TAUL.tud_id = @tud_idRegencia
									AND TAUL.tau_data = ISNULL(TAU.tau_data,Tnt.tnt_data)) = 
								(
								-- Verifica o tipo de apuração de frequenacia
								CASE Mat.fav_tipoApuracaoFrequencia
									WHEN 1 THEN -- 1: Tempos de aula
											Tau.tau_numeroAulas
									ELSE -- 2: Dia
										1
								END
								)
							)
								AND ( Ajf.afj_id IS NULL OR Tjf.tjf_abonaFalta <> 1)
						THEN 1 ELSE 0 
					END
					-- Período ( Nâo terá esta funcionalidade )
				ELSE CASE WHEN @ausenteTurmaNota = 1 THEN ISNULL(tna_naoCompareceu, 0) ELSE 0 END				       
			END AS BIT) --ausente
			-- (FALTA JUSTIFICADA) 0 - não possui FJ / 1 - possui FJ que abona / 2 - possui FJ que não abona 
			, CASE WHEN Ajf.afj_id IS NULL
					THEN '0'					    						    
					ELSE (CASE WHEN Tjf.tjf_abonaFalta = 1 THEN '1' ELSE '2' END)
				END --falta_justificada
			, Tnt.tnt_efetivado
			, Mat.alu_id
			, Mat.mtu_id
			, Mat.mtd_id
			, Tnt.tnt_exclusiva
			, ISNULL(Tna.tna_participante, 0) --tna_participante
			, Mat.AlunoDispensado
			, (CASE WHEN @usu_id is null or ISNULL(Tnt.usu_id, @usu_id) = @usu_id THEN 1 ELSE 0 END) --permissaoAlteracao
			, Mat.mtd_situacao
			, Mat.atm_media

			--Campos para ajuste com as tabelas de avaliacoes automaticas e da secretaria	
			, NULL --avs_id
			, NULL --naoConstaMedia
			, NULL --aas_id
			, CAST(
					CASE WHEN 
							(Tna.alu_id IS NOT NULL AND Tnt.tav_id <> @tavIdLicaoDeCasa)
							OR
							(Tna.alu_id IS NOT NULL AND Tnt.tav_id = @tavIdLicaoDeCasa AND ISNULL(Tna.tna_avaliacao, '') <> '')
						THEN 1
						ELSE 0
					END AS BIT) --PossuiNota
			, ISNULL(Tau.tau_id, -1) AS tau_id
			, NULL --aau_id
			, NULL --aaa_id
			, Mat.fav_id
			, ISNULL(Tav.tav_id, -1) --tav_id
			, ISNULL(Qat.qat_id, -1) --qat_id
			, CASE WHEN Tnt.tnt_data IS NULL THEN 1 ELSE 0 END --semData
			, ISNULL(Qat.qat_ordem, 0) --ordem
			, ISNULL(tntRel.tnt_id, -1) --tnt_idRelacionadaPai
		FROM @tbMatricula Mat 
		INNER JOIN CLS_TurmaNota AS Tnt WITH (NOLOCK)	
			ON Mat.tud_id = Tnt.tud_id
			AND @tpc_id = Tnt.tpc_id
			AND (3 <> Tnt.tnt_situacao and 6 <> Tnt.tnt_situacao)
		INNER JOIN @PermissaoDocenteConsulta pdc
			ON Tnt.tdt_posicao = pdc.tdt_posicaoPermissao
			AND (pdc.pdc_permissaoConsulta = 1 OR Tnt.usu_id = @usu_id OR @usuario_superior = 1 OR Tnt.tav_id = @tavIdLicaoDeCasa)
		LEFT JOIN CLS_TipoAtividadeAvaliativa AS Tav WITH (NOLOCK)
			ON Tnt.tav_id = Tav.tav_id
			AND Tav.tav_situacao <> 3
		LEFT JOIN CLS_QualificadorAtividade Qat WITH(NOLOCK)
			ON qat.qat_id = Tav.qat_id
			AND qat.qat_situacao <> 3
		LEFT OUTER JOIN CLS_TurmaNotaAluno Tna WITH(NOLOCK)
			ON Tnt.tud_id = Tna.tud_id
			AND Tnt.tnt_id = Tna.tnt_id
			AND Mat.alu_id = Tna.alu_id
			AND Mat.mtu_id = Tna.mtu_id
			AND Mat.mtd_id = Tna.mtd_id
			AND 3 <> Tna.tna_situacao
		LEFT JOIN tbAulas Tau WITH (NOLOCK)
			ON Tnt.tnt_data = Tau.tau_data
		LEFT JOIN ACA_AlunoJustificativaFalta Ajf WITH (NOLOCK)
			ON Mat.alu_id = Ajf.alu_id
			AND 3 <> Ajf.afj_situacao
			AND tau_data >= Ajf.afj_dataInicio
			AND (Ajf.afj_dataFim IS NULL OR (tau_data <= Ajf.afj_dataFim))
		LEFT JOIN ACA_TipoJustificativaFalta Tjf WITH (NOLOCK)
			ON Ajf.tjf_id = Tjf.tjf_id
		LEFT JOIN CLS_TurmaNotaRelacionada tntRel WITH(NOLOCK)
			ON tntRel.tud_idRelacionada = Tnt.tud_id
			AND tntRel.tnt_idRelacionada = Tnt.tnt_id
		WHERE
			Qat.qat_id IS NULL
			OR Qat.qat_id <> @qualificadorLicaoDeCasa
			OR (@possuiAtividadeAutomatica = 1 AND Qat.qat_id = @qualificadorLicaoDeCasa)
	END
	ELSE
	BEGIN

		INSERT INTO @tbAvaliacoes
		SELECT
			Tnt.tud_id
			, Tnt.tnt_id
			, CASE WHEN (tnt_nome IS NULL) or (tnt_nome = '') then tav_nome ELSE tnt_nome END --nome
			, Tnt.tdt_posicao
			, Tnt.usu_id
			, CONVERT(VARCHAR(10), ISNULL(tau_data, tnt_data), 103) --tnt_data
			, Tna.tna_avaliacao --avaliacao
			, Tna.tna_relatorio	--relatorio				
			-- (ALUNO AUSENTE) 1 - aluno ausente que não possui falta justificada 
			, CAST( 
				CASE WHEN @ausenteTurmaNota = 0
							AND Mat.fav_tipoLancamentoFrequencia IN (1, 4, 5, 6) 
							AND (Tau.tau_id IS NOT NULL) THEN
					-- Aulas planejadas
					CASE 
						WHEN  
							(
								(SELECT 
									taa_frequencia 
									FROM 
									CLS_TurmaAulaAluno WITH(NOLOCK)
									WHERE 
									tud_id = Tau.tud_id
									AND tau_id = Tau.tau_id
									AND alu_id = Mat.alu_id
									AND mtu_id = Mat.mtu_id
									AND mtd_id = Mat.mtd_id)
								= (
								-- Verifica o tipo de apuração de frequenacia
								CASE Mat.fav_tipoApuracaoFrequencia
									WHEN 1 THEN -- 1: Tempos de aula
											Tau.tau_numeroAulas
									ELSE -- 2: Dia
										1
								END
								)
							)
								AND ( Ajf.afj_id IS NULL OR Tjf.tjf_abonaFalta <> 1)
						THEN 1 ELSE 0 
					END
					-- Período ( Nâo terá esta funcionalidade )
				ELSE CASE WHEN @ausenteTurmaNota = 1 THEN ISNULL(tna_naoCompareceu, 0) ELSE 0 END				       
			END AS BIT) --ausente
			-- (FALTA JUSTIFICADA) 0 - não possui FJ / 1 - possui FJ que abona / 2 - possui FJ que não abona 
			, CASE WHEN Ajf.afj_id IS NULL
					THEN '0'					    						    
					ELSE (CASE WHEN Tjf.tjf_abonaFalta = 1 THEN '1' ELSE '2' END)
				END --falta_justificada
			, Tnt.tnt_efetivado
			, Mat.alu_id
			, Mat.mtu_id
			, Mat.mtd_id
			, Tnt.tnt_exclusiva
			, ISNULL(Tna.tna_participante, 0) --tna_participante
			, Mat.AlunoDispensado	
			, CASE WHEN @usu_id is null or ISNULL(Tnt.usu_id, @usu_id) = @usu_id THEN 1
			  		WHEN ISNULL(pde.tdt_posicaoPermissao, 0) > 0 THEN 1 
					ELSE 0 
				END	--permissaoAlteracao		
			, Mat.mtd_situacao
			, Mat.atm_media
				
			--Campos para ajuste com as tabelas de avaliacoes automaticas e da secretaria
			, NULL --avs_id		
			, NULL --naoConstaMedia
			, NULL --aas_id		
			, CAST(
					CASE WHEN 
							(Tna.alu_id IS NOT NULL AND Tnt.tav_id <> @tavIdLicaoDeCasa)
							OR
							(Tna.alu_id IS NOT NULL AND Tnt.tav_id = @tavIdLicaoDeCasa AND ISNULL(Tna.tna_avaliacao, '') <> '')
						THEN 1
						ELSE 0
					END AS BIT) --PossuiNota
			, ISNULL(Tau.tau_id, -1) AS tau_id
			, NULL --aau_id
			, NULL --aaa_id
			, Mat.fav_id
			, ISNULL(Tav.tav_id, -1) --tav_id
			, ISNULL(Qat.qat_id, -1) --qat_id
			, CASE WHEN Tnt.tnt_data IS NULL THEN 1 ELSE 0 END --semData
			, ISNULL(Qat.qat_ordem, 0) --ordem
			, ISNULL(tntRel.tnt_id, -1) --tnt_idRelacionadaPai
		FROM @tbMatricula Mat 
		INNER JOIN CLS_TurmaNota AS Tnt WITH (NOLOCK)	
			ON @tud_id = Tnt.tud_id
			AND Tnt.tpc_id = @tpc_id
			AND (tnt_situacao <> 3 and tnt_situacao <> 6)
		INNER JOIN @PermissaoDocenteConsulta pdc
			ON Tnt.tdt_posicao = pdc.tdt_posicaoPermissao
			AND (pdc.pdc_permissaoConsulta = 1 OR Tnt.usu_id = @usu_id OR @usuario_superior = 1 OR Tnt.tav_id = @tavIdLicaoDeCasa)
		LEFT JOIN CLS_TipoAtividadeAvaliativa AS Tav WITH (NOLOCK)
			ON Tav.tav_id = Tnt.tav_id
			AND Tav.tav_situacao <> 3
		LEFT JOIN CLS_QualificadorAtividade Qat WITH(NOLOCK)
			ON qat.qat_id = Tav.qat_id
			AND qat.qat_situacao <> 3
		LEFT OUTER JOIN CLS_TurmaNotaAluno Tna WITH(NOLOCK)
			ON Tna.tud_id = Tnt.tud_id
			AND Tna.tnt_id = Tnt.tnt_id
			AND Tna.alu_id = Mat.alu_id
			AND Tna.mtu_id = Mat.mtu_id
			AND Tna.mtd_id = Mat.mtd_id
			AND Tna.tna_situacao <> 3
		LEFT JOIN CLS_TurmaAula Tau WITH (NOLOCK)
			ON Tau.tud_id = Tnt.tud_id
			AND Tau.tau_id = Tnt.tau_id
			AND tau.tpc_id = Tnt.tpc_id
			AND tau.tau_situacao <> 3			
		LEFT JOIN ACA_AlunoJustificativaFalta Ajf WITH (NOLOCK)
			ON Ajf.alu_id = Mat.alu_id
			AND Ajf.afj_situacao <> 3
			AND tau_data >= Ajf.afj_dataInicio
			AND (Ajf.afj_dataFim IS NULL OR (tau_data <= Ajf.afj_dataFim))
		LEFT JOIN ACA_TipoJustificativaFalta Tjf WITH (NOLOCK)
			ON Tjf.tjf_id = Ajf.tjf_id				
		LEFT JOIN @PermissaoDocenteEdicao AS pde
			ON Tau.tdt_posicao = pde.tdt_posicaoPermissao
		
		-- Se for disciplina de docencia compartilhada,
		-- retorno apenas as aulas compartilhadas com a disciplina relacionada do parametro.
		LEFT JOIN CLS_TurmaAulaDisciplinaRelacionada TauRel WITH(NOLOCK)
			ON @tud_tipo = 17
			AND TauRel.tud_id = Tau.tud_id
			AND TauRel.tau_id = Tau.tau_id
			AND TauRel.tud_idRelacionada = @tud_idRelacionada
		LEFT JOIN CLS_TurmaNotaRelacionada tntRel WITH(NOLOCK)
			ON tntRel.tud_idRelacionada = Tnt.tud_id
			AND tntRel.tnt_idRelacionada = Tnt.tnt_id
		WHERE
			(@tud_tipo <> 17 OR TauRel.tud_id IS NOT NULL)
			AND 
			(
				Qat.qat_id IS NULL
				OR Qat.qat_id <> @qualificadorLicaoDeCasa
				OR (@possuiAtividadeAutomatica = 1 AND Qat.qat_id = @qualificadorLicaoDeCasa)
			)
	END

	-- Avaliações automáticas	
	-- Se possui atividade automatica
	IF (@possuiAtividadeAutomatica = 1 AND @tavIdLicaoDeCasa > 0)
	BEGIN
		
		-- Se nao existir atividade automatica cadastrada
		IF (NOT EXISTS(SELECT TOP 1 tnt_id FROM @tbAvaliacoes WHERE tav_id = @tavIdLicaoDeCasa))
		BEGIN
			-- retorno uma atividade com id = -1, com as notas calculadas
			INSERT INTO @tbAvaliacoes
			SELECT	
				Mat.tud_id
				, -1 --tnt_id
				, @tavNomeLicaoDeCasa --nome
				, 1 --tdt_posicao, Apenas docentes da posição 1 podem alterar.
				, NULL --usu_id
				, NULL --tnt_data 
				, NULL --avaliacao
				, NULL --relatorio
				, 0 --ausente
				, NULL --falta_justificada
				, NULL --tnt_efetivado
				, Mat.alu_id
				, Mat.mtu_id
				, Mat.mtd_id
				, 0 --tnt_exclusiva
				, NULL --tna_participante
				, Mat.AlunoDispensado
				, 0 --permissaoAlteracao
				, Mat.mtd_situacao
				, Mat.atm_media	
				, NULL --avs_id
				, NULL --naoConstaMedia	
				, NULL --aas_id			
				, CAST(0 AS BIT) --PossuiNota
				, NULL --tau_id
				, NULL --aau_id					
				, NULL --aaa_id
				, Mat.fav_id
				, @tavIdLicaoDeCasa --tav_id
				, @qualificadorLicaoDeCasa --qat_id
				, 1 --semData
				, @ordemLicaoDeCasa --ordem
				, -1 --tnt_idRelacionadaPai
			FROM @tbMatricula Mat
		END

		-- Verifica se existe aluno sem nota em avaliacao automatica
		IF (EXISTS (SELECT TOP 1 aau_id FROM @tbAvaliacoes WHERE PossuiNota = 0 AND tav_id = @tavIdLicaoDeCasa))
		BEGIN
			DECLARE @tbAlunos TipoTabela_AlunoMatriculaTurmaDisciplina;
			INSERT INTO @tbAlunos
			SELECT 
				alu_id
				, mtu_id
				, mtd_id
			FROM @tbAvaliacoes
			WHERE PossuiNota = 0
			AND tav_id = @tavIdLicaoDeCasa

			-- Calcula as notas
			DECLARE @Retorno TABLE
			(
				alu_id BIGINT
				, mtu_id INT
				, mtd_id INT
				, avaliacao VARCHAR(20)
			)
			INSERT INTO @Retorno
			EXEC NEW_CLS_TurmaNota_CalculaNotaAutomaticaAlunos @tud_id, @tpc_id, @esaIdDocente, @tbAlunos

			UPDATE @tbAvaliacoes
			SET avaliacao = Ret.avaliacao
			FROM @Retorno Ret
			INNER JOIN @tbAvaliacoes Ava
				ON Ava.alu_id = Ret.alu_id
				AND Ava.mtu_id = Ret.mtu_id
				AND Ava.mtd_id = Ret.mtd_id
				AND Ava.tav_id = @tavIdLicaoDeCasa
				AND Ava.PossuiNota = 0
		END 
	END

	;WITH avaliacoes AS 
	(
		SELECT 
			ava.tud_id 
			, ava.tnt_id 
			, ava.nome 
			, ava.tdt_posicao 
			, ava.usu_id 
			, ava.tnt_data 
			, ava.avaliacao 
			, ava.relatorio 
			, ava.ausente 
			, ava.falta_justificada 
			, ava.tnt_efetivado 
			, ava.alu_id 
			, ava.mtu_id 
			, ava.mtd_id 
			, ava.tnt_exclusiva 
			, ava.tna_participante 
			, ava.AlunoDispensado 
			, ava.permissaoAlteracao 
			, ava.mtd_situacao 
			, ava.atm_media 
			, ava.avs_id 
			, ava.naoConstaMedia 
			, ava.aas_id 
			, ava.PossuiNota 
			, ava.tau_id 
			, ava.aau_id 
			, ava.aaa_id 
			--, ava.fav_id 
			, ava.tav_id 
			, ava.qat_id 
			, ava.semData 
			, ava.ordem 
			, ava.tnt_idRelacionadaPai 
			--Variaveis auxiliares da ordenacao
			, CASE WHEN ISNULL(avaRelPai.tnt_id, -1) > 0 THEN avaRelPai.tnt_id ELSE ava.tnt_id END AS tnt_idAux 
			, CASE WHEN ISNULL(avaRelPai.tnt_id, -1) > 0 THEN avaRelPai.semData ELSE ava.semData END AS semData_aux
			, CASE WHEN ISNULL(avaRelPai.tnt_id, -1) > 0 THEN avaRelPai.tnt_data ELSE ava.tnt_data END AS data_aux
			, CASE WHEN ISNULL(avaRelPai.tnt_id, -1) > 0 THEN avaRelPai.nome ELSE ava.nome END AS nome_aux 
			, CASE WHEN ISNULL(avaRelPai.tnt_id, -1) > 0 THEN avaRelPai.ordem ELSE ava.ordem END AS ordem_aux
		FROM @tbAvaliacoes ava
		LEFT JOIN @tbAvaliacoes avaRelPai
			ON ava.tnt_idRelacionadaPai > 0
			AND avaRelPai.tud_id = ava.tud_id
			AND avaRelPai.tnt_id = ava.tnt_idRelacionadaPai
	)
	SELECT 
		tud_id, tnt_id, nome, tdt_posicao, usu_id, tnt_data, avaliacao, relatorio
		, ausente, falta_justificada, tnt_efetivado, alu_id, mtu_id, mtd_id, tnt_exclusiva
		, tna_participante, AlunoDispensado, permissaoAlteracao, mtd_situacao, atm_media 
		, avs_id, naoConstaMedia, aas_id, PossuiNota, tau_id, aau_id, aaa_id, tav_id, qat_id, tnt_idRelacionadaPai
		, ISNULL(@usu_nome,'') AS usuarioAltMedia, ISNULL(@lam_data,'') AS dataAltMedia
	FROM 
		avaliacoes
	GROUP BY 
		tud_id, tnt_id, nome, tdt_posicao, usu_id, tnt_data, avaliacao, relatorio
		, ausente, falta_justificada, tnt_efetivado, alu_id, mtu_id, mtd_id, tnt_exclusiva
		, tna_participante, AlunoDispensado, permissaoAlteracao, mtd_situacao, atm_media
		, avs_id, naoConstaMedia, aas_id, PossuiNota, tau_id, aau_id, aaa_id, tav_id, qat_id, tnt_idRelacionadaPai
		, semData, ordem, ordem_aux, semData_aux, data_aux, nome_aux, tnt_idAux
	ORDER BY 
		ordem_aux, semData_aux, data_aux, nome_aux, tnt_idAux, ordem
END
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
PRINT N'Altering [dbo].[NEW_CLS_TurmaNota_SelectBy_Periodo_NotaAlunoFiltroDeficiencia]'
GO
-- ========================================================================
-- Author:		Daniel Jun Suguimoto
-- Create date: 11/03/2014
-- Description:	Retorna as Atividades com as notas do aluno, filtrando
--				os alunos com ou sem deficiência, dependendo do docente.
--Alterado: Webber V. dos Santos  Data: 30/04/2014
--Description: Alterado forma de filtrar o nome da atividade avaliativa

-- Alterado: Haila Pelloso - 10/07/2015
-- Description: Verificando dado da última alteraçao da tabela de log.
-- ========================================================================
ALTER PROCEDURE [dbo].[NEW_CLS_TurmaNota_SelectBy_Periodo_NotaAlunoFiltroDeficiencia]	
	@tud_id BIGINT
	, @tpc_id INT
	, @tdc_id TINYINT
    , @usu_id UNIQUEIDENTIFIER  
    , @tdt_posicao TINYINT

AS 
BEGIN
	
	DECLARE @tbAlunos TABLE (alu_id INT);
	
	DECLARE @PermissaoDocenteEdicao TABLE (tdt_posicaoPermissao int);
	
	INSERT INTO @PermissaoDocenteEdicao (tdt_posicaoPermissao)
    SELECT 
		tdcPermissao.tdc_posicao AS tdt_posicaoPermissao
	FROM
		ACA_TipoDocente tdc WITH(NOLOCK)
		INNER JOIN CFG_PermissaoDocente pdc WITH(NOLOCK)
			ON tdc.tdc_id = pdc.tdc_id
			AND pdc.pdc_modulo = 1
			AND pdc.pdc_permissaoEdicao = 1  -- retonar as permissões de edição
			AND pdc.pdc_situacao <> 3 
		INNER JOIN ACA_TipoDocente tdcPermissao WITH(NOLOCK)
			ON tdcPermissao.tdc_id = pdc.tdc_idPermissao
			AND tdcPermissao.tdc_situacao <> 3
	WHERE
		tdc.tdc_posicao = ISNULL(@tdt_posicao, tdc.tdc_posicao)
		AND tdc.tdc_situacao <> 3
		AND tdc.tdc_id IN (1,6) --Titular / Segundo titular
	
	IF (@tdc_id = 5)
	BEGIN
		;WITH MatriculaTurmaDisciplina AS
		(
			SELECT
				mtd.alu_id
			FROM
				MTR_MatriculaTurmaDisciplina mtd WITH(NOLOCK)
			WHERE
				mtd.tud_id = @tud_id
				AND mtd.mtd_situacao <> 3
		)
		
		, TipoDeficiencia AS 
		(
			SELECT 
				RelTde.tde_id
			FROM
				TUR_TurmaDisciplinaRelDisciplina DisRel WITH(NOLOCK)
				INNER JOIN ACA_Disciplina dis WITH(NOLOCK)
					ON DisRel.dis_id = dis.dis_id
					AND dis.dis_situacao <> 3
				INNER JOIN ACA_TipoDisciplina tds WITH(NOLOCK)
					ON dis.tds_id = tds.tds_id
					AND tds.tds_situacao <> 3
				INNER JOIN ACA_TipoDisciplinaDeficiencia RelTde WITH(NOLOCK)
					ON tds.tds_id = RelTde.tds_id
			WHERE
				DisRel.tud_id = @tud_id
		)
		
		INSERT INTO @tbAlunos 
		(
			alu_id
		)
		SELECT
			mtd.alu_id
		FROM
			MatriculaTurmaDisciplina mtd 
			INNER JOIN ACA_Aluno alu WITH(NOLOCK)
				ON alu.alu_id = mtd.alu_id
			INNER JOIN Synonym_PES_PessoaDeficiencia pde WITH(NOLOCK)
				ON pde.pes_id = alu.pes_id
			INNER JOIN TipoDeficiencia tde
				ON pde.tde_id = tde.tde_id
	END
	ELSE
	BEGIN
		;WITH MatriculaTurmaDisciplina AS
		(
			SELECT
				mtd.alu_id
			FROM
				MTR_MatriculaTurmaDisciplina mtd WITH(NOLOCK)
			WHERE
				mtd.tud_id = @tud_id
				AND mtd.mtd_situacao <> 3
		)
		
		, TipoDeficiencia AS 
		(
			SELECT 
				RelTde.tde_id
			FROM
				TUR_TurmaDisciplinaRelDisciplina DisRel WITH(NOLOCK)
				INNER JOIN ACA_Disciplina dis WITH(NOLOCK)
					ON DisRel.dis_id = dis.dis_id
					AND dis.dis_situacao <> 3
				INNER JOIN ACA_TipoDisciplina tds WITH(NOLOCK)
					ON dis.tds_id = tds.tds_id
					AND tds.tds_situacao <> 3
				INNER JOIN ACA_TipoDisciplinaDeficiencia RelTde WITH(NOLOCK)
					ON tds.tds_id = RelTde.tds_id
			WHERE
				DisRel.tud_id = @tud_id
		)
		
		INSERT INTO @tbAlunos 
		(
			alu_id
		)
		SELECT
			mtd.alu_id
		FROM
			MatriculaTurmaDisciplina mtd 
			INNER JOIN ACA_Aluno alu WITH(NOLOCK)
				ON alu.alu_id = mtd.alu_id
			LEFT JOIN Synonym_PES_PessoaDeficiencia pde WITH(NOLOCK)
				ON pde.pes_id = alu.pes_id
		WHERE
			(NOT EXISTS (SELECT tde_id FROM TipoDeficiencia tde WHERE tde.tde_id = pde.tde_id ))	
	END
	
	DECLARE @tud_tipo TINYINT;
	SELECT	
		@tud_tipo = ISNULL(tud.tud_tipo,0)
	FROM
		TUR_TurmaDisciplina tud WITH(NOLOCK)
	WHERE
		tud.tud_id = @tud_id
		AND tud.tud_situacao <> 3

	-- Se a disciplina não for um componente da regência, traz os dados normalmente
	IF (@tud_tipo <> 12)
	BEGIN
		SELECT
			Tnt.tud_id
			, Tnt.tnt_id
			--, CASE WHEN (tnt_nome IS NULL) or (tnt_nome = '') then tav_nome ELSE tnt_nome END AS tnt_nome
			--, CASE WHEN Tnt.tav_id IS NULL THEN tnt_nome ELSE tav_nome END AS tnt_nome
			, CASE WHEN (tnt_nome IS NULL) OR (tnt_nome = '') THEN
				CASE WHEN (Tnt.tav_id IS NULL) 
					THEN 'Outro tipo de atividade avaliativa' 
					ELSE tav_nome
				END
			  ELSE tnt_nome
			END AS tnt_nome			
			, Tnt.tdt_posicao
			, ISNULL(Ltn.usu_id, Tnt.usu_id) AS usu_id
			, Tnt.pro_id
			, Tnt.tnt_chaveDiario
			, CONVERT(VARCHAR(10), ISNULL(tau_data, tnt_data), 103) AS tnt_data
			, Tna.tna_avaliacao
			, Tna.tna_relatorio		
			
			-- (ALUNO AUSENTE) 1 - aluno ausente que não possui falta justificada 
			, CAST( 
				CASE WHEN Fav.fav_tipoLancamentoFrequencia IN ( 1, 4, 5, 6) AND (Tau.tau_id IS NOT NULL) THEN
				-- Aulas planejadas
					CASE 
						WHEN  
							(
								(SELECT 
									taa_frequencia 
								 FROM 
									CLS_TurmaAulaAluno WITH(NOLOCK)
								 WHERE 
									tud_id = Tau.tud_id
									AND tau_id = Tau.tau_id
									AND alu_id = Mtd.alu_id
									AND mtu_id = Mtd.mtu_id
									AND mtd_id = Mtd.mtd_id)
							 = (
								-- Verifica o tipo de apuração de frequenacia
								CASE Fav.fav_tipoApuracaoFrequencia
									WHEN 1 THEN -- 1: Tempos de aula
										  Tau.tau_numeroAulas
									ELSE -- 2: Dia
										1
								END
								)
							)
								AND ( Ajf.afj_id IS NULL OR Tjf.tjf_abonaFalta <> 1)
						THEN 1 ELSE 0 
					END
					-- Período ( Nâo terá esta funcionalidade )
				ELSE 0				       
			END AS BIT) AS aluno_ausente 
			-- (FALTA JUSTIFICADA) 0 - não possui FJ / 1 - possui FJ que abona / 2 - possui FJ que não abona 
			, CASE WHEN Ajf.afj_id IS NULL
					THEN '0'					    						    
					ELSE (CASE WHEN Tjf.tjf_abonaFalta = 1 THEN '1' ELSE '2' END)
				END AS falta_justificada
			, Tnt.tnt_efetivado
			, Mtd.alu_id
			, Mtd.mtu_id
			, Mtd.mtd_id
			, Tnt.tnt_exclusiva
			, ISNULL(Tna.tna_participante, 0) AS tna_participante
			, CAST(0 AS BIT) AS AlunoDispensado
			
			--, (CASE WHEN @usu_id is null or ISNULL(Tnt.usu_id, @usu_id) = @usu_id THEN 1 ELSE 0 END) AS permissaoAlteracao			
			, CASE WHEN @usu_id IS NULL OR COALESCE(Ltn.usu_id, Tnt.usu_id, @usu_id) = @usu_id THEN 1
		  		   WHEN ISNULL(pde.tdt_posicaoPermissao, 0) > 0 THEN 1 
				   ELSE 0 
			  END AS permissaoAlteracao
			
			, Mtd.mtd_situacao
			
		    , ISNULL(pesAlteracao.pes_nome,usuAlteracao.usu_Login) as nomeUsuAlteracao -- inserido para poder exibir o usuário que alterou os dados 
		    , ISNULL(Ltn.ltn_data, tnt_dataAlteracao) AS tnt_dataAlteracao -- inserido para poder exibir a data que o usuário realizou a alteração
			, CAST(
						CASE WHEN ISNULL(Tna.alu_id, 0) = Mtd.alu_id
							THEN 1
							ELSE 0
						END AS BIT) AS PossuiNota
			, Tnt.tau_id
		FROM CLS_TurmaNota AS Tnt WITH (NOLOCK)	
		INNER JOIN TUR_TurmaDisciplina AS Tud WITH (NOLOCK)	
			ON Tnt.tud_id = Tud.tud_id
		INNER JOIN TUR_TurmaRelTurmaDisciplina AS RelTud WITH (NOLOCK)
			ON RelTud.tud_id = Tud.tud_id
		INNER JOIN TUR_Turma AS Tur WITH(NOLOCK)
			ON Tur.tur_id = RelTud.tur_id
		INNER JOIN ACA_FormatoAvaliacao Fav WITH(NOLOCK)
			ON Fav.fav_id = Tur.fav_id
		INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
			ON Cap.cal_id = Tur.cal_id 
			AND Cap.tpc_id = Tnt.tpc_id
			AND Cap.cap_situacao <> 3
		-- Trazer todos os alunos matriculados na disciplina.
		INNER JOIN MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
			ON Mtd.tud_id = Tnt.tud_id
			AND Mtd.mtd_situacao IN (1,5)
		INNER JOIN @tbAlunos
			ON mtd.alu_id = [@tbAlunos].alu_id
		LEFT JOIN CLS_TipoAtividadeAvaliativa AS Tav WITH (NOLOCK)
			ON Tav.tav_id = Tnt.tav_id
		LEFT OUTER JOIN CLS_TurmaNotaAluno Tna WITH(NOLOCK)
			ON Tna.tud_id = Tnt.tud_id
			AND Tna.tnt_id = Tnt.tnt_id
			AND Tna.alu_id = Mtd.alu_id
			AND Tna.mtu_id = Mtd.mtu_id
			AND Tna.mtd_id = Mtd.mtd_id
			AND Tna.tna_situacao <> 3
		LEFT JOIN CLS_TurmaAula Tau WITH (NOLOCK)
			ON Tau.tud_id = Tnt.tud_id
			AND Tau.tau_id = Tnt.tau_id
			AND tau.tpc_id = Tnt.tpc_id
			AND tau.tau_situacao <> 3
		LEFT JOIN ACA_AlunoJustificativaFalta Ajf WITH (NOLOCK)
			ON Ajf.alu_id = Mtd.alu_id
			AND Ajf.afj_situacao <> 3
			AND tau_data >= Ajf.afj_dataInicio
			AND (Ajf.afj_dataFim IS NULL OR (tau_data <= Ajf.afj_dataFim))
		LEFT JOIN ACA_TipoJustificativaFalta Tjf WITH (NOLOCK)
			 ON Tjf.tjf_id = Ajf.tjf_id
		LEFT JOIN @PermissaoDocenteEdicao AS pde
			ON Tau.tdt_posicao = pde.tdt_posicaoPermissao
		--- inserido para buscar o nome do ultimo usuario que alterou os dados		
		OUTER APPLY FN_RetornaUltimaAlteracaoTurmaNota(Tnt.tud_id, Tnt.tnt_id, 2) Ltn --Lançamento de notas
		LEFT JOIN Synonym_SYS_Usuario AS usuAlteracao WITH(NOLOCK)
			ON usuAlteracao.usu_id = ISNULL(Ltn.usu_id, tnt.usu_idDocenteAlteracao)
			AND usuAlteracao.usu_situacao <> 3
		LEFT JOIN Synonym_PES_Pessoa AS pesAlteracao WITH(NOLOCK)
			ON usuAlteracao.pes_id = pesAlteracao.pes_id
			AND pesAlteracao.pes_situacao <> 3
		WHERE
			Tnt.tud_id = @tud_id
			AND Tnt.tpc_id = @tpc_id
			AND (tnt_situacao <> 3 and tnt_situacao <> 6)
			AND tud_situacao <> 3
		
		GROUP BY Tnt.tud_id, Tnt.tnt_id, tnt_nome, tav_nome, Tnt.tav_id, Tnt.tdt_posicao, Tnt.pro_id, Tnt.tnt_chaveDiario
			, tau_data, tnt_data, Tna.tna_avaliacao, Tna.tna_relatorio, Tau.tud_id, Tau.tau_id, Mtd.alu_id
			, Mtd.mtu_id, Mtd.mtd_id, Fav.fav_tipoLancamentoFrequencia, Fav.fav_tipoApuracaoFrequencia
			, Tau.tau_numeroAulas, Ajf.afj_id, Tjf.tjf_abonaFalta, Tnt.tnt_efetivado, Tnt.tnt_exclusiva
			, Tna.tna_participante, Cap.cap_descricao, Ltn.usu_id, Tnt.usu_id, Tau.usu_id, Mtd.mtd_situacao
			, ISNULL(pesAlteracao.pes_nome,usuAlteracao.usu_Login), Ltn.ltn_data, tnt_dataAlteracao, pde.tdt_posicaoPermissao, Tna.alu_id, Tnt.tau_id
		
		ORDER BY
			Cap.cap_descricao
			, [tnt_data]
			, tnt_nome
	END
	ELSE
	
	-- Se for um componente da regência, traz os dados baseados nas aulas da regência
	BEGIN
		DECLARE @tud_idRegencia BIGINT = NULL;

		-- Caso seja Componente de regência pega o tud_id da regência
		SELECT 
			@tud_idRegencia = TUR_TUD_REG.tud_id
		FROM 
			dbo.TUR_TurmaDisciplina TUD WITH(NOLOCK)
			INNER JOIN dbo.TUR_TurmaDisciplinaRelDisciplina TUD_DIS WITH(NOLOCK)
				ON	TUD_DIS.tud_id = TUD.tud_id
			INNER JOIN dbo.TUR_TurmaRelTurmaDisciplina TUR_TUD WITH(NOLOCK)
				ON	TUR_TUD.tud_id = TUD.tud_id
			INNER JOIN dbo.ACA_CurriculoDisciplina CRD WITH(NOLOCK)
				ON	CRD.dis_id = TUD_DIS.dis_id
			INNER JOIN dbo.ACA_CurriculoDisciplina CRDREG WITH(NOLOCK)
				ON	CRDREG.cur_id = CRD.cur_id
				AND CRDREG.crr_id = CRD.crr_id
				AND CRDREG.crp_id = CRD.crp_id
			INNER JOIN dbo.TUR_TurmaDisciplinaRelDisciplina TUD_DIS_REG WITH(NOLOCK)
				ON	TUD_DIS_REG.dis_id = CRDREG.dis_id
			INNER JOIN dbo.TUR_TurmaRelTurmaDisciplina TUR_TUD_REG WITH(NOLOCK)
				ON	TUR_TUD_REG.tud_id = TUD_DIS_REG.tud_id
				AND TUR_TUD_REG.tur_id = TUR_TUD.tur_id
		WHERE 
			TUD.tud_id = @tud_id
			AND TUD.tud_tipo = 12
			AND CRDREG.crd_tipo = 11
			-- Exclusão Lógica
			AND TUD.tud_situacao <> 3
			AND CRD.crd_situacao <> 3
			AND CRDREG.crd_situacao <> 3

		-- Aulas da regência
		; WITH tbAulas AS 
		(
			SELECT 
				tau.tud_id,
				tau.tau_id,
				tau.tau_data,
				tau.tau_numeroAulas,
				tau.usu_id,
				tau.tdt_posicao
			FROM 
				CLS_TurmaAula tau WITH(NOLOCK)
			WHERE
				tau.tud_id = @tud_idRegencia
				AND tau.tpc_id = @tpc_id
				AND tau.tau_situacao <> 3
				AND Tau.tdt_posicao = ISNULL(@tdt_posicao, Tau.tdt_posicao) -- inserido para corrigir duplicidade em ativ. avaliativa
		), tbNotas AS
		(		
			SELECT
				Tnt.tud_id
				, Tnt.tnt_id
				--, CASE WHEN (tnt_nome IS NULL) or (tnt_nome = '') then tav_nome ELSE tnt_nome END AS tnt_nome
				--, CASE WHEN Tnt.tav_id IS NULL THEN tnt_nome ELSE tav_nome END AS tnt_nome
				, CASE WHEN (tnt_nome IS NULL) OR (tnt_nome = '') THEN
					CASE WHEN (Tnt.tav_id IS NULL) 
						THEN 'Outro tipo de atividade avaliativa' 
						ELSE tav_nome
					END
				  ELSE tnt_nome
				END AS tnt_nome				
				, Tnt.tdt_posicao
				, ISNULL(Ltn.usu_id, Tnt.usu_id) AS usu_id
				, Tnt.pro_id
				, Tnt.tnt_chaveDiario
				, CONVERT(VARCHAR(10), ISNULL(tau_data, tnt_data), 103) AS tnt_data
				, Tna.tna_avaliacao
				, Tna.tna_relatorio		
				
				-- (ALUNO AUSENTE) 1 - aluno ausente que não possui falta justificada 
				, CAST( 
					CASE WHEN Fav.fav_tipoLancamentoFrequencia IN (1, 4, 5, 6) AND (Tau.tau_id IS NOT NULL) THEN
					-- Aulas planejadas
						CASE 
							WHEN  
								(
									(SELECT TOP 1
										taa_frequencia 
									 FROM 
										CLS_TurmaAulaAluno WITH (NOLOCK)
									 WHERE 
										tud_id = Tau.tud_id
										AND tau_id = Tnt.tau_id
										AND alu_id = Mtd.alu_id
										AND mtu_id = Mtd.mtu_id)
								 = (
									-- Verifica o tipo de apuração de frequenacia
									CASE Fav.fav_tipoApuracaoFrequencia
										WHEN 1 THEN -- 1: Tempos de aula
											  Tau.tau_numeroAulas
										ELSE -- 2: Dia
											1
									END
									)
								)
									AND ( Ajf.afj_id IS NULL OR Tjf.tjf_abonaFalta <> 1)
							THEN 1 ELSE 0 
						END
						-- Período ( Nâo terá esta funcionalidade )
					ELSE 0				       
				END AS BIT) AS aluno_ausente 
				-- (FALTA JUSTIFICADA) 0 - não possui FJ / 1 - possui FJ que abona / 2 - possui FJ que não abona 
				, CASE WHEN Ajf.afj_id IS NULL
						THEN '0'					    						    
						ELSE (CASE WHEN Tjf.tjf_abonaFalta = 1 THEN '1' ELSE '2' END)
				   END AS falta_justificada
				, Tnt.tnt_efetivado
				, Mtd.alu_id
				, Mtd.mtu_id
				, Mtd.mtd_id
				, Tnt.tnt_exclusiva
				, ISNULL(Tna.tna_participante, 0) AS tna_participante
				, CAST(0 AS BIT) AS AlunoDispensado
				
				, (CASE WHEN @usu_id IS NULL OR COALESCE(Ltn.usu_id, Tnt.usu_id, @usu_id) = @usu_id THEN 1 ELSE 0 END) AS permissaoAlteracao
			    
			    , Mtd.mtd_situacao
			    
				, ISNULL(pesAlteracao.pes_nome,usuAlteracao.usu_Login) as nomeUsuAlteracao -- inserido para poder exibir o usuário que alterou os dados 
				, ISNULL(Ltn.ltn_data, tnt_dataAlteracao) AS tnt_dataAlteracao -- inserido para poder exibir a data que o usuário realizou a alteração
			    , CAST(
						CASE WHEN ISNULL(Tna.alu_id, 0) = Mtd.alu_id
							THEN 1
							ELSE 0
						END AS BIT) AS PossuiNota
				, Tnr.tau_idAula AS tau_id
			FROM CLS_TurmaNota AS Tnt WITH (NOLOCK)	
			INNER JOIN TUR_TurmaDisciplina AS Tud WITH (NOLOCK)	
				ON Tnt.tud_id = Tud.tud_id
			INNER JOIN TUR_TurmaRelTurmaDisciplina AS RelTud WITH (NOLOCK)
				ON RelTud.tud_id = Tud.tud_id
			INNER JOIN TUR_Turma AS Tur WITH(NOLOCK)
				ON Tur.tur_id = RelTud.tur_id
			INNER JOIN ACA_FormatoAvaliacao Fav WITH(NOLOCK)
				ON Fav.fav_id = Tur.fav_id
			INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
				ON Cap.cal_id = Tur.cal_id 
				AND Cap.tpc_id = Tnt.tpc_id
				AND Cap.cap_situacao <> 3
			-- Trazer todos os alunos matriculados na disciplina.
			INNER JOIN MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
				ON Mtd.tud_id = Tnt.tud_id
				AND Mtd.mtd_situacao IN (1,5)
			INNER JOIN @tbAlunos
				ON mtd.alu_id = [@tbAlunos].alu_id
			LEFT JOIN CLS_TipoAtividadeAvaliativa AS Tav WITH (NOLOCK)
				ON Tav.tav_id = Tnt.tav_id
			LEFT JOIN CLS_TurmaNotaRegencia AS Tnr WITH(NOLOCK)
				ON Tnt.tud_id = Tnr.tud_id 
				AND Tnt.tnt_id = Tnr.tnt_id
			LEFT OUTER JOIN CLS_TurmaNotaAluno Tna WITH(NOLOCK)
				ON Tna.tud_id = Tnt.tud_id
				AND Tna.tnt_id = Tnt.tnt_id
				AND Tna.alu_id = Mtd.alu_id
				AND Tna.mtu_id = Mtd.mtu_id
				AND Tna.mtd_id = Mtd.mtd_id
				AND Tna.tna_situacao <> 3
			LEFT JOIN tbAulas Tau WITH (NOLOCK)
				ON Tau.tau_data = Tnt.tnt_data
			LEFT JOIN ACA_AlunoJustificativaFalta Ajf WITH (NOLOCK)
				ON Ajf.alu_id = Mtd.alu_id
				AND Ajf.afj_situacao <> 3
				AND tau_data >= Ajf.afj_dataInicio
				AND (Ajf.afj_dataFim IS NULL OR (tau_data <= Ajf.afj_dataFim))
			LEFT JOIN ACA_TipoJustificativaFalta Tjf WITH (NOLOCK)
				 ON Tjf.tjf_id = Ajf.tjf_id
			LEFT JOIN @PermissaoDocenteEdicao AS pde
				ON Tau.tdt_posicao = pde.tdt_posicaoPermissao
			--- inserido para buscar o nome do ultimo usuario que alterou os dados		
			OUTER APPLY FN_RetornaUltimaAlteracaoTurmaNota(Tnt.tud_id, Tnt.tnt_id, 2) Ltn --Lançamento de notas
			LEFT JOIN Synonym_SYS_Usuario AS usuAlteracao WITH(NOLOCK)
				ON usuAlteracao.usu_id = ISNULL(Ltn.usu_id, tnt.usu_idDocenteAlteracao)
				AND usuAlteracao.usu_situacao <> 3
			LEFT JOIN Synonym_PES_Pessoa AS pesAlteracao WITH(NOLOCK)
				ON usuAlteracao.pes_id = pesAlteracao.pes_id
				AND pesAlteracao.pes_situacao <> 3
			WHERE
				Tnt.tud_id = @tud_id
				AND Tnt.tpc_id = @tpc_id
				AND (tnt_situacao <> 3 and tnt_situacao <> 6)
				AND tud_situacao <> 3
		)
		
		SELECT tud_id, tnt_id, tnt_nome, tdt_posicao, usu_id, pro_id, tnt_chaveDiario, tnt_data, tna_avaliacao, tna_relatorio
			, aluno_ausente, falta_justificada, tnt_efetivado, alu_id, mtu_id, mtd_id, tnt_exclusiva
			, tna_participante, AlunoDispensado, permissaoAlteracao, mtd_situacao, nomeUsuAlteracao, tnt_dataAlteracao, PossuiNota, tau_id
			
		FROM tbNotas
		GROUP BY tud_id, tnt_id, tnt_nome, tdt_posicao, usu_id, pro_id, tnt_chaveDiario, tnt_data, tna_avaliacao, tna_relatorio
			, aluno_ausente, falta_justificada, tnt_efetivado, alu_id, mtu_id, mtd_id, tnt_exclusiva
			, tna_participante, AlunoDispensado, permissaoAlteracao, mtd_situacao, nomeUsuAlteracao, tnt_dataAlteracao, PossuiNota, tau_id
		ORDER BY tnt_data, tnt_nome
		
	END
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
PRINT N'Creating [dbo].[STP_ACA_AlunoJustificativaFalta_SELECTBY_tjf_id]'
GO

CREATE PROCEDURE [dbo].[STP_ACA_AlunoJustificativaFalta_SELECTBY_tjf_id]
	@tjf_id INT
AS
BEGIN
	SELECT
		alu_id
		,afj_id
		,tjf_id
		,afj_dataInicio
		,afj_dataFim
		,afj_situacao
		,afj_dataCriacao
		,afj_dataAlteracao
		,afj_observacao

	FROM
		ACA_AlunoJustificativaFalta WITH(NOLOCK)
	WHERE 
		tjf_id = @tjf_id 
END

GO
PRINT N'Altering [dbo].[NEW_CLS_TurmaAulaAluno_SelectBy_TurmaDisciplina_Aluno]'
GO
-- ========================================================================
---- Alterado: Marcia Haga - 02/06/2015
---- Description: Alterada regra para relacionar a disciplina compartilhada.

---- Alterado: Haila Pelloso - 07/07/2015
---- Description: Alterada regra para selecionar apenas aulas do próprio docente caso não tenha permissão de visualizar a posição em que está.
-- ========================================================================
ALTER PROCEDURE [dbo].[NEW_CLS_TurmaAulaAluno_SelectBy_TurmaDisciplina_Aluno]
	@tud_id BIGINT
	, @tpc_id INT
	, @data_inicio DATETIME
	, @data_final DATETIME
	, @usu_id UNIQUEIDENTIFIER
	, @tdt_posicao TINYINT
	, @tud_idRelacionada BIGINT	
	, @usuario_superior BIT
	, @dtTurmas TipoTabela_Turma READONLY

AS
BEGIN
	DECLARE @tud_tipo TINYINT;
	SELECT
		@tud_tipo = ISNULL(tud.tud_tipo,0)
	FROM
		TUR_TurmaDisciplina tud WITH(NOLOCK)
	WHERE
		tud.tud_id = @tud_id
		AND tud.tud_situacao <> 3
		
	CREATE TABLE #PermissaoDocenteEdicao (tdt_posicaoPermissao INT, PRIMARY KEY(tdt_posicaoPermissao));
	CREATE TABLE #PermissaoDocenteConsulta (tdt_posicaoPermissao INT, pdc_permissaoConsulta BIT, PRIMARY KEY(tdt_posicaoPermissao))

	;WITH tbDadosPermissaoDocenteConsulta AS	
	(
		SELECT 
			tdcPermissao.tdc_posicao AS tdt_posicaoPermissao
			, pdc.pdc_permissaoConsulta
		FROM
			ACA_TipoDocente tdc WITH(NOLOCK)
			INNER JOIN CFG_PermissaoDocente pdc WITH(NOLOCK)
				ON tdc.tdc_id = pdc.tdc_id
				AND pdc.pdc_modulo = 1
				AND (@usuario_superior = 1 OR pdc.pdc_permissaoConsulta = 1) -- retonar as permissões de consulta
				AND pdc.pdc_situacao <> 3 
			INNER JOIN ACA_TipoDocente tdcPermissao WITH(NOLOCK)
				ON tdcPermissao.tdc_id = pdc.tdc_idPermissao
				AND tdcPermissao.tdc_situacao <> 3
		WHERE
			(tdc.tdc_posicao = ISNULL(@tdt_posicao, tdc.tdc_posicao) OR @usuario_superior = 1)
			AND tdc.tdc_situacao <> 3
			
		UNION 
		
		SELECT ISNULL(@tdt_posicao, 1), 0 AS pdc_permissaoConsulta
	)
	INSERT INTO #PermissaoDocenteConsulta (tdt_posicaoPermissao, pdc_permissaoConsulta)
	SELECT 
		pdc.tdt_posicaoPermissao
		, MAX(pdc.pdc_permissaoConsulta) AS pdc_permissaoConsulta
	FROM
		tbDadosPermissaoDocenteConsulta pdc WITH(NOLOCK)
	GROUP BY
		pdc.tdt_posicaoPermissao
	
	INSERT INTO #PermissaoDocenteEdicao (tdt_posicaoPermissao)
    SELECT 
		tdcPermissao.tdc_posicao AS tdt_posicaoPermissao
	FROM
		ACA_TipoDocente tdc WITH(NOLOCK)
		INNER JOIN CFG_PermissaoDocente pdc WITH(NOLOCK)
			ON tdc.tdc_id = pdc.tdc_id
			AND pdc.pdc_modulo = 1
			AND pdc.pdc_permissaoEdicao = 1  -- retonar as permissões de edição
			AND pdc.pdc_situacao <> 3 
		INNER JOIN ACA_TipoDocente tdcPermissao WITH(NOLOCK)
			ON tdcPermissao.tdc_id = pdc.tdc_idPermissao
			AND tdcPermissao.tdc_situacao <> 3
	WHERE
		tdc.tdc_posicao = ISNULL(@tdt_posicao, tdc.tdc_posicao)
		AND tdc.tdc_situacao <> 3
	GROUP BY
		tdcPermissao.tdc_posicao
	
	CREATE TABLE #AlunoComCompensacao
      (alu_id bigint,
       mtu_id int,
       mtd_id int,
	   PRIMARY KEY (alu_id, mtu_id, mtd_id))
	
	INSERT INTO #AlunoComCompensacao
	SELECT CAA.alu_id
		   ,CAA.mtu_id
		   ,CAA.mtd_id
	FROM CLS_CompensacaoAusencia CPA WITH(NOLOCK)
	INNER JOIN CLS_CompensacaoAusenciaAluno CAA WITH(NOLOCK)
		ON CPA.tud_id = CAA.tud_id
		AND CPA.cpa_id = CAA.cpa_id
		AND CAA.caa_situacao <> 3
    WHERE CPA.tud_id = @tud_id
	   AND CPA.tpc_id = @tpc_id
	   AND (CPA.cpa_situacao <> 3)
	GROUP BY CAA.alu_id, CAA.mtu_id, CAA.mtd_id
	
	IF (@tud_tipo = 15)
	BEGIN
		--Se os parametros data inicio e data final for nulo será considerado 
		--as 5 ultimas datas de alunas do período. Caso for o período vigente
		--será considerado as 5 ultimas datas de aulas inferior a data atual.  
		IF ((@data_inicio IS NULL) AND (@data_final IS NULL))
		BEGIN
		
			SELECT top 5		
				 Tau.tau_id
				, Tau.tud_id
				, Mtd.alu_id
				, Mtd.mtu_id
				, Mtd.mtd_id
				, taa_frequencia
				, tau_sequencia	
				, tau_data
				, tau_numeroAulas
				, Tau.tau_efetivado
				, Tau.tpc_id
				-- 0 - não possui FJ / 1 - possui FJ que abona / 2 - possui FJ que não abona
				, CASE WHEN afj.afj_id IS NULL
						THEN '0'					    						    
						ELSE (CASE WHEN tjf.tjf_abonaFalta = 1 THEN '1' ELSE '2' END) 
				   END AS falta_justificada
				, CAST(CASE WHEN (EXISTS (
						SELECT Ajf.ajf_id
						FROM ACA_AlunoJustificativaAbonoFalta ajf WITH(NOLOCK)
						WHERE 
							ajf.alu_id = Mtd.alu_id
							AND ajf.tud_id = Mtd.tud_id
							AND ajf.ajf_situacao <> 3
							AND (Tau.tau_data >= ajf.ajf_dataInicio)
							AND (Tau.tau_data <= ajf.ajf_dataFim)
					)) THEN 1 ELSE 0 END AS BIT) AS falta_abonada
				, tdt_posicao
				, taa_frequenciaBitMap
				
				--, (CASE WHEN @usu_id is null or ISNULL(Tau.usu_id, @usu_id) = @usu_id THEN 1 ELSE 0 END) AS permissaoAlteracao
				, CASE WHEN @usu_id is null or ISNULL(Tau.usu_id, @usu_id) = @usu_id THEN 1
			  		   WHEN ISNULL(pde.tdt_posicaoPermissao, 0) > 0 THEN 1 
					   ELSE 0 
				  END AS permissaoAlteracao
				
				, CAST((CASE WHEN ACC.alu_id IS NULL THEN 0 ELSE 1 END) AS BIT) AS AlunoComCompensacao
				, Tau.usu_id
				, Tau.tau_reposicao --
			FROM MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
			INNER JOIN MTR_MatriculaTurma mtu WITH(NOLOCK)
				ON mtu.alu_id = Mtd.alu_id
				AND mtu.mtu_id = Mtd.mtu_id
				AND mtu.mtu_situacao <> 3
			INNER JOIN @dtTurmas dtt
				ON mtu.tur_id = dtt.tur_id
			INNER JOIN CLS_TurmaAula Tau WITH (NOLOCK)
				ON Tau.tud_id = Mtd.tud_id
				AND Tau.tau_situacao <> 3
				AND Tau.tpc_id = @tpc_id
			INNER JOIN #PermissaoDocenteConsulta pdc
				ON Tau.tdt_posicao = pdc.tdt_posicaoPermissao
				AND (pdc.pdc_permissaoConsulta = 1 OR Tau.usu_id = @usu_id OR @usuario_superior = 1)
			LEFT JOIN CLS_TurmaAulaAluno Taa WITH (NOLOCK)
				ON Taa.tud_id = Tau.tud_id
				AND Taa.tau_id = Tau.tau_id
				AND Taa.alu_id = Mtd.alu_id
				AND Taa.mtu_id = Mtd.mtu_id
				AND Taa.mtd_id = Mtd.mtd_id
				AND Taa.taa_situacao <> 3
			LEFT JOIN ACA_AlunoJustificativaFalta afj WITH(NOLOCK)
					ON afj.alu_id = Taa.alu_id
					AND afj.afj_situacao <> 3
					AND (tau_data >= afj.afj_dataInicio)
					AND ((afj.afj_dataFim  IS NULL) OR (tau_data <= afj.afj_dataFim))
			LEFT JOIN ACA_TipoJustificativaFalta tjf WITH(NOLOCK)
				ON tjf.tjf_id = afj.tjf_id
				AND tjf.tjf_situacao <> 3
			LEFT JOIN #AlunoComCompensacao ACC
				ON ACC.alu_id = Mtd.alu_id
				AND ACC.mtd_id = Mtd.mtd_id
				AND ACC.mtu_id = Mtd.mtu_id   
				
			LEFT JOIN #PermissaoDocenteEdicao AS pde
				ON Tau.tdt_posicao = pde.tdt_posicaoPermissao
				 
			WHERE
				Mtd.tud_id = @tud_id
				AND mtd.mtd_situacao <> 3
				AND CAST(Tau.tau_data AS DATE) <= CAST(GETDATE() AS DATE)
			ORDER BY
			 Tau.tau_data DESC	
			 , Tau.tau_id
		END
		ELSE 
		BEGIN 
		
			 -- Se os parametros data inicio e data final não for nulo  
			 -- a pesquisa tera tais datas como filtro de pesquisa, 
			 -- podendo retorna todas as aulas dentro deste intervalo. 
			SELECT
				Tau.tau_id
				, Tau.tud_id
				, Mtd.alu_id
				, Mtd.mtu_id
				, Mtd.mtd_id
				, taa_frequencia
				, tau_sequencia	
				, tau_data
				, tau_numeroAulas
				, Tau.tau_efetivado
				-- 0 - não possui FJ / 1 - possui FJ que abona / 2 - possui FJ que não abona 
				, CASE WHEN afj.afj_id IS NULL
						THEN '0'					    						    
						ELSE (CASE WHEN tjf.tjf_abonaFalta = 1 THEN '1' ELSE '2' END)
				   END AS falta_justificada
				, CAST(CASE WHEN (EXISTS (
						SELECT Ajf.ajf_id
						FROM ACA_AlunoJustificativaAbonoFalta ajf WITH(NOLOCK)
						WHERE 
							ajf.alu_id = Mtd.alu_id
							AND ajf.tud_id = Mtd.tud_id
							AND ajf.ajf_situacao <> 3
							AND (Tau.tau_data >= ajf.ajf_dataInicio)
							AND (Tau.tau_data <= ajf.ajf_dataFim)
					)) THEN 1 ELSE 0 END AS BIT) AS falta_abonada				
				, tdt_posicao
				, taa_frequenciaBitMap
				
				--, (CASE WHEN @usu_id is null or ISNULL(Tau.usu_id, @usu_id) = @usu_id THEN 1 ELSE 0 END) AS permissaoAlteracao
				, CASE WHEN @usu_id is null or ISNULL(Tau.usu_id, @usu_id) = @usu_id THEN 1
			  		   WHEN ISNULL(pde.tdt_posicaoPermissao, 0) > 0 THEN 1 
					   ELSE 0 
				  END AS permissaoAlteracao
				
				, CAST((CASE WHEN ACC.alu_id IS NULL THEN 0 ELSE 1 END) AS BIT) AS AlunoComCompensacao
				, Tau.usu_id
				, Tau.tau_reposicao --
			FROM MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
			INNER JOIN MTR_MatriculaTurma mtu WITH(NOLOCK)
				ON mtu.alu_id = Mtd.alu_id
				AND mtu.mtu_id = Mtd.mtu_id
				AND mtu.mtu_situacao <> 3
			INNER JOIN @dtTurmas dtt
				ON mtu.tur_id = dtt.tur_id
			INNER JOIN CLS_TurmaAula Tau WITH (NOLOCK)
				ON Tau.tud_id = Mtd.tud_id
				AND Tau.tau_situacao <> 3
				AND Tau.tpc_id = @tpc_id
			INNER JOIN #PermissaoDocenteConsulta pdc
				ON Tau.tdt_posicao = pdc.tdt_posicaoPermissao
				AND (pdc.pdc_permissaoConsulta = 1 OR Tau.usu_id = @usu_id OR @usuario_superior = 1)
			LEFT JOIN CLS_TurmaAulaAluno Taa WITH (NOLOCK)
				ON Taa.tud_id = Tau.tud_id
				AND Taa.tau_id = Tau.tau_id
				AND Taa.alu_id = Mtd.alu_id
				AND Taa.mtu_id = Mtd.mtu_id
				AND Taa.mtd_id = Mtd.mtd_id
				AND Taa.taa_situacao <> 3
			LEFT JOIN ACA_AlunoJustificativaFalta afj WITH(NOLOCK)
				ON afj.alu_id = Taa.alu_id
				AND afj.afj_situacao <> 3
				AND (tau_data >= afj.afj_dataInicio)
				AND ((afj.afj_dataFim  IS NULL) OR (tau_data <= afj.afj_dataFim))
			LEFT JOIN ACA_TipoJustificativaFalta tjf WITH(NOLOCK)
				ON tjf.tjf_id = afj.tjf_id
				AND tjf.tjf_situacao <> 3
			LEFT JOIN #AlunoComCompensacao ACC
				ON ACC.alu_id = Mtd.alu_id
				AND ACC.mtd_id = Mtd.mtd_id
				AND ACC.mtu_id = Mtd.mtu_id    
				
			LEFT JOIN #PermissaoDocenteEdicao AS pde
				ON Tau.tdt_posicao = pde.tdt_posicaoPermissao
				
			WHERE
				Mtd.tud_id = @tud_id
				AND Mtd.mtd_situacao <> 3
				AND CAST(Tau.tau_data AS DATE) BETWEEN CAST(@data_inicio AS DATE) AND CAST(@data_final AS DATE)
			ORDER BY
				Tau.tau_data			 
				, Tau.tau_id

		END
	END
	ELSE 
	BEGIN
		--Se os parametros data inicio e data final for nulo será considerado 
		--as 5 ultimas datas de alunas do período. Caso for o período vigente
		--será considerado as 5 ultimas datas de aulas inferior a data atual.  
		IF ((@data_inicio IS NULL) AND (@data_final IS NULL))
		BEGIN
		
			SELECT top 5		
				 Tau.tau_id
				, Tau.tud_id
				, Mtd.alu_id
				, Mtd.mtu_id
				, Mtd.mtd_id
				, taa_frequencia
				, tau_sequencia	
				, tau_data
				, tau_numeroAulas
				, Tau.tau_efetivado
				, Tau.tpc_id
				-- 0 - não possui FJ / 1 - possui FJ que abona / 2 - possui FJ que não abona
				, CASE WHEN afj.afj_id IS NULL
						THEN '0'					    						    
						ELSE (CASE WHEN tjf.tjf_abonaFalta = 1 THEN '1' ELSE '2' END) 
				   END AS falta_justificada
				, CAST(CASE WHEN (EXISTS (
						SELECT Ajf.ajf_id
						FROM ACA_AlunoJustificativaAbonoFalta ajf WITH(NOLOCK)
						WHERE 
							ajf.alu_id = Mtd.alu_id
							AND ajf.tud_id = Mtd.tud_id
							AND ajf.ajf_situacao <> 3
							AND (Tau.tau_data >= ajf.ajf_dataInicio)
							AND (Tau.tau_data <= ajf.ajf_dataFim)
					)) THEN 1 ELSE 0 END AS BIT) AS falta_abonada
				, tdt_posicao
				, taa_frequenciaBitMap
				
				--, (CASE WHEN @usu_id is null or ISNULL(Tau.usu_id, @usu_id) = @usu_id THEN 1 ELSE 0 END) AS permissaoAlteracao
				, CASE WHEN @usu_id is null or ISNULL(Tau.usu_id, @usu_id) = @usu_id THEN 1
			  		   WHEN ISNULL(pde.tdt_posicaoPermissao, 0) > 0 THEN 1 
					   ELSE 0 
				  END AS permissaoAlteracao
				
				, CAST((CASE WHEN ACC.alu_id IS NULL THEN 0 ELSE 1 END) AS BIT) AS AlunoComCompensacao
				, Tau.usu_id
				, Tau.tau_reposicao --
			FROM CLS_TurmaAula Tau WITH (NOLOCK)
			INNER JOIN #PermissaoDocenteConsulta pdc
				ON Tau.tdt_posicao = pdc.tdt_posicaoPermissao
				AND (pdc.pdc_permissaoConsulta = 1 OR Tau.usu_id = @usu_id OR @usuario_superior = 1)
			INNER JOIN MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
				ON Mtd.tud_id = Tau.tud_id
				AND Mtd.mtd_situacao <> 3
			LEFT JOIN CLS_TurmaAulaAluno Taa WITH (NOLOCK)
				ON Taa.tud_id = Tau.tud_id
				AND Taa.tau_id = Tau.tau_id
				AND Taa.alu_id = Mtd.alu_id
				AND Taa.mtu_id = Mtd.mtu_id
				AND Taa.mtd_id = Mtd.mtd_id
				AND Taa.taa_situacao <> 3
			LEFT JOIN ACA_AlunoJustificativaFalta afj WITH(NOLOCK)
					ON afj.alu_id = Taa.alu_id
					AND afj.afj_situacao <> 3
					AND (tau_data >= afj.afj_dataInicio)
					AND ((afj.afj_dataFim  IS NULL) OR (tau_data <= afj.afj_dataFim))
			LEFT JOIN ACA_TipoJustificativaFalta tjf WITH(NOLOCK)
				ON tjf.tjf_id = afj.tjf_id
				AND tjf.tjf_situacao <> 3
			LEFT JOIN #AlunoComCompensacao ACC
				ON ACC.alu_id = Mtd.alu_id
				AND ACC.mtd_id = Mtd.mtd_id
				AND ACC.mtu_id = Mtd.mtu_id    

			LEFT JOIN #PermissaoDocenteEdicao AS pde
				ON Tau.tdt_posicao = pde.tdt_posicaoPermissao

			-- Se for disciplina de docencia compartilhada,
			-- retorno apenas as aulas compartilhadas com a disciplina relacionada do parametro.
			LEFT JOIN CLS_TurmaAulaDisciplinaRelacionada TauRel WITH(NOLOCK)
				ON @tud_tipo = 17
				AND TauRel.tud_id = Tau.tud_id
				AND TauRel.tau_id = Tau.tau_id
				AND TauRel.tud_idRelacionada = @tud_idRelacionada
				
			WHERE
				Tau.tau_situacao <> 3
				AND Tau.tud_id = @tud_id
				AND Tau.tpc_id = @tpc_id
				AND CAST(Tau.tau_data AS DATE) <= CAST(GETDATE() AS DATE)
				AND (@tud_tipo <> 17 OR TauRel.tud_id IS NOT NULL)
			ORDER BY
			 Tau.tau_data DESC	
			 , Tau.tau_id
		END
		ELSE 
		BEGIN 
		
			 -- Se os parametros data inicio e data final não for nulo  
			 -- a pesquisa tera tais datas como filtro de pesquisa, 
			 -- podendo retorna todas as aulas dentro deste intervalo. 
			SELECT
				Tau.tau_id
				, Tau.tud_id
				, Mtd.alu_id
				, Mtd.mtu_id
				, Mtd.mtd_id
				, taa_frequencia
				, tau_sequencia	
				, tau_data
				, tau_numeroAulas
				, Tau.tau_efetivado
				-- 0 - não possui FJ / 1 - possui FJ que abona / 2 - possui FJ que não abona 
				, CASE WHEN afj.afj_id IS NULL
						THEN '0'					    						    
						ELSE (CASE WHEN tjf.tjf_abonaFalta = 1 THEN '1' ELSE '2' END)
				   END AS falta_justificada
				, CAST(CASE WHEN (EXISTS (
						SELECT Ajf.ajf_id
						FROM ACA_AlunoJustificativaAbonoFalta ajf WITH(NOLOCK)
						WHERE 
							ajf.alu_id = Mtd.alu_id
							AND ajf.tud_id = Mtd.tud_id
							AND ajf.ajf_situacao <> 3
							AND (Tau.tau_data >= ajf.ajf_dataInicio)
							AND (Tau.tau_data <= ajf.ajf_dataFim)
					)) THEN 1 ELSE 0 END AS BIT) AS falta_abonada				
				, tdt_posicao
				, taa_frequenciaBitMap
				
				--, (CASE WHEN @usu_id is null or ISNULL(Tau.usu_id, @usu_id) = @usu_id THEN 1 ELSE 0 END) AS permissaoAlteracao
				, CASE WHEN @usu_id is null or ISNULL(Tau.usu_id, @usu_id) = @usu_id THEN 1
			  		   WHEN ISNULL(pde.tdt_posicaoPermissao, 0) > 0 THEN 1 
					   ELSE 0 
				  END AS permissaoAlteracao
				
				, CAST((CASE WHEN ACC.alu_id IS NULL THEN 0 ELSE 1 END) AS BIT) AS AlunoComCompensacao
				, Tau.usu_id
				, Tau.tau_reposicao --
			FROM CLS_TurmaAula Tau WITH (NOLOCK)
			INNER JOIN #PermissaoDocenteConsulta pdc
				ON Tau.tdt_posicao = pdc.tdt_posicaoPermissao
				AND (pdc.pdc_permissaoConsulta = 1 OR Tau.usu_id = @usu_id OR @usuario_superior = 1)
			INNER JOIN MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
				ON Mtd.tud_id = Tau.tud_id
				AND Mtd.mtd_situacao <> 3
			LEFT JOIN CLS_TurmaAulaAluno Taa WITH (NOLOCK)
				ON Taa.tud_id = Tau.tud_id
				AND Taa.tau_id = Tau.tau_id
				AND Taa.alu_id = Mtd.alu_id
				AND Taa.mtu_id = Mtd.mtu_id
				AND Taa.mtd_id = Mtd.mtd_id
				AND Taa.taa_situacao <> 3
			LEFT JOIN ACA_AlunoJustificativaFalta afj WITH(NOLOCK)
				ON afj.alu_id = Taa.alu_id
				AND afj.afj_situacao <> 3
				AND (tau_data >= afj.afj_dataInicio)
				AND ((afj.afj_dataFim  IS NULL) OR (tau_data <= afj.afj_dataFim))
			LEFT JOIN ACA_TipoJustificativaFalta tjf WITH(NOLOCK)
				ON tjf.tjf_id = afj.tjf_id
				AND tjf.tjf_situacao <> 3
			LEFT JOIN #AlunoComCompensacao ACC
				ON ACC.alu_id = Mtd.alu_id
				AND ACC.mtd_id = Mtd.mtd_id
				AND ACC.mtu_id = Mtd.mtu_id    
				
			LEFT JOIN #PermissaoDocenteEdicao AS pde
				ON Tau.tdt_posicao = pde.tdt_posicaoPermissao
			
			-- Se for disciplina de docencia compartilhada,
			-- retorno apenas as aulas compartilhadas com a disciplina relacionada do parametro.
			LEFT JOIN CLS_TurmaAulaDisciplinaRelacionada TauRel WITH(NOLOCK)
				ON @tud_tipo = 17
				AND TauRel.tud_id = Tau.tud_id
				AND TauRel.tau_id = Tau.tau_id
				AND TauRel.tud_idRelacionada = @tud_idRelacionada	
				
			WHERE
				Tau.tau_situacao <> 3
				AND Tau.tud_id = @tud_id
				AND Tau.tpc_id = @tpc_id
				AND CAST(Tau.tau_data AS DATE) BETWEEN CAST(@data_inicio AS DATE) AND CAST(@data_final AS DATE)
				AND (@tud_tipo <> 17 OR TauRel.tud_id IS NOT NULL)

				-- [Carla 14/12/2016] Filtrei data de matrícula e saída no periodo, para não trazer mtds duplicados, no caso
				-- de Recuperação paralela, que tem dois mtds no mesmo mtu.
				AND Mtd.mtd_dataMatricula <=  CAST(@data_final AS DATE)
				AND (Mtd.mtd_situacao = 1 OR Mtd.mtd_dataSaida >= CAST(@data_inicio AS DATE))

				--and Mtd.alu_id = 745963
				--AND Mtd.mtd_id = 16

			ORDER BY
				Tau.tau_data			 
				, Tau.tau_id
		END
	END
    
    DROP TABLE #AlunoComCompensacao
    DROP TABLE #PermissaoDocenteEdicao
	DROP TABLE #PermissaoDocenteConsulta
END
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
PRINT N'Altering [dbo].[NEW_CLS_TurmaAulaAluno_Territorio_SelectBy_TurmaDisciplina_Aluno]'
GO
-- ========================================================================
---- Criado: Juliano Real 
---- Data: 08/07/2016
---- Description: Retorna as aulas criadas na experiencia com os territorios relacionados
-- ========================================================================
ALTER PROCEDURE [dbo].[NEW_CLS_TurmaAulaAluno_Territorio_SelectBy_TurmaDisciplina_Aluno]
	@tud_id BIGINT
	, @tpc_id INT
	, @data_inicio DATETIME
	, @data_final DATETIME
	, @usu_id UNIQUEIDENTIFIER
	, @tdt_posicao TINYINT
	, @tud_idRelacionada BIGINT	
	, @usuario_superior BIT
	, @dtTurmas TipoTabela_Turma READONLY
AS
BEGIN

	DECLARE @tud_tipo TINYINT;
	SELECT
		@tud_tipo = ISNULL(tud.tud_tipo,0)
	FROM
		TUR_TurmaDisciplina tud WITH(NOLOCK)
	WHERE
		tud.tud_id = @tud_id
		AND tud.tud_situacao <> 3
		
	CREATE TABLE #PermissaoDocenteEdicao (tdt_posicaoPermissao INT, PRIMARY KEY(tdt_posicaoPermissao));
	CREATE TABLE #PermissaoDocenteConsulta (tdt_posicaoPermissao INT, pdc_permissaoConsulta BIT, PRIMARY KEY(tdt_posicaoPermissao))

	;WITH tbDadosPermissaoDocenteConsulta AS	
	(
		SELECT 
			tdcPermissao.tdc_posicao AS tdt_posicaoPermissao
			, pdc.pdc_permissaoConsulta
		FROM
			ACA_TipoDocente tdc WITH(NOLOCK)
			INNER JOIN CFG_PermissaoDocente pdc WITH(NOLOCK)
				ON tdc.tdc_id = pdc.tdc_id
				AND pdc.pdc_modulo = 1
				AND (@usuario_superior = 1 OR pdc.pdc_permissaoConsulta = 1) -- retonar as permissões de consulta
				AND pdc.pdc_situacao <> 3 
			INNER JOIN ACA_TipoDocente tdcPermissao WITH(NOLOCK)
				ON tdcPermissao.tdc_id = pdc.tdc_idPermissao
				AND tdcPermissao.tdc_situacao <> 3
		WHERE
			(tdc.tdc_posicao = ISNULL(@tdt_posicao, tdc.tdc_posicao) OR @usuario_superior = 1)
			AND tdc.tdc_situacao <> 3
			
		UNION 
		
		SELECT ISNULL(@tdt_posicao, 1), 0 AS pdc_permissaoConsulta
	)
	INSERT INTO #PermissaoDocenteConsulta (tdt_posicaoPermissao, pdc_permissaoConsulta)
	SELECT 
		pdc.tdt_posicaoPermissao
		, MAX(pdc.pdc_permissaoConsulta) AS pdc_permissaoConsulta
	FROM
		tbDadosPermissaoDocenteConsulta pdc WITH(NOLOCK)
	GROUP BY
		pdc.tdt_posicaoPermissao
	
	INSERT INTO #PermissaoDocenteEdicao (tdt_posicaoPermissao)
    SELECT 
		tdcPermissao.tdc_posicao AS tdt_posicaoPermissao
	FROM
		ACA_TipoDocente tdc WITH(NOLOCK)
		INNER JOIN CFG_PermissaoDocente pdc WITH(NOLOCK)
			ON tdc.tdc_id = pdc.tdc_id
			AND pdc.pdc_modulo = 1
			AND pdc.pdc_permissaoEdicao = 1  -- retonar as permissões de edição
			AND pdc.pdc_situacao <> 3 
		INNER JOIN ACA_TipoDocente tdcPermissao WITH(NOLOCK)
			ON tdcPermissao.tdc_id = pdc.tdc_idPermissao
			AND tdcPermissao.tdc_situacao <> 3
	WHERE
		tdc.tdc_posicao = ISNULL(@tdt_posicao, tdc.tdc_posicao)
		AND tdc.tdc_situacao <> 3
	GROUP BY
		tdcPermissao.tdc_posicao
	
	CREATE TABLE #AlunoComCompensacao
      (alu_id bigint,
       mtu_id int,
       mtd_id int,
	   PRIMARY KEY (alu_id, mtu_id, mtd_id))
	
	INSERT INTO #AlunoComCompensacao
	SELECT CAA.alu_id
		   ,CAA.mtu_id
		   ,CAA.mtd_id
	FROM CLS_CompensacaoAusencia CPA WITH(NOLOCK)
	INNER JOIN CLS_CompensacaoAusenciaAluno CAA WITH(NOLOCK)
		ON CPA.tud_id = CAA.tud_id
		AND CPA.cpa_id = CAA.cpa_id
		AND CAA.caa_situacao <> 3
    WHERE CPA.tud_id = @tud_id
	   AND CPA.tpc_id = @tpc_id
	   AND (CPA.cpa_situacao <> 3)
	GROUP BY CAA.alu_id, CAA.mtu_id, CAA.mtd_id

	CREATE TABLE #TurmaDisciplinaTerritorio(
		tud_id BIGINT,
		tau_id INT,
		tud_idExperiencia BIGINT,
		tau_idExperiencia INT,
		tte_vigenciaInicio DATE,
		tte_vigenciaFim DATE,
		tud_tipo INT)
		
	--Insere os tud_ids dos Territorio
	INSERT INTO #TurmaDisciplinaTerritorio
	(
		tud_id,
		tau_id, 
		tud_idExperiencia,
		tau_idExperiencia,
		tte_vigenciaInicio, 
		tte_vigenciaFim,
		tud_tipo
	)
	SELECT 
		TTDT.tud_idTerritorio,
		CTAT.tau_idTerritorio,
		TTDT.tud_idExperiencia,
		CTAT.tau_idExperiencia,
		TTDT.tte_vigenciaInicio, 
		TTDT.tte_vigenciaFim,
		19
	FROM 
		TUR_TurmaDisciplinaTerritorio AS TTDT WITH(NOLOCK)
		INNER JOIN CLS_TurmaAulaTerritorio AS CTAT WITH (NOLOCK)
			ON CTAT.tud_idExperiencia = TTDT.tud_idExperiencia
			AND CTAT.tud_idTerritorio = TTDT.tud_idTerritorio
		INNER JOIN CLS_TurmaAula AS TAUE WITH(NOLOCK)
			ON CTAT.tud_idExperiencia = TAUE.tud_id
			AND CTAT.tau_idExperiencia = TAUE.tau_id
		INNER JOIN CLS_TurmaAula AS TAUT WITH(NOLOCK)
			ON CTAT.tud_idTerritorio = TAUT.tud_id
			AND CTAT.tau_idTerritorio = TAUT.tau_id
			AND ISNULL(TAUE.tdt_posicao,0) = ISNULL(TAUT.tdt_posicao,0)
	WHERE
		((TTDT.tud_idExperiencia = @tud_id))
		AND (
			((TTDT.tte_vigenciaInicio >= @data_inicio) AND (TTDT.tte_vigenciaFim <= @data_final))
			OR((TTDT.tte_vigenciaInicio <= @data_inicio) AND (TTDT.tte_vigenciaFim >= @data_inicio))
			OR((TTDT.tte_vigenciaInicio <= @data_final) AND (TTDT.tte_vigenciaFim >= @data_final))
			)
		AND TTDT.tte_situacao <> 3

	--Insere os tud_ids da Experiencia
	INSERT INTO #TurmaDisciplinaTerritorio
	(
		tud_id,
		tau_id, 
		tud_idExperiencia,
		tau_idExperiencia,
		tte_vigenciaInicio, 
		tte_vigenciaFim,
		tud_tipo
	)
	SELECT 
		TTDT.tud_idExperiencia,
		TTDT.tau_idExperiencia,
		NULL,
		NULL,
		TTDT.tte_vigenciaInicio, 
		TTDT.tte_vigenciaFim,
		18
	FROM 
		#TurmaDisciplinaTerritorio AS TTDT 
	WHERE
		TTDT.tud_idExperiencia = @tud_id
	GROUP BY
		TTDT.tud_idExperiencia,
		TTDT.tau_idExperiencia,
		TTDT.tte_vigenciaInicio, 
		TTDT.tte_vigenciaFim

		
	-- Se os parametros data inicio e data final não for nulo  
	-- a pesquisa tera tais datas como filtro de pesquisa, 
	-- podendo retorna todas as aulas dentro deste intervalo. 
	SELECT
		Tau.tau_id
		, Tau.tud_id
		, Mtd.alu_id
		, Mtd.mtu_id
		, Mtd.mtd_id
		, taa_frequencia
		, tau_sequencia	
		, tau_data
		, tau_numeroAulas
		, Tau.tau_efetivado
		, CASE WHEN afj.afj_id IS NULL
				THEN '0'					    						    
				ELSE (CASE WHEN tjf.tjf_abonaFalta = 1 THEN '1' ELSE '2' END)
			END AS falta_justificada
		, CAST(CASE WHEN (EXISTS (
				SELECT Ajf.ajf_id
				FROM ACA_AlunoJustificativaAbonoFalta ajf WITH(NOLOCK)
				WHERE 
					ajf.alu_id = Mtd.alu_id
					AND ajf.tud_id = Mtd.tud_id
					AND ajf.ajf_situacao <> 3
					AND (Tau.tau_data >= ajf.ajf_dataInicio)
					AND (Tau.tau_data <= ajf.ajf_dataFim)
			)) THEN 1 ELSE 0 END AS BIT) AS falta_abonada		
		, tdt_posicao
		, taa_frequenciaBitMap
				
		--, (CASE WHEN @usu_id is null or ISNULL(Tau.usu_id, @usu_id) = @usu_id THEN 1 ELSE 0 END) AS permissaoAlteracao
		, CASE WHEN @usu_id is null or ISNULL(Tau.usu_id, @usu_id) = @usu_id THEN 1
			  	WHEN ISNULL(pde.tdt_posicaoPermissao, 0) > 0 THEN 1 
				ELSE 0 
			END AS permissaoAlteracao
				
		, CAST((CASE WHEN ACC.alu_id IS NULL THEN 0 ELSE 1 END) AS BIT) AS AlunoComCompensacao
		, Tau.usu_id
		, Tau.tau_reposicao
		, TDT.tud_tipo
		, TDT.tud_idExperiencia
		, TDT.tau_idExperiencia
	FROM #TurmaDisciplinaTerritorio AS TDT
	INNER JOIN CLS_TurmaAula AS Tau WITH(NOLOCK)
		ON Tau.tud_id = TDT.tud_id 
		AND Tau.tau_id = TDT.tau_id
		AND Tau.tau_data >= TDT.tte_vigenciaInicio 
		AND Tau.tau_data <= ISNULL(TDT.tte_vigenciaFim, Tau.tau_data)
		AND Tau.tau_situacao <> 3
	INNER JOIN #PermissaoDocenteConsulta pdc
		ON Tau.tdt_posicao = pdc.tdt_posicaoPermissao
		AND (pdc.pdc_permissaoConsulta = 1 OR Tau.usu_id = @usu_id OR @usuario_superior = 1)
	INNER JOIN MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
		ON Mtd.tud_id = Tau.tud_id
		--AND (Mtd.mtd_dataMatricula <= TDT.tte_vigenciaFim)
		--AND (Mtd.mtd_dataSaida IS NULL OR Mtd.mtd_dataSaida >= TDT.tte_vigenciaInicio)	
		AND (mtd_situacao = 1 OR (mtd_situacao = 5 AND ISNULL(mtd_numeroChamada, 0) >= 0))
	LEFT JOIN CLS_TurmaAulaAluno Taa WITH (NOLOCK)
		ON Taa.tud_id = Tau.tud_id
		AND Taa.tau_id = Tau.tau_id
		AND Taa.alu_id = Mtd.alu_id
		AND Taa.mtu_id = Mtd.mtu_id
		AND Taa.mtd_id = Mtd.mtd_id
		AND Taa.taa_situacao <> 3
	LEFT JOIN ACA_AlunoJustificativaFalta afj WITH(NOLOCK)
		ON afj.alu_id = Taa.alu_id
		AND afj.afj_situacao <> 3
		AND (tau_data >= afj.afj_dataInicio)
		AND ((afj.afj_dataFim  IS NULL) OR (tau_data <= afj.afj_dataFim))
	LEFT JOIN ACA_TipoJustificativaFalta tjf WITH(NOLOCK)
		ON tjf.tjf_id = afj.tjf_id
		AND tjf.tjf_situacao <> 3
	LEFT JOIN #AlunoComCompensacao ACC
		ON ACC.alu_id = Mtd.alu_id
		AND ACC.mtd_id = Mtd.mtd_id
		AND ACC.mtu_id = Mtd.mtu_id    
				
	LEFT JOIN #PermissaoDocenteEdicao AS pde
		ON Tau.tdt_posicao = pde.tdt_posicaoPermissao
								
	WHERE
		Tau.tau_situacao <> 3
		AND Tau.tpc_id = @tpc_id
		AND CAST(Tau.tau_data AS DATE) BETWEEN CAST(@data_inicio AS DATE) AND CAST(@data_final AS DATE)
	ORDER BY
		tau_data,
		Tau.tud_id

    DROP TABLE #AlunoComCompensacao
    DROP TABLE #PermissaoDocenteEdicao
	DROP TABLE #PermissaoDocenteConsulta
	DROP TABLE #TurmaDisciplinaTerritorio
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
PRINT N'Altering [dbo].[FN_Select_FaltasAulasBy_Turma_DataMovimentacao]'
GO
-- =============================================
-- Author:		Jean Silva
-- Create date: 23/02/2012
-- Description:	Retorna uma tabela com os alunos da turma, com a quantidade de aulas e faltas lançadas
--				pra cada um. Verifica o tipo de lançamento de frequência para buscar as quantidades
--				e somar as frequências. (Utilizado na movimentação)
-- =============================================
ALTER FUNCTION [dbo].[FN_Select_FaltasAulasBy_Turma_DataMovimentacao]
(	
	@tipoLancamento TINYINT
	, @tpc_id INT
	, @tur_id BIGINT
	, @dataMovimentacao DATE
	, @diario BIT
	, @alu_id BIGINT
	, @mtu_id INT
)
-- Quantidade de faltas e aulas para do aluno
RETURNS @TabelaQtdes TABLE 
(
	alu_id BIGINT NOT NULL
	, mtu_id INT NOT NULL
	, qtFaltas INT NULL
	, qtAulas INT NULL
)
AS
BEGIN
	-- Guardar todos os tud_ids da turma
	DECLARE @TabelaTudID TABLE (tud_id BIGINT NOT NULL)
	
	INSERT INTO @TabelaTudID (tud_id)
	-- Trazer todos os tud_id
	SELECT 
		Tds.tud_id 
	FROM TUR_TurmaRelTurmaDisciplina RelTur WITH(NOLOCK)
	INNER JOIN TUR_TurmaDisciplina Tds WITH(NOLOCK)
		ON (Tds.tud_id = RelTur.tud_id)
			AND (Tds.tud_situacao <> 3)
	WHERE
		RelTur.tur_id = @tur_id

	INSERT INTO @TabelaQtdes(alu_id, mtu_id, qtFaltas, qtAulas)
	SELECT
		Mtu.alu_id
		, Mtu.mtu_id
		,
		-- Qt faltas do aluno.
		CASE 
			WHEN (@tipoLancamento = 1 OR @diario = 1) THEN -- 1-Aulas planejadas
				(SELECT 
					SUM(taa_frequencia)
				 FROM 
					CLS_TurmaAulaAluno Taa WITH(NOLOCK)
				 INNER JOIN @TabelaTudID Tud 
					ON Tud.tud_id = Taa.tud_id
				 INNER JOIN CLS_TurmaAula Tau WITH(NOLOCK)
					ON Tau.tau_id = Taa.tau_id
					AND Tau.tud_id = Taa.tud_id
					AND Tau.tau_situacao <> 3
				 LEFT JOIN ACA_AlunoJustificativaFalta afj WITH(NOLOCK)
					ON Taa.alu_id = afj.alu_id
					AND afj.afj_situacao <> 3
					AND tau_data >= afj.afj_dataInicio
					AND ((afj.afj_dataFim  IS NULL) OR (tau_data <= afj.afj_dataFim))
				 LEFT JOIN ACA_TipoJustificativaFalta tjf WITH(NOLOCK)
					ON tjf.tjf_id = afj.tjf_id
					AND tjf.tjf_situacao <> 3
				 WHERE 
					Taa.taa_situacao <> 3
					AND Taa.alu_id = Mtu.alu_id
					AND Taa.mtu_id = Mtu.mtu_id
					AND Tau.tpc_id = @tpc_id
					AND tau.tau_data <= CAST(@dataMovimentacao AS DATE)
					AND ((tjf.tjf_abonaFalta IS NULL) OR (tjf.tjf_abonaFalta = 0))
				) 
		END AS QtFaltasAluno
		,
		-- Qt aulas do aluno.
		CASE 
			WHEN (@tipoLancamento = 1 OR @diario = 1) THEN -- 1-Aulas planejadas
				(SELECT
					SUM(tau_numeroAulas)
				 FROM 
					CLS_TurmaAula Tau WITH(NOLOCK)
				INNER JOIN @TabelaTudID Tud
					ON (Tud.tud_id = Tau.tud_id)
				 WHERE 
					tau_situacao <> 3
					AND tpc_id = @tpc_id
					AND tau_data >= Mtu.mtu_dataMatricula
					AND tau_data <= CAST(@dataMovimentacao AS DATE)
					AND ((Mtu.mtu_dataSaida IS NULL) OR (tau_data < Mtu.mtu_dataSaida))
				)
		END AS QtAulasAluno
	FROM 
		MTR_MatriculaTurma Mtu WITH(NOLOCK)
	WHERE 
		Mtu.alu_id = @alu_id
		AND Mtu.mtu_id = @mtu_id

	RETURN;
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
PRINT N'Creating [dbo].[STP_ACA_AlunoJustificativaFalta_DELETE]'
GO

CREATE PROCEDURE [dbo].[STP_ACA_AlunoJustificativaFalta_DELETE]
	@alu_id BIGINT
	, @afj_id INT

AS
BEGIN
	DELETE FROM 
		ACA_AlunoJustificativaFalta 
	WHERE 
		alu_id = @alu_id 
		AND afj_id = @afj_id 

		
	DECLARE @ret INT
	SELECT @ret = ISNULL(@@ROWCOUNT,-1)
	RETURN @ret
	
END

GO
PRINT N'Creating [dbo].[NEW_ACA_AlunoJustificativaFalta_SelectBy_DCL_Protocolo]'
GO

-- =============================================
-- Author:		Cesar Henrique Marcusso
-- Create date: 09/12/2013
-- Description:	Retorna inf. detalhadas do Aluno/Escola/Periodo Justificativa conforme protocolo (pro_id)
-- =============================================
CREATE PROCEDURE [dbo].[NEW_ACA_AlunoJustificativaFalta_SelectBy_DCL_Protocolo]
	@pro_id UNIQUEIDENTIFIER
AS
BEGIN
	SELECT pes_nome,
		   esc_nome,	
		   tur_codigo,
		   Convert(varchar, afj_dataInicio, 103) as afj_dataInicio,
		   Convert(varchar, afj_dataFim, 103) as afj_dataFim 	      
	FROM ACA_AlunoJustificativaFalta as Afj
	     inner join ACA_Aluno as Alu WITH(NOLOCK) ON
		            Afj.alu_id = Alu.alu_id and  
		            Alu.alu_situacao <> 3

		 inner join MTR_MatriculaTurma as Mtu WITH(NOLOCK) ON 
				    Alu.alu_id = Mtu.alu_id and          
				    Mtu.mtu_situacao <> 3  
				    
		 inner join ACA_AlunoCurriculo as Alc WITH(NOLOCK) ON 
				    Mtu.alu_id = Alc.alu_id and          
				    Mtu.cur_id = Alc.cur_id and
				    Mtu.crr_id = Alc.crr_id and
				    Mtu.crp_id = Alc.crp_id and
				    Mtu.alc_id = Alc.alc_id and
				    Alc.alc_situacao <> 3  
				    
	     inner join ESC_Escola as Esc WITH(NOLOCK) ON    	 
 		      	    Alc.esc_id = Esc.esc_id
		      	    and Esc.esc_situacao <>  3
		      	    
		 inner join TUR_Turma as Tur WITH(NOLOCK) ON
		      	    Mtu.tur_id = Tur.tur_id 
		      	    and Tur.tur_situacao <> 3	 				    
		 inner join VW_DadosAlunoPessoa as Pes ON
					Alu.alu_id = Pes.alu_id
	WHERE    	    	
		 Afj.afj_situacao <> 3 and Afj.pro_id = @pro_id 

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
PRINT N'Creating [dbo].[NEW_ACA_AlunoJustificativaFalta_SelectBy_Aluno]'
GO
-- =======================================================
-- Author:		Aline Dornelas
-- Create date: 22/07/2011 12:09
-- Description: Retorna todas as justificativa de falta
--				ativas, filtrando pelo: alu_id
-- =======================================================
CREATE PROCEDURE [dbo].[NEW_ACA_AlunoJustificativaFalta_SelectBy_Aluno]
	@alu_id BIGINT
AS
BEGIN
	
	SELECT 
		alu_id
		,afj_id
		,tjf.tjf_id
		,tjf.tjf_nome
		,afj_dataInicio
		,afj_dataFim
		,afj_situacao
		,afj_dataCriacao
		,afj_dataAlteracao
		,afj_observacao
	FROM 
		ACA_AlunoJustificativaFalta afj WITH(NOLOCK)
		INNER JOIN ACA_TipoJustificativaFalta tjf WITH(NOLOCK)
		ON afj.tjf_id = tjf.tjf_id
		AND tjf.tjf_situacao <> 3
	WHERE 
		afj_situacao <> 3
		AND alu_id = @alu_id 
	ORDER BY afj_dataInicio
		
	SELECT @@ROWCOUNT	
	
END

GO
PRINT N'Altering [dbo].[FN_Select_FaltasAulasBy_TurmaDisciplina_Completa]'
GO
-- =============================================
-- Author:		Carla Frascareli
-- Create date: 18/01/2012
-- Description:	Retorna uma tabela com os alunos da disciplina, com a quantidade de aulas e faltas
--				lançadas pra cada um. Verifica o tipo de lançamento de frequência para buscar as
--				quantidades e somar as frequências.

---- Alterado: Marcia Haga - 26/01/2015
---- Description: Corrigido para retornar apenas os dados referentes
---- as disciplinas do mesmo calendario.

-- Alterado: Pedro Silva - 29/07/2015
-- Description: Adicionados na tabela de retorno os campos AulasNormais e AulasReposicao, para serem usados no fechamento automático
-- A lógica pra preecher estes campos foi feita apenas para o tipoLançamento = 6 (que é o único usado em SP por enquanto)
-- para os outros tipos estes campos serão retornados nulos por enquanto.
-- Além disso, adicionei tratamento ao final da function para tratar o retorno nulo de alguns campos, quando deveriam retornar 0.
-- =============================================
ALTER FUNCTION [dbo].[FN_Select_FaltasAulasBy_TurmaDisciplina_Completa]
(	
	@tipoLancamento TINYINT
	, @tpc_id INT 
	, @tud_id BIGINT
	, @fav_calculoQtdeAulasDadas TINYINT
	/*
		tipoDocente
		0 - administrador
		1 - titular
		5 - especial
	*/
	, @tipoDocente TINYINT = 0
)
-- Quantidade de faltas e aulas para do aluno
RETURNS @TabelaQtdes TABLE 
(
	alu_id BIGINT NOT NULL
	, mtu_id INT NOT NULL
	, mtd_id INT NOT NULL
	, qtFaltas INT NULL
	, qtAulas INT NULL
	, qtFaltasReposicao INT NULL
	, qtAulasNormais INT NULL
	, qtAulasReposicao INT NULL
	, qtFaltasReposicaoNaoAcumuladas INT NULL
)
AS
BEGIN

	declare @TabelaQtdesFaltas TABLE 
	(	alu_id BIGINT NOT NULL
		, mtu_id INT NOT NULL
		, mtd_id INT NOT NULL
		, qtFaltas INT NULL
		, qtFaltasReposicao INT NULL)
		
	declare @TabelaQtdesFaltas_SemAcumularReposicao TABLE 
	(	alu_id BIGINT NOT NULL
		, mtu_id INT NOT NULL
		, mtd_id INT NOT NULL
		, qtFaltasReposicaoNaoAcumulada INT NULL)
	
	declare @TabelaQtdesAulas TABLE 
	(   alu_id BIGINT NOT NULL
		, mtu_id INT NOT NULL
		, mtd_id INT NOT NULL
		, qtAulas INT NULL
		, qtAulasNormais INT NULL
		, qtAulasReposicao INT NULL)
		
	declare @tbTurmaAula TABLE
	(	tau_id BIGINT
		,tud_id BIGINT
		,tau_data DATE
		,tau_reposicao INT
		,tpc_id INT
		,tpc_ordem INT)


	DECLARE @cal_id INT;
	SELECT @cal_id = tur.cal_id 
	FROM TUR_TurmaRelTurmaDisciplina ttd WITH(NOLOCK)
	INNER JOIN TUR_Turma tur WITH(NOLOCK)
		ON tur.tur_id = ttd.tur_id	
		AND tur.tur_situacao <> 3
	WHERE ttd.tud_id = @tud_id

	DECLARE @tud_tipo TINYINT;
	SELECT
			@tud_tipo = tud_tipo
		FROM
			TUR_TurmaDisciplina WITH(NOLOCK)
		WHERE
			tud_id = @tud_id
			AND tud_situacao <> 3

	/**
	* O cálculo por aulas dadas considera a quantidade de aulas e faltas do lançamento do professor, com algumas observações:
	* Quando a disciplina é "complemento da regência" (Inglês), a quantidade de aulas vêm do Inglês, e a qt de faltas, da regência.
	* Quando é docente especial (posição 5), precisa trazer as aulas separados da posição 5 e da 1 (titular).
	* Quando não é docente especial, sempre considera apenas aulas de titular e substituto (nunca de projetos ou compartilhado).
	* Para todos os casos, considerar as aulas dos alunos em todas as turmas que ele passou no bimestre, ou seja,
	*	quando houver uma transferência, as quantidades de aulas e faltas devem ser somadas (da turma de origem e destino).
	*/
	DECLARE @tbAlunos TABLE (alu_id BIGINT NOT NULL, mtu_id INT NOT NULL, mtd_id INT NOT NULL, tud_id BIGINT NOT NULL
		, mtu_idOrigem INT NOT NULL, mtd_idOrigem INT NOT NULL -- Origem - matrícula do aluno no @tud_id informado.
		, mtd_dataMatricula DATE, mtd_dataSaida DATE
		, tud_tipo TINYINT, tud_idRegencia BIGINT NULL, tud_idAluno BIGINT NULL);
	
	-- Configurar as posições de aulas possíveis.
	DECLARE @tbPosicaoDocente TABLE (tdc_posicao TINYINT NOT NULL)
	
	DECLARE @qtdeTitulares INT;
	
	IF(@tipoLancamento = 2) -- 2-Período
	BEGIN
		IF (@tud_tipo = 15)
		BEGIN
			;WITH MatriculaTurmaDisciplinaMultisseriada AS
			(
				SELECT
					mtdDocente.alu_id,
					mtdDocente.mtu_id,
					mtdDocente.mtd_id AS mtd_idDocente,
					tudDocente.tud_id AS tud_idDocente,
					mtdAluno.mtd_id AS mtd_idAluno,
					tudAluno.tud_id AS tud_idAluno,
					mtdDocente.mtd_dataMatricula AS mtd_dataMatriculaDocente,
					mtdDocente.mtd_dataSaida AS mtd_dataSaidaDocente,
					mtdAluno.mtd_dataMatricula AS mtd_dataMatriculaAluno,
					mtdAluno.mtd_dataSaida AS mtd_dataSaidaAluno
				FROM
					MTR_MatriculaTurmaDisciplina mtdDocente WITH(NOLOCK)
					INNER JOIN TUR_TurmaDisciplina tudDocente WITH(NOLOCK)
						ON tudDocente.tud_id = mtdDocente.tud_id
						AND tudDocente.tud_tipo = @tud_tipo
						AND tudDocente.tud_situacao <> 3
					INNER JOIN MTR_MatriculaTurmaDisciplina mtdAluno WITH(NOLOCK)
						ON mtdAluno.alu_id = mtdDocente.alu_id
						AND mtdAluno.mtu_id = mtdDocente.mtu_id
						AND mtdAluno.mtd_situacao IN (1,5)
					INNER JOIN TUR_TurmaDisciplina tudAluno WITH(NOLOCK)
						ON tudAluno.tud_id = mtdAluno.tud_id
						AND tudAluno.tud_tipo = 16
						AND tudAluno.tud_situacao <> 3
				WHERE 
					mtdDocente.tud_id = @tud_id
					AND mtdDocente.mtd_situacao IN (1,5)
				GROUP BY
					mtdDocente.alu_id,
					mtdDocente.mtu_id,
					mtdDocente.mtd_id,
					tudDocente.tud_id,
					mtdAluno.mtd_id,
					tudAluno.tud_id,
					mtdDocente.mtd_dataMatricula,
					mtdDocente.mtd_dataSaida,
					mtdAluno.mtd_dataMatricula,
					mtdAluno.mtd_dataSaida
			)

			, tbTurmaDisciplina AS
			(
				SELECT tud_idAluno AS tud_id
				FROM MatriculaTurmaDisciplinaMultisseriada
				GROUP BY tud_idAluno

				UNION 

				SELECT @tud_id AS tud_id
			)

			INSERT INTO @TabelaQtdes(alu_id, mtu_id, mtd_id, qtFaltas, qtAulas)
			SELECT
				Mtd.alu_id
				, Mtd.mtu_id
				, Mtd.mtd_idAluno
				,
				-- Qt faltas do aluno.
				0 AS QtFaltasAluno
				,
				-- Qt aulas do aluno.
				0 AS QtAulasAluno
			FROM MatriculaTurmaDisciplinaMultisseriada Mtd
		END
		ELSE
		BEGIN 
			INSERT INTO @TabelaQtdes(alu_id, mtu_id, mtd_id, qtFaltas, qtAulas)
			SELECT
				Mtd.alu_id
				, Mtd.mtu_id
				, Mtd.mtd_id
				,
				-- Qt faltas do aluno.
				0 AS QtFaltasAluno
				,
				-- Qt aulas do aluno.
				0 AS QtAulasAluno
			FROM MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
			WHERE 
				(Mtd.tud_id = @tud_id)
				AND mtd_situacao IN (1,5)
		END
	END
	ELSE IF(@tipoLancamento IN (1, 4)) -- 1-Aulas planejadas, 4-Aulas planejadas e mensal
	BEGIN
		DECLARE @crp_qtdeTemposDia INT
		DECLARE @tur_id BIGINT
		
		IF (@tud_tipo = 15)
		BEGIN
			SELECT TOP 1 
				@crp_qtdeTemposDia = crp.crp_qtdeTemposDia, 
				@tur_id = turRtud.tur_id
			FROM TUR_TurmaRelTurmaDisciplina turRtud WITH(NOLOCK)
			INNER JOIN TUR_TurmaDisciplina AS tud WITH (NOLOCK)
				ON turRtud.tud_id = tud.tud_id
			INNER JOIN TUR_TurmaDisciplinaRelDisciplina disRtud WITH(NOLOCK)
				ON tud.tud_id = disRtud.tud_id
			INNER JOIN ACA_Disciplina dis WITH(NOLOCK)
				ON dis.dis_id = disRtud.dis_id
				AND dis.dis_situacao <> 3
			INNER JOIN TUR_TurmaCurriculo tcr WITH(NOLOCK)
				ON  tcr.tur_id = turRtud.tur_id
			INNER JOIN ACA_CurriculoPeriodo crp WITH(NOLOCK)
				ON crp.cur_id = tcr.cur_id
				AND crp.crr_id = tcr.crr_id
				AND crp.crp_id = tcr.crp_id
			WHERE 
				turRtud.tud_id = @tud_id
				AND tcr.tcr_situacao <> 3	
		
			;WITH MatriculaTurmaDisciplinaMultisseriada AS
			(
				SELECT
					mtdDocente.alu_id,
					mtdDocente.mtu_id,
					mtdDocente.mtd_id AS mtd_idDocente,
					tudDocente.tud_id AS tud_idDocente,
					mtdAluno.mtd_id AS mtd_idAluno,
					tudAluno.tud_id AS tud_idAluno,
					mtdDocente.mtd_dataMatricula AS mtd_dataMatriculaDocente,
					mtdDocente.mtd_dataSaida AS mtd_dataSaidaDocente,
					mtdAluno.mtd_dataMatricula AS mtd_dataMatriculaAluno,
					mtdAluno.mtd_dataSaida AS mtd_dataSaidaAluno
				FROM
					MTR_MatriculaTurmaDisciplina mtdDocente WITH(NOLOCK)
					INNER JOIN TUR_TurmaDisciplina tudDocente WITH(NOLOCK)
						ON tudDocente.tud_id = mtdDocente.tud_id
						AND tudDocente.tud_tipo = @tud_tipo
						AND tudDocente.tud_situacao <> 3
					INNER JOIN MTR_MatriculaTurmaDisciplina mtdAluno WITH(NOLOCK)
						ON mtdAluno.alu_id = mtdDocente.alu_id
						AND mtdAluno.mtu_id = mtdDocente.mtu_id
						AND mtdAluno.mtd_situacao IN (1,5)
					INNER JOIN TUR_TurmaDisciplina tudAluno WITH(NOLOCK)
						ON tudAluno.tud_id = mtdAluno.tud_id
						AND tudAluno.tud_tipo = 16
						AND tudAluno.tud_situacao <> 3
				WHERE 
					mtdDocente.tud_id = @tud_id
					AND mtdDocente.mtd_situacao IN (1,5)
				GROUP BY
					mtdDocente.alu_id,
					mtdDocente.mtu_id,
					mtdDocente.mtd_id,
					tudDocente.tud_id,
					mtdAluno.mtd_id,
					tudAluno.tud_id,
					mtdDocente.mtd_dataMatricula,
					mtdDocente.mtd_dataSaida,
					mtdAluno.mtd_dataMatricula,
					mtdAluno.mtd_dataSaida
			)

			, tbTurmaDisciplina AS
			(
				SELECT tud_idAluno AS tud_id
				FROM MatriculaTurmaDisciplinaMultisseriada
				GROUP BY tud_idAluno

				UNION 

				SELECT @tud_id AS tud_id
			)

			, tbFrequencia AS (
				SELECT 
					Taa.alu_id
					, Taa.mtu_id
					, MIN(ISNULL(taa_frequencia, 0)) frequencia
				FROM 
					CLS_TurmaAulaAluno Taa WITH(NOLOCK)
				INNER JOIN CLS_TurmaAula Tau WITH(NOLOCK)
					ON Tau.tud_id = Taa.tud_id
					AND Tau.tau_id = Taa.tau_id
					AND Tau.tpc_id = @tpc_id
					AND Tau.tau_situacao <> 3
				INNER JOIN tbTurmaDisciplina tud
					ON Tau.tud_id = tud.tud_id
				LEFT JOIN ACA_AlunoJustificativaFalta afj WITH(NOLOCK)
					ON Taa.alu_id = afj.alu_id
					AND afj.afj_situacao <> 3
					AND tau_data >= afj.afj_dataInicio
					AND ((afj.afj_dataFim  IS NULL) OR (tau_data <= afj.afj_dataFim))
				LEFT JOIN ACA_TipoJustificativaFalta tjf WITH(NOLOCK)
					ON tjf.tjf_id = afj.tjf_id
					AND tjf.tjf_situacao <> 3
				WHERE 
					Taa.taa_situacao <> 3
					AND ((tjf.tjf_abonaFalta IS NULL) OR (tjf.tjf_abonaFalta = 0))
				GROUP BY
					Tau.tau_data
					, Taa.alu_id
					, Taa.mtu_id
					, Taa.mtd_id
			)

			, tbQtdes AS
			(
				SELECT
				Mtd.alu_id
				, Mtd.mtu_id
				, Mtd.mtd_idAluno
				,
				-- Qt faltas do aluno.
				(
					SELECT SUM(Freq.frequencia)
					FROM tbFrequencia Freq
					WHERE 
						Freq.alu_id = Mtd.alu_id
						AND Freq.mtu_id = Mtd.mtu_id
				) AS QtFaltasAluno
				,
				-- Qt aulas do aluno.
				CASE  
					-- 1-Automático, 2-Manual
					WHEN @fav_calculoQtdeAulasDadas = 1 THEN NULL
					WHEN @tipoLancamento IN (1, 4) THEN -- 1-Aulas planejadas, 4-Aulas planejadas e mensal
						(SELECT TOP 1
							(dbo.FN_CalcularDiasUteis
							(CASE WHEN Cap.cap_dataInicio < Mtd.mtd_dataMatriculaAluno THEN Mtd.mtd_dataMatriculaAluno ELSE Cap.cap_dataInicio END
							, 
							CASE WHEN Cap.cap_dataFim > Mtd.mtd_dataSaidaAluno THEN Mtd.mtd_dataSaidaAluno ELSE Cap.cap_dataFim END
							, esc.ent_id, tur.cal_id)
							*
							CASE WHEN fav.fav_tipoApuracaoFrequencia = 1 -- Tempo de aula 
								THEN @crp_qtdeTemposDia 
								ELSE 1
							END)
						FROM TUR_Turma tur WITH(NOLOCK)
						INNER JOIN ACA_FormatoAvaliacao fav WITH(NOLOCK)
							ON  fav.fav_id = tur.fav_id
						INNER JOIN ESC_Escola esc WITH(NOLOCK)
							ON  esc.esc_id = tur.esc_id
						INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
							ON  Cap.cal_id = tur.cal_id
							AND Cap.tpc_id = @tpc_id
							AND Cap.cap_situacao <> 3
						WHERE 
							tur.tur_id = @tur_id
						)
					END AS QtAulasAluno
			FROM MatriculaTurmaDisciplinaMultisseriada Mtd 
			)

			INSERT INTO @TabelaQtdes(alu_id, mtu_id, mtd_id, qtFaltas, qtAulas)
			SELECT
				alu_id,
				mtu_id,
				mtd_idAluno,
				QtFaltasAluno,
				QtAulasAluno
			FROM tbQtdes
			GROUP BY 
				alu_id,
				mtu_id,
				mtd_idAluno,
				QtFaltasAluno,
				QtAulasAluno
		END
		ELSE
		BEGIN
			SELECT TOP 1 
				@crp_qtdeTemposDia = crp.crp_qtdeTemposDia, 
				@tur_id = turRtud.tur_id
			FROM TUR_TurmaRelTurmaDisciplina turRtud WITH(NOLOCK)
			INNER JOIN TUR_TurmaDisciplina AS tud WITH (NOLOCK)
				ON turRtud.tud_id = tud.tud_id
			INNER JOIN TUR_TurmaDisciplinaRelDisciplina disRtud WITH(NOLOCK)
				ON tud.tud_id = disRtud.tud_id
			INNER JOIN ACA_Disciplina dis WITH(NOLOCK)
				ON dis.dis_id = disRtud.dis_id
				AND dis.dis_situacao <> 3
			INNER JOIN TUR_TurmaCurriculo tcr WITH(NOLOCK)
				ON  tcr.tur_id = turRtud.tur_id
			INNER JOIN ACA_CurriculoPeriodo crp WITH(NOLOCK)
				ON crp.cur_id = tcr.cur_id
				AND crp.crr_id = tcr.crr_id
				AND crp.crp_id = tcr.crp_id
			WHERE 
				turRtud.tud_id = @tud_id
				AND tcr.tcr_situacao <> 3	

			; WITH tbFrequencia AS (
				SELECT 
					Taa.alu_id
					, Taa.mtu_id
					, Taa.mtd_id
					, MIN(ISNULL(taa_frequencia, 0)) frequencia
				FROM 
					CLS_TurmaAulaAluno Taa WITH(NOLOCK)
				INNER JOIN CLS_TurmaAula Tau WITH(NOLOCK)
					ON Tau.tud_id = Taa.tud_id
					AND Tau.tau_id = Taa.tau_id
					AND Tau.tau_situacao <> 3
				LEFT JOIN ACA_AlunoJustificativaFalta afj WITH(NOLOCK)
					ON Taa.alu_id = afj.alu_id
					AND afj.afj_situacao <> 3
					AND tau_data >= afj.afj_dataInicio
					AND ((afj.afj_dataFim  IS NULL) OR (tau_data <= afj.afj_dataFim))
				LEFT JOIN ACA_TipoJustificativaFalta tjf WITH(NOLOCK)
					ON tjf.tjf_id = afj.tjf_id
					AND tjf.tjf_situacao <> 3
				WHERE 
					Taa.taa_situacao <> 3
					AND Tau.tpc_id = @tpc_id
					AND Tau.tud_id = @tud_id
					AND ((tjf.tjf_abonaFalta IS NULL) OR (tjf.tjf_abonaFalta = 0))
				GROUP BY
					Tau.tau_data
					, Taa.alu_id
					, Taa.mtu_id
					, Taa.mtd_id
			)
		
			INSERT INTO @TabelaQtdes(alu_id, mtu_id, mtd_id, qtFaltas, qtAulas)
			SELECT
				Mtd.alu_id
				, Mtd.mtu_id
				, Mtd.mtd_id
				,
				-- Qt faltas do aluno.
				(
					SELECT SUM(Freq.frequencia)
					FROM tbFrequencia Freq
					WHERE 
						Freq.alu_id = Mtd.alu_id
						AND Freq.mtu_id = Mtd.mtu_id
						AND Freq.mtd_id = Mtd.mtd_id
				) AS QtFaltasAluno
				,
				-- Qt aulas do aluno.
				CASE  
					-- 1-Automático, 2-Manual
					WHEN @fav_calculoQtdeAulasDadas = 1 THEN NULL
					WHEN @tipoLancamento IN (1, 4) THEN -- 1-Aulas planejadas, 4-Aulas planejadas e mensal
						(SELECT TOP 1
							(dbo.FN_CalcularDiasUteis
							(CASE WHEN Cap.cap_dataInicio < Mtd.mtd_dataMatricula THEN Mtd.mtd_dataMatricula ELSE Cap.cap_dataInicio END
							, 
							CASE WHEN Cap.cap_dataFim > Mtd.mtd_dataSaida THEN Mtd.mtd_dataSaida ELSE Cap.cap_dataFim END
							, esc.ent_id, tur.cal_id)
							*
							CASE WHEN fav.fav_tipoApuracaoFrequencia = 1 -- Tempo de aula 
								THEN @crp_qtdeTemposDia 
								ELSE 1
							END)
						FROM TUR_Turma tur WITH(NOLOCK)
						INNER JOIN ACA_FormatoAvaliacao fav WITH(NOLOCK)
							ON  fav.fav_id = tur.fav_id
						INNER JOIN ESC_Escola esc WITH(NOLOCK)
							ON  esc.esc_id = tur.esc_id
						INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
							ON  Cap.cal_id = tur.cal_id
							AND Cap.tpc_id = @tpc_id
							AND Cap.cap_situacao <> 3
						WHERE 
							tur.tur_id = @tur_id
						)
					END AS QtAulasAluno
			FROM MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
			WHERE 
				(Mtd.tud_id = @tud_id)
				AND mtd_situacao IN (1,5)
		END
	END
	ELSE IF(@tipoLancamento = 5) -- 5-Aulas Dadas
	BEGIN
		IF (@tud_tipo = 15)
		BEGIN
			;WITH MatriculaTurmaDisciplinaMultisseriada AS
			(
				SELECT
					mtdDocente.alu_id,
					mtdDocente.mtu_id,
					mtdDocente.mtd_id AS mtd_idDocente,
					tudDocente.tud_id AS tud_idDocente,
					mtdAluno.mtd_id AS mtd_idAluno,
					tudAluno.tud_id AS tud_idAluno,
					mtdDocente.mtd_dataMatricula,
					mtdDocente.mtd_dataSaida
				FROM
					MTR_MatriculaTurmaDisciplina mtdDocente WITH(NOLOCK)
					INNER JOIN TUR_TurmaDisciplina tudDocente WITH(NOLOCK)
						ON tudDocente.tud_id = mtdDocente.tud_id
						AND tudDocente.tud_tipo = @tud_tipo
						AND tudDocente.tud_situacao <> 3
					INNER JOIN MTR_MatriculaTurmaDisciplina mtdAluno WITH(NOLOCK)
						ON mtdAluno.alu_id = mtdDocente.alu_id
						AND mtdAluno.mtu_id = mtdDocente.mtu_id
						AND mtdAluno.mtd_situacao IN (1,5)
					INNER JOIN TUR_TurmaDisciplina tudAluno WITH(NOLOCK)
						ON tudAluno.tud_id = mtdAluno.tud_id
						AND tudAluno.tud_tipo = 16
						AND tudAluno.tud_situacao <> 3
				WHERE 
					mtdDocente.tud_id = @tud_id
					AND mtdDocente.mtd_situacao IN (1,5)
				GROUP BY
					mtdDocente.alu_id,
					mtdDocente.mtu_id,
					mtdDocente.mtd_id,
					tudDocente.tud_id,
					mtdAluno.mtd_id,
					tudAluno.tud_id,
					mtdDocente.mtd_dataMatricula,
					mtdDocente.mtd_dataSaida
			)

			-- Insere todas as matrículas daquele aluno dentro do bimestre.
			INSERT INTO @tbAlunos
			(alu_id, mtu_id, mtd_id, tud_id, mtd_dataMatricula, mtd_dataSaida, tud_tipo, tud_idAluno, mtu_idOrigem, mtd_idOrigem)
			SELECT
				tdm.alu_id
				, tdm.mtu_id
				, tdm.mtd_idDocente
				, tdm.tud_idDocente
				, tdm.mtd_dataMatricula
				-- Traz a data de fim do bimestre caso não tenha saída (para filtrar as aulas).
				, ISNULL(tdm.mtd_dataSaida, DATEADD(DAY, 1, Cap.cap_dataFim)) AS mtd_dataSaida
				, @tud_tipo
				, tdm.tud_idAluno AS tud_idAluno
				, tdm.mtu_id, tdm.mtd_idAluno -- Matrícula do @tud_id
			FROM
				MatriculaTurmaDisciplinaMultisseriada tdm
				INNER JOIN TUR_TurmaRelTurmaDisciplina RelTud WITH(NOLOCK)
					ON RelTud.tud_id = tdm.tud_idDocente
				INNER JOIN TUR_Turma Tur WITH(NOLOCK)
					ON Tur.tur_id = RelTud.tur_id
				INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
					ON Cap.cal_id = Tur.cal_id
					AND Cap.tpc_id = @tpc_id
					AND Cap.cap_situacao <> 3
				
		END
		ELSE IF (@tud_tipo = 10)
		BEGIN
			-- para disciplinas eletivas desconsidera bimestres anteriores
			-- como a busca por matrículas em bimestres anteriores é feita por tipo de disciplina
			-- e as disciplinas eletivas no mesmo bimestre também possuem o mesmo tipo de disciplina
			-- a lógica para buscar bimestres anteriores quebra a regra para o bimestre corrente.
			
			-- Insere todas as matrículas daquele aluno dentro do bimestre.
			INSERT INTO @tbAlunos
			(alu_id, mtu_id, mtd_id, tud_id, mtd_dataMatricula, mtd_dataSaida, tud_tipo, tud_idRegencia, mtu_idOrigem, mtd_idOrigem)
			SELECT
				Mtd.alu_id
				, Mtd.mtu_id
				, Mtd.mtd_id
				, Mtd.tud_id
				, Mtd.mtd_dataMatricula
				-- Traz a data de fim do bimestre caso não tenha saída (para filtrar as aulas).
				, ISNULL(Mtd.mtd_dataSaida, DATEADD(DAY, 1, Cap.cap_dataFim)) AS mtd_dataSaida
				, tud.tud_tipo
				, 
				NULL AS tud_idRegencia
				, Mtd.mtu_id, Mtd.mtd_id -- Matrícula do @tud_id
			FROM MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
			INNER JOIN TUR_TurmaRelTurmaDisciplina RelTud WITH(NOLOCK)
				ON RelTud.tud_id = Mtd.tud_id
			INNER JOIN TUR_Turma Tur WITH(NOLOCK)
				ON Tur.tur_id = RelTud.tur_id
			INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
				ON Cap.cal_id = Tur.cal_id
				AND Cap.tpc_id = @tpc_id
				AND Cap.cap_situacao <> 3
			INNER JOIN TUR_TurmaDisciplina tud WITH(NOLOCK)
				ON Mtd.tud_id = tud.tud_id
				AND tud.tud_situacao <> 3
			WHERE
				Mtd.tud_id = @tud_id
				AND Mtd.mtd_situacao <> 3
		END 
		ELSE 
		BEGIN
			-- Insere todas as matrículas daquele aluno dentro do bimestre.
			INSERT INTO @tbAlunos
			(alu_id, mtu_id, mtd_id, tud_id, mtd_dataMatricula, mtd_dataSaida, tud_tipo, tud_idRegencia, mtu_idOrigem, mtd_idOrigem)
			SELECT
				MtdRelacionada.alu_id
				, MtdRelacionada.mtu_id
				, MtdRelacionada.mtd_id
				, MtdRelacionada.tud_id
				, MtdRelacionada.mtd_dataMatricula
				-- Traz a data de fim do bimestre caso não tenha saída (para filtrar as aulas).
				, ISNULL(MtdRelacionada.mtd_dataSaida, DATEADD(DAY, 1, Cap.cap_dataFim)) AS mtd_dataSaida
				, TudRelacionada.tud_tipo
				, 
				(
					-- Quando a disciplina for complementação da regência, trazer o tud_id da regência.
					SELECT TudRegencia.tud_id
					FROM TUR_TurmaRelTurmaDisciplina RelTudComplRegencia WITH(NOLOCK)
					INNER JOIN TUR_TurmaRelTurmaDisciplina RelTudRegencia WITH(NOLOCK)
						ON RelTudRegencia.tur_id = RelTudComplRegencia.tur_id
					INNER JOIN TUR_TurmaDisciplina TudRegencia WITH(NOLOCK)
						ON TudRegencia.tud_id = RelTudRegencia.tud_id
						AND TudRegencia.tud_situacao <> 3
						AND TudRegencia.tud_tipo = 11 -- 11-Regência
					WHERE
						RelTudComplRegencia.tud_id = MtdRelacionada.tud_id
						AND TudRelacionada.tud_tipo = 13 -- 13-Complementação de regência (trazer o tud_id da regência)
				) AS tud_idRegencia
				, Mtd.mtu_id, Mtd.mtd_id -- Matrícula do @tud_id
			FROM MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
			INNER JOIN TUR_TurmaRelTurmaDisciplina RelTud WITH(NOLOCK)
				ON RelTud.tud_id = Mtd.tud_id
			INNER JOIN TUR_Turma Tur WITH(NOLOCK)
				ON Tur.tur_id = RelTud.tur_id
			INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
				ON Cap.cal_id = Tur.cal_id
				AND Cap.tpc_id = @tpc_id
				AND Cap.cap_situacao <> 3
			INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDis WITH(NOLOCK)
				ON RelDis.tud_id = Mtd.tud_id
			INNER JOIN ACA_Disciplina Dis WITH(NOLOCK)
				ON Dis.dis_id = RelDis.dis_id
				AND Dis.dis_situacao <> 3
			INNER JOIN ACA_Disciplina DisRelacionada WITH(NOLOCK)
				ON DisRelacionada.tds_id = Dis.tds_id
				AND Dis.dis_situacao <> 3
			INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDisRelacionada WITH(NOLOCK)
				ON RelDisRelacionada.dis_id = DisRelacionada.dis_id
			INNER JOIN TUR_TurmaDisciplina TudRelacionada WITH(NOLOCK)
				ON TudRelacionada.tud_id = RelDisRelacionada.tud_id
				AND TudRelacionada.tud_situacao <> 3
			--
			INNER JOIN TUR_TurmaRelTurmaDisciplina RelTurmaDisciplina WITH(NOLOCK)
				ON RelTurmaDisciplina.tud_id = TudRelacionada.tud_id
			INNER JOIN TUR_Turma TurRelacionada WITH(NOLOCK)
				ON TurRelacionada.tur_id = RelTurmaDisciplina.tur_id
				AND TurRelacionada.cal_id = @cal_id	
				AND TurRelacionada.tur_situacao <> 3	
			--		
			INNER JOIN MTR_MatriculaTurmaDisciplina MtdRelacionada WITH(NOLOCK)
				ON MtdRelacionada.alu_id = Mtd.alu_id
				AND MtdRelacionada.tud_id = TudRelacionada.tud_id
				AND MtdRelacionada.mtd_situacao IN (1,5)
				-- Só matrículas dentro do bimestre.
				AND Mtd.mtd_dataMatricula <= Cap.cap_dataFim
				AND (Mtd.mtd_situacao = 1 OR Mtd.mtd_dataSaida > Cap.cap_dataInicio)
			WHERE
				Mtd.tud_id = @tud_id
				AND Mtd.mtd_situacao <> 3
		END

		IF (@tipoDocente = 5)
		BEGIN
			INSERT INTO @tbPosicaoDocente (tdc_posicao)
			SELECT tdc_posicao 
			FROM ACA_TipoDocente TDC WITH(NOLOCK)
			WHERE
				TDC.tdc_id = 5 
				AND TDC.tdc_situacao <> 3
		END
		ELSE
		BEGIN
			INSERT INTO @tbPosicaoDocente (tdc_posicao)
			SELECT tdc_posicao 
			FROM ACA_TipoDocente TDC WITH(NOLOCK)
			WHERE
				TDC.tdc_id IN (1,4,6)
				AND TDC.tdc_situacao <> 3
		END
		
		SELECT
			@qtdeTitulares = COUNT(tdt.tdt_id)
		FROM
			TUR_TurmaDocente tdt WITH(NOLOCK)
			INNER JOIN ACA_TipoDocente tdc WITH(NOLOCK)
				ON tdt.tdt_posicao = tdc.tdc_posicao
				AND tdc.tdc_id IN (1,6)
		WHERE
			tdt.tud_id = @tud_id
			AND tdt.tdt_situacao = 1
		
		IF (ISNULL(@tud_tipo, 0) = 11 AND @qtdeTitulares = 2)
		BEGIN
			;WITH tbQtdeAulaFalta AS
			(
				SELECT
					ROW_NUMBER() OVER (PARTITION BY Tau.tau_data, Mtr.alu_id, Mtr.mtu_id, Mtr.mtd_id ORDER BY Tau.tdt_posicao) AS numLinha,
					Mtr.alu_id,
					Mtr.mtu_id,
					Mtr.mtd_id,
					Tau.tau_data,
					Tau.tdt_posicao,
					SUM(ISNULL(Tau.tau_numeroAulas, 0)) AS qtAulas,
					SUM(ISNULL(Taa.taa_frequencia, 0)) AS qtFaltas
				FROM
					@tbAlunos Mtr
					LEFT JOIN CLS_TurmaAula Tau WITH(NOLOCK)
						ON Tau.tud_id = Mtr.tud_id
						AND Tau.tpc_id = @tpc_id
						AND Tau.tau_data >= Mtr.mtd_dataMatricula AND Tau.tau_data < Mtr.mtd_dataSaida
						AND Tau.tau_situacao <> 3
						AND Tau.tdt_posicao IN (SELECT tdc_posicao FROM @tbPosicaoDocente Tdc)
					LEFT JOIN CLS_TurmaAulaAluno Taa WITH(NOLOCK)
						ON Taa.tud_id = Tau.tud_id
						AND Taa.tau_id = Tau.tau_id
						AND Taa.alu_id = Mtr.alu_id
						AND Taa.mtu_id = Mtr.mtu_id
						AND Taa.mtd_id = Mtr.mtd_id
						AND Taa.taa_situacao <> 3
				GROUP BY
					Mtr.alu_id,
					Mtr.mtu_id,
					Mtr.mtd_id,
					Tau.tau_data,
					Tau.tdt_posicao
			) 
			
			, tbQtdeAula AS 
			(
				SELECT
					alu_id,
					mtu_id,
					mtd_id,
					SUM(ISNULL(qtAulas,0)) AS qtAulas
				FROM
					tbQtdeAulaFalta
				WHERE
					numLinha = 1
				GROUP BY
					alu_id,
					mtu_id,
					mtd_id
			)
			
			, tbQtdeFalta AS 
			(
				SELECT
					alu_id,
					mtu_id,
					mtd_id,
					CASE WHEN SUM(qtAulas) = SUM(qtFaltas) THEN 1 ELSE 0 END AS qtFaltas
				FROM
					tbQtdeAulaFalta
				GROUP BY
					alu_id,
					mtu_id,
					mtd_id,
					tau_data
			)
			
			INSERT INTO @TabelaQtdes
			(alu_id, mtu_id, mtd_id, qtAulas, qtFaltas)
			SELECT
				tqa.alu_id,
				tqa.mtu_id,
				tqa.mtd_id,
				tqa.qtAulas,
				SUM(tqf.qtFaltas) AS qtFaltas
			FROM
				tbQtdeAula tqa
				INNER JOIN tbQtdeFalta tqf
					ON tqa.alu_id = tqf.alu_id
					AND tqa.mtu_id = tqf.mtu_id
					AND tqa.mtd_id = tqf.mtd_id
			GROUP BY
				tqa.alu_id,
				tqa.mtu_id,
				tqa.mtd_id,
				tqa.qtAulas
		END
		ELSE
		IF (@tud_tipo = 15)
		BEGIN
			;WITH tbTurmaDisciplina AS (
				SELECT Mtr.tud_idAluno AS tud_id
				FROM @tbAlunos MTR
				GROUP BY Mtr.tud_idAluno

				UNION

				SELECT @tud_id AS tud_id
			)

			, tbFrequencia AS
			(
				SELECT
					Mtr.alu_id, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
					, ISNULL(SUM(Tau.tau_numeroAulas), 0) AS QtAulas
					, SUM(ISNULL(Taa.taa_frequencia, 0)) AS QtFaltas
					--, Tau.*
				FROM @tbAlunos Mtr
				INNER JOIN MTR_MatriculaTurmaDisciplina mtd WITH(NOLOCK)
					ON mtr.alu_id = mtd.alu_id
					AND mtr.mtu_id = mtd.mtu_id
					AND mtr.mtd_id = mtd.mtd_id
					AND mtd.mtd_situacao <> 3
				INNER JOIN TUR_TurmaRelTurmaDisciplina RelTud WITH(NOLOCK)
					ON RelTud.tud_id = Mtd.tud_id
				INNER JOIN TUR_Turma Tur WITH(NOLOCK)
					ON Tur.tur_id = RelTud.tur_id
				INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
					ON Cap.cal_id = Tur.cal_id
					AND Cap.tpc_id = @tpc_id
					AND Cap.cap_situacao <> 3
				LEFT JOIN CLS_TurmaAula Tau WITH(NOLOCK)
					ON Tau.tud_id = mtd.tud_id
					AND Tau.tpc_id = @tpc_id
					AND Tau.tau_data >= mtd.mtd_dataMatricula AND Tau.tau_data < ISNULL(mtd.mtd_dataSaida, DATEADD(DAY, 1, Cap.cap_dataFim))
					AND Tau.tau_situacao <> 3
					AND Tau.tdt_posicao IN (SELECT tdc_posicao FROM @tbPosicaoDocente Tdc)
				LEFT JOIN CLS_TurmaAulaAluno Taa WITH(NOLOCK)
					ON Taa.tud_id = Tau.tud_id
					AND Taa.tau_id = Tau.tau_id
					AND Taa.alu_id = mtd.alu_id
					AND Taa.mtu_id = mtd.mtu_id
					AND Taa.mtd_id = mtd.mtd_id
					AND Taa.taa_situacao <> 3
				GROUP BY
					Mtr.alu_id, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem

				UNION

				SELECT
					Mtr.alu_id, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
					, ISNULL(SUM(Tau.tau_numeroAulas), 0) AS QtAulas
					, SUM(ISNULL(Taa.taa_frequencia, 0)) AS QtFaltas
					--, Tau.*
				FROM @tbAlunos Mtr
				INNER JOIN MTR_MatriculaTurmaDisciplina mtd WITH(NOLOCK)
					ON mtr.alu_id = mtd.alu_id
					AND mtr.mtu_id = mtd.mtu_id
					AND mtr.mtd_idOrigem = mtd.mtd_id
					AND mtd.mtd_situacao <> 3
				INNER JOIN TUR_TurmaRelTurmaDisciplina RelTud WITH(NOLOCK)
					ON RelTud.tud_id = Mtd.tud_id
				INNER JOIN TUR_Turma Tur WITH(NOLOCK)
					ON Tur.tur_id = RelTud.tur_id
				INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
					ON Cap.cal_id = Tur.cal_id
					AND Cap.tpc_id = @tpc_id
					AND Cap.cap_situacao <> 3
				LEFT JOIN CLS_TurmaAula Tau WITH(NOLOCK)
					ON Tau.tud_id = mtd.tud_id
					AND Tau.tpc_id = @tpc_id
					AND Tau.tau_data >= mtd.mtd_dataMatricula AND Tau.tau_data < ISNULL(mtd.mtd_dataSaida, DATEADD(DAY, 1, Cap.cap_dataFim))
					AND Tau.tau_situacao <> 3
					AND Tau.tdt_posicao IN (SELECT tdc_posicao FROM @tbPosicaoDocente Tdc)
				LEFT JOIN CLS_TurmaAulaAluno Taa WITH(NOLOCK)
					ON Taa.tud_id = Tau.tud_id
					AND Taa.tau_id = Tau.tau_id
					AND Taa.alu_id = mtd.alu_id
					AND Taa.mtu_id = mtd.mtu_id
					AND Taa.mtd_id = mtd.mtd_id
					AND Taa.taa_situacao <> 3
				GROUP BY
					Mtr.alu_id, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
			)

			INSERT INTO @TabelaQtdes
			(alu_id, mtu_id, mtd_id, qtAulas, qtFaltas)
			SELECT
				Mtr.alu_id, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
				, ISNULL(SUM(Mtr.QtAulas), 0) AS QtAulas
				, SUM(ISNULL(Mtr.QtFaltas, 0)) AS QtFaltas
				--, Tau.*
			FROM tbFrequencia Mtr
			GROUP BY Mtr.alu_id, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
		END
		ELSE
		BEGIN
			-- Aulas da disciplina do aluno.
			INSERT INTO @TabelaQtdes
			(alu_id, mtu_id, mtd_id, qtAulas, qtFaltas)
			SELECT
				Mtr.alu_id, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
				, ISNULL(SUM(Tau.tau_numeroAulas), 0) AS QtAulas
				, SUM(ISNULL(Taa.taa_frequencia, 0)) AS QtFaltas
				--, Tau.*
			FROM @tbAlunos Mtr
			LEFT JOIN CLS_TurmaAula Tau WITH(NOLOCK)
				ON Tau.tud_id = Mtr.tud_id
				AND Tau.tpc_id = @tpc_id
				AND Tau.tau_data >= Mtr.mtd_dataMatricula AND Tau.tau_data < Mtr.mtd_dataSaida
				AND Tau.tau_situacao <> 3
				AND Tau.tdt_posicao IN (SELECT tdc_posicao FROM @tbPosicaoDocente Tdc)
			LEFT JOIN CLS_TurmaAulaAluno Taa WITH(NOLOCK)
				ON Taa.tud_id = Tau.tud_id
				AND Taa.tau_id = Tau.tau_id
				AND Taa.alu_id = Mtr.alu_id
				AND Taa.mtu_id = Mtr.mtu_id
				AND Taa.mtd_id = Mtr.mtd_id
				AND Taa.taa_situacao <> 3
			WHERE
				Mtr.tud_tipo NOT IN (13, 16) -- 13-Complementação de regência 
											 -- 16-Multisseriada do aluno
			GROUP BY
				Mtr.alu_id, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
			
			; WITH MatriculasIngles AS
			(
				SELECT
					Mtr.alu_id
					, Mtr.mtu_id
					, Mtd.mtd_id -- Traz o mtd da disciplina Complementação de regência dele na turma.
					, Mtr.tud_id
					, Mtr.mtd_dataMatricula
					, Mtr.mtd_dataSaida
					, Mtr.tud_tipo, Mtr.tud_idRegencia, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
				FROM @tbAlunos Mtr
				INNER JOIN MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
					ON Mtd.alu_id = Mtr.alu_id
					AND Mtd.mtu_id = Mtr.mtu_id
					AND Mtd.tud_id = Mtr.tud_idRegencia
					AND Mtd.mtd_situacao <> 3
			)
			-- Aulas da disciplina Complementação de regência do aluno.
			INSERT INTO @TabelaQtdes
			(alu_id, mtu_id, mtd_id, qtAulas, qtFaltas)
			SELECT
				Mtr.alu_id, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
				, ISNULL(SUM(Tau.tau_numeroAulas), 0) AS QtAulas
				-- Quantidade de aulas do Inglês * quantidade de faltas da regência (no mesmo dia).
				, SUM(ISNULL(Tau.tau_numeroAulas, 0) * ISNULL(TaaRegencia.taa_frequencia, 0)) AS QtFaltas
				--, Tau.*
			FROM MatriculasIngles Mtr
			LEFT JOIN CLS_TurmaAula Tau WITH(NOLOCK)
				ON Tau.tud_id = Mtr.tud_id
				AND Tau.tpc_id = @tpc_id
				AND Tau.tau_data >= Mtr.mtd_dataMatricula AND Tau.tau_data < Mtr.mtd_dataSaida
				AND Tau.tau_situacao <> 3
				AND Tau.tdt_posicao IN (SELECT tdc_posicao FROM @tbPosicaoDocente Tdc)
			LEFT JOIN CLS_TurmaAula TauRegencia WITH(NOLOCK)
				ON TauRegencia.tud_id = Mtr.tud_idRegencia
				AND TauRegencia.tau_data = Tau.tau_data
				AND TauRegencia.tau_situacao <> 3
				AND TauRegencia.tdt_posicao IN (SELECT tdc_posicao FROM @tbPosicaoDocente Tdc)
			LEFT JOIN CLS_TurmaAulaAluno TaaRegencia WITH(NOLOCK)
				ON TaaRegencia.tud_id = TauRegencia.tud_id
				AND TaaRegencia.tau_id = TauRegencia.tau_id -- Busca a quantidade de faltas da regência.
				AND TaaRegencia.alu_id = Mtr.alu_id
				AND TaaRegencia.mtu_id = Mtr.mtu_id
				AND TaaRegencia.mtd_id = Mtr.mtd_id
				AND TaaRegencia.taa_situacao <> 3
			WHERE
				Mtr.tud_tipo = 13 -- 13-Complementação de regência
				AND NOT EXISTS (
					SELECT 1 
					FROM @TabelaQtdes Qtd 
					WHERE 
						Mtr.alu_id = Qtd.alu_id
						AND Mtr.mtu_idOrigem = Qtd.mtu_id
						AND Mtr.mtd_idOrigem = Qtd.mtd_id 
				)
			GROUP BY
				Mtr.alu_id, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
		END
	END
	ELSE IF(@tipoLancamento = 6) -- 6-Aulas previstas do docente
	BEGIN
		DECLARE @tpc_ordem INT;
		
		SELECT TOP 1
			@tpc_ordem = tpc.tpc_ordem
		FROM
			ACA_TipoPeriodoCalendario tpc WITH(NOLOCK)
		WHERE
			tpc.tpc_id = @tpc_id
			AND tpc.tpc_situacao <> 3
		
		IF (@tud_tipo = 15) --Gerando @tbAlunos multiseriada do docente
		BEGIN
			;WITH MatriculaTurmaDisciplinaMultisseriada AS
			(
				SELECT
					mtdDocente.alu_id,
					mtdDocente.mtu_id,
					mtdDocente.mtd_id AS mtd_idDocente,
					tudDocente.tud_id AS tud_idDocente,
					mtdAluno.mtd_id AS mtd_idAluno,
					tudAluno.tud_id AS tud_idAluno,
					mtdDocente.mtd_dataMatricula,
					mtdDocente.mtd_dataSaida
				FROM
					MTR_MatriculaTurmaDisciplina mtdDocente WITH(NOLOCK)
					INNER JOIN TUR_TurmaDisciplina tudDocente WITH(NOLOCK)
						ON tudDocente.tud_id = mtdDocente.tud_id
						AND tudDocente.tud_tipo = @tud_tipo
						AND tudDocente.tud_situacao <> 3
					INNER JOIN MTR_MatriculaTurmaDisciplina mtdAluno WITH(NOLOCK)
						ON mtdAluno.alu_id = mtdDocente.alu_id
						AND mtdAluno.mtu_id = mtdDocente.mtu_id
						AND mtdAluno.mtd_situacao IN (1,5)
					INNER JOIN TUR_TurmaDisciplina tudAluno WITH(NOLOCK)
						ON tudAluno.tud_id = mtdAluno.tud_id
						AND tudAluno.tud_tipo = 16
						AND tudAluno.tud_situacao <> 3
				WHERE 
					mtdDocente.tud_id = @tud_id
					AND mtdDocente.mtd_situacao IN (1,5)
			)

			-- Insere todas as matrículas daquele aluno dentro do bimestre.
			INSERT INTO @tbAlunos
			(alu_id, mtu_id, mtd_id, tud_id, mtd_dataMatricula, mtd_dataSaida, tud_tipo, tud_idAluno, mtu_idOrigem, mtd_idOrigem)
			SELECT
				tdm.alu_id
				, tdm.mtu_id
				, tdm.mtd_idDocente
				, tdm.tud_idDocente
				, tdm.mtd_dataMatricula
				-- Traz a data de fim do bimestre caso não tenha saída (para filtrar as aulas).
				, ISNULL(tdm.mtd_dataSaida, DATEADD(DAY, 1, Cap.cap_dataFim)) AS mtd_dataSaida
				, @tud_tipo
				, tdm.tud_idAluno AS tud_idAluno
				, tdm.mtu_id, tdm.mtd_idAluno -- Matrícula do @tud_id
			FROM
				MatriculaTurmaDisciplinaMultisseriada tdm
				INNER JOIN TUR_TurmaRelTurmaDisciplina RelTud WITH(NOLOCK)
					ON RelTud.tud_id = tdm.tud_idDocente
				INNER JOIN TUR_Turma Tur WITH(NOLOCK)
					ON Tur.tur_id = RelTud.tur_id
				INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
					ON Cap.cal_id = Tur.cal_id
					AND Cap.tpc_id = @tpc_id
					AND Cap.cap_situacao <> 3
			GROUP BY
				tdm.alu_id
				, tdm.mtu_id
				, tdm.mtd_idDocente
				, tdm.tud_idDocente
				, tdm.mtd_dataMatricula
				, ISNULL(tdm.mtd_dataSaida, DATEADD(DAY, 1, Cap.cap_dataFim))
				, tdm.tud_idAluno 
				, tdm.mtu_id
				, tdm.mtd_idAluno


		END
		ELSE IF (@tud_tipo = 10) --Gerando @tbAlunos para eletivas
		BEGIN
			-- para disciplinas eletivas desconsidera bimestres anteriores
			-- como a busca por matrículas em bimestres anteriores é feita por tipo de disciplina
			-- e as disciplinas eletivas no mesmo bimestre também possuem o mesmo tipo de disciplina
			-- a lógica para buscar bimestres anteriores quebra a regra para o bimestre corrente.
			
			-- Insere todas as matrículas daquele aluno dentro do bimestre.
			INSERT INTO @tbAlunos
			(alu_id, mtu_id, mtd_id, tud_id, mtd_dataMatricula, mtd_dataSaida, tud_tipo, tud_idRegencia, mtu_idOrigem, mtd_idOrigem)
			SELECT
				Mtd.alu_id
				, Mtd.mtu_id
				, Mtd.mtd_id
				, Mtd.tud_id
				, Mtd.mtd_dataMatricula
				-- Traz a data de fim do bimestre caso não tenha saída (para filtrar as aulas).
				, ISNULL(Mtd.mtd_dataSaida, DATEADD(DAY, 1, Cap.cap_dataFim)) AS mtd_dataSaida
				, tud.tud_tipo
				, 
				NULL AS tud_idRegencia
				, Mtd.mtu_id, Mtd.mtd_id -- Matrícula do @tud_id
			FROM MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
			INNER JOIN TUR_TurmaRelTurmaDisciplina RelTud WITH(NOLOCK)
				ON RelTud.tud_id = Mtd.tud_id
			INNER JOIN TUR_Turma Tur WITH(NOLOCK)
				ON Tur.tur_id = RelTud.tur_id
			INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
				ON Cap.cal_id = Tur.cal_id
				AND Cap.tpc_id = @tpc_id
				AND Cap.cap_situacao <> 3
			INNER JOIN TUR_TurmaDisciplina tud WITH(NOLOCK)
				ON Mtd.tud_id = tud.tud_id
				AND tud.tud_situacao <> 3
			WHERE
				Mtd.tud_id = @tud_id
				AND Mtd.mtd_situacao <> 3
		END 
		ELSE IF (@tud_tipo = 13) --Gerando @tbAlunos para complementação de regencia
		BEGIN
			-- Insere todas as matrículas daquele aluno dentro do bimestre.
			INSERT INTO @tbAlunos
			(alu_id, mtu_id, mtd_id, tud_id, mtd_dataMatricula, mtd_dataSaida, tud_tipo, tud_idRegencia, mtu_idOrigem, mtd_idOrigem)
			SELECT
				MtdRelacionada.alu_id
				, MtdRelacionada.mtu_id
				, MtdRelacionada.mtd_id
				, MtdRelacionada.tud_id
				, MtdRelacionada.mtd_dataMatricula
				-- Traz a data de fim do bimestre caso não tenha saída (para filtrar as aulas).
				, ISNULL(MtdRelacionada.mtd_dataSaida, DATEADD(DAY, 1, Cap.cap_dataFim)) AS mtd_dataSaida
				, TudRelacionada.tud_tipo
				, 
				(
					-- Quando a disciplina for complementação da regência, trazer o tud_id da regência.
					SELECT TudRegencia.tud_id
					FROM TUR_TurmaRelTurmaDisciplina RelTudComplRegencia WITH(NOLOCK)
					INNER JOIN TUR_TurmaRelTurmaDisciplina RelTudRegencia WITH(NOLOCK)
						ON RelTudRegencia.tur_id = RelTudComplRegencia.tur_id
					INNER JOIN TUR_TurmaDisciplina TudRegencia WITH(NOLOCK)
						ON TudRegencia.tud_id = RelTudRegencia.tud_id
						AND TudRegencia.tud_situacao <> 3
						AND TudRegencia.tud_tipo = 11 -- 11-Regência
					WHERE
						RelTudComplRegencia.tud_id = MtdRelacionada.tud_id
						AND TudRelacionada.tud_tipo = 13 -- 13-Complementação de regência (trazer o tud_id da regência)
				) AS tud_idRegencia
				, Mtd.mtu_id, Mtd.mtd_id -- Matrícula do @tud_id
			FROM MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
			INNER JOIN TUR_TurmaRelTurmaDisciplina RelTud WITH(NOLOCK)
				ON RelTud.tud_id = Mtd.tud_id
			INNER JOIN TUR_Turma Tur WITH(NOLOCK)
				ON Tur.tur_id = RelTud.tur_id
			INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
				ON Cap.cal_id = Tur.cal_id
				AND Cap.tpc_id = @tpc_id
				AND Cap.cap_situacao <> 3
			INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDis WITH(NOLOCK)
				ON RelDis.tud_id = Mtd.tud_id
			INNER JOIN ACA_Disciplina Dis WITH(NOLOCK)
				ON Dis.dis_id = RelDis.dis_id
				AND Dis.dis_situacao <> 3
			INNER JOIN ACA_Disciplina DisRelacionada WITH(NOLOCK)
				ON DisRelacionada.tds_id = Dis.tds_id
				AND Dis.dis_situacao <> 3
			INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDisRelacionada WITH(NOLOCK)
				ON RelDisRelacionada.dis_id = DisRelacionada.dis_id
			INNER JOIN TUR_TurmaDisciplina TudRelacionada WITH(NOLOCK)
				ON TudRelacionada.tud_id = RelDisRelacionada.tud_id
				AND TudRelacionada.tud_situacao <> 3
			--
			INNER JOIN TUR_TurmaRelTurmaDisciplina RelTurmaDisciplina WITH(NOLOCK)
				ON RelTurmaDisciplina.tud_id = TudRelacionada.tud_id
			INNER JOIN TUR_Turma TurRelacionada WITH(NOLOCK)
				ON TurRelacionada.tur_id = RelTurmaDisciplina.tur_id
				AND TurRelacionada.cal_id = @cal_id	
				AND TurRelacionada.tur_situacao <> 3	
			--		
			INNER JOIN MTR_MatriculaTurmaDisciplina MtdRelacionada WITH(NOLOCK)
				ON MtdRelacionada.alu_id = Mtd.alu_id
				AND MtdRelacionada.tud_id = TudRelacionada.tud_id
				AND MtdRelacionada.mtd_situacao IN (1,5)
				-- Só matrículas dentro do bimestre.
				AND Mtd.mtd_dataMatricula <= Cap.cap_dataFim
				AND (Mtd.mtd_situacao = 1 OR Mtd.mtd_dataSaida > Cap.cap_dataInicio)
			WHERE
				Mtd.tud_id = @tud_id
				AND Mtd.mtd_situacao <> 3
		END 
		ELSE --Gerando @tbAlunos para todos os outros tipos de disciplinas (exceto multiseriada do docente, eletivas)
		BEGIN
			-- Insere todas as matrículas daquele aluno dentro do bimestre.
			INSERT INTO @tbAlunos
			(alu_id, mtu_id, mtd_id, tud_id, mtd_dataMatricula, mtd_dataSaida, tud_tipo, tud_idRegencia, mtu_idOrigem, mtd_idOrigem)
			SELECT
				MtdRelacionada.alu_id
				, MtdRelacionada.mtu_id
				, MtdRelacionada.mtd_id
				, MtdRelacionada.tud_id
				, MtdRelacionada.mtd_dataMatricula
				-- Traz a data de fim do bimestre caso não tenha saída (para filtrar as aulas).
				, ISNULL(MtdRelacionada.mtd_dataSaida, DATEADD(DAY, 1, Cap.cap_dataFim)) AS mtd_dataSaida
				, TudRelacionada.tud_tipo
				, NULL AS tud_idRegencia
				, Mtd.mtu_id, Mtd.mtd_id -- Matrícula do @tud_id
			FROM MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
			INNER JOIN TUR_TurmaRelTurmaDisciplina RelTud WITH(NOLOCK)
				ON RelTud.tud_id = Mtd.tud_id
			INNER JOIN TUR_Turma Tur WITH(NOLOCK)
				ON Tur.tur_id = RelTud.tur_id
			INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
				ON Cap.cal_id = Tur.cal_id
				AND Cap.tpc_id = @tpc_id
				AND Cap.cap_situacao <> 3
			INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDis WITH(NOLOCK)
				ON RelDis.tud_id = Mtd.tud_id
			INNER JOIN ACA_Disciplina Dis WITH(NOLOCK)
				ON Dis.dis_id = RelDis.dis_id
				AND Dis.dis_situacao <> 3
			INNER JOIN ACA_Disciplina DisRelacionada WITH(NOLOCK)
				ON DisRelacionada.tds_id = Dis.tds_id
				AND Dis.dis_situacao <> 3
			INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDisRelacionada WITH(NOLOCK)
				ON RelDisRelacionada.dis_id = DisRelacionada.dis_id
			INNER JOIN TUR_TurmaDisciplina TudRelacionada WITH(NOLOCK)
				ON TudRelacionada.tud_id = RelDisRelacionada.tud_id
				AND TudRelacionada.tud_situacao <> 3
			--
			INNER JOIN TUR_TurmaRelTurmaDisciplina RelTurmaDisciplina WITH(NOLOCK)
				ON RelTurmaDisciplina.tud_id = TudRelacionada.tud_id
			INNER JOIN TUR_Turma TurRelacionada WITH(NOLOCK)
				ON TurRelacionada.tur_id = RelTurmaDisciplina.tur_id
				AND TurRelacionada.cal_id = @cal_id	
				AND TurRelacionada.tur_situacao <> 3	
			--		
			INNER JOIN MTR_MatriculaTurmaDisciplina MtdRelacionada WITH(NOLOCK)
				ON MtdRelacionada.alu_id = Mtd.alu_id
				AND MtdRelacionada.tud_id = TudRelacionada.tud_id
				AND MtdRelacionada.mtd_situacao IN (1,5)
				-- Só matrículas dentro do bimestre.
				AND Mtd.mtd_dataMatricula <= Cap.cap_dataFim
				AND (Mtd.mtd_situacao = 1 OR Mtd.mtd_dataSaida > Cap.cap_dataInicio)
			WHERE
				Mtd.tud_id = @tud_id
				AND Mtd.mtd_situacao <> 3
		
		END --FIM DO IF que preenchia a @tbAlunos
		
		IF (@tipoDocente = 5)
		BEGIN
			INSERT INTO @tbPosicaoDocente (tdc_posicao)
			SELECT tdc_posicao 
			FROM ACA_TipoDocente TDC WITH(NOLOCK)
			WHERE
				TDC.tdc_id = 5 
				AND TDC.tdc_situacao <> 3
		END
		ELSE
		BEGIN
			INSERT INTO @tbPosicaoDocente (tdc_posicao)
			SELECT tdc_posicao 
			FROM ACA_TipoDocente TDC WITH(NOLOCK)
			WHERE
				TDC.tdc_id IN (1,4,6)
				AND TDC.tdc_situacao <> 3
		END
		
		SELECT
			@qtdeTitulares = COUNT(tdt.tdt_id)
		FROM
			TUR_TurmaDocente tdt WITH(NOLOCK)
			INNER JOIN ACA_TipoDocente tdc WITH(NOLOCK)
				ON tdt.tdt_posicao = tdc.tdc_posicao
				AND tdc.tdc_id IN (1,6)
		WHERE
			tdt.tud_id = @tud_id
			AND tdt.tdt_situacao = 1

		DECLARE @qtdeAulas INT;
		
		-- Pedro Silva 29/07/2015
		-- a quantidade de aulas ( @qtdeAulas )para este tipo de lançamento de frequência é calculado pelas aulas previstas
		-- porém, as quantidades de aulas normais e de reposição que serão calculadas mais abaixo baseada na CLS_TurmaAula mesmo
		
		IF (@tud_tipo = 15)
		BEGIN
			;WITH tbTurmaDisciplina AS (
				SELECT Mtr.tud_idAluno AS tud_id
				FROM @tbAlunos MTR
				GROUP BY Mtr.tud_idAluno

				UNION

				SELECT @tud_id AS tud_id
			)

			SELECT TOP 1
				@qtdeAulas = SUM(tap.tap_aulasPrevitas)
			FROM
				TUR_TurmaDisciplinaAulaPrevista tap WITH(NOLOCK)
				INNER JOIN tbTurmaDisciplina tud
					ON tap.tud_id = tud.tud_id
			WHERE
				 tap.tpc_id = @tpc_id
				AND tap.tap_situacao <> 3
		END
		ELSE
		BEGIN
			SELECT TOP 1
				@qtdeAulas = tap.tap_aulasPrevitas
			FROM
				TUR_TurmaDisciplinaAulaPrevista tap WITH(NOLOCK)
			WHERE tap.tud_id = @tud_id
			  AND tap.tpc_id = @tpc_id
			  AND tap.tap_situacao <> 3
		END
		
		IF (ISNULL(@tud_tipo, 0) = 11 AND @qtdeTitulares = 2) -- IF que vai preencher a @TabelaQtdes para regência com dois titulares
		BEGIN
			DECLARE @tbQtdeFalta TABLE 
			(
				alu_id BIGINT,
				mtu_id INT,
				mtd_id INT,
				tau_reposicao BIT,
				qtFaltas INT
			)
			
			DECLARE @tbQtdeFalta_SemAcumularReposicao TABLE 
			(
				alu_id BIGINT,
				mtu_id INT,
				mtd_id INT,
				tau_reposicao BIT,
				qtFaltas INT
			)
			
			DECLARE @tbQtdeAulas TABLE 
			(
				alu_id BIGINT,
				mtu_id INT,
				mtd_id INT,
				tau_reposicao BIT,
				qtAulas INT
			)
		
			DECLARE @tbQtdeAulaFalta TABLE
			(
				alu_id BIGINT,
				mtu_id INT,
				mtd_id INT,
				tau_data DATE,
				tdt_posicao INT,
				tau_reposicao INT,
				tpc_id INT,
				tpc_ordem INT,
				qtAulas INT,
				qtFaltas INT
			)
			
			INSERT INTO @tbQtdeAulaFalta
				SELECT
					Mtr.alu_id,
					Mtr.mtu_id,
					Mtr.mtd_id,
					Tau.tau_data,
					Tau.tdt_posicao,
					Tau.tau_reposicao,
					Tau.tpc_id,
					ISNULL(tpc.tpc_ordem, 0) AS tpc_ordem,
					SUM(ISNULL(Tau.tau_numeroAulas,0)) AS qtAulas,
					SUM(ISNULL(Taa.taa_frequencia, 0)) AS qtFaltas
				FROM
					@tbAlunos Mtr
					LEFT JOIN CLS_TurmaAula Tau WITH(NOLOCK)
						ON Tau.tud_id = Mtr.tud_id
						AND Tau.tau_data >= Mtr.mtd_dataMatricula AND Tau.tau_data < Mtr.mtd_dataSaida
						AND Tau.tau_situacao <> 3
						AND Tau.tdt_posicao IN (SELECT tdc_posicao FROM @tbPosicaoDocente Tdc)
					LEFT JOIN CLS_TurmaAulaAluno Taa WITH(NOLOCK)
						ON Taa.tud_id = Tau.tud_id
						AND Tau.tau_id = Taa.tau_id
						AND Taa.alu_id = Mtr.alu_id
						AND Taa.mtu_id = Mtr.mtu_id
						AND Taa.mtd_id = Mtr.mtd_id
						AND Taa.taa_situacao <> 3
					LEFT JOIN ACA_TipoPeriodoCalendario tpc WITH(NOLOCK)
						ON tpc.tpc_id = Tau.tpc_id
						AND tpc.tpc_situacao <> 3
				GROUP BY
					Mtr.alu_id,
					Mtr.mtu_id,
					Mtr.mtd_id,
					Tau.tau_data,
					Tau.tau_reposicao,
					Tau.tdt_posicao,
					Tau.tpc_id,
					tpc.tpc_ordem
			
			----- INICIO FALTAS REGÊNCIA 2 PROFs ------
			INSERT INTO @tbQtdeFalta 
			(
				alu_id,
				mtu_id,
				mtd_id,
				tau_reposicao,
				qtFaltas
			) 
			SELECT
				alu_id,
				mtu_id,
				mtd_id,
				0 AS tau_reposicao,
				CASE WHEN SUM(qtAulas) = SUM(qtFaltas) THEN 1 ELSE 0 END AS qtFaltas
			FROM
				@tbQtdeAulaFalta 
			WHERE
				tpc_id = @tpc_id
			GROUP BY
				alu_id,
				mtu_id,
				mtd_id,
				tau_data
			HAVING
				SUM(CAST(tau_reposicao AS INT)) <> SUM(qtAulas)
				
			UNION ALL
			
			SELECT
				alu_id,
				mtu_id,
				mtd_id,
				1 AS tau_reposicao,
				CASE WHEN SUM(qtAulas) = SUM(qtFaltas) THEN 1 ELSE 0 END AS qtFaltas
			FROM
				@tbQtdeAulaFalta
			WHERE
				tpc_ordem <= @tpc_ordem 
			GROUP BY
				alu_id,
				mtu_id,
				mtd_id,
				tau_data
			HAVING
				SUM(CAST(tau_reposicao AS INT)) = SUM(qtAulas)
			
			INSERT INTO @tbQtdeFalta_SemAcumularReposicao
			(
				alu_id,
				mtu_id,
				mtd_id,
				tau_reposicao,
				qtFaltas
			) 
			SELECT
				alu_id,
				mtu_id,
				mtd_id,
				1 AS tau_reposicao,
				CASE WHEN SUM(qtAulas) = SUM(qtFaltas) THEN 1 ELSE 0 END AS qtFaltas
			FROM
				@tbQtdeAulaFalta
			WHERE
				tpc_id = @tpc_id
			GROUP BY
				alu_id,
				mtu_id,
				mtd_id,
				tau_data
			HAVING
				SUM(CAST(tau_reposicao AS INT)) = SUM(qtAulas)
			
			INSERT INTO @TabelaQtdes
			(alu_id, mtu_id, mtd_id, qtAulas, qtFaltas)
			SELECT
				tqf.alu_id,
				tqf.mtu_id,
				tqf.mtd_id,
				@qtdeAulas AS qtAulas,
				SUM(tqf.qtFaltas) AS qtFaltas
			FROM
				@tbQtdeFalta tqf
			WHERE
				tau_reposicao = 0
			GROUP BY
				tqf.alu_id,
				tqf.mtu_id,
				tqf.mtd_id
				
			;WITH tbFaltasReposicao AS
			(
				SELECT
					tqf.alu_id,
					tqf.mtu_id,
					tqf.mtd_id,
					SUM(tqf.qtFaltas) AS faltas
				FROM
					@tbQtdeFalta tqf
				WHERE
					tqf.tau_reposicao = 1
				GROUP BY
					tqf.alu_id,
					tqf.mtu_id,
					tqf.mtd_id
			)	
				
			UPDATE tbq
			SET tbq.qtFaltasReposicao = tfr.faltas
			FROM
				@TabelaQtdes tbq
				INNER JOIN tbFaltasReposicao tfr
					ON tbq.alu_id = tfr.alu_id
					AND tbq.mtu_id = tfr.mtu_id
					AND tbq.mtd_id = tfr.mtd_id
					
			--repetido o bloco de cima para as faltas de reposição sem acumular
			;WITH tbFaltasReposicaoSemAcumular AS
			(
				SELECT
					tqf.alu_id,
					tqf.mtu_id,
					tqf.mtd_id,
					SUM(tqf.qtFaltas) AS faltas
				FROM
					@tbQtdeFalta_SemAcumularReposicao tqf
				WHERE
					tqf.tau_reposicao = 1
				GROUP BY
					tqf.alu_id,
					tqf.mtu_id,
					tqf.mtd_id
			)	
				
			UPDATE tbq
			SET tbq.qtFaltasReposicaoNaoAcumuladas = tfr.faltas
			FROM
				@TabelaQtdes tbq
				INNER JOIN tbFaltasReposicaoSemAcumular tfr
					ON tbq.alu_id = tfr.alu_id
					AND tbq.mtu_id = tfr.mtu_id
					AND tbq.mtd_id = tfr.mtd_id
					
			----- FIM FALTAS REGÊNCIA 2 PROFs ------
			----- INICIO AULAS REGÊNCIA 2 PROFs ------
			
			INSERT INTO @tbQtdeAulas 
			(
				alu_id,
				mtu_id,
				mtd_id,
				tau_reposicao,
				qtAulas
			) 
			SELECT
				alu_id,
				mtu_id,
				mtd_id,
				tau_reposicao,
				CASE WHEN SUM(qtAulas) > 2 THEN 2 WHEN SUM(qtAulas) = 0 THEN 0 ELSE 1 END AS qtAulas -- 0->0, 1e2->1, Maior que 2->2
			FROM
				@tbQtdeAulaFalta 
			WHERE tpc_id = @tpc_id
			GROUP BY
				alu_id,
				mtu_id,
				mtd_id,
				tau_reposicao,
				tau_data
				
			UPDATE tbq
			   SET tbq.qtAulasReposicao = qa.qtAulasReposicao
				  ,tbq.qtAulasNormais = qa.qtAulasNormais
			  FROM @TabelaQtdes tbq
				   inner join (	select qa.alu_id, qa.mtu_id, qa.mtd_id
									 , sum(case qa.tau_reposicao when 1 then qa.qtAulas else 0 end) as qtAulasReposicao
									 , sum(case qa.tau_reposicao when 0 then qa.qtAulas else 1 end) as qtAulasNormais
								  from @tbQtdeAulas qa 
								 group by qa.alu_id, qa.mtu_id, qa.mtd_id) qa
						   on qa.alu_id = tbq.alu_id
						  and qa.mtu_id = tbq.mtu_id 
						  and qa.mtd_id = tbq.mtd_id
						  
			----- FIM AULAS REGÊNCIA 2 PROFs------
		END
		ELSE IF (@tud_tipo = 15) -- IF que vai preencher a @TabelaQtdes para multiseriada do docente
		BEGIN
			DECLARE @tbFrequencia TABLE 
			( alu_id bigint
			 , mtu_idOrigem int
			 , mtd_idOrigem int
			 , tpc_id int
			 , tpc_ordem int
			 , taa_frequencia int
			 , tau_reposicao int)
			 
			 DECLARE @tbFrequenciaSemAcumularReposicao TABLE 
			( alu_id bigint
			 , mtu_idOrigem int
			 , mtd_idOrigem int
			 , tpc_id int
			 , tpc_ordem int
			 , taa_frequencia int
			 , tau_reposicao int)
			 
			;WITH tbTurmaDisciplina AS (
				SELECT Mtr.tud_idAluno AS tud_id
				FROM @tbAlunos MTR
				GROUP BY Mtr.tud_idAluno

				UNION

				SELECT @tud_id AS tud_id
			)
			
			insert into @tbTurmaAula
				SELECT
					Tau.tau_id,
					Tau.tud_id,
					Tau.tau_data,
					Tau.tau_reposicao,
					Tau.tpc_id,
					ISNULL(tpc.tpc_ordem, 0) AS tpc_ordem
				FROM
					CLS_TurmaAula Tau WITH(NOLOCK)
					INNER JOIN tbTurmaDisciplina tud
						ON Tau.tud_id = tud.tud_id
					INNER JOIN @tbPosicaoDocente Tdc
						ON Tau.tdt_posicao = Tdc.tdc_posicao
					LEFT JOIN ACA_TipoPeriodoCalendario tpc WITH(NOLOCK)
						ON tpc.tpc_id = Tau.tpc_id
						AND tpc.tpc_situacao <> 3
				WHERE
					Tau.tau_situacao <> 3
				GROUP BY
					Tau.tau_id,
					Tau.tud_id,
					Tau.tau_data,
					Tau.tau_reposicao,
					Tau.tpc_id,
					tpc.tpc_ordem

			insert into @tbFrequencia 
			(alu_id, mtu_idOrigem, mtd_idOrigem, tpc_id, tpc_ordem, taa_frequencia, tau_reposicao)
				SELECT
					mtd.alu_id
					, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
					,tau.tpc_id
					,ISNULL(tpc.tpc_ordem, 0) AS tpc_ordem
					,Taa.taa_frequencia
					,Tau.tau_reposicao
				FROM @tbAlunos Mtr
				INNER JOIN MTR_MatriculaTurmaDisciplina mtd
					ON mtd.alu_id = Mtr.alu_id
					AND mtd.mtu_id = Mtr.mtu_id
					AND mtd.mtd_id = Mtr.mtd_id
					AND mtd.mtd_situacao <> 3
				INNER JOIN TUR_TurmaRelTurmaDisciplina RelTud WITH(NOLOCK)
					ON RelTud.tud_id = Mtd.tud_id
				INNER JOIN TUR_Turma Tur WITH(NOLOCK)
					ON Tur.tur_id = RelTud.tur_id
				INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
					ON Cap.cal_id = Tur.cal_id
					AND Cap.tpc_id = @tpc_id
					AND Cap.cap_situacao <> 3
				LEFT JOIN @tbTurmaAula Tau 
					ON mtd.tud_id = Tau.tud_id
					AND Tau.tau_data >= mtd.mtd_dataMatricula AND Tau.tau_data <ISNULL(mtd.mtd_dataSaida, DATEADD(DAY, 1, Cap.cap_dataFim))
					AND
					(
						(
							Tau.tau_reposicao = 0
							AND tau.tpc_id = @tpc_id
						)
						OR
						(
							Tau.tau_reposicao = 1
							AND Tau.tpc_ordem <= @tpc_ordem
						)
					)
				LEFT JOIN ACA_TipoPeriodoCalendario tpc WITH(NOLOCK)
					ON tpc.tpc_id = tau.tpc_id
					AND tpc.tpc_situacao <> 3
				LEFT JOIN CLS_TurmaAulaAluno Taa WITH(NOLOCK)
					ON Taa.tud_id = Tau.tud_id
					AND Tau.tau_id = Taa.tau_id
					AND Taa.alu_id = mtd.alu_id
					AND Taa.mtu_id = mtd.mtu_id
					AND Taa.mtd_id = mtd.mtd_id
					AND Taa.taa_situacao <> 3

				UNION ALL

				SELECT
					mtd.alu_id
					, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
					,tau.tpc_id
					,ISNULL(tpc.tpc_ordem, 0) AS tpc_ordem
					,Taa.taa_frequencia
					,Tau.tau_reposicao
				FROM @tbAlunos Mtr
				INNER JOIN MTR_MatriculaTurmaDisciplina mtd
					ON mtd.alu_id = Mtr.alu_id
					AND mtd.mtu_id = Mtr.mtu_idOrigem
					AND mtd.mtd_id = Mtr.mtd_idOrigem
					AND mtd.tud_id <> @tud_id
					AND mtd.mtd_situacao <> 3
				INNER JOIN TUR_TurmaRelTurmaDisciplina RelTud WITH(NOLOCK)
					ON RelTud.tud_id = Mtd.tud_id
				INNER JOIN TUR_Turma Tur WITH(NOLOCK)
					ON Tur.tur_id = RelTud.tur_id
				INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
					ON Cap.cal_id = Tur.cal_id
					AND Cap.tpc_id = @tpc_id
					AND Cap.cap_situacao <> 3
				LEFT JOIN @tbTurmaAula Tau 
					ON mtd.tud_id = Tau.tud_id
					AND Tau.tau_data >= mtd.mtd_dataMatricula AND Tau.tau_data < ISNULL(mtd.mtd_dataSaida, DATEADD(DAY, 1, Cap.cap_dataFim))
					AND
					(
						(
							Tau.tau_reposicao = 0
							AND tau.tpc_id = @tpc_id
						)
						OR
						(
							Tau.tau_reposicao = 1
							AND Tau.tpc_ordem <= @tpc_ordem
						)
					)
				LEFT JOIN ACA_TipoPeriodoCalendario tpc WITH(NOLOCK)
					ON tpc.tpc_id = tau.tpc_id
					AND tpc.tpc_situacao <> 3
				LEFT JOIN CLS_TurmaAulaAluno Taa WITH(NOLOCK)
					ON Taa.tud_id = Tau.tud_id
					AND Tau.tau_id = Taa.tau_id
					AND Taa.alu_id = mtd.alu_id
					AND Taa.mtu_id = mtd.mtu_id
					AND Taa.mtd_id = mtd.mtd_id
					AND Taa.taa_situacao <> 3
					
			-- Pedro Silva - 14/08/2015
			-- Criei esta nova tabela para calcular um campo qtFaltasReposicao sem acumularBimestres anteriores
			-- é praticamente uma cópia do select de cima, só alterando o filtro de tpc no left da tbTurmaaula
			insert into @tbFrequenciaSemAcumularReposicao
			(alu_id, mtu_idOrigem, mtd_idOrigem, tpc_id, tpc_ordem, taa_frequencia, tau_reposicao)
				SELECT
					mtd.alu_id
					, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
					,tau.tpc_id
					,ISNULL(tpc.tpc_ordem, 0) AS tpc_ordem
					,Taa.taa_frequencia
					,Tau.tau_reposicao
				FROM @tbAlunos Mtr
				INNER JOIN MTR_MatriculaTurmaDisciplina mtd
					ON mtd.alu_id = Mtr.alu_id
					AND mtd.mtu_id = Mtr.mtu_id
					AND mtd.mtd_id = Mtr.mtd_id
					AND mtd.mtd_situacao <> 3
				INNER JOIN TUR_TurmaRelTurmaDisciplina RelTud WITH(NOLOCK)
					ON RelTud.tud_id = Mtd.tud_id
				INNER JOIN TUR_Turma Tur WITH(NOLOCK)
					ON Tur.tur_id = RelTud.tur_id
				INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
					ON Cap.cal_id = Tur.cal_id
					AND Cap.tpc_id = @tpc_id
					AND Cap.cap_situacao <> 3
				LEFT JOIN @tbTurmaAula Tau 
					ON mtd.tud_id = Tau.tud_id
					AND Tau.tau_data >= mtd.mtd_dataMatricula AND Tau.tau_data <ISNULL(mtd.mtd_dataSaida, DATEADD(DAY, 1, Cap.cap_dataFim))
					AND Tau.tpc_id = @tpc_id
				LEFT JOIN ACA_TipoPeriodoCalendario tpc WITH(NOLOCK)
					ON tpc.tpc_id = tau.tpc_id
					AND tpc.tpc_situacao <> 3
				LEFT JOIN CLS_TurmaAulaAluno Taa WITH(NOLOCK)
					ON Taa.tud_id = Tau.tud_id
					AND Tau.tau_id = Taa.tau_id
					AND Taa.alu_id = mtd.alu_id
					AND Taa.mtu_id = mtd.mtu_id
					AND Taa.mtd_id = mtd.mtd_id
					AND Taa.taa_situacao <> 3

				UNION ALL

				SELECT
					mtd.alu_id
					, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
					,tau.tpc_id
					,ISNULL(tpc.tpc_ordem, 0) AS tpc_ordem
					,Taa.taa_frequencia
					,Tau.tau_reposicao
				FROM @tbAlunos Mtr
				INNER JOIN MTR_MatriculaTurmaDisciplina mtd
					ON mtd.alu_id = Mtr.alu_id
					AND mtd.mtu_id = Mtr.mtu_idOrigem
					AND mtd.mtd_id = Mtr.mtd_idOrigem
					AND mtd.tud_id <> @tud_id
					AND mtd.mtd_situacao <> 3
				INNER JOIN TUR_TurmaRelTurmaDisciplina RelTud WITH(NOLOCK)
					ON RelTud.tud_id = Mtd.tud_id
				INNER JOIN TUR_Turma Tur WITH(NOLOCK)
					ON Tur.tur_id = RelTud.tur_id
				INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
					ON Cap.cal_id = Tur.cal_id
					AND Cap.tpc_id = @tpc_id
					AND Cap.cap_situacao <> 3
				LEFT JOIN @tbTurmaAula Tau 
					ON mtd.tud_id = Tau.tud_id
					AND Tau.tau_data >= mtd.mtd_dataMatricula AND Tau.tau_data < ISNULL(mtd.mtd_dataSaida, DATEADD(DAY, 1, Cap.cap_dataFim))
					AND Tau.tpc_id = @tpc_id
				LEFT JOIN ACA_TipoPeriodoCalendario tpc WITH(NOLOCK)
					ON tpc.tpc_id = tau.tpc_id
					AND tpc.tpc_situacao <> 3
				LEFT JOIN CLS_TurmaAulaAluno Taa WITH(NOLOCK)
					ON Taa.tud_id = Tau.tud_id
					AND Tau.tau_id = Taa.tau_id
					AND Taa.alu_id = mtd.alu_id
					AND Taa.mtu_id = mtd.mtu_id
					AND Taa.mtd_id = mtd.mtd_id
					AND Taa.taa_situacao <> 3

			-- Faltas da disciplina do aluno.
			INSERT INTO @TabelaQtdesFaltas
			(alu_id, mtu_id, mtd_id, qtFaltas, qtFaltasReposicao)
			SELECT
				alu_id, mtu_idOrigem, mtd_idOrigem
				, [0] AS qtFaltas
				, [1] AS qtFaltasReposicao
			FROM
			(
				SELECT alu_id, mtu_idOrigem, mtd_idOrigem
					,tpc_id, tpc_ordem, tau_reposicao
					, taa_frequencia
				FROM @tbFrequencia
			) faltas
			PIVOT
			(
				SUM(taa_frequencia) FOR tau_reposicao IN ([0], [1])
			) AS pvt
			
			-- Faltas da disciplina do aluno.
			INSERT INTO @TabelaQtdesFaltas_SemAcumularReposicao
			(alu_id, mtu_id, mtd_id, qtFaltasReposicaoNaoAcumulada)
			SELECT
				alu_id, mtu_idOrigem, mtd_idOrigem
				, SUM(taa_frequencia) AS qtFaltasReposicaoNaoAcumulada
			FROM
			(
				SELECT alu_id, mtu_idOrigem, mtd_idOrigem
					,tpc_id, tpc_ordem, tau_reposicao
					, taa_frequencia
				  FROM @tbFrequenciaSemAcumularReposicao
				 WHERE tau_reposicao = 1
			) faltas
			group by alu_id, mtu_idOrigem, mtd_idOrigem
			
			
			-- Aulas da disciplina do aluno.
			INSERT INTO @TabelaQtdesAulas
			(alu_id, mtu_id, mtd_id, qtAulas, qtAulasNormais, qtAulasReposicao)
			SELECT
				alu_id, mtu_idOrigem, mtd_idOrigem
				, @qtdeAulas AS QtAulas
				, [0] AS qtAulasNormais
				, [1] AS qtAulasReposicao
			FROM
			(
				SELECT alu_id, mtu_idOrigem, mtd_idOrigem, tpc_id as tpc
					,tpc_id, tpc_ordem, tau_reposicao
					, taa_frequencia
				FROM @tbFrequencia
			) aulas
			PIVOT
			(
				COUNT(tpc) FOR tau_reposicao IN ([0], [1])
			) AS pvt
			
			INSERT INTO @TabelaQtdes 
			      (alu_id, mtu_id, mtd_id, qtFaltas, qtFaltasReposicao, qtAulas, 
			       qtAulasNormais, qtAulasReposicao, qtFaltasReposicaoNaoAcumuladas)
			SELECT alu_id, mtu_id, mtd_id, 
				   SUM(isnull(qtFaltas,0)) as qtFaltas, 
				   SUM(isnull(qtFaltasReposicao,0)) as qtFaltasReposicao,
				   SUM(isnull(qtAulas,0)) as qtAulas, 
				   SUM(isnull(qtAulasNormais,0)) as qtAulasNormais, 
				   SUM(isnull(qtAulasReposicao,0)) as qtAulasReposicao,
				   SUM(isnull(qtFaltasReposicaoNaoAcumulada,0)) as qtFaltasReposicaoNaoAcumuladas 
			  FROM (
					select alu_id, mtu_id, mtd_id, qtFaltas, qtFaltasReposicao, 
						   null as qtAulas, null as qtAulasNormais, null as qtAulasReposicao, null as qtFaltasReposicaoNaoAcumulada
					  from @TabelaQtdesFaltas
					  
					union
					
					select alu_id, mtu_id, mtd_id, null, null, qtAulas, qtAulasNormais, qtAulasReposicao, null
					  from @TabelaQtdesAulas
					  
					union
					  
					select alu_id, mtu_id, mtd_id, null, null, null, null, null, qtFaltasReposicaoNaoAcumulada
					  from @TabelaQtdesFaltas_SemAcumularReposicao
					  
					) as TabAulasFaltas
			 GROUP BY alu_id, mtu_id, mtd_id
		END
		ELSE -- ELSE que vai preencher a @TabelaQtdes para todos os outros tipos (exceto regência com dois titulares e multiseriada do docente)
		BEGIN
			;WITH tbTurmaDisciplina AS 
			(
				SELECT
					tud_id
				FROM
					@tbAlunos Mtr
				GROUP BY
					tud_id
			)
			
			insert into @tbTurmaAula
				SELECT
					Tau.tau_id,
					Tau.tud_id,
					Tau.tau_data,
					Tau.tau_reposicao,
					Tau.tpc_id,
					ISNULL(tpc.tpc_ordem, 0) AS tpc_ordem
				FROM
					tbTurmaDisciplina Tud
					INNER JOIN CLS_TurmaAula Tau WITH(NOLOCK)
						ON Tud.tud_id = Tau.tud_id
						AND Tau.tau_situacao <> 3
					INNER JOIN @tbPosicaoDocente Tdc
						ON Tau.tdt_posicao = Tdc.tdc_posicao
					LEFT JOIN ACA_TipoPeriodoCalendario tpc WITH(NOLOCK)
						ON tpc.tpc_id = Tau.tpc_id
						AND tpc.tpc_situacao <> 3
				GROUP BY
					Tau.tau_id,
					Tau.tud_id,
					Tau.tau_data,
					Tau.tau_reposicao,
					Tau.tpc_id,
					tpc.tpc_ordem
			
		
			-- Faltas da disciplina do aluno.
			INSERT INTO @TabelaQtdesFaltas
			(alu_id, mtu_id, mtd_id, qtFaltas, qtFaltasReposicao)
			SELECT
				alu_id, mtu_idOrigem, mtd_idOrigem
				, [0] AS qtFaltas
				, [1] AS qtFaltasReposicao
			FROM
			(
				SELECT
					Mtr.alu_id, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
					,Taa.taa_frequencia
					,Tau.tau_reposicao
				FROM @tbAlunos Mtr
				LEFT JOIN @tbTurmaAula Tau 
					ON Tau.tud_id = Mtr.tud_id
					AND 
					(
						(
							Tau.tau_reposicao = 0
							AND Tau.tpc_id = @tpc_id
						)
						OR
						(
							Tau.tau_reposicao = 1
							AND Tau.tpc_ordem <= @tpc_ordem
						)
					)
					AND Tau.tau_data >= Mtr.mtd_dataMatricula AND Tau.tau_data < Mtr.mtd_dataSaida
				LEFT JOIN CLS_TurmaAulaAluno Taa WITH(NOLOCK)
					ON Taa.tud_id = Mtr.tud_id
					AND Tau.tau_id = Taa.tau_id
					AND Taa.alu_id = Mtr.alu_id
					AND Taa.mtu_id = Mtr.mtu_id
					AND Taa.mtd_id = Mtr.mtd_id
					AND Taa.taa_situacao <> 3
				WHERE
					Mtr.tud_tipo NOT IN (13, 16) -- 13-Complementação de regência 
												 -- 16-Multisseriada do aluno
			) faltas
			PIVOT
			(
				SUM(taa_frequencia) FOR tau_reposicao IN ([0], [1])
			) AS pvt
			
			-- Pedro Silva - 14/08/2015
			-- Criei esta nova tabela para calcular um campo qtFaltasReposicao sem acumularBimestres anteriores
			INSERT INTO @TabelaQtdesFaltas_SemAcumularReposicao
			(alu_id, mtu_id, mtd_id, qtFaltasReposicaoNaoAcumulada)
			SELECT
				alu_id, mtu_idOrigem, mtd_idOrigem
				, SUM(taa_frequencia) AS qtFaltasReposicaoNaoAcumulada
			FROM
			(
				SELECT
					Mtr.alu_id, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
					,Taa.taa_frequencia
					,Tau.tau_reposicao
				FROM @tbAlunos Mtr
				LEFT JOIN @tbTurmaAula Tau 
					ON Tau.tud_id = Mtr.tud_id
				    AND Tau.tpc_id = @tpc_id
				    AND Tau.tau_data >= Mtr.mtd_dataMatricula AND Tau.tau_data < Mtr.mtd_dataSaida
				LEFT JOIN CLS_TurmaAulaAluno Taa WITH(NOLOCK)
					ON Taa.tud_id = Mtr.tud_id
					AND Tau.tau_id = Taa.tau_id
					AND Taa.alu_id = Mtr.alu_id
					AND Taa.mtu_id = Mtr.mtu_id
					AND Taa.mtd_id = Mtr.mtd_id
					AND Taa.taa_situacao <> 3
				WHERE
					Mtr.tud_tipo NOT IN (13, 16)
				    AND Tau.tau_reposicao = 1
			) faltas
			group by alu_id, mtu_idOrigem, mtd_idOrigem
			
			-- Aulas da disciplina do aluno.
			INSERT INTO @TabelaQtdesAulas
			(alu_id, mtu_id, mtd_id, qtAulas, qtAulasNormais, qtAulasReposicao)
			SELECT
				alu_id, mtu_idOrigem, mtd_idOrigem
				, @qtdeAulas AS QtAulas
				, [0] AS qtAulasNormais
				, [1] AS qtAulasReposicao
			FROM
			(
				SELECT
					Mtr.alu_id, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
					,Tau.tud_id as tud
					,Tau.tau_reposicao
				FROM @tbAlunos Mtr
				LEFT JOIN @tbTurmaAula Tau 
					ON Tau.tud_id = Mtr.tud_id
					AND Tau.tpc_id = @tpc_id
					AND Tau.tau_data >= Mtr.mtd_dataMatricula AND Tau.tau_data < Mtr.mtd_dataSaida
				WHERE
					Mtr.tud_tipo NOT IN (13, 16) -- 13-Complementação de regência 
												 -- 16-Multisseriada do aluno
			) aulas
			PIVOT
			(
				COUNT(tud) FOR tau_reposicao IN ([0], [1])
			) AS pvt
			
			INSERT INTO @TabelaQtdes 
			      (alu_id, mtu_id, mtd_id, qtFaltas, qtFaltasReposicao, qtAulas, 
				   qtAulasNormais, qtAulasReposicao, qtFaltasReposicaoNaoAcumuladas)
			SELECT alu_id, mtu_id, mtd_id, 
				   SUM(isnull(qtFaltas,0)) as qtFaltas, 
				   SUM(isnull(qtFaltasReposicao,0)) as qtFaltasReposicao,
				   SUM(isnull(qtAulas,0)) as qtAulas, 
				   SUM(isnull(qtAulasNormais,0)) as qtAulasNormais, 
				   SUM(isnull(qtAulasReposicao,0)) as qtAulasReposicao,
				   SUM(isnull(qtFaltasReposicaoNaoAcumulada,0)) as qtFaltasReposicaoNaoAcumuladas 
			  FROM (
					select alu_id, mtu_id, mtd_id, qtFaltas, qtFaltasReposicao, 
						   null as qtAulas, null as qtAulasNormais, null as qtAulasReposicao, null as qtFaltasReposicaoNaoAcumulada
					  from @TabelaQtdesFaltas
					  
					union
					
					select alu_id, mtu_id, mtd_id, null, null, qtAulas, qtAulasNormais, qtAulasReposicao, null 
					  from @TabelaQtdesAulas
					
					union
					  
					select alu_id, mtu_id, mtd_id, null, null, null, null, null, qtFaltasReposicaoNaoAcumulada
					  from @TabelaQtdesFaltas_SemAcumularReposicao
					  
					) as TabAulasFaltas
			 GROUP BY alu_id, mtu_id, mtd_id
			
			; WITH MatriculasIngles AS
			(
				SELECT
					Mtr.alu_id
					, Mtr.mtu_id
					, Mtd.mtd_id -- Traz o mtd da disciplina Complementação de regência dele na turma.
					, Mtr.tud_id
					, Mtr.mtd_dataMatricula
					, Mtr.mtd_dataSaida
					, Mtr.tud_tipo, Mtr.tud_idRegencia, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
				FROM @tbAlunos Mtr
				INNER JOIN MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
					ON Mtd.alu_id = Mtr.alu_id
					AND Mtd.mtu_id = Mtr.mtu_id
					AND Mtd.tud_id = Mtr.tud_idRegencia
					AND Mtd.mtd_situacao <> 3
			)
			
			, tbTurmaDisciplina AS 
			(
				SELECT
					tud_id
				FROM
					@tbAlunos Mtr
				GROUP BY
					tud_id
			)
			
			, tbTurmaAula AS 
			(
				SELECT
					Tau.tau_id,
					Tau.tud_id,
					Tau.tau_data,
					Tau.tau_numeroAulas,
					Tau.tau_reposicao,
					Tau.tpc_id,
					ISNULL(tpc.tpc_ordem, 0) AS tpc_ordem
				FROM
					tbTurmaDisciplina Tud
					INNER JOIN CLS_TurmaAula Tau WITH(NOLOCK)
						ON Tud.tud_id = Tau.tud_id
						AND Tau.tau_situacao <> 3
					INNER JOIN @tbPosicaoDocente Tdc
						ON Tau.tdt_posicao = Tdc.tdc_posicao
					LEFT JOIN ACA_TipoPeriodoCalendario tpc WITH(NOLOCK)
						ON tpc.tpc_id = Tau.tpc_id
						AND tpc.tpc_situacao <> 3
				GROUP BY
					Tau.tau_id,
					Tau.tud_id,
					Tau.tau_data,
					Tau.tau_numeroAulas,
					Tau.tau_reposicao,
					Tau.tpc_id,
					tpc.tpc_ordem
			)
			
			-- Aulas da disciplina Complementação de regência do aluno.
			INSERT INTO @TabelaQtdes
			(alu_id, mtu_id, mtd_id, qtAulas, qtFaltas, qtFaltasReposicao)
			SELECT
				alu_id, mtu_idOrigem, mtd_idOrigem
				, @qtdeAulas AS QtAulas
				--, [0] AS qtFaltas		-- Comentadas por Daniel Jun para resolução de um bug. Tipo usado apenas em 2014, deve ficar 0 mesmo
				--, [1] AS qtFaltasReposicao
				, 0 AS qtFaltas
				, 0 AS qtFaltasReposicao
			FROM
			(
				SELECT
					Mtr.alu_id, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
					--, @qtdeAulas AS QtAulas
					-- Quantidade de aulas do Inglês * quantidade de faltas da regência (no mesmo dia).
					--, SUM(ISNULL(Tau.tau_numeroAulas, 0) * ISNULL(TaaRegencia.taa_frequencia, 0)) AS QtFaltas
					--, Tau.*
					,ISNULL(Tau.tau_numeroAulas, 0) * ISNULL(TaaRegencia.taa_frequencia, 0) AS taa_frequencia
					,Tau.tau_reposicao
				FROM MatriculasIngles Mtr
				LEFT JOIN tbTurmaAula Tau 
					ON Tau.tud_id = Mtr.tud_id
					AND 
					(
						(
							Tau.tau_reposicao = 0
							AND Tau.tpc_id = @tpc_id
						)
						OR
						(
							Tau.tau_reposicao = 1
							AND Tau.tpc_ordem <= @tpc_ordem
						)
					)
					AND Tau.tau_data >= Mtr.mtd_dataMatricula AND Tau.tau_data < Mtr.mtd_dataSaida
				LEFT JOIN CLS_TurmaAula TauRegencia WITH(NOLOCK)
					ON TauRegencia.tud_id = Mtr.tud_idRegencia
					AND TauRegencia.tau_data = Tau.tau_data
					AND TauRegencia.tau_situacao <> 3
					AND TauRegencia.tdt_posicao IN (SELECT tdc_posicao FROM @tbPosicaoDocente Tdc)
				LEFT JOIN CLS_TurmaAulaAluno TaaRegencia WITH(NOLOCK)
					ON TaaRegencia.tud_id = TauRegencia.tud_id
					AND TaaRegencia.tau_id = TauRegencia.tau_id -- Busca a quantidade de faltas da regência.
					AND TaaRegencia.alu_id = Mtr.alu_id
					AND TaaRegencia.mtu_id = Mtr.mtu_id
					AND TaaRegencia.mtd_id = Mtr.mtd_id
					AND TaaRegencia.taa_situacao <> 3
				WHERE
					Mtr.tud_tipo = 13 -- 13-Complementação de regência
					AND NOT EXISTS (
						SELECT 1 
						FROM @TabelaQtdes Qtd 
						WHERE 
							Mtr.alu_id = Qtd.alu_id
							AND Mtr.mtu_idOrigem = Qtd.mtu_id
							AND Mtr.mtd_idOrigem = Qtd.mtd_id 
					)
				--GROUP BY
				--	Mtr.alu_id, Mtr.mtu_idOrigem, Mtr.mtd_idOrigem
			) faltas
			PIVOT
			(
				SUM(taa_frequencia) FOR tau_reposicao IN ([0], [1])
			) AS pvt
		END
		
	END
	
	--Pedro Silva - 29/07/2015
	--alguns destes campos estavam retornando null em algumas situações 
	--e tinha algumas procedures que não estavam tratando para receber null, por isso adicionei os updates
	update @TabelaQtdes set qtAulas = 0			  where qtAulas is null
	update @TabelaQtdes set qtAulasNormais = 0	  where qtAulasNormais is null
	update @TabelaQtdes set qtAulasReposicao = 0  where qtAulasReposicao is null
	update @TabelaQtdes set qtFaltas = 0		  where qtFaltas is null
	update @TabelaQtdes set qtFaltasReposicao = 0 where qtFaltasReposicao is null
	update @TabelaQtdes set qtFaltasReposicaoNaoAcumuladas = 0 where qtFaltasReposicaoNaoAcumuladas is null

	RETURN;
END
GO
PRINT N'Creating [dbo].[STP_ACA_AlunoJustificativaFalta_SELECT]'
GO

CREATE PROCEDURE [dbo].[STP_ACA_AlunoJustificativaFalta_SELECT]
	
AS
BEGIN
	SELECT 
		alu_id
		,afj_id
		,tjf_id
		,afj_dataInicio
		,afj_dataFim
		,afj_situacao
		,afj_dataCriacao
		,afj_dataAlteracao
		,pro_id
		,afj_observacao

	FROM 
		ACA_AlunoJustificativaFalta WITH(NOLOCK) 
	
END

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
PRINT N'Creating [dbo].[NEW_ACA_AlunoJustificativaFalta_VerificaAlunoAvaliacao]'
GO
-- =======================================================
-- Author:		Ivan Roberto Pimentel
-- Create date: 23/07/2011 
-- Description: Retorna todas as avaliações que já tenham sido efetivadas para um aluno
--				dentro do periodo indicado, filtrando pelo: alu_id e periodo
-- =======================================================
CREATE PROCEDURE [dbo].[NEW_ACA_AlunoJustificativaFalta_VerificaAlunoAvaliacao]
	@alu_id BIGINT,
	@afj_dataInicio DATE,
	@afj_dataFim DATE
AS
BEGIN
	(	
		SELECT 	
			ava.fav_id		
			, ava.ava_id	
			, ava.ava_nome
		FROM 
			CLS_AlunoAvaliacaoTurma aat WITH (NOLOCK)
			INNER JOIN ACA_Avaliacao ava WITH (NOLOCK)
				ON aat.ava_id = ava.ava_id
				AND aat.fav_id = ava.fav_id
				AND ava.ava_situacao <> 3
			INNER JOIN ACA_TipoPeriodoCalendario tpc WITH (NOLOCK)
				ON tpc.tpc_id = ava.tpc_id
				AND tpc.tpc_situacao <> 3
			INNER JOIN ACA_CalendarioPeriodo cp WITH (NOLOCK)
				ON tpc.tpc_id = cp.tpc_id
				AND cp.cap_situacao <> 3	
		WHERE 
			aat.aat_situacao <> 3
			AND aat.alu_id = @alu_id
			AND cap_dataFim >= @afj_dataInicio
			AND ((@afj_dataFim IS NULL) OR (cap_dataInicio <= @afj_dataFim))
	)
	UNION 
	(	
		SELECT 
			ava.fav_id		
			, ava.ava_id
			, ava.ava_nome
		FROM 
			CLS_AlunoAvaliacaoTurmaDisciplina aatd WITH (NOLOCK)
			INNER JOIN ACA_Avaliacao ava WITH (NOLOCK)
				ON aatd.ava_id = ava.ava_id
				AND aatd.fav_id = ava.fav_id
				AND ava.ava_situacao <> 3
			INNER JOIN ACA_TipoPeriodoCalendario tpc WITH (NOLOCK)
				ON tpc.tpc_id = ava.tpc_id
				AND tpc.tpc_situacao <> 3
			INNER JOIN ACA_CalendarioPeriodo cp WITH (NOLOCK)
				ON tpc.tpc_id = cp.tpc_id
				AND cp.cap_situacao <> 3	
		WHERE 
			aatd.atd_situacao <> 3
			AND aatd.alu_id = @alu_id
			AND cap_dataFim >= @afj_dataInicio
			AND ((@afj_dataFim IS NULL) OR (cap_dataInicio <= @afj_dataFim))
			
	)	
	SELECT @@ROWCOUNT	
	
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
			
			--Processa pendência de planejamento (objetos de aprendizagem)
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
PRINT N'Altering [dbo].[NEW_CLS_TurmaAulaAluno_Frequencia_SelectBy_TurmaDisciplina]'
GO
-- ========================================================================
-- Author:		Juliano REal
-- Create date: 09/05/2014 11:35
-- Description:	utilizado no minhas turmas, para retornar a frequencia
-- sem os dados do aluno

-- Alterado: Haila Pelloso - 10/07/2015
-- Description: Verificando dado da última alteraçao da tabela de log.
-- ========================================================================
ALTER PROCEDURE [dbo].[NEW_CLS_TurmaAulaAluno_Frequencia_SelectBy_TurmaDisciplina]
	@tud_id BIGINT
	, @tau_id INT
	, @dtTurmas TipoTabela_Turma READONLY

AS
BEGIN	
	DECLARE @tud_tipo TINYINT =
	(
		SELECT TOP 1
			tud_tipo
		FROM
			TUR_TurmaDisciplina tud WITH(NOLOCK)
		WHERE
			tud_id = @tud_id
			AND tud_situacao <> 3
	)

	DECLARE @tbLogAlteracaoFrequencia AS TABLE (tud_id BIGINT NOT NULL, tau_id INT NOT NULL, usu_id UNIQUEIDENTIFIER NOT NULL, lta_data DATETIME NOT NULL, PRIMARY KEY(tud_id, tau_id))
	INSERT INTO @tbLogAlteracaoFrequencia (tud_id, tau_id, usu_id, lta_data)
		SELECT 
			Lta.tud_id,
			Lta.tau_id,
			Lta.usu_id, 
			Lta.lta_data 
		FROM 
			dbo.FN_RetornaUltimaAlteracaoTurmaAula(@tud_id, @tau_id, 3) AS Lta --Alteração na frequência
	
	
	IF (@tud_tipo = 15)
	BEGIN
		
		--Selecina as movimentações que possuem matrícula anterior
		;WITH TabelaMovimentacao AS (
			SELECT
				alu_id,
				mtu_idAnterior,
				tmv_nome    
			FROM
				MTR_Movimentacao Mov WITH (NOLOCK) 
				INNER JOIN ACA_TipoMovimentacao Tmv WITH (NOLOCK) 
					ON Mov.tmv_idSaida = Tmv.tmv_id
					AND Tmv.tmv_situacao <> 3
			WHERE
				Mov.mov_situacao NOT IN (3,4)
				AND Mov.mtu_idAnterior IS NOT NULL
		)
		SELECT	
			mtd.alu_id
			, mtd.mtu_id
			, mtd.mtd_id
			, mtd.tud_id
			, tau.tau_id
			, taa.taa_frequencia 	
			, tau.tau_numeroAulas	
			, tau.tau_data
			, tau.tau_efetivado
			, tau.tdt_posicao
			-- 0 - não possui FJ / 1 - possui FJ que abona / 2 - possui FJ que não abona 
			, CASE WHEN afj.afj_id IS NULL
					THEN '0'					    						    
					ELSE (CASE WHEN tjf.tjf_abonaFalta = 1 THEN '1' ELSE '2' END)
			   END AS falta_justificada
			, CAST(CASE WHEN ajf.alu_id IS NULL THEN 0 ELSE 1 END AS BIT) AS falta_abonada			
			-- Verifica se há dispensa de disciplina para o aluno.
			, 0 AS dispensadisciplina
			, taa.taa_frequenciaBitMap
			, Mtd.mtd_situacao
			
			, ISNULL(pesAlteracao.pes_nome,usuAlteracao.usu_Login) as nomeUsuAlteracao -- inserido para poder exibir o usuário que alterou os dados 
			, ISNULL(Lta.lta_data, tau.tau_dataAlteracao) AS tau_dataAlteracao			-- inserido para poder exibir a data que o usuário realizou a alteração
			
		FROM 
			MTR_MatriculaTurmaDisciplina mtd WITH(NOLOCK)
			INNER JOIN MTR_MatriculaTurma mtu WITH(NOLOCK)
				ON mtd.alu_id = mtu.alu_id
				AND mtd.mtu_id = mtu.mtu_id
				AND mtd.mtd_situacao <> 3
			INNER JOIN CLS_TurmaAula tau WITH (NOLOCK)
				ON tau.tud_id = mtd.tud_id
				AND tau.tau_id = @tau_id
				AND tau.tau_situacao <> 3
			INNER JOIN TUR_Turma tur WITH(NOLOCK)
				ON tur.tur_id = mtu.tur_id
				AND tur.tur_situacao <> 3
			INNER JOIN @dtTurmas dtt
				ON tur.tur_id = dtt.tur_id
			LEFT JOIN CLS_TurmaAulaAluno taa WITH (NOLOCK)
				ON taa.tud_id = mtd.tud_id
				AND taa.tau_id = tau.tau_id
				AND taa.alu_id = mtd.alu_id
				AND taa.mtu_id = mtd.mtu_id
				AND taa.mtd_id = mtd.mtd_id
				AND taa.taa_situacao <> 3
			LEFT JOIN ACA_AlunoJustificativaFalta afj WITH(NOLOCK)
				ON  afj.alu_id = mtd.alu_id
				AND afj.afj_situacao <> 3
				AND (tau_data >= afj.afj_dataInicio)
				AND ((afj.afj_dataFim  IS NULL) OR (tau_data <= afj.afj_dataFim))
			LEFT JOIN ACA_TipoJustificativaFalta tjf WITH(NOLOCK)
				ON tjf.tjf_id = afj.tjf_id
				AND tjf.tjf_situacao <> 3
			LEFT JOIN ACA_AlunoJustificativaAbonoFalta ajf WITH(NOLOCK)
				ON  ajf.alu_id = mtd.alu_id
				AND ajf.tud_id = mtd.tud_id
				AND ajf.ajf_situacao <> 3
				AND (Tau.tau_data >= ajf.ajf_dataInicio)
				AND (Tau.tau_data <= ajf.ajf_dataFim)
			---		
			LEFT JOIN @tbLogAlteracaoFrequencia AS Lta 
				ON Tau.tud_id = Lta.tud_id
				AND Tau.tau_id = Lta.tau_id	
			LEFT JOIN Synonym_SYS_Usuario AS usuAlteracao WITH(NOLOCK)
				ON usuAlteracao.usu_id = ISNULL(Lta.usu_id, tau.usu_idDocenteAlteracao)
				AND usuAlteracao.usu_situacao <> 3
			LEFT JOIN Synonym_PES_Pessoa AS pesAlteracao WITH(NOLOCK)
				ON  pesAlteracao.pes_id = usuAlteracao.pes_id
				AND pesAlteracao.pes_situacao <> 3				
		WHERE 
			mtd.tud_id = @tud_id
			AND ISNULL(Mtd.mtd_numeroChamada, 0) >= 0
			AND Mtd.mtd_situacao <> 3
			-- Valida o período de matrícula e saída do aluno (se está dentro do período atual).
			AND (DATEDIFF(DAY, Mtd.mtd_dataMatricula, tau.tau_data) >= 0)
			AND (Mtd.mtd_situacao <> 5 OR ISNULL((DATEDIFF(DAY, Mtd.mtd_dataSaida, tau.tau_data)), 0) <= 0)
		GROUP BY
			mtd.alu_id
			, mtd.mtu_id
			, mtd.mtd_id
			, mtd.tud_id
			, tau.tau_id
			, mtd_situacao
			, mtd.mtd_numeroChamada
			, taa.taa_frequencia 	
			, tau.tau_numeroAulas	
			, mtd.mtd_dataMatricula	
			, mtd.mtd_dataSaida
			, tau.tau_data
			, tau.tau_efetivado
			, tau.tdt_posicao
			, afj.afj_id
			, tjf.tjf_abonaFalta
			, ajf.alu_id
			, taa.taa_frequenciaBitMap
			, ISNULL(pesAlteracao.pes_nome,usuAlteracao.usu_Login)
			, Lta.lta_data
			, tau.tau_dataAlteracao			
		
	END
	ELSE
	BEGIN
		--Selecina as movimentações que possuem matrícula anterior
		;WITH TabelaMovimentacao AS (
			SELECT
				alu_id,
				mtu_idAnterior,
				tmv_nome    
			FROM
				MTR_Movimentacao Mov WITH (NOLOCK) 
				INNER JOIN ACA_TipoMovimentacao Tmv WITH (NOLOCK) 
					ON Mov.tmv_idSaida = Tmv.tmv_id
					AND Tmv.tmv_situacao <> 3
			WHERE
				Mov.mov_situacao NOT IN (3,4)
				AND Mov.mtu_idAnterior IS NOT NULL
		)
		SELECT	
			mtd.alu_id
			, mtd.mtu_id
			, mtd.mtd_id
			, mtd.tud_id
			, tau.tau_id
			, ISNULL(taa.taa_frequencia, 0) AS taa_frequencia
			, ISNULL(tau.tau_numeroAulas, 0) AS tau_numeroAulas	
			, tau.tau_data
			, CAST(ISNULL(tau.tau_efetivado, 0) AS BIT) tau_efetivado
			, CAST(ISNULL(tau.tdt_posicao, 0) AS TINYINT) AS tdt_posicao
			-- 0 - não possui FJ / 1 - possui FJ que abona / 2 - possui FJ que não abona 
			, CASE WHEN afj.afj_id IS NULL
					THEN '0'					    						    
					ELSE (CASE WHEN tjf.tjf_abonaFalta = 1 THEN '1' ELSE '2' END)
			   END AS falta_justificada
			, CAST(CASE WHEN ajf.alu_id IS NULL THEN 0 ELSE 1 END AS BIT) AS falta_abonada
			-- Verifica se há dispensa de disciplina para o aluno.
			, 0 AS dispensadisciplina
			, ISNULL(taa.taa_frequenciaBitMap, '') AS taa_frequenciaBitMap
			, Mtd.mtd_situacao
			
			, ISNULL(pesAlteracao.pes_nome,usuAlteracao.usu_Login) as nomeUsuAlteracao	-- inserido para poder exibir o usuário que alterou os dados 
			, ISNULL(Lta.lta_data, tau.tau_dataAlteracao) AS tau_dataAlteracao			-- inserido para poder exibir a data que o usuário realizou a alteração
			
		FROM 
			MTR_MatriculaTurma mtu WITH(NOLOCK)
			INNER JOIN MTR_MatriculaTurmaDisciplina mtd WITH(NOLOCK)
				ON mtu.mtu_id = mtd.mtu_id
				AND mtu.alu_id = mtd.alu_id
				AND ISNULL(Mtd.mtd_numeroChamada, 0) >= 0
				AND Mtd.mtd_situacao <> 3
			INNER JOIN CLS_TurmaAula tau WITH (NOLOCK)
				ON tau.tud_id = mtd.tud_id	
				AND tau.tau_id = @tau_id
				AND tau.tau_situacao <> 3
			INNER JOIN TUR_Turma tur WITH(NOLOCK)
				ON tur.tur_id = mtu.tur_id
				AND tur.tur_situacao <> 3	
			LEFT JOIN CLS_TurmaAulaAluno taa WITH (NOLOCK)		
				ON taa.tud_id = mtd.tud_id
				AND taa.tau_id = tau.tau_id
				AND taa.alu_id = mtd.alu_id
				AND taa.mtu_id = mtd.mtu_id
				AND taa.mtd_id = mtd.mtd_id
				AND taa.taa_situacao <> 3
			LEFT JOIN ACA_AlunoJustificativaFalta afj WITH(NOLOCK)
				ON  afj.alu_id = mtd.alu_id
				AND afj.afj_situacao <> 3
				AND (tau_data >= afj.afj_dataInicio)
				AND ((afj.afj_dataFim  IS NULL) OR (tau_data <= afj.afj_dataFim))
			LEFT JOIN ACA_TipoJustificativaFalta tjf WITH(NOLOCK)
				ON tjf.tjf_id = afj.tjf_id
				AND tjf.tjf_situacao <> 3
			LEFT JOIN ACA_AlunoJustificativaAbonoFalta ajf WITH(NOLOCK)
				ON  ajf.alu_id = mtd.alu_id
				AND ajf.tud_id = mtd.tud_id
				AND ajf.ajf_situacao <> 3
				AND (Tau.tau_data >= ajf.ajf_dataInicio)
				AND (Tau.tau_data <= ajf.ajf_dataFim)
			---		
			LEFT JOIN @tbLogAlteracaoFrequencia AS Lta 
				ON Tau.tud_id = Lta.tud_id
				AND Tau.tau_id = Lta.tau_id	
			LEFT JOIN Synonym_SYS_Usuario AS usuAlteracao WITH(NOLOCK)
				ON usuAlteracao.usu_id = ISNULL(Lta.usu_id, tau.usu_idDocenteAlteracao)
				AND usuAlteracao.usu_situacao <> 3
			LEFT JOIN Synonym_PES_Pessoa AS pesAlteracao WITH(NOLOCK)
				ON  pesAlteracao.pes_id = usuAlteracao.pes_id
				AND pesAlteracao.pes_situacao <> 3				
		WHERE 
			Mtd.tud_id = @tud_id
			AND Mtu.mtu_situacao <> 3
			-- Valida o período de matrícula e saída do aluno (se está dentro do período atual).
			AND (DATEDIFF(DAY, Mtd.mtd_dataMatricula, tau.tau_data) >= 0)
			AND (Mtd.mtd_situacao <> 5 OR ISNULL((DATEDIFF(DAY, Mtd.mtd_dataSaida, tau.tau_data)), 0) <= 0)
		GROUP BY
			mtd.alu_id
			, mtd.mtu_id
			, mtd.mtd_id
			, mtd.tud_id
			, tau.tau_id
			, mtd_situacao
			, mtd.mtd_numeroChamada
			, taa.taa_frequencia 	
			, tau.tau_numeroAulas	
			, mtd.mtd_dataMatricula	
			, mtd.mtd_dataSaida
			, tau.tau_data
			, tau.tau_efetivado
			, tau.tdt_posicao
			, afj.afj_id
			, tjf.tjf_abonaFalta
			, ajf.alu_id
			, taa.taa_frequenciaBitMap
			, ISNULL(pesAlteracao.pes_nome,usuAlteracao.usu_Login)
			, tau.tau_dataAlteracao
			, Lta.lta_data
			
	END 
END
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
PRINT N'Altering [dbo].[NEW_CLS_TurmaAulaAluno_SelectBy_TurmaDisciplinaFiltroDeficiencia]'
GO

-- ========================================================================
-- Author:		Daniel Jun Suguimoto
-- Create date: 11/03/2014
-- Description:	Retorna os lançamentos de frequência dos alunos, filtrando
--				os alunos com ou sem deficiência, dependendo do docente.
-- ========================================================================
ALTER PROCEDURE [dbo].[NEW_CLS_TurmaAulaAluno_SelectBy_TurmaDisciplinaFiltroDeficiencia]
	@tud_id BIGINT	
	, @tau_id INT	
	, @tdc_id TINYINT
	, @ent_id UNIQUEIDENTIFIER	
	, @ordenacao TINYINT
AS
BEGIN	

	DECLARE @tbAlunos TABLE (alu_id INT);
	
	IF (@tdc_id = 5)
	BEGIN
		;WITH MatriculaTurmaDisciplina AS
		(
			SELECT
				mtd.alu_id
			FROM
				MTR_MatriculaTurmaDisciplina mtd WITH(NOLOCK)
			WHERE
				mtd.tud_id = @tud_id
				AND mtd.mtd_situacao <> 3
		)
		
		, TipoDeficiencia AS 
		(
			SELECT 
				RelTde.tde_id
			FROM
				TUR_TurmaDisciplinaRelDisciplina DisRel WITH(NOLOCK)
				INNER JOIN ACA_Disciplina dis WITH(NOLOCK)
					ON DisRel.dis_id = dis.dis_id
					AND dis.dis_situacao <> 3
				INNER JOIN ACA_TipoDisciplina tds WITH(NOLOCK)
					ON dis.tds_id = tds.tds_id
					AND tds.tds_situacao <> 3
				INNER JOIN ACA_TipoDisciplinaDeficiencia RelTde WITH(NOLOCK)
					ON tds.tds_id = RelTde.tds_id
			WHERE
				DisRel.tud_id = @tud_id
		)
		
		INSERT INTO @tbAlunos 
		(
			alu_id
		)
		SELECT
			mtd.alu_id
		FROM
			MatriculaTurmaDisciplina mtd 
			INNER JOIN ACA_Aluno alu WITH(NOLOCK)
				ON alu.alu_id = mtd.alu_id
			INNER JOIN Synonym_PES_PessoaDeficiencia pde WITH(NOLOCK)
				ON pde.pes_id = alu.pes_id
			INNER JOIN TipoDeficiencia tde
				ON pde.tde_id = tde.tde_id
	END
	ELSE
	BEGIN
		;WITH MatriculaTurmaDisciplina AS
		(
			SELECT
				mtd.alu_id
			FROM
				MTR_MatriculaTurmaDisciplina mtd WITH(NOLOCK)
			WHERE
				mtd.tud_id = @tud_id
				AND mtd.mtd_situacao <> 3
		)
		
		, TipoDeficiencia AS 
		(
			SELECT 
				RelTde.tde_id
			FROM
				TUR_TurmaDisciplinaRelDisciplina DisRel WITH(NOLOCK)
				INNER JOIN ACA_Disciplina dis WITH(NOLOCK)
					ON DisRel.dis_id = dis.dis_id
					AND dis.dis_situacao <> 3
				INNER JOIN ACA_TipoDisciplina tds WITH(NOLOCK)
					ON dis.tds_id = tds.tds_id
					AND tds.tds_situacao <> 3
				INNER JOIN ACA_TipoDisciplinaDeficiencia RelTde WITH(NOLOCK)
					ON tds.tds_id = RelTde.tds_id
			WHERE
				DisRel.tud_id = @tud_id
		)
		
		INSERT INTO @tbAlunos 
		(
			alu_id
		)
		SELECT
			mtd.alu_id
		FROM
			MatriculaTurmaDisciplina mtd 
			INNER JOIN ACA_Aluno alu WITH(NOLOCK)
				ON alu.alu_id = mtd.alu_id
			LEFT JOIN Synonym_PES_PessoaDeficiencia pde WITH(NOLOCK)
				ON pde.pes_id = alu.pes_id
		WHERE
			(NOT EXISTS (SELECT tde_id FROM TipoDeficiencia tde WHERE tde.tde_id = pde.tde_id ))	
	END

	--Selecina as movimentações que possuem matrícula anterior
	;WITH TabelaMovimentacao AS (
		SELECT
			alu_id,
			mtu_idAnterior,
			tmv_nome    
		FROM
			MTR_Movimentacao MOV WITH (NOLOCK) 
			INNER JOIN ACA_TipoMovimentacao TMV WITH (NOLOCK) 
				ON MOV.tmv_idSaida = TMV.tmv_id
		WHERE
			mov_situacao NOT IN (3,4)
			AND tmv_situacao <> 3
			AND mtu_idAnterior IS NOT NULL
	)
	SELECT	
		mtd.alu_id
		, mtd.mtu_id
		, mtd.mtd_id
		, mtd.tud_id
		, tau.tau_id
		, pes.pes_nome + 
			(
				CASE WHEN mtd_situacao = 5 THEN ISNULL((SELECT TOP 1 ' (' + tmv_nome + ')' FROM TabelaMovimentacao MOV WITH(NOLOCK) WHERE MOV.mtu_idAnterior = mtd.mtu_id AND MOV.alu_id = mtd.alu_id), ' (Inativo)')
					ELSE '' END
			) AS pes_nome
		,  CASE WHEN mtd.mtd_numeroChamada > 0 THEN CAST(Mtd.mtd_numeroChamada AS VARCHAR)
					ELSE '-' END as mtd_numeroChamada
		, taa.taa_frequencia 	
		, tau.tau_numeroAulas	
		, mtd.mtd_dataMatricula	
		, mtd.mtd_dataSaida
		, tau.tau_data
		-- 0 - não possui FJ / 1 - possui FJ que abona / 2 - possui FJ que não abona 
		, CASE WHEN afj.afj_id IS NULL
				THEN '0'					    						    
				ELSE (CASE WHEN tjf.tjf_abonaFalta = 1 THEN '1' ELSE '2' END)
		   END AS falta_justificada
		, NULL AS tca_numeroAvaliacao
		-- Verifica se há dispensa de disciplina para o aluno.
		, 0 AS dispensadisciplina
		, taa.taa_frequenciaBitMap
		, mtd.mtd_situacao
	FROM 
		MTR_MatriculaTurma mtu WITH(NOLOCK)
		INNER JOIN MTR_MatriculaTurmaDisciplina mtd WITH(NOLOCK)
			ON mtu.mtu_id = mtd.mtu_id
			AND mtu.alu_id = mtd.alu_id
		INNER JOIN @tbAlunos
			ON mtd.alu_id = [@tbAlunos].alu_id
		INNER JOIN ACA_Aluno alu WITH(NOLOCK)
			ON mtd.alu_id = alu.alu_id
		INNER JOIN VW_DadosAlunoPessoa pes
			ON alu.alu_id = pes.alu_id
		INNER JOIN CLS_TurmaAula tau WITH (NOLOCK)
			ON tau.tud_id = mtd.tud_id	
		INNER JOIN TUR_Turma tur WITH(NOLOCK)
			ON tur.tur_id = mtu.tur_id
			AND tur.tur_situacao <> 3		
		LEFT JOIN CLS_TurmaAulaAluno taa WITH (NOLOCK)		
			ON taa.tud_id = mtd.tud_id
				AND taa.tau_id = tau.tau_id
				AND taa.alu_id = mtd.alu_id
				AND taa.mtu_id = mtd.mtu_id
				AND taa.mtd_id = mtd.mtd_id
				AND taa.taa_situacao <> 3
		LEFT JOIN ACA_AlunoJustificativaFalta afj WITH(NOLOCK)
			ON  afj.alu_id = mtd.alu_id
			AND afj.afj_situacao <> 3
			AND (tau_data >= afj.afj_dataInicio)
			AND ((afj.afj_dataFim  IS NULL) OR (tau_data <= afj.afj_dataFim))
		LEFT JOIN ACA_TipoJustificativaFalta tjf WITH(NOLOCK)
			ON tjf.tjf_id = afj.tjf_id
			AND tjf.tjf_situacao <> 3
		LEFT OUTER JOIN CLS_AlunoAvaliacaoTurma Aat WITH(NOLOCK)
			ON (Aat.tur_id = Mtu.tur_id)
				AND (Aat.alu_id = Mtu.alu_id)
				AND (Aat.mtu_id = Mtu.mtu_id)
				AND (Aat.fav_id = tur.fav_id)
				AND (Aat.aat_situacao <> 3)
				AND Aat.ava_id =
				(
					SELECT  TOP 1 ava.ava_id
					FROM    ACA_Avaliacao ava WITH(NOLOCK)
					WHERE
						 ava.fav_id = tur.fav_id
						 AND ava.tpc_id = tau.tpc_id
						 AND ava.ava_situacao <> 3
				)
	WHERE 
		alu_situacao <> 3
		AND tau.tau_situacao <> 3
		AND Mtd.tud_id = @tud_id
		AND tau.tau_id = @tau_id
		AND alu.ent_id = @ent_id
		AND Mtd.mtd_situacao <> 3
		AND mtu_situacao <> 3
		-- Valida o período de matrícula e saída do aluno (se está dentro do período atual).
		AND (DATEDIFF(DAY, Mtd.mtd_dataMatricula, tau.tau_data) >= 0)
		AND ISNULL(Mtd.mtd_numeroChamada, 0) >= 0
		AND (Mtd.mtd_situacao <> 5 OR ISNULL((DATEDIFF(DAY, Mtd.mtd_dataSaida, tau.tau_data)), 0) <= 0)
	GROUP BY
		mtd.alu_id
		, mtd.mtu_id
		, mtd.mtd_id
		, mtd.tud_id
		, tau.tau_id
		, pes.pes_nome
		, mtd_situacao
		, mtd.mtd_numeroChamada
		, taa.taa_frequencia 	
		, tau.tau_numeroAulas	
		, mtd.mtd_dataMatricula	
		, mtd.mtd_dataSaida
		, tau.tau_data
		, afj.afj_id
		, tjf.tjf_abonaFalta
		, taa.taa_frequenciaBitMap
	ORDER BY 	
		CASE WHEN @ordenacao = 0 THEN 
			CASE WHEN ISNULL(Mtd.mtd_numeroChamada,0) <= 0 THEN 1 ELSE 0 END
		END ASC
		, CASE WHEN @ordenacao = 0 THEN ISNULL(Mtd.mtd_numeroChamada,0) END ASC
		, CASE WHEN @ordenacao = 1 THEN pes.pes_nome END ASC		
END


GO
PRINT N'Altering [dbo].[NEW_CLS_TurmaNota_SelectBy_Periodo_NotaAluno]'
GO
-- ========================================================================
-- Author:		Jean Michel Marques da Silva
-- Create date: 29/03/2010 17:10
-- Description:	Retorna as Atividades com as notas do aluno, caso passado 
--				o alu_id, mtu_id e mtd_id.
--Alterado: Webber V. dos Santos  Data: 30/04/2014
--Description: Alterado forma de filtrar o nome da atividade avaliativa

-- Alterado: Haila Pelloso - 10/07/2015
-- Description: Verificando dado da última alteraçao da tabela de log.
-- ========================================================================
ALTER PROCEDURE [dbo].[NEW_CLS_TurmaNota_SelectBy_Periodo_NotaAluno]	
	@tud_id BIGINT
	, @tpc_id INT
	, @usu_id UNIQUEIDENTIFIER
	, @tdt_posicao TINYINT
	, @dtTurmas TipoTabela_Turma READONLY
AS
BEGIN

	DECLARE @tud_tipo TINYINT;
	SELECT	
		@tud_tipo = ISNULL(tud.tud_tipo,0)
	FROM
		TUR_TurmaDisciplina tud WITH(NOLOCK)
	WHERE
		tud.tud_id = @tud_id
		AND tud.tud_situacao <> 3
	
	DECLARE @PermissaoDocenteEdicao TABLE (tdt_posicaoPermissao int);
	
	INSERT INTO @PermissaoDocenteEdicao (tdt_posicaoPermissao)
    SELECT 
		tdcPermissao.tdc_posicao AS tdt_posicaoPermissao
	FROM
		ACA_TipoDocente tdc WITH(NOLOCK)
		INNER JOIN CFG_PermissaoDocente pdc WITH(NOLOCK)
			ON tdc.tdc_id = pdc.tdc_id
			AND pdc.pdc_modulo = 1
			AND pdc.pdc_permissaoEdicao = 1  -- retonar as permissões de edição
			AND pdc.pdc_situacao <> 3 
		INNER JOIN ACA_TipoDocente tdcPermissao WITH(NOLOCK)
			ON tdcPermissao.tdc_id = pdc.tdc_idPermissao
			AND tdcPermissao.tdc_situacao <> 3
	WHERE
		tdc.tdc_posicao = ISNULL(@tdt_posicao, tdc.tdc_posicao)
		AND tdc.tdc_situacao <> 3
		AND tdc.tdc_id IN (1,6) --Titular / Segundo titular

	IF (@tud_tipo = 15)
	BEGIN
	
		SELECT
			Tnt.tud_id
			, Tnt.tnt_id
			--, CASE WHEN (tnt_nome IS NULL) or (tnt_nome = '') then tav_nome ELSE tnt_nome END AS tnt_nome
			--, CASE WHEN Tnt.tav_id IS NULL THEN tnt_nome ELSE tav_nome END AS tnt_nome
			, CASE WHEN (tnt_nome IS NULL) OR (tnt_nome = '') THEN
				CASE WHEN (Tnt.tav_id IS NULL) 
					THEN 'Outro tipo de atividade avaliativa' 
					ELSE tav_nome
				END
			  ELSE tnt_nome
			END AS tnt_nome			
			, Tnt.tdt_posicao
			, ISNULL((CASE WHEN ISNULL(LtnL.ltn_data, tnt_dataAlteracao) > ISNULL(Ltn.ltn_data, tnt_dataAlteracao)
						   THEN ISNULL(LtnL.usu_id, tnt.usu_id)
						   ELSE ISNULL(Ltn.usu_id, tnt.usu_id) END), Tnt.usu_id) AS usu_id
			, Tnt.pro_id
			, Tnt.tnt_chaveDiario
			, CONVERT(VARCHAR(10), ISNULL(tau_data, tnt_data), 103) AS tnt_data
			, Tna.tna_avaliacao
			, Tna.tna_relatorio		
			
			-- (ALUNO AUSENTE) 1 - aluno ausente que não possui falta justificada 
			, CAST( 
				CASE WHEN Fav.fav_tipoLancamentoFrequencia IN ( 1, 4, 5, 6) AND (Tau.tau_id IS NOT NULL) THEN
				-- Aulas planejadas
					CASE 
						WHEN  
							(
								(SELECT 
									taa_frequencia 
								 FROM 
									CLS_TurmaAulaAluno WITH(NOLOCK)
								 WHERE 
									tud_id = Tau.tud_id
									AND tau_id = Tau.tau_id
									AND alu_id = Mtd.alu_id
									AND mtu_id = Mtd.mtu_id
									AND mtd_id = Mtd.mtd_id)
							 = (
								-- Verifica o tipo de apuração de frequenacia
								CASE Fav.fav_tipoApuracaoFrequencia
									WHEN 1 THEN -- 1: Tempos de aula
										  Tau.tau_numeroAulas
									ELSE -- 2: Dia
										1
								END
								)
							)
								AND ( Ajf.afj_id IS NULL OR Tjf.tjf_abonaFalta <> 1)
						THEN 1 ELSE 0 
					END
					-- Período ( Nâo terá esta funcionalidade )
				ELSE 0				       
			END AS BIT) AS aluno_ausente 
			-- (FALTA JUSTIFICADA) 0 - não possui FJ / 1 - possui FJ que abona / 2 - possui FJ que não abona 
			, CASE WHEN Ajf.afj_id IS NULL
					THEN '0'					    						    
					ELSE (CASE WHEN Tjf.tjf_abonaFalta = 1 THEN '1' ELSE '2' END)
			   END AS falta_justificada
			, Tnt.tnt_efetivado
			, Mtd.alu_id
			, Mtd.mtu_id
			, Mtd.mtd_id
			, Tnt.tnt_exclusiva
			, ISNULL(Tna.tna_participante, 0) AS tna_participante
			, CAST(0 AS BIT) AS AlunoDispensado
			
			, CASE WHEN @usu_id IS NULL OR COALESCE(Ltn.usu_id, Tnt.usu_id, @usu_id) = @usu_id THEN 1
			  	   WHEN ISNULL(pde.tdt_posicaoPermissao, 0) > 0 THEN 1 
				   ELSE 0 
			  END AS permissaoAlteracao
			     
			, Mtd.mtd_situacao
			
		    , ISNULL(pesAlteracao.pes_nome,usuAlteracao.usu_Login) as nomeUsuAlteracao -- inserido para poder exibir o usuário que alterou os dados 
		    , ISNULL((CASE WHEN ISNULL(LtnL.ltn_data, tnt_dataAlteracao) > ISNULL(Ltn.ltn_data, tnt_dataAlteracao)
						   THEN ISNULL(LtnL.ltn_data, tnt.tnt_dataAlteracao)
						   ELSE ISNULL(Ltn.ltn_data, tnt.tnt_dataAlteracao) END), tnt_dataAlteracao) AS tnt_dataAlteracao -- inserido para poder exibir a data que o usuário realizou a alteração
		    , CAST(
						CASE WHEN ISNULL(Tna.alu_id, 0) = Mtd.alu_id
							THEN 1
							ELSE 0
						END AS BIT) AS PossuiNota
			, Tnt.tau_id
		FROM CLS_TurmaNota AS Tnt WITH (NOLOCK)	
		INNER JOIN TUR_TurmaDisciplina AS Tud WITH (NOLOCK)	
			ON Tnt.tud_id = Tud.tud_id
		INNER JOIN TUR_TurmaRelTurmaDisciplina AS RelTud WITH (NOLOCK)
			ON RelTud.tud_id = Tud.tud_id
		INNER JOIN TUR_Turma AS Tur WITH(NOLOCK)
			ON Tur.tur_id = RelTud.tur_id
		INNER JOIN ACA_FormatoAvaliacao Fav WITH(NOLOCK)
			ON Fav.fav_id = Tur.fav_id
		INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
			ON Cap.cal_id = Tur.cal_id 
			AND Cap.tpc_id = Tnt.tpc_id
			AND Cap.cap_situacao <> 3
		INNER JOIN MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
			ON Mtd.tud_id = Tnt.tud_id
			AND Mtd.mtd_situacao IN (1,5)
		INNER JOIN MTR_MatriculaTurma mtu WITH(NOLOCK)
			ON mtu.alu_id = Mtd.alu_id
			AND mtu.mtu_id = Mtd.mtu_id
			AND mtu.mtu_situacao <> 3
		INNER JOIN @dtTurmas dtt
			ON mtu.tur_id = dtt.tur_id
		LEFT JOIN CLS_TipoAtividadeAvaliativa AS Tav WITH (NOLOCK)
			ON Tav.tav_id = Tnt.tav_id
		LEFT OUTER JOIN CLS_TurmaNotaAluno Tna WITH(NOLOCK)
			ON Tna.tud_id = Tnt.tud_id
			AND Tna.tnt_id = Tnt.tnt_id
			AND Tna.alu_id = Mtd.alu_id
			AND Tna.mtu_id = Mtd.mtu_id
			AND Tna.mtd_id = Mtd.mtd_id
			AND Tna.tna_situacao <> 3
		LEFT JOIN CLS_TurmaAula Tau WITH (NOLOCK)
			ON Tau.tud_id = Tnt.tud_id
			AND Tau.tau_id = Tnt.tau_id
			AND tau.tpc_id = Tnt.tpc_id
			AND tau.tau_situacao <> 3
		LEFT JOIN ACA_AlunoJustificativaFalta Ajf WITH (NOLOCK)
			ON Ajf.alu_id = Mtd.alu_id
			AND Ajf.afj_situacao <> 3
			AND tau_data >= Ajf.afj_dataInicio
			AND (Ajf.afj_dataFim IS NULL OR (tau_data <= Ajf.afj_dataFim))
		LEFT JOIN ACA_TipoJustificativaFalta Tjf WITH (NOLOCK)
			 ON Tjf.tjf_id = Ajf.tjf_id
		LEFT JOIN @PermissaoDocenteEdicao AS pde
			ON Tau.tdt_posicao = pde.tdt_posicaoPermissao
		--- inserido para buscar o nome do ultimo usuario que alterou os dados		
		OUTER APPLY FN_RetornaUltimaAlteracaoTurmaNota(Tnt.tud_id, Tnt.tnt_id, 1) Ltn --Alteracao de atividade
		OUTER APPLY FN_RetornaUltimaAlteracaoTurmaNota(Tnt.tud_id, Tnt.tnt_id, 2) LtnL --Lançamento de notas
		LEFT JOIN Synonym_SYS_Usuario AS usuAlteracao WITH(NOLOCK)
			ON usuAlteracao.usu_id = ISNULL((CASE WHEN ISNULL(LtnL.ltn_data, tnt_dataAlteracao) > ISNULL(Ltn.ltn_data, tnt_dataAlteracao)
											      THEN ISNULL(LtnL.usu_id, tnt.usu_idDocenteAlteracao)
												  ELSE ISNULL(Ltn.usu_id, tnt.usu_idDocenteAlteracao) END), tnt.usu_idDocenteAlteracao)
			AND usuAlteracao.usu_situacao <> 3
		LEFT JOIN Synonym_PES_Pessoa AS pesAlteracao WITH(NOLOCK)
			ON usuAlteracao.pes_id = pesAlteracao.pes_id
			AND pesAlteracao.pes_situacao <> 3
		WHERE
			Tnt.tud_id = @tud_id
			AND Tnt.tpc_id = @tpc_id
			AND (tnt_situacao <> 3 and tnt_situacao <> 6)
			AND tud_situacao <> 3
			AND (Mtd.mtd_situacao <> 5 OR ISNULL((DATEDIFF(DAY, Mtd.mtd_dataSaida,ISNULL(tau_data, tnt_data))), 0) <= 0)

		GROUP BY Tnt.tud_id, Tnt.tnt_id, tnt_nome, tav_nome, Tnt.tav_id, Tnt.tdt_posicao, Tnt.pro_id, Tnt.tnt_chaveDiario
			, tau_data, tnt_data, Tna.tna_avaliacao, Tna.tna_relatorio, Tau.tud_id, Tau.tau_id, Mtd.alu_id
			, Mtd.mtu_id, Mtd.mtd_id, Fav.fav_tipoLancamentoFrequencia, Fav.fav_tipoApuracaoFrequencia
			, Tau.tau_numeroAulas, Ajf.afj_id, Tjf.tjf_abonaFalta, Tnt.tnt_efetivado, Tnt.tnt_exclusiva
			, Tna.tna_participante, Cap.cap_descricao, Ltn.usu_id, LtnL.usu_id, Tnt.usu_id, Tau.usu_id, Mtd.mtd_situacao
			, ISNULL(pesAlteracao.pes_nome,usuAlteracao.usu_Login), Ltn.ltn_data, LtnL.ltn_data, tnt_dataAlteracao, pde.tdt_posicaoPermissao, Tna.alu_id, Tnt.tau_id
		
		ORDER BY
			Cap.cap_descricao
			, [tnt_data]
			, tnt_nome
	END
	-- Se a disciplina não for um componente da regência, traz os dados normalmente
	ELSE IF (@tud_tipo <> 12)
	BEGIN
		SELECT
			Tnt.tud_id
			, Tnt.tnt_id
			--, CASE WHEN (tnt_nome IS NULL) or (tnt_nome = '') then tav_nome ELSE tnt_nome END AS tnt_nome
			--, CASE WHEN Tnt.tav_id IS NULL THEN tnt_nome ELSE tav_nome END AS tnt_nome
			, CASE WHEN (tnt_nome IS NULL) OR (tnt_nome = '') THEN
				CASE WHEN (Tnt.tav_id IS NULL) 
					THEN 'Outro tipo de atividade avaliativa' 
					ELSE tav_nome
				END
			  ELSE tnt_nome
			END AS tnt_nome			
			, Tnt.tdt_posicao
			, ISNULL((CASE WHEN ISNULL(LtnL.ltn_data, tnt_dataAlteracao) > ISNULL(Ltn.ltn_data, tnt_dataAlteracao)
						   THEN ISNULL(LtnL.usu_id, tnt.usu_id)
						   ELSE ISNULL(Ltn.usu_id, tnt.usu_id) END), Tnt.usu_id) AS usu_id
			, Tnt.pro_id
			, Tnt.tnt_chaveDiario
			, CONVERT(VARCHAR(10), ISNULL(tau_data, tnt_data), 103) AS tnt_data
			, Tna.tna_avaliacao
			, Tna.tna_relatorio		
			
			-- (ALUNO AUSENTE) 1 - aluno ausente que não possui falta justificada 
			, CAST( 
				CASE WHEN Fav.fav_tipoLancamentoFrequencia IN ( 1, 4, 5, 6) AND (Tau.tau_id IS NOT NULL) THEN
				-- Aulas planejadas
					CASE 
						WHEN  
							(
								(SELECT 
									taa_frequencia 
								 FROM 
									CLS_TurmaAulaAluno WITH(NOLOCK)
								 WHERE 
									tud_id = Tau.tud_id
									AND tau_id = Tau.tau_id
									AND alu_id = Mtd.alu_id
									AND mtu_id = Mtd.mtu_id
									AND mtd_id = Mtd.mtd_id)
							 = (
								-- Verifica o tipo de apuração de frequenacia
								CASE Fav.fav_tipoApuracaoFrequencia
									WHEN 1 THEN -- 1: Tempos de aula
										  Tau.tau_numeroAulas
									ELSE -- 2: Dia
										1
								END
								)
							)
								AND ( Ajf.afj_id IS NULL OR Tjf.tjf_abonaFalta <> 1)
						THEN 1 ELSE 0 
					END
					-- Período ( Nâo terá esta funcionalidade )
				ELSE 0				       
			END AS BIT) AS aluno_ausente 
			-- (FALTA JUSTIFICADA) 0 - não possui FJ / 1 - possui FJ que abona / 2 - possui FJ que não abona 
			, CASE WHEN Ajf.afj_id IS NULL
					THEN '0'					    						    
					ELSE (CASE WHEN Tjf.tjf_abonaFalta = 1 THEN '1' ELSE '2' END)
			   END AS falta_justificada
			, Tnt.tnt_efetivado
			, Mtd.alu_id
			, Mtd.mtu_id
			, Mtd.mtd_id
			, Tnt.tnt_exclusiva
			, ISNULL(Tna.tna_participante, 0) AS tna_participante
			, CAST(0 AS BIT) AS AlunoDispensado
			
			--, (CASE WHEN @usu_id is null or ISNULL(Tnt.usu_id, @usu_id) = @usu_id THEN 1 ELSE 0 END) AS permissaoAlteracao
			
			, CASE WHEN @usu_id IS NULL OR COALESCE(Ltn.usu_id, Tnt.usu_id, @usu_id) = @usu_id THEN 1
			  	    WHEN ISNULL(pde.tdt_posicaoPermissao, 0) > 0 THEN 1 
					ELSE 0 
			  END AS permissaoAlteracao
			
			, Mtd.mtd_situacao
			
		    , ISNULL(pesAlteracao.pes_nome,usuAlteracao.usu_Login) as nomeUsuAlteracao -- inserido para poder exibir o usuário que alterou os dados
		    , ISNULL((CASE WHEN ISNULL(LtnL.ltn_data, tnt_dataAlteracao) > ISNULL(Ltn.ltn_data, tnt_dataAlteracao)
						   THEN ISNULL(LtnL.ltn_data, tnt.tnt_dataAlteracao)
						   ELSE ISNULL(Ltn.ltn_data, tnt.tnt_dataAlteracao) END), tnt_dataAlteracao) AS tnt_dataAlteracao -- inserido para poder exibir a data que o usuário realizou a alteração
		    , CAST(
						CASE WHEN ISNULL(Tna.alu_id, 0) = Mtd.alu_id
							THEN 1
							ELSE 0
						END AS BIT) AS PossuiNota
			, Tnt.tau_id
		FROM CLS_TurmaNota AS Tnt WITH (NOLOCK)	
		INNER JOIN TUR_TurmaDisciplina AS Tud WITH (NOLOCK)	
			ON Tnt.tud_id = Tud.tud_id
		INNER JOIN TUR_TurmaRelTurmaDisciplina AS RelTud WITH (NOLOCK)
			ON RelTud.tud_id = Tud.tud_id
		INNER JOIN TUR_Turma AS Tur WITH(NOLOCK)
			ON Tur.tur_id = RelTud.tur_id
		INNER JOIN ACA_FormatoAvaliacao Fav WITH(NOLOCK)
			ON Fav.fav_id = Tur.fav_id
		INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
			ON Cap.cal_id = Tur.cal_id 
			AND Cap.tpc_id = Tnt.tpc_id
			AND Cap.cap_situacao <> 3
		-- Trazer todos os alunos matriculados na disciplina.
		INNER JOIN MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
			ON Mtd.tud_id = Tnt.tud_id
			AND Mtd.mtd_situacao IN (1,5)

		LEFT JOIN CLS_TipoAtividadeAvaliativa AS Tav WITH (NOLOCK)
			ON Tav.tav_id = Tnt.tav_id
		LEFT OUTER JOIN CLS_TurmaNotaAluno Tna WITH(NOLOCK)
			ON Tna.tud_id = Tnt.tud_id
			AND Tna.tnt_id = Tnt.tnt_id
			AND Tna.alu_id = Mtd.alu_id
			AND Tna.mtu_id = Mtd.mtu_id
			AND Tna.mtd_id = Mtd.mtd_id
			AND Tna.tna_situacao <> 3
		LEFT JOIN CLS_TurmaAula Tau WITH (NOLOCK)
			ON Tau.tud_id = Tnt.tud_id
			AND Tau.tau_id = Tnt.tau_id
			AND tau.tpc_id = Tnt.tpc_id
			AND tau.tau_situacao <> 3
		LEFT JOIN ACA_AlunoJustificativaFalta Ajf WITH (NOLOCK)
			ON Ajf.alu_id = Mtd.alu_id
			AND Ajf.afj_situacao <> 3
			AND tau_data >= Ajf.afj_dataInicio
			AND (Ajf.afj_dataFim IS NULL OR (tau_data <= Ajf.afj_dataFim))
		LEFT JOIN ACA_TipoJustificativaFalta Tjf WITH (NOLOCK)
			 ON Tjf.tjf_id = Ajf.tjf_id
		LEFT JOIN @PermissaoDocenteEdicao AS pde
			ON Tau.tdt_posicao = pde.tdt_posicaoPermissao
		--- inserido para buscar o nome do ultimo usuario que alterou os dados		
		OUTER APPLY FN_RetornaUltimaAlteracaoTurmaNota(Tnt.tud_id, Tnt.tnt_id, 1) Ltn --Alteracao de atividade
		OUTER APPLY FN_RetornaUltimaAlteracaoTurmaNota(Tnt.tud_id, Tnt.tnt_id, 2) LtnL --Lançamento de notas
		LEFT JOIN Synonym_SYS_Usuario AS usuAlteracao WITH(NOLOCK)
			ON usuAlteracao.usu_id = ISNULL((CASE WHEN ISNULL(LtnL.ltn_data, tnt_dataAlteracao) > ISNULL(Ltn.ltn_data, tnt_dataAlteracao)
											      THEN ISNULL(LtnL.usu_id, tnt.usu_idDocenteAlteracao)
												  ELSE ISNULL(Ltn.usu_id, tnt.usu_idDocenteAlteracao) END), tnt.usu_idDocenteAlteracao)
			AND usuAlteracao.usu_situacao <> 3
		LEFT JOIN Synonym_PES_Pessoa AS pesAlteracao WITH(NOLOCK)
			ON usuAlteracao.pes_id = pesAlteracao.pes_id
			AND pesAlteracao.pes_situacao <> 3
		WHERE
			Tnt.tud_id = @tud_id
			AND Tnt.tpc_id = @tpc_id
			AND (tnt_situacao <> 3 and tnt_situacao <> 6)
			AND tud_situacao <> 3
		
		GROUP BY Tnt.tud_id, Tnt.tnt_id, tnt_nome, tav_nome, Tnt.tav_id, Tnt.tdt_posicao, Tnt.pro_id, Tnt.tnt_chaveDiario
			, tau_data, tnt_data, Tna.tna_avaliacao, Tna.tna_relatorio, Tau.tud_id, Tau.tau_id, Mtd.alu_id
			, Mtd.mtu_id, Mtd.mtd_id, Fav.fav_tipoLancamentoFrequencia, Fav.fav_tipoApuracaoFrequencia
			, Tau.tau_numeroAulas, Ajf.afj_id, Tjf.tjf_abonaFalta, Tnt.tnt_efetivado, Tnt.tnt_exclusiva
			, Tna.tna_participante, Cap.cap_descricao, Ltn.usu_id, LtnL.usu_id, Tnt.usu_id, Tau.usu_id, Mtd.mtd_situacao
			, ISNULL(pesAlteracao.pes_nome,usuAlteracao.usu_Login), Ltn.ltn_data, LtnL.ltn_data, tnt_dataAlteracao, pde.tdt_posicaoPermissao, Tna.alu_id, Tnt.tau_id
		
		ORDER BY
			Cap.cap_descricao
			, [tnt_data]
			, tnt_nome
	END
	ELSE
	
	-- Se for um componente da regência, traz os dados baseados nas aulas da regência
	BEGIN
		DECLARE @tud_idRegencia BIGINT = NULL;

		-- Caso seja Componente de regência pega o tud_id da regência
		SELECT 
			@tud_idRegencia = TUR_TUD_REG.tud_id
		FROM 
			dbo.TUR_TurmaDisciplina TUD WITH(NOLOCK)
			INNER JOIN dbo.TUR_TurmaDisciplinaRelDisciplina TUD_DIS WITH(NOLOCK)
				ON	TUD_DIS.tud_id = TUD.tud_id
			INNER JOIN dbo.TUR_TurmaRelTurmaDisciplina TUR_TUD WITH(NOLOCK)
				ON	TUR_TUD.tud_id = TUD.tud_id
			INNER JOIN dbo.ACA_CurriculoDisciplina CRD WITH(NOLOCK)
				ON	CRD.dis_id = TUD_DIS.dis_id
			INNER JOIN dbo.ACA_CurriculoDisciplina CRDREG WITH(NOLOCK)
				ON	CRDREG.cur_id = CRD.cur_id
				AND CRDREG.crr_id = CRD.crr_id
				AND CRDREG.crp_id = CRD.crp_id
			INNER JOIN dbo.TUR_TurmaDisciplinaRelDisciplina TUD_DIS_REG WITH(NOLOCK)
				ON	TUD_DIS_REG.dis_id = CRDREG.dis_id
			INNER JOIN dbo.TUR_TurmaRelTurmaDisciplina TUR_TUD_REG WITH(NOLOCK)
				ON	TUR_TUD_REG.tud_id = TUD_DIS_REG.tud_id
				AND TUR_TUD_REG.tur_id = TUR_TUD.tur_id
		WHERE 
			TUD.tud_id = @tud_id
			AND TUD.tud_tipo = 12
			AND CRDREG.crd_tipo = 11
			-- Exclusão Lógica
			AND TUD.tud_situacao <> 3
			AND CRD.crd_situacao <> 3
			AND CRDREG.crd_situacao <> 3

		-- Aulas da regência
		; WITH tbAulas AS 
		(
			SELECT 
				tau.tud_id,
				tau.tau_id,
				tau.tau_data,
				tau.tau_numeroAulas,
				tau.usu_id,
				tau.tdt_posicao
			FROM 
				CLS_TurmaAula tau WITH(NOLOCK)
			WHERE
				tau.tud_id = @tud_idRegencia
				AND tau.tpc_id = @tpc_id
				AND tau.tau_situacao <> 3
				AND Tau.tdt_posicao = ISNULL(@tdt_posicao, Tau.tdt_posicao) -- inserido para corrigir duplicidade em ativ. avaliativa
		
		), tbNotas AS
		(		
			SELECT
				Tnt.tud_id
				, Tnt.tnt_id
				--, CASE WHEN (tnt_nome IS NULL) or (tnt_nome = '') then tav_nome ELSE tnt_nome END AS tnt_nome
				--, CASE WHEN Tnt.tav_id IS NULL THEN tnt_nome ELSE tav_nome END AS tnt_nome
				, CASE WHEN (tnt_nome IS NULL) OR (tnt_nome = '') THEN
					CASE WHEN (Tnt.tav_id IS NULL) 
						THEN 'Outro tipo de atividade avaliativa' 
						ELSE tav_nome
					END
				  ELSE tnt_nome
				END AS tnt_nome					
				, Tnt.tdt_posicao
				, ISNULL((CASE WHEN ISNULL(LtnL.ltn_data, tnt_dataAlteracao) > ISNULL(Ltn.ltn_data, tnt_dataAlteracao)
							   THEN ISNULL(LtnL.usu_id, tnt.usu_id)
							   ELSE ISNULL(Ltn.usu_id, tnt.usu_id) END), Tnt.usu_id) AS usu_id
				, Tnt.pro_id
				, Tnt.tnt_chaveDiario
				, CONVERT(VARCHAR(10), ISNULL(tau_data, tnt_data), 103) AS tnt_data
				, Tna.tna_avaliacao
				, Tna.tna_relatorio		
				
				-- (ALUNO AUSENTE) 1 - aluno ausente que não possui falta justificada 
				, CAST( 
					CASE WHEN Fav.fav_tipoLancamentoFrequencia IN (1, 4, 5, 6) AND (Tau.tau_id IS NOT NULL) THEN
					-- Aulas planejadas
						CASE 
							WHEN  
								(
									(SELECT TOP 1 TAA.taa_frequencia 
									   FROM CLS_TurmaAula TAUL WITH(NOLOCK)
									        INNER JOIN CLS_TurmaAulaAluno TAA WITH(NOLOCK)
										     ON TAUL.tud_id = TAA.tud_id
										    AND TAUL.tau_id = TAA.tau_id
										    AND Mtd.alu_id = TAA.alu_id
										    AND Mtd.mtu_id = TAA.mtu_id
									  WHERE  TAUL.tud_id = @tud_idRegencia
										AND TAUL.tau_data = ISNULL(TAU.tau_data,Tnt.tnt_data))
								 = (
									-- Verifica o tipo de apuração de frequenacia
									CASE Fav.fav_tipoApuracaoFrequencia
										WHEN 1 THEN -- 1: Tempos de aula
											  Tau.tau_numeroAulas
										ELSE -- 2: Dia
											1
									END
									)
								)
									AND ( Ajf.afj_id IS NULL OR Tjf.tjf_abonaFalta <> 1)
							THEN 1 ELSE 0 
						END
						-- Período ( Nâo terá esta funcionalidade )
					ELSE 0				       
				END AS BIT) AS aluno_ausente 
				-- (FALTA JUSTIFICADA) 0 - não possui FJ / 1 - possui FJ que abona / 2 - possui FJ que não abona 
				, CASE WHEN Ajf.afj_id IS NULL
						THEN '0'					    						    
						ELSE (CASE WHEN Tjf.tjf_abonaFalta = 1 THEN '1' ELSE '2' END)
				   END AS falta_justificada
				, Tnt.tnt_efetivado
				, Mtd.alu_id
				, Mtd.mtu_id
				, Mtd.mtd_id
				, Tnt.tnt_exclusiva
				, ISNULL(Tna.tna_participante, 0) AS tna_participante
				
				, CAST(0 AS BIT) AS AlunoDispensado
				
				, (CASE WHEN @usu_id IS NULL OR COALESCE(Ltn.usu_id, Tnt.usu_id, @usu_id) = @usu_id THEN 1 ELSE 0 END) AS permissaoAlteracao

				, Mtd.mtd_situacao
				
				, ISNULL(pesAlteracao.pes_nome,usuAlteracao.usu_Login) as nomeUsuAlteracao -- inserido para poder exibir o usuário que alterou os dados
		    , ISNULL((CASE WHEN ISNULL(LtnL.ltn_data, tnt_dataAlteracao) > ISNULL(Ltn.ltn_data, tnt_dataAlteracao)
						   THEN ISNULL(LtnL.ltn_data, tnt.tnt_dataAlteracao)
						   ELSE ISNULL(Ltn.ltn_data, tnt.tnt_dataAlteracao) END), tnt_dataAlteracao) AS tnt_dataAlteracao -- inserido para poder exibir a data que o usuário realizou a alteração
				, CAST(
						CASE WHEN ISNULL(Tna.alu_id, 0) = Mtd.alu_id
							THEN 1
							ELSE 0
						END AS BIT) AS PossuiNota
				, Tnr.tau_idAula AS tau_id
			FROM CLS_TurmaNota AS Tnt WITH (NOLOCK)	
			INNER JOIN TUR_TurmaDisciplina AS Tud WITH (NOLOCK)	
				ON Tnt.tud_id = Tud.tud_id
			INNER JOIN TUR_TurmaRelTurmaDisciplina AS RelTud WITH (NOLOCK)
				ON RelTud.tud_id = Tud.tud_id
			INNER JOIN TUR_Turma AS Tur WITH(NOLOCK)
				ON Tur.tur_id = RelTud.tur_id
			INNER JOIN ACA_FormatoAvaliacao Fav WITH(NOLOCK)
				ON Fav.fav_id = Tur.fav_id
			INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
				ON Cap.cal_id = Tur.cal_id 
				AND Cap.tpc_id = Tnt.tpc_id
				AND Cap.cap_situacao <> 3
			-- Trazer todos os alunos matriculados na disciplina.
			INNER JOIN MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
				ON Mtd.tud_id = Tnt.tud_id
				AND Mtd.mtd_situacao IN (1,5)
			LEFT JOIN CLS_TipoAtividadeAvaliativa AS Tav WITH (NOLOCK)
				ON Tav.tav_id = Tnt.tav_id
			LEFT JOIN CLS_TurmaNotaRegencia AS Tnr WITH(NOLOCK)
				ON Tnt.tud_id = Tnr.tud_id 
				AND Tnt.tnt_id = Tnr.tnt_id
			LEFT OUTER JOIN CLS_TurmaNotaAluno Tna WITH(NOLOCK)
				ON Tna.tud_id = Tnt.tud_id
				AND Tna.tnt_id = Tnt.tnt_id
				AND Tna.alu_id = Mtd.alu_id
				AND Tna.mtu_id = Mtd.mtu_id
				AND Tna.mtd_id = Mtd.mtd_id
				AND Tna.tna_situacao <> 3
			LEFT JOIN tbAulas Tau WITH (NOLOCK)
				ON Tau.tau_data = Tnt.tnt_data
			LEFT JOIN ACA_AlunoJustificativaFalta Ajf WITH (NOLOCK)
				ON Ajf.alu_id = Mtd.alu_id
				AND Ajf.afj_situacao <> 3
				AND tau_data >= Ajf.afj_dataInicio
				AND (Ajf.afj_dataFim IS NULL OR (tau_data <= Ajf.afj_dataFim))
			LEFT JOIN ACA_TipoJustificativaFalta Tjf WITH (NOLOCK)
				 ON Tjf.tjf_id = Ajf.tjf_id
			LEFT JOIN @PermissaoDocenteEdicao AS pde 
				ON Tau.tdt_posicao = pde.tdt_posicaoPermissao
			--- inserido para buscar o nome do ultimo usuario que alterou os dados		
			OUTER APPLY FN_RetornaUltimaAlteracaoTurmaNota(Tnt.tud_id, Tnt.tnt_id, 1) Ltn --Alteracao de atividade
			OUTER APPLY FN_RetornaUltimaAlteracaoTurmaNota(Tnt.tud_id, Tnt.tnt_id, 2) LtnL --Lançamento de notas
			LEFT JOIN Synonym_SYS_Usuario AS usuAlteracao WITH(NOLOCK)
				ON usuAlteracao.usu_id = ISNULL((CASE WHEN ISNULL(LtnL.ltn_data, tnt_dataAlteracao) > ISNULL(Ltn.ltn_data, tnt_dataAlteracao)
														THEN ISNULL(LtnL.usu_id, tnt.usu_idDocenteAlteracao)
														ELSE ISNULL(Ltn.usu_id, tnt.usu_idDocenteAlteracao) END), tnt.usu_idDocenteAlteracao)
				AND usuAlteracao.usu_situacao <> 3
			LEFT JOIN Synonym_PES_Pessoa AS pesAlteracao WITH(NOLOCK)
				ON usuAlteracao.pes_id = pesAlteracao.pes_id
				AND pesAlteracao.pes_situacao <> 3
			WHERE
				Tnt.tud_id = @tud_id
				AND Tnt.tpc_id = @tpc_id
				AND (tnt_situacao <> 3 and tnt_situacao <> 6)
				AND tud_situacao <> 3
			
			--GROUP BY Tnt.tud_id, Tnt.tnt_id, tnt_nome, tav_nome, Tnt.tav_id, Tnt.tdt_posicao
			--	, tau_data, tnt_data, Tna.tna_avaliacao, Tna.tna_relatorio, Tau.tud_id, Tau.tau_id, Mtd.alu_id
			--	, Mtd.mtu_id, Mtd.mtd_id, Fav.fav_tipoLancamentoFrequencia, Fav.fav_tipoApuracaoFrequencia
			--	, Tau.tau_numeroAulas, Ajf.afj_id, Tjf.tjf_abonaFalta, Tnt.tnt_efetivado, Tnt.tnt_exclusiva
			--	, Tna.tna_participante, Dda.dda_id, Cap.cap_descricao
			
			--ORDER BY
			--	Cap.cap_descricao
			--	, [tnt_data]
			--	, tnt_nome
		)
		
		SELECT tud_id, tnt_id, tnt_nome, tdt_posicao, usu_id, pro_id, tnt_chaveDiario, tnt_data, tna_avaliacao, tna_relatorio
			, aluno_ausente, falta_justificada, tnt_efetivado, alu_id, mtu_id, mtd_id, tnt_exclusiva
			, tna_participante, AlunoDispensado, permissaoAlteracao, mtd_situacao, nomeUsuAlteracao, tnt_dataAlteracao, PossuiNota, tau_id
		FROM tbNotas
		GROUP BY tud_id, tnt_id, tnt_nome, tdt_posicao, usu_id, pro_id, tnt_chaveDiario, tnt_data, tna_avaliacao, tna_relatorio
			, aluno_ausente, falta_justificada, tnt_efetivado, alu_id, mtu_id, mtd_id, tnt_exclusiva
			, tna_participante, AlunoDispensado, permissaoAlteracao, mtd_situacao, nomeUsuAlteracao, tnt_dataAlteracao, PossuiNota, tau_id
		ORDER BY tnt_data, tnt_nome
		
	END
END
GO
PRINT N'Creating trigger [dbo].[TRG_ACA_AlunoJustificativaFalta_Identity] on [dbo].[ACA_AlunoJustificativaFalta]'
GO
-- ========================================================
-- Author:		Aline Dornelas
-- Create date: 22/07/2011 11:39
-- Description:	Trigger para autoincremento do campo afj_id 
--				baseado no alu_id
-- ========================================================
CREATE TRIGGER [dbo].[TRG_ACA_AlunoJustificativaFalta_Identity]
	ON [dbo].[ACA_AlunoJustificativaFalta] INSTEAD OF INSERT
AS
BEGIN

	-- Recuperar o último id + 1 para autoincremento.
	DECLARE @ID INT
	
	SELECT 
		@ID = ISNULL(MAX(ACA_AlunoJustificativaFalta.afj_id), 0) + 1
	FROM 
		ACA_AlunoJustificativaFalta WITH (UPDLOCK, HOLDLOCK)
	INNER JOIN inserted
		ON ACA_AlunoJustificativaFalta.alu_id = inserted.alu_id
		
	-- Insere o registro com id do autoincremento.	
	INSERT INTO ACA_AlunoJustificativaFalta 
		(
			alu_id
			, afj_id
			, tjf_id
			, afj_dataInicio
			, afj_dataFim
			, afj_situacao
			, afj_dataCriacao
			, afj_dataAlteracao
			, pro_id 
			, afj_observacao
		)
    SELECT 
			alu_id
			, @ID
			, tjf_id
			, afj_dataInicio
			, afj_dataFim
			, afj_situacao
			, afj_dataCriacao
			, afj_dataAlteracao
			, pro_id
			, afj_observacao
	FROM inserted
		
	SELECT ISNULL(@ID,-1)								
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
PRINT N'Altering [dbo].[NEW_CLS_TurmaAulaAluno_SelectFreqDisciplinaByDisciplinaPeriodoData]'
GO
-- =================================================
-- Author:		Renata Tiepo Fonseca
-- Create date: 10/02/2012
-- Description:	Retorna as frequências dos alunos 
--				matriculados na disciplina
--				e períodos selecionados.
-- =================================================
ALTER PROCEDURE [dbo].[NEW_CLS_TurmaAulaAluno_SelectFreqDisciplinaByDisciplinaPeriodoData]
	@tur_id BIGINT 
	, @tpc_id INT 
	, @dataInicio DATETIME 
	, @dataFim DATETIME 
AS
BEGIN	
	-- Guardar todos os tud_ids da turma
	DECLARE @TabelaTudID TABLE (tud_id BIGINT NOT NULL)
	
	INSERT INTO @TabelaTudID (tud_id)
	-- Trazer todos os tud_id
	SELECT 
		Tds.tud_id 
	FROM TUR_TurmaRelTurmaDisciplina RelTur WITH(NOLOCK)
	INNER JOIN TUR_TurmaDisciplina Tds WITH(NOLOCK)
		ON (Tds.tud_id = RelTur.tud_id)
			AND (Tds.tud_situacao <> 3)
	WHERE
		RelTur.tur_id = @tur_id
		
	;WITH tbFrequencia AS
	(
		SELECT 
			 Taa.alu_id
			, Taa.mtd_id
			, Taa.mtu_id
			, MIN(ISNULL(taa_frequencia, 0)) frequencia
		FROM CLS_TurmaAulaAluno Taa WITH(NOLOCK)
		INNER JOIN @TabelaTudID Tud
			ON Taa.tud_id = Tud.tud_id
		INNER JOIN CLS_TurmaAula Tau WITH(NOLOCK)
			ON Tau.tud_id = Taa.tud_id
			AND Tau.tau_id = Taa.tau_id
			AND Tau.tau_situacao <> 3
			AND (Tau.tau_data >= @dataInicio AND Tau.tau_data <= @dataFim)
		LEFT JOIN ACA_AlunoJustificativaFalta afj WITH(NOLOCK)
			ON Taa.alu_id = afj.alu_id
			AND afj.afj_situacao <> 3
			AND tau_data >= afj.afj_dataInicio
			AND ((afj.afj_dataFim  IS NULL) OR (tau_data <= afj.afj_dataFim))
		LEFT JOIN ACA_TipoJustificativaFalta tjf WITH(NOLOCK)
			ON tjf.tjf_id = afj.tjf_id
			AND tjf.tjf_situacao <> 3
		WHERE 
			Taa.taa_situacao <> 3
			AND Tau.tpc_id = @tpc_id
			AND ((tjf.tjf_abonaFalta IS NULL) OR (tjf.tjf_abonaFalta = 0))
		GROUP BY
			Tau.tau_data
			, Taa.alu_id
			, Taa.mtd_id
			, Taa.mtu_id
	)
			
	SELECT
		Mtd.alu_id
		, Mtd.mtu_id
		, Mtd.mtd_id
		, Mtd.tud_id
		, SUM(Freq.frequencia) frequencia
	FROM MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
	INNER JOIN @TabelaTudID Tud
		ON Mtd.tud_id = Tud.tud_id
	LEFT JOIN tbFrequencia Freq
		ON  Freq.alu_id = Mtd.alu_id
		AND Freq.mtu_id = Mtd.mtu_id
		AND Freq.mtd_id = Mtd.mtd_id
	WHERE 
		mtd_situacao IN (1,5)
	GROUP BY
		Mtd.alu_id
		, Mtd.mtu_id
		, Mtd.mtd_id
		, Mtd.tud_id
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
PRINT N'Creating [dbo].[NEW_ACA_AlunoJustificativaFalta_UPDATE]'
GO
-- ========================================================================
-- Author:		Aline Dornelas
-- Create date: 22/07/2011 12:06
-- Description:	Altera o registro preservando a data da criação
-- ========================================================================
CREATE PROCEDURE [dbo].[NEW_ACA_AlunoJustificativaFalta_UPDATE]
	@alu_id BIGINT
	, @afj_id INT
	, @tjf_id INT
	, @afj_dataInicio DATETIME
	, @afj_dataFim DATETIME
	, @afj_situacao TINYINT
	, @afj_dataAlteracao DATETIME
	, @afj_observacao VARCHAR(MAX)
AS
BEGIN

	UPDATE ACA_AlunoJustificativaFalta 
	SET 
		tjf_id = @tjf_id 
		, afj_dataInicio = @afj_dataInicio 
		, afj_dataFim = @afj_dataFim 
		, afj_situacao = @afj_situacao 
		, afj_dataAlteracao = @afj_dataAlteracao 
		, afj_observacao = @afj_observacao
	WHERE 
		alu_id = @alu_id 
		AND afj_id = @afj_id 

		
	DECLARE @ret INT
	SELECT @ret = ISNULL(@@ROWCOUNT,-1)
	RETURN @ret
	
END
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
PRINT N'Creating [dbo].[STP_ACA_AlunoJustificativaFalta_INSERT]'
GO

CREATE PROCEDURE [dbo].[STP_ACA_AlunoJustificativaFalta_INSERT]
	@alu_id BigInt
	, @afj_id Int
	, @tjf_id Int
	, @afj_dataInicio DateTime
	, @afj_dataFim DateTime
	, @afj_situacao TinyInt
	, @afj_dataCriacao DateTime
	, @afj_dataAlteracao DateTime
	, @pro_id UniqueIdentifier
	, @afj_observacao varchar(max)

AS
BEGIN
	INSERT INTO 
		ACA_AlunoJustificativaFalta
		( 
			alu_id 
			, afj_id 
			, tjf_id 
			, afj_dataInicio 
			, afj_dataFim 
			, afj_situacao 
			, afj_dataCriacao 
			, afj_dataAlteracao 
			, pro_id 
			, afj_observacao
 
		)
	VALUES
		( 
			@alu_id 
			, @afj_id 
			, @tjf_id 
			, @afj_dataInicio 
			, @afj_dataFim 
			, @afj_situacao 
			, @afj_dataCriacao 
			, @afj_dataAlteracao 
			, @pro_id 
			, @afj_observacao
 
		)
		
		SELECT ISNULL(SCOPE_IDENTITY(),-1)

	
	
END

GO
PRINT N'Adding foreign keys to [dbo].[ACA_AlunoJustificativaFalta]'
GO
ALTER TABLE [dbo].[ACA_AlunoJustificativaFalta] ADD CONSTRAINT [FK_ACA_AlunoJustificativaFalta_ACA_Aluno] FOREIGN KEY ([alu_id]) REFERENCES [dbo].[ACA_Aluno] ([alu_id])
GO
ALTER TABLE [dbo].[ACA_AlunoJustificativaFalta] ADD CONSTRAINT [FK_ACA_AlunoJustificativaFalta_ACA_TipoJustificativaFalta] FOREIGN KEY ([tjf_id]) REFERENCES [dbo].[ACA_TipoJustificativaFalta] ([tjf_id])
GO
ALTER TABLE [dbo].[ACA_AlunoJustificativaFalta] ADD CONSTRAINT [FK_ACA_AlunoJustificativaFalta_DCL_Protocolo] FOREIGN KEY ([pro_id]) REFERENCES [dbo].[DCL_Protocolo] ([pro_id])
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