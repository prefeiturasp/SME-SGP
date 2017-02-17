/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/
using System;
using System.Collections.Generic;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL.Abstracts;

namespace MSTech.GestaoEscolar.DAL
{
    /// <summary>
    /// Tabela responsável pela exibição dos documentos acadêmicos dos alunos disponíveis para impressão de acordo com a entidade do sistema acadêmico.
    /// </summary>
    public class CFG_RelatorioDocumentoAlunoDAO : Abstract_CFG_RelatorioDocumentoAlunoDAO
    {
        #region Consultas

        /// <summary>
        /// Consulta os documentos e seus relatórios por entidade
        /// </summary>
        /// <param name="ent_id">ID da entidade</param>
        /// <returns></returns>
        public DataTable SelecionaPorEntidade(Guid ent_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CFG_RelatorioDocumentoAluno_SelectBy_Entidade", _Banco);

            try
            {
                #region Paramentros

                Param = qs.NewParameter();
                Param.ParameterName = "@ent_id";
                Param.DbType = DbType.Guid;
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
        /// Seleciona todos os documentos do aluno filtrado por ent_id conforme relacionado 
        /// com a tabela CFG_RelatorioServidorRelatorio do banco de dados CoreSSO.
        /// MÉTODO(S) DEPENDENTE(S):
        /// 1 - Classe: CFG_RelatorioDocumentoAlunoBO; Método: ListarDocumentosAluno
        /// </summary>
        /// <param name="ent_id">id da entidade da tabela CFG_RelatorioServidorRelatorio do banco de dados CoreSSO.</param>
        /// <param name="sis_id">id do sistema da tabela CFG_RelatorioServidorRelatorio do banco de dados CoreSSO.</param>
        /// <returns>Lista de documentos do aluno do servidor de relatório da entidade do sistema.</returns>        
        public IList<CFG_RelatorioDocumentoAluno> SelectBy_EntidadeSistema(Guid ent_id)
        {
            IList<CFG_RelatorioDocumentoAluno> lt = new List<CFG_RelatorioDocumentoAluno>();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CFG_RelatorioDocumentoAluno_SELECTBY_EntidadeSistema", this._Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();
                foreach (DataRow dr in qs.Return.Rows)
                {
                    CFG_RelatorioDocumentoAluno entity = new CFG_RelatorioDocumentoAluno();
                    lt.Add(this.DataRowToEntity(dr, entity));
                }
                return lt;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Executa a procedure no banco e retorna um datatable da Declaracao Matricula
        /// </summary>
        /// <param name="alu_ids">Id(s) dos alunos</param>
        /// <param name="tur_id">Id da turma</param>
        /// <param name="situacao">situacao</param>
        /// <param name="ent_id">Entidade logada</param>
        /// <param name="MatriculaEstadual">Nome da matrícula estadual, caso exista</param>
        /// <returns></returns>
        public DataTable Select_DeclaracaoMatricula
        (
            string alu_ids
            , Int64 tur_id
            , Guid ent_id
            , bool situacao
            , string MatriculaEstadual
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_Relatorio_0003_DeclaracaoMatriculaHTML", _Banco);
            try
            {
                #region PARAMETROS


                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@alu_ids";
                if (!String.IsNullOrEmpty(alu_ids))
                    Param.Value = alu_ids;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 4;
                if (tur_id > 0)
                    Param.Value = tur_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                if (ent_id != Guid.Empty)
                    Param.Value = ent_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int16;
                Param.ParameterName = "@situacao";
                Param.Size = 1;
                if (situacao)
                    Param.Value = 1;
                else
                    Param.Value = 0;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@MatriculaEstadual";
                if (!String.IsNullOrEmpty(MatriculaEstadual))
                    Param.Value = MatriculaEstadual;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return.Rows.Count > 0 ? qs.Return : new DataTable();

            }

            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna a entidade filtrada por ent_id e rlt_id
        /// </summary>
        /// <param name="ent_id"></param>
        /// <param name="rlt_id"></param>
        /// <returns></returns>
        public DataTable SelecionaRelatorioDocumentoAluno(Guid ent_id, int rlt_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CFG_RelatorioDocumentoAluno_Seleciona_Sem_Filtros", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@rlt_id";
                Param.Size = 4;
                Param.Value = rlt_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return.Rows.Count > 0 ? qs.Return : new DataTable();

            }

            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Seleciona declaracoes HTML.
        /// </summary>
        /// <param name="alu_ids">IDs alunos</param>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="rlt_id">ID do relatorio.</param>
        /// <param name="cal_id">ID do calendario.</param>
        /// <param name="situacao">situacao</param>
        /// <param name="MatriculaEstadual">Matricula estadual.</param>
        /// <returns></returns>
        public DataTable SelecionaDeclaracoesHTML
        (
            string alu_ids
            , Int64 tur_id
            , Guid ent_id
            , int rlt_id
            , int cal_id
            , bool situacao
            , string MatriculaEstadual
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_Relatorio_DeclaracoesHTML", _Banco);
            try
            {
                #region PARAMETROS


                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@alu_ids";
                if (!String.IsNullOrEmpty(alu_ids))
                    Param.Value = alu_ids;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 4;
                if (tur_id > 0)
                    Param.Value = tur_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                if (ent_id != Guid.Empty)
                    Param.Value = ent_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@rlt_id";
                Param.Size = 4;
                if (rlt_id > 0)
                    Param.Value = rlt_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                if (cal_id > 0)
                    Param.Value = cal_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int16;
                Param.ParameterName = "@situacao";
                Param.Size = 1;
                if (situacao)
                    Param.Value = 1;
                else
                    Param.Value = 0;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@MatriculaEstadual";
                if (!String.IsNullOrEmpty(MatriculaEstadual))
                    Param.Value = MatriculaEstadual;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return.Rows.Count > 0 ? qs.Return : new DataTable();

            }

            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Executa a procedure no banco e retorna um datatable da Declaracao Conclusao Curso
        /// </summary>
        /// <param name="alu_id"></param>
        /// <param name="tur_id"></param>
        /// <param name="cal_id"></param>
        /// <param name="situacao"></param>
        /// <param name="ent_id"></param>
        /// <param name="telefone"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public DataTable Select_DeclaracaoConclusaoCurso
        (
             string alu_id
            , Int64 tur_id
            , int cal_id
            , bool situacao
            , Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs =
                new QuerySelectStoredProcedure("NEW_Relatorio_0003_DeclaracaoConclusaoCursoHTML", _Banco);
            try
            {
                #region PARAMETROS


                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@alu_id";
                if (!String.IsNullOrEmpty(alu_id))
                    Param.Value = alu_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                if (cal_id > 0)
                    Param.Value = cal_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 4;
                if (tur_id > 0)
                    Param.Value = tur_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                if (ent_id != Guid.Empty)
                    Param.Value = ent_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int16;
                Param.ParameterName = "@situacao";
                Param.Size = 1;
                if (situacao)
                    Param.Value = 1;
                else
                    Param.Value = 0;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return.Rows.Count > 0 ? qs.Return : new DataTable();

            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Executa a procedure no banco e retorna um datatable da Declaracao Ex-Aluno Unidade Escolar
        /// </summary>
        /// <param name="alu_id"></param>
        /// <param name="tur_id"></param>
        /// <param name="uni_id"></param>
        /// <param name="esc_id"></param>
        /// <param name="situacao"></param>
        /// <param name="ent_id"></param>
        /// <param name="telefone"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public DataTable Select_DeclaracaoExAlunoUnidadeEscolar
     (

          string alu_id
         , Int64 tur_id
         , int uni_id
         , Int64 esc_id
         , bool situacao
         , Guid ent_id
         , Guid telefone
         , Guid email


     )
        {
            QuerySelectStoredProcedure qs =
                new QuerySelectStoredProcedure("NEW_Relatorio_0003_DeclaracaoExAlunoUnidadeEscolarHTML", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                if (ent_id != Guid.Empty)
                    Param.Value = ent_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@alu_id";
                if (!String.IsNullOrEmpty(alu_id))
                    Param.Value = alu_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 4;
                if (tur_id > 0)
                    Param.Value = tur_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int16;
                Param.ParameterName = "@situacao";
                Param.Size = 1;
                if (situacao)
                    Param.Value = 1;
                else
                    Param.Value = 0;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@telefone";
                if (telefone != Guid.Empty)
                    Param.Value = telefone;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@email";
                if (email != Guid.Empty)
                    Param.Value = email;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@uni_id";
                if (uni_id > 0)
                    Param.Value = uni_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@esc_id";
                if (esc_id > 0)
                    Param.Value = esc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);
                #endregion

                qs.Execute();

                return qs.Return.Rows.Count > 0 ? qs.Return : new DataTable();

            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        ///  Executa a procedure no banco e retorna um datatable da Declaracao Matricula Ex-Aluno
        /// </summary>
        /// <param name="alu_id">Ids dos alunos selecionados</param>
        /// <param name="tur_id">Turma pesquisada, caso selecionado</param>
        /// <param name="situacao">situacao</param>
        /// <param name="ent_id">Entidade logada</param>
        /// <param name="MatriculaEstadual">Nome da matricula estadual, caso exista</param>
        /// <returns></returns>
        public DataTable Select_DeclaracaoPedidoTransferencia
        (
         string alu_id
        , Int64 tur_id
        , bool situacao
        , Guid ent_id
        , string MatriculaEstadual
       )
        {
            QuerySelectStoredProcedure qs =
                new QuerySelectStoredProcedure("NEW_Relatorio_0003_DeclaracaoPedidoTransferenciaHTML", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                if (ent_id != Guid.Empty)
                    Param.Value = ent_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@alu_id";
                if (!String.IsNullOrEmpty(alu_id))
                    Param.Value = alu_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 4;
                if (tur_id > 0)
                    Param.Value = tur_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int16;
                Param.ParameterName = "@situacao";
                Param.Size = 1;
                if (situacao)
                    Param.Value = 1;
                else
                    Param.Value = 0;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@MatriculaEstadual";
                if (!String.IsNullOrEmpty(MatriculaEstadual))
                    Param.Value = MatriculaEstadual;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return.Rows.Count > 0 ? qs.Return : new DataTable();

            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        ///  Executa a procedure no banco e retorna um datatable da Declaracao Matricula Ex-Aluno
        /// </summary>
        /// <param name="alu_id"></param>
        /// <param name="tur_id"></param>
        /// <param name="uni_id"></param>
        /// <param name="esc_id"></param>
        /// <param name="situacao"></param>
        /// <param name="ent_id"></param>
        /// <param name="telefone"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public DataTable Select_DeclaracaoMatriculaExAluno
    (
         string alu_id
        , Int64 tur_id
        , int uni_id
        , int esc_id
        , bool situacao
        , Guid ent_id

    )
        {
            QuerySelectStoredProcedure qs =
                new QuerySelectStoredProcedure("NEW_Relatorio_0003_DeclaracaoMatriculaExAlunoHTML", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                if (ent_id != Guid.Empty)
                    Param.Value = ent_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@alu_id";
                if (!String.IsNullOrEmpty(alu_id))
                    Param.Value = alu_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 4;
                if (tur_id > 0)
                    Param.Value = tur_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int16;
                Param.ParameterName = "@situacao";
                Param.Size = 1;
                if (situacao)
                    Param.Value = 1;
                else
                    Param.Value = 0;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@uni_id";
                if (uni_id > 0)
                    Param.Value = uni_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                if (esc_id > 0)
                    Param.Value = esc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);
                #endregion

                qs.Execute();

                return qs.Return.Rows.Count > 0 ? qs.Return : new DataTable();

            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        ///  Executa a procedure no banco e retorna um datatable da Declaracao Matricula ExAluno Periodo
        /// </summary>
        /// <param name="alu_id"></param>
        /// <param name="tur_id"></param>
        /// <param name="uni_id"></param>
        /// <param name="esc_id"></param>
        /// <param name="cal_ano"></param>
        /// <param name="situacao"></param>
        /// <param name="ent_id"></param>
        /// <param name="telefone"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public DataTable Select_DeclaracaoMatriculaExAlunoPeriodo
    (
         string alu_id
        , Int64 tur_id
        , int uni_id
        , Int64 esc_id
        , string cal_ano
        , bool situacao
        , Guid ent_id
        , Guid telefone
        , Guid email

    )
        {
            QuerySelectStoredProcedure qs =
                new QuerySelectStoredProcedure("NEW_Relatorio_0003_DeclaracaoMatriculaPeriodoHTML", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                if (ent_id != Guid.Empty)
                    Param.Value = ent_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@alu_id";
                if (!String.IsNullOrEmpty(alu_id))
                    Param.Value = alu_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 4;
                if (tur_id > 0)
                    Param.Value = tur_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int16;
                Param.ParameterName = "@situacao";
                Param.Size = 1;
                if (situacao)
                    Param.Value = 1;
                else
                    Param.Value = 0;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@telefone";
                if (telefone != Guid.Empty)
                    Param.Value = telefone;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@email";
                if (email != Guid.Empty)
                    Param.Value = email;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@uni_id";
                if (uni_id > 0)
                    Param.Value = uni_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@esc_id";
                if (esc_id > 0)
                    Param.Value = esc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@cal_ano";
                if (string.IsNullOrEmpty(cal_ano))
                    Param.Value = cal_ano;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return.Rows.Count > 0 ? qs.Return : new DataTable();

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
        /// Método usado para geração de relatório/declaração HTML de solicitação de transferencia.
        /// </summary>
        /// <param name="alu_ids">Id(s) do(s) aluno(s)</param>
        /// <param name="ent_id">Entidade logada</param>
        /// <param name="telefone">Tipo contato telefone</param>
        /// <param name="email">Tipo contato email</param>
        /// <returns>Datatable</returns>
        public DataTable Select_DeclaracaoSolicitacaoTransferencia(string alu_ids, Guid ent_id, Guid telefone, Guid email)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_Relatorio_0003_DeclaracaoSolicitacaoTransferenciaHTML", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@alu_id";
                if (!String.IsNullOrEmpty(alu_ids))
                    Param.Value = alu_ids;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                if (ent_id != Guid.Empty)
                    Param.Value = ent_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@telefone";
                if (telefone != Guid.Empty)
                    Param.Value = telefone;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@email";
                if (email != Guid.Empty)
                    Param.Value = email;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return.Rows.Count > 0 ? qs.Return : new DataTable();

            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Busca Declaracoes HTML
        /// </summary>
        public DataTable SelecionaDeclaracoesHTML( string pda_chave, out int totalRecords)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_Relatorio_BuscaDeclaracoes", _Banco);

            try
            {
                totalRecords = 0;

                #region Parametros
                
                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pda_chave";
                Param.Size = 100;
                Param.Value = pda_chave;
                qs.Parameters.Add(Param);

                #endregion

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
        /// Seleciona a declaracao de comparecimento de responsáveis
        /// </summary>
        /// <param name="alu_ids">Ids dos alunos</param>
        /// <param name="mtu_ids">Ids das matrículas dos alunos</param>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="tra_id">Tipo de responsável na declaração</param>
        /// <param name="data">Data de comparecimento</param>
        /// <param name="horaInicio">Hora inicial de comparecimento</param>
        /// <param name="horaFim">Hora final de comparecimento</param>
        /// <returns></returns>
        public DataTable SelecionaDeclaracaoComparecimentoHTML(string alu_ids, string mtu_ids, int esc_id, int tra_id, DateTime data, string horaInicio, string horaFim)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_Relatorio_0003_CertificadoComparecimento", _Banco);

            try
            {
                #region Parametros

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@alu_id";
                Param.Value = alu_ids;
                qs.Parameters.Add(Param);
                
                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@mtu_id";
                Param.Value = mtu_ids;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                Param.Value = esc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tra_id";
                Param.Size = 4;
                Param.Value = tra_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@data";
                Param.Size = 16;
                Param.Value = data;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@horaInicio";
                Param.Size = 5;
                Param.Value = horaInicio;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@horaFim";
                Param.Size = 5;
                Param.Value = horaFim;
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

        #endregion

        #region Verificações

        /// <summary>
        /// Verifica se já existe um documento salvo com determinado relatório.
        /// </summary>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="rlt_id">ID do relatório</param>
        /// <param name="rda_id">ID do documento do aluno</param>
        /// <returns></returns>
        public bool VerificaRelatorioExistente(Guid ent_id, int rlt_id, int rda_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CFG_RelatorioDocumentoAluno_SelectPorRelatorio", _Banco);

            try
            {
                #region Parametros

                Param = qs.NewParameter();
                Param.ParameterName = "@ent_id";
                Param.DbType = DbType.Guid;
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@rlt_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.Value = rlt_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@rda_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                if (rda_id > 0)
                    Param.Value = rda_id;
                else
                    Param.Value = DBNull.Value;
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
        /// Verifica se já existe um documento salvo com determinada ordem.
        /// </summary>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="rda_id">ID do documento do aluno</param>
        /// <param name="rda_ordem">Ordem do documento</param>
        /// <returns></returns>
        public bool VerificaOrdemExistente(Guid ent_id, int rda_id, int rda_ordem)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CFG_RelatorioDocumentoAluno_SelectPorOrdem", _Banco);

            try
            {
                #region Parametros

                Param = qs.NewParameter();
                Param.ParameterName = "@ent_id";
                Param.DbType = DbType.Guid;
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@rda_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                if (rda_id > 0)
                    Param.Value = rda_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@rda_ordem";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.Value = rda_ordem;
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
        ///  Verifica se já existe um documento salvo com determinado nome.
        /// </summary>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="rda_id">ID do documento do aluno</param>
        /// <param name="rda_nomeDocumento">Nome do documento</param>
        /// <returns></returns>
        public bool VerificaNomeExistente(Guid ent_id, int rda_id, string rda_nomeDocumento)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CFG_RelatorioDocumentoAluno_SelectPorNome", _Banco);

            try
            {
                #region Parametros

                Param = qs.NewParameter();
                Param.ParameterName = "@ent_id";
                Param.DbType = DbType.Guid;
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@rda_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                if (rda_id > 0)
                    Param.Value = rda_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@rda_nomeDocumento";
                Param.DbType = DbType.AnsiString;
                Param.Size = 200;
                Param.Value = rda_nomeDocumento;
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

        #endregion

        #region Sobrescritos

        protected override void ParamInserir(QuerySelectStoredProcedure qs, CFG_RelatorioDocumentoAluno entity)
        {
            entity.rda_dataCriacao = DateTime.Now;
            entity.rda_dataAlteracao = DateTime.Now;
            base.ParamInserir(qs, entity);
        }

        protected override void ParamAlterar(QueryStoredProcedure qs, CFG_RelatorioDocumentoAluno entity)
        {
            entity.rda_dataAlteracao = DateTime.Now;
            base.ParamAlterar(qs, entity);
            qs.Parameters.RemoveAt("@rda_dataCriacao");
        }

        protected override bool Alterar(CFG_RelatorioDocumentoAluno entity)
        {
            __STP_UPDATE = "NEW_CFG_RelatorioDocumentoAluno_UPDATE";
            return base.Alterar(entity);
        }

        protected override void ParamDeletar(QueryStoredProcedure qs, CFG_RelatorioDocumentoAluno entity)
        {
            base.ParamDeletar(qs, entity);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@rda_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@rda_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        public override bool Delete(CFG_RelatorioDocumentoAluno entity)
        {
            __STP_DELETE = "NEW_CFG_RelatorioDocumentoAluno_Update_Situacao";
            return base.Delete(entity);
        }

        #endregion

        #region [Métodos override comentados]

        ///// <summary>
        ///// Inseri os valores da classe em um registro ja existente
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem modificados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Alterar(CFG_RelatorioDocumentoAluno entity)
        //{
        //    return base.Alterar(entity);
        //}
        ///// <summary>
        ///// Inseri os valores da classe em um novo registro
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem inseridos</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Inserir(CFG_RelatorioDocumentoAluno entity)
        //{
        //    return base.Inserir(entity);
        //}
        ///// <summary>
        ///// Carrega um registro da tabela usando os valores nas chaves
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem carregados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Carregar(CFG_RelatorioDocumentoAluno entity)
        //{
        //    return base.Carregar(entity);
        //}
        ///// <summary>
        ///// Exclui um registro do banco
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem apagados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Delete(CFG_RelatorioDocumentoAluno entity)
        //{
        //    return base.Delete(entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Alterar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamAlterar(QueryStoredProcedure qs, CFG_RelatorioDocumentoAluno entity)
        //{
        //    base.ParamAlterar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Carregar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamCarregar(QuerySelectStoredProcedure qs, CFG_RelatorioDocumentoAluno entity)
        //{
        //    base.ParamCarregar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Deletar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamDeletar(QueryStoredProcedure qs, CFG_RelatorioDocumentoAluno entity)
        //{
        //    base.ParamDeletar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Inserir
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamInserir(QuerySelectStoredProcedure qs, CFG_RelatorioDocumentoAluno entity)
        //{
        //    base.ParamInserir(qs, entity);
        //}
        ///// <summary>
        ///// Salva o registro no banco de dados
        ///// </summary>
        ///// <param name="entity">Entidade com os dados para preenchimento para inserir ou alterar</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Salvar(CFG_RelatorioDocumentoAluno entity)
        //{
        //    return base.Salvar(entity);
        //}
        ///// <summary>
        ///// Realiza o select da tabela
        ///// </summary>
        ///// <returns>Lista com todos os registros da tabela</returns>
        //public override IList<CFG_RelatorioDocumentoAluno> Select()
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
        //public override IList<CFG_RelatorioDocumentoAluno> Select_Paginado(int currentPage, int pageSize, out int totalRecord)
        //{
        //    return base.Select_Paginado(currentPage, pageSize, out totalRecord);
        //}
        ///// <summary>
        ///// Recebe o valor do auto incremento e coloca na propriedade 
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CFG_RelatorioDocumentoAluno entity)
        //{
        //    return base.ReceberAutoIncremento(qs, entity);
        //}
        ///// <summary>
        ///// Passa os dados de um datatable para uma entidade
        ///// </summary>
        ///// <param name="dr">DataRow do datatable preenchido</param>
        ///// <param name="entity">Entidade onde ser�o transferidos os dados</param>
        ///// <returns>Entidade preenchida</returns>
        //public override CFG_RelatorioDocumentoAluno DataRowToEntity(DataRow dr, CFG_RelatorioDocumentoAluno entity)
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
        //public override CFG_RelatorioDocumentoAluno DataRowToEntity(DataRow dr, CFG_RelatorioDocumentoAluno entity, bool limparEntity)
        //{
        //    return base.DataRowToEntity(dr, entity, limparEntity);
        //}

        #endregion
    }
}
