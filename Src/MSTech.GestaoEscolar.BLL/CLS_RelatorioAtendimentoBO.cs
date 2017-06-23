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
    using Caching;
    using System.Web;
    using Validation.Exceptions;
    /// <summary>
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

    public enum CLS_RelatorioAtendimentoPeriodicidade : byte
    {
        [Description("CLS_RelatorioAtendimentoBO.CLS_RelatorioAtendimentoPeriodicidade.Periodico")]
        Periodico = 1
        ,
        [Description("CLS_RelatorioAtendimentoBO.CLS_RelatorioAtendimentoPeriodicidade.Encerramento")]
        Encerramento = 2
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

        /// <summary>
        /// Pesquisa relatórios por tipo.
        /// </summary>
        /// <returns></returns>
        public static DataTable PesquisaRelatorioPorTipo(byte rea_tipo, int currentPage, int pageSize)
        {
            if (pageSize == 0)
                pageSize = 1;
            DataTable dt = new CLS_RelatorioAtendimentoDAO().PesquisaRelatorioPorTipo(rea_tipo, currentPage / pageSize, pageSize, out totalRecords);
            
            return dt;
        }

        /// <summary>
        /// Salva o relatório de atendimento
        /// </summary>
        /// <param name="rea">Entidade do relatório de atendimento</param>
        /// <param name="lstGrupo">Lista de grupos</param>
        /// <param name="lstCargo">Lista de cargos</param>
        /// <param name="lstQuestionario">Lista de questionários</param>
        /// <param name="postedFile">Arquivo anexo</param>
        /// <returns></returns>
        public static bool Salvar(CLS_RelatorioAtendimento rea, List<CLS_RelatorioAtendimentoGrupo> lstGrupo, List<CLS_RelatorioAtendimentoCargo> lstCargo, List<CLS_RelatorioAtendimentoQuestionario> lstQuestionario, SYS_Arquivo arquivo)
        {
            CLS_RelatorioAtendimentoDAO dao = new CLS_RelatorioAtendimentoDAO();
            dao._Banco.Open(IsolationLevel.ReadCommitted);
            try
            {
                if (arquivo.arq_tamanhoKB > 0)
                {
                    if (SYS_ArquivoBO.Save(arquivo, dao._Banco))
                        rea.arq_idAnexo = arquivo.arq_id;
                }

                bool isNew = rea.IsNew;
                if (!Save(rea, dao._Banco))
                    throw new ValidationException("Erro ao salvar o relatório de atendimento.");

                List<CLS_RelatorioAtendimentoQuestionario> lstQuestionarioBanco = CLS_RelatorioAtendimentoQuestionarioBO.SelectBy_rea_id(rea.rea_id);

                if (!isNew)
                {
                    CLS_RelatorioAtendimentoCargoBO.DeleteBy_rea_id(rea.rea_id, dao._Banco);
                    CLS_RelatorioAtendimentoGrupoBO.DeleteBy_rea_id(rea.rea_id, dao._Banco);

                    //Exclui todos os questionários que não estão mais ligados ao relatório
                    foreach (CLS_RelatorioAtendimentoQuestionario raq in lstQuestionarioBanco.Where(b => !lstQuestionario.Any(q => q.raq_id == b.raq_id && q.raq_situacao == (byte)CLS_RelatorioAtendimentoQuestionarioSituacao.Ativo && !q.IsNew)))
                    {
                        raq.raq_situacao = (byte)CLS_RelatorioAtendimentoQuestionarioSituacao.Excluido;
                        if (!CLS_RelatorioAtendimentoQuestionarioBO.Delete(raq, dao._Banco))
                            throw new ValidationException("Erro ao remover questionário do relatório de atendimento.");
                    }
                }

                foreach (CLS_RelatorioAtendimentoGrupo rag in lstGrupo)
                {
                    rag.rea_id = rea.rea_id;
                    if (!CLS_RelatorioAtendimentoGrupoBO.Save(rag, dao._Banco))
                        throw new ValidationException("Erro ao salvar grupo do relatório de atendimento.");
                }

                foreach (CLS_RelatorioAtendimentoCargo rac in lstCargo)
                {
                    rac.rea_id = rea.rea_id;
                    if (!CLS_RelatorioAtendimentoCargoBO.Save(rac, dao._Banco))
                        throw new ValidationException("Erro ao salvar cargo do relatório de atendimento.");
                }

                foreach (CLS_RelatorioAtendimentoQuestionario raq in lstQuestionario.Where(q => q.raq_situacao == (byte)CLS_RelatorioAtendimentoQuestionarioSituacao.Ativo))
                {
                    raq.rea_id = rea.rea_id;
                    if (raq.IsNew)
                        raq.raq_id = -1;
                    if (!CLS_RelatorioAtendimentoQuestionarioBO.Save(raq, dao._Banco))
                        throw new ValidationException("Erro ao salvar questionário do relatório de atendimento.");
                }
            }
            catch (Exception ex)
            {
                dao._Banco.Close(ex);
                throw;
            }
            finally
            {
                dao._Banco.Close();
            }
            return true;
        }

        #endregion 
    }
}