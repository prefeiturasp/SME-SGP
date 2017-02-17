/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL.Abstracts;

namespace MSTech.GestaoEscolar.DAL
{
	
	/// <summary>
	/// 
	/// </summary>
	public class TUR_TurmaDisciplinaNaoAvaliadoDAO : Abstract_TUR_TurmaDisciplinaNaoAvaliadoDAO
    {
        #region Consultas

        /// <summary>
        /// Retorna as avaliações que serão desconsideradas para todas as disciplinas da turma.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <returns></returns>
        public DataTable SelectBy_Turma
        (
            long tur_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDisciplinaNaoAvaliado_SelectBy_Turma", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        #endregion

        #region Sobrescritos

        public override bool Carregar(TUR_TurmaDisciplinaNaoAvaliado entity)
        {
            __STP_LOAD = "NEW_TUR_TurmaDisciplinaNaoAvaliado_Load";
            return base.Carregar(entity);
        }

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade 
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, TUR_TurmaDisciplinaNaoAvaliado entity)
        {
            return true;
        }

        #endregion
    }
}