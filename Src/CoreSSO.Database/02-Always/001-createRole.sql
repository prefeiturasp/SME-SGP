DECLARE
	@SQL VARCHAR(MAX)
	,@RoleName sysname = N'db_executor'

IF NOT EXISTS (SELECT loginname FROM master.dbo.syslogins WHERE name = '$UserName$')
BEGIN
	SET @SQL = 'CREATE LOGIN [' + '$UserName$' + '] WITH PASSWORD= N''' + '$UserPass$' + ''', DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF';
	EXEC(@SQL)
END

IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = @RoleName AND type = 'R')
BEGIN
	DECLARE @RoleMemberName sysname
	DECLARE Member_Cursor CURSOR FOR
	SELECT [name]
	FROM sys.database_principals 
	WHERE principal_id IN ( 
		SELECT member_principal_id 
		FROM sys.database_role_members 
		WHERE role_principal_id IN (
			SELECT principal_id
			FROM sys.database_principals WHERE [name] = @RoleName  AND type = 'R' ))

	OPEN Member_Cursor;

	FETCH NEXT FROM Member_Cursor
	INTO @RoleMemberName

	WHILE @@FETCH_STATUS = 0
	BEGIN

		EXEC sp_droprolemember @rolename=@RoleName, @membername= @RoleMemberName

		FETCH NEXT FROM Member_Cursor
		INTO @RoleMemberName
	END;

	CLOSE Member_Cursor;
	DEALLOCATE Member_Cursor;
END 

IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = @roleName AND type = 'R')
BEGIN
	SET @SQL = 'DROP ROLE ' + @roleName + ';'
	EXEC(@SQL)
END

IF  NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = @roleName AND type = 'R')
BEGIN
	SET @SQL = 'CREATE ROLE ' + @roleName + ' AUTHORIZATION [dbo];';
	EXEC(@SQL)
END

SET @SQL = 'GRANT SELECT, INSERT, UPDATE, DELETE, EXECUTE, EXEC TO ' + @roleName + ';'
EXEC(@SQL)

IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = '$UserName$' AND type = 'S')
BEGIN
	SET @SQL = 'DROP USER ' + '$UserName$' + ';'
	EXEC(@SQL)
END

SET @SQL = 'CREATE USER ' + '$UserName$' + ' FOR LOGIN ' + '$UserName$' + ';'
EXEC(@SQL)

SET @SQL = 'EXEC sp_addrolemember ' + @roleName + ', ' + '$UserName$' + ';'
EXEC(@SQL)