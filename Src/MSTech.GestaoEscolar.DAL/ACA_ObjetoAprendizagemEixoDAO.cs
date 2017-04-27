/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using Data.Common;
    using Entities;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Data;

    public class ACA_ObjetoAprendizagemEixoDAO : Abstract_ACA_ObjetoAprendizagemEixoDAO
    {
        /// <summary>
        /// Retorna os eixos por disciplina e ano letivo.
        /// </summary>
        /// <param name="tds_id">ID da disciplina</param>
        /// <param name="cal_ano">Ano letivo</param>
        /// <param name="oae_idPai">ID do eixo pai</param>
        /// <returns></returns>
        public List<ACA_ObjetoAprendizagemEixo> SelectBy_TipoDisciplina(int tds_id, int cal_ano, int oae_idPai)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_ObjetoAprendizagemEixo_SELECT_ByTipoDisciplina", _Banco);
            try
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tds_id";
                Param.Value = tds_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_ano";
                Param.Value = cal_ano;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@oae_idPai";
                if (oae_idPai > 0)
                    Param.Value = oae_idPai;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                qs.Execute();
                
                return (from DataRow dr in qs.Return.Rows select DataRowToEntity(dr, new ACA_ObjetoAprendizagemEixo())).ToList();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Verifica se o objeto de aprendizagem está em uso
        /// </summary>
        /// <param name="oap_id">ID do objeto de aprendizagem</param>
        /// <returns>true = em uso</returns>
        public bool ObjetoEmUso(int oae_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_ObjetoAprendizagemEixo_SELECTEmUsoBy_oae_id", _Banco);
            try
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@oae_id";
                Param.Value = oae_id;
                qs.Parameters.Add(Param);

                qs.Execute();

                return qs.Return.Rows.Count > 0;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Verifica se existe um eixo cadastrado com o mesmo nome (se for subeixo verifica apenas os subeixos do eixo pai)
        /// </summary>
        /// <param name="oae_id">ID do eixo que está sendo salvo</param>
        /// <param name="oae_idPai">ID do eixo pai</param>
        /// <param name="oae_descricao">Descrição do eixo</param>
        /// <param name="banco">Transação do banco</param>
        /// <returns></returns>
        public bool VerificaEixoMesmoNome(int oae_id, int tds_id, int cal_ano, int oae_idPai, string oae_descricao)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_ObjetoAprendizagemEixo_SELECTMesmoNome", _Banco);
            try
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tds_id";
                Param.Value = tds_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_ano";
                Param.Value = cal_ano;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@oae_id";
                Param.Value = oae_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@oae_idPai";
                if (oae_idPai > 0)
                    Param.Value = oae_idPai;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.Size = 500;
                Param.ParameterName = "@oae_descricao";
                Param.Value = oae_descricao;
                qs.Parameters.Add(Param);

                qs.Execute();

                return qs.Return.Rows.Count > 0;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #region Métodos sobrescritos

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_ObjetoAprendizagemEixo entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@oae_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@oae_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_ObjetoAprendizagemEixo entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@oae_dataCriacao");
            qs.Parameters["@oae_dataAlteracao"].Value = DateTime.Now;
        }


        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity"> Entidade ACA_ObjetoAprendizagemEixo</param>
        /// <returns>true = sucesso | false = fracasso</returns> 
        protected override bool Alterar(ACA_ObjetoAprendizagemEixo entity)
        {
            __STP_UPDATE = "NEW_ACA_ObjetoAprendizagemEixo_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        protected override void ParamDeletar(QueryStoredProcedure qs, ACA_ObjetoAprendizagemEixo entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@oae_id";
            Param.Size = 4;
            Param.Value = entity.oae_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@oae_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@oae_dataAlteracao";
            Param.Size = 8;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity"> Entidade ACA_ObjetoAprendizagemEixo</param>
        /// <returns>true = sucesso | false = fracasso</returns>         
        public override bool Delete(ACA_ObjetoAprendizagemEixo entity)
        {
            __STP_DELETE = "NEW_ACA_ObjetoAprendizagemEixo_UpdateSituacao";
            return base.Delete(entity);
        }

        #endregion
    }
}