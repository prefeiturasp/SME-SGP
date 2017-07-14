using System;
using System.Linq;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.CoreSSO.BLL;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using MSTech.Validation.Exceptions;
using System.Web.UI;
using System.Collections.Generic;
using System.IO;
using MSTech.CoreSSO.Entities;

namespace GestaoEscolar.Configuracao.GraficoAtendimento
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
        /// Propriedade em ViewState que armazena valor de gra_id
        /// no caso de atualização de um registro ja existente.
        /// </summary>
        private int VS_gra_id
        {
            get
            {
                if (ViewState["VS_gra_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_gra_id"]);
                }
                return -1;
            }
            set
            {
                ViewState["VS_gra_id"] = value;
            }
        }

        /// <summary>
        /// Propriedade em ViewState que armazena valor de rea_id.
        /// </summary>
        private int VS_rea_id
        {
            get
            {
                if (ViewState["VS_rea_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_rea_id"]);
                }
                return -1;
            }
            set
            {
                ViewState["VS_rea_id"] = value;
            }
        }

        private List<QuestionarioConteudoResposta> VS_listQuestionarioConteudoResposta
        {
            get
            {
                if (ViewState["VS_listQuestionarioConteudoResposta"] == null)
                    ViewState["VS_listQuestionarioConteudoResposta"] = new List<QuestionarioConteudoResposta>();

                return (List<QuestionarioConteudoResposta>)ViewState["VS_listQuestionarioConteudoResposta"];
            }
            set
            {
                ViewState["VS_listQuestionarioConteudoResposta"] = value;
            }
        }

        private List<REL_GraficoAtendimento_FiltrosFixos> VS_lstFiltrosFixos
        {
            get
            {
                if (ViewState["VS_lstFiltrosFixos"] == null)
                    ViewState["VS_lstFiltrosFixos"] = new List<REL_GraficoAtendimento_FiltrosFixos>();

                return (List<REL_GraficoAtendimento_FiltrosFixos>)ViewState["VS_lstFiltrosFixos"];
            }
            set
            {
                ViewState["VS_lstFiltrosFixos"] = value;
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Carrega dados do relatório
        /// </summary>
        /// <param name="rea_id">ID do relatório</param>
        private void _LoadFromEntity(int gra_id)
        {
            try
            {
                VS_gra_id = gra_id;

                REL_GraficoAtendimento gra = new REL_GraficoAtendimento { gra_id = VS_gra_id };
                REL_GraficoAtendimentoBO.GetEntity(gra);

                txtTitulo.Text = gra.gra_titulo;
                ddlTipo.Enabled = false;

                VS_rea_id = gra.rea_id;

                CLS_RelatorioAtendimento rea = new CLS_RelatorioAtendimento { rea_id = gra.rea_id };
                CLS_RelatorioAtendimentoBO.GetEntity(rea);
                ddlTipo.SelectedValue = rea.rea_tipo.ToString();
                ddlTipo_SelectedIndexChanged(null, null);
                UCComboRelatorioAtendimento.Valor = gra.rea_id;
                UCComboRelatorioAtendimento_SelectedIndexChanged();
                UCComboRelatorioAtendimento.PermiteEditar = false;

                ddlTipoGrafico.SelectedValue = gra.gra_tipo.ToString();
                ddlTipoGrafico.Enabled = false;

                ddlEixoAgrupamento.SelectedValue = gra.gra_eixo.ToString();
                ddlEixoAgrupamento.Enabled = false;

                CarregaFiltrosFixos();
                CarregaQuestionarios();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.ErroCarregarRelatorio").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Insere ou altera o relatório
        /// </summary>
        private void Salvar()
        {
            try
            {
                REL_GraficoAtendimentoTipo tipoGrafico;
                Enum.TryParse(ddlTipoGrafico.SelectedValue, out tipoGrafico);

                if (REL_GraficoAtendimentoBO.GetBy_titulo(txtTitulo.Text).AsEnumerable().Any(g => Convert.ToInt32(g["gra_id"]) != VS_gra_id))
                    throw new ValidationException("Já existe um gráfico de atendimento cadastrado com este título.");

                REL_GraficoAtendimento gra = new REL_GraficoAtendimento
                {
                    gra_id = VS_gra_id,
                    rea_id = UCComboRelatorioAtendimento.Valor,
                    gra_titulo = txtTitulo.Text,
                    gra_eixo = Convert.ToByte(ddlEixoAgrupamento.SelectedValue),
                    gra_tipo = Convert.ToByte(tipoGrafico),
                    gra_dataAlteracao = DateTime.Now,
                    IsNew = VS_gra_id <= 0
                };

                if (gra.IsNew)
                    gra.gra_dataCriacao = DateTime.Now;

                if (VS_lstFiltrosFixos.Count == 0 && VS_listQuestionarioConteudoResposta.Count == 0)
                    throw new ValidationException("Adicione pelo menos um filtro fixo ou personalizado.");

                if (REL_GraficoAtendimentoBO.Salvar(gra, VS_lstFiltrosFixos, VS_listQuestionarioConteudoResposta))
                {
                    string message = "";
                    if (VS_gra_id <= 0)
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "gra_id: " + gra.gra_id);
                        message = UtilBO.GetErroMessage("Gráfico cadastrado com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }
                    else
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "gra_id: " + gra.gra_id);
                        message = UtilBO.GetErroMessage("Gráfico alterado com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }
                    if (ParametroPermanecerTela)
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                        lblMessage.Text = message;
                        VS_gra_id = gra.gra_id;
                        _LoadFromEntity(VS_gra_id);
                    }
                    else
                    {
                        __SessionWEB.PostMessages = message;
                        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Configuracao/RelatorioAtendimento/Busca.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.ErroSalvarRelatorio").ToString(), UtilBO.TipoMensagem.Erro);
                }
            }
            catch (ValidationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (DuplicateNameException ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (ArgumentException ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.ErroSalvarRelatorio").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }


        /// <summary>
        /// Carrega os cargos
        /// </summary>
        private void CarregaFiltrosFixos()
        {
            VS_lstFiltrosFixos = REL_GraficoAtendimento_FiltrosFixosBO.SelectBy_gra_id(VS_gra_id);

            gvFiltroFixo.DataSource = VS_lstFiltrosFixos.OrderBy(f => f.gff_tituloFiltro);
            gvFiltroFixo.DataBind();
        }

        /// <summary>
        /// Carrega os questionarios
        /// </summary>
        private void CarregaQuestionarios()
        {

            List<REL_GraficoAtendimento_FiltrosPersonalizados> lstFiltrosPersonalizados = REL_GraficoAtendimento_FiltrosPersonalizadosBO.SelectBy_gra_id(VS_gra_id);

            VS_listQuestionarioConteudoResposta = new List<QuestionarioConteudoResposta>();
            QuestionarioConteudoResposta qcr;
            foreach (REL_GraficoAtendimento_FiltrosPersonalizados gfp in lstFiltrosPersonalizados)
            {
                qcr = new QuestionarioConteudoResposta
                {
                    qtr_id = gfp.qtr_id
                };
                qcr = CLS_QuestionarioRespostaBO.GetEntityQuestionarioConteudoResposta(qcr);
                qcr.IsNew = gfp.IsNew;
                VS_listQuestionarioConteudoResposta.Add(qcr);
            }

            gvQuestionario.DataSource = VS_listQuestionarioConteudoResposta.OrderBy(f => f.qst_titulo).ThenBy(f => f.qtc_texto).ThenBy(f => f.qtr_texto);
            gvQuestionario.DataBind();
        }

        /// <summary>
        /// Carrega os cargos
        /// </summary>
        private List<CFG_DeficienciaDetalhe> CarregaDetalhePreenchidos()
        {
            List<CFG_DeficienciaDetalhe> lstDetalhes = new List<CFG_DeficienciaDetalhe>();

            foreach (ListItem item in cklDetalhes.Items)
            {

                if (item.Selected)

                    lstDetalhes.Add(CFG_DeficienciaDetalheBO.GetEntity(new CFG_DeficienciaDetalhe
                    {
                        tde_id = ComboTipoDeficiencia.Valor,
                        dfd_id = Convert.ToInt32(item.Value),
                    }));

            }

            return lstDetalhes;
        }

        private string RetornaValorFiltroFixo(int valor)
        {

            string retorno = string.Empty;
            List<string> valores = new List<string>();
            if (valor > 0)
            {
                switch (valor)
                {
                    case 1:
                        valores.Add(txtDtInicial.Text);
                        valores.Add(txtDtFinal.Text);
                        break;
                    case 2:
                        valores.Add(UCComboRacaCor.Valor.ToString());
                        break;
                    case 3:
                        valores.Add(txtIdadeInicial.Text);
                        valores.Add(txtIdadeFinal.Text);
                        break;
                    case 4:
                        valores.Add(UCComboSexo.Valor.ToString());
                        break;
                    default:
                        PES_TipoDeficiencia deficiencia = PES_TipoDeficienciaBO.GetEntity(new PES_TipoDeficiencia { tde_id = ComboTipoDeficiencia.Valor });
                        List<CFG_DeficienciaDetalhe> detalhes = CarregaDetalhePreenchidos();

                        valores = detalhes.Select(x => x.dfd_id.ToString()).ToList();

                        break;
                }

                retorno = string.Join(",", valores.ToArray());
            }
            return retorno;
        }

        private string RetornaValorDetalhadoFiltroFixo(int valor)
        {

            string retorno = string.Empty;
            string[] vetor = { };
            if (valor > 0)
            {
                switch (valor)
                {
                    case 1:
                        vetor[0] = txtDtInicial.Text;
                        vetor[1] = txtDtFinal.Text;
                        break;
                    case 2:
                        vetor[0] = UCComboRacaCor._Combo.SelectedItem.Text;
                        break;
                    case 3:
                        vetor[0] = txtIdadeInicial.Text;
                        vetor[1] = txtIdadeFinal.Text;
                        break;
                    case 4:
                        vetor[0] = UCComboSexo._Combo.SelectedItem.Text;
                        break;
                    default:
                        PES_TipoDeficiencia deficiencia = PES_TipoDeficienciaBO.GetEntity(new PES_TipoDeficiencia { tde_id = ComboTipoDeficiencia.Valor });
                        List<CFG_DeficienciaDetalhe> detalhes = CarregaDetalhePreenchidos();

                        vetor = detalhes.Select(x => x.dfd_nome.ToString()).ToArray();

                        break;
                }

                retorno = string.Join(",", vetor);
            }
            return retorno;
        }


        /// <summary>
        /// Inicializa os campos da tela
        /// </summary>
        private void Inicializar()
        {
            VS_gra_id = -1;
            txtTitulo.Text = "";
            ddlTipo.SelectedValue = "0";
            ddlTipoGrafico.SelectedIndex = ddlTipoGrafico.Items.Count == 2 ? 1 : 0;
            UCComboRelatorioAtendimento.PermiteEditar = false;
            ddlEixoAgrupamento.SelectedValue = "0";
            ComboTipoDeficiencia.ExibeDeficienciaMultipla = false;
            gvQuestionario.DataSource = VS_listQuestionarioConteudoResposta.OrderBy(f => f.qst_titulo).ThenBy(f => f.qtc_texto).ThenBy(f => f.qtr_texto);
            gvQuestionario.DataBind();
        }

        private void ValidaCamposFiltroFixo(int valor)
        {
            if (valor > 0)
            {
                switch (valor)
                {
                    case 1:
                        if (String.IsNullOrEmpty(txtDtInicial.Text))
                            throw new ValidationException("Data inicial é obrigatório.");
                        if (String.IsNullOrEmpty(txtDtFinal.Text))
                            throw new ValidationException("Data final é obrigatório.");
                        DateTime dtInicial, dtFinal;
                        if (DateTime.TryParse(txtDtInicial.Text, out dtInicial) && DateTime.TryParse(txtDtFinal.Text, out dtFinal))
                        {
                            if (dtInicial >= dtFinal)
                                throw new ValidationException("Data inicial deve ser menor que a data final.");
                        }
                        else
                            throw new ValidationException("Data inválida.");
                        break;
                    case 2:
                        if (UCComboRacaCor.Valor <= 0)
                            throw new ValidationException("Raça/Cor é obrigatório.");
                        break;
                    case 3:
                        if (String.IsNullOrEmpty(txtIdadeInicial.Text))
                            throw new ValidationException("Idade mínima é obrigatório.");
                        if (String.IsNullOrEmpty(txtIdadeFinal.Text))
                            throw new ValidationException("Idade máxima é obrigatório.");
                        int idMin, idMax;
                        if (Int32.TryParse(txtIdadeInicial.Text, out idMin) && Int32.TryParse(txtIdadeFinal.Text, out idMax))
                        {
                            if (idMin >= idMax)
                                throw new ValidationException("Idade mínima deve ser menor que idade máxima.");
                        }
                        else
                            throw new ValidationException("Idade inválida.");
                        break;
                    case 4:
                        if (UCComboSexo.Valor <= 0)
                            throw new ValidationException("Sexo é obrigatório.");
                        break;
                    default:
                        if (ComboTipoDeficiencia.Valor == Guid.Empty)
                            throw new ValidationException("Tipo de deficiência é obrigatório.");
                        PES_TipoDeficiencia deficiencia = PES_TipoDeficienciaBO.GetEntity(new PES_TipoDeficiencia { tde_id = ComboTipoDeficiencia.Valor });
                        List<CFG_DeficienciaDetalhe> detalhes = CarregaDetalhePreenchidos();

                        if (detalhes.Select(x => x.dfd_id.ToString()).ToList().Count <= 0)
                            throw new ValidationException("É obrigatório selecionar pelo menos um detalhamento.");

                        break;
                }
            }
        }

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
            }

            ComboTipoDeficiencia.OnSeletedIndexChanged += ComboTipoDeficiencia_SelectedIndexChanged;
            UCComboRelatorioAtendimento.IndexChanged += UCComboRelatorioAtendimento_SelectedIndexChanged;
            UCComboQuestionario.IndexChanged += UCComboQuestionario_SelectedIndexChanged;

            if (!IsPostBack)
            {
                try
                {
                    Inicializar();

                    if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                    {
                        bntSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                        btnCancelar.Text = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar ?
                                           GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.btnCancelar.Text").ToString() :
                                           GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.btnVoltar.Text").ToString();

                        _LoadFromEntity(PreviousPage.EditItem);
                    }
                    else
                    {
                        bntSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
                        btnCancelar.Text = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir ?
                                           GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.btnCancelar.Text").ToString() :
                                           GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.btnVoltar.Text").ToString();
                        ckbBloqueado.Visible = false;

                        CarregaFiltrosFixos();
                        CarregaQuestionarios();
                    }


                    Page.Form.DefaultFocus = txtTitulo.ClientID;
                    Page.Form.DefaultButton = bntSalvar.UniqueID;
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao carregar dados do gráfico.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void bntSalvar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
                Salvar();
        }

        protected void ddlTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(ddlTipo.SelectedValue) > 0)
                {
                    UCComboRelatorioAtendimento.CarregarPorPermissaoUuarioTipo((CLS_RelatorioAtendimentoTipo)Convert.ToByte(ddlTipo.SelectedValue));
                    UCComboRelatorioAtendimento.PermiteEditar = true;
                    if (Convert.ToByte(ddlTipo.SelectedValue) == (byte)CLS_RelatorioAtendimentoTipo.AEE)
                        ddlFiltroFixo.Items.Add(new ListItem("Detalhamento das deficiências", "5"));
                    else
                        ddlFiltroFixo.Items.Remove(new ListItem("Detalhamento das deficiências", "5"));

                    updFiltro.Update();
                }
                else
                {
                    UCComboRelatorioAtendimento.PermiteEditar = false;
                }
                UCComboRelatorioAtendimento.SelectedIndex = 0;
                UCComboRelatorioAtendimento_SelectedIndexChanged();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao carregar relatórios.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void ComboTipoDeficiencia_SelectedIndexChanged()
        {
            cklDetalhes.DataSource = CFG_DeficienciaDetalheBO.SelectDetalheBy_Deficiencia(ComboTipoDeficiencia.Valor);
            cklDetalhes.DataBind();
            divDetalhes.Visible = true;
        }

        protected void UCComboRelatorioAtendimento_SelectedIndexChanged()
        {
            try
            {
                if (UCComboRelatorioAtendimento.Valor > 0)
                {
                    VS_rea_id = UCComboRelatorioAtendimento.Valor;
                    UCComboQuestionario.CarregarQuestionarioComPerguntaMultiplaEscolhaBy_rea_id(VS_rea_id);
                    UCComboQuestionario.PermiteEditar = true;
                }
                else
                {
                    UCComboQuestionario.PermiteEditar = false;
                }
                UCComboQuestionario.SelectedIndex = 0;
                UCComboQuestionario_SelectedIndexChanged();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao carregar questionários.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void UCComboQuestionario_SelectedIndexChanged()
        {
            try
            {
                if (UCComboQuestionario.Valor > 0)
                {
                    ddlPergunta.Items.Clear();
                    ddlPergunta.DataSource = CLS_QuestionarioConteudoBO.SelectConteudoRespostaByQuestionario(UCComboQuestionario.Valor, (byte)QuestionarioTipoConteudo.Pergunta, (byte)QuestionarioTipoResposta.MultiplaSelecao);
                    ddlPergunta.DataTextField = "qtc_texto";
                    ddlPergunta.DataValueField = "qtc_id";
                    ddlPergunta.Items.Insert(0, new ListItem("-- Selecione uma pergunta --", "-1", true));
                    ddlPergunta.AppendDataBoundItems = true;
                    ddlPergunta.DataBind();
                    ddlPergunta.Enabled = true;
                }
                else
                {
                    ddlPergunta.Enabled = false;
                }

                ddlPergunta.SelectedIndex = 0;
                ddlPergunta_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao carregar perguntas.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void ddlFiltroFixo_SelectedIndexChanged(object sender, EventArgs e)
        {
            divBotoesFiltro.Visible = divRacaCor.Visible = divSexo.Visible = divIdade.Visible = divDataPreenchimento.Visible = divDetalhamentoDeficiencia.Visible = false;

            if (Convert.ToInt32(ddlFiltroFixo.SelectedValue) > 0)
            {
                switch (Convert.ToInt32(ddlFiltroFixo.SelectedValue))
                {
                    case 1:
                        divDataPreenchimento.Visible = true;
                        break;
                    case 2:
                        divRacaCor.Visible = true;
                        break;
                    case 3:
                        divIdade.Visible = true;
                        break;
                    case 4:
                        divSexo.Visible = true;
                        break;
                    default:
                        ComboTipoDeficiencia.Carregar();
                        divDetalhes.Visible = false;
                        divDetalhamentoDeficiencia.Visible = true;
                        break;
                }
                divBotoesFiltro.Visible = true;
                updFiltro.Update();
            }

        }

        protected void btnAdicionarQuestionario_Click(object sender, EventArgs e)
        {
            try
            {
                if (UCComboQuestionario.Valor <= 0)
                    throw new ValidationException("Selecione um questionário.");

                if (Convert.ToInt32(ddlPergunta.SelectedValue) <= 0)
                    throw new ValidationException("Selecione uma pergunta.");

                if (Convert.ToInt32(ddlResposta.SelectedValue) <= 0)
                    throw new ValidationException("Selecione uma resposta.");

                QuestionarioConteudoResposta qcr = new QuestionarioConteudoResposta
                {
                    qst_id = UCComboQuestionario.Valor
                    ,
                    qtc_id = Convert.ToInt32(ddlPergunta.SelectedValue)
                    ,
                    qtr_id = Convert.ToInt32(ddlResposta.SelectedValue)
                    ,
                    qst_titulo = UCComboQuestionario.Combo.SelectedItem.Text
                    ,
                    qtc_texto = ddlPergunta.SelectedItem.Text
                    ,
                    qtr_texto = ddlResposta.SelectedItem.Text
                    ,
                    IsNew = true
                };

                if (VS_listQuestionarioConteudoResposta.Any(r => r.qtr_id == Convert.ToInt32(ddlResposta.SelectedValue)))
                    throw new ValidationException("Este filtro personalizado já foi adicionado.");

                VS_listQuestionarioConteudoResposta.Add(qcr);

                gvQuestionario.DataSource = VS_listQuestionarioConteudoResposta.OrderBy(f => f.qst_titulo).ThenBy(f => f.qtc_texto).ThenBy(f => f.qtr_texto);
                gvQuestionario.DataBind();

                UCComboQuestionario.Valor = -1;
                UCComboQuestionario_SelectedIndexChanged();

            }
            catch (ValidationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.ErroAdicionarQuestionario").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void gvQuestionario_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnExcluir = (ImageButton)e.Row.FindControl("btnExcluir");
                if (btnExcluir != null)
                {
                    btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
                    btnExcluir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                }
            }
        }

        protected void gvQuestionario_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Excluir")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());

                    int idExcluir = Convert.ToInt32(gvQuestionario.DataKeys[index]["qtr_id"]);

                    if (idExcluir > 0 && VS_listQuestionarioConteudoResposta.Any(r => r.qtr_id == idExcluir))
                    {
                        int ind = VS_listQuestionarioConteudoResposta.IndexOf(VS_listQuestionarioConteudoResposta.Where(r => r.qtr_id == idExcluir).First());
                        VS_listQuestionarioConteudoResposta.RemoveAt(ind);
                    }

                    gvQuestionario.DataSource = VS_listQuestionarioConteudoResposta.OrderBy(f => f.qst_titulo).ThenBy(f => f.qtc_texto).ThenBy(f => f.qtr_texto);
                    gvQuestionario.DataBind();

                }
                catch (ValidationException ex)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.ErroCarregarRelatorio").ToString(), UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void gvFiltroFixo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnExcluir = (ImageButton)e.Row.FindControl("btnExcluir");
                if (btnExcluir != null)
                {
                    btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
                    btnExcluir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                }
            }
        }

        protected void gvFiltroFixo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Excluir")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());

                    int idExcluir = Convert.ToInt32(gvFiltroFixo.DataKeys[index]["gff_tipoFiltro"]);

                    if (idExcluir > 0 && VS_lstFiltrosFixos.Any(f => f.gff_tipoFiltro == idExcluir))
                    {
                        int ind = VS_lstFiltrosFixos.IndexOf(VS_lstFiltrosFixos.Where(f => f.gff_tipoFiltro == idExcluir).First());
                        VS_lstFiltrosFixos.RemoveAt(ind);
                    }

                    gvFiltroFixo.DataSource = VS_lstFiltrosFixos.OrderBy(f => f.gff_tituloFiltro);
                    gvFiltroFixo.DataBind();

                }
                catch (ValidationException ex)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.ErroCarregarRelatorio").ToString(), UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void btnAdicionarFiltro_Click(object sender, EventArgs e)
        {
            try
            {
                ValidaCamposFiltroFixo(Convert.ToByte(ddlFiltroFixo.SelectedValue));

                if (VS_lstFiltrosFixos.Any(p => p.gff_tipoFiltro == Convert.ToByte(ddlFiltroFixo.SelectedValue)))
                    throw new ValidationException(string.Format("Este tipo de filtro já existe."));

                REL_GraficoAtendimento_FiltrosFixos gff = new REL_GraficoAtendimento_FiltrosFixos
                {
                    gra_id = VS_gra_id,
                    gff_tipoFiltro = Convert.ToByte(ddlFiltroFixo.SelectedValue),
                    gff_valorFiltro = RetornaValorFiltroFixo(Convert.ToByte(ddlFiltroFixo.SelectedValue)),
                    IsNew = true
                };

                gff.gff_tituloFiltro = REL_GraficoAtendimento_FiltrosFixosBO.RetornaTituloFiltro(gff.gff_tipoFiltro);
                gff.gff_valorDetalhado = REL_GraficoAtendimento_FiltrosFixosBO.RetornaValorDetalhado((REL_GraficoAtendimentoFiltrosFixos)gff.gff_tipoFiltro, gff.gff_valorFiltro);

                VS_lstFiltrosFixos.Add(gff);

                VS_lstFiltrosFixos = VS_lstFiltrosFixos.OrderBy(q => q.gff_tipoFiltro).ToList();

                gvFiltroFixo.DataSource = VS_lstFiltrosFixos.OrderBy(f => f.gff_tituloFiltro);
                gvFiltroFixo.DataBind();

                divBotoesFiltro.Visible = divRacaCor.Visible = divSexo.Visible = divIdade.Visible = divDataPreenchimento.Visible = divDetalhamentoDeficiencia.Visible = false;
                ddlFiltroFixo.SelectedIndex = 0;
                ddlFiltroFixo_SelectedIndexChanged(null, null);
                updFiltro.Update();
            }
            catch (ValidationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao adicionar filtro fixo.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnCancelarFiltro_Click(object sender, EventArgs e)
        {
            divBotoesFiltro.Visible = divRacaCor.Visible = divSexo.Visible = divIdade.Visible = divDataPreenchimento.Visible = divDetalhamentoDeficiencia.Visible = false;
            updFiltro.Update();
        }

        protected void ddlPergunta_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(ddlPergunta.SelectedValue) > 0)
                {
                    ddlResposta.Items.Clear();
                    ddlResposta.DataSource = CLS_QuestionarioRespostaBO.SelectByConteudo(Convert.ToInt32(ddlPergunta.SelectedValue));
                    ddlResposta.DataTextField = "qtr_texto";
                    ddlResposta.DataValueField = "qtr_id";
                    ddlResposta.Items.Insert(0, new ListItem("-- Selecione uma resposta --", "-1", true));
                    ddlResposta.AppendDataBoundItems = true;
                    ddlResposta.DataBind();
                    ddlResposta.Enabled = true;
                }
                else
                {
                    ddlResposta.Enabled = false;
                }
                ddlResposta.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao carregar perguntas.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion

    }
}