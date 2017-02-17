using System;
using System.Data;
using System.Data.Common;

using MSTech.Data.Common;

namespace MSTech.GestaoEscolar.DAL
{
    /// <summary>
    /// Classe de acesso ao banco de dados
    /// </summary>
    public class UsuarioDAO : Persistent
    {
        #region Atributos

        /// <summary>
        /// Gets or sets Param.
        /// </summary>
        protected DbParameter Param { get; set; }

        #endregion

        #region Propriedades

        /// <summary> 
        /// Indica o nome da conexão da classe.
        /// </summary>
        protected override string ConnectionStringName
        {
            get
            {
                return "GestaoEscolar";
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Retorna a quantidade total de docente, colaborador ou aluno
        /// com usuário e sem usuário. 
        /// </summary>        
        /// <param name="tipo">Tipo de busca (1 - Docente / 2 - Colaborador / 3 - Aluno).</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <param name="adm">Indica se o usuário é administrador</param>
        /// <param name="usu_id">Id do usuário logado</param>
        /// <param name="gru_id">Id do grupo do usuário logado</param>
        /// <param name="qtdComUsuario">Retorna a quantidade total de docente, colaborador ou aluno com usuários.</param>
        /// <param name="qtdSemUsuario">Retorna a quantidade total de docente, colaborador ou aluno sem usuários.</param>
        public void Select_TotalUsuarios
        (
            byte tipo
            , Guid ent_id
            , bool adm
            , Guid usu_id
            , Guid gru_id
            , out int qtdComUsuario
            , out int qtdSemUsuario
        )
        {
            qtdComUsuario = 0;
            qtdSemUsuario = 0;

            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_Usuario_Select_TotalUsuarios", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tipo";
                Param.Size = 1;
                Param.Value = tipo;
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

                #endregion

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                {
                    int.TryParse(qs.Return.Rows[0]["TotalComUsuario"].ToString(), out qtdComUsuario);
                    int.TryParse(qs.Return.Rows[0]["TotalSemUsuario"].ToString(), out qtdSemUsuario);
                }
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna os colaboradores para redefinição de grupos ou criação de usuários
        /// </summary>
        /// <param name="uad_idSuperior">Id da unidade administrativa superior</param>
        /// <param name="uad_id">Id da unidade administrativa</param>
        /// <param name="crg_id">ID do cargo</param>
        /// <param name="fun_id">ID da função</param>
        /// <param name="comUsuario">Indica se vai trazer os colaboradres com usuários</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <param name="adm">Indica se o usuário é administrador</param>
        /// <param name="usu_id">Id do usuário logado</param>
        /// <param name="gru_id">Id do grupo do usuário logado</param>        
        public DataTable Select_Colaborador
        (            
            Guid uad_idSuperior
            , Guid uad_id
            , int crg_id
            , int fun_id
            , bool comUsuario
            , Guid ent_id
            , bool adm
            , Guid usu_id
            , Guid gru_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_Usuario_Select_Colaborador", _Banco);

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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_id";
                Param.Size = 16;
                if (uad_id != Guid.Empty)
                    Param.Value = uad_id;
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
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@comUsuario";
                Param.Size = 1;
                Param.Value = comUsuario;
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
        /// Retorna os docentes para redefinição de grupos ou criação de usuários
        /// </summary>
        /// <param name="uad_idSuperior">Id da unidade administrativa superior</param>
        /// <param name="uad_id">Id da unidade administrativa</param>
        /// <param name="crg_id">ID do cargo</param>
        /// <param name="fun_id">ID da função</param>
        /// <param name="comUsuario">Indica se vai trazer os colaboradres com usuários</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <param name="adm">Indica se o usuário é administrador</param>
        /// <param name="usu_id">Id do usuário logado</param>
        /// <param name="gru_id">Id do grupo do usuário logado</param>        
        public DataTable Select_Docente
        (
            Guid uad_idSuperior
            , Guid uad_id
            , int crg_id
            , int fun_id
            , bool comUsuario
            , Guid ent_id
            , bool adm
            , Guid usu_id
            , Guid gru_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_Usuario_Select_Docente", _Banco);

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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_id";
                Param.Size = 16;
                if (uad_id != Guid.Empty)
                    Param.Value = uad_id;
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
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@comUsuario";
                Param.Size = 1;
                Param.Value = comUsuario;
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
        /// Retorna os alunos para redefinição de grupos ou criação de usuários
        /// </summary>
        /// <param name="uad_idSuperior">Id da unidade administrativa superior</param>        
        /// <param name="esc_id">Id da escola</param>
        /// <param name="uni_id">Id da unidade da escola</param>
        /// <param name="cur_id">Id do curso</param>
        /// <param name="crr_id">Id do curriculo do curso</param>  
        /// <param name="crp_id">Id do período do curriculo</param>
        /// <param name="comUsuario">Indica se vai trazer os colaboradres com usuários</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <param name="adm">Indica se o usuário é administrador</param>
        /// <param name="usu_id">Id do usuário logado</param>
        /// <param name="gru_id">Id do grupo do usuário logado</param>        
        public DataTable Select_Aluno
        (
            Guid uad_idSuperior            
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , bool comUsuario
            , Guid ent_id
            , bool adm
            , Guid usu_id
            , Guid gru_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_Usuario_Select_Aluno", _Banco);

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
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@comUsuario";
                Param.Size = 1;
                Param.Value = comUsuario;
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
        /// O método seleciona os usuários cadastrados no sistema pelo login.
        /// </summary>
        /// <param name="usu_login">Login do usuário.</param>
        /// <param name="adm">Indica se o usuário é administrador.</param>
        /// <param name="sis_id">ID do sistema.</param>
        /// <param name="usu_id">ID do usuário.</param>
        /// <param name="gru_id">ID do grupo do usuário.</param>
        /// <returns></returns>
        public DataTable SelecionaPorLogin
        (
            string usu_login,
            bool adm,
            int sis_id,
            Guid usu_id,
            Guid gru_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_Usuario_Select_Login", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@usu_login";
                Param.Size = 500;
                Param.Value = usu_login;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@adm";
                Param.Size = 1;
                Param.Value = adm;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@sis_id";
                Param.Size = 4;
                Param.Value = sis_id;
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
    }
}
