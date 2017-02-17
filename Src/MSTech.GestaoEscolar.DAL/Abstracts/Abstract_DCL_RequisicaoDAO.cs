using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.Data.Common.Abstracts;
using MSTech.GestaoEscolar.Entities;
using MSTech.Data.Common;
using System.Data;

namespace MSTech.GestaoEscolar.DAL.Abstracts
{
    /// <summary>
    /// Classe abstrata de DCL_Requisicao
    /// </summary>
    public abstract class Abstract_DCL_RequisicaoDAO : Abstract_DAL<DCL_Requisicao>
    {
        protected override string ConnectionStringName
        {
            get
            {
                return "MSTech.DiarioClasse";
            }
        }

        /// <summary>
        /// Configura os parametros do metodo de carregar
        /// </ssummary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamCarregar(QuerySelectStoredProcedure qs, DCL_Requisicao entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@req_id";
            Param.Size = 4;
            Param.Value = entity.req_id;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Configura os parametros do metodo de Inserir
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, DCL_Requisicao entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@req_id";
            Param.Size = 4;
            Param.Value = entity.req_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@req_nome";
            Param.Size = 50;
            Param.Value = entity.req_nome;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@req_permiteAgenda";
            Param.Size = 1;
            Param.Value = entity.req_permiteAgenda;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Configura os parametros do metodo de Alterar
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamAlterar(QueryStoredProcedure qs, DCL_Requisicao entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@req_id";
            Param.Size = 4;
            Param.Value = entity.req_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@req_nome";
            Param.Size = 50;
            Param.Value = entity.req_nome;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@req_permiteAgenda";
            Param.Size = 1;
            Param.Value = entity.req_permiteAgenda;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Configura os parametros do metodo de Deletar
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamDeletar(QueryStoredProcedure qs, DCL_Requisicao entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@req_id";
            Param.Size = 4;
            Param.Value = entity.req_id;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade 
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, DCL_Requisicao entity)
        {
            return false;
        }
    }
}
