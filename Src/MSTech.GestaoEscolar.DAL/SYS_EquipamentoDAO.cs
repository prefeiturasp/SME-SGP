using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.GestaoEscolar.DAL.Abstracts;
using System.Data;
using MSTech.Data.Common;

namespace MSTech.GestaoEscolar.DAL
{
    /// <summary>
    /// 
    /// </summary>
    public class SYS_EquipamentoDAO : Abstract_SYS_EquipamentoDAO
    {
        protected override string ConnectionStringName
        {
            get
            {
                return "GestaoEscolar";
            }
        }

        #region Consulta

        /// <summary>
        /// Método que carrega equipamento utilizando identificador e entidade como parametros
        /// </summary>
        /// <param name="ent_id">Id da entidade</param>
        /// <param name="equ_identificador">Identificador do equipamento (k4)</param>
        /// <returns>DataTable contendo os dados do equipamento</returns>
        public DataTable CarregarBy_entidade_indentificador(Guid ent_id, string equ_identificador)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_SYS_Equipamento_CarregarBy_entidade_identificador", this._Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@equ_identificador";
                Param.Size = 50;
                Param.Value = equ_identificador;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();
                return (qs.Return);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Traz os dados do equipamento de acordo com a chave identificadora.
        /// </summary>
        /// <param name="equ_identificador">Chave identificadora do equipamento</param>
        /// <returns></returns>
        public DataTable CarregarPor_Identificador(string equ_identificador)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_SYS_Equipamento_CarregarPor_Identificador", this._Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@equ_identificador";
            Param.Size = 50;
            Param.Value = equ_identificador;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();
            return (qs.Return);
        }

        /// <summary>
        /// Retorna os equipamentos cadastrados para a entidade..
        /// </summary>
        /// <param name="esc_id">Id da DRE</param>
        /// <param name="uni_id">Id da escola</param>
        /// <param name="equ_descricao">Descrição do equipamento</param>
        /// <returns></returns>

        public DataTable SelectByEscolaEquipamento(int esc_id, int uni_id, string equ_descricao)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_SYS_Consulta_EquipamentosBy_DRE_Escola_DescEquipamento", this._Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@esc_id";
            Param.Size = 4;
            if (esc_id > 0)
                Param.Value = esc_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@uni_id";
            Param.Size = 4;
            if (uni_id > 0)
                Param.Value = uni_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@equ_descricao";
            Param.Size = 200;
            if (string.IsNullOrEmpty(equ_descricao))
                Param.Value = DBNull.Value;
            else
                Param.Value = equ_descricao;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }


        /// <summary>
        /// Retorna os logs de tablets.
        /// </summary>
        /// <param name="esc_id">Id da DRE</param>
        /// <param name="uni_id">Id da escola</param>
        /// <returns></returns>
        public DataTable SelectLogTabletEquipamento(int esc_id, Guid uad_id, out int totalRecords)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_SYS_EquipamentosRelatorioLogTablets", this._Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@esc_id";
            Param.Size = 4;
            if (esc_id > 0)
                Param.Value = esc_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@uad_idSuperiorGestao";
            Param.Size = 16;
            Param.Value = uad_id;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            totalRecords = qs.Return.Rows.Count;

            return qs.Return;
        }


        #endregion
    }
}
