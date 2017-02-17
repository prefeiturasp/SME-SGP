/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using MSTech.GestaoEscolar.Entities;
    using System.Linq;
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class ACA_AlunoAnexoDAO : AbstractACA_AlunoAnexoDAO
    {
        #region Métodos

        /// <summary>
        /// Seleciona os anexos ativos por aluno.
        /// </summary>
        /// <param name="alu_id">ID do aluno.</param>
        /// <returns></returns>
        public List<ACA_AlunoAnexo> SelecionaAtivosPorALuno(long alu_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoAnexo_SelecionaAtivosPorALuno", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return.Rows.Count > 0 ?
                    qs.Return.Rows.Cast<DataRow>().Select(dr => DataRowToEntity(dr, new ACA_AlunoAnexo())).ToList() :
                    new List<ACA_AlunoAnexo>();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion

        #region Métodos sobrescritos

        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_AlunoAnexo entity)
        {
            entity.aan_dataCriacao = entity.aan_dataAlteracao = DateTime.Now;
            base.ParamInserir(qs, entity);
        }

        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_AlunoAnexo entity)
        {
            entity.aan_dataAlteracao = DateTime.Now;
            base.ParamAlterar(qs, entity);
            qs.Parameters.RemoveAt("@aan_dataCriacao");
        }

        protected override bool Alterar(ACA_AlunoAnexo entity)
        {
            __STP_UPDATE = "NEW_ACA_AlunoAnexo_UPDATE";
            return base.Alterar(entity);
        }

        protected override void ParamDeletar(QueryStoredProcedure qs, ACA_AlunoAnexo entity)
        {
            base.ParamDeletar(qs, entity);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@aan_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@aan_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        public override bool Delete(ACA_AlunoAnexo entity)
        {
            __STP_DELETE = "NEW_ACA_AlunoAnexo_UpdateSituacao";
            return base.Delete(entity);
        }

        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_AlunoAnexo entity)
        {
            if (entity != null & qs != null)
            {
                entity.aan_id = Convert.ToInt32(qs.Return.Rows[0][0]);
                return (entity.aan_id > 0);
            }

            return false;
        }

        #endregion
    }
}