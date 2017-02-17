/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL.Abstracts;
using MSTech.GestaoEscolar.Entities;

namespace MSTech.GestaoEscolar.DAL
{
    /// <summary>
    ///
    /// </summary>
    public class CLS_FrequenciaReuniaoResponsaveisDAO : Abstract_CLS_FrequenciaReuniaoResponsaveisDAO
    {
        #region Métodos Criados

        /// <summary>
        /// Verifica se existe frequências cadastradas para o calendário.
        /// </summary>
        /// <param name="cal_id">Id do calendário.</param>
        /// <param name="cur_id">Id do curso. </param>
        /// <param name="crr_id">Id do currículo. </param>
        /// <returns>Verdadeiro se existe algum registro ligado ao calendário.</returns>
        public bool VerificaFrequenciaPorCalendario(int cal_id, int cap_id, int cur_id, int crr_id, bool cadastroPorPeriodo)
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_FrequenciaReuniaoResponsaveis_SelectBy_cal_id", _Banco);
            try
            {
                #region Parâmetros

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
                if (cap_id > 0)
                    Param.Value = cap_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cur_id";
                Param.Size = 4;
                Param.Value = cur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crr_id";
                Param.Size = 4;
                Param.Value = crr_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@cadastroPorPeriodo";
                Param.Size = 1;
                Param.Value = cadastroPorPeriodo;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

                qs.Execute();

                return (qs.Return.Rows.Count) > 0;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Salva os dados em lote.
        /// </summary>
        /// <param name="dtFrequenciaReuniaoResponsaveis">DataTable com os dados.</param>
        /// <returns>True em caso de sucesso.</returns>
        public bool SalvarEmLote(DataTable dtFrequenciaReuniaoResponsaveis)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_FrequenciaReuniaoResponsaveis_SalvarEmLote", _Banco);

            try
            {
                #region Parâmetro

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@dtFrequenciaReuniaoResponsaveis";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_FrequenciaReuniaoResponsaveis";
                sqlParam.Value = dtFrequenciaReuniaoResponsaveis;
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
        /// Override do nome da ConnectionString.
        /// </summary>
        protected override string ConnectionStringName
        {
            get
            {
                return "GestaoEscolar";
            }
        }

        /// <summary>
        /// Override do método inserir
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_FrequenciaReuniaoResponsaveis entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@frp_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@frp_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Override do método alterar
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, CLS_FrequenciaReuniaoResponsaveis entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@frp_dataCriacao");
            qs.Parameters["@frp_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity"> Entidade CLS_FrequenciaReuniaoResponsaveis</param>
        /// <returns>true = sucesso | false = fracasso</returns>
        protected override bool Alterar(CLS_FrequenciaReuniaoResponsaveis entity)
        {
            __STP_UPDATE = "NEW_CLS_FrequenciaReuniaoResponsaveis_UPDATE";
            return base.Alterar(entity);
        }

        #endregion Métodos Sobrescritos

        #region Métodos Comentados

        ///// <summary>
        ///// Inseri os valores da classe em um registro ja existente
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem modificados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Alterar(CLS_FrequenciaReuniaoResponsaveis entity)
        //{
        //    return base.Alterar(entity);
        //}
        ///// <summary>
        ///// Inseri os valores da classe em um novo registro
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem inseridos</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Inserir(CLS_FrequenciaReuniaoResponsaveis entity)
        //{
        //    return base.Inserir(entity);
        //}
        ///// <summary>
        ///// Carrega um registro da tabela usando os valores nas chaves
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem carregados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Carregar(CLS_FrequenciaReuniaoResponsaveis entity)
        //{
        //    return base.Carregar(entity);
        //}
        ///// <summary>
        ///// Exclui um registro do banco
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem apagados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Delete(CLS_FrequenciaReuniaoResponsaveis entity)
        //{
        //    return base.Delete(entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Alterar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamAlterar(QueryStoredProcedure qs, CLS_FrequenciaReuniaoResponsaveis entity)
        //{
        //    base.ParamAlterar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Carregar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamCarregar(QuerySelectStoredProcedure qs, CLS_FrequenciaReuniaoResponsaveis entity)
        //{
        //    base.ParamCarregar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Deletar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamDeletar(QueryStoredProcedure qs, CLS_FrequenciaReuniaoResponsaveis entity)
        //{
        //    base.ParamDeletar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Inserir
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_FrequenciaReuniaoResponsaveis entity)
        //{
        //    base.ParamInserir(qs, entity);
        //}
        ///// <summary>
        ///// Salva o registro no banco de dados
        ///// </summary>
        ///// <param name="entity">Entidade com os dados para preenchimento para inserir ou alterar</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Salvar(CLS_FrequenciaReuniaoResponsaveis entity)
        //{
        //    return base.Salvar(entity);
        //}
        ///// <summary>
        ///// Realiza o select da tabela
        ///// </summary>
        ///// <returns>Lista com todos os registros da tabela</returns>
        //public override IList<CLS_FrequenciaReuniaoResponsaveis> Select()
        //{
        //    return base.Select();
        //}
        ///// <summary>
        ///// Realiza o select da tabela com paginacao
        ///// </summary>
        ///// <param name="currentPage">Pagina atual</param>
        ///// <param name="pageSize">Tamanho da pagina</param>
        ///// <param name="totalRecord">Total de registros na tabela original</param>
        ///// <returns>Lista com todos os registros da p�gina</returns>
        //public override IList<CLS_FrequenciaReuniaoResponsaveis> Select_Paginado(int currentPage, int pageSize, out int totalRecord)
        //{
        //    return base.Select_Paginado(currentPage, pageSize, out totalRecord);
        //}
        ///// <summary>
        ///// Recebe o valor do auto incremento e coloca na propriedade
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CLS_FrequenciaReuniaoResponsaveis entity)
        //{
        //    return base.ReceberAutoIncremento(qs, entity);
        //}
        ///// <summary>
        ///// Passa os dados de um datatable para uma entidade
        ///// </summary>
        ///// <param name="dr">DataRow do datatable preenchido</param>
        ///// <param name="entity">Entidade onde ser�o transferidos os dados</param>
        ///// <returns>Entidade preenchida</returns>
        //public override CLS_FrequenciaReuniaoResponsaveis DataRowToEntity(DataRow dr, CLS_FrequenciaReuniaoResponsaveis entity)
        //{
        //    return base.DataRowToEntity(dr, entity);
        //}
        ///// <summary>
        ///// Passa os dados de um datatable para uma entidade
        ///// </summary>
        ///// <param name="dr">DataRow do datatable preenchido</param>
        ///// <param name="entity">Entidade onde ser�o transferidos os dados</param>
        ///// <param name="limparEntity">Indica se a entidade deve ser limpada antes da transferencia</param>
        ///// <returns>Entidade preenchida</returns>
        //public override CLS_FrequenciaReuniaoResponsaveis DataRowToEntity(DataRow dr, CLS_FrequenciaReuniaoResponsaveis entity, bool limparEntity)
        //{
        //    return base.DataRowToEntity(dr, entity, limparEntity);
        //}

        #endregion Métodos Comentados
    }
}