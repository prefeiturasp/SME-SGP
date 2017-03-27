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
	/// Classe abstrata de ACA_AlunoJustificativaFalta
	/// </summary>
	public abstract class Abstract_ACA_AlunoJustificativaFaltaDAO : Abstract_DAL<ACA_AlunoJustificativaFalta>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_AlunoJustificativaFalta entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@afj_id";
			Param.Size = 4;
			Param.Value = entity.afj_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_AlunoJustificativaFalta entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@afj_id";
			Param.Size = 4;
			Param.Value = entity.afj_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tjf_id";
			Param.Size = 4;
			Param.Value = entity.tjf_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
			Param.ParameterName = "@afj_dataInicio";
			Param.Size = 20;
			Param.Value = entity.afj_dataInicio;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
			Param.ParameterName = "@afj_dataFim";
			Param.Size = 20;
			if( entity.afj_dataFim!= new DateTime() )
				Param.Value = entity.afj_dataFim;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@afj_situacao";
			Param.Size = 1;
			Param.Value = entity.afj_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@afj_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.afj_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@afj_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.afj_dataAlteracao;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@pro_id";
            Param.Size = 16;
            Param.Value = entity.pro_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@afj_observacao";
            if (!string.IsNullOrEmpty(entity.afj_observacao))
                Param.Value = entity.afj_observacao;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);
        }
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, ACA_AlunoJustificativaFalta entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@afj_id";
			Param.Size = 4;
			Param.Value = entity.afj_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tjf_id";
			Param.Size = 4;
			Param.Value = entity.tjf_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
			Param.ParameterName = "@afj_dataInicio";
			Param.Size = 20;
			Param.Value = entity.afj_dataInicio;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
			Param.ParameterName = "@afj_dataFim";
			Param.Size = 20;
			if( entity.afj_dataFim!= new DateTime() )
				Param.Value = entity.afj_dataFim;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@afj_situacao";
			Param.Size = 1;
			Param.Value = entity.afj_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@afj_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.afj_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@afj_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.afj_dataAlteracao;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@pro_id";
            Param.Size = 16;
            Param.Value = entity.pro_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@afj_observacao";
            if (!string.IsNullOrEmpty(entity.afj_observacao))
                Param.Value = entity.afj_observacao;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

        }

		/// <summary>
		/// Configura os parametros do metodo de Deletar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, ACA_AlunoJustificativaFalta entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@afj_id";
			Param.Size = 4;
			Param.Value = entity.afj_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade 
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_AlunoJustificativaFalta entity)
		{
            entity.afj_id = Convert.ToInt32(qs.Return.Rows[0][0]);
            return (entity.afj_id > 0);
		}		
	}
}

