/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using MSTech.Data.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL.Abstracts;
using System.Data;

namespace MSTech.GestaoEscolar.DAL
{

    /// <summary>
    /// Description: .
    /// </summary>
    public class ACA_CurriculoControleSemestralDisciplinaPeriodoDAO : AbstractACA_CurriculoControleSemestralDisciplinaPeriodoDAO
    {
        #region Métodos de consulta

        /// <summary>
        /// Retorna a matriz curricular das disciplinas da turma.
        /// </summary>
        /// <param name="tur_id">ID da Turma</param>
        /// <returns></returns>
        public DataTable SelecionaMatrizCurricularTurma(long tur_id)
        {

            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CurriculoControleSemestralDisciplinaPeriodo_SelecionaMatrizCurricularTurma", _Banco);
            
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 4;
                Param.Value = tur_id;
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


        /// <summary>
        /// Retorna o último período com lançamento de nota da matriz curricular
        /// </summary>
        /// <param name="tur_id">ID da Turma</param>
        /// <returns></returns>
        public DataTable SelecionaUltimoPeriodoNotaTurma(long tur_id)
        {

            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CurriculoControleSemestralDisciplinaPeriodo_SelecionaUltimoPeriodoNotaTurma", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 4;
                Param.Value = tur_id;
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