using System;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL.Abstracts;

namespace MSTech.GestaoEscolar.DAL
{
    public class RHU_ColaboradorFuncaoDAO : Abstract_RHU_ColaboradorFuncaoDAO
    {
        /// <summary>
        /// Verifica o código da última função cadastrada por colaborador/função
        /// filtradas por colaborador e função
        /// </summary>        
        /// <param name="col_id">Id da tabela RHU_Colaborador do bd</param>
        /// <param name="fun_id">Id da tabela RHU_ColaboradorFuncao do bd</param>
        /// <returns>cof_id + 1</returns>
        public int SelectBy_col_id_fun_id_top_one
        (
            long col_id
            , int fun_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_RHU_ColaboradorFuncao_SelectBy_col_id_fun_id_top_one", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@col_id";
                Param.Size = 4;
                if (col_id > 0)
                    Param.Value = col_id;
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

                #endregion

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                    return Convert.ToInt32(qs.Return.Rows[0]["cof_id"].ToString()) + 1;
                else
                    return 1;
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
        /// Retorno booleano na qual verifica se já existe um resposável
        /// pela UA dentro de um determinado período           
        /// </summary>
        /// <returns>True/False</returns>
        public bool SelectBy_VigenciaResponsavelUA
        (
            long col_id
            , int fun_id
            , int cof_id
            , Guid ent_id
            , Guid uad_id
            , DateTime cof_vigenciaInicio
            , DateTime cof_vigenciaFim
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_RHU_ColaboradorFuncao_VigenciaResponsavelUA", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@col_id";
                Param.Size = 8;
                Param.Value = col_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@fun_id";
                Param.Size = 4;
                Param.Value = fun_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cof_id";
                Param.Size = 4;
                if (cof_id > 0)
                    Param.Value = cof_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_id";
                Param.Size = 16;
                Param.Value = uad_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@cof_vigenciaInicio";
                Param.Size = 20;
                Param.Value = cof_vigenciaInicio;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@cof_vigenciaFim";
                Param.Size = 20;
                if (cof_vigenciaFim != new DateTime())
                    Param.Value = cof_vigenciaFim;
                else
                    Param.Value = DBNull.Value;
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

        #region Métodos Sobrescritos

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade 
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        /// <param name="entity"></param>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, RHU_ColaboradorFuncao entity)
        {
            entity.cof_id = Convert.ToInt32(qs.Return.Rows[0][0]);
            return (entity.cof_id > 0);
        }

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, RHU_ColaboradorFuncao entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@cof_vigenciaInicio"].DbType = DbType.DateTime;
            qs.Parameters["@cof_vigenciaFim"].DbType = DbType.DateTime;

            qs.Parameters["@cof_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@cof_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, RHU_ColaboradorFuncao entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters["@cof_vigenciaInicio"].DbType = DbType.DateTime;
            qs.Parameters["@cof_vigenciaFim"].DbType = DbType.DateTime;

            qs.Parameters.RemoveAt("@cof_dataCriacao");
            qs.Parameters["@cof_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity"> Entidade RHU_ColaboradorFuncao</param>
        /// <returns>true = sucesso | false = fracasso</returns>  
        protected override bool Alterar(RHU_ColaboradorFuncao entity)
        {
            __STP_UPDATE = "NEW_RHU_ColaboradorFuncao_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        protected override void ParamDeletar(QueryStoredProcedure qs, RHU_ColaboradorFuncao entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@col_id";
            Param.Size = 8;
            Param.Value = entity.col_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@fun_id";
            Param.Size = 4;
            Param.Value = entity.fun_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cof_id";
            Param.Size = 4;
            Param.Value = entity.cof_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@cof_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@cof_dataAlteracao";
            Param.Size = 8;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity"> Entidade RHU_ColaboradorFuncao</param>
        /// <returns>true = sucesso | false = fracasso</returns>        
        public override bool Delete(RHU_ColaboradorFuncao entity)
        {
            __STP_DELETE = "NEW_RHU_ColaboradorFuncao_Update_Situacao";
            return base.Delete(entity);
        }

        #endregion
    }
}
