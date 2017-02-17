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
	/// Classe abstrata de RHU_ColaboradorCargo.
	/// </summary>
	public abstract class Abstract_RHU_ColaboradorCargoDAO : Abstract_DAL<RHU_ColaboradorCargo>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, RHU_ColaboradorCargo entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@col_id";
			Param.Size = 8;
			Param.Value = entity.col_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crg_id";
			Param.Size = 4;
			Param.Value = entity.crg_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@coc_id";
			Param.Size = 4;
			Param.Value = entity.coc_id;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, RHU_ColaboradorCargo entity)
		{
			if (entity != null & qs != null)
            {
							Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@col_id";
			Param.Size = 8;
			Param.Value = entity.col_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crg_id";
			Param.Size = 4;
			Param.Value = entity.crg_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@coc_id";
			Param.Size = 4;
			Param.Value = entity.coc_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@coc_matricula";
			Param.Size = 30;
				if(!string.IsNullOrEmpty(entity.coc_matricula))
				{
					Param.Value = entity.coc_matricula;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@coc_observacao";
			Param.Size = 1000;
				if(!string.IsNullOrEmpty(entity.coc_observacao))
				{
					Param.Value = entity.coc_observacao;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime2;
			Param.ParameterName = "@coc_vigenciaInicio";
			Param.Size = 20;
			Param.Value = entity.coc_vigenciaInicio;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime2;
			Param.ParameterName = "@coc_vigenciaFim";
			Param.Size = 20;
				if(entity.coc_vigenciaFim!= new DateTime())
				{
					Param.Value = entity.coc_vigenciaFim;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@ent_id";
			Param.Size = 16;
				Param.Value = entity.ent_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@uad_id";
			Param.Size = 16;
				Param.Value = entity.uad_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@chr_id";
			Param.Size = 4;
				if(entity.chr_id > 0 )
				{
					Param.Value = entity.chr_id;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@coc_vinculoSede";
			Param.Size = 1;
				Param.Value = entity.coc_vinculoSede;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@coc_vinculoExtra";
			Param.Size = 1;
				Param.Value = entity.coc_vinculoExtra;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@coc_situacao";
			Param.Size = 1;
			Param.Value = entity.coc_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@coc_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.coc_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@coc_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.coc_dataAlteracao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@coc_complementacaoCargaHoraria";
			Param.Size = 1;
				Param.Value = entity.coc_complementacaoCargaHoraria;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime2;
			Param.ParameterName = "@coc_dataInicioMatricula";
			Param.Size = 20;
				if(entity.coc_dataInicioMatricula!= new DateTime())
				{
					Param.Value = entity.coc_dataInicioMatricula;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@coc_readaptado";
			Param.Size = 1;
				Param.Value = entity.coc_readaptado;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@coc_controladoIntegracao";
			Param.Size = 1;
			Param.Value = entity.coc_controladoIntegracao;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, RHU_ColaboradorCargo entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@col_id";
			Param.Size = 8;
			Param.Value = entity.col_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crg_id";
			Param.Size = 4;
			Param.Value = entity.crg_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@coc_id";
			Param.Size = 4;
			Param.Value = entity.coc_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@coc_matricula";
			Param.Size = 30;
				if(!string.IsNullOrEmpty(entity.coc_matricula))
				{
					Param.Value = entity.coc_matricula;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@coc_observacao";
			Param.Size = 1000;
				if(!string.IsNullOrEmpty(entity.coc_observacao))
				{
					Param.Value = entity.coc_observacao;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime2;
			Param.ParameterName = "@coc_vigenciaInicio";
			Param.Size = 20;
			Param.Value = entity.coc_vigenciaInicio;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime2;
			Param.ParameterName = "@coc_vigenciaFim";
			Param.Size = 20;
				if(entity.coc_vigenciaFim!= new DateTime())
				{
					Param.Value = entity.coc_vigenciaFim;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@ent_id";
			Param.Size = 16;
				Param.Value = entity.ent_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@uad_id";
			Param.Size = 16;
				Param.Value = entity.uad_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@chr_id";
			Param.Size = 4;
				if(entity.chr_id > 0 )
				{
					Param.Value = entity.chr_id;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@coc_vinculoSede";
			Param.Size = 1;
				Param.Value = entity.coc_vinculoSede;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@coc_vinculoExtra";
			Param.Size = 1;
				Param.Value = entity.coc_vinculoExtra;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@coc_situacao";
			Param.Size = 1;
			Param.Value = entity.coc_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@coc_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.coc_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@coc_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.coc_dataAlteracao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@coc_complementacaoCargaHoraria";
			Param.Size = 1;
				Param.Value = entity.coc_complementacaoCargaHoraria;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime2;
			Param.ParameterName = "@coc_dataInicioMatricula";
			Param.Size = 20;
				if(entity.coc_dataInicioMatricula!= new DateTime())
				{
					Param.Value = entity.coc_dataInicioMatricula;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@coc_readaptado";
			Param.Size = 1;
				Param.Value = entity.coc_readaptado;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@coc_controladoIntegracao";
			Param.Size = 1;
			Param.Value = entity.coc_controladoIntegracao;
			qs.Parameters.Add(Param);


			}
		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, RHU_ColaboradorCargo entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@col_id";
			Param.Size = 8;
			Param.Value = entity.col_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crg_id";
			Param.Size = 4;
			Param.Value = entity.crg_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@coc_id";
			Param.Size = 4;
			Param.Value = entity.coc_id;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
		/// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		/// <returns>TRUE - Se entity.ParametroId > 0</returns>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, RHU_ColaboradorCargo entity)
		{
			if (entity != null & qs != null)
            {

			}

			return false;
		}		
	}
}