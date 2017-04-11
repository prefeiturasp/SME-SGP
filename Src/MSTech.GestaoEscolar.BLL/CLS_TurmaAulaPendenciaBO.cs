/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System;
    using Data.Common;
    using System.Linq;
    using System.Collections.Generic;
    using System.Data;/// <summary>
                      /// Description: CLS_TurmaAulaPendencia Business Object. 
                      /// </summary>
    public class CLS_TurmaAulaPendenciaBO : BusinessBase<CLS_TurmaAulaPendenciaDAO, CLS_TurmaAulaPendencia>
    {
        /// <summary>
        /// Atualiza a pendência da aula.
        /// </summary>
        /// <param name="tud_id">Id da disciplina.</param>
        /// <param name="tau_id">Id da aula.</param>
        /// <param name="semPlanoAula">Indica se possui plano de aula.</param>
        public static void AtualizarPendencia(long tud_id, int tau_id, bool semPlanoAula, TalkDBTransaction banco = null)
        {
            CLS_TurmaAulaPendenciaDAO dao = new CLS_TurmaAulaPendenciaDAO();
            if (banco != null)
                dao._Banco = banco;
            dao.AtualizarPendencia(tud_id, tau_id, semPlanoAula);
        }

        /// <summary>
        /// Seleciona as pendências de plano de aula das disciplinas.
        /// </summary>
        /// <param name="listaTurmaDisciplina">lista de turmas disciplinas</param>
        /// <returns></returns>
        public static List<REL_TurmaDisciplinaSituacaoFechamento_Pendencia> SelecionaPendencias(List<sTurmaDisciplinaEscolaCalendario> listaTurmaDisciplina)
        {
            List<REL_TurmaDisciplinaSituacaoFechamento_Pendencia> dados = new List<REL_TurmaDisciplinaSituacaoFechamento_Pendencia>();
            if (listaTurmaDisciplina.Any())
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("tud_id", typeof(Int64));

                listaTurmaDisciplina.ForEach
                       (
                           p =>
                           {
                               DataRow dr = dt.NewRow();
                               dr["tud_id"] = p.tud_id;
                               if (!dt.AsEnumerable().Any(d => Convert.ToInt64(d["tud_id"]) == p.tud_id))
                                   dt.Rows.Add(dr);
                           }
                       );

                DataTable dtPendencias = new CLS_TurmaAulaPendenciaDAO().SelecionaPendencias(dt);
                dados = dtPendencias.Rows.Cast<DataRow>().Select(p => (REL_TurmaDisciplinaSituacaoFechamento_Pendencia)GestaoEscolarUtilBO.DataRowToEntity(p, new REL_TurmaDisciplinaSituacaoFechamento_Pendencia())).ToList();
            }
            return dados;
        }
    }
}