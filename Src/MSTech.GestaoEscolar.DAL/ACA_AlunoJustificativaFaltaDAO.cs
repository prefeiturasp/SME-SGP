/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL.Abstracts;
using MSTech.GestaoEscolar.Entities;

namespace MSTech.GestaoEscolar.DAL
{
    /// <summary>
    ///
    /// </summary>
    public class ACA_AlunoJustificativaFaltaDAO : Abstract_ACA_AlunoJustificativaFaltaDAO
    {
        /// <summary>
        ///  Retorna um datatable contendo todos as justificativas
        ///  de falta do aluno que não foram excluídos logicamente,
        ///  filtrado por alu_id
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="paginado">Indica se será paginado</param>
        /// <param name="currentPage">Página atual</param>
        /// <param name="pageSize">Quantidade de registros por página</param>
        /// <param name="totalRecords">Total de registros retornado</param>
        /// <returns></returns>
        public List<ACA_AlunoJustificativaFalta> SelectBy_alu_id
            (
                Int64 alu_id
                , bool paginado
                , int currentPage
                , int pageSize
                , out int totalRecords
            )
        {
            totalRecords = 0;

            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoJustificativaFalta_SelectBy_Aluno", this._Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

                if (paginado)
                {
                    if (pageSize == 0) pageSize = 1;
                    totalRecords = qs.Execute(currentPage / pageSize, pageSize);
                }
                else
                {
                    qs.Execute();
                    totalRecords = qs.Return.Rows.Count;
                }

                List<ACA_AlunoJustificativaFalta> lt = qs.Return.Rows.Cast<DataRow>().Select(p => DataRowToEntity(p, new ACA_AlunoJustificativaFalta())).ToList();
                return lt;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Verifica se já existe uma justifica para o intervalo de data da justificativa que vai ser salva
        /// </summary>
        /// <param name="entity">Entidade ACA_AlunoJustificativaFalta</param>
        /// <returns>true se exister, false se não</returns>
        public bool VerificaIntervaloPeriodo(ACA_AlunoJustificativaFalta entity)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoJustificativaFalta_VerificaJustificativaIntervalo", this._Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = entity.alu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@afj_id";
                Param.Size = 4;
                Param.Value = entity.afj_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@afj_dataInicio";
                Param.Value = entity.afj_dataInicio;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@afj_dataFim";
                if (entity.afj_dataFim != new DateTime())
                    Param.Value = entity.afj_dataFim;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

                qs.Execute();

                return (qs.Return.Rows.Count > 0);
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Verifica se já esxiste uma avaliacao efetivada para o aluno no intervalo de data da justificativa que vai ser salva
        /// </summary>
        /// <param name="entity">Entidade ACA_AlunoJustificativaFalta</param>
        /// <param name="nomeAvaliacao">Nome da avaliação.</param>
        /// <returns>true se exister, false se não</returns>
        public bool VerificaAlunoAvaliacao(ACA_AlunoJustificativaFalta entity, out string nomeAvaliacao)
        {
            nomeAvaliacao = string.Empty;
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoJustificativaFalta_VerificaAlunoAvaliacao", this._Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = entity.alu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@afj_dataInicio";
                Param.Value = entity.afj_dataInicio;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@afj_dataFim";
                if (entity.afj_dataFim != new DateTime())
                    Param.Value = entity.afj_dataFim;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

                qs.Execute();

                nomeAvaliacao = String.Join( qs.Return.Rows.Count > 0 ? ", " : string.Empty,
                                (from DataRow Ava in qs.Return.Rows
                                 orderby Ava["ava_nome"]
                                 select Ava["ava_nome"].ToString()
                                 ).Distinct().ToArray()
                                 );
         
                // Configura nome das avaliações
                //foreach (DataRow row in qs.Return.Rows)
                //{
                //    nomeAvaliacao = String.Concat(nomeAvaliacao
                //        , (string.IsNullOrEmpty(nomeAvaliacao) ? string.Empty : ", ")
                //        , row["ava_nome"]);
                //}

                return (qs.Return.Rows.Count > 0);
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Seleciona as justificativas de falta dos alunos por mes e ano.
        /// </summary>
        /// <param name="mes">Mes de referência</param>
        /// <param name="ano">Ano de referência</param>
        /// <returns></returns>
        public DataTable SelecionaPorMesEAno(int mes, int ano)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoJustificativaFalta_SelecionaPorMesEAno", _Banco);
            //Sem limite de timeout
            qs.TimeOut = 0;

            DataTable dt = new DataTable();

            try
            {
                #region Parametros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@mes";
                Param.Size = 4;
                Param.Value = mes;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@ano";
                Param.Size = 4;
                Param.Value = ano;
                qs.Parameters.Add(Param);

                #endregion Parametros

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                {
                    dt = qs.Return;
                }

                return dt;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna os dados das justificativas do aluno.
        /// </summary>
        /// <param name="ano">Ano das justificativas</param>
        /// <param name="alu_id">Id do aluno</param>
        /// <returns></returns>
        public DataTable SelectJustificativasBy_Aluno(int ano, long alu_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoJustificativaFalta_SelectJustificativasBy_Aluno", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.ParameterName = "@ano";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.Value = ano;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;
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

        /// <summary>
        /// Configura os parametros do metodo de Inserir
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_AlunoJustificativaFalta entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@afj_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@afj_dataAlteracao"].Value = DateTime.Now;

            if (qs.Parameters["@pro_id"].Value.ToString() == Guid.Empty.ToString())
            {
                qs.Parameters["@pro_id"].Value = DBNull.Value;
            }
        }

        /// <summary>
        /// Configura os parametros do metodo de Alterar
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_AlunoJustificativaFalta entity)
        {
            base.ParamAlterar(qs, entity);

            // Remove o id do protocolo, pois não pode ser alterado.
            qs.Parameters.RemoveAt("@pro_id");

            qs.Parameters.RemoveAt("@afj_dataCriacao");

            qs.Parameters["@afj_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Inseri os valores da classe em um registro ja existente
        /// </summary>
        /// <param name="entity">Entidade com os dados a serem modificados</param>
        /// <returns>True - Operacao bem sucedida</returns>
        protected override bool Alterar(ACA_AlunoJustificativaFalta entity)
        {
            __STP_UPDATE = "NEW_ACA_AlunoJustificativaFalta_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Exclui lógicamente um registro do banco
        /// </summary>
        /// <param name="entity">Entidade com os dados a serem apagados</param>
        /// <returns>True - Operacao bem sucedida</returns>
        public override bool Delete(ACA_AlunoJustificativaFalta entity)
        {
            __STP_DELETE = "NEW_ACA_AlunoJustificativaFalta_Update_Situacao";
            return base.Delete(entity);
        }
    }
}