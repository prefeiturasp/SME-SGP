/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System.Collections.Generic;
    using System.Data;
    using System.ComponentModel;
    using ObjetosSincronizacao.DTO.Saida;
    using System;
    using Caching;
    using Data.Common;
    using System.Web;
    using System.Linq;

    public enum REL_GraficoAtendimentoTipo : byte
    {
        [Description("REL_GraficoAtendimentoBO.REL_GraficoAtendimentoTipo.Barra")]
        Barra = 1
        ,
        [Description("REL_GraficoAtendimentoBO.REL_GraficoAtendimentoTipo.Pizza")]
        Pizza = 2
    }

    public enum REL_GraficoAtendimentoEixoAgrupamento : byte
    {
        [Description("REL_GraficoAtendimentoBO.REL_GraficoAtendimentoEixoAgrupamento.Curso")]
        Curso = 1
        ,
        [Description("REL_GraficoAtendimentoBO.REL_GraficoAtendimentoEixoAgrupamento.Ciclo")]
        Ciclo = 2
        ,
        [Description("REL_GraficoAtendimentoBO.REL_GraficoAtendimentoEixoAgrupamento.PeridoCurso")]
        PeridoCurso = 3
    }
    
    public enum REL_GraficoAtendimentoFiltrosFixos : byte
    {
        [Description("REL_GraficoAtendimentoBO.REL_GraficoAtendimentoFiltrosFixos.PeriodoPreenchimento")]
        PeriodoPreenchimento = 1
        ,
        [Description("REL_GraficoAtendimentoBO.REL_GraficoAtendimentoFiltrosFixos.RacaCor")]
        RacaCor = 2
        ,
        [Description("REL_GraficoAtendimentoBO.REL_GraficoAtendimentoFiltrosFixos.FaixaIdade")]
        FaixaIdade = 3
        ,
        [Description("REL_GraficoAtendimentoBO.REL_GraficoAtendimentoFiltrosFixos.Sexo")]
        Sexo = 4
        ,
        [Description("REL_GraficoAtendimentoBO.REL_GraficoAtendimentoFiltrosFixos.DetalheDeficiencia")]
        DetalheDeficiencia = 5
    }

    public class sComboGraficoAtendimento
    {
        public int gra_id { get; set; }
        public string gra_titulo { get; set; }
    }

    /// <summary>
    /// Description: REL_GraficoAtendimento Business Object. 
    /// </summary>
    public class REL_GraficoAtendimentoBO : BusinessBase<REL_GraficoAtendimentoDAO, REL_GraficoAtendimento>
	{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rea_id"></param>
        /// <param name="gra_titulo"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static DataTable PesquisaGraficoPorRelatorio(int rea_id, string gra_titulo, int currentPage, int pageSize)
        {
            return new REL_GraficoAtendimentoDAO().SelecionaGraficoPorRelatorio(true, currentPage / pageSize, pageSize,rea_id, gra_titulo, out totalRecords);
        }

        /// <summary>
        /// Retorna os dados para renderizar o gráfico de atendimento
        /// </summary>
        /// <param name="gra_id"></param>
        /// <param name="esc_id"></param>
        /// <param name="uni_id"></param>
        /// <param name="cur_id"></param>
        /// <param name="crr_id"></param>
        /// <param name="crp_id"></param>
        /// <returns></returns>
        public static GraficoAtendimentoSaidaDTO SelecionarDadosGrafico
        (
            int gra_id,
            int esc_id,
            int uni_id,
            int cur_id,
            int crr_id,
            int crp_id
        )
        {
            GraficoAtendimentoSaidaDTO entity = new GraficoAtendimentoSaidaDTO();
            using (DataTable dt = new REL_GraficoAtendimentoDAO().SelecionarDadosGrafico(gra_id, esc_id, uni_id, cur_id, crr_id, crp_id))
            {
                if (dt.Rows.Count > 0)
                {
                    entity = dt.Rows[0].ToEntity<GraficoAtendimentoSaidaDTO>();
                    entity.Dados = dt.ToEntityList<GraficoAtendimentoDadoDTO>();
                }
            }

            return entity;
        }

        /// <summary>
        /// Retorna os gráficos de atendimento por tipo de relatório
        /// </summary>
        /// <param name="rea_tipo"></param>
        /// <returns></returns>
        public static List<sComboGraficoAtendimento> SelecionaPorTipoRelatorio(byte rea_tipo, int appMinutosCacheLongo = 0)
        {
            Func<List<sComboGraficoAtendimento>> retorno = delegate ()
            {
                List<sComboGraficoAtendimento> lista = new List<sComboGraficoAtendimento>();
                using (DataTable dt = new REL_GraficoAtendimentoDAO().SelecionaPorTipoRelatorio(rea_tipo))
                {
                    if (dt.Rows.Count > 0)
                    {
                        lista = dt.ToEntityList<sComboGraficoAtendimento>();
                    }
                }

                return lista;
            };

            return CacheManager.Factory.Get
                (
                    string.Format(ModelCache.GRAFICO_ATENDIMENTO_SELECIONA_POR_TIPO_RELATORIO_KEY, rea_tipo)
                    ,
                    retorno
                    ,
                    appMinutosCacheLongo
                );
        }

        public static bool Salvar(REL_GraficoAtendimento entity, List<REL_GraficoAtendimento_FiltrosFixos> lstFiltrosFixos, List<QuestionarioConteudoResposta> lstRespostas, TalkDBTransaction banco = null)
        {
            REL_GraficoAtendimentoDAO dao = new REL_GraficoAtendimentoDAO();
            if (banco == null)
                dao._Banco.Open(IsolationLevel.ReadCommitted);
            else
                dao._Banco = banco;

            try
            {
                //Carrega os filtros fixos ligados ao gráfico (se for um gráfico que já existe)
                List<REL_GraficoAtendimento_FiltrosFixos> lstFiltrosFixosBanco = entity.IsNew ? new List<REL_GraficoAtendimento_FiltrosFixos>() :
                                                            REL_GraficoAtendimento_FiltrosFixosBO.SelectBy_gra_id(entity.gra_id, dao._Banco);

                //Carrega os filtros personalizados ligados ao gráfico (se for um gráfico que já existe)
                List<REL_GraficoAtendimento_FiltrosPersonalizados> lstFiltrosPersonalizadosBanco = entity.IsNew ? new List<REL_GraficoAtendimento_FiltrosPersonalizados>() :
                                                              REL_GraficoAtendimento_FiltrosPersonalizadosBO.SelectBy_gra_id(entity.gra_id, dao._Banco);

                //Salva o gráfico
                if (!dao.Salvar(entity))
                    return false;

                LimpaCache(entity);

                //Salva filtros fixos
                foreach (REL_GraficoAtendimento_FiltrosFixos gff in lstFiltrosFixos)
                {
                    gff.gra_id = entity.gra_id;
                    REL_GraficoAtendimento_FiltrosFixosBO.GetEntity(gff);
                    if (!REL_GraficoAtendimento_FiltrosFixosBO.Save(gff, dao._Banco))
                        return false;
                }

                List<REL_GraficoAtendimento_FiltrosPersonalizados> lstFiltrosPersonalizados = new List<REL_GraficoAtendimento_FiltrosPersonalizados>();
                REL_GraficoAtendimento_FiltrosPersonalizados gfp;
                //Salva filtros personalizados
                foreach (QuestionarioConteudoResposta qcr in lstRespostas)
                {
                    gfp = new REL_GraficoAtendimento_FiltrosPersonalizados
                    {
                        qtr_id = qcr.qtr_id
                        , gra_id = entity.gra_id
                    };
                    REL_GraficoAtendimento_FiltrosPersonalizadosBO.GetEntity(gfp);
                    lstFiltrosPersonalizados.Add(gfp);
                   
                    if (!REL_GraficoAtendimento_FiltrosPersonalizadosBO.Save(gfp, dao._Banco))
                        return false;
                }

                //Remove logicamente no banco os filtros fixos e personalizados que foram removidos do gráfico
                foreach (REL_GraficoAtendimento_FiltrosFixos gffB in lstFiltrosFixosBanco)
                    if (!lstFiltrosFixos.Any(f => f.gff_id == gffB.gff_id && f.gff_situacao != (byte)REL_GraficoAtendimento_FiltrosFixosSituacao.Excluido))
                    {
                        REL_GraficoAtendimento_FiltrosFixosBO.Delete(gffB, dao._Banco);
                    }
                foreach (REL_GraficoAtendimento_FiltrosPersonalizados gfpB in lstFiltrosPersonalizadosBanco)
                    if (!lstFiltrosPersonalizados.Any(f => f.gfp_id == gfpB.gfp_id && f.gfp_situacao != (byte)REL_GraficoAtendimento_FiltrosPersonalizadosSituacao.Excluido))
                    {
                        REL_GraficoAtendimento_FiltrosPersonalizadosBO.Delete(gfpB, dao._Banco);
                    }
                
                return true;
            }
            catch (Exception err)
            {
                if (banco == null)
                    dao._Banco.Close(err);

                throw;
            }
            finally
            {
                if (banco == null)
                    dao._Banco.Close();
            }
        }

        private static void LimpaCache(REL_GraficoAtendimento entity)
        {
            if (HttpContext.Current != null)
            {
                // Chave padrão do cache - nome do método + parâmetros.
                HttpContext.Current.Cache.Remove(RetornaChaveCache_GetEntity(entity));
            }
        }

        private static string RetornaChaveCache_GetEntity(REL_GraficoAtendimento entity)
        {
            return string.Format("REL_GraficoAtendimento_GetEntity_{0}", entity.gra_id);
        }
    }
}