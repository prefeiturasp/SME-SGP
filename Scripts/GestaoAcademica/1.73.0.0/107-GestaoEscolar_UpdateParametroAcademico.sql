USE [GestaoPedagogica]
GO

UPDATE ACA_ParametroAcademico
   SET pac_valor = '0'
	, pac_dataAlteracao = GETDATE()
 WHERE pac_chave = 'TIPO_MODALIDADES_EJA_REMOVER_RELATORIO'

GO