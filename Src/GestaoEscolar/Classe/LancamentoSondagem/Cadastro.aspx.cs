using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;

namespace GestaoEscolar.Classe.LancamentoSondagem
{
    public partial class Cadastro : MotherPageLogado
    {
        #region Propriedades

        /// <summary>
        /// Propriedade em ViewState que armazena valor de snd_id.
        /// </summary>
        private int VS_snd_id
        {
            get
            {
                if (ViewState["VS_snd_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_snd_id"]);
                }
                return -1;
            }
            set
            {
                ViewState["VS_snd_id"] = value;
            }
        }

        /// <summary>
        /// Propriedade em ViewState que armazena valor de sda_id.
        /// </summary>
        private int VS_sda_id
        {
            get
            {
                if (ViewState["VS_sda_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_sda_id"]);
                }
                return -1;
            }
            set
            {
                ViewState["VS_sda_id"] = value;
            }
        }

        /// <summary>
        /// Propriedade em ViewState que armazena se o lançamento está habilitado.
        /// </summary>
        private bool VS_responder
        {
            get
            {
                if (ViewState["VS_responder"] != null)
                {
                    return Convert.ToBoolean(ViewState["VS_responder"]);
                }
                return false;
            }
            set
            {
                ViewState["VS_responder"] = value;
            }
        }

        /// <summary>
        /// Propriedade em ViewState que armazena os dados do lançamento da turma.
        /// </summary>
        private List<ACA_Sondagem_Lancamento> VS_lstLancamentoTurma
        {
            get
            {
                if (ViewState["VS_lstLancamentoTurma"] == null)
                {
                     ViewState["VS_lstLancamentoTurma"] = new List<ACA_Sondagem_Lancamento>();
                }
                return (List<ACA_Sondagem_Lancamento>)ViewState["VS_lstLancamentoTurma"];
            }
            set
            {
                ViewState["VS_lstLancamentoTurma"] = value;
            }
        }

        private List<Questao> lstQuestoes = new List<Questao>();
        private List<Questao> lstSubQuestoes = new List<Questao>();
        private List<Resposta> lstRespostas = new List<Resposta>();

        #endregion

        #region Estruturas

        /// <summary>
        /// Estrutura que armazena as questões e sub questões da sondagem.
        /// </summary>
        [Serializable]
        private struct Questao
        {
            public int sdq_id { get; set; }
            public string sdq_descricao { get; set; }
            public int sdq_ordem { get; set; }
        }

        /// <summary>
        /// Estrutura que armazena as respostas da sondagem.
        /// </summary>
        [Serializable]
        private struct Resposta
        {
            public int sdr_id { get; set; }
            public string sdr_sigla { get; set; }
            public string sdr_descricao { get; set; }
            public int sdr_ordem { get; set; }
        }

        #endregion Estruturas

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            // Seta o metodo criado no delegate ao evento do componente
            UCComboUAEscola.IndexChangedUnidadeEscola += UCComboUAEscola_IndexChangedUnidadeEscola;
            UCComboUAEscola.IndexChangedUA += UCComboUAEscola_IndexChangedUA;
            UCComboCalendario.IndexChanged += UCComboCalendario_IndexChanged;
            UCCCursoCurriculo.IndexChanged += UCCCursoCurriculo_IndexChanged;

            if (!IsPostBack)
            {
                try
                {
                    if (PreviousPage != null && PreviousPage.IsCrossPagePostBack && (PreviousPage.EditItem[0] > 0 || PreviousPage.SelectedItem[0] > 0))
                    {
                        if (PreviousPage.EditItem[0] > 0)
                        {
                            VS_snd_id = PreviousPage.EditItem[0];
                            VS_sda_id = PreviousPage.EditItem[1];
                            VS_responder = true;
                        }
                        else
                        {
                            VS_snd_id = PreviousPage.SelectedItem[0];
                            VS_sda_id = PreviousPage.SelectedItem[1];
                            VS_responder = false;
                        }

                        btnSalvar.Visible = false;
                        btnCancelar.Text = VS_responder && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar ?
                                           GetGlobalResourceObject("Classe", "LancamentoSondagem.Cadastro.btnCancelar.Text").ToString() :
                                           GetGlobalResourceObject("Classe", "LancamentoSondagem.Cadastro.btnVoltar.Text").ToString();

                        ACA_Sondagem entity = ACA_SondagemBO.GetEntity(new ACA_Sondagem { snd_id = VS_snd_id });
                        lblDadosSondagem.Text = string.Format(GetGlobalResourceObject("Classe", "LancamentoSondagem.Cadastro.lblDadosSondagem.Titulo").ToString(), entity.snd_titulo);
                        if (!string.IsNullOrEmpty(entity.snd_descricao))
                        {
                            lblDadosSondagem.Text += string.Format(GetGlobalResourceObject("Classe", "LancamentoSondagem.Cadastro.lblDadosSondagem.Descricao").ToString(), entity.snd_descricao);
                        }

                        lblResultadoVazio.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Classe", "LancamentoSondagem.Cadastro.lblResultadoVazio.Text").ToString(), UtilBO.TipoMensagem.Nenhuma);
                        ddlTurma.Items.Insert(0, new ListItem(GetGlobalResourceObject("Classe", "LancamentoSondagem.Cadastro.ddlTurma.MsgSelecione").ToString(), "-1", true));

                        if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual)
                        {
                            // Busca o doc_id do usuário logado.
                            if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                            {
                                //Esconde os campos não visíveis para docentes
                                UCCCursoCurriculo.Visible = false;
                                ddlTurma.Enabled = false;

                                //Carrega as escolas no combo
                                UCComboUAEscola.InicializarVisaoIndividual(__SessionWEB.__UsuarioWEB.Docente.doc_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                                if (UCComboUAEscola.QuantidadeItemsComboEscolas == 2)
                                {
                                    ddlTurma.Enabled = true;
                                    InicializaFiltroVisaoIndividual(UCComboUAEscola.Esc_ID);
                                }
                                else
                                {
                                    InicializaFiltroVisaoIndividual(0);
                                }
                            }
                            else
                            {
                                lblMessage.Text = UtilBO.GetErroMessage("Essa tela é exclusiva para docentes.", UtilBO.TipoMensagem.Alerta);
                                Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Classe/LancamentoSondagem/Busca.aspx", false);
                                HttpContext.Current.ApplicationInstance.CompleteRequest();
                            }
                        }
                        else
                        {
                            InicializaFiltro();
                        }
                    }
                    else
                    {
                        __SessionWEB.PostMessages = UtilBO.GetErroMessage(GetGlobalResourceObject("Classe", "LancamentoSondagem.Cadastro.SelecioneSondagem").ToString(), UtilBO.TipoMensagem.Alerta);
                        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Classe/LancamentoSondagem/Busca.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }

                    Page.Form.DefaultFocus = lblDadosSondagem.ClientID;
                    Page.Form.DefaultButton = btnSalvar.UniqueID;
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Classe/LancamentoSondagem/Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            //if (Page.IsValid)
                //Salvar();
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Carrega os campos de busca com a configuração padrão para uma nova busca
        /// </summary>
        private void InicializaFiltro()
        {
            //Carrega os campos
            UCComboUAEscola.Inicializar();

            //if (UCComboUAEscola.VisibleUA)
            UCComboUAEscola_IndexChangedUA();
        }

        /// <summary>
        /// Carrega os campos de busca com a configuração padrão para uma nova busca para visão individual
        /// </summary>
        /// <param name="esc_id">Id da escola</param>
        private void InicializaFiltroVisaoIndividual(int esc_id)
        {
            //Carrega os campos
            ddlTurma.Items.Clear();
            
            ddlTurma.Items.Insert(0, new ListItem(GetGlobalResourceObject("Classe", "LancamentoSondagem.Cadastro.ddlTurma.MsgSelecione").ToString(), "-1", true));

            ddlTurma.DataSource = TUR_TurmaBO.GetSelectBy_SondagemAgendamento(
                VS_snd_id
                , VS_sda_id
                , esc_id
                , UCComboCalendario.Valor
                , -1
                , -1
                , __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual && __SessionWEB.__UsuarioWEB.Docente != null && __SessionWEB.__UsuarioWEB.Docente.doc_id > 0 ?
                    __SessionWEB.__UsuarioWEB.Docente.doc_id : -1
                , __SessionWEB.__UsuarioWEB.Grupo.gru_id
                , __SessionWEB.__UsuarioWEB.Usuario.usu_id
                , __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao
                , __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            ddlTurma.DataBind();
        }

        #endregion

        #region Delegates

        /// <summary>
        /// Verifica alteracao do index do combo UA Escola e trata o combo Escola e Curso curriculo.
        /// </summary>
        protected void UCComboUAEscola_IndexChangedUA()
        {
            UCComboUAEscola.MostraApenasAtivas = true;

            UCComboUAEscola.SelectedValueEscolas = new[] { -1, -1 };

            UCComboUAEscola.EnableEscolas = (UCComboUAEscola.Uad_ID != Guid.Empty || !UCComboUAEscola.DdlUA.Visible);

            UCComboUAEscola_IndexChangedUnidadeEscola();
        }

        /// <summary>
        /// Verifica alteracao do index do combo Escola e trata o combo calendário
        /// </summary>
        protected void UCComboUAEscola_IndexChangedUnidadeEscola()
        {
            try
            {
                if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                {
                    if (UCComboUAEscola.Esc_ID > 0)
                    {
                        ddlTurma.Enabled = true;

                        UCComboCalendario.CarregarCalendariosComBimestresAtivos(UCComboUAEscola.Esc_ID, true);

                        UCComboCalendario.SetarFoco();
                        UCComboCalendario.PermiteEditar = true;
                    }
                    else
                    {
                        ddlTurma.SelectedIndex = 0;
                        ddlTurma.Enabled = false;

                        UCComboCalendario.Valor = -1;
                        UCComboCalendario.PermiteEditar = false;
                    }
                }
                else
                {
                    UCComboCalendario.Valor = -1;
                    UCComboCalendario.PermiteEditar = false;

                    if (UCComboUAEscola.Esc_ID > 0 && UCComboUAEscola.Uni_ID > 0)
                    {
                        UCComboCalendario.CarregarCalendariosComBimestresAtivos(UCComboUAEscola.Esc_ID, true);

                        UCComboCalendario.SetarFoco();
                        UCComboCalendario.PermiteEditar = true;
                    }

                    UCComboCalendario_IndexChanged();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Verifica alteracao do index do combo calendario e trata o combo Curso curriculo.
        /// </summary>
        protected void UCComboCalendario_IndexChanged()
        {
            try
            {
                if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                {
                    if (UCComboCalendario.Valor > 0)
                    {
                        UCComboCalendario.PermiteEditar = true;
                        ddlTurma.Enabled = true;
                        InicializaFiltroVisaoIndividual(UCComboUAEscola.Esc_ID);
                    }
                    else
                    {
                        ddlTurma.SelectedIndex = 0;
                        ddlTurma.Enabled = false;
                    }
                }
                else
                {

                    UCCCursoCurriculo.Valor = new[] { -1, -1 };
                    UCCCursoCurriculo.PermiteEditar = false;

                    if (UCComboCalendario.Valor > 0 && UCComboUAEscola.Esc_ID > 0 && UCComboUAEscola.Uni_ID > 0)
                    {
                        UCCCursoCurriculo.CarregarPorEscolaCalendarioSituacaoCurso(UCComboUAEscola.Esc_ID, UCComboUAEscola.Uni_ID, UCComboCalendario.Valor, (byte)ACA_CursoSituacao.Ativo);

                        UCCCursoCurriculo.SetarFoco();
                        UCCCursoCurriculo.PermiteEditar = true;
                    }

                    UCCCursoCurriculo_IndexChanged();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void UCCCursoCurriculo_IndexChanged()
        {
            try
            {
                ddlTurma.SelectedValue = "-1";
                ddlTurma.Enabled = false;

                if (UCCCursoCurriculo.Valor[0] > 0 && UCCCursoCurriculo.Valor[1] > 0)
                {
                    ddlTurma.Items.Clear();

                    ddlTurma.Items.Clear();

                    ddlTurma.Items.Insert(0, new ListItem(GetGlobalResourceObject("Classe", "LancamentoSondagem.Cadastro.ddlTurma.MsgSelecione").ToString(), "-1", true));

                    ddlTurma.DataSource = TUR_TurmaBO.GetSelectBy_SondagemAgendamento(
                        VS_snd_id
                        , VS_sda_id
                        , UCComboUAEscola.Esc_ID
                        , UCComboCalendario.Valor
                        , UCCCursoCurriculo.Valor[0]
                        , UCCCursoCurriculo.Valor[1]
                        , __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual && __SessionWEB.__UsuarioWEB.Docente != null && __SessionWEB.__UsuarioWEB.Docente.doc_id > 0 ?
                            __SessionWEB.__UsuarioWEB.Docente.doc_id : -1
                        , __SessionWEB.__UsuarioWEB.Grupo.gru_id
                        , __SessionWEB.__UsuarioWEB.Usuario.usu_id
                        , __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao
                        , __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                    ddlTurma.DataBind();

                    ddlTurma.Focus();
                    ddlTurma.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                long tur_id = Convert.ToInt64(ddlTurma.SelectedValue);
                if (tur_id > 0)
                {
                    btnSalvar.Visible = VS_responder && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;

                    VS_lstLancamentoTurma = ACA_SondagemBO.SelecionaSondagemLancamentoTurma(VS_snd_id, VS_sda_id, tur_id);
                    if (VS_lstLancamentoTurma.Any())
                    {
                        rptLancamento.DataSource = (
                                from ACA_Sondagem_Lancamento dr in VS_lstLancamentoTurma
                                group dr by dr.alu_id into g
                                select new
                                {
                                    alu_id = g.Key
                                    ,
                                    pes_nome = g.First().pes_nome
                                    ,
                                    mtu_numeroChamada = g.First().mtu_numeroChamada
                                }
                            ).ToList().OrderBy(p => p.pes_nome);

                        if (VS_lstLancamentoTurma.Any(p => p.sdq_id > 0))
                        {
                            lstQuestoes = (
                                from ACA_Sondagem_Lancamento dr in VS_lstLancamentoTurma.Where(p => p.sdq_id > 0)
                                group dr by dr.sdq_id into g
                                select new Questao
                                {
                                    sdq_id = g.Key
                                    ,
                                    sdq_descricao = g.First().sdq_descricao
                                    , 
                                    sdq_ordem = g.First().sdq_ordem
                                }
                            ).OrderBy(p => p.sdq_ordem).ToList();
                        }

                        if (VS_lstLancamentoTurma.Any(p => p.sdq_idSub > 0))
                        {
                            lstSubQuestoes = (
                                from ACA_Sondagem_Lancamento dr in VS_lstLancamentoTurma.Where(p => p.sdq_idSub > 0)
                                group dr by dr.sdq_idSub into g
                                select new Questao
                                {
                                    sdq_id = g.Key
                                    ,
                                    sdq_descricao = g.First().sdq_descricaoSub
                                    ,
                                    sdq_ordem = g.First().sdq_ordemSub
                                }
                            ).OrderBy(p => p.sdq_ordem).ToList();
                        }

                        if (VS_lstLancamentoTurma.Any(p => p.sdr_id > 0))
                        {
                            lstRespostas = (
                                from ACA_Sondagem_Lancamento dr in VS_lstLancamentoTurma.Where(p => p.sdr_id > 0)
                                group dr by dr.sdr_id into g
                                select new Resposta
                                {
                                    sdr_id = g.Key
                                    ,
                                    sdr_sigla = g.First().sdr_sigla
                                    ,
                                    sdr_descricao = g.First().sdr_descricao
                                    ,
                                    sdr_ordem = g.First().sdr_ordem
                                }
                            ).OrderBy(p => p.sdr_ordem).ToList();
                        }

                        lblResultadoVazio.Visible = false;
                        rptLancamento.Visible = true;
                    }
                    else
                    {
                        rptLancamento.DataSource = new List<ACA_Sondagem_Lancamento>();
                        lblResultadoVazio.Visible = true;
                        rptLancamento.Visible = false;
                    }
                    rptLancamento.DataBind();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void rptLancamento_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                if (lstQuestoes.Count > 0)
                {
                    Repeater rptQuestoes = (Repeater)e.Item.FindControl("rptQuestoes");
                    if (rptQuestoes != null)
                    {
                        rptQuestoes.DataSource = lstQuestoes;
                        rptQuestoes.DataBind();
                    }
                }

                int totalRepetir = 0;

                if (lstSubQuestoes.Count > 0)
                {
                    totalRepetir = lstQuestoes.Count > 0 ? lstQuestoes.Count : 1;

                    List<Questao> lstSubQuestoesQuestao = new List<Questao>();
                    for (int i = 0; i < totalRepetir; i++)
                    {
                        lstSubQuestoesQuestao.AddRange(lstSubQuestoes);
                    }

                    Repeater rptSubQuestoes = (Repeater)e.Item.FindControl("rptSubQuestoes");
                    if (rptSubQuestoes != null)
                    {
                        rptSubQuestoes.DataSource = lstSubQuestoesQuestao;
                        rptSubQuestoes.DataBind();
                    }
                }

                if (lstRespostas.Count > 0)
                {
                    totalRepetir = (lstQuestoes.Count > 0 ? lstQuestoes.Count : 1) * (lstSubQuestoes.Count > 0 ? lstSubQuestoes.Count : 1);

                    List<Resposta> lstRespostasQuestao = new List<Resposta>();
                    for (int i = 0; i < totalRepetir; i++)
                    {
                        lstRespostasQuestao.AddRange(lstRespostas);
                    }

                    Repeater rptRespostas = (Repeater)e.Item.FindControl("rptRespostas");
                    if (rptRespostas != null)
                    {
                        rptRespostas.DataSource = lstRespostasQuestao;
                        rptRespostas.DataBind();
                    }
                }
            }
        }

        protected void rptQuestoes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlTableCell thQuestao = (HtmlTableCell)e.Item.FindControl("thQuestao");
                if (thQuestao != null)
                {
                    int numSubQuestoes = lstSubQuestoes.Count() > 0 ? lstSubQuestoes.Count() : 1;
                    thQuestao.Attributes.Add("colspan", (numSubQuestoes * lstRespostas.Count()).ToString());
                }
            }
        }

        protected void rptSubQuestoes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlTableCell thSubQuestao = (HtmlTableCell)e.Item.FindControl("thSubQuestao");
                if (thSubQuestao != null)
                {
                    thSubQuestao.Attributes.Add("colspan", lstRespostas.Count().ToString());
                }
            }
        }

        #endregion
    }
}