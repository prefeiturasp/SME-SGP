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
	/// Classe abstrata de ESC_EscolaClassificacaoVigencia.
	/// </summary>
	public abstract class AbstractESC_EscolaClassificacaoVigenciaDAO : Abstract_DAL<ESC_EscolaClassificacaoVigencia>
	{
        /// <summary>
		/// ConnectionString.
		/// </summary>
        protected override string ConnectionStringName
        {
            get
            {
                return "MSTech.GestaoEscolar";
            }
        }
        	
		/// <summary>
		/// Configura os parametros do metodo de carregar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, ESC_EscolaClassificacaoVigencia entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@ecv_id";
			Param.Size = 8;
			Param.Value = entity.ecv_id;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, ESC_EscolaClassificacaoVigencia entity)
		{
			if (entity != null & qs != null)
            {
							Param = qs.NewParameter();
			Param.DbType = DbType.DateTime2;
			Param.ParameterName = "@ecv_dataInicio";
			Param.Size = 20;
			Param.Value = entity.ecv_dataInicio;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime2;
			Param.ParameterName = "@ecv_dataFim";
			Param.Size = 20;
				if(entity.ecv_dataFim!= new DateTime())
				{
					Param.Value = entity.ecv_dataFim;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@ecv_situacao";
			Param.Size = 1;
			Param.Value = entity.ecv_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@ecv_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.ecv_dataAlteracao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@ecv_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.ecv_dataCriacao;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, ESC_EscolaClassificacaoVigencia entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@ecv_id";
			Param.Size = 8;
			Param.Value = entity.ecv_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime2;
			Param.ParameterName = "@ecv_dataInicio";
			Param.Size = 20;
			Param.Value = entity.ecv_dataInicio;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime2;
			Param.ParameterName = "@ecv_dataFim";
			Param.Size = 20;
				if(entity.ecv_dataFim!= new DateTime())
				{
					Param.Value = entity.ecv_dataFim;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@ecv_situacao";
			Param.Size = 1;
			Param.Value = entity.ecv_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@ecv_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.ecv_dataAlteracao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@ecv_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.ecv_dataCriacao;
			qs.Parameters.Add(Param);


			}
		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, ESC_EscolaClassificacaoVigencia entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@ecv_id";
			Param.Size = 8;
			Param.Value = entity.ecv_id;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
		/// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		/// <returns>TRUE - Se entity.ParametroId > 0</returns>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ESC_EscolaClassificacaoVigencia entity)
		{
			if (entity != null & qs != null)
            {
			entity.ecv_id = Convert.ToInt64(qs.Return.Rows[0][0]);
			return (entity.ecv_id > 0);
			}

			return false;
		}		
	}
}