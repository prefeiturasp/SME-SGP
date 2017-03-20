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
    using Data.Common;
    using System.Collections.Generic;
    using System.Linq;

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

        /// <summary>
        /// Salva os objetos de aprendizagem da turma disciplina
        /// </summary>
        /// <param name="listObjTudDis">Lista de objetos selecionados</param>
        /// <param name="tud_id">ID da turma disciplina</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="banco">Transação do banco</param>
        public static void SalvarLista(List<CLS_ObjetoAprendizagemTurmaDisciplina> listObjTudDis, long tud_id, int cal_id, TalkDBTransaction banco = null)
        {
            CLS_ObjetoAprendizagemTurmaDisciplinaDAO dao = new CLS_ObjetoAprendizagemTurmaDisciplinaDAO();
            if (banco != null)
                dao._Banco = banco;
            else
                dao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                DeletarObjTud(tud_id, dao._Banco);

                foreach (CLS_ObjetoAprendizagemTurmaDisciplina oad in listObjTudDis)
                    Save(oad, dao._Banco);
                
                if (ACA_FormatoAvaliacaoBO.CarregarPorTud(tud_id, dao._Banco).fav_fechamentoAutomatico)
                {
                    List<Struct_ObjetosAprendizagem> lstObjetosAprendizagem = ACA_ObjetoAprendizagemBO.SelectListaBy_TurmaDisciplina(tud_id, cal_id, dao._Banco);

                    foreach (int tpc_id in lstObjetosAprendizagem.GroupBy(p => p.tpc_id).Select(p => p.Key))
                        CLS_AlunoFechamentoPendenciaBO.SalvarFilaPendencias(tud_id, tpc_id, dao._Banco);
                }
            }
            catch (Exception ex)
            {
                if (banco == null)
                    dao._Banco.Close(ex);
                throw;
            }
            finally
            {
                if (banco == null)
                    dao._Banco.Close();
            }
        }

        /// <summary>
        /// Deleta todos os relacionamentos da turma disciplina com objetos de aprendizagem
        /// </summary>
        /// <param name="tud_id">ID da turma disciplina</param>
        /// <param name="banco">Transação do banco</param>
        public static void DeletarObjTud(long tud_id, TalkDBTransaction banco = null)
        {
            CLS_ObjetoAprendizagemTurmaDisciplinaDAO dao = new CLS_ObjetoAprendizagemTurmaDisciplinaDAO();
            if (banco != null)
                dao._Banco = banco;
            dao.DeletarObjTud(tud_id);
        }
    }
}