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
	/// Classe abstrata de MTR_Configuracao
	/// </summary>
	public abstract class Abstract_MTR_ConfiguracaoDAO : Abstract_DAL<MTR_Configuracao>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, MTR_Configuracao entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cfg_id";
			Param.Size = 4;
			Param.Value = entity.cfg_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, MTR_Configuracao entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@cfg_nome";
			Param.Size = 100;
			Param.Value = entity.cfg_nome;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@cfg_dataBaseAluno";
			Param.Size = 16;
			if( entity.cfg_dataBaseAluno!= new DateTime() )
				Param.Value = entity.cfg_dataBaseAluno;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cfg_consideraTurno";
			Param.Size = 1;
				Param.Value = entity.cfg_consideraTurno;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cfg_entregaDoc";
			Param.Size = 1;
			if( entity.cfg_entregaDoc > 0  )
				Param.Value = entity.cfg_entregaDoc;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cfg_prazoEntregaDoc";
			Param.Size = 4;
			if( entity.cfg_prazoEntregaDoc > 0  )
				Param.Value = entity.cfg_prazoEntregaDoc;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cfg_responsavelVaga";
			Param.Size = 1;
			if( entity.cfg_responsavelVaga > 0  )
				Param.Value = entity.cfg_responsavelVaga;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cfg_todasEscolas";
			Param.Size = 1;
				Param.Value = entity.cfg_todasEscolas;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@cfg_situacao";
			Param.Size = 1;
			Param.Value = entity.cfg_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@cfg_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.cfg_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@cfg_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.cfg_dataAlteracao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@evt_id";
			Param.Size = 8;
			if( entity.evt_id > 0  )
				Param.Value = entity.evt_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@ent_id";
			Param.Size = 16;
			Param.Value = entity.ent_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, MTR_Configuracao entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cfg_id";
			Param.Size = 4;
			Param.Value = entity.cfg_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@cfg_nome";
			Param.Size = 100;
			Param.Value = entity.cfg_nome;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@cfg_dataBaseAluno";
			Param.Size = 16;
			if( entity.cfg_dataBaseAluno!= new DateTime() )
				Param.Value = entity.cfg_dataBaseAluno;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cfg_consideraTurno";
			Param.Size = 1;
				Param.Value = entity.cfg_consideraTurno;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cfg_entregaDoc";
			Param.Size = 1;
			if( entity.cfg_entregaDoc > 0  )
				Param.Value = entity.cfg_entregaDoc;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cfg_prazoEntregaDoc";
			Param.Size = 4;
			if( entity.cfg_prazoEntregaDoc > 0  )
				Param.Value = entity.cfg_prazoEntregaDoc;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cfg_responsavelVaga";
			Param.Size = 1;
			if( entity.cfg_responsavelVaga > 0  )
				Param.Value = entity.cfg_responsavelVaga;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cfg_todasEscolas";
			Param.Size = 1;
				Param.Value = entity.cfg_todasEscolas;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@cfg_situacao";
			Param.Size = 1;
			Param.Value = entity.cfg_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@cfg_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.cfg_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@cfg_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.cfg_dataAlteracao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@evt_id";
			Param.Size = 8;
			if( entity.evt_id > 0  )
				Param.Value = entity.evt_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@ent_id";
			Param.Size = 16;
			Param.Value = entity.ent_id;
			qs.Parameters.Add(Param);


		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, MTR_Configuracao entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cfg_id";
			Param.Size = 4;
			Param.Value = entity.cfg_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade 
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, MTR_Configuracao entity)
		{
			entity.cfg_id = Convert.ToInt32(qs.Return.Rows[0][0]);
			return (entity.cfg_id > 0);
		}		
	}
}

