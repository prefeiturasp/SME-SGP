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
	/// Classe abstrata de ACA_CurriculoPeriodo
	/// </summary>
	public abstract class Abstract_ACA_CurriculoPeriodoDAO : Abstract_DAL<ACA_CurriculoPeriodo>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_CurriculoPeriodo entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cur_id";
			Param.Size = 4;
			Param.Value = entity.cur_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crr_id";
			Param.Size = 4;
			Param.Value = entity.crr_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crp_id";
			Param.Size = 4;
			Param.Value = entity.crp_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_CurriculoPeriodo entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cur_id";
			Param.Size = 4;
			Param.Value = entity.cur_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crr_id";
			Param.Size = 4;
			Param.Value = entity.crr_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crp_id";
			Param.Size = 4;
			Param.Value = entity.crp_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mep_id";
			Param.Size = 4;
			if( entity.mep_id > 0  )
				Param.Value = entity.mep_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crp_ordem";
			Param.Size = 4;
			Param.Value = entity.crp_ordem;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@crp_descricao";
			Param.Size = 200;
			Param.Value = entity.crp_descricao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crp_idadeIdealAnoInicio";
			Param.Size = 4;
			if( entity.crp_idadeIdealAnoInicio > 0  )
				Param.Value = entity.crp_idadeIdealAnoInicio;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crp_idadeIdealMesInicio";
			Param.Size = 4;
			if( entity.crp_idadeIdealMesInicio > 0  )
				Param.Value = entity.crp_idadeIdealMesInicio;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crp_idadeIdealAnoFim";
			Param.Size = 4;
			if( entity.crp_idadeIdealAnoFim > 0  )
				Param.Value = entity.crp_idadeIdealAnoFim;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crp_idadeIdealMesFim";
			Param.Size = 4;
			if( entity.crp_idadeIdealMesFim > 0  )
				Param.Value = entity.crp_idadeIdealMesFim;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@crp_controleTempo";
			Param.Size = 1;
			Param.Value = entity.crp_controleTempo;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@crp_qtdeDiasSemana";
			Param.Size = 1;
			Param.Value = entity.crp_qtdeDiasSemana;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crp_qtdeTemposDia";
			Param.Size = 1;
			if( entity.crp_qtdeTemposDia > 0  )
				Param.Value = entity.crp_qtdeTemposDia;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crp_qtdeTemposSemana";
			Param.Size = 1;
			if( entity.crp_qtdeTemposSemana > 0  )
				Param.Value = entity.crp_qtdeTemposSemana;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crp_qtdeHorasDia";
			Param.Size = 1;
			if( entity.crp_qtdeHorasDia > 0  )
				Param.Value = entity.crp_qtdeHorasDia;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crp_qtdeMinutosDia";
			Param.Size = 1;
			if( entity.crp_qtdeMinutosDia > 0  )
				Param.Value = entity.crp_qtdeMinutosDia;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crp_qtdeEletivasAlunos";
			Param.Size = 1;
			if( entity.crp_qtdeEletivasAlunos > 0  )
				Param.Value = entity.crp_qtdeEletivasAlunos;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@crp_ciclo";
			Param.Size = 100;
			if( !string.IsNullOrEmpty(entity.crp_ciclo) )
				Param.Value = entity.crp_ciclo;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@crp_turmaAvaliacao";
			Param.Size = 1;
			Param.Value = entity.crp_turmaAvaliacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@crp_nomeAvaliacao";
			Param.Size = 100;
			if( !string.IsNullOrEmpty(entity.crp_nomeAvaliacao) )
				Param.Value = entity.crp_nomeAvaliacao;
			else
				Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@crp_concluiNivelEnsino";
            Param.Size = 1;
            Param.Value = entity.crp_concluiNivelEnsino;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@crp_fundoFrente";
            Param.Size = 260;
            if (!string.IsNullOrEmpty(entity.crp_fundoFrente))
                Param.Value = entity.crp_fundoFrente;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@crp_situacao";
			Param.Size = 1;
			Param.Value = entity.crp_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@crp_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.crp_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@crp_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.crp_dataAlteracao;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tci_id";
            Param.Size = 4;
            if (entity.tci_id > 0)
                Param.Value = entity.tci_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tcp_id";
            Param.Size = 4;
            if (entity.tcp_id > 0)
            {
                Param.Value = entity.tcp_id;
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
		protected override void ParamAlterar(QueryStoredProcedure qs, ACA_CurriculoPeriodo entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cur_id";
			Param.Size = 4;
			Param.Value = entity.cur_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crr_id";
			Param.Size = 4;
			Param.Value = entity.crr_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crp_id";
			Param.Size = 4;
			Param.Value = entity.crp_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mep_id";
			Param.Size = 4;
			if( entity.mep_id > 0  )
				Param.Value = entity.mep_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crp_ordem";
			Param.Size = 4;
			Param.Value = entity.crp_ordem;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@crp_descricao";
			Param.Size = 200;
			Param.Value = entity.crp_descricao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crp_idadeIdealAnoInicio";
			Param.Size = 4;
			if( entity.crp_idadeIdealAnoInicio > 0  )
				Param.Value = entity.crp_idadeIdealAnoInicio;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crp_idadeIdealMesInicio";
			Param.Size = 4;
			if( entity.crp_idadeIdealMesInicio > 0  )
				Param.Value = entity.crp_idadeIdealMesInicio;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crp_idadeIdealAnoFim";
			Param.Size = 4;
			if( entity.crp_idadeIdealAnoFim > 0  )
				Param.Value = entity.crp_idadeIdealAnoFim;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crp_idadeIdealMesFim";
			Param.Size = 4;
			if( entity.crp_idadeIdealMesFim > 0  )
				Param.Value = entity.crp_idadeIdealMesFim;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@crp_controleTempo";
			Param.Size = 1;
			Param.Value = entity.crp_controleTempo;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@crp_qtdeDiasSemana";
			Param.Size = 1;
			Param.Value = entity.crp_qtdeDiasSemana;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crp_qtdeTemposDia";
			Param.Size = 1;
			if( entity.crp_qtdeTemposDia > 0  )
				Param.Value = entity.crp_qtdeTemposDia;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crp_qtdeTemposSemana";
			Param.Size = 1;
			if( entity.crp_qtdeTemposSemana > 0  )
				Param.Value = entity.crp_qtdeTemposSemana;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crp_qtdeHorasDia";
			Param.Size = 1;
			if( entity.crp_qtdeHorasDia > 0  )
				Param.Value = entity.crp_qtdeHorasDia;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crp_qtdeMinutosDia";
			Param.Size = 1;
			if( entity.crp_qtdeMinutosDia > 0  )
				Param.Value = entity.crp_qtdeMinutosDia;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crp_qtdeEletivasAlunos";
			Param.Size = 1;
			if( entity.crp_qtdeEletivasAlunos > 0  )
				Param.Value = entity.crp_qtdeEletivasAlunos;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@crp_ciclo";
			Param.Size = 100;
			if( !string.IsNullOrEmpty(entity.crp_ciclo) )
				Param.Value = entity.crp_ciclo;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@crp_turmaAvaliacao";
			Param.Size = 1;
			Param.Value = entity.crp_turmaAvaliacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@crp_nomeAvaliacao";
			Param.Size = 100;
			if( !string.IsNullOrEmpty(entity.crp_nomeAvaliacao) )
				Param.Value = entity.crp_nomeAvaliacao;
			else
				Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@crp_concluiNivelEnsino";
            Param.Size = 1;
            Param.Value = entity.crp_concluiNivelEnsino;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@crp_fundoFrente";
            Param.Size = 260;
            if (!string.IsNullOrEmpty(entity.crp_fundoFrente))
                Param.Value = entity.crp_fundoFrente;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@crp_situacao";
			Param.Size = 1;
			Param.Value = entity.crp_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@crp_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.crp_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@crp_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.crp_dataAlteracao;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tci_id";
            Param.Size = 4;
            if (entity.tci_id > 0)
                Param.Value = entity.tci_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tcp_id";
            Param.Size = 4;
            if (entity.tcp_id > 0)
            {
                Param.Value = entity.tcp_id;
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
		protected override void ParamDeletar(QueryStoredProcedure qs, ACA_CurriculoPeriodo entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cur_id";
			Param.Size = 4;
			Param.Value = entity.cur_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crr_id";
			Param.Size = 4;
			Param.Value = entity.crr_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@crp_id";
			Param.Size = 4;
			Param.Value = entity.crp_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade 
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_CurriculoPeriodo entity)
		{
            entity.crp_id = Convert.ToInt32(qs.Return.Rows[0][0]);
            return (entity.crp_id > 0);
		}		
	}
}

