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
	/// Classe abstrata de MTR_Movimentacao
	/// </summary>
	public abstract class Abstract_MTR_MovimentacaoDAO : Abstract_DAL<MTR_Movimentacao>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, MTR_Movimentacao entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mov_id";
			Param.Size = 4;
			Param.Value = entity.mov_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, MTR_Movimentacao entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mov_id";
			Param.Size = 4;
			Param.Value = entity.mov_id;
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

			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@usu_id";
			Param.Size = 16;
				Param.Value = entity.usu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@alc_idAnterior";
			Param.Size = 4;
			if( entity.alc_idAnterior > 0  )
				Param.Value = entity.alc_idAnterior;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@alc_idAtual";
			Param.Size = 4;
			if( entity.alc_idAtual > 0  )
				Param.Value = entity.alc_idAtual;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mtu_idAnterior";
			Param.Size = 4;
			if( entity.mtu_idAnterior > 0  )
				Param.Value = entity.mtu_idAnterior;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mtu_idAtual";
			Param.Size = 4;
			if( entity.mtu_idAtual > 0  )
				Param.Value = entity.mtu_idAtual;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Object;
			Param.ParameterName = "@mov_dataRealizacao";
			Param.Size = 20;
			Param.Value = entity.mov_dataRealizacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tmo_id";
			Param.Size = 4;
			if( entity.tmo_id > 0  )
				Param.Value = entity.tmo_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tmo_idImportado";
            Param.Size = 4;
            if (entity.tmo_idImportado > 0)
            {
                Param.Value = entity.tmo_idImportado;
            }
            else
            {
                Param.Value = DBNull.Value;
            }
            qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mov_ordem";
			Param.Size = 4;
			if( entity.mov_ordem > 0  )
				Param.Value = entity.mov_ordem;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@mov_situacao";
			Param.Size = 1;
			Param.Value = entity.mov_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@mov_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.mov_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@mov_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.mov_dataAlteracao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Decimal;
			Param.ParameterName = "@mov_frequencia";
			Param.Size = 7;
				Param.Value = entity.mov_frequencia;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, MTR_Movimentacao entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mov_id";
			Param.Size = 4;
			Param.Value = entity.mov_id;
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

			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@usu_id";
			Param.Size = 16;
				Param.Value = entity.usu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@alc_idAnterior";
			Param.Size = 4;
			if( entity.alc_idAnterior > 0  )
				Param.Value = entity.alc_idAnterior;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@alc_idAtual";
			Param.Size = 4;
			if( entity.alc_idAtual > 0  )
				Param.Value = entity.alc_idAtual;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mtu_idAnterior";
			Param.Size = 4;
			if( entity.mtu_idAnterior > 0  )
				Param.Value = entity.mtu_idAnterior;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mtu_idAtual";
			Param.Size = 4;
			if( entity.mtu_idAtual > 0  )
				Param.Value = entity.mtu_idAtual;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Object;
			Param.ParameterName = "@mov_dataRealizacao";
			Param.Size = 20;
			Param.Value = entity.mov_dataRealizacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tmo_id";
			Param.Size = 4;
			if( entity.tmo_id > 0  )
				Param.Value = entity.tmo_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mov_ordem";
			Param.Size = 4;
			if( entity.mov_ordem > 0  )
				Param.Value = entity.mov_ordem;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tmo_idImportado";
            Param.Size = 4;
            if (entity.tmo_idImportado > 0)
            {
                Param.Value = entity.tmo_idImportado;
            }
            else
            {
                Param.Value = DBNull.Value;
            }
            qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@mov_situacao";
			Param.Size = 1;
			Param.Value = entity.mov_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@mov_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.mov_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@mov_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.mov_dataAlteracao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Decimal;
			Param.ParameterName = "@mov_frequencia";
			Param.Size = 7;
				Param.Value = entity.mov_frequencia;
			qs.Parameters.Add(Param);


		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, MTR_Movimentacao entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mov_id";
			Param.Size = 4;
			Param.Value = entity.mov_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade 
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, MTR_Movimentacao entity)
		{
            return true;
		}		
	}
}

