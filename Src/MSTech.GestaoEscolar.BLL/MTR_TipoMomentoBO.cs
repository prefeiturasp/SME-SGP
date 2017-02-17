/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Data;
using System.ComponentModel;
using System.Collections.Generic;

using MSTech.Data.Common;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL;
using MSTech.Validation.Exceptions;
using MSTech.CoreSSO.BLL;

namespace MSTech.GestaoEscolar.BLL
{
    public enum MTR_TipoMomentoNomes : byte
    {
        PreparacaoAnoLetivo = 1,
        InicioAnoLetivoAntesFechamento = 2,
        FechamentoMatricula = 3,
        InicioAnoLetivoAposFechamento = 4,
        PeriodoCorrecoes = 5
    }

    /// <summary>
    /// MTR_TipoMomento Business Object 
    /// </summary>
    public class MTR_TipoMomentoBO : BusinessBase<MTR_TipoMomentoDAO, MTR_TipoMomento>
    {
        #region [Metodos Relatorios]
        /// <summary>
        /// Seleciona todos os registros da Table [MTR_TipoMomento]
        /// </summary>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTodosTipoMomento()
        {
            MTR_TipoMomentoDAO dao = new MTR_TipoMomentoDAO();
            return dao.Select_MTR_TipoMomento();
        }
        #endregion

        /// <summary>
        /// Retorna os tipos de momentos de acordo com o ano e id do ano       
        /// </summary>
        /// <param name="mom_ano">Ano do tipo de momento</param>
        /// <param name="mom_id">ID do ano do tipo de momento</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaPorAnoEntidade
        (
            int mom_ano
            , int mom_id
            , Guid ent_id
        )
        {
            MTR_TipoMomentoDAO dao = new MTR_TipoMomentoDAO();
            return dao.SelectBy_mom_ano_mom_id(mom_ano, mom_id, ent_id);
        }

        /// <summary>
        /// Retorna os tipos de momentos cadastrados para a entidade, que sejam periódicos.
        /// </summary>
        /// <param name="ent_id">Entidade - obrigatório.</param>
        /// <param name="mom_id"></param>
        /// <param name="mom_ano">Ano - obrigatório</param>
        /// <returns>Tipos de momentos periódicos</returns>
        public static DataTable GetSelectMomentosPeriodicos(int mom_id, int mom_ano, Guid ent_id)
        {
            MTR_TipoMomentoDAO dao = new MTR_TipoMomentoDAO();
            DataTable dt = dao.SelectBy_AnoEntidade(mom_id, mom_ano, ent_id);

            return dt;
        }
        
    }
}
