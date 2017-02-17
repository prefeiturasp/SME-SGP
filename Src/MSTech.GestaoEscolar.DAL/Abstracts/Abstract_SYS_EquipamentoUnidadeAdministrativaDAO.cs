using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.Data.Common.Abstracts;
using MSTech.GestaoEscolar.Entities;
using System.Data;
using MSTech.Data.Common;

namespace MSTech.GestaoEscolar.DAL.Abstracts
{
    /// <summary>
    /// Classe abstrata de SYS_EquipamentoUnidadeAdministrativa
    /// </summary>
    public abstract class Abstract_SYS_EquipamentoUnidadeAdministrativaDAO : Abstract_DAL<SYS_EquipamentoUnidadeAdministrativa>
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
        protected override void ParamCarregar(QuerySelectStoredProcedure qs, SYS_EquipamentoUnidadeAdministrativa entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@equ_id";
            Param.Size = 16;
            Param.Value = entity.equ_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@uad_id";
            Param.Size = 16;
            Param.Value = entity.uad_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@ent_id";
            Param.Size = 16;
            Param.Value = entity.ent_id;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Configura os parametros do metodo de Inserir
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, SYS_EquipamentoUnidadeAdministrativa entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@equ_id";
            Param.Size = 16;
            Param.Value = entity.equ_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@uad_id";
            Param.Size = 16;
            Param.Value = entity.uad_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@ent_id";
            Param.Size = 16;
            Param.Value = entity.ent_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@eua_dataCriacao";
            Param.Size = 16;
            if (entity.eua_dataCriacao != new DateTime())
                Param.Value = entity.eua_dataCriacao;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@eua_dataAlteracao";
            Param.Size = 16;
            if (entity.eua_dataAlteracao != new DateTime())
                Param.Value = entity.eua_dataAlteracao;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@eua_situacao";
            Param.Size = 1;
            Param.Value = entity.eua_situacao;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Configura os parametros do metodo de Alterar
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamAlterar(QueryStoredProcedure qs, SYS_EquipamentoUnidadeAdministrativa entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@equ_id";
            Param.Size = 16;
            Param.Value = entity.equ_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@uad_id";
            Param.Size = 16;
            Param.Value = entity.uad_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@ent_id";
            Param.Size = 16;
            Param.Value = entity.ent_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@eua_dataCriacao";
            Param.Size = 16;
            if (entity.eua_dataCriacao != new DateTime())
                Param.Value = entity.eua_dataCriacao;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@eua_dataAlteracao";
            Param.Size = 16;
            if (entity.eua_dataAlteracao != new DateTime())
                Param.Value = entity.eua_dataAlteracao;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@eua_situacao";
            Param.Size = 1;
            Param.Value = entity.eua_situacao;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Configura os parametros do metodo de Deletar
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamDeletar(QueryStoredProcedure qs, SYS_EquipamentoUnidadeAdministrativa entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@equ_id";
            Param.Size = 16;
            Param.Value = entity.equ_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@uad_id";
            Param.Size = 16;
            Param.Value = entity.uad_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@ent_id";
            Param.Size = 16;
            Param.Value = entity.ent_id;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade 
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, SYS_EquipamentoUnidadeAdministrativa entity)
        {
            return (Convert.ToInt32(qs.Return.Rows[0][0]) > 0);
        }
    }
}
