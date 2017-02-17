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
	/// Classe abstrata de ACA_FormatoAvaliacao.
	/// </summary>
	public abstract class Abstract_ACA_FormatoAvaliacaoDAO : Abstract_DAL<ACA_FormatoAvaliacao>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_FormatoAvaliacao entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@fav_id";
			Param.Size = 4;
			Param.Value = entity.fav_id;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_FormatoAvaliacao entity)
		{
			if (entity != null & qs != null)
            {
							Param = qs.NewParameter();
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@ent_id";
			Param.Size = 16;
			Param.Value = entity.ent_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@esc_id";
			Param.Size = 4;
				if(entity.esc_id > 0 )
				{
					Param.Value = entity.esc_id;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@uni_id";
			Param.Size = 4;
				if(entity.uni_id > 0 )
				{
					Param.Value = entity.uni_id;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@fav_padrao";
			Param.Size = 1;
			Param.Value = entity.fav_padrao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@fav_nome";
			Param.Size = 100;
			Param.Value = entity.fav_nome;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@fav_tipo";
			Param.Size = 1;
				if(entity.fav_tipo > 0 )
				{
					Param.Value = entity.fav_tipo;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@fav_tipoLancamentoFrequencia";
			Param.Size = 1;
			Param.Value = entity.fav_tipoLancamentoFrequencia;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@fav_tipoApuracaoFrequencia";
			Param.Size = 1;
			Param.Value = entity.fav_tipoApuracaoFrequencia;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@fav_calculoQtdeAulasDadas";
			Param.Size = 1;
			Param.Value = entity.fav_calculoQtdeAulasDadas;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@esa_idConceitoGlobal";
			Param.Size = 4;
				if(entity.esa_idConceitoGlobal > 0 )
				{
					Param.Value = entity.esa_idConceitoGlobal;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@esa_idPorDisciplina";
			Param.Size = 4;
				if(entity.esa_idPorDisciplina > 0 )
				{
					Param.Value = entity.esa_idPorDisciplina;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@esa_idDocente";
			Param.Size = 4;
				if(entity.esa_idDocente > 0 )
				{
					Param.Value = entity.esa_idDocente;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@esa_idConceitoGlobalAdicional";
			Param.Size = 4;
				if(entity.esa_idConceitoGlobalAdicional > 0 )
				{
					Param.Value = entity.esa_idConceitoGlobalAdicional;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@fav_conceitoGlobalAdicional";
			Param.Size = 1;
			Param.Value = entity.fav_conceitoGlobalAdicional;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@fav_conceitoGlobalDocente";
			Param.Size = 1;
			Param.Value = entity.fav_conceitoGlobalDocente;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@fav_obrigatorioRelatorioReprovacao";
			Param.Size = 1;
			Param.Value = entity.fav_obrigatorioRelatorioReprovacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@fav_planejamentoAulasNotasConjunto";
			Param.Size = 1;
			Param.Value = entity.fav_planejamentoAulasNotasConjunto;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@fav_bloqueiaFrequenciaEfetivacao";
			Param.Size = 1;
			Param.Value = entity.fav_bloqueiaFrequenciaEfetivacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@fav_bloqueiaFrequenciaEfetivacaoDisciplina";
			Param.Size = 1;
			Param.Value = entity.fav_bloqueiaFrequenciaEfetivacaoDisciplina;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@valorMinimoAprovacaoConceitoGlobal";
			Param.Size = 10;
				if(!string.IsNullOrEmpty(entity.valorMinimoAprovacaoConceitoGlobal))
				{
					Param.Value = entity.valorMinimoAprovacaoConceitoGlobal;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@valorMinimoAprovacaoPorDisciplina";
			Param.Size = 10;
				if(!string.IsNullOrEmpty(entity.valorMinimoAprovacaoPorDisciplina))
				{
					Param.Value = entity.valorMinimoAprovacaoPorDisciplina;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Decimal;
			Param.ParameterName = "@percentualMinimoFrequencia";
			Param.Size = 7;
				Param.Value = entity.percentualMinimoFrequencia;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tipoProgressaoParcial";
			Param.Size = 1;
				if(entity.tipoProgressaoParcial > 0 )
				{
					Param.Value = entity.tipoProgressaoParcial;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@valorMinimoProgressaoParcialPorDisciplina";
			Param.Size = 10;
				if(!string.IsNullOrEmpty(entity.valorMinimoProgressaoParcialPorDisciplina))
				{
					Param.Value = entity.valorMinimoProgressaoParcialPorDisciplina;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@qtdeMaxDisciplinasProgressaoParcial";
			Param.Size = 1;
				if(entity.qtdeMaxDisciplinasProgressaoParcial > 0 )
				{
					Param.Value = entity.qtdeMaxDisciplinasProgressaoParcial;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@fav_situacao";
			Param.Size = 1;
			Param.Value = entity.fav_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@fav_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.fav_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@fav_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.fav_dataAlteracao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@fav_criterioAprovacaoResultadoFinal";
			Param.Size = 1;
				if(entity.fav_criterioAprovacaoResultadoFinal > 0 )
				{
					Param.Value = entity.fav_criterioAprovacaoResultadoFinal;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@fav_avaliacaoFinalAnalitica";
			Param.Size = 1;
			Param.Value = entity.fav_avaliacaoFinalAnalitica;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Decimal;
			Param.ParameterName = "@fav_variacao";
			Param.Size = 20;
			Param.Value = entity.fav_variacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@fav_sugerirResultadoFinalDisciplina";
			Param.Size = 1;
			Param.Value = entity.fav_sugerirResultadoFinalDisciplina;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Decimal;
			Param.ParameterName = "@fav_percentualMinimoFrequenciaFinalAjustadaDisciplina";
			Param.Size = 7;
				Param.Value = entity.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@fav_exibirBotaoSomaMedia";
			Param.Size = 1;
			Param.Value = entity.fav_exibirBotaoSomaMedia;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@valorMinimoAprovacaoDocente";
			Param.Size = 10;
				if(!string.IsNullOrEmpty(entity.valorMinimoAprovacaoDocente))
				{
					Param.Value = entity.valorMinimoAprovacaoDocente;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Decimal;
			Param.ParameterName = "@percentualBaixaFrequencia";
			Param.Size = 7;
				Param.Value = entity.percentualBaixaFrequencia;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@fav_fechamentoAutomatico";
			Param.Size = 1;
			Param.Value = entity.fav_fechamentoAutomatico;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@fav_permiteRecuperacaoQualquerNota";
			Param.Size = 1;
			Param.Value = entity.fav_permiteRecuperacaoQualquerNota;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@fav_permiteRecuperacaoForaPeriodo";
			Param.Size = 1;
			Param.Value = entity.fav_permiteRecuperacaoForaPeriodo;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@fav_calcularMediaAvaliacaoFinal";
			Param.Size = 1;
			Param.Value = entity.fav_calcularMediaAvaliacaoFinal;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, ACA_FormatoAvaliacao entity)
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
			Param.DbType = DbType.Guid;
			Param.ParameterName = "@ent_id";
			Param.Size = 16;
			Param.Value = entity.ent_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@esc_id";
			Param.Size = 4;
				if(entity.esc_id > 0 )
				{
					Param.Value = entity.esc_id;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@uni_id";
			Param.Size = 4;
				if(entity.uni_id > 0 )
				{
					Param.Value = entity.uni_id;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@fav_padrao";
			Param.Size = 1;
			Param.Value = entity.fav_padrao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@fav_nome";
			Param.Size = 100;
			Param.Value = entity.fav_nome;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@fav_tipo";
			Param.Size = 1;
				if(entity.fav_tipo > 0 )
				{
					Param.Value = entity.fav_tipo;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@fav_tipoLancamentoFrequencia";
			Param.Size = 1;
			Param.Value = entity.fav_tipoLancamentoFrequencia;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@fav_tipoApuracaoFrequencia";
			Param.Size = 1;
			Param.Value = entity.fav_tipoApuracaoFrequencia;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@fav_calculoQtdeAulasDadas";
			Param.Size = 1;
			Param.Value = entity.fav_calculoQtdeAulasDadas;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@esa_idConceitoGlobal";
			Param.Size = 4;
				if(entity.esa_idConceitoGlobal > 0 )
				{
					Param.Value = entity.esa_idConceitoGlobal;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@esa_idPorDisciplina";
			Param.Size = 4;
				if(entity.esa_idPorDisciplina > 0 )
				{
					Param.Value = entity.esa_idPorDisciplina;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@esa_idDocente";
			Param.Size = 4;
				if(entity.esa_idDocente > 0 )
				{
					Param.Value = entity.esa_idDocente;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@esa_idConceitoGlobalAdicional";
			Param.Size = 4;
				if(entity.esa_idConceitoGlobalAdicional > 0 )
				{
					Param.Value = entity.esa_idConceitoGlobalAdicional;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@fav_conceitoGlobalAdicional";
			Param.Size = 1;
			Param.Value = entity.fav_conceitoGlobalAdicional;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@fav_conceitoGlobalDocente";
			Param.Size = 1;
			Param.Value = entity.fav_conceitoGlobalDocente;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@fav_obrigatorioRelatorioReprovacao";
			Param.Size = 1;
			Param.Value = entity.fav_obrigatorioRelatorioReprovacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@fav_planejamentoAulasNotasConjunto";
			Param.Size = 1;
			Param.Value = entity.fav_planejamentoAulasNotasConjunto;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@fav_bloqueiaFrequenciaEfetivacao";
			Param.Size = 1;
			Param.Value = entity.fav_bloqueiaFrequenciaEfetivacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@fav_bloqueiaFrequenciaEfetivacaoDisciplina";
			Param.Size = 1;
			Param.Value = entity.fav_bloqueiaFrequenciaEfetivacaoDisciplina;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@valorMinimoAprovacaoConceitoGlobal";
			Param.Size = 10;
				if(!string.IsNullOrEmpty(entity.valorMinimoAprovacaoConceitoGlobal))
				{
					Param.Value = entity.valorMinimoAprovacaoConceitoGlobal;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@valorMinimoAprovacaoPorDisciplina";
			Param.Size = 10;
				if(!string.IsNullOrEmpty(entity.valorMinimoAprovacaoPorDisciplina))
				{
					Param.Value = entity.valorMinimoAprovacaoPorDisciplina;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Decimal;
			Param.ParameterName = "@percentualMinimoFrequencia";
			Param.Size = 7;
				Param.Value = entity.percentualMinimoFrequencia;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@tipoProgressaoParcial";
			Param.Size = 1;
				if(entity.tipoProgressaoParcial > 0 )
				{
					Param.Value = entity.tipoProgressaoParcial;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@valorMinimoProgressaoParcialPorDisciplina";
			Param.Size = 10;
				if(!string.IsNullOrEmpty(entity.valorMinimoProgressaoParcialPorDisciplina))
				{
					Param.Value = entity.valorMinimoProgressaoParcialPorDisciplina;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@qtdeMaxDisciplinasProgressaoParcial";
			Param.Size = 1;
				if(entity.qtdeMaxDisciplinasProgressaoParcial > 0 )
				{
					Param.Value = entity.qtdeMaxDisciplinasProgressaoParcial;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@fav_situacao";
			Param.Size = 1;
			Param.Value = entity.fav_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@fav_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.fav_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@fav_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.fav_dataAlteracao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@fav_criterioAprovacaoResultadoFinal";
			Param.Size = 1;
				if(entity.fav_criterioAprovacaoResultadoFinal > 0 )
				{
					Param.Value = entity.fav_criterioAprovacaoResultadoFinal;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@fav_avaliacaoFinalAnalitica";
			Param.Size = 1;
			Param.Value = entity.fav_avaliacaoFinalAnalitica;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Decimal;
			Param.ParameterName = "@fav_variacao";
			Param.Size = 20;
			Param.Value = entity.fav_variacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@fav_sugerirResultadoFinalDisciplina";
			Param.Size = 1;
			Param.Value = entity.fav_sugerirResultadoFinalDisciplina;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Decimal;
			Param.ParameterName = "@fav_percentualMinimoFrequenciaFinalAjustadaDisciplina";
			Param.Size = 7;
				Param.Value = entity.fav_percentualMinimoFrequenciaFinalAjustadaDisciplina;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@fav_exibirBotaoSomaMedia";
			Param.Size = 1;
			Param.Value = entity.fav_exibirBotaoSomaMedia;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@valorMinimoAprovacaoDocente";
			Param.Size = 10;
				if(!string.IsNullOrEmpty(entity.valorMinimoAprovacaoDocente))
				{
					Param.Value = entity.valorMinimoAprovacaoDocente;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Decimal;
			Param.ParameterName = "@percentualBaixaFrequencia";
			Param.Size = 7;
				Param.Value = entity.percentualBaixaFrequencia;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@fav_fechamentoAutomatico";
			Param.Size = 1;
			Param.Value = entity.fav_fechamentoAutomatico;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@fav_permiteRecuperacaoQualquerNota";
			Param.Size = 1;
			Param.Value = entity.fav_permiteRecuperacaoQualquerNota;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@fav_permiteRecuperacaoForaPeriodo";
			Param.Size = 1;
			Param.Value = entity.fav_permiteRecuperacaoForaPeriodo;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@fav_calcularMediaAvaliacaoFinal";
			Param.Size = 1;
			Param.Value = entity.fav_calcularMediaAvaliacaoFinal;
			qs.Parameters.Add(Param);


			}
		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, ACA_FormatoAvaliacao entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@fav_id";
			Param.Size = 4;
			Param.Value = entity.fav_id;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
		/// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		/// <returns>TRUE - Se entity.ParametroId > 0</returns>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_FormatoAvaliacao entity)
		{
			if (entity != null & qs != null)
            {
			entity.fav_id = Convert.ToInt32(qs.Return.Rows[0][0]);
			return (entity.fav_id > 0);
			}

			return false;
		}		
	}
}