USE [GestaoPedagogica]
GO

DECLARE @cal_ano INT = 2017
DECLARE @tds_nome VARCHAR(100) = 'Geografia'
DECLARE @nivelEnsino VARCHAR(100) = 'Ensino Médio'

DECLARE @tbObjetos TABLE
(
	ordemEixo INT
	, ordemSubEixo INT
	, ordemObjeto INT
	, descricaoEixo VARCHAR(500)
	, descricaoSubEixo VARCHAR(500)
	, descricaoObjeto VARCHAR(500)
)

DECLARE @ordemEixo INT = 1
DECLARE @ordemSubEixo INT = 1
DECLARE @ordemObjeto INT = 0
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geografia', NULL, 'A perspectiva geográfica')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geografia', NULL, 'Agentes do relevo')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geografia', NULL, 'Astronomia e movimentos terrestres')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geografia', NULL, 'Clima')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geografia', NULL, 'Dinâmica populacional')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geografia', NULL, 'Espaço rural')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geografia', NULL, 'Espaço urbano')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geografia', NULL, 'Evolução geológica da Terra')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geografia', NULL, 'Fontes de energia')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geografia', NULL, 'Formas do relevo')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geografia', NULL, 'Geopolítica e relações internacionais')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geografia', NULL, 'Globalização e desenvolvimento técnico-científico')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geografia', NULL, 'Organização política, territorial e regional brasileira')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geografia', NULL, 'Paisagens vegetais e domínios morfoclimáticos')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geografia', NULL, 'Questão ambiental, mudanças climáticas e desenvolvimento sustentável')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geografia', NULL, 'Recursos hídricos')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geografia', NULL, 'Representações cartográficas')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geografia', NULL, 'Solos')

-- Encontra a disciplina
DECLARE @tbDisciplina TABLE
(
	tds_id INT
)
IF (@nivelEnsino IS NULL)
BEGIN
	INSERT INTO @tbDisciplina
	SELECT tds.tds_id
	FROM ACA_TipoDisciplina tds WITH(NOLOCK)
	WHERE tds.tds_nome = @tds_nome AND tds.tds_situacao <> 3
END
ELSE 
BEGIN
	INSERT INTO @tbDisciplina
	SELECT tds.tds_id
	FROM ACA_TipoDisciplina tds WITH(NOLOCK)
	INNER JOIN ACA_TipoNivelEnsino tne WITH(NOLOCK)
		ON tne.tne_id = tds.tne_id
		AND tne.tne_nome = @nivelEnsino
		AND tne.tne_situacao <> 3
	WHERE tds.tds_nome = @tds_nome AND tds.tds_situacao <> 3
END

