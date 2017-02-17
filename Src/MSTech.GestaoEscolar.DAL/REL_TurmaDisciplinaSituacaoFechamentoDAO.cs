/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using MSTech.GestaoEscolar.Entities;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class REL_TurmaDisciplinaSituacaoFechamentoDAO : Abstract_REL_TurmaDisciplinaSituacaoFechamentoDAO
    {
        #region Consultas

        /// <summary>
        /// Seleciona as pendências de fechamento por disciplinas
        /// </summary>
        /// <param name="dtTurmaDisciplina">Tabela de turmas disciplinas</param>
        /// <param name="tev_EfetivacaoNotas">Tipo de evento de efetivação de notas.</param>
        /// <returns></returns>
        public List<REL_TurmaDisciplinaSituacaoFechamento> SelecionaPendenciasFechamentoDisciplinas(DataTable dtTurmaDisciplina, int tev_EfetivacaoNotas)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_REL_TurmaDisciplinaSituacaoFechamento_SelecionaPendenciasFechamentoDisciplinas", _Banco);

            try
            {
                #region Parâmetros

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.ParameterName = "@tabelaTurmaDisciplina";
                sqlParam.TypeName = "TipoTabela_TurmaDisciplina";
                sqlParam.Value = dtTurmaDisciplina;
                qs.Parameters.Add(sqlParam);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tev_EfetivacaoNotas";
                Param.Size = 4;
                Param.Value = tev_EfetivacaoNotas;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

                qs.Execute();

                return qs.Return.Rows.Cast<DataRow>().Select(p => DataRowToEntity(p, new REL_TurmaDisciplinaSituacaoFechamento())).ToList();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion Consultas
    }
}