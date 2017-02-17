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
	/// Classe abstrata de MTR_MatriculaTurma
	/// </summary>
	public abstract class Abstract_MTR_MatriculaTurmaDAO : Abstract_DAL<MTR_MatriculaTurma>
	{
	
        protected override string ConnectionStringName
        {
            get
            {
                return "MSTech.GestaoEscolar";
            }
        }
        	
		/// <summary>
		/// Configura os parametros do metodo de carregar
		/// </ssummary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, MTR_MatriculaTurma entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mtu_id";
			Param.Size = 4;
			Param.Value = entity.mtu_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, MTR_MatriculaTurma entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mtu_id";
			Param.Size = 4;
			Param.Value = entity.mtu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tur_id";
			Param.Size = 8;
			Param.Value = entity.tur_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cur_id";
			Param.Size = 4;
			Param.Value = entity.cur_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crr_id";
			Param.Size = 4;
			Param.Value = entity.crr_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crp_id";
			Param.Size = 4;
			Param.Value = entity.crp_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@alc_id";
			Param.Size = 4;
			Param.Value = entity.alc_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Object;
			Param.ParameterName = "@mtu_dataMatricula";
			Param.Size = 20;
			Param.Value = entity.mtu_dataMatricula;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@mtu_avaliacao";
			Param.Size = 20;
			if( !string.IsNullOrEmpty(entity.mtu_avaliacao) )
				Param.Value = entity.mtu_avaliacao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Decimal;
			Param.ParameterName = "@mtu_frequencia";
			Param.Size = 7;
				Param.Value = entity.mtu_frequencia;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@mtu_relatorio";
			Param.Size = 2147483647;
			if( !string.IsNullOrEmpty(entity.mtu_relatorio) )
				Param.Value = entity.mtu_relatorio;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mtu_resultado";
			Param.Size = 1;
			if( entity.mtu_resultado > 0  )
				Param.Value = entity.mtu_resultado;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Object;
			Param.ParameterName = "@mtu_dataSaida";
			Param.Size = 20;
			if( entity.mtu_dataSaida!= new DateTime() )
				Param.Value = entity.mtu_dataSaida;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mtu_numeroChamada";
			Param.Size = 4;
			if( entity.mtu_numeroChamada > 0  )
				Param.Value = entity.mtu_numeroChamada;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@mtu_situacao";
			Param.Size = 1;
			Param.Value = entity.mtu_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@mtu_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.mtu_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@mtu_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.mtu_dataAlteracao;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@usu_idResultado";
            Param.Size = 16;
            Param.Value = entity.usu_idResultado;
            qs.Parameters.Add(Param);
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, MTR_MatriculaTurma entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mtu_id";
			Param.Size = 4;
			Param.Value = entity.mtu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tur_id";
			Param.Size = 8;
			Param.Value = entity.tur_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cur_id";
			Param.Size = 4;
			Param.Value = entity.cur_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crr_id";
			Param.Size = 4;
			Param.Value = entity.crr_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crp_id";
			Param.Size = 4;
			Param.Value = entity.crp_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@alc_id";
			Param.Size = 4;
			Param.Value = entity.alc_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Object;
			Param.ParameterName = "@mtu_dataMatricula";
			Param.Size = 20;
			Param.Value = entity.mtu_dataMatricula;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@mtu_avaliacao";
			Param.Size = 20;
			if( !string.IsNullOrEmpty(entity.mtu_avaliacao) )
				Param.Value = entity.mtu_avaliacao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Decimal;
			Param.ParameterName = "@mtu_frequencia";
			Param.Size = 7;
				Param.Value = entity.mtu_frequencia;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@mtu_relatorio";
			Param.Size = 2147483647;
			if( !string.IsNullOrEmpty(entity.mtu_relatorio) )
				Param.Value = entity.mtu_relatorio;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mtu_resultado";
			Param.Size = 1;
			if( entity.mtu_resultado > 0  )
				Param.Value = entity.mtu_resultado;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Object;
			Param.ParameterName = "@mtu_dataSaida";
			Param.Size = 20;
			if( entity.mtu_dataSaida!= new DateTime() )
				Param.Value = entity.mtu_dataSaida;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mtu_numeroChamada";
			Param.Size = 4;
			if( entity.mtu_numeroChamada > 0  )
				Param.Value = entity.mtu_numeroChamada;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@mtu_situacao";
			Param.Size = 1;
			Param.Value = entity.mtu_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@mtu_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.mtu_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@mtu_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.mtu_dataAlteracao;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@usu_idResultado";
            Param.Size = 16;
            Param.Value = entity.usu_idResultado;
            qs.Parameters.Add(Param);
		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, MTR_MatriculaTurma entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mtu_id";
			Param.Size = 4;
			Param.Value = entity.mtu_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade 
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, MTR_MatriculaTurma entity)
		{
            return true;
		}		
	}
}

