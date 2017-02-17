/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL.Abstracts;

namespace MSTech.GestaoEscolar.DAL
{
	
	/// <summary>
	/// 
	/// </summary>
	public class MTR_ProcessoFechamentoInicioEtapaDAO : Abstract_MTR_ProcessoFechamentoInicioEtapaDAO
	{
        #region Métodos

        /// <summary>
        /// Verifica se uma etapa pode ser realizada
        /// </summary>
        /// <param name="pfi_id">Id do processo de fechamento/início do ano letivo</param>        
        /// <param name="pfe_tipoEtapa">Tipo da etapa do processo de fechamento/início do ano letivo</param>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="uni_id">Id da unidade da escola</param>
        /// <param name="cur_id">Id do curso</param>
        /// <param name="crr_id">Id do currículo do curso</param>
        /// <param name="crp_id">Id do período do curso</param>        
        /// <returns>True: A etapa está vigente / False: A etapa não está vigente</returns>        
        public bool VerificaEtapaVigente
        (
            byte pfe_tipoEtapa
            , int pfi_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id            
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_ProcessoFechamentoInicioEtapa_VerificaEtapaVigente", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@pfi_id";
                Param.Size = 4;
                Param.Value = pfi_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@pfe_tipoEtapa";
                Param.Size = 1;
                Param.Value = pfe_tipoEtapa;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                if (esc_id > 0)
                    Param.Value = esc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@uni_id";
                Param.Size = 4;
                if (uni_id > 0)
                    Param.Value = uni_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cur_id";
                Param.Size = 4;
                if (cur_id > 0)
                    Param.Value = cur_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crr_id";
                Param.Size = 4;
                if (crr_id > 0)
                    Param.Value = crr_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                if (crp_id > 0)
                    Param.Value = crp_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return.Rows.Count > 0;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }
        
        #endregion     
    }
}
