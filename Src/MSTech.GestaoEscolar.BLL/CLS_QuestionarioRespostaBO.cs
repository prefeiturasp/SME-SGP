/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System.ComponentModel;
    using System.Data;
    using Validation.Exceptions;
    
    /// <summary>
    /// Description: CLS_QuestionarioResposta Business Object. 
    /// </summary>
    public class CLS_QuestionarioRespostaBO : BusinessBase<CLS_QuestionarioRespostaDAO, CLS_QuestionarioResposta>
    {
        public static DataTable SelectByConteudoPaginado
       (
            int qtc_id
            , int currentPage
            , int pageSize
       )
        {
            CLS_QuestionarioRespostaDAO dao = new CLS_QuestionarioRespostaDAO();
            return dao.SelectByConteudo(true, currentPage / pageSize, pageSize, qtc_id, out totalRecords);
        }

        public static DataTable SelectQuestionarioConteudoRespostaMultiplaSelecao_By_rea_id
       (
            int rea_id
       )
        {
            CLS_QuestionarioRespostaDAO dao = new CLS_QuestionarioRespostaDAO();
            return dao.SelectQuestionarioConteudoRespostaMultiplaSelecao_By_rea_id(rea_id);
        }

        /// <summary>
        /// Altera a ordem da resposta
        /// </summary>
        /// <param name="entitySubir">Entidade do conteúdo</param>
        /// <param name="entityDescer">Entidade do conteúdo</param>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static bool SaveOrdem
        (
            CLS_QuestionarioResposta entityDescer
            , CLS_QuestionarioResposta entitySubir
        )
        {
            CLS_QuestionarioRespostaDAO dao = new CLS_QuestionarioRespostaDAO();

            if (entityDescer.Validate())
                dao.Salvar(entityDescer);
            else
                throw new ValidationException(entityDescer.PropertiesErrorList[0].Message);

            if (entitySubir.Validate())
                dao.Salvar(entitySubir);
            else
                throw new ValidationException(entitySubir.PropertiesErrorList[0].Message);

            return true;
        }
    }
}