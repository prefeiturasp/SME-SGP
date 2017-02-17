/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using System;
    using System.Data;
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using System.Data.SqlClient;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using MSTech.GestaoEscolar.DAL;
    using System.Text;
    /// <summary>
    /// 
    /// </summary>
    public class ACA_AlunoHistoricoDAO : AbstractACA_AlunoHistoricoDAO
    {
        #region Métodos

        /// <summary>
        /// Retorna um DataTable retorna as disciplinas do currículo
        /// utilizado no cadastro de histórico. Não trás as eletivas
        /// do aluno.
        /// </summary>
        /// <param name="cur_id"></param>
        /// <param name="crr_id"></param>
        /// <param name="crp_id"></param>
        /// <param name="paginado"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns>DataTable contendo as disciplinas do currículo</returns>
        public DataTable SelecionaCurriculoDisciplina
        (
            int cur_id
            , int crr_id
            , int crp_id
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoHistorico_SelectBy_CurriculoDisciplina", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cur_id";
                Param.Size = 4;
                if (cur_id > 0)
                    Param.Value = cur_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crr_id";
                Param.Size = 4;
                if (crr_id > 0)
                    Param.Value = crr_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                if (crp_id > 0)
                    Param.Value = crp_id;
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
        /// Seleciona as disciplinas para o histórico configurado no ano letivo informado 
        /// pelo tipo de curriculo período e tipo nível de ensino.
        /// </summary>
        /// <param name="tcp_id"></param>
        /// <param name="chp_anoLetivo"></param>
        /// <param name="tne_id"></param>
        public DataTable Seleciona_TipoCurriculoPeriodoAnoLetivo
        (
            int tcp_id
            , int chp_anoLetivo
            , int tne_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoHistorico_SelectBy_TipoCurriculoPeriodoAnoLetivo", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tcp_id";
                Param.Size = 4;
                Param.Value = tcp_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@chp_anoLetivo";
                Param.Size = 4;
                Param.Value = chp_anoLetivo;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tne_id";
                Param.Size = 4;
                Param.Value = tne_id;
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
        /// Retorna um datatable contendo todos Historicos do Aluno
        /// que não foram excluídos logicamente, filtrados por 
        /// alu_id
        /// </summary>
        /// <param name="alu_id">Id da tabela ACA_Aluno do bd</param>   
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        /// <param name="totalRecords">Total de registros retornado na busca</param>
        /// <returns>DataTable com os Historico do Aluno</returns>
        public DataTable SelectBy_alu_id
        (
            long alu_id
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {            
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoHistorico_SelectBy_alu_id", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                if (alu_id > 0)
                    Param.Value = alu_id;
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
        /// Retorna um datatable contendo todos Historicos do Aluno
        /// que não foram excluídos logicamente, filtrados por 
        /// alu_id e ano letivo
        /// </summary>
        /// <param name="alu_id">Id da tabela ACA_Aluno do bd</param> 
        /// <param name="alh_anoLetivo">Ano letivo do historico</param>  
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        /// <param name="totalRecords">Total de registros retornado na busca</param>
        /// <returns>DataTable com os Historico do Aluno</returns>
        public DataTable SelectBy_alu_id_alh_anoLetivo
        (
            long alu_id
            , int alh_anoLetivo
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoHistorico_SelectBy_alu_id_alh_anoLetivo", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@alh_anoLetivo";
                Param.Size = 4;
                Param.Value = alh_anoLetivo;
                qs.Parameters.Add(Param);

                #endregion

                if (paginado)
                    totalRecords = qs.Execute(currentPage, pageSize);
                else
                {
                    qs.Execute();
                    totalRecords = qs.Return.Rows.Count;
                }

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
        /// Retorna um datatable contendo todos Historicos do Aluno
        /// que não foram excluídos logicamente, filtrados por 
        /// alu_id e tipo curriculo periodo
        /// </summary>
        /// <param name="alu_id">Id da tabela ACA_Aluno do bd</param> 
        /// <param name="tcp_id">Id da tabela ACA_TipoCurriculoPeriodo</param>  
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        /// <param name="totalRecords">Total de registros retornado na busca</param>
        /// <returns>DataTable com os Historico do Aluno</returns>
        public DataTable SelectBy_alu_id_tcp_id
        (
            long alu_id
            , int tcp_id
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoHistorico_SelectBy_alu_id_tcp_id", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tcp_id";
                Param.Size = 4;
                Param.Value = tcp_id;
                qs.Parameters.Add(Param);

                #endregion

                if (paginado)
                    totalRecords = qs.Execute(currentPage, pageSize);
                else
                {
                    qs.Execute();
                    totalRecords = qs.Return.Rows.Count;
                }

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
        /// Retorna um datatable contendo todos Historicos do Aluno
        /// que não foram excluídos logicamente, filtrados por 
        /// alu_id e historico id
        /// </summary>
        /// <param name="alu_id">Id da tabela ACA_Aluno do bd</param> 
        /// <param name="alh_id">Id do historico do aluno</param>  
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        /// <param name="totalRecords">Total de registros retornado na busca</param>
        /// <returns>DataTable com os Historico do Aluno</returns>
        public DataTable SelectBy_alu_id_alh_id
        (
            long alu_id
            , int alh_id
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoHistorico_SelectBy_alu_id_alh_id", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@alh_id";
                Param.Size = 4;
                Param.Value = alh_id;
                qs.Parameters.Add(Param);

                #endregion

                if (paginado)
                    totalRecords = qs.Execute(currentPage, pageSize);
                else
                {
                    qs.Execute();
                    totalRecords = qs.Return.Rows.Count;
                }

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
        /// Retorna um datatable contendo todos Historicos do Aluno
        /// que não foram excluídos logicamente, filtrados por 
        /// alu_id (obrigatoriamente > 0)
        /// Stored Procedure derivada da SelectBy_alu_id, porém otimizada para trazer apenas os campos necessários para carergar o grid
        /// </summary>
        /// <param name="alu_id">Id da tabela ACA_Aluno do bd</param>   
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        /// <param name="totalRecords">Total de registros retornado na busca</param>
        /// <returns>DataTable com os Historico do Aluno</returns>
        public DataTable SelectHistorico_PorAluno
        (
            long alu_id
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoHistorico_SelecionaPorAluno", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;
                qs.Parameters.Add(Param);

                #endregion

                if (paginado)
                    totalRecords = qs.Execute(currentPage, pageSize);
                else
                {
                    qs.Execute();
                    totalRecords = qs.Return.Rows.Count;
                }

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
        /// Retorna um datatable contendo todos Historicos do Aluno
        /// para carregar a tela de historicos resultado final do aluno
        /// </summary>
        /// <param name="alu_id">Id da tabela ACA_Aluno do bd</param>   
        /// <param name="chp_anoLetivo">Ano letivo para pegar os tipos de curriculo periodo</param>
        /// <param name="tne_id">Id do tipo nivel ensino do fundamental</param>
        /// <returns>DataTable com os Historico do Aluno</returns>
        public DataTable SelecionaHistoricoResFinal
        (
            long alu_id
            , int chp_anoLetivo
            , int tne_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoHistorico_SelecionaResultadoFinal", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@chp_anoLetivo";
                Param.Size = 4;
                Param.Value = chp_anoLetivo;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tne_id";
                Param.Size = 4;
                Param.Value = tne_id;
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
        /// Retorna os estudos do aluno para o histórico pedagógico
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <returns>DataTable contendo os estudos</returns>
        public DataTable Seleciona_Estudos
        (
            long alu_id,
            int chp_anoLetivo,
            int tne_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_Relatorio_0005_HistoricoEscolarEstudos", _Banco);

            #region Parâmetros

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@alu_id";
            Param.Size = 8;
            Param.Value = alu_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@chp_anoLetivo";
            Param.Size = 4;
            Param.Value = chp_anoLetivo;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tne_id";
            Param.Size = 4;
            Param.Value = tne_id;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna a grade de históricos do aluno para o histórico pedagógico
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <returns>DataTable contendo as grades de históricos</returns>
        public DataTable Seleciona_Grade
        (
            long alu_id,
            int chp_anoLetivo,
            int tne_id,
            int param_idForaRede
        )
        { 
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_Relatorio_0005_HistoricoEscolarGrade", _Banco);

            #region Parâmetros

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@alu_id";
            Param.Size = 8;
            Param.Value = alu_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@chp_anoLetivo";
            Param.Size = 4;
            Param.Value = chp_anoLetivo;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tne_id";
            Param.Size = 4;
            Param.Value = tne_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@aco_idForaRede";
            Param.Size = 4;
            Param.Value = param_idForaRede;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna a transferencia no histórico do aluno para o histórico pedagógico
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="mtu_id">Id da matricula turma</param>
        /// <returns>DataTable contendo a transferencia histórico</returns>
        public DataTable Seleciona_Transferencia
        (
            long alu_id,
            int mtu_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_Relatorio_0005_HistoricoEscolarTransferencia", _Banco);

            #region Parâmetros

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@alu_id";
            Param.Size = 8;
            Param.Value = alu_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@mtu_id";
            Param.Size = 4;
            Param.Value = mtu_id;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Seleciona histórico do aluno por matrícula turma.
        /// </summary>
        /// <param name="alu_id">ID do aluno.</param>
        /// <param name="mtu_id">ID da matrícula turma.</param>
        /// <returns></returns>
	    public ACA_AlunoHistorico SelecionaPorMatriculaTurma(long alu_id, int mtu_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoHistorico_SelectBy_alu_id_mtu_id", _Banco);
            ACA_AlunoHistorico entity = new ACA_AlunoHistorico();

	        try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@mtu_id";
                Param.Size = 4;
                Param.Value = mtu_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                {
                    entity = DataRowToEntity(qs.Return.Rows[0], entity);
                }

                return entity;
            }
	        finally
	        {
	            qs.Parameters.Clear();
	        }
        }

        /// <summary>
        /// Atualiza as notas e as frequência do histórico global e das disciplinas de um aluno.
        /// </summary>
        /// <param name="alu_id">ID do aluno.</param>
        /// <param name="mtu_id">ID da matrícula do aluno.</param>
        /// <param name="anoLetivoHistorico">Ano do histórico.</param>
        /// <param name="mtu_resultado">Resultado final global do aluno.</param>
        /// <param name="permiteAlterarResultado">Flag que indica se o sistema está configurado para possibiliar a mudança do resultado final do aluno.</param>
        /// <returns></returns>
        public bool CalcularNotaFrequenciaHistorico(long alu_id, int mtu_id, int anoLetivoHistorico, byte mtu_resultado, bool permiteAlterarResultado)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_ACA_AlunoHistorico_AtualizarNotaFrequencia", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@mtu_id";
                Param.Size = 4;
                Param.Value = mtu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@anoLetivoHistorico";
                Param.Size = 4;
                Param.Value = anoLetivoHistorico;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@mtu_resultado";
                Param.Size = 1;
                if (mtu_resultado > 0)
                    Param.Value = mtu_resultado;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@permiteAlterarResultado";
                Param.Size = 1;
                Param.Value = permiteAlterarResultado;
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

        /// <summary>
        /// Gera/atualiza o histórico por aluno.
        /// </summary>
        /// <param name="alu_id">ID do aluno.</param>
        /// <param name="mtu_id">ID da matrícula turma do aluno.</param>
        /// <returns></returns>
        public bool GeracaoHistoricoPedagogicoPorAluno(long alu_id, int mtu_id)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_ACA_AlunoHistorico_GeracaoHistoricoPedagogicoPorAluno", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@mtu_id";
                Param.Size = 4;
                Param.Value = mtu_id;
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

        public DataTable ComparaDisciplinas(long alu_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoHistoricoDisciplina_Compara", _Banco);

            try
            {
                #region Parâmetros

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

        public void RetiraDisciplinas(long alu_id, int ahd_id, int alh_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_AlunoHistoricoDisciplina_RetiraDisciplinas", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@ahd_id";
                Param.Size = 8;
                Param.Value = ahd_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alh_id";
                Param.Size = 8;
                Param.Value = alh_id;
                qs.Parameters.Add(Param);
                #endregion

                qs.Execute();


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

        #region Salvar em lote

        /// <summary>
        /// Salva os dados do historico em lote.
        /// </summary>
        /// <param name="dtAlunoHistorico">Datatable com os dados do historico.</param>
        /// <param name="dtAlunoHistoricoDisciplina">Datatable com os dados do historico por disciplina.</param>
        /// <returns></returns>
        public bool SalvarEmLote(DataTable dtAlunoHistorico, DataTable dtAlunoHistoricoDisciplina)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_ACA_AlunoHistorico_SalvarEmLote", _Banco);

            try
            {
                #region Parâmetro

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@AlunoHistorico";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_AlunoHistorico";
                sqlParam.Value = dtAlunoHistorico;
                qs.Parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@AlunoHistoricoDisciplina";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_AlunoHistoricoDisciplina";
                sqlParam.Value = dtAlunoHistoricoDisciplina;
                qs.Parameters.Add(sqlParam);

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

        #endregion

        #region Métodos sobrescritos

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        /// <returns>TRUE - Se entity.ParametroId > 0</returns>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_AlunoHistorico entity)
        {
            if (entity != null & qs != null)
            {
                entity.alh_id = Convert.ToInt32(qs.Return.Rows[0][0]);
                return (entity.alh_id > 0);
            }

            return false;
        }		

        /// <summary>
        /// Override do método inserir
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_AlunoHistorico entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@alh_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@alh_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_AlunoHistorico entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@alh_dataCriacao");
            qs.Parameters["@alh_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity"> Entidade ACA_AlunoHistorico</param>
        /// <returns>true = sucesso | false = fracasso</returns>  
        protected override bool Alterar(ACA_AlunoHistorico entity)
        {
            __STP_UPDATE = "NEW_ACA_AlunoHistorico_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        protected override void ParamDeletar(QueryStoredProcedure qs, ACA_AlunoHistorico entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@alu_id";
            Param.Size = 8;
            Param.Value = entity.alu_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@alh_id";
            Param.Size = 4;
            Param.Value = entity.alh_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@alh_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@alh_dataAlteracao";
            Param.Size = 8;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity"> Entidade ACA_AlunoHistorico</param>
        /// <returns>true = sucesso | false = fracasso</returns>        
        public override bool Delete(ACA_AlunoHistorico entity)
        {
            __STP_DELETE = "NEW_ACA_AlunoHistorico_Update_Situacao";
            return base.Delete(entity);
        }

        #endregion

        #region Comentados

        ///// <summary>
        ///// Inseri os valores da classe em um registro ja existente
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem modificados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Alterar(ACA_AlunoHistorico entity)
        //{
        //    return base.Alterar(entity);
        //}
        ///// <summary>
        ///// Inseri os valores da classe em um novo registro
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem inseridos</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Inserir(ACA_AlunoHistorico entity)
        //{
        //    return base.Inserir(entity);
        //}
        ///// <summary>
        ///// Carrega um registro da tabela usando os valores nas chaves
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem carregados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Carregar(ACA_AlunoHistorico entity)
        //{
        //    return base.Carregar(entity);
        //}
        ///// <summary>
        ///// Exclui um registro do banco
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem apagados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Delete(ACA_AlunoHistorico entity)
        //{
        //    return base.Delete(entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Alterar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamAlterar(QueryStoredProcedure qs, ACA_AlunoHistorico entity)
        //{
        //    base.ParamAlterar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Carregar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_AlunoHistorico entity)
        //{
        //    base.ParamCarregar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Deletar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamDeletar(QueryStoredProcedure qs, ACA_AlunoHistorico entity)
        //{
        //    base.ParamDeletar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Inserir
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_AlunoHistorico entity)
        //{
        //    base.ParamInserir(qs, entity);
        //}
        ///// <summary>
        ///// Salva o registro no banco de dados
        ///// </summary>
        ///// <param name="entity">Entidade com os dados para preenchimento para inserir ou alterar</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Salvar(ACA_AlunoHistorico entity)
        //{
        //    return base.Salvar(entity);
        //}
        ///// <summary>
        ///// Realiza o select da tabela
        ///// </summary>
        ///// <returns>Lista com todos os registros da tabela</returns>
        //public override IList<ACA_AlunoHistorico> Select()
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
        //public override IList<ACA_AlunoHistorico> Select_Paginado(int currentPage, int pageSize, out int totalRecord)
        //{
        //    return base.Select_Paginado(currentPage, pageSize, out totalRecord);
        //}
        ///// <summary>
        ///// Recebe o valor do auto incremento e coloca na propriedade 
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_AlunoHistorico entity)
        //{
        //    return base.ReceberAutoIncremento(qs, entity);
        //}
        ///// <summary>
        ///// Passa os dados de um datatable para uma entidade
        ///// </summary>
        ///// <param name="dr">DataRow do datatable preenchido</param>
        ///// <param name="entity">Entidade onde ser�o transferidos os dados</param>
        ///// <returns>Entidade preenchida</returns>
        //public override ACA_AlunoHistorico DataRowToEntity(DataRow dr, ACA_AlunoHistorico entity)
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
        //public override ACA_AlunoHistorico DataRowToEntity(DataRow dr, ACA_AlunoHistorico entity, bool limparEntity)
        //{
        //    return base.DataRowToEntity(dr, entity, limparEntity);
        //}

        #endregion
    }
}