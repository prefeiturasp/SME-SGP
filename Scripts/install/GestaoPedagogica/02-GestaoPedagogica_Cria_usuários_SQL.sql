USE [master]
GO
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'insira_seu_usuario')
CREATE LOGIN [insira_seu_usuario] WITH PASSWORD=N'insira_sua_senha', DEFAULT_DATABASE=[master], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO

USE GestaoPedagogica
GO
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'insira_seu_usuario')
DROP USER [insira_seu_usuario]
GO
CREATE USER [insira_seu_usuario] FOR LOGIN [insira_seu_usuario]
GO
EXEC sp_addrolemember N'db_datareader', N'insira_seu_usuario'
GO
EXEC sp_addrolemember N'db_datawriter', N'insira_seu_usuario'
GO
EXEC sp_addrolemember N'db_ddladmin', N'insira_seu_usuario'
GO