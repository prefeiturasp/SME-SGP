using System;
using System.Data;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.CoreSSO.BLL;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;

public partial class Configuracao_HistoricoObservacaoPadrao_Cadastro : MotherPageLogado
{
    #region Propriedades

    /// <summary>
    /// Propriedade em ViewState que armazena valor de hip_id (ID do histórico de observação padrão)
    /// no caso de atualização de um registro ja existente.
    /// </summary>
    private int _VS_hip_id
    {
        get
        {
            if (ViewState["_VS_hip_id"] != null)
                return Convert.ToInt32(ViewState["_VS_hip_id"]);
            return -1;
        }
        set
        {
            ViewState["_VS_hip_id"] = value;
        }
    }

    #endregion

    #region Métodos

    /// <summary>
    /// Insere e altera Observações no histórico escolar.
    /// </summary>
    private void _Salvar()
    {
        try
        {
            ACA_HistoricoObservacaoPadrao _HistoricoObservacaoPadrao = new ACA_HistoricoObservacaoPadrao
            {
                hop_id = _VS_hip_id
                ,
                hop_nome = _txtNomeObservacao.Text
                ,
                hop_tipo = Convert.ToByte(_ddlTipoObservacao.SelectedValue)
                ,
                hop_descricao = string.IsNullOrEmpty(_txtDescricaoObservacao.Text) ? txtDescricaoObservacaoHTML.Text.Replace("<p>", string.Empty).Replace("</p>", string.Empty) : _txtDescricaoObservacao.Text
                ,
                hop_situacao = 1
                ,
                IsNew = (_VS_hip_id > 0) ? false : true
            };
            if (ACA_HistoricoObservacaoPadraoBO.Salvar(_HistoricoObservacaoPadrao))
            {
                if (_VS_hip_id <= 0)
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "hop_id: " + _HistoricoObservacaoPadrao.hop_id);
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Observação do histórico escolar incluída com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
                else
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "hop_id: " + _HistoricoObservacaoPadrao.hop_id);
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Observação do histórico escolar alterada com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }

                Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Configuracao/HistoricoObservacaoPadrao/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            else
            {
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar a observação do histórico escolar.", UtilBO.TipoMensagem.Erro);
            }
        }
        catch (MSTech.Validation.Exceptions.ValidationException e)
        {
            _lblMessage.Text = UtilBO.GetErroMessage(e.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (DuplicateNameException e)
        {
            _lblMessage.Text = UtilBO.GetErroMessage(e.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (ArgumentException e)
        {
            _lblMessage.Text = UtilBO.GetErroMessage(e.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (Exception e)
        {
            ApplicationWEB._GravaErro(e);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar a observação do histórico escolar.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Carrega os dados da observações no histórico escolar nos controles caso seja alteração.
    /// </summary>
    /// <param name="hop_id"></param>
    private void _Carregar(int hop_id)
    {
        try
        {
            ACA_HistoricoObservacaoPadrao _HistoricoObservacaoPadrao = new ACA_HistoricoObservacaoPadrao { hop_id = hop_id };
            ACA_HistoricoObservacaoPadraoBO.GetEntity(_HistoricoObservacaoPadrao);
            _VS_hip_id = _HistoricoObservacaoPadrao.hop_id;
            _ddlTipoObservacao.Enabled = false;
            if (_ddlTipoObservacao.Items.FindByValue(_HistoricoObservacaoPadrao.hop_tipo.ToString()) != null)
                _ddlTipoObservacao.SelectedValue = _HistoricoObservacaoPadrao.hop_tipo.ToString();
            _txtNomeObservacao.Text = _HistoricoObservacaoPadrao.hop_nome;
            if (txtDescricaoObservacaoHTML.Visible)
                txtDescricaoObservacaoHTML.Text = _HistoricoObservacaoPadrao.hop_descricao;
            else
                _txtDescricaoObservacao.Text = _HistoricoObservacaoPadrao.hop_descricao;

            divCamposAuxiliares.Visible = Convert.ToByte(_ddlTipoObservacao.SelectedValue) == (byte)ACA_HistoricoObservacaoPadraoTipo.CertificadoConclusaoCurso;
        }
        catch (Exception e)
        {
            ApplicationWEB._GravaErro(e);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar a observação do histórico escolar.", UtilBO.TipoMensagem.Erro);
        }
    }

    #endregion

    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(this);
        if (sm != null)
            sm.Scripts.Add(new ScriptReference("~/Includes/ckeditor/ckeditor.js"));

        if (!IsPostBack)
        {
            _ddlTipoObservacao.Items.Insert(0, new ListItem("Critério de Avaliação", Convert.ToByte(ACA_HistoricoObservacaoPadraoTipo.CriterioAvaliacao).ToString(), true));
            _ddlTipoObservacao.Items.Insert(0, new ListItem("Observação", Convert.ToByte(ACA_HistoricoObservacaoPadraoTipo.Observacao).ToString(), true));
            _ddlTipoObservacao.Items.Insert(0, new ListItem("Observações Complementares", Convert.ToByte(ACA_HistoricoObservacaoPadraoTipo.ObservacaoComplementar).ToString(), true));
            _ddlTipoObservacao.Items.Insert(0, new ListItem("Certificado de Conclusão de Curso", Convert.ToByte(ACA_HistoricoObservacaoPadraoTipo.CertificadoConclusaoCurso).ToString(), true));
            _ddlTipoObservacao.Items.Insert(0, new ListItem("-- Selecione um tipo de observação --", "-1", true));
            _ddlTipoObservacao.DataBind();

            bool exibeEditorHTML = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_EDITOR_HTML_CADASTRO_OBSERVACOES, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            _txtDescricaoObservacao.Visible = !exibeEditorHTML;
            divEditorHTML.Visible = exibeEditorHTML;

            if (exibeEditorHTML)
            {

                #region Campos Auxiliares
                
                ddlCampoAuxiliar.Items.Insert(0, new ListItem("Número", "[Numero]"));
                ddlCampoAuxiliar.Items.Insert(0, new ListItem("Livro", "[Livro]"));
                ddlCampoAuxiliar.Items.Insert(0, new ListItem("Folha", "[Folha]"));
                ddlCampoAuxiliar.Items.Insert(0, new ListItem("Número GDAE", "[NumeroGDAE]"));
                ddlCampoAuxiliar.Items.Insert(0, new ListItem("Escola", "[Escola]"));
                ddlCampoAuxiliar.Items.Insert(0, new ListItem("Diretor da Unidade Escolar", "[Diretor]"));
                ddlCampoAuxiliar.Items.Insert(0, new ListItem("Nome do aluno", "[NomeAluno]"));
                ddlCampoAuxiliar.Items.Insert(0, new ListItem("Ano letivo", "[AnoLetivo]"));
                ddlCampoAuxiliar.Items.Insert(0, new ListItem("-- Selecione um campo auxiliar--", "-1", true));
                ddlCampoAuxiliar.DataBind();

                #endregion Campos Auxiliares

                #region Config CKEditor

                string script = string.Empty;

                script = "CKEDITOR.instances." + txtDescricaoObservacaoHTML.ClientID + ".insertText("
                     + ddlCampoAuxiliar.ClientID + ".options[" + ddlCampoAuxiliar.ClientID +
                     ".selectedIndex].value); return false;";

                btnCamposAuxiliares.OnClientClick = script;

                txtDescricaoObservacaoHTML.config.toolbar = new object[]
                                        {
                                            new object[]
                                                {
                                                    "Cut", "Copy", "-", "Paste", "PasteText", "-", "Undo",
                                                    "Redo", "-", "Find", "Replace", "-", "SelectAll", "RemoveFormat", "-",
                                                },
                                            new object[]
                                                {
                                                    "Bold", "Italic", "Underline", "Strike"
                                                },
                                            new object[]
                                                {
                                                    "Outdent", "Indent", "-", "JustifyLeft", "JustifyCenter",
                                                    "JustifyRight", "JustifyBlock", "-", "Preview", "-", "About"
                                                },
                                        };

                #endregion Config CKeditor
            }

            if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                _Carregar(PreviousPage.EditItem);
            else
            {
                _btnSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
            }

            Page.Form.DefaultFocus = _ddlTipoObservacao.ClientID;
            Page.Form.DefaultButton = _btnSalvar.UniqueID;
        }
    }

    protected void _btnCancelar_Click(object sender, EventArgs e)
    {
        Response.Redirect("Busca.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    protected void _btnSalvar_Click(object sender, EventArgs e)
    {
        if(Page.IsValid)
            _Salvar();
    }

    protected void _ddlTipoObservacao_SelectedIndexChanged(object sender, EventArgs e)
    {
        divCamposAuxiliares.Visible = Convert.ToByte(_ddlTipoObservacao.SelectedValue) == (byte)ACA_HistoricoObservacaoPadraoTipo.CertificadoConclusaoCurso;
    }

    #endregion
}
