using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL.Abstracts;

namespace MSTech.GestaoEscolar.DAL
{
	/// <summary>
	/// 
	/// </summary>
	public class RHU_CargoDisciplinaDAO : Abstract_RHU_CargoDisciplinaDAO
	{
        /// <summary>
        /// Retorna disciplina do cargo
        /// </summary>
        /// <param name="crg_id">Id do cargo</param>
        /// <returns>List contendo os dados das disciplinas do cargo</returns>
        public List<RHU_CargoDisciplina> CarregaCargoDisciplina(int crg_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_RHU_CargoDisciplina_SelectBy_Disciplinas", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crg_id";
                Param.Size = 4;
                Param.Value = crg_id;
                qs.Parameters.Add(Param);
                #endregion

                qs.Execute();

                DataTable dt = qs.Return;
                List<RHU_CargoDisciplina> list = new List<RHU_CargoDisciplina>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in qs.Return.Rows)
                    {
                        RHU_CargoDisciplina entity = new RHU_CargoDisciplina();
                        list.Add(DataRowToEntity(dr, entity));
                    }
                    
                }

                return list;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna disciplina do cargo
        /// </summary>
        /// <param name="crg_id">Id do cargo</param>
        /// <returns>List contendo os dados das disciplinas do cargo</returns>
        public DataTable SelecionaCargoDisciplinaByCrgId(bool controlarOrdem, int crg_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_RHU_CargoDisciplina_SelectBy_Pesquisa", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@controlarOrdem";
                Param.Size = 4;
                Param.Value = controlarOrdem;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crg_id";
                Param.Size = 4;
                Param.Value = crg_id;
                qs.Parameters.Add(Param);
                #endregion

                qs.Execute();              

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// É feita a exclusão física pois a tabela não possui situação
        /// </summary>
        /// <param name="entity"> Entidade RHU_CargoDisciplina</param>
        public bool DeleteBy_Cargo(RHU_CargoDisciplina entity)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_RHU_CargoDisciplina_DeleteBy_Cargo", _Banco);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@crg_id";
            Param.Size = 4;
            Param.Value = entity.crg_id;
            qs.Parameters.Add(Param);

            qs.Execute();

            return true;
        }
    }
}