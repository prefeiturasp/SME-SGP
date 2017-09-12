DECLARE
	@SQL VARCHAR(MAX)
	,@RoleName sysname = N'db_executor'

IF NOT EXISTS (SELECT loginname FROM master.dbo.syslogins WHERE name = '$UserName$')
BEGIN
	SET @SQL = 'CREATE LOGIN [' + '$UserName$' + '] WITH PASSWORD= N''' + '$UserPass$' + ''', DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF';
	EXEC(@SQL)
END

IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = '$UserName$' AND type = 'S')
BEGIN
	SET @SQL = 'DROP USER ' + '$UserName$' + ';'
	EXEC(@SQL)
END

SET @SQL = 'CREATE USER ' + '$UserName$' + ' FOR LOGIN ' + '$UserName$' + ';'
EXEC(@SQL)

SET @SQL = 'EXEC sp_addrolemember ' + @roleName + ', ' + '$UserName$' + ';'
EXEC(@SQL)