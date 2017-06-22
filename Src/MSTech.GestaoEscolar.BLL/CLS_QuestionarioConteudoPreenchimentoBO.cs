/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
	
	/// <summary>
	/// Description: CLS_QuestionarioConteudoPreenchimento Business Object. 
	/// </summary>
	public class CLS_QuestionarioConteudoPreenchimentoBO : BusinessBase<CLS_QuestionarioConteudoPreenchimentoDAO, CLS_QuestionarioConteudoPreenchimento>
	{
        /// <summary>
        /// Retorna se o conteúdo foi preenchido.
        /// </summary>
        /// <param name="qtc_id">Id do conteúdo.</param>
        /// <returns></returns>
		public static bool ConteudoPreenchido
           (
                string qtc_ids
           )
        {
            CLS_QuestionarioConteudoPreenchimentoDAO dao = new CLS_QuestionarioConteudoPreenchimentoDAO();
            return dao.SelecionaConteudoPreenchido(qtc_ids).Rows.Count > 0;
        }		
	}
}