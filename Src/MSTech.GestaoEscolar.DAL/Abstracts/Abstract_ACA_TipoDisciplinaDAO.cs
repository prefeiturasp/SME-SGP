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
	/// Classe abstrata de ACA_TipoDisciplina
	/// </summary>
	public abstract class Abstract_ACA_TipoDisciplinaDAO : Abstract_DAL<ACA_TipoDisciplina>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_TipoDisciplina entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tds_id";
			Param.Size = 4;
			Param.Value = entity.tds_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_TipoDisciplina entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tne_id";
			Param.Size = 4;
			Param.Value = entity.tne_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mds_id";
			Param.Size = 4;
			if (entity.mds_id > 0)
				Param.Value = entity.mds_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@tds_nome";
			Param.Size = 100;
			Param.Value = entity.tds_nome;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tds_ordem";
			Param.Size = 4;
			if (entity.tds_ordem > 0)
				Param.Value = entity.tds_ordem;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tds_base";
			Param.Size = 1;
			Param.Value = entity.tds_base;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@aco_id";
            Param.Size = 4;
            if (entity.aco_id > 0)
                Param.Value = entity.aco_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tds_tipo";
            Param.Size = 1;
            Param.Value = entity.tds_tipo;
            qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tds_situacao";
			Param.Size = 1;
			Param.Value = entity.tds_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tds_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.tds_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tds_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.tds_dataAlteracao;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@tds_nomeDisciplinaEspecial";
            Param.Size = 200;
            if (!string.IsNullOrEmpty(entity.tds_nomeDisciplinaEspecial))
            {
                Param.Value = entity.tds_nomeDisciplinaEspecial;
            }
            else
            {
                Param.Value = DBNull.Value;
            }
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tds_qtdeDisciplinaRelacionada";
            Param.Size = 4;
            Param.Value = entity.tds_qtdeDisciplinaRelacionada;
            qs.Parameters.Add(Param);
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, ACA_TipoDisciplina entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tds_id";
			Param.Size = 4;
			Param.Value = entity.tds_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tne_id";
			Param.Size = 4;
			Param.Value = entity.tne_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mds_id";
			Param.Size = 4;
			if (entity.mds_id > 0)
				Param.Value = entity.mds_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@tds_nome";
			Param.Size = 100;
			Param.Value = entity.tds_nome;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tds_ordem";
			Param.Size = 4;
			if (entity.tds_ordem > 0)
				Param.Value = entity.tds_ordem;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tds_base";
			Param.Size = 1;
			Param.Value = entity.tds_base;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@aco_id";
            Param.Size = 4;
            if (entity.aco_id > 0)
                Param.Value = entity.aco_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tds_tipo";
            Param.Size = 1;
            Param.Value = entity.tds_tipo;
            qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tds_situacao";
			Param.Size = 1;
			Param.Value = entity.tds_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tds_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.tds_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tds_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.tds_dataAlteracao;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@tds_nomeDisciplinaEspecial";
            Param.Size = 200;
            if (!string.IsNullOrEmpty(entity.tds_nomeDisciplinaEspecial))
            {
                Param.Value = entity.tds_nomeDisciplinaEspecial;
            }
            else
            {
                Param.Value = DBNull.Value;
            }
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tds_qtdeDisciplinaRelacionada";
            Param.Size = 4;
            Param.Value = entity.tds_qtdeDisciplinaRelacionada;
            qs.Parameters.Add(Param);
		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, ACA_TipoDisciplina entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tds_id";
			Param.Size = 4;
			Param.Value = entity.tds_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade 
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_TipoDisciplina entity)
		{
			entity.tds_id = Convert.ToInt32(qs.Return.Rows[0][0]);
			return (entity.tds_id > 0);
		}		
	}
}

