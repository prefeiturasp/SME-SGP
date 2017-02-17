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
    /// Classe abstrata de ACA_AlunoHistorico.
    /// </summary>
    public abstract class AbstractACA_AlunoHistoricoDAO : Abstract_DAL<ACA_AlunoHistorico>
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
        protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_AlunoHistorico entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = entity.alu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@alh_id";
                Param.Size = 4;
                Param.Value = entity.alh_id;
                qs.Parameters.Add(Param);
            }
        }

        /// <summary>
        /// Configura os parametros do metodo de Inserir.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_AlunoHistorico entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = entity.alu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@alh_id";
                Param.Size = 4;
                Param.Value = entity.alh_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cur_id";
                Param.Size = 4;
                if (entity.cur_id > 0)
                {
                    Param.Value = entity.cur_id;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crr_id";
                Param.Size = 4;
                if (entity.crr_id > 0)
                {
                    Param.Value = entity.crr_id;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                if (entity.crp_id > 0)
                {
                    Param.Value = entity.crp_id;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                if (entity.esc_id > 0)
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
                if (entity.uni_id > 0)
                {
                    Param.Value = entity.uni_id;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@mtu_id";
                Param.Size = 4;
                if (entity.mtu_id > 0)
                {
                    Param.Value = entity.mtu_id;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@aho_id";
                Param.Size = 4;
                if (entity.aho_id > 0)
                {
                    Param.Value = entity.aho_id;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@eco_id";
                Param.Size = 8;
                if (entity.eco_id > 0)
                {
                    Param.Value = entity.eco_id;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@alh_anoLetivo";
                Param.Size = 4;
                Param.Value = entity.alh_anoLetivo;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@alh_resultado";
                Param.Size = 1;
                if (entity.alh_resultado > 0)
                {
                    Param.Value = entity.alh_resultado;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alh_resultadoDescricao";
                Param.Size = 30;
                if (!string.IsNullOrEmpty(entity.alh_resultadoDescricao))
                {
                    Param.Value = entity.alh_resultadoDescricao;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alh_avaliacao";
                Param.Size = 100;
                if (!string.IsNullOrEmpty(entity.alh_avaliacao))
                {
                    Param.Value = entity.alh_avaliacao;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alh_frequencia";
                Param.Size = 100;
                if (!string.IsNullOrEmpty(entity.alh_frequencia))
                {
                    Param.Value = entity.alh_frequencia;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@alh_qtdeFaltas";
                Param.Size = 4;
                if (entity.alh_qtdeFaltas > 0)
                {
                    Param.Value = entity.alh_qtdeFaltas;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@alh_tipoControleNotas";
                Param.Size = 1;
                if (entity.alh_tipoControleNotas > 0)
                {
                    Param.Value = entity.alh_tipoControleNotas;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@alh_cargaHorariaBaseNacional";
                Param.Size = 4;
                if (entity.alh_cargaHorariaBaseNacional > 0)
                {
                    Param.Value = entity.alh_cargaHorariaBaseNacional;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@alh_cargaHorariaBaseDiversificada";
                Param.Size = 4;
                if (entity.alh_cargaHorariaBaseDiversificada > 0)
                {
                    Param.Value = entity.alh_cargaHorariaBaseDiversificada;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alh_descricaoProximoPeriodo";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(entity.alh_descricaoProximoPeriodo))
                {
                    Param.Value = entity.alh_descricaoProximoPeriodo;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@alh_situacao";
                Param.Size = 1;
                Param.Value = entity.alh_situacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@alh_dataCriacao";
                Param.Size = 16;
                Param.Value = entity.alh_dataCriacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@alh_dataAlteracao";
                Param.Size = 16;
                Param.Value = entity.alh_dataAlteracao;
                qs.Parameters.Add(Param);
            }
        }

        /// <summary>
        /// Configura os parametros do metodo de Alterar.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_AlunoHistorico entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = entity.alu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@alh_id";
                Param.Size = 4;
                Param.Value = entity.alh_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cur_id";
                Param.Size = 4;
                if (entity.cur_id > 0)
                {
                    Param.Value = entity.cur_id;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crr_id";
                Param.Size = 4;
                if (entity.crr_id > 0)
                {
                    Param.Value = entity.crr_id;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                if (entity.crp_id > 0)
                {
                    Param.Value = entity.crp_id;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                if (entity.esc_id > 0)
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
                if (entity.uni_id > 0)
                {
                    Param.Value = entity.uni_id;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@mtu_id";
                Param.Size = 4;
                if (entity.mtu_id > 0)
                {
                    Param.Value = entity.mtu_id;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@aho_id";
                Param.Size = 4;
                if (entity.aho_id > 0)
                {
                    Param.Value = entity.aho_id;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@eco_id";
                Param.Size = 8;
                if (entity.eco_id > 0)
                {
                    Param.Value = entity.eco_id;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@alh_anoLetivo";
                Param.Size = 4;
                Param.Value = entity.alh_anoLetivo;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@alh_resultado";
                Param.Size = 1;
                if (entity.alh_resultado > 0)
                {
                    Param.Value = entity.alh_resultado;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alh_resultadoDescricao";
                Param.Size = 30;
                if (!string.IsNullOrEmpty(entity.alh_resultadoDescricao))
                {
                    Param.Value = entity.alh_resultadoDescricao;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alh_avaliacao";
                Param.Size = 100;
                if (!string.IsNullOrEmpty(entity.alh_avaliacao))
                {
                    Param.Value = entity.alh_avaliacao;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alh_frequencia";
                Param.Size = 100;
                if (!string.IsNullOrEmpty(entity.alh_frequencia))
                {
                    Param.Value = entity.alh_frequencia;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@alh_qtdeFaltas";
                Param.Size = 4;
                if (entity.alh_qtdeFaltas > 0)
                {
                    Param.Value = entity.alh_qtdeFaltas;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@alh_tipoControleNotas";
                Param.Size = 1;
                if (entity.alh_tipoControleNotas > 0)
                {
                    Param.Value = entity.alh_tipoControleNotas;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@alh_cargaHorariaBaseNacional";
                Param.Size = 4;
                if (entity.alh_cargaHorariaBaseNacional > 0)
                {
                    Param.Value = entity.alh_cargaHorariaBaseNacional;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@alh_cargaHorariaBaseDiversificada";
                Param.Size = 4;
                if (entity.alh_cargaHorariaBaseDiversificada > 0)
                {
                    Param.Value = entity.alh_cargaHorariaBaseDiversificada;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alh_descricaoProximoPeriodo";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(entity.alh_descricaoProximoPeriodo))
                {
                    Param.Value = entity.alh_descricaoProximoPeriodo;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@alh_situacao";
                Param.Size = 1;
                Param.Value = entity.alh_situacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@alh_dataCriacao";
                Param.Size = 16;
                Param.Value = entity.alh_dataCriacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@alh_dataAlteracao";
                Param.Size = 16;
                Param.Value = entity.alh_dataAlteracao;
                qs.Parameters.Add(Param);
            }
        }

        /// <summary>
        /// Configura os parametros do metodo de Deletar.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamDeletar(QueryStoredProcedure qs, ACA_AlunoHistorico entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = entity.alu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@alh_id";
                Param.Size = 4;
                Param.Value = entity.alh_id;
                qs.Parameters.Add(Param);
            }
        }

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        /// <returns>TRUE - Se entity.ParametroId > 0</returns>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_AlunoHistorico entity)
        {
            if (entity != null & qs != null)
            {
            }

            return false;
        }
    }
}