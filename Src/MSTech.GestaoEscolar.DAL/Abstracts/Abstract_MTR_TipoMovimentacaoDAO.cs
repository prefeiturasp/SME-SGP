/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MSTech.Data.Common;
using MSTech.Data.Common.Abstracts;
using MSTech.GestaoEscolar.Entities;

namespace MSTech.GestaoEscolar.DAL.Abstracts
{
	
	/// <summary>
	/// Classe abstrata de MTR_TipoMovimentacao
	/// </summary>
	public abstract class Abstract_MTR_TipoMovimentacaoDAO : Abstract_DAL<MTR_TipoMovimentacao>
	{
	
        protected override string ConnectionStringName
        {
            get
            {
                return "GestaoEscolar";
            }
        }
        	
		/// <summary>
		/// Configura os parametros do metodo de carregar
		/// </ssummary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, MTR_TipoMovimentacao entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tmo_id";
			Param.Size = 4;
			Param.Value = entity.tmo_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, MTR_TipoMovimentacao entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@ent_id";
			Param.Size = 16;
			Param.Value = entity.ent_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@tmo_nome";
			Param.Size = 100;
			Param.Value = entity.tmo_nome;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tmo_tipoMovimento";
			Param.Size = 1;
			Param.Value = entity.tmo_tipoMovimento;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@tmo_todosMomentos";
			Param.Size = 1;
			Param.Value = entity.tmo_todosMomentos;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tmo_situacao";
			Param.Size = 1;
			Param.Value = entity.tmo_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tmo_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.tmo_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tmo_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.tmo_dataAlteracao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tmv_idEntrada";
			Param.Size = 4;
			if( entity.tmv_idEntrada > 0  )
				Param.Value = entity.tmv_idEntrada;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tmv_idSaida";
			Param.Size = 4;
			if( entity.tmv_idSaida > 0  )
				Param.Value = entity.tmv_idSaida;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, MTR_TipoMovimentacao entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tmo_id";
			Param.Size = 4;
			Param.Value = entity.tmo_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@ent_id";
			Param.Size = 16;
			Param.Value = entity.ent_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@tmo_nome";
			Param.Size = 100;
			Param.Value = entity.tmo_nome;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tmo_tipoMovimento";
			Param.Size = 1;
			Param.Value = entity.tmo_tipoMovimento;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@tmo_todosMomentos";
			Param.Size = 1;
			Param.Value = entity.tmo_todosMomentos;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tmo_situacao";
			Param.Size = 1;
			Param.Value = entity.tmo_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tmo_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.tmo_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tmo_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.tmo_dataAlteracao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tmv_idEntrada";
			Param.Size = 4;
			if( entity.tmv_idEntrada > 0  )
				Param.Value = entity.tmv_idEntrada;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tmv_idSaida";
			Param.Size = 4;
			if( entity.tmv_idSaida > 0  )
				Param.Value = entity.tmv_idSaida;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);


		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, MTR_TipoMovimentacao entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tmo_id";
			Param.Size = 4;
			Param.Value = entity.tmo_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade 
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, MTR_TipoMovimentacao entity)
		{
			entity.tmo_id = Convert.ToInt32(qs.Return.Rows[0][0]);
			return (entity.tmo_id > 0);
		}		
	}
}

