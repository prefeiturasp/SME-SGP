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
	
	/// <summary>
	/// Description: CLS_CompensacaoAusenciaAluno Business Object. 
	/// </summary>
	public class CLS_CompensacaoAusenciaAlunoBO : BusinessBase<CLS_CompensacaoAusenciaAlunoDAO, CLS_CompensacaoAusenciaAluno>
	{
        /// <summary>
        /// Retorna todos os registros com cpa_id igual o parametro.
        /// </summary>
        /// <param name="cpa_id"></param>
        /// <returns></returns>
        public static List<CLS_CompensacaoAusenciaAluno> SelectByCpa_id(int cpa_id, long tud_id)
        {
            CLS_CompensacaoAusenciaAlunoDAO dao = new CLS_CompensacaoAusenciaAlunoDAO();
            return dao.SelectByCpa_id(cpa_id, tud_id);
        }

        /// <summary>
        /// Deleta todos os alunos da compensacao
        /// </summary>
        /// <param name="tud_id">Disciplina</param>
        /// <param name="cpa_id">Compensacao</param>
        public static void DeletarAlunosDaCompensacao(long tud_id, int cpa_id, TalkDBTransaction banco)
        {
            List<CLS_CompensacaoAusenciaAluno> list = SelectByCpa_id(cpa_id, tud_id);
            foreach (CLS_CompensacaoAusenciaAluno aluno in list)
            {
                aluno.caa_situacao = 3;
                Save(aluno, banco);
            }
        }
			
	}
}