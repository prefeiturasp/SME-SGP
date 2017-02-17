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
    /// Classe abstrata de ACA_Turno
    /// </summary>
    public abstract class Abstract_ACA_TurnoDAO : Abstract_DAL<ACA_Turno>
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
        protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_Turno entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@trn_id";
            Param.Size = 4;
            Param.Value = entity.trn_id;
            qs.Parameters.Add(Param);


        }

        /// <summary>
        /// Configura os parametros do metodo de Inserir
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_Turno entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@ent_id";
            Param.Size = 16;
            Param.Value = entity.ent_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ttn_id";
            Param.Size = 4;
            Param.Value = entity.ttn_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@trn_descricao";
            Param.Size = 200;
            Param.Value = entity.trn_descricao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@trn_padrao";
            Param.Size = 1;
            Param.Value = entity.trn_padrao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@trn_situacao";
            Param.Size = 1;
            Param.Value = entity.trn_situacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@trn_dataCriacao";
            Param.Size = 16;
            Param.Value = entity.trn_dataCriacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@trn_dataAlteracao";
            Param.Size = 16;
            Param.Value = entity.trn_dataAlteracao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@trn_controleTempo";
            Param.Size = 1;
            if (entity.trn_controleTempo > 0)
                Param.Value = entity.trn_controleTempo;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Object;
            Param.ParameterName = "@trn_horaInicio";
            Param.Size = 32;
            Param.Value = entity.trn_horaInicio;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Object;
            Param.ParameterName = "@trn_horaFim";
            Param.Size = 32;
            Param.Value = entity.trn_horaFim;
            qs.Parameters.Add(Param);


        }

        /// <summary>
        /// Configura os parametros do metodo de Alterar
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_Turno entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@trn_id";
            Param.Size = 4;
            Param.Value = entity.trn_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@ent_id";
            Param.Size = 16;
            Param.Value = entity.ent_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@ttn_id";
            Param.Size = 4;
            Param.Value = entity.ttn_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@trn_descricao";
            Param.Size = 200;
            Param.Value = entity.trn_descricao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@trn_padrao";
            Param.Size = 1;
            Param.Value = entity.trn_padrao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@trn_situacao";
            Param.Size = 1;
            Param.Value = entity.trn_situacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@trn_dataCriacao";
            Param.Size = 16;
            Param.Value = entity.trn_dataCriacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@trn_dataAlteracao";
            Param.Size = 16;
            Param.Value = entity.trn_dataAlteracao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@trn_controleTempo";
            Param.Size = 1;
            if (entity.trn_controleTempo > 0)
                Param.Value = entity.trn_controleTempo;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Object;
            Param.ParameterName = "@trn_horaInicio";
            Param.Size = 32;
            Param.Value = entity.trn_horaInicio;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Object;
            Param.ParameterName = "@trn_horaFim";
            Param.Size = 32;
            Param.Value = entity.trn_horaFim;
            qs.Parameters.Add(Param);


        }

        /// <summary>
        /// Configura os parametros do metodo de Deletar
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamDeletar(QueryStoredProcedure qs, ACA_Turno entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@trn_id";
            Param.Size = 4;
            Param.Value = entity.trn_id;
            qs.Parameters.Add(Param);


        }

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade 
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_Turno entity)
        {
            entity.trn_id = Convert.ToInt32(qs.Return.Rows[0][0]);
            return (entity.trn_id > 0);
        }
    }
}

