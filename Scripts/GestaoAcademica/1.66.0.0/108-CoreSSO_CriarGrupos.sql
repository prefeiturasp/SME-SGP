USE [CoreSSO]
GO

--Iniciar transação
BEGIN TRANSACTION
SET XACT_ABORT ON

	DECLARE @nomeSistema VARCHAR(100) = ' SGP'
	DECLARE @sis_id INT = (SELECT TOP 1 sis_id FROM SYS_Sistema AS Sis WITH(NOLOCK) WHERE Sis.sis_nome = @nomeSistema AND Sis.sis_situacao <> 3)
	DECLARE @dataAtual DATETIME = GETDATE()

	DECLARE @tbGrupos AS TABLE (
		id UNIQUEIDENTIFIER NOT NULL,
		semGrupoOrigem BIT NOT NULL,
		vis_id INT NOT NULL DEFAULT(3),
		gru_nomeOrigemCopia VARCHAR(50) NOT NULL,
		gru_nomeNovo VARCHAR(50) NOT NULL,
		chaveGrupoPadrao VARCHAR(100) NULL,
		gru_idOrigemCopia UNIQUEIDENTIFIER NULL,
		gru_idNovo UNIQUEIDENTIFIER NULL
	)

	DECLARE @tbPermissao AS TABLE (
		gru_nome VARCHAR(50) NOT NULL,
		mod_nome VARCHAR(50) NOT NULL,
		grp_consultar BIT NOT NULL,
		grp_inserir BIT NOT NULL,
		grp_alterar BIT NOT NULL,
		grp_excluir BIT NOT NULL
	)

	INSERT INTO @tbGrupos (id, semGrupoOrigem, gru_nomeOrigemCopia, gru_nomeNovo, chaveGrupoPadrao)
	VALUES 
		(NEWID(), 0, 'Secretário Escolar', 'Secretário Escolar Infantil', NULL),
		(NEWID(), 0, 'Diretor Escolar', 'Diretor Escolar Infantil Terceirizada', 'padrao_diretor_terceirizado'),
		(NEWID(), 0, 'Secretário Escolar', 'Secretário Escolar Infantil Terceirizado', 'padrao_secretario_terceirizado'),
		(NEWID(), 0, 'Docente - CJ e outros', 'Docente - CJ e outros terceirizado', 'padrao_docente_terceirizado')

	INSERT INTO @tbPermissao(gru_nome, mod_nome, grp_consultar, grp_inserir, grp_alterar, grp_excluir)
	VALUES
		--> Secretário Escolar Infantil - Retirar as permissões no Histórico
		('Secretário Escolar Infantil', 'Histórico escolar', 0, 0, 0, 0),
		('Secretário Escolar Infantil', 'Dados do aluno', 0, 0, 0, 0),
		('Secretário Escolar Infantil', 'Ensino fundamental', 0, 0, 0, 0),
		('Secretário Escolar Infantil', 'Transferência', 0, 0, 0, 0),
		('Secretário Escolar Infantil', 'Informações complementares', 0, 0, 0, 0),
		--> Secretário Escolar Infantil - Retirar as permissões para Divergências das rematrículas
		('Secretário Escolar Infantil', 'Divergências das rematrículas', 0, 0, 0, 0),

		--> Diretor Escolar Infantil Terceirizada - Incluir permissão de inclusão/edição/consulta e exclusão para Cadastro de Docente e Cadastro de Funcionários
		('Diretor Escolar Infantil Terceirizada', 'Administração', 1, 1, 1, 1),
		('Diretor Escolar Infantil Terceirizada', 'Docentes', 1, 1, 1, 1),
		('Diretor Escolar Infantil Terceirizada', 'Recursos humanos', 1, 1, 1, 1),
		('Diretor Escolar Infantil Terceirizada', 'Funcionários', 1, 1, 1, 1),
		('Diretor Escolar Infantil Terceirizada', 'Manutenção de Docentes', 1, 1, 1, 1),
		--> Diretor Escolar Infantil Terceirizada - Incluir permissão de inclusão/edição/consulta e exclusão para Atribuição de Docente
		('Diretor Escolar Infantil Terceirizada', 'Atribuição de Docentes', 1, 1, 1, 1),
		--> Diretor Escolar Infantil Terceirizada - Retirar as permissões no Histórico
		('Diretor Escolar Infantil Terceirizada', 'Histórico escolar', 0, 0, 0, 0),
		('Diretor Escolar Infantil Terceirizada', 'Dados do aluno', 0, 0, 0, 0),
		('Diretor Escolar Infantil Terceirizada', 'Ensino fundamental', 0, 0, 0, 0),
		('Diretor Escolar Infantil Terceirizada', 'Transferência', 0, 0, 0, 0),
		('Diretor Escolar Infantil Terceirizada', 'Informações complementares', 0, 0, 0, 0),
		--> Diretor Escolar Infantil Terceirizada - Retirar as permissões para Divergências das rematrículas
		('Diretor Escolar Infantil Terceirizada', 'Divergências das rematrículas', 0, 0, 0, 0),
		--> Diretor Escolar Infantil Terceirizada - Retirar as permissões para Atribuição esporádica
		('Diretor Escolar Infantil Terceirizada', 'Atribuição esporádica', 0, 0, 0, 0),

		--> Secretário Escolar Infantil Terceirizado - Incluir permissão de inclusão/edição/consulta e exclusão para Cadastro de Docente e Cadastro de Funcionários
		('Secretário Escolar Infantil Terceirizado', 'Administração', 1, 1, 1, 1),
		('Secretário Escolar Infantil Terceirizado', 'Docentes', 1, 1, 1, 1),
		('Secretário Escolar Infantil Terceirizado', 'Recursos humanos', 1, 1, 1, 1),
		('Secretário Escolar Infantil Terceirizado', 'Funcionários', 1, 1, 1, 1),
		('Secretário Escolar Infantil Terceirizado', 'Manutenção de Docentes', 1, 1, 1, 1),
		--> Secretário Escolar Infantil Terceirizado - Incluir permissão de inclusão/edição/consulta e exclusão para Atribuição de Docente
		('Secretário Escolar Infantil Terceirizado', 'Atribuição de Docentes', 1, 1, 1, 1),
		--> Secretário Escolar Infantil Terceirizado - Retirar as permissões no Histórico
		('Secretário Escolar Infantil Terceirizado', 'Histórico escolar', 0, 0, 0, 0),
		('Secretário Escolar Infantil Terceirizado', 'Dados do aluno', 0, 0, 0, 0),
		('Secretário Escolar Infantil Terceirizado', 'Ensino fundamental', 0, 0, 0, 0),
		('Secretário Escolar Infantil Terceirizado', 'Transferência', 0, 0, 0, 0),
		('Secretário Escolar Infantil Terceirizado', 'Informações complementares', 0, 0, 0, 0),
		--> Secretário Escolar Infantil Terceirizado - Retirar as permissões para Divergências das rematrículas
		('Secretário Escolar Infantil Terceirizado', 'Divergências das rematrículas', 0, 0, 0, 0),
		--> Secretário Escolar Infantil Terceirizado - Retirar as permissões para Atribuição esporádica
		('Secretário Escolar Infantil Terceirizado', 'Atribuição esporádica', 0, 0, 0, 0),

		--> Docente - CJ e outros terceirizado - Incluir permissão de inclusão/edição/consulta e exclusão para Atribuição de Docente
		('Docente - CJ e outros terceirizado', 'Administração', 1, 1, 1, 1),
		('Docente - CJ e outros terceirizado', 'Docentes', 1, 1, 1, 1),
		('Docente - CJ e outros terceirizado', 'Atribuição de Docentes', 1, 1, 1, 1),
		--> Docente - Retirar as permissões no Histórico
		('Docente - CJ e outros terceirizado', 'Histórico escolar', 0, 0, 0, 0),
		('Docente - CJ e outros terceirizado', 'Dados do aluno', 0, 0, 0, 0),
		('Docente - CJ e outros terceirizado', 'Ensino fundamental', 0, 0, 0, 0),
		('Docente - CJ e outros terceirizado', 'Transferência', 0, 0, 0, 0),
		('Docente - CJ e outros terceirizado', 'Informações complementares', 0, 0, 0, 0),
		--> Docente - Retirar as permissões para Divergências das rematrículas
		('Docente - CJ e outros terceirizado', 'Divergências das rematrículas', 0, 0, 0, 0),
		
		-- Grupos não terceirizados - Retirar permissão de inclusão/edição/consulta e exclusão para Cadastro de Docente e Cadastro de Funcionários
		('Secretário Escolar', 'Funcionários', 0, 0, 0, 0),
		('Secretário Escolar', 'Manutenção de Docentes', 0, 0, 0, 0),
		('Diretor Escolar', 'Funcionários', 0, 0, 0, 0),
		('Diretor Escolar', 'Manutenção de Docentes', 0, 0, 0, 0),
		('Docente - CJ e outros', 'Funcionários', 0, 0, 0, 0),
		('Docente - CJ e outros', 'Manutenção de Docentes', 0, 0, 0, 0)

	DECLARE @id UNIQUEIDENTIFIER
	DECLARE @semGrupoOrigem BIT
	DECLARE @gru_nomeOrigemCopia VARCHAR(50)
	DECLARE @gru_nomeNovo VARCHAR(50)
	DECLARE @vis_id INT

	DECLARE cursor_objects CURSOR FOR
		SELECT id, semGrupoOrigem, gru_nomeOrigemCopia, gru_nomeNovo, vis_id
		FROM @tbGrupos

	OPEN cursor_objects
	FETCH NEXT FROM cursor_objects INTO @id, @semGrupoOrigem, @gru_nomeOrigemCopia, @gru_nomeNovo, @vis_id

	WHILE @@FETCH_STATUS = 0
	BEGIN
		DECLARE @gru_id UNIQUEIDENTIFIER = NULL
		DECLARE @gru_idNovo UNIQUEIDENTIFIER = NEWID()

		--Seleciona o ID do grupo de origem.
		SELECT @gru_id = gru_id 
		FROM SYS_Grupo AS Gru WITH(NOLOCK)
		WHERE
			Gru.sis_id = @sis_id
			AND Gru.gru_nome = LTRIM(RTRIM(@gru_nomeOrigemCopia))
			AND Gru.gru_situacao <> 3

		IF (@gru_id IS NOT NULL OR @semGrupoOrigem = 1)
		BEGIN
			IF NOT EXISTS (SELECT TOP 1 Gru.gru_id FROM SYS_Grupo AS Gru WITH(NOLOCK) WHERE Gru.sis_id = @sis_id AND Gru.gru_nome = @gru_nomeNovo AND Gru.gru_situacao <> 3)
			BEGIN
				IF (@semGrupoOrigem = 0)
				BEGIN
					--Insere o grupo
					INSERT INTO SYS_Grupo(gru_id, gru_nome, gru_situacao, gru_dataCriacao, gru_dataAlteracao, vis_id, sis_id, gru_integridade)
					SELECT
						@gru_idNovo, 
						LTRIM(RTRIM(@gru_nomeNovo)), 
						1 AS gru_situacao, 
						@dataAtual, 
						@dataAtual, 
						vis_id, 
						@sis_id, 
						0 AS gru_integridade
					FROM 
						SYS_Grupo AS Gru WITH(NOLOCK)
					WHERE
						Gru.gru_id = @gru_id
						AND Gru.sis_id = @sis_id
				END
				ELSE
				BEGIN
					INSERT INTO SYS_Grupo(gru_id, gru_nome, gru_situacao, gru_dataCriacao, gru_dataAlteracao, vis_id, sis_id, gru_integridade)
					VALUES (
						@gru_idNovo, 
						LTRIM(RTRIM(@gru_nomeNovo)), 
						1, --gru_situacao
						@dataAtual, 
						@dataAtual, 
						@vis_id, 
						@sis_id, 
						0 -- gru_integridade
					)
				END
			END
			ELSE
			BEGIN
				SET @gru_idNovo = (SELECT TOP 1 Gru.gru_id FROM SYS_Grupo AS Gru WITH(NOLOCK) WHERE Gru.sis_id = @sis_id AND Gru.gru_nome = @gru_nomeNovo AND Gru.gru_situacao <> 3)
			END

			IF (@semGrupoOrigem = 0)
			BEGIN
				-- Insere as permissoes do grupo, caso nao exista.
				INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir)
				SELECT DISTINCT
					@gru_idNovo, @sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir 
				FROM 
					SYS_GrupoPermissao AS Grp WITH(NOLOCK)
				WHERE 
					Grp.gru_id = @gru_id
					AND Grp.sis_id = @sis_id
					AND NOT EXISTS (
						SELECT TOP 1 GrpNovo.gru_id 
						FROM SYS_GrupoPermissao AS GrpNovo WITH(NOLOCK) 
						WHERE
							GrpNovo.gru_id = @gru_idNovo
							AND GrpNovo.sis_id = @sis_id
							AND GrpNovo.mod_id = Grp.mod_id
					)
				ORDER BY
					Grp.mod_id
			END
			ELSE
			BEGIN
				INSERT INTO SYS_GrupoPermissao(gru_id, sis_id, mod_id, grp_consultar, grp_inserir, grp_alterar, grp_excluir)
				SELECT DISTINCT
					@gru_idNovo, @sis_id, mod_id, 0 AS grp_consultar, 0 AS grp_inserir, 0 AS grp_alterar, 0 AS grp_excluir 
				FROM 
					SYS_GrupoPermissao AS Grp WITH(NOLOCK)
				WHERE 
					Grp.sis_id = @sis_id
					AND NOT EXISTS (
						SELECT TOP 1 GrpNovo.gru_id 
						FROM SYS_GrupoPermissao AS GrpNovo WITH(NOLOCK) 
						WHERE
							GrpNovo.gru_id = @gru_idNovo
							AND GrpNovo.sis_id = @sis_id
							AND GrpNovo.mod_id = Grp.mod_id
					)
				ORDER BY
					Grp.mod_id
			END

			--Atualiza os ids na tabela de origem
			UPDATE @tbGrupos
			SET 
				gru_idOrigemCopia = @gru_id,
				gru_idNovo = @gru_idNovo
			WHERE
				id = @id
		END
		ELSE
		BEGIN
			PRINT 'O grupo de origem ' + @gru_nomeOrigemCopia + ' Não existe'
		END

		FETCH NEXT FROM cursor_objects INTO @id, @semGrupoOrigem, @gru_nomeOrigemCopia, @gru_nomeNovo, @vis_id
	END

	CLOSE cursor_objects
	DEALLOCATE cursor_objects 

	-- Inserindo parametro de grupo padrão
	INSERT INTO SYS_ParametroGrupoPerfil(pgs_id, pgs_chave, gru_id, pgs_situacao, pgs_dataCriacao, pgs_dataAlteracao)
	SELECT NEWID(), LTRIM(RTRIM(chaveGrupoPadrao)), gru_idNovo, 1 AS pgs_situacao, @dataAtual, @dataAtual
	FROM @tbGrupos G
	WHERE
		chaveGrupoPadrao IS NOT NULL
		AND NOT EXISTS (
			SELECT Pgs.pgs_id 
			FROM SYS_ParametroGrupoPerfil Pgs WITH(NOLOCK)
			WHERE
				LTRIM(RTRIM(Pgs.pgs_chave)) = LTRIM(RTRIM(chaveGrupoPadrao))
				AND Pgs.gru_id = G.gru_idNovo
				AND Pgs.pgs_situacao <> 3
		)

	--Tratando regras especificas dos grupos
	UPDATE Grp
	SET 
		Grp.grp_consultar = P.grp_consultar, 
		Grp.grp_inserir = P.grp_inserir, 
		Grp.grp_alterar = P.grp_alterar, 
		Grp.grp_excluir = P.grp_excluir
	FROM 
		@tbPermissao P 
		INNER JOIN SYS_Grupo Gru WITH(NOLOCK)
			ON Gru.gru_nome = LTRIM(RTRIM(P.gru_nome))
			AND Gru.sis_id = @sis_id
			AND Gru.gru_situacao <> 3
		INNER JOIN SYS_GrupoPermissao Grp WITH(NOLOCK)
			ON Grp.gru_id = Gru.gru_id
			AND Grp.sis_id = @sis_id
		INNER JOIN SYS_Modulo AS M WITH(NOLOCK) 
			ON Grp.sis_id = M.sis_id
			AND Grp.mod_id = M.mod_id
			AND LTRIM(RTRIM(P.mod_nome)) = LTRIM(RTRIM(M.mod_nome))
			AND M.mod_situacao <> 3

--Fechar transação
SET XACT_ABORT OFF
COMMIT TRANSACTION