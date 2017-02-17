/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
    using System.Data;
	
	/// <summary>
	/// Description: ACA_CompromissoEstudo Business Object. 
	/// </summary>
	public class ACA_CompromissoEstudoBO : BusinessBase<ACA_CompromissoEstudoDAO, ACA_CompromissoEstudo>
	{
        /// <summary>
        /// Busca o compromisso de estudo (autoavaliação) do aluno
        /// </summary>
        /// <param name="alu_id"></param>
        /// <returns></returns>
        public static DataTable GetSelectCompromissoAlunoBy_alu_id(long alu_id, int cpe_ano)
        {
            ACA_CompromissoEstudoDAO dao = new ACA_CompromissoEstudoDAO();
            return dao.SelectCompromissoAlunoBy_alu_id(alu_id, cpe_ano);
        }

        /// <summary>
        /// Busca o compromisso de estudo (autoavaliação) do aluno
        /// </summary>
        /// <param name="alu_id"></param>
        /// <returns></returns>
        public static DataTable SelectSituacaoTodosCompromissoAlunoBy_alu_id(long alu_id)
        {
            ACA_CompromissoEstudoDAO dao = new ACA_CompromissoEstudoDAO();
            return dao.SelectSituacaoTodosCompromissoAlunoBy_alu_id(alu_id);
        }
	}
}