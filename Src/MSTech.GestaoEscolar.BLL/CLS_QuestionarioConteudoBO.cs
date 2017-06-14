/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System.Data;

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

        /// <summary>
        ///Busca os conteúdos filtrado por questionário
        /// </summary>
        /// <param name="qst_id"></param>
        /// <returns></returns>
        public static DataTable SelectByQuestionario
           (
                int qst_id
           )
        {
            CLS_QuestionarioConteudoDAO dao = new CLS_QuestionarioConteudoDAO();
            return dao.SelectByQuestionario(qst_id);
        }

    }
}