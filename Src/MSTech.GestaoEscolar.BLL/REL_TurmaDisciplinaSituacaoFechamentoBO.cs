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
	
	/// <summary>
	/// Description: REL_TurmaDisciplinaSituacaoFechamento Business Object. 
	/// </summary>
	public class REL_TurmaDisciplinaSituacaoFechamentoBO : BusinessBase<REL_TurmaDisciplinaSituacaoFechamentoDAO, REL_TurmaDisciplinaSituacaoFechamento>
    {
        #region Consultas

        /// <summary>
        /// Seleciona as pendências de fechamento por disciplinas
        /// </summary>
        /// <param name="listaTurmaDisciplina">lista de turmas disciplinas</param>
        /// <param name="tev_EfetivacaoNotas">Tipo de evento de efetivação de notas.</param>
        /// <returns></returns>
        public static List<REL_TurmaDisciplinaSituacaoFechamento> SelecionaPendenciasFechamentoDisciplinas(List<sTurmaDisciplinaEscolaCalendario> listaTurmaDisciplina, Guid ent_id, int appMinutosCacheLongo = 0)
        {
            List<REL_TurmaDisciplinaSituacaoFechamento> dados = new List<REL_TurmaDisciplinaSituacaoFechamento>();

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
                                         cal_id = tud.cal_id
                                         ,
                                         cacheIsSet = CacheManager.Factory.IsSet(String.Format(ModelCache.PENDENCIA_FECHAMENTO_ESCOLA_TURMA_DISCIPLINA_MODEL_KEY, tud.esc_id, tud.uni_id, tud.cal_id, tud.tud_id))
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
                        dados = new REL_TurmaDisciplinaSituacaoFechamentoDAO().SelecionaPendenciasFechamentoDisciplinas(dt, ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, ent_id));

                        dados.Where(p => p.tud_tipo != (byte)TurmaDisciplinaTipo.ComponenteRegencia).ToList()
                                 .ForEach
                                 (
                                    p =>
                                    {
                                        sTurmaDisciplinaEscolaCalendario turmaDisciplina = listaTurmaDisciplina.FirstOrDefault(t => t.tud_id == p.tud_id);
                                        CacheManager.Factory.Set
                                        (
                                             String.Format(ModelCache.PENDENCIA_FECHAMENTO_ESCOLA_TURMA_DISCIPLINA_MODEL_KEY, turmaDisciplina.esc_id, turmaDisciplina.uni_id, turmaDisciplina.cal_id, turmaDisciplina.tud_id)
                                             ,
                                             p
                                             ,
                                             appMinutosCacheLongo
                                        );
                                    }
                                 );

                        var dadosRegencia = from REL_TurmaDisciplinaSituacaoFechamento pend in dados
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
                                                    String.Format(ModelCache.PENDENCIA_FECHAMENTO_ESCOLA_TURMA_DISCIPLINA_MODEL_KEY, turmaDisciplina.esc_id, turmaDisciplina.uni_id, turmaDisciplina.cal_id, turmaDisciplina.tud_id)
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
                        listaCache.Where(p => p.cacheIsSet && p.tud_tipo != (byte)TurmaDisciplinaTipo.Regencia).Select(p => CacheManager.Factory.Get<REL_TurmaDisciplinaSituacaoFechamento>(String.Format(ModelCache.PENDENCIA_FECHAMENTO_ESCOLA_TURMA_DISCIPLINA_MODEL_KEY, p.esc_id, p.uni_id, p.cal_id, p.tud_id)))
                    );

                    dados.AddRange
                    (
                        listaCache.Where(p => p.cacheIsSet && p.tud_tipo == (byte)TurmaDisciplinaTipo.Regencia).SelectMany(p => CacheManager.Factory.Get<List<REL_TurmaDisciplinaSituacaoFechamento>>(String.Format(ModelCache.PENDENCIA_FECHAMENTO_ESCOLA_TURMA_DISCIPLINA_MODEL_KEY, p.esc_id, p.uni_id, p.cal_id, p.tud_id)))
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

                    dados = new REL_TurmaDisciplinaSituacaoFechamentoDAO().SelecionaPendenciasFechamentoDisciplinas(dt, ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, ent_id));
                }
            }

            return dados;
        }

        #endregion Consultas
    }
}