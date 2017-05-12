/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using Entities;
    using System;
    using System.Data;    /// <summary>
                          /// Description: .
                          /// </summary>
    public class SYS_UsuarioAPIDAO : Abstract_SYS_UsuarioAPIDAO
	{
        #region Métodos de consulta

        /// <summary>
        /// Verifica se existe o usuário por login e senha.
        /// </summary>
        /// <param name="uap_usuario"></param>
        /// <param name="uap_senha"></param>
        /// <returns></returns>
        public bool AutenticarUsuario(string uap_usuario, string uap_senha)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_SYS_UsuarioAPI_VerificaUsuarioSenha", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.ParameterName = "@uap_usuario";
                Param.DbType = DbType.String;
                Param.Size = 100;
                Param.Value = uap_usuario;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@uap_senha";
                Param.DbType = DbType.String;
                Param.Size = 256;
                Param.Value = uap_senha;
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

        #endregion Métodos de consulta

        #region Métodos sobrescritos

        protected override void ParamInserir(QuerySelectStoredProcedure qs, SYS_UsuarioAPI entity)
        {
            entity.uap_dataCriacao = entity.uap_dataAlteracao = DateTime.Now;
            base.ParamInserir(qs, entity);
        }

        protected override void ParamAlterar(QueryStoredProcedure qs, SYS_UsuarioAPI entity)
        {
            entity.uap_dataAlteracao = DateTime.Now;
            base.ParamAlterar(qs, entity);
            qs.Parameters.RemoveAt("@uap_dataCriacao");
        }

        protected override bool Alterar(SYS_UsuarioAPI entity)
        {
            __STP_UPDATE = "NEW_SYS_UsuarioAPI_UPDATE";
            return base.Alterar(entity);
        }

        protected override void ParamDeletar(QueryStoredProcedure qs, SYS_UsuarioAPI entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@uap_id";
                Param.Size = 4;
                Param.Value = entity.uap_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@uap_situacao";
                Param.Size = 3;
                Param.Value = entity.uap_situacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@uap_dataAlteracao";

                Param.Value = DateTime.Now;
                qs.Parameters.Add(Param);
            }
        }

        public override bool Delete(SYS_UsuarioAPI entity)
        {
            __STP_DELETE = "NEW_SYS_UsuarioAPI_UpdateSituacao";
            return base.Delete(entity);
        }

        #endregion Métodos sobrescritos
    }
}