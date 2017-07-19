/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
    using System.Collections.Generic;
    using System;
    using System.Data;
    using System.Linq;
    using MSTech.GestaoEscolar.BLL.Caching;

    #region Enumeradores

    /// <summary>
    /// Situações da avaliação disciplina do aluno na turma.
    /// </summary>
    public enum REL_TurmaDisciplinaSituacaoFechamentoTipoPendencia : byte
    {
        SemNota = 1,
        DisciplinaSemAula = 2,
        SemSintese = 3,
        SemResultadoFinal = 4,
        SemParecer = 5,
        PendentePlanejamento = 6,
        SemPlanoAula = 7,
        PendenteRelatorioAEE = 8,
        PendenteRelatorioNAAPA = 9,
        PendenteRelatorioRPPeriodico = 10,
        PendenteRelatorioRPEncerramento = 11
    }

    #endregion

    #region Estruturas

    /// <summary>
    /// Estrutura que armazena as pendências das disciplinas.
    /// </summary>
    [Serializable]
    public struct REL_TurmaDisciplinaSituacaoFechamento_Pendencia
    {
        public long tud_idRegencia { get; set; }
        public long tud_id { get; set; }
        public byte tud_tipo { get; set; }
        public byte tipoPendencia { get; set; }
        public int tpc_id { get; set; }
        public DateTime DataProcessamento { get; set; }
        public int tpc_ordem { get; set; }
        public int tipo_ordem { get; set; }
        public string tud_nome { get; set; }
        public int tds_ordem { get; set; }
    }

    #endregion Estruturas

    /// <summary>
    /// Description: REL_TurmaDisciplinaSituacaoFechamento Business Object. 
    /// </summary>
    public class REL_TurmaDisciplinaSituacaoFechamentoBO : BusinessBase<REL_TurmaDisciplinaSituacaoFechamentoDAO, REL_TurmaDisciplinaSituacaoFechamento>
    {
        #region Consultas

        /// <summary>
        /// Seleciona as pendências das disciplinas.
        /// </summary>
        /// <param name="listaTurmaDisciplina">lista de turmas disciplinas</param>
        /// <param name="tev_EfetivacaoNotas">Tipo de evento de efetivação de notas.</param>
        /// <returns></returns>
        public static List<REL_TurmaDisciplinaSituacaoFechamento_Pendencia> SelecionaPendencias(List<sTurmaDisciplinaEscolaCalendario> listaTurmaDisciplina, Guid ent_id, int appMinutosCacheLongo = 0)
        {
            List<REL_TurmaDisciplinaSituacaoFechamento_Pendencia> dados = new List<REL_TurmaDisciplinaSituacaoFechamento_Pendencia>();

            DataTable dt = new DataTable();
            dt.Columns.Add("tud_id", typeof(Int64));

            if (listaTurmaDisciplina.Any())
            {
                if (appMinutosCacheLongo > 0)
                {
                    var listaCache = from sTurmaDisciplinaEscolaCalendario tud in listaTurmaDisciplina
                                     select new
                                     {
                                         tud_id = tud.tud_id
                                         ,
                                         tud_tipo = tud.tud_tipo
                                         ,
                                         esc_id = tud.esc_id
                                         ,
                                         uni_id = tud.uni_id
                                         ,
                                         cal_ano = tud.cal_ano
                                         ,
                                         cacheIsSet = CacheManager.Factory.IsSet(String.Format(ModelCache.PENDENCIAS_DISCIPLINA_MODEL_KEY, tud.esc_id, tud.uni_id, tud.cal_ano, tud.tud_id))
                                     };

                    listaCache.Where(p => !p.cacheIsSet).ToList().ForEach
                        (
                            p =>
                            {
                                DataRow dr = dt.NewRow();
                                dr["tud_id"] = p.tud_id;
                                if (!dt.AsEnumerable().Any(d => Convert.ToInt64(d["tud_id"]) == p.tud_id))
                                    dt.Rows.Add(dr);
                            }
                        );

                    if (dt.Rows.Count > 0)
                    {
                        DataTable dtPendencias = new REL_TurmaDisciplinaSituacaoFechamentoDAO().SelecionaPendencias(dt, ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, ent_id)); 
                        dados = dtPendencias.Rows.Cast<DataRow>().Select(p => (REL_TurmaDisciplinaSituacaoFechamento_Pendencia)GestaoEscolarUtilBO.DataRowToEntity(p, new REL_TurmaDisciplinaSituacaoFechamento_Pendencia())).ToList();

                        var dadosDisciplina = from REL_TurmaDisciplinaSituacaoFechamento_Pendencia pend in dados
                                                where pend.tud_tipo != (byte)TurmaDisciplinaTipo.ComponenteRegencia
                                                group pend by pend.tud_id into grupo
                                                select new
                                                {
                                                    tud_id = grupo.Key
                                                    ,
                                                    listaPendencias = grupo.ToList()
                                                };

                        dadosDisciplina.ToList()
                                         .ForEach
                                         (
                                            p =>
                                            {
                                                sTurmaDisciplinaEscolaCalendario turmaDisciplina = listaTurmaDisciplina.FirstOrDefault(t => t.tud_id == p.tud_id);
                                                CacheManager.Factory.Set
                                                (
                                                    String.Format(ModelCache.PENDENCIAS_DISCIPLINA_MODEL_KEY, turmaDisciplina.esc_id, turmaDisciplina.uni_id, turmaDisciplina.cal_ano, turmaDisciplina.tud_id)
                                                    ,
                                                    p.listaPendencias
                                                    ,
                                                    appMinutosCacheLongo
                                                );
                                            }
                                         );

                        var dadosRegencia = from REL_TurmaDisciplinaSituacaoFechamento_Pendencia pend in dados
                                            where pend.tud_tipo == (byte)TurmaDisciplinaTipo.ComponenteRegencia
                                            group pend by pend.tud_idRegencia into grupo
                                            select new
                                            {
                                                tud_idRegencia = grupo.Key
                                                ,
                                                listaComponentes = grupo.ToList()
                                            };

                        dadosRegencia.ToList()
                                         .ForEach
                                         (
                                            p =>
                                            {
                                                sTurmaDisciplinaEscolaCalendario turmaDisciplina = listaTurmaDisciplina.FirstOrDefault(t => t.tud_id == p.tud_idRegencia);
                                                CacheManager.Factory.Set
                                                (
                                                    String.Format(ModelCache.PENDENCIAS_DISCIPLINA_MODEL_KEY, turmaDisciplina.esc_id, turmaDisciplina.uni_id, turmaDisciplina.cal_ano, turmaDisciplina.tud_id)
                                                    ,
                                                    p.listaComponentes
                                                    ,
                                                    appMinutosCacheLongo
                                                );
                                            }
                                         );
                    }

                    dados.AddRange
                    (
                        listaCache.Where(p => p.cacheIsSet).SelectMany(p => CacheManager.Factory.Get<List<REL_TurmaDisciplinaSituacaoFechamento_Pendencia>>(String.Format(ModelCache.PENDENCIAS_DISCIPLINA_MODEL_KEY, p.esc_id, p.uni_id, p.cal_ano, p.tud_id)))
                    );
                }
                else
                {
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

                    DataTable dtPendencias = new REL_TurmaDisciplinaSituacaoFechamentoDAO().SelecionaPendencias(dt, ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, ent_id));
                    dados = dtPendencias.Rows.Cast<DataRow>().Select(p => (REL_TurmaDisciplinaSituacaoFechamento_Pendencia)GestaoEscolarUtilBO.DataRowToEntity(p, new REL_TurmaDisciplinaSituacaoFechamento_Pendencia())).ToList();
                }

                if (appMinutosCacheLongo > 0)
                {
                    dt.Clear();
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
                }

                // Adiciona a pendência do plano de aula separado, porque ela aparece de acordo com a data atual,
                // então não fica em cache.
                DataTable dtPendenciasPlanoAula = new CLS_TurmaAulaPendenciaDAO().SelecionaPendencias(dt);
                dados.AddRange(dtPendenciasPlanoAula.Rows.Cast<DataRow>().Select(p => (REL_TurmaDisciplinaSituacaoFechamento_Pendencia)GestaoEscolarUtilBO.DataRowToEntity(p, new REL_TurmaDisciplinaSituacaoFechamento_Pendencia())).ToList());
            }

            return dados;
        }

        #endregion Consultas
    }
}