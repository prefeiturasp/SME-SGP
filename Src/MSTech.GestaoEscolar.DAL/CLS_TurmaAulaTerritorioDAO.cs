/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using Data.Common;
    using Entities;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using System;
    using System.Data;

    /// <summary>
    /// Description: .
    /// </summary>
    public class CLS_TurmaAulaTerritorioDAO : Abstract_CLS_TurmaAulaTerritorioDAO
	{
        /// <summary>
        /// Retorna a ligação entre os territórios e experiências nas aulas criadas no período informado.
        /// </summary>
        /// <returns></returns>
        public DataTable SelecionaAulasTerritorioPorExperiencia(long tud_idExperiencia, DateTime dataInicial, DateTime dataFinal)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAulaTerritorio_SelecionaPorExperiencia_Data", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_idExperiencia";
            Param.Size = 8;
            Param.Value = tud_idExperiencia;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@dataInicial";
            Param.Value = dataInicial;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@dataFinal";
            Param.Value = dataFinal;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
		/// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		/// <returns>TRUE - Se entity.ParametroId > 0</returns>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CLS_TurmaAulaTerritorio entity)
        {
            return true;
        }
    }
}