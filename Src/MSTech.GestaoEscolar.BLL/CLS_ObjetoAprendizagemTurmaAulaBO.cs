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
    using System.Collections.Generic;
    using Data.Common;
    using System.Linq;    /// <summary>
                          /// Description: CLS_ObjetoAprendizagemTurmaAula Business Object. 
                          /// </summary>
    public class CLS_ObjetoAprendizagemTurmaAulaBO : BusinessBase<CLS_ObjetoAprendizagemTurmaAulaDAO, CLS_ObjetoAprendizagemTurmaAula>
    {
        /// <summary>
        /// Seleciona os objetos de aprendizagem ligados à aula da disciplina
        /// </summary>
        /// <param name="tud_id">ID da turma disciplina</param>
        /// <param name="tau_id">ID da aula</param>
        /// <returns></returns>
        public static DataTable SelecionaObjTudTau(long tud_id, int tau_id)
        {
            CLS_ObjetoAprendizagemTurmaAulaDAO dao = new CLS_ObjetoAprendizagemTurmaAulaDAO();
            return dao.SelecionaObjTudTau(tud_id, tau_id);
        }

        public static void SalvarLista(List<CLS_ObjetoAprendizagemTurmaAula> listObjTudTau, TalkDBTransaction banco = null)
        {
            DeletarObjTud(listObjTudTau.First().tud_id, listObjTudTau.First().tau_id, banco);

            foreach (CLS_ObjetoAprendizagemTurmaAula oaa in listObjTudTau)
                Save(oaa, banco);
        }

        /// <summary>
        /// Deleta todos os relacionamentos da turma aula com objetos de aprendizagem
        /// </summary>
        /// <param name="tud_id">ID da turma disciplina</param>
        /// <param name="tau_id">ID da aula</param>
        /// <param name="banco">Transação do banco</param>
        public static void DeletarObjTud(long tud_id, int tau_id, TalkDBTransaction banco = null)
        {
            CLS_ObjetoAprendizagemTurmaAulaDAO dao = new CLS_ObjetoAprendizagemTurmaAulaDAO();
            if (banco != null)
                dao._Banco = banco;
            dao.DeletarObjTud(tud_id, tau_id);
        }
    }
}