/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using System;
    using System.Data;
    using System.Collections.Generic;
    using MSTech.GestaoEscolar.Entities;
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class ACA_RecomendacaoAlunoResponsavelDAO : AbstractACA_RecomendacaoAlunoResponsavelDAO
	{
        public DataTable SelecionaAtivos()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_RecomendacaoAlunoResponsavel_SelecionaAtivos", _Banco);
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

        /// <summary>
        /// Seleciona as recomendações dos alunos/responsáveis por tipo
        /// </summary>
        /// <param name="rar_tipo">Tipo</param>
        /// <returns></returns>
        public List<ACA_RecomendacaoAlunoResponsavel> SelecionarAtivosPorTipo(byte rar_tipo)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_RecomendacaoAlunoResponsavel_SelecionaAtivosPorTipo", _Banco);
            List<ACA_RecomendacaoAlunoResponsavel> lst = new List<ACA_RecomendacaoAlunoResponsavel>();

            try
            {
                #region Parametros

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@rar_tipo";
                Param.Size = 1;
                Param.Value = rar_tipo;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                foreach (DataRow dr in qs.Return.Rows)
                {
                    ACA_RecomendacaoAlunoResponsavel entity = new ACA_RecomendacaoAlunoResponsavel();
                    lst.Add(DataRowToEntity(dr, entity));
                }

                return lst;
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

        #region Sobrescritos

        protected override void ParamInserir(QuerySelectStoredProcedure qs, Entities.ACA_RecomendacaoAlunoResponsavel entity)
        {
            base.ParamInserir(qs, entity);
            qs.Parameters["@rar_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@rar_dataAlteracao"].Value = DateTime.Now;
            qs.Parameters["@rar_situacao"].Value = 1;
        }

        protected override void ParamAlterar(QueryStoredProcedure qs, Entities.ACA_RecomendacaoAlunoResponsavel entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@rar_dataCriacao");
            qs.Parameters["@rar_dataAlteracao"].Value = DateTime.Now;
        }

        protected override bool Alterar(Entities.ACA_RecomendacaoAlunoResponsavel entity)
        {
            __STP_UPDATE = "NEW_ACA_RecomendacaoAlunoResponsavel_UPDATE";
            return base.Alterar(entity);
        }

        protected override void ParamDeletar(QueryStoredProcedure qs, Entities.ACA_RecomendacaoAlunoResponsavel entity)
        {
            base.ParamDeletar(qs, entity);
            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@rar_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@rar_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        public override bool Delete(Entities.ACA_RecomendacaoAlunoResponsavel entity)
        {
            __STP_DELETE = "NEW_ACA_RecomendacaoAlunoResponsavel_UPDATE_Situacao";
            return base.Delete(entity);
        }

        #endregion Sobrescritos

	}
}