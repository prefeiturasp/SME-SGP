/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL.Abstracts;
using MSTech.GestaoEscolar.Entities;

namespace MSTech.GestaoEscolar.DAL
{
    /// <summary>
    ///
    /// </summary>
    public class ACA_CursoDAO : Abstract_ACA_CursoDAO
    {
        #region Métodos

        /// <summary>
        /// Retorna os cur_id e crr_id relacionados ao curso informado (além dele mesmo),
        /// que tenham ligação com a escola informada.
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="somenteAtivos">True - Trazer os cursos relacionados ativos / False - Trazer os cursos relacionados não excluídos logicamente</param>
        /// <returns></returns>
        public DataTable Seleciona_CursosRelacionados_Por_Escola
        (
            int cur_id
            , int crr_id
            , int esc_id
            , int uni_id
            , bool somenteAtivos
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Curso_SelecionaCursosRelacionados_Por_Escola", _Banco);

            #region PARAMETROS

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
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@somenteAtivos";
            Param.Size = 1;
            Param.Value = somenteAtivos;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna os cur_id e crr_id relacionados ao curso informado (além dele mesmo),
        /// que tenham ligação com a escola informada e que estejam vigentes.
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <returns></returns>
        public DataTable Seleciona_CursosRelacionadosVigentes_Por_Escola
        (
            int cur_id
            , int crr_id
            , int esc_id
            , int uni_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Curso_SelecionaCursosRelacionadosVigentes_Por_Escola", _Banco);

            #region PARAMETROS

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

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna os dados relacionados ao curso informado (além dele mesmo),
        /// que tenham ligação com a escola e o período informados.
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="crp_id">ID do período do currículo</param>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>        
        /// <returns></returns>
        public DataTable SelecionaCursosRelacionados_Por_EscolaCursoPeriodo
        (
            int cur_id
            , int crr_id
            , int crp_id
            , int esc_id
            , int uni_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Curso_SelecionaCursosRelacionados_Por_EscolaCursoPeriodo", _Banco);

            #region PARAMETROS

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

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna os cur_id e crr_id relacionados ao curso informado.
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <returns></returns>
        public DataTable Seleciona_CursosRelacionados
        (
            int cur_id
            , int crr_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Curso_SelecionaCursosRelacionados", _Banco);

            #region PARAMETROS

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

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna em DataTable os cursos relacionados em mesmo nível de ensino.
        /// </summary>
        /// <param name="cur_id">Id do Curso</param>
        /// <param name="crr_id">Id do Currículo</param>
        /// <param name="tne_id">Id do Nível de Ensino</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <returns>DataTable com cursos relacionados em mesmo nível de ensino.</returns>
        public DataTable SelectCursoRelacionadoNivelEnsino
        (
            int cur_id
            , int crr_id
            , int tne_id
            , Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CursoRelacionadoNivelEnsino", _Banco);
            try
            {
                #region PARAMETROS

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
                Param.ParameterName = "@tne_id";
                Param.Size = 4;
                Param.Value = tne_id;
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
        /// Retorna um datatable contendo todos os cursos
        /// que não foram excluídas logicamente, filtrados por
        /// esc_id, uni_id, cur_id, cur_nome, cur_codigo, tne_id, tme_id, cur_situacao
        /// </summary>
        /// <param name="esc_id"></param>
        /// <param name="uni_id"></param>
        /// <param name="cur_id">Id da tabela ACA_Curso do bd</param>
        /// <param name="cur_nome">Campo cur_nome da tabela ACA_Curso do bd</param>
        /// <param name="cur_codigo">Campo cur_codigo da tabela ACA_Curso do bd</param>
        /// <param name="tne_id">Campo tne_id da tabela ACA_Curso do bd</param>
        /// <param name="tme_id">Campo tme_id da tabela ACA_Curso do bd</param>
        /// <param name="cur_situacao">Campo cur_situacao da tabela ACA_Curso do bd</param>
        /// <param name="ent_id"></param>
        /// <param name="usu_id"></param>
        /// <param name="gru_id"></param>
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        /// <param name="totalRecords">Total de registros retornado na busca</param>
        /// <returns>DataTable com tipos de dependencia paginado</returns>
        public DataTable SelectBy_Pesquisa
        (
            int esc_id
            , int uni_id
            , int cur_id
            , string cur_nome
            , string cur_codigo
            , int tne_id
            , int tme_id
            , byte cur_situacao
            , Guid ent_id
            , Guid usu_id
            , Guid gru_id
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Curso_SelectBy_Pesquisa", _Banco);
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
                Param.ParameterName = "@cur_id";
                Param.Size = 4;
                if (cur_id > 0)
                    Param.Value = cur_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@cur_nome";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(cur_nome))
                    Param.Value = cur_nome;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@cur_codigo";
                Param.Size = 10;
                if (!string.IsNullOrEmpty(cur_codigo))
                    Param.Value = cur_codigo;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tne_id";
                Param.Size = 4;
                if (tne_id > 0)
                    Param.Value = tne_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tme_id";
                Param.Size = 4;
                if (tme_id > 0)
                    Param.Value = tme_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@cur_situacao";
                Param.Size = 1;
                if (cur_situacao > 0)
                    Param.Value = cur_situacao;
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
        /// Retorna um datatable contendo todos os cursos e curriculos
        /// que não foram excluídas logicamente, filtrados por
        /// cur_situacao
        /// </summary>
        /// <param name="esc_id"></param>
        /// <param name="uni_id"></param>
        /// <param name="cur_situacao">Campo cur_situacao da tabela ACA_Curso do bd</param>
        /// <param name="ent_id"></param>
        /// <param name="cal_id"></param>
        /// <returns>DataTable com tipos de dependencia paginado</returns>
        public DataTable SelectCursoCurriculoCalendarioEscola
        (
            int esc_id
            , int uni_id
            , byte cur_situacao
            , Guid ent_id
            , int cal_id
            , bool mostraEJAModalidades
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CursoCurriculo_SelectBy_CalendarioEscola", _Banco);
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
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@cur_situacao";
                Param.Size = 1;
                if (cur_situacao > 0)
                    Param.Value = cur_situacao;
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
                Param.ParameterName = "@cal_id";
                Param.Size = 32;
                Param.Value = cal_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@mostraEJAModalidades";
                Param.Size = 1;
                Param.Value = mostraEJAModalidades;
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
        /// Retorna um datatable contendo todos os cursos e curriculos
        /// que não foram excluídas logicamente, filtrados por
        /// cur_situacao
        /// </summary>
        /// <param name="esc_id"></param>
        /// <param name="uni_id"></param>
        /// <param name="cur_situacao">Campo cur_situacao da tabela ACA_Curso do bd</param>
        /// <param name="ent_id"></param>
        /// <param name="cal_id"></param>
        /// <returns>DataTable com tipos de dependencia paginado</returns>
        public DataTable SelecionaPorCalendarioEscolaDisciplina
        (
            int esc_id
            , int uni_id
            , int dis_id
            , byte cur_situacao
            , Guid ent_id
            , int cal_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CursoCurriculo_SelectBy_CalendarioEscolaDisciplina", _Banco);
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
                Param.ParameterName = "@dis_id";
                Param.Size = 4;
                Param.Value = dis_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@cur_situacao";
                Param.Size = 1;
                if (cur_situacao > 0)
                    Param.Value = cur_situacao;
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
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                Param.Value = cal_id;
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
        /// Retorna um datatable contendo todos os cursos e curriculos
        /// que não foram excluídas logicamente, filtrados por
        /// cur_situacao
        /// </summary>
        /// <param name="esc_id"></param>
        /// <param name="uni_id"></param>
        /// <param name="cur_situacao">Campo cur_situacao da tabela ACA_Curso do bd</param>
        /// <param name="ent_id"></param>
        /// <param name="cal_id"></param>
        /// <returns>DataTable com tipos de dependencia paginado</returns>
        public DataTable SelectCursoCurriculoVigentesCalendarioEscola
        (
            int esc_id
            , int uni_id
            , byte cur_situacao
            , Guid ent_id
            , int cal_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CursoCurriculo_SelectVigentesBy_CalendarioEscola", _Banco);
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
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@cur_situacao";
                Param.Size = 1;
                if (cur_situacao > 0)
                    Param.Value = cur_situacao;
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
                Param.ParameterName = "@cal_id";
                Param.Size = 32;
                Param.Value = cal_id;
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
        /// Traz todos os cursos e currículos da entidade.
        /// </summary>
        /// <param name="ent_id">ID da entidade</param>
        /// <returns></returns>
        public DataTable SelectCursoCurriculo
        (
            Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CursoCurriculo_Select_CursoCurriculo", _Banco);

            #region PARAMETROS

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
        /// Traz todos os cursos que permitem efetivação semestral.
        /// </summary>
        /// <param name="ent_id">ID da entidade</param>
        /// <returns></returns>
        public DataTable CursoCurriculoEfetivacaoSemestral
        (
            Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CursoCurriculo_Select_CursoCurriculoEfetivacaoSemestral", _Banco);

            #region PARAMETROS

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
        /// Traz todos os cursos que permitem efetivação semestral filtrados pelo acesso do usuário
        /// </summary>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="usu_id">ID do usuário</param>
        /// <param name="gru_id">ID do grupo do usuário</param>
        /// <returns></returns>
        public DataTable CursoCurriculoEfetivacaoSemestral_Usuario
        (
            Guid ent_id,
            Guid usu_id,
            Guid gru_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CursoCurriculo_Select_CursoCurriculoEfetivacaoSemestral_Usuario", _Banco);

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
            if (usu_id.Equals(new Guid()))
                Param.Value = DBNull.Value;
            else
                Param.Value = usu_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@gru_id";
            Param.Size = 16;
            if (gru_id.Equals(new Guid()))
                Param.Value = DBNull.Value;
            else
                Param.Value = gru_id;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna um datatable contendo todos os cursos e curriculos
        /// que não foram excluídas logicamente, filtrados por
        /// cur_situacao
        /// </summary>
        /// <param name="uni_id"></param>
        /// <param name="cur_situacao">Campo cur_situacao da tabela ACA_Curso do bd</param>
        /// <param name="ent_id"></param>
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        /// <param name="totalRecords">Total de registros retornado na busca</param>
        /// <param name="esc_id"></param>
        /// <returns>DataTable com tipos de dependencia paginado</returns>
        public DataTable SelectCursoCurriculo
        (
            int esc_id
            , int uni_id
            , byte cur_situacao
            , Guid ent_id
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
            , bool mostraEJAModalidades
        )
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CursoCurriculo_SelectBy_esc_id_uni_id_cur_situacao", _Banco);
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
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@cur_situacao";
                Param.Size = 1;
                if (cur_situacao > 0)
                    Param.Value = cur_situacao;
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
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@mostraEJAModalidades";
                Param.Size = 1;
                Param.Value = mostraEJAModalidades;
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
        /// Retorna um datatable contendo todos os cursos e curriculos
        /// que não foram excluídas logicamente, filtrados por
        /// cur_situacao
        /// </summary>
        /// <param name="usu_id">ID do usuário</param>
        /// <param name="gru_id">ID do grupo do usuário</param>
        /// <param name="cur_situacao">Campo cur_situacao da tabela ACA_Curso do bd</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        /// <param name="totalRecords">Total de registros retornado na busca</param>
        /// <returns>DataTable com tipos de dependencia paginado</returns>
        public DataTable SelectCursoCurriculoPorUsuario
        (
            Guid usu_id
            , Guid gru_id
            , byte cur_situacao
            , Guid ent_id
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CursoCurriculo_SelectBy_usu_id_gru_id_cur_situacao", _Banco);
            try
            {
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
                if (!gru_id.Equals(new Guid()))
                    Param.Value = gru_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@cur_situacao";
                Param.Size = 1;
                if (cur_situacao > 0)
                    Param.Value = cur_situacao;
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
        /// Retorna um datatable contendo todos os cursos e curriculos
        /// que não foram excluídas logicamente, filtrados por
        /// cur_situacao
        /// </summary>
        /// <param name="uni_id"></param>
        /// <param name="ent_id"></param>
        /// <param name="esc_id"></param>
        /// <returns>DataTable com tipos de dependencia paginado</returns>
        public DataTable SelectCursoCurriculoPorEscola
        (
            int esc_id
            , int uni_id
            , Guid ent_id
        )
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CursoCurriculo_Escola", _Banco);
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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

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
        /// Retorna um datatable contendo todos os cursos e curriculos
        /// que não foram excluídas logicamente e que estão vigentes, 
        /// filtrados por cur_situacao
        /// </summary>
        /// <param name="uni_id"></param>
        /// <param name="ent_id"></param>
        /// <param name="esc_id"></param>
        /// <returns>DataTable com tipos de dependencia paginado</returns>
        public DataTable SelectCursoCurriculoVigentesPorEscola
        (
            int esc_id
            , int uni_id
            , Guid ent_id
        )
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CursoCurriculo_VigentesEscola", _Banco);
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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

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
        /// Retorna todos os cursos/currículos não excluídas logicamente
        /// Filtrado por escola
        /// </summary>
        /// <param name="esc_id">Escola do curso</param>
        /// <param name="uni_id">Unidade da escola </param>
        /// <param name="tne_id">ID do tipo nivel de ensino</param> 
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <returns>DataTable com os dados</returns>
        public DataTable SelectCursoCurriculoPorEscolaNivelEnsino
        (
            int esc_id
            , int uni_id
            , int tne_id
            , Guid ent_id
        )
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CursoCurriculo_EscolaNivelEnsino", _Banco);
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
                Param.ParameterName = "@tne_id";
                Param.Size = 4;
                Param.Value = tne_id;
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
        /// Retorna todos os cursos/currículos não excluídas logicamente
        /// Filtrado por tipo de nível de ensino
        /// </summary>
        /// <param name="tne_id">ID do tipo nivel de ensino</param> 
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <returns>DataTable com os dados</returns>
        public DataTable SelectCursoCurriculoPorNivelEnsino
        (
            int tne_id
            , Guid ent_id
        )
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CursoCurriculo_NivelEnsino", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tne_id";
                Param.Size = 4;
                Param.Value = tne_id;
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
        /// Retorna um datatable contendo todos os cursos e curriculos do PEJA
        /// que não foram excluídas logicamente, filtrados por
        /// cur_situacao
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade de escola</param>
        /// <param name="cur_situacao">Situacao do curso</param>
        /// <param name="ent_id">ID da entidade do usuário logado</param>
        /// <returns>DataTable com tipos de dependencia paginado</returns>
        public DataTable SelectCursoCurriculoPorEscolaPEJA(int esc_id, int uni_id, byte cur_situacao, Guid ent_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CursoCurriculo_SelectBy_esc_id_uni_id_cur_situacaoPEJA", _Banco);

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
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@cur_situacao";
                Param.Size = 1;
                if (cur_situacao > 0)
                    Param.Value = cur_situacao;
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
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna um datatable contendo todos os cursos e curriculos
        /// que não foram excluídos logicamente, filtrados (ou não)
        /// por escola e situacao que possuem disciplina eletiva
        /// </summary>
        /// <param name="uni_id">ID da unidade</param>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="tds_id">ID do tipo de disciplina</param>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="cur_situacao">Situação do curso, caso seja passado o valor 0 a situação será desconsiderada</param>
        public DataTable SelectCursoComDisciplinaEletiva
        (
            int esc_id
            , int uni_id
            , int tds_id
            , Guid ent_id
            , int cur_situacao
            , bool mostraEJAModalidades
        )
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Curso_SelectCursoComDisciplinaEletiva", _Banco);
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
                Param.ParameterName = "@tds_id";
                Param.Size = 4;
                if (tds_id > 0)
                    Param.Value = tds_id;
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
                Param.ParameterName = "@cur_situacao";
                Param.Size = 4;
                if (cur_situacao > 0)
                    Param.Value = cur_situacao;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@mostraEJAModalidades";
                Param.Size = 1;
                Param.Value = mostraEJAModalidades;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

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
        /// Verifica se existe o curso na escola
        /// </summary>
        /// <param name="esc_id">Escola do curso</param>
        /// <param name="uni_id">Unidade da escola </param>
        /// <param name="cur_id">Curso da escola </param>
        /// <param name="cur_situacao">Situação do curso</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <returns>
        /// TRUE - o curso passado pelo parametro existe na escola
        /// FALSE - o curso passado pelo parametro NÃO existe na escola
        /// </returns>
        public bool VerificaCursoExistenteEscola
        (
            int esc_id
            , int uni_id
            , int cur_id
            , byte cur_situacao
            , Guid ent_id

        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Curso_VerificaCursoExistenteEscola", _Banco);
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
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@cur_situacao";
                Param.Size = 1;
                if (cur_situacao > 0)
                    Param.Value = cur_situacao;
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
        /// Retorna um datatable contendo todos os cursos e curriculos
        /// que não foram excluídas logicamente, filtrados por
        /// ids das escolas, ids da unidades escola e curso padrao
        /// (utilizado na configuração da matrícula)
        /// </summary>
        /// <param name="esc_ids">Campo cur_situacao da tabela ACA_Curso do bd</param>
        /// <param name="uni_ids">Campo cur_situacao da tabela ACA_Curso do bd</param>
        /// <param name="cur_padrao">Campo cur_situacao da tabela ACA_Curso do bd</param>
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        /// <param name="totalRecords">Total de registros retornado na busca</param>
        /// <returns>DataTable com tipos de dependencia paginado</returns>
        public DataTable SelectBy_esc_ids_uni_ids_cur_padrao
        (
            string esc_ids
            , string uni_ids
            , string cur_padrao
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CursoCurriculo_SelectBy_esc_uni_ids_cur_padrao", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@esc_ids";
                Param.Size = 1000;
                if (!string.IsNullOrEmpty(esc_ids))
                    Param.Value = esc_ids;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@uni_ids";
                Param.Size = 1000;
                if (!string.IsNullOrEmpty(uni_ids))
                    Param.Value = uni_ids;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@cur_padrao";
                Param.Size = 1;
                if (!string.IsNullOrEmpty(cur_padrao))
                    Param.Value = cur_padrao;
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
            catch
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        public bool SelectBy_NomeCurso(string cur_nome, int cur_id, Guid ent_id, out ACA_Curso entity)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Curso_SelectBy_Nome", _Banco);
            try
            {
                entity = new ACA_Curso();

                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@cur_nome";
                Param.Size = 100;
                Param.Value = cur_nome;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cur_id";
                Param.Size = 4;
                Param.Value = cur_id;
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

        public bool SelectBy_NomeCurso(ACA_Curso entity)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Curso_SelectBy_Nome", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@cur_nome";
                Param.Size = 100;
                Param.Value = entity.cur_nome;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cur_id";
                Param.Size = 4;
                Param.Value = entity.cur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = entity.ent_id;
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

        public bool SelectBy_CodigoCurso(ACA_Curso entity)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Curso_SelectBy_Codigo", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@cur_codigo";
                Param.Size = 100;
                Param.Value = entity.cur_codigo;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cur_id";
                Param.Size = 4;
                Param.Value = entity.cur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = entity.ent_id;
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
        /// Verifica se existe algum aluno curriculo para o curso
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        public bool SelectBy_VerificaAlunoCurriculo(int cur_id, int crr_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Curso_VerificaAlunoCurriculo", _Banco);
            try
            {
                #region PARAMETROS

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
        /// Seleciona cursos não excluidos lógicamente
        /// </summary>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <returns>Datatable de cursos</returns>
        public DataTable SelecionaCursoNaoExcluido(Guid ent_id)
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Curso_Select_NaoExcluidos", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

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
        /// Verifica se existe alguma turma para o curso
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        public bool SelectBy_VerificaTurma(int cur_id, int crr_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Curso_VerificaTurma", _Banco);
            try
            {
                #region PARAMETROS

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
        /// Seleciona as etapas de ensino ativas e seus turnos por escola e etapa de ensino.
        /// </summary>
        /// <param name="uad_idSuperior">ID da unidade superior.</param>
        /// <param name="esc_id">ID da escola.</param>
        /// <param name="uni_id">ID da unidade de escola.</param>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo.</param>
        /// <param name="ent_id">ID da entidade do usuário logado.</param>
        /// <returns></returns>
        public DataTable SelecionaCursosTurnosPorEscola(Guid uad_idSuperior, int esc_id, int uni_id, int cur_id, int crr_id, Guid ent_id, bool mostraCodigoEscola)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Curso_SelecionaCursosTurnosPorEscola", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 20;
                if (uad_idSuperior != Guid.Empty)
                    Param.Value = uad_idSuperior;
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
                Param.Value = uni_id;
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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 20;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@mostraCodigoEscola";
                Param.Size = 1;
                Param.Value = mostraCodigoEscola;
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
        /// Carrega os cursos relacionados a escola passada por parametro
        /// </summary>
        /// <param name="esc_id">Id da escola</param>
        /// <returns></returns>
        public DataTable SelecionaCursoByEsc_id(long esc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Curso_SelectBy_Esc_id", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                Param.Value = esc_id;
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
        /// Carrega os cursos pelo id ou nome.
        /// </summary>
        /// <param name="cur_id">Id do curso</param>  
        /// <param name="cur_nome">Nome do curso</param>  
        /// <returns>Datatable com os dados ACA_Calendario, ACA_CalendarioCurso e ACA_CalendarioPeriodo</returns>
        public DataTable SelecionaCursoPorIdNome(int cur_id, string cur_nome)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Curso_SelectBy_Cur_Id_Cur_Nome", _Banco);

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
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@cur_nome";
                Param.Size = 200;
                if (!String.IsNullOrEmpty(cur_nome))
                    Param.Value = cur_nome;
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
        /// Seleciona os cursos não excluídos logicamente filtrados por escola e tipo de ciclo.
        /// </summary>
        /// <param name="esc_id">ID da escola.</param>
        /// <param name="uni_id">ID da unidade de escola.</param>
        /// <param name="tci_id">ID do tipo de ciclo.</param>
        /// <param name="ent_id">ID da entidade do usuário logado.</param>
        /// <returns></returns>
        public DataTable SelecionaPorEscolaTipoCiclo(int esc_id, int uni_id, int tci_id, Guid ent_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CursoCurriculo_SelecionaPorEscolaTipoCiclo", _Banco);

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
                Param.ParameterName = "@tci_id";
                Param.Size = 4;
                Param.Value = tci_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
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
        /// Seleciona os cursos não excluídos logicamente e que estão vigentes filtrados por escola e tipo de ciclo.
        /// </summary>
        /// <param name="esc_id">ID da escola.</param>
        /// <param name="uni_id">ID da unidade de escola.</param>
        /// <param name="tci_id">ID do tipo de ciclo.</param>
        /// <param name="ent_id">ID da entidade do usuário logado.</param>
        /// <returns></returns>
        public DataTable SelecionaVigentesPorEscolaTipoCiclo(int esc_id, int uni_id, int tci_id, Guid ent_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CursoCurriculo_SelecionaVigentesPorEscolaTipoCiclo", _Banco);

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
                Param.ParameterName = "@tci_id";
                Param.Size = 4;
                Param.Value = tci_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
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
        /// Retorna todos os cursos/currículos não excluídos logicamente
        /// filtrado por escola, calendário e tipo de ciclo
        /// </summary>
        /// <param name="esc_id">Escola do curso</param>
        /// <param name="uni_id">Unidade da escola </param>
        /// <param name="tci_id">Tipo de ciclo</param>
        /// <param name="cal_id">Calendario</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <returns>DataTable com os dados</returns>
        public DataTable SelecionaPorEscolaCalendarioTipoCiclo(int esc_id, int uni_id, int cal_id, int tci_id, Guid ent_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CursoCurriculo_SelecionaPorEscolaCalendarioTipoCiclo", _Banco);

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
                Param.ParameterName = "@tci_id";
                Param.Size = 4;
                Param.Value = tci_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
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
        /// Retorna todos os cursos/currículos não excluídos logicamente
        /// filtrado por modalidade de ensino
        /// </summary>
        /// <param name="tme_id">Tipo de modalidade de ensino</param>
        /// <param name="esc_id">id da escola</param>
        /// <param name="uni_id">id da unidade da escola</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <returns>DataTable com os dados</returns>
        public DataTable Seleciona_Cursos_Por_ModalidadeEnsino(int tme_id, int esc_id, int uni_id, Guid ent_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CursoCurriculo_TipoModalidadeEnsino", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tme_id";
                Param.Size = 4;
                Param.Value = tme_id;
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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
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


        #region Plataforma de Itens e Avaliações

        /// <summary>
        /// Retorna os cursos de acordo com entidade e calendario
        /// </summary>
        /// <param name="ent_id">Entidade</param>
        /// <param name="cal_id">Calendario</param>
        public DataTable Seleciona_CursosEntidadeCalendario(Guid ent_id, int cal_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("API_BuscaCursos_Por_EntidadeCalendario", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 20;
                Param.Value = ent_id;
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
        /// Retorna os cursos de acordo com entidade e calendario
        /// </summary>
        /// <param name="ent_id">Entidade</param>
        /// <param name="cal_id">Calendario</param>
        /// <param name="esc_id">Escola</param>
        public DataTable Seleciona_CursosEntidadeCalendarioEscola(Guid ent_id, int cal_id, int esc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("API_BuscaCursos_Por_EntidadeCalendarioEscola", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 20;
                Param.Value = ent_id;
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
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                if (esc_id > 0)
                    Param.Value = esc_id;
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

        #endregion Plataforma de Itens e Avaliações

        #endregion Métodos

        #region Comentados

        ///// <summary>
        ///// Inseri os valores da classe em um registro ja existente
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem modificados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Alterar(ACA_Curso entity)
        //{
        //    return base.Alterar(entity);
        //}
        ///// <summary>
        ///// Inseri os valores da classe em um novo registro
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem inseridos</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Inserir(ACA_Curso entity)
        //{
        //    return base.Inserir(entity);
        //}
        ///// <summary>
        ///// Carrega um registro da tabela usando os valores nas chaves
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem carregados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Carregar(ACA_Curso entity)
        //{
        //    return base.Carregar(entity);
        //}
        ///// <summary>
        ///// Exclui um registro do banco
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem apagados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Delete(ACA_Curso entity)
        //{
        //    return base.Delete(entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Alterar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamAlterar(QueryStoredProcedure qs, ACA_Curso entity)
        //{
        //    base.ParamAlterar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Carregar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_Curso entity)
        //{
        //    base.ParamCarregar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Deletar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamDeletar(QueryStoredProcedure qs, ACA_Curso entity)
        //{
        //    base.ParamDeletar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Inserir
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_Curso entity)
        //{
        //    base.ParamInserir(qs, entity);
        //}
        ///// <summary>
        ///// Salva o registro no banco de dados
        ///// </summary>
        ///// <param name="entity">Entidade com os dados para preenchimento para inserir ou alterar</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Salvar(ACA_Curso entity)
        //{
        //    return base.Salvar(entity);
        //}
        ///// <summary>
        ///// Realiza o select da tabela
        ///// </summary>
        ///// <returns>Lista com todos os registros da tabela</returns>
        //public override IList<ACA_Curso> Select()
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
        //public override IList<ACA_Curso> Select_Paginado(int currentPage, int pageSize, out int totalRecord)
        //{
        //    return base.Select_Paginado(currentPage, pageSize, out totalRecord);
        //}
        ///// <summary>
        ///// Recebe o valor do auto incremento e coloca na propriedade
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_Curso entity)
        //{
        //    return base.ReceberAutoIncremento(qs, entity);
        //}
        ///// <summary>
        ///// Passa os dados de um datatable para uma entidade
        ///// </summary>
        ///// <param name="dr">DataRow do datatable preenchido</param>
        ///// <param name="entity">Entidade onde ser�o transferidos os dados</param>
        ///// <returns>Entidade preenchida</returns>
        //public override ACA_Curso DataRowToEntity(DataRow dr, ACA_Curso entity)
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
        //public override ACA_Curso DataRowToEntity(DataRow dr, ACA_Curso entity, bool limparEntity)
        //{
        //    return base.DataRowToEntity(dr, entity, limparEntity);
        //}

        #endregion Comentados

    }
}