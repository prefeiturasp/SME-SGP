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
    using System.Collections.Generic;
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class CLS_PlanejamentoCicloDAO : Abstract_CLS_PlanejamentoCicloDAO
    {
        #region Métodos de consulta

        /// <summary>
        /// Seleciona o plano de ciclo ativo por turma e tipo de ciclo.
        /// </summary>
        /// <param name="tur_id">ID da turma.</param>
        /// <param name="tci_id">ID do tipo de ciclo.</param>
        /// <returns></returns>
        public CLS_PlanejamentoCiclo SelecionaAtivoPorTurmaTipoCiclo(long tur_id, int tci_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_PlanejamentoCiclo_SelecionaAtivoPorTurmaTipoCiclo", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tci_id";
                Param.Size = 4;
                Param.Value = tci_id;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

                qs.Execute();

                return qs.Return.Rows.Count > 0 ? DataRowToEntity(qs.Return.Rows[0], new CLS_PlanejamentoCiclo()) : new CLS_PlanejamentoCiclo();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        

        /// <summary>
        /// Seleciona as alterações realizadas no plano de ciclo
        /// </summary>
        /// <param name="tur_id">ID da turma.</param>
        /// <param name="tci_id">ID do tipo de ciclo.</param>
        /// <returns></returns>
        public DataTable SelecionaHistoricoAlteracoes(long tur_id, int tci_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_PlanejamentoCiclo_SelecionaHistoricoAlteracoes", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tci_id";
                Param.Size = 4;
                Param.Value = tci_id;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion

        #region Métodos de alteração

        /// <summary>
        /// Atualiza a situação dos planos de ciclo por escola, ano letivo e tipo de ciclo.
        /// </summary>
        /// <param name="esc_id">Id da escola.</param>
        /// <param name="uni_id">Id da unidade da escola.</param>
        /// <param name="plc_anoLetivo">Ano letivo.</param>
        /// <param name="tci_id">Id do tipo de ciclo.</param>
        /// <param name="plc_situacao">Situacao.</param>
        /// <returns></returns>
        public bool AtualizaSituacaoPorEscolaAnoTipoCiclo(int esc_id, int uni_id, int plc_anoLetivo, int tci_id, byte plc_situacao)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_PlanejamentoCiclo_AtualizaSituacaoPorEscolaAnoTipoCiclo", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                Param.Value = esc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@uni_id";
                Param.Size = 4;
                Param.Value = uni_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@plc_anoLetivo";
                Param.Size = 4;
                Param.Value = plc_anoLetivo;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tci_id";
                Param.Size = 4;
                Param.Value = tci_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@plc_situacao";
                Param.Size = 1;
                Param.Value = plc_situacao;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return > 0;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion

        #region Métodos sobrescritos

        protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_PlanejamentoCiclo entity)
        {
            entity.plc_dataCriacao = entity.plc_dataAlteracao = DateTime.Now;
            base.ParamInserir(qs, entity);
        }

        protected override void ParamAlterar(QueryStoredProcedure qs, CLS_PlanejamentoCiclo entity)
        {
            entity.plc_dataAlteracao = DateTime.Now;
            base.ParamAlterar(qs, entity);
            qs.Parameters.RemoveAt("@plc_dataCriacao");
        }

        protected override bool Alterar(CLS_PlanejamentoCiclo entity)
        {
            __STP_UPDATE = "NEW_CLS_PlanejamentoCiclo_UPDATE";
            return base.Alterar(entity);
        }

        protected override void ParamDeletar(QueryStoredProcedure qs, CLS_PlanejamentoCiclo entity)
        {
            base.ParamDeletar(qs, entity);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@plc_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@plc_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        public override bool Delete(CLS_PlanejamentoCiclo entity)
        {
            __STP_DELETE = "NEW_CLS_PlanejamentoCiclo_UpdateSituacao";
            return base.Delete(entity);
        }

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        /// <returns>TRUE - Se entity.ParametroId > 0</returns>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CLS_PlanejamentoCiclo entity)
        {
            if (entity != null & qs != null)
            {
                entity.plc_id = Convert.ToInt32(qs.Return.Rows[0][0]);
                return entity.plc_id > 0;
            }

            return false;
        }

        #endregion Métodos sobrescritos
    }
}