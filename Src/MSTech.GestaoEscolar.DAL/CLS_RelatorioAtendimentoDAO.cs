/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using Data.Common;
    using Entities;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    /// <summary>
    /// Description: .
    /// </summary>
    public class CLS_RelatorioAtendimentoDAO : Abstract_CLS_RelatorioAtendimentoDAO
	{
        #region Métodos de consulta

        /// <summary>
        /// Carrega os tipos de relatório verificando a permissão do usuário.
        /// </summary>
        /// <param name="usu_id"></param>
        /// <returns></returns>
        public DataTable SelecionaPorPermissaoUsuarioTipo(Guid usu_id, byte rea_tipo)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_RelatorioAtendimento_SelecionaPorPermissaoUsuarioTipo", _Banco);

            try
            {
                #region Parâmetro

                Param = qs.NewParameter();
                Param.ParameterName = "@usu_id";
                Param.DbType = DbType.Guid;
                Param.Size = 16;
                Param.Value = usu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@rea_tipo";
                Param.DbType = DbType.Byte;
                Param.Size = 1;
                if (rea_tipo > 0)
                {
                    Param.Value = rea_tipo;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Carrega os relatórios de RP verificando a permissão do usuário e o tipo de disciplina.
        /// </summary>
        /// <returns></returns>
        public DataTable SelecionaRelatoriosRPDisciplina(Guid usu_id, long alu_id, long tud_id, int tds_idRP)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_RelatorioAtendimento_SelecionaRelatoriosRPDisciplina", _Banco);

            try
            {
                #region Parâmetro

                Param = qs.NewParameter();
                Param.ParameterName = "@usu_id";
                Param.DbType = DbType.Guid;
                Param.Size = 16;
                Param.Value = usu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@alu_id";
                Param.DbType = DbType.Int64;
                Param.Size = 8;
                Param.Value = alu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@tud_id";
                Param.DbType = DbType.Int64;
                Param.Size = 8;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@tds_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.Value = tds_idRP;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Carrega a estrutura do relatório
        /// </summary>
        /// <param name="rea_id"></param>
        /// <param name="usu_id"></param>
        /// <returns></returns>
        public DataSet SelecionaRelatorio(int rea_id, Guid usu_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_RelatorioAtendimento_SelecionaRelatorio", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.ParameterName = "@rea_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.Value = rea_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@usu_id";
                Param.DbType = DbType.Guid;
                Param.Size = 16;
                Param.Value = usu_id;
                qs.Parameters.Add(Param);

                #endregion

                return qs.Execute_DataSet();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Pesquisa relatórios por tipo.
        /// </summary>
        /// <returns></returns>
        public DataTable PesquisaRelatorioPorTipo(byte rea_tipo, int currentPage, int pageSize, out int totalRecords)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_RelatorioAtendimento_SelecionaPorTipo", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.ParameterName = "@rea_tipo";
                Param.DbType = DbType.Byte;
                Param.Size = 2;
                if (rea_tipo > 0)
                {
                    Param.Value = rea_tipo;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                #endregion

                totalRecords = qs.Execute(currentPage, pageSize);

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion

        #region Sobrescritos

        protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_RelatorioAtendimento entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@rea_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@rea_dataAlteracao"].Value = DateTime.Now;
        }

        protected override void ParamAlterar(QueryStoredProcedure qs, CLS_RelatorioAtendimento entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@rea_dataCriacao");
            qs.Parameters["@rea_dataAlteracao"].Value = DateTime.Now;
        }

        protected override bool Alterar(CLS_RelatorioAtendimento entity)
        {
            __STP_UPDATE = "NEW_CLS_RelatorioAtendimento_UPDATE";
            return base.Alterar(entity);
        }

        protected override void ParamDeletar(QueryStoredProcedure qs, CLS_RelatorioAtendimento entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@rea_id";
            Param.Size = 4;
            Param.Value = entity.rea_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@rea_dataAlteracao";
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@rea_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);
        }

        public override bool Delete(CLS_RelatorioAtendimento entity)
        {
            __STP_DELETE = "NEW_CLS_RelatorioAtendimento_UPDATESituacao";
            return base.Delete(entity);
        }

        #endregion
    }
}