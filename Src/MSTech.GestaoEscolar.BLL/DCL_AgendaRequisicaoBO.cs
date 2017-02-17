using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Business.Common;
using MSTech.Data.Common;
using System.ComponentModel;
using System.Data;

namespace MSTech.GestaoEscolar.BLL
{
    /// <summary>
    /// DCL_AgendaRequisicao Business Object 
    /// </summary>
    public class DCL_AgendaRequisicaoBO : BusinessBase<DCL_AgendaRequisicaoDAO, DCL_AgendaRequisicao>
    {
        #region DML

        public new static bool Save(DCL_AgendaRequisicao entity)
        {
            try
            {
                if (entity.IsNew)
                {
                    entity.age_dataCriacao = DateTime.Now;
                }

                entity.age_dataAlteracao = DateTime.Now;

                DCL_AgendaRequisicaoDAO dao = new DCL_AgendaRequisicaoDAO();
                return dao.Salvar(entity);
            }
            catch
            {
                throw;
            }
        }

        public new static bool Save(DCL_AgendaRequisicao entity, TalkDBTransaction banco)
        {
            try
            {
                if (entity.IsNew)
                {
                    entity.age_dataCriacao = DateTime.Now;
                }

                entity.age_dataAlteracao = DateTime.Now;

                DCL_AgendaRequisicaoDAO dao = new DCL_AgendaRequisicaoDAO() { _Banco = banco };
                return dao.Salvar(entity);
            }
            catch
            {
                throw;
            }
        }

        #endregion

        /// <summary>
        /// Método que excluí agendaRequisição e AgendaHorarioRequisicao
        /// </summary>
        /// <param name="req_id">Id da requisição</param>
        /// <param name="ent_id">Id da entidade</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static void ApagarAgendaHorarios(int req_id, Guid ent_id)
        {
            DCL_AgendaRequisicaoDAO dao = new DCL_AgendaRequisicaoDAO();
            TalkDBTransaction banco = dao._Banco;

            try
            {

                DCL_AgendaRequisicao agendaRequisicao = new DCL_AgendaRequisicao()
                {
                    req_id = req_id
                    ,
                    ent_id = ent_id
                };

                DCL_AgendaRequisicaoBO.GetEntity(agendaRequisicao);

                agendaRequisicao.age_situacao = 3;
                agendaRequisicao.age_dataAlteracao = DateTime.Now;

                DCL_AgendaRequisicaoBO.Save(agendaRequisicao, banco);


                DataTable dtAgenda = DCL_AgendaHorarioRequisicaoBO.BuscarBy_ent_id_req_id(ent_id, req_id);

                foreach (DataRow row in dtAgenda.Rows)
                {
                    DCL_AgendaHorarioRequisicao agendaHorarioRequisicao = new DCL_AgendaHorarioRequisicao()
                    {
                        ent_id = ent_id
                        ,
                        req_id = req_id
                        ,
                        agh_seq =
                            int.Parse(
                                row["agh_seq"].ToString())
                    };

                    DCL_AgendaHorarioRequisicaoBO.GetEntity(agendaHorarioRequisicao);

                    agendaHorarioRequisicao.agh_situacao = 3;
                    agendaHorarioRequisicao.agh_dataAlteracao = DateTime.Now;

                    DCL_AgendaHorarioRequisicaoBO.Save(agendaHorarioRequisicao, banco);
                }

                banco.Close();
            }
            catch (Exception err)
            {
                banco.Close(err);
                throw err;
            }
        }
    }
}
