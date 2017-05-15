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
	/// Classe abstrata de SYS_RecursoAPI.
	/// </summary>
	public abstract class Abstract_SYS_RecursoAPIDAO : Abstract_DAL<SYS_RecursoAPI>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, SYS_RecursoAPI entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@rap_id";
			Param.Size = 4;
			Param.Value = entity.rap_id;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, SYS_RecursoAPI entity)
		{
			if (entity != null & qs != null)
            {
							Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@rap_id";
			Param.Size = 4;
			Param.Value = entity.rap_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.String;
			Param.ParameterName = "@rap_descricao";
			Param.Size = 200;
			Param.Value = entity.rap_descricao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.String;
			Param.ParameterName = "@rap_url";
			Param.Size = 200;
			Param.Value = entity.rap_url;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@rap_situacao";
			Param.Size = 1;
			Param.Value = entity.rap_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@rap_dataCriacao";

			Param.Value = entity.rap_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@rap_dataAlteracao";

			Param.Value = entity.rap_dataAlteracao;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, SYS_RecursoAPI entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@rap_id";
			Param.Size = 4;
			Param.Value = entity.rap_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.String;
			Param.ParameterName = "@rap_descricao";
			Param.Size = 200;
			Param.Value = entity.rap_descricao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.String;
			Param.ParameterName = "@rap_url";
			Param.Size = 200;
			Param.Value = entity.rap_url;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@rap_situacao";
			Param.Size = 1;
			Param.Value = entity.rap_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@rap_dataCriacao";

			Param.Value = entity.rap_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@rap_dataAlteracao";

			Param.Value = entity.rap_dataAlteracao;
			qs.Parameters.Add(Param);


			}
		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, SYS_RecursoAPI entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@rap_id";
			Param.Size = 4;
			Param.Value = entity.rap_id;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
		/// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		/// <returns>TRUE - Se entity.ParametroId > 0</returns>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, SYS_RecursoAPI entity)
		{
			if (entity != null & qs != null)
            {
				throw new NotImplementedException("Método 'ReceberAutoIncremento' não implementado.");
			}

			return false;
		}		
	}
}