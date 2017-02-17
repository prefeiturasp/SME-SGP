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
	/// Classe abstrata de TUR_TurmaDisciplinaRelacionada.
	/// </summary>
	public abstract class AbstractTUR_TurmaDisciplinaRelacionadaDAO : Abstract_DAL<TUR_TurmaDisciplinaRelacionada>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, TUR_TurmaDisciplinaRelacionada entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tdr_id";
			Param.Size = 8;
			Param.Value = entity.tdr_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tud_id";
			Param.Size = 8;
			Param.Value = entity.tud_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tud_idRelacionada";
			Param.Size = 8;
			Param.Value = entity.tud_idRelacionada;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, TUR_TurmaDisciplinaRelacionada entity)
		{
			if (entity != null & qs != null)
            {
							Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tdr_id";
			Param.Size = 8;
			Param.Value = entity.tdr_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tud_id";
			Param.Size = 8;
			Param.Value = entity.tud_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tud_idRelacionada";
			Param.Size = 8;
			Param.Value = entity.tud_idRelacionada;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime2;
			Param.ParameterName = "@tdr_vigenciaInicio";
			Param.Size = 20;
			Param.Value = entity.tdr_vigenciaInicio;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime2;
			Param.ParameterName = "@tdr_vigenciaFim";
			Param.Size = 20;
				if(entity.tdr_vigenciaFim!= new DateTime())
				{
					Param.Value = entity.tdr_vigenciaFim;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tdr_situacao";
			Param.Size = 1;
			Param.Value = entity.tdr_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tdr_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.tdr_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tdr_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.tdr_dataAlteracao;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, TUR_TurmaDisciplinaRelacionada entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tdr_id";
			Param.Size = 8;
			Param.Value = entity.tdr_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tud_id";
			Param.Size = 8;
			Param.Value = entity.tud_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tud_idRelacionada";
			Param.Size = 8;
			Param.Value = entity.tud_idRelacionada;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime2;
			Param.ParameterName = "@tdr_vigenciaInicio";
			Param.Size = 20;
			Param.Value = entity.tdr_vigenciaInicio;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime2;
			Param.ParameterName = "@tdr_vigenciaFim";
			Param.Size = 20;
				if(entity.tdr_vigenciaFim!= new DateTime())
				{
					Param.Value = entity.tdr_vigenciaFim;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tdr_situacao";
			Param.Size = 1;
			Param.Value = entity.tdr_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tdr_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.tdr_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tdr_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.tdr_dataAlteracao;
			qs.Parameters.Add(Param);


			}
		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, TUR_TurmaDisciplinaRelacionada entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tdr_id";
			Param.Size = 8;
			Param.Value = entity.tdr_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tud_id";
			Param.Size = 8;
			Param.Value = entity.tud_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tud_idRelacionada";
			Param.Size = 8;
			Param.Value = entity.tud_idRelacionada;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
		/// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		/// <returns>TRUE - Se entity.ParametroId > 0</returns>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, TUR_TurmaDisciplinaRelacionada entity)
		{
			if (entity != null & qs != null)
            {

			}

			return false;
		}		
	}
}