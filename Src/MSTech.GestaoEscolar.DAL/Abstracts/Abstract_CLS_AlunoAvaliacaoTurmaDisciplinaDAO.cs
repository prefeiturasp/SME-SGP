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
	/// Classe abstrata de CLS_AlunoAvaliacaoTurmaDisciplina.
	/// </summary>
	public abstract class Abstract_CLS_AlunoAvaliacaoTurmaDisciplinaDAO : Abstract_DAL<CLS_AlunoAvaliacaoTurmaDisciplina>
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
		protected override void ParamCarregar(QuerySelectStoredProcedure qs, CLS_AlunoAvaliacaoTurmaDisciplina entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tud_id";
			Param.Size = 8;
			Param.Value = entity.tud_id;
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
			Param.ParameterName = "@mtd_id";
			Param.Size = 4;
			Param.Value = entity.mtd_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@atd_id";
			Param.Size = 4;
			Param.Value = entity.atd_id;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Configura os parametros do metodo de Inserir.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_AlunoAvaliacaoTurmaDisciplina entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = entity.tud_id;
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
                Param.ParameterName = "@mtd_id";
                Param.Size = 4;
                Param.Value = entity.mtd_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@atd_id";
                Param.Size = 4;
                Param.Value = entity.atd_id;
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
                Param.ParameterName = "@atd_avaliacao";
                Param.Size = 20;
                if (!string.IsNullOrEmpty(entity.atd_avaliacao))
                {
                    Param.Value = entity.atd_avaliacao;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Decimal;
                Param.ParameterName = "@atd_frequencia";
                Param.Size = 7;
                Param.Value = entity.atd_frequencia;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@atd_comentarios";
                Param.Size = 1000;
                if (!string.IsNullOrEmpty(entity.atd_comentarios))
                {
                    Param.Value = entity.atd_comentarios;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@atd_relatorio";
                Param.Size = 2147483647;
                if (!string.IsNullOrEmpty(entity.atd_relatorio))
                {
                    Param.Value = entity.atd_relatorio;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@atd_semProfessor";
                Param.Size = 1;
                Param.Value = entity.atd_semProfessor;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@atd_numeroFaltas";
                Param.Size = 4;
                if (entity.atd_numeroFaltas > 0)
                {
                    Param.Value = entity.atd_numeroFaltas;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@atd_numeroAulas";
                Param.Size = 4;
                if (entity.atd_numeroAulas > 0)
                {
                    Param.Value = entity.atd_numeroAulas;
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
                if (entity.arq_idRelatorio > 0)
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
                Param.ParameterName = "@atd_situacao";
                Param.Size = 1;
                Param.Value = entity.atd_situacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@atd_dataCriacao";
                Param.Size = 16;
                Param.Value = entity.atd_dataCriacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@atd_dataAlteracao";
                Param.Size = 16;
                Param.Value = entity.atd_dataAlteracao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@atd_ausenciasCompensadas";
                Param.Size = 4;
                if (entity.atd_ausenciasCompensadas > 0)
                {
                    Param.Value = entity.atd_ausenciasCompensadas;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@atd_registroexterno";
                Param.Size = 1;
                Param.Value = entity.atd_registroexterno;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@atd_avaliacaoPosConselho";
                Param.Size = 20;
                if (!string.IsNullOrEmpty(entity.atd_avaliacaoPosConselho))
                {
                    Param.Value = entity.atd_avaliacaoPosConselho;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@atd_justificativaPosConselho";
                Param.Size = 2147483647;
                if (!string.IsNullOrEmpty(entity.atd_justificativaPosConselho))
                {
                    Param.Value = entity.atd_justificativaPosConselho;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Decimal;
                Param.ParameterName = "@atd_frequenciaFinalAjustada";
                Param.Size = 7;
                Param.Value = entity.atd_frequenciaFinalAjustada;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@atd_numeroFaltasReposicao";
                Param.Size = 4;
                if (entity.atd_numeroFaltasReposicao > 0)
                {
                    Param.Value = entity.atd_numeroFaltasReposicao;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@atd_numeroAulasReposicao";
                Param.Size = 4;
                if (entity.atd_numeroAulasReposicao > 0)
                {
                    Param.Value = entity.atd_numeroAulasReposicao;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@atd_numeroFaltasExterna";
                Param.Size = 4;
                if (entity.atd_numeroFaltasExterna > 0)
                {
                    Param.Value = entity.atd_numeroFaltasExterna;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@atd_numeroAulasExterna";
                Param.Size = 4;
                if (entity.atd_numeroAulasExterna > 0)
                {
                    Param.Value = entity.atd_numeroAulasExterna;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Decimal;
                Param.ParameterName = "@atd_numeroAtividadeExtraclassePrevista";
                if (entity.atd_numeroAtividadeExtraclassePrevista > 0)
                {
                    Param.Value = entity.atd_numeroAtividadeExtraclassePrevista;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Decimal;
                Param.ParameterName = "@atd_numeroAtividadeExtraclasseEntregue";
                if (entity.atd_numeroAtividadeExtraclasseEntregue > 0)
                {
                    Param.Value = entity.atd_numeroAtividadeExtraclasseEntregue;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);
            }
        }
		
		/// <summary>
		/// Configura os parametros do metodo de Alterar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamAlterar(QueryStoredProcedure qs, CLS_AlunoAvaliacaoTurmaDisciplina entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = entity.tud_id;
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
                Param.ParameterName = "@mtd_id";
                Param.Size = 4;
                Param.Value = entity.mtd_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@atd_id";
                Param.Size = 4;
                Param.Value = entity.atd_id;
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
                Param.ParameterName = "@atd_avaliacao";
                Param.Size = 20;
                if (!string.IsNullOrEmpty(entity.atd_avaliacao))
                {
                    Param.Value = entity.atd_avaliacao;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Decimal;
                Param.ParameterName = "@atd_frequencia";
                Param.Size = 7;
                Param.Value = entity.atd_frequencia;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@atd_comentarios";
                Param.Size = 1000;
                if (!string.IsNullOrEmpty(entity.atd_comentarios))
                {
                    Param.Value = entity.atd_comentarios;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@atd_relatorio";
                Param.Size = 2147483647;
                if (!string.IsNullOrEmpty(entity.atd_relatorio))
                {
                    Param.Value = entity.atd_relatorio;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@atd_semProfessor";
                Param.Size = 1;
                Param.Value = entity.atd_semProfessor;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@atd_numeroFaltas";
                Param.Size = 4;
                if (entity.atd_numeroFaltas > 0)
                {
                    Param.Value = entity.atd_numeroFaltas;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@atd_numeroAulas";
                Param.Size = 4;
                if (entity.atd_numeroAulas > 0)
                {
                    Param.Value = entity.atd_numeroAulas;
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
                if (entity.arq_idRelatorio > 0)
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
                Param.ParameterName = "@atd_situacao";
                Param.Size = 1;
                Param.Value = entity.atd_situacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@atd_dataCriacao";
                Param.Size = 16;
                Param.Value = entity.atd_dataCriacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@atd_dataAlteracao";
                Param.Size = 16;
                Param.Value = entity.atd_dataAlteracao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@atd_ausenciasCompensadas";
                Param.Size = 4;
                if (entity.atd_ausenciasCompensadas > 0)
                {
                    Param.Value = entity.atd_ausenciasCompensadas;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@atd_registroexterno";
                Param.Size = 1;
                Param.Value = entity.atd_registroexterno;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@atd_avaliacaoPosConselho";
                Param.Size = 20;
                if (!string.IsNullOrEmpty(entity.atd_avaliacaoPosConselho))
                {
                    Param.Value = entity.atd_avaliacaoPosConselho;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@atd_justificativaPosConselho";
                Param.Size = 2147483647;
                if (!string.IsNullOrEmpty(entity.atd_justificativaPosConselho))
                {
                    Param.Value = entity.atd_justificativaPosConselho;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Decimal;
                Param.ParameterName = "@atd_frequenciaFinalAjustada";
                Param.Size = 7;
                Param.Value = entity.atd_frequenciaFinalAjustada;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@atd_numeroFaltasReposicao";
                Param.Size = 4;
                if (entity.atd_numeroFaltasReposicao > 0)
                {
                    Param.Value = entity.atd_numeroFaltasReposicao;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@atd_numeroAulasReposicao";
                Param.Size = 4;
                if (entity.atd_numeroAulasReposicao > 0)
                {
                    Param.Value = entity.atd_numeroAulasReposicao;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@atd_numeroFaltasExterna";
                Param.Size = 4;
                if (entity.atd_numeroFaltasExterna > 0)
                {
                    Param.Value = entity.atd_numeroFaltasExterna;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@atd_numeroAulasExterna";
                Param.Size = 4;
                if (entity.atd_numeroAulasExterna > 0)
                {
                    Param.Value = entity.atd_numeroAulasExterna;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Decimal;
                Param.ParameterName = "@atd_numeroAtividadeExtraclassePrevista";
                if (entity.atd_numeroAtividadeExtraclassePrevista > 0)
                {
                    Param.Value = entity.atd_numeroAtividadeExtraclassePrevista;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Decimal;
                Param.ParameterName = "@atd_numeroAtividadeExtraclasseEntregue";
                if (entity.atd_numeroAtividadeExtraclasseEntregue > 0)
                {
                    Param.Value = entity.atd_numeroAtividadeExtraclasseEntregue;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);
            }
        }

		/// <summary>
		/// Configura os parametros do metodo de Deletar.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		protected override void ParamDeletar(QueryStoredProcedure qs, CLS_AlunoAvaliacaoTurmaDisciplina entity)
		{
			if (entity != null & qs != null)
            {
			Param = qs.NewParameter();
			Param.DbType = DbType.Int64;
			Param.ParameterName = "@tud_id";
			Param.Size = 8;
			Param.Value = entity.tud_id;
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
			Param.ParameterName = "@mtd_id";
			Param.Size = 4;
			Param.Value = entity.mtd_id;
			qs.Parameters.Add(Param);

			Param = qs.NewParameter();
			Param.DbType = DbType.Int32;
			Param.ParameterName = "@atd_id";
			Param.Size = 4;
			Param.Value = entity.atd_id;
			qs.Parameters.Add(Param);


			}
		}
		
		/// <summary>
		/// Recebe o valor do auto incremento e coloca na propriedade.
		/// </summary>
		/// <param name="qs">Objeto da Store Procedure.</param>
		/// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
		/// <returns>TRUE - Se entity.ParametroId > 0</returns>
		protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CLS_AlunoAvaliacaoTurmaDisciplina entity)
		{
			if (entity != null & qs != null)
            {

			}

			return false;
		}	
	}
}