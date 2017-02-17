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
	/// Classe abstrata de MTR_ProcessoFechamentoInicioEtapa
	/// </summary>
	public abstract class Abstract_MTR_ProcessoFechamentoInicioEtapaDAO : Abstract_DAL<MTR_ProcessoFechamentoInicioEtapa>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, MTR_ProcessoFechamentoInicioEtapa entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pfi_id";
			Param.Size = 4;
			Param.Value = entity.pfi_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pfe_id";
			Param.Size = 4;
			Param.Value = entity.pfe_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, MTR_ProcessoFechamentoInicioEtapa entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pfi_id";
			Param.Size = 4;
			Param.Value = entity.pfi_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pfe_id";
			Param.Size = 4;
			Param.Value = entity.pfe_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@pfe_tipoEtapa";
			Param.Size = 1;
			Param.Value = entity.pfe_tipoEtapa;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@pfe_alcance";
			Param.Size = 1;
			Param.Value = entity.pfe_alcance;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Object;
			Param.ParameterName = "@pfe_dataInicio";
			Param.Size = 20;
			Param.Value = entity.pfe_dataInicio;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Object;
			Param.ParameterName = "@pfe_dataFim";
			Param.Size = 20;
			Param.Value = entity.pfe_dataFim;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cal_idFechamento";
			Param.Size = 4;
			if( entity.cal_idFechamento > 0  )
				Param.Value = entity.cal_idFechamento;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cal_idInicio";
			Param.Size = 4;
			if( entity.cal_idInicio > 0  )
				Param.Value = entity.cal_idInicio;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cur_id";
			Param.Size = 4;
			if( entity.cur_id > 0  )
				Param.Value = entity.cur_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crr_id";
			Param.Size = 4;
			if( entity.crr_id > 0  )
				Param.Value = entity.crr_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crp_id";
			Param.Size = 4;
			if( entity.crp_id > 0  )
				Param.Value = entity.crp_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@esc_id";
			Param.Size = 4;
			if( entity.esc_id > 0  )
				Param.Value = entity.esc_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@uni_id";
			Param.Size = 4;
			if( entity.uni_id > 0  )
				Param.Value = entity.uni_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@pfe_situacao";
			Param.Size = 1;
			Param.Value = entity.pfe_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@pfe_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.pfe_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@pfe_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.pfe_dataAlteracao;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, MTR_ProcessoFechamentoInicioEtapa entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pfi_id";
			Param.Size = 4;
			Param.Value = entity.pfi_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pfe_id";
			Param.Size = 4;
			Param.Value = entity.pfe_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@pfe_tipoEtapa";
			Param.Size = 1;
			Param.Value = entity.pfe_tipoEtapa;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@pfe_alcance";
			Param.Size = 1;
			Param.Value = entity.pfe_alcance;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Object;
			Param.ParameterName = "@pfe_dataInicio";
			Param.Size = 20;
			Param.Value = entity.pfe_dataInicio;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Object;
			Param.ParameterName = "@pfe_dataFim";
			Param.Size = 20;
			Param.Value = entity.pfe_dataFim;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cal_idFechamento";
			Param.Size = 4;
			if( entity.cal_idFechamento > 0  )
				Param.Value = entity.cal_idFechamento;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cal_idInicio";
			Param.Size = 4;
			if( entity.cal_idInicio > 0  )
				Param.Value = entity.cal_idInicio;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cur_id";
			Param.Size = 4;
			if( entity.cur_id > 0  )
				Param.Value = entity.cur_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crr_id";
			Param.Size = 4;
			if( entity.crr_id > 0  )
				Param.Value = entity.crr_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crp_id";
			Param.Size = 4;
			if( entity.crp_id > 0  )
				Param.Value = entity.crp_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@esc_id";
			Param.Size = 4;
			if( entity.esc_id > 0  )
				Param.Value = entity.esc_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@uni_id";
			Param.Size = 4;
			if( entity.uni_id > 0  )
				Param.Value = entity.uni_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@pfe_situacao";
			Param.Size = 1;
			Param.Value = entity.pfe_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@pfe_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.pfe_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@pfe_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.pfe_dataAlteracao;
			qs.Parameters.Add(Param);


		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, MTR_ProcessoFechamentoInicioEtapa entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pfi_id";
			Param.Size = 4;
			Param.Value = entity.pfi_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pfe_id";
			Param.Size = 4;
			Param.Value = entity.pfe_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade 
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, MTR_ProcessoFechamentoInicioEtapa entity)
		{
            return true;
		}		
	}
}

