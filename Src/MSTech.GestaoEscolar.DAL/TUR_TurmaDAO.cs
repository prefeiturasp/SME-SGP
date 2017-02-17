/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL.Abstracts;
using MSTech.GestaoEscolar.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;

namespace MSTech.GestaoEscolar.DAL
{
    public class TUR_TurmaDAO : Abstract_TUR_TurmaDAO
    {
        #region Métodos

        /// <summary>
        /// Retorna todos os dados das turmas informadas.
        /// </summary>
        /// <param name="tur_id">IDs das turmas</param>
        /// <returns></returns>
        public bool VerificaAcessoControleSemestral(long tur_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_VerificaAcessoControleSemestral", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.Size = 8;
                Param.ParameterName = "@tur_id";
                Param.Value = tur_id;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                    return Convert.ToBoolean(qs.Return.Rows[0][0].ToString());

                return false;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna todos os dados das turmas informadas.
        /// </summary>
        /// <param name="tur_id">IDs das turmas</param>
        /// <returns></returns>
        public DataTable SelecionaDadosPor_Turmas
        (
            string tur_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelecionaDadosPor_Turmas", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@tur_id";
                Param.Value = tur_id;
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
        /// Retorna a capacidade das turmas e a quantidade de alunos matriculados ativos, pra
        /// cada turma.
        /// </summary>
        /// <param name="tur_ids">IDs das turmas separados por ","</param>
        /// <returns></returns>
        public DataTable SelecionaVagasMatriculadosPor_Turmas
        (
            string tur_ids
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_Select_Vagas_Matriculados", _Banco);

            #region Parâmetros

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@tur_id";
            Param.Value = tur_ids;
            qs.Parameters.Add(Param);

            #endregion Parâmetros

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna as informações do turno referente a turma.
        /// </summary>
        /// <param name="tur_id">Id da turma.</param>
        /// <returns>Informações do turno referente a turma.</returns>
        public DataTable SelecionaTurnoPorTurma
        (
            long tur_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelecionaTurnoPorTurma", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;
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
        /// Atualiza a situação das turmas do ano informado
        /// </summary>
        /// <param name="cal_ano">Ano do calendario</param>
        /// <param name="tur_situacao">Nova situação</param>
        /// <param name="escolas">Id's das escolas</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <returns>true = sucesso | false = fracasso</returns>
        public bool AtualizaSituacaoPorAno(int cal_ano, byte tur_situacao, string escolas, Guid ent_id)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_TUR_Turma_UPDATE_ApenasSituacao_PorAno", _Banco);

            #region Parametros

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cal_ano";
            Param.Size = 4;
            Param.Value = cal_ano;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tur_situacao";
            Param.Size = 1;
            Param.Value = tur_situacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@escolas";
            Param.Size = 2147483647;
            Param.Value = escolas;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@tur_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@ent_id";
            Param.Size = 16;
            Param.Value = ent_id;
            qs.Parameters.Add(Param);

            #endregion Parametros

            qs.Execute();

            qs.Parameters.Clear();

            return true;
        }

        /// <summary>
        /// Retorna os alunos matriculados em turmas normais
        /// Para efetuar matricula nas turmas eletivas do aluno
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="dis_id">ID da disciplina eletiva do aluno</param>
        /// <param name="cal_id">ID do calendário escolar</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="adm">Indica se o usuário é administrador</param>
        /// <param name="usu_id">ID do usuário logado</param>
        /// <param name="gru_id">ID do grupo do usuário logado</param>        
        public DataTable SelectBy_TurmasEletivasAluno
        (
            int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int dis_id
            , int cal_id
            , Guid ent_id
            , bool adm
            , Guid usu_id
            , Guid gru_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelectBy_TurmasEletivasAluno", _Banco);
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
                Param.ParameterName = "@dis_id";
                Param.Size = 4;
                Param.Value = dis_id;
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
        /// Retorna as turmas de acordo com os filtros informados e com a permissão do usuário,
        /// traz somente turmas do tipo informado no parâmetro.
        /// </summary>
        /// <param name="usu_id">ID do usuário - obrigatório</param>
        /// <param name="gru_id">ID do grupo</param>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="crp_id">ID do currículoPeríodo</param>        
        /// <param name="trn_id"></param>
        /// <param name="doc_id">ID do docente</param>
        /// <param name="tur_codigo">Código da turma</param>
        /// <param name="ent_id">ID da entidade - obrigatório</param>
        /// <param name="uad_idSuperior">ID da unidade superior a escola</param>
        /// <param name="tur_tipo">Tipo de turma - obrigatório</param>
        /// <param name="MostraCodigoEscola"></param>
        /// <param name="totalRecords">Total de registros afetados</param>
        /// <returns></returns>
        public DataTable SelectBy_Pesquisa_Tipo
        (
            Guid usu_id
            , Guid gru_id
            , int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , int crp_id
            , int trn_id
            , long doc_id
            , string tur_codigo
            , Guid ent_id
            , Guid uad_idSuperior
            , byte tur_tipo
            , bool MostraCodigoEscola
            , int LinhasPorPagina
            , int Pagina
            , int SortDirection
            , string SortExpression
            , out int totalRecords
        )
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelectBy_Pesquisa_Tipo", _Banco);

            #region PARAMETROS

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
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            if (cal_id > 0)
                Param.Value = cal_id;
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
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@trn_id";
            Param.Size = 4;
            if (trn_id > 0)
                Param.Value = trn_id;
            else
                Param.Value = DBNull.Value;
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
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@tur_codigo";
            Param.Size = 30;
            if (!string.IsNullOrEmpty(tur_codigo))
                Param.Value = tur_codigo;
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
            Param.ParameterName = "@uad_idSuperior";
            Param.Size = 16;
            if (uad_idSuperior != Guid.Empty)
                Param.Value = uad_idSuperior;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tur_tipo";
            Param.Size = 1;
            Param.Value = tur_tipo;
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

            if (qs.Return.Rows.Count > 0)
                dt = qs.Return;

            return dt;
        }

        /// <summary>
        /// Retorna as turmas eletivas do aluno de acordo com os filtros informados e com a permissão do usuário
        /// </summary>
        /// <param name="usu_id">ID do usuário - obrigatório</param>
        /// <param name="gru_id">ID do grupo</param>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo da turma</param>
        /// <param name="crp_id"></param>
        /// <param name="doc_id"></param>
        /// <param name="ttn_id">ID do tipo de turno</param>
        /// <param name="dis_id">ID da disciplina</param>
        /// <param name="tur_codigo">Código da turma</param>
        /// <param name="ent_id">ID da entidade - obrigatório</param>
        /// <param name="uad_idSuperior">ID da unidade superior a escola</param>
        /// <param name="MostraCodigoEscola"></param>
        /// <param name="totalRecords">Total de registros encontrados</param>        
        /// <returns></returns>
        public DataTable SelectBy_Pesquisa_TurmasEletivasAluno
        (
            Guid usu_id
            , Guid gru_id
            , int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , int crp_id
            , int doc_id
            , int ttn_id
            , int dis_id
            , string tur_codigo
            , Guid ent_id
            , Guid uad_idSuperior
            , bool MostraCodigoEscola
            , out int totalRecords
        )
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelectBy_Pesquisa_TurmasEletivasAluno", _Banco);
            try
            {
                #region PARAMETROS

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
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                if (cal_id > 0)
                    Param.Value = cal_id;
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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@doc_id";
                Param.Size = 4;
                if (doc_id > 0)
                    Param.Value = doc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@ttn_id";
                Param.Size = 4;
                if (ttn_id > 0)
                    Param.Value = ttn_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@dis_id";
                Param.Size = 4;
                if (dis_id > 0)
                    Param.Value = dis_id;
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

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

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
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@MostraCodigoEscola";
                Param.Size = 1;
                Param.Value = MostraCodigoEscola;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();
                totalRecords = qs.Return.Rows.Count;

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
        /// Retorna as turmas multisseriadas de acordo com os filtros informados e com a permissão do usuário
        /// </summary>
        /// <param name="usu_id">ID do usuário - obrigatório</param>
        /// <param name="gru_id">ID do grupo</param>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo da turma</param>
        /// <param name="crp_id"></param>
        /// <param name="doc_id"></param>
        /// <param name="ttn_id">ID do tipo de turno</param>
        /// <param name="dis_id">ID da disciplina</param>
        /// <param name="tur_codigo">Código da turma</param>
        /// <param name="ent_id">ID da entidade - obrigatório</param>
        /// <param name="uad_idSuperior">ID da unidade superior a escola</param>
        /// <param name="MostraCodigoEscola"></param>
        /// <param name="totalRecords">Total de registros encontrados</param>        
        /// <returns></returns>
        public DataTable SelectBy_Pesquisa_TurmasMultisseriadas
        (
            Guid usu_id
            , Guid gru_id
            , int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , int crp_id
            , int doc_id
            , int ttn_id
            , int dis_id
            , string tur_codigo
            , Guid ent_id
            , Guid uad_idSuperior
            , bool MostraCodigoEscola
            , out int totalRecords
        )
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelectBy_Pesquisa_TurmasMultisseriadas", _Banco);
            try
            {
                #region PARAMETROS

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
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                if (cal_id > 0)
                    Param.Value = cal_id;
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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@doc_id";
                Param.Size = 4;
                if (doc_id > 0)
                    Param.Value = doc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@ttn_id";
                Param.Size = 4;
                if (ttn_id > 0)
                    Param.Value = ttn_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@dis_id";
                Param.Size = 4;
                if (dis_id > 0)
                    Param.Value = dis_id;
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

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

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
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@MostraCodigoEscola";
                Param.Size = 1;
                Param.Value = MostraCodigoEscola;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();
                totalRecords = qs.Return.Rows.Count;

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
        /// Retorna as turmas de acordo com os filtros, podendo ser inforados ou não.
        /// Traz turmas de todos os tipos (Normal e Eletiva do aluno).
        /// </summary>
        /// <param name="usu_id">ID do usuário - obrigatório</param>
        /// <param name="gru_id">ID do grupo do usuário - obrigatório</param>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="trn_id">ID do turno</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="tur_codigo">Código da turma</param>
        /// <param name="ent_id">ID da entidade - obrigatório</param>
        /// <param name="uad_idSuperior">ID da unidade administrativa superior à escola</param>
        /// <param name="doc_id">ID do docente (quando o usuário logado for docente)</param>
        /// <param name="MostraCodigoEscola"></param>
        /// <param name="totalRecords">Total de registros retornados</param>
        /// <param name="adm"></param>
        /// <returns></returns>
        public DataTable SelectBy_Pesquisa_TodosTipos
        (
            Guid usu_id
            , Guid gru_id
            , Guid ent_id
            , Guid uad_idSuperior
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int cal_id
            , int trn_id
            , string tur_codigo
            , long doc_id
            , bool adm
            , bool MostraCodigoEscola
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelectBy_Pesquisa_TodosTipos", _Banco);

            #region PARAMETROS

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
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            if (cal_id > 0)
                Param.Value = cal_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@trn_id";
            Param.Size = 4;
            if (trn_id > 0)
                Param.Value = trn_id;
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
            Param.ParameterName = "@adm";
            Param.Size = 1;
            Param.Value = adm;
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
        /// Retorna as turmas que o docente pode dar aula ou é coordenador 
        /// de alguma disciplina da turma, de todos os tipos (Normal e Eletiva do aluno).
        /// Se for conceito global, traz as turmas apenas se estiver configurado
        /// que docentes pode efetivar notas do conceito global
        /// </summary>
        /// <param name="ent_id">ID da entidade - obrigatório</param>        
        /// <param name="doc_id">ID do docente (quando o usuário logado for docente)</param>
        /// <param name="totalRecords">Total de registros retornados</param>        
        public DataTable SelectBy_Docente_Efetivacao_TodosTipos
        (
            Guid ent_id
            , long doc_id
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelectBy_Docente_Efetivacao_TodosTipos", _Banco);

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

            #endregion PARAMETROS

            qs.Execute();
            totalRecords = qs.Return.Rows.Count;

            return qs.Return;
        }

        /// <summary>
        /// Retorna as turmas que o docente pode dar aula ou é coordenador 
        /// de alguma disciplina da turma, de todos os tipos (Normal e Eletiva do aluno).
        /// Se for conceito global, traz as turmas apenas se estiver configurado
        /// que docentes pode efetivar notas do conceito global
        /// </summary>
        /// <param name="ent_id">ID da entidade - obrigatório</param>        
        /// <param name="doc_id">ID do docente (quando o usuário logado for docente)</param>
        /// <param name="esc_id">Id da escola</param> 
        /// <param name="mostrarCodigoNome">True - Exibe o código e nome da turma | False - Exibe apenas o código da turma</param>
        /// <param name="totalRecords">Total de registros retornados</param>   
        public DataTable SelectBy_Docente_TodosTipos_Posicao
        (
            Guid ent_id
            , long doc_id
            , int tdt_posicao
            , int esc_id
            , bool mostrarCodigoNome
            , bool turmasNormais
            , out int totalRecords
        )
        {
            return SelectBy_Docente_TodosTipos_Posicao(ent_id, doc_id, tdt_posicao, esc_id, 0, mostrarCodigoNome, turmasNormais, out totalRecords);
        }

        /// <summary>
        /// Retorna as turmas que o docente pode dar aula ou é coordenador 
        /// de alguma disciplina da turma, de todos os tipos (Normal e Eletiva do aluno).
        /// Se for conceito global, traz as turmas apenas se estiver configurado
        /// que docentes pode efetivar notas do conceito global
        /// </summary>
        /// <param name="ent_id">ID da entidade - obrigatório</param>        
        /// <param name="doc_id">ID do docente (quando o usuário logado for docente)</param>
        /// <param name="esc_id">Id da escola</param> 
        /// <param name="mostrarCodigoNome">True - Exibe o código e nome da turma | False - Exibe apenas o código da turma</param>
        /// <param name="totalRecords">Total de registros retornados</param>   
        public DataTable SelectBy_Docente_TodosTipos_Posicao
        (
            Guid ent_id
            , long doc_id
            , int tdt_posicao
            , int esc_id
            , int cal_id
            , bool mostrarCodigoNome
            , bool turmasNormais
            , out int totalRecords
        )
        {
            return SelectBy_Docente_TodosTipos_Posicao(ent_id, doc_id, tdt_posicao, esc_id, cal_id, mostrarCodigoNome, turmasNormais, true, out totalRecords);
        }

        /// <summary>
        /// Retorna as turmas que o docente pode dar aula ou é coordenador 
        /// de alguma disciplina da turma, de todos os tipos (Normal e Eletiva do aluno).
        /// Se for conceito global, traz as turmas apenas se estiver configurado
        /// que docentes pode efetivar notas do conceito global
        /// </summary>
        /// <param name="ent_id">ID da entidade - obrigatório</param>        
        /// <param name="doc_id">ID do docente (quando o usuário logado for docente)</param>
        /// <param name="esc_id">Id da escola</param> 
        /// <param name="mostrarCodigoNome">True - Exibe o código e nome da turma | False - Exibe apenas o código da turma</param>
        /// <param name="totalRecords">Total de registros retornados</param>   
        public DataTable SelectBy_Docente_TodosTipos_Posicao
        (
            Guid ent_id
            , long doc_id
            , int tdt_posicao
            , int esc_id
            , int cal_id
            , bool mostrarCodigoNome
            , bool turmasNormais
            , bool mostraEletivas
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelectBy_Docente_TodosTipos_Posicao", _Banco);

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
            Param.DbType = DbType.Int16;
            Param.ParameterName = "@tdt_posicao";
            Param.Size = 8;
            Param.Value = tdt_posicao;
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
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            if (cal_id > 0)
                Param.Value = cal_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@mostrarCodigoNome";
            Param.Size = 1;
            Param.Value = mostrarCodigoNome;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@turmasNormais";
            Param.Size = 1;
            Param.Value = turmasNormais;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@mostraEletivas";
            Param.Size = 1;
            Param.Value = mostraEletivas;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();
            totalRecords = qs.Return.Rows.Count;

            return qs.Return;
        }

        /// <summary>
        /// Verifica quantos registros relacionados a turma existem.
        /// </summary>
        /// <param name="tur_id">Id da Turma</param>
        /// <returns>Retorna verdadeiro se houverem registros relacionados com a turma.</returns>
        public bool VerificaRegistrosAssociados
        (
            long tur_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_VerificaRegistrosAssociados", _Banco);

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

            #endregion PARAMETROS

            qs.Execute();

            return Convert.ToInt64(qs.Return.Rows[0]["cont"]) > 0;
        }

        /// <summary>
        /// Verifica se existe alguma turma pelo calendario, curso, serie e periodo
        /// </summary>
        /// <param name="cur_id">Id do curso.</param>
        /// <param name="crr_id">Id do currículo.</param>
        /// <param name="crp_id">Id do curr. período</param>
        /// <param name="cal_id">Id do calendario</param>
        /// <param name="tur_tipo">Tipo de turma</param>
        /// <returns>True = Encontrou a turma / False = Não encontrou.</returns>
        public bool SelectBy_VerificaExisteTurmaParametroFormacao(int cal_id, int cur_id, int crr_id, int crp_id, byte tur_tipo)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelectBy_VerificaExisteTurmaParametroFormacao", _Banco);

            #region Parâmetros

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            Param.Value = cal_id;
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
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tur_tipo";
            Param.Size = 1;
            Param.Value = tur_tipo;
            qs.Parameters.Add(Param);

            #endregion Parâmetros

            qs.Execute();

            return qs.Return.Rows.Count > 0;
        }

        /// <summary>
        /// Verifica se existem turmas do PEJA por escola e calendario.
        /// </summary>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="cal_id">Id do calendario</param>
        /// <returns>True = Encontrou turmas / False = Não encontrou.</returns>
        public bool VerificaExistenciaTurmasPejaPorEscolaCalendario(int esc_id, int cal_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_VerificaExistenciaTurmasPEJAPorEscolaCalendario", _Banco);

            #region Parâmetros

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@esc_id";
            Param.Size = 4;
            Param.Value = esc_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            Param.Value = cal_id;
            qs.Parameters.Add(Param);

            #endregion Parâmetros

            qs.Execute();

            return Convert.ToBoolean(qs.Return.Rows[0][0]);
        }

        /// <summary>
        /// Retorna turmas associadas a um turno
        /// </summary>
        /// <param name="trn_id">Id to turno para filtrar turmas</param>
        /// <returns>Tabela com todos os campos das turmas associadas</returns>
        public DataTable SelectTurma_ByTurno
        (
            int trn_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelectBY_trn_id", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.ParameterName = "@trn_id";
                Param.Value = trn_id;
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
        /// Retorna o formato de ensino da turma.
        /// </summary>
        /// <param name="tur_id">Id da turma</param>
        /// <returns></returns>
        public DataTable SelectBy_TurmaFormatoAvaliacao(long tur_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_FormatoAvaliacao_SelectBy_Turma", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;
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
        /// Verifica se a turma não está sendo utilizada na matricula turma
        /// </summary>
        /// <param name="tur_id"></param>
        /// <returns>True/False</returns>
        public bool SelectBy_VerificaMatriculaTurma
        (
            long tur_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelectBy_VerificaMatriculaTurma", _Banco);
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

                #endregion PARAMETROS

                qs.Execute();

                return Convert.ToInt32(qs.Return.Rows[0][0]) > 0;
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
        /// Retorna as turmas da escola e ano do calendário.
        /// </summary>
        /// <param name="esc_id">id da escola da turma</param>
        /// <param name="uni_id">unid administrativa da escola</param>
        /// <param name="cal_ano">ano dos calendários</param>
        /// <param name="ent_id">Entidade</param>
        /// <param name="gru_id">Grupo do usuário</param>
        /// <param name="usu_id">Usuário</param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public DataTable Selectby_EscolaAno(Guid ent_id, Guid usu_id, Guid gru_id, int esc_id, int uni_id, int cal_ano, out int totalRecords)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_Selectby_EscolaAno", _Banco);
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
                Param.ParameterName = "@cal_ano";
                Param.Size = 4;
                if (cal_ano > 0)
                    Param.Value = cal_ano;
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
        /// Retorna as turmas da escola, curso, período do curso e calendário.
        /// Somente turmas do tipo 1-Normal.
        /// </summary>
        /// <param name="usu_id">Id do usuário</param>
        /// <param name="gru_id">Id do grupo</param>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="uni_id">Id da unidade administrativa</param>
        /// <param name="cal_id">Id calendário</param>
        /// <param name="cur_id">Id do curso</param>
        /// <param name="crr_id">Id do currículo</param>
        /// <param name="crp_id">Id do currículo período</param>
        /// <param name="ent_id">Id da entidade</param>
        /// <returns></returns>
        public DataTable SelectBy_Escola_Periodo_Calendario
        (
            int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , Guid usu_id
            , Guid gru_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelectBy_Escola_Periodo_Calendario", _Banco);

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
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            Param.Value = cal_id;
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

        /// <summary>
        /// Retorna a entidade da turma passada
        /// </summary>
        /// <param name="tur_id"></param>
        /// <returns></returns>
        public Guid GetEntidadeByTurma
        (
            Int64 tur_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_GetEntidadeByTurma", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return String.IsNullOrEmpty(qs.Return.Rows[0]["ent_id"].ToString()) ? Guid.Empty : new Guid(qs.Return.Rows[0]["ent_id"].ToString());
        }

        /// <summary>
        /// Retorna todas as turmas que estão no ano e nas escolas informadas de acordo com o tipo
        /// </summary>
        /// <param name="cal_ano">Ano do calendario</param>
        /// <param name="tur_tipo">Tipos de turma</param>
        /// <param name="escolas">Id's das escolas</param>
        /// <param name="ent_id">Id da entidade</param>      
        /// <returns></returns>
        public DataTable SelectBy_AnoTipoTurma
        (
            int cal_ano
            , string tur_tipo
            , string escolas
            , Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelectBy_AnoTipoTurma", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cal_ano";
            Param.Size = 4;
            Param.Value = cal_ano;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@tur_tipo";
            Param.Size = 2147483647;
            Param.Value = tur_tipo;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@escolas";
            Param.Size = 2147483647;
            Param.Value = escolas;
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

        /// <summary>
        /// Seleciona todos as turmas filtrando por escola, curso, currículo, período e calendário.
        /// </summary>
        /// <param name="ent_id">ID da entidade.</param>
        /// <param name="esc_id">ID da escola.</param>
        /// <param name="uni_id">ID da unidade.</param>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo do curso.</param>
        /// <param name="crp_id">ID do período do curso.</param>
        /// <param name="cal_id">ID do calendário.</param>
        /// <param name="tur_situacao">Situacao da turma</param>
        public DataTable SelecionarPorEscolaCalendarioEPeriodo(Guid ent_id, int esc_id, int uni_id, int cal_id, int cur_id, int crr_id, int crp_id, byte tur_situacao)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelecionaPorEscolaCalendarioEPeriodo", _Banco);

            #region Parametros

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@ent_id";
            Param.Size = 16;
            Param.Value = ent_id;
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
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            Param.Value = cal_id;
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
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tur_situacao";
            Param.Size = 1;
            if (tur_situacao > 0)
                Param.Value = tur_situacao;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            #endregion Parametros

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna as turmas de acordo com os filtros informados. 
        /// </summary>
        /// <param name="usu_id">Id do usuário</param>
        /// <param name="gru_id">Id do grupo</param>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="uni_id">Id da unidade administrativa</param>
        /// <param name="cal_id">Id calendário</param>
        /// <param name="cur_id">Id do curso</param>
        /// <param name="crr_id">Id do currículo</param>
        /// <param name="crp_id">Id do currículo período</param>
        /// <param name="ent_id">Id da entidade</param>
        /// <param name="tur_situacao">Situação da turma</param>
        /// <param name="paginado">Flag que indica se será paginado</param>
        /// <param name="currentPage">Página atual</param>
        /// <param name="pageSize">Quantidade de itens por página</param>
        /// <param name="totalRecords">Total de registros retornados</param>
        /// <returns></returns>
        public DataTable SelectBy_Escola_Periodo_Situacao
        (
              Guid usu_id
            , Guid gru_id
            , bool adm
            , int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , byte tur_tipo
            , byte tur_situacao
            , bool mostraEletivas = false
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelectBy_Escola_Periodo_Situacao", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@usu_id";
            Param.Size = 16;
            if (!usu_id.Equals(new Guid()))
                Param.Value = usu_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@gru_id";
            Param.Size = 16;
            Param.Value = gru_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@adm";
            Param.Size = 1;
            Param.Value = adm;
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
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            if (cal_id > 0)
                Param.Value = cal_id;
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
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@ent_id";
            Param.Size = 16;
            Param.Value = ent_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tur_tipo";
            Param.Size = 1;
            if (tur_tipo > 0)
                Param.Value = tur_tipo;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tur_situacao";
            Param.Size = 1;
            if (tur_situacao > 0)
                Param.Value = tur_situacao;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@mostraEletivas";
            Param.Size = 1;
            Param.Value = mostraEletivas;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna as turmas de acordo com os filtros informados. 
        /// </summary>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="uni_id">Id da unidade</param>
        /// <param name="cal_id">Id do calendário</param>
        /// <param name="tur_situacao">Situacao da turma</param>
        /// <param name="appMinutosCacheLongo">Minutos do cache</param>
        /// <returns>Turmas</returns>
        public DataTable SelectBy_Escola_Calendario_Situacao(int esc_id, int uni_id, int cal_id, byte tur_situacao)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelectBy_Escola_Calendario_Situacao", _Banco);

            #region Parametros

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
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            Param.Value = cal_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tur_situacao";
            Param.Size = 1;
            Param.Value = tur_situacao;
            qs.Parameters.Add(Param);

            #endregion Parametros

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna as turmas normais (tur_tipo = 1) de acordo com os filtros informados. 
        /// </summary>
        /// <param name="usu_id">Id do usuário</param>
        /// <param name="gru_id">Id do grupo</param>
        /// <param name="adm">Informa se é usuário de visão administrador</param>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="uni_id">Id da unidade administrativa</param>
        /// <param name="cal_id">Id calendário</param>
        /// <param name="cur_id">Id do curso</param>
        /// <param name="crr_id">Id do currículo</param>
        /// <param name="crp_id">Id do currículo período</param>
        /// <param name="ent_id">Id da entidade</param>
        /// <returns></returns>
        public DataTable SelectBy_Escola_Periodo_TurmasNormais
        (
              Guid usu_id
            , Guid gru_id
            , bool adm
            , int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelectBy_Escola_Periodo_TurmasNormais", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@usu_id";
            Param.Size = 16;
            if (!usu_id.Equals(new Guid()))
                Param.Value = usu_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@gru_id";
            Param.Size = 16;
            Param.Value = gru_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@adm";
            Param.Size = 1;
            Param.Value = adm;
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
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            if (cal_id > 0)
                Param.Value = cal_id;
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
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@ent_id";
            Param.Size = 16;
            Param.Value = ent_id;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna todas as turmas ativas e que seu calendario anual exista no MomentoAno    
        /// Utilizado no UserControl de Movimentação e na tela de Solicitação de Transferência
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do curso do curriculo</param>
        /// <param name="crp_id">ID do curriculo do periodo</param>
        /// <param name="ent_id">ID da entidade do usuário logado</param>
        /// <param name="usu_id">ID do usuário logado</param>
        /// <param name="gru_id">ID do grupo do usuário logado</param>  
        /// <param name="ttn_id">ID do tipo de turno</param>
        public DataTable SelectBy_Escola_Periodo_MomentoAno
        (
            int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , Guid usu_id
            , Guid gru_id
            , int ttn_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelectBy_Escola_Periodo_MomentoAno", _Banco);

            #region PARAMETROS

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
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            if (cal_id > 0)
                Param.Value = cal_id;
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
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@ent_id";
            Param.Size = 16;
            Param.Value = ent_id;
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
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ttn_id";
            Param.Size = 4;
            if (ttn_id > 0)
                Param.Value = ttn_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna todas as turmas ativas e que seu calendario anual exista no MomentoAno    
        /// Utilizado no UserControl de Movimentação e na tela de Solicitação de Transferência
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do curso do curriculo</param>
        /// <param name="crp_id">ID do curriculo do periodo</param>
        /// <param name="ent_id">ID da entidade do usuário logado</param>
        /// <param name="usu_id">ID do usuário logado</param>
        /// <param name="gru_id">ID do grupo do usuário logado</param>  
        /// <param name="cal_id">ID do calendário</param>
        public DataTable SelecionaTurmasCursoEquivalentes
        (
            int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , Guid usu_id
            , Guid gru_id
            , int cal_id = 0
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelecionaTurmasCursoEquivalentes", _Banco);

            #region PARAMETROS

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
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@ent_id";
            Param.Size = 16;
            Param.Value = ent_id;
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
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            if (cal_id > 0)
                Param.Value = cal_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna todas as turmas ativas e que seu calendario anual exista no MomentoAno
        /// e tenha o número da avaliação do currículo  
        /// Utilizado no UserControl de Movimentação e na tela de Solicitação de Transferência
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do curso do curriculo</param>
        /// <param name="crp_id">ID do curriculo do periodo</param>
        /// <param name="tca_numeroAvaliacao">Número da avaliação</param>
        /// <param name="ent_id">ID da entidade do usuário logado</param>
        /// <param name="usu_id">ID do usuário logado</param>
        /// <param name="gru_id">ID do grupo do usuário logado</param>  
        public DataTable SelectBy_Escola_Periodo_MomentoAno_Avaliacao
        (
            int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , int crp_id
            , int tca_numeroAvaliacao
            , Guid ent_id
            , Guid usu_id
            , Guid gru_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelectBy_Escola_Periodo_MomentoAno_Avaliacao", _Banco);

            #region PARAMETROS

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
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            if (cal_id > 0)
                Param.Value = cal_id;
            else
                Param.Value = DBNull.Value;
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
            Param.ParameterName = "@tca_numeroAvaliacao";
            Param.Size = 4;
            Param.Value = tca_numeroAvaliacao;
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

        /// <summary>
        /// Retorna todas as turmas ativas (turmas de cursos equivalentes e que seu calendario anual exista no MomentoAno
        /// e tenha o número da avaliação do currículo  
        /// Utilizado no UserControl de Movimentação e na tela de Solicitação de Transferência
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do curso do curriculo</param>
        /// <param name="crp_id">ID do curriculo do periodo</param>
        /// <param name="tca_numeroAvaliacao">Número da avaliação</param>
        /// <param name="ent_id">ID da entidade do usuário logado</param>
        /// <param name="usu_id">ID do usuário logado</param>
        /// <param name="gru_id">ID do grupo do usuário logado</param>  
        public DataTable SelecionaTurmasCursosEquivalentesAvaliacao
        (
            int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , int tca_numeroAvaliacao
            , Guid ent_id
            , Guid usu_id
            , Guid gru_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelecionaTurmasCursosEquivalentes_Avaliacao", _Banco);

            #region PARAMETROS

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
            Param.ParameterName = "@tca_numeroAvaliacao";
            Param.Size = 4;
            Param.Value = tca_numeroAvaliacao;
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

        /// <summary>
        /// Busca turmas da escola, ano, curso e período informados.
        /// Considera os cursos equivalentes.
        /// Traz somente turmas do tipo 1-Normal.
        /// Somente turmas ativas.
        /// </summary>
        /// <param name="usu_id">ID do usuário logado</param>
        /// <param name="gru_id">ID do grupo do usuário logado</param>
        /// <param name="ent_id">ID da entidade do usuário logado</param>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        /// <param name="crp_id">ID do período</param>
        /// <param name="cal_ano">Ano do calendário da turma</param>
        /// <returns></returns>
        public DataTable SelecionaPor_Escola_Calendario_CursoPeriodo_Equivalentes
        (
             Guid usu_id
            , Guid gru_id
            , Guid ent_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , int cal_ano
        )
        {
            QuerySelectStoredProcedure qs =
                new QuerySelectStoredProcedure("NEW_TUR_Turma_SelectBy_Escola_Calendario_CursoPeriodo_Equivalentes",
                                               _Banco);

            #region PARAMETROS

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
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cal_ano";
            Param.Size = 4;
            Param.Value = cal_ano;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Faz busca das turmas. Traz somente turmas do tipo 1-Normal.
        /// </summary>
        /// <param name="usu_id">Id do usuário</param>
        /// <param name="gru_id">Id do grupo</param>
        /// <param name="tur_id">Id da turma</param>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="uni_id">Id da unidade administrativa</param>
        /// <param name="cal_id">Id calendário</param>
        /// <param name="cur_id">Id do curso</param>
        /// <param name="crr_id">Id do currículo</param>
        /// <param name="crp_id">Id do currículo período</param>
        /// <param name="cal_ano">Ano do calendário da turma</param>
        /// <param name="ent_id">Id da entidade</param>
        /// <param name="tur_situacao">Situação da turma</param>
        /// <returns>Retorna as turmas ativas confome os filtros.</returns>
        public DataTable RetornaTurmasCalendario
        (
             Guid usu_id
            , Guid gru_id
            , long tur_id
            , int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , int crp_id
            , int cal_ano
            , Guid ent_id
            , byte tur_situacao
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelectBy_Pesquisa_AnoCalendario", _Banco);
            try
            {
                #region PARAMETROS

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
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                if (cal_id > 0)
                    Param.Value = cal_id;
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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tur_situacao";
                Param.Size = 1;
                if (tur_situacao > 0)
                    Param.Value = tur_situacao;
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
        /// Faz busca das turmas. Traz somente turmas do tipo 1-Normal.
        /// </summary>
        /// <param name="usu_id">id do usuario</param>
        /// <param name="gru_id">id do grupo</param>
        /// <param name="tur_id">id da turma</param>
        /// <param name="esc_id">id da escola</param>
        /// <param name="uni_id">id da unidade administrativa</param>
        /// <param name="cal_id">id calendário</param>
        /// <param name="cur_id">id do curso</param>
        /// <param name="crr_id"></param>
        /// <param name="crp_id">id do curriculo período</param>
        /// <param name="ent_id">id da entidade</param>
        /// <param name="tur_situacao"></param>
        /// <returns>retorna as turmas ativas</returns>
        public DataTable RetornaTurmas
        (
              Guid usu_id
            , Guid gru_id
            , long tur_id
            , int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , byte tur_situacao
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelectBy_Turmas", _Banco);
            try
            {
                #region PARAMETROS

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
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                if (cal_id > 0)
                    Param.Value = cal_id;
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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tur_situacao";
                Param.Size = 1;
                if (tur_situacao > 0)
                    Param.Value = tur_situacao;
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
        /// Retorna tabela com dados da turma selecionada
        /// </summary>
        /// <param name="tur_id"></param>
        /// <param name="esc_id">ID de escola</param>
        /// <param name="uni_id">ID de unidade</param>
        /// <param name="cal_id">ID de calendario</param>
        /// <param name="tur_codigo">Codigo da Turma</param>
        /// <returns>True - caso encontre algum registro no select/False - caso não encontre nada no select</returns>
        public DataTable SelectBy_Codigo
        (
            long tur_id
            , int esc_id
            , int uni_id
            , int cal_id
            , string tur_codigo
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelectBy_Codigo", _Banco);
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
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                if (cal_id > 0)
                    Param.Value = cal_id;
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
        /// Retorna tabela com dados da turma selecionada
        /// </summary>
        /// <param name="tur_id"></param>
        /// <param name="esc_id">ID de escola</param>
        /// <param name="uni_id">ID de unidade</param>
        /// <param name="cal_id">ID de calendario</param>
        /// <param name="tur_codigo">Codigo da Turma</param>
        /// <returns>True - caso encontre algum registro no select/False - caso não encontre nada no select</returns>
        public bool SelectBy_Codigo (TUR_Turma entity)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelectBy_Codigo", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                if (entity.tur_id > 0)
                    Param.Value = entity.tur_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                if (entity.esc_id > 0)
                    Param.Value = entity.esc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@uni_id";
                Param.Size = 4;
                if (entity.uni_id > 0)
                    Param.Value = entity.uni_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                if (entity.cal_id > 0)
                    Param.Value = entity.cal_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tur_codigo";
                Param.Size = 30;
                if (!string.IsNullOrEmpty(entity.tur_codigo))
                    Param.Value = entity.tur_codigo;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                if (qs.Return.Rows.Count >= 1)
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
        /// Verifica se existe um mesmo tur_codigo ligado à escola e ao calendário selecionados
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="tur_codigo">Código da turma</param>
        /// <returns>True - Existe / False - Não existe</returns>
        public bool VerificaPrefixoCodigoPorEscolaCalendario
        (
            int esc_id
            , int uni_id
            , int cal_id
            , string tur_codigo
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_VerificaPrefixoCodigoPorEscolaCalendario", _Banco);
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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                Param.Value = cal_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tur_codigo";
                Param.Size = 30;
                Param.Value = tur_codigo;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

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
        /// Retorna o último código de turma cadastrada de acordo com os parâmetros
        /// </summary>        
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cal_id">ID do calendario</param>          
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do curriculo do curso</param>
        /// <param name="crp_id">ID do período do currpiculo</param>
        /// <param name="tur_codigoPrefixo">Prefixo do código da turma</param>
        public string SelectBy_PrefixoCodigo
        (
            int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , int crp_id
            , string tur_codigoPrefixo
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelectBy_PrefixoCodigo", _Banco);
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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                Param.Value = cal_id;
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
                Param.ParameterName = "@tur_codigoPrefixo";
                Param.Size = 10;
                if (!string.IsNullOrEmpty(tur_codigoPrefixo))
                    Param.Value = tur_codigoPrefixo;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                return qs.Return.Rows.Count > 0 ? qs.Return.Rows[0]["tur_codigo"].ToString() : string.Empty;
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
        /// Retorna o último código de turma eletiva cadastrada de acordo com os parâmetros
        /// </summary>        
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cal_id">ID do calendario</param>          
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do curriculo do curso</param>
        /// <param name="tur_codigoTurma">Código da turma</param>
        public string SelectBy_PrefixoCodigoTurmaEletiva
        (
            int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , string tur_codigoTurma
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelectBy_PrefixoCodigoTurmaEletiva", _Banco);
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
                Param.ParameterName = "@uni_id";
                Param.Size = 4;
                Param.Value = uni_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                Param.Value = cal_id;
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
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tur_codigoTurma";
                Param.Size = 30;
                if (!string.IsNullOrEmpty(tur_codigoTurma))
                    Param.Value = tur_codigoTurma;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

                qs.Execute();

                return qs.Return.Rows.Count > 0 ? qs.Return.Rows[0]["tur_codigo"].ToString() : string.Empty;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        ///  Seleciona a turma pelo código, escola, unidade, currículo, curso e período,
        ///  e preenche a entidade turma.
        /// </summary>
        /// <param name="entity">Entidade TUR_Turma.</param>
        /// <param name="cur_id">Id do curso.</param>
        /// <param name="crr_id">Id do currículo.</param>
        /// <param name="crp_id">Id do curr. período.</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <returns></returns>
        public bool SelectBy_TurmaCurriculo
        (
            TUR_Turma entity
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelectBy_TurmaCurriculo", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@uni_id";
                Param.Size = 4;
                Param.Value = entity.uni_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                Param.Value = entity.esc_id;
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
                Param.DbType = DbType.String;
                Param.ParameterName = "@tur_codigo";
                Param.Size = 30;
                Param.Value = entity.tur_codigo;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                if (qs.Return.Rows.Count == 1)
                {
                    DataRowToEntity(qs.Return.Rows[0], entity, false);
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
        /// Seleciona dados da turma pelo id 
        /// </summary>
        /// <param name="tur_id">id unico da turma</param>
        /// <param name="MostraCodigoEscola"></param>
        /// <returns>tabela com dados da turma seleciona</returns>
        public DataTable SelectBy_tur_id(long tur_id, bool MostraCodigoEscola)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_Selectby_tur_id", _Banco);
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
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@MostraCodigoEscola";
                Param.Size = 1;
                Param.Value = MostraCodigoEscola;
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
        /// Retorna as turmas da escola, curso, período do curso e calendário. 
        /// Traz somente turmas do tipo 1-Normal, e com fav_tipoLancamentoFrequencia = 3 ou 4.
        /// </summary>
        /// <param name="esc_id">ID da escola.</param>
        /// <param name="uni_id">ID da unidade.</param>
        /// <param name="cal_id">ID do calendário.</param>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo.</param>
        /// <param name="crp_id">ID do currículo período.</param>
        /// <param name="ent_id">ID da entidade.</param>
        /// <param name="gru_id">ID do grupo.</param>
        /// <param name="usu_id">ID do usuário.</param>
        /// <returns>DataTable de turmas.</returns>
        public DataTable SelectByEscolaPeriodoCalendarioComFrequenciaMensal
        (
            int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , Guid gru_id
            , Guid usu_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelectByEscolaPeriodoCalendarioComFrequenciaMensal", _Banco);

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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                Param.Value = cal_id;
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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@gru_id";
                Param.Size = 16;
                Param.Value = gru_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@usu_id";
                Param.Size = 16;
                Param.Value = usu_id;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();
                return (qs.Return);
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
        /// Atualiza a situação da turma de acordo com o fechamento de matrícula
        /// </summary>                
        public bool Update_Situacao_FechamentoMatricula
        (
            TUR_Turma entity
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_Update_Situacao_FechamentoMatricula", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = entity.tur_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tur_situacao";
            Param.Size = 1;
            Param.Value = entity.tur_situacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@tur_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return true;
        }

        /// <summary>
        /// Verifica se já existe turma eletiva cadastrada com o mesmo codigo de turma
        /// </summary>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="uni_id">Id da unidade administrativa</param>
        /// <param name="cal_id">Id calendário</param>
        /// <param name="cur_id">Id do curso</param>
        /// <param name="crr_id">Id do currículo</param>
        /// <param name="tur_codigoTurma">código da turma</param>
        public DataTable VerificaExisteTurmaEletiva
        (
             int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , string tur_codigoTurma
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_VerificaExisteTurmaEletiva", _Banco);
            try
            {
                #region PARAMETROS

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
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                if (cal_id > 0)
                    Param.Value = cal_id;
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
                Param.ParameterName = "@tur_codigoTurma";
                Param.Size = 30;
                if (!string.IsNullOrEmpty(tur_codigoTurma))
                    Param.Value = tur_codigoTurma;
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
        /// Retorna todas as turmas que não fecharam o COC.
        /// </summary>
        /// <param name="esc_id">ID da escola da turma</param>
        /// <param name="uni_id">ID da unid administrativa da escola</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cap_id">ID do período do calendário</param>
        /// <returns></returns>
        public DataTable SelecionaTurmasSemFechamentoBimestre(int esc_id, int uni_id, int cal_id, int cap_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelecionaTurmasSemFechamentoCOC", _Banco);
            qs.TimeOut = 0;
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
        /// Retorna todas as turmas que não fecharam o COC para avaliação recuperação final
        /// </summary>
        /// <param name="esc_id">ID da escola da turma</param>
        /// <param name="uni_id">ID da unid administrativa da escola</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cap_id">ID do período do calendário</param>
        /// <returns></returns>
        public DataTable SelecionaTurmasSemFechamentoRecuperacaoFinal(int esc_id, int uni_id, int cal_id, int cap_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelecionaTurmasSemFechamentoCOCRecuperacaoFinal", _Banco);

            qs.TimeOut = 240;

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
        /// Verifica se existe uma turma para escola que tenha avaliação recuperação final
        /// </summary>
        /// <param name="esc_id">ID da escola da turma</param>
        /// <param name="uni_id">ID da unid administrativa da escola</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cap_id">ID do período do calendário</param>
        /// <returns></returns>
        public Boolean VerificaExisteRecuperacaoFinal(int esc_id, int uni_id, int cal_id, int cap_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_VerificaExisteRecuperacaoFinal", _Banco);
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

                #endregion PARAMETROS

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
        /// Retorna as turmas que nunca foi salvo a frequencia de algum
        /// aluno na reunião de pais.
        /// </summary>
        /// <param name="esc_id">ID da escola da turma</param>
        /// <param name="uni_id">ID da unid administrativa da escola</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cap_id">ID do período do calendário</param>
        /// <returns></returns>
        public DataTable SelecionaTurmasSemFrequenciaReuniaoPais(int esc_id, int uni_id, int cal_id, int cap_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelecionaTurmasSemFrequenciaReuniaoPais", _Banco);
            qs.TimeOut = 0;
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
        /// Retorna todas as turmas ativas e que seu calendario anual exista no MomentoAno    
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do curso do curriculo</param>
        /// <param name="crp_id">ID do curriculo do periodo</param>
        /// <param name="ent_id">ID da entidade do usuário logado</param> 
        /// <param name="gru_id">Id do grupo do usuário.</param>
        /// <param name="usu_id">Id do usuário.</param>
        /// <returns></returns>
        public DataTable SelecionaPorEscolaPeriodoMomentoAnoAcertoSituacao
        (
            int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , Guid usu_id
            , Guid gru_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelectBy_Escola_Periodo_MomentoAno_AcertoSituacao", _Banco);

            #region PARAMETROS

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
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            if (cal_id > 0)
                Param.Value = cal_id;
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
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@ent_id";
            Param.Size = 16;
            Param.Value = ent_id;
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

        /// <summary>
        /// Retorna todas as turmas ativas e que seu calendario anual exista no MomentoAno
        /// e tenha o número da avaliação do currículo  
        /// Utilizado no UserControl de Movimentação e na tela de Solicitação de Transferência
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do curso do curriculo</param>
        /// <param name="crp_id">ID do curriculo do periodo</param>
        /// <param name="tca_numeroAvaliacao">Número da avaliação</param>
        /// <param name="ent_id">ID da entidade do usuário logado</param>
        /// <param name="usu_id">ID do usuário logado</param>
        /// <param name="gru_id">ID do grupo do usuário logado</param>  
        /// <returns></returns>
        public DataTable SelecionaPorEscolaPeriodoMomentoAnoAvaliacaoAcertoSituacao
        (
            int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , int crp_id
            , int tca_numeroAvaliacao
            , Guid ent_id
            , Guid usu_id
            , Guid gru_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelectBy_Escola_Periodo_MomentoAno_Avaliacao_AcertoSituacao", _Banco);

            #region PARAMETROS

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
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            if (cal_id > 0)
                Param.Value = cal_id;
            else
                Param.Value = DBNull.Value;
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
            Param.ParameterName = "@tca_numeroAvaliacao";
            Param.Size = 4;
            Param.Value = tca_numeroAvaliacao;
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

        /// <summary>
        /// Busca as turmas de SAAI – Sala de apoio e acompanhamento a inclusão e sala especial para os parametros passados. 
        /// OBS: se o parametro doc_id for passado buscará as turmas daquele docente
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade</param>
        /// <param name="cal_id">ID do colendario</param>
        /// <param name="doc_id">ID do docente</param>
        /// <returns>Retorna uma lista com as turmas e turmas de sala especial com base nos parametros passados</returns>
        public DataTable SelecionaTurmaSalaRecurso(int esc_id, int uni_id, int cal_id, long doc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelectBy_Turma_TurmaSalaRecurso", _Banco);

            #region Parametros

            qs.Parameters.Clear();

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
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            if (cal_id > 0)
                Param.Value = cal_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@doc_id";
            Param.Size = 16;
            if (doc_id > 0)
                Param.Value = doc_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            #endregion Parametros

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Seleciona as turmas ativas para configuração da matriz curricular de um curso.
        /// </summary>
        /// <param name="esc_id">ID da escola.</param>
        /// <param name="uni_id">ID da unidade de escola.</param>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo.</param>
        /// <param name="cal_ano">Ano do calendário.</param>
        /// <param name="ent_id">ID da entidade do usuário logado.</param>
        /// <returns></returns>
        public DataTable SelecionaTurmasMatrizCurricular(int esc_id, int uni_id, int cur_id, int crr_id, int cal_ano, Guid ent_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelecionaTurmasMatrizCurricular", _Banco);

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
                Param.ParameterName = "@cal_ano";
                Param.Size = 4;
                Param.Value = cal_ano;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 20;
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
        /// Lista as turmas por escola
        /// </summary>
        /// <param name="esc_id">Id da escola</param>      
        /// <returns>Lista de turmas</returns>
        public DataTable BuscaTurmasAPI(Int64 tur_id, Int32 esc_id, Int64 doc_id, Int64 tud_id, DateTime dataBase)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_BuscaTurmaPorEscola", _Banco);
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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                if (esc_id > 0)
                    Param.Value = esc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@doc_id";
                Param.Size = 8;
                Param.Value = doc_id;
                if (doc_id > 0)
                    Param.Value = doc_id;
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
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@dataBase";
                Param.Size = 16;
                if(dataBase == DateTime.MinValue)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = dataBase;
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
        /// Lista as turmas por escola, calendario, curso, curriculo e periodo
        /// </summary>
        /// <param name="esc_id"></param>
        /// <param name="cal_id"></param>
        /// <param name="cur_id"></param>
        /// <param name="crr_id"></param>
        /// <param name="crp_id"></param>
        /// <returns></returns>
        public List<TUR_Turma> BuscaTurmasPorEscolaCalendarioCursoCurriculoPeriodo
        (
            int esc_id
          , int cal_id
          , int cur_id
          , int crr_id
          , int crp_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_BuscaTurmasPorEscolaCalendarioCursoCurriculoPeriodo", _Banco);
            try
            {
                List<TUR_Turma> lst = new List<TUR_Turma>();

                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                Param.Value = esc_id;
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



                #endregion PARAMETROS

                qs.Execute();

                foreach (DataRow dr in qs.Return.Rows)
                {
                    TUR_Turma entity = new TUR_Turma();
                    entity = DataRowToEntity(dr, entity);
                    lst.Add(entity);
                }

                return lst;
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
        /// Seleciona a quantidade de tempos por bimestre.
        /// Apenas para cursos que possuem efetivação semestral.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <returns>Retorna datatable com os tempos por bimestre</returns>
        public DataTable SelecionaTemposDiaEfetivacaoSemestral(long tur_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelecionaTemposAulaEfetivacaoSemestral", _Banco);
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
        /// Traz as turmas que o docente pode dar aula ou é coordenador 
        ///	de alguma disciplina da turma, de todos os tipos (Normal e Eletiva do aluno).
        ///	Se for conceito global, traz as turmas apenas se estiver configurado
        ///	que docentes pode efetivar notas do conceito global
        ///	Usada na tela de controle de turmas.
        /// </summary>
        /// <param name="ent_id">ID da entidade do usuário logado.</param>
        /// <param name="doc_id">ID do docente.</param>
        /// <returns></returns>
        public DataTable SelecionaPorDocenteControleTurma(Guid ent_id, long doc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelectPorDocenteControleTurma", _Banco);

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
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@doc_id";
                Param.Size = 8;
                Param.Value = doc_id;
                qs.Parameters.Add(Param);

                qs.Execute();

                return qs.Return;

                #endregion Parâmetros
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Verifica se a turma possui uma disciplina de determinado tipo.
        /// </summary>
        /// <param name="tur_id">ID da turma.</param>
        /// <param name="tud_tipo">Tipo de disciplina a ser buscado.</param>
        /// <returns></returns>
        public bool VerificaPossuiDisciplinaPorTipo(long tur_id, byte tud_tipo)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_TUR_Turma_VerificaPossuiDisciplinaPorTipo", _Banco);

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
                Param.ParameterName = "@tud_tipo";
                Param.Size = 1;
                Param.Value = tud_tipo;
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
        /// Seleciona minhas turmas
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="crp_id">ID do currículoPeríodo</param>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="tur_codigo">Código da turma</param>
        /// <param name="tci_id">ID do tipo de ciclo</param>        
        /// <returns></returns>
        public DataTable SelecionaPorFiltrosMinhasTurmas(
            int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , string tur_codigo
            , int tci_id
        )
        {
            return SelecionaPorFiltrosMinhasTurmas(esc_id, uni_id, 0, cur_id, crr_id, crp_id, ent_id, tur_codigo, tci_id);
        }

        /// <summary>
        /// Seleciona minhas turmas
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="crp_id">ID do currículoPeríodo</param>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="tur_codigo">Código da turma</param>
        /// <param name="tci_id">ID do tipo de ciclo</param>        
        /// <returns></returns>
        public DataTable SelecionaPorFiltrosMinhasTurmas(
            int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , string tur_codigo
            , int tci_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelectByFiltrosMinhasTurmas", _Banco);

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
                Param.ParameterName = "@uni_id";
                Param.Size = 4;
                Param.Value = uni_id;
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
                Param.DbType = DbType.String;
                Param.ParameterName = "@tur_codigo";
                Param.Size = 30;
                if (!string.IsNullOrEmpty(tur_codigo))
                    Param.Value = tur_codigo;
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
                Param.ParameterName = "@tci_id";
                Param.Size = 4;
                if (tci_id > 0)
                    Param.Value = tci_id;
                else
                    Param.Value = DBNull.Value;
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
        /// Seleciona o histórico das turmas do docente
        /// </summary>
        /// <param name="esc_id"></param>
        /// <param name="doc_id"></param>
        /// <returns></returns>
        public DataTable SelecionaHistoricoPorDocenteControleTurma(int esc_id, long doc_id, Guid ent_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelectHistoricoPorDocenteControleTurma", _Banco);

            try
            {
                #region Parâmetros

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
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@doc_id";
                Param.Size = 8;
                Param.Value = doc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                qs.Execute();

                return qs.Return;

                #endregion Parâmetros
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        public DataTable SelecionaMinhasTurmasComboPorTurId(long tur_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelectMinhasTurmasPorTurId", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;
                qs.Parameters.Add(Param);

                qs.Execute();

                return qs.Return;

                #endregion Parâmetros
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Traz as turmas do usuário Gestor.
        /// Usada na tela de Minha Escola - Perfil Gestor.
        /// </summary>
        /// <param name="ent_id">ID da entidade do usuário logado.</param>
        /// <param name="uad_id">ID da unidade do usuário Gestor.</param>
        /// <returns></returns>
        public DataTable SelecionaPorUnidadeGestorMinhaEscola(Guid ent_id, int esc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelectPorUnidadeGestor", _Banco);

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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                Param.Value = esc_id;
                qs.Parameters.Add(Param);

                qs.Execute();

                return qs.Return;

                #endregion Parâmetros
            }
            finally
            {
                qs.Parameters.Clear();
            }

        }

        /// <summary>
        /// Seleciona as turmas por escola e grupamento e as turmas dos grupamentos equivalentes.
        /// </summary>
        /// <param name="usu_id">ID do usuário.</param>
        /// <param name="gru_id">ID do grupo do usuário.</param>
        /// <param name="adm">Flag que indica se o usuário tem permissão de administrador.</param>
        /// <param name="esc_id">ID da escola.</param>
        /// <param name="uni_id">ID da unidade de escola.</param>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo do curso.</param>
        /// <param name="crp_id">ID do grupamento.</param>
        /// <param name="ent_id">ID da entidade do usuário.</param>
        /// <param name="tur_situacao">Situação das turmas.</param>
        /// <returns></returns>
        public DataTable SelecionaPorEscolaPeriodoSituacaoEquivalentes
        (
             Guid usu_id
            , Guid gru_id
            , bool adm
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , byte tur_situacao
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelectPorEscolaPeriodoSituacaoEquivalentes", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@usu_id";
            Param.Size = 16;
            if (!usu_id.Equals(new Guid()))
                Param.Value = usu_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@gru_id";
            Param.Size = 16;
            Param.Value = gru_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@adm";
            Param.Size = 1;
            Param.Value = adm;
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
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tur_situacao";
            Param.Size = 1;
            if (tur_situacao > 0)
                Param.Value = tur_situacao;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Seleciona os dados da turma da tela de VisualizaConteudo
        /// </summary>
        /// <param name="parametro1">Nome OU código da escola</param>
        /// <param name="parametro2">Ano letivo</param>
        /// <param name="parametro3">Código da turma</param>
        /// <returns>Retorna dados da turma</returns>
        public DataSet SelecionaVisualizaConteudo(string parametro1, string parametro2, string parametro3)
        {
            //Grava em DataSet pois retorna vários selects
            DataSet dsRetorno = new DataSet();

            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlConnection con = new SqlConnection(_Banco.GetConnection.ConnectionString);

            adapter.SelectCommand = new SqlCommand("NEW_TUR_Turma_VisualizaConteudo", con);
            adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

            adapter.SelectCommand.Parameters.Add("parametro1", SqlDbType.VarChar, 200);
            adapter.SelectCommand.Parameters["parametro1"].Value = parametro1;

            adapter.SelectCommand.Parameters.Add("parametro2", SqlDbType.VarChar, 4);
            adapter.SelectCommand.Parameters["parametro2"].Value = parametro2;

            adapter.SelectCommand.Parameters.Add("parametro3", SqlDbType.VarChar, 50);
            adapter.SelectCommand.Parameters["parametro3"].Value = parametro3;

            adapter.Fill(dsRetorno);

            return dsRetorno;
        }

        /// <summary>
        /// Seleciona as turmas de origem dos alunos matriculados na turma multisseriada.
        /// </summary>
        /// <param name="tur_idMultisseriada">ID da turma multisseriada.</param>
        /// <param name="tud_tipoMultisseriadaDocente">Tipo de disciplina multisseriada do docente.</param>
        /// <param name="tud_tipoMultisseriadaAluno">Tipo de disciplina multisseriada do aluno.</param>
        /// <returns></returns>
        public DataTable SelecionaTurmasNormaisMatriculaMutisseriada(long tur_idMultisseriada, byte tud_tipoMultisseriadaDocente, byte tud_tipoMultisseriadaAluno)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelecionaTurmasNormaisMatriculaMutisseriada", _Banco);

            try
            {
                #region Parametros

                Param = qs.NewParameter();
                Param.ParameterName = "@tur_idMultisseriada";
                Param.DbType = DbType.Int64;
                Param.Size = 8;
                Param.Value = tur_idMultisseriada;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@tud_tipoMultisseriadaDocente";
                Param.DbType = DbType.Byte;
                Param.Size = 1;
                Param.Value = tud_tipoMultisseriadaDocente;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@tud_tipoMultisseriadaAluno";
                Param.DbType = DbType.Byte;
                Param.Size = 1;
                Param.Value = tud_tipoMultisseriadaAluno;
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
        /// Seleciona turmas por escola, curso, período e calendário mínimo.
        /// </summary>
        /// <param name="esc_id">ID da escola.</param>
        /// <param name="uni_id">ID da unidade de escola.</param>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo do curso.</param>
        /// <param name="crp_id">ID do período do currículo.</param>
        /// <param name="cal_ano">Ano limite mínimo.</param>
        /// <returns></returns>
        public DataTable SelecionaPorEscolaCursoPeriodoCalendarioMinimo(int esc_id, int uni_id, int cur_id, int crr_id, int crp_id, int cal_ano)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelecionaPorEscolaCursoPeriodoCalendarioMinimo", _Banco);

            #region Parâmetros

            Param = qs.NewParameter();
            Param.ParameterName = "@esc_id";
            Param.DbType = DbType.Int32;
            Param.Size = 4;
            Param.Value = esc_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.ParameterName = "@uni_id";
            Param.DbType = DbType.Int32;
            Param.Size = 4;
            Param.Value = uni_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.ParameterName = "@cur_id";
            Param.DbType = DbType.Int32;
            Param.Size = 4;
            Param.Value = cur_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.ParameterName = "@crr_id";
            Param.DbType = DbType.Int32;
            Param.Size = 4;
            Param.Value = crr_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.ParameterName = "@crp_id";
            Param.DbType = DbType.Int32;
            Param.Size = 4;
            Param.Value = crp_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.ParameterName = "@cal_ano";
            Param.DbType = DbType.Int32;
            Param.Size = 4;
            Param.Value = cal_ano;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        #endregion Métodos

        #region Métodos RS

        /// <summary>
        /// Retorna as turmas de acordo com os filtros informados e com a permissão do usuário,
        /// traz somente turmas do tipo informado no parâmetro.
        /// </summary>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="usu_id">ID do usuário</param>
        /// <param name="gru_id">ID do grupo</param>
        /// <param name="uad_idSuperior">ID da unidade superior a escola</param>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="crp_id">ID do currículoPeríodo</param>
        /// <param name="trn_id">ID do turno</param>
        /// <param name="doc_id">ID do docente</param>
        /// <param name="tur_codigo">Código da turma</param>
        /// <param name="tur_tipo">Tipo de turma</param>
        /// <param name="paginado">Indica se será paginado</param>
        /// <param name="currentPage">Página atual</param>
        /// <param name="pageSize">Quantidade de registros por página</param>
        /// <param name="totalRecords">Total de registros retornado</param>
        /// <returns>Retorna turmas ativA</returns>
        public DataTable SelectBy_Tipo(Guid ent_id, Guid usu_id, Guid gru_id, Guid uad_idSuperior, int esc_id, int uni_id, int cal_id, int cur_id, int crr_id, int crp_id, int trn_id, long doc_id, string tur_codigo, byte tur_tipo, bool paginado, int currentPage, int pageSize, out int totalRecords)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_Turma_SelectBy_Tipo", _Banco);

            #region Parâmetros

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
            Param.Value = usu_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@gru_id";
            Param.Size = 16;
            Param.Value = gru_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tur_tipo";
            Param.Size = 1;
            Param.Value = tur_tipo;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@uad_idSuperior";
            Param.Size = 16;
            if (uad_idSuperior != Guid.Empty) Param.Value = uad_idSuperior;
            else Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@esc_id";
            Param.Size = 4;
            if (esc_id > 0) Param.Value = esc_id;
            else Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@uni_id";
            Param.Size = 4;
            if (uni_id > 0) Param.Value = uni_id;
            else Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            if (cal_id > 0) Param.Value = cal_id;
            else Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cur_id";
            Param.Size = 4;
            if (cur_id > 0) Param.Value = cur_id;
            else Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@crr_id";
            Param.Size = 4;
            if (crr_id > 0) Param.Value = crr_id;
            else Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@crp_id";
            Param.Size = 4;
            if (crp_id > 0) Param.Value = crp_id;
            else Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@trn_id";
            Param.Size = 4;
            if (trn_id > 0)
                Param.Value = trn_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@doc_id";
            Param.Size = 8;
            if (doc_id > 0) Param.Value = doc_id;
            else Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@tur_codigo";
            Param.Size = 30;
            if (!string.IsNullOrEmpty(tur_codigo)) Param.Value = tur_codigo;
            else Param.Value = DBNull.Value;
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

            return qs.Return;
        }

        #endregion Métodos RS
    }
}
