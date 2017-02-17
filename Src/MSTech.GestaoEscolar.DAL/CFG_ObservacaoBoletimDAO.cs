/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using System;
    using System.Data;
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class CFG_ObservacaoBoletimDAO : AbstractCFG_ObservacaoBoletimDAO
    {
        #region Métodos de Consulta
        /// <summary>
        /// Retorna todas as observações de boletim ativas.
        /// </summary>
        /// <returns></returns>
        public DataTable SelectAtivos()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CFG_ObservacaoBoletim_SELECT", _Banco);
            try
            {
                qs.Execute();

                return qs.Return;
            }
            catch
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }
        #endregion Métodos de Consulta

        #region Sobrescritos

        protected override void ParamInserir(QuerySelectStoredProcedure qs, Entities.CFG_ObservacaoBoletim entity)
        {
            entity.obb_dataCriacao = DateTime.Now;
            entity.obb_dataAlteracao = DateTime.Now;
            base.ParamInserir(qs, entity);
        }

        protected override void ParamAlterar(QueryStoredProcedure qs, Entities.CFG_ObservacaoBoletim entity)
        {

            entity.obb_dataAlteracao = DateTime.Now;
            base.ParamAlterar(qs, entity);
            qs.Parameters.RemoveAt("@obb_dataCriacao");
        }

        protected override bool Alterar(Entities.CFG_ObservacaoBoletim entity)
        {
            __STP_UPDATE = "NEW_CFG_ObservacaoBoletim_UPDATE";
            return base.Alterar(entity);
        }

        protected override void ParamDeletar(QueryStoredProcedure qs, Entities.CFG_ObservacaoBoletim entity)
        {
            base.ParamDeletar(qs, entity);
            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@obb_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@obb_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        public override bool Delete(Entities.CFG_ObservacaoBoletim entity)
        {
            __STP_DELETE = "NEW_CFG_ObservacaoBoletim_UPDATE_SITUACAO";
            return base.Delete(entity);
        }

        #endregion Sobrescritos
	}
}