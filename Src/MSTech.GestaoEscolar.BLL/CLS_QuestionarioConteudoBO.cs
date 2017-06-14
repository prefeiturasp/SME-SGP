/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;

    #region Enumeradores

    public enum TipoConteudo
    {
        [StringValue("Título 1")]
        Titulo1 = 1,
        [StringValue("Título 2")]
        Titulo2,
        [StringValue("Texto")]
        Texto,
        [StringValue("Pergunta")]
        Pergunta
    }

    #endregion

    /// <summary>
    /// Description: CLS_QuestionarioConteudo Business Object. 
    /// </summary>
    public class CLS_QuestionarioConteudoBO : BusinessBase<CLS_QuestionarioConteudoDAO, CLS_QuestionarioConteudo>
	{
				
	}
}