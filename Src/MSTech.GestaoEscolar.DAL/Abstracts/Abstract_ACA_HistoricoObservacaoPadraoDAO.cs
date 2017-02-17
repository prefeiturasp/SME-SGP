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
    /// Classe abstrata de ACA_HistoricoObservacaoPadrao
    /// </summary>
    public abstract class Abstract_ACA_HistoricoObservacaoPadraoDAO : Abstract_DAL<ACA_HistoricoObservacaoPadrao>
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
        protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_HistoricoObservacaoPadrao entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@hop_id";
            Param.Size = 4;
            Param.Value = entity.hop_id;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Configura os parametros do metodo de Inserir
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_HistoricoObservacaoPadrao entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@hop_tipo";
            Param.Size = 1;
            Param.Value = entity.hop_tipo;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@hop_nome";
            Param.Size = 100;
            Param.Value = entity.hop_nome;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@hop_descricao";
            Param.Size = 2147483647;
            Param.Value = entity.hop_descricao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@hop_situacao";
            Param.Size = 1;
            Param.Value = entity.hop_situacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@hop_dataCriacao";
            Param.Size = 16;
            Param.Value = entity.hop_dataCriacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@hop_dataAlteracao";
            Param.Size = 16;
            Param.Value = entity.hop_dataAlteracao;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Configura os parametros do metodo de Alterar
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_HistoricoObservacaoPadrao entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@hop_id";
            Param.Size = 4;
            Param.Value = entity.hop_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@hop_tipo";
            Param.Size = 1;
            Param.Value = entity.hop_tipo;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@hop_nome";
            Param.Size = 100;
            Param.Value = entity.hop_nome;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@hop_descricao";
            Param.Size = 2147483647;
            Param.Value = entity.hop_descricao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@hop_situacao";
            Param.Size = 1;
            Param.Value = entity.hop_situacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@hop_dataCriacao";
            Param.Size = 16;
            Param.Value = entity.hop_dataCriacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@hop_dataAlteracao";
            Param.Size = 16;
            Param.Value = entity.hop_dataAlteracao;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Configura os parametros do metodo de Deletar
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamDeletar(QueryStoredProcedure qs, ACA_HistoricoObservacaoPadrao entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@hop_id";
            Param.Size = 4;
            Param.Value = entity.hop_id;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade 
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_HistoricoObservacaoPadrao entity)
        {
            entity.hop_id = Convert.ToInt32(qs.Return.Rows[0][0]);
            return (entity.hop_id > 0);
        }
    }
}

