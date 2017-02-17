/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using MSTech.GestaoEscolar.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data;
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class CLS_PlanejamentoProjetoDAO : Abstract_CLS_PlanejamentoProjetoDAO
    {
        /// <summary>
        /// Seleciona os projetos pela turmadisciplina
        /// </summary>
        /// <param name="tud_id">ID da turma disciplina</param>
        /// <returns></returns>
        public DataTable CarregarProjetos(long tud_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_PlanejamentoProjeto_SelectBy_TurmaDisciplina", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                #endregion

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

        #region Métodos sobrescritos

        protected override bool Alterar(CLS_PlanejamentoProjeto entity)
        {
            __STP_UPDATE = "NEW_CLS_PlanejamentoProjeto_UPDATE";
            return base.Alterar(entity);
        }

        public override bool Delete(CLS_PlanejamentoProjeto entity)
        {
            __STP_DELETE = "NEW_CLS_PlanejamentoProjeto_UpdateSituacao";
            return base.Delete(entity);
        }

        protected override void ParamAlterar(QueryStoredProcedure qs, CLS_PlanejamentoProjeto entity)
        {
            entity.plp_dataAlteracao = DateTime.Now;
            base.ParamAlterar(qs, entity);
            qs.Parameters.RemoveAt("@plp_dataCriacao");
        }

        protected override void ParamDeletar(QueryStoredProcedure qs, CLS_PlanejamentoProjeto entity)
        {
            base.ParamDeletar(qs, entity);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@plp_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@plp_situacao";
            Param.Size = 1;
            Param.Value = entity.plp_situacao;
            qs.Parameters.Add(Param);
        }

        protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_PlanejamentoProjeto entity)
        {
            entity.plp_dataCriacao = entity.plp_dataAlteracao = DateTime.Now;
            base.ParamInserir(qs, entity);
        }

        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CLS_PlanejamentoProjeto entity)
        {
            if (entity != null & qs != null)
            {
                entity.plp_id = Convert.ToInt32(qs.Return.Rows[0][0]);
                return entity.plp_id > 0;
            }

            return false;
        }

        #endregion
    }
}