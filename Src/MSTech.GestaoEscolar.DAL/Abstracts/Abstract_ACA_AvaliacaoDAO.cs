/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL.Abstracts
{
	using System;
	using System.Data;
	using MSTech.Data.Common;
	using MSTech.Data.Common.Abstracts;
	using MSTech.GestaoEscolar.Entities;
	
	/// <summary>
	/// Classe abstrata de ACA_Avaliacao.
	/// </summary>
	public abstract class Abstract_ACA_AvaliacaoDAO : Abstract_DAL<ACA_Avaliacao>
	{
        /// <summary>
		/// ConnectionString.
		/// </summary>
        protected override string ConnectionStringName
        {
            get
            {
                return "GestaoEscolar";
            }
        }
        	
		/// <summary>
		/// Configura os parametros do metodo de carregar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_Avaliacao entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@fav_id";
			Param.Size = 4;
			Param.Value = entity.fav_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@ava_id";
			Param.Size = 4;
			Param.Value = entity.ava_id;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_Avaliacao entity)
		{
			if (entity != null & qs != null)
            {
							Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@fav_id";
			Param.Size = 4;
			Param.Value = entity.fav_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@ava_id";
			Param.Size = 4;
			Param.Value = entity.ava_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@ava_nome";
			Param.Size = 100;
			Param.Value = entity.ava_nome;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@ava_tipo";
			Param.Size = 1;
			Param.Value = entity.ava_tipo;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tpc_id";
			Param.Size = 4;
				if(entity.tpc_id > 0 )
				{
					Param.Value = entity.tpc_id;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@ava_ordemPeriodo";
			Param.Size = 4;
				if(entity.ava_ordemPeriodo > 0 )
				{
					Param.Value = entity.ava_ordemPeriodo;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_apareceBoletim";
			Param.Size = 1;
			Param.Value = entity.ava_apareceBoletim;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@ava_situacao";
			Param.Size = 1;
			Param.Value = entity.ava_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@ava_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.ava_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@ava_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.ava_dataAlteracao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_conceitoGlobalObrigatorio";
			Param.Size = 1;
			Param.Value = entity.ava_conceitoGlobalObrigatorio;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_baseadaConceitoGlobal";
			Param.Size = 1;
			Param.Value = entity.ava_baseadaConceitoGlobal;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_baseadaNotaDisciplina";
			Param.Size = 1;
			Param.Value = entity.ava_baseadaNotaDisciplina;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_baseadaAvaliacaoAdicional";
			Param.Size = 1;
			Param.Value = entity.ava_baseadaAvaliacaoAdicional;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_mostraBoletimConceitoGlobalNota";
			Param.Size = 1;
			Param.Value = entity.ava_mostraBoletimConceitoGlobalNota;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_mostraBoletimConceitoGlobalFrequencia";
			Param.Size = 1;
			Param.Value = entity.ava_mostraBoletimConceitoGlobalFrequencia;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_mostraBoletimConceitoGlobalAvaliacaoAdicional";
			Param.Size = 1;
			Param.Value = entity.ava_mostraBoletimConceitoGlobalAvaliacaoAdicional;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_mostraBoletimDisciplinaNota";
			Param.Size = 1;
			Param.Value = entity.ava_mostraBoletimDisciplinaNota;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_mostraBoletimDisciplinaFrequencia";
			Param.Size = 1;
			Param.Value = entity.ava_mostraBoletimDisciplinaFrequencia;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@ava_recFinalConceitoMaximoAprovacao";
			Param.Size = 1;
				if(entity.ava_recFinalConceitoMaximoAprovacao > 0 )
				{
					Param.Value = entity.ava_recFinalConceitoMaximoAprovacao;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_recFinalConceitoGlobalMinimoNaoAtingido";
			Param.Size = 1;
			Param.Value = entity.ava_recFinalConceitoGlobalMinimoNaoAtingido;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_recFinalFrequenciaMinimaFinalNaoAtingida";
			Param.Size = 1;
			Param.Value = entity.ava_recFinalFrequenciaMinimaFinalNaoAtingida;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_recFinalNotaDisciplinaApenasConceitoGlobalNaoAtingido";
			Param.Size = 1;
			Param.Value = entity.ava_recFinalNotaDisciplinaApenasConceitoGlobalNaoAtingido;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_disciplinaObrigatoria";
			Param.Size = 1;
				Param.Value = entity.ava_disciplinaObrigatoria;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_exibeNaoAvaliados";
			Param.Size = 1;
			Param.Value = entity.ava_exibeNaoAvaliados;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_exibeSemProfessor";
			Param.Size = 1;
			Param.Value = entity.ava_exibeSemProfessor;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_exibeObservacaoDisciplina";
			Param.Size = 1;
			Param.Value = entity.ava_exibeObservacaoDisciplina;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_exibeObservacaoConselhoPedagogico";
			Param.Size = 1;
			Param.Value = entity.ava_exibeObservacaoConselhoPedagogico;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_exibeFrequencia";
			Param.Size = 1;
			Param.Value = entity.ava_exibeFrequencia;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_exibeNotaPosConselho";
			Param.Size = 1;
			Param.Value = entity.ava_exibeNotaPosConselho;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_conceitoGlobalObrigatorioFrequencia";
			Param.Size = 1;
			Param.Value = entity.ava_conceitoGlobalObrigatorioFrequencia;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Decimal;
			Param.ParameterName = "@ava_peso";
			Param.Size = 20;
				Param.Value = entity.ava_peso;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_ocultarAtualizacao";
			Param.Size = 1;
				Param.Value = entity.ava_ocultarAtualizacao;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, ACA_Avaliacao entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@fav_id";
			Param.Size = 4;
			Param.Value = entity.fav_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@ava_id";
			Param.Size = 4;
			Param.Value = entity.ava_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@ava_nome";
			Param.Size = 100;
			Param.Value = entity.ava_nome;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@ava_tipo";
			Param.Size = 1;
			Param.Value = entity.ava_tipo;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tpc_id";
			Param.Size = 4;
				if(entity.tpc_id > 0 )
				{
					Param.Value = entity.tpc_id;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@ava_ordemPeriodo";
			Param.Size = 4;
				if(entity.ava_ordemPeriodo > 0 )
				{
					Param.Value = entity.ava_ordemPeriodo;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_apareceBoletim";
			Param.Size = 1;
			Param.Value = entity.ava_apareceBoletim;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@ava_situacao";
			Param.Size = 1;
			Param.Value = entity.ava_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@ava_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.ava_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@ava_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.ava_dataAlteracao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_conceitoGlobalObrigatorio";
			Param.Size = 1;
			Param.Value = entity.ava_conceitoGlobalObrigatorio;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_baseadaConceitoGlobal";
			Param.Size = 1;
			Param.Value = entity.ava_baseadaConceitoGlobal;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_baseadaNotaDisciplina";
			Param.Size = 1;
			Param.Value = entity.ava_baseadaNotaDisciplina;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_baseadaAvaliacaoAdicional";
			Param.Size = 1;
			Param.Value = entity.ava_baseadaAvaliacaoAdicional;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_mostraBoletimConceitoGlobalNota";
			Param.Size = 1;
			Param.Value = entity.ava_mostraBoletimConceitoGlobalNota;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_mostraBoletimConceitoGlobalFrequencia";
			Param.Size = 1;
			Param.Value = entity.ava_mostraBoletimConceitoGlobalFrequencia;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_mostraBoletimConceitoGlobalAvaliacaoAdicional";
			Param.Size = 1;
			Param.Value = entity.ava_mostraBoletimConceitoGlobalAvaliacaoAdicional;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_mostraBoletimDisciplinaNota";
			Param.Size = 1;
			Param.Value = entity.ava_mostraBoletimDisciplinaNota;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_mostraBoletimDisciplinaFrequencia";
			Param.Size = 1;
			Param.Value = entity.ava_mostraBoletimDisciplinaFrequencia;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@ava_recFinalConceitoMaximoAprovacao";
			Param.Size = 1;
				if(entity.ava_recFinalConceitoMaximoAprovacao > 0 )
				{
					Param.Value = entity.ava_recFinalConceitoMaximoAprovacao;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_recFinalConceitoGlobalMinimoNaoAtingido";
			Param.Size = 1;
			Param.Value = entity.ava_recFinalConceitoGlobalMinimoNaoAtingido;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_recFinalFrequenciaMinimaFinalNaoAtingida";
			Param.Size = 1;
			Param.Value = entity.ava_recFinalFrequenciaMinimaFinalNaoAtingida;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_recFinalNotaDisciplinaApenasConceitoGlobalNaoAtingido";
			Param.Size = 1;
			Param.Value = entity.ava_recFinalNotaDisciplinaApenasConceitoGlobalNaoAtingido;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_disciplinaObrigatoria";
			Param.Size = 1;
				Param.Value = entity.ava_disciplinaObrigatoria;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_exibeNaoAvaliados";
			Param.Size = 1;
			Param.Value = entity.ava_exibeNaoAvaliados;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_exibeSemProfessor";
			Param.Size = 1;
			Param.Value = entity.ava_exibeSemProfessor;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_exibeObservacaoDisciplina";
			Param.Size = 1;
			Param.Value = entity.ava_exibeObservacaoDisciplina;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_exibeObservacaoConselhoPedagogico";
			Param.Size = 1;
			Param.Value = entity.ava_exibeObservacaoConselhoPedagogico;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_exibeFrequencia";
			Param.Size = 1;
			Param.Value = entity.ava_exibeFrequencia;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_exibeNotaPosConselho";
			Param.Size = 1;
			Param.Value = entity.ava_exibeNotaPosConselho;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_conceitoGlobalObrigatorioFrequencia";
			Param.Size = 1;
			Param.Value = entity.ava_conceitoGlobalObrigatorioFrequencia;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Decimal;
			Param.ParameterName = "@ava_peso";
			Param.Size = 20;
				Param.Value = entity.ava_peso;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@ava_ocultarAtualizacao";
			Param.Size = 1;
				Param.Value = entity.ava_ocultarAtualizacao;
			qs.Parameters.Add(Param);


			}
		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, ACA_Avaliacao entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@fav_id";
			Param.Size = 4;
			Param.Value = entity.fav_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@ava_id";
			Param.Size = 4;
			Param.Value = entity.ava_id;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
		/// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		/// <returns>TRUE - Se entity.ParametroId > 0</returns>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_Avaliacao entity)
		{
			if (entity != null & qs != null)
            {
return true;
			}

			return false;
		}		
	}
}