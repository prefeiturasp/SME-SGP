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
    /// Classe abstrata de CFG_Alerta.
    /// </summary>
    public abstract class Abstract_CFG_AlertaDAO : Abstract_DAL<CFG_Alerta>
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
        protected override void ParamCarregar(QuerySelectStoredProcedure qs, CFG_Alerta entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cfa_id";
                Param.Size = 4;
                Param.Value = entity.cfa_id;
                qs.Parameters.Add(Param);


            }
        }

        /// <summary>
        /// Configura os parametros do metodo de Inserir.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, CFG_Alerta entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@cfa_tipo";
                Param.Size = 1;
                Param.Value = entity.cfa_tipo;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@cfa_nome";
                Param.Size = 200;
                Param.Value = entity.cfa_nome;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@cfa_nomeProcedimento";
                Param.Size = 100;
                Param.Value = entity.cfa_nomeProcedimento;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@cfa_assunto";
                Param.Size = 500;
                if (!string.IsNullOrEmpty(entity.cfa_assunto))
                {
                    Param.Value = entity.cfa_assunto;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cfa_periodoAnalise";
                Param.Size = 4;
                if (entity.cfa_periodoAnalise > 0)
                {
                    Param.Value = entity.cfa_periodoAnalise;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cfa_periodoValidade";
                Param.Size = 4;
                if (entity.cfa_periodoValidade > 0)
                {
                    Param.Value = entity.cfa_periodoValidade;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@cfa_situacao";
                Param.Size = 1;
                Param.Value = entity.cfa_situacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@cfa_dataCriacao";

                Param.Value = entity.cfa_dataCriacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@cfa_dataAlteracao";

                Param.Value = entity.cfa_dataAlteracao;
                qs.Parameters.Add(Param);


            }
        }

        /// <summary>
        /// Configura os parametros do metodo de Alterar.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamAlterar(QueryStoredProcedure qs, CFG_Alerta entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cfa_id";
                Param.Size = 4;
                Param.Value = entity.cfa_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@cfa_tipo";
                Param.Size = 1;
                Param.Value = entity.cfa_tipo;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@cfa_nome";
                Param.Size = 200;
                Param.Value = entity.cfa_nome;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@cfa_nomeProcedimento";
                Param.Size = 100;
                Param.Value = entity.cfa_nomeProcedimento;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@cfa_assunto";
                Param.Size = 500;
                if (!string.IsNullOrEmpty(entity.cfa_assunto))
                {
                    Param.Value = entity.cfa_assunto;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cfa_periodoAnalise";
                Param.Size = 4;
                if (entity.cfa_periodoAnalise > 0)
                {
                    Param.Value = entity.cfa_periodoAnalise;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cfa_periodoValidade";
                Param.Size = 4;
                if (entity.cfa_periodoValidade > 0)
                {
                    Param.Value = entity.cfa_periodoValidade;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@cfa_situacao";
                Param.Size = 1;
                Param.Value = entity.cfa_situacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@cfa_dataCriacao";

                Param.Value = entity.cfa_dataCriacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@cfa_dataAlteracao";

                Param.Value = entity.cfa_dataAlteracao;
                qs.Parameters.Add(Param);


            }
        }

        /// <summary>
        /// Configura os parametros do metodo de Deletar.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamDeletar(QueryStoredProcedure qs, CFG_Alerta entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cfa_id";
                Param.Size = 4;
                Param.Value = entity.cfa_id;
                qs.Parameters.Add(Param);


            }
        }

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        /// <returns>TRUE - Se entity.ParametroId > 0</returns>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CFG_Alerta entity)
        {
            if (entity != null & qs != null)
            {
                entity.cfa_id = Convert.ToInt32(qs.Return.Rows[0][0]);
                return (entity.cfa_id > 0);
            }

            return false;
        }
    }
}