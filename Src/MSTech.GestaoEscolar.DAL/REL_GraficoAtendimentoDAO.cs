/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using Entities;
    using System;
    using System.Data;

    /// <summary>
    /// Description: .
    /// </summary>
    public class REL_GraficoAtendimentoDAO : Abstract_REL_GraficoAtendimentoDAO
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rea_id"></param>
        /// <param name="gra_titulo"></param>
        /// <returns></returns>
        public DataTable SelecionaGraficoPorTipoRelatorioRelatorio(bool paginado, int currentPage, int pageSize, int rea_id, int rea_tipo, string gra_titulo, out int totalRecords)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_REL_GraficoAtendimento_SelecionaPorRelatorio", _Banco);

            try
            {
                DataTable dt = new DataTable();

                #region Parâmetro
                Param = qs.NewParameter();
                Param.ParameterName = "@rea_id";
                Param.DbType = DbType.Int32;
                Param.Size = 16;
                if (rea_id > 0)
                    Param.Value = rea_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@rea_tipo";
                Param.DbType = DbType.Byte;
                Param.Size = 16;
                if (rea_tipo > 0)
                    Param.Value = rea_tipo;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@gra_titulo";
                Param.DbType = DbType.String;
                if (!String.IsNullOrEmpty(gra_titulo))
                    Param.Value = gra_titulo;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                if (paginado)
                    totalRecords = qs.Execute(currentPage, pageSize);
                else
                {
                    qs.Execute();
                    totalRecords = qs.Return.Rows.Count;
                }

                if (qs.Return.Rows.Count > 0)
                    dt = qs.Return;

                return dt;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        public DataTable SelectBy_titulo
            (
                string gra_titulo
            )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_REL_GraficoAtendimento_SelecionaPorTitulo", _Banco);

            try
            {
                DataTable dt = new DataTable();

                #region Parâmetro                

                Param = qs.NewParameter();
                Param.ParameterName = "@gra_titulo";
                Param.DbType = DbType.String;
                Param.Value = gra_titulo;
                qs.Parameters.Add(Param);

                #endregion
                
                qs.Execute();
                
                if (qs.Return.Rows.Count > 0)
                    dt = qs.Return;

                return dt;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }


        /// <summary>
        /// Retorna os dados para renderizar o gráfico de atendimento
        /// </summary>
        /// <param name="gra_id"></param>
        /// <param name="esc_id"></param>
        /// <param name="uni_id"></param>
        /// <param name="cur_id"></param>
        /// <param name="crr_id"></param>
        /// <param name="crp_id"></param>
        /// <returns></returns>
        public DataTable SelecionarDadosGrafico
        (
            int gra_id,
            int esc_id,
            int uni_id,
            int cur_id,
            int crr_id,
            int crp_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_REL_GraficoAtendimento_SelecionarDadosGrafico", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.ParameterName = "@gra_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.Value = gra_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@esc_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.Value = esc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@uni_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.Value = uni_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@cur_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                if (cur_id > 0)
                {
                    Param.Value = cur_id;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@crr_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                if (crr_id > 0)
                {
                    Param.Value = crr_id;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@crp_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                if (crp_id > 0)
                {
                    Param.Value = crp_id;
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
        /// Retorna os gráficos de atendimento por tipo de relatório
        /// </summary>
        /// <param name="rea_tipo"></param>
        /// <returns></returns>
        public DataTable SelecionaPorTipoRelatorio(byte rea_tipo)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_REL_GraficoAtendimento_SelecionarPorTipoRelatorio", _Banco);

            try
            {
                Param = qs.NewParameter();
                Param.ParameterName = "@rea_tipo";
                Param.DbType = DbType.Byte;
                Param.Size = 1;
                Param.Value = rea_tipo;
                qs.Parameters.Add(Param);

                qs.Execute();

                return qs.Return;
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
        protected override void ParamInserir(QuerySelectStoredProcedure qs, REL_GraficoAtendimento entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@gra_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@gra_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, REL_GraficoAtendimento entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@gra_dataCriacao");
            qs.Parameters["@gra_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity"> Entidade ACA_Sondagem</param>
        /// <returns>true = sucesso | false = fracasso</returns> 
        protected override bool Alterar(REL_GraficoAtendimento entity)
        {
            __STP_UPDATE = "NEW_REL_GraficoAtendimento_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        protected override void ParamDeletar(QueryStoredProcedure qs, REL_GraficoAtendimento entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@gra_id";
            Param.Size = 4;
            Param.Value = entity.gra_id;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity"> Entidade ACA_Sondagem</param>
        /// <returns>true = sucesso | false = fracasso</returns>         
        public override bool Delete(REL_GraficoAtendimento entity)
        {
            __STP_DELETE = "NEW_REL_GraficoAtendimento_UpdateSituacao";
            return base.Delete(entity);
        }

    }
}