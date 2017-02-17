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
	/// Classe abstrata de RHU_TipoVinculo
	/// </summary>
	public abstract class Abstract_RHU_TipoVinculoDAO : Abstract_DAL<RHU_TipoVinculo>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, RHU_TipoVinculo entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tvi_id";
			Param.Size = 4;
			Param.Value = entity.tvi_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, RHU_TipoVinculo entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@tvi_nome";
			Param.Size = 100;
			Param.Value = entity.tvi_nome;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.String;
			Param.ParameterName = "@tvi_descricao";			
			if( !string.IsNullOrEmpty(entity.tvi_descricao) )
				Param.Value = entity.tvi_descricao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tvi_horasSemanais";
			Param.Size = 4;
			if( entity.tvi_horasSemanais > 0  )
				Param.Value = entity.tvi_horasSemanais;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tvi_minutosAlmoco";
			Param.Size = 4;
			if( entity.tvi_minutosAlmoco > 0  )
				Param.Value = entity.tvi_minutosAlmoco;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Time;
			Param.ParameterName = "@tvi_horarioMinEntrada";
			Param.Size = 32;
            if (entity.tvi_horarioMinEntrada != new TimeSpan())
                Param.Value = Convert.ToDateTime(entity.tvi_horarioMinEntrada.ToString());
            else
                Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
            Param.DbType = DbType.Time;
			Param.ParameterName = "@tvi_horarioMaxSaida";
			Param.Size = 32;
            if (entity.tvi_horarioMaxSaida != new TimeSpan())
                Param.Value = Convert.ToDateTime(entity.tvi_horarioMaxSaida.ToString());
            else
                Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@tvi_codIntegracao";
			Param.Size = 20;
			if( !string.IsNullOrEmpty(entity.tvi_codIntegracao) )
				Param.Value = entity.tvi_codIntegracao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tvi_situacao";
			Param.Size = 1;
			Param.Value = entity.tvi_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tvi_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.tvi_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tvi_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.tvi_dataAlteracao;
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
		protected override void ParamAlterar(QueryStoredProcedure qs, RHU_TipoVinculo entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tvi_id";
			Param.Size = 4;
			Param.Value = entity.tvi_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@tvi_nome";
			Param.Size = 100;
			Param.Value = entity.tvi_nome;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.String;
			Param.ParameterName = "@tvi_descricao";			
			if( !string.IsNullOrEmpty(entity.tvi_descricao) )
				Param.Value = entity.tvi_descricao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tvi_horasSemanais";
			Param.Size = 4;
			if( entity.tvi_horasSemanais > 0  )
				Param.Value = entity.tvi_horasSemanais;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tvi_minutosAlmoco";
			Param.Size = 4;
			if( entity.tvi_minutosAlmoco > 0  )
				Param.Value = entity.tvi_minutosAlmoco;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Time;
            Param.ParameterName = "@tvi_horarioMinEntrada";
            Param.Size = 32;
            if (entity.tvi_horarioMinEntrada != new TimeSpan())
                Param.Value = entity.tvi_horarioMinEntrada;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Time;
            Param.ParameterName = "@tvi_horarioMaxSaida";
            Param.Size = 32;
            if (entity.tvi_horarioMaxSaida != new TimeSpan())
                Param.Value = entity.tvi_horarioMaxSaida;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@tvi_codIntegracao";
			Param.Size = 20;
			if( !string.IsNullOrEmpty(entity.tvi_codIntegracao) )
				Param.Value = entity.tvi_codIntegracao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tvi_situacao";
			Param.Size = 1;
			Param.Value = entity.tvi_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tvi_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.tvi_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tvi_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.tvi_dataAlteracao;
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
		protected override void ParamDeletar(QueryStoredProcedure qs, RHU_TipoVinculo entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tvi_id";
			Param.Size = 4;
			Param.Value = entity.tvi_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade 
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, RHU_TipoVinculo entity)
		{
			entity.tvi_id = Convert.ToInt32(qs.Return.Rows[0][0]);
			return (entity.tvi_id > 0);
		}		
	}
}

