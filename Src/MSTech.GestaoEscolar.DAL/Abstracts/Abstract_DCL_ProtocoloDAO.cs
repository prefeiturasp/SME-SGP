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
    /// Classe abstrata de DCL_Protocolo.
    /// </summary>
    public abstract class Abstract_DCL_ProtocoloDAO : Abstract_DAL<DCL_Protocolo>
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
        protected override void ParamCarregar(QuerySelectStoredProcedure qs, DCL_Protocolo entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@pro_id";
                Param.Size = 16;
                Param.Value = entity.pro_id;
                qs.Parameters.Add(Param);


            }
        }

        /// <summary>
        /// Configura os parametros do metodo de Inserir.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, DCL_Protocolo entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@pro_id";
                Param.Size = 16;
                Param.Value = entity.pro_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@equ_id";
                Param.Size = 16;
                Param.Value = entity.equ_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@pro_tipo";
                Param.Size = 1;
                Param.Value = entity.pro_tipo;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@pro_protocolo";
                Param.Size = 8;
                Param.Value = entity.pro_protocolo;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pro_pacote";
                Param.Size = 2147483647;
                Param.Value = entity.pro_pacote;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@pro_status";
                Param.Size = 1;
                Param.Value = entity.pro_status;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pro_statusObservacao";
                Param.Size = 2147483647;
                if (!string.IsNullOrEmpty(entity.pro_statusObservacao))
                {
                    Param.Value = entity.pro_statusObservacao;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                if (entity.tur_id > 0)
                {
                    Param.Value = entity.tur_id;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                if (entity.tud_id > 0)
                {
                    Param.Value = entity.tud_id;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tau_id";
                Param.Size = 4;
                if (entity.tau_id > 0)
                {
                    Param.Value = entity.tau_id;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@pro_qtdeAlunos";
                Param.Size = 4;
                if (entity.pro_qtdeAlunos > 0)
                {
                    Param.Value = entity.pro_qtdeAlunos;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@pro_situacao";
                Param.Size = 1;
                Param.Value = entity.pro_situacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@pro_dataCriacao";
                Param.Size = 16;
                Param.Value = entity.pro_dataCriacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@pro_dataalteracao";
                Param.Size = 16;
                Param.Value = entity.pro_dataalteracao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@pro_tentativa";
                Param.Size = 4;
                if (entity.pro_tentativa > 0)
                {
                    Param.Value = entity.pro_tentativa;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pro_versaoAplicativo";
                Param.Size = 100;
                if (!string.IsNullOrEmpty(entity.pro_versaoAplicativo))
                {
                    Param.Value = entity.pro_versaoAplicativo;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.ParameterName = "@esc_id";
                if (entity.esc_id > 0)
                    Param.Value = entity.esc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);


            }
        }

        /// <summary>
        /// Configura os parametros do metodo de Alterar.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamAlterar(QueryStoredProcedure qs, DCL_Protocolo entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@pro_id";
                Param.Size = 16;
                Param.Value = entity.pro_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@equ_id";
                Param.Size = 16;
                Param.Value = entity.equ_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@pro_tipo";
                Param.Size = 1;
                Param.Value = entity.pro_tipo;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@pro_protocolo";
                Param.Size = 8;
                Param.Value = entity.pro_protocolo;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pro_pacote";
                Param.Size = 2147483647;
                Param.Value = entity.pro_pacote;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@pro_status";
                Param.Size = 1;
                Param.Value = entity.pro_status;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pro_statusObservacao";
                Param.Size = 2147483647;
                if (!string.IsNullOrEmpty(entity.pro_statusObservacao))
                {
                    Param.Value = entity.pro_statusObservacao;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                if (entity.tur_id > 0)
                {
                    Param.Value = entity.tur_id;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                if (entity.tud_id > 0)
                {
                    Param.Value = entity.tud_id;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tau_id";
                Param.Size = 4;
                if (entity.tau_id > 0)
                {
                    Param.Value = entity.tau_id;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@pro_qtdeAlunos";
                Param.Size = 4;
                if (entity.pro_qtdeAlunos > 0)
                {
                    Param.Value = entity.pro_qtdeAlunos;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@pro_situacao";
                Param.Size = 1;
                Param.Value = entity.pro_situacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@pro_dataCriacao";
                Param.Size = 16;
                Param.Value = entity.pro_dataCriacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@pro_dataalteracao";
                Param.Size = 16;
                Param.Value = entity.pro_dataalteracao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@pro_tentativa";
                Param.Size = 4;
                
                if (entity.pro_tentativa > 0)
                    Param.Value = entity.pro_tentativa;
                else
                    Param.Value = DBNull.Value;

                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pro_versaoAplicativo";
                Param.Size = 100;

                if (!string.IsNullOrEmpty(entity.pro_versaoAplicativo))
                    Param.Value = entity.pro_versaoAplicativo;
                else
                    Param.Value = DBNull.Value;
                
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.ParameterName = "@esc_id";
                
                if (entity.esc_id > 0)
                    Param.Value = entity.esc_id;
                else
                    Param.Value = DBNull.Value;
                
                qs.Parameters.Add(Param);
            }
        }

        /// <summary>
        /// Configura os parametros do metodo de Deletar.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamDeletar(QueryStoredProcedure qs, DCL_Protocolo entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@pro_id";
                Param.Size = 16;
                Param.Value = entity.pro_id;
                qs.Parameters.Add(Param);


            }
        }

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        /// <returns>TRUE - Se entity.ParametroId > 0</returns>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, DCL_Protocolo entity)
        {
            if (entity != null & qs != null)
            {

            }

            return false;
        }
    }
}