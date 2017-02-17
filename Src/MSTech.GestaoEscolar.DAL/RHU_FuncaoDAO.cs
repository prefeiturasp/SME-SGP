using System;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL.Abstracts;
using MSTech.GestaoEscolar.Entities;

namespace MSTech.GestaoEscolar.DAL
{
    public class RHU_FuncaoDAO : Abstract_RHU_FuncaoDAO
    {
        #region Métodos

        /// <summary>
        /// Retorna todos as funções não excluídas logicamente
        /// </summary>        
        /// <param name="fun_nome">Nome da função</param>
        /// <param name="fun_codigo">Código da função</param>                
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="totalRecords">Total de registros retornado na busca</param>           
        public DataTable SelectBy_Pesquisa
        (
            string fun_nome
            , string fun_codigo
            , Guid ent_id
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_RHU_Funcao_SelectBy_Pesquisa", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@fun_nome";
                Param.Size = 100;
                if (!string.IsNullOrEmpty(fun_nome))
                    Param.Value = fun_nome;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@fun_codigo";
                Param.Size = 20;
                if (!string.IsNullOrEmpty(fun_codigo))
                    Param.Value = fun_codigo;
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
        /// Verifica se já existe uma função cadastrada com o mesmo nome e entidade
        /// </summary>
        /// <param name="fun_id">ID da função</param>
        /// <param name="fun_nome">Nome da função</param>
        /// <param name="ent_id">Entidade do usuário logado</param>        
        public bool SelectBy_Nome
        (
            int fun_id
            , string fun_nome
            , Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_RHU_Funcao_SelectBy_Nome", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@fun_id";
                Param.Size = 4;
                if (fun_id > 0)
                    Param.Value = fun_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@fun_nome";
                Param.Size = 100;
                Param.Value = fun_nome;
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
        /// Verifica se já existe uma função cadastrada com o mesmo código e entidade
        /// </summary>
        /// <param name="fun_id">ID da função</param>
        /// <param name="fun_codigo">Código da função</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        public bool SelectBy_Codigo
        (
            int fun_id
            , string fun_codigo
            , Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_RHU_Funcao_SelectBy_Codigo", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@fun_codigo";
                Param.Size = 20;
                Param.Value = fun_codigo;
                if (!string.IsNullOrEmpty(fun_codigo))
                    Param.Value = fun_codigo;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@fun_id";
                Param.Size = 4;
                if (fun_id > 0)
                    Param.Value = fun_id;
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
        /// Seleciona todos os campos da função, preenche a entidade função, filtrando
        /// pelo código, id e id da entidade.
        /// </summary>
        /// <param name="entity">Entidade função</param>
        /// <returns>True/False</returns>
        public bool SelectBy_Codigo(RHU_Funcao entity)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_RHU_Funcao_SelectBy_Codigo", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@fun_codigo";
                Param.Size = 20;
                Param.Value = entity.fun_codigo;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@fun_id";
                Param.Size = 4;
                if (entity.fun_id > 0)
                    Param.Value = entity.fun_id;
                else
                    Param.Value = DBNull.Value;
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
        protected override void ParamInserir(QuerySelectStoredProcedure qs, RHU_Funcao entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@fun_descricao"].DbType = DbType.String;

            qs.Parameters["@fun_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@fun_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, RHU_Funcao entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters["@fun_descricao"].DbType = DbType.String;

            qs.Parameters.RemoveAt("@fun_dataCriacao");
            qs.Parameters["@fun_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity"> Entidade RHU_Funcao</param>
        /// <returns>true = sucesso | false = fracasso</returns> 
        protected override bool Alterar(RHU_Funcao entity)
        {
            __STP_UPDATE = "NEW_RHU_Funcao_Update";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        protected override void ParamDeletar(QueryStoredProcedure qs, RHU_Funcao entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@fun_id";
            Param.Size = 4;
            Param.Value = entity.fun_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@fun_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@fun_dataAlteracao";
            Param.Size = 8;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity"> Entidade RHU_Funcao</param>
        /// <returns>true = sucesso | false = fracasso</returns>
        public override bool Delete(RHU_Funcao entity)
        {
            __STP_DELETE = "NEW_RHU_Funcao_Update_Situacao";
            return base.Delete(entity);
        }

        #endregion
    }
}
