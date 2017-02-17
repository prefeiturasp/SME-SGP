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
	/// Description: CFG_ObservacaoBoletim Business Object. 
	/// </summary>
	public class CFG_ObservacaoBoletimBO : BusinessBase<CFG_ObservacaoBoletimDAO, CFG_ObservacaoBoletim>
	{
        /// <summary>
        /// Retorna todas as observações de boletim ativas.
        /// </summary>
        /// <returns></returns>
        public DataTable SelectAtivos()
        {
            CFG_ObservacaoBoletimDAO dao = new CFG_ObservacaoBoletimDAO();
            return dao.SelectAtivos();
        }
				
	}
}