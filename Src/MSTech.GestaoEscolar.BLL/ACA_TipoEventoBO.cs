using System;
using System.Data;
using System.ComponentModel;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using System.Web;
using System.Collections.Generic;
using System.Linq;

namespace MSTech.GestaoEscolar.BLL
{
    #region Structs

    /// <summary>
    /// Estrutura para guardar o select da efetivação em cache.
    /// </summary>
    public struct Cache_EventosEfetivacao
    {
        public int tev_id;
        public string tev_nome;
        public bool tev_periodoCalendario;
        public byte tev_situacao;
    }

    #endregion

    public class ACA_TipoEventoBO : BusinessBase<ACA_TipoEventoDAO, ACA_TipoEvento>
    {
        /// <summary>
        /// Configurações de liberação para cadastro de eventos do tipo de evento
        /// </summary>
        public enum eLiberacao : byte
        {
            /// <summary>
            /// O tipo de evento não tem datas-limite para cadastro de eventos
            /// </summary>
            Desnecessaria = 1,
            /// <summary>
            /// O tipo de evento pode ter datas-limite para cadastro de eventos.
            /// Quando houver datas-limite cadastradas, elas devem ser respeitadas.
            /// Quando não houver, os eventos podem ser cadastrados livremente.
            /// </summary>
            Opcional = 2,
            /// <summary>
            /// O tipo de evento precisa ter datas-limite para cadastro de eventos.
            /// Os eventos só podem ser cadastrados nos períodos informados.
            /// </summary>
            Obrigatoria = 3
        }

        /// <summary>
        /// Retorna todos os tipos de evento não excluídos logicamente
        /// Com paginação
        /// </summary>                        
        /// <param name="tev_situacao">Situação do tipo de evento</param>
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>      
        /// <returns>Retorna todos os tipos de eventos.</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoEventoPaginado
        (
           byte tev_situacao
           , int currentPage
           , int pageSize
        )
        {
            ACA_TipoEventoDAO dao = new ACA_TipoEventoDAO();
            return dao.SelectBy_Pesquisa(tev_situacao, true, currentPage/pageSize, pageSize, new Guid(), out totalRecords);
        }

        /// <summary>
        /// Retorna todos os tipos de evento não excluídos logicamente
        /// Sem paginação
        /// </summary>        
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoEvento
        (
            byte tev_situacao,
            Guid gru_id
        )
        {
            ACA_TipoEventoDAO dao = new ACA_TipoEventoDAO();
            return dao.SelectBy_Pesquisa(tev_situacao, false, 1, 1, gru_id, out totalRecords);
        }

        /// <summary>
        /// Retorna todos os tipos de evento não excluídos logicamente
        /// que podem ter liberação de cadastro de eventos
        /// </summary>        
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaComLiberacao
        (
            Guid gru_id
        )
        {
            ACA_TipoEventoDAO dao = new ACA_TipoEventoDAO();
            return dao.SelecionaComLiberacao(gru_id);
        }

        /// <summary>
        /// Retorna todos os tipos de eventos ativos e relacionados com tipo periodo de calendario.
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTodosRelacionados()
        {
            ACA_TipoEventoDAO dao = new ACA_TipoEventoDAO();
            return dao.SelecionaTodosRelacionados();
        }

        /// <summary>
        /// Retorna todos os tipos de eventos ativos e não relacionados com tipo periodo de calendario.
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTodosNaoRelacionados()
        {
            ACA_TipoEventoDAO dao = new ACA_TipoEventoDAO();
            return dao.SelecionaTodosNaoRelacionados();
        }

