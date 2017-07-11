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
    using Data.Common;    /// <summary>
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
        /// Carrega os relatórios de RP verificando a permissão do usuário e o tipo de disciplina.
        /// </summary>
        /// <returns></returns>
        public static List<CLS_RelatorioAtendimento> SelecionaRelatoriosRPDisciplina(Guid usu_id, long alu_id, long tud_id, bool apenasComPreenchimento)
        {
            return new CLS_RelatorioAtendimentoDAO().SelecionaRelatoriosRPDisciplina(usu_id, alu_id, tud_id, apenasComPreenchimento).ToEntityList<CLS_RelatorioAtendimento>();
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
        /// Seleciona os tipos de relatório com pendencia e os alunos pendentes
        /// </summary>
        /// <param name="tpc_id"></param>
        /// <param name="tur_id"></param>
        /// <returns></returns>
        public static Dictionary<byte, List<long>> SelecionaPendenciasPorTurmaPeriodo(int tpc_id, long tur_id)
        {
            Dictionary<byte, List<long>> dic = new Dictionary<byte, List<long>>();

            using (DataTable dt = new CLS_RelatorioAtendimentoDAO().SelecionaPendenciasPorTurmaPeriodo(tpc_id, tur_id))
            {
                if (dt.Rows.Count > 0)
                {
                    dic = (from DataRow dr in dt.Rows
                           group dr by Convert.ToByte(dr["rea_tipo"]) into grupo
                           select new
                           {
                               chave = grupo.Key
                               ,
                               valor = grupo.Select(p => Convert.ToInt64(p["alu_id"])).ToList()
                           }).ToDictionary(p => p.chave, p => p.valor);
                }

                return dic;
            }
        }

        #endregion

        /// <summary>
        /// Salva o relatório de atendimento
        /// </summary>
        /// <param name="rea">Entidade do relatório de atendimento</param>
        /// <param name="lstGrupo">Lista de grupos</param>
        /// <param name="lstCargo">Lista de cargos</param>
        /// <param name="lstQuestionario">Lista de questionários</param>
        /// <param name="postedFile">Arquivo anexo</param>
        /// <returns></returns>
        public static bool Salvar(CLS_RelatorioAtendimento rea, List<CLS_RelatorioAtendimentoGrupo> lstGrupo, List<CLS_RelatorioAtendimentoCargo> lstCargo, List<CLS_RelatorioAtendimentoQuestionario> lstQuestionario, List<CLS_RelatorioAtendimentoPeriodo> lstRelatorioPeriodo, long arquivo, int TamanhoMaximoArquivo, string[] TiposArquivosPermitidos)
        {
            CLS_RelatorioAtendimentoDAO dao = new CLS_RelatorioAtendimentoDAO();
            dao._Banco.Open(IsolationLevel.ReadCommitted);
            try
            {
                rea.arq_idAnexo = arquivo;

                if (arquivo > 0)
                {
                    SYS_Arquivo arq = new SYS_Arquivo { arq_id = arquivo };
                    SYS_ArquivoBO.GetEntity(arq, dao._Banco);
                    arq.arq_situacao = (byte)SYS_ArquivoSituacao.Ativo;
                    SYS_ArquivoBO.Save(arq, dao._Banco);
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
                        if (raq.emUso)
                            throw new ValidationException(string.Format("O questionário ({0}) possui lançamentos no relatório e não pode ser excluído.", raq.qst_titulo));

                        raq.raq_situacao = (byte)CLS_RelatorioAtendimentoQuestionarioSituacao.Excluido;
                        if (!CLS_RelatorioAtendimentoQuestionarioBO.Delete(raq, dao._Banco))
                            throw new ValidationException("Erro ao remover questionário do relatório de atendimento.");
                    }
                }

                if (lstRelatorioPeriodo.Any())
                {
                    lstRelatorioPeriodo.ForEach(p => p.rea_id = rea.rea_id);
                    CLS_RelatorioAtendimentoPeriodoBO.AtualizarPeriodos(lstRelatorioPeriodo, dao._Banco);
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

        /// <summary>
        /// Deleta um relatório de atendimento que não estiver sendo usado
        /// </summary>
        /// <param name="entity">Entidade do relatório de atendimento que vai excluir</param>
        /// <param name="banco">Transação de banco</param>
        /// <returns></returns>
        public static bool Delete(CLS_RelatorioAtendimento entity, TalkDBTransaction banco = null)
        {
            CLS_RelatorioAtendimentoDAO dao = new CLS_RelatorioAtendimentoDAO();
            if (banco == null)
                dao._Banco.Open(IsolationLevel.ReadCommitted);
            else
                dao._Banco = banco;

            string tabelasNaoVerificarIntegridade = "CLS_RelatorioAtendimento,CLS_RelatorioAtendimentoCargo,CLS_RelatorioAtendimentoGrupo,CLS_RelatorioAtendimentoQuestionario";

            try
            {
                //Verifica se a disciplina pode ser deletada
                if (GestaoEscolarUtilBO.VerificarIntegridade("snd_id", entity.rea_id.ToString(), tabelasNaoVerificarIntegridade, dao._Banco))
                    throw new ValidationException("Não é possível excluir o relatório de atendimento " + entity.rea_titulo + ", pois possui outros registros ligados a ele.");

                //Deleta logicamente a disciplina
                return dao.Delete(entity);
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
    }
}