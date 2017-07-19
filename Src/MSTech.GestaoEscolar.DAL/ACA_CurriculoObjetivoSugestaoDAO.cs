/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using System;
    using System.Data;
    /// <summary>
    /// Description: .
    /// </summary>
    public class ACA_CurriculoObjetivoSugestaoDAO : Abstract_ACA_CurriculoObjetivoSugestaoDAO
	{
        #region Consulta

        /// <summary>
        /// Retorna as sugestões cadastradas de acordo com o tipo de nível de ensino, disciplina e usuário.
        /// </summary>
        /// <param name="tne_id">Id do tipo de nível de ensino</param>
        /// <param name="tme_id">Id do tipo de modalidade de ensino</param>
        /// <param name="tds_id">Id do tipo de disciplina</param>
        /// <param name="tcp_id">Id do tipo de currículo período</param>
        /// <param name="usu_id">Id do usuário</param>
        /// <returns></returns>
        public DataTable SelecionaPorNivelEnsinoDisciplinaPeriodoUsuario(int tne_id, int tme_id, int tds_id, int tcp_id, Guid usu_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CurriculoObjetivoSugestao_SelecionaPorNivelEnsinoDisciplinaPeriodoUsuario", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tne_id";
                Param.Value = tne_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tme_id";
                Param.Value = tme_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tds_id";
                Param.Value = tds_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tcp_id";
                Param.Value = tcp_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@usu_id";
                if (usu_id != Guid.Empty)
                {
                    Param.Value = usu_id;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();
                return qs.Return;
            }
            catch
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion Consulta
    }
}