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
    /// Classe abstrata de ESC_UnidadeEscola.
    /// </summary>
    public abstract class Abstract_ESC_UnidadeEscolaDAO : Abstract_DAL<ESC_UnidadeEscola>
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
        protected override void ParamCarregar(QuerySelectStoredProcedure qs, ESC_UnidadeEscola entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                Param.Value = entity.esc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@uni_id";
                Param.Size = 4;
                Param.Value = entity.uni_id;
                qs.Parameters.Add(Param);


            }
        }

        /// <summary>
        /// Configura os parametros do metodo de Inserir.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ESC_UnidadeEscola entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                Param.Value = entity.esc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@uni_id";
                Param.Size = 4;
                Param.Value = entity.uni_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@uni_codigo";
                Param.Size = 20;
                if (!string.IsNullOrEmpty(entity.uni_codigo))
                {
                    Param.Value = entity.uni_codigo;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@uni_descricao";
                Param.Size = 1000;
                if (!string.IsNullOrEmpty(entity.uni_descricao))
                {
                    Param.Value = entity.uni_descricao;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@uni_principal";
                Param.Size = 1;
                Param.Value = entity.uni_principal;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@uni_zona";
                Param.Size = 1;
                if (entity.uni_zona > 0)
                {
                    Param.Value = entity.uni_zona;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime2;
                Param.ParameterName = "@uni_funcionamentoInicio";
                Param.Size = 20;
                if (entity.uni_funcionamentoInicio != new DateTime())
                {
                    Param.Value = entity.uni_funcionamentoInicio;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime2;
                Param.ParameterName = "@uni_funcionamentoFim";
                Param.Size = 20;
                if (entity.uni_funcionamentoFim != new DateTime())
                {
                    Param.Value = entity.uni_funcionamentoFim;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@uni_cepsProximos";
                Param.Size = 2147483647;
                if (!string.IsNullOrEmpty(entity.uni_cepsProximos))
                {
                    Param.Value = entity.uni_cepsProximos;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@uni_observacao";
                Param.Size = 2147483647;
                if (!string.IsNullOrEmpty(entity.uni_observacao))
                {
                    Param.Value = entity.uni_observacao;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@uni_situacao";
                Param.Size = 1;
                Param.Value = entity.uni_situacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@uni_dataCriacao";
                Param.Size = 16;
                Param.Value = entity.uni_dataCriacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@uni_dataAlteracao";
                Param.Size = 16;
                Param.Value = entity.uni_dataAlteracao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@uni_alimentacaoEscolar";
                Param.Size = 1;
                Param.Value = entity.uni_alimentacaoEscolar;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@uni_propostaFormacaoAlternancia";
                Param.Size = 1;
                Param.Value = entity.uni_propostaFormacaoAlternancia;
                qs.Parameters.Add(Param);


            }
        }

        /// <summary>
        /// Configura os parametros do metodo de Alterar.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamAlterar(QueryStoredProcedure qs, ESC_UnidadeEscola entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                Param.Value = entity.esc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@uni_id";
                Param.Size = 4;
                Param.Value = entity.uni_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@uni_codigo";
                Param.Size = 20;
                if (!string.IsNullOrEmpty(entity.uni_codigo))
                {
                    Param.Value = entity.uni_codigo;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@uni_descricao";
                Param.Size = 1000;
                if (!string.IsNullOrEmpty(entity.uni_descricao))
                {
                    Param.Value = entity.uni_descricao;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@uni_principal";
                Param.Size = 1;
                Param.Value = entity.uni_principal;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@uni_zona";
                Param.Size = 1;
                if (entity.uni_zona > 0)
                {
                    Param.Value = entity.uni_zona;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime2;
                Param.ParameterName = "@uni_funcionamentoInicio";
                Param.Size = 20;
                if (entity.uni_funcionamentoInicio != new DateTime())
                {
                    Param.Value = entity.uni_funcionamentoInicio;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime2;
                Param.ParameterName = "@uni_funcionamentoFim";
                Param.Size = 20;
                if (entity.uni_funcionamentoFim != new DateTime())
                {
                    Param.Value = entity.uni_funcionamentoFim;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@uni_cepsProximos";
                Param.Size = 2147483647;
                if (!string.IsNullOrEmpty(entity.uni_cepsProximos))
                {
                    Param.Value = entity.uni_cepsProximos;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@uni_observacao";
                Param.Size = 2147483647;
                if (!string.IsNullOrEmpty(entity.uni_observacao))
                {
                    Param.Value = entity.uni_observacao;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@uni_situacao";
                Param.Size = 1;
                Param.Value = entity.uni_situacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@uni_dataCriacao";
                Param.Size = 16;
                Param.Value = entity.uni_dataCriacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@uni_dataAlteracao";
                Param.Size = 16;
                Param.Value = entity.uni_dataAlteracao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@uni_alimentacaoEscolar";
                Param.Size = 1;
                Param.Value = entity.uni_alimentacaoEscolar;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@uni_propostaFormacaoAlternancia";
                Param.Size = 1;
                Param.Value = entity.uni_propostaFormacaoAlternancia;
                qs.Parameters.Add(Param);


            }
        }

        /// <summary>
        /// Configura os parametros do metodo de Deletar.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamDeletar(QueryStoredProcedure qs, ESC_UnidadeEscola entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                Param.Value = entity.esc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@uni_id";
                Param.Size = 4;
                Param.Value = entity.uni_id;
                qs.Parameters.Add(Param);


            }
        }

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        /// <returns>TRUE - Se entity.ParametroId > 0</returns>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ESC_UnidadeEscola entity)
        {
            entity.uni_id = Convert.ToInt32(qs.Return.Rows[0][0]);
            return (entity.uni_id > 0);
        }
    }
}