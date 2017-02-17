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
	/// Classe abstrata de ESC_Predio
	/// </summary>
	public abstract class Abstract_ESC_PredioDAO : Abstract_DAL<ESC_Predio>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, ESC_Predio entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@prd_id";
			Param.Size = 4;
			Param.Value = entity.prd_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, ESC_Predio entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@prd_descricao";
			Param.Size = 1000;
			if( !string.IsNullOrEmpty(entity.prd_descricao) )
				Param.Value = entity.prd_descricao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prd_adaptado_especial";
			Param.Size = 1;
				Param.Value = entity.prd_adaptado_especial;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@prd_situacao";
			Param.Size = 1;
			Param.Value = entity.prd_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@prd_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.prd_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@prd_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.prd_dataAlteracao;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, ESC_Predio entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@prd_id";
			Param.Size = 4;
			Param.Value = entity.prd_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@prd_descricao";
			Param.Size = 1000;
			if( !string.IsNullOrEmpty(entity.prd_descricao) )
				Param.Value = entity.prd_descricao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@prd_adaptado_especial";
			Param.Size = 1;
				Param.Value = entity.prd_adaptado_especial;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@prd_situacao";
			Param.Size = 1;
			Param.Value = entity.prd_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@prd_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.prd_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@prd_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.prd_dataAlteracao;
			qs.Parameters.Add(Param);


		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, ESC_Predio entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@prd_id";
			Param.Size = 4;
			Param.Value = entity.prd_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade 
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ESC_Predio entity)
		{
			entity.prd_id = Convert.ToInt32(qs.Return.Rows[0][0]);
			return (entity.prd_id > 0);
		}		
	}
}

