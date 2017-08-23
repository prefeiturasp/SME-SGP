-- =============================================
-- Description:	Atualiza os sinônimos de determinado banco de dados
--		Parâmetros: 
--			@database : Obrigatório
--				Nome do banco de dados que terá os sinônimos atualizados (passar nome exato)
--			@anterior : Obrigatório
--				Nome do banco de dados anterior que o sinônimo referencia.  (passar nome exato)
--			@novo : Obrigatório
--				Nome do banco de dados novo que o sinônimo irá referenciar.  (passar nome exato)
-- =============================================

DECLARE
	@database NVARCHAR(128) = '$SystemDatabase$'
	, @anterior NVARCHAR(128) = '$CoreSource$'
	, @novo NVARCHAR(128) = '$CoreTarget$'

	SET NOCOUNT ON;

	DECLARE @exec VARCHAR(MAX) = ''

	CREATE TABLE #tabelaSinonimosBD
	(
		ID INT IDENTITY(1,1),
		name VARCHAR(MAX) NOT NULL,
		base_object_name VARCHAR(MAX) NOT NULL
	)
	--Seleciona os sinônimos que devem ser atualizados
	EXEC (
		'INSERT INTO #tabelaSinonimosBD (name, base_object_name)
		SELECT name, base_object_name FROM ' + @database + '.sys.synonyms
		WHERE base_object_name LIKE ''[[]' + @anterior + ']%'''
	)

	DECLARE @id INT = (SELECT MAX(ID) FROM #tabelaSinonimosBD)
	WHILE (@id > 0)
	BEGIN
	
		SELECT @exec = 	
			'DROP SYNONYM ' + name + ';' + 
			'CREATE SYNONYM ' + name + ' FOR ' + REPLACE(base_object_name, '[' + @anterior + ']', '[' + @novo + ']') + ';'
		FROM
			#tabelaSinonimosBD
		WHERE
			ID = @id	
		
		--Atualiza o sinônimo
		EXEC (@exec)
	
		SET @id = @id - 1
	END
	
	--Mostra a lista de sinônimos atualizados
	EXEC ('
		DECLARE @printSinonimos VARCHAR(MAX) = ''''

		SELECT @printSinonimos += name COLLATE database_default + '' -> '' + base_object_name COLLATE database_default + CHAR(13)
		FROM ' + @database + '.sys.synonyms
		WHERE name COLLATE database_default IN (SELECT name COLLATE database_default FROM #tabelaSinonimosBD)
		ORDER BY name COLLATE database_default
		
		IF (@printSinonimos = '''')
			PRINT ''Nenhum sinônimo foi atualizado''
		ELSE
			PRINT ''Sinônimos atualizados: '' + CHAR(13) + CHAR(13) + @printSinonimos
	')

	DROP TABLE #tabelaSinonimosBD	