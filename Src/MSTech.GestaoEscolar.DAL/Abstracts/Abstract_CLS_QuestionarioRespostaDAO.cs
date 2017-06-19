/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL.Abstracts
{
	using System;
	using System.Data;
	using MSTech.Data.Common;
	using MSTech.Data.Common.Abstracts;
	using MSTech.GestaoEscolar.Entities;
	
	/// <summary>
	/// Classe abstrata de CLS_QuestionarioResposta.
	/// </summary>
	public abstract class Abstract_CLS_QuestionarioRespostaDAO : Abstract_DAL<CLS_QuestionarioResposta>
	{
        /// <summary>
		/// ConnectionString.
		/// </summary>
        protected override string ConnectionStringName
        {
            get
            {
                return "GestaoEscolar";
            }
        }
        	
		/// <summary>
		/// Configura os parametros do metodo de carregar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, CLS_QuestionarioResposta entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@qtr_id";
			Param.Size = 4;
			Param.Value = entity.qtr_id;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_QuestionarioResposta entity)
		{
			if (entity != null & qs != null)
            {
							Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@qtc_id";
			Param.Size = 4;
			Param.Value = entity.qtc_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.String;
			Param.ParameterName = "@qtr_texto";
			Param.Size = 50;
			Param.Value = entity.qtr_texto;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@qtr_permiteAdicionarTexto";
			Param.Size = 1;
			Param.Value = entity.qtr_permiteAdicionarTexto;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@qtr_dataCriacao";

			Param.Value = entity.qtr_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@qtr_dataAlteracao";

			Param.Value = entity.qtr_dataAlteracao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@qtr_situacao";
			Param.Size = 4;
			Param.Value = entity.qtr_situacao;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, CLS_QuestionarioResposta entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@qtr_id";
			Param.Size = 4;
			Param.Value = entity.qtr_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@qtc_id";
			Param.Size = 4;
			Param.Value = entity.qtc_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.String;
			Param.ParameterName = "@qtr_texto";
			Param.Size = 50;
			Param.Value = entity.qtr_texto;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@qtr_permiteAdicionarTexto";
			Param.Size = 1;
			Param.Value = entity.qtr_permiteAdicionarTexto;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@qtr_dataCriacao";

			Param.Value = entity.qtr_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@qtr_dataAlteracao";

			Param.Value = entity.qtr_dataAlteracao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@qtr_situacao";
			Param.Size = 4;
			Param.Value = entity.qtr_situacao;
			qs.Parameters.Add(Param);


			}
		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, CLS_QuestionarioResposta entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@qtr_id";
			Param.Size = 4;
			Param.Value = entity.qtr_id;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
		/// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		/// <returns>TRUE - Se entity.ParametroId > 0</returns>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CLS_QuestionarioResposta entity)
		{
			if (entity != null & qs != null)
            {
			entity.qtr_id = Convert.ToInt32(qs.Return.Rows[0][0]);
			return (entity.qtr_id > 0);
			}

			return false;
		}		
	}
}