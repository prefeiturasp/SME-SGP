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
	/// Classe abstrata de CLS_AlunoAvaliacaoTurma.
	/// </summary>
	public abstract class Abstract_CLS_AlunoAvaliacaoTurmaDAO : Abstract_DAL<CLS_AlunoAvaliacaoTurma>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, CLS_AlunoAvaliacaoTurma entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tur_id";
			Param.Size = 8;
			Param.Value = entity.tur_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mtu_id";
			Param.Size = 4;
			Param.Value = entity.mtu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@aat_id";
			Param.Size = 4;
			Param.Value = entity.aat_id;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_AlunoAvaliacaoTurma entity)
		{
			if (entity != null & qs != null)
            {
							Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tur_id";
			Param.Size = 8;
			Param.Value = entity.tur_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mtu_id";
			Param.Size = 4;
			Param.Value = entity.mtu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@aat_id";
			Param.Size = 4;
			Param.Value = entity.aat_id;
			qs.Parameters.Add(Param);

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
			Param.ParameterName = "@aat_avaliacao";
			Param.Size = 20;
				if(!string.IsNullOrEmpty(entity.aat_avaliacao))
				{
					Param.Value = entity.aat_avaliacao;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Decimal;
			Param.ParameterName = "@aat_frequencia";
			Param.Size = 7;
				Param.Value = entity.aat_frequencia;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@aat_comentarios";
			Param.Size = 1000;
				if(!string.IsNullOrEmpty(entity.aat_comentarios))
				{
					Param.Value = entity.aat_comentarios;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@aat_relatorio";
			Param.Size = 2147483647;
				if(!string.IsNullOrEmpty(entity.aat_relatorio))
				{
					Param.Value = entity.aat_relatorio;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@aat_semProfessor";
			Param.Size = 1;
				Param.Value = entity.aat_semProfessor;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@aat_numeroFaltas";
			Param.Size = 4;
				if(entity.aat_numeroFaltas > 0 )
				{
					Param.Value = entity.aat_numeroFaltas;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@aat_numeroAulas";
			Param.Size = 4;
				if(entity.aat_numeroAulas > 0 )
				{
					Param.Value = entity.aat_numeroAulas;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@arq_idRelatorio";
			Param.Size = 8;
				if(entity.arq_idRelatorio > 0 )
				{
					Param.Value = entity.arq_idRelatorio;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@aat_situacao";
			Param.Size = 1;
			Param.Value = entity.aat_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@aat_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.aat_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@aat_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.aat_dataAlteracao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@aat_ausenciasCompensadas";
			Param.Size = 4;
				if(entity.aat_ausenciasCompensadas > 0 )
				{
					Param.Value = entity.aat_ausenciasCompensadas;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@aat_avaliacaoAdicional";
			Param.Size = 20;
				if(!string.IsNullOrEmpty(entity.aat_avaliacaoAdicional))
				{
					Param.Value = entity.aat_avaliacaoAdicional;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Decimal;
			Param.ParameterName = "@aat_frequenciaAcumulada";
			Param.Size = 7;
				Param.Value = entity.aat_frequenciaAcumulada;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@aat_faltoso";
			Param.Size = 1;
			Param.Value = entity.aat_faltoso;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@aat_registroexterno";
			Param.Size = 1;
			Param.Value = entity.aat_registroexterno;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Decimal;
			Param.ParameterName = "@aat_frequenciaAcumuladaCalculada";
			Param.Size = 29;
				Param.Value = entity.aat_frequenciaAcumuladaCalculada;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@aat_naoAvaliado";
			Param.Size = 1;
				Param.Value = entity.aat_naoAvaliado;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@aat_avaliacaoPosConselho";
			Param.Size = 20;
				if(!string.IsNullOrEmpty(entity.aat_avaliacaoPosConselho))
				{
					Param.Value = entity.aat_avaliacaoPosConselho;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@aat_justificativaPosConselho";
			Param.Size = 2147483647;
				if(!string.IsNullOrEmpty(entity.aat_justificativaPosConselho))
				{
					Param.Value = entity.aat_justificativaPosConselho;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Decimal;
			Param.ParameterName = "@aat_frequenciaFinalAjustada";
			Param.Size = 7;
				Param.Value = entity.aat_frequenciaFinalAjustada;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamAlterar(QueryStoredProcedure qs, CLS_AlunoAvaliacaoTurma entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tur_id";
			Param.Size = 8;
			Param.Value = entity.tur_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mtu_id";
			Param.Size = 4;
			Param.Value = entity.mtu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@aat_id";
			Param.Size = 4;
			Param.Value = entity.aat_id;
			qs.Parameters.Add(Param);

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
			Param.ParameterName = "@aat_avaliacao";
			Param.Size = 20;
				if(!string.IsNullOrEmpty(entity.aat_avaliacao))
				{
					Param.Value = entity.aat_avaliacao;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Decimal;
			Param.ParameterName = "@aat_frequencia";
			Param.Size = 7;
				Param.Value = entity.aat_frequencia;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@aat_comentarios";
			Param.Size = 1000;
				if(!string.IsNullOrEmpty(entity.aat_comentarios))
				{
					Param.Value = entity.aat_comentarios;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@aat_relatorio";
			Param.Size = 2147483647;
				if(!string.IsNullOrEmpty(entity.aat_relatorio))
				{
					Param.Value = entity.aat_relatorio;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@aat_semProfessor";
			Param.Size = 1;
				Param.Value = entity.aat_semProfessor;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@aat_numeroFaltas";
			Param.Size = 4;
				if(entity.aat_numeroFaltas > 0 )
				{
					Param.Value = entity.aat_numeroFaltas;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@aat_numeroAulas";
			Param.Size = 4;
				if(entity.aat_numeroAulas > 0 )
				{
					Param.Value = entity.aat_numeroAulas;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@arq_idRelatorio";
			Param.Size = 8;
				if(entity.arq_idRelatorio > 0 )
				{
					Param.Value = entity.arq_idRelatorio;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Byte;
			Param.ParameterName = "@aat_situacao";
			Param.Size = 1;
			Param.Value = entity.aat_situacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@aat_dataCriacao";
			Param.Size = 16;
			Param.Value = entity.aat_dataCriacao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.DateTime;
			Param.ParameterName = "@aat_dataAlteracao";
			Param.Size = 16;
			Param.Value = entity.aat_dataAlteracao;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@aat_ausenciasCompensadas";
			Param.Size = 4;
				if(entity.aat_ausenciasCompensadas > 0 )
				{
					Param.Value = entity.aat_ausenciasCompensadas;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@aat_avaliacaoAdicional";
			Param.Size = 20;
				if(!string.IsNullOrEmpty(entity.aat_avaliacaoAdicional))
				{
					Param.Value = entity.aat_avaliacaoAdicional;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Decimal;
			Param.ParameterName = "@aat_frequenciaAcumulada";
			Param.Size = 7;
				Param.Value = entity.aat_frequenciaAcumulada;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@aat_faltoso";
			Param.Size = 1;
			Param.Value = entity.aat_faltoso;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@aat_registroexterno";
			Param.Size = 1;
			Param.Value = entity.aat_registroexterno;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Decimal;
			Param.ParameterName = "@aat_frequenciaAcumuladaCalculada";
			Param.Size = 29;
				Param.Value = entity.aat_frequenciaAcumuladaCalculada;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Boolean;
			Param.ParameterName = "@aat_naoAvaliado";
			Param.Size = 1;
				Param.Value = entity.aat_naoAvaliado;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@aat_avaliacaoPosConselho";
			Param.Size = 20;
				if(!string.IsNullOrEmpty(entity.aat_avaliacaoPosConselho))
				{
					Param.Value = entity.aat_avaliacaoPosConselho;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.AnsiString;
			Param.ParameterName = "@aat_justificativaPosConselho";
			Param.Size = 2147483647;
				if(!string.IsNullOrEmpty(entity.aat_justificativaPosConselho))
				{
					Param.Value = entity.aat_justificativaPosConselho;
				}
				else
				{
					Param.Value = DBNull.Value;
				}
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Decimal;
			Param.ParameterName = "@aat_frequenciaFinalAjustada";
			Param.Size = 7;
				Param.Value = entity.aat_frequenciaFinalAjustada;
			qs.Parameters.Add(Param);


			}
		}

		/// <summary>
		/// Configura os parametros do metodo de Deletar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, CLS_AlunoAvaliacaoTurma entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tur_id";
			Param.Size = 8;
			Param.Value = entity.tur_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@alu_id";
			Param.Size = 8;
			Param.Value = entity.alu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@mtu_id";
			Param.Size = 4;
			Param.Value = entity.mtu_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@aat_id";
			Param.Size = 4;
			Param.Value = entity.aat_id;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
		/// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		/// <returns>TRUE - Se entity.ParametroId > 0</returns>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CLS_AlunoAvaliacaoTurma entity)
		{
			if (entity != null & qs != null)
            {

			}

			return false;
		}		
	}
}