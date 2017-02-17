/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using MSTech.GestaoEscolar.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data;
	
	/// <summary>
	/// Description: Classe tipo área documento.
	/// </summary>
	public class ACA_TipoAreaDocumentoDAO : Abstract_ACA_TipoAreaDocumentoDAO
    {
        #region Métodos Sobrescritos

        /// <sumary>
        /// Parametros para efetuar a inclusão preservando a data de criação.
        /// </sumary>
        /// <param name="qs"></param>
        /// <param name="entity"></param> 
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_TipoAreaDocumento entity)
        {
            entity.tad_dataAlteracao = entity.tad_dataCriacao = DateTime.Now;
            base.ParamInserir(qs, entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação.
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_TipoAreaDocumento entity)
        {
            entity.tad_dataAlteracao = DateTime.Now;
            base.ParamAlterar(qs, entity);
            qs.Parameters.RemoveAt("@tad_dataCriacao");
        }

        /// <summary>
        /// Método alterado para que o update não faça alteração na data. 
        /// </summary>
        /// <param name="entity">Entidade ACA_TipoAreaDocumento</param>
        /// <returns>true = sucesso | false = fracasso</returns>
        protected override bool Alterar(ACA_TipoAreaDocumento entity)
        {
            __STP_UPDATE = "NEW_ACA_TipoAreaDocumento_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar exclusão lógica.
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamDeletar(QueryStoredProcedure qs, ACA_TipoAreaDocumento entity)
        {
            base.ParamDeletar(qs, entity);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tad_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@tad_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Sobrescreve o delete, para exclusão lógica.
        /// </summary>
        /// <param name="entity">Entidade ACA_TipoAreaDocumento</param>
        /// <returns>true = sucesso | false = fracasso</returns>
        public override bool Delete(ACA_TipoAreaDocumento entity)
        {
            __STP_DELETE = "NEW_ACA_TipoAreaDocumento_UPDATE_Situacao";
            return base.Delete(entity);
        }

        #endregion Métodos Sobrescritos

        #region Consultas

        /// <summary>
        /// Retorna os tipos de area documento ativos
        /// </summary>
        /// <param name="totalRecords">Total de registros da consulta</param>
        /// <returns>Lista com os tipos</returns>
        public DataTable SelecionarAreaDocumento(out int totalRecords)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoAreaDocumento_SelecionarAreaDocumento", _Banco);

            try
            {
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
        /// Verifica o maior número de ordem cadastado
        /// </summary>     
        public byte Select_MaiorOrdem()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoAreaDocumento_Select_MaiorOrdem", _Banco);
            try
            {
                qs.Execute();

                return Convert.ToByte(qs.Return.Rows.Count > 0 ? Convert.ToByte(qs.Return.Rows[0][0]) : 0);
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Verifica se já existe um tipo de area documento cadastrado com a mesma descrição
        /// </summary>
        /// <param name="tcp_id">ID do tipo de area documento</param>
        /// <param name="tcp_descricao">Nome do tipo de area documento</param>   
        public bool SelectBy_Nome
        (
            int tad_id
            , string tad_nome
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoAreaDocumento_SelectBy_Nome", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tad_id";
                Param.Size = 4;
                if (tad_id > 0)
                    Param.Value = tad_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tad_nome";
                Param.Size = 200;
                Param.Value = tad_nome;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return (qs.Return.Rows.Count > 0);
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }        

        /// <summary>
        /// Retorna se a area permite cadastro pela escola
        /// </summary>
        /// <param name="tad_id">ID da Area</param>
        /// <returns>Boolean</returns>
        public bool GetCadastroEscolaBy_tad_id
        (
            int tad_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoAreaDocumento_SelectCadastroEscolaBy_tad_id", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tad_id";
                Param.Size = 4;
                if (tad_id > 0)
                    Param.Value = tad_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                {
                    return Convert.ToBoolean(qs.Return.Rows[0][0]);
                }

                return false;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }
        
        /// <summary>
        /// Retorna os tipos de área documento ativos por permissão de usuário.
        /// </summary>
        /// <param name="admin">Indica se é um usuário administrador.</param>
        /// <param name="totalRecords">Total de registros da consulta</param>
        /// <returns></returns>
        public DataTable SelecionarAreaDocumentoPermissao(bool admin, out int totalRecords)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoAreaDocumento_SelecionarAreaDocumentoPermissao", _Banco);

            try
            {
                #region Parâmetro

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@admin";
                Param.Size = 1;
                Param.Value = admin;
                qs.Parameters.Add(Param);

                #endregion Parâmetro

                qs.Execute();

                totalRecords = qs.Return.Rows.Count;

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion Consultas

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
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Verifica se já existe um tipo de área documento cadastrado com o mesmo nome
        /// </summary>
        /// <param name="tad_id">ID do tipo de área</param>
        /// <param name="tad_nome">Nome do tipo de área</param>   
        public bool SelecionaNomeExistente
        (
            int tad_id
            , string tad_nome
        )
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_ACA_TipoAreaDocumento_SelectBy_Nome", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tad_id";
                Param.Size = 4;
                if (tad_id > 0)
                    Param.Value = tad_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tad_nome";
                Param.Size = 200;
                Param.Value = tad_nome;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return (qs.Return > 0);
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

    #endregion Métodos para verificar integridade
    }
}