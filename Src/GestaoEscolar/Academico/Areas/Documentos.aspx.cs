namespace GestaoEscolar.Academico.Areas
{
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using MSTech.CoreSSO.BLL;
    using MSTech.GestaoEscolar.BLL;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.Web.WebProject;
    using System.Linq;
    using MSTech.Validation.Exceptions;

    public partial class Documentos : MotherPageLogado
    {
        #region Constantes

        private string nameSpaceResource = "Academico";
        private string chaveResource = "Areas.Documentos.{0}";

        #endregion Constantes

        #region Propriedades

        /// <summary>
        /// ViewState que armazena o valor de tad_id
        /// </summary>
        private int VS_tad_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_tad_id"] ?? -1);
            }
            set
            {
                ViewState["VS_tad_id"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena a flag que indica se o documento permite cadastro por escola.
        /// </summary>
        private bool VS_permiteCadastroEscola
        {
            get
            {
                return Convert.ToBoolean(ViewState["VS_permiteCadastroEscola"] ?? false);
            }

            set
            {
                ViewState["VS_permiteCadastroEscola"] = value;
            }
        }

        /// <summary>
        /// Lista inicial de arquivos do documento.
        /// </summary>
        private List<ACA_ArquivoArea> VS_listaArquivoArea
        {
            get
            {
                return (List<ACA_ArquivoArea>)(ViewState["VS_listaArquivoArea"] ?? (ViewState["VS_listaArquivoArea"] = new List<ACA_ArquivoArea>()));
            }

            set
            {
                ViewState["VS_listaArquivoArea"] = value;
            }
        }

        /// <summary>
        /// Escola selecionada no combo de escolas.
        /// </summary>
        private int esc_id
        {
            get
            {
                return UCComboUAEscola.Esc_ID;
            }
        }

        /// <summary>
        /// Unidade de escola selecionada no combo de escolas.
        /// </summary>
        private int uni_id
        {
            get
            {
                return UCComboUAEscola.Uni_ID;
            }
        }

        /// <summary>
        /// Unidade administrativa superior da escola selecionada no combo de escolas.
        /// </summary>
        private Guid uad_idSuperior
        {
            get
            {
                return UCComboUAEscola.VisibleUA ?
                           UCComboUAEscola.Uad_ID :
                           ESC_UnidadeEscolaBO.SelecionaUnidadeAdministrativaSuperior(esc_id, uni_id);
            }
        }

        /// <summary>
        /// ViewState que armazena o indice do documento em que o arquivo será salvo.
        /// </summary>
        private int VS_indiceArquivo
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_indiceArquivo"] ?? rptDocumentos.Items.Count - 1);
            }

            set
            {
                ViewState["VS_indiceArquivo"] = value;
            }
        }

        #endregion Propriedades

        #region Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsArquivoArea.js"));
            }

            // Seta o metodo criado no delegate ao evento do componente
            UCComboUAEscola.IndexChangedUnidadeEscola += UCComboUAEscola_IndexChangedUnidadeEscola;
            UCComboUAEscola.IndexChangedUA += UCComboUAEscola_IndexChangedUA;

            if (!IsPostBack)
            {
                try
                {
                    if (PreviousPage != null && PreviousPage.IsCrossPagePostBack)
                    {
                        VS_tad_id = PreviousPage.Edit_tad_id;
                        CarregarTelaInicial();
                    }
                    else
                    {
                         AdicionarLinhaRepeater();
                        Page.Form.DefaultFocus = UCComboUAEscola.VisibleUA ? UCComboUAEscola.ComboUA_ClientID : UCComboUAEscola.ComboEscola_ClientID;
                    }
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMensagem.Text = UtilBO.GetErroMessage(RetornaResource("ErroCarregar"), UtilBO.TipoMensagem.Erro);
                }
            }

        }

        #endregion Page Life Cycle

        #region Delegates

        /// <summary>
        /// Verifica alteracao do index do combo UA Escola e trata o combo Escola
        /// </summary>
        protected void UCComboUAEscola_IndexChangedUA()
        {
            UCComboUAEscola.MostraApenasAtivas = true;

            UCComboUAEscola.SelectedValueEscolas = new[] { -1, -1 };

            UCComboUAEscola.EnableEscolas = UCComboUAEscola.Uad_ID != Guid.Empty;

            if (VS_tad_id > 0)
            {
                try
                {
                    LimparRepeater();
                    Carregar();
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMensagem.Text = UtilBO.GetErroMessage(RetornaResource("ErroCarregar"), UtilBO.TipoMensagem.Erro);
                }
            }
        }

        /// <summary>
        /// Verifica alteracao do index do combo Escola
        /// </summary>
        protected void UCComboUAEscola_IndexChangedUnidadeEscola()
        {
            if (VS_tad_id > 0)
            {
                try
                {
                    LimparRepeater();
                    Carregar();
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMensagem.Text = UtilBO.GetErroMessage(RetornaResource("ErroCarregar"), UtilBO.TipoMensagem.Erro);
                }
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// O método retorna o valor de um resource.
        /// </summary>
        /// <param name="chave"></param>
        /// <returns></returns>
        private string RetornaResource(string chave)
        {
            return GetGlobalResourceObject(nameSpaceResource, String.Format(chaveResource, chave)).ToString();
        }

        /// <summary>
        /// Carrega combos e textos iniciais da tela.
        /// </summary>
        private void CarregarTelaInicial()
        {
            UCComboUAEscola.ObrigatorioUA =
            UCComboUAEscola.ObrigatorioEscola =
            UCComboUAEscola.Visible =
            VS_permiteCadastroEscola = ACA_TipoAreaDocumentoBO.GetCadastroEscolaBy_tad_id(VS_tad_id);

            VS_listaArquivoArea = ACA_ArquivoAreaBO.GetSelectBy_tad_id(VS_tad_id);
            VS_listaArquivoArea.ForEach(p => p.id = Guid.NewGuid());

            if (VS_permiteCadastroEscola)
            {
                UCComboUAEscola.Inicializar();

                Dictionary<Guid, List<ACA_ArquivoArea>> dicArquivoUad = (from arquivo in VS_listaArquivoArea.Where(p => p.esc_id > 0 && p.uni_id > 0 && p.uad_idSuperior != Guid.Empty)
                                                                         group arquivo by arquivo.uad_idSuperior into grupo
                                                                         select new
                                                                         {
                                                                             chave = grupo.Key
                                                                             ,
                                                                             valor = grupo.ToList()
                                                                         }).ToDictionary(p => p.chave, p => p.valor);

                if (UCComboUAEscola.VisibleUA)
                {
                    List<Guid> listaUad = UCComboUAEscola.DdlUA.Items.Cast<ListItem>().Select(p => new Guid(p.Value)).ToList();
                    dicArquivoUad = (from dic in dicArquivoUad
                                     join uad in listaUad on dic.Key equals uad
                                     select dic).ToDictionary(p => p.Key, p => p.Value);

                    if (dicArquivoUad.Any())
                    {
                        UCComboUAEscola.Uad_ID = UCComboUAEscola.Uad_ID == Guid.Empty ? dicArquivoUad.First().Key : UCComboUAEscola.Uad_ID;
                    }
                }

                var listaEscola = UCComboUAEscola.DdlEscola.Items.Cast<ListItem>().Select(p => new { esc_id = Convert.ToInt32(p.Value.Split(';')[0]), uni_id = Convert.ToInt32(p.Value.Split(';')[1]) });
                List<ACA_ArquivoArea> listaArquivoArea = (from arquivo in dicArquivoUad.Where(p => p.Key == uad_idSuperior || (uad_idSuperior == Guid.Empty && !UCComboUAEscola.VisibleUA)).SelectMany(p => p.Value)
                                                          join esc in listaEscola on new { esc_id = arquivo.esc_id, uni_id = arquivo.uni_id } equals new { esc.esc_id, esc.uni_id }
                                                          select arquivo).ToList();

                if (listaArquivoArea.Any())
                {
                    UCComboUAEscola.SelectedValueEscolas = UCComboUAEscola.Esc_ID > 0 ? new int[] { UCComboUAEscola.Esc_ID, UCComboUAEscola.Uni_ID } : new int[] { listaArquivoArea.First().esc_id, listaArquivoArea.First().uni_id };
                }
            }

            UCComboUAEscola.EnableEscolas = UCComboUAEscola.Uad_ID != Guid.Empty || !UCComboUAEscola.VisibleUA;

            Carregar();
        }

        /// <summary>
        /// O método carrega os arquivos por escola.
        /// </summary>
        private void Carregar()
        {
            rptDocumentos.DataSource = VS_permiteCadastroEscola ?
                VS_listaArquivoArea.Where(p => p.esc_id == esc_id && p.uni_id == uni_id) :
                VS_listaArquivoArea;
            rptDocumentos.DataBind();
            

            if (rptDocumentos.Items.Count == 0)
            {
                AdicionarLinhaRepeater();
            }
            else
            {
                rptDocumentos.Items[rptDocumentos.Items.Count - 1].FindControl("btnAdicionar").Visible = true;
            }

            divDocumentos.Visible = (!VS_permiteCadastroEscola || (VS_permiteCadastroEscola && esc_id > 0));
        }

        /// <summary>
        /// Limpa os dados do repeater de arquivos.
        /// </summary>
        private void LimparRepeater()
        {
            lblMensagemDocumentos.Text = "";

            rptDocumentos.DataSource = new List<ACA_ArquivoArea>();
            rptDocumentos.DataBind();
        }

        /// <summary>
        /// Método que adiciona uma linha ao repeater de documentos.
        /// </summary>
        private void AdicionarLinhaRepeater()
        {
            bool valido = true;

            if (rptDocumentos.Items.Count > 0)
            {
                RadioButtonList rblLinkArquivo = (RadioButtonList)rptDocumentos.Items[rptDocumentos.Items.Count - 1].FindControl("rblLinkArquivo");
                TextBox txtDescricao = (TextBox)rptDocumentos.Items[rptDocumentos.Items.Count - 1].FindControl("txtDescricao");
                Label lblErroDescricao = (Label)rptDocumentos.Items[rptDocumentos.Items.Count - 1].FindControl("lblErroDescricao");
                Label lblArqId = (Label)rptDocumentos.Items[rptDocumentos.Items.Count - 1].FindControl("lblArqId");
                TextBox txtLink = (TextBox)rptDocumentos.Items[rptDocumentos.Items.Count - 1].FindControl("txtLink");
                Label lblErroLink = (Label)rptDocumentos.Items[rptDocumentos.Items.Count - 1].FindControl("lblErroLink");
                WebControls_Combos_UCComboNivelEnsino cmbTipoNivelEnsino = (WebControls_Combos_UCComboNivelEnsino)rptDocumentos.Items[rptDocumentos.Items.Count - 1].FindControl("UCComboTipoNivelEnsino");

                if (lblErroDescricao != null)
                    lblErroDescricao.Visible = false;
                if (lblErroLink != null)
                    lblErroLink.Visible = false;

                string msg = "";

                if (rblLinkArquivo != null && txtDescricao != null)
                {
                    long arq_id = 0;

                    if (string.IsNullOrEmpty(txtDescricao.Text))
                    {
                        msg = RetornaResource("DescricaoObrigatorio");
                        if (lblErroDescricao != null)
                            lblErroDescricao.Visible = true;
                        valido = false;
                    }

                    if (rblLinkArquivo.SelectedIndex == -1)
                    {
                        msg += (!string.IsNullOrEmpty(msg) ? "<br/>" : "") + RetornaResource("TipoObrigatorio");
                        valido = false;
                    }

                    if (!string.IsNullOrEmpty(txtDescricao.Text) && rblLinkArquivo.SelectedIndex > -1 && (Convert.ToInt32(rblLinkArquivo.SelectedValue) == (byte)ACA_ArquivoAreaBO.eTipoDocumento.Arquivo && (!Int64.TryParse(lblArqId.Text, out arq_id) || arq_id <= 0)))
                    {
                        msg += (!string.IsNullOrEmpty(msg) ? "<br/>" : "") + RetornaResource("ArquivoObrigatorio");
                        if (lblErroLink != null)
                            lblErroLink.Visible = true;
                        valido = false;
                    }

                    if (!string.IsNullOrEmpty(txtDescricao.Text) && rblLinkArquivo.SelectedIndex > -1 && (Convert.ToInt32(rblLinkArquivo.SelectedValue) == (byte)ACA_ArquivoAreaBO.eTipoDocumento.Link && string.IsNullOrEmpty(txtLink.Text)))
                    {
                        msg += (!string.IsNullOrEmpty(msg) ? "<br/>" : "") + RetornaResource("LinkObrigatorio");
                        if (lblErroLink != null)
                            lblErroLink.Visible = true;
                        valido = false;
                    }

                    if (!string.IsNullOrEmpty(msg))
                        lblMensagemDocumentos.Text = UtilBO.GetErroMessage(msg, UtilBO.TipoMensagem.Alerta);

                    /*if (cmbTipoNivelEnsino.Valor == -1)
                    {
                        //lblMensagemDocumentos.Text = UtilBO.GetErroMessage(RetornaResource(""), UtilBO.TipoMensagem.Alerta);
                        valido = false;
                    }*/
                }
            }

            if (valido)
            {
                lblMensagemDocumentos.Text = "";
                List<ACA_ArquivoArea> listaArquivoArea = RetornaListaDocumentos();

                listaArquivoArea.Add
                (
                    new ACA_ArquivoArea
                    {
                        tad_id = VS_tad_id
                        ,
                        uad_idSuperior = VS_permiteCadastroEscola ? uad_idSuperior : Guid.Empty
                        ,
                        esc_id = VS_permiteCadastroEscola ? esc_id : -1
                        ,
                        uni_id = VS_permiteCadastroEscola ? uni_id : -1                        
                        ,
                        id = Guid.NewGuid()
                        ,
                        aar_tipoDocumento = (byte)ACA_ArquivoAreaBO.eTipoDocumento.Link
                    }
                );

                rptDocumentos.DataSource = listaArquivoArea;
                rptDocumentos.DataBind();

                rptDocumentos.Items[rptDocumentos.Items.Count - 1].FindControl("btnAdicionar").Visible = true;
            }
        }

        /// <summary>
        /// Remove uma linha do repeater
        /// </summary>
        /// <param name="aar_id"></param>
        private void RemoverLinhaRepeater(Guid id)
        {
            if (rptDocumentos.Items.Count > 0)
            {
                List<ACA_ArquivoArea> lista = RetornaListaDocumentos();

                lista.RemoveAll(p => p.id == id);

                if (!lista.Any())
                {
                    lista.Add
                    (
                        new ACA_ArquivoArea
                        {
                            tad_id = VS_tad_id
                            ,
                            uad_idSuperior = VS_permiteCadastroEscola ? uad_idSuperior : Guid.Empty
                            ,
                            esc_id = VS_permiteCadastroEscola ? esc_id : -1
                            ,
                            uni_id = VS_permiteCadastroEscola ? uni_id : -1
                            ,
                            id = Guid.NewGuid()
                            ,
                            aar_tipoDocumento = (byte)ACA_ArquivoAreaBO.eTipoDocumento.Link
                        }
                    );
                }

                rptDocumentos.DataSource = lista;
                rptDocumentos.DataBind();


                rptDocumentos.Items[rptDocumentos.Items.Count - 1].FindControl("btnAdicionar").Visible = true;
            }
        }

        /// <summary>
        /// Retorna a lista de arquivos do documento aprensentados no repeater.
        /// </summary>
        /// <returns></returns>
        private List<ACA_ArquivoArea> RetornaListaDocumentos()
        {
            return (from RepeaterItem ri in rptDocumentos.Items
                    let aar_id = Convert.ToInt32(((Label)ri.FindControl("lblAarId")).Text)
                    let aar_descricao = ((TextBox)ri.FindControl("txtDescricao")).Text
                    where (!string.IsNullOrEmpty(aar_descricao))
                    select new ACA_ArquivoArea
                    {
                        tad_id = Convert.ToInt32(((Label)ri.FindControl("lblTadId")).Text)
                        ,
                        aar_id = aar_id
                        ,
                        tne_id = ((WebControls_Combos_UCComboNivelEnsino)ri.FindControl("UCComboTipoNivelEnsino")).Valor
                        ,
                        aar_planoPoliticoPedagogico = ((CheckBox)ri.FindControl("chkPPP")).Checked
                        ,
                        uad_idSuperior = new Guid(((Label)ri.FindControl("lblUadIdSuperior")).Text)
                        ,
                        esc_id = Convert.ToInt32(((Label)ri.FindControl("lblEscId")).Text)
                        ,
                        uni_id = Convert.ToInt32(((Label)ri.FindControl("lblUniId")).Text)
                        ,
                        aar_descricao = aar_descricao
                        ,
                        aar_tipoDocumento = Convert.ToByte(((RadioButtonList)ri.FindControl("rblLinkArquivo")).SelectedValue)
                        ,
                        arq_id = Convert.ToInt64(((Label)ri.FindControl("lblArqId")).Text)
                        ,
                        aar_link = ((TextBox)ri.FindControl("txtLink")).Text
                        ,
                        id = new Guid(((Label)ri.FindControl("lblId")).Text)
                        ,
                        arq_nome = ((Label)ri.FindControl("lblArqNome")).Text
                        ,
                        IsNew = aar_id <= 0
                    }).ToList();
        }

        /// <summary>
        /// Metodo de Salvar documentos
        /// </summary>
        private void Salvar()
        {
            try
            {
                List<ACA_ArquivoArea> listDocumentos = RetornaListaDocumentos();

                if (ACA_ArquivoAreaBO.Salvar(VS_tad_id, uad_idSuperior, esc_id, listDocumentos))
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "tad_id: " + VS_tad_id);
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Documento(s) salvos(s) com sucesso", UtilBO.TipoMensagem.Sucesso);
                    RedirecionarPagina("~/Academico/Areas/Busca.aspx");
                }
                else
                {
                    lblMensagem.Text = UtilBO.GetErroMessage(RetornaResource("ErroSalvar"), UtilBO.TipoMensagem.Erro);
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage(RetornaResource("ErroSalvar"), UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion Métodos

        #region Eventos

        protected void rptDocumentos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ImageButton btnAdicionar = (ImageButton)e.Item.FindControl("btnAdicionar");
                if (btnAdicionar != null)
                {
                    btnAdicionar.CommandArgument = e.Item.ItemIndex.ToString();
                }

                ImageButton btnExcluir = (ImageButton)e.Item.FindControl("btnExcluir");
                if (btnExcluir != null)
                {
                    btnExcluir.CommandArgument = e.Item.ItemIndex.ToString();
                }

                HyperLink hplDocumento = (HyperLink)e.Item.FindControl("hplDocumento");
                RadioButtonList rblLinkArquivo = (RadioButtonList)e.Item.FindControl("rblLinkArquivo");

                Label lblErroLink = (Label)e.Item.FindControl("lblErroLink");
                if (lblErroLink != null)
                    lblErroLink.Visible = false;

                Label lblErroDescricao = (Label)e.Item.FindControl("lblErroDescricao");
                if (lblErroDescricao != null)
                    lblErroDescricao.Visible = false;

                byte aar_tipoDocumento = Convert.ToByte(DataBinder.Eval(e.Item.DataItem, "aar_tipoDocumento"));
                rblLinkArquivo.SelectedValue = aar_tipoDocumento.ToString();

                WebControls_Combos_UCComboNivelEnsino cmbTipoNivelEnsino = (WebControls_Combos_UCComboNivelEnsino)e.Item.FindControl("UCComboTipoNivelEnsino");
                int tne_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tne_id"));
                cmbTipoNivelEnsino.Valor = tne_id;

                CheckBox chkPPP = (CheckBox)e.Item.FindControl("chkPPP");
                chkPPP.Checked = (bool)((DataBinder.Eval(e.Item.DataItem, "aar_planoPoliticoPedagogico")));
                
                if (rblLinkArquivo.SelectedValue == ((byte)ACA_ArquivoAreaBO.eTipoDocumento.Arquivo).ToString())
                {
                    Button btnUpload = (Button)e.Item.FindControl("btnUpload");
                    long arq_id = Convert.ToInt64(DataBinder.Eval(e.Item.DataItem, "arq_id"));
                    string arq_nome = DataBinder.Eval(e.Item.DataItem, "arq_nome").ToString();

                    if (arq_id > 0)
                    {
                        hplDocumento.NavigateUrl = String.Format("~/FileHandler.ashx?file={0}", arq_id.ToString());
                        hplDocumento.Text = string.IsNullOrEmpty(arq_nome) ? "Download" : arq_nome;
                        hplDocumento.Visible = true;
                        btnUpload.Visible = true;
                    }
                    else
                    {
                        hplDocumento.Visible = false;
                        btnUpload.Visible = true;
                    }
                }
                else
                {
                    Button btnUpload = (Button)e.Item.FindControl("btnUpload");
                    btnUpload.Visible = false;

                    TextBox txtLink = (TextBox)e.Item.FindControl("txtLink");
                    if (!string.IsNullOrEmpty(txtLink.Text))
                    {
                        string httpLink = txtLink.Text;
                        if (!httpLink.Contains("://"))
                            txtLink.Text = "http://" + httpLink;
                        hplDocumento.NavigateUrl = txtLink.Text;
                        hplDocumento.Text = txtLink.Text;

                        hplDocumento.Target = "_blank";
                        hplDocumento.Visible = true;
                        txtLink.Visible = false;
                    }
                    else
                    {
                        hplDocumento.Visible = false;
                        txtLink.Visible = true;
                    }
                }
            }
        }

        protected void rptDocumentos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Excluir")
            {
                try
                {
                    int indice = Convert.ToInt32(e.CommandArgument);
                    RepeaterItem item = rptDocumentos.Items[indice];
                    Label lblId = (Label)item.FindControl("lblId");
                    RemoverLinhaRepeater(new Guid(lblId.Text));
                }
                catch (Exception ex)
                {
                    lblMensagem.Text = UtilBO.GetErroMessage(RetornaResource("ErroExcluir"), UtilBO.TipoMensagem.Erro);
                    ApplicationWEB._GravaErro(ex);
                }
            }

            if (e.CommandName == "Adicionar")
            {
                try
                {
                    bool ValidarArquivo = false;

                    RadioButtonList rblLinkArquivo = (RadioButtonList)rptDocumentos.Items[rptDocumentos.Items.Count - 1].FindControl("rblLinkArquivo");
                    TextBox txtDescricao = (TextBox)rptDocumentos.Items[rptDocumentos.Items.Count - 1].FindControl("txtDescricao");
                    Label lblErroDescricao = (Label)rptDocumentos.Items[rptDocumentos.Items.Count - 1].FindControl("lblErroDescricao");
                    Label lblErroLink = (Label)rptDocumentos.Items[rptDocumentos.Items.Count - 1].FindControl("lblErroLink");

                    if (lblErroDescricao != null)
                        lblErroDescricao.Visible = false;
                    if (lblErroLink != null)
                        lblErroLink.Visible = false;

                    string msg = "";

                    if (string.IsNullOrEmpty(txtDescricao.Text))
                    {
                        ValidarArquivo = true;
                        msg = RetornaResource("DescricaoObrigatorio");
                        if (lblErroDescricao != null)
                            lblErroDescricao.Visible = true;
                    }

                    if (rblLinkArquivo.SelectedIndex == -1)
                    {
                        ValidarArquivo = true;
                        msg += (!string.IsNullOrEmpty(msg) ? "<br/>" : "") + RetornaResource("TipoObrigatorio");
                    }

                    if (rblLinkArquivo.SelectedValue == ((byte)ACA_ArquivoAreaBO.eTipoDocumento.Arquivo).ToString())
                    {
                        Label lblArqId = (Label)rptDocumentos.Items[rptDocumentos.Items.Count - 1].FindControl("lblArqId");
                        long arq_id = 0;
                        if (!Int64.TryParse(lblArqId.Text, out arq_id) || arq_id <= 0)
                        {
                            ValidarArquivo = true;
                            msg += (!string.IsNullOrEmpty(msg) ? "<br/>" : "") + RetornaResource("ArquivoObrigatorio");
                            if (lblErroLink != null)
                                lblErroLink.Visible = true;
                        }
                    }
                    else
                    {
                        TextBox txtLink = (TextBox)rptDocumentos.Items[rptDocumentos.Items.Count - 1].FindControl("txtLink");
                        if (string.IsNullOrEmpty(txtLink.Text))
                        {
                            ValidarArquivo = true;
                            msg += (!string.IsNullOrEmpty(msg) ? "<br/>" : "") + RetornaResource("LinkObrigatorio");
                            if (lblErroLink != null)
                                lblErroLink.Visible = true;
                        }
                    }

                    if (!string.IsNullOrEmpty(msg))
                        lblMensagemDocumentos.Text = UtilBO.GetErroMessage(msg, UtilBO.TipoMensagem.Alerta);

                    if (!ValidarArquivo)
                    {
                        AdicionarLinhaRepeater();
                    }
                }
                catch (Exception ex)
                {
                    lblMensagem.Text = UtilBO.GetErroMessage(RetornaResource("ErroAdicionarNovo"), UtilBO.TipoMensagem.Erro);
                    ApplicationWEB._GravaErro(ex);
                }
            }

            if (e.CommandName == "Upload")
            {
                try
                {
                    VS_indiceArquivo = e.Item.ItemIndex;
                    //Abre o popup
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Upload", "$('#divUpload').dialog('open');", true);
                    fupArquivo.Focus();
                }
                catch (ValidationException ex)
                {
                    lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMensagem.Text = UtilBO.GetErroMessage(RetornaResource("ErroAdicionarArquivo"), UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void rblLinkArquivo_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioButtonList rb = (RadioButtonList)sender;
            RepeaterItem item = rb.Parent as RepeaterItem;
            RadioButtonList rblLinkArquivo = (RadioButtonList)item.FindControl("rblLinkArquivo");

            if (rblLinkArquivo != null)
            {
                HyperLink hplDocumento = (HyperLink)item.FindControl("hplDocumento");
                if (rblLinkArquivo.SelectedValue == ((byte)ACA_ArquivoAreaBO.eTipoDocumento.Arquivo).ToString())
                {
                    Button btnUpload = (Button)item.FindControl("btnUpload");
                    Label lblArqId = (Label)item.FindControl("lblArqId");
                    Label lblArqNome = (Label)item.FindControl("lblArqNome");
                    long arq_id;

                    TextBox txtLink = (TextBox)item.FindControl("txtLink");
                    txtLink.Visible = false;

                    if (Int64.TryParse(lblArqId.Text, out arq_id) && arq_id > 0)
                    {
                        hplDocumento.NavigateUrl = String.Format("~/FileHandler.ashx?file={0}", arq_id.ToString());
                        hplDocumento.Text = string.IsNullOrEmpty(lblArqNome.Text) ? "Download" : lblArqNome.Text;
                        hplDocumento.Visible = true;
                        btnUpload.Visible = true;
                    }
                    else
                    {
                        hplDocumento.Visible = false;
                        btnUpload.Visible = true;
                    }

                }
                else
                {
                    Button btnUpload = (Button)item.FindControl("btnUpload");
                    btnUpload.Visible = false;

                    TextBox txtLink = (TextBox)item.FindControl("txtLink");
                    if (!string.IsNullOrEmpty(txtLink.Text))
                    {
                        string httpLink = txtLink.Text;
                        if (!httpLink.Contains("://"))
                            txtLink.Text = "http://" + httpLink;
                        hplDocumento.NavigateUrl = txtLink.Text;
                        hplDocumento.Text = txtLink.Text;

                        hplDocumento.Target = "_blank";
                        hplDocumento.Visible = true;
                        txtLink.Visible = false;
                    }
                    else
                    {
                        hplDocumento.Visible = false;
                        txtLink.Visible = true;
                    }

                }
            }
        }

        protected void btnAdicionarArquivo_Click(object sender, EventArgs e)
        {
            try
            {
                if (!fupArquivo.HasFile)
                {
                    throw new ValidationException("É obrigatório selecionar um arquivo.");
                }
                SYS_Arquivo entArquivo = SYS_ArquivoBO.CriarAnexo(fupArquivo.PostedFile);
                entArquivo.arq_situacao = (byte)SYS_ArquivoSituacao.Temporario;
                SYS_ArquivoBO.Save(entArquivo, ApplicationWEB.TamanhoMaximoArquivo, ApplicationWEB.TiposArquivosPermitidos);
                entArquivo.IsNew = false;

                Label lblArqId = (Label)rptDocumentos.Items[VS_indiceArquivo].FindControl("lblArqId");
                lblArqId.Text = entArquivo.arq_id.ToString();

                Label lblArqNome = (Label)rptDocumentos.Items[VS_indiceArquivo].FindControl("lblArqNome");
                lblArqNome.Text = entArquivo.arq_nome;

                HyperLink hplDocumento = (HyperLink)rptDocumentos.Items[VS_indiceArquivo].FindControl("hplDocumento");

                hplDocumento.NavigateUrl = String.Format("~/FileHandler.ashx?file={0}", lblArqId.Text);
                hplDocumento.Text = entArquivo.arq_nome;
                hplDocumento.Visible = true;
            }
            catch (ValidationException ex)
            {
                lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage(RetornaResource("ErroCarregar"), UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            bool valido = true;
            long arq_id = 0;
            foreach (RepeaterItem item in rptDocumentos.Items)
            {
                RadioButtonList rblLinkArquivo = (RadioButtonList)item.FindControl("rblLinkArquivo");
                TextBox txtDescricao = (TextBox)item.FindControl("txtDescricao");
                Label lblArqId = (Label)item.FindControl("lblArqId");
                TextBox txtLink = (TextBox)item.FindControl("txtLink");

                if (string.IsNullOrEmpty(txtDescricao.Text)
                    &&
                    (string.IsNullOrEmpty(txtLink.Text))
                    &&
                    (!Int64.TryParse(lblArqId.Text, out arq_id) || arq_id <= 0)
                    &&
                    VS_listaArquivoArea.Count > 0
                    &&
                    rptDocumentos.Items.Count == 1
                    )
                { // na situação de alteração não validar o último registro que foi excluído do repeater(item em branco default)
                    continue;
                }

                if (string.IsNullOrEmpty(txtDescricao.Text))
                {
                    lblMensagemDocumentos.Text = UtilBO.GetErroMessage(RetornaResource("DescricaoObrigatorio"), UtilBO.TipoMensagem.Alerta);
                    valido = false;
                }
                if (rblLinkArquivo.SelectedIndex == -1)
                {
                    lblMensagemDocumentos.Text = UtilBO.GetErroMessage(RetornaResource("TipoObrigatorio"), UtilBO.TipoMensagem.Alerta);
                    valido = false;
                }

                if (!string.IsNullOrEmpty(txtDescricao.Text) && rblLinkArquivo.SelectedIndex > -1 &&
                    (Convert.ToByte(rblLinkArquivo.SelectedValue) == (byte)ACA_ArquivoAreaBO.eTipoDocumento.Arquivo &&
                    (!Int64.TryParse(lblArqId.Text, out arq_id) || arq_id <= 0)))
                {
                    lblMensagemDocumentos.Text = UtilBO.GetErroMessage(RetornaResource("ArquivoObrigatorio"), UtilBO.TipoMensagem.Alerta);
                    valido = false;
                }

                if (!string.IsNullOrEmpty(txtDescricao.Text) && rblLinkArquivo.SelectedIndex > -1 && (Convert.ToByte(rblLinkArquivo.SelectedValue) == (byte)ACA_ArquivoAreaBO.eTipoDocumento.Link && string.IsNullOrEmpty(txtLink.Text)))
                {
                    lblMensagemDocumentos.Text = UtilBO.GetErroMessage(RetornaResource("LinkObrigatorio"), UtilBO.TipoMensagem.Alerta);
                    valido = false;
                }
            }

            if (Page.IsValid && valido)
                Salvar();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            RedirecionarPagina("~/Academico/Areas/Busca.aspx");
        }

        #endregion Eventos
    }
}