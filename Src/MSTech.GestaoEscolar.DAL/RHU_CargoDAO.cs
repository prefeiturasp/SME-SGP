using System;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL.Abstracts;
using MSTech.GestaoEscolar.Entities;

namespace MSTech.GestaoEscolar.DAL
{
    public class RHU_CargoDAO : Abstract_RHU_CargoDAO
    {
        #region Métodos

        /// <summary>
        /// Retorna todos os cargos do tipo informado.
        /// </summary>
        /// <param name="ent_id"></param>
        /// <param name="crg_tipo"></param>
        /// <returns></returns>
        public DataTable SelectPorTipo(Guid ent_id, byte crg_tipo)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_RHU_Cargo_SelectCargos_PorTipo", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@crg_tipo";
                Param.Size = 1;
                Param.Value = crg_tipo;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return;
            }
            catch
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna os cargos que não foram excluídos logicamente
        /// </summary>      
        /// <param name="ent_id">ID da entidade</param>        
        public DataTable SelectNaoExcluidos(Guid ent_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_RHU_Cargo_SelectNaoExcluidos", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return;
            }
            catch
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna os cargos que não foram excluídos logicamente filtrando por crg_cargoDocente
        /// </summary>
        /// <param name="crg_cargoDocente">Indica se é cargo docente</param>      
        /// <param name="ent_id">ID da entidade</param>        
        public DataTable SelectNaoExcluidosByCargoDocente(bool crg_cargoDocente, Guid ent_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_RHU_Cargo_SelectNaoExcluidosByCargoDocente", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@crg_cargoDocente";
                Param.Size = 1;
                Param.Value = crg_cargoDocente;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return;
            }
            catch
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna os cargos que não foram excluídos logicamente filtrando por crg_controleIntegracao.
        /// </summary>    
        /// <param name="crg_controleIntegracao">Indica se o cargo é controlado pela integração</param>
        /// <param name="ent_id">ID da entidade</param>        
        public DataTable SelecionaVerificandoControleIntegracao(bool crg_controleIntegracao, Guid ent_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_RHU_Cargo_SelectNaoExcluidosBy_ControleIntegracao", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@crg_controleIntegracao";
                Param.Size = 1;
                Param.Value = crg_controleIntegracao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return;
            }
            catch
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna os cargos que não foram excluídos logicamente filtrando por crg_controleIntegracao e crg_cargoDocente.
        /// </summary>
        /// <param name="crg_controleIntegracao">Indica se o cargo é controlado pela integração</param>
        /// <param name="crg_cargoDocente">Indica se é cargo docente</param>
        /// <param name="ent_id">ID da entidade</param>        
        public DataTable SelecionaVerificandoControleIntegracaoPorCargoDocente(bool crg_controleIntegracao, bool crg_cargoDocente, Guid ent_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_RHU_Cargo_SelectNaoExcluidosByCargoDocenteControleIntegracao", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@crg_controleIntegracao";
                Param.Size = 1;
                Param.Value = crg_controleIntegracao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@crg_cargoDocente";
                Param.Size = 1;
                Param.Value = crg_cargoDocente;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return;
            }
            catch
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna os cargos que não foram excluídos logicamente filtrando por crg_controleIntegracao e crg_cargoDocente.
        /// </summary>
        /// <param name="crg_controleIntegracao">Indica se o cargo é controlado pela integração</param>
        /// <param name="crg_cargoDocente">Indica se é cargo docente</param>
        /// <param name="ent_id">ID da entidade</param>
        public DataTable SelecionaCargoByTipoVinculo(int tvi_id, Guid ent_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_RHU_Cargo_SelecionaByTipoVinculo", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tvi_id";
                Param.Size = 4;
                Param.Value = tvi_id;
                qs.Parameters.Add(Param);
            
                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return;
            }
            catch
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna todos os cargos não excluídos logicamente
        /// </summary>
        /// <param name="tvi_id">ID do tipo de vínculo</param>
        /// <param name="crg_nome">Nome do cargo</param>
        /// <param name="crg_codigo">Código do cargo</param>
        /// <param name="crg_cargoDocente">Indica se é cargo docente</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="totalRecords">Total de registros retornado na busca</param>           
        public DataTable SelectBy_Pesquisa
        (
            int tvi_id
            , string crg_nome
            , string crg_codigo
            , byte crg_cargoDocente
            , Guid ent_id
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_RHU_Cargo_SelectBy_Pesquisa", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.ParameterName = "@tvi_id";
                if (tvi_id > 0)
                    Param.Value = tvi_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@crg_nome";
                Param.Size = 100;
                if (!String.IsNullOrEmpty(crg_nome))
                    Param.Value = crg_nome;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@crg_codigo";
                Param.Size = 20;
                if (!String.IsNullOrEmpty(crg_codigo))
                    Param.Value = crg_codigo;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@crg_cargoDocente";
                Param.Size = 1;
                if (crg_cargoDocente == 0)
                    Param.Value = false;
                else if (crg_cargoDocente == 1)
                    Param.Value = true;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.Size = 16;
                Param.ParameterName = "@ent_id";
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();
                totalRecords = qs.Return.Rows.Count;

                return qs.Return;
            }
            catch
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Verifica se já existe um cargo cadastrado com o mesmo nome e entidade
        /// </summary>
        /// <param name="crg_id">Id do cargo</param>        
        /// <param name="crg_nome">Nome do cargo</param>
        /// <param name="ent_id">Entidade do usuário logado</param>        
        public bool SelectBy_Nome
        (
            int crg_id            
            , string crg_nome
            , Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_RHU_Cargo_SelectBy_Nome", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crg_id";
                Param.Size = 4;
                if (crg_id > 0)
                    Param.Value = crg_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@crg_nome";
                Param.Size = 100;
                if (!string.IsNullOrEmpty(crg_nome))
                    Param.Value = crg_nome;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;                
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return (qs.Return.Rows.Count > 0);
            }
            catch
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Verifica se já existe um cargo cadastrado com o mesmo código e entidade
        /// </summary>
        /// <param name="crg_id">Id do cargo</param>
        /// <param name="crg_codigo">Código do cargo</param>
        /// <param name="ent_id">Id da entidade</param>        
        public bool SelectBy_Codigo
        (
            int crg_id            
            , string crg_codigo
            , Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_RHU_Cargo_SelectBy_Codigo", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crg_id";
                Param.Size = 4;
                if (crg_id > 0)
                    Param.Value = crg_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@crg_codigo";
                Param.Size = 20;
                Param.Value = crg_codigo;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return (qs.Return.Rows.Count > 0);
            }
            catch
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Seleciona todos os cargos cadastrados com o mesmo código e entidade
        /// </summary>
        /// <param name="crg_codigo">Código do cargo</param>
        /// <param name="ent_id">Id da entidade</param>        
        public DataTable SelectBy_Codigos(string crg_codigos, Guid ent_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_RHU_Cargo_SelectBy_Codigos", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@crg_codigos";
                Param.Size = 200;
                Param.Value = crg_codigos;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return;
            }
            catch
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Seleciona determinado cargo pelo código.
        /// </summary>
        /// <param name="entity">Entidade cargo</param>
        /// <returns>True = encontrou cargo com o código / False = não encontrou cargo com o código</returns>
        public bool SelectBy_Codigo(RHU_Cargo entity)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_RHU_Cargo_SelectBy_Codigo", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crg_id";
                Param.Size = 4;
                if (entity.crg_id > 0)
                    Param.Value = entity.crg_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@crg_codigo";
                Param.Size = 20;
                Param.Value = entity.crg_codigo;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = entity.ent_id;
                qs.Parameters.Add(Param);
                #endregion

