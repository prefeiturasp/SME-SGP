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
        #region Constantes

        private const int maxRespostasPagina = 15; 

        #endregion

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

        /// <summary>
        /// Propriedade em ViewState que armazena a quantidade de questões por página.
        /// </summary>
        private int VS_QtdQuestoesPagina
        {
            get
            {
                if (ViewState["VS_QtdQuestoesPagina"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_QtdQuestoesPagina"]);
                }
                return -1;
            }
            set
            {
                ViewState["VS_QtdQuestoesPagina"] = value;
            }
        }

        /// <summary>
        /// Propriedade em ViewState que armazena a quantidade de subquestões por página.
        /// </summary>
        private int VS_QtdSubQuestoesPagina
        {
            get
            {
                if (ViewState["VS_QtdSubQuestoesPagina"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_QtdSubQuestoesPagina"]);
                }
                return -1;
            }
            set
            {
                ViewState["VS_QtdSubQuestoesPagina"] = value;
            }
        }

        /// <summary>
        /// Propriedade em ViewState que armazena a quantidade de respostas por página.
        /// </summary>
        private int VS_QtdRespostasPagina
        {
            get
            {
                if (ViewState["VS_QtdRespostasPagina"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_QtdRespostasPagina"]);
                }
                return -1;
            }
            set
            {
                ViewState["VS_QtdRespostasPagina"] = value;
            }
        }

        /// <summary>
        /// Propriedade em ViewState que armazena valor da página atual.
        /// </summary>
        private int VS_NumPagina
        {
            get
            {
                if (ViewState["VS_NumPagina"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_NumPagina"]);
                }
                return -1;
            }
            set
            {
                ViewState["VS_NumPagina"] = value;
            }
        }

        /// <summary>
        /// Propriedade em ViewState que armazena o total de páginas.
        /// </summary>
        private int VS_TotalPaginas
        {
            get
            {
                if (ViewState["VS_TotalPaginas"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_TotalPaginas"]);
                }
                return -1;
            }
            set
            {
                ViewState["VS_TotalPaginas"] = value;
            }
        }

        private List<Questao> lstQuestoes = new List<Questao>();
        private List<Questao> lstSubQuestoes = new List<Questao>();
        private List<Resposta> lstRespostas = new List<Resposta>();

        string ultimoGrupo = string.Empty;

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
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroLancamentoSondagem.js"));
            }

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

                        ACA_SondagemAgendamento entityAgendamento = ACA_SondagemAgendamentoBO.GetEntity(new ACA_SondagemAgendamento { snd_id = VS_snd_id, sda_id = VS_sda_id });
                        lblDadosSondagem.Text += string.Format(GetGlobalResourceObject("Classe", "LancamentoSondagem.Cadastro.lblDadosSondagem.DataInicioFim").ToString(), entityAgendamento.sda_dataInicio.ToString("dd/MM/yyyy"), entityAgendamento.sda_dataFim.ToString("dd/MM/yyyy"));

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
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
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
            try
            {
                if (Page.IsValid)
                {
                    GuardarRespostasAlunos();
                    Salvar();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Classe", "LancamentoSondagem.Cadastro.ErroSalvar").ToString(), UtilBO.TipoMensagem.Erro);
            }
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

        private void RecarregarGrid()
        {
            rptLancamento.DataSource = (
                from ACA_Sondagem_Lancamento dr in VS_lstLancamentoTurma.Where(p => p.sda_id == VS_sda_id)
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
            rptLancamento.DataBind();
        }

        private void CarregarListasAuxiliares()
        {
            if (VS_lstLancamentoTurma.Any(p => p.sdq_id > 0))
            {
                lstQuestoes = (
                    from ACA_Sondagem_Lancamento dr in VS_lstLancamentoTurma.Where(p => p.sdq_id > 0 && p.sda_id == VS_sda_id)
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
                    from ACA_Sondagem_Lancamento dr in VS_lstLancamentoTurma.Where(p => p.sdq_idSub > 0 && p.sda_id == VS_sda_id)
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
                    from ACA_Sondagem_Lancamento dr in VS_lstLancamentoTurma.Where(p => p.sdr_id > 0 && p.sda_id == VS_sda_id)
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
        }

        private void GuardarRespostasAlunos()
        {
            foreach (RepeaterItem itemAluno in rptLancamento.Items)
            {
                long alu_id = Convert.ToInt64(((HiddenField)itemAluno.FindControl("hdnAluId")).Value);
                Repeater rptRespostasAluno = (Repeater)itemAluno.FindControl("rptRespostasAluno");
                foreach (RepeaterItem itemResposta in rptRespostasAluno.Items)
                {
                    RadioButton rbResposta = (RadioButton)itemResposta.FindControl("rbResposta");
                    HiddenField hdnRespId = (HiddenField)itemResposta.FindControl("hdnRespId");
                    string[] respId = hdnRespId.Value.Split(';');
                    int sdq_id = Convert.ToInt32(respId[0]);
                    int sdq_idSub = Convert.ToInt32(respId[1]);
                    int sdr_id = Convert.ToInt32(respId[2]);
                    for (int i = 0; i < VS_lstLancamentoTurma.Count; i++)
                    {
                        ACA_Sondagem_Lancamento lancamento = VS_lstLancamentoTurma[i];
                        if (lancamento.alu_id == alu_id && lancamento.sdq_id == sdq_id && lancamento.sdq_idSub == sdq_idSub && lancamento.sdr_id == sdr_id)
                        {
                            VS_lstLancamentoTurma[i].respAluno = rbResposta.Checked;
                            break;
                        }
                    }
                }
            }
        }

        private void Salvar()
        {
            CLS_AlunoSondagemBO.SalvarEmLote(VS_lstLancamentoTurma.Where(p => p.sda_id == VS_sda_id).ToList(), VS_snd_id, VS_sda_id);
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
            lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Classe", "LancamentoSondagem.Cadastro.SucessoSalvar").ToString(), UtilBO.TipoMensagem.Sucesso);
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
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
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

                    ddlTurma_SelectedIndexChanged(null, null);
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
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
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

                ddlTurma_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void ddlTurma_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (btnSalvar.Visible && VS_responder && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar)
                {
                    GuardarRespostasAlunos();
                    Salvar();
                }

                long tur_id = Convert.ToInt64(ddlTurma.SelectedValue);
                if (tur_id > 0)
                {
                    VS_lstLancamentoTurma = ACA_SondagemBO.SelecionaSondagemLancamentoTurma(VS_snd_id, VS_sda_id, tur_id);
                    if (VS_lstLancamentoTurma.Where(p => p.sda_id == VS_sda_id).Any())
                    {
                        CarregarListasAuxiliares();

                        // Calcula a quantidade de respostas que deve permanecer junta
                        int qtdRespostasPorQuestao = 0;
                        if (lstSubQuestoes.Any())
                        {
                            qtdRespostasPorQuestao = lstSubQuestoes.Count() * lstRespostas.Count();
                        }
                        else
                        {
                            qtdRespostasPorQuestao = lstRespostas.Count();
                        }

                        VS_NumPagina = 1;
                        if (qtdRespostasPorQuestao < maxRespostasPagina)
                        {
                            int qtdRepetir = maxRespostasPagina / qtdRespostasPorQuestao;
                            VS_QtdRespostasPagina = qtdRepetir * qtdRespostasPorQuestao;

                            if (lstQuestoes.Any())
                            {
                                VS_QtdQuestoesPagina = qtdRepetir;
                            }
                            else
                            {
                                VS_QtdQuestoesPagina = 0;
                            }

                            if (lstSubQuestoes.Any())
                            {
                                if (lstQuestoes.Any())
                                {
                                    VS_QtdSubQuestoesPagina = qtdRepetir * lstSubQuestoes.Count;
                                }
                                else
                                {
                                    VS_QtdSubQuestoesPagina = qtdRepetir;
                                }
                            }
                            else
                            {
                                VS_QtdSubQuestoesPagina = 0;
                            }
                        }
                        else
                        {
                            VS_QtdRespostasPagina = qtdRespostasPorQuestao;

                            if (lstQuestoes.Any())
                            {
                                VS_QtdQuestoesPagina = 1;
                            }
                            else
                            {
                                VS_QtdQuestoesPagina = 0;
                            }

                            if (lstSubQuestoes.Any())
                            {
                                if (lstQuestoes.Any())
                                {
                                    VS_QtdSubQuestoesPagina = lstSubQuestoes.Count;
                                }
                                else
                                {
                                    VS_QtdSubQuestoesPagina = 1;
                                }
                            }
                            else
                            {
                                VS_QtdSubQuestoesPagina = 0;
                            }
                        }

                        VS_TotalPaginas = (lstQuestoes.Count > 0 ? lstQuestoes.Count : 1) * (lstSubQuestoes.Count > 0 ? lstSubQuestoes.Count : 1) * lstRespostas.Count / VS_QtdRespostasPagina;

                        lblResultadoVazio.Visible = false;
                        rptLancamento.Visible = true;
                        RecarregarGrid();
                    }
                    else
                    {
                        rptLancamento.DataSource = new List<ACA_Sondagem_Lancamento>();
                        lblResultadoVazio.Visible = true;
                        rptLancamento.Visible = false;
                        rptLancamento.DataBind();
                    }
                    btnSalvar.Visible = VS_responder && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar && VS_lstLancamentoTurma.Where(p => p.sda_id == VS_sda_id).Any();
                }
                else
                {
                    rptLancamento.DataSource = new List<ACA_Sondagem_Lancamento>();
                    lblResultadoVazio.Visible = false;
                    rptLancamento.Visible = false;
                    rptLancamento.DataBind();
                    btnSalvar.Visible = false;
                }
                updLancamento.Update();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
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
                        rptQuestoes.DataSource = lstQuestoes.Skip((VS_NumPagina - 1) * VS_QtdQuestoesPagina).Take(VS_QtdQuestoesPagina);
                        rptQuestoes.DataBind();
                    }
                }
                else
                {
                    HtmlTableCell trQuestoes = (HtmlTableCell)e.Item.FindControl("trQuestoes");
                    if (trQuestoes != null)
                    {
                        trQuestoes.Visible = false;
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
                        rptSubQuestoes.DataSource = lstSubQuestoesQuestao.Skip((VS_NumPagina - 1) * VS_QtdSubQuestoesPagina).Take(VS_QtdSubQuestoesPagina);
                        rptSubQuestoes.DataBind();
                    }
                }
                else
                {
                    HtmlTableCell trSubQuestoes = (HtmlTableCell)e.Item.FindControl("trSubQuestoes");
                    if (trSubQuestoes != null)
                    {
                        trSubQuestoes.Visible = false;
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
                        rptRespostas.DataSource = lstRespostasQuestao.Skip((VS_NumPagina - 1) * VS_QtdRespostasPagina).Take(VS_QtdRespostasPagina);
                        rptRespostas.DataBind();
                    }
                }

                HtmlTableCell thBotoes = (HtmlTableCell)e.Item.FindControl("thBotoes");
                if (thBotoes != null)
                {
                    thBotoes.Visible = VS_TotalPaginas > 1;
                    if (thBotoes.Visible)
                    {
                        thBotoes.Attributes.Add("colspan", VS_QtdRespostasPagina.ToString());
                    }
                }

                LinkButton lkbAnterior = (LinkButton)e.Item.FindControl("lkbAnterior");
                if (lkbAnterior != null)
                {
                    lkbAnterior.Visible = VS_NumPagina > 1;
                }

                LinkButton lkbProximo = (LinkButton)e.Item.FindControl("lkbProximo");
                if (lkbProximo != null)
                {
                    lkbProximo.Visible = VS_NumPagina < VS_TotalPaginas;
                }
            }
            else if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                long alu_id = Convert.ToInt64(DataBinder.Eval(e.Item.DataItem, "alu_id"));

                HiddenField hdnAluId = (HiddenField)e.Item.FindControl("hdnAluId");
                hdnAluId.Value = alu_id.ToString();

                HtmlTableCell tdRespostas = (HtmlTableCell)e.Item.FindControl("tdRespostas");
                if (tdRespostas != null)
                {
                    tdRespostas.ColSpan = VS_lstLancamentoTurma.Where(p => p.alu_id == alu_id && p.sda_id == VS_sda_id).OrderBy(p => p.sdq_ordem).ThenBy(p => p.sdq_ordemSub).ThenBy(p => p.sdr_ordem).Skip((VS_NumPagina - 1) * VS_QtdRespostasPagina).Take(VS_QtdRespostasPagina).Count();
                }

                Repeater rptRespostasAluno = (Repeater)e.Item.FindControl("rptRespostasAluno");
                if (rptRespostasAluno != null)
                {
                    rptRespostasAluno.DataSource = (
                        from ACA_Sondagem_Lancamento dr in VS_lstLancamentoTurma.Where(p => p.sda_id == VS_sda_id)
                        where dr.alu_id == alu_id
                        select new
                        {
                            alu_id = dr.alu_id
                            , sdr_id = dr.sdr_id
                            , sdr_descricao = dr.sdr_descricao
                            , sdr_ordem = dr.sdr_ordem
                            , sdq_idSub = dr.sdq_idSub
                            , sdq_ordemSub = dr.sdq_ordemSub
                            , sdq_id = dr.sdq_id
                            , sdq_ordem = dr.sdq_ordem
                            , respAluno = dr.respAluno
                        }
                    ).ToList().OrderBy(p => p.sdq_ordem).ThenBy(p => p.sdq_ordemSub).ThenBy(p => p.sdr_ordem).Skip((VS_NumPagina - 1) * VS_QtdRespostasPagina).Take(VS_QtdRespostasPagina);
                    rptRespostasAluno.DataBind();
                }
            }
        }

        protected void rptAgendamentosAnteriores_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Repeater rptRespostasAluno = (Repeater)e.Item.NamingContainer;
                RepeaterItem rptItemLancamento = (RepeaterItem)((Repeater)((RepeaterItem)rptRespostasAluno.NamingContainer).NamingContainer).NamingContainer;
                if (rptItemLancamento != null)
                {
                    HtmlTableCell thAgendamento = (HtmlTableCell)e.Item.FindControl("thAgendamento");
                    HtmlTableCell tdRespostas = (HtmlTableCell)rptItemLancamento.FindControl("tdRespostas");
                    if (thAgendamento != null && tdRespostas != null)
                    {
                        thAgendamento.ColSpan = tdRespostas.ColSpan;
                    }
                }

                long alu_id = Convert.ToInt64(DataBinder.Eval(e.Item.DataItem, "alu_id"));
                long sda_id = Convert.ToInt64(DataBinder.Eval(e.Item.DataItem, "sda_id"));

                Repeater rptRespAgendamentosAnteriores = (Repeater)e.Item.FindControl("rptRespAgendamentosAnteriores");
                if (rptRespAgendamentosAnteriores != null)
                {
                    rptRespAgendamentosAnteriores.DataSource = VS_lstLancamentoTurma.Where(p => p.sda_id == sda_id && p.alu_id == alu_id)
                                                                                    .OrderBy(p => p.sdq_ordem).ThenBy(p => p.sdq_ordemSub).ThenBy(p => p.sdr_ordem).Skip((VS_NumPagina - 1) * VS_QtdRespostasPagina).Take(VS_QtdRespostasPagina)
                                                                                    .GroupBy(a => new { sda_id = a.sda_id, sdq_id = a.sdq_id, sdq_idSub = a.sdq_idSub, sdr_id = a.sdr_id, sdr_descricao = a.sdr_descricao, respAluno = a.respAluno })
                                                                                    .Select(a => new { sda_id = a.Key.sda_id, sdq_id = a.Key.sdq_id, sdq_idSub = a.Key.sdq_idSub, sdr_id = a.Key.sdr_id, sdr_descricao = a.Key.sdr_descricao, respAluno = a.Key.respAluno });
                    rptRespAgendamentosAnteriores.DataBind();
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

        protected void rptRespostasAluno_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                Repeater rptRespostasAluno = (Repeater)e.Item.NamingContainer;
                RepeaterItem rptItemLancamento = (RepeaterItem)rptRespostasAluno.NamingContainer;
                if (rptItemLancamento != null)
                {
                    long alu_id = Convert.ToInt64(DataBinder.Eval(rptItemLancamento.DataItem, "alu_id"));

                    if (VS_lstLancamentoTurma.Where(a => a.sda_id != VS_sda_id && a.alu_id == alu_id).OrderBy(p => p.sdq_ordem).ThenBy(p => p.sdq_ordemSub).ThenBy(p => p.sdr_ordem).Skip((VS_NumPagina - 1) * VS_QtdRespostasPagina).Take(VS_QtdRespostasPagina).Any())
                    {
                        HtmlGenericControl divRespostasAgendamentos = (HtmlGenericControl)e.Item.FindControl("divRespostasAgendamentos");
                        Repeater rptAgendamentosAnteriores = (Repeater)e.Item.FindControl("rptAgendamentosAnteriores");
                        if (divRespostasAgendamentos != null && rptAgendamentosAnteriores != null)
                        {
                            List<ACA_Sondagem_ListAgendamentos> lstAux = new List<ACA_Sondagem_ListAgendamentos>();
                            foreach (int sda_id in VS_lstLancamentoTurma.Where(p => p.alu_id == alu_id && p.sda_id != VS_sda_id).GroupBy(p => p.sda_id).Select(p => p.Key))
                            {
                                lstAux.AddRange(VS_lstLancamentoTurma.Where(p => p.sda_id == sda_id && p.alu_id == alu_id)
                                                                     .OrderBy(p => p.sdq_ordem).ThenBy(p => p.sdq_ordemSub).ThenBy(p => p.sdr_ordem).Skip((VS_NumPagina - 1) * VS_QtdRespostasPagina).Take(VS_QtdRespostasPagina)
                                                                     .ToList().OrderByDescending(a => Convert.ToDateTime(a.dataInicio))
                                                                     .GroupBy(a => new { alu_id = a.alu_id, sda_id = a.sda_id, dataInicio = a.dataInicio, dataAgendamento = a.dataInicio + " - " + a.dataFim })
                                                                     .Select(a => new ACA_Sondagem_ListAgendamentos { alu_id = a.Key.alu_id, sda_id = a.Key.sda_id, dataInicio = a.Key.dataInicio, dataAgendamento = a.Key.dataAgendamento }).ToList());
                            }
                            rptAgendamentosAnteriores.DataSource = lstAux.OrderBy(a => Convert.ToDateTime(a.dataInicio));
                            rptAgendamentosAnteriores.DataBind();

                            divRespostasAgendamentos.Visible = true;
                        }
                    }
                }
            }
            else if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                long alu_id = Convert.ToInt64(DataBinder.Eval(e.Item.DataItem, "alu_id"));
                int sdr_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "sdr_id"));
                int sdq_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "sdq_id"));
                int sdq_idSub = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "sdq_idSub"));
                string estiloBorda = "1px solid #eee";

                RadioButton rbResposta = (RadioButton)e.Item.FindControl("rbResposta");
                if (rbResposta != null)
                {
                    rbResposta.GroupName = string.Format("{0}_{1}_{2}", alu_id, sdq_id, sdq_idSub);

                    if (string.IsNullOrEmpty(ultimoGrupo) || rbResposta.GroupName != ultimoGrupo)
                    {
                        estiloBorda = "1px solid #ccc";
                        ultimoGrupo = rbResposta.GroupName;
                    }

                    rbResposta.Enabled = VS_responder && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                }

                HiddenField hdnRespId = (HiddenField)e.Item.FindControl("hdnRespId");
                if (hdnRespId != null)
                {
                    hdnRespId.Value = string.Format("{0};{1};{2}", sdq_id, sdq_idSub, sdr_id);
                }

                HtmlTableCell tdResposta = (HtmlTableCell)e.Item.FindControl("tdResposta");
                if (tdResposta != null)
                {
                    tdResposta.Style.Add("border-left", estiloBorda);
                }
            }
        }

        protected void lkbAnterior_Click(object sender, EventArgs e)
        {
            VS_NumPagina--;
            CarregarListasAuxiliares();
            GuardarRespostasAlunos();
            RecarregarGrid();
        }

        protected void lkbProximo_Click(object sender, EventArgs e)
        {
            VS_NumPagina++;
            CarregarListasAuxiliares();
            GuardarRespostasAlunos();
            RecarregarGrid();
        }

        #endregion
    }
}