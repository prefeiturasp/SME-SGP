/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Linq;

    #region Enum

    /// <summary>
    /// Situações das ações realizadas
    /// </summary>
    public enum CLS_RelatorioPreenchimentoAcoesRealizadasSituacao : byte
    {
        Ativo = 1
        ,

        Excluido = 3
    }

    #endregion Enum

    #region Estruturas

    /// <summary>
    /// Estrutura para controle das ações realizadas.
    /// </summary>
    [Serializable]
    public struct sAcoesRealizadas
    {
        public long rpa_id { get; set; }
        public string rpa_data { get; set; }
        public bool rpa_impressao { get; set; }
        public string rpa_acao { get; set; }
        public int idTemp { get; set; }
        public bool excluido { get; set; }
    }

    #endregion

    /// <summary>
    /// Description: CLS_RelatorioPreenchimentoAcoesRealizadas Business Object. 
    /// </summary>
    public class CLS_RelatorioPreenchimentoAcoesRealizadasBO : BusinessBase<CLS_RelatorioPreenchimentoAcoesRealizadasDAO, CLS_RelatorioPreenchimentoAcoesRealizadas>
	{
        #region Consulta

        /// <summary>
        /// Retorna as ações realizadas cadastradas em um preenchimento de relatório.
        /// </summary>
        /// <param name="reap_id">Id do preenchimento de relatório</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sAcoesRealizadas> SelecionaPorPreenchimento(long reap_id)
        {
            DataTable dt = new CLS_RelatorioPreenchimentoAcoesRealizadasDAO().SelecionaPorPreenchimento(reap_id);
            List<sAcoesRealizadas> retorno = (from DataRow dr in dt.Rows
                                              select new sAcoesRealizadas
                                              {
                                                  rpa_id = Convert.ToInt64(dr["rpa_id"])
                                                  ,
                                                  rpa_data = dr["rpa_data"].ToString()
                                                  ,
                                                  rpa_impressao = Convert.ToBoolean(dr["rpa_impressao"])
                                                  ,
                                                  rpa_acao = dr["rpa_acao"].ToString()
                                                  ,
                                                  idTemp = -1
                                                   ,
                                                  excluido = false
                                              }).ToList();
            return retorno;
        }

        #endregion Consulta		
    }
}