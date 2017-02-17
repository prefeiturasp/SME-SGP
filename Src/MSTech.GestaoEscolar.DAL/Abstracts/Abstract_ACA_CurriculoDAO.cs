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
	/// Classe abstrata de ACA_Curriculo
	/// </summary>
	public abstract class Abstract_ACA_CurriculoDAO : Abstract_DAL<ACA_Curriculo>
	{
	
        protected override string ConnectionStringName
        {
            get
            {
                return "MSTech.GestaoEscolar";
            }
        }
        	
		/// <summary>
		/// Configura os parametros do metodo de carregar
		/// </ssummary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_Curriculo entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cur_id";
			Param.Size = 4;
			Param.Value = entity.cur_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crr_id";
			Param.Size = 4;
			Param.Value = entity.crr_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_Curriculo entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cur_id";
			Param.Size = 4;
			Param.Value = entity.cur_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crr_id";
			Param.Size = 4;
			Param.Value = entity.crr_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crr_idBasico";
			Param.Size = 4;
			if( entity.crr_idBasico > 0  )
				Param.Value = entity.crr_idBasico;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@crr_codigo";
			Param.Size = 10;
			if( !string.IsNullOrEmpty(entity.crr_codigo) )
				Param.Value = entity.crr_codigo;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@crr_nome";
			Param.Size = 200;
			if( !string.IsNullOrEmpty(entity.crr_nome) )
				Param.Value = entity.crr_nome;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@crr_regimeMatricula";
			Param.Size = 1;
			Param.Value = entity.crr_regimeMatricula;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@crr_periodosNormal";
			Param.Size = 1;
			Param.Value = entity.crr_periodosNormal;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crr_diasLetivos";
			Param.Size = 4;
			Param.Value = entity.crr_diasLetivos;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Object;
			Param.ParameterName = "@crr_vigenciaInicio";
			Param.Size = 20;
			Param.Value = entity.crr_vigenciaInicio;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Object;
			Param.ParameterName = "@crr_vigenciaFim";
			Param.Size = 20;
			if( entity.crr_vigenciaFim!= new DateTime() )
				Param.Value = entity.crr_vigenciaFim;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crr_qtdeAvaliacaoProgressao";
			Param.Size = 4;
			if( entity.crr_qtdeAvaliacaoProgressao > 0  )
				Param.Value = entity.crr_qtdeAvaliacaoProgressao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@crr_situacao";
			Param.Size = 1;
			Param.Value = entity.crr_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@crr_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.crr_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@crr_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.crr_dataAlteracao;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, ACA_Curriculo entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cur_id";
			Param.Size = 4;
			Param.Value = entity.cur_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crr_id";
			Param.Size = 4;
			Param.Value = entity.crr_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crr_idBasico";
			Param.Size = 4;
			if( entity.crr_idBasico > 0  )
				Param.Value = entity.crr_idBasico;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@crr_codigo";
			Param.Size = 10;
			if( !string.IsNullOrEmpty(entity.crr_codigo) )
				Param.Value = entity.crr_codigo;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@crr_nome";
			Param.Size = 200;
			if( !string.IsNullOrEmpty(entity.crr_nome) )
				Param.Value = entity.crr_nome;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@crr_regimeMatricula";
			Param.Size = 1;
			Param.Value = entity.crr_regimeMatricula;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@crr_periodosNormal";
			Param.Size = 1;
			Param.Value = entity.crr_periodosNormal;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crr_diasLetivos";
			Param.Size = 4;
			Param.Value = entity.crr_diasLetivos;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Object;
			Param.ParameterName = "@crr_vigenciaInicio";
			Param.Size = 20;
			Param.Value = entity.crr_vigenciaInicio;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Object;
			Param.ParameterName = "@crr_vigenciaFim";
			Param.Size = 20;
			if( entity.crr_vigenciaFim!= new DateTime() )
				Param.Value = entity.crr_vigenciaFim;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crr_qtdeAvaliacaoProgressao";
			Param.Size = 4;
			if( entity.crr_qtdeAvaliacaoProgressao > 0  )
				Param.Value = entity.crr_qtdeAvaliacaoProgressao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@crr_situacao";
			Param.Size = 1;
			Param.Value = entity.crr_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@crr_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.crr_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@crr_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.crr_dataAlteracao;
			qs.Parameters.Add(Param);


		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, ACA_Curriculo entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cur_id";
			Param.Size = 4;
			Param.Value = entity.cur_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crr_id";
			Param.Size = 4;
			Param.Value = entity.crr_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade 
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_Curriculo entity)
		{
            entity.crr_id = Convert.ToInt32(qs.Return.Rows[0][0]);
            return (entity.crr_id > 0);
		}		
	}
}

