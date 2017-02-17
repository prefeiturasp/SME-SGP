/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using System;
    using System.Data;
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using MSTech.GestaoEscolar.Entities;
		
	public class ACA_CompromissoEstudoDAO : Abstract_ACA_CompromissoEstudoDAO
    {
        #region Métodos

        /// <summary>
        ///  Busca os compromissos de estudo ativos (autoavaliação) do aluno
        /// </summary>
        /// <param name="alu_id">ID do aluno - Obrigatório</param>        
        /// <returns>DataTable com o compromisso de estudo (autoavaliação) do aluno</returns>
        public DataTable SelectCompromissoAlunoBy_alu_id(long alu_id, int cpe_ano)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CompromissoEstudo_SelectBy_alu_id", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cpe_ano";
                Param.Size = 4;
                if (cpe_ano > 0)
                    Param.Value = cpe_ano;
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

        /// <summary>
        ///  Busca todos os compromissos de estudo (ativos/excluídos) do aluno
        /// </summary>
        /// <param name="alu_id">ID do aluno - Obrigatório</param>        
        /// <returns>DataTable com o compromisso de estudo (autoavaliação) do aluno</returns>
        public DataTable SelectSituacaoTodosCompromissoAlunoBy_alu_id(long alu_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("STP_ACA_CompromissoEstudo_SELECTBY_alu_id", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;
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

        #region Métodos Sobrescritos       

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_CompromissoEstudo entity)
        {
            base.ParamAlterar(qs, entity);
            
            qs.Parameters["@cpe_dataAlteracao"].Value = DateTime.Now;

        }

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_CompromissoEstudo entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@cpe_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@cpe_dataAlteracao"].Value = DateTime.Now;
        }

        #endregion
    }
}