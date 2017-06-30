/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System;

    /// <summary>
    /// Description: REL_GraficoAtendimento_FiltrosFixos Business Object. 
    /// </summary>
    public class REL_GraficoAtendimento_FiltrosFixosBO : BusinessBase<REL_GraficoAtendimento_FiltrosFixosDAO, REL_GraficoAtendimento_FiltrosFixos>
    {

        public new static REL_GraficoAtendimento_FiltrosFixos GetEntity(REL_GraficoAtendimento_FiltrosFixos entity)
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
            entity.gff_valorDetalhado = RetornaValor((REL_GraficoAtendimentoFiltrosFixos)entity.gff_tipoFiltro, entity.gff_valorFiltro);

            return entity;
        }

        public new static string RetornaValor(REL_GraficoAtendimentoFiltrosFixos tipoFiltro, string valor)
        {
            string valorDetalhado = "";
            switch (tipoFiltro)
            {
                case REL_GraficoAtendimentoFiltrosFixos.DetalheDeficiencia:
                    string[] codDetalhe = valor.Split(',');
                    foreach (var item in codDetalhe)
                    {
                        CFG_DeficienciaDetalhe def = new CFG_DeficienciaDetalhe { dfd_id = Convert.ToInt32(item) };
                        CFG_DeficienciaDetalheBO.GetEntity(def);
                        valorDetalhado += "," + def.dfd_nome;
                    }
                    valorDetalhado = "";
                    break;
                case REL_GraficoAtendimentoFiltrosFixos.FaixaIdade:
                    valorDetalhado = valor;
                    break;
                case REL_GraficoAtendimentoFiltrosFixos.Sexo:
                    valorDetalhado = MetodosExtensao.SexoFormatado(Convert.ToInt32(valor));
                    break;
                case REL_GraficoAtendimentoFiltrosFixos.PeriodoPreenchimento:
                    valorDetalhado = valor;
                    break;
                case REL_GraficoAtendimentoFiltrosFixos.RacaCor:                   
                    valorDetalhado = MetodosExtensao.RacaCorFormatado(Convert.ToInt32(valor));
                    break;
                default:
                    break;
            }
            return valorDetalhado;
        }
    }
}