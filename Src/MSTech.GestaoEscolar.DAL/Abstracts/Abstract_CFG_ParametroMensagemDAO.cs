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
	/// Classe abstrata de CFG_ParametroMensagem
	/// </summary>
	public abstract class Abstract_CFG_ParametroMensagemDAO : Abstract_DAL<CFG_ParametroMensagem>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, CFG_ParametroMensagem entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pms_id";
			Param.Size = 4;
			Param.Value = entity.pms_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, CFG_ParametroMensagem entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@pms_tela";
			Param.Size = 1;
			Param.Value = entity.pms_tela;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@pms_chave";
			Param.Size = 100;
			Param.Value = entity.pms_chave;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@pms_valor";
			Param.Size = 2000;
			Param.Value = entity.pms_valor;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@pms_descricao";
			Param.Size = 200;
			if( !string.IsNullOrEmpty(entity.pms_descricao) )
				Param.Value = entity.pms_descricao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@pms_situacao";
			Param.Size = 1;
			Param.Value = entity.pms_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@pms_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.pms_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@pms_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.pms_dataAlteracao;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, CFG_ParametroMensagem entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pms_id";
			Param.Size = 4;
			Param.Value = entity.pms_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@pms_tela";
			Param.Size = 1;
			Param.Value = entity.pms_tela;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@pms_chave";
			Param.Size = 100;
			Param.Value = entity.pms_chave;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@pms_valor";
			Param.Size = 2000;
			Param.Value = entity.pms_valor;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@pms_descricao";
			Param.Size = 200;
			if( !string.IsNullOrEmpty(entity.pms_descricao) )
				Param.Value = entity.pms_descricao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@pms_situacao";
			Param.Size = 1;
			Param.Value = entity.pms_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@pms_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.pms_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@pms_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.pms_dataAlteracao;
			qs.Parameters.Add(Param);


		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, CFG_ParametroMensagem entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@pms_id";
			Param.Size = 4;
			Param.Value = entity.pms_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade 
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CFG_ParametroMensagem entity)
		{
			entity.pms_id = Convert.ToInt32(qs.Return.Rows[0][0]);
			return (entity.pms_id > 0);
		}		
	}
}

