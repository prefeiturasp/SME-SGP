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
	/// Classe abstrata de ACA_AlunoEscolaOrigem
	/// </summary>
	public abstract class Abstract_ACA_AlunoEscolaOrigemDAO : Abstract_DAL<ACA_AlunoEscolaOrigem>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_AlunoEscolaOrigem entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@eco_id";
			Param.Size = 8;
			Param.Value = entity.eco_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_AlunoEscolaOrigem entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tre_id";
			Param.Size = 4;
			if( entity.tre_id > 0  )
				Param.Value = entity.tre_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@eco_nome";
			Param.Size = 200;
			Param.Value = entity.eco_nome;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@eco_codigoInep";
			Param.Size = 20;
			if( !string.IsNullOrEmpty(entity.eco_codigoInep) )
				Param.Value = entity.eco_codigoInep;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@end_id";
			Param.Size = 16;
				Param.Value = entity.end_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@eco_numero";
			Param.Size = 10;
			if( !string.IsNullOrEmpty(entity.eco_numero) )
				Param.Value = entity.eco_numero;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@eco_complemento";
			Param.Size = 100;
			if( !string.IsNullOrEmpty(entity.eco_complemento) )
				Param.Value = entity.eco_complemento;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@eco_situacao";
			Param.Size = 1;
			Param.Value = entity.eco_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@eco_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.eco_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@eco_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.eco_dataAlteracao;
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


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, ACA_AlunoEscolaOrigem entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@eco_id";
			Param.Size = 8;
			Param.Value = entity.eco_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tre_id";
			Param.Size = 4;
			if( entity.tre_id > 0  )
				Param.Value = entity.tre_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@eco_nome";
			Param.Size = 200;
			Param.Value = entity.eco_nome;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@eco_codigoInep";
			Param.Size = 20;
			if( !string.IsNullOrEmpty(entity.eco_codigoInep) )
				Param.Value = entity.eco_codigoInep;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@end_id";
			Param.Size = 16;
				Param.Value = entity.end_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@eco_numero";
			Param.Size = 10;
			if( !string.IsNullOrEmpty(entity.eco_numero) )
				Param.Value = entity.eco_numero;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@eco_complemento";
			Param.Size = 100;
			if( !string.IsNullOrEmpty(entity.eco_complemento) )
				Param.Value = entity.eco_complemento;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@eco_situacao";
			Param.Size = 1;
			Param.Value = entity.eco_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@eco_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.eco_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@eco_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.eco_dataAlteracao;
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


		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, ACA_AlunoEscolaOrigem entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@eco_id";
			Param.Size = 8;
			Param.Value = entity.eco_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade 
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_AlunoEscolaOrigem entity)
		{
			entity.eco_id = Convert.ToInt32(qs.Return.Rows[0][0]);
			return (entity.eco_id > 0);
		}		
	}
}

