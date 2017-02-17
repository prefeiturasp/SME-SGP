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
	/// Classe abstrata de MTR_Matricula.
	/// </summary>
	public abstract class AbstractMTR_MatriculaDAO : Abstract_DAL<MTR_Matricula>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, MTR_Matricula entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pfi_id";
			Param.Size = 4;
			Param.Value = entity.pfi_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mtr_id";
			Param.Size = 4;
			Param.Value = entity.mtr_id;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, MTR_Matricula entity)
		{
			if (entity != null & qs != null)
            {
							Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pfi_id";
			Param.Size = 4;
			Param.Value = entity.pfi_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mtr_id";
			Param.Size = 4;
			Param.Value = entity.mtr_id;
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
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cur_id";
			Param.Size = 4;
				if(entity.cur_id > 0 )
				{
					Param.Value = entity.cur_id;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crr_id";
			Param.Size = 4;
				if(entity.crr_id > 0 )
				{
					Param.Value = entity.crr_id;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crp_id";
			Param.Size = 4;
				if(entity.crp_id > 0 )
				{
					Param.Value = entity.crp_id;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@ttn_id";
			Param.Size = 4;
				if(entity.ttn_id > 0 )
				{
					Param.Value = entity.ttn_id;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tmo_id";
			Param.Size = 4;
				if(entity.tmo_id > 0 )
				{
					Param.Value = entity.tmo_id;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mov_id";
			Param.Size = 4;
				if(entity.mov_id > 0 )
				{
					Param.Value = entity.mov_id;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tur_id";
			Param.Size = 8;
				if(entity.tur_id > 0 )
				{
					Param.Value = entity.tur_id;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tmo_idImportado";
			Param.Size = 4;
				if(entity.tmo_idImportado > 0 )
				{
					Param.Value = entity.tmo_idImportado;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@eco_id";
			Param.Size = 8;
				if(entity.eco_id > 0 )
				{
					Param.Value = entity.eco_id;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@cid_id";
			Param.Size = 16;
				Param.Value = entity.cid_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@unf_id";
			Param.Size = 16;
				Param.Value = entity.unf_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@mda_avaliacao";
			Param.Size = 2147483647;
				if(!string.IsNullOrEmpty(entity.mda_avaliacao))
				{
					Param.Value = entity.mda_avaliacao;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@mda_observacao";
			Param.Size = 2147483647;
				if(!string.IsNullOrEmpty(entity.mda_observacao))
				{
					Param.Value = entity.mda_observacao;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@mtr_concluiNivelEnsino";
			Param.Size = 1;
			Param.Value = entity.mtr_concluiNivelEnsino;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mtr_numeroAvaliacao";
			Param.Size = 4;
				if(entity.mtr_numeroAvaliacao > 0 )
				{
					Param.Value = entity.mtr_numeroAvaliacao;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@mtr_numeroMatricula";
			Param.Size = 50;
				if(!string.IsNullOrEmpty(entity.mtr_numeroMatricula))
				{
					Param.Value = entity.mtr_numeroMatricula;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@mtr_tipoProcesso";
			Param.Size = 1;
			Param.Value = entity.mtr_tipoProcesso;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@mtr_situacao";
			Param.Size = 1;
			Param.Value = entity.mtr_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@mtr_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.mtr_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@mtr_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.mtr_dataAlteracao;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, MTR_Matricula entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pfi_id";
			Param.Size = 4;
			Param.Value = entity.pfi_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mtr_id";
			Param.Size = 4;
			Param.Value = entity.mtr_id;
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
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cur_id";
			Param.Size = 4;
				if(entity.cur_id > 0 )
				{
					Param.Value = entity.cur_id;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crr_id";
			Param.Size = 4;
				if(entity.crr_id > 0 )
				{
					Param.Value = entity.crr_id;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crp_id";
			Param.Size = 4;
				if(entity.crp_id > 0 )
				{
					Param.Value = entity.crp_id;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@ttn_id";
			Param.Size = 4;
				if(entity.ttn_id > 0 )
				{
					Param.Value = entity.ttn_id;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tmo_id";
			Param.Size = 4;
				if(entity.tmo_id > 0 )
				{
					Param.Value = entity.tmo_id;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mov_id";
			Param.Size = 4;
				if(entity.mov_id > 0 )
				{
					Param.Value = entity.mov_id;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tur_id";
			Param.Size = 8;
				if(entity.tur_id > 0 )
				{
					Param.Value = entity.tur_id;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tmo_idImportado";
			Param.Size = 4;
				if(entity.tmo_idImportado > 0 )
				{
					Param.Value = entity.tmo_idImportado;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@eco_id";
			Param.Size = 8;
				if(entity.eco_id > 0 )
				{
					Param.Value = entity.eco_id;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@cid_id";
			Param.Size = 16;
				Param.Value = entity.cid_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@unf_id";
			Param.Size = 16;
				Param.Value = entity.unf_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@mda_avaliacao";
			Param.Size = 2147483647;
				if(!string.IsNullOrEmpty(entity.mda_avaliacao))
				{
					Param.Value = entity.mda_avaliacao;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@mda_observacao";
			Param.Size = 2147483647;
				if(!string.IsNullOrEmpty(entity.mda_observacao))
				{
					Param.Value = entity.mda_observacao;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@mtr_concluiNivelEnsino";
			Param.Size = 1;
			Param.Value = entity.mtr_concluiNivelEnsino;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mtr_numeroAvaliacao";
			Param.Size = 4;
				if(entity.mtr_numeroAvaliacao > 0 )
				{
					Param.Value = entity.mtr_numeroAvaliacao;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@mtr_numeroMatricula";
			Param.Size = 50;
				if(!string.IsNullOrEmpty(entity.mtr_numeroMatricula))
				{
					Param.Value = entity.mtr_numeroMatricula;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@mtr_tipoProcesso";
			Param.Size = 1;
			Param.Value = entity.mtr_tipoProcesso;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@mtr_situacao";
			Param.Size = 1;
			Param.Value = entity.mtr_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@mtr_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.mtr_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@mtr_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.mtr_dataAlteracao;
			qs.Parameters.Add(Param);


			}
		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, MTR_Matricula entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pfi_id";
			Param.Size = 4;
			Param.Value = entity.pfi_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mtr_id";
			Param.Size = 4;
			Param.Value = entity.mtr_id;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
		/// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		/// <returns>TRUE - Se entity.ParametroId > 0</returns>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, MTR_Matricula entity)
		{
			if (entity != null & qs != null)
            {

			}

			return false;
		}		
	}
}