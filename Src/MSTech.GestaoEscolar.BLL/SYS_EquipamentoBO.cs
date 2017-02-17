using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Data.Common;
using System.ComponentModel;
using System.Data;

namespace MSTech.GestaoEscolar.BLL
{
    /// <summary>
    /// SYS_Equipamento Business Object 
    /// </summary>
    public class SYS_EquipamentoBO : BusinessBase<SYS_EquipamentoDAO, SYS_Equipamento>
    {
        #region DML

        public new static bool Save(SYS_Equipamento entity)
        {
            try
            {
                if (entity.IsNew)
                {
                    entity.equ_dataCriacao = DateTime.Now;
                }

                entity.equ_dataAlteracao = DateTime.Now;

                SYS_EquipamentoDAO dao = new SYS_EquipamentoDAO();
                return dao.Salvar(entity);
            }
            catch
            {
                throw;
            }
        }

        public new static bool Save(SYS_Equipamento entity, TalkDBTransaction banco)
        {
            try
            {
                if (entity.IsNew)
                {
                    entity.equ_dataCriacao = DateTime.Now;
                }

                entity.equ_dataAlteracao = DateTime.Now;

                SYS_EquipamentoDAO dao = new SYS_EquipamentoDAO() { _Banco = banco };
                return dao.Salvar(entity);
            }
            catch
            {
                throw;
            }
        }

        #endregion

        /// <summary>
        /// Método que carrega equipamento utilizando identificador e entidade como parametros
        /// </summary>
        /// <param name="ent_id">Id da entidade</param>
        /// <param name="equ_identificador">Identificador do equipamento (k4)</param>
        /// <returns>DataTable contendo os dados do equipamento</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable CarregarBy_entidade_indentificador(Guid ent_id, string equ_identificador)
        {
            try
            {
                SYS_EquipamentoDAO dao = new SYS_EquipamentoDAO();
                return dao.CarregarBy_entidade_indentificador(ent_id, equ_identificador);
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        #region Consulta 
        /// <summary>
        /// Traz os dados do equipamento de acordo com a chave identificadora.
        /// </summary>
        /// <param name="equ_identificador">Chave identificadora do equipamento</param>
        /// <returns></returns>
        public static SYS_Equipamento CarregarPor_Identificador(string equ_identificador)
        {
            SYS_EquipamentoDAO dao = new SYS_EquipamentoDAO();
            DataTable dt = dao.CarregarPor_Identificador(equ_identificador);
            SYS_Equipamento entity = new SYS_Equipamento();
            if (dt.Rows.Count > 0)
            {
                entity = dao.DataRowToEntity(dt.Rows[0], entity);
            }

            return entity;
        }

        /// <summary>
        /// Retorna os equipamentos cadastrados para a entidade..
        /// </summary>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="uni_id">Id da DRE</param>
        /// <param name="equ_descricao">Descrição do equipamento</param>
        /// <returns></returns>
        public static DataTable SelectByEscolaEquipamento(int esc_id, int uni_id, string equ_descricao)
        {
            SYS_EquipamentoDAO dao = new SYS_EquipamentoDAO();
            DataTable dt = dao.SelectByEscolaEquipamento(esc_id, uni_id, equ_descricao);

            totalRecords = dt.Rows.Count;

            return dt;
        }

        /// <summary>
        /// Retorna os logs dos tablets.
        /// </summary>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="uni_id">Id da DRE</param>
        /// <returns></returns>
        public static DataTable SelectLogTabletEquipamento(int esc_id, Guid uad_id)
        {
            SYS_EquipamentoDAO dao = new SYS_EquipamentoDAO();
            DataTable dt = dao.SelectLogTabletEquipamento(esc_id, uad_id, out totalRecords);

            return dt;
        }

        #endregion

    }
}
