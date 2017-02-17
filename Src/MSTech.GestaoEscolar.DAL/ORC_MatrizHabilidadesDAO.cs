/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL.Abstracts;
using MSTech.GestaoEscolar.Entities;
using System;
using System.Data;

namespace MSTech.GestaoEscolar.DAL
{
	
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class ORC_MatrizHabilidadesDAO : Abstract_ORC_MatrizHabilidadesDAO
    {
        #region Métodos Consulta

       /// <summary>
       /// Busca as matrizes de habilidade para a tela de busca.
       /// </summary>
       /// <param name="mat_nome">Nome da matriz</param>
       /// <param name="ent_id">ID da entidade</param>
       /// <returns></returns>
        public DataTable BuscaMatrizHabilidades
        (
            string mat_nome
            , Guid ent_id
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ORC_MatrizHabilidades_SelectTelaBusca", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@mat_nome";
                Param.Size = 100;
                if (!String.IsNullOrEmpty(mat_nome))
                    Param.Value = mat_nome;
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
        /// Busca as matrizes de habilidade para a combo do UC de matriz de habilidades.
        /// </summary>        
        /// <param name="ent_id">ID da entidade</param>
        /// <returns></returns>
        public DataTable SelectComboMatrizHabilidades(Guid ent_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ORC_MatrizHabilidades_SelectComboMatrizHabilidades", _Banco);
            try
            {
                #region Parâmetros

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
        /// Busca as matrizes de habilidade para a combo do UC de matriz de habilidades.
        /// </summary>        
        /// <returns></returns>
        public DataTable SelectMatrizHabilidades_ByCursoPeriodoDisciplinaPadrao(int cur_id, int crr_id, int crp_id, int cal_id, int tds_id, bool mat_padrao, Guid ent_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ORC_MatrizHabilidades_SelectBy_CursoPeriodoDisciplinaPadrao", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cur_id";
                Param.Size = 4;
                Param.Value = cur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crr_id";
                Param.Size = 4;
                Param.Value = crr_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                Param.Value = crp_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                Param.Value = cal_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tds_id";
                Param.Size = 4;
                Param.Value = tds_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@mat_padrao";
                Param.Size = 1;
                Param.Value = mat_padrao;
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
        /// Verifica se existe uma matriz cadastrada com esse nome
        /// </summary>
        /// <param name="entity">Entidade ORC_MatrizHabilidades</param>        
        public bool SelectBy_Nome
        (
            ORC_MatrizHabilidades entity
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ORC_MatrizHabilidades_SelectBy_Nome", _Banco);
            try
            {
                #region PARAMETROS

               
                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@mat_nome";
                Param.Size = 100;
                Param.Value = entity.mat_nome;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = entity.ent_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                if (qs.Return.Rows.Count >= 1)
                {
                    entity = DataRowToEntity(qs.Return.Rows[0], entity, false);
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
        /// Override do método inserir
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ORC_MatrizHabilidades entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@mat_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@mat_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Método alterado para que o insert configure corretamente a matriz padrão
        /// </summary>
        /// <param name="entity"> Entidade ORC_Objetivo</param>
        /// <returns>true = sucesso | false = fracasso</returns>  
        protected override bool Inserir(ORC_MatrizHabilidades entity)
        {
            __STP_INSERT = "NEW_ORC_MatrizHabilidades_INSERT";
            return base.Inserir(entity);
        }

        /// <summary>
        /// Override do método alterar
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, ORC_MatrizHabilidades entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@mat_dataCriacao");
            qs.Parameters["@mat_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity"> Entidade ORC_Objetivo</param>
        /// <returns>true = sucesso | false = fracasso</returns>  
        protected override bool Alterar(ORC_MatrizHabilidades entity)
        {
            __STP_UPDATE = "NEW_ORC_MatrizHabilidades_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        protected override void ParamDeletar(QueryStoredProcedure qs, ORC_MatrizHabilidades entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@mat_id";
            Param.Size = 4;
            Param.Value = entity.mat_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@mat_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@mat_dataAlteracao";
            Param.Size = 8;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);

        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity"> Entidade ORC_Objetivo</param>
        /// <returns>true = sucesso | false = fracasso</returns>        
        public override bool Delete(ORC_MatrizHabilidades entity)
        {
            __STP_DELETE = "NEW_ORC_MatrizHabilidades_Update_Situacao";
            return base.Delete(entity);
        }

        #endregion
	}
}