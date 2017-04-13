using System;
using System.Linq;
using System.ComponentModel;
using System.Data;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using System.Collections.Generic;
using System.IO;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;

namespace MSTech.GestaoEscolar.BLL
{
    public class ACA_CalendarioEventoBO : BusinessBase<ACA_CalendarioEventoDAO, ACA_CalendarioEvento>
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable BuscaCalendarioAnual
        (
            long evt_id
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            if (pageSize == 0)
                pageSize = 1;

            totalRecords = 0;
            ACA_CalendarioEventoDAO daoEvento = new ACA_CalendarioEventoDAO();
            return daoEvento.SelectBy_evt_id(evt_id, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

        /// <summary>
        /// Inclui um novo evento para o calendario
        /// </summary>
        /// <param name="entity">Entidade ACA_CalendarioEvento</param>
        /// <param name="banco">Conexão aberta com o banco de dados</param>
        /// <returns>True = incluído/alterado | False = não incluído/alterado</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Save
        (
            ACA_CalendarioEvento entity
            , Data.Common.TalkDBTransaction banco
        )
        {
            if (entity.Validate())
            {
                ACA_CalendarioEventoDAO dao = new ACA_CalendarioEventoDAO { _Banco = banco };

                return dao.Salvar(entity);
            }

            throw new Validation.Exceptions.ValidationException(entity.PropertiesErrorList[0].Message);
        }

        /// <summary>
        /// Retorna data table contendo os eventos referentes ao caledário
        /// passado por parâmetro
        /// </summary>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="evt_padrao">Se o evento é padrão</param>
        /// <param name="esc_id">Id da escola do evento</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable BuscaEventosCalendario(int cal_id, bool evt_padrao, int esc_id)
        {
            ACA_CalendarioEventoDAO daoEvento = new ACA_CalendarioEventoDAO();
            return daoEvento.SelectBy_cal_id(cal_id, evt_padrao, esc_id);
        }

        /// <summary>
        /// Retorna data table contendo os eventos referentes ao caledário e docente
        /// passado por parâmetro
        /// </summary>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="doc_id">ID do docente</param>
        /// <param name="evt_padrao">Se o evento é padrão</param>
        /// <param name="esc_id">Id da escola do evento</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable BuscaEventosCalendarioDocente(int cal_id, long doc_id, bool evt_padrao, int esc_id)
        {
            ACA_CalendarioEventoDAO daoEvento = new ACA_CalendarioEventoDAO();
            return daoEvento.SelectBy_CalendarioDocente(cal_id, doc_id, evt_padrao, esc_id);
        }

        /// <summary>
        /// Seleciona os Calendários associados a um evento
        /// </summary>
        /// <param name="ent_id">Entidade da visao do usuario (obrigatório)</param>
        /// <param name="evt_id">Id do evento para filtro (qdo 0 não retorna nenhum dado)</param>
        /// <returns>Datatable com cal_id e cal_descricao dos calendários selecionados</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable Select_Associados(Guid ent_id, long evt_id)
        {
            ACA_CalendarioEventoDAO daoEvento = new ACA_CalendarioEventoDAO();
            return daoEvento.Select_Associados(ent_id, evt_id);
        }

        /// <summary>
        /// Seleciona os Calendários NAO associados a um evento
        /// </summary>
        /// <param name="ent_id">Entidade da visao do usuario (obrigatório)</param>
        /// <param name="evt_id">Id do evento para filtro (qdo 0 retorna todos os eventos)</param>
        /// <param name="usu_id">ID do usuário</param>
        /// <param name="gru_id">ID do grupo do usuário</param>
        /// <param name="doc_id">ID do docente</param>
        /// <returns>Datatable com cal_id e cal_descricao dos calendários selecionados</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable Select_naoAssociados(Guid ent_id, long evt_id, int tpc_id, Guid usu_id, Guid gru_id, long doc_id)
        {
            ACA_CalendarioEventoDAO daoEvento = new ACA_CalendarioEventoDAO();
            return daoEvento.Select_naoAssociados(ent_id, evt_id, tpc_id, usu_id, gru_id, doc_id);
        }

        public static DataTable Select_Load(ACA_CalendarioEvento entity)
        {
            ACA_CalendarioEventoDAO daoEvento = new ACA_CalendarioEventoDAO();
            return daoEvento.Select_Load(entity);
        }

        /// <summary>
        /// Busca se o calendario tem algum evento do tipo determinado
        /// vigente no momento da consulta
        /// </summary>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="dataVigencia">Data da consulta</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <returns>Datatable com tpc_id encontrado</returns>
        public static DataTable GetTipoCalendario(int cal_id, DateTime dataVigencia, Guid ent_id)
        {
            totalRecords = 0;
            // Validar parâmetro necessário para a busca.
            int valor = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_RECUPERACAO, ent_id);

            ACA_CalendarioEventoDAO daoEvento = new ACA_CalendarioEventoDAO();

            if (valor <= 0)
                return new DataTable();
            return daoEvento.Select_TipoEvento(cal_id, valor, dataVigencia);
        }
    }
}