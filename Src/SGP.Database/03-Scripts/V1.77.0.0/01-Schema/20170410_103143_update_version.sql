/*------------------------------------------------------------------------------------
   Project/Ticket#: SGP
   Description: Atualiza a versão na tabela de controle
-------------------------------------------------------------------------------------*/
	
DECLARE
	@Versao VARCHAR(MAX) = '1.77.0.0'

IF EXISTS (SELECT ver_id FROM CFG_Versao WHERE ver_Versao = @Versao)
BEGIN
	UPDATE CFG_Versao SET ver_DataAlteracao = GETDATE() WHERE ver_Versao = @Versao
END
ELSE
BEGIN
	INSERT INTO CFG_Versao VALUES (@Versao, GETDATE() ,GETDATE())
END
GO