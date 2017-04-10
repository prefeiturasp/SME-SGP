using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using MSTech.Business.Common;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.DAL;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using MSTech.GestaoEscolar.CustomResourceProviders;
using MSTech.GestaoEscolar.BLL.Caching;
using System.Text;

namespace MSTech.GestaoEscolar.BLL
{
    #region Excessões

    /// <summary>
    /// Classe que trata exceção de Data inicial do evento mais antiga que data atual
    /// </summary>
    public class DataAnteriorException : Exception
    {
        private string _mensagem;

        public DataAnteriorException()
        {
        }

        public DataAnteriorException(string Mensagem)
        {
            _mensagem = Mensagem;
        }

        public override string Message
        {
            get
            {
                if (string.IsNullOrEmpty(_mensagem))
                    _mensagem = base.Message;

                return _mensagem;
            }
        }
    }

    #endregion

    #region Estruturas

    /// <summary>
    /// Estrutura utilizada para armazenar as turmas por escola e calendário.
    /// </summary>
    [Serializable]
    public class Cache_EventosEfetivacaoTodos
    {
        public int cal_id { get; set; }

        public int tpc_id { get; set; }

        public Guid ent_id { get; set; }

        public int esc_id { get; set; }

        public DateTime evt_dataAlteracao { get; set; }

        public DateTime evt_dataCriacao { get; set; }

        public DateTime evt_dataFim { get; set; }

        public DateTime evt_dataInicio { get; set; }

        public string evt_descricao { get; set; }

        public long evt_id { get; set; }

        public string evt_nome { get; set; }

        public bool evt_padrao { get; set; }

        public bool evt_semAtividadeDiscente { get; set; }

        public byte evt_situacao { get; set; }

        public int tev_id { get; set; }

        public int uni_id { get; set; }

        public bool vigente { get; set; }

        public long doc_id { get; set; }

        public bool evt_limitarDocente { get; set; }
    }

    #endregion Estruturas

    public class ACA_EventoBO : BusinessBase<ACA_EventoDAO, ACA_Evento>
    {
        #region Estruturas

        public struct EventoPeriodoCalendario
        {
            public ACA_Evento entityEvento { get; set; }

            public DateTime cap_dataInicio { get; set; }

            public DateTime cap_dataFim { get; set; }

            public Int32 cal_id { get; set; }
        }

        #endregion

        #region Consultas

        /// <summary>
        /// torna os eventos cadastrados no calendário
        /// que estejam marcados como sem atividade discente.
        /// </summary>
        /// <param name="cal_id">ID do calendário</param>
        /// <returns></returns>
        public static List<ACA_Evento> Seleciona_SemAtividadeDiscente_PorCalendario(Int32 cal_id)
        {
            ACA_EventoDAO dao = new ACA_EventoDAO();
            DataTable dt = dao.Select_SemAtividadeDiscente_PorCalendario(cal_id);

            return (from DataRow dr in dt.Rows
                    select dao.DataRowToEntity(dr, new ACA_Evento())
                   ).ToList();
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelect
          (
            long evt_id
            , int tev_id
            , string esc_uni_id
            , string evt_nome
            , byte evt_situacao
            , Guid ent_id
            , Guid uad_idSuperior
            , Int16 evt_padrao
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            int esc_id = 0;
            int uni_id = 0;

            if (!string.IsNullOrEmpty(esc_uni_id))
            {
                esc_id = Convert.ToInt32(esc_uni_id.Split(';')[0]);
                uni_id = Convert.ToInt32(esc_uni_id.Split(';')[1]);
            }

            totalRecords = 0;

            if (pageSize == 0)
                pageSize = 1;

            try
            {
                ACA_EventoDAO dao = new ACA_EventoDAO();
                return dao.SelectBy_All(evt_id, tev_id, esc_id, uni_id, evt_nome, evt_situacao, ent_id, uad_idSuperior, evt_padrao, paginado, currentPage / pageSize, pageSize, out totalRecords);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Busca os eventos ligados ao calendário, que sejam do tipo definido
        /// no parâmetro como de efetivação, e que estejam vigentes. Retorna eventos
        /// padrão ou que sejam da escola da turma informada.
        /// </summary>
        /// <param name="cal_id">Id do calendário - Obrigatório</param>
        /// <param name="tur_id">Id da turma para filtrar as escolas do evento - Obrigatório</param>
        /// <param name="gru_id">Id do grupo do usuário logado</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <returns>Entidade ACA_Evento carregada</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static ACA_Evento GetEntity_Efetivacao(Int32 cal_id, Int64 tur_id, Guid gru_id, Guid ent_id)
        {
            // Validar parâmetro necessário para a busca.
            int tev_id = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, ent_id);

            if (tev_id <= 0)
            {
                throw new Exception("Para efetivar as notas, é necessário definir o parâmetro \"TIPO_EVENTO_EFETIVACAO_NOTAS\".");
            }
            // Validar se o tipo de evento do parâmetro está com a flag
            // "Relacionar com tipo de período".

            ACA_TipoEvento entTipoEvento = new ACA_TipoEvento { tev_id = tev_id };
            ACA_TipoEventoBO.GetEntity(entTipoEvento);

            if (!entTipoEvento.tev_periodoCalendario)
            {
                throw new Exception("O parâmetro \"TIPO_EVENTO_EFETIVACAO_NOTAS\" está com o campo tev_periodoCalendario = falso.");
            }

            ACA_Evento entity = new ACA_Evento();

            ACA_EventoDAO daoEvento = new ACA_EventoDAO();
            DataTable dt = daoEvento.Select_EventoEfetivacao(cal_id, tur_id);

            if (dt.Rows.Count > 0)
            {
                // Retorna os dados da entidade.
                entity = daoEvento.DataRowToEntity(dt.Rows[0], entity);

                if (entity.tpc_id <= 0)
                {
                    // Dispara uma excessão pois o evento foi cadastrado sem um tpc_id.
                    throw new Exception("O evento " + entity.evt_nome + " não está com o tipo de período do calendário definido.");
                }
            }
            else
            {
                string nomeModuloEfetivacao = GestaoEscolarUtilBO.BuscaNomeModulo("~/Classe/Efetivacao/Cadastro.aspx", "Efetivação de notas", gru_id);
                throw new ValidationException("Turma fora do período de " + nomeModuloEfetivacao + ".");
            }

            return entity;
        }

        /// <summary>
        /// Busca os eventos ligados ao calendário, que sejam do tipo definido
        /// no parâmetro como de efetivação, e que estejam vigentes. Retorna eventos
        /// padrão ou que sejam da escola da turma informada.
        /// </summary>
        /// <param name="cal_id">Id do calendário - Obrigatório</param>
        /// <param name="tur_id">Id da turma para filtrar as escolas do evento - Obrigatório</param>
        /// <param name="gru_id">Id do grupo do usuário logado</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <param name="tpc_idFiltrar">ID do período do calendário para filtrar eventos - traz eventos relacionados à esse período do calendário.</param>
        /// <param name="appMinutosCacheLongo">Minutos do cache longo</param>
        /// <returns>Lista de entidade ACA_Evento carregada</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<ACA_Evento> GetEntity_Efetivacao_List(Int32 cal_id, Int64 tur_id, Guid gru_id, Guid ent_id, int appMinutosCacheLongo, bool vigente = true, long doc_id = -1)
        {
            return GetEntity_Efetivacao_List(cal_id, tur_id, gru_id, ent_id, -1, appMinutosCacheLongo, vigente, doc_id);
        }

        /// <summary>
        /// Busca os eventos ligados ao calendário, que sejam do tipo definido
        /// no parâmetro como de efetivação, e que estejam vigentes. Retorna eventos
        /// padrão ou que sejam da escola da turma informada.
        /// </summary>
        /// <param name="cal_id">Id do calendário - Obrigatório</param>
        /// <param name="tur_id">Id da turma para filtrar as escolas do evento - Obrigatório</param>
        /// <param name="gru_id">Id do grupo do usuário logado</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <param name="tpc_idFiltrar">ID do período do calendário para filtrar eventos - traz eventos relacionados à esse período do calendário.</param>
        /// <returns>Lista de entidade ACA_Evento carregada</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<ACA_Evento> GetEntity_Efetivacao_List(Int32 cal_id, Int64 tur_id, Guid gru_id, Guid ent_id, int tpc_idFiltrar, int appMinutosCacheLongo, bool vigente = true, long doc_id = -1)
        {
            // Validar parâmetro necessário para a busca.
            string valor = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, ent_id);
            string valorRecuperacao = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_RECUPERACAO, ent_id);
            string valorFinal = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_FINAL, ent_id);
            string valorRecuperacaoFinal = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_RECUPERACAO_FINAL, ent_id);

