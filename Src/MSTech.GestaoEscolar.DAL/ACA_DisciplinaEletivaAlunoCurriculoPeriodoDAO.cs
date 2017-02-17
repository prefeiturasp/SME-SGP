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
	public class ACA_DisciplinaEletivaAlunoCurriculoPeriodoDAO : Abstract_ACA_DisciplinaEletivaAlunoCurriculoPeriodoDAO
	{

        #region Metodos

        public DataTable SelectBy_Pesquisa_DisciplinasEletivasAlunoPeriodo
            (
                int dea_id
            )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_DisciplinaEletivaAlunoCurriculoPeriodo_SelectPorDisciplina", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@dea_id";
                Param.Size = 4;
                Param.Value = dea_id;
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

        #endregion

        /// <summary>
        /// Nome da conexão.
        /// </summary>
        protected override string ConnectionStringName
        {
            get
            {
                return "GestaoEscolar";
            }
        }

        /// <summary>
        /// Método que carrega os dados da entidade.
        /// </summary>
        /// <param name="entity">Entidade a ser carregada</param>
        /// <returns></returns>
        public override bool Carregar(ACA_DisciplinaEletivaAlunoCurriculoPeriodo entity)
        {
            __STP_LOAD = "NEW_ACA_DisciplinaEletivaAlunoCurriculoPeriodo_LOAD";
            return base.Carregar(entity);
        }
	}
}