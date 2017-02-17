/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using System;
    using System.Data;
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using MSTech.GestaoEscolar.Entities;
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class CLS_AlunoProjetoDAO : Abstract_CLS_AlunoProjetoDAO
    {
        #region Métodos sobrescritos

        protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_AlunoProjeto entity)
        {
            entity.apj_dataCriacao = entity.apj_dataAlteracao = DateTime.Now;
            base.ParamInserir(qs, entity);

            // Verificação pq frequência pode ser igual a zero
            if (entity.apj_frequencia > -1)
                qs.Parameters["@apj_frequencia"].Value = entity.apj_frequencia;
            else
                qs.Parameters["@apj_frequencia"].Value = DBNull.Value;
        }

        protected override void ParamAlterar(QueryStoredProcedure qs, CLS_AlunoProjeto entity)
        {
            entity.apj_dataAlteracao = DateTime.Now;
            base.ParamAlterar(qs, entity);
            qs.Parameters.RemoveAt("@apj_dataCriacao");

            // Verificação pq frequência pode ser igual a zero
            if (entity.apj_frequencia > -1)
                qs.Parameters["@apj_frequencia"].Value = entity.apj_frequencia;
            else
                qs.Parameters["@apj_frequencia"].Value = DBNull.Value;
        }

        protected override bool Alterar(CLS_AlunoProjeto entity)
        {
            __STP_UPDATE = "NEW_CLS_AlunoProjeto_UPDATE";
            return base.Alterar(entity);
        }

        protected override void ParamDeletar(QueryStoredProcedure qs, CLS_AlunoProjeto entity)
        {
            base.ParamDeletar(qs, entity);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@apj_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@apj_situacao";
            Param.Size = 1;
            Param.Value = entity.apj_situacao;
            qs.Parameters.Add(Param);
        }

        public override bool Delete(CLS_AlunoProjeto entity)
        {
            __STP_DELETE = "NEW_CLS_AlunoProjeto_UpdateSituacao";
            return base.Delete(entity);
        }

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        /// <returns>TRUE - Se entity.ParametroId > 0</returns>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CLS_AlunoProjeto entity)
        {
            if (entity != null & qs != null)
            {
                entity.apj_id = Convert.ToInt32(qs.Return.Rows[0][0]);
                return entity.apj_id > 0;
            }

            return false;
        }		

        #endregion
    }
}