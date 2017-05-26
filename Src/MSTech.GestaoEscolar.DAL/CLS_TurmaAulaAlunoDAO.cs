/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL.Abstracts;

namespace MSTech.GestaoEscolar.DAL
{
    using System.Data.SqlClient;

    /// <summary>
    /// 
    /// </summary>
    public class CLS_TurmaAulaAlunoDAO : AbstractCLS_TurmaAulaAlunoDAO
	{
        /// <summary>
        /// Retornar a frequencia de territórios do saber
        /// </summary>
        /// <param name="tud_id"></param>
        /// <param name="tau_id"></param>
        /// <returns></returns>
        public DataTable SelecionaFrequenciaAulaTurmaDisciplinaTerritorio(long tud_id, int tau_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAulaAluno_Frequencia_SelectBy_TurmaDisciplina_Territorio", _Banco);

            #region Parâmetros

            Param = qs.NewParameter();
            Param.ParameterName = "@tud_id";
            Param.DbType = DbType.Int64;
            Param.Size = 8;
            Param.Value = tud_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.ParameterName = "@tau_id";
            Param.DbType = DbType.Int32;
            Param.Size = 4;
            Param.Value = tau_id;
            qs.Parameters.Add(Param);

            #endregion Parâmetros

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna as aulas que os alunos tiveram dentro do bimestre em matrículas diferentes das enviadas no filtro.
        /// </summary>
        /// <param name="tbAlunosPeriodos"></param>
        /// <returns></returns>
        public DataTable SelecionaPor_Por_DiferentesMatriculas_Periodo(DataTable tbAlunosPeriodos)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAulaAluno_SelectPor_DiferentesMatriculas_Periodo", _Banco);
            qs.TimeOut = 0;


            #region PARAMETROS
            
            SqlParameter sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@tbAlunosPeriodos";
            sqlParam.SqlDbType = SqlDbType.Structured;
            sqlParam.TypeName = "TipoTabela_MatriculaTurmaDisciplinaPeriodo";
            sqlParam.Value = tbAlunosPeriodos;
            qs.Parameters.Add(sqlParam);

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }


        /// <summary>
        /// Retorna os dados da CLS_TurmaAulaAluno que sejam pela 
        /// "chave" da matrícula do aluno na disciplina.
        /// </summary>
        /// <param name="tud_id">Id da turma disciplina - obrigatório</param>
        /// <param name="alu_id">Id do aluno - obrigatório</param>
        /// <param name="mtu_id">Id da matrícula do aluno na turma - obrigatório</param>
        /// <param name="mtd_id">Id da matrícula do aluno na disciplina - obrigatório</param>
        /// <returns>Lista de CLS_TurmaAulaAluno</returns>
        public List<CLS_TurmaAulaAluno> SelectBy_Disciplina_Aluno
        (
            Int64 tud_id
            , Int64 alu_id
            , Int32 mtu_id
            , Int32 mtd_id
        )
        {
            List<CLS_TurmaAulaAluno> lista = new List<CLS_TurmaAulaAluno>();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAulaAluno_SelectBy_Disciplina_Aluno", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            Param.Value = tud_id;
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
            Param.ParameterName = "@mtd_id";
            Param.Size = 4;
            Param.Value = mtd_id;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            DataTable dt = qs.Return;

            foreach (DataRow dr in dt.Rows)
            {
                CLS_TurmaAulaAluno entity = new CLS_TurmaAulaAluno();
                entity = DataRowToEntity(dr, entity);

                lista.Add(entity);
            }

            return lista;
        }

