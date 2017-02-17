using System.Data;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using System.ComponentModel;
using MSTech.Data.Common;
using System.Collections.Generic;
using System.Linq;

namespace MSTech.GestaoEscolar.BLL
{
    public class TUR_TurmaRelTurmaDisciplinaBO : BusinessBase<TUR_TurmaRelTurmaDisciplinaDAO, TUR_TurmaRelTurmaDisciplina>    
    {
        /// <summary>
        /// Retorna as disciplinas da turma.
        /// </summary>
        /// <param name="tur_id">ID da turma.</param>
        /// <param name="banco">Transação com banco</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_tur_id
        (
           long tur_id
            , TalkDBTransaction banco
        )
        {                        
            TUR_TurmaRelTurmaDisciplinaDAO dao = new TUR_TurmaRelTurmaDisciplinaDAO();
         
            if (banco != null)
                dao._Banco = banco;

            return dao.SelectBy_tur_id(tur_id, out totalRecords);
        }

        /// <summary>
        /// Retorna as disciplinas da turma.
        /// </summary>
        /// <param name="tur_id">ID da turma.</param>
        /// <param name="banco">Transação com banco</param>
        /// <returns></returns>
        public static List<TUR_TurmaRelTurmaDisciplina> GetSelectBy_Turmas
        (
           string tur_id
            , TalkDBTransaction banco
        )
        {
            TUR_TurmaRelTurmaDisciplinaDAO dao = new TUR_TurmaRelTurmaDisciplinaDAO
            {
                _Banco = banco
            };

            DataTable dt = dao.SelectBy_Turmas(tur_id);

            return (from DataRow dr in dt.Rows
                    select dao.DataRowToEntity(dr, new TUR_TurmaRelTurmaDisciplina())).ToList();
        }

        /// <summary>
        /// retorna a turma relacionada a disciplina
        /// </summary>
        /// <param name="tud_id"></param>
        /// <returns></returns>
        public static TUR_Turma SelecionarTurmaPorTurmaDisciplina(long tud_id)
        {
            TUR_TurmaRelTurmaDisciplinaDAO dao = new TUR_TurmaRelTurmaDisciplinaDAO();
            DataTable dt = dao.SelecionarTurmaPorTurmaDisciplina(tud_id);

            if (dt.Rows.Count > 0)
                return ((TUR_Turma)(from DataRow dr in dt.Rows
                                    select GestaoEscolarUtilBO.DataRowToEntity(dr, new TUR_Turma())).ToList().FirstOrDefault());

            return null;
        }
    }
}
