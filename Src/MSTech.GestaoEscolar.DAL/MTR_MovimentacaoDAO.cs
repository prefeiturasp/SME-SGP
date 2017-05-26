/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL.Abstracts;
using MSTech.GestaoEscolar.Entities;

namespace MSTech.GestaoEscolar.DAL
{
    /// <summary>
    ///
    /// </summary>
    public class MTR_MovimentacaoDAO : Abstract_MTR_MovimentacaoDAO
    {
        #region Métodos

        /// <summary>
        /// Retorna as movimentações realizadas de acordo com os filtros
        /// </summary>
        /// <param name="uad_idSuperior">Id da unidade administrativa superior</param>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="uni_id">Id da unidade da escola</param>
        /// <param name="cur_id">Id do curso</param>
        /// <param name="crr_id">Id do curriculo do curso</param>
        /// <param name="alc_matricula">Matrícula do aluno</param>
        /// <param name="alc_matriculaEstadual">Matrícula estadual do aluno</param>
        /// <param name="tur_codigo">Codigo da turma</param>
        /// <param name="tipoBusca">Tipo de busca por nome do aluno</param>
        /// <param name="nome_aluno">Nome do aluno</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="adm">Indica se o usuário é administrador</param>
        /// <param name="usu_id">Id do usuário</param>
        /// <param name="gru_id">Id do grupo</param>
        /// <param name="dataInicio"></param>
        /// <param name="dataFim"></param>
        /// <param name="MostraCodigoEscola">Se mostra o codigo da escola</param>
        /// <param name="totalRecords">Total de registros retornado na busca</param>
        /// <returns>Tabela com movimentações do aluno</returns>
        public DataTable SelectBy_Pesquisa
        (
            Guid uad_idSuperior
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , string alc_matricula
            , string alc_matriculaEstadual
            , string tur_codigo
            , byte tipoBusca
            , string nome_aluno
            , Guid ent_id
            , bool adm
            , Guid usu_id
            , Guid gru_id
            , DateTime dataInicio
            , DateTime dataFim
            , bool MostraCodigoEscola
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_Movimentacao_Pesquisa", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                if (uad_idSuperior != Guid.Empty)
                    Param.Value = uad_idSuperior;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                if (esc_id > 0)
                    Param.Value = esc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@uni_id";
                Param.Size = 4;
                if (uni_id > 0)
                    Param.Value = uni_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

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
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alc_matricula";
                Param.Size = 50;
                if (!String.IsNullOrEmpty(alc_matricula))
                    Param.Value = alc_matricula;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tur_codigo";
                Param.Size = 30;
                if (!String.IsNullOrEmpty(tur_codigo))
                    Param.Value = tur_codigo;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alc_matriculaEstadual";
                Param.Size = 50;
                if (!String.IsNullOrEmpty(alc_matriculaEstadual))
                    Param.Value = alc_matriculaEstadual;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tipobusca";
                Param.Size = 1;
                Param.Value = tipoBusca;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@nome_aluno";
                Param.Size = 200;
                if (!String.IsNullOrEmpty(nome_aluno))
                    Param.Value = nome_aluno;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                if (ent_id != Guid.Empty)
                    Param.Value = ent_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@adm";
                Param.Size = 1;
                Param.Value = adm;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@usu_id";
                Param.Size = 16;
                if (usu_id != Guid.Empty)
                    Param.Value = usu_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@gru_id";
                Param.Size = 16;
                if (gru_id != Guid.Empty)
                    Param.Value = gru_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Date;
                Param.ParameterName = "@dataInicio";
                Param.Size = 20;
                Param.Value = dataInicio;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Date;
                Param.ParameterName = "@dataFim";
                Param.Size = 20;
                Param.Value = dataFim;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@MostraCodigoEscola";
                Param.Size = 1;
                Param.Value = MostraCodigoEscola;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();
                totalRecords = qs.Return.Rows.Count;

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
        /// Retorna as movimentações realizadas para o aluno.(metodo sobrecarga para UCMovimentação!)
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="MostraCodigoEscola"></param>
        /// <param name="totalRecords">Total de registros retornado na busca</param>
        /// <returns>Tabela com movimentações do aluno</returns>
        public DataTable SelectBy_Aluno_Movimentacao(long alu_id, Guid ent_id, bool MostraCodigoEscola, out int totalRecords)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_Movimentacao_PesquisaAluno", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@alu_id";
            Param.Size = 8;
            Param.Value = alu_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@ent_id";
            Param.Size = 16;
            Param.Value = ent_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@MostraCodigoEscola";
            Param.Size = 1;
            Param.Value = MostraCodigoEscola;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();
            totalRecords = qs.Return.Rows.Count;

            return qs.Return;
        }

