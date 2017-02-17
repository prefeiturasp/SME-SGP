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
	/// Classe abstrata de MTR_ConfiguracaoProcesso
	/// </summary>
	public abstract class Abstract_MTR_ConfiguracaoProcessoDAO : Abstract_DAL<MTR_ConfiguracaoProcesso>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, MTR_ConfiguracaoProcesso entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cfg_id";
			Param.Size = 4;
			Param.Value = entity.cfg_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cpr_id";
			Param.Size = 4;
			Param.Value = entity.cpr_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, MTR_ConfiguracaoProcesso entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cfg_id";
			Param.Size = 4;
			Param.Value = entity.cfg_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cpr_id";
			Param.Size = 4;
			Param.Value = entity.cpr_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cpr_tipoProcesso";
			Param.Size = 4;
			Param.Value = entity.cpr_tipoProcesso;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@cpr_nome";
			Param.Size = 200;
			Param.Value = entity.cpr_nome;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cpr_ordem";
			Param.Size = 4;
			Param.Value = entity.cpr_ordem;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cpr_preMatricula";
			Param.Size = 1;
			Param.Value = entity.cpr_preMatricula;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cpr_confirmacao";
			Param.Size = 1;
			Param.Value = entity.cpr_confirmacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cpr_confirmacaoPresencial";
			Param.Size = 1;
			Param.Value = entity.cpr_confirmacaoPresencial;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cpr_entregaDoc";
			Param.Size = 1;
			Param.Value = entity.cpr_entregaDoc;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cpr_internet";
			Param.Size = 1;
			Param.Value = entity.cpr_internet;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cpr_permiteAlteracao";
			Param.Size = 1;
			Param.Value = entity.cpr_permiteAlteracao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@cpr_listaEspera";
			Param.Size = 1;
			Param.Value = entity.cpr_listaEspera;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cpr_qtdeOpcoes";
			Param.Size = 4;
			Param.Value = entity.cpr_qtdeOpcoes;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cpr_agendamento";
			Param.Size = 1;
			Param.Value = entity.cpr_agendamento;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cpr_localAtendimento";
			Param.Size = 1;
			if( entity.cpr_localAtendimento > 0  )
				Param.Value = entity.cpr_localAtendimento;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cpr_alocacaoAutomatica";
			Param.Size = 1;
			Param.Value = entity.cpr_alocacaoAutomatica;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cpr_porSorteio";
			Param.Size = 1;
				Param.Value = entity.cpr_porSorteio;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cpr_moveAluno";
			Param.Size = 1;
			Param.Value = entity.cpr_moveAluno;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cpr_alterarTurma";
			Param.Size = 1;
			Param.Value = entity.cpr_alterarTurma;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cpr_manterTurma";
			Param.Size = 1;
			Param.Value = entity.cpr_manterTurma;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cpr_porIdade";
			Param.Size = 1;
			Param.Value = entity.cpr_porIdade;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cpr_porSexo";
			Param.Size = 1;
			Param.Value = entity.cpr_porSexo;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cpr_manual";
			Param.Size = 1;
			Param.Value = entity.cpr_manual;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@cpr_situacao";
			Param.Size = 1;
			Param.Value = entity.cpr_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@cpr_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.cpr_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@cpr_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.cpr_dataAlteracao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@evt_id";
			Param.Size = 8;
			Param.Value = entity.evt_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, MTR_ConfiguracaoProcesso entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cfg_id";
			Param.Size = 4;
			Param.Value = entity.cfg_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cpr_id";
			Param.Size = 4;
			Param.Value = entity.cpr_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cpr_tipoProcesso";
			Param.Size = 4;
			Param.Value = entity.cpr_tipoProcesso;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@cpr_nome";
			Param.Size = 200;
			Param.Value = entity.cpr_nome;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cpr_ordem";
			Param.Size = 4;
			Param.Value = entity.cpr_ordem;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cpr_preMatricula";
			Param.Size = 1;
			Param.Value = entity.cpr_preMatricula;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cpr_confirmacao";
			Param.Size = 1;
			Param.Value = entity.cpr_confirmacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cpr_confirmacaoPresencial";
			Param.Size = 1;
			Param.Value = entity.cpr_confirmacaoPresencial;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cpr_entregaDoc";
			Param.Size = 1;
			Param.Value = entity.cpr_entregaDoc;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cpr_internet";
			Param.Size = 1;
			Param.Value = entity.cpr_internet;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cpr_permiteAlteracao";
			Param.Size = 1;
			Param.Value = entity.cpr_permiteAlteracao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@cpr_listaEspera";
			Param.Size = 1;
			Param.Value = entity.cpr_listaEspera;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cpr_qtdeOpcoes";
			Param.Size = 4;
			Param.Value = entity.cpr_qtdeOpcoes;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cpr_agendamento";
			Param.Size = 1;
			Param.Value = entity.cpr_agendamento;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cpr_localAtendimento";
			Param.Size = 1;
			if( entity.cpr_localAtendimento > 0  )
				Param.Value = entity.cpr_localAtendimento;
			else
				Param.Value = DBNull.Value;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cpr_alocacaoAutomatica";
			Param.Size = 1;
			Param.Value = entity.cpr_alocacaoAutomatica;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cpr_porSorteio";
			Param.Size = 1;
				Param.Value = entity.cpr_porSorteio;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cpr_moveAluno";
			Param.Size = 1;
			Param.Value = entity.cpr_moveAluno;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cpr_alterarTurma";
			Param.Size = 1;
			Param.Value = entity.cpr_alterarTurma;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cpr_manterTurma";
			Param.Size = 1;
			Param.Value = entity.cpr_manterTurma;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cpr_porIdade";
			Param.Size = 1;
			Param.Value = entity.cpr_porIdade;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cpr_porSexo";
			Param.Size = 1;
			Param.Value = entity.cpr_porSexo;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@cpr_manual";
			Param.Size = 1;
			Param.Value = entity.cpr_manual;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@cpr_situacao";
			Param.Size = 1;
			Param.Value = entity.cpr_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@cpr_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.cpr_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@cpr_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.cpr_dataAlteracao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@evt_id";
			Param.Size = 8;
			Param.Value = entity.evt_id;
			qs.Parameters.Add(Param);


		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, MTR_ConfiguracaoProcesso entity)
		{
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cfg_id";
			Param.Size = 4;
			Param.Value = entity.cfg_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@cpr_id";
			Param.Size = 4;
			Param.Value = entity.cpr_id;
			qs.Parameters.Add(Param);


		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade 
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure</param>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, MTR_ConfiguracaoProcesso entity)
		{
            entity.cpr_id = Convert.ToInt32(qs.Return.Rows[0][0]);
            return (entity.cpr_id > 0);
		}		
	}
}

