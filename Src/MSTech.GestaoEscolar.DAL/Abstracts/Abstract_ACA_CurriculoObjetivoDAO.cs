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
    /// Classe abstrata de ACA_CurriculoObjetivo.
    /// </summary>
    public abstract class Abstract_ACA_CurriculoObjetivoDAO : Abstract_DAL<ACA_CurriculoObjetivo>
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
        protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_CurriculoObjetivo entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cro_id";
                Param.Size = 4;
                Param.Value = entity.cro_id;
                qs.Parameters.Add(Param);


            }
        }

        /// <summary>
        /// Configura os parametros do metodo de Inserir.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_CurriculoObjetivo entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tne_id";
                Param.Size = 4;
                Param.Value = entity.tne_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tme_id";
                Param.Size = 4;
                Param.Value = entity.tme_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tds_id";
                Param.Size = 4;
                Param.Value = entity.tds_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tcp_id";
                Param.Size = 4;
                Param.Value = entity.tcp_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_ano";
                Param.Size = 4;
                Param.Value = entity.cal_ano;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@cro_descricao";
                Param.Size = 500;
                Param.Value = entity.cro_descricao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cro_ordem";
                Param.Size = 4;
                Param.Value = entity.cro_ordem;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@cro_tipo";
                Param.Size = 1;
                Param.Value = entity.cro_tipo;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cro_idPai";
                Param.Size = 4;
                if (entity.cro_idPai > 0)
                {
                    Param.Value = entity.cro_idPai;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@cro_situacao";
                Param.Size = 1;
                Param.Value = entity.cro_situacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@cro_dataCriacao";
                Param.Size = 16;
                Param.Value = entity.cro_dataCriacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@cro_dataAlteracao";
                Param.Size = 16;
                Param.Value = entity.cro_dataAlteracao;
                qs.Parameters.Add(Param);


            }
        }

        /// <summary>
        /// Configura os parametros do metodo de Alterar.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_CurriculoObjetivo entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cro_id";
                Param.Size = 4;
                Param.Value = entity.cro_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tne_id";
                Param.Size = 4;
                Param.Value = entity.tne_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tme_id";
                Param.Size = 4;
                Param.Value = entity.tme_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tds_id";
                Param.Size = 4;
                Param.Value = entity.tds_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tcp_id";
                Param.Size = 4;
                Param.Value = entity.tcp_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_ano";
                Param.Size = 4;
                Param.Value = entity.cal_ano;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@cro_descricao";
                Param.Size = 500;
                Param.Value = entity.cro_descricao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cro_ordem";
                Param.Size = 4;
                Param.Value = entity.cro_ordem;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@cro_tipo";
                Param.Size = 1;
                Param.Value = entity.cro_tipo;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cro_idPai";
                Param.Size = 4;
                if (entity.cro_idPai > 0)
                {
                    Param.Value = entity.cro_idPai;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@cro_situacao";
                Param.Size = 1;
                Param.Value = entity.cro_situacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@cro_dataCriacao";
                Param.Size = 16;
                Param.Value = entity.cro_dataCriacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@cro_dataAlteracao";
                Param.Size = 16;
                Param.Value = entity.cro_dataAlteracao;
                qs.Parameters.Add(Param);


            }
        }

        /// <summary>
        /// Configura os parametros do metodo de Deletar.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamDeletar(QueryStoredProcedure qs, ACA_CurriculoObjetivo entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cro_id";
                Param.Size = 4;
                Param.Value = entity.cro_id;
                qs.Parameters.Add(Param);


            }
        }

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        /// <returns>TRUE - Se entity.ParametroId > 0</returns>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_CurriculoObjetivo entity)
        {
            if (entity != null & qs != null)
            {
                entity.cro_id = Convert.ToInt32(qs.Return.Rows[0][0]);
                return (entity.cro_id > 0);
            }

            return false;
        }
    }
}