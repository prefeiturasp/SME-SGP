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
    public class ESC_UnidadeEscolaDAO : Abstract_ESC_UnidadeEscolaDAO
    {
        /// <summary>
        /// Retorna as escolas não excluidas logicamente
        /// </summary>
        /// <param name="esc_id">ID de escola</param>
        /// <param name="uni_id">ID de unidade escola</param>
        /// <param name="uni_situacao">Situacao de unidade escola</param>
        /// <param name="ent_id">ID da entidade - Obrigatório</param>
        /// <param name="gru_id">Grupo do usuário logado - Obrigatório</param>
        /// <param name="usu_id">ID do usuário logado - Obrigatório</param>
        /// <param name="paginado">Indica se vai exibir os registros paginados ou não.</param>
        /// <param name="currentPage">Página atual do gridview</param>
        /// <param name="pageSize">Total de registros por página</param>
        /// <param name="totalRecords"></param>
        ///<param name="ordenarEscolasPorCodigo">Valor do parâmetro ORDENAR_ESCOLAS_POR_CODIGO</param>
        ///<returns>DataTable com as unidades escolares</returns>
        public DataTable SelecionaPorEscolaEntidadeUnidadeSituacaoPemissaoUsuario
        (
            int esc_id
            , int uni_id
            , Byte uni_situacao
            , Guid ent_id
            , Guid gru_id
            , Guid usu_id
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
            , bool ordenarEscolasPorCodigo
            , bool buscarTerceirizadas = true
            , bool esc_controleSistema = false
            
        )
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_UnidadeEscola_SelecionaPorEscolaEntidadeUnidadeSituacaoPemissaoUsuario", _Banco);
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
                Param.ParameterName = "@uni_situacao";
                Param.Size = 1;
                if (uni_situacao > 0)
                    Param.Value = uni_situacao;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                if (ent_id == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@gru_id";
                Param.Size = 16;
                if (gru_id == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = gru_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@usu_id";
                Param.Size = 16;
                if (usu_id == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = usu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@ordenarEscolasPorCodigo";
                Param.Size = 1;
                Param.Value = ordenarEscolasPorCodigo;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@buscarTerceirizadas";
                Param.Size = 1;
                Param.Value = buscarTerceirizadas;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@esc_controleSistema";
                Param.Size = 1;
                Param.Value = esc_controleSistema;
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
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna as escolas controladas pelo sistema
        /// </summary>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="gru_id">Grupo do usuário logadoo</param>
        /// <param name="usu_id">ID do usuário logado</param>
        /// <param name="esc_controleSistema">parametro que definira se as escolas controladas pelo sistema serão retornadas ou não</param>
        /// <param name="paginado">Indica se vai exibir os registros paginados ou não.</param>
        /// <param name="currentPage">Página atual do gridview</param>
        /// <param name="pageSize">Total de registros por página</param>
        /// <param name="totalRecords"></param>
        ///<param name="ordenarEscolasPorCodigo">Valor do parâmetro ORDENAR_ESCOLAS_POR_CODIGO</param>
        ///<returns>DataTable com as unidades escolares</returns>
        public DataTable SelecionaEscolasControladasPorEntidadePermissaoUsuario
        (
            Guid ent_id
            , Guid gru_id
            , Guid usu_id
            , Nullable<bool> esc_controleSistema
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
            , bool ordenarEscolasPorCodigo
        )
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_UnidadeEscola_SelecionaEscolasControladasPorEntidadePermissaoUsuario", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                if (ent_id == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@gru_id";
                Param.Size = 16;
                if (gru_id == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = gru_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@usu_id";
                Param.Size = 16;
                if (usu_id == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = usu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@esc_controleSistema";
                Param.Size = 1;
                if (esc_controleSistema.HasValue)
                    Param.Value = esc_controleSistema;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@ordenarEscolasPorCodigo";
                Param.Size = 1;
                Param.Value = ordenarEscolasPorCodigo;
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
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna um datatable contendo todas as unidades de escola
        /// a partir do código do aluno e entidade. Usado na visão individual
        /// Utilizado nas telas: Mensagens.
        /// </summary>
        /// <param name="alu_id">Código do aluno</param>
        /// <param name="ent_id">Código da entidade</param>
        /// <param name="ordenarEscolasCodigo">Indica se vai exibir e ordenar o combo por código da escola</param>
        /// <returns>DataTable com as unidades escolares</returns>
        public DataTable SelectEscolas_VisaoAluno
        (
            Int64 alu_id,
            Guid ent_id,
            bool ordenarEscolasCodigo
        )
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_UnidadeEscola_SelectBy_VisaoAluno", _Banco);
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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@ordenarEscolasCodigo";
                Param.Size = 1;
                Param.Value = ordenarEscolasCodigo;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                    dt = qs.Return;

                return dt;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna um datatable contendo todas as unidades de escola
        /// a partir do código do responsável e entidade. Usado na visão individual
        /// Utilizado nas telas: Mensagens.
        /// </summary>
        /// <param name="alu_id">Código da pessoa do responsável</param>
        /// <param name="ent_id">Código da entidade</param>
        /// <param name="ordenarEscolasCodigo">Indica se vai exibir e ordenar o combo por código da escola</param>
        /// <returns>DataTable com as unidades escolares</returns>
        public DataTable SelectEscolas_VisaoResponsavel
        (
            Guid pes_id,
            Guid ent_id,
            bool ordenarEscolasCodigo
        )
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_UnidadeEscola_SelectBy_VisaoResponsavel", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@pes_id";
                Param.Size = 16;
                Param.Value = pes_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@ordenarEscolasCodigo";
                Param.Size = 1;
                Param.Value = ordenarEscolasCodigo;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                    dt = qs.Return;

                return dt;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna um datatable contendo todas as unidades de escola
        /// a partir do código do docente e entidade. Usado na visão individual
        /// Utilizado nas telas: Documentos do docente, Atribuição do docente, Compensação de Ausências.
        /// </summary>
        /// <param name="doc_id">Código do docente</param>
        /// <param name="ent_id">Código da entidade</param>
        /// <param name="ordenarEscolasCodigo">Indica se vai exibir e ordenar o combo por código da escola</param>
        /// <param name="vinculoColaboradorCargo">
        ///                                       0 -- Busca as escolas pelas atribuições de turma docente do docente
        ///                                       1 -- Busca as escolas do docente pelo vinculo de colaborador cargo
        ///                                       2 -- Busca escolas do docente com vigência na escola
        ///                                       3 -- Busca as escolas pelas atribuições de turma docente do docente com situaçao diferente 3
        /// </param>
        /// <returns>DataTable com as unidades escolares</returns>
        public DataTable SelectEscolas_VisaoIndividual
        (
            Int64 doc_id,
            Guid ent_id,
            Byte vinculoColaboradorCargo
            , bool ordenarEscolasCodigo
        )
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_UnidadeEscola_SelectBy_VisaoIndividual", _Banco);
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

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@vinculoColaboradorCargo";
                Param.Size = 1;
                Param.Value = vinculoColaboradorCargo;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@ordenarEscolasCodigo";
                Param.Size = 1;
                Param.Value = ordenarEscolasCodigo;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                    dt = qs.Return;

                return dt;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna um datatable contendo todas as unidades de escola,
        /// não considerando as permissões do usuário.
        /// Utilizado na tela de movimentação.
        /// </summary>
        /// <param name="ent_id">ID da entidade - Obrigatório</param>
        /// <param name="situacao_Desativado">
        /// flag que determina que serão carregadas apenas
        /// escolas com as situações 1 – Ativo ou 5 – Em ativação
        /// descartando a situação 4 – Desativado
        ///</param>
        /// <param name="esc_controleSistema">parametro que definira se as escolas controladas pelo sistema serão retornadas ou não</param>
        /// <returns>DataTable com as unidades escolares</returns>
        public DataTable SelectBy_All_PermissaoTotal
        (
            Guid ent_id
            , bool situacao_Desativado
            , Nullable<bool> esc_controleSistema
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_UnidadeEscola_SelectBy_All_PermissaoTotal", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                if (ent_id == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@situacao_Desativado";
                Param.Size = 16;
                Param.Value = situacao_Desativado;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@esc_controleSistema";
                Param.Size = 1;
                if (esc_controleSistema.HasValue)
                    Param.Value = esc_controleSistema.Value;
                else
                    Param.Value = DBNull.Value;
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
        /// Retorna um datatable contendo todas as unidades de escola,
        /// não considerando as permissões do usuário.
        /// Utilizado na tela de movimentação.
        /// Além disso, só exibe as escolas que possuem o curso informado ou equivalente
        /// </summary>
        ///<param name="cur_id"></param>
        ///<param name="crr_id"></param>
        ///<param name="crp_id"></param>
        ///<param name="ent_id">ID da entidade - Obrigatório</param>
        /// <param name="situacao_Desativado">
        /// flag que determina que serão carregadas apenas
        /// escolas com as situações 1 – Ativo ou 5 – Em ativação
        /// descartando a situação 4 – Desativado
        ///</param>
        /// <param name="esc_controleSistema">parametro que definira se as escolas controladas pelo sistema serão retornadas ou não</param>
        ///<returns>DataTable com as unidades escolares</returns>
        public DataTable SelecionaPorCursoPeriodo_PermissaoTotal
        (
            int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , bool situacao_Desativado
            , Nullable<bool> esc_controleSistema
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_UnidadeEscola_SelecionaPorCursoPeriodo_PermissaoTotal", _Banco);
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
                if (ent_id == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@situacao_Desativado";
                Param.Size = 16;
                Param.Value = situacao_Desativado;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@esc_controleSistema";
                Param.Size = 1;
                if (esc_controleSistema.HasValue)
                    Param.Value = esc_controleSistema.Value;
                else
                    Param.Value = DBNull.Value;
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
        /// Retorna um datatable contendo todas as unidades de escola
        /// que não foram excluídas logicamente
        /// </summary>
        /// <param name="cur_id">ID de curso</param>
        /// <param name="uad_idSuperior">ID da unidade administrativa superior</param>
        /// <param name="ent_id">ID da entidade - Obrigatório</param>
        /// <param name="usu_id">ID do usuário logado - Obrigatório</param>
        /// <param name="paginado">Indica se vai exibir os registros paginados ou não.</param>
        /// <param name="currentPage">Página atual do gridview</param>
        /// <param name="pageSize">Total de registros por página</param>
        /// <param name="totalRecords"></param>
        /// <param name="gru_id">Grupo do usuário logado - Obrigatório</param>
        /// <returns>DataTable com as unidades escolares</returns>
        public DataTable SelectBy_cur_id
        (
            int cur_id
            , Guid uad_idSuperior
            , Guid ent_id
            , Guid gru_id
            , Guid usu_id
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_UnidadeEscola_SelectBy_cur_id", _Banco);
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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                if (uad_idSuperior == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = uad_idSuperior;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                if (ent_id == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@gru_id";
                Param.Size = 16;
                if (gru_id == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = gru_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@usu_id";
                Param.Size = 16;
                if (usu_id == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = usu_id;
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
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna as escolas pelo uad_idSuperior, de acordo com a permissão da pessoa.
        /// </summary>
        /// <param name="uad_idSuperior">ID da unidade administrativa superior</param>
        /// <param name="ent_id">ID da entidade - Obrigatório</param>
        /// <param name="gru_id">Grupo do usuário logado - Obrigatório</param>
        /// <param name="usu_id">ID do usuário logado - Obrigatório</param>
        /// <param name="uni_situacao">situaçao da unidade escola, caso seja passado o valor 0 a situação será desconsiderada</param>
        /// <param name="esc_controleSistema">parametro que definira se as escolas controladas pelo sistema serão retornadas ou não</param>
        /// <param name="ordenarEscolasPorCodigo">Indica se vai trazer escolas com código e ordenar por código</param>
        /// <returns>DataTable com as unidades escolares</returns>
        public DataTable SelectBy_uad_Superior
        (
            Guid uad_idSuperior
            , Guid ent_id
            , Guid gru_id
            , Guid usu_id
            , byte uni_situacao
            , bool? esc_controleSistema
            , bool ordenarEscolasPorCodigo
            , bool buscarTerceirizadas = true
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_UnidadeEscola_SelectBy_uad_idSuperior", _Banco);
            try
            {
                DataTable dt = new DataTable();

                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                if (!uad_idSuperior.Equals(Guid.Empty))
                    Param.Value = uad_idSuperior;
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

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@uni_situacao";
                Param.Size = 1;
                if (uni_situacao > 0)
                    Param.Value = uni_situacao;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@esc_controleSistema";
                Param.Size = 1;
                if (esc_controleSistema.HasValue)
                    Param.Value = esc_controleSistema.Value;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@ordenarEscolasPorCodigo";
                Param.Size = 1;
                Param.Value = ordenarEscolasPorCodigo;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@buscarTerceirizadas";
                Param.Size = 1;
                Param.Value = buscarTerceirizadas;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                    dt = qs.Return;

                return dt;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna as escolas pelo uad_idSuperior, não considerando as permissões do usuário.
        /// Utilizado na tela de movimentação.
        /// </summary>
        /// <param name="uad_idSuperior"></param>
        /// <param name="ent_id"></param>
        /// <param name="situacao_Desativado">
        /// flag que determina que serão carregadas apenas
        /// escolas com as situações 1 – Ativo ou 5 – Em ativação
        /// descartando a situação 4 – Desativado
        /// </param>
        /// <param name="esc_controleSistema">parametro que definira se as escolas controladas pelo sistema serão retornadas ou não</param>
        /// <param name="esc_situacao"></param>
        /// <returns></returns>
        public DataTable SelectBy_uad_SuperiorPermissaoTotal
        (
            Guid uad_idSuperior
            , Guid ent_id
            , bool situacao_Desativado
            , Nullable<bool> esc_controleSistema
            , byte esc_situacao = 0
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_UnidadeEscola_SelectBy_uad_idSuperior_PermissaoTotal", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                if (!uad_idSuperior.Equals(Guid.Empty))
                    Param.Value = uad_idSuperior;
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
                Param.ParameterName = "@situacao_Desativado";
                Param.Size = 16;
                Param.Value = situacao_Desativado;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@esc_controleSistema";
                Param.Size = 1;
                if (esc_controleSistema.HasValue)
                    Param.Value = esc_controleSistema.Value;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@esc_situacao";
                Param.Size = 1;
                if (esc_situacao > 0)
                    Param.Value = esc_situacao;
                else
                    Param.Value = DBNull.Value;
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
        /// Retorna as escolas sem acesso, de acordo com a permissão da pessoa.
        /// </summary>
        /// <param name="uad_idSuperior">ID da unidade administrativa superior</param>
        /// <param name="cal_id">Calendario</param>
        /// <param name="tpc_id">Periodo</param>
        /// <param name="ent_id">ID da entidade - Obrigatório</param>
        /// <param name="gru_id">Grupo do usuário logado - Obrigatório</param>
        /// <param name="usu_id">ID do usuário logado - Obrigatório</param>
        /// <param name="uni_situacao">situaçao da unidade escola, caso seja passado o valor 0 a situação será desconsiderada</param>
        /// <param name="esc_controleSistema">parametro que definira se as escolas controladas pelo sistema serão retornadas ou não</param>
        /// <param name="ordenarEscolasPorCodigo">Indica se vai trazer escolas com código e ordenar por código</param>
        /// <returns>DataTable com as unidades escolares</returns>
        public DataTable SelectBy_SemAcesso
        (
            Guid uad_idSuperior
            , int cal_id
            , int tpc_id
            , Guid ent_id
            , Guid gru_id
            , Guid usu_id
            , byte uni_situacao
            , bool? esc_controleSistema
            , bool ordenarEscolasPorCodigo
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_UnidadeEscolaSemAcesso", _Banco);
            try
            {
                DataTable dt = new DataTable();

                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                if (!uad_idSuperior.Equals(Guid.Empty))
                    Param.Value = uad_idSuperior;
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

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@uni_situacao";
                Param.Size = 1;
                if (uni_situacao > 0)
                    Param.Value = uni_situacao;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@esc_controleSistema";
                Param.Size = 1;
                if (esc_controleSistema.HasValue)
                    Param.Value = esc_controleSistema.Value;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@ordenarEscolasPorCodigo";
                Param.Size = 1;
                Param.Value = ordenarEscolasPorCodigo;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                    dt = qs.Return;

                return dt;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna as escolas sem acesso
        /// </summary>
        /// <param name="uad_idSuperior">ID da unidade administrativa superior</param>
        /// <param name="cal_id">Calendario</param>
        /// <param name="tpc_id">Periodo</param>
        /// <param name="ent_id">ID da entidade - Obrigatório</param>
        /// <param name="gru_id">Grupo do usuário logado - Obrigatório</param>
        /// <param name="usu_id">ID do usuário logado - Obrigatório</param>
        /// <param name="uni_situacao">situaçao da unidade escola, caso seja passado o valor 0 a situação será desconsiderada</param>
        /// <param name="esc_controleSistema">parametro que definira se as escolas controladas pelo sistema serão retornadas ou não</param>
        /// <param name="ordenarEscolasPorCodigo">Indica se vai trazer escolas com código e ordenar por código</param>
        /// <returns>DataTable com as unidades escolares</returns>
        public DataTable SelectBy_SemAcesso_PermissaoTotal
        (
            Guid uad_idSuperior
            , int cal_id
            , int tpc_id
            , Guid ent_id
            , bool situacao_Desativado
            , Nullable<bool> esc_controleSistema
            , byte esc_situacao = 0
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_UnidadeEscolaSemAcesso_PermissaoTotal", _Banco);
            try
            {
                DataTable dt = new DataTable();

                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                if (!uad_idSuperior.Equals(Guid.Empty))
                    Param.Value = uad_idSuperior;
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
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@situacao_Desativado";
                Param.Size = 16;
                Param.Value = situacao_Desativado;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@esc_controleSistema";
                Param.Size = 1;
                if (esc_controleSistema.HasValue)
                    Param.Value = esc_controleSistema.Value;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@esc_situacao";
                Param.Size = 1;
                if (esc_situacao > 0)
                    Param.Value = esc_situacao;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                    dt = qs.Return;

                return dt;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna as escolas pelo uad_idSuperior, curso e período, não considerando as permissões do usuário.
        /// Utilizado na tela de movimentação.
        /// </summary>
        /// <param name="uad_idSuperior"></param>
        /// <param name="crp_id"></param>
        /// <param name="ent_id"></param>
        /// <param name="situacao_Desativado">
        /// flag que determina que serão carregadas apenas
        /// escolas com as situações 1 – Ativo ou 5 – Em ativação
        /// descartando a situação 4 – Desativado
        /// </param>
        /// <param name="esc_controleSistema">parametro que definira se as escolas controladas pelo sistema serão retornadas ou não</param>
        /// <param name="esc_situacao"></param>
        /// <param name="cur_id"></param>
        /// <param name="crr_id"></param>
        /// <returns></returns>
        public DataTable SelecionaPorUASuperiorCursoPeriodo_PermissaoTotal
        (
            Guid uad_idSuperior
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , bool situacao_Desativado
            , Nullable<bool> esc_controleSistema
            , byte esc_situacao = 0
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_UnidadeEscola_SelecionaPorUASuperiorCursoPeriodo_PermissaoTotal", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                if (!uad_idSuperior.Equals(Guid.Empty))
                    Param.Value = uad_idSuperior;
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
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@situacao_Desativado";
                Param.Size = 1;
                Param.Value = situacao_Desativado;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@esc_controleSistema";
                Param.Size = 1;
                if (esc_controleSistema.HasValue)
                    Param.Value = esc_controleSistema.Value;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@esc_situacao";
                Param.Size = 1;
                if (esc_situacao > 0)
                    Param.Value = esc_situacao;
                else
                    Param.Value = DBNull.Value;
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
        /// Verifica o código do última unidade cadastrada para a escola
        /// filtradas por escola
        /// </summary>
        /// <param name="esc_id">Id da tabela ESC_Escola do bd</param>
        /// <returns>uni_id + 1</returns>
        public Int32 SelectBy_esc_id_top_one
        (
            int esc_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_UnidadeEscola_SelectBy_esc_id_top_one", _Banco);
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

                #endregion PARAMETROS

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                    return Convert.ToInt32(qs.Return.Rows[0]["uni_id"].ToString()) + 1;
                else
                    return 1;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna um datatable contendo todas as unidades de escola
        /// que não foram excluídas logicamente, filtradas por
        ///	cur_id, crr_id, crp_id
        /// </summary>
        /// <param name="cur_id">ID do Curso</param>
        /// <param name="crr_id">ID do Curriculo</param>
        /// <param name="crp_id">ID do Curriculo Periodo</param>
        /// <param name="ent_id">ID da Entidade</param>
        /// <param name="uad_id">ID's da Unidade Administrativa dos usuários do grupo Unidade Administrativa e Gestão (Separados por vírgula)</param>
        /// <returns>DataTable com as unidade escola</returns>
        public DataTable SelectBy_cur_id_crr_id_crp_id
        (
            int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , string uad_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_UnidadeEscola_SelectBy_cur_id_crr_id_crp_id", _Banco);
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
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@uad_id";
                if (!string.IsNullOrEmpty(uad_id))
                    Param.Value = uad_id;
                else
                    Param.Value = DBNull.Value;
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

        public DataTable SelectBy_uad_Superior_cur_id_crr_id_crp_id
        (
            Guid uad_idSuperior
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , string uad_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_UnidadeEscola_SelectBy_uad_idSuperior_cur_id_crr_id_crp_id", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                if (!uad_idSuperior.Equals(Guid.Empty))
                    Param.Value = uad_idSuperior;
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
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@uad_id";
                if (!string.IsNullOrEmpty(uad_id))
                    Param.Value = uad_id;
                else
                    Param.Value = DBNull.Value;
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
        /// Seleciona a unidade administrativa superior de uma unidade de escola.
        /// </summary>
        /// <param name="esc_id">ID da escola.</param>
        /// <param name="uni_id">ID da unidade de escola.</param>
        /// <returns></returns>
        public Guid SelecionaUnidadeAdministrativaSuperior(int esc_id, int uni_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_UnidadeEscola_SelecionaUnidadeAdministrativaSuperior", _Banco);

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

                #endregion Parâmetros

                qs.Execute();

                return qs.Return.Rows.Count > 0 ?
                    new Guid(qs.Return.Rows[0]["uad_idSuperior"].ToString()) :
                    Guid.Empty;
            }
            finally
            {
                qs.Parameters.Clear();
            }

        }

        /// <summary>
        /// Retorna as entidades da UnidadeEscola cadastradas nas escolas.
        /// </summary>
        /// <param name="esc_id">ID das escolas</param>
        /// <returns></returns>
        public DataTable SelecionaPorEscolas
        (
            string esc_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_UnidadeEscola_SelecionaPorEscolas", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@esc_id";
            Param.Value = esc_id;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Seleciona as escolas em que o tipo de classificação possui o cargo passado por parâmetro.
        /// </summary>
        /// <param name="crg_id">Id do cargo</param>
        /// <param name="ent_id">ID da entidade do usuário logado.</param>
        /// <param name="gru_id">ID do grupo do usuário logado.</param>
        /// <param name="usu_id">ID do usuário logado.</param>
        /// <param name="adm">Flag que indica se o usuário é administrador.</param>
        /// <param name="ordenarEscolasPorCodigo">Flag que indica se deve ser feito ordenação das escolas por código.</param>
        /// <returns></returns>
        public DataTable SelecionaPorCargoTipoClassificacaoVigente(int crg_id, Guid ent_id, Guid gru_id, Guid usu_id, bool adm, bool ordenarEscolasPorCodigo, bool trazerTerceridas)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_Escola_SelecionaPorCargoTipoClassificacaoVigente", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.ParameterName = "@crg_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.Value = crg_id;
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
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@adm";
                Param.Size = 1;
                Param.Value = adm;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@ordenarEscolasPorCodigo";
                Param.Size = 1;
                Param.Value = ordenarEscolasPorCodigo;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@trazerTerceridas";
                Param.Size = 1;
                Param.Value = trazerTerceridas;
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
        /// Override do ParamInserir que passa DateTime.Now na data de criação e alteração.
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ESC_UnidadeEscola entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@uni_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@uni_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, ESC_UnidadeEscola entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@uni_dataCriacao");
            qs.Parameters["@uni_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity"> Entidade ESC_UnidadeEscola</param>
        /// <returns>true = sucesso | false = fracasso</returns>
        protected override bool Alterar(ESC_UnidadeEscola entity)
        {
            __STP_UPDATE = "NEW_ESC_UnidadeEscola_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        protected override void ParamDeletar(QueryStoredProcedure qs, ESC_UnidadeEscola entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@esc_id";
            Param.Size = 4;
            Param.Value = entity.esc_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@uni_id";
            Param.Size = 4;
            Param.Value = entity.uni_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@uni_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@uni_dataAlteracao";
            Param.Size = 8;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity"> Entidade ESC_UnidadeEscola</param>
        /// <returns>true = sucesso | false = fracasso</returns>
        public override bool Delete(ESC_UnidadeEscola entity)
        {
            __STP_DELETE = "NEW_ESC_UnidadeEscola_Update_Situacao";
            return base.Delete(entity);
        }
    }
}