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
    using System.Data.SqlClient;
    public class CLS_AlunoSondagemDAO : Abstract_CLS_AlunoSondagemDAO
    {
        /// <summary>
        /// Seleciona os alunos ligados à sondagem/agendamento.
        /// </summary>
        /// <param name="snd_id">ID da sondagem</param>
        /// <param name="sda_id">ID do agendamento</param>
        /// <returns></returns>
        public List<CLS_AlunoSondagem> SelectAgendamentosBy_Sondagem(int snd_id, int sda_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLAS_AlunoSondagem_SelectBy_SondagemAgendamento", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@snd_id";
                Param.Size = 4;
                Param.Value = snd_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@sda_id";
                Param.Size = 4;
                if (sda_id > 0)
                    Param.Value = sda_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return (from DataRow dr in qs.Return.Rows select DataRowToEntity(dr, new CLS_AlunoSondagem())).ToList();
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
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_AlunoSondagem entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@asn_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@asn_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, CLS_AlunoSondagem entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@asn_dataCriacao");
            qs.Parameters["@asn_dataAlteracao"].Value = DateTime.Now;
        }


        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity"> Entidade ACA_Sondagem</param>
        /// <returns>true = sucesso | false = fracasso</returns> 
        protected override bool Alterar(CLS_AlunoSondagem entity)
        {
            __STP_UPDATE = "NEW_CLS_AlunoSondagem_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        protected override void ParamDeletar(QueryStoredProcedure qs, CLS_AlunoSondagem entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@snd_id";
            Param.Size = 4;
            Param.Value = entity.snd_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@sda_id";
            Param.Size = 4;
            Param.Value = entity.sda_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@alu_id";
            Param.Size = 8;
            Param.Value = entity.alu_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@asn_id";
            Param.Size = 4;
            Param.Value = entity.asn_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@asn_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@asn_dataAlteracao";
            Param.Size = 8;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity"> Entidade ACA_Sondagem</param>
        /// <returns>true = sucesso | false = fracasso</returns>         
        public override bool Delete(CLS_AlunoSondagem entity)
        {
            __STP_DELETE = "NEW_CLS_AlunoSondagem_UpdateSituacao";
            return base.Delete(entity);
        }

        /// <summary>
        /// Salva o lançamento de sondagem.
        /// </summary>
        /// <returns></returns>
        public bool SalvarEmLote(DataTable dtAlunoSondagem, int snd_id, int sda_id)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_AlunoSondagem_SalvarEmLote", _Banco);

            try
            {
                #region Parâmetros

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbAlunoSondagem";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_AlunoSondagem";
                sqlParam.Value = dtAlunoSondagem;
                qs.Parameters.Add(sqlParam);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@snd_id";
                Param.Size = 4;
                Param.Value = snd_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@sda_id";
                Param.Size = 4;
                if (sda_id > 0)
                    Param.Value = sda_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

                qs.Execute();

                return qs.Return > 0;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }
    }
}