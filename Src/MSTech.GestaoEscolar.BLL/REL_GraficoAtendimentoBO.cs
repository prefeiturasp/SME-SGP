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
    }
}