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
using System.Collections.Generic;
using System;
    using System.Linq;
	/// <summary>
	/// Description: CLS_AlunoPlanejamentoRelacionada Business Object. 
	/// </summary>
	public class CLS_AlunoPlanejamentoRelacionadaBO : BusinessBase<CLS_AlunoPlanejamentoRelacionadaDAO, CLS_AlunoPlanejamentoRelacionada>
	{
        /// <summary>
        /// Seleciona as turmadisciplinas relacionadas ao planejamento do aluno
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="tud_id">ID da turmadisciplina</param>
        /// <param name="apl_id">ID do planejamento do aluno</param>
        /// <returns></returns>
        public static DataTable SelecionaPlanejamentoAlunoRelacionada(long alu_id, long tud_id, int apl_id)
        {
            CLS_AlunoPlanejamentoRelacionadaDAO dao = new CLS_AlunoPlanejamentoRelacionadaDAO();
            return dao.SelecionaPlanejamentoAlunoRelacionada(alu_id, tud_id, apl_id);
        }
        
        /// <summary>
        /// Remove todas turmadisciplinas relacionadas ao planejamento do aluno
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="tud_id">ID da turmadisciplina</param>
        /// <param name="apl_id">ID do planejamento do aluno</param>
        public static void LimparRelacionadas(long alu_id, long tud_id, int apl_id, TalkDBTransaction banco)
        {
            CLS_AlunoPlanejamentoRelacionadaDAO dao = new CLS_AlunoPlanejamentoRelacionadaDAO();
            dao._Banco = banco;
            dao.LimparRelacionadas(alu_id, tud_id, apl_id);
        }

        /// <summary>
        /// Remove todas turmadisciplinas relacionadas ao planejamento do aluno
        /// </summary>
        /// <param name="alu_ids">ID do aluno</param>
        /// <param name="tud_ids">ID da turma disciplina</param>
        /// <param name="apl_ids">ID da disciplina</param>
        public static void LimparRelacionadas(List<String> lstAlunoPlanejamento,
                                              List<String> lstTurmaDisciplina,
                                              List<String> lstIdsPlanejamento,  
                                              TalkDBTransaction banco)
        {
            String alu_ids = lstAlunoPlanejamento.Aggregate((current, next) => current + "," + next);
            String tud_ids = lstTurmaDisciplina.Aggregate((current, next) => current + "," + next);
            String apl_ids = lstIdsPlanejamento.Aggregate((current, next) => current + "," + next);

            CLS_AlunoPlanejamentoRelacionadaDAO dao = new CLS_AlunoPlanejamentoRelacionadaDAO();
            dao._Banco = banco;
            dao.LimparRelacionadas(alu_ids, tud_ids, apl_ids);
        }
    }
}