/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Data;

    /// <summary>
    /// Situações do questionário do relatório
    /// </summary>
    public enum CLS_RelatorioAtendimentoQuestionarioSituacao : byte
    {
        Ativo = 1
        ,

        Excluido = 3
    }

    /// <summary>
    /// Description: CLS_RelatorioAtendimentoQuestionario Business Object. 
    /// </summary>
    public class CLS_RelatorioAtendimentoQuestionarioBO : BusinessBase<CLS_RelatorioAtendimentoQuestionarioDAO, CLS_RelatorioAtendimentoQuestionario>
    {
        /// <summary>
        /// Carrega os questionários para o relatório de atendimento
        /// </summary>
        /// <param name="rea_id">ID do relatorio de atendimento</param>
        /// <returns></returns>
        public static List<CLS_RelatorioAtendimentoQuestionario> SelectBy_rea_id(int rea_id)
        {
            CLS_RelatorioAtendimentoQuestionarioDAO dao = new CLS_RelatorioAtendimentoQuestionarioDAO();
            return dao.SelectBy_rea_id(rea_id);
        }

        public static DataTable SelectPerguntaMultiplaEscolha_By_rea_id(int rea_id)
        {
            CLS_RelatorioAtendimentoQuestionarioDAO dao = new CLS_RelatorioAtendimentoQuestionarioDAO();

            DataTable dt = dao.SelectBy_RelatorioAtendimento(rea_id);

            if (dt.AsEnumerable().Any(r => Convert.ToBoolean(r["PossuiPerguntaMultiplaEscolha"]) == true))
                dt = dt.AsEnumerable()
                        .Where(row => row.Field<bool>("PossuiPerguntaMultiplaEscolha") == true)
                        .CopyToDataTable();
            else dt = new DataTable();

            return dt;
        }
    }
}