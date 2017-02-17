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
    /// Classe abstrata de SYS_Equipamento
    /// </summary>
    public abstract class Abstract_SYS_EquipamentoDAO : Abstract_DAL<SYS_Equipamento>
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
        protected override void ParamCarregar(QuerySelectStoredProcedure qs, SYS_Equipamento entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@equ_id";
            Param.Size = 16;
            Param.Value = entity.equ_id;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Configura os parametros do metodo de Inserir
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, SYS_Equipamento entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@ent_id";
            Param.Size = 16;
            Param.Value = entity.ent_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@equ_identificador";
            Param.Size = 50;
            Param.Value = entity.equ_identificador;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@equ_descricao";
            Param.Size = 200;
            if (!string.IsNullOrEmpty(entity.equ_descricao))
                Param.Value = entity.equ_descricao;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@equ_situacao";
            Param.Size = 1;
            Param.Value = entity.equ_situacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@equ_dataCriacao";
            Param.Size = 16;
            Param.Value = entity.equ_dataCriacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@equ_dataAlteracao";
            Param.Size = 16;
            Param.Value = entity.equ_dataAlteracao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@equ_appVersion";
            Param.Size = 50;
            if (!string.IsNullOrEmpty(entity.equ_appVersion))
            {
                Param.Value = entity.equ_appVersion;
            }
            else
            {
                Param.Value = DBNull.Value;
            }
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@equ_soVersion";
            Param.Size = 50;
            if (!string.IsNullOrEmpty(entity.equ_soVersion))
            {
                Param.Value = entity.equ_soVersion;
            }
            else
            {
                Param.Value = DBNull.Value;
            }
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@sis_id";
            Param.Size = 4;
            if (entity.sis_id > 0)
            {
                Param.Value = entity.sis_id;
            }
            else
            {
                Param.Value = DBNull.Value;
            }
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Configura os parametros do metodo de Alterar
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamAlterar(QueryStoredProcedure qs, SYS_Equipamento entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@equ_id";
            Param.Size = 16;
            Param.Value = entity.equ_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@ent_id";
            Param.Size = 16;
            Param.Value = entity.ent_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@equ_identificador";
            Param.Size = 50;
            Param.Value = entity.equ_identificador;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@equ_descricao";
            Param.Size = 200;
            if (!string.IsNullOrEmpty(entity.equ_descricao))
                Param.Value = entity.equ_descricao;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@equ_situacao";
            Param.Size = 1;
            Param.Value = entity.equ_situacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@equ_dataCriacao";
            Param.Size = 16;
            Param.Value = entity.equ_dataCriacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@equ_dataAlteracao";
            Param.Size = 16;
            Param.Value = entity.equ_dataAlteracao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@equ_appVersion";
            Param.Size = 50;
            if (!string.IsNullOrEmpty(entity.equ_appVersion))
            {
                Param.Value = entity.equ_appVersion;
            }
            else
            {
                Param.Value = DBNull.Value;
            }
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@equ_soVersion";
            Param.Size = 50;
            if (!string.IsNullOrEmpty(entity.equ_soVersion))
            {
                Param.Value = entity.equ_soVersion;
            }
            else
            {
                Param.Value = DBNull.Value;
            }
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@sis_id";
            Param.Size = 4;
            if (entity.sis_id > 0)
            {
                Param.Value = entity.sis_id;
            }
            else
            {
                Param.Value = DBNull.Value;
            }
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Configura os parametros do metodo de Deletar
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override void ParamDeletar(QueryStoredProcedure qs, SYS_Equipamento entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@equ_id";
            Param.Size = 16;
            Param.Value = entity.equ_id;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade 
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, SYS_Equipamento entity)
        {
            entity.equ_id = new Guid(qs.Return.Rows[0][0].ToString());
            return (entity.equ_id != Guid.Empty);
        }
    }
}
