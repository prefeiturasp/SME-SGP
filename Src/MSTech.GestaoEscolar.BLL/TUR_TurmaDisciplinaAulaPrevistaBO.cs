/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
    using System.Collections.Generic;
    using MSTech.Data.Common;
    using System;
    using MSTech.Validation.Exceptions;
    using System.Web;
    using MSTech.GestaoEscolar.BLL.Caching;
    using System.Linq;
    using System.Data;

	/// <summary>
	/// Description: TUR_TurmaDisciplinaAulaPrevista Business Object. 
	/// </summary>
	public class TUR_TurmaDisciplinaAulaPrevistaBO : BusinessBase<TUR_TurmaDisciplinaAulaPrevistaDAO, TUR_TurmaDisciplinaAulaPrevista>
    {
        #region Estruturas

        /// <summary>
        /// Estrutura com o quantitativo de aula da disciplina da turma.
        /// </summary>
        public struct QuantitativoTurmaDisciplina
        {
            /// <summary>
            /// Id da disciplina da turma.
            /// </summary>
            public long tud_id { get; set; }

            /// <summary>
            /// Nome da disciplina.
            /// </summary>
            public string tud_nome { get; set; }

            /// <summary>
            /// Id do tipo de período do calendário.
            /// </summary>
            public int tpc_id { get; set; }

            /// <summary>
            /// Descrição do período.
            /// </summary>
            public string cap_descricao { get; set; }

            /// <summary>
            /// Quantidade de aulas previstas.
            /// </summary>
            public int aulasPrevistas { get; set; }

            /// <summary>
            /// Quantidade de aulas dadas.
            /// </summary>
            public int aulasDadas { get; set; }

            /// <summary>
            /// Tipo da turma disciplina
            /// </summary>
            public byte tud_tipo { get; set; }

            /// <summary>
            /// Informa se a experiência está vigente
            /// </summary>
            public bool experienciaVigente { get; set; }
        }

        #endregion

        /// <summary>
        /// Retorna todos os dados de uma mesma disciplina
        /// </summary>
        /// <param name="tud_id"></param>
        /// <returns></returns>
        public static List<TUR_TurmaDisciplinaAulaPrevista> SelecionaPorDisciplina(long tud_id, int appMinutosCacheLongo = 0)
        {
            List<TUR_TurmaDisciplinaAulaPrevista> dados = null;

            Func<List<TUR_TurmaDisciplinaAulaPrevista>> retorno = delegate()
            {
            TUR_TurmaDisciplinaAulaPrevistaDAO dao = new TUR_TurmaDisciplinaAulaPrevistaDAO();
            return dao.SelecionaPorDisciplina(tud_id);
            };

            if (appMinutosCacheLongo > 0)
            {
                string chave = String.Format(ModelCache.TURMA_DISCIPLINA_AULA_PREVISTA_MODEL_KEY, tud_id);

                dados = CacheManager.Factory.Get
                            (
                                chave,
                                retorno,
                                appMinutosCacheLongo
                            );
        }
            else
            {
                dados = retorno();
            }

            return dados;
        
        }

        /// <summary>
        /// Verifica se tem algo lancado para aquela turma
        /// </summary>
        /// <param name="tud_id"></param>
        public static bool VerificaLancamento(long tud_id, long doc_id, long tur_id, int cal_id)
        {
            TUR_TurmaDisciplinaAulaPrevistaDAO dao = new TUR_TurmaDisciplinaAulaPrevistaDAO();
            return dao.VerificaLancamento(tud_id, doc_id, tur_id, cal_id);
        }

        /// <summary>
        /// Salva a lista de aulas previstas, validando as entidades.
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public static bool SalvarAulasPrevistas(List<TUR_TurmaDisciplinaAulaPrevista> lista, List<TUR_TurmaDisciplinaAulaPrevista>  listaProcessarPend, Guid ent_id, int esc_id, long doc_id, bool fechamentoAutomatico)
        {
            TalkDBTransaction banco = new TUR_TurmaDisciplinaAulaPrevistaDAO()._Banco.CopyThisInstance();

            try
            {
                banco.Open();
                bool ret = true;

                foreach (TUR_TurmaDisciplinaAulaPrevista entity in lista)
                {
                    if (!entity.Validate())
                    {
                        throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
                    }

                    ret &= Save(entity, banco);
                }

                foreach (var listaTudTpc in listaProcessarPend.Select(tap => new { tap.tud_id, tap.tud_tipo, tap.tpc_id }).Distinct())
                {
                    // Caso o fechamento seja automático, grava na fila de processamento.
                    if (fechamentoAutomatico && listaProcessarPend.Count > 0 && listaTudTpc.tud_tipo != (byte)TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia && listaTudTpc.tpc_id != ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_PERIODO_CALENDARIO_RECESSO, ent_id))
                    {
                        CLS_AlunoFechamentoPendenciaBO.SalvarFilaFrequencia(listaTudTpc.tud_id, listaTudTpc.tpc_id, banco);
                    }
                }

                if (lista.Count > 0 && ret && HttpContext.Current != null)
                {
                    try
                    {                  
                        // Limpa o cache da lista de turmas, para atualizar o check
                        string chave = TUR_TurmaBO.RetornaChaveCache_GestorMinhaEscola(ent_id, esc_id);
                        HttpContext.Current.Cache.Remove(chave);

                        if (doc_id > 0)
                        {
                            chave = TUR_TurmaBO.RetornaChaveCache_DocenteControleTurmas(ent_id, doc_id);
                            HttpContext.Current.Cache.Remove(chave);
                        }
                        else
                        {
                            GestaoEscolarUtilBO.LimpaCache(string.Format("{0}_{1}", TUR_TurmaDisciplinaBO.Cache_SelecionaPorDocenteControleTurma, ent_id));
                        }

                        // Limpa o cache do fechamento
                        long tud_id = lista[0].tud_id;
                        GestaoEscolarUtilBO.LimpaCache(ModelCache.FECHAMENTO_BIMESTRE_PATTERN_KEY, tud_id.ToString());
                        GestaoEscolarUtilBO.LimpaCache(ModelCache.FECHAMENTO_BIMESTRE_FILTRO_DEFICIENCIA_PATTERN_KEY, tud_id.ToString());
                        GestaoEscolarUtilBO.LimpaCache(string.Format(ModelCache.TURMA_DISCIPLINA_AULA_PREVISTA_MODEL_KEY, tud_id));
                        GestaoEscolarUtilBO.LimpaCache(string.Format(ModelCache.TURMA_SELECIONA_POR_DOCENTE_CONTROLE_TURMA_MODEL_KEY, ent_id.ToString(), doc_id.ToString()));
                    }
                    catch
                    { }
                }

                return ret;
            }
            catch (Exception ex)
            {
                banco.Close(ex);
                throw ex;
            }
            finally
            {
                banco.Close();
            }
        }
    }
}