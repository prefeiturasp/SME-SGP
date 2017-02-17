/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL;
using MSTech.Data.Common;
using MSTech.Validation.Exceptions;
using System.Web;
using System.Linq;

namespace MSTech.GestaoEscolar.BLL
{
	
	/// <summary>
	/// CLS_TurmaAulaRecurso Business Object 
	/// </summary>
    public class CLS_TurmaAulaRecursoBO : BusinessBase<CLS_TurmaAulaRecursoDAO, CLS_TurmaAulaRecurso>
    {
        /// <summary>
        /// Retorna a chave do cache para o método GetSelectBy_Turma_Aula
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="tau_id">ID da aula</param>
        /// <returns>Chave</returns>
        private static string retornarChave_GetSelectBy_Turma_Aula
        (
            long tud_id
            , int tau_id
        )
        {
            return string.Format("Cache_GetSelectBy_Turma_Aula_{0}_{1}", tud_id, tau_id);
        }

        /// <summary>
        /// Retorna quantidade de tempos de aula.
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="tau_id">ID da aula</param>
        /// <param name="AppMinutosCacheLongo">Quantidade de minutos da configuração de cache longo</param>
        /// <returns>List<CLS_TurmaAulaRecurso></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<CLS_TurmaAulaRecurso> GetSelectBy_Turma_Aula
        (
            long tud_id
            , int tau_id
            , int AppMinutosCacheLongo = 0
        )
        {
            List<CLS_TurmaAulaRecurso> lista = null;

            if (AppMinutosCacheLongo > 0 && HttpContext.Current != null)
            {
                string chave = retornarChave_GetSelectBy_Turma_Aula(tud_id, tau_id);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    lista = new CLS_TurmaAulaRecursoDAO().SelectBy_tud_id_tau_id(tud_id, tau_id);                             

                    HttpContext.Current.Cache.Insert(chave, lista, null, DateTime.Now.AddMinutes(AppMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                    lista = (List<CLS_TurmaAulaRecurso>)cache;
            }
            else
                lista = new CLS_TurmaAulaRecursoDAO().SelectBy_tud_id_tau_id(tud_id, tau_id);

            return lista;
        }

        /// <summary>
        /// Deleta recursos que foram desmarcados no cadastro
        /// </summary>
        /// <returns>True em caso de sucesso</returns>
        public static bool Delete_Byrsa_id(long tud_id, int tau_id,int rsa_id, TalkDBTransaction banco)
        {
            string chave = retornarChave_GetSelectBy_Turma_Aula(tud_id, tau_id);

            if (HttpContext.Current.Cache[chave] != null)
                GestaoEscolarUtilBO.LimpaCache(chave);

            CLS_TurmaAulaRecursoDAO dao = new CLS_TurmaAulaRecursoDAO() {_Banco = banco };
            return dao.DeleteBy_rsa_id(tud_id, tau_id, rsa_id);
        }

        /// <summary>
        ///Altera recursos já existentes no banco
        /// </summary>
        /// <returns>True em caso de sucesso</returns>
        public static bool Update_Byrsa_id(CLS_TurmaAulaRecurso entity, TalkDBTransaction banco)
        {
            string chave = retornarChave_GetSelectBy_Turma_Aula(entity.tud_id, entity.tau_id);

            if (HttpContext.Current.Cache[chave] != null)
                GestaoEscolarUtilBO.LimpaCache(chave);

            CLS_TurmaAulaRecursoDAO dao = new CLS_TurmaAulaRecursoDAO() { _Banco = banco };
            return dao.UpdateBy_rsa_id(entity);
        }

        /// <summary>
        /// Salva uma lista de recursos de sala de aula.
        /// </summary>
        /// <param name="ltTurmaAulaRecurso"></param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static bool SalvaRecursosAula
        (
            List<CLS_TurmaAulaRecurso> ltTurmaAulaRecurso,
            TalkDBTransaction banco 
        )
        {
            DataTable dtTurmaAulaRecurso = CLS_TurmaAulaRecurso.TipoTabela_TurmaAulaRecurso();
            ltTurmaAulaRecurso.ForEach(p =>
                                        {
                                            if (p.Validate())
                                                dtTurmaAulaRecurso.Rows.Add(TurmaAulaRecursoToDataRow(p, dtTurmaAulaRecurso.NewRow()));
                                            else
                                                throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(p));
                                        });

            // Limpa o cache
            ltTurmaAulaRecurso.ForEach(p => GestaoEscolarUtilBO.LimpaCache(retornarChave_GetSelectBy_Turma_Aula(p.tud_id, p.tau_id)));

            return banco == null ?
                new CLS_TurmaAulaRecursoDAO().SalvaRecursosAula(dtTurmaAulaRecurso) :
                new CLS_TurmaAulaRecursoDAO { _Banco = banco }.SalvaRecursosAula(dtTurmaAulaRecurso);
        }

        /// <summary>
        /// Retorna um datarow com dados de um recurso de sala de aula.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static DataRow TurmaAulaRecursoToDataRow(CLS_TurmaAulaRecurso entity, DataRow dr, DateTime tar_dataAlteracao = new DateTime())
        {
            if (entity.idAula > 0)
                dr["idAula"] = entity.idAula;
            else
                dr["idAula"] = DBNull.Value;
            dr["tud_id"] = entity.tud_id;
            dr["tau_id"] = entity.tau_id;

            if (entity.rsa_id > 0)
                dr["rsa_id"] = entity.rsa_id;
            else
                dr["rsa_id"] = DBNull.Value;

            if (!string.IsNullOrEmpty(entity.tar_observacao))
                dr["tar_observacao"] = entity.tar_observacao;
            else
                dr["tar_observacao"] = DBNull.Value;

            if (tar_dataAlteracao != new DateTime())
                dr["tar_dataAlteracao"] = tar_dataAlteracao;
            else
                dr["tar_dataAlteracao"] = DBNull.Value;

            return dr;
        }

        /// <summary>
        /// Salva a entidade e realiza exclusão de possíves caches.
        /// </summary>
        /// <param name="entity">CLS_TurmaAulaRecurso</param>
        /// <param name="banco">Conexão banco de dados</param>
        /// <returns></returns>
        public static bool Salvar(CLS_TurmaAulaRecurso entity, TalkDBTransaction banco)
        {
            string chave = retornarChave_GetSelectBy_Turma_Aula(entity.tud_id, entity.tau_id);

            if (HttpContext.Current.Cache[chave] != null)
                GestaoEscolarUtilBO.LimpaCache(chave);

            return Save(entity, banco);
        }

    }
}