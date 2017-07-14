/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using Data.Common;
    using System.Web;
    using System;
    using System.Linq;
    using System.ComponentModel;
    using System.Data;
    using Validation.Exceptions;
    using System.Collections.Generic;
    using CustomResourceProviders;

    #region Enumeradores

    /// <summary>
    /// Situações da sondagem
    /// </summary>
    public enum ACA_SondagemSituacao : byte
    {
        Ativo = 1
        ,

        Bloqueado = 2
        ,

        Excluido = 3
    }

    /// <summary>
    /// Tipo de opção de resposta
    /// </summary>
    public enum ACA_SondagemOpcaoResposta : byte
    {
        Multiselecao = 1
        ,

        SelecaoUnica = 2
    }

    #endregion Enumeradores

    #region Estruturas

    /// <summary>
    /// Estrutura que armazena o lançamento da sondagem.
    /// </summary>
    [Serializable]
    public class ACA_Sondagem_Lancamento
    {
        public int sda_id { get; set; }
        public string dataInicio { get; set; }
        public string dataFim { get; set; }
        public long alu_id { get; set; }
        public string pes_nome { get; set; }
        public int mtu_numeroChamada { get; set; }
        public int sdr_id { get; set; }
        public string sdr_sigla { get; set; }
        public string sdr_descricao { get; set; }
        public int sdr_ordem { get; set; }
        public int sdq_idSub { get; set; }
        public string sdq_descricaoSub { get; set; }
        public int sdq_ordemSub { get; set; }
        public int sdq_id { get; set; }
        public string sdq_descricao { get; set; }
        public int sdq_ordem { get; set; }
        public bool respAluno { get; set; }
    }

    [Serializable]
    public class ACA_Sondagem_ListAgendamentos
    {
        public long alu_id { get; set; }
        public int sda_id { get; set; }
        public string dataInicio { get; set; }
        public string dataAgendamento { get; set; }
    }

    #endregion Estruturas

    public class ACA_SondagemBO : BusinessBase<ACA_SondagemDAO, ACA_Sondagem>
    {
        /// <summary>
        /// Salva a sondagem
        /// </summary>
        /// <param name="entity">Entidade da sondagem</param>
        /// <param name="banco">Transação de banco</param>
        /// <param name="lstQuestao">Lista de questões</param>
        /// <param name="lstSubQuestao">Lista de sub-questões</param>
        /// <param name="lstResposta">Lista de respostas</param>
        /// <returns></returns>
        public static bool Salvar(ACA_Sondagem entity, List<ACA_SondagemQuestao> lstQuestao, List<ACA_SondagemQuestao> lstSubQuestao, List<ACA_SondagemResposta> lstResposta, TalkDBTransaction banco = null)
        {
            ACA_SondagemDAO dao = new ACA_SondagemDAO();
            if (banco == null)
                dao._Banco.Open(IsolationLevel.ReadCommitted);
            else
                dao._Banco = banco;

            try
            {
                //Carrega as questões ligadas à sondagem (se for uma sondagem que já existe)
                List<ACA_SondagemQuestao> lstQuestaoBanco = entity.IsNew ? new List<ACA_SondagemQuestao>() :
                                                            ACA_SondagemQuestaoBO.SelectQuestoesBy_Sondagem(entity.snd_id, dao._Banco);

                //Carrega as respostas ligadas à sondagem (se for uma sondagem que já existe)
                List<ACA_SondagemResposta> lstRespostaBanco = entity.IsNew ? new List<ACA_SondagemResposta>() :
                                                              ACA_SondagemRespostaBO.SelectRespostasBy_Sondagem(entity.snd_id, dao._Banco);

                //Salva a sondagem
                if (!dao.Salvar(entity))
                    return false;

                LimpaCache(entity);

                //Salva questões
                foreach (ACA_SondagemQuestao sdq in lstQuestao)
                {
                    sdq.snd_id = entity.snd_id;
                    sdq.sdq_subQuestao = false;
                    if (sdq.IsNew)
                        sdq.sdq_id = -1;
                    if (!ACA_SondagemQuestaoBO.Save(sdq, dao._Banco))
                        return false;
                }

                //Salva sub-questões
                foreach (ACA_SondagemQuestao sdq in lstSubQuestao)
                {
                    sdq.snd_id = entity.snd_id;
                    sdq.sdq_subQuestao = true;
                    if (sdq.IsNew)
                        sdq.sdq_id = -1;
                    if (!ACA_SondagemQuestaoBO.Save(sdq, dao._Banco))
                        return false;
                }

                //Remove logicamente no banco as questões e sub-questões que foram removidas da sondagem
                foreach (ACA_SondagemQuestao sdqB in lstQuestaoBanco)
                    if (!lstQuestao.Any(q => q.sdq_id == sdqB.sdq_id && q.sdq_situacao != (byte)ACA_SondagemQuestaoSituacao.Excluido) &&
                        !lstSubQuestao.Any(q => q.sdq_id == sdqB.sdq_id && q.sdq_situacao != (byte)ACA_SondagemQuestaoSituacao.Excluido))
                    {
                        ACA_SondagemQuestaoBO.Delete(sdqB, dao._Banco);
                    }

                //Salva respostas
                foreach (ACA_SondagemResposta sdr in lstResposta)
                {
                    sdr.snd_id = entity.snd_id;
                    if (sdr.IsNew)
                        sdr.sdr_id = -1;
                    if (!ACA_SondagemRespostaBO.Save(sdr, dao._Banco))
                        return false;
                }

                //Remove logicamente no banco as respostas que foram removidas da sondagem
                foreach (ACA_SondagemResposta sdrB in lstRespostaBanco)
                    if (!lstResposta.Any(r => r.sdr_id == sdrB.sdr_id && r.sdr_situacao != (byte)ACA_SondagemRespostaSituacao.Excluido))
                    {
                        ACA_SondagemRespostaBO.Delete(sdrB, dao._Banco);
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

        /// <summary>
        /// Retorna todos as sondagens não excluídas logicamente
        /// Com paginação
        /// </summary>   
        /// <param name="snd_titulo">Título da sondagem</param>
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>  
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaSondagensPaginado
        (
            string snd_titulo
            , int currentPage
            , int pageSize
        )
        {
            totalRecords = 0;

            if (pageSize == 0)
                pageSize = 1;

            ACA_SondagemDAO dao = new ACA_SondagemDAO();
            return dao.SelectBy_Pesquisa(snd_titulo, true, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Retorna as sondagens não excluídas logicamente, de acordo com a pesquisa para lançamento,
        /// com paginação.
        /// </summary>   
        /// <param name="snd_titulo">Título da sondagem</param>
        /// <param name="snd_dataInicio">Data de início do agendamento da sondagem</param>
        /// <param name="snd_dataFim">Data de fim do agendamento da sondagem</param>
        /// <param name="situacao">Situação da sondagem: 1 - Vigente; 2 - Vigente com lançamento; 3 - Vigente sem lançamento; 4 - Vigência encerrada</param>
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>  
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaSondagensLancamentoPaginado
        (
            string snd_titulo
            , DateTime sda_dataInicio
            , DateTime sda_dataFim
            , byte situacao
            , long doc_id
            , Guid gru_id
            , Guid usu_id
            , bool adm
            , Guid ent_id
            , int currentPage
            , int pageSize
        )
        {
            totalRecords = 0;

            if (pageSize == 0)
                pageSize = 1;

            ACA_SondagemDAO dao = new ACA_SondagemDAO();
            return dao.SelectBy_PesquisaLancamento(snd_titulo, sda_dataInicio, sda_dataFim, situacao, doc_id, gru_id, usu_id, adm, ent_id, true, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Retorna as questões da sondagem para lançamento por turma.
        /// </summary>   
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<ACA_Sondagem_Lancamento> SelecionaSondagemLancamentoTurma
        (
            int snd_id
            , int sda_id
            , long tur_id
        )
        {
            List<ACA_Sondagem_Lancamento> retorno = new List<ACA_Sondagem_Lancamento>();
            DataTable dtLancamento = new ACA_SondagemDAO().SelectBy_LancamentoTurma(snd_id, sda_id, tur_id);
            if (dtLancamento.Rows.Count > 0)
            {
                retorno = dtLancamento.Rows.Cast<DataRow>().Select(p => (ACA_Sondagem_Lancamento)GestaoEscolarUtilBO.DataRowToEntity(p, new ACA_Sondagem_Lancamento())).ToList();
            }
            return retorno;
        }

        /// <summary>
        /// Retorna os dados das sondagens que o aluno participou.
        /// </summary>
        /// <param name="ano">Ano das sondagens</param>
        /// <param name="alu_id">Id do aluno</param>
        /// <returns></returns>
        public static DataTable SelecionaSondagemPorAluno(int ano, long alu_id)
        {
            ACA_SondagemDAO dao = new ACA_SondagemDAO();
            return dao.SelectBy_Aluno(ano, alu_id);
        }

        /// <summary>
        /// Retorna todas as sondagens ativas.
        /// </summary>
        /// <returns></returns>
        public static List<ACA_Sondagem> SelecionaSondagemAtiva()
        {
            ACA_SondagemDAO dao = new ACA_SondagemDAO();

            DataTable dtSondagem = dao.SelectBy_Ativas();

            List<ACA_Sondagem> retorno = new List<ACA_Sondagem>();

            if (dtSondagem.Rows.Count > 0)
            {
                retorno = dtSondagem.Rows.Cast<DataRow>().Select(p => (ACA_Sondagem)GestaoEscolarUtilBO.DataRowToEntity(p, new ACA_Sondagem())).ToList();
            }
            return retorno;

        }

        /// <summary>
        /// Deleta logicamente uma sondagem
        /// </summary>
        /// <param name="entity">Entidade ACA_Sondagem</param>        
        /// <param name="banco">Conexão aberta com o banco de dados</param>
        /// <returns>True = deletado/alterado | False = não deletado/alterado</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static bool Delete
        (
            ACA_Sondagem entity
            , TalkDBTransaction banco = null
        )
        {
            ACA_SondagemDAO dao = new ACA_SondagemDAO();
            if (banco == null)
                dao._Banco.Open(IsolationLevel.ReadCommitted);
            else
                dao._Banco = banco;

            string tabelasNaoVerificarIntegridade = "ACA_Sondagem,ACA_SondagemAgendamento,ACA_SondagemAgendamentoPeriodo,ACA_SondagemQuestao,ACA_SondagemResposta,CLS_AlunoSondagem";

            try
            {
                //Verifica se a disciplina pode ser deletada
                if (GestaoEscolarUtilBO.VerificarIntegridade("snd_id", entity.snd_id.ToString(), tabelasNaoVerificarIntegridade, dao._Banco))
                    throw new ValidationException("Não é possível excluir a sondagem " + entity.snd_titulo + ", pois possui outros registros ligados a ela.");

                LimpaCache(entity);

                //Deleta logicamente a disciplina
                return dao.Delete(entity);
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

        /// <summary>
        /// Override do GetEntity que guarda em Cache a entidade retornada.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new static ACA_Sondagem GetEntity(ACA_Sondagem entity, TalkDBTransaction banco = null)
        {
            ACA_SondagemDAO dao = banco == null ? new ACA_SondagemDAO() : new ACA_SondagemDAO { _Banco = banco };

            if (HttpContext.Current != null)
            {
                // Chave padrão do cache - nome do método + parâmetros.
                string chave = RetornaChaveCache_GetEntity(entity);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    dao.Carregar(entity);
                    // Adiciona cache com validade de 6h.
                    HttpContext.Current.Cache.Insert(chave, entity, null, DateTime.Now.AddMinutes(GestaoEscolarUtilBO.MinutosCacheMedio)
                        , System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    GestaoEscolarUtilBO.CopiarEntity(cache, entity);
                }

                return entity;
            }

            dao.Carregar(entity);

            return entity;
        }

        /// <summary>
        /// Remove do cache a entidade.
        /// </summary>
        /// <param name="entity"></param>
        private static void LimpaCache(ACA_Sondagem entity)
        {
            if (HttpContext.Current != null)
            {
                // Chave padrão do cache - nome do método + parâmetros.
                HttpContext.Current.Cache.Remove(RetornaChaveCache_GetEntity(entity));
            }
        }

        /// <summary>
        /// Retorna a chave para guardar em cache o GetEntity da classe.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static string RetornaChaveCache_GetEntity(ACA_Sondagem entity)
        {
            return string.Format("ACA_Sondagem_GetEntity_{0}", entity.snd_id);
        }

    }
}