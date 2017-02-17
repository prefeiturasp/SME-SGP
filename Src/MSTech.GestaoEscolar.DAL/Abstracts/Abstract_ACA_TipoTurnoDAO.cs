/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MSTech.Data.Common;
using MSTech.Data.Common.Abstracts;
using MSTech.GestaoEscolar.Entities;

namespace MSTech.GestaoEscolar.DAL.Abstracts
{
    /// <summary>
    /// Classe abstrata de ACA_TipoTurno
    /// </summary>
    public abstract class Abstract_ACA_TipoTurnoDAO : Abstract_DAL<ACA_TipoTurno>
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
        protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_TipoTurno entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ttn_id";
            Param.Size = 4;
            Param.Value = entity.ttn_id;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Configura os parametros do metodo de Inserir
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_TipoTurno entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@ttn_nome";
            Param.Size = 100;
            Param.Value = entity.ttn_nome;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@ttn_situacao";
            Param.Size = 1;
            Param.Value = entity.ttn_situacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@ttn_dataCriacao";
            Param.Size = 16;
            Param.Value = entity.ttn_dataCriacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@ttn_dataAlteracao";
            Param.Size = 16;
            Param.Value = entity.ttn_dataAlteracao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ttn_tipo";
            Param.Size = 1;
            if (entity.ttn_tipo > 0)
                Param.Value = entity.ttn_tipo;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Configura os parametros do metodo de Alterar
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_TipoTurno entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ttn_id";
            Param.Size = 4;
            Param.Value = entity.ttn_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@ttn_nome";
            Param.Size = 100;
            Param.Value = entity.ttn_nome;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@ttn_situacao";
            Param.Size = 1;
            Param.Value = entity.ttn_situacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@ttn_dataCriacao";
            Param.Size = 16;
            Param.Value = entity.ttn_dataCriacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@ttn_dataAlteracao";
            Param.Size = 16;
            Param.Value = entity.ttn_dataAlteracao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ttn_tipo";
            Param.Size = 1;
            if (entity.ttn_tipo > 0)
                Param.Value = entity.ttn_tipo;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Configura os parametros do metodo de Deletar
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamDeletar(QueryStoredProcedure qs, ACA_TipoTurno entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ttn_id";
            Param.Size = 4;
            Param.Value = entity.ttn_id;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_TipoTurno entity)
        {
            entity.ttn_id = Convert.ToInt32(qs.Return.Rows[0][0]);
            return (entity.ttn_id > 0);
        }
    }
}