USE [CoreSSO]
GO

DECLARE @RC int
DECLARE @nomeSistema varchar(100)
DECLARE @nomeModulo varchar(50)
DECLARE @nomeModuloPai varchar(50)

-- TODO: Set parameter values here.

EXECUTE @RC = [dbo].[MS_RemovePaginaMenu] 
   @nomeSistema = ' SGP'
  ,@nomeModulo = 'Disciplinas eletivas'
  ,@nomeModuloPai = 'Administração'
GO


