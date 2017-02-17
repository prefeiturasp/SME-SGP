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

    #region Enumerador

    /// <summary>
    /// Enumerador do tipo de recomendação
    /// </summary>
    public enum ACA_RecomendacaoAlunoResponsavelTipo : byte
    {
        Aluno = 1
        ,
        PaisResponsavel = 2
        ,
        Ambos = 3
    }

    #endregion 

    /// <summary>
	/// Description: ACA_RecomendacaoAlunoResponsavel Business Object. 
	/// </summary>
	public class ACA_RecomendacaoAlunoResponsavelBO : BusinessBase<ACA_RecomendacaoAlunoResponsavelDAO, ACA_RecomendacaoAlunoResponsavel>
	{
        public static DataTable SelecionaAtivos()
        {
            ACA_RecomendacaoAlunoResponsavelDAO dao = new ACA_RecomendacaoAlunoResponsavelDAO();
            return dao.SelecionaAtivos();
        }

        /// <summary>
        /// Seleciona as recomendações dos alunos/responsáveis por tipo
        /// </summary>
        /// <param name="rar_tipo">Tipo</param>
        /// <returns></returns>
        public static List<ACA_RecomendacaoAlunoResponsavel> SelecionarAtivosPorTipo(byte rar_tipo)
        {
            ACA_RecomendacaoAlunoResponsavelDAO dao = new ACA_RecomendacaoAlunoResponsavelDAO();
            return dao.SelecionarAtivosPorTipo(rar_tipo);
        }
	}
}