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
	/// Classe abstrata de CLS_TurmaNota
	/// </summary>
	public abstract class Abstract_CLS_TurmaNotaDAO : Abstract_DAL<CLS_TurmaNota>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, CLS_TurmaNota entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tud_id";
			Param.Size = 8;
			Param.Value = entity.tud_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tnt_id";
			Param.Size = 4;
			Param.Value = entity.tnt_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_TurmaNota entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tud_id";
			Param.Size = 8;
			Param.Value = entity.tud_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tnt_id";
			Param.Size = 4;
			Param.Value = entity.tnt_id;
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
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tau_id";
			Param.Size = 4;
			if( entity.tau_id > 0  )
				Param.Value = entity.tau_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tav_id";
			Param.Size = 4;
			if( entity.tav_id > 0  )
				Param.Value = entity.tav_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@tnt_nome";
			Param.Size = 100;
			if( !string.IsNullOrEmpty(entity.tnt_nome) )
				Param.Value = entity.tnt_nome;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tnt_data";
			Param.Size = 20;
			if( entity.tnt_data!= new DateTime() )
				Param.Value = entity.tnt_data;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.String;
			Param.ParameterName = "@tnt_descricao";			
			if( !string.IsNullOrEmpty(entity.tnt_descricao) )
				Param.Value = entity.tnt_descricao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@tnt_efetivado";
			Param.Size = 1;
				Param.Value = entity.tnt_efetivado;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tnt_situacao";
			Param.Size = 1;
			Param.Value = entity.tnt_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tnt_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.tnt_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tnt_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.tnt_dataAlteracao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tdt_posicao";
            Param.Size = 1;
            Param.Value = entity.tdt_posicao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@tnt_exclusiva";
            Param.Size = 1;
            Param.Value = entity.tnt_exclusiva;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@usu_id";
            Param.Size = 16;
            Param.Value = entity.usu_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@pro_id";
            Param.Size = 16;
            Param.Value = entity.pro_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tnt_chaveDiario";
            Param.Size = 4;
            if (entity.tnt_chaveDiario > 0)
            {
                Param.Value = entity.tnt_chaveDiario;
            }
            else
            {
                Param.Value = DBNull.Value;
            }
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@usu_idDocenteAlteracao";
            Param.Size = 16;
            Param.Value = entity.usu_idDocenteAlteracao;
            qs.Parameters.Add(Param);

		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, CLS_TurmaNota entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tud_id";
			Param.Size = 8;
			Param.Value = entity.tud_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tnt_id";
			Param.Size = 4;
			Param.Value = entity.tnt_id;
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
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tau_id";
			Param.Size = 4;
			if( entity.tau_id > 0  )
				Param.Value = entity.tau_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tav_id";
			Param.Size = 4;
			if( entity.tav_id > 0  )
				Param.Value = entity.tav_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@tnt_nome";
			Param.Size = 100;
			if( !string.IsNullOrEmpty(entity.tnt_nome) )
				Param.Value = entity.tnt_nome;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tnt_data";
			Param.Size = 20;
			if( entity.tnt_data!= new DateTime() )
				Param.Value = entity.tnt_data;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.String;
			Param.ParameterName = "@tnt_descricao";			
			if( !string.IsNullOrEmpty(entity.tnt_descricao) )
				Param.Value = entity.tnt_descricao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@tnt_efetivado";
			Param.Size = 1;
				Param.Value = entity.tnt_efetivado;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tnt_situacao";
			Param.Size = 1;
			Param.Value = entity.tnt_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tnt_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.tnt_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tnt_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.tnt_dataAlteracao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tdt_posicao";
            Param.Size = 1;
            Param.Value = entity.tdt_posicao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@tnt_exclusiva";
            Param.Size = 1;
            Param.Value = entity.tnt_exclusiva;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@usu_id";
            Param.Size = 16;
            Param.Value = entity.usu_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@pro_id";
            Param.Size = 16;
            Param.Value = entity.pro_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tnt_chaveDiario";
            Param.Size = 4;
            if (entity.tnt_chaveDiario > 0)
            {
                Param.Value = entity.tnt_chaveDiario;
            }
            else
            {
                Param.Value = DBNull.Value;
            }
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@usu_idDocenteAlteracao";
            Param.Size = 16;
            Param.Value = entity.usu_idDocenteAlteracao;
            qs.Parameters.Add(Param);

		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, CLS_TurmaNota entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tud_id";
			Param.Size = 8;
			Param.Value = entity.tud_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tnt_id";
			Param.Size = 4;
			Param.Value = entity.tnt_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade 
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CLS_TurmaNota entity)
		{
            entity.tnt_id = Convert.ToInt32(qs.Return.Rows[0][0]);
            return (entity.tnt_id > 0);
		}		
	}
}

