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
	/// Classe abstrata de CFG_ConfiguracaoInscricaoCreche
	/// </summary>
	public abstract class Abstract_CFG_ConfiguracaoInscricaoCrecheDAO : Abstract_DAL<CFG_ConfiguracaoInscricaoCreche>
	{
	
        protected override string ConnectionStringName
        {
            get
            {
                return "GestaoEscolarInscricaoCreche";
            }
        }
        	
		/// <summary>
		/// Configura os parametros do metodo de carregar
		/// </ssummary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, CFG_ConfiguracaoInscricaoCreche entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@cfg_id";
			Param.Size = 16;
			Param.Value = entity.cfg_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, CFG_ConfiguracaoInscricaoCreche entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@cfg_id";
			Param.Size = 16;
			Param.Value = entity.cfg_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@cfg_chave";
			Param.Size = 100;
			Param.Value = entity.cfg_chave;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@cfg_valor";
			Param.Size = 300;
			Param.Value = entity.cfg_valor;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@cfg_descricao";
			Param.Size = 200;
			if( !string.IsNullOrEmpty(entity.cfg_descricao) )
				Param.Value = entity.cfg_descricao;
			else
				Param.Value = DBNull.Value;
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


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, CFG_ConfiguracaoInscricaoCreche entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@cfg_id";
			Param.Size = 16;
			Param.Value = entity.cfg_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@cfg_chave";
			Param.Size = 100;
			Param.Value = entity.cfg_chave;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@cfg_valor";
			Param.Size = 300;
			Param.Value = entity.cfg_valor;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@cfg_descricao";
			Param.Size = 200;
			if( !string.IsNullOrEmpty(entity.cfg_descricao) )
				Param.Value = entity.cfg_descricao;
			else
				Param.Value = DBNull.Value;
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


		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, CFG_ConfiguracaoInscricaoCreche entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@cfg_id";
			Param.Size = 16;
			Param.Value = entity.cfg_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade 
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CFG_ConfiguracaoInscricaoCreche entity)
		{
            entity.cfg_id = new Guid(qs.Return.Rows[0][0].ToString());
            return (entity.cfg_id != Guid.Empty); 
		}		
	}
}