DECLARE @tds_id INT
WHILE (EXISTS (SELECT TOP 1 1 FROM @tbDisciplina))
BEGIN
	
	SELECT TOP 1 @tds_id = tds_id
	FROM @tbDisciplina

	DECLARE @dataAtual DATETIME = GETDATE();

	/*
	-- Apaga os registros existentes
	UPDATE ACA_ObjetoAprendizagem
	SET oap_situacao = 3, oap_dataAlteracao = @dataAtual
	WHERE tds_id = @tds_id AND cal_ano = @cal_ano AND oap_situacao <> 3

	UPDATE ACA_ObjetoAprendizagemEixo
	SET oae_situacao = 3, oae_dataAlteracao = @dataAtual
	WHERE tds_id = @tds_id AND cal_ano = @cal_ano AND oae_situacao <> 3
	--
	*/
	
	-- Verifica se os objetos de aprendizagem ainda não foram cadastrados
	-- para a disciplina e ano.
	IF (NOT EXISTS (
		SELECT TOP 1 1 
		FROM ACA_ObjetoAprendizagem WITH(NOLOCK) 
		WHERE tds_id = @tds_id AND cal_ano = @cal_ano AND oap_situacao <> 3
		))
	BEGIN
		--Iniciar transação
		BEGIN TRANSACTION
		SET XACT_ABORT ON

			-- Cadastra os eixos
			INSERT INTO ACA_ObjetoAprendizagemEixo (tds_id, oae_descricao, oae_ordem, oae_situacao, oae_dataCriacao, oae_dataAlteracao, cal_ano)
			SELECT 
				@tds_id
				, descricaoEixo
				, ordemEixo
				, 1
				, @dataAtual
				, @dataAtual
				, @cal_ano
			FROM @tbObjetos
			WHERE descricaoEixo IS NOT NULL
			GROUP BY ordemEixo, descricaoEixo
			ORDER BY ordemEixo

			-- Cadastra os subeixos
			;WITH Subeixos AS
			(
				SELECT ordemEixo, descricaoEixo, ordemSubEixo, descricaoSubEixo
				FROM @tbObjetos
				WHERE descricaoSubEixo IS NOT NULL
				GROUP BY ordemEixo, descricaoEixo, ordemSubEixo, descricaoSubEixo
			)
			INSERT INTO ACA_ObjetoAprendizagemEixo (tds_id, oae_descricao, oae_ordem, oae_idPai, oae_situacao, oae_dataCriacao, oae_dataAlteracao, cal_ano)
			SELECT 
				@tds_id
				, descricaoSubEixo
				, ordemSubEixo
				, CASE WHEN descricaoEixo IS NOT NULL THEN
					(
						SELECT TOP 1 oae_id
						FROM ACA_ObjetoAprendizagemEixo
						WHERE 
						tds_id = @tds_id 
						AND cal_ano = @cal_ano
						AND oae_descricao = descricaoEixo
						AND oae_ordem = ordemEixo
						AND oae_situacao = 1
					)
					ELSE NULL END
				, 1
				, @dataAtual
				, @dataAtual
				, @cal_ano
			FROM Subeixos
			ORDER BY ordemEixo, ordemSubEixo

			-- Cadastra os objetos
			INSERT INTO ACA_ObjetoAprendizagem (tds_id, oap_descricao, oap_situacao, oap_dataCriacao, oap_dataAlteracao, cal_ano, oae_id)
			SELECT 
				@tds_id
				, descricaoObjeto
				, 1
				, @dataAtual
				, @dataAtual
				, @cal_ano
				, CASE WHEN descricaoSubEixo IS NOT NULL THEN
					(
						SELECT TOP 1 oae_id
						FROM ACA_ObjetoAprendizagemEixo
						WHERE 
						tds_id = @tds_id 
						AND cal_ano = @cal_ano
						AND oae_descricao = descricaoSubEixo
						AND oae_ordem = ordemSubEixo
						AND oae_situacao = 1
					)
					ELSE
						CASE WHEN descricaoEixo IS NOT NULL THEN
						(
							SELECT TOP 1 oae_id
							FROM ACA_ObjetoAprendizagemEixo
							WHERE 
							tds_id = @tds_id 
							AND cal_ano = @cal_ano
							AND oae_descricao = descricaoEixo
							AND oae_ordem = ordemEixo
							AND oae_situacao = 1
						)
						ELSE NULL END 			
					END
			FROM @tbObjetos
			WHERE descricaoObjeto IS NOT NULL
			ORDER BY ordemObjeto

			-- Cadastra os tipos de ciclo dos objetos
			INSERT INTO ACA_ObjetoAprendizagemTipoCiclo (oap_id, tci_id)
			SELECT oap.oap_id, tci.tci_id
			FROM @tbObjetos obj
			INNER JOIN ACA_ObjetoAprendizagem oap
				ON oap.tds_id = @tds_id 
				AND oap.cal_ano = @cal_ano
				AND oap.oap_descricao = obj.descricaoObjeto
				AND oap.oap_situacao = 1
				AND 
				(
					(oap.oae_id IS NULL AND obj.descricaoSubEixo IS NULL AND obj.descricaoEixo IS NULL)
					OR 
					(
						obj.descricaoSubEixo IS NOT NULL
						AND oap.oae_id = 
						(
							SELECT TOP 1 oae_id
							FROM ACA_ObjetoAprendizagemEixo
							WHERE 
							tds_id = @tds_id 
							AND cal_ano = @cal_ano
							AND oae_descricao = obj.descricaoSubEixo
							AND oae_ordem = obj.ordemSubEixo
							AND oae_situacao = 1 
						)
					)
					OR 
					(
						obj.descricaoEixo IS NOT NULL
						AND oap.oae_id = 
						(
							SELECT TOP 1 oae_id
							FROM ACA_ObjetoAprendizagemEixo
							WHERE 
							tds_id = @tds_id 
							AND cal_ano = @cal_ano
							AND oae_descricao = obj.descricaoEixo
							AND oae_ordem = obj.ordemEixo
							AND oae_situacao = 1 
						)
					)
				)
			LEFT JOIN ACA_TipoCiclo tci WITH(NOLOCK) 
				ON tci.tci_situacao = 1

		-- Fechar transação	
		SET XACT_ABORT OFF
		COMMIT TRANSACTION
	END	
	ELSE
	BEGIN
		PRINT 'Já existem objetos de conhecimento cadastrados para a disciplina.'
	END 
	
	DELETE TOP(1)
	FROM @tbDisciplina
END