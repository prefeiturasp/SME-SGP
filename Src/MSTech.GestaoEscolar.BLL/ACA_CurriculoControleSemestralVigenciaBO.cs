using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using MSTech.Business.Common;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using System.Web;
using System.Linq;

namespace MSTech.GestaoEscolar.BLL
{
    public class ACA_CurriculoEscolaPeriodoBO : BusinessBase<ACA_CurriculoEscolaPeriodoDAO, ACA_CurriculoEscolaPeriodo>
    {
        public static string RetornaChaveCache_SelecionaPorEscolaCursoCiclo(int cur_id, int crr_id, int esc_id, int uni_id, int tci_id)
        {
            return String.Format("Cache_SelecionaPorEscolaCursoCiclo_{0}_{1}_{2}_{3}_{4}", cur_id, crr_id, esc_id, uni_id, tci_id);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable CarregaCursosPeriodoReferenteEscola
        (
             int esc_id
             , bool paginado
             , int currentPage
             , int pageSize
        )
        {
            ACA_CurriculoEscolaPeriodoDAO dao = new ACA_CurriculoEscolaPeriodoDAO();
            return dao.SelectBy_esc_id(esc_id, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Selecionada os periodos da escola por curso e escola.
        /// </summary>
        /// <param name="esc_id">ID da escola.</param>
        /// <param name="uni_id">ID da unidade de escola.</param>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo.</param>
        /// <returns></returns>
        public static List<ACA_CurriculoEscolaPeriodo> SelecionaPorEscolaCurso(int esc_id, int uni_id, int cur_id, int crr_id, TalkDBTransaction banco = null)
        {
            return banco == null ?
                new ACA_CurriculoEscolaPeriodoDAO().SelecionaPorEscolaCurso(esc_id, uni_id, cur_id, crr_id) :
                new ACA_CurriculoEscolaPeriodoDAO { _Banco = banco }.SelecionaPorEscolaCurso(esc_id, uni_id, cur_id, crr_id);
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Save
        (
            ACA_CurriculoEscolaPeriodo entity
            , TalkDBTransaction banco)
        {
            if (entity.Validate())
            {
                ACA_CurriculoEscolaPeriodoDAO dao = new ACA_CurriculoEscolaPeriodoDAO { _Banco = banco };
                return dao.Salvar(entity);
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));           
        }

        /// <summary>
        /// Deleta logicamente uma ligação de currículo período com a escola
        /// </summary>
        /// <param name="entity">Entidade ACA_CurriculoEscolaPeriodo</param>        
        /// <param name="banco">Conexão aberta com o banco de dados</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>  
        /// <returns>True = deletado/alterado | False = não deletado/alterado</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Delete
        (
            ACA_CurriculoEscolaPeriodo entity
            , TalkDBTransaction banco
            , Guid ent_id
        )
        {
            ACA_CurriculoEscolaPeriodoDAO dao = new ACA_CurriculoEscolaPeriodoDAO();

            if (banco == null)
                dao._Banco.Open(IsolationLevel.ReadCommitted);
            else
                dao._Banco = banco;

            try
            {
                
                //Deleta logicamente o tipo de atendimento especial
                dao.Delete(entity);

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
        /// Seleciona os períodos da escola por curso e ciclo.
        /// </summary>
        /// <param name="cur_id"></param>
        /// <param name="crr_id"></param>
        /// <param name="esc_id"></param>
        /// <param name="uni_id"></param>
        /// <param name="tci_id"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboPeriodo> SelecionaPorEscolaCursoCiclo
        (
            int cur_id,
            int crr_id,
            int esc_id,
            int uni_id,
            int tci_id,
            int appMinutosCacheLongo = 0 
        )
        {
            List<sComboPeriodo> dados = null;

            if (appMinutosCacheLongo > 0 && HttpContext.Current != null)
            {
                string chave = RetornaChaveCache_SelecionaPorEscolaCursoCiclo(cur_id, crr_id, esc_id, uni_id, tci_id);
                object cache = HttpContext.Current.Cache[chave];

                totalRecords = 0;

                if (cache == null)
                {
                    using (DataTable dt = new ACA_CurriculoEscolaPeriodoDAO().SelectCurriculoEscolaPeriodoBy_CursoCiclo(cur_id, crr_id, esc_id, uni_id, tci_id))
                    {
                        dados = (from DataRow dr in dt.Rows
                                 select new sComboPeriodo
                                 {
                                     cur_id_crr_id_crp_id = dr["cur_id_crr_id_crp_id"].ToString()
                                     ,
                                     crp_descricao = dr["crp_descricao"].ToString()
                                 }).ToList();
                    }

                    HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    dados = (List<sComboPeriodo>)cache;
                }
            }

            if (dados == null)
            {
                using (DataTable dt = new ACA_CurriculoEscolaPeriodoDAO().SelectCurriculoEscolaPeriodoBy_CursoCiclo(cur_id, crr_id, esc_id, uni_id, tci_id))
                {
                    dados = (from DataRow dr in dt.Rows
                             select new sComboPeriodo
                             {
                                 cur_id_crr_id_crp_id = dr["cur_id_crr_id_crp_id"].ToString()
                                 ,
                                 crp_descricao = dr["crp_descricao"].ToString()
                             }).ToList();
                }
            }

            return dados;
        }

    }
}
