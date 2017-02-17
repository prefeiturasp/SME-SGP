/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using System;
    using System.Data;
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class CLS_AlunoAvaliacaoTurmaRecomendacaoDAO : Abstract_CLS_AlunoAvaliacaoTurmaRecomendacaoDAO
	{
        #region Consultas

        /// <summary>
        /// Seleciona as recomendações por matrícula do aluno e tipo de recomendação.
        /// </summary>
        /// <param name="tur_id">ID da turma.</param>
        /// <param name="alu_id">ID do aluno.</param>
        /// <param name="mtu_id">ID da matrícula turma do aluno.</param>
        /// <param name="fav_id">ID do formato de avaliação.</param>
        /// <param name="ava_id">ID da avaliação.</param>
        /// <param name="rar_tipo">Tipo de recomendação.</param>
        /// <returns></returns>
        public DataTable SelecionaPorMatriculaTurmaTipo(long tur_id, long alu_id, int mtu_id, int fav_id, int ava_id, byte rar_tipo)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_AlunoAvaliacaoTurmaRecomendacao_SelecionaPorMatriculaTurmaTipo", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@mtu_id";
                Param.Size = 4;
                Param.Value = mtu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@fav_id";
                Param.Size = 4;
                Param.Value = fav_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@ava_id";
                Param.Size = 4;
                Param.Value = ava_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@rar_tipo";
                Param.Size = 1;
                if (rar_tipo > 0)
                    Param.Value = rar_tipo;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion
	}
}