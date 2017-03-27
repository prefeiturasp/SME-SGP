DECLARE
	@user sysname = '$SystemUser$'
	, @database sysname = '$SystemDatabase$'
 
 DECLARE 
  @CMDUSE VARCHAR(128)
  , @CMD1 VARCHAR(8000)
  , @MAXOID INT
 DECLARE 
  @InfoSchemaType int
  , @OwnerName VARCHAR(128)
  , @ObjectName VARCHAR(128)
  , @ObjectType VARCHAR(128)
  , @ObjectDataType VARCHAR(128)
    
 DECLARE @auxCount INT = 0
 DECLARE @auxCount2 INT = 0
 SELECT  @user = RTRIM(LTRIM(@user))
 SELECT  @auxCount = LEN(@user)
 IF (@database IS NOT NULL
  AND EXISTS ( SELECT *
      FROM   sys.databases
      WHERE  NAME = @database )
  AND @user IS NOT NULL
  AND @auxCount > 0
    ) 
 BEGIN 
  DECLARE @sql NVARCHAR(MAX) = 'SELECT @auxCount2 = COUNT(*) FROM ['+ @database + '].sys.sysusers WHERE NAME = @user '
  EXEC sp_executesql @sql,N'@user sysname, @auxCount2 int OUTPUT',@user,@auxCount2 OUTPUT 
   
  IF (@auxCount2 > 0) 
  BEGIN
   CREATE TABLE #TempAllRoutines
     (    
      OID INT IDENTITY(1,1),
      InfoSchemaType int NOT NULL,
      OwnerName VARCHAR(128) NOT NULL,
      ObjectName VARCHAR(128) NOT NULL,
      ObjectType VARCHAR(128) NOT NULL,
      ObjectDataType VARCHAR(128) NULL,
      ObjectSpecific_Catalog VARCHAR(128) NULL
     )
   EXEC
     ('INSERT INTO #TempAllRoutines (InfoSchemaType, OwnerName, ObjectName,ObjectType,ObjectDataType,ObjectSpecific_Catalog)
    SELECT 1, ROUTINE_SCHEMA, ROUTINE_NAME,ROUTINE_TYPE,DATA_TYPE,SPECIFIC_CATALOG 
    FROM [' 
    + @database + '].INFORMATION_SCHEMA.ROUTINES 
    WHERE 
    ROUTINE_NAME NOT LIKE ''dt_%''
    AND 
    Specific_Name NOT IN ( ''sp_alterdiagram'',
     ''sp_creatediagram'',
     ''sp_dropdiagram'',
     ''sp_helpdiagramdefinition'',
     ''sp_helpdiagrams'',
     ''sp_renamediagram'',
     ''sp_upgraddiagrams'',
     ''fn_diagramobjects'')
    UNION 
    SELECT 2, DOMAIN_SCHEMA, DOMAIN_NAME, DATA_TYPE, null, null
    FROM ['+ @database + '].INFORMATION_SCHEMA.DOMAINS'
     )
   -- 4 - Capture the @MAXOID value
   SELECT    @MAXOID = MAX(OID)
   FROM      #TempAllRoutines
   --SELECT * FROM #TempAllRoutines
   SET @CMDUSE = 'USE [' + @database + '];' 
   -- 5 - WHILE loop
   WHILE (@MAXOID > 0)
   BEGIN
    -- 6 - Initialize the variables
    SELECT  
     @InfoSchemaType = InfoSchemaType,  
     @OwnerName = OwnerName,
     @ObjectName = ObjectName,
     @ObjectType = ObjectType,
     @ObjectDataType = ObjectDataType
    FROM      
     #TempAllRoutines
    WHERE     
     OID = @MAXOID
    -- 7 - Build the string 
    IF (@InfoSchemaType = 1)
     SELECT   
      @CMD1 = ' GRANT '
      + CASE 
       WHEN (@ObjectType = 'FUNCTION' AND @ObjectDataType = 'TABLE') 
       THEN 'SELECT ' 
       ELSE 'EXEC ' END 
      + ' ON ' + '[' + @OwnerName
      + ']' + '.' + '[' + @ObjectName + ']'
      + ' TO [' + @user + ']'
    ELSE IF (@InfoSchemaType = 2)
     SELECT   
      @CMD1 = ' GRANT CONTROL ON TYPE::[' + @OwnerName
      + '].[' + @ObjectName + '] TO [' + @user + ']'
      
    PRINT CAST(@MAXOID AS VARCHAR) + ' - ' + @CMD1
    -- 8 - Execute the string 
    EXEC (@CMDUSE + @CMD1)
    -- 9 - Decrement @MAXOID
    SET @MAXOID = @MAXOID - 1
   END
   -- 10 - Drop the temporary table
   DROP TABLE #TempAllRoutines
  END 
  ELSE 
  BEGIN 
     PRINT 'O usuário ' + @user + ' não existe no banco de dados '+ @database + '!'
  END
 END 
 ELSE 
 BEGIN
  PRINT 'Banco de Dados ou usuário inválido!'
 END

