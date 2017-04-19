/*------------------------------------------------------------------------------------
   Project/Ticket#: SGP
   Description: Atualiza o schema do banco de dados GestaoPedagogica da 1.68.0.0 para 1.69.0.0
-------------------------------------------------------------------------------------*/

SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Creating [dbo].[ACA_AlunoJustificativaFaltaAnexo]'
GO
CREATE TABLE [dbo].[ACA_AlunoJustificativaFaltaAnexo]
(
[alu_id] [bigint] NOT NULL,
[afj_id] [int] NOT NULL,
[aja_id] [int] NOT NULL,
[arq_id] [bigint] NOT NULL,
[aja_descricao] [varchar] (500) COLLATE Latin1_General_CI_AS NULL,
[aja_situacao] [tinyint] NOT NULL CONSTRAINT [DF_ACA_AlunoJustificativaFaltaAnexo_aja_situacao] DEFAULT ((1)),
[aja_dataCriacao] [datetime] NOT NULL CONSTRAINT [DF_ACA_AlunoJustificativaFaltaAnexo_aja_dataCriacao] DEFAULT (getdate()),
[aja_dataAlteracao] [datetime] NOT NULL CONSTRAINT [DF_ACA_AlunoJustificativaFaltaAnexo_aja_dataAlteracao] DEFAULT (getdate())
)
GO
PRINT N'Creating primary key [PK_ACA_AlunoJustificativaFaltaAnexo] on [dbo].[ACA_AlunoJustificativaFaltaAnexo]'
GO
ALTER TABLE [dbo].[ACA_AlunoJustificativaFaltaAnexo] ADD CONSTRAINT [PK_ACA_AlunoJustificativaFaltaAnexo] PRIMARY KEY CLUSTERED  ([alu_id], [afj_id], [aja_id])
GO
PRINT N'Creating [dbo].[STP_ACA_AlunoJustificativaFaltaAnexo_INSERT]'
GO


CREATE PROCEDURE [dbo].[STP_ACA_AlunoJustificativaFaltaAnexo_INSERT]
	@alu_id BigInt
	, @afj_id Int
	, @aja_id Int
	, @arq_id BigInt
	, @aja_descricao VarChar (500)
	, @aja_situacao TinyInt
	, @aja_dataCriacao DateTime
	, @aja_dataAlteracao DateTime

AS
BEGIN
	INSERT INTO 
		ACA_AlunoJustificativaFaltaAnexo
		( 
			alu_id 
			, afj_id 
			, aja_id 
			, arq_id 
			, aja_descricao 
			, aja_situacao 
			, aja_dataCriacao 
			, aja_dataAlteracao 
 
		)
	VALUES
		( 
			@alu_id 
			, @afj_id 
			, @aja_id 
			, @arq_id 
			, @aja_descricao 
			, @aja_situacao 
			, @aja_dataCriacao 
			, @aja_dataAlteracao 
 
		)
		
		SELECT ISNULL(SCOPE_IDENTITY(),-1)

	
	
END


GO
PRINT N'Altering [dbo].[FN_MatriculasBoletimDa_Turma]'
GO
-- =============================================
-- Author:		Carla Frascareli
-- Create date: 12/11/2012
-- Description:	Retorna uma tabela com os períodos do calendário e o id da matriculaTurma
--				onde estará as notas da efetivação naquele período, para gerar o boletim,
--				para todas as matriculas daquela turma.
-- =============================================
/******************************************
	* Antes de alterar ou utilizar essa função, verifique
	as procedures criadas para substituição da função:
	NEW_MTR_MatriculaTurma_MatriculasBoletim e NEW_MTR_MatriculaTurma_MatriculasBoletim_DaTurma
******************************************/
ALTER FUNCTION [dbo].[FN_MatriculasBoletimDa_Turma]
(
	@tur_id BIGINT
)
RETURNS TABLE
AS
RETURN
(
	SELECT 
		Mtr.alu_id
		, Mtr.cal_ano
		, Mtr.mtu_id
		, Mtr.tur_id
		, Mtr.cal_id
		, Mtr.fav_id
		, Mtr.cap_id
		, Mtr.tpc_id
		, Mtr.tpc_ordem
		, Mtr.PeriodosEquivalentes
		, Mtr.MesmoCalendario
		, Mtr.MesmoFormato
		, Mtr.mtu_numeroChamada 
		, Mtr.PossuiSaidaPeriodo
		, Mtr.registroExterno
		, Mtr.PermiteConceitoGlobal
		, Mtr.PermiteDisciplinas
		, Mtr.mov_id
	FROM (
		SELECT 
			Mtu.alu_id, MAX(Mtu.mtu_id) AS mtu_id
		FROM MTR_MatriculaTurma Mtu WITH(NOLOCK)
		WHERE
			Mtu.tur_id = @tur_id
			AND Mtu.mtu_situacao IN (1,5)
		GROUP BY Mtu.alu_id
	) AS Mtu
	CROSS APPLY FN_MatriculasBoletim(Mtu.alu_id, Mtu.mtu_id) Mtr
)

GO
PRINT N'Altering [dbo].[NEW_Relatorio_DocDctRelTarjetaBimestral_SubRelatorio]'
GO
-- Stored Procedure

-- =============================================
-- Author:		Carla Frascareli
-- Create date: 23/02/2015
-- Description:	Procedure do subrelatório da Tarjeta bimestral (Ata síntese do CP também).
--				Busca dados do fechamento de um bimestre quando esse já foi fechado, quando
--				não, calcula dados lançados até o momento.
--				Acumula dados dos bimestres anteriores do fechamento.

---- Alterado: Marcia Haga - 06/03/2015
---- Description: Alterado para considerar o fav_percentualMinimoFrequenciaFinalAjustadaDisciplina,
---- ao retornar a frequencia minima esperada para o aluno.

---- Alterado: Marcia Haga - 22/04/2015
---- Description: Alterado tratamento das aulas e faltas com pendencia para corrigir erro de registro duplicado.

---- Alterado: Daniel Jun Suguimoto - 16/06/2015
---- Description: Alterado para considerar disciplinas de turmas multisseriadas.

---- Alterado: Júlio César de Oliveira Leme - 24/09/2015
---- Description: Melhoria de Performance. Retirada de WITH...SELECT e criação de chaves primárias em 
----              algumas variáveis tipo tabela
-- =============================================
ALTER PROCEDURE [dbo].[NEW_Relatorio_DocDctRelTarjetaBimestral_SubRelatorio]
	@tpc_id INT
	, @tur_id BIGINT
	, @tud_id BIGINT
	, @documentoOficial BIT
AS
BEGIN

	SET NOCOUNT ON;	

	DECLARE 
		@tur_tipo TINYINT
		, @fav_id INT
		, @percentualMinimoFrequencia DECIMAL(5,2)
		, @percentualMinimoFrequenciaDisciplina DECIMAL(5,2)
		, @tpc_ordem INT
		, @fav_tipoLancamentoFrequencia TINYINT
		, @fav_calculoQtdeAulasDadas TINYINT
		, @ultimoBimestre BIT
		, @fav_variacao DECIMAL(5,2)
		, @esa_id INT
		, @esa_tipo TINYINT
		, @tud_idCalculo BIGINT -- usado para passar como parâmetro na função FN_Select_FaltasAulasBy_TurmaDisciplina

	SELECT
		@tur_tipo = Tur.tur_tipo
		, @fav_id = Fav.fav_id
		, @percentualMinimoFrequencia = Fav.percentualMinimoFrequencia
		, @percentualMinimoFrequenciaDisciplina = Fav.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina
		, @tpc_ordem = Tpc.tpc_ordem
		, @fav_tipoLancamentoFrequencia = Fav.fav_tipoLancamentoFrequencia
		, @fav_calculoQtdeAulasDadas = Fav.fav_calculoQtdeAulasDadas
		, @fav_variacao = Fav.fav_variacao		
		, @esa_tipo = ISNULL(EsaGlobal.esa_tipo, EsaDisc.esa_tipo) 
		, @ultimoBimestre = 
			(
				-- Verifica se é o último bimestre com avaliação.
				SELECT CASE WHEN MAX(tpc_ordem) = Tpc.tpc_ordem THEN 1 ELSE 0 END
				FROM ACA_Avaliacao Ava WITH(NOLOCK)
				INNER JOIN ACA_TipoPeriodoCalendario TpcUlt WITH(NOLOCK)
					ON TpcUlt.tpc_id = Ava.tpc_id
				WHERE
					Ava.fav_id = Fav.fav_id
					-- Periódica, Periódica + Final
					AND Ava.ava_tipo IN (1,5)
					AND Ava.ava_situacao <> 3
			)
		, @esa_id = ISNULL(EsaGlobal.esa_id, EsaDisc.esa_id) 
	FROM TUR_Turma Tur WITH(NOLOCK)
	INNER JOIN ACA_FormatoAvaliacao Fav WITH(NOLOCK)
		ON Fav.fav_id = Tur.fav_id
	INNER JOIN ACA_TipoPeriodoCalendario Tpc WITH(NOLOCK)
		ON Tpc.tpc_id = @tpc_id
	LEFT JOIN ACA_EscalaAvaliacao EsaGlobal WITH(NOLOCK)
		ON EsaGlobal.esa_id = Fav.esa_idConceitoGlobal
		AND Fav.fav_tipo = 1 --Global
	LEFT JOIN ACA_EscalaAvaliacao EsaDisc WITH(NOLOCK)
		ON EsaDisc.esa_id = Fav.esa_idPorDisciplina
		AND Fav.fav_tipo IN (2,3) --Disciplina e Disciplina + Global
	WHERE
		Tur.tur_id = @tur_id

	DECLARE @tbAlunos AS TABLE 
	(
		tur_id BIGINT NOT NULL,alu_id BIGINT NOT NULL, mtu_numeroChamada INT NULL,
		mtu_id INT NOT NULL, mtu_resultado TINYINT NULL, mtu_situacao TINYINT NULL
		, tpc_id INT, tpc_ordem INT
		PRIMARY KEY (alu_id, mtu_id, tur_id, tpc_id, tpc_ordem)
	)


	IF (@tur_tipo NOT IN (3,4)) -- Se não for Multisseriadas
	BEGIN
		DECLARE @MatriculasBoletimDaTurma AS TABLE

		 (alu_id BIGINT, mtu_id INT, tur_id BIGINT, tpc_id INT, tpc_ordem INT
		  PRIMARY KEY (alu_id, mtu_id, tur_id, tpc_id, tpc_ordem));

        DECLARE @MatriculasTurma AS TABLE
         (alu_id BIGINT, mtu_id INT
          PRIMARY KEY (alu_id,  mtu_id));

        INSERT INTO @MatriculasTurma
        select Mtu.alu_id, MAX(Mtu.mtu_id) mtu_id
          from MTR_MatriculaTurma Mtu WITH(NOLOCK) 
         where tur_id = @tur_id
		   and mtu_situacao <> 3
		 GROUP BY Mtu.alu_id

		INSERT INTO @MatriculasBoletimDaTurma (alu_id, mtu_id, tur_id, tpc_id, tpc_ordem)
		select Mbt.alu_id, Mbt.mtu_id, Mbt.tur_id, Mbt.tpc_id, Mbt.tpc_ordem
		  from MTR_MatriculasBoletim Mbt WITH(NOLOCK)
			   inner join @MatriculasTurma as mtu 
			    on mtu.alu_id = Mbt.alu_id
			   and mtu.mtu_id = Mbt.mtu_origemDados
		 where Mbt.tur_id = @tur_id
			AND Mbt.tpc_ordem <= @tpc_ordem
			-- Não traz alunos com saída no período e registro externo, pois esses também não aparecem no fechamento.
			AND Mbt.PossuiSaidaPeriodo = 0
			AND Mbt.registroExterno = 0


		IF not exists (select top 1 alu_id from @MatriculasBoletimDaTurma)
		begin
			INSERT INTO @MatriculasBoletimDaTurma (alu_id, mtu_id, tur_id, tpc_id, tpc_ordem)
			SELECT
				 Mbt.alu_id, Mbt.mtu_id, Mbt.tur_id, Mbt.tpc_id, tpc_ordem
			FROM 
				dbo.FN_MatriculasBoletimDa_Turma(@tur_id) AS Mbt
			WHERE
				Mbt.tpc_ordem <= @tpc_ordem
				AND Mbt.tur_id = @tur_id
				-- Não traz alunos com saída no período e registro externo, pois esses também não aparecem no fechamento.
				AND Mbt.PossuiSaidaPeriodo = 0
				AND Mbt.registroExterno = 0
		END

		INSERT INTO
			@tbAlunos(tur_id, alu_id, mtu_id, mtu_resultado, mtu_situacao, mtu_numeroChamada, tpc_id, tpc_ordem)
		SELECT 
			Mbt.tur_id
			, Mbt.alu_id
			, Mbt.mtu_id
			, Mtu.mtu_resultado 
			, Mtu.mtu_situacao
			, Mtu.mtu_numeroChamada
			, Mbt.tpc_id
			, Mbt.tpc_ordem
		FROM  @MatriculasBoletimDaTurma AS Mbt
		INNER JOIN MTR_MatriculaTurma Mtu WITH(NOLOCK)
			ON Mtu.alu_id = Mbt.alu_id
			AND Mtu.mtu_id = Mbt.mtu_id

	END
	ELSE IF (@tur_tipo = 4) -- Multisseriada do docente
	BEGIN
		INSERT INTO @tbAlunos(tur_id, alu_id, mtu_id, mtu_resultado, mtu_situacao, mtu_numeroChamada, tpc_id, tpc_ordem)
		SELECT 
			TUR.tur_id AS tur_id
			, MTU.alu_id AS alu_id
			, MTU.mtu_id
			, MTU.mtu_resultado 
			, MTU.mtu_situacao
			, Mtu.mtu_numeroChamada
			, Cap.tpc_id
			, Tpc.tpc_ordem
		FROM  
			TUR_Turma TUR WITH(NOLOCK)
			INNER JOIN TUR_TurmaRelTurmaDisciplina AS TRelTud WITH(NOLOCK)
				ON TUR.tur_id = TRelTud.tur_id
			INNER JOIN TUR_TurmaDisciplinaMultisseriada tdm WITH(NOLOCK)
				ON TRelTud.tud_id = tdm.tud_idDocente
			INNER JOIN MTR_MatriculaTurmaDisciplina AS Mtd WITH(NOLOCK)
				ON tdm.alu_id = Mtd.alu_id
				AND tdm.mtu_id = Mtd.mtu_id 
				AND tdm.mtd_id = Mtd.mtd_id
				AND Mtd.mtd_situacao <> 3
			INNER JOIN MTR_MatriculaTurma MTU WITH(NOLOCK)
				ON Mtd.alu_id = MTU.alu_id 
				AND Mtd.mtu_id = MTU.mtu_id
				AND MTU.mtu_situacao <> 3
			-- Buscar bimestres da matrícula do aluno
			INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
				ON Tur.cal_id = Cap.cal_id
				AND Cap.cap_situacao <> 3
				AND Mtu.mtu_dataMatricula <= Cap.cap_dataFim
				AND (Mtu.mtu_dataSaida IS NULL OR Mtu.mtu_dataSaida >= Cap.cap_dataInicio)
			INNER JOIN ACA_TipoPeriodoCalendario Tpc WITH(NOLOCK)
				ON Tpc.tpc_id = Cap.tpc_id
				AND Tpc.tpc_ordem <= @tpc_ordem
		WHERE
			TUR.tur_id = @tur_id
			-- Exclusão lógica
			AND TUR.tur_situacao <> 3
	END
	ELSE IF (@tur_tipo = 3) -- Multisseriada
	BEGIN
		INSERT INTO @tbAlunos(tur_id, alu_id, mtu_id, mtu_resultado, mtu_situacao, mtu_numeroChamada, tpc_id, tpc_ordem)
		SELECT 
			TUR.tur_id AS tur_id
			, MTU.alu_id AS alu_id
			, MTU.mtu_id
			, MTU.mtu_resultado 
			, MTU.mtu_situacao
			, Mtu.mtu_numeroChamada
			, Cap.tpc_id
			, Tpc.tpc_ordem
		FROM
			TUR_Turma TUR WITH(NOLOCK)
			INNER JOIN TUR_TurmaRelTurmaDisciplina AS TRelTud WITH(NOLOCK)
				ON TUR.tur_id = TRelTud.tur_id
			INNER JOIN MTR_MatriculaTurmaDisciplina AS Mtd WITH(NOLOCK)
				ON TRelTud.tud_id = Mtd.tud_id	
			INNER JOIN MTR_MatriculaTurma MTU WITH(NOLOCK)
				ON Mtd.alu_id = MTU.alu_id 
				AND Mtd.mtu_id = MTU.mtu_id
				AND MTU.mtu_situacao <> 3
			-- Buscar bimestres da matrícula do aluno
			INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK)
				ON Tur.cal_id = Cap.cal_id
				AND Cap.cap_situacao <> 3
				AND Mtu.mtu_dataMatricula <= Cap.cap_dataFim
				AND (Mtu.mtu_dataSaida IS NULL OR Mtu.mtu_dataSaida >= Cap.cap_dataInicio)
			INNER JOIN ACA_TipoPeriodoCalendario Tpc WITH(NOLOCK)
				ON Tpc.tpc_id = Cap.tpc_id
				AND Tpc.tpc_ordem <= @tpc_ordem
		WHERE
			TUR.tur_id = @tur_id
			-- Exclusão lógica
			AND TUR.tur_situacao <> 3
	END		

	DECLARE @tbDisciplinasRelatorio TABLE
	(tur_id BIGINT NOT NULL, tud_id BIGINT NOT NULL, tud_idRegencia BIGINT, tud_tipo TINYINT
	, Regencia BIT, tud_nome VARCHAR(200), dis_id INT, tds_ordem INT, LinhaRegencia INT
	PRIMARY KEY (tur_id, tud_id))

    DECLARE @tbDisciplinas TABLE
    (tur_id BIGINT NOT NULL, tud_id BIGINT NOT NULL, tud_tipo TINYINT,
     Regencia BIT, tud_idRegencia BIGINT, tud_nomeCompRegencia VARCHAR(200)
     PRIMARY KEY (tud_id, tur_id))

	INSERT INTO @tbDisciplinas
		-- Disciplina informada no filtro, que não seja Componente nem Regência.
		SELECT 
			RelTud.tur_id
			, Tud.tud_id
			, Tud.tud_tipo
			, CAST(0 AS BIT) AS Regencia
			, NULL AS tud_idRegencia
			, '' AS tud_nomeCompRegencia
		FROM TUR_TurmaRelTurmaDisciplina RelTud WITH(NOLOCK)
		INNER JOIN TUR_TurmaDisciplina Tud WITH(NOLOCK)
			ON Tud.tud_id = RelTud.tud_id
		WHERE
			RelTud.tur_id = @tur_id
			AND Tud.tud_situacao <> 3
			AND Tud.tud_id = @tud_id
			-- Disicplinas que não são regência.
			AND Tud.tud_tipo NOT IN (11, 12, 13, 17, 18, 19)
		UNION
		-- Disciplina informada, que seja Regência.
		SELECT 
			RelTud.tur_id
			, TudComp.tud_id
			, TudComp.tud_tipo
			, CAST(1 AS BIT) AS Regencia
			, Tud.tud_id AS tud_idRegencia
			, TudComp.tud_nome AS tud_nomeCompRegencia
		FROM TUR_TurmaRelTurmaDisciplina RelTud WITH(NOLOCK)
		INNER JOIN TUR_TurmaDisciplina Tud WITH(NOLOCK)
			ON Tud.tud_id = RelTud.tud_id
		INNER JOIN TUR_TurmaRelTurmaDisciplina RelCompReg WITH(NOLOCK)
			ON RelCompReg.tur_id = RelTud.tur_id
		INNER JOIN TUR_TurmaDisciplina TudComp WITH(NOLOCK)
			ON TudComp.tud_id = RelCompReg.tud_id
			AND TudComp.tud_situacao <> 3
			-- Componentes e complementos da regência.
			AND TudComp.tud_tipo IN (12,13)
		WHERE
			RelTud.tur_id = @tur_id
			AND Tud.tud_situacao <> 3
			AND Tud.tud_id = @tud_id
			-- Disicplinas que são regência.
			AND Tud.tud_tipo = 11
		UNION
		-- Disicplina informada, quando é componente da regência.
		SELECT 
			RelTud.tur_id
			, TudComp.tud_id
			, TudComp.tud_tipo
			, CAST(1 AS BIT) AS Regencia
			, Tud.tud_id AS tud_idRegencia
			, TudComp.tud_nome AS tud_nomeCompRegencia
		FROM TUR_TurmaRelTurmaDisciplina RelTud WITH(NOLOCK)
		INNER JOIN TUR_TurmaDisciplina Tud WITH(NOLOCK)
			ON Tud.tud_id = RelTud.tud_id
		INNER JOIN TUR_TurmaRelTurmaDisciplina RelCompReg WITH(NOLOCK)
			ON RelCompReg.tur_id = RelTud.tur_id
		INNER JOIN TUR_TurmaDisciplina TudComp WITH(NOLOCK)
			ON TudComp.tud_id = RelCompReg.tud_id
			AND TudComp.tud_situacao <> 3
			-- Componentes e complementos da regência.
			AND TudComp.tud_tipo IN (12,13)
		WHERE
			RelTud.tur_id = @tur_id
			AND Tud.tud_situacao <> 3
			AND TudComp.tud_id = @tud_id
			-- Disicplinas que são regência.
			AND Tud.tud_tipo = 11
		UNION
		-- Disciplina não informada, quando a turma possui regência.
		SELECT 
			RelTud.tur_id
			, ISNULL(TudComp.tud_id, Tud.tud_id) AS tud_id
			, ISNULL(TudComp.tud_tipo, Tud.tud_tipo) AS tud_tipo
			, CASE WHEN TudComp.tud_id IS NULL THEN 0 ELSE 1 END AS Regencia
			, CASE WHEN TudComp.tud_id IS NULL THEN NULL ELSE Tud.tud_id END AS tud_idRegencia
			, TudComp.tud_nome AS tud_nomeCompRegencia
		FROM TUR_TurmaRelTurmaDisciplina RelTud WITH(NOLOCK)
		INNER JOIN TUR_TurmaDisciplina Tud WITH(NOLOCK)
			ON Tud.tud_id = RelTud.tud_id
		INNER JOIN TUR_TurmaRelTurmaDisciplina RelCompReg WITH(NOLOCK)
			ON RelCompReg.tur_id = RelTud.tur_id
		INNER JOIN TUR_TurmaDisciplina TudComp WITH(NOLOCK)
			ON TudComp.tud_id = RelCompReg.tud_id
			AND TudComp.tud_situacao <> 3
			-- Componentes e complementos da regência.
			AND TudComp.tud_tipo IN (12,13)
		WHERE
			RelTud.tur_id = @tur_id
			AND Tud.tud_situacao <> 3
			-- Quando o relatório é emitido por turma.
			AND @tud_id IS NULL 
			-- Disicplinas que são regência.
			AND Tud.tud_tipo = 11
		UNION
		-- Disciplina não informada, traz as disciplinas que não são regência.
		SELECT 
			RelTud.tur_id
			, Tud.tud_id
			, Tud.tud_tipo
			, 0 AS Regencia
			, NULL AS tud_idRegencia
			, '' AS tud_nomeCompRegencia
		FROM TUR_TurmaRelTurmaDisciplina RelTud WITH(NOLOCK)
		INNER JOIN TUR_TurmaDisciplina Tud WITH(NOLOCK)
			ON Tud.tud_id = RelTud.tud_id
		WHERE
			RelTud.tur_id = @tur_id
			AND Tud.tud_situacao <> 3
			-- Quando o relatório é emitido por turma.
			AND @tud_id IS NULL 
			-- Disicplinas que não são regência.
			AND Tud.tud_tipo NOT IN (11, 12, 13, 17, 18, 19)

		UNION 

		SELECT
			mtu.tur_id
			, tud.tud_id
			, tud.tud_tipo
			, 0 AS Regencia
			, NULL AS tud_idRegencia
			, '' AS tud_nomeCompRegencia
		FROM
			@tbAlunos mtu
			INNER JOIN MTR_MatriculaTurmaDisciplina mtd WITH(NOLOCK)
				ON mtu.alu_id = mtd.alu_id
				AND mtu.mtu_id = mtd.mtu_id
			INNER JOIN TUR_TurmaDisciplina tud WITH(NOLOCK)
				ON tud.tud_id = mtd.tud_id
				AND tud.tud_tipo = 14
				AND tud.tud_situacao <> 3
			--INNER JOIN TUR_TurmaRelTurmaDisciplina Trt WITH(NOLOCK)
			--	ON Trt.tud_id = tud.tud_id
			INNER JOIN TUR_Turma tur WITH(NOLOCK)
				ON tur.tur_id = mtu.tur_id
				AND tur.tur_situacao <> 3
		WHERE
			(@tud_id IS NULL OR tud.tud_id = @tud_id)
 			AND Tud.tud_tipo NOT IN (18, 19)

	INSERT INTO @tbDisciplinasRelatorio
	(tur_id, tud_id, tud_idRegencia, tud_tipo, Regencia, tud_nome, dis_id, tds_ordem, LinhaRegencia)
		SELECT
			Tud.tur_id
			, Tud.tud_id
			, Tud.tud_idRegencia
			, Tud.tud_tipo
			, Tud.Regencia
			, CASE WHEN Regencia = 1 THEN Dis.dis_nomeAbreviado ELSE Dis.dis_nomeAbreviado END AS tud_nome
			, Dis.tds_id AS dis_id
			, Tds.tds_ordem
			, CASE WHEN Regencia = 0 THEN 1 ELSE ROW_NUMBER() OVER (PARTITION BY Tud.tur_id, Tud.Regencia ORDER BY Tud.tur_id, Tud.Regencia) END AS LinhaRegencia
		FROM @tbDisciplinas Tud
		INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDis WITH(NOLOCK)
			ON RelDis.tud_id = Tud.tud_id
		INNER JOIN ACA_Disciplina Dis WITH(NOLOCK)
			ON Dis.dis_id = RelDis.dis_id
		INNER JOIN ACA_TipoDisciplina Tds WITH(NOLOCK)
			ON Tds.tds_id = Dis.tds_id
		WHERE
			Dis.dis_situacao <> 3
			AND Dis.tds_id NOT IN (
				-- Remover disciplinas de enriquecimento curricular.
				SELECT CAST(pac_valor AS INT)
				FROM ACA_ParametroAcademico WITH(NOLOCK)
				WHERE 
					pac_chave = 'TIPO_DISCIPLINA_ENRIQUECIMENTO_CURRICULAR'
					AND pac_situacao <> 3
			)	

	DECLARE @MatriculaTurmaDisicplina TABLE
	(tur_id BIGINT NOT NULL, alu_id BIGINT NOT NULL, mtu_id INT NOT NULL, tpc_id INt, tpc_ordem INT, mtd_id INT
	, mtd_idReg INT, tud_id BIGINT, tud_idRegencia BIGINT, mtu_numeroChamada INT, mtu_resultado TINYINT
	, LinhaRegencia INT, Regencia BIT
	PRIMARY KEY (alu_id, mtu_id, mtd_id, tpc_id))

	INSERT INTO @MatriculaTurmaDisicplina
	(tur_id, alu_id, mtu_id, tpc_id, tpc_ordem, mtd_id, mtd_idReg, tud_id, tud_idRegencia
	, mtu_numeroChamada, mtu_resultado, LinhaRegencia, Regencia)

		SELECT
			Mtu.tur_id, Mtu.alu_id, Mtu.mtu_id, Mtu.tpc_id, Mtu.tpc_ordem
			, Mtd.mtd_id, MtdReg.mtd_id AS mtd_idReg, Tud.tud_id, Tud.tud_idRegencia
			, Mtu.mtu_numeroChamada
			, Mtu.mtu_resultado
			, Tud.LinhaRegencia
			, Tud.Regencia
		FROM @tbAlunos Mtu
		INNER JOIN @tbDisciplinasRelatorio Tud
			ON Tud.tur_id = Mtu.tur_id
		INNER JOIN MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
			ON Mtd.alu_id = Mtu.alu_id
			AND Mtd.mtu_id = Mtu.mtu_id
			AND Mtd.tud_id = Tud.tud_id
			AND Mtd.mtd_situacao <> 3
		LEFT JOIN MTR_MatriculaTurmaDisciplina MtdReg WITH(NOLOCK)
			ON MtdReg.alu_id = Mtu.alu_id
			AND MtdReg.mtu_id = Mtu.mtu_id
			AND MtdReg.tud_id = Tud.tud_idRegencia
			AND MtdReg.mtd_situacao <> 3
			
	DECLARE @FrequenciaExterna TABLE (alu_id BIGINT, mtu_id INT, mtd_id INT, tpc_id INT, qtdFaltas INT, qtdAulas INT)

	INSERT INTO @FrequenciaExterna (alu_id, mtu_id, mtd_id, tpc_id, qtdFaltas, qtdAulas)
	SELECT
		Mtd.alu_id,
		Mtd.mtu_id,
		Mtd.mtd_id,
		Mtd.tpc_id,
		afx.afx_qtdFaltas AS qtdFaltas,
		afx.afx_qtdAulas AS qtdAulas
	FROM @MatriculaTurmaDisicplina Mtd
	INNER JOIN CLS_AlunoFrequenciaExterna afx WITH(NOLOCK)
		ON Mtd.alu_id = afx.alu_id
		AND Mtd.mtu_id = afx.mtu_id
		AND Mtd.mtd_id = afx.mtd_id
		AND Mtd.tpc_id = afx.tpc_id
		AND afx.afx_situacao <> 3
	WHERE
		Mtd.tpc_id = @tpc_id
	GROUP BY
		Mtd.alu_id,
		Mtd.mtu_id,
		Mtd.mtd_id,
		Mtd.tpc_id,
		afx.afx_qtdFaltas,
		afx.afx_qtdAulas

	INSERT INTO @FrequenciaExterna (alu_id, mtu_id, mtd_id, tpc_id, qtdFaltas, qtdAulas)
	SELECT
		Mtd.alu_id,
		Mtd.mtu_id,
		Mtd.mtd_idReg,
		Mtd.tpc_id,
		afx.afx_qtdFaltas AS qtdFaltas,
		afx.afx_qtdAulas AS qtdAulas
	FROM @MatriculaTurmaDisicplina Mtd
	INNER JOIN CLS_AlunoFrequenciaExterna afx WITH(NOLOCK)
		ON Mtd.alu_id = afx.alu_id
		AND Mtd.mtu_id = afx.mtu_id
		AND Mtd.mtd_idReg = afx.mtd_id
		AND Mtd.tpc_id = afx.tpc_id
		AND afx.afx_situacao <> 3
	WHERE
		Mtd.tpc_id = @tpc_id
	GROUP BY
		Mtd.alu_id,
		Mtd.mtu_id,
		Mtd.mtd_idReg,
		Mtd.tpc_id,
		afx.afx_qtdFaltas,
		afx.afx_qtdAulas

	DECLARE @possuiFreqExterna BIT = 0
	IF (EXISTS(SELECT TOP 1 alu_id FROM @FrequenciaExterna WHERE qtdFaltas > 0 OR qtdAulas > 0))
		SET @possuiFreqExterna = 1

	DECLARE @DadosFechamento TABLE
	(alu_id BIGINT NOT NULL, mtu_id INT NOT NULL, mtd_id INT NOT NULL,  fav_id INT, ava_id INT
	, atd_id INT, tud_id BIGINT, tud_idRegencia BIGINT, tpc_id INt, tpc_ordem INt  
	, LinhaRegencia INT, atd_avaliacao VARCHAR(5), atd_numeroFaltas int, atd_ausenciasCompensadas INT, atd_frequenciaFinalAjustada INT
	, BimestreFechado TINYINT, atd_numeroAulas INT, atd_frequencia INT, esa_id INT, PossuiFreqExterna BIT
	PRIMARY KEY (alu_id, mtu_id, mtd_id, tpc_id))
	INSERT INTO @DadosFechamento
	(
	    alu_id, mtu_id, mtd_id, fav_id, ava_id, atd_id, tud_id, tud_idRegencia, tpc_id, tpc_ordem,
	    LinhaRegencia, atd_avaliacao, atd_numeroFaltas, atd_ausenciasCompensadas, atd_frequenciaFinalAjustada,
	    BimestreFechado, atd_numeroAulas, atd_frequencia, esa_id, PossuiFreqExterna
	)

	(
		-- Busca dados do fechamento de todos os bimestres acumulados até o atual.
		SELECT
			Atd.alu_id
			, Atd.mtu_id
			, Atd.mtd_id
			, Atd.fav_id
			, Atd.ava_id
			, Atd.atd_id
			, Mtd.tud_id
			, Mtd.tud_idRegencia
			, Mtd.tpc_id
			, Mtd.tpc_ordem
			, Mtd.LinhaRegencia
			, ISNULL(Atd.atd_avaliacaoPosConselho, Atd.atd_avaliacao) AS atd_avaliacao
			, CASE WHEN Mtd.tud_idRegencia > 0 
				THEN ISNULL(AtdReg.atd_numeroFaltas, 0) + ISNULL(afxReg.qtdFaltas, 0)
				ELSE ISNULL(Atd.atd_numeroFaltas, 0) + ISNULL(afx.qtdFaltas, 0)
				END AS atd_numeroFaltas
			, CASE WHEN Mtd.tud_idRegencia > 0 
				THEN AtdReg.atd_ausenciasCompensadas
				ELSE Atd.atd_ausenciasCompensadas
				END AS atd_ausenciasCompensadas
			, CASE WHEN Mtd.tud_idRegencia > 0 
				THEN AtdReg.atd_frequenciaFinalAjustada
				ELSE Atd.atd_frequenciaFinalAjustada
				END AS atd_frequenciaFinalAjustada			
			, CASE WHEN Mtd.tpc_id = @tpc_id AND
				        ((ISNULL(Atd.atd_avaliacao,'') <> '') OR (ISNULL(Atd.atd_avaliacaoPosConselho,'') <> ''))
						THEN 1 ELSE 0 END AS BimestreFechado
			, CASE WHEN Mtd.tud_idRegencia > 0 
				THEN ISNULL(AtdReg.atd_numeroAulas, 0) + ISNULL(afxReg.qtdAulas, 0)
				ELSE ISNULL(Atd.atd_numeroAulas, 0) + ISNULL(afx.qtdAulas, 0)
				END AS atd_numeroAulas
			, CASE WHEN Mtd.tud_idRegencia > 0 
				THEN AtdReg.atd_frequencia
				ELSE Atd.atd_frequencia
				END AS atd_frequencia
			, @esa_id AS esa_id
			, CASE WHEN ISNULL(Mtd.tud_idRegencia, 0) <= 0 AND (ISNULL(afx.qtdAulas, 0) > 0 OR ISNULL(afx.qtdFaltas, 0) > 0)
				   THEN CAST(1 AS BIT)
				   WHEN Mtd.tud_idRegencia > 0 AND (ISNULL(afxReg.qtdAulas, 0) > 0 OR ISNULL(afxReg.qtdFaltas, 0) > 0)
				   THEN CAST(1 AS BIT) 
				   ELSE CAST(0 AS BIT) END AS possuiFreqExterna
		FROM @MatriculaTurmaDisicplina Mtd
		INNER JOIN TUR_TurmaDisciplina tud WITH(NOLOCK)
			ON Mtd.tud_id = tud.tud_id
			AND tud.tud_tipo <> 14
			AND tud.tud_situacao <> 3
		INNER JOIN ACA_Avaliacao Ava WITH(NOLOCK)
			ON Ava.fav_id = @fav_id
			AND Ava.tpc_id = Mtd.tpc_id
			-- 1 - Periódica, 5 - Periódica + Final
			AND Ava.ava_tipo IN (1,5)
			AND Ava.ava_situacao <> 3
		INNER JOIN CLS_AlunoAvaliacaoTurmaDisciplina Atd WITH(NOLOCK)
			ON Atd.tud_id = Mtd.tud_id
			AND Atd.alu_id = Mtd.alu_id
			AND Atd.mtu_id = Mtd.mtu_id
			AND Atd.mtd_id = Mtd.mtd_id
			AND Ava.fav_id = Atd.fav_id
			AND Ava.ava_id = Atd.ava_id
			AND Atd.atd_situacao <> 3
		LEFT JOIN CLS_AlunoAvaliacaoTurmaDisciplina AtdReg WITH(NOLOCK)
			ON AtdReg.tud_id = Mtd.tud_idRegencia
			AND AtdReg.alu_id = Mtd.alu_id
			AND AtdReg.mtu_id = Mtd.mtu_id
			AND AtdReg.mtd_id = Mtd.mtd_idReg
			AND AtdReg.fav_id = Ava.fav_id
			AND AtdReg.ava_id = Ava.ava_id
			AND AtdReg.atd_situacao <> 3
		LEFT JOIN @FrequenciaExterna afx
			ON Mtd.alu_id = afx.alu_id
			AND Mtd.mtu_id = afx.mtu_id
			AND Mtd.mtd_id = afx.mtd_id
			AND Mtd.tpc_id = afx.tpc_id
		LEFT JOIN @FrequenciaExterna afxReg
			ON Mtd.alu_id = afxReg.alu_id
			AND Mtd.mtu_id = afxReg.mtu_id
			AND Mtd.mtd_idReg = afxReg.mtd_id
			AND Mtd.tpc_id = afxReg.tpc_id

		UNION

		SELECT
			Atd.alu_id
			, Atd.mtu_id
			, Atd.mtd_id
			, Atd.fav_id
			, Atd.ava_id
			, Atd.atd_id
			, Mtd.tud_id
			, Mtd.tud_idRegencia
			, Mtd.tpc_id
			, Mtd.tpc_ordem
			, Mtd.LinhaRegencia
			, CASE WHEN @esa_id = ISNULL(fav.esa_idConceitoGlobal, fav.esa_idPorDisciplina)
				THEN ISNULL(Atd.atd_avaliacaoPosConselho, Atd.atd_avaliacao)
				ELSE NULL
			  END AS atd_avaliacao
			, CASE WHEN @esa_id = ISNULL(fav.esa_idConceitoGlobal, fav.esa_idPorDisciplina)
				THEN ISNULL(Atd.atd_numeroFaltas, 0)
				ELSE NULL
			  END + ISNULL(afx.qtdFaltas, 0) AS atd_numeroFaltas
			, CASE WHEN @esa_id = ISNULL(fav.esa_idConceitoGlobal, fav.esa_idPorDisciplina)
				THEN Atd.atd_ausenciasCompensadas
				ELSE NULL
			  END AS atd_ausenciasCompensadas
			, CASE WHEN @esa_id = ISNULL(fav.esa_idConceitoGlobal, fav.esa_idPorDisciplina)
				THEN Atd.atd_frequenciaFinalAjustada
				ELSE NULL
			  END AS atd_frequenciaFinalAjustada		
			, CASE WHEN Mtd.tpc_id = @tpc_id AND @esa_id = ISNULL(fav.esa_idConceitoGlobal, fav.esa_idPorDisciplina) AND
				        ((ISNULL(Atd.atd_avaliacao,'') <> '') OR (ISNULL(Atd.atd_avaliacaoPosConselho,'') <> ''))
						THEN 1 ELSE 0 END AS BimestreFechado
			, CASE WHEN @esa_id = ISNULL(fav.esa_idConceitoGlobal, fav.esa_idPorDisciplina)
				THEN ISNULL(Atd.atd_numeroAulas, 0)
				ELSE NULL
			  END + ISNULL(afx.qtdAulas, 0) AS atd_numeroAulas
			, CASE WHEN @esa_id = ISNULL(fav.esa_idConceitoGlobal, fav.esa_idPorDisciplina)
				THEN Atd.atd_frequencia
				ELSE NULL
			  END AS atd_frequencia
			, ISNULL(fav.esa_idConceitoGlobal, fav.esa_idPorDisciplina) AS esa_id
			, CASE WHEN ISNULL(afx.qtdAulas, 0) > 0 OR ISNULL(afx.qtdFaltas, 0) > 0
				   THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS possuiFreqExterna
		FROM @MatriculaTurmaDisicplina Mtd
		INNER JOIN TUR_TurmaDisciplina tud WITH(NOLOCK)
			ON Mtd.tud_id = tud.tud_id
			AND tud.tud_tipo = 14
			AND tud.tud_situacao <> 3
		INNER JOIN CLS_AlunoAvaliacaoTurmaDisciplina Atd WITH(NOLOCK)
			ON Atd.tud_id = Mtd.tud_id
			AND Atd.alu_id = Mtd.alu_id
			AND Atd.mtu_id = Mtd.mtu_id
			AND Atd.mtd_id = Mtd.mtd_id
			AND Atd.atd_situacao <> 3
		INNER JOIN ACA_Avaliacao Ava WITH(NOLOCK)
			ON Ava.fav_id = Atd.fav_id
			AND Ava.ava_id = Atd.ava_id
			AND Ava.tpc_id = Mtd.tpc_id
			-- 1 - Periódica, 5 - Periódica + Final
			AND Ava.ava_tipo IN (1,5)
			AND Ava.ava_situacao <> 3
		INNER JOIN ACA_FormatoAvaliacao fav WITH(NOLOCK)
			ON fav.fav_id = Atd.fav_id
			AND fav.fav_situacao <> 3
		LEFT JOIN @FrequenciaExterna afx
			ON Mtd.alu_id = afx.alu_id
			AND Mtd.mtu_id = afx.mtu_id
			AND Mtd.mtd_id = afx.mtd_id
			AND Mtd.tpc_id = afx.tpc_id

		GROUP BY
			Atd.alu_id
			, Atd.mtu_id
			, Atd.mtd_id
			, Atd.fav_id
			, Atd.ava_id
			, Atd.atd_id
			, Mtd.tud_id
			, Mtd.tud_idRegencia
			, Mtd.tpc_id
			, Mtd.tpc_ordem
			, Mtd.LinhaRegencia
			, Atd.atd_avaliacaoPosConselho
			, Atd.atd_avaliacao
			, Atd.atd_numeroFaltas
			, Atd.atd_ausenciasCompensadas
			, Atd.atd_frequenciaFinalAjustada		
			, Atd.atd_numeroAulas
			, Atd.atd_frequencia
			, fav.esa_idConceitoGlobal
			, fav.esa_idPorDisciplina
			, afx.qtdFaltas
			, afx.qtdAulas
	)

	DECLARE @DadosBimestreAtual TABLE
	(alu_id BIGINT NOT NULL, mtu_id INT NOT NULL, mtd_id INT NOT NULL,  mtd_idReg INT, tpc_id INt
	, tud_id BIGINT, tud_idRegencia BIGINT
	PRIMARY KEY (alu_id, mtu_id, mtd_id, tpc_id))

    DECLARE @tbDadosSemFechamento TABLE 
    (alu_id BIGINT NOT NULL, mtu_id INT NOT NULL, mtd_id INT NOT NULL,  mtd_idReg INT, tpc_id INt
	, tud_id BIGINT, tud_idRegencia BIGINT, tud_tipo TINYINT
	PRIMARY KEY (alu_id, mtu_id, mtd_id, tpc_id))

	INSERT INTO @tbDadosSemFechamento
		SELECT
			Mtd.alu_id
			, Mtd.mtu_id
			, Mtd.mtd_id
			, Mtd.mtd_idReg
			, Mtd.tpc_id
			, Mtd.tud_id
			, Mtd.tud_idRegencia
			, Tud.tud_tipo
		FROM @MatriculaTurmaDisicplina Mtd
		INNER JOIN TUR_TurmaDisciplina Tud WITH(NOLOCK)
			ON Tud.tud_id = Mtd.tud_id
			AND Tud.tud_situacao <> 3
		WHERE
			Mtd.tpc_id = @tpc_id
			AND NOT EXISTS 
			(
				-- Busca dados para calcular somente quando não tiver no fechamento e só para o bimestre atual 
				-- (da emissão do relatório).
				SELECT 1
				FROM @DadosFechamento Atd
				WHERE
					Atd.alu_id = Mtd.alu_id
					AND Atd.mtu_id = Mtd.mtu_id
					AND Atd.mtd_id = Mtd.mtd_id
					AND Atd.tpc_id = Mtd.tpc_id
			)

	INSERT INTO @DadosBimestreAtual
	( alu_id ,mtu_id, mtd_id, mtd_idReg, tpc_id, tud_id, tud_idRegencia)
	(
		SELECT
			Mtd.alu_id
			, Mtd.mtu_id
			, Mtd.mtd_id
			, Mtd.mtd_idReg
			, Mtd.tpc_id
			, Mtd.tud_id
			, Mtd.tud_idRegencia
		FROM  
			@tbDadosSemFechamento Mtd
		WHERE
			Mtd.tud_tipo <> 14

		UNION 

		SELECT
			Mtd.alu_id
			, Mtd.mtu_id
			, Mtd.mtd_id
			, Mtd.mtd_idReg
			, Mtd.tpc_id
			, Mtd.tud_id
			, Mtd.tud_idRegencia
		FROM @tbDadosSemFechamento Mtd
		INNER JOIN TUR_TurmaRelTurmaDisciplina Trt WITH(NOLOCK)
			ON Trt.tud_id = Mtd.tud_id
		INNER JOIN TUR_Turma Tur WITH(NOLOCK)
			ON Tur.tur_id = Trt.tur_id
			AND Tur.tur_situacao <> 3
		INNER JOIN ACA_FormatoAvaliacao Fav WITH(NOLOCK)
			ON Fav.fav_id = Tur.fav_id
			AND ISNULL(fav.esa_idConceitoGlobal, fav.esa_idPorDisciplina) = @esa_id
			AND Fav.fav_situacao <> 3
		WHERE
			Mtd.tud_tipo = 14
	)

	DECLARE @FaltasAulas TABLE
	(tud_id BIGINT NOT NULL,alu_id BIGINT NOT NULL, mtu_id INT NOT NULL, 
	   qtAulas INT, qtFaltas INT, qtFaltasReposicao INT
	PRIMARY KEY (tud_id, alu_id, mtu_id))

	DECLARE @TurmaDisciplinasCalcular TABLE
	(tud_id BIGINT,
	 tpc_id INT,
	 fl_processado BIT
	 PRIMARY KEY (tud_id, tpc_id))

	insert into @TurmaDisciplinasCalcular
	SELECT
		ISNULL(Mtd.tud_idRegencia, Mtd.tud_id) AS tud_id,
		Mtd.tpc_id,
		0 as fl_processado
	FROM @DadosBimestreAtual Mtd
	GROUP BY 
		ISNULL(Mtd.tud_idRegencia, Mtd.tud_id), Mtd.tpc_id

	INSERT INTO @FaltasAulas (tud_id, alu_id, mtu_id, qtAulas, qtFaltas, qtFaltasReposicao)
	select tud_id, alu_id, mtu_id, qtAulas, qtFaltas, qtFaltasReposicao 
	  FROM @TurmaDisciplinasCalcular TDC
		   CROSS APPLY FN_Select_FaltasAulasBy_TurmaDisciplina(@fav_tipoLancamentoFrequencia, TDC.tpc_id, TDC.tud_id, @fav_calculoQtdeAulasDadas, DEFAULT) FALTAS

	DECLARE @CompensacoesCalculadas TABLE
	(alu_id BIGINT NOT NULL, mtu_id INT NOT NULL, tud_id BIGINT NOT NULL,
	   cpa_quantidadeAulasCompensadas INT
	PRIMARY KEY (tud_id, alu_id, mtu_id))

	INSERT INTO @CompensacoesCalculadas
	(alu_id, mtu_id, tud_id, cpa_quantidadeAulasCompensadas)	
	(
		SELECT
			Mtd.alu_id, Mtd.mtu_id, Mtd.tud_id
			, SUM(Cpa.cpa_quantidadeAulasCompensadas) AS cpa_quantidadeAulasCompensadas
		FROM @DadosBimestreAtual Mtd
		INNER JOIN CLS_CompensacaoAusencia Cpa WITH(NOLOCK)
			ON Cpa.tud_id = ISNULL(Mtd.tud_idRegencia, Mtd.tud_id)
			AND Cpa.tpc_id = Mtd.tpc_id
			AND Cpa.cpa_situacao <> 3
		INNER JOIN CLS_CompensacaoAusenciaAluno Caa WITH(NOLOCK)
			ON Caa.tud_id = Cpa.tud_id
			AND Caa.cpa_id = Cpa.cpa_id
			AND Caa.alu_id = Mtd.alu_id
			AND Caa.mtu_id = Mtd.mtu_id
			AND Caa.mtd_id = ISNULL(Mtd.mtd_idReg, Mtd.mtd_id)
			AND Caa.caa_situacao <> 3
		GROUP BY Mtd.alu_id, Mtd.mtu_id, Mtd.mtd_id, Mtd.tud_id, Mtd.tud_idRegencia
	)

	DECLARE @DadosBimestreAtualCalculados TABLE
	(tud_id BIGINT NOT NULL, tud_idRegencia BIGINT, alu_id BIGINT NOT NULL, mtu_id INT NOT NULL, mtd_id INT NOT NULL, 
	   tpc_id INT NOT NULL, atd_avaliacao VARCHAR(5), qtFaltas INT, qtFaltasReposicao INT,
	   cpa_quantidadeAulasCompensadas INT, tap_aulasPrevitas INT
	PRIMARY KEY (tud_id, alu_id, mtu_id,mtd_id))

	INSERT INTO @DadosBimestreAtualCalculados
	(tud_id, tud_idRegencia, alu_id, mtu_id, mtd_id, tpc_id, atd_avaliacao,
	   qtFaltas, qtFaltasReposicao, cpa_quantidadeAulasCompensadas, tap_aulasPrevitas)

	(
		SELECT 
			Mtd.tud_id
			, Mtd.tud_idRegencia
			, Mtd.alu_id
			, Mtd.mtu_id
			, Mtd.mtd_id
			, Mtd.tpc_id
			, Atm.atm_media AS atd_avaliacao
			, Qtd.qtFaltas
			, Qtd.qtFaltasReposicao
			, Cpa.cpa_quantidadeAulasCompensadas
			, ISNULL(Tap.tap_aulasPrevitas, Qtd.qtAulas)
		FROM @DadosBimestreAtual Mtd
		LEFT JOIN @FaltasAulas Qtd
			ON Qtd.alu_id = Mtd.alu_id
			AND Qtd.mtu_id = Mtd.mtu_id
			AND ISNULL(Mtd.tud_idRegencia, Mtd.tud_id) = Qtd.tud_id
		LEFT JOIN TUR_TurmaDisciplinaAulaPrevista Tap WITH(NOLOCK)
			ON Tap.tud_id = ISNULL(Mtd.tud_idRegencia, Mtd.tud_id)
			AND Tap.tpc_id = @tpc_id
			AND Tap.tap_situacao <> 3
		-- Nota do listão.
		LEFT JOIN CLS_AlunoAvaliacaoTurmaDisciplinaMedia Atm WITH(NOLOCK)
			ON Atm.tud_id = Mtd.tud_id
			AND Atm.alu_id = Mtd.alu_id
			AND Atm.mtu_id = Mtd.mtu_id
			AND Atm.mtd_id = Mtd.mtd_id
			AND Atm.tpc_id = Mtd.tpc_id
			AND Atm.atm_situacao <> 3
		LEFT JOIN @CompensacoesCalculadas Cpa
			ON Cpa.alu_id = Mtd.alu_id
			AND Cpa.mtu_id = Mtd.mtu_id
			AND Cpa.tud_id = Mtd.tud_id
	)

	DECLARE @DadosFechamentoAcumulado TABLE
	(tud_id BIGINT, alu_id BIGINT NOT NULL, mtu_id INT NOT NULL, mtd_id INT NOT NULL, atd_ausenciasCompensadas INT,
	  atd_numeroFaltas int, atd_numeroAulas INT, PossuiFreqExterna BIT
	PRIMARY KEY (tud_id, alu_id, mtu_id, mtd_id))

	INSERT INTO @DadosFechamentoAcumulado
	(tud_id, alu_id, mtu_id, mtd_id, atd_ausenciasCompensadas, atd_numeroFaltas, atd_numeroAulas, PossuiFreqExterna)	
	(
		SELECT
			ISNULL(AtdAcum.tud_idRegencia, AtdAcum.tud_id) AS tud_id
			, AtdAcum.alu_id
			, AtdAcum.mtu_id
			, AtdAcum.mtd_id
			, SUM(ISNULL(AtdAcum.atd_ausenciasCompensadas, 0)) AS atd_ausenciasCompensadas
			, SUM(ISNULL(AtdAcum.atd_numeroFaltas, 0)) AS atd_numeroFaltas
			, SUM(ISNULL(AtdAcum.atd_numeroAulas, 0)) AS atd_numeroAulas
			, CASE WHEN SUM(CASE WHEN ISNULL(AtdAcum.PossuiFreqExterna, CAST(0 AS BIT)) = CAST(0 AS BIT) THEN 0 ELSE 1 END) > 0 
				   THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS PossuiFreqExterna
		FROM @MatriculaTurmaDisicplina Mtd
		INNER JOIN @DadosFechamento AtdAcum
			ON AtdAcum.tud_id = Mtd.tud_id
			AND AtdAcum.alu_id = Mtd.alu_id
			AND AtdAcum.mtu_id = Mtd.mtu_id
			AND AtdAcum.mtd_id = Mtd.mtd_id
			AND Mtd.tpc_id = AtdAcum.tpc_id
			-- Busca dados dos bimestres anteriores.
			AND AtdAcum.tpc_ordem < @tpc_ordem
		WHERE
			Mtd.LinhaRegencia = 1
		GROUP BY
			ISNULL(AtdAcum.tud_idRegencia, AtdAcum.tud_id)
			, AtdAcum.alu_id
			, AtdAcum.mtu_id
			, AtdAcum.mtd_id
	)

	DECLARE @AlunoSituacao TABLE
	(alu_id BIGINT, mtu_id INT, mtu_situacao TINYINT
	 PRIMARY KEY (alu_id, mtu_id, mtu_situacao));

	INSERT INTO @AlunoSituacao
		SELECT
			Mtd.alu_id
			, Mtd.mtu_id
			, CASE WHEN Tmo.tmo_tipoMovimento IN (8,23,27) THEN 1 ELSE Mtd.mtu_situacao END AS mtu_situacao
		FROM @tbAlunos Mtd
		LEFT JOIN MTR_Movimentacao Mov WITH(NOLOCK)
			ON Mov.alu_id = Mtd.alu_id
			AND Mov.mov_situacao <> 3
			AND Mov.mtu_idAnterior = Mtd.mtu_id
		LEFT JOIN MTR_TipoMovimentacao Tmo WITH(NOLOCK)
			ON Tmo.tmo_id = Mov.tmo_id
		GROUP BY 
			Mtd.alu_id
			, Mtd.mtu_id
			, Tmo.tmo_tipoMovimento
			, Mtd.mtu_situacao

	DECLARE @TipoResultadoCrp TABLE 

	(cur_id INT, crr_id INT, crp_id INT, tpr_nomenclatura VARCHAR(100), tpr_resultado TINYINT, tpr_id INT
	 PRIMARY KEY (tpr_resultado, tpr_id));

	INSERT INTO @TipoResultadoCrp
		SELECT TprCrp.cur_id, TprCrp.crr_id, TprCrp.crp_id, Tr.tpr_nomenclatura, Tr.tpr_resultado, Tr.tpr_id
		FROM ACA_TipoResultadoCurriculoPeriodo TprCrp WITH(NOLOCK)
		INNER JOIN ACA_TipoResultado Tr WITH(NOLOCK) 
			on tr.tpr_id = TprCrp.tpr_id
			and tpr_tipoLancamento = 1
			and tr.tpr_situacao <> 3
		--  Busca da série da turma.
		INNER JOIN TUR_TurmaCurriculo Tcr WITH(NOLOCK)
			ON Tcr.tur_id = @tur_id
			AND Tcr.cur_id = TprCrp.cur_id
			AND Tcr.crr_id = TprCrp.crr_id
			AND Tcr.crp_id = TprCrp.crp_id
			AND Tcr.tcr_situacao <> 3

    DECLARE @Dados TABLE
    (tur_id BIGINT, dis_id INT, alu_id BIGINT, mtu_id INT, mtd_id INT, tud_id BIGINT, tud_idRegencia BIGINT,
     Regencia VARCHAR(200), ordem INT, mtu_situacao TINYINT, percentualMinimoFrequencia DECIMAL(5,2),
     percentualMinimoFrequenciaDisciplina DECIMAL(5,2), tpc_id INT, NumeroChamada INT, Nome VARCHAR(200),
     fav_variacao DECIMAL(5,2), BimestreFechado TINYINT, Notas VARCHAR(5), Faltas INT, Compensacoes INT,
     AulasPrevistas INT, TotalFaltas INT, TotalCompensacoes INT, TotalAulasPrevistas INT,
     ParecerConclusivo VARCHAR(105), ComponenteRegncia BIT, qtFaltasReposicao INT,
     atd_frequenciaFinalAjustada INT, atd_frequencia INT, PossuiFreqExternaAtual BIT, PossuiFreqExternaAcum BIT
     PRIMARY KEY (alu_id, mtu_id, mtd_id, tpc_id))

	INSERT INTO @Dados
		SELECT
			Tud.tur_id
			, Tud.dis_id
			, Mtd.alu_id
			, Mtd.mtu_id
			, Mtd.mtd_id
			, Tud.tud_id
			, Tud.tud_idRegencia
			, Tud.tud_nome AS Regencia
			, Tud.tds_ordem AS ordem
			, AluSit.mtu_situacao
			, ISNULL(@percentualMinimoFrequencia, 0) AS percentualMinimoFrequencia 
			, ISNULL(@percentualMinimoFrequenciaDisciplina, 0) AS percentualMinimoFrequenciaDisciplina 
			, @tpc_id AS tpc_id
			, Mtd.mtu_numeroChamada AS NumeroChamada
			, CASE WHEN @documentoOficial = 1 THEN Pes.pes_nomeOficial ELSE Pes.pes_nome END AS Nome
			, @fav_variacao AS fav_variacao
			, ISNULL(AtdAtual.BimestreFechado, 0) AS BimestreFechado
			, CASE WHEN AtdAtual.alu_id IS NULL THEN Calculado.atd_avaliacao ELSE AtdAtual.atd_avaliacao END AS Notas
			, CASE WHEN AtdAtual.alu_id IS NULL 
					THEN ISNULL(Calculado.qtFaltas, 0) 
					ELSE ISNULL(AtdAtual.atd_numeroFaltas, 0) 
				END AS Faltas
			, CASE WHEN AtdAtual.alu_id IS NULL 
					THEN ISNULL(Calculado.cpa_quantidadeAulasCompensadas, 0)
					ELSE ISNULL(AtdAtual.atd_ausenciasCompensadas, 0)
				END AS Compensacoes
			, CASE WHEN AtdAtual.alu_id IS NULL 
					THEN ISNULL(Calculado.tap_aulasPrevitas, 0) 
					ELSE ISNULL(AtdAtual.atd_numeroAulas, 0) 
				END AS AulasPrevistas
			, ISNULL(AtdAcum.atd_numeroFaltas, 0) + 
				CASE WHEN AtdAtual.alu_id IS NULL 
					THEN ISNULL(Calculado.qtFaltas, 0) 
					ELSE ISNULL(AtdAtual.atd_numeroFaltas, 0) 
				END AS TotalFaltas
			, ISNULL(AtdAcum.atd_ausenciasCompensadas, 0) + 
				CASE WHEN AtdAtual.alu_id IS NULL 
					THEN ISNULL(Calculado.cpa_quantidadeAulasCompensadas, 0)
					ELSE ISNULL(AtdAtual.atd_ausenciasCompensadas, 0)
				END AS TotalCompensacoes
			, ISNULL(AtdAcum.atd_numeroAulas, 0) + 
				CASE WHEN AtdAtual.alu_id IS NULL 
					THEN ISNULL(Calculado.tap_aulasPrevitas, 0)
					ELSE ISNULL(AtdAtual.atd_numeroAulas, 0)
				END AS TotalAulasPrevistas
			, CASE WHEN @ultimoBimestre = 1 THEN ISNULL(Tpr.tpr_nomenclatura, '-') ELSE '-' END AS ParecerConclusivo
			, CASE WHEN Mtd.tud_idRegencia IS NOT NULL THEN 1 ELSE 0 END AS ComponenteRegncia
			, ISNULL(Calculado.qtFaltasReposicao, 0) AS qtFaltasReposicao
			, AtdAtual.atd_frequenciaFinalAjustada
			, AtdAtual.atd_frequencia
			, AtdAtual.PossuiFreqExterna AS PossuiFreqExternaAtual
			, AtdAcum.PossuiFreqExterna AS PossuiFreqExternaAcum
		FROM @MatriculaTurmaDisicplina Mtd
		INNER JOIN @tbDisciplinasRelatorio Tud
			ON Tud.tud_id = Mtd.tud_id
		INNER JOIN @AlunoSituacao AluSit
			ON AluSit.alu_id = Mtd.alu_id
			AND AluSit.mtu_id = Mtd.mtu_id
		INNER JOIN ACA_Aluno Alu WITH(NOLOCK)
			ON Alu.alu_id = Mtd.alu_id
		INNER JOIN VW_DadosAlunoPessoa Pes
			ON Pes.alu_id = Alu.alu_id
		LEFT JOIN @DadosFechamentoAcumulado AtdAcum
			ON AtdAcum.tud_id = ISNULL(Mtd.tud_idRegencia, Mtd.tud_id)
			AND AtdAcum.alu_id = Mtd.alu_id
			AND AtdAcum.mtu_id = Mtd.mtu_id
		LEFT JOIN @DadosFechamento AtdAtual
			ON AtdAtual.tud_id = Mtd.tud_id
			AND AtdAtual.alu_id = Mtd.alu_id
			AND AtdAtual.mtu_id = Mtd.mtu_id
			AND AtdAtual.mtd_id = Mtd.mtd_id
			AND AtdAtual.tpc_id = @tpc_id
		LEFT JOIN @DadosBimestreAtualCalculados Calculado
			ON Calculado.tud_id = Mtd.tud_id
			AND Calculado.alu_id = Mtd.alu_id
			AND Calculado.mtu_id = Mtd.mtu_id
			AND Calculado.mtd_id = Mtd.mtd_id
		LEFT JOIN @TipoResultadoCrp Tpr
			ON Tpr.tpr_resultado = Mtd.mtu_resultado
		WHERE
			Mtd.tpc_id = @tpc_id

    DECLARE @tbFreqFinalGeralPorAluno TABLE
    (alu_id BIGINT, FrequenciaFinalGeral DECIMAL(5,2) PRIMARY KEY (alu_id))

    INSERT INTO @tbFreqFinalGeralPorAluno
		SELECT
			D.alu_id
			, dbo.FN_Aplica_VariacaoPorcentagemFrequenciaString(
				ISNULL(CASE WHEN SUM(TotalAulasPrevistas) > 0 THEN
						-- Se não estiver fechada, calcula de acordo com os acumulados.
						(((SUM(CAST(TotalAulasPrevistas AS DECIMAL(18,2))) - 
							(SUM(CAST(TotalFaltas AS DECIMAL(18,2))) + SUM(CAST(qtFaltasReposicao AS DECIMAL(18,2)))
								 - SUM(CAST(TotalCompensacoes AS DECIMAL(18,2))))) / SUM(CAST(TotalAulasPrevistas AS DECIMAL(18,2))))) * 100 
						-- Caso o total de aulas previstas seja 0 a frequência deve ser 100.
						ELSE 100 END
					, 0) , @fav_variacao)
				 AS FrequenciaFinalGeral	

		FROM @Dados AS D
		GROUP BY D.alu_id
	
	;WITH tbRetorno AS
	(
		SELECT
			tur_id
			, dis_id
			, Dados.alu_id
			, mtu_id
			, mtd_id
			, tud_id
			, tud_idRegencia
			, Regencia
			, ordem
			, mtu_situacao
			, percentualMinimoFrequencia
			, percentualMinimoFrequenciaDisciplina
			, tpc_id
			, NumeroChamada
			, Nome
			, fav_variacao
			, BimestreFechado
			, Notas
			, Faltas
			, Compensacoes
			, TotalFaltas
			, TotalCompensacoes
			, ParecerConclusivo
			, TotalAulasPrevistas
			, cast(ComponenteRegncia as int) as ComponenteRegncia
			, qtFaltasReposicao
			, AulasPrevistas

			, CASE WHEN ISNULL(Dados.atd_frequenciaFinalAjustada,0) > 0
					THEN Dados.atd_frequenciaFinalAjustada
					ELSE 
						dbo.FN_Aplica_VariacaoPorcentagemFrequenciaString(
						ISNULL(
								CASE WHEN TotalAulasPrevistas > 0 AND 
									 (((CAST(TotalAulasPrevistas AS DECIMAL(18,2)) - 
										(CAST(TotalFaltas AS DECIMAL(18,2)) + CAST(qtFaltasReposicao AS DECIMAL(18,2))
											- CAST(TotalCompensacoes AS DECIMAL(18,2)))) / CAST(TotalAulasPrevistas AS DECIMAL(18,2)))) * 100 > 100
									 THEN 100
									 WHEN TotalAulasPrevistas > 0 THEN
								-- Se não estiver fechada, calcula de acordo com os acumulados.
								(((CAST(TotalAulasPrevistas AS DECIMAL(18,2)) - 
									(CAST(TotalFaltas AS DECIMAL(18,2)) + CAST(qtFaltasReposicao AS DECIMAL(18,2))
											- CAST(TotalCompensacoes AS DECIMAL(18,2)))) / CAST(TotalAulasPrevistas AS DECIMAL(18,2)))) * 100 
								-- Caso o total de aulas previstas seja 0 a frequência deve ser 100.
								ELSE 100 END
							, 0)
						, @fav_variacao)
					END
					AS FrequenciaFinal
			--
			-- codigo comentado e substituido pelo de cima devido a um ajuste na coluna frequencia da disciplina que estava dando divergencia.
			--
			--, dbo.FN_Aplica_VariacaoPorcentagemFrequenciaString(
			--	ISNULL(CASE WHEN BimestreFechado = 1 THEN
			--			-- Caso o bimestre tenha fechamento traz a frequencia final dele.
			--			atd_frequenciaFinalAjustada
			--		ELSE
			--			CASE WHEN TotalAulasPrevistas > 0 THEN
			--			-- Se não estiver fechada, calcula de acordo com os acumulados.
			--			(((CAST(TotalAulasPrevistas AS DECIMAL(18,2)) - 
			--				(CAST(TotalFaltas AS DECIMAL(18,2)) + CAST(qtFaltasReposicao AS DECIMAL(18,2))
			--					 - CAST(TotalCompensacoes AS DECIMAL(18,2)))) / CAST(TotalAulasPrevistas AS DECIMAL(18,2)))) * 100 
			--			-- Caso o total de aulas previstas seja 0 a frequência deve ser 100.
			--			ELSE 100 END
			--		END, 0)
			--	, @fav_variacao)
			--	 AS FrequenciaFinal

			, dbo.FN_Aplica_VariacaoPorcentagemFrequenciaString(
				ISNULL(CASE WHEN BimestreFechado = 1 THEN
						-- Caso o bimestre tenha fechamento traz a frequencia final dele.
						atd_frequencia
					ELSE
						CASE WHEN AulasPrevistas > 0 AND
							 (((CAST(AulasPrevistas AS DECIMAL(18,2)) - CAST(Faltas AS DECIMAL(18,2))) / CAST(AulasPrevistas AS DECIMAL(18,2)))) * 100 > 100
							 THEN 100
							 WHEN AulasPrevistas > 0 THEN
						-- Se não estiver fechada, calcula de acordo com os acumulados.
						(((CAST(AulasPrevistas AS DECIMAL(18,2)) - CAST(Faltas AS DECIMAL(18,2))) / CAST(AulasPrevistas AS DECIMAL(18,2)))) * 100 
						-- Caso as aulas previstas seja 0 a frequência deve ser 100.
						ELSE 100 END
					END, 0)
				, @fav_variacao)
				 AS FrequenciaBimestre
			, @ultimoBimestre AS UltimoBimestre
			, @esa_tipo AS esa_tipo
			, freqFinalGeral.FrequenciaFinalGeral
			, PossuiFreqExternaAtual
			, PossuiFreqExternaAcum
		FROM 
			@Dados Dados
			LEFT JOIN @tbFreqFinalGeralPorAluno freqFinalGeral
				ON Dados.alu_id = freqFinalGeral.alu_id
	)
	, movimentacao AS (
		SELECT
			Da.alu_id,
			Da.mtu_id,
			CASE WHEN tmo_tipoMovimento IN (6, 12, 14, 15, 16)
					THEN 'TR ' + REPLACE(CONVERT(VARCHAR(10), mov.mov_dataRealizacao, 103), '/' + CAST(DATEPART(YEAR, mov.mov_dataRealizacao) AS VARCHAR(4)), '') +
						 ISNULL(' - ' + turD.tur_codigo, '')
					WHEN tmo_tipoMovimento IN (8)
					THEN 'RM' + ISNULL(' ' + turD.tur_codigo, '')
					WHEN tmo_tipoMovimento IN (11)
					THEN 'RC' + ISNULL(' ' + turD.tur_codigo, '')
					ELSE ''
			END movMsg
		FROM tbRetorno Da
		INNER JOIN MTR_Movimentacao mov WITH(NOLOCK)
			ON Da.alu_id = mov.alu_id
			AND Da.mtu_id = mov.mtu_idAnterior
			AND mov.mov_situacao <> 3
		INNER JOIN MTR_TipoMovimentacao tmo WITH(NOLOCK)
			ON mov.tmo_id = tmo.tmo_id
			AND tmo_tipoMovimento IN (6, 8, 11, 12, 14, 15, 16)
			AND tmo.tmo_situacao <> 3
		LEFT JOIN MTR_MatriculaTurma mtuD WITH(NOLOCK)
			ON mov.alu_id = mtuD.alu_id
			AND mov.mtu_idAtual = mtuD.mtu_id
		LEFT JOIN TUR_Turma turD WITH(NOLOCK)
			ON mtuD.tur_id = turD.tur_id
		LEFT JOIN ACA_CalendarioAnual calD WITH(NOLOCK)
			ON turD.cal_id = calD.cal_id
		INNER JOIN MTR_MatriculaTurma mtuO WITH(NOLOCK)
			ON mov.alu_id = mtuO.alu_id
			AND mov.mtu_idAnterior = mtuO.mtu_id
			AND mtuO.tur_id = @tur_id
		LEFT JOIN TUR_Turma turO WITH(NOLOCK)
			ON mtuO.tur_id = turO.tur_id
		LEFT JOIN ACA_CalendarioAnual calO WITH(NOLOCK)
			ON turO.cal_id = calO.cal_id
		WHERE 
			turD.tur_id IS NULL OR calD.cal_ano = calO.cal_ano --Ou não tem turma destino ou a turma destino é do mesmo ano
		GROUP BY
			Da.alu_id,
			Da.mtu_id,
			tmo_tipoMovimento,
			mov.mov_dataRealizacao,
			turD.tur_codigo
	)	

	SELECT
		tur_id
		, dis_id
		, Da.alu_id
		, Da.mtu_id
		, mtd_id
		, tud_id
		, tud_idRegencia
		, Regencia
		, ordem
		, mtu_situacao
		, percentualMinimoFrequencia
		, percentualMinimoFrequenciaDisciplina
		, tpc_id
		, NumeroChamada
		, Da.Nome +
		  CASE WHEN ISNULL(mov.movMsg, '') = ''
			   THEN ''
		  	   ELSE ' (' + mov.movMsg + ')'
		  END AS Nome
		, fav_variacao
		, BimestreFechado
		, Notas
		, Faltas
		, Compensacoes
		, TotalFaltas
		, TotalCompensacoes
		, ParecerConclusivo
		, TotalAulasPrevistas
		, ComponenteRegncia
		, qtFaltasReposicao
		, AulasPrevistas
		, CASE WHEN FrequenciaFinal < 0 THEN 0 ELSE FrequenciaFinal END AS FrequenciaFinal
		, CASE WHEN FrequenciaBimestre < 0 THEN 0 ELSE FrequenciaBimestre END AS FrequenciaBimestre
		, UltimoBimestre
		, esa_tipo
		, CASE WHEN FrequenciaFinalGeral < 0 THEN 0 ELSE FrequenciaFinalGeral END AS FrequenciaFinalGeral
		, PossuiFreqExternaAtual
		, PossuiFreqExternaAcum
		, @possuiFreqExterna AS possuiFreqExterna
	FROM tbRetorno Da
	LEFT JOIN movimentacao mov
		ON Da.alu_id = mov.alu_id
		AND Da.mtu_id = mov.mtu_id	
END
GO
PRINT N'Altering [dbo].[ACA_EventoLimite]'
GO
ALTER TABLE [dbo].[ACA_EventoLimite] ADD
[uad_id] [uniqueidentifier] NULL
GO
PRINT N'Altering [dbo].[STP_ACA_EventoLimite_INSERT]'
GO
ALTER PROCEDURE [dbo].[STP_ACA_EventoLimite_INSERT]
	@cal_id Int
	, @tev_id Int
	, @evl_id Int
	, @tpc_id Int
	, @esc_id Int
	, @uni_id Int
	, @evl_dataInicio DateTime
	, @evl_dataFim DateTime
	, @usu_id UniqueIdentifier
	, @evl_situacao TinyInt
	, @evl_dataCriacao DateTime
	, @evl_dataAlteracao DateTime
	, @uad_id UniqueIdentifier

AS
BEGIN
	INSERT INTO 
		ACA_EventoLimite
		( 
			cal_id 
			, tev_id 
			, evl_id 
			, tpc_id 
			, esc_id 
			, uni_id 
			, evl_dataInicio 
			, evl_dataFim 
			, usu_id 
			, evl_situacao 
			, evl_dataCriacao 
			, evl_dataAlteracao 
			, uad_id
		)
	VALUES
		( 
			@cal_id 
			, @tev_id 
			, @evl_id 
			, @tpc_id 
			, @esc_id 
			, @uni_id 
			, @evl_dataInicio 
			, @evl_dataFim 
			, @usu_id 
			, @evl_situacao 
			, @evl_dataCriacao 
			, @evl_dataAlteracao 
			, @uad_id
		)
		
		SELECT ISNULL(SCOPE_IDENTITY(),-1)

	
	
END
GO
PRINT N'Altering [dbo].[NEW_MTR_MatriculaTurmaDisciplina_SelectBy_TurmaDisciplinaFormatoFiltroDeficiencia]'
GO
-- Stored Procedure

-- ========================================================================
-- Author:		Daniel Jun Suguimoto
-- Create date: 12/03/2014
-- Description: Retorna os alunos matriculados na Turma para o período informado,
--				de acordo com as regras necessárias para ele aparecer na listagem
--				para efetivar.
--				Filtrando os alunos com ou sem deficiência, dependendo do docente.
-- Alterado: Marcia Haga - 19/08/2014
-- Description: Adicionada validacao para retornar apenas registros ativos (situacao <> 3)
-- das tabelas CLS_AlunoAvaliacaoTurmaObservacao e CLS_AlunoAvaliacaoTurmaDisciplinaObservacao

-- Alterado: Marcia Haga - 19/09/2014
-- Description: Retornando o mtu_resultado, para marcar o check no registro do conselho 
-- de classe nos casos em que a aba do parecer conclusivo aparece no pop-up.

-- Alterado: Katiusca Murari - 06/10/2014
-- Description: Adicionada a variação na hora de calcular a frequencia.

---- Alterado: Marcia Haga - 13/03/2015
---- Description: Alterado para considerar a frequencia como 100%,
---- caso o numero de aulas previstas nao tenham sido informadas.

---- Alterado: Marcia Haga - 10/08/2015
---- Description: Alterado para verificar o periodo em que o aluno esteve 
---- presente na turma eletiva de aluno ou multisseriada.
-- ========================================================================
ALTER PROCEDURE [dbo].[NEW_MTR_MatriculaTurmaDisciplina_SelectBy_TurmaDisciplinaFormatoFiltroDeficiencia]
	@tud_id BIGINT
	, @tur_id BIGINT
	, @tpc_id INT
	, @ava_id INT
	, @ordenacao INT
	, @fav_id INT	
	, @tipoAvaliacao TINYINT
	, @esa_id INT
	, @tipoEscalaDisciplina TINYINT
	, @tipoEscalaDocente TINYINT
	, @avaliacaoesRelacionadas NVARCHAR(MAX) = ''
	, @notaMinimaAprovacao DECIMAL(27,4)
	, @ordemParecerMinimo INT
	, @tipoLancamento TINYINT	
	, @fav_calculoQtdeAulasDadas TINYINT
	, @permiteAlterarResultado BIT
	, @tur_tipo TINYINT
	, @cal_id INT
	, @exibirNotaFinal BIT
	, @tdc_id TINYINT 
	, @dtTurma TipoTabela_Turma READONLY
	, @documentoOficial BIT
AS
BEGIN 

	SET TRANSACTION ISOLATION LEVEL SNAPSHOT

	SET @avaliacaoesRelacionadas = CASE WHEN @avaliacaoesRelacionadas = '' THEN NULL ELSE @avaliacaoesRelacionadas END

	-- Armazena exibir compensacao ausencia cadastrada
    DECLARE @ExibeCompensacao BIT
    SELECT TOP 1
		@ExibeCompensacao = CASE WHEN (pac_valor = 'True') THEN 1 ELSE 0 END
    FROM
        ACA_ParametroAcademico -- WITH (NOLOCK)
    WHERE
        pac_chave = 'EXIBIR_COMPENSACAO_AUSENCIA_CADASTRADA'



	DECLARE @MatriculasBoletimDaTurma AS TipoTabela_MatriculasBoletim;

	DECLARE @Matriculas AS TipoTabela_MatriculasBoletimDisciplina;

	DECLARE @SomatorioAulasFaltas TABLE (alu_id BIGINT NOT NULL, aulas INT, faltas INT, faltasReposicao INT, compensadas INT, faltasAnteriores INT, compensadasAnteriores INT);

	DECLARE @tpc_idProximo INT = NULL;

	DECLARE @MatriculaMultisseriadaTurmaAluno TABLE 
		(
			tud_idDocente BIGINT, 
			alu_id BIGINT, 
			mtu_id INT, 
			mtd_id INT
			PRIMARY KEY (tud_idDocente, alu_id, mtu_id, mtd_id)
		);

	DECLARE @tds_id INT = 
		(
			 --Busca o tipo de disciplina para filtrar os mtds abaixo.
			SELECT Dis.tds_id
			FROM TUR_TurmaDisciplinaRelDisciplina RelDis -- WITH (NOLOCK)
			INNER JOIN ACA_Disciplina Dis -- WITH (NOLOCK)
				ON Dis.dis_id = RelDis.dis_id
			WHERE
				RelDis.tud_id = @tud_id
		)

	-- Se for turma de eletiva do aluno, carrega os alunos que devem aparecer na tela de efetivação
	IF ( @tur_tipo IN (2,3) ) 
	BEGIN
		-- Turma eletiva de aluno ou multisseriada, buscar matrículas por aluno.
		DECLARE @tbMatriculaAlunos TipoTabela_AlunoMatriculaTurma;
		INSERT INTO @tbMatriculaAlunos (alu_id, mtu_id)
		SELECT Mtd.alu_id, MAX(Mtd.mtu_id)
		FROM MTR_MatriculaTurmaDisciplina Mtd -- WITH(NOLOCK)
		WHERE
			-- Busca mtus dos alunos pelo MTD - o mtd está ligado ao tud informado, e o mtu_id aponta pra outra turma (turma normal).
			Mtd.tud_id = @tud_id
			AND Mtd.mtd_situacao <> 3
		GROUP BY alu_id	

		INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
			PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
			PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
		Select mb.alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mb.mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes 
		  from MTR_MatriculasBoletim mb  WITH(NOLOCK)
			   inner join @tbMatriculaAlunos amt on amt.alu_id = mb.alu_id and amt.mtu_id = mb.mtu_origemDados

		IF not exists (select top 1 alu_id from @MatriculasBoletimDaTurma)
		begin
			INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
			PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
			PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)

			EXEC NEW_MTR_MatriculaTurma_MatriculasBoletim_Alunos
				@tbMatriculaTurma = @tbMatriculaAlunos
		end	
	END
	ELSE IF (@tur_tipo = 4)
	BEGIN
		-- Turma eletiva de aluno ou multisseriada, buscar matrículas por aluno.
		DECLARE @tbMatriculaAlunosMultisseriada TipoTabela_AlunoMatriculaTurma;
		INSERT INTO @tbMatriculaAlunosMultisseriada (alu_id, mtu_id)
		SELECT Mtd.alu_id, MAX(Mtd.mtu_id)
		FROM MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
		INNER JOIN MTR_MatriculaTurma mtu
			ON Mtd.alu_id = mtu.alu_id
			AND Mtd.mtu_id = mtu.mtu_id
			AND mtu.mtu_situacao <> 3
		INNER JOIN @dtTurma dtt
			ON mtu.tur_id = dtt.tur_id
		WHERE
			-- Busca mtus dos alunos pelo MTD - o mtd está ligado ao tud informado, e o mtu_id aponta pra outra turma (turma normal).
			Mtd.tud_id = @tud_id
			AND Mtd.mtd_situacao <> 3
		GROUP BY mtd.alu_id	

		INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
		Select mb.alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mb.mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes 
		  from MTR_MatriculasBoletim mb  -- WITH (NOLOCK)
			   inner join @tbMatriculaAlunosMultisseriada amt on amt.alu_id = mb.alu_id and amt.mtu_id = mb.mtu_origemDados

		IF not exists (select top 1 alu_id from @MatriculasBoletimDaTurma)
		begin
			INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
				PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
				PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
			EXEC NEW_MTR_MatriculaTurma_MatriculasBoletim_Alunos
				@tbMatriculaTurma = @tbMatriculaAlunosMultisseriada
		end

		INSERT INTO @MatriculaMultisseriadaTurmaAluno (tud_idDocente, alu_id, mtu_id, mtd_id)
		SELECT 
			mtdDocente.tud_id AS tud_idDocente,
			mtdAluno.alu_id,
			mtdAluno.mtu_id,
			mtdAluno.mtd_id
		FROM @MatriculasBoletimDaTurma mtr
		INNER JOIN MTR_MatriculaTurmaDisciplina mtdDocente
			ON mtr.alu_id = mtdDocente.alu_id
			AND mtr.mtu_id = mtdDocente.mtu_id
			AND mtdDocente.tud_id = @tud_id
			AND mtdDocente.mtd_situacao <> 3
		INNER JOIN MTR_MatriculaTurmaDisciplina mtdAluno
			ON mtdAluno.alu_id = mtr.alu_id
			AND mtdAluno.mtu_id = mtr.mtu_id
			AND mtdAluno.mtd_situacao <> 3
		INNER JOIN TUR_TurmaDisciplina tudAluno
			ON mtdAluno.tud_id = tudAluno.tud_id
			AND tudAluno.tud_id <> @tud_id
			AND tudAluno.tud_situacao <> 3
		INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDisAluno
			ON RelDisAluno.tud_id = tudAluno.tud_id
		INNER JOIN ACA_Disciplina disAluno
			ON RelDisAluno.dis_id = disAluno.dis_id
			AND disAluno.tds_id = @tds_id
			AND disAluno.dis_situacao <> 3
		GROUP BY
			mtdDocente.tud_id,
			mtdAluno.alu_id,
			mtdAluno.mtu_id,
			mtdAluno.mtd_id
	END
	ELSE
	BEGIN
		-- Se for turma normal, carrega os alunos de acordo com o boletim
		INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
			PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
			PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
		select mb.alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mb.mtu_id,mb.tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mb.mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes
		  from MTR_MatriculasBoletim mb
			   inner join (select alu_id, mtu_id, ROW_NUMBER() OVER(PARTITION BY alu_id 
														   ORDER BY mtu_id desc) as linha
							 from MTR_MatriculaTurma -- WITH(NOLOCK) 
							where mtu_situacao <> 3
							  and tur_id = @tur_id) mtu 
					   on mtu.alu_id = mb.alu_id and mtu.mtu_id = mb.mtu_origemDados
		 where mtu.linha = 1 --para evitar casos onde o aluno tem dois mtus para o mesmo tur_id

		IF not exists (select top 1 alu_id from @MatriculasBoletimDaTurma)
		begin
			INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
				PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
				PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
			EXEC NEW_MTR_MatriculaTurma_MatriculasBoletim_DaTurma
				@tur_id = @tur_id;
		end
	END

    IF (@tur_tipo = 4)
	BEGIN
		INSERT INTO @Matriculas 
		(
			alu_id, 
			mtu_id, 
			mtd_id, 
			fav_id, 
			tpc_id, 
			tpc_ordem, 
			tud_id, 
			tur_id, 
			registroExterno, 
			PossuiSaidaPeriodo, 
			variacaoFrequencia, 
			mtd_numeroChamadaDocente,
			mtd_situacaoDocente, 
			mtd_dataMatriculaDocente, 
			mtd_dataSaidaDocente
		)
		SELECT
			Mtr.alu_id
			, Mtr.mtu_id
			, Mtd.mtd_id
			, Mtr.fav_id
			, Mtr.tpc_id
			, Mtr.tpc_ordem
			, mtd.tud_id
			, Mtr.tur_id
			, Mtr.registroExterno
			, Mtr.PossuiSaidaPeriodo
			, FAV.fav_variacao
			, mtdDocente.mtd_numeroChamada
			, mtdDocente.mtd_situacao
			, mtdDocente.mtd_dataMatricula
			, mtdDocente.mtd_dataSaida
		FROM @MatriculasBoletimDaTurma Mtr
		INNER JOIN MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
			ON Mtd.alu_id = Mtr.alu_id
			AND Mtd.mtu_id = Mtr.mtu_id
			AND Mtd.mtd_situacao <> 3
		INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDis -- WITH (NOLOCK)
			ON RelDis.tud_id = Mtd.tud_id
		INNER JOIN ACA_Disciplina Dis -- WITH (NOLOCK)
			ON RelDis.dis_id = Dis.dis_id
		INNER JOIN dbo.ACA_FormatoAvaliacao FAV -- WITH (NOLOCK)
			ON	FAV.fav_id = Mtr.fav_id
			AND FAV.fav_situacao <> 3
		INNER JOIN @MatriculaMultisseriadaTurmaAluno tdm 
			ON Mtd.alu_id = tdm.alu_id
			AND Mtd.mtu_id = tdm.mtu_id
			AND Mtd.mtd_id = tdm.mtd_id
		INNER JOIN MTR_MatriculaTurmaDisciplina mtdDocente
			ON mtdDocente.alu_id = Mtd.alu_id
			AND mtdDocente.mtu_id = Mtd.mtu_id
			AND mtdDocente.tud_id = tdm.tud_idDocente
			AND mtdDocente.mtd_situacao <> 3
		WHERE
			Mtr.mtu_id IS NOT NULL
			 --Busca a matrícula na mtd ligada à disciplina (pelo tds_id), em cada COC, da lista de mtus (MatriculasBoletim).
			AND Dis.tds_id = @tds_id
			 --Filtros de matrícula.
			AND Mtr.MesmoCalendario = 1
			--AND Mtr.PeriodosEquivalentes = 1
	END
	ELSE
	BEGIN
		INSERT INTO @Matriculas (alu_id, mtu_id, mtd_id, fav_id, tpc_id, tpc_ordem, tud_id, tur_id, registroExterno, PossuiSaidaPeriodo, variacaoFrequencia)
		SELECT
			Mtr.alu_id
			, Mtr.mtu_id
			, Mtd.mtd_id
			, Mtr.fav_id
			, Mtr.tpc_id
			, Mtr.tpc_ordem
			, Mtd.tud_id
			, Mtr.tur_id
			, Mtr.registroExterno
			, Mtr.PossuiSaidaPeriodo
			, FAV.fav_variacao
		FROM @MatriculasBoletimDaTurma Mtr
		INNER JOIN MTR_MatriculaTurmaDisciplina Mtd --WITH (NOLOCK)
			ON Mtd.alu_id = Mtr.alu_id
			AND Mtd.mtu_id = Mtr.mtu_id
			AND Mtd.mtd_situacao <> 3
		INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDis --WITH (NOLOCK)
			ON RelDis.tud_id = Mtd.tud_id
		INNER JOIN ACA_Disciplina Dis --WITH (NOLOCK)
			ON RelDis.dis_id = Dis.dis_id
		INNER JOIN dbo.ACA_FormatoAvaliacao FAV --WITH (NOLOCK)
			ON	FAV.fav_id = Mtr.fav_id
			AND FAV.fav_situacao <> 3
		WHERE
			Mtr.mtu_id IS NOT NULL
			 --Busca a matrícula na mtd ligada à disciplina (pelo tds_id), em cada COC, da lista de mtus (MatriculasBoletim).
			AND Dis.tds_id = @tds_id
			 --Filtros de matrícula.
			AND Mtr.MesmoCalendario = 1
			--Verifica períodos equivalentes apenas para turmas normais (1)
			AND (Mtr.PeriodosEquivalentes = 1 OR @tur_tipo <> 1)
    END

	-- Verifica o periodo em que o aluno esteve presente na turma eletiva de aluno ou multisseriada
	IF ( @tur_tipo IN (2,3,4) ) 
	BEGIN
		;WITH PresencaAlunoPeriodo AS
		(
			SELECT Mat.alu_id, Mat.mtu_id, Mat.mtd_id, Mat.tpc_id 
			FROM @Matriculas Mat
			INNER JOIN TUR_Turma Tur -- WITH (NOLOCK)
				ON Tur.tur_id = Mat.tur_id
			INNER JOIN MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
				ON Mtd.alu_id = Mat.alu_id
				AND Mtd.mtu_id = Mat.mtu_id
				AND Mtd.mtd_id = Mat.mtd_id
			INNER JOIN ACA_TipoPeriodoCalendario Tpc -- WITH (NOLOCK)
				ON Tpc.tpc_id = Mat.tpc_id
			INNER JOIN ACA_CalendarioPeriodo Cap -- WITH (NOLOCK)
				ON Cap.tpc_id = Tpc.tpc_id
				AND Cap.cal_id = Tur.cal_id
				AND Cap.cap_situacao <> 3
			WHERE
			(
				-- O aluno nao estava presente no periodo se:
				-- o aluno saiu durante o periodo
				Mtd.mtd_dataSaida BETWEEN Cap.cap_dataInicio AND Cap.cap_dataFim
				-- ou o aluno saiu antes de o periodo iniciar
				OR Mtd.mtd_dataSaida < Cap.cap_dataInicio
				-- ou o aluno entrou depois do fim do periodo
				OR Mtd.mtd_dataMatricula > Cap.cap_dataFim
			)
			AND Mat.PossuiSaidaPeriodo = 0
		)
		UPDATE @Matriculas
		SET PossuiSaidaPeriodo = 1
		FROM @Matriculas Mat
		INNER JOIN PresencaAlunoPeriodo Pap
			ON Pap.alu_id = Mat.alu_id
			AND Pap.mtu_id = Mat.mtu_id
			AND Pap.mtd_id = Mat.mtd_id
			AND Pap.tpc_id = Mat.tpc_id
	END

	-- Armazenar médias dos alunos
	DECLARE @tbMediasAlunos TABLE (alu_id BIGINT, mtu_id INT, Media VARCHAR(20))

	-- Parâmetro que define se aparece o campo média final na tela de avaliações (caso positivo, traz a nota informada lá,
	-- caso negativo calcula a média).
	IF (@exibirNotaFinal = 0)
	BEGIN
		IF (@tipoEscalaDocente = 2) --pareceres
		BEGIN
			INSERT INTO @tbMediasAlunos (alu_id, mtu_id, Media)
			SELECT alu_id, mtu_id, Media
			FROM FN_CalculaConceitoAluno_Por_DisciplinaPeriodo_TodosAlunos(@tpc_id, @tud_id)
		END
		ELSE
		BEGIN
			DECLARE @fav_calcularMediaAvaliacaoFinal BIT = (SELECT fav_calcularMediaAvaliacaoFinal FROM ACA_FormatoAvaliacao WHERE fav_id = @fav_id);
			-- Caso esteja marcado para calcular a média da avaliação final.
			IF (@tipoAvaliacao = 3 AND @fav_calcularMediaAvaliacaoFinal = 1) -- 3-Final
			BEGIN
				--Calcular média dos alunos.
				INSERT INTO @tbMediasAlunos (alu_id, mtu_id, Media)
				EXEC dbo.NEW_CLS_AlunoAvaliacaoTurmaDisciplina_MediasFinaisPor_PesoAvaliacoes 
					@fav_id = @fav_id, @MatriculasBoletimDisciplina = @Matriculas
			END
			ELSE
			BEGIN
				-- Calcular média dos alunos.
				INSERT INTO @tbMediasAlunos (alu_id, mtu_id, Media)
				SELECT alu_id, mtu_id, Media
				FROM FN_CalculaMediaAluno_Por_DisciplinaPeriodo_TodosAlunos(@tpc_id, @tud_id, @fav_id)
			END
		END
	END
	ELSE
	BEGIN
		-- Buscar as médias salvas na tabela de Notas finais.
		INSERT INTO @tbMediasAlunos (alu_id, mtu_id, Media)
		SELECT Atm.alu_id, Atm.mtu_id, Atm.atm_media
		FROM CLS_AlunoAvaliacaoTurmaDisciplinaMedia Atm -- WITH(NOLOCK)
		WHERE
			Atm.tud_id = @tud_id
			AND Atm.tpc_id = @tpc_id
			AND Atm.atm_situacao <> 3
	END

	IF (@tpc_id IS NULL)
	BEGIN

		-- Seleciona o próximo período da avaliação periódica relacionada à avliação (recuperação).
		-- Por exemplo: se a avaliação de recuperação é a Rec. do 3º COC, vai usar a matrícula que
		--				deve ser efetivada no 4º COC, pois a recuperação sempre acontece no coc seguinte.
		SET @tpc_idProximo = (  SELECT 
		                            TOP 1 TpcProximo.tpc_id
		                        FROM
		                            ACA_Avaliacao                        AvaPeriodica   --WITH(NOLOCK)
		                            INNER JOIN ACA_TipoPeriodoCalendario Tpc           -- WITH (NOLOCK)  
										 ON  Tpc.tpc_id = AvaPeriodica.tpc_id
		                            INNER JOIN ACA_TipoPeriodoCalendario TpcProximo    -- WITH (NOLOCK)  
										 ON  TpcProximo.tpc_ordem    =  (Tpc.tpc_ordem + 1) 
										 AND TpcProximo.tpc_situacao <> 3
		                        WHERE
									AvaPeriodica.fav_id = @fav_id
		                            AND AvaPeriodica.ava_id IN (SELECT valor FROM FN_StringToArrayInt32(ISNULL(@avaliacaoesRelacionadas,0), ','))		                            
		                        ORDER BY 
		                            Tpc.tpc_ordem DESC
		                     );
	END

	-- Todos os alunos dessas matrículas que estão de recuperação (de acordo com os ava_ids Relacionados).
	DECLARE @notasPeriodicasRecuperacao TABLE (
	      alu_id        BIGINT
	    , mtu_id        INT
	    , mtd_id        INT
	    , recuperacao   BIT
	);

	-- Recuperacao = 2. Insere na tabela as notas da avaliação relacionada à recuperação.
	IF (@tipoAvaliacao = 2)
	BEGIN

		INSERT INTO @notasPeriodicasRecuperacao (alu_id, mtu_id, mtd_id, recuperacao)
		SELECT
			  Mtr.alu_id
			, Mtr.mtu_id
			, Mtr.mtd_id
			, 1
		FROM @Matriculas Mtr
		-- Mtu responsável no período anterior ao que é responsável na avaliação de recuperação.
		INNER JOIN @Matriculas MtrAnterior 
			ON      MtrAnterior.alu_id    = Mtr.alu_id
			    AND MtrAnterior.tpc_ordem = (Mtr.tpc_ordem - 1)
		INNER JOIN CLS_AlunoAvaliacaoTurmaDisciplina Atd --WITH (NOLOCK)
			ON      Atd.tud_id = MtrAnterior.tud_id
			    AND Atd.alu_id = MtrAnterior.alu_id
			    AND Atd.mtu_id = MtrAnterior.mtu_id
			    AND Atd.mtd_id = MtrAnterior.mtd_id
			    AND Atd.fav_id = @fav_id
			    AND Atd.ava_id IN (SELECT valor FROM FN_StringToArrayInt32(ISNULL(@avaliacaoesRelacionadas,0), ','))
			    AND Atd.atd_situacao <> 3
		INNER JOIN ACA_Avaliacao Ava --WITH (NOLOCK)
			ON      Ava.fav_id = Atd.fav_id
			    AND Ava.ava_id = Atd.ava_id
			    AND Ava.ava_situacao <> 3
			    -- Filtrar avaliações ligadas ao período anterior.
			    AND Ava.tpc_id = MtrAnterior.tpc_id
			WHERE
				(
					(@tipoEscalaDisciplina = 1 AND 
						CAST(ISNULL(REPLACE(ISNULL(atd.atd_avaliacaoPosConselho, atd.atd_avaliacao),',','.'),0) AS DECIMAL(27,4)) < @notaMinimaAprovacao )
					OR
					(@tipoEscalaDisciplina = 2 AND 
						CAST(ISNULL((SELECT ordem FROM FN_ACA_EscalaAvaliacaoParecer_RetornarOrdem(@esa_id, ISNULL(atd.atd_avaliacaoPosConselho, atd.atd_avaliacao))),0) AS INT) < @ordemParecerMinimo)
				)
				AND ISNULL(Atd.atd_semProfessor,0) = 0
	END

		;WITH TabelaPeriodosAnteriores AS (
		SELECT 
			tpc.tpc_id, 
			ava.ava_id, 
			ava.fav_id 
		FROM dbo.ACA_TipoPeriodoCalendario AS tpc --WITH (NOLOCK) 
		INNER JOIN dbo.ACA_Avaliacao AS ava --WITH (NOLOCK)
			ON ava.fav_id=@fav_id
			AND tpc.tpc_id = ava.tpc_id	
			AND ava.ava_situacao<>3	
		WHERE tpc_ordem <= (SELECT tpc_ordem FROM dbo.ACA_TipoPeriodoCalendario --WITH (NOLOCK) 
							WHERE tpc_id=@tpc_id)
	)
	,
	TabelaFaltasAulas AS (
		SELECT * 
		FROM FN_Select_FaltasAulasBy_TurmaDisciplina(@tipoLancamento, @tpc_id, @tud_id, @fav_calculoQtdeAulasDadas, DEFAULT)
	)
	, Compensacoes AS 
	(
		-- Trazer as compensações de cada bimestre agrupadas, para trazer um registro único por bimestre.
		SELECT
			mat.alu_id, mat.mtu_id, mat.mtd_id, mat.tpc_id
			, SUM(ISNULL(cpa.cpa_quantidadeAulasCompensadas, 0)) AS cpa_quantidadeAulasCompensadas
		FROM @Matriculas AS mat
		INNER JOIN CLS_CompensacaoAusencia cpa --WITH(NOLOCK)
		ON cpa.tud_id = @tud_id
			AND mat.tpc_id=cpa.tpc_id
			AND cpa.cpa_situacao = 1
		INNER JOIN CLS_CompensacaoAusenciaAluno caa-- WITH(NOLOCK)
			ON  caa.tud_id = cpa.tud_id
				AND caa.cpa_id = cpa.cpa_id
				AND caa.caa_situacao = 1
				AND caa.alu_id=mat.alu_id
		GROUP BY mat.alu_id, mat.mtu_id, mat.mtd_id, mat.tpc_id
	)

	INSERT INTO @SomatorioAulasFaltas (alu_id, faltas, faltasReposicao, aulas, compensadas, faltasAnteriores, compensadasAnteriores)
	SELECT 
		mat.alu_id,
		SUM(CASE WHEN @tpc_id=mat.tpc_id
					THEN COALESCE(atd.atd_numeroFaltas,qtfaltas,0)
				ELSE  ISNULL(atd.atd_numeroFaltas,0)
			END) AS faltas,
		ISNULL(qtFaltasReposicao,0) AS faltasReposicao, 
		SUM(CASE WHEN @tpc_id=mat.tpc_id
					THEN COALESCE(atd.atd_numeroAulas,qtAulas,0)
				ELSE  ISNULL(atd.atd_numeroAulas,0)
			END) AS aulas,	
		SUM(CASE WHEN @tpc_id <> mat.tpc_id 
            THEN ISNULL(atd.atd_ausenciasCompensadas,0) 
            ELSE ( 
					CASE WHEN Atd.atd_id IS NOT NULL 
                    THEN ISNULL(atd.atd_ausenciasCompensadas, 0) 
                    ELSE ISNULL(cpa.cpa_quantidadeAulasCompensadas,0) 
                    END 
                 ) 
			END) AS compensadas,
		SUM(CASE WHEN @tpc_id=mat.tpc_id
						THEN 0
					ELSE  ISNULL(atd.atd_numeroFaltas,0)
				END) + ISNULL(qtFaltasReposicao,0) AS faltasAnteriores,
		SUM(CASE WHEN @tpc_id <> mat.tpc_id 
				THEN ISNULL(atd.atd_ausenciasCompensadas,0) 
				ELSE 0
				END) AS compensadasAnteriores
	FROM @Matriculas AS mat
		INNER JOIN TabelaPeriodosAnteriores tpa
			ON tpa.tpc_id=mat.tpc_id	
	    LEFT JOIN CLS_AlunoAvaliacaoTurmaDisciplina Atd-- WITH(NOLOCK)
	        ON  Atd.tud_id = mat.tud_id
				AND Atd.alu_id = mat.alu_id
				AND Atd.mtu_id = mat.mtu_id
				AND Atd.mtd_id = mat.mtd_id
				AND atd.fav_id=tpa.fav_id
				AND atd.ava_id=tpa.ava_id
				AND Atd.atd_situacao <> 3
	    LEFT JOIN TabelaFaltasAulas tfa 
	        ON  mat.alu_id = tfa.alu_id
	    LEFT JOIN Compensacoes Cpa
			ON Cpa.alu_id = mat.alu_id
			AND Cpa.mtu_id = mat.mtu_id
			AND Cpa.mtd_id = mat.mtd_id
			AND Cpa.tpc_id = mat.tpc_id
	   GROUP BY 
			mat.alu_id, qtFaltasReposicao

	/* [Carla 19/07/2013]
		Excluir os registros que não devem ser exibidos na tela. Esses registros devem ser trazidos para buscar
		a nota do bimestre anterior, caso seja recuperação.
	*/
	DELETE FROM @Matriculas
	WHERE
		registroExterno = 1
		-- Se possuir uma saída no período, não exibe o aluno.
		OR PossuiSaidaPeriodo = 1
		-- Excluir matrículas de outras turmas, pois traz todos os bimestres pra fazer os acumulados.
		OR (@tur_tipo = 1 AND tur_id <> @tur_id)
		-- Excluir matrículas de outras disciplinas em turmas eletivas/multisseriadas.
		OR (@tur_tipo IN (2,3,4) AND tud_id <> @tud_id)

	DECLARE @tbAlunos TABLE (alu_id INT);

	IF (@tdc_id = 5)
	BEGIN
		;WITH TipoDeficiencia AS 
		(
			SELECT 
				RelTde.tde_id
			FROM
				TUR_TurmaDisciplinaRelDisciplina DisRel --WITH(NOLOCK)
				INNER JOIN ACA_Disciplina dis --WITH(NOLOCK)
					ON DisRel.dis_id = dis.dis_id
					AND dis.dis_situacao <> 3
				INNER JOIN ACA_TipoDisciplina tds --WITH(NOLOCK)
					ON dis.tds_id = tds.tds_id
					AND tds.tds_situacao <> 3
				INNER JOIN ACA_TipoDisciplinaDeficiencia RelTde-- WITH(NOLOCK)
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
			@Matriculas mtd 
			INNER JOIN ACA_Aluno alu-- WITH(NOLOCK)
				ON alu.alu_id = mtd.alu_id
			INNER JOIN Synonym_PES_PessoaDeficiencia pde --WITH(NOLOCK)
				ON pde.pes_id = alu.pes_id
			INNER JOIN TipoDeficiencia tde
				ON pde.tde_id = tde.tde_id
	END
	ELSE
	BEGIN
		;WITH TipoDeficiencia AS 
		(
			SELECT 
				RelTde.tde_id
			FROM
				TUR_TurmaDisciplinaRelDisciplina DisRel --WITH(NOLOCK)
				INNER JOIN ACA_Disciplina dis --WITH(NOLOCK)
					ON DisRel.dis_id = dis.dis_id
					AND dis.dis_situacao <> 3
				INNER JOIN ACA_TipoDisciplina tds --WITH(NOLOCK)
					ON dis.tds_id = tds.tds_id
					AND tds.tds_situacao <> 3
				INNER JOIN ACA_TipoDisciplinaDeficiencia RelTde --WITH(NOLOCK)
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
			@Matriculas mtd 
			INNER JOIN ACA_Aluno alu-- WITH(NOLOCK)
				ON alu.alu_id = mtd.alu_id
			LEFT JOIN Synonym_PES_PessoaDeficiencia pde --WITH(NOLOCK)
				ON pde.pes_id = alu.pes_id
		WHERE
			(NOT EXISTS (SELECT tde_id FROM TipoDeficiencia tde WHERE tde.tde_id = pde.tde_id ))	
	END

	; WITH TabelaMovimentacao AS (

			--Selecina as movimentações que possuem matrícula anterior
			SELECT
				MOV.alu_id,
				mtu_idAnterior,
				tmv_nome    
			FROM
				MTR_Movimentacao MOV --WITH (NOLOCK) 
				INNER JOIN @tbAlunos
					ON MOV.alu_id = [@tbAlunos].alu_id
				INNER JOIN ACA_TipoMovimentacao TMV-- WITH (NOLOCK) 
					ON MOV.tmv_idSaida = TMV.tmv_id
			WHERE
				mov_situacao NOT IN (3,4)
				AND tmv_situacao <> 3
				AND mtu_idAnterior IS NOT NULL	
	), 

	TabelaFaltasAulas AS (
		SELECT Qtde.*
		FROM FN_Select_FaltasAulasBy_TurmaDisciplina(@tipoLancamento, @tpc_id, @tud_id, @fav_calculoQtdeAulasDadas, @tdc_id) Qtde
		INNER JOIN @tbAlunos
			ON Qtde.alu_id = [@tbAlunos].alu_id
	), 

	TabelaObservacaoDisciplina AS 
	(
		SELECT
			tud_id
			, alu_id
			, mtu_id
			, mtd_id
			, CASE WHEN qtdeQualidade = 0 AND qtdeDesempenhos = 0 AND qtdeRecomendacao = 0
						AND ado_qualidade IS NULL AND ado_desempenhoAprendizado IS NULL 
						AND ado_recomendacaoAluno IS NULL AND ado_recomendacaoResponsavel IS NULL
				   THEN 0
				   ELSE 1
			  END AS observacaoPreenchida
		FROM
		(
			SELECT 
				Mtr.tud_id
				, Mtr.alu_id
				, Mtr.mtu_id
				, Mtr.mtd_id
				, SUM(CASE WHEN aaq.tqa_id IS NULL THEN 0 ELSE 1 END) AS qtdeQualidade
				, SUM(CASE WHEN aad.tda_id IS NULL THEN 0 ELSE 1 END) AS qtdeDesempenhos
				, SUM(CASE WHEN aar.rar_id IS NULL THEN 0 ELSE 1 END) AS qtdeRecomendacao
				, ado_qualidade
				, ado_desempenhoAprendizado
				, ado_recomendacaoAluno
				, ado_recomendacaoResponsavel
			FROM
				@Matriculas Mtr
				INNER JOIN @tbAlunos
					ON Mtr.alu_id = [@tbAlunos].alu_id
				INNER JOIN ACA_Avaliacao ava --WITH(NOLOCK)
					ON Mtr.fav_id = ava.fav_id
					AND ava.ava_id = @ava_id
					AND ava.ava_exibeObservacaoDisciplina = 1
				LEFT JOIN CLS_AlunoAvaliacaoTurmaDisciplinaQualidade aaq --WITH(NOLOCK)
					ON  Mtr.tud_id = aaq.tud_id
					AND Mtr.alu_id = aaq.alu_id
					AND Mtr.mtu_id = aaq.mtu_id
					AND Mtr.mtd_id = aaq.mtd_id
					AND aaq.fav_id = ava.fav_id
					AND aaq.ava_id = ava.ava_id

				LEFT JOIN CLS_AlunoAvaliacaoTurmaDisciplinaDesempenho aad --WITH(NOLOCK)
					ON  Mtr.tud_id = aad.tud_id
					AND Mtr.alu_id = aad.alu_id
					AND Mtr.mtu_id = aad.mtu_id
					AND Mtr.mtd_id = aad.mtd_id
					AND aad.fav_id = ava.fav_id
					AND aad.ava_id = ava.ava_id  

				LEFT JOIN CLS_AlunoAvaliacaoTurmaDisciplinaRecomendacao aar --WITH(NOLOCK)
					ON  Mtr.tud_id = aar.tud_id
					AND Mtr.alu_id = aar.alu_id
					AND Mtr.mtu_id = aar.mtu_id
					AND Mtr.mtd_id = aar.mtd_id
					AND aar.fav_id = ava.fav_id
					AND aar.ava_id = ava.ava_id

				LEFT JOIN CLS_ALunoAvaliacaoTurmaDisciplinaObservacao ado-- WITH(NOLOCK)
					ON  Mtr.tud_id = ado.tud_id
					AND Mtr.alu_id = ado.alu_id
					AND Mtr.mtu_id = ado.mtu_id
					AND Mtr.mtd_id = ado.mtd_id
					AND ado.fav_id = ava.fav_id
					AND ado.ava_id = ava.ava_id
					AND ado.ado_situacao <> 3

			GROUP BY
				Mtr.tud_id
				, Mtr.alu_id
				, Mtr.mtu_id
				, Mtr.mtd_id
				, ado_qualidade
				, ado_desempenhoAprendizado
				, ado_recomendacaoAluno
				, ado_recomendacaoResponsavel
		) AS tabela 
	),

	TabelaObservacaoConselho AS 
	(
		SELECT
			tur_id
			, alu_id
			, mtu_id
			, CASE WHEN qtdeQualidade = 0 AND qtdeDesempenhos = 0 AND qtdeRecomendacao = 0
						AND ato_qualidade IS NULL AND ato_desempenhoAprendizado IS NULL 
						AND ato_recomendacaoAluno IS NULL AND ato_recomendacaoResponsavel IS NULL
				   THEN 0
				   ELSE 1
			  END AS observacaoPreenchida
		FROM
		(
			SELECT
				Mtr.tur_id
				, Mtr.alu_id
				, Mtr.mtu_id
				, SUM(CASE WHEN aaq.tqa_id IS NULL THEN 0 ELSE 1 END) AS qtdeQualidade
				, SUM(CASE WHEN aad.tda_id IS NULL THEN 0 ELSE 1 END) AS qtdeDesempenhos
				, SUM(CASE WHEN aar.rar_id IS NULL THEN 0 ELSE 1 END) AS qtdeRecomendacao
				, ato_qualidade
				, ato_desempenhoAprendizado
				, ato_recomendacaoAluno
				, ato_recomendacaoResponsavel
			FROM
				@Matriculas Mtr
				INNER JOIN @tbAlunos
					ON Mtr.alu_id = [@tbAlunos].alu_id
				INNER JOIN ACA_Avaliacao ava --WITH(NOLOCK)
					ON Mtr.fav_id = ava.fav_id
					AND ava.ava_id = @ava_id
					AND ava.ava_exibeObservacaoConselhoPedagogico = 1
				LEFT JOIN CLS_AlunoAvaliacaoTurmaQualidade aaq --WITH(NOLOCK)
					ON  Mtr.tur_id = aaq.tur_id
					AND Mtr.alu_id = aaq.alu_id
					AND Mtr.mtu_id = aaq.mtu_id
					AND aaq.fav_id = ava.fav_id
					AND aaq.ava_id = ava.ava_id

				LEFT JOIN CLS_AlunoAvaliacaoTurmaDesempenho aad --WITH(NOLOCK)
					ON  Mtr.tur_id = aad.tur_id
					AND Mtr.alu_id = aad.alu_id
					AND Mtr.mtu_id = aad.mtu_id
					AND aad.fav_id = ava.fav_id
					AND aad.ava_id = ava.ava_id 

				LEFT JOIN CLS_AlunoAvaliacaoTurmaRecomendacao aar-- WITH(NOLOCK)
					ON  Mtr.tur_id = aar.tur_id
					AND Mtr.alu_id = aar.alu_id
					AND Mtr.mtu_id = aar.mtu_id
					AND aar.fav_id = ava.fav_id
					AND aar.ava_id = ava.ava_id

				LEFT JOIN CLS_ALunoAvaliacaoTurmaObservacao ato --WITH(NOLOCK)
					ON  Mtr.tur_id = ato.tur_id
					AND Mtr.alu_id = ato.alu_id
					AND Mtr.mtu_id = ato.mtu_id
					AND ato.fav_id = ava.fav_id
					AND ato.ava_id = ava.ava_id
					AND ato.ato_situacao <> 3
			WHERE
				Mtr.tud_id = @tud_id
			GROUP BY
				Mtr.tur_id
				, Mtr.alu_id
				, Mtr.mtu_id
				, ato_qualidade
				, ato_desempenhoAprendizado
				, ato_recomendacaoAluno
				, ato_recomendacaoResponsavel
		) AS tabela
	),

	AulasCompensadas AS 
	(
		Select
			caa.tud_id
			,caa.alu_id
			,caa.mtu_id
			,caa.mtd_id
			,SUM(ISNULL(cpa.cpa_quantidadeAulasCompensadas, 0)) as qtdCompensadas
		From
			CLS_CompensacaoAusencia cpa --WITH(NOLOCK)
			INNER JOIN CLS_CompensacaoAusenciaAluno caa --WITH(NOLOCK)
				ON  caa.tud_id = cpa.tud_id
				AND caa.cpa_id = cpa.cpa_id
				AND caa.caa_situacao = 1
			WHERE
				cpa.tud_id = @tud_id
				AND cpa.tpc_id = @tpc_id
				AND cpa.cpa_situacao = 1 

		GROUP BY
			caa.tud_id
			,caa.alu_id
			,caa.mtu_id
			,caa.mtd_id
	),
	/*
	    12/06/2013 - Hélio C. Lima
	    Criado mais um "passo" CTE deixando as consultas as functions somente com o resultado a ser exibido
	*/
	tabResult AS (

        --	
	    SELECT	
		      Mtd.alu_id
		    , Mtd.mtu_id
		    , Mtd.mtd_id
		    , tur.tur_id
		    , tur.tur_codigo
		    , alc.alc_matricula
		    , Mtd.tud_id
		    , Atd.atd_id
		    , Atd.atd_avaliacao
		    , Mtd.mtd_resultado
		    , Atd.atd_semProfessor
		    , Atd.atd_frequencia
		    , tfa.qtAulas
		    , Atd.atd_numeroFaltas
		    , tfa.qtFaltas
		    , tfa.qtFaltasReposicao
		    , CASE WHEN @documentoOficial = 1 THEN Pes.pes_nomeOficial ELSE Pes.pes_nome END AS pes_nome
			, Pes.pes_dataNascimento
            , ISNULL(Mtr.mtd_numeroChamadaDocente, Mtd.mtd_numeroChamada) AS mtd_numeroChamada
		    , ISNULL(Mtr.mtd_situacaoDocente, Mtd.mtd_situacao) AS mtd_situacao
		    , Atd.atd_relatorio
		    , Atd.arq_idRelatorio
		    , Atd.atd_numeroAulas
            , ISNULL(Mtr.mtd_dataMatriculaDocente, Mtd.mtd_dataMatricula) AS mtd_dataMatricula
            , ISNULL(Mtr.mtd_dataSaidaDocente, Mtd.mtd_dataSaida) AS mtd_dataSaida
            , CASE WHEN (@ExibeCompensacao = 1 AND Atd.atd_id IS NULL) THEN 
					ISNULL(ac.qtdCompensadas, 0)
				ELSE
					ISNULL(Atd.atd_ausenciasCompensadas, 0)
				END AS ausenciasCompensadas
			, tod.observacaoPreenchida
            , toc.observacaoPreenchida AS observacaoConselhoPreenchida
            , 0 AS frequenciaFinal --tff.frequenciaFinal
            , Atd.atd_avaliacaoPosConselho AS avaliacaoPosConselho
			, Atd.atd_justificativaPosConselho AS justificativaPosConselho
			, (CASE WHEN (@ExibeCompensacao = 1)
				THEN 
					CASE WHEN Atd.atd_id IS NULL 
					THEN 
						-- Se não estiver fechada, faz o calculo da frequencia.
						CASE WHEN Qtd.aulas IS NOT NULL AND Qtd.aulas > 0 THEN 
							dbo.FN_Calcula_PorcentagemFrequenciaVariacao
								(Qtd.aulas, (Qtd.faltas + Qtd.faltasReposicao)-qtd.compensadas, Mtr.variacaoFrequencia)
						-- Caso o total de aulas previstas seja 0 a frequência deve ser 100.
						ELSE 100 END
					ELSE Atd.atd_frequenciaFinalAjustada
					END
				ELSE 
					CASE WHEN Atd.atd_id IS NULL 
					THEN 
						-- Se não estiver fechada, faz o calculo da frequencia.
						CASE WHEN Qtd.aulas IS NOT NULL AND Qtd.aulas > 0 THEN 
							dbo.FN_Calcula_PorcentagemFrequenciaVariacao
								(Qtd.aulas, (Qtd.faltas + Qtd.faltasReposicao), Mtr.variacaoFrequencia)
						-- Caso o total de aulas previstas seja 0 a frequência deve ser 100.
						ELSE 100 END
					ELSE Atd.atd_frequenciaFinalAjustada
					END
			  END) AS FrequenciaFinalAjustada
			, mtu.mtu_resultado
			, Mtr.variacaoFrequencia
			, Qtd.faltasAnteriores
			, Qtd.compensadasAnteriores
	    FROM @Matriculas Mtr
        INNER JOIN MTR_MatriculaTurmaDisciplina Mtd --WITH(NOLOCK)
	        ON  Mtr.alu_id = Mtd.alu_id
            AND Mtr.mtu_id = Mtd.mtu_id
            AND Mtr.mtd_id = Mtd.mtd_id
            AND Mtr.tpc_id = COALESCE(  @tpc_id, 
                                        @tpc_idProximo, 
                                        (
								            -- Se não for avaliação ligada a uma periódica, traz a matrícula no último tpc_id 
								            -- quando for avaliação Final ou Conselho de classe.
								            SELECT MAX(tpc_id)
								            FROM @Matriculas MtrMax 
								            WHERE MtrMax.alu_id = Mtr.alu_id
								            AND @tipoAvaliacao  IN (3, 4) -- 3 - Final, 4 - Conselho de Classe
							            )
							         )
		INNER JOIN @tbAlunos
			ON Mtr.alu_id = [@tbAlunos].alu_id
        INNER JOIN @SomatorioAulasFaltas AS Qtd
	        ON  Mtd.alu_id = Qtd.alu_id

	     LEFT JOIN TabelaFaltasAulas AS tfa
	        ON  Mtd.alu_id = tfa.alu_id
	        AND Mtd.mtu_id = tfa.mtu_id
	        AND Mtd.mtd_id = tfa.mtd_id

		INNER JOIN MTR_MatriculaTurma mtu --WITH(NOLOCK)
			ON mtu.alu_id = Mtd.alu_id
			AND mtu.mtu_id = Mtd.mtu_id
			AND mtu_situacao <> 3
		INNER JOIN TUR_Turma tur --WITH(NOLOCK)
			ON tur.tur_id = mtu.tur_id
			AND tur.tur_situacao <> 3
		INNER JOIN ACA_AlunoCurriculo alc --WITH(NOLOCK)
			ON alc.alu_id = mtu.alu_id
			AND alc.alc_id = mtu.alc_id
			AND alc.alc_situacao <> 3	

        INNER JOIN ACA_Aluno Alu --WITH(NOLOCK)
	        ON  Mtd.alu_id   = Alu.alu_id
	        AND alu_situacao <> 3

        INNER JOIN VW_DadosAlunoPessoa Pes --WITH(NOLOCK)
	        ON  Alu.alu_id   = Pes.alu_id

        LEFT JOIN CLS_AlunoAvaliacaoTurmaDisciplina Atd --WITH(NOLOCK)
	        ON  Atd.tud_id = @tud_id
	        AND Atd.alu_id = Mtd.alu_id
	        AND Atd.mtu_id = Mtd.mtu_id
	        AND Atd.mtd_id = Mtd.mtd_id
	        AND Atd.fav_id = @fav_id
	        AND Atd.ava_id = @ava_id
	        AND Atd.atd_situacao <> 3

	  --  LEFT JOIN TabelaFrequenciaFinal AS tff
			--ON tff.alu_id = Mtd.alu_id
			--AND tff.tud_id = Mtd.tud_id

		LEFT JOIN TabelaObservacaoDisciplina tod
			ON tod.tud_id = Mtd.tud_id
			AND tod.alu_id = Mtd.alu_id
			AND tod.mtu_id = Mtd.mtu_id
			AND tod.mtd_id = Mtd.mtd_id

		LEFT JOIN TabelaObservacaoConselho toc
			ON toc.tur_id = Mtu.tur_id
			AND toc.alu_id = Mtu.alu_id
			AND toc.mtu_id = Mtu.mtu_id

		LEFT JOIN AulasCompensadas ac 
			ON ac.tud_id = Mtd.tud_id
			AND ac.alu_id = Mtd.alu_id
			AND ac.mtu_id = Mtd.mtu_id
			AND ac.mtd_id = Mtd.mtd_id

	    WHERE 
	        ISNULL(Mtr.mtd_situacaoDocente, mtd_situacao) IN (1,5)
		    AND COALESCE(Mtr.mtd_numeroChamadaDocente, mtd_numeroChamada, 0) >= 0
		    AND Alu.alu_situacao <> 3		
		    AND (
				    -- Avaliação que não é do tipo "Recuperação" traz todos os alunos
				    (
					    @tipoAvaliacao <> 2 			
				    )
				    OR	
				    -- Avaliação com lançamento de nota por relatório sempre traz todos os alunos,
				    -- independente do tipo de Avaliação
				    (
					    @tipoEscalaDisciplina = 3
				    ) 									
				    OR
				    -- Avaliação do tipo "Recuperação" e com escala do tipo "Númerica" ou "Pareceres"
				    -- traz apenas os alunos que não atingiram o valor mínimo de aprovação
			        (																				
					    @tipoAvaliacao = 2 										
					    AND EXISTS (SELECT  1
					                FROM    @notasPeriodicasRecuperacao N 
						            WHERE   N.alu_id = Mtr.alu_id AND 
						                    N.mtu_id = Mtr.mtu_id AND 
						                    N.mtd_id = Mtr.mtd_id AND 
						                    N.recuperacao = 1
					               )
				    )
			    )

	)
	, movimentacao AS (

			--Selecina as movimentações que possuem matrícula anterior
			SELECT
				MOV.alu_id,
				mtu_idAnterior,
				CASE WHEN tmo_tipoMovimento IN (6, 12, 14, 15, 16)
					 THEN TMV.tmv_nome + ' em ' + REPLACE(CONVERT(VARCHAR(10), mov.mov_dataRealizacao, 103), '/' + CAST(DATEPART(YEAR, mov.mov_dataRealizacao) AS VARCHAR(4)), '') +
					 	  ISNULL(' p/ ' + turD.tur_codigo, '')
					 WHEN tmo_tipoMovimento IN (8)
					 THEN TMV.tmv_nome + ' p/' + ISNULL(' ' + turD.tur_codigo, '')
					 WHEN tmo_tipoMovimento IN (11)
					 THEN TMV.tmv_nome + ' p/ ' + ISNULL(' ' + turD.tur_codigo, '')
					 ELSE TMV.tmv_nome
				END tmv_nome  
			FROM
				tabResult res
				INNER JOIN MTR_Movimentacao MOV -- WITH (NOLOCK) 
					ON res.alu_id = MOV.alu_id 
				INNER JOIN ACA_TipoMovimentacao TMV -- WITH (NOLOCK) 
					ON MOV.tmv_idSaida = TMV.tmv_id
				LEFT JOIN MTR_TipoMovimentacao tmo -- WITH(NOLOCK)
					ON mov.tmo_id = tmo.tmo_id
					AND tmo.tmo_situacao <> 3
				LEFT JOIN MTR_MatriculaTurma mtuD -- WITH(NOLOCK)
					ON mov.alu_id = mtuD.alu_id
					AND mov.mtu_idAtual = mtuD.mtu_id
				LEFT JOIN TUR_Turma turD -- WITH(NOLOCK)
					ON mtuD.tur_id = turD.tur_id
				LEFT JOIN ACA_CalendarioAnual calD -- WITH(NOLOCK)
					ON turD.cal_id = calD.cal_id
				INNER JOIN MTR_MatriculaTurma mtuO -- WITH(NOLOCK)
					ON mov.alu_id = mtuO.alu_id
					AND mov.mtu_idAnterior = mtuO.mtu_id
					AND mtuO.tur_id = @tur_id
				LEFT JOIN TUR_Turma turO -- WITH(NOLOCK)
					ON mtuO.tur_id = turO.tur_id
				LEFT JOIN ACA_CalendarioAnual calO -- WITH(NOLOCK)
					ON turO.cal_id = calO.cal_id
			WHERE
				mov_situacao NOT IN (3,4)
				AND tmv_situacao <> 3
				AND mtu_idAnterior IS NOT NULL	
	)
	, tbRetorno AS
	(
		SELECT
			  alu_id
			, mtu_id
			, mtd_id
			, tud_id
			, tur_id
			, -1        AS tud_idPrincipal
			, -1        AS mtd_idPrincipal
			, tur_codigo
		    , alc_matricula
			, atd_id    AS AvaliacaoID
			, ISNULL(Res.atd_avaliacao,
				CASE WHEN (Res.atd_id IS NULL AND (@tipoEscalaDisciplina in (1,2)) AND (@tipoEscalaDocente = @tipoEscalaDisciplina))
					THEN REPLACE(CAST(
						(SELECT TOP 1 Media FROM @tbMediasAlunos Med WHERE Med.alu_id = Res.alu_id AND Med.mtu_id = Res.mtu_id)
						 AS VARCHAR(20)), '.', ',')
					ELSE NULL END) AS Avaliacao
			, CASE 
				-- Caso não seja possível alterar o resultado do aluno e a avaliação for do tipo final ou períodica + final não traz o resultado (ele é calculado na tela)
				WHEN @permiteAlterarResultado = 0 AND @tipoAvaliacao IN (3,5) THEN 
					NULL
				-- Caso contrário, traz o resultado normalmente
				ELSE 
					mtd_resultado
			END 
			AS AvaliacaoResultado		

			, ISNULL(atd_semProfessor, 0) AS atd_semProfessor

			-- Frequência
			, CASE 
				WHEN (atd_frequencia IS NULL) THEN 
					-- Se não estiver fechada, faz o calculo da frequencia.
					CASE WHEN qtAulas IS NOT NULL AND qtAulas > 0 THEN 
						dbo.FN_Calcula_PorcentagemFrequenciaVariacao
							(qtAulas, ISNULL(atd_numeroFaltas, qtFaltas), Res.variacaoFrequencia)
					-- Caso o total de aulas previstas seja 0 a frequência deve ser 100.
					ELSE 100 END
				ELSE 
					atd_frequencia
			END
			AS Frequencia

			-- Qtde. de faltas
			, CASE 
				WHEN (atd_numeroFaltas IS NULL AND atd_frequencia IS NULL) THEN 
					qtFaltas
				ELSE 
					atd_numeroFaltas
			END
			AS QtFaltasAluno
			, qtFaltasReposicao AS QtFaltasAlunoReposicao
			-- Qtde. de aulas
			, CASE 
				WHEN (atd_numeroAulas IS NULL AND atd_frequencia IS NULL) THEN 
					qtAulas 
				ELSE 
					atd_numeroAulas
			END
			AS QtAulasAluno

			, Res.pes_nome + 
			(
				CASE 
					WHEN ( Res.mtd_situacao = 5 ) THEN 
						ISNULL((SELECT TOP 1 ' (' + tmv_nome + ')' FROM movimentacao MOV--WITH(NOLOCK) 
								WHERE MOV.mtu_idAnterior = Res.mtu_id AND MOV.alu_id = Res.alu_id), ' (Inativo)')
					ELSE 
						'' 
				END
			) 
			AS pes_nome
			, Res.pes_dataNascimento

			, CASE 
				WHEN Res.mtd_numeroChamada > 0 THEN 
					CAST(Res.mtd_numeroChamada AS VARCHAR)
				ELSE 
					'-' 
			END 
			AS mtd_numeroChamada

			, Res.mtd_numeroChamada AS mtd_numeroChamadaordem

			, CAST(Res.alu_id AS VARCHAR) + ';' + 
			  CAST(Res.mtd_id AS VARCHAR) + ';' + 
			  CAST(Res.mtu_id AS VARCHAR) 
			AS id

			, CAST(ISNULL(frequenciaFinal, 0) AS DECIMAL(5,2)) AS frequenciaAcumulada

			, atd_relatorio
			, arq_idRelatorio

			-- Alc, Ala e Tca_numeroavaliacao : campos da avaliação do Curso (quando seriado por avaliação).
			-- Não implementado quando é lançamento por disciplina.
			, NULL                       AS alc_id
			, NULL                       AS ala_id
			, NULL						 AS tca_numeroAvaliacao
			, Res.mtd_situacao             AS situacaoMatriculaAluno
			, Res.mtd_dataMatricula        AS dataMatricula
			, Res.mtd_dataSaida            AS dataSaida
			, Res.ausenciasCompensadas		AS ausenciasCompensadas
			, CAST(1 AS BIT)				AS ala_avaliado

			-- Campos que não existem na avaliação por disciplina.
			, NULL AS AvaliacaoSalaRecurso
			, NULL AS AvaliacaoAdicional

			-- Traz o campo Faltoso da tabela AlunoAvaliacaoTurma.
			, ISNULL(
				(
					SELECT TOP 1
						Aat.aat_faltoso
					FROM
						CLS_AlunoAvaliacaoTurma Aat --WITH(NOLOCK)
					WHERE 
							Aat.tur_id = @tur_id
						AND Aat.alu_id = Res.alu_id
						AND Aat.mtu_id = Res.mtu_id
						AND Aat.fav_id = @fav_id
						AND Aat.ava_id = @ava_id
						AND Aat.aat_situacao = 1
				), CAST(0 AS BIT)
			) AS faltoso
			, CAST(
					ISNULL(
						(
							SELECT TOP 1
								Aat.aat_faltoso
							FROM
								CLS_AlunoAvaliacaoTurma Aat --WITH(NOLOCK)
							WHERE 
									Aat.tur_id = @tur_id
								AND Aat.alu_id = Res.alu_id
								AND Aat.mtu_id = Res.mtu_id
								AND Aat.fav_id = @fav_id
								AND Aat.ava_id = @ava_id
								AND Aat.aat_situacao = 1
						), 0) AS BIT
				) AS naoAvaliado
			-- Verifica se há dispensa de disciplina para o aluno.
			, 0 AS dispensadisciplina
			, CAST(ISNULL(observacaoPreenchida, 0) AS BIT) AS observacaoPreenchida
            , CAST(ISNULL(observacaoConselhoPreenchida, 0) AS BIT) AS observacaoConselhoPreenchida
            , avaliacaoPosConselho
			, justificativaPosConselho
			, FrequenciaFinalAjustada
			, mtu_resultado
			, atd_numeroAulas AS QtAulasEfetivado
			, faltasAnteriores
			, compensadasAnteriores
		FROM 
			tabResult AS Res
	)   

	SELECT 
		  alu_id
			, mtu_id
			, mtu_id AS mtu_idAnterior
			, mtd_id
			, mtd_id AS mtd_idAnterior
			, tud_id
			, tur_id
			, tur_codigo
		    , alc_matricula
			, tud_idPrincipal
			, mtd_idPrincipal
			, AvaliacaoID
			, Avaliacao
			, AvaliacaoResultado		
			, atd_semProfessor
			, Frequencia
			, QtFaltasAluno
			, QtAulasAluno
			, ISNULL(QtFaltasAlunoReposicao, 0) AS QtFaltasAlunoReposicao
			, pes_nome		
			, ISNULL(CAST(pes_dataNascimento AS VARCHAR(10)), '') AS pes_dataNascimento
			, mtd_numeroChamada
			, id
			, frequenciaAcumulada
			, atd_relatorio
			, arq_idRelatorio
			, alc_id
			, ala_id
			, tca_numeroAvaliacao
			, situacaoMatriculaAluno
			, dataMatricula
			, dataSaida
			, ISNULL(ausenciasCompensadas, 0) AS ausenciasCompensadas
			, ala_avaliado
			, AvaliacaoSalaRecurso
			, AvaliacaoAdicional
			, faltoso
			, naoAvaliado
			, dispensadisciplina
			, observacaoPreenchida
			, observacaoConselhoPreenchida
			, avaliacaoPosConselho
			, justificativaPosConselho
			, FrequenciaFinalAjustada
			, mtu_resultado
			, QtAulasEfetivado
			, ISNULL(faltasAnteriores, 0) AS faltasAnteriores
			, ISNULL(compensadasAnteriores, 0) AS compensadasAnteriores
	FROM	
		tbRetorno 
	GROUP BY
		 alu_id
			, mtu_id
			, mtd_id
			, tud_id
			, tur_id
			, tur_codigo
		    , alc_matricula
			, tud_idPrincipal
			, mtd_idPrincipal
			, AvaliacaoID
			, Avaliacao
			, AvaliacaoResultado		
			, atd_semProfessor
			, Frequencia
			, QtFaltasAluno
			, QtAulasAluno
			, QtFaltasAlunoReposicao
			, pes_nome
			, pes_dataNascimento		
			, mtd_numeroChamada
			, mtd_numeroChamadaordem
			, id
			, frequenciaAcumulada
			, atd_relatorio
			, arq_idRelatorio
			, alc_id
			, ala_id
			, tca_numeroAvaliacao
			, situacaoMatriculaAluno
			, dataMatricula
			, dataSaida
			, ausenciasCompensadas
			, ala_avaliado
			, AvaliacaoSalaRecurso
			, AvaliacaoAdicional
			, faltoso
			, naoAvaliado
			, dispensadisciplina
			, observacaoPreenchida
			, observacaoConselhoPreenchida
			, avaliacaoPosConselho
			, justificativaPosConselho
			, FrequenciaFinalAjustada
			, mtu_resultado
			, QtAulasEfetivado
			, ISNULL(faltasAnteriores, 0)
			, ISNULL(compensadasAnteriores, 0)
	ORDER BY 
		CASE 
		    WHEN @ordenacao = 0 THEN 
			    CASE WHEN ISNULL(mtd_numeroChamadaordem,0) <= 0 THEN 1 ELSE 0 END
		END ASC
		, CASE WHEN @ordenacao = 0 THEN ISNULL(mtd_numeroChamadaordem,0) END ASC
		, CASE WHEN @ordenacao = 1 THEN pes_nome END ASC
END
GO
PRINT N'Altering [dbo].[NEW_MTR_MatriculaTurmaDisciplina_SelectFechamentoFiltroDeficiencia_Final]'
GO
-- Stored Procedure

-- ========================================================================
-- Author:	    Marcia Haga
-- Create date: 03/08/2015
-- Description: Retorna os alunos matriculados na Turma, de acordo com as regras necessárias 
--			    para ele aparecer na listagem para efetivar da avaliacao Final.
--				Filtrando os alunos com ou sem deficiência, dependendo do docente.

---- Alterado: Marcia Haga - 10/08/2015
---- Description: Alterado para verificar o periodo em que o aluno esteve 
---- presente na turma eletiva de aluno ou multisseriada.

---- Alterado: Marcia Haga - 11/08/2015
---- Description: Alterado para priorizar os dados pre-processados, ao inves dos dados ja efetivados.
-- ========================================================================
ALTER PROCEDURE [dbo].[NEW_MTR_MatriculaTurmaDisciplina_SelectFechamentoFiltroDeficiencia_Final]
	@tud_id BIGINT
	, @tur_id BIGINT
	, @ava_id INT
	, @ordenacao INT
	, @fav_id INT
	, @tur_tipo TINYINT
	, @cal_id INT
	, @permiteAlterarResultado BIT
	, @tdc_id TINYINT
	, @dtTurma TipoTabela_Turma READONLY
	, @documentoOficial BIT
AS
BEGIN
    SET TRANSACTION ISOLATION LEVEL SNAPSHOT
	DECLARE @escolaId INT;
	SELECT TOP 1 @escolaId = esc_id
	FROM TUR_Turma -- WITH (NOLOCK)
	WHERE tur_id = @tur_id

	DECLARE @ultimoPeriodo INT;
	SELECT TOP 1 @ultimoPeriodo = tpc_id 
	FROM ACA_CalendarioPeriodo -- WITH (NOLOCK)
	WHERE 
		cal_id = @cal_id AND cap_situacao <> 3 
	ORDER BY cap_dataFim DESC

	DECLARE @MatriculasBoletimDaTurma AS TipoTabela_MatriculasBoletim;

	DECLARE @Matriculas TABLE (alu_id BIGINT NOT NULL, mtu_id INT NOT NULL, mtd_id INT NOT NULL, tur_id BIGINT, tpc_id INT, tpc_ordem INT, tud_id BIGINT, fav_id INT
		, registroExterno BIT, PossuiSaidaPeriodo BIT, esc_id INT, tds_id INT, mtd_numeroChamadaDocente INT NULL
		, mtd_situacaoDocente TINYINT NULL, mtd_dataMatriculaDocente DATE NULL, mtd_dataSaidaDocente DATE NULL
		, PRIMARY KEY (alu_id, mtu_id, mtd_id, tpc_id));

	DECLARE @MatriculaMultisseriadaTurmaAluno TABLE 
		(
			tud_idDocente BIGINT, 
			alu_id BIGINT, 
			mtu_id INT, 
			mtd_id INT
			PRIMARY KEY (tud_idDocente, alu_id, mtu_id, mtd_id)
		);

	DECLARE @tds_id INT = 
		(
			 --Busca o tipo de disciplina para filtrar os mtds abaixo.
			SELECT Dis.tds_id
			FROM TUR_TurmaDisciplinaRelDisciplina RelDis -- WITH (NOLOCK)
			INNER JOIN ACA_Disciplina Dis -- WITH (NOLOCK)
				ON Dis.dis_id = RelDis.dis_id
			WHERE
				RelDis.tud_id = @tud_id
		)

	--Se for turma de eletiva do aluno, carrega os alunos que devem aparecer na tela de efetivação
	IF ( @tur_tipo IN (2,3) ) BEGIN
		-- Turma eletiva de aluno ou multisseriada, buscar matrículas por aluno.
		DECLARE @tbMatriculaAlunos TipoTabela_AlunoMatriculaTurma;
		INSERT INTO @tbMatriculaAlunos (alu_id, mtu_id)
		SELECT Mtd.alu_id, MAX(Mtd.mtu_id)
		FROM MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
		WHERE
			-- Busca mtus dos alunos pelo MTD - o mtd está ligado ao tud informado, e o mtu_id aponta pra outra turma (turma normal).
			Mtd.tud_id = @tud_id
			AND Mtd.mtd_situacao <> 3
		GROUP BY alu_id	

		INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
			PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
			PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
		Select mb.alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mb.mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes 
		  from MTR_MatriculasBoletim mb  -- WITH (NOLOCK)
			   inner join @tbMatriculaAlunos amt on amt.alu_id = mb.alu_id and amt.mtu_id = mb.mtu_origemDados

		IF not exists (select top 1 alu_id from @MatriculasBoletimDaTurma)
		begin
			INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
				PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
				PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
			EXEC NEW_MTR_MatriculaTurma_MatriculasBoletim_Alunos
				@tbMatriculaTurma = @tbMatriculaAlunos
		end
	END
	ELSE IF (@tur_tipo = 4)
	BEGIN
		-- Turma eletiva de aluno ou multisseriada, buscar matrículas por aluno.
		DECLARE @tbMatriculaAlunosMultisseriada TipoTabela_AlunoMatriculaTurma;
		INSERT INTO @tbMatriculaAlunosMultisseriada (alu_id, mtu_id)
		SELECT Mtd.alu_id, MAX(Mtd.mtu_id)
		FROM MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
		INNER JOIN MTR_MatriculaTurma mtu
			ON Mtd.alu_id = mtu.alu_id
			AND Mtd.mtu_id = mtu.mtu_id
			AND mtu.mtu_situacao <> 3
		INNER JOIN @dtTurma dtt
			ON mtu.tur_id = dtt.tur_id
		WHERE
			-- Busca mtus dos alunos pelo MTD - o mtd está ligado ao tud informado, e o mtu_id aponta pra outra turma (turma normal).
			Mtd.tud_id = @tud_id
			AND Mtd.mtd_situacao <> 3
		GROUP BY mtd.alu_id	

		INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
		Select mb.alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mb.mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes 
		  from MTR_MatriculasBoletim mb  -- WITH (NOLOCK)
			   inner join @tbMatriculaAlunosMultisseriada amt on amt.alu_id = mb.alu_id and amt.mtu_id = mb.mtu_origemDados

		IF not exists (select top 1 alu_id from @MatriculasBoletimDaTurma)
		begin
			INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
				PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
				PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
			EXEC NEW_MTR_MatriculaTurma_MatriculasBoletim_Alunos
				@tbMatriculaTurma = @tbMatriculaAlunosMultisseriada
		end

		INSERT INTO @MatriculaMultisseriadaTurmaAluno (tud_idDocente, alu_id, mtu_id, mtd_id)
		SELECT 
			mtdDocente.tud_id AS tud_idDocente,
			mtdAluno.alu_id,
			mtdAluno.mtu_id,
			mtdAluno.mtd_id
		FROM @MatriculasBoletimDaTurma mtr
		INNER JOIN MTR_MatriculaTurmaDisciplina mtdDocente
			ON mtr.alu_id = mtdDocente.alu_id
			AND mtr.mtu_id = mtdDocente.mtu_id
			AND mtdDocente.tud_id = @tud_id
			AND mtdDocente.mtd_situacao <> 3
		INNER JOIN MTR_MatriculaTurmaDisciplina mtdAluno
			ON mtdAluno.alu_id = mtr.alu_id
			AND mtdAluno.mtu_id = mtr.mtu_id
			AND mtdAluno.mtd_situacao <> 3
		INNER JOIN TUR_TurmaDisciplina tudAluno
			ON mtdAluno.tud_id = tudAluno.tud_id
			AND tudAluno.tud_id <> @tud_id
			AND tudAluno.tud_situacao <> 3
		INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDisAluno
			ON RelDisAluno.tud_id = tudAluno.tud_id
		INNER JOIN ACA_Disciplina disAluno
			ON RelDisAluno.dis_id = disAluno.dis_id
			AND disAluno.tds_id = @tds_id
			AND disAluno.dis_situacao <> 3
		GROUP BY
			mtdDocente.tud_id,
			mtdAluno.alu_id,
			mtdAluno.mtu_id,
			mtdAluno.mtd_id
	END
	 --Se for turma normal, carrega os alunos de acordo com o boletim
	ELSE
	BEGIN
		INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
			PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
			PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
		select mb.alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mb.mtu_id,mb.tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mb.mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes
		  from MTR_MatriculasBoletim mb -- WITH (NOLOCK)
			   inner join (select alu_id, mtu_id, ROW_NUMBER() OVER(PARTITION BY alu_id 
														   ORDER BY mtu_id desc) as linha
							 from MTR_MatriculaTurma -- WITH (NOLOCK) 
							where mtu_situacao <> 3
							  and tur_id = @tur_id) mtu 
					   on mtu.alu_id = mb.alu_id and mtu.mtu_id = mb.mtu_origemDados
		 where mtu.linha = 1 --para evitar casos onde o aluno tem dois mtus para o mesmo tur_id

		IF not exists (select top 1 alu_id from @MatriculasBoletimDaTurma)
		begin
		INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
			PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
			PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
		EXEC NEW_MTR_MatriculaTurma_MatriculasBoletim_DaTurma
			@tur_id = @tur_id;
		end
	END	

	IF (@tur_tipo = 4)
	BEGIN
		INSERT INTO @Matriculas (alu_id, mtu_id, mtd_id, fav_id, tpc_id, tpc_ordem, tud_id, tur_id, registroExterno, PossuiSaidaPeriodo, esc_id, tds_id,
								 mtd_numeroChamadaDocente, mtd_situacaoDocente, mtd_dataMatriculaDocente, mtd_dataSaidaDocente)
		SELECT
			Mtr.alu_id, Mtr.mtu_id, Mtd.mtd_id, Mtr.fav_id, Mtr.tpc_id, Mtr.tpc_ordem, Mtd.tud_id, Mtr.tur_id
			, Mtr.registroExterno, Mtr.PossuiSaidaPeriodo, Mtr.esc_id, Dis.tds_id 
			, mtdDocente.mtd_numeroChamada
			, mtdDocente.mtd_situacao
			, mtdDocente.mtd_dataMatricula
			, mtdDocente.mtd_dataSaida
		FROM @MatriculasBoletimDaTurma Mtr
		INNER JOIN MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
			ON Mtd.alu_id = Mtr.alu_id
			AND Mtd.mtu_id = Mtr.mtu_id
			AND Mtd.mtd_situacao <> 3
		INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDis -- WITH (NOLOCK)
			ON RelDis.tud_id = Mtd.tud_id
		INNER JOIN ACA_Disciplina Dis -- WITH (NOLOCK)
			ON RelDis.dis_id = Dis.dis_id	
		INNER JOIN @MatriculaMultisseriadaTurmaAluno tdm 
			ON Mtd.alu_id = tdm.alu_id
			AND Mtd.mtu_id = tdm.mtu_id
			AND Mtd.mtd_id = tdm.mtd_id
		INNER JOIN MTR_MatriculaTurmaDisciplina mtdDocente
			ON mtdDocente.alu_id = Mtd.alu_id
			AND mtdDocente.mtu_id = Mtd.mtu_id
			AND mtdDocente.tud_id = tdm.tud_idDocente
			AND mtdDocente.mtd_situacao <> 3
		WHERE
			Mtr.mtu_id IS NOT NULL
			 --Busca a matrícula na mtd ligada à disciplina (pelo tds_id), em cada COC, da lista de mtus (MatriculasBoletim).
			AND Dis.tds_id = @tds_id
			 --Filtros de matrícula.
			AND Mtr.MesmoCalendario = 1
			--AND Mtr.PeriodosEquivalentes = 1
	END
	ELSE
	BEGIN
		INSERT INTO @Matriculas (alu_id, mtu_id, mtd_id, fav_id, tpc_id, tpc_ordem, tud_id, tur_id, registroExterno, PossuiSaidaPeriodo, esc_id, tds_id)
		SELECT
			Mtr.alu_id, Mtr.mtu_id, Mtd.mtd_id, tur.fav_id, Mtr.tpc_id, Mtr.tpc_ordem, Mtd.tud_id, Mtr.tur_id
			, Mtr.registroExterno, Mtr.PossuiSaidaPeriodo, Mtr.esc_id, Dis.tds_id
		FROM @MatriculasBoletimDaTurma Mtr
		INNER JOIN MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
			ON Mtd.alu_id = Mtr.alu_id
			AND Mtd.mtu_id = Mtr.mtu_id
			AND Mtd.mtd_situacao <> 3
		INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDis -- WITH (NOLOCK)
			ON RelDis.tud_id = Mtd.tud_id
		INNER JOIN TUR_TurmaRelTurmaDisciplina RelTur 
			ON RelTur.tud_id = Mtd.tud_id
		INNER JOIN TUR_Turma tur
			ON tur.tur_id = RelTur.tur_id
			AND tur.tur_situacao <> 3
		INNER JOIN ACA_Disciplina Dis -- WITH (NOLOCK)
			ON RelDis.dis_id = Dis.dis_id	
		WHERE
			Mtr.mtu_id IS NOT NULL
			 --Busca a matrícula na mtd ligada à disciplina (pelo tds_id), em cada COC, da lista de mtus (MatriculasBoletim).
			AND Dis.tds_id = @tds_id
			 --Filtros de matrícula.
			AND Mtr.MesmoCalendario = 1
			--Verifica períodos equivalentes apenas para turmas normais (1)
			AND (Mtr.PeriodosEquivalentes = 1 OR @tur_tipo <> 1)
	END

	-- Verifica o periodo em que o aluno esteve presente na turma eletiva de aluno ou multisseriada
	IF ( @tur_tipo IN (2,3,4) ) 
	BEGIN
		;WITH PresencaAlunoPeriodo AS
		(
			SELECT Mat.alu_id, Mat.mtu_id, Mat.mtd_id, Mat.tpc_id 
			FROM @Matriculas Mat
			INNER JOIN TUR_Turma Tur -- WITH (NOLOCK)
				ON Tur.tur_id = Mat.tur_id
			INNER JOIN MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
				ON Mtd.alu_id = Mat.alu_id
				AND Mtd.mtu_id = Mat.mtu_id
				AND Mtd.mtd_id = Mat.mtd_id
			INNER JOIN ACA_TipoPeriodoCalendario Tpc -- WITH (NOLOCK)
				ON Tpc.tpc_id = Mat.tpc_id
			INNER JOIN ACA_CalendarioPeriodo Cap -- WITH (NOLOCK)
				ON Cap.tpc_id = Tpc.tpc_id
				AND Cap.cal_id = Tur.cal_id
				AND Cap.cap_situacao <> 3
			WHERE
			(
				-- O aluno nao estava presente no periodo se:
				-- o aluno saiu durante o periodo
				Mtd.mtd_dataSaida BETWEEN Cap.cap_dataInicio AND Cap.cap_dataFim
				-- ou o aluno saiu antes de o periodo iniciar
				OR Mtd.mtd_dataSaida < Cap.cap_dataInicio
				-- ou o aluno entrou depois do fim do periodo
				OR Mtd.mtd_dataMatricula > Cap.cap_dataFim
			)
			AND Mat.PossuiSaidaPeriodo = 0
		)
		UPDATE @Matriculas
		SET PossuiSaidaPeriodo = 1
		FROM @Matriculas Mat
		INNER JOIN PresencaAlunoPeriodo Pap
			ON Pap.alu_id = Mat.alu_id
			AND Pap.mtu_id = Mat.mtu_id
			AND Pap.mtd_id = Mat.mtd_id
			AND Pap.tpc_id = Mat.tpc_id
	END

	-- Notas e frequencia que ja foram fechadas
	DECLARE @Fechado TABLE (alu_id BIGINT NOT NULL, mtu_id INT NOT NULL, mtd_id INT NOT NULL
							, atd_id INT NOT NULL, fav_id INT NOT NULL, ava_id INT NOT NULL
							, atd_avaliacao VARCHAR(20), atd_frequencia DECIMAL(5,2)
							, atd_relatorio VARCHAR(MAX), arq_idRelatorio BIGINT
							, atd_avaliacaoPosConselho VARCHAR(20), atd_frequenciaFinalAjustada DECIMAL(5,2)
							, tpc_id INT, atd_numeroAulas INT, atd_numeroAulasReposicao INT
							, atd_numeroFaltas INT, atd_numeroFaltasReposicao INT
							, atd_ausenciasCompensadas INT
			, PRIMARY KEY (alu_id, mtu_id, mtd_id, atd_id));	
	INSERT INTO @Fechado
	SELECT 
		atd.alu_id
		, atd.mtu_id
		, atd.mtd_id
		, atd_id
		, atd.fav_id
		, atd.ava_id
		, atd_avaliacao
		, atd_frequencia
		, atd_relatorio
		, arq_idRelatorio
		, atd_avaliacaoPosConselho
		, atd_frequenciaFinalAjustada
		, ava.tpc_id
		, atd.atd_numeroAulas
		, atd.atd_numeroAulasReposicao
		, atd.atd_numeroFaltas
		, atd.atd_numeroFaltasReposicao
		, atd.atd_ausenciasCompensadas
	FROM @Matriculas m
		INNER JOIN CLS_AlunoAvaliacaoTurmaDisciplina atd -- WITH (NOLOCK) 
			ON atd.tud_id = m.tud_id
			AND atd.alu_id = m.alu_id
			AND atd.mtu_id = m.mtu_id
			AND atd.mtd_id = m.mtd_id
			AND atd.fav_id = m.fav_id
		INNER JOIN ACA_Avaliacao ava -- WITH (NOLOCK) 
			ON ava.fav_id = atd.fav_id 
			AND ava.ava_id = atd.ava_id
			AND (ava.tpc_id = m.tpc_id OR ava.tpc_id IS NULL)
			AND ava.ava_situacao <> 3
	WHERE
		atd.tud_id = @tud_id
		AND (ava.ava_tipo IN (1, 5) -- periodica, periodica + final
			OR (ava.ava_tipo = 3 AND ava.ava_id = @ava_id)) --  final
		AND ava.fav_id = @fav_id
		AND atd_situacao <> 3
	------------------------------------------------------------
	-- Fechado em outras turmas
	UNION	
	SELECT 
		m.alu_id
		, m.mtu_id
		, m.mtd_id
		, atd_id
		, atd.fav_id
		, atd.ava_id
		, atd_avaliacao
		, atd_frequencia
		, atd_relatorio
		, arq_idRelatorio
		, atd_avaliacaoPosConselho
		, atd_frequenciaFinalAjustada
		, m.tpc_id
		, atd.atd_numeroAulas
		, atd.atd_numeroAulasReposicao
		, atd.atd_numeroFaltas
		, atd.atd_numeroFaltasReposicao
		, atd.atd_ausenciasCompensadas
	FROM @Matriculas m
		INNER JOIN CLS_AlunoAvaliacaoTurmaDisciplina atd -- WITH (NOLOCK) 
			ON atd.tud_id = m.tud_id
			AND atd.alu_id = m.alu_id
			AND atd.mtu_id = m.mtu_id
			AND atd.mtd_id = m.mtd_id
			AND atd.fav_id = m.fav_id
		INNER JOIN ACA_Avaliacao ava -- WITH (NOLOCK) 
			ON ava.fav_id = atd.fav_id 
			AND ava.ava_id = atd.ava_id
			AND ava.tpc_id = m.tpc_id
			AND ava.ava_situacao <> 3
	WHERE
		(m.tur_id <> @tur_id
			OR m.tud_id <> @tud_id)
		AND ava.ava_tipo IN (1, 5) -- periodica, periodica + final
		AND atd_situacao <> 3
	------------------------------------------------------------	

	/* [Carla 19/07/2013]
		Excluir os registros que não devem ser exibidos na tela. Esses registros devem ser trazidos para buscar
		a nota do bimestre anterior, caso seja recuperação.
	*/
	DELETE FROM @Matriculas
	WHERE
		registroExterno = 1
		-- Se possuir uma saída no período, não exibe o aluno.
		OR PossuiSaidaPeriodo = 1

	/**/
	DECLARE @tbAlunos TABLE (alu_id INT);	
	IF (@tdc_id = 5)
	BEGIN
		;WITH TipoDeficiencia AS 
		(
			SELECT 
				RelTde.tde_id
			FROM
				TUR_TurmaDisciplinaRelDisciplina DisRel --WITH(NOLOCK)
				INNER JOIN ACA_Disciplina dis --WITH(NOLOCK)
					ON DisRel.dis_id = dis.dis_id
					AND dis.dis_situacao <> 3
				INNER JOIN ACA_TipoDisciplina tds --WITH(NOLOCK)
					ON dis.tds_id = tds.tds_id
					AND tds.tds_situacao <> 3
				INNER JOIN ACA_TipoDisciplinaDeficiencia RelTde-- WITH(NOLOCK)
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
			@Matriculas mtd 
			INNER JOIN ACA_Aluno alu-- WITH(NOLOCK)
				ON alu.alu_id = mtd.alu_id
			INNER JOIN Synonym_PES_PessoaDeficiencia pde --WITH(NOLOCK)
				ON pde.pes_id = alu.pes_id
			INNER JOIN TipoDeficiencia tde
				ON pde.tde_id = tde.tde_id
	END
	ELSE
	BEGIN
		;WITH TipoDeficiencia AS 
		(
			SELECT 
				RelTde.tde_id
			FROM
				TUR_TurmaDisciplinaRelDisciplina DisRel --WITH(NOLOCK)
				INNER JOIN ACA_Disciplina dis --WITH(NOLOCK)
					ON DisRel.dis_id = dis.dis_id
					AND dis.dis_situacao <> 3
				INNER JOIN ACA_TipoDisciplina tds --WITH(NOLOCK)
					ON dis.tds_id = tds.tds_id
					AND tds.tds_situacao <> 3
				INNER JOIN ACA_TipoDisciplinaDeficiencia RelTde --WITH(NOLOCK)
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
			@Matriculas mtd 
			INNER JOIN ACA_Aluno alu-- WITH(NOLOCK)
				ON alu.alu_id = mtd.alu_id
			LEFT JOIN Synonym_PES_PessoaDeficiencia pde --WITH(NOLOCK)
				ON pde.pes_id = alu.pes_id
		WHERE
			(NOT EXISTS (SELECT tde_id FROM TipoDeficiencia tde WHERE tde.tde_id = pde.tde_id ))	
	END
	/**/

	; WITH TabelaMovimentacao AS (
			--Selecina as movimentações que possuem matrícula anterior
			SELECT
				MOV.alu_id,
				mtu_idAnterior,
				CASE WHEN tmo_tipoMovimento IN (6, 12, 14, 15, 16)
					 THEN TMV.tmv_nome + ' em ' + REPLACE(CONVERT(VARCHAR(10), mov.mov_dataRealizacao, 103), '/' + CAST(DATEPART(YEAR, mov.mov_dataRealizacao) AS VARCHAR(4)), '') +
					 	  ISNULL(' p/ ' + turD.tur_codigo, '')
					 WHEN tmo_tipoMovimento IN (8)
					 THEN TMV.tmv_nome + ' p/' + ISNULL(' ' + turD.tur_codigo, '')
					 WHEN tmo_tipoMovimento IN (11)
					 THEN TMV.tmv_nome + ' p/ ' + ISNULL(' ' + turD.tur_codigo, '')
					 ELSE TMV.tmv_nome
				END tmv_nome  
			FROM
				@tbAlunos tAlu
				/**/
				INNER JOIN MTR_Movimentacao MOV -- WITH (NOLOCK) 
					ON tAlu.alu_id = MOV.alu_id 
				/**/
				INNER JOIN ACA_TipoMovimentacao TMV -- WITH (NOLOCK) 
					ON MOV.tmv_idSaida = TMV.tmv_id
				LEFT JOIN MTR_TipoMovimentacao tmo -- WITH(NOLOCK)
					ON mov.tmo_id = tmo.tmo_id
					AND tmo.tmo_situacao <> 3
				LEFT JOIN MTR_MatriculaTurma mtuD -- WITH(NOLOCK)
					ON mov.alu_id = mtuD.alu_id
					AND mov.mtu_idAtual = mtuD.mtu_id
				LEFT JOIN TUR_Turma turD -- WITH(NOLOCK)
					ON mtuD.tur_id = turD.tur_id
				LEFT JOIN ACA_CalendarioAnual calD -- WITH(NOLOCK)
					ON turD.cal_id = calD.cal_id
				INNER JOIN MTR_MatriculaTurma mtuO -- WITH(NOLOCK)
					ON mov.alu_id = mtuO.alu_id
					AND mov.mtu_idAnterior = mtuO.mtu_id
					AND mtuO.tur_id = @tur_id
				LEFT JOIN TUR_Turma turO -- WITH(NOLOCK)
					ON mtuO.tur_id = turO.tur_id
				LEFT JOIN ACA_CalendarioAnual calO -- WITH(NOLOCK)
					ON turO.cal_id = calO.cal_id
			WHERE
				mov_situacao NOT IN (3,4)
				AND tmv_situacao <> 3
				AND mtu_idAnterior IS NOT NULL	
	)
	, avaliacoes AS (
		SELECT 
			ava.tpc_id
			, ava.ava_nome
			, cap.cap_dataInicio AS cap_dataInicio
			, cap.cap_dataFim AS cap_dataFim
			, ava.ava_id
		FROM ACA_Avaliacao ava -- WITH (NOLOCK)
		LEFT JOIN ACA_CalendarioPeriodo cap -- WITH (NOLOCK) 
			ON cap.tpc_id = ava.tpc_id
			AND cap.cal_id = @cal_id
			AND cap.cap_situacao <> 3
		WHERE
			(ava.ava_tipo IN (1, 5) -- periodica, periodica + final
				OR (ava.ava_tipo = 3 AND ava.ava_id = @ava_id)) --  final
			AND ava.fav_id = @fav_id
			AND ava_situacao <> 3
	)
	, TabelaObservacaoConselho AS 
	(
		SELECT
			tur_id
			, alu_id
			, mtu_id
			, CASE WHEN qtdeQualidade = 0 AND qtdeDesempenhos = 0 AND qtdeRecomendacao = 0
						AND ato_qualidade IS NULL AND ato_desempenhoAprendizado IS NULL 
						AND ato_recomendacaoAluno IS NULL AND ato_recomendacaoResponsavel IS NULL
				   -- nenhum campo preenchido
				   THEN 0
				   ELSE
					(CASE WHEN ato_desempenhoAprendizado IS NOT NULL 
							AND ato_recomendacaoAluno IS NOT NULL 
							AND ato_recomendacaoResponsavel IS NOT NULL
					-- todos os campos preenchidos
					THEN 1
					-- algum campo preenchido
					ELSE 2
					END)
			  END AS observacaoPreenchida
		FROM
		(
			SELECT
				Mtr.tur_id
				, Mtr.alu_id
				, Mtr.mtu_id
				, SUM(CASE WHEN aaq.tqa_id IS NULL THEN 0 ELSE 1 END) AS qtdeQualidade
 				, SUM(CASE WHEN aad.tda_id IS NULL THEN 0 ELSE 1 END) AS qtdeDesempenhos
 				, SUM(CASE WHEN aar.rar_id IS NULL THEN 0 ELSE 1 END) AS qtdeRecomendacao
				, ato_qualidade
				, ato_desempenhoAprendizado
				, ato_recomendacaoAluno
				, ato_recomendacaoResponsavel
			FROM
				@Matriculas Mtr
				/**/
				INNER JOIN @tbAlunos tAlu
					ON tAlu.alu_id = Mtr.alu_id
				/**/
				INNER JOIN ACA_Avaliacao ava -- WITH (NOLOCK)
					ON Mtr.fav_id = ava.fav_id
					AND ava.tpc_id = @ultimoPeriodo
					AND ava.ava_exibeObservacaoConselhoPedagogico = 1
				LEFT JOIN CLS_AlunoAvaliacaoTurmaQualidade aaq -- WITH (NOLOCK)
					ON  Mtr.tur_id = aaq.tur_id
					AND Mtr.alu_id = aaq.alu_id
					AND Mtr.mtu_id = aaq.mtu_id
					AND aaq.fav_id = ava.fav_id
					AND aaq.ava_id = ava.ava_id
				LEFT JOIN CLS_AlunoAvaliacaoTurmaDesempenho aad -- WITH (NOLOCK)
					ON  Mtr.tur_id = aad.tur_id
					AND Mtr.alu_id = aad.alu_id
					AND Mtr.mtu_id = aad.mtu_id
					AND aad.fav_id = ava.fav_id
					AND aad.ava_id = ava.ava_id 
				LEFT JOIN CLS_AlunoAvaliacaoTurmaRecomendacao aar -- WITH (NOLOCK)
					ON  Mtr.tur_id = aar.tur_id
					AND Mtr.alu_id = aar.alu_id
					AND Mtr.mtu_id = aar.mtu_id
					AND aar.fav_id = ava.fav_id
					AND aar.ava_id = ava.ava_id	        
				LEFT JOIN CLS_ALunoAvaliacaoTurmaObservacao ato -- WITH (NOLOCK)
					ON Mtr.tur_id = ato.tur_id
					AND Mtr.alu_id = ato.alu_id
					AND Mtr.mtu_id = ato.mtu_id
					AND ato.fav_id = ava.fav_id
					AND ato.ava_id = ava.ava_id
					AND ato.ato_situacao <> 3	
			WHERE
				Mtr.tud_id = @tud_id	
			GROUP BY
				Mtr.tur_id
				, Mtr.alu_id
				, Mtr.mtu_id
				, ato_qualidade
				, ato_desempenhoAprendizado
				, ato_recomendacaoAluno
				, ato_recomendacaoResponsavel
		) 
		AS tabela
		GROUP BY --(Adicionado group by por Webber) 
			tabela.tur_id
			, tabela.alu_id 
			, tabela.mtu_id 
			, CASE WHEN qtdeQualidade = 0 AND qtdeDesempenhos = 0 AND qtdeRecomendacao = 0
							AND ato_qualidade IS NULL AND ato_desempenhoAprendizado IS NULL 
							AND ato_recomendacaoAluno IS NULL AND ato_recomendacaoResponsavel IS NULL
					   -- nenhum campo preenchido
					   THEN 0
					   ELSE
						(CASE WHEN ato_desempenhoAprendizado IS NOT NULL 
								AND ato_recomendacaoAluno IS NOT NULL 
								AND ato_recomendacaoResponsavel IS NOT NULL
						-- todos os campos preenchidos
						THEN 1
						-- algum campo preenchido
						ELSE 2
						END)
				  END	
	)	
	, tbRetorno AS (	
		SELECT
			  Mtd.alu_id
			, Mtd.mtu_id
			, Mtd.mtd_id
			, alc.alc_matricula
			, F.atd_id AS AvaliacaoID
			, COALESCE(F.atd_avaliacaoPosConselho, Caf.caf_avaliacao, F.atd_avaliacao, '') as Avaliacao 
			, CASE WHEN @permiteAlterarResultado = 0 
					THEN NULL
					-- Caso contrário, traz o resultado normalmente
					ELSE Mtd.mtd_resultado 
				END AS AvaliacaoResultado	
			, COALESCE(Caf.caf_frequencia, F.atd_frequencia, 0) AS Frequencia
			, CASE WHEN @documentoOficial = 1 THEN Pes.pes_nomeOficial ELSE Pes.pes_nome END + 
				(
					CASE WHEN ( ISNULL(Mtr.mtd_situacaoDocente, Mtd.mtd_situacao) = 5 ) 
						THEN ISNULL((SELECT TOP 1 ' (' + tmv_nome + ')' FROM TabelaMovimentacao MOV -- WITH (NOLOCK)
									 WHERE MOV.mtu_idAnterior = Mtd.mtu_id AND MOV.alu_id = Mtd.alu_id), ' (Inativo)')
						ELSE '' 
					END
				) 
				AS pes_nome
			, CASE WHEN ISNULL(Mtr.mtd_numeroChamadaDocente, Mtd.mtd_numeroChamada) > 0 
					THEN CAST(ISNULL(Mtr.mtd_numeroChamadaDocente, Mtd.mtd_numeroChamada) AS VARCHAR)
					ELSE '-' 
				END AS mtd_numeroChamada
			, ISNULL(Mtr.mtd_numeroChamadaDocente, Mtd.mtd_numeroChamada) AS mtd_numeroChamadaordem
			, ISNULL(Mtr.mtd_situacaoDocente, Mtd.mtd_situacao) AS situacaoMatriculaAluno
			, F.atd_relatorio
			, F.arq_idRelatorio
			, ISNULL(Mtr.mtd_dataMatriculaDocente, Mtd.mtd_dataMatricula) AS dataMatricula
			, ISNULL(Mtr.mtd_dataSaidaDocente, Mtd.mtd_dataSaida) AS dataSaida
			, COALESCE(Caf.caf_frequenciaFinalAjustada, F.atd_frequenciaFinalAjustada, 100) AS FrequenciaFinalAjustada
			, ava.tpc_id
			, ava.ava_nome AS NomeAvaliacao
			, F.atd_avaliacaoPosConselho AS AvaliacaoPosConselho
			, ava.cap_dataInicio
			, CAST(ISNULL(toc.observacaoPreenchida, 0) AS TINYINT) AS observacaoConselhoPreenchida
			-- se o aluno nao teve a nota efetivada no periodo,
			-- mas ele estava presente no periodo
			-- deve-se informar o usuario.
			, CAST(CASE WHEN 
					(
					(
						/*  
						(
							COALESCE(F.atd_avaliacaoPosConselho, F.atd_avaliacao, '') <> ''
							OR
							(								
								-- se for o ultimo periodo,
								-- e nao tiver fechamento
								-- deve ter a nota do Listao
								ISNULL(ava.tpc_id, 0) = @ultimoPeriodo
								AND F.atd_id IS NULL
								AND ISNULL(Caf.caf_avaliacao,'') <> ''
							)
						)
						*/
						COALESCE(F.atd_avaliacaoPosConselho, Caf.caf_avaliacao, F.atd_avaliacao, '') <> ''
						AND (ISNULL(F.atd_id, 0) > 0 OR ISNULL(ava.tpc_id, 0) = @ultimoPeriodo)
						AND ISNULL(tud.tud_naoLancarNota, 0) = 0
					)
					OR 
					(
						ISNULL(tud.tud_naoLancarNota, 0) = 1
						AND ISNULL(F.atd_id,0) > 0
					))
					AND
					(
						ISNULL(pend.DisciplinaSemAula, 0) = 0 AND
						ISNULL(pend.SemNota, 0) = 0
					)
					THEN 1
					ELSE 0
			   END AS BIT) AS PossuiNota
			, ava.ava_id AS ava_id
			, CASE WHEN (ava.tpc_id IS NOT NULL AND ava.tpc_id = @ultimoPeriodo) 
					THEN 1 
					ELSE 0 
				END AS UltimoPeriodo
			, COALESCE(Caf.caf_qtAulas, F.atd_numeroAulas, 0) as QtAulasAluno		
			, COALESCE(Caf.caf_qtAulasReposicao, F.atd_numeroAulasReposicao, 0) AS QtAulasAlunoReposicao
			, COALESCE(Caf.caf_qtFaltas, F.atd_numeroFaltas, 0) as QtFaltasAluno
			, COALESCE(Caf.caf_qtFaltasReposicao, F.atd_numeroFaltasReposicao, 0) AS QtFaltasAlunoReposicao
			, COALESCE(Caf.caf_qtAusenciasCompensadas, F.atd_ausenciasCompensadas, 0) AS ausenciasCompensadas
			, mtu.mtu_resultado
			, CASE WHEN (
					-- aluno estava presente na rede no periodo da avaliacao
					EXISTS (
						SELECT alu_id
						FROM @Matriculas
						WHERE alu_id = Mtr.alu_id
						AND tpc_id = ava.tpc_id
					)
				) THEN
					(
						CASE WHEN (
							-- aluno estava presente na escola no periodo da avaliacao
							EXISTS (
								SELECT alu_id
								FROM @Matriculas
								WHERE alu_id = Mtr.alu_id
								AND esc_id = @escolaId
								AND tpc_id = ava.tpc_id
							)
						) 
						THEN 1 -- Aluno presente na escola
						ELSE 2 -- Aluno em outra escola		
						END
					)						
				ELSE 0 -- Aluno fora da rede
				END AS PresencaAluno
			, F.atd_numeroAulas AS QtAulasEfetivado
			, ISNULL(tpc.tpc_ordem, 0) AS tpc_ordem
		FROM @Matriculas Mtr
		/**/
		INNER JOIN @tbAlunos tAlu
			ON tAlu.alu_id = Mtr.alu_id
		/**/
		INNER JOIN MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
			ON  Mtd.alu_id = Mtr.alu_id
			AND Mtd.mtu_id = Mtr.mtu_id
			AND Mtd.mtd_id = Mtr.mtd_id
			AND Mtd.tud_id = @tud_id
		INNER JOIN TUR_TurmaDisciplina tud -- WITH (NOLOCK)
			ON Mtd.tud_id = tud.tud_id
			AND tud.tud_situacao <> 3
		INNER JOIN MTR_MatriculaTurma mtu -- WITH (NOLOCK)
			ON mtu.alu_id = Mtd.alu_id
			AND mtu.mtu_id = Mtd.mtu_id
			AND mtu_situacao <> 3    
		INNER JOIN ACA_AlunoCurriculo alc -- WITH (NOLOCK)
			ON alc.alu_id = mtu.alu_id
			AND alc.alc_id = mtu.alc_id
			AND alc.alc_situacao <> 3	
		INNER JOIN ACA_Aluno Alu -- WITH (NOLOCK)
			ON  Mtd.alu_id   = Alu.alu_id
			AND alu_situacao <> 3
		INNER JOIN VW_DadosAlunoPessoa Pes -- WITH (NOLOCK)
	        ON  Alu.alu_id   = Pes.alu_id
		INNER JOIN avaliacoes ava 
			ON 1 = 1    
		LEFT JOIN REL_AlunosSituacaoFechamento pend -- WITH(NOLOCK)
			ON tud.tud_id = pend.tud_id
			AND ava.tpc_id = pend.tpc_id
			AND Mtr.alu_id = pend.alu_id
		LEFT JOIN ACA_TipoPeriodoCalendario tpc -- WITH (NOLOCK)
			ON ava.tpc_id = tpc.tpc_id
			AND tpc.tpc_situacao <> 3    
		LEFT JOIN @Fechado F 
			ON (F.alu_id = Mtd.alu_id 
					AND F.mtu_id = Mtd.mtu_id 
					AND F.mtd_id = Mtd.mtd_id
					AND ((F.tpc_id IS NULL AND ava.tpc_id IS NULL) OR F.tpc_id = ava.tpc_id)
				)
				------------------------------------------------------------
				-- Fechado em outras turmas
				OR (F.alu_id = Mtd.alu_id 
					AND F.mtu_id <> Mtd.mtu_id
					AND F.tpc_id = ava.tpc_id
				)	
				------------------------------------------------------------	  		
		LEFT JOIN CLS_AlunoFechamento Caf -- WITH (NOLOCK)
			ON  Caf.tud_id = Mtd.tud_id
			AND Caf.tpc_id = ava.tpc_id
			AND Caf.alu_id = Mtd.alu_id
			AND Caf.mtu_id = Mtd.mtu_id
			AND Caf.mtd_id = Mtd.mtd_id
			AND ava.tpc_id IS NOT NULL 
			AND Caf.tpc_id = ava.tpc_id
			--AND Caf.tpc_id = @ultimoPeriodo		
		LEFT JOIN TabelaObservacaoConselho toc
			ON 
			toc.alu_id = Mtu.alu_id
			AND toc.mtu_id = Mtu.mtu_id		        
		WHERE 
			Mtr.tpc_id = @ultimoPeriodo
			AND ISNULL(Mtr.mtd_situacaoDocente, mtd_situacao) IN (1,5)
			AND COALESCE(Mtr.mtd_numeroChamadaDocente, mtd_numeroChamada, 0) >= 0		
	)
	, tbRetornoUltimoPeriodo AS
	(
		SELECT alu_id, mtu_id, mtd_id, FrequenciaFinalAjustada
		FROM tbRetorno
		WHERE UltimoPeriodo = 1
	)	
	, tbFinal AS 
	(
		SELECT 
			@tur_id AS tur_id
			, @tud_id AS tud_id
			, r.alu_id
			, r.mtu_id
			, r.mtd_id
			, alc_matricula
			, AvaliacaoID
			, Avaliacao
			, AvaliacaoResultado		
			, Frequencia
			, pes_nome		
			, mtd_numeroChamada
			, atd_relatorio
			, arq_idRelatorio
			, situacaoMatriculaAluno
			, dataMatricula
			, dataSaida
			-- Se for a avaliação final, pego a frequencia final ajustada do ultimo periodo
			, CASE WHEN (tpc_id IS NULL)
				THEN Up.FrequenciaFinalAjustada
				ELSE r.FrequenciaFinalAjustada
				END AS FrequenciaFinalAjustada
			, ISNULL(tpc_id, -1) AS tpc_id
			, NomeAvaliacao
			, AvaliacaoPosConselho
			, observacaoConselhoPreenchida
			-- Valida o fechamento apenas se o aluno estava 
			-- presente na escola no periodo da avaliação
			, CASE WHEN PresencaAluno = 1 THEN PossuiNota ELSE 1 END AS PossuiNota
			, ava_id
			, UltimoPeriodo
			, QtAulasAluno	
			, QtAulasAlunoReposicao		
			, QtFaltasAluno		
			, QtFaltasAlunoReposicao
			, ausenciasCompensadas
			, mtu_resultado
			, CASE WHEN PresencaAluno = 0 AND ISNULL(tpc_id, -1) > 0 THEN 1 ELSE 0 END AS AlunoForaDaRede
			, QtAulasEfetivado
			, cap_dataInicio
			, mtd_numeroChamadaordem
			, tpc_ordem
		FROM tbRetorno r
		LEFT JOIN tbRetornoUltimoPeriodo Up 
			ON Up.alu_id = r.alu_id 
			AND Up.mtu_id = r.mtu_id 
			AND Up.mtd_id = r.mtd_id	
		GROUP BY
			cap_dataInicio
			, tpc_id
			, NomeAvaliacao
			, r.alu_id
			, r.mtu_id
			, r.mtd_id
			, alc_matricula
			, AvaliacaoID
			, Avaliacao
			, AvaliacaoResultado		
			, Frequencia
			, pes_nome		
			, mtd_numeroChamada
			, mtd_numeroChamadaordem
			, atd_relatorio
			, arq_idRelatorio
			, situacaoMatriculaAluno
			, dataMatricula
			, dataSaida
			, r.FrequenciaFinalAjustada
			, Up.FrequenciaFinalAjustada
			, AvaliacaoPosConselho
			, observacaoConselhoPreenchida
			, ava_id
			, UltimoPeriodo
			, QtAulasAluno	
			, QtAulasAlunoReposicao		
			, QtFaltasAluno		
			, QtFaltasAlunoReposicao
			, ausenciasCompensadas
			, mtu_resultado
			, PossuiNota
			, PresencaAluno
			, QtAulasEfetivado
			, tpc_ordem
	)	
	SELECT
		tur_id
		, tud_id
		, alu_id
		, mtu_id
		, mtd_id
		, alc_matricula
		, AvaliacaoID
		, Avaliacao
		, AvaliacaoResultado		
		, Frequencia
		, pes_nome		
		, mtd_numeroChamada
		, atd_relatorio
		, arq_idRelatorio
		, situacaoMatriculaAluno
		, dataMatricula
		, dataSaida
		, FrequenciaFinalAjustada
		, tpc_id
		, NomeAvaliacao
		, AvaliacaoPosConselho
		, observacaoConselhoPreenchida
		, CASE WHEN AlunoForaDaRede = 1 OR PossuiNota = 1 THEN 0 ELSE 1 END AS SemNota
		, ava_id
		, UltimoPeriodo
		, QtAulasAluno	
		, QtAulasAlunoReposicao		
		, QtFaltasAluno		
		, QtFaltasAlunoReposicao
		, ausenciasCompensadas
		, mtu_resultado
		, AlunoForaDaRede
		, QtAulasEfetivado
		, tpc_ordem
	FROM
		tbFinal
	ORDER BY 
		cap_dataInicio
		, tpc_id
		, ava_id
		, CASE 
			WHEN @ordenacao = 0 THEN 
				CASE WHEN ISNULL(mtd_numeroChamadaordem,0) <= 0 THEN 1 ELSE 0 END
		END ASC
		, CASE WHEN @ordenacao = 0 THEN ISNULL(mtd_numeroChamadaordem,0) END ASC
		, CASE WHEN @ordenacao = 1 THEN pes_nome END ASC	
END

GO
PRINT N'Creating [dbo].[NEW_TUR_Turma_SelectByFiltrosPlanejamentoSemanal]'
GO
-- =============================================
-- Author:		Myller Batista
-- Create date: 29/03/2017
-- Description:	Traz as turmas e disciplinas da escola
-- =============================================
CREATE PROCEDURE [dbo].[NEW_TUR_Turma_SelectByFiltrosPlanejamentoSemanal]  
	@esc_id INT,
	@uni_id INT,
	@cal_id INT,
	@cur_id INT, 
	@crr_id INT, 
	@crp_id INT, 
	@tur_codigo VARCHAR(30),
	@ent_id UNIQUEIDENTIFIER,
	@tci_id INT,
	@tud_tipo TINYINT
AS
BEGIN
	DECLARE @Turmas TABLE
	(
		tur_id BIGINT,
		fav_id INT,
		cal_id INT,
		cal_ano INT,
		cal_descricao VARCHAR(200),
		tur_codigo VARCHAR(30),
		esc_id INT,
		uni_id INT,
		tur_dataEncerramento DATETIME,
		tur_escolaUnidade VARCHAR(200),
		trn_id INT,
		tur_tipo TINYINT,
		fav_tipoLancamentoFrequencia TINYINT,
		fav_calculoQtdeAulasDadas TINYINT,
		trn_descricao VARCHAR(200),
		PRIMARY KEY (tur_id)
	)

	INSERT INTO @Turmas
	(
		tur_id,
		fav_id,
		cal_id,
		cal_ano,
		cal_descricao,
		tur_codigo,
		esc_id,
		uni_id,
		tur_dataEncerramento,
		tur_escolaUnidade,
		trn_id,
		tur_tipo,
		fav_tipoLancamentoFrequencia,
		fav_calculoQtdeAulasDadas,
		trn_descricao
	)
	SELECT	
		tur.tur_id,
		tur.fav_id,
		tur.cal_id,
		cal.cal_ano,
		cal.cal_descricao,
		tur.tur_codigo,
		tur.esc_id,
		tur.uni_id,
		tur.tur_dataEncerramento,
		Esc.esc_nome AS tur_escolaUnidade,
		tur.trn_id,
		tur.tur_tipo,
		Fav.fav_tipoLancamentoFrequencia,
		Fav.fav_calculoQtdeAulasDadas,
		Trn.trn_descricao
	FROM
		TUR_Turma tur WITH(NOLOCK)
		INNER JOIN ESC_Escola esc WITH (NOLOCK)
			ON	esc.esc_id = tur.esc_id
			AND esc.ent_id = @ent_id
			AND esc.esc_situacao <> 3
		INNER JOIN ACA_CalendarioAnual Cal WITH (NOLOCK)
			ON	Cal.cal_id = Tur.cal_id
			AND Cal.cal_id = ISNULL(@cal_id, Cal.cal_id)
			AND Cal.cal_situacao <> 3	
		INNER JOIN ACA_Turno Trn WITH (NOLOCK)
			ON	Trn.trn_id = Tur.trn_id	
			AND Trn.trn_situacao <> 3
		INNER JOIN ACA_FormatoAvaliacao Fav WITH (NOLOCK)
			ON Fav.fav_id = Tur.fav_id
			AND Fav.fav_situacao <> 3
	WHERE
		tur.esc_id = @esc_id 
		AND tur.uni_id = @uni_id
		AND tur.tur_situacao IN (1,5,7) --turmas ativas, encerradas ou extintas
	
	DECLARE @turmasAnoAtual BIT =
	(
		CASE WHEN EXISTS
			(
				SELECT TOP 1 1
				FROM ACA_CalendarioAnual Cal WITH(NOLOCK)
					INNER JOIN TUR_Turma Tur WITH(NOLOCK)
					ON Tur.cal_id = Cal.cal_id
					AND Tur.tur_situacao = 1 -- Somente as ativas
				WHERE
					Cal.cal_ano = YEAR(GETDATE())
					AND Cal.cal_situacao <> 3
			)
			THEN 1
			ELSE 0
		END
	) 

	;WITH TurmaCurriculo AS
	(
		SELECT Tur.tur_id
			   , Tcr.cur_id
			   , Tcr.crr_id
			   , Crp.crp_id
			   , Cur.cur_nome
			   , Crp.crp_descricao
			   , Tci.tci_id
			   , Tci.tci_nome
		FROM @Turmas Tur
		INNER JOIN TUR_TurmaCurriculo Tcr WITH(NOLOCK)
			ON Tcr.tur_id = Tur.tur_id
			AND Tcr.cur_id = ISNULL(@cur_id, Tcr.cur_id)
			AND Tcr.crr_id = ISNULL(@crr_id, Tcr.crr_id)
			AND Tcr.crp_id = ISNULL(@crp_id, Tcr.crp_id)
			AND Tcr.tcr_situacao <> 3
		INNER JOIN ACA_Curso Cur WITH(NOLOCK)
			ON Cur.cur_id = Tcr.cur_id
			AND Cur.cur_situacao <> 3
		INNER JOIN ACA_CurriculoPeriodo Crp WITH(NOLOCK)
			ON Crp.cur_id = Tcr.cur_id
			AND Crp.crr_id = Tcr.crr_id
			AND Crp.crp_id = Tcr.crp_id
			AND ISNULL(Crp.tci_id, 0) = COALESCE(@tci_id, Crp.tci_id, 0) -- filtra o ciclo
			AND Crp.crp_situacao <> 3
		LEFT JOIN ACA_TipoCiclo Tci WITH(NOLOCK)
			ON Tci.tci_id = Crp.tci_id
			AND Tci.tci_situacao <> 3	
		GROUP BY Tur.tur_id, Tcr.cur_id, Tcr.crr_id, Crp.crp_id, Cur.cur_nome, Crp.crp_descricao, Tci.tci_id, Tci.tci_nome
	)

	, Dados AS
	(
		SELECT 
			Tur.tur_id
			, Tur.fav_id
			, Tur.cal_id
			, Tur.tur_codigo
			, Tur.tur_escolaUnidade
			, CAST(Tur.cal_ano AS VARCHAR(10)) + ' - ' + Tur.cal_descricao AS tur_calendario
			, Tur.trn_descricao AS tur_turno			
			, Tur.esc_id
			, Tur.uni_id
			, Tur.fav_tipoLancamentoFrequencia
			, Tur.fav_calculoQtdeAulasDadas
			, Tud.tud_id				
			, CASE WHEN tud.tud_disciplinaEspecial = 1
				THEN ISNULL(tds_nomeDisciplinaEspecial, tud.tud_nome)
				ELSE tud.tud_nome
				END AS tud_nome			
			, ISNULL(Tdt.tdt_posicao, 1) AS tdt_posicao
			, ISNULL(Tdc.tdc_id, 0) AS tdc_id			
			, ISNULL(tud.tud_naoLancarNota,0) AS tud_naoLancarNota
			, ISNULL(tud.tud_naoLancarFrequencia,0) AS tud_naoLancarFrequencia
			, ISNULL(tud.tud_disciplinaEspecial,0) AS tud_disciplinaEspecial
			, Tud.tud_tipo
			, Tur.tur_dataEncerramento
			, tds.tds_id
			, Tur.cal_ano
			, Tur.tur_tipo
			, NULL as tud_idAluno
			, Tur.tur_id tur_idNormal
			, TUR.tur_codigo as tur_codigoNormal
			, Tdt.tdt_id
			, Tdt.tdt_vigenciaInicio
			, Tdt.tdt_vigenciaFim
			, ISNULL(Crg.crg_tipo, 1) AS crg_tipo
		FROM 
			@Turmas TUR  
		INNER JOIN TurmaCurriculo tcr 
			ON tcr.tur_id = TUR.tur_id
		INNER JOIN TUR_TurmaRelTurmaDisciplina AS RelTud WITH(NOLOCK)
			ON RelTud.tur_id = Tur.tur_id
		INNER JOIN TUR_TurmaDisciplina tud WITH(NOLOCK)
			ON tud.tud_id = RelTud.tud_id
			AND tud.tud_situacao <> 3
			AND tud.tud_tipo = @tud_tipo
			AND
			(
				EXISTS 
				(
					SELECT
						tdc.tpc_id
					FROM
						TUR_TurmaDisciplinaCalendario tdc WITH(NOLOCK)
					WHERE
						tdc.tud_id = tud.tud_id
				)
				OR tud.tud_tipo NOT IN (3, 4, 7, 9, 10)
			)
		INNER JOIN TUR_TurmaDisciplinaRelDisciplina DisTud WITH(NOLOCK)
			ON tud.tud_id = DisTud.tud_id
		INNER JOIN ACA_Disciplina dis WITH(NOLOCK)
			ON dis.dis_id = DisTud.dis_id
			AND dis.dis_situacao <> 3
		INNER JOIN ACA_TipoDisciplina tds WITH(NOLOCK)
			ON tds.tds_id = dis.tds_id
			AND tds.tds_situacao <> 3		
		LEFT JOIN ACA_TipoDocente tdc WITH(NOLOCK)
			ON tdc.tdc_id = 1
			AND tdc.tdc_situacao <> 3
		LEFT JOIN TUR_TurmaDocente Tdt WITH(NOLOCK)
			ON tdt.tud_id = RelTud.tud_id	
			AND tdc.tdc_posicao = Tdt.tdt_posicao		
			AND Tdt.tdt_situacao = 1
		LEFT JOIN dbo.RHU_Cargo Crg WITH(NOLOCK)
			ON Crg.crg_id = Tdt.crg_id	
		WHERE 
			TUR.tur_codigo LIKE '%'+ISNULL(@tur_codigo, TUR.tur_codigo)+'%'
			AND (TUR.tur_tipo <> 4 
				OR EXISTS(
					SELECT 1
					FROM
						MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
						INNER JOIN TUR_TurmaDisciplina TurDisc WITH(NOLOCK)
							ON TurDisc.tud_id = Mtd.tud_id
							AND TurDisc.tud_situacao <> 3
							AND TurDisc.tud_tipo = @tud_tipo
						INNER JOIN TUR_TurmaRelTurmaDisciplina AS TurRelTud WITH(NOLOCK)
							ON TurDisc.tud_id = TurRelTud.tud_id	
						INNER JOIN TurmaCurriculo tcr 
							ON tcr.tur_id = TurRelTud.tur_id
					WHERE
						Mtd.tud_id = tud.tud_id	
						AND Mtd.mtd_situacao = 1

				)  
			)
	)

	SELECT 
		Tur.tur_id
		, Tur.fav_id
		, Tur.cal_id
		, Tur.tur_codigo
		, Tur.tur_escolaUnidade
		, Tur.tur_calendario
		, Tur.tur_turno			
		, Tur.esc_id
		, Tur.uni_id
		, Tur.fav_tipoLancamentoFrequencia
		, Tur.fav_calculoQtdeAulasDadas
		, Tur.tud_id				
		, Tur.tud_nome			
		, Tur.tdt_posicao
		, Tur.tdc_id			
		, Tur.tud_naoLancarNota
		, Tur.tud_naoLancarFrequencia
		, Tur.tud_disciplinaEspecial
		, Tur.tud_tipo
		, Tur.tur_dataEncerramento
		, Tur.tds_id
		, Tur.cal_ano
		, tur_tipo
		, STUFF(
			(
				SELECT '<br/>' + Tcr.cur_nome + ISNULL(' - ' + Tcr.crp_descricao, '') 
				FROM TurmaCurriculo Tcr
				WHERE Tcr.tur_id = tur.tur_idNormal
				FOR XML PATH(''), TYPE
				).value('.','varchar(max)'), 1, 5, '') 
			AS tur_curso
		, ISNULL(STUFF(
			(
				SELECT ',' + CAST(Tcr.tci_id AS VARCHAR)
				FROM TurmaCurriculo Tcr
				WHERE Tcr.tur_id = tur.tur_id
				AND Tcr.tci_id IS NOT NULL
				GROUP BY Tcr.tci_id
				FOR XML PATH(''), TYPE
			).value('.','varchar(max)'), 1, 1, '')
		, '') AS tciIds
		, @turmasAnoAtual AS turmasAnoAtual
		, tud_idAluno
		, tur_idNormal
		, tur_codigoNormal
		, tdt_id
		, tdt_vigenciaInicio
		, tdt_vigenciaFim
		, crg_tipo
	FROM Dados Tur
	GROUP BY 
		cal_ano
		, cal_id
		, esc_id
		, uni_id
		, Tur.tur_id
		, tur_codigo
		, tur_escolaUnidade
		, tur_calendario
		, tur_turno
		, tur_dataEncerramento
		, fav_id
		, fav_tipoLancamentoFrequencia
		, fav_calculoQtdeAulasDadas
		, tud_id
		, tud_nome
		, tud_naoLancarNota
		, tud_naoLancarFrequencia
		, tud_disciplinaEspecial
		, tud_tipo
		, tds_id
		, tdt_posicao
		, tdc_id
		, tur_tipo
		, tud_idAluno
		, tur_idNormal
		, tur_codigoNormal
		, tdt_id
		, tdt_vigenciaInicio
		, tdt_vigenciaFim
		, crg_tipo
END
GO
PRINT N'Creating [dbo].[REL_DivergenciaAulasPrevistas]'
GO
CREATE TABLE [dbo].[REL_DivergenciaAulasPrevistas]
(
[tud_id] [bigint] NOT NULL,
[tpc_id] [int] NOT NULL,
[AulasPrevistas] [int] NOT NULL,
[AulasDadas] [int] NOT NULL,
[DataProcessamento] [datetime] NOT NULL
)
GO
PRINT N'Creating primary key [PK_REL_DivergenciaAulasPrevistas] on [dbo].[REL_DivergenciaAulasPrevistas]'
GO
ALTER TABLE [dbo].[REL_DivergenciaAulasPrevistas] ADD CONSTRAINT [PK_REL_DivergenciaAulasPrevistas] PRIMARY KEY CLUSTERED  ([tud_id], [tpc_id])
GO
PRINT N'Creating [dbo].[NEW_Relatorio_0005_DivergenciaAulasPrevistas]'
GO
-- =============================================
-- Author:		Marcia Haga
-- Create date: 29/03/2017
-- Description:	Retorna os dados para o relatório de divergência
-- entre aulas criadas e aulas previstas.
-- =============================================
CREATE PROCEDURE [dbo].[NEW_Relatorio_0005_DivergenciaAulasPrevistas]
	@uad_idSuperiorGestao UNIQUEIDENTIFIER
	, @esc_id INT
	, @cal_id INT
	, @tpc_id INT
	, @gru_id UNIQUEIDENTIFIER
	, @usu_id UNIQUEIDENTIFIER
	, @adm BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @tabelaUas TABLE(uad_id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY (uad_id))

	IF @adm <> 1
		INSERT INTO @tabelaUas (uad_id)
		SELECT uad_id FROM Synonym_FN_Select_UAs_By_PermissaoUsuario(@usu_id, @gru_id)
	ELSE
	    INSERT INTO @tabelaUas (uad_id)
		SELECT uad_id FROM ESC_Escola WITH (NOLOCK)
	    WHERE esc_situacao = 1 AND uad_idSuperiorGestao = @uad_idSuperiorGestao

    DECLARE @tabelaUes TABLE(esc_id INT NOT NULL PRIMARY KEY (esc_id))

    IF @esc_id IS NULL
        INSERT INTO @tabelaUes (esc_id)
        SELECT esc_id FROM ESC_Escola esc WITH(NOLOCK)
		INNER JOIN @tabelaUas uad
			ON esc.uad_id = uad.uad_id
	    WHERE esc_situacao = 1
    ELSE
		INSERT INTO @tabelaUes (esc_id) VALUES (@esc_id)

	DECLARE @dataAtual DATE = CAST(GETDATE() AS DATE)

	DECLARE @tev_idEfetivacaoNotas INT = 
	(
		SELECT TOP 1 CAST(pac.pac_valor AS INT)
		FROM ACA_ParametroAcademico pac WITH(NOLOCK)
		WHERE pac.pac_situacao <> 3 AND pac.pac_chave = 'TIPO_EVENTO_EFETIVACAO_NOTAS'
	);

    SELECT 
		esc.esc_id
		, esc.esc_nome
		, tur.tur_id
		, tur.tur_codigo
		, tud.tud_id
		, tud.tud_nome
		, div.AulasPrevistas
		, div.AulasDadas
		, uadSuperior.uad_nome
	FROM @tabelaUes ues
	INNER JOIN TUR_Turma tur WITH(NOLOCK)
		ON tur.esc_id = ues.esc_id
		AND tur.cal_id = @cal_id 
		AND tur.tur_situacao <> 3
	INNER JOIN ACA_CalendarioPeriodo cap WITH(NOLOCK)
		ON cap.cal_id = tur.cal_id
		AND cap.tpc_id = @tpc_id
		-- Não exibir os bimestre(s) que a turma não esteve ativa
		AND (tur.tur_dataEncerramento IS NULL OR tur.tur_dataEncerramento >= cap.cap_dataFim)
		AND cap.cap_situacao <> 3
	INNER JOIN TUR_TurmaRelTurmaDisciplina relTud WITH(NOLOCK)
		ON relTud.tur_id = tur.tur_id
	INNER JOIN REL_DivergenciaAulasPrevistas div WITH(NOLOCK)
		ON div.tud_id = relTud.tud_id
		AND div.tpc_id = cap.tpc_id
	INNER JOIN TUR_TurmaDisciplina tud WITH(NOLOCK)
		ON tud.tud_id = div.tud_id
		AND tud.tud_situacao <> 3
	INNER JOIN ESC_Escola esc WITH(NOLOCK)
		ON esc.esc_id = tur.esc_id
		AND esc.esc_situacao <> 3
	INNER JOIN Synonym_SYS_UnidadeAdministrativa uadEsc WITH(NOLOCK)
		ON uadEsc.uad_id = esc.uad_id
		AND uadEsc.uad_situacao <> 3
    INNER JOIN Synonym_SYS_UnidadeAdministrativa uadSuperior WITH(NOLOCK)
        ON uadSuperior.ent_id = esc.ent_id
        AND uadSuperior.uad_id = ISNULL(esc.uad_idSuperiorGestao, uadEsc.uad_idSuperior)
        AND uadSuperior.uad_situacao <> 3
	INNER JOIN TUR_TurmaCurriculo tcr WITH(NOLOCK)
		ON tcr.tur_id = tur.tur_id
		AND tcr.tcr_situacao <> 3
	INNER JOIN ACA_Curso cur WITH(NOLOCK)
		ON cur.cur_id = tcr.cur_id
		AND cur.cur_situacao <> 3
	INNER JOIN ACA_CurriculoPeriodo crp WITH(NOLOCK)
		ON crp.cur_id = tcr.cur_id
		AND crp.crr_id = tcr.crr_id
		AND crp.crp_id = tcr.crp_id
		AND crp.crp_situacao <> 3
	INNER JOIN ACA_TipoNivelEnsino tne WITH(NOLOCK)
		ON tne.tne_id = cur.tne_id
		AND tne.tne_situacao <> 3
	WHERE
	-- Traz somente os períodos a partir de quando a turma iniciou (períodos que possuem alunos matriculados)
	EXISTS (
		SELECT TOP 1 1
		FROM MTR_MatriculaTurmaDisciplina mtd WITH(NOLOCK)
		WHERE 
		mtd.tud_id = div.tud_id	
		AND mtd.mtd_situacao <> 3
		AND mtd.mtd_dataMatricula <= cap.cap_dataFim
		AND (mtd.mtd_dataSaida IS NULL OR mtd.mtd_dataSaida >= cap.cap_dataInicio)
	)
	AND
	(
		-- Só exibir a divergência se for o bimestre atual
		(
			@dataAtual >= cap.cap_dataInicio
			AND @dataAtual <= cap.cap_dataFim
		)
		OR
		-- ou se é um bimestre que já foi fechado.
		EXISTS
		(
			SELECT TOP 1 1 
			FROM ACA_Evento evt WITH(NOLOCK)
			INNER JOIN ACA_CalendarioEvento cae WITH(NOLOCK)
				ON cae.evt_id = evt.evt_id
				AND cae.cal_id = tur.cal_id
			WHERE evt.tev_id = @tev_idEfetivacaoNotas
			AND evt.tpc_id = div.tpc_id
			AND evt.evt_dataInicio <= @dataAtual
			AND 
			(
				evt.evt_padrao = 1
				OR evt.esc_id = tur.esc_id
			)
			AND evt.evt_situacao <> 3
		)
	)
	GROUP BY
		esc.esc_id
		, esc.esc_nome
		, tur.tur_id
		, tur.tur_codigo
		, tud.tud_id
		, tud.tud_nome
		, div.AulasPrevistas
		, div.AulasDadas
		, uadSuperior.uad_nome
	ORDER BY
	esc.esc_nome, MIN(tne.tne_ordem), tur.tur_codigo, tud.tud_nome
END
GO
PRINT N'Creating [dbo].[NEW_ACA_TipoDisciplina_SelectBy_ObjetosAprendizagem]'
GO
-- ================================================================================
-- Author:		Leonardo Brito
-- Create date: 27/03/2017
-- Description:	Retorna os tipos de disciplina que não foram excluídos logicamente com ligação em objetos de aprendizagem
-- ================================================================================
CREATE PROCEDURE [dbo].[NEW_ACA_TipoDisciplina_SelectBy_ObjetosAprendizagem]	
	@tds_idNaoConsiderar INT
	, @controlarOrdem BIT
	, @cal_ano INT
	, @esc_id INT
	, @uad_idSuperior UNIQUEIDENTIFIER
AS
BEGIN
	IF (ISNULL(@esc_id, 0) > 0)
	BEGIN
		SELECT	  
			tds.tds_id
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
 			dbo.ACA_TipoCiclo tci WITH(NOLOCK)
		INNER JOIN ACA_CurriculoPeriodo crp WITH(NOLOCK)
			ON tci.tci_id = crp.tci_id
			AND crp.crp_situacao <> 3
		INNER JOIN ACA_TipoCurriculoPeriodo tcrp WITH(NOLOCK)
			ON crp.tcp_id = tcrp.tcp_id
			AND tcrp.tcp_situacao <> 3
		INNER JOIN ACA_CurriculoEscola ces WITH(NOLOCK)
			ON ces.cur_id = crp.cur_id
			AND ces.crr_id = crp.crr_id
			AND ces.esc_id = @esc_id
			AND ces.ces_situacao <> 3
		INNER JOIN ACA_ObjetoAprendizagemTipoCiclo oat WITH(NOLOCK)
			ON tci.tci_id = oat.tci_id
		INNER JOIN ACA_ObjetoAprendizagem oap WITH(NOLOCK)
			ON oat.oap_id = oap.oap_id
			AND oap.cal_ano = @cal_ano
			AND oap.oap_situacao <> 3
		INNER JOIN ACA_TipoDisciplina tds WITH (NOLOCK)
			ON tds.tds_id = oap.tds_id
			AND tds.tds_situacao <> 3
			AND tds.tds_tipo NOT IN (3) --Regência
			AND (@tds_idNaoConsiderar IS NULL OR tds.tds_id <> @tds_idNaoConsiderar) 
		INNER JOIN ACA_TipoNivelEnsino tne WITH (NOLOCK)
			ON tne.tne_id = tds.tne_id
			AND tne.tne_situacao <> 3
		WHERE 
			tci_situacao = 1
		GROUP BY
			tds.tds_id
			, tds_nome
			, tds_ordem
			, tne_nome
			, tds_situacao
			, tds_base
		ORDER BY 		
			 CASE WHEN @controlarOrdem = 1 THEN tds_ordem END
			,CASE WHEN @controlarOrdem = 0 THEN tne_nome + ' - ' + tds_nome END 
	END
	ELSE IF (@uad_idSuperior IS NOT NULL)
	BEGIN
		SELECT	
			tds.tds_id
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
 			dbo.ACA_TipoCiclo tci WITH(NOLOCK)
		INNER JOIN ACA_CurriculoPeriodo crp WITH(NOLOCK)
			ON tci.tci_id = crp.tci_id
			AND crp.crp_situacao <> 3
		INNER JOIN ACA_TipoCurriculoPeriodo tcrp WITH(NOLOCK)
			ON crp.tcp_id = tcrp.tcp_id
			AND tcrp.tcp_situacao <> 3
		INNER JOIN ACA_CurriculoEscola ces WITH(NOLOCK)
			ON ces.cur_id = crp.cur_id
			AND ces.crr_id = crp.crr_id
			AND ces.ces_situacao <> 3
		INNER JOIN ESC_Escola esc WITH(NOLOCK)
			ON ces.esc_id = esc.esc_id
			AND esc.esc_situacao <> 3
		INNER JOIN Synonym_SYS_UnidadeAdministrativa uad WITH(NOLOCK)
			ON esc.uad_id = uad.uad_id 
			AND uad.uad_situacao <> 3
		INNER JOIN ACA_ObjetoAprendizagemTipoCiclo oat WITH(NOLOCK)
			ON tci.tci_id = oat.tci_id
		INNER JOIN ACA_ObjetoAprendizagem oap WITH(NOLOCK)
			ON oat.oap_id = oap.oap_id
			AND oap.cal_ano = @cal_ano
			AND oap.oap_situacao <> 3
		INNER JOIN ACA_TipoDisciplina tds WITH (NOLOCK)
			ON tds.tds_id = oap.tds_id
			AND tds.tds_situacao <> 3
			AND tds.tds_tipo NOT IN (3) --Regência
			AND (@tds_idNaoConsiderar IS NULL OR tds.tds_id <> @tds_idNaoConsiderar) 
		INNER JOIN ACA_TipoNivelEnsino tne WITH (NOLOCK)
			ON tne.tne_id = tds.tne_id
			AND tne.tne_situacao <> 3
		WHERE 
			tci_situacao = 1
			AND COALESCE(esc.uad_idSuperiorGestao, uad.uad_idSuperior, @uad_idSuperior) = @uad_idSuperior
		GROUP BY
			tds.tds_id
			, tds_nome
			, tds_ordem
			, tne_nome
			, tds_situacao
			, tds_base
		ORDER BY 		
			 CASE WHEN @controlarOrdem = 1 THEN tds_ordem END
			,CASE WHEN @controlarOrdem = 0 THEN tne_nome + ' - ' + tds_nome END 
	END
	ELSE
	BEGIN
		SELECT 
			tds.tds_id
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
			ACA_TipoDisciplina tds WITH (NOLOCK)
		INNER JOIN ACA_TipoNivelEnsino tne WITH (NOLOCK)
			ON tne.tne_id = tds.tne_id
			AND tne.tne_situacao <> 3
		INNER JOIN ACA_ObjetoAprendizagem oap WITH(NOLOCK)
			ON tds.tds_id = oap.tds_id
			AND oap.cal_ano = @cal_ano
			AND oap.oap_situacao <> 3
		WHERE
			tds.tds_situacao <> 3
			AND tds.tds_tipo NOT IN (3) --Regência
			AND (@tds_idNaoConsiderar IS NULL OR tds.tds_id <> @tds_idNaoConsiderar) 
		GROUP BY
			tds.tds_id
			, tds_nome
			, tds_ordem
			, tne_nome
			, tds_situacao
			, tds_base
		ORDER BY 		
			 CASE WHEN @controlarOrdem = 1 THEN tds_ordem END
			,CASE WHEN @controlarOrdem = 0 THEN tne_nome + ' - ' + tds_nome END 
	END

	SELECT @@ROWCOUNT			
END

GO
PRINT N'Altering [dbo].[NEW_MTR_MatriculaTurmaDisciplina_SelectAlunosCOCDisciplina]'
GO
-- ==========================================================
-- Author:		Juliano Real
-- Create date: 27/12/2013
-- Description:	Retorna todos os alunos com alguma matricula (ativa ou inativa) no periodo por disciplina (Utilizado na tela de controle de turmas na lista de alunos)

-- Alterado: Webber V. dos Santos Data: 24/04/2014 
-- Description: Incluído no Select o campo pes_nome_abreviado

-- Alterado: Juliano Real
-- Data: 07/07/2016
-- Description: Alterado para filtrar as matriculas antes das consultas auxiliares

-- Alterado: Juliano Real
-- Data: 16/09/2016
-- Description: Alterado para não filtrar a turma em displinas normais

---- Alterado: Marcia Haga - 11/01/2017
---- Description: Adicionado parâmetros de início e fim do período,
---- para funcionar com o período de recesso.
-- ===========================================================
ALTER PROCEDURE [dbo].[NEW_MTR_MatriculaTurmaDisciplina_SelectAlunosCOCDisciplina]
	  @tud_id BIGINT
	, @tpc_id INT
	, @dtTurmas TipoTabela_Turma READONLY
	, @documentoOficial BIT
	, @cap_dataInicio DATE
	, @cap_dataFim DATE
AS
BEGIN 
	
	DECLARE @tud_tipo TINYINT
	
	SELECT TOP 1
		@tud_tipo = tud_tipo
	FROM
		TUR_TurmaDisciplina tud WITH(NOLOCK)
	WHERE
		tud_id = @tud_id
		AND tud_situacao <> 3

	DECLARE @MatriculaAlunos TABLE (
		mtd_id INT
		,mtu_id INT
		,alu_id BIGINT
		,mtd_numeroChamada INT
		,mtd_dataMatricula DATE
		,mtd_dataSaida DATETIME
		,mtd_situacao TINYINT
		,ava_id INT
		,mtd_dataAlteracao DATETIME
		,mtu_situacao TINYINT
		)
							
	IF (@tud_tipo = 15)
	BEGIN

		INSERT INTO @MatriculaAlunos
		(
			mtd_id,
			mtu_id,
			alu_id,
			mtd_numeroChamada,
			mtd_dataMatricula,
			mtd_dataSaida,
			mtd_situacao,
			ava_id,
			mtd_dataAlteracao,
			mtu_situacao
		)
		SELECT
			Mtd.mtd_id
			, Mtd.mtu_id
			, Mtd.alu_id
			, Mtd.mtd_numeroChamada
			, Mtd.mtd_dataMatricula
			, Mtd.mtd_dataSaida
			, Mtd.mtd_situacao
			, ISNULL(ava.ava_id, -1)
			, Mtd.mtd_dataAlteracao
			, Mtu.mtu_situacao
		FROM MTR_MatriculaTurmaDisciplina AS Mtd WITH (NOLOCK)
			INNER JOIN MTR_MatriculaTurma AS Mtu WITH (NOLOCK) 
				ON Mtu.alu_id = Mtd.alu_id 
				AND Mtu.mtu_id = Mtd.mtu_id
			INNER JOIN @dtTurmas dtt
				ON mtu.tur_id = dtt.tur_id
			INNER JOIN TUR_Turma AS tur WITH(NOLOCK) 
				ON tur.tur_id = dtt.tur_id 
				AND tur.tur_situacao <> 3					
			LEFT JOIN ACA_Avaliacao AS ava WITH(NOLOCK)
				ON tur.fav_id = ava.fav_id
				AND ava.tpc_id = @tpc_id
				AND ava.ava_situacao = 1
			LEFT JOIN ACA_CalendarioPeriodo AS Cap WITH(NOLOCK)
				ON Cap.cal_id = Tur.cal_id
				AND Cap.tpc_id = @tpc_id
				AND Cap.cap_situacao <> 3
		WHERE	
			mtd.tud_id = @tud_id
			AND mtd_situacao <> 3
			-- Valida o período de matrícula e saída do aluno (se está dentro do período atual).
			AND (Mtd.mtd_dataMatricula <= ISNULL(Cap.cap_dataFim, @cap_dataFim))
			AND (Mtd.mtd_dataSaida IS NULL OR Mtd.mtd_dataSaida >= ISNULL(Cap.cap_dataInicio, @cap_dataInicio))	
			AND (mtd_situacao = 1 OR (mtd_situacao = 5 AND ISNULL(mtd_numeroChamada, 0) >= 0))

		--Selecina as movimentações que possuem matrícula anterior
		; WITH TabelaMovimentacao AS (
			SELECT
				MOV.alu_id,
				MOV.mtu_idAnterior,
				CASE WHEN tmo_tipoMovimento IN (6, 12, 14, 15, 16)
					 THEN TMV.tmv_nome + ' em ' + REPLACE(CONVERT(VARCHAR(10), mov.mov_dataRealizacao, 103), '/' + CAST(DATEPART(YEAR, mov.mov_dataRealizacao) AS VARCHAR(4)), '') +
					 	  ISNULL(' p/ ' + turD.tur_codigo, '')
					 WHEN tmo_tipoMovimento IN (8)
					 THEN TMV.tmv_nome + ' p/' + ISNULL(' ' + turD.tur_codigo, '')
					 WHEN tmo_tipoMovimento IN (11)
					 THEN TMV.tmv_nome + ' p/ ' + ISNULL(' ' + turD.tur_codigo, '')
					 ELSE TMV.tmv_nome
				END tmv_nome  
			FROM
				@MatriculaAlunos AS MTR
				INNER JOIN MTR_Movimentacao AS MOV WITH (NOLOCK) 
					ON MOV.alu_id = MTR.alu_id
					AND MOV.mtu_idAnterior = MTR.mtu_id
				INNER JOIN ACA_TipoMovimentacao AS TMV WITH (NOLOCK) 
					ON MOV.tmv_idSaida = TMV.tmv_id
					AND TMV.tmv_situacao <> 3
				LEFT JOIN MTR_TipoMovimentacao tmo WITH(NOLOCK)
					ON mov.tmo_id = tmo.tmo_id
					AND tmo.tmo_situacao <> 3
				LEFT JOIN MTR_MatriculaTurma mtuD WITH(NOLOCK)
					ON mov.alu_id = mtuD.alu_id
					AND mov.mtu_idAtual = mtuD.mtu_id
				LEFT JOIN TUR_Turma turD WITH(NOLOCK)
					ON mtuD.tur_id = turD.tur_id
				LEFT JOIN ACA_CalendarioAnual calD WITH(NOLOCK)
					ON turD.cal_id = calD.cal_id
				INNER JOIN MTR_MatriculaTurma mtuO WITH(NOLOCK)
					ON mov.alu_id = mtuO.alu_id
					AND mov.mtu_idAnterior = mtuO.mtu_id
				INNER JOIN @dtTurmas dtt
					ON mtuO.tur_id = dtt.tur_id
				LEFT JOIN TUR_Turma turO WITH(NOLOCK)
					ON mtuO.tur_id = turO.tur_id
				LEFT JOIN ACA_CalendarioAnual calO WITH(NOLOCK)
					ON turO.cal_id = calO.cal_id
			WHERE
				MOV.mov_situacao NOT IN (3,4)
				AND MOV.mtu_idAnterior IS NOT NULL
		)
		
		, DeficienciaTud AS 
			(
				SELECT 
					RelTde.tde_id
				FROM
					TUR_TurmaDisciplinaMultisseriada tdm WITH(NOLOCK)
					INNER JOIN MTR_MatriculaTurmaDisciplina mtd WITH(NOLOCK)
						ON mtd.alu_id = tdm.alu_id
						AND mtd.mtu_id = tdm.mtu_id
						AND mtd.mtd_id = tdm.mtd_id
						AND mtd.mtd_situacao <> 3
					INNER JOIN TUR_TurmaDisciplina Tud WITH(NOLOCK)
						ON mtd.tud_id = tud.tud_id
					INNER JOIN TUR_TurmaDisciplinaRelDisciplina DisRel WITH(NOLOCK)
						ON Tud.tud_id = DisRel.tud_id
					INNER JOIN ACA_Disciplina dis WITH(NOLOCK)
						ON DisRel.dis_id = dis.dis_id
						AND dis.dis_situacao <> 3
					INNER JOIN ACA_TipoDisciplina tds WITH(NOLOCK)
						ON dis.tds_id = tds.tds_id
						AND tds.tds_situacao <> 3
					INNER JOIN ACA_TipoDisciplinaDeficiencia RelTde WITH(NOLOCK)
						ON tds.tds_id = RelTde.tds_id
				WHERE
					tdm.tud_idDocente = @tud_id
					AND tud_disciplinaEspecial = 1
			)
		
		SELECT	
			CASE WHEN @documentoOficial = 1 THEN Pes.pes_nomeOficial ELSE Pes.pes_nome END + 
				(
					CASE mtd_situacao 
						WHEN 5 THEN
							ISNULL((SELECT TOP 1 ' (' + tmv_nome + ')' FROM TabelaMovimentacao MOV WITH(NOLOCK) WHERE MOV.alu_id = MTR.alu_id AND MOV.mtu_idAnterior = MTR.mtu_id), ' (Inativo)')
						WHEN 4 THEN ISNULL('Em matrícula', '')
						WHEN 6 THEN ISNULL('Efetivado', '')
						ELSE ''
					END
				) AS pes_nome
			, pes.pes_dataNascimento
			, pes.pes_nomeMae NomeMae
			, alu.alu_dataCriacao
			, alu.alu_dataAlteracao
			, alu.alu_situacao AS alu_situacaoID
			, MTR.mtd_id
			, MTR.mtu_id
			, MTR.alu_id
			, MTR.mtd_numeroChamada
			, MTR.mtd_dataMatricula
			, MTR.mtd_dataSaida
			, MTR.mtd_situacao
			, MTR.ava_id
			, Pes.arq_idFoto
			, Pes.pes_nome_abreviado
			, (CASE WHEN SUM(CASE WHEN det.tde_id IS NULL THEN 0 ELSE 1 END) > 0 THEN 1 
				ELSE 0 END) PossuiDeficiencia
			, 0 AS dispensadisciplina
			, MTR.mtd_dataAlteracao as dataAlteracao		
			, MTR.mtu_situacao
		FROM @MatriculaAlunos AS MTR
			INNER JOIN ACA_Aluno AS ALU WITH (NOLOCK) 
				ON ALU.alu_id = MTR.alu_id
				AND ALU.alu_situacao <> 3
			INNER JOIN VW_DadosAlunoPessoa Pes 
				ON Alu.alu_id = Pes.alu_id
			LEFT JOIN Synonym_PES_PessoaDeficiencia pde WITH(NOLOCK)
				ON pde.pes_id = alu.pes_id	
			LEFT JOIN DeficienciaTud det
				ON pde.tde_id = det.tde_id
		GROUP BY
			pes.pes_nomeOficial
			, pes.pes_nome
			, pes.pes_dataNascimento
			, Pes.Pes_nomeMae
			, alu.alu_dataCriacao
			, alu.alu_dataAlteracao
			, alu.alu_situacao 
			, MTR.mtd_id
			, MTR.mtu_id
			, MTR.alu_id
			, MTR.mtd_dataMatricula
			, MTR.mtd_dataSaida
			, MTR.mtd_situacao
			, MTR.mtd_numeroChamada
			, MTR.ava_id
			, Pes.arq_idFoto
			, Pes.pes_nome_abreviado
			, MTR.mtd_dataAlteracao
			, MTR.mtu_situacao
		ORDER BY 
			CASE WHEN @documentoOficial = 1 THEN Pes.pes_nomeOficial ELSE Pes.pes_nome END ASC
	END
	ELSE 
	BEGIN	
		DECLARE @tur_id BIGINT
		SELECT @tur_id = t.tur_id FROM TUR_TurmaRelTurmaDisciplina t WITH(NOLOCK) 
		WHERE t.tud_id = @tud_id

		INSERT INTO @MatriculaAlunos
		(
			mtd_id,
			mtu_id,
			alu_id,
			mtd_numeroChamada,
			mtd_dataMatricula,
			mtd_dataSaida,
			mtd_situacao,
			ava_id,
			mtd_dataAlteracao,
			mtu_situacao
		)
		SELECT
			Mtd.mtd_id
			, Mtd.mtu_id
			, Mtd.alu_id
			, Mtd.mtd_numeroChamada
			, Mtd.mtd_dataMatricula
			, Mtd.mtd_dataSaida
			, Mtd.mtd_situacao
			, ISNULL(ava.ava_id, -1)
			, Mtd.mtd_dataAlteracao
			, Mtu.mtu_situacao
		FROM MTR_MatriculaTurmaDisciplina AS Mtd WITH (NOLOCK)
			INNER JOIN MTR_MatriculaTurma AS Mtu WITH (NOLOCK) 
				ON Mtu.alu_id = Mtd.alu_id 
				AND Mtu.mtu_id = Mtd.mtu_id
			INNER JOIN TUR_Turma AS tur WITH(NOLOCK) 
				ON tur.tur_id = Mtu.tur_id 
				AND tur.tur_situacao <> 3					
			LEFT JOIN ACA_Avaliacao AS ava WITH(NOLOCK)
				ON tur.fav_id = ava.fav_id
				AND ava.tpc_id = @tpc_id
				AND ava.ava_situacao = 1
			LEFT JOIN ACA_CalendarioPeriodo AS Cap WITH(NOLOCK)
				ON Cap.cal_id = Tur.cal_id
				AND Cap.tpc_id = @tpc_id
				AND Cap.cap_situacao <> 3
		WHERE	
			mtd.tud_id = @tud_id
			AND mtd_situacao <> 3
			-- Valida o período de matrícula e saída do aluno (se está dentro do período atual).
			AND (Mtd.mtd_dataMatricula <= ISNULL(Cap.cap_dataFim, @cap_dataFim))
			AND (Mtd.mtd_dataSaida IS NULL OR Mtd.mtd_dataSaida >= ISNULL(Cap.cap_dataInicio, @cap_dataInicio))	
			AND (mtd_situacao = 1 OR (mtd_situacao = 5 AND ISNULL(mtd_numeroChamada, 0) >= 0))

		--Selecina as movimentações que possuem matrícula anterior
		; WITH TabelaMovimentacao AS (
			SELECT
				MOV.alu_id,
				MOV.mtu_idAnterior,
				CASE WHEN tmo_tipoMovimento IN (6, 12, 14, 15, 16)
					 THEN TMV.tmv_nome + ' em ' + REPLACE(CONVERT(VARCHAR(10), mov.mov_dataRealizacao, 103), '/' + CAST(DATEPART(YEAR, mov.mov_dataRealizacao) AS VARCHAR(4)), '') +
					 	  ISNULL(' p/ ' + turD.tur_codigo, '')
					 WHEN tmo_tipoMovimento IN (8)
					 THEN TMV.tmv_nome + ' p/' + ISNULL(' ' + turD.tur_codigo, '')
					 WHEN tmo_tipoMovimento IN (11)
					 THEN TMV.tmv_nome + ' p/ ' + ISNULL(' ' + turD.tur_codigo, '')
					 ELSE TMV.tmv_nome
				END tmv_nome  
			FROM
				@MatriculaAlunos AS MTR
				INNER JOIN MTR_Movimentacao AS MOV WITH (NOLOCK) 
					ON MOV.alu_id = MTR.alu_id
					AND MOV.mtu_idAnterior = MTR.mtu_id
				INNER JOIN ACA_TipoMovimentacao AS TMV WITH (NOLOCK) 
					ON MOV.tmv_idSaida = TMV.tmv_id
					AND TMV.tmv_situacao <> 3
				LEFT JOIN MTR_TipoMovimentacao tmo WITH(NOLOCK)
					ON mov.tmo_id = tmo.tmo_id
					AND tmo.tmo_situacao <> 3
				LEFT JOIN MTR_MatriculaTurma mtuD WITH(NOLOCK)
					ON mov.alu_id = mtuD.alu_id
					AND mov.mtu_idAtual = mtuD.mtu_id
				LEFT JOIN TUR_Turma turD WITH(NOLOCK)
					ON mtuD.tur_id = turD.tur_id
				LEFT JOIN ACA_CalendarioAnual calD WITH(NOLOCK)
					ON turD.cal_id = calD.cal_id
				INNER JOIN MTR_MatriculaTurma mtuO WITH(NOLOCK)
					ON mov.alu_id = mtuO.alu_id
					AND mov.mtu_idAnterior = mtuO.mtu_id
					AND mtuO.tur_id = @tur_id
				LEFT JOIN TUR_Turma turO WITH(NOLOCK)
					ON mtuO.tur_id = turO.tur_id
				LEFT JOIN ACA_CalendarioAnual calO WITH(NOLOCK)
					ON turO.cal_id = calO.cal_id
			WHERE
				MOV.mov_situacao NOT IN (3,4)
				AND MOV.mtu_idAnterior IS NOT NULL
		)
		
		, DeficienciaTud AS 
			(
				SELECT 
					RelTde.tde_id
				FROM
					TUR_TurmaDisciplina Tud WITH(NOLOCK)
					INNER JOIN TUR_TurmaDisciplinaRelDisciplina DisRel WITH(NOLOCK)
						ON Tud.tud_id = DisRel.tud_id
					INNER JOIN ACA_Disciplina dis WITH(NOLOCK)
						ON DisRel.dis_id = dis.dis_id
						AND dis.dis_situacao <> 3
					INNER JOIN ACA_TipoDisciplina tds WITH(NOLOCK)
						ON dis.tds_id = tds.tds_id
						AND tds.tds_situacao <> 3
					INNER JOIN ACA_TipoDisciplinaDeficiencia RelTde WITH(NOLOCK)
						ON tds.tds_id = RelTde.tds_id
				WHERE
					Tud.tud_id = @tud_id
					AND tud_disciplinaEspecial = 1
			)
		
		SELECT	
			CASE WHEN @documentoOficial = 1 THEN Pes.pes_nomeOficial ELSE Pes.pes_nome END + 
				(
					CASE mtd_situacao 
						WHEN 5 THEN
							ISNULL((SELECT TOP 1 ' (' + tmv_nome + ')' FROM TabelaMovimentacao MOV WITH(NOLOCK) WHERE MOV.alu_id = MTR.alu_id AND MOV.mtu_idAnterior = MTR.mtu_id), ' (Inativo)')
						WHEN 4 THEN ISNULL('Em matrícula', '')
						WHEN 6 THEN ISNULL('Efetivado', '')
						ELSE ''
					END
				) AS pes_nome
			, pes.pes_dataNascimento
			, pes.pes_nomeMae NomeMae
			, alu.alu_dataCriacao
			, alu.alu_dataAlteracao
			, alu.alu_situacao AS alu_situacaoID
			, MTR.mtd_id
			, MTR.mtu_id
			, MTR.alu_id
			, MTR.mtd_numeroChamada
			, MTR.mtd_dataMatricula
			, MTR.mtd_dataSaida
			, MTR.mtd_situacao
			, MTR.ava_id
			, Pes.arq_idFoto
			, Pes.pes_nome_abreviado
			, (CASE WHEN SUM(CASE WHEN det.tde_id IS NULL THEN 0 ELSE 1 END) > 0 THEN 1 
				ELSE 0 END) PossuiDeficiencia
			, 0 AS dispensadisciplina
		    , MTR.mtd_dataAlteracao as dataAlteracao
			, MTR.mtu_situacao
		FROM @MatriculaAlunos AS MTR
			INNER JOIN ACA_Aluno AS Alu WITH (NOLOCK) 
				ON MTR.alu_id = Alu.alu_id
				AND alu_situacao <> 3
			INNER JOIN VW_DadosAlunoPessoa AS Pes 
				ON Alu.alu_id = Pes.alu_id
			LEFT JOIN Synonym_PES_PessoaDeficiencia AS pde WITH(NOLOCK)
				ON pde.pes_id = alu.pes_id	
			LEFT JOIN DeficienciaTud det
				ON pde.tde_id = det.tde_id
		GROUP BY
			pes.pes_nomeOficial
			, pes.pes_nome
			, pes.pes_dataNascimento
			, Pes.pes_nomeMae
			, alu.alu_dataCriacao
			, alu.alu_dataAlteracao
			, alu.alu_situacao 
			, MTR.mtd_id
			, MTR.mtu_id
			, MTR.alu_id
			, MTR.mtd_dataMatricula
			, MTR.mtd_dataSaida
			, MTR.mtd_situacao
			, MTR.mtd_numeroChamada
			, MTR.ava_id
			, Pes.arq_idFoto
			, Pes.pes_nome_abreviado
			, MTR.mtd_dataAlteracao
			, MTR.mtu_situacao
		ORDER BY 
			CASE WHEN @documentoOficial = 1 THEN Pes.pes_nomeOficial ELSE Pes.pes_nome END ASC
	END 
END
GO
PRINT N'Creating [dbo].[STP_ACA_AlunoJustificativaFaltaAnexo_LOAD]'
GO


CREATE PROCEDURE [dbo].[STP_ACA_AlunoJustificativaFaltaAnexo_LOAD]
	@alu_id BigInt
	, @afj_id Int
	, @aja_id Int
	
AS
BEGIN
	SELECT	Top 1
		 alu_id  
		, afj_id 
		, aja_id 
		, arq_id 
		, aja_descricao 
		, aja_situacao 
		, aja_dataCriacao 
		, aja_dataAlteracao 

 	FROM
 		ACA_AlunoJustificativaFaltaAnexo
	WHERE 
		alu_id = @alu_id
		AND afj_id = @afj_id
		AND aja_id = @aja_id
END


GO
PRINT N'Altering [dbo].[NEW_CLS_TurmaAula_SelectBy_Disciplina]'
GO
-- ========================================================================
-- Author:		Nicholas de Assis
-- Create date: 02/12/2014
-- Description:	Retorna as informações de planos de aula para o listão.

-- Alterado: Marcia Haga - 16/01/2015
-- Description: Adicionada ordenacao pela data da aula.

-- Alterado: Marcia Haga - 16/03/2015
-- Description: Alterado para retornar a posicao do docente e o usuario que criou a aula.

-- Alterado: Marcia Haga - 03/06/2015
-- Description: Alterada regra para relacionar a disciplina compartilhada.

-- Alterado: Haila Pelloso - 07/07/2015
-- Description: Alterada regra para selecionar apenas aulas do próprio docente caso não tenha permissão de visualizar a posição em que está.
-- ========================================================================
ALTER PROCEDURE [dbo].[NEW_CLS_TurmaAula_SelectBy_Disciplina]
	@tud_id BIGINT
	, @tpc_id INT
	, @tdt_posicao TINYINT
	, @usu_id UNIQUEIDENTIFIER
	, @usuario_superior BIT
	, @tud_idRelacionada BIGINT

AS
BEGIN
	DECLARE @tabCalendarioEvento TABLE 
	(
		evt_id				BIGINT
		, evt_dataInicio	DATE
		, evt_dataFim		DATE
	)
	
	DECLARE 
		@tud_tipo TINYINT
		, @tur_id BIGINT
		, @esc_id INT
		, @cal_id INT
		, @cal_ano INT
		, @cal_dataInicio DATE
		, @cal_dataFim DATE
		, @cid_id UNIQUEIDENTIFIER
		, @unf_id UNIQUEIDENTIFIER;
	
	;WITH EscolaEndereco AS (
		SELECT
			UAE.uad_id
			, Cid.cid_id
			, Cid.unf_id
		FROM
			TUR_TurmaDisciplina Tud WITH(NOLOCK)
		INNER JOIN TUR_TurmaRelTurmaDisciplina RelTud WITH(NOLOCK)
			ON RelTud.tud_id = Tud.tud_id
		INNER JOIN TUR_Turma Tur WITH(NOLOCK)
			ON Tur.tur_id = RelTud.tur_id
		INNER JOIN ESC_Escola Esc WITH(NOLOCK)
			ON Tur.esc_id = Esc.esc_id
			AND Esc.esc_situacao <> 3
		INNER JOIN Synonym_SYS_UnidadeAdministrativaEndereco Uae WITH(NOLOCK)
			ON Esc.uad_id = Uae.uad_id
			AND Uae.uae_enderecoPrincipal = 1
			AND Uae.uae_situacao <> 3
		INNER JOIN Synonym_END_Endereco EndEscola WITH(NOLOCK)
			ON Uae.end_id = EndEscola.end_id
			AND EndEscola.end_situacao <> 3
		INNER JOIN Synonym_END_Cidade Cid WITH(NOLOCK)
			ON EndEscola.cid_id = Cid.cid_id
			AND Cid.cid_situacao <> 3
		WHERE Tud.tud_id = @tud_id
	)

	SELECT 
		@tud_tipo = Tud.tud_tipo
		, @tur_id = Tur.tur_id
		, @esc_id = Tur.esc_id
		, @cal_id = Tur.cal_id
		, @cal_ano = Cal.cal_ano
		, @cal_dataInicio = Cal.cal_dataInicio
		, @cal_dataFim = Cal.cal_dataFim		
		, @cid_id = ende.cid_id
		, @unf_id = ende.unf_id
	FROM TUR_TurmaDisciplina Tud WITH(NOLOCK)
	INNER JOIN TUR_TurmaRelTurmaDisciplina RelTud WITH(NOLOCK)
		ON RelTud.tud_id = Tud.tud_id
	INNER JOIN TUR_Turma Tur WITH(NOLOCK)
		ON Tur.tur_id = RelTud.tur_id
	INNER JOIN ACA_CalendarioAnual Cal WITH(NOLOCK)
		ON Tur.cal_id = Cal.cal_id
		AND Cal.cal_situacao <> 3		
	INNER JOIN ESC_Escola Esc WITH(NOLOCK)
		ON Tur.esc_id = Esc.esc_id
		AND Esc.esc_situacao <> 3
	INNER JOIN Synonym_SYS_UnidadeAdministrativa Uad WITH(NOLOCK)
		ON Esc.uad_id = Uad.uad_id
		AND Uad.uad_situacao <> 3
	LEFT JOIN EscolaEndereco ende
		ON Uad.uad_id = ende.uad_id
	WHERE Tud.tud_id = @tud_id
	
	; WITH FeriadoCore AS (
		SELECT CAST((CAST(@cal_ano AS CHAR(4)) + '-' + CAST(MONTH(Dnu.dnu_data) AS CHAR(2)) + '-' + CAST(DAY(Dnu.dnu_data) AS CHAR(2))) AS DATE) AS DataFeriado
		FROM Synonym_SYS_DiaNaoUtil Dnu WITH(NOLOCK)
		WHERE
			Dnu.dnu_situacao <> 3
			AND (Dnu.dnu_recorrencia = 1 OR YEAR(Dnu.dnu_data) = @cal_ano)
			AND (Dnu.dnu_abrangencia = 1 --Nacional
				OR (Dnu.dnu_abrangencia = 2 AND Dnu.unf_id = @unf_id) --Estadual
				OR (Dnu.dnu_abrangencia = 3 AND Dnu.cid_id = @cid_id) --Municipal
			)
			AND ((Dnu.dnu_vigenciaInicio BETWEEN @cal_dataInicio AND @cal_dataFim)
				OR (Dnu.dnu_vigenciaFim BETWEEN @cal_dataInicio AND @cal_dataFim)
				OR (@cal_dataInicio BETWEEN Dnu.dnu_vigenciaInicio AND Dnu.dnu_vigenciaFim)
				OR (@cal_dataFim BETWEEN Dnu.dnu_vigenciaInicio AND Dnu.dnu_vigenciaFim)
			)
	)
	INSERT INTO @tabCalendarioEvento (evt_id ,evt_dataInicio ,evt_dataFim)
	SELECT 
			ce.evt_id
		, ev.evt_dataInicio
		, ev.evt_dataFim
	FROM ACA_CalendarioEvento  ce WITH (NOLOCK)
	INNER JOIN ACA_Evento ev WITH (NOLOCK) 
		ON ( ev.evt_id = ce.evt_id )
	WHERE
		(ev.evt_padrao = 1 OR ev.esc_id = @esc_id)
		AND ce.cal_id = @cal_id
		AND ev.evt_semAtividadeDiscente = 1
		AND ev.evt_situacao = 1
	
	UNION	
	--Dias não uteis - CoreSSO
	SELECT 1, DataFeriado, DataFeriado
	FROM FeriadoCore WITH(NOLOCK)

	DECLARE @PermissaoDocenteConsulta TABLE (tdt_posicaoPermissao INT, pdc_permissaoConsulta BIT, PRIMARY KEY(tdt_posicaoPermissao))
	DECLARE @PermissaoDocenteEdicao TABLE (tdt_posicaoPermissao INT, PRIMARY KEY(tdt_posicaoPermissao));

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
	INSERT INTO @PermissaoDocenteConsulta
	(
		tdt_posicaoPermissao,
		pdc_permissaoConsulta
	)
	SELECT 
		pdc.tdt_posicaoPermissao
		, MAX(pdc.pdc_permissaoConsulta) AS pdc_permissaoConsulta
	FROM
		tbDadosPermissaoDocenteConsulta pdc WITH(NOLOCK)
	GROUP BY
		pdc.tdt_posicaoPermissao

	INSERT INTO @PermissaoDocenteEdicao
	(
		tdt_posicaoPermissao
	)
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
	GROUP BY 
		tdcPermissao.tdc_posicao

	DECLARE @TurmaAula TABLE
	(
		tud_id BIGINT,
		tau_id INT,
		tau_data DATE,
		tau_numeroAulas INT,
		tau_planoAula TEXT,
		tau_conteudo TEXT,
		tau_situacao TINYINT,
		tau_dataCriacao DATETIME,
		tau_dataAlteracao DATETIME,
		tau_sintese NVARCHAR(MAX),
		tdt_posicao TINYINT,
		tau_reposicao BIT,
		usu_id UNIQUEIDENTIFIER,
		PRIMARY KEY (tud_id, tau_id)
	)

	;WITH TurmaAulaTerritorios AS (
		SELECT
			Tau.tud_id
			, Tau.tau_id
			, TauT.tud_id AS tud_idTerritorio
			, TauT.tau_id AS tau_idTerritorio
			, TauT.tau_numeroAulas
		FROM TUR_TurmaDisciplinaTerritorio tte WITH(NOLOCK)
		INNER JOIN CLS_TurmaAula Tau WITH(NOLOCK)
			ON tte.tud_idExperiencia = Tau.tud_id
			AND Tau.tpc_id = @tpc_id
			AND Tau.tau_data >= tte.tte_vigenciaInicio
			AND Tau.tau_data <= ISNULL(tte.tte_vigenciaFim, Tau.tau_data)
			AND Tau.tau_situacao <> 3
		INNER JOIN CLS_TurmaAulaTerritorio Tat WITH(NOLOCK)
			ON Tau.tud_id = Tat.tud_idExperiencia
			AND Tau.tau_id = Tat.tau_idExperiencia
			AND Tte.tud_idTerritorio = Tat.tud_idTerritorio
		INNER JOIN CLS_TurmaAula TauT WITH(NOLOCK)
			ON Tat.tud_idTerritorio = TauT.tud_id
			AND Tat.tau_idTerritorio = TauT.tau_id
			AND TauT.tau_situacao <> 3
	)
	
	INSERT INTO @TurmaAula
	(
		tud_id,
		tau_id,
		tau_data,
		tau_numeroAulas,
		tau_planoAula,
		tau_conteudo,
		tau_situacao,
		tau_dataCriacao,
		tau_dataAlteracao,
		tau_sintese,
		tdt_posicao,
		tau_reposicao,
		usu_id
	)
	SELECT
		 Tau.tud_id  
		, Tau.tau_id  
		, tau_data
		, CASE WHEN @tud_tipo = 18
				THEN (SELECT SUM(tat.tau_numeroAulas)
					  FROM TurmaAulaTerritorios tat
					  WHERE Tau.tud_id = tat.tud_id
							AND Tau.tau_id = tat.tau_id)
				ELSE Tau.tau_numeroAulas 
			  END AS tau_numeroAulas
		, tau_planoAula
		, tau_conteudo
		, tau_situacao
		, tau_dataCriacao
		, tau_dataAlteracao
		, tau_sintese
		, tau.tdt_posicao
		, tau.tau_reposicao
		, tau.usu_id
	FROM
		CLS_TurmaAula AS Tau WITH (NOLOCK)
	WHERE 
		Tau.tud_id = @tud_id
		AND tpc_id = @tpc_id
		AND tau_situacao <> 3
		AND (@tud_tipo <> 18 OR EXISTS(SELECT TOP 1 tat.tud_id
									   FROM TurmaAulaTerritorios tat
									   WHERE Tau.tud_id = tat.tud_id
											 AND Tau.tau_id = tat.tau_id))

	SELECT
		 Tau.tud_id  
		, Tau.tau_id  
		, -1 as tud_idFilho
		, CONVERT(VARCHAR(25), tau_data, 103) as data
		, tau_numeroAulas as numeroAulas
		, tau_planoAula as planoAula
		, tau_conteudo as conteudo
		, tau_situacao as situacao
		, tau_dataCriacao as dataCriacao
		, tau_dataAlteracao as dataAlteracao
		, tau_sintese as sintese
		, tau.tdt_posicao
		, tau.tau_reposicao
		, tau.usu_id
		, CASE WHEN Tau.usu_id = @usu_id OR @usuario_superior = 1 OR (ISNULL(pde.tdt_posicaoPermissao,0) > 0)  THEN 1 
			ELSE 0 END AS permissaoAlteracao
		, ISNULL(Apn.apn_semPlanoAula, 0) AS semPlanoAula
		, CASE WHEN EXISTS (
						SELECT	tab.evt_id
						FROM	@tabCalendarioEvento tab
						WHERE
							Tau.tau_data BETWEEN tab.evt_dataInicio AND tab.evt_dataFim

					) THEN 1
					ELSE 0
			END AS EventoSemAtividade
 	FROM
 		@TurmaAula AS Tau 
		INNER JOIN @PermissaoDocenteConsulta pdc
			ON Tau.tdt_posicao = pdc.tdt_posicaoPermissao
			AND (pdc.pdc_permissaoConsulta = 1 OR Tau.usu_id = @usu_id OR @usuario_superior = 1)
 		LEFT JOIN @PermissaoDocenteEdicao pde
			ON Tau.tdt_posicao = pde.tdt_posicaoPermissao

		LEFT JOIN CLS_TurmaAulaPendencia Apn WITH(NOLOCK)
			ON Tau.tud_id = Apn.tud_id
			AND Tau.tau_id = Apn.tau_id
		   				
		-- Se for disciplina de docencia compartilhada,
		-- retorno apenas as aulas compartilhadas com a disciplina relacionada do parametro.
		LEFT JOIN CLS_TurmaAulaDisciplinaRelacionada TauRel WITH(NOLOCK)
			ON @tud_tipo = 17
			AND TauRel.tud_id = Tau.tud_id
			AND TauRel.tau_id = Tau.tau_id
			AND TauRel.tud_idRelacionada = @tud_idRelacionada			
 	WHERE 
		(@tud_tipo <> 17 OR TauRel.tud_id IS NOT NULL)
		
	ORDER BY tau_data
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

---- Alterado: Marcia Haga - 30/03/2017
---- Description: Alterado para não indicar pendências em bimestres que ainda não foram abertos.
-- =============================================
ALTER PROCEDURE [dbo].[MS_JOB_ProcessamentoRelatorioDisciplinasAlunosPendencias]
	@tud_idFiltrar BIGINT = NULL
AS
BEGIN

	DECLARE @tbPendencias TABLE
	(tud_id BIGINT, tpc_id INT, processado tinyint, aberto BIT,
	PRIMARY	KEY	(tud_id, tpc_id))

	DECLARE @dataAtual DATE = CAST(GETDATE() AS DATE)

	DECLARE @tev_EfetivacaoNotas INT = 
	(
		SELECT TOP 1 CAST(pac.pac_valor AS INT)
		FROM ACA_ParametroAcademico pac WITH(NOLOCK)
		WHERE pac.pac_situacao <> 3 AND pac.pac_chave = 'TIPO_EVENTO_EFETIVACAO_NOTAS'
	);

	--adicionado este if para tratar o "fura-fila" de pendências... 
	--ao invés de pegar da CLS_AlunoFechamentoPendencia, pega dp parametro
	IF (@tud_idFiltrar is null)
	BEGIN
		-- Busca os registros que serão afetados.
		INSERT INTO @tbPendencias
		(tud_id, tpc_id, processado, aberto)
		SELECT TOP 500
			afp.tud_id, afp.tpc_id, 3,
			CASE WHEN 
			-- Só indicar pendência se é um bimestre que já foi aberto para fechamento.
			EXISTS
			(
				SELECT TOP 1 1 
				FROM ACA_Evento evt WITH(NOLOCK)
				INNER JOIN ACA_CalendarioEvento cae WITH(NOLOCK)
					ON cae.evt_id = evt.evt_id
					AND cae.cal_id = tur.cal_id
				WHERE evt.tev_id = @tev_EfetivacaoNotas
				AND evt.tpc_id = afp.tpc_id
				AND evt.evt_dataInicio <= @dataAtual
				AND 
				(
					evt.evt_padrao = 1
					OR evt.esc_id = tur.esc_id
				)
				AND evt.evt_situacao <> 3
			) THEN 1 ELSE 0 END
		FROM CLS_AlunoFechamentoPendencia afp WITH(NOLOCK)
		INNER JOIN TUR_TurmaRelTurmaDisciplina relTud WITH(NOLOCK)
			ON relTud.tud_id = afp.tud_id
		INNER JOIN TUR_Turma tur WITH(NOLOCK)
			ON tur.tur_id = relTud.tur_id
		WHERE afp_processado = 2

		; WITH Regencias AS
		(
			SELECT P.tud_id, P.tpc_id, ttrtd.tur_id, P.aberto
			FROM @tbPendencias P
			INNER JOIN dbo.TUR_TurmaDisciplina ttd WITH (NOLOCK)
			ON ttd.tud_id = P.tud_id
			AND ttd.tud_tipo=11--regencia
			INNER JOIN dbo.TUR_TurmaRelTurmaDisciplina ttrtd	 WITH (NOLOCK)
			ON ttrtd.tud_id = ttd.tud_id
		)
		-- Adicionar os componentes das regências que serão processadas.
		INSERT INTO @tbPendencias
		(tud_id, tpc_id, processado, aberto)
		SELECT TudComp.tud_id, R.tpc_id, 3, R.aberto
		FROM Regencias R
		INNER JOIN TUR_TurmaRelTurmaDisciplina RelTudComp WITH(NOLOCK)
			ON RelTudComp.tur_id = R.tur_id
		INNER JOIN dbo.TUR_TurmaDisciplina TudComp WITH (NOLOCK)
			ON TudComp.tud_id = RelTudComp.tud_id
			AND TudComp.tud_tipo = 12 --componente da regencia
		EXCEPT
		(
			SELECT tud_id, tpc_id, processado, aberto
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
		(tud_id, tpc_id, processado, aberto)

		(SELECT trel.tud_id, cap.tpc_id, 3,
			CASE WHEN 
			-- Só indicar pendência se é um bimestre que já foi aberto para fechamento.
			EXISTS
			(
				SELECT TOP 1 1 
				FROM ACA_Evento evt WITH(NOLOCK)
				INNER JOIN ACA_CalendarioEvento cae WITH(NOLOCK)
					ON cae.evt_id = evt.evt_id
					AND cae.cal_id = tur.cal_id
				WHERE evt.tev_id = @tev_EfetivacaoNotas
				AND evt.tpc_id = cap.tpc_id
				AND evt.evt_dataInicio <= @dataAtual
				AND 
				(
					evt.evt_padrao = 1
					OR evt.esc_id = tur.esc_id
				)
				AND evt.evt_situacao <> 3
			) THEN 1 ELSE 0 END
		   FROM TUR_TurmaRelTurmaDisciplina trel WITH(NOLOCK)
				INNER JOIN TUR_Turma tur WITH (NOLOCK) on tur.tur_id = trel.tur_id
				INNER JOIN ACA_CalendarioPeriodo cap WITH (NOLOCK) on cap.cal_id = tur.cal_id and cap.cap_situacao <> 3
		  WHERE trel.tud_id = @tud_idFiltrar
		
			UNION
		 
		 SELECT ttd2.tud_id, cap.tpc_id, 3,
			CASE WHEN 
			-- Só indicar pendência se é um bimestre que já foi aberto para fechamento.
			EXISTS
			(
				SELECT TOP 1 1 
				FROM ACA_Evento evt WITH(NOLOCK)
				INNER JOIN ACA_CalendarioEvento cae WITH(NOLOCK)
					ON cae.evt_id = evt.evt_id
					AND cae.cal_id = tur.cal_id
				WHERE evt.tev_id = @tev_EfetivacaoNotas
				AND evt.tpc_id = cap.tpc_id
				AND evt.evt_dataInicio <= @dataAtual
				AND 
				(
					evt.evt_padrao = 1
					OR evt.esc_id = tur.esc_id
				)
				AND evt.evt_situacao <> 3
			) THEN 1 ELSE 0 END
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

			-- Só marca pendência se o período de fechamento do bimestre já esteve aberto.
			UPDATE @AlunosDisciplinasPendencias
			SET SemNota = 0
			FROM @AlunosDisciplinasPendencias P	
			INNER JOIN @tbPendencias pen
				ON pen.tud_id = P.tud_id
				AND pen.tpc_id = P.tpc_id
				AND pen.aberto = 0

			-- Não marca pendência de nota para as disciplinas que não lançam nota.
			UPDATE @AlunosDisciplinasPendencias
			SET SemNota = 0
			WHERE tud_naoLancarNota = 1

			--IF (@tpc_id = 4)
			BEGIN -- Pegar alunos sem parecer conclusivo se for o último bimestre.
				UPDATE @AlunosDisciplinasPendencias
				SET SemParecer = 1
				FROM @AlunosDisciplinasPendencias P
				-- Só marca pendência se o período de fechamento do bimestre já esteve aberto.
				INNER JOIN @tbPendencias pen
					ON pen.tud_id = P.tud_id
					AND pen.tpc_id = P.tpc_id
					AND pen.aberto = 1
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
				-- Só marca pendência se o período de fechamento do bimestre já esteve aberto.
				INNER JOIN @tbPendencias pen
					ON pen.tud_id = P.tud_id
					AND pen.tpc_id = P.tpc_id
					AND pen.aberto = 1
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
				-- Só marca pendência se o período de fechamento do bimestre já esteve aberto.
				INNER JOIN @tbPendencias pen
					ON pen.tud_id = P.tud_id
					AND pen.tpc_id = P.tpc_id
					AND pen.aberto = 1
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
				-- Só marca pendência se o período de fechamento do bimestre já esteve aberto.
				INNER JOIN @tbPendencias pen
					ON pen.tud_id = P.tud_id
					AND pen.tpc_id = P.tpc_id
					AND pen.aberto = 1
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
			-- Só marca pendência se o período de fechamento do bimestre já esteve aberto.
			INNER JOIN @tbPendencias pen
				ON pen.tud_id = P.tud_id
				AND pen.tpc_id = P.tpc_id
				AND pen.aberto = 1

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
			-- Só marca pendência se o período de fechamento do bimestre já esteve aberto.
			INNER JOIN @tbPendencias pen
				ON pen.tud_id = P.tud_id
				AND pen.tpc_id = P.tpc_id
				AND pen.aberto = 1

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
PRINT N'Altering [dbo].[NEW_MTR_MatriculaTurmaDisciplina_SelectFechamentoFiltroDeficiencia]'
GO
-- Stored Procedure

-- ========================================================================
-- Author:		Marcia Haga
-- Create date: 29/07/2015
-- Description: Retorna os alunos matriculados na Turma para o período informado,
--				de acordo com as regras necessárias para ele aparecer na listagem
--				para efetivar. Filtrando os alunos com ou sem deficiência, dependendo do docente.

---- Alterado: Marcia Haga - 29/07/2015 
---- Description: Alterado para retornar o numero de aulas de reposicao e frequencia.

---- Alterado: Marcia Haga - 30/07/2015 
---- Description: Alterado para retornar se o registro do conselho foi parcialmente preenchido.

---- Alterado: Marcia Haga - 10/08/2015
---- Description: Alterado para verificar o periodo em que o aluno esteve 
---- presente na turma eletiva de aluno ou multisseriada.

---- Alterado: Marcia Haga - 11/08/2015
---- Description: Alterado para priorizar os dados pre-processados, ao inves dos dados ja efetivados.
-- ========================================================================
ALTER PROCEDURE [dbo].[NEW_MTR_MatriculaTurmaDisciplina_SelectFechamentoFiltroDeficiencia]
	@tud_id BIGINT
	, @tur_id BIGINT 
	, @tpc_id INT
	, @ava_id INT
	, @ordenacao INT
	, @fav_id INT 
	, @tipoAvaliacao TINYINT
	, @permiteAlterarResultado BIT
	, @tur_tipo TINYINT
	, @cal_id INT  
	, @tdc_id TINYINT
	, @dtTurma TipoTabela_Turma READONLY
	, @documentoOficial BIT
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL SNAPSHOT

	DECLARE @MatriculasBoletimDaTurma AS TipoTabela_MatriculasBoletim;

	DECLARE @Matriculas TABLE (alu_id BIGINT NOT NULL, mtu_id INT NOT NULL, mtd_id INT NOT NULL, tur_id BIGINT, tpc_id INT, tpc_ordem INT, tud_id BIGINT, fav_id INT
	, registroExterno BIT, PossuiSaidaPeriodo BIT, variacaoFrequencia DECIMAL(18,3), mtd_numeroChamadaDocente INT NULL
	, mtd_situacaoDocente TINYINT NULL, mtd_dataMatriculaDocente DATE NULL, mtd_dataSaidaDocente DATE NULL
	, PRIMARY KEY (alu_id, mtu_id, mtd_id, tpc_id));

	DECLARE @MatriculaMultisseriadaTurmaAluno TABLE 
		(
			tud_idDocente BIGINT, 
			alu_id BIGINT, 
			mtu_id INT, 
			mtd_id INT,
			tud_idAluno BIGINT
			PRIMARY KEY (tud_idDocente, alu_id, mtu_id, mtd_id)
		);

	DECLARE @tds_id INT = 
		(
			 --Busca o tipo de disciplina para filtrar os mtds abaixo.
			SELECT Dis.tds_id
			FROM TUR_TurmaDisciplinaRelDisciplina RelDis -- WITH (NOLOCK)
			INNER JOIN ACA_Disciplina Dis -- WITH (NOLOCK)
				ON Dis.dis_id = RelDis.dis_id
			WHERE
				RelDis.tud_id = @tud_id
		)

	DECLARE @tud_tipo INT = 
		(
			 --Busca o tipo de disciplina para filtrar os mtds abaixo.
			SELECT TUD.tud_tipo
			FROM TUR_TurmaDisciplina AS TUD -- WITH (NOLOCK)
			WHERE
				TUD.tud_id = @tud_id
		)

	--Se for turma de eletiva do aluno, carrega os alunos que devem aparecer na tela de efetivação
	IF ( @tur_tipo IN (2,3) ) BEGIN
		-- Turma eletiva de aluno ou multisseriada, buscar matrículas por aluno.
		DECLARE @tbMatriculaAlunos TipoTabela_AlunoMatriculaTurma;
		INSERT INTO @tbMatriculaAlunos (alu_id, mtu_id)
		SELECT Mtd.alu_id, MAX(Mtd.mtu_id)
		FROM MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
		WHERE
			-- Busca mtus dos alunos pelo MTD - o mtd está ligado ao tud informado, e o mtu_id aponta pra outra turma (turma normal).
			Mtd.tud_id = @tud_id
			AND Mtd.mtd_situacao <> 3
		GROUP BY alu_id	

		INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
		Select mb.alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mb.mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes 
		  from MTR_MatriculasBoletim mb  -- WITH (NOLOCK)
			   inner join @tbMatriculaAlunos amt on amt.alu_id = mb.alu_id and amt.mtu_id = mb.mtu_origemDados

		IF not exists (select top 1 alu_id from @MatriculasBoletimDaTurma)
		begin
			INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
				PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
				PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
			EXEC NEW_MTR_MatriculaTurma_MatriculasBoletim_Alunos
				@tbMatriculaTurma = @tbMatriculaAlunos
		end
	END
	ELSE IF (@tur_tipo = 4)
	BEGIN
		-- Turma eletiva de aluno ou multisseriada, buscar matrículas por aluno.
		DECLARE @tbMatriculaAlunosMultisseriada TipoTabela_AlunoMatriculaTurma;
		INSERT INTO @tbMatriculaAlunosMultisseriada (alu_id, mtu_id)
		SELECT Mtd.alu_id, MAX(Mtd.mtu_id)
		FROM MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
		INNER JOIN MTR_MatriculaTurma mtu
			ON Mtd.alu_id = mtu.alu_id
			AND Mtd.mtu_id = mtu.mtu_id
			AND mtu.mtu_situacao <> 3
		INNER JOIN @dtTurma dtt
			ON mtu.tur_id = dtt.tur_id
		WHERE
			-- Busca mtus dos alunos pelo MTD - o mtd está ligado ao tud informado, e o mtu_id aponta pra outra turma (turma normal).
			Mtd.tud_id = @tud_id
			AND Mtd.mtd_situacao <> 3
		GROUP BY mtd.alu_id	

		INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
		Select mb.alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mb.mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes 
		  from MTR_MatriculasBoletim mb  -- WITH (NOLOCK)
			   inner join @tbMatriculaAlunosMultisseriada amt on amt.alu_id = mb.alu_id and amt.mtu_id = mb.mtu_origemDados

		IF not exists (select top 1 alu_id from @MatriculasBoletimDaTurma)
		begin
			INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
				PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
				PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
			EXEC NEW_MTR_MatriculaTurma_MatriculasBoletim_Alunos
				@tbMatriculaTurma = @tbMatriculaAlunosMultisseriada
		end

		INSERT INTO @MatriculaMultisseriadaTurmaAluno (tud_idDocente, alu_id, mtu_id, mtd_id, tud_idAluno)
		SELECT 
			mtdDocente.tud_id AS tud_idDocente,
			mtdAluno.alu_id,
			mtdAluno.mtu_id,
			mtdAluno.mtd_id,
			mtdAluno.tud_id AS tud_idAluno
		FROM @MatriculasBoletimDaTurma mtr
		INNER JOIN MTR_MatriculaTurmaDisciplina mtdDocente
			ON mtr.alu_id = mtdDocente.alu_id
			AND mtr.mtu_id = mtdDocente.mtu_id
			AND mtdDocente.tud_id = @tud_id
			AND mtdDocente.mtd_situacao <> 3
		INNER JOIN MTR_MatriculaTurmaDisciplina mtdAluno
			ON mtdAluno.alu_id = mtr.alu_id
			AND mtdAluno.mtu_id = mtr.mtu_id
			AND mtdAluno.mtd_situacao <> 3
		INNER JOIN TUR_TurmaDisciplina tudAluno
			ON mtdAluno.tud_id = tudAluno.tud_id
			AND tudAluno.tud_id <> @tud_id
			AND tudAluno.tud_tipo = 16
			AND tudAluno.tud_situacao <> 3
		INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDisAluno
			ON RelDisAluno.tud_id = tudAluno.tud_id
		INNER JOIN ACA_Disciplina disAluno
			ON RelDisAluno.dis_id = disAluno.dis_id
			AND disAluno.tds_id = @tds_id
			AND disAluno.dis_situacao <> 3
		GROUP BY
			mtdDocente.tud_id,
			mtdAluno.alu_id,
			mtdAluno.mtu_id,
			mtdAluno.mtd_id,
			mtdAluno.tud_id
	END
	 --Se for turma normal, carrega os alunos de acordo com o boletim
	ELSE
	BEGIN
		INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
			PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
			PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
		select mb.alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mb.mtu_id,mb.tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mb.mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes
		  from MTR_MatriculasBoletim mb
			   inner join (select alu_id, mtu_id, ROW_NUMBER() OVER(PARTITION BY alu_id 
														   ORDER BY mtu_id desc) as linha
							 from MTR_MatriculaTurma -- WITH (NOLOCK) 
							where mtu_situacao <> 3
							  and tur_id = @tur_id) mtu 
					   on mtu.alu_id = mb.alu_id and mtu.mtu_id = mb.mtu_origemDados
		 where mtu.linha = 1 --para evitar casos onde o aluno tem dois mtus para o mesmo tur_id

		IF not exists (select top 1 alu_id from @MatriculasBoletimDaTurma)
		begin
			INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
				PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
				PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
			EXEC NEW_MTR_MatriculaTurma_MatriculasBoletim_DaTurma
				@tur_id = @tur_id;
		end
	END

	IF (@tur_tipo = 4)
	BEGIN
		INSERT INTO @Matriculas 
		(
			alu_id, 
			mtu_id, 
			mtd_id, 
			fav_id, 
			tpc_id, 
			tpc_ordem, 
			tud_id, 
			tur_id, 
			registroExterno, 
			PossuiSaidaPeriodo, 
			variacaoFrequencia, 
			mtd_numeroChamadaDocente,
			mtd_situacaoDocente, 
			mtd_dataMatriculaDocente, 
			mtd_dataSaidaDocente
		)
		SELECT
			Mtr.alu_id
			, Mtr.mtu_id
			, Mtd.mtd_id
			, Mtr.fav_id
			, Mtr.tpc_id
			, Mtr.tpc_ordem
			, mtd.tud_id
			, Mtr.tur_id
			, Mtr.registroExterno
			, Mtr.PossuiSaidaPeriodo
			, FAV.fav_variacao
			, mtdDocente.mtd_numeroChamada
			, mtdDocente.mtd_situacao
			, mtdDocente.mtd_dataMatricula
			, mtdDocente.mtd_dataSaida
		FROM @MatriculasBoletimDaTurma Mtr
		INNER JOIN MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
			ON Mtd.alu_id = Mtr.alu_id
			AND Mtd.mtu_id = Mtr.mtu_id
			AND Mtd.mtd_situacao <> 3
		INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDis -- WITH (NOLOCK)
			ON RelDis.tud_id = Mtd.tud_id
		INNER JOIN ACA_Disciplina Dis -- WITH (NOLOCK)
			ON RelDis.dis_id = Dis.dis_id
		INNER JOIN dbo.ACA_FormatoAvaliacao FAV -- WITH (NOLOCK)
			ON	FAV.fav_id = Mtr.fav_id
			AND FAV.fav_situacao <> 3
		INNER JOIN @MatriculaMultisseriadaTurmaAluno tdm 
			ON Mtd.alu_id = tdm.alu_id
			AND Mtd.mtu_id = tdm.mtu_id
			AND Mtd.mtd_id = tdm.mtd_id
		INNER JOIN MTR_MatriculaTurmaDisciplina mtdDocente
			ON mtdDocente.alu_id = Mtd.alu_id
			AND mtdDocente.mtu_id = Mtd.mtu_id
			AND mtdDocente.tud_id = tdm.tud_idDocente
			AND mtdDocente.mtd_situacao <> 3
		WHERE
			Mtr.mtu_id IS NOT NULL
			 --Busca a matrícula na mtd ligada à disciplina (pelo tds_id), em cada COC, da lista de mtus (MatriculasBoletim).
			AND Dis.tds_id = @tds_id
			 --Filtros de matrícula.
			AND Mtr.MesmoCalendario = 1
			AND Mtr.PeriodosEquivalentes = 1
		GROUP BY
			Mtr.alu_id
			, Mtr.mtu_id
			, Mtd.mtd_id
			, Mtr.fav_id
			, Mtr.tpc_id
			, Mtr.tpc_ordem
			, mtd.tud_id
			, Mtr.tur_id
			, Mtr.registroExterno
			, Mtr.PossuiSaidaPeriodo
			, FAV.fav_variacao
			, mtdDocente.mtd_numeroChamada
			, mtdDocente.mtd_situacao
			, mtdDocente.mtd_dataMatricula
			, mtdDocente.mtd_dataSaida
	END
	ELSE
	BEGIN
		INSERT INTO @Matriculas (alu_id, mtu_id, mtd_id, fav_id, tpc_id, tpc_ordem, tud_id, tur_id, registroExterno, PossuiSaidaPeriodo, variacaoFrequencia)
		SELECT
			Mtr.alu_id
			, Mtr.mtu_id
			, Mtd.mtd_id
			, Mtr.fav_id
			, Mtr.tpc_id
			, Mtr.tpc_ordem
			, Mtd.tud_id
			, Mtr.tur_id
			, Mtr.registroExterno
			, Mtr.PossuiSaidaPeriodo
			, FAV.fav_variacao
		FROM @MatriculasBoletimDaTurma Mtr
		INNER JOIN MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
			ON Mtd.alu_id = Mtr.alu_id
			AND Mtd.mtu_id = Mtr.mtu_id
			AND Mtd.mtd_situacao <> 3
		INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDis -- WITH (NOLOCK)
			ON RelDis.tud_id = Mtd.tud_id
		INNER JOIN ACA_Disciplina Dis -- WITH (NOLOCK)
			ON RelDis.dis_id = Dis.dis_id
		INNER JOIN dbo.ACA_FormatoAvaliacao FAV -- WITH (NOLOCK)
			ON	FAV.fav_id = Mtr.fav_id
			AND FAV.fav_situacao <> 3
		WHERE
			Mtr.mtu_id IS NOT NULL
			 --Busca a matrícula na mtd ligada à disciplina (pelo tds_id), em cada COC, da lista de mtus (MatriculasBoletim).
			AND Dis.tds_id = @tds_id
			 --Filtros de matrícula.
			AND Mtr.MesmoCalendario = 1
			AND (Mtr.PeriodosEquivalentes = 1 OR @tur_tipo = 2)
    END

	-- Verifica o periodo em que o aluno esteve presente na turma eletiva de aluno ou multisseriada
	IF ( @tur_tipo IN (2,3,4) ) 
	BEGIN
		;WITH PresencaAlunoPeriodo AS
		(
			SELECT Mat.alu_id, Mat.mtu_id, Mat.mtd_id, Mat.tpc_id 
			FROM @Matriculas Mat
			INNER JOIN TUR_Turma Tur -- WITH (NOLOCK)
				ON Tur.tur_id = Mat.tur_id
			INNER JOIN MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
				ON Mtd.alu_id = Mat.alu_id
				AND Mtd.mtu_id = Mat.mtu_id
				AND Mtd.mtd_id = Mat.mtd_id
			INNER JOIN ACA_TipoPeriodoCalendario Tpc -- WITH (NOLOCK)
				ON Tpc.tpc_id = Mat.tpc_id
			INNER JOIN ACA_CalendarioPeriodo Cap -- WITH (NOLOCK)
				ON Cap.tpc_id = Tpc.tpc_id
				AND Cap.cal_id = Tur.cal_id
				AND Cap.cap_situacao <> 3
			WHERE
			(
				-- O aluno nao estava presente no periodo se:
				-- o aluno saiu durante o periodo
				Mtd.mtd_dataSaida BETWEEN Cap.cap_dataInicio AND Cap.cap_dataFim
				-- ou o aluno saiu antes de o periodo iniciar
				OR Mtd.mtd_dataSaida < Cap.cap_dataInicio
				-- ou o aluno entrou depois do fim do periodo
				OR Mtd.mtd_dataMatricula > Cap.cap_dataFim
			)
			AND Mat.PossuiSaidaPeriodo = 0
		)
		UPDATE @Matriculas
		SET PossuiSaidaPeriodo = 1
		FROM @Matriculas Mat
		INNER JOIN PresencaAlunoPeriodo Pap
			ON Pap.alu_id = Mat.alu_id
			AND Pap.mtu_id = Mat.mtu_id
			AND Pap.mtd_id = Mat.mtd_id
			AND Pap.tpc_id = Mat.tpc_id
	END

	/* [Carla 19/07/2013]
		Excluir os registros que não devem ser exibidos na tela. Esses registros devem ser trazidos para buscar
		a nota do bimestre anterior, caso seja recuperação.
	*/
	DELETE FROM @Matriculas
	WHERE
		registroExterno = 1
		-- Se possuir uma saída no período, não exibe o aluno.
		OR PossuiSaidaPeriodo = 1
		-- Excluir matrículas de outras turmas, pois traz todos os bimestres pra fazer os acumulados.
		OR (@tur_tipo = 1 AND tur_id <> @tur_id)
		-- Excluir matrículas de outras disciplinas em turmas eletivas/multisseriadas.
		OR (@tur_tipo IN (2,3) AND tud_id <> @tud_id)
		-- Excluir matrículas de outras disciplinas do territorio do saber com o mesmo tds
		OR ((@tud_tipo = 18) AND (tud_id <> @tud_id))

	/**/
	DECLARE @tbAlunos TABLE (alu_id INT);	
	IF (@tdc_id = 5)
	BEGIN
		;WITH TipoDeficiencia AS 
		(
			SELECT 
				RelTde.tde_id
			FROM
				TUR_TurmaDisciplinaRelDisciplina DisRel --WITH(NOLOCK)
				INNER JOIN ACA_Disciplina dis --WITH(NOLOCK)
					ON DisRel.dis_id = dis.dis_id
					AND dis.dis_situacao <> 3
				INNER JOIN ACA_TipoDisciplina tds --WITH(NOLOCK)
					ON dis.tds_id = tds.tds_id
					AND tds.tds_situacao <> 3
				INNER JOIN ACA_TipoDisciplinaDeficiencia RelTde-- WITH(NOLOCK)
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
			@Matriculas mtd 
			INNER JOIN ACA_Aluno alu-- WITH(NOLOCK)
				ON alu.alu_id = mtd.alu_id
			INNER JOIN Synonym_PES_PessoaDeficiencia pde --WITH(NOLOCK)
				ON pde.pes_id = alu.pes_id
			INNER JOIN TipoDeficiencia tde
				ON pde.tde_id = tde.tde_id
	END
	ELSE
	BEGIN
		;WITH TipoDeficiencia AS 
		(
			SELECT 
				RelTde.tde_id
			FROM
				TUR_TurmaDisciplinaRelDisciplina DisRel --WITH(NOLOCK)
				INNER JOIN ACA_Disciplina dis --WITH(NOLOCK)
					ON DisRel.dis_id = dis.dis_id
					AND dis.dis_situacao <> 3
				INNER JOIN ACA_TipoDisciplina tds --WITH(NOLOCK)
					ON dis.tds_id = tds.tds_id
					AND tds.tds_situacao <> 3
				INNER JOIN ACA_TipoDisciplinaDeficiencia RelTde --WITH(NOLOCK)
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
			@Matriculas mtd 
			INNER JOIN ACA_Aluno alu-- WITH(NOLOCK)
				ON alu.alu_id = mtd.alu_id
			LEFT JOIN Synonym_PES_PessoaDeficiencia pde --WITH(NOLOCK)
				ON pde.pes_id = alu.pes_id
		WHERE
			(NOT EXISTS (SELECT tde_id FROM TipoDeficiencia tde WHERE tde.tde_id = pde.tde_id ))	
	END
	/**/

	; WITH TabelaMovimentacao AS (

			--Selecina as movimentações que possuem matrícula anterior
			SELECT
				MOV.alu_id,
				mtu_idAnterior,
				CASE WHEN tmo_tipoMovimento IN (6, 12, 14, 15, 16)
					 THEN TMV.tmv_nome + ' em ' + REPLACE(CONVERT(VARCHAR(10), mov.mov_dataRealizacao, 103), '/' + CAST(DATEPART(YEAR, mov.mov_dataRealizacao) AS VARCHAR(4)), '') +
					 	  ISNULL(' p/ ' + turD.tur_codigo, '')
					 WHEN tmo_tipoMovimento IN (8)
					 THEN TMV.tmv_nome + ' p/' + ISNULL(' ' + turD.tur_codigo, '')
					 WHEN tmo_tipoMovimento IN (11)
					 THEN TMV.tmv_nome + ' p/ ' + ISNULL(' ' + turD.tur_codigo, '')
					 ELSE TMV.tmv_nome
				END tmv_nome  
			FROM
				@tbAlunos tAlu
				/**/
				INNER JOIN MTR_Movimentacao MOV -- WITH (NOLOCK) 
					ON tAlu.alu_id = MOV.alu_id 
				/**/
				INNER JOIN ACA_TipoMovimentacao TMV -- WITH (NOLOCK) 
					ON MOV.tmv_idSaida = TMV.tmv_id
				LEFT JOIN MTR_TipoMovimentacao tmo -- WITH(NOLOCK)
					ON mov.tmo_id = tmo.tmo_id
					AND tmo.tmo_situacao <> 3
				LEFT JOIN MTR_MatriculaTurma mtuD -- WITH(NOLOCK)
					ON mov.alu_id = mtuD.alu_id
					AND mov.mtu_idAtual = mtuD.mtu_id
				LEFT JOIN TUR_Turma turD -- WITH(NOLOCK)
					ON mtuD.tur_id = turD.tur_id
				LEFT JOIN ACA_CalendarioAnual calD -- WITH(NOLOCK)
					ON turD.cal_id = calD.cal_id
				INNER JOIN MTR_MatriculaTurma mtuO -- WITH(NOLOCK)
					ON mov.alu_id = mtuO.alu_id
					AND mov.mtu_idAnterior = mtuO.mtu_id
					AND mtuO.tur_id = @tur_id
				LEFT JOIN TUR_Turma turO -- WITH(NOLOCK)
					ON mtuO.tur_id = turO.tur_id
				LEFT JOIN ACA_CalendarioAnual calO -- WITH(NOLOCK)
					ON turO.cal_id = calO.cal_id
			WHERE
				mov_situacao NOT IN (3,4)
				AND tmv_situacao <> 3
				AND mtu_idAnterior IS NOT NULL	
	), 

	TabelaObservacaoConselho AS 
	(
		SELECT
			tur_id
			, alu_id
			, mtu_id
			, CASE WHEN qtdeQualidade = 0 AND qtdeDesempenhos = 0 AND qtdeRecomendacao = 0
						AND ato_qualidade IS NULL AND ato_desempenhoAprendizado IS NULL 
						AND ato_recomendacaoAluno IS NULL AND ato_recomendacaoResponsavel IS NULL
				   -- nenhum campo preenchido
				   THEN 0
				   ELSE
					(CASE WHEN ato_desempenhoAprendizado IS NOT NULL 
							AND ato_recomendacaoAluno IS NOT NULL 
							AND ato_recomendacaoResponsavel IS NOT NULL
					-- todos os campos preenchidos
					THEN 1
					-- algum campo preenchido
					ELSE 2
					END)
			  END AS observacaoPreenchida
		FROM
		(
			SELECT
				Mtr.tur_id
				, Mtr.alu_id
				, Mtr.mtu_id
				, SUM(CASE WHEN aaq.tqa_id IS NULL THEN 0 ELSE 1 END) AS qtdeQualidade
				, SUM(CASE WHEN aad.tda_id IS NULL THEN 0 ELSE 1 END) AS qtdeDesempenhos
				, SUM(CASE WHEN aar.rar_id IS NULL THEN 0 ELSE 1 END) AS qtdeRecomendacao
				, ato_qualidade
				, ato_desempenhoAprendizado
				, ato_recomendacaoAluno
				, ato_recomendacaoResponsavel
			FROM
				@Matriculas Mtr
				/**/
				INNER JOIN @tbAlunos tAlu
					ON tAlu.alu_id = Mtr.alu_id
				/**/
				INNER JOIN ACA_Avaliacao ava -- WITH (NOLOCK)
					ON Mtr.fav_id = ava.fav_id
					AND ava.ava_id = @ava_id
					AND ava.ava_exibeObservacaoConselhoPedagogico = 1
				LEFT JOIN CLS_AlunoAvaliacaoTurmaQualidade aaq -- WITH (NOLOCK)
					ON  Mtr.tur_id = aaq.tur_id
					AND Mtr.alu_id = aaq.alu_id
					AND Mtr.mtu_id = aaq.mtu_id
					AND aaq.fav_id = ava.fav_id
					AND aaq.ava_id = ava.ava_id

				LEFT JOIN CLS_AlunoAvaliacaoTurmaDesempenho aad -- WITH (NOLOCK)
					ON  Mtr.tur_id = aad.tur_id
					AND Mtr.alu_id = aad.alu_id
					AND Mtr.mtu_id = aad.mtu_id
					AND aad.fav_id = ava.fav_id
					AND aad.ava_id = ava.ava_id 

				LEFT JOIN CLS_AlunoAvaliacaoTurmaRecomendacao aar -- WITH (NOLOCK)
					ON  Mtr.tur_id = aar.tur_id
					AND Mtr.alu_id = aar.alu_id
					AND Mtr.mtu_id = aar.mtu_id
					AND aar.fav_id = ava.fav_id
					AND aar.ava_id = ava.ava_id

				LEFT JOIN CLS_ALunoAvaliacaoTurmaObservacao ato -- WITH (NOLOCK)
					ON Mtr.tur_id = ato.tur_id 
					AND Mtr.alu_id = ato.alu_id
					AND Mtr.mtu_id = ato.mtu_id
					AND ato.fav_id = ava.fav_id
					AND ato.ava_id = ava.ava_id
					AND ato.ato_situacao <> 3
			WHERE
				Mtr.tud_id = @tud_id
			GROUP BY
				Mtr.tur_id
				, Mtr.alu_id
				, Mtr.mtu_id
				, ato_qualidade
				, ato_desempenhoAprendizado
				, ato_recomendacaoAluno
				, ato_recomendacaoResponsavel
		) AS tabela
			GROUP BY --(Adicionado group by por Webber) 
				tabela.tur_id
				, tabela.alu_id 
				, tabela.mtu_id 
				, CASE WHEN qtdeQualidade = 0 AND qtdeDesempenhos = 0 AND qtdeRecomendacao = 0
							AND ato_qualidade IS NULL AND ato_desempenhoAprendizado IS NULL 
							AND ato_recomendacaoAluno IS NULL AND ato_recomendacaoResponsavel IS NULL
					   -- nenhum campo preenchido
					   THEN 0
					   ELSE
						(CASE WHEN ato_desempenhoAprendizado IS NOT NULL 
								AND ato_recomendacaoAluno IS NOT NULL 
								AND ato_recomendacaoResponsavel IS NOT NULL
						-- todos os campos preenchidos
						THEN 1
						-- algum campo preenchido
						ELSE 2
						END)
				  END		
	),

	/*
	    12/06/2013 - Hélio C. Lima
	    Criado mais um "passo" CTE deixando as consultas as functions somente com o resultado a ser exibido
	*/
	tabResult AS (

        --	
	    SELECT	
		      Mtd.alu_id
		    , Mtd.mtu_id
		    , Mtd.mtd_id
		    , tur.tur_id
		    , tur.tur_codigo
		    , alc.alc_matricula
		    , Mtd.tud_id
		    , Atd.atd_id
		    , COALESCE(Caf.caf_avaliacao, Atd.atd_avaliacao, '') as Avaliacao 
		    , Mtd.mtd_resultado
			, COALESCE(Caf.caf_qtAulas, Atd.atd_numeroAulas, 0) as QtAulasAluno		
			, COALESCE(Caf.caf_qtAulasReposicao, Atd.atd_numeroAulasReposicao, 0) AS QtAulasAlunoReposicao
			, COALESCE(Caf.caf_qtFaltas, Atd.atd_numeroFaltas, 0) as QtFaltasAluno
			, COALESCE(Caf.caf_qtFaltasReposicao, Atd.atd_numeroFaltasReposicao, 0) AS QtFaltasAlunoReposicao			
		    , CASE WHEN @documentoOficial = 1 THEN Pes.pes_nomeOficial ELSE Pes.pes_nome END AS pes_nome
            , ISNULL(Mtr.mtd_numeroChamadaDocente, Mtd.mtd_numeroChamada) AS mtd_numeroChamada
		    , ISNULL(Mtr.mtd_situacaoDocente, Mtd.mtd_situacao) AS mtd_situacao
		    , Atd.atd_relatorio
		    , Atd.arq_idRelatorio
			, Atd.atd_numeroAulas AS QtAulasEfetivado
            , ISNULL(Mtr.mtd_dataMatriculaDocente, Mtd.mtd_dataMatricula) AS mtd_dataMatricula
            , ISNULL(Mtr.mtd_dataSaidaDocente, Mtd.mtd_dataSaida) AS mtd_dataSaida
            , COALESCE(Caf.caf_qtAusenciasCompensadas, Atd.atd_ausenciasCompensadas, 0) AS ausenciasCompensadas
            , toc.observacaoPreenchida AS observacaoConselhoPreenchida
            , Atd.atd_avaliacaoPosConselho AS avaliacaoPosConselho
			, COALESCE(Caf.caf_frequencia, Atd.atd_frequencia, 0) AS Frequencia
			, COALESCE(Caf.caf_frequenciaFinalAjustada, Atd.atd_frequenciaFinalAjustada, 100) AS FrequenciaFinalAjustada			
			, mtu.mtu_resultado
	    FROM @Matriculas Mtr
		/**/
		INNER JOIN @tbAlunos tAlu
			ON tAlu.alu_id = Mtr.alu_id
		/**/
		INNER JOIN MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
			ON  Mtr.alu_id = Mtd.alu_id
            AND Mtr.mtu_id = Mtd.mtu_id
			AND Mtr.mtd_id = Mtd.mtd_id
        INNER JOIN MTR_MatriculaTurma mtu -- WITH (NOLOCK)
			ON  mtu.alu_id = Mtd.alu_id
			AND mtu.mtu_id = Mtd.mtu_id
			AND mtu_situacao <> 3
		INNER JOIN TUR_Turma tur -- WITH (NOLOCK)
			ON  tur.tur_id = mtu.tur_id
			AND tur.tur_situacao <> 3
		INNER JOIN ACA_AlunoCurriculo alc -- WITH (NOLOCK)
			ON  alc.alu_id = mtu.alu_id
			AND alc.alc_id = mtu.alc_id
			AND alc.alc_situacao <> 3			
        INNER JOIN ACA_Aluno Alu -- WITH (NOLOCK)
	        ON  Mtd.alu_id   = Alu.alu_id
	        AND alu_situacao <> 3
        INNER JOIN VW_DadosAlunoPessoa Pes -- WITH (NOLOCK)
	        ON  Alu.alu_id   = Pes.alu_id
		LEFT JOIN CLS_AlunoAvaliacaoTurmaDisciplina Atd -- WITH (NOLOCK)
	        ON  Atd.tud_id = Mtd.tud_id
	        AND Atd.alu_id = Mtd.alu_id
	        AND Atd.mtu_id = Mtd.mtu_id
	        AND Atd.mtd_id = Mtd.mtd_id
	        AND Atd.fav_id = @fav_id
	        AND Atd.ava_id = @ava_id
	        AND Atd.atd_situacao <> 3
		LEFT JOIN CLS_AlunoFechamento Caf -- WITH (NOLOCK)
			ON  Caf.tud_id = Mtd.tud_id
			AND Caf.tpc_id = @tpc_id
			AND Caf.alu_id = Mtd.alu_id
			AND Caf.mtu_id = Mtd.mtu_id
			AND Caf.mtd_id = Mtd.mtd_id			
		LEFT JOIN TabelaObservacaoConselho toc
			ON  toc.alu_id = Mtu.alu_id
			AND toc.mtu_id = Mtu.mtu_id
	    WHERE 
	        Mtr.tpc_id = @tpc_id
		    AND ISNULL(Mtr.mtd_situacaoDocente, mtd_situacao) IN (1,5)
		    AND COALESCE(Mtr.mtd_numeroChamadaDocente, mtd_numeroChamada, 0) >= 0
		    AND Alu.alu_situacao <> 3	
	)	
	, tbRetorno AS
	(
		SELECT
			  alu_id
			, mtu_id
			, mtd_id
			, tud_id
			, tur_id
			, tur_codigo
		    , alc_matricula
			, atd_id AS AvaliacaoID
			, Avaliacao
			, CASE 
				-- Caso não seja possível alterar o resultado do aluno e a avaliação for do tipo final ou períodica + final não traz o resultado (ele é calculado na tela)
				WHEN @permiteAlterarResultado = 0 AND @tipoAvaliacao IN (3,5) THEN 
					NULL
				-- Caso contrário, traz o resultado normalmente
				ELSE 
					mtd_resultado
			END 
			AS AvaliacaoResultado	
			, QtAulasAluno	
			, QtAulasAlunoReposicao		        			
			, QtFaltasAluno
			, QtFaltasAlunoReposicao				
			, Res.pes_nome + 
			(
				CASE 
					WHEN ( Res.mtd_situacao = 5 ) THEN 
						ISNULL((SELECT TOP 1 ' (' + tmv_nome + ')' FROM TabelaMovimentacao MOV -- WITH (NOLOCK)
						          WHERE MOV.mtu_idAnterior = Res.mtu_id AND MOV.alu_id = Res.alu_id), ' (Inativo)')
					ELSE 
						'' 
				END
			) 
			AS pes_nome					
			, CASE 
				WHEN Res.mtd_numeroChamada > 0 THEN 
					CAST(Res.mtd_numeroChamada AS VARCHAR)
				ELSE 
					'-' 
			END 
			AS mtd_numeroChamada			
			, Res.mtd_numeroChamada AS mtd_numeroChamadaordem
			, atd_relatorio
			, arq_idRelatorio 
			, Res.mtd_situacao AS situacaoMatriculaAluno
			, Res.mtd_dataMatricula AS dataMatricula
			, Res.mtd_dataSaida AS dataSaida
			, Res.ausenciasCompensadas		
			-- Verifica se há dispensa de disciplina para o aluno.
			, 0 AS dispensadisciplina
            , CAST(ISNULL(observacaoConselhoPreenchida, 0) AS TINYINT) AS observacaoConselhoPreenchida
            , avaliacaoPosConselho
			, Frequencia
			, FrequenciaFinalAjustada
			, mtu_resultado
			, QtAulasEfetivado		
		FROM 
			tabResult AS Res
	)   

	SELECT 
		  alu_id
			, mtu_id
			, mtd_id
			, tud_id
			, tur_id
			, tur_codigo
		    , alc_matricula
			, AvaliacaoID
			, Avaliacao
			, AvaliacaoResultado				
			, QtAulasAluno
			, QtAulasAlunoReposicao	
			, QtFaltasAluno
			, QtFaltasAlunoReposicao
			, pes_nome		
			, mtd_numeroChamada
			, atd_relatorio
			, arq_idRelatorio
			, situacaoMatriculaAluno
			, dataMatricula
			, dataSaida
			, ausenciasCompensadas
			, dispensadisciplina
			, observacaoConselhoPreenchida
			, avaliacaoPosConselho
			, Frequencia
			, FrequenciaFinalAjustada
			, mtu_resultado
			, QtAulasEfetivado
	FROM	
		tbRetorno 
	GROUP BY
		 alu_id
			, mtu_id
			, mtd_id
			, tud_id
			, tur_id
			, tur_codigo
		    , alc_matricula
			, AvaliacaoID
			, Avaliacao
			, AvaliacaoResultado
			, QtAulasAluno	
			, QtAulasAlunoReposicao		
			, QtFaltasAluno		
			, QtFaltasAlunoReposicao
			, pes_nome		
			, mtd_numeroChamada
			, mtd_numeroChamadaordem
			, atd_relatorio
			, arq_idRelatorio
			, situacaoMatriculaAluno
			, dataMatricula
			, dataSaida
			, ausenciasCompensadas
			, dispensadisciplina
			, observacaoConselhoPreenchida
			, avaliacaoPosConselho
			, Frequencia
			, FrequenciaFinalAjustada
			, mtu_resultado
			, QtAulasEfetivado
	ORDER BY 
		CASE 
		    WHEN @ordenacao = 0 THEN 
			    CASE WHEN ISNULL(mtd_numeroChamadaordem,0) <= 0 THEN 1 ELSE 0 END
		END ASC
		, CASE WHEN @ordenacao = 0 THEN ISNULL(mtd_numeroChamadaordem,0) END ASC
		, CASE WHEN @ordenacao = 1 THEN pes_nome END ASC
END



GO
PRINT N'Altering [dbo].[NEW_CLS_AlunoAvaliacaoTurmaDisciplina_SelecionarAlunosTurma]'
GO
-- =============================================
-- Author:		Haila Pelloso
-- Create date: 31/07/2015
-- Description:	Seleciona os alunos da turma para exibir na tela de fechamento do gestor

-- Author:		Carla Frascareli
-- Create date: 24/09/2015
-- Description:	Melhoria de performance - troquei WITH por variável tabela.

---- Alterado: Marcia Haga - 29/12/2015 
---- Description: Adicionado para considerar a justificativa de pendência no fechamento.
-- =============================================
ALTER PROCEDURE [dbo].[NEW_CLS_AlunoAvaliacaoTurmaDisciplina_SelecionarAlunosTurma]
	@tur_id BIGINT
	, @ordenacao BIT
	, @tev_idFechamento INT
	, @documentoOficial BIT
	, @alu_id BIGINT = NULL

AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @dataAtual DATE = GETDATE()

	DECLARE @tbMatriculaTurma TABLE 
	(alu_id BIGINT, mtu_id INT, tur_id BIGINT, mtu_situacao TINYINT, mtu_numeroChamada INT, Ordem INT 
	PRIMARY KEY(alu_id, mtu_id));
	
	INSERT INTO @tbMatriculaTurma
	(alu_id, mtu_id, tur_id, mtu_situacao, mtu_numeroChamada, Ordem)

	--, tbMatriculaTurma AS (
		SELECT 
			Mtu.alu_id, 
			Mtu.mtu_id,
			Mtu.tur_id,
			Mtu.mtu_situacao,
			CASE WHEN (Mtu.mtu_numeroChamada = 0) THEN NULL ELSE Mtu.mtu_numeroChamada END AS mtu_numeroChamada,
			ROW_NUMBER() OVER(PARTITION BY Mtu.alu_id ORDER BY Mtu.mtu_situacao, Mtu.mtu_dataSaida DESC, Mtu.mtu_dataMatricula DESC) AS Ordem
		FROM 
			MTR_MatriculaTurma AS Mtu WITH(NOLOCK)
		WHERE
			Mtu.tur_id = @tur_id
			AND Mtu.mtu_situacao <> 3
			AND (@alu_id IS NULL OR Mtu.alu_id = @alu_id)
	--)

	DECLARE @DadosTurmaPeriodo TABLE
	(tur_id BIGINT, fav_id INT, ava_id INT, cal_id INT, cap_id INT, tpc_id INT, ultimoPeriodo INT, tpc_ordem INT
	, fav_percentualMinimoFrequenciaFinalAjustadaDisciplina DECIMAL(5,2)
	PRIMARY KEY (tur_id, fav_id, ava_id, tpc_id))

	INSERT INTO @DadosTurmaPeriodo
	(tur_id,fav_id,ava_id,cal_id,cap_id,tpc_id,ultimoPeriodo,tpc_ordem,fav_percentualMinimoFrequenciaFinalAjustadaDisciplina)

	--; WITH DadosTurmaPeriodo AS (
		SELECT
			Tur.tur_id,
			Tur.fav_id,
			Ava.ava_id,
			Tur.cal_id,
			Cap.cap_id,
			Cap.tpc_id,
			CASE WHEN (ROW_NUMBER() OVER(ORDER BY Tpc.tpc_ordem DESC) = 1) THEN 1 ELSE 0 END AS ultimoPeriodo,
			Tpc.tpc_ordem,
			Fav.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina
		FROM 
			TUR_Turma AS Tur WITH(NOLOCK)
			INNER JOIN ACA_FormatoAvaliacao AS Fav WITH(NOLOCK)
				ON Tur.fav_id = Fav.fav_id
				AND Fav.fav_situacao <> 3
			INNER JOIN ACA_CalendarioPeriodo AS Cap WITH(NOLOCK)
				ON Cap.cal_id = Tur.cal_id
				AND Cap.cap_situacao <> 3
			INNER JOIN ACA_TipoPeriodoCalendario AS Tpc WITH(NOLOCK)
				ON Cap.tpc_id = Tpc.tpc_id
				AND Tpc.tpc_situacao <> 3
			INNER JOIN ACA_Avaliacao AS Ava WITH(NOLOCK) 
				ON Tur.fav_id = Ava.fav_id
				AND Tpc.tpc_id = Ava.tpc_id
				AND Ava.ava_situacao <> 3
		WHERE
			Tur.tur_id = @tur_id
			--Busca os períodos que já passaram ou que tenha evento de fechamento aberto.
			AND Cap.cap_dataInicio <= @dataAtual
			AND (Cap.cap_dataFim <= @dataAtual
					OR EXISTS (
						SELECT TOP 1 Evt.evt_id
						FROM 
							ACA_Evento AS Evt WITH(NOLOCK)
							INNER JOIN ACA_CalendarioEvento AS CE WITH(NOLOCK)
								ON Evt.evt_id = CE.evt_id
						WHERE
							CE.cal_id = Cap.cal_id
							AND Evt.tpc_id = Cap.tpc_id
							AND Evt.tev_id = @tev_idFechamento
							AND (Evt.evt_padrao = 1 OR Evt.esc_id = Tur.esc_id)
							AND Evt.evt_dataInicio <= @dataAtual 
							AND Evt.evt_dataFim >= @dataAtual 
							AND Evt.evt_situacao <> 3
					)
				)
	--)

	DECLARE @tbMatriculasBoletimTurma TABLE
	(alu_id BIGINT, mtu_id INT, tur_id BIGINT, ExisteMatriculaPeriodo BIT, tpc_id INT, fav_id INT, ava_id INT
	, tpc_ordem INT, mtu_numeroChamada INT, mtu_situacao TINYINT, 
		fav_percentualMinimoFrequenciaFinalAjustadaDisciplina DECIMAL(5,2), PossuiSaidaPeriodo BIT, registroExterno BIT
	PRIMARY KEY (alu_id, mtu_id, tur_id, tpc_id))

	INSERT INTO	@tbMatriculasBoletimTurma
	(alu_id,mtu_id,tur_id,ExisteMatriculaPeriodo,tpc_id,fav_id,ava_id,tpc_ordem,mtu_numeroChamada,
	mtu_situacao,fav_percentualMinimoFrequenciaFinalAjustadaDisciplina,PossuiSaidaPeriodo,registroExterno)
	--; WITH tbMatriculasBoletimTurma AS (
		SELECT
			Mb.alu_id, 
			ISNULL(Mb.mtu_id, Mtu.mtu_id) AS mtu_id,
			ISNULL(Mb.tur_id, Mtu.tur_id) AS tur_id,
			CASE WHEN (Mb.tur_id IS NOT NULL) THEN 1 ELSE 0 END ExisteMatriculaPeriodo,
			Mb.tpc_id,
			TurTpc.fav_id,
			TurTpc.ava_id,
			TurTpc.tpc_ordem,
			Mtu.mtu_numeroChamada,
			Mtu.mtu_situacao,
			TurTpc.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina,
			Mb.PossuiSaidaPeriodo,
			Mb.registroExterno
		FROM @tbMatriculaTurma AS Mtu
		INNER JOIN MTR_MatriculasBoletim AS Mb WITH(NOLOCK)
			ON Mb.alu_id = Mtu.alu_id 
			AND Mb.mtu_origemDados = Mtu.mtu_id
		INNER JOIN @DadosTurmaPeriodo TurTpc
			ON Mb.tpc_id = TurTpc.tpc_id
		WHERE
			Mtu.Ordem = 1
	--)

	DECLARE @tbUltimaMatriculaAlunoNaTurma TABLE
	(alu_id BIGINT, mtu_id INT, mtu_situacao TINYINT, mtu_numeroChamada INT, tpc_id INT, fav_percentualMinimoFrequenciaFinalAjustadaDisciplina DECIMAL(5,2)
	PRIMARY KEY (alu_id, mtu_id))
	INSERT INTO @tbUltimaMatriculaAlunoNaTurma 
	(alu_id, mtu_id, mtu_situacao, mtu_numeroChamada, tpc_id, fav_percentualMinimoFrequenciaFinalAjustadaDisciplina)
	--; WITH tbUltimaMatriculaAlunoNaTurma AS (
		SELECT 
			alu_id, 
			mtu_id,
			mtu_situacao,
			mtu_numeroChamada,
			tpc_id,
			fav_percentualMinimoFrequenciaFinalAjustadaDisciplina
		FROM
			(SELECT
				Mb.alu_id, 
				Mb.mtu_id,
				Mb.mtu_situacao,
				mtu_numeroChamada,
				Mb.tpc_id,
				TurTpc.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina,
				CASE WHEN (ROW_NUMBER() OVER(PARTITION BY Mb.alu_id ORDER BY Mb.alu_id, TurTpc.tpc_ordem DESC) = 1) THEN 1 ELSE 0 END AS ultimo
			FROM 
				@DadosTurmaPeriodo TurTpc
				INNER JOIN @tbMatriculasBoletimTurma AS Mb
					ON TurTpc.tpc_id = Mb.tpc_id
			WHERE Mb.tur_id = @tur_id) AS res
		WHERE
			ultimo = 1
	--)

	DECLARE @TurmaDisciplinaPorMatricula TABLE
	(tur_id BIGINT,alu_id BIGINT,mtu_id INT,fav_id INT,ava_id INT,tpc_id INT,tud_id BIGINT,tud_nome VARCHAR(200),tud_tipo TINYINT,
	tud_naoLancarNota BIT,tud_naoLancarFrequencia BIT,dis_id BIGINT,existeAulaBimestre BIT,tpc_ordem INT
	,fav_percentualMinimoFrequenciaFinalAjustadaDisciplina DECIMAL(5,2),PossuiSaidaPeriodo BIT,ExisteMatriculaPeriodo BIT,registroExterno BIT
	,mtd_resultado TINYINT,mtd_avaliacao VARCHAR(20),
	PRIMARY KEY (tur_id, alu_id, mtu_id, tpc_id, tud_id))

	-- Verifica se existe alguma disciplina do tipo "Experiência" para o aluno
	DECLARE @tabelaExperiencia TABLE (tur_id BIGINT, tud_id BIGINT, cal_id INT)	
	INSERT INTO @tabelaExperiencia(tur_id, tud_id, cal_id)
	SELECT
		Mb.tur_id
		, Tud.tud_id	
		, Tur.cal_id
	FROM @tbMatriculasBoletimTurma Mb
		INNER JOIN TUR_Turma AS Tur WITH(NOLOCK)
			ON Tur.tur_id = Mb.tur_id
			AND Tur.tur_situacao <> 3
		INNER JOIN TUR_TurmaRelTurmaDisciplina AS TurRelTud WITH(NOLOCK)
			ON Mb.tur_id = TurRelTud.tur_id
		INNER JOIN TUR_TurmaDisciplina AS Tud WITH(NOLOCK)
			ON TurRelTud.tud_id = Tud.tud_id 
			AND Tud.tud_tipo = 18
			AND Tud.tud_situacao <> 3 
	GROUP BY
		Mb.tur_id
		, Tud.tud_id	
		, Tur.cal_id

	DECLARE @tabelaExperienciaVigente TABLE (tur_id BIGINT, tud_id BIGINT, cal_id INT, tpc_id INT, Vigente BIT)

	-- Caso exista alguma disciplina do tipo "Experiência"
	-- Verifica se essa disciplina estava vigente em cada período do calendário
	IF ((SELECT COUNT(*) FROM @tabelaExperiencia) > 0)
	BEGIN
		-- Insere na tabela apenas as "Experiências" existentes para o aluno em todos os bimestres
		INSERT INTO @tabelaExperienciaVigente (tur_id, tud_id, cal_id, tpc_id, Vigente)
		SELECT
			ex.tur_id
			, ex.tud_id
			, ex.cal_id
			, cap.tpc_id
			, CAST(0 AS BIT) AS Vigente
		FROM
			@tabelaExperiencia ex
		INNER JOIN ACA_CalendarioPeriodo cap WITH (NOLOCK)
			ON cap.cal_id = ex.cal_id			
			AND cap.cap_situacao <> 3
		GROUP BY
			ex.tur_id
			, ex.tud_id
			, ex.cal_id
			, cap.tpc_id

		-- Verifica se a "Experiência" está vigente em cada um dos bimestres.
		UPDATE tab
		SET Vigente = 1
		FROM
			@tabelaExperienciaVigente tab
		INNER JOIN TUR_TurmaDisciplinaTerritorio tte WITH (NOLOCK)
			ON tte.tud_idExperiencia = tab.tud_id
			AND tte.tte_situacao <> 3
		INNER JOIN ACA_CalendarioPeriodo cap WITH (NOLOCK)
			ON cap.cal_id = tab.cal_id	
			AND cap.tpc_id = tab.tpc_id
			AND cap.cap_situacao <> 3
		WHERE
			-- Marca como vigente, apenas as disciplinas da  turma em que o aluno terminou o ano letivo
			tab.tur_id = @tur_id
			-- E apenas as experiências ativas em cada período do calendário
			AND ( 
				tte.tte_vigenciaInicio BETWEEN cap.cap_dataInicio AND cap.cap_dataFim
				OR tte.tte_vigenciaFim BETWEEN cap.cap_dataInicio AND cap.cap_dataFim
				OR cap.cap_dataInicio BETWEEN tte.tte_vigenciaInicio AND tte.tte_vigenciaFim
				OR cap.cap_dataFim BETWEEN tte.tte_vigenciaInicio AND tte.tte_vigenciaFim
			)			
	END

	;WITH TurmaDisciplinaPeriodo AS (
		SELECT Tud.tud_id, Mb.tpc_id, Tud.tud_tipo 
		FROM @tbMatriculasBoletimTurma Mb
		INNER JOIN TUR_TurmaRelTurmaDisciplina AS TurRelTud WITH(NOLOCK)
			ON Mb.tur_id = TurRelTud.tur_id
		INNER JOIN TUR_TurmaDisciplina AS Tud WITH(NOLOCK)
			ON TurRelTud.tud_id = Tud.tud_id 
			AND Tud.tud_situacao <> 3 
		GROUP BY Tud.tud_id, Mb.tpc_id, Tud.tud_tipo 
	)

	, tbExisteAulasPorTurmaDisciplinaPeriodo AS (
		SELECT 
			Tdp.tud_id,
			Tdp.tpc_id,
			-- Se não for Experiência (Território do Saber), mantem a verificação da existência de aula atual
			CASE WHEN Tdp.tud_tipo != 18 THEN
				CASE WHEN (EXISTS(
							SELECT TOP 1 Tau.tau_id 
							FROM CLS_TurmaAula AS Tau WITH(NOLOCK) 
							WHERE Tau.tud_id = Tdp.tud_id AND Tau.tpc_id = Tdp.tpc_id AND Tau.tau_situacao <> 3))
				THEN 1 ELSE 0 END
			-- Se for Experiência (Território do Saber), faz a verificação da existência de aula de acordo com a vigência da Experiência.
			ELSE
				CASE WHEN 
					-- Caso exista período vigente
					(
						SELECT COUNT(*)
						FROM @tabelaExperienciaVigente tab
						WHERE							
							tab.tud_id = Tdp.tud_id
							AND tab.tpc_id = Tdp.tpc_id
							AND Vigente = 1
					) > 0
					THEN
						-- Verifica se existe aula criada dentro do período de vigência da "Experiência"
						CASE WHEN (EXISTS(
									SELECT TOP 1 Tau.tau_id 
									FROM CLS_TurmaAula AS Tau WITH(NOLOCK)
									INNER JOIN TUR_TurmaDisciplinaTerritorio Tte WITH(NOLOCK)
										ON Tau.tud_id = Tte.tud_idExperiencia
										AND Tte.tte_situacao <> 3
									WHERE
										Tau.tud_id = Tdp.tud_id
										AND Tau.tpc_id = Tdp.tpc_id
										AND Tau.tau_data BETWEEN Tte.tte_vigenciaInicio AND Tte.tte_vigenciaFim
										AND Tau.tau_situacao <> 3										
									))
						THEN 1 ELSE 0 END
					-- Caso não exista período vigente, marca a aula como existente
					ELSE 1 END				
			END AS existeAulaBimestre
		FROM 
			TurmaDisciplinaPeriodo Tdp
	)

	INSERT INTO @TurmaDisciplinaPorMatricula
	(tur_id,alu_id,mtu_id,fav_id,ava_id,tpc_id,tud_id,tud_nome,tud_tipo,tud_naoLancarNota,tud_naoLancarFrequencia,dis_id,existeAulaBimestre,
	tpc_ordem,fav_percentualMinimoFrequenciaFinalAjustadaDisciplina,PossuiSaidaPeriodo,ExisteMatriculaPeriodo,registroExterno,mtd_resultado, mtd_avaliacao)
	--; WITH TurmaDisciplinaPorMatricula AS (
		SELECT
			Mb.tur_id,
			Mb.alu_id,
			Mb.mtu_id,
			Mb.fav_id,
			Mb.ava_id,
			Mb.tpc_id,
			TurRelTud.tud_id,
			Tud.tud_nome,
			Tud.tud_tipo,
			Tud.tud_naoLancarNota,
			Tud.tud_naoLancarFrequencia,
			TudRelDis.dis_id,
			CASE WHEN (EXISTS(
						SELECT TOP 1 1
						FROM tbExisteAulasPorTurmaDisciplinaPeriodo AS Tau
						WHERE Tau.tud_id = Tud.tud_id AND Tau.tpc_id = Mb.tpc_id AND existeAulaBimestre = 1))
				THEN 1 ELSE 0 END existeAulaBimestre,
			Mb.tpc_ordem,
			Mb.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina,
			Mb.PossuiSaidaPeriodo,
			Mb.ExisteMatriculaPeriodo,
			Mb.registroExterno,
			Mtd.mtd_resultado,
			Mtd.mtd_avaliacao
		FROM 
			@tbMatriculasBoletimTurma Mb
			INNER JOIN TUR_TurmaRelTurmaDisciplina AS TurRelTud WITH(NOLOCK)
				ON Mb.tur_id = TurRelTud.tur_id
			INNER JOIN TUR_TurmaDisciplina AS Tud WITH(NOLOCK)
				ON TurRelTud.tud_id = Tud.tud_id 
				AND Tud.tud_situacao <> 3
			INNER JOIN TUR_TurmaDisciplinaRelDisciplina AS TudRelDis WITH(NOLOCK)
				ON Tud.tud_id = TudRelDis.tud_id
			INNER JOIN MTR_MatriculaTurmaDisciplina AS Mtd WITH(NOLOCK)
				ON Mtd.alu_id = Mb.alu_id
				AND Mtd.mtu_id = Mb.mtu_id
				AND Mtd.tud_id = Tud.tud_id
				AND Mtd.mtd_situacao <> 3
		WHERE tud.tud_tipo <> 18 OR Mb.tur_id = @tur_id
		GROUP BY
			Mb.tur_id,
			Mb.alu_id,
			Mb.mtu_id,
			Mb.fav_id,
			Mb.ava_id,
			Mb.tpc_id,
			TurRelTud.tud_id,
			Tud.tud_nome,
			Tud.tud_tipo,
			Tud.tud_naoLancarNota,
			Tud.tud_naoLancarFrequencia,
			TudRelDis.dis_id,
			Tud.tud_id,
			Mb.tpc_id,
			Mb.tpc_ordem,
			Mb.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina,
			Mb.PossuiSaidaPeriodo,
			Mb.ExisteMatriculaPeriodo,
			Mb.registroExterno,
			Mtd.mtd_resultado,
			Mtd.mtd_avaliacao
	--)

	DECLARE @tbDadosFechamento TABLE
	(tur_id BIGINT,tud_id BIGINT, tud_nome VARCHAR(200),tud_tipo TINYINT,alu_id BIGINT, mtu_id INT, mtd_id INT, atd_id INT
	, fav_id INT, ava_id INT, dis_id BIGINT, avaliacao VARCHAR(50), frequenciaFinalAjustada DECIMAL(5,2)
	,qtAulas INT,tud_naoLancarNota BIT,tud_naoLancarFrequencia BIT,existeAulaBimestre BIT,
	tpc_ordem INT,fav_percentualMinimoFrequenciaFinalAjustadaDisciplina DECIMAL(5,2),PossuiSaidaPeriodo BIT,ExisteMatriculaPeriodo BIT
	,registroExterno BIT, PossuiJustificativaPendencia BIT, mtd_resultado TINYINT, mtd_avaliacao VARCHAR(20),
	PRIMARY KEY (tur_id, tud_id, alu_id, mtu_id, fav_id, ava_id)
	)

	INSERT INTO @tbDadosFechamento
	(tur_id,tud_id, tud_nome,tud_tipo,alu_id, mtu_id, Atd.mtd_id, Atd.atd_id, fav_id, ava_id, 
	dis_id,avaliacao, frequenciaFinalAjustada,qtAulas,tud_naoLancarNota,tud_naoLancarFrequencia,existeAulaBimestre,
	tpc_ordem,fav_percentualMinimoFrequenciaFinalAjustadaDisciplina,PossuiSaidaPeriodo,ExisteMatriculaPeriodo,registroExterno,PossuiJustificativaPendencia,mtd_resultado, mtd_avaliacao
	)
	--; WITH tbDadosFechamento AS (
		SELECT
			Mb.tur_id,
			Mb.tud_id, 
			Mb.tud_nome,
			Mb.tud_tipo,
			Mb.alu_id, 
			Mb.mtu_id, 
			Atd.mtd_id, 
			Atd.atd_id, 
			Mb.fav_id, 
			Mb.ava_id, 
			Mb.dis_id,
			COALESCE(Atd.atd_avaliacaoPosConselho, Caf.caf_avaliacao, Atd.atd_avaliacao) AS avaliacao, 
			ISNULL(Caf.caf_frequenciaFinalAjustada, Atd.atd_frequenciaFinalAjustada) AS frequenciaFinalAjustada,
			COALESCE(Caf.caf_qtAulas, Atd.atd_numeroAulas, 0) AS qtAulas,
			Mb.tud_naoLancarNota,
			Mb.tud_naoLancarFrequencia,
			Mb.existeAulaBimestre,
			Mb.tpc_ordem,
			Mb.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina,
			Mb.PossuiSaidaPeriodo,
			Mb.ExisteMatriculaPeriodo,
			Mb.registroExterno,
			CASE WHEN Fjp.fjp_id IS NULL THEN 0 ELSE 1 END, --AS PossuiJustificativaPendencia
			Mb.mtd_resultado,
			Mb.mtd_avaliacao
		FROM 
			@TurmaDisciplinaPorMatricula AS Mb
			INNER JOIN TUR_Turma AS Tur WITH(NOLOCK)
				ON Tur.tur_id = Mb.tur_id
			LEFT JOIN CLS_AlunoFechamento AS Caf WITH(NOLOCK)
				ON Mb.tud_id = Caf.tud_id
				AND Mb.tpc_id = Caf.tpc_id
				AND Mb.alu_id = Caf.alu_id
				AND Mb.mtu_id = Caf.mtu_id
			LEFT JOIN CLS_AlunoAvaliacaoTurmaDisciplina AS Atd WITH(NOLOCK)
				ON Mb.tud_id = Atd.tud_id
				AND Mb.alu_id = Atd.alu_id
				AND Mb.mtu_id = Atd.mtu_id
				AND Mb.fav_id = Atd.fav_id
				AND Mb.ava_id = Atd.ava_id
				AND Atd.atd_situacao <> 3
			LEFT JOIN CLS_FechamentoJustificativaPendencia AS Fjp WITH(NOLOCK)
				ON Fjp.tud_id = Mb.tud_id
				AND Fjp.cal_id = Tur.cal_id
				AND Fjp.tpc_id = Mb.tpc_id
				AND Fjp.fjp_situacao <> 3
	--)

	DECLARE @TabelaMovimentacao TABLE
	(alu_id BIGINT, mtu_idAnterior INT, tmv_nome VARCHAR(200), exibeCompl BIT, Posicao INT
	PRIMARY KEY (alu_id, mtu_idAnterior, Posicao))

	INSERT INTO @TabelaMovimentacao
	(alu_id, mtu_idAnterior, tmv_nome, exibeCompl, Posicao)
	--; WITH TabelaMovimentacao AS (
		--Selecina as movimentações que possuem matrícula anterior
		SELECT
			Mov.alu_id,
			Mov.mtu_idAnterior,
			CASE WHEN tmo_tipoMovimento IN (6, 12, 14, 15, 16)
					THEN TMV.tmv_nome + ' em ' + REPLACE(CONVERT(VARCHAR(10), mov.mov_dataRealizacao, 103), '/' + CAST(DATEPART(YEAR, mov.mov_dataRealizacao) AS VARCHAR(4)), '') +
					 	ISNULL(' p/ ' + turD.tur_codigo, '')
					WHEN tmo_tipoMovimento IN (8)
					THEN TMV.tmv_nome + ' p/' + ISNULL(' ' + turD.tur_codigo, '')
					WHEN tmo_tipoMovimento IN (11)
					THEN TMV.tmv_nome + ' p/ ' + ISNULL(' ' + turD.tur_codigo, '')
					ELSE TMV.tmv_nome
			END tmv_nome,
			CASE WHEN tmo_tipoMovimento IN (6, 12, 14, 15, 16)
					THEN 1
					WHEN tmo_tipoMovimento IN (8)
					THEN 1
					WHEN tmo_tipoMovimento IN (11)
					THEN 1
					ELSE 0
			END AS exibeCompl,
			ROW_NUMBER() OVER(PARTITION BY Mov.alu_id, Mov.mtu_idAnterior ORDER BY Mov.alu_id, Mov.mtu_idAnterior) AS Posicao
		FROM
			@tbMatriculasBoletimTurma AS Alu
			INNER JOIN MTR_Movimentacao Mov WITH(NOLOCK)
				ON Mov.alu_id = Alu.alu_id
			INNER JOIN ACA_TipoMovimentacao Tmv WITH(NOLOCK)
				ON Mov.tmv_idSaida = Tmv.tmv_id
			LEFT JOIN MTR_TipoMovimentacao tmo WITH(NOLOCK)
				ON mov.tmo_id = tmo.tmo_id
				AND tmo.tmo_situacao <> 3
			LEFT JOIN MTR_MatriculaTurma mtuD WITH(NOLOCK)
				ON mov.alu_id = mtuD.alu_id
				AND mov.mtu_idAtual = mtuD.mtu_id
			LEFT JOIN TUR_Turma turD WITH(NOLOCK)
				ON mtuD.tur_id = turD.tur_id
			LEFT JOIN ACA_CalendarioAnual calD WITH(NOLOCK)
				ON turD.cal_id = calD.cal_id
			INNER JOIN MTR_MatriculaTurma mtuO WITH(NOLOCK)
				ON mov.alu_id = mtuO.alu_id
				AND mov.mtu_idAnterior = mtuO.mtu_id
				AND mtuO.tur_id = @tur_id
			LEFT JOIN TUR_Turma turO WITH(NOLOCK)
				ON mtuO.tur_id = turO.tur_id
			LEFT JOIN ACA_CalendarioAnual calO WITH(NOLOCK)
				ON turO.cal_id = calO.cal_id
		WHERE
			mov_situacao NOT IN (3,4)
			AND tmv_situacao <> 3
			AND mtu_idAnterior IS NOT NULL	
	--)

	DECLARE @Pendencia TABLE
	(alu_id BIGINT, tud_nome VARCHAR(200)
	PRIMARY KEY (alu_id, tud_nome))

	INSERT INTO @Pendencia(alu_id, tud_nome)
	--; WITH Pendencia AS (
		SELECT Atd.alu_id, Atd.tud_nome
		FROM @tbDadosFechamento Atd
		LEFT JOIN ACA_Avaliacao ava WITH(NOLOCK)
			ON Atd.fav_id = ava.fav_id
			AND Atd.ava_id = ava.ava_id
		WHERE 
			Atd.tud_tipo NOT IN (11, 17, 19) --Regencia / Docencia compartilhada
			AND ((Atd.tud_naoLancarNota = 0 AND tud_tipo <> 10 AND (ISNULL(Atd.avaliacao, '') = '' OR (mtd_avaliacao IS NULL AND ava_tipo IN (3,5))))
				OR ((Atd.tud_naoLancarNota = 1 OR (tud_tipo = 10 AND Atd.PossuiJustificativaPendencia = 0)) AND (Atd.existeAulaBimestre = 0 OR (mtd_resultado IS NULL AND ava_tipo IN (3,5)))))
			--Verifica pendencias apenas na turma
			AND Atd.tur_id = @tur_id
			AND Atd.PossuiSaidaPeriodo = 0
			AND Atd.ExisteMatriculaPeriodo = 1
			AND Atd.registroExterno = 0
		GROUP BY Atd.alu_id, Atd.tud_nome
	--)

	DECLARE @tbDadosFechamentoBaixaFrequencia TABLE
	(alu_id BIGINT,tud_id BIGINT,tud_nome VARCHAR(200),tud_naoLancarFrequencia BIT,tud_tipo TINYINT,frequenciaFinalAjustada DECIMAL(5,2),
	fav_percentualMinimoFrequenciaFinalAjustadaDisciplina DECIMAL(5,2),existeAulaBimestre BIT,qtAulas INT,
	ultimoLancamento BIT, mtu_id INT, mtd_id INT, atd_id INT
	PRIMARY KEY (tud_id, alu_id, mtu_id, mtd_id, atd_id))

	INSERT INTO @tbDadosFechamentoBaixaFrequencia
	(alu_id,tud_id,tud_nome,tud_naoLancarFrequencia,tud_tipo,frequenciaFinalAjustada,
	fav_percentualMinimoFrequenciaFinalAjustadaDisciplina,existeAulaBimestre,qtAulas,ultimoLancamento
	, mtu_id, mtd_id, atd_id)
	--;WITH tbDadosFechamentoBaixaFrequencia AS (
		SELECT 
			Atd.alu_id,
			Atd.tud_id,
			Atd.tud_nome,
			Atd.tud_naoLancarFrequencia,
			Atd.tud_tipo,
			Atd.frequenciaFinalAjustada,
			Atd.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina,
			Atd.existeAulaBimestre,
			Atd.qtAulas,
			CAST(CASE WHEN (ROW_NUMBER() OVER(PARTITION BY Atd.dis_id, Atd.alu_id ORDER BY Atd.tpc_ordem DESC) = 1) THEN 1 ELSE 0 END AS BIT) AS ultimoLancamento
			, mtu_id, mtd_id, atd_id
		FROM @tbDadosFechamento Atd
		WHERE Atd.atd_id IS NOT NULL
		AND Atd.qtAulas > 0
	--)

	DECLARE @BaixaFrequencia TABLE
	(alu_id BIGINT PRIMARY KEY)

	INSERT INTO @BaixaFrequencia (alu_id)
	--, BaixaFrequencia AS (
		SELECT Atd.alu_id
		FROM @tbDadosFechamentoBaixaFrequencia Atd
		WHERE 
			Atd.ultimoLancamento = 1
			AND Atd.atd_id > 0
			--Não precisa verificar pois só traz bimestres que a frequência final já foi calculada
			--AND Atd.existeAulaBimestre = 1
			--AND Atd.qtAulas > 0
			AND Atd.tud_naoLancarFrequencia = 0
			AND Atd.tud_tipo NOT IN (12, 13) --Componentes de regencia / Complementaçao da regencia
			AND ISNULL(Atd.frequenciaFinalAjustada, 100) < Atd.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina
		GROUP BY
			Atd.alu_id
	--)

	SELECT
		Mb.alu_id,
		Mb.mtu_id,
		Mb.tpc_id AS tpc_idUltimoPeriodoLancamento,
		Mb.mtu_numeroChamada,
		CASE WHEN @documentoOficial = 1 THEN Pes.pes_nomeOficial ELSE Pes.pes_nome END AS pes_nome, 
		(ISNULL(CAST(Mb.mtu_numeroChamada AS VARCHAR(MAX)) + ' - ', '') 
				+ CASE WHEN @documentoOficial = 1 THEN Pes.pes_nomeOficial ELSE Pes.pes_nome END 
				+ (CASE WHEN ( Mb.mtu_situacao = 5 AND exibeCompl = 1  ) THEN ISNULL(' (' + tmv_nome + ')', ' (Inativo)') ELSE '' END)
			) AS pes_nome_infoCompl,
		(ISNULL(CAST(Mb.mtu_numeroChamada AS VARCHAR(MAX)) + ' - ', '') 
				+ CASE WHEN @documentoOficial = 1 THEN Pes.pes_nomeOficial ELSE Pes.pes_nome END 
				+ (CASE WHEN ( Mb.mtu_situacao = 5) THEN ISNULL(' (' + tmv_nome + ')', ' (Inativo)') ELSE '' END)
			) AS nomeFormatado,
		CAST(CASE WHEN (Mb.mtu_situacao = 5) THEN 1 ELSE 0 END AS BIT) inativo,
		CAST(CASE WHEN (Bf.alu_id IS NOT NULL) THEN 1 ELSE 0 END AS BIT) baixaFrequencia,
		CAST(CASE WHEN EXISTS (SELECT TOP 1 P.alu_id FROM @Pendencia AS P WHERE Alu.alu_id = P.alu_id)
			THEN 1 ELSE 0 END AS BIT) pendencia,
		STUFF(( SELECT ', ' + P.tud_nome
				FROM @Pendencia P
				WHERE Alu.alu_id = P.alu_id 
				GROUP BY P.tud_nome
				ORDER BY P.tud_nome FOR XML PATH ('') ) ,1,2,'') AS DisciplinaPendencia
	FROM 
		@tbUltimaMatriculaAlunoNaTurma Mb
		INNER JOIN ACA_Aluno AS Alu WITH(NOLOCK)
			ON Mb.alu_id = Alu.alu_id
		INNER JOIN VW_DadosAlunoPessoa AS Pes 
			ON Alu.alu_id = Pes.alu_id
		LEFT JOIN @TabelaMovimentacao Mov
			ON Mov.alu_id = Mb.alu_id 
			AND Mov.mtu_idAnterior = Mb.mtu_id
			AND Mov.posicao = 1
		LEFT JOIN @BaixaFrequencia AS Bf
			ON Mb.alu_id = Bf.alu_id
	ORDER BY 
		CASE 
			WHEN @ordenacao = 0 THEN 
				CASE WHEN ISNULL(Mb.mtu_numeroChamada, 0) <= 0 THEN 1 ELSE 0 END
		END ASC
		, CASE WHEN @ordenacao = 0 THEN ISNULL(Mb.mtu_numeroChamada, 0) END ASC
		, CASE WHEN @ordenacao = 0 AND ISNULL(Mb.mtu_numeroChamada, 0) <= 0 THEN CASE WHEN @documentoOficial = 1 THEN Pes.pes_nomeOficial ELSE Pes.pes_nome END END ASC
		, CASE WHEN @ordenacao = 1 THEN CASE WHEN @documentoOficial = 1 THEN Pes.pes_nomeOficial ELSE Pes.pes_nome END END ASC

END
GO
PRINT N'Altering [dbo].[MS_JOB_RelatorioDisciplinasAlunosPendencias]'
GO
-- =============================================
-- Author:		Carla Frascareli
-- Create date: 20/04/2015
-- Description:	Gera uma tabela com alunos, e dados do fechamento por Dre/Escola/Calendário/Bimestre

-- Alterado: Jean Michel - 26/07/2016
-- Description: Alterado para considerar as experiências do território

---- Alterado: Marcia Haga - 30/03/2017
---- Description: Alterado para não indicar pendências em bimestres que ainda não foram abertos.
-- =============================================
ALTER PROCEDURE [dbo].[MS_JOB_RelatorioDisciplinasAlunosPendencias]
	@uad_idSuperiorGestao UNIQUEIDENTIFIER
	,@esc_id BIGINT
	,@cal_ano INT
	,@tpc_id INT
	,@tur_ids VARCHAR(MAX)
	,@tud_ids VARCHAR(MAX)
	,@ProcessaFila BIT = 0
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @filtroTurmas TABLE (tur_id BIGINT NOT NULL PRIMARY KEY);
	DECLARE @filtroTurmaDisciplina TABLE (tud_id BIGINT NOT NULL PRIMARY KEY);

	INSERT INTO @filtroTurmaDisciplina (tud_id)
	SELECT valor FROM dbo.FN_StringToArrayInt64(@tud_ids, ',')

	IF (@tur_ids IS NOT NULL)
	BEGIN
		INSERT INTO @filtroTurmas (tur_id)
		SELECT valor FROM dbo.FN_StringToArrayInt64(@tur_ids, ',')
	END
	ELSE IF (@tud_ids IS NOT NULL)
	BEGIN
		-- Buscar as turmas daquelas disciplinas para filtrar antes.
		INSERT INTO @filtroTurmas (tur_id)
		SELECT tur_id
		FROM TUR_TurmaRelTurmaDisciplina RelTud WITH(NOLOCK)
		INNER JOIN @filtroTurmaDisciplina Tud
			ON Tud.tud_id = RelTud.tud_id
		GROUP BY tur_id
	END

	IF (@cal_ano IS NULL AND (@tur_ids IS NOT NULL OR @tud_ids IS NOT NULL))
	BEGIN
		SELECT TOP 1 @cal_ano = cal.cal_ano
		FROM @filtroTurmas rel
		INNER JOIN TUR_Turma tur WITH(NOLOCK)
			ON rel.tur_id = tur.tur_id
		INNER JOIN ACA_CalendarioAnual cal WITH(NOLOCK)
			ON tur.cal_id = cal.cal_id
	END
	
	IF(@ProcessaFila = 0)
	BEGIN
		--Insere os componentes da regência se o tud_id de regência estiver no filtro
		INSERT INTO @filtroTurmaDisciplina (tud_id)
		SELECT TudC.tud_id FROM @filtroTurmaDisciplina fTud
		INNER JOIN TUR_TurmaDisciplina Tud WITH(NOLOCK)
			ON fTud.tud_id = Tud.tud_id
			AND Tud.tud_tipo = 11 --Regênca
			AND Tud.tud_situacao <> 3
		INNER JOIN TUR_TurmaRelTurmaDisciplina RelTud WITH(NOLOCK)
			ON Tud.tud_id = RelTud.tud_id
		INNER JOIN TUR_TurmaRelTurmaDisciplina RelTudC WITH(NOLOCK)
			ON RelTud.tur_id = RelTudC.tur_id
		INNER JOIN TUR_TurmaDisciplina TudC WITH(NOLOCK)
			ON RelTudC.tud_id = TudC.tud_id
			AND TudC.tud_tipo = 12 --Componente da Regênca
			AND TudC.tud_situacao <> 3
	END

	-- Data final do periodo
	DECLARE @cap_dataInicio DATE, @cap_dataFim DATE, @tpc_nome VARCHAR(200), @cal_id INT;
	
	SELECT 
		TOP 1 
		@cap_dataInicio = cap_dataInicio
		, @cap_dataFim = cap_dataFim
		, @tpc_nome = tpc_nome
		, @cal_id = Cal.cal_id
	FROM ACA_CalendarioAnual Cal WITH(NOLOCK)
	INNER JOIN ACA_CalendarioPeriodo Cap WITH(NOLOCK) 
		ON Cap.cal_id = Cal.cal_id
	INNER JOIN ACA_TipoPeriodoCalendario Tpc WITH(NOLOCK)
		ON Tpc.tpc_id = Cap.tpc_id
	WHERE 
		cal_ano = @cal_ano
		AND Cap.tpc_id = @tpc_id 
		AND cap_situacao <> 3

	DECLARE @EscolasTurmas AS TABLE 
	(
		esc_id BIGINT NOT NULL,
		tur_id BIGINT NOT NULL,
		tur_codigo VARCHAR(30),
		cal_id INT NOT NULL,
		fav_id INT NOT NULL,
		ava_id INT NOT NULL,
		tpc_id INT NOT NULL,
		tur_tipo TINYINT NOT NULL,
		aberto BIT NOT NULL
	);

	DECLARE @dataAtual DATE = CAST(GETDATE() AS DATE)

	DECLARE @tev_EfetivacaoNotas INT = 
	(
		SELECT TOP 1 CAST(pac.pac_valor AS INT)
		FROM ACA_ParametroAcademico pac WITH(NOLOCK)
		WHERE pac.pac_situacao <> 3 AND pac.pac_chave = 'TIPO_EVENTO_EFETIVACAO_NOTAS'
	);

	; WITH Escolas AS 
	(
		SELECT
			Esc.esc_id, @tpc_id AS tpc_id
		FROM ESC_Escola Esc WITH(NOLOCK)
		INNER JOIN Synonym_SYS_UnidadeAdministrativa uad WITH(NOLOCK)
			ON esc.ent_id = uad.ent_id
			and esc.uad_id = uad.uad_id	
		WHERE
			Esc.esc_situacao <> 3
			AND Esc.esc_controleSistema = 1
			AND (@uad_idSuperiorGestao IS NULL OR ISNULL(Esc.uad_idSuperiorGestao, uad.uad_idSuperior) = @uad_idSuperiorGestao)
			AND (Esc.esc_id = ISNULL(@esc_id, Esc.esc_id))
	)

	INSERT INTO @EscolasTurmas (esc_id, tur_id, tur_codigo, cal_id, fav_id, ava_id, tpc_id, tur_tipo, aberto)
	SELECT
		Esc.esc_id, tur_id, tur_codigo, Tur.cal_id, Tur.fav_id, ava_id, @tpc_id, Tur.tur_tipo,
		CASE WHEN
		-- Só indicar pendência se é um bimestre que já foi aberto para fechamento.
		EXISTS
		(
			SELECT TOP 1 1 
			FROM ACA_Evento evt WITH(NOLOCK)
			INNER JOIN ACA_CalendarioEvento cae WITH(NOLOCK)
				ON cae.evt_id = evt.evt_id
				AND cae.cal_id = Tur.cal_id
			WHERE evt.tev_id = @tev_EfetivacaoNotas
			AND evt.tpc_id = @tpc_id
			AND evt.evt_dataInicio <= @dataAtual
			AND 
			(
				evt.evt_padrao = 1
				OR evt.esc_id = Tur.esc_id
			)
			AND evt.evt_situacao <> 3
		) THEN 1 ELSE 0 END
	FROM Escolas Esc
	INNER JOIN TUR_Turma AS Tur WITH ( NOLOCK )
		ON Tur.esc_id = esc.esc_id
		AND Tur.cal_id = @cal_id
		AND Tur.tur_situacao <> 3
		-- Só turmas normais e eletivas (recuperação).
		AND Tur.tur_tipo IN (1,2,3)
	INNER JOIN ACA_Avaliacao Ava WITH(NOLOCK)
		ON Ava.fav_id = Tur.fav_id
		AND Ava.ava_tipo IN (1,5)
		AND Ava.tpc_id = @tpc_id
		AND Ava.ava_situacao <> 3
	WHERE
		-- Quando não passa o filtro traz todas as turmas da escola.
		@tur_ids IS NULL AND @tud_ids IS NULL
	UNION
	SELECT
		Esc.esc_id, Tur.tur_id, tur_codigo, Tur.cal_id, Tur.fav_id, ava_id, @tpc_id, Tur.tur_tipo,
		CASE WHEN
		-- Só indicar pendência se é um bimestre que já foi aberto para fechamento.
		EXISTS
		(
			SELECT TOP 1 1 
			FROM ACA_Evento evt WITH(NOLOCK)
			INNER JOIN ACA_CalendarioEvento cae WITH(NOLOCK)
				ON cae.evt_id = evt.evt_id
				AND cae.cal_id = Tur.cal_id
			WHERE evt.tev_id = @tev_EfetivacaoNotas
			AND evt.tpc_id = @tpc_id
			AND evt.evt_dataInicio <= @dataAtual
			AND 
			(
				evt.evt_padrao = 1
				OR evt.esc_id = Tur.esc_id
			)
			AND evt.evt_situacao <> 3
		) THEN 1 ELSE 0 END
	FROM Escolas Esc
	INNER JOIN TUR_Turma AS Tur WITH ( NOLOCK )
		ON Tur.esc_id = esc.esc_id
		AND Tur.cal_id = @cal_id
		AND Tur.tur_situacao <> 3
		-- Só turmas normais e eletivas (recuperação).
		AND Tur.tur_tipo IN (1,2,3)
	-- Filtra as turmas informadas.
	INNER JOIN @filtroTurmas T
		ON T.tur_id = Tur.tur_id
	INNER JOIN ACA_Avaliacao Ava WITH(NOLOCK)
		ON Ava.fav_id = Tur.fav_id
		AND Ava.ava_tipo IN (1,5)
		AND Ava.tpc_id = @tpc_id
		AND Ava.ava_situacao <> 3
		
	--SELECT * FROM @EscolasTurmas

	DECLARE @MatriculaTurma AS TABLE
	(alu_id BIGINT NOT NULL, mtu_id INT NOT NULL, tur_id BIGINT NOT NULL, aberto BIT NOT NULL
	PRIMARY KEY (alu_id, mtu_id, tur_id))

	INSERT INTO @MatriculaTurma (alu_id, mtu_id, tur_id, aberto)
	SELECT  
		Mtu.alu_id, MAX(Mtu.mtu_id) AS mtu_id, Tur.tur_id, Tur.aberto
	FROM @EscolasTurmas Tur
	INNER JOIN MTR_MatriculaTurma Mtu WITH(NOLOCK)
		ON Mtu.tur_id = Tur.tur_id
		AND Mtu.mtu_situacao <> 3
	INNER JOIN ACA_Aluno alu WITH(NOLOCK)
		ON Mtu.alu_id = alu.alu_id
		AND alu.alu_situacao <> 3
	WHERE Tur.tur_tipo = 1 --Apenas turmas normais.
	GROUP BY Mtu.alu_id, Tur.tur_id, Tur.aberto
	
	--select * from @MatriculaTurma

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
		DisciplinaSemAula BIT DEFAULT(0),
		aberto BIT NOT NULL
		PRIMARY KEY (tur_id, tud_id, alu_id, mtu_id, mtd_id, tpc_id)
	);

	INSERT INTO @AlunosDisciplinasPendencias
	(alu_id, mtu_id, mtd_id, tur_id, tud_id, tud_naoLancarNota, tpc_id, tud_nome, tud_tipo, tur_tipo, aberto)
	SELECT
		Mtr.alu_id, Mtr.mtu_id, Mtd.mtd_id, Mtr.tur_id, Mtd.tud_id, ISNULL(tud_naoLancarNota, 0), tpc_id, tud_nome, tud_tipo, Tur.tur_tipo, Mtu.aberto
	FROM @MatriculaTurma Mtu
	INNER JOIN TUR_Turma Tur WITH(NOLOCK)
		ON Mtu.tur_id = Tur.tur_id
		AND Tur.tur_situacao <> 3
	INNER JOIN MTR_MatriculasBoletim Mtr WITH(NOLOCK)
		ON Mtr.alu_id = Mtu.alu_id
		AND Mtr.mtu_origemDados = Mtu.mtu_id
		AND Mtr.tur_id = Mtu.tur_id
		AND Mtr.tpc_id = @tpc_id
		AND Mtr.PossuiSaidaPeriodo = 0
		AND Mtr.registroExterno = 0
	-- Pegar tud_id e mtd_id pelo mtu_id, para buscar as EFs.
	INNER JOIN MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
		ON Mtd.alu_id = Mtr.alu_id
		AND Mtd.mtu_id = Mtr.mtu_id
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
	WHERE
		(
			-- Filtra as disciplinas informadas.
			@tud_ids IS NULL OR
			EXISTS (SELECT tud_id FROM @filtroTurmaDisciplina T WHERE Tud.tud_id = T.tud_id)
		)

	INSERT INTO @AlunosDisciplinasPendencias
	(alu_id, mtu_id, mtd_id, tur_id, tud_id, tud_naoLancarNota, tpc_id, tud_nome, tud_tipo, tur_tipo, aberto)
	SELECT
		Mtd.alu_id, Mtd.mtu_id, Mtd.mtd_id, Tur.tur_id, Mtd.tud_id, ISNULL(tud_naoLancarNota, 0), tpc_id, tud_nome, tud_tipo, Tur.tur_tipo, Tur.aberto
	FROM @EscolasTurmas Tur
	INNER JOIN TUR_TurmaRelTurmaDisciplina Trt WITH(NOLOCK)
		ON Tur.tur_id = Trt.tur_id
	INNER JOIN TUR_TurmaDisciplina Tud WITH(NOLOCK)
		ON Trt.tud_id = Tud.tud_id
		AND Tud.tud_situacao <> 3
		-- Não trazer 11-Regência - só verifica nota nos seus componentes.
		-- Não trazer 17-Compartilhada.		
		-- Não trazer 19-Territorio - só verifica disciplina sem aula na experiência
		AND Tud.tud_tipo NOT IN (11, 17, 19)
	INNER JOIN MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
		ON Mtd.tud_id = Tud.tud_id
		AND Mtd.mtd_situacao <> 3
		AND Mtd.mtd_dataMatricula <= @cap_dataFim
		AND (Mtd.mtd_dataSaida IS NULL OR Mtd.mtd_dataSaida > @cap_dataFim)
	INNER JOIN ACA_Aluno alu WITH(NOLOCK)
		ON Mtd.alu_id = alu.alu_id
		AND alu.alu_situacao <> 3
	WHERE Tur.tur_tipo in (2,3) --Apenas turmas eletivas (recuperação).
		  AND
		  (
			-- Filtra as disciplinas informadas.
			@tud_ids IS NULL OR
			EXISTS (SELECT tud_id FROM @filtroTurmaDisciplina T WHERE Tud.tud_id = T.tud_id)
		  )	

	--select * from @AlunosDisciplinasPendencias order by alu_id
	
	--select * from @AlunosDisciplinasPendencias where tud_id in (588592,675837,675838)

	; WITH AlunosComNota AS
	(
		-- Alunos com lançamento de nota ok.
		SELECT Mtr.tud_id, Mtr.tur_id, Mtr.alu_id, Mtr.mtu_id, Mtr.mtd_id, Mtr.tpc_id
		FROM @AlunosDisciplinasPendencias Mtr
		INNER JOIN @EscolasTurmas Tur
			ON Tur.tur_id = Mtr.tur_id
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

	-- Só marca pendência se o período de fechamento do bimestre já esteve aberto.
	UPDATE @AlunosDisciplinasPendencias
	SET SemNota = 0
	WHERE aberto = 0

	-- Não marca pendência de nota para as disciplinas que não lançam nota.
	UPDATE @AlunosDisciplinasPendencias
	SET SemNota = 0
	WHERE tud_naoLancarNota = 1

	-- Pegar alunos sem parecer conclusivo se for o último bimestre.
	IF (@tpc_id = 4)
	BEGIN
		UPDATE @AlunosDisciplinasPendencias
		SET SemParecer = 1
		FROM @AlunosDisciplinasPendencias P
		INNER JOIN MTR_MatriculaTurma Mtu WITH(NOLOCK)
			ON P.alu_id = Mtu.alu_id
			AND P.mtu_id = Mtu.mtu_id
			AND P.tpc_id = @tpc_id
			AND Mtu.mtu_resultado IS NULL
			AND P.tur_tipo = 1
		-- Só marca pendência se o período de fechamento do bimestre já esteve aberto.
		WHERE P.aberto = 1

		-- Para os casos em que não é "Experiência" (Território do Saber), mantem a verificação atual
		UPDATE @AlunosDisciplinasPendencias
		SET SemResultadoFinal = 1
		FROM @AlunosDisciplinasPendencias P
		INNER JOIN MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
			ON P.alu_id = Mtd.alu_id
			AND P.mtu_id = Mtd.mtu_id
			AND P.tud_id = Mtd.tud_id
			AND P.tpc_id = @tpc_id
			AND Mtd.mtd_situacao <> 3
			AND Mtd.mtd_resultado IS NULL
		WHERE
			-- Só verifica se nao tem resultado na disciplina(Sintese final) se for "nao lançar nota" ou eletiva de aluno.
			(P.tud_naoLancarNota = 1 OR P.tud_tipo = 10)
			-- Regra padrão, exceto para "Experiência" (Território do Saber)
			AND P.tud_tipo != 18
			-- Só marca pendência se o período de fechamento do bimestre já esteve aberto.
			AND P.aberto = 1

		-- Para os casos em que é "Experiência" (Território do Saber)
		-- Só marca pendencia de resultado final, quando a experiência é oferecida no último período do calendário
		UPDATE @AlunosDisciplinasPendencias
		SET SemResultadoFinal = 1
		FROM @AlunosDisciplinasPendencias P
		INNER JOIN MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
			ON P.alu_id = Mtd.alu_id
			AND P.mtu_id = Mtd.mtu_id
			AND P.tud_id = Mtd.tud_id
			AND P.tpc_id = @tpc_id
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
			-- Só marca pendência se o período de fechamento do bimestre já esteve aberto.
			AND P.aberto = 1

		-- Pedro Silva 20/08
		-- neste caso do campo SemSintese, estou usando a lógica contrária: 
		-- Seto todos para 1, e coloco 0 apenas nos q tiverem registros. 
		-- Fiz isto para evitar o left join e ter melhor performance
		UPDATE @AlunosDisciplinasPendencias
		SET SemSintese = 1
		FROM @AlunosDisciplinasPendencias P
		WHERE P.tpc_id = @tpc_id 
		AND P.tud_naoLancarNota = 0 AND P.tud_tipo <> 10
		-- Só marca pendência se o período de fechamento do bimestre já esteve aberto.
		AND P.aberto = 1

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
		WHERE P.tpc_id = @tpc_id 
			-- Só verifica se nao tem avaliação na disciplina(Sintese final) se for "nao lançar nota" = 1 e não for eletiva de aluno.
		  AND P.tud_naoLancarNota = 0 AND P.tud_tipo <> 10

		UPDATE @AlunosDisciplinasPendencias
		SET SemSintese = 0
		FROM @AlunosDisciplinasPendencias P
		WHERE P.tpc_id = @tpc_id AND P.tud_naoLancarNota = 1
		  
	END

	-- Pegar disciplinas que não lançam nota e ver se elas tem aula (é obrigatório ter uma aula para não ter pendência).
	-- Verificação padrão. Vale para todas as disciplinas exceto para Experiência (Território do Saber)
	; WITH DisciplinasNaoLancarNota AS
	(
		SELECT
			Tud.tud_id
		FROM @AlunosDisciplinasPendencias Tud
		WHERE 
			Tud.tud_naoLancarNota = 1
			-- Tudo que não lança nota exceto Experiência (Território do Saber)
			AND tud.tud_tipo <> 18
		GROUP BY Tud.tud_id
	)
	, DisciplinasSemAula AS
	(
		SELECT 
			tud_id
		FROM DisciplinasNaoLancarNota Tud
		WHERE
			NOT EXISTS
				(
					SELECT TOP 1 1
					FROM CLS_TurmaAula Tau WITH(NOLOCK)
					WHERE
						Tau.tud_id = Tud.tud_id
						AND Tau.tpc_id = @tpc_id
						AND Tau.tau_situacao <> 3
				)
	)
	UPDATE @AlunosDisciplinasPendencias
	SET DisciplinaSemAula = 1
	FROM DisciplinasSemAula Tud
	INNER JOIN @AlunosDisciplinasPendencias P
		ON P.tud_id = Tud.tud_id
		-- Só marca pendência se o período de fechamento do bimestre já esteve aberto.
		AND P.aberto = 1

	-- Pegar disciplinas que não lançam nota e ver se elas tem aula (é obrigatório ter uma aula para não ter pendência).
	-- Verificação específica para Experiência (Território do Saber)
	; WITH DisciplinasNaoLancarNotaExp AS
	(
		SELECT
			tud.tud_id

			  -- Recupera a maior data entre a data inicial do período do calendário e a data inicial da experiência
			, CASE WHEN tte.tte_vigenciaInicio < @cap_dataInicio 
				THEN @cap_dataInicio
				ELSE tte.tte_vigenciaInicio
			  END AS VigenciaInicial

			  -- Recupera a menor data entre a data final do período do calendário e a data final da experiência
			, CASE WHEN tte.tte_vigenciaFim > @cap_dataFim
				THEN @cap_dataFim
				ELSE tte.tte_vigenciaFim
			  END AS VigenciaFinal
		FROM
			@AlunosDisciplinasPendencias tud
		INNER JOIN TUR_TurmaDisciplinaTerritorio tte WITH (NOLOCK)
			ON tte.tud_idExperiencia = tud.tud_id
			-- Apenas experiências ativas no período do calendário informado			
			AND @cap_dataFim >= tte.tte_vigenciaInicio
			AND @cap_dataInicio <= ISNULL(tte.tte_vigenciaFim, @cap_dataInicio)
			AND tte.tte_situacao <> 3
		WHERE 
			tud.tud_naoLancarNota = 1
			-- Apenas para Experiência (Território do Saber)
			AND tud.tud_tipo = 18
		GROUP BY
			Tud.tud_id
			, tte.tte_vigenciaInicio
			, tte.tte_vigenciaFim
	)
	, DisciplinaSemAulaVigenciaExp AS
	(		
		SELECT 
			tud_id
				-- Verifica se existe aula criada para cada período de vigência de cada experiência dentro do período do calendário
			, 	ISNULL((
					SELECT TOP 1 1
					FROM CLS_TurmaAula Tau WITH(NOLOCK)
					WHERE
						Tau.tud_id = Tud.tud_id
						AND Tau.tpc_id = @tpc_id
						AND Tau.tau_data BETWEEN Tud.VigenciaInicial and Tud.VigenciaFinal
						AND Tau.tau_situacao <> 3
				),0) AS AulaCriada
		FROM 
			DisciplinasNaoLancarNotaExp Tud
	)
	, DisciplinasSemAulaExp AS
	(
		-- Retorna apenas as disciplinas que não possuem aula criada em nenhum dos períodos de vigência da experiência dentro do período do calendário
		SELECT 
			Tud.tud_id
		FROM
			DisciplinaSemAulaVigenciaExp Tud
		GROUP BY
			Tud.tud_id
		HAVING 
			SUM(Tud.AulaCriada) = 0
	)
	UPDATE @AlunosDisciplinasPendencias
	SET DisciplinaSemAula = 1
	FROM DisciplinasSemAulaExp Tud
	INNER JOIN @AlunosDisciplinasPendencias P
		ON P.tud_id = Tud.tud_id
		-- Só marca pendência se o período de fechamento do bimestre já esteve aberto.
		AND P.aberto = 1

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

	IF (EXISTS (SELECT TOP 1 1 FROM @filtroTurmaDisciplina))
	BEGIN
		PRINT 'disc'
		-- Exclui os dados pras disciplinas filtradas.
		DELETE R FROM [REL_AlunosSituacaoFechamento] R
		INNER JOIN @filtroTurmaDisciplina T ON R.tud_id = T.tud_id
		WHERE R.tpc_id = @tpc_id

		UPDATE tsf
		SET Pendente = CAST(0 AS BIT),
			PendenteParecer = CAST(0 AS BIT),
			DataProcessamento = @data
		FROM
			REL_TurmaDisciplinaSituacaoFechamento tsf WITH(NOLOCK)
			INNER JOIN @filtroTurmaDisciplina T
				ON T.tud_id = tsf.tud_id
		WHERE
			tsf.tpc_id = @tpc_id
	END
	ELSE IF (EXISTS (SELECT TOP 1 1 FROM @filtroTurmas))
	BEGIN
		PRINT 'turma'
		-- Exclui os dados pras turmas filtradas.
		DELETE R FROM [REL_AlunosSituacaoFechamento] R
		INNER JOIN @filtroTurmas T ON R.tur_id = T.tur_id
		WHERE R.tpc_id = @tpc_id

		UPDATE tsf
		SET Pendente = CAST(0 AS BIT),
			PendenteParecer = CAST(0 AS BIT),
			DataProcessamento = @data
		FROM
			REL_TurmaDisciplinaSituacaoFechamento tsf WITH(NOLOCK)
			INNER JOIN TUR_TurmaRelTurmaDisciplina rel WITH(NOLOCK)
				ON rel.tud_id = tsf.tud_id
			INNER JOIN @filtroTurmas T
				ON T.tur_id = rel.tur_id
		WHERE
			tsf.tpc_id = @tpc_id
	END
	ELSE IF (@esc_id IS NOT NULL)
	BEGIN
		PRINT 'esc'
		-- Exclui os dados daquela escola.
		DELETE FROM [REL_AlunosSituacaoFechamento] WHERE esc_id = @esc_id and tpc_id = @tpc_id

		UPDATE REL_TurmaDisciplinaSituacaoFechamento
		SET Pendente = CAST(0 AS BIT),
			PendenteParecer = CAST(0 AS BIT),
			DataProcessamento = @data
		WHERE esc_id = @esc_id AND tpc_id = @tpc_id
	END
	ELSE IF (@uad_idSuperiorGestao IS NOT NULL)
	BEGIN
		PRINT 'uad'
		-- Exclui os dados daquela DRE.
		DELETE FROM [REL_AlunosSituacaoFechamento] WHERE [uad_idSuperior] = @uad_idSuperiorGestao and tpc_id = @tpc_id

		UPDATE tsf
		SET Pendente = CAST(0 AS BIT),
			PendenteParecer = CAST(0 AS BIT),
			DataProcessamento = @data
		FROM
			REL_TurmaDisciplinaSituacaoFechamento tsf WITH(NOLOCK)
			INNER JOIN ESC_Escola esc WITH(NOLOCK)
				ON tsf.esc_id = esc.esc_id
				AND esc.esc_situacao <> 3
			INNER JOIN Synonym_SYS_UnidadeAdministrativa uad WITH(NOLOCK)
				ON uad.ent_id = esc.ent_id
				AND uad.uad_id = esc.uad_id
				AND uad.uad_situacao <> 3
			INNER JOIN Synonym_SYS_UnidadeAdministrativa uadSuperior WITH(NOLOCK)
				ON uadSuperior.ent_id = uad.ent_id
				AND uadSuperior.uad_id = ISNULL(esc.uad_idSuperiorGestao, uad.uad_idSuperior)
				AND uadSuperior.uad_id = @uad_idSuperiorGestao
				AND uadSuperior.uad_situacao <> 3
		WHERE
			tpc_id = @tpc_id
	END

	INSERT INTO [dbo].[REL_AlunosSituacaoFechamento]
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
	select
		UadSuperior.uad_nome AS DRE
		, Esc.esc_nome AS Escola
		, Tci.tci_nome AS Ciclo
		, Crp.crp_descricao AS Serie
		, EscTur.tur_codigo AS Turma
		, @cal_ano AS AnoLetivo
		, @tpc_nome AS Bimestre
		, PES.pes_nome AS Aluno
		, Tabela.tud_nome AS Disciplina
		, UadSuperior.uad_id AS uad_idSuperior
		, Esc.esc_id
		, Tci.tci_id
		, Crp.cur_id
		, Crp.crr_id
		, Crp.crp_id
		, EscTur.tur_id
		, @cal_id AS cal_id
		, @tpc_id AS tpc_id
		, Alu.alu_id
		, Mtu.mtu_id
		, Tabela.mtd_id
		, Tabela.tud_id
		, Tabela.tud_tipo

		, Tabela.SemNota
		, Tabela.SemSintese
		, Tabela.SemResultadoFinal
		, Tabela.SemParecer
		, Tabela.DisciplinaSemAula
		
		, @data as [DataRegistro]
	from @AlunosDisciplinasPendencias Tabela
	INNER JOIN @EscolasTurmas EscTur
		ON EscTur.tur_id = Tabela.tur_id
	INNER JOIN ESC_Escola Esc WITH(NOLOCK)
		ON Esc.esc_id = EscTur.esc_id
	INNER JOIN Synonym_SYS_UnidadeAdministrativa uad WITH(NOLOCK)
		ON esc.uad_id = uad.uad_id
		AND uad.uad_situacao <> 3
	LEFT JOIN Synonym_SYS_UnidadeAdministrativa UadSuperior  WITH(NOLOCK)
		ON UadSuperior.ent_id = Esc.ent_id
		AND UadSuperior.uad_id = ISNULL(Esc.uad_idSuperiorGestao, uad.uad_id)
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

	IF (@ProcessaFila = 1)
	BEGIN	
		DELETE AFP 
		FROM 
			CLS_AlunoFechamentoPendencia as AFP
			INNER JOIN @filtroTurmaDisciplina AS ftd
			ON AFP.tud_id = ftd.tud_id
			AND AFP.tpc_id = @tpc_id
		WHERE
			AFP.afp_processado = 3;

	END

	DECLARE @PendenciaTurmaDisciplina TABLE
	(
		tud_id BIGINT,
		tpc_id INT,
		esc_id INT,
		cal_id INT,
		Pendente BIT,
		PendenteParecer BIT,
		PRIMARY KEY (tud_id, tpc_id),
		UNIQUE CLUSTERED (tud_id, tpc_id)
	);

	INSERT INTO @PendenciaTurmaDisciplina
	(
		tud_id,
		tpc_id,
		esc_id,
		cal_id,
		Pendente,
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
				SUM(CAST(pend.SemParecer AS INT)) > 0 
					THEN 1 
					ELSE 0 
			  END AS BIT) AS PendenteParecer
	FROM 
		@AlunosDisciplinasPendencias pend
		INNER JOIN @EscolasTurmas est
			ON est.tur_id = pend.tur_id
	GROUP BY 
		pend.tud_id,
		pend.tpc_id,
		est.esc_id,
		est.cal_id

	MERGE REL_TurmaDisciplinaSituacaoFechamento WITH (HOLDLOCK) AS Destino 
	USING @PendenciaTurmaDisciplina AS Origem
	ON Destino.tud_id = Origem.tud_id
	   AND Destino.tpc_id = Origem.tpc_id
	WHEN MATCHED THEN
		UPDATE SET Pendente = Origem.Pendente,
				   PendenteParecer = Origem.PendenteParecer,
				   DataProcessamento = @data
	WHEN NOT MATCHED THEN
		INSERT (tud_id, tpc_id, esc_id, cal_id, Pendente, PendenteParecer, DataProcessamento)
		VALUES (Origem.tud_id, Origem.tpc_id, Origem.esc_id, Origem.cal_id, Origem.Pendente, Origem.PendenteParecer, @data);
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

---- Alterado: Marcia Haga - 27/03/2017
---- Description: Alterado para retornar o número de aulas dadas,
---- para registros com divergência entre aulas dadas e aulas previstas.
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
	
	DECLARE @dataAtual DATE = CAST(GETDATE() AS DATE)
			, @tud_tipo TINYINT
			, @ttn_tipo TINYINT
			, @fav_fechamentoAutomatico BIT
			, @esc_id INT;
	
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
		, @esc_id = Tur.esc_id
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

	DECLARE @tev_idEfetivacaoNotas INT = 
	(
		SELECT TOP 1 CAST(pac.pac_valor AS INT)
		FROM ACA_ParametroAcademico pac WITH(NOLOCK)
		WHERE pac.pac_situacao <> 3 AND pac.pac_chave = 'TIPO_EVENTO_EFETIVACAO_NOTAS'
	);

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
			, Div.AulasDadas AS aulasCriadas
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
		LEFT JOIN REL_DivergenciaAulasPrevistas Div WITH(NOLOCK)
			ON Div.tud_id = @tud_id
			AND Div.tpc_id = Cap.tpc_id
			AND
			(
				-- Só exibir a divergência se for o bimestre atual
				(
					@dataAtual >= cap.cap_dataInicio
					AND @dataAtual <= cap.cap_dataFim
				)
				OR
				-- ou se é um bimestre que já foi fechado.
				EXISTS
				(
					SELECT TOP 1 1 
					FROM ACA_Evento evt WITH(NOLOCK)
					INNER JOIN ACA_CalendarioEvento cae WITH(NOLOCK)
						ON cae.evt_id = evt.evt_id
						AND cae.cal_id = cap.cal_id
					WHERE evt.tev_id = @tev_idEfetivacaoNotas
					AND evt.tpc_id = div.tpc_id
					AND evt.evt_dataInicio <= @dataAtual
					AND 
					(
						evt.evt_padrao = 1
						OR evt.esc_id = @esc_id
					)
					AND evt.evt_situacao <> 3
				)
			)
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
			, NULL AS aulasCriadas
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
			, Div.AulasDadas AS aulasCriadas
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
		LEFT JOIN REL_DivergenciaAulasPrevistas Div WITH(NOLOCK)
			ON Div.tud_id = @tud_id
			AND Div.tpc_id = Cap.tpc_id
			AND
			(
				-- Só exibir a divergência se for o bimestre atual
				(
					@dataAtual >= cap.cap_dataInicio
					AND @dataAtual <= cap.cap_dataFim
				)
				OR
				-- ou se é um bimestre que já foi fechado.
				EXISTS
				(
					SELECT TOP 1 1 
					FROM ACA_Evento evt WITH(NOLOCK)
					INNER JOIN ACA_CalendarioEvento cae WITH(NOLOCK)
						ON cae.evt_id = evt.evt_id
						AND cae.cal_id = cap.cal_id
					WHERE evt.tev_id = @tev_idEfetivacaoNotas
					AND evt.tpc_id = div.tpc_id
					AND evt.evt_dataInicio <= @dataAtual
					AND 
					(
						evt.evt_padrao = 1
						OR evt.esc_id = @esc_id
					)
					AND evt.evt_situacao <> 3
				)
			)
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
PRINT N'Creating [dbo].[NEW_Relatorio_DocDctRelJustificativaFalta]'
GO
-- ===========================================================================
-- Author:		Leticia Goes
-- Create date: 28/03/2017
-- Description: Procedure para o relatório de São Paulo de justificativas de falta
-- ===========================================================================
CREATE PROCEDURE [dbo].[NEW_Relatorio_DocDctRelJustificativaFalta]
	@esc_id INT 
	, @uni_id INT
	, @cal_id INT
	, @cur_id INT
	, @crr_id INT
	, @crp_id INT
	, @tpc_id INT
	, @cap_id INT
	, @tur_id BIGINT
	, @mostraCodigoEscola BIT
	, @documentoOficial BIT
	, @tjf_id INT
AS
BEGIN
	SET NOCOUNT ON;
	
	-- Massa de alunos matricula turma
	DECLARE @MatriculasBoletim   TipoTabela_MatriculasBoletim;	
	DECLARE @AlunoMatriculaTurma TipoTabela_AlunoMatriculaTurma;	
	
	-- Massa de dados inicial
	DECLARE @MatriculasTurma TABLE (
							   tur_id BIGINT NOT NULL
							   , alu_id BIGINT NOT NULL
							   , mtu_id INT NOT NULL
							   , mtd_id INT NOT NULL							   
							   , tpc_id INT NOT NULL					   
							   , tud_id BIGINT NOT NULL								   
							   , fav_id INT
							   , cal_id INT
							   , cap_dataInicio DATETIME NOT NULL
							   , cap_dataFim DATETIME NOT NULL
							   , PRIMARY KEY (tur_id, alu_id, mtu_id, mtd_id, tpc_id));

	-- Inserção das matriculas mediante aos filtros	
	INSERT INTO @AlunoMatriculaTurma (
		alu_id
		, mtu_id)					
	SELECT
		mtu.alu_id
		, MAX(mtu.mtu_id)
	FROM TUR_Turma tur WITH(NOLOCK)
		INNER JOIN TUR_TurmaCurriculo tcr WITH(NOLOCK) 
			ON tur.tur_id = tcr.tur_id
			AND tcr.cur_id = @cur_id
			AND tcr.crr_id = @crr_id
			AND tcr.crp_id = @crp_id
			AND tcr.tcr_situacao <> 3
		INNER JOIN MTR_MatriculaTurma mtu WITH(NOLOCK)
			ON tur.tur_id = mtu.tur_id
			AND mtu.mtu_situacao <> 3
	WHERE tur.tur_id = ISNULL(@tur_id, tur.tur_id)
		AND tur.esc_id = @esc_id
		AND tur.uni_id = @uni_id
		AND tur.cal_id = @cal_id			
		AND tur.tur_tipo = 1		
		AND tur.tur_situacao <> 3
	GROUP BY 
		mtu.alu_id
		
	INSERT INTO @MatriculasBoletim (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
	EXEC NEW_MTR_MatriculaTurma_MatriculasBoletim_Alunos
		@tbMatriculaTurma = @AlunoMatriculaTurma

	INSERT INTO @MatriculasTurma (
		tur_id
		, alu_id
		, mtu_id
		, mtd_id
		, tpc_id
		, tud_id
		, fav_id
		, cal_id
		, cap_dataInicio
		, cap_dataFim)
	SELECT matBol.tur_id
		, matBol.alu_id
		, matBol.mtu_id
		, mtd.mtd_id AS mtd_id
		, matBol.tpc_id
		, mtd.tud_id AS tud_id
		, tur.fav_id	
		, tur.cal_id	
		, cap.cap_dataInicio
		, cap.cap_dataFim
	FROM @MatriculasBoletim matBol
		INNER JOIN @AlunoMatriculaTurma amt 
			ON amt.alu_id = matBol.alu_id
			AND amt.mtu_id = matBol.mtu_id
		INNER JOIN TUR_Turma tur WITH(NOLOCK)
			ON matBol.tur_id = tur.tur_id
			AND tur.esc_id = @esc_id
			AND tur.uni_id = @uni_id
			AND tur.tur_situacao <> 3
		INNER JOIN MTR_MatriculaTurma mtu WITH(NOLOCK)		
			ON matBol.alu_id = mtu.alu_id
			AND matBol.mtu_id = mtu.mtu_id
			AND matBol.tur_id = mtu.tur_id
			AND mtu.cur_id = @cur_id
			AND mtu.crr_id = @crr_id
			AND mtu.crp_id = @crp_id
			AND mtu.mtu_situacao <> 3
		INNER JOIN MTR_MatriculaTurmaDisciplina mtd WITH(NOLOCK)
			ON mtu.alu_id = mtd.alu_id
			AND mtu.mtu_id = mtd.mtu_id
			AND mtd.mtd_situacao <> 3
		INNER JOIN TUR_TurmaDisciplina tud WITH(NOLOCK)
			ON mtd.tud_id = tud.tud_id
			AND tud.tud_tipo NOT IN (10, 12, 13,19)
			AND tud.tud_situacao <> 3		
		INNER JOIN ACA_CalendarioPeriodo cap WITH(NOLOCK)
			ON cap.cal_id = tur.cal_id
			AND cap.tpc_id = matBol.tpc_id
			AND cap.cap_situacao <> 3
	WHERE matBol.tur_id = ISNULL(@tur_id, matBol.tur_id)
		AND matBol.cal_id = @cal_id
		AND matBol.cap_id = ISNULL(@cap_id, matBol.cap_id)
		AND matBol.tpc_id = ISNULL(@tpc_id, matBol.tpc_id)
		AND
		(
			-- Ou o tipo de disciplina é diferente de Experiência (Território do Saber)
			tud.tud_tipo NOT IN (18)
			OR
			(
				-- Ou o tipo de disciplina é Experiência (Território do Saber)
				-- e precisa estar dentro do período de matrícula do aluno
				tud.tud_tipo = 18				
				AND EXISTS
				(
					SELECT TOP 1 tte.tud_idExperiencia
					FROM TUR_TurmaDisciplinaTerritorio tte WITH (NOLOCK)
					WHERE
						tte.tud_idExperiencia = Tud.tud_id	
						-- Apenas experiências ativas durante a matrícula do aluno	
						AND
						( 
							tte.tte_vigenciaInicio BETWEEN Mtd.mtd_dataMatricula AND ISNULL(Mtd.mtd_dataSaida, Cap.cap_dataFim)
							OR tte.tte_vigenciaFim BETWEEN Mtd.mtd_dataMatricula AND ISNULL(Mtd.mtd_dataSaida, Cap.cap_dataFim)
							OR Mtd.mtd_dataMatricula BETWEEN tte.tte_vigenciaInicio AND tte.tte_vigenciaFim
							OR ISNULL(Mtd.mtd_dataSaida, Cap.cap_dataFim) BETWEEN tte.tte_vigenciaInicio AND tte.tte_vigenciaFim
						)
				)					
			)
		)

	SELECT 
		tur.tur_id
		,tur.tur_codigo
		,alu.alu_id
		,CASE WHEN @documentoOficial = 1 THEN pes.pes_nomeOficial ELSE pes.pes_nome END AS pes_nome	
		,jus.afj_id
		,jus.afj_dataInicio
		,jus.afj_dataFim
		,tip.tjf_nome 
		,jus.afj_observacao
		,(CASE WHEN @mostraCodigoEscola = 1 AND ESC.esc_codigo IS NOT NULL THEN ESC.esc_codigo + ' - ' ELSE '' END + ESC.esc_nome) AS Escola
		, CRE.uad_nome AS DRE
		, ISNULL(TCI.tci_nome,'') + ' - ' + tur.tur_codigo + ' - ' + 
			CASE WHEN @tpc_id IS NULL THEN 'ANUAL' ELSE (SELECT tpc_nome FROM ACA_TipoPeriodoCalendario WITH(NOLOCK) WHERE tpc_id = @tpc_id)
		    END AS CicloTurBim
	FROM @MatriculasTurma res
		INNER JOIN ACA_AlunoJustificativaFalta jus WITH(NOLOCK) 
			ON jus.alu_id = res.alu_id
		INNER JOIN ACA_TipoJustificativaFalta tip WITH(NOLOCK) 
			ON jus.tjf_id = tip.tjf_id
		INNER JOIN TUR_Turma tur WITH(NOLOCK) 
			ON res.tur_id = tur.tur_id 
			AND tur.tur_situacao <> 3			
		INNER JOIN MTR_MatriculaTurmaDisciplina mtd WITH(NOLOCK) 
			ON res.alu_id = mtd.alu_id 
			AND res.mtu_id = mtd.mtu_id 
			AND res.mtd_id = mtd.mtd_id 
			AND mtd.mtd_situacao <> 3
		INNER JOIN ACA_Aluno alu WITH(NOLOCK) 
			ON mtd.alu_id = alu.alu_id 
			AND alu.alu_situacao <> 3
		INNER JOIN VW_DadosAlunoPessoa pes 
			ON alu.alu_id = pes.alu_id
		INNER JOIN ESC_Escola ESC WITH(NOLOCK) 
			ON ESC.esc_id = tur.esc_id 
			AND ESC.esc_situacao <> 3
		INNER JOIN Synonym_SYS_UnidadeAdministrativa UA WITH (NOLOCK) 
			ON UA.ent_id = ESC.ent_id  
			AND UA.uad_id = ESC.uad_id 
			AND UA.uad_situacao <> 3
		LEFT JOIN Synonym_SYS_UnidadeAdministrativa CRE WITH (NOLOCK) 
			ON CRE.uad_id = ISNULL(uad_idSuperiorGestao, UA.uad_idSuperior) 
			AND CRE.uad_situacao <> 3
		INNER JOIN TUR_TurmaCurriculo tcr WITH(NOLOCK) 
			ON res.tur_id = tcr.tur_id 
			AND tcr.tcr_situacao <> 3 
		INNER JOIN ACA_Curso cur WITH(NOLOCK) 
			ON tcr.cur_id = cur.cur_id  
			AND cur.cur_situacao <> 3
		INNER JOIN ACA_Curriculo crr WITH(NOLOCK) 
			ON cur.cur_id = crr.cur_id 
			AND tcr.crr_id = crr.crr_id 
			AND crr.crr_situacao <> 3
		INNER JOIN ACA_CurriculoPeriodo crp WITH(NOLOCK) 
			ON crr.cur_id = crp.cur_id 
			AND crr.crr_id = crp.crr_id 
			AND tcr.crp_id = crp.crp_id 
			AND crp.crp_situacao <> 3
		LEFT JOIN ACA_TipoCiclo tci WITH(NOLOCK) 
			ON CRP.tci_id = tci.tci_id 
			AND tci.tci_situacao <> 3
	WHERE 
		jus.afj_situacao <> 3 
		AND (@tpc_id IS NULL OR (jus.afj_dataInicio <= res.cap_dataFim 
								 AND jus.afj_dataFim >= res.cap_dataInicio))
		AND jus.tjf_id = ISNULL(@tjf_id, jus.tjf_id)
	GROUP BY
		tur.tur_id
		,tur.tur_codigo
		,alu.alu_id
		,pes.pes_nomeOficial
		,pes.pes_nome
		,jus.afj_id
		,jus.afj_dataInicio
		,jus.afj_dataFim
		,tip.tjf_nome 
		,jus.afj_observacao
		,ESC.esc_codigo
		,ESC.esc_nome
		,CRE.uad_nome
		,TCI.tci_nome
		,tur.tur_codigo
	ORDER BY
		tur.tur_codigo
		, CASE WHEN @documentoOficial = 1 THEN pes.pes_nomeOficial ELSE pes.pes_nome END
END
GO
PRINT N'Creating [dbo].[MS_JOB_ProcessamentoDivergenciasAulasPrevistas]'
GO
-- =============================================
-- Author:		Marcia Haga
-- Create date: 27/03/2017
-- Description:	Processa as divergências entre registros de aulas dadas e aulas previstas.
-- =============================================
CREATE PROCEDURE [dbo].[MS_JOB_ProcessamentoDivergenciasAulasPrevistas]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Verifica se a StoredProcedure será executada
	DECLARE @exec BIT
			, @ser_id INT = 54

	SELECT 
		@exec = ISNULL(ser_ativo, 0)
	FROM SYS_Servicos WITH (NOLOCK) 
	WHERE ser_nomeProcedimento = 'MS_JOB_ProcessamentoDivergenciasAulasPrevistas'

	IF (@exec = 1)
	BEGIN
		DECLARE @ultimaExecucao DATETIME
		DECLARE @dataAtual DATETIME = GETDATE();

		SELECT @ultimaExecucao = MAX(sle_dataInicioExecucao) FROM SYS_ServicosLogExecucao WITH(NOLOCK) 
		WHERE ser_id = @ser_id AND sle_dataFimExecucao IS NOT NULL

		IF (@ultimaExecucao IS NULL)
		BEGIN
			SET @ultimaExecucao = '2014-01-01 00:00:00.000'
		END

		DECLARE @Atualizar TABLE
		(
			tud_id BIGINT
			, tud_tipo TINYINT
			, tpc_id INT
			, aulasPrevistas INT
			, atualizarAulasDadas BIT
			, aulasDadas INT
			, PRIMARY KEY (tud_id, tpc_id)
		)

		DECLARE @SemDivergencia TABLE
		(
			tud_id BIGINT
			, tpc_id INT
			, PRIMARY KEY (tud_id, tpc_id)
		)

		-- Verificar se teve aula criada, alterada ou excluída, 
		-- ou se teve alteração de aulas previstas,
		-- depois da data da última execução do serviço.
		INSERT INTO @Atualizar
		SELECT tap.tud_id, tud.tud_tipo, tap.tpc_id, tap.tap_aulasPrevitas, CASE WHEN (div.tud_id IS NULL OR COUNT(tau.tau_id) > 0) THEN 1 ELSE 0 END, NULL
		FROM TUR_TurmaDisciplinaAulaPrevista tap WITH(NOLOCK)
		INNER JOIN TUR_TurmaDisciplina tud WITH(NOLOCK)
			ON tud.tud_id = tap.tud_id
			AND tud.tud_tipo NOT IN (17,18,19)
			AND tud.tud_situacao <> 3
		LEFT JOIN REL_DivergenciaAulasPrevistas div WITH(NOLOCK)
			ON div.tud_id = tap.tud_id
			AND div.tpc_id = tap.tpc_id
		LEFT JOIN CLS_TurmaAula tau WITH(NOLOCK)
			ON tau.tud_id = tap.tud_id
			AND tau.tpc_id = tap.tpc_id
			AND tau.tau_dataAlteracao > @ultimaExecucao
		WHERE
		tap.tap_dataAlteracao > @ultimaExecucao
		OR tau.tau_id IS NOT NULL
		GROUP BY tap.tud_id, tud.tud_tipo, tap.tpc_id, tap.tap_aulasPrevitas, div.tud_id

		-- Calcular o número de aulas dadas para cada tud_id e tpc_id.

		-- Contagem diferenciada para Regência Integral 
		;WITH AulasRegencia AS
		(
			SELECT atu.tud_id, atu.tpc_id, tau.tau_data
				, CASE 
						WHEN (SUM(ISNULL(tau.tau_numeroAulas, 0)) = 0) THEN 0
						WHEN (SUM(ISNULL(tau.tau_numeroAulas, 0)) = 1 OR SUM(ISNULL(tau.tau_numeroAulas, 0)) = 2) THEN 1 
						ELSE 2
					END AS tau_numeroAulas
			FROM @Atualizar atu
			INNER JOIN TUR_TurmaRelTurmaDisciplina relTud WITH(NOLOCK)
				ON relTud.tud_id = atu.tud_id
			INNER JOIN TUR_Turma tur WITH(NOLOCK)
				ON tur.tur_id = relTud.tur_id
			INNER JOIN ACA_Turno trn WITH(NOLOCK)
				ON trn.trn_id = tur.trn_id
				AND Trn.trn_situacao <> 3
			INNER JOIN ACA_TipoTurno ttn WITH(NOLOCK)
				ON ttn.ttn_id = trn.ttn_id
				AND ttn.ttn_tipo = 4 -- Integral
				AND ttn.ttn_situacao <> 3
			LEFT JOIN CLS_TurmaAula tau WITH(NOLOCK)
				ON tau.tud_id = atu.tud_id
				AND tau.tpc_id = atu.tpc_id
				AND tau.tau_situacao <> 3		
			WHERE 
			tud_tipo = 11 -- Regência
			AND atualizarAulasDadas = 1	
			GROUP BY
			atu.tud_id, atu.tpc_id, tau.tau_data	
		)
		, SomaAulas AS
		(
			SELECT tud_id, tpc_id, SUM(tau_numeroAulas) AS tau_numeroAulas
			FROM AulasRegencia
			GROUP BY tud_id, tpc_id
		)
		UPDATE @Atualizar
		SET aulasDadas = tau.tau_numeroAulas
		FROM @Atualizar atu 
		INNER JOIN SomaAulas tau
			ON tau.tud_id = atu.tud_id
			AND tau.tpc_id = atu.tpc_id

		-- Contagem padrão
		;WITH SomaAulas AS
		(
			SELECT atu.tud_id, atu.tpc_id, SUM(ISNULL(tau.tau_numeroAulas, 0)) AS tau_numeroAulas
			FROM @Atualizar atu
			LEFT JOIN CLS_TurmaAula tau WITH(NOLOCK)
				ON tau.tud_id = atu.tud_id
				AND tau.tpc_id = atu.tpc_id
				AND tau.tau_situacao <> 3
			WHERE
			atualizarAulasDadas = 1
			AND aulasDadas IS NULL
			GROUP BY atu.tud_id, atu.tpc_id
		)
		UPDATE @Atualizar
		SET aulasDadas = tau.tau_numeroAulas
		FROM @Atualizar atu 
		INNER JOIN SomaAulas tau
			ON tau.tud_id = atu.tud_id
			AND tau.tpc_id = atu.tpc_id

		-- Salvar as divergências em uma tabela.

		INSERT INTO @SemDivergencia (tud_id, tpc_id)
		SELECT tud_id, tpc_id
		FROM @Atualizar
		WHERE atualizarAulasDadas = 1 AND aulasDadas = aulasPrevistas
		UNION
		SELECT atu.tud_id, atu.tpc_id
		FROM @Atualizar atu
		INNER JOIN REL_DivergenciaAulasPrevistas div WITH(NOLOCK)
			ON div.tud_id = atu.tud_id
			AND div.tpc_id = atu.tpc_id
			AND div.AulasDadas = atu.aulasPrevistas
		WHERE atu.atualizarAulasDadas = 0

		-- Se o número de aulas dadas for igual ao número de aulas previstas,
		-- e existir divergência -> Remover divergência
		DELETE REL_DivergenciaAulasPrevistas
		FROM REL_DivergenciaAulasPrevistas div WITH(NOLOCK)
		INNER JOIN @SemDivergencia sdiv
			ON sdiv.tud_id = div.tud_id
			AND sdiv.tpc_id = div.tpc_id

		DELETE @Atualizar
		FROM @Atualizar atu
		INNER JOIN @SemDivergencia sdiv
			ON sdiv.tud_id = atu.tud_id
			AND sdiv.tpc_id = atu.tpc_id

		-- Se o número de aulas dadas for diferente do número de aulas previstas,
		-- e não existir divergência -> Criar divergência
		-- e existir divergência -> Atualizar informações
		MERGE REL_DivergenciaAulasPrevistas AS Destino
		USING @Atualizar AS Origem
		ON (
			Destino.tud_id = Origem.tud_id
			AND Destino.tpc_id = Origem.tpc_id
		)
		WHEN MATCHED THEN
			UPDATE SET
				Destino.AulasPrevistas = Origem.aulasPrevistas
				, Destino.AulasDadas = CASE WHEN Origem.atualizarAulasDadas = 1 THEN Origem.aulasDadas ELSE Destino.AulasDadas END
				, Destino.DataProcessamento = @dataAtual
		WHEN NOT MATCHED THEN
			INSERT (tud_id, tpc_id, AulasPrevistas, AulasDadas, DataProcessamento)
			VALUES (Origem.tud_id, Origem.tpc_id, Origem.aulasPrevistas, ISNULL(Origem.aulasDadas, 0), @dataAtual);
	END
END
GO
PRINT N'Creating [dbo].[STP_ACA_AlunoJustificativaFaltaAnexo_SELECT]'
GO


CREATE PROCEDURE [dbo].[STP_ACA_AlunoJustificativaFaltaAnexo_SELECT]
	
AS
BEGIN
	SELECT 
		alu_id
		,afj_id
		,aja_id
		,arq_id
		,aja_descricao
		,aja_situacao
		,aja_dataCriacao
		,aja_dataAlteracao

	FROM 
		ACA_AlunoJustificativaFaltaAnexo WITH(NOLOCK) 
	
END


GO
PRINT N'Altering [dbo].[NEW_MTR_MatriculaTurma_SelectAlunos]'
GO
-- =============================================
-- Author:		JORGE FREITAS
-- Create date: 01/04/2011
-- Description:	Lista dos alunos matriculados para aquela turma 
--				Dados exibidos no grid de Alunos por turma:
--				1.Número de chamada;
--				2.Número de matrícula ou matrícula estadual de acordo com o parâmetro ;
--				3.Nome do Aluno.
--				Dados usados como DataKeys: MT.alu_id, MT.mtu_id
-- =============================================
ALTER PROCEDURE [dbo].[NEW_MTR_MatriculaTurma_SelectAlunos]
	@tur_id BIGINT
AS
BEGIN

	--Seleciona o parâmetro de matrícula estadual
	DECLARE @MatriculaEstadual VARCHAR(MAX) = ''
	SELECT @MatriculaEstadual = pac_valor FROM ACA_ParametroAcademico WITH(NOLOCK) WHERE pac_chave = 'MATRICULA_ESTADUAL';
	
	--Selecina as movimentações que possuem matrícula anterior
	WITH TabelaMovimentacao AS (
		SELECT
			MOV.alu_id,
			mtu_idAnterior,
			CASE WHEN tmo_tipoMovimento IN (6, 12, 14, 15, 16)
				 THEN TMV.tmv_nome + ' em ' + REPLACE(CONVERT(VARCHAR(10), mov.mov_dataRealizacao, 103), '/' + CAST(DATEPART(YEAR, mov.mov_dataRealizacao) AS VARCHAR(4)), '') +
				 	  ISNULL(' p/ ' + turD.tur_codigo, '')
				 WHEN tmo_tipoMovimento IN (8)
				 THEN TMV.tmv_nome + ' p/' + ISNULL(' ' + turD.tur_codigo, '')
				 WHEN tmo_tipoMovimento IN (11)
				 THEN TMV.tmv_nome + ' p/ ' + ISNULL(' ' + turD.tur_codigo, '')
				 ELSE TMV.tmv_nome
			END tmv_nome,
			mov_ordem,
			mov_dataRealizacao   
		FROM
			MTR_MatriculaTurma AS MT WITH(NOLOCK)
			INNER JOIN MTR_Movimentacao MOV WITH (NOLOCK) 
				ON MT.alu_id = MOV.alu_id
				AND MT.mtu_id = MOV.mtu_idAnterior
			INNER JOIN ACA_TipoMovimentacao TMV WITH (NOLOCK) 
				ON MOV.tmv_idSaida = TMV.tmv_id
			LEFT JOIN MTR_TipoMovimentacao tmo WITH(NOLOCK)
				ON mov.tmo_id = tmo.tmo_id
				AND tmo.tmo_situacao <> 3
			LEFT JOIN MTR_MatriculaTurma mtuD WITH(NOLOCK)
				ON mov.alu_id = mtuD.alu_id
				AND mov.mtu_idAtual = mtuD.mtu_id
			LEFT JOIN TUR_Turma turD WITH(NOLOCK)
				ON mtuD.tur_id = turD.tur_id
			LEFT JOIN ACA_CalendarioAnual calD WITH(NOLOCK)
				ON turD.cal_id = calD.cal_id
			INNER JOIN MTR_MatriculaTurma mtuO WITH(NOLOCK)
				ON mov.alu_id = mtuO.alu_id
				AND mov.mtu_idAnterior = mtuO.mtu_id
				AND mtuO.tur_id = @tur_id
			LEFT JOIN TUR_Turma turO WITH(NOLOCK)
				ON mtuO.tur_id = turO.tur_id
			LEFT JOIN ACA_CalendarioAnual calO WITH(NOLOCK)
				ON turO.cal_id = calO.cal_id
		WHERE
			mov_situacao NOT IN (3,4)
			AND tmv_situacao <> 3
			AND mtu_idAnterior IS NOT NULL	
	)
		
	SELECT 
		CASE WHEN mtu_numeroChamada = -1
			THEN 1000
			ELSE ISNULL(mtu_numeroChamada, 1000) 
		END AS mtu_numeroChamada,
		ROW_NUMBER() OVER 
		(
			ORDER BY 
				mtu_numeroChamada,
				PES.pes_nome
		) AS numeroChamada,
		mtu_numeroChamada AS numeroChamadaReal,
		PES.pes_nome AS alunoNome,
		CASE WHEN (@MatriculaEstadual = '')
			THEN AC.alc_matricula
			ELSE AC.alc_matriculaEstadual				
		END AS numeroMatricula,
		MT.alu_id, 
		MT.mtu_id,
		MT.mtu_situacao AS mtuSituacao,
		(
			SELECT TOP 1 tmv_nome
			FROM TabelaMovimentacao MOV WITH(NOLOCK)
			WHERE MOV.mtu_idAnterior = MT.mtu_id
			AND MOV.alu_id = MT.alu_id
			ORDER BY mov_ordem DESC, mov_dataRealizacao DESC
		) AS movimentacaoSaida,
		MT.mtu_dataMatricula,
		MT.mtu_dataSaida
	FROM 
		MTR_MatriculaTurma AS MT WITH(NOLOCK)	
		INNER JOIN ACA_Aluno AS AL WITH(NOLOCK)
			ON AL.alu_id = MT.alu_id
		INNER JOIN  VW_DadosAlunoPessoa AS PES 
			ON AL.alu_id = PES.alu_id	
		INNER JOIN ACA_AlunoCurriculo AS AC WITH(NOLOCK)
			ON AL.alu_id = AC.alu_id
			AND AC.alc_id = MT.alc_id
			AND AC.cur_id = MT.cur_id
			AND AC.crr_id = MT.crr_id
			AND AC.crp_id = MT.crp_id
	WHERE
		MT.tur_id = @tur_id
		AND MT.mtu_situacao IN (1, 4, 5, 6) -- Ativo, Em matrícula, Inativo e Efetivado
		AND AC.alc_situacao IN (1, 4, 7) -- Ativo, Inativo e Em matrícula
		AND AL.alu_situacao IN (1, 4, 7) -- Ativo, Inativo e Em matrícula
		AND mtu_numeroChamada > -1
	GROUP BY
		PES.pes_nome,
		CASE WHEN (@MatriculaEstadual = '')
			THEN AC.alc_matricula
			ELSE AC.alc_matriculaEstadual				
		END,
		MT.alu_id, 
		MT.mtu_id,
		MT.mtu_situacao,
		mtu_numeroChamada,
		mtu_dataMatricula,
		mtu_dataSaida

	UNION ALL
		
	SELECT 
		CASE WHEN mtu_numeroChamada = -1
			THEN 1000
			ELSE ISNULL(mtu_numeroChamada, 1000) 
		END AS mtu_numeroChamada,
		CASE WHEN mtu_numeroChamada IS NULL
			THEN -1
			ELSE mtu_numeroChamada 
		END AS numeroChamada,
			CASE WHEN mtu_numeroChamada IS NULL
			THEN -1
			ELSE mtu_numeroChamada 
		END AS numeroChamadaReal,
		PES.pes_nome AS alunoNome,
		CASE WHEN (@MatriculaEstadual = '')
			THEN AC.alc_matricula
			ELSE AC.alc_matriculaEstadual				
		END AS numeroMatricula,
		MT.alu_id, 
		MT.mtu_id,
		MT.mtu_situacao AS mtuSituacao,
		(
			SELECT TOP 1 tmv_nome
			FROM TabelaMovimentacao MOV WITH(NOLOCK)
			WHERE MOV.mtu_idAnterior = MT.mtu_id
			AND MOV.alu_id = MT.alu_id
			ORDER BY mov_ordem DESC, mov_dataRealizacao DESC
			
		) AS movimentacaoSaida,
		MT.mtu_dataMatricula,
		MT.mtu_dataSaida
	FROM 
		MTR_MatriculaTurma AS MT WITH(NOLOCK)	
		INNER JOIN ACA_Aluno AS AL WITH(NOLOCK)
			ON AL.alu_id = MT.alu_id	
		INNER JOIN  VW_DadosAlunoPessoa AS PES
			ON AL.alu_id = PES.alu_id	
		INNER JOIN ACA_AlunoCurriculo AS AC WITH(NOLOCK)
			ON AL.alu_id = AC.alu_id
			AND AC.alc_id = MT.alc_id
			AND AC.cur_id = MT.cur_id
			AND AC.crr_id = MT.crr_id
			AND AC.crp_id = MT.crp_id	
	WHERE
		MT.tur_id = @tur_id
		AND MT.mtu_situacao IN (1, 4, 5, 6) -- Ativo, Em matrícula, Inativo e Efetivado
		AND AC.alc_situacao IN (1, 4, 7) -- Ativo, Inativo e Em matrícula
		AND AL.alu_situacao IN (1, 4, 7) -- Ativo, Inativo e Em matrícula
		AND (mtu_numeroChamada = -1 OR mtu_numeroChamada IS NULL)
	GROUP BY
		PES.pes_nome,
		CASE WHEN (@MatriculaEstadual = '')
			THEN AC.alc_matricula
			ELSE AC.alc_matriculaEstadual				
		END,
		MT.alu_id, 
		MT.mtu_id,
		MT.mtu_situacao,
		mtu_numeroChamada,
		mtu_dataMatricula,
		mtu_dataSaida
END




GO
PRINT N'Altering [dbo].[NEW_ACA_ObjetoAprendizagemTipoCiclo_By_Oap_Id]'
GO



ALTER PROCEDURE [dbo].[NEW_ACA_ObjetoAprendizagemTipoCiclo_By_Oap_Id]
	@oap_id Int

AS
BEGIN
	SELECT 
		atc.tci_id,
		CASE WHEN SUM(ISNULL(crp.tci_id, 0)) > 0 THEN 1 ELSE 0 END AS CicloEmUso
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
		atc.tci_id
END



GO
PRINT N'Altering [dbo].[NEW_Relatorio_005_SubAtaFinalResultadosRegencia]'
GO
-- ==========================================================================================
-- Author:		Rafael Benevente
-- Create date: 05/11/2014
-- Description:	Procedure para a geração de dados para o relatório de ata final de resultados
-- ==========================================================================================
-- ==========================================================================================
-- Author:		Daniel Jun Suguimoto
-- Alter date:  28/01/2015
-- Description:	Correção ao carregar o parecer conclusivo.

---- Alterado: Marcia Haga - 07/03/2015
---- Description: Alterado para retornar o percentualMinimoFrequencia.

---- Alterado: Marcia Haga - 09/03/2015
---- Description: Alterado para nao retornar valor nulo no numero de faltas, numero de compensacoes 
---- e porcentagem de frequencia.

---- Alterado: Marcia Haga - 10/03/2015
---- Description: Corrigido nome da coluna de faltas e compensacoes no retorno.
---- Alterado: Marcia Haga - 11/03/2015
---- Description: Corrigido para verificar a situacao de aluno ativo no periodo 
---- de acordo com a matricula na turma (mtu_id).

---- Alterado: Marcia Haga - 13/03/2015
---- Description: Alterado para considerar a frequencia como 100%,
---- caso nao existam aulas.

---- Alterado: Daniel Jun Suguimoto - 16/06/2015
---- Description: Alterado para considerar disciplinas de turmas multisseriadas.

---- Alterado: Marcia Haga - 23/08/2016
---- Description: Alterado para desconsiderar as disciplinas do tipo experiência e território.
-- ==========================================================================================
ALTER PROCEDURE [dbo].[NEW_Relatorio_005_SubAtaFinalResultadosRegencia]
	@tur_id BIGINT
	, @cal_id INT
	, @Executar BIT
	, @documentoOficial BIT
AS
BEGIN
	
	IF (@Executar = 1)
	BEGIN

		DECLARE @MatriculasBoletim TABLE 
			(
				alu_id BIGINT
				, mtu_id INT
				, tur_id BIGINT
				, tpc_id INT
				, tpc_ordem INT
				, PeriodosEquivalentes BIT
				, MesmoCalendario BIT
				, MesmoFormato BIT
				, fav_id INT
				, mtu_numeroChamada INT
				, cal_id INT
				, cal_ano INT
				, cap_id INT
				, PossuiSaidaPeriodo BIT
				, registroExterno BIT 
				, PermiteConceitoGlobal	BIT
				, PermiteDisciplinas BIT
				, tur_idMatriculaBoletim BIGINT
				-- Indica se o fechamento do último bimestre foi realizado nessa turma (na avaliação final).
				, FechamentoUltimoBimestre Bit
			)
		
			DECLARE @dadosAlunos TABLE 
			(
				alu_id BIGINT
				, pes_nome VARCHAR(200)
				, tur_id BIGINT
				, fav_id INT
				, tud_id BIGINT
				, tud_tipo TINYINT
				, atd_avaliacao VARCHAR(20)
				, atd_frequencia DECIMAL(5,2)
				, atd_numeroFaltas INT
				, atd_numeroAulas INT
				, atd_ausenciasCompensadas INT
				, atd_frequenciaFinalAjustada DECIMAL(5,2)
				, dis_id INT
				, tpc_id INT
				, tds_ordem INT
				, tpc_ordem INT
				, Tpc_Agrupamento INT
				, Tpc_exibicao CHAR(3)
				, dis_nome VARCHAR(200)
				, mtu_numeroChamada INT
				, periodoFechado BIT
				, mtu_resultadoDescricao VARCHAR(100)
				, mtu_situacao TINYINT
				, percentualMinimoFrequencia DECIMAL(5,2)
				, fav_percentualMinimoFrequenciaFinalAjustadaDisciplina DECIMAL(5,2)
				, mtu_id INT
				, tur_idMatriculaBoletim BIGINT
				-- Indica se o fechamento do último bimestre foi realizado nessa turma (na avaliação final).
				, FechamentoUltimoBimestre Bit NOT NULL
				, PossuiFreqExterna BIT
			)
		
			DECLARE @FrequenciaExterna TABLE (alu_id BIGINT, mtu_id INT, tud_id BIGINT, dis_id BIGINT, qtdFaltas INT, qtdAulas INT)

			DECLARE @Parecer TABLE 
			(alu_id BIGINT,
			 mtu_resultadoDescricao VARCHAR(100)
			 PRIMARY KEY (alu_id))
		
			DECLARE @dadosAlunosAnuais TABLE 
			(alu_id BIGINT,
			 tud_id BIGINT,				
			 totalFaltas INT,
			 totalAusenciasCompensadas INT,
			 totalAulas INT
			 PRIMARY KEY (alu_id, tud_id))
		
			DECLARE @dadosRegencia TABLE 
			(alu_id BIGINT,
			 frequenciaFinalAjustadaRegencia DECIMAL(5,2),
			 totalAulasRegencia INT,
			 totalFaltasRegencia INT,
			 totalAusenciasCompensadasRegencia INT)
		
			DECLARE @tbAlunosSituacao TABLE 
			(alu_id BIGINT,
			 mtu_id INT,
			 situacao TINYINT
			 PRIMARY KEY (alu_id, mtu_id))
		
			DECLARE @frequenciaAnual TABLE 
			(alu_id BIGINT,
			 frequenciaFinalAnual DECIMAL(5,2)
			 PRIMARY KEY (alu_id))
	
			DECLARE @Tabela TABLE 
			(alu_id BIGINT, 
			 mtu_id INT,
			 tur_id BIGINT,
			 tpc_id INT,
			 tpc_ordem INT,
			 PeriodosEquivalentes BIT,
			 MesmoCalendario BIT,
			 MesmoFormato BIT, 
			 fav_id INT, 
			 mtu_numeroChamada INT, 
			 cal_id INT,
			 cal_ano INT,
			 cap_id INT,
			 PossuiSaidaPeriodo BIT,
			 registroExterno BIT,
			 PermiteConceitoGlobal BIT,
			 PermiteDisciplinas BIT)
		
			DECLARE @Tabela2 TABLE 
			(alu_id BIGINT, 
			 mtu_id INT,
			 tur_id BIGINT,
			 tpc_id INT,
			 tpc_ordem INT,
			 PeriodosEquivalentes BIT,
			 MesmoCalendario BIT,
			 MesmoFormato BIT, 
			 fav_id INT, 
			 mtu_numeroChamada INT, 
			 cal_id INT,
			 cal_ano INT,
			 cap_id INT,
			 PossuiSaidaPeriodo BIT,
			 registroExterno BIT,
			 PermiteConceitoGlobal BIT,
			 PermiteDisciplinas BIT,
			 ExibirMtu BIT,
			 ExibirAluno BIT)		
		
			DECLARE @MTR_MatriculaTurma TABLE 
			(alu_id BIGINT,
			 mtu_id INT,
			 mtu_numeroChamada INT,
			 linha INT)
		
			INSERT INTO @MTR_MatriculaTurma
			SELECT alu_id, mtu_id, mtu_numeroChamada,
				   ROW_NUMBER() OVER(PARTITION BY alu_id ORDER BY mtu_id desc) as linha
			FROM MTR_MatriculaTurma WITH(NOLOCK) 
			WHERE mtu_situacao <> 3
				  AND tur_id = @tur_id
		
			INSERT INTO @Tabela
			SELECT mb.alu_id, mb.mtu_id, mb.tur_id, mb.tpc_id, mb.tpc_ordem, mb.PeriodosEquivalentes, 
				mb.MesmoCalendario, mb.MesmoFormato, mb.fav_id, mtu.mtu_numeroChamada, mb.cal_id, mb.cal_ano, mb.cap_id , mb.PossuiSaidaPeriodo,
				mb.registroExterno, mb.PermiteConceitoGlobal, mb.PermiteDisciplinas
			FROM @MTR_MatriculaTurma mtu 
			INNER JOIN MTR_MatriculasBoletim mb WITH(NOLOCK)
				ON mb.alu_id = mtu.alu_id 
				AND mb.mtu_origemDados = mtu.mtu_id 
				AND 1 = mtu.linha
				AND mb.PeriodosEquivalentes = 1 -- traz apenas alunos que possuem períodos equivalentes
				AND mb.PossuiSaidaPeriodo = 0
				AND mb.registroExterno = 0
		
			INSERT INTO @Tabela2
			SELECT
				mb.alu_id
				, mb.mtu_id
				, mb.tur_id
				, mb.tpc_id
				, mb.tpc_ordem
				, mb.PeriodosEquivalentes
				, mb.MesmoCalendario
				, mb.MesmoFormato
				, mb.fav_id
				, mb.mtu_numeroChamada
				, mb.cal_id
				, mb.cal_ano
				, mb.cap_id
				, mb.PossuiSaidaPeriodo
				, mb.registroExterno
				, mb.PermiteConceitoGlobal
				, mb.PermiteDisciplinas
				, CASE WHEN 
					-- se a turma do relatorio for do 4 bimeste, trazer todos os registros
					-- se nao, trazer so onde tem fechamento naquela turma
					EXISTS 
					(
						SELECT 1
						FROM @Tabela T
						WHERE 
							T.alu_id = mb.alu_id
							AND T.tpc_id = 4 AND T.tur_id = @tur_id
					)
					OR mb.tur_id = @tur_id
					THEN 1 ELSE 0 END AS ExibirMtu
				, CASE WHEN 
					-- se a turma do relatório não for do fechamento de nenhum bimestre para esse aluno,
					-- não exibir o aluno.
					EXISTS 
					(
						SELECT 1
						FROM @Tabela T
						WHERE 
							T.alu_id = mb.alu_id
							AND T.tur_id = @tur_id
					)
					THEN 1 ELSE 0 END AS ExibirAluno
			FROM @Tabela mb
		
			INSERT INTO @MatriculasBoletim (alu_id, mtu_id, tur_id, tpc_id, tpc_ordem, PeriodosEquivalentes, 
								MesmoCalendario, MesmoFormato, fav_id, mtu_numeroChamada, cal_id, cal_ano, cap_id , PossuiSaidaPeriodo,
								registroExterno, PermiteConceitoGlobal, PermiteDisciplinas
								, tur_idMatriculaBoletim, FechamentoUltimoBimestre)
			SELECT
				mb.alu_id
				, CASE WHEN ExibirMtu = 1 THEN mb.mtu_id ELSE NULL END AS mtu_id
				, CASE WHEN ExibirMtu = 1 THEN mb.tur_id ELSE @tur_id END AS tur_id
				, mb.tpc_id
				, mb.tpc_ordem
				, mb.PeriodosEquivalentes
				, mb.MesmoCalendario
				, mb.MesmoFormato
				, mb.fav_id
				, mb.mtu_numeroChamada AS mtu_numeroChamada
				, mb.cal_id
				, mb.cal_ano
				, mb.cap_id
				, mb.PossuiSaidaPeriodo
				, mb.registroExterno
				, mb.PermiteConceitoGlobal
				, mb.PermiteDisciplinas
				, mb.tur_id AS tur_idMatriculaBoletim
				, 0 as FechamentoUltimoBimestre
			FROM @Tabela2 mb
			WHERE mb.ExibirAluno = 1
		
			-- Adiciona um registro para a avaliacao final
			INSERT INTO @MatriculasBoletim (alu_id, mtu_id, tur_id, tpc_id, tpc_ordem, PeriodosEquivalentes, 
								MesmoCalendario, MesmoFormato, fav_id, mtu_numeroChamada, cal_id, cal_ano, cap_id , PossuiSaidaPeriodo,
								registroExterno, PermiteConceitoGlobal, PermiteDisciplinas, tur_idMatriculaBoletim, FechamentoUltimoBimestre)
			SELECT 
				alu_id, mtu_id, tur_id, NULL AS tpc_id, NULL AS tpc_ordem, PeriodosEquivalentes, 
				MesmoCalendario, MesmoFormato, fav_id, mtu_numeroChamada, cal_id, cal_ano, NULL AS cap_id , PossuiSaidaPeriodo,
				registroExterno, PermiteConceitoGlobal, PermiteDisciplinas, tur_idMatriculaBoletim
				, FechamentoUltimoBimestre
			FROM
				(SELECT 
					alu_id, mtu_id, tur_id, null AS tpc_id, null AS tpc_ordem, PeriodosEquivalentes, 
					MesmoCalendario, MesmoFormato, fav_id, mtu_numeroChamada, cal_id, cal_ano, cap_id , PossuiSaidaPeriodo,
					registroExterno, PermiteConceitoGlobal, PermiteDisciplinas, tur_idMatriculaBoletim
					, CASE WHEN tpc_ordem = 4 THEN 1 ELSE 0 END AS FechamentoUltimoBimestre,
					ROW_NUMBER() OVER(PARTITION BY alu_id ORDER BY alu_id, tpc_ordem desc) AS ordem
				FROM 
					@MatriculasBoletim) AS MatAlu
			WHERE 
				MatAlu.ordem = 1
		
			DECLARE @alunosDisciplina TABLE 
			(alu_id BIGINT,
			 tur_id BIGINT,
			 tpc_id INT,
			 mtu_id INT,
			 fav_id INT,
			 tud_id BIGINT,
			 tud_tipo TINYINT,
			 dis_id INT,
			 dis_nome VARCHAR(200),
			 tds_ordem INT,
			 tpc_ordem INT,
			 tur_idMatriculaBoletim BIGINT,
			 mtu_numeroChamada INT,
			 FechamentoUltimoBimestre BIT)
		
			INSERT INTO @alunosDisciplina
			SELECT
				Mb.alu_id,
				Mb.tur_id,
				Mb.tpc_id,
				mb.mtu_id,
				Mb.fav_id,
				Tud.tud_id,
				Tud.tud_tipo,
				Dis.tds_id AS dis_id,
				Dis.dis_nome,
				Tds.tds_ordem,
				mb.tpc_ordem,
				mb.tur_idMatriculaBoletim,
				MB.mtu_numeroChamada,
				Mb.FechamentoUltimoBimestre
			FROM @MatriculasBoletim Mb
				INNER JOIN TUR_TurmaRelTurmaDisciplina TrT WITH(NOLOCK)
					ON TrT.tur_id = Mb.tur_id
				INNER JOIN TUR_TurmaDisciplina Tud WITH(NOLOCK)
					ON Tud.tud_id = TrT.tud_id
					AND Tud.tud_tipo NOT IN (14, 17, 18, 19)
					AND Tud.tud_situacao <> 3
				INNER JOIN TUR_TurmaDisciplinaRelDisciplina TrD WITH(NOLOCK)
					ON TrD.tud_id = Tud.tud_id
				INNER JOIN ACA_Disciplina Dis WITH(NOLOCK)
					ON Dis.dis_id = TrD.dis_id
					AND Dis.dis_situacao <> 3
				INNER JOIN ACA_TipoDisciplina Tds WITH(NOLOCK)
					ON Tds.tds_id = Dis.tds_id
					AND Tds.tds_situacao <> 3
			WHERE
				Tds.tds_tipo IN (1, 3) -- Disciplina e Regencia de classe

			UNION

			SELECT 
				Mb.alu_id,
				Mb.tur_id,
				Mb.tpc_id,
				mb.mtu_id,
				Mb.fav_id,
				Tud.tud_id,
				Tud.tud_tipo,
				Dis.tds_id AS dis_id,
				Dis.dis_nome,
				Tds.tds_ordem,
				mb.tpc_ordem,
				mb.tur_idMatriculaBoletim,
				MB.mtu_numeroChamada,
				Mb.FechamentoUltimoBimestre
			FROM @MatriculasBoletim Mb
				INNER JOIN MTR_MatriculaTurmaDisciplina mtd WITH(NOLOCK)
					ON mtd.alu_id = Mb.alu_id
					AND mtd.mtu_id = Mb.mtu_id
					AND mtd.mtd_situacao <> 3
				INNER JOIN TUR_TurmaDisciplina Tud WITH(NOLOCK)
					ON Tud.tud_id = mtd.tud_id
					AND Tud.tud_tipo = 14
					AND Tud.tud_situacao <> 3
				INNER JOIN TUR_TurmaRelTurmaDisciplina Trt WITH(NOLOCK)
					ON Trt.tud_id = Tud.tud_id
				INNER JOIN TUR_Turma tur WITH(NOLOCK)
					ON tur.tur_id = Trt.tur_id
					AND tur.tur_situacao <> 3
				INNER JOIN TUR_TurmaDisciplinaRelDisciplina TrD WITH(NOLOCK)
					ON TrD.tud_id = Tud.tud_id
				INNER JOIN ACA_Disciplina Dis WITH(NOLOCK)
					ON Dis.dis_id = TrD.dis_id
					AND Dis.dis_situacao <> 3
				INNER JOIN ACA_TipoDisciplina Tds WITH(NOLOCK)
					ON Tds.tds_id = Dis.tds_id
					AND Tds.tds_situacao <> 3

			DECLARE @tipoResultado TABLE 
			(tpr_nomenclatura VARCHAR(100),
			 tpr_resultado TINYINT,
			 tpr_tipoLancamento TINYINT,
			 cur_id INT,
			 crr_id INT,
			 crp_id INT)
		
			INSERT INTO @tipoResultado
			SELECT
				tpr.tpr_nomenclatura,
				tpr.tpr_resultado,
				tpr.tpr_tipoLancamento,
				tcr.cur_id,
				tcr.crr_id,
				tcr.crp_id
			FROM
				ACA_TipoResultado tpr WITH(NOLOCK)
				INNER JOIN ACA_TipoResultadoCurriculoPeriodo tcr WITH(NOLOCK)
					ON tpr.tpr_id = tcr.tpr_id
			WHERE
				tpr.tpr_situacao <> 3
		
			INSERT INTO @FrequenciaExterna (alu_id, mtu_id, tud_id, dis_id, qtdFaltas, qtdAulas)
			SELECT
				Mtd.alu_id,
				Mtd.mtu_id,
				Mtd.tud_id,
				Ad.dis_id,
				SUM(ISNULL(afx.afx_qtdFaltas, 0)) AS qtdFaltas,
				SUM(ISNULL(afx.afx_qtdAulas, 0)) AS qtdAulas
			FROM @alunosDisciplina Ad
			INNER JOIN MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
				ON Mtd.alu_id = Ad.alu_id
				AND Mtd.mtu_id = Ad.mtu_id
				AND Mtd.tud_id = Ad.tud_id
				AND Mtd.mtd_situacao <> 3
			INNER JOIN CLS_AlunoFrequenciaExterna afx WITH(NOLOCK)
				ON Mtd.alu_id = afx.alu_id
				AND Mtd.mtu_id = afx.mtu_id
				AND Mtd.mtd_id = afx.mtd_id
				AND Ad.tpc_id IS NULL
				AND afx.afx_situacao <> 3
			GROUP BY
				Mtd.alu_id,
				Mtd.mtu_id,
				Mtd.tud_id,
				Ad.dis_id

			DECLARE @possuiFreqExterna BIT = 0
			IF (EXISTS(SELECT TOP 1 alu_id FROM @FrequenciaExterna WHERE qtdFaltas > 0 OR qtdAulas > 0))
				SET @possuiFreqExterna = 1

			;WITH tbFechamentoEF AS 
			(
				SELECT
					Atd.tud_id, 
					Atd.alu_id, 
					Atd.mtu_id, 
					Atd.mtd_id, 
					Atd.atd_id, 
					Atd.fav_id, 
					Atd.ava_id, 
					Atd.atd_avaliacao,
					Atd.atd_avaliacaoPosConselho, 
					Atd.atd_frequencia,
					Atd.atd_numeroFaltas,
					Atd.atd_numeroAulas,
					Atd.atd_ausenciasCompensadas,
					Atd.atd_frequenciaFinalAjustada,
					ava.tpc_id,
					esa.esa_id
				FROM
					@alunosDisciplina Ad
					INNER JOIN CLS_AlunoAvaliacaoTurmaDisciplina Atd WITH(NOLOCK)
						ON Atd.tud_id = Ad.tud_id
						AND Atd.alu_id = Ad.alu_id
						AND Atd.mtu_id = Ad.mtu_id
						AND Atd.atd_situacao <> 3
					INNER JOIN ACA_Avaliacao ava WITH(NOLOCK)
						ON ava.fav_id = Atd.fav_id 
						AND ava.ava_id = Atd.ava_id
						AND ISNULL(ava.tpc_id, 999) = ISNULL(Ad.tpc_id, 999) -- Caso seja 999 sera a avaliacao final
						AND ava.ava_situacao <> 3
					INNER JOIN ACA_FormatoAvaliacao fav WITH(NOLOCK)
						ON fav.fav_id = Atd.fav_id
						AND fav.fav_situacao <> 3
					INNER JOIN ACA_EscalaAvaliacao esa WITH(NOLOCK)
						ON esa.esa_id = fav.esa_idPorDisciplina
						AND esa.esa_situacao <> 3
				WHERE
					Ad.tud_tipo = 14
				GROUP BY
					Atd.tud_id, 
					Atd.alu_id, 
					Atd.mtu_id, 
					Atd.mtd_id, 
					Atd.atd_id, 
					Atd.fav_id, 
					Atd.ava_id, 
					Atd.atd_avaliacao,
					Atd.atd_avaliacaoPosConselho, 
					Atd.atd_frequencia,
					Atd.atd_numeroFaltas,
					Atd.atd_numeroAulas,
					Atd.atd_ausenciasCompensadas,
					Atd.atd_frequenciaFinalAjustada,
					ava.tpc_id,
					esa.esa_id
			)

			INSERT INTO @dadosAlunos
			SELECT  
				Ad.alu_id,
				CASE WHEN @documentoOficial = 1 THEN Pes.pes_nomeOficial ELSE Pes.pes_nome END AS pes_nome,
				Ad.tur_id,
				Ad.fav_id,
				Ad.tud_id,
				Ad.tud_tipo,
				ISNULL(Atd.atd_avaliacaoPosConselho, Atd.atd_avaliacao) AS atd_avaliacao,
				Atd.atd_frequencia,
				ISNULL(Atd.atd_numeroFaltas, 0) + ISNULL(afx.qtdFaltas, 0) AS atd_numeroFaltas,
				ISNULL(Atd.atd_numeroAulas, 0) + ISNULL(afx.qtdAulas, 0) AS atd_numeroAulas,
				ISNULL(Atd.atd_ausenciasCompensadas, 0) AS atd_ausenciasCompensadas,
				Atd.atd_frequenciaFinalAjustada,
				Ad.dis_id,
				Ad.tpc_id,
				Ad.tds_ordem,				
				ad.tpc_ordem,
				CASE 
					WHEN Ad.tpc_ordem = 1 THEN 1
					WHEN Ad.tpc_ordem = 2 THEN 2
					WHEN Ad.tpc_ordem = 3 THEN 3
					WHEN Ad.tpc_ordem = 4 THEN 4
					WHEN Ad.tpc_ordem IS NULL THEN 999 
				END  AS Tpc_Agrupamento,
				CASE 
					WHEN Ad.tpc_ordem = 1 THEN '1ºB' 
					WHEN Ad.tpc_ordem = 2 THEN '2ºB' 
					WHEN Ad.tpc_ordem = 3 THEN '3ºB' 
					WHEN Ad.tpc_ordem = 4 THEN '4ºB' 
					WHEN Ad.tpc_ordem IS NULL THEN 'SF' 
				END  AS Tpc_exibicao,
				Ad.dis_nome,
				Ad.mtu_numeroChamada,
				CASE 
					-- Quando não trouxe mtu_id do aluno, ou se ele não terminou no último bimestre nessa turma (na avaliação final)
					-- , não valida como pendência.
					WHEN Ad.mtu_id IS NULL OR (Ad.tpc_ordem IS NULL AND FechamentoUltimoBimestre = 0) THEN 1
					WHEN (Atd.atd_id IS NOT NULL) AND
						 (
							-- Valida a nota somente quando não é regência.
							Ad.tud_tipo = 11 OR 
							((ISNULL(Atd.atd_avaliacao,'') <> '') OR (ISNULL(Atd.atd_avaliacaoPosConselho,'') <> ''))
						 )
					THEN 1 ELSE 0 END AS periodoFechado,
				ISNULL(Tpr.tpr_nomenclatura, '-') AS mtu_resultadoDescricao,
				Mtu.mtu_situacao,
				Fav.percentualMinimoFrequencia,
				Fav.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina,
				Mtu.mtu_id,
				ad.tur_idMatriculaBoletim,
				FechamentoUltimoBimestre,
				CASE WHEN ISNULL(afx.qtdAulas, 0) > 0 OR ISNULL(afx.qtdFaltas, 0) > 0
					 THEN CAST(1 AS BIT) 
					 WHEN EXISTS(SELECT TOP 1 afx2.alu_id FROM @FrequenciaExterna afx2
								 WHERE Ad.alu_id = afx2.alu_id AND Ad.dis_id = afx2.dis_id
								 AND (ISNULL(afx2.qtdAulas, 0) > 0 OR ISNULL(afx2.qtdFaltas, 0) > 0))
					 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS possuiFreqExterna
			FROM
				@alunosDisciplina Ad
			INNER JOIN MTR_MatriculaTurma Mtu WITH(NOLOCK)
				ON Mtu.alu_id = Ad.alu_id
				AND Mtu.mtu_id = Ad.mtu_id
				--AND Mtu.tur_id = Ad.tur_id
				AND Mtu.mtu_situacao <> 3
			INNER JOIN ACA_Aluno Alu WITH (NOLOCK)
				ON Alu.alu_id = Ad.alu_id	
				AND Alu.alu_situacao <> 3
			INNER JOIN VW_DadosAlunoPessoa Pes
				ON Pes.alu_id = Alu.alu_id
			INNER JOIN ACA_AlunoCurriculo Alc WITH (NOLOCK)
				ON Alc.alu_id = Mtu.alu_id
				AND Alc.alc_id = Mtu.alc_id
				AND Alc.alc_situacao <> 3
			INNER JOIN MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
				ON Mtd.alu_id = Mtu.alu_id
				AND Mtd.mtu_id = Mtu.mtu_id
				AND Mtd.tud_id = Ad.tud_id
				AND Mtd.mtd_situacao <> 3
			INNER JOIN ACA_FormatoAvaliacao Fav WITH(NOLOCK)
				ON Fav.fav_id = Ad.fav_id
				AND Fav.fav_situacao <> 3
			INNER JOIN ACA_Avaliacao Ava WITH(NOLOCK)
				ON Ava.fav_id = Fav.fav_id
				AND ISNULL(AVA.tpc_id, 999) = ISNULL(Ad.tpc_id, 999) -- Caso seja 999 sera a avaliacao final
				AND Ava.ava_situacao <> 3
			LEFT JOIN CLS_AlunoAvaliacaoTurmaDisciplina Atd WITH(NOLOCK)
				ON Atd.tud_id = Mtd.tud_id
				AND Atd.alu_id = Mtd.alu_id
				AND Atd.mtu_id = Mtd.mtu_id
				AND Atd.mtd_id = Mtd.mtd_id
				AND Atd.fav_id = Fav.fav_id
				AND Atd.ava_id = Ava.ava_id
				AND Atd.atd_situacao <> 3
			LEFT JOIN @tipoResultado Tpr
				ON Tpr.cur_id = Mtu.cur_id
				AND Tpr.crr_id = Mtu.crr_id
				AND Tpr.crp_id = Mtu.crp_id
				AND Tpr.tpr_resultado = Mtu.mtu_resultado
				AND Tpr.tpr_tipoLancamento = 1
			LEFT JOIN @FrequenciaExterna afx
				ON Mtd.alu_id = afx.alu_id
				AND Mtd.mtu_id = afx.mtu_id
				AND Mtd.tud_id = afx.tud_id
				AND Ad.tpc_id IS NULL
			WHERE
				Ad.tud_tipo <> 14
			GROUP BY
				Ad.alu_id,
				Ad.mtu_id,
				Pes.pes_nome,
				Pes.pes_nomeOficial,
				Ad.tur_id,
				Ad.fav_id,
				Ad.tud_id,
				Ad.tud_tipo,
				Atd.atd_avaliacaoPosConselho, 
				Atd.atd_avaliacao,
				Atd.atd_frequencia,
				Atd.atd_numeroFaltas,
				Atd.atd_numeroAulas,
				Atd.atd_ausenciasCompensadas,
				Atd.atd_frequenciaFinalAjustada,
				Ad.dis_id,
				Ad.tpc_id,
				Ad.tds_ordem,
				Ad.tpc_ordem,
				Ad.tpc_id,
				Ad.dis_nome,
				Ad.mtu_numeroChamada,
				Atd.atd_id,
				Tpr.tpr_nomenclatura,
				Mtu.mtu_situacao,
				Fav.percentualMinimoFrequencia,
				Fav.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina,
				Mtu.mtu_id,
				ad.tur_idMatriculaBoletim,
				FechamentoUltimoBimestre,
				afx.qtdAulas,
				afx.qtdFaltas

		UNION

		SELECT  
			Ad.alu_id,
			CASE WHEN @documentoOficial = 1 THEN Pes.pes_nomeOficial ELSE Pes.pes_nome END AS pes_nome,
			Ad.tur_id,
			Ad.fav_id,
			Ad.tud_id,
			Ad.tud_tipo,
			CASE WHEN Atd.esa_id <> esa.esa_id
				THEN NULL 
				ELSE ISNULL(Atd.atd_avaliacaoPosConselho, Atd.atd_avaliacao) 
			END AS atd_avaliacao,
			CASE WHEN Atd.esa_id <> esa.esa_id
				THEN NULL
				ELSE Atd.atd_frequencia
			END AS atd_frequencia,
			CASE WHEN Atd.esa_id <> esa.esa_id
				THEN 0
				ELSE ISNULL(Atd.atd_numeroFaltas, 0)
			END + ISNULL(afx.qtdFaltas, 0) AS atd_numeroFaltas,
			CASE WHEN Atd.esa_id <> esa.esa_id
				THEN NULL
				ELSE Atd.atd_numeroAulas
			END + ISNULL(afx.qtdAulas, 0) AS atd_numeroAulas,
			CASE WHEN Atd.esa_id <> esa.esa_id
				THEN 0
				ELSE ISNULL(Atd.atd_ausenciasCompensadas, 0)
			END AS atd_ausenciasCompensadas,
			CASE WHEN Atd.esa_id <> esa.esa_id 
				THEN NULL
				ELSE Atd.atd_frequenciaFinalAjustada
			END AS atd_frequenciaFinalAjustada,
			Ad.dis_id,
			Ad.tpc_id,
			Ad.tds_ordem,				
			ad.tpc_ordem,
			CASE 
				WHEN Ad.tpc_ordem = 1 THEN 1
				WHEN Ad.tpc_ordem = 2 THEN 2
				WHEN Ad.tpc_ordem = 3 THEN 3
				WHEN Ad.tpc_ordem = 4 THEN 4
				WHEN Ad.tpc_ordem IS NULL THEN 999 
			END  AS Tpc_Agrupamento,
			CASE 
				WHEN Ad.tpc_ordem = 1 THEN '1ºB' 
				WHEN Ad.tpc_ordem = 2 THEN '2ºB' 
				WHEN Ad.tpc_ordem = 3 THEN '3ºB' 
				WHEN Ad.tpc_ordem = 4 THEN '4ºB' 
				WHEN Ad.tpc_ordem IS NULL THEN 'SF' 
			END  AS Tpc_exibicao,
			Ad.dis_nome,
			Ad.mtu_numeroChamada,
			
			CASE 
				-- Quando não trouxe mtu_id do aluno, ou se ele não terminou no último bimestre nessa turma (na avaliação final)
				-- , não valida como pendência.
				WHEN Ad.mtu_id IS NULL OR (Ad.tpc_ordem IS NULL AND FechamentoUltimoBimestre = 0) THEN 1
				WHEN (Atd.atd_id IS NOT NULL) AND Atd.esa_id = esa.esa_id AND
					 (
						-- Valida a nota somente quando não é regência.
						Ad.tud_tipo = 11 OR 
						((ISNULL(Atd.atd_avaliacao,'') <> '') OR (ISNULL(Atd.atd_avaliacaoPosConselho,'') <> ''))
					 )
				THEN 1 ELSE 0 END AS periodoFechado,
			
			ISNULL(Tpr.tpr_nomenclatura, '-') AS mtu_resultadoDescricao,
			Mtu.mtu_situacao,
			Fav.percentualMinimoFrequencia,
			Fav.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina,
			Mtu.mtu_id,
			ad.tur_idMatriculaBoletim,
			FechamentoUltimoBimestre,
			CASE WHEN ISNULL(afx.qtdAulas, 0) > 0 OR ISNULL(afx.qtdFaltas, 0) > 0
				 THEN CAST(1 AS BIT) 
				 WHEN EXISTS(SELECT TOP 1 afx2.alu_id FROM @FrequenciaExterna afx2
							 WHERE Ad.alu_id = afx2.alu_id AND Ad.dis_id = afx2.dis_id
							 AND (ISNULL(afx2.qtdAulas, 0) > 0 OR ISNULL(afx2.qtdFaltas, 0) > 0))
				 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS possuiFreqExterna
		FROM
			@alunosDisciplina Ad
			INNER JOIN MTR_MatriculaTurma Mtu WITH(NOLOCK)
				ON Mtu.alu_id = Ad.alu_id
					AND Mtu.mtu_id = Ad.mtu_id
					AND Mtu.mtu_situacao <> 3
			INNER JOIN ACA_Aluno Alu WITH (NOLOCK)
				ON Alu.alu_id = Ad.alu_id	
					AND Alu.alu_situacao <> 3
			INNER JOIN VW_DadosAlunoPessoa Pes 
				ON  Pes.alu_id = Alu.alu_id
			INNER JOIN ACA_AlunoCurriculo Alc WITH (NOLOCK)
				ON  Alc.alu_id = Mtu.alu_id
					AND Alc.alc_id = Mtu.alc_id
					AND Alc.alc_situacao <> 3
			INNER JOIN MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
				ON  Mtd.alu_id = Mtu.alu_id
					AND Mtd.mtu_id = Mtu.mtu_id
					AND Mtd.tud_id = Ad.tud_id
					AND Mtd.mtd_situacao <> 3
			INNER JOIN ACA_FormatoAvaliacao Fav WITH(NOLOCK)
				ON Fav.fav_id = Ad.fav_id
					AND Fav.fav_situacao <> 3
			INNER JOIN ACA_EscalaAvaliacao esa WITH(NOLOCK)
				ON esa.esa_id = Fav.esa_idPorDisciplina
				AND esa.esa_situacao <> 3
			LEFT JOIN tbFechamentoEF Atd 
				ON Atd.tud_id = Mtd.tud_id
					AND Atd.alu_id = Mtd.alu_id
					AND Atd.mtu_id = Mtd.mtu_id
					AND Atd.mtd_id = Mtd.mtd_id
					AND ISNULL(Atd.tpc_id, 999) = ISNULL(Ad.tpc_id, 999)
			LEFT JOIN @tipoResultado Tpr
				ON Tpr.cur_id = Mtu.cur_id
				AND Tpr.crr_id = Mtu.crr_id
				AND Tpr.crp_id = Mtu.crp_id
				AND Tpr.tpr_resultado = Mtu.mtu_resultado
				AND Tpr.tpr_tipoLancamento = 1
			LEFT JOIN @FrequenciaExterna afx
				ON Mtd.alu_id = afx.alu_id
				AND Mtd.mtu_id = afx.mtu_id
				AND Mtd.tud_id = afx.tud_id
				AND Ad.tpc_id IS NULL
		WHERE
			Ad.tud_tipo = 14
		GROUP BY
			Ad.alu_id,
			Ad.mtu_id,
			pes.pes_nomeOficial,
			Pes.pes_nome,
			Ad.tur_id,
			Ad.fav_id,
			Ad.tud_id,
			Ad.tud_tipo,
			Atd.atd_avaliacaoPosConselho, 
			Atd.atd_avaliacao,
			Atd.atd_frequencia,
			Atd.atd_numeroFaltas,
			Atd.atd_numeroAulas,
			Atd.atd_ausenciasCompensadas,
			Atd.atd_frequenciaFinalAjustada,
			Ad.dis_id,
			Ad.tpc_id,
			Ad.tds_ordem,
			Ad.tpc_ordem,
			Ad.tpc_id,
			Ad.dis_nome,
			Ad.mtu_numeroChamada,
			Atd.atd_id,
			Tpr.tpr_nomenclatura,
			Mtu.mtu_situacao,
			Fav.percentualMinimoFrequencia,
			Fav.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina,
			Mtu.mtu_id,
			ad.tur_idMatriculaBoletim,
			FechamentoUltimoBimestre,
			atd.esa_id,
			esa.esa_id,
			afx.qtdAulas,
			afx.qtdFaltas

		INSERT INTO @Parecer
		SELECT
			alu_id,
			mtu_resultadoDescricao
		FROM
		(
			SELECT
				alu_id,
				mtu_resultadoDescricao,
				ROW_NUMBER() OVER (PARTITION BY alu_id ORDER BY ISNULL(Tpc_Agrupamento, 0) DESC) AS linha
			FROM 
				@dadosAlunos Da
		) AS tabela
		WHERE linha = 1
		
		INSERT INTO @dadosAlunosAnuais
		SELECT
			alu_id,
			tud_id,				
			SUM(Da.atd_numeroFaltas) AS totalFaltas,
			SUM(Da.atd_ausenciasCompensadas) AS totalAusenciasCompensadas,
			SUM(Da.atd_numeroAulas) AS totalAulas
		FROM
			@dadosAlunos Da
		GROUP BY
			alu_id,
			tud_id

		INSERT INTO @dadosRegencia
		(alu_id, frequenciaFinalAjustadaRegencia, totalAulasRegencia, totalFaltasRegencia, totalAusenciasCompensadasRegencia)
		SELECT
			alu_id,
			frequenciaFinalAjustadaRegencia,
			totalAulasRegencia,
			totalFaltasRegencia,
			totalAusenciasCompensadasRegencia
		FROM
		(
			SELECT
				alu_id,
				atd_frequenciaFinalAjustada AS frequenciaFinalAjustadaRegencia,
				SUM(atd_numeroAulas) OVER (PARTITION BY alu_id) as totalAulasRegencia,
				SUM(atd_numeroFaltas) OVER (PARTITION BY alu_id) AS totalFaltasRegencia,
				SUM(atd_ausenciasCompensadas) OVER (PARTITION BY alu_id) AS totalAusenciasCompensadasRegencia,
				ROW_NUMBER() OVER (PARTITION BY alu_id ORDER BY ISNULL(Da.tpc_ordem, 0) DESC) AS linha
			FROM 
				@dadosAlunos Da
			WHERE Da.atd_frequenciaFinalAjustada IS NOT NULL 
				  AND da.tud_tipo = 11-- Regencia
		) AS tabela
		WHERE linha = 1
		GROUP BY
			alu_id,
			frequenciaFinalAjustadaRegencia,
			totalAulasRegencia,
			totalFaltasRegencia,
			totalAusenciasCompensadasRegencia
		
		INSERT INTO @tbAlunosSituacao
		SELECT 
			Alu.alu_id,
			Alu.mtu_id,
			--Caso seja movimentacao 8-Remanejamento , 27-Conclusão de Nivel de Ensino traz como ativo, para impressao de anos anteriores.
			1 AS situacao
		FROM 
			@dadosAlunos Alu
		INNER JOIN MTR_Movimentacao Mov WITH(NOLOCK)
			ON Mov.alu_id = Alu.alu_id
			AND Mov.mtu_idAnterior = Alu.mtu_id
			AND Mov.mov_situacao <> 3
		INNER JOIN MTR_TipoMovimentacao Tmo WITH(NOLOCK)
			ON Tmo.tmo_id = Mov.tmo_id
			AND Tmo.tmo_tipoMovimento IN (8,23,27)
			AND Tmo.tmo_situacao <> 3
		GROUP BY Alu.alu_id, Alu.mtu_id
		
		;WITH dadosAlunosTpcMax AS (
			SELECT
				Da.alu_id,
				MAX(tpc_ordem) AS maxOrdem
			FROM @dadosAlunos Da
			GROUP BY Da.alu_id
		), movimentacao AS (
			SELECT
				Da.alu_id,
				Da.mtu_id,
				CASE WHEN tmo_tipoMovimento IN (6, 12, 14, 15, 16)
					 THEN 'TR ' + REPLACE(CONVERT(VARCHAR(10), mov.mov_dataRealizacao, 103), '/' + CAST(DATEPART(YEAR, mov.mov_dataRealizacao) AS VARCHAR(4)), '') +
						  ISNULL(' - ' + turD.tur_codigo, '')
					 WHEN tmo_tipoMovimento IN (8)
					 THEN 'RM' + ISNULL(' ' + turD.tur_codigo, '')
					 WHEN tmo_tipoMovimento IN (11)
					 THEN 'RC' + ISNULL(' ' + turD.tur_codigo, '')
					 ELSE ''
				END movMsg
			FROM @dadosAlunos Da
			INNER JOIN dadosAlunosTpcMax dat
				ON Da.alu_id = dat.alu_id
				AND Da.tpc_ordem = dat.maxOrdem
			INNER JOIN MTR_Movimentacao mov WITH(NOLOCK)
				ON Da.alu_id = mov.alu_id
				AND Da.mtu_id = mov.mtu_idAnterior
				AND mov.mov_situacao <> 3
			INNER JOIN MTR_TipoMovimentacao tmo WITH(NOLOCK)
				ON mov.tmo_id = tmo.tmo_id
				AND tmo_tipoMovimento IN (6, 8, 11, 12, 14, 15, 16)
				AND tmo.tmo_situacao <> 3
			LEFT JOIN MTR_MatriculaTurma mtuD WITH(NOLOCK)
				ON mov.alu_id = mtuD.alu_id
				AND mov.mtu_idAtual = mtuD.mtu_id
			LEFT JOIN TUR_Turma turD WITH(NOLOCK)
				ON mtuD.tur_id = turD.tur_id
			LEFT JOIN ACA_CalendarioAnual calD WITH(NOLOCK)
				ON turD.cal_id = calD.cal_id
			INNER JOIN MTR_MatriculaTurma mtuO WITH(NOLOCK)
				ON mov.alu_id = mtuO.alu_id
				AND mov.mtu_idAnterior = mtuO.mtu_id
				AND mtuO.tur_id = @tur_id
			LEFT JOIN TUR_Turma turO WITH(NOLOCK)
				ON mtuO.tur_id = turO.tur_id
			LEFT JOIN ACA_CalendarioAnual calO WITH(NOLOCK)
				ON turO.cal_id = calO.cal_id
			WHERE 
				turD.tur_id IS NULL OR calD.cal_ano = calO.cal_ano --Ou não tem turma destino ou a turma destino é do mesmo ano
			GROUP BY
				Da.alu_id,
				Da.mtu_id,
				tmo_tipoMovimento,
				mov.mov_dataRealizacao,
				turD.tur_codigo
		)

		SELECT
			Da.alu_id,
			Da.pes_nome +
			CASE WHEN ISNULL(mov.movMsg, '') = ''
				 THEN ''
				 ELSE ' (' + mov.movMsg + ')'
			END AS pes_nome,
			Da.tur_id,
			Da.fav_id,
			Da.tud_id,
			Da.tud_tipo,
			Da.atd_avaliacao,
			Da.atd_frequencia,
			ISNULL(Da.atd_numeroFaltas, 0) AS atd_numeroFaltas,
			Da.atd_numeroAulas,
			ISNULL(Da.atd_ausenciasCompensadas, 0) AS atd_ausenciasCompensadas,
			Da.atd_frequenciaFinalAjustada,
			Da.dis_id,
			Da.tpc_id,
			Da.tds_ordem,
			Da.Tpc_Agrupamento,
			Da.Tpc_exibicao,
			Da.dis_nome,
			Da.mtu_numeroChamada,
			Da.periodoFechado,
			CASE WHEN EXISTS(SELECT TOP 1 Da2.alu_id FROM dadosAlunosTpcMax DaT
							 INNER JOIN @dadosAlunos Da2 ON DaT.alu_id = Da2.alu_id AND Dat.maxOrdem = Da2.tpc_ordem
							 WHERE Da2.alu_id = Da.alu_id AND Da2.mtu_situacao = 1)
				 THEN 1 ELSE COALESCE(tas.situacao, Da.mtu_situacao, 5) END AS mtu_situacao, 
			COALESCE(tas.situacao, Da.mtu_situacao, 5) AS mtu_situacaoPeriodo,
			Da.percentualMinimoFrequencia,
			Da.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina,
			SUM(ISNULL(Da.atd_ausenciasCompensadas, 0)) OVER(PARTITION BY Da.alu_id, Da.tud_id) AS atd_AusenciasCompensadasVC,
			ISNULL(CAST(Daa.totalFaltas AS VARCHAR),'0') AS totalFaltas,
			SUM(ISNULL(Daa.totalAusenciasCompensadas, 0)) OVER (PARTITION BY Da.alu_id, Da.tpc_ordem) AS totalAusenciasCompensadas,
			CASE WHEN Da.tud_tipo IN (11,12,13) THEN '100'
			ELSE ISNULL ((SELECT 				
					TOP 1 CAST(CAST(ISNULL(dda.atd_frequenciaFinalAjustada,0) AS DECIMAL(5,0)) AS VARCHAR)
				FROM 
					@dadosAlunos dda 
				WHERE 	
					dda.alu_id = da.alu_id
					AND dda.dis_id = da.dis_id
					AND dda.tpc_ordem = (SELECT MAX(ddaa.tpc_ordem) FROM @dadosAlunos ddaa 
														WHERE 	
															ddaa.alu_id = dda.alu_id
															AND ddaa.dis_id = dda.dis_id
															AND ddaa.tpc_id IS NOT NULL
															AND ddaa.atd_frequenciaFinalAjustada IS NOT NULL)), '100') END AS frequenciaFinalAjustada, 
			CAST(CAST(0 AS DECIMAL(5,0)) AS VARCHAR) AS frequenciaAnual,
			ISNULL(CAST(Dr.totalFaltasRegencia AS VARCHAR),'0') AS totalFaltasRegencia,
			ISNULL(CAST(Dr.totalAusenciasCompensadasRegencia AS VARCHAR),'0') AS totalAusenciasCompensadasRegencia,
			CASE 
				WHEN ISNULL(Dr.totalAulasRegencia, 0) = 0 OR Da.tud_tipo NOT IN (11,12,13) THEN '100'
			ELSE
				CAST(CAST(ISNULL(Dr.frequenciaFinalAjustadaRegencia,100) AS DECIMAL(5,0)) AS VARCHAR)
			END AS frequenciaFinalAjustadaRegencia,
			par.mtu_resultadoDescricao,
			possuiFreqExterna AS PossuiFreqExternaAtual,
			@possuiFreqExterna AS possuiFreqExterna
		FROM
			@dadosAlunos Da
		LEFT JOIN @dadosAlunosAnuais Daa
			ON Daa.alu_id = Da.alu_id
			AND Daa.tud_id = Da.tud_id
		LEFT JOIN @dadosRegencia Dr
			ON Dr.alu_id = Da.alu_id
		LEFT JOIN @Parecer par
			ON par.alu_id = Da.alu_id
		LEFT JOIN @tbAlunosSituacao tas
			ON tas.alu_id = Da.alu_id
			AND tas.mtu_id = Da.mtu_id	
		LEFT JOIN movimentacao mov
			ON Da.alu_id = mov.alu_id
			AND Da.mtu_id = mov.mtu_id	
		ORDER BY 
			mtu_numeroChamada,
			pes_nome,
			dis_nome,
			Tpc_Agrupamento		
	END
END
GO
PRINT N'Altering [dbo].[NEW_MTR_MatriculaTurmaDisciplina_SelectBy_TurmaDisciplinaFormato_Final]'
GO

-- ========================================================================
-- Author:	  Marcia Haga
-- Create date: 04/09/2014
-- Description: Retorna os alunos matriculados na Turma, de acordo com as regras necessárias 
--			  para ele aparecer na listagem para efetivar da avaliacao Final.
-- Alterado: Marcia Haga - 12/09/2014
-- Description: Adicionado dados referentes a avaliação do último período.
--		      Adicionada validação se as notas dos bimestres em que o aluno esteve na escola foram fechadas.
-- Alterado: Marcia Haga - 17/09/2014
-- Description: Alterado para retornar sempre a frequencia final ajustada do ultimo período
--			  na avaliação final.

-- Alterado: Marcia Haga - 19/09/2014
-- Description: Retornando o mtu_resultado, para marcar o check no registro do conselho 
-- de classe nos casos em que a aba do parecer conclusivo aparece no pop-up.

-- Alterado: Marcia Haga - 22/09/2014
-- Description: Corrigido o filtro dos alunos por período para não retornar registros duplicados.

-- Alterado: Marcia Haga - 29/09/2014
-- Description: Retornando apenas os alunos presentes no fechamento do ultimo bimestre.

-- Alterado: Marcia Haga - 30/09/2014
-- Description: Alterado para retornar as notas dos períodos em que o aluno estava 
-- presente em outra turma ou escola.

-- Alterado: Katiusca Murari - 06/10/2014
-- Description: Adicionada a variação na hora de calcular a frequencia.

---- Alterado: Marcia Haga - 08/10/2014
---- Description: Corrigida validação de aluno presente no período da avaliação.

---- Alterado: Marcia Haga - 09/10/2014
---- Description: Alterado para retornar se o aluno estava fora da rede durante o período.

---- Alterado: Marcia Haga - 28/11/2014
---- Description: Corrigido retorno quando existe movimentacao de aluno na mesma turma.

---- Alterado: Daniel Jun Suguimoto - 03/12/2014
---- Description: Alterado para considerar lançamento de notas no listão para habilitar o fechamento final.

---- Alterado: Marcia Haga - 30/03/2015
---- Description: Alterado para retornar aluno fora da rede se nao possuir matricula no bimestre, 
---- independente de possuir nota ou nao.

---- Alterado: Marcia Haga - 14/04/2015
---- Description: Adicionada validacao se existe nota lancada no Listao para o ultimo periodo.
---- Alterado para trazer o numero de aulas, faltas e compensacoes do ultimo periodo.

---- Alterado: Marcia Haga - 30/04/2015
---- Description: Corrigida validacao de aluno fora da rede.

---- Alterado: Marcia Haga - 04/05/2015
---- Description: Corrigido retorno das faltas de reposicao para o calculo da frequencia final ajustada,
---- pois estava retornando registros duplicados.

---- Alterado: Marcia Haga - 10/08/2015
---- Description: Alterado para verificar o periodo em que o aluno esteve 
---- presente na turma eletiva de aluno ou multisseriada.
-- ========================================================================
ALTER PROCEDURE [dbo].[NEW_MTR_MatriculaTurmaDisciplina_SelectBy_TurmaDisciplinaFormato_Final]
	@tud_id BIGINT
	, @tur_id BIGINT
	, @ava_id INT
	, @ordenacao INT
	, @fav_id INT
	, @tipoEscalaDisciplina TINYINT
	, @tipoEscalaDocente TINYINT
	, @tur_tipo TINYINT
	, @cal_id INT
	, @tipoLancamento TINYINT	
	, @fav_calculoQtdeAulasDadas TINYINT
	, @tipoDocente TINYINT
	, @permiteAlterarResultado BIT
	, @dtTurma TipoTabela_Turma READONLY
	, @documentoOficial BIT
AS
BEGIN
    SET TRANSACTION ISOLATION LEVEL SNAPSHOT
	DECLARE @escolaId INT;
	SELECT TOP 1 @escolaId = esc_id
	FROM TUR_Turma -- WITH (NOLOCK)
	WHERE tur_id = @tur_id

	DECLARE @ultimoPeriodo INT;
	SELECT TOP 1 @ultimoPeriodo = tpc_id 
	FROM ACA_CalendarioPeriodo -- WITH (NOLOCK)
	WHERE 
		cal_id = @cal_id AND cap_situacao <> 3 
	ORDER BY cap_dataFim DESC
		
	DECLARE @MatriculasBoletimDaTurma AS TipoTabela_MatriculasBoletim;

	DECLARE @Matriculas TABLE (alu_id BIGINT NOT NULL, mtu_id INT NOT NULL, mtd_id INT NOT NULL, tur_id BIGINT, tpc_id INT, tpc_ordem INT, tud_id BIGINT, fav_id INT
		, registroExterno BIT, PossuiSaidaPeriodo BIT, esc_id INT, tds_id INT, mtd_numeroChamadaDocente INT NULL
		, mtd_situacaoDocente TINYINT NULL, mtd_dataMatriculaDocente DATE NULL, mtd_dataSaidaDocente DATE NULL
		, PRIMARY KEY (alu_id, mtu_id, mtd_id, tpc_id));

	DECLARE @MatriculaMultisseriadaTurmaAluno TABLE 
		(
			tud_idDocente BIGINT, 
			alu_id BIGINT, 
			mtu_id INT, 
			mtd_id INT
			PRIMARY KEY (tud_idDocente, alu_id, mtu_id, mtd_id)
		);

	DECLARE @tds_id INT = 
		(
			 --Busca o tipo de disciplina para filtrar os mtds abaixo.
			SELECT Dis.tds_id
			FROM TUR_TurmaDisciplinaRelDisciplina RelDis -- WITH (NOLOCK)
			INNER JOIN ACA_Disciplina Dis -- WITH (NOLOCK)
				ON Dis.dis_id = RelDis.dis_id
			WHERE
				RelDis.tud_id = @tud_id
		)

	--Se for turma de eletiva do aluno, carrega os alunos que devem aparecer na tela de efetivação
	IF ( @tur_tipo IN (2,3) ) BEGIN
		-- Turma eletiva de aluno ou multisseriada, buscar matrículas por aluno.
		DECLARE @tbMatriculaAlunos TipoTabela_AlunoMatriculaTurma;
		INSERT INTO @tbMatriculaAlunos (alu_id, mtu_id)
		SELECT Mtd.alu_id, MAX(Mtd.mtu_id)
		FROM MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
		WHERE
			-- Busca mtus dos alunos pelo MTD - o mtd está ligado ao tud informado, e o mtu_id aponta pra outra turma (turma normal).
			Mtd.tud_id = @tud_id
			AND Mtd.mtd_situacao <> 3
		GROUP BY alu_id	
		
		INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
			PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
			PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
		Select mb.alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mb.mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes 
		  from MTR_MatriculasBoletim mb  -- WITH (NOLOCK)
			   inner join @tbMatriculaAlunos amt on amt.alu_id = mb.alu_id and amt.mtu_id = mb.mtu_origemDados
		
		IF not exists (select top 1 alu_id from @MatriculasBoletimDaTurma)
		begin
			INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
				PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
				PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
			EXEC NEW_MTR_MatriculaTurma_MatriculasBoletim_Alunos
				@tbMatriculaTurma = @tbMatriculaAlunos
		end
	END
	ELSE IF (@tur_tipo = 4)
	BEGIN
		-- Turma eletiva de aluno ou multisseriada, buscar matrículas por aluno.
		DECLARE @tbMatriculaAlunosMultisseriada TipoTabela_AlunoMatriculaTurma;
		INSERT INTO @tbMatriculaAlunosMultisseriada (alu_id, mtu_id)
		SELECT Mtd.alu_id, MAX(Mtd.mtu_id)
		FROM MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
		INNER JOIN MTR_MatriculaTurma mtu
			ON Mtd.alu_id = mtu.alu_id
			AND Mtd.mtu_id = mtu.mtu_id
			AND mtu.mtu_situacao <> 3
		INNER JOIN @dtTurma dtt
			ON mtu.tur_id = dtt.tur_id
		WHERE
			-- Busca mtus dos alunos pelo MTD - o mtd está ligado ao tud informado, e o mtu_id aponta pra outra turma (turma normal).
			Mtd.tud_id = @tud_id
			AND Mtd.mtd_situacao <> 3
		GROUP BY mtd.alu_id	
		
		INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
		Select mb.alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mb.mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes 
		  from MTR_MatriculasBoletim mb  -- WITH (NOLOCK)
			   inner join @tbMatriculaAlunosMultisseriada amt on amt.alu_id = mb.alu_id and amt.mtu_id = mb.mtu_origemDados
		
		IF not exists (select top 1 alu_id from @MatriculasBoletimDaTurma)
		begin
			INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
				PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
				PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
			EXEC NEW_MTR_MatriculaTurma_MatriculasBoletim_Alunos
				@tbMatriculaTurma = @tbMatriculaAlunosMultisseriada
		end
		
		INSERT INTO @MatriculaMultisseriadaTurmaAluno (tud_idDocente, alu_id, mtu_id, mtd_id)
		SELECT 
			mtdDocente.tud_id AS tud_idDocente,
			mtdAluno.alu_id,
			mtdAluno.mtu_id,
			mtdAluno.mtd_id
		FROM @MatriculasBoletimDaTurma mtr
		INNER JOIN MTR_MatriculaTurmaDisciplina mtdDocente
			ON mtr.alu_id = mtdDocente.alu_id
			AND mtr.mtu_id = mtdDocente.mtu_id
			AND mtdDocente.tud_id = @tud_id
			AND mtdDocente.mtd_situacao <> 3
		INNER JOIN MTR_MatriculaTurmaDisciplina mtdAluno
			ON mtdAluno.alu_id = mtr.alu_id
			AND mtdAluno.mtu_id = mtr.mtu_id
			AND mtdAluno.mtd_situacao <> 3
		INNER JOIN TUR_TurmaDisciplina tudAluno
			ON mtdAluno.tud_id = tudAluno.tud_id
			AND tudAluno.tud_id <> @tud_id
			AND tudAluno.tud_situacao <> 3
		INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDisAluno
			ON RelDisAluno.tud_id = tudAluno.tud_id
		INNER JOIN ACA_Disciplina disAluno
			ON RelDisAluno.dis_id = disAluno.dis_id
			AND disAluno.tds_id = @tds_id
			AND disAluno.dis_situacao <> 3
		GROUP BY
			mtdDocente.tud_id,
			mtdAluno.alu_id,
			mtdAluno.mtu_id,
			mtdAluno.mtd_id
	END
	 --Se for turma normal, carrega os alunos de acordo com o boletim
	ELSE
	BEGIN
		INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
			PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
			PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
		select mb.alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mb.mtu_id,mb.tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mb.mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes
		  from MTR_MatriculasBoletim mb -- WITH (NOLOCK)
			   inner join (select alu_id, mtu_id, ROW_NUMBER() OVER(PARTITION BY alu_id 
														   ORDER BY mtu_id desc) as linha
							 from MTR_MatriculaTurma -- WITH (NOLOCK) 
							where mtu_situacao <> 3
							  and tur_id = @tur_id) mtu 
					   on mtu.alu_id = mb.alu_id and mtu.mtu_id = mb.mtu_origemDados
		 where mtu.linha = 1 --para evitar casos onde o aluno tem dois mtus para o mesmo tur_id
		   
		IF not exists (select top 1 alu_id from @MatriculasBoletimDaTurma)
		begin
		INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
			PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
			PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
		EXEC NEW_MTR_MatriculaTurma_MatriculasBoletim_DaTurma
			@tur_id = @tur_id;
		end
	END	

	IF (@tur_tipo = 4)
	BEGIN
		INSERT INTO @Matriculas (alu_id, mtu_id, mtd_id, fav_id, tpc_id, tpc_ordem, tud_id, tur_id, registroExterno, PossuiSaidaPeriodo, esc_id, tds_id,
								 mtd_numeroChamadaDocente, mtd_situacaoDocente, mtd_dataMatriculaDocente, mtd_dataSaidaDocente)
		SELECT
			Mtr.alu_id, Mtr.mtu_id, Mtd.mtd_id
			, CASE WHEN @tur_tipo = 1 THEN Mtr.fav_id ELSE @fav_id END AS fav_id
			, Mtr.tpc_id, Mtr.tpc_ordem, Mtd.tud_id, Mtr.tur_id
			, Mtr.registroExterno, Mtr.PossuiSaidaPeriodo, Mtr.esc_id, Dis.tds_id 
			, mtdDocente.mtd_numeroChamada
			, mtdDocente.mtd_situacao
			, mtdDocente.mtd_dataMatricula
			, mtdDocente.mtd_dataSaida
		FROM @MatriculasBoletimDaTurma Mtr
		INNER JOIN MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
			ON Mtd.alu_id = Mtr.alu_id
			AND Mtd.mtu_id = Mtr.mtu_id
			AND Mtd.mtd_situacao <> 3
		INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDis -- WITH (NOLOCK)
			ON RelDis.tud_id = Mtd.tud_id
		INNER JOIN ACA_Disciplina Dis -- WITH (NOLOCK)
			ON RelDis.dis_id = Dis.dis_id	
		INNER JOIN @MatriculaMultisseriadaTurmaAluno tdm 
			ON Mtd.alu_id = tdm.alu_id
			AND Mtd.mtu_id = tdm.mtu_id
			AND Mtd.mtd_id = tdm.mtd_id
		INNER JOIN MTR_MatriculaTurmaDisciplina mtdDocente
			ON mtdDocente.alu_id = Mtd.alu_id
			AND mtdDocente.mtu_id = Mtd.mtu_id
			AND mtdDocente.tud_id = tdm.tud_idDocente
			AND mtdDocente.mtd_situacao <> 3
		WHERE
			Mtr.mtu_id IS NOT NULL
			 --Busca a matrícula na mtd ligada à disciplina (pelo tds_id), em cada COC, da lista de mtus (MatriculasBoletim).
			AND Dis.tds_id = @tds_id
			 --Filtros de matrícula.
			AND Mtr.MesmoCalendario = 1
			AND Mtr.PeriodosEquivalentes = 1
	END
	ELSE
	BEGIN
		INSERT INTO @Matriculas (alu_id, mtu_id, mtd_id, fav_id, tpc_id, tpc_ordem, tud_id, tur_id, registroExterno, PossuiSaidaPeriodo, esc_id, tds_id)
		SELECT
			Mtr.alu_id, Mtr.mtu_id, Mtd.mtd_id
			, CASE WHEN @tur_tipo = 1 THEN Mtr.fav_id ELSE @fav_id END AS fav_id
			, Mtr.tpc_id, Mtr.tpc_ordem, Mtd.tud_id, Mtr.tur_id
			, Mtr.registroExterno, Mtr.PossuiSaidaPeriodo, Mtr.esc_id, Dis.tds_id
		FROM @MatriculasBoletimDaTurma Mtr
		INNER JOIN MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
			ON Mtd.alu_id = Mtr.alu_id
			AND Mtd.mtu_id = Mtr.mtu_id
			AND Mtd.mtd_situacao <> 3
		INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDis -- WITH (NOLOCK)
			ON RelDis.tud_id = Mtd.tud_id
		INNER JOIN ACA_Disciplina Dis -- WITH (NOLOCK)
			ON RelDis.dis_id = Dis.dis_id	
		WHERE
			Mtr.mtu_id IS NOT NULL
			 --Busca a matrícula na mtd ligada à disciplina (pelo tds_id), em cada COC, da lista de mtus (MatriculasBoletim).
			AND Dis.tds_id = @tds_id
			 --Filtros de matrícula.
			AND Mtr.MesmoCalendario = 1
			AND Mtr.PeriodosEquivalentes = 1
	END

	-- Verifica o periodo em que o aluno esteve presente na turma eletiva de aluno ou multisseriada
	IF ( @tur_tipo IN (2,3,4) ) 
	BEGIN
		;WITH PresencaAlunoPeriodo AS
		(
			SELECT Mat.alu_id, Mat.mtu_id, Mat.mtd_id, Mat.tpc_id 
			FROM @Matriculas Mat
			INNER JOIN TUR_Turma Tur -- WITH (NOLOCK)
				ON Tur.tur_id = Mat.tur_id
			INNER JOIN MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
				ON Mtd.alu_id = Mat.alu_id
				AND Mtd.mtu_id = Mat.mtu_id
				AND Mtd.mtd_id = Mat.mtd_id
			INNER JOIN ACA_TipoPeriodoCalendario Tpc -- WITH (NOLOCK)
				ON Tpc.tpc_id = Mat.tpc_id
			INNER JOIN ACA_CalendarioPeriodo Cap -- WITH (NOLOCK)
				ON Cap.tpc_id = Tpc.tpc_id
				AND Cap.cal_id = Tur.cal_id
				AND Cap.cap_situacao <> 3
			WHERE
			(
				-- O aluno nao estava presente no periodo se:
				-- o aluno saiu durante o periodo
				Mtd.mtd_dataSaida BETWEEN Cap.cap_dataInicio AND Cap.cap_dataFim
				-- ou o aluno saiu antes de o periodo iniciar
				OR Mtd.mtd_dataSaida < Cap.cap_dataInicio
				-- ou o aluno entrou depois do fim do periodo
				OR Mtd.mtd_dataMatricula > Cap.cap_dataFim
			)
			AND Mat.PossuiSaidaPeriodo = 0
		)
		UPDATE @Matriculas
		SET PossuiSaidaPeriodo = 1
		FROM @Matriculas Mat
		INNER JOIN PresencaAlunoPeriodo Pap
			ON Pap.alu_id = Mat.alu_id
			AND Pap.mtu_id = Mat.mtu_id
			AND Pap.mtd_id = Mat.mtd_id
			AND Pap.tpc_id = Mat.tpc_id
	END

	-- Notas e frequencia que ja foram fechadas
	DECLARE @Fechado TABLE (alu_id BIGINT NOT NULL, mtu_id INT NOT NULL, mtd_id INT NOT NULL
							, atd_id INT NOT NULL, fav_id INT NOT NULL, ava_id INT NOT NULL
							, atd_avaliacao VARCHAR(20), atd_frequencia DECIMAL(5,2)
							, atd_relatorio VARCHAR(MAX), arq_idRelatorio BIGINT
							, atd_avaliacaoPosConselho VARCHAR(20), atd_frequenciaFinalAjustada DECIMAL(5,2)
							, tpc_id INT, atd_numeroAulas INT
			, PRIMARY KEY (alu_id, mtu_id, mtd_id, atd_id));	
	INSERT INTO @Fechado
	SELECT 
		atd.alu_id
		, atd.mtu_id
		, atd.mtd_id
		, atd_id
		, atd.fav_id
		, atd.ava_id
		, atd_avaliacao
		, atd_frequencia
		, atd_relatorio
		, arq_idRelatorio
		, atd_avaliacaoPosConselho
		, atd_frequenciaFinalAjustada
		, ava.tpc_id
		, atd.atd_numeroAulas
	FROM @Matriculas m
		INNER JOIN CLS_AlunoAvaliacaoTurmaDisciplina atd -- WITH (NOLOCK) 
			ON atd.tud_id = m.tud_id
			AND atd.alu_id = m.alu_id
			AND atd.mtu_id = m.mtu_id
			AND atd.mtd_id = m.mtd_id
			AND atd.fav_id = m.fav_id
		INNER JOIN ACA_Avaliacao ava -- WITH (NOLOCK) 
			ON ava.fav_id = atd.fav_id 
			AND ava.ava_id = atd.ava_id
			AND (ava.tpc_id = m.tpc_id OR ava.tpc_id IS NULL)
			AND ava.ava_situacao <> 3
	WHERE
		atd.tud_id = @tud_id
		AND (ava.ava_tipo IN (1, 5) -- periodica, periodica + final
			OR (ava.ava_tipo = 3 AND ava.ava_id = @ava_id)) --  final
		AND ava.fav_id = @fav_id
		AND atd_situacao <> 3
	------------------------------------------------------------
	-- Fechado em outras turmas
	UNION	
	SELECT 
		m.alu_id
		, m.mtu_id
		, m.mtd_id
		, atd_id
		, atd.fav_id
		, atd.ava_id
		, atd_avaliacao
		, atd_frequencia
		, atd_relatorio
		, arq_idRelatorio
		, atd_avaliacaoPosConselho
		, atd_frequenciaFinalAjustada
		, m.tpc_id
		, atd.atd_numeroAulas
	FROM @Matriculas m
		INNER JOIN CLS_AlunoAvaliacaoTurmaDisciplina atd -- WITH (NOLOCK) 
			ON atd.tud_id = m.tud_id
			AND atd.alu_id = m.alu_id
			AND atd.mtu_id = m.mtu_id
			AND atd.mtd_id = m.mtd_id
			AND atd.fav_id = m.fav_id
		INNER JOIN ACA_Avaliacao ava -- WITH (NOLOCK) 
			ON ava.fav_id = atd.fav_id 
			AND ava.ava_id = atd.ava_id
			AND ava.tpc_id = m.tpc_id
			AND ava.ava_situacao <> 3
	WHERE
		(m.tur_id <> @tur_id
			OR m.tud_id <> @tud_id)
		AND ava.ava_tipo IN (1, 5) -- periodica, periodica + final
		AND atd_situacao <> 3
	------------------------------------------------------------	
	
	--********************
	-- Se o ultimo periodo ainda nao foi fechado, 
	-- carregar os dados para salvar junto com o fechamento final.
	DECLARE @tbFrequenciaAlunos TABLE (
		alu_id BIGINT
		, mtu_id INT
		, mtd_id INT
		, Frequencia DECIMAL (27, 2) 
		, QtFaltasAluno INT
		, QtAulasAluno INT
		, FrequenciaAcumulada DECIMAL(5,2)
		, ausenciasCompensadas INT
		, FrequenciaFinalAjustada DECIMAL (27, 2) 
	)

	DECLARE @tbAlunosSemFechamentoUltimoPeriodo TABLE (
		alu_id BIGINT
		, mtu_id INT
		, mtd_id INT
		, PRIMARY KEY (alu_id, mtu_id, mtd_id)
	)
	INSERT INTO @tbAlunosSemFechamentoUltimoPeriodo (alu_id, mtu_id, mtd_id)
	SELECT m.alu_id, m.mtu_id, m.mtd_id 
	FROM @Matriculas m
	LEFT JOIN @Fechado f 
		ON f.alu_id = m.alu_id 
		AND f.mtu_id = m.mtu_id
		AND f.mtd_id = m.mtd_id
		AND f.tpc_id = m.tpc_id
	WHERE 
	m.tpc_id = @ultimoPeriodo
	AND f.alu_id IS NULL

	IF (EXISTS ( SELECT TOP 1 alu_id FROM @tbAlunosSemFechamentoUltimoPeriodo ))	
	BEGIN				

		DECLARE @tud_tipo TINYINT;
		SELECT
			@tud_tipo = tud_tipo
		FROM
			TUR_TurmaDisciplina -- WITH (NOLOCK)
		WHERE
			tud_id = @tud_id
			AND tud_situacao <> 3

		-- Armazena exibir compensacao ausencia cadastrada
		DECLARE @ExibeCompensacao BIT
		SELECT TOP 1
			@ExibeCompensacao = CASE WHEN (pac_valor = 'True') THEN 1 ELSE 0 END
		FROM
			ACA_ParametroAcademico -- WITH (NOLOCK)
		WHERE
			pac_chave = 'EXIBIR_COMPENSACAO_AUSENCIA_CADASTRADA'
	        
		DECLARE @AulasCompensadas TABLE (
			tud_id BIGINT NOT NULL,
			alu_id BIGINT NOT NULL,
			mtu_id INT NOT NULL,
			mtd_id INT NOT NULL,
			qtdCompensadas INT NULL,
			PRIMARY KEY (tud_id, alu_id, mtu_id, mtd_id)
		)
			
		DECLARE @TabelaQtdes TABLE (
			alu_id BIGINT NOT NULL,
			mtu_id INT NOT NULL,
			mtd_id INT NOT NULL,
			qtFaltas INT NULL,
			qtAulas INT NULL,
			qtFaltasReposicao INT NULL
			PRIMARY KEY (alu_id, mtu_id, mtd_id)
		)
		
		DECLARE @SomatorioAulasFaltas TABLE (alu_id BIGINT NOT NULL, aulas INT, faltas INT, faltasReposicao INT, compensadas INT);

		-- Compensacoes de ausencia do ultimo periodo
		INSERT INTO @AulasCompensadas(tud_id, alu_id, mtu_id, mtd_id, qtdCompensadas)	
			SELECT 
				caa.tud_id
				,caa.alu_id
				,caa.mtu_id
				,caa.mtd_id
				,SUM(ISNULL(cpa.cpa_quantidadeAulasCompensadas, 0)) as qtdCompensadas
			FROM CLS_CompensacaoAusencia cpa -- WITH (NOLOCK)
			INNER JOIN CLS_CompensacaoAusenciaAluno caa -- WITH (NOLOCK)
				ON  caa.tud_id = cpa.tud_id
				AND caa.cpa_id = cpa.cpa_id
				AND caa.caa_situacao = 1
			WHERE
				cpa.tud_id = @tud_id
				AND cpa.tpc_id = @ultimoPeriodo
				AND cpa.cpa_situacao = 1 
			GROUP BY
				caa.tud_id
				,caa.alu_id
				,caa.mtu_id
				,caa.mtd_id
		
		IF (@tur_tipo = 4)
		BEGIN
			-- Faltas e aulas do ultimo periodo
			INSERT INTO @TabelaQtdes(alu_id, mtu_id, mtd_id, qtAulas, qtFaltas, qtFaltasReposicao)				
			SELECT 
				tdm.alu_id, 
				tdm.mtu_id, 
				tdm.mtd_id, 
				SUM(qtAulas)  OVER (PARTITION BY tdm.alu_id) AS qtAulas,
				SUM(qtFaltas) OVER (PARTITION BY tdm.alu_id) AS qtFaltas,
				SUM(qtFaltasReposicao) OVER (PARTITION BY tdm.alu_id) AS qtFaltasReposicao
			FROM 
				@MatriculaMultisseriadaTurmaAluno tdm
				CROSS APPLY FN_Select_FaltasAulasBy_TurmaDisciplina(@tipoLancamento, @ultimoPeriodo, @tud_id, @fav_calculoQtdeAulasDadas, @tipoDocente) faltas
			WHERE
				tdm.alu_id = faltas.alu_id
				AND tdm.mtu_id = faltas.mtu_id
				AND tdm.mtd_id = faltas.mtd_id
		END
		ELSE
		BEGIN
			-- Faltas e aulas do ultimo periodo
			INSERT INTO @TabelaQtdes(alu_id, mtu_id, mtd_id, qtAulas, qtFaltas, qtFaltasReposicao)				
			SELECT 
				faltas.alu_id, 
				faltas.mtu_id, 
				faltas.mtd_id, 
				faltas.qtAulas, 
				faltas.qtFaltas,
				faltas.qtFaltasReposicao
			FROM 
				FN_Select_FaltasAulasBy_TurmaDisciplina(@tipoLancamento, @ultimoPeriodo, @tud_id, @fav_calculoQtdeAulasDadas, @tipoDocente) faltas
		END
		-- So faz o calculo do somatorio, se tiver frequencia final ajustada
		--IF (@ExibeCompensacao = 1)
		--BEGIN
			IF (@tud_tipo = 15)
			BEGIN
				;WITH TabelaPeriodosAnteriores AS (
					SELECT 
						tpc.tpc_id, 
						ava.ava_id, 
						ava.fav_id 
					FROM dbo.ACA_TipoPeriodoCalendario AS tpc -- WITH (NOLOCK) 
					INNER JOIN dbo.ACA_Avaliacao AS ava -- WITH (NOLOCK)
						ON ava.fav_id = @fav_id
						AND tpc.tpc_id = ava.tpc_id	
						AND ava.ava_situacao <> 3	
					WHERE tpc_ordem <= (
						SELECT tpc_ordem FROM dbo.ACA_TipoPeriodoCalendario -- WITH (NOLOCK) 
						WHERE tpc_id = @ultimoPeriodo
					)
				)
				INSERT INTO @SomatorioAulasFaltas (alu_id, faltas, faltasReposicao, aulas, compensadas)
				SELECT 
					mat.alu_id,
					SUM(CASE WHEN @ultimoPeriodo = mat.tpc_id
							THEN ISNULL(tfa.qtfaltas,0)
							ELSE ISNULL(atd.atd_numeroFaltas,0)
					END) AS faltas,
					SUM(ISNULL(tfa.qtFaltasReposicao,0)) AS faltasReposicao,
					SUM(CASE WHEN @ultimoPeriodo = mat.tpc_id
							THEN ISNULL(tfa.qtAulas,0)
							ELSE  ISNULL(atd.atd_numeroAulas,0)
						END) AS aulas,
					SUM(CASE WHEN @ultimoPeriodo = mat.tpc_id 
							THEN ISNULL(cpa.qtdCompensadas,0)
							ELSE ISNULL(atd.atd_ausenciasCompensadas,0)
					END) AS compensadas
				FROM TUR_TurmaDisciplinaMultisseriada tdm -- WITH (NOLOCK)
				INNER JOIN @tbAlunosSemFechamentoUltimoPeriodo AS alu
					ON alu.alu_id = tdm.alu_id
				INNER JOIN @Matriculas AS mat
					ON mat.alu_id = alu.alu_id
				INNER JOIN TabelaPeriodosAnteriores tpa
					ON tpa.tpc_id = mat.tpc_id	
				LEFT JOIN CLS_AlunoAvaliacaoTurmaDisciplina Atd -- WITH (NOLOCK)
					ON  Atd.tud_id = mat.tud_id
					AND Atd.alu_id = mat.alu_id
					AND Atd.mtu_id = mat.mtu_id
					AND Atd.mtd_id = mat.mtd_id
					AND atd.fav_id = tpa.fav_id
					AND atd.ava_id = tpa.ava_id
					AND Atd.atd_situacao <> 3
				LEFT JOIN @TabelaQtdes tfa 
					ON  mat.alu_id = tfa.alu_id
					AND mat.tpc_id = @ultimoPeriodo
				LEFT JOIN @AulasCompensadas Cpa
					ON Cpa.alu_id = mat.alu_id
					AND Cpa.mtu_id = mat.mtu_id
					AND Cpa.mtd_id = mat.mtd_id
					AND mat.tpc_id = @ultimoPeriodo      
				GROUP BY mat.alu_id
			END
			ELSE 
			BEGIN
				;WITH TabelaPeriodosAnteriores AS (
					SELECT 
						tpc.tpc_id, 
						ava.ava_id, 
						ava.fav_id 
					FROM dbo.ACA_TipoPeriodoCalendario AS tpc -- WITH (NOLOCK) 
					INNER JOIN dbo.ACA_Avaliacao AS ava -- WITH (NOLOCK)
						ON ava.fav_id = @fav_id
						AND tpc.tpc_id = ava.tpc_id	
						AND ava.ava_situacao <> 3	
					WHERE tpc_ordem <= (
						SELECT tpc_ordem FROM dbo.ACA_TipoPeriodoCalendario -- WITH (NOLOCK) 
						WHERE tpc_id = @ultimoPeriodo
					)
				)
				INSERT INTO @SomatorioAulasFaltas (alu_id, faltas, faltasReposicao, aulas, compensadas)
				SELECT 
					mat.alu_id,
					SUM(CASE WHEN @ultimoPeriodo = mat.tpc_id
							THEN ISNULL(tfa.qtfaltas,0)
							ELSE ISNULL(atd.atd_numeroFaltas,0)
					END) AS faltas,
					SUM(ISNULL(tfa.qtFaltasReposicao,0)) AS faltasReposicao,
					SUM(CASE WHEN @ultimoPeriodo = mat.tpc_id
							THEN ISNULL(tfa.qtAulas,0)
							ELSE  ISNULL(atd.atd_numeroAulas,0)
						END) AS aulas,
					SUM(CASE WHEN @ultimoPeriodo = mat.tpc_id 
							THEN ISNULL(cpa.qtdCompensadas,0)
							ELSE ISNULL(atd.atd_ausenciasCompensadas,0)
					END) AS compensadas
				FROM @tbAlunosSemFechamentoUltimoPeriodo AS alu 
				INNER JOIN @Matriculas AS mat
					ON mat.alu_id = alu.alu_id
				INNER JOIN TabelaPeriodosAnteriores tpa
					ON tpa.tpc_id = mat.tpc_id	
				LEFT JOIN CLS_AlunoAvaliacaoTurmaDisciplina Atd -- WITH (NOLOCK)
					ON  Atd.tud_id = mat.tud_id
					AND Atd.alu_id = mat.alu_id
					AND Atd.mtu_id = mat.mtu_id
					AND Atd.mtd_id = mat.mtd_id
					AND atd.fav_id = tpa.fav_id
					AND atd.ava_id = tpa.ava_id
					AND Atd.atd_situacao <> 3
				LEFT JOIN @TabelaQtdes tfa 
					ON  mat.alu_id = tfa.alu_id
					AND mat.tpc_id = @ultimoPeriodo
				LEFT JOIN @AulasCompensadas Cpa
					ON Cpa.alu_id = mat.alu_id
					AND Cpa.mtu_id = mat.mtu_id
					AND Cpa.mtd_id = mat.mtd_id
					AND mat.tpc_id = @ultimoPeriodo  
				GROUP BY mat.alu_id
			END
		--END
		
		INSERT INTO @tbFrequenciaAlunos
		SELECT 
			alu.alu_id,
			alu.mtu_id,
			alu.mtd_id
			, ISNULL(dbo.FN_Calcula_PorcentagemFrequenciaVariacao(Qtd.qtAulas, Qtd.qtFaltas, FAV.fav_variacao), 0) AS Frequencia
			-- Qtde. de faltas
			, ISNULL(Qtd.qtFaltas, 0) AS QtFaltasAluno
			-- Qtde. de aulas
			, ISNULL(Qtd.qtAulas, 0) AS QtAulasAluno
			, CAST(/*ISNULL(TabelaFrequenciaFinal.frequenciaFinal, 0)*/ 0 AS DECIMAL(5,2)) AS FrequenciaAcumulada
			, ISNULL(ac.qtdCompensadas, 0) AS ausenciasCompensadas 
			, (CASE WHEN (@ExibeCompensacao = 1)
				THEN 
					dbo.FN_Calcula_PorcentagemFrequenciaVariacao(ISNULL(saf.aulas,0), ((ISNULL(saf.faltas,0) + ISNULL(saf.faltasReposicao,0)) - ISNULL(saf.compensadas,0)), FAV.fav_variacao)
				ELSE 
					dbo.FN_Calcula_PorcentagemFrequenciaVariacao(ISNULL(saf.aulas,0), ((ISNULL(saf.faltas,0) + ISNULL(saf.faltasReposicao,0))), FAV.fav_variacao)
			END) AS FrequenciaFinalAjustada
		FROM @tbAlunosSemFechamentoUltimoPeriodo AS alu
		INNER JOIN @TabelaQtdes AS Qtd
			ON alu.alu_id = Qtd.alu_id
			AND alu.mtu_id = Qtd.mtu_id
			AND alu.mtd_id = Qtd.mtd_id
		INNER JOIN ACA_FormatoAvaliacao FAV -- WITH (NOLOCK)
			ON FAV.fav_id = @fav_id
		LEFT JOIN @AulasCompensadas ac 
			ON ac.alu_id = alu.alu_id
			AND ac.mtu_id = alu.mtu_id
			AND ac.mtd_id = alu.mtd_id
		LEFT JOIN @SomatorioAulasFaltas saf		
			ON saf.alu_id = alu.alu_id			  
	END
	--********************	

	/* [Carla 19/07/2013]
		Excluir os registros que não devem ser exibidos na tela. Esses registros devem ser trazidos para buscar
		a nota do bimestre anterior, caso seja recuperação.
	*/
	DELETE FROM @Matriculas
	WHERE
		registroExterno = 1
		-- Se possuir uma saída no período, não exibe o aluno.
		OR PossuiSaidaPeriodo = 1
		
	; WITH TabelaMovimentacao AS (
			--Selecina as movimentações que possuem matrícula anterior
			SELECT
				alu_id,
				mtu_idAnterior,
				tmv_nome    
			FROM
				MTR_Movimentacao MOV -- WITH (NOLOCK) 
				INNER JOIN ACA_TipoMovimentacao TMV -- WITH (NOLOCK) 
					ON MOV.tmv_idSaida = TMV.tmv_id
			WHERE
				mov_situacao NOT IN (3,4)
				AND tmv_situacao <> 3
				AND mtu_idAnterior IS NOT NULL	
	)
	, avaliacoes AS (
		SELECT 
			ava.tpc_id
			, ava.ava_nome
			, cap.cap_dataInicio AS cap_dataInicio
			, cap.cap_dataFim AS cap_dataFim
			, ava.ava_id
		FROM ACA_Avaliacao ava -- WITH (NOLOCK)
		LEFT JOIN ACA_CalendarioPeriodo cap -- WITH (NOLOCK) 
			ON cap.tpc_id = ava.tpc_id
			AND cap.cal_id = @cal_id
			AND cap.cap_situacao <> 3
		WHERE
			(ava.ava_tipo IN (1, 5) -- periodica, periodica + final
				OR (ava.ava_tipo = 3 AND ava.ava_id = @ava_id)) --  final
			AND ava.fav_id = @fav_id
			AND ava_situacao <> 3
	)
	, TabelaObservacaoConselho AS 
	(
		SELECT
			tur_id
			, alu_id
			, mtu_id
			, max(CASE WHEN qtdeQualidade = 0 AND qtdeDesempenhos = 0 AND qtdeRecomendacao = 0
						AND ato_qualidade IS NULL AND ato_desempenhoAprendizado IS NULL 
						AND ato_recomendacaoAluno IS NULL AND ato_recomendacaoResponsavel IS NULL
				   THEN 0
				   ELSE 1
			  END) AS observacaoPreenchida
		FROM
		(
			SELECT
				Mtr.tur_id
				, Mtr.alu_id
				, Mtr.mtu_id
				, SUM(CASE WHEN aaq.tqa_id IS NULL THEN 0 ELSE 1 END) AS qtdeQualidade
 				, SUM(CASE WHEN aad.tda_id IS NULL THEN 0 ELSE 1 END) AS qtdeDesempenhos
 				, SUM(CASE WHEN aar.rar_id IS NULL THEN 0 ELSE 1 END) AS qtdeRecomendacao
				, ato_qualidade
				, ato_desempenhoAprendizado
				, ato_recomendacaoAluno
				, ato_recomendacaoResponsavel
			FROM
				@Matriculas Mtr
				INNER JOIN ACA_Avaliacao ava -- WITH (NOLOCK)
					ON Mtr.fav_id = ava.fav_id
					AND ava.tpc_id = @ultimoPeriodo
					AND ava.ava_exibeObservacaoConselhoPedagogico = 1
				LEFT JOIN CLS_AlunoAvaliacaoTurmaQualidade aaq -- WITH (NOLOCK)
					ON  Mtr.tur_id = aaq.tur_id
					AND Mtr.alu_id = aaq.alu_id
					AND Mtr.mtu_id = aaq.mtu_id
					AND aaq.fav_id = ava.fav_id
					AND aaq.ava_id = ava.ava_id
				LEFT JOIN CLS_AlunoAvaliacaoTurmaDesempenho aad -- WITH (NOLOCK)
					ON  Mtr.tur_id = aad.tur_id
					AND Mtr.alu_id = aad.alu_id
					AND Mtr.mtu_id = aad.mtu_id
					AND aad.fav_id = ava.fav_id
					AND aad.ava_id = ava.ava_id 
				LEFT JOIN CLS_AlunoAvaliacaoTurmaRecomendacao aar -- WITH (NOLOCK)
					ON  Mtr.tur_id = aar.tur_id
					AND Mtr.alu_id = aar.alu_id
					AND Mtr.mtu_id = aar.mtu_id
					AND aar.fav_id = ava.fav_id
					AND aar.ava_id = ava.ava_id	        
				LEFT JOIN CLS_ALunoAvaliacaoTurmaObservacao ato -- WITH (NOLOCK)
					ON Mtr.tur_id = ato.tur_id
					AND Mtr.alu_id = ato.alu_id
					AND Mtr.mtu_id = ato.mtu_id
					AND ato.fav_id = ava.fav_id
					AND ato.ava_id = ava.ava_id
					AND ato.ato_situacao <> 3		
			WHERE
				Mtr.tud_id = @tud_id
			GROUP BY
				Mtr.tur_id
				, Mtr.alu_id
				, Mtr.mtu_id
				, ato_qualidade
				, ato_desempenhoAprendizado
				, ato_recomendacaoAluno
				, ato_recomendacaoResponsavel
		) 
		AS tabela
		GROUP BY --(Adicionado group by por Webber) 
			tabela.tur_id
			, tabela.alu_id 
			, tabela.mtu_id 
			--, CASE WHEN tabela.qtdeQualidade = 0 AND tabela.qtdeDesempenhos = 0 AND tabela.qtdeRecomendacao = 0
			--			AND tabela.ato_qualidade IS NULL AND tabela.ato_desempenhoAprendizado IS NULL 
			--			AND tabela.ato_recomendacaoAluno IS NULL AND tabela.ato_recomendacaoResponsavel IS NULL
			--	   THEN 0
			--	   ELSE 1
			--  END		
	)
	, movimentacao AS (

			--Selecina as movimentações que possuem matrícula anterior
			SELECT
				MOV.alu_id,
				mtu_idAnterior,
				CASE WHEN tmo_tipoMovimento IN (6, 12, 14, 15, 16)
					 THEN TMV.tmv_nome + ' em ' + REPLACE(CONVERT(VARCHAR(10), mov.mov_dataRealizacao, 103), '/' + CAST(DATEPART(YEAR, mov.mov_dataRealizacao) AS VARCHAR(4)), '') +
					 	  ISNULL(' p/ ' + turD.tur_codigo, '')
					 WHEN tmo_tipoMovimento IN (8)
					 THEN TMV.tmv_nome + ' p/' + ISNULL(' ' + turD.tur_codigo, '')
					 WHEN tmo_tipoMovimento IN (11)
					 THEN TMV.tmv_nome + ' p/ ' + ISNULL(' ' + turD.tur_codigo, '')
					 ELSE TMV.tmv_nome
				END tmv_nome  
			FROM
				@Matriculas res
				INNER JOIN MTR_Movimentacao MOV -- WITH (NOLOCK) 
					ON res.alu_id = MOV.alu_id 
				INNER JOIN ACA_TipoMovimentacao TMV -- WITH (NOLOCK) 
					ON MOV.tmv_idSaida = TMV.tmv_id
				LEFT JOIN MTR_TipoMovimentacao tmo -- WITH(NOLOCK)
					ON mov.tmo_id = tmo.tmo_id
					AND tmo.tmo_situacao <> 3
				LEFT JOIN MTR_MatriculaTurma mtuD -- WITH(NOLOCK)
					ON mov.alu_id = mtuD.alu_id
					AND mov.mtu_idAtual = mtuD.mtu_id
				LEFT JOIN TUR_Turma turD -- WITH(NOLOCK)
					ON mtuD.tur_id = turD.tur_id
				LEFT JOIN ACA_CalendarioAnual calD -- WITH(NOLOCK)
					ON turD.cal_id = calD.cal_id
				INNER JOIN MTR_MatriculaTurma mtuO -- WITH(NOLOCK)
					ON mov.alu_id = mtuO.alu_id
					AND mov.mtu_idAnterior = mtuO.mtu_id
					AND mtuO.tur_id = @tur_id
				LEFT JOIN TUR_Turma turO -- WITH(NOLOCK)
					ON mtuO.tur_id = turO.tur_id
				LEFT JOIN ACA_CalendarioAnual calO -- WITH(NOLOCK)
					ON turO.cal_id = calO.cal_id
			WHERE
				mov_situacao NOT IN (3,4)
				AND tmv_situacao <> 3
				AND mtu_idAnterior IS NOT NULL	
	)
	, tbRetorno AS (	
		SELECT
			  Mtd.alu_id
			, Mtd.mtu_id
			, Mtd.mtd_id
			, alc.alc_matricula
			, F.atd_id AS AvaliacaoID
			, CASE WHEN atd_id IS NULL 
					THEN atm.atm_media
					ELSE ISNULL(F.atd_avaliacaoPosConselho, F.atd_avaliacao)
				END AS Avaliacao
			, CASE WHEN @permiteAlterarResultado = 0 
					THEN NULL
					-- Caso contrário, traz o resultado normalmente
					ELSE Mtd.mtd_resultado 
				END AS AvaliacaoResultado	
			, CASE WHEN F.atd_id IS NULL 
					THEN FM.Frequencia 
					ELSE F.atd_frequencia 
				END AS Frequencia
			, CASE WHEN @documentoOficial = 1 THEN Pes.pes_nomeOficial ELSE Pes.pes_nome END + 
				(
					CASE WHEN ( ISNULL(Mtr.mtd_situacaoDocente, Mtd.mtd_situacao) = 5 ) 
						THEN ISNULL((SELECT TOP 1 ' (' + tmv_nome + ')' FROM movimentacao MOV -- WITH (NOLOCK)
									 WHERE MOV.mtu_idAnterior = Mtd.mtu_id AND MOV.alu_id = Mtd.alu_id), ' (Inativo)')
						ELSE '' 
					END
				) 
				AS pes_nome
			, Pes.pes_dataNascimento
			, CASE WHEN ISNULL(Mtr.mtd_numeroChamadaDocente, Mtd.mtd_numeroChamada) > 0 
					THEN CAST(ISNULL(Mtr.mtd_numeroChamadaDocente, Mtd.mtd_numeroChamada) AS VARCHAR)
					ELSE '-' 
				END AS mtd_numeroChamada
			, ISNULL(Mtr.mtd_numeroChamadaDocente, Mtd.mtd_numeroChamada) AS mtd_numeroChamadaordem
			, CAST(Mtd.alu_id AS VARCHAR) + ';' + 
				CAST(Mtd.mtd_id AS VARCHAR) + ';' + 
				CAST(Mtd.mtu_id AS VARCHAR) 
				AS id
			, ISNULL(Mtr.mtd_situacaoDocente, Mtd.mtd_situacao) AS situacaoMatriculaAluno
			, F.atd_relatorio
			, F.arq_idRelatorio
			,ISNULL(Mtr.mtd_dataMatriculaDocente, Mtd.mtd_dataMatricula) AS dataMatricula
			, ISNULL(Mtr.mtd_dataSaidaDocente, Mtd.mtd_dataSaida) AS dataSaida
			, ISNULL(CASE WHEN F.atd_id IS NULL 
				THEN FM.FrequenciaFinalAjustada 
				ELSE F.atd_frequenciaFinalAjustada END, 0) AS FrequenciaFinalAjustada
			, ava.tpc_id
			, ava.ava_nome AS NomeAvaliacao
			, F.atd_avaliacaoPosConselho AS AvaliacaoPosConselho
			, ava.cap_dataInicio
			, CAST(ISNULL(toc.observacaoPreenchida, 0) AS BIT) AS observacaoConselhoPreenchida
			-- se o aluno nao teve a nota efetivada no periodo,
			-- mas ele estava presente no periodo
			-- deve-se informar o usuario.
			, CAST(CASE WHEN 
					(  
						(
							COALESCE(F.atd_avaliacaoPosConselho, F.atd_avaliacao, '') <> ''
							OR
							(								
								-- se for o ultimo periodo,
								-- e nao tiver fechamento
								-- deve ter a nota do Listao
								ISNULL(ava.tpc_id, 0) = @ultimoPeriodo
								AND F.atd_id IS NULL
								AND ISNULL(atm.atm_media,'') <> ''
							)
						)
						AND ISNULL(tud.tud_naoLancarNota, 0) = 0
					)
					OR 
					(
						ISNULL(tud.tud_naoLancarNota, 0) = 1
						AND ISNULL(F.atd_id,0) > 0
					)
					THEN 1
					ELSE 0
			   END AS BIT) AS PossuiNota
			, ava.ava_id AS ava_id
			, CASE WHEN (ava.tpc_id IS NOT NULL AND ava.tpc_id = @ultimoPeriodo) 
					THEN 1 
					ELSE 0 
				END AS UltimoPeriodo
			, FM.QtFaltasAluno
			, FM.QtAulasAluno
			, FM.ausenciasCompensadas
			, mtu.mtu_resultado
			, CASE WHEN (
					-- aluno estava presente na rede no periodo da avaliacao
					EXISTS (
						SELECT alu_id
						FROM @Matriculas
						WHERE alu_id = Mtr.alu_id
						AND tpc_id = ava.tpc_id
					)
				) THEN
					(
						CASE WHEN (
							-- aluno estava presente na escola no periodo da avaliacao
							EXISTS (
								SELECT alu_id
								FROM @Matriculas
								WHERE alu_id = Mtr.alu_id
								AND esc_id = @escolaId
								AND tpc_id = ava.tpc_id
							)
						) 
						THEN 1 -- Aluno presente na escola
						ELSE 2 -- Aluno em outra escola		
						END
					)						
				ELSE 0 -- Aluno fora da rede
				END AS PresencaAluno
			, F.atd_numeroAulas AS QtAulasEfetivado
			, ISNULL(tpc.tpc_ordem, 0) AS tpc_ordem
		FROM @Matriculas Mtr
		INNER JOIN MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
			ON  Mtd.alu_id = Mtr.alu_id
			AND Mtd.mtu_id = Mtr.mtu_id
			AND Mtd.mtd_id = Mtr.mtd_id
			AND Mtd.tud_id = @tud_id
		INNER JOIN TUR_TurmaDisciplina tud -- WITH (NOLOCK)
			ON Mtd.tud_id = tud.tud_id
			AND tud.tud_situacao <> 3
		INNER JOIN MTR_MatriculaTurma mtu -- WITH (NOLOCK)
			ON mtu.alu_id = Mtd.alu_id
			AND mtu.mtu_id = Mtd.mtu_id
			AND mtu_situacao <> 3    
		INNER JOIN ACA_AlunoCurriculo alc -- WITH (NOLOCK)
			ON alc.alu_id = mtu.alu_id
			AND alc.alc_id = mtu.alc_id
			AND alc.alc_situacao <> 3	
		INNER JOIN ACA_Aluno Alu -- WITH (NOLOCK)
			ON  Mtd.alu_id   = Alu.alu_id
			AND alu_situacao <> 3
		INNER JOIN VW_DadosAlunoPessoa Pes -- WITH (NOLOCK)
			ON  Alu.alu_id   = Pes.alu_id
		INNER JOIN avaliacoes ava 
			ON 1 = 1    
		LEFT JOIN ACA_TipoPeriodoCalendario tpc -- WITH (NOLOCK)
			ON ava.tpc_id = tpc.tpc_id
			AND tpc.tpc_situacao <> 3    
		LEFT JOIN @Fechado F 
			ON (F.alu_id = Mtd.alu_id 
					AND F.mtu_id = Mtd.mtu_id 
					AND F.mtd_id = Mtd.mtd_id
					AND ((F.tpc_id IS NULL AND ava.tpc_id IS NULL) OR F.tpc_id = ava.tpc_id)
				)
				------------------------------------------------------------
				-- Fechado em outras turmas
				OR (F.alu_id = Mtd.alu_id 
					AND F.mtu_id <> Mtd.mtu_id
					AND F.tpc_id = ava.tpc_id
				)	
				------------------------------------------------------------	  		
		LEFT JOIN @tbFrequenciaAlunos FM
			ON FM.alu_id = Mtd.alu_id
			AND FM.mtu_id = Mtd.mtu_id
			AND FM.mtd_id = Mtd.mtd_id
			AND ava.tpc_id = @ultimoPeriodo			
		LEFT JOIN CLS_AlunoAvaliacaoTurmaDisciplinaMedia atm -- WITH (NOLOCK)
			ON atm.alu_id = Mtd.alu_id
			AND atm.mtu_id = Mtd.mtu_id
			AND atm.mtd_id = Mtd.mtd_id
			AND atm.tud_id = Mtd.tud_id
			AND atm.tpc_id = ava.tpc_id
			AND atm.tpc_id = @ultimoPeriodo	 
			AND atm.atm_situacao <> 3			
		LEFT JOIN TabelaObservacaoConselho toc
			ON 
			--toc.tur_id = Mtu.tur_id AND (Comentado aqui por Webber)
			toc.alu_id = Mtu.alu_id
			AND toc.mtu_id = Mtu.mtu_id	
	        
		WHERE 
			Mtr.tpc_id = @ultimoPeriodo
			AND ISNULL(Mtr.mtd_situacaoDocente, mtd_situacao) IN (1,5)
			AND COALESCE(Mtr.mtd_numeroChamadaDocente, mtd_numeroChamada, 0) >= 0		
	)
	, tbRetornoUltimoPeriodo AS
	(
		SELECT alu_id, mtu_id, mtd_id, FrequenciaFinalAjustada
		FROM tbRetorno
		WHERE UltimoPeriodo = 1
	)	
	, tbFinal AS 
	(
		SELECT 
			@tur_id AS tur_id
			, @tud_id AS tud_id
			, r.alu_id
			, r.mtu_id
			, r.mtd_id
			, alc_matricula
			, AvaliacaoID
			, Avaliacao
			, AvaliacaoResultado		
			, Frequencia
			, pes_nome	
			, pes_dataNascimento	
			, mtd_numeroChamada
			, id
			, atd_relatorio
			, arq_idRelatorio
			, situacaoMatriculaAluno
			, dataMatricula
			, dataSaida
			-- Se for a avaliação final, pego a frequencia final ajustada do ultimo periodo
			, CASE WHEN (tpc_id IS NULL)
				THEN Up.FrequenciaFinalAjustada
				ELSE r.FrequenciaFinalAjustada
				END AS FrequenciaFinalAjustada
			, ISNULL(tpc_id, -1) AS tpc_id
			, NomeAvaliacao
			, AvaliacaoPosConselho
			, observacaoConselhoPreenchida
			-- Valida o fechamento apenas se o aluno estava 
			-- presente na escola no periodo da avaliação
			, CASE WHEN PresencaAluno = 1 THEN PossuiNota ELSE 1 END AS PossuiNota
			, ava_id
			, UltimoPeriodo
			, QtFaltasAluno
			, QtAulasAluno
			, ausenciasCompensadas
			, mtu_resultado
			, CASE WHEN PresencaAluno = 0 AND ISNULL(tpc_id, -1) > 0 THEN 1 ELSE 0 END AS AlunoForaDaRede
			, QtAulasEfetivado
			, cap_dataInicio
			, mtd_numeroChamadaordem
			, tpc_ordem
		FROM tbRetorno r
		LEFT JOIN tbRetornoUltimoPeriodo Up 
			ON Up.alu_id = r.alu_id 
			AND Up.mtu_id = r.mtu_id 
			AND Up.mtd_id = r.mtd_id	
		GROUP BY
			cap_dataInicio
			, tpc_id
			, NomeAvaliacao
			, r.alu_id
			, r.mtu_id
			, r.mtd_id
			, alc_matricula
			, AvaliacaoID
			, Avaliacao
			, AvaliacaoResultado		
			, Frequencia
			, pes_nome		
			, pes_dataNascimento
			, mtd_numeroChamada
			, mtd_numeroChamadaordem
			, id
			, atd_relatorio
			, arq_idRelatorio
			, situacaoMatriculaAluno
			, dataMatricula
			, dataSaida
			, r.FrequenciaFinalAjustada
			, Up.FrequenciaFinalAjustada
			, AvaliacaoPosConselho
			, observacaoConselhoPreenchida
			, ava_id
			, UltimoPeriodo
			, QtFaltasAluno
			, QtAulasAluno
			, ausenciasCompensadas
			, mtu_resultado
			, PossuiNota
			, PresencaAluno
			, QtAulasEfetivado
			, tpc_ordem
	)	
	SELECT
		tur_id
		, tud_id
		, alu_id
		, mtu_id
		, mtd_id
		, alc_matricula
		, AvaliacaoID
		, Avaliacao
		, AvaliacaoResultado		
		, Frequencia
		, pes_nome		
		, ISNULL(CAST(pes_dataNascimento AS VARCHAR(10)), '') AS pes_dataNascimento
		, mtd_numeroChamada
		, id
		, atd_relatorio
		, arq_idRelatorio
		, situacaoMatriculaAluno
		, dataMatricula
		, dataSaida
		, FrequenciaFinalAjustada
		, tpc_id
		, NomeAvaliacao
		, AvaliacaoPosConselho
		, observacaoConselhoPreenchida
		, CASE WHEN AlunoForaDaRede = 1 OR PossuiNota = 1 THEN 0 ELSE 1 END AS SemNota
		, ava_id
		, UltimoPeriodo
		, QtFaltasAluno
		, QtAulasAluno
		, ausenciasCompensadas
		, mtu_resultado
		, AlunoForaDaRede
		, QtAulasEfetivado
		, tpc_ordem
	FROM
		tbFinal
	ORDER BY 
		cap_dataInicio
		, tpc_id
		, ava_id
		, CASE 
			WHEN @ordenacao = 0 THEN 
				CASE WHEN ISNULL(mtd_numeroChamadaordem,0) <= 0 THEN 1 ELSE 0 END
		END ASC
		, CASE WHEN @ordenacao = 0 THEN ISNULL(mtd_numeroChamadaordem,0) END ASC
		, CASE WHEN @ordenacao = 1 THEN pes_nome END ASC	
END

GO
PRINT N'Creating [dbo].[STP_ACA_AlunoJustificativaFaltaAnexo_UPDATE]'
GO


CREATE PROCEDURE [dbo].[STP_ACA_AlunoJustificativaFaltaAnexo_UPDATE]
	@alu_id BIGINT
	, @afj_id INT
	, @aja_id INT
	, @arq_id BIGINT
	, @aja_descricao VARCHAR (500)
	, @aja_situacao TINYINT
	, @aja_dataCriacao DATETIME
	, @aja_dataAlteracao DATETIME

AS
BEGIN
	UPDATE ACA_AlunoJustificativaFaltaAnexo 
	SET 
		arq_id = @arq_id 
		, aja_descricao = @aja_descricao 
		, aja_situacao = @aja_situacao 
		, aja_dataCriacao = @aja_dataCriacao 
		, aja_dataAlteracao = @aja_dataAlteracao 

	WHERE 
		alu_id = @alu_id 
		AND afj_id = @afj_id 
		AND aja_id = @aja_id 

		
	DECLARE @ret INT
	SELECT @ret = ISNULL(@@ROWCOUNT,-1)
	RETURN @ret
	
END


GO
PRINT N'Creating [dbo].[NEW_ACA_TipoCiclo_SelecionarAtivosEscolaAno]'
GO

-- ========================================================================
-- Author:		Leonardo Brito
-- Create date: 27/03/2017
-- Description:	Retorna os tipos de ciclo de aprendizagem ativos de cursos ligados à escola
-- ========================================================================
CREATE PROCEDURE [dbo].[NEW_ACA_TipoCiclo_SelecionarAtivosEscolaAno]
	@cal_ano INT
	, @tds_id INT
	, @esc_id INT
	, @uad_idSuperior UNIQUEIDENTIFIER
AS
BEGIN

	IF (ISNULL(@esc_id, 0) > 0)
	BEGIN
		SELECT	  
			 tci.tci_id  
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
 			dbo.ACA_TipoCiclo tci WITH(NOLOCK)
		INNER JOIN ACA_CurriculoPeriodo crp WITH(NOLOCK)
			ON tci.tci_id = crp.tci_id
			AND crp.crp_situacao <> 3
		INNER JOIN ACA_TipoCurriculoPeriodo tcrp WITH(NOLOCK)
			ON crp.tcp_id = tcrp.tcp_id
			AND tcrp.tcp_situacao <> 3
		INNER JOIN ACA_CurriculoEscola ces WITH(NOLOCK)
			ON ces.cur_id = crp.cur_id
			AND ces.crr_id = crp.crr_id
			AND ces.esc_id = @esc_id
			AND ces.ces_situacao <> 3
		INNER JOIN ACA_ObjetoAprendizagemTipoCiclo oat WITH(NOLOCK)
			ON tci.tci_id = oat.tci_id
		INNER JOIN ACA_ObjetoAprendizagem oap WITH(NOLOCK)
			ON oat.oap_id = oap.oap_id
			AND oap.tds_id = @tds_id
			AND oap.cal_ano = @cal_ano
			AND oap.oap_situacao < >3
		WHERE 
			tci_situacao = 1
		GROUP BY
			tci.tci_id  
			, tci_nome
			, tci_situacao 
			, tci_dataCriacao 
			, tci_dataAlteracao 		
  			, tci_exibirBoletim
			, tci_ordem
			, tci_objetoAprendizagem
		ORDER BY tci_ordem, tci_nome
	END
	ELSE IF (@uad_idSuperior IS NOT NULL)
	BEGIN
		SELECT	  
			 tci.tci_id  
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
 			dbo.ACA_TipoCiclo tci WITH(NOLOCK)
		INNER JOIN ACA_CurriculoPeriodo crp WITH(NOLOCK)
			ON tci.tci_id = crp.tci_id
			AND crp.crp_situacao <> 3
		INNER JOIN ACA_TipoCurriculoPeriodo tcrp WITH(NOLOCK)
			ON crp.tcp_id = tcrp.tcp_id
			AND tcrp.tcp_situacao <> 3
		INNER JOIN ACA_CurriculoEscola ces WITH(NOLOCK)
			ON ces.cur_id = crp.cur_id
			AND ces.crr_id = crp.crr_id
			AND ces.ces_situacao <> 3
		INNER JOIN ESC_Escola esc WITH(NOLOCK)
			ON ces.esc_id = esc.esc_id
			AND esc.esc_situacao <> 3
		INNER JOIN Synonym_SYS_UnidadeAdministrativa uad WITH(NOLOCK)
			ON esc.uad_id = uad.uad_id 
			AND uad.uad_situacao <> 3
		INNER JOIN ACA_ObjetoAprendizagemTipoCiclo oat WITH(NOLOCK)
			ON tci.tci_id = oat.tci_id
		INNER JOIN ACA_ObjetoAprendizagem oap WITH(NOLOCK)
			ON oat.oap_id = oap.oap_id
			AND oap.tds_id = @tds_id
			AND oap.cal_ano = @cal_ano
			AND oap.oap_situacao < >3
		WHERE 
			tci_situacao = 1
			AND COALESCE(esc.uad_idSuperiorGestao, uad.uad_idSuperior, @uad_idSuperior) = @uad_idSuperior
		GROUP BY
			tci.tci_id  
			, tci_nome
			, tci_situacao 
			, tci_dataCriacao 
			, tci_dataAlteracao 		
  			, tci_exibirBoletim
			, tci_ordem
			, tci_objetoAprendizagem
		ORDER BY tci_ordem, tci_nome
	END
	ELSE
	BEGIN
		SELECT	  
			 tci.tci_id  
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
 			dbo.ACA_TipoCiclo tci WITH(NOLOCK)
		INNER JOIN ACA_ObjetoAprendizagemTipoCiclo oat WITH(NOLOCK)
			ON tci.tci_id = oat.tci_id
		INNER JOIN ACA_ObjetoAprendizagem oap WITH(NOLOCK)
			ON oat.oap_id = oap.oap_id
			AND oap.tds_id = @tds_id
			AND oap.cal_ano = @cal_ano
			AND oap.oap_situacao < >3
		WHERE 
			tci_situacao = 1
		GROUP BY
			tci.tci_id  
			, tci_nome
			, tci_situacao 
			, tci_dataCriacao 
			, tci_dataAlteracao 		
  			, tci_exibirBoletim
			, tci_ordem
			, tci_objetoAprendizagem
		ORDER BY tci_ordem, tci_nome
	END
END


GO
PRINT N'Creating [dbo].[NEW_ACA_AlunoJustificativaFaltaAnexo_UPDATE_Situacao]'
GO


CREATE PROCEDURE [dbo].[NEW_ACA_AlunoJustificativaFaltaAnexo_UPDATE_Situacao]
	@alu_id BIGINT
	, @afj_id INT
	, @aja_id INT
	, @aja_situacao TINYINT
	, @aja_dataAlteracao DATETIME

AS
BEGIN
	UPDATE ACA_AlunoJustificativaFaltaAnexo 
	SET 
		aja_situacao = @aja_situacao 
		, aja_dataAlteracao = @aja_dataAlteracao 

	WHERE 
		alu_id = @alu_id 
		AND afj_id = @afj_id 
		AND aja_id = @aja_id 

		
	DECLARE @ret INT
	SELECT @ret = ISNULL(@@ROWCOUNT,-1)
	RETURN @ret
	
END


GO
PRINT N'Altering [dbo].[NEW_MTR_MatriculaTurmaDisciplina_SelectBy_TurmaDisciplinaFormatoFiltroDeficiencia_Final]'
GO
-- Stored Procedure

-- ========================================================================
-- Author:	  Marcia Haga
-- Create date: 04/09/2014
-- Description: Retorna os alunos matriculados na Turma, de acordo com as regras necessárias 
--			  para ele aparecer na listagem para efetivar da avaliacao Final.
--			  Filtrando os alunos com ou sem deficiência, dependendo do docente.
-- Alterado: Marcia Haga - 12/09/2014
-- Description: Adicionado dados referentes a avaliação do último período.
--		      Adicionada validação se as notas dos bimestres em que o aluno esteve na escola foram fechadas.
-- Alterado: Marcia Haga - 17/09/2014
-- Description: Alterado para retornar sempre a frequencia final ajustada do ultimo período
--			  na avaliação final.

-- Alterado: Marcia Haga - 19/09/2014
-- Description: Retornando o mtu_resultado, para marcar o check no registro do conselho 
-- de classe nos casos em que a aba do parecer conclusivo aparece no pop-up.

-- Alterado: Marcia Haga - 22/09/2014
-- Description: Corrigido o filtro dos alunos por período para não retornar registros duplicados.

-- Alterado: Marcia Haga - 29/09/2014
-- Description: Retornando apenas os alunos presentes no fechamento do ultimo bimestre.

-- Alterado: Marcia Haga - 30/09/2014
-- Description: Alterado para retornar as notas dos períodos em que o aluno estava 
-- presente em outra turma ou escola.

-- Alterado: Katiusca Murari - 06/10/2014
-- Description: Adicionada a variação na hora de calcular a frequencia.

---- Alterado: Marcia Haga - 08/10/2014
---- Description: Corrigida validação de aluno presente no período da avaliação.

---- Alterado: Marcia Haga - 09/10/2014
---- Description: Alterado para retornar se o aluno estava fora da rede durante o período.

---- Alterado: Marcia Haga - 04/11/2014
---- Description: Alterado para retornar a frequencia final ajustada.

---- Alterado: Marcia Haga - 28/11/2014
---- Description: Corrigido retorno quando existe movimentacao de aluno na mesma turma.

---- Alterado: Daniel Jun Suguimoto - 03/12/2014
---- Description: Alterado para considerar lançamento de notas no listão para habilitar o fechamento final.

---- Alterado: Marcia Haga - 30/03/2015
---- Description: Alterado para retornar aluno fora da rede se nao possuir matricula no bimestre, 
---- independente de possuir nota ou nao.

---- Alterado: Marcia Haga - 14/04/2015
---- Description: Adicionada validacao se existe nota lancada no Listao para o ultimo periodo.
---- Alterado para trazer o numero de aulas, faltas e compensacoes do ultimo periodo.

---- Alterado: Marcia Haga - 30/04/2015
---- Description: Corrigida validacao de aluno fora da rede.

---- Alterado: Marcia Haga - 04/05/2015
---- Description: Corrigido retorno das faltas de reposicao para o calculo da frequencia final ajustada,
---- pois estava retornando registros duplicados.

---- Alterado: Marcia Haga - 10/08/2015
---- Description: Alterado para verificar o periodo em que o aluno esteve 
---- presente na turma eletiva de aluno ou multisseriada.
-- ========================================================================
ALTER PROCEDURE [dbo].[NEW_MTR_MatriculaTurmaDisciplina_SelectBy_TurmaDisciplinaFormatoFiltroDeficiencia_Final]
	@tud_id BIGINT
	, @tur_id BIGINT
	, @ava_id INT
	, @ordenacao INT
	, @fav_id INT
	, @tipoEscalaDisciplina TINYINT
	, @tipoEscalaDocente TINYINT
	, @tur_tipo TINYINT
	, @cal_id INT
	, @tipoLancamento TINYINT	
	, @fav_calculoQtdeAulasDadas TINYINT
	, @tipoDocente TINYINT
	, @permiteAlterarResultado BIT
	, @dtTurma TipoTabela_Turma READONLY
	, @documentoOficial BIT
AS
BEGIN

	SET TRANSACTION ISOLATION LEVEL SNAPSHOT

	DECLARE @escolaId INT;
	SELECT TOP 1 @escolaId = esc_id
	FROM TUR_Turma -- WITH(NOLOCK)
	WHERE tur_id = @tur_id

	DECLARE @ultimoPeriodo INT;
	SELECT TOP 1 @ultimoPeriodo = tpc_id 
	FROM ACA_CalendarioPeriodo -- WITH(NOLOCK)
	WHERE 
		cal_id = @cal_id AND cap_situacao <> 3 
	ORDER BY cap_dataFim DESC

	DECLARE @MatriculasBoletimDaTurma AS TipoTabela_MatriculasBoletim;

	DECLARE @Matriculas TABLE (alu_id BIGINT NOT NULL, mtu_id INT NOT NULL, mtd_id INT NOT NULL, tur_id BIGINT, tpc_id INT, tpc_ordem INT, tud_id BIGINT, fav_id INT
		, registroExterno BIT, PossuiSaidaPeriodo BIT, esc_id INT, mtd_numeroChamadaDocente INT NULL
		, mtd_situacaoDocente TINYINT NULL, mtd_dataMatriculaDocente DATE NULL, mtd_dataSaidaDocente DATE NULL
		, PRIMARY KEY (alu_id, mtu_id, mtd_id, tpc_id));

	DECLARE @tds_id INT = 
		(
			 --Busca o tipo de disciplina para filtrar os mtds abaixo.
			SELECT Dis.tds_id
			FROM TUR_TurmaDisciplinaRelDisciplina RelDis -- WITH(NOLOCK)
			INNER JOIN ACA_Disciplina Dis -- WITH(NOLOCK)
				ON Dis.dis_id = RelDis.dis_id
			WHERE
				RelDis.tud_id = @tud_id
		)

	DECLARE @MatriculaMultisseriadaTurmaAluno TABLE 
		(
			tud_idDocente BIGINT, 
			alu_id BIGINT, 
			mtu_id INT, 
			mtd_id INT
			PRIMARY KEY (tud_idDocente, alu_id, mtu_id, mtd_id)
		);

	--Se for turma de eletiva do aluno, carrega os alunos que devem aparecer na tela de efetivação
	IF ( @tur_tipo IN (2,3) ) BEGIN
		-- Turma eletiva de aluno ou multisseriada, buscar matrículas por aluno.
		DECLARE @tbMatriculaAlunos TipoTabela_AlunoMatriculaTurma;
		INSERT INTO @tbMatriculaAlunos (alu_id, mtu_id)
		SELECT Mtd.alu_id, MAX(Mtd.mtu_id)
		FROM MTR_MatriculaTurmaDisciplina Mtd -- WITH(NOLOCK)
		WHERE
			-- Busca mtus dos alunos pelo MTD - o mtd está ligado ao tud informado, e o mtu_id aponta pra outra turma (turma normal).
			Mtd.tud_id = @tud_id
			AND Mtd.mtd_situacao <> 3
		GROUP BY alu_id	

		INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
			PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
			PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
		Select mb.alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mb.mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes 
		  from MTR_MatriculasBoletim mb  -- WITH(NOLOCK)
			   inner join @tbMatriculaAlunos amt on amt.alu_id = mb.alu_id and amt.mtu_id = mb.mtu_origemDados

		IF not exists (select top 1 alu_id from @MatriculasBoletimDaTurma)
		begin
			INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
			PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
			PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
			EXEC NEW_MTR_MatriculaTurma_MatriculasBoletim_Alunos
				@tbMatriculaTurma = @tbMatriculaAlunos
		end
	END
	ELSE IF (@tur_tipo = 4)
	BEGIN
		-- Turma eletiva de aluno ou multisseriada, buscar matrículas por aluno.
		DECLARE @tbMatriculaAlunosMultisseriada TipoTabela_AlunoMatriculaTurma;
		INSERT INTO @tbMatriculaAlunosMultisseriada (alu_id, mtu_id)
		SELECT Mtd.alu_id, MAX(Mtd.mtu_id)
		FROM MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
		INNER JOIN MTR_MatriculaTurma mtu
			ON Mtd.alu_id = mtu.alu_id
			AND Mtd.mtu_id = mtu.mtu_id
			AND mtu.mtu_situacao <> 3
		INNER JOIN @dtTurma dtt
			ON mtu.tur_id = dtt.tur_id
		WHERE
			-- Busca mtus dos alunos pelo MTD - o mtd está ligado ao tud informado, e o mtu_id aponta pra outra turma (turma normal).
			Mtd.tud_id = @tud_id
			AND Mtd.mtd_situacao <> 3
		GROUP BY mtd.alu_id	

		INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
			PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
			PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
		Select mb.alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mb.mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes 
		  from MTR_MatriculasBoletim mb  -- WITH(NOLOCK)
			   inner join @tbMatriculaAlunosMultisseriada amt on amt.alu_id = mb.alu_id and amt.mtu_id = mb.mtu_origemDados

		IF not exists (select top 1 alu_id from @MatriculasBoletimDaTurma)
		begin
			INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
			PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
			PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
			EXEC NEW_MTR_MatriculaTurma_MatriculasBoletim_Alunos
				@tbMatriculaTurma = @tbMatriculaAlunosMultisseriada
		end

		INSERT INTO @MatriculaMultisseriadaTurmaAluno (tud_idDocente, alu_id, mtu_id, mtd_id)
		SELECT 
			mtdDocente.tud_id AS tud_idDocente,
			mtdAluno.alu_id,
			mtdAluno.mtu_id,
			mtdAluno.mtd_id
		FROM @MatriculasBoletimDaTurma mtr
		INNER JOIN MTR_MatriculaTurmaDisciplina mtdDocente
			ON mtr.alu_id = mtdDocente.alu_id
			AND mtr.mtu_id = mtdDocente.mtu_id
			AND mtdDocente.tud_id = @tud_id
			AND mtdDocente.mtd_situacao <> 3
		INNER JOIN MTR_MatriculaTurmaDisciplina mtdAluno
			ON mtdAluno.alu_id = mtr.alu_id
			AND mtdAluno.mtu_id = mtr.mtu_id
			AND mtdAluno.mtd_situacao <> 3
		INNER JOIN TUR_TurmaDisciplina tudAluno
			ON mtdAluno.tud_id = tudAluno.tud_id
			AND tudAluno.tud_id <> @tud_id
			AND tudAluno.tud_situacao <> 3
		INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDisAluno
			ON RelDisAluno.tud_id = tudAluno.tud_id
		INNER JOIN ACA_Disciplina disAluno
			ON RelDisAluno.dis_id = disAluno.dis_id
			AND disAluno.tds_id = @tds_id
			AND disAluno.dis_situacao <> 3
		GROUP BY
			mtdDocente.tud_id,
			mtdAluno.alu_id,
			mtdAluno.mtu_id,
			mtdAluno.mtd_id
	END
	 --Se for turma normal, carrega os alunos de acordo com o boletim
	ELSE
	BEGIN
		INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
			PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
			PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
		select mb.alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mb.mtu_id,mb.tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mb.mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes
		  from MTR_MatriculasBoletim mb -- WITH (NOLOCK)
			   inner join (select alu_id, mtu_id, ROW_NUMBER() OVER(PARTITION BY alu_id 
														   ORDER BY mtu_id desc) as linha
							 from MTR_MatriculaTurma -- WITH(NOLOCK) 
							where mtu_situacao <> 3
							  and tur_id = @tur_id) mtu 
					   on mtu.alu_id = mb.alu_id and mtu.mtu_id = mb.mtu_origemDados
		 where mtu.linha = 1 --para evitar casos onde o aluno tem dois mtus para o mesmo tur_id

		IF not exists (select top 1 alu_id from @MatriculasBoletimDaTurma)
		begin
			INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
				PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
				PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
			EXEC NEW_MTR_MatriculaTurma_MatriculasBoletim_DaTurma
				@tur_id = @tur_id;
		end
	END	

	IF (@tur_tipo = 4)
	BEGIN
		INSERT INTO @Matriculas (alu_id, mtu_id, mtd_id, fav_id, tpc_id, tpc_ordem, tud_id, tur_id, registroExterno, PossuiSaidaPeriodo, esc_id,
								 mtd_numeroChamadaDocente, mtd_situacaoDocente, mtd_dataMatriculaDocente, mtd_dataSaidaDocente)
		SELECT
			Mtr.alu_id, Mtr.mtu_id, Mtd.mtd_id, Mtr.fav_id, Mtr.tpc_id, Mtr.tpc_ordem, Mtd.tud_id, Mtr.tur_id
			, Mtr.registroExterno, Mtr.PossuiSaidaPeriodo, Mtr.esc_id
			, mtdDocente.mtd_numeroChamada
			, mtdDocente.mtd_situacao
			, mtdDocente.mtd_dataMatricula
			, mtdDocente.mtd_dataSaida
		FROM @MatriculasBoletimDaTurma Mtr
		INNER JOIN MTR_MatriculaTurmaDisciplina Mtd -- WITH(NOLOCK)
			ON Mtd.alu_id = Mtr.alu_id
			AND Mtd.mtu_id = Mtr.mtu_id
			AND Mtd.mtd_situacao <> 3
		INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDis -- WITH(NOLOCK)
			ON RelDis.tud_id = Mtd.tud_id
		INNER JOIN ACA_Disciplina Dis -- WITH(NOLOCK)
			ON RelDis.dis_id = Dis.dis_id	
		INNER JOIN @MatriculaMultisseriadaTurmaAluno tdm 
			ON Mtd.alu_id = tdm.alu_id
			AND Mtd.mtu_id = tdm.mtu_id
			AND Mtd.mtd_id = tdm.mtd_id
		INNER JOIN MTR_MatriculaTurmaDisciplina mtdDocente
			ON mtdDocente.alu_id = Mtd.alu_id
			AND mtdDocente.mtu_id = Mtd.mtu_id
			AND mtdDocente.tud_id = tdm.tud_idDocente
			AND mtdDocente.mtd_situacao <> 3
		WHERE
			Mtr.mtu_id IS NOT NULL
			 --Busca a matrícula na mtd ligada à disciplina (pelo tds_id), em cada COC, da lista de mtus (MatriculasBoletim).
			AND Dis.tds_id = @tds_id
			 --Filtros de matrícula.
			AND Mtr.MesmoCalendario = 1
			--AND Mtr.PeriodosEquivalentes = 1
	END
	ELSE
	BEGIN
		INSERT INTO @Matriculas (alu_id, mtu_id, mtd_id, fav_id, tpc_id, tpc_ordem, tud_id, tur_id, registroExterno, PossuiSaidaPeriodo, esc_id)
		SELECT
			Mtr.alu_id, Mtr.mtu_id, Mtd.mtd_id, Mtr.fav_id, Mtr.tpc_id, Mtr.tpc_ordem, Mtd.tud_id, Mtr.tur_id
			, Mtr.registroExterno, Mtr.PossuiSaidaPeriodo, Mtr.esc_id
		FROM @MatriculasBoletimDaTurma Mtr
		INNER JOIN MTR_MatriculaTurmaDisciplina Mtd -- WITH(NOLOCK)
			ON Mtd.alu_id = Mtr.alu_id
			AND Mtd.mtu_id = Mtr.mtu_id
			AND Mtd.mtd_situacao <> 3
		INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDis -- WITH(NOLOCK)
			ON RelDis.tud_id = Mtd.tud_id
		INNER JOIN ACA_Disciplina Dis -- WITH(NOLOCK)
			ON RelDis.dis_id = Dis.dis_id	
		WHERE
			Mtr.mtu_id IS NOT NULL
			 --Busca a matrícula na mtd ligada à disciplina (pelo tds_id), em cada COC, da lista de mtus (MatriculasBoletim).
			AND Dis.tds_id = @tds_id
			 --Filtros de matrícula.
			AND Mtr.MesmoCalendario = 1
			--Verifica períodos equivalentes apenas para turmas normais (1)
			AND (Mtr.PeriodosEquivalentes = 1 OR @tur_tipo <> 1)
	END

	-- Verifica o periodo em que o aluno esteve presente na turma eletiva de aluno ou multisseriada
	IF ( @tur_tipo IN (2,3,4) ) 
	BEGIN
		;WITH PresencaAlunoPeriodo AS
		(
			SELECT Mat.alu_id, Mat.mtu_id, Mat.mtd_id, Mat.tpc_id 
			FROM @Matriculas Mat
			INNER JOIN TUR_Turma Tur -- WITH (NOLOCK)
				ON Tur.tur_id = Mat.tur_id
			INNER JOIN MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
				ON Mtd.alu_id = Mat.alu_id
				AND Mtd.mtu_id = Mat.mtu_id
				AND Mtd.mtd_id = Mat.mtd_id
			INNER JOIN ACA_TipoPeriodoCalendario Tpc -- WITH (NOLOCK)
				ON Tpc.tpc_id = Mat.tpc_id
			INNER JOIN ACA_CalendarioPeriodo Cap -- WITH (NOLOCK)
				ON Cap.tpc_id = Tpc.tpc_id
				AND Cap.cal_id = Tur.cal_id
				AND Cap.cap_situacao <> 3
			WHERE
			(
				-- O aluno nao estava presente no periodo se:
				-- o aluno saiu durante o periodo
				Mtd.mtd_dataSaida BETWEEN Cap.cap_dataInicio AND Cap.cap_dataFim
				-- ou o aluno saiu antes de o periodo iniciar
				OR Mtd.mtd_dataSaida < Cap.cap_dataInicio
				-- ou o aluno entrou depois do fim do periodo
				OR Mtd.mtd_dataMatricula > Cap.cap_dataFim
			)
			AND Mat.PossuiSaidaPeriodo = 0
		)
		UPDATE @Matriculas
		SET PossuiSaidaPeriodo = 1
		FROM @Matriculas Mat
		INNER JOIN PresencaAlunoPeriodo Pap
			ON Pap.alu_id = Mat.alu_id
			AND Pap.mtu_id = Mat.mtu_id
			AND Pap.mtd_id = Mat.mtd_id
			AND Pap.tpc_id = Mat.tpc_id
	END

	-- Notas e frequencia que ja foram fechadas
	DECLARE @Fechado TABLE (alu_id BIGINT NOT NULL, mtu_id INT NOT NULL, mtd_id INT NOT NULL
							, atd_id INT NOT NULL, fav_id INT NOT NULL, ava_id INT NOT NULL
							, atd_avaliacao VARCHAR(20), atd_frequencia DECIMAL(5,2)
							, atd_relatorio VARCHAR(MAX), arq_idRelatorio BIGINT
							, atd_avaliacaoPosConselho VARCHAR(20), atd_frequenciaFinalAjustada DECIMAL(5,2)
							, tpc_id INT, atd_numeroAulas INT
			, PRIMARY KEY (alu_id, mtu_id, mtd_id, atd_id));	
	INSERT INTO @Fechado
	SELECT 
		atd.alu_id
		, atd.mtu_id
		, atd.mtd_id
		, atd_id
		, atd.fav_id
		, atd.ava_id
		, atd_avaliacao
		, atd_frequencia
		, atd_relatorio
		, arq_idRelatorio
		, atd_avaliacaoPosConselho
		, atd_frequenciaFinalAjustada
		, ava.tpc_id
		, atd.atd_numeroAulas
	FROM @Matriculas m
		INNER JOIN CLS_AlunoAvaliacaoTurmaDisciplina atd -- WITH(NOLOCK) 
			ON atd.tud_id = m.tud_id
			AND atd.alu_id = m.alu_id
			AND atd.mtu_id = m.mtu_id
			AND atd.mtd_id = m.mtd_id
			AND atd.fav_id = m.fav_id
		INNER JOIN ACA_Avaliacao ava -- WITH(NOLOCK) 
			ON ava.fav_id = atd.fav_id 
			AND ava.ava_id = atd.ava_id
			AND (ava.tpc_id = m.tpc_id OR ava.tpc_id IS NULL)
			AND ava.ava_situacao <> 3
	WHERE
		atd.tud_id = @tud_id
		AND (ava.ava_tipo IN (1, 5) -- periodica, periodica + final
			OR (ava.ava_tipo = 3 AND ava.ava_id = @ava_id)) --  final
		AND ava.fav_id = @fav_id
		AND atd_situacao <> 3		
	------------------------------------------------------------
	-- Fechado em outras turmas
	UNION	
	SELECT 
		m.alu_id
		, m.mtu_id
		, m.mtd_id
		, atd_id
		, atd.fav_id
		, atd.ava_id
		, atd_avaliacao
		, atd_frequencia
		, atd_relatorio
		, arq_idRelatorio
		, atd_avaliacaoPosConselho
		, atd_frequenciaFinalAjustada
		, m.tpc_id
		, atd.atd_numeroAulas
	FROM @Matriculas m
		INNER JOIN CLS_AlunoAvaliacaoTurmaDisciplina atd -- WITH(NOLOCK) 
			ON atd.tud_id = m.tud_id
			AND atd.alu_id = m.alu_id
			AND atd.mtu_id = m.mtu_id
			AND atd.mtd_id = m.mtd_id
			AND atd.fav_id = m.fav_id
		INNER JOIN ACA_Avaliacao ava -- WITH(NOLOCK) 
			ON ava.fav_id = atd.fav_id 
			AND ava.ava_id = atd.ava_id
			AND ava.tpc_id = m.tpc_id
			AND ava.ava_situacao <> 3
	WHERE
		(m.tur_id <> @tur_id
			OR m.tud_id <> @tud_id)
		AND ava.ava_tipo IN (1, 5) -- periodica, periodica + final
		AND atd_situacao <> 3
	------------------------------------------------------------

	--********************
	-- Se o ultimo periodo ainda nao foi fechado, 
	-- carregar os dados para salvar junto com o fechamento final.
	DECLARE @tbFrequenciaAlunos TABLE (
		alu_id BIGINT
		, mtu_id INT
		, mtd_id INT
		, Frequencia DECIMAL (27, 2) 
		, QtFaltasAluno INT
		, QtAulasAluno INT
		, FrequenciaAcumulada DECIMAL(5,2)
		, ausenciasCompensadas INT
		, FrequenciaFinalAjustada DECIMAL (27, 2) 
	)

	DECLARE @tbAlunosSemFechamentoUltimoPeriodo TABLE (
		alu_id BIGINT
		, mtu_id INT
		, mtd_id INT
		, PRIMARY KEY (alu_id, mtu_id, mtd_id)
	)
	INSERT INTO @tbAlunosSemFechamentoUltimoPeriodo (alu_id, mtu_id, mtd_id)
	SELECT m.alu_id, m.mtu_id, m.mtd_id 
	FROM @Matriculas m
	LEFT JOIN @Fechado f 
		ON f.alu_id = m.alu_id 
		AND f.mtu_id = m.mtu_id
		AND f.mtd_id = m.mtd_id
		AND f.tpc_id = m.tpc_id
	WHERE 
	m.tpc_id = @ultimoPeriodo
	AND f.alu_id IS NULL

	IF (EXISTS ( SELECT TOP 1 alu_id FROM @tbAlunosSemFechamentoUltimoPeriodo ))	
	BEGIN				

		DECLARE @tud_tipo TINYINT;
		SELECT
			@tud_tipo = tud_tipo
		FROM
			TUR_TurmaDisciplina -- WITH (NOLOCK)
		WHERE
			tud_id = @tud_id
			AND tud_situacao <> 3

		-- Armazena exibir compensacao ausencia cadastrada
		DECLARE @ExibeCompensacao BIT
		SELECT TOP 1
			@ExibeCompensacao = CASE WHEN (pac_valor = 'True') THEN 1 ELSE 0 END
		FROM
			ACA_ParametroAcademico -- WITH (NOLOCK)
		WHERE
			pac_chave = 'EXIBIR_COMPENSACAO_AUSENCIA_CADASTRADA'

		DECLARE @AulasCompensadas TABLE (
			tud_id BIGINT NOT NULL,
			alu_id BIGINT NOT NULL,
			mtu_id INT NOT NULL,
			mtd_id INT NOT NULL,
			qtdCompensadas INT NULL,
			PRIMARY KEY (tud_id, alu_id, mtu_id, mtd_id)
		)

		DECLARE @TabelaQtdes TABLE (
			alu_id BIGINT NOT NULL,
			mtu_id INT NOT NULL,
			mtd_id INT NOT NULL,
			qtFaltas INT NULL,
			qtAulas INT NULL,
			qtFaltasReposicao INT NULL
			PRIMARY KEY (alu_id, mtu_id, mtd_id)
		)

		DECLARE @SomatorioAulasFaltas TABLE (alu_id BIGINT NOT NULL, aulas INT, faltas INT, faltasReposicao INT, compensadas INT);

		-- Compensacoes de ausencia do ultimo periodo
		INSERT INTO @AulasCompensadas(tud_id, alu_id, mtu_id, mtd_id, qtdCompensadas)	
			SELECT 
				caa.tud_id
				,caa.alu_id
				,caa.mtu_id
				,caa.mtd_id
				,SUM(ISNULL(cpa.cpa_quantidadeAulasCompensadas, 0)) as qtdCompensadas
			FROM CLS_CompensacaoAusencia cpa -- WITH (NOLOCK)
			INNER JOIN CLS_CompensacaoAusenciaAluno caa -- WITH (NOLOCK)
				ON  caa.tud_id = cpa.tud_id
				AND caa.cpa_id = cpa.cpa_id
				AND caa.caa_situacao = 1
			WHERE
				cpa.tud_id = @tud_id
				AND cpa.tpc_id = @ultimoPeriodo
				AND cpa.cpa_situacao = 1 
			GROUP BY
				caa.tud_id
				,caa.alu_id
				,caa.mtu_id
				,caa.mtd_id

		IF (@tur_tipo = 4)
		BEGIN
			-- Faltas e aulas do ultimo periodo
			INSERT INTO @TabelaQtdes(alu_id, mtu_id, mtd_id, qtAulas, qtFaltas, qtFaltasReposicao)				
			SELECT 
				tdm.alu_id, 
				tdm.mtu_id, 
				tdm.mtd_id, 
				SUM(qtAulas)  OVER (PARTITION BY tdm.alu_id) AS qtAulas,
				SUM(qtFaltas) OVER (PARTITION BY tdm.alu_id) AS qtFaltas,
				SUM(qtFaltasReposicao) OVER (PARTITION BY tdm.alu_id) AS qtFaltasReposicao
			FROM 
				@MatriculaMultisseriadaTurmaAluno tdm
				CROSS APPLY FN_Select_FaltasAulasBy_TurmaDisciplina(@tipoLancamento, @ultimoPeriodo, @tud_id, @fav_calculoQtdeAulasDadas, @tipoDocente) faltas
			WHERE
				tdm.alu_id = faltas.alu_id
				AND tdm.mtu_id = faltas.mtu_id
				AND tdm.mtd_id = faltas.mtd_id
		END
		ELSE
		BEGIN
			-- Faltas e aulas do ultimo periodo
			INSERT INTO @TabelaQtdes(alu_id, mtu_id, mtd_id, qtAulas, qtFaltas, qtFaltasReposicao)				
			SELECT 
				faltas.alu_id, 
				faltas.mtu_id, 
				faltas.mtd_id, 
				faltas.qtAulas, 
				faltas.qtFaltas,
				faltas.qtFaltasReposicao
			FROM 
				FN_Select_FaltasAulasBy_TurmaDisciplina(@tipoLancamento, @ultimoPeriodo, @tud_id, @fav_calculoQtdeAulasDadas, @tipoDocente) faltas
		END
		-- So faz o calculo do somatorio, se tiver frequencia final ajustada
		--IF (@ExibeCompensacao = 1)
		--BEGIN
			IF (@tud_tipo = 15)
			BEGIN
				;WITH TabelaPeriodosAnteriores AS (
					SELECT 
						tpc.tpc_id, 
						ava.ava_id, 
						ava.fav_id 
					FROM dbo.ACA_TipoPeriodoCalendario AS tpc -- WITH (NOLOCK) 
					INNER JOIN dbo.ACA_Avaliacao AS ava -- WITH (NOLOCK)
						ON ava.fav_id = @fav_id
						AND tpc.tpc_id = ava.tpc_id	
						AND ava.ava_situacao <> 3	
					WHERE tpc_ordem <= (
						SELECT tpc_ordem FROM dbo.ACA_TipoPeriodoCalendario --WITH (NOLOCK) 
						WHERE tpc_id = @ultimoPeriodo
					)
				)
				INSERT INTO @SomatorioAulasFaltas (alu_id, faltas, faltasReposicao, aulas, compensadas)
				SELECT 
					mat.alu_id,
					SUM(CASE WHEN @ultimoPeriodo = mat.tpc_id
							THEN ISNULL(tfa.qtfaltas,0)
							ELSE ISNULL(atd.atd_numeroFaltas,0)
					END) AS faltas,
					SUM(ISNULL(tfa.qtFaltasReposicao,0)) AS faltasReposicao,
					SUM(CASE WHEN @ultimoPeriodo = mat.tpc_id
							THEN ISNULL(tfa.qtAulas,0)
							ELSE  ISNULL(atd.atd_numeroAulas,0)
						END) AS aulas,
					SUM(CASE WHEN @ultimoPeriodo = mat.tpc_id 
							THEN ISNULL(cpa.qtdCompensadas,0)
							ELSE ISNULL(atd.atd_ausenciasCompensadas,0)
					END) AS compensadas
				FROM TUR_TurmaDisciplinaMultisseriada tdm -- WITH (NOLOCK)
				INNER JOIN @tbAlunosSemFechamentoUltimoPeriodo AS alu
					ON alu.alu_id = tdm.alu_id
				INNER JOIN @Matriculas AS mat
					ON mat.alu_id = alu.alu_id
				INNER JOIN TabelaPeriodosAnteriores tpa
					ON tpa.tpc_id = mat.tpc_id	
				LEFT JOIN CLS_AlunoAvaliacaoTurmaDisciplina Atd -- WITH (NOLOCK)
					ON  Atd.tud_id = mat.tud_id
					AND Atd.alu_id = mat.alu_id
					AND Atd.mtu_id = mat.mtu_id
					AND Atd.mtd_id = mat.mtd_id
					AND atd.fav_id = tpa.fav_id
					AND atd.ava_id = tpa.ava_id
					AND Atd.atd_situacao <> 3
				LEFT JOIN @TabelaQtdes tfa 
					ON  mat.alu_id = tfa.alu_id
					AND mat.tpc_id = @ultimoPeriodo
				LEFT JOIN @AulasCompensadas Cpa
					ON Cpa.alu_id = mat.alu_id
					AND Cpa.mtu_id = mat.mtu_id
					AND Cpa.mtd_id = mat.mtd_id
					AND mat.tpc_id = @ultimoPeriodo      
				GROUP BY mat.alu_id
			END
			ELSE 
			BEGIN
				;WITH TabelaPeriodosAnteriores AS (
					SELECT 
						tpc.tpc_id, 
						ava.ava_id, 
						ava.fav_id 
					FROM dbo.ACA_TipoPeriodoCalendario AS tpc -- WITH (NOLOCK) 
					INNER JOIN dbo.ACA_Avaliacao AS ava -- WITH (NOLOCK)
						ON ava.fav_id = @fav_id
						AND tpc.tpc_id = ava.tpc_id	
						AND ava.ava_situacao <> 3	
					WHERE tpc_ordem <= (
						SELECT tpc_ordem FROM dbo.ACA_TipoPeriodoCalendario -- WITH (NOLOCK) 
						WHERE tpc_id = @ultimoPeriodo
					)
				)
				INSERT INTO @SomatorioAulasFaltas (alu_id, faltas, faltasReposicao, aulas, compensadas)
				SELECT 
					mat.alu_id,
					SUM(CASE WHEN @ultimoPeriodo = mat.tpc_id
							THEN ISNULL(tfa.qtfaltas,0)
							ELSE ISNULL(atd.atd_numeroFaltas,0)
					END) AS faltas,
					SUM(ISNULL(tfa.qtFaltasReposicao,0)) AS faltasReposicao,
					SUM(CASE WHEN @ultimoPeriodo = mat.tpc_id
							THEN ISNULL(tfa.qtAulas,0)
							ELSE  ISNULL(atd.atd_numeroAulas,0)
						END) AS aulas,
					SUM(CASE WHEN @ultimoPeriodo = mat.tpc_id 
							THEN ISNULL(cpa.qtdCompensadas,0)
							ELSE ISNULL(atd.atd_ausenciasCompensadas,0)
					END) AS compensadas
				FROM @tbAlunosSemFechamentoUltimoPeriodo AS alu 
				INNER JOIN @Matriculas AS mat
					ON mat.alu_id = alu.alu_id
				INNER JOIN TabelaPeriodosAnteriores tpa
					ON tpa.tpc_id = mat.tpc_id	
				LEFT JOIN CLS_AlunoAvaliacaoTurmaDisciplina Atd -- WITH (NOLOCK)
					ON  Atd.tud_id = mat.tud_id
					AND Atd.alu_id = mat.alu_id
					AND Atd.mtu_id = mat.mtu_id
					AND Atd.mtd_id = mat.mtd_id
					AND atd.fav_id = tpa.fav_id
					AND atd.ava_id = tpa.ava_id
					AND Atd.atd_situacao <> 3
				LEFT JOIN @TabelaQtdes tfa 
					ON  mat.alu_id = tfa.alu_id
					AND mat.tpc_id = @ultimoPeriodo
				LEFT JOIN @AulasCompensadas Cpa
					ON Cpa.alu_id = mat.alu_id
					AND Cpa.mtu_id = mat.mtu_id
					AND Cpa.mtd_id = mat.mtd_id
					AND mat.tpc_id = @ultimoPeriodo  
				GROUP BY mat.alu_id
			END
		--END

		INSERT INTO @tbFrequenciaAlunos
		SELECT 
			alu.alu_id,
			alu.mtu_id,
			alu.mtd_id
			, ISNULL(dbo.FN_Calcula_PorcentagemFrequenciaVariacao(Qtd.qtAulas, Qtd.qtFaltas, FAV.fav_variacao), 0) AS Frequencia
			-- Qtde. de faltas
			, ISNULL(Qtd.qtFaltas, 0) AS QtFaltasAluno
			-- Qtde. de aulas
			, ISNULL(Qtd.qtAulas, 0) AS QtAulasAluno
			, CAST(/*ISNULL(TabelaFrequenciaFinal.frequenciaFinal, 0)*/ 0 AS DECIMAL(5,2)) AS FrequenciaAcumulada
			, ISNULL(ac.qtdCompensadas, 0) AS ausenciasCompensadas 
			, (CASE WHEN (@ExibeCompensacao = 1)
				THEN 
					dbo.FN_Calcula_PorcentagemFrequenciaVariacao(ISNULL(saf.aulas,0), ((ISNULL(saf.faltas,0) + ISNULL(saf.faltasReposicao,0)) - ISNULL(saf.compensadas,0)), FAV.fav_variacao)
				ELSE 
					dbo.FN_Calcula_PorcentagemFrequenciaVariacao(ISNULL(saf.aulas,0), ((ISNULL(saf.faltas,0) + ISNULL(saf.faltasReposicao,0))), FAV.fav_variacao)
			END) AS FrequenciaFinalAjustada
		FROM @tbAlunosSemFechamentoUltimoPeriodo AS alu
		INNER JOIN @TabelaQtdes AS Qtd
			ON alu.alu_id = Qtd.alu_id
			AND alu.mtu_id = Qtd.mtu_id
			AND alu.mtd_id = Qtd.mtd_id
		INNER JOIN ACA_FormatoAvaliacao FAV -- WITH (NOLOCK)
			ON FAV.fav_id = @fav_id
		LEFT JOIN @AulasCompensadas ac 
			ON ac.alu_id = alu.alu_id
			AND ac.mtu_id = alu.mtu_id
			AND ac.mtd_id = alu.mtd_id
		LEFT JOIN @SomatorioAulasFaltas saf		
			ON saf.alu_id = alu.alu_id			  
	END
	--********************	

	/* [Carla 19/07/2013]
		Excluir os registros que não devem ser exibidos na tela. Esses registros devem ser trazidos para buscar
		a nota do bimestre anterior, caso seja recuperação.
	*/
	DELETE FROM @Matriculas
	WHERE
		registroExterno = 1
		-- Se possuir uma saída no período, não exibe o aluno.
		OR PossuiSaidaPeriodo = 1

	----** Filtro deficiencia
	DECLARE @tbAlunos TABLE (alu_id INT);

	IF (@tipoDocente = 5)
	BEGIN
		;WITH TipoDeficiencia AS 
		(
			SELECT 
				RelTde.tde_id
			FROM
				TUR_TurmaDisciplinaRelDisciplina DisRel -- WITH(NOLOCK)
				INNER JOIN ACA_Disciplina dis -- WITH(NOLOCK)
					ON DisRel.dis_id = dis.dis_id
					AND dis.dis_situacao <> 3
				INNER JOIN ACA_TipoDisciplina tds -- WITH(NOLOCK)
					ON dis.tds_id = tds.tds_id
					AND tds.tds_situacao <> 3
				INNER JOIN ACA_TipoDisciplinaDeficiencia RelTde -- WITH(NOLOCK)
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
			@Matriculas mtd 
			INNER JOIN ACA_Aluno alu -- WITH(NOLOCK)
				ON alu.alu_id = mtd.alu_id
			INNER JOIN Synonym_PES_PessoaDeficiencia pde -- WITH(NOLOCK)
				ON pde.pes_id = alu.pes_id
			INNER JOIN TipoDeficiencia tde
				ON pde.tde_id = tde.tde_id
	END
	ELSE
	BEGIN
		;WITH TipoDeficiencia AS 
		(
			SELECT 
				RelTde.tde_id
			FROM
				TUR_TurmaDisciplinaRelDisciplina DisRel -- WITH(NOLOCK)
				INNER JOIN ACA_Disciplina dis -- WITH(NOLOCK)
					ON DisRel.dis_id = dis.dis_id
					AND dis.dis_situacao <> 3
				INNER JOIN ACA_TipoDisciplina tds -- WITH(NOLOCK)
					ON dis.tds_id = tds.tds_id
					AND tds.tds_situacao <> 3
				INNER JOIN ACA_TipoDisciplinaDeficiencia RelTde -- WITH(NOLOCK)
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
			@Matriculas mtd 
			INNER JOIN ACA_Aluno alu -- WITH(NOLOCK)
				ON alu.alu_id = mtd.alu_id
			LEFT JOIN Synonym_PES_PessoaDeficiencia pde -- WITH(NOLOCK)
				ON pde.pes_id = alu.pes_id
		WHERE
			(NOT EXISTS (SELECT tde_id FROM TipoDeficiencia tde WHERE tde.tde_id = pde.tde_id ))	
	END
	----**	

	; WITH TabelaMovimentacao AS (
			--Selecina as movimentações que possuem matrícula anterior
			SELECT
				MOV.alu_id,
				mtu_idAnterior,
				tmv_nome    
			FROM
				MTR_Movimentacao MOV -- WITH (NOLOCK) 
				----**
				INNER JOIN @tbAlunos ON MOV.alu_id = [@tbAlunos].alu_id
				----**
				INNER JOIN ACA_TipoMovimentacao TMV -- WITH (NOLOCK) 
					ON MOV.tmv_idSaida = TMV.tmv_id
			WHERE
				mov_situacao NOT IN (3,4)
				AND tmv_situacao <> 3
				AND mtu_idAnterior IS NOT NULL	
	)
	, avaliacoes AS (
		SELECT 
			ava.tpc_id
			, ava.ava_nome
			, cap.cap_dataInicio AS cap_dataInicio
			, cap.cap_dataFim
			, ava.ava_id
		FROM ACA_Avaliacao ava -- WITH(NOLOCK)
		LEFT JOIN ACA_CalendarioPeriodo cap -- WITH(NOLOCK) 
			ON cap.tpc_id = ava.tpc_id
			AND cap.cal_id = @cal_id
			AND cap.cap_situacao <> 3
		WHERE
			(ava.ava_tipo IN (1, 5) -- periodica, periodica + final
				OR (ava.ava_tipo = 3 AND ava.ava_id = @ava_id)) --  final
			AND ava.fav_id = @fav_id
			AND ava_situacao <> 3
	)
	, TabelaObservacaoConselho AS 
	(
		SELECT
			tur_id
			, alu_id
			, mtu_id
			, CASE WHEN qtdeQualidade = 0 AND qtdeDesempenhos = 0 AND qtdeRecomendacao = 0
						AND ato_qualidade IS NULL AND ato_desempenhoAprendizado IS NULL 
						AND ato_recomendacaoAluno IS NULL AND ato_recomendacaoResponsavel IS NULL
				   THEN 0
				   ELSE 1
			  END AS observacaoPreenchida
		FROM
		(
			SELECT
				Mtr.tur_id
				, Mtr.alu_id
				, Mtr.mtu_id
				, SUM(CASE WHEN aaq.tqa_id IS NULL THEN 0 ELSE 1 END) AS qtdeQualidade
  				, SUM(CASE WHEN aad.tda_id IS NULL THEN 0 ELSE 1 END) AS qtdeDesempenhos
  				, SUM(CASE WHEN aar.rar_id IS NULL THEN 0 ELSE 1 END) AS qtdeRecomendacao
				, ato_qualidade
				, ato_desempenhoAprendizado
				, ato_recomendacaoAluno
				, ato_recomendacaoResponsavel
			FROM
				@Matriculas Mtr
				----**
				INNER JOIN @tbAlunos
					ON Mtr.alu_id = [@tbAlunos].alu_id
				----**	
				INNER JOIN ACA_Avaliacao ava -- WITH(NOLOCK)
					ON Mtr.fav_id = ava.fav_id
					AND ava.tpc_id = @ultimoPeriodo
					AND ava.ava_exibeObservacaoConselhoPedagogico = 1
				LEFT JOIN CLS_AlunoAvaliacaoTurmaQualidade aaq -- WITH(NOLOCK)
					ON  Mtr.tur_id = aaq.tur_id
					AND Mtr.alu_id = aaq.alu_id
					AND Mtr.mtu_id = aaq.mtu_id
					AND aaq.fav_id = ava.fav_id
					AND aaq.ava_id = ava.ava_id
				LEFT JOIN CLS_AlunoAvaliacaoTurmaDesempenho aad -- WITH(NOLOCK)
					ON  Mtr.tur_id = aad.tur_id
					AND Mtr.alu_id = aad.alu_id
					AND Mtr.mtu_id = aad.mtu_id
					AND aad.fav_id = ava.fav_id
					AND aad.ava_id = ava.ava_id 
				LEFT JOIN CLS_AlunoAvaliacaoTurmaRecomendacao aar -- WITH(NOLOCK)
					ON  Mtr.tur_id = aar.tur_id
					AND Mtr.alu_id = aar.alu_id
					AND Mtr.mtu_id = aar.mtu_id
					AND aar.fav_id = ava.fav_id
					AND aar.ava_id = ava.ava_id	        
				LEFT JOIN CLS_ALunoAvaliacaoTurmaObservacao ato -- WITH(NOLOCK)
					ON Mtr.tur_id = ato.tur_id
					AND Mtr.alu_id = ato.alu_id
					AND Mtr.mtu_id = ato.mtu_id
					AND ato.fav_id = ava.fav_id
					AND ato.ava_id = ava.ava_id
					AND ato.ato_situacao <> 3	
			WHERE
				Mtr.tud_id = @tud_id	
			GROUP BY
				Mtr.tur_id
				, Mtr.alu_id
				, Mtr.mtu_id
				, ato_qualidade
				, ato_desempenhoAprendizado
				, ato_recomendacaoAluno
				, ato_recomendacaoResponsavel
		) 
		AS tabela
		GROUP BY --(Adicionado group by por Webber) 
			tabela.tur_id
			, tabela.alu_id 
			, tabela.mtu_id 
			, CASE WHEN tabela.qtdeQualidade = 0 AND tabela.qtdeDesempenhos = 0 AND tabela.qtdeRecomendacao = 0
						AND tabela.ato_qualidade IS NULL AND tabela.ato_desempenhoAprendizado IS NULL 
						AND tabela.ato_recomendacaoAluno IS NULL AND tabela.ato_recomendacaoResponsavel IS NULL
				   THEN 0
				   ELSE 1
			  END		
	)
	, movimentacao AS (

			--Selecina as movimentações que possuem matrícula anterior
			SELECT
				MOV.alu_id,
				mtu_idAnterior,
				CASE WHEN tmo_tipoMovimento IN (6, 12, 14, 15, 16)
					 THEN TMV.tmv_nome + ' em ' + REPLACE(CONVERT(VARCHAR(10), mov.mov_dataRealizacao, 103), '/' + CAST(DATEPART(YEAR, mov.mov_dataRealizacao) AS VARCHAR(4)), '') +
					 	  ISNULL(' p/ ' + turD.tur_codigo, '')
					 WHEN tmo_tipoMovimento IN (8)
					 THEN TMV.tmv_nome + ' p/' + ISNULL(' ' + turD.tur_codigo, '')
					 WHEN tmo_tipoMovimento IN (11)
					 THEN TMV.tmv_nome + ' p/ ' + ISNULL(' ' + turD.tur_codigo, '')
					 ELSE TMV.tmv_nome
				END tmv_nome  
			FROM
				@Matriculas res
				INNER JOIN MTR_Movimentacao MOV -- WITH (NOLOCK) 
					ON res.alu_id = MOV.alu_id 
				INNER JOIN ACA_TipoMovimentacao TMV -- WITH (NOLOCK) 
					ON MOV.tmv_idSaida = TMV.tmv_id
				LEFT JOIN MTR_TipoMovimentacao tmo -- WITH(NOLOCK)
					ON mov.tmo_id = tmo.tmo_id
					AND tmo.tmo_situacao <> 3
				LEFT JOIN MTR_MatriculaTurma mtuD -- WITH(NOLOCK)
					ON mov.alu_id = mtuD.alu_id
					AND mov.mtu_idAtual = mtuD.mtu_id
				LEFT JOIN TUR_Turma turD -- WITH(NOLOCK)
					ON mtuD.tur_id = turD.tur_id
				LEFT JOIN ACA_CalendarioAnual calD -- WITH(NOLOCK)
					ON turD.cal_id = calD.cal_id
				INNER JOIN MTR_MatriculaTurma mtuO -- WITH(NOLOCK)
					ON mov.alu_id = mtuO.alu_id
					AND mov.mtu_idAnterior = mtuO.mtu_id
					AND mtuO.tur_id = @tur_id
				LEFT JOIN TUR_Turma turO -- WITH(NOLOCK)
					ON mtuO.tur_id = turO.tur_id
				LEFT JOIN ACA_CalendarioAnual calO -- WITH(NOLOCK)
					ON turO.cal_id = calO.cal_id
			WHERE
				mov_situacao NOT IN (3,4)
				AND tmv_situacao <> 3
				AND mtu_idAnterior IS NOT NULL	
	)
	, tbRetorno AS (	
		SELECT
			  Mtd.alu_id
			, Mtd.mtu_id
			, Mtd.mtd_id
			, alc.alc_matricula
			, F.atd_id AS AvaliacaoID
			, CASE WHEN atd_id IS NULL 
					THEN atm.atm_media
					ELSE ISNULL(F.atd_avaliacaoPosConselho, F.atd_avaliacao)
				END AS Avaliacao
			, CASE WHEN @permiteAlterarResultado = 0 
					THEN NULL
					-- Caso contrário, traz o resultado normalmente
					ELSE Mtd.mtd_resultado 
				END AS AvaliacaoResultado	
			, CASE WHEN F.atd_id IS NULL 
					THEN FM.Frequencia 
					ELSE F.atd_frequencia 
				END AS Frequencia
			, CASE WHEN @documentoOficial = 1 THEN Pes.pes_nomeOficial ELSE Pes.pes_nome END + 
				(
					CASE WHEN ( ISNULL(Mtr.mtd_situacaoDocente, Mtd.mtd_situacao) = 5 ) 
						THEN ISNULL((SELECT TOP 1 ' (' + tmv_nome + ')' FROM movimentacao MOV -- WITH(NOLOCK) 
										WHERE MOV.mtu_idAnterior = Mtd.mtu_id AND MOV.alu_id = Mtd.alu_id), ' (Inativo)')
						ELSE '' 
					END
				) 
				AS pes_nome
			, Pes.pes_dataNascimento
			, CASE WHEN ISNULL(Mtr.mtd_numeroChamadaDocente, Mtd.mtd_numeroChamada) > 0 
					THEN CAST(ISNULL(Mtr.mtd_numeroChamadaDocente, Mtd.mtd_numeroChamada) AS VARCHAR)
					ELSE '-' 
				END AS mtd_numeroChamada
			, ISNULL(Mtr.mtd_numeroChamadaDocente, Mtd.mtd_numeroChamada) AS mtd_numeroChamadaordem
			, CAST(Mtd.alu_id AS VARCHAR) + ';' + 
				CAST(Mtd.mtd_id AS VARCHAR) + ';' + 
				CAST(Mtd.mtu_id AS VARCHAR) 
				AS id
			, Mtd.mtd_situacao AS situacaoMatriculaAluno
			, F.atd_relatorio
			, F.arq_idRelatorio
			, ISNULL(Mtr.mtd_dataMatriculaDocente, Mtd.mtd_dataMatricula) AS dataMatricula
			, ISNULL(Mtr.mtd_dataSaidaDocente, Mtd.mtd_dataSaida) AS dataSaida
			, ISNULL(CASE WHEN F.atd_id IS NULL 
				THEN FM.FrequenciaFinalAjustada 
				ELSE F.atd_frequenciaFinalAjustada END, 0) AS FrequenciaFinalAjustada
			, ava.tpc_id
			, ava.ava_nome AS NomeAvaliacao
			, F.atd_avaliacaoPosConselho AS AvaliacaoPosConselho
			, ava.cap_dataInicio
			, CAST(ISNULL(toc.observacaoPreenchida, 0) AS BIT) AS observacaoConselhoPreenchida
			-- se o aluno nao teve a nota efetivada no periodo,
			-- mas ele estava presente no periodo
			-- deve-se informar o usuario.
			, CAST(CASE WHEN 
					(  
						(
							COALESCE(F.atd_avaliacaoPosConselho, F.atd_avaliacao, '') <> ''
							OR
							(
								-- se for o ultimo periodo,
								-- e nao tiver fechamento
								-- deve ter a nota do Listao
								ISNULL(ava.tpc_id, 0) = @ultimoPeriodo
								AND F.atd_id IS NULL
								AND ISNULL(atm.atm_media,'') <> ''
							)
						)
						AND ISNULL(tud.tud_naoLancarNota, 0) = 0
					)
					OR 
					(
						ISNULL(tud.tud_naoLancarNota, 0) = 1
						AND ISNULL(F.atd_id,0) > 0
					)
					THEN 1
					ELSE 0
			   END AS BIT) AS PossuiNota
			, ava.ava_id AS ava_id
			, CASE WHEN (ava.tpc_id IS NOT NULL AND ava.tpc_id = @ultimoPeriodo) 
					THEN 1 
					ELSE 0 
				END AS UltimoPeriodo		
			, FM.QtFaltasAluno
			, FM.QtAulasAluno
			, FM.ausenciasCompensadas
			, mtu.mtu_resultado
			, CASE WHEN (
					-- aluno estava presente na rede no periodo da avaliacao
					EXISTS (
						SELECT alu_id
						FROM @Matriculas
						WHERE alu_id = Mtr.alu_id
						AND tpc_id = ava.tpc_id
					)
				) THEN
					(
						CASE WHEN (
							-- aluno estava presente na escola no periodo da avaliacao
							EXISTS (
								SELECT alu_id
								FROM @Matriculas
								WHERE alu_id = Mtr.alu_id
								AND esc_id = @escolaId
								AND tpc_id = ava.tpc_id
							)
						) 
						THEN 1 -- Aluno presente na escola
						ELSE 2 -- Aluno em outra escola		
						END
					)						
				ELSE 0 -- Aluno fora da rede
				END AS PresencaAluno
			, F.atd_numeroAulas AS QtAulasEfetivado
			, ISNULL(tpc.tpc_ordem, 0) AS tpc_ordem
		FROM @Matriculas Mtr
		----**
		INNER JOIN @tbAlunos ON Mtr.alu_id = [@tbAlunos].alu_id
		----**
		INNER JOIN MTR_MatriculaTurmaDisciplina Mtd -- WITH(NOLOCK)
			ON  Mtd.alu_id = Mtr.alu_id
			AND Mtd.mtu_id = Mtr.mtu_id
			AND Mtd.mtd_id = Mtr.mtd_id
			AND Mtd.tud_id = @tud_id
		INNER JOIN TUR_TurmaDisciplina tud -- WITH(NOLOCK)
			ON Mtd.tud_id = tud.tud_id
			AND tud.tud_situacao <> 3
		INNER JOIN MTR_MatriculaTurma mtu -- WITH(NOLOCK)
			ON mtu.alu_id = Mtd.alu_id
			AND mtu.mtu_id = Mtd.mtu_id
			AND mtu_situacao <> 3    
		INNER JOIN ACA_AlunoCurriculo alc -- WITH(NOLOCK)
			ON alc.alu_id = mtu.alu_id
			AND alc.alc_id = mtu.alc_id
			AND alc.alc_situacao <> 3	
		INNER JOIN ACA_Aluno Alu -- WITH(NOLOCK)
			ON  Mtd.alu_id   = Alu.alu_id
			AND alu_situacao <> 3
		INNER JOIN VW_DadosAlunoPessoa Pes -- WITH(NOLOCK)
			ON  Alu.alu_id   = Pes.alu_id
		INNER JOIN avaliacoes ava 
			ON 1 = 1   
		LEFT JOIN ACA_TipoPeriodoCalendario tpc -- WITH(NOLOCK)
			ON tpc.tpc_id = ava.tpc_id
			AND tpc.tpc_situacao <> 3
		LEFT JOIN @Fechado F 
			ON (F.alu_id = Mtd.alu_id 
					AND F.mtu_id = Mtd.mtu_id 
					AND F.mtd_id = Mtd.mtd_id
					AND ((F.tpc_id IS NULL AND ava.tpc_id IS NULL) OR F.tpc_id = ava.tpc_id)
				)
				------------------------------------------------------------
				-- Fechado em outras turmas
				OR (F.alu_id = Mtd.alu_id 
					AND F.mtu_id <> Mtd.mtu_id
					AND F.tpc_id = ava.tpc_id
				)	
				------------------------------------------------------------	
		LEFT JOIN @tbFrequenciaAlunos FM
			ON FM.alu_id = Mtd.alu_id
			AND FM.mtu_id = Mtd.mtu_id
			AND FM.mtd_id = Mtd.mtd_id
			AND ava.tpc_id = @ultimoPeriodo			
		LEFT JOIN CLS_AlunoAvaliacaoTurmaDisciplinaMedia atm -- WITH (NOLOCK)
			ON atm.alu_id = Mtd.alu_id
			AND atm.mtu_id = Mtd.mtu_id
			AND atm.mtd_id = Mtd.mtd_id
			AND atm.tud_id = Mtd.tud_id
			AND atm.tpc_id = ava.tpc_id
			AND atm.tpc_id = @ultimoPeriodo	 
			AND atm.atm_situacao <> 3			
		LEFT JOIN TabelaObservacaoConselho toc
			ON 
			--toc.tur_id = Mtu.tur_id AND (Comentado aqui por Webber)
			toc.alu_id = Mtu.alu_id
			AND toc.mtu_id = Mtu.mtu_id	

		WHERE 
			Mtr.tpc_id = @ultimoPeriodo				    
			AND ISNULL(Mtr.mtd_situacaoDocente, Mtd.mtd_situacao) IN (1,5)
			AND COALESCE(Mtr.mtd_numeroChamadaDocente, mtd_numeroChamada, 0) >= 0		
	)
	, tbRetornoUltimoPeriodo AS
	(
		SELECT alu_id, mtu_id, mtd_id, FrequenciaFinalAjustada
		FROM tbRetorno
		WHERE UltimoPeriodo = 1
	)
	, tbFinal AS
	(
		SELECT 
			@tur_id AS tur_id
			, @tud_id AS tud_id
			, r.alu_id
			, r.mtu_id
			, r.mtd_id
			, alc_matricula
			, AvaliacaoID
			, Avaliacao
			, AvaliacaoResultado		
			, Frequencia
			, pes_nome
			, pes_dataNascimento		
			, mtd_numeroChamada
			, id
			, atd_relatorio
			, arq_idRelatorio
			, situacaoMatriculaAluno
			, dataMatricula
			, dataSaida
			-- Se for a avaliação final, pego a frequencia final ajustada do ultimo periodo
			, CASE WHEN (tpc_id IS NULL)
				THEN Up.FrequenciaFinalAjustada
				ELSE r.FrequenciaFinalAjustada
				END AS FrequenciaFinalAjustada
			, ISNULL(tpc_id, -1) AS tpc_id
			, NomeAvaliacao
			, AvaliacaoPosConselho
			, observacaoConselhoPreenchida
			-- Valida o fechamento apenas se o aluno estava 
			-- presente na escola no periodo da avaliação
			, CASE WHEN PresencaAluno = 1 THEN PossuiNota ELSE 1 END AS PossuiNota
			, ava_id
			, UltimoPeriodo
			, QtFaltasAluno
			, QtAulasAluno
			, ausenciasCompensadas
			, mtu_resultado
			, CASE WHEN PresencaAluno = 0 AND ISNULL(tpc_id, -1) > 0 THEN 1 ELSE 0 END AS AlunoForaDaRede
			, QtAulasEfetivado
			, cap_dataInicio
			, mtd_numeroChamadaordem
			, tpc_ordem
		FROM tbRetorno r
		LEFT JOIN tbRetornoUltimoPeriodo Up 
			ON Up.alu_id = r.alu_id 
			AND Up.mtu_id = r.mtu_id 
			AND Up.mtd_id = r.mtd_id	
		GROUP BY
			cap_dataInicio
			, tpc_id
			, NomeAvaliacao
			, r.alu_id
			, r.mtu_id
			, r.mtd_id
			, alc_matricula
			, AvaliacaoID
			, Avaliacao
			, AvaliacaoResultado		
			, Frequencia
			, pes_nome
			, pes_dataNascimento		
			, mtd_numeroChamada
			, mtd_numeroChamadaordem
			, id
			, atd_relatorio
			, arq_idRelatorio
			, situacaoMatriculaAluno
			, dataMatricula
			, dataSaida
			, r.FrequenciaFinalAjustada
			, Up.FrequenciaFinalAjustada
			, AvaliacaoPosConselho
			, observacaoConselhoPreenchida
			, ava_id
			, UltimoPeriodo
			, QtFaltasAluno
			, QtAulasAluno
			, ausenciasCompensadas
			, mtu_resultado
			, PossuiNota
			, PresencaAluno
			, QtAulasEfetivado
			, tpc_ordem
	)	
	SELECT 
		tur_id
		, tud_id
		, alu_id
		, mtu_id
		, mtd_id
		, alc_matricula
		, AvaliacaoID
		, Avaliacao
		, AvaliacaoResultado		
		, Frequencia
		, pes_nome
		, ISNULL(CAST(pes_dataNascimento AS VARCHAR(10)), '') AS pes_dataNascimento		
		, mtd_numeroChamada
		, id
		, atd_relatorio
		, arq_idRelatorio
		, situacaoMatriculaAluno
		, dataMatricula
		, dataSaida
		, FrequenciaFinalAjustada
		, tpc_id
		, NomeAvaliacao
		, AvaliacaoPosConselho
		, observacaoConselhoPreenchida
		, CASE WHEN AlunoForaDaRede = 1 OR PossuiNota = 1 THEN 0 ELSE 1 END AS SemNota
		, ava_id
		, UltimoPeriodo
		, QtFaltasAluno
		, QtAulasAluno
		, ausenciasCompensadas
		, mtu_resultado
		, AlunoForaDaRede
		, QtAulasEfetivado	
		, tpc_ordem
	FROM
		tbFinal
	ORDER BY 
		cap_dataInicio
		, tpc_id
		, ava_id
		, CASE 
			WHEN @ordenacao = 0 THEN 
				CASE WHEN ISNULL(mtd_numeroChamadaordem,0) <= 0 THEN 1 ELSE 0 END
		END ASC
		, CASE WHEN @ordenacao = 0 THEN ISNULL(mtd_numeroChamadaordem,0) END ASC
		, CASE WHEN @ordenacao = 1 THEN pes_nome END ASC	

END

GO
PRINT N'Altering [dbo].[NEW_Relatorio_005_SubAtaFinalResultados]'
GO
-- ==========================================================================================
-- Author:		Rafael Benevente
-- Create date: 05/11/2014
-- Description:	Procedure para a geração de dados para o relatório de ata final de resultados
-- ==========================================================================================
-- ==========================================================================================
-- Author:		Daniel Jun Suguimoto
-- Alter date:  28/01/2015
-- Description:	Correção ao carregar o parecer conclusivo.

---- Alterado: Marcia Haga - 07/03/2015
---- Description: Alterado para retornar o percentualMinimoFrequencia.

---- Alterado: Marcia Haga - 09/03/2015
---- Description: Alterado para nao retornar valor nulo no numero de faltas, numero de compensacoes 
---- e porcentagem de frequencia.

---- Alterado: Marcia Haga - 10/03/2015
---- Description: Corrigido nome da coluna de faltas e compensacoes no retorno.
---- Alterado: Marcia Haga - 11/03/2015
---- Description: Corrigido para verificar a situacao de aluno ativo no periodo 
---- de acordo com a matricula na turma (mtu_id).

---- Alterado: Marcia Haga - 13/03/2015
---- Description: Alterado para considerar a frequencia como 100%,
---- caso nao existam aulas.

---- Alterado: Daniel Jun Suguimoto - 16/06/2015
---- Description: Alterado para considerar disciplinas de turmas multisseriadas.

---- Alterado: Marcia Haga - 23/08/2016
---- Description: Alterado para desconsiderar as disciplinas do tipo experiência e território.
-- ==========================================================================================
ALTER PROCEDURE [dbo].[NEW_Relatorio_005_SubAtaFinalResultados]
	@tur_id BIGINT
	, @cal_id INT
	, @Regencia BIT
	, @documentoOficial BIT
AS
BEGIN

	CREATE TABLE #MatriculasBoletim
		(
			alu_id BIGINT
			, mtu_id INT
			, tur_id BIGINT
			, tpc_id INT
			, tpc_ordem INT
			, PeriodosEquivalentes BIT
			, MesmoCalendario BIT
			, MesmoFormato BIT
			, fav_id INT
			, mtu_numeroChamada INT
			, cal_id INT
			, cal_ano INT
			, cap_id INT
			, PossuiSaidaPeriodo BIT
			, registroExterno BIT 
			, PermiteConceitoGlobal	BIT
			, PermiteDisciplinas BIT
			, tur_idMatriculaBoletim BIGINT
			-- Indica se o fechamento do último bimestre foi realizado nessa turma (na avaliação final).
			, FechamentoUltimoBimestre Bit
			, mov_id INT
		)
		
		CREATE TABLE #dadosAlunos
		(
			alu_id BIGINT
			, pes_nome VARCHAR(400)
			, tur_id BIGINT
			, fav_id INT
			, tud_id BIGINT
			, tud_tipo TINYINT
			, atd_avaliacao VARCHAR(20)
			, atd_frequencia DECIMAL(5,2)
			, atd_numeroFaltas INT
			, atd_numeroAulas INT
			, atd_ausenciasCompensadas INT
			, atd_frequenciaFinalAjustada DECIMAL(5,2)
			, dis_id INT
			, tpc_id INT
			, tds_ordem INT
			, tpc_ordem INT
			, Tpc_Agrupamento INT
			, Tpc_exibicao CHAR(3)
			, dis_nome VARCHAR(200)
			, mtu_numeroChamada INT
			, periodoFechado BIT
			, mtu_resultadoDescricao VARCHAR(100)
			, mtu_situacao TINYINT
			, percentualMinimoFrequencia DECIMAL(5,2)
			, fav_percentualMinimoFrequenciaFinalAjustadaDisciplina DECIMAL(5,2)
			, mtu_id INT
			, mov_id INT
			, tur_idMatriculaBoletim BIGINT
			-- Indica se o fechamento do último bimestre foi realizado nessa turma (na avaliação final).
			, FechamentoUltimoBimestre Bit NOT NULL
			, possuiFreqExterna BIT
		)
		
		DECLARE @FrequenciaExterna TABLE (alu_id BIGINT, mtu_id INT, tud_id BIGINT, dis_id BIGINT, qtdFaltas INT, qtdAulas INT)

		CREATE TABLE #Parecer
		(alu_id BIGINT,
		 mtu_resultadoDescricao VARCHAR(100)
		 PRIMARY KEY (alu_id))
		
		CREATE TABLE #dadosAlunosAnuais
		(alu_id BIGINT,
		 tud_id BIGINT,				
		 totalFaltas DECIMAL(18,2),
		 totalAusenciasCompensadas DECIMAL(18,2),
		 totalAulas DECIMAL(18,2)
		 PRIMARY KEY (alu_id, tud_id))
		
		CREATE TABLE #dadosRegencia
		(alu_id BIGINT,
		 frequenciaFinalAjustadaRegencia DECIMAL(5,2),
		 totalFaltasRegencia INT,
		 totalAusenciasCompensadasRegencia INT)
		
		CREATE TABLE #tbAlunosSituacao
		(alu_id BIGINT,
		 mtu_id INT,
		 situacao TINYINT
		 PRIMARY KEY (alu_id, mtu_id))
		
		CREATE TABLE #frequenciaAnual
		(alu_id BIGINT,
		 frequenciaFinalAnual DECIMAL(5,2)
		 PRIMARY KEY (alu_id))
	
		create table #Tabela
		(alu_id BIGINT, 
		 mtu_id INT,
		 tur_id BIGINT,
		 tpc_id INT,
		 tpc_ordem INT,
		 PeriodosEquivalentes BIT,
		 MesmoCalendario BIT,
		 MesmoFormato BIT, 
		 fav_id INT, 
		 mtu_numeroChamada INT, 
		 cal_id INT,
		 cal_ano INT,
		 cap_id INT,
		 PossuiSaidaPeriodo BIT,
		 registroExterno BIT,
		 PermiteConceitoGlobal BIT,
		 PermiteDisciplinas BIT, 
		 mov_id INT)
		
		create table #Tabela2
		(alu_id BIGINT, 
		 mtu_id INT,
		 tur_id BIGINT,
		 tpc_id INT,
		 tpc_ordem INT,
		 PeriodosEquivalentes BIT,
		 MesmoCalendario BIT,
		 MesmoFormato BIT, 
		 fav_id INT, 
		 mtu_numeroChamada INT, 
		 cal_id INT,
		 cal_ano INT,
		 cap_id INT,
		 PossuiSaidaPeriodo BIT,
		 registroExterno BIT,
		 PermiteConceitoGlobal BIT,
		 PermiteDisciplinas BIT,
		 ExibirMtu BIT,
		 ExibirAluno BIT, 
		 mov_id INT)		
		
		create table #MTR_MatriculaTurma
		(alu_id BIGINT,
		 mtu_id INT,
		 mtu_numeroChamada INT,
		 linha INT)
		
		insert into #MTR_MatriculaTurma
		select alu_id, mtu_id, mtu_numeroChamada,
		       ROW_NUMBER() OVER(PARTITION BY alu_id ORDER BY mtu_id desc) as linha
          from MTR_MatriculaTurma WITH(NOLOCK) 
		 where mtu_situacao <> 3
		   and tur_id = @tur_id
		
		create index IX_MatriculaTurma_00 on #MTR_MatriculaTurma (alu_id, mtu_id, linha)
		
		insert into #Tabela
		SELECT mb.alu_id, mb.mtu_id, mb.tur_id, mb.tpc_id, mb.tpc_ordem, mb.PeriodosEquivalentes, 
			mb.MesmoCalendario, mb.MesmoFormato, mb.fav_id, mtu.mtu_numeroChamada, mb.cal_id, mb.cal_ano, mb.cap_id , mb.PossuiSaidaPeriodo,
			mb.registroExterno, mb.PermiteConceitoGlobal, mb.PermiteDisciplinas, mov_id
		from MTR_MatriculasBoletim mb WITH(NOLOCK)
			 inner join  #MTR_MatriculaTurma mtu 
			  on mb.alu_id = mtu.alu_id and mb.mtu_origemDados = mtu.mtu_id and 1 = mtu.linha
		 where mb.PeriodosEquivalentes = 1 -- traz apenas alunos que possuem períodos equivalentes
		 AND mb.PossuiSaidaPeriodo = 0
		 AND mb.registroExterno = 0
		
		insert into #Tabela2
		SELECT
			mb.alu_id
			, mb.mtu_id
			, mb.tur_id
			, mb.tpc_id
			, mb.tpc_ordem
			, mb.PeriodosEquivalentes
			, mb.MesmoCalendario
			, mb.MesmoFormato
			, mb.fav_id
			, mb.mtu_numeroChamada
			, mb.cal_id
			, mb.cal_ano
			, mb.cap_id
			, mb.PossuiSaidaPeriodo
			, mb.registroExterno
			, mb.PermiteConceitoGlobal
			, mb.PermiteDisciplinas
			, CASE WHEN 
				-- se a turma do relatorio for do 4 bimeste, trazer todos os registros
				-- se nao, trazer so onde tem fechamento naquela turma
				EXISTS 
				(
					SELECT 1
					FROM #Tabela T
					WHERE 
						T.alu_id = mb.alu_id
						AND T.tpc_id = 4 AND T.tur_id = @tur_id
				)
				OR mb.tur_id = @tur_id
				THEN 1 ELSE 0 END AS ExibirMtu
			, CASE WHEN 
				-- se a turma do relatório não for do fechamento de nenhum bimestre para esse aluno,
				-- não exibir o aluno.
				EXISTS 
				(
					SELECT 1
					FROM #Tabela T
					WHERE 
						T.alu_id = mb.alu_id
						AND T.tur_id = @tur_id
				)
				THEN 1 ELSE 0 END AS ExibirAluno
			, mov_id
		FROM #Tabela mb
		
		INSERT INTO #MatriculasBoletim (alu_id, mtu_id, tur_id, tpc_id, tpc_ordem, PeriodosEquivalentes, 
							MesmoCalendario, MesmoFormato, fav_id, mtu_numeroChamada, cal_id, cal_ano, cap_id , PossuiSaidaPeriodo,
							registroExterno, PermiteConceitoGlobal, PermiteDisciplinas
							, tur_idMatriculaBoletim, FechamentoUltimoBimestre, mov_id)
		SELECT
			mb.alu_id
			, CASE WHEN ExibirMtu = 1 THEN mb.mtu_id ELSE NULL END AS mtu_id
			, CASE WHEN ExibirMtu = 1 THEN mb.tur_id ELSE @tur_id END AS tur_id
			, mb.tpc_id
			, mb.tpc_ordem
			, mb.PeriodosEquivalentes
			, mb.MesmoCalendario
			, mb.MesmoFormato
			, mb.fav_id
			, mb.mtu_numeroChamada AS mtu_numeroChamada
			, mb.cal_id
			, mb.cal_ano
			, mb.cap_id
			, mb.PossuiSaidaPeriodo
			, mb.registroExterno
			, mb.PermiteConceitoGlobal
			, mb.PermiteDisciplinas
			, mb.tur_id AS tur_idMatriculaBoletim
			, 0 as FechamentoUltimoBimestre
			, mov_id
		FROM #Tabela2 mb
		WHERE mb.ExibirAluno = 1
		
		-- Adiciona um registro para a avaliacao final
		INSERT INTO #MatriculasBoletim (alu_id, mtu_id, tur_id, tpc_id, tpc_ordem, PeriodosEquivalentes, 
							MesmoCalendario, MesmoFormato, fav_id, mtu_numeroChamada, cal_id, cal_ano, cap_id , PossuiSaidaPeriodo,
							registroExterno, PermiteConceitoGlobal, PermiteDisciplinas, tur_idMatriculaBoletim, FechamentoUltimoBimestre, mov_id)
		SELECT 
			alu_id, mtu_id, tur_id, NULL AS tpc_id, NULL AS tpc_ordem, PeriodosEquivalentes, 
			MesmoCalendario, MesmoFormato, fav_id, mtu_numeroChamada, cal_id, cal_ano, NULL AS cap_id , PossuiSaidaPeriodo,
			registroExterno, PermiteConceitoGlobal, PermiteDisciplinas, tur_idMatriculaBoletim
			, FechamentoUltimoBimestre, mov_id
		FROM
			(SELECT 
				alu_id, mtu_id, tur_id, null AS tpc_id, null AS tpc_ordem, PeriodosEquivalentes, 
				MesmoCalendario, MesmoFormato, fav_id, mtu_numeroChamada, cal_id, cal_ano, cap_id , PossuiSaidaPeriodo,
				registroExterno, PermiteConceitoGlobal, PermiteDisciplinas, tur_idMatriculaBoletim
				, CASE WHEN tpc_ordem = 4 THEN 1 ELSE 0 END AS FechamentoUltimoBimestre, mov_id,
				ROW_NUMBER() OVER(PARTITION BY alu_id ORDER BY alu_id, tpc_ordem desc) AS ordem
			FROM 
				#MatriculasBoletim) AS MatAlu
		WHERE 
			MatAlu.ordem = 1
		
		CREATE TABLE #alunosDisciplina
		(alu_id BIGINT,
		 tur_id BIGINT,
		 tpc_id INT,
		 mtu_id INT,
		 fav_id INT,
		 tud_id BIGINT,
		 tud_tipo TINYINT,
		 dis_id INT,
		 dis_nome VARCHAR(200),
		 tds_ordem INT,
		 tpc_ordem INT,
		 tur_idMatriculaBoletim BIGINT,
		 mtu_numeroChamada INT,
		 FechamentoUltimoBimestre BIT, 
		 mov_id INT)
		
		INSERT INTO #alunosDisciplina
		SELECT
			Mb.alu_id,
			Mb.tur_id,
			Mb.tpc_id,
			mb.mtu_id,
			Mb.fav_id,
			Tud.tud_id,
			Tud.tud_tipo,
			Dis.tds_id AS dis_id,
			Dis.dis_nome,
			Tds.tds_ordem,
			mb.tpc_ordem,
			mb.tur_idMatriculaBoletim,
			MB.mtu_numeroChamada,
			Mb.FechamentoUltimoBimestre, 
			mov_id
		FROM #MatriculasBoletim Mb
			INNER JOIN TUR_TurmaRelTurmaDisciplina TrT WITH(NOLOCK)
				ON TrT.tur_id = Mb.tur_id
			INNER JOIN TUR_TurmaDisciplina Tud WITH(NOLOCK)
				ON Tud.tud_id = TrT.tud_id
				AND Tud.tud_tipo NOT IN (14, 17, 18, 19)
				AND Tud.tud_situacao <> 3
			INNER JOIN TUR_TurmaDisciplinaRelDisciplina TrD WITH(NOLOCK)
				ON TrD.tud_id = Tud.tud_id
			INNER JOIN ACA_Disciplina Dis WITH(NOLOCK)
				ON Dis.dis_id = TrD.dis_id
				AND Dis.dis_situacao <> 3
			INNER JOIN ACA_TipoDisciplina Tds WITH(NOLOCK)
				ON Tds.tds_id = Dis.tds_id
				AND Tds.tds_situacao <> 3
		WHERE
			Tds.tds_tipo IN (1, 3) -- Disciplina e Regencia de classe

		UNION

		SELECT 
			Mb.alu_id,
			Mb.tur_id,
			Mb.tpc_id,
			mb.mtu_id,
			Mb.fav_id,
			Tud.tud_id,
			Tud.tud_tipo,
			Dis.tds_id AS dis_id,
			Dis.dis_nome,
			Tds.tds_ordem,
			mb.tpc_ordem,
			mb.tur_idMatriculaBoletim,
			MB.mtu_numeroChamada,
			Mb.FechamentoUltimoBimestre, 
			mov_id
		FROM #MatriculasBoletim Mb
			INNER JOIN MTR_MatriculaTurmaDisciplina mtd WITH(NOLOCK)
				ON mtd.alu_id = Mb.alu_id
				AND mtd.mtu_id = Mb.mtu_id
				AND mtd.mtd_situacao <> 3
			INNER JOIN TUR_TurmaDisciplina Tud WITH(NOLOCK)
				ON Tud.tud_id = mtd.tud_id
				AND Tud.tud_tipo = 14
				AND Tud.tud_situacao <> 3
			INNER JOIN TUR_TurmaRelTurmaDisciplina Trt WITH(NOLOCK)
				ON Trt.tud_id = Tud.tud_id
			INNER JOIN TUR_Turma tur WITH(NOLOCK)
				ON tur.tur_id = Trt.tur_id
				AND tur.tur_situacao <> 3
			INNER JOIN TUR_TurmaDisciplinaRelDisciplina TrD WITH(NOLOCK)
				ON TrD.tud_id = Tud.tud_id
			INNER JOIN ACA_Disciplina Dis WITH(NOLOCK)
				ON Dis.dis_id = TrD.dis_id
				AND Dis.dis_situacao <> 3
			INNER JOIN ACA_TipoDisciplina Tds WITH(NOLOCK)
				ON Tds.tds_id = Dis.tds_id
				AND Tds.tds_situacao <> 3

		CREATE TABLE #tipoResultado
		(tpr_nomenclatura VARCHAR(100),
		 tpr_resultado TINYINT,
		 tpr_tipoLancamento TINYINT,
		 cur_id INT,
		 crr_id INT,
		 crp_id INT)
		
		INSERT INTO #tipoResultado
		SELECT
			tpr.tpr_nomenclatura,
			tpr.tpr_resultado,
			tpr.tpr_tipoLancamento,
			tcr.cur_id,
			tcr.crr_id,
			tcr.crp_id
		FROM
			ACA_TipoResultado tpr WITH(NOLOCK)
			INNER JOIN ACA_TipoResultadoCurriculoPeriodo tcr WITH(NOLOCK)
				ON tpr.tpr_id = tcr.tpr_id
		WHERE
			tpr.tpr_situacao <> 3
		
		INSERT INTO @FrequenciaExterna (alu_id, mtu_id, tud_id, dis_id, qtdFaltas, qtdAulas)
		SELECT
			Mtd.alu_id,
			Mtd.mtu_id,
			Mtd.tud_id,
			Ad.dis_id,
			SUM(ISNULL(afx.afx_qtdFaltas, 0)) AS qtdFaltas,
			SUM(ISNULL(afx.afx_qtdAulas, 0)) AS qtdAulas
		FROM #alunosDisciplina Ad
		INNER JOIN MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
			ON Mtd.alu_id = Ad.alu_id
			AND Mtd.mtu_id = Ad.mtu_id
			AND Mtd.tud_id = Ad.tud_id
			AND Mtd.mtd_situacao <> 3
		INNER JOIN CLS_AlunoFrequenciaExterna afx WITH(NOLOCK)
			ON Mtd.alu_id = afx.alu_id
			AND Mtd.mtu_id = afx.mtu_id
			AND Mtd.mtd_id = afx.mtd_id
			AND Ad.tpc_id IS NULL
			AND afx.afx_situacao <> 3
		GROUP BY
			Mtd.alu_id,
			Mtd.mtu_id,
			Mtd.tud_id,
			Ad.dis_id

		DECLARE @possuiFreqExterna BIT = 0
		IF (EXISTS(SELECT TOP 1 alu_id FROM @FrequenciaExterna WHERE qtdFaltas > 0 OR qtdAulas > 0))
			SET @possuiFreqExterna = 1

		;WITH tbFechamentoEF AS 
		(
			SELECT
				Atd.tud_id, 
				Atd.alu_id, 
				Atd.mtu_id, 
				Atd.mtd_id, 
				Atd.atd_id, 
				Atd.fav_id, 
				Atd.ava_id, 
				Atd.atd_avaliacao,
				Atd.atd_avaliacaoPosConselho, 
				Atd.atd_frequencia,
				Atd.atd_numeroFaltas,
				Atd.atd_numeroAulas,
				Atd.atd_ausenciasCompensadas,
				Atd.atd_frequenciaFinalAjustada,
				ava.tpc_id,
				esa.esa_id
			FROM
				#alunosDisciplina Ad
				INNER JOIN CLS_AlunoAvaliacaoTurmaDisciplina Atd WITH(NOLOCK)
					ON Atd.tud_id = Ad.tud_id
					AND Atd.alu_id = Ad.alu_id
					AND Atd.mtu_id = Ad.mtu_id
					AND Atd.atd_situacao <> 3
				INNER JOIN ACA_Avaliacao ava WITH(NOLOCK)
					ON ava.fav_id = Atd.fav_id 
					AND ava.ava_id = Atd.ava_id
					AND ISNULL(ava.tpc_id, 999) = ISNULL(Ad.tpc_id, 999) -- Caso seja 999 sera a avaliacao final
					AND ava.ava_situacao <> 3
				INNER JOIN ACA_FormatoAvaliacao fav WITH(NOLOCK)
					ON fav.fav_id = Atd.fav_id
					AND fav.fav_situacao <> 3
				INNER JOIN ACA_EscalaAvaliacao esa WITH(NOLOCK)
					ON esa.esa_id = fav.esa_idPorDisciplina
					AND esa.esa_situacao <> 3
			WHERE
				Ad.tud_tipo = 14
			GROUP BY
				Atd.tud_id, 
				Atd.alu_id, 
				Atd.mtu_id, 
				Atd.mtd_id, 
				Atd.atd_id, 
				Atd.fav_id, 
				Atd.ava_id, 
				Atd.atd_avaliacao,
				Atd.atd_avaliacaoPosConselho, 
				Atd.atd_frequencia,
				Atd.atd_numeroFaltas,
				Atd.atd_numeroAulas,
				Atd.atd_ausenciasCompensadas,
				Atd.atd_frequenciaFinalAjustada,
				ava.tpc_id,
				esa.esa_id
		)
		
		INSERT INTO #dadosAlunos
		SELECT  
			Ad.alu_id,
			CASE WHEN @documentoOficial = 1 THEN Pes.pes_nomeOficial ELSE Pes.pes_nome END AS pes_nome,
			Ad.tur_id,
			Ad.fav_id,
			Ad.tud_id,
			Ad.tud_tipo,
			ISNULL(Atd.atd_avaliacaoPosConselho, Atd.atd_avaliacao) AS atd_avaliacao,
			Atd.atd_frequencia,
			ISNULL(Atd.atd_numeroFaltas, 0) + ISNULL(afx.qtdFaltas, 0) AS atd_numeroFaltas,
			ISNULL(Atd.atd_numeroAulas, 0) + ISNULL(afx.qtdAulas, 0) AS atd_numeroAulas,
			ISNULL(Atd.atd_ausenciasCompensadas, 0) AS atd_ausenciasCompensadas,
			Atd.atd_frequenciaFinalAjustada,
			Ad.dis_id,
			Ad.tpc_id,
			Ad.tds_ordem,				
			ad.tpc_ordem,
			CASE 
				WHEN Ad.tpc_ordem = 1 THEN 1
				WHEN Ad.tpc_ordem = 2 THEN 2
				WHEN Ad.tpc_ordem = 3 THEN 3
				WHEN Ad.tpc_ordem = 4 THEN 4
				WHEN Ad.tpc_ordem IS NULL THEN 999 
			END  AS Tpc_Agrupamento,
			CASE 
				WHEN Ad.tpc_ordem = 1 THEN '1ºB' 
				WHEN Ad.tpc_ordem = 2 THEN '2ºB' 
				WHEN Ad.tpc_ordem = 3 THEN '3ºB' 
				WHEN Ad.tpc_ordem = 4 THEN '4ºB' 
				WHEN Ad.tpc_ordem IS NULL THEN 'SF' 
			END  AS Tpc_exibicao,
			Ad.dis_nome,
			Ad.mtu_numeroChamada,
			
			CASE 
				-- Quando não trouxe mtu_id do aluno, ou se ele não terminou no último bimestre nessa turma (na avaliação final)
				-- , não valida como pendência.
				WHEN Ad.mtu_id IS NULL OR (Ad.tpc_ordem IS NULL AND FechamentoUltimoBimestre = 0) THEN 1
				WHEN (Atd.atd_id IS NOT NULL) AND
					 (
						-- Valida a nota somente quando não é regência.
						Ad.tud_tipo = 11 OR 
						((ISNULL(Atd.atd_avaliacao,'') <> '') OR (ISNULL(Atd.atd_avaliacaoPosConselho,'') <> ''))
					 )
				THEN 1 ELSE 0 END AS periodoFechado,
			
			ISNULL(Tpr.tpr_nomenclatura, '-') AS mtu_resultadoDescricao,
			Mtu.mtu_situacao,
			Fav.percentualMinimoFrequencia,
			Fav.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina,
			Mtu.mtu_id,
			Ad.mov_id,
			ad.tur_idMatriculaBoletim,
			FechamentoUltimoBimestre,
			CASE WHEN ISNULL(afx.qtdAulas, 0) > 0 OR ISNULL(afx.qtdFaltas, 0) > 0
				 THEN CAST(1 AS BIT) 
				 WHEN EXISTS(SELECT TOP 1 afx2.alu_id FROM @FrequenciaExterna afx2
							 WHERE Ad.alu_id = afx2.alu_id AND Ad.dis_id = afx2.dis_id
							 AND (ISNULL(afx2.qtdAulas, 0) > 0 OR ISNULL(afx2.qtdFaltas, 0) > 0))
				 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS possuiFreqExterna
		FROM
			#alunosDisciplina Ad
			INNER JOIN MTR_MatriculaTurma Mtu WITH(NOLOCK)
				ON Mtu.alu_id = Ad.alu_id
					AND Mtu.mtu_id = Ad.mtu_id
					--AND Mtu.tur_id = Ad.tur_id
					AND Mtu.mtu_situacao <> 3
			INNER JOIN ACA_Aluno Alu WITH (NOLOCK)
				ON Alu.alu_id = Ad.alu_id	
					AND Alu.alu_situacao <> 3
			INNER JOIN VW_DadosAlunoPessoa Pes
				ON  Pes.alu_id = Alu.alu_id
			INNER JOIN ACA_AlunoCurriculo Alc WITH (NOLOCK)
				ON  Alc.alu_id = Mtu.alu_id
					AND Alc.alc_id = Mtu.alc_id
					AND Alc.alc_situacao <> 3
			INNER JOIN MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
				ON  Mtd.alu_id = Mtu.alu_id
					AND Mtd.mtu_id = Mtu.mtu_id
					AND Mtd.tud_id = Ad.tud_id
					AND Mtd.mtd_situacao <> 3
			INNER JOIN ACA_FormatoAvaliacao Fav WITH(NOLOCK)
				ON Fav.fav_id = Ad.fav_id
					AND Fav.fav_situacao <> 3
			INNER JOIN ACA_Avaliacao Ava WITH(NOLOCK)
				ON Ava.fav_id = Fav.fav_id
					AND ISNULL(AVA.tpc_id, 999) = ISNULL(Ad.tpc_id, 999) -- Caso seja 999 sera a avaliacao final
					AND Ava.ava_situacao <> 3
			LEFT JOIN CLS_AlunoAvaliacaoTurmaDisciplina Atd WITH(NOLOCK)
				ON Atd.tud_id = Mtd.tud_id
					AND Atd.alu_id = Mtd.alu_id
					AND Atd.mtu_id = Mtd.mtu_id
					AND Atd.mtd_id = Mtd.mtd_id
					AND Atd.fav_id = Fav.fav_id
					AND Atd.ava_id = Ava.ava_id
					AND Atd.atd_situacao <> 3
			LEFT JOIN #tipoResultado Tpr
				ON Tpr.cur_id = Mtu.cur_id
				AND Tpr.crr_id = Mtu.crr_id
				AND Tpr.crp_id = Mtu.crp_id
				AND Tpr.tpr_resultado = Mtu.mtu_resultado
				AND Tpr.tpr_tipoLancamento = 1
			LEFT JOIN @FrequenciaExterna afx
				ON Mtd.alu_id = afx.alu_id
				AND Mtd.mtu_id = afx.mtu_id
				AND Mtd.tud_id = afx.tud_id
				AND Ad.tpc_id IS NULL
		WHERE
			Ad.tud_tipo <> 14
		GROUP BY
			Ad.alu_id,
			Ad.mtu_id,
			Pes.pes_nomeOficial,
			Pes.pes_nome,
			Ad.tur_id,
			Ad.fav_id,
			Ad.tud_id,
			Ad.tud_tipo,
			Atd.atd_avaliacaoPosConselho, 
			Atd.atd_avaliacao,
			Atd.atd_frequencia,
			Atd.atd_numeroFaltas,
			Atd.atd_numeroAulas,
			Atd.atd_ausenciasCompensadas,
			Atd.atd_frequenciaFinalAjustada,
			Ad.dis_id,
			Ad.tpc_id,
			Ad.tds_ordem,
			Ad.tpc_ordem,
			Ad.tpc_id,
			Ad.dis_nome,
			Ad.mtu_numeroChamada,
			Atd.atd_id,
			Tpr.tpr_nomenclatura,
			Mtu.mtu_situacao,
			Fav.percentualMinimoFrequencia,
			Fav.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina,
			Mtu.mtu_id,
			ad.tur_idMatriculaBoletim,
			FechamentoUltimoBimestre,
			afx.qtdAulas,
			afx.qtdFaltas,
			Ad.mov_id
	
		UNION

		SELECT  
			Ad.alu_id,
			CASE WHEN @documentoOficial = 1 THEN Pes.pes_nomeOficial ELSE Pes.pes_nome END AS pes_nome,
			Ad.tur_id,
			Ad.fav_id,
			Ad.tud_id,
			Ad.tud_tipo,
			CASE WHEN Atd.esa_id <> esa.esa_id
				THEN NULL 
				ELSE ISNULL(Atd.atd_avaliacaoPosConselho, Atd.atd_avaliacao) 
			END AS atd_avaliacao,
			CASE WHEN Atd.esa_id <> esa.esa_id
				THEN NULL
				ELSE Atd.atd_frequencia
			END AS atd_frequencia,
			CASE WHEN Atd.esa_id <> esa.esa_id
				THEN 0
				ELSE ISNULL(Atd.atd_numeroFaltas, 0)
			END + ISNULL(afx.qtdFaltas, 0) AS atd_numeroFaltas,
			CASE WHEN Atd.esa_id <> esa.esa_id
				THEN NULL
				ELSE Atd.atd_numeroAulas
			END + ISNULL(afx.qtdAulas, 0) AS atd_numeroAulas,
			CASE WHEN Atd.esa_id <> esa.esa_id
				THEN 0
				ELSE ISNULL(Atd.atd_ausenciasCompensadas, 0)
			END AS atd_ausenciasCompensadas,
			CASE WHEN Atd.esa_id <> esa.esa_id 
				THEN NULL
				ELSE Atd.atd_frequenciaFinalAjustada
			END AS atd_frequenciaFinalAjustada,
			Ad.dis_id,
			Ad.tpc_id,
			Ad.tds_ordem,				
			ad.tpc_ordem,
			CASE 
				WHEN Ad.tpc_ordem = 1 THEN 1
				WHEN Ad.tpc_ordem = 2 THEN 2
				WHEN Ad.tpc_ordem = 3 THEN 3
				WHEN Ad.tpc_ordem = 4 THEN 4
				WHEN Ad.tpc_ordem IS NULL THEN 999 
			END  AS Tpc_Agrupamento,
			CASE 
				WHEN Ad.tpc_ordem = 1 THEN '1ºB' 
				WHEN Ad.tpc_ordem = 2 THEN '2ºB' 
				WHEN Ad.tpc_ordem = 3 THEN '3ºB' 
				WHEN Ad.tpc_ordem = 4 THEN '4ºB' 
				WHEN Ad.tpc_ordem IS NULL THEN 'SF' 
			END  AS Tpc_exibicao,
			Ad.dis_nome,
			Ad.mtu_numeroChamada,
			
			CASE 
				-- Quando não trouxe mtu_id do aluno, ou se ele não terminou no último bimestre nessa turma (na avaliação final)
				-- , não valida como pendência.
				WHEN Ad.mtu_id IS NULL OR (Ad.tpc_ordem IS NULL AND FechamentoUltimoBimestre = 0) THEN 1
				WHEN (Atd.atd_id IS NOT NULL) AND Atd.esa_id = esa.esa_id AND
					 (
						-- Valida a nota somente quando não é regência.
						Ad.tud_tipo = 11 OR 
						((ISNULL(Atd.atd_avaliacao,'') <> '') OR (ISNULL(Atd.atd_avaliacaoPosConselho,'') <> ''))
					 )
				THEN 1 ELSE 0 END AS periodoFechado,
			
			ISNULL(Tpr.tpr_nomenclatura, '-') AS mtu_resultadoDescricao,
			Mtu.mtu_situacao,
			Fav.percentualMinimoFrequencia,
			Fav.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina,
			Mtu.mtu_id,
			ad.tur_idMatriculaBoletim,
			FechamentoUltimoBimestre,
			CASE WHEN ISNULL(afx.qtdAulas, 0) > 0 OR ISNULL(afx.qtdFaltas, 0) > 0
				 THEN CAST(1 AS BIT) 
				 WHEN EXISTS(SELECT TOP 1 afx2.alu_id FROM @FrequenciaExterna afx2
							 WHERE Ad.alu_id = afx2.alu_id AND Ad.dis_id = afx2.dis_id
							 AND (ISNULL(afx2.qtdAulas, 0) > 0 OR ISNULL(afx2.qtdFaltas, 0) > 0))
				 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS possuiFreqExterna,
			Ad.mov_id
		FROM
			#alunosDisciplina Ad
			INNER JOIN MTR_MatriculaTurma Mtu WITH(NOLOCK)
				ON Mtu.alu_id = Ad.alu_id
					AND Mtu.mtu_id = Ad.mtu_id
					--AND Mtu.tur_id = Ad.tur_id
					AND Mtu.mtu_situacao <> 3
			INNER JOIN ACA_Aluno Alu WITH (NOLOCK)
				ON Alu.alu_id = Ad.alu_id	
					AND Alu.alu_situacao <> 3
			INNER JOIN VW_DadosAlunoPessoa Pes
				ON  Pes.alu_id = Alu.alu_id
			INNER JOIN ACA_AlunoCurriculo Alc WITH (NOLOCK)
				ON  Alc.alu_id = Mtu.alu_id
					AND Alc.alc_id = Mtu.alc_id
					AND Alc.alc_situacao <> 3
			INNER JOIN MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
				ON  Mtd.alu_id = Mtu.alu_id
					AND Mtd.mtu_id = Mtu.mtu_id
					AND Mtd.tud_id = Ad.tud_id
					AND Mtd.mtd_situacao <> 3
			INNER JOIN ACA_FormatoAvaliacao Fav WITH(NOLOCK)
				ON Fav.fav_id = Ad.fav_id
					AND Fav.fav_situacao <> 3
			INNER JOIN ACA_EscalaAvaliacao esa WITH(NOLOCK)
				ON esa.esa_id = Fav.esa_idPorDisciplina
				AND esa.esa_situacao <> 3
			LEFT JOIN tbFechamentoEF Atd 
				ON Atd.tud_id = Mtd.tud_id
					AND Atd.alu_id = Mtd.alu_id
					AND Atd.mtu_id = Mtd.mtu_id
					AND Atd.mtd_id = Mtd.mtd_id
					AND ISNULL(Atd.tpc_id, 999) = ISNULL(Ad.tpc_id, 999)
			LEFT JOIN #tipoResultado Tpr
				ON Tpr.cur_id = Mtu.cur_id
				AND Tpr.crr_id = Mtu.crr_id
				AND Tpr.crp_id = Mtu.crp_id
				AND Tpr.tpr_resultado = Mtu.mtu_resultado
				AND Tpr.tpr_tipoLancamento = 1
			LEFT JOIN @FrequenciaExterna afx
				ON Mtd.alu_id = afx.alu_id
				AND Mtd.mtu_id = afx.mtu_id
				AND Mtd.tud_id = afx.tud_id
				AND Ad.tpc_id IS NULL
		WHERE
			Ad.tud_tipo = 14
		GROUP BY
			Ad.alu_id,
			Ad.mtu_id,
			Pes.pes_nomeOficial,
			Pes.pes_nome,
			Ad.tur_id,
			Ad.fav_id,
			Ad.tud_id,
			Ad.tud_tipo,
			Atd.atd_avaliacaoPosConselho, 
			Atd.atd_avaliacao,
			Atd.atd_frequencia,
			Atd.atd_numeroFaltas,
			Atd.atd_numeroAulas,
			Atd.atd_ausenciasCompensadas,
			Atd.atd_frequenciaFinalAjustada,
			Ad.dis_id,
			Ad.tpc_id,
			Ad.tds_ordem,
			Ad.tpc_ordem,
			Ad.tpc_id,
			Ad.dis_nome,
			Ad.mtu_numeroChamada,
			Atd.atd_id,
			Tpr.tpr_nomenclatura,
			Mtu.mtu_situacao,
			Fav.percentualMinimoFrequencia,
			Fav.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina,
			Mtu.mtu_id,
			ad.tur_idMatriculaBoletim,
			FechamentoUltimoBimestre,
			atd.esa_id,
			esa.esa_id,
			afx.qtdAulas,
			afx.qtdFaltas,
			Ad.mov_id

	IF (@Regencia = 1)
	BEGIN

		insert into #Parecer
		SELECT
			alu_id,
			mtu_resultadoDescricao
		FROM
		(
			SELECT
				alu_id,
				mtu_resultadoDescricao,
				ROW_NUMBER() OVER (PARTITION BY alu_id ORDER BY ISNULL(Tpc_Agrupamento, 0) DESC) AS linha
			FROM 
				#dadosAlunos Da
		) AS tabela
		WHERE linha = 1
		
		INSERT INTO #dadosAlunosAnuais
		SELECT
			alu_id,
			tud_id,				
			SUM(Da.atd_numeroFaltas) AS totalFaltas,
			SUM(Da.atd_ausenciasCompensadas) AS totalAusenciasCompensadas,
			SUM(Da.atd_numeroAulas) AS totalAulas
		FROM
			#dadosAlunos Da
		GROUP BY
			alu_id,
			tud_id

		INSERT INTO #dadosRegencia
		SELECT
			alu_id,
			frequenciaFinalAjustadaRegencia,
			totalFaltasRegencia,
			totalAusenciasCompensadasRegencia
		FROM
		(
			SELECT
				alu_id,
				atd_frequenciaFinalAjustada AS frequenciaFinalAjustadaRegencia,
				SUM(atd_numeroFaltas) OVER (PARTITION BY alu_id) AS totalFaltasRegencia,
				SUM(atd_ausenciasCompensadas) OVER (PARTITION BY alu_id) AS totalAusenciasCompensadasRegencia,
				ROW_NUMBER() OVER (PARTITION BY alu_id ORDER BY ISNULL(Da.tpc_ordem, 0) DESC) AS linha
			FROM 
				#dadosAlunos Da
			WHERE Da.atd_frequenciaFinalAjustada IS NOT NULL 
				  AND da.tud_tipo = 11-- Regencia
		) AS tabela
		WHERE linha = 1
		GROUP BY
			alu_id,
			frequenciaFinalAjustadaRegencia,
			totalFaltasRegencia,
			totalAusenciasCompensadasRegencia
		
		INSERT INTO #tbAlunosSituacao
		SELECT 
			Alu.alu_id,
			Alu.mtu_id,
			--Caso seja movimentacao 8-Remanejamento , 27-Conclusão de Nivel de Ensino traz como ativo, para impressao de anos anteriores.
			1 AS situacao
		FROM 
			#dadosAlunos Alu
			INNER JOIN MTR_Movimentacao Mov WITH(NOLOCK)
				ON Mov.alu_id = Alu.alu_id
				AND Mov.mtu_idAnterior = Alu.mtu_id
				AND Mov.mov_situacao <> 3
			INNER JOIN MTR_TipoMovimentacao Tmo WITH(NOLOCK)
				ON Tmo.tmo_id = Mov.tmo_id
				AND Tmo.tmo_tipoMovimento IN (8,23,27)
				AND Tmo.tmo_situacao <> 3
		GROUP BY Alu.alu_id, Alu.mtu_id
		
		;WITH dadosAlunosTpcMax AS (
			SELECT
				Da.alu_id,
				MAX(tpc_ordem) AS maxOrdem
			FROM #dadosAlunos Da
			GROUP BY Da.alu_id
		) , movimentacao AS (
			SELECT
				Da.alu_id,
				Da.mtu_id,
				CASE WHEN tmo_tipoMovimento IN (6, 12, 14, 15, 16)
					 THEN 'TR ' + REPLACE(CONVERT(VARCHAR(10), mov.mov_dataRealizacao, 103), '/' + CAST(DATEPART(YEAR, mov.mov_dataRealizacao) AS VARCHAR(4)), '') +
						  ISNULL(' - ' + turD.tur_codigo, '')
					 WHEN tmo_tipoMovimento IN (8)
					 THEN 'RM' + ISNULL(' ' + turD.tur_codigo, '')
					 WHEN tmo_tipoMovimento IN (11)
					 THEN 'RC' + ISNULL(' ' + turD.tur_codigo, '')
					 ELSE ''
				END movMsg
			FROM #dadosAlunos Da
			INNER JOIN dadosAlunosTpcMax dat
				ON Da.alu_id = dat.alu_id
				AND Da.tpc_ordem = dat.maxOrdem
			INNER JOIN MTR_Movimentacao mov WITH(NOLOCK)
				ON Da.alu_id = mov.alu_id
				AND Da.mtu_id = mov.mtu_idAnterior
				AND mov.mov_situacao <> 3
			INNER JOIN MTR_TipoMovimentacao tmo WITH(NOLOCK)
				ON mov.tmo_id = tmo.tmo_id
				AND tmo_tipoMovimento IN (6, 8, 11, 12, 14, 15, 16)
				AND tmo.tmo_situacao <> 3
			LEFT JOIN MTR_MatriculaTurma mtuD WITH(NOLOCK)
				ON mov.alu_id = mtuD.alu_id
				AND mov.mtu_idAtual = mtuD.mtu_id
			LEFT JOIN TUR_Turma turD WITH(NOLOCK)
				ON mtuD.tur_id = turD.tur_id
			LEFT JOIN ACA_CalendarioAnual calD WITH(NOLOCK)
				ON turD.cal_id = calD.cal_id
			INNER JOIN MTR_MatriculaTurma mtuO WITH(NOLOCK)
				ON mov.alu_id = mtuO.alu_id
				AND mov.mtu_idAnterior = mtuO.mtu_id
				AND mtuO.tur_id = @tur_id
			LEFT JOIN TUR_Turma turO WITH(NOLOCK)
				ON mtuO.tur_id = turO.tur_id
			LEFT JOIN ACA_CalendarioAnual calO WITH(NOLOCK)
				ON turO.cal_id = calO.cal_id
			WHERE 
				turD.tur_id IS NULL OR calD.cal_ano = calO.cal_ano --Ou não tem turma destino ou a turma destino é do mesmo ano
			GROUP BY
				Da.alu_id,
				Da.mtu_id,
				tmo_tipoMovimento,
				mov.mov_dataRealizacao,
				turD.tur_codigo
		)
		
		SELECT
			Da.alu_id,
			Da.pes_nome +
			CASE WHEN ISNULL(mov.movMsg, '') = ''
				 THEN ''
				 ELSE ' (' + mov.movMsg + ')'
			END AS pes_nome,
			Da.tur_id,
			Da.fav_id,
			Da.tud_id,
			Da.tud_tipo,
			Da.atd_avaliacao,
			Da.atd_frequencia,
			ISNULL(Da.atd_numeroFaltas, 0) AS atd_numeroFaltas,
			Da.atd_numeroAulas,
			ISNULL(Da.atd_ausenciasCompensadas, 0) AS atd_ausenciasCompensadas,
			Da.atd_frequenciaFinalAjustada,
			Da.dis_id,
			Da.tpc_id,
			Da.tds_ordem,
			Da.Tpc_Agrupamento,
			Da.Tpc_exibicao,
			Da.dis_nome,
			Da.mtu_numeroChamada,
			Da.periodoFechado,
			CASE WHEN EXISTS(SELECT TOP 1 Da2.alu_id FROM dadosAlunosTpcMax DaT
							 INNER JOIN #dadosAlunos Da2 ON DaT.alu_id = Da2.alu_id AND Dat.maxOrdem = Da2.tpc_ordem
							 WHERE Da2.alu_id = Da.alu_id AND Da2.mtu_situacao = 1)
				 THEN 1 ELSE COALESCE(tas.situacao, Da.mtu_situacao, 5) END AS mtu_situacao,
			Da.percentualMinimoFrequencia,
			Da.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina,
			SUM(ISNULL(Da.atd_ausenciasCompensadas, 0)) OVER(PARTITION BY Da.alu_id, Da.tud_id) AS atd_AusenciasCompensadasVC,
			ISNULL(CAST(Daa.totalFaltas AS VARCHAR),'0') AS totalFaltas,
			SUM(ISNULL(Daa.totalAusenciasCompensadas, 0)) OVER (PARTITION BY Da.alu_id, Da.tpc_ordem) AS totalAusenciasCompensadas,
			CASE WHEN Da.tud_tipo IN (11,12,13) THEN '100'
			ELSE ISNULL ((SELECT 				
					TOP 1 CAST(CAST(ISNULL(dda.atd_frequenciaFinalAjustada,0) AS DECIMAL(5,0)) AS VARCHAR)
				FROM 
					#dadosAlunos dda 
				WHERE 	
					dda.alu_id = da.alu_id
					AND dda.dis_id = da.dis_id
					AND dda.tpc_ordem = (SELECT MAX(ddaa.tpc_ordem) FROM #dadosAlunos ddaa 
														WHERE 	
															ddaa.alu_id = dda.alu_id
															AND ddaa.dis_id = dda.dis_id
															AND ddaa.tpc_id IS NOT NULL
															AND ddaa.atd_frequenciaFinalAjustada IS NOT NULL)), '100') END AS frequenciaFinalAjustada, 
			CAST(CAST(0 AS DECIMAL(5,0)) AS VARCHAR) AS frequenciaAnual,
			ISNULL(CAST(Dr.totalFaltasRegencia AS VARCHAR),'0') AS totalFaltasRegencia,
			ISNULL(CAST(Dr.totalAusenciasCompensadasRegencia AS VARCHAR),'0') AS totalAusenciasCompensadasRegencia,
			CASE 
				WHEN Daa.totalAulas IS NULL OR Da.tud_tipo NOT IN (11,12,13) THEN '100'
			ELSE
				CAST(CAST(ISNULL(Dr.frequenciaFinalAjustadaRegencia,100) AS DECIMAL(5,0)) AS VARCHAR)
			END AS frequenciaFinalAjustadaRegencia,
			par.mtu_resultadoDescricao,
			possuiFreqExterna AS PossuiFreqExternaAtual,
			@possuiFreqExterna AS possuiFreqExterna
		FROM
			#dadosAlunos Da
			LEFT JOIN #dadosAlunosAnuais Daa
				ON Daa.alu_id = Da.alu_id
				AND Daa.tud_id = Da.tud_id
			LEFT JOIN #dadosRegencia Dr
				ON Dr.alu_id = Da.alu_id
			LEFT JOIN #Parecer par
				ON par.alu_id = Da.alu_id
			LEFT JOIN #tbAlunosSituacao tas
				ON tas.alu_id = Da.alu_id
				AND tas.mtu_id = Da.mtu_id	
			LEFT JOIN movimentacao mov
				ON Da.alu_id = mov.alu_id
				AND Da.mtu_id = mov.mtu_id

		ORDER BY 
			mtu_numeroChamada,
			pes_nome,
			dis_nome,
			Tpc_Agrupamento		
	END
	ELSE
	BEGIN
		INSERT INTO #Parecer (alu_id, mtu_resultadoDescricao)
		SELECT
			alu_id,
			mtu_resultadoDescricao
		FROM
		(
			SELECT
				alu_id,
				mtu_resultadoDescricao,
				ROW_NUMBER() OVER (PARTITION BY alu_id ORDER BY ISNULL(Tpc_Agrupamento, 0) DESC) AS linha
			FROM 
				#dadosAlunos Da
		) AS tabela
		WHERE linha = 1

		insert into #dadosAlunosAnuais (alu_id, tud_id, totalFaltas, totalAusenciasCompensadas, totalAulas)
		SELECT
			alu_id,
			tud_id,
			SUM(atd_numeroFaltas) AS totalFaltas,
			SUM(atd_ausenciasCompensadas) AS totalAusenciasCompensadas,
			SUM(atd_numeroAulas) AS totalAulas
		FROM
			#dadosAlunos Da
		GROUP BY
			alu_id,
			tud_id


		INSERT INTO #frequenciaAnual(alu_id, frequenciaFinalAnual)
		SELECT
			alu.alu_id,
			CASE WHEN SUM(totalAulas) > 0 THEN
				(1 - (SUM(totalFaltas) - SUM(totalAusenciasCompensadas)) / SUM(totalAulas)) * 100
			ELSE 100 END AS frequenciaFinalAnual												
		FROM 
			#dadosAlunosAnuais alu
		GROUP BY alu.alu_id

		insert into #tbAlunosSituacao (alu_id, mtu_id, situacao)
		SELECT 
			Alu.alu_id,
			Alu.mtu_id,
			--Caso seja movimentacao 8-Remanejamento , 27-Conclusão de Nivel de Ensino traz como ativo, para impressao de anos anteriores.
			1 AS situacao
		FROM 
			#dadosAlunos Alu
			INNER JOIN MTR_Movimentacao Mov WITH(NOLOCK)
				ON Mov.alu_id = Alu.alu_id
				AND Mov.mtu_idAnterior = Alu.mtu_id
				AND Mov.mov_situacao <> 3
			INNER JOIN MTR_TipoMovimentacao Tmo WITH(NOLOCK)
				ON Tmo.tmo_id = Mov.tmo_id
				AND Tmo.tmo_tipoMovimento IN (8,23,27)
				AND Tmo.tmo_situacao <> 3
		GROUP BY Alu.alu_id, Alu.mtu_id

		;WITH dadosAlunosTpcMax AS (
			SELECT
				Da.alu_id,
				MAX(tpc_ordem) AS maxOrdem
			FROM #dadosAlunos Da
			GROUP BY Da.alu_id
		)
		, movimentacao AS (
			SELECT
				Da.alu_id,
				Da.mtu_id,
				CASE WHEN tmo_tipoMovimento IN (6, 12, 14, 15, 16)
					 THEN 'TR ' + REPLACE(CONVERT(VARCHAR(10), mov.mov_dataRealizacao, 103), '/' + CAST(DATEPART(YEAR, mov.mov_dataRealizacao) AS VARCHAR(4)), '') +
						  ISNULL(' - ' + turD.tur_codigo, '')
					 WHEN tmo_tipoMovimento IN (8)
					 THEN 'RM' + ISNULL(' ' + turD.tur_codigo, '')
					 WHEN tmo_tipoMovimento IN (11)
					 THEN 'RC' + ISNULL(' ' + turD.tur_codigo, '')
					 ELSE ''
				END movMsg
			FROM #dadosAlunos Da
			INNER JOIN dadosAlunosTpcMax dat
				ON Da.alu_id = dat.alu_id
				AND Da.tpc_ordem = dat.maxOrdem
			INNER JOIN MTR_Movimentacao mov WITH(NOLOCK)
				ON Da.alu_id = mov.alu_id
				AND Da.mtu_id = mov.mtu_idAnterior
				AND mov.mov_situacao <> 3
			INNER JOIN MTR_TipoMovimentacao tmo WITH(NOLOCK)
				ON mov.tmo_id = tmo.tmo_id
				AND tmo_tipoMovimento IN (6, 8, 11, 12, 14, 15, 16)
				AND tmo.tmo_situacao <> 3
			LEFT JOIN MTR_MatriculaTurma mtuD WITH(NOLOCK)
				ON mov.alu_id = mtuD.alu_id
				AND mov.mtu_idAtual = mtuD.mtu_id
			LEFT JOIN TUR_Turma turD WITH(NOLOCK)
				ON mtuD.tur_id = turD.tur_id
			LEFT JOIN ACA_CalendarioAnual calD WITH(NOLOCK)
				ON turD.cal_id = calD.cal_id
			LEFT JOIN MTR_MatriculaTurma mtuO WITH(NOLOCK)
				ON mov.alu_id = mtuO.alu_id
				AND mov.mtu_idAnterior = mtuO.mtu_id
				AND mtuO.tur_id = @tur_id
			LEFT JOIN TUR_Turma turO WITH(NOLOCK)
				ON mtuO.tur_id = turO.tur_id
			LEFT JOIN ACA_CalendarioAnual calO WITH(NOLOCK)
				ON turO.cal_id = calO.cal_id
			WHERE 
				turD.tur_id IS NULL OR calD.cal_ano = calO.cal_ano --Ou não tem turma destino ou a turma destino é do mesmo ano
			GROUP BY
				Da.alu_id,
				Da.mtu_id,
				tmo_tipoMovimento,
				mov.mov_dataRealizacao,
				turD.tur_codigo
		)

		SELECT
			Da.alu_id,
			Da.pes_nome +
			CASE WHEN ISNULL(mov.movMsg, '') = ''
				 THEN ''
				 ELSE ' (' + mov.movMsg + ')'
			END AS pes_nome,
			Da.tur_idMatriculaBoletim as tur_id,
			Da.fav_id,
			Da.tud_id,
			Da.tud_tipo,			
			Da.atd_avaliacao,
			Da.atd_frequencia AS atd_frequencia,
			ISNULL(Da.atd_numeroFaltas, 0) AS atd_numeroFaltas,
			Da.atd_numeroAulas AS atd_numeroAulas,
			ISNULL(Da.atd_ausenciasCompensadas, 0) AS atd_ausenciasCompensadas,
			Da.atd_frequenciaFinalAjustada AS atd_frequenciaFinalAjustada,
			Da.dis_id,
			Da.tpc_id,
			Da.tds_ordem,
			Da.Tpc_Agrupamento,
			Da.Tpc_exibicao,
			Da.dis_nome,
			Da.mtu_numeroChamada AS mtu_numeroChamada,
			Da.periodoFechado,
			CASE WHEN EXISTS(SELECT TOP 1 Da2.alu_id FROM dadosAlunosTpcMax DaT
							 INNER JOIN #dadosAlunos Da2 ON DaT.alu_id = Da2.alu_id AND Dat.maxOrdem = Da2.tpc_ordem
							 WHERE Da2.alu_id = Da.alu_id AND Da2.mtu_situacao = 1)
				 THEN 1 ELSE COALESCE(tas.situacao, Da.mtu_situacao, 5) END AS mtu_situacao,
			COALESCE(tas.situacao, Da.mtu_situacao, 5) AS mtu_situacaoPeriodo,
			Da.percentualMinimoFrequencia,
			Da.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina,
			SUM(ISNULL(Da.atd_ausenciasCompensadas, 0)) OVER(PARTITION BY Da.alu_id, Da.tud_id) AS atd_AusenciasCompensadasVC,
			ISNULL(CAST(Daa.totalFaltas AS VARCHAR),'0') AS totalFaltas,
			SUM(ISNULL(CAST(Daa.totalAusenciasCompensadas AS INT), 0)) OVER (PARTITION BY Da.alu_id, Da.tpc_ordem) AS totalAusenciasCompensadas,
			ISNULL ((SELECT 
					TOP 1 CAST(CAST(ISNULL(dda.atd_frequenciaFinalAjustada,0) AS DECIMAL(5,0)) AS VARCHAR)
				FROM 
					#dadosAlunos dda 
				WHERE 	
					dda.alu_id = da.alu_id
					AND dda.dis_id = da.dis_id
					AND dda.tpc_ordem = (SELECT MAX(ddaa.tpc_ordem) FROM #dadosAlunos ddaa 
														WHERE 	
															ddaa.alu_id = dda.alu_id
															AND ddaa.dis_id = dda.dis_id
															AND ddaa.tpc_id IS NOT NULL
															AND ddaa.atd_frequenciaFinalAjustada IS NOT NULL)), '100')
															AS frequenciaFinalAjustada, 
			CASE WHEN Fa.frequenciaFinalAnual IS NULL
				THEN '100'
			ELSE	
				dbo.FN_Aplica_VariacaoPorcentagemFrequenciaString(Fa.frequenciaFinalAnual,1) 
			END AS frequenciaAnual,
			'0' AS totalFaltasRegencia,
			'0' AS totalAusenciasCompensadasRegencia,
			CAST(CAST(0 AS DECIMAL(5,0)) AS VARCHAR) AS frequenciaFinalAjustadaRegencia,
			par.mtu_resultadoDescricao,
			possuiFreqExterna AS PossuiFreqExternaAtual,
			@possuiFreqExterna AS possuiFreqExterna
		FROM #dadosAlunos Da
			LEFT JOIN #dadosAlunosAnuais Daa
				ON Daa.alu_id = Da.alu_id
				AND Daa.tud_id = Da.tud_id
			LEFT JOIN #frequenciaAnual Fa
				ON Fa.alu_id = Da.alu_id
			LEFT JOIN #Parecer par
				ON par.alu_id = Da.alu_id
			LEFT JOIN #tbAlunosSituacao tas
				ON tas.alu_id = Da.alu_id
				AND tas.mtu_id = Da.mtu_id		
			LEFT JOIN movimentacao mov
				ON Da.alu_id = mov.alu_id
				AND Da.mtu_id = mov.mtu_id
		ORDER BY 
			mtu_numeroChamada,
			pes_nome,
			dis_nome,
			Tpc_Agrupamento			
	END
	drop table #MTR_MatriculaTurma
	drop table #MatriculasBoletim
	drop table #Parecer
	drop table #Tabela
	drop table #Tabela2
	drop table #alunosDisciplina
	drop table #dadosAlunos
	drop table #dadosAlunosAnuais
	drop table #dadosRegencia
	drop table #frequenciaAnual
	drop table #tbAlunosSituacao
	drop table #tipoResultado

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

	DECLARE @Dados TABLE (
		tud_id BIGINT,
		tud_tipo TINYINT,
		tdc_id INT,
		tur_docenteEspecialista BIT,
		fav_planejamentoAulasNotasConjunto BIT,
		tur_codigo VARCHAR(50),
		esc_nome VARCHAR(250),
		tur_id BIGINT,
		tud_nome VARCHAR(100),
		tud_disciplinaEspecial BIT,
		tds_nomeDisciplinaEspecial VARCHAR(200),
		tud_naoLancarPlanejamento BIT,
		tds_ordem INT
	)
	
	DECLARE @DisciplinasCoordenador_cte TABLE (
		tud_id BIGINT,
		tur_id BIGINT,
		permissao INT,
		TemPrincipal BIT
	)

	INSERT INTO @DisciplinasCoordenador_cte 
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
			
	INSERT INTO @Dados 
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
		INNER JOIN @DisciplinasCoordenador_cte Cd
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
						FROM @DisciplinasCoordenador_cte AS DisciplinasDocente
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
	
	;WITH DisciplinaRelacionada AS
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
		FROM @Dados Tur
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
		FROM @Dados Tur
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
					FROM @DisciplinasCoordenador_cte Aux 
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
						FROM @DisciplinasCoordenador_cte Aux 
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
							SELECT tud_id FROM @DisciplinasCoordenador_cte AS TurmasDocente
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
PRINT N'Creating [dbo].[NEW_ACA_AlunoJustificativaFaltaAnexo_UPDATE]'
GO


CREATE PROCEDURE [dbo].[NEW_ACA_AlunoJustificativaFaltaAnexo_UPDATE]
	@alu_id BIGINT
	, @afj_id INT
	, @aja_id INT
	, @arq_id BIGINT
	, @aja_descricao VARCHAR (500)
	, @aja_situacao TINYINT
	, @aja_dataAlteracao DATETIME

AS
BEGIN
	UPDATE ACA_AlunoJustificativaFaltaAnexo 
	SET 
		arq_id = @arq_id 
		, aja_descricao = @aja_descricao 
		, aja_situacao = @aja_situacao 
		, aja_dataAlteracao = @aja_dataAlteracao 

	WHERE 
		alu_id = @alu_id 
		AND afj_id = @afj_id 
		AND aja_id = @aja_id 

		
	DECLARE @ret INT
	SELECT @ret = ISNULL(@@ROWCOUNT,-1)
	RETURN @ret
	
END


GO
PRINT N'Altering [dbo].[STP_ACA_EventoLimite_LOAD]'
GO
ALTER PROCEDURE [dbo].[STP_ACA_EventoLimite_LOAD]
	@cal_id Int
	, @tev_id Int
	, @evl_id Int
	
AS
BEGIN
	SELECT	Top 1
		 cal_id  
		, tev_id 
		, evl_id 
		, tpc_id 
		, esc_id 
		, uni_id 
		, evl_dataInicio 
		, evl_dataFim 
		, usu_id 
		, evl_situacao 
		, evl_dataCriacao 
		, evl_dataAlteracao 
		, uad_id

 	FROM
 		ACA_EventoLimite
	WHERE 
		cal_id = @cal_id
		AND tev_id = @tev_id
		AND evl_id = @evl_id
END
GO
PRINT N'Altering [dbo].[NEW_Relatorio_DocDctRelSinteseEnriquecimentoCurricular_SubRelatorio]'
GO

-- ===========================================================================
-- Author:		Ivan Pimentel
-- Create date: 29/07/2014
-- Description: Procedure para o relatorio de Enriquecimento curricular de São Paulo

-- Alterado: Marcia Haga - 22/08/2014
-- Description: Alterada para calcular a porcentagem de falta considerando 
-- tambem o numero de aulas cadastrado na efetivacao.

-- Alterado: Marcia Haga - 29/08/2014
-- Description: Alterada para retornar o aluno inativo de acordo com a situação 
-- do aluno na turma/disciplina. Removido campos não utilizados no relatorio.

-- Alterado: Katiusca Murari - 10/10/2014
-- Description: Adicionada a formatacao para apenas apresentar o numero de casas
--				decimais de acordo com o parametro de variacao.

---- Alterado: Marcia Haga - 24/02/2015
---- Description: Correcao dos calculos.

---- Alterado: Marcia Haga - 25/02/2015
---- Description: Alterado calculos para nao acumular dos bimestres;
---- corrigida formatacao da porcentagem de frequencia.

---- Alterado: Marcia Haga - 01/09/2016
---- Description: Alterado para retornar o tud_nome caso seja visão de docente.
-- ===========================================================================
ALTER PROCEDURE [dbo].[NEW_Relatorio_DocDctRelSinteseEnriquecimentoCurricular_SubRelatorio]
	@tpc_id INT
	, @tur_id BIGINT 
	, @tud_id BIGINT
	, @documentoOficial BIT

AS
BEGIN

	SET NOCOUNT ON;	

	DECLARE @tbTdsIdsEnriquecimentoEscolar TABLE (tds_id INT)

	INSERT INTO @tbTdsIdsEnriquecimentoEscolar (tds_id) 
	SELECT pac_valor FROM dbo.ACA_ParametroAcademico WITH(NOLOCK) WHERE pac_chave LIKE 'TIPO_DISCIPLINA_ENRIQUECIMENTO_CURRICULAR'

	DECLARE @tur_tipo INT,
			@fav_id INT,
			@tipoLancamento TINYINT,
			@controleOrdemDisciplina BIT,
			@percentualMinimoFrequencia DECIMAL(5,2),
			@tpc_ordem INT

	SELECT TOP 1 @tur_tipo = tur_tipo,
				 @fav_id = fav_id 
	FROM TUR_Turma Tur WITH(NOLOCK) 
	WHERE tur_id = @tur_id

	SELECT TOP 1 
		@tipoLancamento = fav_tipoLancamentoFrequencia,
		@percentualMinimoFrequencia = percentualMinimoFrequencia
	FROM ACA_FormatoAvaliacao fav WITH(NOLOCK)
	WHERE fav_id = @fav_id

	SELECT TOP 1 @tpc_ordem = Tpc.tpc_ordem 
	FROM ACA_TipoPeriodoCalendario AS Tpc WITH(NOLOCK) 
	WHERE Tpc.tpc_id = @tpc_id

	SELECT TOP 1 @controleOrdemDisciplina = CASE WHEN pac_valor = 'True' THEN 1 ELSE 0 END
	FROM ACA_ParametroAcademico WITH(NOLOCK)
	WHERE pac_chave = 'CONTROLAR_ORDEM_DISCIPLINAS'

	DECLARE @fav_calculoQtdeAulasDadas TINYINT = 1 --Automatico

	-- Disciplinas da turma do tipo enriquecimento curricular
	DECLARE @tbTurmaDisciplina TABLE (tud_id BIGINT NULL, tud_tipo TINYINT, dis_id INT, tds_ordem INT, ComponenteCurricular VARCHAR(200), tds_id INT)
	DECLARE @tbTurmaDisciplinaAuxiliar TABLE (tud_id BIGINT NULL, tud_tipo TINYINT, tds_id INT)
	INSERT INTO @tbTurmaDisciplina
	SELECT
		tud.tud_id,
		tud.tud_tipo,
		DIS.dis_id,
		Tds.tds_ordem,
		CASE WHEN @tud_id IS NULL THEN ISNULL(DIS.dis_nomeAbreviado, DIS.dis_nome) ELSE tud.tud_nome END,
		DIS.tds_id
	FROM
		TUR_TurmaRelTurmaDisciplina trt WITH(NOLOCK)
		INNER JOIN TUR_TurmaDisciplina tud WITH(NOLOCK)
			ON trt.tud_id = tud.tud_id
			AND Tud.tud_situacao <> 3
			AND tud.tud_tipo NOT IN (17, 19) --DocenciaCompartilhada/TerritorioSaber
		INNER JOIN TUR_TurmaDisciplinaRelDisciplina AS TUD_DIS WITH (NOLOCK)
			ON tud.tud_id = TUD_DIS.tud_id
		INNER JOIN ACA_Disciplina AS DIS WITH (NOLOCK)
			ON TUD_DIS.dis_id = DIS.dis_id
		INNER JOIN ACA_TipoDisciplina AS Tds
			ON Tds.tds_id = DIS.tds_id			
	WHERE
		trt.tur_id = @tur_id
		AND (@tud_id IS NULL OR trt.tud_id = @tud_id)
		AND (tud.tud_tipo = 18 OR EXISTS(SELECT TOP 1 TdsE.tds_id FROM @tbTdsIdsEnriquecimentoEscolar AS TdsE 
										 WHERE DIS.tds_id = TdsE.tds_id	))

	INSERT INTO @tbTurmaDisciplinaAuxiliar (tud_id, tud_tipo, tds_id)
	SELECT
		tud_id
		, tud_tipo
		, tds_id
	FROM @tbTurmaDisciplina

	-- Alunos da turma								  
	DECLARE @MatriculasBoletimDaTurma AS TipoTabela_MatriculasBoletim;
		
	DECLARE @Matriculas TABLE (alu_id BIGINT NOT NULL, mtu_id INT NOT NULL, mtd_id INT NOT NULL, tur_id BIGINT, tpc_id INT, tpc_ordem INT, tud_id BIGINT, fav_id INT
	, registroExterno BIT, PossuiSaidaPeriodo BIT, variacaoFrequencia DECIMAL(18,3)
	, PRIMARY KEY (alu_id, mtu_id, mtd_id, tpc_id));
		
	 --Se for turma de eletiva do aluno, carrega os alunos que devem aparecer na tela de efetivação
	IF ( @tur_tipo IN (2,3) ) 
	BEGIN
		-- Turma eletiva de aluno ou multisseriada, buscar matrículas por aluno.
		DECLARE @tbMatriculaAlunos TipoTabela_AlunoMatriculaTurma;
		INSERT INTO @tbMatriculaAlunos (alu_id, mtu_id)
		SELECT Mtd.alu_id, MAX(Mtd.mtu_id)
		FROM MTR_MatriculaTurmaDisciplina Mtd WITH (NOLOCK)
		INNER JOIN @tbTurmaDisciplina Tud 
			ON Tud.tud_id = Mtd.tud_id
		WHERE
			-- Busca mtus dos alunos pelo MTD - o mtd está ligado ao tud informado, e o mtu_id aponta pra outra turma (turma normal).
			Mtd.mtd_situacao <> 3
		GROUP BY alu_id	
		
		INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
		Select mb.alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mb.mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes 
		  from MTR_MatriculasBoletim mb WITH (NOLOCK)
			   inner join @tbMatriculaAlunos amt on amt.alu_id = mb.alu_id and amt.mtu_id = mb.mtu_origemDados

		IF not exists (select top 1 alu_id from @MatriculasBoletimDaTurma)
		begin
			INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
				PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
				PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
			EXEC NEW_MTR_MatriculaTurma_MatriculasBoletim_Alunos
				@tbMatriculaTurma = @tbMatriculaAlunos
		end
	END
	ELSE IF (@tur_tipo = 4)
	BEGIN
		-- Turma eletiva de aluno ou multisseriada, buscar matrículas por aluno.
		DECLARE @tbMatriculaAlunosMultisseriada TipoTabela_AlunoMatriculaTurma;
		INSERT INTO @tbMatriculaAlunosMultisseriada (alu_id, mtu_id)
		SELECT Mtd.alu_id, MAX(Mtd.mtu_id)
		FROM TUR_TurmaDisciplinaMultisseriada tdm WITH (NOLOCK)
		INNER JOIN MTR_MatriculaTurmaDisciplina Mtd WITH (NOLOCK)
			ON Mtd.alu_id = tdm.alu_id
			AND Mtd.mtu_id = tdm.mtu_id
			AND Mtd.mtd_id = tdm.mtd_id
			AND Mtd.mtd_situacao <> 3
		INNER JOIN @tbTurmaDisciplina Tud 
			ON Tud.tud_id = Mtd.tud_id	
		GROUP BY mtd.alu_id	
		
		INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
		Select mb.alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mb.mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes 
		  from MTR_MatriculasBoletim mb WITH (NOLOCK)
			   inner join @tbMatriculaAlunosMultisseriada amt on amt.alu_id = mb.alu_id and amt.mtu_id = mb.mtu_origemDados

		IF not exists (select top 1 alu_id from @MatriculasBoletimDaTurma)
		begin
			INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
				PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
				PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
			EXEC NEW_MTR_MatriculaTurma_MatriculasBoletim_Alunos
				@tbMatriculaTurma = @tbMatriculaAlunosMultisseriada
		end
	END
	 --Se for turma normal, carrega os alunos de acordo com o boletim
	ELSE
	BEGIN
		INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
			PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
			PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
		select mb.alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mb.mtu_id,mb.tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mb.mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes
		  from MTR_MatriculasBoletim mb
			   inner join (select alu_id, mtu_id, ROW_NUMBER() OVER(PARTITION BY alu_id 
														   ORDER BY mtu_id desc) as linha
							 from MTR_MatriculaTurma WITH (NOLOCK) 
							where mtu_situacao <> 3
							  and tur_id = @tur_id) mtu 
					   on mtu.alu_id = mb.alu_id and mtu.mtu_id = mb.mtu_origemDados
		 where mtu.linha = 1 --para evitar casos onde o aluno tem dois mtus para o mesmo tur_id
		   
		IF not exists (select top 1 alu_id from @MatriculasBoletimDaTurma)
		begin
			INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
				PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
				PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
			EXEC NEW_MTR_MatriculaTurma_MatriculasBoletim_DaTurma
				@tur_id = @tur_id;
		end
	END

	--Busca a matrícula na mtd ligada à disciplina (pelo tds_id), em cada COC, da lista de mtus (MatriculasBoletim).
	INSERT INTO @Matriculas (alu_id, mtu_id, mtd_id, fav_id, tpc_id, tpc_ordem, tud_id, tur_id, registroExterno, PossuiSaidaPeriodo, variacaoFrequencia)
	SELECT
		Mtr.alu_id
		, Mtr.mtu_id
		, Mtd.mtd_id
		, Mtr.fav_id
		, Mtr.tpc_id
		, Mtr.tpc_ordem
		, Mtd.tud_id
		, Mtr.tur_id
		, Mtr.registroExterno
		, Mtr.PossuiSaidaPeriodo
		, FAV.fav_variacao
	FROM @MatriculasBoletimDaTurma Mtr
	INNER JOIN MTR_MatriculaTurmaDisciplina Mtd WITH (NOLOCK)
		ON Mtd.alu_id = Mtr.alu_id
		AND Mtd.mtu_id = Mtr.mtu_id
		AND Mtd.mtd_situacao <> 3
	INNER JOIN TUR_TurmaDisciplina Tud WITH(NOLOCK)
		ON Mtd.tud_id = Tud.tud_id
	INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDis WITH (NOLOCK)
		ON RelDis.tud_id = Mtd.tud_id
	INNER JOIN ACA_Disciplina Dis WITH (NOLOCK)
		ON RelDis.dis_id = Dis.dis_id
	INNER JOIN dbo.ACA_FormatoAvaliacao FAV WITH (NOLOCK)
		ON	FAV.fav_id = Mtr.fav_id
		AND FAV.fav_situacao <> 3	
	WHERE
		Mtr.mtu_id IS NOT NULL
		AND Mtr.MesmoCalendario = 1
		AND Mtr.PeriodosEquivalentes = 1
		--AND Mtr.PossuiSaidaPeriodo = 0
		--AND Mtr.registroExterno = 0
		AND (tud.tud_tipo = 18 OR EXISTS(SELECT TOP 1 TdsE.tds_id FROM @tbTdsIdsEnriquecimentoEscolar AS TdsE 
										 WHERE DIS.tds_id = TdsE.tds_id	))

	-- Aulas e Faltas
	DECLARE @tud_tipo TINYINT;
	DECLARE @tds_id INT;
	DECLARE @SomatorioAulasFaltas TABLE (tds_id INT NOT NULL, alu_id BIGINT NOT NULL, aulas INT, faltas INT, faltasReposicao INT, compensadas INT);

	WHILE ((SELECT COUNT(tud_id) FROM @tbTurmaDisciplinaAuxiliar) > 0)
	BEGIN
		SELECT TOP 1
			@tud_id = tud_id
			, @tud_tipo = tud_tipo
			, @tds_id = tds_id
		FROM 		
			@tbTurmaDisciplinaAuxiliar

		-- Calculo das faltas
		IF (@tud_tipo = 15)
		BEGIN
			;WITH TabelaPeriodosAnteriores AS (
				SELECT 
					tpc.tpc_id, 
					ava.ava_id, 
					ava.fav_id 
				FROM dbo.ACA_TipoPeriodoCalendario AS tpc WITH (NOLOCK) 
				INNER JOIN dbo.ACA_Avaliacao AS ava WITH (NOLOCK)
					ON ava.fav_id = @fav_id
					AND tpc.tpc_id = ava.tpc_id	
					AND ava.ava_situacao <> 3	
				WHERE tpc.tpc_id = @tpc_id
				--WHERE tpc_ordem <= (SELECT tpc_ordem FROM dbo.ACA_TipoPeriodoCalendario WITH (NOLOCK)
				--					WHERE tpc_id = @tpc_id)
			)
			, TabelaFaltasAulas AS (
				SELECT * 
				FROM FN_Select_FaltasAulasBy_TurmaDisciplina(@tipoLancamento, @tpc_id, @tud_id, @fav_calculoQtdeAulasDadas, DEFAULT)
			)
			, Compensacoes AS 
			(
				-- Trazer as compensações de cada bimestre agrupadas, para trazer um registro único por bimestre.
				SELECT
					mat.alu_id, mat.mtu_id, mat.mtd_id, mat.tpc_id
					, SUM(cpa.cpa_quantidadeAulasCompensadas) AS cpa_quantidadeAulasCompensadas
				FROM @Matriculas AS mat
				INNER JOIN CLS_CompensacaoAusencia cpa WITH (NOLOCK)
					ON cpa.tud_id = @tud_id
					AND mat.tpc_id = cpa.tpc_id
					AND cpa.cpa_situacao = 1
				INNER JOIN CLS_CompensacaoAusenciaAluno caa WITH (NOLOCK)
					ON  caa.tud_id = cpa.tud_id
					AND caa.cpa_id = cpa.cpa_id
					AND caa.caa_situacao = 1
					AND caa.alu_id = mat.alu_id
				GROUP BY mat.alu_id, mat.mtu_id, mat.mtd_id, mat.tpc_id
			)
			
			INSERT INTO @SomatorioAulasFaltas (tds_id, alu_id, faltas, faltasReposicao, aulas, compensadas)
			SELECT 
				Dis.tds_id,
				mat.alu_id,				
				SUM(CASE WHEN @tpc_id = mat.tpc_id
							THEN COALESCE(atd.atd_numeroFaltas,qtfaltas,0)
						ELSE  ISNULL(atd.atd_numeroFaltas,0)
					END) AS faltas,
				ISNULL(qtFaltasReposicao,0) AS faltasReposicao,
				SUM(CASE WHEN @tpc_id = mat.tpc_id
							THEN COALESCE(atd.atd_numeroAulas,qtAulas,0)
						ELSE  ISNULL(atd.atd_numeroAulas,0)
					END) AS aulas,	
				SUM(CASE WHEN @tpc_id <> mat.tpc_id 
					THEN ISNULL(atd.atd_ausenciasCompensadas,0) 
					ELSE ( 
							CASE WHEN Atd.atd_id IS NOT NULL 
							THEN ISNULL(atd.atd_ausenciasCompensadas, 0) 
							ELSE ISNULL(cpa.cpa_quantidadeAulasCompensadas,0) 
							END 
						 ) 
					END) AS compensadas
			FROM TUR_TurmaDisciplinaMultisseriada tdm WITH (NOLOCK)
				INNER JOIN @Matriculas AS mat
					ON mat.alu_id = tdm.alu_id
					AND mat.mtu_id = tdm.mtu_id
					AND mat.mtd_id = tdm.mtd_id
				INNER JOIN TabelaPeriodosAnteriores tpa
					ON tpa.tpc_id = mat.tpc_id		
				INNER JOIN TUR_TurmaDisciplinaRelDisciplina relDis WITH(NOLOCK)
					ON relDis.tud_id = mat.tud_id
				INNER JOIN ACA_Disciplina Dis WITH(NOLOCK)
					ON Dis.dis_id = relDis.dis_id
					AND Dis.dis_situacao <> 3
				INNER JOIN @tbTurmaDisciplina Tud
					ON Tud.dis_id = relDis.dis_id
					AND Tud.tds_id = @tds_id 
				LEFT JOIN CLS_AlunoAvaliacaoTurmaDisciplina Atd WITH (NOLOCK)
					ON  Atd.tud_id = mat.tud_id
					AND Atd.alu_id = mat.alu_id
					AND Atd.mtu_id = mat.mtu_id
					AND Atd.mtd_id = mat.mtd_id
					AND atd.fav_id = tpa.fav_id
					AND atd.ava_id = tpa.ava_id
					AND Atd.atd_situacao <> 3
				LEFT JOIN TabelaFaltasAulas tfa 
					ON  tfa.alu_id = mat.alu_id
					AND tfa.mtu_id = mat.mtu_id
					AND tfa.mtd_id = mat.mtd_id
				LEFT JOIN Compensacoes Cpa
					ON Cpa.alu_id = mat.alu_id
					AND Cpa.mtu_id = mat.mtu_id
					AND Cpa.mtd_id = mat.mtd_id
					AND Cpa.tpc_id = mat.tpc_id			
			GROUP BY Dis.tds_id, mat.alu_id, qtFaltasReposicao
		END
		ELSE 
		BEGIN
			;WITH TabelaPeriodosAnteriores AS (
				SELECT 
					tpc.tpc_id, 
					ava.ava_id, 
					ava.fav_id 
				FROM dbo.ACA_TipoPeriodoCalendario AS tpc WITH (NOLOCK) 
				INNER JOIN dbo.ACA_Avaliacao AS ava WITH (NOLOCK)
					ON ava.fav_id = @fav_id
					AND tpc.tpc_id = ava.tpc_id	
					AND ava.ava_situacao <> 3	
				WHERE tpc.tpc_id = @tpc_id
				--WHERE tpc_ordem <= (SELECT tpc_ordem FROM dbo.ACA_TipoPeriodoCalendario WITH (NOLOCK)
				--					 WHERE tpc_id = @tpc_id)
			)
			, TabelaFaltasAulas AS (
				SELECT * 
				FROM FN_Select_FaltasAulasBy_TurmaDisciplina(@tipoLancamento, @tpc_id, @tud_id, @fav_calculoQtdeAulasDadas, DEFAULT)
			)
			, Compensacoes AS 
			(
				-- Trazer as compensações de cada bimestre agrupadas, para trazer um registro único por bimestre.
				SELECT
					mat.alu_id, mat.mtu_id, mat.mtd_id, mat.tpc_id
					, SUM(cpa.cpa_quantidadeAulasCompensadas) AS cpa_quantidadeAulasCompensadas
				FROM @Matriculas AS mat
				INNER JOIN CLS_CompensacaoAusencia cpa WITH (NOLOCK)
					ON cpa.tud_id = @tud_id
					AND mat.tpc_id = cpa.tpc_id
					AND cpa.cpa_situacao = 1
				INNER JOIN CLS_CompensacaoAusenciaAluno caa WITH (NOLOCK)
					ON  caa.tud_id = cpa.tud_id
					AND caa.cpa_id = cpa.cpa_id
					AND caa.caa_situacao = 1
					AND caa.alu_id = mat.alu_id
				GROUP BY mat.alu_id, mat.mtu_id, mat.mtd_id, mat.tpc_id
			)		
					
			INSERT INTO @SomatorioAulasFaltas (tds_id, alu_id, faltas, faltasReposicao, aulas, compensadas)
			SELECT 
				Dis.tds_id,
				mat.alu_id,
				SUM(CASE WHEN @tpc_id = mat.tpc_id
							THEN COALESCE(atd.atd_numeroFaltas,qtfaltas,0)
						ELSE  ISNULL(atd.atd_numeroFaltas,0)
					END) AS faltas,
				ISNULL(qtFaltasReposicao,0) AS faltasReposicao, 
				SUM(CASE WHEN @tpc_id = mat.tpc_id
							THEN COALESCE(atd.atd_numeroAulas,qtAulas,0)
						ELSE  ISNULL(atd.atd_numeroAulas,0)
					END) AS aulas,	
				SUM(CASE WHEN @tpc_id <> mat.tpc_id 
					THEN ISNULL(atd.atd_ausenciasCompensadas,0) 
					ELSE ( 
							CASE WHEN Atd.atd_id IS NOT NULL 
							THEN ISNULL(atd.atd_ausenciasCompensadas, 0) 
							ELSE ISNULL(cpa.cpa_quantidadeAulasCompensadas,0) 
							END 
						 ) 
					END) AS compensadas
			FROM @Matriculas AS mat
				INNER JOIN TabelaPeriodosAnteriores tpa
					ON tpa.tpc_id = mat.tpc_id	
				INNER JOIN TUR_TurmaDisciplinaRelDisciplina relDis WITH(NOLOCK)
					ON relDis.tud_id = mat.tud_id
				INNER JOIN ACA_Disciplina Dis WITH(NOLOCK)
					ON Dis.dis_id = relDis.dis_id
					AND Dis.dis_situacao <> 3
				INNER JOIN @tbTurmaDisciplina Tud
					ON Tud.dis_id = relDis.dis_id
					AND Tud.tds_id = @tds_id 	
				LEFT JOIN CLS_AlunoAvaliacaoTurmaDisciplina Atd WITH (NOLOCK)
					ON  Atd.tud_id = mat.tud_id
					AND Atd.alu_id = mat.alu_id
					AND Atd.mtu_id = mat.mtu_id
					AND Atd.mtd_id = mat.mtd_id
					AND atd.fav_id = tpa.fav_id
					AND atd.ava_id = tpa.ava_id
					AND Atd.atd_situacao <> 3
				LEFT JOIN TabelaFaltasAulas tfa 
					ON  tfa.alu_id = mat.alu_id
					AND tfa.mtu_id = mat.mtu_id
					AND tfa.mtd_id = mat.mtd_id
				LEFT JOIN Compensacoes Cpa
					ON Cpa.alu_id = mat.alu_id
					AND Cpa.mtu_id = mat.mtu_id
					AND Cpa.mtd_id = mat.mtd_id
					AND Cpa.tpc_id = mat.tpc_id	
			GROUP BY Dis.tds_id, mat.alu_id, qtFaltasReposicao, atd.atd_numeroFaltas   
		END   
		
		DELETE TOP(1) FROM @tbTurmaDisciplinaAuxiliar
	END

	-- Resultado
	/* [Carla 19/07/2013]
		Excluir os registros que não devem ser exibidos na tela. Esses registros devem ser trazidos para buscar
		a nota do bimestre anterior, caso seja recuperação.
	*/
	DELETE FROM @Matriculas
	WHERE
		registroExterno = 1
		-- Se possuir uma saída no período, não exibe o aluno.
		OR PossuiSaidaPeriodo = 1

	DECLARE @DadosFechamentoAcumulado TABLE
	(tud_id BIGINT, alu_id BIGINT NOT NULL, mtu_id INT NOT NULL, mtd_id INT NOT NULL, atd_ausenciasCompensadas INT,
	  atd_numeroFaltas int, atd_numeroAulas INT
	PRIMARY KEY (tud_id, alu_id, mtu_id, mtd_id))

	INSERT INTO @DadosFechamentoAcumulado
	(tud_id, alu_id, mtu_id, mtd_id, atd_ausenciasCompensadas, atd_numeroFaltas, atd_numeroAulas)	
	(
		SELECT
			AtdAcum.tud_id
			, AtdAcum.alu_id
			, AtdAcum.mtu_id
			, AtdAcum.mtd_id
			, SUM(ISNULL(AtdAcum.atd_ausenciasCompensadas, 0)) AS atd_ausenciasCompensadas
			, SUM(ISNULL(AtdAcum.atd_numeroFaltas, 0)) AS atd_numeroFaltas
			, SUM(ISNULL(AtdAcum.atd_numeroAulas, 0)) AS atd_numeroAulas
		FROM 
			@Matriculas Mtr
			INNER JOIN @tbTurmaDisciplina Tud
				ON Tud.tud_id = Mtr.tud_id
			INNER JOIN MTR_MatriculaTurmaDisciplina Mtd WITH (NOLOCK)
				ON Mtr.alu_id = Mtd.alu_id
				AND Mtr.mtu_id = Mtd.mtu_id
				AND Mtr.mtd_id = Mtd.mtd_id  
			INNER JOIN CLS_AlunoAvaliacaoTurmaDisciplina AtdAcum WITH(NOLOCK)
				ON AtdAcum.tud_id = Mtd.tud_id
				AND AtdAcum.alu_id = Mtd.alu_id
				AND AtdAcum.mtu_id = Mtd.mtu_id
				AND AtdAcum.mtd_id = Mtd.mtd_id
				AND Mtr.fav_id = AtdAcum.fav_id
			INNER JOIN ACA_Avaliacao Ava WITH(NOLOCK)
				ON AtdAcum.fav_id = Ava.fav_id
				AND AtdAcum.ava_id = Ava.ava_id
				AND Mtr.tpc_id = Ava.tpc_id
		WHERE 
			-- Busca dados dos bimestres anteriores.
			Mtr.tpc_ordem < @tpc_ordem
		GROUP BY
			AtdAcum.tud_id
			, AtdAcum.alu_id
			, AtdAcum.mtu_id
			, AtdAcum.mtd_id
	)
		
	; WITH SomatorioAcumulado AS
	(
		SELECT 
			alu_id,
			tds_id,
			SUM(aulas) AS aulas,
			SUM(faltas) AS faltas,
			SUM(faltasReposicao) AS faltasReposicao,
			SUM(compensadas) AS compensadas
		FROM @SomatorioAulasFaltas	
		GROUP BY
			tds_id, alu_id	
	)

	, BimestreFechado AS
	(
		SELECT 
			Atd.tud_id
			, Atd.alu_id 
			, Atd.mtu_id 
			, Atd.mtd_id 
			, Atd.atd_frequencia
			, Atd.atd_avaliacao  --
			, Atd.atd_avaliacaoPosConselho --
			, Atd.atd_frequenciaFinalAjustada
			, Atd.atd_numeroAulas
			, Atd.atd_numeroFaltas
			, Atd.atd_ausenciasCompensadas
		FROM @Matriculas Mtr
		INNER JOIN @tbTurmaDisciplina Tud
			ON Tud.tud_id = Mtr.tud_id
		INNER JOIN CLS_AlunoAvaliacaoTurmaDisciplina Atd WITH(NOLOCK)
			ON Atd.tud_id = Tud.tud_id
			AND Atd.alu_id = Mtr.alu_id
			AND Atd.mtu_id = Mtr.mtu_id
			AND Atd.mtd_id = Mtr.mtd_id
			AND Atd.fav_id = @fav_id
			AND Atd.atd_situacao <> 3				
		INNER JOIN ACA_Avaliacao Ava WITH(NOLOCK)
			ON Ava.fav_id = Atd.fav_id
			AND Ava.ava_id = Atd.ava_id
			AND Ava.tpc_id = @tpc_id
			AND Ava.ava_situacao <> 3			
	)
	, DadosFechamentoAcumulado AS (
		SELECT
			Mtr.tud_id
			, Mtr.alu_id
			, Mtr.mtu_id
			, Mtr.mtd_id
			, SUM(ISNULL(AtdAcum.atd_numeroFaltas, 0) + 
				CASE WHEN AtdAtual.alu_id IS NULL 
					THEN ISNULL(Calculado.faltas, 0) 
					ELSE ISNULL(AtdAtual.atd_numeroFaltas, 0) 
				END) AS TotalFaltas
			, SUM(ISNULL(AtdAcum.atd_ausenciasCompensadas, 0) + 
				CASE WHEN AtdAtual.alu_id IS NULL 
					THEN ISNULL(Calculado.compensadas, 0)
					ELSE ISNULL(AtdAtual.atd_ausenciasCompensadas, 0)
				END) AS TotalCompensacoes
			, SUM(ISNULL(AtdAcum.atd_numeroAulas, 0) + 
				CASE WHEN AtdAtual.alu_id IS NULL 
					THEN ISNULL(Calculado.aulas, 0)
					ELSE ISNULL(AtdAtual.atd_numeroAulas, 0)
				END) AS TotalAulasPrevistas
			, SUM(ISNULL(Calculado.faltasReposicao, 0)) AS qtFaltasReposicao
		FROM
			@Matriculas Mtr
			INNER JOIN TUR_TurmaDisciplinaRelDisciplina AS TudRelDis WITH(NOLOCK)
				ON Mtr.tud_id = TudRelDis.tud_id
			INNER JOIN ACA_Disciplina AS Dis WITH(NOLOCK)
				ON TudRelDis.dis_id = Dis.dis_id
			LEFT JOIN @DadosFechamentoAcumulado AS AtdAcum
				ON AtdAcum.tud_id = Mtr.tud_id	
				AND AtdAcum.alu_id = Mtr.alu_id
				AND AtdAcum.mtu_id = Mtr.mtu_id
				AND AtdAcum.mtd_id = Mtr.mtd_id
			LEFT JOIN BimestreFechado AtdAtual
				ON AtdAtual.tud_id = Mtr.tud_id	
				AND AtdAtual.alu_id = Mtr.alu_id
				AND AtdAtual.mtu_id = Mtr.mtu_id
				AND AtdAtual.mtd_id = Mtr.mtd_id
			LEFT JOIN SomatorioAcumulado AS Calculado
				ON Calculado.tds_id = Dis.tds_id
				AND Calculado.alu_id = Mtr.alu_id 
		GROUP BY
			Mtr.tud_id
			, Mtr.alu_id
			, Mtr.mtu_id
			, Mtr.mtd_id
	)
	
	, movimentacao AS (
		SELECT
			Da.alu_id,
			Da.mtu_id,
			CASE WHEN tmo_tipoMovimento IN (6, 12, 14, 15, 16)
					THEN 'TR ' + REPLACE(CONVERT(VARCHAR(10), mov.mov_dataRealizacao, 103), '/' + CAST(DATEPART(YEAR, mov.mov_dataRealizacao) AS VARCHAR(4)), '') +
						 ISNULL(' - ' + turD.tur_codigo, '')
					WHEN tmo_tipoMovimento IN (8)
					THEN 'RM' + ISNULL(' ' + turD.tur_codigo, '')
					WHEN tmo_tipoMovimento IN (11)
					THEN 'RC' + ISNULL(' ' + turD.tur_codigo, '')
					ELSE ''
			END movMsg
		FROM @Matriculas Da
		INNER JOIN MTR_Movimentacao mov WITH(NOLOCK)
			ON Da.alu_id = mov.alu_id
			AND Da.mtu_id = mov.mtu_idAnterior
			AND mov.mov_situacao <> 3
		INNER JOIN MTR_TipoMovimentacao tmo WITH(NOLOCK)
			ON mov.tmo_id = tmo.tmo_id
			AND tmo_tipoMovimento IN (6, 8, 11, 12, 14, 15, 16)
			AND tmo.tmo_situacao <> 3
		LEFT JOIN MTR_MatriculaTurma mtuD WITH(NOLOCK)
			ON mov.alu_id = mtuD.alu_id
			AND mov.mtu_idAtual = mtuD.mtu_id
		LEFT JOIN TUR_Turma turD WITH(NOLOCK)
			ON mtuD.tur_id = turD.tur_id
		LEFT JOIN ACA_CalendarioAnual calD WITH(NOLOCK)
			ON turD.cal_id = calD.cal_id
		INNER JOIN MTR_MatriculaTurma mtuO WITH(NOLOCK)
			ON mov.alu_id = mtuO.alu_id
			AND mov.mtu_idAnterior = mtuO.mtu_id
			AND mtuO.tur_id = @tur_id
		LEFT JOIN TUR_Turma turO WITH(NOLOCK)
			ON mtuO.tur_id = turO.tur_id
		LEFT JOIN ACA_CalendarioAnual calO WITH(NOLOCK)
			ON turO.cal_id = calO.cal_id
		WHERE 
			turD.tur_id IS NULL OR calD.cal_ano = calO.cal_ano --Ou não tem turma destino ou a turma destino é do mesmo ano
		GROUP BY
			Da.alu_id,
			Da.mtu_id,
			tmo_tipoMovimento,
			mov.mov_dataRealizacao,
			turD.tur_codigo
	)	
	
	SELECT
		-- IDs
		@tur_id AS tur_id
		, Tud.dis_id 
		, Mtr.alu_id
		, Tud.ComponenteCurricular AS Regencia	
		, @percentualMinimoFrequencia AS percentualMinimoFrequencia
		, (
			CASE 
				WHEN mtu.mtu_numeroChamada <= 0 THEN '-'
				ELSE CAST(mtu.mtu_numeroChamada AS VARCHAR(MAX))
			END
		) AS NumeroChamada
		, CASE WHEN @documentoOficial = 1 THEN Pes.pes_nomeOficial ELSE Pes.pes_nome END +
		  CASE WHEN ISNULL(mov.movMsg, '') = ''
			   THEN ''
		  	   ELSE ' (' + mov.movMsg + ')'
		  END AS Nome
		, ISNULL(Qtd.faltas,0) AS Faltas
		, ISNULL(Qtd.compensadas,0) AS Compensacoes 
		, ISNULL(Qtd.aulas,0) AS numeroAulas
		--, ISNULL(F.aulasPrevistas,0) AS aulasPrevistas
		, dbo.FN_Aplica_VariacaoPorcentagemFrequenciaString(
			ISNULL(CASE WHEN Bim.alu_id IS NOT NULL THEN
					-- Caso o bimestre tenha fechamento traz a frequencia final dele.
					Bim.atd_frequencia
				ELSE
					CASE WHEN Qtd.aulas > 0 THEN
					-- Se não estiver fechada, calcula de acordo com os acumulados.
					dbo.FN_Calcula_PorcentagemFrequenciaVariacao
						(Qtd.aulas, (Qtd.faltas + Qtd.faltasReposicao) - Qtd.compensadas, Mtr.variacaoFrequencia)
					-- Caso o total de aulas previstas seja 0 a frequência deve ser 100.
					ELSE 100 END
				END, 0)
			, Mtr.variacaoFrequencia) AS PorcentagemFaltas
		, dbo.FN_Aplica_VariacaoPorcentagemFrequenciaString(
			ISNULL(CASE WHEN Bim.alu_id IS NOT NULL THEN
					-- Caso o bimestre tenha fechamento traz a frequencia final dele.
					Bim.atd_frequenciaFinalAjustada
				ELSE
					CASE WHEN AtdAcum.TotalAulasPrevistas > 0 THEN
					-- Se não estiver fechada, calcula de acordo com os acumulados.
					(((CAST(AtdAcum.TotalAulasPrevistas AS DECIMAL(18,2)) - 
						(CAST(AtdAcum.TotalFaltas AS DECIMAL(18,2)) + CAST(AtdAcum.qtFaltasReposicao AS DECIMAL(18,2))
							 - CAST(AtdAcum.TotalCompensacoes AS DECIMAL(18,2)))) / CAST(AtdAcum.TotalAulasPrevistas AS DECIMAL(18,2)))) * 100 
					-- Caso o total de aulas previstas seja 0 a frequência deve ser 100.
					ELSE 100 END
				END, 0)
			,  Mtr.variacaoFrequencia)
			 AS FrequenciaFinal
		, CAST(0 AS BIT) AS ComponenteRegncia
		--, CASE WHEN Bim.alu_id IS NOT NULL THEN 1 ELSE 0 END AS bimFechado
		
		, CASE WHEN (Bim.alu_id IS NOT NULL)
					-- verifica se possui alunos e se também existe aula criada. 
					-- não é testado se tem avaliação, pois complemento curricular nao tem notas
					AND
					(SELECT COUNT(tud_id) FROM CLS_TurmaAula AS Tau	with(nolock)
										  WHERE Tau.tud_id = Tud.tud_id 
										  AND Tau.tpc_id = @tpc_id
										  AND Tau.tau_situacao <> 3) > 0
		  THEN 1 ELSE 0 END AS bimFechado
		
		, CASE WHEN (
				-- Verifico a ultima movimentacao do aluno na turma
				SELECT TOP 1 Tmo.tmo_tipoMovimento
				FROM MTR_Movimentacao Mov WITH(NOLOCK)
				INNER JOIN MTR_TipoMovimentacao Tmo WITH(NOLOCK)
					ON Tmo.tmo_id = Mov.tmo_id	
					AND Tmo.tmo_situacao <> 3
				WHERE alu_id = Mtr.alu_id
					AND mtu_idAnterior = Mtr.mtu_id
					AND mov_situacao <> 3
				ORDER BY mov_dataRealizacao DESC
			) IN (8,23,27)
			THEN 0
			ELSE 
				CASE WHEN (mtd.mtd_situacao = 5) THEN 1 ELSE 0 END 
			END AS AlunoInativo
	FROM 
		@Matriculas Mtr
		INNER JOIN @tbTurmaDisciplina Tud
			ON Tud.tud_id = Mtr.tud_id
		INNER JOIN MTR_MatriculaTurmaDisciplina Mtd WITH (NOLOCK)
			ON Mtr.alu_id = Mtd.alu_id
			AND Mtr.mtu_id = Mtd.mtu_id
			AND Mtr.mtd_id = Mtd.mtd_id      
		INNER JOIN MTR_MatriculaTurma mtu WITH (NOLOCK)
			ON mtu.alu_id = Mtd.alu_id
			AND mtu.mtu_id = Mtd.mtu_id
			AND mtu_situacao <> 3
		INNER JOIN TUR_Turma tur WITH (NOLOCK)
			ON tur.tur_id = mtu.tur_id
			AND tur.tur_situacao <> 3
		INNER JOIN ACA_AlunoCurriculo alc WITH (NOLOCK)
			ON alc.alu_id = mtu.alu_id
			AND alc.alc_id = mtu.alc_id
			AND alc.alc_situacao <> 3		
		INNER JOIN ACA_Aluno Alu WITH (NOLOCK)
			ON  Mtd.alu_id = Alu.alu_id
			AND alu_situacao <> 3
		INNER JOIN VW_DadosAlunoPessoa Pes
			ON  Alu.alu_id   = Pes.alu_id
		LEFT JOIN BimestreFechado Bim
			ON Bim.tud_id = Mtr.tud_id	
			AND Bim.alu_id = Mtr.alu_id
			AND Bim.mtu_id = Mtr.mtu_id
			AND Bim.mtd_id = Mtr.mtd_id
		LEFT JOIN SomatorioAcumulado AS Qtd
			ON Qtd.tds_id = Tud.tds_id
			AND Qtd.alu_id = Mtd.alu_id     
		LEFT JOIN DadosFechamentoAcumulado AtdAcum
			ON AtdAcum.tud_id = Mtd.tud_id
			AND AtdAcum.alu_id = Mtd.alu_id
			AND AtdAcum.mtu_id = Mtd.mtu_id
		LEFT JOIN movimentacao mov
			ON Mtr.alu_id = mov.alu_id
			AND Mtr.mtu_id = mov.mtu_id	
	WHERE 
		Mtr.tpc_id = @tpc_id
		AND mtd_situacao IN (1,5)
		AND ISNULL(mtd_numeroChamada, 0) >= 0
		AND Alu.alu_situacao <> 3
	GROUP BY
		Tud.dis_id 
		, Tud.tud_id
		, Tud.tds_ordem
		, Mtr.alu_id
		, Mtr.mtu_id
		, Tud.ComponenteCurricular
		, mtu.mtu_numeroChamada
		, Pes.pes_nomeOficial
		, Pes.pes_nome 
		, Qtd.aulas
		, Qtd.faltas
		, Qtd.faltasReposicao
		, Qtd.compensadas
		, Mtr.variacaoFrequencia
		, Mtd.mtd_situacao
		, Bim.alu_id
		, Bim.atd_frequencia
		, Bim.atd_frequenciaFinalAjustada
		, AtdAcum.TotalAulasPrevistas
		, AtdAcum.TotalFaltas
		, AtdAcum.qtFaltasReposicao
		, AtdAcum.TotalCompensacoes
		, mov.movMsg
	ORDER BY
		(
			CASE 
				WHEN ISNULL(mtu.mtu_numeroChamada, -1) <= 0 THEN 99999
				ELSE mtu.mtu_numeroChamada
			END
		)
		, Nome
		, CASE WHEN @controleOrdemDisciplina = 1 THEN Tud.tds_ordem ELSE Tud.ComponenteCurricular END
		
END	
	



GO
PRINT N'Creating [dbo].[NEW_Relatorio_0005_RelObjetosAprendizagem]'
GO
-- ========================================================================
-- Author:		Leonardo Brito
-- Create date: 28/03/2017
-- Description:	Busca os dados para o relatório de objetos de aprendizagem
-- ========================================================================
CREATE PROCEDURE [dbo].[NEW_Relatorio_0005_RelObjetosAprendizagem]
	@cal_ano INT
	, @esc_id INT
	, @uni_id INT
	, @uad_idSuperior UNIQUEIDENTIFIER
	, @MostraCodigoEscola BIT
	, @ciclosSelecionados VARCHAR(MAX)
	, @tds_id INT
	, @ent_id UNIQUEIDENTIFIER
	, @adm BIT
	, @usu_id UNIQUEIDENTIFIER
	, @gru_id UNIQUEIDENTIFIER
AS
BEGIN

	DECLARE @GuidVazio UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000'
	SET @uad_idSuperior = CASE WHEN @uad_idSuperior IS NULL OR @uad_idSuperior = '00000000-0000-0000-0000-000000000000'
							   THEN NULL ELSE @uad_idSuperior END
	SET @esc_id = CASE WHEN ISNULL(@esc_id,0) > 0 THEN @esc_id ELSE NULL END
	SET @uni_id = CASE WHEN ISNULL(@uni_id,0) > 0 THEN @uni_id ELSE NULL END

	DECLARE @tne_idMedio INT = (SELECT TOP 1 CAST(pac_valor AS INT) FROM ACA_ParametroAcademico WITH(NOLOCK) 
								WHERE pac_chave = 'TIPO_NIVEL_ENSINO_MEDIO' AND pac_situacao <> 3)
	
	DECLARE @uad_ids TABLE (uad_id UNIQUEIDENTIFIER);
	IF (@adm <> 1)
	BEGIN
		INSERT INTO @uad_ids 
		SELECT uad_id 
		FROM Synonym_FN_Select_UAs_By_PermissaoUsuario(@usu_id, @gru_id)
	END
	
	DECLARE @nomeDRE VARCHAR(250) = ''
	DECLARE @nomeEscola VARCHAR(250) = ''
	
	IF (@uad_idSuperior IS NOT NULL)
	BEGIN 
		SELECT TOP 1
			@nomeDRE = uad.uad_nome
		FROM Synonym_SYS_UnidadeAdministrativa uad WITH(NOLOCK)
		WHERE 
			uad.uad_id = @uad_idSuperior
			AND ent_id = @ent_id
			AND uad.uad_situacao <> 3
	END
	
	IF (@esc_id IS NOT NULL AND @uni_id IS NOT NULL)
	BEGIN 
		SELECT TOP 1
			@nomeEscola = CASE WHEN @MostraCodigoEscola = 1
						       THEN ISNULL(E.esc_codigo + ' - ', '')
						       ELSE ''
						  END +	
						  CASE WHEN ((UE.uni_descricao IS NULL) OR (LTRIM(RTRIM(UE.uni_descricao)) = '')) 
							   THEN E.esc_nome
						       ELSE E.esc_nome + ' / ' + UE.uni_descricao
						  END
		FROM ESC_Escola E WITH(NOLOCK)
		INNER JOIN ESC_UnidadeEscola UE WITH(NOLOCK)
			ON UE.esc_id = E.esc_id
			AND UE.uni_id = @uni_id
			AND UE.uni_situacao <> 3
		WHERE 
			E.esc_id = @esc_id 
			AND E.ent_id = @ent_id 
			AND E.esc_situacao <> 3
	END
	
	DECLARE @ciclos TABLE (tci_id INT)
	INSERT INTO @ciclos
	SELECT valor FROM FN_StringToArrayInt32(@ciclosSelecionados, ',')

	;WITH objetosAprendizagem AS (
		SELECT
			oap.oap_id,
			oap.oap_descricao,
			tci.tci_id,
			tci.tci_nome,
			tci.tci_ordem,
			oap.cal_ano
		FROM ACA_ObjetoAprendizagem oap WITH(NOLOCK)
		INNER JOIN ACA_ObjetoAprendizagemTipoCiclo oat WITH(NOLOCK)
			ON oap.oap_id = oat.oap_id
		INNER JOIN @ciclos c
			ON oat.tci_id = c.tci_id
		INNER JOIN ACA_TipoCiclo tci WITH(NOLOCK)
			ON c.tci_id = tci.tci_id
			AND tci.tci_situacao <> 3
		WHERE
			oap.cal_ano = @cal_ano
			AND oap.tds_id = @tds_id
			AND oap.oap_situacao <> 3
	)
	, tpc_Obj AS (
		SELECT
			o.oap_id,
			cap.tpc_id
		FROM objetosAprendizagem o
		INNER JOIN ACA_CalendarioAnual cal WITH(NOLOCK)
			ON o.cal_ano = cal.cal_ano
			AND cal.cal_situacao <> 3
		INNER JOIN ACA_CalendarioPeriodo cap WITH(NOLOCK)
			ON cal.cal_id = cap.cal_id
			AND cap.cap_situacao <> 3
		GROUP BY
			o.oap_id,
			cap.tpc_id
	)
	, tcp_Obj AS (
		SELECT 
			o.oap_id,
			crp.tcp_id
		FROM objetosAprendizagem o
		INNER JOIN ACA_CurriculoPeriodo crp WITH(NOLOCK)
			ON o.tci_id = crp.tci_id
			AND crp.crp_situacao <> 3
		WHERE
			crp.tcp_id IS NOT NULL
		GROUP BY
			o.oap_id,
			crp.tcp_id
	)
	, turmaBimestreObj AS (
		SELECT
			oat.oap_id,
			crp.tcp_id,
			crp.tci_id,
			oat.tpc_id,
			tur.tur_id
		FROM objetosAprendizagem oap
		INNER JOIN CLS_ObjetoAprendizagemTurmaDisciplina oat WITH(NOLOCK)
			ON oap.oap_id = oat.oap_id
		INNER JOIN TUR_TurmaDisciplina tud WITH(NOLOCK)
			ON oat.tud_id = tud.tud_id
			AND tud.tud_situacao <> 3
		INNER JOIN TUR_TurmaRelTurmaDisciplina trt WITH(NOLOCK)
			ON tud.tud_id = trt.tud_id
		INNER JOIN TUR_Turma tur WITH(NOLOCK)
			ON trt.tur_id = tur.tur_id
			AND tur.esc_id = ISNULL(@esc_id, tur.esc_id)
			AND tur.uni_id = ISNULL(@uni_id, tur.uni_id)
			AND tur.tur_situacao <> 3
		INNER JOIN ACA_CalendarioAnual cal WITH(NOLOCK)
			ON tur.cal_id = cal.cal_id
			AND cal.cal_ano = @cal_ano
			AND cal.cal_situacao <> 3
		INNER JOIN TUR_TurmaCurriculo tcr WITH(NOLOCK)
			ON tur.tur_id = tcr.tur_id
			AND tcr.tcr_situacao <> 3
		INNER JOIN ACA_CurriculoPeriodo crp WITH(NOLOCK)
			ON tcr.cur_id = crp.cur_id
			AND tcr.crr_id = crp.crr_id
			AND tcr.crp_id = crp.crp_id
			AND crp.crp_situacao <> 3
		INNER JOIN ESC_Escola esc WITH(NOLOCK)
			ON tur.esc_id = esc.esc_id
			AND esc.ent_id = @ent_id
			AND (@adm = 1 OR esc.uad_id IN (select uad_id from @uad_ids))
			AND esc.esc_controleSistema = 1
			AND esc.esc_situacao <> 3
		INNER JOIN Synonym_SYS_UnidadeAdministrativa uad WITH(NOLOCK)
			ON esc.uad_id = uad.uad_id 
			AND COALESCE(esc.uad_idSuperiorGestao, uad.uad_idSuperior, @uad_idSuperior, @GuidVazio) = COALESCE(@uad_idSuperior, esc.uad_idSuperiorGestao, uad.uad_idSuperior, @GuidVazio)
			AND uad.uad_situacao <> 3
		GROUP BY
			oat.oap_id,
			crp.tcp_id,
			crp.tci_id,
			oat.tpc_id,
			tur.tur_id
	)
	, cicloMedio AS (
		SELECT
			crp.tcp_id,
			cur.tne_id,
			tne.tne_nome,
			MAX(tci.tci_id) AS tci_id,
			MAX(tci.tci_ordem) AS tci_ordem
		FROM 
			ACA_Curso cur WITH(NOLOCK)
		INNER JOIN ACA_CurriculoPeriodo crp WITH(NOLOCK)
			ON cur.cur_id = crp.cur_id
			AND crp.crp_situacao <> 3
		INNER JOIN ACA_TipoCiclo tci WITH(NOLOCK)
			ON crp.tci_id = tci.tci_id
			AND tci.tci_situacao <> 3
		INNER JOIN ACA_TipoNivelEnsino tne WITH(NOLOCK)
			ON cur.tne_id = tne.tne_id
			AND tne.tne_situacao <> 3
		WHERE
			cur.tne_id = @tne_idMedio
			AND cur.cur_situacao <> 3
		GROUP BY
			crp.tcp_id,
			cur.tne_id,
			tne.tne_nome
	)
	, totalTurmas AS (
		SELECT
			crp.tcp_id,
			crp.tci_id,
			cur.tne_id,
			COUNT(tur.tur_id) AS totalTurmas
		FROM
			TUR_Turma tur WITH(NOLOCK)
		INNER JOIN ACA_CalendarioAnual cal WITH(NOLOCK)
			ON tur.cal_id = cal.cal_id
			AND cal.cal_ano = @cal_ano
			AND cal.cal_situacao <> 3
		INNER JOIN TUR_TurmaCurriculo tcr WITH(NOLOCK)
			ON tur.tur_id = tcr.tur_id
			AND tcr.tcr_situacao <> 3
		INNER JOIN ACA_CurriculoPeriodo crp WITH(NOLOCK)
			ON tcr.cur_id = crp.cur_id
			AND tcr.crr_id = crp.crr_id
			AND tcr.crp_id = crp.crp_id
			AND crp.crp_situacao <> 3
		INNER JOIN ACA_Curso cur WITH(NOLOCK)
			ON crp.cur_id = cur.cur_id
			AND cur.cur_situacao <> 3
		INNER JOIN ESC_Escola esc WITH(NOLOCK)
			ON tur.esc_id = esc.esc_id
			AND esc.ent_id = @ent_id
			AND (@adm = 1 OR esc.uad_id IN (select uad_id from @uad_ids))
			AND esc.esc_controleSistema = 1
			AND esc.esc_situacao <> 3
		INNER JOIN Synonym_SYS_UnidadeAdministrativa uad WITH(NOLOCK)
			ON esc.uad_id = uad.uad_id 
			AND COALESCE(esc.uad_idSuperiorGestao, uad.uad_idSuperior, @uad_idSuperior, @GuidVazio) = COALESCE(@uad_idSuperior, esc.uad_idSuperiorGestao, uad.uad_idSuperior, @GuidVazio)
			AND uad.uad_situacao <> 3
		WHERE 
			tur.esc_id = ISNULL(@esc_id, tur.esc_id)
			AND tur.uni_id = ISNULL(@uni_id, tur.uni_id)
			AND tur.tur_situacao <> 3
		GROUP BY
			tcp_id,
			crp.tci_id,
			cur.tne_id
	)
	, totalTurmasObjPeriodo AS (
		SELECT
			oap_id,
			tcp_id,
			tci_id,
			tpc_id,
			COUNT(tur_id) AS totalTurmasObjeto
		FROM
			turmaBimestreObj
		GROUP BY
			oap_id,
			tcp_id,
			tci_id,
			tpc_id
	)

	SELECT
		oap.oap_id,
		oap.oap_descricao,
		CASE WHEN ISNULL(cm.tne_id, 0) = @tne_idMedio
			 THEN cm.tci_id
			 ELSE oap.tci_id
		END AS tci_id,
		CASE WHEN ISNULL(cm.tne_id, 0) = @tne_idMedio
			 THEN cm.tne_nome
			 ELSE oap.tci_nome
		END AS tci_nome,
		CASE WHEN ISNULL(cm.tne_id, 0) = @tne_idMedio
			 THEN cm.tci_ordem
			 ELSE oap.tci_ordem
		END AS tci_ordem,
		tcrp.tcp_id,
		tcrp.tcp_descricao,
		tcrp.tcp_ordem,
		tpc.tpc_id,
		ISNULL(tpc.tpc_nomeAbreviado, tpc.tpc_nome) AS tpc_nome,
		tpc.tpc_ordem,
		ISNULL(ttr.totalTurmas, 0) AS totalTurmas,
		ISNULL(tto.totalTurmasObjeto, 0) AS totalTurmasObjeto,
		CASE WHEN ISNULL(tto.totalTurmasObjeto, 0) > ISNULL(ttr.totalTurmas, 0)
			 THEN '100'
			 WHEN ISNULL(tto.totalTurmasObjeto, 0) > 0 AND ISNULL(ttr.totalTurmas, 0) > 0
			 THEN CAST(CAST((CAST(tto.totalTurmasObjeto AS DECIMAL(10,0)) * 100 / CAST(ttr.totalTurmas AS DECIMAL(10,0))) AS DECIMAL(10,0)) AS VARCHAR(3))
			 ELSE '0'
		END + '%' AS percentTurma,
		ISNULL(@nomeDRE, '') AS nomeDRE,
		ISNULL(@nomeEscola, '') AS nomeEscola,
		tme.tme_id,
		tme.tme_nome
	FROM
		objetosAprendizagem oap
	INNER JOIN tpc_Obj tpo 
		ON oap.oap_id = tpo.oap_id
	INNER JOIN tcp_Obj tco
		ON oap.oap_id = tco.oap_id
	INNER JOIN ACA_TipoCurriculoPeriodo tcrp WITH(NOLOCK)
		ON tco.tcp_id = tcrp.tcp_id
		AND tcrp.tcp_situacao <> 3
	INNER JOIN ACA_TipoModalidadeEnsino tme WITH(NOLOCK)
		ON tcrp.tme_id = tme.tme_id
		AND tme.tme_situacao <> 3
	INNER JOIN ACA_TipoPeriodoCalendario tpc WITH(NOLOCK)
		ON tpo.tpc_id = tpc.tpc_id
		AND tpc.tpc_situacao <> 3
	INNER JOIN totalTurmas ttr 
		ON tcrp.tcp_id = ttr.tcp_id
		AND oap.tci_id = ttr.tci_id
	LEFT JOIN totalTurmasObjPeriodo tto 
		ON oap.oap_id = tto.oap_id
		AND tcrp.tcp_id = tto.tcp_id
		AND tpc.tpc_id = tto.tpc_id
		AND oap.tci_id = tto.tci_id
	LEFT JOIN cicloMedio cm
		ON ttr.tne_id = cm.tne_id
		AND ttr.tcp_id = cm.tcp_id
	ORDER BY
		oap.tci_ordem,
		oap.tci_nome,
		tcrp.tcp_ordem,
		tcrp.tcp_descricao,
		tpc.tpc_ordem,
		tpc.tpc_nome
END
GO
PRINT N'Altering [dbo].[NEW_MTR_MatriculaTurmaDisciplina_SelectBy_TurmaDisciplinaFormato_Final_ByAluno]'
GO


-- =============================================
-- Author:		Marcia Haga
-- Create date: 08/10/2014
-- Description:	Retorna os dados da efetivação final, filtrado por aluno.

---- Alterado: Marcia Haga - 09/10/2014
---- Description: Alterado para retornar se o aluno estava fora da rede durante o período.

---- Alterado: Marcia Haga - 04/11/2014
---- Description: Alterado para retornar a frequencia final ajustada.

---- Alterado: Marcia Haga - 28/11/2014
---- Description: Corrigido retorno quando existe movimentacao de aluno na mesma turma.

---- Alterado: Daniel Jun Suguimoto - 03/12/2014
---- Description: Alterado para considerar lançamento de notas no listão para habilitar o fechamento final.

---- Alterado: Marcia Haga - 30/03/2015
---- Description: Alterado para retornar aluno fora da rede se nao possuir matricula no bimestre, 
---- independente de possuir nota ou nao.

---- Alterado: Marcia Haga - 14/04/2015
---- Description: Adicionada validacao se existe nota lancada no Listao para o ultimo periodo.
---- Alterado para trazer o numero de aulas, faltas e compensacoes do ultimo periodo.

---- Alterado: Marcia Haga - 30/04/2015
---- Description: Corrigida validacao de aluno fora da rede.

---- Alterado: Marcia Haga - 04/05/2015
---- Description: Corrigido retorno das faltas de reposicao para o calculo da frequencia final ajustada,
---- pois estava retornando registros duplicados.
-- =============================================
ALTER PROCEDURE [dbo].[NEW_MTR_MatriculaTurmaDisciplina_SelectBy_TurmaDisciplinaFormato_Final_ByAluno]
	@tud_id BIGINT
	, @tur_id BIGINT
	, @ava_id INT
	, @ordenacao INT
	, @fav_id INT
	, @tipoEscalaDisciplina TINYINT
	, @tipoEscalaDocente TINYINT
	, @tur_tipo TINYINT
	, @cal_id INT
	, @tipoLancamento TINYINT	
	, @fav_calculoQtdeAulasDadas TINYINT
	, @tipoDocente TINYINT
	, @permiteAlterarResultado BIT
	, @alunos TipoTabela_AlunoMatriculaTurma READONLY
	
AS
BEGIN

	SET TRANSACTION ISOLATION LEVEL SNAPSHOT

	DECLARE @escolaId INT;
	SELECT TOP 1 @escolaId = esc_id
	FROM TUR_Turma WITH(NOLOCK)
	WHERE tur_id = @tur_id

	DECLARE @ultimoPeriodo INT;
	SELECT TOP 1 @ultimoPeriodo = tpc_id 
	FROM ACA_CalendarioPeriodo WITH(NOLOCK)
	WHERE 
		cal_id = @cal_id AND cap_situacao <> 3 
	ORDER BY cap_dataFim DESC
		
	DECLARE @MatriculasBoletimDaTurma AS TipoTabela_MatriculasBoletim;

	DECLARE @Matriculas TABLE (alu_id BIGINT NOT NULL, mtu_id INT NOT NULL, mtd_id INT NOT NULL, tur_id BIGINT, tpc_id INT, tpc_ordem INT, tud_id BIGINT, fav_id INT
		, registroExterno BIT, PossuiSaidaPeriodo BIT, esc_id INT
		, PRIMARY KEY (alu_id, mtu_id, mtd_id, tpc_id));

	DECLARE @tds_id INT = 
		(
			 --Busca o tipo de disciplina para filtrar os mtds abaixo.
			SELECT Dis.tds_id
			FROM TUR_TurmaDisciplinaRelDisciplina RelDis WITH(NOLOCK)
			INNER JOIN ACA_Disciplina Dis WITH(NOLOCK)
				ON Dis.dis_id = RelDis.dis_id
			WHERE
				RelDis.tud_id = @tud_id
		)

	IF (@tur_tipo IN (2,3))
	BEGIN
		-- Turma eletiva de aluno ou multisseriada, buscar matrículas por aluno.
		DECLARE @tbMatriculaAlunos TipoTabela_AlunoMatriculaTurma;
		INSERT INTO @tbMatriculaAlunos (alu_id, mtu_id)
		SELECT Mtd.alu_id, MAX(Mtd.mtu_id)
		FROM @alunos alu
		INNER JOIN MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
			ON Mtd.alu_id = alu.alu_id
		WHERE
			-- Busca mtus dos alunos pelo MTD - o mtd está ligado ao tud informado, e o mtu_id aponta pra outra turma (turma normal).
			Mtd.tud_id = @tud_id
			AND Mtd.mtd_situacao <> 3
		GROUP BY Mtd.alu_id	

		INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
			PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
			PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
		Select mb.alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mb.mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes 
		  from MTR_MatriculasBoletim mb  -- WITH (NOLOCK)
			   inner join @tbMatriculaAlunos amt on amt.alu_id = mb.alu_id and amt.mtu_id = mb.mtu_origemDados
		
		IF not exists (select top 1 alu_id from @MatriculasBoletimDaTurma)
		begin
			INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
				PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
				PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
			EXEC NEW_MTR_MatriculaTurma_MatriculasBoletim_Alunos
				@tbMatriculaTurma = @tbMatriculaAlunos
		end
	END
	ELSE
	BEGIN
		--** ByAluno
		INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
			PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
			PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
				Select mb.alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mb.mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
			PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
			PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes 
			  from MTR_MatriculasBoletim mb  WITH(NOLOCK)
				   inner join @alunos amt on amt.alu_id = mb.alu_id and amt.mtu_id = mb.mtu_origemDados
	
			IF not exists (select top 1 alu_id from @MatriculasBoletimDaTurma)
			begin
				INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
					PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
					PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
				EXEC NEW_MTR_MatriculaTurma_MatriculasBoletim_Alunos
				@tbMatriculaTurma = @alunos
			end
	--**
	END

	INSERT INTO @Matriculas (alu_id, mtu_id, mtd_id, fav_id, tpc_id, tpc_ordem, tud_id, tur_id, registroExterno, PossuiSaidaPeriodo, esc_id)
	SELECT
		Mtr.alu_id, Mtr.mtu_id, Mtd.mtd_id, CASE WHEN @tur_tipo = 1 THEN Mtr.fav_id ELSE @fav_id END AS fav_id, Mtr.tpc_id, Mtr.tpc_ordem, Mtd.tud_id, Mtr.tur_id
		, Mtr.registroExterno, Mtr.PossuiSaidaPeriodo, Mtr.esc_id
	FROM @MatriculasBoletimDaTurma Mtr
	INNER JOIN MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
		ON Mtd.alu_id = Mtr.alu_id
		AND Mtd.mtu_id = Mtr.mtu_id
		AND Mtd.mtd_situacao <> 3
	INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDis WITH(NOLOCK)
		ON RelDis.tud_id = Mtd.tud_id
	INNER JOIN ACA_Disciplina Dis WITH(NOLOCK)
		ON RelDis.dis_id = Dis.dis_id	
	WHERE
		Mtr.mtu_id IS NOT NULL
		 --Busca a matrícula na mtd ligada à disciplina (pelo tds_id), em cada COC, da lista de mtus (MatriculasBoletim).
		AND Dis.tds_id = @tds_id
		 --Filtros de matrícula.
		AND Mtr.MesmoCalendario = 1
		AND Mtr.PeriodosEquivalentes = 1

	IF ( @tur_tipo IN (2,3,4) ) 
	BEGIN
		;WITH PresencaAlunoPeriodo AS
		(
			SELECT Mat.alu_id, Mat.mtu_id, Mat.mtd_id, Mat.tpc_id 
			FROM @Matriculas Mat
			INNER JOIN TUR_Turma Tur -- WITH (NOLOCK)
				ON Tur.tur_id = Mat.tur_id
			INNER JOIN MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
				ON Mtd.alu_id = Mat.alu_id
				AND Mtd.mtu_id = Mat.mtu_id
				AND Mtd.mtd_id = Mat.mtd_id
			INNER JOIN ACA_TipoPeriodoCalendario Tpc -- WITH (NOLOCK)
				ON Tpc.tpc_id = Mat.tpc_id
			INNER JOIN ACA_CalendarioPeriodo Cap -- WITH (NOLOCK)
				ON Cap.tpc_id = Tpc.tpc_id
				AND Cap.cal_id = Tur.cal_id
				AND Cap.cap_situacao <> 3
			WHERE
			(
				-- O aluno nao estava presente no periodo se:
				-- o aluno saiu durante o periodo
				Mtd.mtd_dataSaida BETWEEN Cap.cap_dataInicio AND Cap.cap_dataFim
				-- ou o aluno saiu antes de o periodo iniciar
				OR Mtd.mtd_dataSaida < Cap.cap_dataInicio
				-- ou o aluno entrou depois do fim do periodo
				OR Mtd.mtd_dataMatricula > Cap.cap_dataFim
			)
			AND Mat.PossuiSaidaPeriodo = 0
		)
		UPDATE @Matriculas
		SET PossuiSaidaPeriodo = 1
		FROM @Matriculas Mat
		INNER JOIN PresencaAlunoPeriodo Pap
			ON Pap.alu_id = Mat.alu_id
			AND Pap.mtu_id = Mat.mtu_id
			AND Pap.mtd_id = Mat.mtd_id
			AND Pap.tpc_id = Mat.tpc_id
	END

	-- Notas e frequencia que ja foram fechadas
	DECLARE @Fechado TABLE (alu_id BIGINT NOT NULL, mtu_id INT NOT NULL, mtd_id INT NOT NULL
							, atd_id INT NOT NULL, fav_id INT NOT NULL, ava_id INT NOT NULL
							, atd_avaliacao VARCHAR(20), atd_frequencia DECIMAL(5,2)
							, atd_relatorio VARCHAR(MAX), arq_idRelatorio BIGINT
							, atd_avaliacaoPosConselho VARCHAR(20), atd_frequenciaFinalAjustada DECIMAL(5,2)
							, tpc_id INT, atd_numeroAulas INT
			, PRIMARY KEY (alu_id, mtu_id, mtd_id, atd_id));	
	INSERT INTO @Fechado	
	SELECT 
		atd.alu_id
		, atd.mtu_id
		, atd.mtd_id
		, atd_id
		, atd.fav_id
		, atd.ava_id
		, atd_avaliacao
		, atd_frequencia
		, atd_relatorio
		, arq_idRelatorio
		, atd_avaliacaoPosConselho
		, atd_frequenciaFinalAjustada
		, ava.tpc_id
		, atd.atd_numeroAulas
	FROM 
		--** ByAluno
		@alunos AS alu
		INNER JOIN @Matriculas m
			ON m.alu_id = alu.alu_id
			AND m.mtu_id = alu.mtu_id
		INNER JOIN CLS_AlunoAvaliacaoTurmaDisciplina atd WITH(NOLOCK) 
			ON atd.tud_id = m.tud_id
			AND atd.alu_id = m.alu_id
			AND atd.mtu_id = m.mtu_id
			AND atd.mtd_id = m.mtd_id
			AND atd.fav_id = m.fav_id
		--**
		INNER JOIN ACA_Avaliacao ava WITH(NOLOCK) 
			ON ava.fav_id = atd.fav_id 
			AND ava.ava_id = atd.ava_id
			AND (ava.tpc_id = m.tpc_id OR ava.tpc_id IS NULL)
			AND ava.ava_situacao <> 3
	WHERE
		atd.tud_id = @tud_id
		AND (ava.ava_tipo IN (1, 5) -- periodica, periodica + final
			OR (ava.ava_tipo = 3 AND ava.ava_id = @ava_id)) --  final
		AND ava.fav_id = @fav_id
		AND atd_situacao <> 3
	------------------------------------------------------------
	-- Fechado em outras turmas
	UNION	
	SELECT 
		m.alu_id
		, m.mtu_id
		, m.mtd_id
		, atd_id
		, atd.fav_id
		, atd.ava_id
		, atd_avaliacao
		, atd_frequencia
		, atd_relatorio
		, arq_idRelatorio
		, atd_avaliacaoPosConselho
		, atd_frequenciaFinalAjustada
		, m.tpc_id
		, atd.atd_numeroAulas
	FROM @Matriculas m
		INNER JOIN CLS_AlunoAvaliacaoTurmaDisciplina atd WITH(NOLOCK) 
			ON atd.tud_id = m.tud_id
			AND atd.alu_id = m.alu_id
			AND atd.mtu_id = m.mtu_id
			AND atd.mtd_id = m.mtd_id
			AND atd.fav_id = m.fav_id
		INNER JOIN ACA_Avaliacao ava WITH(NOLOCK) 
			ON ava.fav_id = atd.fav_id 
			AND ava.ava_id = atd.ava_id
			AND ava.tpc_id = m.tpc_id
			AND ava.ava_situacao <> 3
		
	WHERE
		(m.tur_id <> @tur_id
			OR m.tud_id <> @tud_id)
		AND ava.ava_tipo IN (1, 5) -- periodica, periodica + final
		AND atd_situacao <> 3
	------------------------------------------------------------	
	
	--********************
	-- Se o ultimo periodo ainda nao foi fechado, 
	-- carregar os dados para salvar junto com o fechamento final.
	DECLARE @tbFrequenciaAlunos TABLE (
		alu_id BIGINT
		, mtu_id INT
		, mtd_id INT
		, Frequencia DECIMAL (27, 2) 
		, QtFaltasAluno INT
		, QtAulasAluno INT
		, FrequenciaAcumulada DECIMAL(5,2)
		, ausenciasCompensadas INT
		, FrequenciaFinalAjustada DECIMAL (27, 2) 
	)

	DECLARE @tbAlunosSemFechamentoUltimoPeriodo TABLE (
		alu_id BIGINT
		, mtu_id INT
		, mtd_id INT
		, PRIMARY KEY (alu_id, mtu_id, mtd_id)
	)
	INSERT INTO @tbAlunosSemFechamentoUltimoPeriodo (alu_id, mtu_id, mtd_id)
	SELECT m.alu_id, m.mtu_id, m.mtd_id 
	FROM @Matriculas m
	LEFT JOIN @Fechado f 
		ON f.alu_id = m.alu_id 
		AND f.mtu_id = m.mtu_id
		AND f.mtd_id = m.mtd_id
		AND f.tpc_id = m.tpc_id
	WHERE 
	m.tpc_id = @ultimoPeriodo
	AND f.alu_id IS NULL

	IF (EXISTS ( SELECT TOP 1 alu_id FROM @tbAlunosSemFechamentoUltimoPeriodo ))	
	BEGIN				

		DECLARE @tud_tipo TINYINT;
		SELECT
			@tud_tipo = tud_tipo
		FROM
			TUR_TurmaDisciplina WITH (NOLOCK)
		WHERE
			tud_id = @tud_id
			AND tud_situacao <> 3

		-- Armazena exibir compensacao ausencia cadastrada
		DECLARE @ExibeCompensacao BIT
		SELECT TOP 1
			@ExibeCompensacao = CASE WHEN (pac_valor = 'True') THEN 1 ELSE 0 END
		FROM
			ACA_ParametroAcademico WITH (NOLOCK)
		WHERE
			pac_chave = 'EXIBIR_COMPENSACAO_AUSENCIA_CADASTRADA'
	        
		DECLARE @AulasCompensadas TABLE (
			tud_id BIGINT NOT NULL,
			alu_id BIGINT NOT NULL,
			mtu_id INT NOT NULL,
			mtd_id INT NOT NULL,
			qtdCompensadas INT NULL,
			PRIMARY KEY (tud_id, alu_id, mtu_id, mtd_id)
		)
			
		DECLARE @TabelaQtdes TABLE (
			alu_id BIGINT NOT NULL,
			mtu_id INT NOT NULL,
			mtd_id INT NOT NULL,
			qtFaltas INT NULL,
			qtAulas INT NULL,
			qtFaltasReposicao INT NULL
			PRIMARY KEY (alu_id, mtu_id, mtd_id)
		)
		
		DECLARE @SomatorioAulasFaltas TABLE (alu_id BIGINT NOT NULL, aulas INT, faltas INT, faltasReposicao INT, compensadas INT);

		-- Compensacoes de ausencia do ultimo periodo
		INSERT INTO @AulasCompensadas(tud_id, alu_id, mtu_id, mtd_id, qtdCompensadas)	
			SELECT 
				caa.tud_id
				,caa.alu_id
				,caa.mtu_id
				,caa.mtd_id
				,SUM(ISNULL(cpa.cpa_quantidadeAulasCompensadas, 0)) as qtdCompensadas
			FROM CLS_CompensacaoAusencia cpa WITH (NOLOCK)
			INNER JOIN CLS_CompensacaoAusenciaAluno caa WITH (NOLOCK)
				ON  caa.tud_id = cpa.tud_id
				AND caa.cpa_id = cpa.cpa_id
				AND caa.caa_situacao = 1
			WHERE
				cpa.tud_id = @tud_id
				AND cpa.tpc_id = @ultimoPeriodo
				AND cpa.cpa_situacao = 1 
			GROUP BY
				caa.tud_id
				,caa.alu_id
				,caa.mtu_id
				,caa.mtd_id
		
		-- Faltas e aulas do ultimo periodo
		INSERT INTO @TabelaQtdes(alu_id, mtu_id, mtd_id, qtAulas, qtFaltas, qtFaltasReposicao)				
		SELECT 
			faltas.alu_id, 
			faltas.mtu_id, 
			faltas.mtd_id, 
			faltas.qtAulas, 
			faltas.qtFaltas,
			faltas.qtFaltasReposicao
		FROM 
			FN_Select_FaltasAulasBy_TurmaDisciplina(@tipoLancamento, @ultimoPeriodo, @tud_id, @fav_calculoQtdeAulasDadas, @tipoDocente) faltas

		-- So faz o calculo do somatorio, se tiver frequencia final ajustada
		--IF (@ExibeCompensacao = 1)
		--BEGIN
			IF (@tud_tipo = 15)
			BEGIN
				;WITH TabelaPeriodosAnteriores AS (
					SELECT 
						tpc.tpc_id, 
						ava.ava_id, 
						ava.fav_id 
					FROM dbo.ACA_TipoPeriodoCalendario AS tpc WITH (NOLOCK) 
					INNER JOIN dbo.ACA_Avaliacao AS ava WITH (NOLOCK)
						ON ava.fav_id = @fav_id
						AND tpc.tpc_id = ava.tpc_id	
						AND ava.ava_situacao <> 3	
					WHERE tpc_ordem <= (
						SELECT tpc_ordem FROM dbo.ACA_TipoPeriodoCalendario WITH (NOLOCK) 
						WHERE tpc_id = @ultimoPeriodo
					)
				)
				INSERT INTO @SomatorioAulasFaltas (alu_id, faltas, faltasReposicao, aulas, compensadas)
				SELECT 
					mat.alu_id,
					SUM(CASE WHEN @ultimoPeriodo = mat.tpc_id
							THEN ISNULL(tfa.qtfaltas,0)
							ELSE ISNULL(atd.atd_numeroFaltas,0)
					END) AS faltas,
					SUM(ISNULL(tfa.qtFaltasReposicao,0)) AS faltasReposicao,
					SUM(CASE WHEN @ultimoPeriodo = mat.tpc_id
							THEN ISNULL(tfa.qtAulas,0)
							ELSE  ISNULL(atd.atd_numeroAulas,0)
						END) AS aulas,
					SUM(CASE WHEN @ultimoPeriodo = mat.tpc_id 
							THEN ISNULL(cpa.qtdCompensadas,0)
							ELSE ISNULL(atd.atd_ausenciasCompensadas,0)
					END) AS compensadas
				FROM TUR_TurmaDisciplinaMultisseriada tdm WITH (NOLOCK)
				INNER JOIN @tbAlunosSemFechamentoUltimoPeriodo AS alu
					ON alu.alu_id = tdm.alu_id
				INNER JOIN @Matriculas AS mat
					ON mat.alu_id = alu.alu_id
				INNER JOIN TabelaPeriodosAnteriores tpa
					ON tpa.tpc_id = mat.tpc_id	
				LEFT JOIN CLS_AlunoAvaliacaoTurmaDisciplina Atd WITH (NOLOCK)
					ON  Atd.tud_id = mat.tud_id
					AND Atd.alu_id = mat.alu_id
					AND Atd.mtu_id = mat.mtu_id
					AND Atd.mtd_id = mat.mtd_id
					AND atd.fav_id = tpa.fav_id
					AND atd.ava_id = tpa.ava_id
					AND Atd.atd_situacao <> 3
				LEFT JOIN @TabelaQtdes tfa 
					ON  mat.alu_id = tfa.alu_id
					AND mat.tpc_id = @ultimoPeriodo
				LEFT JOIN @AulasCompensadas Cpa
					ON Cpa.alu_id = mat.alu_id
					AND Cpa.mtu_id = mat.mtu_id
					AND Cpa.mtd_id = mat.mtd_id
					AND mat.tpc_id = @ultimoPeriodo      
				GROUP BY mat.alu_id
			END
			ELSE 
			BEGIN
				;WITH TabelaPeriodosAnteriores AS (
					SELECT 
						tpc.tpc_id, 
						ava.ava_id, 
						ava.fav_id 
					FROM dbo.ACA_TipoPeriodoCalendario AS tpc WITH (NOLOCK) 
					INNER JOIN dbo.ACA_Avaliacao AS ava WITH (NOLOCK)
						ON ava.fav_id = @fav_id
						AND tpc.tpc_id = ava.tpc_id	
						AND ava.ava_situacao <> 3	
					WHERE tpc_ordem <= (
						SELECT tpc_ordem FROM dbo.ACA_TipoPeriodoCalendario WITH (NOLOCK) 
						WHERE tpc_id = @ultimoPeriodo
					)
				)
				INSERT INTO @SomatorioAulasFaltas (alu_id, faltas, faltasReposicao, aulas, compensadas)
				SELECT 
					mat.alu_id,
					SUM(CASE WHEN @ultimoPeriodo = mat.tpc_id
							THEN ISNULL(tfa.qtfaltas,0)
							ELSE ISNULL(atd.atd_numeroFaltas,0)
					END) AS faltas,
					SUM(ISNULL(tfa.qtFaltasReposicao,0)) AS faltasReposicao,
					SUM(CASE WHEN @ultimoPeriodo = mat.tpc_id
							THEN ISNULL(tfa.qtAulas,0)
							ELSE  ISNULL(atd.atd_numeroAulas,0)
						END) AS aulas,
					SUM(CASE WHEN @ultimoPeriodo = mat.tpc_id 
							THEN ISNULL(cpa.qtdCompensadas,0)
							ELSE ISNULL(atd.atd_ausenciasCompensadas,0)
					END) AS compensadas
				FROM @tbAlunosSemFechamentoUltimoPeriodo AS alu 
				INNER JOIN @Matriculas AS mat
					ON mat.alu_id = alu.alu_id
				INNER JOIN TabelaPeriodosAnteriores tpa
					ON tpa.tpc_id = mat.tpc_id	
				LEFT JOIN CLS_AlunoAvaliacaoTurmaDisciplina Atd WITH (NOLOCK)
					ON  Atd.tud_id = mat.tud_id
					AND Atd.alu_id = mat.alu_id
					AND Atd.mtu_id = mat.mtu_id
					AND Atd.mtd_id = mat.mtd_id
					AND atd.fav_id = tpa.fav_id
					AND atd.ava_id = tpa.ava_id
					AND Atd.atd_situacao <> 3
				LEFT JOIN @TabelaQtdes tfa 
					ON  mat.alu_id = tfa.alu_id
					AND mat.tpc_id = @ultimoPeriodo
				LEFT JOIN @AulasCompensadas Cpa
					ON Cpa.alu_id = mat.alu_id
					AND Cpa.mtu_id = mat.mtu_id
					AND Cpa.mtd_id = mat.mtd_id
					AND mat.tpc_id = @ultimoPeriodo  
				GROUP BY mat.alu_id
			END
		--END
		
		INSERT INTO @tbFrequenciaAlunos
		SELECT 
			alu.alu_id,
			alu.mtu_id,
			alu.mtd_id
			, ISNULL(dbo.FN_Calcula_PorcentagemFrequenciaVariacao(Qtd.qtAulas, Qtd.qtFaltas, FAV.fav_variacao), 0) AS Frequencia
			-- Qtde. de faltas
			, ISNULL(Qtd.qtFaltas, 0) AS QtFaltasAluno
			-- Qtde. de aulas
			, ISNULL(Qtd.qtAulas, 0) AS QtAulasAluno
			, CAST(/*ISNULL(TabelaFrequenciaFinal.frequenciaFinal, 0)*/ 0 AS DECIMAL(5,2)) AS FrequenciaAcumulada
			, ISNULL(ac.qtdCompensadas, 0) AS ausenciasCompensadas 
			, (CASE WHEN (@ExibeCompensacao = 1)
				THEN 
					dbo.FN_Calcula_PorcentagemFrequenciaVariacao(ISNULL(saf.aulas,0), ((ISNULL(saf.faltas,0) + ISNULL(saf.faltasReposicao,0)) - ISNULL(saf.compensadas,0)), FAV.fav_variacao)
				ELSE 
					dbo.FN_Calcula_PorcentagemFrequenciaVariacao(ISNULL(saf.aulas,0), ((ISNULL(saf.faltas,0) + ISNULL(saf.faltasReposicao,0))), FAV.fav_variacao)
			END) AS FrequenciaFinalAjustada
		FROM @tbAlunosSemFechamentoUltimoPeriodo AS alu
		INNER JOIN @TabelaQtdes AS Qtd
			ON alu.alu_id = Qtd.alu_id
			AND alu.mtu_id = Qtd.mtu_id
			AND alu.mtd_id = Qtd.mtd_id
		INNER JOIN ACA_FormatoAvaliacao FAV WITH (NOLOCK)
			ON FAV.fav_id = @fav_id
		LEFT JOIN @AulasCompensadas ac 
			ON ac.alu_id = alu.alu_id
			AND ac.mtu_id = alu.mtu_id
			AND ac.mtd_id = alu.mtd_id
		LEFT JOIN @SomatorioAulasFaltas saf		
			ON saf.alu_id = alu.alu_id			  
	END
	--********************

	/* [Carla 19/07/2013]
		Excluir os registros que não devem ser exibidos na tela. Esses registros devem ser trazidos para buscar
		a nota do bimestre anterior, caso seja recuperação.
	*/
	DELETE FROM @Matriculas
	WHERE
		registroExterno = 1
		-- Se possuir uma saída no período, não exibe o aluno.
		OR PossuiSaidaPeriodo = 1
		
	; WITH TabelaMovimentacao AS (
			--Selecina as movimentações que possuem matrícula anterior
			SELECT
				MOV.alu_id,
				mtu_idAnterior,
				tmv_nome    
			FROM
				--** ByAluno
				@alunos AS alu
				INNER JOIN MTR_Movimentacao MOV WITH (NOLOCK) 
					ON MOV.alu_id = alu.alu_id
				--**
				INNER JOIN ACA_TipoMovimentacao TMV WITH (NOLOCK) 
					ON MOV.tmv_idSaida = TMV.tmv_id
			WHERE
				mov_situacao NOT IN (3,4)
				AND tmv_situacao <> 3
				AND mtu_idAnterior IS NOT NULL	
	)
	, avaliacoes AS (
		SELECT 
			ava.tpc_id
			, ava.ava_nome
			, cap.cap_dataInicio AS cap_dataInicio
			, cap.cap_dataFim AS cap_dataFim
			, ava.ava_id
		FROM ACA_Avaliacao ava WITH(NOLOCK)
		LEFT JOIN ACA_CalendarioPeriodo cap WITH(NOLOCK) 
			ON cap.tpc_id = ava.tpc_id
			AND cap.cal_id = @cal_id
			AND cap.cap_situacao <> 3
		WHERE
			(ava.ava_tipo IN (1, 5) -- periodica, periodica + final
				OR (ava.ava_tipo = 3 AND ava.ava_id = @ava_id)) --  final
			AND ava.fav_id = @fav_id
			AND ava_situacao <> 3
	)
	, TabelaObservacaoConselho AS 
	(
		SELECT
			tur_id
			, alu_id
			, mtu_id
			, CASE WHEN qtdeQualidade = 0 AND qtdeDesempenhos = 0 AND qtdeRecomendacao = 0
						AND ato_qualidade IS NULL AND ato_desempenhoAprendizado IS NULL 
						AND ato_recomendacaoAluno IS NULL AND ato_recomendacaoResponsavel IS NULL
				   THEN 0
				   ELSE 1
			  END AS observacaoPreenchida
		FROM
		(
			SELECT
				Mtr.tur_id
				, Mtr.alu_id
				, Mtr.mtu_id
				, SUM(CASE WHEN aaq.tqa_id IS NULL THEN 0 ELSE 1 END) AS qtdeQualidade
  				, SUM(CASE WHEN aad.tda_id IS NULL THEN 0 ELSE 1 END) AS qtdeDesempenhos
  				, SUM(CASE WHEN aar.rar_id IS NULL THEN 0 ELSE 1 END) AS qtdeRecomendacao
				, ato_qualidade
				, ato_desempenhoAprendizado
				, ato_recomendacaoAluno
				, ato_recomendacaoResponsavel
			FROM
				@Matriculas Mtr
				INNER JOIN ACA_Avaliacao ava WITH(NOLOCK)
					ON Mtr.fav_id = ava.fav_id
					AND ava.tpc_id = @ultimoPeriodo
					AND ava.ava_exibeObservacaoConselhoPedagogico = 1
				LEFT JOIN CLS_AlunoAvaliacaoTurmaQualidade aaq WITH(NOLOCK)
					ON  Mtr.tur_id = aaq.tur_id
					AND Mtr.alu_id = aaq.alu_id
					AND Mtr.mtu_id = aaq.mtu_id
					AND aaq.fav_id = ava.fav_id
					AND aaq.ava_id = ava.ava_id
				LEFT JOIN CLS_AlunoAvaliacaoTurmaDesempenho aad WITH(NOLOCK)
					ON  Mtr.tur_id = aad.tur_id
					AND Mtr.alu_id = aad.alu_id
					AND Mtr.mtu_id = aad.mtu_id
					AND aad.fav_id = ava.fav_id
					AND aad.ava_id = ava.ava_id 
				LEFT JOIN CLS_AlunoAvaliacaoTurmaRecomendacao aar WITH(NOLOCK)
					ON  Mtr.tur_id = aar.tur_id
					AND Mtr.alu_id = aar.alu_id
					AND Mtr.mtu_id = aar.mtu_id
					AND aar.fav_id = ava.fav_id
					AND aar.ava_id = ava.ava_id	        
				LEFT JOIN CLS_ALunoAvaliacaoTurmaObservacao ato WITH(NOLOCK)
					ON Mtr.tur_id = ato.tur_id
					AND Mtr.alu_id = ato.alu_id
					AND Mtr.mtu_id = ato.mtu_id
					AND ato.fav_id = ava.fav_id
					AND ato.ava_id = ava.ava_id
					AND ato.ato_situacao <> 3		
			WHERE
				Mtr.tud_id = @tud_id
			GROUP BY
				Mtr.tur_id
				, Mtr.alu_id
				, Mtr.mtu_id
				, ato_qualidade
				, ato_desempenhoAprendizado
				, ato_recomendacaoAluno
				, ato_recomendacaoResponsavel
		) 
		AS tabela
		GROUP BY --(Adicionado group by por Webber) 
			tabela.tur_id
			, tabela.alu_id 
			, tabela.mtu_id 
			, CASE WHEN tabela.qtdeQualidade = 0 AND tabela.qtdeDesempenhos = 0 AND tabela.qtdeRecomendacao = 0
						AND tabela.ato_qualidade IS NULL AND tabela.ato_desempenhoAprendizado IS NULL 
						AND tabela.ato_recomendacaoAluno IS NULL AND tabela.ato_recomendacaoResponsavel IS NULL
				   THEN 0
				   ELSE 1
			  END		
	)
	, movimentacao AS (

			--Selecina as movimentações que possuem matrícula anterior
			SELECT
				MOV.alu_id,
				mtu_idAnterior,
				CASE WHEN tmo_tipoMovimento IN (6, 12, 14, 15, 16)
					 THEN TMV.tmv_nome + ' em ' + REPLACE(CONVERT(VARCHAR(10), mov.mov_dataRealizacao, 103), '/' + CAST(DATEPART(YEAR, mov.mov_dataRealizacao) AS VARCHAR(4)), '') +
					 	  ISNULL(' p/ ' + turD.tur_codigo, '')
					 WHEN tmo_tipoMovimento IN (8)
					 THEN TMV.tmv_nome + ' p/' + ISNULL(' ' + turD.tur_codigo, '')
					 WHEN tmo_tipoMovimento IN (11)
					 THEN TMV.tmv_nome + ' p/ ' + ISNULL(' ' + turD.tur_codigo, '')
					 ELSE TMV.tmv_nome
				END tmv_nome  
			FROM
				@Matriculas res
				INNER JOIN MTR_Movimentacao MOV WITH (NOLOCK) 
					ON res.alu_id = MOV.alu_id 
				INNER JOIN ACA_TipoMovimentacao TMV WITH (NOLOCK) 
					ON MOV.tmv_idSaida = TMV.tmv_id
				LEFT JOIN MTR_TipoMovimentacao tmo WITH(NOLOCK)
					ON mov.tmo_id = tmo.tmo_id
					AND tmo.tmo_situacao <> 3
				LEFT JOIN MTR_MatriculaTurma mtuD WITH(NOLOCK)
					ON mov.alu_id = mtuD.alu_id
					AND mov.mtu_idAtual = mtuD.mtu_id
				LEFT JOIN TUR_Turma turD WITH(NOLOCK)
					ON mtuD.tur_id = turD.tur_id
				LEFT JOIN ACA_CalendarioAnual calD WITH(NOLOCK)
					ON turD.cal_id = calD.cal_id
				INNER JOIN MTR_MatriculaTurma mtuO WITH(NOLOCK)
					ON mov.alu_id = mtuO.alu_id
					AND mov.mtu_idAnterior = mtuO.mtu_id
					AND mtuO.tur_id = @tur_id
				LEFT JOIN TUR_Turma turO WITH(NOLOCK)
					ON mtuO.tur_id = turO.tur_id
				LEFT JOIN ACA_CalendarioAnual calO WITH(NOLOCK)
					ON turO.cal_id = calO.cal_id
			WHERE
				mov_situacao NOT IN (3,4)
				AND tmv_situacao <> 3
				AND mtu_idAnterior IS NOT NULL	
	)
	, tbRetorno AS (	
		SELECT
			  Mtd.alu_id
			, Mtd.mtu_id
			, Mtd.mtd_id
			, alc.alc_matricula
			, F.atd_id AS AvaliacaoID
			, CASE WHEN atd_id IS NULL 
					THEN atm.atm_media
					ELSE ISNULL(F.atd_avaliacaoPosConselho, F.atd_avaliacao)
				END AS Avaliacao
			, CASE WHEN @permiteAlterarResultado = 0 
					THEN NULL
					-- Caso contrário, traz o resultado normalmente
					ELSE Mtd.mtd_resultado 
				END AS AvaliacaoResultado	
			, CASE WHEN F.atd_id IS NULL 
					THEN FM.Frequencia 
					ELSE F.atd_frequencia 
				END AS Frequencia
			, pes.pes_nome + 
				(
					CASE WHEN ( Mtd.mtd_situacao = 5 ) 
						THEN ISNULL((SELECT TOP 1 ' (' + tmv_nome + ')' FROM movimentacao MOV WITH(NOLOCK) 
										WHERE MOV.mtu_idAnterior = Mtd.mtu_id AND MOV.alu_id = Mtd.alu_id), ' (Inativo)')
						ELSE '' 
					END
				) 
				AS pes_nome
			, Pes.pes_dataNascimento
			, CASE WHEN Mtd.mtd_numeroChamada > 0 
					THEN CAST(Mtd.mtd_numeroChamada AS VARCHAR)
					ELSE '-' 
				END AS mtd_numeroChamada
			, Mtd.mtd_numeroChamada AS mtd_numeroChamadaordem
			, CAST(Mtd.alu_id AS VARCHAR) + ';' + 
				CAST(Mtd.mtd_id AS VARCHAR) + ';' + 
				CAST(Mtd.mtu_id AS VARCHAR) 
				AS id
			, Mtd.mtd_situacao AS situacaoMatriculaAluno
			, F.atd_relatorio
			, F.arq_idRelatorio
			, Mtd.mtd_dataMatricula AS dataMatricula
			, Mtd.mtd_dataSaida AS dataSaida
			, ISNULL(CASE WHEN F.atd_id IS NULL 
				THEN FM.FrequenciaFinalAjustada 
				ELSE F.atd_frequenciaFinalAjustada END, 0) AS FrequenciaFinalAjustada
			, ava.tpc_id
			, ava.ava_nome AS NomeAvaliacao
			, F.atd_avaliacaoPosConselho AS AvaliacaoPosConselho
			, ava.cap_dataInicio
			, CAST(ISNULL(toc.observacaoPreenchida, 0) AS BIT) AS observacaoConselhoPreenchida
			-- se o aluno nao teve a nota efetivada no periodo,
			-- mas ele estava presente no periodo
			-- deve-se informar o usuario.
			, CAST(CASE WHEN 
					(  
						(
							COALESCE(F.atd_avaliacaoPosConselho, F.atd_avaliacao, '') <> ''
							OR
							(
								-- se for o ultimo periodo,
								-- e nao tiver fechamento
								-- deve ter a nota do Listao
								ISNULL(ava.tpc_id, 0) = @ultimoPeriodo
								AND F.atd_id IS NULL
								AND ISNULL(atm.atm_media,'') <> ''
							)
						)
						AND ISNULL(tud.tud_naoLancarNota, 0) = 0
					)
					OR 
					(
						ISNULL(tud.tud_naoLancarNota, 0) = 1
						AND ISNULL(F.atd_id,0) > 0
					)
					THEN 1
					ELSE 0
			   END AS BIT) AS PossuiNota
			, ava.ava_id AS ava_id
			, CASE WHEN (ava.tpc_id IS NOT NULL AND ava.tpc_id = @ultimoPeriodo) 
					THEN 1 
					ELSE 0 
				END AS UltimoPeriodo			
			, FM.QtFaltasAluno
			, FM.QtAulasAluno
			, FM.ausenciasCompensadas
			, mtu.mtu_resultado
			, CASE WHEN (
					-- aluno estava presente na rede no periodo da avaliacao
					EXISTS (
						SELECT alu_id
						FROM @Matriculas
						WHERE alu_id = Mtr.alu_id
						AND tpc_id = ava.tpc_id
					)
				) THEN
					(
						CASE WHEN (
							-- aluno estava presente na escola no periodo da avaliacao
							EXISTS (
								SELECT alu_id
								FROM @Matriculas
								WHERE alu_id = Mtr.alu_id
								AND esc_id = @escolaId
								AND tpc_id = ava.tpc_id
							)
						) 
						THEN 1 -- Aluno presente na escola
						ELSE 2 -- Aluno em outra escola		
						END
					)						
				ELSE 0-- Aluno fora da rede
				END AS PresencaAluno
			, F.atd_numeroAulas AS QtAulasEfetivado
			, ISNULL(tpc.tpc_ordem, 0) AS tpc_ordem
		FROM @Matriculas Mtr
		INNER JOIN MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
			ON  Mtd.alu_id = Mtr.alu_id
			AND Mtd.mtu_id = Mtr.mtu_id
			AND Mtd.mtd_id = Mtr.mtd_id
			AND Mtd.tud_id = @tud_id
		INNER JOIN TUR_TurmaDisciplina tud WITH(NOLOCK)
			ON Mtd.tud_id = tud.tud_id
			AND tud.tud_situacao <> 3
		INNER JOIN MTR_MatriculaTurma mtu WITH(NOLOCK)
			ON mtu.alu_id = Mtd.alu_id
			AND mtu.mtu_id = Mtd.mtu_id
			AND mtu_situacao <> 3    
		INNER JOIN ACA_AlunoCurriculo alc WITH(NOLOCK)
			ON alc.alu_id = mtu.alu_id
			AND alc.alc_id = mtu.alc_id
			AND alc.alc_situacao <> 3	
		INNER JOIN ACA_Aluno Alu WITH(NOLOCK)
			ON  Mtd.alu_id   = Alu.alu_id
			AND alu_situacao <> 3
		INNER JOIN VW_DadosAlunoPessoa Pes 
			ON  Alu.alu_id   = Pes.alu_id
		INNER JOIN avaliacoes ava 
			ON 1 = 1        
		LEFT JOIN ACA_TipoPeriodoCalendario tpc WITH(NOLOCK)
			ON tpc.tpc_id = ava.tpc_id
			AND tpc.tpc_situacao <> 3
		LEFT JOIN @Fechado F 
			ON (F.alu_id = Mtd.alu_id 
					AND F.mtu_id = Mtd.mtu_id 
					AND F.mtd_id = Mtd.mtd_id
					AND ((F.tpc_id IS NULL AND ava.tpc_id IS NULL) OR F.tpc_id = ava.tpc_id)
				)
				------------------------------------------------------------
				-- Fechado em outras turmas
				OR (F.alu_id = Mtd.alu_id 
					AND F.mtu_id <> Mtd.mtu_id
					AND F.tpc_id = ava.tpc_id
				)	
				------------------------------------------------------------	  
		LEFT JOIN @tbFrequenciaAlunos FM
			ON FM.alu_id = Mtd.alu_id
			AND FM.mtu_id = Mtd.mtu_id
			AND FM.mtd_id = Mtd.mtd_id
			AND ava.tpc_id = @ultimoPeriodo			
		LEFT JOIN CLS_AlunoAvaliacaoTurmaDisciplinaMedia atm WITH (NOLOCK)
			ON atm.alu_id = Mtd.alu_id
			AND atm.mtu_id = Mtd.mtu_id
			AND atm.mtd_id = Mtd.mtd_id
			AND atm.tud_id = Mtd.tud_id
			AND atm.tpc_id = ava.tpc_id
			AND atm.tpc_id = @ultimoPeriodo	 
			AND atm.atm_situacao <> 3			
		LEFT JOIN TabelaObservacaoConselho toc
			ON 
			--toc.tur_id = Mtu.tur_id AND (Comentado aqui por Webber)
			toc.alu_id = Mtu.alu_id
			AND toc.mtu_id = Mtu.mtu_id	
	        
		WHERE 
			Mtr.tpc_id = @ultimoPeriodo
			AND mtd_situacao IN (1,5)
			AND ISNULL(mtd_numeroChamada, 0) >= 0		
	)
	, tbRetornoUltimoPeriodo AS
	(
		SELECT alu_id, mtu_id, mtd_id, FrequenciaFinalAjustada
		FROM tbRetorno
		WHERE UltimoPeriodo = 1
	)
	, tbFinal AS
	(
		SELECT 
			@tur_id AS tur_id
			, @tud_id AS tud_id
			, r.alu_id
			, r.mtu_id
			, r.mtd_id
			, alc_matricula
			, AvaliacaoID
			, Avaliacao
			, AvaliacaoResultado		
			, Frequencia
			, pes_nome		
			, pes_dataNascimento
			, mtd_numeroChamada
			, id
			, atd_relatorio
			, arq_idRelatorio
			, situacaoMatriculaAluno
			, dataMatricula
			, dataSaida
			-- Se for a avaliação final, pego a frequencia final ajustada do ultimo periodo
			, CASE WHEN (tpc_id IS NULL)
				THEN Up.FrequenciaFinalAjustada
				ELSE r.FrequenciaFinalAjustada
				END AS FrequenciaFinalAjustada
			, ISNULL(tpc_id, -1) AS tpc_id
			, NomeAvaliacao
			, AvaliacaoPosConselho
			, observacaoConselhoPreenchida
			-- Valida o fechamento apenas se o aluno estava 
			-- presente na escola no periodo da avaliação
			, CASE WHEN PresencaAluno = 1 THEN PossuiNota ELSE 1 END AS PossuiNota
			, ava_id
			, UltimoPeriodo
			, QtFaltasAluno
			, QtAulasAluno
			, ausenciasCompensadas
			, mtu_resultado
			, CASE WHEN PresencaAluno = 0 AND ISNULL(tpc_id, -1) > 0 THEN 1 ELSE 0 END AS AlunoForaDaRede
			, QtAulasEfetivado
			, cap_dataInicio
			, mtd_numeroChamadaordem
			, tpc_ordem
		FROM tbRetorno r
		LEFT JOIN tbRetornoUltimoPeriodo Up 
			ON Up.alu_id = r.alu_id 
			AND Up.mtu_id = r.mtu_id 
			AND Up.mtd_id = r.mtd_id	
		GROUP BY
			cap_dataInicio
			, tpc_id
			, NomeAvaliacao
			, r.alu_id
			, r.mtu_id
			, r.mtd_id
			, alc_matricula
			, AvaliacaoID
			, Avaliacao
			, AvaliacaoResultado		
			, Frequencia
			, pes_nome		
			, pes_dataNascimento
			, mtd_numeroChamada
			, mtd_numeroChamadaordem
			, id
			, atd_relatorio
			, arq_idRelatorio
			, situacaoMatriculaAluno
			, dataMatricula
			, dataSaida
			, r.FrequenciaFinalAjustada
			, Up.FrequenciaFinalAjustada
			, AvaliacaoPosConselho
			, observacaoConselhoPreenchida
			, ava_id
			, UltimoPeriodo
			, QtFaltasAluno
			, QtAulasAluno
			, ausenciasCompensadas
			, mtu_resultado
			, PossuiNota
			, PresencaAluno
			, QtAulasEfetivado
			, tpc_ordem
	)	
	SELECT 
		tur_id
		, tud_id
		, alu_id
		, mtu_id
		, mtd_id
		, alc_matricula
		, AvaliacaoID
		, Avaliacao
		, AvaliacaoResultado		
		, Frequencia
		, pes_nome		
		, ISNULL(CAST(pes_dataNascimento AS VARCHAR(10)),'') AS pes_dataNascimento
		, mtd_numeroChamada
		, id
		, atd_relatorio
		, arq_idRelatorio
		, situacaoMatriculaAluno
		, dataMatricula
		, dataSaida
		, FrequenciaFinalAjustada
		, tpc_id
		, NomeAvaliacao
		, AvaliacaoPosConselho
		, observacaoConselhoPreenchida
		, CASE WHEN AlunoForaDaRede = 1 OR PossuiNota = 1 THEN 0 ELSE 1 END AS SemNota
		, ava_id
		, UltimoPeriodo
		, QtFaltasAluno
		, QtAulasAluno
		, ausenciasCompensadas
		, mtu_resultado
		, AlunoForaDaRede
		, QtAulasEfetivado
		, tpc_ordem
	FROM
		tbFinal
	ORDER BY 
		cap_dataInicio
		, tpc_id
		, ava_id
		, CASE 
			WHEN @ordenacao = 0 THEN 
				CASE WHEN ISNULL(mtd_numeroChamadaordem,0) <= 0 THEN 1 ELSE 0 END
		END ASC
		, CASE WHEN @ordenacao = 0 THEN ISNULL(mtd_numeroChamadaordem,0) END ASC
		, CASE WHEN @ordenacao = 1 THEN pes_nome END ASC	
END



GO
PRINT N'Altering [dbo].[NEW_MTR_MatriculaTurmaDisciplina_SelectFechamento]'
GO
-- Stored Procedure

-- ========================================================================
-- Author:		Marcia Haga
-- Create date: 28/07/2015
-- Description: Retorna os alunos matriculados na Turma para o período informado,
--				de acordo com as regras necessárias para ele aparecer na listagem
--				para efetivar.

---- Alterado: Marcia Haga - 29/07/2015 
---- Description: Alterado para retornar o numero de aulas de reposicao e frequencia.

---- Alterado: Marcia Haga - 30/07/2015 
---- Description: Alterado para retornar se o registro do conselho foi parcialmente preenchido.

---- Alterado: Marcia Haga - 10/08/2015
---- Description: Alterado para verificar o periodo em que o aluno esteve 
---- presente na turma eletiva de aluno ou multisseriada.

---- Alterado: Marcia Haga - 11/08/2015
---- Description: Alterado para priorizar os dados pre-processados, ao inves dos dados ja efetivados.
-- ========================================================================
ALTER PROCEDURE [dbo].[NEW_MTR_MatriculaTurmaDisciplina_SelectFechamento]
	@tud_id BIGINT
	, @tur_id BIGINT 
	, @tpc_id INT
	, @ava_id INT
	, @ordenacao INT
	, @fav_id INT 
	, @tipoAvaliacao TINYINT
	, @permiteAlterarResultado BIT
	, @tur_tipo TINYINT
	, @cal_id INT 
	, @dtTurma TipoTabela_Turma READONLY
	, @documentoOficial BIT
AS
BEGIN

	SET TRANSACTION ISOLATION LEVEL SNAPSHOT

	DECLARE @MatriculasBoletimDaTurma AS TipoTabela_MatriculasBoletim;

	DECLARE @Matriculas TABLE (alu_id BIGINT NOT NULL, mtu_id INT NOT NULL, mtd_id INT NOT NULL, tur_id BIGINT, tpc_id INT, tpc_ordem INT, tud_id BIGINT, fav_id INT
	, registroExterno BIT, PossuiSaidaPeriodo BIT, variacaoFrequencia DECIMAL(18,3), mtd_numeroChamadaDocente INT NULL
	, mtd_situacaoDocente TINYINT NULL, mtd_dataMatriculaDocente DATE NULL, mtd_dataSaidaDocente DATE NULL
	, PRIMARY KEY (alu_id, mtu_id, mtd_id, tpc_id));

	DECLARE @MatriculaMultisseriadaTurmaAluno TABLE 
		(
			tud_idDocente BIGINT, 
			alu_id BIGINT, 
			mtu_id INT, 
			mtd_id INT,
			tud_idAluno BIGINT
			PRIMARY KEY (tud_idDocente, alu_id, mtu_id, mtd_id)
		);

	DECLARE @tds_id INT = 
		(
			 --Busca o tipo de disciplina para filtrar os mtds abaixo.
			SELECT Dis.tds_id
			FROM TUR_TurmaDisciplinaRelDisciplina RelDis -- WITH (NOLOCK)
			INNER JOIN ACA_Disciplina Dis -- WITH (NOLOCK)
				ON Dis.dis_id = RelDis.dis_id
			WHERE
				RelDis.tud_id = @tud_id
		)

	DECLARE @tud_tipo INT = 
		(
			 --Busca o tipo de disciplina para filtrar os mtds abaixo.
			SELECT TUD.tud_tipo
			FROM TUR_TurmaDisciplina AS TUD -- WITH (NOLOCK)
			WHERE
				TUD.tud_id = @tud_id
		)

	--Se for turma de eletiva do aluno, carrega os alunos que devem aparecer na tela de efetivação
	IF ( @tur_tipo IN (2,3) ) BEGIN
		-- Turma eletiva de aluno ou multisseriada, buscar matrículas por aluno.
		DECLARE @tbMatriculaAlunos TipoTabela_AlunoMatriculaTurma;
		INSERT INTO @tbMatriculaAlunos (alu_id, mtu_id)
		SELECT Mtd.alu_id, MAX(Mtd.mtu_id)
		FROM MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
		WHERE
			-- Busca mtus dos alunos pelo MTD - o mtd está ligado ao tud informado, e o mtu_id aponta pra outra turma (turma normal).
			Mtd.tud_id = @tud_id
			AND Mtd.mtd_situacao <> 3
		GROUP BY alu_id	

		INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
		Select mb.alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mb.mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes 
		  from MTR_MatriculasBoletim mb  -- WITH (NOLOCK)
			   inner join @tbMatriculaAlunos amt on amt.alu_id = mb.alu_id and amt.mtu_id = mb.mtu_origemDados

		IF not exists (select top 1 alu_id from @MatriculasBoletimDaTurma)
		begin
			INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
				PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
				PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
			EXEC NEW_MTR_MatriculaTurma_MatriculasBoletim_Alunos
				@tbMatriculaTurma = @tbMatriculaAlunos
		end
	END
	ELSE IF (@tur_tipo = 4)
	BEGIN
		-- Turma eletiva de aluno ou multisseriada, buscar matrículas por aluno.
		DECLARE @tbMatriculaAlunosMultisseriada TipoTabela_AlunoMatriculaTurma;
		INSERT INTO @tbMatriculaAlunosMultisseriada (alu_id, mtu_id)
		SELECT Mtd.alu_id, MAX(Mtd.mtu_id)
		FROM MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
		INNER JOIN MTR_MatriculaTurma mtu
			ON Mtd.alu_id = mtu.alu_id
			AND Mtd.mtu_id = mtu.mtu_id
			AND mtu.mtu_situacao <> 3
		INNER JOIN @dtTurma dtt
			ON mtu.tur_id = dtt.tur_id
		WHERE
			-- Busca mtus dos alunos pelo MTD - o mtd está ligado ao tud informado, e o mtu_id aponta pra outra turma (turma normal).
			Mtd.tud_id = @tud_id
			AND Mtd.mtd_situacao <> 3
		GROUP BY mtd.alu_id	

		INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
		Select mb.alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mb.mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes 
		  from MTR_MatriculasBoletim mb  -- WITH (NOLOCK)
			   inner join @tbMatriculaAlunosMultisseriada amt on amt.alu_id = mb.alu_id and amt.mtu_id = mb.mtu_origemDados

		IF not exists (select top 1 alu_id from @MatriculasBoletimDaTurma)
		begin
			INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
				PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
				PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
			EXEC NEW_MTR_MatriculaTurma_MatriculasBoletim_Alunos
				@tbMatriculaTurma = @tbMatriculaAlunosMultisseriada
		end

		INSERT INTO @MatriculaMultisseriadaTurmaAluno (tud_idDocente, alu_id, mtu_id, mtd_id, tud_idAluno)
		SELECT 
			mtdDocente.tud_id AS tud_idDocente,
			mtdAluno.alu_id,
			mtdAluno.mtu_id,
			mtdAluno.mtd_id,
			mtdAluno.tud_id AS tud_idAluno
		FROM @MatriculasBoletimDaTurma mtr
		INNER JOIN MTR_MatriculaTurmaDisciplina mtdDocente
			ON mtr.alu_id = mtdDocente.alu_id
			AND mtr.mtu_id = mtdDocente.mtu_id
			AND mtdDocente.tud_id = @tud_id
			AND mtdDocente.mtd_situacao <> 3
		INNER JOIN MTR_MatriculaTurmaDisciplina mtdAluno
			ON mtdAluno.alu_id = mtr.alu_id
			AND mtdAluno.mtu_id = mtr.mtu_id
			AND mtdAluno.mtd_situacao <> 3
		INNER JOIN TUR_TurmaDisciplina tudAluno
			ON mtdAluno.tud_id = tudAluno.tud_id
			AND tudAluno.tud_id <> @tud_id
			AND tudAluno.tud_tipo = 16
			AND tudAluno.tud_situacao <> 3
		INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDisAluno
			ON RelDisAluno.tud_id = tudAluno.tud_id
		INNER JOIN ACA_Disciplina disAluno
			ON RelDisAluno.dis_id = disAluno.dis_id
			AND disAluno.tds_id = @tds_id
			AND disAluno.dis_situacao <> 3
		GROUP BY
			mtdDocente.tud_id,
			mtdAluno.alu_id,
			mtdAluno.mtu_id,
			mtdAluno.mtd_id,
			mtdAluno.tud_id
	END
	 --Se for turma normal, carrega os alunos de acordo com o boletim
	ELSE
	BEGIN
		INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
			PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
			PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
		select mb.alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mb.mtu_id,mb.tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mb.mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes
		  from MTR_MatriculasBoletim mb
			   inner join (select alu_id, mtu_id, ROW_NUMBER() OVER(PARTITION BY alu_id 
														   ORDER BY mtu_id desc) as linha
							 from MTR_MatriculaTurma -- WITH (NOLOCK) 
							where mtu_situacao <> 3
							  and tur_id = @tur_id) mtu 
					   on mtu.alu_id = mb.alu_id and mtu.mtu_id = mb.mtu_origemDados
		 where mtu.linha = 1 --para evitar casos onde o aluno tem dois mtus para o mesmo tur_id

		IF not exists (select top 1 alu_id from @MatriculasBoletimDaTurma)
		begin
			INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
				PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
				PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
			EXEC NEW_MTR_MatriculaTurma_MatriculasBoletim_DaTurma
				@tur_id = @tur_id;
		end
	END
		

	IF (@tur_tipo = 4)
	BEGIN
		INSERT INTO @Matriculas 
		(
			alu_id, 
			mtu_id, 
			mtd_id, 
			fav_id, 
			tpc_id, 
			tpc_ordem, 
			tud_id, 
			tur_id, 
			registroExterno, 
			PossuiSaidaPeriodo, 
			variacaoFrequencia, 
			mtd_numeroChamadaDocente,
			mtd_situacaoDocente, 
			mtd_dataMatriculaDocente, 
			mtd_dataSaidaDocente
		)
		SELECT
			Mtr.alu_id
			, Mtr.mtu_id
			, Mtd.mtd_id
			, Mtr.fav_id
			, Mtr.tpc_id
			, Mtr.tpc_ordem
			, mtd.tud_id
			, Mtr.tur_id
			, Mtr.registroExterno
			, Mtr.PossuiSaidaPeriodo
			, FAV.fav_variacao
			, mtdDocente.mtd_numeroChamada
			, mtdDocente.mtd_situacao
			, mtdDocente.mtd_dataMatricula
			, mtdDocente.mtd_dataSaida
		FROM @MatriculasBoletimDaTurma Mtr
		INNER JOIN MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
			ON Mtd.alu_id = Mtr.alu_id
			AND Mtd.mtu_id = Mtr.mtu_id
			AND Mtd.mtd_situacao <> 3
		INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDis -- WITH (NOLOCK)
			ON RelDis.tud_id = Mtd.tud_id
		INNER JOIN ACA_Disciplina Dis -- WITH (NOLOCK)
			ON RelDis.dis_id = Dis.dis_id
		INNER JOIN dbo.ACA_FormatoAvaliacao FAV -- WITH (NOLOCK)
			ON	FAV.fav_id = Mtr.fav_id
			AND FAV.fav_situacao <> 3
		INNER JOIN @MatriculaMultisseriadaTurmaAluno tdm 
			ON Mtd.alu_id = tdm.alu_id
			AND Mtd.mtu_id = tdm.mtu_id
			AND Mtd.mtd_id = tdm.mtd_id
		INNER JOIN MTR_MatriculaTurmaDisciplina mtdDocente
			ON mtdDocente.alu_id = Mtd.alu_id
			AND mtdDocente.mtu_id = Mtd.mtu_id
			AND mtdDocente.tud_id = tdm.tud_idDocente
			AND mtdDocente.mtd_situacao <> 3
		WHERE
			Mtr.mtu_id IS NOT NULL
			 --Busca a matrícula na mtd ligada à disciplina (pelo tds_id), em cada COC, da lista de mtus (MatriculasBoletim).
			AND Dis.tds_id = @tds_id
			 --Filtros de matrícula.
			AND Mtr.MesmoCalendario = 1
			--AND Mtr.PeriodosEquivalentes = 1
		GROUP BY
			Mtr.alu_id
			, Mtr.mtu_id
			, Mtd.mtd_id
			, Mtr.fav_id
			, Mtr.tpc_id
			, Mtr.tpc_ordem
			, mtd.tud_id
			, Mtr.tur_id
			, Mtr.registroExterno
			, Mtr.PossuiSaidaPeriodo
			, FAV.fav_variacao
			, mtdDocente.mtd_numeroChamada
			, mtdDocente.mtd_situacao
			, mtdDocente.mtd_dataMatricula
			, mtdDocente.mtd_dataSaida
	END
	ELSE
	BEGIN
		INSERT INTO @Matriculas (alu_id, mtu_id, mtd_id, fav_id, tpc_id, tpc_ordem, tud_id, tur_id, registroExterno, PossuiSaidaPeriodo, variacaoFrequencia)
		SELECT
			Mtr.alu_id
			, Mtr.mtu_id
			, Mtd.mtd_id
			, Mtr.fav_id
			, Mtr.tpc_id
			, Mtr.tpc_ordem
			, Mtd.tud_id
			, Mtr.tur_id
			, Mtr.registroExterno
			, Mtr.PossuiSaidaPeriodo
			, FAV.fav_variacao
		FROM @MatriculasBoletimDaTurma Mtr
		INNER JOIN MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
			ON Mtd.alu_id = Mtr.alu_id
			AND Mtd.mtu_id = Mtr.mtu_id
			AND Mtd.mtd_situacao <> 3
		INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDis -- WITH (NOLOCK)
			ON RelDis.tud_id = Mtd.tud_id
		INNER JOIN ACA_Disciplina Dis -- WITH (NOLOCK)
			ON RelDis.dis_id = Dis.dis_id
		INNER JOIN dbo.ACA_FormatoAvaliacao FAV -- WITH (NOLOCK)
			ON	FAV.fav_id = Mtr.fav_id
			AND FAV.fav_situacao <> 3
		WHERE
			Mtr.mtu_id IS NOT NULL
			 --Busca a matrícula na mtd ligada à disciplina (pelo tds_id), em cada COC, da lista de mtus (MatriculasBoletim).
			AND Dis.tds_id = @tds_id
			 --Filtros de matrícula.
			AND Mtr.MesmoCalendario = 1
			--Verifica períodos equivalentes apenas para turmas normais (1)
			AND (Mtr.PeriodosEquivalentes = 1 OR @tur_tipo <> 1)
    END
		
	-- Verifica o periodo em que o aluno esteve presente na turma eletiva de aluno ou multisseriada
	IF ( @tur_tipo IN (2,3,4) ) 
	BEGIN
		;WITH PresencaAlunoPeriodo AS
		(
			SELECT Mat.alu_id, Mat.mtu_id, Mat.mtd_id, Mat.tpc_id 
			FROM @Matriculas Mat
			INNER JOIN TUR_Turma Tur -- WITH (NOLOCK)
				ON Tur.tur_id = Mat.tur_id
			INNER JOIN MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
				ON Mtd.alu_id = Mat.alu_id
				AND Mtd.mtu_id = Mat.mtu_id
				AND Mtd.mtd_id = Mat.mtd_id
			INNER JOIN ACA_TipoPeriodoCalendario Tpc -- WITH (NOLOCK)
				ON Tpc.tpc_id = Mat.tpc_id
			INNER JOIN ACA_CalendarioPeriodo Cap -- WITH (NOLOCK)
				ON Cap.tpc_id = Tpc.tpc_id
				AND Cap.cal_id = Tur.cal_id
				AND Cap.cap_situacao <> 3
			WHERE
			(
				-- O aluno nao estava presente no periodo se:
				-- o aluno saiu durante o periodo
				Mtd.mtd_dataSaida BETWEEN Cap.cap_dataInicio AND Cap.cap_dataFim
				-- ou o aluno saiu antes de o periodo iniciar
				OR Mtd.mtd_dataSaida < Cap.cap_dataInicio
				-- ou o aluno entrou depois do fim do periodo
				OR Mtd.mtd_dataMatricula > Cap.cap_dataFim
			)
			AND Mat.PossuiSaidaPeriodo = 0
		)
		UPDATE @Matriculas
		SET PossuiSaidaPeriodo = 1
		FROM @Matriculas Mat
		INNER JOIN PresencaAlunoPeriodo Pap
			ON Pap.alu_id = Mat.alu_id
			AND Pap.mtu_id = Mat.mtu_id
			AND Pap.mtd_id = Mat.mtd_id
			AND Pap.tpc_id = Mat.tpc_id
	END
		
	/* [Carla 19/07/2013]
		Excluir os registros que não devem ser exibidos na tela. Esses registros devem ser trazidos para buscar
		a nota do bimestre anterior, caso seja recuperação.
	*/
	DELETE FROM @Matriculas
	WHERE
		(registroExterno = 1)
		-- Se possuir uma saída no período, não exibe o aluno.
		OR (PossuiSaidaPeriodo = 1)
		-- Excluir matrículas de outras turmas, pois traz todos os bimestres pra fazer os acumulados.
		OR ((@tur_tipo = 1) AND (tur_id <> @tur_id))
		-- Excluir matrículas de outras disciplinas em turmas eletivas/multisseriadas.
		OR ((@tur_tipo IN (2,3)) AND tud_id <> @tud_id)
		-- Excluir matrículas de outras disciplinas do territorio do saber com o mesmo tds
		OR ((@tud_tipo = 18) AND (tud_id <> @tud_id))
			
	; WITH TabelaObservacaoConselho AS 
	(
		SELECT
			tur_id
			, alu_id
			, mtu_id
			, CASE WHEN qtdeQualidade = 0 AND qtdeDesempenhos = 0 AND qtdeRecomendacao = 0
						AND ato_qualidade IS NULL AND ato_desempenhoAprendizado IS NULL 
						AND ato_recomendacaoAluno IS NULL AND ato_recomendacaoResponsavel IS NULL
				   -- nenhum campo preenchido
				   THEN 0
				   ELSE
					(CASE WHEN ato_desempenhoAprendizado IS NOT NULL 
							AND ato_recomendacaoAluno IS NOT NULL 
							AND ato_recomendacaoResponsavel IS NOT NULL
					-- todos os campos preenchidos
					THEN 1
					-- algum campo preenchido
					ELSE 2
					END)
			  END AS observacaoPreenchida
		FROM
		(
			SELECT
				Mtr.tur_id
				, Mtr.alu_id
				, Mtr.mtu_id
				, SUM(CASE WHEN aaq.tqa_id IS NULL THEN 0 ELSE 1 END) AS qtdeQualidade
				, SUM(CASE WHEN aad.tda_id IS NULL THEN 0 ELSE 1 END) AS qtdeDesempenhos
				, SUM(CASE WHEN aar.rar_id IS NULL THEN 0 ELSE 1 END) AS qtdeRecomendacao
				, ato_qualidade
				, ato_desempenhoAprendizado
				, ato_recomendacaoAluno
				, ato_recomendacaoResponsavel
			FROM
				@Matriculas Mtr
				INNER JOIN ACA_Avaliacao ava -- WITH (NOLOCK)
					ON Mtr.fav_id = ava.fav_id
					AND ava.ava_id = @ava_id
					AND ava.ava_exibeObservacaoConselhoPedagogico = 1
				LEFT JOIN CLS_AlunoAvaliacaoTurmaQualidade aaq -- WITH (NOLOCK)
					ON  Mtr.tur_id = aaq.tur_id
					AND Mtr.alu_id = aaq.alu_id
					AND Mtr.mtu_id = aaq.mtu_id
					AND aaq.fav_id = ava.fav_id
					AND aaq.ava_id = ava.ava_id

				LEFT JOIN CLS_AlunoAvaliacaoTurmaDesempenho aad -- WITH (NOLOCK)
					ON  Mtr.tur_id = aad.tur_id
					AND Mtr.alu_id = aad.alu_id
					AND Mtr.mtu_id = aad.mtu_id
					AND aad.fav_id = ava.fav_id
					AND aad.ava_id = ava.ava_id 

				LEFT JOIN CLS_AlunoAvaliacaoTurmaRecomendacao aar -- WITH (NOLOCK)
					ON  Mtr.tur_id = aar.tur_id
					AND Mtr.alu_id = aar.alu_id
					AND Mtr.mtu_id = aar.mtu_id
					AND aar.fav_id = ava.fav_id
					AND aar.ava_id = ava.ava_id

				LEFT JOIN CLS_ALunoAvaliacaoTurmaObservacao ato -- WITH (NOLOCK)
					ON Mtr.tur_id = ato.tur_id
					AND Mtr.alu_id = ato.alu_id
					AND Mtr.mtu_id = ato.mtu_id
					AND ato.fav_id = ava.fav_id
					AND ato.ava_id = ava.ava_id
					AND ato.ato_situacao <> 3
			WHERE
				Mtr.tud_id = @tud_id
			GROUP BY
				Mtr.tur_id
				, Mtr.alu_id
				, Mtr.mtu_id
				, ato_qualidade
				, ato_desempenhoAprendizado
				, ato_recomendacaoAluno
				, ato_recomendacaoResponsavel
		) AS tabela
			GROUP BY --(Adicionado group by por Webber) 
				tabela.tur_id
				, tabela.alu_id 
				, tabela.mtu_id 
				, CASE WHEN qtdeQualidade = 0 AND qtdeDesempenhos = 0 AND qtdeRecomendacao = 0
							AND ato_qualidade IS NULL AND ato_desempenhoAprendizado IS NULL 
							AND ato_recomendacaoAluno IS NULL AND ato_recomendacaoResponsavel IS NULL
					   -- nenhum campo preenchido
					   THEN 0
					   ELSE
						(CASE WHEN ato_desempenhoAprendizado IS NOT NULL 
								AND ato_recomendacaoAluno IS NOT NULL 
								AND ato_recomendacaoResponsavel IS NOT NULL
						-- todos os campos preenchidos
						THEN 1
						-- algum campo preenchido
						ELSE 2
						END)
				  END
	),

	/*
	    12/06/2013 - Hélio C. Lima
	    Criado mais um "passo" CTE deixando as consultas as functions somente com o resultado a ser exibido
	*/
	tabResult AS (

        --	
	    SELECT	
		      Mtd.alu_id
		    , Mtd.mtu_id
		    , Mtd.mtd_id
		    , tur.tur_id
		    , tur.tur_codigo
		    , alc.alc_matricula
		    , Mtd.tud_id
		    , Atd.atd_id
		    , COALESCE(Caf.caf_avaliacao, Atd.atd_avaliacao, '') as Avaliacao 
		    , Mtd.mtd_resultado
			, COALESCE(Caf.caf_qtAulas, Atd.atd_numeroAulas, 0) as QtAulasAluno		
			, COALESCE(Caf.caf_qtAulasReposicao, Atd.atd_numeroAulasReposicao, 0) AS QtAulasAlunoReposicao
			, COALESCE(Caf.caf_qtFaltas, Atd.atd_numeroFaltas, 0) as QtFaltasAluno
			, COALESCE(Caf.caf_qtFaltasReposicao, Atd.atd_numeroFaltasReposicao, 0) AS QtFaltasAlunoReposicao			
			, CASE WHEN @documentoOficial = 1 THEN Pes.pes_nomeOficial ELSE Pes.pes_nome END AS pes_nome
            , ISNULL(Mtr.mtd_numeroChamadaDocente, Mtd.mtd_numeroChamada) AS mtd_numeroChamada
		    , ISNULL(Mtr.mtd_situacaoDocente, Mtd.mtd_situacao) AS mtd_situacao
		    , Atd.atd_relatorio
		    , Atd.arq_idRelatorio
			, Atd.atd_numeroAulas AS QtAulasEfetivado
            , ISNULL(Mtr.mtd_dataMatriculaDocente, Mtd.mtd_dataMatricula) AS mtd_dataMatricula
            , ISNULL(Mtr.mtd_dataSaidaDocente, Mtd.mtd_dataSaida) AS mtd_dataSaida
            , COALESCE(Caf.caf_qtAusenciasCompensadas, Atd.atd_ausenciasCompensadas, 0) AS ausenciasCompensadas
            , toc.observacaoPreenchida AS observacaoConselhoPreenchida
            , Atd.atd_avaliacaoPosConselho AS avaliacaoPosConselho
			, COALESCE(Caf.caf_frequencia, Atd.atd_frequencia, 0) AS Frequencia
			, COALESCE(Caf.caf_frequenciaFinalAjustada, Atd.atd_frequenciaFinalAjustada, 0) AS FrequenciaFinalAjustada			
			, mtu.mtu_resultado
	    FROM @Matriculas   Mtr
		INNER JOIN MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
			ON  Mtr.alu_id = Mtd.alu_id
            AND Mtr.mtu_id = Mtd.mtu_id
			AND Mtr.mtd_id = Mtd.mtd_id
        INNER JOIN MTR_MatriculaTurma mtu -- WITH (NOLOCK)
			ON  mtu.alu_id = Mtd.alu_id
			AND mtu.mtu_id = Mtd.mtu_id
			AND mtu_situacao <> 3
		INNER JOIN TUR_Turma tur -- WITH (NOLOCK)
			ON  tur.tur_id = mtu.tur_id
			AND tur.tur_situacao <> 3
		INNER JOIN ACA_AlunoCurriculo alc -- WITH (NOLOCK)
			ON  alc.alu_id = mtu.alu_id
			AND alc.alc_id = mtu.alc_id
			AND alc.alc_situacao <> 3			
        INNER JOIN ACA_Aluno Alu -- WITH (NOLOCK)
	        ON  Mtd.alu_id   = Alu.alu_id
	        AND alu_situacao <> 3
        INNER JOIN VW_DadosAlunoPessoa Pes -- WITH (NOLOCK)
	        ON  Alu.alu_id   = Pes.alu_id
		LEFT JOIN CLS_AlunoAvaliacaoTurmaDisciplina Atd -- WITH (NOLOCK)
	        ON  Atd.tud_id = Mtd.tud_id
	        AND Atd.alu_id = Mtd.alu_id
	        AND Atd.mtu_id = Mtd.mtu_id
	        AND Atd.mtd_id = Mtd.mtd_id
	        AND Atd.fav_id = @fav_id
	        AND Atd.ava_id = @ava_id
	        AND Atd.atd_situacao <> 3
		LEFT JOIN CLS_AlunoFechamento Caf -- WITH (NOLOCK)
			ON  Caf.tud_id = Mtd.tud_id
			AND Caf.tpc_id = @tpc_id
			AND Caf.alu_id = Mtd.alu_id
			AND Caf.mtu_id = Mtd.mtu_id
			AND Caf.mtd_id = Mtd.mtd_id			
		LEFT JOIN TabelaObservacaoConselho toc
			ON  toc.alu_id = Mtu.alu_id
			AND toc.mtu_id = Mtu.mtu_id
	    WHERE 
	        Mtr.tpc_id = @tpc_id
		    AND ISNULL(Mtr.mtd_situacaoDocente, mtd_situacao) IN (1,5)
		    AND COALESCE(Mtr.mtd_numeroChamadaDocente, mtd_numeroChamada, 0) >= 0
		    AND Alu.alu_situacao <> 3	
	)
	, TabelaMovimentacao AS (

			--Selecina as movimentações que possuem matrícula anterior
			SELECT
				MOV.alu_id,
				mtu_idAnterior,
				CASE WHEN tmo_tipoMovimento IN (6, 12, 14, 15, 16)
					 THEN TMV.tmv_nome + ' em ' + REPLACE(CONVERT(VARCHAR(10), mov.mov_dataRealizacao, 103), '/' + CAST(DATEPART(YEAR, mov.mov_dataRealizacao) AS VARCHAR(4)), '') +
					 	  ISNULL(' p/ ' + turD.tur_codigo, '')
					 WHEN tmo_tipoMovimento IN (8)
					 THEN TMV.tmv_nome + ' p/' + ISNULL(' ' + turD.tur_codigo, '')
					 WHEN tmo_tipoMovimento IN (11)
					 THEN TMV.tmv_nome + ' p/ ' + ISNULL(' ' + turD.tur_codigo, '')
					 ELSE TMV.tmv_nome
				END tmv_nome  
			FROM
				tabResult res
				INNER JOIN MTR_Movimentacao MOV -- WITH (NOLOCK) 
					ON res.alu_id = MOV.alu_id 
				INNER JOIN ACA_TipoMovimentacao TMV -- WITH (NOLOCK) 
					ON MOV.tmv_idSaida = TMV.tmv_id
				LEFT JOIN MTR_TipoMovimentacao tmo -- WITH(NOLOCK)
					ON mov.tmo_id = tmo.tmo_id
					AND tmo.tmo_situacao <> 3
				LEFT JOIN MTR_MatriculaTurma mtuD -- WITH(NOLOCK)
					ON mov.alu_id = mtuD.alu_id
					AND mov.mtu_idAtual = mtuD.mtu_id
				LEFT JOIN TUR_Turma turD -- WITH(NOLOCK)
					ON mtuD.tur_id = turD.tur_id
				LEFT JOIN ACA_CalendarioAnual calD -- WITH(NOLOCK)
					ON turD.cal_id = calD.cal_id
				INNER JOIN MTR_MatriculaTurma mtuO -- WITH(NOLOCK)
					ON mov.alu_id = mtuO.alu_id
					AND mov.mtu_idAnterior = mtuO.mtu_id
					AND mtuO.tur_id = @tur_id
				LEFT JOIN TUR_Turma turO -- WITH(NOLOCK)
					ON mtuO.tur_id = turO.tur_id
				LEFT JOIN ACA_CalendarioAnual calO -- WITH(NOLOCK)
					ON turO.cal_id = calO.cal_id
			WHERE
				mov_situacao NOT IN (3,4)
				AND tmv_situacao <> 3
				AND mtu_idAnterior IS NOT NULL	
	), 

	tbRetorno AS
	(
		SELECT
			  alu_id
			, mtu_id
			, mtd_id
			, tud_id
			, tur_id
			, tur_codigo
		    , alc_matricula
			, atd_id AS AvaliacaoID
			, Avaliacao
			, CASE 
				-- Caso não seja possível alterar o resultado do aluno e a avaliação for do tipo final ou períodica + final não traz o resultado (ele é calculado na tela)
				WHEN @permiteAlterarResultado = 0 AND @tipoAvaliacao IN (3,5) THEN 
					NULL
				-- Caso contrário, traz o resultado normalmente
				ELSE 
					mtd_resultado
			END 
			AS AvaliacaoResultado	
			, QtAulasAluno	
			, QtAulasAlunoReposicao		        			
			, QtFaltasAluno
			, QtFaltasAlunoReposicao				
			, Res.pes_nome + 
			(
				CASE 
					WHEN ( Res.mtd_situacao = 5 ) THEN 
						ISNULL((SELECT TOP 1 ' (' + tmv_nome + ')' FROM TabelaMovimentacao MOV -- WITH (NOLOCK)
						          WHERE MOV.mtu_idAnterior = Res.mtu_id AND MOV.alu_id = Res.alu_id), ' (Inativo)')
					ELSE 
						'' 
				END
			) 
			AS pes_nome					
			, CASE 
				WHEN Res.mtd_numeroChamada > 0 THEN 
					CAST(Res.mtd_numeroChamada AS VARCHAR)
				ELSE 
					'-' 
			END 
			AS mtd_numeroChamada			
			, Res.mtd_numeroChamada AS mtd_numeroChamadaordem
			, atd_relatorio
			, arq_idRelatorio 
			, Res.mtd_situacao AS situacaoMatriculaAluno
			, Res.mtd_dataMatricula AS dataMatricula
			, Res.mtd_dataSaida AS dataSaida
			, Res.ausenciasCompensadas		
			-- Verifica se há dispensa de disciplina para o aluno.
			, 0 AS dispensadisciplina
            , CAST(ISNULL(observacaoConselhoPreenchida, 0) AS TINYINT) AS observacaoConselhoPreenchida
            , avaliacaoPosConselho
			, Frequencia
			, FrequenciaFinalAjustada
			, mtu_resultado
			, QtAulasEfetivado			
		FROM 
			tabResult AS Res
	)   

	SELECT 
		  alu_id
			, mtu_id
			, mtd_id
			, tud_id
			, tur_id
			, tur_codigo
		    , alc_matricula
			, AvaliacaoID
			, Avaliacao
			, AvaliacaoResultado				
			, QtAulasAluno
			, QtAulasAlunoReposicao	
			, QtFaltasAluno
			, QtFaltasAlunoReposicao
			, pes_nome		
			, mtd_numeroChamada
			, atd_relatorio
			, arq_idRelatorio
			, situacaoMatriculaAluno
			, dataMatricula
			, dataSaida
			, ausenciasCompensadas
			, dispensadisciplina
			, observacaoConselhoPreenchida
			, avaliacaoPosConselho
			, Frequencia
			, FrequenciaFinalAjustada
			, mtu_resultado
			, QtAulasEfetivado
	FROM	
		tbRetorno 
	GROUP BY
		 alu_id
			, mtu_id
			, mtd_id
			, tud_id
			, tur_id
			, tur_codigo
		    , alc_matricula
			, AvaliacaoID
			, Avaliacao
			, AvaliacaoResultado
			, QtAulasAluno	
			, QtAulasAlunoReposicao		
			, QtFaltasAluno		
			, QtFaltasAlunoReposicao
			, pes_nome		
			, mtd_numeroChamada
			, mtd_numeroChamadaordem
			, atd_relatorio
			, arq_idRelatorio
			, situacaoMatriculaAluno
			, dataMatricula
			, dataSaida
			, ausenciasCompensadas
			, dispensadisciplina
			, observacaoConselhoPreenchida
			, avaliacaoPosConselho
			, Frequencia
			, FrequenciaFinalAjustada
			, mtu_resultado
			, QtAulasEfetivado
	ORDER BY 
		CASE 
		    WHEN @ordenacao = 0 THEN 
			    CASE WHEN ISNULL(mtd_numeroChamadaordem,0) <= 0 THEN 1 ELSE 0 END
		END ASC
		, CASE WHEN @ordenacao = 0 THEN ISNULL(mtd_numeroChamadaordem,0) END ASC
		, CASE WHEN @ordenacao = 1 THEN pes_nome END ASC
END


GO
PRINT N'Altering [dbo].[NEW_MTR_MatriculaTurmaDisciplina_SelectFechamento_Final]'
GO
-- Stored Procedure

-- ========================================================================
-- Author:	    Marcia Haga
-- Create date: 03/08/2015
-- Description: Retorna os alunos matriculados na Turma, de acordo com as regras necessárias 
--			    para ele aparecer na listagem para efetivar da avaliacao Final.

---- Alterado: Marcia Haga - 10/08/2015
---- Description: Alterado para verificar o periodo em que o aluno esteve 
---- presente na turma eletiva de aluno ou multisseriada.

---- Alterado: Marcia Haga - 11/08/2015
---- Description: Alterado para priorizar os dados pre-processados, ao inves dos dados ja efetivados.

-- Alterado: Jean Michel - 31/08/2016
-- Description: Alterado para não considerar lançamentos realizados em outras turmas/disciplinas,
--				quando for "Experiência" (Território do Saber)
-- ========================================================================
ALTER PROCEDURE [dbo].[NEW_MTR_MatriculaTurmaDisciplina_SelectFechamento_Final]
	@tud_id BIGINT
	, @tur_id BIGINT
	, @ava_id INT
	, @ordenacao INT
	, @fav_id INT
	, @tur_tipo TINYINT
	, @cal_id INT
	, @permiteAlterarResultado BIT
	, @dtTurma TipoTabela_Turma READONLY
	, @documentoOficial BIT
AS
BEGIN
    SET TRANSACTION ISOLATION LEVEL SNAPSHOT
	DECLARE @escolaId INT;
	SELECT TOP 1 @escolaId = esc_id
	FROM TUR_Turma -- WITH (NOLOCK)
	WHERE tur_id = @tur_id

	DECLARE @ultimoPeriodo INT;
	SELECT TOP 1 @ultimoPeriodo = tpc_id 
	FROM ACA_CalendarioPeriodo -- WITH (NOLOCK)
	WHERE 
		cal_id = @cal_id AND cap_situacao <> 3 
	ORDER BY cap_dataFim DESC

	DECLARE @MatriculasBoletimDaTurma AS TipoTabela_MatriculasBoletim;

	DECLARE @Matriculas TABLE (alu_id BIGINT NOT NULL, mtu_id INT NOT NULL, mtd_id INT NOT NULL, tur_id BIGINT, tpc_id INT, tpc_ordem INT, tud_id BIGINT, fav_id INT
		, registroExterno BIT, PossuiSaidaPeriodo BIT, esc_id INT, tds_id INT, mtd_numeroChamadaDocente INT NULL
		, mtd_situacaoDocente TINYINT NULL, mtd_dataMatriculaDocente DATE NULL, mtd_dataSaidaDocente DATE NULL, tud_tipo TINYINT NULL
		, PRIMARY KEY (alu_id, mtu_id, mtd_id, tpc_id));

	DECLARE @MatriculaMultisseriadaTurmaAluno TABLE 
		(
			tud_idDocente BIGINT, 
			alu_id BIGINT, 
			mtu_id INT, 
			mtd_id INT
			PRIMARY KEY (tud_idDocente, alu_id, mtu_id, mtd_id)
		);

	DECLARE @tds_id INT = 
		(
			 --Busca o tipo de disciplina para filtrar os mtds abaixo.
			SELECT Dis.tds_id
			FROM TUR_TurmaDisciplinaRelDisciplina RelDis -- WITH (NOLOCK)
			INNER JOIN ACA_Disciplina Dis -- WITH (NOLOCK)
				ON Dis.dis_id = RelDis.dis_id
			WHERE
				RelDis.tud_id = @tud_id
		)

	--Se for turma de eletiva do aluno, carrega os alunos que devem aparecer na tela de efetivação
	IF ( @tur_tipo IN (2,3) ) BEGIN
		-- Turma eletiva de aluno ou multisseriada, buscar matrículas por aluno.
		DECLARE @tbMatriculaAlunos TipoTabela_AlunoMatriculaTurma;
		INSERT INTO @tbMatriculaAlunos (alu_id, mtu_id)
		SELECT Mtd.alu_id, MAX(Mtd.mtu_id)
		FROM MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
		WHERE
			-- Busca mtus dos alunos pelo MTD - o mtd está ligado ao tud informado, e o mtu_id aponta pra outra turma (turma normal).
			Mtd.tud_id = @tud_id
			AND Mtd.mtd_situacao <> 3
		GROUP BY alu_id	

		INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
			PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
			PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
		Select mb.alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mb.mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes 
		  from MTR_MatriculasBoletim mb  -- WITH (NOLOCK)
			   inner join @tbMatriculaAlunos amt on amt.alu_id = mb.alu_id and amt.mtu_id = mb.mtu_origemDados

		IF not exists (select top 1 alu_id from @MatriculasBoletimDaTurma)
		begin
			INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
				PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
				PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
			EXEC NEW_MTR_MatriculaTurma_MatriculasBoletim_Alunos
				@tbMatriculaTurma = @tbMatriculaAlunos
		end
	END
	ELSE IF (@tur_tipo = 4)
	BEGIN
		-- Turma eletiva de aluno ou multisseriada, buscar matrículas por aluno.
		DECLARE @tbMatriculaAlunosMultisseriada TipoTabela_AlunoMatriculaTurma;
		INSERT INTO @tbMatriculaAlunosMultisseriada (alu_id, mtu_id)
		SELECT Mtd.alu_id, MAX(Mtd.mtu_id)
		FROM MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
		INNER JOIN MTR_MatriculaTurma mtu
			ON Mtd.alu_id = mtu.alu_id
			AND Mtd.mtu_id = mtu.mtu_id
			AND mtu.mtu_situacao <> 3
		INNER JOIN @dtTurma dtt
			ON mtu.tur_id = dtt.tur_id
		WHERE
			-- Busca mtus dos alunos pelo MTD - o mtd está ligado ao tud informado, e o mtu_id aponta pra outra turma (turma normal).
			Mtd.tud_id = @tud_id
			AND Mtd.mtd_situacao <> 3
		GROUP BY mtd.alu_id	

		INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
		Select mb.alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mb.mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes 
		  from MTR_MatriculasBoletim mb  -- WITH (NOLOCK)
			   inner join @tbMatriculaAlunosMultisseriada amt on amt.alu_id = mb.alu_id and amt.mtu_id = mb.mtu_origemDados

		IF not exists (select top 1 alu_id from @MatriculasBoletimDaTurma)
		begin
			INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
				PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
				PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
			EXEC NEW_MTR_MatriculaTurma_MatriculasBoletim_Alunos
				@tbMatriculaTurma = @tbMatriculaAlunosMultisseriada
		end

		INSERT INTO @MatriculaMultisseriadaTurmaAluno (tud_idDocente, alu_id, mtu_id, mtd_id)
		SELECT 
			mtdDocente.tud_id AS tud_idDocente,
			mtdAluno.alu_id,
			mtdAluno.mtu_id,
			mtdAluno.mtd_id
		FROM @MatriculasBoletimDaTurma mtr
		INNER JOIN MTR_MatriculaTurmaDisciplina mtdDocente
			ON mtr.alu_id = mtdDocente.alu_id
			AND mtr.mtu_id = mtdDocente.mtu_id
			AND mtdDocente.tud_id = @tud_id
			AND mtdDocente.mtd_situacao <> 3
		INNER JOIN MTR_MatriculaTurmaDisciplina mtdAluno
			ON mtdAluno.alu_id = mtr.alu_id
			AND mtdAluno.mtu_id = mtr.mtu_id
			AND mtdAluno.mtd_situacao <> 3
		INNER JOIN TUR_TurmaDisciplina tudAluno
			ON mtdAluno.tud_id = tudAluno.tud_id
			AND tudAluno.tud_id <> @tud_id
			AND tudAluno.tud_situacao <> 3
		INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDisAluno
			ON RelDisAluno.tud_id = tudAluno.tud_id
		INNER JOIN ACA_Disciplina disAluno
			ON RelDisAluno.dis_id = disAluno.dis_id
			AND disAluno.tds_id = @tds_id
			AND disAluno.dis_situacao <> 3
		GROUP BY
			mtdDocente.tud_id,
			mtdAluno.alu_id,
			mtdAluno.mtu_id,
			mtdAluno.mtd_id
	END
	 --Se for turma normal, carrega os alunos de acordo com o boletim
	ELSE
	BEGIN
		INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
			PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
			PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
		select mb.alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mb.mtu_id,mb.tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mb.mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes
		  from MTR_MatriculasBoletim mb -- WITH (NOLOCK)
			   inner join (select alu_id, mtu_id, ROW_NUMBER() OVER(PARTITION BY alu_id 
														   ORDER BY mtu_id desc) as linha
							 from MTR_MatriculaTurma -- WITH (NOLOCK) 
							where mtu_situacao <> 3
							  and tur_id = @tur_id) mtu 
					   on mtu.alu_id = mb.alu_id and mtu.mtu_id = mb.mtu_origemDados
		 where mtu.linha = 1 --para evitar casos onde o aluno tem dois mtus para o mesmo tur_id

		IF not exists (select top 1 alu_id from @MatriculasBoletimDaTurma)
		begin
		INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
			PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
			PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
		EXEC NEW_MTR_MatriculaTurma_MatriculasBoletim_DaTurma
			@tur_id = @tur_id;
		end
	END	

	IF (@tur_tipo = 4)
	BEGIN
		INSERT INTO @Matriculas (alu_id, mtu_id, mtd_id, fav_id, tpc_id, tpc_ordem, tud_id, tur_id, registroExterno, PossuiSaidaPeriodo, esc_id, tds_id,
								 mtd_numeroChamadaDocente, mtd_situacaoDocente, mtd_dataMatriculaDocente, mtd_dataSaidaDocente)
		SELECT
			Mtr.alu_id, Mtr.mtu_id, Mtd.mtd_id, Mtr.fav_id, Mtr.tpc_id, Mtr.tpc_ordem, Mtd.tud_id, Mtr.tur_id
			, Mtr.registroExterno, Mtr.PossuiSaidaPeriodo, Mtr.esc_id, Dis.tds_id 
			, mtdDocente.mtd_numeroChamada
			, mtdDocente.mtd_situacao
			, mtdDocente.mtd_dataMatricula
			, mtdDocente.mtd_dataSaida
		FROM @MatriculasBoletimDaTurma Mtr
		INNER JOIN MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
			ON Mtd.alu_id = Mtr.alu_id
			AND Mtd.mtu_id = Mtr.mtu_id
			AND Mtd.mtd_situacao <> 3
		INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDis -- WITH (NOLOCK)
			ON RelDis.tud_id = Mtd.tud_id
		INNER JOIN ACA_Disciplina Dis -- WITH (NOLOCK)
			ON RelDis.dis_id = Dis.dis_id	
		INNER JOIN @MatriculaMultisseriadaTurmaAluno tdm 
			ON Mtd.alu_id = tdm.alu_id
			AND Mtd.mtu_id = tdm.mtu_id
			AND Mtd.mtd_id = tdm.mtd_id
		INNER JOIN MTR_MatriculaTurmaDisciplina mtdDocente
			ON mtdDocente.alu_id = Mtd.alu_id
			AND mtdDocente.mtu_id = Mtd.mtu_id
			AND mtdDocente.tud_id = tdm.tud_idDocente
			AND mtdDocente.mtd_situacao <> 3
		WHERE
			Mtr.mtu_id IS NOT NULL
			 --Busca a matrícula na mtd ligada à disciplina (pelo tds_id), em cada COC, da lista de mtus (MatriculasBoletim).
			AND Dis.tds_id = @tds_id
			 --Filtros de matrícula.
			AND Mtr.MesmoCalendario = 1
			--AND Mtr.PeriodosEquivalentes = 1
	END
	ELSE
	BEGIN
		INSERT INTO @Matriculas (alu_id, mtu_id, mtd_id, fav_id, tpc_id, tpc_ordem, tud_id, tur_id, registroExterno, PossuiSaidaPeriodo, esc_id, tds_id, tud_tipo)
		SELECT
			Mtr.alu_id, Mtr.mtu_id, Mtd.mtd_id, tur.fav_id, Mtr.tpc_id, Mtr.tpc_ordem, Mtd.tud_id, Mtr.tur_id
			, Mtr.registroExterno, Mtr.PossuiSaidaPeriodo, Mtr.esc_id, Dis.tds_id, tud.tud_tipo
		FROM @MatriculasBoletimDaTurma Mtr
		INNER JOIN MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
			ON Mtd.alu_id = Mtr.alu_id
			AND Mtd.mtu_id = Mtr.mtu_id
			AND Mtd.mtd_situacao <> 3
		INNER JOIN TUR_TurmaDisciplina tud -- WITH (NOLOCK)
			ON tud.tud_id = Mtd.tud_id			 
		INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDis -- WITH (NOLOCK)
			ON RelDis.tud_id = Mtd.tud_id
		INNER JOIN TUR_TurmaRelTurmaDisciplina RelTur 
			ON RelTur.tud_id = Mtd.tud_id
		INNER JOIN TUR_Turma tur
			ON tur.tur_id = RelTur.tur_id
			AND tur.tur_situacao <> 3
		INNER JOIN ACA_Disciplina Dis -- WITH (NOLOCK)
			ON RelDis.dis_id = Dis.dis_id	
		WHERE
			Mtr.mtu_id IS NOT NULL
			 --Busca a matrícula na mtd ligada à disciplina (pelo tds_id), em cada COC, da lista de mtus (MatriculasBoletim).
			AND Dis.tds_id = @tds_id
			 --Filtros de matrícula.
			AND Mtr.MesmoCalendario = 1
			--Verifica períodos equivalentes apenas para turmas normais (1)
			AND (Mtr.PeriodosEquivalentes = 1 OR @tur_tipo <> 1)
	END

	-- Verifica o periodo em que o aluno esteve presente na turma eletiva de aluno ou multisseriada
	IF ( @tur_tipo IN (2,3,4) ) 
	BEGIN
		;WITH PresencaAlunoPeriodo AS
		(
			SELECT Mat.alu_id, Mat.mtu_id, Mat.mtd_id, Mat.tpc_id 
			FROM @Matriculas Mat
			INNER JOIN TUR_Turma Tur -- WITH (NOLOCK)
				ON Tur.tur_id = Mat.tur_id
			INNER JOIN MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
				ON Mtd.alu_id = Mat.alu_id
				AND Mtd.mtu_id = Mat.mtu_id
				AND Mtd.mtd_id = Mat.mtd_id
				AND Mtd.mtd_situacao <> 3
			INNER JOIN ACA_TipoPeriodoCalendario Tpc -- WITH (NOLOCK)
				ON Tpc.tpc_id = Mat.tpc_id
			INNER JOIN ACA_CalendarioPeriodo Cap -- WITH (NOLOCK)
				ON Cap.tpc_id = Tpc.tpc_id
				AND Cap.cal_id = Tur.cal_id
				AND Cap.cap_situacao <> 3
			WHERE
			(
				-- O aluno nao estava presente no periodo se:
				-- o aluno saiu durante o periodo
				Mtd.mtd_dataSaida BETWEEN Cap.cap_dataInicio AND Cap.cap_dataFim
				-- ou o aluno saiu antes de o periodo iniciar
				OR Mtd.mtd_dataSaida < Cap.cap_dataInicio
				-- ou o aluno entrou depois do fim do periodo
				OR Mtd.mtd_dataMatricula > Cap.cap_dataFim
			)
			AND Mat.PossuiSaidaPeriodo = 0
		)
	--UPDATE @Matriculas
		--SET PossuiSaidaPeriodo = 1
		--FROM @Matriculas Mat
		--INNER JOIN PresencaAlunoPeriodo Pap
		--	ON Pap.alu_id = Mat.alu_id
		--	AND Pap.mtu_id = Mat.mtu_id
		--	AND Pap.mtd_id = Mat.mtd_id
		--	AND Pap.tpc_id = Mat.tpc_id

		/* [Carla 14/12/2016] Excluir as matrículas que não são dessa turma antes de buscar as notas, 
			para as turmas de RP, pois causa duplicidade quando há 2 mtds em RP no mesmo bimestre.
			[Ticket #5743]
		 */
		DELETE FROM @Matriculas
		FROM @Matriculas Mat
		INNER JOIN PresencaAlunoPeriodo Pap
					ON Pap.alu_id = Mat.alu_id
					AND Pap.mtu_id = Mat.mtu_id
					AND Pap.mtd_id = Mat.mtd_id
					AND Pap.tpc_id = Mat.tpc_id
	END

	-- Notas e frequencia que ja foram fechadas
	DECLARE @Fechado TABLE (alu_id BIGINT NOT NULL, mtu_id INT NOT NULL, mtd_id INT NOT NULL
							, atd_id INT NOT NULL, fav_id INT NOT NULL, ava_id INT NOT NULL
							, atd_avaliacao VARCHAR(20), atd_frequencia DECIMAL(5,2)
							, atd_relatorio VARCHAR(MAX), arq_idRelatorio BIGINT
							, atd_avaliacaoPosConselho VARCHAR(20), atd_frequenciaFinalAjustada DECIMAL(5,2)
							, tpc_id INT, atd_numeroAulas INT, atd_numeroAulasReposicao INT
							, atd_numeroFaltas INT, atd_numeroFaltasReposicao INT
							, atd_ausenciasCompensadas INT
			, PRIMARY KEY (alu_id, mtu_id, mtd_id, atd_id));	
	INSERT INTO @Fechado
	SELECT 
		atd.alu_id
		, atd.mtu_id
		, atd.mtd_id
		, atd_id
		, atd.fav_id
		, atd.ava_id
		, atd_avaliacao
		, atd_frequencia
		, atd_relatorio
		, arq_idRelatorio
		, atd_avaliacaoPosConselho
		, atd_frequenciaFinalAjustada
		, ava.tpc_id
		, atd.atd_numeroAulas
		, atd.atd_numeroAulasReposicao
		, atd.atd_numeroFaltas
		, atd.atd_numeroFaltasReposicao
		, atd.atd_ausenciasCompensadas
	FROM @Matriculas m
		INNER JOIN CLS_AlunoAvaliacaoTurmaDisciplina atd -- WITH (NOLOCK) 
			ON atd.tud_id = m.tud_id
			AND atd.alu_id = m.alu_id
			AND atd.mtu_id = m.mtu_id
			AND atd.mtd_id = m.mtd_id
			AND atd.fav_id = m.fav_id
		INNER JOIN ACA_Avaliacao ava -- WITH (NOLOCK) 
			ON ava.fav_id = atd.fav_id 
			AND ava.ava_id = atd.ava_id
			AND (ava.tpc_id = m.tpc_id OR ava.tpc_id IS NULL)
			AND ava.ava_situacao <> 3
	WHERE
		atd.tud_id = @tud_id
		AND (ava.ava_tipo IN (1, 5) -- periodica, periodica + final
			OR (ava.ava_tipo = 3 AND ava.ava_id = @ava_id)) --  final
		AND ava.fav_id = @fav_id
		AND atd_situacao <> 3
	------------------------------------------------------------
	-- Fechado em outras turmas
	UNION	
	SELECT 
		m.alu_id
		, m.mtu_id
		, m.mtd_id
		, atd_id
		, atd.fav_id
		, atd.ava_id
		, atd_avaliacao
		, atd_frequencia
		, atd_relatorio
		, arq_idRelatorio
		, atd_avaliacaoPosConselho
		, atd_frequenciaFinalAjustada
		, m.tpc_id
		, atd.atd_numeroAulas
		, atd.atd_numeroAulasReposicao
		, atd.atd_numeroFaltas
		, atd.atd_numeroFaltasReposicao
		, atd.atd_ausenciasCompensadas
	FROM @Matriculas m
		INNER JOIN CLS_AlunoAvaliacaoTurmaDisciplina atd -- WITH (NOLOCK) 
			ON atd.tud_id = m.tud_id
			AND atd.alu_id = m.alu_id
			AND atd.mtu_id = m.mtu_id
			AND atd.mtd_id = m.mtd_id
			AND atd.fav_id = m.fav_id
		INNER JOIN ACA_Avaliacao ava -- WITH (NOLOCK) 
			ON ava.fav_id = atd.fav_id 
			AND ava.ava_id = atd.ava_id
			AND ava.tpc_id = m.tpc_id
			AND ava.ava_situacao <> 3
	WHERE
		(m.tur_id <> @tur_id OR m.tud_id <> @tud_id)
		-- Quando for "Experiência" (Território do saber), não considera fechamento realizado em outras turmas/disciplinas
		AND ISNULL(m.tud_tipo,0) <> 18
		AND ava.ava_tipo IN (1, 5) -- periodica, periodica + final
		AND atd_situacao <> 3
	------------------------------------------------------------	

	/* [Carla 19/07/2013]
		Excluir os registros que não devem ser exibidos na tela. Esses registros devem ser trazidos para buscar
		a nota do bimestre anterior, caso seja recuperação.
	*/
	DELETE FROM @Matriculas
	WHERE
		registroExterno = 1
		-- Se possuir uma saída no período, não exibe o aluno.
		OR PossuiSaidaPeriodo = 1

	; WITH avaliacoes AS (
		SELECT 
			ava.tpc_id
			, ava.ava_nome
			, cap.cap_dataInicio AS cap_dataInicio
			, cap.cap_dataFim AS cap_dataFim
			, ava.ava_id
		FROM ACA_Avaliacao ava -- WITH (NOLOCK)
		LEFT JOIN ACA_CalendarioPeriodo cap -- WITH (NOLOCK) 
			ON cap.tpc_id = ava.tpc_id
			AND cap.cal_id = @cal_id
			AND cap.cap_situacao <> 3
		WHERE
			(ava.ava_tipo IN (1, 5) -- periodica, periodica + final
				OR (ava.ava_tipo = 3 AND ava.ava_id = @ava_id)) --  final
			AND ava.fav_id = @fav_id
			AND ava_situacao <> 3
	)
	,alunoBimestre AS
	(
		SELECT
			mtr.alu_id,
			ava.tpc_id,
			ava.ava_nome,
			ava.cap_dataInicio,
			ava.cap_dataFim,
			ava.ava_id,
			CASE WHEN (
					-- aluno estava presente na rede no periodo da avaliacao
					NOT EXISTS (
						SELECT alu_id
						FROM @Matriculas
						WHERE alu_id = Mtr.alu_id
						AND tpc_id = ava.tpc_id 
					)
				)
				THEN 1
				ELSE 0
			END AS ForaRede,
			CASE WHEN (
					-- aluno estava presente na disciplina no periodo da avaliacao
					EXISTS (
						SELECT alu_id
						FROM @Matriculas
						WHERE alu_id = Mtr.alu_id
						AND tud_id = @tud_id
						AND tpc_id = ava.tpc_id
					)
				) 
				THEN 1
				ELSE 0
			END AS PresencaDisciplina,
			CASE WHEN (
					-- aluno estava presente na disciplina no periodo da avaliacao
					EXISTS (
						SELECT alu_id
						FROM @Matriculas
						WHERE alu_id = Mtr.alu_id
						AND esc_id = @escolaId
						AND tpc_id = ava.tpc_id
					)
				) 
				THEN 1
				ELSE 0
			END AS PresencaEscola
		FROM
			@Matriculas mtr
			INNER JOIN avaliacoes ava
				ON 1 = 1
		WHERE
			mtr.tud_id = @tud_id
	)
	, TabelaObservacaoConselho AS 
	(
		SELECT
			tur_id
			, alu_id
			, mtu_id
			, CASE WHEN qtdeQualidade = 0 AND qtdeDesempenhos = 0 AND qtdeRecomendacao = 0
						AND ato_qualidade IS NULL AND ato_desempenhoAprendizado IS NULL 
						AND ato_recomendacaoAluno IS NULL AND ato_recomendacaoResponsavel IS NULL
				   -- nenhum campo preenchido
				   THEN 0
				   ELSE
					(CASE WHEN ato_desempenhoAprendizado IS NOT NULL 
							AND ato_recomendacaoAluno IS NOT NULL 
							AND ato_recomendacaoResponsavel IS NOT NULL
					-- todos os campos preenchidos
					THEN 1
					-- algum campo preenchido
					ELSE 2
					END)
			  END AS observacaoPreenchida
		FROM
		(
			SELECT
				Mtr.tur_id
				, Mtr.alu_id
				, Mtr.mtu_id
				, SUM(CASE WHEN aaq.tqa_id IS NULL THEN 0 ELSE 1 END) AS qtdeQualidade
 				, SUM(CASE WHEN aad.tda_id IS NULL THEN 0 ELSE 1 END) AS qtdeDesempenhos
 				, SUM(CASE WHEN aar.rar_id IS NULL THEN 0 ELSE 1 END) AS qtdeRecomendacao
				, ato_qualidade
				, ato_desempenhoAprendizado
				, ato_recomendacaoAluno
				, ato_recomendacaoResponsavel
			FROM
				@Matriculas Mtr
				INNER JOIN ACA_Avaliacao ava -- WITH (NOLOCK)
					ON Mtr.fav_id = ava.fav_id
					AND ava.tpc_id = @ultimoPeriodo
					AND ava.ava_exibeObservacaoConselhoPedagogico = 1
				LEFT JOIN CLS_AlunoAvaliacaoTurmaQualidade aaq -- WITH (NOLOCK)
					ON  Mtr.tur_id = aaq.tur_id
					AND Mtr.alu_id = aaq.alu_id
					AND Mtr.mtu_id = aaq.mtu_id
					AND aaq.fav_id = ava.fav_id
					AND aaq.ava_id = ava.ava_id
				LEFT JOIN CLS_AlunoAvaliacaoTurmaDesempenho aad -- WITH (NOLOCK)
					ON  Mtr.tur_id = aad.tur_id
					AND Mtr.alu_id = aad.alu_id
					AND Mtr.mtu_id = aad.mtu_id
					AND aad.fav_id = ava.fav_id
					AND aad.ava_id = ava.ava_id 
				LEFT JOIN CLS_AlunoAvaliacaoTurmaRecomendacao aar -- WITH (NOLOCK)
					ON  Mtr.tur_id = aar.tur_id
					AND Mtr.alu_id = aar.alu_id
					AND Mtr.mtu_id = aar.mtu_id
					AND aar.fav_id = ava.fav_id
					AND aar.ava_id = ava.ava_id	        
				LEFT JOIN CLS_ALunoAvaliacaoTurmaObservacao ato -- WITH (NOLOCK)
					ON Mtr.tur_id = ato.tur_id
					AND Mtr.alu_id = ato.alu_id
					AND Mtr.mtu_id = ato.mtu_id
					AND ato.fav_id = ava.fav_id
					AND ato.ava_id = ava.ava_id
					AND ato.ato_situacao <> 3		
			WHERE
				Mtr.tud_id = @tud_id
			GROUP BY
				Mtr.tur_id
				, Mtr.alu_id
				, Mtr.mtu_id
				, ato_qualidade
				, ato_desempenhoAprendizado
				, ato_recomendacaoAluno
				, ato_recomendacaoResponsavel
		) 
		AS tabela
		GROUP BY --(Adicionado group by por Webber) 
			tabela.tur_id
			, tabela.alu_id 
			, tabela.mtu_id 
			, CASE WHEN qtdeQualidade = 0 AND qtdeDesempenhos = 0 AND qtdeRecomendacao = 0
							AND ato_qualidade IS NULL AND ato_desempenhoAprendizado IS NULL 
							AND ato_recomendacaoAluno IS NULL AND ato_recomendacaoResponsavel IS NULL
					   -- nenhum campo preenchido
					   THEN 0
					   ELSE
						(CASE WHEN ato_desempenhoAprendizado IS NOT NULL 
								AND ato_recomendacaoAluno IS NOT NULL 
								AND ato_recomendacaoResponsavel IS NOT NULL
						-- todos os campos preenchidos
						THEN 1
						-- algum campo preenchido
						ELSE 2
						END)
				  END	
	)	
	, TabelaMovimentacao AS (
			--Selecina as movimentações que possuem matrícula anterior
			SELECT
				MOV.alu_id,
				mtu_idAnterior,
				CASE WHEN tmo_tipoMovimento IN (6, 12, 14, 15, 16)
					 THEN TMV.tmv_nome + ' em ' + REPLACE(CONVERT(VARCHAR(10), mov.mov_dataRealizacao, 103), '/' + CAST(DATEPART(YEAR, mov.mov_dataRealizacao) AS VARCHAR(4)), '') +
					 	  ISNULL(' p/ ' + turD.tur_codigo, '')
					 WHEN tmo_tipoMovimento IN (8)
					 THEN TMV.tmv_nome + ' p/' + ISNULL(' ' + turD.tur_codigo, '')
					 WHEN tmo_tipoMovimento IN (11)
					 THEN TMV.tmv_nome + ' p/ ' + ISNULL(' ' + turD.tur_codigo, '')
					 ELSE TMV.tmv_nome
				END tmv_nome  
			FROM
				@Matriculas mtr
				INNER JOIN MTR_Movimentacao MOV -- WITH (NOLOCK) 
					ON mtr.alu_id = MOV.alu_id
				INNER JOIN ACA_TipoMovimentacao TMV -- WITH (NOLOCK) 
					ON MOV.tmv_idSaida = TMV.tmv_id
				LEFT JOIN MTR_TipoMovimentacao tmo -- WITH(NOLOCK)
					ON mov.tmo_id = tmo.tmo_id
					AND tmo.tmo_situacao <> 3
				LEFT JOIN MTR_MatriculaTurma mtuD -- WITH(NOLOCK)
					ON mov.alu_id = mtuD.alu_id
					AND mov.mtu_idAtual = mtuD.mtu_id
				LEFT JOIN TUR_Turma turD -- WITH(NOLOCK)
					ON mtuD.tur_id = turD.tur_id
				LEFT JOIN ACA_CalendarioAnual calD -- WITH(NOLOCK)
					ON turD.cal_id = calD.cal_id
				INNER JOIN MTR_MatriculaTurma mtuO -- WITH(NOLOCK)
					ON mov.alu_id = mtuO.alu_id
					AND mov.mtu_idAnterior = mtuO.mtu_id
					AND mtuO.tur_id = @tur_id
				LEFT JOIN TUR_Turma turO -- WITH(NOLOCK)
					ON mtuO.tur_id = turO.tur_id
				LEFT JOIN ACA_CalendarioAnual calO -- WITH(NOLOCK)
					ON turO.cal_id = calO.cal_id
			WHERE
				mov_situacao NOT IN (3,4)
				AND tmv_situacao <> 3
				AND mtu_idAnterior IS NOT NULL	
	)
	, tbRetorno AS (	
		SELECT
			  Mtd.alu_id
			, Mtd.mtu_id
			, Mtd.mtd_id
			, alc.alc_matricula
			, F.atd_id AS AvaliacaoID
			, COALESCE(F.atd_avaliacaoPosConselho, Caf.caf_avaliacao, F.atd_avaliacao, '') as Avaliacao 
			, CASE WHEN @permiteAlterarResultado = 0 
					THEN NULL
					-- Caso contrário, traz o resultado normalmente
					ELSE Mtd.mtd_resultado 
				END AS AvaliacaoResultado	
			, CASE WHEN ISNULL(Mtr.tud_tipo,0) = 18
				-- Quando for "Experiência" (Território do Saber), se não existir registro de fechamento traz 100% de frequência
				-- bimestral, pois pode não existir lançamento para o aluno no respectivo tud_id, em caso de transferência
				THEN COALESCE(Caf.caf_frequencia, F.atd_frequencia, 100)
				-- Para os demais casos, mantem a frequencia bimestral em 0%
				ELSE COALESCE(Caf.caf_frequencia, F.atd_frequencia, 0)
			  END AS Frequencia
			, CASE WHEN @documentoOficial = 1 THEN Pes.pes_nomeOficial ELSE Pes.pes_nome END + 
				(
					CASE WHEN ( ISNULL(Mtr.mtd_situacaoDocente, Mtd.mtd_situacao) = 5 ) 
						THEN ISNULL((SELECT TOP 1 ' (' + tmv_nome + ')' FROM TabelaMovimentacao MOV -- WITH (NOLOCK)
									 WHERE MOV.mtu_idAnterior = Mtd.mtu_id AND MOV.alu_id = Mtd.alu_id), ' (Inativo)')
						ELSE '' 
					END
				) 
				AS pes_nome
			, CASE WHEN ISNULL(Mtr.mtd_numeroChamadaDocente, Mtd.mtd_numeroChamada) > 0 
					THEN CAST(ISNULL(Mtr.mtd_numeroChamadaDocente, Mtd.mtd_numeroChamada) AS VARCHAR)
					ELSE '-' 
				END AS mtd_numeroChamada
			, ISNULL(Mtr.mtd_numeroChamadaDocente, Mtd.mtd_numeroChamada) AS mtd_numeroChamadaordem
			, ISNULL(Mtr.mtd_situacaoDocente, Mtd.mtd_situacao) AS situacaoMatriculaAluno
			, F.atd_relatorio
			, F.arq_idRelatorio
			, ISNULL(Mtr.mtd_dataMatriculaDocente, Mtd.mtd_dataMatricula) AS dataMatricula
			, ISNULL(Mtr.mtd_dataSaidaDocente, Mtd.mtd_dataSaida) AS dataSaida
			, COALESCE(Caf.caf_frequenciaFinalAjustada, F.atd_frequenciaFinalAjustada, 100) AS FrequenciaFinalAjustada
			, ava.tpc_id
			, ava.ava_nome AS NomeAvaliacao
			, F.atd_avaliacaoPosConselho AS AvaliacaoPosConselho
			, ava.cap_dataInicio
			, CAST(ISNULL(toc.observacaoPreenchida, 0) AS TINYINT) AS observacaoConselhoPreenchida
			-- se o aluno nao teve a nota efetivada no periodo,
			-- mas ele estava presente no periodo
			-- deve-se informar o usuario.
			, CAST(CASE WHEN 
					(
					(  
						/*
						(
							COALESCE(F.atd_avaliacaoPosConselho, F.atd_avaliacao, '') <> ''
							OR
							(								
								-- se for o ultimo periodo,
								-- e nao tiver fechamento
								-- deve ter a nota do Listao
								ISNULL(ava.tpc_id, 0) = @ultimoPeriodo
								AND F.atd_id IS NULL
								AND ISNULL(Caf.caf_avaliacao,'') <> ''
							)
						)
						*/
						COALESCE(F.atd_avaliacaoPosConselho, Caf.caf_avaliacao, F.atd_avaliacao, '') <> ''
						AND (ISNULL(F.atd_id, 0) > 0 OR ISNULL(ava.tpc_id, 0) = @ultimoPeriodo)
						AND ISNULL(tud.tud_naoLancarNota, 0) = 0
					)
					OR 
					(
						ISNULL(tud.tud_naoLancarNota, 0) = 1
						AND ISNULL(F.atd_id,0) > 0
					))
					AND
					(
						ISNULL(pend.DisciplinaSemAula, 0) = 0 AND
						ISNULL(pend.SemNota, 0) = 0
					)
					THEN 1
					ELSE 0
			   END AS BIT) AS PossuiNota
			, ava.ava_id AS ava_id
			, CASE WHEN (ava.tpc_id IS NOT NULL AND ava.tpc_id = @ultimoPeriodo) 
					THEN 1 
					ELSE 0 
				END AS UltimoPeriodo
			, COALESCE(Caf.caf_qtAulas, F.atd_numeroAulas, 0) as QtAulasAluno		
			, COALESCE(Caf.caf_qtAulasReposicao, F.atd_numeroAulasReposicao, 0) AS QtAulasAlunoReposicao
			, COALESCE(Caf.caf_qtFaltas, F.atd_numeroFaltas, 0) as QtFaltasAluno
			, COALESCE(Caf.caf_qtFaltasReposicao, F.atd_numeroFaltasReposicao, 0) AS QtFaltasAlunoReposicao
			, COALESCE(Caf.caf_qtAusenciasCompensadas, F.atd_ausenciasCompensadas, 0) AS ausenciasCompensadas
			, mtu.mtu_resultado
			, F.atd_numeroAulas AS QtAulasEfetivado
			, ISNULL(tpc.tpc_ordem, 0) AS tpc_ordem
			, ava.ForaRede
			, ava.PresencaDisciplina
			, ava.PresencaEscola
		FROM @Matriculas Mtr
		INNER JOIN MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
			ON  Mtd.alu_id = Mtr.alu_id
			AND Mtd.mtu_id = Mtr.mtu_id
			AND Mtd.mtd_id = Mtr.mtd_id
			AND Mtd.tud_id = @tud_id
			AND Mtd.mtd_situacao <> 3
		INNER JOIN TUR_TurmaDisciplina tud -- WITH (NOLOCK)
			ON Mtd.tud_id = tud.tud_id
			AND tud.tud_situacao <> 3
		INNER JOIN MTR_MatriculaTurma mtu -- WITH (NOLOCK)
			ON mtu.alu_id = Mtd.alu_id
			AND mtu.mtu_id = Mtd.mtu_id
			AND mtu_situacao <> 3    
		INNER JOIN ACA_AlunoCurriculo alc -- WITH (NOLOCK)
			ON alc.alu_id = mtu.alu_id
			AND alc.alc_id = mtu.alc_id
			AND alc.alc_situacao <> 3	
		INNER JOIN ACA_Aluno Alu -- WITH (NOLOCK)
			ON  Mtd.alu_id   = Alu.alu_id
			AND alu_situacao <> 3
		INNER JOIN VW_DadosAlunoPessoa Pes -- WITH (NOLOCK)
	        ON  Alu.alu_id   = Pes.alu_id
		INNER JOIN alunoBimestre ava 
			ON ava.alu_id = mtr.alu_id
		LEFT JOIN REL_AlunosSituacaoFechamento pend -- WITH(NOLOCK)
			ON tud.tud_id = pend.tud_id
			AND ava.tpc_id = pend.tpc_id
			AND Mtr.alu_id = pend.alu_id
			AND Mtr.mtu_id = pend.mtu_id
			AND Mtr.mtd_id = pend.mtd_id
		LEFT JOIN ACA_TipoPeriodoCalendario tpc -- WITH (NOLOCK)
			ON ava.tpc_id = tpc.tpc_id
			AND tpc.tpc_situacao <> 3    
		LEFT JOIN @Fechado F 
			ON (F.alu_id = Mtd.alu_id 
					AND F.mtu_id = Mtd.mtu_id 
					AND F.mtd_id = Mtd.mtd_id
					AND ((F.tpc_id IS NULL AND ava.tpc_id IS NULL) OR F.tpc_id = ava.tpc_id)
				)
				------------------------------------------------------------
				-- Fechado em outras turmas
				OR (F.alu_id = Mtd.alu_id 
					AND F.mtu_id <> Mtd.mtu_id
					AND F.tpc_id = ava.tpc_id
				)	
				------------------------------------------------------------	  		
		LEFT JOIN CLS_AlunoFechamento Caf -- WITH (NOLOCK)
			ON  Caf.tud_id = Mtd.tud_id
			AND Caf.tpc_id = ava.tpc_id
			AND Caf.alu_id = Mtd.alu_id
			AND Caf.mtu_id = Mtd.mtu_id
			AND Caf.mtd_id = Mtd.mtd_id
			AND ava.tpc_id IS NOT NULL 
			AND Caf.tpc_id = ava.tpc_id
			--AND Caf.tpc_id = @ultimoPeriodo		
		LEFT JOIN TabelaObservacaoConselho toc
			ON 
			toc.alu_id = Mtu.alu_id
			AND toc.mtu_id = Mtu.mtu_id		        
		WHERE 
			Mtr.tpc_id = @ultimoPeriodo
			AND ISNULL(Mtr.mtd_situacaoDocente, mtd_situacao) IN (1,5)
			AND COALESCE(Mtr.mtd_numeroChamadaDocente, mtd_numeroChamada, 0) >= 0		
	)
	, tbRetornoUltimoPeriodo AS
	(
		SELECT alu_id, mtu_id, mtd_id, FrequenciaFinalAjustada
		FROM tbRetorno
		WHERE UltimoPeriodo = 1
	)	
	, tbFinal AS 
	(
		SELECT 
			@tur_id AS tur_id
			, @tud_id AS tud_id
			, r.alu_id
			, r.mtu_id
			, r.mtd_id
			, alc_matricula
			, AvaliacaoID
			, Avaliacao
			, AvaliacaoResultado		
			, Frequencia
			, pes_nome		
			, mtd_numeroChamada
			, atd_relatorio
			, arq_idRelatorio
			, situacaoMatriculaAluno
			, dataMatricula
			, dataSaida
			-- Se for a avaliação final, pego a frequencia final ajustada do ultimo periodo
			, CASE WHEN (tpc_id IS NULL)
				THEN Up.FrequenciaFinalAjustada
				ELSE r.FrequenciaFinalAjustada
				END AS FrequenciaFinalAjustada
			, ISNULL(tpc_id, -1) AS tpc_id
			, NomeAvaliacao
			, AvaliacaoPosConselho
			, observacaoConselhoPreenchida
			-- Valida o fechamento apenas se o aluno estava 
			-- presente na escola no periodo da avaliação
			, CASE WHEN PresencaDisciplina = 1 AND PresencaEscola = 1 THEN PossuiNota ELSE 1 END AS PossuiNota
			, ava_id
			, UltimoPeriodo
			, QtAulasAluno	
			, QtAulasAlunoReposicao		
			, QtFaltasAluno		
			, QtFaltasAlunoReposicao
			, ausenciasCompensadas
			, mtu_resultado
			, CASE WHEN ForaRede = 1 AND ISNULL(tpc_id, -1) > 0 THEN 1 ELSE 0 END AS AlunoForaDaRede
			, QtAulasEfetivado
			, cap_dataInicio
			, mtd_numeroChamadaordem
			, tpc_ordem
		FROM tbRetorno r
		LEFT JOIN tbRetornoUltimoPeriodo Up 
			ON Up.alu_id = r.alu_id 
			AND Up.mtu_id = r.mtu_id 
			AND Up.mtd_id = r.mtd_id	
		GROUP BY
			cap_dataInicio
			, tpc_id
			, NomeAvaliacao
			, r.alu_id
			, r.mtu_id
			, r.mtd_id
			, alc_matricula
			, AvaliacaoID
			, Avaliacao
			, AvaliacaoResultado		
			, Frequencia
			, pes_nome		
			, mtd_numeroChamada
			, mtd_numeroChamadaordem
			, atd_relatorio
			, arq_idRelatorio
			, situacaoMatriculaAluno
			, dataMatricula
			, dataSaida
			, r.FrequenciaFinalAjustada
			, Up.FrequenciaFinalAjustada
			, AvaliacaoPosConselho
			, observacaoConselhoPreenchida
			, ava_id
			, UltimoPeriodo
			, QtAulasAluno	
			, QtAulasAlunoReposicao		
			, QtFaltasAluno		
			, QtFaltasAlunoReposicao
			, ausenciasCompensadas
			, mtu_resultado
			, PossuiNota
			, QtAulasEfetivado
			, tpc_ordem
			, ForaRede
			, PresencaDisciplina
			, PresencaEscola
	)	
	SELECT
		tur_id
		, tud_id
		, alu_id
		, mtu_id
		, mtd_id
		, alc_matricula
		, AvaliacaoID
		, Avaliacao
		, AvaliacaoResultado		
		, Frequencia
		, pes_nome		
		, mtd_numeroChamada
		, atd_relatorio
		, arq_idRelatorio
		, situacaoMatriculaAluno
		, dataMatricula
		, dataSaida
		, FrequenciaFinalAjustada
		, tpc_id
		, NomeAvaliacao
		, AvaliacaoPosConselho
		, observacaoConselhoPreenchida
		, CASE WHEN AlunoForaDaRede = 1 OR PossuiNota = 1 THEN 0 ELSE 1 END AS SemNota
		, ava_id
		, UltimoPeriodo
		, QtAulasAluno	
		, QtAulasAlunoReposicao		
		, QtFaltasAluno		
		, QtFaltasAlunoReposicao
		, ausenciasCompensadas
		, mtu_resultado
		, AlunoForaDaRede
		, QtAulasEfetivado
		, tpc_ordem
	FROM
		tbFinal
	ORDER BY 
		cap_dataInicio
		, tpc_id
		, ava_id
		, CASE 
			WHEN @ordenacao = 0 THEN 
				CASE WHEN ISNULL(mtd_numeroChamadaordem,0) <= 0 THEN 1 ELSE 0 END
		END ASC
		, CASE WHEN @ordenacao = 0 THEN ISNULL(mtd_numeroChamadaordem,0) END ASC
		, CASE WHEN @ordenacao = 1 THEN pes_nome END ASC
END
GO
PRINT N'Altering [dbo].[STP_ACA_EventoLimite_UPDATE]'
GO
ALTER PROCEDURE [dbo].[STP_ACA_EventoLimite_UPDATE]
	@cal_id INT
	, @tev_id INT
	, @evl_id INT
	, @tpc_id INT
	, @esc_id INT
	, @uni_id INT
	, @evl_dataInicio DATETIME
	, @evl_dataFim DATETIME
	, @usu_id UNIQUEIDENTIFIER
	, @evl_situacao TINYINT
	, @evl_dataCriacao DATETIME
	, @evl_dataAlteracao DATETIME
	, @uad_id UniqueIdentifier

AS
BEGIN
	UPDATE ACA_EventoLimite 
	SET 
		tpc_id = @tpc_id 
		, esc_id = @esc_id 
		, uni_id = @uni_id 
		, evl_dataInicio = @evl_dataInicio 
		, evl_dataFim = @evl_dataFim 
		, usu_id = @usu_id 
		, evl_situacao = @evl_situacao 
		, evl_dataCriacao = @evl_dataCriacao 
		, evl_dataAlteracao = @evl_dataAlteracao 
		, uad_id = @uad_id

	WHERE 
		cal_id = @cal_id 
		AND tev_id = @tev_id 
		AND evl_id = @evl_id 

		
	DECLARE @ret INT
	SELECT @ret = ISNULL(@@ROWCOUNT,-1)
	RETURN @ret
	
END
GO
PRINT N'Altering [dbo].[NEW_Relatorio_005_SubAAtaFinalEnriquecimentoCurricular]'
GO
-- ==========================================================================================
-- Author:		Rafael Benevente
-- Create date: 05/11/2014
-- Description:	Procedure para a geração de dados para o relatório de ata final de resultados

---- Alterado: Marcia Haga - 12/03/2015
---- Description: Alterado para retornar o aluno como inativo, caso a 
---- situacao do aluno seja nula.

---- Alterado: Juliano Real - 07/06/2016
---- Description: Alterado para retornar as faltas do alunos quando inativo na turma

---- Alterado: Marcia Haga - 24/08/2016
---- Description: Alterada verificação de período fechado da experiência
----  para considerar as vigências dos territórios.
-- ==========================================================================================
ALTER PROCEDURE [dbo].[NEW_Relatorio_005_SubAAtaFinalEnriquecimentoCurricular]
	@tur_id BIGINT
	, @cal_id INT
	, @documentoOficial BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
    DECLARE @MatriculasBoletim TABLE
	(
		alu_id BIGINT
		, mtu_id INT
		, tur_id BIGINT
		, tpc_id INT
		, tpc_ordem INT
		, PeriodosEquivalentes BIT
		, MesmoCalendario BIT
		, MesmoFormato BIT
		, fav_id INT
		, mtu_numeroChamada INT
		, cal_id INT
		, cal_ano INT
		, cap_id INT
		, PossuiSaidaPeriodo BIT
		, registroExterno BIT 
		, PermiteConceitoGlobal	BIT
		, PermiteDisciplinas BIT
		, tur_idMatriculaBoletim BIGINT
		-- Indica se o fechamento do último bimestre foi realizado nessa turma (na avaliação final).
		, FechamentoUltimoBimestre Bit
		PRIMARY KEY (alu_id, mtu_id, tur_id, tpc_id, tpc_ordem)
	)
	
	; WITH Tabela AS 
		(
			SELECT mb.alu_id, mb.mtu_id, mb.tur_id, mb.tpc_id, mb.tpc_ordem, mb.PeriodosEquivalentes, 
				mb.MesmoCalendario, mb.MesmoFormato, mb.fav_id, mtu.mtu_numeroChamada, mb.cal_id, mb.cal_ano, mb.cap_id , mb.PossuiSaidaPeriodo,
				mb.registroExterno, mb.PermiteConceitoGlobal, mb.PermiteDisciplinas
			from MTR_MatriculasBoletim mb WITH(NOLOCK)
			inner join (select *, ROW_NUMBER() OVER(PARTITION BY alu_id 
													   ORDER BY mtu_id desc) as linha
						 from MTR_MatriculaTurma WITH(NOLOCK) 
						where mtu_situacao <> 3
						  and tur_id = @tur_id) mtu 
				   on mtu.alu_id = mb.alu_id and mtu.mtu_id = mb.mtu_origemDados
			 where mtu.linha = 1 --para evitar casos onde o aluno tem dois mtus para o mesmo tur_id
			 AND mb.PeriodosEquivalentes = 1 -- traz apenas alunos que possuem períodos equivalentes
			 AND mb.PossuiSaidaPeriodo = 0
			 AND mb.registroExterno = 0
		)

		, Tabela2 AS
		(
			SELECT
				mb.alu_id
				, mb.mtu_id
				, mb.tur_id
				, mb.tpc_id
				, mb.tpc_ordem
				, mb.PeriodosEquivalentes
				, mb.MesmoCalendario
				, mb.MesmoFormato
				, mb.fav_id
				, mb.mtu_numeroChamada
				, mb.cal_id
				, mb.cal_ano
				, mb.cap_id
				, mb.PossuiSaidaPeriodo
				, mb.registroExterno
				, mb.PermiteConceitoGlobal
				, mb.PermiteDisciplinas
				, CASE WHEN 
					-- se a turma do relatorio for do 4 bimeste, trazer todos os registros
					-- se nao, trazer so onde tem fechamento naquela turma
					EXISTS 
					(
						SELECT 1
						FROM Tabela T
						WHERE 
							T.alu_id = mb.alu_id
							AND T.tpc_id = 4 
							AND T.tur_id = @tur_id 
							--AND T.tpc_id = mb.tpc_id
					)
					OR mb.tur_id = @tur_id
					THEN 1 ELSE 0 END AS ExibirMtu
				, CASE WHEN 
					-- se a turma do relatório não for do fechamento de nenhum bimestre para esse aluno,
					-- não exibir o aluno.
					EXISTS 
					(
						SELECT 1
						FROM Tabela T
						WHERE 
							T.alu_id = mb.alu_id
							AND T.tur_id = @tur_id
					)
					THEN 1 ELSE 0 END AS ExibirAluno
			FROM Tabela mb
		)

		INSERT INTO @MatriculasBoletim (alu_id, mtu_id, tur_id, tpc_id, tpc_ordem, PeriodosEquivalentes, 
							MesmoCalendario, MesmoFormato, fav_id, mtu_numeroChamada, cal_id, cal_ano, cap_id , PossuiSaidaPeriodo,
							registroExterno, PermiteConceitoGlobal, PermiteDisciplinas
							, tur_idMatriculaBoletim, FechamentoUltimoBimestre)
		SELECT
			mb.alu_id
			, CASE WHEN ExibirMtu = 1 THEN mb.mtu_id ELSE NULL END AS mtu_id
			, CASE WHEN ExibirMtu = 1 THEN mb.tur_id ELSE @tur_id END AS tur_id
			, mb.tpc_id
			, mb.tpc_ordem
			, mb.PeriodosEquivalentes
			, mb.MesmoCalendario
			, mb.MesmoFormato
			, mb.fav_id
			, mb.mtu_numeroChamada AS mtu_numeroChamada
			, mb.cal_id
			, mb.cal_ano
			, mb.cap_id
			, mb.PossuiSaidaPeriodo
			, mb.registroExterno
			, mb.PermiteConceitoGlobal
			, mb.PermiteDisciplinas
			, mb.tur_id AS tur_idMatriculaBoletim
			, Mb.ExibirMtu AS FechamentoUltimoBimestre
		FROM Tabela2 mb
		WHERE mb.ExibirAluno = 1 and ExibirMtu = 1
	
	DECLARE @alunosDisciplina TABLE (
		alu_id BIGINT,
		tur_id BIGINT,
		tpc_id INT,
		mtu_id INT,
		fav_id INT,
		tud_id BIGINT,
		tud_tipo TINYINT,
		dis_id INT,
		dis_nome VARCHAR(250),
		tds_ordem INT,
		tpc_ordem INT,
		tur_idMatriculaBoletim BIGINT,
		mtu_numeroChamada INT,
		FechamentoUltimoBimestre BIT
	)

	INSERT INTO @alunosDisciplina
	SELECT
		Mb.alu_id,
			Mb.tur_id,
			Mb.tpc_id,
			mb.mtu_id,
			Mb.fav_id,
			Tud.tud_id,
			Tud.tud_tipo,
			Dis.tds_id AS dis_id,
			Dis.dis_nome,
			Tds.tds_ordem,
			mb.tpc_ordem,
			mb.tur_idMatriculaBoletim,
			MB.mtu_numeroChamada,
			FechamentoUltimoBimestre
	FROM
		@MatriculasBoletim Mb
		INNER JOIN Tur_Turma Tur WITH(NOLOCK)
			ON Tur.tur_id = Mb.tur_id
			AND Tur.tur_situacao <> 3
		INNER JOIN TUR_TurmaRelTurmaDisciplina TrT WITH(NOLOCK)
			ON TrT.tur_id = Tur.tur_id
		INNER JOIN TUR_TurmaDisciplina Tud WITH(NOLOCK)
			ON Tud.tud_id = TrT.tud_id
			AND Tud.tud_tipo <> 17
			AND Tud.tud_situacao <> 3
		INNER JOIN TUR_TurmaDisciplinaRelDisciplina TrD WITH(NOLOCK)
			ON TrD.tud_id = Tud.tud_id
		INNER JOIN ACA_Disciplina Dis WITH(NOLOCK)
			ON Dis.dis_id = TrD.dis_id
			AND Dis.dis_situacao <> 3
		INNER JOIN ACA_TipoDisciplina Tds WITH(NOLOCK)
			ON Tds.tds_id = Dis.tds_id
			-- Enriquecimento curricular ou território do saber
			AND (Tds.tds_tipo = 2 OR Tud.tud_tipo = 18) 
			AND Tds.tds_situacao <> 3
	
	DECLARE @tipoResultado TABLE (
		tpr_nomenclatura VARCHAR(100),
		tpr_resultado TINYINT,
		tpr_tipoLancamento TINYINT,
		cur_id INT,
		crr_id INT,
		crp_id INT
	)
	INSERT INTO @tipoResultado
	SELECT
		tpr.tpr_nomenclatura,
		tpr.tpr_resultado,
		tpr.tpr_tipoLancamento,
		tcr.cur_id,
		tcr.crr_id,
		tcr.crp_id
	FROM
		ACA_TipoResultado tpr WITH(NOLOCK)
		INNER JOIN ACA_TipoResultadoCurriculoPeriodo tcr WITH(NOLOCK)
			ON tpr.tpr_id = tcr.tpr_id
	WHERE
		tpr.tpr_situacao <> 3

	-- Segue mesma lógica de NEW_CLS_TurmaAula_VerificaPendenciaCadastroAulaExperiencia
	DECLARE @disciplinasExperiencia TABLE (tud_id BIGINT, tpc_id INT)
	INSERT INTO @disciplinasExperiencia
	SELECT Ad.tud_id, Ad.tpc_id 
	FROM @alunosDisciplina Ad
	WHERE Ad.tud_tipo = 18 AND Ad.tpc_id > 0
	GROUP BY Ad.tud_id, Ad.tpc_id

	DECLARE @vigenciaExperiencia TABLE (
		tud_id BIGINT,
		tud_idTerritorio BIGINT,
		tpc_id INT,
		VigenciaInicial DATETIME,
		VigenciaFinal DATETIME
	)
	INSERT INTO @vigenciaExperiencia
	SELECT
		tud.tud_id
		, tte.tud_idTerritorio					
		, tud.tpc_id

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
	FROM @disciplinasExperiencia tud
	INNER JOIN ACA_CalendarioPeriodo cap WITH (NOLOCK)
		ON cap.cal_id = @cal_id
		AND cap.tpc_id = tud.tpc_id
	INNER JOIN TUR_TurmaDisciplinaTerritorio tte WITH (NOLOCK)
		ON tte.tud_idExperiencia = tud.tud_id
		-- Apenas experiências ativas em cada período do calendário			
		AND ( 
				tte.tte_vigenciaInicio BETWEEN cap.cap_dataInicio AND cap.cap_dataFim
				OR tte.tte_vigenciaFim BETWEEN cap.cap_dataInicio AND cap.cap_dataFim
				OR cap.cap_dataInicio BETWEEN tte.tte_vigenciaInicio AND tte.tte_vigenciaFim
				OR cap.cap_dataFim BETWEEN tte.tte_vigenciaInicio AND tte.tte_vigenciaFim
			)
		AND tte.tte_situacao <> 3				
	GROUP BY
		tud.tud_id
		, tte.tud_idTerritorio			
		, tud.tpc_id		
		, tte.tte_vigenciaInicio
		, tte.tte_vigenciaFim
		, cap.cap_dataInicio
		, cap.cap_dataFim
	
	DECLARE @semAulaExperiencia TABLE (tud_id BIGINT, tpc_id INT)

	;WITH semAulaVigenciaExperiencia AS
	(		
		SELECT 
			Tud.tud_id		
			, Tud.tpc_id		
			-- Verifica se existe aula criada para cada período de vigência de cada experiência dentro de cada período do calendário
			, ISNULL((
					SELECT TOP 1 1
					FROM CLS_TurmaAulaTerritorio Tat WITH(NOLOCK)
					INNER JOIN CLS_TurmaAula Tau WITH(NOLOCK)
						ON Tau.tud_id = Tat.tud_idTerritorio
						AND Tau.tau_id = Tat.tau_idTerritorio
						AND Tau.tpc_id = Tud.tpc_id
						AND Tau.tau_data BETWEEN Tud.VigenciaInicial and Tud.VigenciaFinal
						AND Tau.tau_situacao <> 3
					WHERE
						Tat.tud_idExperiencia = Tud.tud_id
						AND Tat.tud_idTerritorio = Tud.tud_idTerritorio
				),0) AS AulaCriada
		FROM 
			@vigenciaExperiencia Tud
	)
	, semAulaExperiencia1 AS
	(
		-- Retorna apenas as disciplinas que não possuem aula criada em nenhum dos períodos de vigência da experiência dentro de cada período do calendário
		SELECT 
			Tud.tud_id, Tud.tpc_id				
		FROM
			semAulaVigenciaExperiencia Tud
		GROUP BY
			Tud.tud_id, Tud.tpc_id				
		HAVING 
			SUM(Tud.AulaCriada) = 0
	)
	
	INSERT INTO @semAulaExperiencia
	SELECT tud_id, tpc_id FROM semAulaExperiencia1

	DECLARE @turmaAulas TABLE (tud_id BIGINT, tpc_id INT, tau_data DATETIME)

	;WITH TudPeriodo AS 
	(
		SELECT
			Ad.tud_id,
			Ad.tpc_id
		FROM @alunosDisciplina Ad
		GROUP BY
			Ad.tud_id,
			Ad.tpc_id
	)
	
	INSERT INTO @turmaAulas
	SELECT
		Tau.tud_id,
		Tau.tpc_id,
		Tau.tau_data
	FROM TudPeriodo Tp
	INNER JOIN CLS_TurmaAula AS Tau	with(nolock)
		ON Tau.tud_id = Tp.tud_id 
		AND Tau.tpc_id = Tp.tpc_id
		AND Tau.tau_situacao <> 3
	GROUP BY
		Tau.tud_id,
		Tau.tpc_id,
		Tau.tau_data

	--
	;WITH frequenciaExterna AS 
	(
		SELECT
			Mtd.alu_id,
			Mtd.mtu_id,
			Mtd.mtd_id,
			dis.tds_id AS dis_id,
			Ad.tpc_id,
			afx.afx_qtdFaltas AS qtdFaltas,
			afx.afx_qtdAulas AS qtdAulas
		FROM @alunosDisciplina Ad
		INNER JOIN MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
			ON Mtd.alu_id = Ad.alu_id
			AND Mtd.mtu_id = Ad.mtu_id
			AND Mtd.tud_id = Ad.tud_id
			AND Mtd.mtd_situacao <> 3
		INNER JOIN TUR_TurmaDisciplinaRelDisciplina Trd WITH(NOLOCK)
			ON Mtd.tud_id = Trd.tud_id
		INNER JOIN ACA_Disciplina dis WITH(NOLOCK)
			ON Trd.dis_id = dis.dis_id
			AND dis.dis_situacao <> 3
		INNER JOIN CLS_AlunoFrequenciaExterna afx WITH(NOLOCK)
			ON Mtd.alu_id = afx.alu_id
			AND Mtd.mtu_id = afx.mtu_id
			AND Mtd.mtd_id = afx.mtd_id
			AND Ad.tpc_id = afx.tpc_id
			AND afx.afx_situacao <> 3
		GROUP BY
			Mtd.alu_id,
			Mtd.mtu_id,
			Mtd.mtd_id,
			dis.tds_id,
			Ad.tpc_id,
			afx.afx_qtdFaltas,
			afx.afx_qtdAulas

		UNION

		SELECT
			Mtd.alu_id,
			Mtd.mtu_id,
			Mtd.mtd_id,
			dis.tds_id AS dis_id,
			Ad.tpc_id,
			SUM(ISNULL(afx.afx_qtdFaltas, 0)) AS qtdFaltas,
			SUM(ISNULL(afx.afx_qtdAulas, 0)) AS qtdAulas
		FROM @alunosDisciplina Ad
		INNER JOIN MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
			ON Mtd.alu_id = Ad.alu_id
			AND Mtd.mtu_id = Ad.mtu_id
			AND Mtd.tud_id = Ad.tud_id
			AND Mtd.mtd_situacao <> 3
		INNER JOIN TUR_TurmaDisciplinaRelDisciplina Trd WITH(NOLOCK)
			ON Mtd.tud_id = Trd.tud_id
		INNER JOIN ACA_Disciplina dis WITH(NOLOCK)
			ON Trd.dis_id = dis.dis_id
			AND dis.dis_situacao <> 3
		INNER JOIN CLS_AlunoFrequenciaExterna afx WITH(NOLOCK)
			ON Mtd.alu_id = afx.alu_id
			AND Mtd.mtu_id = afx.mtu_id
			AND Mtd.mtd_id = afx.mtd_id
			AND afx.afx_situacao <> 3
		WHERE
			Ad.tpc_id IS NULL AND
			NOT EXISTS(SELECT TOP 1 Ad2.tpc_id FROM @alunosDisciplina Ad2
						INNER JOIN MTR_MatriculaTurmaDisciplina Mtd2 WITH(NOLOCK)
							ON Mtd2.alu_id = Ad2.alu_id
							AND Mtd2.mtu_id = Ad2.mtu_id
							AND Mtd2.tud_id = Ad2.tud_id
							AND Mtd2.mtd_situacao <> 3
						WHERE Mtd2.alu_id = afx.alu_id
						AND Mtd2.mtu_id = afx.mtu_id
						AND Mtd2.mtd_id = afx.mtd_id
						AND Ad2.tpc_id = afx.tpc_id
						AND afx.afx_situacao <> 3)
		GROUP BY
			Mtd.alu_id,
			Mtd.mtu_id,
			Mtd.mtd_id,
			dis.tds_id,
			Ad.tpc_id
	)
	,  dadosAlunos AS
	(
		SELECT
			Ad.alu_id,
			Ad.mtu_id,
			Mtd.mtd_id,
			CASE WHEN @documentoOficial = 1 THEN Pes.pes_nomeOficial ELSE pes.pes_nome END AS pes_nome,
			Ad.tur_id,
			Ad.fav_id,
			Ad.tud_id,
			Ad.tud_tipo,
			ISNULL(Tpr.tpr_nomenclatura, '-') AS mtu_resultadoDescricao,
			Atd.atd_avaliacao,
			Atd.atd_frequencia,
			ISNULL(Atd.atd_numeroFaltas, 0) + ISNULL(afx.qtdFaltas, 0) AS atd_numeroFaltas,
			ISNULL(Atd.atd_numeroAulas, 0) + ISNULL(afx.qtdAulas, 0) AS atd_numeroAulas,
			ISNULL(Atd.atd_ausenciasCompensadas,0) AS atd_ausenciasCompensadas,
			Atd.atd_frequenciaFinalAjustada,
			Ad.dis_id,
			Ad.tpc_id,
			Ad.tds_ordem,
			Ad.tpc_ordem,
			CASE 
				WHEN Ad.tpc_ordem = 1 THEN 1
				WHEN Ad.tpc_ordem = 2 THEN 2
				WHEN Ad.tpc_ordem = 3 THEN 3
				WHEN Ad.tpc_ordem = 4 THEN 4
			END  AS Tpc_Agrupamento,
			CASE 
				WHEN Ad.tpc_ordem = 1 THEN '1ºB' 
				WHEN Ad.tpc_ordem = 2 THEN '2ºB' 
				WHEN Ad.tpc_ordem = 3 THEN '3ºB' 
				WHEN Ad.tpc_ordem = 4 THEN '4ºB' 
			END  AS Tpc_exibicao,
			Ad.dis_nome,
			Ad.mtu_numeroChamada,
			Mtu.mtu_resultado,
			--CASE WHEN (Atd.atd_id IS NOT NULL) THEN 1 ELSE 0 END AS periodoFechado,
			CASE 
				WHEN Ad.mtu_id IS NULL OR Mtd.mtd_id IS NULL OR (Ad.tpc_ordem IS NULL AND FechamentoUltimoBimestre = 0) THEN 1
				WHEN Ad.tud_tipo <> 18 AND (Atd.atd_id IS NOT NULL) AND  
					 EXISTS(SELECT TOP 1 Tau.tud_id FROM @turmaAulas AS Tau
							WHERE Tau.tud_id = Ad.tud_id AND Tau.tpc_id = Ad.tpc_id) THEN 1 
				WHEN Ad.tud_tipo = 18 AND (Atd.atd_id IS NOT NULL) AND
					 NOT EXISTS(SELECT TOP 1 sae.tud_id FROM @semAulaExperiencia sae 
								WHERE sae.tud_id = Ad.tud_id AND sae.tpc_id = Ad.tpc_id) THEN 1 
				ELSE 0 END AS periodoFechado,
			
			CASE WHEN tmo_tipoMovimento IN (8,23,27) THEN 1 ELSE ISNULL(Mtu.mtu_situacao, 5) END AS mtu_situacao,
			Fav.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina,
			CASE WHEN ISNULL(afx.qtdAulas, 0) > 0 OR ISNULL(afx.qtdFaltas, 0) > 0
				 THEN CAST(1 AS BIT) 
				 WHEN EXISTS(SELECT TOP 1 afx2.alu_id FROM frequenciaExterna afx2
							 WHERE Ad.alu_id = afx2.alu_id
							 AND Ad.dis_id = afx2.dis_id
							 AND Ad.tpc_id = afx2.tpc_id
							 AND (ISNULL(afx2.qtdAulas, 0) > 0 OR ISNULL(afx2.qtdFaltas, 0) > 0))
				 THEN CAST(1 AS BIT)
				 ELSE CAST(0 AS BIT) END AS PossuiFreqExternaAtual,
			CASE WHEN EXISTS(SELECT TOP 1 afx2.alu_id FROM frequenciaExterna afx2
							 WHERE Ad.alu_id = afx2.alu_id
							 AND Ad.dis_id = afx2.dis_id
							 AND (ISNULL(afx2.qtdAulas, 0) > 0 OR ISNULL(afx2.qtdFaltas, 0) > 0))
				 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS PossuiFreqExternaAcum
		FROM
			@alunosDisciplina Ad
			LEFT JOIN MTR_MatriculaTurma Mtu WITH(NOLOCK)
				ON Mtu.alu_id = Ad.alu_id
				    AND Mtu.mtu_id = Ad.mtu_id
					AND Mtu.tur_id = Ad.tur_id
					AND Mtu.mtu_situacao <> 3
			INNER JOIN ACA_Aluno Alu WITH (NOLOCK)
				ON Alu.alu_id = Ad.alu_id	
					AND Alu.alu_situacao <> 3
			INNER JOIN VW_DadosAlunoPessoa Pes
				ON  Pes.alu_id = Alu.alu_id
			LEFT JOIN ACA_AlunoCurriculo Alc WITH (NOLOCK)
				ON  Alc.alu_id = Mtu.alu_id
					AND Alc.alc_id = Mtu.alc_id
					AND Alc.alc_situacao <> 3
			LEFT JOIN MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
				ON  Mtd.alu_id = Mtu.alu_id
					AND Mtd.mtu_id = Mtu.mtu_id
					AND Mtd.tud_id = Ad.tud_id
					AND Mtd.mtd_situacao <> 3
			LEFT JOIN ACA_FormatoAvaliacao Fav WITH(NOLOCK)
				ON Fav.fav_id = Ad.fav_id
					AND Fav.fav_situacao <> 3
			LEFT JOIN ACA_Avaliacao Ava WITH(NOLOCK)
				ON Ava.fav_id = Fav.fav_id
					AND ISNULL(AVA.tpc_id, -1) = ISNULL(Ad.tpc_id, -1) -- Caso seja 999 sera a avaliacao final
					AND Ava.ava_situacao <> 3
			LEFT JOIN MTR_Movimentacao Mov WITH(NOLOCK)
				ON Mov.alu_id = Mtu.alu_id
				AND Mov.mtu_idAnterior = Mtu.mtu_id
				AND Mov.mov_situacao <> 3
			LEFT JOIN MTR_TipoMovimentacao Tmo WITH(NOLOCK)
				ON Tmo.tmo_id = Mov.tmo_id
			LEFT JOIN @tipoResultado Tpr
				ON Tpr.cur_id = Mtu.cur_id
				AND Tpr.crr_id = Mtu.crr_id
				AND Tpr.crp_id = Mtu.crp_id
				AND Tpr.tpr_resultado = Mtd.mtd_resultado
				AND Tpr.tpr_tipoLancamento = 2
			LEFT JOIN frequenciaExterna afx
				ON Ad.alu_id = afx.alu_id
				AND Ad.mtu_id = afx.mtu_id
				AND Mtd.mtd_id = afx.mtd_id
				AND Ad.tpc_id = afx.tpc_id
			LEFT OUTER JOIN CLS_AlunoAvaliacaoTurma Aat WITH(NOLOCK)
				ON  Aat.tur_id = Mtu.tur_id
					AND Aat.alu_id = Mtu.alu_id
					AND Aat.mtu_id = Mtu.mtu_id
					AND Aat.fav_id = Fav.fav_id
					AND Aat.ava_id = Ava.ava_id
					AND Aat.aat_situacao <> 3
			LEFT JOIN CLS_AlunoAvaliacaoTurmaDisciplina Atd WITH(NOLOCK)
				ON Atd.tud_id = Mtd.tud_id
					AND Atd.alu_id = Mtd.alu_id
					AND Atd.mtu_id = Mtd.mtu_id
					AND Atd.mtd_id = Mtd.mtd_id
					AND Atd.fav_id = Fav.fav_id
					AND Atd.ava_id = Ava.ava_id
					AND Atd.atd_situacao <> 3
	)
	, dadosAlunosTpcMax AS (
		SELECT
			Da.alu_id,
			MAX(tpc_ordem) AS maxOrdem
		FROM dadosAlunos Da
		GROUP BY Da.alu_id
	)
	, movimentacao AS (
		SELECT
			Da.alu_id,
			Da.mtu_id,
			CASE WHEN tmo_tipoMovimento IN (6, 12, 14, 15, 16)
					THEN 'TR ' + REPLACE(CONVERT(VARCHAR(10), mov.mov_dataRealizacao, 103), '/' + CAST(DATEPART(YEAR, mov.mov_dataRealizacao) AS VARCHAR(4)), '') +
						 ISNULL(' - ' + turD.tur_codigo, '')
					WHEN tmo_tipoMovimento IN (8)
					THEN 'RM' + ISNULL(' ' + turD.tur_codigo, '')
					WHEN tmo_tipoMovimento IN (11)
					THEN 'RC' + ISNULL(' ' + turD.tur_codigo, '')
					ELSE ''
			END movMsg
		FROM dadosAlunos Da
		INNER JOIN dadosAlunosTpcMax dat
			ON Da.alu_id = dat.alu_id
			AND Da.tpc_ordem = dat.maxOrdem
		INNER JOIN MTR_Movimentacao mov WITH(NOLOCK)
			ON Da.alu_id = mov.alu_id
			AND Da.mtu_id = mov.mtu_idAnterior
			AND mov.mov_situacao <> 3
		INNER JOIN MTR_TipoMovimentacao tmo WITH(NOLOCK)
			ON mov.tmo_id = tmo.tmo_id
			AND tmo_tipoMovimento IN (6, 8, 11, 12, 14, 15, 16)
			AND tmo.tmo_situacao <> 3
		LEFT JOIN MTR_MatriculaTurma mtuD WITH(NOLOCK)
			ON mov.alu_id = mtuD.alu_id
			AND mov.mtu_idAtual = mtuD.mtu_id
		LEFT JOIN TUR_Turma turD WITH(NOLOCK)
			ON mtuD.tur_id = turD.tur_id
		LEFT JOIN ACA_CalendarioAnual calD WITH(NOLOCK)
			ON turD.cal_id = calD.cal_id
		INNER JOIN MTR_MatriculaTurma mtuO WITH(NOLOCK)
			ON mov.alu_id = mtuO.alu_id
			AND mov.mtu_idAnterior = mtuO.mtu_id
			AND mtuO.tur_id = @tur_id
		LEFT JOIN TUR_Turma turO WITH(NOLOCK)
			ON mtuO.tur_id = turO.tur_id
		LEFT JOIN ACA_CalendarioAnual calO WITH(NOLOCK)
			ON turO.cal_id = calO.cal_id
		WHERE 
			turD.tur_id IS NULL OR calD.cal_ano = calO.cal_ano --Ou não tem turma destino ou a turma destino é do mesmo ano
		GROUP BY
			Da.alu_id,
			Da.mtu_id,
			tmo_tipoMovimento,
			mov.mov_dataRealizacao,
			turD.tur_codigo
	)	
	
	SELECT
		Da.alu_id, 
		Da.pes_nome +
		CASE WHEN ISNULL(mov.movMsg, '') = ''
				THEN ''
				ELSE ' (' + mov.movMsg + ')'
		END AS pes_nome,
		Da.tur_id, 
		Da.fav_id, 
		Da.tud_id, 
		Da.tud_tipo, 
		Da.mtu_resultadoDescricao, 
		Da.atd_avaliacao, 
		Da.atd_frequencia, 
		Da.atd_numeroFaltas, 
		Da.atd_numeroAulas, 
		Da.atd_ausenciasCompensadas, 
		Da.atd_frequenciaFinalAjustada, 
		Da.dis_id, 
		Da.tpc_id, 
		Da.tds_ordem, 
		Da.tpc_ordem, 
		Da.Tpc_Agrupamento, 
		Da.Tpc_exibicao, 
		Da.dis_nome, 
		Da.mtu_numeroChamada, 
		Da.mtu_resultado, 
		Da.periodoFechado, 
		CASE WHEN EXISTS(SELECT TOP 1 Da2.alu_id FROM dadosAlunosTpcMax DaT
						 INNER JOIN dadosAlunos Da2 ON DaT.alu_id = Da2.alu_id AND Dat.maxOrdem = Da2.tpc_ordem
						 WHERE Da2.alu_id = Da.alu_id AND Da2.mtu_situacao = 1)
			 THEN 1 ELSE Da.mtu_situacao END AS mtu_situacao, 
		Da.mtu_situacao AS mtu_situacaoPeriodo,
		Da.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina,
		ISNULL(CAST(Da.atd_ausenciasCompensadas AS VARCHAR),'0') AS atd_AusenciasCompensadasVC,
		ISNULL
		((
			SELECT 
				TOP 1 CAST(CAST(ISNULL(dda.atd_frequenciaFinalAjustada,100) AS DECIMAL(5,0)) AS VARCHAR)
			FROM 
				dadosAlunos dda 
			WHERE 	
				dda.alu_id = da.alu_id
				AND dda.dis_id = da.dis_id
				AND dda.tpc_ordem = 
				(
					SELECT MAX(ddaa.tpc_ordem) FROM dadosAlunos ddaa 
					WHERE 	
						ddaa.alu_id = dda.alu_id
						AND ddaa.dis_id = dda.dis_id
						AND ddaa.tpc_id IS NOT NULL
						AND ddaa.atd_frequenciaFinalAjustada IS NOT NULL
				)
		), 100) AS frequenciaFinalAjustada,
		PossuiFreqExternaAtual,
		PossuiFreqExternaAcum,
		CASE WHEN EXISTS(SELECT TOP 1 afx.alu_id FROM frequenciaExterna afx)
			 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS possuiFreqExterna
	FROM
		dadosAlunos Da
	LEFT JOIN movimentacao mov
		ON Da.alu_id = mov.alu_id
		AND Da.mtu_id = mov.mtu_id	
		
	ORDER BY 
		Da.mtu_numeroChamada,
		Da.pes_nome,
		Da.dis_nome,
		Da.Tpc_Agrupamento

END
GO
PRINT N'Creating [dbo].[NEW_ACA_EventoLimite_LoadByTipoEvento_and_Dre]'
GO
CREATE PROCEDURE [dbo].[NEW_ACA_EventoLimite_LoadByTipoEvento_and_Dre]
	@tev_id Int
	, @uad_id Uniqueidentifier
	
AS
BEGIN
	SELECT	Top 1
		 cal_id  
		, tev_id 
		, evl_id 
		, tpc_id 
		, esc_id 
		, uni_id 
		, evl_dataInicio 
		, evl_dataFim 
		, usu_id 
		, evl_situacao 
		, evl_dataCriacao 
		, evl_dataAlteracao 
		, uad_id

 	FROM
 		ACA_EventoLimite
	WHERE 
		uad_id = @uad_id
		AND tev_id = @tev_id
END
GO
PRINT N'Creating [dbo].[STP_ACA_AlunoJustificativaFaltaAnexo_DELETE]'
GO



CREATE PROCEDURE [dbo].[STP_ACA_AlunoJustificativaFaltaAnexo_DELETE]
	@alu_id BIGINT
	, @afj_id INT
	, @aja_id INT

AS
BEGIN
	DELETE FROM 
		ACA_AlunoJustificativaFaltaAnexo 
	WHERE 
		alu_id = @alu_id 
		AND afj_id = @afj_id 
		AND aja_id = @aja_id 

		
	DECLARE @ret INT
	SELECT @ret = ISNULL(@@ROWCOUNT,-1)
	RETURN @ret
	
END


GO
PRINT N'Altering [dbo].[NEW_MTR_MatriculaTurmaDisciplina_SelectBy_TurmaDisciplinaFormato]'
GO
-- Stored Procedure

-- ========================================================================
-- Author:		Carla Frascareli
-- Create date: 20/04/2011
-- Description: Retorna os alunos matriculados na Turma para o período informado,
--				de acordo com as regras necessárias para ele aparecer na listagem
--				para efetivar.

-- Alterado: Webber V. dos Santos
-- Description: Removido uso do filtro @tur_id na listagem do campo observacaoConselhoPreenchida

-- Alterado: Marcia Haga - 19/08/2014
-- Description: Adicionada validacao para retornar apenas registros ativos (situacao <> 3)
-- das tabelas CLS_AlunoAvaliacaoTurmaObservacao e CLS_AlunoAvaliacaoTurmaDisciplinaObservacao

-- Alterado: Marcia Haga - 19/09/2014
-- Description: Retornando o mtu_resultado, para marcar o check no registro do conselho 
-- de classe nos casos em que a aba do parecer conclusivo aparece no pop-up.

-- Alterado: Katiusca Murari - 06/10/2014
-- Description: Adicionada a variação na hora de calcular a frequencia.

---- Alterado: Marcia Haga - 13/03/2015
---- Description: Alterado para considerar a frequencia como 100%,
---- caso o numero de aulas previstas nao tenham sido informadas.

---- Alterado: Marcia Haga - 22/04/2015
---- Description: Alterado tratamento das aulas e faltas com pendencia para corrigir erro de registro duplicado.

---- Alterado: Marcia Haga - 10/08/2015
---- Description: Alterado para verificar o periodo em que o aluno esteve 
---- presente na turma eletiva de aluno ou multisseriada.
-- ========================================================================
ALTER PROCEDURE [dbo].[NEW_MTR_MatriculaTurmaDisciplina_SelectBy_TurmaDisciplinaFormato]
	@tud_id BIGINT
	, @tur_id BIGINT
	, @tpc_id INT
	, @ava_id INT
	, @ordenacao INT
	, @fav_id INT
	, @tipoAvaliacao TINYINT
	, @esa_id INT
	, @tipoEscalaDisciplina TINYINT
	, @tipoEscalaDocente TINYINT
	, @avaliacaoesRelacionadas NVARCHAR(MAX)
	, @notaMinimaAprovacao DECIMAL(27,4)
	, @ordemParecerMinimo INT
	, @tipoLancamento TINYINT
	, @fav_calculoQtdeAulasDadas TINYINT
	, @permiteAlterarResultado BIT
	, @tur_tipo TINYINT
	, @cal_id INT
	, @exibirNotaFinal BIT
	, @tud_tipo TINYINT
	, @tpc_ordem INT
	, @fav_variacao DECIMAL(18,3)
	, @ExibeCompensacao BIT
	, @dtTurma TipoTabela_Turma READONLY
	, @documentoOficial BIT
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL SNAPSHOT

	SET @avaliacaoesRelacionadas = CASE WHEN @avaliacaoesRelacionadas = '' THEN NULL ELSE @avaliacaoesRelacionadas END
	
	DECLARE @MatriculasBoletimDaTurma AS TipoTabela_MatriculasBoletim;

	DECLARE @Matriculas AS TipoTabela_MatriculasBoletimDisciplina;

	DECLARE @SomatorioAulasFaltas TABLE (alu_id BIGINT NOT NULL, aulas INT, faltas INT, faltasReposicao INT, compensadas INT, faltasAnteriores INT, compensadasAnteriores INT);

	DECLARE @tpc_idProximo INT = NULL;

	DECLARE @MatriculaMultisseriadaTurmaAluno TABLE 
		(
			tud_idDocente BIGINT, 
			alu_id BIGINT, 
			mtu_id INT, 
			mtd_id INT,
			tud_idAluno BIGINT
			PRIMARY KEY (tud_idDocente, alu_id, mtu_id, mtd_id)
		);

	DECLARE @tds_id INT = 
		(
			 --Busca o tipo de disciplina para filtrar os mtds abaixo.
			SELECT Dis.tds_id
			FROM TUR_TurmaDisciplinaRelDisciplina RelDis -- WITH (NOLOCK)
			INNER JOIN ACA_Disciplina Dis -- WITH (NOLOCK)
				ON Dis.dis_id = RelDis.dis_id
			WHERE
				RelDis.tud_id = @tud_id
		)

	 --Se for turma de eletiva do aluno, carrega os alunos que devem aparecer na tela de efetivação
	IF ( @tur_tipo IN (2,3) ) BEGIN
		-- Turma eletiva de aluno ou multisseriada, buscar matrículas por aluno.
		DECLARE @tbMatriculaAlunos TipoTabela_AlunoMatriculaTurma;
		INSERT INTO @tbMatriculaAlunos (alu_id, mtu_id)
		SELECT Mtd.alu_id, MAX(Mtd.mtu_id)
		FROM MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
		WHERE
			-- Busca mtus dos alunos pelo MTD - o mtd está ligado ao tud informado, e o mtu_id aponta pra outra turma (turma normal).
			Mtd.tud_id = @tud_id
			AND Mtd.mtd_situacao <> 3
		GROUP BY alu_id	

		INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
		Select mb.alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mb.mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes 
		  from MTR_MatriculasBoletim mb  -- WITH (NOLOCK)
			   inner join @tbMatriculaAlunos amt on amt.alu_id = mb.alu_id and amt.mtu_id = mb.mtu_origemDados

		IF not exists (select top 1 alu_id from @MatriculasBoletimDaTurma)
		begin
			INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
				PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
				PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
			EXEC NEW_MTR_MatriculaTurma_MatriculasBoletim_Alunos
				@tbMatriculaTurma = @tbMatriculaAlunos
		end
	END
	ELSE IF (@tur_tipo = 4)
	BEGIN
		-- Turma eletiva de aluno ou multisseriada, buscar matrículas por aluno.
		DECLARE @tbMatriculaAlunosMultisseriada TipoTabela_AlunoMatriculaTurma;
		INSERT INTO @tbMatriculaAlunosMultisseriada (alu_id, mtu_id)
		SELECT Mtd.alu_id, MAX(Mtd.mtu_id)
		FROM MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
		INNER JOIN MTR_MatriculaTurma mtu
			ON Mtd.alu_id = mtu.alu_id
			AND Mtd.mtu_id = mtu.mtu_id
			AND mtu.mtu_situacao <> 3
		INNER JOIN @dtTurma dtt
			ON mtu.tur_id = dtt.tur_id
		WHERE
			-- Busca mtus dos alunos pelo MTD - o mtd está ligado ao tud informado, e o mtu_id aponta pra outra turma (turma normal).
			Mtd.tud_id = @tud_id
			AND Mtd.mtd_situacao <> 3
		GROUP BY mtd.alu_id	

		INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
		Select mb.alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mb.mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes 
		  from MTR_MatriculasBoletim mb  -- WITH (NOLOCK)
			   inner join @tbMatriculaAlunosMultisseriada amt on amt.alu_id = mb.alu_id and amt.mtu_id = mb.mtu_origemDados

		IF not exists (select top 1 alu_id from @MatriculasBoletimDaTurma)
		begin
			INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
				PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
				PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
			EXEC NEW_MTR_MatriculaTurma_MatriculasBoletim_Alunos
				@tbMatriculaTurma = @tbMatriculaAlunosMultisseriada
		end

		INSERT INTO @MatriculaMultisseriadaTurmaAluno (tud_idDocente, alu_id, mtu_id, mtd_id, tud_idAluno)
		SELECT 
			mtdDocente.tud_id AS tud_idDocente,
			mtdAluno.alu_id,
			mtdAluno.mtu_id,
			mtdAluno.mtd_id,
			mtdAluno.tud_id AS tud_idAluno
		FROM @MatriculasBoletimDaTurma mtr
		INNER JOIN MTR_MatriculaTurmaDisciplina mtdDocente
			ON mtr.alu_id = mtdDocente.alu_id
			AND mtr.mtu_id = mtdDocente.mtu_id
			AND mtdDocente.tud_id = @tud_id
			AND mtdDocente.mtd_situacao <> 3
		INNER JOIN MTR_MatriculaTurmaDisciplina mtdAluno
			ON mtdAluno.alu_id = mtr.alu_id
			AND mtdAluno.mtu_id = mtr.mtu_id
			AND mtdAluno.mtd_situacao <> 3
		INNER JOIN TUR_TurmaDisciplina tudAluno
			ON mtdAluno.tud_id = tudAluno.tud_id
			AND tudAluno.tud_id <> @tud_id
			AND tudAluno.tud_tipo = 16
			AND tudAluno.tud_situacao <> 3
		INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDisAluno
			ON RelDisAluno.tud_id = tudAluno.tud_id
		INNER JOIN ACA_Disciplina disAluno
			ON RelDisAluno.dis_id = disAluno.dis_id
			AND disAluno.tds_id = @tds_id
			AND disAluno.dis_situacao <> 3
		GROUP BY
			mtdDocente.tud_id,
			mtdAluno.alu_id,
			mtdAluno.mtu_id,
			mtdAluno.mtd_id,
			mtdAluno.tud_id
	END
	 --Se for turma normal, carrega os alunos de acordo com o boletim
	ELSE
	BEGIN
		INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
			PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
			PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
		select mb.alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mb.mtu_id,mb.tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
		PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mb.mtu_numeroChamada,PossuiSaidaPeriodo,
		PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes
		  from MTR_MatriculasBoletim mb
			   inner join (select alu_id, mtu_id, ROW_NUMBER() OVER(PARTITION BY alu_id 
														   ORDER BY mtu_id desc) as linha
							 from MTR_MatriculaTurma -- WITH (NOLOCK) 
							where mtu_situacao <> 3
							  and tur_id = @tur_id) mtu 
					   on mtu.alu_id = mb.alu_id and mtu.mtu_id = mb.mtu_origemDados
		 where mtu.linha = 1 --para evitar casos onde o aluno tem dois mtus para o mesmo tur_id

		IF not exists (select top 1 alu_id from @MatriculasBoletimDaTurma)
		begin
			INSERT INTO @MatriculasBoletimDaTurma (alu_id,cal_ano,cal_id,cap_id,tpc_id,tpc_ordem,mtu_id,tur_id,tur_codigo,fav_id,tpc_nome,PermiteConceitoGlobal,
				PermiteDisciplinas,PeriodosEquivalentes,MesmoCalendario,MesmoFormato,MesmaEscola,esc_id,mtu_numeroChamada,PossuiSaidaPeriodo,
				PossuiEntradaPeriodo,mov_id,mov_frequencia,registroExterno,EntradaImportacaoSCA,EntradaTransfOutrasRedes)
			EXEC NEW_MTR_MatriculaTurma_MatriculasBoletim_DaTurma
				@tur_id = @tur_id;
		end
	END

	IF (@tur_tipo = 4)
	BEGIN
		INSERT INTO @Matriculas 
		(
			alu_id, 
			mtu_id, 
			mtd_id, 
			fav_id, 
			tpc_id, 
			tpc_ordem, 
			tud_id, 
			tur_id, 
			registroExterno, 
			PossuiSaidaPeriodo, 
			variacaoFrequencia, 
			mtd_numeroChamadaDocente,
			mtd_situacaoDocente, 
			mtd_dataMatriculaDocente, 
			mtd_dataSaidaDocente
		)
		SELECT
			Mtr.alu_id
			, Mtr.mtu_id
			, Mtd.mtd_id
			, Mtr.fav_id
			, Mtr.tpc_id
			, Mtr.tpc_ordem
			, mtd.tud_id
			, Mtr.tur_id
			, Mtr.registroExterno
			, Mtr.PossuiSaidaPeriodo
			, FAV.fav_variacao
			, mtdDocente.mtd_numeroChamada
			, mtdDocente.mtd_situacao
			, mtdDocente.mtd_dataMatricula
			, mtdDocente.mtd_dataSaida
		FROM @MatriculasBoletimDaTurma Mtr
		INNER JOIN MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
			ON Mtd.alu_id = Mtr.alu_id
			AND Mtd.mtu_id = Mtr.mtu_id
			AND Mtd.mtd_situacao <> 3
		INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDis -- WITH (NOLOCK)
			ON RelDis.tud_id = Mtd.tud_id
		INNER JOIN ACA_Disciplina Dis -- WITH (NOLOCK)
			ON RelDis.dis_id = Dis.dis_id
		INNER JOIN dbo.ACA_FormatoAvaliacao FAV -- WITH (NOLOCK)
			ON	FAV.fav_id = Mtr.fav_id
			AND FAV.fav_situacao <> 3
		INNER JOIN @MatriculaMultisseriadaTurmaAluno tdm 
			ON Mtd.alu_id = tdm.alu_id
			AND Mtd.mtu_id = tdm.mtu_id
			AND Mtd.mtd_id = tdm.mtd_id
		INNER JOIN MTR_MatriculaTurmaDisciplina mtdDocente
			ON mtdDocente.alu_id = Mtd.alu_id
			AND mtdDocente.mtu_id = Mtd.mtu_id
			AND mtdDocente.tud_id = tdm.tud_idDocente
			AND mtdDocente.mtd_situacao <> 3
		WHERE
			Mtr.mtu_id IS NOT NULL
			 --Busca a matrícula na mtd ligada à disciplina (pelo tds_id), em cada COC, da lista de mtus (MatriculasBoletim).
			AND Dis.tds_id = @tds_id
			 --Filtros de matrícula.
			AND Mtr.MesmoCalendario = 1
			--AND Mtr.PeriodosEquivalentes = 1
		GROUP BY
			Mtr.alu_id
			, Mtr.mtu_id
			, Mtd.mtd_id
			, Mtr.fav_id
			, Mtr.tpc_id
			, Mtr.tpc_ordem
			, mtd.tud_id
			, Mtr.tur_id
			, Mtr.registroExterno
			, Mtr.PossuiSaidaPeriodo
			, FAV.fav_variacao
			, mtdDocente.mtd_numeroChamada
			, mtdDocente.mtd_situacao
			, mtdDocente.mtd_dataMatricula
			, mtdDocente.mtd_dataSaida
	END
	ELSE
	BEGIN
		INSERT INTO @Matriculas (alu_id, mtu_id, mtd_id, fav_id, tpc_id, tpc_ordem, tud_id, tur_id, registroExterno, PossuiSaidaPeriodo, variacaoFrequencia)
		SELECT
			Mtr.alu_id
			, Mtr.mtu_id
			, Mtd.mtd_id
			, Mtr.fav_id
			, Mtr.tpc_id
			, Mtr.tpc_ordem
			, Mtd.tud_id
			, Mtr.tur_id
			, Mtr.registroExterno
			, Mtr.PossuiSaidaPeriodo
			, FAV.fav_variacao
		FROM @MatriculasBoletimDaTurma Mtr
		INNER JOIN MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
			ON Mtd.alu_id = Mtr.alu_id
			AND Mtd.mtu_id = Mtr.mtu_id
			AND Mtd.mtd_situacao <> 3
		INNER JOIN TUR_TurmaDisciplinaRelDisciplina RelDis -- WITH (NOLOCK)
			ON RelDis.tud_id = Mtd.tud_id
		INNER JOIN ACA_Disciplina Dis -- WITH (NOLOCK)
			ON RelDis.dis_id = Dis.dis_id
		INNER JOIN dbo.ACA_FormatoAvaliacao FAV -- WITH (NOLOCK)
			ON	FAV.fav_id = Mtr.fav_id
			AND FAV.fav_situacao <> 3
		WHERE
			Mtr.mtu_id IS NOT NULL
			 --Busca a matrícula na mtd ligada à disciplina (pelo tds_id), em cada COC, da lista de mtus (MatriculasBoletim).
			AND Dis.tds_id = @tds_id
			 --Filtros de matrícula.
			AND Mtr.MesmoCalendario = 1
			--Verifica períodos equivalentes apenas para turmas normais (1)
			AND (Mtr.PeriodosEquivalentes = 1 OR @tur_tipo <> 1)
    END

	-- Verifica o periodo em que o aluno esteve presente na turma eletiva de aluno ou multisseriada
	IF ( @tur_tipo IN (2,3,4) ) 
	BEGIN
		;WITH PresencaAlunoPeriodo AS
		(
			SELECT Mat.alu_id, Mat.mtu_id, Mat.mtd_id, Mat.tpc_id 
			FROM @Matriculas Mat
			INNER JOIN TUR_Turma Tur -- WITH (NOLOCK)
				ON Tur.tur_id = Mat.tur_id
			INNER JOIN MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
				ON Mtd.alu_id = Mat.alu_id
				AND Mtd.mtu_id = Mat.mtu_id
				AND Mtd.mtd_id = Mat.mtd_id
			INNER JOIN ACA_TipoPeriodoCalendario Tpc -- WITH (NOLOCK)
				ON Tpc.tpc_id = Mat.tpc_id
			INNER JOIN ACA_CalendarioPeriodo Cap -- WITH (NOLOCK)
				ON Cap.tpc_id = Tpc.tpc_id
				AND Cap.cal_id = Tur.cal_id
				AND Cap.cap_situacao <> 3
			WHERE
			(
				-- O aluno nao estava presente no periodo se:
				-- o aluno saiu durante o periodo
				Mtd.mtd_dataSaida BETWEEN Cap.cap_dataInicio AND Cap.cap_dataFim
				-- ou o aluno saiu antes de o periodo iniciar
				OR Mtd.mtd_dataSaida < Cap.cap_dataInicio
				-- ou o aluno entrou depois do fim do periodo
				OR Mtd.mtd_dataMatricula > Cap.cap_dataFim
			)
			AND Mat.PossuiSaidaPeriodo = 0
		)
		UPDATE @Matriculas
		SET PossuiSaidaPeriodo = 1
		FROM @Matriculas Mat
		INNER JOIN PresencaAlunoPeriodo Pap
			ON Pap.alu_id = Mat.alu_id
			AND Pap.mtu_id = Mat.mtu_id
			AND Pap.mtd_id = Mat.mtd_id
			AND Pap.tpc_id = Mat.tpc_id
	END 

	-- Armazenar médias dos alunos
	DECLARE @tbMediasAlunos TABLE (alu_id BIGINT, mtu_id INT, Media VARCHAR(20))

	-- Parâmetro que define se aparece o campo média final na tela de avaliações (caso positivo, traz a nota informada lá,
	-- caso negativo calcula a média).
	IF (@exibirNotaFinal = 0) 
	BEGIN
		IF (@tipoEscalaDocente = 2) --pareceres
		BEGIN
			INSERT INTO @tbMediasAlunos (alu_id, mtu_id, Media)
			SELECT alu_id, mtu_id, Media
			FROM FN_CalculaConceitoAluno_Por_DisciplinaPeriodo_TodosAlunos(@tpc_id, @tud_id)
		END
		ELSE
		BEGIN
			DECLARE @fav_calcularMediaAvaliacaoFinal BIT = (SELECT fav_calcularMediaAvaliacaoFinal FROM ACA_FormatoAvaliacao WHERE fav_id = @fav_id);
			-- Caso esteja marcado para calcular a média da avaliação final.
			IF (@tipoAvaliacao = 3 AND @fav_calcularMediaAvaliacaoFinal = 1) -- 3-Final
			BEGIN
				--Calcular média dos alunos.
				INSERT INTO @tbMediasAlunos (alu_id, mtu_id, Media)
				EXEC dbo.NEW_CLS_AlunoAvaliacaoTurmaDisciplina_MediasFinaisPor_PesoAvaliacoes 
					@fav_id = @fav_id, @MatriculasBoletimDisciplina = @Matriculas
			END
			ELSE
			BEGIN
				--Calcular média dos alunos.
				INSERT INTO @tbMediasAlunos (alu_id, mtu_id, Media)
				SELECT alu_id, mtu_id, Media
				FROM FN_CalculaMediaAluno_Por_DisciplinaPeriodo_TodosAlunos(@tpc_id, @tud_id, @fav_id)
			END
		END
	END
	ELSE
	BEGIN
			--Buscar as médias salvas na tabela de Notas finais.
		INSERT INTO @tbMediasAlunos (alu_id, mtu_id, Media)
		SELECT Atm.alu_id, Atm.mtu_id, Atm.atm_media
		FROM CLS_AlunoAvaliacaoTurmaDisciplinaMedia Atm -- WITH (NOLOCK)
		WHERE
			Atm.tud_id = @tud_id
			AND Atm.tpc_id = @tpc_id
			AND Atm.atm_situacao <> 3
	END

	IF (@tpc_id IS NULL)
	BEGIN

		 --Seleciona o próximo período da avaliação periódica relacionada à avliação (recuperação).
		 --Por exemplo: se a avaliação de recuperação é a Rec. do 3º COC, vai usar a matrícula que
			--			deve ser efetivada no 4º COC, pois a recuperação sempre acontece no coc seguinte.
		SET @tpc_idProximo = (  SELECT 
		                            TOP 1 TpcProximo.tpc_id
		                        FROM
		                            ACA_Avaliacao                        AvaPeriodica   -- WITH (NOLOCK)
		                            INNER JOIN ACA_TipoPeriodoCalendario Tpc            -- WITH (NOLOCK)
		                            ON  Tpc.tpc_id = AvaPeriodica.tpc_id
		                            INNER JOIN ACA_TipoPeriodoCalendario TpcProximo     -- WITH (NOLOCK)
		                            ON  TpcProximo.tpc_ordem    =  (Tpc.tpc_ordem + 1) AND 
			                                                                                                TpcProximo.tpc_situacao <> 3
		                        WHERE
									AvaPeriodica.fav_id = @fav_id
		                            AND AvaPeriodica.ava_id IN (SELECT valor FROM FN_StringToArrayInt32(ISNULL(@avaliacaoesRelacionadas,0), ','))		                            
		                        ORDER BY 
		                            Tpc.tpc_ordem DESC
		                     );
	END

	 --Todos os alunos dessas matrículas que estão de recuperação (de acordo com os ava_ids Relacionados).
	DECLARE @notasPeriodicasRecuperacao TABLE (
	      alu_id        BIGINT
	    , mtu_id        INT
	    , mtd_id        INT
	    , recuperacao   BIT
	);

	 --Recuperacao = 2. Insere na tabela as notas da avaliação relacionada à recuperação.
	IF (@tipoAvaliacao = 2)
	BEGIN

		INSERT INTO @notasPeriodicasRecuperacao (alu_id, mtu_id, mtd_id, recuperacao)
		SELECT
			  Mtr.alu_id
			, Mtr.mtu_id
			, Mtr.mtd_id
			, 1
		FROM @Matriculas Mtr
		 --Mtu responsável no período anterior ao que é responsável na avaliação de recuperação.
		INNER JOIN @Matriculas MtrAnterior 
			ON      MtrAnterior.alu_id    = Mtr.alu_id
			    AND MtrAnterior.tpc_ordem = (Mtr.tpc_ordem - 1)
		INNER JOIN CLS_AlunoAvaliacaoTurmaDisciplina Atd -- WITH (NOLOCK)
			ON      Atd.tud_id = MtrAnterior.tud_id
			    AND Atd.alu_id = MtrAnterior.alu_id
			    AND Atd.mtu_id = MtrAnterior.mtu_id
			    AND Atd.mtd_id = MtrAnterior.mtd_id
			    AND Atd.fav_id = @fav_id
			    AND Atd.ava_id IN (SELECT valor FROM FN_StringToArrayInt32(ISNULL(@avaliacaoesRelacionadas,0), ','))
			    AND Atd.atd_situacao <> 3
		INNER JOIN ACA_Avaliacao Ava -- WITH (NOLOCK)
			ON      Ava.fav_id = Atd.fav_id
			    AND Ava.ava_id = Atd.ava_id
			    AND Ava.ava_situacao <> 3
			     --Filtrar avaliações ligadas ao período anterior.
			    AND Ava.tpc_id = MtrAnterior.tpc_id
			WHERE
				(
					(@tipoEscalaDisciplina = 1 AND 
						CAST(ISNULL(REPLACE(ISNULL(atd.atd_avaliacaoPosConselho, atd.atd_avaliacao),',','.'),0) AS DECIMAL(27,4)) < @notaMinimaAprovacao )
					OR
					(@tipoEscalaDisciplina = 2 AND 
						CAST(ISNULL((SELECT ordem FROM FN_ACA_EscalaAvaliacaoParecer_RetornarOrdem(@esa_id, ISNULL(atd.atd_avaliacaoPosConselho, atd.atd_avaliacao))),0) AS INT) < @ordemParecerMinimo)
				)
				AND ISNULL(Atd.atd_semProfessor,0) = 0
	END

	DECLARE @TabelaFaltasAulas TABLE
	(
		alu_id BIGINT NOT NULL
		, mtu_id INT NOT NULL
		, mtd_id INT NOT NULL
		, qtFaltas INT NULL
		, qtAulas INT NULL
		, qtFaltasReposicao INT NULL
		, PRIMARY KEY (alu_id, mtu_id, mtd_id)
	)

	IF (@tur_tipo = 4)
	BEGIN
		INSERT INTO @TabelaFaltasAulas
		(
			alu_id
			, mtu_id 
			, mtd_id 
			, qtFaltas
			, qtAulas 
			, qtFaltasReposicao
		)
		SELECT
			tdm.alu_id
			, tdm.mtu_id 
			, tdm.mtd_id 
			, SUM(qtFaltas) OVER (PARTITION BY tdm.alu_id, tdm.mtu_id, tdm.tud_idDocente) AS qtFaltas
			, SUM(qtAulas)  OVER (PARTITION BY tdm.alu_id, tdm.mtu_id, tdm.tud_idDocente) AS qtAulas
			, SUM(qtFaltasReposicao) OVER (PARTITION BY tdm.alu_id, tdm.mtu_id, tdm.tud_idDocente) AS qtFaltasReposicao
		FROM
			@MatriculaMultisseriadaTurmaAluno tdm
			CROSS APPLY FN_Select_FaltasAulasBy_TurmaDisciplina(@tipoLancamento, @tpc_id, tdm.tud_idDocente, @fav_calculoQtdeAulasDadas, DEFAULT) Qtd
			WHERE Qtd.alu_id = tdm.alu_id
				AND Qtd.mtu_id = tdm.mtu_id
				AND Qtd.mtd_id = tdm.mtd_id
	END
	ELSE
	BEGIN
		INSERT INTO @TabelaFaltasAulas
		(
			alu_id
			, mtu_id 
			, mtd_id 
			, qtFaltas
			, qtAulas 
			, qtFaltasReposicao
		)
		SELECT
			alu_id
			, mtu_id 
			, mtd_id 
			, qtFaltas
			, qtAulas 
			, qtFaltasReposicao 
		FROM
			FN_Select_FaltasAulasBy_TurmaDisciplina(@tipoLancamento, @tpc_id, @tud_id, @fav_calculoQtdeAulasDadas, DEFAULT) Qtd
	END

	IF (@tud_tipo = 15)
	BEGIN
		;WITH TabelaPeriodosAnteriores AS (
		SELECT 
			tpc.tpc_id, 
			ava.ava_id, 
			ava.fav_id 
		FROM dbo.ACA_TipoPeriodoCalendario AS tpc -- WITH (NOLOCK) 
		INNER JOIN dbo.ACA_Avaliacao AS ava -- WITH (NOLOCK)
			ON ava.fav_id=@fav_id
			AND tpc.tpc_id = ava.tpc_id	
			AND ava.ava_situacao<>3	
		WHERE tpc_ordem <= (SELECT tpc_ordem FROM dbo.ACA_TipoPeriodoCalendario -- WITH (NOLOCK)
		                     WHERE tpc_id=@tpc_id)
		)

		, Compensacoes AS 
		(
			-- Trazer as compensações de cada bimestre agrupadas, para trazer um registro único por bimestre.
			SELECT
				mat.alu_id, mat.mtu_id, mat.mtd_id, mat.tpc_id
				, SUM(ISNULL(cpa.cpa_quantidadeAulasCompensadas, 0)) AS cpa_quantidadeAulasCompensadas
			FROM @Matriculas AS mat
			INNER JOIN CLS_CompensacaoAusencia cpa -- WITH (NOLOCK)
			ON cpa.tud_id = @tud_id
				AND mat.tpc_id=cpa.tpc_id
				AND cpa.cpa_situacao = 1
			INNER JOIN CLS_CompensacaoAusenciaAluno caa -- WITH (NOLOCK)
				ON  caa.tud_id = cpa.tud_id
					AND caa.cpa_id = cpa.cpa_id
					AND caa.caa_situacao = 1
					AND caa.alu_id=mat.alu_id
			GROUP BY mat.alu_id, mat.mtu_id, mat.mtd_id, mat.tpc_id
		)

		INSERT INTO @SomatorioAulasFaltas (alu_id, faltas, faltasReposicao, aulas, compensadas, faltasAnteriores, compensadasAnteriores)
		SELECT 
			mat.alu_id,

			SUM(CASE WHEN @tpc_id=mat.tpc_id
						THEN COALESCE(atd.atd_numeroFaltas,qtfaltas,0)
					ELSE  ISNULL(atd.atd_numeroFaltas,0)
				END) AS faltas,
			ISNULL(qtFaltasReposicao,0) AS faltasReposicao,
			SUM(CASE WHEN @tpc_id=mat.tpc_id
						THEN COALESCE(atd.atd_numeroAulas,qtAulas,0)
					ELSE  ISNULL(atd.atd_numeroAulas,0)
				END) AS aulas,	
			SUM(CASE WHEN @tpc_id <> mat.tpc_id 
				THEN ISNULL(atd.atd_ausenciasCompensadas,0) 
				ELSE ( 
						CASE WHEN Atd.atd_id IS NOT NULL 
						THEN ISNULL(atd.atd_ausenciasCompensadas, 0) 
						ELSE ISNULL(cpa.cpa_quantidadeAulasCompensadas,0) 
						END 
					 ) 
				END) AS compensadas,
			SUM(CASE WHEN @tpc_id=mat.tpc_id
						THEN 0
					ELSE  ISNULL(atd.atd_numeroFaltas,0)
				END) + ISNULL(qtFaltasReposicao,0) AS faltasAnteriores,
			SUM(CASE WHEN @tpc_id <> mat.tpc_id 
				THEN ISNULL(atd.atd_ausenciasCompensadas,0) 
				ELSE 0
				END) AS compensadasAnteriores
		FROM @Matriculas AS mat
			INNER JOIN TabelaPeriodosAnteriores tpa
				ON tpa.tpc_id=mat.tpc_id	
			LEFT JOIN CLS_AlunoAvaliacaoTurmaDisciplina Atd -- WITH (NOLOCK)
				ON  Atd.tud_id = mat.tud_id
					AND Atd.alu_id = mat.alu_id
					AND Atd.mtu_id = mat.mtu_id
					AND Atd.mtd_id = mat.mtd_id
					AND atd.fav_id=tpa.fav_id
					AND atd.ava_id=tpa.ava_id
					AND Atd.atd_situacao <> 3
			LEFT JOIN @TabelaFaltasAulas tfa 
				ON  mat.alu_id = tfa.alu_id
			LEFT JOIN Compensacoes Cpa
				ON Cpa.alu_id = mat.alu_id
				AND Cpa.mtu_id = mat.mtu_id
				AND Cpa.mtd_id = mat.mtd_id
				AND Cpa.tpc_id = mat.tpc_id
		   GROUP BY mat.alu_id, qtFaltasReposicao
	END
	ELSE 
	BEGIN
		;WITH TabelaPeriodosAnteriores AS (
			SELECT 
				tpc.tpc_id, 
				ava.ava_id, 
				ava.fav_id 
			FROM dbo.ACA_TipoPeriodoCalendario AS tpc -- WITH (NOLOCK) 
			INNER JOIN dbo.ACA_Avaliacao AS ava -- WITH (NOLOCK)
				ON ava.fav_id=@fav_id
				AND tpc.tpc_id = ava.tpc_id	
				AND ava.ava_situacao<>3	
			WHERE tpc_ordem <= (SELECT tpc_ordem FROM dbo.ACA_TipoPeriodoCalendario -- WITH (NOLOCK)
			                     WHERE tpc_id=@tpc_id)
		)

		, Compensacoes AS 
		(
			-- Trazer as compensações de cada bimestre agrupadas, para trazer um registro único por bimestre.
			SELECT
				mat.alu_id, mat.mtu_id, mat.mtd_id, mat.tpc_id
				, SUM(ISNULL(cpa.cpa_quantidadeAulasCompensadas, 0)) AS cpa_quantidadeAulasCompensadas
			FROM @Matriculas AS mat
			INNER JOIN CLS_CompensacaoAusencia cpa -- WITH (NOLOCK)
			ON cpa.tud_id = @tud_id
				AND mat.tpc_id=cpa.tpc_id
				AND cpa.cpa_situacao = 1
			INNER JOIN CLS_CompensacaoAusenciaAluno caa -- WITH (NOLOCK)
				ON  caa.tud_id = cpa.tud_id
					AND caa.cpa_id = cpa.cpa_id
					AND caa.caa_situacao = 1
					AND caa.alu_id=mat.alu_id
			GROUP BY mat.alu_id, mat.mtu_id, mat.mtd_id, mat.tpc_id
		)

		INSERT INTO @SomatorioAulasFaltas (alu_id, faltas, faltasReposicao, aulas, compensadas, faltasAnteriores, compensadasAnteriores)
		SELECT 
			mat.alu_id,
			SUM(CASE WHEN @tpc_id=mat.tpc_id
						THEN COALESCE(atd.atd_numeroFaltas,qtfaltas,0)
					ELSE  ISNULL(atd.atd_numeroFaltas,0)
				END) AS faltas,
			ISNULL(qtFaltasReposicao,0) AS faltasReposicao, 
			SUM(CASE WHEN @tpc_id=mat.tpc_id
						THEN COALESCE(atd.atd_numeroAulas,qtAulas,0)
					ELSE  ISNULL(atd.atd_numeroAulas,0)
				END) AS aulas,	
			SUM(CASE WHEN @tpc_id <> mat.tpc_id 
				THEN ISNULL(atd.atd_ausenciasCompensadas,0) 
				ELSE ( 
						CASE WHEN Atd.atd_id IS NOT NULL 
						THEN ISNULL(atd.atd_ausenciasCompensadas, 0) 
						ELSE ISNULL(cpa.cpa_quantidadeAulasCompensadas,0) 
						END 
					 ) 
				END) AS compensadas,
			SUM(CASE WHEN @tpc_id=mat.tpc_id
						THEN 0
					ELSE  ISNULL(atd.atd_numeroFaltas,0)
				END) + ISNULL(qtFaltasReposicao,0) AS faltasAnteriores,
			SUM(CASE WHEN @tpc_id <> mat.tpc_id 
				THEN ISNULL(atd.atd_ausenciasCompensadas,0) 
				ELSE 0
				END) AS compensadasAnteriores
		FROM @Matriculas AS mat
			INNER JOIN TabelaPeriodosAnteriores tpa
				ON tpa.tpc_id=mat.tpc_id	
			LEFT JOIN CLS_AlunoAvaliacaoTurmaDisciplina Atd -- WITH (NOLOCK)
				ON  Atd.tud_id = mat.tud_id
					AND Atd.alu_id = mat.alu_id
					AND Atd.mtu_id = mat.mtu_id
					AND Atd.mtd_id = mat.mtd_id
					AND atd.fav_id=tpa.fav_id
					AND atd.ava_id=tpa.ava_id
					AND Atd.atd_situacao <> 3
			LEFT JOIN @TabelaFaltasAulas tfa 
				ON  mat.alu_id = tfa.alu_id
			LEFT JOIN Compensacoes Cpa
				ON Cpa.alu_id = mat.alu_id
				AND Cpa.mtu_id = mat.mtu_id
				AND Cpa.mtd_id = mat.mtd_id
				AND Cpa.tpc_id = mat.tpc_id
		   GROUP BY mat.alu_id, qtFaltasReposicao
	END

	/* [Carla 19/07/2013]
		Excluir os registros que não devem ser exibidos na tela. Esses registros devem ser trazidos para buscar
		a nota do bimestre anterior, caso seja recuperação.
	*/
	DELETE FROM @Matriculas
	WHERE
		registroExterno = 1
		-- Se possuir uma saída no período, não exibe o aluno.
		OR PossuiSaidaPeriodo = 1
		-- Excluir matrículas de outras turmas, pois traz todos os bimestres pra fazer os acumulados.
		OR (@tur_tipo = 1 AND tur_id <> @tur_id)
		-- Excluir matrículas de outras disciplinas em turmas eletivas/multisseriadas.
		OR (@tur_tipo IN (2,3) AND tud_id <> @tud_id)

	; WITH TabelaMovimentacao AS (

			--Selecina as movimentações que possuem matrícula anterior
			SELECT
				alu_id,
				mtu_idAnterior,
				tmv_nome    
			FROM
				MTR_Movimentacao MOV -- WITH (NOLOCK) 
				INNER JOIN ACA_TipoMovimentacao TMV -- WITH (NOLOCK) 
					ON MOV.tmv_idSaida = TMV.tmv_id
			WHERE
				mov_situacao NOT IN (3,4)
				AND tmv_situacao <> 3
				AND mtu_idAnterior IS NOT NULL	
	), 

	TabelaObservacaoDisciplina AS 
	(
		SELECT
			tud_id
			, alu_id
			, mtu_id
			, mtd_id
			, CASE WHEN qtdeQualidade = 0 AND qtdeDesempenhos = 0 AND qtdeRecomendacao = 0
						AND ado_qualidade IS NULL AND ado_desempenhoAprendizado IS NULL 
						AND ado_recomendacaoAluno IS NULL AND ado_recomendacaoResponsavel IS NULL
				   THEN 0
				   ELSE 1
			  END AS observacaoPreenchida
		FROM
		(
			SELECT 
				Mtr.tud_id
				, Mtr.alu_id
				, Mtr.mtu_id
				, Mtr.mtd_id
				, SUM(CASE WHEN aaq.tqa_id IS NULL THEN 0 ELSE 1 END) AS qtdeQualidade
				, SUM(CASE WHEN aad.tda_id IS NULL THEN 0 ELSE 1 END) AS qtdeDesempenhos
				, SUM(CASE WHEN aar.rar_id IS NULL THEN 0 ELSE 1 END) AS qtdeRecomendacao
				, ado_qualidade
				, ado_desempenhoAprendizado
				, ado_recomendacaoAluno
				, ado_recomendacaoResponsavel
			FROM
				@Matriculas Mtr
				INNER JOIN ACA_Avaliacao ava -- WITH (NOLOCK)
					ON Mtr.fav_id = ava.fav_id
					AND ava.ava_id = @ava_id
					AND ava.ava_exibeObservacaoDisciplina = 1
				LEFT JOIN CLS_AlunoAvaliacaoTurmaDisciplinaQualidade aaq -- WITH (NOLOCK)
					ON  Mtr.tud_id = aaq.tud_id
					AND Mtr.alu_id = aaq.alu_id
					AND Mtr.mtu_id = aaq.mtu_id
					AND Mtr.mtd_id = aaq.mtd_id
					AND aaq.fav_id = ava.fav_id
					AND aaq.ava_id = ava.ava_id

				LEFT JOIN CLS_AlunoAvaliacaoTurmaDisciplinaDesempenho aad -- WITH (NOLOCK)
					ON  Mtr.tud_id = aad.tud_id
					AND Mtr.alu_id = aad.alu_id
					AND Mtr.mtu_id = aad.mtu_id
					AND Mtr.mtd_id = aad.mtd_id
					AND aad.fav_id = ava.fav_id
					AND aad.ava_id = ava.ava_id  

				LEFT JOIN CLS_AlunoAvaliacaoTurmaDisciplinaRecomendacao aar -- WITH (NOLOCK)
					ON  Mtr.tud_id = aar.tud_id
					AND Mtr.alu_id = aar.alu_id
					AND Mtr.mtu_id = aar.mtu_id
					AND Mtr.mtd_id = aar.mtd_id
					AND aar.fav_id = ava.fav_id
					AND aar.ava_id = ava.ava_id

				LEFT JOIN CLS_ALunoAvaliacaoTurmaDisciplinaObservacao ado -- WITH (NOLOCK)
					ON  Mtr.tud_id = ado.tud_id
					AND Mtr.alu_id = ado.alu_id
					AND Mtr.mtu_id = ado.mtu_id
					AND Mtr.mtd_id = ado.mtd_id
					AND ado.fav_id = ava.fav_id
					AND ado.ava_id = ava.ava_id
					AND ado.ado_situacao <> 3

			GROUP BY
				Mtr.tud_id
				, Mtr.alu_id
				, Mtr.mtu_id
				, Mtr.mtd_id
				, ado_qualidade
				, ado_desempenhoAprendizado
				, ado_recomendacaoAluno
				, ado_recomendacaoResponsavel
		) AS tabela 
	),

	TabelaObservacaoConselho AS 
	(
		SELECT
			tur_id
			, alu_id
			, mtu_id
			, CASE WHEN qtdeQualidade = 0 AND qtdeDesempenhos = 0 AND qtdeRecomendacao = 0
						AND ato_qualidade IS NULL AND ato_desempenhoAprendizado IS NULL 
						AND ato_recomendacaoAluno IS NULL AND ato_recomendacaoResponsavel IS NULL
				   THEN 0
				   ELSE 1
			  END AS observacaoPreenchida
		FROM
		(
			SELECT
				Mtr.tur_id
				, Mtr.alu_id
				, Mtr.mtu_id
				, SUM(CASE WHEN aaq.tqa_id IS NULL THEN 0 ELSE 1 END) AS qtdeQualidade
				, SUM(CASE WHEN aad.tda_id IS NULL THEN 0 ELSE 1 END) AS qtdeDesempenhos
				, SUM(CASE WHEN aar.rar_id IS NULL THEN 0 ELSE 1 END) AS qtdeRecomendacao
				, ato_qualidade
				, ato_desempenhoAprendizado
				, ato_recomendacaoAluno
				, ato_recomendacaoResponsavel
			FROM
				@Matriculas Mtr
				INNER JOIN ACA_Avaliacao ava -- WITH (NOLOCK)
					ON Mtr.fav_id = ava.fav_id
					AND ava.ava_id = @ava_id
					AND ava.ava_exibeObservacaoConselhoPedagogico = 1
				LEFT JOIN CLS_AlunoAvaliacaoTurmaQualidade aaq -- WITH (NOLOCK)
					ON  Mtr.tur_id = aaq.tur_id
					AND Mtr.alu_id = aaq.alu_id
					AND Mtr.mtu_id = aaq.mtu_id
					AND aaq.fav_id = ava.fav_id
					AND aaq.ava_id = ava.ava_id

				LEFT JOIN CLS_AlunoAvaliacaoTurmaDesempenho aad -- WITH (NOLOCK)
					ON  Mtr.tur_id = aad.tur_id
					AND Mtr.alu_id = aad.alu_id
					AND Mtr.mtu_id = aad.mtu_id
					AND aad.fav_id = ava.fav_id
					AND aad.ava_id = ava.ava_id 

				LEFT JOIN CLS_AlunoAvaliacaoTurmaRecomendacao aar -- WITH (NOLOCK)
					ON  Mtr.tur_id = aar.tur_id
					AND Mtr.alu_id = aar.alu_id
					AND Mtr.mtu_id = aar.mtu_id
					AND aar.fav_id = ava.fav_id
					AND aar.ava_id = ava.ava_id

				LEFT JOIN CLS_ALunoAvaliacaoTurmaObservacao ato -- WITH (NOLOCK)
					ON Mtr.tur_id = ato.tur_id
					AND Mtr.alu_id = ato.alu_id
					AND Mtr.mtu_id = ato.mtu_id
					AND ato.fav_id = ava.fav_id
					AND ato.ava_id = ava.ava_id
					AND ato.ato_situacao <> 3
			WHERE
				Mtr.tud_id = @tud_id
			GROUP BY
				Mtr.tur_id
				, Mtr.alu_id
				, Mtr.mtu_id
				, ato_qualidade
				, ato_desempenhoAprendizado
				, ato_recomendacaoAluno
				, ato_recomendacaoResponsavel
		) AS tabela
			GROUP BY --(Adicionado group by por Webber) 
				tabela.tur_id
				, tabela.alu_id 
				, tabela.mtu_id 
				, CASE WHEN tabela.qtdeQualidade = 0 AND tabela.qtdeDesempenhos = 0 AND tabela.qtdeRecomendacao = 0
							AND tabela.ato_qualidade IS NULL AND tabela.ato_desempenhoAprendizado IS NULL 
							AND tabela.ato_recomendacaoAluno IS NULL AND tabela.ato_recomendacaoResponsavel IS NULL
					   THEN 0
					   ELSE 1
				  END		
	),

	AulasCompensadas AS 
	(
		Select
			caa.tud_id
			,caa.alu_id
			,caa.mtu_id
			,caa.mtd_id
			,SUM(ISNULL(cpa.cpa_quantidadeAulasCompensadas, 0)) as qtdCompensadas
		From
			CLS_CompensacaoAusencia cpa -- WITH (NOLOCK)
			INNER JOIN CLS_CompensacaoAusenciaAluno caa -- WITH (NOLOCK)
				ON  caa.tud_id = cpa.tud_id
				AND caa.cpa_id = cpa.cpa_id
				AND caa.caa_situacao = 1
			WHERE
				cpa.tud_id = @tud_id
				AND cpa.tpc_id = @tpc_id
				AND cpa.cpa_situacao = 1 

		GROUP BY
			caa.tud_id
			,caa.alu_id
			,caa.mtu_id
			,caa.mtd_id
	),

	/*
	    12/06/2013 - Hélio C. Lima
	    Criado mais um "passo" CTE deixando as consultas as functions somente com o resultado a ser exibido
	*/
	tabResult AS (

        --	
	    SELECT	
		      Mtd.alu_id
		    , Mtd.mtu_id
		    , Mtd.mtd_id
		    , tur.tur_id
		    , tur.tur_codigo
		    , alc.alc_matricula
		    , Mtd.tud_id
		    , Atd.atd_id
		    , Atd.atd_avaliacao
		    , Mtd.mtd_resultado
		    , Atd.atd_semProfessor
		    , Atd.atd_frequencia
		    , tfa.qtAulas
		    , Atd.atd_numeroFaltas
		    , tfa.qtFaltas
		    , tfa.qtFaltasReposicao
		    , CASE WHEN @documentoOficial = 1 THEN Pes.pes_nomeOficial ELSE Pes.pes_nome END AS pes_nome
			, Pes.pes_dataNascimento
            , ISNULL(Mtr.mtd_numeroChamadaDocente, Mtd.mtd_numeroChamada) AS mtd_numeroChamada
		    , ISNULL(Mtr.mtd_situacaoDocente, Mtd.mtd_situacao) AS mtd_situacao
		    , Atd.atd_relatorio
		    , Atd.arq_idRelatorio
		    , Atd.atd_numeroAulas
            , ISNULL(Mtr.mtd_dataMatriculaDocente, Mtd.mtd_dataMatricula) AS mtd_dataMatricula
            , ISNULL(Mtr.mtd_dataSaidaDocente, Mtd.mtd_dataSaida) AS mtd_dataSaida
            , CASE WHEN (@ExibeCompensacao = 1 AND Atd.atd_id IS NULL) THEN 
					ISNULL(ac.qtdCompensadas, 0)
				ELSE
					ISNULL(Atd.atd_ausenciasCompensadas, 0)
				END AS ausenciasCompensadas
			, tod.observacaoPreenchida
            , toc.observacaoPreenchida AS observacaoConselhoPreenchida
            , 0 AS frequenciaFinal --tff.frequenciaFinal
            , Atd.atd_avaliacaoPosConselho AS avaliacaoPosConselho
			, Atd.atd_justificativaPosConselho AS justificativaPosConselho
			, (CASE WHEN (@ExibeCompensacao = 1)
				THEN 
					CASE WHEN Atd.atd_id IS NULL 
					THEN  
						-- Se não estiver fechada, faz o calculo da frequencia.
						CASE WHEN Qtd.aulas IS NOT NULL AND Qtd.aulas > 0 THEN 
							dbo.FN_Calcula_PorcentagemFrequenciaVariacao
								(Qtd.aulas, (Qtd.faltas + Qtd.faltasReposicao)-qtd.compensadas, Mtr.variacaoFrequencia)
						-- Caso o total de aulas previstas seja 0 a frequência deve ser 100.
						ELSE 100 END
					ELSE Atd.atd_frequenciaFinalAjustada
					END
				ELSE 0
			  END) AS FrequenciaFinalAjustada
			, mtu.mtu_resultado
			, Mtr.variacaoFrequencia
			, Qtd.faltasAnteriores
			, Qtd.compensadasAnteriores
	    FROM @Matriculas   Mtr
		INNER JOIN MTR_MatriculaTurmaDisciplina Mtd -- WITH (NOLOCK)
			ON  Mtr.alu_id = Mtd.alu_id
            AND Mtr.mtu_id = Mtd.mtu_id
			AND Mtr.mtd_id = Mtd.mtd_id
        LEFT JOIN @SomatorioAulasFaltas AS Qtd
	        ON  Mtd.alu_id = Qtd.alu_id
	    LEFT JOIN @TabelaFaltasAulas AS tfa
	        ON  Mtd.alu_id = tfa.alu_id
	        AND Mtd.mtu_id = tfa.mtu_id
	        AND Mtd.mtd_id = tfa.mtd_id

		INNER JOIN MTR_MatriculaTurma mtu -- WITH (NOLOCK)
			ON mtu.alu_id = Mtd.alu_id
			AND mtu.mtu_id = Mtd.mtu_id
			AND mtu_situacao <> 3
		INNER JOIN TUR_Turma tur -- WITH (NOLOCK)
			ON tur.tur_id = mtu.tur_id
			AND tur.tur_situacao <> 3
		INNER JOIN ACA_AlunoCurriculo alc -- WITH (NOLOCK)
			ON alc.alu_id = mtu.alu_id
			AND alc.alc_id = mtu.alc_id
			AND alc.alc_situacao <> 3	

        INNER JOIN ACA_Aluno Alu -- WITH (NOLOCK)
	        ON  Mtd.alu_id   = Alu.alu_id
	        AND alu_situacao <> 3

        INNER JOIN VW_DadosAlunoPessoa Pes -- WITH (NOLOCK)
	        ON  Alu.alu_id   = Pes.alu_id

        LEFT JOIN CLS_AlunoAvaliacaoTurmaDisciplina Atd -- WITH (NOLOCK)
	        ON  Atd.tud_id = Mtd.tud_id
	        AND Atd.alu_id = Mtd.alu_id
	        AND Atd.mtu_id = Mtd.mtu_id
	        AND Atd.mtd_id = Mtd.mtd_id
	        AND Atd.fav_id = @fav_id
	        AND Atd.ava_id = @ava_id
	        AND Atd.atd_situacao <> 3

	  --  LEFT JOIN TabelaFrequenciaFinal AS tff
			--ON tff.alu_id = Mtd.alu_id
			--AND tff.tud_id = Mtd.tud_id

		LEFT JOIN TabelaObservacaoDisciplina tod
			ON tod.alu_id = Mtd.alu_id
			AND tod.mtu_id = Mtd.mtu_id
			AND tod.mtd_id = Mtd.mtd_id

		LEFT JOIN TabelaObservacaoConselho toc
			ON 
			--toc.tur_id = Mtu.tur_id (Comentado aqui por Webber)
			--AND (Comentado aqui por Webber)
			toc.alu_id = Mtu.alu_id
			AND toc.mtu_id = Mtu.mtu_id

		LEFT JOIN AulasCompensadas ac 
			ON ac.alu_id = Mtd.alu_id
			AND ac.mtu_id = Mtd.mtu_id
			AND ac.mtd_id = Mtd.mtd_id

	    WHERE 
	       Mtr.tpc_id = COALESCE(  @tpc_id, 
                                        @tpc_idProximo, 
                                        (
								            -- Se não for avaliação ligada a uma periódica, traz a matrícula no último tpc_id 
								            -- quando for avaliação Final ou Conselho de classe.
								            SELECT MAX(tpc_id)
								            FROM @Matriculas MtrMax 
								            WHERE MtrMax.alu_id = Mtr.alu_id
								            AND @tipoAvaliacao  IN (3, 4) -- 3 - Final, 4 - Conselho de Classe
							            )
							         )
		    AND ISNULL(Mtr.mtd_situacaoDocente, mtd_situacao) IN (1,5)
		    AND COALESCE(Mtr.mtd_numeroChamadaDocente, mtd_numeroChamada, 0) >= 0
		    AND Alu.alu_situacao <> 3		
		    AND (
				    -- Avaliação que não é do tipo "Recuperação" traz todos os alunos
				    (
					    @tipoAvaliacao <> 2 			
				    )
				    OR	
				    -- Avaliação com lançamento de nota por relatório sempre traz todos os alunos,
				    -- independente do tipo de Avaliação
				    (
					    @tipoEscalaDisciplina = 3
				    ) 									
				    OR
				    -- Avaliação do tipo "Recuperação" e com escala do tipo "Númerica" ou "Pareceres"
				    -- traz apenas os alunos que não atingiram o valor mínimo de aprovação
			        (																				
					    @tipoAvaliacao = 2 										
					    AND EXISTS (SELECT  1
					                FROM    @notasPeriodicasRecuperacao N 
						            WHERE   N.alu_id = Mtr.alu_id AND 
						                    N.mtu_id = Mtr.mtu_id AND 
						                    N.mtd_id = Mtr.mtd_id AND 
						                    N.recuperacao = 1
					               )
				    )
			    )

	),
	
	movimentacao AS (

			--Selecina as movimentações que possuem matrícula anterior
			SELECT
				MOV.alu_id,
				mtu_idAnterior,
				CASE WHEN tmo_tipoMovimento IN (6, 12, 14, 15, 16)
					 THEN TMV.tmv_nome + ' em ' + REPLACE(CONVERT(VARCHAR(10), mov.mov_dataRealizacao, 103), '/' + CAST(DATEPART(YEAR, mov.mov_dataRealizacao) AS VARCHAR(4)), '') +
					 	  ISNULL(' p/ ' + turD.tur_codigo, '')
					 WHEN tmo_tipoMovimento IN (8)
					 THEN TMV.tmv_nome + ' p/' + ISNULL(' ' + turD.tur_codigo, '')
					 WHEN tmo_tipoMovimento IN (11)
					 THEN TMV.tmv_nome + ' p/ ' + ISNULL(' ' + turD.tur_codigo, '')
					 ELSE TMV.tmv_nome
				END tmv_nome  
			FROM
				tabResult res
				INNER JOIN MTR_Movimentacao MOV -- WITH (NOLOCK) 
					ON res.alu_id = MOV.alu_id 
				INNER JOIN ACA_TipoMovimentacao TMV -- WITH (NOLOCK) 
					ON MOV.tmv_idSaida = TMV.tmv_id
				LEFT JOIN MTR_TipoMovimentacao tmo -- WITH(NOLOCK)
					ON mov.tmo_id = tmo.tmo_id
					AND tmo.tmo_situacao <> 3
				LEFT JOIN MTR_MatriculaTurma mtuD -- WITH(NOLOCK)
					ON mov.alu_id = mtuD.alu_id
					AND mov.mtu_idAtual = mtuD.mtu_id
				LEFT JOIN TUR_Turma turD -- WITH(NOLOCK)
					ON mtuD.tur_id = turD.tur_id
				LEFT JOIN ACA_CalendarioAnual calD -- WITH(NOLOCK)
					ON turD.cal_id = calD.cal_id
				INNER JOIN MTR_MatriculaTurma mtuO -- WITH(NOLOCK)
					ON mov.alu_id = mtuO.alu_id
					AND mov.mtu_idAnterior = mtuO.mtu_id
					AND mtuO.tur_id = @tur_id
				LEFT JOIN TUR_Turma turO -- WITH(NOLOCK)
					ON mtuO.tur_id = turO.tur_id
				LEFT JOIN ACA_CalendarioAnual calO -- WITH(NOLOCK)
					ON turO.cal_id = calO.cal_id
			WHERE
				mov_situacao NOT IN (3,4)
				AND tmv_situacao <> 3
				AND mtu_idAnterior IS NOT NULL	
	)

	, tbRetorno AS
	(
		SELECT
			  alu_id
			, mtu_id
			, mtd_id
			, tud_id
			, tur_id
			, -1        AS tud_idPrincipal
			, -1        AS mtd_idPrincipal
			, tur_codigo
		    , alc_matricula
			, atd_id    AS AvaliacaoID
			, ISNULL(Res.atd_avaliacao,
				CASE WHEN (Res.atd_id IS NULL AND (@tipoEscalaDisciplina in (1,2)) AND (@tipoEscalaDocente = @tipoEscalaDisciplina))
					THEN REPLACE(CAST(
						(SELECT TOP 1 Media FROM @tbMediasAlunos Med WHERE Med.alu_id = Res.alu_id AND Med.mtu_id = Res.mtu_id)
						 AS VARCHAR(20)), '.', ',')
					ELSE NULL END) AS Avaliacao
			, CASE 
				-- Caso não seja possível alterar o resultado do aluno e a avaliação for do tipo final ou períodica + final não traz o resultado (ele é calculado na tela)
				WHEN @permiteAlterarResultado = 0 AND @tipoAvaliacao IN (3,5) THEN 
					NULL
				-- Caso contrário, traz o resultado normalmente
				ELSE 
					mtd_resultado
			END 
			AS AvaliacaoResultado		

			, ISNULL(atd_semProfessor, 0) AS atd_semProfessor

			-- Frequência
			, CASE 
				WHEN (atd_frequencia IS NULL) THEN 
					-- Se não estiver fechada, faz o calculo da frequencia.
					CASE WHEN qtAulas IS NOT NULL AND qtAulas > 0 THEN 
						dbo.FN_Calcula_PorcentagemFrequenciaVariacao
							(qtAulas, ISNULL(atd_numeroFaltas, qtFaltas), Res.variacaoFrequencia)
					-- Caso o total de aulas previstas seja 0 a frequência deve ser 100.
					ELSE 100 END
				ELSE 
					atd_frequencia
			END
			AS Frequencia

			-- Qtde. de faltas
			, CASE 
				WHEN (atd_numeroFaltas IS NULL AND atd_frequencia IS NULL) THEN 
					qtFaltas
				ELSE 
					atd_numeroFaltas
			END
			AS QtFaltasAluno
			, qtFaltasReposicao AS QtFaltasAlunoReposicao
			-- Qtde. de aulas
			, CASE 
				WHEN (atd_numeroAulas IS NULL AND atd_frequencia IS NULL) THEN 
					qtAulas 
				ELSE 
					atd_numeroAulas
			END
			AS QtAulasAluno

			, Res.pes_nome + 
			(
				CASE 
					WHEN ( Res.mtd_situacao = 5 ) THEN 
						ISNULL((SELECT TOP 1 ' (' + tmv_nome + ')' FROM movimentacao MOV -- WITH (NOLOCK)
						          WHERE MOV.mtu_idAnterior = Res.mtu_id AND MOV.alu_id = Res.alu_id), ' (Inativo)')
					ELSE 
						'' 
				END
			) 
			AS pes_nome
			, Res.pes_dataNascimento

			, CASE 
				WHEN Res.mtd_numeroChamada > 0 THEN 
					CAST(Res.mtd_numeroChamada AS VARCHAR)
				ELSE 
					'-' 
			END 
			AS mtd_numeroChamada

			, Res.mtd_numeroChamada AS mtd_numeroChamadaordem

			, CAST(Res.alu_id AS VARCHAR) + ';' + 
			  CAST(Res.mtd_id AS VARCHAR) + ';' + 
			  CAST(Res.mtu_id AS VARCHAR) 
			AS id

			, CAST(ISNULL(frequenciaFinal, 0) AS DECIMAL(5,2)) AS frequenciaAcumulada

			, atd_relatorio
			, arq_idRelatorio

			-- Alc, Ala e Tca_numeroavaliacao : campos da avaliação do Curso (quando seriado por avaliação).
			-- Não implementado quando é lançamento por disciplina.
			, NULL                       AS alc_id
			, NULL                       AS ala_id
			, NULL						 AS tca_numeroAvaliacao
			, Res.mtd_situacao             AS situacaoMatriculaAluno
			, Res.mtd_dataMatricula        AS dataMatricula
			, Res.mtd_dataSaida            AS dataSaida
			, Res.ausenciasCompensadas		AS ausenciasCompensadas
			, CAST(1 AS BIT)				AS ala_avaliado

			-- Campos que não existem na avaliação por disciplina.
			, NULL AS AvaliacaoSalaRecurso
			, NULL AS AvaliacaoAdicional

			-- Traz o campo Faltoso da tabela AlunoAvaliacaoTurma.
			, ISNULL(
				(
					SELECT TOP 1
						Aat.aat_faltoso
					FROM
						CLS_AlunoAvaliacaoTurma Aat -- WITH (NOLOCK)
					WHERE 
							Aat.tur_id = @tur_id
						AND Aat.alu_id = Res.alu_id
						AND Aat.mtu_id = Res.mtu_id
						AND Aat.fav_id = @fav_id
						AND Aat.ava_id = @ava_id
						AND Aat.aat_situacao = 1
				), CAST(0 AS BIT)
			) AS faltoso
			, CAST(
					ISNULL(
						(
							SELECT TOP 1
								Aat.aat_faltoso
							FROM
								CLS_AlunoAvaliacaoTurma Aat -- WITH (NOLOCK)
							WHERE 
									Aat.tur_id = @tur_id
								AND Aat.alu_id = Res.alu_id
								AND Aat.mtu_id = Res.mtu_id
								AND Aat.fav_id = @fav_id
								AND Aat.ava_id = @ava_id
								AND Aat.aat_situacao = 1
						), 0) AS BIT
				) AS naoAvaliado
			-- Verifica se há dispensa de disciplina para o aluno.
			, 0 AS dispensadisciplina
			, CAST(ISNULL(observacaoPreenchida, 0) AS BIT) AS observacaoPreenchida
            , CAST(ISNULL(observacaoConselhoPreenchida, 0) AS BIT) AS observacaoConselhoPreenchida
            , avaliacaoPosConselho
			, justificativaPosConselho
			, FrequenciaFinalAjustada
			, mtu_resultado
			, atd_numeroAulas AS QtAulasEfetivado
			, faltasAnteriores
			, compensadasAnteriores
		FROM 
			tabResult AS Res
	)   

	SELECT 
		  alu_id
			, mtu_id
			, mtu_id AS mtu_idAnterior
			, mtd_id
			, mtd_id AS mtd_idAnterior
			, tud_id
			, tur_id
			, tur_codigo
		    , alc_matricula
			, tud_idPrincipal
			, mtd_idPrincipal
			, AvaliacaoID
			, Avaliacao
			, AvaliacaoResultado		
			, atd_semProfessor
			, Frequencia
			, QtFaltasAluno
			, QtAulasAluno
			, ISNULL(QtFaltasAlunoReposicao, 0) AS QtFaltasAlunoReposicao
			, pes_nome		
			, ISNULL(CAST(pes_dataNascimento AS VARCHAR(10)), '') AS pes_dataNascimento
			, mtd_numeroChamada
			, id
			, frequenciaAcumulada
			, atd_relatorio
			, arq_idRelatorio
			, alc_id
			, ala_id
			, tca_numeroAvaliacao
			, situacaoMatriculaAluno
			, dataMatricula
			, dataSaida
			, ISNULL(ausenciasCompensadas, 0) AS ausenciasCompensadas
			, ala_avaliado
			, AvaliacaoSalaRecurso
			, AvaliacaoAdicional
			, faltoso
			, naoAvaliado
			, dispensadisciplina
			, observacaoPreenchida
			, observacaoConselhoPreenchida
			, avaliacaoPosConselho
			, justificativaPosConselho
			, FrequenciaFinalAjustada
			, mtu_resultado
			, QtAulasEfetivado
			, ISNULL(faltasAnteriores, 0) AS faltasAnteriores
			, ISNULL(compensadasAnteriores, 0) AS compensadasAnteriores
	FROM	
		tbRetorno 
	GROUP BY
		 alu_id
			, mtu_id
			, mtd_id
			, tud_id
			, tur_id
			, tur_codigo
		    , alc_matricula
			, tud_idPrincipal
			, mtd_idPrincipal
			, AvaliacaoID
			, Avaliacao
			, AvaliacaoResultado		
			, atd_semProfessor
			, Frequencia
			, QtFaltasAluno
			, QtAulasAluno
			, QtFaltasAlunoReposicao
			, pes_nome
			, pes_dataNascimento		
			, mtd_numeroChamada
			, mtd_numeroChamadaordem
			, id
			, frequenciaAcumulada
			, atd_relatorio
			, arq_idRelatorio
			, alc_id
			, ala_id
			, tca_numeroAvaliacao
			, situacaoMatriculaAluno
			, dataMatricula
			, dataSaida
			, ausenciasCompensadas
			, ala_avaliado
			, AvaliacaoSalaRecurso
			, AvaliacaoAdicional
			, faltoso
			, naoAvaliado
			, dispensadisciplina
			, observacaoPreenchida
			, observacaoConselhoPreenchida
			, avaliacaoPosConselho
			, justificativaPosConselho
			, FrequenciaFinalAjustada
			, mtu_resultado
			, QtAulasEfetivado
			, ISNULL(faltasAnteriores, 0)
			, ISNULL(compensadasAnteriores, 0)
	ORDER BY 
		CASE 
		    WHEN @ordenacao = 0 THEN 
			    CASE WHEN ISNULL(mtd_numeroChamadaordem,0) <= 0 THEN 1 ELSE 0 END
		END ASC
		, CASE WHEN @ordenacao = 0 THEN ISNULL(mtd_numeroChamadaordem,0) END ASC
		, CASE WHEN @ordenacao = 1 THEN pes_nome END ASC
END
GO
PRINT N'Altering [dbo].[NEW_Relatorio_0005_SubAtaFinalResultadosEnriquecimentoCurricular]'
GO

-- ==========================================================================================
-- Author:		Daniel Jun Suguimoto
-- Create date: 16/12/2014
-- Description:	Procedure para a geração de dados para o relatório de ata final de resultados e enriquecimento curricular.

---- Alterado: Marcia Haga - 09/03/2015
---- Description: Alterado para nao retornar valor nulo no numero de faltas, numero de compensacoes 
---- e porcentagem de frequencia.

---- Alterado: Marcia Haga - 10/03/2015
---- Description: Corrigido nome da coluna de faltas e compensacoes no retorno.

---- Alterado: Marcia Haga - 13/03/2015
---- Description: Alterado para considerar a frequencia como 100%,
---- caso nao existam aulas.
-- ==========================================================================================
ALTER PROCEDURE [dbo].[NEW_Relatorio_0005_SubAtaFinalResultadosEnriquecimentoCurricular]
	@tur_id BIGINT
	, @cal_id INT 
	, @Regencia BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
    DECLARE @MatriculasBoletim TABLE
	(
		alu_id BIGINT
		, mtu_id INT
		, tur_id BIGINT
		, tpc_id INT
		, tpc_ordem INT
		, PeriodosEquivalentes BIT
		, MesmoCalendario BIT
		, MesmoFormato BIT
		, fav_id INT
		, mtu_numeroChamada INT
		, cal_id INT
		, cal_ano INT
		, cap_id INT
		, PossuiSaidaPeriodo BIT
		, registroExterno BIT 
		, PermiteConceitoGlobal	BIT
		, PermiteDisciplinas BIT
		, mtu_origemDados INT
		, FechamentoUltimoBimestre BIT
		, mov_id INT
	)
	
	DECLARE @dadosAlunos TABLE
	(
		alu_id BIGINT
		, pes_nome VARCHAR(400)
		, tur_id BIGINT
		, fav_id INT
		, tud_id BIGINT
		, tud_tipo TINYINT
		, atd_avaliacao VARCHAR(20)
		, atd_frequencia DECIMAL(5,2)
		, atd_numeroFaltas INT
		, atd_numeroAulas INT
		, atd_ausenciasCompensadas INT
		, atd_frequenciaFinalAjustada DECIMAL(5,2)
		, dis_id INT
		, tds_id INT 
		, tpc_id INT
		, tpc_ordem INT
		, tds_ordem INT
		, Tpc_Agrupamento INT
		, Tpc_exibicao CHAR(3)
		, dis_nome VARCHAR(200)
		, mtu_numeroChamada INT
		, periodoFechado BIT
		, mtu_resultadoDescricao VARCHAR(100)
		, mtu_situacao TINYINT
		, mtd_resultadoDescricao VARCHAR(100)
		, fav_percentualMinimoFrequenciaFinalAjustadaDisciplina DECIMAL(5,2)
		, tds_tipo TINYINT 
		, tap_aulasPrevitas INT
		, mtu_id INT
		, mov_id INT
		, FechamentoUltimoBimestre BIT NOT NULL
		, PossuiFreqExterna BIT
	)
	
	INSERT INTO @MatriculasBoletim (alu_id, mtu_id, tur_id, tpc_id, tpc_ordem, PeriodosEquivalentes, 
						MesmoCalendario, MesmoFormato, fav_id, mtu_numeroChamada, cal_id, cal_ano, cap_id , PossuiSaidaPeriodo,
						registroExterno, PermiteConceitoGlobal, PermiteDisciplinas, mtu_origemDados, FechamentoUltimoBimestre, mov_id)
	select mb.alu_id, mb.mtu_id, mb.tur_id, mb.tpc_id, mb.tpc_ordem, mb.PeriodosEquivalentes, 
						mb.MesmoCalendario, mb.MesmoFormato, mb.fav_id, mb.mtu_numeroChamada, mb.cal_id, mb.cal_ano, mb.cap_id , mb.PossuiSaidaPeriodo,
						mb.registroExterno, mb.PermiteConceitoGlobal, mb.PermiteDisciplinas, mb.mtu_origemDados, mb.mov_id
						, 0 AS FechamentoUltimoBimestre
	from MTR_MatriculasBoletim mb WITH(NOLOCK)
	inner join (select *, ROW_NUMBER() OVER(PARTITION BY alu_id 
											   ORDER BY mtu_id desc) as linha
				 from MTR_MatriculaTurma WITH(NOLOCK) 
				where mtu_situacao <> 3
				  and tur_id = @tur_id) mtu 
		   on mtu.alu_id = mb.alu_id and mtu.mtu_id = mb.mtu_origemDados
	 where mtu.linha = 1 --para evitar casos onde o aluno tem dois mtus para o mesmo tur_id
		AND mb.PeriodosEquivalentes = 1 -- traz apenas alunos que possuem períodos equivalentes
		AND mb.PossuiSaidaPeriodo = 0
		AND mb.registroExterno = 0
	   
	IF not exists (select top 1 alu_id from @MatriculasBoletim)
	begin
		INSERT INTO @MatriculasBoletim (alu_id, mtu_id, tur_id, tpc_id, tpc_ordem, PeriodosEquivalentes, 
						MesmoCalendario, MesmoFormato, fav_id, mtu_numeroChamada, cal_id, cal_ano, cap_id , PossuiSaidaPeriodo,
						registroExterno, PermiteConceitoGlobal, PermiteDisciplinas, mtu_origemDados, FechamentoUltimoBimestre, mov_id)
		SELECT
			alu_id, mtu_id, tur_id, tpc_id, tpc_ordem, PeriodosEquivalentes, MesmoCalendario, MesmoFormato, fav_id, 
			mtu_numeroChamada, cal_id, cal_ano, cap_id, PossuiSaidaPeriodo, registroExterno, PermiteConceitoGlobal, PermiteDisciplinas, mtu_id,
			0 AS FechamentoUltimoBimestre, mov_id
		FROM FN_MatriculasBoletimDa_Turma(@tur_id)
		WHERE PeriodosEquivalentes = 1 -- traz apenas alunos que possuem períodos equivalentes
			 AND PossuiSaidaPeriodo = 0
			 AND registroExterno = 0
	end	
	
	-- Adiciona um registro para a avaliacao final
	INSERT INTO @MatriculasBoletim (alu_id, mtu_id, tur_id, tpc_id, tpc_ordem, PeriodosEquivalentes, 
						MesmoCalendario, MesmoFormato, fav_id, mtu_numeroChamada, cal_id, cal_ano, cap_id , PossuiSaidaPeriodo,
						registroExterno, PermiteConceitoGlobal, PermiteDisciplinas, mtu_origemDados, FechamentoUltimoBimestre, mov_id)
	SELECT 
		alu_id, mtu_id, tur_id, NULL AS tpc_id, NULL AS tpc_ordem, PeriodosEquivalentes, 
		MesmoCalendario, MesmoFormato, fav_id, mtu_numeroChamada, cal_id, cal_ano, NULL AS cap_id , PossuiSaidaPeriodo,
		registroExterno, PermiteConceitoGlobal, PermiteDisciplinas, mtu_origemDados
		, CASE WHEN MatAlu.tpc_ordem = 4 THEN 1 ELSE 0 END AS FechamentoUltimoBimestre, mov_id
	FROM
		(SELECT 
			alu_id, mtu_id, tur_id, null AS tpc_id, null AS tpc_ordem, PeriodosEquivalentes, 
			MesmoCalendario, MesmoFormato, fav_id, mtu_numeroChamada, cal_id, cal_ano, cap_id , PossuiSaidaPeriodo,
			registroExterno, PermiteConceitoGlobal, PermiteDisciplinas,mtu_origemDados, mov_id,
			ROW_NUMBER() OVER(PARTITION BY alu_id ORDER BY alu_id) AS ordem
		FROM 
			@MatriculasBoletim) AS MatAlu
	WHERE 
		MatAlu.ordem = 1

	;WITH alunosDisciplina AS 
	(
		SELECT
			mtu.alu_id,
			mtu.tur_id,
			Mb.tpc_id,
			mtu.mtu_id,
			Mb.fav_id,
			Tud.tud_id,
			Tud.tud_tipo,
			Dis.dis_id,
			tds.tds_id,
			Dis.dis_nome,
			Mb.tpc_ordem,
			Tds.tds_ordem,
			-- Tipo de tipo de disciplina. (1- Disciplina, 2- Enriquecimento curricular, 3- Regência de classe)
			Tds.tds_tipo,
			FechamentoUltimoBimestre,
			Mb.mov_id
		FROM
			@MatriculasBoletim Mb
			INNER JOIN MTR_MatriculaTurma AS MTU WITH(NOLOCK)
				ON MTU.alu_id = Mb.alu_id	
				AND MTU.mtu_id = ISNULL(Mb.mtu_id, Mb.mtu_origemDados)
				AND MTU.mtu_situacao <> 3		
			
			INNER JOIN MTR_MatriculaTurmaDisciplina AS mmtd WITH(NOLOCK)
				ON mmtd.alu_id = MTU.alu_id 
				AND mmtd.mtu_id = MTU.mtu_id
				AND mmtd.mtd_situacao <> 3

			INNER JOIN TUR_TurmaDisciplina AS TUD WITH(NOLOCK)
				ON Tud.tud_id = mmtd.tud_id
				AND Tud.tud_tipo NOT IN (10, 17)
				AND TUD.tud_situacao <> 3
				
			INNER JOIN TUR_Turma AS TUR WITH(NOLOCK)
				ON TUR.tur_id = ISNULL(Mb.tur_id, MTU.tur_id)
				AND TUR.tur_tipo = 1
				AND TUR.tur_situacao <> 3

			INNER JOIN TUR_TurmaDisciplinaRelDisciplina TrD WITH(NOLOCK)
				ON TrD.tud_id = Tud.tud_id

			INNER JOIN ACA_Disciplina Dis WITH(NOLOCK)
				ON Dis.dis_id = TrD.dis_id
				AND Dis.dis_situacao <> 3

			INNER JOIN ACA_TipoDisciplina Tds WITH(NOLOCK)
				ON Tds.tds_id = Dis.tds_id					
				AND Tds.tds_situacao <> 3
		-- Removido filtro de turma para recuperar os dados de todos os alunos no ano
		--WHERE 
			--ISNULL(Mb.tur_id, @tur_id) = @tur_id			
	)
	
	, tipoResultado AS
	(
		SELECT
			tpr.tpr_nomenclatura,
			tpr.tpr_resultado,
			tpr.tpr_tipoLancamento,
			tcr.cur_id,
			tcr.crr_id,
			tcr.crp_id
		FROM
			ACA_TipoResultado tpr WITH(NOLOCK)
			INNER JOIN ACA_TipoResultadoCurriculoPeriodo tcr WITH(NOLOCK)
				ON tpr.tpr_id = tcr.tpr_id
		WHERE
			tpr.tpr_situacao <> 3
	)
	, FrequenciaExterna AS
	(
		SELECT
			Mtd.alu_id,
			Mtd.mtu_id,
			Mtd.tud_id,
			Ad.dis_id,
			SUM(ISNULL(afx.afx_qtdFaltas, 0)) AS qtdFaltas,
			SUM(ISNULL(afx.afx_qtdAulas, 0)) AS qtdAulas
		FROM alunosDisciplina Ad
		INNER JOIN MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
			ON Mtd.alu_id = Ad.alu_id
			AND Mtd.mtu_id = Ad.mtu_id
			AND Mtd.tud_id = Ad.tud_id
			AND Mtd.mtd_situacao <> 3
		INNER JOIN CLS_AlunoFrequenciaExterna afx WITH(NOLOCK)
			ON Mtd.alu_id = afx.alu_id
			AND Mtd.mtu_id = afx.mtu_id
			AND Mtd.mtd_id = afx.mtd_id
			AND Ad.tpc_id = afx.tpc_id
			AND afx.afx_situacao <> 3
		GROUP BY
			Mtd.alu_id,
			Mtd.mtu_id,
			Mtd.tud_id,
			Ad.dis_id
	)
	
	INSERT INTO @dadosAlunos
		SELECT
			Ad.alu_id,
			Pes.pes_nome,
			Ad.tur_id,
			Ad.fav_id,
			Ad.tud_id,
			Ad.tud_tipo,
			ISNULL(Atd.atd_avaliacaoPosConselho, Atd.atd_avaliacao) AS atd_avaliacao,
			Atd.atd_frequencia,
			ISNULL(Atd.atd_numeroFaltas, 0) + ISNULL(afx.qtdFaltas, 0) AS atd_numeroFaltas,
			ISNULL(Atd.atd_numeroAulas, 0) + ISNULL(afx.qtdAulas, 0) AS atd_numeroAulas,
			Atd.atd_ausenciasCompensadas,
			Atd.atd_frequenciaFinalAjustada,
			Ad.dis_id,
			Ad.tds_id,
			Ad.tpc_id,	
			Ad.tpc_ordem,		
			Ad.tds_ordem,
			CASE 
				WHEN Ad.tpc_ordem = 1 THEN 1
				WHEN Ad.tpc_ordem = 2 THEN 2
				WHEN Ad.tpc_ordem = 3 THEN 3
				WHEN Ad.tpc_ordem = 4 THEN 4
				WHEN Ad.tpc_ordem IS NULL THEN 999 
			END  AS Tpc_Agrupamento,
			CASE 
				WHEN Ad.tpc_ordem = 1 THEN '1ºB' 
				WHEN Ad.tpc_ordem = 2 THEN '2ºB' 
				WHEN Ad.tpc_ordem = 3 THEN '3ºB' 
				WHEN Ad.tpc_ordem = 4 THEN '4ºB' 
				WHEN Ad.tpc_ordem IS NULL THEN 'SF' 
			END  AS Tpc_exibicao,
			Ad.dis_nome,
			Mtu.mtu_numeroChamada,
			CASE WHEN Atd.atd_id IS NOT NULL OR (Ad.tpc_ordem IS NULL AND FechamentoUltimoBimestre = 0) THEN 1 ELSE 0 END AS periodoFechado,
			ISNULL(Tpr.tpr_nomenclatura, '-') AS mtu_resultadoDescricao,
			Mtu.mtu_situacao,
			ISNULL(TprDis.tpr_nomenclatura, '') AS mtd_resultadoDescricao,
			Fav.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina,
			Ad.tds_tipo,
			tap_aulasPrevitas,
			Mtu.mtu_id,
			Ad.mov_id,
			FechamentoUltimoBimestre,
			CASE WHEN ISNULL(afx.qtdAulas, 0) > 0 OR ISNULL(afx.qtdFaltas, 0) > 0
				 THEN CAST(1 AS BIT) 
				 WHEN EXISTS(SELECT TOP 1 afx2.alu_id FROM frequenciaExterna afx2
							 WHERE Ad.alu_id = afx2.alu_id
							 AND Ad.dis_id = afx2.dis_id
							 AND (ISNULL(afx2.qtdAulas, 0) > 0 OR ISNULL(afx2.qtdFaltas, 0) > 0))
				 THEN CAST(1 AS BIT)
				 ELSE CAST(0 AS BIT) END AS PossuiFreqExterna
		FROM
			alunosDisciplina Ad
			INNER JOIN MTR_MatriculaTurma Mtu WITH(NOLOCK)
				ON Mtu.alu_id = Ad.alu_id
					AND Mtu.mtu_id = Ad.mtu_id
					AND Mtu.tur_id = Ad.tur_id
					AND Mtu.mtu_situacao <> 3
			INNER JOIN ACA_Aluno Alu WITH (NOLOCK)
				ON Alu.alu_id = Mtu.alu_id	
					AND Alu.alu_situacao <> 3
			INNER JOIN VW_DadosAlunoPessoa Pes
				ON  Pes.alu_id = Alu.alu_id
			INNER JOIN ACA_AlunoCurriculo Alc WITH (NOLOCK)
				ON  Alc.alu_id = Mtu.alu_id
					AND Alc.alc_id = Mtu.alc_id
					AND Alc.alc_situacao <> 3
			INNER JOIN MTR_MatriculaTurmaDisciplina Mtd WITH(NOLOCK)
				ON  Mtd.alu_id = Mtu.alu_id
					AND Mtd.mtu_id = Mtu.mtu_id
					AND Mtd.tud_id = Ad.tud_id
					AND Mtd.mtd_situacao <> 3
			INNER JOIN ACA_FormatoAvaliacao Fav WITH(NOLOCK)
				ON Fav.fav_id = Ad.fav_id
					AND Fav.fav_situacao <> 3
			INNER JOIN ACA_Avaliacao Ava WITH(NOLOCK)
				ON Ava.fav_id = Fav.fav_id
					AND ISNULL(AVA.tpc_id, 999) = ISNULL(Ad.tpc_id, 999) -- Caso seja 999 sera a avaliacao final
					AND Ava.ava_situacao <> 3
			LEFT JOIN CLS_AlunoAvaliacaoTurmaDisciplina Atd WITH(NOLOCK)
				ON Atd.tud_id = Mtd.tud_id
					AND Atd.alu_id = Mtd.alu_id
					AND Atd.mtu_id = Mtd.mtu_id
					AND Atd.mtd_id = Mtd.mtd_id
					AND Atd.fav_id = Fav.fav_id
					AND Atd.ava_id = Ava.ava_id
					AND Atd.atd_situacao <> 3
			LEFT JOIN tipoResultado Tpr
				ON Tpr.cur_id = Mtu.cur_id
				AND Tpr.crr_id = Mtu.crr_id
				AND Tpr.crp_id = Mtu.crp_id
				AND Tpr.tpr_resultado = Mtu.mtu_resultado
				AND Tpr.tpr_tipoLancamento = 1
			LEFT JOIN tipoResultado TprDis
				ON TprDis.cur_id = Mtu.cur_id
				AND TprDis.crr_id = Mtu.crr_id
				AND TprDis.crp_id = Mtu.crp_id
				AND TprDis.tpr_resultado = Mtd.mtd_resultado
				AND TprDis.tpr_tipoLancamento = 2
			LEFT JOIN TUR_TurmaDisciplinaAulaPrevista tap WITH(NOLOCK)
				ON tap.tud_id = mtd.tud_id
				AND tap.tpc_id = ava.tpc_id
				AND tap.tap_situacao <> 3
			LEFT JOIN FrequenciaExterna afx
				ON Mtd.alu_id = afx.alu_id
				AND Mtd.mtu_id = afx.mtu_id
				AND Mtd.tud_id = afx.tud_id
				AND Ad.tpc_id IS NULL
		GROUP BY
			Ad.alu_id,
			Pes.pes_nome,
			Ad.tur_id,
			Ad.fav_id,
			Ad.tud_id,
			Ad.tud_tipo,
			Atd.atd_avaliacaoPosConselho, 
			Atd.atd_avaliacao,
			Atd.atd_frequencia,
			Atd.atd_numeroFaltas,
			Atd.atd_numeroAulas,
			Atd.atd_ausenciasCompensadas,
			Atd.atd_frequenciaFinalAjustada,
			Ad.dis_id,
			Ad.tds_id,
			Ad.tpc_id,
			Ad.tpc_ordem,
			Ad.tds_ordem,
			Ad.tpc_id,
			Ad.dis_nome,
			Mtu.mtu_numeroChamada,
			Atd.atd_id,
			Tpr.tpr_nomenclatura,
			Mtu.mtu_situacao,
			TprDis.tpr_nomenclatura,
			Fav.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina,
			Ad.tds_tipo,
			tap_aulasPrevitas,
			Mtu.mtu_id,
			Ad.mov_id,
			FechamentoUltimoBimestre,
			afx.qtdAulas,
			afx.qtdFaltas

	DECLARE @possuiFreqExterna BIT = 0
	IF (EXISTS(SELECT TOP 1 alu_id FROM @dadosAlunos WHERE PossuiFreqExterna = 1))
		SET @possuiFreqExterna = 1
	
	IF (@Regencia = 1)
	BEGIN
		;WITH tbAlunosSituacao AS 
		(
			SELECT 
				Alu.alu_id,
				--Caso seja movimentacao 8-Remanejamento , 27-Conclusão de Nivel de Ensino traz como ativo, para impressao de anos anteriores.
				1 AS situacao
			FROM 
				@dadosAlunos AS Alu
				INNER JOIN MTR_Movimentacao Mov WITH(NOLOCK)
					ON Mov.alu_id = Alu.alu_id
					AND Mov.mtu_idAnterior = Alu.mtu_id
					AND Mov.mov_situacao <> 3
				INNER JOIN MTR_TipoMovimentacao Tmo WITH(NOLOCK)
					ON Tmo.tmo_id = Mov.tmo_id	
					AND tmo_tipoMovimento IN (8,23,27)
					AND Tmo.tmo_situacao <> 3
			GROUP BY Alu.alu_id
		)

		, dadosAlunosAnuais AS 
		(
			SELECT
				alu_id,
				tud_id,
				SUM(atd_numeroFaltas) AS totalFaltas,
				SUM(atd_ausenciasCompensadas) AS totalAusenciasCompensadas
			FROM
				@dadosAlunos Da
			GROUP BY
				alu_id,
				tud_id
		)
		
		, dadosRegencia AS
		(
			SELECT
				alu_id,
				frequenciaFinalAjustadaRegencia,
				totalFaltasRegencia,
				totalAusenciasCompensadasRegencia
			FROM
			(
				SELECT
					alu_id,
					atd_frequenciaFinalAjustada AS frequenciaFinalAjustadaRegencia,
					SUM(atd_numeroFaltas) OVER (PARTITION BY alu_id) AS totalFaltasRegencia,
					SUM(atd_ausenciasCompensadas) OVER (PARTITION BY alu_id) AS totalAusenciasCompensadasRegencia,
					ROW_NUMBER() OVER (PARTITION BY alu_id ORDER BY ISNULL(tpc.tpc_ordem, 0) DESC) AS linha
				FROM 
					@dadosAlunos Da
					INNER JOIN ACA_TipoPeriodoCalendario tpc WITH(NOLOCK)
						ON tpc.tpc_id = Da.tpc_id
						AND tpc.tpc_situacao <> 3
				WHERE Da.atd_frequenciaFinalAjustada IS NOT NULL 
					AND da.tud_tipo = 11-- Regencia
			) AS tabela
			WHERE linha = 1
			GROUP BY
				alu_id,
				frequenciaFinalAjustadaRegencia,
				totalFaltasRegencia,
				totalAusenciasCompensadasRegencia
		)
		, dadosAlunosTpcMax AS (
			SELECT
				Da.alu_id,
				MAX(tpc_ordem) AS maxOrdem
			FROM @dadosAlunos Da
			GROUP BY Da.alu_id
		)
		, movimentacao AS (
			SELECT
				Da.alu_id,
				Da.mtu_id,
				CASE WHEN tmo_tipoMovimento IN (6, 12, 14, 15, 16)
					 THEN 'TR ' + REPLACE(CONVERT(VARCHAR(10), mov.mov_dataRealizacao, 103), '/' + CAST(DATEPART(YEAR, mov.mov_dataRealizacao) AS VARCHAR(4)), '') +
					 	  ISNULL(' - ' + turD.tur_codigo, '')
					 WHEN tmo_tipoMovimento IN (8)
					 THEN 'RM' + ISNULL(' ' + turD.tur_codigo, '')
					 WHEN tmo_tipoMovimento IN (11)
					 THEN 'RC' + ISNULL(' ' + turD.tur_codigo, '')
					 ELSE ''
				END movMsg
			FROM @dadosAlunos Da
			INNER JOIN dadosAlunosTpcMax dat
				ON Da.alu_id = dat.alu_id
				AND Da.tpc_ordem = dat.maxOrdem
			INNER JOIN MTR_Movimentacao mov WITH(NOLOCK)
				ON Da.alu_id = mov.alu_id
				AND Da.mtu_id = mov.mtu_idAnterior
				AND mov.mov_situacao <> 3
			INNER JOIN MTR_TipoMovimentacao tmo WITH(NOLOCK)
				ON mov.tmo_id = tmo.tmo_id
				AND tmo_tipoMovimento IN (6, 8, 11, 12, 14, 15, 16)
				AND tmo.tmo_situacao <> 3
			LEFT JOIN MTR_MatriculaTurma mtuD WITH(NOLOCK)
				ON mov.alu_id = mtuD.alu_id
				AND mov.mtu_idAtual = mtuD.mtu_id
			LEFT JOIN TUR_Turma turD WITH(NOLOCK)
				ON mtuD.tur_id = turD.tur_id
			LEFT JOIN ACA_CalendarioAnual calD WITH(NOLOCK)
				ON turD.cal_id = calD.cal_id
			INNER JOIN MTR_MatriculaTurma mtuO WITH(NOLOCK)
				ON mov.alu_id = mtuO.alu_id
				AND mov.mtu_idAnterior = mtuO.mtu_id
				AND mtuO.tur_id = @tur_id
			LEFT JOIN TUR_Turma turO WITH(NOLOCK)
				ON mtuO.tur_id = turO.tur_id
			LEFT JOIN ACA_CalendarioAnual calO WITH(NOLOCK)
				ON turO.cal_id = calO.cal_id
			WHERE 
				turD.tur_id IS NULL OR calD.cal_ano = calO.cal_ano --Ou não tem turma destino ou a turma destino é do mesmo ano
			GROUP BY
				Da.alu_id,
				Da.mtu_id,
				tmo_tipoMovimento,
				mov.mov_dataRealizacao,
				turD.tur_codigo
		)
		
		SELECT
			Da.alu_id,
			Da.pes_nome +
			CASE WHEN ISNULL(mov.movMsg, '') = ''
				 THEN ''
				 ELSE ' (' + mov.movMsg + ')'
			END AS pes_nome,
			Da.tur_id,
			Da.fav_id,
			Da.tud_id,
			Da.tud_tipo,
			Da.atd_avaliacao,
			Da.atd_frequencia,
			ISNULL(Da.atd_numeroFaltas, 0) AS atd_numeroFaltas,
			Da.atd_numeroAulas,
			ISNULL(Da.atd_ausenciasCompensadas, 0) AS atd_ausenciasCompensadas,
			Da.atd_frequenciaFinalAjustada,
			Da.dis_id,
			Da.tds_id,
			Da.tpc_id,
			Da.tds_ordem,
			Da.Tpc_Agrupamento,
			Da.Tpc_exibicao,
			Da.dis_nome,
			Da.mtu_numeroChamada,
			Da.periodoFechado,
			Da.mtu_resultadoDescricao,
			CASE WHEN EXISTS(SELECT TOP 1 Da2.alu_id FROM dadosAlunosTpcMax DaT
							 INNER JOIN @dadosAlunos Da2 ON DaT.alu_id = Da2.alu_id AND Dat.maxOrdem = Da2.tpc_ordem
							 WHERE Da2.alu_id = Da.alu_id AND Da2.mtu_situacao = 1)
				 THEN 1 ELSE ISNULL(TAS.situacao, Da.mtu_situacao) END AS mtu_situacao, 
			ISNULL(TAS.situacao, Da.mtu_situacao) AS mtu_situacaoPeriodo,
			Da.mtd_resultadoDescricao,
			Da.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina,
			Da.tds_tipo,
			Da.tap_aulasPrevitas,
			Da.mtu_id,
			SUM(ISNULL(Da.atd_ausenciasCompensadas, 0)) OVER(PARTITION BY Da.alu_id, Da.tud_id) AS atd_AusenciasCompensadasVC,
			ISNULL(CAST(Daa.totalFaltas AS VARCHAR),'0') AS totalFaltas,
			SUM(ISNULL(Daa.totalAusenciasCompensadas, 0)) OVER (PARTITION BY Da.alu_id, Da.tpc_ordem) AS totalAusenciasCompensadas,
			ISNULL((SELECT 
					TOP 1 CAST(CAST(ISNULL(dda.atd_frequenciaFinalAjustada,0) AS DECIMAL(5,0)) AS VARCHAR)
				FROM 
					@dadosAlunos dda 
				WHERE 	
					dda.alu_id = da.alu_id
					AND dda.tud_id = da.tud_id
					AND dda.tpc_ordem = (SELECT MAX(ddaa.tpc_ordem) FROM @dadosAlunos ddaa 
														WHERE 	
															ddaa.alu_id = dda.alu_id
															AND ddaa.tud_id = dda.tud_id
															AND ddaa.tpc_id IS NOT NULL
															AND ddaa.tud_tipo = 11
															AND ddaa.atd_frequenciaFinalAjustada IS NOT NULL)), '100')
															AS frequenciaFinalAjustada, 
			CAST(CAST(0 AS DECIMAL(5,0)) AS VARCHAR) AS frequenciaAnual,
			ISNULL(CAST(Dr.totalFaltasRegencia AS VARCHAR),'0') AS totalFaltasRegencia,
			ISNULL(CAST(Dr.totalAusenciasCompensadasRegencia AS VARCHAR),'0') AS totalAusenciasCompensadasRegencia,
			CAST(CAST(ISNULL(Dr.frequenciaFinalAjustadaRegencia,100) AS DECIMAL(5,0)) AS VARCHAR) AS frequenciaFinalAjustadaRegencia,
			PossuiFreqExterna AS PossuiFreqExternaAtual,
			@possuiFreqExterna AS possuiFreqExterna
		FROM
			@dadosAlunos Da
			LEFT JOIN dadosAlunosAnuais Daa
				ON Daa.alu_id = Da.alu_id
				AND Daa.tud_id = Da.tud_id
			LEFT JOIN dadosRegencia Dr
				ON Dr.alu_id = Da.alu_id
			LEFT JOIN tbAlunosSituacao AS TAS
				ON TAS.alu_id = Da.alu_id
			LEFT JOIN movimentacao mov
				ON Da.alu_id = mov.alu_id
				AND Da.mtu_id = mov.mtu_id
		ORDER BY 
			Da.mtu_numeroChamada,
			Da.pes_nome,
			Da.dis_nome,
			Da.Tpc_Agrupamento
	END
	ELSE
	BEGIN
		;WITH tbAlunosSituacao AS 
		(
			SELECT 
				Alu.alu_id,
				--Caso seja movimentacao 8-Remanejamento , 27-Conclusão de Nivel de Ensino traz como ativo, para impressao de anos anteriores.
				1 AS situacao
			FROM 
				@dadosAlunos AS Alu
				INNER JOIN MTR_Movimentacao Mov WITH(NOLOCK)
					ON Mov.alu_id = Alu.alu_id
					AND Mov.mtu_idAnterior = Alu.mtu_id
					AND Mov.mov_situacao <> 3
				INNER JOIN MTR_TipoMovimentacao Tmo WITH(NOLOCK)
					ON Tmo.tmo_id = Mov.tmo_id	
					AND tmo_tipoMovimento IN (8,23,27)
					AND Tmo.tmo_situacao <> 3
			GROUP BY Alu.alu_id
		)

		, dadosAlunosAnuais AS
		(
			SELECT
				alu_id,
				tud_id,
				CAST(SUM(atd_numeroFaltas) AS DECIMAL(10,2)) AS totalFaltas,
				CAST(SUM(atd_ausenciasCompensadas) AS DECIMAL(10,2)) AS totalAusenciasCompensadas,
				CAST(SUM(atd_numeroAulas) AS DECIMAL(10,2)) AS totalAulas
			FROM
				@dadosAlunos Da
			GROUP BY
				alu_id,
				tud_id
		)
				
		, frequenciaAnual AS 
		(
			SELECT
				alu.alu_id,
				CASE WHEN SUM(totalAulas) > 0 THEN
					(1 - (SUM(totalFaltas) - SUM(totalAusenciasCompensadas)) / SUM(totalAulas)) * 100
				ELSE 100 END AS frequenciaFinalAnual												
			FROM 
				dadosAlunosAnuais alu
			GROUP BY alu.alu_id
		)		

		---****************************************Método antigo calculado a média das últimas atd_frequenciaFinalAjustada*************************************
		--, frequenciaFinaisBimestres AS
		-- (
		--	SELECT
		--		alu_id,
		--		tud_id,
		--		atd_frequenciaFinalAjustada
		--	FROM
		--	(
		--		SELECT 
		--			dda.alu_id,
		--			dda.tud_id,
		--			dda.atd_frequenciaFinalAjustada,
		--			ROW_NUMBER() OVER (PARTITION BY dda.alu_id, dda.tud_id ORDER BY dda.tpc_ordem DESC) AS linha
		--		FROM 
		--			@dadosAlunos dda 
		--		WHERE
		--			dda.tpc_id IS NOT NULL
		--			AND dda.atd_frequenciaFinalAjustada IS NOT NULL
		--	) AS tabela
		--	WHERE linha = 1
		--	GROUP BY 
		--		alu_id,
		--		tud_id,
		--		atd_frequenciaFinalAjustada
		--)

		--, frequenciaAnual AS
		--(
		--	SELECT
		--		alu_id,
		--		CAST(AVG(atd_frequenciaFinalAjustada) AS DECIMAL(5,2)) AS frequenciaFinalAjustada
		--	FROM 
		--		frequenciaFinaisBimestres FFB
		--	GROUP BY FFB.alu_id
		--)
		---****************************************Método antigo calculado a média das últimas atd_frequenciaFinalAjustada*************************************
		
		, dadosAlunosTpcMax AS (
			SELECT
				Da.alu_id,
				MAX(tpc_ordem) AS maxOrdem
			FROM @dadosAlunos Da
			GROUP BY Da.alu_id
		)
		, movimentacao AS (
			SELECT
				Da.alu_id,
				Da.mtu_id,
				CASE WHEN tmo_tipoMovimento IN (6, 12, 14, 15, 16)
					 THEN 'TR ' + REPLACE(CONVERT(VARCHAR(10), mov.mov_dataRealizacao, 103), '/' + CAST(DATEPART(YEAR, mov.mov_dataRealizacao) AS VARCHAR(4)), '') +
					 	  ISNULL(' - ' + turD.tur_codigo, '')
					 WHEN tmo_tipoMovimento IN (8)
					 THEN 'RM' + ISNULL(' ' + turD.tur_codigo, '')
					 WHEN tmo_tipoMovimento IN (11)
					 THEN 'RC' + ISNULL(' ' + turD.tur_codigo, '')
					 ELSE ''
				END movMsg
			FROM @dadosAlunos Da
			INNER JOIN dadosAlunosTpcMax dat
				ON Da.alu_id = dat.alu_id
				AND Da.tpc_ordem = dat.maxOrdem
			INNER JOIN MTR_Movimentacao mov WITH(NOLOCK)
				ON Da.alu_id = mov.alu_id
				AND Da.mtu_id = mov.mtu_idAnterior
				AND mov.mov_situacao <> 3
			INNER JOIN MTR_TipoMovimentacao tmo WITH(NOLOCK)
				ON mov.tmo_id = tmo.tmo_id
				AND tmo_tipoMovimento IN (6, 8, 11, 12, 14, 15, 16)
				AND tmo.tmo_situacao <> 3
			LEFT JOIN MTR_MatriculaTurma mtuD WITH(NOLOCK)
				ON mov.alu_id = mtuD.alu_id
				AND mov.mtu_idAtual = mtuD.mtu_id
			LEFT JOIN TUR_Turma turD WITH(NOLOCK)
				ON mtuD.tur_id = turD.tur_id
			LEFT JOIN ACA_CalendarioAnual calD WITH(NOLOCK)
				ON turD.cal_id = calD.cal_id
			LEFT JOIN MTR_MatriculaTurma mtuO WITH(NOLOCK)
				ON mov.alu_id = mtuO.alu_id
				AND mov.mtu_idAtual = mtuO.mtu_id
			LEFT JOIN TUR_Turma turO WITH(NOLOCK)
				ON mtuO.tur_id = turO.tur_id
			LEFT JOIN ACA_CalendarioAnual calO WITH(NOLOCK)
				ON turO.cal_id = calO.cal_id
			WHERE 
				turD.tur_id IS NULL OR calD.cal_ano = calO.cal_ano --Ou não tem turma destino ou a turma destino é do mesmo ano
			GROUP BY
				Da.alu_id,
				Da.mtu_id,
				tmo_tipoMovimento,
				mov.mov_dataRealizacao,
				turD.tur_codigo
		)
		
		SELECT
			Da.alu_id,
			Da.pes_nome +
			CASE WHEN ISNULL(mov.movMsg, '') = ''
				 THEN ''
				 ELSE ' (' + mov.movMsg + ')'
			END AS pes_nome,
			Da.tur_id,
			Da.fav_id,
			Da.tud_id,
			Da.tud_tipo,
			Da.atd_avaliacao,
			Da.atd_frequencia,
			ISNULL(Da.atd_numeroFaltas, 0) AS atd_numeroFaltas,
			Da.atd_numeroAulas,
			ISNULL(Da.atd_ausenciasCompensadas, 0) AS atd_ausenciasCompensadas,
			Da.atd_frequenciaFinalAjustada,
			Da.dis_id,
			Da.tds_id,
			Da.tpc_id,
			Da.tds_ordem,
			Da.Tpc_Agrupamento,
			Da.Tpc_exibicao,
			Da.dis_nome,
			Da.mtu_numeroChamada,
			Da.periodoFechado,
			Da.mtu_resultadoDescricao,
			CASE WHEN EXISTS(SELECT TOP 1 Da2.alu_id FROM dadosAlunosTpcMax DaT
							 INNER JOIN @dadosAlunos Da2 ON DaT.alu_id = Da2.alu_id AND Dat.maxOrdem = Da2.tpc_ordem
							 WHERE Da2.alu_id = Da.alu_id AND Da2.mtu_situacao = 1)
				 THEN 1 ELSE ISNULL(TAS.situacao, Da.mtu_situacao) END AS mtu_situacao,
			ISNULL(TAS.situacao, Da.mtu_situacao) AS mtu_situacaoPeriodo,
			Da.mtd_resultadoDescricao,
			Da.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina,
			Da.tds_tipo,
			Da.tap_aulasPrevitas,
			Da.mtu_id,
			SUM(ISNULL(Da.atd_ausenciasCompensadas, 0)) OVER(PARTITION BY Da.alu_id, Da.tud_id) AS atd_AusenciasCompensadasVC,
			ISNULL(CAST(Daa.totalFaltas AS VARCHAR),'0') AS totalFaltas,
			SUM(ISNULL(Daa.totalAusenciasCompensadas, 0)) OVER (PARTITION BY Da.alu_id, Da.tpc_ordem) AS totalAusenciasCompensadas,
			ISNULL((SELECT 
					TOP 1 CAST(CAST(ISNULL(dda.atd_frequenciaFinalAjustada,0) AS DECIMAL(5,0)) AS VARCHAR)
				FROM 
					@dadosAlunos dda 
				WHERE 	
					dda.alu_id = da.alu_id
					AND dda.tud_id = da.tud_id
					AND dda.tpc_ordem = (SELECT MAX(ddaa.tpc_ordem) FROM @dadosAlunos ddaa 
														WHERE 	
															ddaa.alu_id = dda.alu_id
															AND ddaa.tud_id = dda.tud_id
															AND ddaa.tpc_id IS NOT NULL
															AND ddaa.atd_frequenciaFinalAjustada IS NOT NULL)), '100')
															AS frequenciaFinalAjustada, 
			--dbo.FN_Aplica_VariacaoPorcentagemFrequenciaString(Fa.frequenciaFinalAnual,1) AS frequenciaAnual,
			0 AS frequenciaAnual,
			ISNULL(CAST(0 AS VARCHAR),'0') AS totalFaltasRegencia,
			ISNULL(CAST(0 AS VARCHAR),'0') AS totalAusenciasCompensadasRegencia,
			CAST(CAST(0 AS DECIMAL(5,0)) AS VARCHAR) AS frequenciaFinalAjustadaRegencia,
			PossuiFreqExterna AS PossuiFreqExternaAtual,
			@possuiFreqExterna AS possuiFreqExterna
		FROM
			@dadosAlunos Da
			LEFT JOIN dadosAlunosAnuais Daa
				ON Daa.alu_id = Da.alu_id
				AND Daa.tud_id = Da.tud_id
			LEFT JOIN frequenciaAnual Fa
				ON Fa.alu_id = Da.alu_id
			LEFT JOIN tbAlunosSituacao AS TAS
				ON TAS.alu_id = Da.alu_id
			LEFT JOIN movimentacao mov
				ON Da.alu_id = mov.alu_id
				AND Da.mtu_id = mov.mtu_id
		ORDER BY 
			Da.mtu_numeroChamada,
			Da.pes_nome,
			Da.dis_nome,
			Da.Tpc_Agrupamento
	END
END
GO
PRINT N'Creating [dbo].[NEW_TUR_TurmaDisciplina_SelecionaDisciplinasDivergenciasAulasPrevistas]'
GO
-- =============================================
-- Author:		Marcia Haga
-- Create date: 28/03/2017
-- Description:	Retorna as disciplinas com divergência entre aulas criadas e aulas previstas.
-- =============================================
CREATE PROCEDURE [dbo].[NEW_TUR_TurmaDisciplina_SelecionaDisciplinasDivergenciasAulasPrevistas]
	@tudIds VARCHAR(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @dataAtual DATE = CAST(GETDATE() AS DATE)

	DECLARE @tev_idEfetivacaoNotas INT = 
	(
		SELECT TOP 1 CAST(pac.pac_valor AS INT)
		FROM ACA_ParametroAcademico pac WITH(NOLOCK)
		WHERE pac.pac_situacao <> 3 AND pac.pac_chave = 'TIPO_EVENTO_EFETIVACAO_NOTAS'
	);

	SELECT div.tud_id
	FROM FN_StringToArrayInt64(@tudIds, ',') 
	INNER JOIN REL_DivergenciaAulasPrevistas div WITH(NOLOCK)
		ON div.tud_id = valor
	INNER JOIN TUR_TurmaRelTurmaDisciplina relTud WITH(NOLOCK)
		ON relTud.tud_id = div.tud_id
	INNER JOIN TUR_Turma tur WITH(NOLOCK)
		ON tur.tur_id = relTud.tur_id
		AND tur.tur_situacao <> 3
	INNER JOIN ACA_CalendarioAnual cal WITH(NOLOCK)
		ON cal.cal_id = tur.cal_id
		AND cal.cal_situacao <> 3
	LEFT JOIN ACA_CalendarioPeriodo cap WITH(NOLOCK)
		ON cap.cal_id = cal.cal_id
		AND cap.tpc_id = div.tpc_id
		AND @dataAtual >= cap.cap_dataInicio
		AND @dataAtual <= cap.cap_dataFim
		AND cap.cap_situacao <> 3
	WHERE
	-- Só exibir a divergência se for o bimestre atual
	cap.cap_id IS NOT NULL
	OR
	-- ou se é um bimestre que já foi fechado.
	EXISTS
	(
		SELECT TOP 1 1 
		FROM ACA_Evento evt WITH(NOLOCK)
		INNER JOIN ACA_CalendarioEvento cae WITH(NOLOCK)
			ON cae.evt_id = evt.evt_id
			AND cae.cal_id = cal.cal_id
		WHERE evt.tev_id = @tev_idEfetivacaoNotas
		AND evt.tpc_id = div.tpc_id
		AND evt.evt_dataInicio <= @dataAtual
		AND 
		(
			evt.evt_padrao = 1
			OR evt.esc_id = tur.esc_id
		)
		AND evt.evt_situacao <> 3
	)
	GROUP BY div.tud_id

END
GO
PRINT N'Adding foreign keys to [dbo].[ACA_AlunoJustificativaFaltaAnexo]'
GO
ALTER TABLE [dbo].[ACA_AlunoJustificativaFaltaAnexo] ADD CONSTRAINT [FK_ACA_AlunoJustificativaFaltaAnexo_ACA_AlunoJustificativaFalta] FOREIGN KEY ([alu_id], [afj_id]) REFERENCES [dbo].[ACA_AlunoJustificativaFalta] ([alu_id], [afj_id])
GO
ALTER TABLE [dbo].[ACA_AlunoJustificativaFaltaAnexo] ADD CONSTRAINT [FK_ACA_AlunoJustificativaFaltaAnexo_SYS_Arquivo] FOREIGN KEY ([arq_id]) REFERENCES [dbo].[SYS_Arquivo] ([arq_id])
GO
PRINT N'Adding foreign keys to [dbo].[REL_DivergenciaAulasPrevistas]'
GO
ALTER TABLE [dbo].[REL_DivergenciaAulasPrevistas] ADD CONSTRAINT [FK_REL_DivergenciaAulasPrevistas_TUR_TurmaDisciplina] FOREIGN KEY ([tud_id]) REFERENCES [dbo].[TUR_TurmaDisciplina] ([tud_id])
GO
ALTER TABLE [dbo].[REL_DivergenciaAulasPrevistas] ADD CONSTRAINT [FK_REL_DivergenciaAulasPrevistas_ACA_TipoPeriodoCalendario] FOREIGN KEY ([tpc_id]) REFERENCES [dbo].[ACA_TipoPeriodoCalendario] ([tpc_id])
GO
PRINT N'Altering trigger [dbo].[TRG_ACA_EventoLimite_Identity] on [dbo].[ACA_EventoLimite]'
GO

-- =============================================
-- Author:		Amanda de Maia Areias
-- Create date: 24/07/2015
-- Description:	Realiza o autoincremento do 
--				campo evl_id garantindo que
--				sempre será reiniciado em 1
--				qdo uma combinação de cal_id e
--				tev_id for inserida
-- =============================================
ALTER TRIGGER [dbo].[TRG_ACA_EventoLimite_Identity]
ON [dbo].[ACA_EventoLimite] INSTEAD OF INSERT
AS
BEGIN
	DECLARE @ID INT
	SELECT 
		@ID = CASE WHEN MAX(ACA_EventoLimite.evl_id) IS NULL THEN 1 ELSE MAX(ACA_EventoLimite.evl_id)+1 END 
	FROM 
		ACA_EventoLimite WITH (UPDLOCK, HOLDLOCK)
		INNER JOIN inserted
			ON ACA_EventoLimite.cal_id = inserted.cal_id
			   AND ACA_EventoLimite.tev_id = inserted.tev_id
				
	INSERT INTO ACA_EventoLimite (cal_id, tev_id, evl_id, tpc_id, esc_id, uni_id, evl_dataInicio, evl_dataFim, usu_id, evl_situacao, evl_dataCriacao, evl_dataAlteracao, uad_id)
    SELECT cal_id, tev_id, @ID, tpc_id, esc_id, uni_id, evl_dataInicio, evl_dataFim, usu_id, evl_situacao, evl_dataCriacao, evl_dataAlteracao, uad_id FROM inserted

	SELECT ISNULL(@ID,-1)								
END
GO