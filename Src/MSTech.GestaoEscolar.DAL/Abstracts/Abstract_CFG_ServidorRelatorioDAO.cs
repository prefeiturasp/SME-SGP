/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL.Abstracts
{
    using System;
    using System.Data;
    using MSTech.Data.Common;
    using MSTech.Data.Common.Abstracts;
    using MSTech.GestaoEscolar.Entities;

    /// <summary>
    /// Classe abstrata de CFG_ServidorRelatorio.
    /// </summary>
    public abstract class Abstract_CFG_ServidorRelatorioDAO : Abstract_DAL<CFG_ServidorRelatorio>
    {
        /// <summary>
        /// ConnectionString.
        /// </summary>
        protected override string ConnectionStringName
        {
            get
            {
                return "GestaoEscolar";
            }
        }

        /// <summary>
        /// Configura os parametros do metodo de carregar.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamCarregar(QuerySelectStoredProcedure qs, CFG_ServidorRelatorio entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = entity.ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@srr_id";
                Param.Size = 4;
                Param.Value = entity.srr_id;
                qs.Parameters.Add(Param);


            }
        }

        /// <summary>
        /// Configura os parametros do metodo de Inserir.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, CFG_ServidorRelatorio entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = entity.ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@srr_id";
                Param.Size = 4;
                Param.Value = entity.srr_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@srr_nome";
                Param.Size = 100;
                Param.Value = entity.srr_nome;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@srr_descricao";
                Param.Size = 1000;
                if (!string.IsNullOrEmpty(entity.srr_descricao))
                {
                    Param.Value = entity.srr_descricao;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@srr_remoteServer";
                Param.Size = 1;
                Param.Value = entity.srr_remoteServer;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@srr_usuario";
                Param.Size = 512;
                if (!string.IsNullOrEmpty(entity.srr_usuario))
                {
                    Param.Value = entity.srr_usuario;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@srr_senha";
                Param.Size = 512;
                if (!string.IsNullOrEmpty(entity.srr_senha))
                {
                    Param.Value = entity.srr_senha;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@srr_dominio";
                Param.Size = 512;
                if (!string.IsNullOrEmpty(entity.srr_dominio))
                {
                    Param.Value = entity.srr_dominio;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@srr_diretorioRelatorios";
                Param.Size = 1000;
                if (!string.IsNullOrEmpty(entity.srr_diretorioRelatorios))
                {
                    Param.Value = entity.srr_diretorioRelatorios;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@srr_pastaRelatorios";
                Param.Size = 1000;
                Param.Value = entity.srr_pastaRelatorios;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@srr_situacao";
                Param.Size = 1;
                Param.Value = entity.srr_situacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@srr_dataCriacao";
                Param.Size = 16;
                Param.Value = entity.srr_dataCriacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@srr_dataAlteracao";
                Param.Size = 16;
                Param.Value = entity.srr_dataAlteracao;
                qs.Parameters.Add(Param);


            }
        }

        /// <summary>
        /// Configura os parametros do metodo de Alterar.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamAlterar(QueryStoredProcedure qs, CFG_ServidorRelatorio entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = entity.ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@srr_id";
                Param.Size = 4;
                Param.Value = entity.srr_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@srr_nome";
                Param.Size = 100;
                Param.Value = entity.srr_nome;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@srr_descricao";
                Param.Size = 1000;
                if (!string.IsNullOrEmpty(entity.srr_descricao))
                {
                    Param.Value = entity.srr_descricao;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@srr_remoteServer";
                Param.Size = 1;
                Param.Value = entity.srr_remoteServer;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@srr_usuario";
                Param.Size = 512;
                if (!string.IsNullOrEmpty(entity.srr_usuario))
                {
                    Param.Value = entity.srr_usuario;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@srr_senha";
                Param.Size = 512;
                if (!string.IsNullOrEmpty(entity.srr_senha))
                {
                    Param.Value = entity.srr_senha;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@srr_dominio";
                Param.Size = 512;
                if (!string.IsNullOrEmpty(entity.srr_dominio))
                {
                    Param.Value = entity.srr_dominio;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@srr_diretorioRelatorios";
                Param.Size = 1000;
                if (!string.IsNullOrEmpty(entity.srr_diretorioRelatorios))
                {
                    Param.Value = entity.srr_diretorioRelatorios;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@srr_pastaRelatorios";
                Param.Size = 1000;
                Param.Value = entity.srr_pastaRelatorios;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@srr_situacao";
                Param.Size = 1;
                Param.Value = entity.srr_situacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@srr_dataCriacao";
                Param.Size = 16;
                Param.Value = entity.srr_dataCriacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@srr_dataAlteracao";
                Param.Size = 16;
                Param.Value = entity.srr_dataAlteracao;
                qs.Parameters.Add(Param);


            }
        }

        /// <summary>
        /// Configura os parametros do metodo de Deletar.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamDeletar(QueryStoredProcedure qs, CFG_ServidorRelatorio entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = entity.ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@srr_id";
                Param.Size = 4;
                Param.Value = entity.srr_id;
                qs.Parameters.Add(Param);


            }
        }

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        /// <returns>TRUE - Se entity.ParametroId > 0</returns>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CFG_ServidorRelatorio entity)
        {
            if (entity != null & qs != null)
            {
                return true;
            }

            return false;
        }
    }
}