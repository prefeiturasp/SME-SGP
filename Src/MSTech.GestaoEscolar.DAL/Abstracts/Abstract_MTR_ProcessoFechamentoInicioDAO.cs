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
	/// Classe abstrata de MTR_ProcessoFechamentoInicio
	/// </summary>
	public abstract class Abstract_MTR_ProcessoFechamentoInicioDAO : Abstract_DAL<MTR_ProcessoFechamentoInicio>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, MTR_ProcessoFechamentoInicio entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pfi_id";
			Param.Size = 4;
			Param.Value = entity.pfi_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, MTR_ProcessoFechamentoInicio entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@ent_id";
			Param.Size = 16;
				Param.Value = entity.ent_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pfi_anoFechamento";
			Param.Size = 4;
			Param.Value = entity.pfi_anoFechamento;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pfi_anoInicio";
			Param.Size = 4;
			Param.Value = entity.pfi_anoInicio;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Object;
			Param.ParameterName = "@pfi_dataReferencia";
			Param.Size = 20;
			if( entity.pfi_dataReferencia!= new DateTime() )
				Param.Value = entity.pfi_dataReferencia;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@pfi_remanejamentoNaoAtendido";
			Param.Size = 1;
			Param.Value = entity.pfi_remanejamentoNaoAtendido;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@pfi_anoLetivoCorrente";
			Param.Size = 1;
			Param.Value = entity.pfi_anoLetivoCorrente;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@pfi_situacao";
			Param.Size = 1;
			Param.Value = entity.pfi_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@pfi_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.pfi_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@pfi_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.pfi_dataAlteracao;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, MTR_ProcessoFechamentoInicio entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pfi_id";
			Param.Size = 4;
			Param.Value = entity.pfi_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@ent_id";
			Param.Size = 16;
				Param.Value = entity.ent_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pfi_anoFechamento";
			Param.Size = 4;
			Param.Value = entity.pfi_anoFechamento;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pfi_anoInicio";
			Param.Size = 4;
			Param.Value = entity.pfi_anoInicio;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Object;
			Param.ParameterName = "@pfi_dataReferencia";
			Param.Size = 20;
			if( entity.pfi_dataReferencia!= new DateTime() )
				Param.Value = entity.pfi_dataReferencia;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@pfi_remanejamentoNaoAtendido";
			Param.Size = 1;
			Param.Value = entity.pfi_remanejamentoNaoAtendido;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@pfi_anoLetivoCorrente";
			Param.Size = 1;
			Param.Value = entity.pfi_anoLetivoCorrente;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@pfi_situacao";
			Param.Size = 1;
			Param.Value = entity.pfi_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@pfi_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.pfi_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@pfi_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.pfi_dataAlteracao;
			qs.Parameters.Add(Param);


		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, MTR_ProcessoFechamentoInicio entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pfi_id";
			Param.Size = 4;
			Param.Value = entity.pfi_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade 
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, MTR_ProcessoFechamentoInicio entity)
		{
			entity.pfi_id = Convert.ToInt32(qs.Return.Rows[0][0]);
			return (entity.pfi_id > 0);
		}		
	}
}

