using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
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

namespace GestaoEscolar.Academico.ControleSemanal
{
    public partial class Cadastro : MotherPageLogado
    {
        #region Propriedades

        /// <summary>
        /// Retorna o valor do parâmetro "Permanecer na tela após gravações"
        /// </summary>
        private bool ParametroPermanecerTela
        {
            get
            {
                return ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.BOTAO_SALVAR_PERMANECE_TELA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            }
        }

        /// <summary>
        /// ViewState que armazena os calendarios do período(mesmos itens do repeater mostrado em tela).
        /// </summary>
        private List<Struct_CalendarioPeriodos> VS_CalendarioPeriodo
        {
            get
            {
                return (List<Struct_CalendarioPeriodos>)(ViewState["VS_CalendarioPeriodo"] ?? ((ViewState["VS_CalendarioPeriodo"] = new List<Struct_CalendarioPeriodos>())));
            }

            set
            {
                ViewState["VS_CalendarioPeriodo"] = value;
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de tur_id
        /// </summary>
        public long VS_tur_id
        {
            get
            {
                if (ViewState["VS_tur_id"] != null)
                    return Convert.ToInt64(ViewState["VS_tur_id"]);
                return -1;
            }
            set
            {
                ViewState["VS_tur_id"] = value;
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de tud_id
        /// </summary>
        public long VS_tud_id
        {
            get
            {
                if (ViewState["VS_tud_id"] != null)
                    return Convert.ToInt64(ViewState["VS_tud_id"]);
                return -1;
            }
            set
            {
                ViewState["VS_tud_id"] = value;
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de tdt_posicao
        /// </summary>
        public byte VS_tdt_posicao
        {
            get
            {
                if (ViewState["VS_tdt_posicao"] != null)
                    return Convert.ToByte(ViewState["VS_tdt_posicao"]);
                return 1;
            }
            set
            {
                ViewState["VS_tdt_posicao"] = value;
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de cal_id
        /// </summary>
        public int VS_cal_id
        {
            get
            {
                if (ViewState["VS_cal_id"] != null)
                    return Convert.ToInt32(ViewState["VS_cal_id"]);
                return -1;
            }
            set
            {
                ViewState["VS_cal_id"] = value;
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de VS_cal_ano
        /// </summary>
        public int VS_cal_ano
        {
            get
            {
                if (ViewState["VS_cal_ano"] != null)
                    return Convert.ToInt32(ViewState["VS_cal_ano"]);
                return -1;
            }
            set
            {
                ViewState["VS_cal_ano"] = value;
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de tne_id
        /// </summary>
        public int VS_tne_id
        {
            get
            {
                if (ViewState["VS_tne_id"] != null)
                    return Convert.ToInt32(ViewState["VS_tne_id"]);
                return -1;
            }
            set
            {
                ViewState["VS_tne_id"] = value;
            }
        }

        /// <summary>
        /// Armazena o ID do tipo do período do calendário em viewstate.
        /// </summary>
        public int VS_tpc_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_tpc_id"] ?? -1);
            }

            set
            {
                ViewState["VS_tpc_id"] = value;
            }
        }

        /// <summary>
        /// Armazena a ordem do tipo do período do calendário em viewstate.
        /// </summary>
        public int VS_tpc_ordem
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_tpc_ordem"] ?? -1);
            }

            set
            {
                ViewState["VS_tpc_ordem"] = value;
            }
        }
        
        /// <summary>
        /// Lista de permissões do docente para cadastro de plano de aula.
        /// </summary>
        private List<sPermissaoDocente> VS_ltPermissaoPlanoAula
        {
            get
            {
                return (List<sPermissaoDocente>)
                        (
                            ViewState["VS_ltPermissaoPlanoAula"] ??
                            (
                                ViewState["VS_ltPermissaoPlanoAula"] = CFG_PermissaoDocenteBO.SelecionaPermissaoModulo(VS_tdt_posicao, (byte)EnumModuloPermissao.PlanoAula)
                            )
                        );
            }
        }

        private List<sComboTurmaDisciplina> dtTurmaDisciplinaDoc
        {
            get
            {
                if (dtTurmaDisciplinaAux == null)
                    dtTurmaDisciplinaAux = TUR_TurmaDisciplinaBO.SelecionaDisciplinaPorTurmaDocente_SemVigencia(0, __SessionWEB.__UsuarioWEB.Docente.doc_id, 0, 0, false, ApplicationWEB.AppMinutosCacheLongo);
                return dtTurmaDisciplinaAux;
            }
        }

        private List<sComboTurmaDisciplina> dtTurmaDisciplinaTud
        {
            get
            {
                if (dtTurmaDisciplinaAux == null)
                    dtTurmaDisciplinaAux = TUR_TurmaDisciplinaBO.SelecionaDisciplinaPorTurmaDocente_SemVigencia(VS_tur_id, 0, 0, 0, false, ApplicationWEB.AppMinutosCacheLongo);
                return dtTurmaDisciplinaAux;
            }
        }

        private List<CLS_TurmaAulaPlanoDisciplina> dtTurmaAulaPlanoDisc
        {
            get
            {
                if (dtTurmaAulaPlanoDiscAux == null)
                    dtTurmaAulaPlanoDiscAux = CLS_TurmaAulaPlanoDisciplinaBO.SelectBy_tud_id(VS_tud_id);
                return dtTurmaAulaPlanoDiscAux;
            }
        }

        private List<sComboTurmaDisciplina> dtTurmaDisciplinaAux;

        private List<CLS_TurmaAulaPlanoDisciplina> dtTurmaAulaPlanoDiscAux;

        private DataTable planosPermissaoDocente;

        #endregion Propriedades

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    string message = __SessionWEB.PostMessages;
                    if (!String.IsNullOrEmpty(message))
                        _lblMessage.Text = message;

                    if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                    {
                        CarregarDados(PreviousPage.Edit_esc_id, PreviousPage.Edit_escola, PreviousPage.Edit_tur_id, PreviousPage.Edit_tur_codigo,
                                      PreviousPage.Edit_cal_id, PreviousPage.Edit_tud_id, PreviousPage.Edit_tud_nome, PreviousPage.Edit_tdt_posicao);
                    }
                    else if (Session["DadosPaginaRetorno"] != null)
                    {
                        Dictionary<string, string> listaDados = (Dictionary<string, string>)Session["DadosPaginaRetorno"];

                        CarregarDados(Convert.ToInt32(listaDados["Edit_esc_id"]), listaDados["Edit_escola"],
                                      Convert.ToInt64(listaDados["Edit_tur_id"]), listaDados["Edit_tur_codigo"],
                                      Convert.ToInt32(listaDados["Edit_cal_id"]), Convert.ToInt64(listaDados["Edit_tud_id"]),
                                      listaDados["Edit_tud_nome"], Convert.ToByte(listaDados["Edit_tdt_posicao"]));

                        // Remove os dados que possam estar na sessao
                        Session.Remove("DadosPaginaRetorno");
                        Session.Remove("VS_DadosTurmas");
                        Session.Remove("Historico");
                    }
                    else
                    {
                        __SessionWEB.PostMessages = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ControleSemanal.Cadastro.MensagemSelecione").ToString(), UtilBO.TipoMensagem.Alerta);
                        Response.Redirect("~/Academico/ControleSemanal/Busca.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
            }
            catch (ValidationException ex)
            {
                _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ControleSemanal.Cadastro.ErroCarregar").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnPeriodo_Click(object sender, EventArgs e)
        {
            try
            {
                Salvar(true);

                Button btnPeriodo = (Button)sender;
                RepeaterItem itemPeriodo = (RepeaterItem)btnPeriodo.NamingContainer;
                Repeater rptPeriodo = (Repeater)itemPeriodo.NamingContainer;
                HiddenField hdnPeriodo = (HiddenField)itemPeriodo.FindControl("hdnPeriodo");
                HiddenField hdnPeriodoOrdem = (HiddenField)itemPeriodo.FindControl("hdnPeriodoOrdem");

                VS_tpc_id = Convert.ToInt32(hdnPeriodo.Value);
                VS_tpc_ordem = Convert.ToInt32(hdnPeriodoOrdem.Value);
                lblInicio.Text = lblFim.Text = "";
                lkbAnterior.Visible = lkbProximo.Visible = false;

                if (VS_CalendarioPeriodo.Any(p => p.tpc_id == VS_tpc_id))
                {
                    DateTime dataInicio = VS_CalendarioPeriodo.Where(p => p.tpc_id == VS_tpc_id).First().cap_dataInicio;
                    lblInicio.Text = dataInicio.ToShortDateString();
                    if (VS_CalendarioPeriodo.Any(p => p.tpc_id == VS_tpc_id && p.cap_dataFim > dataInicio.AddDays(6 - ((int)dataInicio.DayOfWeek))))
                        lblFim.Text = dataInicio.AddDays(6 - ((int)dataInicio.DayOfWeek)).ToShortDateString();
                    else
                        lblFim.Text = VS_CalendarioPeriodo.Where(p => p.tpc_id == VS_tpc_id).First().cap_dataFim.ToShortDateString();
                    lkbProximo.Visible = VS_CalendarioPeriodo.Any(p => p.tpc_id == VS_tpc_id && p.cap_dataFim > dataInicio.AddDays(6 + ((int)dataInicio.DayOfWeek)));

                    if (VS_CalendarioPeriodo.Any(p => p.tpc_id == VS_tpc_id && (p.cap_dataFim <= DateTime.Today || p.cap_dataInicio <= DateTime.Today)) &&
                        Convert.ToDateTime(lblFim.Text) < DateTime.Today)
                    {
                        bool desiste = false;
                        while (Convert.ToDateTime(lblFim.Text) < DateTime.Today && !desiste)
                        {
                            DateTime dataFim = Convert.ToDateTime(lblFim.Text);
                            if (VS_CalendarioPeriodo.Any(p => p.tpc_id == VS_tpc_id) &&
                                VS_CalendarioPeriodo.Any(p => p.tpc_id == VS_tpc_id && p.cap_dataFim >= dataFim.AddDays(2)))
                            {
                                dataInicio = dataFim.AddDays(2);
                                lblInicio.Text = dataInicio.ToShortDateString();
                                if (VS_CalendarioPeriodo.Any(p => p.tpc_id == VS_tpc_id && p.cap_dataFim > dataInicio.AddDays(6 - ((int)dataInicio.DayOfWeek))))
                                    lblFim.Text = dataInicio.AddDays(6 - ((int)dataInicio.DayOfWeek)).ToShortDateString();
                                else
                                {
                                    lblFim.Text = VS_CalendarioPeriodo.Where(p => p.tpc_id == VS_tpc_id).First().cap_dataFim.ToShortDateString();
                                    desiste = true;
                                }
                                lkbProximo.Visible = VS_CalendarioPeriodo.Any(p => p.tpc_id == VS_tpc_id && p.cap_dataFim > dataInicio.AddDays(6 + ((int)dataInicio.DayOfWeek)));
                                lkbAnterior.Visible = true;
                            }
                            else
                                desiste = true;
                        }
                    }
                }

                rptPeriodo.Items.Cast<RepeaterItem>().ToList()
                            .Select(p => (Button)p.FindControl("btnPeriodo"))
                            .ToList().ForEach(p => RemoveClass(p, "periodo_selecionado"));

                AddClass(btnPeriodo, "periodo_selecionado");

                CarregarAulasData();
            }
            catch (ValidationException ex)
            {
                _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ControleSemanal.Cadastro.ErroCarregar").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void rptDiasSemana_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                Image imgEventoSemAtividade = (Image)e.Item.FindControl("imgEventoSemAtividade");
                Label lblDataAula = (Label)e.Item.FindControl("lblDataAula");

                if (lblDataAula != null && imgEventoSemAtividade != null)
                {
                    DateTime data = Convert.ToDateTime(lblDataAula.Text);

                    imgEventoSemAtividade.ToolTip = GetGlobalResourceObject("Academico", "ControleTurma.DiarioClasse.MensagemEventoSemAtivDiscente").ToString();
                    imgEventoSemAtividade.Visible = planosPermissaoDocente.AsEnumerable().Any() && planosPermissaoDocente.AsEnumerable().Any(p => Convert.ToDateTime(p["data"]) == data && Convert.ToBoolean(p["EventoSemAtividade"]));

                    if (imgEventoSemAtividade.Visible)
                        divEventoSemAtividade.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ControleSemanal.Cadastro.ErroCarregar").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void rptPeriodo_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem ||
                e.Item.ItemType == ListItemType.Item)
            {
                Button btnPeriodo = (Button)e.Item.FindControl("btnPeriodo");
                if (btnPeriodo != null)
                {
                    if (!Convert.ToString(btnPeriodo.CssClass).Contains("btnMensagemUnload"))
                    {
                        btnPeriodo.CssClass += " btnMensagemUnload";
                    }
                }
            }
        }

        protected void lkbAnterior_Click(object sender, EventArgs e)
        {
            try
            {
                Salvar(true);

                DateTime dataInicio = Convert.ToDateTime(lblInicio.Text);
                if (VS_CalendarioPeriodo.Any(p => p.tpc_id == VS_tpc_id) &&
                    VS_CalendarioPeriodo.Any(p => p.tpc_id == VS_tpc_id && p.cap_dataInicio <= dataInicio.AddDays(-2)))
                {
                    DateTime dataFim = dataInicio.AddDays(-2);
                    lblFim.Text = dataFim.ToShortDateString();
                    if (VS_CalendarioPeriodo.Any(p => p.tpc_id == VS_tpc_id && p.cap_dataInicio <= dataFim.AddDays(-5)))
                        lblInicio.Text = dataFim.AddDays(-5).ToShortDateString();
                    else
                        lblInicio.Text = VS_CalendarioPeriodo.Where(p => p.tpc_id == VS_tpc_id).First().cap_dataInicio.ToShortDateString();
                    lkbAnterior.Visible = VS_CalendarioPeriodo.Any(p => p.tpc_id == VS_tpc_id && p.cap_dataInicio <= Convert.ToDateTime(lblInicio.Text).AddDays(-2));
                    lkbProximo.Visible = true;
                }

                CarregarAulasData();
            }
            catch (ValidationException ex)
            {
                _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ControleSemanal.Cadastro.ErroCarregarDatas").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void lkbProximo_Click(object sender, EventArgs e)
        {
            try
            {
                Salvar(true);

                DateTime dataFim = Convert.ToDateTime(lblFim.Text);
                if (VS_CalendarioPeriodo.Any(p => p.tpc_id == VS_tpc_id) &&
                    VS_CalendarioPeriodo.Any(p => p.tpc_id == VS_tpc_id && p.cap_dataFim >= dataFim.AddDays(2)))
                {
                    DateTime dataInicio = dataFim.AddDays(2);
                    lblInicio.Text = dataInicio.ToShortDateString();
                    if (VS_CalendarioPeriodo.Any(p => p.tpc_id == VS_tpc_id && p.cap_dataFim > dataInicio.AddDays(6 - ((int)dataInicio.DayOfWeek))))
                        lblFim.Text = dataInicio.AddDays(6 - ((int)dataInicio.DayOfWeek)).ToShortDateString();
                    else
                        lblFim.Text = VS_CalendarioPeriodo.Where(p => p.tpc_id == VS_tpc_id).First().cap_dataFim.ToShortDateString();
                    lkbProximo.Visible = VS_CalendarioPeriodo.Any(p => p.tpc_id == VS_tpc_id && p.cap_dataFim > dataInicio.AddDays(6 + ((int)dataInicio.DayOfWeek)));
                    lkbAnterior.Visible = true;
                }

                CarregarAulasData();
            }
            catch (ValidationException ex)
            {
                _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ControleSemanal.Cadastro.ErroCarregarDatas").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Remove uma classe css ao um controle da página.
        /// Habilita o controle também.
        /// </summary>
        /// <param name="control">Controle da página</param>
        /// <param name="cssClass">classe css</param>
        private void RemoveClass(WebControl control, string cssClass)
        {
            control.Enabled = true;
            List<string> classes = control.CssClass.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            classes.Remove(cssClass);

            control.CssClass = string.Join(" ", classes.ToArray());
        }

        /// <summary>
        /// Adiciona uma classe css ao um controle da página.
        /// Desabilita o controle também.
        /// </summary>
        /// <param name="control">Controle da página</param>
        /// <param name="cssClass">classe css</param>
        private void AddClass(WebControl control, string cssClass)
        {
            control.Enabled = false;

            List<string> classes = control.CssClass.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            classes.Add(cssClass);

            control.CssClass = string.Join(" ", classes.ToArray());
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                Salvar(false);
            }
            catch (ValidationException ex)
            {
                _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ControleSemanal.Cadastro.ErroSalvar").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Academico/ControleSemanal/Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void rptAulas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) ||
                (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                int tau_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tau_id"));
                TextBox txtPlanoAula = (TextBox)e.Item.FindControl("txtPlanoAula");

                bool permissaoAlteracao = __SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.Gestao
                                            && __SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.UnidadeAdministrativa
                                            && Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "permissaoAlteracao")) > 0
                                            && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;

                txtPlanoAula.Enabled = permissaoAlteracao;
                txtPlanoAula.Visible = tau_id > 0;

                DateTime dataAula = Convert.ToDateTime(DataBinder.Eval(e.Item.DataItem, "data"));

                // Apenas aulas dos dias anteriores sem plano de aula devem exibir o aviso.
                Image imgSemPlanoAula = (Image)e.Item.FindControl("imgSemPlanoAula");
                HiddenField hdfSemPlanoAula = (HiddenField)e.Item.FindControl("hdfSemPlanoAula");
                if (tau_id > 0 && imgSemPlanoAula != null && hdfSemPlanoAula != null && dataAula.Date < DateTime.Now.Date)
                {
                    imgSemPlanoAula.Visible = Convert.ToBoolean(hdfSemPlanoAula.Value)
                                                && (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual
                                                    || VS_tne_id != ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_NIVEL_ENSINO_EDUCACAO_INFANTIL, __SessionWEB.__UsuarioWEB.Usuario.ent_id)
                                                    || ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_ALERTA_AULA_SEM_PLANO_ENSINO_INFANTIL, __SessionWEB.__UsuarioWEB.Usuario.ent_id));
                    imgSemPlanoAula.ToolTip = GetGlobalResourceObject("Academico", "ControleTurma.DiarioClasse.imgSemPlanoAula").ToString();
                }

                CheckBoxList chlComponenteCurricular = (CheckBoxList)e.Item.FindControl("chlComponenteCurricular");
                chlComponenteCurricular.Visible = tau_id > 0;
                chlComponenteCurricular.Enabled = permissaoAlteracao;

                if (tau_id > 0)
                {
                    if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                    {
                        var turmaDisciplina = (from dr in dtTurmaDisciplinaDoc
                                               where Convert.ToInt64(dr.tur_tud_id.Split(';')[0]) == VS_tur_id
                                               && Convert.ToByte(dr.tur_tud_id.Split(';')[3]) == Convert.ToByte(ACA_CurriculoDisciplinaTipo.ComponenteRegencia)
                                               select dr);
                        chlComponenteCurricular.DataSource = turmaDisciplina;
                        chlComponenteCurricular.DataBind();

                        foreach (ListItem item in chlComponenteCurricular.Items)
                            item.Selected = dtTurmaAulaPlanoDisc.Any(p => p.tud_idPlano == Convert.ToInt64(item.Value.Split(';')[1]) &&
                                                                          p.tau_id == Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tau_id")));
                    }
                    else
                    {
                        var turmaDisciplina = (from dr in dtTurmaDisciplinaTud
                                               where Convert.ToByte(dr.tur_tud_id.Split(';')[3]) != Convert.ToByte(ACA_CurriculoDisciplinaTipo.ComponenteRegencia)
                                               select dr);

                        //Carrega componentes da regência.
                        var turmaDisciplinaComponenteRegencia = (from dr in dtTurmaDisciplinaTud
                                                                 where Convert.ToByte(dr.tur_tud_id.Split(';')[3]) == Convert.ToByte(ACA_CurriculoDisciplinaTipo.ComponenteRegencia)
                                                                 select dr);
                        chlComponenteCurricular.DataSource = turmaDisciplinaComponenteRegencia;
                        chlComponenteCurricular.DataBind();

                        foreach (ListItem item in chlComponenteCurricular.Items)
                            item.Selected = dtTurmaAulaPlanoDisc.Any(p => p.tud_idPlano == Convert.ToInt64(item.Value.Split(';')[1]) &&
                                                                            p.tau_id == Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tau_id")));
                    }
                }
            }
        }

        #endregion Eventos

        #region Métodos

        /// <summary>
        /// Carrega os dados da turma disciplina na tela
        /// </summary>
        public void CarregarDados(int esc_id, string escola, long tur_id, string tur_codigo, int cal_id, long tud_id, string tud_nome, byte tdt_posicao)
        {
            ACA_CalendarioAnual cal = new ACA_CalendarioAnual { cal_id = cal_id };
            ACA_CalendarioAnualBO.GetEntity(cal);

            TUR_TurmaDisciplina tud = new TUR_TurmaDisciplina { tud_id = tud_id };
            TUR_TurmaDisciplinaBO.GetEntity(tud);

            Enum posicao = ACA_TipoDocenteBO.SelecionaTipoDocentePorPosicao(tdt_posicao, ApplicationWEB.AppMinutosCacheLongo);
            FieldInfo infoElemento = posicao.GetType().GetField(posicao.ToString());
            DescriptionAttribute[] atributos = (DescriptionAttribute[])infoElemento.GetCustomAttributes(typeof(DescriptionAttribute), false);
            string posicaoText = "";

            if (atributos.Length > 0)
            {
                if (atributos[0].Description != null)
                    posicaoText = atributos[0].Description;
                else
                    posicaoText = "Titular";
            }

            ACA_Curso cur = new ACA_Curso { cur_id = TUR_TurmaCurriculoBO.GetSelectBy_Turma(tur_id, ApplicationWEB.AppMinutosCacheLongo).First().cur_id };
            ACA_CursoBO.GetEntity(cur);

            lblCabecalho.Text = string.Format(GetGlobalResourceObject("Academico", "ControleSemanal.Cadastro.lblCabecalho.Text").ToString(), 
                                              escola, cal.cal_descricao, tur_codigo, tud_nome, posicaoText);
            
            VS_cal_id = cal_id;
            VS_cal_ano = cal.cal_ano;
            VS_tdt_posicao = tdt_posicao;
            VS_tud_id = tud_id;
            VS_tur_id = tur_id;
            VS_tne_id = cur.tne_id;

            CarregarPeriodos(esc_id, tud.tud_tipo, tdt_posicao, tur_id, tud_id);

            bool permiteEditar = VS_ltPermissaoPlanoAula.Any(p => p.tdt_posicaoPermissao == tdt_posicao && p.pdc_permissaoEdicao);
            btnSalvar.Visible = permiteEditar;
            btnCancelar.Text = permiteEditar ? GetGlobalResourceObject("Academico", "ControleSemanal.Cadastro.btnCancelar.Text").ToString() :
                               GetGlobalResourceObject("Academico", "ControleSemanal.Cadastro.btnVoltar.Text").ToString();
        }

        /// <summary>
        /// Carregar os períodos e seta a visibilidade dos botões de acordo com a permissão do usuário.
        /// </summary>
        public void CarregarPeriodos
        (
            int esc_id
            , byte tud_tipo
            , byte tdt_posicao
            , Int64 tur_id
            , Int64 tud_id
        )
        {
            try
            {
                if (tur_id > 0 && tud_id > 0 && ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PRE_CARREGAR_CACHE_EFETIVACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                {
                    TUR_Turma entityTurma = new TUR_Turma { tur_id = tur_id };
                    TUR_TurmaBO.GetEntity(entityTurma);
                    
                    hdnTudId.Value = tud_id.ToString();
                    hdnTurId.Value = entityTurma.tur_id.ToString();
                    hdnTurTipo.Value = entityTurma.tur_tipo.ToString();
                    hdnCalId.Value = entityTurma.cal_id.ToString();
                    hdnTudTipo.Value = tud_tipo.ToString();
                    hdnTipoDocente.Value = (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0 ?
                                (byte)ACA_TipoDocenteBO.SelecionaTipoDocentePorPosicao(tdt_posicao, ApplicationWEB.AppMinutosCacheLongo) : (byte)0).ToString();

                }
                
                List<Struct_CalendarioPeriodos> lstCalendarioPeriodos = ACA_CalendarioPeriodoBO.SelecionaPor_Calendario(VS_cal_id, ApplicationWEB.AppMinutosCacheLongo, false, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                VS_CalendarioPeriodo = lstCalendarioPeriodos;

                List<ESC_EscolaCalendarioPeriodo> lstEscCalPeriodo = ESC_EscolaCalendarioPeriodoBO.SelectEscolasCalendarioCache(VS_cal_id, ApplicationWEB.AppMinutosCacheCurto);

                VS_CalendarioPeriodo = VS_CalendarioPeriodo.Where(calP => (lstEscCalPeriodo.Where(escP => (escP.esc_id == esc_id && escP.tpc_id == calP.tpc_id)).Count() == 0)).ToList();
                
                rptPeriodo.DataSource = VS_CalendarioPeriodo;

                rptPeriodo.DataBind();

                //Seleciona o ultimo bimestre
                List<Struct_CalendarioPeriodos> tabelaPeriodos = ACA_CalendarioPeriodoBO.SelecionaPor_Calendario(VS_cal_id, ApplicationWEB.AppMinutosCacheLongo);
                int tpc_idUltimoPeriodo = tabelaPeriodos.Count > 0 ? tabelaPeriodos.Last().tpc_id : -1;
                int tpc_ordemUltimoPeriodo = tabelaPeriodos.Count > 0 ? tabelaPeriodos.Last().tpc_ordem : 0;

                if (VS_tpc_id <= 0)
                {
                    //Busca o bimestre corrente
                    Struct_CalendarioPeriodos periodo = VS_CalendarioPeriodo.Where(x => (x.cap_dataInicio.Date <= DateTime.Now.Date && x.cap_dataFim.Date >= DateTime.Now.Date)).FirstOrDefault();
                    VS_tpc_id = periodo.tpc_id;
                    VS_tpc_ordem = periodo.tpc_ordem;

                    if (VS_tpc_id <= 0)
                    {
                        //Se não tem bimestre selecionado e nem bimestre corrente então seleciona o próximo corrente
                        periodo = VS_CalendarioPeriodo.Where(x => (x.cap_dataInicio.Date >= DateTime.Now.Date)).FirstOrDefault();
                        VS_tpc_id = periodo.tpc_id;
                        VS_tpc_ordem = periodo.tpc_ordem;

                        if (VS_tpc_id <= 0)
                        {
                            //Se não tem bimestre selecionado então seleciona o ultimo
                            VS_tpc_id = tpc_idUltimoPeriodo;
                            VS_tpc_ordem = tpc_ordemUltimoPeriodo;
                        }
                    }
                }

                lblInicio.Text = lblFim.Text = "";
                lkbAnterior.Visible = lkbProximo.Visible = false;

                if (VS_CalendarioPeriodo.Any(p => p.tpc_id == VS_tpc_id))
                {
                    DateTime dataInicio = VS_CalendarioPeriodo.Where(p => p.tpc_id == VS_tpc_id).First().cap_dataInicio;
                    lblInicio.Text = dataInicio.ToShortDateString();
                    if (VS_CalendarioPeriodo.Any(p => p.tpc_id == VS_tpc_id && p.cap_dataFim > dataInicio.AddDays(6 - ((int)dataInicio.DayOfWeek))))
                        lblFim.Text = dataInicio.AddDays(6 - ((int)dataInicio.DayOfWeek)).ToShortDateString();
                    else
                        lblFim.Text = VS_CalendarioPeriodo.Where(p => p.tpc_id == VS_tpc_id).First().cap_dataFim.ToShortDateString();
                    lkbProximo.Visible = VS_CalendarioPeriodo.Any(p => p.tpc_id == VS_tpc_id && p.cap_dataFim > dataInicio.AddDays(6 + ((int)dataInicio.DayOfWeek)));

                    if (VS_CalendarioPeriodo.Any(p => p.tpc_id == VS_tpc_id && (p.cap_dataFim <= DateTime.Today || p.cap_dataInicio <= DateTime.Today)) &&
                        Convert.ToDateTime(lblFim.Text) < DateTime.Today)
                    {
                        bool desiste = false;
                        while (Convert.ToDateTime(lblFim.Text) < DateTime.Today && !desiste)
                        {
                            DateTime dataFim = Convert.ToDateTime(lblFim.Text);
                            if (VS_CalendarioPeriodo.Any(p => p.tpc_id == VS_tpc_id) &&
                                VS_CalendarioPeriodo.Any(p => p.tpc_id == VS_tpc_id && p.cap_dataFim >= dataFim.AddDays(2)))
                            {
                                dataInicio = dataFim.AddDays(2);
                                lblInicio.Text = dataInicio.ToShortDateString();
                                if (VS_CalendarioPeriodo.Any(p => p.tpc_id == VS_tpc_id && p.cap_dataFim > dataInicio.AddDays(6 - ((int)dataInicio.DayOfWeek))))
                                    lblFim.Text = dataInicio.AddDays(6 - ((int)dataInicio.DayOfWeek)).ToShortDateString();
                                else
                                {
                                    lblFim.Text = VS_CalendarioPeriodo.Where(p => p.tpc_id == VS_tpc_id).First().cap_dataFim.ToShortDateString();
                                    desiste = true;
                                }
                                lkbProximo.Visible = VS_CalendarioPeriodo.Any(p => p.tpc_id == VS_tpc_id && p.cap_dataFim > dataInicio.AddDays(6 + ((int)dataInicio.DayOfWeek)));
                                lkbAnterior.Visible = true;
                            }
                            else
                                desiste = true;
                        }
                    }

                    CarregarAulasData();
                }

                if (VS_tpc_ordem < 0)
                {
                    VS_tpc_ordem = 0;
                }

                // Seleciona o botão do bimestre informado (VS_tpc_id)
                rptPeriodo.Items.Cast<RepeaterItem>().ToList()
                            .Select(p => (Button)p.FindControl("btnPeriodo"))
                            .ToList().ForEach(p => RemoveClass(p, "periodo_selecionado"));
                rptPeriodo.Items.Cast<RepeaterItem>().ToList()
                            .Where(p => Convert.ToInt32(((HiddenField)p.FindControl("hdnPeriodo")).Value) == VS_tpc_id
                                    && Convert.ToInt32(((HiddenField)p.FindControl("hdnPeriodoOrdem")).Value) == VS_tpc_ordem)
                            .Select(p => (Button)p.FindControl("btnPeriodo"))
                            .ToList()
                            .ForEach
                            (
                                p =>
                                {
                                    AddClass(p, "periodo_selecionado");

                                    HiddenField hdn = (HiddenField)p.FindControl("hdnPeriodoOrdem");
                                    if (!string.IsNullOrEmpty(hdn.Value))
                                    {
                                        hdnTpcOrdem.Value = hdn.Value;
                                    }
                                }
                            );
            }
            catch (ValidationException ex)
            {
                _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ControleSemanal.Cadastro.ErroCarregar").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }
        
        /// <summary>
        /// Carrega as aulas conforme o bimestre e período(semana) selecionados
        /// </summary>
        private void CarregarAulasData()
        {
            try
            {
                divEventoSemAtividade.Visible = false;
                divAulas.Visible = false;
                if (string.IsNullOrEmpty(lblInicio.Text) || string.IsNullOrEmpty(lblFim.Text))
                    throw new ValidationException("Data inválida.");

                divAulas.Visible = true;
                lblMessageAulas.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ControleSemanal.Cadastro.NenhumaAulaPeriodo").ToString(), UtilBO.TipoMensagem.Alerta);

                DataTable dtPlanos = CLS_TurmaAulaBO.SelecionaPlanosAulaPor_Disciplina(VS_tud_id, VS_tpc_id, __SessionWEB.__UsuarioWEB.Usuario.usu_id, VS_tdt_posicao, __SessionWEB.__UsuarioWEB.Docente.doc_id == 0, 0);
                if (dtPlanos.AsEnumerable().Any(p => !Convert.ToBoolean(p["tau_reposicao"])))
                    dtPlanos = dtPlanos.AsEnumerable().Where(p => !Convert.ToBoolean(p["tau_reposicao"])).CopyToDataTable();
                else
                    dtPlanos.Rows.Clear();

                planosPermissaoDocente = new DataTable();

                if ((from DataRow dr in dtPlanos.AsEnumerable()
                     where VS_ltPermissaoPlanoAula.Any(p => p.tdt_posicaoPermissao == (byte)dr["tdt_posicao"] && (p.pdc_permissaoConsulta || p.pdc_permissaoEdicao)) &&
                           Convert.ToDateTime(dr["data"]) >= Convert.ToDateTime(lblInicio.Text) &&
                           Convert.ToDateTime(dr["data"]) <= Convert.ToDateTime(lblFim.Text) &&
                           (byte)dr["tdt_posicao"] == VS_tdt_posicao
                     select dr).Any())
                {
                    planosPermissaoDocente = (from DataRow dr in dtPlanos.AsEnumerable()
                                              where VS_ltPermissaoPlanoAula.Any(p => p.tdt_posicaoPermissao == (byte)dr["tdt_posicao"] && (p.pdc_permissaoConsulta || p.pdc_permissaoEdicao)) &&
                                                    Convert.ToDateTime(dr["data"]) >= Convert.ToDateTime(lblInicio.Text) &&
                                                    Convert.ToDateTime(dr["data"]) <= Convert.ToDateTime(lblFim.Text) &&
                                                    (byte)dr["tdt_posicao"] == VS_tdt_posicao
                                              select dr).CopyToDataTable();
                }

                if (planosPermissaoDocente.AsEnumerable().Any())
                    lblMessageAulas.Visible = false;
                else
                    lblMessageAulas.Visible = true;

                Dictionary<int, string> diasSemana = new Dictionary<int, string>();
                DateTime dataInicio = Convert.ToDateTime(lblInicio.Text);
                while (dataInicio <= Convert.ToDateTime(lblFim.Text))
                {
                    diasSemana.Add((int)dataInicio.DayOfWeek, dataInicio.ToShortDateString());
                    dataInicio = dataInicio.AddDays(1);
                }
                for (int i = 1; i <= 6; i++)
                {
                    if (!diasSemana.ContainsKey(i))
                    {
                        DateTime ultimoValor = Convert.ToDateTime(diasSemana.Last().Value);
                        DateTime primeiroValor = Convert.ToDateTime(diasSemana.First().Value);
                        diasSemana.Add(i, (diasSemana.Last().Key < i ? ultimoValor.AddDays(i - diasSemana.Last().Key) :
                                                                        primeiroValor.AddDays(i - diasSemana.First().Key)).ToShortDateString());
                    }
                }

                // seleciona apenas os planos que o docente tem permissao para consultar ou alterar
                rptAulas.DataSource = diasSemana.OrderBy(p => p.Key).Select(d => new
                                                {
                                                    tud_id = planosPermissaoDocente.AsEnumerable().Any() && planosPermissaoDocente.AsEnumerable().Any(p => Convert.ToDateTime(p["data"]) == Convert.ToDateTime(d.Value)) ? planosPermissaoDocente.AsEnumerable().Where(p => Convert.ToDateTime(p["data"]) == Convert.ToDateTime(d.Value)).First()["tud_id"] : "-1",
                                                    tau_id = planosPermissaoDocente.AsEnumerable().Any() && planosPermissaoDocente.AsEnumerable().Any(p => Convert.ToDateTime(p["data"]) == Convert.ToDateTime(d.Value)) ? planosPermissaoDocente.AsEnumerable().Where(p => Convert.ToDateTime(p["data"]) == Convert.ToDateTime(d.Value)).First()["tau_id"] : "-1",
                                                    tud_idFilho = planosPermissaoDocente.AsEnumerable().Any() && planosPermissaoDocente.AsEnumerable().Any(p => Convert.ToDateTime(p["data"]) == Convert.ToDateTime(d.Value)) ? planosPermissaoDocente.AsEnumerable().Where(p => Convert.ToDateTime(p["data"]) == Convert.ToDateTime(d.Value)).First()["tud_idFilho"] : "-1",
                                                    data = planosPermissaoDocente.AsEnumerable().Any() && planosPermissaoDocente.AsEnumerable().Any(p => Convert.ToDateTime(p["data"]) == Convert.ToDateTime(d.Value)) ? planosPermissaoDocente.AsEnumerable().Where(p => Convert.ToDateTime(p["data"]) == Convert.ToDateTime(d.Value)).First()["data"] : d.Value,
                                                    numeroAulas = planosPermissaoDocente.AsEnumerable().Any() && planosPermissaoDocente.AsEnumerable().Any(p => Convert.ToDateTime(p["data"]) == Convert.ToDateTime(d.Value)) ? planosPermissaoDocente.AsEnumerable().Where(p => Convert.ToDateTime(p["data"]) == Convert.ToDateTime(d.Value)).First()["numeroAulas"] : "0",
                                                    planoAula = planosPermissaoDocente.AsEnumerable().Any() && planosPermissaoDocente.AsEnumerable().Any(p => Convert.ToDateTime(p["data"]) == Convert.ToDateTime(d.Value)) ? planosPermissaoDocente.AsEnumerable().Where(p => Convert.ToDateTime(p["data"]) == Convert.ToDateTime(d.Value)).First()["planoAula"] : "",
                                                    conteudo = planosPermissaoDocente.AsEnumerable().Any() && planosPermissaoDocente.AsEnumerable().Any(p => Convert.ToDateTime(p["data"]) == Convert.ToDateTime(d.Value)) ? planosPermissaoDocente.AsEnumerable().Where(p => Convert.ToDateTime(p["data"]) == Convert.ToDateTime(d.Value)).First()["conteudo"] : "",
                                                    situacao = planosPermissaoDocente.AsEnumerable().Any() && planosPermissaoDocente.AsEnumerable().Any(p => Convert.ToDateTime(p["data"]) == Convert.ToDateTime(d.Value)) ? planosPermissaoDocente.AsEnumerable().Where(p => Convert.ToDateTime(p["data"]) == Convert.ToDateTime(d.Value)).First()["situacao"] : "1",
                                                    dataCriacao = planosPermissaoDocente.AsEnumerable().Any() && planosPermissaoDocente.AsEnumerable().Any(p => Convert.ToDateTime(p["data"]) == Convert.ToDateTime(d.Value)) ? planosPermissaoDocente.AsEnumerable().Where(p => Convert.ToDateTime(p["data"]) == Convert.ToDateTime(d.Value)).First()["dataCriacao"] : DateTime.Now.ToShortDateString(),
                                                    dataAlteracao = planosPermissaoDocente.AsEnumerable().Any() && planosPermissaoDocente.AsEnumerable().Any(p => Convert.ToDateTime(p["data"]) == Convert.ToDateTime(d.Value)) ? planosPermissaoDocente.AsEnumerable().Where(p => Convert.ToDateTime(p["data"]) == Convert.ToDateTime(d.Value)).First()["dataAlteracao"] : DateTime.Now.ToShortDateString(),
                                                    sintese = planosPermissaoDocente.AsEnumerable().Any() && planosPermissaoDocente.AsEnumerable().Any(p => Convert.ToDateTime(p["data"]) == Convert.ToDateTime(d.Value)) ? planosPermissaoDocente.AsEnumerable().Where(p => Convert.ToDateTime(p["data"]) == Convert.ToDateTime(d.Value)).First()["sintese"] : "",
                                                    tdt_posicao = planosPermissaoDocente.AsEnumerable().Any() && planosPermissaoDocente.AsEnumerable().Any(p => Convert.ToDateTime(p["data"]) == Convert.ToDateTime(d.Value)) ? planosPermissaoDocente.AsEnumerable().Where(p => Convert.ToDateTime(p["data"]) == Convert.ToDateTime(d.Value)).First()["tdt_posicao"] : VS_tdt_posicao.ToString(),
                                                    usu_id = planosPermissaoDocente.AsEnumerable().Any() && planosPermissaoDocente.AsEnumerable().Any(p => Convert.ToDateTime(p["data"]) == Convert.ToDateTime(d.Value)) ? planosPermissaoDocente.AsEnumerable().Where(p => Convert.ToDateTime(p["data"]) == Convert.ToDateTime(d.Value)).First()["usu_id"] : Guid.Empty.ToString(),
                                                    permissaoAlteracao = planosPermissaoDocente.AsEnumerable().Any() && planosPermissaoDocente.AsEnumerable().Any(p => Convert.ToDateTime(p["data"]) == Convert.ToDateTime(d.Value)) ? planosPermissaoDocente.AsEnumerable().Where(p => Convert.ToDateTime(p["data"]) == Convert.ToDateTime(d.Value)).First()["permissaoAlteracao"] : "0",
                                                    semPlanoAula = planosPermissaoDocente.AsEnumerable().Any() && planosPermissaoDocente.AsEnumerable().Any(p => Convert.ToDateTime(p["data"]) == Convert.ToDateTime(d.Value)) ? planosPermissaoDocente.AsEnumerable().Where(p => Convert.ToDateTime(p["data"]) == Convert.ToDateTime(d.Value)).First()["semPlanoAula"] : "false"
                                                });
                rptAulas.DataBind();
                    
                Dictionary<int, string> nomeDiasSemana = new Dictionary<int, string>();
                nomeDiasSemana.Add(1, "Segunda-feira"); nomeDiasSemana.Add(2, "Terça-feira"); nomeDiasSemana.Add(3, "Quarta-feira");
                nomeDiasSemana.Add(4, "Quinta-feira"); nomeDiasSemana.Add(5, "Sexta-feira"); nomeDiasSemana.Add(6, "Sábado");
                rptDiasSemana.DataSource = diasSemana.OrderBy(p => p.Key).Select(p => new { data = p.Value, diaSemana = nomeDiasSemana[p.Key] });
                rptDiasSemana.DataBind();
            }
            catch (ValidationException ex)
            {
                _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ControleSemanal.Cadastro.ErroCarregarAulas").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Salva os dados da tela
        /// </summary>
        /// <param name="navegacao">Informa se é navegação na tela, se não for verifica o parâmetro para permanecer na tela</param>
        private void Salvar(bool navegacao)
        {
            string tau_ids = "";
            List<CLS_TurmaAula> lstTurmaAula = new List<CLS_TurmaAula>();
            List<CLS_TurmaAulaPlanoDisciplina> lstTurmaAulaPlanoDisc = new List<CLS_TurmaAulaPlanoDisciplina>();
            List<CLS_TurmaAulaPlanoDisciplina> lstTurmaAulaPlanoDiscDeletar = new List<CLS_TurmaAulaPlanoDisciplina>();

            foreach (RepeaterItem item in rptAulas.Items)
            {
                List<CLS_TurmaAulaPlanoDisciplina> lstTurmaAulaPlanoDiscAux = new List<CLS_TurmaAulaPlanoDisciplina>();
                List<CLS_TurmaAulaPlanoDisciplina> lstTurmaAulaPlanoDiscDeletarAux = new List<CLS_TurmaAulaPlanoDisciplina>();
                int permiteAlteracao;
                Int32.TryParse((((HiddenField)item.FindControl("hdnPermissaoAlteracao")).Value), out permiteAlteracao);

                bool permissaoAlteracao = __SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.Gestao
                                            && __SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.UnidadeAdministrativa
                                            && permiteAlteracao > 0
                                            && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;

                if (permissaoAlteracao)
                {
                    HiddenField hdftau_id = (HiddenField)item.FindControl("hdftau_id");
                    CheckBoxList chlComponenteCurricular = (CheckBoxList)item.FindControl("chlComponenteCurricular");

                    if (hdftau_id != null && !string.IsNullOrEmpty(hdftau_id.Value) && Convert.ToInt32(hdftau_id.Value) > 0)
                    {
                        CLS_TurmaAula tau = new CLS_TurmaAula { tud_id = VS_tud_id, tau_id = Convert.ToInt32(hdftau_id.Value) };
                        CLS_TurmaAulaBO.GetEntity(tau);

                        lstTurmaAulaPlanoDiscDeletarAux.Add(new CLS_TurmaAulaPlanoDisciplina
                        {
                            tud_id = VS_tud_id,
                            tau_id = tau.tau_id,
                            tud_idPlano = -1
                        });

                        foreach (ListItem ckb in chlComponenteCurricular.Items)
                        {
                            if (ckb.Selected)
                            {
                                //existeDisciplinaPlanoRegencia = true;
                                lstTurmaAulaPlanoDiscAux.Add(new CLS_TurmaAulaPlanoDisciplina
                                {
                                    tud_id = VS_tud_id,
                                    tau_id = tau.tau_id,
                                    tud_idPlano = Convert.ToInt64(ckb.Value.Split(';')[1])
                                });
                            }
                        }

                        List<CLS_TurmaAulaPlanoDisciplina> lstBanco = CLS_TurmaAulaPlanoDisciplinaBO.SelectBy_aulaDisciplina(VS_tud_id, tau.tau_id);

                        TextBox txtPlanoAula = (TextBox)item.FindControl("txtPlanoAula");

                        if (txtPlanoAula != null)
                        {
                            //Se não houve alteração dos dados então não precisa salvar
                            if (((!lstBanco.Any() && !lstTurmaAulaPlanoDiscAux.Any()) ||
                                 (!lstBanco.Any(b => !lstTurmaAulaPlanoDiscAux.Any(p => p.tud_idPlano == b.tud_idPlano)) &&
                                  !lstTurmaAulaPlanoDiscAux.Any(b => !lstBanco.Any(p => p.tud_idPlano == b.tud_idPlano)))) &&
                                ((string.IsNullOrEmpty(txtPlanoAula.Text) && (tau.IsNew || tau.tau_planoAula == null || string.IsNullOrEmpty(tau.tau_planoAula))) ||
                                 txtPlanoAula.Text.Equals(tau.tau_planoAula)))
                                continue;

                            tau.tau_planoAula = txtPlanoAula.Text;
                            lstTurmaAulaPlanoDisc.AddRange(lstTurmaAulaPlanoDiscAux);
                            lstTurmaAulaPlanoDiscDeletar.AddRange(lstTurmaAulaPlanoDiscDeletarAux);
                        }

                        tau.tau_statusPlanoAula = (byte)CLS_TurmaAulaBO.RetornaStatusPlanoAula(tau,
                                                    ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_SINTESE_REGENCIA_AULA_TURMA, __SessionWEB.__UsuarioWEB.Usuario.ent_id),
                                                    VS_cal_ano < 2015 || !ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PLANEJAMENTO_ANUAL_CICLO, __SessionWEB.__UsuarioWEB.Usuario.ent_id));

                        tau.tau_dataAlteracao = DateTime.Now;
                        tau_ids += (string.IsNullOrEmpty(tau_ids) ? "" : ",") + tau.tau_id.ToString();
                        tau.usu_idDocenteAlteracao = __SessionWEB.__UsuarioWEB.Usuario.usu_id;

                        lstTurmaAula.Add(tau);
                    }
                }
            }

            if (lstTurmaAula.Any())
            {
                CLS_TurmaAulaPlanoDisciplinaBO.SalvarEmLote(lstTurmaAula, lstTurmaAulaPlanoDisc, lstTurmaAulaPlanoDiscDeletar);

                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "tud_id: " + VS_tud_id.ToString() + "tau_ids: " + tau_ids);
                _lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ControleSemanal.Cadastro.PlanejamentoSalvoSucesso").ToString(), UtilBO.TipoMensagem.Sucesso);
                if (!navegacao && !ParametroPermanecerTela)
                {
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ControleSemanal.Cadastro.PlanejamentoSalvoSucesso").ToString(), UtilBO.TipoMensagem.Sucesso);
                    Response.Redirect("~/Academico/ControleSemanal/Busca.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    CarregarAulasData();
                }
            }
            else if (!navegacao)
            {
                _lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ControleSemanal.Cadastro.NenhumPlanejamentoAlterado").ToString(), UtilBO.TipoMensagem.Informacao);
            }
        }

        #endregion Métodos
    }
}