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
	/// Classe abstrata de ACA_Aluno
	/// </summary>
	public abstract class Abstract_ACA_AlunoDAO : Abstract_DAL<ACA_Aluno>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_Aluno entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_Aluno entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@pes_id";
			Param.Size = 16;
			Param.Value = entity.pes_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@ent_id";
			Param.Size = 16;
			Param.Value = entity.ent_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@alu_observacao";
			Param.Size = 2147483647;
			if( !string.IsNullOrEmpty(entity.alu_observacao) )
				Param.Value = entity.alu_observacao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@alu_dadosIncompletos";
			Param.Size = 1;
			Param.Value = entity.alu_dadosIncompletos;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@alu_historicoEscolaIncompleto";
			Param.Size = 1;
			Param.Value = entity.alu_historicoEscolaIncompleto;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@rlg_id";
			Param.Size = 4;
			if( entity.rlg_id > 0  )
				Param.Value = entity.rlg_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
			Param.ParameterName = "@alu_meioTransporte";
			Param.Size = 1;
			if( entity.alu_meioTransporte > 0  )
				Param.Value = entity.alu_meioTransporte;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
			Param.ParameterName = "@alu_tempoDeslocamento";
			Param.Size = 1;
			if( entity.alu_tempoDeslocamento > 0  )
				Param.Value = entity.alu_tempoDeslocamento;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@alu_regressaSozinho";
			Param.Size = 1;
			Param.Value = entity.alu_regressaSozinho;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Object;
            Param.ParameterName = "@alu_dataCadastroFisico";
            Param.Size = 20;
            if (entity.alu_dataCadastroFisico != new DateTime())
                Param.Value = entity.alu_dataCadastroFisico;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@alu_responsavelInfo";
            Param.Size = 100;
            if (!string.IsNullOrEmpty(entity.alu_responsavelInfo))
                Param.Value = entity.alu_responsavelInfo;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@alu_responsavelInfoDoc";
            Param.Size = 20;
            if (!string.IsNullOrEmpty(entity.alu_responsavelInfoDoc))
                Param.Value = entity.alu_responsavelInfoDoc;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@alu_responsavelInfoOrgaoEmissao";
            Param.Size = 20;
            if (!string.IsNullOrEmpty(entity.alu_responsavelInfoOrgaoEmissao))
                Param.Value = entity.alu_responsavelInfoOrgaoEmissao;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@alu_situacao";
			Param.Size = 1;
			Param.Value = entity.alu_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@alu_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.alu_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@alu_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.alu_dataAlteracao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@alu_aulaReligiao";
            Param.Size = 1;
            Param.Value = entity.alu_aulaReligiao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@alu_gemeo";
            Param.Size = 1;
            Param.Value = entity.alu_gemeo;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@alu_possuiEmail";
            Param.Size = 1;
            Param.Value = entity.alu_possuiEmail;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@end_id";
            Param.Size = 16;
            Param.Value = entity.end_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@alu_observacaoSituacao";
            Param.Size = 2147483647;
            if (!string.IsNullOrEmpty(entity.alu_observacaoSituacao))
                Param.Value = entity.alu_observacaoSituacao;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@alu_codigoExterno";
            Param.Size = 200;
            if (!string.IsNullOrEmpty(entity.alu_codigoExterno))
                Param.Value = entity.alu_codigoExterno;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@alu_protocoloExcedente";
            Param.Size = 20;
            if (!string.IsNullOrEmpty(entity.alu_protocoloExcedente))
                Param.Value = entity.alu_protocoloExcedente;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@alu_possuiInformacaoSigilosa";
            Param.Size = 1;
            Param.Value = entity.alu_possuiInformacaoSigilosa;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@alu_bloqueioBoletimOnline";
            Param.Size = 1;
            Param.Value = entity.alu_bloqueioBoletimOnline;
            qs.Parameters.Add(Param);
        }
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, ACA_Aluno entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@pes_id";
			Param.Size = 16;
			Param.Value = entity.pes_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@ent_id";
			Param.Size = 16;
			Param.Value = entity.ent_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@alu_observacao";
			Param.Size = 2147483647;
			if( !string.IsNullOrEmpty(entity.alu_observacao) )
				Param.Value = entity.alu_observacao;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@alu_dadosIncompletos";
			Param.Size = 1;
			Param.Value = entity.alu_dadosIncompletos;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@alu_historicoEscolaIncompleto";
			Param.Size = 1;
			Param.Value = entity.alu_historicoEscolaIncompleto;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@rlg_id";
			Param.Size = 4;
			if( entity.rlg_id > 0  )
				Param.Value = entity.rlg_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
			Param.ParameterName = "@alu_meioTransporte";
			Param.Size = 1;
			if( entity.alu_meioTransporte > 0  )
				Param.Value = entity.alu_meioTransporte;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
			Param.ParameterName = "@alu_tempoDeslocamento";
			Param.Size = 1;
			if( entity.alu_tempoDeslocamento > 0  )
				Param.Value = entity.alu_tempoDeslocamento;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@alu_regressaSozinho";
			Param.Size = 1;
			Param.Value = entity.alu_regressaSozinho;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Object;
            Param.ParameterName = "@alu_dataCadastroFisico";
            Param.Size = 20;
            if (entity.alu_dataCadastroFisico != new DateTime())
                Param.Value = entity.alu_dataCadastroFisico;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@alu_responsavelInfo";
            Param.Size = 100;
            if (!string.IsNullOrEmpty(entity.alu_responsavelInfo))
                Param.Value = entity.alu_responsavelInfo;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@alu_responsavelInfoDoc";
            Param.Size = 20;
            if (!string.IsNullOrEmpty(entity.alu_responsavelInfoDoc))
                Param.Value = entity.alu_responsavelInfoDoc;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@alu_responsavelInfoOrgaoEmissao";
            Param.Size = 20;
            if (!string.IsNullOrEmpty(entity.alu_responsavelInfoOrgaoEmissao))
                Param.Value = entity.alu_responsavelInfoOrgaoEmissao;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@alu_situacao";
			Param.Size = 1;
			Param.Value = entity.alu_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@alu_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.alu_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@alu_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.alu_dataAlteracao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@alu_aulaReligiao";
            Param.Size = 1;
            Param.Value = entity.alu_aulaReligiao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@alu_gemeo";
            Param.Size = 1;
            Param.Value = entity.alu_gemeo;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@alu_possuiEmail";
            Param.Size = 1;
            Param.Value = entity.alu_possuiEmail;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@end_id";
            Param.Size = 16;
            Param.Value = entity.end_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@alu_observacaoSituacao";
            Param.Size = 2147483647;
            if (!string.IsNullOrEmpty(entity.alu_observacaoSituacao))
                Param.Value = entity.alu_observacaoSituacao;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@alu_codigoExterno";
            Param.Size = 200;
            if (!string.IsNullOrEmpty(entity.alu_codigoExterno))
                Param.Value = entity.alu_codigoExterno;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@alu_protocoloExcedente";
            Param.Size = 20;
            if (!string.IsNullOrEmpty(entity.alu_protocoloExcedente))
                Param.Value = entity.alu_protocoloExcedente;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@alu_possuiInformacaoSigilosa";
            Param.Size = 1;
            Param.Value = entity.alu_possuiInformacaoSigilosa;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@alu_bloqueioBoletimOnline";
            Param.Size = 1;
            Param.Value = entity.alu_bloqueioBoletimOnline;
            qs.Parameters.Add(Param);
        }

		/// <summary>
		/// Configura os parametros do metodo de Deletar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, ACA_Aluno entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade 
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_Aluno entity)
		{
			entity.alu_id = Convert.ToInt32(qs.Return.Rows[0][0]);
			return (entity.alu_id > 0);
		}		
	}
}

