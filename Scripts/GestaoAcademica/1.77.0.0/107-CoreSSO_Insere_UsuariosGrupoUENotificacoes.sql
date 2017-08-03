USE [CoreSSO]
GO

BEGIN TRANSACTION
SET XACT_ABORT ON
-- BY Ana Cecília Nascimento, 03/08/2017

-- Guid do grupo UE do notificações
DECLARE @gru_id_UE UNIQUEIDENTIFIER = (SELECT [gru_id]
  FROM [dbo].[SYS_Grupo]
  WHERE sis_id = 219
  AND gru_nome = 'Notificação UE'
  AND gru_situacao <> 3)

--Conferindo o Guid do grupo UE do notificações
SELECT @gru_id_UE AS guid_ue_notificacoes

-- Tabela dos grupos que serão adicionados no grupo UE do notificações
DECLARE @gru_ids TABLE (
					gru_id UNIQUEIDENTIFIER
					, gru_nome VARCHAR(50)
					)

-- Inserindo na tabela os grupos CPs, Diretor e AD do SGP
INSERT INTO @gru_ids SELECT 
	gru_id
	, gru_nome
FROM [dbo].[SYS_Grupo]
WHERE (gru_nome = 'Coordenador Pedagógico'
  OR gru_nome = 'Diretor Escolar'
  OR gru_nome = 'Assistente de Diretor na UE')
  AND sis_id = 102
  AND gru_situacao <> 3

--Conferindo se pegou 3 grupos (CPs, Diretor e AD do SGP) 
SELECT COUNT(gru_id) AS qtd_grupos FROM @gru_ids 

--Tabela dos usuários que fazem parte dos 3 grupos acima
DECLARE @usuarios TABLE (
					usu_id UNIQUEIDENTIFIER 
					)

-- Inserindo na tabela os usuários dos grupos CPs, Diretor e AD do SGP
INSERT INTO @usuarios 
SELECT usu.[usu_id]
  FROM [dbo].[SYS_UsuarioGrupo] usu
  INNER JOIN [dbo].[SYS_Usuario] u
  ON u.usu_id = usu.usu_id
  AND u.usu_situacao <> 3
  WHERE usu.usg_situacao <> 3
  AND (gru_id IN (SELECT gru_id FROM @gru_ids)
  AND gru_id <> @gru_id_UE)
  GROUP BY usu.usu_id

--Conferindo a quantidade de usuários
SELECT COUNT(usu_id) AS qtd_usuarios_a_inserir FROM @usuarios 

--Inserindo na tabela [SYS_UsuarioGrupo] os usuários dos grupos CPs, Diretor e AD do SGP para o grupo UE do notificações
INSERT INTO [dbo].[SYS_UsuarioGrupo]
SELECT usu_id
      ,@gru_id_UE
      ,1
  FROM @usuarios u
  WHERE NOT EXISTS (SELECT usu_id FROM [SYS_UsuarioGrupo] ug WHERE u.usu_id = ug.usu_id AND ug.gru_id = @gru_id_UE)

-- Conferindo a quantidade de usuários inseridos (se bate com a quantidade de usuários dos grupos CPs, Diretor e AD do SGP)
SELECT COUNT(gru_id) AS qtd_usuarios_inseridos FROM SYS_UsuarioGrupo WHERE gru_id = @gru_id_UE 

--Tabela dos usuários UA que fazem parte dos grupos CPs, Diretor e AD do SGP
DECLARE @usuariosUA TABLE (
			usu_id uniqueidentifier           
           ,ent_id uniqueidentifier
           ,uad_id uniqueidentifier
					)

--Inserindo na tabela os usuários dos grupos CPs, Diretor e AD do SGP
INSERT INTO @usuariosUA 
SELECT
	usuUA.usu_id    
    ,usuUA.ent_id
    ,uad_id
  FROM [dbo].[SYS_UsuarioGrupoUA] usuUA
  INNER JOIN [dbo].[SYS_Usuario] u
  ON u.usu_id = usuUA.usu_id
  AND u.usu_situacao <> 3
  INNER JOIN  [dbo].[SYS_UsuarioGrupo] usg
  ON usuUA.usu_id = usg.usu_id
  AND usuUA.gru_id = usg.gru_id
  AND usg.usg_situacao <> 3
  WHERE (usuUA.gru_id IN (SELECT gru_id FROM @gru_ids)
  AND usuUA.gru_id <> @gru_id_UE)
  GROUP BY usuUA.usu_id, usuUA.ent_id, usuUA.uad_id

--Conferindo a quantidade de usuários
SELECT COUNT(usu_id) AS qtd_usuariosUA_a_inserir FROM @usuariosUA

--Inserindo na tabela [SYS_UsuarioGrupoUA] os usuários dos grupos CPs, Diretor e AD do SGP para o grupo UE do notificações
INSERT INTO [dbo].[SYS_UsuarioGrupoUA] (usu_id, gru_id, ent_id, uad_id)
           SELECT usu_id
           ,@gru_id_UE
           ,ent_id
           ,uad_id
		   FROM @usuariosUA u
		   WHERE NOT EXISTS (SELECT usu_id FROM [SYS_UsuarioGrupoUA] uUa WHERE u.usu_id = uUa.usu_id AND u.uad_id = uUa.uad_id AND uUa.gru_id = @gru_id_UE)
		   AND EXISTS (SELECT usu_id FROM SYS_UsuarioGrupo ug WHERE ug.usu_id = u.usu_id AND ug.gru_id = @gru_id_UE)

--Conferindo a quantidade de usuários inseridos (se bate com a quantidade de usuários dos grupos CPs, Diretor e AD do SGP)
SELECT COUNT(gru_id) AS qtd_usuariosUA_inseridos FROM SYS_UsuarioGrupoUA WHERE gru_id = @gru_id_UE

SET XACT_ABORT OFF
COMMIT TRANSACTION	

GO
