/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using Data.Common;
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
    }
}