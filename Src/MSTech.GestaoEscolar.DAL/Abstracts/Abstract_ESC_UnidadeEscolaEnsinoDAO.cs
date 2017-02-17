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
	/// Classe abstrata de ESC_UnidadeEscolaEnsino
	/// </summary>
	public abstract class Abstract_ESC_UnidadeEscolaEnsinoDAO : Abstract_DAL<ESC_UnidadeEscolaEnsino>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, ESC_UnidadeEscolaEnsino entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@esc_id";
			Param.Size = 4;
			Param.Value = entity.esc_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@uni_id";
			Param.Size = 4;
			Param.Value = entity.uni_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@uee_id";
			Param.Size = 4;
			Param.Value = entity.uee_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, ESC_UnidadeEscolaEnsino entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@esc_id";
			Param.Size = 4;
			Param.Value = entity.esc_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@uni_id";
			Param.Size = 4;
			Param.Value = entity.uni_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@uee_id";
			Param.Size = 4;
			Param.Value = entity.uee_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tne_id";
			Param.Size = 4;
			Param.Value = entity.tne_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tme_id";
			Param.Size = 4;
			Param.Value = entity.tme_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
			Param.ParameterName = "@uee_vigenciaInicio";
			Param.Size = 16;
			Param.Value = entity.uee_vigenciaInicio;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
			Param.ParameterName = "@uee_vigenciaFim";
			Param.Size = 16;
			if( entity.uee_vigenciaFim!= new DateTime() )
				Param.Value = entity.uee_vigenciaFim;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@uee_situacao";
			Param.Size = 1;
			Param.Value = entity.uee_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@uee_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.uee_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@uee_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.uee_dataAlteracao;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, ESC_UnidadeEscolaEnsino entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@esc_id";
			Param.Size = 4;
			Param.Value = entity.esc_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@uni_id";
			Param.Size = 4;
			Param.Value = entity.uni_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@uee_id";
			Param.Size = 4;
			Param.Value = entity.uee_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tne_id";
			Param.Size = 4;
			Param.Value = entity.tne_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tme_id";
			Param.Size = 4;
			Param.Value = entity.tme_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
			Param.ParameterName = "@uee_vigenciaInicio";
            Param.Size = 16;
			Param.Value = entity.uee_vigenciaInicio;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
			Param.ParameterName = "@uee_vigenciaFim";
			Param.Size = 16;
			if( entity.uee_vigenciaFim!= new DateTime() )
				Param.Value = entity.uee_vigenciaFim;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@uee_situacao";
			Param.Size = 1;
			Param.Value = entity.uee_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@uee_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.uee_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@uee_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.uee_dataAlteracao;
			qs.Parameters.Add(Param);


		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, ESC_UnidadeEscolaEnsino entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@esc_id";
			Param.Size = 4;
			Param.Value = entity.esc_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@uni_id";
			Param.Size = 4;
			Param.Value = entity.uni_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@uee_id";
			Param.Size = 4;
			Param.Value = entity.uee_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade 
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ESC_UnidadeEscolaEnsino entity)
		{
            entity.uee_id = Convert.ToInt32(qs.Return.Rows[0][0]);
            return (entity.uee_id > 0);
		}		
	}
}

