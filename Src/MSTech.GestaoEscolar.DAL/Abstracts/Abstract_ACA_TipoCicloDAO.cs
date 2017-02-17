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
	/// Classe abstrata de ACA_TipoCiclo
	/// </summary>
	public abstract class Abstract_ACA_TipoCicloDAO : Abstract_DAL<ACA_TipoCiclo>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_TipoCiclo entity)
		{
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tci_id";
            Param.Size = 4;
            Param.Value = entity.tci_id;
            qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_TipoCiclo entity)
		{
            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@tci_nome";
            Param.Size = 100;
            Param.Value = entity.tci_nome;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tci_situacao";
            Param.Size = 1;
            Param.Value = entity.tci_situacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@tci_dataCriacao";
            Param.Size = 16;
            Param.Value = entity.tci_dataCriacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@tci_dataAlteracao";
            Param.Size = 16;
            Param.Value = entity.tci_dataAlteracao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@tci_exibirBoletim";
            Param.Size = 1;
            Param.Value = entity.tci_exibirBoletim;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tci_ordem";
            Param.Size = 4;
            Param.Value = entity.tci_ordem;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@tci_layout";
            Param.Size = 50;
            if (!string.IsNullOrEmpty(entity.tci_layout))
            {
                Param.Value = entity.tci_layout;
            }
            else
            {
                Param.Value = DBNull.Value;
            }
            qs.Parameters.Add(Param);

		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, ACA_TipoCiclo entity)
		{
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tci_id";
            Param.Size = 4;
            Param.Value = entity.tci_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@tci_nome";
            Param.Size = 100;
            Param.Value = entity.tci_nome;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tci_situacao";
            Param.Size = 1;
            Param.Value = entity.tci_situacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@tci_dataCriacao";
            Param.Size = 16;
            Param.Value = entity.tci_dataCriacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@tci_dataAlteracao";
            Param.Size = 16;
            Param.Value = entity.tci_dataAlteracao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@tci_exibirBoletim";
            Param.Size = 1;
            Param.Value = entity.tci_exibirBoletim;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tci_ordem";
            Param.Size = 4;
            Param.Value = entity.tci_ordem;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@tci_layout";
            Param.Size = 50;
            if (!string.IsNullOrEmpty(entity.tci_layout))
            {
                Param.Value = entity.tci_layout;
            }
            else
            {
                Param.Value = DBNull.Value;
            }
            qs.Parameters.Add(Param);

		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, ACA_TipoCiclo entity)
		{
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tci_id";
            Param.Size = 4;
            Param.Value = entity.tci_id;
            qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade 
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_TipoCiclo entity)
		{
            entity.tci_id = Convert.ToInt32(qs.Return.Rows[0][0]);
            return (entity.tci_id > 0);
		}		
	}
}

