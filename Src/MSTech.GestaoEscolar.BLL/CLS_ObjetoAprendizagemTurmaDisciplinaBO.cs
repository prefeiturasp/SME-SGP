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
        /// <param name="tud_ids">IDs da turma disciplina</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="banco">Transação do banco</param>
        public static void SalvarLista(List<CLS_ObjetoAprendizagemTurmaDisciplina> listObjTudDis, List<long> tud_ids, int cal_id, TalkDBTransaction banco = null, long tud_idRegencia = -1)
        {
            CLS_ObjetoAprendizagemTurmaDisciplinaDAO dao = new CLS_ObjetoAprendizagemTurmaDisciplinaDAO();
            if (banco != null)
                dao._Banco = banco;
            else
                dao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                foreach (long tud_id in tud_ids)
                    DeletarObjTud(tud_id, dao._Banco);

                foreach (CLS_ObjetoAprendizagemTurmaDisciplina oad in listObjTudDis)
                    Save(oad, dao._Banco);

                GestaoEscolarUtilBO.LimpaCache("Cache_SelecionaTipoDisciplinaObjetosAprendizagem");

                if (ACA_FormatoAvaliacaoBO.CarregarPorTud(tud_ids.First(), dao._Banco).fav_fechamentoAutomatico)
                {
                    List<int> lstTpc = ACA_TipoPeriodoCalendarioBO.CarregarPeriodosAteDataAtual(cal_id, tud_ids.First())
                                        .AsEnumerable().Select(p => new { tpc_id = Convert.ToInt32(p["tpc_id"]) })
                                        .GroupBy(p => p.tpc_id).Select(p => p.Key).ToList();

                    foreach (int tpc_id in lstTpc)
                    {
                        foreach (long tud_id in tud_ids)
                            CLS_AlunoFechamentoPendenciaBO.SalvarFilaPendencias(tud_id, tpc_id, dao._Banco);

                        if (tud_idRegencia > 0)
                        {
                            CLS_AlunoFechamentoPendenciaBO.SalvarFilaPendencias(tud_idRegencia, tpc_id, dao._Banco);
                        }
                    }
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
            GestaoEscolarUtilBO.LimpaCache("Cache_SelecionaTipoDisciplinaObjetosAprendizagem");

            CLS_ObjetoAprendizagemTurmaDisciplinaDAO dao = new CLS_ObjetoAprendizagemTurmaDisciplinaDAO();
            if (banco != null)
                dao._Banco = banco;
            dao.DeletarObjTud(tud_id);
        }
    }
}