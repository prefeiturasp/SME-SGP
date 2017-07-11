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
    using System.Collections.Generic;

    #region Enumeradores

    /// <summary>
    /// Situações do filtro fixo do gráfico
    /// </summary>
    public enum REL_GraficoAtendimento_FiltrosFixosSituacao : byte
    {
        Ativo = 1
        ,

        Excluido = 3
    }

    #endregion Enumeradores


    /// <summary>
    /// Description: REL_GraficoAtendimento_FiltrosFixos Business Object. 
    /// </summary>
    public class REL_GraficoAtendimento_FiltrosFixosBO : BusinessBase<REL_GraficoAtendimento_FiltrosFixosDAO, REL_GraficoAtendimento_FiltrosFixos>
    {

        public new static REL_GraficoAtendimento_FiltrosFixos GetEntityDetalhado(REL_GraficoAtendimento_FiltrosFixos entity)
        {
            GetEntity(entity);

            switch (entity.gff_tipoFiltro)
            {
                case (byte)REL_GraficoAtendimentoFiltrosFixos.DetalheDeficiencia:
                    entity.gff_tituloFiltro = "Detalhamento das deficiências";
                    break;
                case (byte)REL_GraficoAtendimentoFiltrosFixos.FaixaIdade:
                    entity.gff_tituloFiltro = "Faixa de idade";
                    break;
                case (byte)REL_GraficoAtendimentoFiltrosFixos.Sexo:
                    entity.gff_tituloFiltro = "Sexo";
                    break;
                case (byte)REL_GraficoAtendimentoFiltrosFixos.PeriodoPreenchimento:
                    entity.gff_tituloFiltro = "Período do preenchimento do relatório";
                    break;
                case (byte)REL_GraficoAtendimentoFiltrosFixos.RacaCor:
                    entity.gff_tituloFiltro = "Raça/Cor";
                    break;
                default:
                    break;
            }
            entity.gff_valorDetalhado = RetornaValorDetalhado((REL_GraficoAtendimentoFiltrosFixos)entity.gff_tipoFiltro, entity.gff_valorFiltro);

            return entity;
        }

        public new static string RetornaValorDetalhado(REL_GraficoAtendimentoFiltrosFixos tipoFiltro, string valor)
        {
            string[] codDetalhe = valor.Split(',');
            string retorno = string.Empty;
            switch (tipoFiltro)
            {
                case REL_GraficoAtendimentoFiltrosFixos.DetalheDeficiencia:
                    List<string> valoresDetalhados = new List<string>();
                    foreach (var item in codDetalhe)
                    {
                        CFG_DeficienciaDetalhe def = new CFG_DeficienciaDetalhe { dfd_id = Convert.ToInt32(item) };
                        def = CFG_DeficienciaDetalheBO.GetDetalhamento(def);
                        valoresDetalhados.Add(def.dfd_nome);
                    }
                    codDetalhe = valoresDetalhados.ToArray();

                    if (codDetalhe.Length == 1) retorno = codDetalhe[0];
                    else if (codDetalhe.Length == 2) retorno = codDetalhe[0] + " ou " + codDetalhe[1];
                    else if (codDetalhe.Length >= 3)
                    {

                        string[] concat = new string[codDetalhe.Length - 1];
                        Array.Copy(codDetalhe, 0, concat, 0, codDetalhe.Length - 1);

                        retorno = string.Join(", ", concat);
                        retorno += " ou " + codDetalhe[codDetalhe.Length - 1];
                    }
                    else retorno = string.Empty;
                    break;
                case REL_GraficoAtendimentoFiltrosFixos.FaixaIdade:
                    retorno = codDetalhe.Length >= 2 ? codDetalhe[0] + " até " + codDetalhe[1] + " anos" : string.Empty;
                    break;
                case REL_GraficoAtendimentoFiltrosFixos.Sexo:
                    retorno = codDetalhe.Length >= 1 ? MetodosExtensao.SexoFormatado(Convert.ToInt32(codDetalhe[0])) : string.Empty;
                    break;
                case REL_GraficoAtendimentoFiltrosFixos.PeriodoPreenchimento:
                    retorno = codDetalhe.Length >= 2 ? codDetalhe[0] + " até " + codDetalhe[1] : string.Empty;
                    break;
                case REL_GraficoAtendimentoFiltrosFixos.RacaCor:
                    retorno = codDetalhe.Length >= 1 ? MetodosExtensao.RacaCorFormatado(Convert.ToInt32(codDetalhe[0])) : string.Empty;
                    break;
                default:
                    break;
            }
            return retorno;
        }

        public static List<REL_GraficoAtendimento_FiltrosFixos> RetornaListaDetalhada(List<REL_GraficoAtendimento_FiltrosFixos> lstFiltrosFixos)
        {
            foreach (REL_GraficoAtendimento_FiltrosFixos gff in lstFiltrosFixos)
            {
                REL_GraficoAtendimento_FiltrosFixos gffB = new REL_GraficoAtendimento_FiltrosFixos { gff_tipoFiltro = gff.gff_tipoFiltro, gra_id = gff.gra_id };
                GetEntityDetalhado(gffB);

                gff.gff_tipoFiltro = gffB.gff_tipoFiltro;
                gff.gff_tituloFiltro = gffB.gff_tituloFiltro;
                gff.gff_valorDetalhado = gffB.gff_valorDetalhado;
                gff.gff_valorFiltro = gffB.gff_valorFiltro;
            }

            return lstFiltrosFixos;
        }

        public static List<REL_GraficoAtendimento_FiltrosFixos> SelectBy_gra_id(int gra_id, TalkDBTransaction banco = null)
        {
            REL_GraficoAtendimento_FiltrosFixosDAO dao = new REL_GraficoAtendimento_FiltrosFixosDAO();
            if (banco != null)
                dao._Banco = banco;
            List<REL_GraficoAtendimento_FiltrosFixos> lstFiltrosFixos = dao.SelectBy_gra_id(gra_id);

            return RetornaListaDetalhada(dao.SelectBy_gra_id(gra_id));
        }
    }
}