using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.GestaoEscolar.DAL.Abstracts;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.Entities;

namespace MSTech.GestaoEscolar.DAL
{
    /// <summary>
    /// 
    /// </summary>
    public class DCL_ProtocoloReprocessoDAO : Abstract_DCL_ProtocoloReprocessoDAO
    {
        protected override string ConnectionStringName
        {
            get
            {
                return "GestaoEscolar";
            }
        }

        /// <summary>
        /// Seleciona o historico de reprocessamento filtrado pelo protocolo
        /// </summary>
        /// <param name="ent_id">Id do protocolo</param>
        /// <returns></returns>
        public DataTable SelectBy_Protocolo(Guid pro_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("STP_DCL_ProtocoloReprocesso_SelectBy_Protocolo", this._Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@pro_id";
                Param.Size = 16;
                Param.Value = pro_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return (qs.Return);
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade 
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, DCL_ProtocoloReprocesso entity)
        {
            entity.prp_seq = Convert.ToInt32(qs.Return.Rows[0][0]);
            return (entity.prp_seq > 0);
        }
    }
}
