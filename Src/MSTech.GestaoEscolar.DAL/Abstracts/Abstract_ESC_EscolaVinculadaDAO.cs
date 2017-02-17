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
	/// Classe abstrata de ESC_EscolaVinculada
	/// </summary>
	public abstract class Abstract_ESC_EscolaVinculadaDAO : Abstract_DAL<ESC_EscolaVinculada>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, ESC_EscolaVinculada entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@esc_id";
			Param.Size = 4;
			Param.Value = entity.esc_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@esv_id";
			Param.Size = 4;
			Param.Value = entity.esv_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, ESC_EscolaVinculada entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@esc_id";
			Param.Size = 4;
			Param.Value = entity.esc_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@esv_id";
			Param.Size = 4;
			Param.Value = entity.esv_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@esc_idVinculada";
			Param.Size = 4;
			Param.Value = entity.esc_idVinculada;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@esv_observacao";
			Param.Size = 1000;
			if( !string.IsNullOrEmpty(entity.esv_observacao) )
				Param.Value = entity.esv_observacao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
			Param.ParameterName = "@esv_vigenciaInicio";
			Param.Size = 16;
			Param.Value = entity.esv_vigenciaInicio;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
			Param.ParameterName = "@esv_vigenciaFim";
			Param.Size = 16;
			if( entity.esv_vigenciaFim!= new DateTime() )
				Param.Value = entity.esv_vigenciaFim;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@esv_situacao";
			Param.Size = 1;
			Param.Value = entity.esv_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@esv_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.esv_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@esv_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.esv_dataAlteracao;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, ESC_EscolaVinculada entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@esc_id";
			Param.Size = 4;
			Param.Value = entity.esc_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@esv_id";
			Param.Size = 4;
			Param.Value = entity.esv_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@esc_idVinculada";
			Param.Size = 4;
			Param.Value = entity.esc_idVinculada;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@esv_observacao";
			Param.Size = 1000;
			if( !string.IsNullOrEmpty(entity.esv_observacao) )
				Param.Value = entity.esv_observacao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
			Param.ParameterName = "@esv_vigenciaInicio";
			Param.Size = 16;
			Param.Value = entity.esv_vigenciaInicio;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
			Param.ParameterName = "@esv_vigenciaFim";
			Param.Size = 16;
			if( entity.esv_vigenciaFim!= new DateTime() )
				Param.Value = entity.esv_vigenciaFim;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@esv_situacao";
			Param.Size = 1;
			Param.Value = entity.esv_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@esv_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.esv_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@esv_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.esv_dataAlteracao;
			qs.Parameters.Add(Param);


		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, ESC_EscolaVinculada entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@esc_id";
			Param.Size = 4;
			Param.Value = entity.esc_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@esv_id";
			Param.Size = 4;
			Param.Value = entity.esv_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade 
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ESC_EscolaVinculada entity)
		{
            entity.esv_id = Convert.ToInt32(qs.Return.Rows[0][0]);
            return (entity.esv_id > 0);
		}		
	}
}

