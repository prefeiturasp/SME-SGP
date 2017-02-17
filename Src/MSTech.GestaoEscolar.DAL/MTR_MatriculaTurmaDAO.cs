/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Data;
using System.Data.SqlClient;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL.Abstracts;
using MSTech.GestaoEscolar.Entities;
using System.Collections.Generic;

namespace MSTech.GestaoEscolar.DAL
{
    /// <summary>
    ///
    /// </summary>
    public class MTR_MatriculaTurmaDAO : Abstract_MTR_MatriculaTurmaDAO
    {
        #region Métodos

        /// <summary>
        /// Calcula a média e traz os campos relacionados à frequencia do aluno (quantidade de aulas, faltas e a
        /// frequência).
        /// Filtra pela matrícula do aluno na turma, e pelo período.
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="mtu_id">ID da mtrícula na turma</param>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="fav_id">ID do formato</param>
        /// <param name="tpc_id">ID do período</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="tipoEscala">Tipo de escala de avaliação</param>
        /// <param name="tipoLancamento">Tipo de lançamento de frequência</param>
        /// <param name="fav_calculoQtdeAulasDadas">Tipo de cálculo da quantidade de aulas</param>
        /// <returns></returns>
        public DataTable CalculaFrequencia_Media_Aluno
        (
            long alu_id
            , int mtu_id
            , long tur_id
            , int fav_id
            , int tpc_id
            , int ava_id
            , byte tipoEscala
            , byte tipoLancamento
            , byte fav_calculoQtdeAulasDadas
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurma_CalculaFrequencia_Media_Aluno", _Banco);

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
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ava_id";
            Param.Size = 4;
            Param.Value = ava_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoEscala";
            Param.Size = 1;
            Param.Value = tipoEscala;
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

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Calcula a média e traz os campos relacionados à frequencia do aluno (quantidade de aulas, faltas e a
        /// frequência).
        /// Filtra pelo período.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="fav_id">ID do formato</param>
        /// <param name="tpc_id">ID do período</param>
        /// <param name="tipoEscala">Tipo de escala de avaliação</param>
        /// <param name="tipoLancamento">Tipo de lançamento de frequência</param>
        /// <param name="fav_calculoQtdeAulasDadas">Tipo de cálculo da quantidade de aulas</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <returns></returns>
        public DataTable CalculaFrequencia_Media_TodosAlunos
        (
            long tur_id
            , int fav_id
            , int tpc_id
            , byte tipoEscala
            , byte tipoLancamento
            , byte fav_calculoQtdeAulasDadas
            , int ava_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurma_CalculaFrequencia_Media_TodosAlunos", _Banco);

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
            Param.ParameterName = "@tipoEscala";
            Param.Size = 1;
            Param.Value = tipoEscala;
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
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ava_id";
            Param.Size = 4;
            Param.Value = ava_id;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna a ultima matricula cadastrada no sistema.
        /// </summary>
        /// <param name="alu_id"></param>
        /// <returns>Retorna todos os dados de MatriculaTurma</returns>
        public DataTable SelectBy_MatriculaAluno
        (
            long alu_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurma_SelectBy_Atual_Aluno", _Banco);
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
        /// Retorna a ultima matricula cadastrada no sistema,
        /// para os alunos da lista passada por parametro.
        /// </summary>
        /// <param name="alu_id"></param>
        /// <returns>Retorna todos os dados de MatriculaTurma</returns>
        public DataTable SelectBy_MatriculaListaAlunos
        (
            DataTable dtAlunos
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurma_SelectBy_Atual_ListaAlunos", _Banco);
            try
            {
                #region PARAMETROS

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@alunos";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_Aluno";
                sqlParam.Value = dtAlunos;
                qs.Parameters.Add(sqlParam);

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
        /// Retorna os alunos matriculados na turma, com a frequência e nota dele
        /// totais no período informado de acordo com as regras necessárias para ele
        /// aparecer na listagem para efetivar.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="tpc_id">Tipo de período do calendário</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="ordenacao">Ordenação da busca</param>
        /// <param name="fav_id">Formato de avaliação</param>
        /// <param name="tipoAvaliacao">Tipo de avaliação</param>
        /// <param name="esa_id">ID da escala de avaliação utilizada</param>
        /// <param name="tipoEscala">Tipo de escala de avaliação utilizada</param>
        /// <param name="avaliacaoesRelacionadas">Avaliações relacionadas à avaliação informada (ids separados por ",")</param>
        /// <param name="notaMinimaAprovacao">Nota mínima de aprovação (se escala for numérica)</param>
        /// <param name="ordemParecerMinimo">Ordem do parecer mínimo da aprovação (se escala for por pareceres)</param>
        /// <param name="tipoLancamento">Tipo de lançamento de frequência</param>
        /// <param name="esa_tipoAvaliacaoAdicional">Tipod de escala da avaliacao adicional.</param>
        /// <param name="permiteAlterarResultado">Indica se o usuário pode alterar o resultado do aluno</param>
        /// <returns></returns>
        public DataTable SelectBy_Turma_Periodo
        (
            Int64 tur_id
            , Int32 tpc_id
            , Int32 ava_id
            , Int32 ordenacao
            , Int32 fav_id
            , byte tipoAvaliacao
            , int esa_id
            , byte tipoEscala
            , string avaliacaoesRelacionadas
            , double notaMinimaAprovacao
            , int ordemParecerMinimo
            , byte tipoLancamento
            , byte esa_tipoAvaliacaoAdicional
            , bool permiteAlterarResultado
            , bool documentoOficial
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurma_SelectBy_Turma", _Banco);

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
            Param.ParameterName = "@tipoEscala";
            Param.Size = 1;
            Param.Value = tipoEscala;
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
            Param.ParameterName = "@esa_tipoAvaliacaoAdicional";
            Param.Size = 1;
            Param.Value = esa_tipoAvaliacaoAdicional;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@permiteAlterarResultado";
            Param.Size = 1;
            Param.Value = permiteAlterarResultado;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@documentoOficial";
            Param.Size = 1;
            Param.Value = documentoOficial;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna os alunos matriculados na turma de acordo com as regras necessárias para ele
        /// aparecer na listagem para efetivar da avaliacao Final.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="ordenacao">Ordenação da busca</param>
        /// <param name="fav_id">Formato de avaliação</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="tipoEscala">Tipo de escala de avaliação utilizada</param>
        /// <param name="tipoLancamento">Tipo de lançamento de frequência</param>
        /// <param name="esa_tipoAvaliacaoAdicional">Tipod de escala da avaliacao adicional.</param>
        /// <param name="permiteAlterarResultado">Indica se o usuário pode alterar o resultado do aluno</param>
        /// <returns></returns>
        public DataTable SelectBy_Turma_Final
        (
            Int64 tur_id
            , Int32 ava_id
            , Int32 ordenacao
            , Int32 fav_id
            , Int32 cal_id
            , byte tipoEscala
            , byte tipoLancamento
            , byte esa_tipoAvaliacaoAdicional
            , bool permiteAlterarResultado
            , bool documentoOficial
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurma_SelectBy_Turma_Final", _Banco);

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
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            Param.Value = cal_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoEscala";
            Param.Size = 1;
            Param.Value = tipoEscala;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoLancamento";
            Param.Size = 1;
            Param.Value = tipoLancamento;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@esa_tipoAvaliacaoAdicional";
            Param.Size = 1;
            Param.Value = esa_tipoAvaliacaoAdicional;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@permiteAlterarResultado";
            Param.Size = 1;
            Param.Value = permiteAlterarResultado;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@documentoOficial";
            Param.Size = 1;
            Param.Value = documentoOficial;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna os alunos matriculados na turma de acordo com as regras necessárias para ele
        /// aparecer na listagem para efetivar da avaliacao Final.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="ordenacao">Ordenação da busca</param>
        /// <param name="fav_id">Formato de avaliação</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="tipoEscala">Tipo de escala de avaliação utilizada</param>
        /// <param name="tipoLancamento">Tipo de lançamento de frequência</param>
        /// <param name="esa_tipoAvaliacaoAdicional">Tipod de escala da avaliacao adicional.</param>
        /// <param name="permiteAlterarResultado">Indica se o usuário pode alterar o resultado do aluno</param>
        /// <param name="alunos">Lista dos alunos para filtro</param>
        /// <returns></returns>
        public DataTable SelectBy_Turma_Final_ByAluno
        (
            Int64 tur_id
            , Int32 ava_id
            , Int32 ordenacao
            , Int32 fav_id
            , Int32 cal_id
            , byte tipoEscala
            , byte tipoLancamento
            , byte esa_tipoAvaliacaoAdicional
            , bool permiteAlterarResultado
            , DataTable alunos
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurma_SelectBy_Turma_Final_ByAluno", _Banco);

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
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            Param.Value = cal_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoEscala";
            Param.Size = 1;
            Param.Value = tipoEscala;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipoLancamento";
            Param.Size = 1;
            Param.Value = tipoLancamento;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@esa_tipoAvaliacaoAdicional";
            Param.Size = 1;
            Param.Value = esa_tipoAvaliacaoAdicional;
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

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna os alunos matriculados na TurmaDisciplina para a recuperação final,
        ///	de acordo com as regras necessárias para ele aparecer na listagem para efetivar.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="tpc_id">Tipo de período do calendário</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <param name="ordenacao">Ordenação da busca</param>
        /// <param name="fav_id">Formato de avaliação</param>        
        /// <param name="esa_id">ID da escala de avaliação utilizada</param>
        /// <param name="tipoEscala">Tipo de escala de avaliação utilizada</param>
        /// <param name="avaliacaoesRelacionadas">Avaliações relacionadas à avaliação informada (ids separados por ",")</param>
        /// <param name="notaMinimaAprovacao">Nota mínima de aprovação (se escala for numérica)</param>
        /// <param name="ordemParecerMinimo">Ordem do parecer mínimo da aprovação (se escala for por pareceres)</param>
        /// <param name="tipoLancamento">Tipo de lançamento de frequência</param>
        /// <param name="esa_tipoAvaliacaoAdicional">Tipod de escala da avaliacao adicional.</param>        
        public DataTable SelectBy_Alunos_RecuperacaoFinal_By_Turma
        (
            Int64 tur_id
            , Int32 tpc_id
            , Int32 ava_id
            , Int32 ordenacao
            , Int32 fav_id            
            , int esa_id
            , byte tipoEscala
            , string avaliacaoesRelacionadas
            , double notaMinimaAprovacao
            , int ordemParecerMinimo
            , byte tipoLancamento
            , byte esa_tipoAvaliacaoAdicional
            , bool documentoOficial
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurma_SelectRecuperacaoFinalBy_Turma", _Banco);

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
            Param.ParameterName = "@tipoEscala";
            Param.Size = 1;
            Param.Value = tipoEscala;
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
            Param.ParameterName = "@esa_tipoAvaliacaoAdicional";
            Param.Size = 1;
            Param.Value = esa_tipoAvaliacaoAdicional;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@documentoOficial";
            Param.Size = 1;
            Param.Value = documentoOficial;
            qs.Parameters.Add(Param);


            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Busca os anos em que o aluno teve alguma matricula
        /// </summary>
        /// <param name="alu_id">Id da tabela ACA_Aluno do bd</param>
        /// <returns>DataTable com os anos</returns>
        public DataTable SelectAnoMatricula
        (
            long alu_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurma_SelectAnoCalendario", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@alu_id";
                Param.Size = 4;
                Param.Value = alu_id;
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
        /// Retorna alunos deficiente na turma
        /// </summary>
        /// <param name="tur_id">Id da turma</param>
        /// <returns>DataTable com os alunos</returns>
        public DataTable RetornaAlunoDeficienteTurma
        (
            long tur_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurma_VerificaAlunoDeficienteTurma", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 4;
                Param.Value = tur_id;
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
        /// Seleciona turma, periodo, escola, curso e numero da chamada
        /// que o aluno esta ativo ou em matricula
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <returns>DataTable turma, periodo, escola, curso e numero da chamada do aluno</returns>
        public DataTable SelectDadosMatriculaAluno
        (
           long alu_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurma_SelectDadosMatriculaAluno_by_alu_id", _Banco);
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
        /// Seleciona turma, periodo, escola, curso e numero da chamada
        /// que o aluno esta ativo ou em matricula
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <returns>DataTable turma, periodo, escola, curso e numero da chamada do aluno</returns>
        public DataTable SelectDadosMatriculaAlunoMtu
        (
           long alu_id
            , int mtu_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurma_SelecionaDadosMatriculaAluno_Aluno_Mtu", _Banco);
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

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@mtu_id";
                Param.Size = 4;
                Param.Value = mtu_id;
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
        /// Verifica se o aluno já está cadastrado em alguma turma
        /// com as situações ativo ou em matrícula
        /// </summary>
        public bool SelectBy_alu_id
        (
            long alu_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurma_SelectBy_alu_id", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 4;
                if (alu_id > 0)
                    Param.Value = alu_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                return (qs.Return.Rows.Count > 0);
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
        /// Traz todos os alunos de uma turma em ordem alfabetica
        /// com as situações ativo ou em matrícula
        /// </summary>
        public DataTable SelectBy_tur_id
        (
            long tur_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurma_SelectBy_tur_id", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 4;
                if (tur_id > 0)
                    Param.Value = tur_id;
                else
                    Param.Value = DBNull.Value;
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
        /// Description:	Lista dos alunos matriculados para aquela turma
        /// 				Número de chamada;
        /// 				Número de matrícula ou matrícula estadual de acordo com o parâmetro ;
        /// 				Nome do Aluno.
        /// </summary>
        /// <param name="tur_id">Id de filtro da turma</param>
        /// <returns>Datatable com alunos da turma</returns>
        public DataTable SelectAlunosBy_tur_id
        (
            long tur_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurma_SelectAlunos", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 4;
                if (tur_id > 0)
                    Param.Value = tur_id;
                else
                    Param.Value = DBNull.Value;
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
        /// Description:	Lista dos alunos ativos matriculados para aquela turma
        ///
        /// </summary>
        /// <param name="tur_id">Id de filtro da turma</param>
        /// <returns>Datatable com alunos da turma</returns>
        public DataTable SelectAlunosAtivosBy_tur_id
        (
            long tur_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurma_SelectAlunosAtivos", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 4;
                if (tur_id > 0)
                    Param.Value = tur_id;
                else
                    Param.Value = DBNull.Value;
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
        /// Verifica o código da última matrícula turma cadastrada para o aluno
        /// </summary>
        /// <param name="alu_id">Id da tabela ACA_Aluno do bd</param>
        /// <returns>mtu_id + 1</returns>
        public int SelectBy_alu_id_top_one
        (
            long alu_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurma_SelectBy_alu_id_top_one", _Banco);
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

                #endregion PARAMETROS

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                    return Convert.ToInt32(qs.Return.Rows[0]["mtu_id"].ToString()) + 1;
                else
                    return 1;
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
        /// Retorna o novo número de chamada da turma informada, pega o último número de chamada cadastrado
        /// + 1.
        /// </summary>
        /// <param name="tur_id">ID da turma - obrigatório</param>
        /// <param name="cur_id">ID do curso - obrigatório</param>
        /// <param name="crr_id">ID do currículo - obrigatório</param>
        /// <param name="crp_id">ID do período - obrigatório</param>
        /// <returns>Último número de chamada + 1</returns>
        public int RetornaNovoNumeroChamada_Turma
        (
            long tur_id
            , int cur_id
            , int crr_id
            , int crp_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_MatriculaTurma_SelectBy_tur_id_top_one", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
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

            #endregion PARAMETROS

            qs.Execute();

            if (qs.Return.Rows.Count > 0 && !string.IsNullOrEmpty(qs.Return.Rows[0]["mtu_numeroChamada"].ToString()))
                return Convert.ToInt32(qs.Return.Rows[0]["mtu_numeroChamada"].ToString()) + 1;

            return 1;
        }

        /// <summary>
        /// Atualiza numero de chamada em Matricula por turma e matricula por disciplina turma
        /// </summary>
        /// <param name="alu_id">Id do aluno (filtro)</param>
        /// <param name="mtu_id">Id da turma (filtro)</param>
        /// <param name="mtu_numeroChamada">Novo número de chamada</param>
        /// <returns>TRUE - alteracao com sucesso</returns>
        public bool AtualizaNumeroChamada(Int64 alu_id, Int32 mtu_id, Int32 mtu_numeroChamada)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurma_Update_NumeroChamada", _Banco);
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
                Param.ParameterName = "@mtu_numeroChamada";
                Param.Value = mtu_numeroChamada;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                return true;
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

        ///// <summary>
        ///// Calcula a frequência do aluno.
        ///// </summary>
        ///// <param name="alu_id">Id do aluno</param>
        ///// <param name="tur_id">Id da turma</param>
        ///// <param name="tpc_id">Id do tipo período calendário</param>
        ///// <param name="ava_id">Id da avaliação</param>
        ///// <param name="fav_id">Id do tipo de lançamento</param>
        ///// <returns></returns>
        //public DataTable SelectBy_Frequencia_alu_id
        //(
        //    long alu_id
        //    , long tur_id
        //    , int tpc_id
        //    , int ava_id
        //    , int fav_id
        //)
        //{
        //    QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurma_CalculaFrequenciaAluno", _Banco);
        //    try
        //    {
        //        #region Parâmetros

        //        Param = qs.NewParameter();
        //        Param.DbType = DbType.Int64;
        //        Param.ParameterName = "@alu_id";
        //        Param.Size = 8;
        //        Param.Value = alu_id;
        //        qs.Parameters.Add(Param);

        //        Param = qs.NewParameter();
        //        Param.DbType = DbType.Int64;
        //        Param.ParameterName = "@tur_id";
        //        Param.Size = 8;
        //        Param.Value = tur_id;
        //        qs.Parameters.Add(Param);

        //        Param = qs.NewParameter();
        //        Param.DbType = DbType.Int32;
        //        Param.ParameterName = "@tpc_id";
        //        Param.Size = 4;
        //        Param.Value = tpc_id;
        //        qs.Parameters.Add(Param);

        //        Param = qs.NewParameter();
        //        Param.DbType = DbType.Int32;
        //        Param.ParameterName = "@ava_id";
        //        Param.Size = 4;
        //        Param.Value = ava_id;
        //        qs.Parameters.Add(Param);

        //        Param = qs.NewParameter();
        //        Param.DbType = DbType.Int32;
        //        Param.ParameterName = "@fav_id";
        //        Param.Size = 4;
        //        Param.Value = fav_id;
        //        qs.Parameters.Add(Param);

        //        #endregion

        //        qs.Execute();

        //        return qs.Return;
        //    }
        //    finally
        //    {
        //        qs.Parameters.Clear();
        //    }
        //}

        ///// <summary>
        ///// Calcula a média do aluno.
        ///// </summary>
        ///// <param name="alu_id">Id do aluno.</param>
        ///// <param name="tur_id">Id da turma.</param>
        ///// <param name="tpc_id">Id do tipo de período do calendário.</param>
        ///// <param name="ava_id">Id da avaliação.</param>
        ///// <param name="fav_id">Id do formato de avaliação.</param>
        ///// <param name="tipoAvaliacao">Tipo de avaliação.</param>
        ///// <param name="esa_id">Id da escala de avaliação.</param>
        ///// <param name="tipoEscala">Tipo de escala.</param>
        ///// <param name="avaliacaoesRelacionadas">Avaliações relacionadas.</param>
        ///// <param name="notaMinimaAprovacao">Nota mínima para aprovação.</param>
        ///// <param name="ordemParecerMinimo">Ordem de parecer mínimo.</param>
        ///// <param name="tipoLancamento">Tipo de lançamento.</param>
        ///// <param name="cap_dataInicio">Data de início do período do calendário.</param>
        ///// <param name="cap_dataFim">Data de fim do período do calendário.</param>
        ///// <returns>Retorna um DataTable com os dados do aluno e a média calculada.</returns>
        //public DataTable SelectBy_MediaAluno
        //(
        //    long alu_id
        //    , long tur_id
        //    , int tpc_id
        //    , int ava_id
        //    , int fav_id
        //    , byte tipoAvaliacao
        //    , int esa_id
        //    , byte tipoEscala
        //    , string avaliacaoesRelacionadas
        //    , double notaMinimaAprovacao
        //    , int ordemParecerMinimo
        //    , byte tipoLancamento
        //    , DateTime cap_dataInicio
        //    , DateTime cap_dataFim
        //)
        //{
        //    QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurma_CalculaMediaAluno", _Banco);

        //    #region PARAMETROS

        //    Param = qs.NewParameter();
        //    Param.DbType = DbType.Int64;
        //    Param.ParameterName = "@alu_id";
        //    Param.Size = 8;
        //    Param.Value = alu_id;
        //    qs.Parameters.Add(Param);

        //    Param = qs.NewParameter();
        //    Param.DbType = DbType.Int64;
        //    Param.ParameterName = "@tur_id";
        //    Param.Size = 8;
        //    Param.Value = tur_id;
        //    qs.Parameters.Add(Param);

        //    Param = qs.NewParameter();
        //    Param.DbType = DbType.Int32;
        //    Param.ParameterName = "@tpc_id";
        //    Param.Size = 4;
        //    Param.Value = tpc_id;
        //    qs.Parameters.Add(Param);

        //    Param = qs.NewParameter();
        //    Param.DbType = DbType.Int32;
        //    Param.ParameterName = "@ava_id";
        //    Param.Size = 4;
        //    if (ava_id > 0)
        //        Param.Value = ava_id;
        //    else
        //        Param.Value = DBNull.Value;
        //    qs.Parameters.Add(Param);

        //    Param = qs.NewParameter();
        //    Param.DbType = DbType.Int32;
        //    Param.ParameterName = "@fav_id";
        //    Param.Size = 4;
        //    Param.Value = fav_id;
        //    qs.Parameters.Add(Param);

        //    Param = qs.NewParameter();
        //    Param.DbType = DbType.Byte;
        //    Param.ParameterName = "@tipoAvaliacao";
        //    Param.Size = 1;
        //    Param.Value = tipoAvaliacao;
        //    qs.Parameters.Add(Param);

        //    Param = qs.NewParameter();
        //    Param.DbType = DbType.Int32;
        //    Param.ParameterName = "@esa_id";
        //    Param.Size = 4;
        //    Param.Value = esa_id;
        //    qs.Parameters.Add(Param);

        //    Param = qs.NewParameter();
        //    Param.DbType = DbType.Byte;
        //    Param.ParameterName = "@tipoEscala";
        //    Param.Size = 1;
        //    Param.Value = tipoEscala;
        //    qs.Parameters.Add(Param);

        //    Param = qs.NewParameter();
        //    Param.DbType = DbType.String;
        //    Param.ParameterName = "@avaliacaoesRelacionadas";
        //    if (string.IsNullOrEmpty(avaliacaoesRelacionadas))
        //        Param.Value = DBNull.Value;
        //    else
        //        Param.Value = avaliacaoesRelacionadas;
        //    qs.Parameters.Add(Param);

        //    Param = qs.NewParameter();
        //    Param.DbType = DbType.Decimal;
        //    Param.ParameterName = "@notaMinimaAprovacao";
        //    Param.Size = 20;
        //    Param.Value = notaMinimaAprovacao;
        //    qs.Parameters.Add(Param);

        //    Param = qs.NewParameter();
        //    Param.DbType = DbType.Int32;
        //    Param.ParameterName = "@ordemParecerMinimo";
        //    Param.Size = 4;
        //    Param.Value = ordemParecerMinimo;
        //    qs.Parameters.Add(Param);

        //    Param = qs.NewParameter();
        //    Param.DbType = DbType.Byte;
        //    Param.ParameterName = "@tipoLancamento";
        //    Param.Size = 1;
        //    Param.Value = tipoLancamento;
        //    qs.Parameters.Add(Param);

        //    Param = qs.NewParameter();
        //    Param.DbType = DbType.Date;
        //    Param.ParameterName = "@cap_dataInicio";
        //    Param.Size = 20;
        //    Param.Value = cap_dataInicio;
        //    qs.Parameters.Add(Param);

        //    Param = qs.NewParameter();
        //    Param.DbType = DbType.Date;
        //    Param.ParameterName = "@cap_dataFim";
        //    Param.Size = 20;
        //    Param.Value = cap_dataFim;
        //    qs.Parameters.Add(Param);

        //    #endregion

        //    qs.Execute();

        //    return qs.Return;
        //}

        /// <summary>
        /// Retorna os alunos matriculados na turma selecionada dentro do período informado.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="ordenacao">0 - ordena pelo numero de chamada / 1- ordena pelo nome do aluno</param>
        /// <param name="dataInicio">Data de início do período</param>
        /// <param name="dataFim">Data de fim do período</param>
        /// <returns>DataTable de alunos matriculados</returns>
        public DataTable SelectBy_Turma_DataMatricula
        (
            long tur_id
            , int ordenacao
            , DateTime dataInicio
            , DateTime dataFim
            , int tpc_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurma_SelectBy_Turma_DataMatricula", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ordenacao";
            Param.Size = 4;
            Param.Value = ordenacao;
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
        /// Retorna os alunos de uma turma
        /// </summary>
        /// <param name="tur_id">id da turma</param>
        /// <returns>DataTable com os ids dos alunos</returns>
        public DataTable BuscaAlunosPorTurma
        (
            long tur_id            
        )
        {

            List<MTR_MatriculaTurma> lt = new List<MTR_MatriculaTurma>();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurma_BuscaAlunosPorTurma", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
            qs.Parameters.Add(Param);           

            #endregion PARAMETROS

            qs.Execute();
                       
            return qs.Return;
        }

        /// <summary>
        /// Lista dos alunos matriculados na turma dentro do periodo de calendário.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cap_id">ID do período do calendário</param>
        /// <param name="trazerInativos">1 - traz inativos | 0 - não traz inativos</param>
        /// <returns></returns>
        public DataTable SelecionaAlunosPorTurmaPeriodoCalendario(int tur_id, Guid ent_id, int cal_id, int cap_id, byte trazerInativos)
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurma_SelectBy_PeriodoCalendario", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
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

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@trazerInativos";
                Param.Size = 1;
                Param.Value = trazerInativos;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                    dt = qs.Return;

                return dt;
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
        /// Verifica o status da turma atual da matricula turma
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="mtu_id">ID da matricula turma</param>
        /// <returns></returns>
        public DataTable VerificaStatusTurmaAtual(long alu_id, int mtu_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurma_VerificaStatusTurmaAtual", _Banco);

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

                #endregion Parâmetros

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

        public bool VerificaResultadoRecuperacaoFinal(long tur_id, int esa_id, byte tipoEscala, double notaMinimaAprovacao, int ordemParecerMinimo, string avaliacaoesRelacionadas)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("MTR_MatriculaTurma_VerificaResultadoRecuperacaoFinal", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esa_id";
                Param.Size = 4;
                Param.Value = esa_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tipoEscala";
                Param.Size = 1;
                Param.Value = tipoEscala;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Double;
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
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@avaliacaoesRelacionadas";
                Param.Value = avaliacaoesRelacionadas;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return.Rows.Count > 0;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// O método recalcula e atualiza as notas e frequência finais de um aluno.
        /// </summary>
        /// <param name="alu_id">ID do aluno.</param>
        /// <param name="mtu_id">ID da matrícula do aluno.</param>
        /// <param name="mtu_resultado">Resultado final global do aluno.</param>
        /// <param name="permiteAlterarResultado">Flag que indica se o sistema está configurado para possibiliar a mudança do resultado final do aluno.</param>
        /// <returns></returns>
        public bool CalcularNotaFrequenciaMatriculaAnoAnterior(long alu_id, int mtu_id, byte mtu_resultado, bool permiteAlterarResultado)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_MTR_MatriculaTurma_AtualizarNotaFrequencia", _Banco);

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

                #endregion Parâmetros

                qs.Execute();

                return qs.Return > 0;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// O método recalcula e atualiza as notas e frequência finais de um aluno.
        /// </summary>
        /// <param name="alu_id">ID do aluno.</param>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo.</param>
        /// <param name="crp_id">ID do período.</param>
        /// <returns></returns>
        public DataTable SelecionaPeriodosAvaliacaoPorAluno(long alu_id, int cur_id, int crr_id, int crp_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MatriculaTurma_SelectBy_AluId_CurId_CrrId_CrpId", _Banco);

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

                #endregion Parâmetros

                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion Métodos

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
        /// Recebe o valor do auto incremento e coloca na propriedade
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        /// <param name="entity"></param>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, MTR_MatriculaTurma entity)
        {
            entity.mtu_id = Convert.ToInt32(qs.Return.Rows[0][0]);
            return (entity.mtu_id > 0);
        }

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, MTR_MatriculaTurma entity)
        {
            base.ParamInserir(qs, entity);

            if (entity.mtu_frequencia == 0)
            {
                if (string.IsNullOrEmpty(entity.mtu_avaliacao) && string.IsNullOrEmpty(entity.mtu_relatorio))
                qs.Parameters["@mtu_frequencia"].Value = DBNull.Value;
            }

            qs.Parameters["@mtu_dataMatricula"].DbType = DbType.DateTime;
            qs.Parameters["@mtu_dataSaida"].DbType = DbType.DateTime;

            qs.Parameters["@mtu_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@mtu_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de cr7iação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamAlterar(QueryStoredProcedure qs, MTR_MatriculaTurma entity)
        {
            base.ParamAlterar(qs, entity);

            if (entity.mtu_frequencia == 0)
            {
                if (string.IsNullOrEmpty(entity.mtu_avaliacao) && string.IsNullOrEmpty(entity.mtu_relatorio))
                    qs.Parameters["@mtu_frequencia"].Value = DBNull.Value;
            }

            qs.Parameters["@mtu_dataMatricula"].DbType = DbType.DateTime;
            qs.Parameters["@mtu_dataSaida"].DbType = DbType.DateTime;

            qs.Parameters.RemoveAt("@mtu_dataCriacao");
            qs.Parameters["@mtu_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity">Entidade MTR_MatriculaTurma</param>
        /// <returns>true = sucesso | false = fracasso</returns>
        protected override bool Alterar(MTR_MatriculaTurma entity)
        {
            __STP_UPDATE = "NEW_MTR_MatriculaTurma_Update";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamDeletar(QueryStoredProcedure qs, MTR_MatriculaTurma entity)
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
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@mtu_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@mtu_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity">Entidade MTR_MatriculaTurma</param>
        /// <returns>true = sucesso | false = fracasso</returns>
        public override bool Delete(MTR_MatriculaTurma entity)
        {
            __STP_DELETE = "NEW_MTR_MatriculaTurma_Update_Situacao";
            return base.Delete(entity);
        }

        #endregion Métodos Sobrescritos
    }
}