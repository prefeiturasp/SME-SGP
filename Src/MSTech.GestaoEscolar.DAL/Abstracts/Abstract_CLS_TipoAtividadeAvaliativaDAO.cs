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
	/// Classe abstrata de CLS_TipoAtividadeAvaliativa
	/// </summary>
	public abstract class Abstract_CLS_TipoAtividadeAvaliativaDAO : Abstract_DAL<CLS_TipoAtividadeAvaliativa>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, CLS_TipoAtividadeAvaliativa entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tav_id";
			Param.Size = 4;
			Param.Value = entity.tav_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_TipoAtividadeAvaliativa entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@tav_nome";
			Param.Size = 100;
			Param.Value = entity.tav_nome;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tav_situacao";
			Param.Size = 1;
			Param.Value = entity.tav_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tav_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.tav_dataAlteracao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tav_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.tav_dataCriacao;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@qat_id";
            Param.Size = 4;
            if (entity.qat_id > 0)
                Param.Value = entity.qat_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, CLS_TipoAtividadeAvaliativa entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tav_id";
			Param.Size = 4;
			Param.Value = entity.tav_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@tav_nome";
			Param.Size = 100;
			Param.Value = entity.tav_nome;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tav_situacao";
			Param.Size = 1;
			Param.Value = entity.tav_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tav_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.tav_dataAlteracao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tav_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.tav_dataCriacao;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@qat_id";
            Param.Size = 4;
            if (entity.qat_id > 0)
                Param.Value = entity.qat_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);
		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, CLS_TipoAtividadeAvaliativa entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tav_id";
			Param.Size = 4;
			Param.Value = entity.tav_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade 
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CLS_TipoAtividadeAvaliativa entity)
		{
			entity.tav_id = Convert.ToInt32(qs.Return.Rows[0][0]);
			return (entity.tav_id > 0);
		}		
	}
}

