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
	/// Classe abstrata de REL_GraficoAtendimento_FiltrosFixos.
	/// </summary>
	public abstract class Abstract_REL_GraficoAtendimento_FiltrosFixosDAO : Abstract_DAL<REL_GraficoAtendimento_FiltrosFixos>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, REL_GraficoAtendimento_FiltrosFixos entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@gra_id";
			Param.Size = 4;
			Param.Value = entity.gra_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@gff_tipoFiltro";
			Param.Size = 1;
			Param.Value = entity.gff_tipoFiltro;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, REL_GraficoAtendimento_FiltrosFixos entity)
		{
			if (entity != null & qs != null)
            {
							Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@gra_id";
			Param.Size = 4;
			Param.Value = entity.gra_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@gff_tipoFiltro";
			Param.Size = 1;
			Param.Value = entity.gff_tipoFiltro;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.String;
			Param.ParameterName = "@gff_valorFiltro";

			Param.Value = entity.gff_valorFiltro;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, REL_GraficoAtendimento_FiltrosFixos entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@gra_id";
			Param.Size = 4;
			Param.Value = entity.gra_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@gff_tipoFiltro";
			Param.Size = 1;
			Param.Value = entity.gff_tipoFiltro;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.String;
			Param.ParameterName = "@gff_valorFiltro";

			Param.Value = entity.gff_valorFiltro;
			qs.Parameters.Add(Param);


			}
		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, REL_GraficoAtendimento_FiltrosFixos entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@gra_id";
			Param.Size = 4;
			Param.Value = entity.gra_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@gff_tipoFiltro";
			Param.Size = 1;
			Param.Value = entity.gff_tipoFiltro;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
		/// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		/// <returns>TRUE - Se entity.ParametroId > 0</returns>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, REL_GraficoAtendimento_FiltrosFixos entity)
		{
			return true;
		}		
	}
}