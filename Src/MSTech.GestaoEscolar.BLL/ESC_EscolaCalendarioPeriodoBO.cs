/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
    using System.Data;
    using MSTech.Data.Common;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using MSTech.GestaoEscolar.BLL.Caching;
	
	/// <summary>
	/// Description: ESC_EscolaCalendarioPeriodo Business Object. 
	/// </summary>
	public class ESC_EscolaCalendarioPeriodoBO : BusinessBase<ESC_EscolaCalendarioPeriodoDAO, ESC_EscolaCalendarioPeriodo>
	{
        /// <summary>
        /// Retorna os períodos da escola.
        /// </summary>
        /// <param name="esc_id">Id da escola.</param>
        /// <returns></returns>
        public static DataTable SelectEscolaPeriodos(int esc_id)
        {
            ESC_EscolaCalendarioPeriodoDAO dao = new ESC_EscolaCalendarioPeriodoDAO();
            return dao.SelectEscolaPeriodos(esc_id);
        }

        /// <summary>
        /// Retorna os períodos da escola.
        /// </summary>
        /// <param name="cal_id">Id do calendário.</param>
        /// <returns></returns>
        public static List<ESC_EscolaCalendarioPeriodo> SelectEscolaPeriodosCalendario(int cal_id)
        {
            ESC_EscolaCalendarioPeriodoDAO dao = new ESC_EscolaCalendarioPeriodoDAO();
            return dao.SelectEscolaPeriodosCalendario(cal_id);
        }

        /// <summary>
        /// Retorna os períodos da escola.
        /// </summary>
        /// <param name="cal_id">Id do calendário.</param>
        /// <param name="appMinutosCacheCurto">Cache curto.</param>
        /// <returns></returns>
        public static List<ESC_EscolaCalendarioPeriodo> SelectEscolasCalendarioCache(int cal_id, int appMinutosCacheCurto)
        {
            List<ESC_EscolaCalendarioPeriodo> dados = null;
            if (appMinutosCacheCurto > 0)
            {
                // Chave padrão do cache - nome do método + parâmetros.
                string chave = RetornaChaveCache_EscolaPeriodosCalendario(cal_id);

                dados = CacheManager.Factory.Get
                            (
                                chave,
                                () =>
                                {
                                    return SelectEscolaPeriodosCalendario(cal_id);
                                },
                                appMinutosCacheCurto
                            );
            }
            else
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                dados = SelectEscolaPeriodosCalendario(cal_id);
            }

            return dados;
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para a consulta.
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_EscolaPeriodosCalendario(int cal_id)
        {
            return string.Format(ModelCache.ESCOLA_PERIODO_CALENDARIO_POR_CALENDARIO_MODEL_KEY, cal_id);
        }

        public static void RemoveCacheEscolaCalendarioPeriodo(int cal_id)
        {
            // Chave padrão do cache - nome do método + parâmetros.
            string chave = RetornaChaveCache_EscolaPeriodosCalendario(cal_id);
            CacheManager.Factory.Remove(chave);
        }

        public static bool SaveList(int esc_id, List<ESC_EscolaCalendarioPeriodo> listPeriodos)
        {
            ESC_EscolaDAO daoEscola = new ESC_EscolaDAO();
            TalkDBTransaction bancoGestao = daoEscola._Banco;
            bancoGestao.Open();
            daoEscola._Banco = bancoGestao;

            try
            {
                DataTable dtPeriodosEscola = SelectEscolaPeriodos(esc_id);

                foreach (ESC_EscolaCalendarioPeriodo escolaPeriodo in listPeriodos)
                {
                    bool existePeriodo = dtPeriodosEscola.Rows.Cast<DataRow>().Any(row => row["tpc_id"].ToString().Equals(escolaPeriodo.tpc_id.ToString()) && row["cal_id"].ToString().Equals(escolaPeriodo.cal_id.ToString()));

                    if (!existePeriodo)
                    {
                        escolaPeriodo.esc_id = esc_id;
                        Save(escolaPeriodo, bancoGestao);
                    }

                    RemoveCacheEscolaCalendarioPeriodo(escolaPeriodo.cal_id);
                }

                // Se exister no DataTable, mas não exister na Lista -> Exclui
                foreach (DataRow row in dtPeriodosEscola.Rows)
                {
                    ESC_EscolaCalendarioPeriodo escolaPeriodo = listPeriodos.Find(p => p.cal_id == Convert.ToInt32(row["cal_id"]) && p.tpc_id == Convert.ToInt32(row["tpc_id"]));
                    if (escolaPeriodo == null)
                    {
                        ESC_EscolaCalendarioPeriodo entityEscolaPeriodo = new ESC_EscolaCalendarioPeriodo
                        {
                            esc_id = esc_id,
                            cal_id = Convert.ToInt32(row["cal_id"].ToString()),
                            tpc_id = Convert.ToInt32(row["tpc_id"].ToString())
                        };

                        Delete(entityEscolaPeriodo, bancoGestao);
                    }
                }
            }
            catch (Exception ex)
            {
                bancoGestao.Close(ex);
                throw (ex);
            }
            finally
            {
                if (bancoGestao.ConnectionIsOpen)
                {
                    bancoGestao.Close();
                }
            }

            return true;
        }
	}
}