/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using MSTech.Business.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL;
using System.Data;
using System.Collections.Generic;
using MSTech.Data.Common;

namespace MSTech.GestaoEscolar.BLL
{
	
	/// <summary>
	/// MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodo Business Object 
	/// </summary>
	public class MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodoBO : BusinessBase<MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodoDAO,MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodo>
	{
        public static List<MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodo> GetSelectByTipoMomento(int tmo_id)
        {
            MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodoDAO dao = new MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodoDAO();
            List<MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodo> listaRetorno = new List<MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodo>();
            DataTable dt = dao.GetSelectByTipoMovimentacao(tmo_id);

            foreach (DataRow dr in dt.Rows)
            {
                listaRetorno.Add(dao.DataRowToEntity(dr, new MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodo()));
            }

            return listaRetorno;
        }
        /// <summary>
        /// Retorna os periodos de um calendario para o tipo de movimentaçao do curso momento informado
        /// </summary>
        public static List<MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodo> GetSelectByTipoMovimentacaoCursoMomento(int tmo_id, int cur_id, int crr_id, int tcm_id)
        {
            MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodoDAO dao = new MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodoDAO();
            List<MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodo> listaRetorno = new List<MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodo>();
            DataTable dt = dao.GetSelectByTipoMovimentacaoCursoMomento(tmo_id, cur_id, crr_id, tcm_id);

            foreach (DataRow dr in dt.Rows)
            {
                listaRetorno.Add(dao.DataRowToEntity(dr, new MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodo()));
            }

            return listaRetorno;
        }
        /// <summary>
        /// Override do metodo que retorna os periodos de um calendario para o tipo de movimentaçao do curso momento informado
        /// utilizando o objeto para conexao com o banco TalkDBTransaction.
        /// </summary>
        /// <param name="tmo_id">campo id do tipo movimentaçao</param>
        /// <param name="cur_id">campo id do curso</param>
        /// <param name="crr_id">campo id do curriculo</param>
        /// <param name="tcm_id">campo id do tipo curso momento</param>
        /// <param name="bancoGestao">Transação com banco - obrigatório</param>
        public static List<MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodo> GetSelectByTipoMovimentacaoCursoMomento(int tmo_id, int cur_id, int crr_id, int tcm_id, TalkDBTransaction bancoGestao)
        {
            MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodoDAO dao = new MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodoDAO() { _Banco = bancoGestao };
            List<MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodo> listaRetorno = new List<MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodo>();
            DataTable dt = dao.GetSelectByTipoMovimentacaoCursoMomento(tmo_id, cur_id, crr_id, tcm_id);

            foreach (DataRow dr in dt.Rows)
            {
                listaRetorno.Add(dao.DataRowToEntity(dr, new MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodo()));
            }

            return listaRetorno;
        }

	}
}