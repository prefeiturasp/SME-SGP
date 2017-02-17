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
    /// Description: ACA_AlunoJustificativaAbonoFalta Business Object. 
    /// </summary>
    public class ACA_AlunoJustificativaAbonoFaltaBO : BusinessBase<ACA_AlunoJustificativaAbonoFaltaDAO, ACA_AlunoJustificativaAbonoFalta>
	{
        /// <summary>
        /// Selecionar as justificativas por aluno e disciplina.
        /// </summary>
        /// <param name="alu_id">Id do aluno.</param>
        /// <param name="tud_id">Id da disciplina.</param>
        /// <returns>DataTable com os dados selecionados.</returns>
	    public static DataTable SelecionarPorAlunoETurmaDisciplina(long alu_id, long tud_id)
        {
            ACA_AlunoJustificativaAbonoFaltaDAO dao = new ACA_AlunoJustificativaAbonoFaltaDAO();
            return dao.SelecionarPorAlunoETurmaDisciplina(alu_id, tud_id);
        }
    }
}