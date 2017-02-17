/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using MSTech.GestaoEscolar.Entities;

    /// <summary>
    /// Description: .
    /// </summary>
    public class CLS_FrequenciaReuniaoDAO : AbstractCLS_FrequenciaReuniaoDAO
    {
        #region Métodos Criados

        /// <summary>
        /// Seleciona pelas PK
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cap_id">ID do período do calendário</param>
        /// <returns></returns>
        public DataTable SelectBy_tur_id_cal_id_cap_id
           (
               long tur_id
               , int cal_id
               , int cap_id
           )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_FrequenciaReuniao_SelectBy_tur_id_cal_id_cap_id", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            Param.Value = cal_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cap_id";
            Param.Size = 4;
            Param.Value = cap_id;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Salva os dados em lote.
        /// </summary>
        /// <param name="dtFrequenciaReuniao">DataTable com os dados.</param>
        /// <returns>True em caso de sucesso.</returns>
        public bool SalvarEmLote(DataTable dtFrequenciaReuniao)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_FrequenciaReuniao_SalvarEmLote", _Banco);

            try
            {
                #region Parâmetro

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@dtFrequenciaReuniao";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_FrequenciaReuniao";
                sqlParam.Value = dtFrequenciaReuniao;
                qs.Parameters.Add(sqlParam);

                #endregion Parâmetro

                qs.Execute();

                return qs.Return > 0;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion Métodos Criados

        #region Métodos Sobrescritos

        /// <summary>
        /// Override do método inserir
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_FrequenciaReuniao entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@frr_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@frr_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Override do método alterar
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, CLS_FrequenciaReuniao entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@frr_dataCriacao");
            qs.Parameters["@frr_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity"> Entidade CLS_FrequenciaReuniao</param>
        /// <returns>true = sucesso | false = fracasso</returns>
        protected override bool Alterar(CLS_FrequenciaReuniao entity)
        {
            __STP_UPDATE = "NEW_CLS_FrequenciaReuniao_UPDATE";
            return base.Alterar(entity);
        }

        #endregion Métodos Sobrescritos
    }
}