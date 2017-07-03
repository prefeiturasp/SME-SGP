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
    /// Classe abstrata de CLS_TurmaAtividadeExtraClasseRelacionada.
    /// </summary>
    public abstract class Abstract_CLS_TurmaAtividadeExtraClasseRelacionadaDAO : Abstract_DAL<CLS_TurmaAtividadeExtraClasseRelacionada>
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
        protected override void ParamCarregar(QuerySelectStoredProcedure qs, CLS_TurmaAtividadeExtraClasseRelacionada entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@taer_id";
                Param.Size = 16;
                Param.Value = entity.taer_id;
                qs.Parameters.Add(Param);


            }
        }

        /// <summary>
        /// Configura os parametros do metodo de Inserir.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_TurmaAtividadeExtraClasseRelacionada entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@usu_id";
                Param.Size = 16;
                Param.Value = entity.usu_id;
                qs.Parameters.Add(Param);


            }
        }

        /// <summary>
        /// Configura os parametros do metodo de Alterar.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamAlterar(QueryStoredProcedure qs, CLS_TurmaAtividadeExtraClasseRelacionada entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@taer_id";
                Param.Size = 16;
                Param.Value = entity.taer_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@usu_id";
                Param.Size = 16;
                Param.Value = entity.usu_id;
                qs.Parameters.Add(Param);


            }
        }

        /// <summary>
        /// Configura os parametros do metodo de Deletar.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamDeletar(QueryStoredProcedure qs, CLS_TurmaAtividadeExtraClasseRelacionada entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@taer_id";
                Param.Size = 16;
                Param.Value = entity.taer_id;
                qs.Parameters.Add(Param);


            }
        }

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        /// <returns>TRUE - Se entity.ParametroId > 0</returns>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CLS_TurmaAtividadeExtraClasseRelacionada entity)
        {
            entity.taer_id = new Guid(qs.Return.Rows[0][0].ToString());
            return (entity.taer_id != Guid.Empty);
        }
    }
}