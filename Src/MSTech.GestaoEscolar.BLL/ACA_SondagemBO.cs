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
    using System.ComponentModel;
    using System.Data;
    using Validation.Exceptions;
    #region Enumeradores

    /// <summary>
    /// Situações do curso
    /// </summary>
    public enum ACA_SondagemSituacao : byte
    {
        Ativo = 1
        ,

        Bloqueado = 2
        ,

        Excluido = 3
    }
    
    #endregion Enumeradores

    public class ACA_SondagemBO : BusinessBase<ACA_SondagemDAO, ACA_Sondagem>
	{
        /// <summary>
        /// Salva a sondagem
        /// </summary>
        /// <param name="entity">Entidade da sondagem</param>
        /// <param name="banco">Transação de banco</param>
        /// <returns></returns>
        public static bool Salvar(ACA_Sondagem entity, TalkDBTransaction banco = null)
        {
            ACA_SondagemDAO dao = new ACA_SondagemDAO();
            if (banco == null)
                dao._Banco.Open(IsolationLevel.ReadCommitted);
            else
                dao._Banco = banco;

            try
            {
                if (!dao.Salvar(entity))
                    return false;

                LimpaCache(entity);

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

            string tabelasNaoVerificarIntegridade = "ACA_Sondagem,ACA_SondagemAgendamento,ACA_SondagemAvaliacoes,ACA_SondagemRespostas";

            try
            {
                //Verifica se a disciplina pode ser deletada
                if (GestaoEscolarUtilBO.VerificarIntegridade("snd_id", entity.snd_id.ToString(), tabelasNaoVerificarIntegridade, dao._Banco))
                    throw new ValidationException("Não é possível excluir a sondagem " + entity.snd_titulo + ", pois possui outros registros ligados a ela.");

                //TODO: validar dados para exclusão

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