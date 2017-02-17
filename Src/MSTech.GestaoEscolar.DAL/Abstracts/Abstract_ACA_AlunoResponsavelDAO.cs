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
	/// Classe abstrata de ACA_AlunoResponsavel
	/// </summary>
	public abstract class Abstract_ACA_AlunoResponsavelDAO : Abstract_DAL<ACA_AlunoResponsavel>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_AlunoResponsavel entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@alr_id";
			Param.Size = 4;
			Param.Value = entity.alr_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_AlunoResponsavel entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@alr_id";
			Param.Size = 4;
			Param.Value = entity.alr_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tra_id";
			Param.Size = 4;
			Param.Value = entity.tra_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@pes_id";
            Param.Size = 16;
            if (entity.pes_id != Guid.Empty)
                Param.Value = entity.pes_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@alr_profissao";
			Param.Size = 200;
			if( !string.IsNullOrEmpty(entity.alr_profissao) )
				Param.Value = entity.alr_profissao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@alr_empresa";
            Param.Size = 200;
            if (!string.IsNullOrEmpty(entity.alr_empresa))
                Param.Value = entity.alr_empresa;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@alr_principal";
			Param.Size = 1;
			Param.Value = entity.alr_principal;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@alr_constaCertidaoNascimento";
			Param.Size = 1;
			Param.Value = entity.alr_constaCertidaoNascimento;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@alr_situacao";
			Param.Size = 1;
			Param.Value = entity.alr_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@alr_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.alr_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@alr_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.alr_dataAlteracao;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@alr_apenasFiliacao";
            Param.Size = 1;
            Param.Value = entity.alr_apenasFiliacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@alr_moraComAluno";
            Param.Size = 1;
            Param.Value = entity.alr_moraComAluno;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@alr_omitidoFormaLei";
            Param.Size = 1;
            Param.Value = entity.alr_omitidoFormaLei;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@alr_tipoResponsavel";
            Param.Size = 1;
            if (entity.alr_tipoResponsavel > 0)
            {
                Param.Value = entity.alr_tipoResponsavel;
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
		protected override void ParamAlterar(QueryStoredProcedure qs, ACA_AlunoResponsavel entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@alr_id";
			Param.Size = 4;
			Param.Value = entity.alr_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tra_id";
			Param.Size = 4;
			Param.Value = entity.tra_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@pes_id";
			Param.Size = 16;
            if (entity.pes_id != Guid.Empty)
                Param.Value = entity.pes_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@alr_profissao";
			Param.Size = 200;
			if( !string.IsNullOrEmpty(entity.alr_profissao) )
				Param.Value = entity.alr_profissao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@alr_empresa";
            Param.Size = 200;
            if (!string.IsNullOrEmpty(entity.alr_empresa))
                Param.Value = entity.alr_empresa;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@alr_principal";
			Param.Size = 1;
			Param.Value = entity.alr_principal;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@alr_constaCertidaoNascimento";
			Param.Size = 1;
			Param.Value = entity.alr_constaCertidaoNascimento;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@alr_situacao";
			Param.Size = 1;
			Param.Value = entity.alr_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@alr_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.alr_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@alr_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.alr_dataAlteracao;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@alr_apenasFiliacao";
            Param.Size = 1;
            Param.Value = entity.alr_apenasFiliacao;
            qs.Parameters.Add(Param);


            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@alr_moraComAluno";
            Param.Size = 1;
            Param.Value = entity.alr_moraComAluno;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@alr_omitidoFormaLei";
            Param.Size = 1;
            Param.Value = entity.alr_omitidoFormaLei;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@alr_tipoResponsavel";
            Param.Size = 1;
            if (entity.alr_tipoResponsavel > 0)
            {
                Param.Value = entity.alr_tipoResponsavel;
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
		protected override void ParamDeletar(QueryStoredProcedure qs, ACA_AlunoResponsavel entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@alr_id";
			Param.Size = 4;
			Param.Value = entity.alr_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade 
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_AlunoResponsavel entity)
		{
            entity.alr_id = Convert.ToInt32(qs.Return.Rows[0][0]);
            return (entity.alr_id > 0);
		}		
	}
}

