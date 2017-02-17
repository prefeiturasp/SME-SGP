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
    public class ACA_DocenteDAO : Abstract_ACA_DocenteDAO
    {
        #region Métodos

        /// <summary>
        /// Retorna todos os docentes que
        /// não foram excluídos logicamente.
        /// </summary>
        /// <param name="ent_id">Id da entidade.</param>
        /// <param name="esc_id">Id da escola.</param>
        /// <param name="tur_id">Id da turma.</param>
        /// <returns>Id e nome do docente.</returns>
        public DataTable SelecionaPorTurma
        (
            Guid ent_id
            , int esc_id
            , long tur_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Docente_SelecionaPorTurma", _Banco);
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
        /// Retorna um datatable contendo todos os docentes
        /// que não foram excluídas logicamente.
        /// </summary>
        /// <param name="esc_id">Id da escola.</param>
        /// <param name="uni_id">Id da unidade.</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <returns>Id e nome do docente.</returns>
        public DataTable SelecionaPorEscola
        (
            int esc_id
            , int uni_id
            , Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Docente_SelecionaPorEscola", _Banco);
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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
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

        /// <summary>
        /// Retorna os professores de acordo com a especialidade dele
        /// (ou ele é especialista ou pode lecionar a matéiria informada [tds_id])
        /// </summary>
        /// <returns></returns>
        public DataTable SelectBy_Especialidade_Escola
        (
            Int32 esc_id
            , Int32 uni_id
            , Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Docente_SelectBy_Escpecialidade_Escola", _Banco);

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

            return qs.Return;
        }

        /// <summary>
        /// Retorna os professores de acordo com a especialidade dele
        /// (ou ele é especialista ou pode lecionar a matéiria informada [tds_id])
        /// </summary>
        /// <returns></returns>
        public DataTable SelectBy_Especialidade
        (
            Int32 esc_id
            , Int32 uni_id
            , Guid ent_id
            , bool crg_especialista
            , Int32 tds_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Docente_SelectBy_Escpecialidade", _Banco);

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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@crg_especialista";
                Param.Size = 1;
                Param.Value = crg_especialista;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tds_id";
                Param.Size = 4;
                Param.Value = tds_id;
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
        /// Traz os docentes que lecionam na escola informda (pela UA do cargo).
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="ent_id">ID da entidade</param>
        /// <returns></returns>
        public DataTable SelectBy_EscolaCargo_ExibindoCargo
        (
            int esc_id
            , Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Docente_SelectBy_EscolaCargo_ExibindoCargo", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@esc_id";
            Param.Size = 4;
            Param.Value = esc_id;
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
        /// Traz os docentes que lecionam na escola informda (pela UA do cargo), filtra se for um docente só.
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="doc_id">ID do docente</param>
        /// <returns></returns>
        public DataTable SelectBy_Docente_EscolaCargo_ExibindoCargo
        (
            int esc_id
            , long doc_id
            , Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Docente_SelectBy_Docente_EscolaCargo_ExibindoCargo", _Banco);

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
            if (doc_id > 0) Param.Value = doc_id;
            else Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Traz os docentes que lecionam na turma informda (pela turmadocente)
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="tur_id">ID da turma</param>
        /// <returns></returns>
        public DataTable SelectBy_Turma_EscolaCargo_ExibindoCargo
        (
            int esc_id
            , long tur_id
            , Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Docente_SelectBy_Turma_EscolaCargo_ExibindoCargo", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@esc_id";
            Param.Size = 4;
            Param.Value = esc_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@ent_id";
            Param.Size = 16;
            Param.Value = ent_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            if (tur_id > 0) Param.Value = tur_id;
            else Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Traz os docentes que lecionam na escola informda (pela UA do cargo).
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="ent_id">ID da entidade</param>
        /// <returns></returns>
        public DataTable SelectBy_EscolaCargo
        (
            int esc_id
            , Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Docente_SelectBy_EscolaCargo", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@esc_id";
            Param.Size = 4;
            Param.Value = esc_id;
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
        /// Traz os docentes de acordo com a função na escola
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade</param>
        /// <param name="fun_id">ID da função</param>
        ///<param name="doc_id">ID do docente especifico(pode ser ignorado o filtro)</param>
        /// <returns></returns>
        public DataTable SelectBy_EscolaFuncao
        (
            Int32 esc_id
            , Int32 uni_id
            , Int32 fun_id
            , Int64 doc_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Docente_SelectBy_EscolaFuncao", _Banco);
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
                Param.ParameterName = "@fun_id";
                Param.Size = 4;
                Param.Value = fun_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@doc_id";
                Param.Size = 8;
                if (doc_id > 0) Param.Value = doc_id;
                else Param.Value = DBNull.Value;
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
        /// Retorna o docente apartir da entidade e pessoa
        /// </summary>
        /// <param name="ent_id">Id da entidade</param>
        /// <param name="pes_id">Id da pessoa</param>
        /// <returns></returns>
        public ACA_Docente SelectBy_Pessoa
        (
            Guid ent_id
            , Guid pes_id
        )
        {
            ACA_Docente entity = new ACA_Docente();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Docente_SelectBy_Pessoa", _Banco);

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
                Param.ParameterName = "@pes_id";
                Param.Size = 16;
                Param.Value = pes_id;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                {
                    entity = DataRowToEntity(qs.Return.Rows[0], entity, false);
                    return entity;
                }

                return new ACA_Docente();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna um datatable contendo todos os docentes
        /// que não foram excluídos logicamente, filtrados por
        /// nome da pessoa, tipo de documento CPF, tipo de documento RG,
        /// entidade, escola, unidade escola, cargo e situação
        /// </summary>
        /// <param name="pes_nome">Id da tabela PES_Pessoa do bd</param>
        /// <param name="tipo_rg"></param>
        /// <param name="ent_id">Id da tabela SYS_Entidade do bd</param>
        /// <param name="usu_id">Id usado na tabela SYS_UsuarioGrupoUA para filtro</param>
        /// <param name="gru_id">Id usado na tabela tabela SYS_UsuarioGrupoUA para filtro</param>
        /// <param name="crg_id">Id da tabela RHU_Cargo do bd</param>
        /// <param name="esc_id">Id da tabela ESC_Escola do bd</param>
        /// <param name="uni_id">Id da tabela ESC_UnidadeEscola do bd</param>
        /// <param name="pes_id"></param>
        /// <param name="doc_situacao">Campo col_situcao da tabela ACA_Docente do bd</param>
        /// <param name="fun_id">Id da Função</param>
        /// <param name="uad_idSuperior"></param>
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        /// <param name="totalRecords">Total de registros retornado na busca</param>
        /// <param name="tipo_cpf"></param>
        /// <returns>DataTable com os docentes</returns>
        public DataTable SelectBy_Pesquisa
        (
            string pes_nome
            , string tipo_cpf
            , string tipo_rg
            , Guid ent_id
            , Guid usu_id
            , Guid gru_id
            , int crg_id
            , int esc_id
            , int uni_id
            , Guid pes_id
            , byte doc_situacao
            , int fun_id
            , Guid uad_idSuperior
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Docente_SelectBy_Pesquisa", _Banco);
            try
            {
                #region PARAMETROS

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
                Param.ParameterName = "@tipo_documentacao_cpf";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(tipo_cpf))
                    Param.Value = tipo_cpf;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tipo_documentacao_rg";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(tipo_rg))
                    Param.Value = tipo_rg;
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
                Param.ParameterName = "@crg_id";
                Param.Size = 4;
                if (crg_id > 0)
                    Param.Value = crg_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@pes_id";
                Param.Size = 16;
                if (pes_id != Guid.Empty)
                    Param.Value = pes_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@doc_situacao";
                Param.Size = 1;
                if (doc_situacao > 0)
                    Param.Value = doc_situacao;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@fun_id";
                Param.Size = 4;
                if (fun_id > 0)
                    Param.Value = fun_id;
                else
                    Param.Value = DBNull.Value;
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
        /// Retorna um datatable contendo todos os docentes
        /// que não foram excluídos logicamente conforme os filtros passados.
        /// </summary>
        /// <param name="pes_nome">Nome do docente.</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <param name="usu_id">Id do usuário.</param>
        /// <param name="gru_id">Id do grupo do usuário.</param>
        /// <param name="esc_id">Id da escola.</param>
        /// <param name="uni_id">Id da unidade escolar./param>
        /// <param name="doc_situacao">Situação do docente.</param>
        /// <param name="paginado">Indica se o datatable será paginado ou não.</param>
        /// <param name="currentPage">Página atual do grid.</param>
        /// <param name="coc_matricula">Matrícula do docente.</param>
        /// <param name="pageSize">Total de registros por página do grid.</param>
        /// <param name="totalRecords">Total de registros retornado na busca.</param>
        /// <param name="uad_idSuperior">Id da unidade adm superior.</param>
        /// <returns>DataTable com os docentes</returns>
        public DataTable SelectBy_Pesquisa
        (
            string pes_nome
            , Guid ent_id
            , Guid usu_id
            , Guid gru_id
            , int esc_id
            , int uni_id
            , byte doc_situacao
            , Guid uad_idSuperior
            , string coc_matricula
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Docente_SelectBy_PesquisaMigracao", _Banco);
            try
            {
                #region PARAMETROS

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
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@doc_situacao";
                Param.Size = 1;
                if (doc_situacao > 0)
                    Param.Value = doc_situacao;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@coc_matricula";
                Param.Size = 30;
                if (!string.IsNullOrEmpty(coc_matricula))
                    Param.Value = coc_matricula;
                else
                    Param.Value = DBNull.Value;
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
        /// Retorna todos os docentes sem considerar permissão, se assim for configurado
        /// </summary>
        /// <param name="pes_nome">Nome do colaborador</param>
        /// <param name="coc_matricula">Matrícula do colaborador</param>
        /// <param name="tipo_cpf">CPF do colaborador</param>
        /// <param name="tipo_rg">RG do colaborador</param>
        /// <param name="crg_id">Cargo do colaborador</param>
        /// <param name="fun_id">Função do colaborador</param>
        /// <param name="uad_id">UA do cargo/função do colaborador</param>
        /// <param name="uad_idSuperior">ID da unidade administrativa superior</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="todosDocentes">Indica se vai considerar a permissão ou não</param>
        /// <param name="usu_id">Usuário logado</param>
        /// <param name="gru_id">Grupo do usuário logado</param>
        /// <param name="totalRecords">Total de registros retornado na busca</param>
        public DataTable SelectBy_Pesquisa_PermissaoTotal
        (
            string pes_nome
            , string coc_matricula
            , string tipo_cpf
            , string tipo_rg
            , int crg_id
            , int fun_id
            , Guid uad_id
            , Guid uad_idSuperior
            , Guid ent_id
            , bool todosDocentes
            , Guid usu_id
            , Guid gru_id
            , bool MostraCodigoEscola
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Docente_SelectBy_Pesquisa_PermissaoTotal", _Banco);

            #region PARAMETROS

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
            Param.ParameterName = "@coc_matricula";
            Param.Size = 30;
            if (!string.IsNullOrEmpty(coc_matricula))
                Param.Value = coc_matricula;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@tipo_documentacao_cpf";
            Param.Size = 50;
            if (!string.IsNullOrEmpty(tipo_cpf))
                Param.Value = tipo_cpf;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@tipo_documentacao_rg";
            Param.Size = 50;
            if (!string.IsNullOrEmpty(tipo_rg))
                Param.Value = tipo_rg;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@crg_id";
            Param.Size = 4;
            if (crg_id > 0)
                Param.Value = crg_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@fun_id";
            Param.Size = 4;
            if (fun_id > 0)
                Param.Value = fun_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.Size = 16;
            Param.ParameterName = "@uad_id";
            if (uad_id != Guid.Empty)
                Param.Value = uad_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.Size = 16;
            Param.ParameterName = "@uad_idSuperior";
            if (uad_idSuperior != Guid.Empty)
                Param.Value = uad_idSuperior;
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
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@todosDocentes";
            Param.Size = 1;
            Param.Value = @todosDocentes;
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
        /// Retorna um datatable contendo todos os docentes
        /// que não foram excluídas logicamente, filtrados por
        /// código do docente, código do colaborador, código da escola,
        /// código da unidade e situação do docente
        /// </summary>
        /// <param name="doc_id">Id da Tabela ACA_Docente do bd</param>
        /// <param name="col_id">ID da Tabela RHU_Colaborador do bd</param>
        /// <param name="esc_id">Id da tabela ESC_Escola do bd</param>
        /// <param name="uni_id">Id da tabela ESC_UnidadeEscola do bd</param>
        /// <param name="doc_situacao">Campo col_situcao da tabela ACA_Docente do bd</param>
        /// <param name="ent_id"></param>
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        /// <param name="totalRecords">Total de registros retornado na busca</param>
        /// <returns>DataTable com os docentes</returns>
        public DataTable SelectBy_All
        (
           int doc_id
            , int col_id
            , int esc_id
            , int uni_id
            , byte doc_situacao
            , Guid ent_id
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Docente_SelectBy_All", _Banco);
            try
            {
                #region PARAMETROS

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
                Param.ParameterName = "@col_id";
                Param.Size = 4;
                if (col_id > 0)
                    Param.Value = col_id;
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
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@doc_situacao";
                Param.Size = 1;
                if (doc_situacao > 0)
                    Param.Value = doc_situacao;
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
        /// Seleciona os dados utilizados na tela de alteração de docente
        /// </summary>
        /// <param name="col_id">ID do colaborador</param>
        /// <param name="doc_id">ID do docente</param>
        /// <returns>DataTable de dados utilizados na tela de alteração de docente</returns>
        public DataTable SelectByColaboradorDocente(long col_id, long doc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Docente_SelectByColaboradorDocente", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@col_id";
                Param.Size = 8;
                Param.Value = col_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@doc_id";
                Param.Size = 8;
                Param.Value = doc_id;
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
        /// Seleciona os dados da pessoa e das turmas do docente.
        /// </summary>
        /// <param name="doc_id">ID do docente.</param>
        /// <returns></returns>
        public DataTable SelecionaDadosDocente(long doc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Docente_SelecionaDadosDocente", _Banco);

            try
            {
                #region Parâmetro

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@doc_id";
                Param.Size = 8;
                Param.Value = doc_id;
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
        /// Seleciona os dados da pessoa e das escolas do docente.
        /// </summary>
        /// <param name="doc_id">ID do docente.</param>
        /// <returns></returns>
        public DataTable SelecionaEscolaDocente(long doc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Docente_SelecionaEscolaDocente", _Banco);

            try
            {
                #region Parâmetro

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@doc_id";
                Param.Size = 8;
                Param.Value = doc_id;
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
        /// retorna os registros de docente com os dados de pessoa e usuarios. se passado a ultimaModificacao
        /// retorna apenas os dados alterados/criados a partir desta data.
        /// </summary>
        /// <param name="tur_id">id da turma</param>
        /// <param name="dataBase">data base para seleção dos registros</param>
        /// <returns></returns>
        public DataTable SelecionaDocentesPorTurma(long tur_id, DateTime dataBase)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Docente_SelecionarPorTurma", _Banco);
            try
            {

                #region Parametros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;
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
        /// retorna uma lista de docentes por escola. se passado a ultimaModificacao
        /// retorna apenas os dados alterados/criados a partir desta data.
        /// </summary>
        /// <param name="esc_id">id da escola</param>
        /// <param name="dataBase">data base para seleção dos registros</param>
        /// <returns></returns>
        public DataTable SelecionaDocentesPorEscola(int esc_id, DateTime dataBase)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Docente_SelecionarPorEscola", _Banco);
            try
            {

                #region Parametros
                
                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                Param.Value = esc_id;
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
        /// Retorna os docentes de uma escola que não foram excluidos logicamente conforme os filtros passados.
        /// </summary>
        /// <param name="escId"></param>
        /// <param name="nome"></param>
        /// <param name="matricula"></param>
        /// <param name="cpf"></param>
        /// <param name="tdsId"></param>
        /// <returns></returns>
        public DataTable SelectBy_PesquisaEscola(
            int escId
            , string nome
            , string matricula
            , string cpf
            , int tdsId
            , out int totalRecords)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Docente_SelectBy_PesquisaEscola", _Banco);
            try
            {

                #region Parametros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                Param.Value = escId;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pes_nome";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(nome))
                    Param.Value = nome;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@coc_matricula";
                Param.Size = 30;
                if (!string.IsNullOrEmpty(matricula))
                    Param.Value = matricula;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@TIPO_DOCUMENTACAO_CPF";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(cpf))
                    Param.Value = cpf;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tds_id";
                Param.Size = 4;
                Param.Value = tdsId;
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
        /// Retorna os docentes de uma escola quando informado ou da rede quando não informado conforme o filtro passado.
        /// </summary>
        /// <param name="escId"></param>
        /// <returns></returns>
        public DataTable SelectBy_PesquisaEscolaRede(
            int escId
            , string nome
            , string matricula
            , string cpf
            , string rg
            , Guid usu_id
            , Guid gru_id
            , Guid ent_id
            , bool adm
            , out int totalRecords)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Docente_SelectBy_PesquisaEscolaRede", _Banco);
            try
            {
                #region Parametros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                if (escId > 0)
                    Param.Value = escId;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pes_nome";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(nome))
                    Param.Value = nome;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@coc_matricula";
                Param.Size = 30;
                if (!string.IsNullOrEmpty(matricula))
                    Param.Value = matricula;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@TIPO_DOCUMENTACAO_CPF";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(cpf))
                    Param.Value = cpf;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@TIPO_DOCUMENTACAO_RG";
                Param.Size = 50;
                if (!string.IsNullOrEmpty(rg))
                    Param.Value = rg;
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
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@adm";
                Param.Size = 1;
                Param.Value = adm;
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
        /// Retorna se o docente possui uma atribuição esporádica 
        /// com vigência que englobe a data da aula, ou, no caso do docente normal,
        /// retorna o proprio docente.
        /// </summary>
        /// <param name="docId"></param>
        /// <param name="dataAula"></param>
        /// <returns></returns>
        public bool ValidaAulaAtribuicaoEsporadica(long docId, DateTime dataAula)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Docente_ValidaAulaAtribuicaoEsporadica", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@doc_id";
            Param.Size = 8;
            Param.Value = docId;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@data_aula";
            Param.Size = 8;
            Param.Value = dataAula;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return.Rows.Count > 0;
        }

        /// <summary>
        /// Retorna uma listagem de usuários de docentes na mesma unidade ou abaixo da unidade do usuário/grupo informado.
        /// </summary>
        /// <param name="usu_id">ID do usuário que está pesquisando</param>
        /// <param name="gru_id">ID do grupo do usuário que está pesquisando</param>
        /// <param name="sis_id">ID do sistema</param>
        /// <returns></returns>
        public DataTable SelecionarUsrDocentesPorUsuarioGrupo(Guid usu_id, Guid gru_id, int sis_id, string usu_login, string usu_email, string pes_nome)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Docente_SelectBy_UsuarioGrupo", _Banco);
            try
            {
                #region Parametros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@sis_id";
                Param.Size = 4;
                if (sis_id > 0)
                    Param.Value = sis_id;
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
                Param.ParameterName = "@usu_login";
                Param.Size = 500;
                if (!string.IsNullOrEmpty(usu_login))
                    Param.Value = usu_login;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@usu_email";
                Param.Size = 500;
                if (!string.IsNullOrEmpty(usu_email))
                    Param.Value = usu_email;
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

        #endregion Métodos

        #region RS

        /// <summary>
        /// Retorna todos os docentes sem considerar permissão, se assim for configurado
        /// </summary>
        /// <param name="pes_nome">Nome do colaborador</param>
        /// <param name="tipo_cpf">CPF do colaborador</param>
        /// <param name="tipo_rg">RG do colaborador</param>
        /// <param name="crg_id">Cargo do colaborador</param>
        /// <param name="uad_id">UA do cargo/função do colaborador</param>
        /// <param name="uad_idSuperior">ID da unidade administrativa superior</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="adm">Indica se o usuário é administrador</param>
        /// <param name="usu_id">Usuário logado</param>
        /// <param name="gru_id">Grupo do usuário logado</param>
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        /// <param name="totalRecords">Total de registros retornado na busca</param>
        public DataTable SelectBy_BuscaDocenteCargo
        (
            string pes_nome
            , string tipo_cpf
            , string tipo_rg
            , int crg_id
            , Guid uad_idSuperior
            , Guid uad_id
            , bool adm
            , Guid ent_id
            , Guid usu_id
            , Guid gru_id
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Docente_SelectBy_BuscaDocenteCargo", _Banco);

            #region PARAMETROS

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
            Param.ParameterName = "@tipo_documentacao_cpf";
            Param.Size = 50;
            if (!string.IsNullOrEmpty(tipo_cpf))
                Param.Value = tipo_cpf;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@tipo_documentacao_rg";
            Param.Size = 50;
            if (!string.IsNullOrEmpty(tipo_rg))
                Param.Value = tipo_rg;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@crg_id";
            Param.Size = 4;
            if (crg_id > 0)
                Param.Value = crg_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.Size = 16;
            Param.ParameterName = "@uad_id";
            if (uad_id != Guid.Empty)
                Param.Value = uad_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.Size = 16;
            Param.ParameterName = "@uad_idSuperior";
            if (uad_idSuperior != Guid.Empty)
                Param.Value = uad_idSuperior;
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

            #endregion PARAMETROS

            totalRecords = qs.Execute(currentPage, pageSize);

            return qs.Return;
        }

        #endregion RS

        #region Métodos Sobrescritos

        /// <summary>
        /// Parâmetros para efetuar a inclusão com data de criação e de alteração fixas
        /// </summary>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_Docente entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@doc_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@doc_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_Docente entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@doc_dataCriacao");
            qs.Parameters["@doc_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity"> Entidade ACA_Docente</param>
        /// <returns>true = sucesso | false = fracasso</returns>
        protected override bool Alterar(ACA_Docente entity)
        {
            __STP_UPDATE = "NEW_ACA_Docente_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        protected override void ParamDeletar(QueryStoredProcedure qs, ACA_Docente entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@doc_id";
            Param.Size = 8;
            Param.Value = entity.doc_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@doc_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@doc_dataAlteracao";
            Param.Size = 8;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity"> Entidade ACA_Docente</param>
        /// <returns>true = sucesso | false = fracasso</returns>
        public override bool Delete(ACA_Docente entity)
        {
            __STP_DELETE = "NEW_ACA_Docente_Update_Situacao";
            return base.Delete(entity);
        }

        #endregion Métodos Sobrescritos

        #region Comentados

        ///// <summary>
        ///// Inseri os valores da classe em um registro ja existente
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem modificados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Alterar(ACA_Docente entity)
        //{
        //    return base.Alterar(entity);
        //}
        ///// <summary>
        ///// Inseri os valores da classe em um novo registro
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem inseridos</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Inserir(ACA_Docente entity)
        //{
        //    return base.Inserir(entity);
        //}
        ///// <summary>
        ///// Carrega um registro da tabela usando os valores nas chaves
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem carregados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Carregar(ACA_Docente entity)
        //{
        //    return base.Carregar(entity);
        //}
        ///// <summary>
        ///// Exclui um registro do banco
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem apagados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Delete(ACA_Docente entity)
        //{
        //    return base.Delete(entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Alterar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamAlterar(QueryStoredProcedure qs, ACA_Docente entity)
        //{
        //    base.ParamAlterar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Carregar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_Docente entity)
        //{
        //    base.ParamCarregar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Deletar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamDeletar(QueryStoredProcedure qs, ACA_Docente entity)
        //{
        //    base.ParamDeletar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Inserir
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_Docente entity)
        //{
        //    base.ParamInserir(qs, entity);
        //}
        ///// <summary>
        ///// Salva o registro no banco de dados
        ///// </summary>
        ///// <param name="entity">Entidade com os dados para preenchimento para inserir ou alterar</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Salvar(ACA_Docente entity)
        //{
        //    return base.Salvar(entity);
        //}
        ///// <summary>
        ///// Realiza o select da tabela
        ///// </summary>
        ///// <returns>Lista com todos os registros da tabela</returns>
        //public override IList<ACA_Docente> Select()
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
        //public override IList<ACA_Docente> Select_Paginado(int currentPage, int pageSize, out int totalRecord)
        //{
        //    return base.Select_Paginado(currentPage, pageSize, out totalRecord);
        //}
        ///// <summary>
        ///// Recebe o valor do auto incremento e coloca na propriedade
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_Docente entity)
        //{
        //    return base.ReceberAutoIncremento(qs, entity);
        //}
        ///// <summary>
        ///// Passa os dados de um datatable para uma entidade
        ///// </summary>
        ///// <param name="dr">DataRow do datatable preenchido</param>
        ///// <param name="entity">Entidade onde ser�o transferidos os dados</param>
        ///// <returns>Entidade preenchida</returns>
        //public override ACA_Docente DataRowToEntity(DataRow dr, ACA_Docente entity)
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
        //public override ACA_Docente DataRowToEntity(DataRow dr, ACA_Docente entity, bool limparEntity)
        //{
        //    return base.DataRowToEntity(dr, entity, limparEntity);
        //}

        #endregion Comentados
    }
}