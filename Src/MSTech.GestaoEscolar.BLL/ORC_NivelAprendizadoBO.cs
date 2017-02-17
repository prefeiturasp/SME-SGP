/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
    using System.Data;
    using MSTech.Validation.Exceptions;
    using MSTech.Data.Common;
    using System;
    using System.Linq;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.Web;

    #region Estruturas

    [Serializable]
    public struct sNivelAprendizado
    {
        public int nap_id { get; set; }
        public string nap_descricao { get; set; }
        public string nap_sigla { get; set; }
        public byte nap_situacao { get; set; }
        public DateTime nap_dataCriacao { get; set; }
        public DateTime nap_dataAlteracao { get; set; }
        public string nivelAprendizado { get; set; }
        public string tur_curso { get; set; }
    }

    #endregion
        
	/// <summary>
	/// Description: ORC_NivelAprendizado Business Object. 
	/// </summary>
	public class ORC_NivelAprendizadoBO : BusinessBase<ORC_NivelAprendizadoDAO, ORC_NivelAprendizado>
    {
        #region Métodos de consulta

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar a lista de nivel de aprendizado
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelectNiveisAprendizadoAtivos(int cur_id, int crr_id, int crp_id)
        {
            return string.Format("Cache_GetSelectNiveisAprendizadoAtivos_{0}_{1}_{2}", cur_id, crr_id, crp_id);
        }

        /// <summary>
        /// Retorna os níveis de aprendizado
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sNivelAprendizado> GetSelectNiveisAprendizadoAtivos(int cur_id, int crr_id, int crp_id, int appMinutosCacheLongo = 0)
        {
            List<sNivelAprendizado> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_SelectNiveisAprendizadoAtivos(cur_id, crr_id, crp_id);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        ORC_NivelAprendizadoDAO dao = new ORC_NivelAprendizadoDAO();
                        DataTable dtDados = dao.SelectNiveisAprendizadoAtivos(cur_id, crr_id, crp_id);
                        dados = (from DataRow dr in dtDados.Rows
                                 select new sNivelAprendizado
                                 {
                                     nap_id = Convert.ToInt32(dr["nap_id"]),
                                     nap_descricao = dr["nap_descricao"].ToString(),
                                     nap_sigla = dr["nap_sigla"].ToString(),
                                     nap_situacao = Convert.ToByte(dr["nap_situacao"]),
                                     nap_dataCriacao = Convert.ToDateTime(dr["nap_dataCriacao"]),
                                     nap_dataAlteracao = Convert.ToDateTime(dr["nap_dataAlteracao"]),
                                     nivelAprendizado = dr["nivelAprendizado"].ToString(),
                                     tur_curso = dr["tur_curso"].ToString()
                                 }).ToList();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sNivelAprendizado>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                ORC_NivelAprendizadoDAO dao = new ORC_NivelAprendizadoDAO();
                DataTable dtDados = dao.SelectNiveisAprendizadoAtivos(cur_id, crr_id, crp_id);
                dados = (from DataRow dr in dtDados.Rows
                         select new sNivelAprendizado
                         {
                             nap_id = Convert.ToInt32(dr["nap_id"]),
                             nap_descricao = dr["nap_descricao"].ToString(),
                             nap_sigla = dr["nap_sigla"].ToString(),
                             nap_situacao = Convert.ToByte(dr["nap_situacao"]),
                             nap_dataCriacao = Convert.ToDateTime(dr["nap_dataCriacao"]),
                             nap_dataAlteracao = Convert.ToDateTime(dr["nap_dataAlteracao"]),
                             nivelAprendizado = dr["nivelAprendizado"].ToString(),
                             tur_curso = dr["tur_curso"].ToString()
                         }).ToList();
            }

            return dados;
        }

        /// <summary>
        /// Retorna todos os niveis de aprendizado desde a ultima sincronização, ou apenas os
        /// niveis ativos caso syncDate for nula.
        /// </summary>
        /// <param name="syncDate">Data da última sincronização</param>
        /// <param name="cur_id">Id do curso da turma</param>
        /// <param name="crr_id">Id do curriculo da turma</param>
        /// <param name="crp_id">Id do curriculoPeriodo da turma</param>
        /// <returns></returns>
        public static DataTable BuscarNiveisPorDataSincronizacao(DateTime syncDate, Int64 tur_id, int cur_id, int crr_id, int crp_id)
        {
            ORC_NivelAprendizadoDAO dao = new ORC_NivelAprendizadoDAO();
            return dao.SelectNiveisPorDataSincronizacao(syncDate, tur_id, cur_id, crr_id, crp_id);
        }

        /// <summary>
        /// Retorna o nível de aprendizado com a mesma sigla e ativo
        /// </summary>
        private static bool GetSelectNivelAprendizadoBySigla(ORC_NivelAprendizado nivelAprendizado)
        {
            ORC_NivelAprendizadoDAO dao = new ORC_NivelAprendizadoDAO();
            return dao.SelectNivelAprendizadoBySigla(nivelAprendizado);
        }

        /// <summary>
        /// Retorna o nível de aprendizado por curso, curriculo e período.
        /// </summary>
        private static DataTable GetSelectNivelAprendizadoByCursoPeriodo(ORC_NivelAprendizado nivelAprendizado)
        {
            ORC_NivelAprendizadoDAO dao = new ORC_NivelAprendizadoDAO();
            return dao.SelectNivelAprendizadoByCursoPeriodo(nivelAprendizado);
        }

        /// <summary>
        /// Retorna o nível de aprendizado por curso, curriculo e período.
        /// </summary>
        public static DataTable GetSelectNivelAprendizadoByCursoPeriodo(int cur_id, int crr_id, int crp_id)
        {
            return new ORC_NivelAprendizadoDAO().SelectNivelAprendizadoByCursoPeriodo
                (
                  new ORC_NivelAprendizado
                  {
                      cur_id = cur_id,
                      crr_id = crr_id,
                      crp_id = crp_id
                  }
                );
        }

        public static int SelectCursoPeriodoBy_nap_id(int nap_id, int nvl_id, TalkDBTransaction banco)
        {
            ORC_NivelAprendizadoDAO dao = new ORC_NivelAprendizadoDAO();
            return dao.SelectCursoPeriodoBy_nap_id(nap_id, nvl_id, banco);
        }

        #endregion

        #region Métodos de alteração e inclusão

        /// <summary>
        /// Salva o nivel de aprendizado
        /// </summary>
        /// <param name="entity">Entidade ORC_NivelAprendizado</param> 
        /// <param name="banco">Conexão aberta com o banco de dados</param>
        /// <returns>True = incluído/alterado | False = não incluído/alterado</returns>
        public new static bool Save(ORC_NivelAprendizado entity)
        {
            if (HttpContext.Current != null)
            {
                // Chave padrão do cache - nome do método + parâmetros.
                HttpContext.Current.Cache.Remove(RetornaChaveCache_SelectNiveisAprendizadoAtivos(entity.cur_id, entity.crr_id, entity.crp_id));
                HttpContext.Current.Cache.Remove(RetornaChaveCache_SelectNiveisAprendizadoAtivos(0, 0, 0));
            }
            GestaoEscolarUtilBO.LimpaCache(string.Format(ORC_OrientacaoCurricularNivelAprendizadoBO.Cache_SelecionaPorOrientacaoNivelAprendizado + "_{0}", entity.nap_id));

            if (entity.Validate())
            {
                ORC_NivelAprendizadoDAO dao = new ORC_NivelAprendizadoDAO();
                return dao.Salvar(entity);
            }

            throw new ValidationException(entity.PropertiesErrorList[0].Message);
        }

        /// <summary>
        /// Salva o nivel de aprendizado
        /// </summary>
        /// <param name="entity">Entidade ORC_NivelAprendizado</param> 
        /// <param name="banco">Conexão aberta com o banco de dados</param>
        /// <returns>True = incluído/alterado | False = não incluído/alterado</returns>
        public new static bool Save(ORC_NivelAprendizado entity, TalkDBTransaction banco)
        {
            if (HttpContext.Current != null)
            {
                // Chave padrão do cache - nome do método + parâmetros.
                HttpContext.Current.Cache.Remove(RetornaChaveCache_SelectNiveisAprendizadoAtivos(entity.cur_id, entity.crr_id, entity.crp_id));
                HttpContext.Current.Cache.Remove(RetornaChaveCache_SelectNiveisAprendizadoAtivos(0, 0, 0));
            }
            GestaoEscolarUtilBO.LimpaCache(string.Format(ORC_OrientacaoCurricularNivelAprendizadoBO.Cache_SelecionaPorOrientacaoNivelAprendizado + "_{0}", entity.nap_id));

            if (entity.Validate())
            {
                ORC_NivelAprendizadoDAO dao = new ORC_NivelAprendizadoDAO { _Banco = banco };
                return dao.Salvar(entity);
            }

            throw new ValidationException(entity.PropertiesErrorList[0].Message);
        }

        public static bool Salvar(ORC_NivelAprendizado nivel, Guid ent_id)
        {
            TalkDBTransaction bancoGestao = new ORC_NivelAprendizadoDAO()._Banco.CopyThisInstance();
            bancoGestao.Open(IsolationLevel.ReadCommitted);

            try
            {
                #region Validações

                if (GetSelectNivelAprendizadoBySigla(nivel))
                {
                    throw new DuplicateNameException("Já existe um nível de aprendizado com essa sigla.");
                }

                if (GetSelectNivelAprendizadoByCursoPeriodo(nivel).Rows.Count >= 10)
                {
                    throw new ValidationException(string.Format("Não é possível adicionar mais de 10 níveis de aprendizado por {0}.", GestaoEscolarUtilBO.nomePadraoPeriodo(ent_id).ToLower()));
                }
            
                #endregion

                if (nivel.Validate())
                {
                    ORC_NivelAprendizadoBO.Save(nivel, bancoGestao);
                }
                else
                {
                    throw new ValidationException(nivel.PropertiesErrorList[0].Message);
                }
                return true;
            }
            catch (Exception ex)
            {
                bancoGestao.Close(ex);
                throw;
            }
            finally
            {
                if (bancoGestao.ConnectionIsOpen)
                    bancoGestao.Close();
            }
        }

        #endregion
    }
}