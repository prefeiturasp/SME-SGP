USE master;
GO

DECLARE @DBName VARCHAR(120)

-- Banco de dados do Sistema
SET @DBName = 'GestaoPedagogica'

--------------------------------------------------
-- Caminho do arquivo do banco de dados			--
-- Por Padrão é pego o caminho do bd Master.	--
--------------------------------------------------                  
DECLARE @data_path nvarchar(512);
DECLARE @data_path_log nvarchar(512);
SET @data_path = (SELECT SUBSTRING(physical_name, 1, CHARINDEX(N'master.mdf', LOWER(physical_name)) - 1)
                  FROM master.sys.master_files
                  WHERE database_id = 1 AND file_id = 1); 
SET @data_path_log = @data_path
               
-- Configure o caminho do arquivo.              
--SET @data_path = 'C:\'
--SET @data_path_log = 'C:\'

--IF DB_ID (@DBName) IS NOT NULL 
--BEGIN
--	EXECUTE('DROP DATABASE ' + @DBName)
--	PRINT 'O banco de dados ' + @DBName + ' excluído.' 
--END

-- execute the CREATE DATABASE statement 
-- Banco Principal
PRINT 'Criando banco de dados: '+ @DBName + '...'
EXECUTE
(
'CREATE DATABASE '+ @DBName + ' ON  PRIMARY 
( NAME = '+ @DBName +', FILENAME = '''+ @data_path + @DBName 
+ '.mdf'', SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = '+ @DBName +'_log, FILENAME = '''+ @data_path_log + @DBName 
+ '_log.ldf'' , SIZE = 1024KB , FILEGROWTH = 10%) 
 ALTER DATABASE '+ @DBName +' SET RECOVERY FULL 
' )

IF DB_ID (@DBName) IS NOT NULL
BEGIN
	PRINT 'Banco de dados: ' + @DBName + ' criado com sucesso.'
	EXECUTE('ALTER DATABASE ' + @DBName + ' SET TRUSTWORTHY ON')
	PRINT 'Banco de dados: ' + @DBName + ' configurado com sucesso.'
END

GO
EXEC SP_CONFIGURE 'CLR ENABLED', 1;
RECONFIGURE WITH OVERRIDE;
PRINT 'CLR Configurado.'
GO 
