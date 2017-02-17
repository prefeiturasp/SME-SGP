/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
    using System.Data;
    using System.Collections.Generic;
    using MSTech.Data.Common;
    using System;
	
	/// <summary>
	/// Description: CLS_PlanejamentoProjeto Business Object. 
	/// </summary>
	public class CLS_PlanejamentoProjetoBO : BusinessBase<CLS_PlanejamentoProjetoDAO, CLS_PlanejamentoProjeto>
	{

        /// <summary>
        /// Seleciona os projetos pela turmadisciplina
        /// </summary>
        /// <param name="tud_id">ID da turma disciplina</param>
        /// <returns></returns>
        public static DataTable CarregarProjetos(long tud_id)
        {
            CLS_PlanejamentoProjetoDAO dao = new CLS_PlanejamentoProjetoDAO();
            return dao.CarregarProjetos(tud_id);
        }

        /// <summary>
        /// Salva o planejamento de projeto com as disciplinas relacionadas
        /// </summary>
        /// <param name="planejamento">Entidade de planejamento de projeto</param>
        /// <param name="lstPlanRelacionada">Lista de disciplinas relacionadas</param>
        /// <returns></returns>
        public static bool SalvaPlanejamentoProjeto(CLS_PlanejamentoProjeto planejamento, List<CLS_PlanejamentoProjetoRelacionada> lstPlanRelacionada)
        {
            TalkDBTransaction banco = new CLS_AlunoPlanejamentoDAO()._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                Save(planejamento, banco);

                CLS_PlanejamentoProjetoRelacionadaBO.LimparRelacionadas(planejamento.esc_id, planejamento.uni_id, planejamento.cal_id, 
                                                                        planejamento.cur_id, planejamento.plp_id, banco);

                foreach (CLS_PlanejamentoProjetoRelacionada ppr in lstPlanRelacionada)
                {
                    ppr.plp_id = planejamento.plp_id;
                    CLS_PlanejamentoProjetoRelacionadaBO.Save(ppr, banco);
                }

                return true;
            }
            catch (Exception ex)
            {
                banco.Close(ex);
                throw;
            }
            finally
            {
                banco.Close();
            }
        }
    }
}