            if (string.IsNullOrEmpty(valor))
                throw new ValidationException("Para efetivar as notas, é necessário definir o parâmetro 'Tipo de evento de efetivação de notas do período'.");

            if (string.IsNullOrEmpty(valorRecuperacao))
                throw new ValidationException("Para efetivar as notas, é necessário definir o parâmetro 'Tipo de evento de efetivação da recuperação'.");

            if (string.IsNullOrEmpty(valorFinal))
                throw new ValidationException("Para efetivar as notas, é necessário definir o parâmetro 'Tipo de evento da efetivação da nota final'.");

            if (string.IsNullOrEmpty(valorRecuperacaoFinal))
                throw new ValidationException("Para efetivar as notas, é necessário definir o parâmetro 'Tipo de evento da efetivação da nota de recuperação final'.");

            List<Cache_EventosEfetivacao> lista = ACA_TipoEventoBO.GetEntity_Eventos_Efetivacao(string.Concat(valor, ",", valorFinal, ",", valorRecuperacao, ",", valorRecuperacaoFinal));

            // Validar se o tipo de evento Efetivação notas do parâmetro está com a flag
            // "Relacionar com tipo de período".
            var x = from Cache_EventosEfetivacao item in lista
                    where item.tev_id.ToString() == valor
                    select item;
            Cache_EventosEfetivacao drTipoEvento = x.First();

            if (!drTipoEvento.tev_periodoCalendario)
                throw new ValidationException("O parâmetro 'Tipo de evento de efetivação de notas do período' não se relaciona a um tipo de período de calendário.");

            // Validar se o tipo de evento Efetivação final do parâmetro não está com a flag
            // "Relacionar com tipo de período".
            var x2 = from Cache_EventosEfetivacao dr in lista
                     where Convert.ToString(dr.tev_id) == valorRecuperacao
                     select dr;
            drTipoEvento = x2.First();

            if (!drTipoEvento.tev_periodoCalendario)
                throw new ValidationException("O parâmetro 'Tipo de evento de efetivação da recuperação' não se relaciona a um tipo de período de calendário.");

            // Validar se o tipo de evento Efetivação recuperação do parâmetro está com a flag
            // "Relacionar com tipo de período".
            var x3 = from Cache_EventosEfetivacao dr in lista
                     where Convert.ToString(dr.tev_id) == valorFinal
                     select dr;
            drTipoEvento = x3.First();

            if (drTipoEvento.tev_periodoCalendario)
                throw new ValidationException("O parâmetro 'Tipo de evento da efetivação da nota final' se relaciona a um tipo de período de calendário.");

            // Validar se o tipo de evento Recuperação final do parâmetro não está com a flag
            // "Relacionar com tipo de período".
            var x4 = from Cache_EventosEfetivacao dr in lista
                     where Convert.ToString(dr.tev_id) == valorRecuperacaoFinal
                     select dr;
            drTipoEvento = x4.First();

            if (drTipoEvento.tev_periodoCalendario)
                throw new ValidationException("O parâmetro 'Tipo de evento da efetivação da nota de recuperação final' não se relaciona a um tipo de período de calendário.");

            List<ACA_Evento> li = new List<ACA_Evento>();
            List<Cache_EventosEfetivacaoTodos> listEventosEfetivacaoTodos =
                Select_EventoEfetivacaoTodos(cal_id, tur_id, string.Concat(valor, ",", valorFinal, ",", valorRecuperacao, ",", valorRecuperacaoFinal), tpc_idFiltrar, appMinutosCacheLongo, ent_id, vigente, doc_id);

            if (listEventosEfetivacaoTodos.Any())
            {
                foreach (Cache_EventosEfetivacaoTodos eef in listEventosEfetivacaoTodos)
                {
                    ACA_Evento entity = new ACA_Evento
                    {
                        evt_id = eef.evt_id,
                        tev_id = eef.tev_id,
                        ent_id = eef.ent_id,
                        esc_id = eef.esc_id,
                        uni_id = eef.uni_id,
                        evt_padrao = eef.evt_padrao,
                        tpc_id = eef.tpc_id,
                        evt_nome = eef.evt_nome,
                        evt_descricao = eef.evt_descricao,
                        evt_dataInicio = eef.evt_dataInicio,
                        evt_dataFim = eef.evt_dataFim,
                        evt_semAtividadeDiscente = eef.evt_semAtividadeDiscente,
                        evt_situacao = eef.evt_situacao,
                        evt_dataCriacao = eef.evt_dataCriacao,
                        evt_dataAlteracao = eef.evt_dataAlteracao,
                        vigente = eef.vigente
                    };

                    // Dispara uma excessão pois o evento foi cadastrado sem um tpc_id.
                    // Caso seja Periodica, Periodica+Final ou Recuperação.
                    if (entity.tev_id.ToString() == valor || entity.tev_id.ToString() == valorRecuperacao)
                    {
                        if (entity.tpc_id <= 0)
                        {
                            throw new ValidationException("O evento " + entity.evt_nome + " não está com o tipo de período do calendário definido.");
                        }
                    }

                    li.Add(entity);
                }
            }

