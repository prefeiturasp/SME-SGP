using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.CustomResourceProviders;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.Turma.Turma
{
    public partial class QuadroHorarios : MotherPageLogado
    {
        #region Propriedades

        private string VS_PaginaVoltar
        {
            get { return ViewState["VS_PaginaVoltar"] as string; }
            set { ViewState["VS_PaginaVoltar"] = value; }
        }

        /// <summary>
        /// Id da turma
        /// </summary>
        private long VS_tur_id
        {
            get
            {
                return Convert.ToInt64(ViewState["VS_tur_id"] ?? -1);
            }

            set
            {
                ViewState["VS_tur_id"] = value;
            }
        }

        /// <summary>
        /// Id da turma
        /// </summary>
        private bool VS_turmaMultisseriada
        {
            get
            {
                return Convert.ToBoolean(ViewState["VS_turmaMultisseriada"] ?? false);
            }

            set
            {
                ViewState["VS_turmaMultisseriada"] = value;
            }
        }

        /// <summary>
        /// Entidade do turno
        /// </summary>
        public ACA_Turno VS_turno
        {
            get
            {
                return (ViewState["VS_turno"] ?? (ViewState["VS_turno"] = new ACA_Turno())) as ACA_Turno;
            }

            set
            {
                ViewState["VS_turno"] = value;
            }
        }

        /// <summary>
        /// Lista de turma horários
        /// </summary>
        public List<TUR_TurmaHorario> VS_lstTurmaHorario
        {
            get
            {
                return (ViewState["VS_lstTurmaHorario"] ?? (ViewState["VS_lstTurmaHorario"] = new List<TUR_TurmaHorario>())) as List<TUR_TurmaHorario>;
            }
            set
            {
                ViewState["VS_lstTurmaHorario"] = value;
            }
        }

        #endregion Propriedades

        #region Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ScriptManager sm = ScriptManager.GetCurrent(this);
                if (sm != null)
                {
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                    sm.Scripts.Add(new ScriptReference("~/Includes/jsQuadroHorarios.js"));
                }

                if (!IsPostBack)
                {
                    if (PreviousPage != null && PreviousPage.IsCrossPagePostBack)
                    {
                        VS_PaginaVoltar = Request.UrlReferrer.ToString();

                        if (PreviousPage is Academico_Turma_Busca)
                        {
                            var previousPage = (Academico_Turma_Busca)PreviousPage;
                            VS_tur_id = previousPage.Edit_tur_id;
                            VS_turno = new ACA_Turno { trn_id = previousPage.Edit_trn_id };
                        }

                        if (VS_tur_id > 0)
                        {
                            ACA_TurnoBO.GetEntity(VS_turno);

                            CarregarQuadro();

                            // Se exister algum registro externo, não permite a edição via sistema
                            if (VS_lstTurmaHorario.Exists(p => p.thr_registroExterno) || VS_turmaMultisseriada)
                            {
                                btnCancelar.Visible = false;
                                btnCancelarAtribuicao.Visible = false;
                                UCCTurmaDisciplina.PermiteEditar = false;

                                btnVoltar.Visible = btnFecharAtribuicao.Visible = true;
                            }
                            else
                            {
                                btnCancelar.Visible = btnCancelarAtribuicao.Visible = UCCTurmaDisciplina.PermiteEditar = false;
                                btnVoltar.Visible = btnFecharAtribuicao.Visible = true;
                            }
                        }
                        else
                        {
                            RedirecionarPagina(VS_PaginaVoltar);
                        }
                    }
                    else
                    {
                        RedirecionarPagina("~/Turma/Turma/Busca.aspx");
                    }

                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage(CustomResource.GetGlobalResourceObject("Turma", "Turma.QuadroHorarios.ErroCarregar"), UtilBO.TipoMensagem.Erro);
                updMensagem.Update();
            }
        }

        #endregion Page Life Cycle

        #region Métodos

        /// <summary>
        /// Carrega o quadro de horários
        /// </summary>
        private void CarregarQuadro()
        {
            // Recupera os dados da turma
            DataTable dtTurma = TUR_TurmaBO.SelectBY_tur_id(VS_tur_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            string turma = dtTurma.Rows[0]["tur_codigo"].ToString();
            string escola = dtTurma.Rows[0]["tur_escolaUnidade"].ToString();
            string calendario = dtTurma.Rows[0]["tur_calendario"].ToString();
            string curso = dtTurma.Rows[0]["tur_curso"].ToString();
            string turno = dtTurma.Rows[0]["tur_turno"].ToString();

            Type objType = typeof(eDiasSemana);
            FieldInfo[] propriedades = objType.GetFields();
            foreach (FieldInfo objField in propriedades)
            {
                DescriptionAttribute[] attributes = (DescriptionAttribute[])objField.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes.Length > 0)
                {
                    hdnDiasSemana.Value += CustomResource.GetGlobalResourceObject("Enumerador", attributes[0].Description) + ";";
                }
            }

            UCComboTipoHorario.Carregar();

            UCCTurmaDisciplina.CarregarTurmaDisciplina(VS_tur_id, true);
            UCCTurmaDisciplina.PermiteEditar = false;
            
            VS_lstTurmaHorario = TUR_TurmaHorarioBO.SelecionaPorTurma(VS_tur_id);

            CarregarCalendario();

            lblDados.Text = string.Format(CustomResource.GetGlobalResourceObject("Turma", "Turma.QuadroHorarios.lblDados.Text"), turma, escola, calendario, curso, turno);

        }

        /// <summary>
        /// Carrega as classes dos tipos de eventos no calendário.
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        protected string getClassNameTipoHorario(ACA_TurnoHorarioTipo tipo)
        {
            switch (tipo)
            {
                case ACA_TurnoHorarioTipo.AulaNormal:
                    return "eventAulaNornal";

                case ACA_TurnoHorarioTipo.AulaForaPeriodo:
                    return "eventAulaForaPeriodo";

                case ACA_TurnoHorarioTipo.IntervaloEntreAulas:
                    return "eventIntervaloEntreAulas";

                case ACA_TurnoHorarioTipo.IntervaloEntrePeriodos:
                    return "eventIntervaloEntrePeriodos";

                default:
                    return "eventAulaNornal";
            }
        }

        /// <summary>
        /// Carrega o controle de calendário.
        /// </summary>
        /// <param name="inicializar"></param>
        protected void CarregarCalendario(bool inicializar = true)
        {
            List<CalendarioBO.EventTime> lstEventos = new List<CalendarioBO.EventTime>();

            Func<byte, string> disciplinaNaoInformada = delegate (byte trh_tipo)
            {
                return trh_tipo == (byte)ACA_TurnoHorarioTipo.AulaNormal ||
                       trh_tipo == (byte)ACA_TurnoHorarioTipo.AulaForaPeriodo ? "<span class=\"sem-info\">" + CustomResource.GetGlobalResourceObject("Turma", "Turma.QuadroHorarios.DisciplinaNaoInformada") + "</span>" : string.Empty;
            };

            lstEventos = VS_lstTurmaHorario.Select(p =>
                new CalendarioBO.EventTime
                {
                    evento = new CalendarioBO.Event
                    {
                        id =
                            p.trn_id.ToString() + ";" +
                            p.trh_id.ToString() + ";" +
                            p.trh_tipo.ToString() + ";" +
                            p.trh_diaSemana.ToString() + ";" +
                            (p.tud_id > 0 ? p.tud_id : -1).ToString() + ";" +
                            ((p.trh_tipo == (byte)ACA_TurnoHorarioTipo.AulaNormal ||
                              p.trh_tipo == (byte)ACA_TurnoHorarioTipo.AulaForaPeriodo) ? "1" : "0")
                    ,
                        title = "<span class=\"quadro-title\">" + (p.tud_id > 0 && !string.IsNullOrEmpty(p.tud_nome) ? p.tud_nome + "</span>" : disciplinaNaoInformada(p.trh_tipo)) +
                                (p.trh_tipo == (byte)ACA_TurnoHorarioTipo.IntervaloEntreAulas || p.trh_tipo == (byte)ACA_TurnoHorarioTipo.IntervaloEntrePeriodos ? "" : "<br/>") + "<span class=\"quadro-tipo\">" + GestaoEscolarUtilBO.GetEnumDescription((ACA_TurnoHorarioTipo)Enum.ToObject(typeof(ACA_TurnoHorarioTipo), p.trh_tipo)) + "</span>"
                    ,
                        allDay = false
                    ,
                        url = ""
                    ,
                        className = getClassNameTipoHorario((ACA_TurnoHorarioTipo)Enum.ToObject(typeof(ACA_TurnoHorarioTipo), p.trh_tipo))
                    ,
                        editable = false
                    ,
                        startEditable = false
                    ,
                        durationEditable = false
                    ,
                        overlap = false
                    ,
                        color = ""
                    ,
                        backgroundColor = ""
                    ,
                        borderColor = ""
                    ,
                        textColor = ""
                    }
                ,
                    diaSemana = (eDiasSemana)Enum.ToObject(typeof(eDiasSemana), p.trh_diaSemana)
                ,
                    inicio = p.trh_horaInicio
                ,
                    fim = p.trh_horaFim
                }
            ).ToList();

            // Encontro os máximos e mínimos para garantir que os horários cadastrados
            // fora do período do turno também sejam exibidos.
            TimeSpan minTime = new TimeSpan(23, 59, 00);
            TimeSpan maxTime = new TimeSpan(00, 00, 00);

            if (VS_turno.trn_horaInicio != new TimeSpan())
            {
                minTime = VS_turno.trn_horaInicio;
            }

            if (VS_turno.trn_horaFim != new TimeSpan())
            {
                maxTime = VS_turno.trn_horaFim;
            }

            if (VS_lstTurmaHorario.Count > 0)
            {
                TimeSpan minTimeHorario = VS_lstTurmaHorario.Min(p => p.trh_horaInicio);
                TimeSpan maxTimeHorario = VS_lstTurmaHorario.Max(p => p.trh_horaFim);

                if (minTimeHorario < minTime)
                {
                    minTime = minTimeHorario;
                }
                if (maxTimeHorario > maxTime)
                {
                    maxTime = maxTimeHorario;
                }
            }

            if (minTime == new TimeSpan(23, 59, 00) || maxTime == new TimeSpan(00, 00, 00))
            {
                if (minTime != new TimeSpan(23, 59, 00))
                {
                    maxTime = minTime.Add(TimeSpan.FromMinutes(10));
                }
                else if (maxTime != new TimeSpan(00, 00, 00))
                {
                    minTime = maxTime.Subtract(TimeSpan.FromMinutes(10));
                }
                else
                {
                    minTime = new TimeSpan(00, 00, 00);
                    maxTime = new TimeSpan(23, 59, 00);
                }
            }

            CalendarioBO.OptionsCalendar option = new CalendarioBO.OptionsCalendar
            {
                header = ""
                ,
                defaultView = CalendarioBO.optionView.agendaWeek
                ,
                allDaySlot = false
                ,
                slotEventOverlap = false
                ,
                selectable = false
                ,
                columnFormat = "dddd"
                ,
                timeFormat = "HH:mm"
                ,
                slotDuration = new TimeSpan(00, 10, 00)
                ,
                minTime = minTime
                ,
                maxTime = maxTime
                ,
                scrollTime = minTime
                ,
                aspectRatio = 1.8F
                ,
                lang = "pt-br"
                ,
                defaultDate = new DateTime(2000, 10, 01)
                ,
                calendarClass = "calendar"
                ,
                onClickEvent = "clickHorario"
            };

            string strMinTime = string.Format("{0},{1}", minTime.Hours, minTime.Minutes);
            string strMaxTime = string.Format("{0},{1}", maxTime.Hours, maxTime.Minutes);
            if (string.IsNullOrEmpty(hdnMinTime.Value) || strMinTime != hdnMinTime.Value
                || string.IsNullOrEmpty(hdnMaxTime.Value) || strMaxTime != hdnMaxTime.Value)
            {
                inicializar = true;
                hdnMinTime.Value = strMinTime;
                hdnMaxTime.Value = strMaxTime;
            }

            int slotMin = -1;
            if (VS_turno.trn_horaInicio != new TimeSpan())
            {
                slotMin = (int)Math.Ceiling((double)((VS_turno.trn_horaInicio.Hours * 60 + VS_turno.trn_horaInicio.Minutes) - (minTime.Hours * 60 + minTime.Minutes)) / 10) + 1;
            }

            int slotMax = -1;
            if (VS_turno.trn_horaFim != new TimeSpan())
            {
                slotMax = (int)Math.Ceiling((double)((VS_turno.trn_horaFim.Hours * 60 + VS_turno.trn_horaFim.Minutes) - (minTime.Hours * 60 + minTime.Minutes)) / 10);
            }

            UCCalendario.CarregarCalendarioSemanal(lstEventos, option, inicializar, slotMin, slotMax);
        }
        
        #endregion Métodos

        #region Eventos
        
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            RedirecionarPagina(VS_PaginaVoltar);
        }

        #endregion Eventos
    }
}