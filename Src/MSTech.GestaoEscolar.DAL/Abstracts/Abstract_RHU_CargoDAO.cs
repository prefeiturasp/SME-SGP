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
    /// Classe abstrata de RHU_Cargo.
    /// </summary>
    public abstract class Abstract_RHU_CargoDAO : Abstract_DAL<RHU_Cargo>
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
        protected override void ParamCarregar(QuerySelectStoredProcedure qs, RHU_Cargo entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crg_id";
                Param.Size = 4;
                Param.Value = entity.crg_id;
                qs.Parameters.Add(Param);


            }
        }

        /// <summary>
        /// Configura os parametros do metodo de Inserir.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, RHU_Cargo entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@crg_codigo";
                Param.Size = 20;
                if (!string.IsNullOrEmpty(entity.crg_codigo))
                {
                    Param.Value = entity.crg_codigo;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@crg_nome";
                Param.Size = 100;
                Param.Value = entity.crg_nome;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Object;
                Param.ParameterName = "@crg_descricao";
                Param.Size = 2147483647;
                if (!string.IsNullOrEmpty(entity.crg_descricao))
                {
                    Param.Value = entity.crg_descricao;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tvi_id";
                Param.Size = 4;
                Param.Value = entity.tvi_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@crg_cargoDocente";
                Param.Size = 1;
                Param.Value = entity.crg_cargoDocente;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crg_maxAulaSemana";
                Param.Size = 1;
                if (entity.crg_maxAulaSemana > 0)
                {
                    Param.Value = entity.crg_maxAulaSemana;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crg_maxAulaDia";
                Param.Size = 1;
                if (entity.crg_maxAulaDia > 0)
                {
                    Param.Value = entity.crg_maxAulaDia;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@crg_codIntegracao";
                Param.Size = 20;
                if (!string.IsNullOrEmpty(entity.crg_codIntegracao))
                {
                    Param.Value = entity.crg_codIntegracao;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@crg_especialista";
                Param.Size = 1;
                Param.Value = entity.crg_especialista;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@crg_situacao";
                Param.Size = 1;
                Param.Value = entity.crg_situacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@crg_dataCriacao";
                Param.Size = 16;
                Param.Value = entity.crg_dataCriacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@crg_dataAlteracao";
                Param.Size = 16;
                Param.Value = entity.crg_dataAlteracao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = entity.ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pgs_chave";
                Param.Size = 100;
                if (!string.IsNullOrEmpty(entity.pgs_chave))
                {
                    Param.Value = entity.pgs_chave;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@crg_tipo";
                Param.Size = 1;
                Param.Value = entity.crg_tipo;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@crg_controleIntegracao";
                Param.Size = 1;
                Param.Value = entity.crg_controleIntegracao;
                qs.Parameters.Add(Param);
            }
        }

        /// <summary>
        /// Configura os parametros do metodo de Alterar.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamAlterar(QueryStoredProcedure qs, RHU_Cargo entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crg_id";
                Param.Size = 4;
                Param.Value = entity.crg_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@crg_codigo";
                Param.Size = 20;
                if (!string.IsNullOrEmpty(entity.crg_codigo))
                {
                    Param.Value = entity.crg_codigo;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@crg_nome";
                Param.Size = 100;
                Param.Value = entity.crg_nome;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Object;
                Param.ParameterName = "@crg_descricao";
                Param.Size = 2147483647;
                if (!string.IsNullOrEmpty(entity.crg_descricao))
                {
                    Param.Value = entity.crg_descricao;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tvi_id";
                Param.Size = 4;
                Param.Value = entity.tvi_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@crg_cargoDocente";
                Param.Size = 1;
                Param.Value = entity.crg_cargoDocente;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crg_maxAulaSemana";
                Param.Size = 1;
                if (entity.crg_maxAulaSemana > 0)
                {
                    Param.Value = entity.crg_maxAulaSemana;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crg_maxAulaDia";
                Param.Size = 1;
                if (entity.crg_maxAulaDia > 0)
                {
                    Param.Value = entity.crg_maxAulaDia;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@crg_codIntegracao";
                Param.Size = 20;
                if (!string.IsNullOrEmpty(entity.crg_codIntegracao))
                {
                    Param.Value = entity.crg_codIntegracao;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@crg_especialista";
                Param.Size = 1;
                Param.Value = entity.crg_especialista;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@crg_situacao";
                Param.Size = 1;
                Param.Value = entity.crg_situacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@crg_dataCriacao";
                Param.Size = 16;
                Param.Value = entity.crg_dataCriacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@crg_dataAlteracao";
                Param.Size = 16;
                Param.Value = entity.crg_dataAlteracao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = entity.ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pgs_chave";
                Param.Size = 100;
                if (!string.IsNullOrEmpty(entity.pgs_chave))
                {
                    Param.Value = entity.pgs_chave;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@crg_tipo";
                Param.Size = 1;
                Param.Value = entity.crg_tipo;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@crg_controleIntegracao";
                Param.Size = 1;
                Param.Value = entity.crg_controleIntegracao;
                qs.Parameters.Add(Param);
            }
        }

        /// <summary>
        /// Configura os parametros do metodo de Deletar.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamDeletar(QueryStoredProcedure qs, RHU_Cargo entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crg_id";
                Param.Size = 4;
                Param.Value = entity.crg_id;
                qs.Parameters.Add(Param);


            }
        }

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        /// <returns>TRUE - Se entity.ParametroId > 0</returns>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, RHU_Cargo entity)
        {
            if (entity != null & qs != null)
            {
                entity.crg_id = Convert.ToInt32(qs.Return.Rows[0][0]);
                return (entity.crg_id > 0);
            }

            return false;
        }
    }
}