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
	/// Classe abstrata de ACA_Evento
	/// </summary>
	public abstract class Abstract_ACA_EventoDAO : Abstract_DAL<ACA_Evento>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_Evento entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@evt_id";
			Param.Size = 8;
			Param.Value = entity.evt_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_Evento entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tev_id";
			Param.Size = 4;
			Param.Value = entity.tev_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@ent_id";
			Param.Size = 16;
			Param.Value = entity.ent_id;
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
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@evt_padrao";
			Param.Size = 1;
				Param.Value = entity.evt_padrao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tpc_id";
			Param.Size = 4;
			if( entity.tpc_id > 0  )
				Param.Value = entity.tpc_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@evt_nome";
			Param.Size = 200;
			Param.Value = entity.evt_nome;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.String;
			Param.ParameterName = "@evt_descricao";		
			if( !string.IsNullOrEmpty(entity.evt_descricao) )
				Param.Value = entity.evt_descricao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@evt_dataInicio";
			Param.Size = 16;
			Param.Value = entity.evt_dataInicio;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@evt_dataFim";
			Param.Size = 16;
			Param.Value = entity.evt_dataFim;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@evt_semAtividadeDiscente";
			Param.Size = 1;
			Param.Value = entity.evt_semAtividadeDiscente;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@evt_situacao";
			Param.Size = 1;
			Param.Value = entity.evt_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@evt_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.evt_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@evt_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.evt_dataAlteracao;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@evt_limitarDocente";
            Param.Size = 1;
            Param.Value = entity.evt_limitarDocente;
            qs.Parameters.Add(Param);

		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, ACA_Evento entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@evt_id";
			Param.Size = 8;
			Param.Value = entity.evt_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tev_id";
			Param.Size = 4;
			Param.Value = entity.tev_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@ent_id";
			Param.Size = 16;
			Param.Value = entity.ent_id;
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
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@evt_padrao";
			Param.Size = 1;
				Param.Value = entity.evt_padrao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tpc_id";
			Param.Size = 4;
			if( entity.tpc_id > 0  )
				Param.Value = entity.tpc_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@evt_nome";
			Param.Size = 200;
			Param.Value = entity.evt_nome;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.String;
			Param.ParameterName = "@evt_descricao";			
			if( !string.IsNullOrEmpty(entity.evt_descricao) )
				Param.Value = entity.evt_descricao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@evt_dataInicio";
			Param.Size = 16;
			Param.Value = entity.evt_dataInicio;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
			Param.ParameterName = "@evt_dataFim";
			Param.Size = 16;
			Param.Value = entity.evt_dataFim;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@evt_semAtividadeDiscente";
			Param.Size = 1;
			Param.Value = entity.evt_semAtividadeDiscente;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@evt_situacao";
			Param.Size = 1;
			Param.Value = entity.evt_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@evt_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.evt_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@evt_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.evt_dataAlteracao;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@evt_limitarDocente";
            Param.Size = 1;
            Param.Value = entity.evt_limitarDocente;
            qs.Parameters.Add(Param);
		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, ACA_Evento entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@evt_id";
			Param.Size = 8;
			Param.Value = entity.evt_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade 
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_Evento entity)
		{
			entity.evt_id = Convert.ToInt32(qs.Return.Rows[0][0]);
			return (entity.evt_id > 0);
		}		
	}
}

