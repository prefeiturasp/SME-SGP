/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL.Abstracts;

namespace MSTech.GestaoEscolar.DAL
{
    /// <summary>
    /// 
    /// </summary>
    public class ACA_CalendarioAnualDAO : Abstract_ACA_CalendarioAnualDAO
    {
        #region Métodos

        /// <summary>
        /// Retorna todos os calendários não excluídos logicamente por curso
        /// </summary>                
        /// <param name="esc_id">ID da escola</param>        
        /// <param name="ent_id">Entidade do usuário logado</param>
        public DataTable SelectBy_CalendarioAnual_Esc_id
        (
              int esc_id
            , Guid ent_id
            , long doc_id = 0
            , Guid usu_id = new Guid()
            , Guid gru_id = new Guid()
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioAnual_SelectBy_Esc_id", _Banco);
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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@usu_id";
                Param.Size = 16;
                if (usu_id != new Guid())
                    Param.Value = usu_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@gru_id";
                Param.Size = 16;
                if (gru_id != new Guid())
                    Param.Value = gru_id;
                else
                    Param.Value = DBNull.Value;
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
        /// Retorna todos os calendários não excluídos logicamente
        /// filtrados entidade, escola
        /// </summary>        
        /// <param name="esc_id">ID da escola</param>        
        /// <param name="ent_id">Entidade do usuário logado</param>
        public DataTable SelectBy_CalendarioAnualRelCurso_EscId
        (
              int esc_id
            , Guid ent_id
            , long doc_id = 0
            , Guid usu_id = new Guid()
            , Guid gru_id = new Guid()
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioAnualRelCurso_SelectBy_EscId", _Banco);
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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@usu_id";
                Param.Size = 16;
                if (usu_id != new Guid())
                    Param.Value = usu_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@gru_id";
                Param.Size = 16;
                if (gru_id != new Guid())
                    Param.Value = gru_id;
                else
                    Param.Value = DBNull.Value;
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
        /// Verifica se existe o Ano letivo cadastrado
        /// </summary>      
        /// <param name="cal_ano">Ano do calendário escolar</param>        
        /// <param name="ent_id">Entidade do usuário logado</param>    
        public bool VerificaAnoLetivoExistente
        (
            int cal_ano
             , Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioAnual_VerificaAnoLetivo", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_ano";
                Param.Size = 4;
                Param.Value = cal_ano;
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
        /// Retorna todos os calendários da entidade.
        /// </summary>
        /// <param name="ent_id">ID da entidade</param>
        /// <returns></returns>
        public DataTable SelectBy_Entidade
        (
           Guid ent_id
            , long doc_id = 0
            , Guid usu_id = new Guid()
            , Guid gru_id = new Guid()
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioAnual_SelectBy_Entidade", _Banco);

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
            if (doc_id > 0)
                Param.Value = doc_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@usu_id";
            Param.Size = 16;
            if (usu_id != new Guid())
                Param.Value = usu_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@gru_id";
            Param.Size = 16;
            if (gru_id != new Guid())
                Param.Value = gru_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }


        /// <summary>
        /// Seleciona os calendarios com bimestres ativos por entidade e escola
        /// </summary>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="tev_idEfetivacao">Tipo de evento de efetivação de notas</param>
        /// <param name="VerificaEscolaCalendarioPeriodo">Informa se irá selecionar todos os dados conforme os filtros (false) 
        /// ou se irá selecionar apenas os dados que não estão na tabela ESC_EscolaCalendarioPeriodo (true)</param>
        /// <returns></returns>
        public DataTable SelecionaCalendariosComBimestresAberto_Por_EntidadeEscola
        (
            Guid ent_id
            , int esc_id
            , int tev_idEfetivacao
            , bool VerificaEscolaCalendarioPeriodo
            , long doc_id = 0
            , Guid usu_id = new Guid()
            , Guid gru_id = new Guid()
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioAnual_BimestresAberto_SelectBy_EntidadeEscola", _Banco);

            #region PARAMETROS

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
            if (esc_id > 0)
                Param.Value = esc_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tev_idEfetivacao";
            Param.Size = 4;
            if (tev_idEfetivacao > 0)
                Param.Value = tev_idEfetivacao;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@VerificaEscolaCalendarioPeriodo";
            Param.Size = 1;
            Param.Value = VerificaEscolaCalendarioPeriodo;
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
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@usu_id";
            Param.Size = 16;
            if (usu_id != new Guid())
                Param.Value = usu_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@gru_id";
            Param.Size = 16;
            if (gru_id != new Guid())
                Param.Value = gru_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }


        /// <summary>
        /// Retorna todos os calendários não excluídos logicamente
        /// </summary>                
        /// <param name="cal_ano">Ano do calendário escolar</param>
        /// <param name="cal_descricao">Descrição do calendário escolar</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="paginado">Indica se o datatable será paginado ou não</param> 
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        /// <param name="totalRecords">Total de registros retornado na busca</param>   
        public DataTable SelectBy_Pesquisa
        (
            int cal_ano
            , string cal_descricao
            , Guid ent_id
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioAnual_SelectBy_Pesquisa", _Banco);
            try
            {
                #region PARAMETROS

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
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@cal_descricao";
                Param.Size = 200;
                if (!String.IsNullOrEmpty(cal_descricao))
                    Param.Value = cal_descricao;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
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
        /// Retorna todos os calendários não excluídos logicamente por docente
        /// </summary>                
        /// <param name="doc_id">ID do docente</param>        
        /// <param name="ent_id">Entidade do usuário logado</param>
        public DataTable SelectBy_doc_id
        (
            long doc_id
            , Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioAnual_SelectBy_doc_id", _Banco);
            try
            {
                #region PARAMETROS

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
        /// Retorna todos os calendários não excluídos logicamente por curso
        /// </summary>                
        /// <param name="cur_id">ID do curso</param>        
        /// <param name="ent_id">Entidade do usuário logado</param>
        public DataTable SelectBy_cur_id
        (
            int cur_id
            , Guid ent_id
            , long doc_id = 0
            , Guid usu_id = new Guid()
            , Guid gru_id = new Guid()
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioAnual_SelectBy_cur_id", _Banco);
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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@usu_id";
                Param.Size = 16;
                if (usu_id != new Guid())
                    Param.Value = usu_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@gru_id";
                Param.Size = 16;
                if (gru_id != new Guid())
                    Param.Value = gru_id;
                else
                    Param.Value = DBNull.Value;
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
        /// Retorna todos os calendários não excluídos logicamente por tipo nível ensino
        /// </summary>                
        /// <param name="tne_id">ID do tipo nível ensino</param>        
        /// <param name="ent_id">Entidade do usuário logado</param>
        public DataTable SelectBy_tne_id
        (
            int tne_id
            , Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioAnual_SelectBy_tne_id", _Banco);
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
        /// Retorna todos os calendários não excluídos logicamente por curso e ano inicio processo
        /// </summary>                
        /// <param name="cur_id">ID do curso</param>        
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="pfi_id">ID do processo fechamento inicio</param>
        public DataTable SelectBy_cur_id_pfi_id
        (
            int cur_id
            , Guid ent_id
            , int pfi_id
            , long doc_id = 0
            , Guid usu_id = new Guid()
            , Guid gru_id = new Guid()
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioAnual_SelectBy_cur_id_pfi_id", _Banco);
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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@pfi_id";
                Param.Size = 4;
                Param.Value = pfi_id;
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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@usu_id";
                Param.Size = 16;
                if (usu_id != new Guid())
                    Param.Value = usu_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@gru_id";
                Param.Size = 16;
                if (gru_id != new Guid())
                    Param.Value = gru_id;
                else
                    Param.Value = DBNull.Value;
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
        /// Retorna todos os calendários anuais de cursos que possuem 
        ///	disciplina eletiva filtrando (ou não) por curso e por escola.
        /// </summary>        
        /// <param name="cur_id">ID do curso</param>
        /// <param name="cur_id">ID da escola</param>
        /// <param name="cur_id">ID da unidade</param>
        /// <param name="cur_id">ID do tipo disciplina</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        public DataTable GetSelectPorCursoComDisciplinaEletiva
        (
            int cur_id
            , int esc_id
            , int uni_id
            , int tds_id
            , Guid ent_id
            , long doc_id = 0
            , Guid usu_id = new Guid()
            , Guid gru_id = new Guid()
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioAnual_SelectByCursoComDisciplinaEletiva", _Banco);
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
                Param.Value = tds_id;
                qs.Parameters.Add(Param);

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
                if (doc_id > 0)
                    Param.Value = doc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@usu_id";
                Param.Size = 16;
                if (usu_id != new Guid())
                    Param.Value = usu_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@gru_id";
                Param.Size = 16;
                if (gru_id != new Guid())
                    Param.Value = gru_id;
                else
                    Param.Value = DBNull.Value;
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
        /// Retorna todos os calendários não excluídos logicamente, por curso e quantidade de períodos do calendário
        /// </summary>                
        /// <param name="cur_id">ID do curso</param>        
        /// <param name="qtdePeriodos">Quantidade de períodos do calendário</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        public DataTable SelectBy_CursoQtdePeriodos
        (
            int cur_id
            , int qtdePeriodos
            , Guid ent_id
            , long doc_id = 0
            , Guid usu_id = new Guid()
            , Guid gru_id = new Guid()
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioAnual_SelectBy_CursoQtdePeriodos", _Banco);
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
                Param.ParameterName = "@qtdePeriodos";
                Param.Size = 4;
                Param.Value = qtdePeriodos;
                qs.Parameters.Add(Param);

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
                if (doc_id > 0)
                    Param.Value = doc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@usu_id";
                Param.Size = 16;
                if (usu_id != new Guid())
                    Param.Value = usu_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@gru_id";
                Param.Size = 16;
                if (gru_id != new Guid())
                    Param.Value = gru_id;
                else
                    Param.Value = DBNull.Value;
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

        public DataTable SelectBy_AnosLetivos()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioAnual_Select_AnosLetivos", _Banco);
            try
            {
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
        /// Verifica se o calendário escolar está sendo utilizado em alguma turma do peja
        /// </summary>
        /// <param name="cal_id">ID do calendário escolar</param>        
        /// <param name="ent_id">Entidade do usuário logado</param>        
        public bool SelectBy_VerificaTurmaPeja
        (
            int cal_id
            , Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioAnual_SelectBy_VerificaTurmaPeja", _Banco);
            try
            {
                #region PARAMETROS

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

                #endregion

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
        /// Verifica se já existe um calendário cadastrado com a mesma descrição
        /// </summary>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cal_descricao">Nome do calendário</param>   
        /// <param name="ent_id">Entidade do usuário logado</param>
        public bool SelectBy_Nome
        (
            ACA_CalendarioAnual entity
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioAnual_SelectBy_Nome", _Banco);
            try
            {
                #region PARAMETROS

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
                Param.ParameterName = "@cal_descricao";
                Param.Size = 100;
                Param.Value = entity.cal_descricao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = entity.ent_id;
                qs.Parameters.Add(Param);

                #endregion

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
        /// Retorna o calendário anual da turma.
        /// </summary>
        /// <param name="tur_id">ID da turma.</param>
        /// <returns></returns>
        public ACA_CalendarioAnual SelecionaPorTurma(long tur_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioAnual_SelecionaPorTurma", _Banco);

            try
            {
                #region Parametro

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return.Rows.Count > 0 ?
                    DataRowToEntity(qs.Return.Rows[0], new ACA_CalendarioAnual()) : new ACA_CalendarioAnual();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna o calendário anual da turma disciplina.
        /// </summary>
        /// <param name="tud_id">ID da turma disciplina.</param>
        /// <returns></returns>
        public ACA_CalendarioAnual SelecionaPorTurmaDisciplina(long tud_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioAnual_SelecionaPorTurmaDisciplina", _Banco);

            try
            {
                #region Parametro

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return.Rows.Count > 0 ?
                    DataRowToEntity(qs.Return.Rows[0], new ACA_CalendarioAnual()) : new ACA_CalendarioAnual();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Seleciona ano letivo do calendário corrente
        /// </summary>
        /// <param name="ent_id">ID da entidade do usuário logado.</param>
        /// <returns></returns>
        public int SelecionaAnoLetivoCorrente(Guid ent_id)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_ACA_CalendarioAnual_SelecionaAnoLetivoCorrente", _Banco);

            try
            {
                #region Parâmetro

                Param = qs.NewParameter();
                Param.ParameterName = "@ent_id";
                Param.DbType = DbType.Guid;
                Param.Size = 16;
                Param.Value = ent_id;
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

        #endregion

        #region Métodos Sobrescritos

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_CalendarioAnual entity)
        {
            entity.cal_dataCriacao = DateTime.Now;
            entity.cal_dataAlteracao = DateTime.Now;

            base.ParamInserir(qs, entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_CalendarioAnual entity)
        {
            entity.cal_dataAlteracao = DateTime.Now;
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@cal_dataCriacao");
        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity">Entidade ACA_CalendarioAnual</param>
        /// <returns>true = sucesso | false = fracasso</returns> 
        protected override bool Alterar(ACA_CalendarioAnual entity)
        {
            __STP_UPDATE = "NEW_ACA_CalendarioAnual_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamDeletar(QueryStoredProcedure qs, ACA_CalendarioAnual entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            Param.Value = entity.cal_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@cal_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@cal_dataAlteracao";
            Param.Size = 8;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity">Entidade ACA_CalendarioAnual</param>
        /// <returns>true = sucesso | false = fracasso</returns> 
        public override bool Delete(ACA_CalendarioAnual entity)
        {
            __STP_DELETE = "NEW_ACA_CalendarioAnual_UPDATE_Situacao";
            return base.Delete(entity);
        }

        #endregion

        #region Comentados

        ///// <summary>
        ///// Inseri os valores da classe em um registro ja existente
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem modificados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Alterar(ACA_CalendarioAnual entity)
        //{
        //    return base.Alterar(entity);
        //}
        ///// <summary>
        ///// Inseri os valores da classe em um novo registro
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem inseridos</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Inserir(ACA_CalendarioAnual entity)
        //{
        //    return base.Inserir(entity);
        //}
        ///// <summary>
        ///// Carrega um registro da tabela usando os valores nas chaves
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem carregados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Carregar(ACA_CalendarioAnual entity)
        //{
        //    return base.Carregar(entity);
        //}
        ///// <summary>
        ///// Exclui um registro do banco
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem apagados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Delete(ACA_CalendarioAnual entity)
        //{
        //    return base.Delete(entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Alterar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamAlterar(QueryStoredProcedure qs, ACA_CalendarioAnual entity)
        //{
        //    base.ParamAlterar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Carregar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_CalendarioAnual entity)
        //{
        //    base.ParamCarregar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Deletar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamDeletar(QueryStoredProcedure qs, ACA_CalendarioAnual entity)
        //{
        //    base.ParamDeletar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Inserir
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_CalendarioAnual entity)
        //{
        //    base.ParamInserir(qs, entity);
        //}
        ///// <summary>
        ///// Salva o registro no banco de dados
        ///// </summary>
        ///// <param name="entity">Entidade com os dados para preenchimento para inserir ou alterar</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Salvar(ACA_CalendarioAnual entity)
        //{
        //    return base.Salvar(entity);
        //}
        ///// <summary>
        ///// Realiza o select da tabela
        ///// </summary>
        ///// <returns>Lista com todos os registros da tabela</returns>
        //public override IList<ACA_CalendarioAnual> Select()
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
        //public override IList<ACA_CalendarioAnual> Select_Paginado(int currentPage, int pageSize, out int totalRecord)
        //{
        //    return base.Select_Paginado(currentPage, pageSize, out totalRecord);
        //}
        ///// <summary>
        ///// Recebe o valor do auto incremento e coloca na propriedade 
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_CalendarioAnual entity)
        //{
        //    return base.ReceberAutoIncremento(qs, entity);
        //}
        ///// <summary>
        ///// Passa os dados de um datatable para uma entidade
        ///// </summary>
        ///// <param name="dr">DataRow do datatable preenchido</param>
        ///// <param name="entity">Entidade onde ser�o transferidos os dados</param>
        ///// <returns>Entidade preenchida</returns>
        //public override ACA_CalendarioAnual DataRowToEntity(DataRow dr, ACA_CalendarioAnual entity)
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
        //public override ACA_CalendarioAnual DataRowToEntity(DataRow dr, ACA_CalendarioAnual entity, bool limparEntity)
        //{
        //    return base.DataRowToEntity(dr, entity, limparEntity);
        //}

        #endregion
    }
}