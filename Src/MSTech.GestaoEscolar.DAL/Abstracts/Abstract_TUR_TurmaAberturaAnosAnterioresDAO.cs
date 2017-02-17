/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL.Abstracts
{
	using System;
	using System.Data;
	using MSTech.Data.Common;
	using MSTech.Data.Common.Abstracts;
	using GestaoEscolar.Entities;
	
	/// <summary>
	/// Classe abstrata de TUR_TurmaAberturaAnosAnteriores.
	/// </summary>
	public abstract class Abstract_TUR_TurmaAberturaAnosAnterioresDAO : Abstract_DAL<TUR_TurmaAberturaAnosAnteriores>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, TUR_TurmaAberturaAnosAnteriores entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tab_id";
			Param.Size = 8;
			Param.Value = entity.tab_id;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, TUR_TurmaAberturaAnosAnteriores entity)
		{
			if (entity != null & qs != null)
            {
							Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tab_ano";
			Param.Size = 4;
			Param.Value = entity.tab_ano;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@uad_idSuperior";
            Param.Size = 16;
            if (entity.uad_idSuperior == new Guid())
                Param.Value = DBNull.Value;
            else
                Param.Value = entity.uad_idSuperior;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@esc_id";
			Param.Size = 4;
				if(entity.esc_id > 0 )
				{
					Param.Value = entity.esc_id;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@uni_id";
			Param.Size = 4;
				if(entity.uni_id > 0 )
				{
					Param.Value = entity.uni_id;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime2;
			Param.ParameterName = "@tab_dataInicio";
			Param.Size = 20;
			Param.Value = entity.tab_dataInicio;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime2;
			Param.ParameterName = "@tab_dataFim";
			Param.Size = 20;
				if(entity.tab_dataFim!= new DateTime())
				{
					Param.Value = entity.tab_dataFim;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tab_status";
			Param.Size = 1;
			Param.Value = entity.tab_status;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tab_situacao";
			Param.Size = 1;
			Param.Value = entity.tab_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tab_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.tab_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tab_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.tab_dataAlteracao;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, TUR_TurmaAberturaAnosAnteriores entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tab_id";
			Param.Size = 8;
			Param.Value = entity.tab_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tab_ano";
			Param.Size = 4;
			Param.Value = entity.tab_ano;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@uad_idSuperior";
            Param.Size = 16;
            if (entity.uad_idSuperior == new Guid())
                Param.Value = DBNull.Value;
            else
                Param.Value = entity.uad_idSuperior;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@esc_id";
			Param.Size = 4;
				if(entity.esc_id > 0 )
				{
					Param.Value = entity.esc_id;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@uni_id";
			Param.Size = 4;
				if(entity.uni_id > 0 )
				{
					Param.Value = entity.uni_id;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime2;
			Param.ParameterName = "@tab_dataInicio";
			Param.Size = 20;
			Param.Value = entity.tab_dataInicio;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime2;
			Param.ParameterName = "@tab_dataFim";
			Param.Size = 20;
				if(entity.tab_dataFim!= new DateTime())
				{
					Param.Value = entity.tab_dataFim;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tab_status";
			Param.Size = 1;
			Param.Value = entity.tab_status;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tab_situacao";
			Param.Size = 1;
			Param.Value = entity.tab_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tab_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.tab_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tab_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.tab_dataAlteracao;
			qs.Parameters.Add(Param);


			}
		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, TUR_TurmaAberturaAnosAnteriores entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tab_id";
			Param.Size = 8;
			Param.Value = entity.tab_id;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
		/// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		/// <returns>TRUE - Se entity.ParametroId > 0</returns>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, TUR_TurmaAberturaAnosAnteriores entity)
		{
			if (entity != null & qs != null)
            {
			entity.tab_id = Convert.ToInt32(qs.Return.Rows[0][0]);
			return (entity.tab_id > 0);
			}

			return false;
		}		
	}
}