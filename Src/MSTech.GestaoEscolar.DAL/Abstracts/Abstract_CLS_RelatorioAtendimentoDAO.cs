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
	/// Classe abstrata de CLS_RelatorioAtendimento.
	/// </summary>
	public abstract class Abstract_CLS_RelatorioAtendimentoDAO : Abstract_DAL<CLS_RelatorioAtendimento>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, CLS_RelatorioAtendimento entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@rea_id";
			Param.Size = 4;
			Param.Value = entity.rea_id;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_RelatorioAtendimento entity)
		{
			if (entity != null & qs != null)
            {
							Param = qs.NewParameter();
			Param.DbType = DbType.String;
			Param.ParameterName = "@rea_titulo";
			Param.Size = 200;
			Param.Value = entity.rea_titulo;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@rea_tipo";
			Param.Size = 1;
			Param.Value = entity.rea_tipo;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@rea_permiteEditarRecaCor";
			Param.Size = 1;
			Param.Value = entity.rea_permiteEditarRecaCor;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@rea_permiteEditarHipoteseDiagnostica";
			Param.Size = 1;
			Param.Value = entity.rea_permiteEditarHipoteseDiagnostica;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tds_id";
			Param.Size = 4;
				if(entity.tds_id > 0 )
				{
					Param.Value = entity.tds_id;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@rea_periodicidadePreenchimento";
			Param.Size = 1;
			Param.Value = entity.rea_periodicidadePreenchimento;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@arq_idAnexo";
			Param.Size = 8;
				if(entity.arq_idAnexo > 0 )
				{
					Param.Value = entity.arq_idAnexo;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.String;
			Param.ParameterName = "@rea_tituloAnexo";
			Param.Size = 256;
				if(!string.IsNullOrEmpty(entity.rea_tituloAnexo))
				{
					Param.Value = entity.rea_tituloAnexo;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@rea_situacao";
			Param.Size = 1;
			Param.Value = entity.rea_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@rea_dataCriacao";

			Param.Value = entity.rea_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@rea_dataAlteracao";

			Param.Value = entity.rea_dataAlteracao;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, CLS_RelatorioAtendimento entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@rea_id";
			Param.Size = 4;
			Param.Value = entity.rea_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.String;
			Param.ParameterName = "@rea_titulo";
			Param.Size = 200;
			Param.Value = entity.rea_titulo;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@rea_tipo";
			Param.Size = 1;
			Param.Value = entity.rea_tipo;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@rea_permiteEditarRecaCor";
			Param.Size = 1;
			Param.Value = entity.rea_permiteEditarRecaCor;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@rea_permiteEditarHipoteseDiagnostica";
			Param.Size = 1;
			Param.Value = entity.rea_permiteEditarHipoteseDiagnostica;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tds_id";
			Param.Size = 4;
				if(entity.tds_id > 0 )
				{
					Param.Value = entity.tds_id;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@rea_periodicidadePreenchimento";
			Param.Size = 1;
			Param.Value = entity.rea_periodicidadePreenchimento;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@arq_idAnexo";
			Param.Size = 8;
				if(entity.arq_idAnexo > 0 )
				{
					Param.Value = entity.arq_idAnexo;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.String;
			Param.ParameterName = "@rea_tituloAnexo";
			Param.Size = 256;
				if(!string.IsNullOrEmpty(entity.rea_tituloAnexo))
				{
					Param.Value = entity.rea_tituloAnexo;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@rea_situacao";
			Param.Size = 1;
			Param.Value = entity.rea_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@rea_dataCriacao";

			Param.Value = entity.rea_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@rea_dataAlteracao";

			Param.Value = entity.rea_dataAlteracao;
			qs.Parameters.Add(Param);


			}
		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, CLS_RelatorioAtendimento entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@rea_id";
			Param.Size = 4;
			Param.Value = entity.rea_id;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
		/// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		/// <returns>TRUE - Se entity.ParametroId > 0</returns>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CLS_RelatorioAtendimento entity)
		{
			if (entity != null & qs != null)
            {
			entity.rea_id = Convert.ToInt32(qs.Return.Rows[0][0]);
			return (entity.rea_id > 0);
			}

			return false;
		}		
	}
}