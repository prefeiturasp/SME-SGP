USE [GestaoPedagogica]

BEGIN TRANSACTION
SET XACT_ABORT ON 

	EXEC MS_InsereResource 
		@rcr_chave = 'Index.lblBoletimNaoDisponivel.Text',
		@rcr_NomeResource = 'AreaAluno', 
		@rcr_cultura = 'pt-BR',
		@rcr_valorPadrao = 'Para informações sobre o boletim, procurar a secretaria da escola.' 
 	            
SET XACT_ABORT OFF
COMMIT TRANSACTION            