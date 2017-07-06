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
    using System;

    [Serializable]
    public class QuestionarioConteudoResposta
    {
        public int qtr_id { get; set; }
        public int qtc_id { get; set; }
        public int qst_id { get; set; }
        public string qtr_texto { get; set; }
        public string qtc_texto { get; set; }
        public string qst_titulo { get; set; }
        public bool IsNew { get; set; }
    }

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

        public static DataTable SelectByConteudo
       (
            int qtc_id
       )
        {
            CLS_QuestionarioRespostaDAO dao = new CLS_QuestionarioRespostaDAO();
            return dao.SelectByConteudo(qtc_id);
        }

        public static QuestionarioConteudoResposta GetEntityQuestionarioConteudoResposta
            (
                QuestionarioConteudoResposta entity
            )
        {
            CLS_QuestionarioRespostaDAO dao = new CLS_QuestionarioRespostaDAO();
            DataTable dt = dao.GetEntityQuestionarioConteudoResposta(entity.qtr_id);
            
            if (dt.Rows.Count > 0)
            {
                entity.qst_id = dt.Rows[0].Field<int>("qst_id");
                entity.qst_titulo = dt.Rows[0].Field<string>("qst_titulo");
                entity.qtc_id = dt.Rows[0].Field<int>("qtc_id");
                entity.qtc_texto = dt.Rows[0].Field<string>("qtc_texto");
                entity.qtr_id = dt.Rows[0].Field<int>("qtr_id");
                entity.qtr_texto = dt.Rows[0].Field<string>("qtr_texto");
            }

            return entity;
        }

        public static DataTable SelectByConteudoTipoResposta
       (
            int qtc_id
            , byte tipoResposta
       )
        {
            CLS_QuestionarioRespostaDAO dao = new CLS_QuestionarioRespostaDAO();
            return dao.SelectByConteudo(qtc_id).AsEnumerable()
                        .Where(row => row.Field<byte>("qtc_tipoResposta") == tipoResposta)
                        .CopyToDataTable(); ;
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