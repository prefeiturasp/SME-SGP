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
    /// DCL_AgendaHorarioRequisicao Business Object 
    /// </summary>
    public class DCL_AgendaHorarioRequisicaoBO : BusinessBase<DCL_AgendaHorarioRequisicaoDAO, DCL_AgendaHorarioRequisicao>
    {
        #region DML

        public new static bool Save(DCL_AgendaHorarioRequisicao entity)
        {
            try
            {
                if (entity.IsNew)
                {
                    entity.agh_dataCriacao = DateTime.Now;
                }

                entity.agh_dataAlteracao = DateTime.Now;

                DCL_AgendaHorarioRequisicaoDAO dao = new DCL_AgendaHorarioRequisicaoDAO();
                return dao.Salvar(entity);
            }
            catch
            {
                throw;
            }
        }

        public new static bool Save(DCL_AgendaHorarioRequisicao entity, TalkDBTransaction banco)
        {
            try
            {
                if (entity.IsNew)
                {
                    entity.agh_dataCriacao = DateTime.Now;
                }

                entity.agh_dataAlteracao = DateTime.Now;

                DCL_AgendaHorarioRequisicaoDAO dao = new DCL_AgendaHorarioRequisicaoDAO() { _Banco = banco };
                return dao.Salvar(entity);
            }
            catch
            {
                throw;
            }
        }

        #endregion

        /// <summary>
        /// Buscar horários da agenda
        /// </summary>
        /// <param name="ent_id">Id da entidade</param>
        /// <param name="req_id">Id da requisição</param>
        /// <returns>Horários da agenda</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable BuscarBy_ent_id_req_id(Guid ent_id, int req_id)
        {
            DCL_AgendaHorarioRequisicaoDAO dao = new DCL_AgendaHorarioRequisicaoDAO();
            return dao.SelectBy_ent_id_req_id(ent_id, req_id);
        }

        /// <summary>
        /// Método que salva agenda e os horários
        /// </summary>
        /// <param name="_VS_Agenda">Data table com os dados dos horários</param>
        /// <param name="ent_id">Id da entidade</param>
        /// <param name="req_id">Id da requisição</param>
        /// <param name="agendaRequisicao">Objeto agenda requisição</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static void SalvarAgendaHorarios(DataTable _VS_Agenda, Guid ent_id, int req_id, DCL_AgendaRequisicao agendaRequisicao)
        {
            DCL_AgendaHorarioRequisicaoDAO dao = new DCL_AgendaHorarioRequisicaoDAO();
            TalkDBTransaction banco = dao._Banco;

            try
            {
                DCL_AgendaRequisicaoBO.Save(agendaRequisicao, banco);

                for (int i = 0; i < _VS_Agenda.Rows.Count; i++)
                {
                    DCL_AgendaHorarioRequisicao agendaHorarioRequisicao = new DCL_AgendaHorarioRequisicao();

                    if (_VS_Agenda.Rows[i].RowState != DataRowState.Deleted)
                    {
                        if (int.Parse(_VS_Agenda.Rows[i]["agh_seq"].ToString()) < 1)
                        {
                            agendaHorarioRequisicao.IsNew = true;
                            agendaHorarioRequisicao.agh_dataAlteracao = DateTime.Now;
                            agendaHorarioRequisicao.agh_dataCriacao = DateTime.Now;
                            agendaHorarioRequisicao.agh_horarioFim =
                                TimeSpan.Parse(_VS_Agenda.Rows[i]["agh_horarioFim"].ToString());
                            agendaHorarioRequisicao.agh_horarioInicio =
                                TimeSpan.Parse(_VS_Agenda.Rows[i]["agh_horarioInicio"].ToString());
                            agendaHorarioRequisicao.agh_intervalo =
                                Int32.Parse(_VS_Agenda.Rows[i]["agh_intervalo"].ToString());
                            agendaHorarioRequisicao.agh_seq = 1;
                            agendaHorarioRequisicao.agh_situacao = 1;
                            agendaHorarioRequisicao.ent_id = ent_id;
                            agendaHorarioRequisicao.req_id = req_id;
                        }
                        else
                        {
                            agendaHorarioRequisicao.ent_id = ent_id;
                            agendaHorarioRequisicao.agh_seq = int.Parse(_VS_Agenda.Rows[i]["agh_seq"].ToString());
                            agendaHorarioRequisicao.req_id = req_id;

                            DCL_AgendaHorarioRequisicaoBO.GetEntity(agendaHorarioRequisicao);

                            agendaHorarioRequisicao.agh_horarioFim =
                                TimeSpan.Parse(_VS_Agenda.Rows[i]["agh_horarioFim"].ToString());
                            agendaHorarioRequisicao.agh_horarioInicio =
                                TimeSpan.Parse(_VS_Agenda.Rows[i]["agh_horarioInicio"].ToString());
                            agendaHorarioRequisicao.agh_intervalo =
                                Int32.Parse(_VS_Agenda.Rows[i]["agh_intervalo"].ToString());
                            agendaHorarioRequisicao.agh_dataAlteracao = DateTime.Now;
                            agendaHorarioRequisicao.IsNew = false;
                        }
                    }
                    else
                    {
                        agendaHorarioRequisicao.ent_id = ent_id;
                        agendaHorarioRequisicao.agh_seq =
                            int.Parse(_VS_Agenda.Rows[i]["agh_seq", DataRowVersion.Original].ToString());
                        agendaHorarioRequisicao.req_id = req_id;

                        DCL_AgendaHorarioRequisicaoBO.GetEntity(agendaHorarioRequisicao);

                        agendaHorarioRequisicao.agh_dataAlteracao = DateTime.Now;
                        agendaHorarioRequisicao.agh_situacao = 3;
                        agendaHorarioRequisicao.IsNew = false;
                    }

                    DCL_AgendaHorarioRequisicaoBO.Save(agendaHorarioRequisicao, banco);

                    banco.Close();
                }
            }
            catch (Exception err)
            {
                banco.Close(err);
                throw err;
            }
        }
    }
}
