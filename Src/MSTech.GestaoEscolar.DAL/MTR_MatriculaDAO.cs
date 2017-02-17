/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using MSTech.GestaoEscolar.Entities;
    using System.Data.SqlClient;

    /// <summary>
    /// Classe MTR_MatriculaDAO
    /// </summary>
    public class MTR_MatriculaDAO : AbstractMTR_MatriculaDAO
    {
        #region Métodos sobrescritos

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        /// <returns>TRUE - Se entity.ParametroId > 0</returns>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, MTR_Matricula entity)
        {
            if (entity != null & qs != null)
            {
                entity.mtr_id = Convert.ToInt32(qs.Return.Rows[0][0]);
                return (entity.mtr_id > 0);
            }

            return false;
        }

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        /// <param name="entity">Entidade MTR_Matricula</param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, MTR_Matricula entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@mtr_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@mtr_dataAlteracao"].Value = DateTime.Now;

            if (entity.unf_id == Guid.Empty)
            {
                qs.Parameters["@unf_id"].Value = DBNull.Value;
            }

            if (entity.cid_id == Guid.Empty)
            {
                qs.Parameters["@cid_id"].Value = DBNull.Value;
            }
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        /// <param name="entity">Entidade MTR_Matricula</param>
        protected override void ParamAlterar(QueryStoredProcedure qs, MTR_Matricula entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@mtr_dataCriacao");
            qs.Parameters["@mtr_dataAlteracao"].Value = DateTime.Now;

            if (entity.unf_id == Guid.Empty)
            {
                qs.Parameters["@unf_id"].Value = DBNull.Value;
            }

            if (entity.cid_id == Guid.Empty)
            {
                qs.Parameters["@cid_id"].Value = DBNull.Value;
            }
        }

        /// <summary>
        /// Inseri os valores da classe em um registro ja existente.
        /// </summary>
        /// <param name="entity">Entidade com os dados a serem modificados</param>
        /// <returns>True - Operacao bem sucedida</returns>
        protected override bool Alterar(MTR_Matricula entity)
        {
            __STP_UPDATE = "NEW_MTR_Matricula_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        /// <param name="entity">Entidade MTR_Matricula</param>
        protected override void ParamDeletar(QueryStoredProcedure qs, MTR_Matricula entity)
        {
            base.ParamDeletar(qs, entity);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@mtr_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@mtr_dataAlteracao";
            Param.Size = 8;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity"> Entidade MTR_Matricula</param>
        /// <returns>True = sucesso | False = fracasso</returns>
        public override bool Delete(MTR_Matricula entity)
        {
            __STP_DELETE = "NEW_MTR_Matricula_Update_Situacao";
            return base.Delete(entity);
        }

        #endregion Métodos sobrescritos
    }
}