            return li;
        }

        /// <summary>
        /// Busca os eventos ligados ao calendário, que sejam do tipo definido
        /// no parâmetro como de efetivação, e que estejam vigentes.
        /// </summary>
        /// <param name="cal_id">Id do calendário - Obrigatório</param>
        /// <param name="cap_id">Id do período do calendário</param>
        /// <param name="gru_id">Id do grupo do usuário logado</param>
        /// <param name="esc_id"></param>
        /// <param name="uni_id"></param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <returns>Lista de entidade ACA_Evento carregada</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<ACA_Evento> GetEntity_Efetivacao_ListPorPeriodo(int cal_id, int cap_id, Guid gru_id, int esc_id, int uni_id, Guid ent_id)
        {
            // Validar parâmetro necessário para a busca.
            string valor = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, ent_id);
            string valorRecuperacao = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_RECUPERACAO, ent_id);
            string valorFinal = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_FINAL, ent_id);
            string valorRecuperacaoFinal = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_RECUPERACAO_FINAL, ent_id);

            if (string.IsNullOrEmpty(valor))
                throw new ValidationException("Para efetivar as notas, é necessário definir o parâmetro 'Tipo de evento de efetivação de notas do período'.");

            if (string.IsNullOrEmpty(valorRecuperacao))
                throw new ValidationException("Para efetivar as notas, é necessário definir o parâmetro 'Tipo de evento de efetivação da recuperação'.");

            if (string.IsNullOrEmpty(valorFinal))
                throw new ValidationException("Para efetivar as notas, é necessário definir o parâmetro 'Tipo de evento da efetivação da nota final'.");

            if (string.IsNullOrEmpty(valorRecuperacaoFinal))
                throw new ValidationException("Para efetivar as notas, é necessário definir o parâmetro 'Tipo de evento da efetivação da nota de recuperação final'.");

            List<Cache_EventosEfetivacao> lista = ACA_TipoEventoBO.GetEntity_Eventos_Efetivacao(string.Concat(valor, ",", valorFinal, ",", valorRecuperacao, ",", valorRecuperacaoFinal));

            // Validar se o tipo de evento Efetivação notas do parâmetro está com a flag
            // "Relacionar com tipo de período".
            var x = from Cache_EventosEfetivacao dr in lista
                    where Convert.ToString(dr.tev_id) == valor
                    select dr;
            Cache_EventosEfetivacao drTipoEvento = x.First();

            if (!drTipoEvento.tev_periodoCalendario)
                throw new ValidationException("O parâmetro 'Tipo de evento de efetivação de notas do período' não se relaciona a um tipo de período de calendário.");

            // Validar se o tipo de evento Efetivação final do parâmetro não está com a flag
            // "Relacionar com tipo de período".
            var x2 = from Cache_EventosEfetivacao dr in lista
                     where Convert.ToString(dr.tev_id) == valorRecuperacao
                     select dr;
            drTipoEvento = x2.First();

            if (!drTipoEvento.tev_periodoCalendario)
                throw new ValidationException("O parâmetro 'Tipo de evento de efetivação da recuperação' não se relaciona a um tipo de período de calendário.");

            // Validar se o tipo de evento Efetivação recuperação do parâmetro está com a flag
            // "Relacionar com tipo de período".
            var x3 = from Cache_EventosEfetivacao dr in lista
                     where Convert.ToString(dr.tev_id) == valorFinal
                     select dr;
            drTipoEvento = x3.First();

            if (drTipoEvento.tev_periodoCalendario)
                throw new ValidationException("O parâmetro 'Tipo de evento da efetivação da nota final' se relaciona a um tipo de período de calendário.");

            // Validar se o tipo de evento Recuperação final do parâmetro não está com a flag
            // "Relacionar com tipo de período".
            var x4 = from Cache_EventosEfetivacao dr in lista
                     where Convert.ToString(dr.tev_id) == valorRecuperacaoFinal
                     select dr;
            drTipoEvento = x4.First();

            if (drTipoEvento.tev_periodoCalendario)
                throw new ValidationException("O parâmetro 'Tipo de evento da efetivação da nota de recuperação final' não se relaciona a um tipo de período de calendário.");

            List<ACA_Evento> li = new List<ACA_Evento>();
            List<ACA_Evento> lstEventos = Select_EventoEfetivacaoTodosPorPeriodoEscola(cal_id, cap_id, esc_id, uni_id, string.Concat(valor, ",", valorFinal, ",", valorRecuperacao, ",", valorRecuperacaoFinal), GestaoEscolarUtilBO.MinutosCacheMedio);

            if (lstEventos.Count > 0)
            {
                foreach (ACA_Evento entity in lstEventos)
                {
                    // Dispara uma excessão pois o evento foi cadastrado sem um tpc_id.
                    // Caso seja Periodica, Periodica+Final ou Recuperação.
                    if (entity.tev_id.ToString() == valor || entity.tev_id.ToString() == valorRecuperacao)
                    {
                        if (entity.tpc_id <= 0)
                        {
                            throw new ValidationException("O evento " + entity.evt_nome + " não está com o tipo de período do calendário definido.");
                        }
                    }

                    li.Add(entity);
                }
            }
            else if (cap_id > 0)
            {
                string nomeModuloEfetivacao = GestaoEscolarUtilBO.BuscaNomeModulo("~/Classe/Efetivacao/Cadastro.aspx", "Efetivação de notas", gru_id);
                throw new ValidationException("Período do calendário fora do período de " + nomeModuloEfetivacao + ".");
            }

            return li;
        }

        /// <summary>
        /// Busca os eventos ligados ao calendário, que sejam do tipo definido
        /// no parâmetro como de efetivação, e que estejam vigentes.
        /// </summary>
        /// <param name="cal_id">Id do calendário - Obrigatório</param>
        /// <param name="cap_id">Id do período do calendário</param>
        /// <param name="gru_id">Id do grupo do usuário logado</param>
        /// <param name="esc_id"></param>
        /// <param name="uni_id"></param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <returns>Lista de entidade ACA_Evento carregada</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<ACA_Evento> SelecionaEventosEfetivacaoPeriodoCalendario(int cal_id, int cap_id, Guid gru_id, int esc_id, int uni_id, Guid ent_id)
        {
            // Validar parâmetro necessário para a busca.
            string valor = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, ent_id);
            string valorRecuperacao = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_RECUPERACAO, ent_id);
            string valorFinal = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_FINAL, ent_id);
            string valorRecuperacaoFinal = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_RECUPERACAO_FINAL, ent_id);

            if (string.IsNullOrEmpty(valor))
                throw new ValidationException("Para efetivar as notas, é necessário definir o parâmetro 'Tipo de evento de efetivação de notas do período'.");

            if (string.IsNullOrEmpty(valorRecuperacao))
                throw new ValidationException("Para efetivar as notas, é necessário definir o parâmetro 'Tipo de evento de efetivação da recuperação'.");

            if (string.IsNullOrEmpty(valorFinal))
                throw new ValidationException("Para efetivar as notas, é necessário definir o parâmetro 'Tipo de evento da efetivação da nota final'.");

            if (string.IsNullOrEmpty(valorRecuperacaoFinal))
                throw new ValidationException("Para efetivar as notas, é necessário definir o parâmetro 'Tipo de evento da efetivação da nota de recuperação final'.");

            List<Cache_EventosEfetivacao> lista = ACA_TipoEventoBO.GetEntity_Eventos_Efetivacao(string.Concat(valor, ",", valorFinal, ",", valorRecuperacao, ",", valorRecuperacaoFinal));

            // Validar se o tipo de evento Efetivação notas do parâmetro está com a flag
            // "Relacionar com tipo de período".
            var x = from Cache_EventosEfetivacao dr in lista
                    where Convert.ToString(dr.tev_id) == valor
                    select dr;
            Cache_EventosEfetivacao drTipoEvento = x.First();

            if (!drTipoEvento.tev_periodoCalendario)
                throw new ValidationException("O parâmetro 'Tipo de evento de efetivação de notas do período' não se relaciona a um tipo de período de calendário.");

            // Validar se o tipo de evento Efetivação final do parâmetro não está com a flag
            // "Relacionar com tipo de período".
            var x2 = from Cache_EventosEfetivacao dr in lista
                     where Convert.ToString(dr.tev_id) == valorRecuperacao
                     select dr;
            drTipoEvento = x2.First();

            if (!drTipoEvento.tev_periodoCalendario)
                throw new ValidationException("O parâmetro 'Tipo de evento de efetivação da recuperação' não se relaciona a um tipo de período de calendário.");

            // Validar se o tipo de evento Efetivação recuperação do parâmetro está com a flag
            // "Relacionar com tipo de período".
            var x3 = from Cache_EventosEfetivacao dr in lista
                     where Convert.ToString(dr.tev_id) == valorFinal
                     select dr;
            drTipoEvento = x3.First();

            if (drTipoEvento.tev_periodoCalendario)
                throw new ValidationException("O parâmetro 'Tipo de evento da efetivação da nota final' se relaciona a um tipo de período de calendário.");

            // Validar se o tipo de evento Recuperação final do parâmetro não está com a flag
            // "Relacionar com tipo de período".
            var x4 = from Cache_EventosEfetivacao dr in lista
                     where Convert.ToString(dr.tev_id) == valorRecuperacaoFinal
                     select dr;
            drTipoEvento = x4.First();

            if (drTipoEvento.tev_periodoCalendario)
                throw new ValidationException("O parâmetro 'Tipo de evento da efetivação da nota de recuperação final' não se relaciona a um tipo de período de calendário.");

            List<ACA_Evento> li = new List<ACA_Evento>();

            //ACA_EventoDAO daoEvento = new ACA_EventoDAO();
            //DataTable dt = Select_EventoEfetivacaoTodosPorPeriodoEscola(cal_id, cap_id, esc_id, uni_id, string.Concat(valor, ",", valorFinal, ",", valorRecuperacao, ",", valorRecuperacaoFinal));
            List<ACA_Evento> lstEventos = Select_EventoEfetivacaoTodosPorPeriodoEscola(cal_id, cap_id, esc_id, uni_id, string.Concat(valor, ",", valorFinal, ",", valorRecuperacao, ",", valorRecuperacaoFinal), GestaoEscolarUtilBO.MinutosCacheMedio);

            if (lstEventos.Count > 0)
            {
                foreach (ACA_Evento entity in lstEventos)
                {
                    // Dispara uma excessão pois o evento foi cadastrado sem um tpc_id.
                    // Caso seja Periodica, Periodica+Final ou Recuperação.
                    if (entity.tev_id.ToString() == valor || entity.tev_id.ToString() == valorRecuperacao)
                    {
                        if (entity.tpc_id <= 0)
                        {
                            throw new ValidationException("O evento " + entity.evt_nome + " não está com o tipo de período do calendário definido.");
                        }
                    }

                    li.Add(entity);
                }
            }

            return li;
        }

        /// <summary>
        /// 28/04/2011
        /// Metodo que pesquisa dados para o grid da busca de Eventos de calendario
        /// Qdo criado está sendo usado apenas nesse grid, e filtrado conforme campos da tela e parametros:
        /// </summary>
        /// <param name="evt_id">id do Evento (chave principal da tabela de eventos)</param>
        /// <param name="tev_id">id do Tipo de evento</param>
        /// <param name="esc_uni_id">id da Escola (qdo selecionada como parametro)</param>
        /// <param name="evt_nome">Nome do evento</param>
        /// <param name="cal_id"></param>
        /// <param name="evt_situacao">Situacao - trazendo apenas 1</param>
        /// <param name="ent_id">Entidade</param>
        /// <param name="uad_idSuperior">id da unidade superior (filtro na tela)</param>
        /// <param name="evt_padrao">Se true filtra somente o padroes</param>
        /// <param name="usu_id">id do usuario - obrigatorio</param>
        /// <param name="gru_id">ud grupo do usuario - obrigatorio</param>
        /// <param name="adm">Se o usuario tem o grupo de administrador do sistema</param>
        /// <returns>TAbela com dados dos eventos encontrados</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelect_Busca
          (
            long evt_id
            , int tev_id
            , string esc_uni_id
            , string evt_nome
            , int cal_id
            , byte evt_situacao
            , Guid ent_id
            , Guid uad_idSuperior
            , Int16 evt_padrao
            , Guid usu_id
            , Guid gru_id
            , bool adm
        )
        {
            int esc_id = 0;
            int uni_id = 0;

            if (!string.IsNullOrEmpty(esc_uni_id))
            {
                esc_id = Convert.ToInt32(esc_uni_id.Split(';')[0]);
                uni_id = Convert.ToInt32(esc_uni_id.Split(';')[1]);
            }

            totalRecords = 0;

            try
            {
                bool MostraCodigoEscola = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id);

                ACA_EventoDAO dao = new ACA_EventoDAO();
                return dao.SelectBy_Busca(
                    evt_id,
                    tev_id,
                    esc_id,
                    uni_id,
                    evt_nome,
                    cal_id,
                    evt_situacao,
                    ent_id,
                    uad_idSuperior,
                    evt_padrao,
                    usu_id,
                    gru_id,
                    adm,
                    MostraCodigoEscola,
                    out totalRecords);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Seleciona a maior data entre os eventos para a escola, período de tipo.
        /// </summary>
        /// <param name="esc_id">ID da escola.</param>
        /// <param name="uni_id">ID da unidade de escola.</param>
        /// <param name="tpc_id">ID do tipo de período do calendário.</param>
        /// <param name="tev_id">Tipos de eventos.</param>
        /// <returns></returns>
        public static DateTime SelecionaMaiorDataPorTipoPeriodoEscola(int esc_id, int uni_id, int tpc_id, string tev_id)
        {
            return new ACA_EventoDAO().SelecionaMaiorDataPorTipoPeriodoEscola(esc_id, uni_id, tpc_id, tev_id);
        }

        /// <summary>
        /// Seleciona o evento do calendário
        /// </summary>
        /// <param name="cal_id"></param>
        /// <param name="tur_id"></param>
        /// <param name="tpc_id"></param>
        /// <returns></returns>
        public static DataTable Select_EventoLiberacao(Int32 cal_id, Int64 tur_id, Int32 tpc_id)
        {
            ACA_EventoDAO dao = new ACA_EventoDAO();
            return dao.Select_EventoLiberacao(cal_id, tur_id, tpc_id);
        }
               
        /// <summary>
        /// Seleciona o evento do calendário
        /// </summary>
        /// <param name="cal_id"></param>
        /// <param name="tur_id"></param>
        /// <param name="tpc_id"></param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <returns></returns>
        public static List<Cache_EventosEfetivacaoTodos> Select_EventoEfetivacaoTodos(Int32 cal_id, Int64 tur_id, Int32 tpc_id, Guid ent_id, int appMinutosCacheLongo)
        {
            string valorFinal = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, ent_id);
            return Select_EventoEfetivacaoTodos(cal_id, tur_id, valorFinal, tpc_id, appMinutosCacheLongo, ent_id);
        }

        /// <summary>
        /// Seleciona o evento do calendário
        /// </summary>
        /// <param name="cal_id"></param>
        /// <param name="tur_id"></param>
        /// <param name="tpc_id"></param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <returns></returns>
        private static List<Cache_EventosEfetivacaoTodos> Select_EventoEfetivacaoTodos(Int32 cal_id, Int64 tur_id, string tev_id, Int32 tpc_id, int appMinutosCacheLongo, Guid ent_id, bool vigente = true, long doc_id = -1)
        {
            ACA_EventoDAO dao = new ACA_EventoDAO();
            List<Cache_EventosEfetivacaoTodos> dados = null;

            if (appMinutosCacheLongo > 0)
            {
                // Chave padrão do cache - nome do método + parâmetros.
                string chave = RetornaChaveCache_EventoEfetivacaoTodos(cal_id, tur_id, tev_id, tpc_id, vigente);

                dados = CacheManager.Factory.Get
                            (
                                chave,
                                () =>
                                {
                                    return (from DataRow dr in dao.Select_EventoEfetivacaoTodos(cal_id, tur_id, tev_id, tpc_id).Rows
                                            where !vigente || Convert.ToBoolean(dr["vigente"])
                                            select new Cache_EventosEfetivacaoTodos
                                            {
                                                cal_id = Convert.ToInt32(dr["cal_id"].ToString()),
                                                tpc_id = Convert.ToInt32(dr["tpc_id"].ToString()),
                                                ent_id = new Guid(dr["ent_id"].ToString()),
                                                esc_id = Convert.ToInt32(dr["esc_id"].ToString() == string.Empty ? "0" : dr["esc_id"].ToString()),
                                                evt_dataAlteracao = Convert.ToDateTime(dr["evt_dataAlteracao"].ToString()),
                                                evt_dataCriacao = Convert.ToDateTime(dr["evt_dataCriacao"].ToString()),
                                                evt_dataFim = Convert.ToDateTime(dr["evt_dataFim"].ToString()),
                                                evt_dataInicio = Convert.ToDateTime(dr["evt_dataInicio"].ToString()),
                                                evt_descricao = dr["evt_descricao"].ToString(),
                                                evt_id = Convert.ToInt64(dr["evt_id"].ToString()),
                                                evt_nome = dr["evt_nome"].ToString(),
                                                evt_padrao = Convert.ToBoolean(dr["evt_padrao"].ToString()),
                                                evt_semAtividadeDiscente = Convert.ToBoolean(dr["evt_semAtividadeDiscente"].ToString()),
                                                evt_situacao = Convert.ToByte(dr["evt_situacao"].ToString()),
                                                tev_id = Convert.ToInt32(dr["tev_id"].ToString()),
                                                uni_id = Convert.ToInt32(dr["uni_id"].ToString() == string.Empty ? "0" : dr["uni_id"].ToString()),
                                                vigente = Convert.ToBoolean(dr["vigente"]),
                                                doc_id = Convert.ToInt64(dr["doc_id"]),
                                                evt_limitarDocente = Convert.ToBoolean(dr["evt_limitarDocente"])
                                            }
                     ).ToList();
                                },
                                appMinutosCacheLongo
                            );
            }
            else
            {
                dados = (from DataRow dr in dao.Select_EventoEfetivacaoTodos(cal_id, tur_id, tev_id, tpc_id).Rows
                         where !vigente || Convert.ToBoolean(dr["vigente"])
                         select new Cache_EventosEfetivacaoTodos
                         {
                             cal_id = Convert.ToInt32(dr["cal_id"].ToString()),
                             tpc_id = Convert.ToInt32(dr["tpc_id"].ToString()),
                             ent_id = new Guid(dr["ent_id"].ToString()),
                             esc_id = Convert.ToInt32(dr["esc_id"].ToString() == string.Empty ? "0" : dr["esc_id"].ToString()),
                             evt_dataAlteracao = Convert.ToDateTime(dr["evt_dataAlteracao"].ToString()),
                             evt_dataCriacao = Convert.ToDateTime(dr["evt_dataCriacao"].ToString()),
                             evt_dataFim = Convert.ToDateTime(dr["evt_dataFim"].ToString()),
                             evt_dataInicio = Convert.ToDateTime(dr["evt_dataInicio"].ToString()),
                             evt_descricao = dr["evt_descricao"].ToString(),
                             evt_id = Convert.ToInt64(dr["evt_id"].ToString()),
                             evt_nome = dr["evt_nome"].ToString(),
                             evt_padrao = Convert.ToBoolean(dr["evt_padrao"].ToString()),
                             evt_semAtividadeDiscente = Convert.ToBoolean(dr["evt_semAtividadeDiscente"].ToString()),
                             evt_situacao = Convert.ToByte(dr["evt_situacao"].ToString()),
                             tev_id = Convert.ToInt32(dr["tev_id"].ToString()),
                             uni_id = Convert.ToInt32(dr["uni_id"].ToString() == string.Empty ? "0" : dr["uni_id"].ToString()),
                             vigente = Convert.ToBoolean(dr["vigente"]),
                             doc_id = Convert.ToInt64(dr["doc_id"]),
                             evt_limitarDocente = Convert.ToBoolean(dr["evt_limitarDocente"])
                         }
                     ).ToList();
            }

            bool limitarDocentesEvento = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_LIMITAR_DOCENTES_EVENTO, ent_id);
            return dados.Where(p => doc_id <= 0 || (p.doc_id == doc_id && limitarDocentesEvento) || !p.evt_limitarDocente).ToList();
        }

        /// <summary>
        /// Seleciona o evento do calendário
        /// </summary>
        /// <param name="cal_id"></param>
        /// <param name="tur_id"></param>
        /// <param name="tpc_id"></param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <returns></returns>
        public static DataTable Select_TodosEventosPorTipo_CalendarioPeriodo(Int32 cal_id, Int64 tur_id, Int32 tpc_id, Guid ent_id)
        {
            int tivo_evento = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, ent_id);

            ACA_EventoDAO dao = new ACA_EventoDAO();
            return dao.Select_TodosEventosPorTipo_CalendarioPeriodo(cal_id, tur_id, tivo_evento, tpc_id);
        }

        /// <summary>
        /// Seleciona os eventos por calendários, unidades escolares e tipo de evento.
        /// </summary>
        /// <param name="cal_ids">Ids dos calendários anuais.</param>
        /// <param name="esc_id">Id da escola.</param>
        /// <param name="uni_id">Id da unidade escolar.</param>
        /// <param name="tev_id">Id do tipo de evento.</param>
        /// <returns></returns>
        public static List<EventoPeriodoCalendario> SelecionaPorCalendarioEscolaTipoEvento(string cal_ids, int esc_id, int uni_id, int tev_id, TalkDBTransaction banco = null, bool apenasVigentes = true)
        {
            ACA_EventoDAO dao = banco == null ? new ACA_EventoDAO() : new ACA_EventoDAO { _Banco = banco };
            return (from DataRow dr in dao.SelecionaPorCalendarioEscolaTipoEvento(cal_ids, esc_id, uni_id, tev_id, apenasVigentes).Rows
                    select new EventoPeriodoCalendario
                    {
                        entityEvento = dao.DataRowToEntity(dr, new ACA_Evento())
                        ,
                        cap_dataInicio = Convert.ToDateTime(string.IsNullOrEmpty(dr["cap_dataInicio"].ToString()) ? new DateTime().ToString() : dr["cap_dataInicio"])
                        ,
                        cap_dataFim = Convert.ToDateTime(string.IsNullOrEmpty(dr["cap_dataFim"].ToString()) ? new DateTime().ToString() : dr["cap_dataFim"])
                        ,
                        cal_id = Convert.ToInt32(dr["cal_id"])
                    }).ToList();
        }
                
        /// <summary>
        /// Seleciona os eventos por calendários, unidades escolares e tipo de evento.
        /// </summary>
        /// <param name="cal_id">Id do calendário anual.</param>
        /// <param name="cap_id">Id do período do calendário</param>
        /// <param name="esc_id">Id da escola.</param>
        /// <param name="uni_id">Id da unidade escolar.</param>
        /// <param name="tev_id">Id do tipo de evento.</param>
        /// <returns></returns>
        public static List<ACA_Evento> Select_EventoEfetivacaoTodosPorPeriodoEscola(int cal_id, int cap_id, int esc_id, int uni_id, string tev_id, int appMinutosCacheCurto = 0, bool vigente = true)
        {
            List<ACA_Evento> dados;
            ACA_EventoDAO dao = new ACA_EventoDAO();

            if (appMinutosCacheCurto > 0)
            {
                // Chave padrão do cache - nome do método + parâmetros.
                string chave = RetornaChaveCache_EventoEfetivacaoTodosPorPeriodoEscola(cal_id, cap_id, esc_id, uni_id, tev_id, vigente);

                dados = CacheManager.Factory.Get(
                                chave,
                                () =>
                                {
                                    return (from DataRow eventos in dao.Select_EventoEfetivacaoTodosPorPeriodoEscola(cal_id, cap_id, esc_id, uni_id, tev_id).Rows
                                            select dao.DataRowToEntity(eventos, new ACA_Evento())).ToList();
                                },
                                    appMinutosCacheCurto
                                );
            }
            else
            {
                // busca no banco.
                dados = (from DataRow eventos in dao.Select_EventoEfetivacaoTodosPorPeriodoEscola(cal_id, cap_id, esc_id, uni_id, tev_id).Rows
                        select dao.DataRowToEntity(eventos, new ACA_Evento())).ToList();
            }
            return dados;
        }

        #endregion

        #region Validações

        /// <summary>
        /// Método que configura a validação das datas do evento e se o mesmo pode ser salvo, conforme os limites
        /// cadastrados para o tipo de evento e calendários selecionados.
        /// </summary>
        public static bool ValidarLimite
        (
            bool verificarPeriodo
            , ACA_TipoEvento entTipoEvento
            , IEnumerable<ACA_EventoLimite> Limites
            , bool evt_padrao
            , int esc_id
            , int uni_id
            , int tpc_id
            , IEnumerable<int> calendarios
            , string evt_inicio
            , string evt_fim
            , int vis_id
            , out string msgValidacao
            , List<Guid> lstUadIdPermissao
        )
        {
            msgValidacao = string.Empty;
            ESC_Escola escola = new ESC_Escola { esc_id = esc_id };

            if (entTipoEvento != null && Limites != null)
            {
                var limites = Limites.Where(l => l.tev_id == entTipoEvento.tev_id);
                var limitesUad = Limites.Where(l => l.tev_id == entTipoEvento.tev_id);

                // Filtra os limites de acordo com a seleção de alcance do evento
                if (evt_padrao)
                {
                    limites = limites.Where(evl => evl.esc_id <= 0 && evl.uad_id == Guid.Empty).ToList();
                }
                else if (esc_id > 0)
                {
                    ESC_EscolaBO.GetEntity(escola);

                    limitesUad = limitesUad.Where(evl => evl.uad_id == escola.uad_idSuperiorGestao);
                    limites = limites.Where(evl => (evl.esc_id == esc_id && evl.uni_id == uni_id) || 
                                                   (evl.esc_id <= 0 && evl.uad_id == Guid.Empty));
                }
                if (lstUadIdPermissao.Any())
                {
                    limites = Limites.Where(l => l.tev_id == entTipoEvento.tev_id)
                                     .Where(evl => lstUadIdPermissao.Any(u => evl.uad_id == u))
                                     .Union(Limites.Where(l => l.tev_id == entTipoEvento.tev_id)
                                                   .Where(evl => (evl.esc_id == esc_id && evl.uni_id == uni_id) ||
                                                                 (evl.esc_id <= 0 && evl.uad_id == Guid.Empty)));
                }
                
                // Filtra os limites de acordo com a seleção de período
                if (entTipoEvento.tev_periodoCalendario)
                {
                    if (tpc_id > 0)
                        limites = limites.Where(evl => evl.tpc_id == tpc_id);
                }
                else
                    limites = limites.Where(evl => evl.tpc_id <= 0);
                
                // Filtra os limites de acordo com a seleção de calendários
                foreach (int cal_id in calendarios)
                {
                    var limitesCalendario = limites.Where(evl => evl.cal_id == cal_id);
                    if (!limitesCalendario.Any())
                    {
                        // Não há limites cadastrados para os campos selecionados
                        // Validação falha somente quando a liberação do tipo de evento é obrigatória
                        if ((ACA_TipoEventoBO.eLiberacao)entTipoEvento.tev_liberacao ==
                            ACA_TipoEventoBO.eLiberacao.Obrigatoria)
                        {
                            msgValidacao = CustomResource.GetGlobalResourceObject("Academico"
                                , "Evento.Cadastro.ErroDataLimiteInexistente").ToString();
                            break;
                        }
                    }
                    else if (verificarPeriodo)
                    {
                        var dataInicio = Convert.ToDateTime(evt_inicio);
                        var dataFim = Convert.ToDateTime(evt_fim);

                        var temp = dataInicio;

                        /* Se houver qualquer data no intervalo informado que não esteja contemplada nos limites
                         * cadastrados, bloqueia a gravação do evento e aprensenta mensagem */
                        do
                        {
                            if (!limitesCalendario.Any(evl => temp >= evl.evl_dataInicio && temp <= evl.evl_dataFim))
                            {
                                StringBuilder sLimites = new StringBuilder();
                                limitesCalendario.OrderBy(p => p.evl_dataInicio).ToList().ForEach
                                    (evl => 
                                        sLimites.AppendFormat("- De {0} a {1}<br/>"
                                        , evl.evl_dataInicio.ToString("dd/MM/yyyy"), evl.evl_dataFim.ToString("dd/MM/yyyy")));

                                msgValidacao = string.Format
                                    (CustomResource.GetGlobalResourceObject("Academico", "Evento.Cadastro.ErroDataLimite").ToString()
                                    , sLimites.ToString());
                                break;
                            }

                            temp = temp.AddDays(1);
                        }
                        while (temp <= dataFim);
                    }
                }

                // Não permite a escola criar evento se existir limite de alcance DRE, para o mesmo tipo de evento
                if (!string.IsNullOrEmpty(msgValidacao) && vis_id == SysVisaoID.UnidadeAdministrativa && esc_id > 0)
                {
                    foreach (int cal_id in calendarios)
                    {
                        var limitesCalendario = limitesUad.Where(evl => evl.cal_id == cal_id);
                        if (limitesCalendario.Any() && limitesCalendario.Any(evl => Convert.ToDateTime(evt_inicio) >= evl.evl_dataInicio && Convert.ToDateTime(evt_inicio) <= evl.evl_dataFim))
                            msgValidacao = "Entre em contato com a sua DRE para criação do evento.";
                    }
                }
            }

            return string.IsNullOrEmpty(msgValidacao);
        }
        
        /// <summary>
        /// Verifica se existem aulas criadas no calendário da escola entre as datas informadas
        /// </summary>
        /// <param name="uni_id">ID do calendário.</param>
        /// <param name="esc_id">ID da escola.</param>
        /// <param name="data_ini">Data inicial</param>
        /// <param name="data_fim">Data final</param>
        /// <returns>Flag indicando se foram encontradas aulas.</returns>
        public static bool VerificaAulaPorCalendarioEscolaData(string cal_id, int esc_id, DateTime data_ini, DateTime data_fim)
        {
            return new ACA_EventoDAO().VerificaAulaPorCalendarioEscolaData(cal_id, esc_id, data_ini, data_fim);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaNomeExistente(ACA_Evento entity, string cal_ids)
        {
            try
            {
                ACA_EventoDAO dao = new ACA_EventoDAO();
                return dao.SelectBy_Name(entity.evt_nome, entity.evt_id, entity.esc_id, entity.uni_id, entity.ent_id, cal_ids);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Valida as datas de início e fim do evento de acordo com as regras.
        /// </summary>
        /// <param name="entity">Entidade a ser salva - obrigatório.</param>
        /// <param name="msgErro">Mensagem de erro disparada - obrigatório.</param>
        /// <param name="bancoGestao">Transação com banco - obrigatório.</param>
        /// <returns>Flag indicando se a validação teve sucesso.</returns>
        public static bool ValidaDatas(ACA_Evento entity, out string msgErro, TalkDBTransaction bancoGestao)
        {
            msgErro = "";

            if (entity.evt_dataInicio.Date > entity.evt_dataFim.Date)
            {
                msgErro = "Data de início do evento deve ser menor que data de fim do evento.";
                return false;
            }

            ACA_Evento entAux = new ACA_Evento
            {
                evt_id = entity.evt_id
            };
            GetEntity(entAux, bancoGestao);

            if ((!entAux.IsNew) && (entAux.evt_dataFim != entity.evt_dataFim) &&
                (entity.evt_dataFim.Date < DateTime.Now.Date))
            {
                // Se mudou a data fim - e a data fim for menor que hoje.
                msgErro = "Data de fim do evento deve ser maior que a data atual.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Valida se o evento a ser salvo é padrão ou
        /// SE nao for padrao tem que escola
        /// </summary>
        /// <param name="entity">Entidade do evento a ser salvo</param>
        /// <returns>TRUE - qdo a entidade tem as informacoes corretamente (ou padrao ou escola)</returns>
        public static bool verificaPadraoOuSelecaoEscola(ACA_Evento entity)
        {
            return ((!entity.evt_padrao) && (entity.esc_id <= 0));
        }

        #endregion

        #region Saves

        /// <summary>
        /// Overload do metodo salvar acima porém recebe Calendários a serem excluidos
        /// Usado na alteracao qdo é 'desassociado' algum  calendário do evento
        /// </summary>
        /// <param name="entity">Entidade de Evento</param>
        /// <param name="dtCalendario">TAbela com calendários associados</param>
        /// <param name="dtExcluidos">TAbela com calendarios desassociados</param>
        /// <param name="bValidaDataInicial">Se valida data inicial faz teste se data é igual à de hoje e dá mensagem</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <param name="vis_id">Id da visão do grupo vinculado ao usuário logado.</param>
        /// <returns>TRUE - salvou com sucesso</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static bool Salvar
        (
            ACA_Evento entity
            , DataTable dtCalendario
            , DataTable dtExcluidos
            , bool bValidaDataInicial
            , Guid ent_id
            , int vis_id
            , List<Guid> lstUadIdPermissao
        )
        {
            TalkDBTransaction bancoGestao = new ACA_EventoDAO()._Banco.CopyThisInstance();
            bancoGestao.Open(IsolationLevel.ReadCommitted);

            TalkDBTransaction bancoCore = new SYS_EntidadeDAO()._Banco.CopyThisInstance();
            bancoCore.Open(IsolationLevel.ReadCommitted);

            try
            {
                string msgErro;
                DateTime dataAtual = DateTime.Today;
                IEnumerable<int> lst_calIds = (from DataRow row in dtCalendario.Rows
                                   select Convert.ToInt32(row["cal_id"])).ToList();
                string cal_ids = string.Join(";", lst_calIds);

                // Valida os campos de data da entidade.
                if (!ValidaDatas(entity, out msgErro, bancoGestao))
                {
                    throw new ArgumentException(msgErro);
                }
                if (VerificaNomeExistente(entity, cal_ids))
                {
                    throw new ArgumentException("Já existe um evento cadastrado com este nome.");
                }
                if (verificaPadraoOuSelecaoEscola(entity))
                {
                    throw new ValidationException("É necessário selecionar uma escola ou o evento do calendário deve ser padrão.");
                }
                // Bug #9852
                if (bValidaDataInicial && entity.evt_semAtividadeDiscente)
                {
                    // valida se data do inicio do evento é anterior à data de hoje
                    if (entity.evt_dataInicio == dataAtual)
                        throw new DataAnteriorException();

                    if (entity.evt_dataInicio < dataAtual
                         && !ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PERMITIR_CADASTRO_EVENTO_RETROATIVO_SEM_ATIVIDADE_DISCENTE, ent_id))
                    // Mostra a mensagem
                    // Qdo o flag sem atividade estiver marcado e o parâmetro 'PERMITIR_CADASTRO_EVENTO_RETROATIVO_SEM_ATIVIDADE_DISCENTE' for false

                    // Ignora a mensagem
                    // Qdo o flag sem atividade estiver marcado e o parâmetro 'PERMITIR_CADASTRO_EVENTO_RETROATIVO_SEM_ATIVIDADE_DISCENTE' for true
                    {
                        throw new ArgumentException("Data de início do evento deve ser maior ou igual à data atual.");
                    }
                }

                // Buscar o tipo de evento para validações.
                ACA_TipoEvento entTipoEvento = ACA_TipoEventoBO.GetEntity
                    (new ACA_TipoEvento { tev_id = entity.tev_id }, bancoGestao);

                if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade
                    (eChaveAcademico.EVENTOEFETIVACAO_BLOQUEAR_CADASTRO_ANTES_FIM_PERIODO, ent_id)
                    && ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade
                    (eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, ent_id) == entTipoEvento.tev_id)
                {
                    // Bloquear o cadastro de eventos de fechamento/efetivação de bimestre, que terminam 
                    // antes do fim do bimestre. Task #28150.
                    foreach (int cal_id in lst_calIds)
                    {
                        // Validar o bimestre de cada calendário.
                         DatasPeriodosCalendario datas = 
                             ACA_CalendarioPeriodoBO.SelecionaDatasCalendario(cal_id, entity.tpc_id, bancoGestao);
                        
                        if (entity.evt_dataFim.Date < datas.cap_dataFim.Date)
                        {
                            throw new ValidationException
                                ("Não é possível cadastrar o evento, pois o " + entTipoEvento.tev_nome.ToLower() +
                                 " não pode terminar antes do último dia do " + 
                                 GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(ent_id).ToLower() + ".");
                        }
                    }
                }

                // valido a data de início do evento para não ser menor que a do início do bimestres
                if (entity.evt_semAtividadeDiscente)
                {
                    foreach (int calId in lst_calIds)
                    {
                        var dataInicioPeriodo = ACA_CalendarioPeriodoBO.SelecionaPor_Calendario(calId).FirstOrDefault().cap_dataInicio;

                        if ((entity.evt_dataInicio < dataInicioPeriodo) || dataInicioPeriodo == new DateTime())
                        {
                            throw new ValidationException(CustomResource.GetGlobalResourceObject("BLL", "Evento.Cadastro.ValidacaoDataInicioEventoDentroPeriodoCalendario").ToString());
                        }
                    }
                }

                if (entity.Validate())
                {
                    string msg;
                    if (!ValidarLimite(true, entTipoEvento
                       , (ACA_TipoEventoBO.eLiberacao)entTipoEvento.tev_liberacao == ACA_TipoEventoBO.eLiberacao.Desnecessaria
                            ? null
                            : ACA_EventoLimiteBO.GetSelectByTipoEvento(entTipoEvento.tev_id)
                       , entity.evt_padrao
                       , entity.esc_id, entity.uni_id, entity.tpc_id, lst_calIds
                       , entity.evt_dataInicio.ToString(), entity.evt_dataFim.ToString()
                       , vis_id
                       , out msg
                       , lstUadIdPermissao
                       ))
                    {
                        throw new ValidationException(msg);
                    }

                    Save(entity, bancoGestao);
                }
                else
                {
                    throw new ValidationException(UtilBO.ErrosValidacao(entity));
                }

                if (dtExcluidos != null)
                {
                    ACA_CalendarioEvento entCEventoExclui = new ACA_CalendarioEvento();

                    foreach (DataRow rowEx in dtExcluidos.Rows)
                    {
                        entCEventoExclui.cal_id = Convert.ToInt32(rowEx["cal_id"]);
                        entCEventoExclui.evt_id = entity.evt_id;

                        ACA_CalendarioEventoBO.GetEntity(entCEventoExclui, bancoGestao);

                        ACA_CalendarioEventoBO.Delete(entCEventoExclui, bancoGestao);
                    }
                }

                ACA_CalendarioEvento entityCalendarioEvento = new ACA_CalendarioEvento();

                foreach (DataRow rowEv in dtCalendario.Rows)
                {
                    int cal_id = Convert.ToInt32(rowEv["cal_id"].ToString());
                    entityCalendarioEvento.cal_id = cal_id;
                    entityCalendarioEvento.evt_id = entity.evt_id;

                    if (ACA_CalendarioEventoBO.Select_Load(entityCalendarioEvento).Rows.Count <= 0)
                        ACA_CalendarioEventoBO.Save(entityCalendarioEvento, bancoGestao);

                    GestaoEscolarUtilBO.LimpaCache(string.Format(ACA_TipoPeriodoCalendarioBO.chaveCache_SelecionaPor_PeriodoVigente_EventoEfetivacaoVigente + "_{0}", cal_id.ToString()));
                    GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.Cache_SelecionaDisciplinaPorTurmaDocente);

                    CacheManager.Factory.RemoveByPattern(ModelCache.TURMA_DISCIPLINA_SELECIONA_DISCIPLINA_POR_TURMADOCENTE_SEM_VIGENCIA_PATTERN_KEY);
                }

                if (entity.IsNew)
                {
                    SYS_EntidadeDAO entDao = new SYS_EntidadeDAO { _Banco = bancoCore };

                    // Incrementa um na integridade da entidade.
                    entDao.Update_IncrementaIntegridade(entity.ent_id);
                }

                #region Remove o cache

                List<int> listCalendario = new List<int>();

                listCalendario.AddRange((from DataRow dr in dtCalendario.Rows select Convert.ToInt32(dr["cal_id"])).Distinct().ToList());
                listCalendario.AddRange((from DataRow dr in dtExcluidos.Rows select Convert.ToInt32(dr["cal_id"])).Distinct().ToList());

                LimpaCache_EventoEfetivacaoTodos(listCalendario);

                foreach (int cal_id in lst_calIds)
                    CacheManager.Factory.RemoveByPattern(string.Format(ModelCache.EVENTOS_CALENDARIO_TURMA_TIPO_PERIODO_PATTERN_KEY + "_{0}", cal_id.ToString()));

                CacheManager.Factory.RemoveByPattern(ModelCache.EVENTOS_EFETIVACAO_POR_TIPO_PERIODO_ESCOLA_PATTERN_KEY);
                #endregion Remove o cache

                return true;
            }
            catch (DataAnteriorException ex)
            {
                bancoGestao.Close(ex);
                bancoCore.Close(ex);

                throw;
            }
            catch (Exception ex)
            {
                bancoGestao.Close(ex);
                bancoCore.Close(ex);

                throw;
            }
            finally
            {
                bancoGestao.Close();
                bancoCore.Close();
            }
        }

        /// <summary>
        /// Deleta logicamente o evento
        /// </summary>
        /// <param name="entity">Entidade ACA_Evento</param>
        /// <returns>True = deletado/alterado | False = não deletado/alterado</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Delete
        (
            ACA_Evento entity
        )
        {
            ACA_EventoDAO evtDao = new ACA_EventoDAO();
            evtDao._Banco.Open(IsolationLevel.ReadCommitted);

            SYS_EntidadeDAO entDao = new SYS_EntidadeDAO();
            entDao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                DataTable dtCalendario = ACA_CalendarioEventoBO.Select_Associados(entity.ent_id, entity.evt_id);

                // Verificar se existem dependências para o evento.
                if (GestaoEscolarUtilBO.VerificarIntegridade
                (
                    "evt_id"
                    , entity.evt_id.ToString()
                    , "ACA_Evento,ACA_CalendarioEvento"
                    , evtDao._Banco
                ))
                {
                    throw new ValidationException("Não é possível excluir o evento pois possui outros registros ligados a ele.");
                }

                //Decrementa um na integridade da entidade
                entDao.Update_DecrementaIntegridade(entity.ent_id);

                //Deleta logicamente o evento
                evtDao.Delete(entity);

                #region Remove o cache

                List<int> listCalendario = new List<int>();

                listCalendario.AddRange((from DataRow dr in dtCalendario.Rows select Convert.ToInt32(dr["cal_id"])).Distinct().ToList());

                LimpaCache_EventoEfetivacaoTodos(listCalendario);

                #endregion Remove o cache

                return true;
            }
            catch (Exception err)
            {
                evtDao._Banco.Close(err);
                entDao._Banco.Close(err);

                throw;
            }
            finally
            {
                evtDao._Banco.Close();
                entDao._Banco.Close();
            }
        }

        #endregion

        #region Cache

        /// <summary>
        /// Retorna a chave do cache utilizada para guardar os eventos da efetivação
        /// </summary>
        /// <returns>Chave</returns>
        private static string RetornaChaveCache_EventoEfetivacaoTodos(int cal_id, long tur_id, string tev_id, Int32 tpc_id, bool vigente)
        {
            return string.Format(ModelCache.EVENTOS_CALENDARIO_TURMA_TIPO_PERIODO_MODEL_KEY, cal_id, tur_id, tev_id, (tpc_id <= 0 ? 0 : tpc_id), vigente);
        }

        /// <summary>
        /// Remove do cache as pesquisas que um evento faz parte
        /// </summary>
        /// <param name="listCalendarios">Lista de calendarios assosiados ou que foram excluidos do evento</param>
        private static void LimpaCache_EventoEfetivacaoTodos(List<int> listCalendarios)
        {
            if (HttpContext.Current != null)
            {
                listCalendarios.ForEach(x =>
                {
                    GestaoEscolarUtilBO.LimpaCache(string.Format("Cache_EventoEfetivacaoTodos_{0}_", x));
                });
            }
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para guardar os eventos da efetivação por período da escola.
        /// </summary>
        /// <returns>Chave</returns>
        private static string RetornaChaveCache_EventoEfetivacaoTodosPorPeriodoEscola(int cal_id, int cap_id, int esc_id, int uni_id, string tev_id, bool vigente)
        {
            return string.Format(ModelCache.EVENTOS_EFETIVACAO_POR_TIPO_PERIODO_ESCOLA_MODEL_KEY, cal_id, cap_id, esc_id, uni_id, tev_id, vigente);
        }
        #endregion
    }

}