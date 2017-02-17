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
    /// Classe abstrata de CLS_TurmaAulaRegencia.
    /// </summary>
    public abstract class AbstractCLS_TurmaAulaRegenciaDAO : Abstract_DAL<CLS_TurmaAulaRegencia>
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
        protected override void ParamCarregar(QuerySelectStoredProcedure qs, CLS_TurmaAulaRegencia entity)
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
                Param.ParameterName = "@tau_id";
                Param.Size = 4;
                Param.Value = entity.tau_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_idFilho";
                Param.Size = 8;
                Param.Value = entity.tud_idFilho;
                qs.Parameters.Add(Param);


            }
        }

        /// <summary>
        /// Configura os parametros do metodo de Inserir.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_TurmaAulaRegencia entity)
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
                Param.ParameterName = "@tau_id";
                Param.Size = 4;
                Param.Value = entity.tau_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_idFilho";
                Param.Size = 8;
                Param.Value = entity.tud_idFilho;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime2;
                Param.ParameterName = "@tuf_data";
                Param.Size = 20;
                if (entity.tuf_data != new DateTime())
                {
                    Param.Value = entity.tuf_data;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tuf_numeroAulas";
                Param.Size = 4;
                if (entity.tuf_numeroAulas > 0)
                {
                    Param.Value = entity.tuf_numeroAulas;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Object;
                Param.ParameterName = "@tuf_planoAula";
                Param.Size = 2147483647;
                if (!string.IsNullOrEmpty(entity.tuf_planoAula))
                {
                    Param.Value = entity.tuf_planoAula;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Object;
                Param.ParameterName = "@tuf_diarioClasse";
                Param.Size = 2147483647;
                if (!string.IsNullOrEmpty(entity.tuf_diarioClasse))
                {
                    Param.Value = entity.tuf_diarioClasse;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tuf_situacao";
                Param.Size = 1;
                Param.Value = entity.tuf_situacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@tuf_dataCriacao";
                Param.Size = 16;
                Param.Value = entity.tuf_dataCriacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@tuf_dataAlteracao";
                Param.Size = 16;
                Param.Value = entity.tuf_dataAlteracao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Object;
                Param.ParameterName = "@tuf_conteudo";
                Param.Size = 2147483647;
                if (!string.IsNullOrEmpty(entity.tuf_conteudo))
                {
                    Param.Value = entity.tuf_conteudo;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tuf_atividadeCasa";
                Param.Size = 2147483646;
                if (!string.IsNullOrEmpty(entity.tuf_atividadeCasa))
                {
                    Param.Value = entity.tuf_atividadeCasa;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@pro_id";
                Param.Size = 16;
                Param.Value = entity.pro_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tuf_sintese";
                Param.Size = 2147483646;
                if (!string.IsNullOrEmpty(entity.tuf_sintese))
                {
                    Param.Value = entity.tuf_sintese;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@tuf_checadoAtividadeCasa";
                Param.Size = 1;
                Param.Value = entity.tuf_checadoAtividadeCasa;
                qs.Parameters.Add(Param);
            }
        }

        /// <summary>
        /// Configura os parametros do metodo de Alterar.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamAlterar(QueryStoredProcedure qs, CLS_TurmaAulaRegencia entity)
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
                Param.ParameterName = "@tau_id";
                Param.Size = 4;
                Param.Value = entity.tau_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_idFilho";
                Param.Size = 8;
                Param.Value = entity.tud_idFilho;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime2;
                Param.ParameterName = "@tuf_data";
                Param.Size = 20;
                if (entity.tuf_data != new DateTime())
                {
                    Param.Value = entity.tuf_data;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tuf_numeroAulas";
                Param.Size = 4;
                if (entity.tuf_numeroAulas > 0)
                {
                    Param.Value = entity.tuf_numeroAulas;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Object;
                Param.ParameterName = "@tuf_planoAula";
                Param.Size = 2147483647;
                if (!string.IsNullOrEmpty(entity.tuf_planoAula))
                {
                    Param.Value = entity.tuf_planoAula;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Object;
                Param.ParameterName = "@tuf_diarioClasse";
                Param.Size = 2147483647;
                if (!string.IsNullOrEmpty(entity.tuf_diarioClasse))
                {
                    Param.Value = entity.tuf_diarioClasse;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tuf_situacao";
                Param.Size = 1;
                Param.Value = entity.tuf_situacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@tuf_dataCriacao";
                Param.Size = 16;
                Param.Value = entity.tuf_dataCriacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@tuf_dataAlteracao";
                Param.Size = 16;
                Param.Value = entity.tuf_dataAlteracao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Object;
                Param.ParameterName = "@tuf_conteudo";
                Param.Size = 2147483647;
                if (!string.IsNullOrEmpty(entity.tuf_conteudo))
                {
                    Param.Value = entity.tuf_conteudo;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tuf_atividadeCasa";
                Param.Size = 2147483646;
                if (!string.IsNullOrEmpty(entity.tuf_atividadeCasa))
                {
                    Param.Value = entity.tuf_atividadeCasa;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@pro_id";
                Param.Size = 16;
                Param.Value = entity.pro_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tuf_sintese";
                Param.Size = 2147483646;
                if (!string.IsNullOrEmpty(entity.tuf_sintese))
                {
                    Param.Value = entity.tuf_sintese;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@tuf_checadoAtividadeCasa";
                Param.Size = 1;
                Param.Value = entity.tuf_checadoAtividadeCasa;
                qs.Parameters.Add(Param);
            }
        }

        /// <summary>
        /// Configura os parametros do metodo de Deletar.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamDeletar(QueryStoredProcedure qs, CLS_TurmaAulaRegencia entity)
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
                Param.ParameterName = "@tau_id";
                Param.Size = 4;
                Param.Value = entity.tau_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_idFilho";
                Param.Size = 8;
                Param.Value = entity.tud_idFilho;
                qs.Parameters.Add(Param);


            }
        }

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        /// <returns>TRUE - Se entity.ParametroId > 0</returns>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CLS_TurmaAulaRegencia entity)
        {
            if (entity != null & qs != null)
            {

            }

            return false;
        }
    }
}