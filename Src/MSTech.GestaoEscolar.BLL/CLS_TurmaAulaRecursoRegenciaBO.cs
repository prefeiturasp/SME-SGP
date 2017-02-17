/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
    using System.Data;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System;
    using MSTech.Data.Common;
    using MSTech.Validation.Exceptions;
    using System.Web;
	
	/// <summary>
	/// Description: CLS_TurmaAulaRecursoRegencia Business Object. 
	/// </summary>
	public class CLS_TurmaAulaRecursoRegenciaBO : BusinessBase<CLS_TurmaAulaRecursoRegenciaDAO, CLS_TurmaAulaRecursoRegencia>
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
            return string.Format("Cache_GetSelectBy_Turma_Aula_Regencia_{0}_{1}", tud_id, tau_id);
        }
        
        /// <summary>
        /// Excluir recursos.
        /// </summary>
        /// <param name="tud_id"></param>
        /// <param name="tau_id"></param>
        /// <param name="rsa_id"></param>
        /// <returns></returns>
        public static bool Delete_Byrsa_id
        (
            long tud_id
            , int tau_id
            , int rsa_id
        )
        {
            CLS_TurmaAulaRecursoRegenciaDAO dao = new CLS_TurmaAulaRecursoRegenciaDAO();
            return dao.DeleteBy_rsa_id(tud_id, tau_id, rsa_id);
        }

        /// <summary>
        /// Atualiza recursos.
        /// </summary>
        /// <param name="entityAltera"></param>
        /// <returns></returns>
        public static bool Update_Byrsa_id(CLS_TurmaAulaRecursoRegencia entityAltera)
        {
            CLS_TurmaAulaRecursoRegenciaDAO dao = new CLS_TurmaAulaRecursoRegenciaDAO();
            return dao.UpdateBy_rsa_id(entityAltera);
        }

        /// <summary>
        /// Seleciona recurso da aula componente da regência, por disciplina e aula.
        /// </summary>
        /// <param name="tud_id">Id da disciplina</param>
        /// <param name="tau_id">Id da aula</param>
        /// <param name="AppMinutosCacheLongo">Quantidade de minutos da configuração de cache longo</param>
        /// <returns>List<CLS_TurmaAulaRecursoRegencia></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<CLS_TurmaAulaRecursoRegencia> GetSelectBy_Turma_Aula
        (
            long tud_id
            , int tau_id
            , int AppMinutosCacheLongo = 0
        )
        {
            List<CLS_TurmaAulaRecursoRegencia> lista = null;

            if (AppMinutosCacheLongo > 0 && HttpContext.Current != null)
            {
                string chave = retornarChave_GetSelectBy_Turma_Aula(tud_id, tau_id);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    lista = new CLS_TurmaAulaRecursoRegenciaDAO().SelectBy_tud_id_tau_id(tud_id, tau_id);

                    HttpContext.Current.Cache.Insert(chave, lista, null, DateTime.Now.AddMinutes(AppMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                    lista = (List<CLS_TurmaAulaRecursoRegencia>)cache;
            }
            else
                lista = new CLS_TurmaAulaRecursoRegenciaDAO().SelectBy_tud_id_tau_id(tud_id, tau_id);

            return lista;            
        }

        /// <summary>
        /// Deleta os recursos por aula regencia
        /// </summary>
        /// <param name="tud_id">Disciplina</param>
        /// <param name="tau_id">Aula</param>
        /// <param name="tud_idFilho">Disciplina filha</param>
        /// <returns></returns>
        public static bool DeletaPorAulaRegencia(long tud_id, int tau_id, long tud_idFilho)
        {
            CLS_TurmaAulaRecursoRegenciaDAO dao = new CLS_TurmaAulaRecursoRegenciaDAO();
            return dao.DeletePorAulaRegencia(tud_id, tau_id, tud_idFilho);
        }

        /// <summary>
        /// Seleciona os recursos por componente da regência.
        /// </summary>
        /// <param name="tud_id">Id da disciplina regente</param>
        /// <param name="tau_id">Id da aula</param>
        /// <param name="tud_idFilho">Id da disciplina componente</param>
        /// <param name="AppMinutosCacheLongo">Quantidade de minutos da configuração de cache longo</param>
        /// <returns>List<CLS_TurmaAulaRecursoRegencia></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<CLS_TurmaAulaRecursoRegencia> GetSelectBy_Turma_Aula_DisciplinaComponente
        (
            long tud_id
            , int tau_id
            , long tud_idFilho
            , int AppMinutosCacheLongo = 0
        )
        {
            List<CLS_TurmaAulaRecursoRegencia> lista = null;

            if (AppMinutosCacheLongo > 0 && HttpContext.Current != null)
            {
                string chave = retornarChave_GetSelectBy_Turma_Aula(tud_id, tau_id);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    lista = new CLS_TurmaAulaRecursoRegenciaDAO().SelectBy_tud_id_tau_id_tud_idFilho(tud_id, tau_id, tud_idFilho);

                    HttpContext.Current.Cache.Insert(chave, lista, null, DateTime.Now.AddMinutes(AppMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                    lista = (List<CLS_TurmaAulaRecursoRegencia>)cache;
            }
            else
                lista = new CLS_TurmaAulaRecursoRegenciaDAO().SelectBy_tud_id_tau_id_tud_idFilho(tud_id, tau_id, tud_idFilho);

            return lista; 
        }

        /// <summary>
        /// Altera/Inclui os recursos de aula regente da tabela
        /// </summary>
        /// <param name="ltTurmaAulaRecursoRegencia"></param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static bool SalvaRecursosAulaRegencia(List<CLS_TurmaAulaRecursoRegencia> ltTurmaAulaRecursoRegencia, TalkDBTransaction banco = null)
        {
            DataTable dtTurmaAulaRecursoRegencia = CLS_TurmaAulaRecursoRegencia.TipoTabela_TurmaAulaRecursoRegencia();
            ltTurmaAulaRecursoRegencia.ForEach(p =>
                                                {
                                                    if (p.Validate())
                                                        dtTurmaAulaRecursoRegencia.Rows.Add(TurmaAulaRecursoRegenciaToDataRow(p, dtTurmaAulaRecursoRegencia.NewRow()));
                                                    else
                                                        throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(p));
                                                });
            return banco == null ?
                new CLS_TurmaAulaRecursoRegenciaDAO().SalvaRecursosAulaRegencia(dtTurmaAulaRecursoRegencia) :
                new CLS_TurmaAulaRecursoRegenciaDAO { _Banco = banco }.SalvaRecursosAulaRegencia(dtTurmaAulaRecursoRegencia);
        }

        /// <summary>
        /// Retorna um datarow com dados de um recurso de sala de aula de componente da regência.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static DataRow TurmaAulaRecursoRegenciaToDataRow(CLS_TurmaAulaRecursoRegencia entity, DataRow dr, DateTime trr_dataAlteracao = new DateTime())
        {
            if (entity.idAula > 0)
                dr["idAula"] = entity.idAula;
            else
                dr["idAula"] = DBNull.Value;

            dr["tud_id"] = entity.tud_id;
            dr["tau_id"] = entity.tau_id;
            dr["tud_idFilho"] = entity.tud_idFilho;

            if (entity.rsa_id > 0)
                dr["rsa_id"] = entity.rsa_id;
            else
                dr["rsa_id"] = DBNull.Value;

            if (!string.IsNullOrEmpty(entity.trr_observacao))
                dr["trr_observacao"] = entity.trr_observacao;
            else
                dr["trr_observacao"] = DBNull.Value;

            if (trr_dataAlteracao != new DateTime())
                dr["trr_dataAlteracao"] = trr_dataAlteracao;
            else
                dr["trr_dataAlteracao"] = DBNull.Value;

            return dr;
        }

        /// <summary>
        /// Salva a entidade e realiza exclusão de possíves caches.
        /// </summary>
        /// <param name="entity">CLS_TurmaAulaRecursoRegencia</param>
        /// <param name="banco">Conexão banco de dados</param>
        /// <returns></returns>
        public static bool Salvar(CLS_TurmaAulaRecursoRegencia entity, TalkDBTransaction banco)
        {
            string chave = retornarChave_GetSelectBy_Turma_Aula(entity.tud_id, entity.tau_id);

            if (HttpContext.Current.Cache[chave] != null)
                GestaoEscolarUtilBO.LimpaCache(chave);

            return Save(entity, banco);
        }
    }
}