        /// <summary>
        /// Retorna o lançamento de frequência dos alunos que não foram excluídos logicamente
        /// </summary>                
        /// <param name="tud_id">ID da disciplina da turma</param>        
        /// <param name="tau_id">ID da aula da disciplina da turma</param>
        public DataTable SelectBy_FrequenciaTurmaDisciplina
        (
            long tud_id
            , int tau_id
            , DataTable dtTurmas
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAulaAluno_Frequencia_SelectBy_TurmaDisciplina", _Banco);
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
                Param.ParameterName = "@tau_id";
                Param.Size = 4;
                Param.Value = tau_id;
                qs.Parameters.Add(Param);

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.ParameterName = "@dtTurmas";
                sqlParam.TypeName = "TipoTabela_Turma";
                sqlParam.Value = dtTurmas;
                qs.Parameters.Add(sqlParam);

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
        /// Retorna o lançamento de frequência dos alunos que não foram excluídos logicamente
        /// </summary>                
        /// <param name="tud_id">ID da disciplina da turma</param>        
        /// <param name="tau_id">ID da aula da disciplina da turma</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="ordenacao">Tipo de ordenação dos alunos 0-Número chamada, 1-Nome do aluno</param>
        public DataTable SelectBy_TurmaDisciplina
        (
            long tud_id
            , int tau_id
            , Guid ent_id
            , byte ordenacao
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAulaAluno_SelectBy_TurmaDisciplina", _Banco);
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
                Param.ParameterName = "@tau_id";
                Param.Size = 4;
                Param.Value = tau_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@ordenacao";
                Param.Size = 1;
                Param.Value = ordenacao;
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
        /// Retorna os lançamentos de frequência dos alunos, filtrando os alunos com ou sem deficiência, dependendo do docente.
        /// </summary>                
        /// <param name="tud_id">ID da disciplina da turma</param>        
        /// <param name="tau_id">ID da aula da disciplina da turma</param>
        /// <param name="tdc_id">ID do tipo de docente</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="ordenacao">Tipo de ordenação dos alunos 0-Número chamada, 1-Nome do aluno</param>
        public DataTable SelectBy_TurmaDisciplinaFiltroDeficiencia
        (
            long tud_id
            , int tau_id
            , byte tdc_id
            , Guid ent_id
            , byte ordenacao
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAulaAluno_SelectBy_TurmaDisciplinaFiltroDeficiencia", _Banco);
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
                Param.ParameterName = "@tau_id";
                Param.Size = 4;
                Param.Value = tau_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tdc_id";
                Param.Size = 1;
                Param.Value = tdc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@ordenacao";
                Param.Size = 1;
                Param.Value = ordenacao;
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
        /// Retorna todas as entidades da CLS_TurmaAulaAluno
        /// de todos os alunos matriculados na disicplina, para as aulas informadas (tau_id).
        /// </summary>                
        /// <param name="tud_id">ID da disciplina da turma</param>        
        /// <param name="tau_id">ID das aulas da disciplina da turma</param>
        public List<CLS_TurmaAulaAluno> SelectBy_Disciplina_Aulas
        (
            long tud_id
            , string tau_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAulaAluno_SelectBy_Disciplina_Aulas", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            Param.Value = tud_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@tau_id";
            if (tau_id != "")
                Param.Value = tau_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            List<CLS_TurmaAulaAluno> lista =
                (from DataRow dr in qs.Return.Rows
                 select DataRowToEntity(dr, new CLS_TurmaAulaAluno())).ToList();

            return lista;
        }

        /// <summary>
        /// Retorna datatable contendo todos os alunos que possuam anotações.
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param> 
        /// <param name="tau_id">ID da aula da disciplina da turma</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        public DataTable SelectBy_tud_id_Return_anotacao
        (
            long tud_id   
            , int tau_id
            , Guid ent_id
            , DataTable dtTurmas
        )
        {            
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAulaAluno_SelectBy_tud_id_Return_anotacao", _Banco);

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
                Param.ParameterName = "@tau_id";
                Param.Size = 4;
                Param.Value = tau_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.ParameterName = "@dtTurmas";
                sqlParam.TypeName = "TipoTabela_Turma";
                sqlParam.Value = dtTurmas;
                qs.Parameters.Add(sqlParam);

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
        /// Retorna as aulas da turmaDisciplina. Se passado o id do aluno, retorna a frequência
        /// do aluno pra cada aula.
        /// </summary>
        /// <param name="tud_id">id da turma</param>
        /// <param name="tpc_id">id do período</param>
        /// <param name="usu_id">id do usuario que corresponde ao docente</param>
        /// <param name="data_inicio">data inicio</param>
        /// <param name="data_final">data final</param>
        /// <param name="tdt_posicao">Posição do docente</param>
        /// <param name="usuario_superior">Indica se é um usuário superior</param>
        /// <param name="tud_idRelacionada">Disciplina relacionada</param>
        /// <returns>
        /// Quando a data inicio e data final for zero retorna os dados das 5 aulas
        /// caso contrário retorna todas as aulas dentro do intervalo das datas.
        /// </returns>
        public DataTable SelectBy_TurmaDisciplina
        (
             long tud_id
            , int tpc_id
            , Guid usu_id
            , DateTime data_inicio
            , DateTime data_final
            , byte tdt_posicao
            , bool usuario_superior
            , long tud_idRelacionada
            , DataTable dtTurmas
        )
        {            
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAulaAluno_SelectBy_TurmaDisciplina_Aluno", this._Banco);

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
            Param.Size = 5;
            Param.Value = tpc_id;
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
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@data_inicio";
            Param.Size = 5;
            if (data_inicio != new DateTime())
                Param.Value = data_inicio;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@data_final";
            Param.Size = 5;
            if (data_final != new DateTime())
                Param.Value = data_final;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tdt_posicao";
            Param.Size = 1;
            if (tdt_posicao > 0)
                Param.Value = tdt_posicao;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_idRelacionada";
            Param.Size = 8;
            if (tud_idRelacionada > 0)
                Param.Value = tud_idRelacionada;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.Size = 1;
            Param.ParameterName = "@usuario_superior";
            Param.Value = usuario_superior;
            qs.Parameters.Add(Param);

            SqlParameter sqlParam = new SqlParameter();
            sqlParam.SqlDbType = SqlDbType.Structured;
            sqlParam.ParameterName = "@dtTurmas";
            sqlParam.TypeName = "TipoTabela_Turma";
            sqlParam.Value = dtTurmas;
            qs.Parameters.Add(sqlParam);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna Turma, Disciplina, Professor, data da aula e anotações do Aluno.
        /// </summary>
        public DataTable SelectAulasAluno
        (
             long alu_id
            , int cal_ano
            , Guid usu_id
            , bool usuario_superior
        )
        {            
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAulaAluno_SelectBy_Aluno", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                if (alu_id > 0)
                    Param.Value = alu_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_ano";
                Param.Size = 4;
                if (cal_ano > 0)
                    Param.Value = cal_ano;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@usu_id";
                Param.Size = 16;
                Param.Value = usu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@usuario_superior";
                Param.Value = usuario_superior;
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
        /// Retorna as frequências dos alunos matriculados na disciplina e períodos selecionados.
        /// </summary>
        /// <param name="tud_id">ID da turma disciplina</param>
        /// <param name="tpc_id">ID do tipo período calendário</param>
        /// <param name="dataInicio">Data de ínicio do período</param>
        /// <param name="dataFim">Data de fim do período</param>
        /// <returns>DataTable de frequências por disciplina dos alunos</returns>
        public DataTable SelectFreqDisciplinaByTurmaPeriodoData
        (
            long tur_id
            , int tpc_id
            , DateTime dataInicio
            , DateTime dataFim
        )
        {

            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAulaAluno_SelectFreqDisciplinaByDisciplinaPeriodoData", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 16;
                Param.Value = tur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 8;
                Param.Value = tpc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@dataInicio";
                Param.Value = dataInicio;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@dataFim";
                Param.Value = dataFim;
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
        /// Retorna a frequência global dos alunos matriculados na turma e períodos selecionados.
        /// </summary>
        /// <param name="tud_id">ID da turma disciplina</param>
        /// <param name="tpc_id">ID do tipo período calendário</param>
        /// <param name="tau_id">ID da aula</param>
        /// <returns>DataTable de frequência global</returns>
        public DataTable VerificaLancamentoFrequencia
        (
            long tud_id
            , int tpc_id
            , int tau_id
        )
        {

            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAulaAluno_VerificaLancamentoFrequencia", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 16;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 8;
                Param.Value = tpc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tau_id";
                Param.Size = 8;
                Param.Value = tau_id;
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
        /// Retorna a frequência global dos alunos matriculados na turma e períodos selecionados.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="tpc_id">ID do tipo período calendário</param>
        /// <param name="dataInicio">Data de ínicio do período</param>
        /// <param name="dataFim">Data de fim do período</param>
        /// <returns>DataTable de frequência global</returns>
        public DataTable SelectFreqGlobalByTurmaPeriodoData
        (
            long tur_id
            , int tpc_id
            , DateTime dataInicio
            , DateTime dataFim
        )
        {

            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAulaAluno_SelectFreqGlobalByTurmaPeriodoData", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 16;
                Param.Value = tur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 8;
                Param.Value = tpc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@dataInicio";
                Param.Value = dataInicio;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@dataFim";
                Param.Value = dataFim;
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
        /// Salva os dados das frequências dos alunos.
        /// </summary>
        /// <param name="dtTurmaAulaAluno">DataTable de dados do listão de frequência.</param>
        /// <returns>True em caso de sucesso.</returns>
        public bool SalvaFrequenciaAlunos(DataTable dtTurmaAulaAluno)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_TurmaAulaAluno_SalvaFrequenciaAlunos", _Banco);

            try
            {
                #region Parâmetro

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbTurmaAulaAluno";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_TurmaAulaAluno";
                sqlParam.Value = dtTurmaAulaAluno;
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
        /// Retorna as aulas da turmaDisciplina. Se passado o id do aluno, retorna a frequência
        /// do aluno pra cada aula.
        /// </summary>
        /// <param name="tud_id">id da turma</param>
        /// <param name="tpc_id">id do período</param>
        /// <param name="usu_id">id do usuario que corresponde ao docente</param>
        /// <param name="data_inicio">data inicio</param>
        /// <param name="data_final">data final</param>
        /// <param name="tdt_posicao">Posição do docente</param>
        /// <param name="usuario_superior">Indica se é um usuário superior</param>
        /// <param name="tud_idRelacionada">Disciplina relacionada</param>
        /// <returns>
        /// Quando a data inicio e data final for zero retorna os dados das 5 aulas
        /// caso contrário retorna todas as aulas dentro do intervalo das datas.
        /// </returns>
        public DataTable SelectBy_TurmaDisciplinaTerritorio
        (
             long tud_id
            , int tpc_id
            , Guid usu_id
            , DateTime data_inicio
            , DateTime data_final
            , byte tdt_posicao
            , bool usuario_superior
            , long tud_idRelacionada
            , DataTable dtTurmas
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAulaAluno_Territorio_SelectBy_TurmaDisciplina_Aluno", this._Banco);

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
            Param.Size = 5;
            Param.Value = tpc_id;
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
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@data_inicio";
            Param.Size = 5;
            if (data_inicio != new DateTime())
                Param.Value = data_inicio;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@data_final";
            Param.Size = 5;
            if (data_final != new DateTime())
                Param.Value = data_final;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tdt_posicao";
            Param.Size = 1;
            if (tdt_posicao > 0)
                Param.Value = tdt_posicao;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_idRelacionada";
            Param.Size = 8;
            if (tud_idRelacionada > 0)
                Param.Value = tud_idRelacionada;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.Size = 1;
            Param.ParameterName = "@usuario_superior";
            Param.Value = usuario_superior;
            qs.Parameters.Add(Param);

            SqlParameter sqlParam = new SqlParameter();
            sqlParam.SqlDbType = SqlDbType.Structured;
            sqlParam.ParameterName = "@dtTurmas";
            sqlParam.TypeName = "TipoTabela_Turma";
            sqlParam.Value = dtTurmas;
            qs.Parameters.Add(sqlParam);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna os dados das anotações do aluno, tanto do docente como da equipe gestora.
        /// </summary>
        /// <param name="ano">Ano das sondagens</param>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="mtu_id">Id da matrícula do aluno na turma</param>
        /// <returns></returns>
        public DataTable SelectAnotacoesBy_Aluno(int ano, long alu_id, int mtu_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAulaAluno_SelectAnotacoesBy_Aluno", _Banco);
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

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_TurmaAulaAluno entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@taa_anotacao"].DbType = DbType.String;

            qs.Parameters["@taa_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@taa_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamAlterar(QueryStoredProcedure qs, CLS_TurmaAulaAluno entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters["@taa_anotacao"].DbType = DbType.String;

            qs.Parameters.RemoveAt("@taa_dataCriacao");
            qs.Parameters["@taa_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity">Entidade CLS_TurmaAulaAluno</param>
        /// <returns>true = sucesso | false = fracasso</returns> 
        protected override bool Alterar(CLS_TurmaAulaAluno entity)
        {
            __STP_UPDATE = "NEW_CLS_TurmaAulaAluno_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamDeletar(QueryStoredProcedure qs, CLS_TurmaAulaAluno entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            Param.Value = entity.tud_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tau_id";
            Param.Size = 1;
            Param.Value = entity.tau_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@alu_id";
            Param.Size = 8;
            Param.Value = entity.alu_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@mtd_id";
            Param.Size = 1;
            Param.Value = entity.mtd_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@mtu_id";
            Param.Size = 4;
            Param.Value = entity.mtd_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@taa_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@taa_dataAlteracao";
            Param.Size = 8;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity">Entidade CLS_TurmaAulaAluno</param>
        /// <returns>true = sucesso | false = fracasso</returns>     
        public override bool Delete(CLS_TurmaAulaAluno entity)
        {
            __STP_DELETE = "NEW_CLS_TurmaAulaAluno_Update_Situacao";
            return base.Delete(entity);
        }

		///// <summary>
        ///// Inseri os valores da classe em um registro ja existente
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem modificados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Alterar(CLS_TurmaAulaAluno entity)
        //{
        //    return base.Alterar(entity);
        //}
        ///// <summary>
        ///// Inseri os valores da classe em um novo registro
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem inseridos</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Inserir(CLS_TurmaAulaAluno entity)
        //{
        //    return base.Inserir(entity);
        //}
        ///// <summary>
        ///// Carrega um registro da tabela usando os valores nas chaves
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem carregados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Carregar(CLS_TurmaAulaAluno entity)
        //{
        //    return base.Carregar(entity);
        //}
        ///// <summary>
        ///// Exclui um registro do banco
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem apagados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Delete(CLS_TurmaAulaAluno entity)
        //{
        //    return base.Delete(entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Alterar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamAlterar(QueryStoredProcedure qs, CLS_TurmaAulaAluno entity)
        //{
        //    base.ParamAlterar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Carregar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamCarregar(QuerySelectStoredProcedure qs, CLS_TurmaAulaAluno entity)
        //{
        //    base.ParamCarregar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Deletar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamDeletar(QueryStoredProcedure qs, CLS_TurmaAulaAluno entity)
        //{
        //    base.ParamDeletar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Inserir
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_TurmaAulaAluno entity)
        //{
        //    base.ParamInserir(qs, entity);
        //}
        ///// <summary>
        ///// Salva o registro no banco de dados
        ///// </summary>
        ///// <param name="entity">Entidade com os dados para preenchimento para inserir ou alterar</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Salvar(CLS_TurmaAulaAluno entity)
        //{
        //    return base.Salvar(entity);
        //}
        ///// <summary>
        ///// Realiza o select da tabela
        ///// </summary>
        ///// <returns>Lista com todos os registros da tabela</returns>
        //public override IList<CLS_TurmaAulaAluno> Select()
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
        //public override IList<CLS_TurmaAulaAluno> Select_Paginado(int currentPage, int pageSize, out int totalRecord)
        //{
        //    return base.Select_Paginado(currentPage, pageSize, out totalRecord);
        //}
        ///// <summary>
        ///// Recebe o valor do auto incremento e coloca na propriedade 
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CLS_TurmaAulaAluno entity)
        //{
        //    return base.ReceberAutoIncremento(qs, entity);
        //}
        ///// <summary>
        ///// Passa os dados de um datatable para uma entidade
        ///// </summary>
        ///// <param name="dr">DataRow do datatable preenchido</param>
        ///// <param name="entity">Entidade onde ser�o transferidos os dados</param>
        ///// <returns>Entidade preenchida</returns>
        //public override CLS_TurmaAulaAluno DataRowToEntity(DataRow dr, CLS_TurmaAulaAluno entity)
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
        //public override CLS_TurmaAulaAluno DataRowToEntity(DataRow dr, CLS_TurmaAulaAluno entity, bool limparEntity)
        //{
        //    return base.DataRowToEntity(dr, entity, limparEntity);
        //}
	}
}