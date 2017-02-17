using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.GestaoEscolar.DAL.Abstracts;
using MSTech.Data.Common;
using System.Data;

namespace MSTech.GestaoEscolar.DAL
{
    /// <summary>
    /// 
    /// </summary>
    public class DCL_AgendaHorarioRequisicaoDAO : Abstract_DCL_AgendaHorarioRequisicaoDAO
    {
        protected override string ConnectionStringName
        {
            get
            {
                return "GestaoEscolar";
            }
        }

        /// <summary>
        /// Buscar horários da agenda
        /// </summary>
        /// <param name="ent_id">Id da entidade</param>
        /// <param name="req_id">Id da requisição</param>
        /// <returns>Horários da agenda</returns>
        public DataTable SelectBy_ent_id_req_id(Guid ent_id, int req_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_DCL_AgendaHorarioRequisicao_LOADBy_ent_id_req_id", this._Banco);

            try
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@req_id";
                Param.Size = 8;
                Param.Value = req_id;
                qs.Parameters.Add(Param);

                qs.Execute();

                return qs.Return;
            }
            catch
            {
                throw;
            }
        }
    }
}
