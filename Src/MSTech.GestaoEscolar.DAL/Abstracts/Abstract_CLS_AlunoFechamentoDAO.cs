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
    /// Classe abstrata de CLS_AlunoFechamento.
    /// </summary>
    public abstract class Abstract_CLS_AlunoFechamentoDAO : Abstract_DAL<CLS_AlunoFechamento>
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
        protected override void ParamCarregar(QuerySelectStoredProcedure qs, CLS_AlunoFechamento entity)
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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                Param.Value = entity.tpc_id;
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


            }
        }

        /// <summary>
        /// Configura os parametros do metodo de Inserir.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_AlunoFechamento entity)
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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                Param.Value = entity.tpc_id;
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
                Param.ParameterName = "@caf_qtFaltas";
                Param.Size = 4;
                if (entity.caf_qtFaltas > 0)
                {
                    Param.Value = entity.caf_qtFaltas;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@caf_qtAulas";
                Param.Size = 4;
                if (entity.caf_qtAulas > 0)
                {
                    Param.Value = entity.caf_qtAulas;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@caf_qtFaltasReposicao";
                Param.Size = 4;
                if (entity.caf_qtFaltasReposicao > 0)
                {
                    Param.Value = entity.caf_qtFaltasReposicao;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@caf_qtAulasReposicao";
                Param.Size = 4;
                if (entity.caf_qtAulasReposicao > 0)
                {
                    Param.Value = entity.caf_qtAulasReposicao;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@caf_qtAusenciasCompensadas";
                Param.Size = 4;
                if (entity.caf_qtAusenciasCompensadas > 0)
                {
                    Param.Value = entity.caf_qtAusenciasCompensadas;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Decimal;
                Param.ParameterName = "@caf_frequencia";
                Param.Size = 7;
                Param.Value = entity.caf_frequencia;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Decimal;
                Param.ParameterName = "@caf_frequenciaFinalAjustada";
                Param.Size = 7;
                Param.Value = entity.caf_frequenciaFinalAjustada;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@caf_avaliacao";
                Param.Size = 20;
                if (!string.IsNullOrEmpty(entity.caf_avaliacao))
                {
                    Param.Value = entity.caf_avaliacao;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@caf_efetivado";
                Param.Size = 1;
                Param.Value = entity.caf_efetivado;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@caf_dataAlteracao";
                Param.Size = 16;
                Param.Value = entity.caf_dataAlteracao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@caf_qtFaltasExterna";
                Param.Size = 4;
                if (entity.caf_qtFaltasExterna > 0)
                {
                    Param.Value = entity.caf_qtFaltasExterna;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@caf_qtAulasExterna";
                Param.Size = 4;
                if (entity.caf_qtAulasExterna > 0)
                {
                    Param.Value = entity.caf_qtAulasExterna;
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
        protected override void ParamAlterar(QueryStoredProcedure qs, CLS_AlunoFechamento entity)
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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                Param.Value = entity.tpc_id;
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
                Param.ParameterName = "@caf_qtFaltas";
                Param.Size = 4;
                if (entity.caf_qtFaltas > 0)
                {
                    Param.Value = entity.caf_qtFaltas;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@caf_qtAulas";
                Param.Size = 4;
                if (entity.caf_qtAulas > 0)
                {
                    Param.Value = entity.caf_qtAulas;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@caf_qtFaltasReposicao";
                Param.Size = 4;
                if (entity.caf_qtFaltasReposicao > 0)
                {
                    Param.Value = entity.caf_qtFaltasReposicao;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@caf_qtAulasReposicao";
                Param.Size = 4;
                if (entity.caf_qtAulasReposicao > 0)
                {
                    Param.Value = entity.caf_qtAulasReposicao;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@caf_qtAusenciasCompensadas";
                Param.Size = 4;
                if (entity.caf_qtAusenciasCompensadas > 0)
                {
                    Param.Value = entity.caf_qtAusenciasCompensadas;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Decimal;
                Param.ParameterName = "@caf_frequencia";
                Param.Size = 7;
                Param.Value = entity.caf_frequencia;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Decimal;
                Param.ParameterName = "@caf_frequenciaFinalAjustada";
                Param.Size = 7;
                Param.Value = entity.caf_frequenciaFinalAjustada;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@caf_avaliacao";
                Param.Size = 20;
                if (!string.IsNullOrEmpty(entity.caf_avaliacao))
                {
                    Param.Value = entity.caf_avaliacao;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@caf_efetivado";
                Param.Size = 1;
                Param.Value = entity.caf_efetivado;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@caf_dataAlteracao";
                Param.Size = 16;
                Param.Value = entity.caf_dataAlteracao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@caf_qtFaltasExterna";
                Param.Size = 4;
                if (entity.caf_qtFaltasExterna > 0)
                {
                    Param.Value = entity.caf_qtFaltasExterna;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@caf_qtAulasExterna";
                Param.Size = 4;
                if (entity.caf_qtAulasExterna > 0)
                {
                    Param.Value = entity.caf_qtAulasExterna;
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
        protected override void ParamDeletar(QueryStoredProcedure qs, CLS_AlunoFechamento entity)
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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                Param.Value = entity.tpc_id;
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


            }
        }

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        /// <returns>TRUE - Se entity.ParametroId > 0</returns>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CLS_AlunoFechamento entity)
        {
            if (entity != null & qs != null)
            {
                return true;
            }

            return false;
        }
    }
}