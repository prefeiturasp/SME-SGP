/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
    using System.ComponentModel;
    using MSTech.Data.Common;
    using System.Collections.Generic;
    using System;
    using System.Data;
	
	/// <summary>
	/// Description: ACA_AlunoAvaliacaoObservacao Business Object. 
	/// </summary>
	public class ACA_AlunoAvaliacaoObservacaoBO : BusinessBase<ACA_AlunoAvaliacaoObservacaoDAO, ACA_AlunoAvaliacaoObservacao>
	{
        /// <summary>
        /// Retorna um datatable contendo todas as Observações de Avaliações do Aluno
        /// que não foram excluídos logicamente, filtrados por
        /// alu_id
        /// </summary>
        /// <param name="alu_id">Id da tabela ACA_Aluno do bd</param>
        /// <param name="bancoGestao">Transação.</param>
        /// <returns>DataTable com as Observações de Avaliações do Aluno</returns>
        //[DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<ACA_AlunoAvaliacaoObservacao> SelecionaPorAluno(long alu_id, TalkDBTransaction bancoGestao)
        {
            ACA_AlunoAvaliacaoObservacaoDAO dao = new ACA_AlunoAvaliacaoObservacaoDAO();
            if (bancoGestao != null)
            {
                dao._Banco = bancoGestao;
            }
            return dao.SelectBy_alu_id(alu_id);
        }
	}
}