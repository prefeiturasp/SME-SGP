DECLARE
	@roleName NVARCHAR(200) = 'db_executor'
	,@userName NVARCHAR(200) = '$SystemUser$'

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

/* ADD USER TO THE ROLE */ 
SET @EXEC = 'EXEC sp_addrolemember ' + @roleName + ', ' + @userName + ';'
EXEC(@EXEC)