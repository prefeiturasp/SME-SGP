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
    /// Classe CLS_AlunoAvaliacaoTurmaDAO
    /// </summary>
    public class CLS_AlunoAvaliacaoTurmaDAO : Abstract_CLS_AlunoAvaliacaoTurmaDAO
    {
        #region Métodos

        /// <summary>
        /// Retorna a frequência acumulada calculada no registro.
        /// </summary>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="alu_id">Id do aluno.</param>
        /// <param name="mtu_id">Id da matrícula turma.</param>
        /// <param name="fav_id">Id do formato de avaliação.</param>
        /// <param name="ava_id">Id da avaliação.</param>
        /// <returns>Valor da frequência acumulada calculada.</returns>
        public DataTable RetornaFrequenciaAcumulada_Registro(
            long tur_id,
            long alu_id,
            int mtu_id,
            int fav_id,
            int ava_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_AlunoAvaliacaoTurma_RetornaFrequenciaAcumuladaRegistro", _Banco);

            #region Parâmetros

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
            Param.ParameterName = "@fav_id";
            Param.Size = 4;
            Param.Value = fav_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ava_id";
            Param.Size = 4;
            Param.Value = ava_id;
            qs.Parameters.Add(Param);

            #endregion Parâmetros

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna a frequência acumulada calculada.
        /// </summary>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="alu_id">Id do aluno.</param>
        /// <param name="mtu_id">Id da matrícula turma.</param>
        /// <param name="fav_id">Id do formato de avaliação.</param>
        /// <param name="ava_id">Id da avaliação.</param>
        /// <param name="aat_numeroAulas">Número de aulas.</param>
        /// <param name="aat_numeroFaltas">Número de faltas.</param>
        /// <returns>Valor da frequência acumulada calculada.</returns>
        public DataTable RetornaFrequenciaAcumuladaCalculada(
            long tur_id,
            long alu_id,
            int mtu_id,
            int fav_id,
            int ava_id,
            int aat_numeroAulas,
            int aat_numeroFaltas
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_AlunoAvaliacaoTurma_RetornaFrequenciaAcumuladaCalculada", _Banco);

            #region Parâmetros

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
            Param.ParameterName = "@fav_id";
            Param.Size = 4;
            Param.Value = fav_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ava_id";
            Param.Size = 4;
            Param.Value = ava_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@aat_numeroAulas";
            Param.Size = 4;
            Param.Value = aat_numeroAulas;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@aat_numeroFaltas";
            Param.Size = 4;
            Param.Value = aat_numeroFaltas;
            qs.Parameters.Add(Param);

            #endregion Parâmetros

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna a frequência ajustada calculada.
        /// </summary>
        /// <param name="tud_id">Id da turmaDisciplina.</param>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="alu_id">Id do aluno.</param>
        /// <param name="mtu_id">Id da matrícula turma.</param>
        /// <param name="fav_id">Id do formato de avaliação.</param>
        /// <param name="ava_id">Id da avaliação.</param>
        /// <param name="tpc_id">Id do tipoPeriodoCalendario .</param>
        /// <param name="tipoEscalaDocente">Tipo de escala de avaliação do docente</param>
        /// <param name="tipoEscalaDisciplina">Tipo de escala de avaliação da disciplina</param>
        /// <param name="tipoLancamento">Tipo de lançamento de frequência</param>
        /// <param name="fav_calculoQtdeAulasDadas">Tipo de cálculo da quantidade de aulas</param>
        /// <param name="aat_numeroAulas">Número de aulas.</param>
        /// <param name="aat_numeroFaltas">Número de faltas.</param>
        /// <param name="aat_numeroAusenciasCompensadas">Número de ausências compensadas.</param>
        /// <returns>Valor da frequência ajustada calculada.</returns>
        public DataTable RetornaFrequenciaAjustadaCalculada(
            long tud_id,
            long tur_id,
            long alu_id,
            int mtu_id,
            int fav_id,
            int ava_id,
            int tpc_id,
            byte tipoEscalaDisciplina,
            byte tipoEscalaDocente,
            byte tipoLancamento,
            byte fav_calculoQtdeAulasDadas,
            int aat_numeroAulas,
            int aat_numeroFaltas,
            int aat_numeroAusenciasCompensadas
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_AlunoAvaliacaoTurma_RetornaFrequenciaAjustadaCalculada", _Banco);

            #region Parâmetros

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
            Param.ParameterName = "@ava_id";
            Param.Size = 4;
            Param.Value = ava_id;
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
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@aat_numeroAulas";
            Param.Size = 4;
            Param.Value = aat_numeroAulas;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@aat_numeroFaltas";
            Param.Size = 4;
            Param.Value = aat_numeroFaltas;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@aat_numeroAusenciasCompensadas";
            Param.Size = 4;
            Param.Value = aat_numeroAusenciasCompensadas;
            qs.Parameters.Add(Param);

            #endregion Parâmetros

            qs.Execute();

            return qs.Return;
        }


        /// <summary>
        /// Busca as notas das avaliações da secretaria para o boletim do aluno.
        /// Todas as notas em todas as turmas que ele passou naquele ano letivo (buscando pelo mtu_id informado).
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="mtu_id">ID da matrícula que será buscadas as notas do ano letivo</param>
        /// <returns></returns>
        public DataTable SelecionaBoletimAlunoAvaliacoesSecretaria(long alu_id, int mtu_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_Relatorio_0001_SubBoletimEscolarAlunoAvaliacoes", _Banco);

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

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Busca as notas das avaliações da secretaria para o boletim do aluno.
        /// Todas as notas em todas as turmas que ele passou naquele ano letivo (buscando pelo mtu_id informado).
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="mtu_id">ID da matrícula que será buscadas as notas do ano letivo</param>
        /// <returns></returns>
        public DataTable SelecionaAvaliacoesLiberadasView(long alu_id, int mtu_id)
        {
            QuerySelect qs = new QuerySelect("SELECT * FROM VW_AvaliacoesLiberadas_Por_TurmaAluno WHERE alu_id = @alu_id AND mtu_id = @mtu_id", _Banco);

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

        /// <summary>
        /// Busca o boletim do aluno.
        /// Todas as notas em todas as turmas que ele passou naquele ano letivo (buscando pelo mtu_id informado).
        /// Busca também disciplinas eletivas que o aluno cursou (tem nota) mas que a turma atual não oferece.
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="mtu_id">ID da matrícula que será buscadas as notas do ano letivo</param>
        /// <returns></returns>
        public DataTable SelecionaBoletimAluno(long alu_id, int mtu_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_Relatorio_0001_SubBoletimEscolarAluno", _Banco);

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

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Busca o boletim do aluno.
        /// Todas as notas em todas as turmas que ele passou naquele ano letivo (buscando pelo mtu_id informado).
        /// Busca também disciplinas eletivas que o aluno cursou (tem nota) mas que a turma atual não oferece.
        /// </summary>
        /// <param name="dtAlunoMatriculaTurma">Alunos.</param>
        /// <returns>DataTable com os dados do boletim.</returns>
        public DataTable SelecionaBoletimAluno(DataTable dtAlunoMatriculaTurma)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_Relatorio_0001_SubBoletimEscolarAluno", _Banco);

            #region PARAMETROS

            SqlParameter sqlParam = new SqlParameter();
            sqlParam.DbType = DbType.Int64;
            sqlParam.ParameterName = "@alu_id";
            sqlParam.Size = 8;
            sqlParam.Value = DBNull.Value;
            qs.Parameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.DbType = DbType.Int32;
            sqlParam.ParameterName = "@mtu_id";
            sqlParam.Size = 4;
            sqlParam.Value = DBNull.Value;
            qs.Parameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@alunoMatriculaTurma";
            sqlParam.SqlDbType = SqlDbType.Structured;
            sqlParam.TypeName = "TipoTabela_AlunoMatriculaTurma";
            sqlParam.Value = dtAlunoMatriculaTurma;
            qs.Parameters.Add(sqlParam);

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Seleciona ou Corrige a frequencia acumulada do aluno.
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="tpc_id">Tcp_id a corrigir</param>
        /// <param name="mtu_id">Mtu_Id</param>
        /// <param name="realizaUpdateFrequenciaAcumulada">Se realiza ou não a alteração</param>
        /// <param name="calcularQtAulasNovamente">Se calcula a quantidade de aulas novamente</param>
        /// <param name="buscarLancamentosDocente">Se busca lancamentos do docente</param>
        /// <param name="totalRecords"></param>
        /// <returns>Resultados</returns>
        public DataTable SelecionaCorrigeFrequenciaAcumulada(
            long alu_id
            , int tpc_id
            , int mtu_id
            , byte realizaUpdateFrequenciaAcumulada
            , byte calcularQtAulasNovamente
            , byte buscarLancamentosDocente
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_AlunoAvaliacaoTurma_CorrigeFrequenciaAcumulada", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_idBuscar";
                Param.Size = 8;
                Param.Value = alu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_idBuscar";
                Param.Size = 4;
                if (tpc_id > 0)
                    Param.Value = tpc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@mtu_idBuscar";
                Param.Size = 4;
                if (mtu_id > 0)
                    Param.Value = mtu_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int16;
                Param.ParameterName = "@realizaUpdateFrequenciaAcumulada";
                Param.Size = 1;
                if (realizaUpdateFrequenciaAcumulada > 0)
                    Param.Value = realizaUpdateFrequenciaAcumulada;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int16;
                Param.ParameterName = "@calcularQtAulasNovamente";
                Param.Size = 1;
                if (calcularQtAulasNovamente > 0)
                    Param.Value = calcularQtAulasNovamente;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int16;
                Param.ParameterName = "@buscarLancamentosDocente";
                Param.Size = 1;
                if (buscarLancamentosDocente > 0)
                    Param.Value = buscarLancamentosDocente;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

                qs.Execute();

                totalRecords = qs.Return.Rows.Count;

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Lista os mut_id de um aluno
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="mtu_id">mtu_id</param>
        /// <param name="totalRecords"></param>
        /// <returns>Resultados</returns>
        public DataTable ListaMtuDeAluno(
            long alu_id
            , int mtu_id
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_AlunoAvaliacaoTurma_ListaMtuDeAluno", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_idBuscar";
                Param.Size = 8;
                Param.Value = alu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@mtu_idBuscar";
                Param.Size = 4;
                Param.Value = mtu_id;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

                qs.Execute();

                totalRecords = qs.Return.Rows.Count;

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Busca a última frequência acumulada para a matrícula do aluno na turma e calendário
        /// informados.
        /// Busca: O último COC fechado (efetivação) antes do tpc_id informado (caso o tpc_id não seja informado,
        /// busca o tpc_id em que se encontra a data da movimentação).
        /// Caso não encontre nenhum COC fechado (efetivação), busca a porcentagem de frequência
        /// armazenada na movimentação em que o aluno entrou na turma (mtu_idAtual = mtu_id passado
        /// por parâmetro).
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="mtu_id">ID da matrícula do aluno</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="tpc_id">ID do período do calendário que está sendo efetivado</param>
        /// <param name="dataMovimentacao">Data de realização da movimentação</param>
        /// <param name="UltimaFrequenciaAcumulada">Retorno - % da última frequência acumulada armazenada</param>
        /// <param name="dataInicioUltimaFrequenciaAcumulada">Retorno - Data de início do cálculo da última frequência acumulada armazenada</param>
        /// <param name="dataFimUltimaFrequenciaAcumulada">Retorno - Data de fim do cálculo da última frequência acumulada armazenada</param>
        /// <returns>True caso encontrou alguma frequência armazenada, e false caso contrário ou se a frequência encontrada for Null</returns>
        public bool SelectUltimaFrequenciaAcumulada_Aluno
        (
            long tur_id
            , long alu_id
            , int mtu_id
            , int cal_id
            , ref int tpc_id
            , DateTime dataMovimentacao
            , out decimal UltimaFrequenciaAcumulada
            , out DateTime dataInicioUltimaFrequenciaAcumulada
            , out DateTime dataFimUltimaFrequenciaAcumulada
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_AlunoAvaliacaoTurma_SelectFrequenciaAcumulada_Aluno", _Banco);

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
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            Param.Value = cal_id;
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
            Param.DbType = DbType.Date;
            Param.ParameterName = "@dataMovimentacao";
            Param.Size = 20;
            if (dataMovimentacao != new DateTime())
                Param.Value = dataMovimentacao;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            if (qs.Return.Rows.Count > 0)
            {
                UltimaFrequenciaAcumulada = Convert.ToDecimal
                    (qs.Return.Rows[0]["UltimaFrequenciaAcumulada"] != DBNull.Value
                         ? qs.Return.Rows[0]["UltimaFrequenciaAcumulada"]
                         : 0);
                dataInicioUltimaFrequenciaAcumulada = Convert.ToDateTime
                    (qs.Return.Rows[0]["dataInicioUltimaFrequenciaAcumulada"] != DBNull.Value
                        ? qs.Return.Rows[0]["dataInicioUltimaFrequenciaAcumulada"]
                        : new DateTime());
                dataFimUltimaFrequenciaAcumulada = Convert.ToDateTime
                    (qs.Return.Rows[0]["dataFimUltimaFrequenciaAcumulada"] != DBNull.Value
                        ? qs.Return.Rows[0]["dataFimUltimaFrequenciaAcumulada"]
                        : new DateTime());
                tpc_id = Convert.ToInt32
                    (qs.Return.Rows[0]["tpc_id"] != DBNull.Value
                         ? qs.Return.Rows[0]["tpc_id"]
                         : 0);

                return qs.Return.Rows[0]["UltimaFrequenciaAcumulada"] != null;
            }

            UltimaFrequenciaAcumulada = 0;
            dataInicioUltimaFrequenciaAcumulada = new DateTime();
            dataFimUltimaFrequenciaAcumulada = new DateTime();

            return false;
        }

        /// <summary>
        /// Retorna os dados da CLS_AlunoAvaliacaoTurma que estejam cadastradas na avaliação para a turma.
        /// </summary>
        /// <param name="tur_id">Id da turma - obrigatório</param>
        /// <param name="fav_id">Id do formato de avaliação - obrigatório</param>
        /// <param name="ava_id">Id da avaliação - obrigatório</param>
        /// <returns></returns>
        public DataTable SelectBy_TurmaAvaliacao
        (
            long tur_id
            , int fav_id
            , int ava_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_AlunoAvaliacaoTurma_SelectBy_TurmaAvaliacao", _Banco);

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
            Param.ParameterName = "@ava_id";
            Param.Size = 4;
            Param.Value = ava_id;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna uma entidade carregada, buscando pela "chave" da avaliação do aluno
        /// (parâmetros).
        /// </summary>
        /// <param name="tur_id">Id da turma - obrigatório</param>
        /// <param name="alu_id">Id do aluno - obrigatório</param>
        /// <param name="mtu_id">Id da matrícula do aluno na turma - obrigatório</param>
        /// <param name="fav_id">Id do formato de avaliação - obrigatório</param>
        /// <param name="ava_id">Id da avaliação - obrigatório</param>
        /// <returns>Entidade CLS_AlunoAvaliacaoTurma</returns>
        public CLS_AlunoAvaliacaoTurma LoadBy_ChaveAvaliacaoAluno
        (
            long tur_id
            , long alu_id
            , int mtu_id
            , int fav_id
            , int ava_id
        )
        {
            CLS_AlunoAvaliacaoTurma entity = new CLS_AlunoAvaliacaoTurma();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_AlunoAvaliacaoTurma_LoadBy_ChaveAvaliacaoAluno", _Banco);

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
            Param.ParameterName = "@fav_id";
            Param.Size = 4;
            Param.Value = fav_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ava_id";
            Param.Size = 4;
            Param.Value = ava_id;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            if (qs.Return.Rows.Count > 0)
            {
                entity = DataRowToEntity(qs.Return.Rows[0], entity);
            }

            return entity;
        }

        /// <summary>
        /// Retorna a soma da quantidade de tempos de aula da turma/disciplina selecionada
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="tpc_id">ID do tipo período calendário</param>
        /// <param name="tud_id">ID da turma disciplina</param>
        /// <param name="tipoLancamento">1 - Aulas planejadas / 2 - Período / 3 - Mensal / 4 - Aulas planejadas e mensal</param>
        /// <param name="fav_calculoQtdeAulasDadas">1 - Automático / 2 - Manual</param>
        /// <returns></returns>
        public long CalculaQtdeTemposAula
        (
            long tur_id
            , int tpc_id
            , long tud_id
            , byte tipoLancamento
            , byte fav_calculoQtdeAulasDadas
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_AlunoAvaliacaoTurma_CalculaQtdeTemposAula", _Banco);

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
            Param.Value = tpc_id;
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
            Param.DbType = DbType.Int16;
            Param.ParameterName = "@tipoLancamento";
            Param.Size = 1;
            Param.Value = tipoLancamento;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int16;
            Param.ParameterName = "@fav_calculoQtdeAulasDadas";
            Param.Size = 1;
            Param.Value = fav_calculoQtdeAulasDadas;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            if (qs.Return.Rows.Count > 0)
                return string.IsNullOrEmpty(qs.Return.Rows[0]["QtAulasAluno"].ToString()) ? 0 : 
                       Convert.ToInt64(qs.Return.Rows[0]["QtAulasAluno"]);
            return 0;
        }

        /// <summary>
        /// O método carrega dados do boletim de um aluno de anos anteriores.
        /// </summary>
        /// <param name="alu_id">ID do aluno.</param>
        /// <param name="mtu_id">ID da matrícula turma do aluno.</param>
        /// <param name="controleOrdemDisciplina">Flag que indica se as disciplinas terão controle na ordenação.</param>
        /// <returns></returns>
        public DataTable SelecionaBoletimAlunoAnosAnteriores(long alu_id, int mtu_id, bool controleOrdemDisciplina)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_AlunoAvaliacaoTurma_SelecionaBoletimAlunoAnosAnteriores", _Banco);

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
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@controleOrdemDisciplina";
                Param.Size = 1;
                Param.Value = controleOrdemDisciplina;
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
        /// Seleciona os dados das notas da tela de VisualizaConteudo
        /// </summary>
        /// <param name="tipo">Tipo de busca: 
        ///                    "Turma" seleciona os dados das notas da turma por escola, ano e turma
        ///                    "Aluno" seleciona os dados das notas do aluno na turma por id de aluno e id de matricula na turma</param>
        /// <param name="parametro1">Nome OU código da escola / Id do aluno</param>
        /// <param name="parametro2">Ano letivo / Id da matricula do aluno na turma</param>
        /// <param name="parametro3">Código da turma / Vazio</param>
        /// <returns>Retorna dados das notas</returns>
        public DataSet SelecionaVisualizaConteudo(string tipo, string parametro1, string parametro2, string parametro3)
        {
            //Grava em DataSet pois retorna vários selects
            DataSet dsRetorno = new DataSet();

            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlConnection con = new SqlConnection(_Banco.GetConnection.ConnectionString);

            adapter.SelectCommand = new SqlCommand("NEW_CLS_AlunoAvaliacaoTurma_VisualizaConteudo", con);
            adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

            adapter.SelectCommand.Parameters.Add("tipo", SqlDbType.VarChar, 20);
            adapter.SelectCommand.Parameters["tipo"].Value = tipo;

            adapter.SelectCommand.Parameters.Add("parametro1", SqlDbType.VarChar, 200);
            adapter.SelectCommand.Parameters["parametro1"].Value = parametro1;

            adapter.SelectCommand.Parameters.Add("parametro2", SqlDbType.VarChar, 20);
            adapter.SelectCommand.Parameters["parametro2"].Value = parametro2;

            adapter.SelectCommand.Parameters.Add("parametro3", SqlDbType.VarChar, 50);
            adapter.SelectCommand.Parameters["parametro3"].Value = parametro3;

            adapter.Fill(dsRetorno);

            return dsRetorno;
        }

        #endregion Métodos

        #region Métodos Sobrescritos

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
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CLS_AlunoAvaliacaoTurma entity)
        {
            entity.aat_id = Convert.ToInt32(qs.Return.Rows[0][0]);
            return (entity.aat_id > 0);
        }

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_AlunoAvaliacaoTurma entity)
        {
            base.ParamInserir(qs, entity);

            // Verificação pq frequência pode ser igual a zero
            if (entity.aat_frequencia > -1)
                qs.Parameters["@aat_frequencia"].Value = entity.aat_frequencia;
            else
                qs.Parameters["@aat_frequencia"].Value = DBNull.Value;

            // Verificação pq número de faltas pode ser igual a zero
            if (entity.aat_numeroFaltas > -1)
                qs.Parameters["@aat_numeroFaltas"].Value = entity.aat_numeroFaltas;
            else
                qs.Parameters["@aat_numeroFaltas"].Value = DBNull.Value;

            qs.Parameters["@aat_relatorio"].DbType = DbType.String;
            qs.Parameters["@aat_relatorio"].Size = Int32.MaxValue;

            qs.Parameters["@aat_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@aat_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamAlterar(QueryStoredProcedure qs, CLS_AlunoAvaliacaoTurma entity)
        {
            base.ParamAlterar(qs, entity);

            // Verificação pq frequência pode ser igual a zero
            if (entity.aat_frequencia > -1)
                qs.Parameters["@aat_frequencia"].Value = entity.aat_frequencia;
            else
                qs.Parameters["@aat_frequencia"].Value = DBNull.Value;

            // Verificação pq número de faltas pode ser igual a zero
            if (entity.aat_numeroFaltas > -1)
                qs.Parameters["@aat_numeroFaltas"].Value = entity.aat_numeroFaltas;
            else
                qs.Parameters["@aat_numeroFaltas"].Value = DBNull.Value;

            qs.Parameters["@aat_relatorio"].DbType = DbType.String;
            qs.Parameters["@aat_relatorio"].Size = Int32.MaxValue;

            qs.Parameters.RemoveAt("@aat_dataCriacao");
            qs.Parameters.RemoveAt("@aat_registroexterno");
            qs.Parameters.RemoveAt("@aat_frequenciaAcumulada");
            qs.Parameters.RemoveAt("@aat_frequenciaAcumuladaCalculada");

            qs.Parameters["@aat_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity">Entidade CLS_AlunoAvaliacaoTurma</param>
        /// <returns>true = sucesso | false = fracasso</returns>
        protected override bool Alterar(CLS_AlunoAvaliacaoTurma entity)
        {
            __STP_UPDATE = "NEW_CLS_AlunoAvaliacaoTurma_Update";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamDeletar(QueryStoredProcedure qs, CLS_AlunoAvaliacaoTurma entity)
        {
            base.ParamDeletar(qs, entity);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@aat_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@aat_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity">Entidade CLS_AlunoAvaliacaoTurma</param>
        /// <returns>true = sucesso | false = fracasso</returns>
        public override bool Delete(CLS_AlunoAvaliacaoTurma entity)
        {
            __STP_DELETE = "NEW_CLS_AlunoAvaliacaoTurma_Update_Situacao";
            return base.Delete(entity);
        }

        #endregion Métodos Sobrescritos
    }
}