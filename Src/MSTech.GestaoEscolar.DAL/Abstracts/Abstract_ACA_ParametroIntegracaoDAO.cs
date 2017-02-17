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
	/// Classe abstrata de ACA_ParametroIntegracao
	/// </summary>
	public abstract class Abstract_ACA_ParametroIntegracaoDAO : Abstract_DAL<ACA_ParametroIntegracao>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_ParametroIntegracao entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pri_id";
			Param.Size = 4;
			Param.Value = entity.pri_id;
			qs.Parameters.Add(Param);
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_ParametroIntegracao entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@pri_chave";
			Param.Size = 100;
			Param.Value = entity.pri_chave;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@pri_valor";
			Param.Size = 1000;
			Param.Value = entity.pri_valor;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@pri_descricao";
			Param.Size = 200;
			if( !string.IsNullOrEmpty(entity.pri_descricao) )
				Param.Value = entity.pri_descricao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@pri_situacao";
			Param.Size = 1;
			Param.Value = entity.pri_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@pri_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.pri_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@pri_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.pri_dataAlteracao;
			qs.Parameters.Add(Param);
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, ACA_ParametroIntegracao entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pri_id";
			Param.Size = 4;
			Param.Value = entity.pri_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@pri_chave";
			Param.Size = 100;
			Param.Value = entity.pri_chave;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@pri_valor";
			Param.Size = 1000;
			Param.Value = entity.pri_valor;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@pri_descricao";
			Param.Size = 200;
			if( !string.IsNullOrEmpty(entity.pri_descricao) )
				Param.Value = entity.pri_descricao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@pri_situacao";
			Param.Size = 1;
			Param.Value = entity.pri_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@pri_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.pri_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@pri_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.pri_dataAlteracao;
			qs.Parameters.Add(Param);
		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, ACA_ParametroIntegracao entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pri_id";
			Param.Size = 4;
			Param.Value = entity.pri_id;
			qs.Parameters.Add(Param);
		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade 
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_ParametroIntegracao entity)
        {
            entity.pri_id = Convert.ToInt32(qs.Return.Rows[0][0]);
            return (entity.pri_id > 0);
		}		
	}
}

