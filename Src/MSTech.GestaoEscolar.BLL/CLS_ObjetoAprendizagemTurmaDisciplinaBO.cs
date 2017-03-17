/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System;
    using System.Data;

    /// <summary>
    /// Description: CLS_ObjetoAprendizagemTurmaDisciplina Business Object. 
    /// </summary>
    public class CLS_ObjetoAprendizagemTurmaDisciplinaBO : BusinessBase<CLS_ObjetoAprendizagemTurmaDisciplinaDAO, CLS_ObjetoAprendizagemTurmaDisciplina>
    {
        /// <summary>
        /// Seleciona os objetos de aprendizagem ligados à disciplina e período do calendário
        /// </summary>
        /// <param name="tud_id">ID da turma disciplina</param>
        /// <param name="tpc_id">ID do período do calendário</param>
        /// <returns></returns>
        public static DataTable SelecionaObjTudTpc(long tud_id, int tpc_id)
        {
            CLS_ObjetoAprendizagemTurmaDisciplinaDAO dao = new CLS_ObjetoAprendizagemTurmaDisciplinaDAO();
            return dao.SelecionaObjTudTpc(tud_id, tpc_id);
        }
    }
}