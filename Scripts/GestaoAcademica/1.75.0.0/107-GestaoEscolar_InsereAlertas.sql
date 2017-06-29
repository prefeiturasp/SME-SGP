USE [GestaoPedagogica]
GO

--Iniciar transação
BEGIN TRANSACTION
SET XACT_ABORT ON

	IF (NOT EXISTS (SELECT TOP 1 1 FROM CFG_Alerta WITH(NOLOCK)))
	BEGIN
		DECLARE @dataAtual DATETIME = GETDATE()

		INSERT INTO CFG_Alerta
           (cfa_tipo
           ,cfa_nome
           ,cfa_nomeProcedimento
           ,cfa_assunto
           ,cfa_periodoAnalise
           ,cfa_periodoValidade
           ,cfa_situacao
           ,cfa_dataCriacao
           ,cfa_dataAlteracao)
		 VALUES
			   (1
			   ,'Docente - Preenchimento de frequência'
			   ,'MS_JOB_AlertaPreenchimentoFrequencia'
			   ,null
			   ,null
			   ,null
			   ,1
			   ,@dataAtual
			   ,@dataAtual)

		INSERT INTO CFG_Alerta
           (cfa_tipo
           ,cfa_nome
           ,cfa_nomeProcedimento
           ,cfa_assunto
           ,cfa_periodoAnalise
           ,cfa_periodoValidade
           ,cfa_situacao
           ,cfa_dataCriacao
           ,cfa_dataAlteracao)
		 VALUES
			   (2
			   ,'Docente - Aviso de início de fechamento'
			   ,'MS_JOB_AlertaInicioFechamento'
			   ,null
			   ,null
			   ,null
			   ,1
			   ,@dataAtual
			   ,@dataAtual)

		INSERT INTO CFG_Alerta
           (cfa_tipo
           ,cfa_nome
           ,cfa_nomeProcedimento
           ,cfa_assunto
           ,cfa_periodoAnalise
           ,cfa_periodoValidade
           ,cfa_situacao
           ,cfa_dataCriacao
           ,cfa_dataAlteracao)
		 VALUES
			   (3
			   ,'Docente - Aviso de final de fechamento'
			   ,'MS_JOB_AlertaFimFechamento'
			   ,null
			   ,null
			   ,null
			   ,1
			   ,@dataAtual
			   ,@dataAtual)

		INSERT INTO CFG_Alerta
           (cfa_tipo
           ,cfa_nome
           ,cfa_nomeProcedimento
           ,cfa_assunto
           ,cfa_periodoAnalise
           ,cfa_periodoValidade
           ,cfa_situacao
           ,cfa_dataCriacao
           ,cfa_dataAlteracao)
		 VALUES
			   (4
			   ,'Gestores - Alunos com baixa frequência'
			   ,'MS_JOB_AlertaAlunosBaixaFrequencia'
			   ,null
			   ,null
			   ,null
			   ,1
			   ,@dataAtual
			   ,@dataAtual)

		INSERT INTO CFG_Alerta
           (cfa_tipo
           ,cfa_nome
           ,cfa_nomeProcedimento
           ,cfa_assunto
           ,cfa_periodoAnalise
           ,cfa_periodoValidade
           ,cfa_situacao
           ,cfa_dataCriacao
           ,cfa_dataAlteracao)
		 VALUES
			   (5
			   ,'Gestores - Alunos com faltas consecutivas'
			   ,'MS_JOB_AlertaAlunosFaltasConsecutivas'
			   ,null
			   ,null
			   ,null
			   ,1
			   ,@dataAtual
			   ,@dataAtual)
	END

-- Fechar transação
SET XACT_ABORT OFF
COMMIT TRANSACTION
GO