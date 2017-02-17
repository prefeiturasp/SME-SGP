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
	/// 
	/// </summary>
	public class CFG_ParametroMensagemDAO : Abstract_CFG_ParametroMensagemDAO
    {
        #region Métodos

        /// <summary>
        /// Retorna todos os parâmetros cadastrados.
        /// </summary>
        /// <returns></returns>
        public DataTable Seleciona()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CFG_ParametroMensagem_Select", _Banco);

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Busca o valor do parametro disciplina pela chave.
        /// </summary>
        /// <param name="pms_chave">Chave de parametro mensagem</param>
        /// <returns></returns>
        public DataTable BuscaValoraPorChave(string pms_chave)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CFG_ParametroMensagem_SelecionaValorPor_Chave", _Banco);

            #region Parametros

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@pms_chave";
            Param.Size = 100;
            Param.Value = pms_chave;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        #endregion

        #region Sobrescritos

        /// <summary>
        /// String de conexão a ser utilizada.
        /// </summary>
        protected override string ConnectionStringName
        {
            get
            {
                return "GestaoEscolar";
            }
        }

        /// <summary>
        /// Retorna todos os parâmetros cadastrados.
        /// </summary>
        /// <returns></returns>
        public override IList<CFG_ParametroMensagem> Select()
        {
            __STP_SELECT = "NEW_CFG_ParametroMensagem_Select";
            return base.Select();
        }

        /// <summary>
        /// Configura os parâmetros de inclusão.
        /// </summary>
        /// <param name="qs">Stored procedure</param>
        /// <param name="entity">entidade com os dados a serem passados para a procedure</param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, CFG_ParametroMensagem entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@pms_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@pms_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Configura os parâmetros de alteração.
        /// </summary>
        /// <param name="qs">Stored procedure</param>
        /// <param name="entity">entidade com os dados a serem passados para a procedure</param>
        protected override void ParamAlterar(QueryStoredProcedure qs, CFG_ParametroMensagem entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@pms_dataCriacao");
            qs.Parameters["@pms_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Método de alteração.
        /// </summary>
        /// <param name="entity">Entidade a ser alterada</param>
        /// <returns></returns>
        protected override bool Alterar(CFG_ParametroMensagem entity)
        {
            __STP_UPDATE = "NEW_CFG_ParametroMensagem_Update";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Configura os parâmetros de exclusão.
        /// </summary>
        /// <param name="qs">Stored procedure</param>
        /// <param name="entity">entidade com os dados a serem passados para a procedure</param>
        protected override void ParamDeletar(QueryStoredProcedure qs, CFG_ParametroMensagem entity)
        {
            base.ParamDeletar(qs, entity);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@pms_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@pms_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método de alteração.
        /// </summary>
        /// <param name="entity">Entidade a ser excçuída</param>
        /// <returns></returns>
        public override bool Delete(CFG_ParametroMensagem entity)
        {
            __STP_DELETE = "NEW_CFG_ParametroMensagem_UpdateSituacao";
            return base.Delete(entity);
        }

        /// <summary>
        /// Método de inserir.
        /// </summary>
        /// <param name="entity">Entidade a ser alterada</param>
        /// <returns></returns>
        protected override bool Inserir(CFG_ParametroMensagem entity)
        {                    
            __STP_INSERT = "NEW_CFG_ParametroMensagem_INSERT";
            return base.Inserir(entity);
        }

        #endregion
    }
}