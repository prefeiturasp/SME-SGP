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

        private byte VS_snd_opcaoResposta
        {
            get
            {
                if (ViewState["VS_snd_opcaoResposta"] != null)
                {
                    return Convert.ToByte(ViewState["VS_snd_opcaoResposta"]);
                }
                return 0;
            }
            set
            {
                ViewState["VS_snd_opcaoResposta"] = value;
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
                            VS_snd_opcaoResposta = (byte)PreviousPage.EditItem[2];
                            VS_responder = true;
                        }
                        else
                        {
                            VS_snd_id = PreviousPage.SelectedItem[0];
                            VS_sda_id = PreviousPage.SelectedItem[1];
                            VS_snd_opcaoResposta = (byte)PreviousPage.SelectedItem[2];
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
            if (VS_snd_opcaoResposta == (byte)ACA_SondagemOpcaoResposta.Multiselecao)
            {
                rptLancamentoMulti.DataSource = (
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
                rptLancamentoMulti.DataBind();
            }
            else if (VS_snd_opcaoResposta == (byte)ACA_SondagemOpcaoResposta.SelecaoUnica)
            {
                rptLancamentoUnico.DataSource = (
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
                rptLancamentoUnico.DataBind();
            }
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
            if (VS_snd_opcaoResposta == (byte)ACA_SondagemOpcaoResposta.Multiselecao)
                foreach (RepeaterItem itemAluno in rptLancamentoMulti.Items)
                {
                    long alu_id = Convert.ToInt64(((HiddenField)itemAluno.FindControl("hdnAluId")).Value);
                    Repeater rptRespostasAluno = (Repeater)itemAluno.FindControl("rptRespostasAluno");
                    foreach (RepeaterItem itemResposta in rptRespostasAluno.Items)
                    {
                        CheckBox ckbResposta = (CheckBox)itemResposta.FindControl("ckbResposta");
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
                                VS_lstLancamentoTurma[i].respAluno = ckbResposta.Checked;
                            }
                        }
                    }
                }
            else if (VS_snd_opcaoResposta == (byte)ACA_SondagemOpcaoResposta.SelecaoUnica)
                foreach (RepeaterItem itemAluno in rptLancamentoUnico.Items)
                {
                    long alu_id = Convert.ToInt64(((HiddenField)itemAluno.FindControl("hdnAluId")).Value);
                    Repeater rptRespostasAlunoUnico = (Repeater)itemAluno.FindControl("rptRespostasAlunoUnico");
                    foreach (RepeaterItem itemResposta in rptRespostasAlunoUnico.Items)
                    {
                        DropDownList ddlResposta = (DropDownList)itemResposta.FindControl("ddlResposta");
                        HiddenField hdnRespId = (HiddenField)itemResposta.FindControl("hdnRespId");
                        string[] respId = hdnRespId.Value.Split(';');
                        int sdq_id = Convert.ToInt32(respId[0]);
                        int sdq_idSub = Convert.ToInt32(respId[1]);
                        for (int i = 0; i < VS_lstLancamentoTurma.Count; i++)
                        {
                            ACA_Sondagem_Lancamento lancamento = VS_lstLancamentoTurma[i];
                            if (lancamento.alu_id == alu_id && lancamento.sdq_id == sdq_id && lancamento.sdq_idSub == sdq_idSub)
                            {
                                VS_lstLancamentoTurma[i].respAluno = Convert.ToInt32(ddlResposta.SelectedValue) == lancamento.sdr_id;
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
                        divLancamentoMulti.Visible = VS_snd_opcaoResposta == (byte)ACA_SondagemOpcaoResposta.Multiselecao;
                        divLancamentoUnica.Visible = VS_snd_opcaoResposta == (byte)ACA_SondagemOpcaoResposta.SelecaoUnica;
                        RecarregarGrid();
                    }
                    else
                    {
                        divLancamentoMulti.Visible = divLancamentoUnica.Visible = false;
                        lblResultadoVazio.Visible = true;
                    }
                    btnSalvar.Visible = VS_responder && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar && VS_lstLancamentoTurma.Where(p => p.sda_id == VS_sda_id).Any();
                }
                else
                {
                    divLancamentoMulti.Visible = divLancamentoUnica.Visible = false;
                    lblResultadoVazio.Visible = false;
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
                    Repeater rptQuestoesHistorico = (Repeater)e.Item.FindControl("rptQuestoesHistorico");
                    if (rptQuestoesHistorico != null)
                    {
                        List<Questao> lstQuestoesHist = new List<Questao>();
                        foreach (int sda_id in VS_lstLancamentoTurma.Where(p => p.sda_id != VS_sda_id).OrderBy(p => Convert.ToDateTime(p.dataInicio)).GroupBy(p => p.sda_id).Select(p => p.Key))
                            lstQuestoesHist.AddRange(lstQuestoes.Skip((VS_NumPagina - 1) * VS_QtdQuestoesPagina).Take(VS_QtdQuestoesPagina));

                        rptQuestoesHistorico.DataSource = lstQuestoesHist;
                        rptQuestoesHistorico.DataBind();
                    }
                    Repeater rptQuestoes = (Repeater)e.Item.FindControl("rptQuestoes");
                    if (rptQuestoes != null)
                    {
                        rptQuestoes.DataSource = lstQuestoes.Skip((VS_NumPagina - 1) * VS_QtdQuestoesPagina).Take(VS_QtdQuestoesPagina);
                        rptQuestoes.DataBind();

                        if (VS_snd_opcaoResposta == (byte)ACA_SondagemOpcaoResposta.SelecaoUnica && lstSubQuestoes.Count > 0)
                        {
                            Label lblNomeAluno = (Label)e.Item.FindControl("lblNomeAluno");
                            if (lblNomeAluno != null)
                            {
                                lblNomeAluno.Visible = false;
                            }
                        }
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

                    Repeater rptSubQuestoesHistorico = (Repeater)e.Item.FindControl("rptSubQuestoesHistorico");
                    if (rptSubQuestoesHistorico != null)
                    {
                        List<Questao> lstSubQuestoesHist = new List<Questao>();
                        foreach (int sda_id in VS_lstLancamentoTurma.Where(p => p.sda_id != VS_sda_id).OrderBy(p => Convert.ToDateTime(p.dataInicio)).GroupBy(p => p.sda_id).Select(p => p.Key))
                            lstSubQuestoesHist.AddRange(lstSubQuestoesQuestao.Skip((VS_NumPagina - 1) * VS_QtdSubQuestoesPagina).Take(VS_QtdSubQuestoesPagina));

                        rptSubQuestoesHistorico.DataSource = lstSubQuestoesHist;
                        rptSubQuestoesHistorico.DataBind();
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

                    Repeater rptRespostasHistorico = (Repeater)e.Item.FindControl("rptRespostasHistorico");
                    if (rptRespostasHistorico != null)
                    {
                        List<Resposta> lstRespostasHist = new List<Resposta>();
                        foreach (int sda_id in VS_lstLancamentoTurma.Where(p => p.sda_id != VS_sda_id).OrderBy(p => Convert.ToDateTime(p.dataInicio)).GroupBy(p => p.sda_id).Select(p => p.Key))
                            lstRespostasHist.AddRange(lstRespostasQuestao.Skip((VS_NumPagina - 1) * VS_QtdRespostasPagina).Take(VS_QtdRespostasPagina));

                        rptRespostasHistorico.DataSource = lstRespostasHist;
                        rptRespostasHistorico.DataBind();
                    }
                    Repeater rptRespostas = (Repeater)e.Item.FindControl("rptRespostas");
                    if (rptRespostas != null)
                    {
                        rptRespostas.DataSource = lstRespostasQuestao.Skip((VS_NumPagina - 1) * VS_QtdRespostasPagina).Take(VS_QtdRespostasPagina);
                        rptRespostas.DataBind();
                    }
                }

                Repeater rptAgendamentos = (Repeater)e.Item.FindControl("rptAgendamentos");
                if (rptAgendamentos != null)
                {
                    rptAgendamentos.DataSource = VS_lstLancamentoTurma.GroupBy(p => new { sda_id = p.sda_id, dataInicio = p.dataInicio, dataFim = p.dataFim })
                                                                      .Select(p => new { sda_id = p.Key.sda_id, dataInicio = p.Key.dataInicio, dataFim = p.Key.dataFim, periodo = p.Key.dataInicio + " - " + p.Key.dataFim })
                                                                      .OrderBy(p => Convert.ToDateTime(p.dataInicio));
                    rptAgendamentos.DataBind();
                }

                HtmlTableCell thBotoes = (HtmlTableCell)e.Item.FindControl("thBotoes");
                if (thBotoes != null)
                {
                    thBotoes.Visible = VS_TotalPaginas > 1;
                    if (thBotoes.Visible)
                    {
                        if (VS_snd_opcaoResposta == (byte)ACA_SondagemOpcaoResposta.Multiselecao)
                            thBotoes.Attributes.Add("colspan", (VS_QtdRespostasPagina * VS_lstLancamentoTurma.GroupBy(p => p.sda_id).Count()).ToString());
                        else if (VS_snd_opcaoResposta == (byte)ACA_SondagemOpcaoResposta.SelecaoUnica)
                            thBotoes.Attributes.Add("colspan", ((VS_QtdSubQuestoesPagina > 0 ? VS_QtdSubQuestoesPagina : VS_QtdQuestoesPagina) * VS_lstLancamentoTurma.GroupBy(p => p.sda_id).Count()).ToString());
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

                if (VS_snd_opcaoResposta == (byte)ACA_SondagemOpcaoResposta.Multiselecao)
                {
                    Repeater rptRespostasAlunoHistorico = (Repeater)e.Item.FindControl("rptRespostasAlunoHistorico");
                    if (rptRespostasAlunoHistorico != null)
                    {
                        List<ACA_Sondagem_Lancamento> lstRespAlunoHist = new List<ACA_Sondagem_Lancamento>();
                        foreach (int sda_id in VS_lstLancamentoTurma.Where(p => p.sda_id != VS_sda_id).OrderBy(p => Convert.ToDateTime(p.dataInicio)).GroupBy(p => p.sda_id).Select(p => p.Key))
                        {
                            lstRespAlunoHist.AddRange((
                            from ACA_Sondagem_Lancamento dr in VS_lstLancamentoTurma.Where(p => p.sda_id == sda_id && p.alu_id == alu_id)
                            select new ACA_Sondagem_Lancamento
                            {
                                sda_id = dr.sda_id
                                ,
                                alu_id = dr.alu_id
                                ,
                                sdr_id = dr.sdr_id
                                ,
                                sdr_descricao = dr.sdr_descricao
                                ,
                                sdr_ordem = dr.sdr_ordem
                                ,
                                sdq_idSub = dr.sdq_idSub
                                ,
                                sdq_ordemSub = dr.sdq_ordemSub
                                ,
                                sdq_id = dr.sdq_id
                                ,
                                sdq_ordem = dr.sdq_ordem
                                ,
                                respAluno = dr.respAluno
                                ,
                                dataInicio = dr.dataInicio
                            }
                            ).ToList().OrderBy(p => p.sdq_ordem).ThenBy(p => p.sdq_ordemSub).ThenBy(p => p.sdr_ordem).Skip((VS_NumPagina - 1) * VS_QtdRespostasPagina).Take(VS_QtdRespostasPagina).ToList());
                        }
                        rptRespostasAlunoHistorico.DataSource = lstRespAlunoHist;
                        rptRespostasAlunoHistorico.DataBind();
                    }

                    Repeater rptRespostasAluno = (Repeater)e.Item.FindControl("rptRespostasAluno");
                    if (rptRespostasAluno != null)
                    {
                        rptRespostasAluno.DataSource = (
                            from ACA_Sondagem_Lancamento dr in VS_lstLancamentoTurma.Where(p => p.sda_id == VS_sda_id && p.alu_id == alu_id)
                            select new
                            {
                                sda_id = dr.sda_id
                                ,
                                alu_id = dr.alu_id
                                ,
                                sdr_id = dr.sdr_id
                                ,
                                sdr_descricao = dr.sdr_descricao
                                ,
                                sdr_ordem = dr.sdr_ordem
                                ,
                                sdq_idSub = dr.sdq_idSub
                                ,
                                sdq_ordemSub = dr.sdq_ordemSub
                                ,
                                sdq_id = dr.sdq_id
                                ,
                                sdq_ordem = dr.sdq_ordem
                                ,
                                respAluno = dr.respAluno
                                ,
                                dataInicio = dr.dataInicio
                            }
                        ).ToList().OrderBy(p => p.sdq_ordem).ThenBy(p => p.sdq_ordemSub).ThenBy(p => p.sdr_ordem).Skip((VS_NumPagina - 1) * VS_QtdRespostasPagina).Take(VS_QtdRespostasPagina);
                        rptRespostasAluno.DataBind();
                    }
                }
                else if (VS_snd_opcaoResposta == (byte)ACA_SondagemOpcaoResposta.SelecaoUnica)
                {
                    Repeater rptRespostasAlunoUnicoHistorico = (Repeater)e.Item.FindControl("rptRespostasAlunoUnicoHistorico");
                    if (rptRespostasAlunoUnicoHistorico != null)
                    {
                        List<ACA_Sondagem_Lancamento> lstRespAlunoHist = new List<ACA_Sondagem_Lancamento>();
                        foreach (int sda_id in VS_lstLancamentoTurma.Where(p => p.sda_id != VS_sda_id).OrderBy(p => Convert.ToDateTime(p.dataInicio)).GroupBy(p => p.sda_id).Select(p => p.Key))
                        {
                            List<ACA_Sondagem_Lancamento> lstRespAluno = new List<ACA_Sondagem_Lancamento>();
                            lstRespAluno = VS_lstLancamentoTurma.Where(p => p.sda_id == sda_id && p.alu_id == alu_id)
                                                                .GroupBy(p => new
                                                                {
                                                                    sda_id = p.sda_id,
                                                                    alu_id = p.alu_id,
                                                                    sdq_idSub = p.sdq_idSub,
                                                                    sdq_ordemSub = p.sdq_ordemSub,
                                                                    sdq_id = p.sdq_id,
                                                                    sdq_ordem = p.sdq_ordem,
                                                                    dataInicio = p.dataInicio,
                                                                })
                                                                .Select(p => new ACA_Sondagem_Lancamento
                                                                {
                                                                    sda_id = p.Key.sda_id,
                                                                    alu_id = p.Key.alu_id,
                                                                    sdq_idSub = p.Key.sdq_idSub,
                                                                    sdq_ordemSub = p.Key.sdq_ordemSub,
                                                                    sdq_id = p.Key.sdq_id,
                                                                    sdq_ordem = p.Key.sdq_ordem,
                                                                    dataInicio = p.Key.dataInicio
                                                                })
                                                                .ToList();
                            lstRespAluno = lstRespAluno.OrderBy(p => p.sdq_ordem).ThenBy(p => p.sdq_ordemSub).Skip((VS_NumPagina - 1) * (VS_QtdSubQuestoesPagina > 0 ? (VS_QtdQuestoesPagina > 0 ? VS_QtdQuestoesPagina : 1) * VS_QtdSubQuestoesPagina : (VS_QtdQuestoesPagina > 0 ? VS_QtdQuestoesPagina : 1))).Take((VS_QtdSubQuestoesPagina > 0 ? (VS_QtdQuestoesPagina > 0 ? VS_QtdQuestoesPagina : 1) * VS_QtdSubQuestoesPagina : (VS_QtdQuestoesPagina > 0 ? VS_QtdQuestoesPagina : 1))).ToList();
                            lstRespAlunoHist.AddRange((
                            from ACA_Sondagem_Lancamento dr in lstRespAluno
                            select new ACA_Sondagem_Lancamento
                            {
                                sda_id = dr.sda_id
                                ,
                                alu_id = dr.alu_id
                                ,
                                sdq_idSub = dr.sdq_idSub
                                ,
                                sdq_ordemSub = dr.sdq_ordemSub
                                ,
                                sdq_id = dr.sdq_id
                                ,
                                sdr_id = (VS_lstLancamentoTurma.Any(r => r.sda_id == dr.sda_id && r.alu_id == dr.alu_id && r.sdq_id == dr.sdq_id && r.sdq_idSub == dr.sdq_idSub && r.respAluno) ?
                                          VS_lstLancamentoTurma.Where(r => r.sda_id == dr.sda_id && r.alu_id == dr.alu_id && r.sdq_id == dr.sdq_id && r.sdq_idSub == dr.sdq_idSub && r.respAluno).First().sdr_id : 0)
                                ,
                                sdq_ordem = dr.sdq_ordem
                                ,
                                dataInicio = dr.dataInicio
                            }
                            ).ToList());
                        }
                        rptRespostasAlunoUnicoHistorico.DataSource = lstRespAlunoHist;
                        rptRespostasAlunoUnicoHistorico.DataBind();
                    }

                    Repeater rptRespostasAlunoUnico = (Repeater)e.Item.FindControl("rptRespostasAlunoUnico");
                    if (rptRespostasAlunoUnico != null)
                    {
                        List<ACA_Sondagem_Lancamento> lstRespAluno = new List<ACA_Sondagem_Lancamento>();
                        lstRespAluno = VS_lstLancamentoTurma.Where(p => p.sda_id == VS_sda_id && p.alu_id == alu_id)
                                                            .GroupBy(p => new
                                                            {
                                                                sda_id = p.sda_id,
                                                                alu_id = p.alu_id,
                                                                sdq_idSub = p.sdq_idSub,
                                                                sdq_ordemSub = p.sdq_ordemSub,
                                                                sdq_id = p.sdq_id,
                                                                sdq_ordem = p.sdq_ordem,
                                                                dataInicio = p.dataInicio
                                                            })
                                                            .Select(p => new ACA_Sondagem_Lancamento
                                                            {
                                                                sda_id = p.Key.sda_id,
                                                                alu_id = p.Key.alu_id,
                                                                sdq_idSub = p.Key.sdq_idSub,
                                                                sdq_ordemSub = p.Key.sdq_ordemSub,
                                                                sdq_id = p.Key.sdq_id,
                                                                sdq_ordem = p.Key.sdq_ordem,
                                                                dataInicio = p.Key.dataInicio
                                                            })
                                                            .ToList();
                        lstRespAluno = lstRespAluno.OrderBy(p => p.sdq_ordem).ThenBy(p => p.sdq_ordemSub).Skip((VS_NumPagina - 1) * (VS_QtdSubQuestoesPagina > 0 ? (VS_QtdQuestoesPagina > 0 ? VS_QtdQuestoesPagina : 1) * VS_QtdSubQuestoesPagina : (VS_QtdQuestoesPagina > 0 ? VS_QtdQuestoesPagina : 1))).Take((VS_QtdSubQuestoesPagina > 0 ? (VS_QtdQuestoesPagina > 0 ? VS_QtdQuestoesPagina : 1) * VS_QtdSubQuestoesPagina : (VS_QtdQuestoesPagina > 0 ? VS_QtdQuestoesPagina : 1))).ToList();
                        rptRespostasAlunoUnico.DataSource = (
                            from ACA_Sondagem_Lancamento dr in lstRespAluno
                            select new
                            {
                                sda_id = dr.sda_id
                                ,
                                alu_id = dr.alu_id
                                ,
                                sdq_idSub = dr.sdq_idSub
                                ,
                                sdq_ordemSub = dr.sdq_ordemSub
                                ,
                                sdq_id = dr.sdq_id
                                ,
                                sdr_id = (VS_lstLancamentoTurma.Any(r => r.sda_id == dr.sda_id && r.alu_id == dr.alu_id && r.sdq_id == dr.sdq_id && r.sdq_idSub == dr.sdq_idSub && r.respAluno) ?
                                          VS_lstLancamentoTurma.Where(r => r.sda_id == dr.sda_id && r.alu_id == dr.alu_id && r.sdq_id == dr.sdq_id && r.sdq_idSub == dr.sdq_idSub && r.respAluno).First().sdr_id : 0)
                                ,
                                sdq_ordem = dr.sdq_ordem
                                ,
                                dataInicio = dr.dataInicio
                            }
                        );
                        rptRespostasAlunoUnico.DataBind();
                    }
                }
            }
        }

        protected void rptAgendamentos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlTableCell thAgendamento = (HtmlTableCell)e.Item.FindControl("thAgendamento");
                if (thAgendamento != null)
                {
                    int numQuestoes = lstQuestoes.Count() > 0 ? lstQuestoes.Count() : 1;
                    int numSubQuestoes = lstSubQuestoes.Count() > 0 ? lstSubQuestoes.Count() : 1;
                    int span; 
                    if (VS_snd_opcaoResposta == (byte)ACA_SondagemOpcaoResposta.Multiselecao)
                    {
                        int numRespostas = lstRespostas.Count() > 0 ? lstRespostas.Count() : 1;
                        span = numQuestoes * numSubQuestoes * numRespostas;
                        thAgendamento.Attributes.Add("colspan", VS_QtdRespostasPagina < span ? VS_QtdRespostasPagina.ToString() : span.ToString());
                    }                  
                    else if (VS_snd_opcaoResposta == (byte)ACA_SondagemOpcaoResposta.SelecaoUnica)
                    {
                        int qtdPagina = VS_QtdSubQuestoesPagina > 0 ? VS_QtdSubQuestoesPagina * VS_QtdQuestoesPagina : VS_QtdQuestoesPagina;
                        span = numQuestoes * numSubQuestoes;
                        thAgendamento.Attributes.Add("colspan", qtdPagina < span ? qtdPagina.ToString() : span.ToString());
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
                    if (VS_snd_opcaoResposta == (byte)ACA_SondagemOpcaoResposta.Multiselecao)
                        thQuestao.Attributes.Add("colspan", (numSubQuestoes * lstRespostas.Count()).ToString());
                    else if (VS_snd_opcaoResposta == (byte)ACA_SondagemOpcaoResposta.SelecaoUnica)
                        thQuestao.Attributes.Add("colspan", numSubQuestoes.ToString());
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
                    if (VS_snd_opcaoResposta == (byte)ACA_SondagemOpcaoResposta.Multiselecao)
                        thSubQuestao.Attributes.Add("colspan", lstRespostas.Count().ToString());
                    else if (VS_snd_opcaoResposta == (byte)ACA_SondagemOpcaoResposta.SelecaoUnica)
                        thSubQuestao.Attributes.Add("colspan", 1.ToString());
                }
            }
        }

        protected void rptRespostasAluno_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int sda_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "sda_id"));
                long alu_id = Convert.ToInt64(DataBinder.Eval(e.Item.DataItem, "alu_id"));
                int sdr_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "sdr_id"));
                int sdq_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "sdq_id"));
                int sdq_idSub = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "sdq_idSub"));
                string estiloBorda = "1px solid #eee";

                CheckBox ckbResposta = (CheckBox)e.Item.FindControl("ckbResposta");
                HiddenField groupName = (HiddenField)e.Item.FindControl("groupName");

                if (ckbResposta != null && groupName != null)
                {
                    groupName.Value = string.Format("{0}_{1}_{2}_{3}", alu_id, sda_id, sdq_id, sdq_idSub);

                    if (string.IsNullOrEmpty(ultimoGrupo) || groupName.Value != ultimoGrupo)
                    {
                        estiloBorda = "1px solid #ccc";
                        ultimoGrupo = groupName.Value;
                    }

                    ckbResposta.Enabled = sda_id == VS_sda_id && VS_responder && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
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

        protected void rptRespostasAlunoUnico_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int sda_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "sda_id"));
                long alu_id = Convert.ToInt64(DataBinder.Eval(e.Item.DataItem, "alu_id"));
                int sdr_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "sdr_id"));
                int sdq_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "sdq_id"));
                int sdq_idSub = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "sdq_idSub"));
                string estiloBorda = "1px solid #eee";

                HiddenField hdnRespId = (HiddenField)e.Item.FindControl("hdnRespId");
                if (hdnRespId != null)
                {
                    hdnRespId.Value = string.Format("{0};{1};{2}", sdq_id, sdq_idSub, sdr_id);
                }
                HiddenField groupName = (HiddenField)e.Item.FindControl("groupName");

                DropDownList ddlResposta = (DropDownList)e.Item.FindControl("ddlResposta");
                if (ddlResposta != null && groupName != null)
                {
                    if (ddlResposta.Items.Count <= 0)
                    {
                        ddlResposta.DataSource = VS_lstLancamentoTurma.Where(p => p.sda_id == VS_sda_id)
                                                                      .GroupBy(p => new { sdr_id = p.sdr_id, sdr_descricao = p.sdr_descricao, sdr_ordem = p.sdr_ordem })
                                                                      .Select(p => new { sdr_id = p.Key.sdr_id, sdr_descricao = p.Key.sdr_descricao, sdr_ordem = p.Key.sdr_ordem })
                                                                      .OrderBy(p => p.sdr_ordem).ThenBy(p => p.sdr_descricao);
                        ddlResposta.DataBind();
                        ddlResposta.Items.Add(new ListItem("-", "0"));
                        ddlResposta.SelectedValue = "0";
                    }

                    string[] respId = hdnRespId.Value.Split(';');
                    int sdr_idResp = Convert.ToInt32(respId[2]);
                    if (ddlResposta.Items.FindByValue(sdr_idResp.ToString()) != null)
                        ddlResposta.SelectedValue = sdr_idResp.ToString();

                    groupName.Value = string.Format("{0}_{1}_{2}_{3}", alu_id, sda_id, sdq_id, sdq_idSub);

                    if (string.IsNullOrEmpty(ultimoGrupo) || groupName.Value != ultimoGrupo)
                    {
                        estiloBorda = "1px solid #ccc";
                        ultimoGrupo = groupName.Value;
                    }

                    ddlResposta.Enabled = sda_id == VS_sda_id && VS_responder && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
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