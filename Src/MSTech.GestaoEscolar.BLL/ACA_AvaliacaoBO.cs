using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using MSTech.Business.Common;
using MSTech.CoreSSO.BLL;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using System.Web;
using System.Linq;
using MSTech.GestaoEscolar.BLL.Caching;

namespace MSTech.GestaoEscolar.BLL
{
    #region Enumeradores

    /// <summary>
    /// Tipo de avaliação.
    /// </summary>
    public enum AvaliacaoTipo : byte
    {
        [StringValue("Periódica")]
        Periodica = 1,

        [StringValue("Recuperação")]
        Recuperacao = 2,

        [StringValue("Final")]
        Final = 3,

        [StringValue("Conselho de classe")]
        ConselhoClasse = 4,

        [StringValue("Periódica final")]
        PeriodicaFinal = 5,

        [StringValue("Prova periódica da secretaria")]
        ProvaPeriodicaSecretaria = 6,

        [StringValue("Recuperação final")]
        RecuperacaoFinal = 7
    }

    /// <summary>
    /// Conceito/nota máxima da recuperação final: (1-Qualquer um; 2-Conceito/nota mínimo para aprovação)
    /// </summary>
    public enum ACA_Avaliacao_RecFinalConceitoMaximoAprovacao : byte
    {
        QualquerUm = 1
        , Conceito_Nota_Minima_Aprovacao = 2
    }

    #endregion

    public class ACA_AvaliacaoBO : BusinessBase<ACA_AvaliacaoDAO, ACA_Avaliacao>
    {
        #region estruturas

        /// <summary>
        /// Estrutura utilizada para armazenar os tipos de avaliações. 
        /// </summary>
        public struct sTipoAvaliacao
        {
            public string fav_ava_id { get; set; }
            public string ava_nome { get; set; }
        }

        #endregion

