/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using Data.Common;
    using Entities;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using System.Data;
    /// <summary>
    /// Description: .
    /// </summary>
    public class CLS_AlunoDeficienciaDetalheDAO : Abstract_CLS_AlunoDeficienciaDetalheDAO
	{
        #region Métodos de consulta

        /// <summary>
        /// Seleciona os tipos deficiência e seus detalhes por aluno.
        /// </summary>
        /// <param name="alu_id"></param>
        /// <returns></returns>
        public DataTable SelecionaPorAluno(long alu_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_AlunoDeficienciaDetalhe_SelecionaPorAluno", _Banco);

            try
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;
                qs.Parameters.Add(Param);

                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion Métodos de consulta

        #region Métodos sobrecritos

        /// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
		/// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		/// <returns>TRUE - Se entity.ParametroId > 0</returns>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CLS_AlunoDeficienciaDetalhe entity)
        {
            if (entity != null & qs != null)
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}