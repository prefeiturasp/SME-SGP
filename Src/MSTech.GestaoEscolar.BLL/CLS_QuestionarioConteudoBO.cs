/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System.Data;
    using System.ComponentModel;
    using Validation.Exceptions;
    using System.Collections.Generic;
    #region Enumeradores

    public enum QuestionarioTipoConteudo
    {
        [Description("Título 1")]
        Titulo1 = 1,
        [Description("Título 2")]
        Titulo2,
        [Description("Texto")]
        Texto,
        [Description("Pergunta")]
        Pergunta
    }

    public enum QuestionarioTipoResposta
    {
        [Description("Múltipla seleção")]
        MultiplaSelecao = 1,
        [Description("Seleção única")]
        SelecaoUnica,
        [Description("Texto aberto")]
        TextoAberto
    }

    #endregion

    public class QuestionarioConteudo : CLS_QuestionarioConteudo
    {
        public List<CLS_QuestionarioResposta> lstRepostas { get; set; }
    }

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
        public static DataTable SelectByQuestionarioPaginado
           (
                int qst_id
                , int currentPage
                , int pageSize
           )
        {
            totalRecords = 0;

            if (pageSize == 0)
                pageSize = 1;

            CLS_QuestionarioConteudoDAO dao = new CLS_QuestionarioConteudoDAO();
            return dao.SelectByQuestionario(true, currentPage / pageSize, pageSize, qst_id, out totalRecords);
        }

        /// <summary>
        /// Altera a ordem do conteúdo
        /// </summary>
        /// <param name="entitySubir">Entidade do conteúdo</param>
        /// <param name="entityDescer">Entidade do conteúdo</param>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static bool SaveOrdem
        (
            CLS_QuestionarioConteudo entityDescer
            , CLS_QuestionarioConteudo entitySubir
        )
        {
            CLS_QuestionarioConteudoDAO dao = new CLS_QuestionarioConteudoDAO();

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