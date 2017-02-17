/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using MSTech.Business.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL;
using System.ComponentModel;
using System;
using System.Data;
using MSTech.Validation.Exceptions;
using System.Collections.Generic;
using MSTech.Data.Common;
using MSTech.CoreSSO.BLL;

namespace MSTech.GestaoEscolar.BLL
{
    /// <summary>
    /// MTR_MomentoCalendarioPeriodo Business Object 
    /// </summary>
    public class MTR_MomentoCalendarioPeriodoBO : BusinessBase<MTR_MomentoCalendarioPeriodoDAO, MTR_MomentoCalendarioPeriodo>
    {
        /// <summary>
        /// Retorna datatable contendo dados referentes a cada periodo de cada calendário da entidade,
        /// filtrado por ano
        /// </summary>
        /// </summary>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="mom_ano">ano base</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaPorCalendarioAnoEntidade(int cal_id, int mom_ano, int mom_id, Guid ent_id)
        {
            MTR_MomentoCalendarioPeriodoDAO dao = new MTR_MomentoCalendarioPeriodoDAO();
            return dao.SelectByCalendarioAnoEntidade(cal_id, mom_ano, mom_id, ent_id);
        }

        /// <summary>
        /// Retorna os períodos do calendário por curso e ano do momento        
        /// </summary>
        /// </summary>                
        /// <param name="cur_id">ID do curso</param>        
        /// <param name="mom_ano">Ano do tipo de momento</param>
        /// <param name="mom_id">ID do ano do tipo de momento</param>
        /// <param name="ent_id">Entidade do usuário logado</param>        
        /// <returns>DataTable com os dados</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaPorCursoAno
        (
            int cur_id
            , int mom_ano
            , int mom_id
            , Guid ent_id
        )
        {
            MTR_MomentoCalendarioPeriodoDAO dao = new MTR_MomentoCalendarioPeriodoDAO();
            return dao.SelectBy_cur_id_mom_ano_mom_id(cur_id, mom_ano, mom_id, ent_id);
        }
    }
}