        /// <summary>
        /// Retorna os dados da movimentação de um aluno
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <returns></returns>
        public DataTable SelectBy_AlunoMovimentacaoRetroativa(long alu_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_Movimentacao_SelectBy_AlunoMovimentacaoRetroativa", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@alu_id";
            Param.Size = 8;
            Param.Value = alu_id;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna os dados de uma movimentação do aluno
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="mov_id">ID da movimentação</param>
        /// <returns>Tabela com movimentações do aluno</returns>
        public DataTable SelectBy_alu_id_mov_id(long alu_id, int mov_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_Movimentacao_SelectBy_alu_id_mov_id", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@alu_id";
            Param.Size = 8;
            Param.Value = alu_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@mov_id";
            Param.Size = 4;
            Param.Value = mov_id;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna todas as movimentação ativas apartir
        /// do tipo de movimento
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="tmo_tipoMovimento">Tipo de movimento</param>
        /// <returns></returns>
        public List<MTR_Movimentacao> SelectBy_TipoMovimento(long alu_id, byte tmo_tipoMovimento)
        {
            List<MTR_Movimentacao> lt = new List<MTR_Movimentacao>();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_Movimentacao_SelectBy_TipoMovimento", _Banco);
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
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tmo_tipoMovimento";
                Param.Size = 1;
                Param.Value = tmo_tipoMovimento;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                foreach (DataRow dr in qs.Return.Rows)
                {
                    MTR_Movimentacao entity = new MTR_Movimentacao();
                    lt.Add(DataRowToEntity(dr, entity));
                }

                return lt;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Seleciona as movimentações de saída realizadas em determinado ano em um dos dois meses do parâmetro.
        /// </summary>
        /// <param name="ent_id">ID da entidade.</param>
        /// <param name="mes1">Mês 1</param>
        /// <param name="mes2">Mês 2</param>
        /// <param name="ano">Ano</param>
        /// <returns>Movimentação dos meses/ano</returns>
        public List<MTR_Movimentacao> SelecionaMovimentacaoDeSaidaPorMesEAno(Guid ent_id, int mes1, int mes2, int ano)
        {
            List<MTR_Movimentacao> lt = new List<MTR_Movimentacao>();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_TipoMovimentacao_SelecionaMovimentacaoDeSaidaPorMesEAno", _Banco);
            //Sem limite de timeout
            qs.TimeOut = 0;

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@mes1";
                Param.Size = 4;
                Param.Value = mes1;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@mes2";
                Param.Size = 4;
                Param.Value = mes2;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@ano";
                Param.Size = 4;
                Param.Value = ano;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                foreach (DataRow dr in qs.Return.Rows)
                {
                    MTR_Movimentacao entity = new MTR_Movimentacao();
                    lt.Add(DataRowToEntity(dr, entity));
                }

                return lt;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Verifica o maior número de ordem cadastado de movimentação por aluno
        /// </summary>
        public int Select_MaiorOrdem(long alu_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_Movimentacao_Select_MaiorOrdem", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                return qs.Return.Rows.Count > 0 ? Convert.ToInt32(qs.Return.Rows[0][0]) : 0;
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
        /// Retorna a quantidade de faltas e de aulas por Global
        /// </summary>
        /// <param name="tur_id">Id da turma</param>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="mtu_id"></param>
        /// <param name="tpc_id">Id do tipo periodo</param>
        /// <param name="tipoLancamento">Id do tipo lançamento</param>
        /// <param name="dataInicial">Data inicial</param>
        /// <param name="dataFinal">Data final</param>
        public DataTable CalcularFrequenciaGlobal
        (
            long tur_id
            , long alu_id
            , int mtu_id
            , int tpc_id
            , byte tipoLancamento
            , DateTime dataInicial
            , DateTime dataFinal
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_Movimentacao_CalcularFrequenciaGlobal", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;
                qs.Parameters.Add(Param);

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
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                Param.Value = tpc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tipoLancamento";
                Param.Size = 1;
                Param.Value = tipoLancamento;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@dataInicial";
                Param.Size = 16;
                Param.Value = dataInicial;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@dataFinal";
                Param.Size = 16;
                Param.Value = dataFinal;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

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
        /// Retorna os dados da última movimentação do aluno
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        public DataTable Select_UltimaMovimentacaoAluno(long alu_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_Movimentacao_SelecionaUltimaMovimentacaoAluno", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;

                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                return (qs.Return);
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Seleciona as movimentações realizadas após a data informada.
        /// </summary>
        /// <param name="uad_idSuperior">ID da unidade superior.</param>
        /// <param name="esc_id">ID da escola.</param>
        /// <param name="uni_id">ID da unidade escolar.</param>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo.</param>
        /// <param name="crp_id">ID do grupamento.</param>
        /// <param name="alc_matricula">Matrícula do aluno.</param>
        /// <param name="apenasAtivos">Flag para indicar se busca alunos inativos.</param>
        /// <param name="dataMovimentacao">Data referência da busca.</param>
        /// <param name="paginado"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public DataTable SelecionaPorData
        (
            Guid uad_idSuperior
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , string alc_matricula
            , bool apenasAtivos
            , DateTime dataMovimentacao
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_Movimentacao_PesquisaData", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                Param.Value = uad_idSuperior;
                qs.Parameters.Add(Param);

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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                Param.Value = crp_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alc_matricula";
                Param.Size = 50;
                if (string.IsNullOrEmpty(alc_matricula))
                    Param.Value = DBNull.Value;
                else
                    Param.Value = alc_matricula;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@apenasAtivos";
                Param.Size = 1;
                Param.Value = apenasAtivos;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Date;
                Param.ParameterName = "@dataMovimentacao";
                Param.Size = 20;
                Param.Value = dataMovimentacao;
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
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Verifica se o aluno possui movimentações por tipo e ano.
        /// </summary>
        /// <param name="alu_id">Id do aluno.</param>
        /// <param name="tipoMovimentacao">Tipos de movimentações separados por ";"</param>
        /// <param name="ano">Ano do calendário escolar.</param>
        /// <returns></returns>
        public bool VerificaAlunoPossuiMovimentacaoSaidaEscola(long alu_id, int mtu_id)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_MTR_Movimentacao_VerificaAlunoPossuiMovimentacaoSaida", _Banco);

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

        /// <summary>
        /// Retorna os dados de algumas movimentações específicas do aluno.
        /// </summary>
        /// <param name="ano">Ano das movimentações</param>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="mtu_id">Id da matrícula do aluno na turma</param>
        /// <returns></returns>
        public DataTable SelectMovimentacoesEspecificasBy_Aluno(int ano, long alu_id, int mtu_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_Movimentacao_SelectMovimentacoesEspecificasBy_Aluno", _Banco);
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

                Param = qs.NewParameter();
                Param.ParameterName = "@mtu_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                if (mtu_id > 0)
                {
                    Param.Value = mtu_id;
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
            catch
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion Métodos


        #region Sobrescritos

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

        #endregion Sobrescritos
    }
}