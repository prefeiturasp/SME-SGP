/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL.Abstracts;
using MSTech.GestaoEscolar.Entities;

namespace MSTech.GestaoEscolar.DAL
{
    /// <summary>
    /// Classe CLS_AlunoAvaliacaoTurmaDisciplinaDAO
    /// </summary>
    public class CLS_AlunoAvaliacaoTurmaDisciplinaDAO : Abstract_CLS_AlunoAvaliacaoTurmaDisciplinaDAO
    {
        #region Métodos

        public DataTable SelecionaDadosHistoricoTransferencia(long alu_id, int mtu_id, long doc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_AlunoAvaliacaoTurmaDisciplina_SelecionaDadosHistoricoTransferencia", _Banco);

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
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@doc_id";
                Param.Size = 8;
                if (doc_id > 0)
                {
                    Param.Value = doc_id;
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
        /// Busca as efetivações da matrícula turma disicplina do aluno de todas as
        /// disciplinas oferecidas de acordo com o formato de avaliação.
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="mtu_id">ID da matrícula do aluno</param>
        /// <param name="fav_id">ID do formato de avaliação</param>
        /// <param name="ava_id">ID da Avaliação</param>
        /// <returns>DataTable contendo as efetivações da matrícula turma disciplina oferecidas do aluno</returns>
        public DataTable SelecionaEfetivacaoDisciplinasOferecidas_Aluno(long alu_id, int mtu_id, int fav_id, int ava_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_AlunoAvaliacaoTurmaDisciplina_SelectEfetivacaoDisciplinasOferecidas_Aluno", _Banco);

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

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna os dados da CLS_AlunoAvaliacaoTurmaDisciplina cadastrados para a disciplina
        /// e avaliação informados.
        /// </summary>
        /// <param name="tud_id">Id da turma disciplina - obrigatório</param>
        /// <param name="fav_id">ID do formato de avaliação</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <returns>Lista de CLS_AlunoAvaliacaoTurma</returns>
        public DataTable SelectBy_DisciplinaAvaliacao
        (
            long tud_id
            , int fav_id
            , int ava_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_AlunoAvaliacaoTurmaDisciplina_SelectBy_DisciplinaAvaliacao", _Banco);

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
            Param.ParameterName = "@ava_id";
            Param.Size = 4;
            Param.Value = ava_id;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna os dados da CLS_AlunoAvaliacaoTurmaDisciplina cadastrados para a disciplina
        /// e avaliação informados.
        /// </summary>
        /// <param name="tud_id">Id da turma disciplina - obrigatório</param>
        /// <param name="fav_id">ID do formato de avaliação</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <returns>Lista de CLS_AlunoAvaliacaoTurma</returns>
        public DataTable SelectBy_DisciplinaAvaliacaoRegencia
        (
            long tur_id
            , int fav_id
            , int ava_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_AlunoAvaliacaoTurmaDisciplina_SelectBy_DisciplinaAvaliacaoRegencia", _Banco);

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

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna os dados da CLS_AlunoAvaliacaoTurmaDisciplina cadastrados para a disciplinas
        /// e avaliação informados.
        /// </summary>
        /// <param name="tud_id">Lista com Id´s da turma disciplina</param>
        /// <param name="fav_id">ID do formato de avaliação</param>
        /// <param name="ava_id">ID da avaliação</param>
        /// <returns>Lista de CLS_AlunoAvaliacaoTurma</returns>
        public DataTable SelectBy_DisciplinaAvaliacaoTurmaDisciplina
        (
            string tud_ids
            , int fav_id
            , int ava_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_AlunoAvaliacaoTurmaDisciplina_SelectBy_DisciplinaAvaliacaoTurmaDisciplina", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@tud_ids";
            Param.Value = tud_ids;
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

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna os dados da CLS_AlunoAvaliacaoTurmaDisciplina que sejam pela 
        /// "chave" da matrícula do aluno na disciplina.
        /// </summary>
        /// <param name="tud_id">Id da turma disciplina - obrigatório</param>
        /// <param name="alu_id">Id do aluno - obrigatório</param>
        /// <param name="mtu_id">Id da matrícula do aluno na turma - obrigatório</param>
        /// <param name="mtd_id">Id da matrícula do aluno na disciplina - obrigatório</param>
        /// <returns>Lista de CLS_AlunoAvaliacaoTurma</returns>
        public List<CLS_AlunoAvaliacaoTurmaDisciplina> SelectBy_Disciplina_Aluno
        (
            long tud_id
            , long alu_id
            , int mtu_id
            , int mtd_id
        )
        {
            List<CLS_AlunoAvaliacaoTurmaDisciplina> lista = new List<CLS_AlunoAvaliacaoTurmaDisciplina>();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_AlunoAvaliacaoTurmaDisciplina_SelectBy_Disciplina_Aluno", _Banco);

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
                CLS_AlunoAvaliacaoTurmaDisciplina entity = new CLS_AlunoAvaliacaoTurmaDisciplina();
                entity = DataRowToEntity(dr, entity);

                lista.Add(entity);
            }

            return lista;
        }

        /// <summary>
        /// Retorna uma entidade carregada, buscando pela "chave" da avaliação do aluno 
        /// (parâmetros).
        /// </summary>
        /// <param name="tud_id">Id da turma disciplina - obrigatório</param>
        /// <param name="alu_id">Id do aluno - obrigatório</param>
        /// <param name="mtu_id">Id da matrícula do aluno na turma - obrigatório</param>
        /// <param name="mtd_id">Id da matrícula do aluno na disciplina - obrigatório</param>
        /// <param name="fav_id">Id do formato de avaliação - obrigatório</param>
        /// <param name="ava_id">Id da avaliação - obrigatório</param>
        /// <returns>Entidade CLS_AlunoAvaliacaoTurma</returns>
        public CLS_AlunoAvaliacaoTurmaDisciplina LoadBy_ChaveAvaliacaoAluno
        (
            long tud_id
            , long alu_id
            , int mtu_id
            , int mtd_id
            , int fav_id
            , int ava_id
        )
        {
            CLS_AlunoAvaliacaoTurmaDisciplina entity = new CLS_AlunoAvaliacaoTurmaDisciplina();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_AlunoAvaliacaoTurmaDisciplina_LoadBy_ChaveAvaliacaoAluno", _Banco);

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

            #endregion

            qs.Execute();

            if (qs.Return.Rows.Count > 0)
            {
                entity = DataRowToEntity(qs.Return.Rows[0], entity);
            }

            return entity;
        }

        /// <summary>
        /// Retorna true/false
        /// para saber se a Efetivação (CLS_AlunoAvaliacaoTurmaDisciplina) já está cadastrada
        /// filtradas por tud_id, alu_id, mtd_id, atd_id
        /// </summary>        
        /// <param name="tud_id">Id da tabela TUR_TurmaDisciplina do bd</param>        
        /// <param name="alu_id">Campo alu_id da tabela CLS_AlunoAvaliacaoTurmaDisciplina do bd</param>
        /// <param name="mtd_id">Campo mtd_id da tabela CLS_AlunoAvaliacaoTurmaDisciplina do bd</param>
        /// <param name="mtu_id"></param>
        /// <param name="atd_id">Campo atd_id da tabela CLS_AlunoAvaliacaoTurmaDisciplina do bd</param>
        /// <returns>Retorna true = frequencia já cadastrada | false para frequencia ainda não cadastrado</returns>        
        public bool SelectBy_Chaves
        (
            long tud_id
            , long alu_id
            , int mtd_id
            , int mtu_id
            , int atd_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_AlunoAvaliacaoTurmaDisciplina_SelectBy_Chaves", _Banco);

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
                Param.ParameterName = "@mtd_id";
                Param.Size = 4;
                if (mtd_id > 0)
                    Param.Value = mtd_id;
                else
                    Param.Value = DBNull.Value;
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

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@atd_id";
                Param.Size = 4;
                if (atd_id > 0)
                    Param.Value = atd_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                    return true;

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
        /// Salva os dados obtidos na importação de dados da efetivação de bimestre.
        /// </summary>
        /// <param name="dtAlunoAvaliacaoTurmaDisciplina">Datatable de dados do fechamento.</param>
        /// <returns></returns>
        public bool ImportarDadosFechamento(DataTable dtAlunoAvaliacaoTurmaDisciplina, Int64 tpc_id)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_AlunoAvaliacaoTurmaDisciplina_ImportacaoDadosFechamento", _Banco);

            try
            {
                #region Parâmetro

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbAlunoAvaliacaoTurmaDisciplina";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_AlunoAvaliacaoTurmaDisciplina";
                sqlParam.Value = dtAlunoAvaliacaoTurmaDisciplina;
                qs.Parameters.Add(sqlParam);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tpc_id";
                Param.Size = 8;
                Param.Value = tpc_id;
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
        /// Retorna as notas de fechamento para todas as disciplinas da turma
        /// e para um determinado aluno.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="fav_id">ID do formato de avaliacao</param>
        /// <param name="fav_id">ID do calendario</param>
        /// <returns>DataTable</returns>
        public DataTable SelectBy_AlunoTurma
        (
            long tur_id
            , long alu_id
            , int mtu_id
            , int fav_id
            , int cal_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_AlunoAvaliacaoTurmaDisciplina_SelectBy_AlunoTurma", _Banco);

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
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            Param.Value = cal_id;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Seleciona as pendências de fechamento por disciplinas
        /// </summary>
        /// <param name="dtTurmaDisciplina">Datatable de ids de turma disciplina.</param>
        /// <param name="tev_EfetivacaoNotas">ID do parametro de tipo de evento de fechamento de notas</param>
        /// <returns></returns>
        public DataTable SelecionaPendenciasFechamentoDisciplinas(DataTable dtTurmaDisciplina, int tev_EfetivacaoNotas)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_AlunoAvaliacaoTurmaDisciplina_SelecionaPendenciasFechamentoDisciplinas", _Banco);

            try
            {
                #region Parâmetro

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.ParameterName = "@tabelaTurmaDisciplina";
                sqlParam.TypeName = "TipoTabela_TurmaDisciplina";
                sqlParam.Value = dtTurmaDisciplina;
                qs.Parameters.Add(sqlParam);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tev_EfetivacaoNotas";
                Param.Size = 4;
                Param.Value = tev_EfetivacaoNotas;
                qs.Parameters.Add(Param);

                #endregion Parâmetro

                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Seleciona os alunos da turma para exibir na tela de fechamento do gestor.
        /// </summary>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="ordenacao">Ordenação (Por número de chamado ou por nome).</param>
        /// <param name="tev_idFechamento">Id do tipo de evento do fechamento.</param>
        /// <param name="alu_id">Id do aluno (passar 0 para trazer os dados de todos os alunos).</param>
        /// <returns>Alunos da turma.</returns>
        public DataTable SelecionarAlunosTurma(long tur_id, byte ordenacao, int tev_idFechamento, bool documentoOficial, long alu_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_AlunoAvaliacaoTurmaDisciplina_SelecionarAlunosTurma", _Banco);

            #region Parâmetros

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@ordenacao";
            Param.Size = 1;
            Param.Value = ordenacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tev_idFechamento";
            Param.Size = 4;
            Param.Value = tev_idFechamento;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@documentoOficial";
            Param.Size = 1;
            Param.Value = documentoOficial;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@alu_id";
            Param.Size = 8;
            if (alu_id > 0)
            {
                Param.Value = alu_id;
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

        /// <summary>
        /// Traz os dados do fechamento da matricula do aluno (Baseada na procedure NEW_Relatorio_0001_SubBoletimEscolarAluno).
        /// </summary>
        /// <returns>DataTable com os dados do boletim.</returns>
        public DataTable SelecionaDadosAlunoFechamentoGestor(long alu_id, int mtu_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_AlunoAvaliacaoTurmaDisciplina_DadosAlunoFechamentoGestor", _Banco);

            #region PARAMETROS

            SqlParameter sqlParam = new SqlParameter();
            sqlParam.DbType = DbType.Int64;
            sqlParam.ParameterName = "@alu_id";
            sqlParam.Size = 8;
            sqlParam.Value = alu_id;
            qs.Parameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.DbType = DbType.Int32;
            sqlParam.ParameterName = "@mtu_id";
            sqlParam.Size = 4;
            sqlParam.Value = mtu_id;
            qs.Parameters.Add(sqlParam);

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        #endregion

        #region Métodos em lote

        /// <summary>
        /// Retorna do banco uma lista de entidades com as chaves passadas na tabela
        /// </summary>
        /// <param name="dtAlunoAvaliacaoTurmaDisciplina">Datatable com os dados do fechamento do bimestre.</param>
        /// <returns></returns>
        public List<CLS_AlunoAvaliacaoTurmaDisciplina> GetEntity_EmLote(DataTable dtAlunoAvaliacaoTurmaDisciplina)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_AlunoAvaliacaoTurmaDisciplina_GetEntity_EmLote", _Banco);

            try
            {
                #region Parâmetro

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@AlunoAvaliacaoTurmaDisciplina";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_AlunoAvaliacaoTurmaDisciplina";
                sqlParam.Value = dtAlunoAvaliacaoTurmaDisciplina;
                qs.Parameters.Add(sqlParam);

                #endregion

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                    return (from DataRow dr in qs.Return.Rows
                            select DataRowToEntity(dr, new CLS_AlunoAvaliacaoTurmaDisciplina())).ToList();

                return new List<CLS_AlunoAvaliacaoTurmaDisciplina>();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion

        #region Métodos em lote

        /// <summary>
        /// Salva os dados do fechamento de bimestre em lote.
        /// </summary>
        /// <param name="dtAlunoAvaliacaoTurmaDisciplina">Datatable com os dados do fechamento do bimestre.</param>
        /// <returns></returns>
        public bool SalvarEmLote(DataTable dtAlunoAvaliacaoTurmaDisciplina)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_AlunoAvaliacaoTurmaDisciplina_SalvarEmLote", _Banco);

            try
            {
                #region Parâmetro

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@AlunoAvaliacaoTurmaDisciplina";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_AlunoAvaliacaoTurmaDisciplina";
                sqlParam.Value = dtAlunoAvaliacaoTurmaDisciplina;
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
        /// Salva os dados do fechamento de bimestre do gestor em lote.
        /// </summary>
        /// <param name="dtAlunoAvaliacaoTurmaDisciplina">Datatable com os dados do fechamento do bimestre.</param>
        /// <returns></returns>
        public bool SalvarEmLotePosConselho(DataTable dtAlunoAvaliacaoTurmaDisciplina)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_AlunoAvaliacaoTurmaDisciplina_SalvarEmLotePosConselho", _Banco);

            try
            {
                #region Parâmetro

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@AlunoAvaliacaoTurmaDisciplina";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_AlunoAvaliacaoTurmaDisciplina";
                sqlParam.Value = dtAlunoAvaliacaoTurmaDisciplina;
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

        #region Métodos Sobrescritos

        protected override string ConnectionStringName
        {
            get
            {
                return "GestaoEscolar";
            }
        }

        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CLS_AlunoAvaliacaoTurmaDisciplina entity)
        {
            entity.atd_id = Convert.ToInt32(qs.Return.Rows[0][0]);
            return (entity.atd_id > 0);
        }

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_AlunoAvaliacaoTurmaDisciplina entity)
        {
            base.ParamInserir(qs, entity);

            // Verificação pq frequência pode ser igual a zero
            if (entity.atd_frequencia > -1)
                qs.Parameters["@atd_frequencia"].Value = entity.atd_frequencia;
            else
                qs.Parameters["@atd_frequencia"].Value = DBNull.Value;

            // Verificação pq número de faltas pode ser igual a zero
            if (entity.atd_numeroFaltas > -1)
                qs.Parameters["@atd_numeroFaltas"].Value = entity.atd_numeroFaltas;
            else
                qs.Parameters["@atd_numeroFaltas"].Value = DBNull.Value;

            if (entity.atd_numeroFaltasReposicao > -1)
                qs.Parameters["@atd_numeroFaltasReposicao"].Value = entity.atd_numeroFaltasReposicao;
            else
                qs.Parameters["@atd_numeroFaltasReposicao"].Value = DBNull.Value;

            if (entity.atd_numeroFaltasExterna > -1)
                qs.Parameters["@atd_numeroFaltasExterna"].Value = entity.atd_numeroFaltasExterna;
            else
                qs.Parameters["@atd_numeroFaltasExterna"].Value = DBNull.Value;

            qs.Parameters["@atd_relatorio"].DbType = DbType.String;
            qs.Parameters["@atd_relatorio"].Size = Int32.MaxValue;

            qs.Parameters["@atd_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@atd_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamAlterar(QueryStoredProcedure qs, CLS_AlunoAvaliacaoTurmaDisciplina entity)
        {
            base.ParamAlterar(qs, entity);

            // Verificação pq frequência pode ser igual a zero
            if (entity.atd_frequencia > -1)
                qs.Parameters["@atd_frequencia"].Value = entity.atd_frequencia;
            else
                qs.Parameters["@atd_frequencia"].Value = DBNull.Value;

            // Verificação pq número de faltas pode ser igual a zero
            if (entity.atd_numeroFaltas > -1)
                qs.Parameters["@atd_numeroFaltas"].Value = entity.atd_numeroFaltas;
            else
                qs.Parameters["@atd_numeroFaltas"].Value = DBNull.Value;

            if (entity.atd_numeroFaltasReposicao > -1)
                qs.Parameters["@atd_numeroFaltasReposicao"].Value = entity.atd_numeroFaltasReposicao;
            else
                qs.Parameters["@atd_numeroFaltasReposicao"].Value = DBNull.Value;

            if (entity.atd_numeroFaltasExterna > -1)
                qs.Parameters["@atd_numeroFaltasExterna"].Value = entity.atd_numeroFaltasExterna;
            else
                qs.Parameters["@atd_numeroFaltasExterna"].Value = DBNull.Value;

            qs.Parameters["@atd_relatorio"].DbType = DbType.String;
            qs.Parameters["@atd_relatorio"].Size = Int32.MaxValue;

            qs.Parameters.RemoveAt("@atd_dataCriacao");
            qs.Parameters.RemoveAt("@atd_registroexterno");

            qs.Parameters["@atd_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity">Entidade CLS_AlunoAvaliacaoTurmaDisciplina</param>
        /// <returns>true = sucesso | false = fracasso</returns> 
        protected override bool Alterar(CLS_AlunoAvaliacaoTurmaDisciplina entity)
        {
            __STP_UPDATE = "NEW_CLS_AlunoAvaliacaoTurmaDisciplina_Update";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamDeletar(QueryStoredProcedure qs, CLS_AlunoAvaliacaoTurmaDisciplina entity)
        {
            base.ParamDeletar(qs, entity);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@atd_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@atd_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity">Entidade CLS_AlunoAvaliacaoTurmaDisciplina</param>
        /// <returns>true = sucesso | false = fracasso</returns> 
        public override bool Delete(CLS_AlunoAvaliacaoTurmaDisciplina entity)
        {
            __STP_DELETE = "NEW_CLS_AlunoAvaliacaoTurmaDisciplina_Update_Situacao";
            return base.Delete(entity);
        }

        #endregion
    }
}