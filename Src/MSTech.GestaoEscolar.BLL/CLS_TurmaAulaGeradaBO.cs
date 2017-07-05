/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System.ComponentModel;
    using System.Data;
    using System.Collections.Generic;
    using MSTech.Data.Common;
    using System;
    using System.Linq;
    using MSTech.Validation.Exceptions;
    using MSTech.GestaoEscolar.CustomResourceProviders;

    public enum CLS_TurmaAulaGeradaDiaSemana : byte
    {
        Segunda = 1
        ,
        Terca = 2
            ,
        Quarta = 3
            ,
        Quinta = 4
            ,
        Sexta = 5
            ,
        Sabado = 6
            , Domingo = 7
    }

    /// <summary>
    /// Description: CLS_TurmaAulaGerada Business Object. 
    /// </summary>
    public class CLS_TurmaAulaGeradaBO : BusinessBase<CLS_TurmaAulaGeradaDAO, CLS_TurmaAulaGerada>
    {
        /// <summary>
        /// Retorna os dias da semana e os nomes onde é possível gerar aulas.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<CLS_TurmaAulaGeradaDiaSemana, string> RetornaDiasSemanaGerarAulas()
        {
            Dictionary<CLS_TurmaAulaGeradaDiaSemana, string> retorno = new Dictionary<CLS_TurmaAulaGeradaDiaSemana, string>();

            retorno.Add(CLS_TurmaAulaGeradaDiaSemana.Segunda,
                Enum.GetName(typeof(CLS_TurmaAulaGeradaDiaSemana), CLS_TurmaAulaGeradaDiaSemana.Segunda));
            retorno.Add(CLS_TurmaAulaGeradaDiaSemana.Terca,
                Enum.GetName(typeof(CLS_TurmaAulaGeradaDiaSemana), CLS_TurmaAulaGeradaDiaSemana.Terca));
            retorno.Add(CLS_TurmaAulaGeradaDiaSemana.Quarta,
                Enum.GetName(typeof(CLS_TurmaAulaGeradaDiaSemana), CLS_TurmaAulaGeradaDiaSemana.Quarta));
            retorno.Add(CLS_TurmaAulaGeradaDiaSemana.Quinta,
                Enum.GetName(typeof(CLS_TurmaAulaGeradaDiaSemana), CLS_TurmaAulaGeradaDiaSemana.Quinta));
            retorno.Add(CLS_TurmaAulaGeradaDiaSemana.Sexta,
                Enum.GetName(typeof(CLS_TurmaAulaGeradaDiaSemana), CLS_TurmaAulaGeradaDiaSemana.Sexta));
            retorno.Add(CLS_TurmaAulaGeradaDiaSemana.Sabado,
                Enum.GetName(typeof(CLS_TurmaAulaGeradaDiaSemana), CLS_TurmaAulaGeradaDiaSemana.Sabado));

            return retorno;
        }

        /// <summary>
        /// Retorna todas as turmas com suas respectivas disciplinas.
        /// <param name="doc_id">Id do docente.</param>
        /// <param name="cal_id">Id do calendário</param>
        /// <param name="tpc_id">Id do docente.</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GerarAula(long doc_id, int cal_id, int tpc_id, Guid ent_id)
        {
            bool ordenaCodigoEscola = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id);
            return new CLS_TurmaAulaGeradaDAO().GerarAula(doc_id, ordenaCodigoEscola, cal_id, tpc_id);
        }

        /// <summary>
        /// Retorna as entidades das aulas geradas.
        /// </summary>
        public static List<CLS_TurmaAulaGerada> SelectBy_DisciplinaDocente(string tud_id, long doc_id, int tpc_id, TalkDBTransaction banco = null)
        {
            CLS_TurmaAulaGeradaDAO dao = new CLS_TurmaAulaGeradaDAO();
            if (banco != null)
                dao._Banco = banco;
            DataTable dt = dao.SelectBy_DisciplinaDocente(tud_id, doc_id, tpc_id);

            return (from DataRow dr in dt.Rows
                    select dao.DataRowToEntity(dr, new CLS_TurmaAulaGerada())).ToList();
        }

        /// <summary>
        /// Inclui ou altera o planejamento.
        /// </summary>
        /// <param name="lstAula">Lista da aula CLS_TurmaAulaGerada</param>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static bool Salvar(List<CLS_TurmaAulaGerada> lstAula)
        {
            TalkDBTransaction banco = new CLS_TurmaAulaGeradaDAO()._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                foreach (CLS_TurmaAulaGerada aula in lstAula)
                {
                    Save(aula, banco);
                }

                return true;
            }
            catch (Exception ex)
            {
                banco.Close(ex);
                throw ex;
            }
            finally
            {
                if (banco.ConnectionIsOpen)
                    banco.Close();
            }
        }

        /// <summary>
        /// Retorna o dia da semana.
        /// </summary>
        /// <param name="tag_diaSemana">Byte que representa um dia da semana.</param>
        /// <returns></returns>
        private static DayOfWeek RetornaDiaSemana(byte tag_diaSemana)
        {
            switch (tag_diaSemana)
            {
                case 1:
                    return DayOfWeek.Monday;
                case 2:
                    return DayOfWeek.Tuesday;
                case 3:
                    return DayOfWeek.Wednesday;
                case 4:
                    return DayOfWeek.Thursday;
                case 5:
                    return DayOfWeek.Friday;
                case 6:
                    return DayOfWeek.Saturday;
                default:
                    throw new ValidationException("Dia da semana inválido.");
            }
        }

        /// <summary>
        /// Gera as aulas do planejamento diário.
        /// </summary>
        /// <param name="tagsSalvar">Lista de CLS_TurmaAulaGerada para gravar</param>
        /// <param name="doc_id">Docente para quem devem ser geradas as CLS_TurmaAula correspondentes</param>
        /// <param name="usu_id">ID do usuário logado.</param>
        /// <param name="ent_id">ID da entidade do usuário logado</param>
        /// <param name="gerouTodasAulas">Flag que indica se todas as aulas foram salvas.</param>
        /// <param name="ultrapassouCargaHorariaSemanal">Lista de alertas para os itens que não foram plenamente atendidos.</param>
        /// <returns></returns>
        public static bool GerarAulasPlanejamentoDiario
        (
            List<CLS_TurmaAulaGerada> tagsSalvar,
            int tpc_id,
            long doc_id,
            DateTime? dataInicial,
            DateTime? dataFinal,
            Guid usu_id,
            Guid ent_id,
            Dictionary<long, string> dicTurmasDisciplinas,
            out bool gerouTodasAulas,
            out Dictionary<long, string> ultrapassouCargaHorariaSemanal,
            out Dictionary<long, string> semVigencia,
            out Dictionary<long, string> semAulasPrevistas,
            out Dictionary<long, Exception> outrosErros,
            byte origemLogAula = 0
        )
        {
            gerouTodasAulas = true;
            ultrapassouCargaHorariaSemanal = new Dictionary<long, string>();
            semVigencia = new Dictionary<long, string>();
            semAulasPrevistas = new Dictionary<long, string>();
            outrosErros = new Dictionary<long, Exception>();
            Dictionary<long, string> ultrapassouCargaHorariaSemanalTerritorio = new Dictionary<long, string>();

            // armazenará os períodos de cada calendário
            var dicPeriodosCalendario = new Dictionary<int, DataTable>();
            // armazenará os dias não-úteis de cada escola
            var dicDiasNaoUteis = new Dictionary<int, List<DateTime>>();

            DateTime dataLimiteLancamento = new DateTime();
            string dataBloqueio = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.DATA_VALIDADE_BLOQUEIO_ACESSO_MINHAS_TURMAS, ent_id);
            if (!string.IsNullOrEmpty(dataBloqueio))
            {
                if (!DateTime.TryParse(dataBloqueio, out dataLimiteLancamento))
                    dataLimiteLancamento = new DateTime();
                    //throw new ValidationException("A data de bloqueio informada no parâmetro não é válida.");
            }

            #region Valida itens que bloqueiam toda a execução e preenche listas de calendários

            // utilizadas na verificação de carga horária semanal, cuja soma precisa ser verificada por tud_id
            long tud_id_anterior = 0;
            int tud_cargaHorariaSemanal = 0;

            foreach (var tag in tagsSalvar.OrderBy(tag => tag.tud_id))
            {
                if (tud_id_anterior != tag.tud_id)
                {
                    tud_id_anterior = tag.tud_id;
                    tud_cargaHorariaSemanal = 0;
                }

                tud_cargaHorariaSemanal += tag.tag_numeroAulas;
                if (tud_cargaHorariaSemanal > tag.tud_cargaHorariaSemanal)
                {
                    throw new ValidationException(string.Format("{0} excede a carga horária semanal permitida.", dicTurmasDisciplinas[tag.tud_id]));
                }

                var cap_dataInicio = new DateTime();
                var cap_dataFim = new DateTime();

                #region Preenche as listas de calendários, cap_dataInicio e cap_dataFim

                DataTable cal = null;

                if (dicPeriodosCalendario.Any(i => i.Key == tag.cal_id))
                {
                    cal = dicPeriodosCalendario.First(i => i.Key == tag.cal_id).Value;
                }
                else
                {
                    cal = ACA_TipoPeriodoCalendarioBO.SelecionaTipoPeriodoCalendarioPorTipoPeriodoCalendario(tpc_id, tag.cal_id);
                    dicPeriodosCalendario.Add(tag.cal_id, cal);
                }

                cap_dataInicio = Convert.ToDateTime(cal.Rows[0]["cap_dataInicio"]);
                cap_dataFim = Convert.ToDateTime(cal.Rows[0]["cap_dataFim"]);

                if (!dicDiasNaoUteis.Any(i => i.Key == tag.esc_id))
                {
                    dicDiasNaoUteis.Add(tag.esc_id, ACA_CalendarioPeriodoBO.SelecionaDiasNaoUteis(tag.esc_id, tag.uni_id, tag.cal_id, cap_dataInicio, cap_dataFim, ent_id));
                }

                #endregion

                if (dataInicial.HasValue
                    && (dataInicial < cap_dataInicio || dataInicial > cap_dataFim))
                {
                    throw new ArgumentException(String.Format(CustomResource.GetGlobalResourceObject("BLL", "TurmaAula.ValidaDataInicioBimestre"),
                                                              cap_dataInicio.ToShortDateString(),
                                                              cap_dataFim.ToShortDateString()));
                }

                if (dataFinal.HasValue
                    && (dataFinal < cap_dataInicio || dataFinal > cap_dataFim))
                {
                    throw new ArgumentException(String.Format(CustomResource.GetGlobalResourceObject("BLL", "TurmaAula.ValidaDataFimBimestre"),
                                                              cap_dataInicio.ToShortDateString(),
                                                              cap_dataFim.ToShortDateString()));
                }
            }

            #endregion

            var dao = new CLS_TurmaAulaGeradaDAO();
            foreach (var tud_id in tagsSalvar.Select(tag => tag.tud_id).Distinct())
            {
                var banco = dao._Banco.CopyThisInstance();

                try
                {
                    banco.Open(IsolationLevel.ReadCommitted);

                    #region Busca dados iniciais do tud.

                    var tagsByTud = tagsSalvar.Where(t => t.tud_id == tud_id).ToList();

                    var tud_tipo = tagsByTud.First().tud_tipo;

                    var fav_tipoApuracaoFrequencia = tagsByTud.First().fav_tipoApuracaoFrequencia;

                    var ttn_tipo = tagsByTud.First().ttn_tipo;

                    if (((tud_tipo == (byte)TurmaDisciplinaTipo.Regencia && fav_tipoApuracaoFrequencia != (byte)ACA_FormatoAvaliacaoTipoApuracaoFrequencia.TemposAula) ||
                         (tud_tipo == (byte)TurmaDisciplinaTipo.DisciplinaPrincipal && ttn_tipo != (byte)ACA_TipoTurnoBO.TipoTurno.Integral)) && tagsByTud.Any(tag => tag.tag_numeroAulas > 1))
                    {
                        throw new ValidationException(string.Format("A carga horária diária do componente {0} não deve ser maior que 1.", dicTurmasDisciplinas[tud_id]));
                    }

                    if (tud_tipo == (byte)TurmaDisciplinaTipo.DisciplinaPrincipal && ttn_tipo == (byte)ACA_TipoTurnoBO.TipoTurno.Integral && tagsByTud.Any(tag => tag.tag_numeroAulas > 2))
                    {
                        throw new ValidationException(string.Format("A carga horária diária do componente {0} não deve ser maior que 2.", dicTurmasDisciplinas[tud_id]));
                    }

                    tud_cargaHorariaSemanal = tagsByTud.First().tud_cargaHorariaSemanal;

                    var cal = dicPeriodosCalendario.First(i => i.Key == tagsByTud.First().cal_id).Value;
                    var inicio = dataInicial.GetValueOrDefault(Convert.ToDateTime(cal.Rows[0]["cap_dataInicio"]));
                    var fim = dataFinal.GetValueOrDefault(Convert.ToDateTime(cal.Rows[0]["cap_dataFim"]));

                    var cap_dataInicio = dataInicial.GetValueOrDefault(Convert.ToDateTime(cal.Rows[0]["cap_dataInicio"]));
                    var cap_dataFim = dataFinal.GetValueOrDefault(Convert.ToDateTime(cal.Rows[0]["cap_dataFim"]));

                    string tud_ids = tud_id.ToString();

                    List<TUR_TurmaDisciplinaTerritorio> territorios = null;
                    if (tagsByTud.First().tud_tipo == (byte)TurmaDisciplinaTipo.Experiencia)
                    {
                        territorios = TUR_TurmaDisciplinaTerritorioBO.SelecionaVigentesPorExperienciaPeriodo(tud_id, inicio, fim);
                        tud_ids = territorios.Aggregate(tud_ids, (a, i) => string.Format("{0};{1}", a, i.tud_idTerritorio));

                        // Quantidade de aulas máxima da experiência: quantidade de territórios ligados à ela.
                        tud_cargaHorariaSemanal = territorios.Count;
                        //if (tagsByTud.Sum(p => p.tag_numeroAulas) > tud_cargaHorariaSemanal)
                        //{
                        //    throw new ValidationException(string.Format("A carga horária semanal do componente {0} não deve ser maior que {1}."
                        //        , dicTurmasDisciplinas[tud_id], tud_cargaHorariaSemanal));
                        //}
                    }

                    // Recupera as CLS_TurmaAula que já existem no banco para o docente na turma e período informado
                    // Territórios do saber: se tud_id for relativo a uma experiência, buscará as aulas também dos territórios
                    var aulasBanco = CLS_TurmaAulaBO.SelecionaAulasAtividadesPor_DisicplinasDocentePeriodo(tud_ids, tpc_id, doc_id, banco).Rows.OfType<DataRow>().ToList();

                    // Recupera as disciplinas compartilhadas vigentes para o docente na turma
                    var lstDisciplinaCompartilhada = TUR_TurmaDisciplinaBO.SelectRelacionadaVigenteBy_DisciplinaCompartilhada(tud_id, 0, false, doc_id, banco);
                    
                    var diasNaoUteis = dicDiasNaoUteis.First(i => i.Key == tagsByTud.First().esc_id).Value;

                    var aulasSalvar = new Dictionary<long[], CLS_TurmaAula>();
                    var aulasExcluir = new List<CLS_TurmaAula>();

                    // esta lista totalizará as aulas que *permanecerão* na semana 
                    // (considerando também as que permanecerão inalteradas no banco)
                    // após as gravações das listas aulasSalvar e aulasExcluir
                    var aulasSemana = new List<CLS_TurmaAula>();

                    // variáveis usadas na geração de msgs de erro de vigência dos territórios do saber
                    bool temTerritorioVigente = true;
                    bool temTerritorioVigenteDiaAnterior = true;
                    DateTime vigenciaInicio = inicio;
                    DateTime vigenciaFim = inicio;

                    #endregion

                    while (inicio <= fim)
                    {
                        foreach (var tag in tagsByTud.Where(t => inicio.DayOfWeek == RetornaDiaSemana(t.tag_diaSemana)))
                        {
                            #region Percorre os dias da semana configurado com aula na agenda

                            if (!diasNaoUteis.Any(d => d.Date == inicio.Date))
                            {
                                #region Territórios do Saber - Recupera dados para verificações

                                //guarda os territórios que efetivamente estão vigentes para a experiência na data em questão
                                var territoriosVigentes = (territorios == null)
                                    ? null
                                    : territorios.Where(t => t.tte_vigenciaInicio.Date <= inicio.Date && (t.tte_vigenciaFim == new DateTime() || t.tte_vigenciaFim.Date >= inicio.Date)).ToList();

                                //guarda as aulas que já estão criadas no banco para os territórios vigentes na data em questão
                                var aulasTerritoriosBanco = (territorios == null)
                                    ? null
                                    : aulasBanco.Where(dr =>
                                                    territoriosVigentes.Select(t => t.tud_idTerritorio).Contains(Convert.ToInt64(dr["tud_id"]))
                                                    && Convert.ToInt16(dr["tdt_posicao"]) == tag.tdt_posicao
                                                    && Convert.ToDateTime(dr["tau_data"]) == inicio)
                                                .Select(dr =>
                                                    new
                                                    {
                                                        PermiteAlterar = Convert.ToBoolean(dr["PermiteAlterar"]),
                                                        Aula = new CLS_TurmaAulaDAO().DataRowToEntity(dr, new CLS_TurmaAula())
                                                    })
                                                .ToList();

                                #endregion

                                #region Recupera aulas existentes no banco de dados

                                var aulaBanco = aulasBanco
                                    .Where(dr =>
                                        Convert.ToInt64(dr["tud_id"]) == tag.tud_id
                                        && Convert.ToInt16(dr["tdt_posicao"]) == tag.tdt_posicao
                                        && Convert.ToDateTime(dr["tau_data"]) == inicio
                                        && (tag.tud_idRelacionada <= 0 || Convert.ToInt64(dr["tud_idRelacionada"]) == tag.tud_idRelacionada))
                                    .Select(dr =>
                                        new
                                        {
                                            /* Territórios do saber: 
                                             * só poderá editar a aula da experiência se ela E as aulas 
                                             * correspondentes de TODOS os territórios vigentes na data 
                                             * da aula puderem ser editadas também */
                                            PermiteAlterar = Convert.ToBoolean(dr["PermiteAlterar"])
                                                           && (territorios == null
                                                            || !aulasTerritoriosBanco.Any(a => !a.PermiteAlterar)),
                                            Aula = new CLS_TurmaAulaDAO().DataRowToEntity(dr, new CLS_TurmaAula())
                                        })
                                    .FirstOrDefault();

                                #endregion

                                temTerritorioVigente = (territorios == null || territoriosVigentes.Count > 0);

                                if (tag.tag_numeroAulas > 0)
                                {
                                    // Há aulas previstas para esse dia da semana
                                    if (aulaBanco == null)
                                    {
                                        #region Cria aula

                                        if (temTerritorioVigente)
                                        {
                                            var nova = new CLS_TurmaAula
                                                        {
                                                            tud_id = tag.tud_id,
                                                            tau_id = -1,
                                                            tur_id = tag.tur_id,
                                                            tpc_id = tpc_id,
                                                            tau_data = inicio,
                                                            tau_sequencia = -1,
                                                            tau_numeroAulas = tag.tag_numeroAulas,
                                                            tau_situacao = 1,
                                                            tdt_posicao = Convert.ToByte(tag.tdt_posicao > 0 ? tag.tdt_posicao : 1),
                                                            usu_id = usu_id,
                                                            IsNew = true,
                                                            tud_tipo = tag.tud_tipo
                                                        };

                                            aulasSalvar.Add(new long[] { tag.tud_id, tag.tud_idRelacionada }, nova);

                                            aulasSemana.Add(nova);
                                        }

                                        #endregion
                                    }
                                    else
                                    {
                                        if (aulaBanco.PermiteAlterar)
                                        {
                                            #region Altera aula existente

                                            if (temTerritorioVigente)
                                            {
                                                aulaBanco.Aula.tau_numeroAulas = tag.tag_numeroAulas;
                                                aulaBanco.Aula.tau_sequencia = -1;
                                                aulaBanco.Aula.tur_id = tag.tur_id;
                                                aulaBanco.Aula.IsNew = false;
                                                aulaBanco.Aula.tud_tipo = tag.tud_tipo;

                                                aulasSalvar.Add(new long[] { tag.tud_id, tag.tud_idRelacionada }, aulaBanco.Aula);
                                            }

                                            #endregion
                                        }
                                        else
                                            gerouTodasAulas = false;

                                        aulasSemana.Add(aulaBanco.Aula);
                                    }
                                }
                                // Não há aulas previstas para esse dia da semana
                                // Exclui as aulas que já estão no calendário (quando possível)
                                else if (aulaBanco != null)
                                {
                                    if (aulaBanco.PermiteAlterar)
                                    {
                                        if (territorios == null || territoriosVigentes.Count > 0)
                                        {
                                            aulaBanco.Aula.tur_id = tag.tur_id;
                                            aulasExcluir.Add(aulaBanco.Aula);
                                        }
                                        else
                                        {
                                            // Se não vai excluir a aula, coloca na lista da semana para contabilizar quantidade de aulas.
                                            aulasSemana.Add(aulaBanco.Aula);
                                        }
                                    }
                                    else
                                    {
                                        gerouTodasAulas = false;
                                        // Se não vai excluir a aula, coloca na lista da semana para contabilizar quantidade de aulas.
                                        aulasSemana.Add(aulaBanco.Aula);
                                    }
                                }

                                #region Territorios do saber - msg de vigência

                                if (temTerritorioVigente && !temTerritorioVigenteDiaAnterior)
                                {
                                    string inicioFimTxt = "";

                                    if (territorios != null && (territorios.Any(t => t.tte_vigenciaInicio.Date <= vigenciaInicio.Date) ||
                                                                territorios.Any(t => t.tte_vigenciaFim != new DateTime() && t.tte_vigenciaFim.Date < vigenciaFim.Date)))
                                    {
                                        if (!territorios.Any(t => t.tte_vigenciaInicio.Date <= vigenciaInicio.Date))
                                            inicioFimTxt = vigenciaInicio.ToString("dd/MM/yyyy") + " - " +
                                                           territorios.Where(t => t.tte_vigenciaInicio.Date > vigenciaInicio.Date)
                                                                      .OrderBy(t => t.tte_vigenciaInicio).First()
                                                                      .tte_vigenciaInicio.ToString("dd/MM/yyyy");

                                        if (territorios.Any(t => t.tte_vigenciaFim != new DateTime() && t.tte_vigenciaFim.Date < vigenciaFim.Date))
                                            foreach (TUR_TurmaDisciplinaTerritorio tte in territorios.Where(t => t.tte_vigenciaFim != new DateTime() && t.tte_vigenciaFim.Date < vigenciaFim.Date)
                                                                                         .OrderBy(t => t.tte_vigenciaInicio))
                                            {
                                                inicioFimTxt += string.IsNullOrEmpty(inicioFimTxt) ? "" : " e ";
                                                if (tte.tte_vigenciaFim.Date < vigenciaFim.Date)
                                                    inicioFimTxt += tte.tte_vigenciaFim.AddDays(1).ToString("dd/MM/yyyy") + " - " +
                                                                    (territorios.Any(t => t.tte_vigenciaInicio > tte.tte_vigenciaFim.Date) ?
                                                                     territorios.Where(t => t.tte_vigenciaInicio > tte.tte_vigenciaFim.Date)
                                                                                .OrderBy(t => t.tte_vigenciaInicio).First()
                                                                                .tte_vigenciaInicio.AddDays(-1).ToString("dd/MM/yyyy") :
                                                                     vigenciaFim.ToString("dd/MM/yyyy"));
                                            }

                                    }
                                    else
                                        inicioFimTxt = vigenciaInicio.ToString("dd/MM/yyyy") + " - " + vigenciaFim.ToString("dd/MM/yyyy");

                                    // mostra msg
                                    if (semVigencia.ContainsKey(tud_id))
                                    {
                                        semVigencia[tud_id] = String.Format("{0}, {1}", semVigencia[tud_id], inicioFimTxt);
                                    }
                                    else
                                    {
                                        semVigencia.Add(tud_id, String.Format("{0}: {1}", dicTurmasDisciplinas[tud_id], inicioFimTxt));
                                    }
                                }

                                if (temTerritorioVigente != temTerritorioVigenteDiaAnterior)
                                {
                                    // reinicia contagem
                                    vigenciaInicio = inicio;
                                }

                                temTerritorioVigenteDiaAnterior = temTerritorioVigente;
                                vigenciaFim = inicio;

                                #endregion
                            }

                            #endregion
                        }

                        // Ao final da semana ou do intervalo informado
                        if (inicio.DayOfWeek == DayOfWeek.Sunday || inicio == fim)
                        {
                            #region Grava os dados da semana

                            DateTime dataIniSemana;
                            DateTime dataFimSemana;

                            #region Calcula datas de início e final da semana em questão

                            if (inicio.DayOfWeek == DayOfWeek.Sunday)
                            {
                                dataIniSemana = inicio.AddDays(-7);
                                dataFimSemana = inicio.AddDays(-1);
                            }
                            else
                            {
                                dataIniSemana = inicio.AddDays((int)inicio.DayOfWeek * (-1));
                                dataFimSemana = fim.AddDays(((int)fim.DayOfWeek - (int)DayOfWeek.Saturday) * (-1));
                                
                            }

                            #endregion
                            
                            int quantidadeAulasDaSemana = aulasSalvar.Sum(p=>p.Value.tau_numeroAulas);

                            if (dataIniSemana.Date < cap_dataInicio.Date)
                            {
                                // Somar com as aulas que estiverem cadastradas fora do tpc_id.
                                quantidadeAulasDaSemana += CLS_TurmaAulaBO.VerificaSomaNumeroAulasSemana
                                        (tud_id, dataIniSemana, cap_dataInicio.Date.AddDays(-1),
                                        banco, Convert.ToByte(tagsByTud.FirstOrDefault().tdt_posicao));
                            }
                            if (dataFimSemana.Date > cap_dataFim.Date)
                            {
                                // Somar com as aulas que estiverem cadastradas fora do tpc_id.
                                quantidadeAulasDaSemana += CLS_TurmaAulaBO.VerificaSomaNumeroAulasSemana
                                        (tud_id, cap_dataFim.Date.AddDays(1), dataFimSemana,
                                        banco, Convert.ToByte(tagsByTud.FirstOrDefault().tdt_posicao));
                            }

                            if ((aulasSalvar.Any() || aulasExcluir.Any()) &&
                                DateTime.Today >= dataLimiteLancamento &&
                                tud_tipo != (byte)ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada &&
                                tud_tipo != (byte)ACA_CurriculoDisciplinaTipo.Experiencia &&
                                !semAulasPrevistas.ContainsKey(tud_id) &&
                                !TUR_TurmaDisciplinaAulaPrevistaBO.VerificaLancamento(tud_id, doc_id, tagsByTud.First().tur_id, tagsByTud.First().cal_id))
                            {
                                semAulasPrevistas.Add(tud_id, dicTurmasDisciplinas[tud_id]);
                            }
                            // Se extrapolou a qtde de horas na semana...
                            else if (quantidadeAulasDaSemana > tud_cargaHorariaSemanal)
                            {
                                // ... e há itens nas listas para gravar...
                                if (aulasSalvar.Any() || aulasExcluir.Any())
                                {
                                    // ... gera mensagem de alerta ao usuário
                                    if (ultrapassouCargaHorariaSemanal.ContainsKey(tud_id))
                                    {
                                        ultrapassouCargaHorariaSemanal[tud_id] = String.Format("{0}, {1} - {2}", ultrapassouCargaHorariaSemanal[tud_id], dataIniSemana.ToString("dd/MM/yyyy"), dataFimSemana.ToString("dd/MM/yyyy"));
                                    }
                                    else
                                    {
                                        ultrapassouCargaHorariaSemanal.Add(tud_id, String.Format("{0}: {1} - {2}", dicTurmasDisciplinas[tud_id], dataIniSemana.ToString("dd/MM/yyyy"), dataFimSemana.ToString("dd/MM/yyyy")));
                                    }
                                }
                            }
                            else
                            {
                                bool gravouTodasTurmaAula = gerouTodasAulas;
                                bool ultrapassouTerritorios = false;

                                if (aulasExcluir.Any())
                                {
                                    aulasExcluir.ForEach(tau => gravouTodasTurmaAula &= CLS_TurmaAulaBO.Delete(tau, banco, origemLogAula, (byte)LOG_TurmaAula_Alteracao_Tipo.ExclusaoAula, usu_id));
                                }

                                if (aulasSalvar.Any())
                                {
                                    // Ligação da experiência com territórios nas aulas da semana.
                                    List<TurmaAulaTerritorioDados> aulasTerritorios = new List<TurmaAulaTerritorioDados>();
                                    Dictionary<long, int> dicTerritorios = new Dictionary<long, int>();

                                    if (tagsByTud.FirstOrDefault().tud_tipo == (byte)TurmaDisciplinaTipo.Experiencia && territorios != null && territorios.Count > 0)
                                    {

                                        // Territórios vigentes dentro da semana.
                                        var territoriosVigentes = territorios.Where
                                            (t => t.tte_vigenciaInicio.Date <= dataFimSemana.Date
                                            && (t.tte_vigenciaFim == new DateTime() || t.tte_vigenciaFim.Date >= dataIniSemana.Date)).ToList();

                                        if (aulasSalvar.Any(tau => tau.Value.tau_numeroAulas > territoriosVigentes.Count))
                                        {
                                            ultrapassouTerritorios = true;

                                            if (ultrapassouCargaHorariaSemanal.ContainsKey(tud_id))
                                            {
                                                ultrapassouCargaHorariaSemanal[tud_id] = String.Format("{0}, {1} - {2}", ultrapassouCargaHorariaSemanal[tud_id], dataIniSemana.ToString("dd/MM/yyyy"), dataFimSemana.ToString("dd/MM/yyyy"));
                                            }
                                            else
                                            {
                                                ultrapassouCargaHorariaSemanal.Add(tud_id, String.Format("{0}: {1} - {2}", dicTurmasDisciplinas[tud_id], dataIniSemana.ToString("dd/MM/yyyy"), dataFimSemana.ToString("dd/MM/yyyy")));
                                            }
                                        }
                                    }

                                    if (!ultrapassouTerritorios)
                                    {
                                        ultrapassouCargaHorariaSemanalTerritorio = ultrapassouCargaHorariaSemanal;

                                        aulasSalvar.ToList()
                                            .ForEach(tau =>
                                            {
                                                gravouTodasTurmaAula &= /*(tau.Value.tud_tipo == (byte)TurmaDisciplinaTipo.Experiencia ? CLS_TurmaAulaBO.SalvarAulaTerritorio(tau.Value, banco): */
                                                    tau.Key[1] > 0
                                                            ? CLS_TurmaAulaBO.Save(tau.Value, banco, lstDisciplinaCompartilhada.First(tdr => tdr.tud_id == tau.Key[1]), origemLogAula, (byte)LOG_TurmaAula_Alteracao_Tipo.AlteracaoAula, usu_id)
                                                            : CLS_TurmaAulaBO.Save(tau.Value, banco, origemLogAula, (byte)LOG_TurmaAula_Alteracao_Tipo.AlteracaoAula, usu_id);

                                                if (tagsByTud.FirstOrDefault().tud_tipo == (byte)TurmaDisciplinaTipo.Experiencia && territorios != null && territorios.Count > 0)
                                                {

                                                    // Territórios vigentes dentro da semana.
                                                    var territoriosVigentes = territorios.Where
                                                        (t => t.tte_vigenciaInicio.Date <= dataFimSemana.Date
                                                        && (t.tte_vigenciaFim == new DateTime() || t.tte_vigenciaFim.Date >= dataIniSemana.Date)).ToList();

                                                    // Ligação da experiência com territórios nas aulas da semana.
                                                    aulasTerritorios = CLS_TurmaAulaTerritorioBO.SelecionaAulasTerritorioPorExperiencia
                                                        (tud_id, dataIniSemana, dataFimSemana, banco);

                                                    dicTerritorios =
                                                        (from TUR_TurmaDisciplinaTerritorio item in
                                                             territoriosVigentes.Where(t => !aulasTerritorios.Any(a => a.tud_idTerritorio == t.tud_idTerritorio &&
                                                                                                                       a.tau_idExperiencia != tau.Value.tau_id))
                                                         select new
                                                         {
                                                             tud_id = item.tud_idTerritorio
                                                             ,
                                                             tud_nomeTerritorio = item.tud_nomeTerritorio
                                                             ,
                                                             qtAulas = (from TurmaAulaTerritorioDados iAula in aulasTerritorios
                                                                        where
                                                                        iAula.tud_idExperiencia == item.tud_idExperiencia
                                                                        && iAula.tud_idTerritorio == item.tud_idTerritorio
                                                                        select iAula.tud_idTerritorio).Count()
                                                         }).OrderBy(p => p.tud_nomeTerritorio).ToDictionary(p => p.tud_id, p => p.qtAulas);

                                                    //Se for uma edição de aula então pega apenas as aulas ligadas à ela
                                                    aulasTerritorios = aulasTerritorios.Where(a => a.tud_idExperiencia == tau.Value.tud_id &&
                                                                                                   a.tau_idExperiencia == tau.Value.tau_id).ToList();

                                                    //Valida a carga horária da experiência
                                                    if (!dicTerritorios.Any() && !aulasTerritorios.Any())
                                                    {
                                                        gravouTodasTurmaAula &= false;

                                                        // ... gera mensagem de alerta ao usuário
                                                        if (ultrapassouCargaHorariaSemanalTerritorio.ContainsKey(tud_id))
                                                        {
                                                            ultrapassouCargaHorariaSemanalTerritorio[tud_id] = String.Format("{0}, {1} - {2}", ultrapassouCargaHorariaSemanalTerritorio[tud_id], dataIniSemana.ToString("dd/MM/yyyy"), dataFimSemana.ToString("dd/MM/yyyy"));
                                                        }
                                                        else
                                                        {
                                                            ultrapassouCargaHorariaSemanalTerritorio.Add(tud_id, String.Format("{0}: {1} - {2}", dicTurmasDisciplinas[tud_id], dataIniSemana.ToString("dd/MM/yyyy"), dataFimSemana.ToString("dd/MM/yyyy")));
                                                        }
                                                    }
                                                    else
                                                    {
                                                        // Verifica ligações com territórios quando a aula é de experiência.
                                                        CLS_TurmaAulaBO.CriaLigacoesTerritorios(usu_id, origemLogAula, tau.Value, banco, aulasTerritorios, dicTerritorios);
                                                    }
                                                }
                                            });

                                        ultrapassouCargaHorariaSemanal = ultrapassouCargaHorariaSemanalTerritorio;

                                    }
                                }
                                gerouTodasAulas &= gravouTodasTurmaAula;
                            }

                            aulasSalvar.Clear();
                            aulasExcluir.Clear();
                            aulasSemana.Clear();

                            #endregion
                        }

                        inicio = inicio.AddDays(1);
                    }

                    #region Territorios do saber - msg de vigência

                    if (!temTerritorioVigente)
                    {
                        string inicioFimTxt = "";

                        if (territorios != null && (territorios.Any(t => t.tte_vigenciaInicio.Date <= vigenciaInicio.Date) ||
                                                    territorios.Any(t => t.tte_vigenciaFim != new DateTime() && t.tte_vigenciaFim.Date < vigenciaFim.Date)))
                        {
                            if (!territorios.Any(t => t.tte_vigenciaInicio.Date <= vigenciaInicio.Date))
                                inicioFimTxt = vigenciaInicio.ToString("dd/MM/yyyy") + " - " +
                                               territorios.Where(t => t.tte_vigenciaInicio.Date > vigenciaInicio.Date)
                                                          .OrderBy(t => t.tte_vigenciaInicio).First()
                                                          .tte_vigenciaInicio.ToString("dd/MM/yyyy");

                            if (territorios.Any(t => t.tte_vigenciaFim != new DateTime() && t.tte_vigenciaFim.Date < vigenciaFim.Date))
                                foreach (TUR_TurmaDisciplinaTerritorio tte in territorios.Where(t => t.tte_vigenciaFim != new DateTime() && t.tte_vigenciaFim.Date < vigenciaFim.Date)
                                                                                         .OrderBy(t => t.tte_vigenciaInicio))
                                {
                                    inicioFimTxt += string.IsNullOrEmpty(inicioFimTxt) ? "" : " e ";
                                    if (tte.tte_vigenciaFim.Date < vigenciaFim.Date)
                                        inicioFimTxt += tte.tte_vigenciaFim.AddDays(1).ToString("dd/MM/yyyy") + " - " +
                                                        (territorios.Any(t => t.tte_vigenciaInicio > tte.tte_vigenciaFim.Date) ?
                                                         territorios.Where(t => t.tte_vigenciaInicio > tte.tte_vigenciaFim.Date)
                                                                    .OrderBy(t => t.tte_vigenciaInicio).First()
                                                                    .tte_vigenciaInicio.AddDays(-1).ToString("dd/MM/yyyy") :
                                                         vigenciaFim.ToString("dd/MM/yyyy"));
                                }

                        }
                        else
                            inicioFimTxt = vigenciaInicio.ToString("dd/MM/yyyy") + " - " + vigenciaFim.ToString("dd/MM/yyyy");

                        // Entra aqui somente qdo não há vigência na última data com aula para gerar
                        if (semVigencia.ContainsKey(tud_id))
                        {
                            semVigencia[tud_id] = String.Format("{0}, {1}", semVigencia[tud_id], inicioFimTxt);
                        }
                        else
                        {
                            semVigencia.Add(tud_id, String.Format("{0}: {1}", dicTurmasDisciplinas[tud_id], inicioFimTxt));
                        }
                    }

                    #endregion

                    bool gravouTodas = gerouTodasAulas;
                    tagsByTud.ForEach(tag => gravouTodas &= Save(tag, banco));
                    gerouTodasAulas &= gravouTodas;

                    CLS_TurmaAulaBO.AtualizarSequenciaAulasPorTurmaDisciplina(tud_id, banco);

                    // Caso o fechamento seja automático, grava na fila de processamento.
                    if (tagsByTud.First().fav_fechamentoAutomatico && tagsByTud.First().tud_tipo != (byte)TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia && tpc_id != ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_PERIODO_CALENDARIO_RECESSO, ent_id))
                    {
                        CLS_AlunoFechamentoPendenciaBO.SalvarFilaFrequencia(tud_id, tpc_id, banco);
                    }
                }
                catch (Exception ex)
                {
                    outrosErros.Add(tud_id, ex);
                    banco.Close(ex);
                }
                finally
                {
                    if (banco.ConnectionIsOpen)
                        banco.Close();
                }
            }

            return true;
        }
    }
}