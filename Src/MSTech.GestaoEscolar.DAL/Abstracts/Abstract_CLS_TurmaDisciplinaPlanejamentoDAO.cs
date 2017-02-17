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
	/// Classe abstrata de CLS_TurmaDisciplinaPlanejamento
	/// </summary>
	public abstract class Abstract_CLS_TurmaDisciplinaPlanejamentoDAO : Abstract_DAL<CLS_TurmaDisciplinaPlanejamento>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, CLS_TurmaDisciplinaPlanejamento entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tud_id";
			Param.Size = 8;
			Param.Value = entity.tud_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tdp_id";
			Param.Size = 4;
			Param.Value = entity.tdp_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_TurmaDisciplinaPlanejamento entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tud_id";
			Param.Size = 8;
			Param.Value = entity.tud_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tdp_id";
			Param.Size = 4;
			Param.Value = entity.tdp_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tpc_id";
			Param.Size = 4;
			if( entity.tpc_id > 0  )
				Param.Value = entity.tpc_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Object;
			Param.ParameterName = "@tdp_planejamento";
			Param.Size = 2147483647;
			if( !string.IsNullOrEmpty(entity.tdp_planejamento) )
				Param.Value = entity.tdp_planejamento;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Object;
			Param.ParameterName = "@tdp_diagnostico";
			Param.Size = 2147483647;
			if( !string.IsNullOrEmpty(entity.tdp_diagnostico) )
				Param.Value = entity.tdp_diagnostico;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@tdp_avaliacaoTrabalho";
			Param.Size = 2147483647;
			if( !string.IsNullOrEmpty(entity.tdp_avaliacaoTrabalho) )
				Param.Value = entity.tdp_avaliacaoTrabalho;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@tdp_recursos";
            Param.Size = 2147483647;
            if (!string.IsNullOrEmpty(entity.tdp_recursos))
                Param.Value = entity.tdp_recursos;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@tdp_intervencoesPedagogicas";
            Param.Size = 2147483647;
            if (!string.IsNullOrEmpty(entity.tdp_intervencoesPedagogicas))
            {
                Param.Value = entity.tdp_intervencoesPedagogicas;
            }
            else
            {
                Param.Value = DBNull.Value;
            }
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@tdp_registroIntervencoes";
            Param.Size = 2147483647;
            if (!string.IsNullOrEmpty(entity.tdp_registroIntervencoes))
            {
                Param.Value = entity.tdp_registroIntervencoes;
            }
            else
            {
                Param.Value = DBNull.Value;
            }
            qs.Parameters.Add(Param);

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
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tdp_situacao";
			Param.Size = 1;
			Param.Value = entity.tdp_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tdp_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.tdp_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tdp_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.tdp_dataAlteracao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tdt_posicao";
            Param.Size = 1;
            Param.Value = entity.tdt_posicao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@pro_id";
            Param.Size = 16;
            Param.Value = entity.pro_id;
            qs.Parameters.Add(Param);

		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, CLS_TurmaDisciplinaPlanejamento entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tud_id";
			Param.Size = 8;
			Param.Value = entity.tud_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tdp_id";
			Param.Size = 4;
			Param.Value = entity.tdp_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tpc_id";
			Param.Size = 4;
			if( entity.tpc_id > 0  )
				Param.Value = entity.tpc_id;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Object;
			Param.ParameterName = "@tdp_planejamento";
			Param.Size = 2147483647;
			if( !string.IsNullOrEmpty(entity.tdp_planejamento) )
				Param.Value = entity.tdp_planejamento;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Object;
			Param.ParameterName = "@tdp_diagnostico";
			Param.Size = 2147483647;
			if( !string.IsNullOrEmpty(entity.tdp_diagnostico) )
				Param.Value = entity.tdp_diagnostico;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@tdp_avaliacaoTrabalho";
			Param.Size = 2147483647;
			if( !string.IsNullOrEmpty(entity.tdp_avaliacaoTrabalho) )
				Param.Value = entity.tdp_avaliacaoTrabalho;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@tdp_recursos";
            Param.Size = 2147483647;
            if (!string.IsNullOrEmpty(entity.tdp_recursos))
                Param.Value = entity.tdp_recursos;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@tdp_intervencoesPedagogicas";
            Param.Size = 2147483647;
            if (!string.IsNullOrEmpty(entity.tdp_intervencoesPedagogicas))
            {
                Param.Value = entity.tdp_intervencoesPedagogicas;
            }
            else
            {
                Param.Value = DBNull.Value;
            }
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@tdp_registroIntervencoes";
            Param.Size = 2147483647;
            if (!string.IsNullOrEmpty(entity.tdp_registroIntervencoes))
            {
                Param.Value = entity.tdp_registroIntervencoes;
            }
            else
            {
                Param.Value = DBNull.Value;
            }
            qs.Parameters.Add(Param);

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
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@tdp_situacao";
			Param.Size = 1;
			Param.Value = entity.tdp_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tdp_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.tdp_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@tdp_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.tdp_dataAlteracao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tdt_posicao";
            Param.Size = 1;
            Param.Value = entity.tdt_posicao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@pro_id";
            Param.Size = 16;
            Param.Value = entity.pro_id;
            qs.Parameters.Add(Param);

		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, CLS_TurmaDisciplinaPlanejamento entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tud_id";
			Param.Size = 8;
			Param.Value = entity.tud_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tdp_id";
			Param.Size = 4;
			Param.Value = entity.tdp_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade 
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CLS_TurmaDisciplinaPlanejamento entity)
		{
		    return true;
		}		
	}
}

