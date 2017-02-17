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
    public class ESC_EscolaDAO : Abstract_ESC_EscolaDAO
    {
        #region Métodos

        /// <summary>
        /// retorna as escolas pela entidade, qdo informado a data base o retorno é 
        /// apenas de escolas criadas ou alteradas apos esta data... caso contrario 
        /// apenas escolas ativas serão retornadas.
        /// </summary>
        /// <param name="ent_id">id da entidade</param>
        /// <param name="dataBase">data base para a seleção.</param>
        /// <returns></returns>
        public DataTable SelecionarEscolaApi(Int32 esc_id, string esc_codigo, Guid ent_id, DateTime dataBase)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_Escola_SelecionarPorEntidade", _Banco);
            try
            {
                #region Parametros

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
                Param.DbType = DbType.String;
                Param.ParameterName = "@esc_codigo";
                Param.Size = 20;

                if (string.IsNullOrEmpty(esc_codigo))
                    Param.Value = DBNull.Value;
                else
                    Param.Value = esc_codigo;

                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;

                if (Guid.Empty.Equals(ent_id))
                    Param.Value = DBNull.Value;
                else
                    Param.Value = ent_id;

                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@dataBase";
                Param.Size = 8;

                if (dataBase.Equals(new DateTime()))
                    Param.Value = DBNull.Value;
                else
                    Param.Value = dataBase;

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
        /// retorna lista de escola
        /// </summary>
        /// <param name="esc_codigo">código da escola que será substituído pelo INEP</param>
        /// <param name="dataBase">data base para a seleção.</param>
        /// <returns></returns>
        public DataTable ListarEscola()
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_Escola_SelectAll", _Banco);
                        
            try
            {
                qs.Execute();
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

        public DataTable RetornaUadPermissao(Guid usu_id, Guid gru_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_SYS_UnidadeAdministrativa_RetornaUnidades_PorUsuario", _Banco);
            try
            {
                #region Parâmetros

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
        /// Seleciona as Unidades de escolas do Gestão Escolar.
        /// </summary>
        /// <returns></returns>
        public DataTable SelecionaEscola_GestaoIntegracaoRio()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_Importacao_DESESC_SelectEscola", this._Banco);

            try
            {
                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Verifica se há escolas cadastradas com o tua_id passado.
        /// </summary>
        /// <param name="tua_id">Id do tipo de unidade administrativa.</param>
        /// <returns>Verdadeiro se existe pelo menos uma escola com o tipo.</returns>
        public bool VerificaEscolaComTipo
        (
            Guid tua_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_Escola_SelectBy_tua_id", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@tua_id";
                Param.Size = 16;
                Param.Value = tua_id;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

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
        /// Retorna os dados da UA Superior da escola, de acordo com a permissão que o usuário tem (na UA ou na UASuperior).
        /// </summary>
        /// <param name="ent_id"></param>
        /// <param name="gru_id"></param>
        /// <param name="usu_id"></param>
        /// <returns></returns>
        public DataTable Select_UASuperiorBy_Permissao(Guid ent_id,
            Guid gru_id,
            Guid usu_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_Escola_Select_UASuperiorBy_Permissao", _Banco);
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

                #endregion Parâmetros

                qs.Execute();

                DataTable dt = new DataTable();
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
        /// Busca usada pelo método ESC_EscolaBO.ConsultarPeloNome
        /// </summary>
        /// <param name="esc_nome"></param>
        /// <param name="usu_id"></param>
        /// <param name="ent_id"></param>
        /// <param name="gru_id"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public DataTable ConsultarPeloNome(string esc_nome,
            Guid usu_id,
            Guid ent_id,
            Guid gru_id,
            int pageSize,
            int currentPage,
            out int totalRecords)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_Escola_SelectBy_esc_nome", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.Size = 200;
                Param.ParameterName = "@esc_nome";
                if (string.IsNullOrEmpty(esc_nome))
                    Param.Value = DBNull.Value;
                else
                    Param.Value = esc_nome;
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

                #endregion Parâmetros

                DataTable dt = new DataTable();
                totalRecords = qs.Execute(currentPage, pageSize);
                if (qs.Return.Rows.Count > 0)
                    dt = qs.Return;

                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Verifica se o usuário tem permissão para a escola com o código passado,
        /// e preenche a entidade ESC_Escola.
        /// </summary>
        /// <param name="usu_id">Id do usuário.</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <param name="gru_id">Id do grupo usuário.</param>
        /// <param name="entity">Entidade ESC_Escola com o esc_codigo preenchido.</param>
        /// <returns>True - Usuário tem permissão. | False - Usuário não tem permissão.</returns>
        public bool VerificaPermissaoUsuarioBy_Codigo
        (
            Guid usu_id
            , Guid ent_id
            , Guid gru_id
            , ESC_Escola entity
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_Escola_VerificaPermissaoUsuarioBy_Codigo", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.Size = 20;
                Param.ParameterName = "@esc_codigo";
                if (string.IsNullOrEmpty(entity.esc_codigo))
                    Param.Value = DBNull.Value;
                else
                    Param.Value = entity.esc_codigo;
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

                #endregion Parâmetros

                qs.Execute();

                if (qs.Return.Rows.Count == 1)
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
        /// Consulta no banco escolas com o mesmo nome, com Id diferente do id passado, dentro
        /// da mesma entidade.
        /// </summary>
        /// <param name="esc_id"></param>
        /// <param name="esc_nome"></param>
        /// <param name="ent_id"></param>
        /// <returns></returns>
        public bool ExisteEscolaNomeIgual(Int32 esc_id, string esc_nome, Guid ent_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_Escola_SelectBy_NomeIgual", _Banco);

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
                Param.DbType = DbType.String;
                Param.ParameterName = "@esc_nome";
                Param.Value = esc_nome;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

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
        /// Consulta no banco escolas com o mesmo nome, com Id diferente do id passado, dentro
        /// da mesma entidade.
        /// </summary>
        public DataTable RetornaMaiorIDEscola()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_Escola_SelectMaior_esc_id", _Banco);

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
        /// Consulta no banco escolas com o mesmo nome, com Id diferente do id passado, dentro
        /// da mesma entidade, para poder retornar o uad_id.
        /// </summary>
        /// <param name="esc_id"></param>
        /// <param name="esc_nome"></param>
        /// <param name="ent_id"></param>
        /// <returns></returns>
        public DataTable ExisteEscolaNomeIgualDt(Int32 esc_id, string esc_nome, Guid ent_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_Escola_SelectBy_NomeIgual", _Banco);

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
                Param.DbType = DbType.String;
                Param.ParameterName = "@esc_nome";
                Param.Value = esc_nome;
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
        /// Consulta no banco escolas pelo nome, com id diferente do id passado, dentro
        /// da mesma entidade e traz a entidade escola carregada.
        /// </summary>
        /// <param name="entity">Entidade escola</param>
        /// <returns>True = se encontrou escola com determinado nome / False = não encontrou</returns>
        public bool SelectBy_Nome(ESC_Escola entity)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_Escola_SelectBy_NomeIgual", _Banco);

            try
            {
                #region Parâmetros

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
                Param.DbType = DbType.String;
                Param.ParameterName = "@esc_nome";
                Param.Value = entity.esc_nome;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = entity.ent_id;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

                qs.Execute();

                if (qs.Return.Rows.Count == 1)
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
        /// Retorna um datatable contendo todos as escolas
        /// que não foram excluídas logicamente, filtrados por
        /// esc_id, esc_nome, esc_codigo, tne_id, tme_id, esc_situacao
        /// </summary>
        /// <param name="esc_id">Id da tabela ESC_Escola do bd</param>
        /// <param name="esc_nome">Campo esc_nome da tabela ESC_Escola do bd</param>
        /// <param name="esc_codigo">Campo esc_codigo da tabela ESC_Escola do bd</param>
        /// <param name="esc_situacao">Campo esc_situacao da tabela ESC_Escola do bd</param>
        /// <param name="tua_id">Tipo de Escola (UA)</param>
        /// <param name="ent_id">Campo ent_id da tabela ESC_Escola</param>
        /// <param name="uad_idSuperior"></param>
        /// <param name="usu_id"></param>
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        /// <param name="totalRecords">Total de registros retornado na busca</param>
        /// <param name="gru_id"></param>
        /// <param name="TIPO_MEIOCONTATO_TELEFONE"></param>
        /// <param name="cur_id"></param>
        /// <param name="crr_id"></param>
        /// <returns>DataTable com os registros selecionados</returns>
        public DataTable SelectBy_Pesquisa
        (
            int esc_id
            , string esc_nome
            , string esc_codigo
            , byte esc_situacao
            , Guid tua_id
            , Guid ent_id
            , Guid uad_idSuperior
            , Guid gru_id
            , Guid usu_id
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
            , string TIPO_MEIOCONTATO_TELEFONE
            , int cur_id
            , int crr_id
        )
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_Escola_SelectBy_Pesquisa", _Banco);
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
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@esc_nome";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(esc_nome))
                    Param.Value = esc_nome;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@esc_codigo";
                Param.Size = 20;
                if (!string.IsNullOrEmpty(esc_codigo))
                    Param.Value = esc_codigo;
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
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                if (ent_id == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = ent_id;
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
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@esc_telefone";
                Param.Size = 20;
                if (!string.IsNullOrEmpty(TIPO_MEIOCONTATO_TELEFONE))
                    Param.Value = TIPO_MEIOCONTATO_TELEFONE;
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
        /// Retorna um datatable contendo todos as escolas
        /// que não foram excluídas logicamente, filtrados por
        /// esc_id, esc_nome, esc_codigo, tne_id, tme_id, esc_situacao
        /// </summary>
        /// <param name="esc_id">Id da tabela ESC_Escola do bd</param>
        /// <param name="esc_nome">Campo esc_nome da tabela ESC_Escola do bd</param>
        /// <param name="esc_codigo">Campo esc_codigo da tabela ESC_Escola do bd</param>
        /// <param name="esc_situacao">Campo esc_situacao da tabela ESC_Escola do bd</param>
        /// <param name="tua_id">Tipo de Escola (UA)</param>
        /// <param name="ent_id">Campo ent_id da tabela ESC_Escola</param>
        /// <param name="uad_idSuperior"></param>
        /// <param name="usu_id"></param>
        /// <param name="totalRecords">Total de registros retornado na busca</param>
        /// <param name="gru_id"></param>
        /// <param name="TIPO_MEIOCONTATO_TELEFONE"></param>
        /// <param name="cur_id"></param>
        /// <param name="crr_id"></param>
        /// <param name="tce_id">Id do tipo de classificação.</param>
        /// <returns>DataTable com os registros selecionados</returns>
        public DataTable SelectBy_PesquisaNaoPaginado
        (
            int esc_id
            , string esc_nome
            , string esc_codigo
            , byte esc_situacao
            , Guid tua_id
            , Guid ent_id
            , Guid uad_idSuperior
            , Guid gru_id
            , Guid usu_id
            , out int totalRecords
            , string TIPO_MEIOCONTATO_TELEFONE
            , int cur_id
            , int crr_id
            , int tce_id
        )
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_Escola_SelectBy_Pesquisa", _Banco);
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
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@esc_nome";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(esc_nome))
                    Param.Value = esc_nome;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@esc_codigo";
                Param.Size = 20;
                if (!string.IsNullOrEmpty(esc_codigo))
                    Param.Value = esc_codigo;
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
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                if (ent_id == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = ent_id;
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
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@esc_telefone";
                Param.Size = 20;
                if (!string.IsNullOrEmpty(TIPO_MEIOCONTATO_TELEFONE))
                    Param.Value = TIPO_MEIOCONTATO_TELEFONE;
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
                Param.ParameterName = "@tce_id";
                Param.Size = 4;
                if (tce_id > 0)
                    Param.Value = tce_id;
                else
                    Param.Value = DBNull.Value;
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
        /// Retorna um datatable contendo todos os usuarios
        /// das escolas que não foram excluídas logicamente
        /// </summary>
        /// <param name="cur_id"></param>
        /// <param name="tua_id"></param>
        /// <param name="ent_id"></param>
        /// <param name="uad_idSuperior"></param>
        /// <param name="usu_id"></param>
        /// <param name="gru_id"></param>
        /// <param name="sis_id"></param>
        /// <param name="gru_idPadraoSistema"></param>
        /// <returns>DataTable com os registros selecionados</returns>
        public DataTable Select_Usuarios
        (
            int cur_id
            , Guid tua_id
            , Guid ent_id
            , Guid uad_idSuperior
            , Guid gru_id
            , Guid usu_id
            , int sis_id
            , Guid gru_idPadraoSistema
        )
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_Escola_Select_Usuarios", _Banco);
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
                Param.ParameterName = "@tua_id";
                Param.Size = 16;
                if (tua_id == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = tua_id;
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
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                if (uad_idSuperior == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = uad_idSuperior;
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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@sis_id";
                Param.Size = 4;
                if (sis_id <= 0)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = sis_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@gru_idPadraoSistema";
                Param.Size = 16;
                if (gru_id == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = gru_idPadraoSistema;
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
        /// Retorna o nome da escola filtrado pelo código da escola
        /// </summary>
        /// <param name="esc_codigo">Código da escola</param>
        /// <returns>DataTable com os registros selecionados</returns>
        public string SelecionaNomeEscolaPorCodigoEscola
        (
           string esc_codigo
        )
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_Escola_SelecionaNomeCodigoEscola", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@esc_codigo";
                Param.Size = 20;
                Param.Value = esc_codigo;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                    return qs.Return.Rows[0][0].ToString();
                else
                    return string.Empty;
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
        /// Retorna um datatable contendo todos as escolas
        /// que não foram excluídas logicamente
        /// </summary>
        /// <param name="cur_id">Id da tabela ACA_Curso do bd</param>
        /// <param name="tua_id">Tipo de Escola (UA)</param>
        /// <param name="ent_id">Campo ent_id da tabela ESC_Escola</param>
        /// <param name="uad_idSuperior"></param>
        /// <param name="usu_id"></param>
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        /// <param name="totalRecords">Total de registros retornado na busca</param>
        /// <param name="gru_id"></param>
        /// <returns>DataTable com os registros selecionados</returns>
        public DataTable SelectBy_cur_id
        (
            int cur_id
            , Guid tua_id
            , Guid ent_id
            , Guid uad_idSuperior
            , Guid gru_id
            , Guid usu_id
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_Escola_SelectBy_cur_id", _Banco);
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
                Param.ParameterName = "@tua_id";
                Param.Size = 16;
                if (tua_id == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = tua_id;
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
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                if (uad_idSuperior == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = uad_idSuperior;
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
        /// Retorna um datatable com todas as Unidades administrativas que o usuário tem acesso
        /// no grupo.
        /// </summary>
        /// <param name="gru_id"></param>
        /// <param name="usu_id"></param>
        /// <returns></returns>
        public DataTable Select_Uad_Ids_By_PermissaoUsuario(Guid gru_id, Guid usu_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_Escola_Select_Uad_Ids_By_Permissao", _Banco);

            try
            {
                DataTable dt = new DataTable();

                #region Parâmetros

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

                #endregion Parâmetros

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
        /// Método para verificar se já existe o código da escola
        /// e preencher a entity.
        /// </summary>
        /// <param name="entity"> Entidade escola</param>
        /// <returns> true = Encontrou código igual | false = Não encontrou código igual</returns>
        public bool SelectBy_Codigo(ESC_Escola entity)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_Escola_SelectBy_Codigo", _Banco);
            try
            {
                #region PARAMETROS

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
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@esc_codigo";
                Param.Size = 20;
                Param.Value = entity.esc_codigo;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = entity.ent_id;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                if (qs.Return.Rows.Count == 1)
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
        /// Seleciona a escola pelo código para número de matrícula.
        /// </summary>
        /// <param name="ent_id">Id da entidade.</param>
        /// <param name="esc_codigoNumeroMatricula">Código para número de matrícula.</param>
        /// <returns>Escola.</returns>
        public ESC_Escola ConsultarCodigoNumeroMatricula(Guid ent_id, int esc_codigoNumeroMatricula)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_Escola_SelectBy_CodigoNumeroMatricula", _Banco);
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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_codigoNumeroMatricula";
                Param.Size = 4;
                Param.Value = esc_codigoNumeroMatricula;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                ESC_Escola entity = new ESC_Escola();
                if (qs.Return.Rows.Count == 1)
                {
                    entity = DataRowToEntity(qs.Return.Rows[0], entity);
                }
                return entity;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Sobrecarga do método SelecBy_Codigo que só seleciona
        /// a escola pelo código e não preenche a entity.
        /// </summary>
        /// <param name="esc_id"></param>
        /// <param name="esc_codigo">Código da escola</param>
        /// <param name="ent_id">Id da entidade</param>
        /// <returns>True = Encontrou código igual  | false = Não encontrou código igual</returns>
        public bool SelectBy_Codigo(Int32 esc_id, string esc_codigo, Guid ent_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_Escola_SelectBy_Codigo", _Banco);
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
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@esc_codigo";
                Param.Size = 20;
                Param.Value = esc_codigo;
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
        /// Sobrecarga do método SelecBy_Codigo que só seleciona
        /// a escola pelo código de integração e não preenche a entity.
        /// </summary>
        /// <param name="esc_id"></param>
        /// <param name="codigoIntegracao">Código de integração da escola</param>
        /// <param name="ent_id">Id da entidade</param>
        /// <returns>True = Encontrou código igual  | false = Não encontrou código igual</returns>
        public bool SelectBy_CodigoIntegracao(Int32 esc_id, string codigoIntegracao, Guid ent_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_Escola_SelectBy_CodigoIntegracao", _Banco);
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
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@codigoIntegracao";
                Param.Size = 50;
                Param.Value = codigoIntegracao;
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
        /// Seleciona as informaçoes da escola de acordo com
        /// o uad_id informado e preenche a entidade de escola
        /// </summary>
        /// <param name="entity">entidade escola a ser preenchida</param>
        /// <returns>True = Encontrou código igual  | false = Não encontrou código igual</returns>

        public bool SelectBy_UAD(ESC_Escola entity)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_Escola_SelectBy_uad_id", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_id";
                Param.Size = 16;
                Param.Value = entity.uad_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = entity.ent_id;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                if (qs.Return.Rows.Count == 1)
                {
                    entity = DataRowToEntity(qs.Return.Rows[0], entity, false);
                    return true;
                }
                return false;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }
                
        /// <summary>
        ///Buscas as escolas conforme o filtro passado
        /// </summary>
        /// <param name="esc_nome">nome da escola</param>
        /// <param name="esc_codigo">codigo da escola</param>
        /// <param name="uad_idSuperior">id da unidade administraviva superior</param>
        /// <param name="usu_id">id do usuario logado</param>
        /// <param name="gru_id">id do grupo do usuario logado</param>
        /// <param name="paginado"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public DataTable SelectBy_NomeEscola_CodEscola
        (
            string esc_nome
            , string esc_codigo
            , Guid uad_idSuperior
            , Guid usu_id
            , Guid gru_id
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_Escola_SelectBy_NomeEscola_CodEscola", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@esc_nome";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(esc_nome))
                    Param.Value = esc_nome;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@esc_codigo";
                Param.Size = 20;
                if (!string.IsNullOrEmpty(esc_codigo))
                    Param.Value = esc_codigo;
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
                Param.ParameterName = "@usu_id";
                Param.Size = 16;
                if (usu_id == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = usu_id;
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
        ///Buscas as escolas por permissao do usuario
        /// </summary>
        /// <param name="usu_id">id do usuario logado</param>
        /// <param name="gru_id">id do grupo do usuario logado</param>
        /// <param name="uad_idSuperior">id da uad superior</param>
        /// <param name="orgEscolaCodigo">Se vai mostrar e ordenar as uad pelo codigo</param>
        /// <param name="totalRecords"></param>
        /// <returns>DataTable com os registros</returns>
        public DataTable SelectBy_PermissaoDoUsuario
        (
            Guid usu_id
            , Guid gru_id
            , Guid uad_idSuperior
            , bool orgEscolaCodigo
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_Escola_PorPermissaoDoUsuario", _Banco);
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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                if (uad_idSuperior == Guid.Empty)
                    Param.Value = DBNull.Value;
                else
                    Param.Value = uad_idSuperior;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@orgEscolaCodigo";
                Param.Size = 1;
                Param.Value = orgEscolaCodigo;
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
        /// O método verifica se já existe uma escola com o mesmo código para número de matrícula
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="esc_codigoNumeroMatricula">Código para número de matrícula da escola</param>
        /// <param name="ent_id"></param>
        /// <returns>True se já existe</returns>
        public bool VerificaExistentePorCodigoNumeroMatricula(int esc_id, int esc_codigoNumeroMatricula, Guid ent_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_Escola_VerificaExistentePorCodigoNumeroMatricula", _Banco);

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
                Param.ParameterName = "@esc_codigoNumeroMatricula";
                Param.Size = 4;
                if (esc_codigoNumeroMatricula > 0)
                    Param.Value = esc_codigoNumeroMatricula;
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

                return qs.Return.Rows.Count > 0;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Seleciona a escola ou UASuperior de acordo com a permissão do grupo do usuário na entidade
        /// </summary>
        /// <param name="usu_id">ID do usuário</param>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="gru_id">ID do grupo</param>
        /// <returns>DataTable com o registro selecionado</returns>
        public DataTable RetornaUAPermissaoUsuarioGrupo(Guid usu_id, Guid ent_id, Guid gru_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_Escola_RetornaUAPermissaoUsuarioGrupo", _Banco);

            try
            {
                DataTable dt = new DataTable();

                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@usu_id";
                Param.Size = 16;
                Param.Value = usu_id;
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

                #endregion Parâmetros

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
        /// Verifica se a escola possui mais de uma unidade administrativa superior
        /// </summary>
        /// <param name="esc_id">ID da escola.</param>
        /// <returns></returns>
        public bool VerificaPossuiOutraUnidadeAdministrativa(int esc_id)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_ESC_Escola_VerificaPossuiOutraUnidadeAdministrativa", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                Param.Value = esc_id;
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
        /// Retorna um datatable contendo todas as escolas a partir do cargo do colaborador. Usado na visão individual.
        /// Utilizado na tela: Atribuição do docente.
        /// </summary>
        /// <param name="col_id">Id do colaborador.</param>
        /// <param name="crg_id">Id do cargo.</param>
        /// <param name="coc_id">Id do cargo do colaborador.</param>
        /// <returns>DataTable com as escolas.</returns>
        public DataTable SelecionaPorColaboradorCargoComHierarquia(long col_id, int crg_id, int coc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_Escola_SelecionaPorColaboradorCargoComHierarquia", _Banco);
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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crg_id";
                Param.Size = 4;
                Param.Value = crg_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@coc_id";
                Param.Size = 4;
                Param.Value = coc_id;
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
        /// Retorna um datatable contendo todas as disciplinas e suas respectivas turmas que estao sem docentes e sem a marcacao da flag sem docente.
        /// Utilizado na tela: confirmação de fechamento do coc.
        /// </summary>
        /// <param name="esc_id">Id da escola.</param>
        /// <param name="esc_id">Id do calendario.</param>
        /// <returns>DataTable com as disciplinas da escola sem docente e sem marcacao de flag que esta sem docente.</returns>
        public DataTable SelecionaDisciplinasSemDocente(int esc_id, int cal_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_Escola_ValidacaoTumaSemProfessor", _Banco);
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

        #endregion Métodos

        #region Métodos Sobrescritos

        /// <summary>
        /// Sobrecarga do método carregar, que traz o campo UAd_IDSuperior.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Carregar(ESC_Escola entity)
        {
            __STP_LOAD = "NEW_ESC_Escola_LOAD";
            return base.Carregar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, ESC_Escola entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@esc_id";
            Param.Size = 4;
            Param.Value = entity.esc_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@esc_controleSistema";
            Param.Size = 1;
            Param.Value = entity.esc_controleSistema;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@esc_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity"> Entidade ESC_Escola</param>
        /// <returns>true = sucesso | false = fracasso</returns>
        protected override bool Alterar(ESC_Escola entity)
        {
            __STP_UPDATE = "NEW_ESC_Escola_UPDATE";
            return base.Alterar(entity);
        }

        #endregion Métodos Sobrescritos

        #region Métodos para verificar integridade

        /// <summary>
        /// Verifica a existência da chave informada (1 campo) nas tabelas do sistema, exceto nas tabelas
        /// que estiverem no parâmetro tabelasNaoVerificar. Retorna true se a chave estiver sendo usada.
        /// </summary>
        /// <param name="campo1">Nome da coluna 1 da chave</param>
        /// <param name="valorCampo1">Valor da coluna 1 da chave</param>
        /// <param name="tabelasNaoVerificar">Tabelas que não serão verificadas (separadas por ",")</param>
        /// <returns>Flag que indica se chave está sendo usada em outros lugares</returns>
        public bool Select_VerificarIntegridade
        (
            string campo1
            , string valorCampo1
            , string tabelasNaoVerificar
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW__Select_VerificarIntegridade", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@campo1";
                Param.Value = campo1;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@valorCampo1";
                Param.Value = valorCampo1;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@tabelasNaoVerificar";
                Param.Value = tabelasNaoVerificar;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                if (Convert.ToInt32(qs.Return.Rows[0][0]) > 0)
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
        /// Verifica a existência da chave informada (2 campos) nas tabelas do sistema, exceto nas tabelas
        /// que estiverem no parâmetro tabelasNaoVerificar. Retorna true se a chave estiver sendo usada.
        /// </summary>
        /// <param name="campo1">Nome da coluna 1 da chave</param>
        /// <param name="campo2">Nome da coluna 2 da chave</param>
        /// <param name="valorCampo1">Valor da coluna 1 da chave</param>
        /// <param name="valorCampo2">Valor da coluna 2 da chave</param>
        /// <param name="tabelasNaoVerificar">Tabelas que não serão verificadas (separadas por ",")</param>
        /// <returns>Flag que indica se chave está sendo usada em outros lugares</returns>
        public bool VerificaIntegridadaChaveDupla
        (
            string campo1
            , string campo2
            , string valorCampo1
            , string valorCampo2
            , string tabelasNaoVerificar
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW__Select_VerificarIntegridade_ChaveDupla", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@campo1";
            Param.Value = campo1;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@campo2";
            Param.Value = campo2;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@valorCampo1";
            Param.Value = valorCampo1;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@valorCampo2";
            Param.Value = valorCampo2;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@tabelasNaoVerificar";
            Param.Value = tabelasNaoVerificar;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return Convert.ToInt32(qs.Return.Rows[0][0]) > 0;
        }

        /// <summary>
        /// Verifica a existência da chave informada (3 campos) nas tabelas do sistema, exceto nas tabelas
        /// que estiverem no parâmetro tabelasNaoVerificar. Retorna true se a chave estiver sendo usada.
        /// </summary>
        /// <param name="campo1">Nome da coluna 1 da chave</param>
        /// <param name="campo2">Nome da coluna 2 da chave</param>
        /// <param name="campo3">Nome da coluna 3 da chave</param>
        /// <param name="valorCampo1">Valor da coluna 1 da chave</param>
        /// <param name="valorCampo2">Valor da coluna 2 da chave</param>
        /// <param name="valorCampo3">Valor da coluna 3 da chave</param>
        /// <param name="tabelasNaoVerificar">Tabelas que não serão verificadas (separadas por ",")</param>
        /// <returns>Flag que indica se chave está sendo usada em outros lugares</returns>
        public bool VerificaIntegridadaChaveTripla
        (
            string campo1
            , string campo2
            , string campo3
            , string valorCampo1
            , string valorCampo2
            , string valorCampo3
            , string tabelasNaoVerificar
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW__Select_VerificarIntegridade_ChaveTripla", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@campo1";
            Param.Value = campo1;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@campo2";
            Param.Value = campo2;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@campo3";
            Param.Value = campo3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@valorCampo1";
            Param.Value = valorCampo1;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@valorCampo2";
            Param.Value = valorCampo2;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@valorCampo3";
            Param.Value = valorCampo3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@tabelasNaoVerificar";
            Param.Value = tabelasNaoVerificar;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return Convert.ToInt32(qs.Return.Rows[0][0]) > 0;
        }

        /// <summary>
        /// Verifica a existência da chave informada (4 campos) nas tabelas do sistema, exceto nas tabelas
        /// que estiverem no parâmetro tabelasNaoVerificar. Retorna true se a chave estiver sendo usada.
        /// </summary>
        /// <param name="campo1">Nome da coluna 1 da chave</param>
        /// <param name="campo2">Nome da coluna 2 da chave</param>
        /// <param name="campo3">Nome da coluna 3 da chave</param>
        /// <param name="valorCampo1">Valor da coluna 1 da chave</param>
        /// <param name="valorCampo2">Valor da coluna 2 da chave</param>
        /// <param name="valorCampo3">Valor da coluna 3 da chave</param>
        /// <param name="tabelasNaoVerificar">Tabelas que não serão verificadas (separadas por ",")</param>
        /// <returns>Flag que indica se chave está sendo usada em outros lugares</returns>
        public bool VerificaIntegridadaChaveTetra
        (
            string campo1
            , string campo2
            , string campo3
            , string campo4
            , string valorCampo1
            , string valorCampo2
            , string valorCampo3
            , string valorCampo4
            , string tabelasNaoVerificar
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW__Select_VerificarIntegridade_ChaveTetra", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@campo1";
            Param.Value = campo1;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@campo2";
            Param.Value = campo2;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@campo3";
            Param.Value = campo3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@campo4";
            Param.Value = campo4;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@valorCampo1";
            Param.Value = valorCampo1;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@valorCampo2";
            Param.Value = valorCampo2;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@valorCampo3";
            Param.Value = valorCampo3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@valorCampo4";
            Param.Value = valorCampo4;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@tabelasNaoVerificar";
            Param.Value = tabelasNaoVerificar;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return Convert.ToInt32(qs.Return.Rows[0][0]) > 0;
        }

        #endregion Métodos para verificar integridade

        #region Comentados

        ///// <summary>
        ///// Inseri os valores da classe em um registro ja existente
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem modificados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Alterar(ESC_Escola entity)
        //{
        //    return base.Alterar(entity);
        //}
        ///// <summary>
        ///// Inseri os valores da classe em um novo registro
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem inseridos</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Inserir(ESC_Escola entity)
        //{
        //    return base.Inserir(entity);
        //}
        ///// <summary>
        ///// Carrega um registro da tabela usando os valores nas chaves
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem carregados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Carregar(ESC_Escola entity)
        //{
        //    return base.Carregar(entity);
        //}
        ///// <summary>
        ///// Exclui um registro do banco
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem apagados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Delete(ESC_Escola entity)
        //{
        //    return base.Delete(entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Alterar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamAlterar(QueryStoredProcedure qs, ESC_Escola entity)
        //{
        //    base.ParamAlterar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Carregar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamCarregar(QuerySelectStoredProcedure qs, ESC_Escola entity)
        //{
        //    base.ParamCarregar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Deletar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamDeletar(QueryStoredProcedure qs, ESC_Escola entity)
        //{
        //    base.ParamDeletar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Inserir
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamInserir(QuerySelectStoredProcedure qs, ESC_Escola entity)
        //{
        //    base.ParamInserir(qs, entity);
        //}
        ///// <summary>
        ///// Salva o registro no banco de dados
        ///// </summary>
        ///// <param name="entity">Entidade com os dados para preenchimento para inserir ou alterar</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Salvar(ESC_Escola entity)
        //{
        //    return base.Salvar(entity);
        //}
        ///// <summary>
        ///// Realiza o select da tabela
        ///// </summary>
        ///// <returns>Lista com todos os registros da tabela</returns>
        //public override IList<ESC_Escola> Select()
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
        //public override IList<ESC_Escola> Select_Paginado(int currentPage, int pageSize, out int totalRecord)
        //{
        //    return base.Select_Paginado(currentPage, pageSize, out totalRecord);
        //}
        ///// <summary>
        ///// Recebe o valor do auto incremento e coloca na propriedade
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ESC_Escola entity)
        //{
        //    return base.ReceberAutoIncremento(qs, entity);
        //}
        ///// <summary>
        ///// Passa os dados de um datatable para uma entidade
        ///// </summary>
        ///// <param name="dr">DataRow do datatable preenchido</param>
        ///// <param name="entity">Entidade onde ser�o transferidos os dados</param>
        ///// <returns>Entidade preenchida</returns>
        //public override ESC_Escola DataRowToEntity(DataRow dr, ESC_Escola entity)
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
        //public override ESC_Escola DataRowToEntity(DataRow dr, ESC_Escola entity, bool limparEntity)
        //{
        //    return base.DataRowToEntity(dr, entity, limparEntity);
        //}

        #endregion Comentados
    }
}