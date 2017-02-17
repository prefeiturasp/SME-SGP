/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
    using System.Data;
    using System;
    using System.Collections.Generic;
	
	/// <summary>
	/// Description: ACA_AlunoAnotacao Business Object. 
	/// </summary>
	public class ACA_AlunoAnotacaoBO : BusinessBase<ACA_AlunoAnotacaoDAO, ACA_AlunoAnotacao>
	{
        /// <summary>
        /// Seleciona as anotações do aluno no ano informado
        /// </summary>
        /// <param name="alu_id">Id do Aluno</param>
        /// <param name="cal_ano">Ano letivo</param>
        /// <returns></returns>
        public static List<ACA_AlunoAnotacao> SelecionaAnotacoesAluno(Int64 alu_id, int cal_ano)
        {
            ACA_AlunoAnotacaoDAO dao = new ACA_AlunoAnotacaoDAO();
            DataTable dt = dao.SelecionaAnotacoesAluno(alu_id, cal_ano);
            
            List<ACA_AlunoAnotacao> listaAlunoAnotacao = new List<ACA_AlunoAnotacao>();
            foreach(DataRow dr in dt.Rows)
            {
                ACA_AlunoAnotacao alunoAnotacao = new ACA_AlunoAnotacao();
                alunoAnotacao = dao.DataRowToEntity(dr, alunoAnotacao);

                listaAlunoAnotacao.Add(alunoAnotacao);
            }

            return listaAlunoAnotacao;
        }
	}
}