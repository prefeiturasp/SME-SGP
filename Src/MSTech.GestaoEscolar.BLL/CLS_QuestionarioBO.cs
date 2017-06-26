/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System.Data;
    using System.Collections.Generic;
    using System;
    using Data.Common;
    [Serializable]
    public class Questionario : CLS_Questionario
    {
        public int raq_id { get; set; }
        public int raq_ordem { get; set; }
        public List<QuestionarioConteudo> lstConteudo { get; set; }

        public Questionario()
        {
            lstConteudo = new List<QuestionarioConteudo>();
        }
    }

    /// <summary>
    /// Description: CLS_Questionario Business Object. 
    /// </summary>
    public class CLS_QuestionarioBO : BusinessBase<CLS_QuestionarioDAO, CLS_Questionario>
	{
        /// <summary>
        ///Busca os questionários filtrado por título
        /// </summary>
        /// <param name="qst_titulo"></param>
        /// <returns></returns>
        public static DataTable GetQuestionarioBy_qst_titulo
           (
                string qst_titulo
           )
        {
            CLS_QuestionarioDAO dao = new CLS_QuestionarioDAO();
            return dao.SelectBy_qst_titulo(qst_titulo);
        }

        /// <summary>
        /// Verifica se o questionário estáem uso no relatório
        /// </summary>
        /// <param name="qst_id">ID do questionário</param>
        /// <param name="rea_id">ID do relatório</param>
        /// <returns></returns>
        public static bool VerificaQuestionarioEmUso(int qst_id, int rea_id, TalkDBTransaction banco = null)
        {
            CLS_QuestionarioDAO dao = new CLS_QuestionarioDAO();
            if (banco != null)
                dao._Banco = banco;
            return dao.VerificaQuestionarioEmUso(qst_id, rea_id);
        }
    }
}