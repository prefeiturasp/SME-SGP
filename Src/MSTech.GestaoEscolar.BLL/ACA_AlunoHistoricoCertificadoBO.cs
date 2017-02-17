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
	/// Description: ACA_AlunoHistoricoCertificado Business Object. 
	/// </summary>
	public class ACA_AlunoHistoricoCertificadoBO : BusinessBase<ACA_AlunoHistoricoCertificadoDAO, ACA_AlunoHistoricoCertificado>
	{
		public static DataTable SelecionaPorAluno(long alu_id)
        {
            ACA_AlunoHistoricoCertificadoDAO dao = new ACA_AlunoHistoricoCertificadoDAO();
            return dao.SelecionaPorAluno(alu_id);
        }

        public static DataTable SelecionaAnosDisponiveis(long alu_id)
        {
            ACA_AlunoHistoricoCertificadoDAO dao = new ACA_AlunoHistoricoCertificadoDAO();
            return dao.SelecionaAnosDisponiveis(alu_id);
        }
	}
}