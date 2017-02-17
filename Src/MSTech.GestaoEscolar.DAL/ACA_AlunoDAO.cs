/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL.Abstracts;
using MSTech.GestaoEscolar.Entities;
using System.Data.SqlClient;

namespace MSTech.GestaoEscolar.DAL
{
    /// <summary>
    ///
    /// </summary>
    public class ACA_AlunoDAO : Abstract_ACA_AlunoDAO
    {
        #region Temporários

        /// <summary>
        /// Seleciona IDs da pessoa e da foto relacionados ao aluno. (todos alunos ativos)
        /// </summary>
        /// <returns></returns>
        public DataTable SelecionaPessoaFotoAtivos(int qtTop)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_SelecionaPessoaFotoAtivos", _Banco);
            qs.TimeOut = 0;

            try
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@qtTop";
                Param.Size = 4;
                Param.Value = qtTop;
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
        /// Insere um registro na tabela que guarda os ids dos arquivos redimensionados.
        /// Processo criado para diminuição do tamanho das imagens do banco do Core.
        /// </summary>
        /// <returns></returns>
        public void InsereArquivoRedimensionado(long arq_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CFG_ArquivoRedimensionado_Insert", _Banco);

            try
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@arq_id";
                Param.Size = 8;
                Param.Value = arq_id;
                qs.Parameters.Add(Param);

                qs.Execute();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Retorna as deficiências dos alunos informados
        /// </summary>
        /// <param name="alu_id">IDs dos alunos</param>
        /// <returns></returns>
        public DataTable SelectAlunos_Deficiencias
        (
            string alu_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_SelecionaAluno_Deficiencias", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@alu_id";
            Param.Value = alu_id;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }
             
        /// <summary>
        /// Retorna os dados do aluno para a exibição do boletim na area do aluno
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="mtu_id">Id da matricula turma a trazer os dados</param>
        /// <returns>Dados do aluno</returns>
        public DataTable AreaAluno_DadosTurmaAtualBoletim(long alu_id, int mtu_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_AreaAluno_DadosTurmaAtualBoletim", _Banco);
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
                if (mtu_id > 0)
                    Param.Value = mtu_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                return qs.Return;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #region Acesso Boletim Online

        /// <summary>
        /// Seleciona aluno por número de matricula na view VW_Alunos_Acesso_Boletim
        /// </summary>
        /// <param name="numMatricula">Numero de matricula</param>
        /// <returns></returns>
        public DataTable SelectAlunoAcessoBoletimView(string numMatricula)
        {
            QuerySelect qs = new QuerySelect("SELECT * FROM VW_Alunos_Acesso_Boletim WHERE numMatricula = @numMatricula", _Banco);

            #region Parâmetros

            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@numMatricula";
            Param.Size = 50;
            Param.Value = numMatricula;
            qs.Parameters.Add(Param);

            #endregion Parâmetros

            qs.Execute();

            return qs.Return;
        }

        #endregion Acesso Boletim Online

        /// <summary>
        /// Retorna todas as disciplinas da turma e aluno selecionados
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="mtu_id">ID da matrícula turma</param>
        /// <returns>DataTable de disciplinas</returns>
        public DataTable SelectDisciplinasPorTurmaAlunoView(long tur_id, long alu_id, int mtu_id)
        {
            QuerySelect qs = new QuerySelect("SELECT tud_id, tud_nome, segEpoca, concFinal, mediaFinal, totalFaltas, secretaria " +
                                             "FROM VW_Disciplinas_Por_TurmaAluno WHERE tur_id = @tur_id AND alu_id = @alu_id AND " +
                                             "mtu_id = @mtu_id ORDER BY secretaria, tud_id", _Banco);
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

                #endregion PARAMETROS

                qs.Execute();

                return qs.Return;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna todas as avalições da turma selecionada
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="mtu_id">ID da matrícula turma</param>
        /// <returns>DataTable de avalições</returns>
        public DataTable SelectAvaliacoesPorTurmaAlunoView(long tur_id, long alu_id, int mtu_id)
        {
            QuerySelect qs = new QuerySelect("SELECT tud_id, ava_nome, ava_id, concFinal, mediaFinal, totalFaltas, rp, tpc_ordem, tpc_id, secretaria " +
                                             "FROM VW_Avaliacoes_Por_TurmaAluno WHERE (tur_id = @tur_id AND alu_id = @alu_id AND " +
                                             "mtu_id = @mtu_id) OR (tur_id = 0 AND alu_id = 0 AND mtu_id = 0)  ORDER BY tpc_ordem", _Banco);
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

                #endregion PARAMETROS

                qs.Execute();

                return qs.Return;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna todas as turmas do aluno selecionado
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <returns>DataTable de turmas</returns>
        public DataTable SelectTurmasPorAlunoView(long alu_id)
        {
            QuerySelect qs = new QuerySelect("SELECT tur_mtu_id, tur_nome, tur_codigo, esc_nome, crp_descricao, cur_nome, " +
                                             "UltimaFrequenciaAcumulada, nomeCurso, nomePeriodo, mtu_situacao " +
                                             "FROM VW_Turmas_Por_Aluno WHERE alu_id = @alu_id ORDER BY cal_ano DESC, tur_nome ASC", _Banco);
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
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        public DataTable SelectView()
        {
            QuerySelect qs = new QuerySelect("SELECT * FROM VW_Alunos_Acesso_Boletim WHERE matricula = @matricula", _Banco);

            //QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("VW_Alunos_Acesso_Boletim", _Banco);
            //#region Parâmetros

            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@matricula";
            Param.Size = 20;
            Param.Value = "asdasd";
            qs.Parameters.Add(Param);

            //#endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Seleciona os alunos ativos e matriculados na turma, ordenados por nome.
        /// </summary>
        /// <param name="tur_id">ID da turma.</param>
        /// <returns>DataTable contendo dados dos alunos ativos e matriculados.</returns>
        public DataTable SelecionarAlunosAtivosPorTurma
        (
            Int64 tur_id
        )
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_SelecionaAtivosPorTurma", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 16;
                Param.Value = tur_id;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                    dt = qs.Return;

                return dt;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Seleciona os alunos matriculados na turma e ordena por nome ou matricula.
        /// </summary>
        /// <param name="tur_id">ID da turma.</param>
        /// <param name="order">Bit de ordenação 0 - Por matricula / 1 - Por Nome</param>
        /// <returns>DataTable contendo dados dos alunos matriculados.</returns>
        public DataTable SelecionarAlunosPorTurma
        (
            Int64 tur_id
            , byte order
        )
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_SelectByTurma", _Banco);
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
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@order";
                Param.Size = 1;
                Param.Value = order;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                    dt = qs.Return;

                return dt;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Seleciona os registro do aluno duplicado, de acordo com o id passado.
        /// </summary>
        /// <param name="pes_id">Id da pessoa.</param>
        /// <returns>DataTable contendo os dados de cada registro do aluno duplicado.</returns>
        public DataTable SelecionarAlunosDuplicados
        (
            Guid pes_id
        )
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_SelecionarAlunosDuplicados", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@pes_id";
                Param.Size = 16;
                Param.Value = pes_id;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                    dt = qs.Return;

                return dt;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }
     

        /// <summary>
        /// Retorna os alunos matriculados em turmas eletivas
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="pes_nome">Nome do aluno</param>
        /// <param name="tur_codigo">Código da turma</param>
        /// <param name="cal_id">ID do calendário escolar</param>
        /// <param name="filtraTurma">Indica se filtrará pelo ID ou apenas pelo curriculo da turma</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="adm">Indica se o usuário é administrador</param>
        /// <param name="usu_id">ID do usuário logado</param>
        /// <param name="gru_id">ID do grupo do usuário logado</param>
        public DataTable SelectBy_CursoPeriodoTurmaMultisseriada
        (
            int esc_id
            , int uni_id
            , Int64 tur_id
            , int cal_id
            , int tds_id
            , byte tud_tipo
            , string pes_nome
            , string tur_codigo
            , DataTable dtAlunos
            , Guid ent_id
            , bool adm
            , Guid usu_id
            , Guid gru_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_SelectBy_TurmaMultisseriada", _Banco);
            try
            {
                #region PARAMETROS

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
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pes_nome";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(pes_nome))
                    Param.Value = pes_nome;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tur_codigo";
                Param.Size = 30;
                if (!string.IsNullOrEmpty(tur_codigo))
                    Param.Value = tur_codigo;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.ParameterName = "@TipoTabela_Aluno";
                sqlParam.TypeName = "TipoTabela_Aluno";
                sqlParam.Value = dtAlunos;
                qs.Parameters.Add(sqlParam);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                Param.Value = cal_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tds_id";
                Param.Size = 4;
                if (tds_id > 0)
                {
                    Param.Value = tds_id;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tud_tipo";
                Param.Size = 1;
                if (tud_tipo > 0)
                {
                    Param.Value = tud_tipo;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
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
                Param.Value = usu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@gru_id";
                Param.Size = 16;
                Param.Value = gru_id;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                return qs.Return;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna os alunos matriculados em turmas eletivas
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="cal_id">ID do calendário escolar</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="adm">Indica se o usuário é administrador</param>
        /// <param name="usu_id">ID do usuário logado</param>
        /// <param name="gru_id">ID do grupo do usuário logado</param>
        public DataTable SelectBy_MatriculaMultisseriaAluno_TurmaMultisseriada
        (
            int esc_id
            , int uni_id
            , Int64 tur_id
            , int cal_id
            , Guid ent_id
            , bool adm
            , Guid usu_id
            , Guid gru_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_SelectBy_MatriculaMultisseriaAluno_TurmaMultisseriada", _Banco);
            try
            {
                #region PARAMETROS

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
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                Param.Value = cal_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
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
                Param.Value = usu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@gru_id";
                Param.Size = 16;
                Param.Value = gru_id;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                return qs.Return;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna os alunos matriculados em turmas eletivas por disciplina da turma
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        public DataTable SelectBy_MatriculaEletivasAluno_TurmaEletiva
        (
            long tud_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_SelectBy_MatriculaEletivasAluno_TurmaEletiva", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                return qs.Return;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        
        /// <summary>
        /// Seleciona o aluno pelo nome, data de nascimento e nome da mãe,
        /// e carrega a entidade ACA_Aluno.
        /// </summary>
        /// <param name="entity">Entidade ACA_Aluno</param>
        /// <param name="pes_nome">Nome do aluno.</param>
        /// <param name="pes_dataNascimento">Data de nascimento do aluno.</param>
        /// <param name="pes_nomeMae">Nome da mãe.</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <returns> True = Encontrou o aluno. / False = Não encontrou.</returns>
        public bool SelectBy_Nome_DataNasc_NomeMae
        (
            string pes_nome
            , DateTime pes_dataNascimento
            , string pes_nomeMae
            , Guid ent_id
            , out ACA_Aluno entity
        )
        {
            entity = new ACA_Aluno();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_SelectBy_NomeDataNascNomeMae", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pes_nome";
                Param.Size = 200;
                Param.Value = pes_nome;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@pes_dataNascimento";
                Param.Size = 20;
                Param.Value = pes_dataNascimento;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pes_nomeMae";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(pes_nomeMae))
                    Param.Value = pes_nomeMae;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                {
                    entity = DataRowToEntity(qs.Return.Rows[0], entity, false);
                    return true;
                }

                return false;
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
        /// -Metodo irá verificar se no cadastro Rápido de aluno existe uma matricula estadual
        /// -na entidade que está logado.
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="ent_id">Entidade que esta logado</param>
        /// <param name="alc_matriculaEstadual">Numero da matricula Estadual do aluno</param>
        /// <returns></returns>
        public Boolean SelectBy_Entidade_MatriculaEstadual(long alu_id, Guid ent_id, String alc_matriculaEstadual)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_SelectBy_Entidade_MatriculaEstadual", _Banco);
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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@alc_matriculaEstadual";
                Param.Size = 50;
                Param.Value = alc_matriculaEstadual ?? "";
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                //Retorna o count da stored: true ou false
                return qs.Return.Rows.Count > 0;
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
        /// -Metodo irá verificar se no cadastro Rápido de aluno existe uma matricula estadual
        /// -na entidade que está logado.
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="ent_id">Entidade que esta logado</param>
        /// <param name="alc_matricula">Numero da matricula do aluno</param>
        /// <returns></returns>
        public Boolean SelectBy_Entidade_NumeroMatricula(long alu_id, Guid ent_id, string alc_matricula)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_SelectBy_Entidade_NumeroMatricula", _Banco);
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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@alc_matricula";
                Param.Size = 50;
                Param.Value = alc_matricula;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                //Retorna o count da stored: true ou false
                return (Convert.ToInt32(qs.Return.Rows[0][0]) > 0);
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
        /// Verifica se ja existe aluno cadastrado com o mesmo numero de protocolo.
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="ent_id">Entidade que esta logado</param>
        /// <param name="protocolo">Numero do protocolo</param>
        /// <returns></returns>
        public Boolean SelectBy_Entidade_ProtocoloExcedente(long alu_id, Guid ent_id, string protocolo)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_SelectBy_Entidade_ProtocoloExcedente", _Banco);
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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@protocoloExcedente";
                Param.Size = 20;
                Param.Value = protocolo;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                //Retorna o count da stored: true ou false
                return (Convert.ToInt32(qs.Return.Rows[0][0]) > 0);
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
        /// Realiza a busca dos alunos de acordo com os filtros informados, escolhendo o melhor caminho de acordo
        /// os filtros que forem passados ou não.
        /// Ordem de preferência:
        /// 1 - Número de matrícula, 2 - Nome do aluno, 3 - Todos os filtros
        /// </summary>
        /// <param name="uad_idSuperior">Id da unidade administrativa</param>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="uni_id">Id da unidade</param>
        /// <param name="pes_nome">Nome do aluno</param>
        /// <param name="tipoBusca">Tipo de busca do aluno. 1 - Contém, 2 - Começa por, 3 - Fonética</param>
        /// <param name="pes_dataNascimento">Data de nascimento do aluno</param>
        /// <param name="pes_nomeMae">Nome da mãe do aluno</param>
        /// <param name="alc_matricula">Número de matrícula</param>
        /// <param name="alc_matriculaEstadual">Número da matrícula estadual</param>
        /// <param name="alu_situacao">Situação do aluno</param>
        /// <param name="alu_dataCriacao">Data de criação do aluno</param>
        /// <param name="alu_dataAlteracao">Última data de criação do aluno</param>
        /// <param name="adm">Indica se visão do usuário é Administração</param>
        /// <param name="podeVisualizarTodos">Indica se usuário checou a opção para ver todos os alunos da rede</param>
        /// <param name="usu_id">Id do usuário</param>
        /// <param name="gru_id">Id do grupo do usuário</param>
        /// <param name="apenasDeficiente">Se busca apenas alunos com deficiencia</param>
        /// <param name="apenasGemeo">Indica se serão buscados apenas alunos com irmão gemeo</param>
        /// <param name="deficiencia">Deficiencia a procurar</param>
        /// <param name="ent_id">ID da entidade do usuário logado</param>
        /// <param name="buscaMatriculaIgual">Indica se vai buscar o número de matrícula exato</param>
        /// <param name="buscaPreferencial">Busca preferencial. 1 - Número de matrícula, 2 - Nome do aluno, 3 - Todos os filtros</param>
        /// <param name="totalRecords">Total de registros retornados</param>
        /// <returns></returns>
        public DataTable BuscaAlunos_PorFiltroPreferencial
        (
            Guid uad_idSuperior
            , int esc_id
            , int uni_id
            , string pes_nome
            , byte tipoBusca
            , DateTime pes_dataNascimento
            , string pes_nomeMae
            , string alc_matricula
            , string alc_matriculaEstadual
            , byte alu_situacao
            , DateTime alu_dataCriacao
            , DateTime alu_dataAlteracao
            , bool apenasDeficiente
            , Guid deficiencia
            , bool adm
            , bool podeVisualizarTodos
            , bool apenasGemeo
            , Guid usu_id
            , Guid gru_id
            , Guid ent_id
            , bool buscaMatriculaIgual
            , byte buscaPreferencial
            , bool retornaExcedentes
            , bool retornaPreMatricula
            , int LinhasPorPagina
            , int Pagina
            , int SortDirection
            , string SortExpression
            , out int totalRecords
            , bool documentoOficial
        )
        {
            #region Decide nome da procedure

            string nomeProcedure;

            switch (buscaPreferencial)
            {
                case 1:
                    {
                        // 1 - Número de matrícula.
                        nomeProcedure = "NEW_ACA_Aluno_BuscaPor_Matricula_Filtros";
                        break;
                    }
                case 2:
                    {
                        // 2 - Nome do aluno.
                        nomeProcedure = "NEW_ACA_Aluno_BuscaPor_Nome_Filtros";
                        break;
                    }
                default:
                    {
                        nomeProcedure = "NEW_ACA_Aluno_BuscaPor_Todos_Filtros";
                        break;
                    }
            }

            #endregion Decide nome da procedure

            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure(nomeProcedure, _Banco);

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
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@pes_nome";
            Param.Size = 200;
            Param.Value = pes_nome ?? "";
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipobuscaNome";
            Param.Size = 1;
            Param.Value = tipoBusca;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@pes_dataNascimento";
            Param.Size = 20;
            if (pes_dataNascimento != new DateTime())
                Param.Value = pes_dataNascimento;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@pes_nomeMae";
            Param.Size = 200;
            Param.Value = pes_nomeMae ?? "";
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@alc_matricula";
            Param.Size = 50;
            Param.Value = alc_matricula ?? "";
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@trazMatriculaIgual";
            Param.Size = 1;
            Param.Value = buscaMatriculaIgual;
            qs.Parameters.Add(Param);

            if (buscaPreferencial != 1)
            {
                // Se a busca for preferencial por alc_matricula, não adiciona o campo alc_matriculaEstadual.
                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alc_matriculaEstadual";
                Param.Size = 50;
                Param.Value = alc_matriculaEstadual ?? "";
                qs.Parameters.Add(Param);
            }

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@alu_situacao";
            Param.Size = 1;
            if (alu_situacao > 0)
                Param.Value = alu_situacao;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Date;
            Param.ParameterName = "@alu_dataCriacao";
            Param.Size = 20;
            if (alu_dataCriacao != new DateTime())
                Param.Value = alu_dataCriacao;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Date;
            Param.ParameterName = "@alu_dataAlteracao";
            Param.Size = 20;
            if (alu_dataAlteracao != new DateTime())
                Param.Value = alu_dataAlteracao;
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
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@apenasDeficiente";
            Param.Size = 1;
            if (apenasDeficiente)
                Param.Value = apenasDeficiente;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@deficiencia";
            Param.Size = 16;
            if (deficiencia.Equals(Guid.Empty))
                Param.Value = DBNull.Value;
            else
                Param.Value = deficiencia;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@podeVisualizarTodos";
            Param.Size = 1;
            Param.Value = podeVisualizarTodos;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@apenasGemeo";
            Param.Size = 1;
            if (apenasGemeo)
                Param.Value = apenasGemeo;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@retornaExcedentes";
            Param.Size = 1;
            if (retornaExcedentes)
                Param.Value = retornaExcedentes;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@retornaPreMatricula";
            Param.Size = 1;
            if (retornaPreMatricula)
                Param.Value = retornaPreMatricula;
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
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@gru_id";
            Param.Size = 16;
            Param.Value = gru_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@ent_id";
            Param.Size = 16;
            Param.Value = ent_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@LinhasPorPagina";
            Param.Size = 4;
            if (LinhasPorPagina > 0)
            {
                Param.Value = LinhasPorPagina;
            }
            else
            {
                Param.Value = DBNull.Value;
            }
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@Pagina";
            Param.Size = 4;
            if (LinhasPorPagina > 0)
            {
                Param.Value = Pagina;
            }
            else
            {
                Param.Value = DBNull.Value;
            }
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@SortDirection";
            Param.Size = 4;
            if (SortDirection >= 0)
            {
                Param.Value = SortDirection;
            }
            else
            {
                Param.Value = DBNull.Value;
            }
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@SortExpression";
            Param.Size = 300;
            if (!string.IsNullOrEmpty(SortExpression))
            {
                Param.Value = SortExpression;
            }
            else
            {
                Param.Value = DBNull.Value;
            }
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@documentoOficial";
            Param.Size = 1;
            Param.Value = documentoOficial;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            if (LinhasPorPagina > 0 && Pagina >= 0)
            {
                totalRecords = qs.Return.Rows.Count > 0 ?
                    Convert.ToInt32(qs.Return.Rows[0]["qtd_registros"]) :
                    0;
            }
            else
            {
                totalRecords = qs.Return.Rows.Count;
            }

            return qs.Return;
        }


        /// <summary>
        /// Retorna um datatable contendo todos os alunos
        /// que não foram excluídos logicamente, filtrados por
        /// Escola/UA, usuario, grupo, entidade, nome do aluno, matricula, situação
        /// </summary>
        public DataTable SelectBy_ParametrosAluno
        (
            Guid ent_id
            , Guid uad_idSuperior
            , Guid usu_id
            , Guid gru_id
            , int esc_id
            , int uni_id
            , string nome_aluno
            , byte tipoBusca
            , bool trazExcedentes
            , bool verificaPermissao
            , DateTime pes_dataNascimento
            , Guid tdo_idCPF
            , string tdo_nomeCPF
            , Guid tdo_idRG
            , string tdo_nomeRG
            , string pes_nomePai
            , string pes_nomeMae
            , string ctc_numeroTermo
            , string ctc_folha
            , string ctc_livro
            , DateTime ctc_dataEmissao
            , string alc_matricula
            , string alc_matriculaEstadual
            , byte alu_situacao
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_SelectBy_BuscaNome", _Banco);
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
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@nome_aluno";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(nome_aluno))
                    Param.Value = nome_aluno;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@alu_situacao";
                Param.Size = 1;
                if (alu_situacao > 0)
                    Param.Value = alu_situacao;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alc_matricula";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(alc_matricula))
                    Param.Value = alc_matricula;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alc_matriculaEstadual";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(alc_matriculaEstadual))
                    Param.Value = alc_matriculaEstadual;
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
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tipoBusca";
                Param.Size = 1;
                Param.Value = tipoBusca;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@trazExcedentes";
                Param.Size = 1;
                Param.Value = trazExcedentes;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@verificaPermissao";
                Param.Size = 1;
                Param.Value = verificaPermissao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@pes_dataNascimento";
                Param.Size = 20;
                if (pes_dataNascimento != new DateTime())
                    Param.Value = pes_dataNascimento;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@tdo_idCPF";
                Param.Size = 16;
                if (tdo_idCPF != Guid.Empty)
                    Param.Value = tdo_idCPF;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tdo_nomeCPF";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(tdo_nomeCPF))
                    Param.Value = tdo_nomeCPF;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@tdo_idRG";
                Param.Size = 16;
                if (tdo_idRG != Guid.Empty)
                    Param.Value = tdo_idRG;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tdo_nomeRG";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(tdo_nomeRG))
                    Param.Value = tdo_nomeRG;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pes_nomePai";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(pes_nomePai))
                    Param.Value = pes_nomePai;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pes_nomeMae";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(pes_nomeMae))
                    Param.Value = pes_nomeMae;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@ctc_numeroTermo";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(ctc_numeroTermo))
                    Param.Value = ctc_numeroTermo;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@ctc_folha";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(ctc_folha))
                    Param.Value = ctc_folha;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@ctc_livro";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(ctc_livro))
                    Param.Value = ctc_livro;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@ctc_dataEmissao";
                Param.Size = 20;
                if (ctc_dataEmissao != new DateTime())
                    Param.Value = ctc_dataEmissao;
                else
                    Param.Value = DBNull.Value;
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
        /// Retorna um datatable contendo todos os alunos que não foram excluídos logicamente,
        /// filtrados principalmente pelo docente e pelo tipo de busca por nome do aluno: 1 - Contém / 2- Começa por
        /// </summary>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="doc_id"></param>
        /// <param name="tipo_busca">Tipo de busca</param>
        /// <param name="pes_nome">Nome</param>
        /// <param name="pes_nomeMae">Nome da Mã</param>
        /// <param name="pes_dataNascimento">Data de nascimento</param>
        /// <param name="alc_matricula">numero de matricula</param>
        /// <param name="alc_matriculaEstadual">numero de matricula estadual</param>
        /// <param name="alu_dataCriacao">data de criacao</param>
        /// <param name="alu_dataAlteracao">data de alteracao</param>
        /// <param name="apenasDeficiente">apenas deficiente</param>
        /// <param name="apenasGemeo">apenas alunos que possuam irmao(s) gemeo(s)</param>
        /// <param name="deficiencia">deficiencia</param>
        /// <param name="totalRecords">total de registros</param>
        /// <returns>DataTable com os alunos</returns>
        public DataTable SelectBy_Pesquisa_Docente
        (
            Guid ent_id
            , long doc_id
            , byte tipo_busca
            , string pes_nome
            , string pes_nomeMae
            , DateTime pes_dataNascimento
            , string alc_matricula
            , string alc_matriculaEstadual
            , DateTime alu_dataCriacao
            , DateTime alu_dataAlteracao
            , bool apenasDeficiente
            , bool apenasGemeo
            , Guid deficiencia
            , int LinhasPorPagina
            , int Pagina
            , int SortDirection
            , string SortExpression
            , out int totalRecords
            , bool documentoOficial
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_BuscaPor_Filtros_Docente", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@ent_id";
            Param.Size = 16;
            Param.Value = ent_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@doc_id";
            Param.Size = 8;
            Param.Value = doc_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tipobusca";
            Param.Size = 1;
            Param.Value = tipo_busca;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@pes_nome";
            Param.Size = 200;
            Param.Value = pes_nome ?? "";
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@pes_nomeMae";
            Param.Size = 200;
            Param.Value = pes_nomeMae ?? "";
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@pes_dataNascimento";
            Param.Size = 20;
            if (pes_dataNascimento != new DateTime())
                Param.Value = pes_dataNascimento;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@alu_dataCriacao";
            Param.Size = 20;
            if (alu_dataCriacao != new DateTime())
                Param.Value = alu_dataCriacao;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@alu_dataAlteracao";
            Param.Size = 20;
            if (alu_dataAlteracao != new DateTime())
                Param.Value = alu_dataAlteracao;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@alc_matricula";
            Param.Size = 50;
            Param.Value = alc_matricula ?? "";
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@alc_matriculaEstadual";
            Param.Size = 50;
            Param.Value = alc_matriculaEstadual ?? "";
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@apenasDeficiente";
            Param.Size = 1;
            if (apenasDeficiente)
                Param.Value = apenasDeficiente;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@apenasGemeo";
            Param.Size = 1;
            if (apenasGemeo)
                Param.Value = apenasGemeo;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@deficiencia";
            Param.Size = 16;
            if (deficiencia.Equals(Guid.Empty))
                Param.Value = DBNull.Value;
            else
                Param.Value = deficiencia;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@LinhasPorPagina";
            Param.Size = 4;
            if (LinhasPorPagina > 0)
            {
                Param.Value = LinhasPorPagina;
            }
            else
            {
                Param.Value = DBNull.Value;
            }
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@Pagina";
            Param.Size = 4;
            if (LinhasPorPagina > 0)
            {
                Param.Value = Pagina;
            }
            else
            {
                Param.Value = DBNull.Value;
            }
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@SortDirection";
            Param.Size = 4;
            if (SortDirection >= 0)
            {
                Param.Value = SortDirection;
            }
            else
            {
                Param.Value = DBNull.Value;
            }
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@SortExpression";
            Param.Size = 300;
            if (!string.IsNullOrEmpty(SortExpression))
            {
                Param.Value = SortExpression;
            }
            else
            {
                Param.Value = DBNull.Value;
            }
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@documentoOficial";
            Param.Size = 1;
            Param.Value = documentoOficial;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();
            if (LinhasPorPagina > 0 && Pagina >= 0)
            {
                totalRecords = qs.Return.Rows.Count > 0 ?
                    Convert.ToInt32(qs.Return.Rows[0]["qtd_registros"]) :
                    0;
            }
            else
            {
                totalRecords = qs.Return.Rows.Count;
            }

            return qs.Return;
        }

        /// <summary>
        /// Utilizado na tela de Transferência.
        /// </summary>
        /// <param name="uad_idSuperior"></param>
        /// <param name="esc_id">id da escola</param>
        /// <param name="uni_id">id da unidade administrativa</param>
        /// <param name="cur_id">id do curso</param>
        /// <param name="tur_codigo">código da turma</param>
        /// <param name="tipoBusca"></param>
        /// <param name="nome_aluno">nome do aluno</param>
        /// <param name="ent_id">id entidade</param>
        /// <param name="paginado"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns>
        /// Retorna um datatable contendo todos os alunos com a situação "Ativo",
        /// que possuem matrícula (AlunoCurriculo) também com a situação "Ativo".
        /// </returns>
        public DataTable SelectBy_Transferencia
        (
            Guid uad_idSuperior
            , int esc_id
            , int uni_id
            , int cur_id
            , string tur_codigo
            , byte tipoBusca
            , string nome_aluno
            , Guid ent_id
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_SelectBy_Transferencia", _Banco);
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
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tur_codigo";
                Param.Size = 30;
                if (!String.IsNullOrEmpty(tur_codigo))
                    Param.Value = tur_codigo;
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
                Param.Size = 30;
                if (!String.IsNullOrEmpty(nome_aluno))
                    Param.Value = nome_aluno;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

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
        /// Verifica se já existe um aluno cadastrado com as mesma informações
        /// de acordo com os parametros de busca de aluno e sua integridade
        /// </summary>
        /// <returns>DataTable com os alunos</returns>
        public DataTable SelectBy_VerificaUnicidadeAluno
        (
            Guid ent_id
            , string pes_nome
            , DateTime pes_dataNascimento
            , string pes_nomeMae
            , long alu_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_SelectBy_VerificaUnicidade", _Banco);
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
                Param.DbType = DbType.String;
                Param.ParameterName = "@pes_nome";
                Param.Value = pes_nome;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Date;
                Param.ParameterName = "@pes_dataNascimento";
                Param.Size = 20;
                Param.Value = pes_dataNascimento;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@pes_nomeMae";
                Param.Value = pes_nomeMae;
                qs.Parameters.Add(Param);

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
        /// Verifica se já existe um aluno cadastrado com o mesmo nome e data de nascimento
        /// </summary>
        public DataTable SelectBy_VerificaIntegridadeNomeDataNasc
        (
            string pes_nome
            , DateTime pes_dataNascimento
            , long alu_id
            , Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_SelectBy_VerificaUnicidadeNomeDataNasc", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pes_nome";
                Param.Size = 200;
                Param.Value = pes_nome;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@pes_dataNascimento";
                Param.Size = 20;
                Param.Value = pes_dataNascimento;
                qs.Parameters.Add(Param);

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
        /// Verifica se já existe um aluno cadastrado com o mesmo nome, data de nascimento
        /// e nome da mãe, pesquisando os nomes pelo som das palavras.
        /// </summary>
        public DataTable SelectBy_VerificaUnicidadeSom
        (
            string pes_nome
            , DateTime pes_dataNascimento
            , string pes_nomeMae
            , long alu_id
            , Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_SelectBy_VerificaUnicidadeFonetica", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pes_nome";
                Param.Size = 200;
                Param.Value = pes_nome;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@pes_dataNascimento";
                Param.Size = 20;
                Param.Value = pes_dataNascimento;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pes_nomeMae";
                Param.Size = 200;
                Param.Value = pes_nomeMae;
                qs.Parameters.Add(Param);

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
        /// Retorna um datatable contendo todas as pessoas no sistema,
        /// com uma flag dizendo se os alunos da entidade serão incluídos ou não na busca.
        /// </summary>
        /// <param name="ent_id">Id da entidade</param>
        /// <param name="nome">nome da pessoa</param>
        /// <param name="cpf">cpf da pessoa</param>
        /// <param name="rg">rg da pessoa</param>
        /// <param name="nis"></param>
        /// <param name="consultaAlunos">Indica se irá incluir alunos na busca</param>
        /// <param name="tdo_idnis"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <param name="tdo_idcpf"></param>
        /// <param name="tdo_idrg"></param>
        public DataTable SelectBuscaPessoas
        (
          Guid ent_id
            , string nome
            , string cpf
            , string rg
            , string nis
            , bool consultaAlunos
            , Guid tdo_idcpf
            , Guid tdo_idrg
            , Guid tdo_idnis
            , int currentPage
            , int pageSize
            , out int totalRecords

        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_BuscaPessoas", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@consultaAlunos";
                Param.Value = consultaAlunos;
                Param.Size = 1;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@pes_nome";
                if (!string.IsNullOrEmpty(nome))
                    Param.Value = nome;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@TIPO_DOCUMENTACAO_RG";
                Param.Size = 50;
                if (!String.IsNullOrEmpty(rg))
                    Param.Value = rg;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@TIPO_DOCUMENTACAO_CPF";
                Param.Size = 50;
                if (!String.IsNullOrEmpty(cpf))
                    Param.Value = cpf;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@TIPO_DOCUMENTACAO_NIS";
                Param.Size = 50;
                if (!String.IsNullOrEmpty(nis))
                    Param.Value = nis;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@tdo_idcpf";
                Param.Size = 16;
                if (tdo_idcpf != Guid.Empty)
                    Param.Value = tdo_idcpf;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@tdo_idrg";
                Param.Size = 16;
                if (tdo_idrg != Guid.Empty)
                    Param.Value = tdo_idrg;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@tdo_idnis";
                Param.Size = 16;
                if (tdo_idnis != Guid.Empty)
                    Param.Value = tdo_idnis;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

                totalRecords = qs.Execute(currentPage / pageSize, pageSize);

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna um datatable contendo todos os dados para declaração de
        /// um ou mais alunos que não foram excluidos logicamente, filtrados por
        /// id do aluno e tipo de documento de RG padrão do sistema
        /// </summary>
        /// <param name="alu_ids"> string que contem os ids dos alunos</param>
        /// <param name="tipo_doc_rg"> Valor de identificação única do tipo de documento de rg padrão</param>
        /// <returns>DataTable com os alunos</returns>
        public DataTable SelectBy_DadosDeclaracaoporAluno
        (
            string alu_ids
            , Guid tipo_doc_rg
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_SelectBy_DadosDeclaracaoporAluno", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alu_ids";
                Param.Value = alu_ids;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@tipo_doc_rg";
                Param.Size = 16;
                if (!Guid.Empty.Equals(tipo_doc_rg))
                    Param.Value = tipo_doc_rg;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

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
        /// Seleciona os alunos que possuem dados a corrigir devido a migração,
        /// filtrando através da unidade adm, escola, curso e período.
        /// </summary>
        /// <param name="uad_id">Id da unidade adm.</param>
        /// <param name="esc_id">Id da escola.</param>
        /// <param name="uni_id">Id da unidade.</param>
        /// <param name="cur_id">Id do curso.</param>
        /// <param name="crr_id">Id do currículo.</param>
        /// <param name="crp_id">Id do período.</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <param name="usu_id">Id do usuário.</param>
        /// <param name="gru_id">Id do grupo do usuário.</param>
        /// <param name="nomeAluno">Nome do aluno.</param>
        /// <param name="paginado"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <param name="adm">Indica se é adm (1) ou não (0).</param>
        public DataTable SelectBy_PesquisaDadosCorrecao
        (
            Guid uad_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , Guid usu_id
            , Guid gru_id
            , int adm
            , string nomeAluno
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_SelectBy_PesquisaAlunosCorrecao", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                if (uad_id == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = uad_id;
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
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                if (esc_id > 0)
                    Param.Value = esc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@adm";
                Param.Size = 1;
                Param.Value = adm;
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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                if (crp_id > 0)
                    Param.Value = crp_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@nomeAluno";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(nomeAluno))
                    Param.Value = nomeAluno;
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

                #endregion PARAMETROS

                if (paginado)
                    totalRecords = qs.Execute(currentPage, pageSize);
                else
                {
                    qs.Execute();
                    totalRecords = qs.Return.Rows.Count;
                }

                return qs.Return;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Seleciona as escolas que possuem alunos com dados a corrigir devido a migração,
        /// filtrando através da unidade adm, escola, curso e período.
        /// </summary>
        /// <param name="uad_id">Id da unidade adm.</param>
        /// <param name="esc_id">Id da escola.</param>
        /// <param name="uni_id">Id da unidade.</param>
        /// <param name="cur_id">Id do curso.</param>
        /// <param name="crr_id">Id do currículo.</param>
        /// <param name="crp_id">Id do período.</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <param name="usu_id">Id do usuário.</param>
        /// <param name="gru_id">Id do grupo do usuário.</param>
        /// <param name="adm"></param>
        /// <param name="paginado"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        public DataTable SelectBy_PesquisaDadosCorrecaoPorEscola
        (
            Guid uad_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , Guid usu_id
            , Guid gru_id
            , int adm
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_SelectBy_PesquisaCorrecaoPorEscola", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                if (uad_id == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = uad_id;
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
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                if (esc_id > 0)
                    Param.Value = esc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@adm";
                Param.Size = 1;
                Param.Value = adm;
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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                if (crp_id > 0)
                    Param.Value = crp_id;
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

                #endregion PARAMETROS

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
            catch (Exception)
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Seleciona a quantidade de alunos que possuem dados a corrigir devido a migração,
        /// filtrando através da unidade adm, escola, curso e período.
        /// </summary>
        /// <param name="tua_id"></param>
        /// <param name="uad_id">Id da unidade adm.</param>
        /// <param name="esc_id">Id da escola.</param>
        /// <param name="uni_id">Id da unidade.</param>
        /// <param name="cur_id">Id do curso.</param>
        /// <param name="crr_id">Id do currículo.</param>
        /// <param name="crp_id">Id do período.</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <param name="usu_id">Id do usuário.</param>
        /// <param name="gru_id">Id do grupo do usuário.</param>
        /// <param name="paginado"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <param name="adm"></param>
        public DataTable SelectBy_PesquisaDadosCorrecaoPorUA
        (
            Guid tua_id
            , Guid uad_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , Guid usu_id
            , Guid gru_id
            , int adm
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_SelectBy_PesquisaCorrecaoPorUA", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@tua_id";
                Param.Size = 16;
                if (tua_id == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = tua_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                if (uad_id == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = uad_id;
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
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                if (esc_id > 0)
                    Param.Value = esc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@adm";
                Param.Size = 1;
                Param.Value = adm;
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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                if (crp_id > 0)
                    Param.Value = crp_id;
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

                #endregion PARAMETROS

                if (paginado)
                    totalRecords = qs.Execute(currentPage, pageSize);
                else
                {
                    qs.Execute();
                    totalRecords = qs.Return.Rows.Count;
                }

                return qs.Return;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Seleciona os alunos que possuem dados a corrigir devido a migração,
        /// filtrando através da unidade adm, escola, curso e período.
        /// </summary>
        /// <param name="uad_id">Id da unidade adm.</param>
        /// <param name="esc_id">Id da escola.</param>
        /// <param name="uni_id">Id da unidade.</param>
        /// <param name="cur_id">Id do curso.</param>
        /// <param name="crr_id">Id do currículo.</param>
        /// <param name="crp_id">Id do período.</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <param name="usu_id">Id do usuário.</param>
        /// <param name="gru_id">Id do grupo do usuário.</param>
        /// <param name="adm">É usuário do tipo administrador</param>
        /// <param name="NivelSME">Visualiza duplicações cujo responsável é do nível SME</param>
        /// <param name="NivelCRE">Visualiza duplicações cujo responsável é do nível CRE</param>
        /// <param name="NivelEscola">Visualiza duplicações cujo responsável é do nível Escola</param>
        /// <param name="paginado"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        public DataTable SelectBy_PesquisaDadosDuplicados
        (
            Guid uad_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , Guid usu_id
            , Guid gru_id
            , int adm
            , bool NivelSME
            , bool NivelCRE
            , bool NivelEscola
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_SelectBy_PesquisaAlunosDuplicados", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                if (uad_id == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = uad_id;
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
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                if (esc_id > 0)
                    Param.Value = esc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@adm";
                Param.Size = 1;
                Param.Value = adm;
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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                if (crp_id > 0)
                    Param.Value = crp_id;
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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@NivelSME";
                Param.Size = 1;
                Param.Value = NivelSME;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@NivelCRE";
                Param.Size = 1;
                Param.Value = NivelCRE;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@NivelEscola";
                Param.Size = 1;
                Param.Value = NivelEscola;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                if (paginado)
                    totalRecords = qs.Execute(currentPage, pageSize);
                else
                {
                    qs.Execute();
                    totalRecords = qs.Return.Rows.Count;
                }

                return qs.Return;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Seleciona as escolas que possuem alunos com dados a corrigir devido a migração,
        /// filtrando através da unidade adm, escola, curso e período.
        /// </summary>
        /// <param name="uad_id">Id da unidade adm.</param>
        /// <param name="esc_id">Id da escola.</param>
        /// <param name="uni_id">Id da unidade.</param>
        /// <param name="cur_id">Id do curso.</param>
        /// <param name="crr_id">Id do currículo.</param>
        /// <param name="crp_id">Id do período.</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <param name="usu_id">Id do usuário.</param>
        /// <param name="gru_id">Id do grupo do usuário.</param>
        /// <param name="adm">É usuário do tipo administrador</param>
        /// <param name="NivelSME">Visualiza duplicações cujo responsável é do nível SME</param>
        /// <param name="NivelCRE">Visualiza duplicações cujo responsável é do nível CRE</param>
        /// <param name="NivelEscola">Visualiza duplicações cujo responsável é do nível Escola</param>
        /// <param name="paginado"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        public DataTable SelectBy_PesquisaDadosDuplicadosPorEscola
        (
            Guid uad_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , Guid usu_id
            , Guid gru_id
            , int adm
            , bool NivelSME
            , bool NivelCRE
            , bool NivelEscola
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_SelectBy_PesquisaDuplicadosPorEscola", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                if (uad_id == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = uad_id;
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
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                if (esc_id > 0)
                    Param.Value = esc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@adm";
                Param.Size = 1;
                Param.Value = adm;
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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                if (crp_id > 0)
                    Param.Value = crp_id;
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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@NivelSME";
                Param.Size = 1;
                Param.Value = NivelSME;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@NivelCRE";
                Param.Size = 1;
                Param.Value = NivelCRE;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@NivelEscola";
                Param.Size = 1;
                Param.Value = NivelEscola;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

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
            catch (Exception)
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Seleciona a quantidade de alunos que possuem dados a corrigir devido a migração,
        /// filtrando através da unidade adm, escola, curso e período.
        /// </summary>
        /// <param name="tua_id"></param>
        /// <param name="uad_id">Id da unidade adm.</param>
        /// <param name="esc_id">Id da escola.</param>
        /// <param name="uni_id">Id da unidade.</param>
        /// <param name="cur_id">Id do curso.</param>
        /// <param name="crr_id">Id do currículo.</param>
        /// <param name="crp_id">Id do período.</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <param name="usu_id">Id do usuário.</param>
        /// <param name="gru_id">Id do grupo do usuário.</param>
        /// <param name="adm">É usuário do tipo administrador</param>
        /// <param name="NivelSME">Visualiza duplicações cujo responsável é do nível SME</param>
        /// <param name="NivelCRE">Visualiza duplicações cujo responsável é do nível CRE</param>
        /// <param name="NivelEscola">Visualiza duplicações cujo responsável é do nível Escola</param>
        /// <param name="paginado"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        public DataTable SelectBy_PesquisaDadosDuplicadosPorUA
        (
            Guid tua_id
            , Guid uad_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , Guid usu_id
            , Guid gru_id
            , int adm
            , bool NivelSME
            , bool NivelCRE
            , bool NivelEscola
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_SelectBy_PesquisaDuplicadosPorUA", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@tua_id";
                Param.Size = 16;
                if (tua_id == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = tua_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                if (uad_id == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = uad_id;
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
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                if (esc_id > 0)
                    Param.Value = esc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@adm";
                Param.Size = 1;
                Param.Value = adm;
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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                if (crp_id > 0)
                    Param.Value = crp_id;
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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@NivelSME";
                Param.Size = 1;
                Param.Value = NivelSME;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@NivelCRE";
                Param.Size = 1;
                Param.Value = NivelCRE;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@NivelEscola";
                Param.Size = 1;
                Param.Value = NivelEscola;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                if (paginado)
                    totalRecords = qs.Execute(currentPage, pageSize);
                else
                {
                    qs.Execute();
                    totalRecords = qs.Return.Rows.Count;
                }

                return qs.Return;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Se encotrar a pessoa (pes_id) no cadastro de aluno
        /// retorna o codigo do aluno (alu_id) referente à essa
        /// pessoa
        /// </summary>
        /// <param name="pes_id">Id da pessoa pesquisada</param>
        /// <returns>Int64 - Id do aluno ou 0 se nao achar</returns>
        public long SelectAlunoby_pes_id(Guid pes_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_SelectBy_pes_id", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@pes_id";
                Param.Size = 16;
                Param.Value = pes_id;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                if (qs.Return.Rows.Count == 0)
                    return 0;

                long alu_id;
                Int64.TryParse(qs.Return.Rows[0]["alu_id"].ToString(), out alu_id);

                return alu_id;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        public DataTable SelecionarSexoDataNascPorAluno(Int64 alu_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_SelecionarSexoDataNascPorAluno", _Banco);

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
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }
                

        /// <summary>
        /// Seleciona alunos ativos que possuam NIS.
        /// </summary>
        /// <param name="tdo_idNIS">Tipo de documento NIS</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <returns></returns>
        public DataTable SelecionaPorNIS(Guid tdo_idNIS, Guid ent_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_SelecionaPorNIS", _Banco);

            //Sem limite de timeout
            qs.TimeOut = 0;

            try
            {
                #region Parametros

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@tdo_id";
                Param.Size = 16;
                Param.Value = tdo_idNIS;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                #endregion Parametros

                qs.Execute();

                return qs.Return;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Verifica se o aluno possui matrícula filtrado
        /// por processo e número de matrícula.
        /// </summary>
        /// <param name="alc_matricula">String com as matrículas dos alunos para pesquisa.</param>        
        /// <param name="pfi_id"></param>
        /// <returns></returns>
        public DataTable VerificaAlunoExistentePorMatriculaProcesso(string alc_matricula, int pfi_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_SelecionaAlunoExistentePorMatriculaProcesso", _Banco);

            try
            {
                #region Parametros

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alc_matricula";
                Param.Value = alc_matricula;
                Param.Size = 50;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@pfi_id";
                Param.Size = 4;
                Param.Value = pfi_id;
                qs.Parameters.Add(Param);

                #endregion Parametros

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
        /// O método seleciona os alunos com as matrículas presentes na string alc_matricula.
        /// </summary>
        /// <param name="alc_matricula">String com as matrículas dos alunos para pesquisa.</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <returns></returns>
        public DataTable SelecionaPorNumeroMatricula(string alc_matricula, Guid ent_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_SelecionaPorNumeroMatricula", _Banco);

            try
            {
                #region Parametros

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alc_matricula";
                Param.Value = alc_matricula;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                #endregion Parametros

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
        /// Verifica se a última movimentação é  duplicidade por matrícula ou exclusão por erro da escola
        /// </summary>
        /// <param name="alc_matricula">Matricula do aluno.</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <returns></returns>
        public bool SelecionaPorNumeroMatriculaMovimentacaoDuplicidade(string alc_matricula, Guid ent_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_SelecionaPorNumeroMatriculaMovimentacaoDuplicidade", _Banco);

            try
            {
                #region Parametros

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alc_matricula";
                Param.Value = alc_matricula;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                #endregion Parametros

                qs.Execute();

                return qs.Return.Rows.Count > 0;
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
        /// Seleciona os alunos segundo a cidade de seu primeiro endereço e o ano da turma de sua última movimentação.
        /// </summary>
        /// <param name="cid_id">ID da cidade.</param>
        /// <param name="ano">Ano da turma.</param>
        /// <param name="ent_id">Entidade do usuário logado.</param>
        /// <returns></returns>
        public DataTable SelecionaAlunosPorCidadeMovimentacaoAno(Guid cid_id, int ano, Guid ent_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_SelecionaAlunosPorCidadeMovimentacaoAno", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.Size = 16;
                Param.ParameterName = "@cid_id";
                Param.Value = cid_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.ParameterName = "@ano";
                Param.Value = ano;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.Size = 16;
                Param.ParameterName = "@ent_id";
                Param.Value = ent_id;
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

        /// <summary>
        /// Verifica se já existe o aluno matriculado para a escola no ano de início do processo de fechamento/início do ano letivo.
        /// </summary>
        /// <param name="esc_id">ID da escola.</param>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo do curso.</param>
        /// <param name="crp_id">ID do período do curso.</param>
        /// <param name="pfi_id">ID do processo de fechamento/início do ano letivo.</param>
        /// <param name="alc_matricula">Número de matrícula do aluno.</param>
        /// <param name="pes_nome">Nome do aluno.</param>
        /// <param name="pes_nomeMae">Nome da mãe do aluno.</param>
        /// <param name="pes_dataNascimento">Data de nascimento do aluno.</param>
        /// <param name="ent_id">ID da entidade do usuário logado.</param>
        /// <returns></returns>
        public bool VerificaAlunoMatriculadoProcessoEscolaCurso
        (
            int esc_id,
            int cur_id,
            int crr_id,
            int crp_id,
            int pfi_id,
            string alc_matricula,
            string pes_nome,
            string pes_nomeMae,
            DateTime pes_dataNascimento,
            Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_VerificaAlunoMatriculadoProcessoEscolaCurso", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                Param.Value = esc_id;
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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@pfi_id";
                Param.Size = 4;
                Param.Value = pfi_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alc_matricula";
                Param.Size = 50;
                Param.Value = alc_matricula;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pes_nome";
                Param.Size = 200;
                Param.Value = pes_nome;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pes_nomeMae";
                Param.Size = 200;
                Param.Value = pes_nomeMae;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Date;
                Param.ParameterName = "@pes_dataNascimento";
                Param.Size = 16;
                Param.Value = pes_dataNascimento.Date;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
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
        /// Retorna os alunos matriculados em turmas multisseriada do docente
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="cal_id">ID do calendário escolar</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="adm">Indica se o usuário é administrador</param>
        /// <param name="usu_id">ID do usuário logado</param>
        /// <param name="gru_id">ID do grupo do usuário logado</param>
        public DataTable SelectBy_MatriculaMultisseriadaDocente_TurmaDisciplinaMultisseriada
        (
            int esc_id
            , int uni_id
            , Int64 tur_id
            , int cal_id
            , int tds_id
            , Guid ent_id
            , bool adm
            , Guid usu_id
            , Guid gru_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_SelectBy_MatriculaMultisseriadaDocente_TurmaDisciplinaMultisseriada", _Banco);
            try
            {
                #region PARAMETROS

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
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                Param.Value = cal_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tds_id";
                Param.Size = 4;
                Param.Value = tds_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
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
                Param.Value = usu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@gru_id";
                Param.Size = 16;
                Param.Value = gru_id;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                return qs.Return;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }


        /// <summary>
        /// Retorna os alunos matriculados em turmas com o mesmo curso e curriculo para disciplina educação fisica 
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="cal_id">ID do calendário escolar</param>
        /// <param name="tds_id">ID do tipo de disciplina</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="adm">Indica se o usuário é administrador</param>
        /// <param name="usu_id">ID do usuário logado</param>
        /// <param name="gru_id">ID do grupo do usuário logado</param>
        public DataTable SelectBy_CursoPeriodoTurmaMultisseriadaDocente
        (
            int esc_id
            , int uni_id
            , Int64 tur_id
            , int cal_id
            , int tds_id
            , Guid ent_id
            , bool adm
            , Guid usu_id
            , Guid gru_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_SelectBy_TurmaMultisseriadaDocente", _Banco);
            try
            {
                #region PARAMETROS

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
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tds_id";
                Param.Size = 4;
                Param.Value = tds_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                Param.Value = cal_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
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
                Param.Value = usu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@gru_id";
                Param.Size = 16;
                Param.Value = gru_id;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Seleciona os dados da pessoa e da matrícula do aluno por id do aluno ou escola.
        /// </summary>
        /// <param name="alu_ids">IDs do aluno separados por ;</param>
        /// <param name="dataBase">data base para seleção dos registros</param>
        /// <param name="esc_id">codigo da escola</param>
        /// <returns></returns>
        public DataTable SelecionaDadosAlunoMatricula(string alu_ids, int esc_id, DateTime dataBase)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_SelecionaDadosAlunoMatricula", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@alu_ids";

                if (alu_ids == null)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = alu_ids;

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
                Param.DbType = DbType.Date;
                Param.ParameterName = "@dataBase";
                Param.Size = 3;
                if (dataBase != new DateTime())
                    Param.Value = dataBase;
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
        /// retorna registros de usuário dos alunos pelos ids concatenados
        /// </summary>
        /// <param name="alu_ids">IDs do aluno separados por ;</param>
        /// <param name="trazerFoto">informa se irá trazer as fotos</param>
        /// <returns></returns>
        public DataTable SelecionaDadosFotoAlunos(string alu_ids)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_SYS_Usuario_SelecionaPorAlunos", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@alu_ids";

                if (alu_ids == null)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = alu_ids;

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
        /// Seleciona os dados do aluno da tela de VisualizaConteudo
        /// </summary>
        /// <param name="parametro">Parâmetro: Nome do aluno OU matrícula</param>
        /// <returns>Retorna dados do aluno</returns>
        public DataSet SelecionaVisualizaConteudo(string parametro)
        {
            //Grava em DataSet pois retorna vários selects
            DataSet dsRetorno = new DataSet();

            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlConnection con = new SqlConnection(_Banco.GetConnection.ConnectionString);

            adapter.SelectCommand = new SqlCommand("NEW_ACA_Aluno_VisualizaConteudo", con);
            adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

            adapter.SelectCommand.Parameters.Add("parametro", SqlDbType.VarChar, 200);
            adapter.SelectCommand.Parameters["parametro"].Value = parametro;

            adapter.Fill(dsRetorno);

            return dsRetorno;
        }

        /// <summary>
        /// Retorna os alunos matriculados que estão vinculados a uma turma.
        /// Para efetuar matricula nas turmas selecionada
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="crp_id">ID currículo período</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="adm">Indica se o usuário é administrador</param>
        /// <param name="usu_id">ID do usuário logado</param>
        /// <param name="gru_id">ID do grupo do usuário logado</param>
        public DataTable SelectBy_Alunos_MatriculadosSemTurma
        (
            int pfi_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , bool adm
            , Guid usu_id
            , Guid gru_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_SelectBy_MTR_Matricula_SemTurma", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@pfi_id";
                Param.Size = 4;
                Param.Value = pfi_id;
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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
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
                Param.Value = usu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@gru_id";
                Param.Size = 16;
                Param.Value = gru_id;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                return qs.Return;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Utilizado na consulta de alunos para o lançamento de justificativa de falta.
        /// </summary>
        public DataTable SelectBy_JustificativaAbonoFalta
        (
            int cal_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Int64 tur_id
            , byte tipoBusca
            , string pes_nome
            , DateTime pes_dataNascimento
            , string pes_nomeMae
            , string alc_matricula
            , string alc_matriculaEstadual
            , Guid ent_id
            , Guid uad_idSuperior
            , bool adm
            , Guid usu_id
            , Guid gru_id
            , bool buscaMatriculaIgual
            , bool MostraCodigoEscola
            , int LinhasPorPagina
            , int Pagina
            , int SortDirection
            , string SortExpression
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_BuscaJustificativaAbonoFalta", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                if (uad_idSuperior == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = uad_idSuperior;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@cal_id";
                if (cal_id > 0)
                    Param.Value = cal_id;
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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                if (crp_id > 0)
                    Param.Value = crp_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tipobusca";
                Param.Size = 1;
                Param.Value = tipoBusca;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pes_nome";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(pes_nome))
                    Param.Value = pes_nome;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Date;
                Param.ParameterName = "@pes_dataNascimento";
                Param.Size = 3;
                if (pes_dataNascimento != new DateTime())
                    Param.Value = pes_dataNascimento;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alc_matricula";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(alc_matricula))
                    Param.Value = alc_matricula;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alc_matriculaEstadual";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(alc_matriculaEstadual))
                    Param.Value = alc_matriculaEstadual;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pes_nomeMae";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(pes_nomeMae))
                    Param.Value = pes_nomeMae;
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
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@trazMatriculaIgual";
                Param.Size = 1;
                Param.Value = buscaMatriculaIgual;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@MostraCodigoEscola";
                Param.Size = 1;
                Param.Value = MostraCodigoEscola;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@LinhasPorPagina";
                Param.Size = 4;
                if (LinhasPorPagina > 0)
                {
                    Param.Value = LinhasPorPagina;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@Pagina";
                Param.Size = 4;
                if (LinhasPorPagina > 0)
                {
                    Param.Value = Pagina;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@SortDirection";
                Param.Size = 4;
                if (SortDirection >= 0)
                {
                    Param.Value = SortDirection;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@SortExpression";
                Param.Size = 300;
                if (!string.IsNullOrEmpty(SortExpression))
                {
                    Param.Value = SortExpression;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                if (LinhasPorPagina > 0 && Pagina >= 0)
                {
                    totalRecords = qs.Return.Rows.Count > 0 ?
                        Convert.ToInt32(qs.Return.Rows[0]["qtd_registros"]) :
                        0;
                }
                else
                {
                    totalRecords = qs.Return.Rows.Count;
                }

                return qs.Return;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Seleciona os dados de pessoa do aluno
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <returns></returns>
        public DataTable GetDadosAluno(long alu_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_SelecionaDados", _Banco);

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
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion Métodos

        #region Métodos de consulta - Documentos do Aluno

        /// <summary>
        /// Utilizado na consulta de documentos do aluno
        /// </summary>
        public DataTable SelectBy_PesquisaDocumentoAluno_BoletimEscolar
        (
            int cal_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Int64 tur_id
            , byte tipoBusca
            , string pes_nome
            , DateTime pes_dataNascimento
            , string pes_nomeMae
            , string alc_matricula
            , string alc_matriculaEstadual
            , Guid ent_id
            , Guid uad_idSuperior
            , bool adm
            , Guid usu_id
            , Guid gru_id
            , bool buscaMatriculaIgual
            , bool MostraCodigoEscola
            , int LinhasPorPagina
            , int Pagina
            , int SortDirection
            , string SortExpression
            , out int totalRecords
       )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_BuscaDocumentos_BoletimEscolar", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                if (uad_idSuperior == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = uad_idSuperior;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@cal_id";
                if (cal_id > 0)
                    Param.Value = cal_id;
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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                if (crp_id > 0)
                    Param.Value = crp_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tipobusca";
                Param.Size = 1;
                Param.Value = tipoBusca;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pes_nome";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(pes_nome))
                    Param.Value = pes_nome;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Date;
                Param.ParameterName = "@pes_dataNascimento";
                Param.Size = 3;
                if (pes_dataNascimento != new DateTime())
                    Param.Value = pes_dataNascimento;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alc_matricula";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(alc_matricula))
                    Param.Value = alc_matricula;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alc_matriculaEstadual";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(alc_matriculaEstadual))
                    Param.Value = alc_matriculaEstadual;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pes_nomeMae";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(pes_nomeMae))
                    Param.Value = pes_nomeMae;
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
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@trazMatriculaIgual";
                Param.Size = 1;
                Param.Value = buscaMatriculaIgual;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@MostraCodigoEscola";
                Param.Size = 1;
                Param.Value = MostraCodigoEscola;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@LinhasPorPagina";
                Param.Size = 4;
                if (LinhasPorPagina > 0)
                {
                    Param.Value = LinhasPorPagina;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@Pagina";
                Param.Size = 4;
                if (LinhasPorPagina > 0)
                {
                    Param.Value = Pagina;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@SortDirection";
                Param.Size = 4;
                if (SortDirection >= 0)
                {
                    Param.Value = SortDirection;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@SortExpression";
                Param.Size = 300;
                if (!string.IsNullOrEmpty(SortExpression))
                {
                    Param.Value = SortExpression;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                if (LinhasPorPagina > 0 && Pagina >= 0)
                {
                    totalRecords = qs.Return.Rows.Count > 0 ?
                        Convert.ToInt32(qs.Return.Rows[0]["qtd_registros"]) :
                        0;
                }
                else
                {
                    totalRecords = qs.Return.Rows.Count;
                }

                return qs.Return;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Utilizado na consulta de documentos do aluno
        /// </summary>
        public DataTable SelectBy_PesquisaDocumentoAluno_HistoricoEscolarPedagogico
        (
            int cal_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Int64 tur_id
            , byte tipoBusca
            , string pes_nome
            , string alc_matricula
            , Guid ent_id
            , Guid uad_idSuperior
            , bool adm
            , Guid usu_id
            , Guid gru_id
            , bool buscaMatriculaIgual
            , bool MostraCodigoEscola
            , bool emitirDocAnoAnt
            , int LinhasPorPagina
            , int Pagina
            , int SortDirection
            , string SortExpression
            , out int totalRecords
            , bool documentoOficial
       )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_BuscaDocumentos_HistoricoEscolar005", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                if (uad_idSuperior == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = uad_idSuperior;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@cal_id";
                if (cal_id > 0)
                    Param.Value = cal_id;
                else
                    Param.Value = DBNull.Value;
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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                if (crp_id > 0)
                    Param.Value = crp_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tipobusca";
                Param.Size = 1;
                Param.Value = tipoBusca;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pes_nome";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(pes_nome))
                    Param.Value = pes_nome;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alc_matricula";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(alc_matricula))
                    Param.Value = alc_matricula;
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
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@trazMatriculaIgual";
                Param.Size = 1;
                Param.Value = buscaMatriculaIgual;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@MostraCodigoEscola";
                Param.Size = 1;
                Param.Value = MostraCodigoEscola;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@emitirDocAnoAnt";
                Param.Size = 1;
                Param.Value = emitirDocAnoAnt;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@LinhasPorPagina";
                Param.Size = 4;
                if (LinhasPorPagina > 0)
                {
                    Param.Value = LinhasPorPagina;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@Pagina";
                Param.Size = 4;
                if (LinhasPorPagina > 0)
                {
                    Param.Value = Pagina;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@SortDirection";
                Param.Size = 4;
                if (SortDirection >= 0)
                {
                    Param.Value = SortDirection;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@SortExpression";
                Param.Size = 300;
                if (!string.IsNullOrEmpty(SortExpression))
                {
                    Param.Value = SortExpression;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@documentoOficial";
                Param.Size = 1;
                Param.Value = documentoOficial;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                if (LinhasPorPagina > 0 && Pagina >= 0)
                {
                    totalRecords = qs.Return.Rows.Count > 0 ?
                        Convert.ToInt32(qs.Return.Rows[0]["qtd_registros"]) :
                        0;
                }
                else
                {
                    totalRecords = qs.Return.Rows.Count;
                }

                return qs.Return;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna um datatable contendo todos os alunos
        /// que não foram excluídos logicamente e traz o último curriculo
        /// inativo do aluno por permissão do usuário
        /// </summary>
        public DataTable SelectBy_PesquisaDocumentoAluno_HistoricoEscolar
        (
            int cal_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Int64 tur_id
            , byte tipoBusca
            , string pes_nome
            , DateTime pes_dataNascimento
            , string pes_nomeMae
            , string alc_matricula
            , string alc_matriculaEstadual
            , Guid ent_id
            , Guid uad_idSuperior
            , bool adm
            , Guid usu_id
            , Guid gru_id
            , bool buscaMatriculaIgual
            , bool MostraCodigoEscola
            , int LinhasPorPagina
            , int Pagina
            , int SortDirection
            , string SortExpression
            , out int totalRecords
       )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_BuscaDocumentos_HistoricoEscolar", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                if (uad_idSuperior == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = uad_idSuperior;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@cal_id";
                if (cal_id > 0)
                    Param.Value = cal_id;
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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                if (crp_id > 0)
                    Param.Value = crp_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tipobusca";
                Param.Size = 1;
                Param.Value = tipoBusca;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pes_nome";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(pes_nome))
                    Param.Value = pes_nome;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Date;
                Param.ParameterName = "@pes_dataNascimento";
                Param.Size = 3;
                if (pes_dataNascimento != new DateTime())
                    Param.Value = pes_dataNascimento;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alc_matricula";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(alc_matricula))
                    Param.Value = alc_matricula;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alc_matriculaEstadual";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(alc_matriculaEstadual))
                    Param.Value = alc_matriculaEstadual;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pes_nomeMae";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(pes_nomeMae))
                    Param.Value = pes_nomeMae;
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
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@trazMatriculaIgual";
                Param.Size = 1;
                Param.Value = buscaMatriculaIgual;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@MostraCodigoEscola";
                Param.Size = 1;
                Param.Value = MostraCodigoEscola;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@LinhasPorPagina";
                Param.Size = 4;
                if (LinhasPorPagina > 0)
                {
                    Param.Value = LinhasPorPagina;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@Pagina";
                Param.Size = 4;
                if (LinhasPorPagina > 0)
                {
                    Param.Value = Pagina;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@SortDirection";
                Param.Size = 4;
                if (SortDirection >= 0)
                {
                    Param.Value = SortDirection;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@SortExpression";
                Param.Size = 300;
                if (!string.IsNullOrEmpty(SortExpression))
                {
                    Param.Value = SortExpression;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                if (LinhasPorPagina > 0 && Pagina >= 0)
                {
                    totalRecords = qs.Return.Rows.Count > 0 ?
                        Convert.ToInt32(qs.Return.Rows[0]["qtd_registros"]) :
                        0;
                }
                else
                {
                    totalRecords = qs.Return.Rows.Count;
                }

                return qs.Return;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Utilizado na consulta de documentos do aluno para o
        /// certificado de conclusao de etapa de ensino
        /// </summary>
        public DataTable SelectBy_PesquisaDocumentoAluno_CertificadoConclusaoCurso
        (
            int cal_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Int64 tur_id
            , byte tipoBusca
            , string pes_nome
            , DateTime pes_dataNascimento
            , string pes_nomeMae
            , string alc_matricula
            , string alc_matriculaEstadual
            , Guid ent_id
            , Guid uad_idSuperior
            , bool adm
            , Guid usu_id
            , Guid gru_id
            , bool emitirDocAnoAnt
            , bool buscaMatriculaIgual
            , bool MostraCodigoEscola
            , int LinhasPorPagina
            , int Pagina
            , int SortDirection
            , string SortExpression
            , out int totalRecords
       )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_BuscaDocumentos_CertificadoConclusaoCurso", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                if (uad_idSuperior == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = uad_idSuperior;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@cal_id";
                if (cal_id > 0)
                    Param.Value = cal_id;
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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                if (crp_id > 0)
                    Param.Value = crp_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tipobusca";
                Param.Size = 1;
                Param.Value = tipoBusca;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pes_nome";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(pes_nome))
                    Param.Value = pes_nome;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Date;
                Param.ParameterName = "@pes_dataNascimento";
                Param.Size = 3;
                if (pes_dataNascimento != new DateTime())
                    Param.Value = pes_dataNascimento;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alc_matricula";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(alc_matricula))
                    Param.Value = alc_matricula;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alc_matriculaEstadual";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(alc_matriculaEstadual))
                    Param.Value = alc_matriculaEstadual;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pes_nomeMae";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(pes_nomeMae))
                    Param.Value = pes_nomeMae;
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
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@emitirDocAnoAnt";
                Param.Size = 1;
                Param.Value = emitirDocAnoAnt;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@trazMatriculaIgual";
                Param.Size = 1;
                Param.Value = buscaMatriculaIgual;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@MostraCodigoEscola";
                Param.Size = 1;
                Param.Value = MostraCodigoEscola;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@LinhasPorPagina";
                Param.Size = 4;
                if (LinhasPorPagina > 0)
                {
                    Param.Value = LinhasPorPagina;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@Pagina";
                Param.Size = 4;
                if (LinhasPorPagina > 0)
                {
                    Param.Value = Pagina;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@SortDirection";
                Param.Size = 4;
                if (SortDirection >= 0)
                {
                    Param.Value = SortDirection;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@SortExpression";
                Param.Size = 300;
                if (!string.IsNullOrEmpty(SortExpression))
                {
                    Param.Value = SortExpression;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                if (LinhasPorPagina > 0 && Pagina >= 0)
                {
                    totalRecords = qs.Return.Rows.Count > 0 ?
                        Convert.ToInt32(qs.Return.Rows[0]["qtd_registros"]) :
                        0;
                }
                else
                {
                    totalRecords = qs.Return.Rows.Count;
                }

                return qs.Return;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }


        /// <summary>
        /// Utilizado na consulta de documentos do aluno
        /// </summary>
        public DataTable SelectBy_PesquisaAcompanhamentoIndividualAluno
        (
            int cal_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Int64 tur_id
            , byte tipoBusca
            , string pes_nome
            , DateTime pes_dataNascimento
            , string pes_nomeMae
            , string alc_matricula
            , string alc_matriculaEstadual
            , Guid ent_id
            , Guid uad_idSuperior
            , bool adm
            , Guid usu_id
            , Guid gru_id
            , bool buscaMatriculaIgual
            , bool MostraCodigoEscola
            , out int totalRecords
       )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_AcompanhamentoIndividual", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                if (uad_idSuperior == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = uad_idSuperior;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@cal_id";
                if (cal_id > 0)
                    Param.Value = cal_id;
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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                if (crp_id > 0)
                    Param.Value = crp_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tipobusca";
                Param.Size = 1;
                Param.Value = tipoBusca;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pes_nome";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(pes_nome))
                    Param.Value = pes_nome;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Date;
                Param.ParameterName = "@pes_dataNascimento";
                Param.Size = 3;
                if (pes_dataNascimento != new DateTime())
                    Param.Value = pes_dataNascimento;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alc_matricula";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(alc_matricula))
                    Param.Value = alc_matricula;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alc_matriculaEstadual";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(alc_matriculaEstadual))
                    Param.Value = alc_matriculaEstadual;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pes_nomeMae";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(pes_nomeMae))
                    Param.Value = pes_nomeMae;
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
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@trazMatriculaIgual";
                Param.Size = 1;
                Param.Value = buscaMatriculaIgual;
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
            catch (Exception)
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Utilizado na consulta de alunos para o grafico individual de notas.
        /// </summary>
        public DataTable SelectBy_PesquisaGraficoIndividualNotas
        (
            int cal_id
            , int tpc_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Int64 tur_id
            , byte tipoBusca
            , string pes_nome
            , DateTime pes_dataNascimento
            , string pes_nomeMae
            , string alc_matricula
            , string alc_matriculaEstadual
            , Guid ent_id
            , Guid uad_idSuperior
            , bool adm
            , Guid usu_id
            , Guid gru_id
            , bool buscaMatriculaIgual
            , bool MostraCodigoEscola
            , out int totalRecords
       )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_BuscaGrafIndividualNotas", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                if (uad_idSuperior == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = uad_idSuperior;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@cal_id";
                if (cal_id > 0)
                    Param.Value = cal_id;
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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                if (crp_id > 0)
                    Param.Value = crp_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                if (tpc_id > 0)
                    Param.Value = tpc_id;
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
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tipobusca";
                Param.Size = 1;
                Param.Value = tipoBusca;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pes_nome";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(pes_nome))
                    Param.Value = pes_nome;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Date;
                Param.ParameterName = "@pes_dataNascimento";
                Param.Size = 3;
                if (pes_dataNascimento != new DateTime())
                    Param.Value = pes_dataNascimento;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alc_matricula";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(alc_matricula))
                    Param.Value = alc_matricula;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alc_matriculaEstadual";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(alc_matriculaEstadual))
                    Param.Value = alc_matriculaEstadual;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pes_nomeMae";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(pes_nomeMae))
                    Param.Value = pes_nomeMae;
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
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@trazMatriculaIgual";
                Param.Size = 1;
                Param.Value = buscaMatriculaIgual;
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
            catch (Exception)
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Utilizado na consulta de documentos do aluno
        /// </summary>
        public DataTable SelectBy_PesquisaDocumentoAluno
        (
            int cal_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Int64 tur_id
            , byte tipoBusca
            , string pes_nome
            , DateTime pes_dataNascimento
            , string pes_nomeMae
            , string alc_matricula
            , string alc_matriculaEstadual
            , Guid ent_id
            , Guid uad_idSuperior
            , bool adm
            , Guid usu_id
            , Guid gru_id
            , bool emitirDocAnoAnt
            , bool buscaMatriculaIgual
            , bool MostraCodigoEscola
            , int LinhasPorPagina
            , int Pagina
            , int SortDirection
            , string SortExpression
            , bool documentoOficial
            , out int totalRecords
       )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_BuscaDocumentos", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                if (uad_idSuperior == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = uad_idSuperior;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@cal_id";
                if (cal_id > 0)
                    Param.Value = cal_id;
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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                if (crp_id > 0)
                    Param.Value = crp_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tipobusca";
                Param.Size = 1;
                Param.Value = tipoBusca;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pes_nome";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(pes_nome))
                    Param.Value = pes_nome;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Date;
                Param.ParameterName = "@pes_dataNascimento";
                Param.Size = 3;
                if (pes_dataNascimento != new DateTime())
                    Param.Value = pes_dataNascimento;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alc_matricula";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(alc_matricula))
                    Param.Value = alc_matricula;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alc_matriculaEstadual";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(alc_matriculaEstadual))
                    Param.Value = alc_matriculaEstadual;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pes_nomeMae";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(pes_nomeMae))
                    Param.Value = pes_nomeMae;
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
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@emitirDocAnoAnt";
                Param.Size = 1;
                Param.Value = emitirDocAnoAnt;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@trazMatriculaIgual";
                Param.Size = 1;
                Param.Value = buscaMatriculaIgual;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@MostraCodigoEscola";
                Param.Size = 1;
                Param.Value = MostraCodigoEscola;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@LinhasPorPagina";
                Param.Size = 4;
                if (LinhasPorPagina > 0)
                {
                    Param.Value = LinhasPorPagina;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@Pagina";
                Param.Size = 4;
                if (LinhasPorPagina > 0)
                {
                    Param.Value = Pagina;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@SortDirection";
                Param.Size = 4;
                if (SortDirection >= 0)
                {
                    Param.Value = SortDirection;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@SortExpression";
                Param.Size = 300;
                if (!string.IsNullOrEmpty(SortExpression))
                {
                    Param.Value = SortExpression;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@documentoOficial";
                Param.Size = 1;
                Param.Value = documentoOficial;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                if (LinhasPorPagina > 0 && Pagina >= 0)
                {
                    totalRecords = qs.Return.Rows.Count > 0 ?
                        Convert.ToInt32(qs.Return.Rows[0]["qtd_registros"]) :
                        0;
                }
                else
                {
                    totalRecords = qs.Return.Rows.Count;
                }

                return qs.Return;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Utilizado na consulta de documentos do aluno
        /// </summary>
        public DataTable SelectBy_PesquisaDocumentoAluno_GraficoIndividualNotas
        (
            int cal_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Int64 tur_id
            , byte tipoBusca
            , string pes_nome
            , DateTime pes_dataNascimento
            , string pes_nomeMae
            , string alc_matricula
            , string alc_matriculaEstadual
            , Guid ent_id
            , Guid uad_idSuperior
            , bool adm
            , Guid usu_id
            , Guid gru_id
            , bool buscaMatriculaIgual
            , bool MostraCodigoEscola
            , int LinhasPorPagina
            , int Pagina
            , int SortDirection
            , string SortExpression
            , out int totalRecords
            , bool documentoOficial
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_BuscaDocumentos_GraficoIndividualNotas", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                if (uad_idSuperior == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = uad_idSuperior;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@cal_id";
                if (cal_id > 0)
                    Param.Value = cal_id;
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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                if (crp_id > 0)
                    Param.Value = crp_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tipobusca";
                Param.Size = 1;
                Param.Value = tipoBusca;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pes_nome";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(pes_nome))
                    Param.Value = pes_nome;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Date;
                Param.ParameterName = "@pes_dataNascimento";
                Param.Size = 3;
                if (pes_dataNascimento != new DateTime())
                    Param.Value = pes_dataNascimento;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alc_matricula";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(alc_matricula))
                    Param.Value = alc_matricula;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alc_matriculaEstadual";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(alc_matriculaEstadual))
                    Param.Value = alc_matriculaEstadual;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pes_nomeMae";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(pes_nomeMae))
                    Param.Value = pes_nomeMae;
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
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@trazMatriculaIgual";
                Param.Size = 1;
                Param.Value = buscaMatriculaIgual;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@MostraCodigoEscola";
                Param.Size = 1;
                Param.Value = MostraCodigoEscola;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@LinhasPorPagina";
                Param.Size = 4;
                if (LinhasPorPagina > 0)
                {
                    Param.Value = LinhasPorPagina;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@Pagina";
                Param.Size = 4;
                if (LinhasPorPagina > 0)
                {
                    Param.Value = Pagina;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@SortDirection";
                Param.Size = 4;
                if (SortDirection >= 0)
                {
                    Param.Value = SortDirection;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@SortExpression";
                Param.Size = 300;
                if (!string.IsNullOrEmpty(SortExpression))
                {
                    Param.Value = SortExpression;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@documentoOficial";
                Param.Size = 1;
                Param.Value = documentoOficial;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                if (LinhasPorPagina > 0 && Pagina >= 0)
                {
                    totalRecords = qs.Return.Rows.Count > 0 ?
                        Convert.ToInt32(qs.Return.Rows[0]["qtd_registros"]) :
                        0;
                }
                else
                {
                    totalRecords = qs.Return.Rows.Count;
                }

                return qs.Return;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Utilizado na consulta de documentos do docente
        /// </summary>
        public DataTable SelectBy_PesquisaAnotacoesAluno
        (
            int cal_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Int64 tur_id
            , int cap_id
            , Int64 tud_id
            , byte tipoBusca
            , string pes_nome
            , DateTime pes_dataNascimento
            , string pes_nomeMae
            , string alc_matricula
            , string alc_matriculaEstadual
            , Guid ent_id
            , Guid uad_idSuperior
            , bool adm
            , Guid usu_id
            , Guid gru_id
            , bool emitirDocAnoAnt
            , bool buscaMatriculaIgual
            , bool MostraCodigoEscola
            , int LinhasPorPagina
            , int Pagina
            , int SortDirection
            , string SortExpression
            , out int totalRecords
            , bool documentoOficial
       )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_BuscaAnotacoes", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                if (uad_idSuperior == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = uad_idSuperior;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@cal_id";
                if (cal_id > 0)
                    Param.Value = cal_id;
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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                if (crp_id > 0)
                    Param.Value = crp_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cap_id";
                Param.Size = 4;
                if (cap_id > 0)
                    Param.Value = cap_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 4;
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
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tipobusca";
                Param.Size = 1;
                Param.Value = tipoBusca;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pes_nome";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(pes_nome))
                    Param.Value = pes_nome;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Date;
                Param.ParameterName = "@pes_dataNascimento";
                Param.Size = 3;
                if (pes_dataNascimento != new DateTime())
                    Param.Value = pes_dataNascimento;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alc_matricula";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(alc_matricula))
                    Param.Value = alc_matricula;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alc_matriculaEstadual";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(alc_matriculaEstadual))
                    Param.Value = alc_matriculaEstadual;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pes_nomeMae";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(pes_nomeMae))
                    Param.Value = pes_nomeMae;
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
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@emitirDocAnoAnt";
                Param.Size = 1;
                Param.Value = emitirDocAnoAnt;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@trazMatriculaIgual";
                Param.Size = 1;
                Param.Value = buscaMatriculaIgual;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@MostraCodigoEscola";
                Param.Size = 1;
                Param.Value = MostraCodigoEscola;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@LinhasPorPagina";
                Param.Size = 4;
                if (LinhasPorPagina > 0)
                {
                    Param.Value = LinhasPorPagina;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@Pagina";
                Param.Size = 4;
                if (LinhasPorPagina > 0)
                {
                    Param.Value = Pagina;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@SortDirection";
                Param.Size = 4;
                if (SortDirection >= 0)
                {
                    Param.Value = SortDirection;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@SortExpression";
                Param.Size = 300;
                if (!string.IsNullOrEmpty(SortExpression))
                {
                    Param.Value = SortExpression;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@documentoOficial";
                Param.Size = 1;
                Param.Value = documentoOficial;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                if (LinhasPorPagina > 0 && Pagina >= 0)
                {
                    totalRecords = qs.Return.Rows.Count > 0 ?
                        Convert.ToInt32(qs.Return.Rows[0]["qtd_registros"]) :
                        0;
                }
                else
                {
                    totalRecords = qs.Return.Rows.Count;
                }

                return qs.Return;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Utilizado na consulta de documentos do aluno - Ficha Individual
        /// </summary>
        public DataTable SelectBy_PesquisaDocumentoAluno_FichaIndividual
        (
            int cal_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Int64 tur_id
            , byte tipoBusca
            , string pes_nome
            , DateTime pes_dataNascimento
            , string pes_nomeMae
            , string alc_matricula
            , string alc_matriculaEstadual
            , Guid ent_id
            , Guid uad_idSuperior
            , bool adm
            , Guid usu_id
            , Guid gru_id
            , bool emitirDocAnoAnt
            , bool buscaMatriculaIgual
            , bool MostraCodigoEscola
            , int LinhasPorPagina
            , int Pagina
            , int SortDirection
            , string SortExpression
            , out int totalRecords
       )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_BuscaDocumentos_FichaIndividual", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                if (uad_idSuperior == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = uad_idSuperior;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@cal_id";
                if (cal_id > 0)
                    Param.Value = cal_id;
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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                if (crp_id > 0)
                    Param.Value = crp_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tipobusca";
                Param.Size = 1;
                Param.Value = tipoBusca;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pes_nome";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(pes_nome))
                    Param.Value = pes_nome;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Date;
                Param.ParameterName = "@pes_dataNascimento";
                Param.Size = 3;
                if (pes_dataNascimento != new DateTime())
                    Param.Value = pes_dataNascimento;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alc_matricula";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(alc_matricula))
                    Param.Value = alc_matricula;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alc_matriculaEstadual";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(alc_matriculaEstadual))
                    Param.Value = alc_matriculaEstadual;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pes_nomeMae";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(pes_nomeMae))
                    Param.Value = pes_nomeMae;
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
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@emitirDocAnoAnt";
                Param.Size = 1;
                Param.Value = emitirDocAnoAnt;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@trazMatriculaIgual";
                Param.Size = 1;
                Param.Value = buscaMatriculaIgual;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@MostraCodigoEscola";
                Param.Size = 1;
                Param.Value = MostraCodigoEscola;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@LinhasPorPagina";
                Param.Size = 4;
                if (LinhasPorPagina > 0)
                {
                    Param.Value = LinhasPorPagina;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@Pagina";
                Param.Size = 4;
                if (LinhasPorPagina > 0)
                {
                    Param.Value = Pagina;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@SortDirection";
                Param.Size = 4;
                if (SortDirection >= 0)
                {
                    Param.Value = SortDirection;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@SortExpression";
                Param.Size = 300;
                if (!string.IsNullOrEmpty(SortExpression))
                {
                    Param.Value = SortExpression;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                if (LinhasPorPagina > 0 && Pagina >= 0)
                {
                    totalRecords = qs.Return.Rows.Count > 0 ?
                        Convert.ToInt32(qs.Return.Rows[0]["qtd_registros"]) :
                        0;
                }
                else
                {
                    totalRecords = qs.Return.Rows.Count;
                }

                return qs.Return;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion

        #region Métodos de consulta - Boletim do aluno

        /// <summary>
        /// Retorna todos os dados do boletim de todos os alunos informados na lista em suas respectivas matriculas turmas.
        /// </summary>
        /// <param name="dtAlunoMatriculaTurma">DataTable com os alunos e suas matrículas.</param>
        /// <param name="tpc_id">Id do período do calendário.</param>
        /// <returns></returns>
        public DataTable BuscaBoletimAlunos(DataTable dtAlunoMatriculaTurma, int tpc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_BuscaDadosBoletimAlunos", _Banco);

            #region PARAMETROS

            SqlParameter sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@alunoMatriculaTurma";
            sqlParam.SqlDbType = SqlDbType.Structured;
            sqlParam.TypeName = "TipoTabela_AlunoMatriculaTurma";
            sqlParam.Value = dtAlunoMatriculaTurma;
            qs.Parameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.DbType = DbType.Int32;
            sqlParam.ParameterName = "@tpc_id";
            sqlParam.Value = tpc_id;
            qs.Parameters.Add(sqlParam);

            #endregion

            qs.Execute();
            qs.Parameters.Clear();

            return qs.Return;
        }

        /// <summary>
        /// Para cada alu_id informado, retorna o mtu_id correspondente a cada tpc_id do calendário.
        /// </summary>
        /// <param name="dtAlunos">Tabela com os campos alu_id e mtu_id (opcional). Quando mtu_id for omitido, será considerado o mais recente.</param>
        /// <returns>Tabela com os campos alu_id, mtu_id, tpc_id, tpc_ordem.</returns>
        public DataTable BuscarMatriculasPeriodos(DataTable dtAlunos)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_BuscarMatriculasPeriodos", _Banco);

            #region PARAMETROS

            SqlParameter sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@alunoMatriculaTurma";
            sqlParam.SqlDbType = SqlDbType.Structured;
            sqlParam.TypeName = "TipoTabela_AlunoMatriculaTurma";
            sqlParam.Value = dtAlunos;
            qs.Parameters.Add(sqlParam);

            #endregion

            qs.Execute();
            qs.Parameters.Clear();

            return qs.Return;
        }

        #endregion

        #region Métodos de consulta - Histórico escolar

        public DataTable BuscaDadosAluno(Int64 alu_id, bool documentoOficial)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Aluno_BuscaDadosHistoricoDoAluno", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@alu_id";
            Param.Size = 8;
            Param.Value = alu_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@documentoOficial";
            Param.Size = 1;
            Param.Value = documentoOficial;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();
            qs.Parameters.Clear();

            return qs.Return;
        }


        #endregion
    }
}
