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
    /// Classe abstrata de LOG_AlertaFimFechamento.
    /// </summary>
    public abstract class Abstract_LOG_AlertaFimFechamentoDAO : Abstract_DAL<LOG_AlertaFimFechamento>
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
        protected override void ParamCarregar(QuerySelectStoredProcedure qs, LOG_AlertaFimFechamento entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@usu_id";
                Param.Size = 16;
                Param.Value = entity.usu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@evt_id";
                Param.Size = 8;
                Param.Value = entity.evt_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@lff_dataEnvio";

                Param.Value = entity.lff_dataEnvio;
                qs.Parameters.Add(Param);


            }
        }

        /// <summary>
        /// Configura os parametros do metodo de Inserir.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, LOG_AlertaFimFechamento entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@usu_id";
                Param.Size = 16;
                Param.Value = entity.usu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@evt_id";
                Param.Size = 8;
                Param.Value = entity.evt_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@lff_dataEnvio";

                Param.Value = entity.lff_dataEnvio;
                qs.Parameters.Add(Param);


            }
        }

        /// <summary>
        /// Configura os parametros do metodo de Alterar.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamAlterar(QueryStoredProcedure qs, LOG_AlertaFimFechamento entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@usu_id";
                Param.Size = 16;
                Param.Value = entity.usu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@evt_id";
                Param.Size = 8;
                Param.Value = entity.evt_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@lff_dataEnvio";

                Param.Value = entity.lff_dataEnvio;
                qs.Parameters.Add(Param);


            }
        }

        /// <summary>
        /// Configura os parametros do metodo de Deletar.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamDeletar(QueryStoredProcedure qs, LOG_AlertaFimFechamento entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@usu_id";
                Param.Size = 16;
                Param.Value = entity.usu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@evt_id";
                Param.Size = 8;
                Param.Value = entity.evt_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@lff_dataEnvio";

                Param.Value = entity.lff_dataEnvio;
                qs.Parameters.Add(Param);


            }
        }

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        /// <returns>TRUE - Se entity.ParametroId > 0</returns>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, LOG_AlertaFimFechamento entity)
        {
            return true;
        }
    }
}