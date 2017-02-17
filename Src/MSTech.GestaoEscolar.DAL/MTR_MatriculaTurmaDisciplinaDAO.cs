/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL.Abstracts;
using MSTech.GestaoEscolar.Entities;
using System.Linq;

namespace MSTech.GestaoEscolar.DAL
{
	/// <summary>
    /// Classe MTR_MatriculaTurmaDisciplinaDAO
	/// </summary>
	public class MTR_MatriculaTurmaDisciplinaDAO : Abstract_MTR_MatriculaTurmaDisciplinaDAO
    {
        #region Métodos

        /// <summary>
        /// Retorna as turmas eletivas do aluno.
        /// </summary>
        /// <param name="alu_id">Id do aluno.</param>
        /// <returns>Turmas eletivas.</returns>
        public DataTable PesquisarTurmasEletivas
        (
            long alu_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_PesquisarTurmasEletivas", _Banco);

            #region Parâmetros

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@alu_id";
            Param.Size = 8;
            Param.Value = alu_id;
            qs.Parameters.Add(Param);

            #endregion Parâmetros

            qs.Execute();
            return qs.Return;
        }

        /// <summary>
        /// Calcula a média das notas do aluno por turma e disciplina
        /// </summary>
        /// <param name="tur_id">Id do turma</param>
        /// <param name="tud_id">Id da disciplina da turma</param>
        /// <param name="tpc_id">Id do período do calendário</param>
        /// <returns>Nota média dos alunos da turma</returns>
        public DataTable CalculaNota_Media_PorTurmaDisciplina
        (
            long tur_id
            , long tud_id
            , int tpc_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_CalculaNota_Media_PorTurmaDisciplina", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            Param.Value = tud_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tpc_id";
            Param.Size = 4;
            Param.Value = tpc_id;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Verifica as  MatriculaTurmaDisciplina cadastradas para um tud_id de uma turma do 
        /// tipo 2-Eletiva do aluno, e retorna os tur_ids das turmas eletivas.
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="mtu_id">ID da MatriculaTurma</param>
        /// <returns></returns>
        public DataTable SelectTurmasBy_TurmaEletivaAluno
        (
            long alu_id
            , int mtu_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_SelectBy_TurmaEletiva", _Banco);

            #region PARAMETROS

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
        /// Verifica as  MatriculaTurmaDisciplina cadastradas para um tud_id de uma turma do 
        /// tipo 2-Eletiva do aluno, e retorna os tud_ids das turmas eletivas.
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="mtu_id">ID da MatriculaTurma</param>
        /// <returns></returns>
        public DataTable SelectTurmasDisciplinasBy_TurmaEletivaAluno
        (
            long alu_id
            , int mtu_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_Select_tud_id_By_TurmaEletiva", _Banco);

            #region PARAMETROS

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
        /// Verifica as  MatriculaTurmaDisciplina cadastradas para um tud_id de uma turma do 
        /// tipo 2-Eletiva do aluno, e retorna os tud_ids das turmas eletivas.
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="mtu_id">ID da MatriculaTurma</param>
        /// <returns></returns>
        public DataTable SelecionaTurmaDisciplinaEletiva_MatriculaAtiva
        (
            long alu_id
            , int mtu_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_SelecionaTurmaDisciplinaEletiva_MatriculaAtiva", _Banco);

            #region PARAMETROS

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
        /// Retorna a turma do tipo 4-Multisseriada do docente com alunos ativos,
        /// que estejam relacionados com a MatriculaTurma passada por parametro.
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="mtu_id">ID da MatriculaTurma</param>
        /// <returns></returns>
        public DataTable SelecionaTurmaDisciplinaMultisseriadaDocente_MatriculaAtiva
        (
            long alu_id
            , int mtu_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_SelecionaDisciplinaMultisseriadaDocente_MatriculaAtiva", _Banco);

            #region PARAMETROS

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
        /// Retorna as MatriculaTurmaDisciplina para a MatriculaTurma.
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="mtu_id">ID da MatriculaTurma</param>
        /// <returns></returns>
        public DataTable SelectBy_MatriculaTurma
        (
            long alu_id
            , int mtu_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_SelectBy_MatriculaTurma", _Banco);

            #region PARAMETROS

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
        /// Retorna as MatriculaTurmaDisciplina por turma disciplina.
        /// </summary>
        /// <param name="tud_ids">Lista de tud_ids.</param>
        /// <returns></returns>
        public DataTable SelecionaMatriculasPorTurmaDisciplina
        (
            string tud_ids
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_SelecionaMatriculasTurmaDisciplina", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@tud_ids";
            Param.Value = tud_ids;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna as MatriculaTurmaDisciplina Ativas para a MatriculaTurma.
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="mtu_id">ID da MatriculaTurma</param>
        /// <returns></returns>
        public DataTable SelecionaMatriculaTurmaDisciplinaAtiva
        (
            long alu_id
            , int mtu_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_SelecionaMatriculaTurmaDisciplinaAtiva", _Banco);

            #region PARAMETROS

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
        /// Retorna a média de todos os alunos na disciplina e no período informados.
        /// </summary>
        /// <param name="tud_id">ID da disciplina na turma</param>
        /// <param name="fav_id">Formato de avaliação</param>
        /// <param name="tpc_id">Tipo de período do calendário</param>
        /// <param name="tipoEscalaAvaliacaoAdicional">Tipo de escala da avaliação adicional</param>
        /// <param name="tipoEscalaDocente">Tipo de escala de avaliação do docente</param>
        /// <returns></returns>
        public DataTable CalculaMediaAvaliacaoAdicional_TodosAlunos
        (
            long tud_id
            , int fav_id
            , int tpc_id
            , byte tipoEscalaAvaliacaoAdicional
            , byte tipoEscalaDocente
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_CalculaMediaAvaliacaoAdicional_TodosAlunos", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            Param.Value = tud_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@fav_id";
            Param.Size = 4;
            Param.Value = fav_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tpc_id";
            Param.Size = 4;
            Param.Value = tpc_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoEscalaAvaliacaoAdicional";
            Param.Size = 1;
            Param.Value = tipoEscalaAvaliacaoAdicional;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoEscalaDocente";
            Param.Size = 1;
            Param.Value = tipoEscalaDocente;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna a média do aluno na disciplina e no período informados.
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="mtu_id">ID da matricula na turma</param>
        /// <param name="mtd_id">ID da matricula na turma disciplina</param>
        /// <param name="tud_id">ID da disciplina na turma</param>
        /// <param name="fav_id">Formato de avaliação</param>
        /// <param name="tpc_id">Tipo de período do calendário</param>
        /// <param name="tipoEscalaAvaliacaoAdicional">Tipo de escala da avaliação adicional</param>
        /// <param name="tipoEscalaDocente">Tipo de escala de avaliação do docente</param>
        /// <returns></returns>
        public decimal CalculaMediaAvaliacaoAdicional_Alunos
        (
            long alu_id
            , int mtu_id
            , int mtd_id
            , long tud_id
            , int fav_id
            , int tpc_id
            , byte tipoEscalaAvaliacaoAdicional
            , byte tipoEscalaDocente
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_CalculaMediaAvaliacaoAdicional_Alunos", _Banco);

            #region PARAMETROS

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
            Param.ParameterName = "@mtd_id";
            Param.Size = 4;
            Param.Value = mtd_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            Param.Value = tud_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@fav_id";
            Param.Size = 4;
            Param.Value = fav_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tpc_id";
            Param.Size = 4;
            Param.Value = tpc_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoEscalaAvaliacaoAdicional";
            Param.Size = 1;
            Param.Value = tipoEscalaAvaliacaoAdicional;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoEscalaDocente";
            Param.Size = 1;
            Param.Value = tipoEscalaDocente;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return.Rows.Count > 0 ? Convert.ToDecimal(qs.Return.Rows[0]["Avaliacao"]) : Convert.ToDecimal("0,00");
        }

	    /// <summary>
	    /// Retorna os alunos matriculados na TurmaDisciplina, com os dados de frequência
	    /// e notas lançados no período.
	    /// </summary>
	    /// <param name="tud_id">ID da disciplina na turma</param>
	    /// <param name="tur_id">Id da turma.</param>
	    /// <param name="tpc_id">Tipo de período do calendário</param>
	    /// <param name="ava_id">ID da avaliação</param>
	    /// <param name="ordenacao">Ordenação da busca</param>
	    /// <param name="fav_id">Formato de avaliação</param>
	    /// <param name="tipoAvaliacao">Tipo de avaliação</param>
	    /// <param name="esa_id">ID da escala de avaliação utilizada</param>
	    /// <param name="tipoEscalaDisciplina">Tipo de escala de avaliação da disciplina</param>
	    /// <param name="tipoEscalaDocente">Tipo de escala de avaliação do docente</param>
	    /// <param name="avaliacaoesRelacionadas">Avaliações relacionadas à avaliação informada (ids separados por ",")</param>
	    /// <param name="notaMinimaAprovacao">Nota mínima de aprovação (se escala for numérica)</param>
	    /// <param name="ordemParecerMinimo">Ordem do parecer mínimo da aprovação (se escala for por pareceres)</param>
	    /// <param name="tipoLancamento">Tipo de lançamento de frequência</param>
	    /// <param name="fav_calculoQtdeAulasDadas">Cálculo da quantidade de aulas dadas</param>
	    /// <param name="permiteAlterarResultado">Indica se o usuário pode alterar o resultado do aluno</param>
	    /// <param name="tur_tipo">Tipo de turma</param>
	    /// <param name="cal_id">ID do calendário da turma</param>
        /// <param name="exibirNotaFinal">Indica se deve buscar a média das notas finais salvas na tela de avaliação</param>
        /// <returns></returns>
	    public DataTable SelectBy_TurDiscPeriodoFormato
        (
            long tud_id
            , long tur_id
            , int tpc_id
            , int ava_id
            , int ordenacao
            , int fav_id
            , byte tipoAvaliacao
            , int esa_id
            , byte tipoEscalaDisciplina
            , byte tipoEscalaDocente
            , string avaliacaoesRelacionadas
            , double notaMinimaAprovacao
            , int ordemParecerMinimo
            , byte tipoLancamento
            , byte fav_calculoQtdeAulasDadas
            , bool permiteAlterarResultado
            , byte tur_tipo
            , int cal_id
            , bool exibirNotaFinal
            , byte tud_tipo
            , int tpc_ordem
            , decimal fav_variacao
            , bool ExibeCompensacao
            , DataTable dtTurma
            , bool documentoOficial
        )
        {            
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_SelectBy_TurmaDisciplinaFormato", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            Param.Value = tud_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tpc_id";
            Param.Size = 4;
            if (tpc_id > 0)
                Param.Value = tpc_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ava_id";
            Param.Size = 4;
            if (ava_id > 0)
                Param.Value = ava_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ordenacao";
            Param.Size = 4;
            Param.Value = ordenacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@fav_id";
            Param.Size = 4;
            Param.Value = fav_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoAvaliacao";
            Param.Size = 1;
            Param.Value = tipoAvaliacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@esa_id";
            Param.Size = 4;
            Param.Value = esa_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoEscalaDisciplina";
            Param.Size = 1;
            Param.Value = tipoEscalaDisciplina;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoEscalaDocente";
            Param.Size = 1;
            Param.Value = tipoEscalaDocente;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@avaliacaoesRelacionadas";
            if (string.IsNullOrEmpty(avaliacaoesRelacionadas))
                Param.Value = DBNull.Value;
            else
                Param.Value = avaliacaoesRelacionadas;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Decimal;
            Param.ParameterName = "@notaMinimaAprovacao";
            Param.Size = 20;
            Param.Value = notaMinimaAprovacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ordemParecerMinimo";
            Param.Size = 4;
            Param.Value = ordemParecerMinimo;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoLancamento";
            Param.Size = 1;
            Param.Value = tipoLancamento;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@fav_calculoQtdeAulasDadas";
            Param.Size = 1;
            Param.Value = fav_calculoQtdeAulasDadas;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@permiteAlterarResultado";
            Param.Size = 1;
            Param.Value = permiteAlterarResultado;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tur_tipo";
            Param.Size = 1;
            Param.Value = tur_tipo;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            Param.Value = cal_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@exibirNotaFinal";
            Param.Size = 1;
            Param.Value = exibirNotaFinal;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tud_tipo";
            Param.Size = 1;
            Param.Value = tud_tipo;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tpc_ordem";
            Param.Size = 4;
            Param.Value = tpc_ordem;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Decimal;
            Param.ParameterName = "@fav_variacao";
            Param.Size = 20;
            Param.Value = fav_variacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@ExibeCompensacao";
            Param.Size = 1;
            Param.Value = ExibeCompensacao;
            qs.Parameters.Add(Param);

            SqlParameter sqlParam = new SqlParameter();
            sqlParam.SqlDbType = SqlDbType.Structured;
            sqlParam.ParameterName = "@dtTurma";
            sqlParam.TypeName = "TipoTabela_Turma";
            sqlParam.Value = dtTurma;
            qs.Parameters.Add(sqlParam);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@documentoOficial";
            Param.Size = 1;
            Param.Value = documentoOficial;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna os alunos matriculados na TurmaDisciplina, com os dados de frequência
        /// e notas lançados no período.
        /// </summary>
        /// <param name="tud_id">ID da disciplina na turma</param>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="tpc_id">Tipo de período do calendário</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="ordenacao">Ordenação da busca</param>
        /// <param name="fav_id">Formato de avaliação</param>
        /// <param name="tipoAvaliacao">Tipo de avaliação</param>
        /// <param name="esa_id">ID da escala de avaliação utilizada</param>
        /// <param name="tipoEscalaDisciplina">Tipo de escala de avaliação da disciplina</param>
        /// <param name="tipoEscalaDocente">Tipo de escala de avaliação do docente</param>
        /// <param name="avaliacaoesRelacionadas">Avaliações relacionadas à avaliação informada (ids separados por ",")</param>
        /// <param name="notaMinimaAprovacao">Nota mínima de aprovação (se escala for numérica)</param>
        /// <param name="ordemParecerMinimo">Ordem do parecer mínimo da aprovação (se escala for por pareceres)</param>
        /// <param name="tipoLancamento">Tipo de lançamento de frequência</param>
        /// <param name="fav_calculoQtdeAulasDadas">Cálculo da quantidade de aulas dadas</param>
        /// <param name="permiteAlterarResultado">Indica se o usuário pode alterar o resultado do aluno</param>
        /// <param name="tur_tipo">Tipo de turma</param>
        /// <param name="cal_id">ID do calendário da turma</param>
        /// <param name="exibirNotaFinal">Indica se deve buscar a média das notas finais salvas na tela de avaliação</param>
        /// <param name="tdc_id">ID to tipo de docente</param>
        /// <returns></returns>
        public DataTable SelectBy_TurDiscPeriodoFormatoFiltroDeficiencia
        (
            long tud_id
            , long tur_id
            , int tpc_id
            , int ava_id
            , int ordenacao
            , int fav_id
            , byte tipoAvaliacao
            , int esa_id
            , byte tipoEscalaDisciplina
            , byte tipoEscalaDocente
            , string avaliacaoesRelacionadas
            , double notaMinimaAprovacao
            , int ordemParecerMinimo
            , byte tipoLancamento
            , byte fav_calculoQtdeAulasDadas
            , bool permiteAlterarResultado
            , byte tur_tipo
            , int cal_id
            , bool exibirNotaFinal
            , byte tdc_id
            , DataTable dtTurma
            , bool documentoOficial
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_SelectBy_TurmaDisciplinaFormatoFiltroDeficiencia", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            Param.Value = tud_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tpc_id";
            Param.Size = 4;
            if (tpc_id > 0)
                Param.Value = tpc_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ava_id";
            Param.Size = 4;
            if (ava_id > 0)
                Param.Value = ava_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ordenacao";
            Param.Size = 4;
            Param.Value = ordenacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@fav_id";
            Param.Size = 4;
            Param.Value = fav_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoAvaliacao";
            Param.Size = 1;
            Param.Value = tipoAvaliacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@esa_id";
            Param.Size = 4;
            Param.Value = esa_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoEscalaDisciplina";
            Param.Size = 1;
            Param.Value = tipoEscalaDisciplina;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoEscalaDocente";
            Param.Size = 1;
            Param.Value = tipoEscalaDocente;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@avaliacaoesRelacionadas";
            if (string.IsNullOrEmpty(avaliacaoesRelacionadas))
                Param.Value = DBNull.Value;
            else
                Param.Value = avaliacaoesRelacionadas;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Decimal;
            Param.ParameterName = "@notaMinimaAprovacao";
            Param.Size = 20;
            Param.Value = notaMinimaAprovacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ordemParecerMinimo";
            Param.Size = 4;
            Param.Value = ordemParecerMinimo;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoLancamento";
            Param.Size = 1;
            Param.Value = tipoLancamento;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@fav_calculoQtdeAulasDadas";
            Param.Size = 1;
            Param.Value = fav_calculoQtdeAulasDadas;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@permiteAlterarResultado";
            Param.Size = 1;
            Param.Value = permiteAlterarResultado;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tur_tipo";
            Param.Size = 1;
            Param.Value = tur_tipo;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            Param.Value = cal_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@exibirNotaFinal";
            Param.Size = 1;
            Param.Value = exibirNotaFinal;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tdc_id";
            Param.Size = 1;
            Param.Value = tdc_id;
            qs.Parameters.Add(Param);

            SqlParameter sqlParam = new SqlParameter();
            sqlParam.SqlDbType = SqlDbType.Structured;
            sqlParam.ParameterName = "@dtTurma";
            sqlParam.TypeName = "TipoTabela_Turma";
            sqlParam.Value = dtTurma;
            qs.Parameters.Add(sqlParam);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@documentoOficial";
            Param.Size = 1;
            Param.Value = documentoOficial;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna os dados cadastrados para os componentes da regencia para o período informado,
        /// de acordo com as regras necessárias para ele aparecer na listagem para efetivar.
        /// </summary>
        /// <param name="tur_id">Id da turma</param>
        /// <param name="tpc_id">Tipo de período do calendário</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="fav_id">ID do formato de avaliação</param>
        /// <param name="tipoAvaliacao">Tipo da avaliacao</param>
        /// <param name="esa_id">ID da escala de avaliação utilizada</param>
        /// <param name="tipoEscalaDisciplina">Tipo de escala de avaliação da disciplina</param>
        /// <param name="tipoEscalaDocente">Tipo de escala de avaliação do docente</param>
        /// <param name="avaliacaoesRelacionadas">Avaliações relacionadas à avaliação informada (ids separados por ",")</param>
        /// <param name="notaMinimaAprovacao">Nota mínima de aprovação (se escala for numérica)</param>
        /// <param name="ordemParecerMinimo">Ordem do parecer mínimo da aprovação (se escala for por pareceres)</param>
        /// <param name="permiteAlterarResultado">Indica se o usuário pode alterar o resultado do aluno</param>
        /// <param name="tur_tipo">Tipo de turma</param>
        /// <param name="exibirNotaFinal">Indica se deve buscar a média das notas finais salvas na tela de avaliação</param>
        /// <param name="alunos">Tabela com os alunos necessários</param>
        /// <returns>DataTable</returns>
        public DataTable SelectComponentesRegenciaBy_TurmaFormato
        (
            long tur_id
            , int tpc_id
            , int ava_id
            , int fav_id
            , byte tipoAvaliacao
            , int esa_id
            , byte tipoEscalaDisciplina
            , byte tipoEscalaDocente
            , string avaliacaoesRelacionadas
            , double notaMinimaAprovacao
            , int ordemParecerMinimo
            , bool permiteAlterarResultado
            , byte tur_tipo
            , bool exibirNotaFinal
            , DataTable alunos
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_SelectComponentesRegenciaBy_TurmaFormato", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tpc_id";
            Param.Size = 4;
            if (tpc_id > 0)
                Param.Value = tpc_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ava_id";
            Param.Size = 4;
            if (ava_id > 0)
                Param.Value = ava_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@fav_id";
            Param.Size = 4;
            Param.Value = fav_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoAvaliacao";
            Param.Size = 1;
            Param.Value = tipoAvaliacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@esa_id";
            Param.Size = 4;
            Param.Value = esa_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoEscalaDisciplina";
            Param.Size = 1;
            Param.Value = tipoEscalaDisciplina;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoEscalaDocente";
            Param.Size = 1;
            Param.Value = tipoEscalaDocente;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@avaliacaoesRelacionadas";
            if (string.IsNullOrEmpty(avaliacaoesRelacionadas))
                Param.Value = DBNull.Value;
            else
                Param.Value = avaliacaoesRelacionadas;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Decimal;
            Param.ParameterName = "@notaMinimaAprovacao";
            Param.Size = 20;
            Param.Value = notaMinimaAprovacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ordemParecerMinimo";
            Param.Size = 4;
            Param.Value = ordemParecerMinimo;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@permiteAlterarResultado";
            Param.Size = 1;
            Param.Value = permiteAlterarResultado;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tur_tipo";
            Param.Size = 1;
            Param.Value = tur_tipo;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@exibirNotaFinal";
            Param.Size = 1;
            Param.Value = exibirNotaFinal;
            qs.Parameters.Add(Param);

            SqlParameter sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@alunos";
            sqlParam.SqlDbType = SqlDbType.Structured;
            sqlParam.TypeName = "TipoTabela_AlunoMatriculaTurma";
            sqlParam.Value = alunos;
            qs.Parameters.Add(sqlParam);

            #endregion

            qs.Execute();

            return qs.Return;
        }
        
        /// <summary>
        /// Retorna os alunos matriculados na TurmaDisciplina para a recuperação final,
        ///	de acordo com as regras necessárias para ele aparecer na listagem para efetivar.
        /// </summary>
        /// <param name="tud_id">ID da disciplina na turma</param>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="tpc_id">Tipo de período do calendário</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="ordenacao">Ordenação da busca</param>
        /// <param name="fav_id">Formato de avaliação</param>        
        /// <param name="esa_id">ID da escala de avaliação utilizada</param>
        /// <param name="tipoEscalaDisciplina">Tipo de escala de avaliação da disciplina</param>
        /// <param name="tipoEscalaDocente">Tipo de escala de avaliação do docente</param>
        /// <param name="avaliacaoesRelacionadas">Avaliações relacionadas à avaliação informada (ids separados por ",")</param>
        /// <param name="notaMinimaAprovacao">Nota mínima de aprovação (se escala for numérica)</param>
        /// <param name="ordemParecerMinimo">Ordem do parecer mínimo da aprovação (se escala for por pareceres)</param>
        /// <param name="tipoLancamento">Tipo de lançamento de frequência</param>
        /// <param name="fav_calculoQtdeAulasDadas">Cálculo da quantidade de aulas dadas</param>        
        public DataTable SelectBy_Alunos_RecuperacaoFinal_By_TurmaDisciplina
        (
            long tud_id
            , long tur_id
            , int tpc_id
            , int ava_id
            , int ordenacao
            , int fav_id            
            , int esa_id
            , byte tipoEscalaDisciplina
            , byte tipoEscalaDocente
            , string avaliacaoesRelacionadas
            , double notaMinimaAprovacao
            , int ordemParecerMinimo
            , byte tipoLancamento
            , byte fav_calculoQtdeAulasDadas
            , bool documentoOficial
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_SelectRecuperacaoFinalBy_TurmaDisciplina", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            Param.Value = tud_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tpc_id";
            Param.Size = 4;
            if (tpc_id > 0)
                Param.Value = tpc_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ava_id";
            Param.Size = 4;
            if (ava_id > 0)
                Param.Value = ava_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ordenacao";
            Param.Size = 4;
            Param.Value = ordenacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@fav_id";
            Param.Size = 4;
            Param.Value = fav_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@esa_id";
            Param.Size = 4;
            Param.Value = esa_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoEscalaDisciplina";
            Param.Size = 1;
            Param.Value = tipoEscalaDisciplina;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoEscalaDocente";
            Param.Size = 1;
            Param.Value = tipoEscalaDocente;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@avaliacaoesRelacionadas";
            if (string.IsNullOrEmpty(avaliacaoesRelacionadas))
                Param.Value = DBNull.Value;
            else
                Param.Value = avaliacaoesRelacionadas;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Decimal;
            Param.ParameterName = "@notaMinimaAprovacao";
            Param.Size = 20;
            Param.Value = notaMinimaAprovacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ordemParecerMinimo";
            Param.Size = 4;
            Param.Value = ordemParecerMinimo;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoLancamento";
            Param.Size = 1;
            Param.Value = tipoLancamento;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@fav_calculoQtdeAulasDadas";
            Param.Size = 1;
            Param.Value = fav_calculoQtdeAulasDadas;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@documentoOficial";
            Param.Size = 1;
            Param.Value = documentoOficial;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna os alunos matriculados na TurmaDisciplina para a recuperação final,
        ///	de acordo com as regras necessárias para ele aparecer na listagem para efetivar.
        ///	Filtrando os alunos com ou sem deficiência, dependendo do docente.
        /// </summary>
        /// <param name="tud_id">ID da disciplina na turma</param>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="tpc_id">Tipo de período do calendário</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="ordenacao">Ordenação da busca</param>
        /// <param name="fav_id">Formato de avaliação</param>        
        /// <param name="esa_id">ID da escala de avaliação utilizada</param>
        /// <param name="tipoEscalaDisciplina">Tipo de escala de avaliação da disciplina</param>
        /// <param name="tipoEscalaDocente">Tipo de escala de avaliação do docente</param>
        /// <param name="avaliacaoesRelacionadas">Avaliações relacionadas à avaliação informada (ids separados por ",")</param>
        /// <param name="notaMinimaAprovacao">Nota mínima de aprovação (se escala for numérica)</param>
        /// <param name="ordemParecerMinimo">Ordem do parecer mínimo da aprovação (se escala for por pareceres)</param>
        /// <param name="tipoLancamento">Tipo de lançamento de frequência</param>
        /// <param name="fav_calculoQtdeAulasDadas">Cálculo da quantidade de aulas dadas</param>        
        /// <param name="tdc_id">ID do tipo de docente</param>
        public DataTable SelectBy_Alunos_RecuperacaoFinal_By_TurmaDisciplinaFiltroDeficiencia
        (
            long tud_id
            , long tur_id
            , int tpc_id
            , int ava_id
            , int ordenacao
            , int fav_id
            , int esa_id
            , byte tipoEscalaDisciplina
            , byte tipoEscalaDocente
            , string avaliacaoesRelacionadas
            , double notaMinimaAprovacao
            , int ordemParecerMinimo
            , byte tipoLancamento
            , byte fav_calculoQtdeAulasDadas
            , byte tdc_id
            , bool documentoOficial
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_SelectRecuperacaoFinalBy_TurmaDisciplinaFiltroDeficiencia", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            Param.Value = tud_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tpc_id";
            Param.Size = 4;
            if (tpc_id > 0)
                Param.Value = tpc_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ava_id";
            Param.Size = 4;
            if (ava_id > 0)
                Param.Value = ava_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ordenacao";
            Param.Size = 4;
            Param.Value = ordenacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@fav_id";
            Param.Size = 4;
            Param.Value = fav_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@esa_id";
            Param.Size = 4;
            Param.Value = esa_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoEscalaDisciplina";
            Param.Size = 1;
            Param.Value = tipoEscalaDisciplina;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoEscalaDocente";
            Param.Size = 1;
            Param.Value = tipoEscalaDocente;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@avaliacaoesRelacionadas";
            if (string.IsNullOrEmpty(avaliacaoesRelacionadas))
                Param.Value = DBNull.Value;
            else
                Param.Value = avaliacaoesRelacionadas;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Decimal;
            Param.ParameterName = "@notaMinimaAprovacao";
            Param.Size = 20;
            Param.Value = notaMinimaAprovacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ordemParecerMinimo";
            Param.Size = 4;
            Param.Value = ordemParecerMinimo;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoLancamento";
            Param.Size = 1;
            Param.Value = tipoLancamento;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@fav_calculoQtdeAulasDadas";
            Param.Size = 1;
            Param.Value = fav_calculoQtdeAulasDadas;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tdc_id";
            Param.Size = 1;
            Param.Value = tdc_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@documentoOficial";
            Param.Size = 1;
            Param.Value = documentoOficial;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
	    /// Calcula a média e traz os campos relacionados à frequencia do aluno (quantidade de aulas, faltas e a 
	    /// frequência).
	    /// Filtra pela matrícula do aluno na disciplina, e pelo período.
	    /// </summary>
	    /// <param name="alu_id">ID do aluno</param>
	    /// <param name="mtu_id">ID da mtrícula na turma</param>
	    /// <param name="mtd_id">ID da matrícula na disciplina</param>
	    /// <param name="tud_id">ID da turma</param>
	    /// <param name="fav_id">ID do formato</param>
	    /// <param name="tpc_id">ID do período</param>
	    /// <param name="tipoLancamento">Tipo de lançamento de frequência</param>
	    /// <param name="fav_calculoQtdeAulasDadas">Tipo de cálculo da quantidade de aulas</param>
        /// <param name="tipoEscalaDocente">Tipo de escala de avaliação do docente</param>
        /// <param name="tipoEscalaDisciplina">Tipo de escala de avaliação da disciplina</param>
	    /// <returns></returns>
	    public DataTable CalculaFrequencia_Media_Aluno
        (
            long alu_id
            , int mtu_id
            , int mtd_id
            , long tud_id
            , int fav_id
            , int tpc_id
            , byte tipoEscalaDisciplina
            , byte tipoEscalaDocente
            , byte tipoLancamento
            , byte fav_calculoQtdeAulasDadas
            , byte tipoDocente
            , byte ava_tipo
            , bool fav_calcularMediaAvaliacaoFinal
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_CalculaFrequencia_Media_Aluno", _Banco);

            #region PARAMETROS

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
            Param.ParameterName = "@mtd_id";
            Param.Size = 4;
            Param.Value = mtd_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            Param.Value = tud_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@fav_id";
            Param.Size = 4;
            Param.Value = fav_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tpc_id";
            Param.Size = 4;
            Param.Value = tpc_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoEscalaDisciplina";
            Param.Size = 1;
            Param.Value = tipoEscalaDisciplina;
            qs.Parameters.Add(Param);
            
            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoEscalaDocente";
            Param.Size = 1;
            Param.Value = tipoEscalaDocente;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoLancamento";
            Param.Size = 1;
            Param.Value = tipoLancamento;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@fav_calculoQtdeAulasDadas";
            Param.Size = 1;
            Param.Value = fav_calculoQtdeAulasDadas;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoDocente";
            Param.Size = 1;
            Param.Value = tipoDocente;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@ava_tipo";
            Param.Size = 1;
            Param.Value = ava_tipo;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@fav_calcularMediaAvaliacaoFinal";
            Param.Size = 1;
            Param.Value = fav_calcularMediaAvaliacaoFinal;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Calcula a média e traz os campos relacionados à frequencia de todos alunos de uma turmadisciplina (quantidade de aulas, faltas e a 
        /// frequência).
        /// Filtra pelo período.
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>        
        /// <param name="fav_id">ID do formato</param>
        /// <param name="tpc_id">ID do período</param>
        /// <param name="tipoLancamento">Tipo de lançamento de frequência</param>
        /// <param name="fav_calculoQtdeAulasDadas">Tipo de cálculo da quantidade de aulas</param>
        /// <param name="tipoEscalaDocente">Tipo de escala de avaliação do docente</param>
        /// <param name="tipoEscalaDisciplina">Tipo de escala de avaliação da disciplina</param>
        /// <returns></returns>
        public DataTable CalculaFrequencia_Media_TodosAlunos
        (
            long tud_id            
            , int fav_id
            , int tpc_id
            , byte tipoEscalaDisciplina
            , byte tipoEscalaDocente
            , byte tipoLancamento
            , byte fav_calculoQtdeAulasDadas
            , byte tipoDocente
            , DataTable dtTurmas
            , byte ava_tipo
            , bool fav_calcularMediaAvaliacaoFinal
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_CalculaFrequencia_Media_TodosAlunos", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            Param.Value = tud_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@fav_id";
            Param.Size = 4;
            Param.Value = fav_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tpc_id";
            Param.Size = 4;
            Param.Value = tpc_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoEscalaDisciplina";
            Param.Size = 1;
            Param.Value = tipoEscalaDisciplina;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoEscalaDocente";
            Param.Size = 1;
            Param.Value = tipoEscalaDocente;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoLancamento";
            Param.Size = 1;
            Param.Value = tipoLancamento;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@fav_calculoQtdeAulasDadas";
            Param.Size = 1;
            Param.Value = fav_calculoQtdeAulasDadas;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoDocente";
            Param.Size = 1;
            Param.Value = tipoDocente;
            qs.Parameters.Add(Param);

            SqlParameter sqlParam = new SqlParameter();
            sqlParam.SqlDbType = SqlDbType.Structured;
            sqlParam.ParameterName = "@dtTurmas";
            sqlParam.TypeName = "TipoTabela_Turma";
            sqlParam.Value = dtTurmas;
            qs.Parameters.Add(sqlParam);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@ava_tipo";
            Param.Size = 1;
            Param.Value = ava_tipo;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@fav_calcularMediaAvaliacaoFinal";
            Param.Size = 1;
            Param.Value = fav_calcularMediaAvaliacaoFinal;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

	    /// <summary>
	    /// Retorna todos os alunos com matricula ativa no COC por disciplina
	    /// (Utilizado na tela de registro de avaliação)
	    /// </summary>
	    /// <param name="tur_id">Id da turma</param>
	    /// <param name="tud_id">ID da turmadisciplina</param>	    
	    /// <param name="tpc_id">ID do tipo de período do calendário</param>
	    /// <param name="cap_dataInicio">Data inicial do período do calendário</param>
	    /// <param name="cap_dataFim">Data final do período do calendário</param>
	    /// <param name="ordenacao">Modo de ordenação dos resultados</param>	    
	    /// <param name="ent_id">ID da entidade</param>
	    /// <returns></returns>
	    public DataTable SelectAtivosCOCByTurmaDisciplina
        (
            long tur_id
            , long tud_id            
            , int tpc_id
            , DateTime cap_dataInicio
            , DateTime cap_dataFim
            , int ordenacao            
            , Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_SelectAtivosCOCByDisciplina", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                if (tur_id > 0)
                    Param.Value = tur_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                if (tud_id > 0)
                    Param.Value = tud_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                Param.Value = tpc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@cap_dataInicio";
                Param.Size = 8;
                Param.Value = cap_dataInicio;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@cap_dataFim";
                Param.Size = 8;
                Param.Value = cap_dataFim;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@ordenacao";
                Param.Size = 4;
                Param.Value = ordenacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
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
        /// Retorna todos os alunos com matricula ativa no COC por disciplina
        /// filtrando os alunos com ou sem deficiência, dependendo do docente.
        /// </summary>
        /// <param name="tur_id">Id da turma</param>
        /// <param name="tud_id">ID da turmadisciplina</param>	    
        /// <param name="tpc_id">ID do tipo de período do calendário</param>
        /// <param name="cap_dataInicio">Data inicial do período do calendário</param>
        /// <param name="cap_dataFim">Data final do período do calendário</param>
        /// <param name="tdc_id">ID do tipo de docente</param>
        /// <param name="ordenacao">Modo de ordenação dos resultados</param>	    
        /// <param name="ent_id">ID da entidade</param>
        /// <returns></returns>
        public DataTable SelectAtivosCOCByTurmaDisciplinaFiltroDeficiencia
        (
            long tur_id
            , long tud_id
            , int tpc_id
            , DateTime cap_dataInicio
            , DateTime cap_dataFim
            , byte tdc_id
            , int ordenacao
            , Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_SelectAtivosCOCByDisciplinaFiltroDeficiencia", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                if (tur_id > 0)
                    Param.Value = tur_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                if (tud_id > 0)
                    Param.Value = tud_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                Param.Value = tpc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@cap_dataInicio";
                Param.Size = 8;
                Param.Value = cap_dataInicio;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@cap_dataFim";
                Param.Size = 8;
                Param.Value = cap_dataFim;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tdc_id";
                Param.Size = 1;
                Param.Value = tdc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@ordenacao";
                Param.Size = 4;
                Param.Value = ordenacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
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
        /// Retorna todos os alunos com matricula ativa no COC por disciplina
        /// (Utilizado na tela de compensação de ausencia)
        /// </summary>
        /// <param name="tud_id">ID da turmadisciplina</param>	    
        /// <param name="tpc_id">ID do tipo de período do calendário</param>
        /// <param name="ordenacao">Modo de ordenação dos resultados</param>	    
        /// <param name="cpa_id">ID da compensacao de ausencia</param>
        /// <param name="filtrarAlunosComFalta">Caso true, filtra para retornar apenas os alunos com faltas a serem compensadas</param>
        /// <returns></returns>
        public DataTable SelecionaAtivosCompensacaoAusencia
        (
            long tud_id
            , int tpc_id           
            , int ordenacao
            , int cpa_id
            , bool documentoOficial
            , bool filtrarAlunosComFalta
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_SelectAlunosAtivosCompensacaoAusencia", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                if (tud_id > 0)
                    Param.Value = tud_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                Param.Value = tpc_id;
                qs.Parameters.Add(Param);                

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@ordenacao";
                Param.Size = 4;
                Param.Value = ordenacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cpa_id";
                Param.Size = 4;
                Param.Value = cpa_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@documentoOficial";
                Param.Size = 1;
                Param.Value = documentoOficial;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@filtrarAlunosComFalta";
                Param.Size = 1;
                Param.Value = filtrarAlunosComFalta;
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
        /// Retorna todos os alunos com matricula ativa no COC por disciplina
        /// Filtrando os alunos com ou sem deficiência, dependendo do docente
        /// (Utilizado na tela de compensação de ausencia)
        /// </summary>
        /// <param name="tur_id">Id da turma</param>
        /// <param name="tud_id">ID da turmadisciplina</param>	    
        /// <param name="tpc_id">ID do tipo de período do calendário</param>
        /// <param name="ordenacao">Modo de ordenação dos resultados</param>	    
        /// <param name="tdc_id">ID do tipo de docente.</param>
        /// <param name="cpa_id">ID da compensacao de ausencia</param>      
        /// <param name="filtrarAlunosComFalta">Caso true, filtra para retornar apenas os alunos com faltas a serem compensadas</param>
        /// <returns></returns>
        public DataTable SelecionaAtivosCompensacaoAusenciaFiltroDeficiencia
        (
            long tur_id
            , long tud_id
            , int tpc_id            
            , int ordenacao                        
            , byte tdc_id
            , int cpa_id
            , bool documentoOficial
            , bool filtrarAlunosComFalta
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_SelectAlunosAtivosCompensacaoAusenciaFiltroDeficiencia", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                if (tur_id > 0)
                    Param.Value = tur_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                if (tud_id > 0)
                    Param.Value = tud_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                Param.Value = tpc_id;
                qs.Parameters.Add(Param);
                
                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@ordenacao";
                Param.Size = 4;
                Param.Value = ordenacao;
                qs.Parameters.Add(Param);
                
                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tdc_id";
                Param.Size = 1;
                Param.Value = tdc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cpa_id";
                Param.Size = 4;
                Param.Value = cpa_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@documentoOficial";
                Param.Size = 1;
                Param.Value = documentoOficial;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@filtrarAlunosComFalta";
                Param.Size = 1;
                Param.Value = filtrarAlunosComFalta;
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
        /// Retorna todos os alunos com matricula ativa no COC por disciplina
        /// (Utilizado na tela de controle de turmas na lista de alunos)
        /// </summary>
        /// <param name="tud_id">ID da turmadisciplina</param>	    
        /// <param name="tpc_id">ID do tipo de período do calendário</param>
        /// <returns></returns>
        public DataTable SelecionaAlunosAtivosCOCPorTurmaDisciplina
        (
            long tud_id
            , int tpc_id
            , DataTable dtTurmas
            , bool documentoOficial
            , DateTime cap_dataInicio
            , DateTime cap_dataFim
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_SelectAlunosCOCDisciplina", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                Param.Value = tpc_id;
                qs.Parameters.Add(Param);

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.ParameterName = "@dtTurmas";
                sqlParam.TypeName = "TipoTabela_Turma";
                sqlParam.Value = dtTurmas;
                qs.Parameters.Add(sqlParam);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@documentoOficial";
                Param.Size = 1;
                Param.Value = documentoOficial;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@cap_dataInicio";
                Param.Size = 16;
                Param.Value = cap_dataInicio;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@cap_dataFim";
                Param.Size = 16;
                Param.Value = cap_dataFim;
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
        /// Retorna todos os alunos com matricula ativa no COC por disciplina
        /// (Utilizado na tela de controle de turmas na lista de alunos)
        /// </summary>
        /// <param name="tud_id">ID da turmadisciplina</param>	    
        /// <param name="tpc_id">ID do tipo de período do calendário</param>
        /// <returns></returns>
        public DataTable SelecionaAlunosAtivosCOCPorTurmaDisciplinaFiltroDeficiencia
        (
            long tud_id
            , int tpc_id
            , byte tdc_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_SelectAlunosCOCDisciplinaFiltroDeficiencia", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                Param.Value = tpc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tdc_id";
                Param.Size = 1;
                Param.Value = tdc_id;
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
        /// Retorna os alunos matriculados na TurmaDisciplina.
        /// </summary>
        /// <param name="tud_id">ID da turmadisciplina</param>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="tpc_id">ID do tipo de período do calendário</param>        
        /// <param name="ordenacao">Modo de ordenação dos resultados</param>
        /// <param name="trazerInativos">Flag que indica se é para retornar alunos que estejam inativos na turma</param>
        /// <returns></returns>
        public DataTable SelectBy_TurmaDisciplinaPeriodo
        (
            long tud_id
            , Guid ent_id
            , int tpc_id
            , int ordenacao
            , bool trazerInativos
        )
        {            
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_SelectBy_TurmaDisciplina", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                if (tud_id > 0)
                    Param.Value = tud_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                Param.Value = tpc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@ordenacao";
                Param.Size = 4;
                Param.Value = ordenacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@trazerInativos";
                Param.Size = 1;
                Param.Value = trazerInativos;
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
        /// Retorna os alunos matriculados na TurmaDisciplina, filtrando os alunos com ou sem deficiência, dependendo do docente.
        /// </summary>
        /// <param name="tud_id">ID da turmadisciplina</param>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="tpc_id">ID do tipo de período do calendário</param>        
        /// <param name="ordenacao">Modo de ordenação dos resultados</param>
        /// <param name="trazerInativos">Flag que indica se é para retornar alunos que estejam inativos na turma</param>
        /// <param name="tdc_id">ID do tipo de docente.</param>
        /// <returns></returns>
        public DataTable SelectBy_TurmaDisciplinaPeriodoFiltroDeficiencia
        (
            long tud_id
            , Guid ent_id
            , int tpc_id
            , int ordenacao
            , bool trazerInativos
            , byte tdc_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_SelectBy_TurmaDisciplinaFiltroDeficiencia", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                if (tud_id > 0)
                    Param.Value = tud_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                Param.Value = tpc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@ordenacao";
                Param.Size = 4;
                Param.Value = ordenacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@trazerInativos";
                Param.Size = 1;
                Param.Value = trazerInativos;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tdc_id";
                Param.Size = 1;
                Param.Value = tdc_id;
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
        /// Seleciona os alunos da disciplina de uma turma, pegando
        /// os alunos ativos e inativos, se houver um mesmo 
        /// aluno com mais de uma matrícula, pega a última  matrícula cadastrada
        /// desse aluno, pelo maior mtu_id.
        /// A data de matrícula do aluno tem que ser maior ou igual a data da aula
        /// </summary>
        /// <param name="tud_id">Id da disciplina da turma.</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <param name="tau_data">Data da aula.</param>
        public DataTable SelectBy_TurmaDisciplina_Max_Mtu
        (
            long tud_id
            , Guid ent_id
            , DateTime tau_data
        )
                {
                    QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_SelectBy_TurmaDisciplina_MAX_Mtu", _Banco);
                    try
                    {
                        #region PARAMETROS

                        Param = qs.NewParameter();
                        Param.DbType = DbType.Int64;
                        Param.ParameterName = "@tud_id";
                        Param.Size = 8;
                        if (tud_id > 0)
                            Param.Value = tud_id;
                        else
                            Param.Value = DBNull.Value;
                        qs.Parameters.Add(Param);

                        Param = qs.NewParameter();
                        Param.DbType = DbType.Guid;
                        Param.ParameterName = "@ent_id";
                        Param.Size = 16;
                        Param.Value = ent_id;
                        qs.Parameters.Add(Param);

                        Param = qs.NewParameter();
                        Param.DbType = DbType.DateTime;
                        Param.ParameterName = "@tau_data";
                        Param.Size = 16;
                        if (tau_data != new DateTime())
                            Param.Value = tau_data;
                        else
                            Param.Value = DBNull.Value;
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
        /// Seleciona os alunos da disciplina de uma turma, pegando
        /// os alunos ativos e inativos, se houver um mesmo 
        /// aluno com mais de uma matrícula, pega a última  matrícula cadastrada
        /// desse aluno, pelo maior mtu_id.
        /// A data de matrícula do aluno tem que ser maior ou igual a data da aula
        /// filtrando os alunos com ou sem deficiência, dependendo do docente.
        /// </summary>
        /// <param name="tud_id">Id da disciplina da turma.</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <param name="tau_data">Data da aula.</param>
        /// <param name="tdc_id">ID do tipo de docente.</param>
        public DataTable SelectBy_TurmaDisciplina_Max_Mtu_FiltroDeficiencia
        (
            long tud_id
            , Guid ent_id
            , DateTime tau_data
            , byte tdc_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_SelectBy_TurmaDisciplina_MAX_Mtu_FiltroDeficiencia", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                if (tud_id > 0)
                    Param.Value = tud_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@tau_data";
                Param.Size = 16;
                if (tau_data != new DateTime())
                    Param.Value = tau_data;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tdc_id";
                Param.Size = 1;
                Param.Value = tdc_id;
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
        /// Retorna as matrículas realizadas para as disciplinas da turma.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <returns>DataTable com registros da MatriculaTurma</returns>
        public DataTable SelectBy_tur_id
        (
            long tur_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_SELECTBY_tur_id", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna dataTable contendo todos os alunos na Matricula Turma Disciplina.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do gridview</param>
        /// <param name="pageSize">Total de registros por página</param> 
        /// <param name="totalRecords"></param>
        /// <returns>DataTable com registros da MatriculaTurma</returns>
        public DataTable SelectBy_tur_id
        (
            long tur_id          
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {            
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_SELECTBY_tur_id", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
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

        /// <summary>        
        /// Verifica a qtde de tempos de aulas em turma eletivas do aluno
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="tud_idNaoConsiderar">ID da disciplina da turma</param>        
        public int SelectBy_VerificaTempoEletivasAluno
        (
            long alu_id
            , long tur_id
            , long tud_idNaoConsiderar
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculTurmaDisciplina_SelectBy_VerificaTempoEletivasAluno", _Banco);
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
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_idNaoConsiderar";
                Param.Size = 8;
                Param.Value = tud_idNaoConsiderar;
                qs.Parameters.Add(Param);                

                #endregion

                qs.Execute();

                return qs.Return.Rows.Count > 0 ? Convert.ToInt32(qs.Return.Rows[0][0].ToString()) : 0;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna a MatriculaTurmaDisciplina para a matricula turma, o aluno, a turma disciplina e a data de matricula selecionados
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="mtu_id">ID da matrícula turma</param>
        /// <param name="tud_id">ID da turma disciplina</param>
        /// <returns>DataTable de MatriculaTurmaDisciplina</returns>
        public DataTable SelectByDisciplinaPeriodoAluno
        (
            long alu_id
            , int mtu_id
            , int tud_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_SelectByDisciplinaPeriodoAluno", _Banco);
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
                Param.ParameterName = "@mtu_id";
                Param.Size = 4;
                Param.Value = mtu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tud_id";
                Param.Size = 4;
                Param.Value = tud_id;
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
        /// Retorna a matricula do aluno pela turma disciplina
        /// </summary>
        /// <param name="alu_id">Aluno</param>
        /// <param name="tud_id">Turma disciplina</param>
        /// <returns></returns>
        public DataTable SelectPorAlunoTurmaDisciplina(long alu_id, long tud_id)
        {

            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_Select_AlunoDisciplina", _Banco);
            try
            {

                #region Parametros
                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
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
	    /// Atualiza a situação de todas as matrículas turma disciplina
	    /// através da turma disciplina passada e da matrícula turma disciplina.
	    /// </summary>
	    /// <param name="tud_id">Id da turma disciplina.</param>	    
	    /// <param name="mtd_dataSaida">Data de saída</param>
        /// <param name="mtd_situacao">Situação para qual será atualizada.</param>
	    public void UpdateSituacaoBy_TurmaDisciplina
        (
            long tud_id            
            , DateTime mtd_dataSaida
            , byte mtd_situacao
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_UpdateSituacaoPorTurmaDisciplina", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@mtd_situacao";
                Param.Size = 1;
                Param.Value = mtd_situacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@mtd_dataSaida";
                Param.Size = 16;
                Param.Value = mtd_dataSaida;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@mtd_dataAlteracao";
                Param.Size = 16;
                Param.Value = DateTime.Now;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        public DataTable CalculaFaltaCompensacaoAluno(long alu_id, int mtu_id, int mtd_id, long tud_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_CalculaFaltaCompensacao_Aluno", _Banco);
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
                Param.ParameterName = "@mtd_id";
                Param.Size = 4;
                Param.Value = mtd_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
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
        /// Retorna os alunos com compensação de ausência
        /// (Utilizado na tela de compensação de ausencia)
        /// </summary>
        /// <param name="cpa_id">ID da compensacao de ausencia</param>
        /// <param name="tud_id">ID da turmadisciplina</param>	    
        /// <param name="tpc_id">ID do tipo de período do calendário</param>
        /// <param name="doc_id">ID do docente</param>
        /// <returns></returns>
        public DataTable SelecionaQtdeAlunosAusenciasCompensadas
        (
            int cpa_id
            , long tud_id
            , int tpc_id
            , long doc_id
            , bool documentoOficial
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_SelectQtdeAlunosAusenciasCompensadas", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cpa_id";
                Param.Size = 4;
                if (cpa_id > 0)
                    Param.Value = cpa_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                if (tud_id > 0)
                    Param.Value = tud_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                Param.Value = tpc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@doc_id";
                Param.Size = 8;
                if (doc_id > 0)
                    Param.Value = doc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@documentoOficial";
                Param.Size = 1;
                Param.Value = documentoOficial;
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
        /// Calcula a média do aluno.
        /// Filtra pela matrícula do aluno nos componentes da regencia da turma, e pelo período.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="mtu_id">ID da mtrícula na turma</param>
        /// <param name="fav_id">ID do formato</param>
        /// <param name="tpc_id">ID do período</param>
        /// <param name="tipoEscalaDocente">Tipo de escala de avaliação do docente</param>
        /// <param name="tipoEscalaDisciplina">Tipo de escala de avaliação da disciplina</param>
        /// <returns></returns>
        public DataTable Calcula_MediaComponentesRegencia_Aluno
        (
            long tur_id
            , long alu_id
            , int mtu_id
            , int fav_id
            , int tpc_id
            , byte tipoEscalaDisciplina
            , byte tipoEscalaDocente
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_CalculaMediaComponentesRegencia_Aluno", _Banco);

            #region PARAMETROS

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
            Param.ParameterName = "@fav_id";
            Param.Size = 4;
            Param.Value = fav_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tpc_id";
            Param.Size = 4;
            Param.Value = tpc_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoEscalaDisciplina";
            Param.Size = 1;
            Param.Value = tipoEscalaDisciplina;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoEscalaDocente";
            Param.Size = 1;
            Param.Value = tipoEscalaDocente;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Calcula a média de todos os alunos nos componentes da regencia da turma.
        /// Filtra pelo período.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>        
        /// <param name="fav_id">ID do formato</param>
        /// <param name="tpc_id">ID do período</param>
        /// <param name="tipoEscalaDisciplina">Tipo de escala de avaliação da disciplina</param>
        /// <param name="tipoEscalaDocente">Tipo de escala de avaliação do docente</param>
        /// <returns></returns>
        public DataTable Calcula_MediaComponentesRegencia_TodosAlunos
        (
            long tur_id
            , int fav_id
            , int tpc_id
            , byte tipoEscalaDisciplina
            , byte tipoEscalaDocente
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_CalculaMediaComponentesRegencia_TodosAlunos", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@fav_id";
            Param.Size = 4;
            Param.Value = fav_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tpc_id";
            Param.Size = 4;
            Param.Value = tpc_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoEscalaDisciplina";
            Param.Size = 1;
            Param.Value = tipoEscalaDisciplina;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoEscalaDocente";
            Param.Size = 1;
            Param.Value = tipoEscalaDocente;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna os alunos matriculados na TurmaDisciplina, com dados de frequência
        /// e notas lançados para todos os períodos, para a avaliação Final.
        /// </summary>
        /// <param name="tud_id">ID da disciplina na turma</param>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="ordenacao">Ordenação da busca</param>
        /// <param name="fav_id">Formato de avaliação</param>
        /// <param name="tipoEscalaDisciplina">Tipo de escala de avaliação da disciplina</param>
        /// <param name="tipoEscalaDocente">Tipo de escala de avaliação do docente</param>
        /// <param name="tur_tipo">Tipo de turma</param>
        /// <param name="cal_id">ID do calendário da turma</param>
        /// <param name="tipoLancamento">Tipo de lançamento de frequência</param>
        /// <param name="fav_calculoQtdeAulasDadas">Cálculo da quantidade de aulas dadas</param>
        /// <param name="permiteAlterarResultado">Indica se o usuário pode alterar o resultado do aluno</param>
        /// <param name="tipoDocente">Tipo de docente</param>
        /// <returns></returns>
        public DataTable SelectBy_TurDiscFinalFormato
        (
            long tud_id
            , long tur_id
            , int ava_id
            , int ordenacao
            , int fav_id
            , byte tipoEscalaDisciplina
            , byte tipoEscalaDocente
            , byte tur_tipo
            , int cal_id
            , byte tipoLancamento
            , byte fav_calculoQtdeAulasDadas
            , bool permiteAlterarResultado
            , byte tipoDocente
            , DataTable dtTurma
            , bool documentoOficial
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_SelectBy_TurmaDisciplinaFormato_Final", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            Param.Value = tud_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ava_id";
            Param.Size = 4;
            if (ava_id > 0)
                Param.Value = ava_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ordenacao";
            Param.Size = 4;
            Param.Value = ordenacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@fav_id";
            Param.Size = 4;
            Param.Value = fav_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoEscalaDisciplina";
            Param.Size = 1;
            Param.Value = tipoEscalaDisciplina;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoEscalaDocente";
            Param.Size = 1;
            Param.Value = tipoEscalaDocente;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tur_tipo";
            Param.Size = 1;
            Param.Value = tur_tipo;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            Param.Value = cal_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoLancamento";
            Param.Size = 1;
            Param.Value = tipoLancamento;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@fav_calculoQtdeAulasDadas";
            Param.Size = 1;
            Param.Value = fav_calculoQtdeAulasDadas;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoDocente";
            Param.Size = 1;
            Param.Value = tipoDocente;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@permiteAlterarResultado";
            Param.Size = 1;
            Param.Value = permiteAlterarResultado;
            qs.Parameters.Add(Param);

            SqlParameter sqlParam = new SqlParameter();
            sqlParam.SqlDbType = SqlDbType.Structured;
            sqlParam.ParameterName = "@dtTurma";
            sqlParam.TypeName = "TipoTabela_Turma";
            sqlParam.Value = dtTurma;
            qs.Parameters.Add(sqlParam);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@documentoOficial";
            Param.Size = 1;
            Param.Value = documentoOficial;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna os alunos matriculados na TurmaDisciplina, com dados de frequência
        /// e notas lançados para todos os períodos, para a avaliação Final.
        /// </summary>
        /// <param name="tud_id">ID da disciplina na turma</param>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="ordenacao">Ordenação da busca</param>
        /// <param name="fav_id">Formato de avaliação</param>
        /// <param name="tipoEscalaDisciplina">Tipo de escala de avaliação da disciplina</param>
        /// <param name="tipoEscalaDocente">Tipo de escala de avaliação do docente</param>
        /// <param name="tur_tipo">Tipo de turma</param>
        /// <param name="cal_id">ID do calendário da turma</param>
        /// <param name="tipoLancamento">Tipo de lançamento de frequência</param>
        /// <param name="fav_calculoQtdeAulasDadas">Cálculo da quantidade de aulas dadas</param>
        /// <param name="permiteAlterarResultado">Indica se o usuário pode alterar o resultado do aluno</param>
        /// <param name="tipoDocente">Tipo de docente</param>
        /// <param name="alunos">Lista dos alunos para filtro</param>
        /// <returns></returns>
        public DataTable SelectBy_TurDiscFinalFormato_ByAluno
        (
            long tud_id
            , long tur_id
            , int ava_id
            , int ordenacao
            , int fav_id
            , byte tipoEscalaDisciplina
            , byte tipoEscalaDocente
            , byte tur_tipo
            , int cal_id
            , byte tipoLancamento
            , byte fav_calculoQtdeAulasDadas
            , bool permiteAlterarResultado
            , byte tipoDocente
            , DataTable alunos
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_SelectBy_TurmaDisciplinaFormato_Final_ByAluno", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            Param.Value = tud_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ava_id";
            Param.Size = 4;
            if (ava_id > 0)
                Param.Value = ava_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ordenacao";
            Param.Size = 4;
            Param.Value = ordenacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@fav_id";
            Param.Size = 4;
            Param.Value = fav_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoEscalaDisciplina";
            Param.Size = 1;
            Param.Value = tipoEscalaDisciplina;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoEscalaDocente";
            Param.Size = 1;
            Param.Value = tipoEscalaDocente;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tur_tipo";
            Param.Size = 1;
            Param.Value = tur_tipo;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            Param.Value = cal_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoLancamento";
            Param.Size = 1;
            Param.Value = tipoLancamento;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@fav_calculoQtdeAulasDadas";
            Param.Size = 1;
            Param.Value = fav_calculoQtdeAulasDadas;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoDocente";
            Param.Size = 1;
            Param.Value = tipoDocente;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@permiteAlterarResultado";
            Param.Size = 1;
            Param.Value = permiteAlterarResultado;
            qs.Parameters.Add(Param);

            SqlParameter sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@alunos";
            sqlParam.SqlDbType = SqlDbType.Structured;
            sqlParam.TypeName = "TipoTabela_AlunoMatriculaTurma";
            sqlParam.Value = alunos;
            qs.Parameters.Add(sqlParam);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna os alunos matriculados na TurmaDisciplina, com dados de frequência
        /// e notas lançados para todos os períodos, para a avaliação Final.
        /// Filtrando os alunos com ou sem deficiência, dependendo do docente.
        /// </summary>
        /// <param name="tud_id">ID da disciplina na turma</param>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="ordenacao">Ordenação da busca</param>
        /// <param name="fav_id">Formato de avaliação</param>
        /// <param name="tipoEscalaDisciplina">Tipo de escala de avaliação da disciplina</param>
        /// <param name="tipoEscalaDocente">Tipo de escala de avaliação do docente</param>
        /// <param name="tur_tipo">Tipo de turma</param>
        /// <param name="cal_id">ID do calendário da turma</param>
        /// <param name="tipoLancamento">Tipo de lançamento de frequência</param>
        /// <param name="fav_calculoQtdeAulasDadas">Cálculo da quantidade de aulas dadas</param>
        /// <param name="permiteAlterarResultado">Indica se o usuário pode alterar o resultado do aluno</param>
        /// <param name="tipoDocente">Tipo de docente</param>
        /// <returns></returns>
        public DataTable SelectBy_TurDiscFinalFormatoFiltroDeficiencia
        (
            long tud_id
            , long tur_id
            , int ava_id
            , int ordenacao
            , int fav_id
            , byte tipoEscalaDisciplina
            , byte tipoEscalaDocente
            , byte tur_tipo
            , int cal_id
            , byte tipoLancamento
            , byte fav_calculoQtdeAulasDadas
            , bool permiteAlterarResultado
            , byte tipoDocente
            , DataTable dtTurma
            , bool documentoOficial
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_SelectBy_TurmaDisciplinaFormatoFiltroDeficiencia_Final", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            Param.Value = tud_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ava_id";
            Param.Size = 4;
            if (ava_id > 0)
                Param.Value = ava_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ordenacao";
            Param.Size = 4;
            Param.Value = ordenacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@fav_id";
            Param.Size = 4;
            Param.Value = fav_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoEscalaDisciplina";
            Param.Size = 1;
            Param.Value = tipoEscalaDisciplina;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoEscalaDocente";
            Param.Size = 1;
            Param.Value = tipoEscalaDocente;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tur_tipo";
            Param.Size = 1;
            Param.Value = tur_tipo;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            Param.Value = cal_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoLancamento";
            Param.Size = 1;
            Param.Value = tipoLancamento;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@fav_calculoQtdeAulasDadas";
            Param.Size = 1;
            Param.Value = fav_calculoQtdeAulasDadas;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoDocente";
            Param.Size = 1;
            Param.Value = tipoDocente;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@permiteAlterarResultado";
            Param.Size = 1;
            Param.Value = permiteAlterarResultado;
            qs.Parameters.Add(Param);

            SqlParameter sqlParam = new SqlParameter();
            sqlParam.SqlDbType = SqlDbType.Structured;
            sqlParam.ParameterName = "@dtTurma";
            sqlParam.TypeName = "TipoTabela_Turma";
            sqlParam.Value = dtTurma;
            qs.Parameters.Add(sqlParam);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@documentoOficial";
            Param.Size = 1;
            Param.Value = documentoOficial;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna os dados cadastrados para os componentes da regencia,
        /// de acordo com as regras necessárias para ele aparecer na listagem para efetivar da avaliacao Final. 
        /// </summary>
        /// <param name="tur_id">Id da turma</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="fav_id">ID do formato de avaliação</param>
        /// <param name="tipoEscalaDisciplina">Tipo de escala de avaliação da disciplina</param>
        /// <param name="tipoEscalaDocente">Tipo de escala de avaliação do docente</param>
        /// <param name="tur_tipo">Tipo de turma</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="alunos">Tabela com os alunos necessários</param>
        /// <param name="permiteAlterarResultado">Indica se o usuário pode alterar o resultado do aluno</param>
        /// <returns>DataTable</returns>
        public DataTable SelectComponentesRegenciaBy_TurmaFormato_Final
        (
            long tur_id
            , int ava_id
            , int fav_id
            , byte tipoEscalaDisciplina
            , byte tipoEscalaDocente
            , byte tur_tipo
            , int cal_id
            , DataTable alunos
            , bool permiteAlterarResultado
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_SelectComponentesRegenciaBy_TurmaFormato_Final", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ava_id";
            Param.Size = 4;
            if (ava_id > 0)
                Param.Value = ava_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@fav_id";
            Param.Size = 4;
            Param.Value = fav_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoEscalaDisciplina";
            Param.Size = 1;
            Param.Value = tipoEscalaDisciplina;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoEscalaDocente";
            Param.Size = 1;
            Param.Value = tipoEscalaDocente;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tur_tipo";
            Param.Size = 1;
            Param.Value = tur_tipo;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            Param.Value = cal_id;
            qs.Parameters.Add(Param);

            SqlParameter sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@alunos";
            sqlParam.SqlDbType = SqlDbType.Structured;
            sqlParam.TypeName = "TipoTabela_AlunoMatriculaTurma";
            sqlParam.Value = alunos;
            qs.Parameters.Add(sqlParam);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@permiteAlterarResultado";
            Param.Size = 1;
            Param.Value = permiteAlterarResultado;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Atualiza o resultados das matrículas turma disciplina.
        /// </summary>
        /// <param name="dtMatriculaTurmaDisciplinaResultado">DataTable de matrículas turmna disciplina e seus resultado.</param>
        /// <returns></returns>
        public bool AtualizarResultado(DataTable dtMatriculaTurmaDisciplinaResultado)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_AtualizaResultado", _Banco);

            try
            {
                #region Parâmetros

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbMatriculaTurmaDisciplinaResultado";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_MatriculaTurmaDisciplinaResultado";
                sqlParam.Value = dtMatriculaTurmaDisciplinaResultado;
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

        /// <summary>
        /// Seleciona os alunos matriculados na disciplina dada por determinado docente.
        /// </summary>
        /// <param name="doc_id">ID do docente.</param>
        /// <param name="tud_id">ID da turma disciplina.</param>
        /// <returns></returns>
        public DataTable SelecionaAlunosPorTurmaDocente(long doc_id, long tud_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_MatriculaTurmaDisciplina_SelecionaAlunosPorTurmaDocente", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@doc_id";
                Param.Size = 8;
                Param.Value = doc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
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
        /// O método retorna uma lista de matrícula turma disciplina, passando uma tabela de chaves.
        /// </summary>
        /// <param name="dtMatriculaTurmaDisciplina">Tabela de chaves da MTR_MatriculaTurmaDisciplina.</param>
        /// <returns></returns>
        public List<MTR_MatriculaTurmaDisciplina> SelecionaPorMatriculaTurmaDisciplina(DataTable dtMatriculaTurmaDisciplina)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_SelecionaPorMatriculaTurmaDisciplina", _Banco);

            try
            {
                #region Parâmetros

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@AlunoMatriculaTurmaDisciplina";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_AlunoMatriculaTurmaDisciplina";
                sqlParam.Value = dtMatriculaTurmaDisciplina;
                qs.Parameters.Add(sqlParam);

                #endregion

                qs.Execute();

                return qs.Return.Rows.Count > 0 ?
                    qs.Return.Rows.Cast<DataRow>().AsParallel().Select(p => DataRowToEntity(p, new MTR_MatriculaTurmaDisciplina())).ToList() :
                    new List<MTR_MatriculaTurmaDisciplina>();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna os alunos matriculados na TurmaDisciplina para o período informado,
        /// de acordo com as regras necessárias para ele aparecer na listagem
        /// para efetivar.
        /// </summary>
        /// <param name="tud_id">ID da disciplina na turma</param>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="tpc_id">Tipo de período do calendário</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="ordenacao">Ordenação da busca</param>
        /// <param name="fav_id">Formato de avaliação</param>
        /// <param name="tipoAvaliacao">Tipo de avaliação</param>
        /// <param name="permiteAlterarResultado">Indica se o usuário pode alterar o resultado do aluno</param>
        /// <param name="tur_tipo">Tipo de turma</param>
        /// <param name="cal_id">ID do calendário da turma</param>
        /// <param name="tud_tipo">Tipo da turma disciplina</param> 
        /// <param name="dtTurma">Turmas da multisseriada</param>   
        /// <returns></returns>
        public DataTable SelectFechamento
        (
            long tud_id
            , long tur_id
            , int tpc_id
            , int ava_id
            , int ordenacao
            , int fav_id
            , byte tipoAvaliacao
            , bool permiteAlterarResultado
            , byte tur_tipo
            , int cal_id
            , DataTable dtTurma
            , bool documentoOficial
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_SelectFechamento", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            Param.Value = tud_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tpc_id";
            Param.Size = 4;
            if (tpc_id > 0)
                Param.Value = tpc_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ava_id";
            Param.Size = 4;
            if (ava_id > 0)
                Param.Value = ava_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ordenacao";
            Param.Size = 4;
            Param.Value = ordenacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@fav_id";
            Param.Size = 4;
            Param.Value = fav_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoAvaliacao";
            Param.Size = 1;
            Param.Value = tipoAvaliacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@permiteAlterarResultado";
            Param.Size = 1;
            Param.Value = permiteAlterarResultado;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tur_tipo";
            Param.Size = 1;
            Param.Value = tur_tipo;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            Param.Value = cal_id;
            qs.Parameters.Add(Param);

            SqlParameter sqlParam = new SqlParameter();
            sqlParam.SqlDbType = SqlDbType.Structured;
            sqlParam.ParameterName = "@dtTurma";
            sqlParam.TypeName = "TipoTabela_Turma";
            sqlParam.Value = dtTurma;
            qs.Parameters.Add(sqlParam);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@documentoOficial";
            Param.Size = 1;
            Param.Value = documentoOficial;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna os dados do fechamento cadastrados para os componentes da regencia
        /// para o período informado, de acordo com as regras necessárias para ele aparecer na listagem para efetivar.
        /// </summary>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="tpc_id">Tipo de período do calendário</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="fav_id">Formato de avaliação</param>
        /// <param name="tipoAvaliacao">Tipo de avaliação</param>
        /// <param name="permiteAlterarResultado">Indica se o usuário pode alterar o resultado do aluno</param>
        /// <param name="tur_tipo">Tipo de turma</param>
        /// <param name="alunos">Tabela com os alunos necessários</param>
        /// <returns>DataTable</returns>
        public DataTable SelectFechamentoComponentesRegencia
        (
            long tur_id
            , int tpc_id
            , int ava_id
            , int fav_id
            , byte tipoAvaliacao
            , bool permiteAlterarResultado
            , byte tur_tipo
            , DataTable alunos
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_SelectFechamentoComponentesRegencia", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tpc_id";
            Param.Size = 4;
            if (tpc_id > 0)
                Param.Value = tpc_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@fav_id";
            Param.Size = 4;
            Param.Value = fav_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoAvaliacao";
            Param.Size = 1;
            Param.Value = tipoAvaliacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tur_tipo";
            Param.Size = 1;
            Param.Value = tur_tipo;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ava_id";
            Param.Size = 4;
            if (ava_id > 0)
                Param.Value = ava_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@permiteAlterarResultado";
            Param.Size = 1;
            Param.Value = permiteAlterarResultado;
            qs.Parameters.Add(Param);

            SqlParameter sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@alunos";
            sqlParam.SqlDbType = SqlDbType.Structured;
            sqlParam.TypeName = "TipoTabela_AlunoMatriculaTurma";
            sqlParam.Value = alunos;
            qs.Parameters.Add(sqlParam);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna os alunos matriculados na TurmaDisciplina para o período informado,
        /// de acordo com as regras necessárias para ele aparecer na listagem
        /// para efetivar. Filtrando os alunos com ou sem deficiência, dependendo do docente.
        /// </summary>
        /// <param name="tud_id">ID da disciplina na turma</param>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="tpc_id">Tipo de período do calendário</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="ordenacao">Ordenação da busca</param>
        /// <param name="fav_id">Formato de avaliação</param>
        /// <param name="tipoAvaliacao">Tipo de avaliação</param>
        /// <param name="permiteAlterarResultado">Indica se o usuário pode alterar o resultado do aluno</param>
        /// <param name="tur_tipo">Tipo de turma</param>
        /// <param name="cal_id">ID do calendário da turma</param>
        /// <param name="tud_tipo">Tipo da turma disciplina</param> 
        /// <param name="tdc_id">Tipo de docente</param> 
        /// <param name="dtTurma">Turmas da multisseriada</param>   
        /// <returns></returns>
        public DataTable SelectFechamentoFiltroDeficiencia
        (
            long tud_id
            , long tur_id
            , int tpc_id
            , int ava_id
            , int ordenacao
            , int fav_id
            , byte tipoAvaliacao
            , bool permiteAlterarResultado
            , byte tur_tipo
            , int cal_id
            , byte tdc_id
            , DataTable dtTurma
            , bool documentoOficial
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_SelectFechamentoFiltroDeficiencia", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            Param.Value = tud_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tpc_id";
            Param.Size = 4;
            if (tpc_id > 0)
                Param.Value = tpc_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ava_id";
            Param.Size = 4;
            if (ava_id > 0)
                Param.Value = ava_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ordenacao";
            Param.Size = 4;
            Param.Value = ordenacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@fav_id";
            Param.Size = 4;
            Param.Value = fav_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoAvaliacao";
            Param.Size = 1;
            Param.Value = tipoAvaliacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@permiteAlterarResultado";
            Param.Size = 1;
            Param.Value = permiteAlterarResultado;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tur_tipo";
            Param.Size = 1;
            Param.Value = tur_tipo;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            Param.Value = cal_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tdc_id";
            Param.Size = 1;
            Param.Value = tdc_id;
            qs.Parameters.Add(Param);

            SqlParameter sqlParam = new SqlParameter();
            sqlParam.SqlDbType = SqlDbType.Structured;
            sqlParam.ParameterName = "@dtTurma";
            sqlParam.TypeName = "TipoTabela_Turma";
            sqlParam.Value = dtTurma;
            qs.Parameters.Add(sqlParam);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@documentoOficial";
            Param.Size = 1;
            Param.Value = documentoOficial;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna os alunos matriculados na TurmaDisciplina, com dados de frequência
        /// e notas lançados para todos os períodos, para a avaliação Final.
        /// </summary>
        /// <param name="tud_id">ID da disciplina na turma</param>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="ordenacao">Ordenação da busca</param>
        /// <param name="fav_id">Formato de avaliação</param>
        /// <param name="tur_tipo">Tipo de turma</param>
        /// <param name="cal_id">ID do calendário da turma</param>
        /// <param name="permiteAlterarResultado">Indica se o usuário pode alterar o resultado do aluno</param>
        /// <param name="dtTurma">Turmas da multisseriada</param>         
        /// <returns></returns>
        public DataTable SelectFechamentoFinal
        (
            long tud_id
            , long tur_id
            , int ava_id
            , int ordenacao
            , int fav_id
            , byte tur_tipo
            , int cal_id
            , bool permiteAlterarResultado
            , DataTable dtTurma
            , bool documentoOficial
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_SelectFechamento_Final", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            Param.Value = tud_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ava_id";
            Param.Size = 4;
            if (ava_id > 0)
                Param.Value = ava_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ordenacao";
            Param.Size = 4;
            Param.Value = ordenacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@fav_id";
            Param.Size = 4;
            Param.Value = fav_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tur_tipo";
            Param.Size = 1;
            Param.Value = tur_tipo;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            Param.Value = cal_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@permiteAlterarResultado";
            Param.Size = 1;
            Param.Value = permiteAlterarResultado;
            qs.Parameters.Add(Param);

            SqlParameter sqlParam = new SqlParameter();
            sqlParam.SqlDbType = SqlDbType.Structured;
            sqlParam.ParameterName = "@dtTurma";
            sqlParam.TypeName = "TipoTabela_Turma";
            sqlParam.Value = dtTurma;
            qs.Parameters.Add(sqlParam);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@documentoOficial";
            Param.Size = 1;
            Param.Value = documentoOficial;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna os alunos matriculados na TurmaDisciplina, com dados de frequência
        /// e notas lançados para todos os períodos, para a avaliação Final.
        /// </summary>
        /// <param name="tud_id">ID da disciplina na turma</param>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="ordenacao">Ordenação da busca</param>
        /// <param name="fav_id">Formato de avaliação</param>
        /// <param name="tur_tipo">Tipo de turma</param>
        /// <param name="cal_id">ID do calendário da turma</param>
        /// <param name="permiteAlterarResultado">Indica se o usuário pode alterar o resultado do aluno</param>
        /// <param name="tdc_id">Tipo de docente</param> 
        /// <param name="dtTurma">Turmas da multisseriada</param>         
        /// <returns></returns>
        public DataTable SelectFechamentoFiltroDeficienciaFinal
        (
            long tud_id
            , long tur_id
            , int ava_id
            , int ordenacao
            , int fav_id
            , byte tur_tipo
            , int cal_id
            , bool permiteAlterarResultado
            , byte tdc_id
            , DataTable dtTurma
            , bool documentoOficial
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_SelectFechamentoFiltroDeficiencia_Final", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            Param.Value = tud_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ava_id";
            Param.Size = 4;
            if (ava_id > 0)
                Param.Value = ava_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ordenacao";
            Param.Size = 4;
            Param.Value = ordenacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@fav_id";
            Param.Size = 4;
            Param.Value = fav_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tur_tipo";
            Param.Size = 1;
            Param.Value = tur_tipo;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            Param.Value = cal_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@permiteAlterarResultado";
            Param.Size = 1;
            Param.Value = permiteAlterarResultado;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tdc_id";
            Param.Size = 1;
            Param.Value = tdc_id;
            qs.Parameters.Add(Param);

            SqlParameter sqlParam = new SqlParameter();
            sqlParam.SqlDbType = SqlDbType.Structured;
            sqlParam.ParameterName = "@dtTurma";
            sqlParam.TypeName = "TipoTabela_Turma";
            sqlParam.Value = dtTurma;
            qs.Parameters.Add(sqlParam);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@documentoOficial";
            Param.Size = 1;
            Param.Value = documentoOficial;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna os dados cadastrados para os componentes da regencia,
        /// de acordo com as regras necessárias para ele aparecer na listagem para efetivar da avaliacao Final. 
        /// </summary>
        /// <param name="tur_id">Id da turma</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="fav_id">ID do formato de avaliação</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="alunos">Tabela com os alunos necessários</param>
        /// <param name="permiteAlterarResultado">Indica se o usuário pode alterar o resultado do aluno</param>
        /// <returns>DataTable</returns>
        public DataTable SelectFechamentoComponentesRegenciaFinal
        (
            long tur_id
            , int ava_id
            , int fav_id
            , int cal_id
            , DataTable alunos
            , bool permiteAlterarResultado
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurmaDisciplina_SelectFechamentoComponentesRegencia_Final", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ava_id";
            Param.Size = 4;
            if (ava_id > 0)
                Param.Value = ava_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@fav_id";
            Param.Size = 4;
            Param.Value = fav_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            Param.Value = cal_id;
            qs.Parameters.Add(Param);

            SqlParameter sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@alunos";
            sqlParam.SqlDbType = SqlDbType.Structured;
            sqlParam.TypeName = "TipoTabela_AlunoMatriculaTurma";
            sqlParam.Value = alunos;
            qs.Parameters.Add(sqlParam);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@permiteAlterarResultado";
            Param.Size = 1;
            Param.Value = permiteAlterarResultado;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        #endregion

        #region Métodos Sobrescritos

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, MTR_MatriculaTurmaDisciplina entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@mtd_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@mtd_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamAlterar(QueryStoredProcedure qs, MTR_MatriculaTurmaDisciplina entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@mtd_dataCriacao");
            qs.Parameters["@mtd_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity">Entidade MTR_MatriculaTurmaDisciplina</param>
        /// <returns>true = sucesso | false = fracasso</returns> 
        protected override bool Alterar(MTR_MatriculaTurmaDisciplina entity)
        {
            __STP_UPDATE = "NEW_MTR_MatriculaTurmaDisciplina_Update";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamDeletar(QueryStoredProcedure qs, MTR_MatriculaTurmaDisciplina entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@alu_id";
            Param.Size = 8;
            Param.Value = entity.alu_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@mtu_id";
            Param.Size = 4;
            Param.Value = entity.mtu_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@mtd_id";
            Param.Size = 4;
            Param.Value = entity.mtd_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@mtd_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@mtd_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity">Entidade MTR_MatriculaTurmaDisciplina</param>
        /// <returns>true = sucesso | false = fracasso</returns>   
        public override bool Delete(MTR_MatriculaTurmaDisciplina entity)
        {
            __STP_DELETE = "NEW_MTR_MatriculaTurmaDisciplina_Update_Situacao";
            return base.Delete(entity);
        }

        #endregion

        #region Comentados

        ///// <summary>
        ///// Inseri os valores da classe em um registro ja existente
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem modificados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Alterar(MTR_MatriculaTurmaDisciplina entity)
        //{
        //    return base.Alterar(entity);
        //}
        ///// <summary>
        ///// Inseri os valores da classe em um novo registro
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem inseridos</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Inserir(MTR_MatriculaTurmaDisciplina entity)
        //{
        //    return base.Inserir(entity);
        //}
        ///// <summary>
        ///// Carrega um registro da tabela usando os valores nas chaves
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem carregados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Carregar(MTR_MatriculaTurmaDisciplina entity)
        //{
        //    return base.Carregar(entity);
        //}
        ///// <summary>
        ///// Exclui um registro do banco
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem apagados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Delete(MTR_MatriculaTurmaDisciplina entity)
        //{
        //    return base.Delete(entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Alterar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamAlterar(QueryStoredProcedure qs, MTR_MatriculaTurmaDisciplina entity)
        //{
        //    base.ParamAlterar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Carregar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamCarregar(QuerySelectStoredProcedure qs, MTR_MatriculaTurmaDisciplina entity)
        //{
        //    base.ParamCarregar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Deletar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamDeletar(QueryStoredProcedure qs, MTR_MatriculaTurmaDisciplina entity)
        //{
        //    base.ParamDeletar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Inserir
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamInserir(QuerySelectStoredProcedure qs, MTR_MatriculaTurmaDisciplina entity)
        //{
        //    base.ParamInserir(qs, entity);
        //}
        ///// <summary>
        ///// Salva o registro no banco de dados
        ///// </summary>
        ///// <param name="entity">Entidade com os dados para preenchimento para inserir ou alterar</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Salvar(MTR_MatriculaTurmaDisciplina entity)
        //{
        //    return base.Salvar(entity);
        //}
        ///// <summary>
        ///// Realiza o select da tabela
        ///// </summary>
        ///// <returns>Lista com todos os registros da tabela</returns>
        //public override IList<MTR_MatriculaTurmaDisciplina> Select()
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
        //public override IList<MTR_MatriculaTurmaDisciplina> Select_Paginado(int currentPage, int pageSize, out int totalRecord)
        //{
        //    return base.Select_Paginado(currentPage, pageSize, out totalRecord);
        //}
        ///// <summary>
        ///// Recebe o valor do auto incremento e coloca na propriedade 
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, MTR_MatriculaTurmaDisciplina entity)
        //{
        //    return base.ReceberAutoIncremento(qs, entity);
        //}
        ///// <summary>
        ///// Passa os dados de um datatable para uma entidade
        ///// </summary>
        ///// <param name="dr">DataRow do datatable preenchido</param>
        ///// <param name="entity">Entidade onde ser�o transferidos os dados</param>
        ///// <returns>Entidade preenchida</returns>
        //public override MTR_MatriculaTurmaDisciplina DataRowToEntity(DataRow dr, MTR_MatriculaTurmaDisciplina entity)
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
        //public override MTR_MatriculaTurmaDisciplina DataRowToEntity(DataRow dr, MTR_MatriculaTurmaDisciplina entity, bool limparEntity)
        //{
        //    return base.DataRowToEntity(dr, entity, limparEntity);
        //}

        #endregion
    }
}