                qs.Execute();

                if (qs.Return.Rows.Count == 1)
                {
                    DataRowToEntity(qs.Return.Rows[0], entity, false);
                    return true;
                }

                return false;
            }
            catch
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

		/// <summary>
		/// Seleciona cargos de acordo com os filtros passados
		/// </summary>
		/// <param name="crg_situacao">Situação do cargo</param>
		/// <param name="crg_cargoDocente">Flag se é cargo de docente</param>
		public DataTable SelectBy_CargoDocente_Situacao
		(
			int crg_situacao
			, bool crg_cargoDocente
			, Guid ent_id
		)
		{
			QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_RHU_Cargo_SelectBy_CargoDocente_Situacao", _Banco);

			try
			{
				#region PARAMETROS

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@crg_situacao";
				Param.Size = 4;
				if (crg_situacao > 0)
					Param.Value = crg_situacao;
				else
					Param.Value = DBNull.Value;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Boolean;
				Param.ParameterName = "@crg_cargoDocente";
				Param.Value = crg_cargoDocente;
				qs.Parameters.Add(Param);

				Param = qs.NewParameter();
				Param.DbType = DbType.Guid;
				Param.ParameterName = "@ent_id";
				Param.Size = 16;
				Param.Value = ent_id;
				qs.Parameters.Add(Param);

				#endregion

				qs.Execute();

				return qs.Return;
			}
			catch
			{
				throw;
			}
			finally
			{
				qs.Parameters.Clear();
			}
		}

        #endregion

        #region Métodos Sobrescritos

        /// <summary>
        /// Override do nome da ConnectionString.
        /// </summary>
        protected override string ConnectionStringName
        {
            get
            {
                return "GestaoEscolar";
            }
        }

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, RHU_Cargo entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@crg_descricao"].DbType = DbType.String;

            qs.Parameters["@crg_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@crg_dataAlteracao"].Value = DateTime.Now;            
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, RHU_Cargo entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters["@crg_descricao"].DbType = DbType.String;

            qs.Parameters.RemoveAt("@crg_dataCriacao");
            qs.Parameters["@crg_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity"> Entidade RHU_Cargo</param>
        /// <returns>true = sucesso | false = fracasso</returns> 
        protected override bool  Alterar(RHU_Cargo entity)
        {
            __STP_UPDATE = "NEW_RHU_Cargo_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        protected override void ParamDeletar(QueryStoredProcedure qs, RHU_Cargo entity)
        {

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@crg_id";
            Param.Size = 4;
            Param.Value = entity.crg_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@crg_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@crg_dataAlteracao";
            Param.Size = 8;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity"> Entidade RHU_Cargo</param>
        /// <returns>true = sucesso | false = fracasso</returns>
        public override bool Delete(RHU_Cargo entity)
        {
            __STP_DELETE = "NEW_RHU_Cargo_UPDATE_Situacao";
            return base.Delete(entity);
        }

        #endregion
    }
}
