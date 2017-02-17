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
	/// Classe abstrata de ACA_AlunoHistoricoDisciplina
	/// </summary>
	public abstract class Abstract_ACA_AlunoHistoricoDisciplinaDAO : Abstract_DAL<ACA_AlunoHistoricoDisciplina>
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
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_AlunoHistoricoDisciplina entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@alh_id";
			Param.Size = 4;
			Param.Value = entity.alh_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@ahd_id";
			Param.Size = 4;
			Param.Value = entity.ahd_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_AlunoHistoricoDisciplina entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@alh_id";
			Param.Size = 4;
			Param.Value = entity.alh_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@ahd_id";
			Param.Size = 4;
			Param.Value = entity.ahd_id;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ahp_id";
            Param.Size = 4;
            if (entity.ahp_id > 0)
                Param.Value = entity.ahp_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tds_id";
			Param.Size = 4;
			if( entity.tds_id > 0  )
				Param.Value = entity.tds_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@ahd_disciplina";
			Param.Size = 200;
			Param.Value = entity.ahd_disciplina;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@ahd_resultado";
			Param.Size = 1;
			if( entity.ahd_resultado > 0  )
				Param.Value = entity.ahd_resultado;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@ahd_resultadoDescricao";
			Param.Size = 30;
			if( !string.IsNullOrEmpty(entity.ahd_resultadoDescricao) )
				Param.Value = entity.ahd_resultadoDescricao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@ahd_avaliacao";
			Param.Size = 100;
			if( !string.IsNullOrEmpty(entity.ahd_avaliacao) )
				Param.Value = entity.ahd_avaliacao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@ahd_frequencia";
			Param.Size = 100;
			if( !string.IsNullOrEmpty(entity.ahd_frequencia) )
				Param.Value = entity.ahd_frequencia;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ahd_indicacaoDependencia";
			Param.Size = 1;
			Param.Value = entity.ahd_indicacaoDependencia;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@ahd_situacao";
			Param.Size = 1;
			Param.Value = entity.ahd_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@ahd_qtdeFaltas";
			Param.Size = 4;
			if( entity.ahd_qtdeFaltas > 0  )
				Param.Value = entity.ahd_qtdeFaltas;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, ACA_AlunoHistoricoDisciplina entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@alh_id";
			Param.Size = 4;
			Param.Value = entity.alh_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@ahd_id";
			Param.Size = 4;
			Param.Value = entity.ahd_id;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ahp_id";
            Param.Size = 4;
            if (entity.ahp_id > 0)
                Param.Value = entity.ahp_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tds_id";
			Param.Size = 4;
			if( entity.tds_id > 0  )
				Param.Value = entity.tds_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@ahd_disciplina";
			Param.Size = 200;
			Param.Value = entity.ahd_disciplina;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@ahd_resultado";
			Param.Size = 1;
			if( entity.ahd_resultado > 0  )
				Param.Value = entity.ahd_resultado;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@ahd_resultadoDescricao";
			Param.Size = 30;
			if( !string.IsNullOrEmpty(entity.ahd_resultadoDescricao) )
				Param.Value = entity.ahd_resultadoDescricao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@ahd_avaliacao";
			Param.Size = 100;
			if( !string.IsNullOrEmpty(entity.ahd_avaliacao) )
				Param.Value = entity.ahd_avaliacao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@ahd_frequencia";
			Param.Size = 100;
			if( !string.IsNullOrEmpty(entity.ahd_frequencia) )
				Param.Value = entity.ahd_frequencia;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ahd_indicacaoDependencia";
			Param.Size = 1;
			Param.Value = entity.ahd_indicacaoDependencia;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@ahd_situacao";
			Param.Size = 1;
			Param.Value = entity.ahd_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@ahd_qtdeFaltas";
			Param.Size = 4;
			if( entity.ahd_qtdeFaltas > 0  )
				Param.Value = entity.ahd_qtdeFaltas;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);


		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, ACA_AlunoHistoricoDisciplina entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@alh_id";
			Param.Size = 4;
			Param.Value = entity.alh_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@ahd_id";
			Param.Size = 4;
			Param.Value = entity.ahd_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade 
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_AlunoHistoricoDisciplina entity)
		{
		    return true;
		}		
	}
}

