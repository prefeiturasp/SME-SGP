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
	/// Classe abstrata de ESC_TipoClassificacaoEscola
	/// </summary>
	public abstract class Abstract_ESC_TipoClassificacaoEscolaDAO : Abstract_DAL<ESC_TipoClassificacaoEscola>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, ESC_TipoClassificacaoEscola entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tce_id";
			Param.Size = 4;
			Param.Value = entity.tce_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, ESC_TipoClassificacaoEscola entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@tce_nome";
			Param.Size = 100;
			Param.Value = entity.tce_nome;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tce_situacao";
			Param.Size = 1;
			Param.Value = entity.tce_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tce_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.tce_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tce_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.tce_dataAlteracao;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@tce_permiteQualquerCargoEscola";
            Param.Size = 1;
            Param.Value = entity.tce_permiteQualquerCargoEscola;
            qs.Parameters.Add(Param);
        }
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, ESC_TipoClassificacaoEscola entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tce_id";
			Param.Size = 4;
			Param.Value = entity.tce_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@tce_nome";
			Param.Size = 100;
			Param.Value = entity.tce_nome;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tce_situacao";
			Param.Size = 1;
			Param.Value = entity.tce_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tce_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.tce_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tce_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.tce_dataAlteracao;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@tce_permiteQualquerCargoEscola";
            Param.Size = 1;
            Param.Value = entity.tce_permiteQualquerCargoEscola;
            qs.Parameters.Add(Param);
        }

		/// <summary>
		/// Configura os parametros do metodo de Deletar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, ESC_TipoClassificacaoEscola entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tce_id";
			Param.Size = 4;
			Param.Value = entity.tce_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade 
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ESC_TipoClassificacaoEscola entity)
		{
			entity.tce_id = Convert.ToInt32(qs.Return.Rows[0][0]);
			return (entity.tce_id > 0);
		}		
	}
}

