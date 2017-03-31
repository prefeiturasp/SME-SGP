using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.Academico.ControleSemanal
{
    public partial class Cadastro : MotherPageLogado
    {

        #region Delegates

        public delegate void CarregaDadosTela();

        public CarregaDadosTela OnCarregaDadosTela;

        public delegate void AlteraPeriodo();

        public AlteraPeriodo OnAlteraPeriodo;

        public delegate void commandRecarregar(bool atualizaData, bool proximo, bool anterior, bool inalterado);

        public event commandRecarregar Recarregar;

        #endregion Delegates
        
        #region Propriedades

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
        /// view state que armeza a descrição do período selecionado
        /// </summary>
        public string VS_cap_Descricao
        {
            get
            {
                Struct_CalendarioPeriodos per = VS_CalendarioPeriodo.Find(p => p.tpc_id == VS_tpc_id && p.tpc_ordem == VS_tpc_ordem);

                if (per.cap_descricao != null)
                {
                    return per.cap_descricao;
                }

                return "";
            }
        }

        /// <summary>
        /// view state que armeza o cap_id do período
        /// </summary>
        public int VS_cap_id
        {
            get
            {
                Struct_CalendarioPeriodos per = VS_CalendarioPeriodo.Find(p => p.tpc_id == VS_tpc_id && p.tpc_ordem == VS_tpc_ordem);

                if (per.cap_id > 0)
                {
                    return per.cap_id;
                }

                return -1;
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de cal_ano
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
        /// ViewState que armazena se deve ser adicionado o botao Final no final dos períodos.
        /// </summary>
        public bool VS_IncluirPeriodoFinal
        {
            get
            {
                return (bool)(ViewState["VS_IncluirPeriodoFinal"] ?? false);
            }

            set
            {
                ViewState["VS_IncluirPeriodoFinal"] = value;
            }
        }

        #endregion Propriedades

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string message = __SessionWEB.PostMessages;
                if (!String.IsNullOrEmpty(message))
                    _lblMessage.Text = message;

                if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                {

                }

            }
        }

        protected void btnPeriodo_Click(object sender, EventArgs e)
        {
            Button btnPeriodo = (Button)sender;
            RepeaterItem itemPeriodo = (RepeaterItem)btnPeriodo.NamingContainer;
            Repeater rptPeriodo = (Repeater)itemPeriodo.NamingContainer;
            HiddenField hdnPeriodo = (HiddenField)itemPeriodo.FindControl("hdnPeriodo");
            HiddenField hdnPeriodoOrdem = (HiddenField)itemPeriodo.FindControl("hdnPeriodoOrdem");

            VS_tpc_id = Convert.ToInt32(hdnPeriodo.Value);
            VS_tpc_ordem = Convert.ToInt32(hdnPeriodoOrdem.Value);

            rptPeriodo.Items.Cast<RepeaterItem>().ToList()
                        .Select(p => (Button)p.FindControl("btnPeriodo"))
                        .ToList().ForEach(p => RemoveClass(p, "periodo_selecionado"));

            AddClass(btnPeriodo, "periodo_selecionado");

            // Chamar evento de mudar de período.
            if (OnAlteraPeriodo != null)
                OnAlteraPeriodo();
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
            if (this.Recarregar != null)
                this.Recarregar(true, false, true, false);
        }

        protected void lkbProximo_Click(object sender, EventArgs e)
        {
            if (this.Recarregar != null)
                this.Recarregar(true, true, false, false);
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

        #endregion Eventos

        #region Métodos

        /// <summary>
        /// Carregar os períodos e seta a visibilidade dos botões de acordo com a permissão do usuário.
        /// </summary>
        public void CarregarPeriodos
        (
            TUR_TurmaDisciplina VS_turmaDisciplinaRelacionada
            , int esc_id
            , byte tud_tipo
            , byte tdt_posicao
            , Int64 tur_id
            , Int64 tud_id
        )
        {
            if (tur_id > 0 && tud_id > 0 && ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PRE_CARREGAR_CACHE_EFETIVACAO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
            {
                TUR_Turma entityTurma = new TUR_Turma { tur_id = tur_id };
                TUR_TurmaBO.GetEntity(entityTurma);

                TUR_TurmaDisciplina entityTurmaDisciplina = new TUR_TurmaDisciplina { tud_id = tud_id };
                TUR_TurmaDisciplinaBO.GetEntity(entityTurmaDisciplina);
                
                hdnTudId.Value = tud_id.ToString();
                hdnTurId.Value = entityTurma.tur_id.ToString();
                hdnTurTipo.Value = entityTurma.tur_tipo.ToString();
                hdnCalId.Value = entityTurma.cal_id.ToString();
                hdnTudTipo.Value = entityTurmaDisciplina.tud_tipo.ToString();
                hdnTipoDocente.Value = (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0 ?
                            (byte)ACA_TipoDocenteBO.SelecionaTipoDocentePorPosicao(tdt_posicao, ApplicationWEB.AppMinutosCacheLongo) : (byte)0).ToString();
                
            }
            
            
            List<Struct_CalendarioPeriodos> lstCalendarioPeriodos = ACA_CalendarioPeriodoBO.SelecionaPor_Calendario(VS_cal_id, ApplicationWEB.AppMinutosCacheLongo, false, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            
            VS_CalendarioPeriodo = lstCalendarioPeriodos;
            
            VS_cal_ano = VS_CalendarioPeriodo.Find(p => p.cal_id == VS_cal_id).cal_ano;

            List<ESC_EscolaCalendarioPeriodo> lstEscCalPeriodo = ESC_EscolaCalendarioPeriodoBO.SelectEscolasCalendarioCache(VS_cal_id, ApplicationWEB.AppMinutosCacheCurto);

            VS_CalendarioPeriodo = VS_CalendarioPeriodo.Where(calP => (lstEscCalPeriodo.Where(escP => (escP.esc_id == esc_id && escP.tpc_id == calP.tpc_id)).Count() == 0)).ToList();

            if (VS_IncluirPeriodoFinal)
            {
                Struct_CalendarioPeriodos[] calendarioPeriodosCopy = new Struct_CalendarioPeriodos[VS_CalendarioPeriodo.Count() + 1];
                VS_CalendarioPeriodo.CopyTo(calendarioPeriodosCopy, 0);

                Struct_CalendarioPeriodos periodoFinal = new Struct_CalendarioPeriodos();
                periodoFinal.cap_descricao = periodoFinal.tpc_nomeAbreviado = GetGlobalResourceObject("UserControl", "NavegacaoTelaPeriodo.UCNavegacaoTelaPeriodo.PeriodoFinal").ToString();
                periodoFinal.tpc_id = -1;
                calendarioPeriodosCopy[VS_CalendarioPeriodo.Count()] = periodoFinal;

                rptPeriodo.DataSource = calendarioPeriodosCopy;
            }
            else
            {
                rptPeriodo.DataSource = VS_CalendarioPeriodo;
            }
            
            rptPeriodo.DataBind();

            //Seleciona o ultimo bimestre
            List<Struct_CalendarioPeriodos> tabelaPeriodos = ACA_CalendarioPeriodoBO.SelecionaPor_Calendario(VS_cal_id, ApplicationWEB.AppMinutosCacheLongo);
            int tpc_idUltimoPeriodo = tabelaPeriodos.Count > 0 ? tabelaPeriodos.Last().tpc_id : -1;
            int tpc_ordemUltimoPeriodo = tabelaPeriodos.Count > 0 ? tabelaPeriodos.Last().tpc_ordem : 0;

            if (VS_tpc_id <= 0 && !VS_IncluirPeriodoFinal)
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

            if (VS_tpc_id >= 0 && VS_IncluirPeriodoFinal)
            {
                if (VS_tpc_id == tpc_idUltimoPeriodo)
                {
                    // Se for o ultimo periodo e a avaliacao final estiver aberta,
                    // selecionar a avaliacao final
                    List<ACA_Evento> listaEventos = ACA_EventoBO.GetEntity_Efetivacao_List(VS_cal_id, tur_id, __SessionWEB.__UsuarioWEB.Grupo.gru_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo, true, __SessionWEB.__UsuarioWEB.Docente.doc_id);
                    if (listaEventos.Exists(p => p.tev_id == ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id)))
                    {
                        VS_tpc_id = -1;
                        VS_tpc_ordem = 0;
                    }
                }

                if (VS_tpc_id == 0)
                {
                    //Se não tem bimestre selecionado e nem bimestre corrente então seleciona o próximo corrente
                    Struct_CalendarioPeriodos periodo = VS_CalendarioPeriodo.Where(x => (x.cap_dataInicio.Date >= DateTime.Now.Date)).FirstOrDefault();
                    VS_tpc_id = periodo.tpc_id;
                    VS_tpc_ordem = periodo.tpc_ordem;

                    if (VS_tpc_id <= 0)
                    {
                        //Se não tem bimestre selecionado então seleciona o final
                        VS_tpc_id = -1;
                        VS_tpc_ordem = 0;
                    }
                }
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


        /// <summary>
        /// Mostra/esconde os controles de mudança de semana.
        /// </summary>
        /// <param name="mostra">Indica se vai exibir os controles</param>
        private void MostraPeriodo(bool mostra)
        {
            lblInicio.Visible = mostra;
            lblFim.Visible = mostra;
            td_lkbAnterior.Visible = mostra;
            td_lkbProximo.Visible = mostra;
        }

        #endregion Métodos
    }
}