        /// <summary>
        /// Override do GetEntity que guarda em Cache a entidade retornada.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new static ACA_Avaliacao GetEntity(ACA_Avaliacao entity)
        {
            if (HttpContext.Current != null)
            {
                // Chave padrão do cache - nome do método + parâmetros.
                string chave = RetornaChaveCache_GetEntity(entity);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    new ACA_AvaliacaoDAO().Carregar(entity);
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

            new ACA_AvaliacaoDAO().Carregar(entity);

            return entity;
        }

        /// <summary>
        /// Remove do cache a entidade.
        /// </summary>
        /// <param name="entity"></param>
        private static void LimpaCache(ACA_Avaliacao entity)
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
        private static string RetornaChaveCache_GetEntity(ACA_Avaliacao entity)
        {
            return string.Format("ACA_Avaliacao_GetEntity_{0}_{1}", entity.fav_id, entity.ava_id);
        }

        /// <summary>
        /// Busca o maior bimestre pelo formato de avaliação
        /// </summary>
        /// <param name="fav_id"></param>
        /// <returns></returns>
        public static int SelecionaMaiorBimestre_ByFormatoAvaliacao(int fav_id, TalkDBTransaction banco)
        {
            return new ACA_AvaliacaoDAO { _Banco = banco }.SelecionaMaiorBimestre_ByFormatoAvaliacao(fav_id);
        }


        /// <summary>
        ///  Retorna os dados da avaliação do tipo final
        ///	 ou do tipo periódica + final do formato informado
        /// </summary>
        /// <param name="fav_id">ID do formato de avaliação - Obrigatório</param>        
        /// <returns>DataTable com a avaliação final ou periódica m+ final</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelectAvaliacaoFinal_PorFormato
        (
           int fav_id
        )
        {
            ACA_AvaliacaoDAO dao = new ACA_AvaliacaoDAO();
            DataTable dtDados = null;
            if (HttpContext.Current != null)
            {
                string chave = String.Format(RetornaChaveCache_SelectAvaliacaoFinal_PorFormato()
                                             , fav_id);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    dtDados = dao.SelectAvaliacaoFinal_PorFormato(fav_id);

                    // Adiciona cache com validade de 6h.
                    HttpContext.Current.Cache.Insert(chave, dtDados, null, DateTime.Now.AddMinutes(GestaoEscolarUtilBO.MinutosCacheMedio)
                        , System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    dtDados = (DataTable)cache;
                }
                return dtDados;
            }
            return dao.SelectAvaliacaoFinal_PorFormato(fav_id);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<ACA_Avaliacao> SelectAvaliacaoFinal_PorFormato
        (
           int fav_id,
           int appMinutosCacheLongo = 0
        )
        {
            List<ACA_Avaliacao> dados = null;

            Func<List<ACA_Avaliacao>> retorno = delegate()
            {
                ACA_AvaliacaoDAO dao = new ACA_AvaliacaoDAO();
                return dao.SelectAvaliacaoFinal_PorFormato(fav_id).Rows
                          .Cast<DataRow>()
                          .Select(p => dao.DataRowToEntity(p, new ACA_Avaliacao())).ToList();
            };

            if (appMinutosCacheLongo > 0)
            {
                string chave = String.Format(ModelCache.AVALIACAO_FINAL_FORMATO_MODEL_KEY, fav_id);

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
        /// Retorna um datatable contendo todas as avaliações
        /// que não foram excluídas logicamente, filtradas por
        ///	fav_id_ava_id, ava_situacao
        /// </summary>
        /// <param name="fav_id_ava_id">IDs concatenados separados por ';' de fav_id e ava_id</param>
        /// <param name="ava_situacao">Situação da avaliação</param>
        /// <param name="ent_id"></param>
        /// <param name="paginado">Indica se vai exibir os registros paginados ou não.</param>
        /// <param name="currentPage">Página atual do gridview</param>
        /// <param name="pageSize">Total de registros por página</param>
        /// <returns>DataTable com as avaliações</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelect
        (
           string fav_id_ava_id
            , byte ava_situacao
            , Guid ent_id
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            int fav_id;
            int ava_id;
            if (!string.IsNullOrEmpty(fav_id_ava_id))
            {
                fav_id = Convert.ToInt32(fav_id_ava_id.Split(';')[0]);
                ava_id = Convert.ToInt32(fav_id_ava_id.Split(';')[1]);
            }
            else
            {
                fav_id = -1;
                ava_id = -1;
            }
            totalRecords = 0;

            if (pageSize == 0)
                pageSize = 1;

            ACA_AvaliacaoDAO dao = new ACA_AvaliacaoDAO();
            return dao.SelectBy_All(fav_id, ava_id, ava_situacao, ent_id, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Retorna um datatable contendo todas as avaliações
        /// que não foram excluídas logicamente, filtradas por
        ///	fav_id
        /// </summary>
        /// <param name="fav_id">ID de formato avaliacao</param>
        /// <param name="paginado">Indica se vai exibir os registros paginados ou não.</param>
        /// <param name="currentPage">Página atual do gridview</param>
        /// <param name="pageSize">Total de registros por página</param>
        /// <returns>DataTable com as avaliações</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_fav_id
        (
           int fav_id
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            totalRecords = 0;

            if (pageSize == 0)
                pageSize = 1;

            ACA_AvaliacaoDAO dao = new ACA_AvaliacaoDAO();
            return dao.SelectBy_fav_id(fav_id, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Retorna as avaliações cadastradas no formato para o período informado.
        /// </summary>
        /// <param name="fav_id">ID do formato de avaliação - Obrigatório</param>
        /// <param name="tud_id">TudId</param>
        /// <param name="tpc_idPeriodicaPeriodicaFinal">ID do tipo de período do calendário das avaliações
        ///                                             períodicas ou periódicas+final - Obrigatório</param>
        /// <param name="tpc_idRecuperacao">ID do tipo de período do calendário das avaliações
        ///                                 de recuperação - Obrigatório</param>
        /// <param name="existeFinal">Indica se vai trazer as avaliações finais e do conselho</param>
        /// <param name="existeRecuperacaoFinal">Indica se vai trazer as avaliações de recuperação final</param>
        /// <param name="verificarRegrasRecuperacao">Indicar se é para verificar as regras de recuperação.</param>
        /// <param name="efetivacaoSemestral">Indicar se para trazer as avaliações de acordo com a matriz curricular (EfetivacaoSemestral).</param>
        /// <returns>DataTable com as avaliações</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable ConsultaPor_Periodo_Efetivacao
        (
           long tur_id
           , int fav_id
            , long tud_id
            , string tpc_idPeriodicaPeriodicaFinal
            , string tpc_idRecuperacao
            , bool existeFinal
            , bool existeRecuperacaoFinal
            , bool verificarRegrasRecuperacao
            , bool efetivacaoSemestral
            , int tpc_idFiltrar = -1
            , int appMinutosCacheLongo = 0
        )
        {

            DataTable dados = null;

            Func<DataTable> retorno = delegate()
            {
                ACA_AvaliacaoDAO dao = new ACA_AvaliacaoDAO();
                return dao.SelectBy_Periodo_Efetivacao(tur_id, fav_id, tud_id, tpc_idPeriodicaPeriodicaFinal, tpc_idRecuperacao, existeFinal, existeRecuperacaoFinal, verificarRegrasRecuperacao, efetivacaoSemestral, tpc_idFiltrar);                          
            };

            if (appMinutosCacheLongo > 0)
            {
                string chave = String.Format(ModelCache.AVALIACAO_PERIODO_EFETIVACAO_MODEL_KEY, tur_id, fav_id, tud_id, tpc_idPeriodicaPeriodicaFinal, tpc_idRecuperacao, existeFinal, existeRecuperacaoFinal, verificarRegrasRecuperacao, efetivacaoSemestral, tpc_idFiltrar);

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
        /// Retorna as avaliações cadastradas no formato para o período informado.
        /// Se o curso da turma possuir efetivação semestral, retorna os períodos de acordo com a matriz do curso
        /// </summary>
        /// <param name="fav_id">ID do formato de avaliação - Obrigatório</param>
        /// <param name="tud_id">TudId</param>
        /// <param name="tpc_idPeriodicaPeriodicaFinal">ID do tipo de período do calendário das avaliações
        ///                                             períodicas ou periódicas+final - Obrigatório</param>
        /// <param name="tpc_idRecuperacao">ID do tipo de período do calendário das avaliações
        ///                                 de recuperação - Obrigatório</param>
        /// <param name="existeFinal">Indica se vai trazer as avaliações finais e do conselho</param>
        /// <param name="existeRecuperacaoFinal">Indica se vai trazer as avaliações de recuperação final</param>
        /// <param name="verificarRegrasRecuperacao">Indicar se é para verificar as regras de recuperação.</param>
        /// <returns>DataTable com as avaliações</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable ConsultaPor_Periodo_EfetivacaoSemestral
        (
           long tur_id
           , int fav_id
            , long tud_id
            , string tpc_idPeriodicaPeriodicaFinal
            , string tpc_idRecuperacao
            , bool existeFinal
            , bool existeRecuperacaoFinal
            , bool verificarRegrasRecuperacao
        )
        {
            ACA_AvaliacaoDAO dao = new ACA_AvaliacaoDAO();
            return dao.SelectBy_Periodo_EfetivacaoSemestral(tur_id,fav_id, tud_id, tpc_idPeriodicaPeriodicaFinal, tpc_idRecuperacao, existeFinal, existeRecuperacaoFinal, verificarRegrasRecuperacao);
        }

        /// <summary>
        /// Retorna as notas e frequencias efetivadas para o aluno
        /// </summary>
        /// <param name="alu_id">ID do tipo de período do calendário das avaliações</param>
        /// <param name="tpc_id">ID do período do calendário</param>
        /// <param name="fav_id">ID do formato de avaliação</param>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="mtu_id">ID da matricula da turma</param>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="tds_id">ID do tipo de disciplina.</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaNotasFreqEfetivadaPorAluno
        (

            long alu_id
            , int tpc_id
            , int fav_id
            , long tud_id
            , int mtu_id
            , long tur_id
            , int tds_id
        )
        {
            ACA_AvaliacaoDAO dao = new ACA_AvaliacaoDAO();
            return dao.SelecionaNotasFreqEfetivadaPorAluno(alu_id, tpc_id, fav_id, tud_id, mtu_id, tur_id, tds_id);
        }

        /// <summary>
        /// Retorna as avaliações cadastradas no formato para o período informado.
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="fav_id">ID do formato de avaliação - Obrigatório</param>
        /// <param name="tpc_idPeriodicaPeriodicaFinal">ID do tipo de período do calendário das avaliações
        ///                                             períodicas ou periódicas+final - Obrigatório</param>
        /// <param name="tpc_idRecuperacao">ID do tipo de período do calendário das avaliações
        ///                                 de recuperação - Obrigatório</param>
        /// <param name="existeFinal">Indica se vai trazer as avaliações finais e do conselho</param>
        /// <param name="verificarRegrasRecuperacao">Indicar se é para verificar as regras de recuperação.</param>
        /// <param name="efetivacaoSemestral">Indicar se para trazer as avaliações de acordo com a matriz curricular (EfetivacaoSemestral).</param>
        /// <returns>DataTable com as avaliações</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable ConsultaPor_Periodo_Efetivacao_TurmaDisciplinaCalendario
        (
            long tur_id
            , long tud_id
            , int fav_id
            , string tpc_idPeriodicaPeriodicaFinal
            , string tpc_idRecuperacao
            , bool existeFinal
            , bool verificarRegrasRecuperacao
            , bool efetivacaoSemestral
        )
        {
            ACA_AvaliacaoDAO dao = new ACA_AvaliacaoDAO();
            return dao.SelectBy_Periodo_Efetivacao_TurmaDisciplinaCalendario(tur_id,tud_id, fav_id, tpc_idPeriodicaPeriodicaFinal, tpc_idRecuperacao, existeFinal, verificarRegrasRecuperacao, efetivacaoSemestral);
        }

        /// <summary>
        /// Retorna as avaliações cadastradas no formato para o período informado.
        /// Retorna também as avaliações relacionadas às avaliações períodidas.
        /// </summary>
        /// <param name="fav_id">ID do formato de avaliação - Obrigatório</param>
        /// <param name="tpc_id">ID do tipo de período do calendário - Obrigatório</param>
        /// <returns>DataTable com as avaliações</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable ConsultaPor_Periodo_Relacionadas
        (
           int fav_id
            , string tpc_id
        )
        {
            ACA_AvaliacaoDAO dao = new ACA_AvaliacaoDAO();
            return dao.SelectBy_Periodo_Relacionadas(fav_id, tpc_id);
        }

        /// <summary>
        /// Retorna as avaliações cadastradas no formato para o período informado.
        /// Retorna também as avaliações relacionadas às avaliações períodidas.
        /// </summary>
        /// <param name="fav_id">ID do formato de avaliação - Obrigatório</param>
        /// <param name="tpc_id">ID do tipo de período do calendário - Obrigatório</param>
        /// <returns>DataTable com as avaliações</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<ACA_Avaliacao> ConsultaPor_Periodo_Relacionadas
        (
           int fav_id
            , int tpc_id
            , int appMinutosCacheLongo = 0
        )
        {
            List<ACA_Avaliacao> dados = null;

            Func<List<ACA_Avaliacao>> retorno = delegate()
            {
            ACA_AvaliacaoDAO dao = new ACA_AvaliacaoDAO();
                return dao.SelectBy_Periodo_Relacionadas(fav_id, Convert.ToString(tpc_id))
                          .Rows
                          .Cast<DataRow>()
                          .Select(p => dao.DataRowToEntity(p, new ACA_Avaliacao())).ToList();
            };

            if (appMinutosCacheLongo > 0)
            {
                string chave = String.Format(ModelCache.AVALIACAO_PERIODO_RELACIONADAS_MODEL_KEY, fav_id, tpc_id);

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
        /// Retorna as avaliações cadastradas no formato para o período informado.
        /// Retorna também as avaliações relacionadas às avaliações períodidas.
        /// </summary>
        /// <param name="fav_id">ID do formato de avaliação - Obrigatório</param>
        /// <param name="tpc_id">ID do tipo de período do calendário - Obrigatório</param>
        /// <returns>DataTable com as avaliações</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<ACA_Avaliacao> ConsultaPor_Periodo_Relacionadas
        (
           int fav_id
            , string tpc_id
            , int appMinutosCacheLongo = 0
        )
        {
            List<ACA_Avaliacao> dados = null;

            Func<List<ACA_Avaliacao>> retorno = delegate()
            {
            ACA_AvaliacaoDAO dao = new ACA_AvaliacaoDAO();
                return dao.SelectBy_Periodo_Relacionadas(fav_id, tpc_id)
                          .Rows
                          .Cast<DataRow>()
                          .Select(p => dao.DataRowToEntity(p, new ACA_Avaliacao())).ToList();
            };

            if (appMinutosCacheLongo > 0)
            {
                string chave = String.Format(ModelCache.AVALIACAO_PERIODO_RELACIONADAS_MODEL_KEY, fav_id, tpc_id);

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
        /// Retorna as avaliações do tipo informado para o formato de avaliação.
        /// </summary>
        /// <param name="ava_tipo">Tipo de avaliação</param>
        /// <param name="fav_id">Formato de avaliação</param>
        /// <returns></returns>
        public static DataTable GetSelectBy_TipoAvaliacao(AvaliacaoTipo ava_tipo, int fav_id)
        {
            ACA_AvaliacaoDAO dao = new ACA_AvaliacaoDAO();
            return dao.SelectBy_TipoAvaliacao((byte)ava_tipo, fav_id);
        }

        /// <summary>
        /// Retorna as avaliações do tipo informado para o formato de avaliação.
        /// </summary>        
        /// <param name="fav_id">Formato de avaliação</param>
        /// <returns></returns>
        public static DataTable SelecionaPeriodicaOuPeriodicaMaisFinal_PorFormato(int fav_id)
        {
            ACA_AvaliacaoDAO dao = new ACA_AvaliacaoDAO();
            return dao.SelecionaPeriodicaOuPeriodicaMaisFinal_PorFormato(fav_id);
        }

        /// <summary>
        /// Retorna um DataTable contendo dados sobre as avaliações,
        /// filtrados pelo tipo
        /// </summary>
        /// <param name="ava_tipos">string contendo os ids dos tipos</param>
        /// <param name="tur_id">ID da turma</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sTipoAvaliacao> SelecionaPorTipoAvaliacao(string ava_tipos, Int64 tur_id)
        {
            ACA_AvaliacaoDAO dao = new ACA_AvaliacaoDAO();
            List<sTipoAvaliacao> lstTipoAvaliacao = null;

            if (HttpContext.Current != null)
            {
                string chave = String.Format(RetornaChaveCache_SelecionaPorTipoAvaliacao()
                                             , ava_tipos, tur_id);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    // converte um dataTable em uma lista
                    lstTipoAvaliacao = dao.SelectBy_TipoAvaliacao(ava_tipos, tur_id).AsEnumerable().Select(m => new sTipoAvaliacao()
                    {
                        fav_ava_id = m.Field<string>("fav_ava_id"),
                        ava_nome = m.Field<string>("ava_nome")
                    }).ToList();

                    // Adiciona cache com validade de 6h.
                    HttpContext.Current.Cache.Insert(chave, lstTipoAvaliacao, null, DateTime.Now.AddMinutes(GestaoEscolarUtilBO.MinutosCacheMedio)
                        , System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    lstTipoAvaliacao = (List<sTipoAvaliacao>)cache;
                }
                return lstTipoAvaliacao;
            }

            return dao.SelectBy_TipoAvaliacao(ava_tipos, tur_id).AsEnumerable().Select(m => new sTipoAvaliacao()
                   {
                       fav_ava_id = m.Field<string>("fav_ava_id"),
                       ava_nome = m.Field<string>("ava_nome")
                   }).ToList();

            //return dao.SelectBy_TipoAvaliacao(ava_tipos, tur_id);
        }

        /// <summary>
        /// Cria List de Avaliacao, cada entidade recebe um registro do DataTable.
        /// </summary>
        /// <param name="dtAvaliacao">DataTable de Avaliacao</param>
        /// <returns>List Entidade Avaliacao</returns>
        public static List<ACA_Avaliacao> CriaList_Entities_Avaliacao
            (DataTable dtAvaliacao, byte tipoPeso)
        {
            //cria List
            List<ACA_Avaliacao> lt = new List<ACA_Avaliacao>();
            for (int i = 0; i < dtAvaliacao.Rows.Count; i++)
            {
                //cria entidade
                ACA_Avaliacao entityAvaliacao = new ACA_Avaliacao();
                //verifica se registro do DataTable é um novo registro
                if (dtAvaliacao.Rows[i].RowState == DataRowState.Added)
                {
                    entityAvaliacao.fav_id = Convert.ToInt32(dtAvaliacao.Rows[i]["fav_id"]);
                    entityAvaliacao.ava_id = Convert.ToInt32(dtAvaliacao.Rows[i]["ava_id"]);
                    entityAvaliacao.ava_nome = Convert.ToString(dtAvaliacao.Rows[i]["ava_nome"]);
                    entityAvaliacao.ava_tipo = Convert.ToByte(dtAvaliacao.Rows[i]["ava_tipo"]);
                    entityAvaliacao.tpc_id = Convert.ToInt32(dtAvaliacao.Rows[i]["tpc_id"]);
                    entityAvaliacao.ava_ordemPeriodo = -1; //Convert.ToInt32(dtAvaliacao.Rows[i]["ava_ordemPeriodo"]);
                    entityAvaliacao.ava_apareceBoletim = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_apareceBoletim"]);
                    entityAvaliacao.ava_baseadaConceitoGlobal = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_baseadaConceitoGlobal"]);
                    entityAvaliacao.ava_baseadaNotaDisciplina = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_baseadaNotaDisciplina"]);
                    entityAvaliacao.ava_baseadaAvaliacaoAdicional = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_baseadaAvaliacaoAdicional"]);
                    entityAvaliacao.ava_mostraBoletimConceitoGlobalAvaliacaoAdicional = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_mostraBoletimConceitoGlobalAvaliacaoAdicional"]);
                    entityAvaliacao.ava_mostraBoletimConceitoGlobalFrequencia = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_mostraBoletimConceitoGlobalFrequencia"]);
                    entityAvaliacao.ava_mostraBoletimConceitoGlobalNota = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_mostraBoletimConceitoGlobalNota"]);
                    entityAvaliacao.ava_mostraBoletimDisciplinaFrequencia = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_mostraBoletimDisciplinaFrequencia"]);
                    entityAvaliacao.ava_mostraBoletimDisciplinaNota = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_mostraBoletimDisciplinaNota"]);
                    entityAvaliacao.ava_recFinalConceitoMaximoAprovacao = Convert.ToInt16(string.IsNullOrEmpty(dtAvaliacao.Rows[i]["ava_recFinalConceitoMaximoAprovacao"].ToString()) ? "0" : dtAvaliacao.Rows[i]["ava_recFinalConceitoMaximoAprovacao"].ToString());
                    entityAvaliacao.ava_recFinalConceitoGlobalMinimoNaoAtingido = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_recFinalConceitoGlobalMinimoNaoAtingido"]);
                    entityAvaliacao.ava_recFinalFrequenciaMinimaFinalNaoAtingida = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_recFinalFrequenciaMinimaFinalNaoAtingida"]);
                    entityAvaliacao.ava_recFinalNotaDisciplinaApenasConceitoGlobalNaoAtingido = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_recFinalNotaDisciplinaApenasConceitoGlobalNaoAtingido"]);
                    entityAvaliacao.ava_situacao = Convert.ToByte(1);
                    entityAvaliacao.ava_dataCriacao = DateTime.Now;
                    entityAvaliacao.ava_dataAlteracao = DateTime.Now;
                    entityAvaliacao.ava_conceitoGlobalObrigatorio = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_conceitoGlobalObrigatorio"]);
                    entityAvaliacao.ava_conceitoGlobalObrigatorioFrequencia = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_conceitoGlobalObrigatorioFrequencia"]);
                    entityAvaliacao.ava_disciplinaObrigatoria = dtAvaliacao.Rows[i]["ava_disciplinaObrigatoria"] ==
                                                                DBNull.Value || string.IsNullOrEmpty(Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_disciplinaObrigatoria"]).ToString()) ? new bool() : Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_disciplinaObrigatoria"]);
                    entityAvaliacao.ava_exibeNaoAvaliados = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_exibeNaoAvaliados"]);
                    entityAvaliacao.ava_exibeSemProfessor = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_exibeSemProfessor"]);
                    entityAvaliacao.ava_exibeObservacaoDisciplina = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_exibeObservacaoDisciplina"]);
                    entityAvaliacao.ava_exibeObservacaoConselhoPedagogico = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_exibeObservacaoConselhoPedagogico"]);
                    entityAvaliacao.ava_exibeFrequencia = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_exibeFrequencia"]);
                    entityAvaliacao.ava_exibeNotaPosConselho = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_exibeNotaPosConselho"]);
                    entityAvaliacao.ava_ocultarAtualizacao = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_ocultarAtualizacao"]);

                    if (entityAvaliacao.ava_tipo == (byte)AvaliacaoTipo.Periodica
                      || entityAvaliacao.ava_tipo == (byte)AvaliacaoTipo.PeriodicaFinal)
                    {
                        if (tipoPeso == 1)
                        {
                            // Se o peso for informado pelo usuário.
                            entityAvaliacao.ava_peso = !string.IsNullOrEmpty(dtAvaliacao.Rows[i]["ava_peso"].ToString())
                                ? Convert.ToDecimal(dtAvaliacao.Rows[i]["ava_peso"]) : 0;
                        }
                        else if (tipoPeso == 2)
                        {
                            // Tipo de peso = soma das avaliações, aplica 100% fixo.
                            entityAvaliacao.ava_peso = 100;
                        }
                        else
                        {
                            // Se for pra não utilizar peso.
                            entityAvaliacao.ava_peso = 0;
                        }
                    }

                    entityAvaliacao.IsNew = true;
                    //adiciona entidade na List
                    lt.Add(entityAvaliacao);
                }
                //verifica se registro do Datable foi deletado.
                else if (dtAvaliacao.Rows[i].RowState == DataRowState.Deleted)
                {
                    //instancia valores para entidade como um registro deletado logicamente.
                    entityAvaliacao.fav_id = Convert.ToInt32(dtAvaliacao.Rows[i]["fav_id", DataRowVersion.Original]);
                    entityAvaliacao.ava_id = Convert.ToInt32(dtAvaliacao.Rows[i]["ava_id", DataRowVersion.Original]);
                    entityAvaliacao.ava_nome = Convert.ToString(dtAvaliacao.Rows[i]["ava_nome", DataRowVersion.Original]);
                    entityAvaliacao.ava_tipo = Convert.ToByte(dtAvaliacao.Rows[i]["ava_tipo", DataRowVersion.Original]);
                    entityAvaliacao.tpc_id = (dtAvaliacao.Rows[i]["tpc_id", DataRowVersion.Original].ToString() != string.Empty) ? Convert.ToInt32(dtAvaliacao.Rows[i]["tpc_id", DataRowVersion.Original].ToString()) : Convert.ToInt32(0);  //Convert.ToInt32(dtAvaliacao.Rows[i]["tpc_id"])
                    entityAvaliacao.ava_ordemPeriodo = -1; //(dtAvaliacao.Rows[i]["ava_ordemPeriodo", DataRowVersion.Original].ToString() != string.Empty) ? Convert.ToInt32(dtAvaliacao.Rows[i]["ava_ordemPeriodo"]) : Convert.ToInt32(0);
                    entityAvaliacao.ava_apareceBoletim = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_apareceBoletim", DataRowVersion.Original]);
                    entityAvaliacao.ava_baseadaConceitoGlobal = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_baseadaConceitoGlobal", DataRowVersion.Original]);
                    entityAvaliacao.ava_baseadaNotaDisciplina = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_baseadaNotaDisciplina", DataRowVersion.Original]);
                    entityAvaliacao.ava_baseadaAvaliacaoAdicional = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_baseadaAvaliacaoAdicional", DataRowVersion.Original]);
                    entityAvaliacao.ava_mostraBoletimConceitoGlobalAvaliacaoAdicional = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_mostraBoletimConceitoGlobalAvaliacaoAdicional", DataRowVersion.Original]);
                    entityAvaliacao.ava_mostraBoletimConceitoGlobalFrequencia = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_mostraBoletimConceitoGlobalFrequencia", DataRowVersion.Original]);
                    entityAvaliacao.ava_mostraBoletimConceitoGlobalNota = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_mostraBoletimConceitoGlobalNota", DataRowVersion.Original]);
                    entityAvaliacao.ava_mostraBoletimDisciplinaFrequencia = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_mostraBoletimDisciplinaFrequencia", DataRowVersion.Original]);
                    entityAvaliacao.ava_mostraBoletimDisciplinaNota = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_mostraBoletimDisciplinaNota", DataRowVersion.Original]);
                    entityAvaliacao.ava_recFinalConceitoMaximoAprovacao = Convert.ToInt16(string.IsNullOrEmpty(dtAvaliacao.Rows[i]["ava_recFinalConceitoMaximoAprovacao", DataRowVersion.Original].ToString()) ? "0" : dtAvaliacao.Rows[i]["ava_recFinalConceitoMaximoAprovacao", DataRowVersion.Original].ToString());
                    entityAvaliacao.ava_recFinalConceitoGlobalMinimoNaoAtingido = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_recFinalConceitoGlobalMinimoNaoAtingido", DataRowVersion.Original]);
                    entityAvaliacao.ava_recFinalFrequenciaMinimaFinalNaoAtingida = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_recFinalFrequenciaMinimaFinalNaoAtingida", DataRowVersion.Original]);
                    entityAvaliacao.ava_recFinalNotaDisciplinaApenasConceitoGlobalNaoAtingido = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_recFinalNotaDisciplinaApenasConceitoGlobalNaoAtingido", DataRowVersion.Original]);
                    entityAvaliacao.ava_situacao = Convert.ToByte(3);
                    entityAvaliacao.ava_dataCriacao = DateTime.Now;
                    entityAvaliacao.ava_dataAlteracao = DateTime.Now;
                    entityAvaliacao.IsNew = false;
                    entityAvaliacao.ava_conceitoGlobalObrigatorio = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_conceitoGlobalObrigatorio", DataRowVersion.Original]);
                    entityAvaliacao.ava_conceitoGlobalObrigatorioFrequencia = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_conceitoGlobalObrigatorioFrequencia", DataRowVersion.Original]);
                    entityAvaliacao.ava_disciplinaObrigatoria = dtAvaliacao.Rows[i]["ava_disciplinaObrigatoria", DataRowVersion.Original] ==
                                                                DBNull.Value || string.IsNullOrEmpty(Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_disciplinaObrigatoria", DataRowVersion.Original]).ToString()) ? new bool() : Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_disciplinaObrigatoria", DataRowVersion.Original]);
                    entityAvaliacao.ava_exibeNaoAvaliados = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_exibeNaoAvaliados", DataRowVersion.Original]);
                    entityAvaliacao.ava_exibeSemProfessor = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_exibeSemProfessor", DataRowVersion.Original]);
                    entityAvaliacao.ava_exibeObservacaoDisciplina = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_exibeObservacaoDisciplina", DataRowVersion.Original]);
                    entityAvaliacao.ava_exibeObservacaoConselhoPedagogico = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_exibeObservacaoConselhoPedagogico", DataRowVersion.Original]);
                    entityAvaliacao.ava_exibeFrequencia = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_exibeFrequencia", DataRowVersion.Original]);
                    entityAvaliacao.ava_exibeNotaPosConselho = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_exibeNotaPosConselho", DataRowVersion.Original]);
                    entityAvaliacao.ava_ocultarAtualizacao = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_ocultarAtualizacao", DataRowVersion.Original]);

                    if (entityAvaliacao.ava_tipo == (byte)AvaliacaoTipo.Periodica
                      || entityAvaliacao.ava_tipo == (byte)AvaliacaoTipo.PeriodicaFinal)
                    {
                        if (tipoPeso == 1)
                        {
                            // Se o peso for informado pelo usuário.
                            entityAvaliacao.ava_peso = !string.IsNullOrEmpty(dtAvaliacao.Rows[i]["ava_peso"].ToString())
                                ? Convert.ToDecimal(dtAvaliacao.Rows[i]["ava_peso", DataRowVersion.Original]) : 0;
                        }
                        else if (tipoPeso == 2)
                        {
                            // Tipo de peso = soma das avaliações, aplica 100% fixo.
                            entityAvaliacao.ava_peso = 100;
                        }
                        else
                        {
                            // Se for pra não utilizar peso.
                            entityAvaliacao.ava_peso = 0;
                        }
                    }
                    //adiciona entidade na List
                    lt.Add(entityAvaliacao);
                }
                //em ultimo caso registro é um registro já existente e não foi modificado.
                else
                {
                    //instancia valores para entidade como um registro já existente sem modificação. Atualiza apenas data de alteração
                    //para este registro.
                    entityAvaliacao.fav_id = Convert.ToInt32(dtAvaliacao.Rows[i]["fav_id"]);
                    entityAvaliacao.ava_id = Convert.ToInt32(dtAvaliacao.Rows[i]["ava_id"]);
                    entityAvaliacao.ava_nome = Convert.ToString(dtAvaliacao.Rows[i]["ava_nome"]);
                    entityAvaliacao.ava_tipo = Convert.ToByte(dtAvaliacao.Rows[i]["ava_tipo"]);
                    entityAvaliacao.tpc_id = (dtAvaliacao.Rows[i]["tpc_id"].ToString() != string.Empty) ? Convert.ToInt32(dtAvaliacao.Rows[i]["tpc_id"]) : Convert.ToInt32(0);
                    entityAvaliacao.ava_ordemPeriodo = -1; //(dtAvaliacao.Rows[i]["ava_ordemPeriodo"].ToString() != string.Empty) ? Convert.ToInt32(dtAvaliacao.Rows[i]["ava_ordemPeriodo"]) : Convert.ToInt32(0);
                    entityAvaliacao.ava_apareceBoletim = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_apareceBoletim"]);
                    entityAvaliacao.ava_baseadaConceitoGlobal = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_baseadaConceitoGlobal"]);
                    entityAvaliacao.ava_baseadaNotaDisciplina = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_baseadaNotaDisciplina"]);
                    entityAvaliacao.ava_baseadaAvaliacaoAdicional = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_baseadaAvaliacaoAdicional"]);
                    entityAvaliacao.ava_mostraBoletimConceitoGlobalAvaliacaoAdicional = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_mostraBoletimConceitoGlobalAvaliacaoAdicional"]);
                    entityAvaliacao.ava_mostraBoletimConceitoGlobalFrequencia = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_mostraBoletimConceitoGlobalFrequencia"]);
                    entityAvaliacao.ava_mostraBoletimConceitoGlobalNota = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_mostraBoletimConceitoGlobalNota"]);
                    entityAvaliacao.ava_mostraBoletimDisciplinaFrequencia = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_mostraBoletimDisciplinaFrequencia"]);
                    entityAvaliacao.ava_mostraBoletimDisciplinaNota = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_mostraBoletimDisciplinaNota"]);
                    entityAvaliacao.ava_recFinalConceitoMaximoAprovacao = Convert.ToInt16(string.IsNullOrEmpty(dtAvaliacao.Rows[i]["ava_recFinalConceitoMaximoAprovacao"].ToString()) ? "0" : dtAvaliacao.Rows[i]["ava_recFinalConceitoMaximoAprovacao"].ToString());
                    entityAvaliacao.ava_recFinalConceitoGlobalMinimoNaoAtingido = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_recFinalConceitoGlobalMinimoNaoAtingido"]);
                    entityAvaliacao.ava_recFinalFrequenciaMinimaFinalNaoAtingida = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_recFinalFrequenciaMinimaFinalNaoAtingida"]);
                    entityAvaliacao.ava_recFinalNotaDisciplinaApenasConceitoGlobalNaoAtingido = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_recFinalNotaDisciplinaApenasConceitoGlobalNaoAtingido"]);
                    entityAvaliacao.ava_situacao = Convert.ToByte(1);
                    entityAvaliacao.ava_dataCriacao = DateTime.Now;
                    entityAvaliacao.ava_dataAlteracao = DateTime.Now;
                    entityAvaliacao.ava_conceitoGlobalObrigatorio = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_conceitoGlobalObrigatorio"]);
                    entityAvaliacao.ava_conceitoGlobalObrigatorioFrequencia = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_conceitoGlobalObrigatorioFrequencia"]);
                    entityAvaliacao.ava_disciplinaObrigatoria = dtAvaliacao.Rows[i]["ava_disciplinaObrigatoria"] ==
                                                                DBNull.Value || string.IsNullOrEmpty(Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_disciplinaObrigatoria"]).ToString()) ? new bool() : Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_disciplinaObrigatoria"]);
                    entityAvaliacao.ava_exibeNaoAvaliados = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_exibeNaoAvaliados"]);
                    entityAvaliacao.ava_exibeSemProfessor = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_exibeSemProfessor"]);
                    entityAvaliacao.ava_exibeObservacaoDisciplina = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_exibeObservacaoDisciplina"]);
                    entityAvaliacao.ava_exibeObservacaoConselhoPedagogico = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_exibeObservacaoConselhoPedagogico"]);
                    entityAvaliacao.ava_exibeFrequencia = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_exibeFrequencia"]);
                    entityAvaliacao.ava_exibeNotaPosConselho = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_exibeNotaPosConselho"]);
                    entityAvaliacao.ava_ocultarAtualizacao = Convert.ToBoolean(dtAvaliacao.Rows[i]["ava_ocultarAtualizacao"]);

                    if (entityAvaliacao.ava_tipo == (byte)AvaliacaoTipo.Periodica
                        || entityAvaliacao.ava_tipo == (byte)AvaliacaoTipo.PeriodicaFinal)
                    {
                        if (tipoPeso == 1)
                        {
                            // Se o peso for informado pelo usuário.
                            entityAvaliacao.ava_peso = !string.IsNullOrEmpty(dtAvaliacao.Rows[i]["ava_peso"].ToString())
                                ? Convert.ToDecimal(dtAvaliacao.Rows[i]["ava_peso"]) : 0;
                        }
                        else if (tipoPeso == 2)
                        {
                            // Tipo de peso = soma das avaliações, aplica 100% fixo.
                            entityAvaliacao.ava_peso = 100;
                        }
                        else
                        {
                            // Se for pra não utilizar peso.
                            entityAvaliacao.ava_peso = 0;
                        }
                    }
                    entityAvaliacao.IsNew = false;
                    //adiciona entidade na List
                    lt.Add(entityAvaliacao);
                }
            }

            //retorna List
            return lt;
        }

        /// <summary>
        /// Retorno booleano na qual verifica se existe uma avaliação com o mesmo nome
        /// cadastrado no banco com situação diferente de Excluido.
        /// </summary>
        /// <returns>True - caso encontre algum registro no select/False - caso não encontre nada no select</returns>
        public static bool VerificaMesmoNomeAvaliacao(ACA_Avaliacao entity1, ACA_Avaliacao entity2)
        {
            if (entity1.ava_situacao != 3 && entity2.ava_situacao != 3)
            {
                if (entity1.ava_nome.ToUpper() == entity2.ava_nome.ToUpper())
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Busca a ordem das avaliações do tipo periódica do formato informado
        /// </summary>
        /// <param name="fav_id"></param>
        /// <returns></returns>
        public static DataTable GetSelectBy_FormatoAvaliacao(int fav_id)
        {
            ACA_AvaliacaoDAO dao = new ACA_AvaliacaoDAO();
            return dao.SelectBy_FormatoAvaliacao(fav_id);
        }

        /// <summary>
        /// Busca avaliações do tipo periódica do formato e periodo informados
        /// </summary>
        /// <param name="fav_id">Id do formato de avaliação</param>
        /// <param name="tpc_id">Id do periodo</param>
        /// <returns></returns>
        public static List<ACA_Avaliacao> GetSelectBy_FormatoAvaliacaoPeriodo(int fav_id, int tpc_id)
        {
            List<ACA_Avaliacao> dados = null;

            string chave = String.Format(ModelCache.AVALIACAO_POR_FORMATO_PERIODO_MODEL_KEY, fav_id, tpc_id);

            dados = CacheManager.Factory.Get(
                chave,
                () =>{
                    return new ACA_AvaliacaoDAO().SelectBy_FormatoAvaliacaoPeriodo(fav_id, tpc_id).AsEnumerable().Select(m => new ACA_Avaliacao()
                    {
                        ava_id = Convert.ToInt32(m["ava_id"].ToString()),
                        ava_nome = m["ava_nome"].ToString(),
                        ava_tipo = Convert.ToInt16(m["ava_tipo"].ToString()),
                    }).ToList();
                },
                GestaoEscolarUtilBO.MinutosCacheMedio
            );

            return dados;
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para a consulta.
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelectAvaliacaoFinal_PorFormato()
        {
            return "Cache_SelectAvaliacaoFinal_PorFormato_fav_id_{0}";
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para a consulta.
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaPorTipoAvaliacao()
        {
            return "Cache_SelecionaPorTipoAvaliacao_ava_tipos_{0}_tur_id_{1}";
        }
    }
}