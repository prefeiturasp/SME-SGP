DECLARE
	@roleName sysname = 'db_executor'
	,@userName sysname = '$SystemUser$'

DECLARE
	@EXEC VARCHAR(MAX)

/* CREATE A NEW ROLE */
IF((SELECT DATABASE_PRINCIPAL_ID(@roleName)) IS NULL)
BEGIN 
	SET @EXEC = 'CREATE ROLE ' + @roleName + ';'
	EXEC(@EXEC)
END	
		
/* GRANT EXECUTE TO THE ROLE */
SET @EXEC = 'GRANT SELECT, INSERT, DELETE, EXECUTE, EXEC TO ' + @roleName + ';'
EXEC(@EXEC)

IF NOT EXISTS (SELECT loginname FROM master.dbo.syslogins WHERE name = @userName AND dbname = '$SystemDatabase$')
BEGIN
	CREATE USER [user_gestaoescolar] FOR LOGIN [user_gestaoescolar]
END

/* ADD USER TO THE ROLE */ 
SET @EXEC = 'EXEC sp_addrolemember ' + @roleName + ', ' + @userName + ';'
EXEC(@EXEC)