        /// <summary>
        /// Retorna os eventos necessarios para as validações no método GetEntity_Efetivacao_List da ACA_EventoBO
        /// </summary>
        /// <param name="valorTipoEvento"></param>
        /// <param name="valorRecuperacao"></param>
        /// <param name="valorFinal"></param>
        /// <returns>DataTable com os Tipos de Evento</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<Cache_EventosEfetivacao> GetEntity_Eventos_Efetivacao
          (
            string chavesEventos
        )
        {
            try
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_Select_Efetivacao(chavesEventos);
                    object cache = HttpContext.Current.Cache[chave];
                    List<Cache_EventosEfetivacao> lista;

                    if (cache == null)
                    {
                        // Cache_EventosEfetivacao
                        DataTable dt = new ACA_TipoEventoDAO().SelectBy_EventosEfetivacao(chavesEventos);

                        lista = (from DataRow dr in dt.Rows
                                 select (Cache_EventosEfetivacao)
                                 GestaoEscolarUtilBO.DataRowToEntity(dr, new Cache_EventosEfetivacao())
                                     ).ToList();

                        // Adiciona cache com validade de 6h.
                        HttpContext.Current.Cache.Insert(chave, lista, null, DateTime.Now.AddMinutes(GestaoEscolarUtilBO.MinutosCacheMedio)
                            , System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        lista = (List<Cache_EventosEfetivacao>)cache;
                    }

                    return lista;
                }
                else
                {
                    ACA_TipoEventoDAO dao = new ACA_TipoEventoDAO();
                    DataTable dt = dao.SelectBy_EventosEfetivacao(chavesEventos);
                    return (from DataRow dr in dt.Rows
                     select (Cache_EventosEfetivacao)
                     GestaoEscolarUtilBO.DataRowToEntity(dr, new Cache_EventosEfetivacao())
                                     ).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Remove do cache a entidade.
        /// </summary>
        /// <param name="entity"></param>
        private static void LimpaCache(string chaves)
        {
            if (HttpContext.Current != null)
            {
                // Chave padrão do cache - nome do método + parâmetros.
                HttpContext.Current.Cache.Remove(RetornaChaveCache_Select_Efetivacao(chaves));
            }
        }

        /// <summary>
        /// Retorna a chave para guardar em cache o GetEntity da classe.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static string RetornaChaveCache_Select_Efetivacao(string chaves)
        {
            return string.Format("ACA_TipoEvento_SelectEfetivacao_{0}", chaves);
        }

        /// <summary>
        /// Verifica se já existe um tipo de evento cadastrado com o mesmo nome
        /// </summary>
        /// <param name="entity">Entidade ACA_TipoEvento</param> 
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaTipoEventoExistente
        (
            ACA_TipoEvento entity
        )
        {
            ACA_TipoEventoDAO dao = new ACA_TipoEventoDAO();
            return dao.SelectBy_Nome(entity);
        }

        /// <summary>
        /// Inclui ou altera o tipo de evento
        /// </summary>
        /// <param name="entity">Entidade ACA_TipoEvento</param>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Save
        (
            ACA_TipoEvento entity
        )
        {
            if (entity.Validate())
            {
                if (VerificaTipoEventoExistente(entity))
                    throw new DuplicateNameException("Já existe um tipo de evento cadastrado com este nome.");

                ACA_TipoEventoDAO dao = new ACA_TipoEventoDAO();
                return dao.Salvar(entity);
            }

            throw new ValidationException(entity.PropertiesErrorList[0].Message);
        }

        /// <summary>
        /// Deleta logicamente um tipo de evento
        /// </summary>
        /// <param name="entity">Entidade ACA_TipoEvento</param>        
        /// <returns>True = deletado/alterado | False = não deletado/alterado</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Delete
        (
            ACA_TipoEvento entity
        )
        {
            ACA_TipoEventoDAO dao = new ACA_TipoEventoDAO();
            dao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                //Verifica se o tipo de evento pode ser deletado
                if (GestaoEscolarUtilBO.VerificarIntegridade("tev_id", entity.tev_id.ToString(), "ACA_TipoEvento", dao._Banco))                
                    throw new ValidationException("Não é possível excluir o tipo de evento pois possui outros registros ligados a ele.");
                
                //Deleta logicamente o tipo de evento
                dao.Delete(entity);

                return true;
            }
            catch (Exception err)
            {
                dao._Banco.Close(err);
                throw;
            }
            finally
            {
                dao._Banco.Close();
            }
        }
    }
}
