using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.Data.Common;
using MSTech.Data.Common.Abstracts;
using MSTech.GestaoEscolar.Entities;
using System.Data;

namespace MSTech.GestaoEscolar.DAL.Abstracts
{
    /// <summary>
    /// Classe abstrata de DCL_AgendaHorarioRequisicao
    /// </summary>
    public abstract class Abstract_DCL_AgendaHorarioRequisicaoDAO : Abstract_DAL<DCL_AgendaHorarioRequisicao>
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
        protected override void ParamCarregar(QuerySelectStoredProcedure qs, DCL_AgendaHorarioRequisicao entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@ent_id";
            Param.Size = 16;
            Param.Value = entity.ent_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@req_id";
            Param.Size = 4;
            Param.Value = entity.req_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@agh_seq";
            Param.Size = 4;
            Param.Value = entity.agh_seq;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Configura os parametros do metodo de Inserir
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, DCL_AgendaHorarioRequisicao entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@ent_id";
            Param.Size = 16;
            Param.Value = entity.ent_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@req_id";
            Param.Size = 4;
            Param.Value = entity.req_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@agh_seq";
            Param.Size = 4;
            Param.Value = entity.agh_seq;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            //Param.DbType = DbType.DateTime;
            Param.ParameterName = "@agh_horarioInicio";
            Param.Size = 16;
            Param.Value = entity.agh_horarioInicio;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            //Param.DbType = DbType.DateTime;
            Param.ParameterName = "@agh_horarioFim";
            Param.Size = 16;
            Param.Value = entity.agh_horarioFim;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@agh_intervalo";
            Param.Size = 4;
            Param.Value = entity.agh_intervalo;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@agh_situacao";
            Param.Size = 1;
            Param.Value = entity.agh_situacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@agh_dataCriacao";
            Param.Size = 16;
            Param.Value = entity.agh_dataCriacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@agh_dataAlteracao";
            Param.Size = 16;
            Param.Value = entity.agh_dataAlteracao;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Configura os parametros do metodo de Alterar
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamAlterar(QueryStoredProcedure qs, DCL_AgendaHorarioRequisicao entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@ent_id";
            Param.Size = 16;
            Param.Value = entity.ent_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@req_id";
            Param.Size = 4;
            Param.Value = entity.req_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@agh_seq";
            Param.Size = 4;
            Param.Value = entity.agh_seq;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            //Param.DbType = DbType.DateTime;
            Param.ParameterName = "@agh_horarioInicio";
            Param.Size = 16;
            Param.Value = entity.agh_horarioInicio;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            //Param.DbType = DbType.DateTime;
            Param.ParameterName = "@agh_horarioFim";
            Param.Size = 16;
            Param.Value = entity.agh_horarioFim;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@agh_intervalo";
            Param.Size = 4;
            Param.Value = entity.agh_intervalo;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@agh_situacao";
            Param.Size = 1;
            Param.Value = entity.agh_situacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@agh_dataCriacao";
            Param.Size = 16;
            Param.Value = entity.agh_dataCriacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@agh_dataAlteracao";
            Param.Size = 16;
            Param.Value = entity.agh_dataAlteracao;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Configura os parametros do metodo de Deletar
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamDeletar(QueryStoredProcedure qs, DCL_AgendaHorarioRequisicao entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@ent_id";
            Param.Size = 16;
            Param.Value = entity.ent_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@req_id";
            Param.Size = 4;
            Param.Value = entity.req_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@agh_seq";
            Param.Size = 4;
            Param.Value = entity.agh_seq;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade 
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, DCL_AgendaHorarioRequisicao entity)
        {
            entity.agh_seq = Int32.Parse(qs.Return.Rows[0][0].ToString());
            return (entity.agh_seq > 0);
        }
    }
}
