using MSTech.Business.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL;
using System.Collections.Generic;
using System.Web;
using System;
using MSTech.Validation.Exceptions;
using System.Data;
using System.Linq;

namespace MSTech.GestaoEscolar.BLL
{
    #region Estrutura

    [Serializable]
    public struct sFaixaRelatorioCor
    {
        public int far_id { get; set; }
		public int rlt_id { get; set; }
		public string far_descricao { get; set; }
		public string far_inicio { get; set; }
		public string far_fim { get; set; }
		public string eap_valor { get; set; }
        public int eap_ordem { get; set; }
        public string eap_abreviatura { get; set; }
		public string far_cor { get; set; }
		public int esa_id { get; set; }
        public byte esa_tipo { get; set; }
        public string esa_nome { get; set; }
		public int eap_id { get; set; }
		public byte far_situacao { get; set; }
        public bool IsNew { get; set; }
    }

    #endregion

    /// <summary>
	/// Description: CFG_FaixaRelatorio Business Object. 
	/// </summary>
    public class CFG_FaixaRelatorioBO : BusinessBase<CFG_FaixaRelatorioDAO, CFG_FaixaRelatorio>
    {
        /// <summary>
        /// Selecionas as faixas do relatorio.
        /// </summary>
        /// <param name="rlt_id">rlt_id.</param>
        /// <param name="appMinutosCacheLongo">Tempo de cache longo</param>
        /// <returns>descrição dos relátorios com suas respectivas faixas</returns>    
        /// <datetime>24/09/2014-10:44</datetime>
        public static List<sFaixaRelatorioCor> SelecionaCoresRelatorio(int rlt_id, int appMinutosCacheLongo = 0)
        {
            List<sFaixaRelatorioCor> dados = null;
            if (appMinutosCacheLongo > 0 && HttpContext.Current != null)
            {
                string chave = string.Format("Cache_SelecionaCoresRelatorio_{0}", rlt_id.ToString());
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    dados = (from DataRow dr in new CFG_FaixaRelatorioDAO().SelecionaFaixasRelatorio(rlt_id).Rows
                             select (sFaixaRelatorioCor)GestaoEscolarUtilBO.DataRowToEntity(dr, new sFaixaRelatorioCor())).ToList();

                    // Adiciona cache com validade do tempo informado na configuração.
                    HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    dados = (List<sFaixaRelatorioCor>)cache;
                }
            }

            if (dados == null)
            {
                dados = (from DataRow dr in new CFG_FaixaRelatorioDAO().SelecionaFaixasRelatorio(rlt_id).Rows
                         select (sFaixaRelatorioCor)GestaoEscolarUtilBO.DataRowToEntity(dr, new sFaixaRelatorioCor())).ToList();
            }

            return dados;
        }

        /// <summary>
        /// Salva a faixa de relatório
        /// </summary>
        /// <param name="entity">Entidade da faixa</param>
        /// <returns></returns>
        public new static bool Save(CFG_FaixaRelatorio entity)
        {
            try
            {
                if (entity.Validate())
                {
                    CFG_FaixaRelatorioDAO dao = new CFG_FaixaRelatorioDAO();

                    // Removendo o cache
                    string chave = string.Format("Cache_SelecionaCoresRelatorio_{0}", entity.rlt_id.ToString());

                    if (HttpContext.Current.Cache[chave] != null)
                        HttpContext.Current.Cache.Remove(chave);

                    dao.Salvar(entity);
                }
                else
                {
                    throw new ValidationException(entity.PropertiesErrorList[0].Message);
                }

                return true;
            }
            catch
            {
                throw;
            }
        }

        public new static bool Delete(CFG_FaixaRelatorio entity)
        {
            try
            {
                CFG_FaixaRelatorioDAO dao = new CFG_FaixaRelatorioDAO();

                // Removendo o cache
                string chave = string.Format("Cache_SelecionaCoresRelatorio_{0}", entity.rlt_id.ToString());

                if (HttpContext.Current.Cache[chave] != null)
                    HttpContext.Current.Cache.Remove(chave);

                dao.Delete(entity);

                return true;
            }
            catch
            {
                throw;
            }
        }
    }    
}