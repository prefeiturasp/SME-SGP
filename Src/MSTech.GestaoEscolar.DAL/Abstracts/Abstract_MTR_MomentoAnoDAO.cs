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
    /// Classe abstrata de MTR_MomentoAno
    /// </summary>
    public abstract class Abstract_MTR_MomentoAnoDAO : Abstract_DAL<MTR_MomentoAno>
    {
        protected override string ConnectionStringName
        {
            get
            {
                return "MSTech.GestaoEscolar";
            }
        }

        /// <summary>
        /// Configura os parametros do metodo de carregar
        /// </ssummary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamCarregar(QuerySelectStoredProcedure qs, MTR_MomentoAno entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@mom_ano";
            Param.Size = 4;
            Param.Value = entity.mom_ano;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@mom_id";
            Param.Size = 4;
            Param.Value = entity.mom_id;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Configura os parametros do metodo de Inserir
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, MTR_MomentoAno entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@mom_ano";
            Param.Size = 4;
            Param.Value = entity.mom_ano;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@mom_id";
            Param.Size = 4;
            Param.Value = entity.mom_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@ent_id";
            Param.Size = 16;
            Param.Value = entity.ent_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Object;
            Param.ParameterName = "@mom_dataCongelamento";
            Param.Size = 20;
            Param.Value = entity.mom_dataCongelamento;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@mom_congelamentoEscola";
            Param.Size = 1;
            Param.Value = entity.mom_congelamentoEscola;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Object;
            Param.ParameterName = "@mom_dataCongelamentoCenso";
            Param.Size = 20;
            if (entity.mom_dataCongelamentoCenso != new DateTime())
                Param.Value = entity.mom_dataCongelamentoCenso;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@mom_prazoMovimentacao";
            Param.Size = 4;
            Param.Value = entity.mom_prazoMovimentacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@mom_prazoAprovacaoRetroativa";
            Param.Size = 4;
            Param.Value = entity.mom_prazoAprovacaoRetroativa;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@mom_situacao";
            Param.Size = 1;
            Param.Value = entity.mom_situacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@mom_dataCriacao";
            Param.Size = 16;
            Param.Value = entity.mom_dataCriacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@mom_dataAlteracao";
            Param.Size = 16;
            Param.Value = entity.mom_dataAlteracao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime2;
            Param.ParameterName = "@mom_dataCalculoIdade";
            Param.Size = 20;
            Param.Value = entity.mom_dataCalculoIdade;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@mom_controleSemestral";
            Param.Size = 1;
            Param.Value = entity.mom_controleSemestral;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Configura os parametros do metodo de Alterar
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamAlterar(QueryStoredProcedure qs, MTR_MomentoAno entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@mom_ano";
            Param.Size = 4;
            Param.Value = entity.mom_ano;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@mom_id";
            Param.Size = 4;
            Param.Value = entity.mom_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@ent_id";
            Param.Size = 16;
            Param.Value = entity.ent_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Object;
            Param.ParameterName = "@mom_dataCongelamento";
            Param.Size = 20;
            Param.Value = entity.mom_dataCongelamento;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@mom_congelamentoEscola";
            Param.Size = 1;
            Param.Value = entity.mom_congelamentoEscola;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Object;
            Param.ParameterName = "@mom_dataCongelamentoCenso";
            Param.Size = 20;
            if (entity.mom_dataCongelamentoCenso != new DateTime())
                Param.Value = entity.mom_dataCongelamentoCenso;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@mom_prazoMovimentacao";
            Param.Size = 4;
            Param.Value = entity.mom_prazoMovimentacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@mom_prazoAprovacaoRetroativa";
            Param.Size = 4;
            Param.Value = entity.mom_prazoAprovacaoRetroativa;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@mom_situacao";
            Param.Size = 1;
            Param.Value = entity.mom_situacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@mom_dataCriacao";
            Param.Size = 16;
            Param.Value = entity.mom_dataCriacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@mom_dataAlteracao";
            Param.Size = 16;
            Param.Value = entity.mom_dataAlteracao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@mom_dataCalculoIdade";
            Param.Size = 16;
            Param.Value = entity.mom_dataCalculoIdade;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@mom_controleSemestral";
            Param.Size = 1;
            Param.Value = entity.mom_controleSemestral;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Configura os parametros do metodo de Deletar
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamDeletar(QueryStoredProcedure qs, MTR_MomentoAno entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@mom_ano";
            Param.Size = 4;
            Param.Value = entity.mom_ano;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@mom_id";
            Param.Size = 4;
            Param.Value = entity.mom_id;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, MTR_MomentoAno entity)
        {
            return true;
        }
    }
}