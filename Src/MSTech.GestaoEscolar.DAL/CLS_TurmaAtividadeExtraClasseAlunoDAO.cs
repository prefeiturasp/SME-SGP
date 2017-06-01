/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using Entities;
    using System;
    /// <summary>
    /// Description: .
    /// </summary>
    public class CLS_TurmaAtividadeExtraClasseAlunoDAO : Abstract_CLS_TurmaAtividadeExtraClasseAlunoDAO
	{
        #region Métodos sobrescritos

        protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_TurmaAtividadeExtraClasseAluno entity)
        {
            entity.aea_dataCriacao = entity.aea_dataAlteracao = DateTime.Now;
            base.ParamInserir(qs, entity);
        }

        protected override void ParamAlterar(QueryStoredProcedure qs, CLS_TurmaAtividadeExtraClasseAluno entity)
        {
            entity.aea_dataAlteracao = DateTime.Now;
            base.ParamAlterar(qs, entity);
            qs.Parameters.RemoveAt("@aea_dataCriacao");
        }

        protected override bool Alterar(CLS_TurmaAtividadeExtraClasseAluno entity)
        {
            __STP_UPDATE = "NEW_CLS_TurmaAtividadeExtraClasseAluno_UPDATE";
            return base.Alterar(entity);
        }

        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CLS_TurmaAtividadeExtraClasseAluno entity)
        {
            if (entity != null & qs != null)
            {
                return true;
            }

            return false;
        }

        #endregion Métodos sobrescritos
    }
}