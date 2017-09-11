DECLARE
	@SQL VARCHAR(MAX)
	,@RoleName sysname = N'db_executor'

IF NOT EXISTS (SELECT loginname FROM master.dbo.syslogins WHERE name = '$UserName$')
BEGIN
	SET @SQL = 'CREATE LOGIN [' + '$UserName$' + '] WITH PASSWORD= N''' + '$UserPass$' + ''', DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF';
	EXEC(@SQL)
END

/* Drop role if exist */
IF ((SELECT DATABASE_PRINCIPAL_ID(@roleName)) IS NULL)
BEGIN
	SET @SQL = 'CREATE ROLE ' + @roleName + ';'
	EXEC(@SQL)
END

SET @SQL = 'GRANT SELECT, INSERT, DELETE, EXECUTE, EXEC TO ' + @roleName + ';'
EXEC(@SQL)

IF ((SELECT DATABASE_PRINCIPAL_ID('$UserName$')) IS NOT NULL)
BEGIN
	SET @SQL = 'DROP USER ' + '$UserName$' + ';'
	EXEC(@SQL)
END

SET @SQL = 'CREATE USER ' + '$UserName$' + ' FOR LOGIN ' + '$UserName$' + ';'
EXEC(@SQL)

/* Add user to the role */ 
SET @SQL = 'EXEC sp_addrolemember ' + @roleName + ', ' + '$UserName$' + ';'
EXEC(@SQL)