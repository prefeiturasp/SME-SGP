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
    }
}