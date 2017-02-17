/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using MSTech.Business.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System;
using MSTech.Validation.Exceptions;
using System.Web;
using System.Linq;

namespace MSTech.GestaoEscolar.BLL
{

    #region Estrutura

    /// <summary>
    /// Estrutura utilizada no combo de tipo de ciclo.
    /// </summary>
    public struct sComboTipoCiclo
    {
        public string tci_id { get; set; }
        public string tci_nome { get; set; }
    }

    #endregion

    /// <summary>
    /// ACA_TipoCiclo Business Object 
    /// </summary>
    public class ACA_TipoCicloBO : BusinessBase<ACA_TipoCicloDAO, ACA_TipoCiclo>
    {
        public static string RetornaChaveCache_SelecionaTipoCicloAtivos()
        {
            return "Cache_SelecionaTipoCicloAtivos";
        }

        public static string RetornaChaveCache_SelecionaCicloPorCursoCurriculo(int cur_id, int crr_id)
        {
            return String.Format("Cache_SelecionaCicloPorCursoCurriculo_{0}_{1}", cur_id, crr_id);
        }

        #region Saves
        
        /// <summary>
        /// Verifica o maior número de ordem cadastado de tipo de ciclo
        /// </summary>  
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static int SelecionaMaiorOrdem()
        {
            ACA_TipoCicloDAO dao = new ACA_TipoCicloDAO();
            return dao.Select_MaiorOrdem();
        }

        /// <summary>
        /// Altera a ordem do tipo ciclo tci_ordem
        /// </summary>
        /// <param name="entitySubir">Entidade do tipo de ciclo do calendário</param>
        /// <param name="entityDescer">Entidade do tipo de ciclo do calendário</param>        
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static bool SaveOrdem
        (
            ACA_TipoCiclo entityDescer
            , ACA_TipoCiclo entitySubir
        )
        {
            ACA_TipoCicloDAO dao = new ACA_TipoCicloDAO();

            if (entityDescer.Validate())
                dao.Salvar(entityDescer);
            else
                throw new ValidationException(entityDescer.PropertiesErrorList[0].Message);

            if (entitySubir.Validate())
                dao.Salvar(entitySubir);
            else
                throw new ValidationException(entitySubir.PropertiesErrorList[0].Message);

            return true;
        }
        
        #endregion
        
        #region Consultas

        /// <summary>
        /// Retorna os tipos de ciclo de aprendizagem ativos
        /// </summary>
        /// <returns>Lista com os tipos</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionarAtivos()
        {
            ACA_TipoCicloDAO dao = new ACA_TipoCicloDAO();
            return dao.SelecionarAtivos(out totalRecords);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboTipoCiclo> SelecionaTipoCicloAtivos(int appMinutosCacheLongo = 0)
        {
            List<sComboTipoCiclo> dados = null;

            if (appMinutosCacheLongo > 0 && HttpContext.Current != null)
            {
                string chave = RetornaChaveCache_SelecionaTipoCicloAtivos();
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    using (DataTable dt = new ACA_TipoCicloDAO().SelecionarAtivos(out totalRecords))
                    {
                        dados = (from DataRow dr in dt.Rows
                                 select new sComboTipoCiclo
                                 {
                                     tci_id = dr["tci_id"].ToString()
                                     ,
                                     tci_nome = dr["tci_nome"].ToString()
                                 }).ToList();
                    }

                    HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    dados = (List<sComboTipoCiclo>)cache;
                }
            }

            if (dados == null)
            {
                using (DataTable dt = new ACA_TipoCicloDAO().SelecionarAtivos(out totalRecords))
                {
                    dados = (from DataRow dr in dt.Rows
                             select new sComboTipoCiclo
                             {
                                 tci_id = dr["tci_id"].ToString()
                                 ,
                                 tci_nome = dr["tci_nome"].ToString()
                             }).ToList();
                }
            }

            return dados;
        }

        /// <summary>
        /// Retorna as informações do tipo do ciclo do aluno
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <returns>DataTable com os dados</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaDadosDoCicloDoAluno
        (
            long alu_id
        )
        {
            ACA_TipoCicloDAO dao = new ACA_TipoCicloDAO();
            return dao.SelecionaDadosDoCicloDoAluno(alu_id);
        }

        /// <summary>
        /// Retorna True para exibir os campos referente ao compromisso do aluno
        /// e falso para não exibir
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaSeExibeCompromissoAluno
        (
            long alu_id
        )
        {
            DataTable dtTipoCiclo = SelecionaDadosDoCicloDoAluno(alu_id);

            if (dtTipoCiclo.Rows.Count <= 0)
            {
                return false;
            }
            else if (dtTipoCiclo.Rows[0]["tci_exibirBoletim"] is DBNull)
            {
                return false;
            }
            return Convert.ToInt32(dtTipoCiclo.Rows[0]["tci_exibirBoletim"]) == 0 ? false : true;
        }


        /// <summary>
        /// Carrega os ciclos vinculados ao curso/curriculo período não excluídos logicamente
        /// filtrados por curso e currículo
        /// </summary>
        /// <param name="cur_id">ID de Curso</param>
        /// <param name="crr_id">ID de curriculo</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboTipoCiclo> SelecionaCicloPorCursoCurriculo
        (
            int cur_id, 
            int crr_id,
            int appMinutosCacheLongo = 0
        )
        {
            List<sComboTipoCiclo> dados = null;

            if (appMinutosCacheLongo > 0 && HttpContext.Current != null)
            {
                string chave = RetornaChaveCache_SelecionaCicloPorCursoCurriculo(cur_id, crr_id);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    using (DataTable dt = new ACA_TipoCicloDAO().SelecionaCicloPorCursoCurriculo(cur_id, crr_id))
                    {
                        dados = (from DataRow dr in dt.Rows
                             select new sComboTipoCiclo
                             {
                                 tci_id = dr["tci_id"].ToString()
                                 ,
                                 tci_nome = dr["tci_nome"].ToString()
                             }).ToList();
                    }

                    HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    dados = (List<sComboTipoCiclo>)cache;
                }
            }

            if (dados == null)
            {
                using (DataTable dt = new ACA_TipoCicloDAO().SelecionaCicloPorCursoCurriculo(cur_id, crr_id))
                    {
                        dados = (from DataRow dr in dt.Rows
                             select new sComboTipoCiclo
                             {
                                 tci_id = dr["tci_id"].ToString()
                                 ,
                                 tci_nome = dr["tci_nome"].ToString()
                             }).ToList();
                    }
            }

            return dados;
        }

        #endregion

    }
}