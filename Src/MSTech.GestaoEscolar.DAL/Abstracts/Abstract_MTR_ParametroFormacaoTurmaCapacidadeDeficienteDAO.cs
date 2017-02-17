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
    /// Classe abstrata de MTR_ParametroFormacaoTurmaCapacidadeDeficiente
    /// </summary>
    public abstract class Abstract_MTR_ParametroFormacaoTurmaCapacidadeDeficienteDAO : Abstract_DAL<MTR_ParametroFormacaoTurmaCapacidadeDeficiente>
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
        protected override void ParamCarregar(QuerySelectStoredProcedure qs, MTR_ParametroFormacaoTurmaCapacidadeDeficiente entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@pfi_id";
            Param.Size = 4;
            Param.Value = entity.pfi_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@pft_id";
            Param.Size = 4;
            Param.Value = entity.pft_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@pfc_id";
            Param.Size = 4;
            Param.Value = entity.pfc_id;
            qs.Parameters.Add(Param);


        }

        /// <summary>
        /// Configura os parametros do metodo de Inserir
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, MTR_ParametroFormacaoTurmaCapacidadeDeficiente entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@pfi_id";
            Param.Size = 4;
            Param.Value = entity.pfi_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@pft_id";
            Param.Size = 4;
            Param.Value = entity.pft_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@pfc_id";
            Param.Size = 4;
            Param.Value = entity.pfc_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@pfc_qtdDeficiente";
            Param.Size = 4;
            Param.Value = entity.pfc_qtdDeficiente;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@pfc_capacidadeComDeficiente";
            Param.Size = 4;
            Param.Value = entity.pfc_capacidadeComDeficiente;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@pfc_situacao";
            Param.Size = 1;
            Param.Value = entity.pfc_situacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@pfc_dataCriacao";
            Param.Size = 16;
            Param.Value = entity.pfc_dataCriacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@pfc_dataAlteracao";
            Param.Size = 16;
            Param.Value = entity.pfc_dataAlteracao;
            qs.Parameters.Add(Param);


        }

        /// <summary>
        /// Configura os parametros do metodo de Alterar
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamAlterar(QueryStoredProcedure qs, MTR_ParametroFormacaoTurmaCapacidadeDeficiente entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@pfi_id";
            Param.Size = 4;
            Param.Value = entity.pfi_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@pft_id";
            Param.Size = 4;
            Param.Value = entity.pft_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@pfc_id";
            Param.Size = 4;
            Param.Value = entity.pfc_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@pfc_qtdDeficiente";
            Param.Size = 4;
            Param.Value = entity.pfc_qtdDeficiente;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@pfc_capacidadeComDeficiente";
            Param.Size = 4;
            Param.Value = entity.pfc_capacidadeComDeficiente;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@pfc_situacao";
            Param.Size = 1;
            Param.Value = entity.pfc_situacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@pfc_dataCriacao";
            Param.Size = 16;
            Param.Value = entity.pfc_dataCriacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@pfc_dataAlteracao";
            Param.Size = 16;
            Param.Value = entity.pfc_dataAlteracao;
            qs.Parameters.Add(Param);


        }

        /// <summary>
        /// Configura os parametros do metodo de Deletar
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamDeletar(QueryStoredProcedure qs, MTR_ParametroFormacaoTurmaCapacidadeDeficiente entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@pfi_id";
            Param.Size = 4;
            Param.Value = entity.pfi_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@pft_id";
            Param.Size = 4;
            Param.Value = entity.pft_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@pfc_id";
            Param.Size = 4;
            Param.Value = entity.pfc_id;
            qs.Parameters.Add(Param);


        }

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade 
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, MTR_ParametroFormacaoTurmaCapacidadeDeficiente entity)
        {
            entity.pfc_id = Convert.ToInt32(qs.Return.Rows[0][0]);
            return (entity.pfc_id > 0);
        }	
    }
}

