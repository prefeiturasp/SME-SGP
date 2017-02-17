/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MSTech.Data.Common;
using MSTech.Data.Common.Abstracts;
using MSTech.GestaoEscolar.Entities;

namespace MSTech.GestaoEscolar.DAL.Abstracts
{
	
    /// <summary>
    /// Classe abstrata de ESC_PredioEndereco
    /// </summary>
    public abstract class Abstract_ESC_PredioEnderecoDAO : Abstract_DAL<ESC_PredioEndereco>
    {

        protected override string ConnectionStringName
        {
            get
            {
                return "GestaoEscolar";
            }
        }

        /// <summary>
        /// Configura os parametros do metodo de carregar
        /// </ssummary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamCarregar(QuerySelectStoredProcedure qs, ESC_PredioEndereco entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@prd_id";
            Param.Size = 4;
            Param.Value = entity.prd_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ped_id";
            Param.Size = 4;
            Param.Value = entity.ped_id;
            qs.Parameters.Add(Param);


        }

        /// <summary>
        /// Configura os parametros do metodo de Inserir
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ESC_PredioEndereco entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@prd_id";
            Param.Size = 4;
            Param.Value = entity.prd_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ped_id";
            Param.Size = 4;
            Param.Value = entity.ped_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@end_id";
            Param.Size = 16;
            Param.Value = entity.end_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@uae_id";
            Param.Size = 16;
            Param.Value = entity.uae_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@ped_numero";
            Param.Size = 20;
            Param.Value = entity.ped_numero;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@ped_complemento";
            Param.Size = 100;
            if (!string.IsNullOrEmpty(entity.ped_complemento))
                Param.Value = entity.ped_complemento;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@ped_situacao";
            Param.Size = 1;
            Param.Value = entity.ped_situacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@ped_dataCriacao";
            Param.Size = 16;
            Param.Value = entity.ped_dataCriacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@ped_dataAlteracao";
            Param.Size = 16;
            Param.Value = entity.ped_dataAlteracao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@ped_enderecoPrincipal";
            Param.Size = 1;
            Param.Value = entity.ped_enderecoPrincipal;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Decimal;
            Param.ParameterName = "@ped_latitude";
            Param.Size = 20;
            Param.Value = entity.ped_latitude;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Decimal;
            Param.ParameterName = "@ped_longitude";
            Param.Size = 20;
            Param.Value = entity.ped_longitude;
            qs.Parameters.Add(Param);


        }

        /// <summary>
        /// Configura os parametros do metodo de Alterar
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamAlterar(QueryStoredProcedure qs, ESC_PredioEndereco entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@prd_id";
            Param.Size = 4;
            Param.Value = entity.prd_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ped_id";
            Param.Size = 4;
            Param.Value = entity.ped_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@end_id";
            Param.Size = 16;
            Param.Value = entity.end_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@uae_id";
            Param.Size = 16;
            Param.Value = entity.uae_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@ped_numero";
            Param.Size = 20;
            Param.Value = entity.ped_numero;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@ped_complemento";
            Param.Size = 100;
            if (!string.IsNullOrEmpty(entity.ped_complemento))
                Param.Value = entity.ped_complemento;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@ped_situacao";
            Param.Size = 1;
            Param.Value = entity.ped_situacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@ped_dataCriacao";
            Param.Size = 16;
            Param.Value = entity.ped_dataCriacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@ped_dataAlteracao";
            Param.Size = 16;
            Param.Value = entity.ped_dataAlteracao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@ped_enderecoPrincipal";
            Param.Size = 1;
            Param.Value = entity.ped_enderecoPrincipal;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Decimal;
            Param.ParameterName = "@ped_latitude";
            Param.Size = 20;
            Param.Value = entity.ped_latitude;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Decimal;
            Param.ParameterName = "@ped_longitude";
            Param.Size = 20;
            Param.Value = entity.ped_longitude;
            qs.Parameters.Add(Param);

        }

        /// <summary>
        /// Configura os parametros do metodo de Deletar
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamDeletar(QueryStoredProcedure qs, ESC_PredioEndereco entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@prd_id";
            Param.Size = 4;
            Param.Value = entity.prd_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ped_id";
            Param.Size = 4;
            Param.Value = entity.ped_id;
            qs.Parameters.Add(Param);


        }

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade 
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ESC_PredioEndereco entity)
        {
            entity.ped_id = Convert.ToInt32(qs.Return.Rows[0][0]);
            return (entity.ped_id > 0);
        }
    }
}

