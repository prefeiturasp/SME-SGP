/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System;
    using System.Linq;
    using System.Data;
    using Caching;    /// <summary>
                      /// Situações da movimentação dos tipos de movimentação
                      /// </summary>
    public enum CLS_RelatorioAtendimentoTipo : byte
    {
        [Description("CLS_RelatorioAtendimentoBO.CLS_RelatorioAtendimentoTipo.AEE")]
        AEE = 1
        ,
        [Description("CLS_RelatorioAtendimentoBO.CLS_RelatorioAtendimentoTipo.NAAPA")]
        NAAPA = 2
        ,
        [Description("CLS_RelatorioAtendimentoBO.CLS_RelatorioAtendimentoTipo.RP")]
        RP = 3
    }

    [Serializable]
    public class RelatorioAtendimento : CLS_RelatorioAtendimento
    {
        public List<CLS_RelatorioAtendimentoGrupo> lstGrupoPermissao { get; set; }
        public List<CLS_RelatorioAtendimentoCargo> lstCargoPermissao { get; set; }

        public List<Questionario> lstQuestionario { get; set; }

        public RelatorioAtendimento()
        {
            lstCargoPermissao = new List<CLS_RelatorioAtendimentoCargo>();
            lstGrupoPermissao = new List<CLS_RelatorioAtendimentoGrupo>();
            lstQuestionario = new List<Questionario>();
        }
    }


    /// <summary>
    /// Description: CLS_RelatorioAtendimento Business Object. 
    /// </summary>
    public class CLS_RelatorioAtendimentoBO : BusinessBase<CLS_RelatorioAtendimentoDAO, CLS_RelatorioAtendimento>
	{
        #region Métodos de consulta

        /// <summary>
        /// Carrega os tipos de relatório verificando a permissão do usuário.
        /// </summary>
        /// <param name="usu_id"></param>
        /// <returns></returns>
        public static List<CLS_RelatorioAtendimento> SelecionaPorPermissaoUsuarioTipo(Guid usu_id, CLS_RelatorioAtendimentoTipo rea_tipo)
        {
            return new CLS_RelatorioAtendimentoDAO().SelecionaPorPermissaoUsuarioTipo(usu_id, (byte)rea_tipo).ToEntityList<CLS_RelatorioAtendimento>();
        }

        /// <summary>
        /// Carrega a estrutura do relatório
        /// </summary>
        /// <param name="rea_id"></param>
        /// <param name="usu_id"></param>
        /// <param name="appMinutosCacheLongo"></param>
        /// <returns></returns>
        public static RelatorioAtendimento SelecionaRelatorio(int rea_id, Guid usu_id, int appMinutosCacheLongo = 0)
        {
            Func<RelatorioAtendimento> retorno = delegate ()
            {
                RelatorioAtendimento rel = new RelatorioAtendimento();
                using (DataSet ds = new CLS_RelatorioAtendimentoDAO().SelecionaRelatorio(rea_id, usu_id))
                {
                    using (DataTable dtRelatorio = ds.Tables[0],
                                     dtGrupoPermissao = ds.Tables[1],
                                     dtCargoPermissao = ds.Tables[2],
                                     dtQuestionario = ds.Tables[3],
                                     dtQuestionarioConteudo = ds.Tables[4],
                                     dtQuestionarioResposta = ds.Tables[5])
                    {
                        if (dtRelatorio.Rows.Count > 0)
                        {
                            rel = dtRelatorio.Rows[0].ToEntity<RelatorioAtendimento>();
                            rel.lstGrupoPermissao = dtGrupoPermissao.ToEntityList<CLS_RelatorioAtendimentoGrupo>();
                            rel.lstCargoPermissao = dtCargoPermissao.ToEntityList<CLS_RelatorioAtendimentoCargo>();
                            List<Questionario> lstQuestionario = dtQuestionario.ToEntityList<Questionario>();
                            List<QuestionarioConteudo> lstQuestionarioConteudo = dtQuestionarioConteudo.ToEntityList<QuestionarioConteudo>();
                            List<CLS_QuestionarioResposta> lstQuestionarioResposta = dtQuestionarioResposta.ToEntityList<CLS_QuestionarioResposta>();

                            lstQuestionarioConteudo.ForEach(c => c.lstRepostas = lstQuestionarioResposta.Where(r => r.qtc_id == c.qtc_id).OrderBy(r => r.qtr_ordem).ToList());
                            lstQuestionario.ForEach(q => q.lstConteudo = lstQuestionarioConteudo.Where(c => c.qst_id == q.qst_id).OrderBy(c => c.qtc_ordem).ToList());
                            rel.lstQuestionario = lstQuestionario.OrderBy(q => q.raq_ordem).ToList();
                        }

                        return rel;
                    }
                }
            };

            return CacheManager.Factory.Get
                        (
                            string.Format(ModelCache.RELATORIO_ATENDIMENTO_BUSCA_ESTRUTURA_RELATORIO_KEY, rea_id, usu_id)
                            ,
                            retorno
                            ,
                            appMinutosCacheLongo
                        );
        }

        #endregion 
    }
}