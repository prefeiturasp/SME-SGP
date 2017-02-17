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
	/// Classe abstrata de MTR_MatriculaTurmaDisciplina
	/// </summary>
	public abstract class Abstract_MTR_MatriculaTurmaDisciplinaDAO : Abstract_DAL<MTR_MatriculaTurmaDisciplina>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, MTR_MatriculaTurmaDisciplina entity)
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
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mtd_id";
			Param.Size = 4;
			Param.Value = entity.mtd_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, MTR_MatriculaTurmaDisciplina entity)
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
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mtd_id";
			Param.Size = 4;
			Param.Value = entity.mtd_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tud_id";
			Param.Size = 8;
			Param.Value = entity.tud_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@mtd_dataMatricula";
			Param.Size = 20;
			Param.Value = entity.mtd_dataMatricula;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mtd_numeroChamada";
			Param.Size = 4;
			if( entity.mtd_numeroChamada > 0  )
				Param.Value = entity.mtd_numeroChamada;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@mtd_avaliacao";
			Param.Size = 20;
			if( !string.IsNullOrEmpty(entity.mtd_avaliacao) )
				Param.Value = entity.mtd_avaliacao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Decimal;
			Param.ParameterName = "@mtd_frequencia";
			Param.Size = 7;
				Param.Value = entity.mtd_frequencia;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.String;
			Param.ParameterName = "@mtd_relatorio";			
			if( !string.IsNullOrEmpty(entity.mtd_relatorio) )
				Param.Value = entity.mtd_relatorio;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mtd_resultado";
			Param.Size = 1;
			if( entity.mtd_resultado > 0  )
				Param.Value = entity.mtd_resultado;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@mtd_dataSaida";
			Param.Size = 20;
			if( entity.mtd_dataSaida!= new DateTime() )
				Param.Value = entity.mtd_dataSaida;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@mtd_situacao";
			Param.Size = 1;
			Param.Value = entity.mtd_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@mtd_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.mtd_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@mtd_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.mtd_dataAlteracao;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, MTR_MatriculaTurmaDisciplina entity)
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
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mtd_id";
			Param.Size = 4;
			Param.Value = entity.mtd_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tud_id";
			Param.Size = 8;
			Param.Value = entity.tud_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@mtd_dataMatricula";
			Param.Size = 20;
			Param.Value = entity.mtd_dataMatricula;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mtd_numeroChamada";
			Param.Size = 4;
			if( entity.mtd_numeroChamada > 0  )
				Param.Value = entity.mtd_numeroChamada;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@mtd_avaliacao";
			Param.Size = 20;
			if( !string.IsNullOrEmpty(entity.mtd_avaliacao) )
				Param.Value = entity.mtd_avaliacao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Decimal;
			Param.ParameterName = "@mtd_frequencia";
			Param.Size = 7;
				Param.Value = entity.mtd_frequencia;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.String;
			Param.ParameterName = "@mtd_relatorio";			
			if( !string.IsNullOrEmpty(entity.mtd_relatorio) )
				Param.Value = entity.mtd_relatorio;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mtd_resultado";
			Param.Size = 1;
			if( entity.mtd_resultado > 0  )
				Param.Value = entity.mtd_resultado;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@mtd_dataSaida";
			Param.Size = 20;
			if( entity.mtd_dataSaida!= new DateTime() )
				Param.Value = entity.mtd_dataSaida;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@mtd_situacao";
			Param.Size = 1;
			Param.Value = entity.mtd_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@mtd_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.mtd_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@mtd_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.mtd_dataAlteracao;
			qs.Parameters.Add(Param);


		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, MTR_MatriculaTurmaDisciplina entity)
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
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mtd_id";
			Param.Size = 4;
			Param.Value = entity.mtd_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade 
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, MTR_MatriculaTurmaDisciplina entity)
		{
            entity.mtd_id = Convert.ToInt32(qs.Return.Rows[0][0]);
            return (entity.mtd_id > 0);
		}		
	}
}

