/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System.ComponentModel;
    using System.Data;
    using MSTech.Data.Common;

    /// <summary>
    /// Description: ACA_TipoDisciplinaDeficiencia Business Object. 
    /// </summary>
    public class ACA_TipoDisciplinaDeficienciaBO : BusinessBase<ACA_TipoDisciplinaDeficienciaDAO, ACA_TipoDisciplinaDeficiencia>
    {
        /// <summary>
        /// Exclusão fisicamente por tipo de disciplina
        /// </summary>
        /// <param name="tds_id">Id do tipo da disciplina</param>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static bool DeleteBy_TipoDisciplinaDeficiencia(int tds_id, TalkDBTransaction banco = null)
        {
            ACA_TipoDisciplinaDeficienciaDAO dao = banco == null ? new ACA_TipoDisciplinaDeficienciaDAO() : new ACA_TipoDisciplinaDeficienciaDAO() { _Banco = banco };
            return dao.DeleteBy_TipoDisciplinaDeficiencia(tds_id);
        }

        /// <summary>
        /// Retorna todos os tipos de deficiencias de acordo com o tipo de disciplina
        /// </summary>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelectBy_ACA_TipoDisciplina_ACA_TipoDisciplinaDeficiencia(int tds_id)
        {
            ACA_TipoDisciplinaDeficienciaDAO dao = new ACA_TipoDisciplinaDeficienciaDAO();
            DataTable dt = dao.SelectBy_ACA_TipoDisciplina_ACA_TipoDisciplinaDeficiencia(tds_id);

            totalRecords = dt.Rows.Count;

            return dt;
        }
    }
}